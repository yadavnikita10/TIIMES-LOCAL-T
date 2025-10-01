using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    
        public static class DapperORM
        {
            private static string connectionString = "data source=mssql.worldindia.com;initial catalog=TestKioskApi;user id=TestKioskApi;password=MVAOr075P6fU;MultipleActiveResultSets=True";

            public static void ExecutewithoutReturn(string name, DynamicParameters Para = null)
            {

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    sqlCon.Execute(name, Para, commandType: CommandType.StoredProcedure);
                }
            }

            public static T ExecutewithoutScalar<T>(string name, DynamicParameters Para = null)
            {

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    return (T)Convert.ChangeType(sqlCon.Execute(name, Para, commandType: CommandType.StoredProcedure), typeof(T));
                }
            }


            public static IEnumerable<T> ReturnList<T>(string name, DynamicParameters Para = null)
            {

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    return sqlCon.Query<T>(name, Para, commandType: CommandType.StoredProcedure);
                }
            }


        }
    
}