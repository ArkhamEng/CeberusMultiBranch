using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("TrackingItem", Schema = "Operative")]
    public class TrackingItem
    {
        public int TrackingItemId { get; set; }

        public int ProductId { get; set; }

        public string SerialNumber { get; set; }

        public bool IsBatch { get; set; }

        public DateTime InsDate { get; set; }

        public string InsUser { get; set; }

        public virtual Product Product   { get; set; }

        public ICollection<StockMovement> StockMovements { get; set; }

        public ICollection<ItemLocation> ItemLocations { get; set; }

        public TrackingItem()
        {
            this.InsDate = DateTime.Now.ToLocal();
            this.InsUser = HttpContext.Current.User.Identity.Name;
        }
    }
}