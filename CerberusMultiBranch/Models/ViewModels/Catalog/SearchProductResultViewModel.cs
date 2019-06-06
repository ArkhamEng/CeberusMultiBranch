using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    public class SearchProductResultViewModel
    {
        public int ProductId { get; set; }

        [Display(Name="Código")]
        public string Code { get; set; }

        [Display(Name = "Descripción")]
        public string Name { get; set; }

        [Display(Name = "Marca")]
        public string TradeMark { get; set; }


        public string Image { get; set; }

        [Display(Name = "Disponibles")]
        public double Stock { get; set; }

        [Display(Name = "Precio Mostrador")]
        [DataType(DataType.Currency)]
        public double StorePrice { get; set; }


        [Display(Name = "Precio Distribuidor")]
        [DataType(DataType.Currency)]
        public double DealerPrice { get; set; }


        [Display(Name = "Precio Mayorista")]
        [DataType(DataType.Currency)]
        public double WholesalerPrice { get; set; }

        [Display(Name = "Mínimo")]
        public double MaxQuantity { get; set; }

        [Display(Name = "Máximo")]
        public double MinQuantity { get; set; }

        public double OrderQty { get; set; }

        public double SellQty { get; set; }

        public double SaleCommission { get; set; }
    }
}