using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Support;
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

        [Required(ErrorMessage ="Se requiere un nombre de sistema")]
        [Display(Name = "Sistema")]
        [MaxLength(50)]
        [Index("IDX_Name", IsUnique = true)]
        public string Name { get; set; }

        [Display(Name = "% Comision")]
        [Required(ErrorMessage = "Se require un porcentaje de 0 a 100")]
        public int Commission { get; set; }

        [Required]
        [Display(Name = "Editado")]
        public DateTime UpdDate { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Editado por")]
        public string UpdUser { get; set; }

        [Display(Name ="Activo")]
        public bool IsActive { get; set; }

        public int Id { get { return PartSystemId; } }

        public ICollection<Product> Products { get; set; }

        public ICollection<SystemCategory> SystemCategories { get; set; }

        public PartSystem()
        {
            this.UpdDate = DateTime.Now.ToLocal();
            this.IsActive = true;
            this.UpdUser = HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : null;
           
        }

    }
}