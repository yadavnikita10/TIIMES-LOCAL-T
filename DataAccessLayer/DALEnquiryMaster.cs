using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using System.Web.Mvc;
using System.Web;
using System.Globalization;

namespace TuvVision.DataAccessLayer
{
    public class DALEnquiryMaster
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public List<EnquiryMaster> GetEnquiryListDashBoard(string Type)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                if (Type == "NoOfEnquiryInLast15Days")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 29);

                }
                else if (Type == "NoOfEnquiryRegret")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 30);
                }
                else if (Type == "NoOfEnquiryRegretInLast15Days")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 31);
                }
                else
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 1);
                }

                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new EnquiryMaster
                           {
                               Count = DTEMDashBoard.Rows.Count,

                               /*
                                * EQ_ID = Convert.ToInt32(dr["EQ_ID"]),

                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               CompanyName = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               Owner = Convert.ToString(dr["OwnerName"]),
                               CreatedDate = Convert.ToDateTime(dr["DateOpened"]),
                               Type = Convert.ToString(dr["ServiceType"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               SubServiceType = Convert.ToString(dr["ServiceType"]),
                               EstClose = Convert.ToDateTime(dr["EstClose"]),
                               Opendate = Convert.ToDateTime(dr["DateOpened"]),
                               RegretStatus = Convert.ToString(dr["RegretStatus"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               CreatedBy = Convert.ToString(dr["OwnerName"]),
							   DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               Icurrency = Convert.ToString(dr["Icurrency"]),
                               EstimatedAmount = Convert.ToDecimal(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),

                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                               */

                               RefDate = Convert.ToString(dr["refDate"]),
                               EndCustomer = Convert.ToString(dr["EndCustomer"]),
                               Notes = Convert.ToString(dr["Notes"]),
                               NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                               //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                               ContactNo = Convert.ToString(dr["ContactNo"]),
                               ContactName = Convert.ToString(dr["ContactName"]),
                               ARC = Convert.ToString(dr["ARC"]),
                               ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               ProjectName = Convert.ToString(dr["ProjectName"]),
                               Company_Name = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               OpendateS = Convert.ToString(dr["DateOpened"]),
                               Source = Convert.ToString(dr["source"]),
                               EstCloseS = Convert.ToString(dr["EstClose"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                               Status = Convert.ToString(dr["EStatus"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                               Owner = Convert.ToString(dr["OwnerName"]),
                               //DEstimatedAmount ="", //Convert.ToString(dr["DEstimatedAmount"]),
                               //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),
                               DEstimatedAmount = Convert.ToString(dr["TEstimatedAmount"]),
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               //  TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               Intlocation = Convert.ToString(dr["Intlocation"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               Icurrency = Convert.ToString(dr["ICurrency"]),
                               LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),
                               OrderType = Convert.ToString(dr["OrderType"]),
                               RegretStatus = Convert.ToString(dr["EStatus"]),
                               CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                               Quotation = Convert.ToString(dr["Quotation"]),
                               JobNumber = Convert.ToString(dr["JobNumber"]),
                               BudgetoryText = Convert.ToString(dr["BudgetoryText"]),
                               QCreatedDate = Convert.ToString(dr["QCreatedDate"]),
                               InspectionLocation = Convert.ToString(dr["InspectionLocation"]),



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

        public List<EnquiryMaster> GetRegEnquiryListDashBoard()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 38);

                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new EnquiryMaster
                           {
                               Count = DTEMDashBoard.Rows.Count,
                               RefDate = Convert.ToString(dr["refDate"]),
                               EndCustomer = Convert.ToString(dr["EndCustomer"]),
                               Notes = Convert.ToString(dr["Notes"]),
                               NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                               //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                               ContactNo = Convert.ToString(dr["ContactNo"]),
                               ContactName = Convert.ToString(dr["ContactName"]),
                               ARC = Convert.ToString(dr["ARC"]),
                               ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               ProjectName = Convert.ToString(dr["ProjectName"]),
                               Company_Name = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               OpendateS = Convert.ToString(dr["DateOpened"]),
                               Source = Convert.ToString(dr["source"]),
                               EstCloseS = Convert.ToString(dr["EstClose"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                               Status = Convert.ToString(dr["status"]),
                               RegretActionTaken = Convert.ToString(dr["EnquiryRegretActionTaken"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                               Owner = Convert.ToString(dr["OwnerName"]),//added by nikita 09082023
                               //DEstimatedAmount = "", //Convert.ToString(dr["DEstimatedAmount"]),commenetd by nikita
                               //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),commenetd by nikita
                               DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),//added by nikita 09082023
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),//added by nikita 09082023
                               TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               Intlocation = Convert.ToString(dr["Intlocation"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               Icurrency = Convert.ToString(dr["ICurrency"]),
                               LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),
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

        public List<EnquiryMaster> GetEnquiryListDashBoardForMIS(string Type)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                if (Type == "NoOfEnquiryInLast15Days")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 29);
                }
                else if (Type == "NoOfEnquiryRegret")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 30);
                }
                else if (Type == "NoOfEnquiryRegretInLast15Days")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 31);
                }
                else
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 46);
                }

                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new EnquiryMaster
                           {
                               Count = DTEMDashBoard.Rows.Count,

                               /*
                                * EQ_ID = Convert.ToInt32(dr["EQ_ID"]),

                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               CompanyName = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               Owner = Convert.ToString(dr["OwnerName"]),
                               CreatedDate = Convert.ToDateTime(dr["DateOpened"]),
                               Type = Convert.ToString(dr["ServiceType"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               SubServiceType = Convert.ToString(dr["ServiceType"]),
                               EstClose = Convert.ToDateTime(dr["EstClose"]),
                               Opendate = Convert.ToDateTime(dr["DateOpened"]),
                               RegretStatus = Convert.ToString(dr["RegretStatus"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               CreatedBy = Convert.ToString(dr["OwnerName"]),
							   DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               Icurrency = Convert.ToString(dr["Icurrency"]),
                               EstimatedAmount = Convert.ToDecimal(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),

                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                               */

                               ContactNo = Convert.ToString(dr["ContactNo"]),
                               ContactName = Convert.ToString(dr["ContactName"]),
                               ARC = Convert.ToString(dr["ARC"]),
                               ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               ProjectName = Convert.ToString(dr["ProjectName"]),
                               Company_Name = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               OpendateS = Convert.ToString(dr["DateOpened"]),
                               Source = Convert.ToString(dr["source"]),
                               EstCloseS = Convert.ToString(dr["EstClose"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               SubServiceType = Convert.ToString(dr["ServiceType"]),
                               Status = Convert.ToString(dr["status"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               CreatedBy = Convert.ToString(dr["OwnerName"]),
                               DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               Intlocation = Convert.ToString(dr["Intlocation"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               Icurrency = Convert.ToString(dr["ICurrency"]),
                               LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),
                               RegretStatus = Convert.ToString(dr["EStatus"]),
                               CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),

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


        //Searching Record By From Date And To Date wise, Code By Manoj Sharma 17 Dec 2019
        public DataTable GetDataByDateWise(string FromDate, string ToDate)
        {
            DataTable DTEMDashBoard = new DataTable();
            try
            {
                string _date1 = string.Empty;
                string _date2 = string.Empty;
                string[] FromDt = FromDate.Split('/');
                string[] ToDt = ToDate.Split('/');
                _date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                _date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];

                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 21);
                //CMDGetEnquriy.Parameters.AddWithValue("@DateFrom", _date1);
                //CMDGetEnquriy.Parameters.AddWithValue("@DateTo", _date2);

                CMDGetEnquriy.Parameters.AddWithValue("@Fromdate", DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                CMDGetEnquriy.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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



        //Searching Record By From Date And To Date wise, Code By Manoj Sharma 17 Dec 2019
        public DataTable GetRegDataByDateWise(string FromDate, string ToDate)
        {
            DataTable DTEMDashBoard = new DataTable();
            try
            {
                string _date1 = string.Empty;
                string _date2 = string.Empty;
                string[] FromDt = FromDate.Split('/');
                string[] ToDt = ToDate.Split('/');
                _date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                _date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];

                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 39);
                CMDGetEnquriy.Parameters.AddWithValue("@DateFrom", _date1);
                CMDGetEnquriy.Parameters.AddWithValue("@DateTo", _date2);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataSet GetAllddlLst()//Binding All Dropdownlist
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 2);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }
        public DataSet GetEditAllddlLst()
        {
            string CompanyName = Convert.ToString(System.Web.HttpContext.Current.Session["CompanyName"]);
            DataSet DSGetDdlList = new DataSet();
            try
            {
                if (CompanyName != null && CompanyName != "")
                {
                    SqlCommand CMDGetDdlLst = new SqlCommand("SP_EnquiryMaster", con);
                    CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                    CMDGetDdlLst.CommandTimeout = 900000000;
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 20);
                    CMDGetDdlLst.Parameters.AddWithValue("@CompanyName", CompanyName.Trim());
                    CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                    SDAGetAllDdlLst.Fill(DSGetDdlList);
                }
                else
                {
                    SqlCommand CMDGetDdlLst = new SqlCommand("SP_EnquiryMaster", con);
                    CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                    CMDGetDdlLst.CommandTimeout = 900000000;
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 2);

                    CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                    SDAGetAllDdlLst.Fill(DSGetDdlList);
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }



        public DataSet GetEmployeeTypeLst()
        {

            DataSet DSGetDdlList = new DataSet();
            try
            {

                SqlCommand CMDGetDdlLst = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 27);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }
        public string InsertAndUpdateEnquiry(EnquiryMaster EM, string Cities, string Countries, string IPath)
        {
            string Result = string.Empty;
            var roleid = Convert.ToString(System.Web.HttpContext.Current.Session["RoleID"]);
            con.Open();
            try
            {
                if (EM.EQ_ID != 0)
                {
                    SqlCommand CMDUpdateEnquiry = new SqlCommand("SP_EnquiryMaster", con);//Updating Record In Database
                    CMDUpdateEnquiry.CommandType = CommandType.StoredProcedure;
                    CMDUpdateEnquiry.Parameters.AddWithValue("@SP_Type", 5);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@EQ_ID", EM.EQ_ID);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@EnquiryDescription", EM.EnquiryDescription);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@CompanyName", EM.CompanyName);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@EndCustomer", EM.EndCustomer);

                    CMDUpdateEnquiry.Parameters.AddWithValue("@EnquiryReferenceNo", EM.EnquiryReferenceNo);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@ProjectType", EM.ProjectType);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@EstimatedAmount", EM.EstimatedAmount);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@Branch", EM.Branch);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@Type", EM.Type);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@Notes", EM.Notes);
                    //CMDUpdateEnquiry.Parameters.AddWithValue("@RegretReason", EM.RegretReason);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@RegretReason", EM.RegretReason);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@RegretActionTakenId", EM.RegretActionTakenId);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@RegretReasonDescription", EM.RegretReasonDescription);
                    // CMDUpdateEnquiry.Parameters.AddWithValue("@RegretStatus", EM.RegretStatus);
                    if (EM.RegretReason != null && EM.RegretReason != "")
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@RegretStatus", "Regret");
                    }
                    //Adding New Fields Start 07 June 2019
                    if (EM.ContactName != "--Select Contact--" && EM.ContactName != null)
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@ContactName", EM.ContactName);
                    }
                    if (EM.Source != "--Select Source Type--" && EM.Source != null)
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@Source", EM.Source);
                    }
                    if (EM.InspectionLocation != "--Select Inspection Location" && EM.InspectionLocation != null)
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@InspectionLocation", EM.InspectionLocation);//Edit
                    }
                    if (Cities != null && Cities != "")
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@City", Cities);
                    }

                    if (Countries != null && Countries != "")
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@Country", Countries);
                    }
                    if (IPath != null && IPath != "")
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@DocumentAttached", IPath);
                    }

                    if (EM.SubType != 0 && EM.SubType != null)
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@Ped", EM.SubType);
                    }

                    if (EM.OtherType != 0 && EM.OtherType != null)
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@Others", EM.OtherType);
                    }

                    if (EM.EnergyType != 0 && EM.EnergyType != null)
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@Energy", EM.EnergyType);
                    }
                    //Adding New Fields End 07 June 2019
                    CMDUpdateEnquiry.Parameters.AddWithValue("@PortfolioType", EM.PortfolioType);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@SubServiceType", EM.SubServiceType);

                    CMDUpdateEnquiry.Parameters.AddWithValue("@DEstimatedAmount", EM.DEstimatedAmount);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@Dcurrency ", EM.Dcurrency);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@DExchangeRate ", EM.DExchangeRate);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@DTotalAmount", EM.DTotalAmount);

                    CMDUpdateEnquiry.Parameters.AddWithValue("@IEstimatedAmount", EM.IEstimatedAmount);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@Icurrency ", EM.Icurrency);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@IExchangeRate ", EM.IExchangeRate);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@ITotalAmount ", EM.ITotalAmount);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@chkArc ", EM.chkArc);

                    CMDUpdateEnquiry.Parameters.AddWithValue("@Legalreview", EM.LegalReview);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@Legalcomment", EM.Legalcomment);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@QuotationReview", EM.Quotationviewed);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@Budgetary", EM.Budgetary);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@cmp_id", EM.CMP_ID);
                    if (roleid == "60") //added by nikita on 18062024
                    {
                        CMDUpdateEnquiry.Parameters.AddWithValue("@LegalApprovedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                    }


                    CMDUpdateEnquiry.Parameters.AddWithValue("@RefDate", DateTime.ParseExact(EM.RefDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    CMDUpdateEnquiry.Parameters.AddWithValue("@EstClose", DateTime.ParseExact(EM.EstClose, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    CMDUpdateEnquiry.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@LeadGivenBy", EM.LeadGivenBy);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@NotesbyLead", EM.NotesbyLeads);

                    CMDUpdateEnquiry.Parameters.AddWithValue("@CheckListId", EM.CheckListId);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@CheckListDescription", EM.CheckListDescription);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@ENQDuplicate", EM.ENQDuplicate);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@EnqOrderType", EM.OrderType);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@Budgetary_PKQTID", EM.BudgetaryPk_QTid);


                    CMDUpdateEnquiry.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(CMDUpdateEnquiry.Parameters["@CompanyName"].Value);
                }
                else
                {
                    SqlCommand CMDInsertEnquiry = new SqlCommand("SP_EnquiryMaster", con);//Saving Record in Database
                    CMDInsertEnquiry.CommandType = CommandType.StoredProcedure;
                    CMDInsertEnquiry.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertEnquiry.Parameters.AddWithValue("@EnquiryDescription", EM.EnquiryDescription);
                    CMDInsertEnquiry.Parameters.AddWithValue("@CompanyName", EM.CompanyName);
                    CMDInsertEnquiry.Parameters.AddWithValue("@EndCustomer", EM.EndCustomer);

                    CMDInsertEnquiry.Parameters.AddWithValue("@EnquiryReferenceNo", EM.EnquiryReferenceNo);
                    CMDInsertEnquiry.Parameters.AddWithValue("@ProjectType", EM.ProjectType);
                    CMDInsertEnquiry.Parameters.AddWithValue("@EstimatedAmount", EM.EstimatedAmount);
                    CMDInsertEnquiry.Parameters.AddWithValue("@Branch", EM.Branch);
                    CMDInsertEnquiry.Parameters.AddWithValue("@Type", EM.Type);
                    CMDInsertEnquiry.Parameters.AddWithValue("@Notes", EM.Notes);
                    CMDInsertEnquiry.Parameters.AddWithValue("@RegretReason", EM.RegretId);
                    CMDInsertEnquiry.Parameters.AddWithValue("@RegretActionTakenId", EM.RegretActionTakenId);
                    if (EM.RegretReason != null && EM.RegretReason != "")
                    {
                        CMDInsertEnquiry.Parameters.AddWithValue("@RegretStatus", "Regret");
                    }
                    //Adding New Fields Start 07 June 2019
                    if (EM.ContactName != "--Select Contact--")
                    {
                        CMDInsertEnquiry.Parameters.AddWithValue("@ContactName", EM.ContactName);
                    }
                    if (EM.Source != "--Select Source Type--")
                    {
                        CMDInsertEnquiry.Parameters.AddWithValue("@Source", EM.Source);
                    }
                    if (EM.InspectionLocation != "--Select Inspection Location")
                    {
                        CMDInsertEnquiry.Parameters.AddWithValue("@InspectionLocation", EM.InspectionLocation);//Add
                    }
                    if (Cities != "--Select City Type--")
                    {
                        CMDInsertEnquiry.Parameters.AddWithValue("@City", Cities);
                    }

                    if (Countries != "--Select Country Type--")
                    {
                        CMDInsertEnquiry.Parameters.AddWithValue("@Country", Countries);
                    }

                    if (IPath != null && IPath != "")
                    {
                        CMDInsertEnquiry.Parameters.AddWithValue("@DocumentAttached", IPath);
                    }

                    if (EM.SubType != 0 && EM.SubType != null)
                    {
                        CMDInsertEnquiry.Parameters.AddWithValue("@Ped", EM.SubType);
                    }

                    if (EM.OtherType != 0 && EM.OtherType != null)
                    {
                        CMDInsertEnquiry.Parameters.AddWithValue("@Others", EM.OtherType);
                    }

                    if (EM.EnergyType != 0 && EM.EnergyType != null)
                    {
                        CMDInsertEnquiry.Parameters.AddWithValue("@Energy", EM.EnergyType);
                    }
                    //Adding New Fields End 07 June 2019
                    CMDInsertEnquiry.Parameters.Add("@EnquiryIDs", SqlDbType.VarChar, 30);
                    // CMDInsertEnquiry.Parameters["@EnquiryIDs"].Direction = ParameterDirection.Output;
                    CMDInsertEnquiry.Parameters.AddWithValue("@PortfolioType", EM.PortfolioType);
                    CMDInsertEnquiry.Parameters.AddWithValue("@SubServiceType", EM.SubServiceType);

                    CMDInsertEnquiry.Parameters.AddWithValue("@DEstimatedAmount", EM.DEstimatedAmount);
                    CMDInsertEnquiry.Parameters.AddWithValue("@Dcurrency ", EM.Dcurrency);
                    CMDInsertEnquiry.Parameters.AddWithValue("@DExchangeRate ", EM.DExchangeRate);
                    CMDInsertEnquiry.Parameters.AddWithValue("@DTotalAmount", EM.DTotalAmount);

                    CMDInsertEnquiry.Parameters.AddWithValue("@IEstimatedAmount", EM.IEstimatedAmount);
                    CMDInsertEnquiry.Parameters.AddWithValue("@Icurrency ", EM.Icurrency);
                    CMDInsertEnquiry.Parameters.AddWithValue("@IExchangeRate ", EM.IExchangeRate);
                    CMDInsertEnquiry.Parameters.AddWithValue("@ITotalAmount ", EM.ITotalAmount);
                    CMDInsertEnquiry.Parameters.AddWithValue("@chkArc ", EM.chkArc);
                    CMDInsertEnquiry.Parameters.AddWithValue("@RefDate", DateTime.ParseExact(EM.RefDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    CMDInsertEnquiry.Parameters.AddWithValue("@EstClose", DateTime.ParseExact(EM.EstClose, "dd/MM/yyyy", CultureInfo.InvariantCulture));




                    CMDInsertEnquiry.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);

                    CMDInsertEnquiry.Parameters.AddWithValue("@LeadGivenBy", EM.LeadGivenBy);
                    CMDInsertEnquiry.Parameters.AddWithValue("@NotesbyLead", EM.NotesbyLeads);

                    CMDInsertEnquiry.Parameters.AddWithValue("@CheckListId", EM.CheckListId);
                    CMDInsertEnquiry.Parameters.AddWithValue("@CheckListDescription", EM.CheckListDescription);
                    CMDInsertEnquiry.Parameters.AddWithValue("@Legalreview", EM.LegalReview);
                    CMDInsertEnquiry.Parameters.AddWithValue("@Legalcomment", EM.Legalcomment);
                    CMDInsertEnquiry.Parameters.AddWithValue("@QuotationReview", EM.Quotationviewed);
                    CMDInsertEnquiry.Parameters.AddWithValue("@Budgetary", EM.Budgetary);
                    CMDInsertEnquiry.Parameters.AddWithValue("@cmp_id", EM.CMP_ID);
                    CMDInsertEnquiry.Parameters.AddWithValue("@EnqOrderType", EM.OrderType);
                    CMDInsertEnquiry.Parameters["@EnquiryIDs"].Direction = ParameterDirection.Output;
                    CMDInsertEnquiry.ExecuteNonQuery().ToString();//
                    Result = Convert.ToString(CMDInsertEnquiry.Parameters["@CompanyName"].Value);
                    System.Web.HttpContext.Current.Session["CompanyName"] = Result;
                    string EnquiryID = Convert.ToString(CMDInsertEnquiry.Parameters["@EnquiryIDs"].Value);
                    System.Web.HttpContext.Current.Session["EnquiryIDs"] = EnquiryID;
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
        public DataTable GetEnquiryDetals(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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
        public DataTable GetJobCount(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 33);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        //public string DeleteEnquiry()
        //{
        //    string Result = string.Empty;
        //    con.Open();
        //    try
        //    {

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
        //    return Result;
        //}

        public int DeleteEnquiry(int? EQ_ID, string reason)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDCompanyDelete = new SqlCommand("SP_EnquiryMaster", con);
                CMDCompanyDelete.CommandType = CommandType.StoredProcedure;
                CMDCompanyDelete.CommandTimeout = 100000;
                CMDCompanyDelete.Parameters.AddWithValue("@SP_Type", 14);
                CMDCompanyDelete.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDCompanyDelete.Parameters.AddWithValue("@DeleteReason", reason);
                CMDCompanyDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDCompanyDelete.ExecuteNonQuery();
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
        public DataTable GetCompanyName(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 6);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
                //CMDSearchNameCode.Parameters.AddWithValue("", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public DataTable GetCompanyNameTest(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", "6Test");
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
                //CMDSearchNameCode.Parameters.AddWithValue("", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public DataSet GetCompanyNameDetls(string CompanyNames)
        {
            DataSet DSGetComapnyDtls = new DataSet();
            try
            {
                SqlCommand CMDGetCompanyDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetCompanyDtls.CommandType = CommandType.StoredProcedure;
                CMDGetCompanyDtls.Parameters.AddWithValue("@SP_Type", 7);
                CMDGetCompanyDtls.Parameters.AddWithValue("@CompanyName", CompanyNames);
                SqlDataAdapter SDAGetCompanyDtls = new SqlDataAdapter(CMDGetCompanyDtls);
                SDAGetCompanyDtls.Fill(DSGetComapnyDtls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetComapnyDtls.Dispose();
            }
            return DSGetComapnyDtls;
        }

        public List<NameCode> BindEditPedType(int PK_ID)//Ped Type DDL bind
        {
            SqlCommand CMDPedType = new SqlCommand("SP_EnquiryMaster", con);
            DataTable DTPedType = new DataTable();
            List<NameCode> lstPedType = new List<NameCode>();
            try
            {
                CMDPedType.CommandType = CommandType.StoredProcedure;
                CMDPedType.Parameters.AddWithValue("@SP_Type", 11);
                CMDPedType.Parameters.AddWithValue("@PK_ID", PK_ID);
                SqlDataAdapter SDAGetPedType = new SqlDataAdapter(CMDPedType);
                SDAGetPedType.Fill(DTPedType);
                if (DTPedType.Rows.Count > 0)
                {
                    lstPedType = (from n in DTPedType.AsEnumerable()
                                  select new NameCode()
                                  {
                                      Code = n.Field<Int32>(DTPedType.Columns["PK_ID"].ToString()),
                                      Name = n.Field<string>(DTPedType.Columns["OthersSubName"].ToString())
                                  }).ToList();
                }
                return lstPedType;
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                DTPedType.Dispose();
            }
            return lstPedType;
        }
        public List<NameCode> BindEditOtherType(int PK_ID)//Ped Type DDL bind
        {
            SqlCommand CMDPedType = new SqlCommand("SP_EnquiryMaster", con);
            DataTable DTPedType = new DataTable();
            List<NameCode> lstPedType = new List<NameCode>();
            try
            {
                CMDPedType.CommandType = CommandType.StoredProcedure;
                CMDPedType.Parameters.AddWithValue("@SP_Type", 12);
                CMDPedType.Parameters.AddWithValue("@PK_ID", PK_ID);
                SqlDataAdapter SDAGetPedType = new SqlDataAdapter(CMDPedType);
                SDAGetPedType.Fill(DTPedType);
                if (DTPedType.Rows.Count > 0)
                {
                    lstPedType = (from n in DTPedType.AsEnumerable()
                                  select new NameCode()
                                  {
                                      Code = n.Field<Int32>(DTPedType.Columns["PK_ID"].ToString()),
                                      Name = n.Field<string>(DTPedType.Columns["OthersSubName"].ToString())
                                  }).ToList();
                }
                return lstPedType;
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                DTPedType.Dispose();
            }
            return lstPedType;
        }
        public List<NameCode> BindEditEnergyType(int PK_ID)//Ped Type DDL bind
        {
            SqlCommand CMDPedType = new SqlCommand("SP_EnquiryMaster", con);
            DataTable DTPedType = new DataTable();
            List<NameCode> lstPedType = new List<NameCode>();
            try
            {
                CMDPedType.CommandType = CommandType.StoredProcedure;
                CMDPedType.Parameters.AddWithValue("@SP_Type", 13);
                CMDPedType.Parameters.AddWithValue("@PK_ID", PK_ID);
                SqlDataAdapter SDAGetPedType = new SqlDataAdapter(CMDPedType);
                SDAGetPedType.Fill(DTPedType);
                if (DTPedType.Rows.Count > 0)
                {
                    lstPedType = (from n in DTPedType.AsEnumerable()
                                  select new NameCode()
                                  {
                                      Code = n.Field<Int32>(DTPedType.Columns["PK_ID"].ToString()),
                                      Name = n.Field<string>(DTPedType.Columns["OthersSubName"].ToString())
                                  }).ToList();
                }
                return lstPedType;
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                DTPedType.Dispose();
            }
            return lstPedType;
        }

        //Adding Time Binding
        public List<NameCode> BindPedType(int FK_ID)//Ped Type DDL bind
        {
            SqlCommand CMDPedType = new SqlCommand("SP_EnquiryMaster", con);
            DataTable DTPedType = new DataTable();
            List<NameCode> lstPedType = new List<NameCode>();
            try
            {
                CMDPedType.CommandType = CommandType.StoredProcedure;
                CMDPedType.Parameters.AddWithValue("@SP_Type", 8);
                CMDPedType.Parameters.AddWithValue("@FK_ID", FK_ID);
                SqlDataAdapter SDAGetPedType = new SqlDataAdapter(CMDPedType);
                SDAGetPedType.Fill(DTPedType);
                if (DTPedType.Rows.Count > 0)
                {
                    lstPedType = (from n in DTPedType.AsEnumerable()
                                  select new NameCode()
                                  {
                                      Code = n.Field<Int32>(DTPedType.Columns["PK_ID"].ToString()),
                                      Name = n.Field<string>(DTPedType.Columns["OthersSubName"].ToString())
                                  }).ToList();
                }
                return lstPedType;
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                DTPedType.Dispose();
            }
            return lstPedType;
        }

        public List<NameCode> BindCity(string FK_ILID)//City Type DDL bind
        {
            SqlCommand CMDCity = new SqlCommand("SP_EnquiryMaster", con);
            DataTable DTCity = new DataTable();
            List<NameCode> lstCity = new List<NameCode>();
            try
            {
                CMDCity.CommandType = CommandType.StoredProcedure;
                CMDCity.Parameters.AddWithValue("@SP_Type", 9);
                CMDCity.Parameters.AddWithValue("@FK_ID", FK_ILID);
                SqlDataAdapter SDAGetCity = new SqlDataAdapter(CMDCity);
                SDAGetCity.Fill(DTCity);
                if (DTCity.Rows.Count > 0)
                {
                    lstCity = (from n in DTCity.AsEnumerable()
                               select new NameCode()
                               {
                                   Code = n.Field<Int32>(DTCity.Columns["PK_ID"].ToString()),
                                   Name = n.Field<string>(DTCity.Columns["CityName"].ToString())
                               }).ToList();
                }
                return lstCity;
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                DTCity.Dispose();
            }
            return lstCity;
        }
        public List<NameCode> BindCountry(string FK_ILID)//Country Type DDL bind
        {
            SqlCommand CMDCountry = new SqlCommand("SP_EnquiryMaster", con);
            DataTable DTCountry = new DataTable();
            List<NameCode> lstCountry = new List<NameCode>();
            try
            {
                CMDCountry.CommandType = CommandType.StoredProcedure;
                CMDCountry.Parameters.AddWithValue("@SP_Type", 10);
                CMDCountry.Parameters.AddWithValue("@FK_ID", FK_ILID);
                SqlDataAdapter SDAGetCountry = new SqlDataAdapter(CMDCountry);
                SDAGetCountry.Fill(DTCountry);
                if (DTCountry.Rows.Count > 0)
                {
                    lstCountry = (from n in DTCountry.AsEnumerable()
                                  select new NameCode()
                                  {
                                      Code = n.Field<Int32>(DTCountry.Columns["PK_ID"].ToString()),
                                      Name = n.Field<string>(DTCountry.Columns["CountryName"].ToString())
                                  }).ToList();
                }
                return lstCountry;
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                DTCountry.Dispose();
            }
            return lstCountry;
        }
        public DataSet GetContactDdlList()//Get All DropDownlist Contact Details in Enquiry Modules
        {
            DataSet DSGetddlLst = new DataSet();



            string CompanyName1 = Convert.ToString(System.Web.HttpContext.Current.Session["CompanyNames"]);

            string CompanyName = Convert.ToString(System.Web.HttpContext.Current.Session["CompanyNameInsertInContact"]);
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CompanyMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;

                if (CompanyName != null && CompanyName != "")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@CompanyName", CompanyName);
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 33);
                }
                else
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 7);
                }

                //CMDGetDdlLst.Parameters.AddWithValue("",Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                CMDGetDdlLst.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                SDAGetDdlLst.Fill(DSGetddlLst);
                string id = CMDGetDdlLst.Parameters["@ReturnCompanyId"].Value.ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }
        public string InsertUpdateContact(EnquiryMaster ECM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDInsertUpdateContact = new SqlCommand("SP_CompanyMaster", con);
                CMDInsertUpdateContact.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateContact.Parameters.AddWithValue("@SP_Type", 5);
                CMDInsertUpdateContact.Parameters.AddWithValue("@ContactName", ECM.ContactNames);
                CMDInsertUpdateContact.Parameters.AddWithValue("@CompanyName", ECM.CompanyName);
                //  CMDInsertUpdateContact.Parameters.AddWithValue("@FK_CMP_ID", ECM.ContactCompanyName);
                CMDInsertUpdateContact.Parameters.AddWithValue("@FK_CMP_ID", ECM.CMP_ID);
                CMDInsertUpdateContact.Parameters.AddWithValue("@Title", ECM.TitleName);
                CMDInsertUpdateContact.Parameters.AddWithValue("@HomePhone", ECM.HomePhone);
                CMDInsertUpdateContact.Parameters.AddWithValue("@Mobile", ECM.Mobile);
                CMDInsertUpdateContact.Parameters.AddWithValue("@Address", ECM.Address);
                CMDInsertUpdateContact.Parameters.AddWithValue("@FaxNo", ECM.Fax_No);
                CMDInsertUpdateContact.Parameters.AddWithValue("@PanNumber", ECM.Pan_No);
                CMDInsertUpdateContact.Parameters.AddWithValue("@IsMainContact", ECM.IsMainContact);
                CMDInsertUpdateContact.Parameters.AddWithValue("@ContactStatus", ECM.ContactStatus);
                CMDInsertUpdateContact.Parameters.AddWithValue("@Email", ECM.Email);


                CMDInsertUpdateContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                // Result = CMDInsertUpdateContact.ExecuteNonQuery().ToString();
                CMDInsertUpdateContact.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                Result = CMDInsertUpdateContact.ExecuteNonQuery().ToString();
                string id = CMDInsertUpdateContact.Parameters["@ReturnCompanyId"].Value.ToString();
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

        //QuotationMaster List Binding
        public List<QuotationMaster> QuotaionMastertDashBoard(int? EQ_ID)// Binding QuotationMAster DashBoard of Master Page 
        {
            DataTable DTQMDashBoard = new DataTable();
            List<QuotationMaster> lstQuotationMastDashB = new List<QuotationMaster>();
            try
            {
                SqlCommand CMDQMDashBoard = new SqlCommand("SP_QuotationMaster", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 1);
                CMDQMDashBoard.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMDashBoard);
                SDAGetEnquiry.Fill(DTQMDashBoard);
                if (DTQMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTQMDashBoard.Rows)
                    {
                        lstQuotationMastDashB.Add(
                           new QuotationMaster
                           {
                               PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                               QuotationNumber = Convert.ToString(dr["QuotationNumber"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               Quotation_Description = Convert.ToString(dr["Description"]),
                               Reference = Convert.ToString(dr["Reference"]),
                               Enquiry = Convert.ToString(dr["Enquiry"]),
                               ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                               ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                               StatusType = Convert.ToString(dr["Status"]),
                               QTType = Convert.ToString(dr["QuotationType"]),
                               InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                               DApprovalStatus = Convert.ToString(dr["DCostSheetApproveStatus"]),
                               IApprovalStatus = Convert.ToString(dr["ICostSheetApproveStatus"]),


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
                DTQMDashBoard.Dispose();
            }
            return lstQuotationMastDashB;
        }

        //**********************Manoj Added code for Uploading File in database
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int EQ_ID, string type)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_EQID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("EnquiryNumber", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("AttachmentType", typeof(string)));


                foreach (var item in lstFileUploaded)
                {
                    string fileNameWithID1 = Convert.ToString(EQ_ID) + "_" + item.FileName;
                    DTUploadFile.Rows.Add(EQ_ID, null, fileNameWithID1, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent, null);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_EnquiryUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListEnquiryUploadedFile", DTUploadFile);
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
        //added by nikita on 25062024
        public string InsertFileLegalAttachment(List<FileDetails> lstFileUploaded, int EQ_ID, string type)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_EQID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("EnquiryNumber", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("AttachmentType", typeof(string)));

                foreach (var item in lstFileUploaded)
                {
                    string fileNameWithID = Convert.ToString(EQ_ID) + "_" + item.FileName;
                    DTUploadFile.Rows.Add(EQ_ID, null, fileNameWithID, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent, type);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_EnquiryUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListEnquiryUploadedFile", DTUploadFile);
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


        public DataTable EditUploadedFile(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_EnquiryUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_EQID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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
        //Delete Uploaded File From Database Code Added by Manoj Sharma 7 March 2020
        public string DeleteUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_EnquiryUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@FileID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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
        public DataTable GetFileExt(string FileID)
        {
            DataTable DTGetFileExtenstion = new DataTable();
            try
            {
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_EnquiryUploadedFile", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetExtenstion.Parameters.AddWithValue("@FileID", FileID);
                CMDGetExtenstion.Parameters.AddWithValue("@CreatedBy", UserIDs);
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


        //****************************************************************Ending Code Related File Uploaded*******************

        #region Delete Code By Rahul 
        public int DeleteEnquiry(int? EQ_ID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDCompanyDelete = new SqlCommand("SP_EnquiryMaster", con);
                CMDCompanyDelete.CommandType = CommandType.StoredProcedure;
                CMDCompanyDelete.CommandTimeout = 100000;
                CMDCompanyDelete.Parameters.AddWithValue("@SP_Type", 14);
                CMDCompanyDelete.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                //CMDCompanyDelete.Parameters.AddWithValue("@Status", 0);
                CMDCompanyDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDCompanyDelete.ExecuteNonQuery();
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
        #endregion

        #region Contact List Data 

        //public List<CompanyMaster> GetInspectorList(string CompanyName)// Binding Sales Masters DashBoard of Master Page 
        //{
        //    DataTable DTEMDashBoard = new DataTable();
        //    List<CompanyMaster> lstEnquiryDashB = new List<CompanyMaster>();
        //    try
        //    {
        //        SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
        //        CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
        //        CMDGetEnquriy.CommandTimeout = 1000000;
        //        CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 19);
        //        CMDGetEnquriy.Parameters.AddWithValue("@CompanyName", CompanyName);
        //        SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
        //        SDAGetEnquiry.Fill(DTEMDashBoard);
        //        if (DTEMDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in DTEMDashBoard.Rows)
        //            {
        //                lstEnquiryDashB.Add(
        //                   new CompanyMaster
        //                   {
        //                       ContactName = Convert.ToString(dr["ContactName"]),
        //                       PK_ContID = Convert.ToInt32(dr["PK_ContID"]),

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
        public DataTable GetInspectorList(string CompanyName)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CompanyMaster> lstEnquiryDashB = new List<CompanyMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 19);
                CMDGetEnquriy.Parameters.AddWithValue("@CompanyName", CompanyName);
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
        #endregion


        public DataTable GetProfileList(int Project)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CompanyMaster> lstEnquiryDashB = new List<CompanyMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 22);
                CMDGetEnquriy.Parameters.AddWithValue("@FK_ID", Project);
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

        public string GetServiceCode(int Project)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            string code = string.Empty;
            List<CompanyMaster> lstEnquiryDashB = new List<CompanyMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 24);
                CMDGetEnquriy.Parameters.AddWithValue("@FK_ID", Project);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                code = DTEMDashBoard.Rows[0]["Code"].ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return code;
        }

        public string GetProfileCode(int Project)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            string code = string.Empty;
            List<CompanyMaster> lstEnquiryDashB = new List<CompanyMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 25);
                CMDGetEnquriy.Parameters.AddWithValue("@FK_ID", Project);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                code = DTEMDashBoard.Rows[0]["Code"].ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return code;
        }

        public DataTable GetSubServiceList(int PortFolio)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CompanyMaster> lstEnquiryDashB = new List<CompanyMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 23);
                CMDGetEnquriy.Parameters.AddWithValue("@FK_ID", PortFolio);
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

        public DataTable GetCompany(string companyname)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 28);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@CompanyName", companyname);

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

        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_EnquiryUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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

        public DataSet GetDashboardData()
        {
            DataSet DTScripName = new DataSet();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_Dashboard", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                //CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 1);
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 3);

                CMDSearchNameCode.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataSet GetIndividualPerformance()
        {
            DataSet DTScripName = new DataSet();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_IndividualPerformance", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                //CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 1);

                CMDSearchNameCode.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable GetCompAddress(string compName)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 32);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@CompanyName", compName);

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

        public string InsertUpdateOrderType(EnquiryMaster QM) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_EnquiryMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "34");

                cmd.Parameters.AddWithValue("@OrderType", QM.OrderType);
                cmd.Parameters.AddWithValue("@OrderRate", QM.OrderRate);
                cmd.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", QM.Estimate_ManDays_ManMonth);
                cmd.Parameters.AddWithValue("@Estimate_ManMonth", QM.Estimate_ManMonth);
                cmd.Parameters.AddWithValue("@Distance", QM.Distance);
                cmd.Parameters.AddWithValue("@EstimatedAmount", QM.DEstimatedAmount);
                cmd.Parameters.AddWithValue("@Dcurrency", QM.Dcurrency);
                cmd.Parameters.AddWithValue("@DExchangeRate", QM.DExchangeRate);
                cmd.Parameters.AddWithValue("@DTotalAmount", QM.DTotalAmount);
                cmd.Parameters.AddWithValue("@EQ_ID", QM.EQ_ID);
                cmd.Parameters.AddWithValue("@TypeDI", QM.Type);
                cmd.Parameters.AddWithValue("@Remark", QM.Remark);
                cmd.Parameters.AddWithValue("@AutoOrderRate", QM.AutoOrderRate);


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

        public string InsertUpdateIOrderType(EnquiryMaster QM) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_EnquiryMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "34");

                cmd.Parameters.AddWithValue("@OrderType", QM.IOrderType);
                cmd.Parameters.AddWithValue("@OrderRate", QM.IOrderRate);
                cmd.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", QM.IEstimate_ManDays_ManMonth);
                cmd.Parameters.AddWithValue("@Estimate_ManMonth", QM.IEstimate_ManMonth);
                cmd.Parameters.AddWithValue("@Distance", QM.IDistance);
                cmd.Parameters.AddWithValue("@EstimatedAmount", QM.IEstimatedAmount);
                cmd.Parameters.AddWithValue("@Dcurrency", QM.Icurrency);
                cmd.Parameters.AddWithValue("@DExchangeRate", QM.IExchangeRate);
                cmd.Parameters.AddWithValue("@DTotalAmount", QM.ITotalAmount);
                cmd.Parameters.AddWithValue("@EQ_ID", QM.EQ_ID);
                cmd.Parameters.AddWithValue("@TypeDI", QM.Type);
                cmd.Parameters.AddWithValue("@Remark", QM.Remark);
                cmd.Parameters.AddWithValue("@AutoOrderRate", QM.AutoOrderRate);

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

        public DataTable DOrderType(int? QT_ID)
        {
            DataTable DOrderType = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_EnquiryMaster", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "35");
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_EM_ID", QT_ID);

                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DOrderType);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DOrderType.Dispose();
            }
            return DOrderType;
        }

        public DataTable IOrderType(int? QT_ID)
        {
            DataTable DOrderType = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_EnquiryMaster", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "36");
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_EM_ID", QT_ID);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DOrderType);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DOrderType.Dispose();
            }
            return DOrderType;
        }

        //public List<EnquiryMaster> GetRegretReason()// Binding Sales Masters DashBoard of Master Page 
        //{
        //    DataTable DTEMDashBoard = new DataTable();
        //    List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
        //    try
        //    {
        //        SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
        //        CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
        //        CMDGetEnquriy.CommandTimeout = 1000000;
        //        CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "38");

        //        SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
        //        SDAGetEnquiry.Fill(DTEMDashBoard);
        //        if (DTEMDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in DTEMDashBoard.Rows)
        //            {
        //                lstEnquiryDashB.Add(
        //                   new EnquiryMaster
        //                   {
        //                       RegretId = Convert.ToInt32(dr["Id"]),
        //                       RegretReason = Convert.ToString(dr["Reason"]),
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

        //public List<EnquiryMaster> GetRegretActionTaken()// Binding Sales Masters DashBoard of Master Page 
        //{
        //    DataTable DTEMDashBoard = new DataTable();
        //    List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
        //    try
        //    {
        //        SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
        //        CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
        //        CMDGetEnquriy.CommandTimeout = 1000000;
        //        CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "39");

        //        SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
        //        SDAGetEnquiry.Fill(DTEMDashBoard);
        //        if (DTEMDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in DTEMDashBoard.Rows)
        //            {
        //                lstEnquiryDashB.Add(
        //                   new EnquiryMaster
        //                   {
        //                       RegretActionTakenId = Convert.ToInt32(dr["Id"]),
        //                       RegretActionTaken = Convert.ToString(dr["Action"]),
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



        public List<EnquiryMaster> GetRegretReason()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "41");

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new EnquiryMaster
                           {
                               RegretId = Convert.ToInt32(dr["Id"]),
                               RegretReason = Convert.ToString(dr["Reason"]),
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

        public List<EnquiryMaster> GetRegretActionTaken()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "42");

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new EnquiryMaster
                           {
                               RegretActionTakenId = Convert.ToInt32(dr["Id"]),
                               RegretActionTaken = Convert.ToString(dr["Action"]),
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

        public DataTable GetEnquiryCheckList()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 0;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "44");
                // CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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


        public DataTable GetTrainerNames(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 47);
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

        public DataTable GetTraineeName(string prefix, string obsType, string EmpCat, string TypeOfEmp, string Branch, string TrainType, string UserRole)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_EnquiryMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 48);
                cmd.Parameters.AddWithValue("@CompanyName", prefix);
                cmd.Parameters.AddWithValue("@obsType", obsType);
                cmd.Parameters.AddWithValue("@EmpCat", EmpCat);
                cmd.Parameters.AddWithValue("@TypeOfEmp", TypeOfEmp);
                cmd.Parameters.AddWithValue("@Branch", Branch);
                cmd.Parameters.AddWithValue("@TrainType", TrainType);
                cmd.Parameters.AddWithValue("@UserRole", UserRole);

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

        public DataSet GetDropDownList()//Binding All Dropdownlist
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 49);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }

        public List<EnquiryMaster> GetRegEnquiryListDashBoardOrderTypeWise()// added by nikita RegEnquiryOrderTypeWise
        {
            DataTable DTEMDashBoard = new DataTable();
            List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 51);

                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new EnquiryMaster
                           {
                               //Count = DTEMDashBoard.Rows.Count,
                               //ContactNo = Convert.ToString(dr["ContactNo"]),
                               //ContactName = Convert.ToString(dr["ContactName"]),
                               //ARC = Convert.ToString(dr["ARC"]),
                               //ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               //EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               //EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               //EnquiryDescription = Convert.ToString(dr["Description"]),
                               //ProjectName = Convert.ToString(dr["ProjectName"]),
                               //Company_Name = Convert.ToString(dr["CompanyName"]),
                               //Branch = Convert.ToString(dr["OriginatingBranch"]),
                               //OpendateS = Convert.ToString(dr["DateOpened"]),
                               //Source = Convert.ToString(dr["source"]),
                               //EstCloseS = Convert.ToString(dr["EstClose"]),
                               //ProjectType = Convert.ToString(dr["ProjectType"]),
                               //PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               //Status = Convert.ToString(dr["status"]),
                               //RegretReason = Convert.ToString(dr["RegretReason"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),
                               //DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                               //IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               //TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               //IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               //Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               //Intlocation = Convert.ToString(dr["Intlocation"]),
                               //Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               //Icurrency = Convert.ToString(dr["ICurrency"])

                               //added by nikita 16-08-2023
                               RefDate = Convert.ToString(dr["refDate"]),
                               EndCustomer = Convert.ToString(dr["EndCustomer"]),
                               Notes = Convert.ToString(dr["Notes"]),
                               NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                               //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                               ContactNo = Convert.ToString(dr["ContactNo"]),
                               ContactName = Convert.ToString(dr["ContactName"]),
                               ARC = Convert.ToString(dr["ARC"]),
                               ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               ProjectName = Convert.ToString(dr["ProjectName"]),
                               Company_Name = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               OpendateS = Convert.ToString(dr["DateOpened"]),
                               Source = Convert.ToString(dr["source"]),
                               EstCloseS = Convert.ToString(dr["EstClose"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                               Status = Convert.ToString(dr["status"]),
                               RegretActionTaken = Convert.ToString(dr["EnquiryRegretActionTaken"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                               Owner = Convert.ToString(dr["OwnerName"]),//added by nikita 09082023
                               //DEstimatedAmount = "", //Convert.ToString(dr["DEstimatedAmount"]),commenetd by nikita
                               //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),commenetd by nikita
                               DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),//added by nikita 09082023
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),//added by nikita 09082023
                               TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               Intlocation = Convert.ToString(dr["Intlocation"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               OrderType = Convert.ToString(dr["OrderType"]),

                               Icurrency = Convert.ToString(dr["ICurrency"]),
                               LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),
                               //    RegretStatus = Convert.ToString(dr["EStatus"]),
                               // CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
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

        public DataTable GetRegDataByDateOrderTypeWise(string FromDate, string ToDate)
        {
            DataTable DTEMDashBoard = new DataTable();
            try
            {
                string _date1 = string.Empty;
                string _date2 = string.Empty;
                string[] FromDt = FromDate.Split('/');
                string[] ToDt = ToDate.Split('/');
                _date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                _date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];

                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 52);
                CMDGetEnquriy.Parameters.AddWithValue("@DateFrom", _date1);
                CMDGetEnquriy.Parameters.AddWithValue("@DateTo", _date2);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public List<EnquiryMaster> GetEnquiryListDashBoard1(string Type)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                if (Type == "NoOfEnquiryInLast15Days")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 54);
                }
                else if (Type == "NoOfEnquiryRegret")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 55);
                }
                else if (Type == "NoOfEnquiryRegretInLast15Days")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 56);
                }
                else
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 53);
                }

                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new EnquiryMaster
                           {
                               Count = DTEMDashBoard.Rows.Count,


                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),

                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               Company_Name = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               Owner = Convert.ToString(dr["OwnerName"]),
                               CreatedDate = Convert.ToDateTime(dr["DateOpened"]),
                               Type = Convert.ToString(dr["ServiceType"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               SubServiceType = Convert.ToString(dr["ServiceType"]),
                               EstCloseS = Convert.ToString(dr["EstClose"]),
                               OpendateS = Convert.ToString(dr["DateOpened"]),
                               //RegretStatus = Convert.ToString(dr["RegretStatus"]),
                               //RegretReason = Convert.ToString(dr["RegretReason"]),
                               CreatedBy = Convert.ToString(dr["OwnerName"]),
                               DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               Icurrency = Convert.ToString(dr["Icurrency"]),
                               EstimatedAmount = Convert.ToDecimal(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               RegretStatus = Convert.ToString(dr["EStatus"]),


                               //Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               //InspectionLocation = Convert.ToString(dr["InspectionLocation"]),



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

        public DataSet GetPrimaryMaterial()
        {

            DataSet DSGetDdlList = new DataSet();
            try
            {

                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 78);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }


        #region Conflict Jobs
        public DataTable GetConflictDetail(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                //CMDGetEnquiryDtls.CommandTimeout = 120; // Increase timeout to 2 minutes
                //CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 64);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 74);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataTable GetConflictDetail1(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                //CMDGetEnquiryDtls.CommandTimeout = 120; // Increase timeout to 2 minutes
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 71);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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
        public DataTable GetConflictApprovalDetail(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                //CMDGetEnquiryDtls.CommandTimeout = 120; // Increase timeout to 2 minutes
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", "65");
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public string InsertAndUpdateMetegatedReason(EnquiryMaster EM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (EM.EQ_ID != 0)
                {
                    SqlCommand CMDUpdateEnquiry = new SqlCommand("SP_EnquiryMaster", con);//Updating Record In Database
                    CMDUpdateEnquiry.CommandType = CommandType.StoredProcedure;
                    CMDUpdateEnquiry.Parameters.AddWithValue("@SP_Type", 66);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@EQ_ID", EM.EQ_ID);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@Reason", EM.Reason);
                    CMDUpdateEnquiry.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDUpdateEnquiry.ExecuteNonQuery().ToString();
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

        public string ChangeStatus(int PK_EQID, string Type, string Status)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDUpdateEnquiry = new SqlCommand("SP_EnquiryMaster", con);//Updating Record In Database
                CMDUpdateEnquiry.CommandType = CommandType.StoredProcedure;
                CMDUpdateEnquiry.Parameters.AddWithValue("@SP_Type", 67);
                CMDUpdateEnquiry.Parameters.AddWithValue("@EQ_ID", PK_EQID);
                CMDUpdateEnquiry.Parameters.AddWithValue("@Type", Type);
                CMDUpdateEnquiry.Parameters.AddWithValue("@Status", Status);
                Result = CMDUpdateEnquiry.ExecuteNonQuery().ToString();

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

        public DataTable GetConflictEnquiryD(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                //CMDGetEnquiryDtls.CommandTimeout = 120; // Increase timeout to 2 minutes
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 68);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public string ChangeStatusConflictEnquiry(EnquiryMaster EQ)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDUpdateEnquiry = new SqlCommand("SP_EnquiryMaster", con);//Updating Record In Database
                CMDUpdateEnquiry.CommandType = CommandType.StoredProcedure;
                CMDUpdateEnquiry.Parameters.AddWithValue("@SP_Type", 69);
                CMDUpdateEnquiry.Parameters.AddWithValue("@EQ_ID", EQ.EQ_ID);
                CMDUpdateEnquiry.Parameters.AddWithValue("@ConflictConfirmation", EQ.ConflictConfirmation);
                Result = CMDUpdateEnquiry.ExecuteNonQuery().ToString();

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

        public DataTable GetConflictConfirmationDetail(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                //CMDGetEnquiryDtls.CommandTimeout = 120; // Increase timeout to 2 minutes
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", "70");
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataTable GetConflictAdminD(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", "72");
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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





        public DataTable GetLegalMail()
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", "99");
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDGetEnquiryDtls);
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
        //added by nikita on 27062024

        public DataSet GetLegaldata()
        {
            DataSet DTScripName = new DataSet();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("GetLegalData", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
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

        //addded by nikita on 25062024
        public DataTable GetEnquiryNumber(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", "EN");
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataTable GetConflictAdminDForApprovalMail(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", "72");
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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
        //added by nikita 
        public DataTable GetselectedCompanyName(string CompanyName, string CompanyCode)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_SapCustomerData", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 2);
                CMDSearchNameCode.Parameters.AddWithValue("@ClientCode", CompanyCode);
                //CMDSearchNameCode.Parameters.AddWithValue("", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetInspectorList_Client(string CompanyName)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CompanyMaster> lstEnquiryDashB = new List<CompanyMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "74AA");
                CMDGetEnquriy.Parameters.AddWithValue("@CompanyName", CompanyName);
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

        //added by nikita 
        public DataSet GetCompanyAddrVendor(string Company_Name)//Geting List Of Enquiry Master record Details Binding Dddl List
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CompanyMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "27");
                CMDGetDdlLst.Parameters.AddWithValue("@Company_Name", Company_Name);
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }
        public CompanyMaster GetAddressByName(string companyname)
        {
            con.Open();
            CompanyMaster _vmcompany = new CompanyMaster();
            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_CompanyMaster", con);
                GetAddress.CommandType = CommandType.StoredProcedure;
                GetAddress.Parameters.AddWithValue("@SP_Type", 28);
                GetAddress.Parameters.AddWithValue("@Company_Name", companyname);

                SqlDataReader dr = GetAddress.ExecuteReader();
                while (dr.Read())
                {
                    _vmcompany.Address = dr["Address"].ToString();
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

        public DataTable GetselectedCompanyData(string CompanyCode)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_SapCustomerData", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 3);
                CMDSearchNameCode.Parameters.AddWithValue("@ClientCode", CompanyCode);
                //CMDSearchNameCode.Parameters.AddWithValue("", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetSearchCompanyNametoshow(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_SapCustomerData", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 4);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
                CMDSearchNameCode.Parameters.AddWithValue("@Createdby", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable ChkCityExistFromEnquiry(string CityName)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CompanyMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", "23");
                CMDEditContact.Parameters.AddWithValue("@CompanyName", CityName);


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


        public string InsertUpdatevendorContact(EnquiryMaster ECM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDInsertUpdateContact = new SqlCommand("SP_CompanyMaster", con);
                CMDInsertUpdateContact.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateContact.Parameters.AddWithValue("@SP_Type", 5);
                CMDInsertUpdateContact.Parameters.AddWithValue("@ContactName", ECM.ContactNames);
                CMDInsertUpdateContact.Parameters.AddWithValue("@CompanyName", ECM.CompanyName);
                //  CMDInsertUpdateContact.Parameters.AddWithValue("@FK_CMP_ID", ECM.ContactCompanyName);
                CMDInsertUpdateContact.Parameters.AddWithValue("@FK_CMP_ID", 0);
                CMDInsertUpdateContact.Parameters.AddWithValue("@Title", ECM.TitleName);
                CMDInsertUpdateContact.Parameters.AddWithValue("@HomePhone", ECM.HomePhone);
                CMDInsertUpdateContact.Parameters.AddWithValue("@Mobile", ECM.Mobile);
                CMDInsertUpdateContact.Parameters.AddWithValue("@Address", ECM.Address);
                CMDInsertUpdateContact.Parameters.AddWithValue("@FaxNo", ECM.Fax_No);
                CMDInsertUpdateContact.Parameters.AddWithValue("@PanNumber", ECM.Pan_No);
                CMDInsertUpdateContact.Parameters.AddWithValue("@IsMainContact", ECM.IsMainContact);
                CMDInsertUpdateContact.Parameters.AddWithValue("@ContactStatus", ECM.ContactStatus);
                CMDInsertUpdateContact.Parameters.AddWithValue("@Email", ECM.Email);


                CMDInsertUpdateContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                // Result = CMDInsertUpdateContact.ExecuteNonQuery().ToString();
                CMDInsertUpdateContact.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                Result = CMDInsertUpdateContact.ExecuteNonQuery().ToString();
                string id = CMDInsertUpdateContact.Parameters["@ReturnCompanyId"].Value.ToString();
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



        public DataTable GetSearchCompanyName(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_SapCustomerData", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 1);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
                CMDSearchNameCode.Parameters.AddWithValue("@Createdby", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetSapCompanyName(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 75);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
                //CMDSearchNameCode.Parameters.AddWithValue("", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetInspector(string CompanyName)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CompanyMaster> lstEnquiryDashB = new List<CompanyMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 76);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_ID", CompanyName);
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


        public DataTable GetDuplicate(string CompanyName,int ? EQID)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CompanyMaster> lstEnquiryDashB = new List<CompanyMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 81    );
                CMDGetEnquriy.Parameters.AddWithValue("@PK_ID", CompanyName);
                CMDGetEnquriy.Parameters.AddWithValue("@EQ_ID", EQID);
                
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

        public List<EnquiryMaster> GetAddress()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<EnquiryMaster> lstEnquiryDashB = new List<EnquiryMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "77");

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new EnquiryMaster
                           {
                               CompanyAddress = Convert.ToString(dr["Name"]),
                               Address = Convert.ToString(dr["Name"]),
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

        public DataSet Get_Enquirydetails(string CompanyName, int EQ_ID, int CMP_id)
        {
            DataSet DSFeedbackQuestion = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_EnquiryMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 78);
                cmd.Parameters.AddWithValue("@cmp_id", CMP_id);
                cmd.Parameters.AddWithValue("@EQ_Id", EQ_ID);
                //cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserID"]);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DSFeedbackQuestion);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSFeedbackQuestion.Dispose();
            }
            return DSFeedbackQuestion;
        }

        public DataSet Get_EnquiryBudgetarydetails(string CompanyName, int EQ_ID, int CMP_id)
        {
            DataSet DSFeedbackQuestion = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_EnquiryMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 79);
                cmd.Parameters.AddWithValue("@cmp_id", CMP_id);
                // cmd.Parameters.AddWithValue("@EQ_Id", EQ_ID);
                //cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserID"]);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DSFeedbackQuestion);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSFeedbackQuestion.Dispose();
            }
            return DSFeedbackQuestion;
        }


    }
}