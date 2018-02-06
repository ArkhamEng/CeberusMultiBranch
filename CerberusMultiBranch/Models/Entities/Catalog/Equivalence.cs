using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("Equivalence", Schema = "Catalog")]
    public class Equivalence
    {
        [Column(Order = 0),Key]
        public int EquivalenceId { get; set; }

        [Index("IDX_ProviderId", IsUnique = false)]
        public int ProviderId { get; set; }

        [Index("IDX_Code", IsUnique = false)]
        [MaxLength(30)]
        public string  Code { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

      
        public virtual Product Product { get; set; }

        [NotMapped]
        public  ExternalProduct ExternalProduct { get; set; }
    }
}