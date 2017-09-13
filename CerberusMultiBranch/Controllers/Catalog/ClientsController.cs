using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.ViewModels.Catalog;
using CerberusMultiBranch.Support;

namespace CerberusMultiBranch.Controllers.Catalog
{
    [Authorize]
    public class ClientsController : Controller
    {
        private ApplicationData db = new ApplicationData();

        // GET: Clients
        public ActionResult Index()
        {

            var model = new SearchClientViewModel();
            model.States = db.States.ToSelectList();
            model.Clients = db.Clients.Take(200).ToList();

            return View(model);
        }


        [HttpPost]
        public ActionResult Search(int? stateId, int? cityId, string code, string name, string phone)
        {
            var model = (from c in db.Clients
                         where
                             (name == null || c.Name.Contains(name)) &&
                             (stateId == null || c.City.StateId == stateId) &&
                             (cityId == null || c.CityId == cityId) &&
                             (phone == null || c.Phone == phone) &&
                             (code == null || c.Code == code)
                         select c).ToList();

            return PartialView("_List", model);
        }


        // GET: Clients/Create
        public ActionResult Create(int? id)
        {
            ClientViewModel model;

            if (id == null)
                model = new ClientViewModel();
            else
                model = CreateModel(db.Clients.Find(id));


            model.States = db.States.ToSelectList();
            return View(model);

        }

        private ClientViewModel CreateModel(Client client)
        {
            var model = new ClientViewModel(client);
            model.States = db.States.ToSelectList();
            model.StateId = db.Cities.Find(model.CityId).StateId;
            model.Cities = db.Cities.Where(c => c.StateId == model.StateId).ToSelectList();

            return model;
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            try
            {
                if (client.ClientId == Cons.Zero)
                {
                    client.Code = db.Clients.Max(c => c.Code).ToCode();
                    db.Clients.Add(client);
                }
                else
                    db.Entry(client).State = EntityState.Modified;

                db.SaveChanges();
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("Phone"))
                    ModelState.AddModelError("Phone", "El número "+client.Phone+" ya se encuentra registrado");

                if (ex.InnerException.InnerException.Message.Contains("BussinessName"))
                    ModelState.AddModelError("BusinessName", "Ya existe un cliente con el nombre "+client.BusinessName);

                var model = CreateModel(client);
                return View(model);
            }

            return RedirectToAction("Create", new { id = client.ClientId });
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
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
