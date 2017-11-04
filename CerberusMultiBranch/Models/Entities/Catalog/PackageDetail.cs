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
        public int PackageDetailId { get; set; }

        [ForeignKey("Package")]
        public int PackageId { get; set; }

        [ForeignKey("Detail")]
        public int DetailtId { get; set; }

        public double Quantity { get; set; }

        public virtual Product Package { get; set; }

        public virtual Product Detail { get; set; }

    }

}