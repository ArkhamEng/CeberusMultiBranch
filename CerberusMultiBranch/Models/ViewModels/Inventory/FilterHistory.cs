using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Inventory
{
    public class FilterHistory
    {
        public int BranchId { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}