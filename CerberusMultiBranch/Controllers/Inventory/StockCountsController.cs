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
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Models.ViewModels.Inventory;
using CerberusMultiBranch.Support;

namespace CerberusMultiBranch.Controllers.Inventory
{
    [CustomAuthorize]
    public class StockCountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private StockCountsViewModel model = null;

        // GET: StockCounts
        public ActionResult Index()
        {
            model = new StockCountsViewModel();
            model.Branches = User.Identity.GetBranches().OrderBy(o => o.Name).ToList();
            var systems = db.Systems.Where(w => w.IsActive == true).ToList();
            systems.Add(new Models.Entities.Config.PartSystem() { IsActive = true, Name = "--------------------", PartSystemId = 0 });
            model.Systems = systems.OrderBy(o => o.PartSystemId).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult AddStockCount(InventoryFilter filter)
        {

            db = new ApplicationDbContext();
 
            var searchProduct = filter.Products.LastOrDefault();

            var branchProduct = db.BranchProducts.Include(p => p.Product)
                .Where(w => w.ProductId == searchProduct.ProductId && w.BranchId == filter.IdBranch)
                .FirstOrDefault();

            StockCountDetail stockCD = new StockCountDetail();
            stockCD.Product = new Product();
            stockCD.Product.ProductId  = branchProduct.Product.ProductId;
            stockCD.Product.Code = branchProduct.Product.Code;
            stockCD.Product.Name = branchProduct.Product.Name;
            stockCD.ProductId = branchProduct.ProductId;
            stockCD.UnitCost = branchProduct.BuyPrice;
            stockCD.CountQty = 0;
            stockCD.CurrentQty = branchProduct.Stock;
            stockCD.VarianceQty = 0;
            stockCD.VarianceCost = 0;
            return Json(stockCD);
        }

        [HttpPost]
        public JsonResult CreateRegister(InventoryFilter filter)
        {

            db = new ApplicationDbContext();
            bool result = false;
            try
            {

                double totalCost = 0;
                double totalCostVariance = 0;
                filter.StockCountDetails.ForEach((row) => {

                    totalCost = totalCost + row.VarianceQty;
                    totalCostVariance = totalCostVariance + row.VarianceCost;
                });

                StockCount stockCount = new StockCount();
                stockCount.BranchId = filter.IdBranch;
                stockCount.Description = filter.Name;
                stockCount.Comment = filter.Observations;
                stockCount.Compleation = filter.BeginDate;
                stockCount.LinesCounted = filter.LinesCounted;
                stockCount.CorrectLines = filter.CorrectLines;
                stockCount.LinesAccurancy = filter.LinesAccurancy;
                stockCount.TotalCost = totalCost;
                stockCount.TotalCostVariance = totalCostVariance;
                stockCount.User = User.Identity.Name;
                stockCount.PartSystemId = filter.IdPartSystem;

                filter.StockCountDetails.ForEach(s => { s.Product = null;
                    stockCount.StockCountDetails.Add(s);
                });

                db.StockCounts.Add(stockCount);
                db.SaveChanges();


                foreach (StockCountDetail scd in filter.StockCountDetails)
                {

                    StockMovement sm = new StockMovement();
                    sm.BranchId = stockCount.BranchId;
                    sm.ProductId = scd.ProductId;
                    sm.Quantity = scd.VarianceQty;
                    sm.MovementDate = filter.BeginDate;
                    sm.Comment = string.Format("StockCount: {0}-{1}", filter.BeginDate.ToFileTimeUtc(), scd.StockCountDetailId);
                    sm.MovementType = (scd.VarianceQty > 0) ? MovementType.Entry : MovementType.Exit;
                    sm.Quantity = scd.CountQty;
                    sm.User = User.Identity.Name;
                    sm.StockCountId = stockCount.StockCountId;
                    db.StockMovements.Add(sm);
                    var bp = db.BranchProducts.Where(w => w.ProductId == scd.ProductId && w.BranchId == stockCount.BranchId).FirstOrDefault();
                    if (bp != null)
                    {
                        bp.Stock = scd.CountQty;
                        bp.LastStock = scd.CurrentQty;
                    }


                }
                    db.SaveChanges();

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetListMarks(SelectedOption options)
        {

            List<Mark> marks = new List<Mark>();
            var m = db.Products.Where(w => w.PartSystemId == (int?)options.Id).Select(s => s.TradeMark).Distinct().ToList();
            marks.Add(new Mark() { Id = 0, Name = "Todas" });
            int i = 1;
            foreach (var mark in m)
            {
                marks.Add(new Mark() { Id =  i, Name = mark  });
                i++;
            }
            return Json(marks);
        }



        [HttpGet]
        public ActionResult SearchProduct(string filter, string idBranch, string idPartSystem, string TradeMark ,bool fromModal = false)
        {
            int branchId = Convert.ToInt32(idBranch);

            int?  PartSystemId = (Convert.ToInt32(idPartSystem) == 0) ? null : (int?)Convert.ToInt32(idPartSystem);


            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
                arr = filter.Trim().Split(' ');

            if(TradeMark == "Todas" || TradeMark == "")
            {
                TradeMark = string.Empty;
            }

            var model = (from bp in db.BranchProducts.Include(p => p.Product).Include(p => p.Product.Images)
                         where (string.IsNullOrEmpty(filter) || arr.All(s => (bp.Product.Code + " " + bp.Product.Name).Contains(s))) &&
                               (bp.BranchId == branchId) && (bp.Product.IsActive) && ( string.IsNullOrEmpty(TradeMark) || bp.Product.TradeMark == TradeMark) && (PartSystemId == null || bp.Product.PartSystemId == PartSystemId)
                    select new SearchProductResultViewModel
                    {
                        ProductId = bp.Product.ProductId,
                        Name = bp.Product.Name.ToUpper(),
                        Code = bp.Product.Code.ToUpper(),
                        TradeMark = bp.Product.TradeMark.ToUpper(),
                        Image = bp.Product.Images.Count > Cons.Zero ? bp.Product.Images.FirstOrDefault().Path : Cons.NoImagePath,
                        Stock = bp.Stock,
                        StorePrice = Math.Round(bp.StorePrice, Cons.Zero),
                        DealerPrice = Math.Round(bp.DealerPrice, Cons.Zero),
                        WholesalerPrice = Math.Round(bp.WholesalerPrice, Cons.Zero),
                        MinQuantity = bp.MinQuantity,
                        MaxQuantity = bp.MaxQuantity,
                        OrderQty = (bp.MaxQuantity - (bp.Stock + bp.Reserved)),
                        SellQty = (bp.Stock < Cons.One && bp.Stock > Cons.Zero) ? bp.Stock : Cons.One,
                        SaleCommission = bp.Product.System != null ? bp.Product.System.Commission : Cons.Zero,

                    }).OrderBy(p => p.Name).Take(Cons.QuickResults).ToList();

            
            if (model.Count == Cons.One)
            {
                var product = model.First();

                return Json(new JResponse
                {
                    Code = Cons.Responses.Codes.Success,
                    JProperty = product

                }, JsonRequestBehavior.AllowGet);
            }
            else if (fromModal)
                return PartialView("~/Views/Selling/SearchProductResults.cshtml", model);
            else
                return PartialView("~/Views/Selling/SearchProduct.cshtml", model);
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

        public ActionResult GetProductsInfo(List<int> productIds)
        {
            var branchId = User.Identity.GetBranchId();

            var model = GetProducts(productIds);

            return Json(new JResponse
            {
                Code = Cons.Responses.Codes.Success,
                Result = Cons.Responses.Success,
                JProperty = model
            }, JsonRequestBehavior.AllowGet);
        }


        private List<SearchProductResultViewModel> GetProducts(List<int> productIds)
        {
            var branchId = User.Identity.GetBranchId();

            var model = (from bp in db.BranchProducts.Include(b => b.Product.Images)
                         where (bp.BranchId == branchId) && productIds.Contains(bp.ProductId)
                         select new SearchProductResultViewModel
                         {
                             ProductId = bp.Product.ProductId,
                             Name = bp.Product.Name.ToUpper(),
                             Code = bp.Product.Code.ToUpper(),
                             TradeMark = bp.Product.TradeMark.ToUpper(),
                             Image = bp.Product.Images.Count > Cons.Zero ? bp.Product.Images.FirstOrDefault().Path : Cons.NoImagePath,
                             Stock = bp.Stock,
                             StorePrice = Math.Round(bp.StorePrice, Cons.Zero),
                             DealerPrice = Math.Round(bp.DealerPrice, Cons.Zero),
                             WholesalerPrice = Math.Round(bp.WholesalerPrice, Cons.Zero),
                             MinQuantity = bp.MinQuantity,
                             MaxQuantity = bp.MaxQuantity,
                             OrderQty = (bp.MaxQuantity - (bp.Stock + bp.Reserved)),
                             SellQty = (bp.Stock < Cons.One && bp.Stock > Cons.Zero) ? bp.Stock : Cons.One,
                             SaleCommission = bp.Product.System != null ? bp.Product.System.Commission : Cons.Zero

                         }).ToList();

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
