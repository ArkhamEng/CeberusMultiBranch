using CerberusMultiBranch.Models.Entities.Operative;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Finances
{
    [Table("SalePayment", Schema = "Finances")]
    public class SalePayment:FinancialOperation
    {
        public int SalePaymentId { get; set; }

        public int? SaleId { get; set; }

        public int? ReceivableId { get; set; }


        #region Navigation Properties
        public virtual Sale Sale { get; set; }

        public virtual Receivable Receivable { get; set; }
        #endregion

        //los pagos no se pueden eliminar de las ventas al contado
        public override bool CanDelete
        {
            get
            {
                return (base.CanDelete && this.Sale.TransactionType != TransactionType.Contado);
            }
        }
    }
}