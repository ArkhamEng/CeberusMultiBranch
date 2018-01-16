using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
  
    [Table("SalePayment", Schema = "Finances")]
    public class SalePayment
    {
        public int SalePaymentId { get; set; }

        public int SaleId { get; set; }

        public double Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime PaymentDate { get; set; }

        [MaxLength(100)]
        public string UpdUser { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }

        [MaxLength(30)]
        public string Reference { get; set; }

        public DateTime UpdDate { get; set; }

        public virtual Sale Sale { get; set; }

    }
}