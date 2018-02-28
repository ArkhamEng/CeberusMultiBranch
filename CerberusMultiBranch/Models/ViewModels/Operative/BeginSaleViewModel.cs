using CerberusMultiBranch.Models.Entities.Operative;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class BeginSaleViewModel
    {
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Nombre del Cliente")]
        public string ClientName { get; set; }


        [Display(Name = "Fecha de Venta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime SaleDate { get; set; }


        [Display(Name = "Dias de crédito")]
        [Required]
        [Range(1, 60,ErrorMessage ="el crédito debe estar entre 1 y 60 días")]
        public int Days { get; set; }

        [Required(ErrorMessage = "Se require el tipo de veta")]
        [Display(Name = "Tipo de venta")]
        public TransactionType TransactionType { get; set; }

        public List<TransactionType> TransactionTypes { get; set; }

        public BeginSaleViewModel()
        {
            this.TransactionType = TransactionType.Credito;

            TransactionTypes = new List<TransactionType>();
            this.TransactionTypes.Add(TransactionType.Credito);
            this.TransactionTypes.Add(TransactionType.Preventa);
            this.SaleDate = DateTime.Today;
        }
    }

    public class SaleEditViewModel : BeginSaleViewModel
    {
        public ICollection<SaleDetail> SaleDetails { get; set; }
    }
}