using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public class Receivable
    {
        public int ReceivableId { get; set; }

        public int SaleId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public double InitialAmount { get; set; }

        public double CurrentAmount { get; set; }

        public virtual Sale Sale { get; set; }
    }
}