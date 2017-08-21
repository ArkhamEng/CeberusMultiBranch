using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Common
{
    [Table("Category", Schema = "Common")]
    public class Category:ISelectable
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }

        public int Id { get { return CategoryId; } }
    }
}