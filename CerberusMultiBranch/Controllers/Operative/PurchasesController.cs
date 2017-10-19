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

namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize]
    public class PurchasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Purchases
        public ActionResult Index()
        {
            //obtengo las sucursales configuradas para el empleado
            var branches = User.Identity.GetBranches();

            TransactionViewModel model = new TransactionViewModel();
            model.Branches = branches.ToSelectList();

            model.Purchases = LookFor(null, DateTime.Today, null, null, null, null, branches);

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate, string bill, string provider, string employee)
        {
            var branches = User.Identity.GetBranches();
            var model = LookFor(branchId, beginDate, endDate, bill, provider, employee, branches);

            return PartialView("_PurchaseList", model);
        }

     

        private List<Purchase> LookFor(int? branchId, DateTime? beginDate, DateTime? endDate, string bill, string provider, string employee, List<Branch> branches)
        {
            //var bList = branches.Select(b => b.BranchId).ToList();
            var bId = User.Identity.GetBranchId();

            //busco los userId de los empleados que coincidan con el filtro
            var uList = (employee == null || employee == string.Empty) ?
                db.Employees.Where(e => e.Name.Contains(employee)).Select(e => e.UserId).ToList() : null;

            var purchases = (from p in db.Purchases.Include(p => p.User).Include(p => p.User.Employees).Include(p => p.TransactionDetails)
                             where (p.BranchId == bId)
                             && (beginDate == null || p.TransactionDate >= beginDate)
                             && (endDate == null || p.TransactionDate <= endDate)
                             && (bill == null || bill == string.Empty || p.Bill.Contains(bill))
                             && (provider == null || provider == string.Empty || p.Provider.Name.Contains(provider))
                             && (employee == null || employee == string.Empty || uList.Contains(p.UserId))
                             select p).ToList();

            return purchases;
        }

   

     
        public ActionResult Create(int? id)
        {
            Transaction model = null;

            if (id != null)
            {
                model = db.Purchases.Include(p => p.TransactionDetails.Select(d => d.Product.Images)).
                    FirstOrDefault(p => p.TransactionId == id);
            }

            if (model == null)
            {
                model = new Purchase();
                model.UserId = User.Identity.GetUserId<string>();
                model.BranchId = User.Identity.GetBranchSession().Id;
            }

            var employee = db.Employees.FirstOrDefault(e => e.UserId == model.UserId);
            model.EmployeeName = employee.Name;
            model.EmployeeId = employee.EmployeeId;

            return View("Create",model);
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "User,Provider")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                purchase.IsPayed = (purchase.PaymentType == PaymentType.Contado);

                if (purchase.TransactionId == Cons.Zero)
                    db.Purchases.Add(purchase);
                else
                {
                    purchase.UserId = User.Identity.GetUserId();
                    purchase.BranchId = User.Identity.GetBranchId();
                    db.Entry(purchase).State = EntityState.Modified;
                }

                if (purchase.Inventoried)
                {
                    var detailes = db.TransactionDetails.Where(td => td.TransactionId == purchase.TransactionId).ToList();

                    if(purchase.IsPayed)
                    {
                        Payment p       = new Payment();
                        p.Amount        = purchase.TotalAmount;
                        p.PaymentDate   = purchase.TransactionDate;
                        p.TransactionId = purchase.TransactionId;
                        p.PaymentType   = PaymentType.Contado;

                        db.Payments.Add(p);
                    }

                    foreach (var det in detailes)
                    {
                        var prod = db.Products.Include(p => p.BranchProducts).FirstOrDefault(p => p.ProductId == det.ProductId);
                        var bProd = prod.BranchProducts.FirstOrDefault(bp => bp.BranchId == purchase.BranchId);

                        //if the new price is biger than the old one, just update it
                        if (det.Price > prod.BuyPrice)
                        {
                            prod.BuyPrice        = det.Price;
                            prod.DealerPrice     = det.Price.GetPrice(prod.DealerPercentage);
                            prod.StorePrice      = det.Price.GetPrice(prod.StorePercentage);
                            prod.WholesalerPrice = det.Price.GetPrice(prod.WholesalerPercentage);
                            db.Entry(prod).State = EntityState.Modified;
                        }
                        else if (det.Price < prod.BuyPrice)
                        {
                            var oldAmount = bProd.Stock * prod.BuyPrice;
                            var newAmount = det.Quantity * det.Price;
                            var totQuantity = det.Quantity + prod.BranchProducts.Sum(bp => bp.Stock);

                            var newPrice = Math.Round(((oldAmount + newAmount) / totQuantity), Cons.Two);

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
                                BranchId  = purchase.BranchId,
                                LastStock = Cons.Zero,
                                Stock     = det.Quantity,
                                UpdDate   = DateTime.Now
                            };

                            db.BranchProducts.Add(bProd);
                        }
                        else
                        {
                            bProd.LastStock = bProd.Stock;
                            bProd.Stock     += det.Quantity;
                            bProd.UpdDate   = DateTime.Now;

                            db.Entry(bProd).State = EntityState.Modified;
                        }
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Create", new { id = purchase.TransactionId });
            }

            return View(purchase);
        }

        [HttpPost]
        public ActionResult AddDetail(int productId, int transactionId, double price, double quantity)
        {
            try
            {
                var detail = db.TransactionDetails.FirstOrDefault(d => d.ProductId == productId && d.TransactionId == transactionId);

                if (detail != null)
                {
                    detail.Quantity += quantity;
                    detail.Amount = detail.Quantity * detail.Price;

                    db.Entry(detail).State = EntityState.Modified;
                }
                else
                {
                    detail = new TransactionDetail
                    {
                        ProductId = productId,
                        TransactionId = transactionId,
                        Quantity = quantity,
                        Price = price,
                        Amount = price * quantity
                    };

                    db.TransactionDetails.Add(detail);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("AddDetail", ex);
            }

            var model = db.TransactionDetails.Include(d => d.Product.Images).
                Where(d => d.TransactionId == transactionId).ToList();


            return PartialView("_Details", model);
        }

        [HttpPost]
        public ActionResult RemoveDetail(int transactionId, int productId)
        {
            var detail = db.TransactionDetails.FirstOrDefault(d => d.ProductId == productId && d.TransactionId == transactionId);

            if (detail != null)
            {
                db.TransactionDetails.Remove(detail);
                db.SaveChanges();
            }

            var model = db.TransactionDetails.Include(d => d.Product.Images).
              Where(d => d.TransactionId == transactionId).ToList();


            return PartialView("_Details", model);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PurchaseId,ProviderId,BranchId,TotalAmount,Bill,PurchaseDate,InsDate,UpdDate,EmployeeId")] Transaction purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var purchase = db.Purchases.Find(id);
            db.Purchases.Remove(purchase);
            db.SaveChanges();
            return RedirectToAction("Index");
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
