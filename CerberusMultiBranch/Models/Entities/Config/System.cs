using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("PartSystem", Schema = "Config")]
    public class PartSystem:ISelectable
    {
        public int PartSystemId { get; set; }

        [Display(Name = "Sistema")]
        public string Name { get; set; }

        public int Id { get { return PartSystemId; } }

        public ICollection<Product> Products { get; set; }
    }
}