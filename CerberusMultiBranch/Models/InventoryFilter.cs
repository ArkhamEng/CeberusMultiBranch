using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Inventory;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models
{
    public class InventoryFilter
    {
      
        public List<StockCountDetail> Table { get; set; }

        public string Observations { get; set; }

        public string Name { get; set; }

        public int IdBranch { get; set; }

        public int IdPartSystem { get; set; }

        public List<SearchProductResultViewModel> Products { get; set; }

        public List<StockCountDetail> StockCountDetails { get; set; }

        public int CorrectLines { get; set; }

        public int LinesCounted { get; set; }

        public int LinesAccurancy { get; set; }

        public double TotalCost { get; set; }

        public double TotalCostVariance { get; set; }

        public string User { get; set; }

        public DateTime BeginDate { get; set; }



    }
}