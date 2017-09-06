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
using CerberusMultiBranch.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.IO;

namespace CerberusMultiBranch.Controllers.Catalog
{
    public class EmployeesController : Controller
    {
        private ApplicationData db = new ApplicationData();

        // GET: Employees
        public ActionResult Index()
        {
            var model = new SearchEmployeeViewModel();
            model.Employees = db.Employees.Include(e => e.City);
            model.States = db.States.ToSelectList();

            return View(model);
        }


        public FileResult GetPicture()
        {

            string userId = User.Identity.GetUserId();

            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var empId = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault().EmployeeId;

            var employee = db.Employees.Where(e => e.EmployeeId == empId).FirstOrDefault();

            return new FileContentResult(employee.ClearImage, employee.PictureType);

        }


        // GET: Employees/Create
        public ActionResult Create(int? id)
        {
            EmployeeViewModel model;

            if (id != null)
            {
                model = new EmployeeViewModel(db.Employees.Find(id));

                model.StateId = db.Cities.Find(model.CityId).StateId;
                model.Cities = db.Cities.Where(c => c.StateId == model.StateId).ToSelectList();

                var ap = new ApplicationDbContext();
                var ui = ap.Users.FirstOrDefault(u => u.EmployeeId == id);


                if (ui != null)
                    model.Register = new Models.RegisterViewModel { EmployeeId = id, Email = ui.Email, HasAccount = true, Password = ui.Email, ConfirmPassword = ui.Email };
            }
            else
                model = new EmployeeViewModel();

            model.States = db.States.ToSelectList();

            return View(model);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee, RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                if (employee.EmployeeId == Cons.Zero)
                {
                    if (employee.PostedFile != null)
                    {
                        employee.PictureType = employee.PostedFile.ContentType;
                        employee.Picture = employee.PostedFile.ToCompressedFile();
                    }

                    db.Employees.Add(employee);
                    db.SaveChanges();

                    register.EmployeeId = employee.EmployeeId;

                    CreateUser(register);
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
                        db.Entry(employee).Property(x => x.PictureType).IsModified = false;
                        db.Entry(employee).Property(x => x.Picture).IsModified = false;
                    }

                    db.SaveChanges();

                    if (register != null && !register.HasAccount)
                        CreateUser(register);
                }

                return RedirectToAction("Create", new { id = employee.EmployeeId });
            }

            return RedirectToAction("Create", new { id = employee.EmployeeId });
        }

        public void CreateUser(RegisterViewModel register)
        {
            try
            {
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());

                ApplicationUserManager _userManager = new ApplicationUserManager(store);

                var manger = _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var user = new ApplicationUser() { Email = register.Email, UserName = register.Email, EmployeeId = register.EmployeeId };
                var usmanger = manger.Create(user, register.Password);
            }
            catch (Exception ex)
            {

            }
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
