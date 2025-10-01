using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DalInternaltionUsers
    {

        SqlConnection Con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string Userid = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public DataTable CreateUser(string Firstname, string lastname, string DOB, string role,string EncryptedPass, string CoreStudy)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CreateInternationalUser", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");
                cmd.Parameters.AddWithValue("@FirstName", Firstname);
                cmd.Parameters.AddWithValue("@LastName", lastname);
                cmd.Parameters.AddWithValue("@DOB", DOB);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@Createdby", Userid);
                cmd.Parameters.AddWithValue("@Password", "Pass@123");
                cmd.Parameters.AddWithValue("@EncryptedPassword", EncryptedPass);
                cmd.Parameters.AddWithValue("@CoreStudy", CoreStudy);
                //cmd.Parameters.AddWithValue("@Qualification", Qualification);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                
            }
            catch(Exception ex)
            {
                string error = ex.Message;
            }
            finally
            {
                dt.Dispose();
            }
            return dt;

        }



        public DataSet GetData()
        {

            DataSet dt = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CreateInternationalUser", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "2");
                cmd.Parameters.AddWithValue("@Createdby", Userid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            finally
            {
                dt.Dispose();
            }
            return dt;

        }



        public DataSet Activate_Deactivate_User(string PK_UserID,string isActive)
        {
            DataSet dt = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CreateInternationalUser", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "3");
                cmd.Parameters.AddWithValue("@Createdby", PK_UserID);
                cmd.Parameters.AddWithValue("@isActive", isActive);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            finally
            {
                dt.Dispose();
            }
            return dt;

        }

        public DataTable InsertDataUser()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CreateInternationalUser", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "2");
                cmd.Parameters.AddWithValue("@Createdby", Userid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            finally
            {
                dt.Dispose();
            }
            return dt;

        }


        public DataTable GetDataForCV(string id)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CreateInternationalUser", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@Createdby", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            finally
            {
                dt.Dispose();
            }
            return dt;

        }

        public DataTable InsertUpdateCVData(string id)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CreateInternationalUser", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@Createdby", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            finally
            {
                dt.Dispose();
            }
            return dt;

        }

        public DataTable UpdateProfile_CV(string id)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CreateInternationalUser", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@Createdby", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            finally
            {
                dt.Dispose();
            }
            return dt;

        }


        //public string InsertQualificationDetail(Education URS, int id, int chkVerified, string pk_userid)
        //{
        //    string Result1 = string.Empty;
        //    Con.Open();
        //    try
        //    {
        //        //if (UserPrimaryKey > 0)
        //        //{
        //            SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", Con);
        //            CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", "16");
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@Course", URS.Course);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@Degree", URS.Degree);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@MajorFieldOfStudy", URS.FOS);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@University", URS.University);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@LastYearPerc", URS.LastYearCGPA);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@AggregatePerc", URS.Aggregate);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@YearOfPassing", URS.Year);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@UserPrimaryKey", id);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@chkVerified", chkVerified);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", pk_userid);
        //            Result1 = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (Con.State != ConnectionState.Closed)
        //        {
        //            Con.Close();
        //        }
        //    }
        //    return Result1;
        //}
        //public string InsertCVData(string UserID, string ExpertiseSummary, string Inspection, DataTable Employmentdata, DataTable Projectdata, DataTable Trainingdata, DataTable Achievementdata)
        //{
        //    string Result1 = string.Empty;
        //    Con.Close();
        //    Con.Open();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_InsertCVData", Con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@UserID", UserID);
        //        cmd.Parameters.AddWithValue("@Inspection", Inspection);
        //        cmd.Parameters.AddWithValue("@ExpertiseSummary",ExpertiseSummary);
        //        cmd.Parameters.AddWithValue("@IsInspectionCheck", 0);
        //        cmd.Parameters.AddWithValue("@Employmentdata", Employmentdata);
        //        cmd.Parameters.AddWithValue("@Projectdata", Projectdata);
        //        cmd.Parameters.AddWithValue("@Trainingdata", Trainingdata);
        //        cmd.Parameters.AddWithValue("@Achievementdata", Achievementdata);
        //        Result1 = cmd.ExecuteNonQuery().ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (Con.State != ConnectionState.Closed)
        //        {
        //            Con.Close();
        //        }
        //    }
        //    return Result1;
        //}

        //public DataTable UpdateProfile_CV(Users externalcv, string id)
        //{
        //    DataTable dt = new DataTable();
        //    Con.Close();
        //    Con.Open();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("Sp_ExternalAgencyCV", Con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@MiddleName", externalcv.MiddleName);
        //        DateTime dob = DateTime.Parse(externalcv.DOB);
        //        cmd.Parameters.AddWithValue("@DOB", dob);
        //        cmd.Parameters.AddWithValue("@Gender", externalcv.Gender);
        //        cmd.Parameters.AddWithValue("@Marital_Status", externalcv.Marital_Status);
        //        cmd.Parameters.AddWithValue("@LanguageSpoken", externalcv.LanguageSpoken);
        //        cmd.Parameters.AddWithValue("@Image", externalcv.Image);
        //        cmd.Parameters.AddWithValue("@Nationality", externalcv.Nationality);
        //        cmd.Parameters.AddWithValue("@TotalyearofExprience", externalcv.TotalyearofExprience);
        //        cmd.Parameters.AddWithValue("@Agency", externalcv.Agency);
        //        cmd.Parameters.AddWithValue("@CreatedBy", id);                
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //        Con.Close();
        //    }
        //    finally
        //    {
        //        dt.Dispose();
        //    }
        //    return dt;

        //}


        //public string InsertProfsionalCerts(Proffestional URS, int UserPrimaryKey, int chkVerified)
        //{
        //    string Result1 = string.Empty;
        //    Con.Open();
        //    try
        //    {
        //        if (UserPrimaryKey > 0)
        //        {
        //            SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", Con);
        //            CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", "22");

        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@CertName", URS.certification);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@CertNo", URS.certificationNO);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@CertIssueDate", DateTime.ParseExact(URS.issuedate, "dd/MM/yyyy", theCultureInfo));

        //            if (URS.validtill != null)
        //                CMDInsertUpdateUsers.Parameters.AddWithValue("@CertValidTill", DateTime.ParseExact(URS.validtill, "dd/MM/yyyy", theCultureInfo));


        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@UserPrimaryKey", Convert.ToString(UserPrimaryKey));
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@chkVerified", chkVerified);

        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
        //            Result1 = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (Con.State != ConnectionState.Closed)
        //        {
        //            Con.Close();
        //        }
        //    }
        //    return Result1;
        //}


        //public string InsertQualificationDetail(Education URS, int UserPrimaryKey, int chkVerified)
        //{
        //    string Result1 = string.Empty;
        //    Con.Open();
        //    try
        //    {
        //        if (UserPrimaryKey > 0)
        //        {
        //            SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", Con);
        //            CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", "16");
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@Course", URS.Course);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@Degree", URS.Degree);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@MajorFieldOfStudy", URS.FOS);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@University", URS.University);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@LastYearPerc", URS.LastYearCGPA);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@AggregatePerc", URS.Aggregate);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@YearOfPassing", URS.Year);
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@UserPrimaryKey", Convert.ToString(UserPrimaryKey));
        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@chkVerified", chkVerified);


        //            CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
        //            Result1 = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (Con.State != ConnectionState.Closed)
        //        {
        //            Con.Close();
        //        }
        //    }
        //    return Result1;
        //}

        //public DataSet GetUserCV(string PK_UserID)
        //{
        //    DataSet DTEditUsers = new DataSet();
        //    try
        //    {
        //        SqlCommand CMDEditUsers = new SqlCommand("SP_GetUserCV_international", Con);
        //        CMDEditUsers.CommandType = CommandType.StoredProcedure;
        //        CMDEditUsers.Parameters.AddWithValue("@PK_UserID", PK_UserID);
        //        SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
        //        SDAEditUsers.Fill(DTEditUsers);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTEditUsers.Dispose();
        //    }
        //    return DTEditUsers;
        //}

    }
}