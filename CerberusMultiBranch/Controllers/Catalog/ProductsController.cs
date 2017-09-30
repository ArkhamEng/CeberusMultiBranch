using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Support;
using System;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using CerberusMultiBranch.Models.Entities.Operative;

namespace CerberusMultiBranch.Controllers.Catalog
{
    [Authorize]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Index Methods
        public ActionResult Index()
        {
            var model = new SearchProductViewModel();
            model.Products = LookFor(null, null, null, null,false);

            model.Categories = db.Categories.ToSelectList();
            model.Makes = db.CarMakes.ToSelectList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(int? categoryId, int? carYear, string name, string code,bool isGrid)
        {
            var model = LookFor(categoryId, carYear, name, code, isGrid);
            return PartialView("_List", model);
        }

        [HttpPost]
        public ActionResult GetStockInBranches(int productId)
        {
            var branches = db.Branches.ToList();
            var details = db.TransactionDetails.Include(td => td.Transaction).Where(td => td.ProductId == productId).ToList();

            foreach (var branch in branches)
                branch.Quantity = details.Where(td => td.Transaction.BranchId == branch.BranchId && td.Transaction.IsCompleated).Sum(dt => dt.Quantity);

            return PartialView("_StockInBranches", branches);
        }

        private List<List<Product>> LookFor(int? categoryId, int? carYear, string name, string code, bool isGrid)
        {
            var branchId = User.Identity.GetBranchSession().Id;

            var products = (from p in db.Products.Include(p => p.Images).Include(p => p.Compatibilities).Include(p => p.BranchProducts)
                            where (categoryId == null || p.CategoryId == categoryId)
                            && (name == null || name == string.Empty || p.Name.Contains(name))
                            && (code == null || code == string.Empty || p.Code == code)
                            && (carYear == null || p.Compatibilities.Where(c => c.CarYearId == carYear).ToList().Count > Cons.Zero)
                            select p ).ToList();

        //   products.ForEach(p => p.Quantity = p.BranchProducts.FirstOrDefault(bp=> bp.BranchId == branchId).Stock);
            foreach(var prod in products)
            {
                var bp = prod.BranchProducts.FirstOrDefault(b => b.BranchId == branchId);
                prod.Quantity = bp != null ? bp.Stock : 0;
            }

            products.OrderCarModels();

            if (isGrid)
                return OrderAsGrid(products);
            else
            {
                List<List<Product>> ord = new List<List<Product>>();
                foreach(var p in products)
                {
                    List<Product> pl = new List<Product>();
                    pl.Add(p);
                    ord.Add(pl);
                }
                return ord;
            }
        }

        private List<List<Product>> OrderAsGrid(List<Product> products)
        {
            List<List<Product>> prodMod = new List<List<Product>>();

            bool newRow = true;
            List<Product> list = null;

            for (int i = Cons.Zero; i < products.Count; i++)
            {
                if (newRow)
                {
                    list = new List<Product>();
                    list.Add(products[i]);

                    if (i == products.Count - Cons.One)
                        prodMod.Add(list);

                    newRow = false;
                }
                else
                {
                    list.Add(products[i]);
                    prodMod.Add(list);
                    newRow = true;
                }
            }

            return prodMod;
        }

        #endregion

        [HttpPost]
        public ActionResult QuickSearch(string code, string name)
        {
            var model = (from p in db.Products
                         where (code == null || code == string.Empty || p.Code == code)
                            && (name == null || name == string.Empty || p.Name.Contains(name))
                         select p).Include(p => p.Images).Take(Cons.QuickResults).ToList();

            return PartialView("_ProductList", model);
        }

        public ActionResult Detail(int id)
        {
            var branchId = User.Identity.GetBranchSession().Id;
            var userId   = User.Identity.GetUserId();

            var trans = db.Transactions.FirstOrDefault(t => t.BranchId == branchId 
            && t.UserId == userId &&  !t.IsCompleated && t.TransactionTypeId == (int)TransType.Sale);
           
            var model = db.Products.Include(p => p.Images).Include(p => p.Compatibilities).Include(p=> p.BranchProducts).
                        Include(p=> p.TransactionDetails).FirstOrDefault(p => p.ProductId == id);

            model.TransactionId = trans == null ? Cons.Zero : trans.TransactionId;

            //model.Quantity = model.TransactionDetails.
            //    Where(td => td.Transaction.IsCompleated && td.Transaction.BranchId == branchId).Sum(td => td.Quantity);

            var bProd = model.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);
            model.Quantity = bProd != null ? bProd.Stock : Cons.Zero;

            model.OrderCarModels();

            model.Branches = db.Branches.Include(b=> b.BranchProducts).ToList();
            var details = db.TransactionDetails.Include(td => td.Transaction).Where(td => td.ProductId == id).ToList();

            foreach (var branch in model.Branches)
            {
                var bpr = branch.BranchProducts.FirstOrDefault(bp => bp.ProductId == model.ProductId);
                branch.Quantity = bpr != null ? bpr.Stock : Cons.Zero;
                //branch.Quantity = details.Where(td => td.Transaction.BranchId == branch.BranchId && td.Transaction.IsCompleated).Sum(dt => dt.Quantity);
            }

            return View(model);
        }


        // GET: Products/Create
        public ActionResult Create(int? id)
        {
            ProductViewModel model;
            if (id == null)
                model = new ProductViewModel();
            else
            {
                var product = db.Products.Include(p => p.Images).Include(p => p.Compatibilities).FirstOrDefault(p => p.ProductId == id);
                model = new ProductViewModel(product);
            }

            model.Categories = db.Categories.ToSelectList();
            model.CarMakes = db.CarMakes.ToSelectList();

            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ProductId,Code,Name,Description,MinQuantity,BarCode,BuyPrice")] Product product,HttpPostedFileBase file)
        public ActionResult Create([Bind(Exclude = "Compatibilities")]Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductId == Cons.Zero)
                    db.Products.Add(product);
                else
                    db.Entry(product).State = EntityState.Modified;

                db.SaveChanges();

                int i = Cons.Zero;

                foreach (var c in product.NewCompatibilities)
                {
                    var mArr = c.Split('-');
                    var mId = Convert.ToInt32(mArr[Cons.Zero]);
                    var yIni = Convert.ToInt32(mArr[Cons.One]);
                    var yEnd = Convert.ToInt32(mArr[Cons.Two]) + Cons.One;

                    for (int j = yIni; j < yEnd; j++)
                    {
                        var year = db.CarYears.FirstOrDefault(y => y.Year == j && y.CarModelId == mId);

                        if (year == null)
                        {
                            year = new CarYear { Year = j, CarModelId = mId };
                            db.CarYears.Add(year);
                            db.SaveChanges();
                        }

                        Compatibility comp = new Compatibility { CarYearId = year.CarYearId, ProductId = product.ProductId };
                        db.Compatibilites.Add(comp);
                    }
                }

                db.SaveChanges();


                //Guardado Imagenes
                foreach (var file in product.Files)
                {
                    if (file != null)
                    {
                        ProductImage f = new ProductImage();

                        f.Path = FileManager.SaveImage(file, product.ProductId, ImageType.Products);
                        f.ProductId = product.ProductId;
                        f.Name = file.FileName;
                        f.Type = file.ContentType;
                        f.Size = file.ContentLength;

                        db.ProductImages.Add(f);

                        i++;
                    }
                }

                if (i > Cons.Zero)
                    db.SaveChanges();
            }

            return RedirectToAction("Create", new { id = product.ProductId });
        }

        [HttpPost]
        public ActionResult GetImages(int productId)
        {
            var model = db.ProductImages.Where(pi => pi.ProductId == productId);
            return PartialView("_ImagesLoaded", model);
        }

        [HttpPost]
        public ActionResult DeleteImage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var image = db.ProductImages.Find(id);
            int pId = image.ProductId;
            if (image == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            db.ProductImages.Remove(image);
            db.SaveChanges();

            var model = db.ProductImages.Where(i => i.ProductId == pId).ToList();

            ModelState.Clear();

            return PartialView("_ImagesLoaded", model);
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
