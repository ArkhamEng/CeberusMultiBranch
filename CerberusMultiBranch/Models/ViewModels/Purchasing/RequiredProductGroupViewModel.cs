using CerberusMultiBranch.Models.ViewModels.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Purchasing
{
    public class RequiredProductGroupViewModel
    {
        public string Description { get; set; }

        public int   ProductId { get; set; }

        public string ProviderCode { get; set; }

        public string Code { get; set; }

        public string Unit { get; set; }

        public string TradeMark { get; set; }

        public IEnumerable<ProductViewModel> Branches { get; set; }
    }
}