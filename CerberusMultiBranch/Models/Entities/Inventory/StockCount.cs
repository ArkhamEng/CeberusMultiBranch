using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Inventory
{
    [Table("StockCount", Schema = "Inventory")]
    public class StockCount
    {
        public int StockCountId { get; set; }

        [Index("IDX_Branch_System_Date",Order =0)]
        public int PartSystemId { get; set; }

        [Index("IDX_Branch_System_Date", Order = 1)]
        public int BranchId { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        [Index("IDX_Branch_System_Date", Order = 2)]
        public DateTime Compleation { get; set; }

        public int LinesCounted { get; set; }

        public int CorrectLines { get; set; }

        public double LinesAccurancy { get; set; }

        public double TotalCost { get; set; }

        public double TotalCostVariance { get; set; }

        public string User { get; set; }

         public virtual PartSystem System { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual ICollection<StockCountDetail> StockCountDetails { get; set; }


        public StockCount()
        {
            StockCountDetails = new List<StockCountDetail>();
        }

    }
}