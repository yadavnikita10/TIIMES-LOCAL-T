using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DALFieldInspectionMaster
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        public DataSet GetIAFScope()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_FieldInspectionMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '6');
                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }


        public DataSet GetDataById(int PK_FieldInspection)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_FieldInspectionMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@PK_FieldInspection", PK_FieldInspection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        
        public string Insert(FieldInspectionMaster FIM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (FIM.PK_FieldInspection > 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_FieldInspectionMaster", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@InspectionName", FIM.InspectionName);
                    cmd.Parameters.AddWithValue("@FK_IAFScopeId", FIM.FK_IAFScopeId);
                    cmd.Parameters.AddWithValue("@PK_FieldInspection", FIM.PK_FieldInspection);
                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_FieldInspectionMaster", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '1');
                    cmd.Parameters.AddWithValue("@InspectionName", FIM.InspectionName);
                    cmd.Parameters.AddWithValue("@FK_IAFScopeId", FIM.FK_IAFScopeId);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_FieldInspectionMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }


        public string Delete(int PK_FieldInspection)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_FieldInspectionMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@PK_FieldInspection", PK_FieldInspection);
                Result = cmd.ExecuteNonQuery().ToString();

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }



    }
}