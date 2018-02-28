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
        [Index("IDX_ProviderId", IsUnique = false)]
        [Column(Order = 0), Key, ForeignKey("Provider")]
        public int ProviderId { get; set; }

        [Display(Name = "Código")]
        [Required]
        [MaxLength(30)]
        [Index("IDX_Code", IsUnique = false)]
        [Column(Order = 1), Key]
        public string Code { get; set; }


        [Display(Name = "Categoría")]
        [Required]
        [MaxLength(60)]
        [Index("IDX_Category", IsUnique = false)]
        public string Category { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(300)]
        [Index("IDX_Descripction", IsUnique = false)]
        public string Description { get; set; }


        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }

        [Display(Name = "Marca")]
        [MaxLength(50)]
        public string TradeMark { get; set; }

        [Display(Name = "Unidad")]
        [MaxLength(20)]
        public string Unit { get; set; }

        public virtual Provider Provider { get; set; }

        //public virtual Product Product { get; set; }
        [NotMapped]
        public Equivalence Equivalence { get; set; }

        [NotMapped]
        public string InternalCode { get; set; }

        [NotMapped]
        public int ProductId { get; set; }

        [NotMapped]
        public string ProviderName { get; set; }


    }
}