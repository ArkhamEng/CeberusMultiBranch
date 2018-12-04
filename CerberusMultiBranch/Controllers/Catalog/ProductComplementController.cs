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
                        
                            join e  in db.Equivalences on bp.ProductId equals e.ProductId into em
                            from eq in em.DefaultIfEmpty()
                            join ex in db.ExternalProducts on new { eq.ProviderId, eq.Code } equals new { ex.ProviderId, ex.Code } into exm
                            from ep in exm.DefaultIfEmpty()
                            join it in db.PurchaseItems on new { bp.BranchId, bp.ProductId, eq.ProviderId } equals new { it.BranchId, it.ProductId, it.ProviderId } into itm
                            from its in itm.DefaultIfEmpty()

                            where (string.IsNullOrEmpty(filter) || arr.All(s => (bp.Product.Code + " " + bp.Product.Name).Contains(s))) &&
                                  (its == null || its.UserId == userId) &&
                                  (eq == null || eq.ProviderId == providerId) &&
                                  (branches.Contains(bp.BranchId)) &&
                                  (bp.Stock < bp.MaxQuantity)


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
                                AddQuantity = bp.Product.MaxQuantity - bp.Stock,
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

                var model = db.StockMovements.Where(sm => sm.ProductId == id && sm.BranchId == branchId).OrderByDescending(m => m.MovementDate).ToList();

                if (model.Count == Cons.Zero)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.RecordNotFound,
                        Header = "Producto sin movimientos",
                        Body = "El producto, aun no tiene movimientos registrados en esta sucursal"
                    });
                }


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
                    destBp.LastStock = destBp.Stock;
                    destBp.Stock += quantity;
                    destBp.UpdDate = DateTime.Now;
                    destBp.UpdUser = User.Identity.Name;
                }

                db.Entry(destBp).Property(p => p.LastStock).IsModified = true;
                db.Entry(destBp).Property(p => p.Stock).IsModified = true;
                db.Entry(destBp).Property(p => p.UpdDate).IsModified = true;
                db.Entry(destBp).Property(p => p.UpdDate).IsModified = true;

                //agrego los movimientos de Stock en la sucursal destino
                StockMovement smD = new StockMovement
                {
                    BranchId = destBp.BranchId,
                    ProductId = destBp.ProductId,
                    Comment = "Transferencia de sucursal " + orBP.Branch.Name,
                    MovementType = MovementType.Entry,
                    MovementDate = DateTime.Now,
                    User = User.Identity.Name,
                    Quantity = quantity
                };

                db.StockMovements.Add(smD);

                //agrego los movimientos de Stock en la sucursal origen
                StockMovement smO = new StockMovement
                {
                    BranchId = orBP.BranchId,
                    ProductId = orBP.ProductId,
                    Comment = "Transferencia a sucursal " + destBp.Branch.Name,
                    MovementType = MovementType.Exit,
                    MovementDate = DateTime.Now,
                    User = User.Identity.Name,
                    Quantity = quantity
                };

                db.StockMovements.Add(smO);

                //actualizo stock en sucursal de origen
                orBP.LastStock = orBP.Stock;
                orBP.Stock -= quantity;
                orBP.UpdUser = User.Identity.Name;
                orBP.UpdDate = DateTime.Now.ToLocal();

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