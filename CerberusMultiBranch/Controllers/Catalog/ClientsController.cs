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

namespace CerberusMultiBranch.Controllers.Catalog
{
    [Authorize]
    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clients
        public ActionResult Index()
        {
            var model = new SearchClientViewModel();
            model.States = db.States.OrderBy(s=> s.Name).ToSelectList();
            model.Clients = db.Clients.Where(c => c.ClientId != Cons.Zero).ToList();

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
        public ActionResult Search(int? stateId, int? cityId, string name, string ftr)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var model = (from c in db.Clients
                         where
                             (name      == null || name == string.Empty || arr.Any(n=> (c.Code+" "+ c.Name).Contains(name))) &&
                             (stateId   == null || c.City.StateId == stateId) &&
                             (cityId    == null || c.CityId == cityId) &&
                             (ftr     == null || ftr == string.Empty || c.FTR == ftr) &&
                             (c.ClientId != Cons.Zero)
                         select c).OrderBy(c=> c.Name).ToList();

            return PartialView("_List", model);
        }

        [HttpPost]
        public ActionResult QuickSearch(string name)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var model = (from p in db.Clients
                         where (name == null || name == string.Empty || arr.All(s => (p.Code + "" + p.Name).Contains(s)))
                         select p).OrderBy(c=> c.Name).Take(Cons.QuickResults).ToList();

            return PartialView("_QuickClientList", model);
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
            model.States = db.States.OrderBy(s=> s.Name).ToSelectList();
            model.StateId = db.Cities.Find(model.CityId).StateId;
            model.Cities = db.Cities.Where(c => c.StateId == model.StateId).OrderBy(c=> c.Name).ToSelectList();

            return model;
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Capturista,Vendedor")]
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
                {
                    db.Entry(client).State = EntityState.Modified;
                    db.Entry(client).Property(c => c.UsedAmount).IsModified = false;
                    db.Entry(client).Property(c => c.Code).IsModified = false;
                }
                    

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

        [HttpPost]
        public ActionResult GetClientCreditData(int id)
        {
            var client = db.Clients.Find(id);

            if(client.CreditDays == Cons.Zero)
                return Json(new { Result = "Error", Message = client.CreditComment ?? "No se han configurado los días de credito  para este cliente"});
            if (client.CreditAvailable <= Cons.Zero)
                return Json(new {Result = "Error", Message = "El cliente no tiene suficiente crédito" });

            return Json(new { Result = "OK", Days=client.CreditDays,
                Message = string.Format("Credito dispoible {0} con {1} días para pagar",client.CreditAvailable.ToMoney(), client.CreditDays) });
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
