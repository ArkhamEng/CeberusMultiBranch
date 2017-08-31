using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    public class SearchProductViewModel
    {
        public SelectList Categories { get; set; }

        public SelectList Makes { get; set; }

        public SelectList Models { get; set; }

        public SelectList Years { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public SearchProductViewModel()
        {
            Categories    = new List<Category>().ToSelectList();
            Makes = new List<Make>().ToSelectList();
            Models = new List<Model>().ToSelectList();
            Years = new List<Year>().ToSelectList();
            Products      = new List<Product>();
        }
    }
}