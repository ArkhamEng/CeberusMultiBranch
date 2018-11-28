using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.ViewModels.Operative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Operative
{
    public partial class PurchasesController : Controller
    {
       
        public ActionResult Order(int? id)
        {
            var model = new PurchasOrderViewModel();
            return View(model);
        }
    }
}