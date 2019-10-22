using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.Entities.Purchasing;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Models.ViewModels.Operative;
using CerberusMultiBranch.Models.ViewModels.Purchasing;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Util;

namespace CerberusMultiBranch.Controllers.Operative
{
    [CustomAuthorize]
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

            var model = db.PurchaseOrders.Where(o => !st.Contains(o.PurchaseStatusId)).OrderByDescending(o => o.InsDate).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult SearchPurchaseOrders(int? branchId, string folio, List<PStatus> status, string provider, DateTime? beginDate, DateTime? endDate)
        {
            var branchIds = User.Identity.GetBranches().Select(b => b.BranchId);

            status = (status == null) ? new List<PStatus>() : status;

            var model = db.PurchaseOrders.Where(o =>
                         (status.Count == Cons.Zero || status.Contains(o.PurchaseStatusId)) &&

                         (string.IsNullOrEmpty(folio) || o.Folio.Contains(folio)) &&

                         (branchId == null && branchIds.Contains(o.BranchId) || o.BranchId == branchId) &&

                         (string.IsNullOrEmpty(provider) || (o.Folio + " " + o.Provider.Name).Contains(provider)) &&

                         (beginDate == null || o.InsDate >= beginDate) && (endDate == null || o.InsDate <= endDate)).
                         OrderByDescending(or => or.InsDate).ToList();


            return PartialView("_PurchaseOrderList", model);
        }


        #region Búsqueda de Productos

        [HttpPost]
        public ActionResult OpenSearchProducts(int providerId, int branchId, int purchaseOrderId)
        {
            var model = LookForPurchase(null, providerId, branchId, purchaseOrderId);

            return PartialView("_SearchProductForOrder", model);
        }


        [HttpPost]
        public ActionResult SearchProducts(string filter, int providerId, int branchId, int purchaseOrderId)
        {
            var model = LookForPurchase(filter, providerId, branchId, purchaseOrderId);

            return PartialView("_SearchProductForOrderList", model);
        }


        private List<ProductViewModel> LookForPurchase(string filter, int providerId, int branchId, int purchaseOrderId)
        {

            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
            {
                arr = filter.Trim().Split(' ');

                if (arr.Length == Cons.One)
                    arr[Cons.Zero] = Regex.Replace(arr[Cons.Zero], "[^a-zA-Z0-9]+", "");
            }

            var products = (from p in db.Products.Where(p => (string.IsNullOrEmpty(filter) || arr.All(s => (p.Code + " " + p.Name).Contains(s))) && p.IsActive)

                            join bp in db.BranchProducts on p.ProductId equals bp.ProductId

                            join d in db.PurchaseOrderDetails.Where(d => d.PurchaseOrderId == purchaseOrderId) on p.ProductId equals d.ProductId into dm
                            from od in dm.DefaultIfEmpty()

                            join e in db.Equivalences.Where(ev => ev.ProviderId == providerId) on bp.ProductId equals e.ProductId into em
                            from eq in em.DefaultIfEmpty()

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
                                AddQuantity = bp.MaxQuantity - bp.Stock,
                                BranchName = bp.Branch.Name,
                                ProviderCode = eq != null ? eq.Code : "No asignado",
                                BuyPrice = eq != null ? eq.BuyPrice  : Cons.Zero,
                                Discount =  Cons.Zero,
                                //AddToPurchaseDisabled = (od != null)

                            }).OrderBy(p => p.Code).Take(Cons.MaxProductResult).ToList();

            return products;
        }

        #endregion




        [HttpGet]
        public ActionResult PurchaseOrder(int? id)
        {
            ViewBag.PurchaseTypes = db.PurchaseTypes.ToSelectList();
            ViewBag.ShipMethodes = db.ShipMethodes.ToSelectList();

            PurchaseOrder model = null;

            if (id != null)
                model = db.PurchaseOrders.Include(o => o.PurchaseOrderDetails.Select(d => d.Product.Equivalences)).
                   Include(o => o.PurchaseOrderHistories).Include(p => p.Purchases).
                                                   FirstOrDefault(o => o.PurchaseOrderId == id);
            else
            {
                model = new PurchaseOrder();
                model.PurchaseOrderDetails = new List<PurchaseOrderDetail>();
                model.PurchaseOrderHistories = new List<PurchaseOrderHistory>();
            }


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

            var model = db.PurchaseOrders.Include(o => o.PurchaseOrderDetails.Select(d => d.Product.Equivalences)).
                Include(o => o.PurchaseOrderHistories).Include(p => p.Purchases).
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
                UnitPrice = detail.UnitPrice,
                IsTrackable = detail.Product.IsTrackable,
                Serials = new List<SerialItemViewModel>(),
            };

            model.ReceiveDisabled = detail.PurchaseOrder.PurchaseStatusId != PStatus.Watting;
            model.ComplementDisabled = detail.PurchaseOrder.PurchaseStatusId != PStatus.Partial;

            if (detail.PurchaseOrder.PurchaseStatusId == PStatus.Partial && !detail.IsCompleated)
            {
                model.ComplementQty = model.RequestedQty - model.ReceivedQty;
                model.StockedQty = model.RequestedQty;
            }


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
                model.Discount = item.Discount;
                model.UnitPrice = item.UnitPrice;
                model.Serials = item.Serials != null ? item.Serials : new List<SerialItemViewModel>();
            }
            else
                model.Serials = new List<SerialItemViewModel>();

            return PartialView("_ReceivePurchaseItem", model);
        }

        [HttpPost]
        public ActionResult ViewDetail(int id)
        {
            var detail = db.PurchaseOrderDetails.Include(d => d.StockMovements).FirstOrDefault(d => d.PurchaseOrderDetailId == id);

            var model = new ProductReceptionViewModel
            {
                Description = detail.Product.Name,
                DetailId    = detail.PurchaseOrderDetailId,
                MeasureUnit = detail.Product.Unit,
                ReceivedQty = detail.ReceivedQty,
                RequestedQty = detail.OrderQty,
                StockedQty = detail.StockedQty,
                ComplementQty = detail.ComplementQty,
                Discount = detail.Discount,
                UnitPrice = detail.UnitPrice,
                IsCompleated = detail.IsCompleated,
                IsTrackable = detail.Product.IsTrackable,
                Comment = detail.Comment,
            };

            model.SerialsSaved = detail.StockMovements.Where(m => m.TrackingItem != null).
              Select(m => new SerialItemViewModel
              {
                  SerialNumber = m.TrackingItem.SerialNumber,
                  InsDate = m.TrackingItem.InsDate,
                  InsUser = m.TrackingItem.InsUser
              }).ToList();

            return PartialView("_ReceptionDetail", model);

        }


        [HttpPost]
        public ActionResult CompleateReception(List<ProductReceptionViewModel> items, int purchaseOrderId, string comment, int? shipMethodId, double? freight, double? insurance, double? discount)
        {
            try
            {
                //obtengo el registro de la orden de compra guardado
                var order = db.PurchaseOrders.Include(o => o.PurchaseOrderDetails.Select(d => d.Product)).
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

                //Movimientos y seriales
                SetMovements(order, inventories, items);

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

                //calculo de precios y actualización de stock
                SetPricesAndStock(order, inventories, discount, freight, insurance, shipMethodId);


                order.Comment = comment;
                order.UpdDate = DateTime.Now.ToLocal();
                order.UpdUser = HttpContext.User.Identity.Name;
                order.DeliveryDate = DateTime.Now.ToLocal();
                order.PurchaseStatusId = order.PurchaseOrderDetails.Count(d => !d.IsCompleated) > Cons.Zero ? PStatus.Partial : PStatus.Received;

                db.Entry(order);

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

        private void SetMovements(PurchaseOrder order, List<BranchProduct> inventories, List<ProductReceptionViewModel> items)
        {
            foreach (var detail in order.PurchaseOrderDetails)
            {
                var item = items.FirstOrDefault(i => i.DetailId == detail.PurchaseOrderDetailId);

                if (item != null)
                {
                    detail.Discount = item.Discount;
                    detail.ReceivedQty = item.ReceivedQty;
                    detail.UnitPrice = item.UnitPrice;
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

                    db.Entry(detail);
                }
            }
        }

        private void SetPricesAndStock(PurchaseOrder order, List<BranchProduct> inventories, double? discount, double? freight, double? insurance, int? shipMethodId)
        {
            var subTotal = order.PurchaseOrderDetails.Sum(d => d.LineTotal).RoundMoney();

            var taxAmount = (subTotal * (order.TaxRate / Cons.OneHundred)).RoundMoney();

            //calculo la suma de las partidas mas el IVA 
            var totalParts = (subTotal + taxAmount).RoundMoney();

            //los gastos (envío mas seguro)
            var expenses = (freight ?? order.Freight) + (insurance ?? order.Insurance);


            //aplico los descuentos a al total de las partidas y a los gastos por separado
            if (discount > Cons.Zero)
            {
                totalParts = totalParts - (totalParts * (discount.Value / Cons.OneHundred));

                expenses = expenses - (expenses * (discount.Value / Cons.OneHundred));
            }

            //para calcular la proporcion de gasto (envío y seguro) de cada partida, tomare como base el costo unitario con
            //con su respectivo descuento, esto para tener una mejor referencia de la equivalencia, 
            //si el descuento es 100% no lo aplicare para la referencia 
            var totalRef = order.PurchaseOrderDetails.Where(d => (d.IsCompleated && d.StockedQty > 0d) || (!d.IsCompleated)).
                         Sum(d =>
                         {
                             if (d.Discount > Cons.Zero && d.Discount < Cons.OneHundred)
                                 return d.UnitPrice - (d.UnitPrice * (discount / Cons.OneHundred));
                             else
                                 return d.UnitPrice;
                         });

            //total de partidas a analizar
            var partCount = order.PurchaseOrderDetails.Where(d => (d.IsCompleated && d.StockedQty > 0d) || (!d.IsCompleated)).Count();

            //itero en los completados con stock o los pendientes de recibir
            foreach (var detail in order.PurchaseOrderDetails.Where(d => (d.IsCompleated && d.StockedQty > 0d) || (!d.IsCompleated)))
            {
                var expensesLine = 0d;

                var inventory = inventories.FirstOrDefault(p => p.ProductId == detail.ProductId);

                var isModified = false;

                //solo evaluo costos en la primera recepción, ya que ahi se indica si se contabiliza lo almacenado o lo pedido
                if (order.PurchaseStatusId == PStatus.Watting)
                {
                    var priceRef = detail.UnitPrice;

                    //si hay descuento en la partida (menor al 100%) lo aplico
                    if (detail.Discount > Cons.Zero && detail.Discount < Cons.OneHundred)
                        priceRef = priceRef - (priceRef * (detail.Discount / Cons.OneHundred));

                    //calculo la parte proporcional de gastos para la línea (reparto de forma proporcional en base al precio unitario)
                    //gastos entre total de precios multiplicado por el precio de la línea
                    if (expenses > 0d)
                        expensesLine = priceRef * (expenses / totalRef.Value);


                    //el total de la línea mas el iva (aqui ya viene calculado el descuento por partida)
                    priceRef = (priceRef * (Cons.One + order.TaxRate / Cons.OneHundred)).RoundMoney();

                    //si hay descuento global lo aplico a los gastos y al total de la línea
                    if (discount > Cons.Zero)
                        priceRef = priceRef - (priceRef * (discount.Value / Cons.OneHundred));

                    //calcúlo el gasto por unidad
                    var unitExpenses = (expensesLine / (detail.IsCompleated ? detail.StockedQty : detail.OrderQty));

                    //obtengo el costo real  de los articulos de la partida
                    var buyPrice = (priceRef + unitExpenses).RoundMoney();

                    //cantidad actual en inventario
                    var currentStock = inventory.Stock + inventory.Reserved;

                    //si hay diferencia entre los precios de compra hago modificaciones
                    if (buyPrice != inventory.BuyPrice)
                    {

                        //si el producto subio de precio o bajó y no hay stock , actualizo el nuevo precio 
                        if (buyPrice > inventory.BuyPrice || (buyPrice < inventory.BuyPrice && buyPrice > 0d && currentStock == 0d))
                            inventory.BuyPrice = Math.Round(buyPrice, 2);

                        else
                        {
                            var currentAmount = (inventory.Stock + inventory.Reserved) * inventory.BuyPrice;

                            var newAmount = buyPrice * (detail.IsCompleated ? detail.StockedQty : detail.OrderQty);

                            inventory.BuyPrice = Math.Round((newAmount + currentAmount) / (inventory.Stock + inventory.Reserved + (detail.IsCompleated ? detail.StockedQty : detail.OrderQty)), 2);
                        }


                        //calculo los precios en base a la utilidad configurada, si no hay margenes de utilidad solo coloco el precio de compra
                        inventory.DealerPrice = inventory.DealerPercentage != Cons.Zero ?
                                                inventory.BuyPrice * (Cons.One + inventory.DealerPercentage / Cons.OneHundred) : inventory.BuyPrice;

                        inventory.WholesalerPrice = inventory.WholesalerPercentage != Cons.Zero ?
                                                    inventory.BuyPrice * (Cons.One + inventory.WholesalerPercentage / Cons.OneHundred) : inventory.BuyPrice;

                        inventory.StorePrice = inventory.StorePercentage != Cons.Zero ?
                                               inventory.BuyPrice * (Cons.One + inventory.StorePercentage / Cons.OneHundred) : inventory.BuyPrice;
                        //si es sucursal web
                        if (inventory.Branch.IsWebStore)
                        {
                            inventory.OnlinePrice = inventory.OnlinePercentage != Cons.Zero ?
                                                   inventory.BuyPrice * (Cons.One + inventory.OnlinePercentage / Cons.OneHundred) : inventory.BuyPrice;

                            inventory.OnlinePrice = Math.Round(inventory.OnlinePrice, Cons.Zero);
                        }


                        //el cliente quiere precio cerrados sin centavos

                        inventory.DealerPrice = Math.Round(inventory.DealerPrice, Cons.Zero);
                        inventory.WholesalerPrice = Math.Round(inventory.WholesalerPrice, Cons.Zero);
                        inventory.StorePrice = Math.Round(inventory.StorePrice, Cons.Zero);
                        

                        db.Entry(inventory).Property(i => i.DealerPrice).IsModified = true;
                        db.Entry(inventory).Property(i => i.WholesalerPrice).IsModified = true;
                        db.Entry(inventory).Property(i => i.StorePrice).IsModified = true;
                        db.Entry(inventory).Property(i => i.OnlinePrice).IsModified = true;
                        db.Entry(inventory).Property(i => i.BuyPrice).IsModified = true;

                        isModified = true;
                    }
                }

                var equ = db.Equivalences.FirstOrDefault(p => p.ProductId == detail.ProductId && p.ProviderId == order.ProviderId);

                if (equ != null && equ.BuyPrice != detail.UnitPrice)
                {
                    equ.BuyPrice = detail.UnitPrice;
                    equ.UpdDate = DateTime.Now.ToLocal();
                    equ.UpdUser = User.Identity.Name;
                    db.Entry(equ);
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

            if (order.PurchaseStatusId == PStatus.Watting)
            {
                order.Freight = freight.Value;
                order.Insurance = insurance.Value;
                order.Discount = discount.Value;
                order.ShipMethodId = shipMethodId.Value;
                order.SubTotal = subTotal;
                order.TaxAmount = taxAmount;
                order.TotalDue = (totalParts + expenses).RoundMoney();
            }
        }

        [HttpPost]
        public ActionResult BeginBilling(int id)
        {
            var model = db.PurchaseOrders.Select(o => new BillingViewModel
            {
                OrderId = o.PurchaseOrderId,
                BillTotal = o.TotalDue
            }).FirstOrDefault(o => o.OrderId == id);

            return PartialView("_CreateBill", model);
        }

        [HttpPost]
        public ActionResult CreateBill(BillingViewModel model)
        {
            try
            {
                var order = db.PurchaseOrders.Include(o => o.PurchaseOrderDetails.Select(d => d.Product.Equivalences)).
                                            FirstOrDefault(o => o.PurchaseOrderId == model.OrderId);

                order.Bill = model.BillNumber;

                Purchase pBill = new Purchase
                {
                    Bill = model.BillNumber,
                    Comment = model.BillComment,
                    Status = TranStatus.Reserved,
                    TransactionDate = order.DeliveryDate.Value,
                    Expiration = order.DeliveryDate.Value.AddDays(order.DaysToPay),
                    ProviderId = order.ProviderId,
                    BranchId = order.BranchId,
                    LastStatus = TranStatus.InProcess,
                    TransactionType = order.PurchaseTypeId == PType.Credit ? TransactionType.Credit : TransactionType.Cash,
                    UserId = HttpContext.User.Identity.GetUserId(),
                    PurchaseOrderId = order.PurchaseOrderId,
                    Freight = order.Freight,
                    Insurance = order.Insurance,
                    DiscountPercentage = order.Discount,
                    FinalAmount = order.TotalDue,
                    TotalAmount = order.SubTotal.RoundMoney(),
                    TotalTaxAmount = order.TaxAmount,
                    TotalTaxedAmount = (order.SubTotal + order.TaxAmount + order.Freight + order.Insurance).RoundMoney(),
                };

                if (pBill.DiscountPercentage > Cons.Zero)
                    pBill.DiscountedAmount = (pBill.TotalTaxedAmount * (pBill.DiscountPercentage / Cons.OneHundred)).RoundMoney();

                pBill.FinalAmount = (pBill.TotalTaxedAmount - pBill.DiscountedAmount).RoundMoney();

                pBill.PurchaseDetails = (from det in order.PurchaseOrderDetails
                                         where (det.StockedQty > 0d && det.IsCompleated) || (!det.IsCompleated)
                                         select new PurchaseDetail
                                         {
                                             Amount = det.LineTotal,
                                             Quantity = det.IsCompleated ? det.StockedQty : det.OrderQty,
                                             Price = (det.UnitPrice - (det.Discount > Cons.Zero ? (det.UnitPrice * (det.Discount / Cons.OneHundred)) : Cons.Zero)).RoundMoney(),
                                             ProductId = det.ProductId,

                                         }).ToList();

                db.Entry(order).Property(o => o.Bill).IsModified = true;
                db.Purchases.Add(pBill);
                db.SaveChanges();

                return Json(new JResponse
                {
                    Code = Cons.Responses.Codes.Success,
                    Id = pBill.PurchaseId,
                    Body = string.Format("Se agrego la factura {0} a la order de compra", model.BillNumber.ToUpper()),
                    Header = "Factura Generada",
                    Result = Cons.Responses.Success
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Code = Cons.Responses.Codes.ErroSaving,
                    Body = "Detalle del error" + ex.Message,
                    Header = "Error al generar factura",
                    Result = Cons.Responses.Danger
                });
            }

        }

        [HttpPost]
        public ActionResult PrintPurchaseOrder(int id)
        {
            var model = db.PurchaseOrders.Include(o => o.PurchaseOrderDetails.Select(d => d.Product.Equivalences)).Include(o => o.PurchaseOrderHistories).
                                             FirstOrDefault(o => o.PurchaseOrderId == id);

            foreach (var detail in model.PurchaseOrderDetails)
            {
                detail.ProviderCode = detail.Product.Equivalences.FirstOrDefault(e => e.ProviderId == model.ProviderId).Code;
            }

            return PartialView("_PurchaseOrderRequest", model);
        }

        [HttpPost]
        public ActionResult BeginAction(int id, bool? changeRequested)
        {
            var model = db.PurchaseOrders.FirstOrDefault(o => o.PurchaseOrderId == id);

            if (model.PurchaseStatusId == PStatus.Watting || model.PurchaseStatusId == PStatus.Watting)
                ViewBag.ShipMethodes = db.ShipMethodes.ToSelectList();


            if (changeRequested != null)
                model.PurchaseStatusId = PStatus.Expired;


            //mando el comentario en blanco para que sea capturado uno nuevo
            model.Comment = string.Empty;

            return PartialView("_PurchaseOrderRevision", model);
        }

        [HttpPost]
        public ActionResult SetAction(int id, string comment, bool authorized, string to)
        {
            try
            {
                var purchaseOrder = db.PurchaseOrders.Include(o => o.PurchaseOrderDetails.Select(d => d.Product.Equivalences)).Include(o => o.PurchaseOrderHistories).
                                               FirstOrDefault(o => o.PurchaseOrderId == id);

                foreach (var detail in purchaseOrder.PurchaseOrderDetails)
                {
                    detail.ProviderCode = detail.Product.Equivalences.FirstOrDefault(e => e.ProviderId == purchaseOrder.ProviderId).Code;
                }



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
                            jresponse = SendOrder(purchaseOrder, comment, to);
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
                if (authorized && string.IsNullOrEmpty(purchaseOrder.Folio))
                {
                    //año en curso
                    var year = Convert.ToInt32(DateTime.Now.TodayLocal().ToString("yy"));


                    var lasOrder = db.PurchaseOrders.Where(o => !string.IsNullOrEmpty(o.Folio) && o.BranchId == purchaseOrder.BranchId && o.Year == year).
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

                //si ya tiene una fecha de pedido, lo coloco status de recepción
                purchaseOrder.PurchaseStatusId = authorized ? purchaseOrder.OrderDate != null ? PStatus.Watting : PStatus.Authorized : PStatus.NotAuthorized;

                purchaseOrder.Comment = comment;
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

        public JResponse SendOrder(PurchaseOrder model, string comment, string toAddress)
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

                var to = toAddress.Split(',').ToList();

                model.Comment = comment;
                model.UpdDate = DateTime.Now.ToLocal();
                model.UpdUser = HttpContext.User.Identity.Name;


                var body = PartialView("_PurchaseOrderRequest", model).RenderToString();

                //obtengo las variables de configuración
                var var = db.Variables.ToList();

                var cred = var.First(v => v.Name == ConfigVariable.Maling.PurchasingCredentials).Value.Split(Cons.SplitChar);
                var smtpSer = var.First(v => v.Name == ConfigVariable.Maling.SmtpHost).Value.Split(Cons.SplitChar);
                var sender = var.First(v => v.Name == ConfigVariable.Maling.PurchasingSender).Value;
                var logoPath = var.First(v => v.Name == ConfigVariable.Maling.PurchasingLogo).Value;
                var subject = var.First(v => v.Name == ConfigVariable.Maling.PurchasingSubject).Value;
                var cc = var.First(v => v.Name == ConfigVariable.Maling.PurchasingCC).Value;

                //el correo es una mascarilla, lo relleno con el codigo de la sucursal
                var from = string.Format(cred[Cons.Zero], model.Branch.Code.ToLower());

                var mail = new MailSender(from, cred[Cons.One], smtpSer[Cons.Zero], Convert.ToInt32(smtpSer[Cons.One]), sender);
                mail.CC = cc.Split(',').ToList();

                //agrego la imagen embebida
                Dictionary<string, string> att = new Dictionary<string, string>();
                att.Add(ConfigVariable.Maling.PurchasingLogo, Server.MapPath(logoPath));

                try
                {

                    mail.SendMail(to, body, string.Format(subject, model.Folio), att);

                    model.PurchaseStatusId = PStatus.Watting;
                    model.OrderDate = DateTime.Now.ToLocal();
                }
                catch (Exception ex)
                {
                    model.Comment = string.Format("Error al enviar a [{0}] detalle [{1}]", toAddress, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                    model.PurchaseStatusId = PStatus.SendingFailed;
                }


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


        [HttpPost]
        public ActionResult BeginAddDetail(List<ProductViewModel> items, List<int> toRemove, int id)
        {
            //var model = db.PurchaseOrderDetails.Where(o => o.PurchaseOrderId == id).Include(o => o.Product).ToList();

            var order = db.PurchaseOrders.Include(p => p.PurchaseOrderDetails).FirstOrDefault(p => p.PurchaseOrderId == id);

            var model = order.PurchaseOrderDetails.ToList();

            if (toRemove != null)
            {
                foreach (var rId in toRemove)
                {
                    var item = items != null ? items.FirstOrDefault(i => i.ProductId == rId) : null;

                    if (item != null)
                    {
                        var det = model.FirstOrDefault(d => d.ProductId == item.ProductId);
                        det.UnitPrice = item.BuyPrice;
                        det.OrderQty = item.AddQuantity;
                        det.Discount = item.Discount;
                        det.LineTotal = (item.AddQuantity * item.BuyPrice);
                        det.Comment = order.OrderDate != null ? "ChangeRequested" : null; //debo identificar las partidas que se agregan despues de que la orden ha sido envíada por correo, para que 
                                                                                          // que solo esas puedan removerse en caso de un rechazo al autorizar o revisar
                        items.Remove(item);
                    }
                    else
                    {
                        var det = model.FirstOrDefault(d => d.ProductId == rId);
                        model.Remove(det);
                    }
                }
            }

            if (items != null)
            {
                var pIds = items.Select(i => i.ProductId);

                var products = db.Products.Where(p => pIds.Contains(p.ProductId)).ToList();

                foreach (var product in products)
                {
                    var item = items.FirstOrDefault(i => i.ProductId == product.ProductId);

                    var detail = new PurchaseOrderDetail
                    {
                        Product = product,
                        UnitPrice = item.BuyPrice,
                        Discount = item.Discount,
                        OrderQty = item.AddQuantity,
                        ProductId = item.ProductId,
                        PurchaseOrderId = id,
                        LineTotal = (item.AddQuantity * item.BuyPrice),
                        Comment = order.OrderDate != null ? "ChangeRequested" : null //debo identificar las partidas que se agregan despues de que la orden ha sido envíada por correo, para que 
                                                                                         // que solo esas puedan removerse en caso de un rechazo al autorizar o revisar
                    };

                    if (detail.Discount > Cons.Zero)
                        detail.LineTotal = detail.LineTotal - (detail.LineTotal * (detail.Discount / Cons.OneHundred));

                    model.Add(detail);
                }
            }
            order.SubTotal = model.Sum(d => d.LineTotal);
            order.TaxAmount = (order.SubTotal * (order.TaxRate / Cons.OneHundred));
            order.TotalDue = (order.SubTotal + order.TaxAmount + order.Freight + order.Insurance).RoundMoney();

            if (order.Discount > Cons.Zero)
                order.TotalDue = order.TotalDue - (order.TotalDue * (order.Discount / Cons.OneHundred));

            var peIds = model.Select(m => m.ProductId).ToList();

            var eqList = db.Equivalences.Where(e => peIds.Contains(e.ProductId) && e.ProviderId == order.ProviderId).ToList();

            foreach (var detail in model)
            {
                var eq = eqList.FirstOrDefault(e => e.ProductId == detail.ProductId && e.ProviderId == order.ProviderId);
                detail.ProviderCode = eq != null ? eq.Code : "NO ASIGNADO";
            }

            return PartialView("_PurchaseOrderDetails", model);
        }

        [HttpPost]
        public ActionResult RequestChange(List<ProductViewModel> items, List<int> toRemove, string comment, int id)
        {
            try
            {
                var order = db.PurchaseOrders.Include(o => o.PurchaseOrderDetails).Include(o => o.PurchaseOrderHistories).FirstOrDefault(o => o.PurchaseOrderId == id);

                var history = new PurchaseOrderHistory
                {
                    BeginDate = order.UpdDate,
                    Comment = order.Comment,
                    ModifyByUser = order.UpdUser,
                    EndDate = DateTime.Now.ToLocal(),
                    PurchaseOrderId = order.PurchaseOrderId,
                    Status = order.PurchaseStatus.Name,
                    ShipMethod = order.ShipMethod.Name,
                    Type = order.PurchaseType.Name
                };

                order.PurchaseOrderHistories.Add(history);


                if (toRemove != null)
                {
                    foreach (var rId in toRemove)
                    {
                        var item = items != null ? items.FirstOrDefault(i => i.ProductId == rId) : null;

                        if (item != null)
                        {
                            var det = order.PurchaseOrderDetails.FirstOrDefault(d => d.ProductId == item.ProductId);
                            det.UnitPrice = item.BuyPrice;
                            det.OrderQty = item.AddQuantity;
                            det.Discount = item.Discount;
                            det.LineTotal = (item.AddQuantity * item.BuyPrice);
                            det.Comment = order.OrderDate != null ? "ChangeRequested" : null; //debo identificar las partidas que se agregan despues de que la orden ha sido envíada por correo, para que 
                                                                                          // que solo esas puedan removerse en caso de un rechazo al autorizar o revisar

                            //ya no lo agrego solo modifico
                            items.Remove(item);
                        }
                        else
                        {
                            var det = order.PurchaseOrderDetails.FirstOrDefault(d => d.ProductId == rId);
                            order.PurchaseOrderDetails.Remove(det);
                            db.PurchaseOrderDetails.Remove(det);
                        }
                    }
                }

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var detail = new PurchaseOrderDetail
                        {
                            UnitPrice = item.BuyPrice,
                            Discount = item.Discount,
                            OrderQty = item.AddQuantity,
                            ProductId = item.ProductId,
                            PurchaseOrderId = id,
                            LineTotal = (item.AddQuantity * item.BuyPrice),
                            Comment   = order.OrderDate != null ? "ChangeRequested" : null //debo identificar las partidas que se agregan despues de que la orden ha sido envíada por correo, para que 
                                                                                          // que solo esas puedan removerse en caso de un rechazo al autorizar o revisar.
                        };

                        if (detail.Discount > Cons.Zero)
                            detail.LineTotal = detail.LineTotal - (detail.LineTotal * (detail.Discount / Cons.OneHundred));

                        order.PurchaseOrderDetails.Add(detail);
                    }
                }

                order.Comment = comment;
                order.PurchaseStatusId = PStatus.InRevision;
                order.SubTotal = order.PurchaseOrderDetails.Sum(d => d.LineTotal);
                order.TaxAmount = (order.SubTotal * (order.TaxRate / Cons.OneHundred));
                order.TotalDue = (order.SubTotal + order.TaxAmount + order.Freight + order.Insurance).RoundMoney();

                if (order.Discount > Cons.Zero)
                    order.TotalDue = order.TotalDue - (order.TotalDue * (order.Discount / Cons.OneHundred));

                order.UpdDate = DateTime.Now.ToLocal();
                order.UpdUser = User.Identity.Name;

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new JResponse
                {
                    Header = "Cambio solicitado",
                    Body = "Los cambios de la orden de compra se han solicitado con exito",
                    Code = Cons.Responses.Codes.Success,
                    Result = Cons.Responses.Success
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Header = "Error al modificar",
                    Body = "detalle " + ex.Message,
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Danger
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