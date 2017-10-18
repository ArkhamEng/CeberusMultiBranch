﻿using CerberusMultiBranch.Models.Entities.Catalog;
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
}