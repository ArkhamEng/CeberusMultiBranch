using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    [NotMapped]
    public class PurchasOrderViewModel:Purchase
    {
      
        public SelectList PurchaseTypes { get; set; }

        public PurchasOrderViewModel():base()
        {
            var types = new List<Selectable>();
            types.Add(new Selectable { Id = (int)TransactionType.Credito, Name = TransactionType.Credito.ToString() });
            types.Add(new Selectable { Id = (int)TransactionType.Contado, Name = TransactionType.Contado.ToString() });

            this.PurchaseTypes = types.ToSelectList();

            TransactionType = TransactionType.Credito;
        }
    }
}