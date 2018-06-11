using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using CerberusMultiBranch.Models.ViewModels.Config;

namespace CerberusMultiBranch.Controllers.Config
{
    [CustomAuthorize(Roles = "Administrador,Capturista")]
    public class ConfigurationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult GetCauses()
        {
            var causes = db.WithdrawalCauses.OrderBy(c => c.WithdrawalCauseId).ToList();
            return PartialView("_WithdrawalCauseList", causes);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult SaveCause(string name)
        {
            db.WithdrawalCauses.Add(new Models.Entities.Operative.WithdrawalCause { Name = name, UserAdd = User.Identity.Name, InsDate = DateTime.Now });
            db.SaveChanges();

            var model = db.WithdrawalCauses.ToList();

            return PartialView("_WithdrawalCauseList", model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult SaveConfigVariable(Variable variable)
        {
            if (ModelState.IsValid)
            {
                if (variable.VariableId == Cons.Zero)
                    db.Variables.Add(variable);
                else
                    db.Entry(variable).State = EntityState.Modified;

                db.SaveChanges();
            }

            var variables = db.Variables.OrderBy(c => c.VariableId);
            return PartialView("_CategoryList", variables);
        }



        #region Systems
        public ActionResult SystemCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewSystem()
        {

            return PartialView("_SystemEdition",new PartSystem());
        }

        [HttpPost]
        public ActionResult GetSystem(int partSystemId)
        {
            var model = db.Systems.Find(partSystemId);
            return PartialView("_SystemEdition", model);
        }

        [HttpPost]
        public ActionResult GetSystemConfig(int id)
        {
            var model = new SystemCategoryViewModel();

            // obtengo las categorias configuradas
            var selected = db.SystemCategories.Where(sc => sc.PartSystemId == id).
                            Select(sc => sc.Category).ToList();

            var cIds = selected.Select(c => c.CategoryId).ToList();

            var available = db.Categories.Where(c => !cIds.Contains(c.CategoryId)).ToList();

            model.AvailableCategories = available.ToSelectList();
            model.AssignedCategories = selected;

            return PartialView("_SystemCategory", model);
        }

        [HttpPost]
        public ActionResult SearchSystems(string filter)
        {
            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
                arr = filter.Trim().Split(' ');

            var model = db.Systems.Include(s=> s.SystemCategories).Where(s =>
            (filter == null || filter == string.Empty || arr.All(f => (s.Name).Contains(f))))
            .OrderBy(c => c.Name).ToList();

            return PartialView("_SystemList", model);
        }

        [HttpPost]
        public ActionResult SaveSystem(PartSystem system)
        {
            try
            {
                system.UpdDate = DateTime.Now.ToLocal();
                system.UpdUser = User.Identity.Name;

                if (system.PartSystemId == Cons.Zero)
                    db.Systems.Add(system);
                else
                    db.Entry(system).State = EntityState.Modified;

                db.SaveChanges();

                var model = db.Systems.Include(s=> s.SystemCategories).
                    Where(c => c.PartSystemId == system.PartSystemId).ToList();
                return PartialView("_SystemList", model);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error al  guardar sistema",
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult DeleteSystem(int id)
        {
            try
            {
                var system = db.Systems.Find(id);

                if (system != null)
                {
                    var pc = db.Products.Where(p => p.PartSystemId == system.PartSystemId).Count();

                    if (pc == Cons.Zero)
                    {
                        db.Systems.Remove(system);
                        db.SaveChanges();
                    }
                        
                    else
                        return Json(new
                        {
                            Result = "Imposible eliminar el sistema",
                            Message = "Este sistema esta siendo usando en " + pc + " producto(s)"
                        });
                }

                var model = db.Systems.Include(s=> s.SystemCategories).OrderBy(s => s.Name).ToList();
                return PartialView("_SystemList", model);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error al  eliminar el sistema",
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult AddSystemCategory(SystemCategory sysCat)
        {
            try
            {
                var system = db.Systems.Find(sysCat.PartSystemId);
                system.UpdDate = DateTime.Now.ToLocal();
                system.UpdUser = User.Identity.Name;

                sysCat.UpdDate = DateTime.Now.ToLocal();
                sysCat.UpdUser = User.Identity.Name;
                db.SystemCategories.Add(sysCat);

                db.Entry(system).State = EntityState.Modified;
                db.SaveChanges();

                return GetSystemConfig(sysCat.PartSystemId);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al agregar la categoría", Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult RemoveSystemCategory(int partSystemId, int categoryId)
        {
            var sysCat = db.SystemCategories.Find(partSystemId, categoryId);
            
            if(sysCat!=null)
            {
                var system = db.Systems.Find(partSystemId);
                system.UpdDate = DateTime.Now.ToLocal();
                system.UpdUser = User.Identity.Name;

                db.SystemCategories.Remove(sysCat);

                db.Entry(system).State = EntityState.Modified;
                db.SaveChanges();
            }

            return GetSystemConfig(partSystemId);
        }
        #endregion

        #region Categories
        [HttpPost]
        public ActionResult NewCategory()
        {
            return PartialView("_CategoryEdition");
        }

        [HttpPost]
        public ActionResult GetCategory(int categoryId)
        {
            var model = db.Categories.Find(categoryId);
            return PartialView("_CategoryEdition",model);
        }

        [HttpPost]
        public ActionResult SaveCategory(Category category)
        {
            try
            {
                category.UpdDate = DateTime.Now.ToLocal();
                category.UpdUser = User.Identity.Name;

                if (category.CategoryId == Cons.Zero)
                    db.Categories.Add(category);
                else
                    db.Entry(category).State = EntityState.Modified;

                db.SaveChanges();


                var model = db.Categories.Where(c => c.SatCode == category.SatCode).ToList();
                return PartialView("_CategoryList", model);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error al Agregar la categoría", Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult SearchCategories(string filter)
        {
            string[] arr = new List<string>().ToArray();

            if (filter != null && filter != string.Empty)
                arr = filter.Trim().Split(' ');

            var categories = db.Categories.Where(c =>
            (filter == null || filter == string.Empty || arr.All(s => (c.SatCode + " " + c.Name).Contains(s))))
            .OrderBy(c => c.Name).ToList();

            return PartialView("_CategoryList", categories);
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                var category = db.Categories.Find(id);

                if (category != null)
                {
                    var pc = db.Products.Where(p => p.CategoryId == category.CategoryId).Count();
                    var scc = db.SystemCategories.Where(sc => sc.CategoryId == category.CategoryId).Count();

                    if (pc == Cons.Zero && scc == Cons.Zero)
                    {
                        db.Categories.Remove(category);
                        db.SaveChanges();
                    }
                    else if(pc> Cons.Zero)
                    {
                        return Json(new
                        {
                            Result = "Imposible eliminar la categoria SAT",
                            Message = string.Format("Esta categoría esta siendo usada en {0} producto(s) "+
                            "y ha sido configurada en {1} sistema(s)", pc, scc )
                        });
                    }
                }

                var model = db.Categories.OrderBy(s => s.Name).ToList();
                return PartialView("_CategoryList", model);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Error al  eliminar la categoría",
                    Message = ex.Message
                });
            }
        }

        #endregion

        [HttpPost]
        public ActionResult SaveBranch(int? branchId, string name)
        {

            if (ModelState.IsValid)
            {
                if (branchId == null)
                {
                    var branch = new Branch { Name = name };
                    db.Branches.Add(branch);
                }
                else
                {
                    var branch = new Branch { BranchId = branchId.Value, Name = name };
                    db.Entry(branch).State = EntityState.Modified;
                }
                db.SaveChanges();
            }

            var branches = db.Branches.OrderBy(c => c.BranchId);
            return PartialView("_BranchList", branches);
        }

        public ActionResult GetBranches()
        {
            var branches = db.Branches.OrderBy(b => b.BranchId).ToList();
            return PartialView("_BranchList", branches);
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
