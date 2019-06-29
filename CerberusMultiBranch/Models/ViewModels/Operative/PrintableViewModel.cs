using CerberusMultiBranch.Models.Entities.Operative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class PrintableViewModel
    {
        public bool HasCreditNote { get { return (SaleCreditNote != null); } }

        public bool HasSale { get { return (Sale != null); } }

        public int PrintType { get; set; }

        public PrintRefundViewModel PrintRefund { get; set; }

        public Sale Sale { get; set; }

        public SaleCreditNote SaleCreditNote { get; set; }
    }
}