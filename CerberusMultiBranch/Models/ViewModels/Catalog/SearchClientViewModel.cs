using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    public class SearchClientViewModel
    {
        public SelectList States { get; set; }

        public SelectList Cities { get; set; }

        public IEnumerable<Client> Clients { get; set; }

        public SearchClientViewModel()
        {
            this.States  = new List<State>().ToSelectList();
            this.Cities  = new List<City>().ToSelectList();
            this.Clients = new List<Client>();
        }
    }
}