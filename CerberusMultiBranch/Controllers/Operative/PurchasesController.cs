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
                   && (userId == null || p.UserId == userId));

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
                    Expiration = model.ExpirationDate,
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
                return Json(new { Result = "Error al registrar venta", Message = "Ocurrio un error al crear la compra detalle " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Compleate(int purchaseId, DateTime purchaseDate, DateTime expirationDate, TransactionType purchaseType)
        {
            try
            {
                var branchId = User.Identity.GetBranchId();

                var model = db.Purchases.Include(p => p.PurchaseDetails).Include(p => p.PurchaseDetails.Select(pd => pd.Product.BranchProducts)).
                    FirstOrDefault(p => p.PurchaseId == purchaseId);

                model.TotalAmount = model.PurchaseDetails.Sum(pd => pd.Amount);
                model.UpdDate = DateTime.Now.ToLocal();
                model.UpdUser = User.Identity.Name;
                model.TransactionDate = purchaseDate;
                model.Expiration = expirationDate;
                model.TransactionType = purchaseType;
                model.Status = TranStatus.Compleated;

                foreach (var detail in model.PurchaseDetails)
                {
                    var brp = detail.Product.BranchProducts.FirstOrDefault(bp => bp.BranchId == branchId);

                    if(brp != null)
                    {
                        brp.LastStock = brp.Stock;
                        brp.Stock += detail.Quantity;

                        brp.UpdDate = DateTime.Now.ToLocal();
                        brp.UpdUser = User.Identity.Name;

                        db.Entry(brp).State = EntityState.Modified;
                    }
                    else
                    {
                        //si no existe una realcion de producto sucursal
                        //obtengo los porcentajes configurados en base de datos y calculo los precios
                        var variables = db.Variables;
                        brp = new BranchProduct();
                        brp.BranchId  = branchId;
                        brp.ProductId = detail.ProductId;
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

                    var stkM = new StockMovement
                    {
                        BranchId = branchId,
                        Comment ="COMPRA CON FOLIO "+model.Bill,
                        MovementDate = DateTime.Now.ToLocal(),
                        ProductId = detail.ProductId,
                        MovementType = MovementType.Entry,
                        User = User.Identity.Name,
                        Quantity = detail.Quantity,
                    };

                    db.StockMovements.Add(stkM);
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
                    Header = "Error al inventariar",
                    Message = "Ocurrio un error al finalizar la compra detalle " + ex.Message
                });
            }


        }

        public ActionResult Edit(int id)
        {
            var branchIds = User.Identity.GetBranches().Select(b => b.BranchId);

            var model = db.Purchases.Include(p => p.PurchaseDetails).Include(p => p.PurchasePayments).Include(p => p.User).
                FirstOrDefault(p => p.PurchaseId == id && branchIds.Contains(p.BranchId));

            return View(model);
        }

        [HttpPost]
        public ActionResult BeginPurchase()
        {
            BeginPurchaseViewModel model = new BeginPurchaseViewModel();

            model.TransactionTypes = new List<TransactionType>();
            model.TransactionTypes.Add(TransactionType.Contado);
            model.TransactionTypes.Add(TransactionType.Credito);

            model.TransactionType = model.TransactionTypes.First();
            model.PurchaseDate = DateTime.Today.ToLocal();
            model.ExpirationDate = model.PurchaseDate.AddDays(30);

            return PartialView("_BeginPurchase", model);
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
        public ActionResult AddDetail(int productId, int purchaseId, double price, double quantity)
        {
            try
            {
                var detail = db.PurchaseDetails.
                    FirstOrDefault(d => d.ProductId == productId && d.PurchaseId == purchaseId);

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
                        PurchaseId = purchaseId,
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
                Where(d => d.PurchaseId == purchaseId).ToList();


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
