using CerberusMultiBranch.Models.Entities;
using System.Linq;
using CerberusMultiBranch.Support;

using System.Web.Mvc;
using CerberusMultiBranch.Models.ViewModels.Config;
using System;

namespace CerberusMultiBranch.Controllers.Common
{
    [Authorize]
    public class JsonController : Controller
    {
        private ApplicationData db = new ApplicationData();


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
        public JsonResult GetAvailableBranches()
        {
            var list = db.Branches.ToList();
            return Json(list);
        }

        [HttpPost]
        public JsonResult BeginBranchSession(int branchId, string name)
        {
            System.Web.HttpContext.Current.Session.Add(Cons.SessionBranchId, branchId);
            System.Web.HttpContext.Current.Session.Add(Cons.SessionBranchName, name);
            return Json(true);
        }

        public JsonResult GetBenchSession()
        {
            JCatalogEntity s;

            if (System.Web.HttpContext.Current.Session[Cons.SessionBranchId] != null)
            {
                s = new JCatalogEntity();
                s.Id = Convert.ToInt32(System.Web.HttpContext.Current.Session[Cons.SessionBranchId]);
                s.Name = System.Web.HttpContext.Current.Session[Cons.SessionBranchName].ToString();
            }
            else
            { 
                s = new JCatalogEntity
                {
                    Id = 0,
                    Name = string.Empty
                };
            }

            return Json(s);
        }
    }
}
