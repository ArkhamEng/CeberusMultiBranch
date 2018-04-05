﻿using System;
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
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using System.Text.RegularExpressions;
using CerberusMultiBranch.Models.Entities.Finances;

namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize(Roles = "Supervisor,Capturista")]
    public class PurchasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult History()
        {
            //obtengo las sucursales configuradas para el empleado
            var branches = User.Identity.GetBranches();

            TransactionViewModel model = new TransactionViewModel();
            model.Branches = branches.ToSelectList();
            model.Purchases = new List<Purchase>();

            return View(model);
        }

        public ActionResult DetailOld(int id)
        {
            //obtengo las sucursales configuradas para el empleado
            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var userId = !User.IsInRole("Supervisor") ? User.Identity.GetUserId() : null;

            var purchase = db.Purchases.Include(p => p.PurchaseDetails).Include(p => p.User).
                   Include(p => p.PurchaseDetails.Select(td => td.Product.Images)).
                   FirstOrDefault(p => p.PurchaseId == id && brancheIds.Contains(p.BranchId)
                   && (userId == null || p.UserId == userId));

            if (purchase == null)
                return RedirectToAction("History");

            return View(purchase);
        }

        [HttpPost]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate,
            string bill, string provider, string user, TranStatus? status)
        {
            //si el usuario no es un supervisor, busco solo las comprar registradas por el 
            var userId = !User.IsInRole("Supervisor") ? User.Identity.GetUserId() : null;

            var model = LookFor(branchId, beginDate, endDate, bill, provider, user, userId, status);
            return PartialView("_PurchaseList", model);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public JsonResult Cancel(int transactionId, string comment)
        {
            try
            {
                //busco la venta a cancelar
                var purchase = db.Purchases.Include(s => s.PurchaseDetails).
                    FirstOrDefault(s => s.PurchaseId == transactionId);

                //regreso los productos al stock
                foreach (var detail in purchase.PurchaseDetails)
                {
                    var bp = db.BranchProducts.Find(purchase.BranchId, detail.ProductId);

                    if (bp.Stock < detail.Quantity)
                    {
                        return Json(new
                        {
                            Result = "Error al cancelar la compra",
                            Message = "No hay producto suficiente para realizar la devolución"
                        });
                    }

                    bp.LastStock = bp.Stock;
                    bp.Stock -= detail.Quantity;
                    db.Entry(bp).State = EntityState.Modified;

                    //creo un movimiento de stock
                    StockMovement movement = new StockMovement
                    {
                        BranchId = bp.BranchId,
                        ProductId = bp.ProductId,
                        MovementDate = DateTime.Now,
                        User = User.Identity.Name,
                        MovementType = MovementType.Exit,
                        Comment = string.Format("Cancelación de compra {0} comentarios {1}",
                                                        purchase.Bill, comment),
                        Quantity = detail.Quantity
                    };

                    db.StockMovements.Add(movement);
                }

                //desactivo la venta y registo usuario, comentario y fecha de cancelación
                purchase.LastStatus = purchase.Status;
                purchase.Status = TranStatus.Canceled;
                purchase.UpdUser = User.Identity.Name;
                purchase.UpdDate = DateTime.Now;
                purchase.Comment = comment;

                db.Entry(purchase).State = EntityState.Modified;

                db.SaveChanges();

                return Json(new { Result = "OK", Message = "Compra Cancelada, el producto ha sido regreado al stock" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al cancelar la compra", Message = ex.Message });
            }
        }

        private List<Purchase> LookFor(int? branchId, DateTime? beginDate, DateTime? endDate, string bill, string provider, string user, string userId, TranStatus? status)
        {
            //si el filtro de sucursal viene nulo
            //Busco las compras hechas en las sucursales asignadas del usuario
            var branchIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var purchases = (from p in db.Purchases.Include(p => p.User).Include(p => p.User.Employees).Include(p => p.PurchaseDetails)
                             where (branchId == null && branchIds.Contains(p.BranchId) || p.BranchId == branchId)
                             && (beginDate == null || p.TransactionDate >= beginDate)
                             && (endDate == null || p.TransactionDate <= endDate)
                             && (bill == null || bill == string.Empty || p.Bill.Contains(bill))
                             && (provider == null || provider == string.Empty || p.Provider.Name.Contains(provider))
                             && (userId == null || p.UserId == userId)
                             && (user == null || user == string.Empty || p.User.UserName.Contains(user))
                             && (status == null || p.Status == status)
                             select p).ToList();

            return purchases;
        }

        //this method is call when a new purchase is going to be registered
        [HttpPost]
        public ActionResult BeginPurchase()
        {
            BeginPurchaseViewModel model = new BeginPurchaseViewModel();

            return PartialView("_BeginPurchase", model);
        }


        //this method is call when a new purchase is going to be saved
        [HttpPost]
        [Authorize(Roles = "Capturista")]
        public ActionResult Create(BeginPurchaseViewModel model)
        {
            try
            {
                var purchase = new Purchase
                {
                    Bill = model.Bill,
                    BranchId = User.Identity.GetBranchId(),
                    UserId = User.Identity.GetUserId(),
                    TransactionDate = model.PurchaseDate,
                    Expiration = model.PurchaseDate.AddDays(model.Days),
                    TransactionType = model.TransactionType,
                    ProviderId = model.ProviderId,
                    Status = TranStatus.InProcess,
                    UpdUser = User.Identity.Name,
                    UpdDate = DateTime.Now.ToLocal()
                };

                db.Purchases.Add(purchase);
                db.SaveChanges();

                return Json(new { Result = "OK", Id = purchase.PurchaseId });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al registrar la compra", Message = "Ocurrio un error al crear la compra detalle " + ex.Message });
            }
        }


        [HttpPost]
        public ActionResult SearchProducts(string filter, int providerId)
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

            products = (from p in db.Products.Include(p => p.Images).Include(p => p.BranchProducts).Include(p => p.Equivalences)
                        where (p.Code == code) && (p.ProductType == ProductType.Single)
                        select p).Take((int)Cons.OneHundred).ToList();

            if (products.Count == Cons.Zero)
            {
                products = (from ep in db.Products.Include(p => p.Images).Include(p => p.BranchProducts).Include(p => p.Equivalences)
                            where (filter == null || filter == string.Empty || arr.All(s => (ep.Code + " " + ep.Name + " " + ep.TradeMark).Contains(s)))
                            && (ep.ProductType == ProductType.Single)
                            select ep).Take((int)Cons.OneHundred).ToList();
            }

            //obtengo el código del proveedor si es que tiene
            products.ForEach(p =>
            {
                var eq = p.Equivalences.FirstOrDefault(e => e.ProviderId == providerId);
                p.ProviderCode = eq == null ? string.Empty : eq.Code;
            });

            //esto lo dejo pq lo puedo usar para solo permitir compras de productos
            //que su existencia esta por debajo del máximo
            /*  var branchId = User.Identity.GetBranchId();

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
              }*/


            return PartialView("_ProductResult", products);
        }


        [HttpPost]
        public ActionResult BeginAdd(int productId, int providerId)
        {
            var branchId = User.Identity.GetBranchId();

            var product = db.Products.Include(p => p.Images).Include(p => p.Equivalences).
                Include(p => p.BranchProducts).
                FirstOrDefault(p => p.ProductId == productId);

            //obtengo el precio de compra seteado en la sucursal
            var branP = product.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);

            //si no existe configuración de sucursal, envío un precio en cero
            product.BuyPrice = (branP == null) ? (double)Decimal.Zero : branP.BuyPrice;

            //si la cantidad máxima de producto no ha sido configurada, permito la compra de 100 articulos
            product.MaxQuantity = product.MaxQuantity == Cons.Zero ? Cons.OneHundred : product.MaxQuantity;

            //si el producto esta relacionado con un producto del proveedor
            var eq = product.Equivalences.FirstOrDefault(e => e.ProviderId == providerId);

            if (eq != null)
            {
                //si hay una realción con producto del proveedor
                var exProd = db.ExternalProducts.FirstOrDefault(e => e.ProviderId == providerId && e.Code == eq.Code);

                if (exProd != null)
                {
                    product.BuyPrice = exProd.Price;
                    product.ProviderCode = exProd.Code;
                }
            }

            return PartialView("_AddProduct", product);
        }


        [HttpPost]
        public ActionResult AutoCompleateExternal(string filter, int entityId)
        {
            var model = db.ExternalProducts.Where(p => p.Code.Contains(filter) && p.ProviderId == entityId).Take(20).
                Select(p => new { Id = p.Code.ToUpper(), Label = p.Code.ToUpper(), Value = p.Description.ToUpper() });

            return Json(model);
        }

        [HttpPost]
        public ActionResult AddDetail(int productId, int purchaseId, double price, double quantity, string externalCode, int providerId)
        {
            try
            {
                var detail = db.PurchaseDetails.
                    FirstOrDefault(d => d.ProductId == productId && d.PurchaseId == purchaseId);

                //obtengo el IVA configurado en base de datos
                var taxPercentage = Convert.ToDouble(db.Variables.FirstOrDefault(v => v.Name == Cons.VariableIVA).Value);

                //Calculo el precio con IVA
                var taxAmount = Math.Round((price * (taxPercentage / Cons.OneHundred)), Cons.Two);
                var taxedPrice = Math.Round((price + taxAmount), Cons.Two);


                //compruebo si existe el código entre los productos del proveedor
                var extProd = db.ExternalProducts.
                    FirstOrDefault(ex => ex.ProviderId == providerId && ex.Code == externalCode);

                var product = db.Products.Include(p => p.Equivalences).
                                FirstOrDefault(p => p.ProductId == productId);

                //si el producto no existe en la lista del proveedor, lo agrego con el precio SIN IVA
                if (extProd == null)
                {
                    extProd = new ExternalProduct
                    {
                        ProviderId = providerId,
                        Code = externalCode,
                        Price = price,
                        Description = product.Name,
                        TradeMark = product.TradeMark,
                        Unit = product.Unit
                    };
                    //agrego el producto a la lista
                    db.ExternalProducts.Add(extProd);
                }
                else
                {
                    //modifico el precio en el listado de proveedor
                    extProd.Price = price;
                    db.Entry(extProd).Property(p => p.Price).IsModified = true;
                }

                var equivalence = product.Equivalences.FirstOrDefault(e => e.ProviderId == providerId && e.ProductId == productId);

                if (equivalence == null)
                {
                    equivalence = new Equivalence
                    { ProviderId = providerId, Code = externalCode, ProductId = productId };

                    db.Equivalences.Add(equivalence);
                }
                else if (equivalence.Code != externalCode)
                {
                    equivalence.Code = externalCode;
                    db.Entry(equivalence).State = EntityState.Modified;
                }


                if (detail != null)
                {
                    detail.Quantity += quantity;
                    detail.Price = price;
                    detail.Amount = price * detail.Quantity;
                    detail.TaxAmount = taxAmount;
                    detail.TaxedPrice = taxedPrice;
                    detail.TaxPercentage = taxPercentage;
                    detail.Amount = detail.Quantity * detail.Price;
                    detail.TaxAmount = detail.Quantity * detail.TaxedPrice;


                    db.Entry(detail).State = EntityState.Modified;
                }
                else
                {
                    detail = new PurchaseDetail
                    {
                        ProductId = productId,
                        PurchaseId = purchaseId,
                        Quantity = quantity,
                        Price = price, //precio sin IVA
                        Amount = price * quantity, // monto total sin IVA
                        TaxedPrice = taxedPrice, //Precio con IVA
                        TaxedAmount = taxedPrice * quantity, //monto total Con IVA
                        TaxPercentage = taxPercentage, //Porcentaje e IVA
                        TaxAmount = taxAmount //monto de IVA
                    };

                    db.PurchaseDetails.Add(detail);
                }

                db.SaveChanges();

                var purchase = db.Purchases.Include(s => s.PurchaseDetails).FirstOrDefault(s => s.PurchaseId == purchaseId);

                purchase.TotalAmount = purchase.PurchaseDetails.Sum(d => d.Amount);
                purchase.TotalTaxedAmount = purchase.PurchaseDetails.Sum(d => d.TaxedAmount);
                purchase.TotalTaxAmount = purchase.PurchaseDetails.Sum(d => d.TaxAmount);
                purchase.UpdDate = DateTime.Now.ToLocal();
                purchase.UpdUser = User.Identity.Name;

                db.Entry(purchase).State = EntityState.Modified;

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("AddDetail", ex);
            }

            var model = db.PurchaseDetails.Include(d => d.Product.Images).
                Where(d => d.PurchaseId == purchaseId).ToList();

            model.ForEach(p =>
            {
                p.Product.Equivalences = db.Equivalences.Where(e => e.ProductId == p.ProductId && e.ProviderId == providerId).ToList();
            });


            return PartialView("_Details", model);
        }

        [HttpPost]
        public ActionResult RemoveDetail(int transactionId, int productId)
        {
            var detail = db.PurchaseDetails.FirstOrDefault(d => d.ProductId == productId && d.PurchaseId == transactionId);

            if (detail != null)
            {
                db.PurchaseDetails.Remove(detail);
                db.SaveChanges();
            }

            var purchase = db.Purchases.Include(s => s.PurchaseDetails).FirstOrDefault(s => s.PurchaseId == transactionId);

            purchase.TotalAmount = purchase.PurchaseDetails.Sum(d => d.Amount);
            purchase.TotalTaxAmount = purchase.PurchaseDetails.Sum(d => d.TaxAmount);
            purchase.TotalTaxedAmount = purchase.PurchaseDetails.Sum(d => d.TaxedAmount);

            purchase.UpdDate = DateTime.Now.ToLocal();
            purchase.UpdUser = User.Identity.Name;

            db.Entry(purchase).State = EntityState.Modified;

            db.SaveChanges();

            var model = db.PurchaseDetails.Include(d => d.Product.Images).Include(d => d.Purchase).
              Where(d => d.PurchaseId == transactionId).ToList();


            model.ForEach(p =>
            {
                p.Product.Equivalences = db.Equivalences.Where(e => e.ProductId == p.ProductId
                && e.ProviderId == p.Purchase.ProviderId).ToList();
            });

            return PartialView("_Details", model);
        }

        //this method is call when the purchase is finishes and inventoried
        [HttpPost]
        public ActionResult Compleate(int purchaseId)
        {
            try
            {
                var branchId = User.Identity.GetBranchId();

                var model = db.Purchases.Include(p => p.PurchaseDetails).Include(p => p.PurchaseDetails.Select(pd => pd.Product.BranchProducts)).
                    FirstOrDefault(p => p.PurchaseId == purchaseId);

                model.TotalAmount = model.PurchaseDetails.Sum(pd => pd.Amount);
                model.UpdDate = DateTime.Now.ToLocal();
                model.UpdUser = User.Identity.Name;
                model.Status = TranStatus.Reserved;

                foreach (var detail in model.PurchaseDetails)
                {
                    var brp = detail.Product.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);

                    //obtengo los porcentajes configurados en base de datos y calculo los precios
                    var variables = db.Variables;

                    if (brp != null)
                    {
                        //si ya existe una relacion de producto sucursal verifico los precios

                        //si el precio de compra en sucursal es mayor al precio del detalle
                        //se promedian las cantidades y se genera un nuevo precio
                        if (brp.BuyPrice > detail.Price)
                        {
                            var oldAmount = (brp.Stock + brp.Reserved) * brp.BuyPrice;

                            brp.BuyPrice = (oldAmount + detail.Amount) / (brp.Stock + brp.Reserved + detail.Quantity);
                        }
                        //si el nuevo precio de compra es mayor o igual se actualiza y recalculan los precios (para eliminar errores previos en la captura del precio)
                        else
                            brp.BuyPrice = detail.Price;

                        //se recalculan los precios
                        brp.DealerPrice = Math.Round(brp.BuyPrice * (Cons.One + (brp.DealerPercentage / Cons.OneHundred)), Cons.Zero);
                        brp.StorePrice = Math.Round(brp.BuyPrice * (Cons.One + (brp.StorePercentage / Cons.OneHundred)), Cons.Zero);
                        brp.WholesalerPrice = Math.Round(brp.BuyPrice * (Cons.One + (brp.WholesalerPercentage / Cons.OneHundred)), Cons.Zero);

                        //si el producto esta configurado para generar stock, actualizo las cantidades
                        if (detail.Product.StockRequired)
                        {
                            brp.LastStock = brp.Stock;
                            brp.Stock += detail.Quantity;
                        }

                        brp.UpdDate = DateTime.Now.ToLocal();
                        brp.UpdUser = User.Identity.Name;

                        db.Entry(brp).State = EntityState.Modified;
                    }
                    else
                    {
                        //si no existe una relacion de producto sucursal

                        brp = new BranchProduct();
                        brp.BranchId = branchId;
                        brp.ProductId = detail.ProductId;
                        brp.BuyPrice = detail.Price;
                        brp.DealerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.DealerPercentage)).Value);
                        brp.StorePercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.StorePercentage)).Value);
                        brp.WholesalerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.WholesalerPercentage)).Value);

                        brp.DealerPrice = Math.Round(brp.BuyPrice * (Cons.One + (brp.DealerPercentage / Cons.OneHundred)), Cons.Zero);
                        brp.StorePrice = Math.Round(brp.BuyPrice * (Cons.One + (brp.StorePercentage / Cons.OneHundred)), Cons.Zero);
                        brp.WholesalerPrice = Math.Round(brp.BuyPrice * (Cons.One + (brp.WholesalerPercentage / Cons.OneHundred)), Cons.Zero);

                        brp.LastStock = Cons.Zero;
                        brp.Stock += detail.Quantity;

                        brp.UpdDate = DateTime.Now.ToLocal();
                        brp.UpdUser = User.Identity.Name;

                        db.BranchProducts.Add(brp);
                    }

                    //si el producto esta configurado para generar stock, entonces registro el movimiento
                    if (detail.Product.StockRequired)
                    {
                        var stkM = new StockMovement
                        {
                            BranchId = branchId,
                            Comment = "COMPRA CON FOLIO " + model.Bill,
                            MovementDate = DateTime.Now.ToLocal(),
                            ProductId = detail.ProductId,
                            MovementType = MovementType.Entry,
                            User = User.Identity.Name,
                            Quantity = detail.Quantity,
                        };

                        db.StockMovements.Add(stkM);
                    }
                }
                db.Entry(model);
                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error",
                    Header = "Error al inventariar!!",
                    Message = "Ocurrio un error al finalizar la compra detalle " + ex.Message
                });
            }
        }

        //this method is call for edit purchases
        public ActionResult Detail(int id)
        {
            var branchIds = User.Identity.GetBranches().Select(b => b.BranchId);

            ViewBag.Systems = db.Systems.ToSelectList();

            var model = db.Purchases.
                Where(p => p.PurchaseId == id && branchIds.Contains(p.BranchId)).
                Include(p => p.PurchaseDetails).Include(p => p.PurchaseDetails.Select(td => td.Product.Images)).
                Include(p => p.PurchasePayments).Include(p => p.User).
                FirstOrDefault();

            model.PurchaseDetails.ToList().ForEach(p =>
            {
                p.Product.Equivalences = db.Equivalences.Where(e => e.ProductId == p.ProductId
                && e.ProviderId == p.Purchase.ProviderId).ToList();
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult AddPayment(PurchasePayment payment)
        {
            try
            {
                payment.UpdDate = DateTime.Now.ToLocal();
                payment.UpdUser = User.Identity.Name;

                var purchase = db.Purchases.Include(p => p.PurchasePayments).
                    FirstOrDefault(p => p.PurchaseId == payment.PurchaseId);

                var total = purchase.PurchasePayments.Sum(p => p.Amount);

                total += payment.Amount;
                if (purchase.TotalAmount >= total)
                {
                    db.PurchasePayments.Add(payment);
                    if (total == purchase.TotalAmount)
                    {
                        purchase.Status = TranStatus.Compleated;
                        purchase.UpdDate = DateTime.Now.ToLocal();
                        purchase.UpdUser = User.Identity.Name;

                        db.Entry(purchase).State = EntityState.Modified;
                    }

                    db.SaveChanges();

                    purchase.PurchasePayments.Add(payment);
                    purchase.PurchasePayments.OrderByDescending(p => p.PaymentDate);

                    return PartialView("_PurchasePayments", purchase.PurchasePayments);
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
                var payment = db.PurchasePayments.Find(id);

                var purchaseId = payment.PurchaseId;

                if (payment.Purchase.Status == TranStatus.Compleated)
                {
                    payment.Purchase.Status = TranStatus.Reserved;
                    payment.Purchase.UpdDate = DateTime.Now.ToLocal();
                    payment.Purchase.UpdUser = User.Identity.Name;

                    db.Entry(payment.Purchase).State = EntityState.Modified;
                }

                db.PurchasePayments.Remove(payment);
                db.SaveChanges();

                var model = db.PurchasePayments.Where(p => p.PurchaseId == purchaseId).ToList();
                return PartialView("_PurchasePayments", model);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error",
                    Header = "Error al remover el pago!!",
                    Message = "Ocurrio un error inesperado al eliminar el pago detalle: " + ex.Message
                });
            }
        }


        #region Metodos Descontinuados
        //this method looks for providers products to be registered in a purchase
        [HttpPost]
        public ActionResult SearchExternalProducts(string filter, int providerId)
        {
            string code = string.Empty;
            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
                arr = filter.Trim().Split(' ');


            var products = (from ep in db.ExternalProducts
                            join eq in db.Equivalences.Include(e => e.Product)
                            on new { ep.ProviderId, ep.Code } equals new { eq.ProviderId, eq.Code } into gj
                            from x in gj.DefaultIfEmpty()

                            where (filter == null || filter == string.Empty || arr.All(s => (ep.Code + "" + ep.Description).Contains(s)))
                             && (ep.ProviderId == providerId)
                            select new
                            {
                                ProviderId = ep.ProviderId,
                                Code = ep.Code,
                                Description = ep.Description,
                                InternalCode = gj.FirstOrDefault().Product.Code,
                                Price = ep.Price,
                                TradeMark = ep.TradeMark,
                                Unit = ep.Unit,
                                ProductId = (int?)gj.FirstOrDefault().ProductId ?? Cons.Zero
                            }).Take((int)Cons.OneHundred).ToList();

            var model = products.Select(ep => new ExternalProduct
            {
                ProviderId = ep.ProviderId,
                Code = ep.Code,
                Description = ep.Description,
                InternalCode = ep.InternalCode,
                Price = ep.Price,
                TradeMark = ep.TradeMark,
                Unit = ep.Unit,
                ProductId = ep.ProductId
            }).ToList();

            return PartialView("_ExternalProductList", model);
        }

        [HttpPost]
        public ActionResult SearchInternalProducts(string filter)
        {
            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
                arr = filter.Trim().Split(' ');

            var products = (from ep in db.Products.Include(p => p.Category)
                            where (filter == null || filter == string.Empty || arr.All(s => (ep.Code + "" + ep.Name).Contains(s)))
                            select ep).Take((int)Cons.OneHundred).ToList();

            return PartialView("_InternalProductList", products);
        }

        [HttpPost]
        public JsonResult AddRelation(int internalId, int providerId, string code)
        {
            try
            {
                //busco si el producto del provedor ta tiene una relación de equivalencia
                var eq = db.Equivalences.FirstOrDefault(e => e.ProviderId == providerId && e.Code == code);

                //si ya existe la actualizo con un nuevo productId
                if (eq != null)
                {
                    eq.ProductId = internalId;
                    db.Entry(eq).State = EntityState.Modified;
                }
                else
                {
                    //si no existe creo una nueva realacion
                    eq = new Equivalence { ProductId = internalId, ProviderId = providerId, Code = code };
                    db.Equivalences.Add(eq);
                }

                db.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al crear realción", Message = ex.Message });
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
