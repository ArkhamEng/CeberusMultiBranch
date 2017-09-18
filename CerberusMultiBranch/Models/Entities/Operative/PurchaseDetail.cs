using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("PurchaseDetail", Schema = "Operative")]
    public class PurchaseDetail
    {
        public int PurchaseDetailId { get; set; }

        public int PurchaseId { get; set; }

        public int ProductId { get; set; }

        public double Quantity { get; set; }

        public double BuyPrice { get; set; }

        public double Amout { get; set; }

        [MaxLength(29)]
        public string Unit { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime InsDate { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        public virtual Purchase Purchase { get; set; }

        public virtual Product Product { get; set; }

    }
}