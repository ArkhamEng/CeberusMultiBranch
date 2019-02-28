using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Finances
{
    [Table("PurchaseDiscount", Schema = "Finances")]
    public class PurchaseDiscount
    {
        public int PurchaseDiscountId { get; set; }

        public int PurchaseId { get; set; }

        public double DiscountAmount { get; set; }

        public double DiscountPercentage { get; set; }

        public string Comment { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime InsDate { get; set; }

        [MaxLength(30)]
        public string InsUser { get; set; }

        public virtual Purchase Purchase { get; set; }

    }
}