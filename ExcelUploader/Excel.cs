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
                command.CommandText = "select * from[Catalogo$]";
                var i = 0;
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var product = new Product();
                        product.Code        = dr["CODIGO"].ToString().Trim();
                        product.Description = dr["DESCRIPCION"].ToString().Trim();
                        product.TradeMark   = dr["MARCA"].ToString().Trim();
                        product.Unit        = dr["UNIDAD"].ToString().Trim();
                        product.Category    = dr["CATEGORIA"].ToString().Trim();
                        product.Price       =Convert.ToDouble( dr["PRECIO"].ToString().Trim());
                        i++;

                        Console.Write("\r Productos encontrados {0}", i);
                        list.Add(product);
                    }
                }
            }
            return list;
        }


        public static  bool SetProcess(string codigo)
        {
            try
            {
                Console.Write("Actualizando...");
                var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Excel"].ToString();

                using (OleDbConnection con = new OleDbConnection(cs))
                {
                    con.Open();
                    OleDbCommand command = con.CreateCommand();
                    command.CommandText = "Update [Catalogo$] SET Procesado ='SI' where Codigo='" + codigo + "'";

                    return command.ExecuteNonQuery() > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        
        }
    }
}
