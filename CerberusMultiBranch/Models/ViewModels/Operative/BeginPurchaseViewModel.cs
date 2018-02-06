using CerberusMultiBranch.Models.Entities.Operative;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class BeginPurchaseViewModel
    {
        public int ProviderId { get; set; }

        [Required]
        public string ProviderName { get; set; }

        [Display(Name = "Factura")]
        [Required]
        public string Bill { get; set; }

        [Display(Name ="Fecha de Compra")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PurchaseDate { get; set; }

        [Display(Name = "Fecha limite de pago")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpirationDate { get; set; }

        [Required(ErrorMessage = "Se require el tipo de compra")]
        [Display(Name = "Tipo de compra")]
        public TransactionType TransactionType { get; set; }

        public List<TransactionType> TransactionTypes { get; set; }

    }


    public class PurchaseEditViewModel:BeginPurchaseViewModel
    {
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}