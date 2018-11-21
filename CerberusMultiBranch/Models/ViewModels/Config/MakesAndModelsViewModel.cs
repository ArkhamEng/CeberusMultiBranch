using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Config
{
    public class MakesAndModelsViewModel
    {
        public List<CarMake> CarMakes { get; set; }

        public List<CarModel> CarModels { get; set; }
    }

    [NotMapped]
    public class CarModelViewModel:CarModel
    {
        public SelectList CarMakes { get; set; }

        [Required(ErrorMessage ="Debes seleccionar una Armadora")]
        [Display(Name="Armadora")]
        public int CarModelMakeId { get { return this.CarMakeId; } }
    }
}