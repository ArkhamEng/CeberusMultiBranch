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
            model.Sales = new List<Sale>(); //LookFor(null, DateTime.Today, null, null, null, null, null, null);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor,Cajero")]
        public JsonResult Cancel(int transactionId, string comment)
        {
            return ChangeStatus(transactionId, comment, TranStatus.Canceled);
        }


        private JsonResult ChangeStatus(int transactionId, string comment, TranStatus status)
        {
            try
            {
                //busco la venta a la que se le cambiara el status
                var sale = db.Sales.Include(s => s.SaleDetails).Include(s => s.SalePayments).
                    FirstOrDefault(s => s.SaleId == transactionId);

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

                        sm.Comment = "Cancelación de venta folio:" + sale.Folio;
                        sm.MovementType = MovementType.Entry;
                    }
                    else
                    {
                        bp.LastStock = bp.Stock + bp.Reserved;
                        bp.Reserved += detail.Quantity;

                        sm.Comment = "Cancelación de venta folio:" + sale.Folio;
                        sm.MovementType = MovementType.Reservation;
                    }

                    db.StockMovements.Add(sm);
                    db.Entry(bp).State = EntityState.Modified;
                }

                //desactivo la venta y registo usuario, comentario y fecha de cancelación
                sale.LastStatus = sale.Status;
                sale.Status = status;
                sale.UpdUser = User.Identity.Name;
                sale.UpdDate = DateTime.Now.ToLocal();
                sale.Comment = comment;

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Result = "OK", Message = "Venta Cancelada, el producto ha sido regreado al stock" });
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
            if (!User.IsInRole("Supervisor"))
                user = User.Identity.GetUserName();

            var model = LookFor(branchId, beginDate, endDate, folio, client, user, status, null);

            return PartialView("_SaleList", model);
        }


        private List<Sale> LookFor(int? branchId, DateTime? beginDate, DateTime? endDate, string folio, string client,
            string user, TranStatus? status, string userId)
        {

            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var sales = (from p in db.Sales.Include(p => p.User).Include(p => p.SaleDetails)
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
                CPrice = transDet.Price,
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
                    //Include(s => s.TransactionDetails.Select(td => td.Product.Category)).
                    Include(s => s.SaleDetails.Select(td => td.Product.BranchProducts)).
                    FirstOrDefault(s => s.SaleId == transactionId);

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
                    sale = new Sale();
                    sale.BranchId = branchId;
                    sale.UpdUser = User.Identity.GetUserName();
                    sale.UpdDate = DateTime.Now.ToLocal();
                    sale.ClientId = clientId;
                    sale.UserId = User.Identity.GetUserId();
                    sale.Folio = Cons.CodeMask;

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
                            deteail.Price = deteail.Product.StorePrice;
                            break;
                        case ClientType.Dealer:
                            deteail.Price = deteail.Product.DealerPrice;
                            break;
                        case ClientType.Wholesaler:
                            deteail.Price = deteail.Product.WholesalerPrice;
                            break;
                    }
                    deteail.Amount = deteail.Quantity * deteail.Price;

                    db.Entry(deteail).State = EntityState.Modified;
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
                var det = db.SaleDetails.Include(td => td.Product).Include(td => td.Product.Images)
             .FirstOrDefault(td => td.SaleId == transactionId && td.ProductId == productId);

                det.Price = price;
                var newAmount = det.Price * det.Quantity;

                //si la nueva cantidad es menor a la anterior
                //resto la diferencia del total de la venta
                if (det.Amount > newAmount)
                {
                    var f = det.Amount - newAmount;
                    det.Sale.TotalAmount -= f;
                }

                //si la nueva cantidad es mayor a la anterior
                //sumo la diferencia del total de la venta
                else
                {
                    var f = newAmount - det.Amount;
                    det.Sale.TotalAmount += f;
                }

                det.Amount = newAmount;

                db.Entry(det).State = EntityState.Modified;
                db.Entry(det.Sale).State = EntityState.Modified;
                db.SaveChanges();

                var model = db.SaleDetails.Include(s => s.Product).
                    Include(s => s.Product.Images).Where(s => s.SaleId == transactionId).ToList();

                if (isCart)
                    return PartialView("_CartDetails", model);
                else
                    return PartialView("_Details", model);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al cambiar el costo!", Message = "Ocurrio un error al actualizar el precio detail " + ex.Message });
            }
        }

        [Authorize(Roles = "Vendedor")]
        [HttpPost]
        public ActionResult OpenCart()
        {
            SaleViewModel model;
            var branchId = User.Identity.GetBranchId();
            string userId = User.Identity.GetUserId();

            //busco una venta de contrado en proceso
            var sale = db.Sales.Include(s => s.SaleDetails).Include(s => s.Client).
                Include(s => s.SaleDetails.Select(td => td.Product)).
                Include(s => s.SaleDetails.Select(td => td.Product.Images)).
                Include(s => s.SaleDetails.Select(td => td.Product.BranchProducts)).

                FirstOrDefault(s => (s.UserId == userId)
                               && (s.Status == TranStatus.InProcess && s.TransactionType == TransactionType.Contado)
                               && s.BranchId == branchId);

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
                model = new SaleViewModel(sale);
            }

            else
            {
                model = new SaleViewModel();
                model.UpdDate = DateTime.Now.ToLocal();
                model.UserId = userId;
                model.BranchId = branchId;
                model.SaleDetails = new List<SaleDetail>();
            }

            model.UpdUser = User.Identity.Name;
            return PartialView("_Cart", model);
        }

        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public ActionResult RemoveFromCart(int transactionId, int productId)
        {
            var detail = db.SaleDetails.Find(transactionId, productId);

            if (detail != null)
            {
                try
                {
                    db.SaleDetails.Remove(detail);
                    db.SaveChanges();

                    var model = db.SaleDetails.Include(t => t.Product).
                        Include(t => t.Product.Images).
                        Where(t => t.SaleId == transactionId).ToList();

                    var sale = db.Sales.Find(transactionId);

                    if (sale != null)
                    {
                        sale.TotalAmount = model.Count > Cons.Zero ? model.Sum(td => td.Amount) : Cons.Zero;
                        sale.UpdDate = DateTime.Now.ToLocal();
                        sale.UpdUser = User.Identity.GetUserName();

                        db.Entry(sale).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return PartialView("_CartDetails", model);
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Ocurrio un error al eliminar el registro", Message = ex.Message });
                }
            }
            else
            {
                return Json(new { Result = "Dato no encontrado", Message = "No se encontro el registo a eliminar, intenta recargar la pagina usando F5" });
            }
        }


        [HttpPost]
        [Authorize(Roles = "Vendedor")]
        public JsonResult AddToCart(int productId, double quantity)
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();


            var bp = db.BranchProducts.Include(brp => brp.Product).
                        FirstOrDefault(brp => brp.BranchId == branchId && brp.ProductId == productId);

            var existance = bp != null ? bp.Stock : Cons.Zero;

            if (existance < quantity)
            {
                var j = new
                {
                    Result = "Sin producto en almacen",
                    Message = "Estas intentando vender mas productos de los disponibles, revisa existencia en sucursal"
                };
                return Json(j);
            }

            if (bp.StorePrice <= Cons.Zero)
            {
                var j = new
                {
                    Result = "Error en precio",
                    Message = "El precio de mostrador debe ser mayor a $0, revisa la configuración"
                };
                return Json(j);
            }
            else if (bp.DealerPrice <= Cons.Zero)
            {
                var j = new
                {
                    Result = "Error en precio",
                    Message = "El precio de distribuidor debe ser mayor a $0, revisa la configuración"
                };
                return Json(j);
            }
            else if (bp.WholesalerPrice <= Cons.Zero)
            {
                var j = new
                {
                    Result = "Error en precio",
                    Message = "El precio de mayorista debe ser mayor a $0, revisa la configuración"
                };
                return Json(j);
            }


            //consulto si el usuario tiene una venta de contado activa
            var sale = db.Sales.Include(s => s.Client).FirstOrDefault(s => s.UserId == userId &&
                         s.BranchId == branchId && s.Status == TranStatus.InProcess && s.TransactionType == TransactionType.Contado);


            //Si no hay un registro de venta activo, creo uno nuevo con cliente por defecto
            if (sale == null)
            {
                sale = new Sale
                {
                    UserId = userId,
                    BranchId = branchId,
                    TransactionDate = DateTime.Now.ToLocal(),
                    UpdDate = DateTime.Now.ToLocal(),
                    UpdUser = User.Identity.Name,
                    Folio = Cons.CodeMask,
                    LastStatus = TranStatus.InProcess,
                    Status = TranStatus.InProcess,
                    TransactionType = TransactionType.Contado
                };

                db.Sales.Add(sale);
                db.SaveChanges();
            }


            var amount = 0.0;
            var price = 0.0;

            //se asigna el precio en base a la configuración del cliente
            //si no hay un cliente asignado se asigna precio mostrador
            if (sale.Client == null || sale.Client.Type == ClientType.Store)
            {
                amount = (quantity * bp.StorePrice);
                price = bp.StorePrice;
            }
            else
            {
                switch (sale.Client.Type)
                {
                    case ClientType.Dealer:
                        amount = (quantity * bp.DealerPrice);
                        price = bp.DealerPrice;
                        break;
                    case ClientType.Wholesaler:
                        amount = (quantity * bp.WholesalerPrice);
                        price = bp.WholesalerPrice;
                        break;
                }
            }

            //checo si el producto ya esta en la venta
            var detail = db.SaleDetails.
                FirstOrDefault(td => td.ProductId == productId && td.SaleId == sale.SaleId);

            //si lo esta, sumo la cantidad
            if (detail != null)
            {
                //utilizo el precio seteado (ya que pudo haber sido modificado manualmente)
                amount = detail.Price * quantity;

                detail.Amount += amount;
                detail.Quantity += quantity;

                //verifico el stock y valido si es posible agregar mas producto a la venta
                if (existance < detail.Quantity)
                {
                    var j = new
                    {
                        Result = "Cantidad insuficiente",
                        Message = "Estas intentando vender mas productos de los disponibles, revisa el carrito de venta"
                    };

                    return Json(j);
                }

                db.Entry(detail).State = EntityState.Modified;
            }
            else
            {
                detail = new SaleDetail
                {
                    ProductId = productId,
                    SaleId = sale.SaleId,
                    Price = price,
                    Quantity = quantity,
                    Amount = amount
                };

                db.SaleDetails.Add(detail);
            }
            sale.TotalAmount += amount;

            try
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }

            var js = new { Result = "OK" };
            return Json(js);
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
                    Folio = Cons.CodeMask
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

            return View(model);
        }

        [HttpPost]
        public ActionResult SearchProducts(string filter, int? systemId)
        {
            string[] arr = new List<string>().ToArray();
            string code = string.Empty;

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
                            select ep).Take((int)Cons.OneHundred).ToList();
            }

            var branchId = User.Identity.GetBranchId();

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
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();

            var bp = db.BranchProducts.Include(brp => brp.Product).
                        FirstOrDefault(brp => brp.BranchId == branchId && brp.ProductId == productId);

            var existance = bp != null ? bp.Stock : Cons.Zero;

            //consulto si el usuario tiene una venta activa
            var sale = db.Sales.Include(s=> s.SaleDetails).Include(s => s.Client).FirstOrDefault(s => s.SaleId == saleId);


            var price = 0.0;
            var amount = 0.0;
            var totQuantity = quantity;

            //checo si el producto ya esta en la venta
            var detail = sale.SaleDetails.
                FirstOrDefault(td => td.ProductId == productId && td.SaleId == sale.SaleId);

            //si lo esta, sumo la cantidad
            if (detail != null)
            {
                //sumo la cantidad a la cantidad existente
                totQuantity += detail.Quantity;

                //utilizo el precio seteado (ya que pudo haber sido modificado manualmente)
                price = detail.Price;

                detail.Quantity += quantity;
                detail.Amount = (price * detail.Quantity);


                //verifico el stock y valido si es posible agregar mas producto a la venta
                if (existance < totQuantity)
                {
                    var j = new
                    {
                        Result = "Cantidad insuficiente",
                        Message = "Estas intentando vender mas productos de los disponibles, consulta existencias"
                    };

                    return Json(j);
                }

                 db.Entry(detail).State = EntityState.Modified;
            }
            else
            {
                //verifico el stock y valido si es posible agregar mas producto a la venta
                if (existance < totQuantity)
                {
                    var j = new
                    {
                        Result = "Cantidad insuficiente",
                        Message = "Estas intentando vender mas productos de los disponibles, consulta existencias"
                    };

                    return Json(j);
                }

                //obtengo el precio por el tipo de cliente
                switch (sale.Client.Type)
                {
                    case ClientType.Store:
                        price = bp.StorePrice;
                        break;
                    case ClientType.Dealer:
                        price = bp.DealerPrice;
                        break;
                    case ClientType.Wholesaler:
                        price = bp.WholesalerPrice;
                        break;
                }

                detail = new SaleDetail
                {
                    ProductId = productId,
                    SaleId = sale.SaleId,
                    Price = price,
                    Quantity = quantity,
                    Amount = (price * quantity)
                };

                sale.SaleDetails.Add(detail);
            }

            amount = sale.SaleDetails.Sum(s => s.Amount);

            if (sale.TransactionType == TransactionType.Credito)
            {
              
                if (amount > (sale.Client.CreditLimit - sale.Client.UsedAmount))
                    return Json(new
                    {
                        Result = "Error",
                        Header = "Limite de crédito excedido",
                        Message = "Estas intentando vender una cantidad mayor al crédito disponible, Disponible del cliente " +
                        (sale.Client.CreditLimit - sale.Client.UsedAmount).ToString("c")
                    });
            }

            sale.TotalAmount = amount;

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
            var sale = db.Sales.Include(s => s.SaleDetails).FirstOrDefault(s => s.SaleId == saleId);

            var detail = sale.SaleDetails.FirstOrDefault(d => d.ProductId == productId);

            if (detail != null)
            {
                sale.SaleDetails.Remove(detail);

                sale.TotalAmount = sale.SaleDetails.Sum(sd => sd.Amount);
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

        #region Private Methods

        private JsonResult Compleate(int saleId, int sending, bool addFolio)
        {
            try
            {
                var sale = db.Sales.Include(s => s.SaleDetails).
                           FirstOrDefault(s => s.SaleId == saleId);

                var branchId = User.Identity.GetBranchId();

                var folio = sale.Folio;

                if (addFolio)
                {
                    var lastSale = db.Sales.
                    Where(s => s.Status != TranStatus.InProcess && s.BranchId == branchId).
                    OrderByDescending(s => s.TransactionDate).FirstOrDefault();

                    var lastFolio = lastSale != null ? Convert.ToInt32(lastSale.Folio) : Cons.Zero;

                    folio = (lastFolio + Cons.One).ToString(Cons.CodeMask);
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
                    if (pb == null || pb.Stock < detail.Quantity)
                        throw new Exception("Un producto execede la cantidad en inventario");

                    //guardo dato del detalle incluyendo la comision de la venta del determinado producto
                    detail.Amount = detail.Price * detail.Quantity;
                    detail.Quantity = detail.Quantity;
                    detail.SortOrder = sortOrder;

                    if (pb.Product.Category.Commission > Cons.Zero)
                        detail.Commission = Math.Round(detail.Amount * (pb.Product.Category.Commission / Cons.OneHundred), Cons.Two);

                    db.Entry(detail).State = EntityState.Modified;

                    //actualizo stock de sucursal
                    pb.LastStock = pb.Stock;
                    pb.Stock -= detail.Quantity;

                    db.Entry(pb).State = EntityState.Modified;

                    //si el producto que se esta agregando vendiendo es un paquete
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

                            //agrego el moviento al inventario
                            StockMovement detSm = new StockMovement
                            {
                                BranchId = branchId,
                                ProductId = pckDet.DetailtId,
                                MovementType = MovementType.Exit,
                                Comment = "Salida en Paquete venta folio:" + folio,
                                User = User.Identity.Name,
                                MovementDate = DateTime.Now.ToLocal(),
                                Quantity = detail.Quantity
                            };

                            db.StockMovements.Add(detSm);
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
                        Comment = "Venta folio:" + folio,
                        User = User.Identity.Name,
                        MovementDate = DateTime.Now.ToLocal(),
                        Quantity = detail.Quantity
                    };

                    db.StockMovements.Add(sm);

                    sortOrder++;
                }

                //ajusto el monto total y agrego el folio
                sale.TotalAmount = sale.SaleDetails.Sum(td => td.Amount);
                sale.Folio = folio;

                //verifico que la venta no exceda el crédito del cliente
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
                        sale.Client.UsedAmount += sale.TotalAmount;
                        db.Entry(sale.Client).State = EntityState.Modified;

                        db.Entry(sale.Client).Property(c => c.CreditDays).IsModified = false;
                        db.Entry(sale.Client).Property(c => c.CreditLimit).IsModified = false;
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
                sale.TransactionDate = DateTime.Now.ToLocal();
                sale.Expiration = DateTime.Now.ToLocal();
                sale.Status = TranStatus.Reserved;
                sale.LastStatus = TranStatus.InProcess;
                sale.UpdUser = User.Identity.Name;
                sale.UpdDate = DateTime.Now.ToLocal();
                // sale.PaymentType     = (PaymentType)Enum.Parse(typeof(PaymentType), payment.ToString());
                sale.SendingType = sending;

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();


                return Json(new { Result = "OK", Message = "Se ha generado la venta con folio:" + folio });

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