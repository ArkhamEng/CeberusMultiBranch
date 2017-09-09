using CerberusMultiBranch.Models.Entities;
using System.Linq;
using CerberusMultiBranch.Support;

using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Common
{
    [Authorize]
    public class CascadeController : Controller
    {
        private ApplicationData db = new ApplicationData();

        //public JsonResult GetSubCategories(int parentId)
        //{
        //    var list = db.SubCategories.Where(sc => sc.CategoryId == parentId).ToSelectList();
        //    return Json(list);
        //}

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
    }
}
