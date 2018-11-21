using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.ViewModels.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers.Config
{
    [CustomAuthorize(Roles ="Administrador")]
    public class MakesAndModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Makes
        public ActionResult Index()
        {
            var model = new MakesAndModelsViewModel
            {
                CarMakes = db.CarMakes.Where(m => m.IsActive).ToList(),
                CarModels = new List<CarModel>()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult SearchMakes(string filter)
        {
            try
            {
                string[] arr = new List<string>().ToArray();

                if (filter != null && filter != string.Empty)
                    arr = filter.Trim().Split(' ');

                var model = db.CarMakes.Where(make =>
                 (filter == null || filter == string.Empty || arr.All(f => (make.Name).Contains(f))) &&
                 (make.IsActive)).OrderBy(c => c.Name).ToList();

                return PartialView("_CarMakeList", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al obtener datos",
                    Code = Cons.Responses.Codes.RecordNotFound,
                    Body = "Ocurrio un error al realizar la busqueda de modelos"
                });
            }
        }

        [HttpPost]
        public ActionResult SearchModels(string filter)
        {
            try
            {
                string[] arr = new List<string>().ToArray();

                if (filter != null && filter != string.Empty)
                    arr = filter.Trim().Split(' ');

                var model = db.CarModels.Where(mod =>
                 (filter == null || filter == string.Empty || arr.All(f => (mod.Name + " " + mod.CarMake.Name).Contains(f))) &&
                 (mod.IsActive) && mod.CarMake.IsActive).OrderBy(c => c.Name).ToList();

                return PartialView("_CarModelList", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al obtener datos",
                    Code = Cons.Responses.Codes.RecordNotFound,
                    Body = "Ocurrio un error al realizar la busqueda de modelos"
                });
            }
        }


        [HttpPost]
        public ActionResult EditMake(int? id)
        {
            if (id == null)
                return PartialView("_CarMakeEdition", new CarMake());
            else
            {
                try
                {
                    var model = db.CarMakes.FirstOrDefault(m => m.CarMakeId == id && m.IsActive);

                    if (model != null)
                        return PartialView("_CarMakeEdition", model);
                    else
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Header = "Error al obtener datos",
                            Code = Cons.Responses.Codes.RecordNotFound,
                            Body = "No se encontro la armadora seleccionada, el registro ya no existe"
                        });
                }
                catch (Exception)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Danger,
                        Header = "Error al obtener datos",
                        Code = Cons.Responses.Codes.ServerError,
                        Body = "Ocurrio un error al obtener la armadora"
                    });
                }
            }
        }

        [HttpPost]
        public ActionResult EditModel(int? id)
        {
            CarModelViewModel model = new CarModelViewModel();
            if (id == null)
            {
                model.CarMakes = db.CarMakes.Where(cm => cm.IsActive).ToSelectList();
                return PartialView("_CarModelEdition", model);
            }
            else
            {
                try
                {
                    var carm = db.CarModels.FirstOrDefault(m => m.CarModelId == id && m.IsActive && m.CarMake.IsActive);

                    model.CarMakes = db.CarMakes.Where(cm => cm.IsActive).ToSelectList();
                    model.CarModelId    = carm.CarModelId;
                    model.CarMakeId     = carm.CarMakeId;
                    model.Name          = carm.Name;

                    if (model != null)
                        return PartialView("_CarModelEdition", model);
                    else
                        return Json(new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Header = "Error al obtener datos",
                            Code = Cons.Responses.Codes.RecordNotFound,
                            Body = "No se encontro el modelo seleccionado, el registro ya no existe"
                        });
                }
                catch (Exception)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Danger,
                        Header = "Error al obtener datos",
                        Code = Cons.Responses.Codes.ServerError,
                        Body = "Ocurrio un error al obtener el modelo"
                    });
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteMake(int id)
        {
            try
            {
                var make = db.CarMakes.FirstOrDefault(m => m.IsActive && m.CarMakeId == id);
                var count = db.CarModels.Where(d => d.CarMakeId == id).Count();

                JResponse response = null;

                if (make != null)
                {
                    if (count > Cons.Zero)
                    {
                       return Json(new JResponse
                        {
                            Result = Cons.Responses.Warning,
                            Header = "Imposible Eliminar",
                            Code = Cons.Responses.Codes.ServerError,
                            Body = "Existen " + count + " relacionados a esta armadora"
                        });
                    }
                    else
                        db.CarMakes.Remove(make);

                    db.SaveChanges();

                    response = new JResponse
                    {
                        Result = Cons.Responses.Success,
                        Header = "Registro eliminado",
                        Code = Cons.Responses.Codes.Success,
                        Body = string.Format("Se elimino la Armadora {0} ", make.Name)
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
                    Body = "Ocurrio un error al eliminar la armadora"
                });
            }
        }

        [HttpPost]
        public ActionResult DeleteModel(int id)
        {
            try
            {
                var model = db.CarModels.FirstOrDefault(m => m.IsActive && m.CarModelId == id);

                JResponse response = null;

                if (model != null)
                {
                    db.CarModels.Remove(model);
                    db.SaveChanges();

                    response = new JResponse
                    {
                        Result = Cons.Responses.Success,
                        Header = "Registro eliminado",
                        Code = Cons.Responses.Codes.Success,
                        Body = string.Format("Se elimino el modelo {0} ", model.Name)
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
                    Body = "Ocurrio un error al eliminar el modelo"
                });
            }
        }



        [HttpPost]
        public ActionResult SaveCarMake(CarMake carMake)
        {
            try
            {
                if (carMake.CarMakeId > Cons.Zero)
                    db.Entry(carMake).State = EntityState.Modified;
                else
                    db.CarMakes.Add(carMake);

                db.SaveChanges();

                var model = db.CarMakes.Where(w => w.IsActive).ToList();

                return PartialView("_CarMakeList", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al guardar",
                    Code = Cons.Responses.Codes.ErroSaving,
                    Body = "Ocurrio un error al guardar la Armadora " + carMake.Name
                });
            }
        }

        [HttpPost]
        public ActionResult SaveCarModel(CarModel carModel)
        {
            try
            {
                if (carModel.CarModelId > Cons.Zero)
                    db.Entry(carModel).State = EntityState.Modified;
                else
                    db.CarModels.Add(carModel);

                db.SaveChanges();

                var model = db.CarModels.Where(w => w.IsActive).ToList();

                return PartialView("_CarModelList", model);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Header = "Error al guardar",
                    Code = Cons.Responses.Codes.ErroSaving,
                    Body = "Ocurrio un error al guardar el Modelo " + carModel.Name
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
