using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Finances;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("Purchase", Schema = "Operative")]
    public class Purchase : Transaction
    {
        public int PurchaseId { get; set; }

        public int ProviderId { get; set; }

        [Display(Name = "Factura")]
        [MaxLength(30)]
        [Required]
        public string Bill { get; set; }

        [MaxLength(20)]
        public string Folio { get; set; }

        #region Navigation Properties
        public virtual Provider Provider { get; set; }

        public ICollection<PurchasePayment> PurchasePayments { get; set; }

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }

        public ICollection<PurchaseDiscount> PurchaseDiscount { get; set; }
        #endregion

        public string StatusStyle
        {
            get
            {
                var style = string.Empty;

                switch (this.Status)
                {
                    case TranStatus.InProcess:
                        style = "alert-warning";
                        break;
                    case TranStatus.Reserved:
                        style = "alert-info";
                        break;
                    case TranStatus.Compleated:
                        style = "alert-success";
                        break;
                  
                    case TranStatus.Canceled:
                        style = "alert-danger";
                        break;

                 
                }

                return style;
            }
        }

        public string PurchasStatus
        {
            get
            {
                var name = string.Empty;

                switch (this.Status)
                {
                    case TranStatus.InProcess:
                        name = "En Captura";
                        break;
                    case TranStatus.Reserved:
                        name = "Inventariado";
                        break;
                    case TranStatus.Compleated:
                        name = "Pagado";
                        break;
                  
                    case TranStatus.Canceled:
                        name = "Cancelado";
                        break;
                }

                return name;
            }
        }

        public string Days
        {
            get
            {
                var days = DateTime.Now.TodayLocal().Subtract(this.Expiration).Days;

                if (days > Cons.Zero)
                    return string.Format("{0} días Expirado", days);
                else if(days == Cons.Zero)
                    return "Expira hoy!";
                else 
                {
                    days = days * -Cons.One;
                    return string.Format("Expira en {0} días", days);
                }
            }
        }
    }


}