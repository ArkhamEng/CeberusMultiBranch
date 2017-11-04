using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("PackageDetail", Schema = "Catalog")]
    public class PackageDetail
    {
        [Column(Order = 0), Key]
        public int PackageId { get; set; }

        [Column(Order = 1), Key]
        public int ProductId { get; set; }

        public double Quantity { get; set; }

        [ForeignKey("PackageId")]
        public Product Package { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }

}