using CerberusMultiBranch.Models.Entities.Operative;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class OfferViewModel:Offer
    {
        [NotMapped]
        public SelectList Types { get; set; }
    }
}