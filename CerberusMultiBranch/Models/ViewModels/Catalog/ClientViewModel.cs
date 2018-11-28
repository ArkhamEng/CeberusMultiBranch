using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public SelectList PersonTypes { get; set; }

        public int StateId { get; set; }

        [Display(Name = "Dirección")]
        public string StringAddress
        {
            get
            {
                return Addresses.First().ToString();
            }
        }

        [Display(Name = "Teléfonos")]
        public string Phones
        {
            get
            {
                return string.Format("Tel 1: {0} | Tel 2: {1}",
                    string.IsNullOrEmpty(this.Phone) ? "No Asignado" : this.Phone,
                    string.IsNullOrEmpty(this.Phone2) ? "No Asignado" : this.Phone2);
            }
        }

        [Display(Name = "Edición")]
        public string Edition
        {
            get
            {
                return string.Format("{0} por  el usuario {1}",
                    this.UpdDate.ToString("dddd, dd MMMM yyyy h:mm tt"), this.UpdUser);
            }
        }


        public bool IsLocked
        {
            get
            {
                return (LockEndDate != null && LockUser != HttpContext.Current.User.Identity.Name);
            }
        }
     
        public bool EditionDisabled
        {
            get { return  this.ClientId == Cons.Zero ? true : !(HttpContext.Current.User.IsInRole("Capturista") || HttpContext.Current.User.IsInRole("Vendedor"));    }
        }

        public bool DeleteDisabled
        {
            get { return this.ClientId == Cons.Zero ? true : !(HttpContext.Current.User.IsInRole("Supervisor")); }
        }


        public ClientViewModel()
        {
            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
            this.Addresses = new List<Address>();
            this.Addresses.Add(new Address());
            FillTypes();
        }

        private void FillTypes()
        {
            //utilizo una coleccion de ciudades (ISelectable)
            var types = new List<City>();
            types.Add(new City { CityId = 0, Name = "Mostrador" });
            types.Add(new City { CityId = 1, Name = "Distribuidor" });
            types.Add(new City { CityId = 2, Name = "Mayorista" });

            var Ptypes = new List<SelectListItem>();
            Ptypes.Add(new SelectListItem { Text ="Fisica", Value ="Fisica" });
            Ptypes.Add(new SelectListItem { Text = "Moral", Value = "Moral" });

            this.Types = types.ToSelectList();
            this.PersonTypes = new SelectList(Ptypes, nameof(SelectListItem.Value), nameof(SelectListItem.Text));
        }

        public ClientViewModel(Client client)
        {
            this.BusinessName = client.BusinessName;
            
            this.ClientId     = client.ClientId;
            this.Code         = client.Code;
            this.Email        = client.Email;
            this.Entrance     = client.Entrance;
            this.FTR          = client.FTR;
            this.IsActive     = client.IsActive;
            this.CreditLimit = client.CreditLimit;
            this.Name         = client.Name;
            this.Phone        = client.Phone;
            this.UsedAmount = client.UsedAmount;
            this.UpdDate      = client.UpdDate;
            this.Type         = client.Type;
            this.CreditDays   = client.CreditDays;
            this.CreditComment = client.CreditComment;
            this.LockEndDate = client.LockEndDate;
            this.LockUser    = client.LockUser;


            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
            this.Addresses = client.Addresses;
            
            FillTypes();
        }
    }
}