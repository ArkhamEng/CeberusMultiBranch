using CerberusMultiBranch.Models.Entities;
using System.Linq;
using CerberusMultiBranch.Support;

using System.Web.Mvc;
using CerberusMultiBranch.Models.ViewModels.Config;

namespace CerberusMultiBranch.Controllers.Common
{
    [Authorize]
    public class JsonController : Controller
    {
        private ApplicationData db = new ApplicationData();

     
        public JsonResult GetCities(int parentId)
        {
            var list = db.Cities.Where(c => c.StateId == parentId).ToSelectList();
            return Json(list);
        }

        public JsonResult GetModels(int parentId)
        {
            var list = db.CarModels.Where(m => m.CarMakeId == parentId).ToSelectList();
            return Json(list);
        }

        public JsonResult GetYears(int parentId)
        {
            var list = db.CarYears.Where(m => m.CarModelId == parentId).ToSelectList();
            return Json(list);
        }

        public JsonResult CheckProductCode(string code)
        {
            var exist = db.Products.Any(p => p.Code == code);
            return Json(exist);
        }

        public JsonResult QuickClient(int? id, string code, string name)
        {
            var clients = (from c in db.Clients
                         where (code == null || code == string.Empty || c.Code == code) &&
                               (name == null || name == string.Empty || c.Name.Contains(name)) &&
                               (id == null || id == Cons.Zero || c.ClientId == id)
                           select new JCatalogEntity { Id = c.ClientId, Name = c.Name, Code = c.Code,Phone =c.Phone }
                ).Take(20).ToList();

            return Json(clients);
        }
    }
}
