using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Inventory
{
    [Table("ProductInBranch", Schema = "Inventory")]
    public class ProductInBranch
    {
        public int ProductInBranchId { get; set; }

        public int ProductId { get; set; }

        public int BranchId { get; set; }

        public double Quantity { get; set; }

        public virtual  Product Product { get; set; }

    }
}