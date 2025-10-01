using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class AdminDashboardController : Controller
    {
        // GET: AdminDashboard
        public ActionResult Dashboard()
        {
            //DataTable DTGetEnquiryDtls = new DataTable();
            DataSet DTGetEnquiryDtls = new DataSet();
            Dashboard ObjModelEnquiry = new Dashboard();

            DataTable FinalData1 = new DataTable();
            DataTable FinalData2 = new DataTable();
            DataTable FinalData3 = new DataTable();
            DataTable FinalData4 = new DataTable();

            DataRow[] SelectedRows; // = new DataTable();
            string filterexpression = string.Empty;
            DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
            List<ListQuotationDashboard> lstQuotation1 = new List<ListQuotationDashboard>();
            List<ListQuotationDashboard> lstQuotation2 = new List<ListQuotationDashboard>();
            List<ListQuotationDashboard> lstQuotation3 = new List<ListQuotationDashboard>();
            List<ListQuotationDashboard> lstQuotation4 = new List<ListQuotationDashboard>();

            DTGetEnquiryDtls = objDalEnquiryMaster.GetDashboardData();
            #region quotation Dashboard

            if (DTGetEnquiryDtls.Tables.Count > 0)
            {
                /// Action Required
                filterexpression = "QType = '1'";
                SelectedRows = DTGetEnquiryDtls.Tables[1].Select(filterexpression);
                if (SelectedRows.Count() != 0)
                    FinalData1 = SelectedRows.CopyToDataTable();

                SelectedRows = null;

                /// Auto Approved
                filterexpression = "QType = '2'";
                SelectedRows = DTGetEnquiryDtls.Tables[1].Select(filterexpression);

                if (SelectedRows.Count() != 0)
                    FinalData2 = SelectedRows.CopyToDataTable();

                SelectedRows = null;
                /// Orders Won and Lost
                filterexpression = "QType = '3'";
                SelectedRows = DTGetEnquiryDtls.Tables[1].Select(filterexpression);
                if (SelectedRows.Count() != 0)
                    FinalData3 = SelectedRows.CopyToDataTable();

                SelectedRows = null;

                ///Regret Enquires
                filterexpression = "QType = '4'";
                SelectedRows = DTGetEnquiryDtls.Tables[1].Select(filterexpression);
                if (SelectedRows.Count() != 0)
                    FinalData4 = SelectedRows.CopyToDataTable();

                SelectedRows = null;

                lstQuotation1 = (from n in FinalData1.AsEnumerable()
                                 select new ListQuotationDashboard()
                                 {
                                     Name = n.Field<string>(FinalData1.Columns["Name"].ToString()),
                                     QuotationNumber = n.Field<string>(FinalData1.Columns["QuotationNumber"].ToString()),
                                     CreatedDate = n.Field<string>(FinalData1.Columns["createdate"].ToString()),
                                     PK_QM_ID = n.Field<int>(FinalData1.Columns["PK_QTID"].ToString()),
                                     Role = n.Field<string>(FinalData1.Columns["Role"].ToString()),
                                     //EstimatedAmount =  n.Field<int>(FinalData1.Columns["EstimatedAmount"].ToString()),
                                     //EstimatedAmount1 = n.Field<string>(FinalData1.Columns["EstimatedAmount"].ToString()),
                                     BranchHeadName = n.Field<string>(FinalData1.Columns["BranchHeadName"].ToString()),

                                     RegretStatus = n.Field<string>(FinalData1.Columns["RegretStatus"].ToString()),
                                     RegretReason = n.Field<string>(FinalData1.Columns["RegretReason"].ToString()),
                                     EnquiryNumber = n.Field<string>(FinalData1.Columns["EnquiryNumber"].ToString()),
                                     QuotationStatus = n.Field<string>(FinalData1.Columns["QStatus"].ToString()),
                                     PCHComment = n.Field<string>(FinalData1.Columns["PCHComment1"].ToString()),
                                     CHComment = n.Field<string>(FinalData1.Columns["CHComment1"].ToString()),
                                     CostSheetPCHStatus = n.Field<string>(FinalData1.Columns["CostSheetPCHStatus"].ToString()),
                                     CostSheetCHStatus = n.Field<string>(FinalData1.Columns["CostSheetCHStatus"].ToString()),
                                     // OBSType = n.Field<string>(FinalData1.Columns["OBSType"].ToString()),
                                     ClusterHeadName = n.Field<string>(FinalData1.Columns["ClusterHead"].ToString()),
                                     LostReason = n.Field<string>(FinalData1.Columns["LostReason"].ToString()),
                                     SendForApprovel = n.Field<string>(FinalData1.Columns["SendForApprovel"].ToString()),

                                     SendForApprovelDate = n.Field<string>(FinalData1.Columns["SendForApprovelDate"].ToString()),
                                     ApprovedOrNotApprovedDatePCH = n.Field<string>(FinalData1.Columns["ApprovedOrNotApprovedDatePCH"].ToString()),
                                     ApprovedOrNotApprovedDateCH = n.Field<string>(FinalData1.Columns["ApprovedOrNotApprovedDateCH"].ToString()),

                                     SenderCommentDate = n.Field<string>(FinalData1.Columns["SenderCommentDate1"].ToString()),
                                     PCHCommentDate = n.Field<string>(FinalData1.Columns["PCHCommentDate1"].ToString()),
                                     CHCommentDate = n.Field<string>(FinalData1.Columns["CHCommentDate1"].ToString()),
                                     costsheettype = n.Field<string>(FinalData1.Columns["costsheettype"].ToString()),
                                     CompanyName = n.Field<string>(FinalData1.Columns["CompanyName"].ToString()),
                                 }).ToList();
            }
            ViewBag.Quotation1 = lstQuotation1;

            lstQuotation2 = (from n in FinalData2.AsEnumerable()
                             select new ListQuotationDashboard()
                             {
                                 Name = n.Field<string>(FinalData2.Columns["Name"].ToString()),
                                 QuotationNumber = n.Field<string>(FinalData2.Columns["QuotationNumber"].ToString()),
                                 CreatedDate = n.Field<string>(FinalData2.Columns["createdate"].ToString()),
                                 PK_QM_ID = n.Field<int>(FinalData2.Columns["PK_QTID"].ToString()),
                                 Role = n.Field<string>(FinalData2.Columns["Role"].ToString()),
                                 //EstimatedAmount =  n.Field<int>(FinalData2.Columns["EstimatedAmount"].ToString()),
                                 //EstimatedAmount1 = n.Field<string>(FinalData2.Columns["EstimatedAmount"].ToString()),
                                 BranchHeadName = n.Field<string>(FinalData2.Columns["BranchHeadName"].ToString()),

                                 RegretStatus = n.Field<string>(FinalData2.Columns["RegretStatus"].ToString()),
                                 RegretReason = n.Field<string>(FinalData2.Columns["RegretReason"].ToString()),
                                 EnquiryNumber = n.Field<string>(FinalData2.Columns["EnquiryNumber"].ToString()),
                                 QuotationStatus = n.Field<string>(FinalData2.Columns["QStatus"].ToString()),
                                 PCHComment = n.Field<string>(FinalData2.Columns["PCHComment1"].ToString()),
                                 CHComment = n.Field<string>(FinalData2.Columns["CHComment1"].ToString()),
                                 CostSheetPCHStatus = n.Field<string>(FinalData2.Columns["CostSheetPCHStatus"].ToString()),
                                 CostSheetCHStatus = n.Field<string>(FinalData2.Columns["CostSheetCHStatus"].ToString()),
                                 // OBSType = n.Field<string>(FinalData2.Columns["OBSType"].ToString()),
                                 ClusterHeadName = n.Field<string>(FinalData2.Columns["ClusterHead"].ToString()),
                                 LostReason = n.Field<string>(FinalData2.Columns["LostReason"].ToString()),
                                 SendForApprovel = n.Field<string>(FinalData2.Columns["SendForApprovel"].ToString()),

                                 SendForApprovelDate = n.Field<string>(FinalData2.Columns["SendForApprovelDate"].ToString()),
                                 ApprovedOrNotApprovedDatePCH = n.Field<string>(FinalData2.Columns["ApprovedOrNotApprovedDatePCH"].ToString()),
                                 ApprovedOrNotApprovedDateCH = n.Field<string>(FinalData2.Columns["ApprovedOrNotApprovedDateCH"].ToString()),

                                 SenderCommentDate = n.Field<string>(FinalData2.Columns["SenderCommentDate1"].ToString()),
                                 PCHCommentDate = n.Field<string>(FinalData2.Columns["PCHCommentDate1"].ToString()),
                                 CHCommentDate = n.Field<string>(FinalData2.Columns["CHCommentDate1"].ToString()),
                                 costsheettype = n.Field<string>(FinalData2.Columns["costsheettype"].ToString()),
                                 CompanyName = n.Field<string>(FinalData2.Columns["CompanyName"].ToString()),
                             }).ToList();

            ViewBag.Quotation2 = lstQuotation2;

            lstQuotation3 = (from n in FinalData3.AsEnumerable()
                             select new ListQuotationDashboard()
                             {
                                 Name = n.Field<string>(FinalData3.Columns["Name"].ToString()),
                                 QuotationNumber = n.Field<string>(FinalData3.Columns["QuotationNumber"].ToString()),
                                 CreatedDate = n.Field<string>(FinalData3.Columns["createdate"].ToString()),
                                 PK_QM_ID = n.Field<int>(FinalData3.Columns["PK_QTID"].ToString()),
                                 Role = n.Field<string>(FinalData3.Columns["Role"].ToString()),
                                 //EstimatedAmount =  n.Field<int>(FinalData3.Columns["EstimatedAmount"].ToString()),
                                 //EstimatedAmount1 = n.Field<string>(FinalData3.Columns["EstimatedAmount"].ToString()),
                                 BranchHeadName = n.Field<string>(FinalData3.Columns["BranchHeadName"].ToString()),

                                 RegretStatus = n.Field<string>(FinalData3.Columns["RegretStatus"].ToString()),
                                 RegretReason = n.Field<string>(FinalData3.Columns["RegretReason"].ToString()),
                                 EnquiryNumber = n.Field<string>(FinalData3.Columns["EnquiryNumber"].ToString()),
                                 QuotationStatus = n.Field<string>(FinalData3.Columns["QStatus"].ToString()),
                                 PCHComment = n.Field<string>(FinalData3.Columns["PCHComment1"].ToString()),
                                 CHComment = n.Field<string>(FinalData3.Columns["CHComment1"].ToString()),
                                 CostSheetPCHStatus = n.Field<string>(FinalData3.Columns["CostSheetPCHStatus"].ToString()),
                                 CostSheetCHStatus = n.Field<string>(FinalData3.Columns["CostSheetCHStatus"].ToString()),
                                 // OBSType = n.Field<string>(FinalData3.Columns["OBSType"].ToString()),
                                 ClusterHeadName = n.Field<string>(FinalData3.Columns["ClusterHead"].ToString()),
                                 LostReason = n.Field<string>(FinalData3.Columns["LostReason"].ToString()),
                                 SendForApprovel = n.Field<string>(FinalData3.Columns["SendForApprovel"].ToString()),

                                 SendForApprovelDate = n.Field<string>(FinalData3.Columns["SendForApprovelDate"].ToString()),
                                 ApprovedOrNotApprovedDatePCH = n.Field<string>(FinalData3.Columns["ApprovedOrNotApprovedDatePCH"].ToString()),
                                 ApprovedOrNotApprovedDateCH = n.Field<string>(FinalData3.Columns["ApprovedOrNotApprovedDateCH"].ToString()),

                                 SenderCommentDate = n.Field<string>(FinalData3.Columns["SenderCommentDate1"].ToString()),
                                 PCHCommentDate = n.Field<string>(FinalData3.Columns["PCHCommentDate1"].ToString()),
                                 CHCommentDate = n.Field<string>(FinalData3.Columns["CHCommentDate1"].ToString()),
                                 costsheettype = n.Field<string>(FinalData3.Columns["costsheettype"].ToString()),
                                 CompanyName = n.Field<string>(FinalData3.Columns["CompanyName"].ToString()),
                             }).ToList();

            ViewBag.Quotation3 = lstQuotation3;


            lstQuotation4 = (from n in FinalData4.AsEnumerable()
                             select new ListQuotationDashboard()
                             {
                                 Name = n.Field<string>(FinalData4.Columns["Name"].ToString()),
                                 QuotationNumber = n.Field<string>(FinalData4.Columns["QuotationNumber"].ToString()),
                                 CreatedDate = n.Field<string>(FinalData4.Columns["createdate"].ToString()),
                                 PK_QM_ID = n.Field<int>(FinalData4.Columns["PK_QTID"].ToString()),
                                 Role = n.Field<string>(FinalData4.Columns["Role"].ToString()),
                                 //EstimatedAmount =  n.Field<int>(FinalData4.Columns["EstimatedAmount"].ToString()),
                                 //EstimatedAmount1 = n.Field<string>(FinalData4.Columns["EstimatedAmount"].ToString()),
                                 BranchHeadName = n.Field<string>(FinalData4.Columns["BranchHeadName"].ToString()),

                                 RegretStatus = n.Field<string>(FinalData4.Columns["RegretStatus"].ToString()),
                                 RegretReason = n.Field<string>(FinalData4.Columns["RegretReason"].ToString()),
                                 EnquiryNumber = n.Field<string>(FinalData4.Columns["EnquiryNumber"].ToString()),
                                 QuotationStatus = n.Field<string>(FinalData4.Columns["QStatus"].ToString()),
                                 PCHComment = n.Field<string>(FinalData4.Columns["PCHComment1"].ToString()),
                                 CHComment = n.Field<string>(FinalData4.Columns["CHComment1"].ToString()),
                                 CostSheetPCHStatus = n.Field<string>(FinalData4.Columns["CostSheetPCHStatus"].ToString()),
                                 CostSheetCHStatus = n.Field<string>(FinalData4.Columns["CostSheetCHStatus"].ToString()),
                                 // OBSType = n.Field<string>(FinalData4.Columns["OBSType"].ToString()),
                                 ClusterHeadName = n.Field<string>(FinalData4.Columns["ClusterHead"].ToString()),
                                 LostReason = n.Field<string>(FinalData4.Columns["LostReason"].ToString()),
                                 SendForApprovel = n.Field<string>(FinalData4.Columns["SendForApprovel"].ToString()),

                                 SendForApprovelDate = n.Field<string>(FinalData4.Columns["SendForApprovelDate"].ToString()),
                                 ApprovedOrNotApprovedDatePCH = n.Field<string>(FinalData4.Columns["ApprovedOrNotApprovedDatePCH"].ToString()),
                                 ApprovedOrNotApprovedDateCH = n.Field<string>(FinalData4.Columns["ApprovedOrNotApprovedDateCH"].ToString()),

                                 SenderCommentDate = n.Field<string>(FinalData4.Columns["SenderCommentDate1"].ToString()),
                                 PCHCommentDate = n.Field<string>(FinalData4.Columns["PCHCommentDate1"].ToString()),
                                 CHCommentDate = n.Field<string>(FinalData4.Columns["CHCommentDate1"].ToString()),
                                 costsheettype = n.Field<string>(FinalData4.Columns["costsheettype"].ToString()),
                                 CompanyName = n.Field<string>(FinalData4.Columns["CompanyName"].ToString()),
                             }).ToList();

            ViewBag.Quotation4 = lstQuotation4;

            #endregion




            #region Get Individual Performance
            List<IndividualPerformance> lstAppeal = new List<IndividualPerformance>();
            DataSet dsI = new DataSet();
            // dsI = objDalEnquiryMaster.GetIndividualPerformance();
            if (dsI.Tables.Count > 0)
            {
                if (dsI.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dsI.Tables[0].Rows)
                    {
                        string SYTD = "";
                        if (dr["ScoreYTD"].Equals(DBNull.Value))
                        {
                            SYTD = "";
                        }
                        else
                        {
                            Double inputValue = Math.Round(Convert.ToDouble(dr["ScoreYTD"]), 3);
                            SYTD = Convert.ToString(inputValue);
                        }


                        lstAppeal.Add(
                            new IndividualPerformance
                            {


                                SubHeadingName = Convert.ToString(dr["SubHeadingName"]),
                                UOM = Convert.ToString(dr["UOM"]),
                                Weightage = Convert.ToString(dr["Weightage"]),
                                MonthlyTarget = Convert.ToString(dr["MonthlyTarget"]),
                                TargetYTD = Convert.ToString(dr["TargetYTD"]),
                                CurrentMonth = Convert.ToString(dr["CurrentMonth"]),
                                PreviousMonth = Convert.ToString(dr["PreviousMonth"]),
                                YTD = Convert.ToString(dr["YTD"]),
                                ScoreYTD = SYTD,

                                PreviousYTD = Convert.ToString(dr["PreviousYTD"]),
                                PreviousScoreYTD = Convert.ToString(dr["PreviousScoreYTD"]),
                                ScoreYTDColor = dr["ScoreYTD"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dr["ScoreYTD"]),
                                ScorePreviousYTDColor = dr["PreviousScoreYTD"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dr["PreviousScoreYTD"]),


                            });
                    }
                    ViewData["LibraryList"] = lstAppeal;
                }
            }
            #endregion

            ViewData["LibraryList"] = lstAppeal;
            return View(ObjModelEnquiry);





        }




    }
}

