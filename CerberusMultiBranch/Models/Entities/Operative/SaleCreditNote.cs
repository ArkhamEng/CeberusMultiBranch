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
        [Key, Column(Order = 0), ForeignKey("Sale")]
        public int SaleCreditNoteId { get; set; }

        [Key,Column(Order = 1)]
        [MaxLength(30)]
        [Index("IDX_Identifier_Active",Order =0)]
        public string Folio { get; set; }

        [Index("IDX_Identifier_Active",Order =1)]
        [MaxLength(15)]
        public string Ident { get; set; }

        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Index("IDX_Sequential",0)]
        public int Year { get; set; }

        [Index("IDX_Sequential",1)]
        public int Sequential { get; set; }


        [MaxLength(30)]
        public string User { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime ExplirationDate { get; set; }

        [Index("IDX_Identifier_Active", Order = 2)]
        public bool IsActive { get; set; }

        public virtual Sale Sale { get; set; }

        //public ICollection<CreditNoteHistory> CreditNoteHistories { get; set; }
    }

    [Table("CreditNoteHistory", Schema = "Operative")]
    public class CreditNoteHistory
    {
        public int CreditNoteHistoryId { get; set; }

        public int SaleCreditNoteId { get; set; }

        public string Folio { get; set; }

        public double Amount { get; set; }

        [MaxLength(30)]
        public string User { get; set; }

        public DateTime ChangeDate { get; set; }

        public virtual SaleCreditNote SaleCreditNote { get; set; }
    }
}