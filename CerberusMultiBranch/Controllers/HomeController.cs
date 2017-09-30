using CerberusMultiBranch.Models;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CerberusMultiBranch.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var branches = User.Identity.GetBranches();
            var bList = branches.Select(b => b.BranchId);

            List<string> sources = new List<string>();
            using (var db = new ApplicationDbContext())
            {
                foreach (var branch in branches)
                {
                    var sales = db.Sales.Where(t => t.BranchId == branch.BranchId && t.IsCompleated).ToList();
                    var sValues = sales.Select(s => s.TotalAmount).ToList();

                    var purchases = db.Purchases.Where(t => t.BranchId == branch.BranchId && t.IsCompleated).ToList();
                    var pValues = purchases.Select(p => p.TotalAmount).ToList();


                    var chart = new Chart(600, 400,theme: ChartTheme.Blue); 
                    chart.AddTitle("Sucursal " + branch.Name);
                    chart.AddSeries("Ventas", chartType: "Column",  yValues: sValues);
                    chart.AddSeries("Compras", chartType: "Column", yValues: pValues);

                    chart.AddLegend("Ventas vs Compras");

                    var imgB = chart.GetBytes();

                    var base64 = Convert.ToBase64String(imgB);
                    var imgSrc = String.Format("data:image/jpeg;base64,{0}", base64);

                    sources.Add(imgSrc);
                }
            }

            return View(sources);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}