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

namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize]
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            //obtengo las sucursales configuradas para el empleado
            var branches = User.Identity.GetBranches();

            TransactionViewModel model = new TransactionViewModel();
            model.Branches = branches.ToSelectList();
            model.Sales = LookFor(DateTime.Today, null, null, null, null, null,true,null);

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(DateTime? beginDate, DateTime? endDate, string folio, string client, string user)
        {
            var model = LookFor(beginDate, endDate, folio, client, user,null,true,null );

            return PartialView("_SaleList", model);
        }

        [Authorize(Roles = "Vendedor")]
        public ActionResult MySales()
        {
            TransactionViewModel model = new TransactionViewModel();
            model.Sales = LookFor(DateTime.Today, null, null, null,null,null,true,User.Identity.GetUserId());

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult SearchPerUser(DateTime? beginDate, DateTime? endDate, string folio, string client)
        {
            var model = LookFor(beginDate, endDate, folio, client,null,null,true,User.Identity.GetUserId());

            return PartialView("_MySaleList", model);
        }


        private List<Sale> LookFor(DateTime? beginDate, DateTime? endDate, string folio, string client, 
            string user, bool? isPayed, bool? compleated, string userId)
        {
            var bId = User.Identity.GetBranchId();

            var sales = (from p in db.Sales.Include(p => p.User).Include(p => p.User.Employees).Include(p => p.TransactionDetails)
                         where (p.BranchId == bId)
                         && (beginDate == null || p.TransactionDate >= beginDate)
                         && (endDate == null || p.TransactionDate <= endDate)
                         && (folio == null || folio == string.Empty || p.Folio.Contains(folio))
                         && (client == null || client == string.Empty || p.Client.Name.Contains(client))
                         && (user == null || user == string.Empty || p.User.UserName.Contains(user))
                         && (userId == null || userId == string.Empty || p.UserId == userId)
                         && (isPayed == null ||  p.IsPayed == isPayed) 
                         &&  (compleated == null || p.Compleated == compleated)
                         select p).OrderByDescending(p=> p.TransactionDate).ToList();

            return sales;
        }


        public ActionResult CheckCart()
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();


            var sale = db.Sales.Include(s => s.TransactionDetails).
                FirstOrDefault(s => s.BranchId == branchId && s.UserId == userId && !s.Compleated);

            if (sale != null)
            {
                var list = sale.TransactionDetails;
                return Content((list.Sum(td => td.Quantity)).ToString());
            }

            else
                return Content(Cons.Zero.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult SetClient(int clientId, int transactionId)
        {
            try
            {
                var sale = db.Sales.Find(transactionId);

                if (sale != null)
                    sale.ClientId = clientId;

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
        public ActionResult ShopingCart(int? id)
        {
            SaleViewModel model;
            string userId = null;
            if (id==null)
                userId = User.Identity.GetUserId();


            var branchId = User.Identity.GetBranchId();

            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.Client).
                Include(s => s.TransactionDetails.Select(td => td.Product)).
                Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                Include(s => s.TransactionDetails.Select(td => td.Product.BranchProducts)).
                FirstOrDefault(s => (userId ==null || s.UserId == userId)
                               && (id != null || s.Compleated == false)
                               && (id == null || s.TransactionId == id )
                               && s.BranchId == branchId);


            if (sale != null)
                model = new SaleViewModel(sale);
            else
            {
                model = new SaleViewModel();
                model.UserId = userId;
                model.BranchId = branchId;
                model.TransactionDetails = new List<TransactionDetail>();
            }
            if (id == null)
            {
                model.Categories = db.Categories.ToSelectList();
                model.CarMakes = db.CarMakes.ToSelectList();
            }

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
        public JsonResult AddToCart(int transactionId, int productId, double quantity, double price)
        {
            Sale sale = null;

            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();

            var amount = (quantity * price);

            //if there is no transaction, create a new one
            if (transactionId == Cons.Zero)
            {
                sale = new Sale
                {
                    UserId = userId,
                    BranchId = branchId,
                    TransactionDate = DateTime.Now,
                    UpdDate = DateTime.Now,
                    Folio = Cons.CodeMask
                };

                db.Sales.Add(sale);
                db.SaveChanges();
            }
            else
                sale = db.Sales.Find(transactionId);

            if (sale.TransactionId > Cons.Zero)
            {
                //check if the product is already added to the transaction
                var detail = db.TransactionDetails.Include(td => td.Product).Include(td => td.Product.BranchProducts)
                    .FirstOrDefault(td => td.ProductId == productId && td.TransactionId == transactionId);

                //if is sum the new quantity
                if (detail != null)
                {
                    detail.Amount += amount;
                    detail.Quantity += quantity;

                    var brP = detail.Product.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);
                    var existance = brP != null ? brP.Stock : Cons.Zero;

                    if (existance < detail.Quantity)
                    {
                        var j = new
                        {
                            Result = "ERROR",
                            Message = "Error:La cantidad total del carrito: "
                            + detail.Quantity + " excede lo disponible en sucursal dispobiles:" + existance
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

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
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
                    foreach (var detail in sale.TransactionDetails)
                    {
                        //busco stock en sucursal
                        var pb = db.BranchProducts.Find(sale.BranchId, detail.ProductId);

                        // si no hay producto suficiente la operación concluye
                        if (pb.Stock < detail.Quantity)
                            throw new Exception("un producto de esta venta excede la cantidad en stock");

                        //guardo dato del detalle
                        detail.Amount = detail.Price * detail.Quantity;
                        detail.Quantity = detail.Quantity;
                        db.Entry(detail).State = EntityState.Modified;

                        //actualizo stock de sucursal
                        pb.LastStock = pb.Stock;
                        pb.Stock     = pb.Stock - detail.Quantity;

                        db.Entry(pb).State = EntityState.Modified;
                    }

                    //ajusto el monto total y agrego el folio
                    sale.TotalAmount = sale.TransactionDetails.Sum(td => td.Amount);
                    sale.Folio = sale.TransactionId.ToString(Cons.CodeMask);

                    //coloco el porcentaje de comision del empleado
                    sale.ComPer = User.Identity.GetSalePercentage();

                    //si tiene comision por venta, coloco la cantidad de esta
                    if (sale.ComPer > Cons.Zero)
                        sale.ComAmount = Math.Round(sale.TotalAmount * (1d / sale.ComPer), Cons.Two);

                    //ajuto la hora y fecha de venta a la actual y concluyo
                    sale.TransactionDate = DateTime.Now;
                    sale.Compleated      = true;

                    db.Entry(sale).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("MySales");
                }
                catch (Exception ex)
                {
                    db.Dispose();
                    db = null;

                    var model = GetVM(sale.TransactionId);
                    ViewBag.Header  = "Error al cerrar la venta!";
                    ViewBag.Message = ex.Message+" "+ex.InnerException!=null? "Detalle interno: " +ex.InnerException.Message : string.Empty;
                    return View("ShopingCart", model);
                }
            }
            else
            {
                db.Dispose();
                db = null;

                var model = GetVM(sale.TransactionId);
                ViewBag.Header  = "Datos inválidos!";
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
                                     Include(s => s.TransactionDetails.Select(td => td.Product.BranchProducts)).FirstOrDefault(s => s.TransactionId == id);

            SaleViewModel model = new SaleViewModel(sale);
            model.Categories = db.Categories.ToSelectList();
            model.CarMakes = db.CarMakes.ToSelectList();

            return model;
        }


        #region Cash Methods
        [Authorize(Roles = "Cajero")]
        public ActionResult Detail(int id)
        {

            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.Client).Include(s => s.User).
             Include(s => s.TransactionDetails.Select(td => td.Product)).
             Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
             Include(s => s.TransactionDetails.Select(td => td.Product.BranchProducts)).
             FirstOrDefault(s => s.TransactionId == id && s.IsPayed);

            return View("SaleDetail", sale);
        }

        [Authorize(Roles = "Cajero")]
        public ActionResult RegisterPayment(int id)
        {
            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.Client).
                       Include(s => s.TransactionDetails.Select(td => td.Product)).
                       Include(s => s.User).
                       Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                       FirstOrDefault(s => s.TransactionId == id && !s.IsPayed && s.Compleated);

            if (sale == null)
                return RedirectToAction("Index");

            return View(sale);
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult RegisterPayment(int transactionId, string payment, double? cash, double? card)
        {
            try
            {
                //busco la venta a pagar
                var sale = db.Sales.Include(s => s.TransactionDetails).FirstOrDefault(s => s.TransactionId == transactionId);
                //marco la venta como pagada y coloco el tipo de pago
                sale.IsPayed = true;
                sale.PaymentType = (PaymentType)Enum.Parse(typeof(PaymentType), payment);
                sale.Payments = new List<Payment>();

                #region Registros de pago
                //si el pago es con efectivo o tarjeta agrego un registro de pago por el monto total
                //de la venta
                if (payment != PaymentType.Mixto.ToString())
                {
                    var p = new Payment
                    {
                        TransactionId = sale.TransactionId,
                        Amount = sale.TotalAmount,
                        PaymentDate = DateTime.Now,
                        PaymentType = sale.PaymentType.Value,
                    };
                    sale.Payments.Add(p);
                }
                //si el pago es Mixto (Efectivo y Tarjeta) agrego un registro de pago con efectivo
                //y otro de tarjeta con los parametros de entrada correspondientes
                else
                {
                    var pm = new Payment
                    { TransactionId = sale.TransactionId, Amount = cash.Value, PaymentDate = DateTime.Now, PaymentType = PaymentType.Efectivo };

                    var pc = new Payment
                    { TransactionId = sale.TransactionId, Amount = card.Value, PaymentDate = DateTime.Now, PaymentType = PaymentType.Tarjeta };

                    sale.Payments.Add(pm);
                    sale.Payments.Add(pc);
                }
                #endregion

                //hago attach a la venta.. esto permite agregar los registros de pago a la base de datos
                db.Sales.Attach(sale);
                db.Entry(sale).State = EntityState.Modified;

                #region registro de Ingreso a caja
                //obtengo el registro de caja activo
                var cr = User.Identity.GetCashRegister();

                if(cr==null)
                {
                    return Json(new
                    {
                        Result = "Error al registrar el pago!",
                        Message = "El modulo de caja no ha sido abierto"
                    });
                }
                //por cada registro de pago agrego una registro de entrada a la caja
                foreach (var pay in sale.Payments)
                {
                    var dt = new Income();
                    dt.CashRegisterId = cr.CashRegisterId;
                    dt.Amount = pay.Amount;
                    dt.InsDate = DateTime.Now;
                    dt.User = User.Identity.Name;
                    dt.Type = pay.PaymentType;
                    dt.SaleFolio = sale.Folio;
                    db.Incomes.Add(dt);
                }
                #endregion

                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al registrar el pago!",
                    Message ="Detalle de la excepción "+ex.Message +" "+ex.InnerException!=null?ex.InnerException.Message:string.Empty });
            }
        }

        #endregion

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