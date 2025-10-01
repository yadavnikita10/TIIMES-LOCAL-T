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
    public class OnSiteMonitoring
    {
        monitors obj = new monitors();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


        public monitors GetDataByControllNo(string Call_No)
        {
            con.Open();
            monitors _vmcompany = new monitors();
            List<string> Selected = new List<string>();
            string[] splitedProduct_Name;
            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_MonitoringDetails", con);
                GetAddress.CommandType = CommandType.StoredProcedure;
                GetAddress.Parameters.AddWithValue("@SP_Type", "2");
                GetAddress.Parameters.AddWithValue("@Call_No", Call_No);

                SqlDataReader dr = GetAddress.ExecuteReader();
                while (dr.Read())
                {

                    _vmcompany.Date = Convert.ToDateTime(dr["Date"]).ToString("dd/MM/yyyy");
                    _vmcompany.Inspector_Name = dr["Inspector_Name"].ToString();
                    _vmcompany.TUVI_control_number = dr["TUV_Control_Number"].ToString();
                    _vmcompany.Customer_Name = dr["Customer_Name"].ToString();
                    _vmcompany.EndCustomerName = dr["End_Customer_Name"].ToString();
                    _vmcompany.ProjectName = dr["Project_Name"].ToString();
                    _vmcompany.Vendor_Name = dr["VendorName"].ToString();
                    _vmcompany.Vendor_Location = dr["Vendor_Location"].ToString();
                    _vmcompany.Po_No = dr["Vendor_Po_No"].ToString();
                    _vmcompany.Item_Inspected = dr["Item_Inspected"].ToString();
                    _vmcompany.SubSubVendorDate_of_PO = dr["SubVendorPoDate"].ToString();
                    _vmcompany.Date_of_PO = dr["TopvendorPODate"].ToString();
                    _vmcompany.DEC_PMC_EPC_Name = dr["DECName"].ToString();
                    _vmcompany.DEC_PMC_EPC_Assignment_No = dr["DECNumber"].ToString();
                    _vmcompany.Sub_Vendor_Name = dr["SubVendorName"].ToString();
                    _vmcompany.Po_No_SubVendor = dr["SubVendorPONo"].ToString();
                    _vmcompany.inspectorId = dr["inspectorId"].ToString();
                    //added by shrutika salve 23042024
                    _vmcompany.chkMan = Convert.ToBoolean(dr["ManMonthsAssignment"]);


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

        public DataTable getcontrolNodata()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MonitoringDetails", con);
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



        public DataTable GetQId()
        {
            DataTable DTFeedBack = new DataTable();
            try
            {

                SqlCommand CMDFeedBack = new SqlCommand("SP_MonitoringDetails", con);
                CMDFeedBack.CommandType = CommandType.StoredProcedure;
                CMDFeedBack.Parameters.AddWithValue("@SP_Type", 5);
                //CMDFeedBack.Parameters.AddWithValue("@QId", Qid);
                //CMDFeedBack.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDFeedBack.CommandTimeout = 100000;
                SqlDataAdapter Ad = new SqlDataAdapter(CMDFeedBack);
                Ad.Fill(DTFeedBack);

            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                DTFeedBack.Dispose();
            }
            return DTFeedBack;
        }


        public string InsertDetails(monitors CM)
        {
            string Result1 = string.Empty;
            //Users obj = new Users();
            DataTable DTExistUserName = new DataTable();
            List<monitors> lstEnquiryDashB = new List<monitors>();
            con.Open();
            try
            {
                SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_MonitoringDetails", con);
                CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 4);


                //CMDInsertUpdateUsers.Parameters.AddWithValue("@ID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@UIN", CM.UIN);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", CM.InspectorComment);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Call_no", CM.Call_No);
               
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", CM.Reporting_manager_comments);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ReferenceDoc", CM.Reference_Document);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@DetailsOfInspectionActivity", CM.Details_of_inspection_activity);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", CM.Inspector_Level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", CM.Monitor_level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", CM.Scope);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", CM.on_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", CM.off_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", CM.travel_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Vendor", CM.Vendor_Name);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemInspected", CM.Item_Inspected);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", CM.inspectorId);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TypeOfMonitoring", CM.TypeOfmonitoring);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@VendorLocation", CM.Vendor_Location); 
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemDescription", CM.itemDescription);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@MonitoringDate", CM.Date);

                CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", CM.CurrentAssignment);

                //added by shrutika salve 26062024
                CMDInsertUpdateUsers.Parameters.AddWithValue("@specialMonitoring", CM.specialMonitoring);

                //added By shrutika salve 08-06-2023
                if (HttpContext.Current.Session["UserID"].ToString() == CM.inspectorId)
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommentName", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommmentDate", DateTime.Now.ToString());
                }
                else
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommentName", CM.inspectorCommetName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommmentDate", CM.InspectorCommentDate);
                }
                if (HttpContext.Current.Session["Branch"] == HttpContext.Current.Session["UserID"])
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportiongCommentName", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingCommentDate", DateTime.Now.ToString());
                }
                else
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportiongCommentName", CM.ManagerCommentName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingCommentDate", CM.ManagerCommentDate);
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



        //public DataTable InsertQuestion(monitors CM)
        //{
        //    string Result1 = string.Empty;
        //    //Users obj = new Users();
        //    DataTable DTExistUserName = new DataTable();
        //    List<monitors> lstEnquiryDashB = new List<monitors>();
        //    con.Open();
        //    try
        //    {
        //        SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_MonitoringDetails", con);
        //        CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 5);
        //        // Result1 = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
        //        SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDInsertUpdateUsers);
        //        SDAScripName.Fill(DTExistUserName);

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
        //    return DTExistUserName;
        //}



        public string InsertQuestionAns(List<monitors> lstCallDashBoard1, int QuestionNo)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            //string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("Qid", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("Ans", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("UINId", typeof(string)));

                foreach (var item in lstCallDashBoard1)
                {
                    item.Qid = Convert.ToString(QuestionNo) + '_' + item.QuestionNo;
                    DTUploadFile.Rows.Add(QuestionNo, item.Ans, null, DateTime.Now, null, null, item.UIN);
                }

                if (lstCallDashBoard1.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_MonitoringDetails", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListQuestion", DTUploadFile);
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






        public DataTable GetQid(monitors Qid)//Getting Data of Qutation Details
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_MonitoringDetails", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@Qid ", Qid);
                // CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiryDtls = new SqlDataAdapter(CMDGetEnquiryDtls);
                SDAGetEnquiryDtls.Fill(DTGetEnquiryDtls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetEnquiryDtls.Dispose();
            }
            return DTGetEnquiryDtls;

        }

        public DataTable GetDetails() //User Role DashBoard
        {
            DataTable DTDashBoard = new DataTable();
            //DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("SP_MonitoringDetails", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 6);
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


        public string AddQuestionsAnswers(monitors CM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MonitoringDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "5");
                cmd.Parameters.AddWithValue("@Qid", CM.Qid);
                cmd.Parameters.AddWithValue("@Answer", CM.Ans);
                cmd.Parameters.AddWithValue("@UID", CM.UIN);
                cmd.Parameters.AddWithValue("@Checkbox", CM.insertcheckbox);
                cmd.Parameters.AddWithValue("@FreeTextBox", CM.FreeText); 
                 cmd.Parameters.AddWithValue("@check", CM.Check1);
                

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

        public DataTable GetReportsDashBoard(string UserID) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDReportDash = new SqlCommand("SP_MonitoringDetails", con);
                CMDReportDash.CommandType = CommandType.StoredProcedure;
                CMDReportDash.CommandTimeout = 100000;
                CMDReportDash.Parameters.AddWithValue("@SP_Type","13");
                CMDReportDash.Parameters.AddWithValue("@UserID",UserID);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDReportDash);
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

        public DataTable GetMentorList(string UserID) 
        {

            DataTable DTMentorList = new DataTable();
            try
            {
                SqlCommand CMDMentorList = new SqlCommand("SP_GetMentorList", con);
                CMDMentorList.CommandType = CommandType.StoredProcedure;
                CMDMentorList.CommandTimeout = 100000;
                CMDMentorList.Parameters.AddWithValue("@MentorID", UserID);
                SqlDataAdapter SDAMentor = new SqlDataAdapter(CMDMentorList);
                SDAMentor.Fill(DTMentorList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTMentorList.Dispose();
            }

            return DTMentorList;
        }



        public DataSet EditReport(string UIN)
        {
            DataSet DTEditComplaints = new DataSet();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_MonitoringDetails", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 24);
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
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 10);
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

        public bool DeleteData(string UID)
        {
            int i = 0;
            SqlCommand CMDDeleteRoll = new SqlCommand("SP_Mentoring", con);
            CMDDeleteRoll.CommandType = CommandType.StoredProcedure;
            CMDDeleteRoll.Parameters.AddWithValue("@SP_Type", 10);
            CMDDeleteRoll.Parameters.AddWithValue("@UID", UID);
            CMDDeleteRoll.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
            con.Open();
            i = CMDDeleteRoll.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public DataTable getlistforEdit()
        {
            DataTable dt = new DataTable();
            SqlCommand CMDGetEdit = new SqlCommand("SP_MonitoringDetails", con);
            CMDGetEdit.CommandType = CommandType.StoredProcedure;
            CMDGetEdit.CommandTimeout = 1000000;
            CMDGetEdit.Parameters.AddWithValue("@SP_Type", 7);

            SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEdit);
            SDAGetEnquiry.Fill(dt);
            return dt;
        }


        public DataSet GetDdlLst()
        {
            DataSet DTEMDashBoard = new DataSet();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_MonitoringDetails", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 8);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }

        public List<Calls> GetDrpList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<Calls> lstEnquiryDashB = new List<Calls>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_MonitoringDetails", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 7);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new Calls
                           {
                               Product_ID = Convert.ToInt32(dr["Product_ID"]),
                               Product_Name = Convert.ToString(dr["Name"]),
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

        public string UpdateReport(monitors URS)
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

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", URS.travel_time);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@vendor", URS.Vendor_Name);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@itemInspected", URS.Item_Inspected);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Call_No", URS.Call_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TypeOfMonitoring", URS.TypeOfmonitoring);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", URS.inspectorId);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@VendorLocation", URS.Vendor_Location); 
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@itemDescription", URS.itemDescription);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MonitoringDate", URS.Date);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", URS.CurrentAssignment);

                //added by shrutika salve 09052023
                CMDInsertUpdateUsers.Parameters.AddWithValue("@specialMonitoring", URS.specialMonitoring);

                CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                //added by shrutika salve 08-06-2023
                if (HttpContext.Current.Session["UserID"].ToString() == Convert.ToString(URS.inspectorId))
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommentName", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommmentDate", DateTime.Now.ToString());
                }
                else
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommentName", URS.inspectorCommetName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommmentDate", URS.InspectorCommentDate);
                }
                if (HttpContext.Current.Session["Branch"].ToString() == "True")
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportiongCommentName", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingCommentDate", DateTime.Now.ToString());
                }
                else
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportiongCommentName", URS.inspectorCommetName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingCommentDate", URS.InspectorCommentDate);
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


        public string UpdateReportQuestion(monitors URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_MonitoringDetails", con);
                CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 12);
                //CMDInsertUpdateUsers.Parameters.AddWithValue("@UID", URS.UINId);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@UID", URS.UIN);

                CMDInsertUpdateUsers.Parameters.AddWithValue("@Qid", URS.Qid);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Answer", URS.Ans);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Checkbox", URS.insertcheckbox);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@FreeTextBox", URS.FreeText);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@check", URS.Check1);
                
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

        public string InsertUpdate(monitors CM,string UIN)
        {
            string Result = string.Empty;
            con.Open();
            if (UIN != null )
                {
                try
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_MonitoringDetails", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 4);


                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@ID", System.Web.HttpContext.Current.Session["UserIDs"]);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@UID", CM.UIN);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", CM.InspectorComment);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Call_no", CM.Call_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", CM.Reporting_manager_comments);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReferenceDoc", CM.Reference_Document);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@DetailsOfInspectionActivity", CM.Details_of_inspection_activity);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", CM.Inspector_Level_of_authorisation);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", CM.Monitor_level_of_authorisation);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", CM.Scope);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", CM.on_site_time);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", CM.off_site_time);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", CM.travel_time);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Vendor", CM.Vendor_Name);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@itemInspected", CM.Item_Inspected);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@VendorLocation", CM.Vendor_Location);

                    Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();

                }
                catch (Exception Ex)
                {
                    string Error = Ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                try
                {
                    
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_Mentoring", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 13);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@UID", CM.UIN);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", CM.InspectorComment);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", CM.Reporting_manager_comments);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReferenceDoc", CM.Reference_Document);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@DetailsOfInspectionActivity", CM.Details_of_inspection_activity);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", CM.Inspector_Level_of_authorisation);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", CM.Scope);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", CM.Monitor_level_of_authorisation);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", CM.on_site_time);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", CM.off_site_time);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", CM.travel_time);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@vendor", CM.Vendor_Name);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@itemInspected", CM.Item_Inspected);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Call_No", CM.Call_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TypeOfMonitoring", CM.TypeOfmonitoring);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", CM.Inspector_Name);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@VendorLocation", CM.Vendor_Location);


                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
                }
                catch (Exception Ex)
                {
                    string Error = Ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            return Result;
        }

        public string InsertUpdateQuestion(monitors CM,string UID)
        {
            string Result = string.Empty;
            con.Open();
            if (UID != null)
            {
                try
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_Mentoring", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 12);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@UID", CM.UINId);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Qid", CM.QuestionNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Answer", CM.Ans);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Checkbox", CM.insertcheckbox);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FreeTextBox", CM.freetextans);

                    Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();

                }
                catch (Exception Ex)
                {
                    string Error = Ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                try
                {
                    
                    SqlCommand cmd = new SqlCommand("SP_MonitoringDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", "5");
                    cmd.Parameters.AddWithValue("@Qid", CM.Qid);
                    cmd.Parameters.AddWithValue("@Answer", CM.Ans);
                    cmd.Parameters.AddWithValue("@UID", CM.UINId);
                    cmd.Parameters.AddWithValue("@Checkbox", CM.insertcheckbox);
                    cmd.Parameters.AddWithValue("@FreeTextBox", CM.FreeText);
                    Result = cmd.ExecuteNonQuery().ToString();

                }
                catch (Exception Ex)
                {
                    string Error = Ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            return Result;
        }

        public monitors GetDataByControll(string Report_no)
        {
            con.Open();
            monitors _vmcompany = new monitors();
            List<string> Selected = new List<string>();
            string[] splitedProduct_Name;
            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_MonitoringDetails", con);
                GetAddress.CommandType = CommandType.StoredProcedure;
                GetAddress.Parameters.AddWithValue("@SP_Type", "2");
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

       

        //public List<monitors> GetBranchManger()// Binding Sales Masters DashBoard of Master Page 
        //{
        //    DataTable DTEMDashBoard = new DataTable();
        //    List<monitors> lstEnquiryDashB = new List<monitors>();
        //    try
        //    {
        //        SqlCommand CMDGetEnquriy = new SqlCommand("SP_MonitoringDetails", con);
        //        CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
        //        CMDGetEnquriy.CommandTimeout = 1000000;
        //        CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 32);

        //        SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
        //        SDAGetEnquiry.Fill(DTEMDashBoard);
        //        if (DTEMDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in DTEMDashBoard.Rows)
        //            {
        //                lstEnquiryDashB.Add(
        //                   new monitors
        //                   {
        //                       BranchManager = Convert.ToString(dr["BranchManager"]),
        //                   }
        //                 );
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTEMDashBoard.Dispose();
        //    }
        //    return lstEnquiryDashB;
        //}

      
        public DataTable GetBranchManger(string UserID)
        {

            DataTable DTMentorList = new DataTable();
            try
            {
                SqlCommand CMDMentorList = new SqlCommand("SP_MonitoringDetails", con);
                CMDMentorList.CommandType = CommandType.StoredProcedure;
                CMDMentorList.CommandTimeout = 100000;
                CMDMentorList.Parameters.AddWithValue("@SP_Type", 32);
                CMDMentorList.Parameters.AddWithValue("@UserId", UserID);
                SqlDataAdapter SDAMentor = new SqlDataAdapter(CMDMentorList);
                SDAMentor.Fill(DTMentorList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTMentorList.Dispose();
            }

            return DTMentorList;
        }

        public DataTable GetDataByDate(monitors CM)
        {
            DataTable ds = new DataTable();
            try
            {


                SqlCommand cmd = new SqlCommand("SP_Mentoring", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@SP_Type", "9");
                cmd.Parameters.AddWithValue("@UserID", CM.UserId);
                cmd.Parameters.AddWithValue("@DateF", CM.FromDate);
                cmd.Parameters.AddWithValue("@DateTo", CM.ToDate);
                //cmd.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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


        

        public DataTable UIDNoCheck(string UID)
        {

            DataTable UIDNO = new DataTable();
            try
            {
                SqlCommand CMD = new SqlCommand("SP_MonitoringDetails", con);
                CMD.CommandType = CommandType.StoredProcedure;
                CMD.CommandTimeout = 100000;
                CMD.Parameters.AddWithValue("@SP_Type", 33);
                CMD.Parameters.AddWithValue("@UID", UID);
                SqlDataAdapter SDAMentor = new SqlDataAdapter(CMD);
                SDAMentor.Fill(UIDNO);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                UIDNO.Dispose();
            }

            return UIDNO;
        }

        
        public DataTable CheckPreviousActivity(string date)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_MonitoringDetails", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "37");
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                CMDGetDdlLst.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(date, "dd/MM/yyyy", null));
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




        public DataTable CheckPreviousActivityWithCallId(string date, string monitor)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_MonitoringDetails", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "36");
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                CMDGetDdlLst.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(date, "dd/MM/yyyy", null));
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

        public DataTable CheckIfLeavePresent(string date)
        {
            DataTable ValidateTotal = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MonitoringDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "38");
                cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(date, "dd/MM/yyyy", null));
                //cmd.Parameters.AddWithValue("@CreatedBy", objL.FirstName);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ValidateTotal);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ValidateTotal.Dispose();
            }
            return ValidateTotal;
        }


        public DataSet Checkbox(String UserId1)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Mentor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 7);
                cmd.Parameters.AddWithValue("@UserID", UserId1);
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

        //added bu shrutika salve  28092023

        public string AddRatingAnswers(monitors CM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MonitoringRating", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");
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

        //Added  by shrutika salve 28092023
        public DataTable GetDetailsQuestionRate() //User Role DashBoard
        {
            DataTable DTDashBoard = new DataTable();
            //DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("SP_MonitoringRating", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 2);
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
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 3);
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


        public string updateRatingAnswers(monitors CM)
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

        public DataSet Monitoringid()//Checking User Login
        {
            DataSet DTLoginDtl = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MonitoringDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "39");
                //cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataAdapter SDALoginDtl = new SqlDataAdapter(cmd);
                SDALoginDtl.Fill(DTLoginDtl);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTLoginDtl.Dispose();
            }
            return DTLoginDtl;
        }




    }
}