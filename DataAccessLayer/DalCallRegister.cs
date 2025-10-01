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
    public class DalCallRegister
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);


        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public List<CallsRegister> GetData()
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CallsRegister> lstcalls = new List<CallsRegister>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("Sp_CallRegister", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "2");
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);

                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstcalls.Add(
                           new CallsRegister
                           {
                               Job = Convert.ToString(dr["Job"]),
                               call_no = Convert.ToString(dr["call_no"]),
                               Call_Received_Date = Convert.ToString(dr["Call Received Date"]),
                               Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                               Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                               createddate = Convert.ToString(dr["CreatedDate"]),
                               CreatedBy = Convert.ToString(dr["CreatedBy"]),
                               Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                               Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                               Continuous_Call = Convert.ToString(dr["Continuous Call"]),
                               inspector = Convert.ToString(dr["Inspector"]),
                               status = Convert.ToString(dr["Status"]),
                               iscompetant = Convert.ToString(dr["iscompetant"]),
                               CallAssignBy = Convert.ToString(dr["CallAssignBy"]),
                               client = Convert.ToString(dr["client"]),
                               End_Customer = Convert.ToString(dr["End_Customer"]),
                               Vendor_Name = Convert.ToString(dr["Vendor_Name"]),

                               //added by shrutika salve 29012023
                               ProjectName = Convert.ToString(dr["Project_Name"]),
                               DECPMCName = Convert.ToString(dr["DECName"]),
                               itemTobeInspected = Convert.ToString(dr["Itemtobeinspected"]),
                               stageOfinspection = Convert.ToString(dr["stageInspection"]),
                               primaryMaterial = Convert.ToString(dr["PrimaryMaterial"]),
                               StageDescription = Convert.ToString(dr["Description"]),
                               itemQty = Convert.ToString(dr["Quantity"]),
                               EstimatedTimeinHours = Convert.ToString(dr["EstimatedHours"]),
                               FormFilled = Convert.ToString(dr["FormFilled"]),
                               IsVerified = Convert.ToString(dr["IsVerified"]),


                               //added by shrutika salve 29032024
                               Finalinsepection = Convert.ToString(dr["FinalInspection"]),
                               ManMonthsAssi = Convert.ToString(dr["ManMonthsAssignment"]),
                               subendusername = Convert.ToString(dr["subendUsername"]),
                               subsubendusername = Convert.ToString(dr["SubSubendUsername"]),
                               //added by shrutika salve 24042024
                               vendorLoaction = Convert.ToString(dr["InspectionLocation"]),
                               SAP_NO = Convert.ToString(dr["sapnumber"]),
                               CallCancelDate=Convert.ToString(dr["CallCancelledDate"]),
                               callcancelby = Convert.ToString(dr["CallCancelledBy"]),
                               CallAssignDate = Convert.ToString(dr["callAssignDate"]),
                               //CallAssignBy = Convert.ToString(dr["CallCancelledBy"]),

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
            return lstcalls;
        }

        public DataTable CallsRegisterDataSearch(string fromDate, string ToDate)
        {
            DataTable dsCalls = new DataTable();
            try
            {
                SqlCommand cmdCall = new SqlCommand("Sp_CallRegister_1", con);
                cmdCall.CommandType = CommandType.StoredProcedure;
                cmdCall.Parameters.AddWithValue("@SP_Type", "1");

                cmdCall.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(fromDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                cmdCall.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                cmdCall.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);



                SqlDataAdapter daCall = new SqlDataAdapter(cmdCall);
                cmdCall.CommandTimeout = 1000000;
                daCall.Fill(dsCalls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsCalls.Dispose();
            }
            return dsCalls;
        }

        /*
        public DataTable CallsRegisterDataSearch(string fromDate, string ToDate)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_CallRegister", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");
               // cmd.Parameters.AddWithValue("@fromDate", fromDate);
              //  cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@fromDate", DateTime.ParseExact(DateTime.ParseExact(fromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                //cmd.CommandTimeout = 1000000;
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
        */
    }
}