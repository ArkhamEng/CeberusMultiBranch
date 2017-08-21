using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Inventory;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("Product", Schema = "Catalog")]
    public class Product
    {
        public int ProductId { get; set; }

        [Display(Name="Sub Categoría")]
        public int SubCategoryId { get; set; }

        [Display(Name = "Código")]
        [Required]
        [MaxLength(10)]
        [Index("IDX_Code", IsUnique = true)]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(20)]
        [Index("IDX_Name", IsUnique = true)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name="Cantidad Minima")]
        public double MinQuantity { get; set; }

        public string BarCode { get; set; }

        [Display(Name = "Precio de Compra")]
        [DataType(DataType.Currency)]
        public double BuyPrice { get; set; }

        [Display(Name = "% Mostrador")]
        public int StorePercentage { get; set; }

        [Display(Name = "% Distribuidor")]
        public int DealerPercentage { get; set; }

        [Display(Name = "Precio Mostrador")]
        [DataType(DataType.Currency)]
        public double StorePrice { get; set; }

        [Display(Name = "Precio Distribuidor")]
        [DataType(DataType.Currency)]
        public double DealerPrice { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public ICollection<ProductImage> Images { get; set; }

        [NotMapped]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }


    }

   
}