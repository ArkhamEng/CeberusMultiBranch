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

        public ActionResult Detail(int id)
        {
            //obtengo las sucursales configuradas para el empleado
            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var userId = !User.IsInRole("Supervisor") ? User.Identity.GetUserId() : null;

            var purchase = db.Purchases.Include(p => p.PurchaseDetails).Include(p => p.User).
                   Include(p => p.PurchaseDetails.Select(td => td.Product.Images)).
                   FirstOrDefault(p => p.PurchaseId == id && brancheIds.Contains(p.BranchId) 
                   && (userId == null || p.UserId == userId) );

            if (purchase == null)
                return RedirectToAction("History");

            return View(purchase);
        }

        [HttpPost]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate,
            string bill, string provider, string user)
        {
            //si el usuario no es un supervisor, busco solo las comprar registradas por el 
            var userId = !User.IsInRole("Supervisor") ? User.Identity.GetUserId() : null;

            var model = LookFor(branchId, beginDate, endDate, bill, provider, user, userId);
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

        private List<Purchase> LookFor(int? branchId, DateTime? beginDate, DateTime? endDate, string bill, string provider, string user, string userId)
        {
            //si el filtro de sucursal viene nulo
            //Busco las compras hechas en las sucursales asignadas del usuario
            var branchIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var purchases = (from p in db.Purchases.Include(p => p.User).Include(p => p.User.Employees).Include(p => p.PurchaseDetails)
                             where (branchId == null || branchIds.Contains(p.BranchId) || p.BranchId == branchId)
                             && (beginDate == null || p.TransactionDate >= beginDate)
                             && (endDate == null || p.TransactionDate <= endDate)
                             && (bill == null || bill == string.Empty || p.Bill.Contains(bill))
                             && (provider == null || provider == string.Empty || p.Provider.Name.Contains(provider))
                             && (userId == null || p.UserId == userId)
                             && (user == null || user == string.Empty || p.User.UserName.Contains(user))
                             select p).ToList();

            return purchases;
        }


        [Authorize(Roles = "Capturista")]
        public ActionResult Create(int? id)
        {
            Purchase model = null;

            if (id != null)
            {
                model = db.Purchases.Include(p => p.PurchaseDetails.Select(d => d.Product.Images)).
                    FirstOrDefault(p => p.PurchaseId == id);
            }

            if (model == null)
            {
                model = new Purchase();
                model.UserId = User.Identity.GetUserId<string>();
                model.BranchId = User.Identity.GetBranchSession().Id;
                model.UpdUser = User.Identity.Name;
            }

            return View("Create", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Capturista")]
        public ActionResult Create([Bind(Exclude = "User,Provider")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                purchase.UpdDate = DateTime.Now;
                purchase.UpdUser = User.Identity.Name;

                if (purchase.PurchaseId == Cons.Zero)
                    db.Purchases.Add(purchase);
                else
                {
                    purchase.UserId = User.Identity.GetUserId();
                    purchase.BranchId = User.Identity.GetBranchId();
                    
                    db.Entry(purchase).State = EntityState.Modified;
                }

                if (purchase.Status == TranStatus.Compleated)
                {
                    var detailes = db.PurchaseDetails.Where(td => td.PurchaseId == purchase.PurchaseId).ToList();

                    //if (purchase.PaymentType == PaymentType.Contado)
                    //{
                    //    Payment p = new Payment();
                    //    p.Amount = purchase.TotalAmount;
                    //    p.PaymentDate = purchase.TransactionDate;
                    //    p.TransactionId = purchase.PurchaseId;
                    //    p.PaymentType = PaymentType.Contado;

                    //    db.Payments.Add(p);
                    //}

                    foreach (var det in detailes)
                    {
                        var prod = db.Products.Include(p => p.BranchProducts).
                            FirstOrDefault(p => p.ProductId == det.ProductId);

                        var bProd = prod.BranchProducts.FirstOrDefault(bp => bp.BranchId == purchase.BranchId);

                        //if the new price is biger than the old one, just update it
                        if (det.Price > prod.BuyPrice)
                        {
                            prod.BuyPrice = det.Price;
                            prod.DealerPrice = det.Price.GetPrice(prod.DealerPercentage);
                            prod.StorePrice = det.Price.GetPrice(prod.StorePercentage);
                            prod.WholesalerPrice = det.Price.GetPrice(prod.WholesalerPercentage);
                            db.Entry(prod).State = EntityState.Modified;
                        }
                        else if (det.Price < prod.BuyPrice)
                        {
                            var oldAmount = bProd.Stock * prod.BuyPrice;
                            var newAmount = det.Quantity * det.Price;
                            var totQuantity = det.Quantity + prod.BranchProducts.Sum(bp => bp.Stock);

                            var newPrice = Math.Round(((oldAmount + newAmount) / totQuantity), Cons.Two);

                            prod.BuyPrice = newPrice;
                            prod.DealerPrice = newPrice.GetPrice(prod.DealerPercentage);
                            prod.StorePrice = newPrice.GetPrice(prod.StorePercentage);
                            prod.WholesalerPrice = newPrice.GetPrice(prod.WholesalerPercentage);
                            db.Entry(prod).State = EntityState.Modified;
                        }


                        if (bProd == null)
                        {
                            bProd = new BranchProduct
                            {
                                ProductId = det.ProductId,
                                BranchId = purchase.BranchId,
                                LastStock = Cons.Zero,
                                Stock = det.Quantity,
                                UpdDate = DateTime.Now
                            };

                            db.BranchProducts.Add(bProd);
                        }
                        else
                        {
                            bProd.LastStock = bProd.Stock;
                            bProd.Stock += det.Quantity;
                            bProd.UpdDate = DateTime.Now;

                            db.Entry(bProd).State = EntityState.Modified;
                        }

                        StockMovement sm = new StockMovement
                        {
                            ProductId = bProd.ProductId,
                            BranchId = bProd.BranchId,
                            User = User.Identity.Name,
                            MovementDate = DateTime.Now,
                            Comment = string.Format("Ingreso por compra en factura {0}", purchase.Bill),
                            MovementType = MovementType.Entry,
                            Quantity = det.Quantity
                        };

                        db.StockMovements.Add(sm);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Create", new { id = purchase.PurchaseId });
            }

            return View(purchase);
        }

        [HttpPost]
        public ActionResult SearchExternalProducts(string filter, int providerId)
        {
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

        [HttpPost]
        public ActionResult BeginCopy(int providerId, string code)
        {
            var variables = db.Variables;
            var ep = db.ExternalProducts.Find(providerId, code);

            ProductViewModel vm = new ProductViewModel();
            vm.Categories = db.Categories.ToSelectList();
            vm.Name = ep.Description;
            vm.TradeMark = ep.TradeMark;
            vm.Unit = ep.Unit;
            vm.BuyPrice = ep.Price;
            vm.MinQuantity = Cons.One;
            vm.Code = Regex.Replace(ep.Code, @"[^A-Za-z0-9]+$", "");
            vm.DealerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.DealerPercentage)).Value);
            vm.StorePercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.StorePercentage)).Value);
            vm.WholesalerPercentage = Convert.ToInt16(variables.FirstOrDefault(v => v.Name == nameof(Product.WholesalerPercentage)).Value);

            return PartialView("_CloneProduct", vm);
        }

        [HttpPost]
        public JsonResult Copy(Product product, int providerId, string code)
        {
            if (db.Products.FirstOrDefault(p => p.Code == product.Code) != null)
            {
                return Json(new { Result = "Codigo invalido", Message = "Ya existe un producto con este código" });
            }
            else
            {
                try
                {
                    product.Code = Regex.Replace(product.Code, @"[^A-Za-z0-9]+", "");
                    product.UpdUser = User.Identity.Name;
                    product.UpdDate = DateTime.Now;
                    product.MinQuantity = Cons.One;
                    product.StorePrice = Math.Round(product.BuyPrice * (Cons.One + (product.StorePercentage / Cons.OneHundred)), Cons.Zero);
                    product.DealerPrice = Math.Round(product.BuyPrice * (Cons.One + (product.DealerPercentage / Cons.OneHundred)), Cons.Zero);
                    product.WholesalerPrice = Math.Round(product.BuyPrice * (Cons.One + (product.WholesalerPercentage / Cons.OneHundred)), Cons.Zero);
                    product.IsActive = true;

                    db.Products.Add(product);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Error al guardar el producto", Message = ex.Message });
                }

                try
                {
                    var eq = new Equivalence { ProviderId = providerId, Code = code, ProductId = product.ProductId };
                    db.Equivalences.Add(eq);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Error al asociar el producto", Message = ex.Message });
                }

                return Json(new { Result = "OK" });
            }
        }

        [HttpPost]
        public ActionResult AddDetail(int productId, int transactionId, double price, double quantity)
        {
            try
            {
                var detail = db.PurchaseDetails.
                    FirstOrDefault(d => d.ProductId == productId && d.PurchaseId == transactionId);

                if (detail != null)
                {
                    detail.Quantity += quantity;
                    detail.Amount = detail.Quantity * detail.Price;

                    db.Entry(detail).State = EntityState.Modified;
                }
                else
                {
                    detail = new PurchaseDetail
                    {
                        ProductId = productId,
                        PurchaseId = transactionId,
                        Quantity = quantity,
                        Price = price,
                        Amount = price * quantity
                    };

                    db.PurchaseDetails.Add(detail);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("AddDetail", ex);
            }

            var model = db.PurchaseDetails.Include(d => d.Product.Images).
                Where(d => d.PurchaseId == transactionId).ToList();


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

            var model = db.PurchaseDetails.Include(d => d.Product.Images).
              Where(d => d.PurchaseId == transactionId).ToList();


            return PartialView("_Details", model);
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
