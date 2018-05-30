using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class PrintRefundViewModel
    {
        public string Folio { get; set; }

        public string Date { get; set; }

        public Branch Branch { get; set; }

        public string   User { get; set; }

        public string Client { get; set; }

        public string Comment { get; set; }

        public string Cash { get; set; }

        public string   Note { get; set; }

        public bool HasNote { get; set; }

        public string Total { get; set; }

        public SaleCreditNote CreditNote { get; set; }
    }
}