using CerberusMultiBranch.Models.Entities.Catalog;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Inventory
{
    [Table("StockCountDetail", Schema = "Inventory")]
    public class StockCountDetail
    {
        public int StockCountDetailId { get; set; }

        public int ProductId { get; set; }

        public int StockCountId { get; set; }

        public double UnitCost { get; set; }

        public double CurrentQty { get; set; }

        public double CountQty { get; set; }

        public double VarianceCost { get; set; }

        public double VarianceQty { get; set; }

        public virtual StockCount Counting { get; set; }

        public virtual Product  Product { get; set; }
    }
}