using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("ExternalProduct", Schema = "Catalog")]
    public class ExternalProduct
    {
        public int ExternalProductId { get; set; }

        [Index("IDX_ProviderId", IsUnique = false)]
        public int ProviderId { get; set; }

        [Index("IDX_ProductId", IsUnique = false)]
        public int? ProductId { get; set; }

        [Display(Name = "Código")]
        [Required]
        [MaxLength(30)]
        [Index("IDX_Code", IsUnique = false)]
        public string Code { get; set; }

        [Display(Name = "Categoría")]
        [Required]
        [MaxLength(60)]
        [Index("IDX_Category", IsUnique = false)]
        public string Category { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(200)]
        [Index("IDX_Descripction", IsUnique = false)]
        public string Description { get; set; }


        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Display(Name = "Marca")]
        [MaxLength(50)]
        public string TradeMark { get; set; }

        [Display(Name = "Unidad")]
        [MaxLength(20)]
        public string Unit { get; set; }

        public virtual Provider Provider { get; set; }

        public virtual Product Product { get; set; }

       // public ICollection<Equivalence> Equivalences { get; set; }
    }
}