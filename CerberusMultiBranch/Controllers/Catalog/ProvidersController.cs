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
using System.Web;
using System;
using System.Xml.Serialization;
using System.IO;

namespace CerberusMultiBranch.Controllers.Catalog
{
    [CustomAuthorize]
    public class ProvidersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        public ActionResult Index()
        {
            var model = new SearchPersonViewModel<ProviderViewModel>();
            model.Persons = LookFor(null, null, null, null, null, Cons.MaxResults);

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(int? stateId, int? cityId, string name, string phone, int? id, bool quickSearch = false)
        {
            var top = quickSearch ? Cons.QuickResults : Cons.MaxResults;

            top = Cons.MaxResults;

            var model = LookFor(stateId, cityId, name,phone, id, top);

            if (!quickSearch)
                return PartialView("_List", model);
            else
                return PartialView("_ProviderQuickSearchList", model);
        }

        [HttpPost]
        public ActionResult ShowQuickSearch()
        {
            var model = LookFor(null, null, null, null, null, Cons.QuickResults);

            return PartialView("_ProviderQuickSearch", model);
        }


        private List<ProviderViewModel> LookFor(int? stateId, int? cityId, string name, string phone, int? id, int top)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var model = (from provider in db.Providers.Include(e => e.Addresses)
                         where
                             (id == null || provider.ProviderId == id) &&
                             (name == null || name == string.Empty || arr.Any(n => (provider.Code + " " + provider.Name).Contains(name))) &&
                             (phone == null || phone == string.Empty || provider.Phone == phone) &&
                             (provider.IsActive)
                         select new ProviderViewModel
                         {
                             ProviderId = provider.ProviderId,
                             Code = provider.Code,
                             Email = provider.Email,
                             Email2 = provider.Email2,
                             WebSite = provider.WebSite,
                             FTR = provider.FTR,
                             IsActive = provider.IsActive,
                             Name = provider.Name,
                             Phone = provider.Phone,
                             Phone2 = provider.Phone2,
                             Phone3 = provider.Phone3,
                             Agent = provider.Agent,
                             AgentPhone = provider.AgentPhone,
                             BusinessName = provider.BusinessName,
                             Catalog = provider.Catalog,
                             CreditLimit = provider.CreditLimit,
                             DaysToPay = provider.DaysToPay,
                             Line = provider.Line,
                             UpdDate = provider.UpdDate,
                             UpdUser = provider.UpdUser,

                             LockUser = provider.LockUser,
                             LockEndDate = provider.LockEndDate,
                             Addresses = provider.Addresses,
                         }).Take(top).OrderBy(c => c.Name).ToList();

            return model;
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Capturista,Supervisor")]
        public ActionResult BeginAdd(int? id)
        {
            ProviderViewModel model;

            if (id != null)
            {
                var provider = db.Providers.Include(p => p.Addresses).FirstOrDefault(p => p.ProviderId == id);
                model = new ProviderViewModel(provider);

                if (model.IsLocked)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.RecordLocked,
                        Header = "Registro bloqueado",
                        Body = "Este proveedor " + provider.Name.ToUpper() + " se encuentra bloqueado por " + model.LockUser + " y no puede ser editado hasta ser liberado",
                    });
                }

                //bloqueo del registro
                try
                {
                    provider.LockEndDate = DateTime.Now.ToLocal().AddMinutes(Cons.LockTimeOut);
                    provider.LockUser = HttpContext.User.Identity.Name;
                    model.LockEndDate = provider.LockEndDate;
                    model.LockUser = provider.LockUser;

                    db.Entry(provider).State = EntityState.Modified;

                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Danger,
                        Code = Cons.Responses.Codes.ServerError,
                        Header = "Error al bloquear",
                        Body = "Ocurrio un error al bloquear el proveedor " + provider.Name.ToUpper()
                    });
                }

                var stateId = model.Addresses.First().City.StateId;
                model.StateId = stateId;
                model.States = db.States.OrderBy(s => s.Name).ToSelectList(stateId);
                model.Cities = db.Cities.Where(c => c.StateId == model.StateId).OrderBy(c => c.Name).ToSelectList(model.Addresses.First().CityId);
            }
            else
                model = new ProviderViewModel();

            model.States = db.States.OrderBy(s => s.Name).ToSelectList();

            return PartialView("_ProviderEdition", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Capturista,Supervisor")]
        public ActionResult Save(Provider provider)
        {
            try
            {
                var response = new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success
                };

                if (provider.ProviderId == Cons.Zero)
                {

                    var exProv = db.Providers.FirstOrDefault(p => (!string.IsNullOrEmpty(p.FTR)  && p.FTR == provider.FTR) || p.Name == provider.Name);

                    if (exProv != null)
                    {
                        response.Result = Cons.Responses.Danger;
                        response.Code = Cons.Responses.Codes.ErroSaving;
                        response.Header = "Registro duplicado";
                        response.Body = string.Format("Ya existe un proveedor con nombre {0} y rfc {1}, no debe repetir Nombre o RFC", exProv.Name, exProv.FTR);

                        return Json(response);
                    }

                    provider.Code = db.Providers.Max(c => c.Code).ToCode();
                    db.Providers.Add(provider);

                    response.Header = "Nuevo Proveedor registrado";
                    response.Body = "El Proveedor " + provider.Name.ToUpperInvariant() +
                        " fue agregado correctamente";
                }
                else
                {

                    var exProv = db.Providers.FirstOrDefault(p => p.ProviderId != provider.ProviderId &&
                    (p.FTR == provider.FTR || p.Name == provider.Name));

                    if (exProv != null)
                    {
                        response.Result = Cons.Responses.Danger;
                        response.Code = Cons.Responses.Codes.ErroSaving;
                        response.Header = "Registro duplicado";
                        response.Body = string.Format("Ya existe un proveedor con nombre {0} y rfc {1}, no debe repetir Nombre o RFC", exProv.Name, exProv.FTR);

                        return Json(response);
                    }


                    db.Entry(provider).State = EntityState.Modified;
                    db.Entry(provider).Property(c => c.IsActive).IsModified = false;
                    db.Entry(provider).Property(c => c.Catalog).IsModified = false;
                    db.Entry(provider).Property(c => c.Code).IsModified = false;

                    foreach (var address in provider.Addresses)
                        db.Entry(address).State = EntityState.Modified;

                    response.Header = "Proveedor actualizado!";
                    response.Body = "Se registraron las modificaciones del proveedor " + provider.Name.ToUpperInvariant() +
                        " y se liberaron bloqueos sobre el registro";
                }

                db.SaveChanges();

                response.Id = provider.ProviderId;

                return Json(response);
            }

            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ErroSaving,
                    Header = "Error al guardar",
                    Body = "Ocurrio un error al guardar los datos del proveedor " +
                            provider.Name.ToUpperInvariant() + " Detalle:" + ex.Message
                });
            }

        }

        [HttpPost]
        public ActionResult UnLock(int id)
        {
            try
            {
                var provider = db.Providers.Find(id);
                provider.LockEndDate = null;
                provider.LockUser = null;

                db.Entry(provider).Property(p => p.LockUser).IsModified = true;
                db.Entry(provider).Property(p => p.LockEndDate).IsModified = true;

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Info,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Proveedor desbloqueado!",
                    Body = "Se libero el bloque del proveedor: " +
                          provider.Name.ToUpperInvariant()
                });
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ErroSaving,
                    Header = "Error al desbloquear",
                    Body = "Ocurrio un error al desbloquear el registro del Proveedor"
                });
            }

        }

        [HttpPost]
        public ActionResult Deactivate(int id)
        {
            try
            {
                var provider = db.Providers.Find(id);

                provider.UpdDate = DateTime.Now.ToLocal();
                provider.UpdUser = HttpContext.User.Identity.Name;
                provider.IsActive = false;

                db.Entry(provider).Property(p => p.IsActive).IsModified = true;
                db.Entry(provider).Property(p => p.UpdUser).IsModified = true;
                db.Entry(provider).Property(p => p.UpdDate).IsModified = true;

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Info,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Proveedor Eliminado",
                    Body = "Se elimino el proveedor: " +
                          provider.Name.ToUpperInvariant() + " del catálogo"
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Warning,
                    Code = Cons.Responses.Codes.ErroSaving,
                    Header = "Error al Eliminar",
                    Body = "Ocurrio un error al eliminar el registro de proveedor"
                });
            }
        }


        [HttpPost]
        public ActionResult AutoCompleate(string filter)
        {
            var model = db.Providers.OrderBy(p => p.Name).Where(p => p.Name.Contains(filter)).Take(20).
                Select(p => new { Id = p.ProviderId, Label = p.Name, Value = p.FTR });

            return Json(model);
        }

        [HttpPost]
        public ActionResult QuickSearch(string name)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var providers = (from p in db.Providers
                             where (name == null || name == string.Empty || arr.Any(n => (p.Code + " " + p.Name).Contains(name)))
                             select p).OrderBy(p => p.Name).Take(Cons.QuickResults).ToList();

            return PartialView("_QuickProviderList", providers);
        }

        [HttpPost]
        public ActionResult BeginUpdateCatalog(int id)
        {
            var p = db.Providers.FirstOrDefault(d => d.ProviderId == id);

            var pCount = db.ExternalProducts.Count(d => d.ProviderId == id);

            var pending = db.TempExternalProducts.Count(te => te.ProviderId == id);

            var model = new ProviderCatalogViewModel
            {
                ProviderName = p.Name,
                ProviderKey = id,
                ProductsCount = pCount,
                PendingCount = pending
            };

            return PartialView("_ProviderCatalogEdition", model);
        }

        [HttpPost]
        public ActionResult ProcessCatalog(int providerId)
        {
            try
            {
                var done = DBHelper.ProcessExternalProducts(providerId);

                return Json(new JResponse
                {
                    Header = "Productos Procesados",
                    Body = "Los productos cargados fueron procesados correctamente",
                    Id = providerId,
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success
                });
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Header = "Error al Procesar",
                    Body = "Ocurrio un error inesperado al cargar la lista de producto, revisa que no existan codigos repetidos",
                    Id = providerId,
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ErroSaving
                });
            }
        }

        [HttpPost]
        public ActionResult UpdateCatalog(int ProviderKey, string ProviderName, HttpPostedFileBase file)
        {
            RecordSet records = null;
            List<TempExternalProduct> toInsert = new List<TempExternalProduct>();
            DateTime begin = DateTime.Now;
            Record[] loadedProd = new List<Record>().ToArray();

            try
            {
                try
                {
                    //deserializo el xml y lo convierto en objeto
                    XmlSerializer xml = new XmlSerializer(typeof(RecordSet));
                    Stream myStr = file.InputStream;
                    records = (RecordSet)xml.Deserialize(file.InputStream);

                    records.Records.ForEach(r =>
                    {
                        var description = r.Description.Trim();

                        if (description.Length > Cons.DescriptionLength)
                            description = description.Substring(0, Cons.DescriptionLength);

                        TempExternalProduct ex = new TempExternalProduct
                        {
                            Code = r.Code.Trim().Length > 30 ? r.Code.Trim().Substring(0, 30) : r.Code,
                            ProviderId = ProviderKey,
                            Description = description,
                            TradeMark = r.TradeMark.Trim() ?? "N/A",
                            Unit = r.Unit.Trim() ?? "N/A",
                            Price = r.Price
                        };

                        toInsert.Add(ex);
                    });


                    xml = null;
                }
                catch (Exception)
                {
                    return Json(new JResponse
                    {
                        Header = "Error de Formato",
                        Body = "Ocurrio un error en la recepcion de los datos, verifica que estas cargando el archivo corrento y que no tienes datos incorrectos",
                        Id = ProviderKey,
                        Result = Cons.Responses.Danger,
                        Code = Cons.Responses.Codes.ErroSaving
                    });
                }


                if (toInsert.Count > Cons.Zero)
                {
                    try
                    {
                        DBHelper.BulkInsertBulkCopy(toInsert);
                    }
                    catch (Exception)
                    {
                        return Json(new JResponse
                        {
                            Header = "Error al Cargar",
                            Body = "Ocurrio un error inesperado al procesar la lista de producto, revisa que no existan codigos repetidos",
                            Id = ProviderKey,
                            Result = Cons.Responses.Warning,
                            Code = Cons.Responses.Codes.ErroSaving
                        });
                    }
                }

                var time = (DateTime.Now - begin).TotalMinutes;

                var response = new JResponse
                {
                    Header = "Productos cargados",
                    Body = string.Format("Se cargaron {0} productos al proveedor {1}, utiliza el botón 'Procesar' para concluir el proceso", 
                                        toInsert.Count, ProviderName),
                    Id = ProviderKey,
                    Result = Cons.Responses.Info,
                    Code = Cons.Responses.Codes.Success,
                    Extra = toInsert.Count.ToString()
                };

                return Json(response);
            }
            catch (Exception)
            {
                return Json(new JResponse
                {
                    Header = "Error al Cargar",
                    Body = "Ocurrio un error inesperado al cargar la lista de producto, revisa que no existan codigos repetidos",
                    Id = ProviderKey,
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ErroSaving
                });
            }
            finally
            {

                file.InputStream.Dispose();
                if (records != null)
                {
                    records.Records.Clear();
                    records = null;
                }

                toInsert.Clear();
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
