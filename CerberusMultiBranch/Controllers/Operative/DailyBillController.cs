using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.ViewModels.Operative;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Operative
{
    public partial class SalesController
    {
        public ActionResult DailyBill()
        {
            DailyBillViewModel model = new DailyBillViewModel();
            model.Branches = User.Identity.GetBranches().ToSelectList();
           
            return View(model);
        }

        [HttpPost]
        public ActionResult SearchSoldItems(DailyBillViewModel model)
        {
            
            var SoldItems = (from sd in db.SaleDetails.Include(sd => sd.Product.Images).Include(sd=> sd.Product.Category)
                         where (sd.Sale.TransactionDate >= model.Date) &&
                               (sd.Sale.TransactionDate < model.EndDate) &&
                               (sd.Sale.TransactionType == model.TransType) &&
                               (sd.Sale.Status == TranStatus.Compleated) && //solo las ventas pagadas en su totalidad
                               (sd.Sale.BranchId == model.BranchId) &&
                               (model.Client == null || sd.Sale.Client.Name == model.Client) &&
                               (model.Folio == null || sd.Sale.Folio == model.Folio) 
                         select sd).ToList();

            
            return PartialView("_SoldItemsDetails", SoldItems);
        }

      
    }
}