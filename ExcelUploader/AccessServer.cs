using System;
using System.Collections.Generic;
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
                command.CommandText = "Select * from MAE_CLIE";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Person client = new Person();
                        client.Name = dr["NOMBRE_FISCAL"].ToString().Trim();
                        client.BusinessName = dr["RAZON_SOC"].ToString().Trim(); 
                        client.Code = dr["CLAVE"].ToString().Trim();
                        client.Address = dr["DIRECCION"].ToString().Trim();
                        client.Email = dr["E_MAIL"].ToString().Trim();
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
                command.CommandText = "Select * from MAE_PROV";

                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Person provider = new Person();
                        provider.Name = dr["NOMBRE_FISCAL"].ToString().Trim();
                        provider.BusinessName = dr["RAZON_SOC"].ToString().Trim();
                        provider.Code = dr["CLAVE"].ToString().Trim();
                        provider.Address = dr["DIRECCION"].ToString().Trim() + " " + dr["LUGAR_CP"].ToString().Trim();
                        provider.Email = dr["E_MAIL"].ToString().Trim();
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
    }
}
