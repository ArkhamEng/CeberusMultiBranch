using CerberusMultiBranch.Models.Entities.Config;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using CerberusMultiBranch.Models.Entities.Operative;
using System;
using CerberusMultiBranch.Support;


namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("Product", Schema = "Catalog")]
    public class Product
    {
        public int ProductId { get; set; }

        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }

        [Display(Name = "Sistema")]
        public int? PartSystemId { get; set; }

        [Display(Name = "Código")]
        [Required]
        [MaxLength(30)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El código solo admite letras y numeros (sin espacios en blanco)")]
        [Index("UNQ_Code", IsUnique = true)]
        [Index("IDX_Ident_TradeMark", Order =0)]
        public string Code { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(200)]
        [Index("IDX_Ident_TradeMark", Order = 1)]
        [Required(ErrorMessage ="Se requiere una descripción del producto")]
        public string Name { get; set; }


        [Display(Name = "Cantidad Minima")]
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "La cantidad minima debe ser mayor a cero")]
        public double MinQuantity { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "La cantidad máxima debe ser mayor a cero")]
        public double MaxQuantity { get; set; }


        [Display(Name = "Código de barras")]
        public string BarCode { get; set; }


        [Required]
        public ProductType ProductType { get; set; }


        [Display(Name = "Fabricante")]
        [MaxLength(50)]
        [Index("IDX_Ident_TradeMark", Order = 2)]
        public string TradeMark { get; set; }

        [Display(Name = "Unidad")]
        [MaxLength(20)]
        public string Unit { get; set; }


        public bool IsActive { get; set; }


        public bool StockRequired { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        [MaxLength(100)]
        public string UpdUser { get; set; }

        [Index("IDX_Lock",Order =0)]
        public DateTime? LockDate { get; set; }

        [Index("IDX_Lock",Order =1)]
        [MaxLength(30)]
        public string UserLock { get; set; }


        public virtual Category Category { get; set; }

        public virtual PartSystem System { get; set; }

        public ICollection<ProductImage> Images { get; set; }

        public ICollection<Compatibility> Compatibilities { get; set; }

        public ICollection<SaleDetail> TransactionDetails { get; set; }

        public ICollection<BranchProduct> BranchProducts { get; set; }

        public ICollection<Equivalence> Equivalences { get; set; }

        //[InverseProperty("Detail")]
        //public virtual ICollection<PackageDetail> Details { get; set; }


        [InverseProperty("Package")]
        public virtual ICollection<PackageDetail> PackageDetails { get; set; }

        #region NotMapped Properties

        [NotMapped]
        [Display(Name = "Precio de Compra")]
        [DataType(DataType.Currency)]
        public double BuyPrice { get; set; }

        [NotMapped]
        [Display(Name = "% Mostrador")]
        [Required]
        public int StorePercentage { get; set; }

        [NotMapped]
        [Display(Name = "% Distribuidor")]
        [Required]
        public int DealerPercentage { get; set; }

        [NotMapped]
        [Display(Name = "% Mayorista")]
        [Required]
        [Range(15, 100, ErrorMessage = "El porcentaje de mayoreo debe ser mayor o igual a 15%")]
        public int WholesalerPercentage { get; set; }

        [NotMapped]
        [Display(Name = "Precio Mostrador")]
        [DataType(DataType.Currency)]
        [Required]
        public double StorePrice { get; set; }

        [NotMapped]
        [Display(Name = "Precio Mayorista")]
        [DataType(DataType.Currency)]
        [Required]
        public double WholesalerPrice { get; set; }

        [NotMapped]
        [Display(Name = "Precio Distribuidor")]
        [DataType(DataType.Currency)]
        [Required]
        public double DealerPrice { get; set; }

        [NotMapped]
        [Display(Name = "Fila")]
        [MaxLength(30)]
        [Required(ErrorMessage ="Se requiere la fila")]
        public string Row { get; set; }

        [NotMapped]
        [Display(Name = "Anaquel")]
        [MaxLength(30)]
        [Required(ErrorMessage = "Se requiere el anaquel")]
        public string Ledge { get; set; }

        [NotMapped]
        public bool CanSell
        {
            get; set;
        }

        [NotMapped]
        public bool IsLocked
        {
            get
            {
                if (LockDate != null)
                {
                   var t = LockDate.Value.AddMinutes(Cons.LockTimeOut);
                    if (DateTime.Now.ToLocal() <= t)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        [NotMapped]
        public bool StockLocked { get; set; }

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

        [NotMapped]
        public string ProviderCode { get; set; }

        public Product()
        {
            this.NewCompatibilities = new List<string>();

        }
        #endregion
    }

    public enum ProductType : int
    {
        Single = 0,
        Package = 1
    }

}