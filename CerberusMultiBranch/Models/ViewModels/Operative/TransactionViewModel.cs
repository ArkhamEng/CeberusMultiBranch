
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class TransactionViewModel
    {
        public SelectList Branches { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? BeginDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? EndDate { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public ICollection<Sale> Sales { get; set; }

        public ICollection<CashRegister> CashRegisters { get; set; }

        public ICollection<StockMovement> StockMovements { get; set; }

        public TransactionViewModel()
        {
            BeginDate = DateTime.Now.TodayLocal();
            EndDate = BeginDate.Value.AddHours(23);
        }
    }
}