using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class BeginPurchaseViewModel
    {
        [Required(ErrorMessage = "debes seleccionar un proveedor registrado")]
        public int ProviderId { get; set; }

        [Required(ErrorMessage = "debes seleccionar un proveedor registrado")]
        [Display(Name = "Nombre del Proveedor")]
        public string ProviderName { get; set; }

        [Display(Name = "Factura")]
        [Required(ErrorMessage = "debes ingresar un numero de factura válido")]
        public string Bill { get; set; }

        [Display(Name ="Fecha de Compra")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PurchaseDate { get; set; }

       
        [Display(Name ="Dias de crédito")]
        [Required(ErrorMessage ="es necesario indicar los días de crédito")]
        public int Days { get; set; }

        [Required(ErrorMessage = "Se require el tipo de compra")]
        [Display(Name = "Tipo de compra")]
        public TransactionType TransactionType { get; set; }

        public List<TransactionType> TransactionTypes { get; set; }

        [Display(Name ="Descuento", Prompt ="Porcentaje de descuento")]
        [Range(0,100,ErrorMessage ="el porcentaje de descuento debe ir del 0% al 100%")]
        public double Discount { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage ="Es necesario un motivo para aplicar el descuento")]
        [Display(Name ="Motivo de descuento")]
        public string Motive { get; set; }

        public BeginPurchaseViewModel()
        {
            this.TransactionType = new TransactionType();

            this.TransactionTypes = new List<TransactionType>();
            this.TransactionTypes.Add(TransactionType.Cash);
            this.TransactionTypes.Add(TransactionType.Credit);

            this.TransactionType = TransactionType.Cash;

            this.PurchaseDate = DateTime.Today;
        }

    }


    public class PurchaseEditViewModel:BeginPurchaseViewModel
    {
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}