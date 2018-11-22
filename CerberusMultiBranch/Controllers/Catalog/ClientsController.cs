using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Support;
using CerberusMultiBranch.Models;
using System.Collections.Generic;
using System;

namespace CerberusMultiBranch.Controllers.Catalog
{
    [CustomAuthorize]
    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clients
        public ActionResult Index()
        {
            var model = new SearchPersonViewModel<ClientViewModel>();

            model.Persons = LookFor(null, null, null, null, null,Cons.MaxResults);

            return View(model);
        }

        [HttpPost]
        public ActionResult AutoCompleate(string filter)
        {
            var model = db.Clients.Where(p => p.Name.Contains(filter) && p.IsActive).OrderBy(c => c.Name).Take(20).
                Select(p => new { Id = p.ClientId, Label = p.Name.ToUpper(), Value = p.FTR.ToUpper() });

            return Json(model);
        }

        [HttpPost]
        public ActionResult Search(int? stateId, int? cityId, string name, string ftr, int? id, bool quickSearch = false)
        {
            var top = quickSearch ? Cons.QuickResults : Cons.MaxResults;

            var model = LookFor(stateId, cityId, name, ftr, id, top);

            if (!quickSearch)
                return PartialView("_List", model);
            else
                return PartialView("_ClientQuickSearchList", model);
        }


        [HttpPost]
        public ActionResult ShowQuickSearch()
        {
            var model = LookFor(null, null, null, null, null, Cons.QuickResults);

            return PartialView("_ClientQuickSearch", model);
        }


        private List<ClientViewModel> LookFor(int? stateId, int? cityId, string name, string ftr, int? id, int top)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var model = (from c in db.Clients.Include(c => c.Addresses)
                         where
                             (id == null || c.ClientId == id) &&
                             (name == null || name == string.Empty || arr.Any(n => (c.Code + " " + c.Name).Contains(name))) &&
                             (ftr == null || ftr == string.Empty || c.FTR == ftr) &&
                             (c.IsActive)
                         select new ClientViewModel
                         {
                             Address = c.Address,
                             BusinessName = c.BusinessName,
                             ClientId = c.ClientId,
                             Code = c.Code,
                             Email = c.Email,
                             Entrance = c.Entrance,
                             FTR = c.FTR,
                             IsActive = c.IsActive,
                             CreditLimit = c.CreditLimit,
                             Name = c.Name,
                             Phone = c.Phone,
                             UsedAmount = c.UsedAmount,
                             UpdDate = c.UpdDate,
                             ZipCode = c.ZipCode,
                             Type = c.Type,
                             CreditDays = c.CreditDays,
                             CreditComment = c.CreditComment,
                             LockEndDate = c.LockEndDate,
                             LockUser = c.LockUser,
                             Addresses = c.Addresses,
                         }).Take(top).OrderBy(c => c.Name).ToList();

            return model;
        }

        [HttpPost]
        public ActionResult QuickSearch(string name)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var model = (from p in db.Clients
                         where (name == null || name == string.Empty || arr.All(s => (p.Code + "" + p.Name).Contains(s)))
                         select p).OrderBy(c => c.Name).Take(Cons.QuickResults).ToList();

            return PartialView("_QuickClientList", model);
        }


        [HttpPost]
        public ActionResult BeginAdd(int? id)
        {
            ClientViewModel model;

            if (id != null)
            {
                var client = db.Clients.Include(c => c.Addresses).FirstOrDefault(c => c.ClientId == id);
                model = new ClientViewModel(client);

                if (model.IsLocked)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Warning,
                        Code = Cons.Responses.Codes.RecordLocked,
                        Header = "Registro bloqueado",
                        Body = "Este cliente " + client.Name.ToUpper() + " se encuentra bloqueado por " + model.LockUser + " y no puede ser editado hasta ser liberado",
                    });
                }

                //bloqueo del registro
                try
                {
                    client.LockEndDate = DateTime.Now.ToLocal().AddMinutes(Cons.LockTimeOut);
                    client.LockUser = HttpContext.User.Identity.Name;
                    model.LockEndDate = client.LockEndDate;
                    model.LockUser = client.LockUser;

                    db.Entry(client).State = EntityState.Modified;

                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(new JResponse
                    {
                        Result = Cons.Responses.Danger,
                        Code = Cons.Responses.Codes.ServerError,
                        Header = "Error al bloquear",
                        Body = "Ocurrio un error al bloquear el cliente " + client.Name.ToUpper()
                    });
                }

                var stateId = model.Addresses.First().City.StateId;
                model.StateId = stateId;
                model.States = db.States.OrderBy(s => s.Name).ToSelectList(stateId);
                model.Cities = db.Cities.Where(c => c.StateId == model.StateId).OrderBy(c => c.Name).ToSelectList(model.Addresses.First().CityId);

            }
            else
                model = new ClientViewModel();

            model.States = db.States.ToSelectList();

            return PartialView("_ClientEdition", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Capturista,Vendedor")]
        public ActionResult Save(Client client)
        {

            try
            {
                var response = new JResponse
                {
                    Result = Cons.Responses.Success,
                    Code = Cons.Responses.Codes.Success
                };

                if (client.ClientId == Cons.Zero)
                {
                    client.Code = db.Clients.Max(c => c.Code).ToCode();
                    db.Clients.Add(client);

                    response.Id = client.ClientId;
                    response.Header = "Nuevo Cliente registrado";
                    response.Body = "El Cliente " + client.Name.ToUpperInvariant() +
                        " fue agregado correctamente";
                }
                else
                {
                    db.Entry(client).State = EntityState.Modified;

                    db.Entry(client).Property(c => c.UsedAmount).IsModified = false;
                    db.Entry(client).Property(c => c.Code).IsModified = false;
                    db.Entry(client).Property(c => c.IsActive).IsModified = false;

                    foreach (var address in client.Addresses)
                        db.Entry(address).State = EntityState.Modified;

                    response.Id = client.ClientId;
                    response.Header = "Datos del cliente actualizados!";
                    response.Body = "Se registraron las modificaciones del Cliente " + client.Name.ToUpperInvariant() +
                        " y se libero el bloqueo sobre el registro";
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
                    Header = "Error al guardar los datos" + client.Name.ToUpperInvariant(),
                    Body = "Ocurrio un error al guardar los datos del cliente " +
                           client.Name.ToUpperInvariant() + " Detalle:" + ex.Message
                });
            }
        }


        [HttpPost]
        public ActionResult UnLock(int id)
        {
            try
            {

                var client = db.Clients.Find(id);
                client.LockEndDate = null;
                client.LockUser = null;

                db.Entry(client).Property(p => p.LockUser).IsModified = true;
                db.Entry(client).Property(p => p.LockEndDate).IsModified = true;

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Info,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Registro desbloqueado",
                    Body = "Se libero el bloque del Cliente: " +
                          client.Name.ToUpperInvariant()
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Danger,
                    Code = Cons.Responses.Codes.ErroSaving,
                    Header = "Error al guardar desbloquear",
                    Body = "Ocurrio un error al desbloquear el registro del Cliente"
                });
            }

        }


        [HttpPost]
        public ActionResult Deactivate(int id)
        {
            try
            {
                var client = db.Clients.Find(id);

                client.UpdDate = DateTime.Now.ToLocal();
                client.UpdUser = HttpContext.User.Identity.Name;
                client.IsActive = false;

                db.Entry(client).Property(p => p.IsActive).IsModified = true;
                db.Entry(client).Property(p => p.UpdUser).IsModified = true;
                db.Entry(client).Property(p => p.UpdDate).IsModified = true;

                db.SaveChanges();

                return Json(new JResponse
                {
                    Result = Cons.Responses.Info,
                    Code = Cons.Responses.Codes.Success,
                    Header = "Cliente Eliminado",
                    Body = "Se elimino el Cliente: " +
                          client.Name.ToUpperInvariant() + ", ya no podrá se usado en ninguna operación"
                });
            }
            catch (Exception ex)
            {
                return Json(new JResponse
                {
                    Result = Cons.Responses.Warning,
                    Code = Cons.Responses.Codes.ErroSaving,
                    Header = "Error al Eliminar",
                    Body = "Ocurrio un error al eliminar el registro de cliente"
                });
            }
        }




        [HttpPost]
        public ActionResult GetClientCreditData(int id)
        {
            var client = db.Clients.Find(id);

            if (client.CreditDays == Cons.Zero)
                return Json(new { Result = "Error", Message = client.CreditComment ?? "No se han configurado los días de credito  para este cliente" });
            if (client.CreditAvailable <= Cons.Zero)
                return Json(new { Result = "Error", Message = "El cliente no tiene suficiente crédito" });

            return Json(new
            {
                Result = "OK",
                Days = client.CreditDays,
                Message = string.Format("Credito dispoible {0} con {1} días para pagar", client.CreditAvailable.ToMoney(), client.CreditDays)
            });
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
