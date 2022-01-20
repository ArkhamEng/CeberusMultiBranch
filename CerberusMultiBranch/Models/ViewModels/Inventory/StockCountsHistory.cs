using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Inventory
{
    public class StockCountsHistory
    {
        public int StockCountId { get; set; }

        public int BranchId { get; set; }

        public string BranchName { get; set; }

        public int PartSystemId { get; set; }

        public string PartSystemName { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}")]
        public DateTime Compleation { get; set; }

        public string Comment { get; set; }

        public int LinesCounted { get; set; }

        public int CorrectLines { get; set; }

        public double LinesAccurancy { get; set; }

        public double TotalCost { get; set; }

        public double TotalCostVariance { get; set; }

        public string User { get; set; }

        public List<StockCountsHistoryDetail> StockCountsHistoryDetail { get; set; }

        public StockCountsHistory()
        {

            StockCountsHistoryDetail = new List<StockCountsHistoryDetail>();
        }
    }

}





