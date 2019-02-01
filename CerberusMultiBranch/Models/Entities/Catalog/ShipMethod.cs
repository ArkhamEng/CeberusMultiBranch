using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("ShipMethod", Schema = "Catalog")]
    public class ShipMethod:ISelectable
    {
        public int ShipMethodId { get; set; }

        [MaxLength(30)]
        public string  Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        public DateTime InsDate { get; set; }

        public string InsUser { get; set; }

        public DateTime UpdDate { get; set; }

        public string UpdUser { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public int Id
        {
            get
            {
                return this.ShipMethodId;
            }
        }
    }
}