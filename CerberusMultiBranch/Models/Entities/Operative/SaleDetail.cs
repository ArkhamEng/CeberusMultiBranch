using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("SaleDetail", Schema = "Operative")]
    public class SaleDetail:TransactionDetail
    {
        [Column(Order = 0), Key, ForeignKey("Sale")]
        public int SaleId { get; set; }

        [DataType(DataType.Currency)]
        public double Commission { get; set; }

        public int SortOrder { get; set; }

        public int? ParentId { get; set; }

        public virtual Sale Sale { get; set; }
    }
}