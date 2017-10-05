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

        public SelectList Systems { get; set; }

        public SelectList Makes { get; set; }

        public SelectList Models { get; set; }

        public SelectList Years { get; set; }

        public IEnumerable<IEnumerable<Product>> Products { get; set; }

        public SearchProductViewModel()
        {
            Categories    = new List<Category>().ToSelectList();
            Makes = new List<CarMake>().ToSelectList();
            Models = new List<CarModel>().ToSelectList();
            Years = new List<CarYear>().ToSelectList();
            Products = new List<List<Product>>();
        }
    }
}