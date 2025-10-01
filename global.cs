using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TuvVision
{
    public class global
    {
        public static DataTable checkLogIn(string UserName, string UserId, string useValue, string sessionId, string InfoString)
        {
            SqlConnection con = new SqlConnection(Convert.ToString(System.Configuration.ConfigurationManager.ConnectionStrings["TuvConnection"]));
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_ActiveUsers";
            cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 100));
            cmd.Parameters["@UserName"].Value = UserName;

            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar, 1000));
            cmd.Parameters["@UserId"].Value = Convert.ToString(UserId);

            cmd.Parameters.Add(new SqlParameter("@Use", SqlDbType.Int));
            cmd.Parameters["@Use"].Value = useValue;

            cmd.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.NVarChar));
            cmd.Parameters["@SessionId"].Value = sessionId;

            cmd.Parameters.Add(new SqlParameter("@UserData", SqlDbType.NVarChar));
            cmd.Parameters["@UserData"].Value = InfoString;

            con.Open();
            //object result = cmd.ExecuteScalar();

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            con.Close();

            return dataTable;
        }

        public static void SessionModule(string UserName,string UserId,string useValue,string sessionId, string InfoString)
        {
            SqlConnection con = new SqlConnection(Convert.ToString(System.Configuration.ConfigurationManager.ConnectionStrings["TuvConnection"]));
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_ActiveUsers";
            cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 100));
            cmd.Parameters["@UserName"].Value = UserName;

            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar, 1000));
            cmd.Parameters["@UserId"].Value = Convert.ToString(UserId);

            cmd.Parameters.Add(new SqlParameter("@Use", SqlDbType.Int));
            cmd.Parameters["@Use"].Value = useValue;

            cmd.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.NVarChar));
            cmd.Parameters["@SessionId"].Value = sessionId;

            cmd.Parameters.Add(new SqlParameter("@UserData", SqlDbType.NVarChar));
            cmd.Parameters["@UserData"].Value = InfoString;

            con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();
        }

        
        public static void ActivityPing(string UserId,string SessionId, string status)
        {
            SqlConnection con = new SqlConnection(Convert.ToString(System.Configuration.ConfigurationManager.ConnectionStrings["TuvConnection"]));
            SqlCommand cmd = new SqlCommand("SELECT * FROM ActiveUsers where UserId = '"+UserId+ "' and SessionID =  '" + SessionId+"'", con);
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            con.Close();

            

            if (dataTable.Rows.Count > 0)
            {
                con.Open();
                SqlCommand cmds = new SqlCommand("INSERT INTO tbl_SessionActivity (UserId, SessionID,ActivityCheckTime,SessionStatus) VALUES (@Value1, @Value2,@Value3,@Value4)", con);
                  
                cmds.Parameters.Add(new SqlParameter("@Value1", SqlDbType.NVarChar, 100));
                cmds.Parameters["@Value1"].Value = UserId;

                cmds.Parameters.Add(new SqlParameter("@Value2", SqlDbType.NVarChar, 100));
                cmds.Parameters["@Value2"].Value = SessionId;

                cmds.Parameters.Add(new SqlParameter("@Value3", SqlDbType.DateTime, 100));
                cmds.Parameters["@Value3"].Value = DateTime.Now;

                cmds.Parameters.Add(new SqlParameter("@Value4", SqlDbType.NVarChar, 100));
                cmds.Parameters["@Value4"].Value = status;
                
                int rowsAffected = cmds.ExecuteNonQuery();
                con.Close();
                
            }
        }

        

    }
}