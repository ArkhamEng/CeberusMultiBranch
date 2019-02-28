using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Purchasing
{
    public class BillingViewModel
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage ="Es necesario un numero de factura")]
        [Display(Name ="Número de factura")]
        public string BillNumber { get; set; }

        [DataType(DataType.Currency)]
        public double BillTotal { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BeginDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }


        [Display(Name = "Observaciones")]
        [DataType(DataType.MultilineText)]
        public string BillComment { get; set; }

    }
}