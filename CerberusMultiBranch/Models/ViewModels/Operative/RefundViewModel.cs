using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class RefundViewModel
    {
        public double RefundCash { get; set; }

        public double RefundCredit { get; set; }

        public int RefundSaleId { get; set; }

        public string RefundClient { get; set; }

        public int RefundClientId { get; set; }


    }
}