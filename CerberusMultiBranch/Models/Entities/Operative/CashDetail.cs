using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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

        public virtual CashRegister CashRegister { get; set; }

    }

    public class Withdrawal : CashDetail
    {
        [Display(Name = "Comentario")]
        [Required]
        [MaxLength(100)]
        public string Comment { get; set; }
    }

    public class Income : CashDetail
    {
        public PaymentType Type { get; set; }
    }
}