using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("State", Schema = "Config")]
    public class State : ISelectable
    {
        public int StateId { get; set; }

        [MaxLength(15)]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [MaxLength(50)]
        [Index("IDX_Name", IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string ShorName { get; set; }

        public bool IsActive { get; set; }

        public ICollection<City> Cities { get; set; }

        [NotMapped]
        public int Id { get { return this.StateId; } }

        public State()
        {
            this.Cities = new List<City>();
        }
    }
}