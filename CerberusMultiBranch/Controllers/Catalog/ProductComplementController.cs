using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;

namespace CerberusMultiBranch.Controllers.Catalog
{
    public partial class ProductsController : Controller
    {
        public ActionResult SearchPagin()
        {
            var model = GetPagin(Cons.One, 10);

            return View(model);
        }

        [HttpPost]
        public ActionResult SearchP(int page, int records)
        {
            var model = GetPagin(page, records);

            return PartialView("_SearchPagin", model);
        }

        private PagedResult<Product> GetPagin(int page, int records)
        {
            var model = (from p in db.Products.Include(p => p.Images).Include(p => p.Compatibilities).
                   Include(p => p.BranchProducts).Include(p => p.Compatibilities.Select(c => c.CarYear)).
                   Include(p => p.Compatibilities.Select(c => c.CarYear.CarModel))

                         select p).OrderBy(p => p.ProductId).GetPaged<Product>(page, records);

            return model;
        }

        #region QuickSearch

        [HttpPost]
        public ActionResult ShowQuickSearch(int? providerId)
        {
            var model = LookForPurchase(null, providerId.Value);

            return PartialView("_ProductQuickSearch", model);
        }

        [HttpPost]
        public ActionResult QuickSearch(string filter, int? providerId)
        {
            var model = LookForPurchase(filter, providerId.Value);

            return PartialView("_ProductQuickSearchList", model);
        }

        private List<ProductViewModel> LookForPurchase(string filter, int providerId)
        {

            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
            {
                arr = filter.Trim().Split(' ');

                if (arr.Length == Cons.One)
                    arr[Cons.Zero] = Regex.Replace(arr[Cons.Zero], "[^a-zA-Z0-9]+", "");
            }

            var branches = User.Identity.GetBranches().Select(b => b.BranchId);
            var userId = User.Identity.GetUserId();

            var products = (from bp in db.BranchProducts
                        
                            join e  in db.Equivalences.Where(ev=> ev.ProviderId == providerId) on bp.ProductId equals e.ProductId into em
                            from eq in em.DefaultIfEmpty()
                            join ex in db.ExternalProducts on new { eq.ProviderId, eq.Code } equals new { ex.ProviderId, ex.Code } into exm
                            from ep in exm.DefaultIfEmpty()
                            join it in db.PurchaseItems.Where(i=> i.UserId == userId && i.ProviderId == providerId) on 
                                new { bp.BranchId, bp.ProductId} equals new { it.BranchId, it.ProductId} into itm
                            from its in itm.DefaultIfEmpty()

                            where (string.IsNullOrEmpty(filter) || arr.All(s => (bp.Product.Code + " " + bp.Product.Name).Contains(s))) &&
                                   (branches.Contains(bp.BranchId)) &&
                                   (bp.Stock < bp.MaxQuantity) && 
                                   (bp.Product.IsActive)

                            select new ProductViewModel
                            {
                                ProductId = bp.ProductId,
                                Name = bp.Product.Name,
                                Code = bp.Product.Code,
                                BranchId = bp.BranchId,
                                TradeMark = bp.Product.TradeMark,
                                LockDate = bp.Product.LockDate,
                                UserLock = bp.Product.UserLock,
                                MaxQuantity = bp.Product.MaxQuantity,
                                MinQuantity = bp.Product.MinQuantity,
                                Quantity = bp.Stock,
                                AddQuantity = its != null? its.Quantity :  bp.Product.MaxQuantity - bp.Stock,
                                BranchName = bp.Branch.Name,
                                ProviderCode = ep != null ? ep.Code : "No asignado",
                                BuyPrice = ep != null ? ep.Price : Cons.Zero,
                                AddToPurchaseDisabled = (its != null)

                            }).OrderBy(p => p.Code).Take(Cons.MaxProductResult).ToList();

            return products;
        }


        #endregion


        #region Product Movements
        [HttpPost]
        [CustomAuthorize(Roles = "Supervisor")]
        public ActionResult GetProductMovements(int id)
        {
            try
            {
                int branchId = User.Identity.GetBranchId();

                List<StockMovement> model = null;

                var date = (from m in db.StockMovements
                            join s in db.StockCounts on m.StockCountId equals s.StockCountId
                            join sd in db.StockCountDetails on s.StockCountId equals sd.StockCountId
                            where (m.BranchId == branchId && m.ProductId == id)
                            orderby m.MovementDate descending
                            select m).FirstOrDefault();

                if (date != null)
                {
                    model = db.StockMovements.Where(sm => sm.ProductId == id && sm.BranchId == branchId && sm.MovementDate > date.MovementDate).OrderByDescending(m => m.MovementDate).ToList();
                    
                    ViewBag.Date = string.Format($"Fecha último corte: { date.MovementDate }        Cantidad: { date.Quantity }        Almacenista: { date.User }");
                }
                else
                {
                   model = db.StockMovements.Where(sm => sm.ProductId == id && sm.BranchId == branchId).OrderByDescending(m => m.MovementDate).ToList();

                    ViewBag.Date = string.Format("No hay corte para este producto");
                }

                //if (model.Count == Cons.Zero)
                //{
                //    return Json(new JResponse
                //    {
                //        Result = Cons.Responses.Info,
                //        Code = Cons.Responses.Codes.RecordNotFound,
                //        Header = "Producto sin movimientos",
                //        Body = "El producto, aun no tiene movimientos registrados en esta sucursal"
          

                //    });

                //}

       
                return PartialView("_ProductMovement", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al obtener datos",
                    Body = "Ocurrio un error inesperado obtener los movimientos del producto"
                });
            }

        }

        #endregion

        #region Images
        [HttpPost]
        public ActionResult GetProductImages(int id)
        {
            var list = db.ProductImages.Where(pi => pi.ProductId == id).Select(pi => pi.Path).ToList();

            if (list.Count == Cons.Zero)
                list.Add(Cons.NoImagePath);

            return PartialView("_ProductImages", list);
        }



        [HttpPost]
        [CustomAuthorize(Roles = "Capturista")]
        public ActionResult SaveImage(int id, HttpPostedFileBase image)
        {
            try
            {
                if (image.FileName.Trim().Length >= 80)
                {
                    return Json(new JResponse
                    {
                        Header = "Nombre de Archivo Invalido!",
                        Body = "El nombre del archivo no puede ser mayor a 80 caracteres",
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.ErroSaving
                    });
                }

                if (image.ContentLength > 0)
                {
                    if (image != null)
                    {
                        ProductImage f = new ProductImage();

                        f.Path = FileManager.SaveImage(image, id.ToString(), ImageType.Products);
                        f.ProductId = id;
                        f.Name = image.FileName;
                        f.Type = image.ContentType;
                        f.Size = image.ContentLength;
                        

                        db.ProductImages.Add(f);
                        db.SaveChanges();
                    }
                }


                var images = db.ProductImages.Where(i => i.ProductId == id).ToList();
                return PartialView("_ProductEditionImages", images);
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Header = "Erro al agregar imagen!",
                    Body = "Mensaje "+ex.Message,
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError
                });
            }

        }


        [HttpPost]
        [CustomAuthorize(Roles = "Capturista")]
        public ActionResult DeleteImage(int id)
        {
            try
            {
                var image = db.ProductImages.FirstOrDefault(i => i.ProductImageId == id);
                int pId = image.ProductId;

                if (image == null)
                {
                    return Json(new JResponse
                    {
                        Header = "Error al eliminar imagen!",
                        Body = "No se encontro la imagen seleccionada",
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.RecordNotFound
                    });
                }

                var pPath = Server.MapPath(image.Path);
                System.IO.File.Delete(pPath);


                db.ProductImages.Remove(image);
                db.SaveChanges();

                var model = db.ProductImages.Where(i => i.ProductId == pId).ToList();

                ModelState.Clear();

                return PartialView("_ProductEditionImages", model);
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Header = "Error al eliminar imagen!",
                    Body = "Mensaje " + ex.Message,
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError
                });
            }
        }

        #endregion

        #region Transference
        [HttpPost]
        [CustomAuthorize(Roles = "Supervisor")]
        public ActionResult BeginTransference(int destBranchId, int productId)
        {
            try
            {
                var branchId = User.Identity.GetBranchId();

                var branchP = db.BranchProducts.Include(bp => bp.Product).
                    Include(bp => bp.Product.Images).Include(bp => bp.Branch).
                     FirstOrDefault(bp => bp.BranchId == branchId && bp.ProductId == productId);

                var destBranch = db.Branches.FirstOrDefault(p => p.BranchId == destBranchId);

                TransferViewModel transfer = new TransferViewModel
                {
                    TransBranch = destBranch.Name,
                    TransBranchId = destBranch.BranchId,
                    TransCode = branchP.Product.Code,
                    TransImage = branchP.Product.Images.Count > Cons.Zero ? branchP.Product.Images.First().Path : "/Content/Images/sinimagen.jpg",
                    TransName = branchP.Product.Name,
                    TransProductId = productId,
                    TransStock = branchP.Stock,
                    TransUnit = branchP.Product.Unit
                };

                return PartialView("_Transference", transfer);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Danger,
                    Header = "Errro al Transferir",
                    Body = "Ocurrio un error inesperado al realizar la transferencia"
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Supervisor")]
        public JsonResult Transfer(int detsBranchId, int productId, double quantity)
        {

            var branchId = User.Identity.GetBranchId();

            var orBP = db.BranchProducts.Include(br => br.Product).Include(br => br.Branch).
                FirstOrDefault(br => br.BranchId == branchId && br.ProductId == productId);

            if (orBP.Stock < quantity)
            {
                return Json(new JResponse
                {  
                    Result = Cons.Responses.Warning,
                    Code = Cons.Responses.Codes.InvalidData,
                    Header = "Imposible realizar la transferencia",
                    Body = "La cantidad solicitada es mayor a la disponible en sucursal"
                });
            }

            //Obtengo la relacion producto sucursal destino
            var destBp = db.BranchProducts.Include(b => b.Branch).
                        FirstOrDefault(b => b.BranchId == detsBranchId && b.ProductId == productId);

            try
            {
                //si no existe la creo de lo contrario la actualizo
                if (destBp == null)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.InvalidData,
                        Header = "Imposible realizar la transferencia",
                        Body = "El producto no se esta configurado en la sucursal de destino, es necesario realizar las configuraciones antes de transferir"
                    });
                }
                else
                {
                    var newStock = destBp.Stock + quantity;

                    var currentAmount = (destBp.Stock * destBp.BuyPrice);


                    destBp.LastStock = destBp.Stock;
                    destBp.Stock     = newStock;

                    //si el precio de la sucursal destino es mayor que la de origen, se promedia de lo contrario se actualiza
                    if (destBp.BuyPrice > orBP.BuyPrice && destBp.Stock > Cons.Zero)
                    {
                        var newAmount = (quantity * orBP.BuyPrice);
                        destBp.BuyPrice = Math.Round(((currentAmount + newAmount) / destBp.Stock), Cons.Two);
                    }
                    else
                        destBp.BuyPrice = orBP.BuyPrice;

                    destBp.StorePrice = Math.Round(  (destBp.StorePercentage > Cons.Zero ?
                                               destBp.BuyPrice * (Cons.One + (destBp.StorePercentage / Cons.OneHundred)) : 
                                               destBp.BuyPrice), Cons.Zero);

                    destBp.DealerPrice = Math.Round( (destBp.DealerPercentage > Cons.Zero ?
                                               destBp.BuyPrice * (Cons.One + (destBp.DealerPercentage / Cons.OneHundred)) : 
                                               destBp.BuyPrice), Cons.Zero);

                    destBp.WholesalerPrice = Math.Round( (destBp.WholesalerPercentage > Cons.Zero ?
                                              destBp.BuyPrice * (Cons.One + (destBp.WholesalerPercentage / Cons.OneHundred)) : 
                                              destBp.BuyPrice), Cons.Zero);

                    //si es sucursal web
                    if (destBp.Branch.IsWebStore)
                    {
                        destBp.OnlinePrice = Math.Round((destBp.OnlinePercentage != Cons.Zero ?
                                               destBp.BuyPrice * (Cons.One + (destBp.OnlinePercentage / Cons.OneHundred)) : 
                                               destBp.BuyPrice), Cons.Zero);
                    }

                    destBp.UpdDate = DateTime.Now.ToLocal();
                    destBp.UpdUser = User.Identity.Name;
                }

                db.Entry(destBp).State = EntityState.Modified;
              
                //agrego los movimientos de Stock en la sucursal destino
                StockMovement smD = new StockMovement
                {
                    BranchId     = destBp.BranchId,
                    ProductId    = destBp.ProductId,
                    Comment      = "Transferencia de sucursal " + orBP.Branch.Name,
                    MovementType = MovementType.Entry,
                    MovementDate = DateTime.Now.ToLocal(),
                    User         = User.Identity.Name,
                    Quantity     = quantity
                };

                db.StockMovements.Add(smD);

                //agrego los movimientos de Stock en la sucursal origen
                StockMovement smO = new StockMovement
                {
                    BranchId     = orBP.BranchId,
                    ProductId    = orBP.ProductId,
                    Comment      = "Transferencia a sucursal " + destBp.Branch.Name,
                    MovementType = MovementType.Exit,
                    MovementDate = DateTime.Now.ToLocal(),
                    User = User.Identity.Name,
                    Quantity = quantity
                };

                db.StockMovements.Add(smO);

                //actualizo stock en sucursal de origen
                orBP.LastStock = orBP.Stock;
                orBP.Stock     -= quantity;
                orBP.UpdUser   = User.Identity.Name;
                orBP.UpdDate   = DateTime.Now.ToLocal();

                db.Entry(orBP).Property(p => p.LastStock).IsModified = true;
                db.Entry(orBP).Property(p => p.Stock).IsModified = true;
                db.Entry(orBP).Property(p => p.UpdDate).IsModified = true;
                db.Entry(orBP).Property(p => p.UpdDate).IsModified = true;

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Transferencia exitosa!",
                    Body = "Se transfirieron " + quantity + " unidades del producto " + orBP.Product.Code.ToUpper() + " a la sucursale " + destBp.Branch.Name.ToUpper()
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al transferir",
                    Body = "Ocurrio un error inesperado al transferir, detalle:" + ex.Message
                });
            }
        }

        #endregion

    }
}