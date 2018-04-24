using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("SaleCreditNote", Schema = "Operative")]
    public class SaleCreditNote
    {
        [Key, ForeignKey("Sale")]
        public int SaleCreditNoteId { get; set; }

        [MaxLength(30)]
        public string Folio { get; set; }

        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Index("IDX_Year")]
        public int Year { get; set; }

        [Index("IDX_Sequential")]
        public int Sequential { get; set; }

        [Index("IDX_Ident")]
        [MaxLength(15)]
        public string Ident { get; set; }

        [MaxLength(30)]
        public string User { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime ExplirationDate { get; set; }

        public bool IsActive { get; set; }

        public virtual Sale Sale { get; set; }

        public ICollection<CreditNoteHistory> CreditNoteHistories { get; set; }
    }

    [Table("CreditNoteHistory", Schema = "Operative")]
    public class CreditNoteHistory
    {
        public int CreditNoteHistoryId { get; set; }

        public int SaleCreditNoteId { get; set; }

        public double Amount { get; set; }

        [MaxLength(30)]
        public string User { get; set; }

        public DateTime ChangeDate { get; set; }

        public virtual SaleCreditNote SaleCreditNote { get; set; }
    }
}