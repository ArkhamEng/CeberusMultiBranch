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

        [Authorize(Roles = "Supervisor,Vendedor")]
        public ActionResult History()
        {
            //obtengo las sucursales configuradas para el empleado
            var branches = User.Identity.GetBranches();

            TransactionViewModel model = new TransactionViewModel();
            model.Branches = branches.ToSelectList();
            model.Sales = new List<Sale>(); //LookFor(null, DateTime.Today, null, null, null, null, null, null);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor,Cajero")]
        public JsonResult Cancel(int transactionId, string comment)
        {
           return ChangeStatus(transactionId, comment, TranStatus.Canceled);
        }

       
        public JsonResult ChangeStatus(int transactionId, string comment, TranStatus status)
        {
            try
            {
                //busco la venta a la que se le cambiara el status
                var sale = db.Sales.Include(s => s.TransactionDetails).
                    FirstOrDefault(s => s.TransactionId == transactionId);

                //regreso los productos al stock
                foreach (var detail in sale.TransactionDetails)
                {
                    //se  regresa al inventario todo producto de la venta
                    //en el caso de los paquetes solo el producto padre
                    if (detail.ParentId == null)
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
                            Comment = "Entrada por Cancelacion de venta con folio:" + sale.Folio + " comentario:" + comment,
                            User = User.Identity.Name,
                            MovementDate = DateTime.Now.ToLocal(),
                            MovementType = MovementType.Entry,
                            Quantity = detail.Quantity
                        };

                        db.StockMovements.Add(sm);
                    }
                }

                //desactivo la venta y registo usuario, comentario y fecha de cancelación
                sale.LastStatus = sale.Status;
                sale.Status     = status;
                sale.UpdUser    = User.Identity.Name;
                sale.UpdDate    = DateTime.Now.ToLocal();
                sale.Comment    = comment;

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
        [Authorize(Roles = "Supervisor,Vendedor")]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate,
            string folio, string client, string user, TranStatus? status)
        {
            //si el usuario no es un supervisor, solo se le permite ver el dato de sus ventas
            if (!User.IsInRole("Supervisor"))
                user = User.Identity.GetUserName();

            var model = LookFor(branchId, beginDate, endDate, folio, client, user, status, null);

            return PartialView("_SaleList", model);
        }


        [Authorize(Roles = "Supervisor,Vendedor")]
        public ActionResult Detail(int id)
        {
            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.User).
                Include(s => s.TransactionDetails.Select(td => td.Product.Category)).
                Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                FirstOrDefault(s => s.TransactionId == id && brancheIds.Contains(s.BranchId));

            if (sale == null)
                return RedirectToAction("History");

            sale.TransactionDetails = sale.TransactionDetails.OrderBy(td => td.SortOrder).ToList();

            return View(sale);
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
                var branchId = User.Identity.GetBranchId();

                var sale = db.Sales.Include(s => s.TransactionDetails).
                    Include(s => s.TransactionDetails.Select(td => td.Product)).
                    //Include(s => s.TransactionDetails.Select(td => td.Product.Category)).
                    Include(s => s.TransactionDetails.Select(td => td.Product.BranchProducts)).
                    FirstOrDefault(s => s.TransactionId == transactionId);

                if (sale != null)
                {
                    foreach (var detail in sale.TransactionDetails)
                    {
                        var brp = detail.Product.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);
                        detail.Product.Quantity = brp != null ? brp.Stock : Cons.Zero;
                        detail.Product.StorePrice = brp != null ? brp.StorePrice : Cons.Zero;
                        detail.Product.DealerPrice = brp != null ? brp.DealerPrice : Cons.Zero;
                        detail.Product.WholesalerPrice = brp != null ? brp.WholesalerPrice : Cons.Zero;
                    }
                }

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
        public ActionResult SetNewPrice(int productId, int transactionId, double price)
        {
            try
            {
                var det = db.TransactionDetails.Include(td => td.Product).Include(td => td.Product.Images)
             .FirstOrDefault(td => td.TransactionId == transactionId && td.ProductId == productId);

                det.Price = price;
                var newAmount = det.Price * det.Quantity;

                //si la nueva cantidad es menor a la anterior
                //resto la diferencia del total de la venta
                if (det.Amount > newAmount)
                {
                    var f = det.Amount - newAmount;
                    det.Sale.TotalAmount -= f;
                }

                //si la nueva cantidad es mayor a la anterior
                //sumo la diferencia del total de la venta
                else
                {
                    var f = det.Amount - newAmount;
                    det.Sale.TotalAmount += newAmount;
                }

                det.Amount = newAmount;

                db.Entry(det).State = EntityState.Modified;
                db.Entry(det.Sale).State = EntityState.Modified;
                db.SaveChanges();

                var model = db.TransactionDetails.Include(s => s.Product).
                    Include(s => s.Product.Images).Where(s => s.TransactionId == transactionId).ToList();

                return PartialView("_CartDetails", model);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al cambiar el costo!", Message = "Ocurrio un error al actualizar el precio detail " + ex.Message });
            }
        }

        [Authorize(Roles = "Vendedor")]
        [HttpPost]
        public ActionResult OpenCart()
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
                    detail.Product.StorePrice = brp != null ? brp.StorePrice : Cons.Zero;
                    detail.Product.DealerPrice = brp != null ? brp.DealerPrice : Cons.Zero;
                    detail.Product.WholesalerPrice = brp != null ? brp.WholesalerPrice : Cons.Zero;
                }
                model = new SaleViewModel(sale);
            }

            else
            {
                model = new SaleViewModel();
                model.UpdDate = DateTime.Now.ToLocal();
                model.UserId = userId;
                model.BranchId = branchId;
                model.TransactionDetails = new List<TransactionDetail>();
            }

            model.UpdUser = User.Identity.Name;
            return PartialView("_Cart", model);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult RemoveFromCart(int transactionId, int productId)
        {
            var detail = db.TransactionDetails.Find(transactionId, productId);

            if (detail != null)
            {
                try
                {
                    db.TransactionDetails.Remove(detail);
                    db.SaveChanges();

                    var model = db.TransactionDetails.Include(t => t.Product).
                        Include(t => t.Product.Images).
                        Where(t => t.TransactionId == transactionId).ToList();

                    var sale = db.Sales.Find(transactionId);

                    if (sale != null)
                    {
                        sale.TotalAmount = model.Count > Cons.Zero ? model.Sum(td => td.Amount) : Cons.Zero;
                        sale.UpdDate = DateTime.Now.ToLocal();
                        sale.UpdUser = User.Identity.GetUserName();

                        db.Entry(sale).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return PartialView("_CartDetails", model);
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Ocurrio un error al eliminar el registro", Message = ex.Message });
                }
            }
            else
            {
                return Json(new { Result = "Dato no encontrado", Message = "No se encontro el registo a eliminar, intenta recargar la pagina usando F5" });
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

            if (bp.StorePrice <= Cons.Zero)
            {
                var j = new
                {
                    Result = "Error en precio",
                    Message = "El precio de mostrador debe ser mayor a $0, revisa la configuración"
                };
                return Json(j);
            }
            else if (bp.DealerPrice <= Cons.Zero)
            {
                var j = new
                {
                    Result = "Error en precio",
                    Message = "El precio de distribuidor debe ser mayor a $0, revisa la configuración"
                };
                return Json(j);
            }
            else if (bp.WholesalerPrice <= Cons.Zero)
            {
                var j = new
                {
                    Result = "Error en precio",
                    Message = "El precio de mayorista debe ser mayor a $0, revisa la configuración"
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
                    TransactionDate = DateTime.Now.ToLocal(),
                    UpdDate = DateTime.Now.ToLocal(),
                    UpdUser = User.Identity.Name,
                    Folio = Cons.CodeMask,
                    LastStatus = TranStatus.InProcess,
                    Status = TranStatus.InProcess
                };

                db.Sales.Add(sale);
                db.SaveChanges();
            }


            var amount = 0.0;
            var price = 0.0;

            //se asigna el precio en base a la configuración del cliente
            //si no hay un cliente asignado se asigna precio mostrador
            if (sale.Client == null || sale.Client.Type == ClientType.Store)
            {
                amount = (quantity * bp.StorePrice);
                price = bp.StorePrice;
            }
            else
            {
                switch (sale.Client.Type)
                {
                    case ClientType.Dealer:
                        amount = (quantity * bp.DealerPrice);
                        price = bp.DealerPrice;
                        break;
                    case ClientType.Wholesaler:
                        amount = (quantity * bp.WholesalerPrice);
                        price = bp.WholesalerPrice;
                        break;
                }
            }

            //checo si el producto ya esta en la venta
            var detail = db.TransactionDetails.
                FirstOrDefault(td => td.ProductId == productId && td.TransactionId == sale.TransactionId);

            //si lo esta, sumo la cantidad
            if (detail != null)
            {
                //utilizo el precio seteado (ya que pudo haber sido modificado manualmente)
                amount = detail.Price * quantity;

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
                    Price = price,
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
        public JsonResult CompleateSale(int transactionId, int sending)
        {
            var sale = db.Sales.Include(s => s.TransactionDetails).
                FirstOrDefault(s => s.TransactionId == transactionId);

            try
            {
                var branchId = User.Identity.GetBranchId();

                var lastSale = db.Sales.
                    Where(s => s.Status != TranStatus.InProcess && s.BranchId == branchId).
                    OrderByDescending(s => s.TransactionDate).FirstOrDefault();

                var lastFolio = lastSale != null ? Convert.ToInt32(lastSale.Folio) : Cons.Zero;

                var folio = (lastFolio + Cons.One).ToString(Cons.CodeMask);

                int sortOrder = Cons.One;

                foreach (var detail in sale.TransactionDetails)
                {
                    //busco stock en sucursal incluyo la categoría de producto para el calculo de comision
                    var pb = db.BranchProducts.Include(brp => brp.Product).
                        Include(brp => brp.Product.Category).
                        Include(brp => brp.Product.PackageDetails).
                        FirstOrDefault(brp => brp.BranchId == sale.BranchId && brp.ProductId == detail.ProductId);

                    // si no hay producto suficiente la operación concluye
                    if (pb == null || pb.Stock < detail.Quantity)
                        throw new Exception("Un producto execede la cantidad en inventario");

                    //guardo dato del detalle incluyendo la comision de la venta del determinado producto
                    detail.Amount = detail.Price * detail.Quantity;
                    detail.Quantity = detail.Quantity;
                    detail.SortOrder = sortOrder;

                    if (pb.Product.Category.Commission > Cons.Zero)
                        detail.Commission = Math.Round(detail.Amount * (pb.Product.Category.Commission / Cons.OneHundred), Cons.Two);

                    db.Entry(detail).State = EntityState.Modified;

                    //actualizo stock de sucursal
                    pb.LastStock = pb.Stock;
                    pb.Stock -= detail.Quantity;

                    db.Entry(pb).State = EntityState.Modified;

                    //si el producto que se esta agregando vendiendo es un paquete
                    //agrego todos los productos q lo complementan a la venta con precio 0
                    if (pb.Product.ProductType == ProductType.Package)
                    {
                        foreach (var pckDet in pb.Product.PackageDetails)
                        {
                            sortOrder++;
                            var tDeatil = new TransactionDetail
                            {
                                TransactionId = detail.TransactionId,
                                Quantity = pckDet.Quantity,
                                Price = Cons.Zero,
                                Commission = Cons.Zero,
                                ProductId = pckDet.DetailtId,
                                SortOrder = sortOrder,
                                ParentId = pckDet.PackageId,
                            };

                            db.TransactionDetails.Add(tDeatil);
                        }
                    }

                    //agrego el moviento al inventario
                    StockMovement sm = new StockMovement
                    {
                        BranchId = pb.BranchId,
                        ProductId = pb.ProductId,
                        MovementType = MovementType.Exit,
                        Comment = "Salida por venta Folio:" + folio,
                        User = User.Identity.Name,
                        MovementDate = DateTime.Now.ToLocal(),
                        Quantity = detail.Quantity
                    };

                    db.StockMovements.Add(sm);

                    sortOrder++;
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
                sale.TransactionDate = DateTime.Now.ToLocal();
                sale.Status = TranStatus.Reserved;
                sale.LastStatus = TranStatus.InProcess;
                sale.UpdUser = User.Identity.Name;
                sale.UpdDate = DateTime.Now.ToLocal();
                // sale.PaymentType     = (PaymentType)Enum.Parse(typeof(PaymentType), payment.ToString());
                sale.SendingType = sending;

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Result = "OK", Message = "Se ha generado la venta con folio:" + folio });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Ocurrio un error",
                    Message = "No se pudo concretar la venta debido a un error inesperado " + ex.Message
                });
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