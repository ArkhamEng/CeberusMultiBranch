
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
        public static bool DeleteExternal(int providerId)
        {
            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = conn.CreateCommand();
                com.CommandText = "[Catalog].[DelExternalProduct]";
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.Add(new SqlParameter { Value = providerId, ParameterName = "@ProviderId" });
              
                return com.ExecuteNonQuery() > 0 ? true : false;
            }
        }

        public static void BulkInsertBulkCopy(List<ExternalProduct> list)
        {
            var dt = ToDataTable(list);

            var cs = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

            using (var connection = new SqlConnection(cs))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.FireTriggers, null))
                {
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.ColumnMappings.Add("[ProviderId]", "[ProviderId]");
                    bulkCopy.ColumnMappings.Add("[Code]", "[Code]");
                    bulkCopy.ColumnMappings.Add("[Category]", "[Category]");
                    bulkCopy.ColumnMappings.Add("[Description]", "[Description]");
                    bulkCopy.ColumnMappings.Add("[Price]", "[Price]");
                    bulkCopy.ColumnMappings.Add("[TradeMark]", "[TradeMark]");
                    bulkCopy.ColumnMappings.Add("[Unit]", "[Unit]");
                    bulkCopy.DestinationTableName = "[Catalog].[ExternalProduct]";
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
    }
}