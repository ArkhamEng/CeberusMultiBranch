using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Config
{
    public class SystemCategoryViewModel
    {
        [Display(Name="Categorías disponibles")]
        public int SelectedCategoryId { get; set; }

        public SelectList AvailableCategories { get; set; }

        public List<Category> AssignedCategories { get; set; }
    }
}