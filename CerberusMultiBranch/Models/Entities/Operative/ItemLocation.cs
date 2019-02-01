using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("InventoryItem", Schema = "Operative")]
    public class ItemLocation
    {
        [Column(Order = 0),Key, ForeignKey("BranchProduct")]
        public int BranchId { get; set; }

        [Column(Order = 1), Key, ForeignKey("BranchProduct")]
        public int ProductId { get; set; }

        [Column(Order = 2), Key, ForeignKey("TrackingItem")]
        public int TrackingItemId { get; set; }

        public BranchProduct BranchProduct { get; set; }

        public TrackingItem TrackingItem { get; set; }
    }
}