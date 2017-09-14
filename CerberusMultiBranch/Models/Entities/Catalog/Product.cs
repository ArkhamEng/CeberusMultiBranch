using CerberusMultiBranch.Models.Entities.Config;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using CerberusMultiBranch.Models.Entities.Operative;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("Product", Schema = "Catalog")]
    public class Product
    {
        public int ProductId { get; set; }

        [Display(Name="Categoría")]
        public int CategoryId { get; set; }

        [Display(Name = "Código")]
        [Required]
        [MaxLength(10)]
        [Index("IDX_Code", IsUnique = true)]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(100)]
        [Index("IDX_Name", IsUnique = true)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name="Cantidad Minima")]
        public double MinQuantity { get; set; }

        [Display(Name = "Código de barras")]
        public string BarCode { get; set; }

        [Display(Name = "Precio de Compra")]
        [DataType(DataType.Currency)]
        
        public double BuyPrice { get; set; }

        [Display(Name = "% Mostrador")]
        public int StorePercentage { get; set; }

        [Display(Name = "% Distribuidor")]
        public int DealerPercentage { get; set; }

        [Display(Name = "% Mayorista")]
        public int WholesalerPercentage { get; set; }

        [Display(Name = "Precio Mostrador")]
        [DataType(DataType.Currency)]
        public double StorePrice { get; set; }

        [Display(Name = "Precio Mayorista")]
        [DataType(DataType.Currency)]
        public double WholesalerPrice { get; set; }
        

        [Display(Name = "Precio Distribuidor")]
        [DataType(DataType.Currency)]
        public double DealerPrice { get; set; }

        [DataType(DataType.Currency)]
        public double MinimunPrice { get; set; }

        [Display(Name = "Fabricante")]
        public string TradeMark { get; set; }

        [Display(Name = "Unidad")]
        public string Unit { get; set; }

        public virtual Category Category { get; set; }

        public ICollection<ProductImage> Images { get; set; }

        public ICollection<Compatibility> Compatibilities { get; set; }

        public ICollection<ProductInventory> ProductInventories { get; set; }

        #region NotMapped Properties
        [NotMapped]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        [NotMapped]
        public List<CarModel> ModelCompatibilities { get; set; }

        [NotMapped]
        public List<string> NewCompatibilities { get; set; }
        #endregion
    }

}