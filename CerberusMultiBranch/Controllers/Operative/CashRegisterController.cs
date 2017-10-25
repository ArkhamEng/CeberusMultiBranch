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

namespace CerberusMultiBranch.Controllers.Operative
{
    [Authorize]
    public class CashRegisterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private CashRegister GetNewCR()
        {
            // var cs = DateTime.Now.GetShift();
            var cr = new CashRegister();
            cr.OpeningDate = DateTime.Now;
            cr.UserOpen = User.Identity.Name;
            cr.BranchId = User.Identity.GetBranchId();
            return cr;
        }
        public ActionResult History()
        {
            var branchId = User.Identity.GetBranchId();
            var model = db.CashRegisters.Include(cr=> cr.CashDetails).Where(cr => cr.BranchId == branchId);
            
            return View(model);
        }

        
        public ActionResult Detail(int id)
        {
            var branchId = User.Identity.GetBranchId();
            var model    = db.CashRegisters.Include(cr => cr.CashDetails).FirstOrDefault(cr => cr.CashRegisterId == id );

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(DateTime? beginDate, DateTime? endDate, string user)
        {
            var branchId = User.Identity.GetBranchId();
            var model = db.CashRegisters.Include(cr => cr.CashDetails).
                Where(cr => cr.BranchId == branchId && 
                (beginDate == null || cr.OpeningDate >= beginDate) &&
                (endDate == null || cr.OpeningDate <= endDate) &&
                (user == null || user== string.Empty || cr.UserOpen == user ));

            return PartialView("_CashRegisterList",model);
        }


        // GET: CashRegister
        public ActionResult Index()
        {
            var cr = User.Identity.GetCashRegister();

            if (cr == null)
            {
                cr = GetNewCR();
                return View(cr);
            }

            //var shift = cr.OpeningDate.GetShift();

            //var range = shift.EndDate - shift.BeginDate;
            //var xVal = Enumerable.Range(0, (int)range.TotalHours).Select(i => shift.BeginDate.AddHours(i).Hour).ToList();

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
        public ActionResult AddWithdrawal(double amount, string comment, int causeId)
        {
            var cr = User.Identity.GetCashRegister();

            var wd = new Withdrawal { Amount = amount, InsDate = DateTime.Now, User = User.Identity.Name, Comment = comment, CashRegisterId = cr.CashRegisterId, WithdrawalCauseId = causeId };

            db.Withdrawals.Add(wd);
            db.SaveChanges();

            return Json("OK");
        }

        [HttpPost]
        public ActionResult Close(double amount, string comment)
        {
            var cr = User.Identity.GetCashRegister();

            cr.FinalAmount = Math.Round(amount, Cons.Two);
            cr.UserClose = User.Identity.Name;
            cr.ClosingDate = DateTime.Now;
            cr.CloseComment = comment;
            cr.IsOpen = false;

            db.Entry(cr).State = EntityState.Modified;
            db.SaveChanges();

            return Json("OK");
        }
    }
}