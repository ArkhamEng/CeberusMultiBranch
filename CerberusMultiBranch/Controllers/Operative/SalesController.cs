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

        private List<Sale> LookFor(int? branchId, DateTime? beginDate, DateTime? endDate, string bill, string client, string employee, List<Branch> branches)
        {
            var bList = branches.Select(b => b.BranchId).ToList();

            //busco los userId de los empleados que coincidan con el filtro
            var uList = (employee == null || employee == string.Empty) ?
                db.Employees.Where(e => e.Name.Contains(employee)).Select(e => e.UserId).ToList() : null;

            var sales = (from p in db.Sales.Include(p => p.User).Include(p => p.User.Employees).Include(p => p.TransactionDetails)
                             where (branchId == null && bList.Contains(p.BranchId)
                             || p.BranchId == branchId && bList.Contains(p.BranchId))
                             && (beginDate == null || p.TransactionDate >= beginDate)
                             && (endDate == null || p.TransactionDate <= endDate)
                             && (bill == null || bill == string.Empty || p.Folio.Contains(bill))
                             && (client == null || client == string.Empty || p.Client.Name.Contains(client))
                             && (employee == null || employee == string.Empty || uList.Contains(p.UserId))
                             select p).ToList();

            return sales;
        }

        public ActionResult CheckCart()
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchSession().Id;

           var list = db.TransactionDetails.Where(td => td.Transaction.BranchId == branchId && !td.Transaction.IsCompleated
           && td.Transaction.UserId == userId && td.Transaction.TransactionTypeId == (int)TransType.Sale).ToList();

            if (list != null)
                return Content((list.Sum(td => td.Quantity)*(-Cons.One)).ToString());
            else
                return Content(Cons.Zero.ToString());
        }

        public ActionResult ShopingCart()
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchSession().Id;

            var model = db.Sales.Include(s => s.TransactionDetails.Select(d => d.Product.Images)).
                    FirstOrDefault(s => s.BranchId == branchId && s.UserId == userId && !s.IsCompleated);

            if(model==null || model.TransactionDetails.Count < 0)
                return View();
            
            foreach (var detail in model.TransactionDetails)
                detail.Quantity *= -Cons.One;

            if (model != null)
            {
                var employee = db.Employees.FirstOrDefault(e => e.UserId == model.UserId);
                model.EmployeeName = employee.Name;
                model.EmployeeId = employee.EmployeeId;
            }
            else
                model = new Sale();
            return View(model);
        }

        [HttpPost]
        public JsonResult AddToCart(int transactionId, int productId, double quantity, double price)
        {
            Sale sale = null;

            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchSession().Id;

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
                    Folio = Cons.CodeMask,
                    TransactionTypeId = (int)TransType.Sale
                };

                db.Transactions.Add(sale);
                db.SaveChanges();
            }
            else
                sale = db.Sales.Find(transactionId);

            if (sale.TransactionId > Cons.Zero)
            {
                //check if the product is already added to the transaction
                var detail = db.TransactionDetails.FirstOrDefault(td => td.ProductId == productId && td.TransactionId == transactionId);

                //if is sum the new quantity
                if (detail != null)
                {
                    detail.Amount += amount;
                    detail.Quantity += quantity;

                    var existance = db.TransactionDetails.
                        Where(td => td.ProductId == productId && td.Transaction.BranchId == branchId && td.Transaction.IsCompleated).
                        Sum(td => td.Quantity);

                    if (existance < -(detail.Quantity))
                        return Json("La cantidad total del carrito: "+detail.Quantity+" excede lo disponible en sucursal dispobiles:" +existance);

                    db.Entry(detail).State = EntityState.Modified;
                }
                else
                {
                    detail = new TransactionDetail { ProductId = productId, TransactionId = sale.TransactionId,
                        Price = price,Quantity = -quantity, Amount = amount };
                    db.TransactionDetails.Add(detail);
                }
                sale.TotalAmount += amount;

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json("OK");
        }

        [HttpPost]
        public ActionResult ShopingCart(Sale sale,string payment, double? cash, double? card)
        {
            var branchId = User.Identity.GetBranchSession().Id;

            //Check for existance before to update
            foreach(var detail in sale.TransactionDetails)
            {
                var ex          = User.Identity.GetStock(detail.ProductId);
                detail.Amount   = detail.Price * detail.Quantity;
                detail.Quantity = detail.Quantity;

                db.Entry(detail).State = EntityState.Modified;

                var pb = db.BranchProducts.Find(detail.ProductId, branchId);
                pb.LastStock = pb.Stock;
                pb.Stock -= detail.Quantity;

                db.Entry(pb).State = EntityState.Modified;
            }

            sale.TotalAmount  = sale.TransactionDetails.Sum(td => td.Amount);
            sale.Folio        = sale.TransactionId.ToString(Cons.CodeMask);
            sale.IsCompleated = true;

            sale.Payments    = new List<Payment>();

            if(payment != PaymentType.Mixed.ToString() )
            {
                var p = new Payment
                {
                    TransactionId = sale.TransactionId,
                    Amount = sale.TotalAmount,
                    PaymentDate = DateTime.Now,

                    PaymentType = (PaymentType)Enum.Parse(typeof(PaymentType),payment),
                };
                sale.Payments.Add(p);
            }
            else
            {
                var pm = new Payment
                {TransactionId = sale.TransactionId, Amount = cash.Value, PaymentDate = DateTime.Now, PaymentType = PaymentType.Cash };

                var pc = new Payment
                { TransactionId = sale.TransactionId, Amount = card.Value, PaymentDate = DateTime.Now, PaymentType = PaymentType.Card };

                db.Payments.Add(pm);
                db.Payments.Add(pc);
            }

            db.Sales.Attach(sale);
            db.Entry(sale).State = EntityState.Modified;
            db.SaveChanges();

            //if (ex < (detail.Quantity))
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Bla bla bla");
            //}
            //return Json("OK");

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