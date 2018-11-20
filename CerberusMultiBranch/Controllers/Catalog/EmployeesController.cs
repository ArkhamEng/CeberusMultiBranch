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
    [CustomAuthorize]
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        public ActionResult Index()
        {
            var model = new SearchPersonViewModel<EmployeeViewModel>();
            model.Persons = LookFor(null, null, null, null, null);

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(int? stateId, int? cityId, string name, string phone, int? id)
        {
            var model = LookFor(stateId, cityId, name, phone, id);
            return PartialView("_List", model);
        }


        
        private List<EmployeeViewModel> LookFor(int? stateId, int? cityId, string name, string phone, int? id)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var model = (from employee in db.Employees.Where(e=> e.IsActive).Include(e=> e.Addresses)
                         where
                             (id == null || employee.EmployeeId == id) &&
                             (name == null || name == string.Empty || arr.Any(n => (employee.Code + " " + employee.Name).Contains(name))) &&
                             //(stateId == null || employee.City.StateId == stateId) &&
                             //(cityId == null || employee.CityId == cityId) &&
                             (phone == null || phone == string.Empty || employee.Phone == phone) &&
                             (employee.IsActive)
                         select new EmployeeViewModel
                         {
                             EmployeeId = employee.EmployeeId,
                             Code = employee.Code,
                             Email = employee.Email,
                             Entrance = employee.Entrance,
                             FTR = employee.FTR,
                             IsActive = employee.IsActive,
                             Name = employee.Name,
                             Phone = employee.Phone,

                             PictureType = employee.PictureType,
                             Picture = employee.Picture,
                             UserId = employee.UserId,
                             NSS = employee.NSS,
                             EmergencyPhone = employee.EmergencyPhone,
                             Salary = employee.Salary,
                             ComissionForSale = employee.ComissionForSale,

                             JobPosition = employee.JobPosition,

                             LockUser = employee.LockUser,
                             LockEndDate = employee.LockEndDate,
                             Addresses = employee.Addresses,
                         }).OrderBy(e => e.Name).ToList();

            return model;
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Capturista,Supervisor")]
        public ActionResult BeginAdd(int? id)
        {
            EmployeeViewModel model;

            if (id != null)
            {
                var employee = db.Employees.Include(e => e.Addresses).FirstOrDefault(e => e.EmployeeId == id);
                model = new EmployeeViewModel(employee);

                if (model.IsLocked)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.RecordLocked,
                        Header = "Registro bloqueado",
                        Body = "Este cliente " + employee.Name.ToUpper() + " se encuentra bloqueado por " + model.LockUser + " y no puede ser editado hasta ser liberado",
                    });
                }

                //bloqueo del registro
                try
                {
                    employee.LockEndDate = DateTime.Now.ToLocal().AddMinutes(Cons.LockTimeOut);
                    employee.LockUser = HttpContext.User.Identity.Name;
                    model.LockEndDate = employee.LockEndDate;
                    model.LockUser = employee.LockUser;

                    db.Entry(employee).State = EntityState.Modified;

                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Danger,
                        Code = Cons.Responses.Codes.ServerError,
                        Header = "Error al bloquear",
                        Body = "Ocurrio un error al bloquear el empleado " + employee.Name.ToUpper()
                    });
                }

                var stateId = model.Addresses.First().City.StateId;
                model.StateId = stateId;
                model.Cities = db.Cities.Where(c => c.StateId == model.StateId).OrderBy(c => c.Name).ToSelectList(model.Addresses.First().CityId);
                
            }
            else
                model = new EmployeeViewModel();

            model.States = db.States.OrderBy(s => s.Name).ToSelectList();
            model.JobPositions = db.JobPositions.OrderBy(j => j.Name).ToSelectList();

            return PartialView("_EmployeeEdition", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Capturista,Supervisor")]
        public ActionResult Save(Employee employee)
        {
            try
            {
                var response = new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success
                };

                if (employee.EmployeeId == Cons.Zero)
                {
                    if (employee.PostedFile != null)
                    {
                        employee.PictureType = employee.PostedFile.ContentType;
                        employee.Picture = employee.PostedFile.ToCompressedFile();
                    }

                    employee.Code = db.Employees.Max(c => c.Code).ToCode();
                    db.Employees.Add(employee);

                    response.Id = employee.EmployeeId;
                    response.Header = "Nuevo Empleado registrado";
                    response.Body = "Empleado " + employee.Name.ToUpperInvariant() +
                        " fue agregado correctamente, si requiere el uso de sistema deberas configurarle un usuario";
                }
                else
                {
                    if (employee.PostedFile != null)
                    {
                        employee.PictureType = employee.PostedFile.ContentType;
                        employee.Picture = employee.PostedFile.ToCompressedFile();
                    }

                    db.Entry(employee).State = EntityState.Modified;
                    db.Entry(employee).Property(c => c.IsActive).IsModified = false;
                    db.Entry(employee).Property(c => c.Code).IsModified = false;

                    if (employee.PostedFile == null)
                    {
                        db.Entry(employee).Property(e => e.Picture).IsModified = false;
                        db.Entry(employee).Property(e => e.PictureType).IsModified = false;
                    }

                    foreach (var address in employee.Addresses)
                        db.Entry(address).State = EntityState.Modified;

                    response.Id = employee.EmployeeId;
                    response.Header = "Empleado actualizado!";
                    response.Body = "Se registraron las modificaciones del empleado " + employee.Name.ToUpperInvariant() +
                        " y se liberaron bloqueos sobre el registro";
                }

                db.SaveChanges();

                return Json(response);
            }

            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ErroSaving,
                    Header = "Error al guardar",
                    Body = "Ocurrio un error al guardar los datos del empleado " +
                            employee.Name.ToUpperInvariant() + " Detalle:" + ex.Message
                });
            }

        }

        [HttpPost]
        public ActionResult UnLock(int id)
        {
            try
            {
                var employee = db.Employees.Find(id);
                employee.LockEndDate = null;
                employee.LockUser    = null;

                db.Entry(employee).Property(p => p.LockUser).IsModified = true;
                db.Entry(employee).Property(p => p.LockEndDate).IsModified = true;

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Info,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Empleado desbloqueado!",
                    Body = "Se libero el bloque del empleado: " +
                          employee.Name.ToUpperInvariant()
                });
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ErroSaving,
                    Header = "Error al desbloquear",
                    Body = "Ocurrio un error al desbloquear el registro del Empleado"
                });
            }

        }

        [HttpPost]
        public ActionResult Deactivate(int id)
        {
            try
            {
                var employee = db.Employees.Find(id);

                employee.UpdDate = DateTime.Now.ToLocal();
                employee.UpdUser = HttpContext.User.Identity.Name;
                employee.IsActive = false;

                db.Entry(employee).Property(p => p.IsActive).IsModified = true;
                db.Entry(employee).Property(p => p.UpdUser).IsModified = true;
                db.Entry(employee).Property(p => p.UpdDate).IsModified = true;

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Info,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Empleado Eliminado",
                    Body = "Se elimino el empleado: " +
                          employee.Name.ToUpperInvariant() + " del catálogo"
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Warning,
                    Code = Cons.Responses.Codes.ErroSaving,
                    Header = "Error al Eliminar",
                    Body = "Ocurrio un error al eliminar el registro de empleado"
                });
            }
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
