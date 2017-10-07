using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Support;
using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.ViewModels.Config;
using System.Collections.Generic;

namespace CerberusMultiBranch.Controllers.Catalog
{
    [Authorize]
    public class ProvidersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Providers
        public ActionResult Index()
        {
            var model       = new SearchProviderViewModel();
            model.Providers = db.Providers.Take(100).ToList();
            model.States    = db.States.ToSelectList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(int? stateId, int? cityId, string name, string phone)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var model = (from c in db.Providers
                         where
                             (name == null || name == string.Empty || arr.Any(n => (c.Code + " " + c.Name).Contains(name))) &&
                             (stateId == null || c.City.StateId == stateId) &&
                             (cityId == null || c.CityId == cityId) &&
                             (phone == null || phone == string.Empty || c.Phone == phone) 
                         select c).ToList();

            return PartialView("_List", model);
        }

        // GET: Providers/Create
        public ActionResult Create(int? id)
        {
            ProviderViewModel model;
            if (id != null)
            {
                model = CreateModel(db.Providers.Find(id));
            }
            else
                model = new ProviderViewModel();

            model.States = db.States.ToSelectList();
            return View(model);

        }

        private ProviderViewModel CreateModel(Provider provider)
        {
            var model = new ProviderViewModel(provider);
            model.States = db.States.ToSelectList();
            model.StateId = db.Cities.Find(model.CityId).StateId;
            model.Cities = db.Cities.Where(c => c.StateId == model.StateId).ToSelectList();

            return model;
        }

        // POST: Providers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Provider provider)
        {
                if (provider.ProviderId == Cons.Zero)
                {
                    provider.Code = db.Providers.Max(c => c.Code).ToCode();
                    db.Providers.Add(provider);
                }
                else
                    db.Entry(provider).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Create",new { id = provider.ProviderId });            
        }

        [HttpPost]
        public ActionResult QuickSearch(string name)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var providers = (from p in db.Providers
                             where (name == null || name == string.Empty || arr.Any(n => (p.Code + " " + p.Name).Contains(name))) 
                             select p).Take(Cons.QuickResults).ToList();

            return PartialView("_QuickProviderList", providers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
