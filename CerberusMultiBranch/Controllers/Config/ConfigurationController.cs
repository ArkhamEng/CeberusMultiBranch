using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using CerberusMultiBranch.Models.ViewModels.Config;
using CerberusMultiBranch.Models.Entities.Operative;

namespace CerberusMultiBranch.Controllers.Config
{
    [CustomAuthorize(Roles = "Administrador")]
    public class ConfigurationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditCause(int? id)
        {
            if (id == null)
                return PartialView("_WithDrawalCauseEdition", new WithdrawalCause());
            else
            {
                try
                {
                    var model = db.WithdrawalCauses.FirstOrDefault(m => m.WithdrawalCauseId == id && m.IsActive);

                    if (model != null)
                        return PartialView("_WithDrawalCauseEdition", model);
                    else
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Header = "Error al obtener datos",
                            Code = Cons.Responses.Codes.RecordNotFound,
                            Body = "No se encontro la causa seleccionada, el registro ya no existe"
                        });
                }
                catch (Exception)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Danger,
                        Header = "Error al obtener datos",
                        Code = Cons.Responses.Codes.ServerError,
                        Body = "Ocurrio un error al obtener la causa de retiro"
                    });
                }
            }
        }

        [HttpPost]
        public ActionResult SearchCauses(string filter)
        {
            try
            {
                string[] arr = new List<string>().ToArray();

                if (filter != null && filter != string.Empty)
                    arr = filter.Trim().Split(' ');

                var model = db.WithdrawalCauses.Where(w =>
                 (filter == null || filter == string.Empty || arr.All(f => (w.Name).Contains(f))) &&
                 (w.IsActive)).OrderBy(c => c.Name).ToList();

                return PartialView("_WithdrawalCauseList", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al obtener datos",
                    Code = Cons.Responses.Codes.RecordNotFound,
                    Body = "Ocurrio un error al realizar la busqueda de causas de retiro"
                });
            }
        }


        [HttpPost]
        public ActionResult SaveCause(WithdrawalCause cause)
        {
            try
            {
                if (cause.WithdrawalCauseId > Cons.Zero)
                    db.Entry(cause).State = EntityState.Modified;
                else
                    db.WithdrawalCauses.Add(cause);

                db.SaveChanges();

                var model = db.WithdrawalCauses.Where(w => w.IsActive).ToList();

                return PartialView("_WithdrawalCauseList", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al guardar",
                    Code = Cons.Responses.Codes.ErroSaving,
                    Body = "Ocurrio un error al guardar la causa de retiro " + cause.Name
                });
            }

        }


        [HttpPost]
        public ActionResult DeleteCause(int id)
        {
            try
            {
                var cause = db.WithdrawalCauses.FirstOrDefault(m => m.IsActive && m.WithdrawalCauseId == id);
                var count = db.CashDetails.Where(d => d.WithdrawalCauseId == id).Count();

                JResponse response = null;

                if (cause != null)
                {

                    if (count > Cons.Zero)
                    {
                        cause.UpdDate = DateTime.Now.ToLocal();
                        cause.UpdUser = HttpContext.User.Identity.Name;
                        cause.IsActive = false;

                        db.Entry(cause).State = EntityState.Modified;
                    }
                    else
                        db.WithdrawalCauses.Remove(cause);

                    db.SaveChanges();

                    response = new JResponse
                    {
                        Result = Cons.Responses.Success,
                        Header = "Registro eliminado",
                        Code = Cons.Responses.Codes.Success,
                        Body = string.Format("Se elimino la causa de retiro {0} ", cause.Name)
                    };
                }
                else
                {
                    response = new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Header = "Registro no encontrado",
                        Code = Cons.Responses.Codes.RecordNotFound,
                        Body = "No se encontro el registro que se desea eliminar, intenta buscarlo de nuevo"
                    };
                }

                return Json(response);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al eliminar",
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al eliminar el sistema"
                });
            }
        }


        [HttpPost]
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
        public ActionResult Clasifications()
        {
            var model = new SystemAndCategoryViewModel();
            model.Systems = db.Systems.Include(s => s.SystemCategories).OrderBy(s => s.Name).ToList();
            model.Categories = db.Categories.OrderBy(c => c.Name).ToList();
            model.WithdrawalCauses = db.WithdrawalCauses.Where(w => w.IsActive).OrderBy(c => c.Name).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSystem(int? id)
        {
            if (id == null)
                return PartialView("_SystemEdition", new PartSystem());
            else
            {
                try
                {
                    var model = db.Systems.Find(id);

                    if (model != null)
                        return PartialView("_SystemEdition", model);
                    else
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Header = "Error al obtener datos",
                            Code = Cons.Responses.Codes.RecordNotFound,
                            Body = "No se encontro el sistema seleccionado, el registro ya no existe"
                        });
                }
                catch (Exception)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Danger,
                        Header = "Error al obtener datos",
                        Code = Cons.Responses.Codes.ServerError,
                        Body = "Ocurrio un error al obtener los datos del sistema"
                    });
                }
            }
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
            try
            {
                string[] arr = new List<string>().ToArray();

                if (filter != null && filter != string.Empty)
                    arr = filter.Trim().Split(' ');

                var model = db.Systems.Include(s => s.SystemCategories).Where(s =>
                 (filter == null || filter == string.Empty || arr.All(f => (s.Name).Contains(f))))
                .OrderBy(c => c.Name).ToList();

                return PartialView("_SystemList", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al obtener datos",
                    Code = Cons.Responses.Codes.RecordNotFound,
                    Body = "Ocurrio un error al realizar la busqueda de categorías"
                });
            }
        }


        public ActionResult SystemTest()
        {
            return View();
        }


        [AllowAnonymous]
        public JsonResult DtSearchSystem(string filter)
        {
            try
            {
                string[] arr = new List<string>().ToArray();

                if (filter != null && filter != string.Empty)
                    arr = filter.Trim().Split(' ');

                var model = db.Systems.Include(s => s.SystemCategories).Where(s =>
                 (filter == null || filter == string.Empty || arr.All(f => (s.Name).Contains(f))))
                .OrderBy(c => c.Name).ToList();

                var jlist = new List<Object>();

                model.ForEach(m =>
                {
                    jlist.Add(new { Name = m.Name, Commission = m.Commission, UpdDate = m.UpdDate.ToString("dd/MM/yyyy hh:mm:sss"), UpdUser = m.UpdUser });
                });

                var count = db.Systems.Count();
                var j = new { data = jlist, draw = 1, recordsTotal = count, recordsFiltered = count };

                // var ser = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(j);
                var result = Json(jlist, JsonRequestBehavior.AllowGet);

                return result;
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al obtener datos",
                    Code = Cons.Responses.Codes.RecordNotFound,
                    Body = "Ocurrio un error al realizar la busqueda de categorías"
                });
            }
        }




        [HttpPost]
        public ActionResult SaveSystem(PartSystem system)
        {
            try
            {
                if (system.PartSystemId == Cons.Zero)
                    db.Systems.Add(system);
                else
                    db.Entry(system).State = EntityState.Modified;

                db.SaveChanges();

                var model = db.Systems.Include(s => s.SystemCategories).
                    Where(c => c.PartSystemId == system.PartSystemId).ToList();

                return PartialView("_SystemList", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al guardar",
                    Code = Cons.Responses.Codes.ErroSaving,
                    Body = "Ocurrio un error al guardar los datos deL Sistema " + system.Name
                });
            }
        }

        [HttpPost]
        public ActionResult DeleteSystem(int id)
        {
            try
            {
                var system = db.Systems.Find(id);
                JResponse response = null;

                if (system != null)
                {
                    var pc = db.Products.Where(p => p.PartSystemId == system.PartSystemId).Count();

                    if (pc == Cons.Zero)
                    {
                        db.Systems.Remove(system);
                        db.SaveChanges();

                        response = new JResponse
                        {
                            Result = Cons.Responses.Success,
                            Header = "Registro eliminado",
                            Code = Cons.Responses.Codes.Success,
                            Body = string.Format("Se elimino el sistema {0} ", system.Name)
                        };
                    }

                    else
                    {
                        response = new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Header = "Acción no permitida",
                            Code = Cons.Responses.Codes.ServerError,
                            Body = string.Format("El sistema {0} esta siendo usado en {1} producto(s) ", system.Name, pc)
                        };
                    }
                }
                else
                {
                    response = new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Header = "Registro no encontrado",
                        Code = Cons.Responses.Codes.RecordNotFound,
                        Body = "No se encontro el registro que se desea eliminar, intenta buscarlo de nuevo"
                    };
                }

                return Json(response);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al eliminar",
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al eliminar el sistema"
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

            if (sysCat != null)
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
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
                return PartialView("_CategoryEdition");
            else
            {
                try
                {
                    var model = db.Categories.Find(id);

                    if (model != null)
                        return PartialView("_CategoryEdition", model);

                    else
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Header = "Error al obtener datos",
                            Code = Cons.Responses.Codes.RecordNotFound,
                            Body = "No se encontro la categoría seleccionada, el registro ya no existe"
                        });
                }
                catch (Exception)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Danger,
                        Header = "Error al obtener datos",
                        Code = Cons.Responses.Codes.ServerError,
                        Body = "Ocurrio un error al obtener los datos de la categoría"
                    });

                }
            }
        }

        [HttpPost]
        public ActionResult GetCategory(int categoryId)
        {
            var model = db.Categories.Find(categoryId);
            return PartialView("_CategoryEdition", model);
        }

        [HttpPost]
        public ActionResult SaveCategory(Category category)
        {
            try
            {
                if (category.CategoryId == Cons.Zero)
                    db.Categories.Add(category);
                else
                    db.Entry(category).State = EntityState.Modified;

                db.SaveChanges();


                var model = db.Categories.Where(c => c.SatCode == category.SatCode).ToList();
                return PartialView("_CategoryList", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al guardar",
                    Code = Cons.Responses.Codes.ErroSaving,
                    Body = "Ocurrio un error al guardar los datos de la categoría " + category.Name
                });
            }

        }

        [HttpPost]
        public ActionResult SearchCategories(string filter)
        {
            try
            {
                string[] arr = new List<string>().ToArray();

                if (filter != null && filter != string.Empty)
                    arr = filter.Trim().Split(' ');

                var categories = db.Categories.Where(c =>
                (filter == null || filter == string.Empty || arr.All(s => (c.SatCode + " " + c.Name).Contains(s))))
                .OrderBy(c => c.Name).ToList();

                return PartialView("_CategoryList", categories);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al obtener datos",
                    Code = Cons.Responses.Codes.RecordNotFound,
                    Body = "Ocurrio un error al realizar la busqueda de categorías"
                });
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                var category = db.Categories.Find(id);
                JResponse response = null;

                if (category != null)
                {
                    var pc = db.Products.Where(p => p.CategoryId == category.CategoryId).Count();
                    var scc = db.SystemCategories.Where(sc => sc.CategoryId == category.CategoryId).Count();

                    if (pc == Cons.Zero && scc == Cons.Zero)
                    {
                        db.Categories.Remove(category);
                        db.SaveChanges();

                        response = new JResponse
                        {
                            Result = Cons.Responses.Success,
                            Header = "Registro eliminado",
                            Code = Cons.Responses.Codes.Success,
                            Body = string.Format("Se elimino la  categoría {0} ", category.Name)
                        };
                    }
                    else
                    {
                        response = new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Header = "Acción no permitida",
                            Code = Cons.Responses.Codes.ServerError,
                            Body = string.Format("La categoría {0} esta siendo usada en {1} producto(s) " +
                            "y ha sido configurada en {2} sistema(s)", category.Name, pc, scc)
                        };
                    }
                }
                else
                {
                    response = new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Header = "Registro no encontrado",
                        Code = Cons.Responses.Codes.RecordNotFound,
                        Body = "No se encontro el registro que se desea eliminar, intenta buscarlo de nuevo"
                    };
                }

                return Json(response);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al eliminar",
                    Code = Cons.Responses.Codes.ServerError,
                    Body = "Ocurrio un error al eliminar la categoría"
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
