using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class RefundViewModel
    {
        [DataType(DataType.Currency)]
        public double RefundCash { get; set; }

        [DataType(DataType.Currency)]
        public double RefundCredit { get; set; }

        public double TotalRefund { get { return RefundCash + RefundCredit; } }

        public int RefundSaleId { get; set; }

        public string RefundClient { get; set; }


        [MaxLength(15,ErrorMessage ="este campo no puede exceder de 15 caractéres")]
        [Required(ErrorMessage ="Es necesario ingrese el número de IFE para generar nota de credito")]
        public string Ident { get; set; }

        

        public int RefundClientId { get; set; }


    }
}