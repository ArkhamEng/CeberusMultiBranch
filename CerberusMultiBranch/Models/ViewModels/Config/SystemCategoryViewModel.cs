using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Config
{
    public class SystemCategoryViewModel
    {
        public SelectList Systems { get; set; }

        public int PartSystemId { get; set; }

        public List<Category> AvailableCategories { get; set; }

        public List<Category> AssignedCategories { get; set; }
    }
}