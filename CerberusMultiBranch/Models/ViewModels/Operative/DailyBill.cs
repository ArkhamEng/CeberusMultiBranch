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
    public class DailyBillViewModel
    {
        public int BranchId { get; set; }

        public TransactionType TransType { get; set; }

        public string Client { get; set; }

        public string Folio { get; set; }

        public SelectList Branches { get; set; }

        public SelectList TransTypes { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        public List<SaleDetail> SoldItems { get; set; }

        public DateTime EndDate { get { return Date.AddDays(Cons.One); } }

        public DailyBillViewModel()
        {
            this.Date = DateTime.Now.TodayLocal();
            this.SoldItems = new List<SaleDetail>();

            List<SelectListItem> list = new List<SelectListItem>
             {
             new SelectListItem { Text = TransactionType.Cash.ToString(), Value = ((int)TransactionType.Cash).ToString() },
             new SelectListItem { Text = TransactionType.Credit.ToString(), Value = ((int)TransactionType.Credit).ToString() },
             new SelectListItem { Text = TransactionType.Presale.ToString(), Value = ((int)TransactionType.Presale).ToString() }
             };

            this.TransTypes = new SelectList(list,"Value","Text");
        }
    }
}
