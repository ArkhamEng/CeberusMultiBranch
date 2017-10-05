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
                        product.Reference = dr["Referencia"].ToString().Trim().Replace(" ",string.Empty);
                        product.StorePercentage = 30;
                        product.DealerPercentage = 20;
                        product.WholesalerPercentage = 16;

                        product.StorePrice = Math.Round(product.BuyPrice * (1 + (product.StorePercentage / 100)));
                        product.DealerPrice = Math.Round(product.BuyPrice * (1 + (product.DealerPercentage / 100)));
                        product.WholesalerPrice = Math.Round(product.BuyPrice * (1 + (product.WholesalerPercentage / 100)));

                        product.CategoryId = SQLServer.GetCategoryId(dr["Genero"].ToString().Trim());
                        product.PartSystemId = SQLServer.GetSystemId(dr["Sistema"].ToString().Trim());
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
