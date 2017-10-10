using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public enum PaymentType
    {
        Card   = 1, //Tarjeda Credito o débito
        Cash   = 2, // Contado o Efectivo
        Check  = 3, // Con cheque
        Mixed  = 4, //Efectivo y Tarjeta
        Credit = 5  //Acuerdo de credito
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