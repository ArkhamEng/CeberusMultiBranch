using CerberusMultiBranch.Models.Entities.Operative;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Finances
{
    [Table("PurchasePayment", Schema = "Finances")]
    public class PurchasePayment:FinancialOperation
    {
        public int PurchasePaymentId { get; set; }

        public int PurchaseId { get; set; }


        public virtual Purchase Purchase { get; set; }

    }
}