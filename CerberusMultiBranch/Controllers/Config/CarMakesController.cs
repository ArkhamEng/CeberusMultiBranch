﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities;
using CerberusMultiBranch.Models.Entities.Config;

namespace CerberusMultiBranch.Controllers.Config
{
    public class CarMakesController : Controller
    {
        private ApplicationData db = new ApplicationData();

        // GET: Makes
        public ActionResult Index()
        {
            return View(db.CarMakes.ToList());
        }

        // GET: Makes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarMake make = db.CarMakes.Find(id);
            if (make == null)
            {
                return HttpNotFound();
            }
            return View(make);
        }

        // GET: Makes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Makes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MakeId,Name")] CarMake make)
        {
            if (ModelState.IsValid)
            {
                db.CarMakes.Add(make);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(make);
        }

        // GET: Makes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarMake make = db.CarMakes.Find(id);
            if (make == null)
            {
                return HttpNotFound();
            }
            return View(make);
        }

        // POST: Makes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MakeId,Name")] CarMake make)
        {
            if (ModelState.IsValid)
            {
                db.Entry(make).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(make);
        }

        // GET: Makes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarMake make = db.CarMakes.Find(id);
            if (make == null)
            {
                return HttpNotFound();
            }
            return View(make);
        }

        // POST: Makes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarMake make = db.CarMakes.Find(id);
            db.CarMakes.Remove(make);
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
