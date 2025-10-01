using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DALTrainingCreation
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        static string strConnection = System.Configuration.ConfigurationManager.ConnectionStrings["TuvConnection"].ToString();

        public DataSet BindBranch()
        {
            DataSet DTBindBranch = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("SP_Training", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 2);
                SqlDataAdapter da = new SqlDataAdapter(CMDGetBranch);
                da.Fill(DTBindBranch);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTBindBranch.Dispose();
            }
            return DTBindBranch;

        }

        public DataSet BindCurrency()
        {
            DataSet DTBindBranch = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("SP_JobMaster", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 10);
                SqlDataAdapter da = new SqlDataAdapter(CMDGetBranch);
                da.Fill(DTBindBranch);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTBindBranch.Dispose();
            }
            return DTBindBranch;

        }


        public string Insert(TrainingCreationModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Training", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '1');
                cmd.Parameters.AddWithValue("@TrainingTopic", CPM.TrainingTopic);
                cmd.Parameters.AddWithValue("@EvaluationMethod", CPM.EvaluationMethod);
                cmd.Parameters.AddWithValue("@BranchId", CPM.BranchId);
                cmd.Parameters.AddWithValue("@Category", CPM.Category);
                cmd.Parameters.AddWithValue("@Other", CPM.Other);
                cmd.Parameters.AddWithValue("@TrainType", CPM.TrainType);
                cmd.Parameters.AddWithValue("@Remarks", CPM.Remarks);

                cmd.Parameters.AddWithValue("@ProposedDate", DateTime.ParseExact(CPM.ProposedDate, "dd/MM/yyyy", theCultureInfo) );
                

                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Training", con);
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

        public DataSet GetDataById(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Training", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '4');
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


        public string Update(TrainingCreationModel N, int id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Training", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@Id", N.Id);
                cmd.Parameters.AddWithValue("@TrainingTopic", N.TrainingTopic);
                cmd.Parameters.AddWithValue("@EvaluationMethod", N.EvaluationMethod);
                cmd.Parameters.AddWithValue("@BranchId", N.BranchId);
                cmd.Parameters.AddWithValue("@Category", N.Category);
                cmd.Parameters.AddWithValue("@Other", N.Other);
                cmd.Parameters.AddWithValue("@TrainType", N.TrainType);
                cmd.Parameters.AddWithValue("@Remarks", N.Remarks);
                cmd.Parameters.AddWithValue("@ProposedDate", DateTime.ParseExact(N.ProposedDate, "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
               // cmd.Parameters.AddWithValue("@ModifiedDateTime", N.ServiceCode);
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

        public string Delete(int id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Training", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '6');
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

        //Get Topic list For Auto Fill Textbox Field
        public DataSet GetTopicList(string prefix)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Training", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '7');
                cmd.Parameters.AddWithValue("@TrainingTopic", prefix);
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



    }
}