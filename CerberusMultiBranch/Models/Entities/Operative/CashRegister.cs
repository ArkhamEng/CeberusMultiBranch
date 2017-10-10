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

        public string UserOpen { get; set; }

        public string UserClose { get; set; }

        public bool IsClosed { get; set; }

        public double InitialAmount { get; set; }

        public double FinalAmount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BeginDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        public virtual Branch Branch { get; set; }

        public ICollection<CashDetail> CashDetails { get; set; }

        [NotMapped]
        public ICollection<Withdrawal> Withdrawals { get { return this.CashDetails.OfType<Withdrawal>().ToList(); } }

        [NotMapped]
        public ICollection<Income> Incomes { get { return this.CashDetails.OfType<Income>().ToList(); } }

        [NotMapped]
        public string ChartSource { get; set; }

        public CashRegister()
        {
            this.Fill();
            this.CashDetails = new List<CashDetail>();
        }
    

        private void Fill()
        {
            this.BeginDate = DateTime.Now;
            this.InitialAmount = Cons.Zero;
            this.FinalAmount = Cons.Zero;
            this.IsClosed = true;
        }
    }
}