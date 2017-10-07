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
        public DateTime EndDate { get; set; }

        public virtual Branch Branch { get; set; }

        public ICollection<CashDetail> CashDeatails { get; set; }

        public CashRegister()
        {
            this.Fill();
        }
    

        private void Fill()
        {
            this.BeginDate = DateTime.Now;
            this.EndDate = DateTime.Now;
            this.InitialAmount = Cons.Zero;
            this.FinalAmount = Cons.Zero;
            this.CashDeatails = new List<CashDetail>();
            this.IsClosed = true;
        }
    }

    [Table("CashDetail", Schema = "Operative")]
    public class CashDetail
    {
        public int CashDetailId { get; set; }

        public int CashRegisterId { get; set; }

        public string User { get; set; }

        public double Amount { get; set; }

        [DataType(DataType.Time)]
        public DateTime Date { get; set; }

        public virtual CashRegister CashRegister { get; set; }

    }

    
    public class Withdrawal:CashDetail
    {
        [Display(Name ="Comentario")]
        [Required]
        [MaxLength(100)]
        public string Comment { get; set; }
    }
}