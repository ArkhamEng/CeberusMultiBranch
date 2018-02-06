using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using CerberusMultiBranch.Models.Entities.Catalog;
using System.ComponentModel.DataAnnotations;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
using System.Collections.Generic;
using System.Web;
using System;

namespace CerberusMultiBranch.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationUser : IdentityUser
    {
        public string PicturePath { get; set; }

        public int ComissionForSale { get; set; }

        public  ICollection<UserBranch> UserBranches { get; set; }

        public ICollection<Employee> Employees { get; set; }

       

        [NotMapped]
        public HttpPostedFileBase PostedFile { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }

    }

}