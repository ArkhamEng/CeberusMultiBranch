using CerberusMultiBranch.Models.Entities.Purchasing;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    [NotMapped]
    public class PurchaseCartViewModel
    {
        public int ProviderId { get; set; }

        [Display(Name ="Proveedor")]
        public string ProviderName { get; set; }

        [Display(Name = "Tipo de Compra")]
        public PType PuschaseType { get; set; }

        public SelectList PurchaseTypes { get; set; }

        public IEnumerable<ProductViewModel> PurchaseItems { get; set; }

        public PurchaseCartViewModel()
        {
            this.PurchaseItems = new List<ProductViewModel>();
        }
    }
}