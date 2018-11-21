using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("CarMake", Schema = "Config")]
    public class CarMake:ISelectable
    {
        public int CarMakeId { get; set; }

        [Display(Name = "Armadora")]
        [Required(ErrorMessage = "Se requiere un nombre de Armadora")]
        public string Name { get; set; }

        [MaxLength(100)]
        [Display(Name="Editado por")]
        public string UpdUser { get; set; }

        [Display(Name = "Editado")]
        public DateTime UpdDate { get; set; }

        [Display(Name = "Activo")]
        public bool  IsActive { get; set; }

        public ICollection<CarModel> Models { get; set; }

        [NotMapped]
        public int Id { get { return this.CarMakeId; } }

        public CarMake()
        {
            this.UpdDate = DateTime.Now.ToLocal();
            this.UpdUser = HttpContext.Current.User.Identity.Name;
            this.IsActive = true;
        }
    }

    [Table("CarModel", Schema = "Config")]
    public class CarModel : ISelectable
    {
        [NotMapped]
        public int Id { get { return this.CarModelId; } }

        [Display(Name="Marca")]
        [Index("IDX_CarMakeId", IsUnique = false)]
        public int CarMakeId { get; set; }

        public int CarModelId { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "Se requiere un Nombre de modelo")]
        public string Name { get; set; }

        [MaxLength(100)]
        [Display(Name = "Editado por")]
        public string UpdUser { get; set; }

        [Display(Name = "Editado")]
        public DateTime UpdDate { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        public virtual CarMake CarMake { get; set; }

        [Display(Name = "Años")]
        public ICollection<CarYear> CarYears { get; set; }

        public CarModel()
        {
            this.UpdDate = DateTime.Now.ToLocal();
            this.UpdUser = HttpContext.Current.User.Identity.Name;
            this.IsActive = true;
        }
    }

    [Table("CarYear", Schema = "Config")]
    public class CarYear : ISelectable
    {
        [NotMapped]
        public int Id { get { return this.CarYearId; } }

        [NotMapped]
        public string Name { get { return this.Year.ToString(); } }

        public int CarYearId { get; set; }

        [Index("IDX_CarModelId", IsUnique = false)]
        public int CarModelId { get; set; }

        [Display(Name = "Año")]
        public int Year { get; set; }

        public virtual CarModel CarModel { get; set; }

        public ICollection<Compatibility> Compatibilities { get; set; }
    }

}