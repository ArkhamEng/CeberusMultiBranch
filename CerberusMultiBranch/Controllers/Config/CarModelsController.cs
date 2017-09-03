using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;

namespace CerberusMultiBranch.Controllers.Config
{
    public class CarModelsController : Controller
    {
        private ApplicationData db = new ApplicationData();

        // GET: CarModels
        public ActionResult Index()
        {
            var model = db.CarModels.Include(m=> m.CarYears).ToList();
         
            return View(model);
        }

        // GET: CarModels/Create
        public ActionResult Create(int? id)
        {
            ViewBag.CarMakes = db.CarMakes.ToList();

            if (id != null)
            {
                CarModel model = db.CarModels.Include(m => m.CarYears).ToList().Find(m=> m.CarModelId == id.Value);
                

                return View(model);
            }
            else
            {
                CarModel model = new CarModel();
                model.CarYears = new List<CarYear>();
                return View(model);
            }
        }

        // POST: CarModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarModelId,CarMakeId,Name")] CarModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.CarModelId != Cons.Zero)
                    db.Entry(model).State = EntityState.Modified;
                else
                    db.CarModels.Add(model);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddYear(int carModelId, int year)
        {

            var carYear = new CarYear { CarModelId = carModelId, Year = year };
            db.CarYears.Add(carYear);
            db.SaveChanges();

            var model = db.CarYears.Where(y => y.CarModelId == carYear.CarModelId).OrderBy(y => y.Year);
            return PartialView("_YearList", model);
        }
        

       
        // GET: CarModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel model = db.CarModels.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: CarModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarModel model = db.CarModels.Find(id);
            db.CarModels.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
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
