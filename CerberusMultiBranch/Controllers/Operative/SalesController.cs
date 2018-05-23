using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Operative;
using Microsoft.AspNet.Identity;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity.Owin;
using CerberusMultiBranch.Models.ViewModels.Operative;
using CerberusMultiBranch.Models.Entities.Config;
using System.Net.Http;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Finances;


namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize]
    public partial class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Supervisor,Vendedor")]
        public ActionResult History()
        {
            //obtengo las sucursales configuradas para el empleado
            var branches = User.Identity.GetBranches();

            TransactionViewModel model = new TransactionViewModel();
            model.Branches = branches.ToSelectList();
            model.Sales = LookFor(null, null, null, null, null, null, TranStatus.Revision, null);

            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Supervisor, Cajero")]
        public JsonResult Cancel(int saleId, string comment)
        {
            try
            {
                //busco la venta a la que se le cambiara el status
                var sale = db.Sales.Include(s => s.SaleDetails).Include(s => s.SalePayments).Include(s => s.Client).
                    FirstOrDefault(s => s.SaleId == saleId);

                //si es una venta en proceso borro el registro
                if (sale.Status == TranStatus.InProcess)
                {
                    db.Sales.Remove(sale);
                    db.SaveChanges();

                    return Json(new
                    {
                        Result = "OK",
                        Message = "Venta Cancelada, el producto ha sido regreado al stock, no se requiere devolución"
                    });
                }

                //regreso los productos al stock
                foreach (var detail in sale.SaleDetails)
                {
                    var bp = db.BranchProducts.Find(sale.BranchId, detail.ProductId);

                    //agrego movimiento al inventario
                    StockMovement sm = new StockMovement
                    {
                        BranchId = bp.BranchId,
                        ProductId = bp.ProductId,
                        User = User.Identity.Name,
                        MovementDate = DateTime.Now.ToLocal(),
                        Quantity = detail.Quantity
                    };

                    //se  regresa al inventario todo producto de la venta
                    //en el caso de los paquetes sus detalles regresan a reservado
                    if (detail.ParentId == null)
                    {
                        bp.LastStock = bp.Stock;
                        bp.Stock += detail.Quantity;

                        sm.Comment = "CANCELACION DE VENTA: " + sale.Folio;
                        sm.MovementType = MovementType.Entry;
                    }
                    else
                    {
                        bp.LastStock = bp.Stock + bp.Reserved;
                        bp.Reserved += detail.Quantity;

                        sm.Comment = "CANCELACION DE VENTA: " + sale.Folio;
                        sm.MovementType = MovementType.Reservation;
                    }

                    db.StockMovements.Add(sm);
                    db.Entry(bp).State = EntityState.Modified;
                }

                var payments = sale.SalePayments.Sum(p => p.Amount);

                //si la venta es credito, se resta el monto deudo de la cuenta del cliente
                if (sale.TransactionType == TransactionType.Credito)
                    sale.Client.UsedAmount -= (sale.TotalTaxedAmount - payments).RoundMoney();

                sale.LastStatus = sale.Status;
                sale.Status = TranStatus.Canceled; //status cancelado
                sale.UpdUser = User.Identity.Name;
                sale.UpdDate = DateTime.Now.ToLocal();
                sale.Comment = comment;

                var message = "Venta Cancelada, el producto ha sido regreado al stock";

                //si la venta tiene pagos, coloco el status como precancel para que pase por caja para generar 
                //una devolución
                if (payments > Cons.Zero)
                {
                    sale.Status = TranStatus.PreCancel;
                    message = "Venta Cancelada, podra realizar la devolución de efectivo desde el modulo de caja";
                }


                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Result = "OK", Message = message });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al cancelar la venta", Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor,Vendedor")]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate,
            string folio, string client, string user, TranStatus? status)
        {
            //si el usuario no es un supervisor, solo se le permite ver el dato de sus ventas
            var userId = !User.IsInRole("Supervisor") ? User.Identity.GetUserId() : null;

            var model = LookFor(branchId, beginDate, endDate, folio, client, user, status, userId);

            return PartialView("_SaleList", model);
        }


        private List<Sale> LookFor(int? branchId, DateTime? beginDate, DateTime? endDate, string folio, string client,
            string user, TranStatus? status, string userId)
        {

            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var sales = (from p in db.Sales.Include(p => p.User).Include(p => p.SaleDetails).Include(p => p.SalePayments)
                         where
                            (branchId == null && brancheIds.Contains(p.BranchId) || p.BranchId == branchId)
                         && (beginDate == null || p.TransactionDate >= beginDate)
                         && (endDate == null || p.TransactionDate <= endDate)
                         && (folio == null || folio == string.Empty || p.Folio.Contains(folio))
                         && (client == null || client == string.Empty || p.Client.Name.Contains(client))
                         && (user == null || user == string.Empty || p.User.UserName.Contains(user))
                         && (userId == null || userId == string.Empty || p.UserId == userId)
                         && (status == null || p.Status == status)
                         select p).OrderByDescending(p => p.TransactionDate).ToList();

            return sales;
        }

        [HttpPost]
        public JsonResult CheckCart()
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();

            var sale = db.Sales.Include(s => s.SaleDetails).
                FirstOrDefault(s => s.BranchId == branchId && s.UserId == userId
                && s.Status == TranStatus.InProcess && s.TransactionType == TransactionType.Contado);

            if (sale != null)
            {
                var list = sale.SaleDetails;
                return Json(new { Result = "OK", Message = (list.Sum(td => td.Quantity)).ToString() });
            }

            else
                return Json(new { Result = "OK", Message = Cons.Zero.ToString() });
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult OpenChangePrice(int productId, int saleId, bool isCart)
        {
            var branchId = User.Identity.GetBranchId();

            var transDet = db.SaleDetails.Include(td => td.Product).Include(td => td.Product.BranchProducts).
                FirstOrDefault(td => td.ProductId == productId && td.SaleId == saleId);

            var bp = transDet.Product.BranchProducts.FirstOrDefault(p => p.BranchId == branchId);

            var model = new PriceSelectorViewModel
            {
                PsProductId = productId,
                PsTransactionId = saleId,
                CPrice = transDet.TaxedPrice,
                SPrice = bp != null ? bp.StorePrice : Cons.Zero,
                DPrice = bp != null ? bp.DealerPrice : Cons.Zero,
                WPrice = bp != null ? bp.WholesalerPrice : Cons.Zero,
                IsCart = isCart
            };

            return PartialView("_PriceSelector", model);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult SetClient(int clientId, int transactionId)
        {
            try
            {
                var branchId = User.Identity.GetBranchId();

                var sale = db.Sales.Include(s => s.SaleDetails).
                    Include(s => s.SaleDetails.Select(td => td.Product)).
                    Include(s => s.SaleDetails.Select(td => td.Product.BranchProducts)).
                    FirstOrDefault(s => s.SaleId == transactionId);

                //obtengo el IVA configurado en base de datos
                var iva = Convert.ToDouble(db.Variables.FirstOrDefault(v => v.Name == Cons.VariableIVA).Value);


                if (sale != null)
                {
                    foreach (var detail in sale.SaleDetails)
                    {
                        var brp = detail.Product.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);
                        detail.Product.Quantity = brp != null ? brp.Stock : Cons.Zero;
                        detail.Product.StorePrice = brp != null ? brp.StorePrice : Cons.Zero;
                        detail.Product.DealerPrice = brp != null ? brp.DealerPrice : Cons.Zero;
                        detail.Product.WholesalerPrice = brp != null ? brp.WholesalerPrice : Cons.Zero;
                    }
                }
                else
                {

                    var b = User.Identity.GetBranchSession();

                    sale = new Sale();
                    sale.BranchId = branchId;
                    sale.UpdUser = User.Identity.GetUserName();
                    sale.UpdDate = DateTime.Now.ToLocal();
                    sale.ClientId = clientId;
                    sale.UserId = User.Identity.GetUserId();
                    sale.Folio = User.Identity.GetFolio(Cons.Zero);

                    db.Sales.Add(sale);
                    db.SaveChanges();

                    return Json(new { Result = "OK" });
                }

                var client = db.Clients.Find(clientId);

                sale.ClientId = clientId;

                foreach (var deteail in sale.SaleDetails)
                {
                    //ajusto el precio dependiendo de la configuración del cliente
                    switch (client.Type)
                    {
                        case ClientType.Store:
                            deteail.TaxedPrice = deteail.Product.StorePrice;
                            break;
                        case ClientType.Dealer:
                            deteail.TaxedPrice = deteail.Product.DealerPrice;
                            break;
                        case ClientType.Wholesaler:
                            deteail.TaxedPrice = deteail.Product.WholesalerPrice;
                            break;
                    }

                    deteail.TaxedAmount = deteail.Quantity * deteail.TaxedPrice;

                    //Calculo el precio sin IVA y el monto Total sin IVA
                    deteail.Price = (deteail.TaxedPrice / (Cons.One + (iva / Cons.OneHundred))).RoundMoney();
                    deteail.Amount = (deteail.Price * deteail.Quantity).RoundMoney();

                    //calculo el monto de IVA en el detalle
                    deteail.TaxAmount = ((deteail.TaxedPrice - deteail.Price) * deteail.Quantity).RoundMoney();
                    db.Entry(deteail).State = EntityState.Modified;
                }

                //ajusto los precios de la venta
                if (sale.SaleDetails != null || sale.SaleDetails.Count > Cons.Zero)
                {
                    sale.TotalAmount = sale.SaleDetails.Sum(d => d.Amount).RoundMoney();
                    sale.TotalTaxedAmount = sale.SaleDetails.Sum(d => d.TaxedAmount).RoundMoney();
                    sale.TotalTaxAmount = (sale.TotalTaxedAmount - sale.TotalAmount).RoundMoney();
                    sale.FinalAmount = sale.TotalTaxedAmount;

                }

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error Al asignar el cliente", Data = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult SetNewPrice(int productId, int transactionId, double price, bool isCart)
        {
            try
            {
                //obtengo el IVA configurado en base de datos
                var iva = Convert.ToDouble(db.Variables.FirstOrDefault(v => v.Name == Cons.VariableIVA).Value);

                //obtengo toda la venta, con sus detalles
                var sale = db.Sales.Include(s => s.SaleDetails).Include(s => s.SaleDetails.Select(sd => sd.Product.Images)).
                           FirstOrDefault(s => s.SaleId == transactionId);

                //dentro de la venta, busco  el detalle a modificar
                var det = sale.SaleDetails.FirstOrDefault(sd => sd.ProductId == productId);

                //el precio seteado Incluye IVA
                det.TaxedPrice = price;
                det.TaxedAmount = det.TaxedPrice * det.Quantity;

                //Calculo el precio sin IVA y el monto Total sin IVA
                det.Price = (det.TaxedPrice / (Cons.One + (iva / Cons.OneHundred))).RoundMoney();
                det.Amount = (det.Price * det.Quantity).RoundMoney();

                //calculo el monto de IVA en el detalle
                det.TaxAmount = ((det.TaxedPrice - det.Price) * det.Quantity).RoundMoney();

                //actualizo los totales de la venta
                sale.TotalAmount = sale.SaleDetails.Sum(d => d.Amount).RoundMoney();
                sale.TotalTaxedAmount = sale.SaleDetails.Sum(d => d.TaxedAmount).RoundMoney();
                sale.TotalTaxAmount = (sale.TotalTaxedAmount - sale.TotalAmount).RoundMoney();
                sale.FinalAmount = sale.TotalTaxedAmount;


                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                var model = db.SaleDetails.Include(s => s.Product).
                    Include(s => s.Product.Images).Where(s => s.SaleId == transactionId).ToList();

                if (isCart)
                    return PartialView("_CartDetails", model);
                else
                    return PartialView("_Details", sale.SaleDetails);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al cambiar el costo!", Message = "Ocurrio un error al actualizar el precio detail " + ex.Message });
            }
        }


        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult CompleateSale(int transactionId, int sending)
        {
            return Compleate(transactionId, sending, true);
        }


        private SaleViewModel GetVM(int id)
        {
            if (db == null)
                db = new ApplicationDbContext();


            var sale = db.Sales.Include(s => s.SaleDetails).Include(s => s.Client).
                                     Include(s => s.SaleDetails.Select(td => td.Product)).
                                     Include(s => s.SaleDetails.Select(td => td.Product.Images)).
                                     Include(s => s.SaleDetails.Select(td => td.Product.BranchProducts)).
                                     FirstOrDefault(s => s.SaleId == id);

            SaleViewModel model = new SaleViewModel(sale);
            model.Categories = db.Categories.ToSelectList();
            model.CarMakes = db.CarMakes.ToSelectList();

            return model;
        }

        #region Manual Sale

        //this method is call when a new purchase is going to be registered
        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult BeginSale()
        {
            BeginSaleViewModel model = new BeginSaleViewModel();

            return PartialView("_BeginSale", model);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult Create(BeginSaleViewModel model)
        {
            try
            {
                var sale = new Sale
                {
                    BranchId = User.Identity.GetBranchId(),
                    UserId = User.Identity.GetUserId(),
                    TransactionDate = model.SaleDate,
                    ComPer = User.Identity.GetSalePercentage(),
                    Expiration = model.SaleDate.AddDays(model.Days),
                    TransactionType = model.TransactionType,
                    ClientId = model.ClientId,
                    Status = TranStatus.InProcess,
                    UpdUser = User.Identity.Name,
                    UpdDate = DateTime.Now.ToLocal(),
                    Folio = User.Identity.GetFolio(Cons.Zero)
                };

                db.Sales.Add(sale);
                db.SaveChanges();

                return Json(new { Result = "OK", Id = sale.SaleId });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al registrar la venta", Message = "Ocurrio un error al crear la venta detalle " + ex.Message });
            }
        }


        [Authorize(Roles = "Supervisor,Vendedor")]
        public ActionResult Detail(int id)
        {
            var branchIds = User.Identity.GetBranches().Select(b => b.BranchId);

            ViewBag.Systems = db.Systems.ToSelectList();

            var model = db.Sales.Include(p => p.SaleDetails).
                Include(p => p.SalePayments).Include(p => p.User).
                Include(s => s.SaleDetails.Select(td => td.Product.Category)).
                Include(s => s.SaleDetails.Select(td => td.Product.Images)).
                FirstOrDefault(p => p.SaleId == id && branchIds.Contains(p.BranchId));

            if (model == null)
                return RedirectToAction("History");

            return View(model);
        }

        [HttpPost]
        public ActionResult SearchProducts(string filter, int? systemId, TransactionType type)
        {
            string[] arr = new List<string>().ToArray();
            string code = string.Empty;

            var branchId = User.Identity.GetBranchId();

            if (filter != null && filter != string.Empty)
            {
                arr = filter.Trim().Split(' ');

                if (arr.Length == Cons.One)
                    code = arr.FirstOrDefault();
            }

            List<Product> products = new List<Product>();

            products = (from p in db.Products.Include(p => p.Images).Include(p => p.BranchProducts)
                        where (p.Code == code) && (p.ProductType == ProductType.Single)
                        select p).Take((int)Cons.OneHundred).ToList();

            if (products.Count == Cons.Zero)
            {
                products = (from ep in db.Products.Include(p => p.Images).Include(p => p.BranchProducts)
                            where (filter == null || filter == string.Empty || arr.All(s => (ep.Code + " " + ep.Name + " " + ep.TradeMark).Contains(s)))
                            && (systemId == null || ep.PartSystemId == systemId)
                            && (ep.IsActive)
                            select ep).OrderByDescending(s => s.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId) != null ?
                            s.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId).Stock : Cons.Zero).Take((int)Cons.OneHundred).ToList();
            }

           

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
                prod.CanSell = (prod.IsActive && ((type == TransactionType.Preventa) || (prod.Quantity > Cons.Zero)));
            }


            return PartialView("_ProductResult", products);
        }

        [HttpPost]
        public ActionResult BeginAdd(int productId)
        {
            var branchId = User.Identity.GetBranchId();


            var branchProd = db.BranchProducts.Include(bp => bp.Product).Include(bp => bp.Product.Images).
                FirstOrDefault(bp => bp.ProductId == productId && bp.BranchId == branchId);


            branchProd.Product.StorePrice = branchProd.StorePrice;
            branchProd.Product.DealerPrice = branchProd.DealerPrice;
            branchProd.Product.WholesalerPrice = branchProd.WholesalerPrice;
            branchProd.Product.Quantity = branchProd.Stock;

            return PartialView("_AddProduct", branchProd.Product);
        }

        [HttpPost]
        public ActionResult CompleateAdd(int productId, int saleId, double quantity)
        {
            var userId   = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();

            //obtengo el IVA configurado en base de datos
            var iva = Convert.ToDouble(db.Variables.FirstOrDefault(v => v.Name == Cons.VariableIVA).Value);

            var bp = db.BranchProducts.Include(brp => brp.Product).
                        FirstOrDefault(brp => brp.BranchId == branchId && brp.ProductId == productId);

            var existance = bp != null ? bp.Stock : Cons.Zero;

            //consulto si el usuario tiene una venta activa
            var sale = db.Sales.Include(s => s.SaleDetails).Include(s => s.Client).
                Include(s => s.SaleDetails.Select(sd => sd.Product.Images)).FirstOrDefault(s => s.SaleId == saleId);

            var taxedPrice = 0.0;

            //checo si el producto ya esta en la venta
            var detail = sale.SaleDetails.
                FirstOrDefault(td => td.ProductId == productId && td.SaleId == sale.SaleId);

            //si lo esta, sumo la cantidad
            if (detail != null)
            {
                //utilizo el precio seteado, ya que pudo haber sido modificado manualmente
                detail.Quantity += quantity;

                detail.Amount = (detail.Price * detail.Quantity).RoundMoney();
                detail.TaxedAmount = (detail.TaxedPrice * detail.Quantity).RoundMoney();
                detail.TaxAmount = (detail.TaxedAmount - detail.Amount).RoundMoney();

                //si no es una preventa
                //verifico el stock y valido si es posible agregar mas producto a la venta
                if (sale.TransactionType != TransactionType.Preventa && existance < quantity)
                {
                    var j = new
                    {
                        Result  = "Cantidad insuficiente",
                        Message = "Estas intentando vender mas productos de los disponibles, consulta existencias, " +
                        "para vender producto sin existencia, es necesario registrar una preventa desde el modulo de ventas"
                    };

                    return Json(j);
                }

                db.Entry(detail).State = EntityState.Modified;
            }
            else
            {
                //si no es una preventa
                //verifico el stock y valido si es posible agregar mas producto a la venta
                if (sale.TransactionType != TransactionType.Preventa && existance < quantity)
                {
                    var j = new
                    {
                        Result = "Cantidad insuficiente",
                        Message = "Estas intentando vender mas productos de los disponibles, consulta existencias, para vender producto sin existencia" +
                        " es necesario registrar una preventa desde el modulo de ventas"
                    };

                    return Json(j);
                }

                //obtengo el precio por el tipo de cliente (Precio con IVA)
                switch (sale.Client.Type)
                {
                    case ClientType.Store:
                        taxedPrice = bp.StorePrice;
                        break;
                    case ClientType.Dealer:
                        taxedPrice = bp.DealerPrice;
                        break;
                    case ClientType.Wholesaler:
                        taxedPrice = bp.WholesalerPrice;
                        break;
                }

                //Calculo el precio sin IVA y el monto del IVA
                var price = (taxedPrice / (Cons.One + (iva / Cons.OneHundred))).RoundMoney();
                var taxAmount = (taxedPrice - price).RoundMoney();


                detail = new SaleDetail
                {
                    ProductId = productId,
                    SaleId = sale.SaleId,
                    Quantity = quantity,
                    Price = price, //Precio Sin IVA
                    TaxedPrice = taxedPrice, //Precio Con IVA
                    Amount = (price * quantity).RoundMoney(), //El total sin IVA
                    TaxedAmount = taxedPrice * quantity, //El total con IVA
                    TaxPercentage = iva
                };
                detail.TaxAmount = (detail.TaxedAmount - detail.Amount).RoundMoney(); //El monto total de IVA

                sale.SaleDetails.Add(detail);
            }

            sale.TotalAmount = sale.SaleDetails.Sum(d => d.Amount).RoundMoney();
            sale.TotalTaxedAmount = sale.SaleDetails.Sum(d => d.TaxedAmount).RoundMoney();
            sale.TotalTaxAmount = (sale.TotalTaxedAmount - sale.TotalAmount).RoundMoney();
            sale.FinalAmount = sale.TotalTaxedAmount;


            if (sale.TransactionType == TransactionType.Credito)
            {
                if (sale.TotalTaxedAmount > (sale.Client.CreditLimit - sale.Client.UsedAmount))
                    return Json(new
                    {
                        Result = "Error",
                        Header = "Limite de crédito excedido",
                        Message = "Estas intentando vender una cantidad mayor al crédito disponible del cliente! Crédito restante: " +
                        (sale.Client.CreditLimit - sale.Client.UsedAmount).ToMoney()
                    });
            }

            try
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Header = "Error desconocido", Message = ex.Message });
            }

            var model = sale.SaleDetails;//db.SaleDetails.Where(sd => sd.SaleId == saleId).Include(sd => sd.Product.Images).ToList();
            return PartialView("_Details", model);
        }

        [HttpPost]
        public ActionResult RemoveDetail(int saleId, int productId)
        {
            //obtnego la venta con imagenes
            var sale = db.Sales.Include(s => s.SaleDetails).
                Include(s => s.SaleDetails.Select(sd => sd.Product.Images)).FirstOrDefault(s => s.SaleId == saleId);

            var detail = sale.SaleDetails.FirstOrDefault(d => d.ProductId == productId);

            if (detail != null)
            {
                sale.SaleDetails.Remove(detail);

                sale.TotalAmount = sale.SaleDetails.Sum(d => d.Amount).RoundMoney();
                sale.TotalTaxedAmount = sale.SaleDetails.Sum(d => d.TaxedAmount).RoundMoney();
                sale.TotalTaxAmount = (sale.TotalTaxedAmount - sale.TotalAmount).RoundMoney();
                sale.FinalAmount = sale.TotalTaxedAmount;

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
            }

            var model = db.SaleDetails.Include(d => d.Product.Images).
              Where(d => d.SaleId == saleId).ToList();

            return PartialView("_Details", model);
        }

        [HttpPost]
        public ActionResult CompleateManual(int saleId)
        {
            return Compleate(saleId, Cons.Zero, true);
        }


        private JsonResult Compleate(int saleId, int sending, bool addFolio)
        {
            try
            {
                var sale = db.Sales.Include(s => s.SaleDetails).
                           FirstOrDefault(s => s.SaleId == saleId);

                sale.Year = Convert.ToInt32(DateTime.Now.TodayLocal().ToString("yy"));

                if (addFolio)
                {
                    var lastSale = db.Sales.Where(s => s.Status != TranStatus.InProcess &&
                                                  s.BranchId == sale.BranchId && s.Year == sale.Year).
                                                  OrderByDescending(s => s.Sequential).FirstOrDefault();

                    var seq = lastSale != null ? lastSale.Sequential : Cons.Zero;
                    sale.Sequential = (seq + Cons.One);

                    sale.Folio = User.Identity.GetFolio(sale.Sequential);
                }


                int sortOrder = Cons.One;

                foreach (var detail in sale.SaleDetails)
                {
                    //busco stock en sucursal incluyo la categoría de producto para el calculo de comision
                    var pb = db.BranchProducts.Include(brp => brp.Product).
                        Include(brp => brp.Product.Category).
                        Include(brp => brp.Product.PackageDetails).
                        FirstOrDefault(brp => brp.BranchId == sale.BranchId && brp.ProductId == detail.ProductId);

                    // si no hay producto suficiente la operación concluye
                    if (sale.TransactionType != TransactionType.Preventa && (pb == null || pb.Stock < detail.Quantity))
                        throw new Exception("El producto con código " + pb.Product.Code +
                            " no tiene existencia suficiente en la sucursal, para realizar la venta de este articulo debe generar una preventa");

                    //guardo dato del detalle incluyendo la comision de la venta del determinado producto

                    detail.SortOrder = sortOrder;

                    if (pb.Product.Category.Commission > Cons.Zero)
                        detail.Commission = Math.Round(detail.TaxedAmount * (pb.Product.Category.Commission / Cons.OneHundred), Cons.Two);

                    db.Entry(detail).State = EntityState.Modified;

                    //actualizo stock de sucursal
                    pb.LastStock = pb.Stock;
                    pb.Stock -= detail.Quantity;

                    db.Entry(pb).State = EntityState.Modified;

                    //si el producto que se esta vendiendo es un paquete
                    //agrego todos los productos q lo complementan a la venta con precio 0
                    if (pb.Product.ProductType == ProductType.Package)
                    {
                        foreach (var pckDet in pb.Product.PackageDetails)
                        {
                            sortOrder++;
                            var tDeatil = new SaleDetail
                            {
                                SaleId = detail.SaleId,
                                Quantity = pckDet.Quantity,
                                Price = Cons.Zero,
                                Commission = Cons.Zero,
                                ProductId = pckDet.DetailtId,
                                SortOrder = sortOrder,
                                ParentId = pckDet.PackageId,
                            };
                            //busco el stock del detalle en sucursal y resto el producto de los reservados
                            var detBP = db.BranchProducts.Find(pckDet.DetailtId);
                            detBP.LastStock = (detBP.Stock + detBP.Reserved);
                            detBP.Reserved -= pckDet.Quantity;

                            db.SaleDetails.Add(tDeatil);

                            db.Entry(detBP).State = EntityState.Modified;
                        }
                    }

                    //agrego el moviento al inventario
                    StockMovement sm = new StockMovement
                    {
                        BranchId = pb.BranchId,
                        ProductId = pb.ProductId,
                        MovementType = MovementType.Exit,
                        Comment = "SALIDA AUTOMATICA, VENTA FOLIO:" + sale.Folio,
                        User = User.Identity.Name,
                        MovementDate = DateTime.Now.ToLocal(),
                        Quantity = detail.Quantity
                    };

                    db.StockMovements.Add(sm);

                    sortOrder++;
                }

                //agrego el folio
                sale.Folio = sale.Folio;

                //verifico que la venta no exceda el crédito del cliente, siempre y cuando no sea contado
                if (sale.TransactionType == TransactionType.Credito)
                {
                    if ((sale.TotalAmount + sale.Client.UsedAmount) > sale.Client.CreditLimit)
                    {
                        return Json(new
                        {
                            Result = "Error",
                            Header = "Limite de crédito excedito",
                            Message = "El total de la venta excede el disponible de credito"
                        });
                    }
                    else
                    {
                        sale.Client.UsedAmount += sale.TotalTaxedAmount.RoundMoney();
                        db.Entry(sale.Client).Property(c => c.UsedAmount).IsModified = true;
                    }
                }

                //coloco el porcentaje de comision del empleado
                sale.ComPer = User.Identity.GetSalePercentage();

                //si tiene comision por venta, coloco la cantidad de esta
                if (sale.ComPer > Cons.Zero)
                {
                    var comTot = sale.SaleDetails.Sum(td => td.Commission);
                    if (comTot > Cons.Zero)
                        sale.ComAmount = Math.Round(comTot * (sale.ComPer / Cons.OneHundred), Cons.Two);
                }

                //ajuto la hora y fecha de venta a la actual y concluyo
                if (sale.TransactionType == TransactionType.Contado)
                {
                    sale.TransactionDate = DateTime.Now.ToLocal();
                    sale.Expiration = DateTime.Now.ToLocal();
                    sale.SendingType = sending;
                }
                else if (sale.TransactionDate == DateTime.Now.TodayLocal())
                    sale.TransactionDate = DateTime.Now.ToLocal();

                sale.LastStatus = sale.Status;
                sale.Status = TranStatus.Reserved;

                sale.UpdUser = User.Identity.Name;
                sale.UpdDate = DateTime.Now.ToLocal();


                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();


                return Json(new { Result = "OK", Message = "Se ha generado la venta con folio:" + sale.Folio });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Ocurrio un error",
                    Message = "No se pudo concretar la venta debido a un error inesperado " + ex.Message
                });
            }
        }

        #endregion



        #region Discontinued Methods

        [HttpPost]
        public ActionResult AddPayment(SalePayment payment)
        {
            try
            {
                payment.UpdDate = DateTime.Now.ToLocal();
                payment.UpdUser = User.Identity.Name;

                var sale = db.Sales.Include(p => p.SalePayments).
                    FirstOrDefault(p => p.SaleId == payment.SaleId);

                var total = sale.SalePayments.Sum(p => p.Amount);

                total += payment.Amount;
                if (sale.TotalAmount >= total)
                {
                    db.SalePayments.Add(payment);

                    if (total == sale.TotalAmount)
                    {
                        sale.Status = TranStatus.Compleated;
                        sale.UpdDate = DateTime.Now.ToLocal();
                        sale.UpdUser = User.Identity.Name;

                        db.Entry(sale).State = EntityState.Modified;
                    }

                    sale.Client.UsedAmount -= total;
                    db.SaveChanges();
                    sale.SalePayments.OrderByDescending(p => p.PaymentDate);


                    return PartialView("_SalePayments", sale.SalePayments);
                }
                else
                {
                    return Json(new
                    {
                        Result = "warning",
                        Header = "Error al registrar el pago!!",
                        Message = "La cantidad que intenta registrar supera lo permitido para esta cuenta"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error",
                    Header = "Error al registrar el pago!!",
                    Message = "Ocurrio un error inesperado detalle " + ex.Message
                });
            }
        }



        [HttpPost]
        public ActionResult RemovePayment(int id)
        {
            try
            {
                var payment = db.SalePayments.Find(id);

                var saleId = payment.SaleId;

                if (payment.Sale.Status == TranStatus.Compleated)
                {
                    payment.Sale.Status = TranStatus.Reserved;
                    payment.Sale.UpdDate = DateTime.Now.ToLocal();
                    payment.Sale.UpdUser = User.Identity.Name;

                    db.Entry(payment.Sale).State = EntityState.Modified;
                }

                var client = db.Clients.Find(payment.Sale.ClientId);
                client.UsedAmount += payment.Amount;

                db.Entry(client).State = EntityState.Modified;
                db.SalePayments.Remove(payment);
                db.SaveChanges();

                var model = db.SalePayments.Where(p => p.SaleId == saleId).ToList();
                return PartialView("_SalePayments", model);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error",
                    Header = "Error al remover el pago!!",
                    Message = "Ocurrio un error inesperado al eliminar el pago... detalle: " + ex.Message
                });
            }
        }
        #endregion

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