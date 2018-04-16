using CerberusMultiBranch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CerberusMultiBranch.Support;
using CerberusMultiBranch.Models.Entities.Operative;
using System.Data.Entity;
using System.Security.Principal;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using CerberusMultiBranch.Models.Entities.Finances;
using CerberusMultiBranch.Models.ViewModels.Operative;

namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize(Roles = "Supervisor,Cajero")]
    public class CashRegisterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private CashRegister GetNewCR()
        {
            var cr = new CashRegister();
            cr.OpeningDate = DateTime.Now.ToLocal();
            cr.UserOpen = User.Identity.Name;
            cr.BranchId = User.Identity.GetBranchId();
            return cr;
        }


        public ActionResult History()
        {
            var model = new TransactionViewModel();
            model.CashRegisters = LookForCashReg(null, DateTime.Today, null, null);
            model.Branches = User.Identity.GetBranches().ToSelectList();

            return View(model);
        }

        public ActionResult Detail(int id)
        {
            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);
            var model = db.CashRegisters.Include(cr => cr.CashDetails).
                FirstOrDefault(cr => cr.CashRegisterId == id && brancheIds.Contains(cr.BranchId));

            if (model == null)
                return RedirectToAction("History");

            model.CashDetails.OrderBy(d => d.InsDate);

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate)
        {
            string user = null;
            if (!User.IsInRole("Supervisor"))
                user = User.Identity.GetUserName();

            var model = LookForCashReg(branchId, beginDate, endDate, user);

            return PartialView("_CashRegisterList", model);
        }


        private List<CashRegister> LookForCashReg(int? branchId, DateTime? beginDate, DateTime? endDate, string user)
        {
            var branchIds = User.Identity.GetBranches().Select(b => b.Id);

            var model = db.CashRegisters.Include(cr => cr.CashDetails).Include(cr => cr.Branch).
            Where(cr => (branchId == null && branchIds.Contains(cr.BranchId) || cr.BranchId == branchId) &&
                        (beginDate == null || cr.OpeningDate >= beginDate) &&
                        (endDate == null || cr.OpeningDate <= endDate) &&
                        (user == null || user == string.Empty || cr.UserOpen == user)).ToList();

            return model;

        }

        private CashRegister GetCashRegister()
        {
            var brachId = User.Identity.GetBranchId();


            var cash = db.CashRegisters.Include(cr => cr.CashDetails).OrderByDescending(cr => cr.OpeningDate).
                FirstOrDefault(cr => cr.BranchId == brachId && cr.IsOpen);

            if (cash != null && cash.CashDetails != null)
                cash.CashDetails.OrderBy(d => d.InsDate);
            return cash;
        }

        // GET: CashRegister
        public ActionResult Index()
        {
            var cr = GetCashRegister();

            if (cr == null)
            {
                cr = GetNewCR();
                return View(cr);
            }

            var incomes = cr.Incomes.GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount).ToString("c"), Key = cd.Key }).ToList();
            var cash = cr.Incomes.Where(cd => cd.Type == PaymentMethod.Efectivo).GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount).ToString("c"), Key = cd.Key }).ToList();
            var card = cr.Incomes.Where(cd => cd.Type == PaymentMethod.Tarjeta).GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount).ToString("c"), Key = cd.Key }).ToList();
            var Withdrawals = cr.Withdrawals.GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount).ToString("c"), Key = cd.Key }).ToList();

            var accum = new List<double>();
            var accumN = new List<int>();

            incomes = incomes.OrderBy(c => c.Key).ToList();
            cash = cash.OrderBy(c => c.Key).ToList();
            card = card.OrderBy(c => c.Key).ToList();
            Withdrawals = Withdrawals.OrderBy(c => c.Key).ToList();

            accumN = accumN.OrderBy(d => d).ToList();

            var balance = new Chart(800, 500, theme: ChartTheme.Green);

            balance.AddSeries("Efectivo", chartType: "Column", yValues: cash.Select(i => i.Total).ToList(), xValue: cash.Select(i => i.Key).ToList());
            balance.AddSeries("Tarjeta", chartType: "Column", yValues: card.Select(i => i.Total).ToList(), xValue: card.Select(i => i.Key).ToList());
            balance.AddSeries("Retiros", chartType: "Column", yValues: Withdrawals.Select(i => i.Total).ToList(), xValue: Withdrawals.Select(i => i.Key).ToList());

            balance.AddLegend("Movimiento de caja X Hora");

            var imgS = balance.GetBytes();
            var baseS = Convert.ToBase64String(imgS);
            cr.ChartSource = String.Format("data:image/jpeg;base64,{0}", baseS);

            return View(cr);
        }

        [HttpPost]
        public JsonResult CashRegStatus()
        {
            var cr = GetCashRegister();

            if (cr != null)
                return Json("OK");
            else
                return Json("No se ha abierto el modulo de caja");
        }



        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public JsonResult OpenCashRegister(double initialAmount)
        {
            try
            {
                var cr = GetNewCR();
                cr.IsOpen = true;
                cr.InitialAmount = Math.Round(initialAmount, Cons.Two);

                db.CashRegisters.Add(cr);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json("Error al abrir la caja " + ex.Message);
            }

            return Json("OK");
        }

        [HttpPost]
        public ActionResult GetWithdrawals()
        {
            var cr = GetCashRegister();

            var list = db.CashDetails.Include(w => w.Cause).Where(w => w.CashRegisterId == cr.CashRegisterId && w.DetailType == Cons.Zero).ToList();
            ViewBag.Causes = db.WithdrawalCauses.ToSelectList();
            return PartialView("_Withdrawals", list);
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult AddWithdrawal(double amount, string comment, int causeId)
        {
            var cr = GetCashRegister();

            var wd = new CashDetail
            {
                Amount = amount,
                InsDate = DateTime.Now.ToLocal(),
                User = User.Identity.Name,
                Comment = comment,
                CashRegisterId = cr.CashRegisterId,
                WithdrawalCauseId = causeId,
                DetailType = Cons.Zero,
                Type = PaymentMethod.Efectivo
            };

            db.CashDetails.Add(wd);
            db.SaveChanges();

            return Json("OK");
        }

        [HttpPost]
        public ActionResult BeginClose()
        {
            var cr = GetCashRegister();

            var i = cr.CashDetails.Where(cd => cd.DetailType == Cons.One).Sum(cd => cd.Amount);
            var o = cr.CashDetails.Where(cd => cd.DetailType == Cons.Zero).Sum(cd => cd.Amount);

            var total = cr.InitialAmount + i - o;

            return PartialView("_CloseCash", total);
        }

        [HttpPost]
        public ActionResult Close(double amount, string comment)
        {
            var cr = GetCashRegister();

            cr.FinalAmount = Math.Round(amount, Cons.Two);
            cr.UserClose = User.Identity.Name;
            cr.ClosingDate = DateTime.Now.ToLocal();
            cr.CloseComment = comment;
            cr.IsOpen = false;

            db.Entry(cr).State = EntityState.Modified;
            db.SaveChanges();

            return Json("OK");
        }

        [Authorize(Roles = "Cajero")]
        public ActionResult CheckPending()
        {
            var branchId = User.Identity.GetBranchId();
            var count = db.Sales.Where(s => s.BranchId == branchId && s.Status == TranStatus.Reserved).ToList().Count();
            return Json(new { Result = "OK", Count = count });
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult OpenPending()
        {
            var branchId = User.Identity.GetBranchId();

            var model = db.Sales.Where(s => s.BranchId == branchId && s.Status == TranStatus.Reserved)
                .Include(s => s.User).Include(s => s.Client).ToList();

            return PartialView("_PendingPayment", model);
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult OpenRefunding()
        {
            var branchId = User.Identity.GetBranchId();

            var model = db.Sales.Where(s => s.BranchId == branchId && s.Status == TranStatus.PreCancel)
                .Include(s => s.User).Include(s => s.Client).Include(s => s.SalePayments).ToList();

            return PartialView("_PendingRefundment", model);
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult BeginRefund(int saleId)
        {
            var branchId = User.Identity.GetBranchId();

            var sale = db.Sales.Include(s => s.Client).Include(s => s.SalePayments).
                FirstOrDefault(s => s.SaleId == saleId && s.Status == TranStatus.PreCancel);

            if (sale == null)
                return Json(new { Result = "Error", Message = "Esta venta ya no esta disponible para reembolso" });

            var model = new RefundViewModel
            {
                RefundSaleId = sale.SaleId,
                RefundClient = sale.Client.Name,
                RefundCash = sale.SalePayments.Where(p => p.PaymentMethod == PaymentMethod.Efectivo).Sum(p => p.Amount),
                RefundCredit = sale.SalePayments.Where(p => p.PaymentMethod != PaymentMethod.Efectivo).Sum(p => p.Amount),
                RefundClientId = sale.ClientId,

            };

            return PartialView("_Refundment", model);
        }


        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult CreateRefund(RefundViewModel refund)
        {
            try
            {
                var branchId = User.Identity.GetBranchId();

                var sale = db.Sales.Include(s => s.Client).Include(s => s.SalePayments).
                    FirstOrDefault(s => s.SaleId == refund.RefundSaleId && s.Status == TranStatus.PreCancel);

                if (sale == null)
                    return Json(new { Result = "Error", Message = "Esta venta ya no esta disponible para reembolso" });


                var cr = GetCashRegister();

                var message = "Se ha registrado correctamente el rembolso ";

                if (refund.RefundCash > Cons.Zero)
                {
                    message += "Efectivo devuelto " + refund.RefundCash.ToString("c");
                    var cd = new CashDetail
                    {
                        CashRegisterId = cr.CashRegisterId,
                        Amount = refund.RefundCash,
                        Type = PaymentMethod.Efectivo,
                        Comment = "DEVOLUCIÓN POR CANCELACIÓN " + sale.Folio,
                        InsDate = DateTime.Now.ToLocal(),
                        User = User.Identity.Name,
                        DetailType = Cons.Zero,
                        SaleFolio = sale.Folio,
                        WithdrawalCauseId = 3
                    };

                    db.CashDetails.Add(cd);
                }

                var noteFolio = string.Empty;

                if (refund.RefundCredit > Cons.Zero)
                {
                    var year = Convert.ToInt32(DateTime.Now.ToLocal().ToString("yy"));
                    var last = db.SaleCreditNotes.Where(n => n.Year == year).OrderByDescending(n => n.Sequential).FirstOrDefault();

                    var seq = last == null ? Cons.One : (last.Sequential + Cons.One);

                    var nc = new SaleCreditNote
                    {
                        SaleCreditNoteId = sale.SaleId,
                        Amount = refund.RefundCredit,
                        ExplirationDate = DateTime.Now.ToLocal().AddDays(Cons.DaysToCancel),
                        RegisterDate = DateTime.Now.ToLocal(),
                        IsActive = true,
                        User = User.Identity.Name,
                        Year = year,
                        Sequential = seq,
                        Folio = DateTime.Now.ToLocal().ToString("yy") + "-" + seq.ToString(Cons.CodeSeqFormat)
                    };

                    message += "Cantidad en Vale " + refund.RefundCredit.ToString("c") + " Folio del vale " + nc.Folio;

                    db.SaleCreditNotes.Add(nc);
                }

                sale.LastStatus = sale.Status;
                sale.Status = TranStatus.Canceled;
                sale.UpdUser = User.Identity.Name;
                sale.UpdDate = DateTime.Now.ToLocal();

                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Result = "OK", Message = message });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = "Ocurrio un error al generar al devolución, detalle:" + ex.Message });
            }

        }


        [HttpPost]
        public ActionResult TicketsAndNotes()
        {
            var model = new TransactionViewModel();
            model.Sales = LookForNotes(DateTime.Today, null, null, null);
            return PartialView("_TicketsAndNotes", model);
        }

        [HttpPost]
        public ActionResult SearchNotes(DateTime? begin, DateTime? end, string folio, string client)
        {
            var sales = LookForNotes(begin, end, folio, client);
            return PartialView("_SalesToPayList", sales);
        }

        private List<Sale> LookForNotes(DateTime? begin, DateTime? end, string folio, string client)
        {
            var branchId = User.Identity.GetBranchId();

            //busco las ventas de la sucursal que esten reservadas o completadas (ya pagadas)
            var sales = db.Sales.Include(s => s.Client).Include(s => s.SaleDetails).Include(s => s.SalePayments).Include(s => s.User).
                Where(s => s.BranchId == branchId &&
                (begin == null || s.TransactionDate >= begin) &&
                (end == null || s.TransactionDate <= end) &&
                (folio == null || folio == string.Empty || s.Folio == folio) &&
                (client == null || client == string.Empty || s.Client.Name.Contains(client)) &&
                (s.Status == TranStatus.Compleated || s.Status == TranStatus.Revision)).
                OrderByDescending(s => s.TransactionDate).ToList();

            return sales;
        }


        public ActionResult PrintNote(int id)
        {
            var branchId = User.Identity.GetBranchId();
            // busco la venta con el id provisto validando que ya tenga status compleated (pagada)
            var sale = db.Sales.Include(s => s.SaleDetails).Include(s => s.Client).Include(s => s.User).
             Include(s => s.SaleDetails.Select(td => td.Product)).
             Include(s => s.SaleDetails.Select(td => td.Product.Images)).
             Include(s => s.SaleDetails.Select(td => td.Product.BranchProducts)).
             Include(s => s.Branch).
             FirstOrDefault(s => s.SaleId == id && s.Status == TranStatus.Compleated && s.BranchId == branchId);

            if (sale == null)
                return RedirectToAction("Index");

            sale.SaleDetails = sale.SaleDetails.OrderBy(td => td.SortOrder).ToList();

            return View(sale);
        }


        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult BeginRegistPayment(int id)
        {
            var branchId = User.Identity.GetBranchId();

            //obtengo los detalles de la venta, incluyendo imagenes y pagos
            var details = db.SaleDetails.Include(sd => sd.Sale.SalePayments).Include(sd => sd.Product.Images).
                       Where(sd => sd.SaleId == id && (sd.Sale.Status == TranStatus.Reserved || sd.Sale.Status == TranStatus.Revision)
                       && sd.Sale.BranchId == branchId).ToList();

            //si no encuentro detalles de venta, envío un error
            if (details == null || details.Count == Cons.Zero)
                return Json(new { Result = "Error", Message = "no se encontro la venta para cobrar" });

            //construyo el modelo para recibir el pago
            var model = new ChoosePaymentViewModel
            {
                Details = details,
                AmountToPay = details.Sum(d => d.TaxedAmount) - details.First().Sale.SalePayments.Sum(sp => sp.Amount),
                PaymentMethod = PaymentMethod.Efectivo,
                SaleId = id
            };

            //si la venta es a crédito establesco el moto en efectivo como el total de la venta
            if (details.First().Sale.TransactionType == TransactionType.Contado)
                model.CashAmount = details.Sum(d => d.TaxedAmount);

            if (details.First().Sale.TransactionType == TransactionType.Preventa)
                model.CashAmount = (details.Sum(d => d.TaxedAmount) / Cons.Two).RoundMoney();

            return PartialView("_RegistPayment", model);
        }


        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult RegistPayment(int saleId, double cash, double card, string reference, int printType)
        {
            try
            {
                //busco la venta a pagar
                var sale = db.Sales.Where(s => s.SaleId == saleId).Include(s => s.SalePayments).
                            Include(s => s.SaleDetails).Include(s => s.Client).Include(s => s.User).
                            Include(s => s.Client.City).Include(s => s.Client.City.State).
                            Include(s => s.SaleDetails.Select(td => td.Product)).
                            Include(s => s.SaleDetails.Select(td => td.Product.Images)).
                            Include(s => s.SaleDetails.Select(td => td.Product.BranchProducts)).
                            Include(s => s.Branch).FirstOrDefault();

                var totalPaymets = sale.SalePayments != null ? sale.SalePayments.Sum(s => s.Amount) : Cons.Zero;

                totalPaymets += (cash + card);

                #region Validaciones
                //si la venta ya fue cobrada en su totalidad, envio error
                if (sale.Status == TranStatus.Compleated)
                    return Json(new { Result = "Error", Message = "Esta venta ya ha sido cobrada en su totalidad" });

                if (totalPaymets > sale.TotalTaxedAmount)
                    return Json(new { Result = "Error", Message = "El monto ingresado excede la cantidad total de la deuda, por favor verifique" });


                //si la venta no es de CONTADO y esta en status revision, envio error
                if (sale.TransactionType != TransactionType.Contado && sale.TotalTaxedAmount < totalPaymets)
                    return Json(new { Result = "Error", Message = "El monto a pagar excede la cantidad de la deuda" });

                if (sale.TransactionType == TransactionType.Contado && sale.TotalTaxedAmount != totalPaymets)
                    return Json(new { Result = "Error", Message = "El monto del pago no coincide con el total de la venta" });

                //si la venta no es de CONTADO y esta en status revision, envio error
                if (sale.TransactionType != TransactionType.Preventa && sale.Status == TranStatus.Reserved
                    && (sale.TotalTaxedAmount / 0.1) < totalPaymets)
                    return Json(new { Result = "Error", Message = "La preventa requiere por lo menos un 10% de anticipo" });
                #endregion

                //ordeno por el campo Sort Order
                sale.SaleDetails = sale.SaleDetails.OrderBy(td => td.SortOrder).ToList();

                sale.UpdDate = DateTime.Now.ToLocal();
                sale.UpdUser = User.Identity.Name;
                sale.SalePayments = new List<SalePayment>();

                #region Registro de pagos
                //si viene un monto en efectivo, agrego el registro de pago
                if (cash > Cons.Zero)
                {
                    var p = new SalePayment
                    {
                        SaleId = sale.SaleId,
                        Amount = cash,
                        PaymentDate = DateTime.Now.ToLocal(),
                        PaymentMethod = PaymentMethod.Efectivo,
                        UpdDate = DateTime.Now.ToLocal(),
                        UpdUser = User.Identity.Name,
                        Comment = "COBRO AUTOMATICO EN CAJA"
                    };

                    sale.SalePayments.Add(p);
                }
                //si viene un monto de tarjeta, agrego el registro de pago
                if (card > Cons.Zero)
                {
                    var p = new SalePayment
                    {
                        SaleId = sale.SaleId,
                        Amount = card,
                        PaymentDate = DateTime.Now.ToLocal(),
                        PaymentMethod = PaymentMethod.Tarjeta,
                        UpdDate = DateTime.Now.ToLocal(),
                        UpdUser = User.Identity.Name,
                        Comment = "COBRO AUTOMATICO EN CAJA",
                        Reference = reference
                    };

                    sale.SalePayments.Add(p);
                }

                #endregion


                //marco la venta con el status correspondiente
                sale.LastStatus = sale.Status;

                if (sale.TotalTaxedAmount == totalPaymets)
                    sale.Status = TranStatus.Compleated;
                else
                    sale.Status = TranStatus.Revision;

                if (sale.TransactionType == TransactionType.Credito)
                    sale.Client.UsedAmount -= totalPaymets;

                //hago attach a la venta.. esto permite agregar los registros de pago a la base de datos
                db.Sales.Attach(sale);
                db.Entry(sale).State = EntityState.Modified;

                #region registro de Ingreso a caja
                //obtengo el registro de caja activo
                var cr = GetCashRegister();

                if (cr == null)
                {
                    return Json(new
                    {
                        Result = "Error al registrar el pago!",
                        Message = "El modulo de caja no ha sido abierto"
                    });
                }
                //por cada registro de pago agrego una registro de entrada a la caja
                foreach (var pay in sale.SalePayments)
                {
                    var dt = new CashDetail();
                    dt.CashRegisterId = cr.CashRegisterId;
                    dt.Amount = pay.Amount;
                    dt.InsDate = DateTime.Now.ToLocal();
                    dt.User = User.Identity.Name;
                    dt.Type = pay.PaymentMethod;
                    dt.SaleFolio = sale.Folio;
                    dt.DetailType = Cons.One;
                    dt.Comment = "COBRO EN CAJA";

                    db.CashDetails.Add(dt);
                }
                #endregion

                db.SaveChanges();

                if (printType == Cons.One)
                    return PartialView("_PrintTicket", sale);
                else
                    return PartialView("_PrintNote", sale);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error al registrar el pago!",
                    Message = "Detalle de la excepción " + ex.Message + " "
                    + ex.InnerException != null ? ex.InnerException.Message : string.Empty
                });
            }
        }

        [HttpPost]
        public ActionResult PrintDocument(int saleId, int printType)
        {
            var sale = db.Sales.Where(s => s.SaleId == saleId).Include(s => s.SalePayments).
                           Include(s => s.SaleDetails).Include(s => s.Client).Include(s => s.User).
                           Include(s => s.SaleDetails.Select(td => td.Product)).
                           Include(s => s.SaleDetails.Select(td => td.Product.Images)).
                           Include(s => s.SaleDetails.Select(td => td.Product.BranchProducts)).
                           Include(s => s.Branch).FirstOrDefault();

            sale.SaleDetails = sale.SaleDetails.OrderBy(td => td.SortOrder).ToList();


            if (printType == Cons.One)
                return PartialView("_PrintTicket", sale);
            else
                return PartialView("_PrintNote", sale);
        }
    }
}