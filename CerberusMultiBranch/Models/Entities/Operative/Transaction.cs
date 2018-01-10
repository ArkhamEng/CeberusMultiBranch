using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public class Transaction
    {
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

        [Display(Name = "Monto Pagado")]
        [DataType(DataType.Currency)]
        [Required]
        public double PaidAmount { get; set; }

        [Display(Name = "Monto en Deuda")]
        [DataType(DataType.Currency)]
        [Required]
        public double DebtAmount { get; set; }

        [Index("IDX_Status", IsUnique = false)]
        public TranStatus Status { get; set; }

        public TranStatus LastStatus { get; set; }

        public bool IsCredit { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "Fecha de Operación")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Index("IDX_TransactionDate", IsUnique = false)]
        public  DateTime TransactionDate { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        [Required]
        public string UpdUser { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public virtual Branch Branch { get; set; }



    }
}