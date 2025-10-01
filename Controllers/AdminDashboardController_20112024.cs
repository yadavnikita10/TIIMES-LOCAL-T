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
            DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
            List<ListQuotationDashboard> lstQuotation = new List<ListQuotationDashboard>();
            DALJob objjob = new DALJob();

            DTGetEnquiryDtls = objDalEnquiryMaster.GetDashboardData();
            if (DTGetEnquiryDtls.Tables[0].Rows.Count > 0)
            {
                //ObjModelEnquiry.NoOfEnquiry = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfEnquiry"]);
                //ObjModelEnquiry.NoOfEnquiryInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfEnquiryInLast15Days"]);
                //ObjModelEnquiry.NoOfEnquiryRegret = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfEnquiryRegret"]);
                //ObjModelEnquiry.NoOfEnquiryRegretInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfEnquiryRegretInLast15Days"]);

                //ObjModelEnquiry.NoOfQuotation = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfQuotation"]);
                //ObjModelEnquiry.NoOfQuotationInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfQuotationInLast15Days"]);

                //ObjModelEnquiry.QuotationApprovedByPCH = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedByPCH"]);
                //ObjModelEnquiry.QuotationApprovedByPCHInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedByPCHInLast15Days"]);

                //ObjModelEnquiry.QuotationApprovedByClusterHead = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedByClusterHead"]);
                //ObjModelEnquiry.QuotationApprovedByClusterHeadInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedByPCHInLast15Days"]);

                //ObjModelEnquiry.QuotationWon = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationWon"]);
                //ObjModelEnquiry.QuotationWonInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationWonInLast15Days"]);

                //ObjModelEnquiry.QuotationLost = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationLost"]);
                //ObjModelEnquiry.QuotationLostInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationLostInLast15Days"]);

                //ObjModelEnquiry.QuotationPendingForApprovel = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationPendingForApprovel"]);
                //ObjModelEnquiry.QuotationPendingForApprovelInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationPendingForApprovelInLast15Days"]);


                //ObjModelEnquiry.QuotationOpen = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationOpen"]);
                //ObjModelEnquiry.QuotationOpenFromLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationInLast15Days"]);

                //ObjModelEnquiry.HitRate = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["HitRate"]);
                //ObjModelEnquiry.HitRateInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["HitRateInLast15Days"]);

                //ObjModelEnquiry.LossRate = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["LossRate"]);
                //ObjModelEnquiry.LossRateIn15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["LossRateIn15Days"]);

                //ObjModelEnquiry.InProcess = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["InProcess"]);
                //ObjModelEnquiry.InProcessLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["InProcessLast15Days"]);

                //ObjModelEnquiry.AutoApproved = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["AutoApproved"]);
                //ObjModelEnquiry.AutoApprovedInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["AutoApprovedInLast15Days"]);

                //ObjModelEnquiry.QuotationAmount = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationAmount"]);
                //ObjModelEnquiry.QuotationAmountInLast15Days = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationAmountInLast15Days"]);

                #region
                //ObjModelEnquiry.NoOfEnquiryCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfEnquiryCFY"]);
                //ObjModelEnquiry.NoOfEnquiryInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfEnquiryInPFY"]);
                //ObjModelEnquiry.NoOfEnquiryInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfEnquiryInYTD"]);
                //ObjModelEnquiry.NoOfEnquiryRegretInCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfEnquiryRegretInCFY"]);
                //ObjModelEnquiry.NoOfEnquiryRegretInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfEnquiryRegretInPFY"]);
                //ObjModelEnquiry.NoOfEnquiryRegretInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfEnquiryRegretInYTD"]);
                //ObjModelEnquiry.NoOfQuotationCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfQuotationCFY"]);
                //ObjModelEnquiry.NoOfQuotationInLFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfQuotationInLFY"]);
                //ObjModelEnquiry.NoOfQuotationInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["NoOfQuotationInYTD"]);
                //ObjModelEnquiry.QuotationApprovedInCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedInCFY"]);
                //ObjModelEnquiry.QuotationApprovedInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedInPFY"]);
                //ObjModelEnquiry.QuotationApprovedInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedInYTD"]);
                //ObjModelEnquiry.QuotationPendingApprovelCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationPendingApprovelCFY"]);
                //ObjModelEnquiry.QuotationPendingApprovelInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationPendingApprovelInPFY"]);
                //ObjModelEnquiry.QuotationPendingApprovelInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationPendingApprovelInYTD"]);
                //ObjModelEnquiry.QuotationApprovedByPCHCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedByPCHCFY"]);
                //ObjModelEnquiry.QuotationApprovedByPCHInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedByPCHInPFY"]);
                //ObjModelEnquiry.QuotationApprovedByPCHInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedByPCHInYTD"]);
                //ObjModelEnquiry.QuotationApprovedByClusterHeadInCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedByClusterHeadInCFY"]);
                //ObjModelEnquiry.QuotationApprovedByClusterHeadInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedByClusterHeadInPFY"]);
                //ObjModelEnquiry.QuotationApprovedByClusterHeadInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationApprovedByClusterHeadInYTD"]);
                //ObjModelEnquiry.QuotationWonInCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationWonInCFY"]);
                //ObjModelEnquiry.QuotationWonInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationWonInPFY"]);
                //ObjModelEnquiry.QuotationWonInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationWonInYTD"]);
                //ObjModelEnquiry.QuotationLostInCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationLostInCFY"]);
                //ObjModelEnquiry.QuotationLostInLastInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationLostInLastInPFY"]);
                //ObjModelEnquiry.QuotationLostInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationLostInYTD"]);
                //ObjModelEnquiry.QuotationPendingForApprovelInCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationPendingForApprovelInCFY"]);
                //ObjModelEnquiry.QuotationPendingForApprovelInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationPendingForApprovelInPFY"]);
                //ObjModelEnquiry.QuotationPendingForApprovelInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationPendingForApprovelInYTD"]);
                //ObjModelEnquiry.QuotationOpenCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationOpenCFY"]);
                //ObjModelEnquiry.QuotationInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationInPFY"]);
                //ObjModelEnquiry.QuotationInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationInYTD"]);
                //ObjModelEnquiry.HitRateCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["HitRateCFY"]);
                //ObjModelEnquiry.HitRateInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["HitRateInPFY"]);
                //ObjModelEnquiry.HitRateInFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["HitRateInFY"]);
                //ObjModelEnquiry.LossRateInCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["LossRateInCFY"]);
                //ObjModelEnquiry.LossRateInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["LossRateInPFY"]);
                //ObjModelEnquiry.LossRateInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["LossRateInYTD"]);
                //ObjModelEnquiry.InProcessCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["InProcessCFY"]);
                //ObjModelEnquiry.InProcessPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["InProcessPFY"]);
                //ObjModelEnquiry.InProcessYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["InProcessYTD"]);
                //ObjModelEnquiry.AutoApprovedCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["AutoApprovedCFY"]);
                //ObjModelEnquiry.AutoApprovedInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["AutoApprovedInPFY"]);
                //ObjModelEnquiry.AutoApprovedInFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["AutoApprovedInFY"]);
                //ObjModelEnquiry.QuotationAmountInCFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationAmountInCFY"]);
                //ObjModelEnquiry.QuotationAmountInPFY = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationAmountInPFY"]);
                //ObjModelEnquiry.QuotationAmountInYTD = Convert.ToString(DTGetEnquiryDtls.Tables[0].Rows[0]["QuotationAmountInYTD"]);
                #endregion



                #region quotation Dashboard
                if (DTGetEnquiryDtls.Tables[1].Rows.Count > 0)
                {
                    lstQuotation = (from n in DTGetEnquiryDtls.Tables[1].AsEnumerable()
                                    select new ListQuotationDashboard()
                                    {
                                        Name = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["Name"].ToString()),
                                        QuotationNumber = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["QuotationNumber"].ToString()),
                                        CreatedDate = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["createdate"].ToString()),
                                        PK_QM_ID = n.Field<int>(DTGetEnquiryDtls.Tables[1].Columns["PK_QTID"].ToString()),
                                        Role = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["Role"].ToString()),
                                        //EstimatedAmount =  n.Field<int>(DTGetEnquiryDtls.Tables[1].Columns["EstimatedAmount"].ToString()),
                                        //EstimatedAmount1 = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["EstimatedAmount"].ToString()),
                                        BranchHeadName = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["BranchHeadName"].ToString()),

                                        RegretStatus = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["RegretStatus"].ToString()),
                                        RegretReason = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["RegretReason"].ToString()),
                                        EnquiryNumber = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["EnquiryNumber"].ToString()),
                                        QuotationStatus = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["QStatus"].ToString()),
                                        PCHComment = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["PCHComment1"].ToString()),
                                        CHComment = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["CHComment1"].ToString()),
                                        CostSheetPCHStatus = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["CostSheetPCHStatus"].ToString()),
                                        CostSheetCHStatus = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["CostSheetCHStatus"].ToString()),
                                        // OBSType = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["OBSType"].ToString()),
                                        ClusterHeadName = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["ClusterHead"].ToString()),
                                        LostReason = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["LostReason"].ToString()),
                                        SendForApprovel = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["SendForApprovel"].ToString()),

                                        SendForApprovelDate = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["SendForApprovelDate"].ToString()),
                                        ApprovedOrNotApprovedDatePCH = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["ApprovedOrNotApprovedDatePCH"].ToString()),
                                        ApprovedOrNotApprovedDateCH = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["ApprovedOrNotApprovedDateCH"].ToString()),

                                        SenderCommentDate = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["SenderCommentDate1"].ToString()),
                                        PCHCommentDate = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["PCHCommentDate1"].ToString()),
                                        CHCommentDate = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["CHCommentDate1"].ToString()),
                                        costsheettype = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["costsheettype"].ToString()),
                                        CompanyName = n.Field<string>(DTGetEnquiryDtls.Tables[1].Columns["CompanyName"].ToString()),
                                    }).ToList();
                }
                ViewBag.Quotation = lstQuotation;
                #endregion


                #region Get Enquiry Legal data
                List<EnquiryMaster> legal = new List<EnquiryMaster>();
                DataSet data = new DataSet();
                data = objDalEnquiryMaster.GetLegaldata();
                if (data.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in data.Tables[0].Rows)
                    {

                        legal.Add(
                            new EnquiryMaster
                            {
                                EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                                Date_ = Convert.ToString(dr["CreatedDate_"]),
                                EmployeeName = Convert.ToString(dr["EmpName"]),
                                LegalReview = Convert.ToBoolean(dr["legalReview"]),
                                CompanyName = Convert.ToString(dr["CompanyName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                EQ_ID = Convert.ToInt32(dr["EQ_ID"]),

                            });
                    }
                    ViewData["Legaldata"] = legal;
                    ViewBag.legaldata = legal;


                }
                #endregion


                #region Get Job Legal data
                List<JobMasters> joblegal = new List<JobMasters>();
                DataSet data_ = new DataSet();
                data_ = objjob.GetLegaldata_job();
                if (data_.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in data_.Tables[0].Rows)
                    {

                        joblegal.Add(
                            new JobMasters
                            {
                                Job_Number = Convert.ToString(dr["job_number"]),
                                Date_ = Convert.ToString(dr["CreatedDate_"]),
                                EmpName = Convert.ToString(dr["EmpName"]),
                                LegalReview = Convert.ToBoolean(dr["legalReview"]),
                                Client_Name = Convert.ToString(dr["CompanyName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                                PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),



                            });
                    }
                    ViewData["JobLegaldata"] = joblegal;
                    ViewBag.legaldatajob = joblegal;


                }
                #endregion

                #region Get Individual Performance
                List<IndividualPerformance> lstAppeal = new List<IndividualPerformance>();
                DataSet dsI = new DataSet();
                dsI = objDalEnquiryMaster.GetIndividualPerformance();
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
                                ScoreYTD = SYTD,//Convert.ToString(dr["ScoreYTD"]),

                                PreviousYTD = Convert.ToString(dr["PreviousYTD"]),
                                PreviousScoreYTD = Convert.ToString(dr["PreviousScoreYTD"]),
                                ScoreYTDColor = dr["ScoreYTD"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dr["ScoreYTD"]),
                                ScorePreviousYTDColor = dr["PreviousScoreYTD"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dr["PreviousScoreYTD"]),


                            });
                     }
                    ViewData["LibraryList"] = lstAppeal;


                }
                #endregion

                ViewData["LibraryList"] = lstAppeal;
                return View(ObjModelEnquiry);
            }


            return View();

        }




    }
}

