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
        [Index("IDX_CategoryId", IsUnique = false)]
        public int CategoryId { get; set; }

        [Display(Name = "Sistema")]
        [Index("IDX_PartSystemId", IsUnique = false)]
        public int? PartSystemId { get; set; }

        [Display(Name = "Código")]
        [Required]
        [MaxLength(30)]
        [Index("IDX_Code", IsUnique = true)]
        public string Code { get; set; }

        //[Display(Name = "Referencia")]
        //[Required]
        //[MaxLength(100)]
        //public string Reference { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(200)]
        [Index("IDX_Name", IsUnique = false)]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name="Cantidad Minima")]
        [Required]
        public double MinQuantity { get; set; }

        [Display(Name = "Código de barras")]
        public string BarCode { get; set; }

        [Display(Name = "Precio de Compra")]
        [DataType(DataType.Currency)]
        public double BuyPrice { get; set; }

        [Display(Name = "% Mostrador")]
        [Required]
        public int StorePercentage { get; set; }

        [Display(Name = "% Distribuidor")]
        [Required]
        public int DealerPercentage { get; set; }

        [Display(Name = "% Mayorista")]
        [Required]
        public int WholesalerPercentage { get; set; }

        [Display(Name = "Precio Mostrador")]
        [DataType(DataType.Currency)]
        [Required]
        public double StorePrice { get; set; }

        [Display(Name = "Precio Mayorista")]
        [DataType(DataType.Currency)]
        [Required]
        public double WholesalerPrice { get; set; }
        

        [Display(Name = "Precio Distribuidor")]
        [DataType(DataType.Currency)]
        [Required]
        public double DealerPrice { get; set; }

     
        [Display(Name = "Fabricante")]
        [MaxLength(50)]
        public string TradeMark { get; set; }

        [Display(Name = "Unidad")]
        [MaxLength(20)]
        public string Unit { get; set; }

        public virtual Category Category { get; set; }

        public virtual PartSystem System { get; set; }

        public ICollection<ProductImage> Images { get; set; }

        public ICollection<Compatibility> Compatibilities { get; set; }

        public ICollection<TransactionDetail> TransactionDetails { get; set; }

        public ICollection<BranchProduct> BranchProducts { get; set; }

        #region NotMapped Properties
        [NotMapped]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        [NotMapped]
        public List<CarModel> ModelCompatibilities { get; set; }

        [NotMapped]
        public List<string> NewCompatibilities { get; set; }

        [NotMapped]
        public List<Branch> Branches { get; set; }

        [NotMapped]
        public double Quantity { get; set; }

        [NotMapped]
        public int TransactionId { get; set; }

        public Product()
        {
            this.NewCompatibilities = new List<string>();
        }
        #endregion
    }

}