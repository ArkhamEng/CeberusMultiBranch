using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    public class SearchEmployeeViewModel
    {
        public SelectList States { get; set; }

        public SelectList Cities { get; set; }

        public IEnumerable<Employee> Employees { get; set; }

        public SearchEmployeeViewModel()
        {
            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
            this.Employees = new List<Employee>();
        }
    }
}