using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Catalog
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        public ActionResult Index()
        {
            var model = new SearchEmployeeViewModel();
            model.Employees = db.Employees.Include(e => e.City).Where(e=> e.IsActive);
            model.States = db.States.ToSelectList();

            return View(model);
        }


        [HttpPost]
        public ActionResult Search(int? stateId, int? cityId, string name, string phone)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var model = (from c in db.Employees
                         where
                             (name == null || name == string.Empty || arr.Any(n => (c.Code + " " + c.Name).Contains(name))) &&
                             (stateId == null || c.City.StateId == stateId) &&
                             (cityId == null || c.CityId == cityId) &&
                             (phone == null || phone == string.Empty || c.Phone == phone) 
                         select c).ToList();

            return PartialView("_List", model);
        }

        // GET: Employees/Create
        public ActionResult Create(int? id)
        {
            EmployeeViewModel model;

            if (id != null)
            {
                model = new EmployeeViewModel(db.Employees.FirstOrDefault(e=> e.EmployeeId==id));

                model.StateId = db.Cities.Find(model.CityId).StateId;
                model.Cities = db.Cities.Where(c => c.StateId == model.StateId).ToSelectList();
            }
            else
                model = new EmployeeViewModel();

            model.States = db.States.ToSelectList();

            return View(model);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    employee.UpdUser = User.Identity.Name;
                    employee.UpdDate = DateTime.Now;

                    if (employee.EmployeeId == Cons.Zero)
                    {
                        if (employee.PostedFile != null)
                        {
                            employee.PictureType = employee.PostedFile.ContentType;
                            employee.Picture = employee.PostedFile.ToCompressedFile();
                        }

                        employee.Code = db.Employees.Max(c => c.Code).ToCode();
                        db.Employees.Add(employee);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (employee.PostedFile != null)
                        {
                            employee.PictureType = employee.PostedFile.ContentType;
                            employee.Picture = employee.PostedFile.ToCompressedFile();
                        }

                        db.Entry(employee).State = EntityState.Modified;

                        if (employee.PostedFile == null)
                        {
                            db.Entry(employee).Property(e => e.Picture).IsModified = false;
                            db.Entry(employee).Property(e => e.PictureType).IsModified = false;
                        }

                        db.SaveChanges();
                    }

                    return RedirectToAction("Create", new { id = employee.EmployeeId });
                }
                
                catch (Exception ex)
                {
                    ViewBag.Header = "Error al guardar los datos";
                    ViewBag.Message = "Ocurrio un error al guardar los datos del empleador detail:"+ex.Message;
                }
            }

            return RedirectToAction("Create", new { id = employee.EmployeeId });
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Code", employee.CityId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,Code,Name,BusinessName,FTR,TaxAddress,Address,ZipCode,Entrance,Email,Phone,CityId,Picture,IsActive,InsDate,UpdDate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Code", employee.CityId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
