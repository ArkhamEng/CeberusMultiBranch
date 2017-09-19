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

        [Display(Name = "Cantidad")]
        public double Quantity { get; set; }

        [Display(Name = "Precio Compra")]
        public double BuyPrice { get; set; }

        [Display(Name ="Importe")]
        public double Amount { get; set; }

        [Required]
        public DateTime InsDate { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        public virtual Purchase Purchase { get; set; }

        public virtual Product Product { get; set; }

        public PurchaseDetail()
        {
            this.InsDate = DateTime.Now;
            this.UpdDate = DateTime.Now;
        }
    }
}