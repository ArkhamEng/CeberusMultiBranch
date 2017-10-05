using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.ViewModels.Config;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;



namespace CerberusMultiBranch.Support
{
    public static class Extension
    {
        public static double GetPrice(this double buyPrice,int percentage)
        {
            return buyPrice * (Cons.One + (percentage / Cons.OneHundred));
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
                return (Convert.ToInt32(code) + Cons.One).ToString(Cons.CodeMask);
            else
                return (decimal.Zero + Cons.One).ToString(Cons.CodeMask);
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
                session =  Convert.ToInt32(claim.Value);
            else
                session = Cons.Zero;

            return session;
        }

        public static  List<Branch> GetBranches(this IIdentity user)
        {
            var userId = user.GetUserId();

            using (var db = new ApplicationDbContext())
            {
                var branches = db.EmployeeBranches.Include(eb => eb.Employee).
                             Where(eb => eb.Employee.UserId == userId).
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
    }
}