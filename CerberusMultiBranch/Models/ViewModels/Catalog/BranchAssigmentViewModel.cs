using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    public class BranchAssigmentViewModel
    {
        public ICollection<Branch> Assigned { get; set; }

        public ICollection<Branch> NotAssigned { get; set; }
    }
}