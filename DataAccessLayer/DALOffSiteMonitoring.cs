using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using TuvVision.Models;
using System.Configuration;
using System.Web.Mvc;

namespace TuvVision.DataAccessLayer
{
    public class DALOffSiteMonitoring
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


        public List<ModelOffSiteMonitoring> GetScopeList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ModelOffSiteMonitoring> lstEnquiryDashB = new List<ModelOffSiteMonitoring>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_MonitoringDetails", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 1);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new ModelOffSiteMonitoring
                           {
                               PK_IAFScopeId = Convert.ToInt32(dr["PK_IAFScopeId"]),
                               IAFScopeName = Convert.ToString(dr["IAFScopeName"]),
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

        public List<ModelOffSiteMonitoring> GetAllReportList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ModelOffSiteMonitoring> lstEnquiryDashB = new List<ModelOffSiteMonitoring>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_MonitoringDetails", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 17);
                // CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new ModelOffSiteMonitoring
                           {
                               ReportName = Convert.ToString(dr["ReportName"]),
                               PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),

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

        public DataTable GetDetails() //User Role DashBoard
        {
            DataTable DTDashBoard = new DataTable();
            //DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("SP_MonitoringDetails", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 18);
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


        public string AddQuestionsAnswers(ModelOffSiteMonitoring CM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MonitoringDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "29");
                cmd.Parameters.AddWithValue("@Qid", CM.Qid);
                cmd.Parameters.AddWithValue("@Answer", CM.Ans);
                cmd.Parameters.AddWithValue("@UID", CM.UIN);
                cmd.Parameters.AddWithValue("@Checkbox", CM.insertcheckbox);
                cmd.Parameters.AddWithValue("@FreeTextBox", CM.FreeText1);
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

        public string UpdateReport(ModelOffSiteMonitoring URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_MonitoringDetails", con);
                CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 19);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@UID", URS.UIN);

                CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", URS.InspectorComment);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", URS.Reporting_manager_comments);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ReferenceDoc", URS.Reference_Document);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@MonitoringDate", URS.Date);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", URS.InspectrorLevelofauthorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", URS.Scope);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", URS.Monitorlevelofauthorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", URS.on_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", URS.off_site_time);

                CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", URS.travel_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@vendor", URS.VendorName);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemInspected", URS.Item_Inspected);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Call_No", URS.TUVIcontrolnumber);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@VendorLocation", URS.VendorLocation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", URS.inspectorId); 
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemDescription", URS.itemDescription);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", URS.CurrentAssignment);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Id", URS.id);


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


        public string UpdateReportQuestion(ModelOffSiteMonitoring URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_MonitoringDetails", con);
                CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 20);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@UID", URS.UIN);

                CMDInsertUpdateUsers.Parameters.AddWithValue("@Qid", URS.Qid);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Answer", URS.Ans);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Checkbox", URS.insertcheckbox);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@FreeTextBox", URS.FreeText1);

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

        public string InsertDetails(ModelOffSiteMonitoring CM)
        {
            string Result1 = string.Empty;
            //Users obj = new Users();
            DataTable DTExistUserName = new DataTable();
            List<ModelOffSiteMonitoring> lstEnquiryDashB = new List<ModelOffSiteMonitoring>();
            con.Open();
            try
            {
                SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_MonitoringDetails", con);
                CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 30);


                //CMDInsertUpdateUsers.Parameters.AddWithValue("@ID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@UIN", CM.UIN);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", CM.InspectorComment);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Call_no", CM.TUVIcontrolnumber);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", CM.Reporting_manager_comments);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ReferenceDoc", CM.Reference_Document);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@VendorLocation", CM.VendorLocation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", CM.InspectrorLevelofauthorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", CM.Monitorlevelofauthorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", CM.Scope);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", CM.on_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", CM.off_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", CM.travel_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Vendor", CM.VendorName);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemInspected", CM.Item_Inspected);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", CM.inspectorId);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_CALL_ID", CM.PK_CALL_ID);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Type", CM.Type); 
                CMDInsertUpdateUsers.Parameters.AddWithValue("@itemDescription", CM.itemDescription);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@MonitoringDate", CM.Date);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", CM.CurrentAssignment);

                if (HttpContext.Current.Session["UserID"].ToString() == Convert.ToString(CM.inspectorId))
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommentName", Convert.ToString(System.Web.HttpContext.Current.Session["fullName"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommmentDate", DateTime.Now.ToString());
                }
                else
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommentName", CM.inspectorCommetName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorCommmentDate", CM.InspectorCommentDate);
                }
                if (HttpContext.Current.Session["Branch"].ToString() == "True")
                {
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportiongCommentName", Convert.ToString(System.Web.HttpContext.Current.Session["fullName"]));
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

        public DataTable EditReportQUestion(string UIN)
        {
            DataTable DTEditComplaints = new DataTable();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_MonitoringDetails", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 23);
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


        public List<ModelOffSiteMonitoring> StringGetReportListForIRNNew(string Sub_JobNo, String Type)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ModelOffSiteMonitoring> lstEnquiryDashB = new List<ModelOffSiteMonitoring>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_MonitoringDetails", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                if (Type == "IRN")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "26");
                }
                else
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "25");
                }

                CMDGetEnquriy.Parameters.AddWithValue("@SubJob_No", Sub_JobNo);
               

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new ModelOffSiteMonitoring
                           {
                               ReportName = Convert.ToString(dr["ReportName"]),
                               PK_CALL_ID = Convert.ToInt32(dr["PK_IVR_ID"]),
                               //PK_CALL_ID = Convert.ToInt32(dr["PK_Call_id"]),

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


        public ModelOffSiteMonitoring GetDataByControllNo(string TUVIcontrolnumber)
        {
            con.Open();
            ModelOffSiteMonitoring _vmcompany = new ModelOffSiteMonitoring();
            List<string> Selected = new List<string>();
            string[] splitedProduct_Name;
            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_MonitoringDetails", con);
                GetAddress.CommandType = CommandType.StoredProcedure;
                GetAddress.Parameters.AddWithValue("@SP_Type", "27");
                GetAddress.Parameters.AddWithValue("@Subjobno", TUVIcontrolnumber);

                SqlDataReader dr = GetAddress.ExecuteReader();
                while (dr.Read())
                {

                    _vmcompany.Date = Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy");
                    _vmcompany.InspectorName = dr["Inspector_Name"].ToString();
                    _vmcompany.TUVIcontrolnumber = dr["TUV_Control_Number"].ToString();
                    _vmcompany.CustomerName = dr["Customer_Name"].ToString();
                    _vmcompany.EndCustomerName = dr["End_Customer_Name"].ToString();
                    _vmcompany.ProjectName = dr["Project_Name"].ToString();
                    _vmcompany.VendorName = dr["VendorName"].ToString();
                    _vmcompany.VendorLocation = dr["Vendor_Location"].ToString();
                    
                    //_vmcompany.Item_Inspected = dr["Item_Inspected"].ToString();

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
                cmd.Parameters.AddWithValue("@SP_Type", 28);
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



        //public string GetReportListBySubjobno(ModelOffSiteMonitoring CM)//Get All DropDownlist 
        //{

        //    //DataSet DSGetddlList = new DataSet();
        //    string Result = string.Empty;
        //    int ReturnId = 0;
        //    con.Open();
        //    try
        //    {
 
        //            SqlCommand CMDType = new SqlCommand("SP_MonitoringDetails", con);
        //            CMDType.CommandType = CommandType.StoredProcedure;
        //            if (CM.Type == "IVR")
        //            {
        //                CMDType.Parameters.AddWithValue("@SP_Type", 27);
        //            }
        //            else if (CM.Type == "IRN")
        //            {
        //                CMDType.Parameters.AddWithValue("@SP_Type", 34);
        //            }
        //            else
        //            {

        //            }

        //            CMDType.Parameters.AddWithValue("@PK_Call_Id", CM.PK_CALL_ID);
        //            CMDType.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
        //            SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDType);
        //           // Result=   SDAGetDdlLst.(DSGetddlList);
        //        Result = CMDType.ExecuteNonQuery().ToString();
        //    }
        //        catch (Exception ex)
        //        {
        //            string Error = ex.Message.ToString();
        //        }
        //        finally
        //        {
        //        con.Close();
        //        }
        //        return Result;
            
            


        //   }





      


        public DataSet EditReport(string UIN)
        {
            DataSet DTEditComplaints = new DataSet();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_MonitoringDetails", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 31);
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






        //public List<ModelOffSiteMonitoring> GetReportListBySubjobno(string Type , String Pk_Call_No)// Binding Sales Masters DashBoard of Master Page 
        //{
        //    DataTable DTEMDashBoard = new DataTable();
        //    List<ModelOffSiteMonitoring> lstEnquiryDashB = new List<ModelOffSiteMonitoring>();
        //    try
        //    {
        //        SqlCommand CMDGetEnquriy = new SqlCommand("SP_MonitoringDetails", con);
        //        CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
        //        CMDGetEnquriy.CommandTimeout = 1000000;
        //        if (Type == "IRN")
        //        {
        //            CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "34");
        //        }
        //        else
        //        {
        //            CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "27");
        //        }

        //        CMDGetEnquriy.Parameters.AddWithValue("@PK_Call_Id", Pk_Call_No);


        //        SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
        //        SDAGetEnquiry.Fill(DTEMDashBoard);
        //        if (DTEMDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in DTEMDashBoard.Rows)
        //            {
        //                lstEnquiryDashB.Add(
        //                   new ModelOffSiteMonitoring
        //                   {
        //                Date = Convert.ToString(dr["Date"]),

        //                ProjectName = Convert.ToString(dr["Project_Name"]),
        //                EndCustomerName = Convert.ToString(dr["EndUser"]),
        //                TUVIcontrolnumber = Convert.ToString(dr["TUVI_Control_Number"]),
        //                VendorName = Convert.ToString(dr["vendor_name"]),
        //                TUVIConrolNumber = Convert.ToString(dr["TUVIControlNumber"]),
        //                Item_Inspected = Convert.ToString(dr["Item_Inspected"]),
        //                Date_of_PO = Convert.ToString(dr["TopvendorPODate"]),
        //                Sub_Vendor_Name = Convert.ToString(dr["SubVendorName"]),
        //                Po_No_SubVendor = Convert.ToString(dr["SubVendorPONo"]),
        //                SubSubVendorDate_of_PO = Convert.ToString(dr["SubVendorPoDate"]),
        //                DEC_PMC_EPC_Name = Convert.ToString(dr["DECName"]),
        //                DEC_PMC_EPC_Assignment_No = Convert.ToString(dr["DECNumber"]),
        //                Po_No = Convert.ToString(dr["Vendor_Po_No"]),
        //                inspectorId = Convert.ToString(dr["inspectorId"]),
        //                //CM. bjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notific
        //                VendorLocation = Convert.ToString(dr["Vendor_Location"]),
        //                InspectorName = Convert.ToString(dr["Inspector_Name"]),
        //                CustomerName = Convert.ToString(dr["Customer_name"]),
        //                PK_CALL_ID = Convert.ToInt32(dr["PK_Call_ID"]),

        //            }
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

        public DataSet GetReportListBySubjobno(string Type,String Pk_Call_No)//Get All DropDownlist 
        {

            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_MonitoringDetails", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                //CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 27);
                if (Type == "IRN")
                           {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "34");
                           }
                           else
                       {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "27");
                      }

                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_Id", Pk_Call_No);
                //CMDGetDdlLst.Parameters.AddWithValue("@ReportName", ReportName);
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
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 6);
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

        //added bu shrutika salve  28092023

        public string AddRatingAnswers(ModelOffSiteMonitoring CM)
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


        public DataTable EditRatingQuetion(string UIN)
        {
            DataTable DTEditComplaints = new DataTable();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_MonitoringRating", con);
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

//added by shrutika salve 03102923
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


        public string updateRatingAnswers(ModelOffSiteMonitoring CM)
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

    }
}