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

namespace CerberusMultiBranch.Controllers.Inventory
{
    public class ProductsController : Controller
    {
        private ApplicationData db = new ApplicationData();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5

        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId<int>();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            product.Images = db.ProductImages.Where(p => p.ProductId == product.ProductId).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create(int? id)
        {
            ProductViewModel model = new ProductViewModel();
            model.Categories = db.Categories.ToSelectList();
            return View(model);
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
                db.Products.Add(product);
                db.SaveChanges();

                int i = Cons.Zero;

                foreach (var file in product.Files)
                {
                    if (file != null)
                    {
                        using (MemoryStream target = new MemoryStream())
                        {
                            file.InputStream.CopyTo(target);

                            ProductImage f = new ProductImage();

                            f.ProductId = product.ProductId;
                            f.Name = file.FileName;
                            f.Type = file.ContentType;
                            f.File = target.ToArray();

                            db.ProductImages.Add(f);
                        }
                        i++;
                    }
                }

                if (i > Cons.Zero)
                    db.SaveChanges();

                return RedirectToAction("Create");
            }

            return View(product);
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Code,Name,Description,MinQuantity,BarCode,BuyPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
