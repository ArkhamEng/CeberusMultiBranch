using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("SaleHistory", Schema = "Operative")]
    public class SaleHistory
    {
        public int SaleHistoryId { get; set; }

        public int SaleId { get; set; }

        [Display(Name ="Total")]
        [DataType(DataType.Currency)]
        public double TotalDue { get; set; }

        [Display(Name = "Usuario")]
        public string User { get; set; }

        [Display(Name = "Comentario")]
        public string Comment { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime InsDate { get; set; }

        public virtual Sale Sale { get; set; }
    }
}