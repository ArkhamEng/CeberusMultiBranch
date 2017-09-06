using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    [NotMapped]
    public class ProductViewModel:Product
    {
        public SelectList Categories { get; set; }

        public ProductViewModel()
        {
            this.Categories    = new List<Category>().ToSelectList();
            this.Images        = new List<ProductImage>();
        }

        public ProductViewModel(Product product)
        {
            this.ProductId = product.ProductId;
            this.Name = product.Name;
            this.CategoryId = product.CategoryId;
            this.BarCode = product.BarCode;
            this.BuyPrice = product.BuyPrice;
            this.Code = product.Code;
            this.DealerPercentage = product.DealerPercentage;
            this.DealerPrice = product.DealerPrice;
            this.Description = product.Description;
            this.Images = product.Images;
            this.MinQuantity = product.MinQuantity;
            this.StorePercentage = product.StorePercentage;
            this.StorePrice = product.StorePrice;
            this.TradeMark = product.TradeMark;
            this.Unit = product.Unit;
   
            this.Categories = new List<Category>().ToSelectList();
         
        }
    }
} 