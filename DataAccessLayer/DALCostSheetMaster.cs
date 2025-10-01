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
    public class DALCostSheetMaster
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);



        #region  Cost Sheet master


        public DataTable GetCostList(int? PK_QTID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CostSheetMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_QTID", PK_QTID);
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
		public DataTable GetCostListDomestic(int? PK_QTID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CostSheetMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "17");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_QTID", PK_QTID);
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


        public DataTable GetInspectionLocation()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CostSheetMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 15);
                
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


        public DataTable GetLegalApprovalStatus(int? PK_QTID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CostSheetMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "15K");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_QTID", PK_QTID);
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



        public DataTable GetCurrency()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CostSheetMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 16);

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
		
        public DataSet GetEstimatedAmount(int? PK_EQID)//Get All DropDownlist 
        {
            DataSet DSGetEstAmount = new DataSet();
            try
            {
                SqlCommand CMDEstAmount = new SqlCommand("SP_CostSheetMaster", con);
                CMDEstAmount.CommandType = CommandType.StoredProcedure;
                CMDEstAmount.Parameters.AddWithValue("@SP_Type", 6);
                CMDEstAmount.Parameters.AddWithValue("@EQ_ID", PK_EQID);
                CMDEstAmount.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDEstAmount);
                SDAGetDdlLst.Fill(DSGetEstAmount);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetEstAmount.Dispose();
            }
            return DSGetEstAmount;
        }
        public string InsertUpdateCost(CostSheet CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_Cs_Id == 0)
                {
                    SqlCommand CMDInsertUpdateCostSheet = new SqlCommand("SP_CostSheetMaster", con);
                    CMDInsertUpdateCostSheet.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateCostSheet.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateCostSheet.Parameters.AddWithValue("@Costsheet", CPM.Costsheet);
                    CMDInsertUpdateCostSheet.Parameters.AddWithValue("@EQ_ID", CPM.EQ_ID);
                    CMDInsertUpdateCostSheet.Parameters.AddWithValue("@RefNo", CPM.RefNo);
                    CMDInsertUpdateCostSheet.Parameters.AddWithValue("@PK_QTID", CPM.PK_QTID);
					CMDInsertUpdateCostSheet.Parameters.AddWithValue("@CostsheetType", CPM.CostSheetType);																					  

                    CMDInsertUpdateCostSheet.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateCostSheet.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand CMDUpdateCostSheet = new SqlCommand("SP_CostSheetMaster", con);
                    CMDUpdateCostSheet.CommandType = CommandType.StoredProcedure;
                    CMDUpdateCostSheet.Parameters.AddWithValue("@SP_Type", 3);
                    CMDUpdateCostSheet.Parameters.AddWithValue("@Status", "Approve");
                    CMDUpdateCostSheet.Parameters.AddWithValue("@PK_Cs_Id",CPM.PK_Cs_Id);
					CMDUpdateCostSheet.Parameters.AddWithValue("@CostsheetType", CPM.CostSheetType);																				
                    CMDUpdateCostSheet.Parameters.AddWithValue("@ModifyDate", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDUpdateCostSheet.ExecuteNonQuery().ToString();
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

        public DataTable GetCostListByStatus(int? PK_QTID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CostSheetMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 7);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_QTID", PK_QTID);
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

        public int ApproveCostSheet(int? PK_Cs_Id)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_CostSheetMaster", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 4);
                CMDContactDelete.Parameters.AddWithValue("@PK_Cs_Id", PK_Cs_Id);
                CMDContactDelete.Parameters.AddWithValue("@ModifyDate", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDContactDelete.ExecuteNonQuery();
                if (Result != 0)
                {
                    return Result;
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

        public DataTable GetEnquiryRefernceNo()
        {
            DataTable DTGetList = new DataTable();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_CostSheetMaster", con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetList.Parameters.AddWithValue("", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetList = new SqlDataAdapter(CMDGetList);
                SDAGetList.Fill(DTGetList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetList.Dispose();
            }
            return DTGetList;
        }


        public List<EnquiryMaster> GetEnquiryRefNoList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CostSheetMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 4);
              
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new EnquiryMaster
                           {
                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                              
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

        #endregion
        public string ChangeSheetStatus(int csid,CostSheet C)
        {
            string Result = string.Empty;
            try
            {
                SqlCommand CMDStatus = new SqlCommand("SP_CostSheetMaster", con);
                CMDStatus.CommandType = CommandType.StoredProcedure;
                CMDStatus.Parameters.AddWithValue("@SP_Type", 5);
                CMDStatus.Parameters.AddWithValue("@PK_Cs_Id", csid);
				CMDStatus.Parameters.AddWithValue("@PCHComment", C.PCHComment);															   
                con.Open();
                Result = CMDStatus.ExecuteNonQuery().ToString();
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
        public string ChangeApproval(int csid)
        {
            string Result = string.Empty;
            try
            {
                SqlCommand CMDStatus = new SqlCommand("SP_CostSheetMaster", con);
                CMDStatus.CommandType = CommandType.StoredProcedure;
                CMDStatus.Parameters.AddWithValue("@SP_Type", 8);
                CMDStatus.Parameters.AddWithValue("@PK_Cs_Id", csid);
                con.Open();
                Result = CMDStatus.ExecuteNonQuery().ToString();
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

        public string ChangeCLSheetStatus(int csid)
        {
            string Result = string.Empty;
            try
            {
                SqlCommand CMDStatus = new SqlCommand("SP_CostSheetMaster", con);
                CMDStatus.CommandType = CommandType.StoredProcedure;
                CMDStatus.Parameters.AddWithValue("@SP_Type", 9);
                CMDStatus.Parameters.AddWithValue("@PK_Cs_Id", csid);
                con.Open();
                Result = CMDStatus.ExecuteNonQuery().ToString();
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

        public string ChangeCLApproval(int csid)
        {
            string Result = string.Empty;
            try
            {
                SqlCommand CMDStatus = new SqlCommand("SP_CostSheetMaster", con);
                CMDStatus.CommandType = CommandType.StoredProcedure;
                CMDStatus.Parameters.AddWithValue("@SP_Type", 10);
                CMDStatus.Parameters.AddWithValue("@PK_Cs_Id", csid);
                con.Open();
                Result = CMDStatus.ExecuteNonQuery().ToString();
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
		public int DeleteCostSheet(int Appeal_ID)
        {
            int result = 0;
            con.Open();
            try
            {

                SqlCommand CMDAppeal = new SqlCommand("SP_CostSheetMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 11);
                CMDAppeal.Parameters.AddWithValue("@PK_Cs_Id", Appeal_ID);
                result = CMDAppeal.ExecuteNonQuery();
                if (result != 0)
                {
                    return result;
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
            return result;
        }

        public string SendForApprovel(int Appeal_ID,CostSheet C)
        {
            string result = string.Empty;
            DataTable DTEMDashBoard = new DataTable();
            con.Open();
            try
            {

                SqlCommand CMDAppeal = new SqlCommand("SP_CostSheetMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 12);
                CMDAppeal.Parameters.AddWithValue("@PK_Cs_Id", Appeal_ID);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDAppeal);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    result = DTEMDashBoard.Rows[0][0].ToString();
                    ///result = CMDAppeal.ExecuteNonQuery();                    
                    //if (result != 0)
                    //{
                    //    return result;
                    //}
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
            return result;
        }

        public DataTable GetquoationDetails(string csID)
        {
            DataTable dtdetails = new DataTable();
            con.Open();
            try
            {

                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CostSheetMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "14N");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Cs_Id", csID);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);

                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(dtdetails);
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
            return dtdetails;
        }

        public DataTable GetQuotUserDetails(string csID)
        {
            DataTable dtdetails = new DataTable();
            con.Open();
            try
            {

                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CostSheetMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
              //  CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 18);
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "14N");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Cs_Id", csID);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(dtdetails);
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
            return dtdetails;
        }

        public string AddComment(int csid, CostSheet C)
        {
            string Result = string.Empty;
            try
            {
                SqlCommand CMDStatus = new SqlCommand("SP_CostSheetMaster", con);
                CMDStatus.CommandType = CommandType.StoredProcedure;
                CMDStatus.Parameters.AddWithValue("@SP_Type", 13);
                CMDStatus.Parameters.AddWithValue("@PK_Cs_Id", C.PK_Cs_Id);
                CMDStatus.Parameters.AddWithValue("@Comment", C.SenderComment);
                CMDStatus.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                con.Open();
                Result = CMDStatus.ExecuteNonQuery().ToString();
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
        public string ChangeFINSheetStatus(int csid)
        {
            string Result = string.Empty;
            try
            {
                SqlCommand CMDStatus = new SqlCommand("SP_CostSheetMaster", con);
                CMDStatus.CommandType = CommandType.StoredProcedure;
                CMDStatus.Parameters.AddWithValue("@SP_Type", 19);
                CMDStatus.Parameters.AddWithValue("@PK_Cs_Id", csid);
                con.Open();
                Result = CMDStatus.ExecuteNonQuery().ToString();
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

        public string ChangeFINApproval(int csid)
        {
            string Result = string.Empty;
            try
            {
                SqlCommand CMDStatus = new SqlCommand("SP_CostSheetMaster", con);
                CMDStatus.CommandType = CommandType.StoredProcedure;
                CMDStatus.Parameters.AddWithValue("@SP_Type", 20);
                CMDStatus.Parameters.AddWithValue("@PK_Cs_Id", csid);
                con.Open();
                Result = CMDStatus.ExecuteNonQuery().ToString();
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