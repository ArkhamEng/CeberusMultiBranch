using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public class Account
    {
        public int AccountId { get; set; }

        public double InitialAmount { get; set; }
   
        public double CurrentAmunt { get; set; }

        public DateTime BeginDate { get; set; }

    }

    public class PriverAccount:Account
    {
        public int ProviderId { get; set; }

        public virtual Provider Provider { get; set; }
    }

    public class ClientAccount:Account
    {
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}