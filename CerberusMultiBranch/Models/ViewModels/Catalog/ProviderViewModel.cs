using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    [NotMapped]
    public class ProviderViewModel:Provider
    {
        public SelectList States { get; set; }

        public SelectList Cities { get; set; }


        public int StateId { get; set; }

        [Display(Name="Dirección")]
        public string StringAddress
        {
            get
            {
                return Addresses.First().ToString();
            }
        }

        [Display(Name = "Crédito")]
        public string Credit
        {
            get
            {
                return string.Format("Monto Límite: {0} | Días de crédito {1}", 
                    this.CreditLimit > Cons.Zero ? this.CreditLimit.ToMoney() : "No Asignado", 
                    this.DaysToPay > Cons.Zero ? this.DaysToPay.ToString() : "No Asignado");
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

        [Display(Name = "Agente")]
        public string AgentData
        {
            get
            {
                return string.Format("{0} | Teléfono {1}",
                    string.IsNullOrEmpty(this.Agent) ? "No Asignado" : this.Agent,
                    string.IsNullOrEmpty(this.AgentPhone) ? "No Asignado" : this.AgentPhone);
            }
        }

        [Display(Name = "Teléfonos")]
        public string Phones
        {
            get
            {
                return string.Format("Telefono 1: {0},  Telefono 2: {1}, Teléfono 3: {2}", 
                    string.IsNullOrEmpty(this.Phone) ? "No Asignado" : this.Phone,
                    string.IsNullOrEmpty(this.Phone2) ? "No Asignado" : this.Phone2,
                    string.IsNullOrEmpty(this.Phone3) ? "No Asignado" : this.Phone3);
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
            get { return !(HttpContext.Current.User.IsInRole("Capturista") || HttpContext.Current.User.IsInRole("Vendedor")); }
        }

        public bool DeleteDisabled
        {
            get { return !(HttpContext.Current.User.IsInRole("Supervisor")); }
        }

        public bool UpdateCatalogDisabled
        {
            get { return !(HttpContext.Current.User.IsInRole("Supervisor")); }
        }

        public bool DeleteCatalogDisabled
        {
            get { return !(HttpContext.Current.User.IsInRole("Supervisor")); }
        }


        public ProviderViewModel()
        {
            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
            this.Addresses = new List<Address>();
            this.Addresses.Add(new Address());
        }

        public ProviderViewModel(Provider provider)
        {
            this.Address = provider.Address;
            this.BusinessName = provider.BusinessName;
            this.ProviderId = provider.ProviderId;
            this.Code = provider.Code;
            this.Email = provider.Email;
            this.FTR = provider.FTR;
            this.IsActive = provider.IsActive;
            this.Name = provider.Name;
            this.Phone = provider.Phone;
            this.UpdDate = provider.UpdDate;
            this.UpdUser = provider.UpdUser;
            this.ZipCode = provider.ZipCode;
            this.WebSite = provider.WebSite;
            this.Agent = provider.Agent;
            this.AgentPhone = provider.AgentPhone;
            this.Line = provider.Line;
            this.CreditLimit = provider.CreditLimit;
            this.DaysToPay = provider.DaysToPay;
            this.Catalog = provider.Catalog;
            this.LockUser = provider.LockUser;
            this.LockEndDate = provider.LockEndDate;
            this.Addresses = provider.Addresses;

            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
        }
    }

    public class ProviderCatalogViewModel
    {
        public string  ProviderName { get; set; }

        public int ProviderKey { get; set; }

        public int ProductsCount { get; set; }

        public int PendingCount { get; set; }

        public bool CanProcess
        {
            get
            {
                return (this.PendingCount > 0);
            }
        }
    }
}