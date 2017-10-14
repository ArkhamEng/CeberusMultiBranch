using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploader
{

    public class Code
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class SQLServer
    {
        public static bool AddClient(Person client)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();

                com.CommandText = "[Catalog].[LoadClient]";
                com.CommandType = System.Data.CommandType.StoredProcedure;

                com.Parameters.Add(new SqlParameter { Value = client.CityId, ParameterName = "@CityId" });
                com.Parameters.Add(new SqlParameter { Value = client.Code, ParameterName = "@Code" });
                com.Parameters.Add(new SqlParameter { Value = client.Name, ParameterName = "@Name" });
                com.Parameters.Add(new SqlParameter { Value = client.BusinessName, ParameterName = "@BusinessName" });
                com.Parameters.Add(new SqlParameter { Value = client.LegalRepresentative, ParameterName = "@LegalRepresentative" });
                com.Parameters.Add(new SqlParameter { Value = client.FTR, ParameterName = "@FTR" });
                com.Parameters.Add(new SqlParameter { Value = client.TaxAddress, ParameterName = "@TaxAddress" });
                com.Parameters.Add(new SqlParameter { Value = client.Address, ParameterName = "@Address" });
                com.Parameters.Add(new SqlParameter { Value = client.ZipCode, ParameterName = "@ZipCode" });
                com.Parameters.Add(new SqlParameter { Value = client.Entrance, ParameterName = "@Entrance" });
                com.Parameters.Add(new SqlParameter { Value = client.Email, ParameterName = "@Email" });
                com.Parameters.Add(new SqlParameter { Value = client.Phone, ParameterName = "@Phone" });
                com.Parameters.Add(new SqlParameter { Value = client.IsActive, ParameterName = "@IsActive" });
                com.Parameters.Add(new SqlParameter { Value = client.UpdDate, ParameterName = "@UpdDate" });

                return com.ExecuteNonQuery() > 0 ? true : false;
            }
        }

        public static bool AddProvider(Person provider)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();

                com.CommandText = "[Catalog].[LoadProvider]";
                com.CommandType = System.Data.CommandType.StoredProcedure;

                com.Parameters.Add(new SqlParameter { Value = provider.CityId, ParameterName = "@CityId" });
                com.Parameters.Add(new SqlParameter { Value = provider.Code, ParameterName = "@Code" });
                com.Parameters.Add(new SqlParameter { Value = provider.Name, ParameterName = "@Name" });
                com.Parameters.Add(new SqlParameter { Value = provider.BusinessName, ParameterName = "@BusinessName" });
                com.Parameters.Add(new SqlParameter { Value = provider.FTR, ParameterName = "@FTR" });

                com.Parameters.Add(new SqlParameter { Value = provider.Address, ParameterName = "@Address" });
                com.Parameters.Add(new SqlParameter { Value = provider.ZipCode, ParameterName = "@ZipCode" });
                
                com.Parameters.Add(new SqlParameter { Value = provider.Email, ParameterName = "@Email" });
                com.Parameters.Add(new SqlParameter { Value = provider.Phone, ParameterName = "@Phone" });
                com.Parameters.Add(new SqlParameter { Value = provider.IsActive, ParameterName = "@IsActive" });
                com.Parameters.Add(new SqlParameter { Value = provider.UpdDate, ParameterName = "@UpdDate" });

                return com.ExecuteNonQuery() > 0 ? true : false;
            }
        }

        public static bool AddCategory(string name)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();

                com.CommandText = "INSERT INTO [Config].[Category]([Name]) VALUES('"+name+"')";
                return com.ExecuteNonQuery() > 0 ? true:false;
            }
        }

        public static List<Code> GetVariables()
        {
            List<Code> list = new List<Code>();
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();

                com.CommandText = "Select VariableId, Name, Value from [Config].[Variable]";
                var r = com.ExecuteReader();

                while (r.Read())
                {
                    var code = new Code();
                    code.Id = Convert.ToInt32(r["VariableId"]);
                    code.Name = r["Name"].ToString();
                    code.Value = r["Value"].ToString();

                    list.Add(code);
                }

                return list;
            }
        }


        public static List<Code> GetCategories()
        {
            List<Code> list = new List<Code>();
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();

                com.CommandText = "Select CategoryId, Name from [Config].[Category]";
                var r = com.ExecuteReader();

                while(r.Read())
                {
                    var code = new Code();
                    code.Id = Convert.ToInt32(r["CategoryId"]);
                    code.Name = r["Name"].ToString();

                    list.Add(code);
                }

                return list;
            }
        }

        public static List<Code> GetSystems()
        {
            List<Code> list = new List<Code>();
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();

                com.CommandText = "Select PartSystemId, Name from [Config].[PartSystem]";
                var r = com.ExecuteReader();

                while (r.Read())
                {
                    var code = new Code();
                    code.Id = Convert.ToInt32(r["PartSystemId"]);
                    code.Name = r["Name"].ToString();

                    list.Add(code);
                }

                return list;
            }
        }

        public static bool AddSystem(string name)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();

                com.CommandText = "INSERT INTO [Config].[PartSystem]([Name]) VALUES('" + name + "')";
                return com.ExecuteNonQuery() > 0 ? true : false;
            }
        }

        public static int GetProductId(string code)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();

                com.CommandText = "Select ProductId from [Catalog].[Product]  Where Code='" + code + "'";
                var r = com.ExecuteScalar();

                return r != null ? Convert.ToInt32(r) : 0;
            }
        }

        public static bool AddProduct(Product product)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();
                com.CommandText = "[Catalog].[LoadProduct]";
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.Add(new SqlParameter { Value = product.CategoryId, ParameterName = "@CategoryId" });
                com.Parameters.Add(new SqlParameter { Value = product.Code, ParameterName = "@Code" });
                com.Parameters.Add(new SqlParameter { Value = product.Name, ParameterName = "@Name" });
                com.Parameters.Add(new SqlParameter { Value = product.Description, ParameterName = "@Description" });
                com.Parameters.Add(new SqlParameter { Value = product.MinQuantity, ParameterName = "@MinQuantity" });
                
                com.Parameters.Add(new SqlParameter { Value = product.StorePercentage, ParameterName = "@StorePercentage" });
                com.Parameters.Add(new SqlParameter { Value = product.DealerPercentage, ParameterName = "@DealerPercentage" });
                com.Parameters.Add(new SqlParameter { Value = product.WholesalerPercentage, ParameterName = "@WholesalerPercentage" });

                com.Parameters.Add(new SqlParameter { Value = product.BuyPrice, ParameterName = "@BuyPrice" });
                com.Parameters.Add(new SqlParameter { Value = product.StorePrice, ParameterName = "@StorePrice" });
                com.Parameters.Add(new SqlParameter { Value = product.WholesalerPrice, ParameterName = "@WholesalerPrice" });
                com.Parameters.Add(new SqlParameter { Value = product.DealerPrice, ParameterName = "@DealerPrice" });
                com.Parameters.Add(new SqlParameter { Value = product.TradeMark, ParameterName = "@TradeMark" });
                com.Parameters.Add(new SqlParameter { Value = product.Unit, ParameterName = "@Unit" });

                return com.ExecuteNonQuery() > 0 ? true : false;
            }
        }

    }
}
