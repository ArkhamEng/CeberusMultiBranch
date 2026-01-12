using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Inventory
{
    public class StockCountsHistoryDetail
    {
        public int StockCountDetailId { get; set; }

        public int StockCountId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public double UnitCost { get; set; }

        public double CurrentQty { get; set; }

        public double CountQty { get; set; }

        public double VarianceQty { get; set; }

        public double VarianceCost { get; set; }
    }
}