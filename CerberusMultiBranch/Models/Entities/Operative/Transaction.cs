using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("Transaction", Schema = "Operative")]
    public class Transaction
    {
        public int TransactionId { get; set; }

        [Required]
        [Index("IDX_BranchId", IsUnique = false)]
        [ForeignKey("Branch")]
        public int BranchId { get; set; }

        [Required]
        [MaxLength(128)]
        [Index("IDX_UserId", IsUnique = false)]
        public string UserId { get; set; }


        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        [Required]
        public double TotalAmount { get; set; }

        public PaymentType? PaymentType { get; set; }

        [Required]
        [Display(Name = "Fecha de Operación")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Index("IDX_TransactionDate", IsUnique = false)]
        public DateTime TransactionDate { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        [Index("IDX_IsPayed", IsUnique = false)]
        public bool IsPayed { get; set; }


        public ICollection<TransactionDetail> TransactionDetails { get; set; }

        public ICollection<Payment> Payments { get; set; }


        #region Navigation Properties

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public virtual Branch Branch { get; set; }

        #endregion

        #region NotMapped
        [NotMapped]
        [Display(Name = "Empleado")]
        public string EmployeeName { get; set; }

        [NotMapped]
        public int EmployeeId { get; set; }

        #endregion

        public Transaction()
        {
            this.TransactionDetails = new List<TransactionDetail>();
            this.TransactionDate = DateTime.Now;
            this.UpdDate = DateTime.Now;
        }
    }

    public class Purchase : Transaction
    {
        public int ProviderId { get; set; }

        [Display(Name = "Factura")]
        [MaxLength(30)]
        [Required]
        public string Bill { get; set; }

        [Display(Name = "Vencimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Expiration { get; set; }

        [Index("IDX_Inventoried", IsUnique = false)]
        public bool Inventoried { get; set; }

        public virtual Provider Provider { get; set; }

        public Purchase() : base()
        {
            this.Expiration = DateTime.Now;
        }

    }

    public class Sale : Transaction
    {
        public int ClientId { get; set; }

        [Display(Name = "Folio Venta")]
        [MaxLength(30)]
        [Required]
        public string Folio { get; set; }

        public bool Compleated { get; set; }

        public virtual Client Client { get; set; }

        [NotMapped]
        public SelectList Categories { get; set; }

        [NotMapped]
        public SelectList CarMakes { get; set; }

        [NotMapped]
        public SelectList CarModels { get; set; }

        [NotMapped]
        public SelectList CarYear { get; set; }

        public Sale() : base()
        {
            this.CarMakes   = new List<ISelectable>().ToSelectList();
            this.Categories = new List<ISelectable>().ToSelectList();
            this.CarModels  = new List<ISelectable>().ToSelectList();
            this.CarYear    = new List<ISelectable>().ToSelectList();
        }

    }
}