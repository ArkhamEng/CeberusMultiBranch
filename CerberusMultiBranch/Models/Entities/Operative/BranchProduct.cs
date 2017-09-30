using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("BranchProduct", Schema = "Operative")]
    public class BranchProduct
    {
        [Column(Order = 0), Key, ForeignKey("Branch")]
        public int BranchId { get; set; }

        [Column(Order = 1), Key, ForeignKey("Product")]
        public int ProductId { get; set; }

        public double Stock { get; set; }

        public double LastStock { get; set; }

        public DateTime UpdDate { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Product Product { get; set; }
    }
}