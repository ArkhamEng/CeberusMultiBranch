using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Inventory;
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

        public SelectList SubCategories { get; set; }

        [Display(Name="Categoría")]
        public int CategoryId { get; set; }

        public ProductViewModel()
        {
            this.Categories    = new List<Category>().ToSelectList();
            this.SubCategories = new List<SubCategory>().ToSelectList();
            this.Images        = new List<ProductImage>();
        }

        public ProductViewModel(Product product)
        {
            this.ProductId = product.ProductId;
            this.Name = product.Name;
            this.BarCode = product.BarCode;
            this.BuyPrice = product.BuyPrice;
            this.CategoryId = product.SubCategory.CategoryId;
            this.Code = product.Code;
            this.DealerPercentage = product.DealerPercentage;
            this.DealerPrice = product.DealerPrice;
            this.Description = product.Description;
            this.Images = product.Images;
            this.MinQuantity = product.MinQuantity;
            this.StorePercentage = product.StorePercentage;
            this.StorePrice = product.StorePrice;
            this.SubCategory = product.SubCategory;
            this.SubCategoryId = product.SubCategoryId;

            this.Categories = new List<Category>().ToSelectList();
            this.SubCategories = new List<SubCategory>().ToSelectList();

        }
    }
} 