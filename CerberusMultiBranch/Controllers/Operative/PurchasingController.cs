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
using System.Text;
using CerberusMultiBranch.Models.Entities.Operative;
using System.Text.RegularExpressions;
using CerberusMultiBranch.Models.ViewModels.Purchasing;

namespace CerberusMultiBranch.Controllers.Operative
{

    public class PurchasingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        public ActionResult PurchaseOrders()
        {
            var st = new List<PStatus>();

            st.Add(PStatus.Canceled);
            st.Add(PStatus.Received);

            ViewBag.Branches = User.Identity.GetBranches().ToSelectList();
            ViewBag.PurchaseStatuses = db.PurchaseStatuses.ToSelectList();

            var model = db.PurchaseOrder.Where(o => !st.Contains(o.PurchaseStatusId)).OrderByDescending(o => o.InsDate).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult SearchPurchaseOrders(int? branchId, string folio, int? purchaseStatusId, string provider, DateTime? beginDate, DateTime? endDate)
        {

            var branchIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var model = (from o in db.PurchaseOrder

                         where (string.IsNullOrEmpty(folio) || o.Folio.Contains(folio)) &&
                         (branchId == null && branchIds.Contains(o.BranchId) || o.BranchId == branchId) &&
                         (purchaseStatusId == null || (int)o.PurchaseStatusId == purchaseStatusId) &&
                         (string.IsNullOrEmpty(provider) || o.Provider.Name.Contains(provider)) &&
                         (beginDate == null || o.InsDate >= beginDate) && (endDate == null || o.InsDate <= endDate)

                         select o).OrderByDescending(or => or.InsDate).ToList();



            return PartialView("_PurchaseOrderList", model);
        }

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
                              //join ex in db.ExternalProducts on new { eq.ProviderId, eq.Code } equals new { ex.ProviderId, ex.Code } into exm
                              //from ep in exm.DefaultIfEmpty()


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

        #region Búsqueda de Productos
        [HttpPost]
        public ActionResult ShowRequiredProductSearch(int? providerId)
        {
            var model = LookForPurchase(null, providerId.Value);


            return PartialView("_RequiredProductSearch", model);
        }

        [HttpPost]
        public ActionResult SearchRequiredProduct(string filter, int? providerId)
        {
            var model = LookForPurchase(filter, providerId.Value);


            return PartialView("_RequiredProductSearchList", model);
        }

        private List<RequiredProductGroupViewModel> LookForPurchase(string filter, int providerId)
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

            var groups = (from bp in db.BranchProducts

                          join e in db.Equivalences.Where(ev => ev.ProviderId == providerId) on bp.ProductId equals e.ProductId into em
                          from eq in em.DefaultIfEmpty()
                              //join ex in db.ExternalProducts on new { eq.ProviderId, eq.Code } equals new { ex.ProviderId, ex.Code } into exm
                              //from ep in exm.DefaultIfEmpty()
                          join it in db.PurchaseItems.Where(i => i.UserId == userId && i.ProviderId == providerId) on
                              new { bp.BranchId, bp.ProductId } equals new { it.BranchId, it.ProductId } into itm
                          from its in itm.DefaultIfEmpty()

                          where (string.IsNullOrEmpty(filter) || arr.All(s => (bp.Product.Code + " " + bp.Product.Name).Contains(s))) &&
                                 (branches.Contains(bp.BranchId)) &&
                                 (bp.Stock <= bp.MinQuantity) &&
                                 (bp.Product.IsActive)

                          select new ProductViewModel
                          {
                              ProductId = bp.ProductId,
                              Name = bp.Product.Name,
                              Code = bp.Product.Code,
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
                              AddToPurchaseDisabled = (its != null)

                          }).OrderBy(p => p.Code).Take(Cons.MaxProductResult).GroupBy(i => i.ProductId).ToList();


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
        #endregion

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


                    var total = (order.SubTotal + order.TaxAmount);

                    //si hay descuento
                    if (order.Discount > Cons.Zero && order.Discount < Cons.OneHundred)
                        total = total - (total * (order.Discount / Cons.OneHundred));

                    else if (order.Discount == Cons.OneHundred)
                        total = total - total;

                    //total de la mercancía con descuento e Iva mas envío y seguro
                    order.TotalDue = (total + order.Freight + order.Insurance).RoundMoney();

                    purchaseOrders.Add(order);
                }


                db.PurchaseOrder.AddRange(purchaseOrders);

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



        public ActionResult PurchaseOrder(int id)
        {
            ViewBag.PurchaseTypes = db.PurchaseTypes.ToSelectList();
            ViewBag.ShipMethodes = db.ShipMethodes.ToSelectList();

            var model = db.PurchaseOrder.Include(o => o.PurchaseOrderDetails.Select(d => d.Product.Equivalences)).Include(o => o.PurchaseOrderHistories).
                                                FirstOrDefault(o => o.PurchaseOrderId == id);

            foreach (var detail in model.PurchaseOrderDetails)
            {
                detail.ProviderCode = detail.Product.Equivalences.FirstOrDefault(e => e.ProviderId == model.ProviderId).Code;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult ReloadPurchaseOrder(int id)
        {
            ViewBag.PurchaseTypes = db.PurchaseTypes.ToSelectList();
            ViewBag.ShipMethodes = db.ShipMethodes.ToSelectList();

            var model = db.PurchaseOrder.Include(o => o.PurchaseOrderDetails.Select(d => d.Product.Equivalences)).Include(o => o.PurchaseOrderHistories).
                                               FirstOrDefault(o => o.PurchaseOrderId == id);

            foreach (var detail in model.PurchaseOrderDetails)
            {
                detail.ProviderCode = detail.Product.Equivalences.FirstOrDefault(e => e.ProviderId == model.ProviderId).Code;
            }

            return PartialView("_PurchaseOrder", model);
        }


        [HttpPost]
        public ActionResult ReceiveProduct(ProductReceptionViewModel item)
        {

            var detail = db.PurchaseOrderDetails.Include(d => d.StockMovements).FirstOrDefault(d => d.PurchaseOrderDetailId == item.DetailId);

            var model = new ProductReceptionViewModel
            {
                Description = detail.Product.Name,
                DetailId = detail.PurchaseOrderDetailId,
                MeasureUnit = detail.Product.Unit,
                ReceivedQty = detail.ReceivedQty,
                RequestedQty = detail.OrderQty,
                StockedQty = detail.StockedQty,
                ComplementQty = detail.ComplementQty,
                Discount = detail.Discount,
                IsCompleated = detail.IsCompleated,
                IsTrackable = detail.Product.IsTrackable,
                Serials = new List<SerialItemViewModel>(),
            };

            model.ReceiveDisabled = detail.PurchaseOrder.PurchaseStatusId != PStatus.Watting;
            model.ComplementDisabled = detail.PurchaseOrder.PurchaseStatusId != PStatus.Partial;

            model.SerialsSaved = detail.StockMovements.Where(m => m.TrackingItem != null).
                Select(m => new SerialItemViewModel
                {
                    SerialNumber = m.TrackingItem.SerialNumber,
                    InsDate = m.TrackingItem.InsDate,
                    InsUser = m.TrackingItem.InsUser
                }).ToList();


            if (item.HasValues)
            {
                model.Comment = item.Comment;
                model.ComplementQty = item.ComplementQty;
                model.ReceivedQty = item.ReceivedQty;
                model.StockedQty = item.StockedQty;
                model.IsCompleated = item.IsCompleated;
                model.Serials = item.Serials != null ? item.Serials : new List<SerialItemViewModel>();
            }
            else
                model.Serials = new List<SerialItemViewModel>();

            return PartialView("_ReceivePurchaseItem", model);
        }


        [HttpPost]
        public ActionResult CompleateReception(List<ProductReceptionViewModel> items, int purchaseOrderId, string comment, int? shipMethodId, double? freight, double? insurance, double? discount)
        {
            try
            {
                //obtengo el registro de la orden de compra guardado
                var order = db.PurchaseOrder.Include(o => o.PurchaseOrderDetails.Select(d => d.Product)).Include(o => o.PurchaseOrderHistories).
                                                FirstOrDefault(o => o.PurchaseOrderId == purchaseOrderId);


                //si el status no es envíado o recibido parcial, no se puede realizar una recepción
                if (order.PurchaseStatusId != PStatus.Watting && order.PurchaseStatusId != PStatus.Partial)
                {
                    return Json(new JResponse
                    {
                        Header = "Operación denegada",
                        Body = "Solo se pueden recibir ordenes en estado [En Espera] o [Recibido Parcial]",
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.Success,
                    });
                }


                var prodIds = order.PurchaseOrderDetails.Select(d => d.ProductId).ToList();

                //busco el inventario de todos los productos de la orden de compra
                var inventories = db.BranchProducts.Where(bp => bp.BranchId == order.BranchId && prodIds.Contains(bp.ProductId)).ToList();

                //recorro las partidas para generar los movimientos
                foreach (var detail in order.PurchaseOrderDetails)
                {
                    var item = items.FirstOrDefault(i => i.DetailId == detail.PurchaseOrderDetailId);

                    if (item != null)
                    {
                        detail.Discount = item.Discount;
                        detail.ReceivedQty = item.ReceivedQty;
                        detail.ComplementQty = item.ComplementQty;
                        detail.StockedQty = item.StockedQty;
                        detail.UpdDate = DateTime.Now.ToLocal();
                        detail.UpdUser = HttpContext.User.Identity.Name;
                        detail.Comment = item.Comment;
                        detail.IsCompleated = item.IsCompleated;


                        var inventory = inventories.FirstOrDefault(i => i.ProductId == detail.ProductId);

                        if (inventory.Product.StockRequired)
                        {
                            #region Numeros de serie y movimientos de inventario
                            List<TrackingItem> trackingItems = new List<TrackingItem>();
                            //si hay numeros de serie, se crean las instancias 
                            if (item.Serials != null)
                            {
                                item.Serials.ForEach(s =>
                                {
                                    //item rastreable por número de serie
                                    TrackingItem ti = new TrackingItem
                                    {
                                        SerialNumber = s.SerialNumber,
                                        ProductId = detail.ProductId,
                                        ItemLocations = new List<ItemLocation>(),
                                        StockMovements = new List<StockMovement>()
                                    };

                                    //se crea la relación del Item con el respectivo inventario
                                    ti.ItemLocations.Add(new ItemLocation { ProductId = detail.ProductId, BranchId = order.BranchId });


                                    //se crea el movimiento de inventario para el item
                                    ti.StockMovements.Add(new StockMovement
                                    {
                                        ProductId = detail.ProductId,
                                        BranchId = order.BranchId,
                                        MovementType = MovementType.Entry,
                                        PurchaseOrderDetailId = detail.PurchaseOrderDetailId,
                                        Comment = string.Format("ORDEN DE COMPRA [{0}]", order.Folio.ToUpper()),
                                        Quantity = Cons.One
                                    });

                                    trackingItems.Add(ti);
                                });
                            }

                            //si no hubo numeros de serie, registro un solo movimiento de inventario para la partida
                            if (trackingItems.Count == Cons.Zero)
                            {
                                var stockMovement = new StockMovement
                                {
                                    ProductId = detail.ProductId,
                                    BranchId = order.BranchId,
                                    MovementType = MovementType.Entry,
                                    PurchaseOrderDetailId = detail.PurchaseOrderDetailId,
                                    Comment = string.Format("ORDEN DE COMPRA [{0}]", order.Folio.ToUpper()),
                                    Quantity = (order.PurchaseStatusId == PStatus.Watting ? detail.ReceivedQty : detail.ComplementQty)
                                };

                                //agrego el movimiento al contexto
                                db.StockMovements.Add(stockMovement);
                            }
                            else
                            {
                                //agrego los objetos al contexto
                                db.TrackingItems.AddRange(trackingItems);
                            }
                            #endregion
                        }

                        if (order.PurchaseStatusId == PStatus.Watting)
                        {
                            var amount = detail.UnitPrice * (detail.IsCompleated ? detail.StockedQty : detail.OrderQty);

                            detail.LineTotal = (detail.Discount == (double)Cons.Zero ? amount.RoundMoney() : (amount - (amount * (detail.Discount / Cons.OneHundred)))).RoundMoney();

                            db.Entry(detail).Property(d => d.LineTotal).IsModified = true;
                        }

                        db.Entry(detail).Property(d => d.Discount).IsModified = true;
                        db.Entry(detail).Property(d => d.ReceivedQty).IsModified = true;
                        db.Entry(detail).Property(d => d.ComplementQty).IsModified = true;
                        db.Entry(detail).Property(d => d.StockedQty).IsModified = true;
                        db.Entry(detail).Property(d => d.UpdDate).IsModified = true;
                        db.Entry(detail).Property(d => d.UpdUser).IsModified = true;
                        db.Entry(detail).Property(d => d.Comment).IsModified = true;
                        db.Entry(detail).Property(d => d.IsCompleated).IsModified = true;
                    }
                }

                //agrego historico
                PurchaseOrderHistory history = new PurchaseOrderHistory
                {
                    PurchaseOrderId = order.PurchaseOrderId,
                    BeginDate = order.UpdDate,
                    EndDate = DateTime.Now.ToLocal(),
                    Comment = order.Comment,
                    ModifyByUser = order.UpdUser,
                    Status = order.PurchaseStatus.Name,
                    ShipMethod = order.ShipMethod.Name,
                    Type = order.PurchaseType.Name
                };

                db.PurchaseOrderHistories.Add(history);



                #region Precios y cantidades en stock

                
                var subTotal = order.PurchaseOrderDetails.Sum(d => d.LineTotal).RoundMoney();

                var taxAmount = (subTotal * (Cons.One + order.TaxRate / Cons.OneHundred)).RoundMoney();

                //calculo la suma de las partidas mas el IVA 
                var totalDisc = (subTotal + taxAmount).RoundMoney();

                //y menos el descuento
                if (discount > Cons.Zero && discount < Cons.OneHundred)
                    totalDisc = totalDisc - (totalDisc * (discount.Value / Cons.OneHundred));



                foreach (var detail in order.PurchaseOrderDetails)
                {
                    var expensesLine = 0d;

                    var inventory = inventories.FirstOrDefault(p => p.ProductId == detail.ProductId);

                    var isModified = false;

                    //solo evaluo costos en la primera recepción, ya que ahi se indica si se contabiliza lo almacenado o lo pedido
                    if (order.PurchaseStatusId == PStatus.Watting)
                    {
                        //el total de la línea mas el iva
                        var lineTotal = (detail.LineTotal * (Cons.One + order.TaxRate / Cons.OneHundred)).RoundMoney();

                        //y menos el descuento
                        if (discount > Cons.Zero && discount < Cons.OneHundred)
                            lineTotal = lineTotal - (lineTotal * (discount.Value / Cons.OneHundred));

                        //los gastos (envío mas seguro)
                        var expenses = freight.Value + insurance.Value;

                        //calculo la parte proporcional de gastos para la línea (reparto de forma proporcional al costo)
                        //el total de los gastos entre el total de las partidas (ya con iva y descuentos) por el total de cada partida
                        if (expenses > 0.0)
                            expensesLine = ((expenses / totalDisc) * lineTotal).RoundMoney();

                        lineTotal += expensesLine;

                        //obtengo el costo real individual de los articulos de la partida
                        var buyPrice = (lineTotal / (detail.IsCompleated ? detail.StockedQty : detail.OrderQty)).RoundMoney();

                        //cantidad actual en inventario
                        var currentStock = inventory.Stock + inventory.Reserved;

                        //si hay diferencia entre los precios de compra hago modificaciones
                        if (buyPrice != inventory.BuyPrice)
                        {
                            //si el producto subio de precio o bajó y no hay stock, actualizo el nuevo precio
                            if (buyPrice > inventory.BuyPrice || (buyPrice < inventory.BuyPrice && currentStock == Cons.Zero))
                                inventory.BuyPrice = buyPrice;
                            else
                            {
                                var currentAmount = (inventory.Stock + inventory.Reserved) * inventory.BuyPrice;

                                inventory.BuyPrice = ((lineTotal + currentAmount) / (inventory.Stock + inventory.Reserved + (detail.IsCompleated ? detail.StockedQty : detail.OrderQty))).RoundMoney();
                            }

                            //calculo los precios en base a la utilidad configurada, si no hay margenes de utilidad solo coloco el precio de compra
                            inventory.DealerPrice = inventory.DealerPercentage != Cons.Zero ?
                                                    inventory.BuyPrice * (Cons.One + inventory.DealerPercentage / Cons.OneHundred) : inventory.BuyPrice;
                            inventory.WholesalerPrice = inventory.WholesalerPercentage != Cons.Zero ?
                                                        inventory.BuyPrice * (Cons.One + inventory.WholesalerPercentage / Cons.OneHundred) : inventory.BuyPrice;
                            inventory.StorePrice = inventory.StorePercentage != Cons.Zero ?
                                                   inventory.BuyPrice * (Cons.One + inventory.StorePercentage / Cons.OneHundred) : inventory.BuyPrice;

                            //el cliente quiere precio cerrados sin centavos

                            inventory.DealerPrice = Math.Round(inventory.DealerPrice, Cons.Zero);
                            inventory.WholesalerPrice = Math.Round(inventory.WholesalerPrice, Cons.Zero);
                            inventory.StorePrice = Math.Round(inventory.StorePrice, Cons.Zero);

                            db.Entry(inventory).Property(i => i.DealerPrice).IsModified = true;
                            db.Entry(inventory).Property(i => i.WholesalerPrice).IsModified = true;
                            db.Entry(inventory).Property(i => i.StorePrice).IsModified = true;
                            db.Entry(inventory).Property(i => i.BuyPrice).IsModified = true;

                            isModified = true;
                        }
                    }


                    if (inventory.Product.StockRequired)
                    {
                        inventory.LastStock = inventory.Stock;
                        inventory.Stock += (order.PurchaseStatusId == PStatus.Watting ? detail.ReceivedQty : detail.ComplementQty);

                        db.Entry(inventory).Property(i => i.LastStock).IsModified = true;
                        db.Entry(inventory).Property(i => i.Stock).IsModified = true;
                        isModified = true;
                    }

                    if (isModified)
                    {
                        inventory.UpdDate = DateTime.Now.ToLocal();
                        inventory.UpdUser = HttpContext.User.Identity.Name;

                        db.Entry(inventory).Property(i => i.UpdDate).IsModified = true;
                        db.Entry(inventory).Property(i => i.UpdUser).IsModified = true;
                    }
                }



                #endregion



                if (order.PurchaseStatusId == PStatus.Watting)
                {
                    order.Freight      = freight.Value;
                    order.Insurance    = insurance.Value;
                    order.Discount     = discount.Value;
                    order.ShipMethodId = shipMethodId.Value;
                    order.SubTotal     = subTotal;
                    order.TaxAmount    = taxAmount;
                    order.TotalDue     = (totalDisc + freight.Value + order.Insurance).RoundMoney();

                    db.Entry(order).Property(o => o.Freight).IsModified = true;
                    db.Entry(order).Property(o => o.ShipMethodId).IsModified = true;
                    db.Entry(order).Property(o => o.SubTotal).IsModified = true;
                    db.Entry(order).Property(o => o.TaxAmount).IsModified = true;
                    db.Entry(order).Property(o => o.TotalDue).IsModified = true;
                }
                order.Comment = comment;
                order.UpdDate = DateTime.Now.ToLocal();
                order.UpdUser = HttpContext.User.Identity.Name;
                order.DeliveryDate = DateTime.Now.ToLocal();
                order.PurchaseStatusId = order.PurchaseOrderDetails.Count(d => !d.IsCompleated) > Cons.Zero ? PStatus.Partial : PStatus.Received;


                db.Entry(order).Property(o => o.Comment).IsModified = true;
                db.Entry(order).Property(o => o.UpdDate).IsModified = true;
                db.Entry(order).Property(o => o.UpdUser).IsModified = true;
                db.Entry(order).Property(o => o.DeliveryDate).IsModified = true;
                db.Entry(order).Property(o => o.PurchaseStatusId).IsModified = true;

                db.SaveChanges();

                return Json(new JResponse
                {
                    Header = "Mercancía recibida",
                    Body = "La orden de compra fue" + (order.PurchaseStatusId == PStatus.Received ? "recibida completamente" : "recibída parcialmente"),
                    Code = Cons.Responses.Codes.Success,
                    Result = (order.PurchaseStatusId == PStatus.Received ? Cons.Responses.Success : Cons.Responses.Info)
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Header = "Error al recibir los articulos",
                    Body = "Detalle del error: " + ex.Message,
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Danger
                });
            }
        }

        [HttpPost]
        public ActionResult BeginAction(int id)
        {
            var model = db.PurchaseOrder.FirstOrDefault(o => o.PurchaseOrderId == id);

            if (model.PurchaseStatusId == PStatus.Watting || model.PurchaseStatusId == PStatus.Watting)
                ViewBag.ShipMethodes = db.ShipMethodes.ToSelectList();


            //mando el comentario en blanco para que sea capturado uno nuevo
            model.Comment = string.Empty;

            return PartialView("_PurchaseOrderRevision", model);
        }

        [HttpPost]
        public ActionResult SetAction(int id, string comment, bool authorized, string to)
        {
            try
            {
                var purchaseOrder = db.PurchaseOrder.Include(o => o.PurchaseOrderDetails.Select(s => s.Product.Equivalences)).
                                    FirstOrDefault(o => o.PurchaseOrderId == id);

                PurchaseOrderHistory history = new PurchaseOrderHistory
                {
                    PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                    ShipMethod = purchaseOrder.ShipMethod.Name,
                    Status = purchaseOrder.PurchaseStatus.Name,
                    Type = purchaseOrder.PurchaseType.Name,
                    BeginDate = purchaseOrder.UpdDate,
                    ModifyByUser = purchaseOrder.UpdUser,
                    Comment = purchaseOrder.Comment
                };

                JResponse jresponse = null;

                switch (purchaseOrder.PurchaseStatusId)
                {
                    case PStatus.InRevision:
                        jresponse = SetRevision(purchaseOrder, comment, authorized);
                        break;
                    case PStatus.Revised:
                        jresponse = SetAuthorization(purchaseOrder, comment, authorized);
                        break;
                    case PStatus.Authorized:
                    case PStatus.SendingFailed:
                        if (authorized)
                            jresponse = SendOrder(purchaseOrder, to);
                        else
                            jresponse = SkipSending(purchaseOrder, comment);
                        break;
                }

                if ((bool)jresponse.JProperty == true)
                {
                    db.PurchaseOrderHistories.Add(history);
                    db.SaveChanges();
                }

                return Json(jresponse);
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Header = "Error al modificar la orden",
                    Body = "Detalle del error: " + ex.Message,
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Danger
                });
            }
        }

        [Authorize(Roles = "Almacenista, Administrador")]
        public JResponse SetRevision(PurchaseOrder purchaseOrder, string comment, bool authorized)
        {
            try
            {
                if (purchaseOrder.PurchaseStatusId != PStatus.InRevision)
                {
                    return new JResponse
                    {
                        Header = "Error al revisar",
                        Body = "La orden a revisar debe estar en estado [En revisión] ",
                        Code = Cons.Responses.Codes.Success,
                        Result = Cons.Responses.Warning,
                        JProperty = false
                    };
                }

                purchaseOrder.Comment = comment;
                purchaseOrder.PurchaseStatusId = authorized ? PStatus.Revised : PStatus.NotAuthorized;
                purchaseOrder.UpdDate = DateTime.Now.ToLocal();
                purchaseOrder.UpdUser = HttpContext.User.Identity.Name;


                db.Entry(purchaseOrder).Property(p => p.PurchaseStatusId).IsModified = true;
                db.Entry(purchaseOrder).Property(p => p.UpdDate).IsModified = true;
                db.Entry(purchaseOrder).Property(p => p.UpdUser).IsModified = true;
                db.Entry(purchaseOrder).Property(p => p.Comment).IsModified = true;

                return new JResponse
                {
                    Header = authorized ? "Orden Revisada!" : "Orden Rechazada",
                    Body = authorized ? string.Format("La orden de compra ha sido revisada y envíada a autorizacón") :
                                       string.Format("La orden de compra ha sido rechazada, comentario: {0}", comment),
                    Code = authorized ? Cons.Responses.Codes.Success : Cons.Responses.Codes.Success,
                    Result = authorized ? Cons.Responses.Success : Cons.Responses.Info,
                    JProperty = true
                };
            }
            catch (Exception ex)
            {
                return new JResponse
                {
                    Header = "Error al revisar la orden",
                    Body = "Detalle del error: " + ex.Message,
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Danger,
                    JProperty = false
                };
            }
        }


        [Authorize(Roles = "Supervisor,Administrador")]
        public JResponse SetAuthorization(PurchaseOrder purchaseOrder, string comment, bool authorized)
        {
            try
            {
                if (purchaseOrder.PurchaseStatusId != PStatus.Revised)
                {
                    return new JResponse
                    {
                        Header = "Error al autorizar",
                        Body = "La orden a autorizar debe estar en estado [Revisado]",
                        Code = Cons.Responses.Codes.Success,
                        Result = Cons.Responses.Warning,
                        JProperty = false
                    };
                }


                //si la orden fue autorizada, creo un folio
                if (authorized)
                {
                    //año en curso
                    var year = Convert.ToInt32(DateTime.Now.TodayLocal().ToString("yy"));

                    //status con folio
                    List<PStatus> slist = new List<PStatus>();
                    slist.Add(PStatus.Authorized);
                    slist.Add(PStatus.Canceled);
                    slist.Add(PStatus.Watting);
                    slist.Add(PStatus.SendingFailed);
                    slist.Add(PStatus.Received);
                    slist.Add(PStatus.Partial);

                    var lasOrder = db.PurchaseOrder.Where(o => slist.Contains(o.PurchaseStatusId) &&
                                                          o.BranchId == purchaseOrder.BranchId && o.Year == year).
                                                          OrderByDescending(s => s.Sequential).FirstOrDefault();

                    var seq = lasOrder != null ? lasOrder.Sequential : Cons.Zero;
                    seq++;

                    //creación del folio
                    purchaseOrder.Year = year;
                    purchaseOrder.Sequential = seq;
                    purchaseOrder.Folio = string.Format(Cons.Formats.PurchaseFolioMask, purchaseOrder.Branch.Code,
                                                year.ToString(Cons.Formats.YearFolioFormat), seq.ToString(Cons.Formats.PurchaseSeqFormat));

                    db.Entry(purchaseOrder).Property(p => p.Year).IsModified = true;
                    db.Entry(purchaseOrder).Property(p => p.Sequential).IsModified = true;
                    db.Entry(purchaseOrder).Property(p => p.Folio).IsModified = true;
                }

                purchaseOrder.Comment = comment;
                purchaseOrder.PurchaseStatusId = authorized ? PStatus.Authorized : PStatus.NotAuthorized;
                purchaseOrder.UpdDate = DateTime.Now.ToLocal();
                purchaseOrder.UpdUser = HttpContext.User.Identity.Name;


                db.Entry(purchaseOrder).Property(p => p.PurchaseStatusId).IsModified = true;
                db.Entry(purchaseOrder).Property(p => p.UpdDate).IsModified = true;
                db.Entry(purchaseOrder).Property(p => p.UpdUser).IsModified = true;
                db.Entry(purchaseOrder).Property(p => p.Comment).IsModified = true;


                return new JResponse
                {
                    Header = authorized ? "Orden Autorizada!" : "Orden Rechazada",
                    Body = authorized ? string.Format("La orden de compra ha sido autorizada con el folio {0}", purchaseOrder.Folio) :
                                        string.Format("La orden de compra ha sido rechazada, comentario: {0}", comment),
                    Code = authorized ? Cons.Responses.Codes.Success : Cons.Responses.Codes.Success,
                    Result = authorized ? Cons.Responses.Success : Cons.Responses.Info,
                    JProperty = true
                };
            }
            catch (Exception ex)
            {
                return new JResponse
                {
                    Header = "Error al autorizar la orden",
                    Body = "Detalle del error: " + ex.Message,
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Danger,
                    JProperty = false
                };
            }
        }

        public JResponse SkipSending(PurchaseOrder purchaseOrder, string comment)
        {
            if (purchaseOrder.PurchaseStatusId != PStatus.SendingFailed && purchaseOrder.PurchaseStatusId != PStatus.Authorized)
            {
                return new JResponse
                {
                    Header = "Estado incorrecto",
                    Body = "Esta acción requiere el estado [Autorizado] o [Envío Fallido] ",
                    Code = Cons.Responses.Codes.Success,
                    Result = Cons.Responses.Info,
                    JProperty = false
                };
            }

            purchaseOrder.Comment = comment;
            purchaseOrder.PurchaseStatusId = PStatus.Watting;
            purchaseOrder.UpdDate = DateTime.Now.ToLocal();
            purchaseOrder.UpdUser = HttpContext.User.Identity.Name;
            purchaseOrder.OrderDate = DateTime.Now.ToLocal();


            db.Entry(purchaseOrder).Property(p => p.PurchaseStatusId).IsModified = true;
            db.Entry(purchaseOrder).Property(p => p.UpdDate).IsModified = true;
            db.Entry(purchaseOrder).Property(p => p.UpdUser).IsModified = true;
            db.Entry(purchaseOrder).Property(p => p.Comment).IsModified = true;
            db.Entry(purchaseOrder).Property(p => p.OrderDate).IsModified = true;

            return new JResponse
            {
                Header = "Envío omitido",
                Body = "Se omitio el envío por correo, la orden esta en espera de mercancía",
                Code = Cons.Responses.Codes.Success,
                Result = Cons.Responses.Info,
                JProperty = true
            };
        }

        public JResponse SendOrder(PurchaseOrder model, string toAddress)
        {
            try
            {
                List<PStatus> slist = new List<PStatus>();
                slist.Add(PStatus.Authorized);
                slist.Add(PStatus.SendingFailed);


                if (!slist.Contains(model.PurchaseStatusId))
                {
                    return new JResponse
                    {
                        Header = "Envío no autorizado",
                        Body = "Solo se puede envíar una order con status Autorizado o Error de envío",
                        Code = Cons.Responses.Codes.Success,
                        Result = Cons.Responses.Info,
                        JProperty = false
                    };
                }


                foreach (var detail in model.PurchaseOrderDetails)
                    detail.ProviderCode = db.Equivalences.FirstOrDefault(e => e.ProviderId == model.ProviderId).Code;


                var body = PartialView("_PurchaseOrderRequest", model).RenderToString();

                //obtengo las variables de configuración
                var var = db.Variables.ToList();

                var cred = var.First(v => v.Name == ConfigVariable.Maling.PurchasingCredentials).Value.Split(Cons.SplitChar);
                var smtpSer = var.First(v => v.Name == ConfigVariable.Maling.SmtpHost).Value.Split(Cons.SplitChar);
                var sender = var.First(v => v.Name == ConfigVariable.Maling.PurchasingSender).Value;
                var logoPath = var.First(v => v.Name == ConfigVariable.Maling.PurchasingLogo).Value;
                var subject = var.First(v => v.Name == ConfigVariable.Maling.PurchasingSubject).Value;

                var mail = new MailSender(cred[Cons.Zero], cred[Cons.One], smtpSer[Cons.Zero], Convert.ToInt32(smtpSer[Cons.One]), sender);

                //agrego la imagen embebida
                Dictionary<string, string> att = new Dictionary<string, string>();
                att.Add(ConfigVariable.Maling.PurchasingLogo, Server.MapPath(logoPath));

                //agrego el destinatario
                List<string> to = new List<string>();
                to.Add(toAddress);

                try
                {
                    mail.SendMail(to, body, string.Format(subject, model.Folio), att);

                    model.Comment = string.Format("Orden envíada a {0}", toAddress);
                    model.PurchaseStatusId = PStatus.Watting;
                    model.OrderDate = DateTime.Now.ToLocal();
                }
                catch (Exception ex)
                {
                    model.Comment = string.Format("Error al enviar a [{0}] detalle [{1}]", toAddress, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                    model.PurchaseStatusId = PStatus.SendingFailed;
                }

                model.UpdDate = DateTime.Now.ToLocal();
                model.UpdUser = HttpContext.User.Identity.Name;

                db.Entry(model).Property(o => o.Comment).IsModified = true;
                db.Entry(model).Property(o => o.OrderDate).IsModified = true;
                db.Entry(model).Property(o => o.UpdUser).IsModified = true;
                db.Entry(model).Property(o => o.PurchaseStatusId).IsModified = true;
                db.Entry(model).Property(o => o.UpdDate).IsModified = true;

                return new JResponse
                {
                    Header = model.PurchaseStatusId == PStatus.SendingFailed ? "Fallo en el envío" : "Pedido envíado",
                    Body = model.Comment,
                    Code = Cons.Responses.Codes.Success,
                    Result = model.PurchaseStatusId == PStatus.SendingFailed ? Cons.Responses.Warning : Cons.Responses.Success,
                    JProperty = true
                };
            }
            catch (Exception ex)
            {
                return new JResponse
                {
                    Header = "Error al enviar la orden",
                    Body = "Detalle del error: " + ex.Message,
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Danger,
                    JProperty = false
                };
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