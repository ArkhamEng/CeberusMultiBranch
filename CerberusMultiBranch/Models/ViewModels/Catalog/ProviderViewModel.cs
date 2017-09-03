using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Support;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    [NotMapped]
    public class ProviderViewModel:Provider
    {
        public SelectList States { get; set; }

        public SelectList Cities { get; set; }

        public int StateId { get; set; }

        public ProviderViewModel()
        {
            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
        }

        public ProviderViewModel(Provider provider)
        {
            this.Address = provider.Address;
            this.BusinessName = provider.BusinessName;
            this.CityId = provider.CityId;
            this.ProviderId = provider.ProviderId;
            this.Code = provider.Code;
            this.Email = provider.Email;
            this.FTR = provider.FTR;
            this.IsActive = provider.IsActive;
            this.InsDate = provider.InsDate;
            this.Name = provider.Name;
            this.Phone = provider.Phone;
            this.UpdDate = provider.UpdDate;
            this.ZipCode = provider.ZipCode;

            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
        }
    }
}