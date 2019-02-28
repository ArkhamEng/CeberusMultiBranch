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
        public int DaysToPay { get; set; }

        [Display(Name = "Tipo de Compra")]
        public PType PurchaseType { get; set; }

        [Display(Name = "Método de envío")]
        public int ShipmentMethodId { get; set; }

        [Display(Name = "Costo de envío")]
        public double Freight { get; set; }

        [Display(Name = "Descuento Global")]
        public double Discount { get; set; }

        [Display(Name = "Seguro")]
        public double Insurance { get; set; }

     
        public SelectList PurchaseTypes { get; set; }

     
        public SelectList ShipmentMethodes { get; set; }

        public SelectList Branches { get; set; }

        [Display(Name ="Sucursal")]
        public int BranchId { get; set; }

        public bool SearchProviderDisabled
        {
            get { return (this.PurchaseItems.Count() > Cons.Zero); }
        }


        public bool ActionsDisabled
        {
            get { return (this.PurchaseItems.Count() == Cons.Zero); }
        }

        public Dictionary<string,IEnumerable<ProductViewModel>> PurchaseItems { get; set; }

        public PurchaseCartViewModel()
        {
            this.PurchaseItems = new Dictionary<string, IEnumerable<ProductViewModel>>();
        }
    }
}