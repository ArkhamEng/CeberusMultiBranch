using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("TempExternalProduct", Schema = "Catalog")]
    public class TempExternalProduct
    {
        [Index("IDX_ProviderId", IsUnique = false)]
        [Column(Order = 0), Key]
        public int ProviderId { get; set; }

        [Index("IDX_Code", IsUnique = false)]
        [Column(Order = 1), Key]
        public string Code { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string TradeMark { get; set; }

        public string Unit { get; set; }
    }
}