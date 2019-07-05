using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Support;
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

        [Display(Name ="Devuelto")]
        public double Refund { get; set; }

        [Display(Name = "Creado")]
        [Index("IDX_InsDate", IsClustered = false, IsUnique =false)]
        public DateTime InsDate { get; set; }

        [Display(Name = "Creador por")]
        public string InsUser { get; set; }

        [Display(Name = "Editado")]
        public DateTime UpdDate { get; set; }

        [Display(Name = "Editado por")]
        public string UpdUser { get; set; }

        public virtual Sale Sale { get; set; }

        [NotMapped]
        [Display(Name ="Cantidad")]
        public double DueQuantity { get { return (this.Quantity - this.Refund); } }

        [NotMapped]
        [Display(Name = "Devolver")]
        public double NewRefund { get; set; }

      
    }
}