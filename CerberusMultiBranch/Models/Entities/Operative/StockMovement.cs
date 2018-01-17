using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("StockMovement", Schema = "Operative")]
    public class StockMovement
    {
        public int StockMovementId { get; set; }

        [Column(Order = 0), ForeignKey("BranchProduct")]
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

  
}