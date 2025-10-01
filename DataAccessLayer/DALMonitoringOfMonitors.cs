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
    public class DALMonitoringOfMonitors
    {
        MonitoringOfMonitors obj = new MonitoringOfMonitors();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        CultureInfo provider = CultureInfo.InvariantCulture;
        

        public DataTable GetMentoringDetailsQue() //User Role DashBoard
        {
            DataTable DTDashBoard = new DataTable();
            //DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("SP_Mentoring", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 14);
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

        public DataSet GetReportList(string Report_No)//Get All DropDownlist 
        {

            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Mentoring", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 15);
                CMDGetDdlLst.Parameters.AddWithValue("@UID", Report_No);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return DSGetddlList;
        }


        public string UpdateReport(MonitoringOfMonitors URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_MonitoringDetails", con);
                CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 11);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@UID", URS.UIN);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ID", URS.Id);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", URS.InspectorComment);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", URS.Reporting_manager_comments);
                //CMDInsertUpdateUsers.Parameters.AddWithValue("@ReferenceDoc", URS.Reference_Document);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Call_no", URS.Report_No);
                //CMDInsertUpdateUsers.Parameters.AddWithValue("@DetailsOfInspectionActivity", URS.Details_of_inspection_activity);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", URS.Inspector_Level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", URS.Scope);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", URS.Monitor_level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", URS.on_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", URS.off_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_CALL_ID", URS.PK_CALL_ID);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", URS.travel_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@vendor", URS.Vendor_Name);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemInspected", URS.Item_Inspected);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@VendorLocation", URS.Vendor_Location);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemDescription/", URS.itemDescription);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", URS.inspectorId); 
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TypeOfMonitoring", URS.Inspector_Name);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@MonitoringDate", URS.Date);

                CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", URS.CurrentAssignment);
                //added by shrutika salve 09052024
                CMDInsertUpdateUsers.Parameters.AddWithValue("@specialMonitoring", URS.specialMonitoring);


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


        public string UpdateReportQuestion(MonitoringOfMonitors URS)
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


        public string InsertDetails(MonitoringOfMonitors QM)
        {
            string Result1 = string.Empty;
            //Users obj = new Users();
            DataTable DTExistUserName = new DataTable();
            List<MonitoringOfMonitors> lstEnquiryDashB = new List<MonitoringOfMonitors>();
            con.Open();
            try
            {
                SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_Mentoring", con);
                CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 17);


                //CMDInsertUpdateUsers.Parameters.AddWithValue("@ID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@UIN", QM.UIN);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", QM.InspectorComment);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Call_no", QM.Report_No);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", QM.Reporting_manager_comments);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_CALL_ID", QM.PK_CALL_ID);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@VendorLocation", QM.Vendor_Location);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", QM.Inspector_Level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", QM.Monitor_level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", QM.Scope);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", QM.on_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", QM.off_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", QM.travel_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Vendor", QM.Vendor_Name);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemInspected", QM.Item_Inspected);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", QM.inspectorId);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemDescription", QM.itemDescription); 
                CMDInsertUpdateUsers.Parameters.AddWithValue("@MonitorNameMOMReport", QM.Inspector_Name);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@MonitoringDate", QM.Date);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", QM.CurrentAssignment);

                //added by shrutika salve 09052024
                CMDInsertUpdateUsers.Parameters.AddWithValue("@specialMonitoring", QM.specialMonitoring);


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

        public string AddQuestionsAnswers(MonitoringOfMonitors CM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Mentoring", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "18");
                cmd.Parameters.AddWithValue("@Qid", CM.Qid);
                cmd.Parameters.AddWithValue("@Answer", CM.Ans);
                cmd.Parameters.AddWithValue("@UID", CM.UIN);
                cmd.Parameters.AddWithValue("@Checkbox", CM.insertcheckbox);
                cmd.Parameters.AddWithValue("@FreeTextBox", CM.FreeText);
                //cmd.Parameters.AddWithValue("@TypeOfMonitoring", CM.TypeOfMonitoring);

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


        public DataSet EditReport(string UIN)
        {
            DataSet DTEditComplaints = new DataSet();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_Mentoring", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 19);
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
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 16);
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
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 14);
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
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 15);
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


        public string updateRatingAnswers(MonitoringOfMonitors CM)
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


        public string AddRatingAnswers(MonitoringOfMonitors CM)
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



        //added by nikita on 1602024

        public DataTable CheckData(string pkcallid)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_Check_OPE_SendForApproval", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 2);
                CMDGetDdlLst.Parameters.AddWithValue("@Pk_Call_ID", pkcallid);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return DSGetddlList;
        }


        //added by nikita on 06052024
        public DataTable CheckData_Mom(string pkcallid)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_Check_OPE_SendForApproval", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetDdlLst.Parameters.AddWithValue("@Pk_Call_ID", pkcallid);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return DSGetddlList;
        }


    }
}