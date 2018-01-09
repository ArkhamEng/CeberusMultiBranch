using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("Purchase", Schema = "Operative")]
    public class Purchase : Transaction
    {
        public int PurchaseId { get; set; }

        public int ProviderId { get; set; }

        [Display(Name = "Factura")]
        [MaxLength(30)]
        [Required]
        public string Bill { get; set; }

        [Display(Name = "Vencimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Expiration { get; set; }


        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }

        public virtual Provider Provider { get; set; }

        public Purchase() 
        {  
            this.Expiration = DateTime.Now;
        }

    }


}