using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CerberusMultiBranch.Models.ViewModels.Config
{
    [NotMapped]
    public class UserInRole:ApplicationUser
    {
        public string[] AvIds { get; set; }
        public SelectList AvailableRoles { get; set; }

        public string[] SelIds { get; set; }
        public SelectList SelectedRoles { get; set; }

        public string[] avBrn { get; set; }
        public SelectList AvailableBranches { get; set; }

        public string[] SelBrn { get; set; }
        public SelectList SelectedBranches { get; set; }


        

        public UserInRole(ApplicationUser user)
        {
            this.UserName = user.UserName;
            this.AccessFailedCount = user.AccessFailedCount;
            this.ComissionForSale = user.ComissionForSale;
            this.Email = user.Email;
            this.EmailConfirmed = user.EmailConfirmed;
            this.Id = user.Id;
            this.PhoneNumber = user.PhoneNumber;
            this.PicturePath = user.PicturePath;
            this.SelectedRoles = new List<ISelectable>().ToSelectList();
            this.AvailableRoles = new List<ISelectable>().ToSelectList();
            this.SelectedBranches = new List<ISelectable>().ToSelectList();
            this.AvailableBranches = new List<ISelectable>().ToSelectList();
        }
    }
}