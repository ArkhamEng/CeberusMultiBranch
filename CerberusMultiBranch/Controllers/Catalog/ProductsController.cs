using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities;
using CerberusMultiBranch.Models.Entities.Inventory;
using Microsoft.AspNet.Identity;
using System.IO;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Support;
using System.IO.Compression;

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
        public ActionResult Search(int? categoryId, int? subCatergoryId, string name, string code)
        {
            var model = (from p in db.Products
                         where (categoryId == null || p.CategoryId == categoryId)
                         &&    (name == string.Empty || p.Name.Contains(name))
                         &&    (code == string.Empty || p.Code == code)
                         select p
                         ).ToList();

            foreach (var prod in model)
            {
                prod.Images = db.ProductImages.Where(p => p.ProductId == prod.ProductId).ToList();

                foreach (var image in prod.Images)
                    image.File = GzipWrapper.Decompress(image.File);

            }

            return PartialView("_List", model);
        }


        // GET: Products/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                ProductViewModel model = new ProductViewModel();
                model.Categories = db.Categories.ToSelectList();
                return View(model);
            }
            else
            {
                var product = db.Products.Find(id);
                product.Images = db.ProductImages.Where(i => i.ProductId == product.ProductId).ToList();

                ProductViewModel model = new ProductViewModel(product);
                model.Categories = db.Categories.ToSelectList();
                
                return View(model);
            }
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ProductId,Code,Name,Description,MinQuantity,BarCode,BuyPrice")] Product product,HttpPostedFileBase file)
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductId == Cons.Zero)
                    db.Products.Add(product);
                else
                    db.Entry(product).State = EntityState.Modified;

                db.SaveChanges();

                int i = Cons.Zero;

                foreach (var file in product.Files)
                {
                    if (file != null)
                    {
                        using (MemoryStream target = new MemoryStream())
                        {

                            file.InputStream.CopyTo(target);
                            var bArr = target.ToArray();

                            ProductImage f = new ProductImage();

                            f.ProductId = product.ProductId;
                            f.Name = file.FileName;
                            f.Type = file.ContentType;
                            f.File = GzipWrapper.Compress(bArr);

                            db.ProductImages.Add(f);
                        }
                        i++;
                    }
                }

                if (i > Cons.Zero)
                    db.SaveChanges();
            }

            return RedirectToAction("Create",new { id = product.ProductId });
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
