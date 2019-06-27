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

        [StringLength(15, MinimumLength = 1, ErrorMessage = "Solo se permiten 15 caractéres")]
        [Required(ErrorMessage ="Es necesario ingrese el número de IFE para generar nota de credito")]
        public string Ident { get; set; }

        [Display(Name ="Recibe")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Se requiren por lo menos 10 caractéres")]
        [Required(ErrorMessage = "Se requiere el nombre de la persona que recibe el rembolso")]
        public string ReceivedBy { get; set; }

        public int RefundClientId { get; set; }


    }
}