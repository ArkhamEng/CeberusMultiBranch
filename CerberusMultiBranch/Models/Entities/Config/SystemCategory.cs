using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("SystemCategory", Schema = "Config")]
    public class SystemCategory
    {
        [Column(Order = 0), Key, ForeignKey("System")]
        public int PartSystemId { get; set; }

        [Column(Order = 1), Key, ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        [MaxLength(100)]
        public string UpdUser { get; set; }

        public virtual PartSystem System { get; set; }

        public virtual Category Category { get; set; }
    }
}