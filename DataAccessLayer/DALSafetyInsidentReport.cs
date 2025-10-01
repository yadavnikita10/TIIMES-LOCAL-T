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
   
    public class DALSafetyInsidentReport
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);


        public string Insert(SafetyInsidentReport SR)
        {
            
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {
                if (SR.PKId == 0)
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_SafetyInsidentReport", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 1);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Branch", SR.Branch);
                    if (SR.DateofReport != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@DateofReport", DateTime.ParseExact(SR.DateofReport, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    }
                    else
                    {

                    }
                    if (SR.DateOfIncident != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@DateOfIncident", DateTime.ParseExact(SR.DateOfIncident, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    }
                    else
                    {

                    }
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@TypeOfIncident", SR.TypeOfIncident);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@NameOfInjuredPerson", SR.NameOfInjuredPerson);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@IPHomeAddress", SR.IPHomeAddress);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@LocationofIncident", SR.LocationofIncident);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@TypeOfInjury", SR.TypeOfInjury);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@MedicalTreatmentDetails", SR.MedicalTreatmentDetails);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DescriptionOfIncident", SR.DescriptionOfIncident);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@RootCauseAnalysis", SR.RootCauseAnalysis);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Correction", SR.Correction);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CorrectiveAction", SR.CorrectiveAction);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@MandaysLost", SR.MandaysLost);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@RiskAndOpportunities", SR.RiskAndOpportunities);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@AIHIRAReviewed", SR.AIHIRAReviewed);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@ShareLessonsLearnt", SR.ShareLessonsLearnt);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Status", SR.Status);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@UniqueNumber", SR.TiimesUIN);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@LessonsLearned", SR.LessonsLearned);

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@FormFilledBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdateJOB.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;

                    Result = CMDInsertUpdateJOB.ExecuteNonQuery().ToString();
                    ReturnId = Convert.ToInt32(CMDInsertUpdateJOB.Parameters["@ReturnId"].Value.ToString());
                    Result = Convert.ToString(ReturnId);

                    //CMDInsertUpdateJOB.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    //Result = CMDInsertUpdateJOB.ExecuteNonQuery().ToString();
                    //ReturnId = Convert.ToInt32(CMDInsertUpdateJOB.Parameters["@ReturnId"].Value.ToString());
                    //Result = Convert.ToString(ReturnId);



                }
                else
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_SafetyInsidentReport", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Branch", SR.Branch);
                    if(SR.DateofReport!=null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@DateofReport", DateTime.ParseExact(SR.DateofReport, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    }
                    else
                    {

                    }
                    if (SR.DateOfIncident != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@DateOfIncident", DateTime.ParseExact(SR.DateOfIncident, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    }
                    else
                    {

                    }
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@TypeOfIncident", SR.TypeOfIncident);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@NameOfInjuredPerson", SR.NameOfInjuredPerson);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@IPHomeAddress", SR.IPHomeAddress);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@LocationofIncident", SR.LocationofIncident);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@TypeOfInjury", SR.TypeOfInjury);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@MedicalTreatmentDetails", SR.MedicalTreatmentDetails);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DescriptionOfIncident", SR.DescriptionOfIncident);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@RootCauseAnalysis", SR.RootCauseAnalysis);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Correction", SR.Correction);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CorrectiveAction", SR.CorrectiveAction);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@MandaysLost", SR.MandaysLost);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@RiskAndOpportunities", SR.RiskAndOpportunities);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@AIHIRAReviewed", SR.AIHIRAReviewed);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@ShareLessonsLearnt", SR.ShareLessonsLearnt);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Status", SR.Status);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PKId", SR.PKId);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@FormFilledBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@LessonsLearned", SR.LessonsLearned);

                    Result = CMDInsertUpdateJOB.ExecuteNonQuery().ToString();

                   

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

        public DataSet GetOnPageLoad()
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_SafetyInsidentReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 3);
                //CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }


        public DataSet GetData()
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_SafetyInsidentReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 4);
                //CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                //CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        public DataSet dsGetDataById(int? PKId)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_SafetyInsidentReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 6);
                CMDEditContact.Parameters.AddWithValue("@PKId", PKId);
                //CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        public string Delete(int id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_SafetyInsidentReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@PKId", id);
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


        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int JOB_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("SP_SafetyInsidentReportUploadedFile", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("Type", typeof(string)));
                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(JOB_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent,"IP");
                }


                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_SafetyInsidentReportUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@SafetyInsidentReportUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
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

        public string InsertLessonsLearntPhotographs(List<FileDetails> lstFileUploaded, int JOB_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("SP_SafetyInsidentReportUploadedFile", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("Type", typeof(string)));
                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(JOB_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent, "LSP");
                }


                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_SafetyInsidentReportUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@SafetyInsidentReportUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
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

        public string InsertOtherAttachment(List<FileDetails> lstFileUploaded, int JOB_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("SP_SafetyInsidentReportUploadedFile", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("Type", typeof(string)));
                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(JOB_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent, "OA");
                }


                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_SafetyInsidentReportUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@SafetyInsidentReportUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
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

        public DataTable EditUploadedFile(int? PK_JOB_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SafetyInsidentReportUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_SafetyInsidentReportId", PK_JOB_ID);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }

        public DataTable EditLearntPhotographs(int? PK_JOB_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SafetyInsidentReportUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_SafetyInsidentReportId", PK_JOB_ID);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }

        public DataTable OtherAtt(int? PK_JOB_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SafetyInsidentReportUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 4);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_SafetyInsidentReportId", PK_JOB_ID);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }

        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SafetyInsidentReportUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", EQ_ID);
                //CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }


        public List<Users> GetInspectorList1()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<Users> lstEnquiryDashB = new List<Users>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "64");
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new Users
                           {
                               PK_UserID = Convert.ToString(dr["PK_UserID"]),
                               FirstName = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),

                           }
                         );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }



    }
}