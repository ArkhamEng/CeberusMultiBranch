using CerberusMultiBranch.Models.Entities.Catalog;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace CerberusMultiBranch.Models.Entities.Common
{
    [Table("SubCategory", Schema = "Common")]
    public class SubCategory:ISelectable
    {
        public int SubCategoryId { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string UnitMeasure { get; set; }

        public virtual Category Category { get; set; }

        public ICollection<Product> Products { get; set; }

        public int Id { get { return SubCategoryId; } }
      
    }
}