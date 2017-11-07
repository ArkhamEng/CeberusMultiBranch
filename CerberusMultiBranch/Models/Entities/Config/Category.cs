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
        [Index("IDX_Name", IsUnique = false)]
        public string Name { get; set; }

        [MaxLength(30)]
        [Display(Name = "Clave SAT")]
        public string SatCode { get; set; }

       
        [Display(Name = "% Comision")]
        public int Commission { get; set; }


        public int Id { get { return CategoryId; } }
    }
}