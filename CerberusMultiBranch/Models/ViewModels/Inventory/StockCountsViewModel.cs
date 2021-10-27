using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Inventory;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CerberusMultiBranch.Models.ViewModels.Inventory
{
    public class StockCountsViewModel
    {
        
        public List<Branch> Branches { get; set; } 

        public List<PartSystem> Systems { get; set; }

        public List<StockCountDetail> StockCountsDetails { get; set; }

        public StockCount StockCount { get; set; }

        public StockCountsViewModel()
        {
            StockCount = new StockCount();

            StockCountsDetails = new List<StockCountDetail>();

            Branches = new List<Branch>();

            Systems = new List<PartSystem>();

        }
    }
}