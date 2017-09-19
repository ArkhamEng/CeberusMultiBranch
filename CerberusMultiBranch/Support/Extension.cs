using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.ViewModels.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;


namespace CerberusMultiBranch.Support
{
    public static class Extension
    {
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

        public static JCatalogEntity GetBranchSession()
        {
            JCatalogEntity session;

            if (System.Web.HttpContext.Current.Session[Cons.BranchSession]!=null)
                session = (JCatalogEntity)System.Web.HttpContext.Current.Session[Cons.BranchSession];
            else
                session = new JCatalogEntity { Id = 0, Name = string.Empty };

            return session;
        }

    }
}