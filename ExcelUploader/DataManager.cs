using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploader
{
    public static class Ext
    {
        public static string Clear(this string data, int max)
        {
            data = data.Trim().Replace("*", string.Empty).Replace("-", string.Empty).
                            Replace("_", string.Empty).Replace(" ", string.Empty).Replace("/", string.Empty);

            if (data.Length > max)
                return data.Substring(0, max);
            else
                return data;
        }
    }
    class DataManager
    {

        public static bool AddTypes()
        {
            Console.Write("Revisando categorias .. \n");

            var sqlSis = SQLServer.GetSystems();
            var sqlCat = SQLServer.GetCategories();

            var sis = Excel.GetSystems();
            var cat = Excel.GetCategories();

            foreach (var s in sis)
            {
                var sys = sqlSis.FirstOrDefault(ss => ss.Name == s);
                if (sys == null)
                {
                    SQLServer.AddSystem(s);
                    Console.Write("\rSistemas Agregados:" + s);
                }
            }

            foreach (var c in cat)
            {
                var cate = sqlCat.FirstOrDefault(ss => ss.Name == c);
                if (cate == null)
                {
                    SQLServer.AddCategory(c);
                    Console.Write("\rCategoría agregadas:" + c);
                }
            }

            Console.Write("revisión de categorias completa \n");
            return true;
        }


        public static void ExportProducts()
        {
            List<string> codes = new List<string>();
            int error = 0;
            int ok = 0;
            var prodList = Excel.GetProducts();
            var it = 0;
            var o = 0;
            Console.Write("\n Revisando existencia en Base de datos \n");

            foreach (var prod in prodList)
            {
                it++;
                Console.Write("\r Productos revisados {0}", it);
                if (SQLServer.GetProductId(prod.Code) == 0)
                {
                    if (!SQLServer.AddProduct(prod))
                        error++;
                    else
                        ok++;
                }
                else
                {
                    o++;
                    codes.Add(prod.Code + " " + prod.Name);
                }
            }

            Console.WriteLine("Revision de productos terminada\n");
            Console.WriteLine("Agregados {0}  Errores {1} Omitidos {2}", ok, error, o);

            Console.WriteLine("Productos omitidos:");
            foreach (var c in codes)
                Console.WriteLine(c);
        }

        //public static bool AddProducts()
        //{

        //    List<Product> products = new List<Product>();

        //    foreach (var s in sis)
        //    {
        //        Console.Write("Agegando Sistema:" + s + "\n");
        //        SQLServer.AddSystem(s);
        //    }

        //    foreach (var c in cat)
        //    {
        //        Console.Write("Agegando Categoría:" + c + "\n");
        //        SQLServer.AddCategory(c);
        //    }

        //    return true;
        //}
    }
}
