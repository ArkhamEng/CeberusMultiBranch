using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploader
{
    public class AccessServer
    {
        public static List<Person> GetClients()
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Catalogs"].ToString();

            List<Person> Clients = new List<Person>();

            using (OleDbConnection con = new OleDbConnection(cs))
            {
                con.Open();
                OleDbCommand command = con.CreateCommand();
                command.CommandText = "Select * from MAE_CLIE WHERE (CLAVE <> '0000') AND (RAZON_SOC <> ' ') AND (NOMBRE_FISCAL <> ' ')";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Person client = new Person();
                        client.Name = dr["NOMBRE_FISCAL"].ToString().Trim();
                        client.BusinessName = dr["RAZON_SOC"].ToString().Trim(); 
                        client.Code = dr["CLAVE"].ToString().Trim();
                        client.Address = dr["DIRECCION"].ToString().Trim();
                        client.Email = dr["E_MAIL"].ToString().Trim().ToLower();
                        client.Phone  = dr["TEL_FAX"].ToString().Clear(20);
                        client.CityId = 2309;
                        client.UpdDate = DateTime.Now;
                        client.Entrance = DateTime.Now;
                        client.FTR    = dr["RFC"].ToString().Trim().Clear(13);
                        client.TaxAddress = dr["DIRECCION"].ToString().Trim() +" "+ dr["LUGAR_CP"].ToString().Trim();
                        client.IsActive = true;
                        client.LegalRepresentative = string.Empty;
                        client.ZipCode = string.Empty;
                        Clients.Add(client);
                    }
                }
            }
            return Clients;
        }

        public static List<Person> GetProviders()
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Catalogs"].ToString();

            List<Person> providers = new List<Person>();

            using (OleDbConnection con = new OleDbConnection(cs))
            {
                con.Open();
                OleDbCommand command = con.CreateCommand();
                command.CommandText = "Select * from MAE_PROV WHERE (CLAVE <> '0000') AND (RAZON_SOC <> ' ') AND (NOMBRE_FISCAL <> ' ')";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Person provider = new Person();
                        provider.Name = dr["NOMBRE_FISCAL"].ToString().Trim();
                        provider.BusinessName = dr["RAZON_SOC"].ToString().Trim();
                        provider.Code = dr["CLAVE"].ToString().Trim();
                        provider.Address = dr["DIRECCION"].ToString().Trim() + " " + dr["LUGAR_CP"].ToString().Trim();
                        provider.Email = dr["E_MAIL"].ToString().Trim().ToLower();
                        provider.Phone = dr["TEL_FAX"].ToString().Clear(20);
                        provider.CityId = 2309;
                        provider.UpdDate = DateTime.Now;
                        provider.FTR = dr["RFC"].ToString().Trim().Clear(13);
                        provider.IsActive = true;
                        provider.ZipCode = string.Empty;
                        providers.Add(provider);
                    }
                }
            }
            return providers;
        }

        public static List<Product> GetProducts(List<Code> categories)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["STOCK"].ToString();

            List<Product> products = new List<Product>();

            using (OleDbConnection con = new OleDbConnection(cs))
            {
                con.Open();
                OleDbCommand command = con.CreateCommand();
                command.CommandText = "SELECT MAE_ART.*, MAE_GEN.NOMB_GEN FROM(MAE_ART INNER JOIN MAE_GEN ON MAE_ART.GENERICO = MAE_GEN.GENERICO)";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var cat = dr["NOMB_GEN"].ToString().Trim();

                        Product product = new Product();
                        product.CategoryId = categories.FirstOrDefault(c => c.Name == cat).Id;

                        product.Name = dr["DESCRIP"].ToString().Truncate(200);
                        product.Description = dr["DESCRIP"].ToString().Truncate(200);
                        product.Code = dr["CLAVE"].ToString().Clear(30);
                        product.TradeMark = dr["MARCA"].ToString().Trim();
                        product.Unit = dr["UNIDAD"].ToString().Trim();
                        product.MinQuantity = dr["MINIMO"].ToInt();

                        product.BuyPrice                =Math.Round( dr["COSTO_ACT"].ToDouble() / 1 + (dr["IVA"].ToInt() /10d),2);
                        product.DealerPercentage        = dr["PORCEN_D"].ToInt();
                        product.WholesalerPercentage    = dr["PORCEN_Y"].ToInt();
                        product.StorePercentage         = dr["PORCEN_M"].ToInt();

                        product.StorePrice      = Math.Round(product.BuyPrice * (1 + (product.StorePercentage / 100d)),2);
                        product.DealerPrice     = Math.Round(product.BuyPrice * (1 + (product.DealerPercentage / 100d)),2);
                        product.WholesalerPrice = Math.Round(product.BuyPrice * (1 + (product.WholesalerPercentage / 100d)),2);
                        product.Reference       = dr["NOMB_GEN"].ToString().Trim();
                        
                        products.Add(product);
                    }
                }
            }
            return products;
        }


        public static List<Code> GetCategories()
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["STOCK"].ToString();

            List<Code> Categories = new List<Code>();

            using (OleDbConnection con = new OleDbConnection(cs))
            {
                con.Open();
                OleDbCommand command = con.CreateCommand();
                command.CommandText = "SELECT NOMB_GEN FROM MAE_GEN";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Code cat = new Code();
                        cat.Name = dr["NOMB_GEN"].ToString().Truncate(100);

                        Categories.Add(cat);
                    }
                }
            }
            return Categories;
        }

    }
}
