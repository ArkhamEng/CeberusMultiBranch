using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class SaleCancelViewModel
    {
        public int SaleCancelId { get; set; }

        [Display(Name ="Folio de venta")]
        public string SaleFolio { get; set; }

        [Required(ErrorMessage ="Debes agregar un comentario")]
        [Display(Name ="Comentario")]
        public string CancelComment { get; set; }


        [Display(Name = "Pagos Efectivo")]
        [DataType(DataType.Currency)]
        public double PaymentCash { get; set; }

        [Display(Name = "Pagos Tarjeta")]
        [DataType(DataType.Currency)]
        public double PaymentCard { get; set; }

        [Display(Name = "Pagos Vales")]
        [DataType(DataType.Currency)]
        public double PaymentCreditNote { get; set; }
    }


    public class PurchaseCancelViewModel
    {
        public int PurchaseCancelId { get; set; }

        [Display(Name = "Folio de venta")]
        public string PurchaseBill { get; set; }

        [Required(ErrorMessage = "Debes agregar un comentario")]
        [Display(Name = "Comentario")]
        public string CancelComment { get; set; }


        [Display(Name = "Pagos Efectivo")]
        [DataType(DataType.Currency)]
        public double PaymentCash { get; set; }

        [Display(Name = "Pagos Tarjeta")]
        [DataType(DataType.Currency)]
        public double PaymentCard { get; set; }

        [Display(Name = "Pagos Vales")]
        [DataType(DataType.Currency)]
        public double PaymentCreditNote { get; set; }
    }
}