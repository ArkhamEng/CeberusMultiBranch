﻿using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.ViewModels.Config;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CerberusMultiBranch.Support
{
    public static class Extension
    {
        public static DateTime ToLocal(this DateTime serverDate)
        {
            DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverDate, 
                TimeZoneInfo.Local.Id, "Central Standard Time (Mexico)");

            return localTime;
        }

        public static DateTime TodayLocal(this DateTime serverDate)
        {
            DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverDate,
                TimeZoneInfo.Local.Id, "Central Standard Time (Mexico)");

            var today = new DateTime(localTime.Year, localTime.Month, localTime.Day);

            return today;
        }

        public static double GetPrice(this double buyPrice, int percentage)
        {
            return Math.Round( buyPrice * (Cons.One + (percentage / Cons.OneHundred)),Cons.Zero);
        }

        public static string Val(this string value)
        {
            return (value == null || value == string.Empty) ? null : value;
        }

        public static int ToInt(this string text)
        {
            if (text != null && text != string.Empty)
                return Convert.ToInt32(text);
            else
                return (int)decimal.Zero;
        }

        public static string ToCode(this string code)
        {
            if (code != null && code != string.Empty)
                return (Convert.ToInt32(code) + Cons.One).ToString(Cons.CodeSeqFormat);
            else
                return (decimal.Zero + Cons.One).ToString(Cons.CodeSeqFormat);
        }

        public static SelectList ToSelectList(this IEnumerable data)
        {
            return new SelectList(data, nameof(ISelectable.Id), nameof(ISelectable.Name));
        }

        public static byte[] ToCompressedFile(this HttpPostedFileBase stream)
        {
            byte[] b;

            using (MemoryStream target = new MemoryStream())
            {
                stream.InputStream.CopyTo(target);
                var bArr = target.ToArray();
                b = GzipWrapper.Compress(bArr);
            }

            return b;
        }

        public static void OrderCarModels(this IEnumerable<Product> products)
        {
            foreach (var p in products)
            {
                p.ModelCompatibilities = new List<CarModel>();
                foreach (var m in p.Compatibilities)
                {
                    if (!p.ModelCompatibilities.Contains(m.CarYear.CarModel))
                        p.ModelCompatibilities.Add(m.CarYear.CarModel);
                }
            }
        }

        public static void OrderCarModels(this Product product)
        {
            product.ModelCompatibilities = new List<CarModel>();
            foreach (var m in product.Compatibilities)
            {
                if (!product.ModelCompatibilities.Contains(m.CarYear.CarModel))
                    product.ModelCompatibilities.Add(m.CarYear.CarModel);
            }
        }


        public static int GetBranchId(this IIdentity user)
        {
            return user.GetBranchSession().Id;
        }

        public static string GetFolio(this IIdentity user,int sequential)
        {
            var id =  user.GetBranchSession().Id;
            string code = string.Empty;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                code =  db.Branches.Find(id).Code;
            }

            var yearPart = DateTime.Now.TodayLocal().Year.ToString().Substring(2,2);

            return string.Format(Cons.CodeMask, code, yearPart, sequential.ToString(Cons.CodeSeqFormat));
        }


        public static int GetSalePercentage(this IIdentity user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
               return db.Users.Find(user.GetUserId()).ComissionForSale;
            }
        }

        public static FileResult GetPicture(this IIdentity u)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string userId =  u.GetUserId();
                var user = db.Users.Find(userId);

                FileContentResult picture = null;

                if (user.PicturePath != null)
                {
                    byte[] imgdata = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath(user.PicturePath));
                    picture = new FileContentResult(imgdata, "img/jpg");
                }
                else
                {
                    byte[] imgdata = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("/Content/images/sinimagen.jpg"));
                    picture = new FileContentResult(imgdata, "img/jpg");
                }

                return picture;
            }
        }

        public static JCatalogEntity GetBranchSession(this IIdentity user)
        {
            var um = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userId = user.GetUserId();
            var claim = um.GetClaims(userId).FirstOrDefault(c => c.Type == Cons.BranchSession);

            JCatalogEntity session;

            if (claim != null)
            {
                var sArry = claim.Value.Split(',');
                session = new JCatalogEntity { Id = Convert.ToInt32(sArry[0]), Name = sArry[1] };
            }
            else
                session = new JCatalogEntity();

            return session;
        }

        public static int GetSaleId(this IIdentity user)
        {
            var um = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userId = user.GetUserId();
            var claim = um.GetClaims(userId).FirstOrDefault(c => c.Type == Cons.SaleSession);

            int session;

            if (claim != null)
                session = Convert.ToInt32(claim.Value);
            else
                session = Cons.Zero;

            return session;
        }

        public static List<Branch> GetBranches(this IIdentity user)
        {
            var userId = user.GetUserId();

            using (var db = new ApplicationDbContext())
            {
                var branches = db.UserBranches.Include(ub => ub.User).
                             Where(eb => eb.User.Id == userId).
                             Select(eb => eb.Branch).ToList();

                return branches;
            }
        }

        public static double GetStock(this IIdentity user, int productId)
        {
            var branchId = user.GetBranchSession().Id;
            double quantity = Cons.Zero;

            using (var db = new ApplicationDbContext())
            {
                var bprod = db.BranchProducts.FirstOrDefault(b => b.ProductId == productId && b.BranchId == branchId);
                quantity = bprod != null ? bprod.Stock : Cons.Zero;
            }

            return quantity;
        }

        public static string enletras(string num)
        {
            string res, dec = "";
            Int64 entero;
            int decimales;
            double nro;

            try

            {
                nro = Convert.ToDouble(num);
            }
            catch
            {
                return "";
            }

            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
            if (decimales > 0)
            {
                dec = " CON " + decimales.ToString() + "/100";
            }

            res = toText(Convert.ToDouble(entero)) + dec;
            return res;
        }

        public static string toText(this double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;
        }
    }
}