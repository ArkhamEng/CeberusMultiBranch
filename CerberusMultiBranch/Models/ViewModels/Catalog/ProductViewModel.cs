using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
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

        public SelectList Systems { get; set; }

        public SelectList CarMakes { get; set; }

        public SelectList CarModels { get; set; }


        public ProductViewModel()
        {
            this.Categories    = new List<Category>().ToSelectList();
            this.Images        = new List<ProductImage>();
            this.Compatibilities = new List<Compatibility>();
            this.CarMakes = new List<CarMake>().ToSelectList();
            this.CarModels = new List<CarModel>().ToSelectList();
            this.ModelCompatibilities = new List<CarModel>();
            this.BranchProducts = new List<BranchProduct>();
            this.Systems = new List<PartSystem>().ToSelectList();
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
            this.Images = product.Images;
            this.MinQuantity = product.MinQuantity;
            this.StorePercentage = product.StorePercentage;
            this.StorePrice = product.StorePrice;
            this.WholesalerPercentage = product.WholesalerPercentage;
            this.WholesalerPrice = product.WholesalerPrice;
            this.TradeMark = product.TradeMark;
            this.Unit = product.Unit;
            this.Compatibilities = product.Compatibilities;
            this.BranchProducts = product.BranchProducts;
            this.PartSystemId = product.PartSystemId;
            this.Row = product.Row;
            this.Ledge = product.Ledge;
            this.Categories = new List<Category>().ToSelectList();
            this.CarMakes = new List<CarMake>().ToSelectList();
            this.CarModels = new List<CarModel>().ToSelectList();
            this.ModelCompatibilities = new List<CarModel>();
            this.Systems = new List<PartSystem>().ToSelectList();

        }
    }
} 