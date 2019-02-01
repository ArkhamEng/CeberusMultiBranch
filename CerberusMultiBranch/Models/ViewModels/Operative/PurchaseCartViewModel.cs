using CerberusMultiBranch.Models.Entities.Purchasing;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Support;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    [NotMapped]
    public class PurchaseCartViewModel
    {
        public int ProviderId { get; set; }

        [Display(Name ="Proveedor")]
        public string ProviderName { get; set; }

        [Display(Name = "Observaciones")]
        public string Comment { get; set; }

        [Display(Name = "Días de Crédito")]
        public double DaysToPay { get; set; }

        [Display(Name = "Tipo de Compra")]
        public PType PuschaseType { get; set; }

        public SelectList PurchaseTypes { get; set; }

        public bool SearchProviderDisabled
        {
            get { return (this.PurchaseItems.Count() > Cons.Zero); }
        }


        public bool ActionsDisabled
        {
            get { return (this.PurchaseItems.Count() == Cons.Zero); }
        }

        public IEnumerable<ProductViewModel> PurchaseItems { get; set; }

        public PurchaseCartViewModel()
        {
            this.PurchaseItems = new List<ProductViewModel>();
        }
    }
}