using System;
using CerberusMultiBranch.Models.Entities.Operative;
using System.ComponentModel.DataAnnotations;

namespace CerberusMultiBranch.Models.Entities.Finances
{
    public abstract class FinancialOperation
    {
        [DataType(DataType.Date)]
        public double Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }

        [MaxLength(30)]
        public string Reference { get; set; }

        [MaxLength(100)]
        public string UpdUser { get; set; }

        public DateTime UpdDate { get; set; }
    }
}