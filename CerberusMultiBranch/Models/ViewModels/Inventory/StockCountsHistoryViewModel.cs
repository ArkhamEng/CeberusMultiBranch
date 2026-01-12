using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Inventory;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Inventory
{
    public class StockCountsHistoryViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? BeginDateTime { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? EndDateTime { get; set; }

        public List<Branch> Branches { get; set; }

        public List<PartSystem> Systems { get; set; }

        public List<StockCountsHistory> StockCounts { get; set; }

       

        public StockCountsHistoryViewModel()
        {

            BeginDateTime = DateTime.Now.ToLocal();

            EndDateTime = BeginDateTime.Value.AddHours(23);

            Branches = new List<Branch>();

            Systems = new List<PartSystem>();

            StockCounts = new List<StockCountsHistory>();

        }
    }
}