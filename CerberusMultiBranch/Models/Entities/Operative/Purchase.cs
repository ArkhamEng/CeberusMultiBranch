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
    [Table("Purchase", Schema = "Operative")]
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
        [DataType(DataType.Date)]
        [Display(Name ="Fecha de compra")]
        public string PurchaseDate { get; set; }

        [Required]
        public DateTime InsDate { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        [Required]
        [Display(Name="Empleado")]
        public int  EmployeeId { get; set; }

        #region Navigation Properties

        public virtual Employee Employee { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Provider Provider { get; set; }

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        #endregion

        public Purchase()
        {
            this.PurchaseDetails = new List<PurchaseDetail>();
        }

    }

}