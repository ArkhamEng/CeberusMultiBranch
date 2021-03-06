﻿using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Models.ViewModels.Operative;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Catalog
{
    [CustomAuthorize]
    public partial class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Basic Catalog Methods
        public ActionResult Index()
        {
            var model = new SearchProductViewModel();
            model.Products = new List<List<Product>>();
            model.Systems = db.Systems.OrderBy(s => s.Name).ToSelectList();
            model.Categories = db.Categories.OrderBy(c => c.Name).ToSelectList();
            model.Makes = db.CarMakes.OrderBy(m => m.Name).ToSelectList();
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> Search(int? categoryId, int? partSystemId, int? carYear, int? carModelId, int? carMakeId, string name, string code, bool isGrid, int? id)
        {
            var model = await LookFor(categoryId, partSystemId, carYear, carModelId, carMakeId, name, code, isGrid, id);
            return PartialView("_List", model);
        }



        private async Task<List<List<Product>>> LookFor(int? categoryId, int? partSystemId, int? carYear, int? carModel, int? carMake, string name, string code, bool isGrid, int? id, int top = Cons.MaxProductResult)
        {
            var branchId = User.Identity.GetBranchId();

            string[] arr = new List<string>().ToArray();

            var key = string.Empty;

            if (name != null && name != string.Empty)
            {
                arr = name.Trim().Split(' ');

                if (arr.Length == Cons.One)
                {
                    arr[Cons.Zero] = Regex.Replace(arr[Cons.Zero], "[^a-zA-Z0-9]+", "");
                    key = arr[Cons.Zero];
                }
            }

            arr = arr.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            var userId = User.Identity.GetUserId();
            var ids = db.ShoppingCarts.Where(s => s.BranchId == branchId && s.UserId == userId).Select(s => s.ProductId).ToArray();

            List<Product> products = new List<Product>();
            /*
            products = (from p in db.Products.Include(p => p.Images)
                        //.Include(p => p.Compatibilities)
                        //.Include(p => p.BranchProducts)
                            //.Include(p => p.Compatibilities.Select(c => c.CarYear))
                            //.Include(p => p.Compatibilities.Select(c => c.CarYear.CarModel))

                        where (p.IsActive) &&
                        (id == null || p.ProductId == id)
                        && (categoryId == null || p.CategoryId == categoryId)
                        && (partSystemId == null || p.PartSystemId == partSystemId)
                        && (name == null || name == string.Empty || arr.All(s => (p.Code + " " + p.Name ).Contains(s)))

                        //&& (carYear == null || p.Compatibilities.Where(c => c.CarYearId == carYear).ToList().Count > Cons.Zero)
                        //&& (carModel == null || p.Compatibilities.Where(c => c.CarYear.CarModelId == carModel).ToList().Count > Cons.Zero)
                        //&& (carMake == null || p.Compatibilities.Where(c => c.CarYear.CarModel.CarMakeId == carMake).ToList().Count > Cons.Zero)
                        select p).Take(top).ToList();
                        //.OrderByDescending(s => s.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId) != null ?
                        //s.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId).Stock : Cons.Zero).Take(top).ToList();*/


            //for (int i = 0; i < arr.Length; i++)
            //{
            //    var word = arr[i];

            //    tmProducts = (from p in tmProducts
            //                  where (p.Code + " " + p.Name + " " + p.TradeMark).Contains(word)
            //                  select p);
            //}

            var anonProd = await (from prod in db.Products.Where(p =>
                                  string.IsNullOrEmpty(name) || arr.All(s => (p.Code + " " + p.Name + " " + p.TradeMark).Contains(s.Trim())))

                                  join b in db.BranchProducts on new { prod.ProductId, BranchId = branchId } equals new { b.ProductId, b.BranchId } into bt
                                  from bp in bt.DefaultIfEmpty()

                                  join i in db.ProductImages on prod.ProductId equals i.ProductId into it
                                  from img in it.DefaultIfEmpty()

                                  where prod.IsActive && (id == null || prod.ProductId == id)
                                  && (categoryId == null || prod.CategoryId == categoryId)
                                  && (partSystemId == null || prod.PartSystemId == partSystemId)


                                  select new
                                  {
                                      Stock = bp != null ? bp.Stock : Cons.Zero,
                                      prod,
                                      img,
                                      bp
                                  }).GroupBy(a => a.prod).Take(top).ToListAsync();//.GroupBy(a => a.prod).OrderByDescending(a => a.Sum(b => b.Stock)).Take(top).ToListAsync();


            anonProd.ForEach(group =>
                     {
                         var bp = group.Select(v => v.bp).FirstOrDefault();// (b => b != null && b.BranchId == branchId);

                         group.Key.Quantity = bp != null ? bp.Stock : Cons.Zero;
                         group.Key.StorePercentage = bp != null ? bp.StorePercentage : Cons.Zero;
                         group.Key.DealerPercentage = bp != null ? bp.DealerPercentage : Cons.Zero;
                         group.Key.WholesalerPercentage = bp != null ? bp.WholesalerPercentage : Cons.Zero;
                         group.Key.StorePrice = bp != null ? bp.StorePrice : Cons.Zero;
                         group.Key.DealerPrice = bp != null ? bp.DealerPrice : Cons.Zero;
                         group.Key.WholesalerPrice = bp != null ? bp.WholesalerPrice : Cons.Zero;
                         group.Key.Row = bp != null ? bp.Row : string.Empty;
                         group.Key.Ledge = bp != null ? bp.Ledge : string.Empty;

                         group.Key.MaxQuantity = bp != null ? bp.MaxQuantity : Cons.Zero;
                         group.Key.MinQuantity = bp != null ? bp.MinQuantity : Cons.Zero;

                         group.Key.StockLocked = bp != null ? bp.StockLocked : false;
                         group.Key.IsIncart = ids.Contains(group.Key.ProductId);
                         //group.Key.Images = group.Where(v => v.img != null).Select(v => v.img).ToList();
                     });

            products = anonProd.Select(a => a.Key).ToList();

            // products.OrderCarModels();

            if (isGrid)
                return OrderAsGrid(products);
            else
            {
                List<List<Product>> ord = new List<List<Product>>();

                products.ForEach(p =>
                                        {
                                            List<Product> pl = new List<Product>();
                                            pl.Add(p);
                                            ord.Add(pl);
                                        });

                return ord;
            }
        }



        [HttpPost]
        [CustomAuthorize(Roles = "Capturista")]
        public ActionResult BeginAdd(int? id)
        {
            try
            {
                ProductViewModel vm;
                //si el Id tiene valor busco el producto
                if (id != null)
                {
                    var branchId = User.Identity.GetBranchId();

                    var product = db.Products.Include(p => p.BranchProducts).Include(p => p.Compatibilities).
                                   Include(p => p.Images).FirstOrDefault(p => p.ProductId == id);

                    if (product.IsLocked && User.Identity.Name != product.UserLock)
                    {
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Info,
                            Code = Cons.Responses.Codes.InvalidData,
                            Header = "Producto bloqueado",
                            Body = string.Format("Bloqueado por {0}, Fecha del bloqueo {1}", product.UserLock,
                           product.LockDate.Value.ToString("dd/MM/yyyy HH:mm"))
                        });
                    }

                    DBHelper.SetProductState(id.Value, branchId, User.Identity.Name, true);

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
                    product.Ledge = bp != null ? bp.Ledge : string.Empty;
                    product.Row = bp != null ? bp.Row : string.Empty;
                    product.StockLocked = bp != null ? bp.StockLocked : false;

                    product.MaxQuantity = bp != null ? bp.MaxQuantity : Cons.Zero;
                    product.MinQuantity = bp != null ? bp.MinQuantity : Cons.Zero;



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
                vm.Systems = db.Systems.OrderBy(s => s.Name).ToSelectList();

                //si es un producto existente, busco la lista de Armadoras de vehiculos, para el view model
                if (vm.ProductId > Cons.Zero)
                    vm.CarMakes = db.CarMakes.ToSelectList();

                return PartialView("_ProductEdition", vm);
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Header = "Error al comenzar la edición!",
                    Body = "Mensaje " + ex.Message,
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError
                });
            }

        }


        [HttpPost]
        [CustomAuthorize(Roles = "Capturista")]
        public ActionResult Save(Product product)
        {
            try
            {
                product.UpdDate = DateTime.Now.ToLocal();
                product.UpdUser = User.Identity.GetUserName();

                var branchId = User.Identity.GetBranchId();

                var branch = db.Branches.Find(branchId);

                if (product.WholesalerPercentage < 10)
                    return Json(new JResponse { Header = "Error en porcentaje", Body = "El porcentaje de mayorista no puede ser menor al 10%", Code = Cons.Responses.Codes.ConditionMissing, Result = Cons.Responses.Warning });
                if (product.DealerPercentage < 15)
                    return Json(new JResponse { Header = "Error en porcentaje", Body = "El porcentaje de distribuidor no puede ser menor al 15%", Code = Cons.Responses.Codes.ConditionMissing, Result = Cons.Responses.Warning });
                if (product.StorePercentage < 20)
                    return Json(new JResponse { Header = "Error en porcentaje", Body = "El porcentaje de mostrador no puede ser menor al 20%", Code = Cons.Responses.Codes.ConditionMissing, Result = Cons.Responses.Warning });

                if (branch.IsWebStore && product.OnlinePercentage < 10)
                    return Json(new JResponse { Header = "Error en porcentaje", Body = "El porcentaje de Online no puede ser menor al 15%", Code = Cons.Responses.Codes.ConditionMissing, Result = Cons.Responses.Warning });


                //si el producto es nuevo
                if (product.ProductId == Cons.Zero)
                {
                    var pc = db.Products.Count(p => p.Code == product.Code.Trim() && p.IsActive);

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
                                    return Json(new JResponse
                                    {
                                        Header = "Cantidad no autorizada",
                                        Body = "No es posible agregar una cantidad inicial mayor a cero a un paquete " +
                                                  "sin haber configurado al menos un producto en su contenido",
                                        Result = Cons.Responses.Warning,
                                        Code = Cons.Responses.Codes.InvalidData
                                    });
                                }
                                SavePackage(product);
                                break;
                        }
                    }
                    else
                    {
                        return Json(new JResponse
                        {
                            Header = "El código de producto ya existe",
                            Body = "Ya existen un producto con este código, por favor verifica" + product.Code,
                            Result = Cons.Responses.Danger,
                            Code = Cons.Responses.Codes.InvalidData
                        });
                    }

                    return Json(new JResponse
                    {
                        Header = "Producto agregado",
                        Body = "El producto " + product.Code.ToUpper() + " fue agregado correctamente",
                        Result = Cons.Responses.Success,
                        Code = Cons.Responses.Codes.Success,
                        Id = product.ProductId
                    });
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

                    return Json(new JResponse
                    {
                        Header = "Producto actualizado",
                        Body = "Los datos del product " + product.Code + " fueron actualizados correctamente",
                        Result = Cons.Responses.Success,
                        Code = Cons.Responses.Codes.Success,
                        Id = product.ProductId
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al guardar el producto", Message = ex.Message });
            }

        }


        [HttpPost]
        public ActionResult UnLock(int id)
        {
            try
            {
                var branchId = User.Identity.GetBranchId();
                DBHelper.SetProductState(id, branchId, User.Identity.Name, false);

                return Json(new JResponse
                {
                    Result = Cons.Responses.Info,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Producto desbloqueado!",
                    Body = "Se libero el bloque del producto"
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ErroSaving,
                    Header = "Error al desbloquear el producto",
                    Body = "Mensaje: " + ex.Message
                });
            }

        }

        #endregion

        #region Method for Copy
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

                            where (filter == null || filter == string.Empty || arr.All(s => (ep.Code + "" + ep.Description + ""
                            + ep.Provider.Name + "" + ep.TradeMark).Contains(s)))
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
        [CustomAuthorize(Roles = "Capturista")]
        public ActionResult BeginCopy(int providerId, string code)
        {
            var variables = db.Variables;
            var ep = db.ExternalProducts.Find(providerId, code);

            ProductViewModel vm = new ProductViewModel();

            vm.IsActive = true;
            vm.Categories = new List<Category>().ToSelectList();//cats.ToSelectList();
            vm.Systems = db.Systems.OrderBy(s => s.Name).ToSelectList();
            vm.Name = ep.Description.ToUpper();
            vm.TradeMark = ep.TradeMark.ToUpper();
            vm.Unit = ep.Unit.ToUpper();
            vm.BuyPrice = Math.Round(ep.Price, Cons.Two);
            vm.MinQuantity = Cons.One;
            vm.Code = Regex.Replace(ep.Code, @"[^0-9a-zA-Z]+", "").ToUpper();
            vm.ProviderCode = ep.Code;
            vm.FromProviderId = ep.ProviderId;
            vm.DealerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.DealerPercentage)).Value);
            vm.StorePercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.StorePercentage)).Value);
            vm.WholesalerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.WholesalerPercentage)).Value);

            vm.DealerPrice = Math.Round(vm.BuyPrice * (Cons.One + (vm.DealerPercentage / Cons.OneHundred)), Cons.Zero);
            vm.StorePrice = Math.Round(vm.BuyPrice * (Cons.One + (vm.StorePercentage / Cons.OneHundred)), Cons.Zero);
            vm.WholesalerPrice = Math.Round(vm.BuyPrice * (Cons.One + (vm.WholesalerPercentage / Cons.OneHundred)), Cons.Zero);

            return PartialView("_ProductEdition", vm);
        }

        [HttpPost]
        public ActionResult Copy(Product product, int providerId, string code)
        {
            product.UpdDate = DateTime.Now;
            product.UpdUser = User.Identity.GetUserName();
            product.IsActive = true;
            try
            {
                var pc = db.Products.Where(p => p.Code == product.Code.Trim() && p.IsActive).Count();

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

                return Json("OK"); //Edit(product.ProductId);
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



        #endregion


        #region Utility Methods
        [HttpPost]
        public ActionResult ShowDetail(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var branchId = User.Identity.GetBranchId();

                //obtengo el producto con imagenes, compatibilidades (modelos de auto), y equivalencias (productos de proveedor
                var product = db.Products.Include(p => p.Images).Include(p => p.Compatibilities).
                            Include(p => p.BranchProducts).Include(p => p.Equivalences).
                            FirstOrDefault(p => p.ProductId == id);

                if (product.IsLocked && User.Identity.Name != product.UserLock)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.RecordLocked,
                        Header = "Producto bloqueado",
                        Body = string.Format("Bloqueado por {0}, Fecha del bloqueo {1}", product.UserLock,
                      product.LockDate.Value.ToString("dd/MM/yyyy HH:mm"))
                    });
                }


                //reviso si el producto ya cuenta con una relación en la sucursal
                var bProd = product.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);

                //si no existe relacion con sucursal, los precios y existencias seran 0
                product.Quantity = bProd != null ? bProd.Stock : Cons.Zero;
                product.StorePercentage = bProd != null ? bProd.StorePercentage : Cons.Zero;
                product.DealerPercentage = bProd != null ? bProd.DealerPercentage : Cons.Zero;
                product.WholesalerPercentage = bProd != null ? bProd.WholesalerPercentage : Cons.Zero;
                product.StorePrice = bProd != null ? bProd.StorePrice : Cons.Zero;
                product.DealerPrice = bProd != null ? bProd.DealerPrice : Cons.Zero;
                product.WholesalerPrice = bProd != null ? bProd.WholesalerPrice : Cons.Zero;
                product.MaxQuantity = bProd != null ? bProd.MaxQuantity : Cons.Zero;
                product.MinQuantity = bProd != null ? bProd.MinQuantity : Cons.Zero;

                product.Ledge = bProd != null ? bProd.Ledge : string.Empty;
                product.Row = bProd != null ? bProd.Row : string.Empty;

                product.OrderCarModels();

                //Obtengo existencias en otras sucursales
                product.Branches = db.Branches.ToList();

                foreach (var branch in product.Branches)
                {
                    var bpr = db.BranchProducts.FirstOrDefault(bp => bp.ProductId == product.ProductId
                    && bp.BranchId == branch.BranchId);

                    branch.Quantity = bpr != null ? bpr.Stock : Cons.Zero;
                }

                foreach (var eq in product.Equivalences)
                {
                    eq.ExternalProduct = db.ExternalProducts.Include(ep => ep.Provider).
                        FirstOrDefault(ep => ep.ProviderId == eq.ProviderId && ep.Code == eq.Code);
                }

                return PartialView("_ProductDetail", product);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error Inesperado",
                    Body = "Ocurrio un error al obtener el detalle del producto"
                });
            }
        }

        private void SaveSingle(Product product)
        {
            var branchId = User.Identity.GetBranchId();

            //si es producto nuevo lo guardo de inmediato para generar un ID
            if (product.ProductId == Cons.Zero)
            {
                if (!string.IsNullOrEmpty(product.ProviderCode))
                {
                    Equivalence eq = new Equivalence
                    {
                        ProviderId = product.FromProviderId,
                        Code = product.ProviderCode,
                        BuyPrice = product.BuyPrice,
                        UpdDate = DateTime.Now.ToLocal(),
                        UpdUser = User.Identity.Name,
                        InsDate = DateTime.Now.ToLocal(),
                        InsUser = User.Identity.Name,
                        IsDefault = true
                    };

                    product.Equivalences = new List<Equivalence>();
                    product.Equivalences.Add(eq);
                }

                db.Products.Add(product);
                db.SaveChanges();
            }
            else
            {
                //quito el bloqueo
                product.LockDate = null;
                product.UserLock = null;

                db.Entry(product).State = EntityState.Modified;
            }


            var branchP = db.BranchProducts.FirstOrDefault(bp => bp.ProductId == product.ProductId
                                                            && bp.BranchId == branchId);

            //si no existe una realacion con sucursal la creo con las unidades ingresadas
            if (branchP == null)
            {
                branchP = new BranchProduct
                {
                    ProductId = product.ProductId,
                    BranchId = User.Identity.GetBranchId(),
                    Stock = product.StockRequired ? product.Quantity : Cons.Zero,
                    LastStock = Cons.Zero,
                    UpdDate = product.UpdDate,
                    BuyPrice = product.BuyPrice,
                    DealerPercentage = product.DealerPercentage,
                    StorePercentage = product.StorePercentage,
                    WholesalerPercentage = product.WholesalerPercentage,
                    StorePrice = product.StorePrice,
                    DealerPrice = product.DealerPrice,
                    WholesalerPrice = product.WholesalerPrice,
                    UpdUser = product.UpdUser,
                    Row = product.Row ?? string.Empty,
                    Ledge = product.Ledge ?? string.Empty,
                    StockLocked = product.StockLocked,
                    MaxQuantity = product.MaxQuantity,
                    MinQuantity = product.MinQuantity
                };

                db.BranchProducts.Add(branchP);

                if (product.StockRequired && product.Quantity > Cons.Zero)
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
                //si estoy actualizando pero el inventario ha sido bloqueado, entonces conservo el  precio de compra original
                if (!product.StockLocked)
                    branchP.BuyPrice = product.BuyPrice;

                //coloco porcentajes de venta
                branchP.DealerPercentage = product.DealerPercentage;
                branchP.StorePercentage = product.StorePercentage;
                branchP.WholesalerPercentage = product.WholesalerPercentage;

                branchP.MaxQuantity = product.MaxQuantity;
                branchP.MinQuantity = product.MinQuantity;
                branchP.Ledge = product.Ledge ?? string.Empty;
                branchP.Row = product.Row ?? string.Empty;
                branchP.UpdDate = product.UpdDate;
                branchP.UpdUser = product.UpdUser;
                branchP.LockDate = null;
                branchP.UserLock = null;
                branchP.StockLocked = product.StockLocked;

                //calculo precios tomando como base el precio de compra que saque de la base de datos
                //ya que puede haber actualizaciones por medio de compras
                branchP.DealerPrice = Math.Round(branchP.BuyPrice * (Cons.One + (branchP.DealerPercentage / Cons.OneHundred)), Cons.Zero);
                branchP.StorePrice = Math.Round(branchP.BuyPrice * (Cons.One + (branchP.StorePercentage / Cons.OneHundred)), Cons.Zero);
                branchP.WholesalerPrice = Math.Round(branchP.BuyPrice * (Cons.One + (branchP.WholesalerPercentage / Cons.OneHundred)), Cons.Zero);

                //si la cantidad indicada es diferente a la cantidad en stock agrego el movimiento requerido 
                if (!product.StockLocked && (product.StockRequired && product.Quantity > branchP.Stock || product.Quantity < branchP.Stock))
                {
                    double units;
                    branchP.LastStock = branchP.Stock;

                    var sm = new StockMovement
                    {
                        BranchId = branchId,
                        ProductId = product.ProductId,
                        User = product.UpdUser,
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
                    BuyPrice = package.BuyPrice
                };
            }
            //si estoy actualizando pero el inventario ha sido bloqueado, entonces conservo el  precio de compra
            else if (!package.StockLocked)
                branchP.BuyPrice = package.BuyPrice;

            //coloco los porcentajes de utilidad
            branchP.WholesalerPercentage = package.WholesalerPercentage;
            branchP.DealerPercentage = package.DealerPercentage;
            branchP.StorePercentage = package.StorePercentage;

            branchP.Ledge = package.Ledge ?? string.Empty;
            branchP.Row = package.Row ?? string.Empty;
            branchP.MaxQuantity = package.MaxQuantity;
            branchP.MinQuantity = package.MinQuantity;
            branchP.UpdDate = package.UpdDate;
            branchP.UpdUser = package.UpdUser;
            branchP.StockLocked = package.StockLocked;

            //calculo precios tomando como base el precio de compra que saque de la base de datos
            //ya que puede haber actualizaciones por medio de compras
            branchP.DealerPrice = Math.Round(branchP.BuyPrice * (Cons.One + (branchP.DealerPercentage / Cons.OneHundred)), Cons.Zero);
            branchP.StorePrice = Math.Round(branchP.BuyPrice * (Cons.One + (branchP.StorePercentage / Cons.OneHundred)), Cons.Zero);
            branchP.WholesalerPrice = Math.Round(branchP.BuyPrice * (Cons.One + (branchP.WholesalerPercentage / Cons.OneHundred)), Cons.Zero);

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
                    if (!package.StockLocked && (package.StockRequired && package.Quantity > branchP.Stock))
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
                return Json(new JResponse
                {
                    Header = "Error al guardar los datos",
                    Body = "Ocurrio un error al guardar la compatibilidad " + ex.Message,
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError
                });
            }
        }


        [HttpPost]
        public ActionResult RemoveCompatibility(int productId, int yearId)
        {
            try
            {
                var comp = db.Compatibilites.FirstOrDefault(c => c.CarYearId == yearId && c.ProductId == productId);

                if (comp == null)
                {
                    return Json(new JResponse
                    {
                        Header = "No se encontraron datos!",
                        Body = "la compatibilidad que se pretende eliminar ya no existe!",
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.RecordNotFound
                    });
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
                return Json(new JResponse
                {
                    Header = "Error al Eliminar",
                    Body = "Ocurrio un error desconocido al eliminar la compatibilidad",
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError
                });
            }
        }

        #endregion

        [HttpPost]
        [CustomAuthorize(Roles = "Supervisor")]
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
        [CustomAuthorize(Roles = "Supervisor")]
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
        public ActionResult SearchEdit(string code)
        {
            var model = LookFor(null, null, null, null, null, code, null, false, null);
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


        //[HttpPost]
        //public ActionResult QuickSearch(string name)
        //{
        //    string[] arr = new List<string>().ToArray();

        //    if (name != null && name != string.Empty)
        //        arr = name.Trim().Split(' ');


        //    var model = (from p in db.Products
        //                 where
        //                   (name == null || name == string.Empty || arr.All(s => (p.Code + " " + p.Name).Contains(s)) && p.IsActive)
        //                 select p).Include(p => p.Images).Take(Cons.QuickResults).ToList();

        //    return PartialView("_ProductList", model);
        //}



        [HttpPost]
        [CustomAuthorize(Roles = "Capturista")]
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
        [CustomAuthorize(Roles = "Capturista")]
        public ActionResult Edit1(int? productId)
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

                if (product.IsLocked && User.Identity.Name != product.UserLock)
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.InvalidData,
                        Header = "Producto bloqueado",
                        Body = string.Format("Bloqueado por {0}, Fecha del bloqueo {1}", product.UserLock,
                        product.LockDate.Value.ToString("dd/MM/yyyy HH:mm"))
                    });

                DBHelper.SetProductState(productId.Value, branchId, User.Identity.Name, true);

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
                product.Ledge = bp != null ? bp.Ledge : string.Empty;
                product.Row = bp != null ? bp.Row : string.Empty;
                product.StockLocked = bp != null ? bp.StockLocked : false;

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
            vm.Systems = db.Systems.OrderBy(s => s.Name).ToSelectList();

            //si es un producto existente, busco la lista de Armadoras de vehiculos, para el view model
            if (vm.ProductId > Cons.Zero)
                vm.CarMakes = db.CarMakes.ToSelectList();

            return PartialView("_QuickAddProduct", vm);
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
            if (products.Count == Cons.Zero)
            {
                products = (from ep in db.Products.Include(p => p.Category)
                            where (filter == null || filter == string.Empty || arr.All(s => (ep.Code + " " + ep.Name + " " + ep.TradeMark).Contains(s)))
                            && (ep.ProductType == ProductType.Single) && (ep.IsActive)
                            select ep).Take((int)Cons.OneHundred).ToList();
            }


            return PartialView("_ListForPackage", products);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "Capturista")]
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
                            return Json(new JResponse
                            {
                                Result = Cons.Responses.Warning,
                                Code = Cons.Responses.Codes.InvalidData,
                                Header = "Imposible eliminar",
                                Body = "Esto producto tiene existencias en la sucursal " + bProd.Branch.Name
                            });
                        }
                    }

                    //reviso si el producto ya se encuentra en alguna transacción,
                    //si es asi solo se desactiva
                    var saleDetailCount = db.SaleDetails.Where(td => td.ProductId == product.ProductId).Count();

                    var purchaseDetailCount = db.PurchaseDetails.Where(td => td.ProductId == product.ProductId).Count();

                    var response = new JResponse
                    {
                        Result = Cons.Responses.Success,
                        Code = Cons.Responses.Codes.Success,
                        Header = "Producto Eliminado",
                        Body = "El producto ha sido eliminado peramanentemente"
                    };

                    if (saleDetailCount > Cons.Zero || purchaseDetailCount > Cons.Zero)
                    {
                        product.IsActive = false;
                        product.UpdDate = DateTime.Now.ToLocal();
                        product.UpdUser = User.Identity.Name;

                        db.Entry(product).Property(p => p.IsActive).IsModified = true;
                        db.Entry(product).Property(p => p.UpdDate).IsModified = true;
                        db.Entry(product).Property(p => p.UpdUser).IsModified = true;

                        db.Configuration.ValidateOnSaveEnabled = false;

                        response.Header = "Producto desactivado!";
                        response.Result = "El producto ha sido desactivado y no podrá ser usado en ninguna operación";
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

                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Success,
                        Code = Cons.Responses.Codes.Success,
                        Header = "Producto Desactivdao",
                        Body = "El producto ha sido desactivado y no podra ser utiliado en ninguna operación"
                    });
                }
                else
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.RecordNotFound,
                        Header = "Datos no encontrados!",
                        Body = "El producto seleccionado ya no esta disponible"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al eliminar",
                    Body = "Ocurrio un error al intentar eliminar el producto"
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Supervisor")]
        public ActionResult Activate(int id)
        {
            try
            {
                var product = db.Products.FirstOrDefault(p => p.ProductId == id);

                product.IsActive = true;
                product.UpdDate = DateTime.Now.ToLocal();
                product.UpdUser = User.Identity.Name;

                db.Entry(product).Property(p => p.IsActive).IsModified = true;
                db.Entry(product).Property(p => p.UpdDate).IsModified = true;
                db.Entry(product).Property(p => p.UpdUser).IsModified = true;

                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Producto Reactivado",
                    Body = "El Producto seleccionado ha sido reactivado"
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al Activar",
                    Body = "Ocurrio un error al intentar reactivar el producto"
                });
            }
        }



        [CustomAuthorize(Roles = "Supervisor")]
        public ActionResult StockMovement()
        {
            var model = new TransactionViewModel();
            model.Branches = User.Identity.GetBranches().ToSelectList();
            model.StockMovements = new List<StockMovement>();
            return View(model);
        }

        [CustomAuthorize(Roles = "Supervisor")]
        [HttpPost]
        public ActionResult SearchMovements(int branchId, string description, DateTime? beginDate, DateTime? endDate)
        {

            string code = null;
            string[] arr = new List<string>().ToArray();

            if (description != null && description != string.Empty)
            {
                arr = description.Trim().Split(' ');

                if (arr.Length == Cons.One)
                    code = arr.FirstOrDefault();
            }

            List<StockMovement> model = null;
            //if (code != null)
            //{
            //    model = db.StockMovements.Include(sm => sm.BranchProduct.Product).Where(sm => sm.BranchProduct.Product.Code == code).ToList();
            //}
            //else
            //{
            model = (from sm in db.StockMovements.Include(sm => sm.BranchProduct.Product)

                     where (description == null || description == string.Empty ||
                            arr.All(s => (sm.BranchProduct.Product.Code + " " + sm.BranchProduct.Product.Name + " " + sm.BranchProduct.Product.TradeMark).Contains(s)))
                          && (beginDate == null || sm.MovementDate >= beginDate)
                          && (endDate == null || sm.MovementDate <= endDate)
                          && (sm.BranchId == branchId)
                     select sm).OrderByDescending(s => s.MovementDate).ToList();
            //}

            return PartialView("_MovementList", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Capturista")]
        //public ActionResult Create([Bind(Include = "ProductId,Code,Name,Description,MinQuantity,BarCode,BuyPrice")] Product product,HttpPostedFileBase file)
        public ActionResult Save1([Bind(Exclude = "Compatibilities")]Product product)
        {

            try
            {
                var response = new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success
                };

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
                    bp.UpdUser = User.Identity.Name;
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

                        response.Id = product.ProductId;
                        response.Header = "Nuevo Producto registrado";
                        response.Body = "El Producto " + product.Code.ToUpperInvariant() +
                            " fue agregado correctamente, ahora puedes adjuntarle imagenes y configruar compatibilidades";
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


                    response.Id = product.ProductId;
                    response.Header = "Producto actualizado!";
                    response.Body = "Se registraron las modificaciones del Producto " + product.Code.ToUpperInvariant() +
                        " y se liberaron bloqueos sobre el registro";
                }
                db.SaveChanges();

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ErroSaving,
                    Header = "Error al guardar el producto",
                    Body = "Mensaje:" + ex.Message
                });
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
