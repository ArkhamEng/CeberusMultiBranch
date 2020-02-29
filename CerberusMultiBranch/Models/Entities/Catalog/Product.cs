using CerberusMultiBranch.Models.Entities.Config;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using CerberusMultiBranch.Models.Entities.Operative;
using System;
using CerberusMultiBranch.Support;
using CerberusMultiBranch.Models.Entities.Purchasing;

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
        //[Index("IDX_Ident_TradeMark", Order =0)]
        [Index("IDX_Code")]
        public string Code { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(300)]
        //[Index("IDX_Ident_TradeMark", Order = 1)]
        [Index("IDX_Name")]
        [Required(ErrorMessage ="Se requiere una descripción del producto")]
        public string Name { get; set; }


        [Display(Name = "Fabricante")]
        [MaxLength(50)]
        //[Index("IDX_Ident_TradeMark", Order = 2)]
        [Index("IDX_TradeMark")]
        public string TradeMark { get; set; }


        [Display(Name = "Nombre Corto")]
        [MaxLength(50)]
        public string ShortName { get; set; }

        [Display(Name = "Observaciones")]
        [MaxLength(300)]
        public string Comment { get; set; }


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


        [Display(Name = "Unidad")]
        [MaxLength(20)]
        public string Unit { get; set; }


        public bool IsActive { get; set; }

        public bool IsOnlineSold { get; set; }

        
        public int OnlinePercentage { get; set; }

        [DataType(DataType.Currency)]
        public double OnlinePrice { get; set; }


        public bool StockRequired { get; set; }

        [Display(Name = "Rastreable")]
        public bool IsTrackable { get; set; }

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

        public  ICollection<ProductImage> Images { get; set; }

        public ICollection<Compatibility> Compatibilities { get; set; }

        public ICollection<SaleDetail> SaleDetails { get; set; }

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }

        public ICollection<BranchProduct> BranchProducts { get; set; }

        public ICollection<Equivalence> Equivalences { get; set; }

        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; }

        public ICollection<TrackingItem> TrackingItems { get; set; }


        [InverseProperty("Package")]
        public virtual ICollection<PackageDetail> PackageDetails { get; set; }

        #region NotMapped Properties

        [NotMapped]
        [Display(Name = "Precio de Compra")]
        [DataType(DataType.Currency)]
        public double BuyPrice { get; set; }


        [NotMapped]
        [Display(Name = "Descuento")]
        public double Discount { get; set; }

        [NotMapped]
        [Display(Name = "% Mostrador")]
        [Required]
        [Range(20, 1000, ErrorMessage = "El porcentaje de mostrador debe ser mayor o igual a 20%")]
        public int StorePercentage { get; set; }

        [NotMapped]
        [Display(Name = "% Distribuidor")]
        [Required]
        [Range(15, 1000, ErrorMessage = "El porcentaje de distribuidor debe ser mayor o igual a 15%")]
        public int DealerPercentage { get; set; }

        [NotMapped]
        [Display(Name = "% Mayorista")]
        [Required]
        [Range(10, 1000, ErrorMessage = "El porcentaje de mayoreo debe ser mayor o igual a 10%")]
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
        public bool IsIncart
        {
            get;set;
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

        [NotMapped]
        public int FromProviderId { get; set; }

        public Product()
        {
            this.NewCompatibilities = new List<string>();
            this.UpdDate = DateTime.Now.ToLocal();
            this.UpdUser = HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : null;
        }

        public Product(Product product)
        {
            this.BarCode = product.BarCode;
            this.Branches = product.Branches;
            this.BranchProducts = product.BranchProducts;
            this.BuyPrice = product.BuyPrice;
            this.CanSell = product.CanSell;
            this.Category = product.Category;
            this.CategoryId = product.CategoryId;
            this.Code = product.Code;
            this.Comment = product.Comment;
            this.Compatibilities = product.Compatibilities;
            this.DealerPercentage = product.DealerPercentage;
            this.DealerPrice = product.DealerPrice;
            this.Discount = product.Discount;
            this.Equivalences = product.Equivalences;
            this.Files = product.Files;
            this.FromProviderId = product.FromProviderId;
            this.Images = product.Images;
            this.IsActive = product.IsActive;
            this.IsIncart = product.IsIncart;
            
            this.IsOnlineSold = product.IsOnlineSold;
            this.IsTrackable = product.IsTrackable;
            this.Ledge = product.Ledge;
            this.LockDate = product.LockDate;
            this.MaxQuantity = product.MaxQuantity;
            this.MinQuantity = product.MinQuantity;
            this.ModelCompatibilities = product.ModelCompatibilities;
            this.Name = product.Name;
            this.NewCompatibilities = product.NewCompatibilities;
            this.OnlinePercentage = product.OnlinePercentage;
            this.OnlinePrice = product.OnlinePrice;
            this.PackageDetails = product.PackageDetails;
            this.PartSystemId = product.PartSystemId;
            this.ProductId = product.ProductId;
            this.ProductType = product.ProductType;
            this.ProviderCode = product.ProviderCode;
            this.PurchaseDetails = product.PurchaseDetails;
            this.PurchaseItems = product.PurchaseItems;
            this.PurchaseOrderDetails = product.PurchaseOrderDetails;
            this.Quantity = product.Quantity;
            this.Row = product.Row;
            this.SaleDetails = product.SaleDetails;
            this.ShortName = product.ShortName;
            this.StockLocked = product.StockLocked;
            this.StockRequired = product.StockRequired;
            this.StorePercentage = product.StorePercentage;
            this.StorePrice = product.StorePrice;
            this.System = product.System;
            this.TrackingItems = product.TrackingItems;
            this.TradeMark = product.TradeMark;
            this.TransactionId = product.TransactionId;
            this.Unit = product.Unit;
            this.UpdDate = product.UpdDate;
            this.UpdUser = product.UpdUser;
            this.UserLock = product.UserLock;
            this.WholesalerPercentage = product.WholesalerPercentage;
            this.WholesalerPrice = product.WholesalerPrice;
        }
        #endregion
    }

    public enum ProductType : int
    {
        Single = 0,
        Package = 1
    }

}