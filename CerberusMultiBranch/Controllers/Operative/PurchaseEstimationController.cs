using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Purchasing;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Models.ViewModels.Operative;
using CerberusMultiBranch.Models.ViewModels.Purchasing;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Operative
{
    [CustomAuthorize]
    public class PurchaseEstimationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Obtiene la sesión de compra del usuario en turno
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchaseEstimation()
        {
            var userId = User.Identity.GetUserId();

            var model = new PurchaseCartViewModel();
            model.PurchaseTypes = db.PurchaseTypes.ToSelectList();
            model.ShipmentMethodes = db.ShipMethodes.ToSelectList();
            model.Branches = User.Identity.GetBranches().OrderBy(b=> b.Name).ToSelectList();

            var items = GetEstimationDetails(userId);

            if (items.Count > Cons.Zero)
            {
                model.ProviderId = items.FirstOrDefault().Value.First().ProviderId;
                model.ProviderName = items.FirstOrDefault().Value.First().ProviderName;
                model.PurchaseItems = items;
            }

            return View(model);
        }



        private Dictionary<string, IEnumerable<ProductViewModel>> GetEstimationDetails(string userId)
        {
            var branches = User.Identity.GetBranches().Select(b => b.BranchId);

            var groups = (from itm in db.PurchaseItems
                          join p in db.Products on itm.ProductId equals p.ProductId
                          join b in db.BranchProducts on new { itm.BranchId, itm.ProductId } equals new { b.BranchId, b.ProductId }

                          join e in db.Equivalences on new { itm.ProviderId, itm.ProductId } equals new { e.ProviderId, e.ProductId } into em
                          from eq in em.DefaultIfEmpty()


                          where (itm.UserId == userId) &&
                                (branches.Contains(itm.Branch.BranchId))

                          select new ProductViewModel
                          {
                              ProductId = p.ProductId,
                              Name = p.Name,
                              Code = p.Code,
                              TradeMark = p.TradeMark,
                              MaxQuantity = b.MaxQuantity,
                              MinQuantity = b.MinQuantity,
                              Quantity = b.Stock,
                              BranchName = b.Branch.Name,
                              BranchId = itm.BranchId,
                              AddQuantity = itm.Quantity,
                              ProviderId = itm.ProviderId,
                              ProviderName = itm.Provider.Name,
                              TotalLine = itm.TotalLine,
                              ProviderCode = eq.Code,
                              BuyPrice = itm.Price,
                              Discount = itm.Discount

                          }).OrderBy(p => p.BranchName).GroupBy(p => p.BranchName).ToList();

            var model = new Dictionary<string, IEnumerable<ProductViewModel>>();

            groups.ForEach(group =>
            {
                var header = group.Key + " - Total " + group.Sum(p => p.TotalLine).ToMoney();
                model.Add(header, group);
            });


            return model;
        }

        [HttpPost]
        public ActionResult OpenSearchProducts(int providerId, int branchId)
        {
            var model = LookForPurchase(null, providerId, branchId);

            return PartialView("_SearchProductForOrder", model);
        }


        [HttpPost]
        public ActionResult SearchProducts(string filter, int providerId, int branchId)
        {
            var model = LookForPurchase(filter, providerId, branchId);

            return PartialView("_SearchProductForOrderList", model);
        }


        private List<ProductViewModel> LookForPurchase(string filter, int providerId, int branchId)
        {

            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
            {
                arr = filter.Trim().Split(' ');

                if (arr.Length == Cons.One)
                    arr[Cons.Zero] = Regex.Replace(arr[Cons.Zero], "[^a-zA-Z0-9]+", "");
            }

           
            var userId = User.Identity.GetUserId();

            var products = (from p in db.Products.Where(p => (string.IsNullOrEmpty(filter) || arr.All(s => (p.Code + " " + p.Name).Contains(s))) && p.IsActive)

                            join bp in db.BranchProducts on p.ProductId equals bp.ProductId

                            join e in db.Equivalences.Where(ev => ev.ProviderId == providerId) on bp.ProductId equals e.ProductId into em
                            from eq in em.DefaultIfEmpty()

                            join it in db.PurchaseItems.Where(i => i.UserId == userId && i.ProviderId == providerId) on
                                new { bp.BranchId, bp.ProductId } equals new { it.BranchId, it.ProductId } into itm
                            from its in itm.DefaultIfEmpty()

                            where
                                   (bp.BranchId == branchId) &&
                                   (bp.Stock < bp.MaxQuantity)

                            select new ProductViewModel
                            {
                                ProductId = bp.ProductId,
                                Name = p.Name,
                                Code = p.Code,
                                BranchId = bp.BranchId,
                                TradeMark = bp.Product.TradeMark,
                                LockDate = bp.LockDate,
                                UserLock = bp.UserLock,
                                MaxQuantity = bp.MaxQuantity,
                                MinQuantity = bp.MinQuantity,
                                Quantity = bp.Stock + bp.Reserved,
                                AddQuantity = its != null ? its.Quantity : bp.MaxQuantity - bp.Stock,
                                BranchName = bp.Branch.Name,
                                ProviderCode = eq != null ? eq.Code : "No asignado",
                                BuyPrice = eq != null ? eq.BuyPrice : Cons.Zero,
                                Discount = its != null ? its.Discount : Cons.Zero,

                                AddToPurchaseDisabled = (its != null)

                            }).OrderBy(p => p.Code).Take(Cons.MaxProductResult).ToList();

            return products;
        }



        private List<RequiredProductGroupViewModel> GroupByProduct(List<ProductViewModel> products)
        {
            var groups = products.GroupBy(p => p.ProductId);

            List<RequiredProductGroupViewModel> productGroups = new List<RequiredProductGroupViewModel>();

            foreach (var group in groups)
            {
                RequiredProductGroupViewModel pGroup = new RequiredProductGroupViewModel
                {
                    Code = group.First().Code,
                    Description = group.First().Name,
                    ProviderCode = group.First().ProviderCode,
                    ProductId = group.First().ProductId,
                    Unit = group.First().Unit,
                    TradeMark = group.First().TradeMark,
                    Branches = group
                };

                productGroups.Add(pGroup);
            }

            return productGroups;
        }

        [HttpPost]
        public ActionResult EditDetail(int productId, int branchId)
        {
            try
            {

                var userId = HttpContext.User.Identity.GetUserId();

                var model = (from itm in db.PurchaseItems.Where(i => i.ProductId == productId && i.BranchId == branchId && i.UserId == userId)
                             join p in db.Products on itm.ProductId equals p.ProductId
                             join b in db.BranchProducts on new { itm.BranchId, itm.ProductId } equals new { b.BranchId, b.ProductId }

                             join e in db.Equivalences on new { itm.ProviderId, itm.ProductId } equals new { e.ProviderId, e.ProductId } into em
                             from eq in em.DefaultIfEmpty()

                             select new ProductViewModel
                             {
                                 ProductId = p.ProductId,
                                 Name = p.Name,
                                 Code = p.Code,
                                 TradeMark = p.TradeMark,
                                 MaxQuantity = b.MaxQuantity,
                                 MinQuantity = b.MinQuantity,
                                 Quantity = b.Stock,
                                 BranchName = b.Branch.Name,
                                 BranchId = itm.BranchId,
                                 AddQuantity = itm.Quantity,
                                 ProviderId = itm.ProviderId,
                                 ProviderName = itm.Provider.Name,
                                 TotalLine = itm.TotalLine,
                                 ProviderCode = eq.Code,
                                 BuyPrice = itm.Price,
                                 Discount = itm.Discount

                             }).FirstOrDefault();



                return PartialView("_EditEstimationDetail", model);
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
        public ActionResult SetDatailChange(int productId, int branchId, double quantity, double discount)
        {
            try
            {
                var userId = HttpContext.User.Identity.GetUserId();

                var detail = db.PurchaseItems.FirstOrDefault(i => i.ProductId == productId && i.BranchId == branchId && i.UserId == userId);

                detail.Quantity = quantity;
                detail.Discount = discount;
                detail.TotalLine = discount == Cons.Zero ? (quantity * detail.Price) : (quantity * detail.Price) - ((quantity * detail.Price) * (discount / Cons.OneHundred));

                db.Entry(detail).Property(p => p.Quantity).IsModified = true;
                db.Entry(detail).Property(p => p.TotalLine).IsModified = true;
                db.Entry(detail).Property(p => p.Discount).IsModified = true;

                db.SaveChanges();

                var model = GetEstimationDetails(userId);

                return PartialView("_PurchaseEstimationDetails", model);

            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al guardar",
                    Body = "Detalle del error " + ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult BeginSetCode(int productId, int providerId)
        {
            try
            {
                var model = (from p in db.Products

                             join e in db.Equivalences.Where(ie => ie.ProviderId == providerId) on p.ProductId equals e.ProductId into em
                             from eq in em.DefaultIfEmpty()

                                 //join ex in db.ExternalProducts on new { eq.ProviderId, eq.Code } equals new { ex.ProviderId, ex.Code } into exm
                                 //from ep in exm.DefaultIfEmpty()

                             where (p.ProductId == productId)

                             select new SetProviderCodeViewModel
                             {
                                 ProviderCode = eq != null ? eq.Code : string.Empty,
                                 InternalCode = p.Code,
                                 ProductId = p.ProductId,
                                 ProviderId = providerId,
                                 Price = eq != null ? eq.BuyPrice : Cons.Zero
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
                var eqv = db.Equivalences.FirstOrDefault(eq => eq.ProviderId == model.ProviderId && eq.ProductId == model.ProductId);

                //si no hay una relación de equivalencia
                if (eqv == null)
                {
                    eqv = new Equivalence { Code = model.ProviderCode, ProductId = model.ProductId, ProviderId = model.ProviderId, BuyPrice = model.Price.RoundMoney() };
                    db.Equivalences.Add(eqv);
                }
                else
                {
                    eqv.Code = model.ProviderCode;
                    eqv.BuyPrice = model.Price.RoundMoney();
                    eqv.UpdDate = DateTime.Now.ToLocal();
                    eqv.UpdUser = HttpContext.User.Identity.Name;

                    db.Entry(eqv).Property(p => p.Code).IsModified = true;
                }

                var items = db.PurchaseItems.Where(i => i.ProductId == model.ProductId && i.ProviderId == model.ProviderId && i.Price != model.Price).ToList();

                //si hay alguna partidas de orden de compra en proceso, se ajusta el costo
                items.ForEach(item =>
                {
                    item.Price = model.Price;
                    item.TotalLine = (item.Price * item.Quantity).RoundMoney();

                    db.Entry(item).Property(i => i.TotalLine).IsModified = true;
                    db.Entry(item).Property(i => i.Price).IsModified = true;
                });

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Relación establecida",
                    Body = "la relación con el código del proveedor ha sido actualizada correctamente",
                    JProperty = new { Price = model.Price, ProviderCode = model.ProviderCode }
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
        public ActionResult RemoveAll(int providerId)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var items = db.PurchaseItems.Where(i => i.ProviderId == providerId && i.UserId == userId).ToList();

                if (items.Count > Cons.Zero)
                {
                    db.PurchaseItems.RemoveRange(items);
                    db.SaveChanges();
                }
                return Json(new JResponse { Result = Cons.Responses.Info, Code = Cons.Responses.Codes.Success, Header = "Elementos removidos", Body = "Se removieros todas las partidas" });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al remover partidas",
                    Body = "Detalle del error " + ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult RemoveFromEstimation(int branchId, int productId)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var item = db.PurchaseItems.FirstOrDefault(i => i.BranchId == branchId && i.ProductId == productId && i.UserId == userId);

                if (item != null)
                {
                    db.PurchaseItems.Remove(item);
                    db.SaveChanges();
                }

                var model = GetEstimationDetails(userId);

                return PartialView("_PurchaseEstimationDetails", model);
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al eliminar partida",
                    Body = "Detalle del error " + ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult AddToEstimation(PurchaseItem item)
        {
            try
            {
                item.UserId = User.Identity.GetUserId();

                var bProduct = db.BranchProducts.FirstOrDefault(bp => bp.BranchId == item.BranchId && bp.ProductId == item.ProductId);

                var totalQty = bProduct.Stock + bProduct.Reserved + item.Quantity;


                //valído no exceder el máximo
                if (totalQty > bProduct.MaxQuantity)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.InvalidData,
                        Header = "Cantidad Excedente!",
                        Body = "La cantidad de a comprar mas la cantidad en inventario superan el máximo permitido, por favor verifica"
                    });
                }

                //si hay descuento lo aplico
                item.TotalLine = item.Discount > Cons.Zero ? (item.Quantity * item.Price) - ((item.Quantity * item.Price) * (item.Discount / Cons.OneHundred)) : (item.Quantity * item.Price).RoundMoney();


                db.PurchaseItems.Add(item);
                db.SaveChanges();

                var model = GetEstimationDetails(item.UserId);

                return PartialView("_PurchaseEstimationDetails", model);
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


        [HttpPost]
        public ActionResult CreateOrders(PurchaseCartViewModel model)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var items = db.PurchaseItems.Where(i => i.ProviderId == model.ProviderId && i.UserId == userId).ToList();

                var vari = db.Variables.FirstOrDefault(v => v.Name == ConfigVariable.Pricing.IVA);

                var iva = Convert.ToDouble(vari.Value);

                if (items.Count == Cons.Zero)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.RecordNotFound,
                        Header = "Datos no encontrados",
                        Body = "No se encontraron elementos para generar alguna orden de compra"
                    });
                }


                var itemGroups = items.GroupBy(i => i.BranchId).ToList();

                List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

                foreach (var group in itemGroups)
                {
                    var first = group.First();

                    PurchaseOrder order = new PurchaseOrder
                    {
                        BranchId = first.BranchId,
                        ProviderId = model.ProviderId,
                        PurchaseTypeId = model.PurchaseType,
                        PurchaseOrderDetails = new List<PurchaseOrderDetail>(),
                        Comment = model.Comment,
                        DaysToPay = model.DaysToPay,
                        Freight = model.Freight,
                        Insurance = model.Insurance,
                        Discount = model.Discount,
                        ShipMethodId = model.ShipmentMethodId
                    };

                    foreach (var item in group)
                    {
                        PurchaseOrderDetail detail = new PurchaseOrderDetail
                        {
                            LineTotal = item.TotalLine,
                            OrderQty = item.Quantity,
                            UnitPrice = item.Price,
                            ProductId = item.ProductId,
                            Discount = item.Discount
                        };

                        order.PurchaseOrderDetails.Add(detail);
                    }


                    order.TaxRate = iva;
                    order.PurchaseStatusId = PStatus.InRevision;

                    //total de partidas
                    order.SubTotal = order.PurchaseOrderDetails.Sum(d => d.LineTotal).RoundMoney();

                    //monto de iva
                    order.TaxAmount = (order.SubTotal * (iva / Cons.OneHundred)).RoundMoney();

                    //calculo el total global, incluye envío y seguro
                    var total = (order.SubTotal + order.TaxAmount + order.Freight + order.Insurance);

                    //si hay descuento global lo aplico a todo
                    if (order.Discount > Cons.Zero)
                    {
                        total = total - (total * (order.Discount / Cons.OneHundred));
                    }


                    //total de la mercancía con descuento e Iva mas envío y seguro
                    order.TotalDue = total.RoundMoney();

                    purchaseOrders.Add(order);
                }


                db.PurchaseOrders.AddRange(purchaseOrders);

                db.PurchaseItems.RemoveRange(items);

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Orden generada",
                    Body = string.Format("Se generaron {0} ordernes de compra", purchaseOrders.Count)
                });

            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error al generar orden",
                    Body = "Detalle del error: " + ex.Message
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