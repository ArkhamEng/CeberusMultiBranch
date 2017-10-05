using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploader
{
    class DataManager
    {
        public static bool AddTypes()
        {
            Console.Write("Revisando categorias .. \n");
            var sis = Excel.GetSystems();

            var cat = Excel.GetCategories();

            foreach (var s in sis)
            {
               if( SQLServer.GetSystemId(s)== 0)
                {
                    Console.Write("\rSistemas Agregados:" + s);
                    SQLServer.AddSystem(s);
                }
            }

            foreach (var c in cat)
            {
                if (SQLServer.GetCategoryId(c) == 0)
                {
                    Console.Write("\rCategoría agregadas:" + c );
                    SQLServer.AddCategory(c);
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
            Console.WriteLine("Agregados {0}  Errores {1} Omitidos {2}", ok,error,o);

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
