using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("CashDetail", Schema = "Operative")]
    public class CashDetail
    {
        public int CashDetailId { get; set; }

        [ForeignKey("CashRegister")]
        public int CashRegisterId { get; set; }
        
        [Display(Name ="Usuario")]
        public string User { get; set; }

        [Display(Name = "Cantidad")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Hora")]
        public DateTime InsDate { get; set; }

        public PaymentType Type { get; set; }

        public string SaleFolio { get; set; }

        [Display(Name = "Comentario")]
        [Required]
        [MaxLength(100)]
        public string Comment { get; set; }

        [ForeignKey("Cause")]
        public int? WithdrawalCauseId { get; set; }

        public int DetailType { get; set; }

        public virtual WithdrawalCause Cause { get; set; }

        public virtual CashRegister CashRegister { get; set; }

    }

 /*   public class Withdrawal : CashDetail
    {
        [Display(Name = "Comentario")]
        [Required]
        [MaxLength(100)]
        public string Comment { get; set; }

        [ForeignKey("Cause")]
        public int WithdrawalCauseId { get; set; }

        public virtual WithdrawalCause Cause { get; set; }
    }

    public class Income : CashDetail
    {
        public PaymentType Type { get; set; }
        
        public string SaleFolio { get; set; }    
    }*/

    [Table("WithdrawalCause", Schema = "Operative")]
    public class WithdrawalCause:ISelectable
    {
        public int WithdrawalCauseId { get; set; }

        public string Name { get; set; }

        public string UserAdd { get; set; }

        public DateTime InsDate { get; set; }

        public ICollection<CashDetail> Withdrawals { get; set; }

        public int Id { get { return this.WithdrawalCauseId; } }
       
    }
}