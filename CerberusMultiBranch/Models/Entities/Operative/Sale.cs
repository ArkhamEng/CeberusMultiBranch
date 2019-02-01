using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Finances;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("Sale", Schema = "Operative")]
    public class Sale : Transaction
    {
        [Key]
        public int SaleId { get; set; }

        public int ClientId { get; set; }


        [Display(Name = "Folio Venta")]
        [MaxLength(30)]
        [Required]
        public string Folio { get; set; }

        [Display(Name = "Comisión")]
        public int ComPer { get; set; }

        [Display(Name = "Monto de Comisión")]
        public double ComAmount { get; set; }

        [Display(Name = "Entrega")]
        public int SendingType { get; set; }

        [Index("IDX_Year")]
        public int Year { get; set; }

        [Index("IDX_Sequential")]
        public int Sequential { get; set; }


        #region Navigation Properties
        public ICollection<SaleDetail> SaleDetails { get; set; }

        public ICollection<SalePayment> SalePayments { get; set; }

        public virtual Client Client { get; set; }

        public virtual SaleCreditNote SaleCreditNote { get; set; }
        #endregion


        public string StatusStyle
        {
            get
            {
                var style = string.Empty;

                switch (this.Status)
                {
                    case TranStatus.Compleated:
                        style = "alert-success";
                        break;
                    case TranStatus.Revision:
                        style = "alert-attention";
                        break;
                    case TranStatus.Reserved:
                        style = "alert-info";
                        break;
                    case TranStatus.Canceled:
                        style = "alert-danger";
                        break;

                    case TranStatus.PreCancel:
                        style = "alert-warning";
                        break;
                }

                return style;
            }
        }

        public string Days
        {
            get
            {
                var days = DateTime.Now.TodayLocal().Subtract(this.Expiration).Days;

                if (days > Cons.Zero)
                    return string.Format("{0} días Expirado", days);
                else if (days == Cons.Zero)
                    return "Expira hoy!";
                else
                {
                    days = days * -Cons.One;
                    return string.Format("Expira en {0} días", days);
                }
            }
        }

        public string Delivery
        {
            get { return this.SendingType == Cons.Zero ? "En Sucursal" : "A Domicilio"; }
        }

        public string SaleStatus
        {
            get
            {
                var name = string.Empty;

                switch (this.Status)
                {
                    case TranStatus.Compleated:
                        name = "Pagado";
                        break;
                    case TranStatus.Revision:
                        name = "Pago Pendiente";
                        break;
                    case TranStatus.Reserved:
                        name = "Reservado";
                        break;
                    case TranStatus.Canceled:
                        name = "Cancelado";
                        break;

                    case TranStatus.PreCancel:
                        name = "Por Cancelar";
                        break;
                }

                return name;
            }
        }

        public bool CancelDisabled
        {
            get
            {
                return !(HttpContext.Current.User.IsInRole("Supervisor") &&
                   ((this.Status == TranStatus.Revision || this.Status == TranStatus.Compleated) &&
                   DateTime.Now.ToLocal().Subtract(this.TransactionDate).Days < Cons.DaysToCancel));
            }
        }

        public Sale() : base()
        {
            this.SaleDetails = new List<SaleDetail>();
            this.TransactionDate = DateTime.Now.ToLocal();
        }
    }


}