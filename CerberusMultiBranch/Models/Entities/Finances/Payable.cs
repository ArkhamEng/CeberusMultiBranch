using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Finances;
using CerberusMultiBranch.Models.Entities.Operative;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Finances
{
    [Table("Payable", Schema = "Finances")]
    public class Payable:Account
    {
        public int PayableId { get; set; }

        public int PurchaseId { get; set; }

        #region Navigation Properties
        public virtual Purchase Purchase { get; set; }

        public ICollection<PurchasePayment> PurchasePayments { get; set; }
        #endregion
    }
}