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

namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize]
    public class PurchasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Purchases
        public ActionResult Index()
        {
            var userId   = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchSession().Id;

            PurchaseHistoryViewModel model = new PurchaseHistoryViewModel();
            model.Branches = db.EmployeeBranches.Include(eb => eb.Employee).Where(eb => eb.Employee.UserId == userId).Select(eb => eb.Branch).ToSelectList();

            model.Purchases = db.Purchases.Where(p => p.BranchId == branchId).ToList();
            return View(model);
        }

        public ActionResult QuickSearch(string code, string name)
        {
            var model = (from p in db.Products
                         where (code == null || code == string.Empty || p.Code == code)
                            && (name == null || name == string.Empty || p.Name.Contains(name))
                         select p).Include(p => p.Images).ToList();

            return PartialView("_ProductList", model);
        }

        // GET: Purchases/Create
        public ActionResult Create(int? id)
        {
            Transaction model;

            if (id != null)
            {
                model = db.Purchases.Include(p => p.TransactionDetails.Select(d => d.Product.Images)).
                    FirstOrDefault(p => p.TransactionId == id);

                var employee = db.Employees.FirstOrDefault(e => e.UserId == model.UserId);
                model.EmployeeName = employee.Name;
                model.EmployeeId = employee.EmployeeId;
            }
            else
            {
                model = new Purchase();
                model.UserId = User.Identity.GetUserId<string>();
                model.BranchId = User.Identity.GetBranchSession().Id;
                model.TransactionTypeId = (int)TransType.Purchase;
                var employee = db.Employees.FirstOrDefault(e => e.UserId == model.UserId);

                model.EmployeeName = employee.Name;
                model.EmployeeId = employee.EmployeeId;
            }
            return View(model);
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "User,Provider")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                if(purchase.TransactionId == Cons.Zero)
                    db.Purchases.Add(purchase);
                else
                {
                    purchase.UserId = User.Identity.GetUserId<string>();
                    purchase.BranchId = User.Identity.GetBranchSession().Id;
                    db.Entry(purchase).State = EntityState.Modified;
                }
             
                db.SaveChanges();
                return RedirectToAction("Create", new { id = purchase.TransactionId });
            }

            return View(purchase);
        }

        [HttpPost]
        public ActionResult AddDetail(int productId, int transactionId, double price, double quantity)
        {
            try
            {
                var detail = db.TransactionsDetail.FirstOrDefault(d => d.ProductId == productId && d.TransactionId == transactionId);

                if (detail != null)
                {
                    detail.Quantity += quantity;
                    detail.Amount = detail.Quantity * detail.Price;

                    db.Entry(detail).State = EntityState.Modified;
                }
                else
                {
                    detail = new TransactionDetail
                    {
                        ProductId = productId,
                        TransactionId = transactionId,
                        Quantity = quantity,
                        Price = price,
                        Amount = price * quantity
                    };

                    db.TransactionsDetail.Add(detail);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("AddDetail", ex);
            }

            var model = db.TransactionsDetail.Include(d => d.Product.Images).
                Where(d => d.TransactionId == transactionId).ToList();


            return PartialView("_Details", model);
        }

        //[HttpPost]
        //public ActionResult RemoveDetail(int transactionId, int transactionDetailId)
        //{
        //}

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PurchaseId,ProviderId,BranchId,TotalAmount,Bill,PurchaseDate,InsDate,UpdDate,EmployeeId")] Transaction purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction purchase = db.Transactions.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction purchase = db.Transactions.Find(id);
            db.Transactions.Remove(purchase);
            db.SaveChanges();
            return RedirectToAction("Index");
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
