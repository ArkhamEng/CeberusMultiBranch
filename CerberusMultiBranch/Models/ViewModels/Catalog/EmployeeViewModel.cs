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
    public class EmployeeViewModel:Employee
    {
        public SelectList States { get; set; }

        public SelectList Cities { get; set; }

        public SelectList JobPositions { get; set; }

        public int StateId { get; set; }

     
        public RegisterViewModel Register { get; set; }

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
                return string.Format("Tel : {0} | Emergencia: {1}", 
                    string.IsNullOrEmpty(this.Phone) ? "No Asignado" : this.Phone,
                    string.IsNullOrEmpty(this.EmergencyPhone) ? "No Asignado" : this.EmergencyPhone);
            }
        }

        [Display(Name="Usuario de Sistema")]
        public string SystemUser
        {
            get
            {
                return string.IsNullOrEmpty(this.UserId) ? "No Asignado" : this.User.UserName;
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
            get { return !(HttpContext.Current.User.IsInRole("Capturista") || HttpContext.Current.User.IsInRole("Vendedor")); }
        }

        public bool DeleteDisabled
        {
            get { return !(HttpContext.Current.User.IsInRole("Supervisor")); }
        }



        public EmployeeViewModel()
        {
            this.States   = new List<State>().ToSelectList();
            this.Cities   = new List<City>().ToSelectList();
            this.Register = new RegisterViewModel();
            this.JobPositions = new List<State>().ToSelectList();
            this.Addresses = new List<Address>();
            this.Addresses.Add(new Address());
        }

        public EmployeeViewModel(Employee employee)
        {
            this.EmployeeId   = employee.EmployeeId;
            this.Code         = employee.Code;
            this.Email        = employee.Email;
            this.Entrance     = employee.Entrance;
            this.FTR          = employee.FTR;
            this.IsActive     = employee.IsActive;
            this.Name         = employee.Name;
            this.Phone        = employee.Phone;

            this.PictureType  = employee.PictureType;
            this.Picture      = employee.Picture;
            this.UserId       = employee.UserId;
            this.NSS              = employee.NSS;
            this.EmergencyPhone   = employee.EmergencyPhone;
            this.Salary           = employee.Salary;
            this.ComissionForSale = employee.ComissionForSale;
           
            
            this.JobPosition = employee.JobPosition;

            this.LockUser = employee.LockUser;
            this.LockEndDate = employee.LockEndDate;
            this.Addresses = employee.Addresses;

            this.States = new List<State>().ToSelectList();
            this.Cities = new List<City>().ToSelectList();
        }
    }
}