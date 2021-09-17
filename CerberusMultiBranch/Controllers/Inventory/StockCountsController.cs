using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Inventory;
using CerberusMultiBranch.Models.ViewModels.Inventory;
using CerberusMultiBranch.Support;

namespace CerberusMultiBranch.Controllers.Inventory
{
    [CustomAuthorize]
    public class StockCountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockCounts
        public ActionResult Index()
        {
            var model = new StockCountsViewModel(); 
                
            var stock =  db.StockCounts.Include(s => s.Branch);
            model.Products = new List<List<Product>>();
            model.Systems = db.Systems.OrderBy(s => s.Name).ToSelectList();
            model.Categories = db.Categories.OrderBy(c => c.Name).ToSelectList();
            model.Makes = db.CarMakes.OrderBy(m => m.Name).ToSelectList();
            return View(model);
        }

        // GET: StockCounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockCount stockCount = db.StockCounts.Find(id);
            if (stockCount == null)
            {
                return HttpNotFound();
            }
            return View(stockCount);
        }

        // GET: StockCounts/Create
        public ActionResult Create()
        {
            ViewBag.BranchId = new SelectList(db.Branches, "BranchId",  "Name");
            return View();
        }

        // POST: StockCounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StockCountId,SystemId,BranchId,Description,Comment,Compleation,LinesCounted,CorrectLines,LinesAccurancy,TotalCost,TotalCostVariance,User")] StockCount stockCount)
        {
            if (ModelState.IsValid)
            {
                db.StockCounts.Add(stockCount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "Name", stockCount.BranchId);
            return View(stockCount);
        }

        // GET: StockCounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockCount stockCount = db.StockCounts.Find(id);
            if (stockCount == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "Name", stockCount.BranchId);
            return View(stockCount);
        }

        // POST: StockCounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StockCountId,SystemId,BranchId,Description,Comment,Compleation,LinesCounted,CorrectLines,LinesAccurancy,TotalCost,TotalCostVariance,User")] StockCount stockCount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockCount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "Name", stockCount.BranchId);
            return View(stockCount);
        }

        // GET: StockCounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockCount stockCount = db.StockCounts.Find(id);
            if (stockCount == null)
            {
                return HttpNotFound();
            }
            return View(stockCount);
        }

        // POST: StockCounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockCount stockCount = db.StockCounts.Find(id);
            db.StockCounts.Remove(stockCount);
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
