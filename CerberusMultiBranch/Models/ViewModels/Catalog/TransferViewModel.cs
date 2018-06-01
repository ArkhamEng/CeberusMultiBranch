using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    public class TransferViewModel
    {
        public int TransProductId { get; set; }

        public double TransStock { get; set; }

        public string TransBranch { get; set; }

        public int TransBranchId { get; set; }

        public string TransCode { get; set; }

        public string TransName { get; set; }

        public string TransUnit { get; set; }

        public string TransImage { get; set; }
    }
}