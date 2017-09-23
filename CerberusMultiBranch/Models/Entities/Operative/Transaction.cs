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
    [Table("Transaction", Schema = "Operative")]
    public class Transaction
    {
        public int TransactionId { get; set; }

        [Index("IDX_TransactionTypeId", IsUnique = false)]
        public int TransactionTypeId { get; set; }

        [Required]
        [Index("IDX_BranchId", IsUnique = false)]
        public int BranchId { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        [Required]
        public double TotalAmount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Index("IDX_TransactionDate", IsUnique = false)]
        public DateTime TransactionDate { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        [Required]
        [MaxLength(128)]
        [Index("IDX_UserId", IsUnique = false)]
        public string UserId { get; set; }

        [Index("IDX_IsCompleated", IsUnique = false)]
        public bool IsCompleated { get; set; }

        #region Navigation Properties

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual TransactionType TransactionType { get; set; }

        public ICollection<TransactionDetail> TransactionDetails { get; set; }
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

    public class Purchase:Transaction
    {
        public int ProviderId { get; set; }

        [Display(Name = "Factura")]
        [MaxLength(30)]
        [Required]
        public string Bill { get; set; }

        public virtual Provider Provider { get; set; }

    }

    public class Sale:Transaction
    {
        public int ClientId { get; set; }

        [Display(Name = "Folio Venta")]
        [MaxLength(30)]
        [Required]
        public string Folio { get; set; }

        public virtual Client Client { get; set; }

    }

    public class Transference : Transaction
    {
        public int OriginBranchId { get; set; }

       [ForeignKey("OriginBranchId")]
        public virtual Branch OriginBranch { get; set; }

    }

}