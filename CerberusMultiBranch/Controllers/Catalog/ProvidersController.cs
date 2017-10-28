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
            model.Providers = db.Providers.Take(100).ToList();
            model.States = db.States.ToSelectList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(int? stateId, int? cityId, string name, string phone)
        {
            string[] arr = new List<string>().ToArray();

            if (name != null && name != string.Empty)
                arr = name.Trim().Split(' ');

            var model = (from c in db.Providers
                         where
                             (name == null || name == string.Empty || arr.Any(n => (c.Code + " " + c.Name).Contains(name))) &&
                             (stateId == null || c.City.StateId == stateId) &&
                             (cityId == null || c.CityId == cityId) &&
                             (phone == null || phone == string.Empty || c.Phone == phone)
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
            var p = db.Providers.Find(id);

            ViewBag.Name = p.Name;
            ViewBag.Code = p.Code;
            ViewBag.ProviderId = id;
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
                        loadedProd = records.Records.ToArray();

                    xml = null;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deserializar el archivo", ex.InnerException);
                }

                try
                {
                    //obtengo de base de datos los productos existentes
                    extProds = db.ExternalProducts.Where(ex => ex.ProviderId == providerId).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al consultar los datos existentes", ex.InnerException);
                }

                int j = 0;

                List<ExternalProduct> toUpdate = new List<ExternalProduct>();
                for (int i = 0; i < loadedProd.Length; i++)
                {
                    var up = extProds.FirstOrDefault(p => p.Code == loadedProd[i].Code);

                    if (up != null)
                    {
                        if (up.Price != loadedProd[i].Price)
                        {
                            up.Price = loadedProd[i].Price;
                            up.TradeMark = loadedProd[i].TradeMark;
                            up.Unit = loadedProd[i].Unit;

                            toUpdate.Add(up);
                        }
                    }


                    else
                    {
                        string d = string.Empty;
                        if (loadedProd[i].Description.Length > 200)
                            d = loadedProd[i].Description.Substring(0, 199);
                        else
                            d = loadedProd[i].Description;

                        toInsert.Add(new ExternalProduct
                        {
                            Description = d,
                            Category = loadedProd[i].Category ?? "N/A",
                            Code = loadedProd[i].Code,
                            ProviderId = providerId,
                            TradeMark = loadedProd[i].TradeMark ?? "N/A",
                            Unit = loadedProd[i].Unit ?? "N/A",
                            Price = loadedProd[i].Price
                        });
                    }
                }

                if (toInsert.Count > Cons.Zero)
                {
                    try
                    {
                        //agrego el rango de productos nuevos al data context
                        DBHelper.BulkInsertBulkCopy(toInsert);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al agregar nuevos registros", ex.InnerException);
                    }
                }

                var time = (DateTime.Now - begin).TotalMinutes;
                ViewBag.Result = "OK";
                ViewBag.Message = string.Format("Operación concluida, se agregaron {0} y se actualizaron {1} tiempo de ejecucion {2}", toInsert.Count, j, time.ToString());
                ViewBag.ProviderId = providerId;
                ViewBag.Name = name;
                return View();

            }
            catch (Exception ex)
            {
                var time = (DateTime.Now - begin).TotalMinutes;
                ViewBag.Result = ex.Message;
                ViewBag.Message = "Tiempo de ejecucion " + time.ToString("hh:mm:ss") + " Detalle" + ex.InnerException;
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
