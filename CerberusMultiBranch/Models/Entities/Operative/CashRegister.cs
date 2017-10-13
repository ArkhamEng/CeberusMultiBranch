using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("CashRegister", Schema = "Operative")]
    public class CashRegister
    {
        public int CashRegisterId { get; set; }

        public int BranchId { get; set; }

        [MaxLength(50)]
        public string UserOpen { get; set; }

        [MaxLength(50)]
        public string UserClose { get; set; }

        [DataType(DataType.Currency)]
        public double InitialAmount { get; set; }

        [DataType(DataType.Currency)]
        public double FinalAmount { get; set; }

        [MaxLength(100)]
        public string CloseComment { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OpeningDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ClosingDate { get; set; }

        public virtual Branch Branch { get; set; }

        public ICollection<CashDetail> CashDetails { get; set; }

        [NotMapped]
        public ICollection<Withdrawal> Withdrawals { get { return this.CashDetails.OfType<Withdrawal>().ToList(); } }

        [NotMapped]
        public ICollection<Income> Incomes { get { return this.CashDetails.OfType<Income>().ToList(); } }

        [NotMapped]
        public string ChartSource { get; set; }

        public bool IsOpen { get; set; }
    
        public CashRegister()
        {
            this.Fill();
            this.CashDetails = new List<CashDetail>();
        }
    

        private void Fill()
        {
            this.OpeningDate = DateTime.Now;
            this.InitialAmount = Cons.Zero;
            this.FinalAmount = Cons.Zero;
        }
    }
}