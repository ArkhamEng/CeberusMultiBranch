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

namespace CerberusMultiBranch.Controllers.Operative
{
    public partial class PurchasesController : Controller
    {
        /// <summary>
        /// Obtiene la sesión de compra del usuario en turno
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchaseEstimation()
        {
            var userId = User.Identity.GetUserId();

            var model = new PurchaseCartViewModel();
            model.PurchaseTypes = db.PurchaseTypes.ToSelectList();

            var items = GetEstimationDetails(userId);

            if (items.Count > Cons.Zero)
            {
                model.ProviderId = items.FirstOrDefault().ProviderId;
                model.ProviderName = items.FirstOrDefault().ProviderName;
                model.PuschaseType = items.FirstOrDefault().PurchaseType;
                model.PurchaseItems = items;
            }

            return View(model);
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

                            join e in db.Equivalences.Where(ev => ev.ProviderId == providerId) on bp.ProductId equals e.ProductId into em
                            from eq in em.DefaultIfEmpty()
                            join ex in db.ExternalProducts on new { eq.ProviderId, eq.Code } equals new { ex.ProviderId, ex.Code } into exm
                            from ep in exm.DefaultIfEmpty()
                            join it in db.PurchaseItems.Where(i => i.UserId == userId && i.ProviderId == providerId) on
                                new { bp.BranchId, bp.ProductId } equals new { it.BranchId, it.ProductId } into itm
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
                                AddQuantity = its != null ? its.Quantity : bp.Product.MaxQuantity - bp.Stock,
                                BranchName = bp.Branch.Name,
                                ProviderCode = ep != null ? ep.Code : "No asignado",
                                BuyPrice = ep != null ? ep.Price : Cons.Zero,
                                AddToPurchaseDisabled = (its != null)

                            }).OrderBy(p => p.Code).Take(Cons.MaxProductResult).ToList();

            return products;
        }
        #endregion


        [HttpPost]
        public ActionResult BeginSetCode(int productId, int providerId)
        {
            try
            {
                var model = (from p in db.Products

                             join e in db.Equivalences.Where(ie => ie.ProviderId == providerId) on p.ProductId equals e.ProductId into em
                             from eq in em.DefaultIfEmpty()

                             join ex in db.ExternalProducts on new { eq.ProviderId, eq.Code } equals new { ex.ProviderId, ex.Code } into exm
                             from ep in exm.DefaultIfEmpty()

                             where (p.ProductId == productId)

                             select new SetProviderCodeViewModel
                             {
                                 ProviderCode = ep != null ? ep.Code : string.Empty,
                                 InternalCode = p.Code,
                                 ProductId = p.ProductId,
                                 ProviderId = providerId,
                                 Price = ep != null ? ep.Price : Cons.Zero
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
                if (eqv == null)
                {
                    eqv = new Equivalence { Code = model.ProviderCode, ProductId = model.ProductId, ProviderId = model.ProviderId };
                    db.Equivalences.Add(eqv);
                }
                else
                {
                    eqv.Code = model.ProviderCode;
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

                item.TotalLine = (item.Quantity * item.Price).RoundMoney();

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



        private List<ProductViewModel> GetEstimationDetails(string userId)
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
                             PurchaseType = itm.PurchaseTypeId,
                             ProviderCode = ep.Code,
                             BuyPrice = itm.Price

                         }).OrderBy(p => p.BranchName).ToList();

            model.ForEach(m =>
            {
                m.BranchName += " - Total " + model.Where(md => md.BranchId == m.BranchId).Sum(p => p.TotalLine).ToMoney();
            });

            return model;
        }

        [HttpPost]
        public ActionResult CreateOrders(int providerId)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var items = db.PurchaseItems.Where(i => i.ProviderId == providerId && i.UserId == userId).ToList();

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
                        OrderDate = DateTime.Now.TodayLocal(),
                        ProviderId = first.ProviderId,
                        PurchaseTypeId = first.PurchaseTypeId,
                        PurchaseOrderDetails = new List<PurchaseOrderDetail>(),
                        Comment = first.Comment,
                        DaysToPay = first.DaysToPay
                    };

                    foreach (var item in group)
                    {
                        PurchaseOrderDetail detail = new PurchaseOrderDetail
                        {
                            LineTotal = item.TotalLine,
                            OrderQty = item.Quantity,
                            UnitPrice = item.Price,
                            ProductId = item.ProductId,
                        };

                        order.PurchaseOrderDetails.Add(detail);
                    }


                    order.TaxRate = iva;
                    order.PurchaseStatusId = PStatus.InRevision;
                    order.ShipMethodId = Cons.One;
                    order.SubTotal  = order.PurchaseOrderDetails.Sum(d => d.LineTotal).RoundMoney();
                    order.TaxAmount = (order.SubTotal * (iva / 100)).RoundMoney();
                    order.TotalDue  = (order.SubTotal + order.TaxAmount).RoundMoney();
                    order.Comment   = items.First().Comment;

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

            var model = db.PurchaseOrder.Include(o => o.PurchaseOrderDetails.Select(d => d.Product)).Include(o => o.PurchaseOrderHistories).
                                                FirstOrDefault(o => o.PurchaseOrderId == id);

            return View(model);
        }

        [HttpPost]
        public ActionResult ReloadPurchaseOrder(int id)
        {
            ViewBag.PurchaseTypes = db.PurchaseTypes.ToSelectList();
            ViewBag.ShipMethodes = db.ShipMethodes.ToSelectList();

            var model = db.PurchaseOrder.Include(o => o.PurchaseOrderDetails.Select(d => d.Product)).Include(o => o.PurchaseOrderHistories).
                                                FirstOrDefault(o => o.PurchaseOrderId == id);

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
                RejectedQty = detail.RejectedQty,
                IsTrackable = detail.Product.IsTrackable,
                Serials = new List<SerialItemViewModel>(),
            };

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
                model.RejectedQty = item.RejectedQty;
                model.ReceivedQty = item.ReceivedQty;
                model.StocketQty = item.StocketQty;
                model.Serials = item.Serials != null ? item.Serials : new List<SerialItemViewModel>();
            }
            else
                model.Serials = new List<SerialItemViewModel>();

            return PartialView("_ReceivePurchaseItem", model);
        }

        public ActionResult CompleateReception(List<ProductReceptionViewModel> items, int purchaseOrderId)
        {
            try
            {
                //obtengo el registro de la orden de compra guardado
                var order = db.PurchaseOrder.Include(o => o.PurchaseOrderDetails.Select(d => d.Product)).Include(o => o.PurchaseOrderHistories).
                                                FirstOrDefault(o => o.PurchaseOrderId == purchaseOrderId);

                var prodIds = order.PurchaseOrderDetails.Select(d => d.ProductId).ToList();

                //busco el inventario de todos los productos de la orden de compra
                var inventories = db.BranchProducts.Where(bp => bp.BranchId == order.BranchId &&  prodIds.Contains(bp.ProductId)).ToList();


                //si el status no es envíado o recibido parcial, no se puede realizar una recepción
                if (order.PurchaseStatusId != PStatus.Sended || order.PurchaseStatusId == PStatus.Partial)
                {
                    return Json(new JResponse
                    {
                        Header ="Operación denegada",
                        Body   = "La orden de compra ya fue recibida o ha sido cancelada",
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.Success,
                    });
                }

                //recorro las partidas para generar los movimientos
                foreach(var detail in order.PurchaseOrderDetails)
                {
                    var item = items.FirstOrDefault(i => i.DetailId == detail.PurchaseOrderDetailId);

                    if(item != null)
                    {
                        detail.Discount    = item.Discount;
                        detail.ReceivedQty = item.ReceivedQty;
                        detail.RejectedQty = item.RejectedQty;
                        detail.StockedQty  = item.StocketQty;
                        detail.UpdDate     = DateTime.Now.ToLocal();
                        detail.UpdUser     = HttpContext.User.Identity.Name;
                        detail.Comment     = item.Comment;

                        if (item.IsCompleated)
                            detail.LineTotal = (detail.Discount == (double)Cons.Zero ? (detail.UnitPrice * detail.StockedQty) :
                            (detail.UnitPrice * detail.StockedQty) * (Cons.One + (detail.Discount / Cons.OneHundred))).RoundMoney();

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
                        if(trackingItems.Count == Cons.Zero)
                        {
                            var stockMovement = new StockMovement
                            {
                                ProductId    = detail.ProductId,
                                BranchId     = order.BranchId,
                                MovementType = MovementType.Entry,
                                PurchaseOrderDetailId = detail.PurchaseOrderDetailId,
                                Comment      = string.Format("ORDEN DE COMPRA [{0}]", order.Folio.ToUpper()),
                                Quantity     = detail.StockedQty
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

                        #region Precios y cantidades en stock
                        var inventory = inventories.FirstOrDefault(p => p.ProductId == detail.ProductId);

                        //obtengo el precio real individual para comparar con el precio actual
                        var newBuyPrice = (detail.LineTotal * (Cons.One + order.TaxRate / Cons.OneHundred)) / 
                                        (detail.IsCompleated ? detail.ReceivedQty : detail.StockedQty).RoundMoney();
                        
                        //obtengo el monto real de la línea
                        var detailAmount = (detail.LineTotal * (detail.IsCompleated ? detail.ReceivedQty : detail.StockedQty)).RoundMoney();

                        var stock = inventory.Stock + inventory.Reserved;

                        if (newBuyPrice != inventory.BuyPrice)
                        {
                            //si el preducto subio de precio, actualizo
                            if (newBuyPrice > inventory.BuyPrice || stock == Cons.Zero)
                                inventory.BuyPrice = newBuyPrice;
                            else
                            {
                                var currentAmount = (inventory.Stock + inventory.Reserved) * inventory.BuyPrice;

                                inventory.BuyPrice = (detailAmount + currentAmount) / (inventory.Stock + inventory.Reserved );
                            }
                        }

                        inventory.LastStock = inventory.Stock;
                        inventory.Stock += detail.StockedQty;
                        inventory.UpdDate = DateTime.Now.ToLocal();
                        inventory.UpdUser = HttpContext.User.Identity.Name;

                        db.Entry(inventory).Property(i => i.LastStock).IsModified = true;
                        db.Entry(inventory).Property(i => i.Stock).IsModified = true;
                        db.Entry(inventory).Property(i => i.UpdDate).IsModified = true;
                        db.Entry(inventory).Property(i => i.UpdUser).IsModified = true;
                        db.Entry(inventory).Property(i => i.BuyPrice).IsModified = true;

                        #endregion

                        db.Entry(detail).Property(d => d.Discount).IsModified    = true;
                        db.Entry(detail).Property(d => d.ReceivedQty).IsModified = true;
                        db.Entry(detail).Property(d => d.RejectedQty).IsModified = true;
                        db.Entry(detail).Property(d => d.StockedQty).IsModified = true;
                        db.Entry(detail).Property(d => d.UpdDate).IsModified = true;
                        db.Entry(detail).Property(d => d.UpdUser).IsModified = true;
                        db.Entry(detail).Property(d => d.LineTotal).IsModified = true;
                        db.Entry(detail).Property(d => d.Comment).IsModified = true;
                    }
                }


                PurchaseOrderHistory history = new PurchaseOrderHistory
                {
                    PurchaseOrderId = order.PurchaseOrderId,
                    BeginDate       = order.UpdDate,
                    EndDate      = DateTime.Now.ToLocal(),
                    Comment      = order.Comment,
                    ModifyByUser = order.UpdUser,
                    Status = order.PurchaseStatus.Name,
                    ShipMethod = order.ShipMethod.Name,
                    Type = order.PurchaseType.Name
                };

                db.PurchaseOrderHistories.Add(history);

                order.SubTotal  = order.PurchaseOrderDetails.Sum(d => d.LineTotal).RoundMoney();
                order.TaxAmount = (order.SubTotal * (Cons.One + (order.TaxRate / Cons.OneHundred))).RoundMoney();
                order.TotalDue  = (order.SubTotal + order.TaxAmount).RoundMoney();
                order.UpdDate   = DateTime.Now.ToLocal();
                order.UpdUser   = HttpContext.User.Identity.Name;
                order.DeliveryDate = DateTime.Now.ToLocal();
                order.PurchaseStatusId = order.PurchaseOrderDetails.Count(d => !d.IsCompleated) > Cons.Zero ? PStatus.Partial : PStatus.Received;

                db.Entry(order).Property(o => o.SubTotal).IsModified = true;
                db.Entry(order).Property(o => o.TaxAmount).IsModified = true;
                db.Entry(order).Property(o => o.TotalDue).IsModified = true;
                db.Entry(order).Property(o => o.UpdDate).IsModified = true;
                db.Entry(order).Property(o => o.UpdUser).IsModified = true;
                db.Entry(order).Property(o => o.Comment).IsModified = true;
                db.Entry(order).Property(o => o.Comment).IsModified = true;


                db.SaveChanges();

                return Json("OK");
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
        public ActionResult BeginRevision(int id)
        {
            var model = db.PurchaseOrder.FirstOrDefault(o => o.PurchaseOrderId == id);

            if (model.PurchaseStatusId == PStatus.Authorized || model.PurchaseStatusId == PStatus.NotAuthorized)
            {
                return Json(new JResponse
                {
                    Header = "Orden Invalida!",
                    Body = "Esta orden ya ha sido revisada previamente",
                    Code = Cons.Responses.Codes.Success,
                    Result = Cons.Responses.Info
                });
            }

            return PartialView("_PurchaseOrderRevision", model);
        }

        [HttpPost]
        public ActionResult SetRevision(int id, string comment, bool autorized)
        {
            try
            {
                var purchaseOrder = db.PurchaseOrder.FirstOrDefault(o => o.PurchaseOrderId == id);

                if (purchaseOrder.PurchaseStatusId == PStatus.Authorized)
                {
                    return Json(new JResponse
                    {
                        Header = "Orden Invalida!",
                        Body = "Esta orden ya ha sido autorizada previamente",
                        Code = Cons.Responses.Codes.Success,
                        Result = Cons.Responses.Info
                    });
                }

                purchaseOrder.OrderDate = DateTime.Now.ToLocal();

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

                //año en curso
                var year = Convert.ToInt32(DateTime.Now.TodayLocal().ToString("yy"));

                var lasOrder = db.PurchaseOrder.Where(o => o.PurchaseStatusId != PStatus.InRevision || o.PurchaseStatusId != PStatus.NotAuthorized &&
                                                      o.BranchId == purchaseOrder.BranchId && o.Year == year).
                                                      OrderByDescending(s => s.Sequential).FirstOrDefault();

                var seq = lasOrder != null ? lasOrder.Sequential : Cons.Zero;
                seq++;

                if (autorized)
                {
                    //creación del folio
                    purchaseOrder.Year = year;
                    purchaseOrder.Sequential = seq;
                    purchaseOrder.Folio = string.Format(Cons.Formats.PurchaseFolioMask, purchaseOrder.Branch.Code,
                                                year.ToString(Cons.Formats.YearFolioFormat), seq.ToString(Cons.Formats.PurchaseSeqFormat));
                }

                purchaseOrder.Comment = comment;
                purchaseOrder.PurchaseStatusId = autorized ? PStatus.Authorized : PStatus.NotAuthorized;
                purchaseOrder.UpdDate = DateTime.Now.ToLocal();
                purchaseOrder.UpdUser = HttpContext.User.Identity.Name;

                db.Entry(purchaseOrder).Property(p => p.PurchaseStatusId).IsModified = true;
                db.Entry(purchaseOrder).Property(p => p.UpdDate).IsModified = true;
                db.Entry(purchaseOrder).Property(p => p.UpdUser).IsModified = true;
                db.Entry(purchaseOrder).Property(p => p.Comment).IsModified = true;


                if (autorized)
                {//almaceno datos del folio
                    db.Entry(purchaseOrder).Property(p => p.Year).IsModified = true;
                    db.Entry(purchaseOrder).Property(p => p.Sequential).IsModified = true;
                    db.Entry(purchaseOrder).Property(p => p.Folio).IsModified = true;
                }

                db.PurchaseOrderHistories.Add(history);

                db.SaveChanges();

                return Json(new JResponse
                {
                    Header = autorized ? "Orden Autorizada!" : "Orden Rechazada",
                    Body = autorized ? string.Format("La orden de compra ha sido autorizada con el folio {0}", purchaseOrder.Folio) :
                                        string.Format("La orden de compra ha sido rechazada, comentario: {0}", comment),
                    Code = autorized ? Cons.Responses.Codes.Success : Cons.Responses.Codes.Success,
                    Result = autorized ? Cons.Responses.Success : Cons.Responses.Info
                });
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



        [HttpPost]
        public ActionResult SendPurchaseOrder(int id)
        {
            try
            {
                var model = db.PurchaseOrder.Include(o => o.PurchaseOrderDetails.Select(d => d.Product)).
                                            FirstOrDefault(o => o.PurchaseOrderId == id);

                if (model.PurchaseStatusId != PStatus.Authorized)
                {
                    return Json(new JResponse
                    {
                        Header = "Envío no autorizado",
                        Body = "Solo se puede envíar una order con status Autorizado",
                        Code = Cons.Responses.Codes.Success,
                        Result = Cons.Responses.Info
                    });
                }

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
                to.Add(model.Provider.Email);


                mail.SendMail(to, body, string.Format(subject, model.Folio), att);


                PurchaseOrderHistory history = new PurchaseOrderHistory
                {
                    BeginDate = model.UpdDate,
                    EndDate = DateTime.Now.ToLocal(),
                    ModifyByUser = HttpContext.User.Identity.Name,
                    Comment = model.Comment,
                    PurchaseOrderId = model.PurchaseOrderId,
                    ShipMethod = model.ShipMethod.Name,
                    Status = model.PurchaseStatus.Name,
                    Type = model.PurchaseType.Name
                };

                db.PurchaseOrderHistories.Add(history);

                model.OrderDate = DateTime.Now.ToLocal();
                model.PurchaseStatusId = PStatus.Sended;
                model.UpdUser = HttpContext.User.Identity.Name;


                db.Entry(model).Property(o => o.UpdUser).IsModified = true;
                db.Entry(model).Property(o => o.PurchaseStatusId).IsModified = true;
                db.Entry(model).Property(o => o.UpdUser).IsModified = true;

                db.SaveChanges();

                return Json(new JResponse
                {
                    Header = "Pedido envíado",
                    Body = "La Orden de compra ha sido enviada al destinatario " + to.First().ToLower(),
                    Code = Cons.Responses.Codes.Success,
                    Result = Cons.Responses.Success
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Header = "Error al enviar la orden",
                    Body = "Detalle del error: " + ex.Message,
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Danger
                });
            }
        }

    }

}