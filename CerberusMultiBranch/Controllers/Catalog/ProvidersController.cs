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
    [Authorize]
    public class ProvidersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Providers
        public ActionResult Index()
        {
            var model = new SearchProviderViewModel();
            model.Providers = db.Providers.ToList();
            model.States = db.States.ToSelectList();

            return View(model);
        }

        [HttpPost]
        public ActionResult AutoCompleate(string filter)
        {
            var model = db.Providers.Where(p => p.Name.Contains(filter)).Take(20).
                Select(p => new { Id = p.ProviderId, Label = p.Name, Value = p.FTR });

            return Json(model);
        }

        [HttpPost]
        public ActionResult Search(int? stateId, int? cityId, string name, string ftr)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var model = (from c in db.Providers
                         where
                             (name == null || name == string.Empty || arr.Any(n => (c.Code + " " + c.Name).Contains(name))) &&
                             (stateId == null || c.City.StateId == stateId) &&
                             (cityId == null || c.CityId == cityId) &&
                             (ftr == null || ftr == string.Empty || c.FTR == ftr)
                         select c).ToList();

            return PartialView("_List", model);
        }

        // GET: Providers/Create
        public ActionResult Create(int? id)
        {
            ProviderViewModel model;
            if (id != null)
            {
                model = CreateModel(db.Providers.Find(id));
            }
            else
                model = new ProviderViewModel();

            model.WebSite = model.WebSite ?? "http://";
            model.States = db.States.ToSelectList();
            return View(model);

        }

        private ProviderViewModel CreateModel(Provider provider)
        {
            var model = new ProviderViewModel(provider);
            model.States = db.States.ToSelectList();
            model.StateId = db.Cities.Find(model.CityId).StateId;
            model.Cities = db.Cities.Where(c => c.StateId == model.StateId).ToSelectList();

            return model;
        }

        // POST: Providers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Provider provider)
        {
            provider.UdpUser = User.Identity.Name;
            provider.UpdDate = DateTime.Now.ToLocal();

            if (provider.ProviderId == Cons.Zero)
            {
                provider.Code = db.Providers.Max(c => c.Code).ToCode();
                db.Providers.Add(provider);
            }
            else
                db.Entry(provider).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Create", new { id = provider.ProviderId });
        }

        [HttpPost]
        public ActionResult QuickSearch(string name)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var providers = (from p in db.Providers
                             where (name == null || name == string.Empty || arr.Any(n => (p.Code + " " + p.Name).Contains(name)))
                             select p).Take(Cons.QuickResults).ToList();

            return PartialView("_QuickProviderList", providers);
        }

        public ActionResult UpdateCatalog(int id)
        {
            var p = db.Providers.Include(d=> d.ExternalProducts).FirstOrDefault(d=> d.ProviderId == id);

            ViewBag.Name = p.Name;
            ViewBag.Code = p.Code;
            ViewBag.ProviderId = id;
            ViewBag.Products = p.ExternalProducts.Count;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCatalog(int providerId, string name, HttpPostedFileBase file)
        {
            RecordSet records = null;
            List<ExternalProduct> extProds = new List<ExternalProduct>();
            List<ExternalProduct> toInsert = new List<ExternalProduct>();
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

                    if (records != null)
                    {
                        for(int i=0;i<records.Records.Count;i++)
                        {
                            if(toInsert.FirstOrDefault(ti=> ti.Code == records.Records[i].Code.Trim()) == null)
                            {
                                var description = records.Records[i].Description;

                                if (description.Length > 200)
                                    description = description.Substring(0, 199);

                                ExternalProduct ex = new ExternalProduct
                                {
                                    Code = records.Records[i].Code.Trim(),
                                    ProviderId = providerId,
                                    Description = description,
                                    TradeMark = records.Records[i].TradeMark ?? "N/A",
                                    Unit = records.Records[i].Unit ?? "N/A",
                                    Category = records.Records[i].Category ?? "N/A",
                                    Price = records.Records[i].Price
                                };
                                toInsert.Add(ex);
                            }
                               
                        }
                    }
                       
                    xml = null;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deserializar el archivo", ex.InnerException);
                }


                if (toInsert.Count > Cons.Zero)
                {
                    try
                    {
                        DBHelper.DeleteExternal(providerId);
                        //agrego el rango de productos nuevos al data context
                        DBHelper.BulkInsertBulkCopy(toInsert);

                        var provider = db.Providers.Find(providerId);
                        provider.Catalog = toInsert.Count();
                        provider.UdpUser = User.Identity.Name;
                        provider.UpdDate = DateTime.Now.ToLocal();

                        db.Entry(provider).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al agregar nuevos registros", ex);
                    }
                }

                var time = (DateTime.Now - begin).TotalMinutes;
                ViewBag.Result = "OK";
                ViewBag.Message = string.Format("Operación concluida, se agregaron {0} ", toInsert.Count);
                ViewBag.ProviderId = providerId;
                ViewBag.Name = name;
                ViewBag.Products = toInsert.Count;
                return View();

            }
            catch (Exception ex)
            {
                var time = (DateTime.Now - begin).TotalMinutes;
                ViewBag.Result = ex.Message;
                ViewBag.Message = "Se encontro un error inesperado al insertar los registros";
                ViewBag.ProviderId = providerId;
                ViewBag.Name = name;
                return View();
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
