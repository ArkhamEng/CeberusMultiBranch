using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("ProductImage", Schema = "Catalog")]
    public class ProductImage
    {
        public int ProductImageId { get; set; }

        public int ProductId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Path { get; set; }

        [Required]
        [MaxLength(30)]
        public string Type { get; set; }

        public int Size { get; set; }

        public virtual Product Product { get; set; }
    }
}