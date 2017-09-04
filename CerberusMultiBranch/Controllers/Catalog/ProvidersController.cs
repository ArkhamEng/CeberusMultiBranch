using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Support;

namespace CerberusMultiBranch.Controllers.Catalog
{
    public class ProvidersController : Controller
    {
        private ApplicationData db = new ApplicationData();

        // GET: Providers
        public ActionResult Index()
        {
            var model       = new SearchProviderViewModel();
            model.Providers = db.Providers.Take(100).ToList();
            model.States    = db.States.ToSelectList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(int? stateId, int? cityId, string code, string name, string phone)
        {
            var model = (from c in db.Providers
                         where
                             (name == null || c.Name.Contains(name)) &&
                             (stateId == null || c.City.StateId == stateId) &&
                             (cityId == null || c.CityId == cityId) &&
                             (phone == null || c.Phone == phone) &&
                             (code == null || c.Code == code)
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
