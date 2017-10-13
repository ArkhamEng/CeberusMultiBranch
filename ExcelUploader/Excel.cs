using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploader
{
    public class Excel
    {

        // string con = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = C:\Users\ArkhamEng\Documents\LISTA CIOSA MAYO 2017.xls; Extended Properties = 'Excel 12.0;HDR=Yes'";

        public static List<string> GetCategories()
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Excel"].ToString();
            List<string> categories = new List<string>();
            using (OleDbConnection con = new OleDbConnection(cs))
            {
                con.Open();
                OleDbCommand command = con.CreateCommand();
                command.CommandText = "select Genero from[Precios$] group by Genero";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        categories.Add(dr["Genero"].ToString());
                    }
                }
            }
            return categories;
        }

        public static List<string> GetSystems()
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Excel"].ToString();
            List<string> systems = new List<string>();
            using (OleDbConnection con = new OleDbConnection(cs))
            {
                con.Open();
                OleDbCommand command = con.CreateCommand();
                command.CommandText = "select Sistema from[Precios$] group by Sistema";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        systems.Add(dr["Sistema"].ToString());
                    }
                }
            }
            return systems;
        }

        public static List<string> GetMakes()
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Excel"].ToString();
            List<string> systems = new List<string>();
            using (OleDbConnection con = new OleDbConnection(cs))
            {
                con.Open();
                OleDbCommand command = con.CreateCommand();
                command.CommandText = "select Sistema from[Precios$] group by Sistema";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        systems.Add(dr["Armadora"].ToString());
                    }
                }
            }
            return systems;
        }

        public static List<Product> GetProducts()
        {

            var sis = SQLServer.GetSystems();
            var cat = SQLServer.GetCategories();
            var variables = SQLServer.GetVariables();

            Console.Write("Buscando productos.. \n");
            List<Product> list = new List<Product>();
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Excel"].ToString();

            using (OleDbConnection con = new OleDbConnection(cs))
            {
                con.Open();
                OleDbCommand command = con.CreateCommand();
                command.CommandText = "select * from[Precios$]";
                var i = 0;
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var product = new Product();
                        product.TradeMark = dr["Marca"].ToString().Trim();

                        product.Unit = "Pieza";
                        product.Code = dr["Npc"].ToString().Trim().Replace(" ", string.Empty).Replace("-", string.Empty); 
                        product.Description = dr["Descripcion"].ToString().Trim();

                        var length = dr["Descripcion"].ToString().Trim().Length;
                        product.Name = dr["Descripcion"].ToString().Trim().Substring(0, length >= 100 ? 99 : length);
                      product.BuyPrice = Math.Round(Convert.ToDouble(dr["Precio Lista"]) / 1.16, 2);

                       // product.BuyPrice = Math.Round(Convert.ToDouble(dr["Precio Lista"]), 2);
                        product.Reference = dr["Referencia"].ToString().Trim().Replace(" ",string.Empty);

                        product.StorePercentage = Convert.ToInt32(variables.FirstOrDefault(v=> v.Name == "StorePercentage").Value);
                        product.DealerPercentage = Convert.ToInt32(variables.FirstOrDefault(v => v.Name == "DealerPercentage").Value);
                        product.WholesalerPercentage = Convert.ToInt32(variables.FirstOrDefault(v => v.Name == "WholesalerPercentage").Value);

                        var s = 1 + product.StorePercentage / 100d;
                        var d = 1 + product.DealerPercentage / 100d;
                        var w = 1 + product.WholesalerPercentage / 100d;

                        product.StorePrice = Math.Round(product.BuyPrice * s, 2);
                        product.DealerPrice = Math.Round(product.BuyPrice * d, 2);
                        product.WholesalerPrice = Math.Round(product.BuyPrice * w, 2);

                        var cName = dr["Genero"].ToString().Trim().ToUpper();
                        var cate = cat.FirstOrDefault(c => c.Name.Contains(cName));
                        product.CategoryId = cate.Id;


                        var sName = dr["Sistema"].ToString().Trim().ToUpper();
                        var sys = sis.FirstOrDefault(sy => sy.Name.Contains(sName));
                        product.PartSystemId =sys.Id;
                        i++;

                        Console.Write("\r Productos encontrados {0}", i);
                        list.Add(product);

                    }
                }
            }
            return list;
        }
    }
}
