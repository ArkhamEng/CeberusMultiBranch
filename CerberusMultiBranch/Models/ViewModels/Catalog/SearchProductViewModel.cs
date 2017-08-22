using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Support;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    public class SearchProductViewModel
    {
        public SelectList Categories { get; set; }

        public SelectList SubCategories { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public SearchProductViewModel()
        {
            Categories    = new List<Category>().ToSelectList();
            SubCategories = new List<SubCategory>().ToSelectList();
            Products      = new List<Product>();
        }
    }
}