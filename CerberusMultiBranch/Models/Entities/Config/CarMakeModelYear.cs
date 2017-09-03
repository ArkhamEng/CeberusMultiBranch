using CerberusMultiBranch.Models.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("CarMake", Schema = "Config")]
    public class CarMake:ISelectable
    {
        public int CarMakeId { get; set; }

        [Display(Name = "Marca")]
        public string Name { get; set; }

        public ICollection<CarModel> Models { get; set; }

        [NotMapped]
        public int Id { get { return this.CarMakeId; } }
     
    }

    [Table("CarModel", Schema = "Config")]
    public class CarModel : ISelectable
    {
        [NotMapped]
        public int Id { get { return this.CarModelId; } }

        [Display(Name="Marca")]
        public int CarMakeId { get; set; }

        public int CarModelId { get; set; }

        [Display(Name = "Modelo")]
        public string Name { get; set; }

        public virtual CarMake CarMake { get; set; }

        [Display(Name = "Años")]
        public ICollection<CarYear> CarYears { get; set; }

     
    }

    [Table("CarYear", Schema = "Config")]
    public class CarYear : ISelectable
    {
        [NotMapped]
        public int Id { get { return this.CarYearId; } }

        [NotMapped]
        public string Name { get { return this.Year.ToString(); } }

        public int CarYearId { get; set; }

        public int CarModelId { get; set; }

        [Display(Name = "Año")]
        public int Year { get; set; }

        public virtual CarModel CarModel { get; set; }
    }

    [Serializable()]
    public class TestYear
    {
        public string CarModelId { get; set; }

        
        public string Year { get; set; }
    }
}