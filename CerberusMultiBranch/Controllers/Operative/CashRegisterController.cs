using CerberusMultiBranch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CerberusMultiBranch.Support;
using CerberusMultiBranch.Models.Entities.Operative;

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
            return View(cr);
        }

        public JsonResult CashRegStatus()
        {
            var cr = User.Identity.GetCashRegister();

            if (cr != null && !cr.IsClosed)
                return Json("OK" );
            else
                return Json("No se ha abierto el modulo de caja");
        }
    }
}