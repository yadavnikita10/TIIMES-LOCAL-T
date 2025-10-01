using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class Dashboard
    {
        public string NoOfEnquiry { get; set; }
        public string NoOfEnquiryInLast15Days { get; set; }
        public string NoOfEnquiryRegret { get; set; }
        public string NoOfEnquiryRegretInLast15Days { get; set; }
        public string NoOfQuotation { get; set; }
        public string NoOfQuotationInLast15Days { get; set; }
        public string QuotationApprovedByPCH { get; set; }
        public string QuotationApprovedByPCHInLast15Days { get; set; }
        public string QuotationApprovedByClusterHead { get; set; }
        public string QuotationApprovedByClusterHeadInLast15Days { get; set; }
        public string QuotationPendingApprovel { get; set; }

        public string Name { get; set; }
        public string QuotationNumber { get; set; }


        //  public string QuotationPendingApprovel { get; set; }
        //13 Feb 
        public string QuotationWon { get; set; }
        public string QuotationWonInLast15Days { get; set; }
        public string QuotationLost { get; set; }
        public string QuotationLostInLast15Days { get; set; }
        public string QuotationPendingForApprovel { get; set; }
        public string QuotationPendingForApprovelInLast15Days { get; set; }
        public string QuotationOpen { get; set; }
        public string QuotationOpenFromLast15Days { get; set; }
        public string HitRate { get; set; }
        public string HitRateInLast15Days { get; set; }
        public string InProcess { get; set; }
        public string InProcessLast15Days { get; set; }
        public string LossRate { get; set; }
        public string LossRateIn15Days { get; set; }
        public string AutoApproved { get; set; }
        public string AutoApprovedInLast15Days { get; set; }
        public string QuotationAmount { get; set; }
        public string QuotationAmountInLast15Days { get; set; }



        public string NoOfEnquiryCFY { get; set; }
        public string NoOfEnquiryInPFY { get; set; }
        public string NoOfEnquiryInYTD { get; set; }
        public string NoOfEnquiryRegretInCFY { get; set; }
        public string NoOfEnquiryRegretInPFY { get; set; }
        public string NoOfEnquiryRegretInYTD { get; set; }
        public string NoOfQuotationCFY { get; set; }
        public string NoOfQuotationInLFY { get; set; }
        public string NoOfQuotationInYTD { get; set; }
        public string QuotationApprovedInCFY { get; set; }
        public string QuotationApprovedInPFY { get; set; }
        public string QuotationApprovedInYTD { get; set; }
        public string QuotationPendingApprovelCFY { get; set; }
        public string QuotationPendingApprovelInPFY { get; set; }
        public string QuotationPendingApprovelInYTD { get; set; }
        public string QuotationApprovedByPCHCFY { get; set; }
        public string QuotationApprovedByPCHInPFY { get; set; }
        public string QuotationApprovedByPCHInYTD { get; set; }
        public string QuotationApprovedByClusterHeadInCFY { get; set; }
        public string QuotationApprovedByClusterHeadInPFY { get; set; }
        public string QuotationApprovedByClusterHeadInYTD { get; set; }
        public string QuotationWonInCFY { get; set; }
        public string QuotationWonInPFY { get; set; }
        public string QuotationWonInYTD { get; set; }
        public string QuotationLostInCFY { get; set; }
        public string QuotationLostInLastInPFY { get; set; }
        public string QuotationLostInYTD { get; set; }
        public string QuotationPendingForApprovelInCFY { get; set; }
        public string QuotationPendingForApprovelInPFY { get; set; }
        public string QuotationPendingForApprovelInYTD { get; set; }
        public string QuotationOpenCFY { get; set; }
        public string QuotationInPFY { get; set; }
        public string QuotationInYTD { get; set; }
        public string HitRateCFY { get; set; }
        public string HitRateInPFY { get; set; }
        public string HitRateInFY { get; set; }
        public string LossRateInCFY { get; set; }
        public string LossRateInPFY { get; set; }
        public string LossRateInYTD { get; set; }
        public string InProcessCFY { get; set; }
        public string InProcessPFY { get; set; }
        public string InProcessYTD { get; set; }
        public string AutoApprovedCFY { get; set; }
        public string AutoApprovedInPFY { get; set; }
        public string AutoApprovedInFY { get; set; }
        public string QuotationAmountInCFY { get; set; }
        public string QuotationAmountInPFY { get; set; }
        public string QuotationAmountInYTD { get; set; }





    }


    public class ListQuotationDashboard
    {
        public string Name { get; set; }
        public string QuotationNumber { get; set; }
        public string CreatedDate { get; set; }
        public string Role { get; set; }
        public string BranchHeadName { get; set; }
        public string RegretStatus { get; set; }
        public string RegretReason { get; set; }
        public string EnquiryNumber { get; set; }
        public string QuotationStatus { get; set; }
        public int EstimatedAmount { get; set; }
        public string EstimatedAmount1 { get; set; }
        public string PCHComment { get; set; }
        public string CHComment { get; set; }
        public string CostSheetPCHStatus { get; set; }
        public string CostSheetCHStatus { get; set; }

        public int PK_QM_ID { get; set; }
        public string OBSType { get; set; }
        public string ClusterHeadName { get; set; }
        public string LostReason { get; set; }
        public string SendForApprovel { get; set; }
        public string SendForApprovelDate { get; set; }
        public string ApprovedOrNotApprovedDatePCH { get; set; }
        public string ApprovedOrNotApprovedDateCH { get; set; }

        public string SenderCommentDate { get; set; }
        public string PCHCommentDate { get; set; }
        public string CHCommentDate { get; set; }
        public string CompanyName { get; set; }
        public string costsheettype { get; set; }


    }

    public class IndividualPerformance
    {
        public string Utilization { get; set; }
        public string On_site_Marketing_visits { get; set; }
        public string OffsiteMarketingvisits { get; set; }
        public string NewCustomerVisits { get; set; }
        public string TotalCustomervisits { get; set; }
        public string Leaves { get; set; }
        public string LTI { get; set; }
        public string TrainingMandays { get; set; }
        public string WorkingMandays { get; set; }
        public string BuisnessDevelopment { get; set; }
        public string NewCustomeradded { get; set; }
        public string NewCustomerOrders { get; set; }
        public string NewCustomerOrderValue { get; set; }
        public string NoofLeadsInternal { get; set; }
        public string InternalLeadconversionratio_Nos { get; set; }
        public string Sales { get; set; }
        public string Enquiriesreceived { get; set; }
        public string Enquiriesinopenstate { get; set; }
        public string Enquiriesregretted { get; set; }
        public string Quotationsgenerated { get; set; }
        public string QuotationwithOpenStatus { get; set; }
        public string QuotationwithPartialStatus { get; set; }
        public string QuotationAutoApproved { get; set; }
        public string QuotationApprovedbyPCH { get; set; }
        public string QuotationApprovedbyCH { get; set; }
        public string QuotationApprovalPending { get; set; }
        public string Quotedamount { get; set; }
        public string WonorderValue { get; set; }
        public string Lostordervalue { get; set; }
        public string InprocessOrdervalue { get; set; }
        public string InProcessRateOrderValue { get; set; }
        public string HitRateordervalue { get; set; }
        public string LossRateOrderValue { get; set; }
        public string InProcessRateNos { get; set; }
        public string HitRateNos { get; set; }
        public string LossRateNos { get; set; }
        public string TIIMESUtilization { get; set; }
        public string EnquiryGeneratedontime { get; set; }
        public string QuotationGeneratedontime { get; set; }
        public string Ordersgeneratedontime { get; set; }


        public string SubHeadingName { get; set; }
        public string UOM { get; set; }
        public string Weightage { get; set; }
        public string MonthlyTarget { get; set; }
        public string TargetYTD { get; set; }
        public string CurrentMonth { get; set; }
        public string PreviousMonth { get; set; }
        public string YTD { get; set; }
        public string ScoreYTD { get; set; }

        public string PreviousYTD { get; set; }
        public string PreviousScoreYTD { get; set; }

        public int ScoreYTDColor { get; set; }
        public int ScorePreviousYTDColor { get; set; }
        public string Total { get; set; }


    }




}