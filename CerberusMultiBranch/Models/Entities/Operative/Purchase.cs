using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Display(Name = "Total")]
        public double TotalAmount { get; set; }

        
        [Display(Name = "Factura")]
        [MaxLength(30)]
        public string Bill { get; set; }

        [Required]
        [Display(Name ="Fecha de compra")]
        public string PurchaseDate { get; set; }

        [Required]
        public DateTime InsDate { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        [Required]
        public int  EmployeeId { get; set; }

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }

    public class PurchaseDetail
    {
        public int PurchaseDetailId { get; set; }

        public int PurchaseId { get; set; }

        public int ProductId { get; set; }

        public double Quantity { get; set; }

        public double BuyPrice { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime InsDate { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public virtual Purchase Purchase { get; set; }

    }
}