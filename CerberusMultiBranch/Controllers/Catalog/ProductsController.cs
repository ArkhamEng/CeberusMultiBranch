using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities;
using CerberusMultiBranch.Models.Entities.Catalog;
using System.IO;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Support;

namespace CerberusMultiBranch.Controllers.Catalog
{
    [Authorize]
    public class ProductsController : Controller
    {
        private ApplicationData db = new ApplicationData();

        public ActionResult Index()
        {
            var model = new SearchProductViewModel();
            model.Products = db.Products.Include(p => p.Images);
            model.Categories = db.Categories.ToSelectList();
            model.Makes = db.CarMakes.ToSelectList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(int? categoryId, int? carYear, string name, string code)
        {
            var model = (from p in db.Products
                         where (categoryId == null || p.CategoryId == categoryId)
                         && (name == null || p.Name.Contains(name))
                         && (code == null || p.Code == code)
                         && (carYear == null || p.Compatibilities.Where(c=> c.CarYearId == carYear).ToList().Count > 0)
                         select p
                         ).Include(p=> p.Images).ToList();

            //foreach (var prod in model)
            //{
            //    prod.Images = db.ProductImages.Where(p => p.ProductId == prod.ProductId).ToList();

            //    foreach (var image in prod.Images)
            //        image.File = GzipWrapper.Decompress(image.File);

            //}

            return PartialView("_List", model);
        }


        // GET: Products/Create
        public ActionResult Create(int? id)
        {
            ProductViewModel model;
            if (id == null)
                model = new ProductViewModel();
            else
            {
                var product = db.Products.Find(id);
                product.Images = db.ProductImages.Where(i => i.ProductId == product.ProductId).ToList();
                product.Compatibilities = db.Compatibilites.Where(c => c.ProductId == product.ProductId).ToList();
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

                foreach(var carYearId in product.AvailableModels)
                {
                    Compatibility comp = new Compatibility { CarYearId = carYearId, ProductId = product.ProductId };
                    db.Compatibilites.Add(comp);
                }
                db.SaveChanges();

                //Guardado Imagenes
                foreach (var file in product.Files)
                {
                    if (file != null)
                    {
                        ProductImage f = new ProductImage();

                        f.ProductId = product.ProductId;
                        f.Name = file.FileName;
                        f.Type = file.ContentType;
                        f.File = file.ToCompressedFile();
                        f.Size = f.File.Length;

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
            foreach (var img in model)
                img.File = GzipWrapper.Decompress(img.File);

            ModelState.Clear();

            return PartialView("_ImagesLoaded", model);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
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
