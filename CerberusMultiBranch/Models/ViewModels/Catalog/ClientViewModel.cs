using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
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

        public SelectList Types { get; set; }

        public int StateId { get; set; }

        public ClientViewModel()
        {
            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
            FillTypes();
        }

        private void FillTypes()
        {
            //utilizo una coleccion de ciudades (ISelectable)
            var types = new List<City>();
            types.Add(new City { CityId = 0, Name = "Mostrador" });
            types.Add(new City { CityId = 1, Name = "Distribuidor" });
            types.Add(new City { CityId = 2, Name = "Mayorista" });

            this.Types = types.ToSelectList();
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
            this.LegalRepresentative = client.LegalRepresentative;
            this.Name         = client.Name;
            this.Phone        = client.Phone;
            this.TaxAddress   = client.TaxAddress;
            this.UpdDate      = client.UpdDate;
            this.ZipCode      = client.ZipCode;
            this.Type         = client.Type;

            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
            FillTypes();
        }
    }
}