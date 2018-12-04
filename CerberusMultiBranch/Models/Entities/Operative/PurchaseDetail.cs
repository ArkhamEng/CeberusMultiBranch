using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("PurchaseDetail", Schema = "Operative")]
    public class PurchaseDetail:TransactionDetail
    {
        [Column(Order = 0), Key, ForeignKey("Purchase")]
        public int PurchaseId { get; set; }

        public int? BranchId { get; set; }

        public decimal Received { get; set; }

        public decimal Stocked { get; set; }

        public decimal Rejected { get; set; }

        public virtual Purchase Purchase { get; set; }

        public virtual Branch Branch { get; set; }
    }
}