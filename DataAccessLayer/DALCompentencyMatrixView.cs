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
    public class DALCompentencyMatrixView
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        //public DataSet GetDataById(int Id)
        public DataSet GetDataById(String Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@Ids", Id);
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

        
        public string Insert(CompentencyMetrixView SM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (SM.CandidateId == null)
                {
                    SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@Ids", SM.CandidateId);
                    cmd.Parameters.AddWithValue("@CandidateName", SM.CandidateName);
                    cmd.Parameters.AddWithValue("@Location", SM.Location);
                    cmd.Parameters.AddWithValue("@EducationalQualification", SM.EducationalQualification);
                    cmd.Parameters.AddWithValue("@AdditionalQualification", SM.AdditionalQualification);
                    cmd.Parameters.AddWithValue("@Designation", SM.Designation);
                    cmd.Parameters.AddWithValue("@EmailId", SM.EmailId);
                    cmd.Parameters.AddWithValue("@CellPhoneNumber", SM.CellPhoneNumber);
                    //cmd.Parameters.AddWithValue("@JoiningDate", SM.JoiningDate1);
                    cmd.Parameters.AddWithValue("@JoiningDate", DateTime.ParseExact(SM.JoiningDate1, "dd/MM/yyyy", null));
                    cmd.Parameters.AddWithValue("@TotalExperienceInYears", SM.TotalExperienceInYears);
                    cmd.Parameters.AddWithValue("@NumberOfYearsWithTUVIndia", SM.NumberOfYearsWithTUVIndia);
                    cmd.Parameters.AddWithValue("@CheckBoxValue", SM.CheckBoxValue);
                    cmd.Parameters.AddWithValue("@CandidateId", SM.CandidateId);
                    
                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '1');
                    cmd.Parameters.AddWithValue("@Ids", SM.CandidateId);
                    cmd.Parameters.AddWithValue("@CandidateName", SM.CandidateName);
                    cmd.Parameters.AddWithValue("@Location", SM.Location);
                    cmd.Parameters.AddWithValue("@EducationalQualification", SM.EducationalQualification);
                    cmd.Parameters.AddWithValue("@AdditionalQualification", SM.AdditionalQualification);
                    cmd.Parameters.AddWithValue("@Designation", SM.Designation);
                    cmd.Parameters.AddWithValue("@EmailId", SM.EmailId);
                    cmd.Parameters.AddWithValue("@CellPhoneNumber", SM.CellPhoneNumber);
                    //cmd.Parameters.AddWithValue("@JoiningDate", SM.JoiningDate1);
                    cmd.Parameters.AddWithValue("@JoiningDate", DateTime.ParseExact(SM.JoiningDate1, "dd/MM/yyyy", null));
                    cmd.Parameters.AddWithValue("@TotalExperienceInYears", SM.TotalExperienceInYears);
                    cmd.Parameters.AddWithValue("@NumberOfYearsWithTUVIndia", SM.NumberOfYearsWithTUVIndia);
                    cmd.Parameters.AddWithValue("@CheckBoxValue", SM.CheckBoxValue);
                    cmd.Parameters.AddWithValue("@CandidateId", SM.CandidateId);
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

        public DataTable dtGetNAME()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
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

        public DataTable dtGetNAME1()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1N");
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


        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
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

        public DataSet GetDataN()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
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

        public DataSet GetMasterDataN()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "3MN");
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataSet GetCandidateName(string prefix)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '6');
                cmd.Parameters.AddWithValue("@CandidateName", prefix);
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


        public DataSet GetCandidateDetail(string Category)
        {
            DataSet ds = new DataSet();
            try
            {
                //Category = Category.Split(' ')[0];
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '7');

                cmd.Parameters.AddWithValue("@CandidateName", Category);
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


        public string Delete(int id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '8');
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

        public DataSet GetApprovalDataN()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 50;
                cmd.Parameters.AddWithValue("@SP_Type", "3AN");
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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