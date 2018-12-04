using CerberusMultiBranch.Models.Entities.Config;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel;

namespace CerberusMultiBranch.Models.Entities.Purchasing
{
    [Table("PurchaseType", Schema = "Purchasing")]
    public class PurchaseType:ISelectable
    {
        public PType PurchaseTypeId { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; }

        public int Id
        {
            get
            {
                return (int)this.PurchaseTypeId;
            }
        }
    }

    public enum PType
    {
        [Display(Name ="Crédito")]
        Credit = 1,

        [Display(Name = "Contado")]
        Cash = 2
    }
}