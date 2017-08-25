using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    [NotMapped]
    public class ClientViewModel:Client
    {
        public SelectList States { get; set; }

        public SelectList Cities { get; set; }

        public int StateId { get; set; }

        public ClientViewModel()
        {
            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
        }

        public ClientViewModel(Client client)
        {
            this.Address      = client.Address;
            this.BusinessName = client.BusinessName;
            this.CityId       = client.CityId;
            this.ClientId     = client.ClientId;
            this.Code         = client.Code;
            this.Email        = client.Email;
            this.Entrance     = client.Entrance;
            this.FTR          = client.FTR;
            this.IsActive     = client.IsActive;
            this.InsDate      = client.InsDate;
            this.LegalRepresentative = client.LegalRepresentative;
            this.Name         = client.Name;
            this.Phone        = client.Phone;
            this.TaxAddress   = client.TaxAddress;
            this.UpdDate      = client.UpdDate;
            this.ZipCode      = client.ZipCode;

            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
        }
    }
}