using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
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
    [Table("Sale", Schema = "Operative")]
    public class Sale:Transaction
    {
        [Key]
        public int SaleId { get; set; }

        public int ClientId { get; set; }

        [Display(Name = "Folio Venta")]
        [MaxLength(30)]
        [Required]
        public string Folio { get; set; }

        public int ComPer { get; set; }

        public double ComAmount { get; set; }

        public int SendingType { get; set; }

        public PaymentType PaymentType { get; set; }

        public ICollection<SaleDetail> SaleDetails { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public virtual Client Client { get; set; }


        public Sale()
        {
            this.SaleDetails = new List<SaleDetail>();
            this.TransactionDate = DateTime.Now;
            this.UpdDate = DateTime.Now;
        }
    }

    public enum TranStatus:int
    {
        Canceled      = -1, //Cancelado en venta y compra
        InProcess     = 0, //Abierto-venta, En proceso -compra
        Reserved      = 1,
        Revision      = 2, 
        Compleated    = 3, //Pagado-Venta, Inventariado-Compra
    }
}