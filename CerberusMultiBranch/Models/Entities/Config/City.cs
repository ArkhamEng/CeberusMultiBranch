using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("City", Schema = "Config")]
    public class City : ISelectable
    {
        public int CityId { get; set; }

        [Display(Name = "Estado")]
        [Index("IDX_StateId", IsUnique = false)]
        public int StateId { get; set; }

        [MaxLength(15)]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public virtual State State { get; set; }

        [NotMapped]
        public int Id { get { return this.CityId; } }

    }
}