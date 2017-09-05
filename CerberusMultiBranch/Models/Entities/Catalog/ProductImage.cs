using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
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

        public string Name { get; set; }

        public byte[] File { get; set; }

        public string Type { get; set; }

        public double Size { get; set; }

        public virtual Product Product { get; set; }
        [NotMapped]
        public byte[] ClearImage { get { return Support.GzipWrapper.Decompress(this.File); } }


    }
}