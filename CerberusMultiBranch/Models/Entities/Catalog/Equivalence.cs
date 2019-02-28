using CerberusMultiBranch.Support;
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

        [Index("IDX_Provider_Product", Order = 0, IsUnique = false)]
        public int ProviderId { get; set; }

        [ForeignKey("Product")]
        [Index("IDX_Provider_Product", Order = 1, IsUnique = false)]
        public int ProductId { get; set; }

        [Index("IDX_Code", IsUnique = false)]
        [MaxLength(30)]
        public string  Code { get; set; }

        

        public double BuyPrice { get; set; }

        public bool IsDefault { get; set; }

        public DateTime InsDate { get; set; }

        public string  InsUser { get; set; }

        public DateTime UpdDate { get; set; }

        public string UpdUser { get; set; }


        public virtual Product Product { get; set; }

        public virtual Provider Provider { get; set; }

        [NotMapped]
        public  ExternalProduct ExternalProduct { get; set; }

        public Equivalence()
        {
            this.InsDate = DateTime.Now.ToLocal();
            this.InsUser = HttpContext.Current.User.Identity.Name;
            this.UpdDate = DateTime.Now.ToLocal();
            this.UpdUser = HttpContext.Current.User.Identity.Name;
        }
    }
}