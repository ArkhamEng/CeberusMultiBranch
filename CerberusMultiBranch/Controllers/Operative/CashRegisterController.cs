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
    [CustomAuthorize(Roles = "Supervisor,Cajero")]
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

            var incomes = cr.Incomes.GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount).ToMoney(), Key = cd.Key }).ToList();
            var cash = cr.Incomes.Where(cd => cd.Type == PaymentMethod.Efectivo).GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount).ToMoney(), Key = cd.Key }).ToList();
            var card = cr.Incomes.Where(cd => cd.Type == PaymentMethod.Tarjeta).GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount).ToMoney(), Key = cd.Key }).ToList();
            var Withdrawals = cr.Withdrawals.GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount).ToMoney(), Key = cd.Key }).ToList();

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
        [CustomAuthorize(Roles = "Cajero")]
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
            try
            {
                var cr = GetCashRegister();

                var list = db.CashDetails.Include(w => w.Cause).
                            Where(w => w.CashRegisterId == cr.CashRegisterId && w.DetailType == Cons.Zero).ToList();

                ViewBag.Causes = db.WithdrawalCauses.Where(c => c.WithdrawalCauseId != Cons.RefundId).ToSelectList();

                return PartialView("_Withdrawals", list);

            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al obtener los retiros de caja",
                    Header = "Error General"
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Cajero")]
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
            try
            {
                var cr = GetCashRegister();

                var i = cr.CashDetails.Where(cd => cd.DetailType == Cons.One).Sum(cd => cd.Amount);
                var o = cr.CashDetails.Where(cd => cd.DetailType == Cons.Zero).Sum(cd => cd.Amount);

                var total = 0.0;


                if (i > o)
                    total = cr.InitialAmount + (i - o);
                else
                    total = cr.InitialAmount + (o - i);

                return PartialView("_CloseCash", total);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al obtener el cierre de caja",
                    Header = "Errro al obtener datos"
                });
            }
        }

        [HttpPost]
        public ActionResult Close(double amount, string comment)
        {
            try
            {
                var cr = GetCashRegister();

                var branchId = User.Identity.GetBranchId();


                var salesCount = db.Sales.Where(s => s.BranchId == branchId && s.Status == TranStatus.Reserved).Count();

                if (salesCount > Cons.Zero)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.InvalidData,
                        Body = "Aun tienes " + salesCount + " venta(s) pendiente(s) de cobro, no puedes dejar pendientes al cerrar caja",
                        Header = "Corte no permitido"
                    });
                }

                var refunding = db.Sales.Where(s => s.BranchId == branchId && s.Status == TranStatus.PreCancel).Count();


                if (refunding > Cons.Zero)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.InvalidData,
                        Body = "Aun tienes " + refunding + "reembolsos(s) pendiente(s) de aplicar, no puedes dejar pendientes al cerrar caja",
                        Header = "Corte no permitido"
                    });
                }


                var i = cr.CashDetails.Where(cd => cd.DetailType == Cons.One).Sum(cd => cd.Amount);
                var o = cr.CashDetails.Where(cd => cd.DetailType == Cons.Zero).Sum(cd => cd.Amount);
                var total = 0.0;

                if (!cr.IsOpen)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.RecordNotFound,
                        Body = "Esta sesión de caja ya no se encuentra activa",
                        Header = "Caja cerrada"
                    });
                }

                if (i > o)
                    total = cr.InitialAmount + (i - o);
                else
                    total = cr.InitialAmount + (o - i);

                cr.FinalAmount = total.RoundMoney();
                cr.UserClose = User.Identity.Name;
                cr.ClosingDate = DateTime.Now.ToLocal();
                cr.CloseComment = comment;
                cr.IsOpen = false;

                db.Entry(cr).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success,
                    Body = "El corete de caja se efectuo exitosamente",
                    Header = "Corte realizado"
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = ex.Message,
                    Header = "Errro al obtener datos"
                });
            }
        }

        [Authorize(Roles = "Cajero")]
        public ActionResult CheckPending()
        {
            var branchId = User.Identity.GetBranchId();
            var count = db.Sales.Where(s => s.BranchId == branchId && s.Status == TranStatus.Reserved).ToList().Count();
            return Json(new { Result = "OK", Count = count });
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Cajero")]
        public ActionResult OpenPending()
        {
            try
            {
                var branchId = User.Identity.GetBranchId();

                var model = db.Sales.Where(s => s.BranchId == branchId &&
                        (s.Status == TranStatus.Reserved) || (s.Status == TranStatus.Modified))
                    .Include(s => s.User).Include(s => s.Client).ToList();

                return PartialView("_PendingPayment", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al obtener las ventas pendientes de cobro",
                    Header = "Errro al obtener datos"
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Cajero")]
        public ActionResult OpenRefunding()
        {
            try
            {
                var branchId = User.Identity.GetBranchId();

                var model = db.Sales.Where(s => s.BranchId == branchId && s.Status == TranStatus.PreCancel)
                    .Include(s => s.User).Include(s => s.Client).Include(s => s.SalePayments).ToList();

                return PartialView("_PendingRefundment", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al obtener las las cancelaciones pendientes",
                    Header = "Errro al obtener datos"
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Cajero")]
        public ActionResult BeginRefund(int id)
        {
            try
            {
                var branchId = User.Identity.GetBranchId();

                var sale = db.Sales.Include(s => s.Client).Include(s => s.SalePayments).
                    FirstOrDefault(s => s.SaleId == id && s.Status == TranStatus.PreCancel);

                if (sale == null)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.RecordNotFound,
                        Body = "Esta venta ya no esta disponible para reembolso",
                        Header = "No se encontro registro"
                    });
                }


                var model = new RefundViewModel
                {
                    RefundSaleId = sale.SaleId,
                    RefundClient = sale.Client.Name,
                    RefundCash = sale.SalePayments.Where(p => p.PaymentMethod == PaymentMethod.Efectivo).Sum(p => p.Amount).RoundMoney(),
                    RefundCredit = sale.SalePayments.Where(p => p.PaymentMethod != PaymentMethod.Efectivo).Sum(p => p.Amount).RoundMoney(),
                    RefundClientId = sale.ClientId
                };

                return PartialView("_Refundment", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al obtener la ´venta para reembolso",
                    Header = "Errro al obtener datos"
                });
            }
        }

        [HttpPost]
        public ActionResult AutoCompleate(string filter)
        {

            var model = db.SaleCreditNotes.Where(p => p.Ident.Contains(filter) || p.Folio.Contains(filter) && p.IsActive).Take(20).
                Select(p =>
                    new { Id = p.SaleCreditNoteId, Label = p.Folio, Value = "IFE " + p.Ident, Response = Cons.Responses.Success, Code = Cons.Responses.Codes.Success });

            return Json(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Cajero")]
        public ActionResult CreateRefund(RefundViewModel refund)
        {
            try
            {
                var printRefund = new PrintRefundViewModel();

                var branchId = User.Identity.GetBranchId();

                var sale = db.Sales.Include(s => s.Client).Include(s => s.SalePayments).
                    FirstOrDefault(s => s.SaleId == refund.RefundSaleId && s.Status == TranStatus.PreCancel);

                if (sale == null)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.RecordNotFound,
                        Body = "Esta venta ya no esta disponible para reembolso",
                        Header = "Registro no disponible"
                    });
                }


                printRefund.Branch = sale.Branch;
                printRefund.Client = refund.ReceivedBy;
                printRefund.Cash = refund.RefundCash.ToMoney();
                printRefund.Note = refund.RefundCredit.ToMoney();
                printRefund.Total = (refund.RefundCash + refund.RefundCredit).toText();
                printRefund.Folio = sale.Folio;
                printRefund.Comment = sale.Comment;
                printRefund.Date = DateTime.Now.ToLocal().ToString("dd/MM/yyyy hh:mm");


                var cr = GetCashRegister();

                var message = "El rembolso se aplico correctamente.";

                if (refund.RefundCash > Cons.Zero)
                {
                    message += " Efectivo devuelto " + refund.RefundCash.ToMoney();
                    var cd = new CashDetail
                    {
                        CashRegisterId = cr.CashRegisterId,
                        Amount = refund.RefundCash,
                        Type = PaymentMethod.Efectivo,
                        Comment = string.Format("DEVOLUCIÓN POR CANCELACIÓN {0}! RECIBIDA POR {1}", sale.Folio, refund.ReceivedBy.ToUpper()),
                        InsDate = DateTime.Now.ToLocal(),
                        User = User.Identity.Name,
                        DetailType = Cons.Zero,
                        SaleFolio = sale.Folio,
                        WithdrawalCauseId = Cons.RefundId
                    };

                    db.CashDetails.Add(cd);
                }
                SaleCreditNote nc = null;

                if (refund.RefundCredit > Cons.Zero)
                {
                    var year = Convert.ToInt32(DateTime.Now.ToLocal().ToString("yy"));
                    var last = db.SaleCreditNotes.Where(n => n.Year == year).OrderByDescending(n => n.Sequential).FirstOrDefault();

                    var seq = last == null ? Cons.One : (last.Sequential + Cons.One);

                    nc = new SaleCreditNote
                    {
                        SaleCreditNoteId = sale.SaleId,
                        Amount = refund.RefundCredit,
                        ExplirationDate = DateTime.Now.ToLocal().AddDays(Cons.DaysToCancel),
                        RegisterDate = DateTime.Now.ToLocal(),
                        IsActive = true,
                        User = User.Identity.Name,
                        Year = year,
                        Sequential = seq,
                        Ident = refund.Ident,
                        Folio = DateTime.Now.ToLocal().ToString("yy") + "-" + seq.ToString(Cons.CodeSeqFormat)
                    };

                    message += " Monto en Vale " + refund.RefundCredit.ToMoney() + " Folio del vale " + nc.Folio;
                    printRefund.HasNote = true;

                    db.SaleCreditNotes.Add(nc);
                }

                var history = new SaleHistory
                {
                    SaleId = sale.SaleId,
                    Comment = sale.Comment,
                    InsDate = sale.UpdDate,
                    User = sale.UpdUser,
                    Status = sale.Status.GetName(),
                    TotalDue = sale.FinalAmount
                };

                //solo cambio el status sin registrar, el usuario que lo cambia 
                //para que quede registrado, el usuario que realiza la cancelación
                sale.LastStatus = sale.Status;
                sale.Status = TranStatus.Canceled;
                sale.UpdUser = User.Identity.Name;
                sale.UpdDate = DateTime.Now.ToLocal();

                db.SaleHistories.Add(history);
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();

                //obtengo la nota de crédito a imprimir

                if (nc != null)
                {
                    printRefund.CreditNote = db.SaleCreditNotes.Include(n => n.Sale).Include(n => n.Sale.Branch).
                    FirstOrDefault(n => n.SaleCreditNoteId == nc.SaleCreditNoteId && n.Folio == nc.Folio);
                }

                return PartialView("_PrintRefund", printRefund);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al obtener aplicar el rembolso",
                    Header = "Errro al interno"
                });
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Cajero")]
        public ActionResult PrintCreditNote(int noteId)
        {
            var model = db.SaleCreditNotes.Include(n => n.Sale).Include(n => n.Sale.Branch).
                FirstOrDefault(n => n.SaleCreditNoteId == noteId);
            if (model == null)
                return Json(new { Result = "Error", Message = "No se encontro la nota de crédito" });
            else
                return PartialView("_PrintCreditNote", model);
        }


        [HttpPost]
        public ActionResult TicketsAndNotes()
        {
            try
            {
                var model = new TransactionViewModel();
                model.Sales = LookForNotes(DateTime.Today, null, null, null, null);
                return PartialView("_TicketsAndNotes", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al Obtener los datos de ticket y notas",
                    Header = "Error General"
                });
            }
        }

        [HttpPost]
        public ActionResult SearchNotes(DateTime? begin, DateTime? end, string folio, string client, TranStatus? status)
        {
            try
            {
                if (end != null)
                    end = end.Value.AddHours(23).AddMinutes(59);

                if (begin != null && end != null && (begin > end))
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.InvalidData,
                        Body = "La fecha final debe ser mayor ó igual que la fecha de inicio",
                        Header = "Fechas invalidas"
                    });
                }


                var sales = LookForNotes(begin, end, folio, client, status);
                return PartialView("_SalesToPayList", sales);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al Obtener los datos de ticket y notas",
                    Header = "Error General"
                });
            }

        }

        private List<Sale> LookForNotes(DateTime? begin, DateTime? end, string folio, string client, TranStatus? status)
        {
            var branchId = User.Identity.GetBranchId();

            //busco las ventas de la sucursal que esten reservadas o completadas (ya pagadas)
            var sales = db.Sales.Include(s => s.Client).Include(s => s.SaleDetails).Include(s => s.SalePayments).Include(s => s.User).
                Where(s => s.BranchId == branchId &&
                (begin == null || s.TransactionDate >= begin) &&
                (end == null || s.TransactionDate <= end) &&
                (string.IsNullOrEmpty(folio) || s.Folio.Contains(folio)) &&
                (client == null || client == string.Empty || s.Client.Name.Contains(client)) &&
                (status == null && (s.Status == TranStatus.Compleated || s.Status == TranStatus.Revision) || s.Status == status)).
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


        [HttpGet]
        [CustomAuthorize(Roles = "Cajero")]
        public ActionResult BeginRegistPayment(int id)
        {
            try
            {
                var branchId = User.Identity.GetBranchId();

                var sale = db.Sales.Include(s => s.User).Include(sd => sd.SaleDetails.Select(d => d.Product.Images)).
                    Include(s => s.SalePayments).FirstOrDefault(s => s.SaleId == id &&
                    (s.Status == TranStatus.Reserved || s.Status == TranStatus.Revision || s.Status == TranStatus.Modified));


                //si no encuentro detalles de venta, envío un error
                if (sale == null)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.RecordNotFound,
                        Body = "no se encontro la venta para cobrar",
                        Header = "Registro no encontrado"
                    }, JsonRequestBehavior.AllowGet);
                }



                //construyo el modelo para recibir el pago
                var model = new ChoosePaymentViewModel
                {
                    Details = sale.SaleDetails,
                    AmountToPay = Math.Round(sale.FinalAmount - sale.SalePayments.Sum(sp => sp.Amount), Cons.Two),
                    PaymentMethod = PaymentMethod.Efectivo,
                    SaleId = id,
                    User = sale.User.UserName.ToUpper(),
                    Delivery = sale.SendingType.GetName().ToUpper(),
                    Client = sale.Client.Name.ToUpper()
                };

                //si la venta es a crédito establesco el moto en efectivo como el total de la venta
                if (sale.TransactionType == TransactionType.Cash)
                    model.CashAmount = sale.FinalAmount;

                if (sale.TransactionType == TransactionType.Presale)
                    model.CashAmount = (sale.FinalAmount / Cons.Two).RoundMoney();

                if (sale.TransactionType == TransactionType.Reservation)
                    model.CashAmount = (sale.FinalAmount * 0.1).RoundMoney();

                return PartialView("_RegistPayment", model);
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = ex.Message,
                    Header = "Errro al obtener datos"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [CustomAuthorize(Roles = "Cajero")]
        public ActionResult GetFolioAmount(int id, string folio)
        {
            var note = db.SaleCreditNotes.Find(id, folio);

            if (note != null && note.IsActive)
                return Json(new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success,
                    Extra = note.Amount.ToString(),
                    Body = note.ExplirationDate.ToString("dd/MM/yyyy"),
                    Header = "Folio " + note.Folio + " IFE " + note.Ident,
                    JProperty = note.Folio
                });
            else
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Warning,
                    Body = "No se encontro una nota de creidito activa",
                    Code = Cons.Responses.Codes.RecordNotFound,
                    Header = "No se encontro el registro"
                });
            }

        }



        [HttpPost]
        [CustomAuthorize(Roles = "Cajero")]
        public ActionResult RegistPayment(ChoosePaymentViewModel payment)
        {
            try
            {   //busco la venta a pagar
                var sale = db.Sales.Where(s => s.SaleId == payment.SaleId).Include(s => s.SalePayments).Include(s => s.SaleHistories).
                            Include(s => s.SaleDetails).Include(s => s.Client.Addresses).Include(s => s.User).
                            Include(s => s.SaleDetails.Select(td => td.Product.Images)).
                            Include(s => s.SaleDetails.Select(td => td.Product.BranchProducts)).
                            Include(s => s.Branch).FirstOrDefault();

                //valido que la venta este en un status valido para recibir pago o hacer devolución
                if (sale.Status == TranStatus.PreCancel || sale.Status == TranStatus.Canceled)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.InvalidData,
                        Body = "Esta venta se encuentra cancelada o en cancelación y no requiere pago",
                        Header = "Venta cancelada"
                    });
                }

                if (sale.Status == TranStatus.InProcess || sale.Status == TranStatus.OnChange)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.InvalidData,
                        Body = "Esta venta esta siendo modificada, no puede recibir pagos hasta que la edición concluya",

                        Header = "Venta en modificación"
                    });
                }


                //creo el historico de la venta por si se modifica
                var SaleHistory = new SaleHistory
                {
                    Comment = sale.Comment,
                    InsDate = sale.UpdDate,
                    User = sale.UpdUser,
                    SaleId = sale.SaleId,
                    Status = sale.Status.GetName(),
                    TotalDue = sale.FinalAmount
                };

                //valido si estoy haciendo devolución
                var refund = payment.RefundCash + payment.RefundCredit;
                var isRefund = refund > Cons.Zero;

                SaleCreditNote note = null;

                if (isRefund)
                {
                    var response = ApplyRefund(sale, payment, ref note);

                    if (response.Code != Cons.Responses.Codes.Success)
                        return Json(response);
                }

                // monto del pago en dinero (tarjeta o efectivo)
                var money = payment.CashAmount + payment.CardAmount;
                var wholePayment = payment.CreditNoteAmount + money;
                var isPayment = wholePayment > Cons.Zero;

                //pagos actuales
                var cPayments = sale.SalePayments != null ? sale.SalePayments.Sum(s => s.Amount) : Cons.Zero;
                //monto del adeudo de la venta
                var debtAmount = Math.Round(sale.FinalAmount - cPayments, Cons.Two);

                //hago validaciones con respecto a si se hace o no se hace pago
                var valResponse = ValidatePayment(sale, payment, debtAmount, wholePayment);

                //si la respueste es diferente de null, algo falló y salgo
                if (valResponse != null)
                    return Json(valResponse);

                //si se esta aplicando un pago
                if (isPayment)
                {
                    var response = ApplyPayment(sale, payment, note, debtAmount, cPayments);

                    if (response.Code != Cons.Responses.Codes.Success)
                        return Json(response);
                }

                //si la venta no lleva pago ni abono solo se regresa a su estado anterior
                if (!isPayment && !isRefund)
                    sale.Status = sale.LastStatus;

                //ordeno por el campo Sort Order
                sale.SaleDetails = sale.SaleDetails.OrderBy(td => td.SortOrder).ToList();

                sale.SalePayments = sale.SalePayments ?? new List<SalePayment>();

                db.SaveChanges();

                PrintableViewModel model = new PrintableViewModel();


                //evito la impresion de detalles en cantidad 0
                var noPrintDetails = sale.SaleDetails.Where(d => d.DueQuantity == Cons.Zero).ToList();

                noPrintDetails.ForEach(d =>
                {
                    sale.SaleDetails.Remove(d);
                });

                if (isRefund)
                {
                    var printRefund = new PrintRefundViewModel
                    {
                        Cash = payment.RefundCash.ToMoney(),
                        Comment = "Devolucíon por modificación de venta " + sale.Folio,
                        Client = payment.ReceivedBy,
                        HasNote = (note != null),
                        Date = DateTime.Now.ToLocal().ToString("dd/MM/yyyy hh:mm"),
                        Folio = note.Folio,
                        Note = note.Amount.ToMoney(),
                        User = User.Identity.Name,
                        Total = (payment.RefundCash + payment.RefundCredit).ToText(),
                        Branch = sale.Branch,
                        CreditNote = note ?? note,
                    };

                    model.PrintRefund = printRefund;
                }
                else
                    model.SaleCreditNote = note != null && note.IsActive ? note : null;

                model.PrintType = payment.PrintType;
                model.Sale = sale;

                return PartialView("_PrintElements", model);
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "detale :" + ex.Message,
                    Header = "Error al cobrar!"
                });
            }
        }

        private JResponse ApplyPayment(Sale sale, ChoosePaymentViewModel payment, SaleCreditNote note, 
            double debtAmount,double cPayments)
        {
            try
            {
                var money = payment.CashAmount + payment.CardAmount;
                var wholePayment = payment.CreditNoteAmount + money;

                //determino si se usa nota de crédito en el pago
                var usingCreditNote = (payment.CreditNoteAmount > Cons.Zero);

              
                //creo el historico
                var history = new SaleHistory
                {
                    Comment = sale.Comment,
                    InsDate = sale.UpdDate,
                    User = sale.UpdUser,
                    SaleId = sale.SaleId,
                    Status = sale.Status.GetName(),
                    TotalDue = sale.FinalAmount
                };

                var cashRegister = GetCashRegister();

                var totalPayments = 0.00;
                var comment = sale.TransactionType == TransactionType.Cash ? "Pago de venta " : "Abono de venta ";

                var payments = new List<SalePayment>();

                //si viene un monto en efectivo, agrego el registro de pago a la venta
                if (payment.CashAmount > Cons.Zero)
                {
                    totalPayments += payment.CashAmount;
                    var p = new SalePayment
                    {
                        SaleId = sale.SaleId,
                        Amount = payment.CashAmount,
                        PaymentDate = DateTime.Now.ToLocal(),
                        PaymentMethod = PaymentMethod.Efectivo,
                        UpdDate = DateTime.Now.ToLocal(),
                        UpdUser = User.Identity.Name,
                        Comment = comment + sale.Folio
                    };

                    payments.Add(p);
                }
                //si viene un monto de tarjeta, agrego el registro de pago
                if (payment.CardAmount > Cons.Zero)
                {
                    totalPayments += payment.CardAmount;
                    var p = new SalePayment
                    {
                        SaleId = sale.SaleId,
                        Amount = payment.CardAmount,
                        PaymentDate = DateTime.Now.ToLocal(),
                        PaymentMethod = PaymentMethod.Tarjeta,
                        UpdDate = DateTime.Now.ToLocal(),
                        UpdUser = User.Identity.Name,
                        Comment = comment + sale.Folio,
                        Reference = payment.Reference
                    };

                    payments.Add(p);
                }

                var folioChanged = false;
                //si se esta aplicando un folio
                if (payment.CreditNoteAmount > Cons.Zero)
                {
                    totalPayments += payment.CreditNoteAmount;

                    note = db.SaleCreditNotes.Find(payment.CreditNoteId, payment.CreditNoteFolio);

                    if (Math.Round(payment.CreditNoteAmount, Cons.Two) > Math.Round(note.Amount, Cons.Two))
                    {
                        return new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Code = Cons.Responses.Codes.InvalidData,
                            Body = "El monto en vale excede el disponible",
                            Header = "Vale sin fondos"
                        };
                    }

                    if (note.ExplirationDate < DateTime.Now.ToLocal().AddDays(Cons.One))
                    {
                        return new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Code = Cons.Responses.Codes.InvalidData,
                            Body = "El vale cadudo en la fecha " + note.ExplirationDate.ToString("dd/MM/yyyy"),
                            Header = "Vale sin caducado"
                        };
                    }

                    var p = new SalePayment
                    {
                        SaleId = sale.SaleId,
                        Amount = (folioChanged ? debtAmount : payment.CreditNoteAmount),
                        PaymentDate = DateTime.Now.ToLocal(),
                        PaymentMethod = PaymentMethod.Vale,
                        UpdDate = DateTime.Now.ToLocal(),
                        UpdUser = User.Identity.Name,
                        Comment = "Abono a venta Folio: " + sale.Folio.ToUpper(),
                        Reference = "vale:" + note.Folio
                    };

                    //creo el historico del vale
                    CreditNoteHistory noteHistory = new CreditNoteHistory
                    {
                        Amount = note.Amount,
                        ChangeDate = DateTime.Now.ToLocal(),
                        User = User.Identity.Name,
                        SaleCreditNoteId = note.SaleCreditNoteId,
                        Folio = note.Folio,
                    };

                    //si el vale se queda con menos de un peso, ya no lo imprimo
                    note.Amount -= payment.CreditNoteAmount;
                    note.IsActive = (Math.Round(note.Amount, Cons.Two) >= Cons.One);

                    db.CreditNoteHistories.Add(noteHistory);
                    db.Entry(note).State = EntityState.Modified;
                    payments.Add(p);
                }

                payments.ForEach(p =>
                {
                    var dt = new CashDetail
                    {
                        CashRegisterId = cashRegister.CashRegisterId,
                        Amount = p.Amount,
                        InsDate = DateTime.Now.ToLocal(),
                        User = User.Identity.Name,
                        Type = p.PaymentMethod,
                        SaleFolio = sale.Folio,
                        DetailType = Cons.One,
                        Comment = p.Comment,
                    };

                    sale.SalePayments.Add(p);
                    db.CashDetails.Add(dt);
                });

                //si se paga completamente
                if (Math.Round(sale.FinalAmount, Cons.Two) == Math.Round(cPayments + wholePayment, Cons.Two))
                {
                    sale.LastStatus = sale.Status;
                    sale.Status = TranStatus.Compleated;
                    sale.Comment = "Venta cobrada totalmente";
                    db.SaleHistories.Add(history);
                }
                //si queda adeudo y es preventa
                else 
                {  //si la venta queda con adeudo la pongo en seguimiento 
                  
                    if(sale.Status == TranStatus.Revision)
                        sale.Comment = "Abono recibido";
                    else
                    {  //agrego el historial solo en cambios de estado
                        sale.Status  = TranStatus.Revision;
                        sale.Comment = "Abono recibido";

                        db.SaleHistories.Add(history);
                    }
                }

                //si la venta es a crédito, se descuenta el monto abonado de la cuenta del cliente
                if (sale.TransactionType == TransactionType.Credit && wholePayment > Cons.Zero)
                    sale.Client.UsedAmount -= wholePayment;

                sale.SaleHistories.Add(history);
                db.Sales.Attach(sale);
                db.Entry(sale).State = EntityState.Modified;

                return new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success,
                    Body = "El pago se ha registrado correctamente",
                    Header = "Pago realizado"
                };
            }
            catch (Exception ex)
            {
                return new JResponse
                {
                    Result = Cons.Responses.Warning,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = ex.Message,
                    Header = "Error al validar!"
                };
            }
        }

        private JResponse ApplyRefund(Sale sale, ChoosePaymentViewModel payment, ref SaleCreditNote note)
        {
            try
            {
                var refundAmount = payment.RefundCash + payment.RefundCredit;
                var refundPending = sale.SalePayments.Sum(sp => sp.Amount) - sale.FinalAmount;

                //solo se puede aplicar devolución en ventas Modificadas
                if (sale.Status != TranStatus.Modified)
                {
                    return new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.ConditionMissing,
                        Body = "Solo se puede aplicar devoluciones en ventas con estado Modificado, " +
                               "El estado actual es " + sale.Status.GetName(),
                        Header = "Estado de venta incorrecto"
                    };
                }

                if (refundAmount > refundPending)
                {
                    return new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.InvalidData,
                        Body = "El monto que intentas devolver supera lo permitido: " + refundPending.ToMoney(),
                        Header = "Monto incorrecto"
                    };
                }

                //creo el historico
                var history = new SaleHistory
                {
                    Comment = sale.Comment,
                    InsDate = sale.UpdDate,
                    User = sale.UpdUser,
                    SaleId = sale.SaleId,
                    Status = sale.Status.GetName(),
                    TotalDue = sale.FinalAmount
                };
                var cashRegister = GetCashRegister();

                //agrego pagos negativos a la venta (devoluciones)
                if (payment.RefundCash > Cons.Zero)
                {
                    SalePayment p = new SalePayment
                    {
                        SaleId = sale.SaleId,
                        Amount = payment.RefundCash * -1,
                        PaymentDate = DateTime.Now.ToLocal(),
                        Comment = "Devolución por modificacón, recibida por: " + payment.ReceivedBy,
                        PaymentMethod = PaymentMethod.Efectivo,
                        UpdDate = DateTime.Now.ToLocal(),
                        UpdUser = User.Identity.Name
                    };

                    var cashDetail = new CashDetail
                    {
                        Amount = payment.RefundCash,
                        CashRegisterId = cashRegister.CashRegisterId,
                        Comment = p.Comment,
                        InsDate = DateTime.Now.ToLocal(),

                        Type = PaymentMethod.Efectivo,
                        User = User.Identity.Name,
                        DetailType = Cons.Zero,
                        SaleFolio = sale.Folio,
                        WithdrawalCauseId = Cons.RefundId
                    };

                    db.CashDetails.Add(cashDetail);
                    sale.SalePayments.Add(p);
                }

                if (payment.RefundCredit > Cons.Zero)
                {
                    SalePayment p = new SalePayment
                    {
                        SaleId = sale.SaleId,
                        Amount = payment.RefundCredit * -1,
                        PaymentDate = DateTime.Now.ToLocal(),
                        PaymentMethod = PaymentMethod.Vale,
                        UpdDate = DateTime.Now.ToLocal(),
                        UpdUser = User.Identity.Name
                    };

                    //hay que crear nota de crédito
                    var year = Convert.ToInt32(DateTime.Now.ToLocal().ToString("yy"));
                    var last = db.SaleCreditNotes.Where(n => n.Year == year).OrderByDescending(n => n.Sequential).FirstOrDefault();

                    var seq = last == null ? Cons.One : (last.Sequential + Cons.One);

                    note = new SaleCreditNote
                    {
                        SaleCreditNoteId = sale.SaleId,
                        Amount = payment.RefundCredit,
                        ExplirationDate = DateTime.Now.ToLocal().AddDays(Cons.DaysToCancel),
                        RegisterDate = DateTime.Now.ToLocal(),
                        IsActive = true,
                        User = User.Identity.Name,
                        Year = year,
                        Sequential = seq,
                        Ident = payment.Ident,
                        Folio = DateTime.Now.ToLocal().ToString("yy") + "-" + seq.ToString(Cons.CodeSeqFormat)
                    };

                    var cashDetail = new CashDetail
                    {
                        Amount = payment.RefundCredit,
                        CashRegisterId = cashRegister.CashRegisterId,
                        Comment = string.Format("Devolución por modificación! vale {0}, recibo por {1}", note.Folio, payment.ReceivedBy),
                        InsDate = DateTime.Now.ToLocal(),

                        Type = PaymentMethod.Efectivo,
                        User = User.Identity.Name,
                        DetailType = Cons.Zero,
                        SaleFolio = sale.Folio,
                        WithdrawalCauseId = Cons.RefundId
                    };

                    p.Comment = cashDetail.Comment;

                    db.SaleCreditNotes.Add(note);
                    db.CashDetails.Add(cashDetail);
                    sale.SalePayments.Add(p);
                }

                sale.Comment = "Devolución recibida por " + payment.ReceivedBy;
                sale.Status = TranStatus.Compleated;
                sale.UpdDate = DateTime.Now.ToLocal();
                sale.UpdUser = User.Identity.Name;


                if (sale.TransactionType == TransactionType.Presale || sale.TransactionType == TransactionType.Reservation)
                {
                    var isCompleated = (sale.SaleHistories.Count(h => h.Status.Contains(TranStatus.Compleated.GetName())) > Cons.Zero);
                    sale.Status = TranStatus.Revision;
                }

                sale.SaleHistories.Add(history);
                db.Sales.Attach(sale);
                db.Entry(sale).State = EntityState.Modified;

                return new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success,
                    Body = "La devolución ha sido aplicada correctamente",
                    Header = "Devolución realizada!"
                };
            }
            catch (Exception ex)
            {
                return new JResponse
                {
                    Result = Cons.Responses.Warning,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = ex.Message,
                    Header = "Error en la devolución!"
                };
            }
        }

        private JResponse ValidatePayment(Sale sale, ChoosePaymentViewModel payment, double toPay, double wholePayment)
        {
            try
            {
                //verifico que no haya negativos
                if (payment.CardAmount < Cons.Zero || payment.CashAmount < Cons.Zero || payment.CreditNoteAmount < Cons.Zero)
                {
                    return new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.InvalidData,
                        Body = "No puede ingresar montos negativos, en el pago de una venta",
                        Header = "Datos incorrectos!"
                    };
                }

                //el cobro no puede exceder el monto de la deuda
                if (wholePayment > toPay)
                {
                    return new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.InvalidData,
                        Body = string.Format("Estas excediendo el monto del adeudo de esta venta, Adeudo actual:{0}", toPay.ToMoney()),
                        Header = "Pago excedente",
                    };
                }

                //si el monto a pagar es cero y la venta no fue modificada, error
                if (toPay <= 0.0 && sale.Status != TranStatus.Modified)
                {
                    return new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.InvalidData,
                        Body = "Esta venta ya ha sido cobrada en su totalidad",
                        Header = "Cobro no requerido"
                    };
                }

                //si es venta de contado o si el estado de la venta ya había sido completado y aun hay adeudo
                if ((sale.TransactionType == TransactionType.Cash || sale.LastStatus == TranStatus.Compleated) && wholePayment < toPay)
                {
                    return new JResponse
                    {
                        Result = Cons.Responses.Info,
                        Code = Cons.Responses.Codes.InvalidData,
                        Body = string.Format("El monto ingresado {0} no cubre el total del adeudo {1}",
                                            wholePayment.ToMoney(), toPay.ToMoney()),
                        Header = "Se requiere cobro total"
                    };
                }

                //si es Preventa o apartado
                if (sale.TransactionType == TransactionType.Presale || sale.TransactionType == TransactionType.Reservation)
                {
                    var percentage = sale.TransactionType == TransactionType.Presale ? 0.1 :
                              sale.TransactionType == TransactionType.Reservation ? 0.2 : 0;

                    var minAmount = Math.Round((sale.FinalAmount * percentage), Cons.Two);

                    //si es el primer despacho y no se cubre el porcentaje mímimo
                    if (sale.Status == TranStatus.Reserved && minAmount > wholePayment)
                    {
                        return new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Code = Cons.Responses.Codes.InvalidData,
                            Body = string.Format("Esta venta requiere un pago mínimo del {0} %", (percentage * 100)),
                            Header = "Anticipo requerido!"
                        };
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return new JResponse
                {
                    Result = Cons.Responses.Warning,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = ex.Message,
                    Header = "Error al validar!"
                };
            }
        }

        [HttpPost]
        public ActionResult PrintDocument(int saleId, int printType)
        {
            try
            {
                var sale = db.Sales.Where(s => s.SaleId == saleId).Include(s => s.SalePayments).
                         Include(s => s.SaleDetails).Include(s => s.Client.Addresses).Include(s => s.User).
                         Include(s => s.SaleDetails.Select(td => td.Product)).
                         Include(s => s.SaleDetails.Select(td => td.Product.Images)).
                         Include(s => s.SaleDetails.Select(td => td.Product.BranchProducts)).
                         Include(s => s.Branch).FirstOrDefault();


                sale.SaleDetails = sale.SaleDetails.OrderBy(td => td.SortOrder).ToList();

                //se remueven las partidas en ceros
                var toRemove = sale.SaleDetails.Where(d => d.DueQuantity == Cons.Zero).ToList();

                toRemove.ForEach(d => { sale.SaleDetails.Remove(d); });


                if (printType == Cons.One)
                    return PartialView("_PrintTicket", sale);
                else
                    return PartialView("_PrintNote", sale);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al obtener los datos para impresión",
                    Header = "Error obtener datos"
                });
            }
        }
    }
}