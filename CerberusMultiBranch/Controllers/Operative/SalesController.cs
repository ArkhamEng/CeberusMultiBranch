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

        // GET: Purchases
        public ActionResult Index()
        {
            //obtengo las sucursales configuradas para el empleado
            var branches = User.Identity.GetBranches();

            TransactionViewModel model = new TransactionViewModel();
            model.Branches = branches.ToSelectList();
            model.Sales = LookFor(null, DateTime.Today, null, null, null, null, branches);

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate, string folio, string client, string employee)
        {
            var branches = User.Identity.GetBranches();
            var model = LookFor(branchId, beginDate, endDate, folio, client, employee, branches);

            return PartialView("_SaleList", model);
        }

        private List<Sale> LookFor(int? branchId, DateTime? beginDate, DateTime? endDate, string folio, string client, string employee, List<Branch> branches)
        {
            var bId = User.Identity.GetBranchId();

            //busco los userId de los empleados que coincidan con el filtro
            var uList = (employee == null || employee == string.Empty) ?
                db.Employees.Where(e => e.Name.Contains(employee)).Select(e => e.UserId).ToList() : null;

            var sales = (from p in db.Sales.Include(p => p.User).Include(p => p.User.Employees).Include(p => p.TransactionDetails)
                         where (p.BranchId == bId)
                         && (beginDate == null || p.TransactionDate >= beginDate)
                         && (endDate == null || p.TransactionDate <= endDate)
                         && (folio == null || folio == string.Empty || p.Folio.Contains(folio))
                         && (client == null || client == string.Empty || p.Client.Name.Contains(client))
                         && (employee == null || employee == string.Empty || uList.Contains(p.UserId))
                         && (p.IsPayed)
                         select p).ToList();

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

        public ActionResult ShopingCart()
        {
            SaleViewModel model;

            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();

            var sale = db.Sales.Include(s=> s.TransactionDetails).Include(s => s.Client).
                Include(s=> s.TransactionDetails.Select(td=> td.Product)).
                Include(s=> s.TransactionDetails.Select(td=> td.Product.Images)).
                Include(s=> s.TransactionDetails.Select(td=> td.Product.BranchProducts)).
                FirstOrDefault(s => s.UserId == userId && s.BranchId == branchId && s.Compleated==false);


            if (sale!=null)
                model = new SaleViewModel(sale);
            else
            {
                model = new SaleViewModel();
                model.UserId = userId;
                model.BranchId = branchId;
                model.TransactionDetails = new List<TransactionDetail>();
            }

            model.Categories = db.Categories.ToSelectList();
            model.CarMakes   = db.CarMakes.ToSelectList();
    
            return View(model);
        }

        [HttpPost]
        public JsonResult RemoveFromCart(int transactionId, int productId)
        {
            var detail =  db.TransactionDetails.Find(transactionId, productId);

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
        public JsonResult AddToCart(int transactionId, int productId, double quantity, double price)
        {
            Sale sale = null;

            var userId   = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();

            var amount = (quantity * price);

            //if there is no transaction, create a new one
            if (transactionId == Cons.Zero)
            {
                sale = new Sale
                {
                    UserId          = userId,
                    BranchId        = branchId,
                    TransactionDate = DateTime.Now,
                    UpdDate         = DateTime.Now,
                    Folio           = Cons.CodeMask
                };

                db.Sales.Add(sale);
                db.SaveChanges();
            }
            else
                sale = db.Sales.Find(transactionId);

            if (sale.TransactionId > Cons.Zero)
            {
                //check if the product is already added to the transaction
                var detail = db.TransactionDetails.Include(td=>td.Product).Include(td=>td.Product.BranchProducts)
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
                        var j = new { Result = "ERROR", Message = "Error:La cantidad total del carrito: " 
                            + detail.Quantity + " excede lo disponible en sucursal dispobiles:" + existance };

                        return Json(j);
                    }

                    db.Entry(detail).State = EntityState.Modified;
                }
                else
                {
                    detail = new TransactionDetail
                    {
                        ProductId     = productId,
                        TransactionId = sale.TransactionId,
                        Price         = price,
                        Quantity      = quantity,
                        Amount        = amount
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
        public ActionResult Create(Sale sale, string payment, double? cash, double? card)
        {
            var branchId = User.Identity.GetBranchId();

            var chasR = User.Identity.GetCashRegister();

            //Check for existance before to update
            foreach (var detail in sale.TransactionDetails)
            {
                var ex = User.Identity.GetStock(detail.ProductId);
                detail.Amount = detail.Price * detail.Quantity;
                detail.Quantity = detail.Quantity;

                db.Entry(detail).State = EntityState.Modified;

                var pb = db.BranchProducts.Find(branchId, detail.ProductId);
                pb.LastStock = pb.Stock;
                pb.Stock -= detail.Quantity;

                db.Entry(pb).State = EntityState.Modified;
            }

            sale.TotalAmount = sale.TransactionDetails.Sum(td => td.Amount);
            sale.Folio = sale.TransactionId.ToString(Cons.CodeMask);
            sale.IsPayed = true;
            sale.Compleated = true;
            sale.PaymentType = (PaymentType)Enum.Parse(typeof(PaymentType),payment);
            sale.Payments = new List<Payment>();

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
            else
            {
                var pm = new Payment
                { TransactionId = sale.TransactionId, Amount = cash.Value, PaymentDate = DateTime.Now, PaymentType = PaymentType.Efectivo };

                var pc = new Payment
                { TransactionId = sale.TransactionId, Amount = card.Value, PaymentDate = DateTime.Now, PaymentType = PaymentType.Tarjeta };

                db.Payments.Add(pm);
                db.Payments.Add(pc);
            }


            db.Sales.Attach(sale);
            db.Entry(sale).State = EntityState.Modified;
            db.SaveChanges();

            //Creo el registro en caja

            foreach (var pay in sale.Payments)
            {
                CashRegisterController.AddIncome(pay.Amount, pay.PaymentType, User.Identity);
            }


            return RedirectToAction("ShopingCart");
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