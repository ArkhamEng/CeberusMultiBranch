using CerberusMultiBranch.Models.Entities.Common;
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

        public virtual CarMake Make { get; set; }
    }

    [Table("CarYear", Schema = "Config")]
    public class CarYear : ISelectable
    {

        public int Id { get { return this.CarYearId; } }

        public int CarYearId { get; set; }

        public string Name { get; set; }
    }
}