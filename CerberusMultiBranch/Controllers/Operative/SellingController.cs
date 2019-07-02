using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Models.ViewModels.Operative;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Operative
{
    [CustomAuthorize]
    public class SellingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [CustomAuthorize(Roles = "Vendedor, Supevisor")]
        public ActionResult SaleOrder(int? id)
        {
            Sale model = null;

            if (id != null)
            {
                //solo ventas de mis sucursales 
                var branchIds = User.Identity.GetBranches().Select(b => b.BranchId);

                //si no se es supervisor o administrador, solo ventas del usuario
                var userId = User.IsInRole("Supervisor") ? string.Empty : User.Identity.GetUserId();


                model = db.Sales.Include(p => p.SaleHistories).
                Include(p => p.SalePayments).Include(p => p.User).
                Include(s => s.SaleDetails.Select(td => td.Product.Images)).
                FirstOrDefault(p => p.SaleId == id &&
                                   branchIds.Contains(p.BranchId) &&
                                   (string.IsNullOrEmpty(userId) || p.UserId == userId));

                if (model == null)
                    return RedirectToAction("SaleOrder", new { id = new Nullable<int>() });
            }
            else
            {
                model = new Sale
                {
                    UserId = User.Identity.GetUserId(),
                    BranchId = User.Identity.GetBranchId(),
                    Client = db.Clients.FirstOrDefault(c => c.ClientId == Cons.Zero)
                };
            }

            return View("SaleOrder", model);
        }


        public ActionResult GetProductsInfo(List<int> productIds)
        {
            var branchId = User.Identity.GetBranchId();

            var model = GetProducts(productIds);

            return Json(new JResponse
            {
                Code = Cons.Responses.Codes.Success,
                Result = Cons.Responses.Success,
                JProperty = model
            }, JsonRequestBehavior.AllowGet);
        }

        private List<SearchProductResultViewModel> GetProducts(List<int> productIds)
        {
            var branchId = User.Identity.GetBranchId();

            var model = (from bp in db.BranchProducts.Include(b => b.Product.Images)
                         where (bp.BranchId == branchId) && productIds.Contains(bp.ProductId)
                         select new SearchProductResultViewModel
                         {
                             ProductId = bp.Product.ProductId,
                             Name = bp.Product.Name.ToUpper(),
                             Code = bp.Product.Code.ToUpper(),
                             TradeMark = bp.Product.TradeMark.ToUpper(),
                             Image = bp.Product.Images.Count > Cons.Zero ? bp.Product.Images.FirstOrDefault().Path : Cons.NoImagePath,
                             Stock = bp.Stock,
                             StorePrice = Math.Round(bp.StorePrice, Cons.Zero),
                             DealerPrice = Math.Round(bp.DealerPrice, Cons.Zero),
                             WholesalerPrice = Math.Round(bp.WholesalerPrice, Cons.Zero),
                             MinQuantity = bp.MinQuantity,
                             MaxQuantity = bp.MaxQuantity,
                             OrderQty = (bp.MaxQuantity - (bp.Stock + bp.Reserved)),
                             SellQty = (bp.Stock < Cons.One && bp.Stock > Cons.Zero) ? bp.Stock : Cons.One,
                             SaleCommission = bp.Product.System != null ? bp.Product.System.Commission : Cons.Zero

                         }).ToList();

            return model;
        }

        [HttpGet]
        public ActionResult SearchCustomer(string filter, bool fromModal = false)
        {
            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
                arr = filter.Trim().Split(' ');

            var model = (from customer in db.Clients
                         where (string.IsNullOrEmpty(filter) || arr.All(s => (customer.Code + " " + customer.Name).Contains(s))) &&
                               (customer.IsActive)
                         select customer).OrderBy(c => c.Name).Take(Cons.QuickResults).ToList();

            if (model.Count == Cons.One)
            {
                var customer = model.First();

                return Json(new JResponse
                {
                    Code = Cons.Responses.Codes.Success,
                    JProperty = new
                    {
                        ClientId = customer.ClientId,
                        Name = customer.Name,
                        Type = customer.Type.GetAttribute<DisplayAttribute>().Name,
                        CreditAvailable = customer.CreditAvailable,
                        CreditDays = customer.CreditDays,
                        CreditLimit = customer.CreditLimit
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else if (fromModal)
                return PartialView("SearchCustomerResults", model);
            else
                return PartialView("SearchCustomer", model);
        }

        [HttpGet]
        public ActionResult SearchProduct(string filter, bool fromModal = false)
        {
            var branchId = User.Identity.GetBranchId();

            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
                arr = filter.Trim().Split(' ');


            var model = (from bp in db.BranchProducts.Include(p => p.Product).Include(p => p.Product.Images)
                         where (string.IsNullOrEmpty(filter) || arr.All(s => (bp.Product.Code + " " + bp.Product.Name).Contains(s))) &&
                               (bp.BranchId == branchId) && (bp.Product.IsActive)
                         select new SearchProductResultViewModel
                         {
                             ProductId = bp.Product.ProductId,
                             Name = bp.Product.Name.ToUpper(),
                             Code = bp.Product.Code.ToUpper(),
                             TradeMark = bp.Product.TradeMark.ToUpper(),
                             Image = bp.Product.Images.Count > Cons.Zero ? bp.Product.Images.FirstOrDefault().Path : Cons.NoImagePath,
                             Stock = bp.Stock,
                             StorePrice = Math.Round(bp.StorePrice, Cons.Zero),
                             DealerPrice = Math.Round(bp.DealerPrice, Cons.Zero),
                             WholesalerPrice = Math.Round(bp.WholesalerPrice, Cons.Zero),
                             MinQuantity = bp.MinQuantity,
                             MaxQuantity = bp.MaxQuantity,
                             OrderQty = (bp.MaxQuantity - (bp.Stock + bp.Reserved)),
                             SellQty = (bp.Stock < Cons.One && bp.Stock > Cons.Zero) ? bp.Stock : Cons.One,
                             SaleCommission = bp.Product.System != null ? bp.Product.System.Commission : Cons.Zero,

                         }).OrderBy(p => p.Name).Take(Cons.QuickResults).ToList();

            if (model.Count == Cons.One)
            {
                var product = model.First();

                return Json(new JResponse
                {
                    Code = Cons.Responses.Codes.Success,
                    JProperty = product

                }, JsonRequestBehavior.AllowGet);
            }
            else if (fromModal)
                return PartialView("SearchProductResults", model);
            else
                return PartialView("SearchProduct", model);
        }


        // POST: Selling/Create
        [HttpPost]
        public ActionResult SendSaleOrder(Sale sale)
        {
            try
            {   //valido que la caja este abierta
                var isCashOpen = User.IsCashRegisterOpen();

                if (!isCashOpen)
                {
                    return Json(new JResponse
                    {
                        Code = Cons.Responses.Codes.InvalidData,
                        Result = Cons.Responses.Danger,
                        Header = "Caja cerrada",
                        Body = "No se puede hacer modificaciones a la venta si la caja esta cerrada"
                    });
                }

                //si es una modificación obtengo el registro original
                Sale dbSale = null;

                if (sale.SaleId != Cons.Zero)
                    dbSale = db.Sales.Include(s => s.SaleDetails).FirstOrDefault(s => s.SaleId == sale.SaleId);

                //si los status son diferentes
                if (dbSale != null && dbSale.Status != sale.Status)
                {
                    return Json(new JResponse
                    {
                        Code = Cons.Responses.Codes.InvalidData,
                        Result = Cons.Responses.Danger,
                        Header = "Discrepancia de datos",
                        Body = "El status de la venta no corresponde al registro en base de datos," +
                        "por favor recarga la venta presionando F5"
                    });
                }

                sale.BranchId = dbSale != null ? dbSale.BranchId : User.Identity.GetBranchId();

                var IVA = Convert.ToDouble(db.Variables.FirstOrDefault(v => v.Name == Cons.VariableIVA).Value);

                var pIds = sale.SaleDetails.Select(d => d.ProductId).ToList();

                //obtengo la información de los productos en la venta
                var productsInfo = GetProducts(pIds);

                //obtengo el inventario de los productos en la venta
                var branchProducts = db.BranchProducts.
                    Where(bp => bp.BranchId == sale.BranchId && pIds.Contains(bp.ProductId)).ToList();

                JResponse response = null;

                List<StockMovement> movements = new List<StockMovement>();

                //desgloso el IVA, Calcúlo comisión y valido existencias
                foreach (var detail in sale.SaleDetails)
                {
                    var pInfo = productsInfo.First(p => p.ProductId == detail.ProductId);

                    //partida en base de datos
                    SaleDetail dbDetail = dbSale != null ? dbSale.SaleDetails.FirstOrDefault(sd => sd.ProductId == detail.ProductId) : null;

                    detail.TaxedPrice = detail.Price; //precio con IVA
                    detail.TaxedAmount = detail.Amount; //Monto partida con IVA

                    //para productos nuevos se usa el IVA actual, para partidas anteriores se usa el 
                    //guardado en base de datos
                    var iva = dbDetail != null ? dbDetail.TaxPercentage : IVA;

                    detail.TaxPercentage = iva; //porcentaje de IVA
                    detail.Price = (detail.TaxedPrice / (Cons.One + (iva / Cons.OneHundred))).RoundMoney(); //precio sin IVA
                    detail.Amount = (detail.TaxedAmount / (Cons.One + (iva / Cons.OneHundred))).RoundMoney(); //Monto partida sin Iva
                    detail.TaxAmount = (detail.TaxedAmount - detail.Amount).RoundMoney(); //Monto total de IVA

                    if (dbDetail == null)
                    {
                        response = ValidateNewDetail(sale, detail, pInfo);

                        if (response != null)
                            break;
                    }

                    //sumo la nueva devolución a las existentes
                    detail.Refund += detail.NewRefund;

                    //si la partida es nueva y el producto tiene comisión se asigna
                    if (dbDetail == null && pInfo.SaleCommission > Cons.Zero)
                        detail.Commission = (detail.TaxedAmount * (pInfo.SaleCommission / Cons.OneHundred)).RoundMoney();

                    else if (dbDetail != null) //si la partida es vieja se asigna la comisión que había en memoria
                        detail.Commission = dbDetail.Commission;

                    var dbQuantity = dbDetail != null ? dbDetail.Quantity - dbDetail.Refund : Cons.Zero;
                    var quantity = detail.Quantity - detail.Refund;

                    //si la diferencia es negativa indica que se agregan partidas y se deben restar del stock
                    //si la diferencia es positiva indica devoluciones  (entradas de inventario)
                    var difQuantity = dbQuantity - quantity;

                    if (difQuantity != Cons.Zero)
                    {
                        var bp = branchProducts.First(b => b.ProductId == detail.ProductId);

                        bp.LastStock = bp.Stock;
                        bp.Stock = bp.Stock + difQuantity;
                        bp.UpdUser = User.Identity.Name;
                        bp.UpdDate = DateTime.Now.ToLocal();

                        //agrego el moviento al inventario
                        StockMovement movement = new StockMovement
                        {
                            ProductId = detail.ProductId,
                            BranchId = sale.BranchId,
                            User = User.Identity.Name,
                            MovementDate = DateTime.Now.ToLocal(),
                            MovementType = difQuantity > Cons.Zero ? MovementType.Entry : MovementType.Exit,
                            Quantity = difQuantity < Cons.Zero ? (-Cons.One * difQuantity) : difQuantity,
                            Comment = difQuantity > Cons.Zero ? "Devolución en venta " : "Venta con folio "
                        };

                        movements.Add(movement);
                    }
                }

                if (response != null)
                    return Json(response);


                sale.Year = dbSale != null ? dbSale.Year : Convert.ToInt32(DateTime.Now.TodayLocal().ToString("yy"));
                sale.Sequential = dbSale != null ? dbSale.Sequential : Cons.Zero;
                sale.Folio = dbSale != null ? dbSale.Folio : string.Empty;
                sale.UserId = dbSale != null ? dbSale.UserId : User.Identity.GetUserId();

                sale.TotalTaxAmount = sale.SaleDetails.Sum(d => d.TaxAmount).RoundMoney();
                sale.TotalAmount = sale.SaleDetails.Sum(d => d.Amount).RoundMoney();
                sale.TotalTaxedAmount = sale.SaleDetails.Sum(d => d.TaxedAmount).RoundMoney();

                //si no hay descuentos 
                sale.FinalAmount = sale.TotalTaxedAmount;

                //calculo de comisión para el empleado
                sale.ComPer = User.Identity.GetSalePercentage();

                if (sale.ComPer > Cons.Zero)
                    sale.ComAmount = (sale.SaleDetails.Sum(d => d.Commission) * (sale.ComPer / Cons.OneHundred)).RoundMoney();

                //una cantidad positiva indica que el monto de la venta bajo (hay que dar vale)
                //una cantidad negativa indica que el monto ve la venta incremento (hay que validar crédito)
                var sDifference = (dbSale == null ? Cons.Zero : dbSale.FinalAmount) - sale.FinalAmount;

                var change = sDifference > Cons.Zero ? sDifference : Cons.Zero;
                var credit = sDifference < Cons.Zero ? (sDifference * -Cons.One) : Cons.Zero;

                //validaciones para venta a crédito
                if (sale.TransactionType == TransactionType.Credit)
                    response = ValidateCredit(sale, credit, change);

                if (response != null)
                    return Json(response);

                //si es nueva venta se genera folio
                if (dbSale == null)
                {
                    var lastSale = db.Sales.Where(s => s.BranchId == sale.BranchId && s.Year == sale.Year).
                                                        OrderByDescending(s => s.Sequential).FirstOrDefault();

                    var seq = lastSale != null ? lastSale.Sequential : Cons.Zero;

                    sale.Sequential = (seq + Cons.One);
                    sale.Folio = User.Identity.GetFolio(sale.Sequential);
                    sale.Status = TranStatus.Reserved;
                    sale.Comment = "Venta generada con Folio " + sale.Folio;
                }
                //si es una modificación concateno el número de revisión al folio
                else if (sale.Status == TranStatus.OnChange)
                {
                    var c = 'V';

                    var arr = dbSale.Folio.Split(c);

                    if (string.IsNullOrEmpty(arr[Cons.One]))
                        sale.Folio = arr[Cons.Zero] + c + Cons.One.ToString();
                    else
                        sale.Folio = arr[Cons.Zero] + c + (Convert.ToUInt32(arr[Cons.One]) + Cons.One).ToString();

                    sale.Comment = "Venta modificada con el folio " + sale.Folio;
                    sale.Status = TranStatus.Modified;
                }
                else
                {
                    sale.Comment = "Venta modificada Folio:" + sale.Folio;
                    sale.Status = TranStatus.Reserved;
                }

                //toda venta modificada se coloca en status reserved para que aparezca en la caja
                //para su reimpresión y aplicación de pago o devolución
                var lStatus = dbSale != null ? dbSale.LastStatus : TranStatus.InProcess;

                //agrego el número de folio a los movimientos
                movements.ForEach(m => { m.Comment += sale.Folio; });

                branchProducts.ForEach(bp => { db.Entry(bp).State = EntityState.Modified; });

                db.StockMovements.AddRange(movements);

                //si es venta nueva solo la inserto
                if (sale.SaleId == Cons.Zero)
                    db.Sales.Add(sale);
                else //si es modificación debo crear un historico antes de modificar
                {
                    var saleHistory = new SaleHistory
                    {
                        SaleId = dbSale.SaleId,
                        Status = dbSale.Status.GetName(),
                        InsDate = dbSale.UpdDate,
                        User = dbSale.UpdUser,
                        Comment = dbSale.Comment,
                        TotalDue = dbSale.FinalAmount
                    };

                    db.SaleHistories.Add(saleHistory);

                    db.Entry(dbSale).CurrentValues.SetValues(sale);
                    db.Entry(dbSale).Property(s => s.InsDate).IsModified = false;
                    db.Entry(dbSale).Property(s => s.InsUser).IsModified = false;

                    var ids = sale.SaleDetails.Select(s => s.ProductId).ToList();

                    //registros a borrar (los que no esten en la nueva lista)
                    var toDelete = dbSale.SaleDetails.Where(d => !ids.Contains(d.ProductId)).ToList();

                    //detalles a actualizar
                    var toUpdate = dbSale.SaleDetails.Where(d => ids.Contains(d.ProductId)).ToList();

                    //detalles nuevos
                    var toAdd = sale.SaleDetails.Where(d => !dbSale.SaleDetails.Select(db => db.ProductId).Contains(d.ProductId)).ToList();

                    toDelete.ForEach(d => { dbSale.SaleDetails.Remove(d); });

                    toAdd.ForEach(d => { d.SaleId = sale.SaleId; dbSale.SaleDetails.Add(d); });

                    toUpdate.ForEach(d =>
                    {
                        var ds = sale.SaleDetails.First(s => s.ProductId == d.ProductId);

                        ds.SaleId = sale.SaleId;
                        db.Entry(d).CurrentValues.SetValues(ds);
                    });
                }

                db.SaveChanges();

                return Json(new JResponse
                {
                    Code = Cons.Responses.Codes.Success,
                    Result = Cons.Responses.Success,
                    Header = sale.Status == TranStatus.Reserved ? "Venta Generada!" : "Venta Modificada",
                    Body = sale.Comment,
                    Id = sale.SaleId
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Danger,
                    Header = "Error al generar la venta",
                    Body = ex.Message,
                    Id = sale.SaleId
                });
            }
        }

        [HttpGet]
        public ActionResult BeginRequestChange(int saleId, bool isCancelation)
        {
            try
            {
                var sale = db.Sales.Include(s => s.SalePayments).FirstOrDefault(s => s.SaleId == saleId);

                if (sale == null)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Header = "Registro no encontrado",
                        Body = "No se encontro la venta solicitada",
                        Code = Cons.Responses.Codes.RecordNotFound
                    }, JsonRequestBehavior.AllowGet);
                }

                if (sale.Status == TranStatus.Canceled || sale.Status == TranStatus.PreCancel)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Header = "Venta cancelada",
                        Body = "Esta venta ya ha sido cancelada",
                        Code = Cons.Responses.Codes.Success
                    }, JsonRequestBehavior.AllowGet);
                }

                if (sale.Status == TranStatus.InProcess || sale.Status == TranStatus.OnChange)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Header = "Venta en modificación",
                        Body = "Ya se solicito la modificación para esta venta",
                        Code = Cons.Responses.Codes.Success
                    }, JsonRequestBehavior.AllowGet);
                }

                var model = new SaleCancelViewModel
                {
                    SaleFolio = sale.Folio,
                    SaleCancelId = sale.SaleId,
                    PaymentCard = sale.SalePayments.Where(s => s.PaymentMethod == PaymentMethod.Tarjeta).Sum(s => s.Amount).RoundMoney(),
                    PaymentCash = sale.SalePayments.Where(s => s.PaymentMethod == PaymentMethod.Efectivo).Sum(s => s.Amount).RoundMoney(),
                    PaymentCreditNote = sale.SalePayments.Where(s => s.PaymentMethod == PaymentMethod.Vale).Sum(s => s.Amount).RoundMoney(),
                    IsCancelation = isCancelation
                };

                return PartialView("SaleOrderChange", model);

            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al obtener datos",
                    Body = ex.Message,
                    Code = Cons.Responses.Codes.ServerError
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult RequestChange(int saleId, string comment, bool isCancelation)
        {
            try
            {

                //busco la venta a la que se le cambiara el status
                var sale = db.Sales.Include(s => s.SaleDetails).Include(s => s.SalePayments).Include(s => s.Client).
                    FirstOrDefault(s => s.SaleId == saleId);

                //si se solicita una modificación sobre un estado modificado
                //regreso un error
               /* if (!isCancelation && sale.Status == TranStatus.Modified)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Header = "Operación no permita",
                        Body = "No se puede solicitar modificación sobre una venta recien modificada",
                        Code = Cons.Responses.Codes.Success
                    });
                }*/

                //creo el historico
                var saleHistory = new SaleHistory
                {
                    SaleId = sale.SaleId,
                    Comment = sale.Comment,
                    InsDate = sale.UpdDate,
                    User = sale.UpdUser,
                    Status = sale.Status.GetName(),
                    TotalDue = sale.FinalAmount
                };

                db.SaleHistories.Add(saleHistory);

                sale.LastStatus = sale.Status;
                sale.UpdUser = User.Identity.Name;
                sale.UpdDate = DateTime.Now.ToLocal();
                sale.Comment = comment;

                //si es una cancelación 
                if (isCancelation)
                    return Json(CancelSale(sale));

                //si no es cancelación
                sale.Status = sale.Status == TranStatus.Reserved ?  TranStatus.InProcess : TranStatus.OnChange;

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Success,
                    Header = "Venta " + sale.Status.GetName(),
                    Body = "Se ha solicitado la modificación de la venta " + sale.Folio,
                    Code = Cons.Responses.Codes.Success
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error en solicitud de cambio",
                    Body = ex.Message,
                    Code = Cons.Responses.Codes.ServerError
                });
            }
        }

        private JResponse CancelSale(Sale sale)
        {
            try
            {
                //ids de producto
                var ids = sale.SaleDetails.Select(d => d.ProductId).ToList();

                //obtengo los productos hijos
                var prodDetails = db.PackageDetails.Where(pd => ids.Contains(pd.PackageId)).ToList();

                //agrego esos ids de producto al listado
                ids.AddRange(prodDetails.Select(c => c.DetailtId));

                //obtengo el detalle de inventario para productos padre e hijos
                var branchProducts = db.BranchProducts.
                    Where(bp => bp.BranchId == sale.BranchId && ids.Contains(bp.ProductId)).ToList();

                foreach (var detail in sale.SaleDetails)
                {
                    var parentStock = branchProducts.
                        FirstOrDefault(b => b.ProductId == detail.ProductId);

                    //agrego movimiento al inventario
                    StockMovement parentMovement = new StockMovement
                    {
                        BranchId = parentStock.BranchId,
                        ProductId = parentStock.ProductId,
                        User = User.Identity.Name,
                        MovementDate = DateTime.Now.ToLocal(),
                        Comment = "Cancelación de venta: " + sale.Folio,
                        MovementType = MovementType.Entry
                    };
                    //agrego el movimiento de inventario
                    db.StockMovements.Add(parentMovement);

                    //modifico las existencias
                    parentStock.LastStock = parentStock.Stock;
                    parentStock.Stock += (detail.Quantity - detail.Refund);

                    //reviso si el producto es un paquete (tiene productos hijos)
                    foreach (var packDetail in prodDetails.Where(d => d.PackageId == detail.ProductId))
                    {
                        var childStock = branchProducts.FirstOrDefault(b => b.ProductId == detail.ProductId);

                        //agrego movimiento al inventario
                        StockMovement childMovement = new StockMovement
                        {
                            BranchId = parentStock.BranchId,
                            ProductId = parentStock.ProductId,
                            User = User.Identity.Name,
                            MovementDate = DateTime.Now.ToLocal(),
                            Comment = "Cancelación de venta: " + sale.Folio,
                            MovementType = MovementType.Reservation
                        };

                        childStock.Reserved += (detail.Quantity - detail.Quantity) * packDetail.Quantity;

                        db.StockMovements.Add(childMovement);
                        db.Entry(childStock).State = EntityState.Modified;
                    }

                    db.Entry(parentStock).State = EntityState.Modified;
                }

                //se obtiene el total de pagos
                var payments = sale.SalePayments.Sum(p => p.Amount);

                //si la venta es credito, se resta el monto al deudo de la cuenta del cliente
                if (sale.TransactionType == TransactionType.Credit)
                    sale.Client.UsedAmount -= (sale.FinalAmount - payments).RoundMoney();

                var message = string.Empty;
                if (payments > Cons.Zero)
                {
                    message = "La venta ha sido enviada a caja para completar la cancelación";
                    sale.Status = TranStatus.PreCancel;
                }
                else
                {
                    message = "La venta ha sido completamente cancelada";
                    sale.Status = TranStatus.Canceled;
                }

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                return new JResponse
                {
                    Code = Cons.Responses.Codes.Success,
                    Result = Cons.Responses.Success,
                    Body = message,
                    Header = "Venta " + sale.Status.GetName()
                };

            }
            catch (Exception ex)
            {
                return new JResponse
                {
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Danger,
                    Body = ex.Message,
                    Header = "Error al Cancelar!"
                };
            }
        }


        private JResponse ValidateNewDetail(Sale sale, SaleDetail detail, SearchProductResultViewModel pInfo)
        {

            JResponse response = null;

            //se valida que los productos tengan existencias suficiente, salvo para preventa
            if (sale.TransactionType != TransactionType.Presale && detail.Quantity > pInfo.Stock)
            {
                response = new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Cantidad  insuficiente",
                    Body = string.Format("Estas requiriendo {0} unidades del Producto {1}! Cantidad en inventario: {2} ",
                    detail.Quantity, pInfo.Code, pInfo.Stock),
                    Code = Cons.Responses.Codes.InvalidData
                };
            }

            if (sale.TransactionType == TransactionType.Presale && pInfo.Stock > Cons.Zero)
            {
                response = new JResponse
                {
                    Result = Cons.Responses.Info,
                    Header = "Producto en Inventario",
                    Body = string.Format("No es posible generar preventa con productos que aun tienen existencia, " +
                        "el inventario cuenta con {0} unidades del producto {1}", pInfo.Stock, pInfo.Code),
                    Code = Cons.Responses.Codes.InvalidData
                };
            }

            return response;

        }

        private JResponse ValidateCredit(Sale sale, double credit, double change)
        {
            var client = db.Clients.Find(sale.ClientId);

            if (sale.Status == TranStatus.InProcess)
            {
                var dt = DateTime.Now.TodayLocal();

                //verifico si el cliente tiene alguna venta a credito pendiente de pago
                var pendingSales = db.Sales.Where(s => s.ClientId == client.ClientId && s.Expiration < dt && 
                                   s.TransactionType == TransactionType.Credit && s.Status == TranStatus.Reserved);

                if (pendingSales != null && pendingSales.Count() > Cons.Zero)
                {
                    return new JResponse
                    {
                        Result = Cons.Responses.Danger,
                        Header = "Incumplimiento de crédito",
                        Body = string.Format("El cliente {0} tiene ventas a crédito expiradas, es necesario liquidar el monto",
                                            client.Name.ToUpper())
                    };
                }
            }

            //los días de crédito solo se validan para nuevas ventas
            if (sale.Status == TranStatus.InProcess && client.CreditAvailable < credit)
            {
                return new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Límite de crédito excedido",
                    Body = string.Format("El costo total de esta venta {0} excede el crédito disponible {1} del cliente {2}",
                                       credit.ToMoney(), client.CreditAvailable.ToMoney(), client.Name.ToUpper()),
                    Code = Cons.Responses.Codes.ConditionMissing
                };
            }

            else if (client.CreditDays <= Cons.Zero)
            {
                return new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Sin Prorroga de pago",
                    Body = string.Format("No se han configurado días de crédito para el cliente {0}", client.Name.ToUpper()),
                    Code = Cons.Responses.Codes.ConditionMissing
                };
            }
            else //actualizo el monto usado, sumo el nuevo crédiro y resto el cambio
            {
                client.UsedAmount += credit;
                client.UsedAmount -= change;

                db.Entry(client).Property(c => c.UsedAmount).IsModified = true;
            }

            return null;
        }


        [HttpPost]
        public ActionResult AutoCompleateSale(string filter)
        {
            //solo ventas las sucursales autorizadas
            var branchsIds = User.Identity.GetBranches().Select(p => p.BranchId).ToList();

            //si no se es supervisor, solo ventas del usuario
            var userId = User.IsInRole("Supervisor") ? string.Empty : User.Identity.GetUserId();

            var model = db.Sales.Where(p =>
                            branchsIds.Contains(p.BranchId) &&
                            (string.IsNullOrEmpty(userId) || p.UserId == userId) &&
                            p.Folio.Contains(filter)).Take(20).
                Select(p =>
                    new { Id = p.SaleId, Label = p.Folio, Value = p.Folio, Response = Cons.Responses.Success, Code = Cons.Responses.Codes.Success });

            return Json(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public ActionResult CreateBudget(Sale sale)
        {
            try
            {
                Budget budget = new Budget
                {
                    ClientId = sale.ClientId,
                    DueDate = DateTime.Today.ToLocal().AddDays(Cons.DaysToCancel),
                    BranchId = User.Identity.GetBranchId(),
                    BudgetDate = DateTime.Now.ToLocal(),
                    UserName = User.Identity.Name
                };

                var iva = Convert.ToDouble(db.Variables.FirstOrDefault(v => v.Name == Cons.VariableIVA).Value);


                sale.SaleDetails.ToList().ForEach(item =>
                {
                    item.TaxedAmount = Math.Round(item.Price * item.Quantity, Cons.Two);
                    item.TaxedPrice = item.Price;
                    item.TaxPercentage = iva; //porcentaje de IVA
                    item.Price = (item.TaxedPrice / (Cons.One + (iva / Cons.OneHundred))).RoundMoney(); //precio sin IVA
                    item.Amount = (item.TaxedAmount / (Cons.One + (iva / Cons.OneHundred))).RoundMoney(); //Monto partida sin Iva
                    item.TaxAmount = (item.TaxedAmount - item.Amount).RoundMoney(); //Monto total de IVA

                    var detail = new BudgetDetail
                    {
                        Amount = item.Amount,
                        InsDate = DateTime.Now.ToLocal(),
                        Price = item.Price,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        TaxAmount = item.TaxAmount,
                        TaxedAmount = item.TaxedAmount,
                        TaxedPrice = item.TaxedPrice,
                        TaxPercentage = item.TaxPercentage
                    };

                    budget.BudgetDetails.Add(detail);
                });

                db.Budgets.Add(budget);
                db.SaveChanges();
                ViewBag.Message = "Hola mundo mundial";
                var model = db.Budgets.Include(b => b.Branch).Include(b => b.Client.Addresses).
                    Include(b => b.BudgetDetails.Select(d => d.Product)).Include(b => b.Client).FirstOrDefault(b => b.BudgetId == budget.BudgetId);

                return PartialView("PrintBudget", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Body = "Ocurrio un error mientras se generaba el presupuesto",
                    Header = "Error al Generar!",
                    Code = Cons.Responses.Codes.ServerError
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
