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
        [Display(Name ="Nombre del Proveedor")]
        public string ProviderName { get; set; }

        [Display(Name = "Factura")]
        [Required]
        public string Bill { get; set; }

        [Display(Name ="Fecha de Compra")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PurchaseDate { get; set; }

       
        [Display(Name ="Dias de crédito")]
        [Required]
        public int Days { get; set; }

        [Required(ErrorMessage = "Se require el tipo de compra")]
        [Display(Name = "Tipo de compra")]
        public TransactionType TransactionType { get; set; }

        public List<TransactionType> TransactionTypes { get; set; }

        public BeginPurchaseViewModel()
        {
            this.TransactionType = new TransactionType();

            this.TransactionTypes = new List<TransactionType>();
            this.TransactionTypes.Add(TransactionType.Contado);
            this.TransactionTypes.Add(TransactionType.Credito);

            this.TransactionType = TransactionType.Contado;

            this.PurchaseDate = DateTime.Today;
        }

    }


    public class PurchaseEditViewModel:BeginPurchaseViewModel
    {
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}