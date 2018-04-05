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

        public static List<string> GetEstados()
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Excel"].ToString();
            List<string> categories = new List<string>();
            using (OleDbConnection con = new OleDbConnection(cs))
            {
                con.Open();
                OleDbCommand command = con.CreateCommand();
                command.CommandText = "select Estado from[Códigos Postales$] group by Estado";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        categories.Add(dr["Estado"].ToString());
                    }
                }
            }
            return categories;
        }

        public static List<string> GetMunicipality()
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Excel"].ToString();
            List<string> categories = new List<string>();
            using (OleDbConnection con = new OleDbConnection(cs))
            {
                con.Open();
                OleDbCommand command = con.CreateCommand();
                command.CommandText = "select Municipio, Estado from[Códigos Postales$] group by Municipio, Estado";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        categories.Add(dr["Municipio"].ToString()+"|"+dr["Estado"].ToString());
                    }
                }
            }
            return categories;
        }

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
                        product.CategoryId  = 1;
                        product.Code        = dr["Clave"].ToString().Trim();
                        product.Name = dr["Descripcion"].ToString().Trim();
                        product.TradeMark      = dr["Marca"].ToString().Trim();
                        product.Unit           = dr["Unidad"].ToString().Trim();
                        product.MinQuantity    =dr["Minimo"].ToInt();
                        product.BuyPrice       =dr["Precio"].ToDouble();
                        product.StorePercentage      = dr["Mostrador"].ToInt();
                        product.DealerPercentage     = dr["Distribuidor"].ToInt();
                        product.WholesalerPercentage = dr["Mayorista"].ToInt();
                        

                        if(product.BuyPrice <=0)
                        {
                            product.StorePrice = 0;
                            product.DealerPrice = 0;
                            product.WholesalerPrice = 0;
                        }
                        else
                        {
                            product.StorePrice = Math.Round(product.BuyPrice * (1 +(product.StorePercentage / 100.00)),0);
                            product.DealerPrice =Math.Round(product.BuyPrice * (1 + (product.DealerPercentage / 100.00)),0); 
                            product.WholesalerPrice = Math.Round(product.BuyPrice * (1 + (product.WholesalerPercentage / 100.00)),0); 
                        }
                        product.IsActive = true;
                        product.UpdUser  = "Automatico";
                        product.UpdDate  = DateTime.Now;

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
