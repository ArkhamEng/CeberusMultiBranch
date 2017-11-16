using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public class Sale : Transaction
    {
        public int ClientId { get; set; }

        [Display(Name = "Folio Venta")]
        [MaxLength(30)]
        [Required]
        public string Folio { get; set; }

        public int ComPer { get; set; }

        public double ComAmount { get; set; }

        

        public virtual Client Client { get; set; }

    }

  
}