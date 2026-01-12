using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Inventory;
using CerberusMultiBranch.Models.ViewModels.Inventory;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Inventory
{
    public class StockCountsHistoryController : Controller
    {

        private StockCountsHistoryViewModel model;

        // GET: StockCountsHistory
        public ActionResult Index()
        {
            model = new StockCountsHistoryViewModel();
            model.Branches = User.Identity.GetBranches().OrderBy(o => o.Name).ToList();
            return View(model);
        }



        [HttpPost]
        public ActionResult Search(int? branchId, DateTime? beginDate, DateTime? endDate)
        {

            ApplicationDbContext db = new ApplicationDbContext();


            model = new StockCountsHistoryViewModel();

            model.StockCounts = (from data in db.StockCounts
                               where (data.Compleation >= beginDate && data.Compleation <= endDate) && data.BranchId == branchId
                               select new StockCountsHistory()
                               {
                                   StockCountId = data.StockCountId,
                                   BranchId = data.BranchId,
                                   BranchName = data.Branch.Name,
                                   PartSystemId = data.PartSystemId,
                                   PartSystemName = data.System.Name,
                                   Description = data.Description,
                                   Comment = data.Comment,
                                   Compleation =  data.Compleation,
                                   LinesCounted =  data.LinesCounted,
                                   CorrectLines = data.CorrectLines,
                                   LinesAccurancy = data.LinesAccurancy,
                                   TotalCost = data.TotalCost,
                                   TotalCostVariance = data.TotalCostVariance,
                                   User = data.User,
                                   StockCountsHistoryDetail = (from datadetail in data.StockCountDetails
                                                               select new StockCountsHistoryDetail
                                                               {
                                                                   StockCountDetailId = datadetail.StockCountDetailId,
                                                                   StockCountId = datadetail.StockCountId,
                                                                   ProductId = datadetail.ProductId,
                                                                   ProductName = datadetail.Product.Name,
                                                                   ProductCode = datadetail.Product.Code,
                                                                   CountQty = datadetail.CountQty,
                                                                   CurrentQty = datadetail.CurrentQty,
                                                                   UnitCost = datadetail.UnitCost,
                                                                   VarianceQty = datadetail.VarianceQty,
                                                                   VarianceCost = datadetail.VarianceCost
                                                                   
                                                               }
                                                               ).ToList()
                               }).ToList();


        


            return PartialView("_StockCountsHistory", model.StockCounts);

        }

      
    }
}