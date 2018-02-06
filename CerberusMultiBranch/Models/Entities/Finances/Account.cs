using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Finances
{
    public abstract class Account
    {
        [DataType(DataType.Date)] 
        public double InitialAmount { get; set; }

        [DataType(DataType.Date)]
        public double CurrentAmount { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Period { get; set; }

        public DateTime UpdDate { get; set; }

        public string UpdUser { get; set; }

    }
}