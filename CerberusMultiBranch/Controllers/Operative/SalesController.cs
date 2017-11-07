using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Operative;
using Microsoft.AspNet.Identity;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity.Owin;
using CerberusMultiBranch.Models.ViewModels.Operative;
using CerberusMultiBranch.Models.Entities.Config;
using System.Net.Http;
using CerberusMultiBranch.Models.Entities.Catalog;

namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize]
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Supervisor")]
        public ActionResult History()
        {
            //obtengo las sucursales configuradas para el empleado
            var branches = User.Identity.GetBranches();

            TransactionViewModel model = new TransactionViewModel();
            model.Branches = branches.ToSelectList();
            model.Sales = LookFor(null, DateTime.Today, null, null, null, null, null, null);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public JsonResult Cancel(int transactionId, string comment)
        {
            try
            {
                //busco la venta a cancelar
                var sale = db.Sales.Include(s => s.TransactionDetails).
                    FirstOrDefault(s => s.TransactionId == transactionId);

                //regreso los productos al stock
                foreach (var detail in sale.TransactionDetails)
                {
                    var bp = db.BranchProducts.Find(sale.BranchId, detail.ProductId);

                    bp.LastStock = bp.Stock;
                    bp.Stock += detail.Quantity;

                    db.Entry(bp).State = EntityState.Modified;

                    //agrego movimiento al inventario
                    StockMovement sm = new StockMovement
                    {
                        BranchId = bp.BranchId,
                        ProductId = bp.ProductId,
                        Comment = "Cancelación de venta con folio:" + sale.Folio + " comentario:" + comment,
                        User = User.Identity.Name,
                        MovementDate = DateTime.Now,
                        MovementType = MovementType.Entry,
                        Quantity = detail.Quantity
                    };

                    db.StockMovements.Add(sm);
                }

                //desactivo la venta y registo usuario, comentario y fecha de cancelación
                sale.LastStatus = sale.Status;
                sale.Status = TranStatus.Canceled;
                sale.UpdUser = User.Identity.Name;
                sale.UpdDate = DateTime.Now;
                sale.Comment = comment;

                db.Entry(sale).State = EntityState.Modified;

                db.SaveChanges();

                return Json(new { Result = "OK", Message = "Venta Cancelada, el producto ha sido regreado al stock" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al cancelar la venta", Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate,
            string folio, string client, string user, TranStatus? status)
        {
            var model = LookFor(branchId, beginDate, endDate, folio, client, user, status, null);

            return PartialView("_SaleList", model);
        }


        [Authorize(Roles = "Supervisor")]
        public ActionResult Detail(int id)
        {
            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.User).
                Include(s => s.TransactionDetails.Select(td => td.Product.Category)).
                Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                FirstOrDefault(s => s.TransactionId == id && brancheIds.Contains(s.BranchId));

            if (sale == null)
                return RedirectToAction("History");

            return View(sale);
        }

        [Authorize(Roles = "Vendedor")]
        public ActionResult MyDetail(int id)
        {
            var brancheId = User.Identity.GetBranchId();

            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.User).
                Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                FirstOrDefault(s => s.TransactionId == id && s.BranchId == brancheId);

            if (sale == null)
                return RedirectToAction("MyHistory");

            return View(sale);
        }

        [Authorize(Roles = "Vendedor")]
        public ActionResult MySales()
        {
            var bId = User.Identity.GetBranchId();

            TransactionViewModel model = new TransactionViewModel();
            model.Sales = LookFor(bId, DateTime.Today, null, null, null, null, null, User.Identity.GetUserId());

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult SearchPerUser(DateTime? beginDate, DateTime? endDate, string folio, string client)
        {
            var bId = User.Identity.GetBranchId();
            var model = LookFor(bId, beginDate, endDate, folio, client, null, null, User.Identity.GetUserId());

            return PartialView("_MySaleList", model);
        }


        private List<Sale> LookFor(int? branchId, DateTime? beginDate, DateTime? endDate, string folio, string client,
            string user, TranStatus? status, string userId)
        {

            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var sales = (from p in db.Sales.Include(p => p.User).Include(p => p.TransactionDetails)
                         where
                            (branchId == null && brancheIds.Contains(p.BranchId) || p.BranchId == branchId)
                         && (beginDate == null || p.TransactionDate >= beginDate)
                         && (endDate == null || p.TransactionDate <= endDate)
                         && (folio == null || folio == string.Empty || p.Folio.Contains(folio))
                         && (client == null || client == string.Empty || p.Client.Name.Contains(client))
                         && (user == null || user == string.Empty || p.User.UserName.Contains(user))
                         && (userId == null || userId == string.Empty || p.UserId == userId)
                         && (status == null || p.Status == status)
                         select p).OrderByDescending(p => p.TransactionDate).ToList();

            return sales;
        }

        [HttpPost]
        public JsonResult CheckCart()
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();


            var sale = db.Sales.Include(s => s.TransactionDetails).
                FirstOrDefault(s => s.BranchId == branchId && s.UserId == userId && s.Status == TranStatus.InProcess);

            if (sale != null)
            {
                var list = sale.TransactionDetails;
                return Json(new { Result = "OK", Message = (list.Sum(td => td.Quantity)).ToString() });
            }

            else
                return Json(new { Result = "OK", Message = Cons.Zero.ToString() });
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult SetClient(int clientId, int transactionId)
        {
            try
            {
                var sale = db.Sales.Include(s => s.TransactionDetails).
                    Include(s => s.TransactionDetails.Select(td => td.Product)).
                    Include(s => s.TransactionDetails.Select(td => td.Product.Category)).
                    FirstOrDefault(s => s.TransactionId == transactionId);

                var client = db.Clients.Find(clientId);

                sale.ClientId = clientId;

                foreach (var deteail in sale.TransactionDetails)
                {
                    //ajusto el precio dependiendo de la configuración del cliente
                    switch (client.Type)
                    {
                        case ClientType.Store:
                            deteail.Price = deteail.Product.StorePrice;
                            break;
                        case ClientType.Dealer:
                            deteail.Price = deteail.Product.DealerPrice;
                            break;
                        case ClientType.Wholesaler:
                            deteail.Price = deteail.Product.WholesalerPrice;
                            break;
                    }
                    deteail.Amount = deteail.Quantity * deteail.Price;

                    db.Entry(deteail).State = EntityState.Modified;
                }

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error Al asignar el cliente", Data = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult SetNewPrice(int productId, int transactionId, double price)
        {
            try
            {
                var det = db.TransactionDetails.Include(td => td.Product).Include(td => td.Product.Images)
             .FirstOrDefault(td => td.TransactionId == transactionId && td.ProductId == productId);

                det.Price = price;
                det.Amount = det.Price * det.Quantity;

                db.Entry(det).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al cambiar el costo!", Message = "Ocurrio un error al actualizar el precio detail " + ex.Message });
            }
        }

        [Authorize(Roles = "Vendedor")]
        public ActionResult ShopingCart()
        {
            SaleViewModel model;
            var branchId = User.Identity.GetBranchId();
            string userId = User.Identity.GetUserId();

            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.Client).
                Include(s => s.TransactionDetails.Select(td => td.Product)).
                Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                Include(s => s.TransactionDetails.Select(td => td.Product.BranchProducts)).

                FirstOrDefault(s => (userId == null || s.UserId == userId)
                               && (s.Status == TranStatus.InProcess)
                               && s.BranchId == branchId);

            if (sale != null)
            {
                foreach (var detail in sale.TransactionDetails)
                {
                    var brp = detail.Product.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);
                    detail.Product.Quantity = brp != null ? brp.Stock : Cons.Zero;
                }
                model = new SaleViewModel(sale);
            }

            else
            {
                model = new SaleViewModel();
                model.UpdDate = DateTime.Now;
                model.UserId = userId;
                model.BranchId = branchId;
                model.TransactionDetails = new List<TransactionDetail>();
            }

            model.Categories = db.Categories.ToSelectList();
            model.CarMakes = db.CarMakes.ToSelectList();

            model.UpdUser = User.Identity.Name;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult RemoveFromCart(int transactionId, int productId)
        {
            var detail = db.TransactionDetails.Find(transactionId, productId);

            if (detail != null)
            {
                try
                {
                    db.TransactionDetails.Remove(detail);
                    db.SaveChanges();

                    return Json("OK");
                }
                catch (Exception ex)
                {
                    return Json("Ocurrio un error al eliminar el registro det:" + ex.Message);
                }
            }
            else
            {
                return Json("No se encontro el registro");
            }
        }


        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult AddToCart(int productId, double quantity)
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();


            var bp = db.BranchProducts.Include(brp => brp.Product).
                        FirstOrDefault(brp => brp.BranchId == branchId && brp.ProductId == productId);

            var existance = bp != null ? bp.Stock : Cons.Zero;

            if (existance < quantity)
            {
                var j = new
                {
                    Result = "Sin producto en almacen",
                    Message = "Estas intentando vender mas productos de los disponibles, revisa existencia en sucursal"
                };
                return Json(j);
            }

            //consulto si el usuario tiene una venta activa
            var sale = db.Sales.Include(s => s.Client).FirstOrDefault(s => s.UserId == userId &&
                         s.BranchId == branchId && s.Status == TranStatus.InProcess);


            //Si no hay un registro de venta activo, creo uno nuevo con cliente por defecto
            if (sale == null)
            {
                sale = new Sale
                {
                    UserId = userId,
                    BranchId = branchId,
                    TransactionDate = DateTime.Now,
                    UpdDate = DateTime.Now,
                    UpdUser = User.Identity.Name,
                    Folio = Cons.CodeMask,
                    LastStatus = TranStatus.InProcess,
                    Status = TranStatus.InProcess
                };

                db.Sales.Add(sale);
                db.SaveChanges();
            }


            var amount = 0.0;

            //se asigna el precio en base a la configuración del cliente
            //si no hay un cliente asignado se asigna precio mostrador
            if (sale.Client == null || sale.Client.Type == ClientType.Store)
                amount = (quantity * bp.Product.StorePrice);
            else
            {
                switch (sale.Client.Type)
                {
                    case ClientType.Dealer:
                        amount = (quantity * bp.Product.DealerPrice);
                        break;
                    case ClientType.Wholesaler:
                        amount = (quantity * bp.Product.WholesalerPrice);
                        break;
                }
            }

            //checo si el producto ya esta en la venta
            var detail = db.TransactionDetails.
                FirstOrDefault(td => td.ProductId == productId && td.TransactionId == sale.TransactionId);

            //si lo esta, sumo la cantidad
            if (detail != null)
            {
                detail.Amount += amount;
                detail.Quantity += quantity;

                //verifico el stock y valido si es posible agregar mas producto a la venta
                if (existance < detail.Quantity)
                {
                    var j = new
                    {
                        Result = "Cantidad insuficiente",
                        Message = "Estas intentando vender mas productos de los disponibles, revisa el carrito de venta"
                    };

                    return Json(j);
                }

                db.Entry(detail).State = EntityState.Modified;
            }
            else
            {
                detail = new TransactionDetail
                {
                    ProductId = productId,
                    TransactionId = sale.TransactionId,
                    Price = bp.Product.StorePrice,
                    Quantity = quantity,
                    Amount = amount
                };

                db.TransactionDetails.Add(detail);
            }
            sale.TotalAmount += amount;

            try
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }

            var js = new { Result = "OK" };
            return Json(js);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult ShopingCart(Sale sale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var lastSale = db.Sales.
                        Where(s => s.Status != TranStatus.InProcess).
                        OrderByDescending(s => s.TransactionDate).FirstOrDefault();


                    var lastFolio = lastSale != null ? Convert.ToInt32(lastSale.Folio) : Cons.Zero;

                    var folio = (lastFolio + Cons.One).ToString(Cons.CodeMask);

                    foreach (var detail in sale.TransactionDetails)
                    {
                        //busco stock en sucursal incluyo la categoría de producto para el calculo de comision
                        var pb = db.BranchProducts.Include(brp => brp.Product).
                            Include(brp => brp.Product.Category).
                            FirstOrDefault(brp => brp.BranchId == sale.BranchId && brp.ProductId == detail.ProductId);

                        // si no hay producto suficiente la operación concluye
                        if (pb == null || pb.Stock < detail.Quantity)
                            throw new Exception("Un producto execede la cantidad en inventario");

                        //guardo dato del detalle incluyendo la comision de la venta del determinado producto
                        detail.Amount = detail.Price * detail.Quantity;
                        detail.Quantity = detail.Quantity;

                        if (pb.Product.Category.Commission > Cons.Zero)
                            detail.Commission = Math.Round(detail.Amount * (pb.Product.Category.Commission / Cons.OneHundred),Cons.Two);

                        db.Entry(detail).State = EntityState.Modified;

                        //actualizo stock de sucursal
                        pb.LastStock = pb.Stock;
                        pb.Stock -= detail.Quantity;

                        db.Entry(pb).State = EntityState.Modified;

                        //agrego el moviento al inventario
                        StockMovement sm = new StockMovement
                        {
                            BranchId = pb.BranchId,
                            ProductId = pb.ProductId,
                            MovementType = MovementType.Exit,
                            Comment = "Salida por venta Folio:" + folio,
                            User = User.Identity.Name,
                            MovementDate = DateTime.Now,
                            Quantity = detail.Quantity
                        };

                        db.StockMovements.Add(sm);
                    }

                    //ajusto el monto total y agrego el folio
                    sale.TotalAmount = sale.TransactionDetails.Sum(td => td.Amount);
                    sale.Folio = folio;

                    //coloco el porcentaje de comision del empleado
                    sale.ComPer = User.Identity.GetSalePercentage();

                    //si tiene comision por venta, coloco la cantidad de esta
                    if (sale.ComPer > Cons.Zero)
                    {
                        var comTot = sale.TransactionDetails.Sum(td => td.Commission);
                        if (comTot > Cons.Zero)
                            sale.ComAmount = Math.Round(comTot * (sale.ComPer / Cons.OneHundred), Cons.Two);
                    }


                    //ajuto la hora y fecha de venta a la actual y concluyo
                    sale.TransactionDate = DateTime.Now;
                    sale.Status = TranStatus.Reserved;
                    sale.LastStatus = TranStatus.InProcess;
                    sale.UpdUser = User.Identity.Name;
                    sale.UpdDate = DateTime.Now;

                    db.Entry(sale).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("MySales");
                }
                catch (Exception ex)
                {
                    db.Dispose();
                    db = null;

                    var model = GetVM(sale.TransactionId);
                    ViewBag.Header = "Error al cerrar la venta!";
                    ViewBag.Message = ex.Message;
                    return View("ShopingCart", model);
                }
            }
            else
            {
                db.Dispose();
                db = null;

                var model = GetVM(sale.TransactionId);
                ViewBag.Header = "Datos inválidos!";
                ViewBag.Message = "No se pudo concretar venta debido un error en los datos de ingreso";
                return View("ShopingCart", model);
            }
        }



        private SaleViewModel GetVM(int id)
        {
            if (db == null)
                db = new ApplicationDbContext();


            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.Client).
                                     Include(s => s.TransactionDetails.Select(td => td.Product)).
                                     Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                                     Include(s => s.TransactionDetails.Select(td => td.Product.BranchProducts)).
                                     FirstOrDefault(s => s.TransactionId == id);

            SaleViewModel model = new SaleViewModel(sale);
            model.Categories = db.Categories.ToSelectList();
            model.CarMakes = db.CarMakes.ToSelectList();

            return model;
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}