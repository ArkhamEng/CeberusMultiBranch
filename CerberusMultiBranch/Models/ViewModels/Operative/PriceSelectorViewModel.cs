﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class PriceSelectorViewModel
    {
        public int PsProductId { get; set; }

        public int PsTransactionId { get; set; }

        public double SPrice { get; set; }

        public double DPrice { get; set; }

        public double WPrice { get; set; }

        public double CPrice { get; set; }

        public bool IsCart { get; set; }
    }

    public class QuantityChangeViewModel
    {
        public int cqProductId { get; set; }

        public double cqQuantity { get; set; }

        public string cqCode { get; set; }

        public  string cqUnit { get; set; }
    }
}