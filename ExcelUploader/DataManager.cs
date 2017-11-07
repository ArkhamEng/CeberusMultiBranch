using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploader
{
    public static class Ext
    {



        public static DataTable ToDataDateble<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        public static string Clear(this string data, int max)
        {
            data = data.Trim().Replace("*", string.Empty).Replace("-", string.Empty).
                            Replace("_", string.Empty).Replace(" ", string.Empty).Replace("/", string.Empty).Replace("*", string.Empty);

            if (data.Length > max)
                return data.Substring(0, max);
            else
                return data;
        }

        public static string Truncate(this string data, int max)
        {
            data = data.Trim();

            if (data.Length > max)
                return data.Substring(0, max);
            else
                return data;
        }

        public static int ToInt(this object data)
        {
            var dt = data.ToString().Trim();
            int d;

            return int.TryParse(dt, out d) ? d : 0;

        }

        public static double ToDouble(this object data)
        {
            var dt = data.ToString().Trim();
            double d;

            return Math.Round((double.TryParse(dt, out d) ? d : 0), 2);

        }
    }
    class DataManager
    {
        public static void Begin()
        {
            var prod = Excel.GetProducts();
            Console.WriteLine("Agregando productos");
            int i = 0;
            int e = 0;
            foreach (var p in prod)
            {
                Console.WriteLine("Productos agregados 0");
                var done = false;
                try
                {
                    done = SQLServer.AddProduct(p);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    done = false;
                }

                if (done)
                {
                    i++;
                    Console.Write("\r Productos agregados {0}", i);
                }
                else
                {
                    e++;
                }
                   
            }
            Console.WriteLine("Resumen de Operación...");
            Console.WriteLine("Agregador {0}",i);
            Console.WriteLine("Fallidos  {0}",e);
        }


        public static void AddCatalogs()
        {
            var providers = AccessServer.GetProviders();

            var clients = AccessServer.GetClients();

            var categories = AccessServer.GetCategories();

            Console.WriteLine("Proveedores {0}  Clientes {1}   Categorias {2}", providers.Count, clients.Count, categories.Count);
            Console.WriteLine("Comenzando la exportación de proveedores");

            int prC = 0;
            foreach (var provider in providers)
            {
                var done = SQLServer.AddProvider(provider);
                if (done)
                {
                    prC++;
                    Console.Write("\rProveedores Agregados {0}", prC);
                }
            }

            Console.WriteLine("Comenzando la exportación de clientes");

            int cC = 0;
            foreach (var client in clients)
            {
                var done = SQLServer.AddClient(client);
                if (done)
                {
                    cC++;
                    Console.Write("\rClientes Agregados {0}", cC);
                }
            }


            Console.Write("Comenzando la exportación de Categorías");

            int caC = 0;
            foreach (var cat in categories)
            {
                var done = SQLServer.AddCategory(cat.Name);
                if (done)
                {
                    caC++;
                    Console.WriteLine("\rClientes Agregados {0}", caC);
                }
            }
        }

        public static void AddProducts()
        {
            Console.WriteLine("Obteninedo Catalogo de categorias");

            var sCat = SQLServer.GetCategories();

            Console.WriteLine("Categorias Cargadas {0}", sCat.Count);

            Console.WriteLine("Obteniendo productos a exportar....");

            var products = AccessServer.GetProducts(sCat);

            Console.WriteLine("Productos encontrados {0} Presiona una tecla para continuar", products.Count);

            Console.ReadLine();
            Console.WriteLine("Comenzando la exporacion de productos...");

            int pC = 0;
            foreach (var product in products)
            {
                var done = SQLServer.AddProduct(product);

                if (done)
                {
                    pC++;
                    Console.Write("\rProductos Agregados {0}", pC);
                }
            }
        }


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
