
using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CerberusMultiBranch.Support
{
    public class DBHelper
    {
        const string connectionName = "LocalConnection";
        public static bool ProcessExternalProducts(int providerId)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();
                com.CommandTimeout = 180000;
                com.CommandText = "[Catalog].[ProcessExternalProducts]";
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.Add(new SqlParameter { Value = providerId, ParameterName = "@ProviderId" });
              
                return com.ExecuteNonQuery() > 0 ? true : false;
            }
        }

        public static bool SetProductState(int productId, int branchId, string user, bool isLocked)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();
                com.CommandText = "[Catalog].[LockProduct]";
                com.CommandType = CommandType.StoredProcedure;

                DateTime? dateLock = DateTime.Now.ToLocal();

               
                com.Parameters.Add(new SqlParameter { Value = productId, ParameterName = "@ProductId" });
                com.Parameters.Add(new SqlParameter { Value = branchId, ParameterName  = "@BranchId" });
                com.Parameters.Add(new SqlParameter { Value = dateLock, ParameterName  = "@DateLock" });
                com.Parameters.Add(new SqlParameter { Value = user, ParameterName      = "@UserLock" });
                com.Parameters.Add(new SqlParameter { Value = isLocked, ParameterName  = "@IsLocked" });

                var done = com.ExecuteNonQuery() > 0 ? true : false;

                return done;
            }
        }

        public static void BulkInsertBulkCopy(List<TempExternalProduct> list)
        {
            var dt = ToDataTable(list);

            var cs = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ToString();

            using (var connection = new SqlConnection(cs))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.FireTriggers, null))
                {
                    bulkCopy.BatchSize = 5000;
                    bulkCopy.ColumnMappings.Add("[ProviderId]", "[ProviderId]");
                    bulkCopy.ColumnMappings.Add("[Code]", "[Code]");                    
                    bulkCopy.ColumnMappings.Add("[Description]", "[Description]");
                    bulkCopy.ColumnMappings.Add("[Price]", "[Price]");
                    bulkCopy.ColumnMappings.Add("[TradeMark]", "[TradeMark]");
                    bulkCopy.ColumnMappings.Add("[Unit]", "[Unit]");
                    bulkCopy.DestinationTableName = "[Catalog].[TempExternalProduct]";
                    bulkCopy.WriteToServer(dt);
                }
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static bool SearchProduct(int branchId, string[] words)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();
                com.CommandText = "[Catalog].[SearchProductsByWords]";
                com.CommandType = CommandType.StoredProcedure;

                DateTime? dateLock = DateTime.Now.ToLocal();


                //com.Parameters.Add(new SqlParameter { Value = productId, ParameterName = "@ProductId" });
                //com.Parameters.Add(new SqlParameter { Value = branchId, ParameterName = "@BranchId" });
                //com.Parameters.Add(new SqlParameter { Value = dateLock, ParameterName = "@DateLock" });
                //com.Parameters.Add(new SqlParameter { Value = user, ParameterName = "@UserLock" });
                //com.Parameters.Add(new SqlParameter { Value = isLocked, ParameterName = "@IsLocked" });

                var done = com.ExecuteNonQuery() > 0 ? true : false;

                return done;
            }
        }
    }
}