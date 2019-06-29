using CerberusMultiBranch.Models.Entities.Operative;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class ChoosePaymentViewModel
    {
        public int SaleId { get; set; }

        public string User { get; set; }

        public string Delivery { get; set; }

        public string Client { get; set; }

        [DataType(DataType.Currency)]
        public double CashAmount { get; set; }

        [DataType(DataType.Currency)]
        public double AmountToPay { get; set; }

        [DataType(DataType.Currency)]
        public double CardAmount { get; set; }

        [DataType(DataType.Currency)]
        public double CreditNoteAmount { get; set; }

      
        public string Reference { get; set; }

        public int CreditNoteId { get; set; }

        public string CreditNoteFolio { get; set; }

        public string Folio { get; set; }

        public string Expiration { get; set; }

        public int PrintType { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }

        public ICollection<SaleDetail> Details { get; set; }

        public double RefundCash { get; set; }

        public double RefundCredit { get; set; }

        [MaxLength(15, ErrorMessage = "este campo no puede exceder de 15 caractéres")]
        [Required(ErrorMessage = "Es necesario ingrese el número de IFE para generar nota de credito")]
        public string Ident { get; set; }

        [Required(ErrorMessage = "Escribe el nombre de quien recibe")]
        public string ReceivedBy { get; set; }

        public ChoosePaymentViewModel()
        {
            this.PaymentMethod = PaymentMethod.Efectivo;
            this.PaymentMethods = new List<PaymentMethod>();
            this.Details = new List<SaleDetail>();
        }
    }
}