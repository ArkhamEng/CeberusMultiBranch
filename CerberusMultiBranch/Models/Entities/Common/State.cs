using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Common
{
    [Table("State", Schema = "Common")]
    public class State : ISelectable
    {
        public int StateId { get; set; }

        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(50)]
        [Index("IDX_Name", IsUnique = true)]
        public string Name { get; set; }

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