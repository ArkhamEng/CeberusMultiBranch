using CerberusMultiBranch.Models.Entities.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}