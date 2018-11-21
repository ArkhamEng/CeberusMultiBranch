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
    [CustomAuthorize]
    public partial class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Sale History
        [CustomAuthorize(Roles = "Supervisor,Vendedor")]
        public ActionResult Report()
        {
            //obtengo las sucursales configuradas para el empleado
            var branches = User.Identity.GetBranches();

            TransactionViewModel model = new TransactionViewModel();
            model.Branches = branches.ToSelectList();
            model.Sales = LookFor(null, null, null, null, null, null, TranStatus.Revision, null,100);

            return View(model);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "Supervisor, Cajero")]
        public JsonResult Cancel(int saleId, string comment)
        {
            try
            {
                //busco la venta a la que se le cambiara el status
                var sale = db.Sales.Include(s => s.SaleDetails).Include(s => s.SalePayments).Include(s => s.Client).
                    FirstOrDefault(s => s.SaleId == saleId);

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
        [CustomAuthorize(Roles = "Supervisor,Vendedor")]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate,
            string folio, string client, string user, TranStatus? status)
        {
            //si el usuario no es un supervisor, solo se le permite ver el dato de sus ventas
            var userId = !User.IsInRole("Supervisor") ? User.Identity.GetUserId() : null;

            var model = LookFor(branchId, beginDate, endDate, folio, client, user, status, userId);

            return PartialView("_SaleList", model);
        }


        private List<Sale> LookFor(int? branchId, DateTime? beginDate, DateTime? endDate, string folio, string client,
            string user, TranStatus? status, string userId, int top = 10000)
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
                         select p).Take(top).OrderByDescending(p => p.TransactionDate).ToList();

            return sales;
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



        [CustomAuthorize(Roles = "Supervisor,Vendedor")]
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
        #endregion


        #region Budget History

        [CustomAuthorize(Roles = "Supervisor,Vendedor")]
        public ActionResult Budget()
        {
            var model = new BudgetViewModel();

            model.Branches = db.Branches.ToSelectList();

            return View(model);
        }



        [HttpPost]
        [CustomAuthorize(Roles = "Supervisor,Vendedor")]
        public ActionResult SearchBudgets(int? branchId, DateTime? beginDate, DateTime? endDate, string folio, string client)
        {
            int? fol = (folio != null && folio != string.Empty)? Convert.ToInt32(folio):new Nullable<int>();

            var model = (from p in db.Budgets.Include(b=> b.BudgetDetails)
                         where
                         (branchId == null || p.BranchId == branchId)
                         && (beginDate == null || p.BudgetDate >= beginDate)
                         && (endDate == null || p.BudgetDate <= endDate)
                         && (folio == null || folio == string.Empty || p.BudgetId == fol)
                         && (client == null || client == string.Empty || p.Client.Name.Contains(client))

                         select p).OrderByDescending(p => p.BudgetDate).ToList();

            return PartialView("_BudgetList",model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Supervisor,Vendedor")]
        public ActionResult ApplyBudget(int budgetId)
        {
            try
            {
                var branchId = User.Identity.GetBranchId();
                var userId = User.Identity.GetUserId();

                var cartItems = GetCart(userId, branchId);

                if (cartItems.Count > Cons.Zero)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Header = "Carrito lleno",
                        Body = "Antes de aplicar una cotización es necesario vaciar el carrito",
                        Code = Cons.Responses.Codes.ConditionMissing
                    });
                }
                else
                {

                    var budget = db.Budgets.Include(b => b.Branch).Include(b=> b.BudgetDetails).
                                 FirstOrDefault(b => b.BudgetId == budgetId);


                    if (budget.DueDate < DateTime.Now.ToLocal())
                    {
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Danger,
                            Header = "Cotizacion Expirada",
                            Body = "Esta cotización expiro en "+budget.DueDate.ToString("dddd, dd MMMM yyyy h:mm tt"),
                            Code = Cons.Responses.Codes.InvalidData
                        });
                    }

                    foreach (var detail in budget.BudgetDetails)
                    {
                        ShoppingCart item = new ShoppingCart
                        {
                            Amount    = detail.Amount,
                            Price     = detail.Price,
                            ProductId = detail.ProductId,
                            BranchId  = branchId,
                            ClientId  = budget.ClientId,
                            BudgetId  = detail.BudgetId,
                            InsDate   = DateTime.Now.ToLocal(),
                            Quantity  = detail.Quantity,
                            TaxAmount = detail.TaxAmount,
                            TaxedAmount = detail.TaxedAmount,
                            TaxedPrice = detail.TaxedPrice,
                            TaxPercentage = detail.TaxPercentage,
                            UserId = userId
                        };

                        db.ShoppingCarts.Add(item);
                    }

                    db.SaveChanges();

                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Success,
                        Header = "Cotizacón aplicada!",
                        Body = "Los productos en la cotización fueron aplicados al carrito",
                        Code  = Cons.Responses.Codes.Success
                    });
                }

            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al aplicar la cotización",
                    Body = "Ocurrio un error inesperado al cargar la cotizacion ",
                    Code = Cons.Responses.Codes.ServerError
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Supervisor,Vendedor")]
        public ActionResult GetBudgeDetail(int budgetId)
        {
            var model = db.BudgetDetails.Include(d => d.Product.Images).
                        Where(d => d.BudgetId == budgetId).ToList();

            return PartialView("_BudgetDetail",model);
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