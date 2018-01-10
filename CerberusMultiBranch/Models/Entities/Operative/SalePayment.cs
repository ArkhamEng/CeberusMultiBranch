using System;
using System.Collections.Generic;
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

        public virtual Sale Sale { get; set; }

    }
}