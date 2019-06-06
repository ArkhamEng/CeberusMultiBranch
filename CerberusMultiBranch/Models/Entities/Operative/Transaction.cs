using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public abstract class Transaction 
    {
        [Required]
        [Index("IDX_BranchId", IsUnique = false)]
        [ForeignKey("Branch")]
        public int BranchId { get; set; }

        public TranStatus LastStatus { get; set; }


        [Required]
        [Display(Name = "Fecha de Operación")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Index("IDX_TransactionDate", IsUnique = false)]
        public DateTime TransactionDate { get; set; }


        [Required]
        [MaxLength(128)]
        [Index("IDX_UserId", IsUnique = false)]
        public string UserId { get; set; }



        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        [Required]
        public double TotalAmount { get; set; }

        [Display(Name = "Total con IVA")]
        [DataType(DataType.Currency)]
        [Required]
        public double TotalTaxedAmount { get; set; }

        [Display(Name = "Total de IVA")]
        [DataType(DataType.Currency)]
        [Required]
        public double TotalTaxAmount { get; set; }

        [Display(Name = "Monto descuento")]
        [DataType(DataType.Currency)]
        public double DiscountedAmount { get; set; }

        [Display(Name = "Descuento")]
        public double DiscountPercentage { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Monto Final")]
        public double FinalAmount { get; set; }

        [Index("IDX_Status", IsUnique = false)]
        public TranStatus Status { get; set; }

     
        public TransactionType TransactionType { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }

        [Display(Name = "Vencimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Expiration { get; set; }


        [Required]
        [Display(Name = "Editado")]
        public DateTime UpdDate { get; set; }

        [Required]
        [Display(Name = "Editado por")]
        public string UpdUser { get; set; }

        [Required]
        [Display(Name = "Creado")]
        public DateTime InsDate { get; set; }

        [Required]
        [Display(Name ="Creador por")]
        public string InsUser { get; set; }


        public virtual Branch Branch { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

      

        public Transaction()
        {
            this.Expiration = DateTime.Now;
            this.UpdDate = DateTime.Now.ToLocal();
            this.InsDate = DateTime.Now.ToLocal();
            this.UpdUser = HttpContext.Current.User.Identity.Name;
            this.InsUser = HttpContext.Current.User.Identity.Name;
        }

        [NotMapped]
        public virtual bool CanCancel
        {
            get { return DateTime.Now.ToLocalTime() < 
                    this.TransactionDate.AddDays(Cons.DaysToCancel) && 
                    this.Status != TranStatus.Canceled && this.Status != TranStatus.PreCancel; }
        }

    }
}