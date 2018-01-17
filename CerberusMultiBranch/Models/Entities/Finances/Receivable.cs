using CerberusMultiBranch.Models.Entities.Operative;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Finances
{
    [Table("Receivable", Schema = "Finances")]
    public class Receivable:Account
    {
        public int ReceivableId { get; set; }

        public int SaleId { get; set; }

        #region Navigation Properties
        public virtual Sale Sale { get; set; }

        public ICollection<SalePayment> SalePayments { get; set; }
        #endregion
    }
}