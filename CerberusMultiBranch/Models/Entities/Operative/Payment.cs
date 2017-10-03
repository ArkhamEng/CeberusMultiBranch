using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public enum PaymentType
    {
        Card   = 1,
        Cash   = 2,
        Check  = 3,
        Mixed  = 4,
        Credit = 5
    }

    [Table("Payment", Schema = "Operative")]
    public class Payment
    {
        public int PaymentId { get; set; }

        public int TransactionId { get; set; }

        public double Amount { get; set; }

        public PaymentType PaymentType { get; set; }

        public DateTime PaymentDate { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}