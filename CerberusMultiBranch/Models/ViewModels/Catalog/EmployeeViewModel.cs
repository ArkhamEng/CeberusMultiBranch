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
    public class EmployeeViewModel:Employee
    {
        public SelectList States { get; set; }

        public SelectList Cities { get; set; }

        public int StateId { get; set; }

        [NotMapped]
        public byte[] ClearImage { get { return Support.GzipWrapper.Decompress(this.Picture); } }

        public RegisterViewModel Register { get; set; }


        public EmployeeViewModel()
        {
            this.States   = new List<State>().ToSelectList();
            this.Cities   = new List<City>().ToSelectList();
            this.Register = new RegisterViewModel();
        }

        public EmployeeViewModel(Employee employee)
        {
            this.Address      = employee.Address;
            this.CityId       = employee.CityId;
            this.EmployeeId   = employee.EmployeeId;
            this.Code         = employee.Code;
            this.Email        = employee.Email;
            this.Entrance     = employee.Entrance;
            this.FTR          = employee.FTR;
            this.IsActive     = employee.IsActive;
            this.InsDate      = employee.InsDate;
            this.Name         = employee.Name;
            this.Phone        = employee.Phone;
            this.UpdDate      = employee.UpdDate;
            this.ZipCode      = employee.ZipCode;
            this.PictureType  = employee.PictureType;
            this.Picture      = employee.Picture;
            this.UserId       = employee.UserId;
            this.EmployeeBranches = employee.EmployeeBranches;
            this.States   = new List<State>().ToSelectList();
            this.Cities   = new List<City>().ToSelectList();
            this.Register = new RegisterViewModel();
        }
    }
}