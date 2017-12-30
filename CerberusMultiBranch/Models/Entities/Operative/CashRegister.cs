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

        [Display(Name = "Usuario Apertura")]
        [MaxLength(50)]
        public string UserOpen { get; set; }

        [Display(Name ="Usuario Cierre")]
        [MaxLength(50)]
        public string UserClose { get; set; }

        [Display(Name = "Monto Inicial")]
        [DataType(DataType.Currency)]
        public double InitialAmount { get; set; }

        [DataType(DataType.Currency)]
        public double FinalAmount { get; set; }

        [Display(Name = "Comentario de Cierre")]
        [MaxLength(100)]
        public string CloseComment { get; set; }

        [Display(Name = "Fecha Apertura")]
        [DataType(DataType.DateTime)]
        public DateTime OpeningDate { get; set; }

        [Display(Name = "Fecha Cierre")]
        [DataType(DataType.DateTime)]
        public DateTime? ClosingDate { get; set; }

        public virtual Branch Branch { get; set; }

        public ICollection<CashDetail> CashDetails { get; set; }

        [NotMapped]
        public ICollection<CashDetail> Withdrawals { get { return this.CashDetails.Where(d=> d.DetailType == Cons.Zero).ToList(); } }

        [NotMapped]
        public ICollection<CashDetail> Incomes { get { return this.CashDetails.Where(d => d.DetailType == Cons.One).ToList(); } }

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