using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("BranchProduct", Schema = "Operative")]
    public class BranchProduct
    {
        [Column(Order = 0), Key, ForeignKey("Branch")]
        public int BranchId { get; set; }

        [Column(Order = 1), Key, ForeignKey("Product")]
        public int ProductId { get; set; }

        public double Stock { get; set; }

        public double LastStock { get; set; }

        public DateTime UpdDate { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Product Product { get; set; }

        public ICollection<StockMovement> StockMovements { get; set; }
    }

    [Table("StockMovement", Schema = "Operative")]
    public class StockMovement
    {
        public int StockMovementId { get; set; }

        [Column(Order = 0),ForeignKey("BranchProduct")]
        public int BranchId { get; set; }

        [Column(Order = 1), ForeignKey("BranchProduct")]
        public int ProductId { get; set; }

        public double Quantity { get; set; }

        public DateTime MovementDate { get; set; }

        public string User { get; set; }

        public MovementType MovementType { get; set; }

        public string Comment { get; set; }

        public virtual BranchProduct BranchProduct { get; set; }
    }

    public enum MovementType
    {
        Entry=1,
        Exit=2,
    }
}