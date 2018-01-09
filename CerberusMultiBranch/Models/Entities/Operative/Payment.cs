using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public enum PaymentType
    {
        Ninguno   = 0,
        Tarjeta   = 1, //Tarjeda Credito o débito
        Efectivo  = 2, // Efectivo
        Cheque    = 3, // Con cheque
        Mixto     = 4, //Efectivo y Tarjeta
        Credito   = 5,  //Acuerdo de credito
        Contado   = 6
    }


    [Table("Payment", Schema = "Operative")]
    public class Payment
    {
        public int PaymentId { get; set; }

        public int SaleId { get; set; }

        public double Amount { get; set; }

        public PaymentType PaymentType { get; set; }

        public DateTime PaymentDate { get; set; }

        public virtual Sale Sale { get; set; }

    }
}