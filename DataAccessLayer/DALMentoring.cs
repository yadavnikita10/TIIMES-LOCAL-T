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
    public class DALMentoring
    {
        Mentoring obj = new Mentoring();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public Mentoring GetDataByControllNo(string Call_No)
        {
            con.Open();
            Mentoring _vmcompany = new Mentoring();

            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_Mentoring", con);
                GetAddress.CommandType = CommandType.StoredProcedure;
                GetAddress.Parameters.AddWithValue("@SP_Type", "2");
                GetAddress.Parameters.AddWithValue("@Call_No", Call_No);

                SqlDataReader dr = GetAddress.ExecuteReader();
                while (dr.Read())
                {

                    _vmcompany.Date = Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy");
                    _vmcompany.Inspector_Name = dr["Inspector_Name"].ToString();
                    _vmcompany.TUVI_control_number = dr["TUV_Control_Number"].ToString();
                    _vmcompany.Customer_Name = dr["Customer_Name"].ToString();
                    _vmcompany.EndCustomerName = dr["End_Customer_Name"].ToString();
                    _vmcompany.ProjectName = dr["Project_Name"].ToString();
                    _vmcompany.Vendor_Name = dr["VendorName"].ToString();
                    _vmcompany.Vendor_Location = dr["Vendor_Location"].ToString();
                    _vmcompany.Item_Inspected = dr["Item_Inspected"].ToString();


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

            return _vmcompany;
        }


        public DataTable getcallNo()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Mentoring", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@SP_Type", 3);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }


        public DataTable GetMentoringDetailsQue() //User Role DashBoard
        {
            DataTable DTDashBoard = new DataTable();
            //DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("SP_Mentoring", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 4);
                //CMDCallDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }



        public string InsertDetailsMentoring(Mentoring QM)
        {
            string Result1 = string.Empty;
            //Users obj = new Users();
            DataTable DTExistUserName = new DataTable();
            List<Mentoring> lstEnquiryDashB = new List<Mentoring>();
            con.Open();
            try
            {
                SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_Mentoring", con);
                CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 5);


                //CMDInsertUpdateUsers.Parameters.AddWithValue("@ID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@UIN", QM.UIN);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", QM.InspectorComment);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Call_no", QM.Call_No);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", QM.Reporting_manager_comments);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ReferenceDoc", QM.Reference_Document);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@DetailsOfInspectionActivity", QM.Details_of_inspection_activity);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", QM.Inspector_Level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", QM.Monitor_level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", QM.Scope);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", QM.on_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", QM.off_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", QM.travel_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Vendor", QM.Vendor_Name);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemInspected", QM.Item_Inspected);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", QM.inspectorId);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TypeOfMonitoring", QM.TypeOfMonitoring);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@VendorLocation", QM.Vendor_Location); 
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemDescription", QM.itemDescription);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@MonitoringDate", QM.Date);
                //added by hsrutika salve 26042024
                CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", QM.CurrentAssignment);
                if (HttpContext.Current.Session["UserID"].ToString() == Convert.ToString(QM.inspectorId))
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommentName", Convert.ToString(System.Web.HttpContext.Current.Session["fullName"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommmentDate", DateTime.Now.ToString());
                }
                else
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommentName", QM.inspectorCommetName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommmentDate", QM.InspectorCommentDate);
                }
                if (HttpContext.Current.Session["Branch"].ToString() == "True")
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportiongCommentName", Convert.ToString(System.Web.HttpContext.Current.Session["fullName"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingCommentDate", DateTime.Now.ToString());
                }
                else
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportiongCommentName", QM.ManagerCommentName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingCommentDate", QM.ManagerCommentDate);
                }


                // Result1 = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDInsertUpdateUsers);
                Result1 = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();

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
            return Result1;
        }

        public string AddQuestionsAnswers(Mentoring CM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Mentoring", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "6");
                cmd.Parameters.AddWithValue("@Qid", CM.Qid);
                cmd.Parameters.AddWithValue("@Answer", CM.Ans);
                cmd.Parameters.AddWithValue("@UID", CM.UIN);
                cmd.Parameters.AddWithValue("@Checkbox", CM.insertcheckbox);
                cmd.Parameters.AddWithValue("@FreeTextBox", CM.FreeText);
                cmd.Parameters.AddWithValue("@TypeOfMonitoring", CM.TypeOfMonitoring);

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





        public string UpdateReport(Mentoring URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_MonitoringDetails", con);
                CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 11);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@UID", URS.UIN);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Id", URS.Id);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", URS.InspectorComment);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", URS.Reporting_manager_comments);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ReferenceDoc", URS.Reference_Document);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@DetailsOfInspectionActivity", URS.Details_of_inspection_activity);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", URS.Inspector_Level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", URS.Scope);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", URS.Monitor_level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", URS.on_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", URS.off_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@VendorLocation", URS.Vendor_Location);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", URS.travel_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@vendor", URS.Vendor_Name);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemInspected", URS.Item_Inspected);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Call_No", URS.Call_No);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TypeOfMonitoring", URS.TypeOfMonitoring);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", URS.inspectorId);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemDescription", URS.itemDescription);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@MonitoringDate", URS.Date);

                //added by shrutika salve 26042024
                CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", URS.CurrentAssignment);

                CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                //added by shrutika salve 08-06-2023
                if (HttpContext.Current.Session["UserID"].ToString() == Convert.ToString(URS.inspectorId))
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommentName", Convert.ToString(System.Web.HttpContext.Current.Session["fullName"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommmentDate", DateTime.Now.ToString());
                }
                else
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommentName", URS.inspectorCommetName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommmentDate", URS.InspectorCommentDate);
                }
                if (HttpContext.Current.Session["Branch"].ToString() == "True")
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportiongCommentName", Convert.ToString(System.Web.HttpContext.Current.Session["fullName"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingCommentDate", DateTime.Now.ToString());
                }
                else
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportiongCommentName", URS.ManagerCommentName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingCommentDate", URS.ManagerCommentDate);
                }

                Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();



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


        public string UpdateReportQuestion(Mentoring URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_MonitoringDetails", con);
                CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 12);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@UID", URS.UIN);

                CMDInsertUpdateUsers.Parameters.AddWithValue("@Qid", URS.Qid);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Answer", URS.Ans);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Checkbox", URS.insertcheckbox);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@FreeTextBox", URS.FreeText);

                Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();



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



        //Edit Data Mentoring Report 19042023//

        public DataSet EditReport(string UIN)
        {
            DataSet DTEditComplaints = new DataSet();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_Mentoring", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 7);
                CMDEditComplaints.Parameters.AddWithValue("@UID", UIN);
                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditComplaints);
                SDAEditUsers.Fill(DTEditComplaints);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditComplaints.Dispose();
            }
            return DTEditComplaints;
        }


        public DataTable EditReportQUestion(string UIN)
        {
            DataTable DTEditComplaints = new DataTable();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_MonitoringDetails", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 35);
                CMDEditComplaints.Parameters.AddWithValue("@uinid", UIN);
                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditComplaints);
                SDAEditUsers.Fill(DTEditComplaints);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditComplaints.Dispose();
            }
            return DTEditComplaints;
        }

        //Mentoring Of Monitoring report Sp start 20-04-2022

        public DataTable getreportNo()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MonitoringDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@SP_Type", 14);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }


        public MonitoringOfMonitors GetDataByControll(string Report_no)
        {
            con.Open();
            MonitoringOfMonitors _vmcompany = new MonitoringOfMonitors();
            List<string> Selected = new List<string>();
            string[] splitedProduct_Name;
            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_MonitoringDetails", con);
                GetAddress.CommandType = CommandType.StoredProcedure;
                GetAddress.Parameters.AddWithValue("@SP_Type", "13");
                GetAddress.Parameters.AddWithValue("@UID", Report_no);

                SqlDataReader dr = GetAddress.ExecuteReader();
                while (dr.Read())
                {

                    _vmcompany.Date = Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy");
                    _vmcompany.Inspector_Name = dr["Inspector_Name"].ToString();
                    _vmcompany.TUVI_control_number = dr["TUV_Control_Number"].ToString();
                    _vmcompany.Customer_Name = dr["Customer_Name"].ToString();
                    _vmcompany.EndCustomerName = dr["End_Customer_Name"].ToString();
                    _vmcompany.ProjectName = dr["Project_Name"].ToString();
                    _vmcompany.Vendor_Name = dr["VendorName"].ToString();
                    _vmcompany.Vendor_Location = dr["Vendor_Location"].ToString();
                    _vmcompany.Item_Inspected = dr["Item_Inspected"].ToString();

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

            return _vmcompany;
        }

        //added by shrutika salve 07092023


        public DataTable EditUploadedFile(string ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_MonitoringRecordattachement", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@UID", ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_MonitoringRecordattachement", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable GetConFileExt(string FileID)
        {
            DataTable DTGetFileExtenstion = new DataTable();
            try
            {
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_MonitoringRecordattachement", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetExtenstion.Parameters.AddWithValue("@FileID", FileID);
                CMDGetExtenstion.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetExtenstion = new SqlDataAdapter(CMDGetExtenstion);
                SDAGetExtenstion.Fill(DTGetFileExtenstion);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetFileExtenstion.Dispose();
            }
            return DTGetFileExtenstion;
        }

        public string DeleteConUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_MonitoringRecordattachement", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@UID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDDeleteUploadedFile.ExecuteNonQuery().ToString();
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


        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, string ID, int? PK_Call_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("UID", typeof(string)));
                //DTUploadFile.Columns.Add(new DataColumn("EnquiryNumber", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                // DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("PK_call_id", typeof(int)));
                foreach (var item in lstFileUploaded)
                {
                    if (PK_Call_ID == 0)
                    {
                        item.FileName = Convert.ToString(ID) + '_' + item.FileName;
                    }
                    else
                    {
                        item.FileName = Convert.ToString(PK_Call_ID) + '_' + item.FileName;
                    }

                    DTUploadFile.Rows.Add(ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, PK_Call_ID);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_MonitoringRecordattachement", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@MonitoringRecord", DTUploadFile);
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

        //Edit Data Mentoring Report 19042023//

        public DataSet EditReportavgvalue(string UIN)
        {
            DataSet DTEditComplaints = new DataSet();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_MonitoringRating", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 8);
                CMDEditComplaints.Parameters.AddWithValue("@UID", UIN);
                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditComplaints);
                SDAEditUsers.Fill(DTEditComplaints);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditComplaints.Dispose();
            }
            return DTEditComplaints;
        }
        //added by shrutika salve 03102023


        public DataTable GetDetailsQuestionRate() //User Role DashBoard
        {
            DataTable DTDashBoard = new DataTable();
            //DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("SP_MonitoringRating", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 10);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }


        public DataTable EditRatingQuetion(string UIN)
        {
            DataTable DTEditComplaints = new DataTable();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_MonitoringRating", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 11);
                CMDEditComplaints.Parameters.AddWithValue("@UID", UIN);
                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditComplaints);
                SDAEditUsers.Fill(DTEditComplaints);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditComplaints.Dispose();
            }
            return DTEditComplaints;
        }


        public string updateRatingAnswers(Mentoring CM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MonitoringRating", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "17");
                cmd.Parameters.AddWithValue("@ID", CM.Qidrate);
                cmd.Parameters.AddWithValue("@UID", CM.UIN);
                cmd.Parameters.AddWithValue("@Ans", CM.rating);
                cmd.Parameters.AddWithValue("@Userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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


        public string AddRatingAnswers(Mentoring CM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MonitoringRating", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "5");
                cmd.Parameters.AddWithValue("@ID", CM.Qidrate);
                cmd.Parameters.AddWithValue("@UID", CM.UIN);
                cmd.Parameters.AddWithValue("@Ans", CM.rating);
                cmd.Parameters.AddWithValue("@Userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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