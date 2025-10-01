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
    public class DALStampMaster
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        public string Insert(StampMaster SM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                if (SM.Id > 0) //Update
                {
                    SqlCommand cmd = new SqlCommand("SP_StampMaster", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '4');
                    cmd.Parameters.AddWithValue("@Attachment", SM.Attachment);
                    cmd.Parameters.AddWithValue("@ImageName", SM.ImageName);
                    cmd.Parameters.AddWithValue("@Quantity", SM.Quantity);
                    cmd.Parameters.AddWithValue("@Id", SM.Id);
                    cmd.Parameters.AddWithValue("@Type", SM.Type);
                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_StampMaster", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '1');
                    cmd.Parameters.AddWithValue("@Attachment", SM.Attachment);
                    cmd.Parameters.AddWithValue("@ImageName", SM.ImageName);
                    cmd.Parameters.AddWithValue("@Quantity", SM.Quantity);
                    cmd.Parameters.AddWithValue("@Type", SM.Type);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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
                SqlCommand cmd = new SqlCommand("SP_StampMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '2');
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

        public DataSet GetDataById(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_StampMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                cmd.Parameters.AddWithValue("@Id", id);
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

        //public string Update(GradeMaster Gm, int id)
        //{
        //    string Result = string.Empty;
        //    con.Open();
        //    try
        //    {

        //        SqlCommand cmd = new SqlCommand("SP_GradeMaster", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SP_Type", '4');
        //        cmd.Parameters.AddWithValue("@Id", Gm.Id);
        //        //cmd.Parameters.AddWithValue("@EmployeeGrade", Gm.EmployeeGrade);            
        //        cmd.Parameters.AddWithValue("@CarRate", Gm.CarRate);
        //        cmd.Parameters.AddWithValue("@MotorBikeRate", Gm.MotorBikeRate);
        //        cmd.Parameters.AddWithValue("@OPERate", Gm.OPERate);
        //        cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
        //        Result = cmd.ExecuteNonQuery().ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (con.State != ConnectionState.Closed)
        //        {
        //            con.Close();
        //        }
        //    }
        //    return Result;
        //}

        public string Delete(int id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_StampMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@Id", id);
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