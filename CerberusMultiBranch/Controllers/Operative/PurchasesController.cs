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

namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize]
    public class PurchasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Purchases
        public ActionResult Index()
        {
            return View(db.Purchases.ToList());
        }

        public ActionResult QuickSearch(string code, string name)
        {
            var model = (from p in db.Products
                         where (code == null || code == string.Empty || p.Code == code)
                         && (name == null || name == string.Empty || p.Name == name)
                         select p).Include(p => p.Images).ToList();

            return PartialView("_ProductList", model);
        }

        // GET: Purchases/Create
        public ActionResult Create(int? id)
        {
            Purchase model;

            if (id != null)
            {
                model = db.Purchases.Include(p => p.PurchaseDetails.Select(d => d.Product.Images)).FirstOrDefault(p => p.PurchaseId == id);
            }
            else
            {
                model = new Purchase();
                var UserId = User.Identity.GetUserId<string>();
                model.BranchId = Extension.GetBranchSession().Id;
                model.Employee = db.Employees.FirstOrDefault(e => e.UserId == UserId);
                model.EmployeeId = model.Employee.EmployeeId;
            }
            return View(model);
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Employee")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Purchases.Add(purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(purchase);
        }

        [HttpPost]
        public ActionResult AddDetail(int productId, int purchaseId, double buyprice, double quantity)
        {
            try
            {

                var detail = db.PurchaseDetails.FirstOrDefault(d => d.ProductId == productId);

                if(detail != null)
                {
                    detail.Quantity += quantity;
                    detail.Amount   = detail.Quantity * detail.BuyPrice;

                    db.Entry(detail).State = EntityState.Modified;
                }
                else
                {
                    detail = new PurchaseDetail
                    {
                        ProductId  = productId,
                        PurchaseId = purchaseId,
                        Quantity   = quantity,
                        BuyPrice   = buyprice,
                        Amount     = buyprice * quantity
                    };

                    db.PurchaseDetails.Add(detail);
                }
                
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("AddDetail", ex);
            }

            var model = db.PurchaseDetails.Include(d=> d.Product.Images).
                Where(d => d.PurchaseId == purchaseId).ToList();
             

            return PartialView("_Details", model);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PurchaseId,ProviderId,BranchId,TotalAmount,Bill,PurchaseDate,InsDate,UpdDate,EmployeeId")] Purchase purchase)
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
            Purchase purchase = db.Purchases.Find(id);
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
            Purchase purchase = db.Purchases.Find(id);
            db.Purchases.Remove(purchase);
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
