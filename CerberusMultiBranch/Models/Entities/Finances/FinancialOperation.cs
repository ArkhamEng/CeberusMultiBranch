using System;
using CerberusMultiBranch.Models.Entities.Operative;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using CerberusMultiBranch.Support;

namespace CerberusMultiBranch.Models.Entities.Finances
{
    public abstract class FinancialOperation
    {
        [DataType(DataType.Currency)]
        [Display(Name = "Monto")]
        public double Amount { get; set; }

        [Display(Name = "Pago")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [Display(Name = "Fecha de pago")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [MaxLength(100, ErrorMessage = "Solo se permiten 100 caractéres")]
        [Display(Name = "Comentarios")]
        public string Comment { get; set; }

        [MaxLength(30, ErrorMessage = "Solo se permiten 30 caractéres")]
        [Display(Name = "Referencia")]
        public string Reference { get; set; }

        [MaxLength(100)]
        public string UpdUser { get; set; }

        public DateTime UpdDate { get; set; }


        #region NotMapped Property

        //los pagos no se pueden eliminar si han pasado mas de dos días
        public virtual bool CanDelete
        {
            get
            {
                return DateTime.Now.ToLocal() <= this.UpdDate.AddDays(Cons.DaysToModify);
            }
        }
        #endregion

    }
}