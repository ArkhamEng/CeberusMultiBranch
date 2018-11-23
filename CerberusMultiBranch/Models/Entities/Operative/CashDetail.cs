using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Finances;
using CerberusMultiBranch.Support;
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

        public PaymentMethod Type { get; set; }

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



    [Table("WithdrawalCause", Schema = "Operative")]
    public class WithdrawalCause:ISelectable
    {
        public int WithdrawalCauseId { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Se requier un nombre")]
        public string Name { get; set; }

        [Display(Name = "Editado")]
        public DateTime UpdDate { get; set; }

        [Display(Name = "Editado por")]
        [MaxLength(100)]
        public string UpdUser { get; set; }

        [Display(Name ="Activo")]
        public bool IsActive { get; set; }

        public ICollection<CashDetail> Withdrawals { get; set; }

        public int Id { get { return this.WithdrawalCauseId; } }

        public WithdrawalCause()
        {
            this.UpdDate = DateTime.Now.ToLocal();
            this.UpdUser = HttpContext.Current.User.Identity.Name;
            this.IsActive = true;
        }

    }
}