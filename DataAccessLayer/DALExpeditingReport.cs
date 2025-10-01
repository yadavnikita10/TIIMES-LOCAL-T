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
    public class DALExpeditingReport
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public DataSet EditExpeditingReportByCall(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_ExpeditingReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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
        public DataSet EditExpeditingReport(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_ExpeditingReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 1);
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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


        public string InsertUpdateExpediting(ExpeditingModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_Expediting_Id == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_ExpeditingReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Controle_No", CPM.Controle_No);
                    if (CPM.Executing_branchId != null)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Executing_branchId", CPM.Executing_branchId);
                    }
                    else
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Executing_branchId", CPM.Executing_branchId);
                    }

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@NotificationNumber", CPM.NotificationNumber);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Date_Of_Expediting", CPM.Date_Of_Expediting);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ExpeditingLocation", CPM.ExpeditingLocation);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Customer_Name", CPM.Customer_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@End_Customer_Name", CPM.End_Customer_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DEC_PMC_EPC_Name", CPM.DEC_PMC_EPC_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DEC_PMC_EPC_Assignment_No", CPM.DEC_PMC_EPC_Assignment_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@VendorName", CPM.VendorName);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_No", CPM.Po_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sub_VendorName", CPM.Sub_VendorName);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SubPo_No", CPM.SubPo_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SubJob_No", CPM.Sub_Job_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_call_id);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ClosureRemarks", CPM.closureRemarks);
                    //CMDInsertUpdatebranch.Parameters.AddWithValue("@Contractual_DeliveryDate", DateTime.ParseExact(CPM.Contractual_DeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IsComfirmation", CPM.IsComfirmation); //added by nikita on 08102024
                    if (CPM.Contractual_DeliveryDate == null)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Contractual_DeliveryDate", CPM.Contractual_DeliveryDate);
                    }
                    else
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Contractual_DeliveryDate", DateTime.ParseExact(CPM.Contractual_DeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }


                    //CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", "Exp");

                    if (CPM.Po_Date == null)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_Date", CPM.Po_Date);

                    }
                    else
                    {
                        //CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_Date", DateTime.ParseExact(CPM.Po_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_Date", CPM.Po_Date);
                    }
                    if (CPM.SubPo_Date == null)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@SubPo_Date", CPM.SubPo_Date);
                    }
                    else
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@SubPo_Date", DateTime.ParseExact(CPM.SubPo_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    }
                    //CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", "Exp");
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CallIDs", CPM.CallIDs);

                    if (CPM.checkCustomer == true)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@DCustomer", "1");
                    }
                    if (CPM.Vendor == true)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@DVendor", "1");
                    }
                    if (CPM.TUVI == true)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@DTuvi", "1");
                    }
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();

                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_ExpeditingReport", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 4);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Controle_No", CPM.Controle_No);
                    if (CPM.Executing_branchId != null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Executing_branchId", CPM.Executing_branchId);
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Executing_branchId", CPM.Executing_branchId);
                    }

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@NotificationNumber", CPM.NotificationNumber);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Date_Of_Expediting", CPM.Date_Of_Expediting);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ExpeditingLocation", CPM.ExpeditingLocation);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Customer_Name", CPM.Customer_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@End_Customer_Name", CPM.End_Customer_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DEC_PMC_EPC_Name", CPM.DEC_PMC_EPC_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DEC_PMC_EPC_Assignment_No", CPM.DEC_PMC_EPC_Assignment_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@VendorName", CPM.VendorName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_No", CPM.Po_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Sub_VendorName", CPM.Sub_VendorName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubPo_No", CPM.SubPo_No);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@SubJob_No", CPM.Sub_Job_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_call_id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ClosureRemarks", CPM.closureRemarks);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@IsComfirmation", CPM.IsComfirmation); //added by nikita on 08102024
                    if (CPM.Contractual_DeliveryDate == null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Contractual_DeliveryDate", CPM.Contractual_DeliveryDate);
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Contractual_DeliveryDate", DateTime.ParseExact(CPM.Contractual_DeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }


                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Type", "Exp");


                    if (CPM.Po_Date == null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_Date", CPM.Po_Date);
                    }
                    else
                    {
                        // CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_Date", DateTime.ParseExact(CPM.Po_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_Date", CPM.Po_Date);
                    }
                    if (CPM.SubPo_Date == null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@SubPo_Date", CPM.SubPo_Date);
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@SubPo_Date", DateTime.ParseExact(CPM.SubPo_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CallIDs", CPM.CallIDs);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_Expediting_Id", CPM.PK_Expediting_Id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateBranch.ExecuteNonQuery().ToString();
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

        public DataTable GetReportDashboard()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 6);
                //CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);

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

        public DataTable GetitemDescription(int PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport_Singleitem", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 1000000;
                //CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 7);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
                
                //CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string InsertItemDescription(ExpItemDescription SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 8);
                cmd.Parameters.AddWithValue("@PK_call_id", SR.PK_Call_ID);
                cmd.Parameters.AddWithValue("@PO_Item_Number", SR.PO_Item_Number);
                cmd.Parameters.AddWithValue("@ItemCode", SR.ItemCode);
                cmd.Parameters.AddWithValue("@ItemDescription", SR.ItemDescription);
                cmd.Parameters.AddWithValue("@Equipment_TagNo", SR.Equipment_TagNo);
                cmd.Parameters.AddWithValue("@Quantity", SR.Quantity);
                cmd.Parameters.AddWithValue("@Unit", SR.Unit);
                cmd.Parameters.AddWithValue("@itemName", SR.itemName);
                //cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", SR.EstimatedDeliveryDate);
                if (SR.EstimatedDeliveryDate == null || SR.EstimatedDeliveryDate == "")
                {
                    cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", SR.EstimatedDeliveryDate);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", DateTime.ParseExact(SR.EstimatedDeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }

                if (SR.Contractual_DeliveryDateAsPerPO == null || SR.Contractual_DeliveryDateAsPerPO == "")
                {
                    cmd.Parameters.AddWithValue("@Contractual_DeliveryDateAsPerPO", SR.Contractual_DeliveryDateAsPerPO);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Contractual_DeliveryDateAsPerPO", DateTime.ParseExact(SR.Contractual_DeliveryDateAsPerPO, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }

                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string InsertUpdateItemDescription(ExpItemDescription SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (SR.PK_Item_Detail == 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", 8);
                    cmd.Parameters.AddWithValue("@PK_call_id", SR.PK_Call_ID);
                    cmd.Parameters.AddWithValue("@PO_Item_Number", SR.PO_Item_Number);
                    cmd.Parameters.AddWithValue("@ItemCode", SR.ItemCode);
                    cmd.Parameters.AddWithValue("@ItemDescription", SR.ItemDescription);
                    cmd.Parameters.AddWithValue("@Equipment_TagNo", SR.Equipment_TagNo);
                    cmd.Parameters.AddWithValue("@Quantity", SR.Quantity);
                    cmd.Parameters.AddWithValue("@Unit", SR.Unit);
                    //cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", SR.EstimatedDeliveryDate);
                    if (SR.EstimatedDeliveryDate == null || SR.EstimatedDeliveryDate == "")
                    {
                        cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", SR.EstimatedDeliveryDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", DateTime.ParseExact(SR.EstimatedDeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    if (SR.Contractual_DeliveryDateAsPerPO == null || SR.Contractual_DeliveryDateAsPerPO == "")
                    {
                        cmd.Parameters.AddWithValue("@Contractual_DeliveryDateAsPerPO", SR.Contractual_DeliveryDateAsPerPO);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Contractual_DeliveryDateAsPerPO", DateTime.ParseExact(SR.Contractual_DeliveryDateAsPerPO, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", 10);
                    cmd.Parameters.AddWithValue("@PK_call_id", SR.PK_Call_ID);
                    cmd.Parameters.AddWithValue("@PO_Item_Number", SR.PO_Item_Number);
                    cmd.Parameters.AddWithValue("@ItemCode", SR.ItemCode);
                    cmd.Parameters.AddWithValue("@ItemDescription", SR.ItemDescription);
                    cmd.Parameters.AddWithValue("@Equipment_TagNo", SR.Equipment_TagNo);
                    cmd.Parameters.AddWithValue("@Quantity", SR.Quantity);
                    cmd.Parameters.AddWithValue("@Unit", SR.Unit);
                    //cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", SR.EstimatedDeliveryDate);
                    if (SR.EstimatedDeliveryDate == null || SR.EstimatedDeliveryDate == "")
                    {
                        cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", SR.EstimatedDeliveryDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", DateTime.ParseExact(SR.EstimatedDeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    if (SR.Contractual_DeliveryDateAsPerPO == null || SR.Contractual_DeliveryDateAsPerPO == "")
                    {
                        cmd.Parameters.AddWithValue("@Contractual_DeliveryDateAsPerPO", SR.Contractual_DeliveryDateAsPerPO);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Contractual_DeliveryDateAsPerPO", DateTime.ParseExact(SR.Contractual_DeliveryDateAsPerPO, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }
                    cmd.Parameters.AddWithValue("@PK_Item_Detail", SR.PK_Item_Detail);
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

        public string UpdateItemDescription(ExpItemDescription SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 10);
                cmd.Parameters.AddWithValue("@PK_call_id", SR.PK_Call_ID);
                cmd.Parameters.AddWithValue("@PO_Item_Number", SR.PO_Item_Number);
                cmd.Parameters.AddWithValue("@ItemCode", SR.ItemCode);
                cmd.Parameters.AddWithValue("@ItemDescription", SR.ItemDescription);
                cmd.Parameters.AddWithValue("@Equipment_TagNo", SR.Equipment_TagNo);
                cmd.Parameters.AddWithValue("@Quantity", SR.Quantity);
                cmd.Parameters.AddWithValue("@Unit", SR.Unit);
                //cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", SR.EstimatedDeliveryDate);
                //if (SR.Contractual_DeliveryDateAsPerPO == null || SR.Contractual_DeliveryDateAsPerPO == "")
                //{
                //    cmd.Parameters.AddWithValue("@Contractual_DeliveryDateAsPerPO", SR.Contractual_DeliveryDateAsPerPO);
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@Contractual_DeliveryDateAsPerPO", DateTime.ParseExact(SR.Contractual_DeliveryDateAsPerPO, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                //}
                if (SR.EstimatedDeliveryDate == null || SR.EstimatedDeliveryDate == "")
                {
                    cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", SR.EstimatedDeliveryDate);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", DateTime.ParseExact(SR.EstimatedDeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }

                if (SR.Contractual_DeliveryDateAsPerPO == null || SR.Contractual_DeliveryDateAsPerPO == "")
                {
                    cmd.Parameters.AddWithValue("@Contractual_DeliveryDateAsPerPO", SR.Contractual_DeliveryDateAsPerPO);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Contractual_DeliveryDateAsPerPO", DateTime.ParseExact(SR.Contractual_DeliveryDateAsPerPO, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }
                cmd.Parameters.AddWithValue("@PK_Item_Detail", SR.PK_Item_Detail);
                cmd.Parameters.AddWithValue("@itemName", SR.itemName);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataSet EditItemDescription(ExpItemDescription IVR)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_ExpeditingReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 9);
                CMDEditContact.Parameters.AddWithValue("@PK_Item_Detail", IVR.PK_Item_Detail);
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

        public string DeleteExpItemDescription(int? id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "11");
                cmd.Parameters.AddWithValue("@PK_Item_Detail", id);
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


        #region Progress
        public DataTable GetProgress()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "12");
                //CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);

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


        public string InsertUpdateProcess(Progress CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_Process_Id != null)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_ExpeditingReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", "POItemNoinsertNew");
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Process_Id", CPM.PK_Process_Id);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Call_Id", CPM.PK_Call_Id);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Stages", CPM.Id);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Item", CPM.item);
                    if (CPM.ExpectedStartDate == null)
                    {
                        CPM.ExpectedStartDate = null;
                    }
                    else
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@ExpectedStartDate", DateTime.ParseExact(DateTime.ParseExact(CPM.ExpectedStartDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                    }
                    if (CPM.ExpectedEndDate == null)
                    {
                        CPM.ExpectedEndDate = null;
                    }
                    else
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@ExpectedEndDate", DateTime.ParseExact(DateTime.ParseExact(CPM.ExpectedEndDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                    }

                    if (CPM.Actual_Start_Date == null)
                    {
                        CPM.Actual_Start_Date = null;
                    }
                    else
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Actual_Start_Date", DateTime.ParseExact(DateTime.ParseExact(CPM.Actual_Start_Date, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                    }





                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();

                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_ExpeditingReport", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", "POItemNoinsertNew");
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_Call_Id", CPM.PK_Call_Id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Stages", CPM.Id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Item", CPM.item);


                    if (CPM.ExpectedStartDate == null)
                    {
                        CPM.ExpectedStartDate = null;
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@ExpectedStartDate", DateTime.ParseExact(DateTime.ParseExact(CPM.ExpectedStartDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                    }
                    if (CPM.ExpectedEndDate == null)
                    {
                        CPM.ExpectedEndDate = null;
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@ExpectedEndDate", DateTime.ParseExact(DateTime.ParseExact(CPM.ExpectedEndDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                    }

                    if (CPM.Actual_Start_Date == null)
                    {
                        CPM.Actual_Start_Date = null;
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Actual_Start_Date", DateTime.ParseExact(DateTime.ParseExact(CPM.Actual_Start_Date, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                    }




                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateBranch.ExecuteNonQuery().ToString();
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

        #endregion

        #region Engineering

        public string InsertEngineering(Engineering SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (SR.PK_Engi_Id == 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", 15);
                    cmd.Parameters.AddWithValue("@Item", SR.Item);
                    cmd.Parameters.AddWithValue("@PK_call_id", SR.PK_Call_Id);
                    cmd.Parameters.AddWithValue("@ReferenceDocument", SR.ReferenceDocument);
                    cmd.Parameters.AddWithValue("@Number", SR.Number);
                    cmd.Parameters.AddWithValue("@Status", SR.Status);
                    cmd.Parameters.AddWithValue("@QuantityNumber", SR.QuantityNumber);


                    if (SR.StatusUpdatedOn == null)
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    if (SR.EstimatedEndDate == null)
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", SR.EstimatedEndDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", DateTime.ParseExact(SR.EstimatedEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }else
                {
                    SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", 18);
                    cmd.Parameters.AddWithValue("@Item", SR.Item);
                    cmd.Parameters.AddWithValue("@ReferenceDocument", SR.ReferenceDocument);
                    cmd.Parameters.AddWithValue("@Number", SR.Number);
                    cmd.Parameters.AddWithValue("@Status", SR.Status);
                    cmd.Parameters.AddWithValue("@PK_Engi_Id", SR.PK_Engi_Id);
                    cmd.Parameters.AddWithValue("@QuantityNumber", SR.QuantityNumber);

                    if (SR.StatusUpdatedOn == null)
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }
                    if (SR.EstimatedEndDate == null)
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", SR.EstimatedEndDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", DateTime.ParseExact(SR.EstimatedEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string UpdateEngineering(Engineering SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 18);
                cmd.Parameters.AddWithValue("@ReferenceDocument", SR.ReferenceDocument);
                cmd.Parameters.AddWithValue("@Number", SR.Number);
                cmd.Parameters.AddWithValue("@Status", SR.Status);
                cmd.Parameters.AddWithValue("@PK_Engi_Id", SR.PK_Engi_Id);
                cmd.Parameters.AddWithValue("@QuantityNumber", SR.QuantityNumber);

                if (SR.StatusUpdatedOn == null)
                {
                    cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }

                cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable GetEngineeringDataByPK_Call_Id(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "17");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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

        public DataTable ddlGetEngineeringStatus(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "46");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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


        public DataTable ddlGetDocumentStatus(string PK_Call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "63");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_call_id", PK_Call_id);
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


        public List<NameStatusEngineering> GetDDLStatus(string PK_Call_id)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "46");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_call_id", PK_Call_id);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public List<NameStatusEngineering> GetItem(String PK_Call_id)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "20");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_Call_id", PK_Call_id);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public string DeleteEngineeringIfPresent(String pk_engid)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "DeleteEngi");
                cmd.Parameters.AddWithValue("@PK_Engi_Id", pk_engid);



                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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




        #endregion


        #region Material

        public List<NameStatusEngineering> GetMaterialStatus(string Pk_Call_id)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "47");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_call_id", Pk_Call_id);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public DataTable GetMaterialStatusDynamic(string Pk_Call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "47");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_call_id", Pk_Call_id);
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

        public DataTable GetItemDynamic(String PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "20");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_Id", PK_Call_Id);
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

        public DataTable GetMaterialDataByPK_Call_Id(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "19");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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


        public string InsertMaterial(Material SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (SR.PK_Material_Id == 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", 22);
                    cmd.Parameters.AddWithValue("@Item", SR.Item);
                    cmd.Parameters.AddWithValue("@Material_Category", SR.Material_Category);
                    cmd.Parameters.AddWithValue("@Description", SR.Description);
                    cmd.Parameters.AddWithValue("@UOM", SR.UOM);
                    cmd.Parameters.AddWithValue("@Quantity", SR.Quantity);
                    cmd.Parameters.AddWithValue("@Source", SR.Source);
                    cmd.Parameters.AddWithValue("@Status", SR.Status);
                    cmd.Parameters.AddWithValue("@PONumber", SR.PONumber);
                    cmd.Parameters.AddWithValue("@PK_Call_Id", SR.PK_Call_Id);
                    cmd.Parameters.AddWithValue("@OrderPlacedOn", SR.OrderPlacedOn);


                    if (SR.StatusUpdatedOn == null)
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }
                    if (SR.EstimatedEndDate == null)
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", SR.EstimatedEndDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", DateTime.ParseExact(SR.EstimatedEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    //added by shrutika salve 02012023
                    if (SR.PODate == null)
                    {
                        cmd.Parameters.AddWithValue("@Po_Date", SR.PODate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Po_Date", DateTime.ParseExact(SR.PODate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }
                    if (SR.Contractual_DeliveryDateAsPerPO == null)
                    {
                        cmd.Parameters.AddWithValue("@Contractual_DeliveryDate", SR.Contractual_DeliveryDateAsPerPO);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Contractual_DeliveryDate", DateTime.ParseExact(SR.Contractual_DeliveryDateAsPerPO, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }
                    if (SR.EstimatedDeliveryDate == null)
                    {
                        cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", SR.EstimatedDeliveryDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EstimatedDeliveryDate", DateTime.ParseExact(SR.EstimatedDeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }


                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    
                        SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SP_Type", 23);
                        cmd.Parameters.AddWithValue("@PK_Material_Id", SR.PK_Material_Id);
                        cmd.Parameters.AddWithValue("@Item", SR.Item);
                        cmd.Parameters.AddWithValue("@Material_Category", SR.Material_Category);
                        cmd.Parameters.AddWithValue("@Description", SR.Description);
                        cmd.Parameters.AddWithValue("@UOM", SR.UOM);
                        cmd.Parameters.AddWithValue("@Quantity", SR.Quantity);
                        cmd.Parameters.AddWithValue("@Source", SR.Source);
                        cmd.Parameters.AddWithValue("@Status", SR.Status);
                        cmd.Parameters.AddWithValue("@PONumber", SR.PONumber);
                        cmd.Parameters.AddWithValue("@OrderPlacedOn", SR.OrderPlacedOn);
                        cmd.Parameters.AddWithValue("@PK_Call_Id", SR.PK_Call_Id);



                        if (SR.StatusUpdatedOn == null)
                        {
                            cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        }
                        if (SR.EstimatedEndDate == null)
                        {
                            cmd.Parameters.AddWithValue("@EstimatedEndDate", SR.EstimatedEndDate);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@EstimatedEndDate", DateTime.ParseExact(SR.EstimatedEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        }
                        //added by shrutika salve 02012023
                        if (SR.PODate == null)
                        {
                            cmd.Parameters.AddWithValue("@Po_Date", SR.PODate);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Po_Date", DateTime.ParseExact(SR.PODate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        }
                        //if (SR.OrderPlacedOn == null)
                        //{
                        //    cmd.Parameters.AddWithValue("@OrderPlacedOn", SR.OrderPlacedOn);
                        //}
                        //else
                        //{
                        //    cmd.Parameters.AddWithValue("@OrderPlacedOn", DateTime.ParseExact(SR.OrderPlacedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        //}
                        if (SR.Contractual_DeliveryDateAsPerPO == null)
                        {
                            cmd.Parameters.AddWithValue("@Contractual_DeliveryDate", SR.Contractual_DeliveryDateAsPerPO);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Contractual_DeliveryDate", DateTime.ParseExact(SR.Contractual_DeliveryDateAsPerPO, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        }


                        cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string UpdateMaterial(Material SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 23);
                cmd.Parameters.AddWithValue("@PK_Material_Id", SR.PK_Material_Id);
                cmd.Parameters.AddWithValue("@Item", SR.Item);
                cmd.Parameters.AddWithValue("@Material_Category", SR.Material_Category);
                cmd.Parameters.AddWithValue("@Description", SR.Description);
                cmd.Parameters.AddWithValue("@UOM", SR.UOM);
                cmd.Parameters.AddWithValue("@Quantity", SR.Quantity);
                cmd.Parameters.AddWithValue("@Source", SR.Source);
                cmd.Parameters.AddWithValue("@Status", SR.Status);
                cmd.Parameters.AddWithValue("@PONumber", SR.PONumber);



                if (SR.StatusUpdatedOn == null)
                {
                    cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }
                if (SR.EstimatedEndDate == null)
                {
                    cmd.Parameters.AddWithValue("@EstimatedEndDate", SR.EstimatedEndDate);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@EstimatedEndDate", DateTime.ParseExact(SR.EstimatedEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }
                if (SR.OrderPlacedOn == null)
                {
                    cmd.Parameters.AddWithValue("@OrderPlacedOn", SR.OrderPlacedOn);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@OrderPlacedOn", DateTime.ParseExact(SR.OrderPlacedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }


                cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string DeleteMaterialIfPresent(String pk_materialid)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "DeleteMaterial");
                cmd.Parameters.AddWithValue("@PK_Material_Id", pk_materialid);



                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        #endregion


        #region Manufacturing

        public List<NameStatusEngineering> GetManufacturingStatus(string Pk_Call_id)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "48");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_call_id", Pk_Call_id);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public DataTable GetManufacturingDataByPK_Call_Id(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "24");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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
        public DataTable GetManufacturingStatusDynamic(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "48");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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

        public string InsertManufacturing(Manufacturing SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (SR.PK_Manufacturing_Id == 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", 26);
                    cmd.Parameters.AddWithValue("@Item", SR.Item);
                    cmd.Parameters.AddWithValue("@PartName", SR.PartName);
                    cmd.Parameters.AddWithValue("@UOM", SR.UOM);
                    cmd.Parameters.AddWithValue("@Quantity", SR.Quantity);
                    cmd.Parameters.AddWithValue("@Status", SR.Status);
                    cmd.Parameters.AddWithValue("@PK_Call_Id", SR.PK_Call_Id);


                    if (SR.StatusUpdatedOn == null)
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    if (SR.EstimatedEndDate == null)
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", SR.EstimatedEndDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", DateTime.ParseExact(SR.EstimatedEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }


                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", 27);
                    cmd.Parameters.AddWithValue("@PK_Manufacturing_Id", SR.PK_Manufacturing_Id);
                    cmd.Parameters.AddWithValue("@Item", SR.Item);
                    cmd.Parameters.AddWithValue("@PartName", SR.PartName);
                    cmd.Parameters.AddWithValue("@UOM", SR.UOM);
                    cmd.Parameters.AddWithValue("@Quantity", SR.Quantity);
                    cmd.Parameters.AddWithValue("@Status", SR.Status);
                    cmd.Parameters.AddWithValue("@PK_Call_Id", SR.PK_Call_Id);

                    if (SR.StatusUpdatedOn == null)
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    if (SR.EstimatedEndDate == null)
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", SR.EstimatedEndDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", DateTime.ParseExact(SR.EstimatedEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string UpdateManufacturing(Manufacturing SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 27);
                cmd.Parameters.AddWithValue("@PK_Manufacturing_Id", SR.PK_Manufacturing_Id);
                cmd.Parameters.AddWithValue("@Item", SR.Item);
                cmd.Parameters.AddWithValue("@PartName", SR.PartName);
                cmd.Parameters.AddWithValue("@UOM", SR.UOM);
                cmd.Parameters.AddWithValue("@Quantity", SR.Quantity);
                cmd.Parameters.AddWithValue("@Status", SR.Status);




                if (SR.StatusUpdatedOn == null)
                {
                    cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }



                cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public string DeleteManufacturingIfPresent(String PK_Manufacturing_Id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "DeleteManufacturing");
                cmd.Parameters.AddWithValue("@PK_Manufacturing_Id", PK_Manufacturing_Id);



                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable GetFinalStageMaster(string Pk_Call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "62");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_call_id", Pk_Call_id);
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



        #endregion


        #region Final Stage
        public DataTable GetStageMaster(string Pk_Call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "64");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_call_id", Pk_Call_id);
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

        public DataTable GetFinalStatusMaster(string Pk_Call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "49");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_call_id", Pk_Call_id);
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

        public List<NameStatusEngineering> GetddlConcernsStage()// Binding Sales Masters DashBoard of Master Pag
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "62");


                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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



        //bind final stage data 
        public List<NameStatusEngineering> GetddlFinalStageMaster(string pk_call_id)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "64");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_call_id", pk_call_id);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public List<NameStatusEngineering> GetddlFinalStageStatusMaster(string Pk_Call_id)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "49");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_call_id", Pk_Call_id);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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


        public DataTable GetFinalStageByPK_Call_Id(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "30");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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

        public string InsertFinalStage(FinalStages SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (SR.PK_FinalStage_Id == 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", 32);

                    cmd.Parameters.AddWithValue("@PK_Call_Id", SR.PK_Call_Id);
                    cmd.Parameters.AddWithValue("@Item", SR.Item);
                    cmd.Parameters.AddWithValue("@Stage", SR.Stage);
                    cmd.Parameters.AddWithValue("@Status", SR.Status);

                    if (SR.StatusUpdatedOn == null)
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }
                    if (SR.EstimatedEndDate == null)
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", SR.EstimatedEndDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", DateTime.ParseExact(SR.EstimatedEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }




                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", 79);

                    cmd.Parameters.AddWithValue("@PK_Call_Id", SR.PK_Call_Id);
                    cmd.Parameters.AddWithValue("@Item", SR.Item);
                    cmd.Parameters.AddWithValue("@Stage", SR.Stage);
                    cmd.Parameters.AddWithValue("@Status", SR.Status);
                    cmd.Parameters.AddWithValue("@PK_FinalStage_Id", SR.PK_FinalStage_Id);

                    if (SR.StatusUpdatedOn == null)
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }
                    if (SR.EstimatedEndDate == null)
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", SR.EstimatedEndDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EstimatedEndDate", DateTime.ParseExact(SR.EstimatedEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string DeleteFinalStage(String PK_FinalStage_Id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 31);
                cmd.Parameters.AddWithValue("@PK_FinalStage_Id", PK_FinalStage_Id);



                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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



        #endregion


        #region Concern

        public DataTable GetConcernDataByPK_Call_Id(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "33");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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

        public DataTable GetOpenConcernDataByPK_Call_Id(string PK_Call_Id)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "77");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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

        public DataTable GetClosedConcernDataByPK_Call_Id(string PK_Call_Id)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "78");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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


        public DataSet privousCommentsPK_Call_Id(string PK_Call_Id)//Get All DropDownlist 
        {

            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "67");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                //CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public List<NameStatusEngineering> GetddlConcernStageMaster()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "28");

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public List<NameStatusEngineering> GetddlConcernCategory()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "34");

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public DataTable GetConcernCategoryMaster()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "34");
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

        public DataTable GetItemMaster(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "20");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_Id", PK_Call_Id);
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

        public List<NameStatusEngineering> GetddlConcernStatus()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "35");

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public DataTable GetddlConcernStatusJson(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "35");
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

        public DataTable GetConcernStageJson()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "62");

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

        public DataTable GetConcernStatusJsonD(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "35");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_Id", PK_Call_Id);
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

        public string DeleteConcern(String pk_concerns_id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 36);
                cmd.Parameters.AddWithValue("@pk_concerns_id", pk_concerns_id);

                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string InsertConcern(Concerns SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (SR.PK_Concern_Id == 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", 37);

                    cmd.Parameters.AddWithValue("@PK_Call_Id", SR.PK_Call_ID);
                    cmd.Parameters.AddWithValue("@Category", SR.Category);
                    cmd.Parameters.AddWithValue("@Item", SR.Item);
                    cmd.Parameters.AddWithValue("@Stage", SR.Stage);
                    cmd.Parameters.AddWithValue("@Details", SR.Details);
                    cmd.Parameters.AddWithValue("@MitigationBy", SR.MitigationBy);
                    cmd.Parameters.AddWithValue("@MitigationByUserid", SR.MitigationByUserid);
                    cmd.Parameters.AddWithValue("@ResponsiblePerson", SR.ResponsiblePerson);
                    cmd.Parameters.AddWithValue("@ResponsiblePersonUserid", SR.pk_userid);
                    cmd.Parameters.AddWithValue("@Status", SR.Status);

                    cmd.Parameters.AddWithValue("@Comment", SR.Comment);
                    cmd.Parameters.AddWithValue("@ActionReq", SR.ActionReq);



                    if (SR.Date == null)
                    {
                        cmd.Parameters.AddWithValue("@Date", SR.Date);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(SR.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }
                    if (SR.ExpectedClosureDate == null)
                    {
                        cmd.Parameters.AddWithValue("@ExpectedClosureDate", SR.ExpectedClosureDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ExpectedClosureDate", DateTime.ParseExact(SR.ExpectedClosureDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }
                    if (SR.StatusUpdatedOn == null)
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }




                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    
                        SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SP_Type", 80);
                        cmd.Parameters.AddWithValue("@pk_concerns_id", SR.PK_Concern_Id);
                        cmd.Parameters.AddWithValue("@PK_Call_Id", SR.PK_Call_ID);
                        cmd.Parameters.AddWithValue("@Category", SR.Category);
                        cmd.Parameters.AddWithValue("@Item", SR.Item);
                        cmd.Parameters.AddWithValue("@Stage", SR.Stage);
                        cmd.Parameters.AddWithValue("@Details", SR.Details);
                        cmd.Parameters.AddWithValue("@MitigationBy", SR.MitigationBy);
                        cmd.Parameters.AddWithValue("@MitigationByUserid", SR.MitigationByUserid);
                        cmd.Parameters.AddWithValue("@ResponsiblePerson", SR.ResponsiblePerson);
                        cmd.Parameters.AddWithValue("@ResponsiblePersonUserid", SR.pk_userid);
                        cmd.Parameters.AddWithValue("@Status", SR.Status);

                        cmd.Parameters.AddWithValue("@Comment", SR.Comment);
                        cmd.Parameters.AddWithValue("@ActionReq", SR.ActionReq);



                        if (SR.Date == null)
                        {
                            cmd.Parameters.AddWithValue("@Date", SR.Date);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(SR.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        }
                        if (SR.ExpectedClosureDate == null)
                        {
                            cmd.Parameters.AddWithValue("@ExpectedClosureDate", SR.ExpectedClosureDate);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ExpectedClosureDate", DateTime.ParseExact(SR.ExpectedClosureDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        }
                        if (SR.StatusUpdatedOn == null)
                        {
                            cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        }



                        cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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



        //added by shrutika salve 27122023

        public DataTable GetautosuggetionJsonD(string UserName)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "38");
                CMDGetDdlLst.Parameters.AddWithValue("@FirstName ", UserName);
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


        public DataTable GetUserName(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 43);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
                CMDSearchNameCode.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDSearchNameCode);
                SDAScripName.Fill(DTScripName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTScripName.Dispose();
            }
            return DTScripName;
        }


        #endregion


        public List<NamePoitemDetails> GetItemPOitem(string pk_call_id)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NamePoitemDetails> lstEnquiryDashB = new List<NamePoitemDetails>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "ItemNobind");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_call_id", pk_call_id);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NamePoitemDetails
                           {

                               Text = Convert.ToString(dr["Item"]),
                               Valuepo = Convert.ToString(dr["value1"]),

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

        //Added BY shrutika salve On 09 jan 2024
        public DataTable fetdataitemwise(string pk_call_id, string item)
        {


            string Result = "";
            con.Open();

            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DataTable getdata = new DataTable();

            try
            {


                SqlCommand CMD_getdataHistory = new SqlCommand("SP_ExpeditingReport", con);
                CMD_getdataHistory.CommandType = CommandType.StoredProcedure;
                CMD_getdataHistory.Parameters.AddWithValue("@SP_Type", "itemgetValueSearch");
                CMD_getdataHistory.Parameters.AddWithValue("@PK_call_id", pk_call_id);
                CMD_getdataHistory.Parameters.AddWithValue("@Item ", item);
                //CMD_getdataHistory.Parameters.AddWithValue("@CompanyName", pk_process_id);
                CMD_getdataHistory.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));


                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMD_getdataHistory);
                SDAScripName.Fill(getdata);

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

            return getdata;




        }

        public List<NameStatusEngineering> GetSatges(String PK_Call_id)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "39");
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Id"]),
                               Text = Convert.ToString(dr["Stages"]),
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


        public DataTable GetObservationDataByPK_Call_Id(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "52");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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


        //added by shrutika salve 18012023

        public string InsertObservation(Observation SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 50);

                cmd.Parameters.AddWithValue("@PK_call_id", SR.PK_Call_ID);
                cmd.Parameters.AddWithValue("@Stage", SR.Stage);
                cmd.Parameters.AddWithValue("@Findings", SR.Findings);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public string DeleteObservation(String PK_Call_Id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 51);
                cmd.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);



                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable GetObservationMaster()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "53");

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


        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, string PK_Call_ID, string Name)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("PK_Call_ID", typeof(string)));
                //DTUploadFile.Columns.Add(new DataColumn("EnquiryNumber", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("Name", typeof(string)));
                // DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                foreach (var item in lstFileUploaded)
                {
                    if (PK_Call_ID == "0")
                    {
                        item.FileName = Convert.ToString(PK_Call_ID) + '_' + item.FileName;
                    }
                    else
                    {
                        item.FileName = Convert.ToString(PK_Call_ID) + '_' + item.FileName;
                    }

                    DTUploadFile.Rows.Add(PK_Call_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, Name);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_ExpeditingRecordattachement", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@Expediting", DTUploadFile);
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


        public DataTable GetFileContent(string pk_call_id)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_ExpeditingRecordattachement", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@Pk_call_id", pk_call_id);
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

        public DataTable GetConFileExt(int FileID)
        {
            DataTable DTGetFileExtenstion = new DataTable();
            try
            {
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_ExpeditingRecordattachement", con);
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
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_ExpeditingRecordattachement", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@Pk_call_id", FileID);
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


        public string InsertUpdateReportImage(Photo CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_IP_Id == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_ExpeditingReportImages", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Heading", CPM.Heading);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Image", CPM.Image);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();


                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_ExpeditingReportImages", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Heading", CPM.Heading);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Image", CPM.Image);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_IP_Id", CPM.PK_IP_Id);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateItemDescription.ExecuteNonQuery().ToString();
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


        public DataTable GetReportImageByCall_Id(String PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReportImages", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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


        public string UpdateHeading(Photo CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_ExpeditingReportImages", con);
                CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 5);
                CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Heading", CPM.Heading);

                CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_IP_Id", CPM.PK_IP_Id);
                CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDInsertUpdateItemDescription.ExecuteNonQuery().ToString();


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


        public bool DeleteImage(int id)
        {
            SqlCommand CMDDeleteAcvt = new SqlCommand("SP_ExpeditingReportImages", con);
            CMDDeleteAcvt.CommandType = CommandType.StoredProcedure;
            CMDDeleteAcvt.Parameters.AddWithValue("@SP_Type", "6");
            CMDDeleteAcvt.Parameters.AddWithValue("@PK_IP_Id", id);
            con.Open();
            int i = CMDDeleteAcvt.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public DataTable EXpDalayWise(string pk_call_id)
        {


            string Result = "";
            con.Open();

            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DataTable getdata = new DataTable();

            try
            {


                SqlCommand CMD_getdataHistory = new SqlCommand("SP_ExpeditingSingleItemValueData", con);
                CMD_getdataHistory.CommandType = CommandType.StoredProcedure;
                //CMD_getdataHistory.Parameters.AddWithValue("@SP_Type", "54");
                CMD_getdataHistory.Parameters.AddWithValue("@PK_call_id", pk_call_id);
                //CMD_getdataHistory.CommandTimeout = 120;
                CMD_getdataHistory.CommandTimeout = 1000000000;
                //CMD_getdataHistory.Parameters.AddWithValue("@CompanyName", pk_process_id);
                //CMD_getdataHistory.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));


                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMD_getdataHistory);
                SDAScripName.Fill(getdata);

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

            return getdata;




        }


        public string InsertUpdateDelay(ExpDelays CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_ExpeditingReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", "55");
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Process_Id", CPM.pk_expDelayID);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Call_Id", CPM.PK_Call_ID);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@DelayBy", CPM.ExpDelayBy);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@DelayReasonfordelay", CPM.ExpReasonfordelay);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@DelayMitigationby", CPM.ExpMitigationby);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@DelayResponsiblePerson", CPM.ExpResponsiblePerson);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@Comments", CPM.ExpComments);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@item", CPM.item);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@Stages", CPM.Stage);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@ActionReq", CPM.ActionReq);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();




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


        public DataSet GetReportImageById(int? PK_IP_Id)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReportImages", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IP_Id", PK_IP_Id);
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



        //Payments & status Dropdown value  bind

        public DataTable EXpPayment(string pk_call_id)
        {

            string Result = "";
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DataTable getdata = new DataTable();

            try
            {


                SqlCommand CMD_getdataHistory = new SqlCommand("SP_ExpeditingReport", con);
                CMD_getdataHistory.CommandType = CommandType.StoredProcedure;
                CMD_getdataHistory.Parameters.AddWithValue("@SP_Type", 56);
                CMD_getdataHistory.Parameters.AddWithValue("@PK_call_id", pk_call_id);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMD_getdataHistory);
                SDAScripName.Fill(getdata);

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

            return getdata;

        }

        //insert and update data payments
        public string InsertUpdatePayments(ExpCommercial EM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_ExpeditingReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", "57");

                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Call_Id", EM.PK_Call_Id);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@ReferenceDocument", EM.Documents);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", EM.Status);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@Remarks", EM.Remarks);
                //CMDInsertUpdatebranch.Parameters.AddWithValue("@PaymentDate", EM.Date);
                if (EM.Date == null)
                {
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PaymentDate", EM.Date);
                }
                else
                {
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PaymentDate", DateTime.ParseExact(EM.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }
                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();




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


        public DataTable fetdataAllItemwise(string pk_call_id)
        {

            string Result = "";
            con.Open();

            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DataTable getdata = new DataTable();
            try
            {
                SqlCommand CMD_getdataHistory = new SqlCommand("SP_ExpeditingAllOverData", con);
                CMD_getdataHistory.CommandType = CommandType.StoredProcedure;

                CMD_getdataHistory.Parameters.AddWithValue("@PK_call_id", pk_call_id);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMD_getdataHistory);
                SDAScripName.Fill(getdata);

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

            return getdata;




        }


        public List<NameStatusEngineering> GetStatus()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "59");


                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public List<NameStatusEngineering> GetSecondStatus()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "60");


                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public List<NameStatusEngineering> GetLastStatus()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "61");


                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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


        //added by shrutika salve 25012024

        public List<NameStatusEngineering> GetDDLDocument(string PK_Call_id)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "63");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_call_id", PK_Call_id);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public string InsertdataEng(string stage, string pk_Call_id, string ActionValue)
        {
            string Result = "";
            con.Open();
            try
            {

                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                SqlCommand CMDInsert = new SqlCommand("SP_ExpeditingReport", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 65);
                CMDInsert.Parameters.AddWithValue("@ActionValue", ActionValue);
                CMDInsert.Parameters.AddWithValue("@Pk_call_id", pk_Call_id);
                CMDInsert.Parameters.AddWithValue("@Stage", stage);

                CMDInsert.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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


        public DataSet GetRecord(string pk_call_id, string stage)
        {
            DataSet DsClientFeedbackHistory = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@SP_Type", 66);
                cmd.Parameters.AddWithValue("@PK_call_id", pk_call_id);
                cmd.Parameters.AddWithValue("@stage", stage);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsClientFeedbackHistory);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsClientFeedbackHistory.Dispose();
            }
            return DsClientFeedbackHistory;
        }


        public DataSet privousCommentsitemwisedelay(string PK_Call_Id)//Get All DropDownlist 
        {

            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "68");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                //CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetActionDataByPK_Call_Id(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "69");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
                //CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string InsertUpdateAttendees(ExpeditingModel CPM) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {

                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 71);
                cmd.Parameters.AddWithValue("@Represting", CPM.Represting);
                cmd.Parameters.AddWithValue("@Name", CPM.Name);
                cmd.Parameters.AddWithValue("@pk_expeditingid", CPM.PK_Expediting_Id);
                cmd.Parameters.AddWithValue("@Designation", CPM.Designation);
                cmd.Parameters.AddWithValue("@Emailid", CPM.Email);
                cmd.Parameters.AddWithValue("@PK_call_id", CPM.PK_call_id);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public string DeleteAttendees(String PK_Call_Id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "72");
                cmd.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);



                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable EditExpeditingReportAttendeesName(int? PK_Call_ID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_ExpeditingReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 73);
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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


        public string UpdateDistributionlist(ExpeditingModel SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 74);
                cmd.Parameters.AddWithValue("@pk_call_id", SR.PK_call_id);
                if (SR.checkCustomer == true)
                {
                    cmd.Parameters.AddWithValue("@DCustomer", "1");
                }
                if (SR.Vendor == true)
                {
                    cmd.Parameters.AddWithValue("@DVendor", "1");
                }
                if (SR.TUVI == true)
                {
                    cmd.Parameters.AddWithValue("@DTuvi", "1");
                }


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

        public DataTable GetdistributionId(int pk_call_id)//Getting Data of Qutation Details
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 75);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@pk_call_id", pk_call_id);

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


        public DataTable GetObservation(string PK_Call_Id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "76");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_id", PK_Call_Id);
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


        public DataSet FillALLDropdown(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string id, string TagNumber,string ItemName)
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_FillALLDropdown", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@Company", Company);
                CMDGetDdlLst.Parameters.AddWithValue("@Project", Project);
                //CMDGetDdlLst.Parameters.AddWithValue("@VendorName", VendorName);
                CMDGetDdlLst.Parameters.AddWithValue("@SubVendorName", SubVendorname);
                CMDGetDdlLst.Parameters.AddWithValue("@PONumber", PONumber);
                CMDGetDdlLst.Parameters.AddWithValue("@ItemNumber", ItemNumber);
                CMDGetDdlLst.Parameters.AddWithValue("@itemCode", TagNumber);
                CMDGetDdlLst.Parameters.AddWithValue("@itemName", ItemName);
                CMDGetDdlLst.Parameters.AddWithValue("@SPType", id);
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

        public string ConvertDataTabletoString(DataTable dt)
        {

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);

        }

        public DataTable FillCompany(string Field)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_FillCompany", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@Field", Field);
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


        public DataSet FillALLDropdownConcerns(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string id, string stage)
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_FillALLDropdown", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@Company", Company);
                CMDGetDdlLst.Parameters.AddWithValue("@Project", Project);
                //CMDGetDdlLst.Parameters.AddWithValue("@VendorName", VendorName);
                CMDGetDdlLst.Parameters.AddWithValue("@SubVendorName", SubVendorname);
                CMDGetDdlLst.Parameters.AddWithValue("@PONumber", PONumber);
                CMDGetDdlLst.Parameters.AddWithValue("@ItemNumber", ItemNumber);
                CMDGetDdlLst.Parameters.AddWithValue("@stage", stage);
                CMDGetDdlLst.Parameters.AddWithValue("@SPType", id);
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

        public DataSet FillALLDropdownEng(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string id, string stage)
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_FillALLDropdown", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@Company", Company);
                CMDGetDdlLst.Parameters.AddWithValue("@Project", Project);
                //CMDGetDdlLst.Parameters.AddWithValue("@VendorName", VendorName);
                CMDGetDdlLst.Parameters.AddWithValue("@SubVendorName", SubVendorname);
                CMDGetDdlLst.Parameters.AddWithValue("@PONumber", PONumber);
                CMDGetDdlLst.Parameters.AddWithValue("@ItemNumber", ItemNumber);
                CMDGetDdlLst.Parameters.AddWithValue("@stage", stage);
                CMDGetDdlLst.Parameters.AddWithValue("@SPType", id);
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

        public DataTable GetConcernData(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string id, string stage)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_FillALLDropdown", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@Company", Company);
                CMDGetDdlLst.Parameters.AddWithValue("@Project", Project);
                //CMDGetDdlLst.Parameters.AddWithValue("@VendorName", VendorName);
                CMDGetDdlLst.Parameters.AddWithValue("@SubVendorName", SubVendorname);
                CMDGetDdlLst.Parameters.AddWithValue("@PONumber", PONumber);
                CMDGetDdlLst.Parameters.AddWithValue("@ItemNumber", ItemNumber);
                CMDGetDdlLst.Parameters.AddWithValue("@stage", stage);
                CMDGetDdlLst.Parameters.AddWithValue("@SPType", id);
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



        public string UpdateConcern(Concerns SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 81);
                cmd.Parameters.AddWithValue("@pk_concerns_id", SR.PK_Concern_Id);
                cmd.Parameters.AddWithValue("@Status", SR.Status);
                cmd.Parameters.AddWithValue("@Comment", SR.Comment);
                cmd.Parameters.AddWithValue("@Details", SR.Details);
                cmd.Parameters.AddWithValue("@MitigationBy", SR.MitigationBy);

                if (SR.StatusUpdatedOn == null)
                {
                    cmd.Parameters.AddWithValue("@StatusUpdatedOn", SR.StatusUpdatedOn);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StatusUpdatedOn", DateTime.ParseExact(SR.StatusUpdatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }

                cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        //added by nikita on20092024
        public DataTable GetEmaildata(int? pk_call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "1");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
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
        public DataTable GetAttachmentsData(int? Pk_Call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "2");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", Pk_Call_id);
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

        public DataTable UpdateMailFlag(int? pk_call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "7");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
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


        public DataTable GetAttachmentsData_(int? pk_ivr_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "6");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_ivr_id);
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


        public DataTable Updatetable_vendor_Email(int? pk_call_id, string Email, string type)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "9");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@Type", type);
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


        public DataTable UpdateClientTable(int? pk_call_id, string Email, string ClientName)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "10");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", ClientName);
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


        public DataTable UpdateTuvMail(int? pk_call_id, string Email)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "11");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@User", Email);
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


        public DataTable UpdateTUVEmial(int? pk_call_id, string Email, string ClientName)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "11");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@userid", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", ClientName);
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

        public DataTable updateClient(int? pk_call_id, string Email, string ClientName)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "12");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", ClientName);

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

        public DataTable updateVendor(int? pk_call_id, string Email, string Vendor_Name_Location)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "13");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Vendor_Name_Location);

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

        public DataTable updateSubVendor(int? pk_call_id, string Email, string Vendor_Name_Location)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "14");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Vendor_Name_Location);

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


        public DataTable updateSubSubVendor(int? pk_call_id, string Email, string Vendor_Name_Location)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "15");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Vendor_Name_Location);

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
        public DataTable DeleteEmail(int? pk_call_id, string EmailType, string Name, string Email)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_DeleteEmailData_Expediting", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                if (EmailType == "TUV")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "1");
                    CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                    CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                }
                else if (EmailType == "Client_Email")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "2");
                    CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Name);
                    CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                    CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                }
                else if (EmailType == "SubSub Vendor")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "4");
                    CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                    CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Name);
                    CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                }
                else if (EmailType == "Sub Vendor")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "3");
                    CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                    CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Name);
                    CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                }
                else if (EmailType == "SubSub Vendor")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "5");
                    CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", pk_call_id);
                    CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Name);
                    CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                }

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
        public DataTable SaveData_(string pk_call_id, string emails, string attachments, string mailsubject)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("ExpeditingReportData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "16");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID  ", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@Semd_Mails_To_Email", emails);
                CMDGetDdlLst.Parameters.AddWithValue("@Attachment_Send_To_client", attachments);
                CMDGetDdlLst.Parameters.AddWithValue("@MailSubject", mailsubject);
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



