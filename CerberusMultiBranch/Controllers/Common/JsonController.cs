﻿using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.ViewModels.Config;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Common
{
    [Authorize]
    public class JsonController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public FileResult GetPicture()
        {
            string userId = User.Identity.GetUserId();

            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var user = bdUsers.Users.Find(userId);
            
            FileContentResult picture = null;

            if(user.Picture != null)
                picture = new FileContentResult(user.ClearImage, user.PictureType);
            else
            {
                byte[] imgdata = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("/Content/images/sinimagen.jpg"));
                picture = new FileContentResult(imgdata, "img/jpg");
            }

            return picture;
        }

        [HttpPost]
        public JsonResult GetCities(int parentId)
        {
            var list = db.Cities.Where(c => c.StateId == parentId).ToSelectList();
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetModels(int parentId)
        {
            var list = db.CarModels.Where(m => m.CarMakeId == parentId).ToSelectList();
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetYears(int parentId)
        {
            var list = db.CarYears.Where(m => m.CarModelId == parentId).ToSelectList();
            return Json(list);
        }

        [HttpPost]
        public JsonResult CheckProductCode(string code)
        {
            var exist = db.Products.Any(p => p.Code == code);
            return Json(exist);
        }

        [HttpPost]
        public JsonResult QuickSearchClient(int? id, string code, string name)
        {
            var clients = (from c in db.Clients
                           where (code == null || code == string.Empty || c.Code == code) &&
                                 (name == null || name == string.Empty || c.Name.Contains(name)) &&
                                 (id == null || id == Cons.Zero || c.ClientId == id) 
                                 
                           select new JCatalogEntity { Id = c.ClientId, Name = c.Name, Code = c.Code, Phone = c.Phone }
                ).Take(20).ToList();

            return Json(clients);
        }

        [HttpPost]
        public JsonResult QuickSearchProvider(int? id, string code, string name)
        {
            var providers = (from p in db.Providers
                             where (code == null || code == string.Empty || p.Code == code) &&
                                   (name == null || name == string.Empty || p.Name.Contains(name)) &&
                                   (id == null || id == Cons.Zero || p.ProviderId == id)
                             select new JCatalogEntity { Id = p.ProviderId, Name = p.Name, Code = p.Code, Phone = p.Phone }
                ).Take(20).ToList();

            return Json(providers);
        }

        [HttpPost]
        public JsonResult QuickSearchUser(string id, string name, string email)
        {
            List<JCatalogEntity> users;

            if (id != null && id != string.Empty)
            {
                users = (from u in db.Users where (u.Id == id)
                          select new JCatalogEntity { Code = u.Id, Name = u.UserName, Email = u.Email }).ToList();
            }
            else
            { 
                var idList = (from e in db.Employees
                              select e.UserId).ToList();

                users = (from u in db.Users
                       where (email == null || email == string.Empty || u.Email.Contains(email)) &&
                             (name == null || name == string.Empty || u.UserName.Contains(name)) &&
                             (!idList.Contains(u.Id))

                       select new JCatalogEntity { Code = u.Id, Name = u.UserName, Email = u.Email }
                ).Take(10).ToList();
            }

            return Json(users);
        }

        [HttpPost]
        public JsonResult GetAvailableBranches()
        {
            var list = db.Branches.ToList();
            return Json(list);
        }

        [HttpPost]
        public JsonResult BeginBranchSession(int branchId, string name)
        {
            JCatalogEntity session = new JCatalogEntity { Id = branchId, Name = name };

            if(System.Web.HttpContext.Current.Session[Cons.BranchSession] != null)
                System.Web.HttpContext.Current.Items[Cons.BranchSession] = session;
            else
                System.Web.HttpContext.Current.Session.Add(Cons.BranchSession,session);

            return Json(true);
        }

        public JsonResult GetBranchSession()
        {
            var session = Extension.GetBranchSession();
            return Json(session);
        }
    }
}
