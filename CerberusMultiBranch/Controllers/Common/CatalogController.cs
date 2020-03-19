using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.ApiModel;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace CerberusMultiBranch.Controllers.Common
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CatalogController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/Catalog
        [HttpGet]
        [Route("api/Products/Search/{page}/{pageSize}/{category}/{description}/{minPrice}/{maxPrice}/{tradeMarks}")]
        [ResponseType(typeof(PagedResult<ProductModel>))]
        public async Task<IHttpActionResult> GetProducts(int page, int pageSize, string description,
                                                         int category, double minPrice, double maxPrice, string tradeMarks)
        {
            var filter = new SearchFilter
            {
                PageSize = pageSize,
                Page    = page,
                Description = description == Cons.NullString ? null : description,
                Category    = category,
                MinPrice    = minPrice,
                MaxPrice    = maxPrice,
                TradeMarks = tradeMarks == Cons.NullString ? new List<string>() : tradeMarks.Split(',').ToList()
            };

            var paged = await this.SearchProducts(filter);

            return Ok(paged);
        }

        [HttpGet]
        [Route("api/Offers/All")]
        [ResponseType(typeof(List<OfferModel>))]
        public async Task<IHttpActionResult> GetOffers()
        {
            var categories = new List<OfferModel>();
            var url = Request.RequestUri.GetLeftPart(UriPartial.Authority);


            await Task.Factory.StartNew(() =>
            {
                 categories.Add(new OfferModel { Name = "Oferta Chevy",
                    Description = "Palanca de velocidades Chevy 84-10", Image = url + Cons.OfferImagesPath + "/offer1.jpeg", Price = 1320 });
                 categories.Add(new OfferModel { Name = "Oferta mes patrio",
                    Description = "Fascia Tsuro, trasera  y deltantera 94-17", Image = url + Cons.OfferImagesPath + "/offer2.jpeg", Price = 260 });
                 categories.Add(new OfferModel { Name = "Afinación Chevy",
                    Description = "Kit de Afinación Chevy AC Delco", Image = url + Cons.OfferImagesPath + "/offer4.jpeg", Price = 490 });
                 categories.Add(new OfferModel { Name = "Afinación Aveo",
                    Description = "Garrafa de aceite,bujias y filtros de aire, gasolina y aceite", Image = url + Cons.OfferImagesPath + "/offer5.jpeg", Price = 650 });
            });

            return Ok(categories);
        }

        [HttpGet]
        [Route("api/Offers/Active")]
        [ResponseType(typeof(List<OfferModel>))]
        public async Task<IHttpActionResult> GetActiveOffers()
        {
            var offers = new List<OfferModel>();
            var url = Request.RequestUri.GetLeftPart(UriPartial.Authority);

            await Task.Factory.StartNew(() =>
            {
                var date = DateTime.Now.ToLocal();

               offers = db.Offers.Where(o => o.IsActive && o.EndDate >= date).Select(o => new OfferModel 
                {
                    Image = url + o.ImagePath,
                    Id = o.OfferId,
                    Description = o.Description,
                    Discount = o.Discount,
                    Name = o.Name,
                    TextShadow = "-1px 0 " + o.ShadowColor + ", 0 1px " + o.ShadowColor + ", 1px 0 " + o.ShadowColor + ", 0 -1px "+ o.ShadowColor,
                    TextColor = o.TextColor
                
                }).ToList();
            });

            return Ok(offers);
        }

        [HttpGet]
        [Route("api/Categories/All")]
        [ResponseType(typeof(List<CategoryModel>))]
        public async Task<IHttpActionResult> GetCategories()
        {
            var categories = await db.Systems.Where(c => c.IsActive).
                                        OrderBy(c => c.Name).Select(s => new CategoryModel
                                        {
                                            Id = s.PartSystemId,
                                            Description = s.Name.Trim().ToUpper(),
                                            Name = s.Name.Trim().ToUpper(),
                                        }).ToListAsync();

            return Ok(categories);
        }

        [HttpGet]
        [Route("api/TradeMarks/Search/{category}/{description}/{minPrice}/{maxPrice}/{tradeMarks}")]
        [ResponseType(typeof(List<TradeMark>))]
        public async Task<IHttpActionResult> GetTradeMarks(string description,
                                                         int category, double minPrice, double maxPrice, string tradeMarks)
        {
            var filter = new SearchFilter
            {
                Description = description == Cons.NullString ? null : description,
                Category = category,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                TradeMarks = tradeMarks == Cons.NullString ? new List<string>() : tradeMarks.Split(',').ToList()
            };

            var response = await SearchQuery(filter).GroupBy(p => p.TradeMark.Trim().ToUpper()).OrderBy(g => g.Key).
                            Select(g => new TradeMark { Name = g.Key, Quantity = g.Count() }).ToListAsync();

            return Ok(response);
        }

        [HttpGet]
        [Route("api/Branches/All")]
        [ResponseType(typeof(List<BranchModel>))]
        public async Task<IHttpActionResult> GetBranches()
        {
            var url = Request.RequestUri.GetLeftPart(UriPartial.Authority);

            var branches = await db.Branches.Where(b=> b.ShowInMap).Select(b => new BranchModel
            {
                Id        = b.BranchId,
                Address   = b.Address.Trim().ToUpper(),
                Phone     = b.Phone.Trim().ToUpper(),
                Latitude  = b.Latitude,
                Longitude = b.Longitude,
                Image     = url + b.Image,
                Name      = b.Name.Trim().ToUpper()

            }).ToListAsync();

            return Ok(branches);
        }



        private async Task<PagedResult<ProductModel>> SearchProducts(SearchFilter filter)
        {
            return await Task.Factory.StartNew(() =>
            {
                var paged = this.SearchQuery(filter).OrderBy(p=> p.Name).GetPaged(filter.Page, filter.PageSize);

                var url = Request.RequestUri.GetLeftPart(UriPartial.Authority);

                var model = new PagedResult<ProductModel>();
                model.CurrentMax = paged.CurrentMax;
                model.CurrentMin = paged.CurrentMin;
                model.CurrentPage = paged.CurrentPage;
                model.RowCount = paged.RowCount;
                model.PagesCount = paged.PagesCount;
                model.PageSize = paged.PageSize;
                model.PageCount = paged.PageCount;

                model.Results = paged.Results.Select(p => new ProductModel
                {
                    Id        = p.ProductId,
                    Code      = p.Code.ToUpper(),
                    TradeMark = p.TradeMark.Trim().ToUpper(),
                    Price     = p.OnlinePrice,
                    Unit      = p.Unit.ToUpper(),
                    Name      = string.IsNullOrEmpty(p.ShortName) ? "" :p.ShortName.ToUpper(),
                    Description = p.Name.ToUpper(),
                    Images = p.Images.Count > 0 ? p.Images.Select(i => url + i.Path).ToList() :
                    new List<string> { url + Cons.NoImagePath },
                    Stock = p.BranchProducts.Sum(b => b.Stock),
                    Category = p.System != null ? new CategoryModel { Id = p.System.PartSystemId, Name = p.Category.Name.ToUpper() } : null
                }).ToList();

                return model;
            });
        }

        private  IQueryable<Product> SearchQuery(SearchFilter filter)
        {

            string[] arr = !string.IsNullOrEmpty(filter.Description) ? filter.Description.Trim().Split(' ') : new List<string>().ToArray();

            if (arr.Length == Cons.One)
                arr[Cons.Zero] = Regex.Replace(arr[Cons.Zero], "[^a-zA-Z0-9]+", "");

            var query = (from p in db.Products.Include(p => p.Images).Include(p => p.Compatibilities).
                   Include(p => p.BranchProducts).Include(p => p.Compatibilities.Select(c => c.CarYear)).
                   Include(p => p.Compatibilities.Select(c => c.CarYear.CarModel))

                         where (p.BranchProducts.Where(bp => bp.Branch.IsWebStore).Count() > Cons.Zero) &&
                               (p.IsActive && p.IsOnlineSold) &&
                               (string.IsNullOrEmpty(filter.Description) || arr.All(s => (p.Code + " "+p.ShortName+" " + p.Name + " " + p.TradeMark).Contains(s))) &&
                               (filter.Category == Cons.Zero || p.System.PartSystemId == filter.Category) &&
                               (filter.TradeMarks.Count == Cons.Zero || filter.TradeMarks.Contains(p.TradeMark)) &&
                               (filter.MaxPrice == Cons.Zero ||
                                    p.BranchProducts.Where(bp => bp.StorePrice >= filter.MinPrice && bp.StorePrice <= filter.MinPrice).Count() > Cons.Zero) 
                              

                         select p);

            return query;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}