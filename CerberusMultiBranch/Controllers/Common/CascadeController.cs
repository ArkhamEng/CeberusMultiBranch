using CerberusMultiBranch.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CerberusMultiBranch.Support;

using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Common
{
    public class CascadeController : Controller
    {
        private ApplicationData db = new ApplicationData();

        public JsonResult GetSubCategories(int parentId)
        {
            var list = db.SubCategories.Where(sc => sc.CategoryId == parentId).ToSelectList();
            return Json(list);
        }

        public JsonResult GetCities(int parentId)
        {
            var list = db.Cities.Where(c => c.StateId == parentId).ToSelectList();
            return Json(list);
        }
    }
}
