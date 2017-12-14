using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("Category", Schema = "Config")]
    public class Category:ISelectable
    {
        public int CategoryId { get; set; }

        [Display(Name="Categoría")]
        [MaxLength(100)]
        [Required(ErrorMessage ="Se requiere una descripción para la categoría")]
        [Index("IDX_Name", IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(30)]
        [Required(ErrorMessage = "Clave SAT requerida")]
        [Index("IDX_SatCode", IsUnique = true)]
        [Display(Name = "Clave SAT")]
        public string SatCode { get; set; }

       
        [Display(Name = "% Comision")]
        [Required(ErrorMessage = "Se require un porcentaje de 0 a 100")]
        public int Commission { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string UpdUser { get; set; }


        public int Id { get { return CategoryId; } }

        public ICollection<Product> Products { get; set; }

        public ICollection<SystemCategory> SystemCategories { get; set; }
    }
}