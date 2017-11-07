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
            model.Products = LookFor(null, null, null, null, null, false);
            model.Systems = db.Systems.ToSelectList();
            model.Categories = db.Categories.ToSelectList();
            model.Makes = db.CarMakes.ToSelectList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public ActionResult BeginTransference(int branchId, int productId)
        {
            var branchP = db.BranchProducts.Include(bp => bp.Product).
                Include(bp => bp.Product.Images).Include(bp => bp.Branch).
                 FirstOrDefault(bp => bp.BranchId == branchId && bp.ProductId == productId);

            return PartialView("_Transference", branchP);
        }


        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public ActionResult BeginMovement(int productId)
        {
            var branchId = User.Identity.GetBranchId();

            var branchP = db.BranchProducts.Include(bp => bp.Product).
                         Include(bp => bp.Product.Images).Include(bp => bp.Branch).
                         FirstOrDefault(bp => bp.BranchId == branchId && bp.ProductId == productId);

            if (branchP == null)
            {
                branchP = new BranchProduct { BranchId = branchId, ProductId = productId, UpdDate = DateTime.Now };
                db.BranchProducts.Add(branchP);
                db.SaveChanges();

                branchP = db.BranchProducts.Include(bp => bp.Product).
                       Include(bp => bp.Product.Images).Include(bp => bp.Branch).
                       FirstOrDefault(bp => bp.BranchId == branchId && bp.ProductId == productId);
            }


            return PartialView("_Movement", branchP);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public JsonResult Move(int productId, MovementType type, double quantity)
        {
            var branchId = User.Identity.GetBranchId();

            var branchP = db.BranchProducts.Include(bp => bp.Product).
                         Include(bp => bp.Product.Images).Include(bp => bp.Branch).
                         Include(bp => bp.Product.Details).
                         FirstOrDefault(bp => bp.BranchId == branchId && bp.ProductId == productId);



            if (type == MovementType.Exit)
            {
                if (branchP.Stock < quantity)
                    return Json(new { Result = "Imposible realizar el movimiento", Message = "La cantidad a retirar supera la disponible" });
                else
                {
                    branchP.LastStock = branchP.Stock;
                    branchP.Stock -= quantity;
                    branchP.UpdDate = DateTime.Now;
                    branchP.StockMovements = branchP.StockMovements ?? new List<StockMovement>();

                    var mSm = new StockMovement
                    {
                        BranchId = branchP.BranchId,
                        ProductId = branchP.ProductId,
                        Quantity = quantity,
                        MovementType = MovementType.Exit,
                        User = User.Identity.Name,
                        MovementDate = DateTime.Now,
                        Comment = "Salida manual producto"
                    };

                    db.StockMovements.Add(mSm);

                    // si el producto es un paquete, debo regresar cada producto que lo complementa al stock individual
                    if (branchP.Product.ProductType == ProductType.Package)
                    {
                        foreach (var det in branchP.Product.Packages)
                        {

                            var dtBranchP = db.BranchProducts.Find(branchId, det.DetailtId);

                            dtBranchP.LastStock = dtBranchP.Stock;
                            dtBranchP.Stock += det.Quantity * quantity;

                            db.Entry(dtBranchP).State = EntityState.Modified;

                            //creo los reingresos del producto al inventario
                            var sm = new StockMovement
                            {
                                BranchId = dtBranchP.BranchId,
                                ProductId = dtBranchP.ProductId,
                                Quantity = det.Quantity,
                                MovementType = MovementType.Entry,
                                User = User.Identity.Name,
                                MovementDate = DateTime.Now,
                                Comment = "Ingreso por salida de disolución de paquete"
                            };
                        }
                    }
                }
            }
            else
            {
                branchP.LastStock = branchP.Stock;
                branchP.Stock += quantity;

                var mSm = new StockMovement
                {
                    BranchId = branchP.BranchId,
                    ProductId = branchP.ProductId,
                    Quantity = quantity,
                    MovementType = MovementType.Entry,
                    User = User.Identity.Name,
                    MovementDate = DateTime.Now,
                    Comment = "Entrada manual de producto"
                };

                db.StockMovements.Add(mSm);

                // si el producto es un paquete, debo regresar cada producto que lo complementa al stock individual
                if (branchP.Product.ProductType == ProductType.Package)
                {
                    foreach (var det in branchP.Product.Packages)
                    {
                        var dtBranchP = db.BranchProducts.Find(branchId, det.DetailtId);

                        //verifico si la cantidad en stock es suficiente para alimentar el paquete, si no lo es, 
                        //la operación concluye
                        if (dtBranchP == null || dtBranchP.Stock < (det.Quantity * quantity))
                        {
                            return Json(new
                            {
                                Result = "Imposible realizar el ingreso del paquete!",
                                Message = "No hay suficiente producto en inventario, revise la configuración del paquete para conocer las cantidades necesarias"
                            });
                        }

                        dtBranchP.LastStock = dtBranchP.Stock;
                        dtBranchP.Stock -= det.Quantity;

                        db.Entry(dtBranchP).State = EntityState.Modified;

                        //creo los reingresos del producto al inventario
                        var sm = new StockMovement
                        {
                            BranchId = dtBranchP.BranchId,
                            ProductId = dtBranchP.ProductId,
                            Quantity = det.Quantity,
                            MovementType = MovementType.Exit,
                            User = User.Identity.Name,
                            MovementDate = DateTime.Now,
                            Comment = "Salida para formar paquete"
                        };

                        db.StockMovements.Add(sm);
                    }
                }
            }

            try
            {
                db.Entry(branchP).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new
                {
                    Result = "OK",
                    Message = "Movimiento exitoso! se ha actualizado la cantidad de producto en inventario",
                    Code = branchP.Product.Code
                });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Ocurrio un error al guardar los datos", Message = ex.Message });
            }

        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public JsonResult Transfer(int originBranchId, int productId, double quantity)
        {
            var branchId = User.Identity.GetBranchId();

            var bp = db.BranchProducts.Include(br => br.Product).Include(br => br.Branch).
                FirstOrDefault(br => br.BranchId == originBranchId && br.ProductId == productId);

            if (bp.Stock < quantity)
            {
                return Json(new
                {
                    Result = "Imposible realizar la transferencia",
                    Message = "La cantidad solicitada es mayor a la disponible en sucursal"
                });
            }


            //Obtengo la relacion producto sucursal destino
            var myBp = db.BranchProducts.Find(branchId, productId);

            try
            {
                //si no existe la creo de lo contrario la actualizo
                if (myBp == null)
                {
                    myBp = new BranchProduct
                    {
                        Stock = quantity,
                        LastStock = Cons.Zero,
                        BranchId = branchId,
                        ProductId = productId,
                        UpdDate = DateTime.Now
                    };

                    db.BranchProducts.Add(myBp);
                }
                else
                {
                    myBp.LastStock = myBp.Stock;
                    myBp.Stock += quantity;
                    myBp.UpdDate = DateTime.Now;
                    db.Entry(myBp).State = EntityState.Modified;
                }

                //agrego los movimientos de Stock en la sucursal destino
                StockMovement smD = new StockMovement
                {
                    BranchId = myBp.BranchId,
                    ProductId = myBp.ProductId,
                    Comment = "Ingreso por transferencia",
                    MovementType = MovementType.Entry,
                    MovementDate = DateTime.Now,
                    User = User.Identity.Name,
                    Quantity = quantity
                };

                db.StockMovements.Add(smD);

                //agrego los movimientos de Stock en la sucursal origen
                StockMovement smO = new StockMovement
                {
                    BranchId = bp.BranchId,
                    ProductId = bp.ProductId,
                    Comment = "Salida por transferencia",
                    MovementType = MovementType.Exit,
                    MovementDate = DateTime.Now,
                    User = User.Identity.Name,
                    Quantity = quantity
                };

                db.StockMovements.Add(smO);


                //actualizo stock en sucursal de origen
                bp.LastStock = bp.Stock;
                bp.Stock -= quantity;
                db.Entry(bp).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new
                {
                    Result = "OK",
                    Message = "Transferencia exitosa!",
                    Code = bp.Product.Code
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error al transferir",
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult Search(int? categoryId, int? partSystemId, int? carYear, string name, string code, bool isGrid)
        {
            var model = LookFor(categoryId, partSystemId, carYear, name, code, isGrid);
            return PartialView("_List", model);
        }

        [HttpPost]
        public ActionResult GetStockInBranches(int productId)
        {
            var branches = db.Branches.Include(b => b.BranchProducts).ToList();

            foreach (var branch in branches)
            {
                var bp = branch.BranchProducts.FirstOrDefault(b => b.ProductId == productId);
                branch.Quantity = bp != null ? bp.Stock : 0;
            }


            return PartialView("_StockInBranches", branches);
        }

        private List<List<Product>> LookFor(int? categoryId, int? partSystemId, int? carYear, string name, string code, bool isGrid)
        {
            var branchId = User.Identity.GetBranchSession().Id;

            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');


            var products = (from p in db.Products.Include(p => p.Images).Include(p => p.Compatibilities).Include(p => p.BranchProducts)
                            where (categoryId == null || p.CategoryId == categoryId)
                            && (partSystemId == null || p.PartSystemId == partSystemId)
                            && (name == null || name == string.Empty || arr.All(s => (p.Code + "" + p.Name).Contains(s)))
                            //&& (code == null || code == string.Empty || p.Code == code)
                            && (carYear == null || p.Compatibilities.Where(c => c.CarYearId == carYear).ToList().Count > Cons.Zero)
                            select p).OrderBy(s=> s.Name).Take(400).ToList();

            //   products.ForEach(p => p.Quantity = p.BranchProducts.FirstOrDefault(bp=> bp.BranchId == branchId).Stock);
            foreach (var prod in products)
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
                foreach (var p in products)
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
        public ActionResult QuickSearch(string name)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');


            var model = (from p in db.Products
                         where
                           (name == null || name == string.Empty || arr.All(s => (p.Code + " " + p.Name).Contains(s)))
                         select p).Include(p => p.Images).Take(Cons.QuickResults).ToList();

            return PartialView("_ProductList", model);
        }

        [HttpPost]
        public ActionResult Detail(int id)
        {
            var branchId = User.Identity.GetBranchSession().Id;
            var userId = User.Identity.GetUserId();

            var trans = db.Sales.FirstOrDefault(s => s.BranchId == branchId && s.UserId == userId
            && s.Status == TranStatus.InProcess);

            var model = db.Products.Include(p => p.Images).Include(p => p.Compatibilities).
                        Include(p => p.BranchProducts).
                        FirstOrDefault(p => p.ProductId == id);

            model.TransactionId = (trans == null) ? Cons.Zero : trans.TransactionId;


            var bProd = model.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);
            model.Quantity = bProd != null ? bProd.Stock : Cons.Zero;

            model.OrderCarModels();

            model.Branches = db.Branches.Include(b => b.BranchProducts).ToList();
            var details = db.Sales.Include(s => s.TransactionDetails).Select(s => s.TransactionDetails.
             Where(td => td.ProductId == id)).ToList();

            foreach (var branch in model.Branches)
            {
                var bpr = branch.BranchProducts.FirstOrDefault(bp => bp.ProductId == model.ProductId);
                branch.Quantity = bpr != null ? bpr.Stock : Cons.Zero;
            }

            return PartialView("Detail", model);
        }


        // GET: Products/Create
        [Authorize(Roles = "Capturista")]
        public ActionResult Create(int? id)
        {
            ProductViewModel model;
            if (id == null)
            {
                var variables = db.Variables;
                model = new ProductViewModel();
                model.IsActive = true;
                model.MinQuantity = Cons.One;
                model.DealerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.DealerPercentage)).Value);
                model.StorePercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.StorePercentage)).Value);
                model.WholesalerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.WholesalerPercentage)).Value);
            }
            else
            {
                var product = db.Products.Include(p => p.Images).
                    Include(p => p.Details).
                    Include(p => p.Compatibilities).FirstOrDefault(p => p.ProductId == id);
                model = new ProductViewModel(product);
            }

            var categories = db.Categories.ToList();
            categories.ForEach(c => c.Name = c.SatCode + "-" + c.Name);

            model.Categories = categories.ToSelectList();
            model.CarMakes = db.CarMakes.ToSelectList();
            model.Systems = db.Systems.ToSelectList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddToPackage(int packagedId, int productId, double quantity)
        {

            var pd = db.PackageDetails.FirstOrDefault(p=> p.PackageId == packagedId && p.DetailtId == productId);

            if(pd == null)
            {
                PackageDetail d = new PackageDetail { PackageId = packagedId, DetailtId = productId, Quantity = quantity };
                db.PackageDetails.Add(d);
            }
            else
            {
                pd.Quantity += quantity;
                db.Entry(pd).State = EntityState.Modified;
            }

           
            db.SaveChanges();

            var model = db.PackageDetails.Include(pkD => pkD.Detail).
                Where(pkD => pkD.PackageId == packagedId).ToList();

            return PartialView("_PackageDetails", model);

        }


        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Capturista")]
        //public ActionResult Create([Bind(Include = "ProductId,Code,Name,Description,MinQuantity,BarCode,BuyPrice")] Product product,HttpPostedFileBase file)
        public ActionResult Create([Bind(Exclude = "Compatibilities")]Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    product.UpdDate = DateTime.Now;
                    product.UpdUser = User.Identity.Name;

                    if (product.ProductId == Cons.Zero)
                    {
                        db.Products.Add(product);
                        db.SaveChanges();
                    }
                    else
                        db.Entry(product).State = EntityState.Modified;

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
                                //db.SaveChanges();
                            }

                            //Compatibility comp = new Compatibility { CarYearId = year.CarYearId, ProductId = product.ProductId };
                            //db.Compatibilites.Add(comp);
                        }
                    }

                    //db.SaveChanges();

                    //Guardado Imagenes
                    foreach (var file in product.Files)
                    {
                        if (file != null)
                        {
                            ProductImage f = new ProductImage();

                            f.Path = FileManager.SaveImage(file, product.ProductId.ToString(), ImageType.Products);
                            f.ProductId = product.ProductId;
                            f.Name = file.FileName;
                            f.Type = file.ContentType;
                            f.Size = file.ContentLength;

                            db.ProductImages.Add(f);

                            i++;
                        }
                    }

                    //if (i > Cons.Zero)
                        db.SaveChanges();
                }
                catch (Exception ex)
                {

                    ViewBag.Header = "Error al guardar";
                    ViewBag.Message = "Ocurrio un error al guardo los datos del producto detail:" + ex.Message + " inner exception" + ex.InnerException.Message;
                    var model = new ProductViewModel(product);

                    model.Images = new List<ProductImage>();
                    model.Compatibilities = new List<Compatibility>();
                    model.Categories = db.Categories.ToSelectList();
                    model.CarMakes = db.CarMakes.ToSelectList();
                    model.Systems = db.Systems.ToSelectList();
                    

                    return View(model);
                }
            }

            return RedirectToAction("Create", new { id = product.ProductId });
        }


        [HttpPost]
        public ActionResult SearchForPackage(string filter)
        {
            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
                arr = filter.Trim().Split(' ');

            var products = (from ep in db.Products.Include(p => p.Category)
                            where (filter == null || filter == string.Empty || arr.All(s => (ep.Code + "" + ep.Name).Contains(s)))
                            && (ep.ProductType == ProductType.Single)
                            select ep).Take((int)Cons.OneHundred).ToList();

            return PartialView("_ListForPackage", products);
        }

        [HttpPost]
        public ActionResult GetImages(int productId)
        {
            var model = db.ProductImages.Where(pi => pi.ProductId == productId);
            return PartialView("_ImagesLoaded", model);
        }

        [HttpPost]
        [Authorize(Roles = "Capturista")]
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

            var pPath = Server.MapPath(image.Path);
            System.IO.File.Delete(pPath);


            db.ProductImages.Remove(image);
            db.SaveChanges();

            var model = db.ProductImages.Where(i => i.ProductId == pId).ToList();

            ModelState.Clear();

            return PartialView("_ImagesLoaded", model);
        }

        [HttpPost]
        public ActionResult SearchForSale(int? categoryId, int? carYear, string name)
        {
            var branchId = User.Identity.GetBranchSession().Id;

            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var products = (from p in db.Products.Include(p => p.Images).Include(p => p.Compatibilities).
                                     Include(p => p.BranchProducts)

                            where (categoryId == null || p.CategoryId == categoryId)
                               && (name == null || name == string.Empty || arr.All(s => (p.Code + "" + p.Name).Contains(s)))
                               && (carYear == null || p.Compatibilities.Where(c => c.CarYearId == carYear).ToList().Count > Cons.Zero)
                            select p).Take(1000).ToList();

            foreach (var prod in products)
            {
                var bp = prod.BranchProducts.FirstOrDefault(b => b.BranchId == branchId);
                prod.Quantity = bp != null ? bp.Stock : Cons.Zero;
            }

            return PartialView("_ListForSale", products);
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
