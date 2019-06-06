using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.ViewModels.Operative;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Operative
{
    public partial class SalesController
    {

        #region Shopping Cart Methods
        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public JsonResult CheckCart()
        {
            var model = GetCart(User.Identity.GetUserId(), User.Identity.GetBranchId());

            if (model != null)
                return Json(new JResponse { Result = Cons.Responses.Success, Code = Cons.Responses.Codes.Success, Extra = (model.Sum(td => td.Quantity)).ToString() });
            else
                return Json(new JResponse { Result = Cons.Responses.Success, Code = Cons.Responses.Codes.Success, Extra = (Cons.Zero).ToString() });
        }

        [CustomAuthorize(Roles = "Vendedor")]
        [HttpPost]
        public ActionResult OpenCart()
        {
            try
            {
                var model = GetCart(User.Identity.GetUserId(), User.Identity.GetBranchId());
                return PartialView("_Cart", model);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Ocurrio un error al revisar el carrito", Message = ex.Message });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public ActionResult RemoveFromCart(int productId)
        {
            var branchId = User.Identity.GetBranchId();
            var userId = User.Identity.GetUserId();

            var detail = db.ShoppingCarts.Find(userId, branchId, productId);

            if (detail != null)
            {
                try
                {
                    db.ShoppingCarts.Remove(detail);
                    db.SaveChanges();

                    var model = GetCart(userId, branchId);

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

        private List<ShoppingCart> GetCart(string userId, int branchId)
        {
            var model = db.ShoppingCarts.Include(s => s.Product.Images).Include(s => s.Client).
                            Include(s => s.Product.BranchProducts).Include(s => s.Product.PackageDetails).
                            Where(s => s.UserId == userId && s.BranchId == branchId).OrderBy(s => s.InsDate)
                            .OrderByDescending(s => s.InsDate).ToList();

            model.ForEach(m =>
            {
                var bp = m.Product.BranchProducts.FirstOrDefault(b => b.BranchId == branchId);
                m.InStock = bp != null ? bp.Stock : Cons.Zero;
                m.Product.StorePrice = bp != null ? bp.StorePrice : Cons.Zero;
                m.Product.DealerPrice = bp != null ? bp.DealerPrice : Cons.Zero;
                m.Product.WholesalerPrice = bp != null ? bp.WholesalerPrice : Cons.Zero;
            });

            return model;
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public JsonResult AddToCart(int productId, double quantity)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var branchId = User.Identity.GetBranchId();

                //Obtengo el inventario en la sucursal
                var bp = db.BranchProducts.Include(brp => brp.Product).
                            FirstOrDefault(brp => brp.BranchId == branchId && brp.ProductId == productId);

                if (bp == null)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.ConditionMissing,
                        Header = "Producto no inventariado",
                        Body = "Este producto no ha sido inventariado en la sucursal, configuralo correctamente antes de intentar venderlo"
                    });
                }

                if (bp.Product.IsLocked && bp.Product.UserLock == User.Identity.Name)
                {
                    if (bp == null)
                    {
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Info,
                            Code = Cons.Responses.Codes.ConditionMissing,
                            Header = "Producto bloqueado!",
                            Body = "Bloqueado por " + bp.Product.UserLock + " hora de bloqueo " + bp.Product.LockDate.Value.ToString("dd/MM/yyyy hh:mm")
                        });
                    }
                }

                #region validación de precios
                //valido la configuración de precios
                if (bp.StorePrice <= Cons.Zero)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.ConditionMissing,
                        Header = "Error en el precio",
                        Body = "El precio de mostrador debe ser mayor a $0, revisa la configuración"
                    });
                }
                else if (bp.DealerPrice <= Cons.Zero)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.ConditionMissing,
                        Header = "Error en el precio",
                        Body = "El precio de distribuidor debe ser mayor a $0, revisa la configuración"
                    });
                }
                else if (bp.WholesalerPrice <= Cons.Zero)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.ConditionMissing,
                        Header = "Error en el precio",
                        Body = "El precio de mayorista debe ser mayor a $0, revisa la configuración"
                    });
                }
                #endregion


                //obtengo el IVA configurado en base de datos
                var iva = Convert.ToDouble(db.Variables.FirstOrDefault(v => v.Name == Cons.VariableIVA).Value);

                //consulto si el usuario tiene un carrito activo en la sucursal
                var items = db.ShoppingCarts.Include(s => s.Client).Where(s => s.UserId == userId && s.BranchId == branchId).ToList();

                var amount = 0.0;
                var price = 0.0;


                //se asigna el precio en base a la configuración del cliente
                //si no hay un cliente asignado se asigna precio mostrador
                var f = items.FirstOrDefault() ?? new ShoppingCart { UserId = userId, BranchId = branchId };

                if (f.Client == null || f.Client.Type == ClientType.Store)
                {
                    amount = (quantity * bp.StorePrice);
                    price = bp.StorePrice;
                }
                else
                {
                    switch (f.Client.Type)
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


                ShoppingCart detail = null;
                //checo si el producto ya esta en la venta
                if (items.Count > Cons.Zero)
                    detail = items.FirstOrDefault(td => td.ProductId == productId);

                if (detail != null)
                {
                    detail.Quantity += quantity;
                    detail.TaxedAmount = (detail.TaxedPrice * detail.Quantity);
                    detail.Price = (detail.TaxedPrice / (Cons.One + (iva / Cons.OneHundred))).RoundMoney();
                    detail.Amount = (detail.Price * detail.Quantity).RoundMoney();
                    detail.TaxAmount = ((detail.TaxedPrice - detail.Price) * detail.Quantity).RoundMoney();

                    db.Entry(detail).Property(p => p.Quantity).IsModified = true;
                    db.Entry(detail).Property(p => p.TaxedAmount).IsModified = true;
                    db.Entry(detail).Property(p => p.Price).IsModified = true;
                    db.Entry(detail).Property(p => p.Amount).IsModified = true;
                    db.Entry(detail).Property(p => p.TaxAmount).IsModified = true;

                    db.SaveChanges();
                }
                else
                {
                    detail = new ShoppingCart
                    {
                        ProductId = productId,
                        BranchId = branchId,
                        UserId = userId,
                        ClientId = f.ClientId,
                        InsDate = DateTime.Now.ToLocal()
                    };

                    detail.TaxPercentage = iva;
                    detail.Quantity = quantity;
                    detail.TaxedAmount = amount.RoundMoney();
                    detail.TaxedPrice = price.RoundMoney();
                    detail.Price = (detail.TaxedPrice / (Cons.One + (iva / Cons.OneHundred))).RoundMoney();
                    detail.Amount = (detail.Price * detail.Quantity).RoundMoney();
                    detail.TaxAmount = ((detail.TaxedPrice - detail.Price) * detail.Quantity).RoundMoney();

                    db.ShoppingCarts.Add(detail);
                    db.SaveChanges();
                }

                return Json(new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Producto agregado",
                    Body = "Se agregaron " + quantity + " unidades del producto " + bp.Product.Code + " a la venta"
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Header = "Error el agregar",
                    Body = "Ocurrio un error al agregar el producto al carrito"
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public JsonResult SetClient(int clientId)
        {
            try
            {
                var branchId = User.Identity.GetBranchId();
                var userId = User.Identity.GetUserId();

                var cartItems = GetCart(userId, branchId);

                //obtengo el IVA configurado en base de datos
                var iva = Convert.ToDouble(db.Variables.FirstOrDefault(v => v.Name == Cons.VariableIVA).Value);

                var client = db.Clients.Find(clientId);

                foreach (var deteail in cartItems)
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

                    deteail.ClientId = clientId;
                    db.Entry(deteail).State = EntityState.Modified;
                }

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Info,
                    Header = "Cliente Asignado",
                    Body = "Se asigno el cliente " + client.Name.ToUpper() + " a la venta",
                    Code = Cons.Responses.Codes.Success
                });
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al Asignar Cliente",
                    Body = "Ocurrio un error inesperado al asignar el cliente a la venta",
                    Code = Cons.Responses.Codes.Success
                });
            }
        }


        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public ActionResult BeginChangePrice(int productId)
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();

            var item = db.ShoppingCarts.Include(s => s.Product).Include(s => s.Product.BranchProducts).
                        FirstOrDefault(s => s.BranchId == branchId && s.UserId == userId && s.ProductId == productId);

            var bp = item.Product.BranchProducts.FirstOrDefault(b => b.BranchId == branchId);

            var model = new PriceSelectorViewModel
            {
                PsProductId = productId,
                CPrice = item.TaxedPrice,
                SPrice = bp != null ? bp.StorePrice : Cons.Zero,
                DPrice = bp != null ? bp.DealerPrice : Cons.Zero,
                WPrice = bp != null ? bp.WholesalerPrice : Cons.Zero,
                IsCart = true
            };

            return PartialView("_PriceSelector", model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public ActionResult SetPrice(int productId, double price)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var branchId = User.Identity.GetBranchId();

                var model = GetCart(userId, branchId);
                var det = model.FirstOrDefault(s => s.ProductId == productId);

                //obtengo el IVA configurado en base de datos
                var iva = Convert.ToDouble(db.Variables.FirstOrDefault(v => v.Name == Cons.VariableIVA).Value);

                //el precio seteado Incluye IVA
                det.TaxedPrice = price;
                det.TaxedAmount = det.TaxedPrice * det.Quantity;

                //Calculo el precio sin IVA y el monto Total sin IVA
                det.Price = (det.TaxedPrice / (Cons.One + (iva / Cons.OneHundred))).RoundMoney();
                det.Amount = (det.Price * det.Quantity).RoundMoney();

                //calculo el monto de IVA en el detalle
                det.TaxAmount = ((det.TaxedPrice - det.Price) * det.Quantity).RoundMoney();

                db.Entry(det).State = EntityState.Modified;
                db.SaveChanges();


                return PartialView("_CartDetails", model);

            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Header = "Error al modificar el costo!",
                    Result = Cons.Responses.Danger,
                    Body = "Ocurrio un error al actualizar el precio  de ventar",
                    Code = Cons.Responses.Codes.ServerError
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public ActionResult SetQuantity(int productId, double quantity)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var branchId = User.Identity.GetBranchId();

                var model = GetCart(userId, branchId);

                var i = model.FirstOrDefault(s => s.ProductId == productId);

                i.Quantity = quantity;
                i.Amount = (quantity * i.Price).RoundMoney();
                i.TaxedAmount = (quantity * i.TaxedPrice).RoundMoney();
                i.TaxAmount = (i.TaxedAmount - i.Amount).RoundMoney();

                db.Entry(i).Property(p => p.Quantity).IsModified = true;
                db.Entry(i).Property(p => p.Amount).IsModified = true;
                db.Entry(i).Property(p => p.TaxedAmount).IsModified = true;
                db.Entry(i).Property(p => p.TaxAmount).IsModified = true;

                db.SaveChanges();

                return PartialView("_CartDetails", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Header = "Error al modificar la cantidad!",
                    Result = Cons.Responses.Danger,
                    Body = "Ocurrio un error al actualizar el la cantidad de unidades",
                    Code = Cons.Responses.Codes.ServerError
                });
            }

        }

        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public ActionResult OpenQuantity(int productId)
        {
            var userId = User.Identity.GetUserId();
            var branchId = User.Identity.GetBranchId();
            var prod = db.ShoppingCarts.Include(s => s.Product).FirstOrDefault(s => s.ProductId == productId &&
                                         s.UserId == userId && s.BranchId == branchId);

            QuantityChangeViewModel model = new QuantityChangeViewModel
            { cqProductId = productId, cqCode = prod.Product.Code, cqQuantity = prod.Quantity, cqUnit = prod.Product.Unit };

            return PartialView("_ChangeQuantity", model);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public ActionResult EmptyCart()
        {
            try
            {
                var cartItems = GetCart(User.Identity.GetUserId(), User.Identity.GetBranchId());

                Budget budget = new Budget();

                db.ShoppingCarts.RemoveRange(cartItems);
                db.SaveChanges();

                return PartialView("_CartDetails", new List<ShoppingCart>());
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error al imprimir el presupuesto",
                    Message = "Ocurrion un error mientras se generaba el presupuesto " + ex.Message
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public JsonResult CompleateSale(DispatchMethod sending, TransactionType type)
        {
            try
            {
                if (!User.IsCashRegisterOpen())
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Header = "Caja Cerrada",
                        Body = "No hay ninguna caja abierta en la sucursa, antes de vender la caja debe ser abierta",
                        Code = Cons.Responses.Codes.ConditionMissing
                    });
                }

                var userId = User.Identity.GetUserId();
                var branchId = User.Identity.GetBranchId();

                var cartItems = GetCart(userId, branchId);

                var client = cartItems.First().Client;

                #region Validaciones para preventas y ventas a credito

                //validación de cliente valido
                if (type != TransactionType.Cash && client.ClientId == Cons.Zero)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Header = "Venta sin cliente asignado",
                        Body = "Los apartados, preventas y ventas a crédito requieren que asigne un cliente",
                        Code = Cons.Responses.Codes.ConditionMissing
                    });
                }

                var amount = cartItems.Sum(i => i.TaxedAmount);

                int days = Cons.DaysToCancel;

                //validaciones de crédito
                if (type == TransactionType.Credit)
                {
                    var dt = DateTime.Now.TodayLocal();

                    //verifico si el cliente tiene alguna venta a credito pendiente de pago
                    var pendingSales = db.Sales.Where(s => s.Expiration < dt &&
                                       s.TransactionType == TransactionType.Credit && s.Status == TranStatus.Reserved);

                    if (pendingSales != null && pendingSales.Count() > Cons.Zero)
                    {
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Danger,
                            Header = "Incumplimiento de crédito",
                            Body = string.Format("El cliente {0} tiene ventas a crédito expiradas, es necesario liquidar el monto",
                                                client.Name.ToUpper())
                        });
                    }

                    if (amount > client.CreditAvailable)
                    {
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Danger,
                            Header = "Limite de crédito excedido",
                            Body = string.Format("El costo total de esta venta {0} excede el crédito disponible {1} del cliente {2}",
                                               amount.ToMoney(), client.CreditAvailable.ToMoney(), client.Name.ToUpper()),
                            Code = Cons.Responses.Codes.ConditionMissing
                        });
                    }

                    else if (client.CreditDays <= Cons.Zero)
                    {
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Danger,
                            Header = "Sin Prorroga de pago",
                            Body = string.Format("No se han configurado días de crédito para el cliente {0}", client.Name.ToUpper()),
                            Code = Cons.Responses.Codes.ConditionMissing
                        });
                    }
                    else
                    {
                        //actualizo el monto usado
                        client.UsedAmount += amount;
                        db.Entry(client).Property(c => c.UsedAmount).IsModified = true;
                    }

                    days = client.CreditDays;
                }
                //preventa, solo permite productos sin existencias
                if (type == TransactionType.Presale)
                {
                    var item = cartItems.FirstOrDefault(i => i.InStock > Cons.Zero);

                    if (item != null)
                    {
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Info,
                            Header = "Producto en Inventario",
                            Body = string.Format("No es posible generar preventa con productos que aun tienen existencia, " +
                            "el inventario cuenta con {0} {1}(s) del producto {2}", item.InStock, item.Product.Unit.ToUpper(), item.Product.Code.ToUpper()),
                            Code = Cons.Responses.Codes.InvalidData
                        });
                    }
                }

                #endregion

                //creo la venta
                Sale sale = new Sale
                {
                    Year = Convert.ToInt32(DateTime.Now.TodayLocal().ToString("yy")),
                    BranchId = User.Identity.GetBranchId(),
                    UserId = User.Identity.GetUserId(),
                    SendingType = sending,
                    ClientId = cartItems.FirstOrDefault().ClientId,
                    LastStatus = TranStatus.InProcess,
                    Status = TranStatus.Reserved,
                    TransactionType = type,
                    Expiration = type == TransactionType.Cash ? DateTime.Now.ToLocal() : DateTime.Now.ToLocal().AddDays(days)
                };



                int sortOrder = Cons.One;
                List<StockMovement> movements = new List<StockMovement>();

                foreach (var item in cartItems)
                {
                    //se valida que los productos tengan existencias suficiente, salvo para preventa
                    if (type != TransactionType.Presale && !item.CanSell)
                    {
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Danger,
                            Header = "Cantidad  insuficiente",
                            Body = string.Format("Estas requiriendo {0} {1}(s) del Producto {2}! Cantidad en inventario {3} {1}(s)",
                            item.Quantity, item.Product.Unit, item.Product.Code, item.InStock),
                            Code = Cons.Responses.Codes.InvalidData
                        });
                    }

                    //creo un detalle por cada item del carrito
                    var detail = new SaleDetail
                    {
                        Amount = item.Amount,
                        Price = item.Price,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        TaxAmount = item.TaxAmount,
                        TaxedAmount = item.TaxedAmount,
                        TaxedPrice = item.TaxedPrice,
                        TaxPercentage = item.TaxPercentage
                    };

                    detail.SortOrder = sortOrder;

                    if (item.Product.System.Commission > Cons.Zero)
                        detail.Commission = Math.Round(detail.TaxedAmount * (item.Product.System.Commission / Cons.OneHundred), Cons.Decimals);

                    //agrego el detalle a la venta
                    sale.SaleDetails.Add(detail);

                    //las preventas no generan movimiento de inventario al ser creadas
                    //if (type != TransactionType.Preventa)
                    //{
                    var bp = item.Product.BranchProducts.FirstOrDefault(b => b.BranchId == branchId);

                    //actualizo stock de sucursal
                    bp.LastStock = bp.Stock;
                    bp.Stock -= detail.Quantity;
                    bp.UpdDate = DateTime.Now.ToLocal();
                    bp.UpdUser = User.Identity.Name;

                    db.Entry(bp).State = EntityState.Modified;
                    //}


                    //si el producto que se esta vendiendo es un paquete
                    //agrego todos los productos q lo complementan a la venta con precio 0
                    if (item.Product.ProductType == ProductType.Package)
                    {
                        foreach (var pckDet in item.Product.PackageDetails)
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

                            detBP.Reserved -= pckDet.Quantity;
                            detBP.UpdDate = DateTime.Now.ToLocal();
                            detBP.UpdUser = User.Identity.Name;

                            //agrego el detalle (del paquete) a la venta

                            sale.SaleDetails.Add(tDeatil);
                            db.Entry(detBP).State = EntityState.Modified;

                        }
                    }

                   
                    //agrego el moviento al inventario
                    StockMovement sm = new StockMovement
                    {
                        ProductId = item.ProductId,
                        BranchId = branchId,
                        MovementType = MovementType.Exit,
                        User = User.Identity.Name,
                        MovementDate = DateTime.Now.ToLocal(),
                        Quantity = detail.Quantity,
                    };
                    movements.Add(sm);
                   
                    sortOrder++;
                }

                //coloco el porcentaje de comision del empleado
                sale.ComPer = User.Identity.GetSalePercentage();

                //si tiene comision por venta, coloco la cantidad de esta
                if (sale.ComPer > Cons.Zero)
                {
                    var comTot = sale.SaleDetails.Sum(td => td.Commission);

                    if (comTot > Cons.Zero)
                        sale.ComAmount = Math.Round(comTot * (sale.ComPer / Cons.OneHundred), Cons.Decimals);
                }

                //monto antes de Iva
                sale.TotalAmount = sale.SaleDetails.Sum(d => d.Amount).RoundMoney();

                //monto de Iva
                sale.TotalTaxAmount = sale.SaleDetails.Sum(d => d.TaxAmount).RoundMoney();

                //monto despues de Iva
                sale.TotalTaxedAmount = sale.SaleDetails.Sum(d => d.TaxedAmount).RoundMoney();

                //Monto final (si hay descuentos varia del anterior)
                sale.FinalAmount = sale.SaleDetails.Sum(d => d.TaxedAmount).RoundMoney();

                //obtengo la utima venta para generar unel folio siguiente
                var lastSale = db.Sales.Where(s => s.Status != TranStatus.InProcess &&
                                                      s.BranchId == sale.BranchId && s.Year == sale.Year).
                                                      OrderByDescending(s => s.Sequential).FirstOrDefault();

                var seq = lastSale != null ? lastSale.Sequential : Cons.Zero;

                sale.Sequential = (seq + Cons.One);
                sale.Folio = User.Identity.GetFolio(sale.Sequential);

                if (type != TransactionType.Presale)
                {
                    movements.ForEach(m => { m.Comment = "SALIDA AUTOMATICA, VENTA CON FOLIO: " + sale.Folio.ToUpper(); });
                    db.StockMovements.AddRange(movements);
                }

                db.Sales.Add(sale);
                db.ShoppingCarts.RemoveRange(cartItems);
                db.SaveChanges();


                return Json(new JResponse
                {
                    Result = Cons.Responses.Success,
                    Body = "Se ha generado la venta con folio:" + sale.Folio,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Venta Concluida"
                });

            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al generar la venta",
                    Body = "Ocurrio un error inesperado al generar la venta",
                    Code = Cons.Responses.Codes.ServerError
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public ActionResult CreateBudget()
        {
            try
            {
                var cartItems = GetCart(User.Identity.GetUserId(), User.Identity.GetBranchId());

                Budget budget = new Budget();
                budget.ClientId = cartItems.First().ClientId;

                cartItems.ForEach(item =>
                {
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
                db.ShoppingCarts.RemoveRange(cartItems);

                db.SaveChanges();

                var model = db.Budgets.Include(b => b.Branch).Include(b => b.Client.Addresses).
                    Include(b => b.BudgetDetails.Select(d => d.Product)).Include(b => b.Client).FirstOrDefault(b => b.BudgetId == budget.BudgetId);

                return PartialView("_PrintBudget", model);
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

        [HttpPost]
        [CustomAuthorize(Roles = "Vendedor")]
        public ActionResult PrintBudget(int id)
        {
            try
            {
                var model = db.Budgets.Include(b => b.Branch).Include(b => b.Client.Addresses).
             Include(b => b.BudgetDetails.Select(d => d.Product)).Include(b => b.Client).FirstOrDefault(b => b.BudgetId == id);

                return PartialView("_PrintBudget", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Body = "Ocurrio un error al obtener el presupuesto",
                    Header = "Error al Imprimir!",
                    Code = Cons.Responses.Codes.ServerError
                });
            }

        }

        #endregion

        #region Factura diaria
        public ActionResult DailyBill()
        {
            DailyBillViewModel model = new DailyBillViewModel();
            model.Branches = User.Identity.GetBranches().ToSelectList();

            return View(model);
        }

        [HttpPost]
        public ActionResult SearchSoldItems(DailyBillViewModel model)
        {

            var SoldItems = (from sd in db.SaleDetails.Include(sd => sd.Product.Category)
                             where (sd.Sale.TransactionDate >= model.Date) &&
                                   (sd.Sale.TransactionDate < model.EndDate) &&
                                   (sd.Sale.TransactionType == model.TransType) &&
                                   (sd.Sale.Status == TranStatus.Compleated) && //solo las ventas pagadas en su totalidad
                                   (sd.Sale.BranchId == model.BranchId) &&
                                   (string.IsNullOrEmpty(model.Client) || sd.Sale.Client.Name == model.Client) &&
                                   (string.IsNullOrEmpty(model.Folio) || sd.Sale.Folio == model.Folio)
                             select sd).ToList();


            return PartialView("_SoldItemsDetails", SoldItems);
        }

        #endregion
    }
}