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

           var list = db.TransactionDetails.Where(td => td.Transaction.BranchId == branchId 
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
                    detail.Amount -= amount;
                    detail.Quantity += quantity;

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

            return Json(true);
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