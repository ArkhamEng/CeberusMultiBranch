using CerberusMultiBranch.Models;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;


namespace CerberusMultiBranch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index1()
        {
            var branches = User.Identity.GetBranches();
            var bList = branches.Select(b => b.BranchId);

            List<string> sources = new List<string>();
            using (var db = new ApplicationDbContext())
            {
                var br = db.Branches.Include(b => b.Transactions).Where(b => bList.Contains(b.BranchId)).ToList();

                var cSales    = new Chart(800, 500, theme: ChartTheme.Blue);
                var cPurchase = new Chart(800, 500, theme: ChartTheme.Yellow);

                cSales.AddTitle("Ventas");
                cPurchase.AddTitle("Compras");
                List<double> sValues = new List<double>();
                List<double> pValues = new List<double>();

                List<string> names = new List<string>();

                foreach (var branch in br)
                {
                    sValues.Add(branch.Sales.Sum(t=> t.TotalAmount));

                    names.Add(branch.Name);
                    pValues.Add(branch.Purchases.Sum(t => t.TotalAmount));

                    cSales.AddLegend("Sucursales");
                    cPurchase.AddLegend("Sucursales");
                }


                cSales.AddSeries("Venta", chartType: "Column", yValues: sValues,xValue:names);
                cPurchase.AddSeries("Compra", chartType: "Column", yValues: pValues, xValue: names);


                var imgS = cSales.GetBytes();
                var baseS = Convert.ToBase64String(imgS);
                var srcS = String.Format("data:image/jpeg;base64,{0}", baseS);

                var imgP = cPurchase.GetBytes();
                var baseP = Convert.ToBase64String(imgP);
                var srcP = String.Format("data:image/jpeg;base64,{0}", baseP);

                sources.Add(srcS);
                sources.Add(srcP);
            }

            return View(sources);
        }


        public ActionResult Index()
        {
            return View();
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