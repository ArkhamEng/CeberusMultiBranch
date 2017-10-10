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
        // GET: CashRegister
        public ActionResult Index()
        {
            var cr = User.Identity.GetCashRegister();

            if (cr == null)
            {
                cr = new CashRegister();
                cr.BranchId = User.Identity.GetBranchId();
                db.CashRegisters.Add(cr);
                db.SaveChanges();
            }

            DateTime begin, end;

            if (DateTime.Now.Hour < 14)
            {
                begin = new DateTime(cr.BeginDate.Year, cr.BeginDate.Month, cr.BeginDate.Day, 6, 0, 0);
                end = new DateTime(cr.BeginDate.Year, cr.BeginDate.Month, cr.BeginDate.Day, 14, 0, 0);
            }
            else
            {
                begin = new DateTime(cr.BeginDate.Year, cr.BeginDate.Month, cr.BeginDate.Day, 14, 0, 0);
                end = new DateTime(cr.BeginDate.Year, cr.BeginDate.Month, cr.BeginDate.Day, 22, 0, 0);
            }

            var range = end - begin;
            var xVal = Enumerable.Range(0, (int)range.TotalHours).Select(i => begin.AddHours(i).Hour).ToList();

            var incomes = cr.Incomes.GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount), Key = cd.Key }).ToList();
            var cash = cr.Incomes.Where(cd => cd.Type == PaymentType.Cash).GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount), Key = cd.Key }).ToList();
            var card = cr.Incomes.Where(cd => cd.Type == PaymentType.Card).GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount), Key = cd.Key }).ToList();
            var Withdrawals = cr.Withdrawals.GroupBy(cd => cd.InsDate.Hour).Select(cd => new { Total = cd.Sum(c => c.Amount), Key = cd.Key }).ToList();

            var accum = new List<double>();
            var accumN = new List<int>();

            foreach (var hour in xVal)
            {
                if (!incomes.Select(c => c.Key).Contains(hour))
                    incomes.Add(new { Total = Convert.ToDouble(Cons.Zero), Key = hour });

                if (!cash.Select(c => c.Key).Contains(hour))
                    cash.Add(new { Total = Convert.ToDouble(Cons.Zero), Key = hour });

                if (!card.Select(c => c.Key).Contains(hour))
                    card.Add(new { Total = Convert.ToDouble(Cons.Zero), Key = hour });

                if (!Withdrawals.Select(c => c.Key).Contains(hour))
                    Withdrawals.Add(new { Total = Convert.ToDouble(Cons.Zero), Key = hour });
            }



            cash = cash.OrderBy(c => c.Key).ToList();
            card = card.OrderBy(c => c.Key).ToList();
            Withdrawals = Withdrawals.OrderBy(c => c.Key).ToList();

            for (int i = 0; i < xVal.Count; i++)
            {
                double val = Cons.Zero;

                if (i == Cons.Zero)
                    val = incomes[i].Total - Withdrawals[i].Total;
                else
                    val = accum[i - Cons.One] + (incomes[i].Total - Withdrawals[i].Total);

                if (val != Cons.Zero)
                {
                    accum.Add(val);
                    accumN.Add(incomes[i].Key);
                }
            }

           accumN = accumN.OrderBy(d=>d).ToList();

            var balance = new Chart(800, 500, theme: ChartTheme.Green);

            balance.AddSeries("Efectivo", chartType: "Column", yValues: cash.Select(i => i.Total).ToList(), xValue: cash.Select(i => i.Key).ToList());
            balance.AddSeries("Tarjeta", chartType: "Column", yValues: card.Select(i => i.Total).ToList(), xValue: card.Select(i => i.Key).ToList());
            balance.AddSeries("Retiros", chartType: "Column", yValues: Withdrawals.Select(i => i.Total).ToList(), xValue: Withdrawals.Select(i => i.Key).ToList());
            balance.AddSeries("Acumulado", chartType: "Line", yValues: accum, xValue: accumN,yFields:"Cantidad",xField:"Hora");

            balance.AddLegend("Turno " + begin.TimeOfDay.ToString() + "-" + end.TimeOfDay.ToString());

            var imgS = balance.GetBytes();
            var baseS = Convert.ToBase64String(imgS);
            cr.ChartSource = String.Format("data:image/jpeg;base64,{0}", baseS);

            return View(cr);
        }

        public JsonResult CashRegStatus()
        {
            var cr = User.Identity.GetCashRegister();

            if (cr != null && !cr.IsClosed)
                return Json("OK");
            else
                return Json("No se ha abierto el modulo de caja");
        }

        public static void AddIncome(double amount, PaymentType type, IIdentity user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var cr = user.GetCashRegister();
                var dt = new Income();
                dt.Amount = amount;
                dt.InsDate = DateTime.Now;
                dt.CashRegisterId = cr.CashRegisterId;
                dt.User = user.Name;
                dt.Type = type;
                db.Incomes.Add(dt);
                db.SaveChanges();
            }
        }

        public static void AddWithdrawal(double amount, IIdentity user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var cr = user.GetCashRegister();
                var dt = new Income();
                dt.Amount = amount;
                dt.InsDate = DateTime.Now;
                dt.CashRegisterId = cr.CashRegisterId;
                dt.User = user.Name;

                db.Incomes.Add(dt);
                db.SaveChanges();
            }
        }

        [HttpPost]
        public JsonResult OpenCashRegister(double initialAmount)
        {
            try
            {
                var cr = User.Identity.GetCashRegister();
                cr.BeginDate = DateTime.Now;
                cr.UserOpen = User.Identity.Name;
                cr.IsClosed = false;
                cr.InitialAmount = Math.Round(initialAmount, Cons.Two);


                db.Entry(cr).State = EntityState.Modified;

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
            return PartialView("_Withdrawals", cr.Withdrawals);
        }

        [HttpPost]
        public ActionResult AddWithdrawal(double amount, string comment)
        {
            var cr = User.Identity.GetCashRegister();

            var wd = new Withdrawal { Amount = amount, InsDate= DateTime.Now, User = User.Identity.Name, Comment = comment, CashRegisterId = cr.CashRegisterId };

            db.Withdrawals.Add(wd);
            db.SaveChanges();

            return Json("OK");
        }
    }
}