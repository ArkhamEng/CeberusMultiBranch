using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public class Purchase : Transaction
    {
        public int ProviderId { get; set; }

        [Display(Name = "Factura")]
        [MaxLength(30)]
        [Required]
        public string Bill { get; set; }

        [Display(Name = "Vencimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Expiration { get; set; }

        [Index("IDX_Inventoried", IsUnique = false)]
        public bool Inventoried { get; set; }

        public virtual Provider Provider { get; set; }

        public Purchase() : base()
        {
            this.Expiration = DateTime.Now;
        }

    }


}