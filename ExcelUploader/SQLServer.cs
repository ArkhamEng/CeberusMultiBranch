using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploader
{
    public class SQLServer
    {
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

        public static int GetCategoryId(string name)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();

                com.CommandText = "Select CategoryId from [Config].[Category]  Where Name='" + name + "'";
                var r = com.ExecuteScalar();

               return r != null ? Convert.ToInt32(r) : 0;
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

        public static int GetSystemId(string name)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["Sql"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();

                com.CommandText = "Select PartSystemId from [Config].[PartSystem]  Where Name='" + name + "'";
                var r = com.ExecuteScalar();

                return r != null ? Convert.ToInt32(r) : 0;
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

                com.CommandText = GetCommand(product);
                return com.ExecuteNonQuery() > 0 ? true : false;
            }
        }

        private static string GetCommand(Product prod)
        {
            var text = "INSERT INTO [Catalog].[Product]";
            text += "([CategoryId],[Code],[Name],[Description],[MinQuantity],[BarCode],[BuyPrice],[StorePercentage],[DealerPercentage],";
            text += "[WholesalerPercentage],[StorePrice],[WholesalerPrice],[DealerPrice],[MinimunPrice],[TradeMark],[Unit],[PartSystemId],[Reference]) ";
            text += string.Format("VALUES({0},'{1}','{2}','{3}',{4},'{5}',{6},{7},{8},{9},{10},{11},{12},{13},'{14}','{15}',{16},'{17}')",
                prod.CategoryId,prod.Code,prod.Name,prod.Description,prod.MinQuantity,prod.BarCode,prod.BuyPrice,prod.StorePercentage,prod.DealerPercentage,
                prod.WholesalerPercentage,prod.StorePrice,prod.WholesalerPrice,prod.DealerPrice,prod.MinimunPrice,prod.TradeMark,prod.Unit,prod.PartSystemId,prod.Reference);
           return text;
        }
    }
}
