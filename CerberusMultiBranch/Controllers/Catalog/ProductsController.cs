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
using System.Text.RegularExpressions;
using System.Web;

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
            model.Products = new List<List<Product>>(); //LookFor(null, null, null, null,null,null, null, false);
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
                         Include(bp => bp.Product.PackageDetails).
                         FirstOrDefault(bp => bp.BranchId == branchId && bp.ProductId == productId);

            if (type == MovementType.Exit)
            {
                if (branchP.Stock < quantity)
                    return Json(new
                    {
                        Result = "Imposible realizar el movimiento",
                        Message = "La cantidad a retirar supera la disponible"
                    });
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
                        foreach (var det in branchP.Product.PackageDetails)
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
                    foreach (var det in branchP.Product.PackageDetails)
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
        public ActionResult Search(int? categoryId, int? partSystemId, int? carYear, int? carModelId, int? carMakeId, string name, string code, bool isGrid)
        {
            var model = LookFor(categoryId, partSystemId, carYear, carModelId, carMakeId, name, code, isGrid);
            return PartialView("_List", model);
        }

        [HttpPost]
        public ActionResult SearchEdit(string code)
        {
            var model = LookFor(null, null, null, null, null, code, null, false);
            return PartialView("_List", model);
        }

        [HttpPost]
        public ActionResult ExternalSearch(string filter)
        {
            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
                arr = filter.Trim().Split(' ');

            var products = (from ep in db.ExternalProducts.Include(ep => ep.Provider)
                            join eq in db.Equivalences.Include(e => e.Product)
                            on new { ep.ProviderId, ep.Code } equals new { eq.ProviderId, eq.Code } into gj
                            from x in gj.DefaultIfEmpty()

                            where (filter == null || filter == string.Empty || arr.All(s => (ep.Code + "" + ep.Description).Contains(s)))
                            select new
                            {
                                ProviderId = ep.ProviderId,
                                Code = ep.Code,
                                Description = ep.Description,
                                InternalCode = gj.FirstOrDefault().Product.Code,
                                Price = ep.Price,
                                TradeMark = ep.TradeMark,
                                Unit = ep.Unit,
                                ProductId = (int?)gj.FirstOrDefault().ProductId ?? Cons.Zero,
                                ProviderName = ep.Provider.Name
                            }).Take((int)Cons.OneHundred).ToList();

            var model = products.Select(ep => new ExternalProduct
            {
                ProviderId = ep.ProviderId,
                Code = ep.Code,
                Description = ep.Description,
                InternalCode = ep.InternalCode,
                Price = ep.Price,
                TradeMark = ep.TradeMark,
                Unit = ep.Unit,
                ProductId = ep.ProductId,
                ProviderName = ep.ProviderName
            }).ToList();

            return PartialView("_ProviderProducts", model);
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

        private List<List<Product>> LookFor(int? categoryId, int? partSystemId, int? carYear, int? carModel, int? carMake, string name, string code, bool isGrid)
        {
            var branchId = User.Identity.GetBranchId();

            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
            {
                arr = name.Trim().Split(' ');
                code = arr.FirstOrDefault();
            }

            List<Product> products = new List<Product>();

            if (code != null)
            {
                products = (from p in db.Products.Include(p => p.Images).Include(p => p.Compatibilities).Include(p => p.BranchProducts)
                             .Include(p => p.Compatibilities.Select(c => c.CarYear)).Include(p => p.Compatibilities.Select(c => c.CarYear.CarModel))
                            where (p.Code == code)
                            select p).ToList();
            }

            if (products.Count == Cons.Zero)
            {
                products = (from p in db.Products.Include(p => p.Images).Include(p => p.Compatibilities).Include(p => p.BranchProducts)
                               .Include(p => p.Compatibilities.Select(c => c.CarYear)).Include(p => p.Compatibilities.Select(c => c.CarYear.CarModel))
                            where (categoryId == null || p.CategoryId == categoryId)
                            && (partSystemId == null || p.PartSystemId == partSystemId)
                            && (name == null || name == string.Empty || arr.All(s => (p.Code+" "+p.Name + " " + p.TradeMark).Contains(s)))

                            && (carYear == null || p.Compatibilities.Where(c => c.CarYearId == carYear).ToList().Count > Cons.Zero)
                            && (carModel == null || p.Compatibilities.Where(c => c.CarYear.CarModelId == carModel).ToList().Count > Cons.Zero)
                            && (carMake == null || p.Compatibilities.Where(c => c.CarYear.CarModel.CarMakeId == carMake).ToList().Count > Cons.Zero)
                            && (p.IsActive)
                            select p).OrderBy(s => s.Name).Take(400).ToList();
            }
            //   products.ForEach(p => p.Quantity = p.BranchProducts.FirstOrDefault(bp=> bp.BranchId == branchId).Stock);
            foreach (var prod in products)
            {
                var bp = prod.BranchProducts.FirstOrDefault(b => b.BranchId == branchId);
                prod.Quantity = bp != null ? bp.Stock : Cons.Zero;
                prod.StorePercentage = bp != null ? bp.StorePercentage : Cons.Zero;
                prod.DealerPercentage = bp != null ? bp.DealerPercentage : Cons.Zero;
                prod.WholesalerPercentage = bp != null ? bp.WholesalerPercentage : Cons.Zero;
                prod.StorePrice = bp != null ? bp.StorePrice : Cons.Zero;
                prod.DealerPrice = bp != null ? bp.DealerPrice : Cons.Zero;
                prod.WholesalerPrice = bp != null ? bp.WholesalerPrice : Cons.Zero;
                prod.Row = bp != null ? bp.Row : string.Empty;
                prod.Row = bp != null ? bp.Ledge : string.Empty;
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
                           (name == null || name == string.Empty || arr.All(s => (p.Code + " " + p.Name).Contains(s)) && p.IsActive)
                         select p).Include(p => p.Images).Take(Cons.QuickResults).ToList();

            return PartialView("_ProductList", model);
        }

        [HttpPost]
        public ActionResult Detail(int id)
        {
            var branchId = User.Identity.GetBranchId();
            var userId = User.Identity.GetUserId();

            var trans = db.Sales.FirstOrDefault(s => s.BranchId == branchId && s.UserId == userId
            && s.Status == TranStatus.InProcess);

            var model = db.Products.Include(p => p.Images).Include(p => p.Compatibilities).
                        Include(p => p.BranchProducts).
                        FirstOrDefault(p => p.ProductId == id);

            model.TransactionId = (trans == null) ? Cons.Zero : trans.TransactionId;


            var bProd = model.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);
            model.Quantity = bProd != null ? bProd.Stock : Cons.Zero;
            model.StorePercentage = bProd != null ? bProd.StorePercentage : Cons.Zero;
            model.DealerPercentage = bProd != null ? bProd.DealerPercentage : Cons.Zero;
            model.WholesalerPercentage = bProd != null ? bProd.WholesalerPercentage : Cons.Zero;
            model.StorePrice = bProd != null ? bProd.StorePrice : Cons.Zero;
            model.DealerPrice = bProd != null ? bProd.DealerPrice : Cons.Zero;
            model.WholesalerPrice = bProd != null ? bProd.WholesalerPrice : Cons.Zero;

            model.OrderCarModels();

            //Obtengo existencias en otras sucursales
            model.Branches = db.Branches.ToList();

            //var details = db.Sales.Include(s => s.TransactionDetails).Select(s => s.TransactionDetails.
            // Where(td => td.ProductId == id)).ToList();

            foreach (var branch in model.Branches)
            {
                var bpr = db.BranchProducts.FirstOrDefault(bp => bp.ProductId == model.ProductId
                && bp.BranchId == branch.BranchId);

                branch.Quantity = bpr != null ? bpr.Stock : Cons.Zero;
            }

            return PartialView("Detail", model);
        }


        // GET: Products/Create
        [Authorize(Roles = "Capturista")]
        public ActionResult Create1(int? id)
        {
            var branchId = User.Identity.GetBranchId();

            var cats = new List<Category>();
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
                var product = db.Products.Include(p => p.Images).Include(p => p.BranchProducts).
                    Include(p => p.PackageDetails).
                    Include(p => p.Compatibilities).FirstOrDefault(p => p.ProductId == id);

                var bp = product.BranchProducts.FirstOrDefault(b => b.BranchId == branchId);
                product.Quantity = bp != null ? bp.Stock : Cons.Zero;
                product.BuyPrice = bp != null ? bp.BuyPrice : Cons.Zero;
                product.StorePercentage = bp != null ? bp.StorePercentage : Cons.Zero;
                product.DealerPercentage = bp != null ? bp.DealerPercentage : Cons.Zero;
                product.WholesalerPercentage = bp != null ? bp.WholesalerPercentage : Cons.Zero;
                product.StorePrice = bp != null ? bp.StorePrice : Cons.Zero;
                product.DealerPrice = bp != null ? bp.DealerPrice : Cons.Zero;
                product.WholesalerPrice = bp != null ? bp.WholesalerPrice : Cons.Zero;
                product.Row = bp != null ? bp.Row : string.Empty;
                product.Row = bp != null ? bp.Ledge : string.Empty;

                if (product.PartSystemId > Cons.Zero)
                {
                    cats = db.SystemCategories.Where(sc => sc.PartSystemId == product.PartSystemId).
                        Select(sc => sc.Category).OrderBy(c => c.Name).ToList();

                    cats.ForEach(c => c.Name += " | " + c.SatCode);
                }

                model = new ProductViewModel(product);
            }

            model.Categories = cats.ToSelectList();
            model.CarMakes = db.CarMakes.ToSelectList();
            model.Systems = db.Systems.ToSelectList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Capturista")]
        public ActionResult AddToPackage(int packagedId, int productId, double quantity)
        {
            // busco el producto padre (paquete)
            var pck = db.Products.Include(p => p.BranchProducts).
                Include(p => p.PackageDetails).
                FirstOrDefault(p => p.ProductId == packagedId);

            //si ya ha sido ingresado en alguna sucursal, verifico que no haya existencia
            //actualmente.
            if (pck.BranchProducts != null)
            {
                foreach (var bp in pck.BranchProducts)
                {
                    //si el hay existencia en alguna sucursal, no se puede modificar la composición
                    if (bp.Stock > Cons.Zero)
                    {
                        return Json(new
                        {
                            Result = "Imposible modificar el paquete",
                            Message = "La composición del paquete no se puede alterar " +
                            "ya que aun hay existencia de este es una o mas sucursales. " +
                            "Debes eliminar el paquete de todos los inventarios para poder modificarlo"
                        });
                    }
                }
            }

            var pd = pck.PackageDetails.FirstOrDefault(pkd => pkd.DetailtId == productId);

            if (pd == null)
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

        [HttpPost]
        [Authorize(Roles = "Capturista")]
        public ActionResult SaveImage()
        {
            var productId = System.Web.HttpContext.Current.Request.Form["productId"].ToInt();
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var file = System.Web.HttpContext.Current.Request.Files["image"];


                if (file.ContentLength > 0)
                {
                    if (file != null)
                    {
                        ProductImage f = new ProductImage();

                        f.Path = FileManager.SaveImage(file, productId.ToString(), ImageType.Products);
                        f.ProductId = productId;
                        f.Name = file.FileName;
                        f.Type = file.ContentType;
                        f.Size = file.ContentLength;

                        db.ProductImages.Add(f);
                        db.SaveChanges();
                    }
                }
            }

            var images = db.ProductImages.Where(i => i.ProductId == productId);
            return PartialView("_ImagesLoaded", images);
        }

        [HttpPost]
        [Authorize(Roles = "Capturista")]
        public ActionResult Edit(int? productId)
        {
            ProductViewModel vm;
            //si el Id tiene valor busco el producto
            if (productId != null)
            {
                var branchId = User.Identity.GetBranchId();

                var product = db.Products.Where(p => p.ProductId == productId).
                    Include(p => p.BranchProducts).
                    Include(p => p.Compatibilities).
                    Include(p => p.Images).
                    Include(p => p.PackageDetails).FirstOrDefault();

                //busco los datos del stock en sucursal
                var bp = product.BranchProducts.FirstOrDefault(b => b.BranchId == branchId);

                product.Quantity = bp != null ? bp.Stock : Cons.Zero;
                product.StorePercentage = bp != null ? bp.StorePercentage : Cons.Zero;
                product.DealerPercentage = bp != null ? bp.DealerPercentage : Cons.Zero;
                product.WholesalerPercentage = bp != null ? bp.WholesalerPercentage : Cons.Zero;

                product.BuyPrice = bp != null ? bp.BuyPrice : Cons.Zero;
                product.StorePrice = bp != null ? bp.StorePrice : Cons.Zero;
                product.DealerPrice = bp != null ? bp.DealerPrice : Cons.Zero;
                product.WholesalerPrice = bp != null ? bp.WholesalerPrice : Cons.Zero;

                vm = new ProductViewModel(product);
            }
            else
                vm = new ProductViewModel();

            var variables = db.Variables;

            //si el porcentaje de venta en mostrador viene en 0, ocupo los porcentajes por defecto
            if (vm.StorePercentage <= Cons.Zero)
            {
                vm.DealerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.DealerPercentage)).Value);
                vm.StorePercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.StorePercentage)).Value);
                vm.WholesalerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.WholesalerPercentage)).Value);
            }

            //si la unidad viene vacía ocupo el valor por defecto
            if (vm.Unit == null || vm.Unit == string.Empty)
                vm.Unit = variables.FirstOrDefault(v => v.Name == nameof(Product.Unit)).Value;

            List<Category> cats = new List<Category>();

            //si hay un sistema seleccionado busco sus categorías
            if (vm.PartSystemId > Cons.Zero)
            {
                cats = db.SystemCategories.Where(sc => sc.PartSystemId == vm.PartSystemId).
                    Select(sc => sc.Category).OrderBy(c => c.Name).ToList();

                cats.ForEach(c => c.Name += " | " + c.SatCode);
            }

            //asigno la lista de categorias y sistemas al view model
            vm.Categories = cats.ToSelectList();
            vm.Systems = db.Systems.ToSelectList();

            //si es un produto existente, busco la lista de Armadoras de vehiculos, para el view model
            if (vm.ProductId > Cons.Zero)
                vm.CarMakes = db.CarMakes.ToSelectList();

            return PartialView("_QuickAddProduct", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Capturista")]
        public ActionResult BeginCopy(int providerId, string code)
        {
            var variables = db.Variables;
            var ep = db.ExternalProducts.Find(providerId, code);

            ProductViewModel vm = new ProductViewModel();

            vm.IsActive = true;
            vm.Categories = new List<Category>().ToSelectList();//cats.ToSelectList();
            vm.Systems = db.Systems.ToSelectList();
            vm.Name = ep.Description;
            vm.TradeMark = ep.TradeMark;
            vm.Unit = ep.Unit;
            vm.BuyPrice = ep.Price;
            vm.MinQuantity = Cons.One;
            vm.Code = ep.Code;//Regex.Replace(ep.Code, @"^[a-zA-Z0-9]+$", "");
            vm.DealerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.DealerPercentage)).Value);
            vm.StorePercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.StorePercentage)).Value);
            vm.WholesalerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.WholesalerPercentage)).Value);

            vm.DealerPrice = Math.Round(vm.BuyPrice * (Cons.One + (vm.DealerPercentage / Cons.OneHundred)), Cons.Zero);
            vm.StorePrice = Math.Round(vm.BuyPrice * (Cons.One + (vm.StorePercentage / Cons.OneHundred)), Cons.Zero);
            vm.WholesalerPrice = Math.Round(vm.BuyPrice * (Cons.One + (vm.WholesalerPrice / Cons.OneHundred)), Cons.Zero);

            return PartialView("_QuickAddProduct", vm);
        }


        private void SaveSingle(Product product)
        {
            var branchId = User.Identity.GetBranchId();

            //si es producto nuevo lo guardo de inmediato para generar un ID
            if (product.ProductId == Cons.Zero)
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
            else
                db.Entry(product).State = EntityState.Modified;

            var branchP = db.BranchProducts.FirstOrDefault(bp => bp.ProductId == product.ProductId
                                                            && bp.BranchId == branchId);

            //si no existe una realacion con sucursal la creo con las unidades ingresadas
            if (branchP == null)
            {
                branchP = new BranchProduct
                {
                    ProductId = product.ProductId,
                    BranchId = User.Identity.GetBranchId(),
                    Stock = product.Quantity,
                    LastStock = Cons.Zero,
                    UpdDate = DateTime.Now.ToLocal(),
                    BuyPrice = product.BuyPrice,
                    DealerPercentage = product.DealerPercentage,
                    StorePercentage = product.StorePercentage,
                    WholesalerPercentage = product.WholesalerPercentage,
                    StorePrice = product.StorePrice,
                    DealerPrice = product.DealerPrice,
                    WholesalerPrice = product.WholesalerPrice
                };

                db.BranchProducts.Add(branchP);

                if (product.Quantity > Cons.Zero)
                {
                    var sm = new StockMovement
                    {
                        BranchId = branchId,
                        ProductId = product.ProductId,
                        User = User.Identity.Name,
                        MovementDate = DateTime.Now.ToLocal(),
                        Comment = "Ingreso Inicial Manual",
                        MovementType = MovementType.Entry,
                        Quantity = product.Quantity
                    };

                    db.StockMovements.Add(sm);
                }
            }
            //si la relacion de sucursal ya existe actualizo
            else
            {
                branchP.BuyPrice = product.BuyPrice;
                branchP.DealerPercentage = product.DealerPercentage;
                branchP.DealerPrice = product.DealerPrice;
                branchP.Ledge = product.Ledge;
                branchP.Row = product.Row;
                branchP.StorePercentage = product.StorePercentage;
                branchP.StorePrice = product.StorePrice;
                branchP.UpdDate = DateTime.Now.ToLocal();
                branchP.WholesalerPercentage = product.WholesalerPercentage;
                branchP.WholesalerPrice = product.WholesalerPrice;

                //si la cantidad indicada es diferente a la cantidad en stock agrego el movimiento requerido 
                if (product.Quantity > branchP.Stock || product.Quantity < branchP.Stock)
                {
                    double units;
                    branchP.LastStock = branchP.Stock;

                    var sm = new StockMovement
                    {
                        BranchId = branchId,
                        ProductId = product.ProductId,
                        User = User.Identity.Name,
                        MovementDate = DateTime.Now.ToLocal(),
                        Comment = "Movimiento manual",
                    };

                    if (product.Quantity > branchP.Stock)
                    {
                        sm.MovementType = MovementType.Entry;
                        units = product.Quantity - branchP.Stock;
                        branchP.Stock += units;
                    }
                    else
                    {
                        sm.MovementType = MovementType.Exit;
                        units = branchP.Stock - product.Quantity;
                        branchP.Stock -= units;
                    }

                    sm.Quantity = units;
                    db.StockMovements.Add(sm);
                }
            }

            db.SaveChanges();
        }

        [HttpPost]
        [Authorize(Roles = "Capturista")]
        public ActionResult QuickSave(Product product)
        {
            try
            {
                product.UpdDate = DateTime.Now;
                product.UpdUser = User.Identity.GetUserName();

                //si el producto es nuevo
                if (product.ProductId == Cons.Zero)
                {
                    var pc = db.Products.Where(p => p.Code == product.Code.Trim()).Count();

                    if (pc == Cons.Zero)
                    {
                        switch (product.ProductType)
                        {
                            case ProductType.Single:
                                SaveSingle(product);
                                break;
                            case ProductType.Package:
                                if (product.Quantity > Cons.Zero)
                                {
                                    return Json(new
                                    {
                                        Result = "Cantidad no autorizada",
                                        Message = "No es posible agregar una cantidad inicial mayor a cero a un paquete " +
                                                  "sin haber configurado al menos un producto en su contenido"
                                    });
                                }
                                SavePackage(product);
                                break;
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            Result = "El código de producto ya existe",
                            Message = "Se aplico formato y se encontro que ya existe el código de producto " + product.Code
                        });
                    }

                    return Edit(product.ProductId);
                }
                else
                {
                    switch (product.ProductType)
                    {
                        case ProductType.Single:
                            SaveSingle(product);
                            break;
                        case ProductType.Package:
                            SavePackage(product);
                            break;
                    }

                    return Json(new
                    {
                        Result = "OK",
                        Message = "Los cambios al producto se aplicaron correctamente " + product.Code
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al guardar el producto", Message = ex.Message });
            }

        }

        private void SavePackage(Product package)
        {
            bool isNewProduct, isNewBp = false;
            var branchId = User.Identity.GetBranchId();

            isNewProduct = package.ProductId == Cons.Zero;
            //si es un paquete nuevo lo guardo inmediatamente para generar un Id
            if (isNewProduct)
            {
                isNewProduct = true;
                db.Products.Add(package);
                db.SaveChanges();
            }
            else
            {
                if (package.Quantity > Cons.Zero && db.PackageDetails.Where(pd => pd.PackageId == package.ProductId).Count() == Cons.Zero)
                {
                    throw new Exception("No es posible agregar una cantidad  mayor a cero a un paquete " +
                                                 "sin haber configurado al menos un producto en su contenido");
                }
                db.Entry(package).State = EntityState.Modified;
            }


            //consulto la relación sucursal-producto
            var branchP = db.BranchProducts.Include(bp => bp.Product).
                Include(bp => bp.Product.PackageDetails)
                .FirstOrDefault(bp => bp.BranchId == branchId && bp.ProductId == package.ProductId);

            isNewBp = branchP == null;

            //si no hay una relación la creo
            if (isNewBp)
            {
                branchP = new BranchProduct
                {
                    BranchId = branchId,
                    ProductId = package.ProductId,
                };
            }

            branchP.BuyPrice = package.BuyPrice;
            branchP.DealerPercentage = package.DealerPercentage;
            branchP.DealerPrice = package.DealerPrice;
            branchP.Ledge = package.Ledge;
            branchP.Row = package.Row;
            branchP.StorePercentage = package.StorePercentage;
            branchP.StorePrice = package.StorePrice;
            branchP.UpdDate = package.UpdDate;
            branchP.WholesalerPercentage = package.WholesalerPercentage;
            branchP.WholesalerPrice = package.WholesalerPrice;

            //si es un paquete  nuevo agrego la relacion a la sucursal en turno
            if (isNewProduct)
            {
                db.BranchProducts.Add(branchP);
                db.SaveChanges();
            }
            //si el paquete ya existe 
            else
            {
                //si el paquete existe pero no tiene relación con la sucursal, la creo
                if (isNewBp)
                {
                    db.BranchProducts.Add(branchP);
                    db.SaveChanges();

                    branchP = db.BranchProducts.Include(bp => bp.Product).Include(bp => bp.Product.PackageDetails)
                        .FirstOrDefault(bp => bp.BranchId == branchId && bp.ProductId == package.ProductId);
                }

                //busco si ya tiene detalles (productos asignados al paquete)
                var products = db.PackageDetails.Where(pd => pd.PackageId == package.ProductId).ToList();

                //si la cantidad indicada es igual a la cantidad en stock, hago una actualizacón de precios y porcentajes
                if (package.Quantity == branchP.Stock)
                {
                    db.Entry(branchP).State = EntityState.Modified;
                    db.SaveChanges();
                }
                    

                //si hay diferencia
                else
                {
                    var ids = branchP.Product.PackageDetails.Select(pd => pd.DetailtId).ToList();
                    var stockList = db.BranchProducts.Where(bp => ids.Contains(bp.ProductId) && bp.BranchId == branchId).ToList();

                    double amount = Cons.Zero;
                    //si la cantidad ingresada es mayor a la existente, debo registrar un ingreso del paquete
                    if (package.Quantity > branchP.Stock)
                    {
                        var packageAmount = package.Quantity - branchP.Stock;

                        foreach (var prod in products)
                        {
                            amount = packageAmount * prod.Quantity;

                            var stDete = stockList.FirstOrDefault(s => s.ProductId == prod.DetailtId);

                            if (stDete != null && stDete.Stock >= amount)
                            {
                                stDete.Stock -= amount;
                                stDete.Reserved += amount;
                                stDete.UpdDate = DateTime.Now.ToLocal();

                                StockMovement sm = new StockMovement
                                {
                                    BranchId = branchId,
                                    ProductId = prod.DetailtId,
                                    Comment = "Reservado para paquete",
                                    Quantity = amount,
                                    MovementDate = DateTime.Now.ToLocal(),
                                    User = User.Identity.Name,
                                    MovementType = MovementType.Reservation
                                };
                                db.Entry(stDete).State = EntityState.Modified;
                                db.StockMovements.Add(sm);
                            }
                            else
                                throw new Exception("No hay cantidad suficiente de producto para agregar el paquete, revise la configuración");
                        }
                       
                        branchP.LastStock = branchP.Stock;
                        branchP.Stock = package.Quantity;

                        StockMovement pSm = new StockMovement
                        {
                            BranchId = branchId,
                            ProductId = package.ProductId,
                            Comment = "Ingreso  manual",
                            Quantity = packageAmount,
                            MovementDate = DateTime.Now.ToLocal(),
                            User = User.Identity.Name,
                            MovementType = MovementType.Entry
                        };
                        db.StockMovements.Add(pSm);

                    }
                    else
                    {
                        var packageAmount = branchP.Stock - package.Quantity;
                        foreach (var prod in products)
                        {
                            amount = packageAmount * prod.Quantity;

                            var stDete = stockList.FirstOrDefault(s => s.ProductId == prod.DetailtId);

                            if (stDete != null)
                            {
                                stDete.Stock += amount;
                                stDete.Reserved -= amount;
                                stDete.UpdDate = DateTime.Now.ToLocal();

                                StockMovement sm = new StockMovement
                                {
                                    BranchId = branchId,
                                    ProductId = prod.DetailtId,
                                    Comment = "Disolución de paquete",
                                    Quantity = amount,
                                    MovementDate = DateTime.Now.ToLocal(),
                                    User = User.Identity.Name,
                                    MovementType = MovementType.Release
                                };
                                db.Entry(stDete).State = EntityState.Modified;
                                db.StockMovements.Add(sm);
                            }
                            else
                            {
                                throw new Exception("Un producto del paquete no existe en la sucursal");
                            }
                        }

                      
                        branchP.LastStock = branchP.Stock;
                        branchP.Stock = package.Quantity;

                        StockMovement pSm = new StockMovement
                        {
                            BranchId = branchId,
                            ProductId = package.ProductId,
                            Comment = "Salida  manual",
                            Quantity = packageAmount,
                            MovementDate = DateTime.Now.ToLocal(),
                            User = User.Identity.Name,
                            MovementType = MovementType.Exit
                        };
                        db.StockMovements.Add(pSm);
                    }
                    //si todo se cumple guardo los cambios
                    db.Entry(branchP).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        [HttpPost]
        public ActionResult Copy(Product product, int providerId, string code)
        {
            product.UpdDate = DateTime.Now;
            product.UpdUser = User.Identity.GetUserName();
            product.IsActive = true;
            try
            {
                var pc = db.Products.Where(p => p.Code == product.Code.Trim()).Count();

                if (pc == Cons.Zero)
                {
                    //creo la equivalencia
                    SaveSingle(product);
                    var eq = new Equivalence { ProviderId = providerId, Code = code, ProductId = product.ProductId };
                    db.Equivalences.Add(eq);
                    db.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        Result = "El código de producto ya existe",
                        Message = "Se aplico formato y se encontro que ya existe el código de producto " + product.Code
                    });
                }

                return Edit(product.ProductId);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error al asociar el producto",
                    Message = ex.Message
                });
            }
        }


        [HttpPost]
        public ActionResult RemoveFromPackage(int packageId, int productId)
        {
            // busco el producto padre (paquete)
            var pck = db.Products.Include(p => p.BranchProducts).
                Include(p => p.PackageDetails).
                FirstOrDefault(p => p.ProductId == packageId);

            //reviso si hay existencias de este paquete en alguna sucursal
            if (pck.BranchProducts != null)
            {
                foreach (var bp in pck.BranchProducts)
                {
                    if (bp.Stock > Cons.Zero)
                    {
                        return Json(new
                        {
                            Result = "Imposible modificar el paquete",
                            Message = "La composición del paquete no se puede alterar " +
                            "ya que aun hay existencia de este es una o mas sucursales. " +
                            "Debes eliminar el paquete de todos los inventarios para poder modificarlo"
                        });
                    }
                }
            }
            //obtengo el detalle a eliminar del producto padre (paquete)
            var pd = pck.PackageDetails.FirstOrDefault(p => p.DetailtId == productId);

            db.PackageDetails.Remove(pd);
            db.SaveChanges();

            //hago una busqueda de los detalles del paquete actualizado
            var model = db.PackageDetails.Include(pkD => pkD.Detail).
                Where(pkD => pkD.PackageId == packageId).ToList();

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
                    var branchId = User.Identity.GetBranchId();

                    product.UpdDate = DateTime.Now.ToLocal();
                    product.UpdUser = User.Identity.Name;

                    if (product.ProductId == Cons.Zero)
                    {
                        db.Products.Add(product);
                        db.SaveChanges();
                    }
                    else
                        db.Entry(product).State = EntityState.Modified;

                    var bp = db.BranchProducts.
                        FirstOrDefault(bProd => bProd.ProductId == product.ProductId && bProd.BranchId == branchId);

                    if (bp == null)
                    {
                        bp = new BranchProduct();
                        bp.ProductId = product.ProductId;
                        bp.BranchId = branchId;
                        bp.Stock = product.Quantity;
                        bp.BuyPrice = product.BuyPrice;
                        bp.LastStock = Cons.Zero;
                        bp.StorePercentage = product.StorePercentage;
                        bp.DealerPercentage = product.DealerPercentage;
                        bp.WholesalerPercentage = product.WholesalerPercentage;
                        bp.WholesalerPrice = product.WholesalerPrice;
                        bp.DealerPrice = product.DealerPrice;
                        bp.StorePrice = product.StorePrice;
                        bp.Row = product.Row;
                        bp.Ledge = product.Ledge;
                        bp.UpdDate = DateTime.Now.ToLocal();
                        db.BranchProducts.Add(bp);

                        //si se ingresa una cantidad de producto mayor a cero al crearlo
                        //entonces genero movimiento de stock (Entrada)
                        if (product.Quantity > Cons.Zero)
                        {
                            var sm = new StockMovement();
                            sm.Quantity = product.Quantity;
                            sm.BranchId = branchId;
                            sm.ProductId = product.ProductId;
                            sm.MovementDate = DateTime.Now.ToLocal();
                            sm.MovementType = MovementType.Entry;
                            sm.User = User.Identity.Name;
                            sm.Comment = "Ingreso en creación";

                            db.StockMovements.Add(sm);
                        }
                    }
                    else
                    {
                        //si la cantidad de producto que se esta ingresando es diferente a la que hay es stock
                        //registro un movimiento de inventario
                        var sM = new StockMovement();
                        sM.ProductId = product.ProductId;
                        sM.BranchId = bp.BranchId;
                        sM.MovementDate = DateTime.Now.ToLocal();
                        sM.User = User.Identity.GetUserName();


                        if (bp.Stock > product.Quantity)
                        {
                            sM.Quantity = bp.Stock - product.Quantity;
                            sM.MovementType = MovementType.Exit;
                            sM.Comment = "Salida por ajuste rápido";

                            db.StockMovements.Add(sM);
                        }
                        //si la nueva cantidad es mayor, registro una entrada
                        else if (bp.Stock < product.Quantity)
                        {
                            sM.Quantity = product.Quantity - bp.Stock;
                            sM.MovementType = MovementType.Entry;
                            sM.Comment = "Entrada por ajuste rápido";

                            db.StockMovements.Add(sM);
                        }

                        //si hay actualización de stock
                        bp.LastStock = bp.Stock;
                        bp.Stock = product.Quantity;

                        //actualizo precios y porcentajes 
                        bp.StorePercentage = product.StorePercentage;
                        bp.DealerPercentage = product.DealerPercentage;
                        bp.BuyPrice = product.BuyPrice;
                        bp.WholesalerPercentage = product.WholesalerPercentage;
                        bp.WholesalerPrice = product.WholesalerPrice;
                        bp.DealerPrice = product.DealerPrice;
                        bp.StorePrice = product.StorePrice;
                        bp.Row = product.Row;
                        bp.Ledge = product.Ledge;
                        bp.UpdDate = DateTime.Now.ToLocal();

                        db.Entry(bp).State = EntityState.Modified;
                    }


                    int i = Cons.Zero;

                    /*
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
                    }*/

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
        public ActionResult AddCompatibility(int productId, int modelId, int begin, int end)
        {
            try
            {
                var itEnd = end + Cons.One;

                for (var yr = begin; yr < itEnd; yr++)
                {
                    var year = db.CarYears.FirstOrDefault(y => y.Year == yr && y.CarModelId == modelId);

                    if (year == null)
                    {
                        year = new CarYear { Year = yr, CarModelId = modelId };
                        db.CarYears.Add(year);
                        db.SaveChanges();
                    }

                    var comp = db.Compatibilites.FirstOrDefault(c => c.CarYearId == year.CarYearId && c.ProductId == productId);

                    if (comp == null)
                    {
                        comp = new Compatibility { CarYearId = year.CarYearId, ProductId = productId };
                        db.Compatibilites.Add(comp);
                    }
                }

                db.SaveChanges();

                var compatibilities = db.Compatibilites.Include(c => c.CarYear.CarModel).
                     Include(c => c.CarYear.CarModel.CarMake).Where(c => c.ProductId == productId).ToList();

                return PartialView("_Compatibilities", compatibilities);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al guardar los datos", Message = "Ocurrio un error al guardar la compatibilidad " + ex.Message });
            }
        }


        [HttpPost]
        public ActionResult RemoveCompatibility(int productId, int yearId)
        {
            try
            {
                var comp = db.Compatibilites.Find(yearId, productId);

                if (comp == null)
                {
                    return Json(new { Result = "No se encontraron datos!", Message = "la compatibilidad que se pretende eliminar ya no existe!" });
                }
                else
                {
                    db.Compatibilites.Remove(comp);
                    db.SaveChanges();

                    var compatibilities = db.Compatibilites.Include(c => c.CarYear.CarModel).
                         Include(c => c.CarYear.CarModel.CarMake).Where(c => c.ProductId == productId).ToList();

                    return PartialView("_Compatibilities", compatibilities);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al eliminar", Message = "Ocurrio un error al eliminar la compatibilidad " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult SearchForPackage(string filter)
        {
            string[] arr = new List<string>().ToArray();
            string code = string.Empty;
            if (filter != null && filter != string.Empty)
            {
                arr = filter.Trim().Split(' ');
                code = arr.FirstOrDefault();
            }

            List<Product> products = new List<Product>();

            products = (from ep in db.Products.Include(p => p.Category)
                        where (ep.Code == code) && (ep.ProductType == ProductType.Single) && (ep.IsActive)
                        select ep).Take((int)Cons.OneHundred).ToList();
            if(products.Count == Cons.Zero)
            {
                products = (from ep in db.Products.Include(p => p.Category)
                            where (filter == null || filter == string.Empty || arr.All(s => (ep.Code + " " + ep.Name + " " + ep.TradeMark).Contains(s)))
                            && (ep.ProductType == ProductType.Single) && (ep.IsActive)
                            select ep).Take((int)Cons.OneHundred).ToList();
            }
            

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


        [HttpPost]
        [Authorize(Roles = "Capturista")]
        public ActionResult Delete(int id)
        {
            try
            {
                var product = db.Products.Include(p => p.Images).Include(p => p.BranchProducts).
                    Include(p => p.BranchProducts.Select(bp => bp.Branch)).
                    FirstOrDefault(p => p.ProductId == id && p.IsActive);

                if (product != null)
                {
                    //reviso si el producto tiene existencias en alguna sucursal
                    foreach (var bProd in product.BranchProducts)
                    {
                        if (bProd.Stock > Cons.Zero)
                        {
                            return Json(new
                            {
                                Result = "Imposible eliminar",
                                Message = "Esto producto tiene existencias en la sucursal " + bProd.Branch.Name
                            });
                        }
                    }

                    //reviso si el producto ya se encuentra en alguna transacción,
                    //si es asi solo se desactiva
                    var detail = db.TransactionDetails.FirstOrDefault(td => td.ProductId == product.ProductId);

                    if (detail != null)
                    {
                        product.IsActive = false;
                        product.UpdDate = DateTime.Now.ToLocal();
                        product.UpdUser = User.Identity.Name;

                        db.Entry(product).State = EntityState.Modified;
                    }
                    else
                    {
                        if (product.Images != null)
                        {
                            //Elimino las imagenes guardadas
                            foreach (var im in product.Images)
                            {
                                var pPath = Server.MapPath(im.Path);
                                System.IO.File.Delete(pPath);
                            }
                        }

                        db.Products.Remove(product);
                    }

                    db.SaveChanges();

                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new
                    {
                        Result = "Datos no encontrados!",
                        Message = "El producto seleccionado ya no esta disponible"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al eliminar el producto", Message = ex.Message });
            }
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
