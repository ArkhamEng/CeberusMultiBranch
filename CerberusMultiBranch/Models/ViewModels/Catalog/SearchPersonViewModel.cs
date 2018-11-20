using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    public class SearchPersonViewModel<T>
    {
        public SelectList States { get; set; }

        public SelectList Cities { get; set; }

        public IEnumerable<T> Persons { get; set; }

        public bool CreateDisabled
        {
            get { return false; }
        }

        public SearchPersonViewModel()
        {
            this.States  = new List<State>().ToSelectList();
            this.Cities  = new List<City>().ToSelectList();
            this.Persons = new List<T>();
        }
    }
}