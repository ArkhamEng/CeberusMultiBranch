using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace CerberusMultiBranch.Controllers.Operative
{
    [CustomAuthorize(Roles ="Supervisor, Administrador")]
    public class OffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult ImageEditor()
        {
            return PartialView("_ImageEditor");
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Index()
        {
            var date = DateTime.Today.ToLocal();
            var offers = db.Offers.Where(o => o.IsActive && o.EndDate >= date).ToList();
            return View(offers);
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Search(string description)
        {

            var arr = description.Split(' ');

            var date = DateTime.Today.ToLocal();
            var offers = db.Offers.Where(o => o.IsActive 
            && ( string.IsNullOrEmpty(description) || arr.All(a=> (o.Name +" "+ o.Description).Contains(a)))
            ).ToList();

            return PartialView("_OfferList",offers);
        }


        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Editor(int? id)
        {
            var model = new Offer();

            if (id == null)
            {
                model.BeginDate = DateTime.Today.ToLocal();
                model.EndDate = DateTime.Today.AddMonths(Cons.One).ToLocal();
                model.ImagePath = Cons.NoImagePath;
                model.Type = OfferType.NoDiscount;
            }
            else
            {
                model = db.Offers.Find(id);
                model.HasImage = true;
            }
            
            
            ViewBag.Colors = new List<SelectListItem>
            { new SelectListItem    { Text="Gris",     Value= "#333" },
             new SelectListItem     { Text="Blanco",   Value= "#fffdfd" },
            new SelectListItem      { Text="Rojo",     Value= "#c9302c" },
            new SelectListItem      { Text="Amarillo", Value= "#fff159" } };


            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Save(Offer offer)
        {
            try
            {
                offer.InsUser = this.HttpContext.User.Identity.Name;
                offer.InsDate = DateTime.Now.ToLocal();

                //si es nuevo registro, se crea el codigo (Name)
                if (offer.OfferId == 0)
                {
                    var year = DateTime.Today.ToLocal().ToString("yy");
                    offer.Year = Convert.ToInt32(year);

                    var lastConsecutive = db.Offers.Where(o => o.Year == offer.Year)
                        .OrderByDescending(o => o.Consecutive).FirstOrDefault();

                    offer.Consecutive = lastConsecutive != null ? (lastConsecutive.Consecutive + Cons.One) : Cons.One;
                    offer.Name = string.Format("OF{0}{1}", year, offer.Consecutive.ToString("00000"));

                    offer.ImagePath = FileManager.SaveFile(offer.Base64, Cons.OfferImagesPath, offer.Name);

                    db.Offers.Add(offer);
                }

                else
                {
                    var dbOffer = db.Offers.Find(offer.OfferId);

                    if (!string.IsNullOrEmpty(offer.Base64))
                        offer.ImagePath = FileManager.SaveFile(offer.Base64, Cons.OfferImagesPath, offer.Name);
                    else
                        offer.ImagePath = dbOffer.ImagePath;

                    offer.Year = dbOffer.Year;
                    offer.Consecutive = dbOffer.Consecutive;
                    
                   
                    db.Entry(dbOffer).CurrentValues.SetValues(offer);
                    db.Entry(dbOffer).State = System.Data.Entity.EntityState.Modified;
                }

                await db.SaveChangesAsync();

                return Json(new JResponse
                {
                    Body = "Los datos de la oferta se guardaron correctamente",
                    Code = Cons.Responses.Codes.Success,
                    Id = offer.OfferId,
                    Header = "Cambios guardados",
                    Result = Cons.Responses.Success
                });
            }
            catch
            {
                return Json(new JResponse
                {
                    Body = "Ocurrio un error al guardar los datos de la oferta",
                    Code = Cons.Responses.Codes.ServerError,
                    Id = offer.OfferId,
                    Header = "Error al guardar",
                    Result = Cons.Responses.Danger
                });
            }
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var offer = await db.Offers.FindAsync(id);

                offer.IsActive = false;
                offer.InsDate = DateTime.Now.ToLocal();
                offer.InsUser = this.HttpContext.User.Identity.Name;

                FileManager.DropFile(offer.ImagePath);

                db.Entry(offer).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();

                var date = DateTime.Today.ToLocal();
                var offers = db.Offers.Where(o => o.IsActive && o.EndDate >= date).ToList();

                return PartialView("_OfferList", offers);
            }
            catch (Exception)
            {

                throw;
            }
          
        }
    }
}
