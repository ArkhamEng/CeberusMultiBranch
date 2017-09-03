using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("Compatibility", Schema = "Catalog")]
    public class Compatibility
    {
        public int CompatibilityId { get; set; }

        public int CarYearId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual CarYear CarYear { get; set; }
    }

}