using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.ViewModels.Operative;
using System.Web.Mvc;
using System.Linq;
using Microsoft.AspNet.Identity;
using CerberusMultiBranch.Support;
using CerberusMultiBranch.Models.Entities.Purchasing;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using System;
using CerberusMultiBranch.Models.Entities.Catalog;
using System.Data.Entity;
using System.Collections.Generic;

namespace CerberusMultiBranch.Controllers.Operative
{
    public partial class PurchasesController : Controller
    {
        /// <summary>
        /// Obtiene la sesión de compra del usuario en turno
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchaseCart()
        {
            var userId = User.Identity.GetUserId();

            var model = new PurchaseCartViewModel();
            model.PurchaseTypes = db.PurchaseTypes.ToSelectList();


            var items = GetCartItems(userId);

            if (items.Count > Cons.Zero)
            {
                model.ProviderId   = items.FirstOrDefault().ProviderId;
                model.ProviderName = items.FirstOrDefault().ProviderName;
                model.PuschaseType = items.FirstOrDefault().PurchaseType;
                model.PurchaseItems = items;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult BeginSetCode(int productId, int providerId)
        {
            try
            {
                var model = (from p in db.Products

                            join e in db.Equivalences on p.ProductId equals e.ProductId into em
                            from eq in em.DefaultIfEmpty()

                            join ex in db.ExternalProducts on new { eq.ProviderId, eq.Code } equals new { ex.ProviderId, ex.Code } into exm
                            from ep in exm.DefaultIfEmpty()

                            where (p.ProductId == productId) &&
                                  (eq == null || eq.ProviderId == providerId) 

                                  select new SetProviderCodeViewModel
                                  {
                                     ProviderCode = ep != null? ep.Code : string.Empty,
                                     InternalCode = p.Code,
                                     ProductId = p.ProductId,
                                     ProviderId = providerId,
                                     Price = ep != null? ep.Price : Cons.Zero
                                  }
                            ).FirstOrDefault();

          
                return PartialView("_SetProviderCode", model);
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al obtener datos",
                    Body = "Detalle del error " + ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult SetProviderCode(SetProviderCodeViewModel model)
        {
            try
            {
                var ep = db.ExternalProducts.FirstOrDefault(eq => eq.ProviderId == model.ProviderId && eq.Code == model.ProviderCode);

                //si no hoy producto externo, lo creo
                if (ep == null)
                {
                    var prod = db.Products.FirstOrDefault(p => p.ProductId == model.ProductId);

                    ep = new ExternalProduct
                    {
                        Code = model.ProviderCode,
                        Price = model.Price,
                        Description = prod.Name,
                        TradeMark = prod.TradeMark,
                        Unit = prod.Unit,
                        ProviderId = model.ProviderId
                    };

                    db.ExternalProducts.Add(ep);
                }
                //de lo contrario solo actualizo el precio
                else
                {
                    ep.Price = model.Price;
                    db.Entry(ep).Property(p => p.Price).IsModified = true;
                }

                var eqv = db.Equivalences.FirstOrDefault(eq => eq.ProviderId == model.ProviderId && eq.ProductId == model.ProductId);

                //si no hay una relación de equivalencia
                if(eqv == null)
                {
                    eqv = new Equivalence { Code = model.ProviderCode, ProductId = model.ProductId, ProviderId = model.ProviderId };
                    db.Equivalences.Add(eqv);
                }
                else
                {
                    eqv.Code = model.ProviderCode;
                    db.Entry(eqv).Property(p => p.Code).IsModified = true;
                }

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Relación establecida",
                    Body = "la relación con el código del proveedor ha sido actualizada correctamente",
                    JProperty = new { Price= model.Price, ProviderCode= model.ProviderCode }
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al crear realación",
                    Body = "Detalle del error " + ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult AddToCart(PurchaseItem item)
        {
            try
            {
                var eqv = db.Equivalences.FirstOrDefault(eq => eq.ProductId == item.ProductId && eq.ProviderId == item.ProviderId);

                if (eqv == null)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.InvalidData,
                        Header = "Sin código de proveedor",
                        Body = "Este producto no tiene realación con algun producto de proveedor"
                    });
                }

                item.UserId = User.Identity.GetUserId();

                var bProduct = db.BranchProducts.FirstOrDefault(bp => bp.BranchId == item.BranchId && bp.ProductId == item.ProductId);

                var totalQty = bProduct.Stock + item.Quantity;

                //valído no exceder el máximo
                if (totalQty > bProduct.Product.MaxQuantity)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.InvalidData,
                        Header = "Cantidad Excedente!",
                        Body = "La cantidad de a comprar mas la cantidad en inventario superan el máximo permitido, por favor verifica"
                    });
                }

                db.PurchaseItems.Add(item);
                db.SaveChanges();

                var model = GetCartItems(item.UserId);

                return PartialView("_PurchaseCartDetail", model);
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al agregar producto",
                    Body = "Detalle del error " + ex.Message
                });
            }

        }

        private List<ProductViewModel> GetCartItems(string userId)
        {
            var branches = User.Identity.GetBranches().Select(b => b.BranchId);

            var model = (from itm in db.PurchaseItems
                         join p in db.Products on itm.ProductId equals p.ProductId
                         join b in db.BranchProducts on new { itm.BranchId, itm.ProductId } equals new { b.BranchId, b.ProductId }

                         join e in db.Equivalences on new { itm.ProviderId, itm.ProductId } equals new { e.ProviderId, e.ProductId } into em
                         from eq in em.DefaultIfEmpty()
                         join ex in db.ExternalProducts on new { eq.ProviderId, eq.Code } equals new { ex.ProviderId, ex.Code } into exm
                         from ep in exm.DefaultIfEmpty()

                         where (itm.UserId == userId) &&
                               (branches.Contains(itm.Branch.BranchId))

                         select new ProductViewModel
                         {
                             ProductId   = p.ProductId,
                             Name        = p.Name,
                             Code        = p.Code,
                             TradeMark   = p.TradeMark,
                             MaxQuantity = b.MaxQuantity,
                             MinQuantity = b.MinQuantity,
                             Quantity    = b.Stock,
                             BranchName  = b.Branch.Name,
                             AddQuantity = itm.Quantity,
                             ProviderId  = itm.ProviderId,
                             ProviderName = itm.Provider.Name,
                             PurchaseType = itm.PurchaseTypeId,
                             ProviderCode = ep != null ? ep.Code : "No Asignado",
                             BuyPrice = ep != null ? ep.Price : Cons.Zero

                         }).OrderBy(p => p.Code).ToList();

            return model;
        }
    }

  
}