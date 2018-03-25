using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Finances;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int ComPer { get; set; }

        public double ComAmount { get; set; }

        public int SendingType { get; set; }

        [Index("IDX_Year")]
        public int Year { get; set; }

        [Index("IDX_Sequential")]
        public int Sequential { get; set; }


        #region Navigation Properties
        public ICollection<SaleDetail> SaleDetails { get; set; }

        public ICollection<SalePayment> SalePayments { get; set; }

        public virtual Client Client { get; set; }
        #endregion

        public Sale()
        {
            this.SaleDetails = new List<SaleDetail>();
            this.TransactionDate = DateTime.Now;
            this.UpdDate = DateTime.Now;
        }
    }

   
}