using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

using System.Web;

namespace TuvVision.Models
{
    public class OnsiteMonitoring
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

            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_MonitoringDetails", con);
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

        //public DataTable InsertOnSiteMonitoring(string UserDetails,monitors CM)
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
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", "4");
        //        //CMDInsertUpdateUsers.Parameters.AddWithValue("@Qid", Qid);
        //        //SqlDataReader dr = CMDInsertUpdateUsers.ExecuteReader();


        //        //CMDInsertUpdateUsers.Parameters.AddWithValue("@ID", System.Web.HttpContext.Current.Session["UserIDs"]);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@UIN", CM.UIN);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifiedBy", CM.ModifiedBy);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifiedDate", CM.ModifiedDate);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", CM.InspectorComment);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", CM.Reporting_manager_comments);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", CM.Inspector_Name);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@Manager", CM.Manager);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", CM.Inspector_Level_of_authorisation);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", CM.Scope);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", CM.Monitor_level_of_authorisation);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", CM.on_site_time);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", CM.off_site_time);
        //        CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", CM.travel_time);
        //        //CMDInsertUpdateUsers.Parameters.AddWithValue("@Qid", CM.Qid);
        //        //CMDInsertUpdateUsers.Parameters.AddWithValue("@Ans", CM.Ans);

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


        public List<monitors> InsertDetails(monitors CM)
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
                CMDInsertUpdateUsers.Parameters.AddWithValue("@InspectorComment", CM.InspectorComment);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                CMDInsertUpdateUsers.Parameters.AddWithValue("@ManagerComments", CM.Reporting_manager_comments);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", CM.Inspector_Name);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspecto_level", CM.Inspector_Level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Monitor_level", CM.Monitor_level_of_authorisation);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@Scope", CM.IAFScopeName);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OnSiteTime", CM.on_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@OffSiteTime", CM.off_site_time);
                CMDInsertUpdateUsers.Parameters.AddWithValue("@TravelTime", CM.travel_time);
               



                // Result1 = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDInsertUpdateUsers);
                SDAScripName.Fill(DTExistUserName);

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
            return lstEnquiryDashB;
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


    }
}