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

namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize]
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

        [Authorize(Roles ="Cajero")]
        public ActionResult MyHistory()
        {
            var branchId = User.Identity.GetBranchId();
            var model = LookForCashReg(branchId, DateTime.Today, null, User.Identity.Name);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult MySearch(DateTime? beginDate, DateTime? endDate)
        {
            var branchId = User.Identity.GetBranchId();
            var model = LookForCashReg(branchId, beginDate, endDate, User.Identity.Name);

            return PartialView("_MyCashRegisterList", model);
        }

        [Authorize(Roles ="Cajero")]
        public ActionResult MyDetail(int id)
        {
            var model = db.CashRegisters.Include(cr => cr.CashDetails).
                FirstOrDefault(cr => cr.CashRegisterId == id && cr.UserOpen == User.Identity.Name);

            if (model == null)
                return RedirectToAction("MyHistory");

            return View(model);
        }

        [Authorize(Roles ="Supervisor")]
        public ActionResult History()
        {
            var model = LookForCashReg(null, DateTime.Today, null, null);
            ViewBag.Branches =  User.Identity.GetBranches().ToSelectList();

            return View(model);
        }

        [Authorize(Roles = "Supervisor")]
        public ActionResult Detail(int id)
        {
            var brancheIds = User.Identity.GetBranches().Select(b => b.BranchId);
            var model    = db.CashRegisters.Include(cr => cr.CashDetails).
                FirstOrDefault(cr => cr.CashRegisterId == id && brancheIds.Contains(cr.BranchId));

            if (model == null)
                return RedirectToAction("History");

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate, string user)
        {
            var model = LookForCashReg(branchId, beginDate, endDate, user);

            return PartialView("_CashRegisterList",model);
        }


        private List<CashRegister> LookForCashReg(int? branchId, DateTime? beginDate, DateTime? endDate, string user)
        {
            var branchIds = User.Identity.GetBranches().Select(b => b.Id);

            var model = db.CashRegisters.Include(cr => cr.CashDetails).Include(cr=> cr.Branch).
            Where(cr => (branchId ==null && branchIds.Contains(cr.BranchId) || cr.BranchId == branchId) &&
                        (beginDate == null || cr.OpeningDate >= beginDate) &&
                        (endDate == null || cr.OpeningDate <= endDate) &&
                        (user == null || user == string.Empty || cr.UserOpen == user)).ToList();

            return model;

        }

        // GET: CashRegister
        [Authorize(Roles = "Supervisor,Cajero")]
        public ActionResult Index()
        {
            var cr = User.Identity.GetCashRegister();

            if (cr == null)
            {
                cr = GetNewCR();
                return View(cr);
            }

            var incomes = cr.Incomes.GroupBy(cd => cd.InsDate).Select(cd => new { Total = cd.Sum(c => c.Amount).ToString("c"), Key = cd.Key.ToString("HH:mm") }).ToList();
            var cash = cr.Incomes.Where(cd => cd.Type == PaymentType.Efectivo).GroupBy(cd => cd.InsDate).Select(cd => new { Total = cd.Sum(c => c.Amount).ToString("c"), Key = cd.Key.ToString("HH:mm") }).ToList();
            var card = cr.Incomes.Where(cd => cd.Type == PaymentType.Tarjeta).GroupBy(cd => cd.InsDate).Select(cd => new { Total = cd.Sum(c => c.Amount).ToString("c"), Key = cd.Key.ToString("HH:mm") }).ToList();
            var Withdrawals = cr.Withdrawals.GroupBy(cd => cd.InsDate).Select(cd => new { Total = cd.Sum(c => c.Amount).ToString("c"), Key = cd.Key.ToString("HH:mm") }).ToList();

            var accum = new List<double>();
            var accumN = new List<int>();

            incomes.Add(new { Total = (0.0).ToString("c"), Key = cr.OpeningDate.ToString("HH:mm") });
            cash.Add(new { Total = (0.0).ToString("c"), Key = cr.OpeningDate.ToString("HH:mm") });
            card.Add(new { Total = (0.0).ToString("c"), Key = cr.OpeningDate.ToString("HH:mm") });
            Withdrawals.Add(new { Total = (0.0).ToString("c"), Key = cr.OpeningDate.ToString("HH:mm") });


            incomes = incomes.OrderBy(c => c.Key).ToList();
            cash = cash.OrderBy(c => c.Key).ToList();
            card = card.OrderBy(c => c.Key).ToList();
            Withdrawals = Withdrawals.OrderBy(c => c.Key).ToList();

            accumN = accumN.OrderBy(d => d).ToList();

            var balance = new Chart(800, 500, theme: ChartTheme.Green);

            balance.AddSeries("Efectivo", chartType: "Column", yValues: cash.Select(i => i.Total).ToList(), xValue: cash.Select(i => i.Key).ToList());
            balance.AddSeries("Tarjeta", chartType: "Column", yValues: card.Select(i => i.Total).ToList(), xValue: card.Select(i => i.Key).ToList());
            balance.AddSeries("Retiros", chartType: "Column", yValues: Withdrawals.Select(i => i.Total).ToList(), xValue: Withdrawals.Select(i => i.Key).ToList());
            //balance.AddSeries("Acumulado", chartType: "Line", yValues: accum, xValue: accumN, yFields: "Cantidad", xField: "Hora");

            balance.AddLegend("Balance de Caja");

            var imgS = balance.GetBytes();
            var baseS = Convert.ToBase64String(imgS);
            cr.ChartSource = String.Format("data:image/jpeg;base64,{0}", baseS);

            return View(cr);
        }

        [HttpPost]
        public JsonResult CashRegStatus()
        {
            var cr = User.Identity.GetCashRegister();

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
            var cr = User.Identity.GetCashRegister();

            var list = db.Withdrawals.Include(w => w.Cause).Where(w => w.CashRegisterId == cr.CashRegisterId).ToList();
            ViewBag.Causes = db.WithdrawalCauses.ToSelectList();
            return PartialView("_Withdrawals", list);
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult AddWithdrawal(double amount, string comment, int causeId)
        {
            var cr = User.Identity.GetCashRegister();

            var wd = new Withdrawal { Amount = amount, InsDate = DateTime.Now.ToLocal(),
                User = User.Identity.Name, Comment = comment, CashRegisterId = cr.CashRegisterId,
                WithdrawalCauseId = causeId };

            db.Withdrawals.Add(wd);
            db.SaveChanges();

            return Json("OK");
        }

        [HttpPost]
        [Authorize(Roles = "Cajero,Supervisor")]
        public ActionResult Close(double amount, string comment)
        {
            var cr = User.Identity.GetCashRegister();

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

        [Authorize(Roles = "Cajero")]
        public ActionResult OpenPending()
        {
            var branchId = User.Identity.GetBranchId();

            var model = db.Sales.Where(s => s.BranchId == branchId && s.Status == TranStatus.Reserved)
                .Include(s=> s.User).Include(s => s.Client).ToList();

            return PartialView("_PendingPayment", model);
        }

        [Authorize(Roles = "Cajero")]
        public ActionResult TicketsAndNotes()
        {
            var sales = LookForNotes(DateTime.Today,null,null,null);
            return PartialView("_TicketsAndNotes",sales);
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult SearchNotes(DateTime? begin, DateTime? end, string folio, string client)
        {
            var sales = LookForNotes(begin, end, folio,client);
            return PartialView("_SalesToPayList", sales);
        }

        private List<Sale> LookForNotes(DateTime? begin, DateTime? end,string folio, string client)
        {
           var branchId = User.Identity.GetBranchId();

            //busco las ventas de la sucursal que esten reservadas o completadas (ya pagadas)
            var sales = db.Sales.Include(s=> s.Client).Include(s => s.TransactionDetails).
                Include(s => s.User).Where(s => s.BranchId == branchId &&
            (begin == null || s.TransactionDate >= begin) &&
            (end == null || s.TransactionDate <= end) &&
            (folio == null || folio == string.Empty || s.Folio == folio) &&
            (client == null || client == string.Empty || s.Client.Name.Contains(client)) &&
            (s.Status == TranStatus.Compleated)).
            OrderByDescending(s=> s.TransactionDate).ToList();

            return sales;
        }

     
        [Authorize(Roles = "Cajero")]
        public ActionResult PrintNote(int id)
        {
            var branchId = User.Identity.GetBranchId();
            // busco la venta con el id provisto validando que ya tenga status compleated (pagada)
            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.Client).Include(s => s.User).
             Include(s => s.TransactionDetails.Select(td => td.Product)).
             Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
             Include(s => s.TransactionDetails.Select(td => td.Product.BranchProducts)).
             Include(s => s.Branch).
             FirstOrDefault(s => s.TransactionId == id && s.Status == TranStatus.Compleated && s.BranchId == branchId);

            if(sale == null)
                return RedirectToAction("Index");

            sale.TransactionDetails = sale.TransactionDetails.OrderBy(td => td.SortOrder).ToList();

            return View(sale);
        }

        [Authorize(Roles = "Cajero")]
        public ActionResult RegistPayment(int id)
        {
            var branchId = User.Identity.GetBranchId();

            //obtengo la venta con el id dado verificando que este en status Reserved
            var sale = db.Sales.Include(s => s.TransactionDetails).Include(s => s.Client).
                       Include(s => s.TransactionDetails.Select(td => td.Product)).
                       Include(s => s.User).
                       Include(s => s.TransactionDetails.Select(td => td.Product.Images)).
                       FirstOrDefault(s => s.TransactionId == id && 
                       s.Status == TranStatus.Reserved && s.BranchId == branchId);

            if (sale == null)
                return RedirectToAction("Index");

            sale.TransactionDetails = sale.TransactionDetails.OrderBy(td => td.SortOrder).ToList();

            return View(sale);
        }

        [HttpPost]
        [Authorize(Roles = "Cajero")]
        public ActionResult RegisterPayment(int transactionId, string payment, double? cash, double? card)
        {
            try
            {
                //busco la venta a pagar
                var sale = db.Sales.Include(s => s.TransactionDetails).
                    FirstOrDefault(s => s.TransactionId == transactionId);

                //marco la venta como pagada y coloco el tipo de pago
                sale.LastStatus     = sale.Status;
                sale.Status         = TranStatus.Compleated;
                sale.UpdDate        = DateTime.Now.ToLocal();
                sale.UpdUser        = User.Identity.Name;
                sale.PaymentType    = (PaymentType)Enum.Parse(typeof(PaymentType), payment);
                sale.Payments       = new List<Payment>();

                #region Registros de pago
                //si el pago es con efectivo o tarjeta agrego un registro de pago por el monto total
                //de la venta
                if (payment != PaymentType.Mixto.ToString())
                {
                    var p = new Payment
                    {
                        TransactionId = sale.TransactionId,
                        Amount = sale.TotalAmount,
                        PaymentDate = DateTime.Now.ToLocal(),
                        PaymentType = sale.PaymentType,
                    };
                    sale.Payments.Add(p);
                }
                //si el pago es Mixto (Efectivo y Tarjeta) agrego un registro de pago con efectivo
                //y otro de tarjeta con los parametros de entrada correspondientes
                else
                {
                    var pm = new Payment
                    { TransactionId = sale.TransactionId, Amount = cash.Value,
                        PaymentDate = DateTime.Now.ToLocal(), PaymentType = PaymentType.Efectivo };

                    var pc = new Payment
                    { TransactionId = sale.TransactionId, Amount = card.Value,
                        PaymentDate = DateTime.Now.ToLocal(), PaymentType = PaymentType.Tarjeta };

                    sale.Payments.Add(pm);
                    sale.Payments.Add(pc);
                }
                #endregion

                //hago attach a la venta.. esto permite agregar los registros de pago a la base de datos
                db.Sales.Attach(sale);
                db.Entry(sale).State = EntityState.Modified;

                #region registro de Ingreso a caja
                //obtengo el registro de caja activo
                var cr = User.Identity.GetCashRegister();

                if (cr == null)
                {
                    return Json(new
                    {
                        Result = "Error al registrar el pago!",
                        Message = "El modulo de caja no ha sido abierto"
                    });
                }
                //por cada registro de pago agrego una registro de entrada a la caja
                foreach (var pay in sale.Payments)
                {
                    var dt = new Income();
                    dt.CashRegisterId = cr.CashRegisterId;
                    dt.Amount = pay.Amount;
                    dt.InsDate = DateTime.Now.ToLocal();
                    dt.User = User.Identity.Name;
                    dt.Type = pay.PaymentType;
                    dt.SaleFolio = sale.Folio;
                    db.Incomes.Add(dt);
                }
                #endregion

                db.SaveChanges();

                return Json(new { Result = "OK", Message= sale.TransactionId.ToString() });
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

    }
}