using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    
    public class SearchProviderViewModel
    {
        public SelectList States { get; set; }

        public SelectList Cities { get; set; }

        public IEnumerable<Provider> Providers { get; set; }

        public SearchProviderViewModel()
        {
            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
            this.Providers = new List<Provider>();
        }
    }
}