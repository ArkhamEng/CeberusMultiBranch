using CerberusMultiBranch.Models.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("TransactionType", Schema = "Operative")]
    public class TransactionType:ISelectable
    {
        public int TransactionTypeId { get; set; }

        public string Name { get; set; }

        public int Id { get { return this.TransactionTypeId; } }

        public ICollection<Transaction> Transaction { get; set; }
    }

    public enum TransType:int
    {
        Purchase = 1,
        Sale = 2,
        Transference=3
    }

}