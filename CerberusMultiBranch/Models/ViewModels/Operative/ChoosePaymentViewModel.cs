﻿using CerberusMultiBranch.Models.Entities.Operative;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class ChoosePaymentViewModel
    {
        public int SaleId { get; set; }

        [DataType(DataType.Currency)]
        public double CashAmount { get; set; }

        [DataType(DataType.Currency)]
        public double AmountToPay { get; set; }

        [DataType(DataType.Currency)]
        public double CardAmount { get; set; }

        
        public string Reference { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }

        public ICollection<SaleDetail> Details { get; set; }

        public ChoosePaymentViewModel()
        {
            this.PaymentMethod = PaymentMethod.Efectivo;
            this.PaymentMethods = new List<PaymentMethod>();
            this.Details = new List<SaleDetail>();
        }
    }
}