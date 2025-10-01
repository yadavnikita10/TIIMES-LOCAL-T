using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;
using OfficeOpenXml;
using SelectPdf;
using System.IO;
using System.Text;
using NonFactors.Mvc.Grid;
using System.Net;
using System.Configuration;
using Newtonsoft.Json;
using System.Xml;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace TuvVision.Controllers
{
    public class Internal_Audit_ReportController : Controller
    {
        // GET: Internal_Audit_Report
        DALUsers objDalCreateUser = new DALUsers();
        DALNdt dalndt = new DALNdt();
        DALInternalAuditReport DAlAccess = new DALInternalAuditReport();
        Internal_Audit_Report InterAud = new Internal_Audit_Report();
        DALTrainingSchedule objDTS = new DALTrainingSchedule();

        NDTQualification ndtq = new NDTQualification();


        DALNdt objDALNdt = new DALNdt();
        DALAudit objDALAudit = new DALAudit();
        List<Internal_Audit_Report> lstCompanyDashBoard = new List<Internal_Audit_Report>();
        public PdfPageRotation Rotation { get; set; }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AuditreportDashBoard()
        {
            
            DataTable DTDash = new DataTable();
            DataSet dsGetAudit = new DataSet();
            DTDash = DAlAccess.GetAuditReportDashBoard();
            List<Internal_Audit_Report> lstReport = new List<Internal_Audit_Report>();
            try
            {
                if (DTDash.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTDash.Rows)
                    {
                        lstReport.Add(
                            new Internal_Audit_Report
                            {
                                AuditId = Convert.ToInt32(dr["AuditId"]),
                                Internal_Audit_Id = Convert.ToInt32(dr["Internal_Audit_Id"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                Auditor = Convert.ToString(dr["Auditor"]),
                                ExAuditor = Convert.ToString(dr["ExAuditor"]),
                                Auditee = Convert.ToString(dr["Auditee"]),
                                Department = Convert.ToString(dr["Department"]),
                                //SDate_Of_Audit = Convert.ToString(dr["Date_Of_Audit"]),

                                //ActualAuditDateFrom = Convert.ToString(dr["ActualAuditDateFrom"]),
                                ActualAuditDateTo = Convert.ToString(dr["ActualAuditDateTo"]),
                                ProposeDateFrom = Convert.ToString(dr["ProposeDateFrom"]),
                                ProposeDateTo = Convert.ToString(dr["ProposeDateTo"]),
                                //Nominated_By_Management_Remark = Convert.ToString(dr["Nominated_By_Management_Remark"]),
                                //Nominated_By_Management_NCR = Convert.ToString(dr["Nominated_By_Management_NCR"]),
                                //Findings_From_PreviousAudit_Remark = Convert.ToString(dr["Findings_From_PreviousAudit_Remark"]),
                                //Findings_From_PreviousAudit_NCR = Convert.ToString(dr["Findings_From_PreviousAudit_NCR"]),
                                //Customer_Complaints_Remarks = Convert.ToString(dr["Customer_Complaints_Remarks"]),
                                //Customer_Complaints_NCR = Convert.ToString(dr["Customer_Complaints_NCR"]),
                                //CustomerFeedBacknAnalysis_Remark = Convert.ToString(dr["CustomerFeedBacknAnalysis_Remark"]),
                                //CustomerFeedBacknAnalysis_NCR = Convert.ToString(dr["CustomerFeedBacknAnalysis_NCR"]),
                                //Process_Measures_Remarks = Convert.ToString(dr["Process_Measures_Remarks"]),
                                //Process_Measures_NCR = Convert.ToString(dr["Process_Measures_NCR"]),
                                //Training_AppraisalnKRAs_Remarks = Convert.ToString(dr["Training_AppraisalnKRAs_Remarks"]),
                                //Training_AppraisalnKRAs_NCR = Convert.ToString(dr["Training_AppraisalnKRAs_NCR"]),
                                //Enquiry_Management_Process_Remarks = Convert.ToString(dr["Enquiry_Management_Process_Remarks"]),
                                //Enquiry_Management_Process_NCR = Convert.ToString(dr["Enquiry_Management_Process_NCR"]),
                                //Quotation_Process_Remarks = Convert.ToString(dr["Quotation_Process_Remarks"]),
                                //Quotation_Process_NCR = Convert.ToString(dr["Quotation_Process_NCR"]),
                                //OrderReviewnAccepatanceContractreview_Remarks = Convert.ToString(dr["OrderReviewnAccepatanceContractreview_Remarks"]),
                                //OrderReviewnAccepatanceContractreview_NCR = Convert.ToString(dr["OrderReviewnAccepatanceContractreview_NCR"]),
                                //OrganisationStructure_Remarks = Convert.ToString(dr["OrganisationStructure_Remarks"]),
                                //OrganisationStructure_NCR = Convert.ToString(dr["OrganisationStructure_NCR"]),
                                //Control_Of_Documents_Remarks = Convert.ToString(dr["Control_Of_Documents_Remarks"]),
                                //Control_Of_Documents_NCR = Convert.ToString(dr["Control_Of_Documents_NCR"]),
                                //Control_Of_Records_Remarks = Convert.ToString(dr["Control_Of_Records_Remarks"]),
                                //Control_Of_Records_NCR = Convert.ToString(dr["Control_Of_Records_NCR"]),
                                //Competancy_Mapping_Remarks = Convert.ToString(dr["Competancy_Mapping_Remarks"]),
                                //Competancy_Mapping_NCR = Convert.ToString(dr["Competancy_Mapping_NCR"]),
                                //TrainingRecords_Effectiveness_Remarks = Convert.ToString(dr["TrainingRecords_Effectiveness_Remarks"]),
                                //TrainingRecords_Effectiveness_NCR = Convert.ToString(dr["TrainingRecords_Effectiveness_NCR"]),
                                //Impartiality_Confidentiality_Remarks = Convert.ToString(dr["Impartiality_Confidentiality_Remarks"]),
                                //Impartiality_Confidentiality_NCR = Convert.ToString(dr["Impartiality_Confidentiality_NCR"]),
                                //InspectionCordinationnInspectionProcess_Remarks = Convert.ToString(dr["InspectionCordinationnInspectionProcess_Remarks"]),
                                //InspectionCordinationnInspectionProcess_NCR = Convert.ToString(dr["InspectionCordinationnInspectionProcess_NCR"]),
                                //Onsite_Monitoring_Remarks = Convert.ToString(dr["Onsite_Monitoring_Remarks"]),
                                //Onsite_Monitoring_NCR = Convert.ToString(dr["Onsite_Monitoring_NCR"]),
                                //Document_Work_Review_Remarks = Convert.ToString(dr["Document_Work_Review_Remarks"]),
                                //Document_Work_Review_NCR = Convert.ToString(dr["Document_Work_Review_NCR"]),
                                //Safety_Of_Personnel_Remarks = Convert.ToString(dr["Safety_Of_Personnel_Remarks"]),
                                //Safety_Of_Personnel_NCR = Convert.ToString(dr["Safety_Of_Personnel_NCR"]),
                                //Planning_opportunities_Remarks = Convert.ToString(dr["Planning_opportunities_Remarks"]),
                                //Planning_opportunities_NCR = Convert.ToString(dr["Planning_opportunities_NCR"]),
                                // Plan_ref_No = Convert.ToInt32(dr["Plan_ref_No"]),
                                Plan_ref_No = Convert.ToString(dr["Plan_ref_No"]),
                                Total_NCR_raised = Convert.ToInt32(dr["TotalFindings"]),
                                PDF = Convert.ToString(dr["PDF"]),
                                NCDocument = Convert.ToString(dr["NCDocument"]),
                                SupportingDocument = Convert.ToString(dr["SupportingDocument"]),
                                //SDateOfAudit_TODate = Convert.ToString(dr["DateOfAudit_TODate"]),
                                IsAuditCompleted = Convert.ToString(dr["IsAuditCompleted"]),
                                AreFindingsClose = Convert.ToString(dr["AreFindingsClose"]),

                                CriticalNCCount = Convert.ToString(dr["CriticalNCCount"]),
                                MajorNCCount = Convert.ToString(dr["MajorNCCount"]),
                                MinorNCCount = Convert.ToString(dr["MinorNCCount"]),
                                OBSERVATIONCount = Convert.ToString(dr["OBSERVATIONCount"]),
                                ConcernCount = Convert.ToString(dr["ConcernCount"]),
                                OFICount = Convert.ToString(dr["OFICount"]),
                                //AdditionalRemarks = Convert.ToString(dr["AdditionalRemarks"]),


                            });
                    }
                }




            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            ViewData["dashboardreport"] = lstReport;
            InterAud.lstReport1 = lstReport;
            return View(InterAud);
        }
        [HttpGet]
        public ActionResult CreateInternal_Audit_Report(int? AuditId, string Edit)
        {

            //var Data1 = objDalCreateUser.GetCostCenterList();
            //ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");
            //List<NameCode> lstProjectType = new List<NameCode>();
            //DataSet dsCostCenter = new DataSet();
            //dsCostCenter = DAlAccess.GetCostSheet();
            //if (dsCostCenter.Tables[0].Rows.Count > 0)
            //{
            //    lstProjectType = (from n in dsCostCenter.Tables[0].AsEnumerable()
            //                      select new NameCode()
            //                      {
            //                          Name = n.Field<string>(dsCostCenter.Tables[0].Columns["Cost_Center"].ToString()),
            //                          Code = n.Field<Int32>(dsCostCenter.Tables[0].Columns["Pk_CC_Id"].ToString())

            //                      }).ToList();
            //}
            //IEnumerable<SelectListItem> OBSTypeItems;
            //OBSTypeItems = new SelectList(lstProjectType, "Code", "Name");

            //ViewBag.SubCatlist = lstProjectType;
            //ViewData["SubCatlist"] = OBSTypeItems;



            //DataTable DTGetUploadedFile = new DataTable();
            //List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            //DataTable DTGetUploadedFile1 = new DataTable();
            //List<FileDetails> lstEditFileDetails1 = new List<FileDetails>();
            List<NameCode> lstProjectType = new List<NameCode>();
            List<NameCode> lstScope = new List<NameCode>();
            DataSet DSGetAllddllst = new DataSet();
            DataSet dtIAFScope = new DataSet();
            DataSet DSEditGetList = new DataSet();

            List<NameCode> lstEditBranchList = new List<NameCode>();
            List<NameCode> lstEditUserList = new List<NameCode>();
            List<NameCode> lstEmploymentCategory = new List<NameCode>();
            List<NameCode> listTrainCate = new List<NameCode>();

            DSEditGetList = objDTS.GetDdlLst();

            //// OBS Types
            if (DSEditGetList.Tables[0].Rows.Count > 0)
            {
                lstProjectType = (from n in DSEditGetList.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DSEditGetList.Tables[0].Columns["ProjectName"].ToString()),
                                      Code = n.Field<Int32>(DSEditGetList.Tables[0].Columns["PK_ID"].ToString())

                                  }).ToList();
            }
            IEnumerable<SelectListItem> OBSTypeItems;
            OBSTypeItems = new SelectList(lstProjectType, "Code", "Name");

            ViewBag.OBSType = lstProjectType;
            ViewData["OBSType"] = OBSTypeItems;



            DataTable DTGetUploadedFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DataTable DTGetUploadedFile1 = new DataTable();
            List<FileDetails> lstEditFileDetails1 = new List<FileDetails>();



            DataTable DTedit = new DataTable();
            DataSet dsGetAudit = new DataSet();

            #region Bind Auditor Name
            DataSet dsAuditorName = new DataSet();
            //List<AuditorName> lstAuditorNamee = new List<AuditorName>();
            List<Audit> lstAuditorNamee = new List<Audit>();
            dsAuditorName = objDALAudit.BindAuditorName();

            if (dsAuditorName.Tables[0].Rows.Count > 0)
            {
                lstAuditorNamee = (from n in dsAuditorName.Tables[0].AsEnumerable()
                                   select new Audit()
                                   {
                                       DAuditorName = n.Field<string>(dsAuditorName.Tables[0].Columns["Name"].ToString()),
                                       DAuditorCode = n.Field<string>(dsAuditorName.Tables[0].Columns["Code"].ToString())

                                   }).ToList();
            }

            IEnumerable<SelectListItem> AuditorName;
            AuditorName = new SelectList(lstAuditorNamee, "DAuditorCode", "DAuditorName");
            ViewBag.AuditorName = AuditorName;
            ViewData["AuditorName"] = AuditorName;
            ViewData["AuditorName"] = lstAuditorNamee;
            #endregion

            if (Edit == "Edit")
            {

                #region Bind Dates
                DataTable dtDOrderType = new DataTable();
                List<Internal_Audit_Report> lstDOrderType = new List<Internal_Audit_Report>();

                dtDOrderType = DAlAccess.GetDates(AuditId);
                if (dtDOrderType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDOrderType.Rows)
                    {
                        lstDOrderType.Add(
                           new Internal_Audit_Report
                           {

                               SDate_Of_Audit = Convert.ToString(dr["DateSe"]),
                               OnSiteTime = Convert.ToString(dr["StartTime"]),
                               OffSiteTime = Convert.ToString(dr["EndTime"]),
                               TravelTime = Convert.ToString(dr["TravelTime"]),
                               
                           }
                         );
                    }
                    ViewBag.lstDOrderType = lstDOrderType;
                    ViewBag.Internal_id = Convert.ToString(dtDOrderType.Rows[0][1]);

                }
                #endregion


                #region Bind Activity Type
                DataSet ActivityType = new DataSet();
                dsGetAudit = objDALAudit.GetActivityTypeById(AuditId);
                if (dsGetAudit.Tables.Count > 0)
                {


                    foreach (DataRow dr in dsGetAudit.Tables[0].Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new Internal_Audit_Report
                            {
                                PKActivityTypeId = Convert.ToString(dr["PKId"]),
                                ActivityNameMaster = Convert.ToString(dr["Activity"]),
                                Remarks = Convert.ToString(dr["Remarks"]),
                                CriticalNC = Convert.ToString(dr["CriticalNC"]),
                                MajorNC = Convert.ToString(dr["MajorNC"]),
                                MinorNC = Convert.ToString(dr["MinorNC"]),
                                Observation = Convert.ToString(dr["Observation"]),
                                Concern = Convert.ToString(dr["Concern"]),
                                OFI = Convert.ToString(dr["OFI"]),
                                Clause1 = Convert.ToString(dr["ClauseNo1"]),
                                Clause2 = Convert.ToString(dr["ClauseNo2"]),

                                EvidenceChecked = Convert.ToString(dr["EvidenceChecked"]),
                                RCA = Convert.ToString(dr["RCA"]),
                                CA = Convert.ToString(dr["CA"]),
                                Correction = Convert.ToString(dr["Correction"]),
                                Auditorremarks = Convert.ToString(dr["Auditorremarks"])


                                
                                //None = Convert.ToString(dr["None"]),


                            }
                            );
                    }
                    ViewBag.CostSheet = lstCompanyDashBoard;
                    InterAud.lst1 = lstCompanyDashBoard;
                }


                #endregion


                ViewBag.checkAuditee = "Auditee";
                ViewBag.check = "AuditorName";
                string[] splitedProduct_Name;
                string[] splitedAuditorName;

                DTedit = DAlAccess.EditReport(AuditId);
                List<Internal_Audit_Report> lstReport = new List<Internal_Audit_Report>();
                //try
                //{
                if (DTedit.Rows.Count > 0)
                {
                    string LoginBy = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

                    string strList = Convert.ToString(DTedit.Rows[0]["AuditorId"]);

                    if (strList.Split(',').Contains(LoginBy))
                    {
                        InterAud.SaveButtonVisible = "Yes";
                    }
                    else
                    {
                        InterAud.SaveButtonVisible = "No";
                    }
                    InterAud.LoginBy = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);



                    foreach (DataRow dr in DTedit.Rows)
                    {
                        InterAud.AuditorId = Convert.ToString(dr["AuditorId"]);
                        //AuditId = Convert.ToInt32(dr["FkAuditId"]);
                        AuditId = Convert.ToInt32(dr["FkAuditId"]);
                        InterAud.Internal_Audit_Id = Convert.ToInt32(dr["Internal_Audit_Id"]);
                        InterAud.Branch = Convert.ToString(dr["Branch"]);
                        InterAud.Auditor = Convert.ToString(dr["Auditor"]);
                        InterAud.ExAuditor = Convert.ToString(dr["ExAuditor"]);
                        //InterAud.Plan_Ref_Serial_No = Convert.ToInt32(dr["Plan_Ref_Serial_No"]);
                        InterAud.Auditee = Convert.ToString(dr["Auditee"]);
                        InterAud.CostCenter_ = Convert.ToString(dr["CostCenter"]);

                        List<string> Selected = new List<string>();
                        var Existingins = Convert.ToString(dr["Auditee"]);
                        splitedProduct_Name = Existingins.Split(',');
                        foreach (var single in splitedProduct_Name)
                        {
                            Selected.Add(single);
                        }
                        ViewBag.EditproductName = Selected;

                        InterAud.Department = Convert.ToString(dr["Department"]);
                        //InterAud.Date_Of_Audit = Convert.ToDateTime(dr["Date_Of_Audit"]);
                        //InterAud.DateOfAudit_TODate = Convert.ToDateTime(dr["DateOfAudit_TODate"]);

                        InterAud.SDate_Of_Audit = Convert.ToString(dr["Date_Of_Audit"]);
                        InterAud.SDateOfAudit_TODate = Convert.ToString(dr["DateOfAudit_TODate"]);


                        //InterAud.Date_Of_Audit = Convert.ToDateTime(dr["ActualAuditDateFrom"]);

                        //InterAud.Nominated_By_Management_Remark = Convert.ToString(dr["Nominated_By_Management_Remark"]);
                        //InterAud.Nominated_By_Management_NCR = Convert.ToString(dr["Nominated_By_Management_NCR"]);
                        //InterAud.Findings_From_PreviousAudit_Remark = Convert.ToString(dr["Findings_From_PreviousAudit_Remark"]);
                        //InterAud.Findings_From_PreviousAudit_NCR = Convert.ToString(dr["Findings_From_PreviousAudit_NCR"]);
                        //InterAud.Customer_Complaints_Remarks = Convert.ToString(dr["Customer_Complaints_Remarks"]);
                        //InterAud.Customer_Complaints_NCR = Convert.ToString(dr["Customer_Complaints_NCR"]);
                        //InterAud.CustomerFeedBacknAnalysis_Remark = Convert.ToString(dr["CustomerFeedBacknAnalysis_Remark"]);
                        //InterAud.CustomerFeedBacknAnalysis_NCR = Convert.ToString(dr["CustomerFeedBacknAnalysis_NCR"]);
                        //InterAud.Process_Measures_Remarks = Convert.ToString(dr["Process_Measures_Remarks"]);
                        //InterAud.Process_Measures_NCR = Convert.ToString(dr["Process_Measures_NCR"]);
                        //InterAud.Training_AppraisalnKRAs_Remarks = Convert.ToString(dr["Training_AppraisalnKRAs_Remarks"]);
                        //InterAud.Training_AppraisalnKRAs_NCR = Convert.ToString(dr["Training_AppraisalnKRAs_NCR"]);
                        //InterAud.Enquiry_Management_Process_Remarks = Convert.ToString(dr["Enquiry_Management_Process_Remarks"]);
                        //InterAud.Enquiry_Management_Process_NCR = Convert.ToString(dr["Enquiry_Management_Process_NCR"]);
                        //InterAud.Quotation_Process_Remarks = Convert.ToString(dr["Quotation_Process_Remarks"]);
                        //InterAud.Quotation_Process_NCR = Convert.ToString(dr["Quotation_Process_NCR"]);
                        //InterAud.OrderReviewnAccepatanceContractreview_Remarks = Convert.ToString(dr["OrderReviewnAccepatanceContractreview_Remarks"]);
                        //InterAud.OrderReviewnAccepatanceContractreview_NCR = Convert.ToString(dr["OrderReviewnAccepatanceContractreview_NCR"]);
                        //InterAud.OrganisationStructure_Remarks = Convert.ToString(dr["OrganisationStructure_Remarks"]);
                        //InterAud.OrganisationStructure_NCR = Convert.ToString(dr["OrganisationStructure_NCR"]);
                        //InterAud.Control_Of_Documents_Remarks = Convert.ToString(dr["Control_Of_Documents_Remarks"]);
                        //InterAud.Control_Of_Documents_NCR = Convert.ToString(dr["Control_Of_Documents_NCR"]);
                        //InterAud.Control_Of_Records_Remarks = Convert.ToString(dr["Control_Of_Records_Remarks"]);
                        //InterAud.Control_Of_Records_NCR = Convert.ToString(dr["Control_Of_Records_NCR"]);
                        //InterAud.Competancy_Mapping_Remarks = Convert.ToString(dr["Competancy_Mapping_Remarks"]);
                        //InterAud.Competancy_Mapping_NCR = Convert.ToString(dr["Competancy_Mapping_NCR"]);
                        //InterAud.TrainingRecords_Effectiveness_Remarks = Convert.ToString(dr["TrainingRecords_Effectiveness_Remarks"]);
                        //InterAud.TrainingRecords_Effectiveness_NCR = Convert.ToString(dr["TrainingRecords_Effectiveness_NCR"]);
                        //InterAud.Impartiality_Confidentiality_Remarks = Convert.ToString(dr["Impartiality_Confidentiality_Remarks"]);
                        //InterAud.Impartiality_Confidentiality_NCR = Convert.ToString(dr["Impartiality_Confidentiality_NCR"]);
                        //InterAud.InspectionCordinationnInspectionProcess_Remarks = Convert.ToString(dr["InspectionCordinationnInspectionProcess_Remarks"]);
                        //InterAud.InspectionCordinationnInspectionProcess_NCR = Convert.ToString(dr["InspectionCordinationnInspectionProcess_NCR"]);
                        //InterAud.Onsite_Monitoring_Remarks = Convert.ToString(dr["Onsite_Monitoring_Remarks"]);
                        //InterAud.Onsite_Monitoring_NCR = Convert.ToString(dr["Onsite_Monitoring_NCR"]);
                        //InterAud.Document_Work_Review_Remarks = Convert.ToString(dr["Document_Work_Review_Remarks"]);
                        //InterAud.Document_Work_Review_NCR = Convert.ToString(dr["Document_Work_Review_NCR"]);
                        //InterAud.Safety_Of_Personnel_Remarks = Convert.ToString(dr["Safety_Of_Personnel_Remarks"]);
                        //InterAud.Safety_Of_Personnel_NCR = Convert.ToString(dr["Safety_Of_Personnel_NCR"]);
                        //InterAud.Total_NCR_raised = Convert.ToInt32(dr["Total_NCR_raised"]);
                        //InterAud.Planning_opportunities_Remarks = Convert.ToString(dr["Planning_opportunities_Remarks"]);
                        //InterAud.Planning_opportunities_NCR = Convert.ToString(dr["Planning_opportunities_NCR"]);
                        InterAud.Plan_ref_No = Convert.ToString(dr["Plan_ref_No"]);
                        //InterAud.Academic_records_Remarks = Convert.ToString(dr["Academic_records_Remarks"]);
                        //InterAud.Academic_records_NCR = Convert.ToString(dr["Academic_records_NCR"]);
                        //InterAud.NDE_certifications_Remark = Convert.ToString(dr["NDE_certifications_Remark"]);
                        //InterAud.NDE_certifications_NCR = Convert.ToString(dr["NDE_certifications_NCR"]);
                        //InterAud.Visual_Activity_Remarks = Convert.ToString(dr["Visual_Activity_Remarks"]);
                        //InterAud.Visual_activity_NCR = Convert.ToString(dr["Visual_activity_NCR"]);
                        InterAud.NCDocument = Convert.ToString(dr["NCDocument"]);
                        InterAud.SupportingDocument = Convert.ToString(dr["SupportingDocument"]);
                        
                        //InterAud.DateOfAudit_TODate = Convert.ToDateTime(dr["ActualAuditDateTo"]);
                        InterAud.IsAuditCompleted = Convert.ToString(dr["IsAuditCompleted"]);
                        InterAud.AreFindingsClose = Convert.ToString(dr["AreFindingsClose"]);

                        InterAud.CriticalNCCount = Convert.ToString(dr["CriticalNCCount"]);
                        InterAud.MajorNCCount = Convert.ToString(dr["MajorNCCount"]);
                        InterAud.MinorNCCount = Convert.ToString(dr["MinorNCCount"]);
                        InterAud.OBSERVATIONCount = Convert.ToString(dr["OBSERVATIONCount"]);
                        InterAud.ConcernCount = Convert.ToString(dr["ConcernCount"]);
                        InterAud.OFICount = Convert.ToString(dr["OFICount"]);
                        InterAud.AdditionalRemarks = Convert.ToString(dr["AdditionalRemarks"]);
                        InterAud.CostCenter_ = Convert.ToString(dr["CostCenter_"]);


                    }

                    #region Bind Attachment

                    DTGetUploadedFile = DAlAccess.EditUploadedFile(InterAud.Internal_Audit_Id);
                    if (DTGetUploadedFile.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFile.Rows)
                        {
                            lstEditFileDetails.Add(
                               new FileDetails
                               {

                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstEditFileDetails"] = lstEditFileDetails;
                        InterAud.FileDetails = lstEditFileDetails;
                    }

                    
                    DTGetUploadedFile1 = DAlAccess.EditUploadedFile1(InterAud.Internal_Audit_Id);
                    if (DTGetUploadedFile1.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFile1.Rows)
                        {
                            lstEditFileDetails1.Add(
                               new FileDetails
                               {

                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstEditFileDetails1"] = lstEditFileDetails1;
                        InterAud.FileDetails1 = lstEditFileDetails1;
                    }
                    #endregion
                }
                else
                { }

            }
            else
            {

                #region Bind Dates
                DataTable dtDOrderType1 = new DataTable();
                List<Internal_Audit_Report> lstDOrderType = new List<Internal_Audit_Report>();

                dtDOrderType1 = DAlAccess.GetScheduleDates(AuditId);
                if (dtDOrderType1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDOrderType1.Rows)
                    {
                        lstDOrderType.Add(
                           new Internal_Audit_Report
                           {

                               SDate_Of_Audit = Convert.ToString(dr["Date"]),
                               OnSiteTime = "",
                               TravelTime = "",
                               OffSiteTime="",
                           }
                         );
                    }
                    ViewBag.lstDOrderType = lstDOrderType;

                }
                #endregion


                #region Bind Activity Type
                DataSet ActivityType = new DataSet();
                dsGetAudit = objDALAudit.GetActivityType();
                if (dsGetAudit.Tables.Count > 0)
                {
                    

                    foreach (DataRow dr in dsGetAudit.Tables[0].Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new Internal_Audit_Report
                            {
                                PKActivityTypeId = Convert.ToString(dr["PKId"]),
                                ActivityNameMaster = Convert.ToString(dr["Activity"]),
                                Clause1 = Convert.ToString(dr["ClauseNo1"]),
                                Clause2 = Convert.ToString(dr["ClauseNo2"]),


                                Remarks = Convert.ToString(dr["Remarks"]),
                                CriticalNC = Convert.ToString(dr["CriticalNC"]),
                                MajorNC = Convert.ToString(dr["MajorNC"]),
                                MinorNC = Convert.ToString(dr["MinorNC"]),
                                Observation = Convert.ToString(dr["Observation"]),
                                Concern = Convert.ToString(dr["Concern"]),
                                OFI = Convert.ToString(dr["OFI"]),
                                None = Convert.ToString(dr["None"]),


                            }
                            );
                    }
                    ViewBag.CostSheet = lstCompanyDashBoard;
                    InterAud.lst1 = lstCompanyDashBoard;
                }


                #endregion

                #region Generate Unique no
                //int _min = 1000;
                //int _max = 9999;
                //Random _rdm = new Random();
                //int a = _rdm.Next(_min, _max);
                //string T = "IAR" + Convert.ToString(a);
                //objM.TiimesUIN = T;
                #endregion


                dsGetAudit = objDALAudit.GetDataByIdForReport(AuditId);
                if (dsGetAudit.Tables.Count > 0)
                {
                    ViewBag.checkAuditee = "Auditee";
                    ViewBag.check = "AuditorName";
                    string[] splitedProduct_Name;
                    string[] splitedAuditorName;

                    string LoginBy = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

                    string strList = Convert.ToString(dsGetAudit.Tables[0].Rows[0]["AuditorId"]);

                    if (strList.Split(',').Contains(LoginBy))
                    {
                        InterAud.SaveButtonVisible = "Yes";
                    }
                    else
                    {
                        InterAud.SaveButtonVisible = "No";
                    }
                    InterAud.LoginBy = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
                    foreach (DataRow dr in dsGetAudit.Tables[0].Rows)
                    {
                        //DateTime? myDate = Convert.ToDateTime(dr["ScheduleDate"]);
                        //string sqlFormattedDate = myDate.Value.ToString("yyyy-MM-dd");

                        InterAud.AuditorId = Convert.ToString(dr["AuditorId"]);
                        AuditId = Convert.ToInt32(dr["AuditId"]);
                        InterAud.Branch = Convert.ToString(dr["Branch"]);
                        InterAud.Auditor = Convert.ToString(dr["AuditorName"]);
                        InterAud.ExAuditor = Convert.ToString(dr["ExAuditor"]);
                        InterAud.Plan_ref_No = Convert.ToString(dr["Plan_ref_No"]);//Convert.ToInt32(dr["AuditId"]);
                                                                                   //InterAud.Auditee = Convert.ToString(dr["Auditee"]);
                        InterAud.CostCenter_ = Convert.ToString(dr["CostCenter"]); //


                        List<string> Selected = new List<string>();
                        var Existingins = Convert.ToString(dr["Auditee"]);
                        splitedProduct_Name = Existingins.Split(',');
                        foreach (var single in splitedProduct_Name)
                        {
                            Selected.Add(single);
                        }
                        ViewBag.EditproductName = Selected;

                        //Department = Convert.ToString(dr["Department"]),

                        //InterAud.Date_Of_Audit = Convert.ToDateTime(dr["ProposeDate"]);
                        //InterAud.DateOfAudit_TODate = Convert.ToDateTime(dr["ScheduleDate"]);
                        //InterAud.SDate_Of_Audit = Convert.ToString(dr["ProposeDate"]); //11 july 2023
                        //InterAud.SDateOfAudit_TODate = Convert.ToString(dr["ScheduleDate"]);
                    }
                }
            }




            //else
            //{

            //    else
            //    {

            //    }
            //}
            //Added by Ankush for Delete file and update file
            //DataTable DTGetUploadedFile = new DataTable();
            //List<FileDetails> lstEditFileDetails = new List<FileDetails>();

            DTGetUploadedFile = DAlAccess.EditUploadedFile(AuditId);
            if (DTGetUploadedFile.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetUploadedFile.Rows)
                {
                    lstEditFileDetails.Add(
                       new FileDetails
                       {

                           PK_ID = Convert.ToInt32(dr["PK_ID"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Extension = Convert.ToString(dr["Extenstion"]),
                           IDS = Convert.ToString(dr["FileID"]),
                       }
                     );
                }
                ViewData["lstEditFileDetails"] = lstEditFileDetails;
                InterAud.FileDetails = lstEditFileDetails;
            }

            //DataTable DTGetUploadedFile1 = new DataTable();
            //List<FileDetails> lstEditFileDetails1 = new List<FileDetails>();
            DTGetUploadedFile1 = DAlAccess.EditUploadedFile1(AuditId);
            if (DTGetUploadedFile1.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetUploadedFile1.Rows)
                {
                    lstEditFileDetails1.Add(
                       new FileDetails
                       {

                           PK_ID = Convert.ToInt32(dr["PK_ID"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Extension = Convert.ToString(dr["Extenstion"]),
                           IDS = Convert.ToString(dr["FileID"]),
                       }
                     );
                }
                ViewData["lstEditFileDetails1"] = lstEditFileDetails1;
                InterAud.FileDetails1 = lstEditFileDetails1;
            }
            //Added by Ankush for Delete file and update file

            //}
            //catch (Exception ex)
            //{
            //    string msg = ex.Message.ToString();
            //}
            return View(InterAud);
        }
        [HttpPost]
        public ActionResult CreateInternal_Audit_Report(Internal_Audit_Report ObjModelVisitReport, FormCollection formCollection, List<HttpPostedFileBase> img_Banner, FormCollection fc, List<Internal_Audit_Report> DArray , List<Internal_Audit_Report> DActivity)
        {
            
            string[] Date_Of_Audit = formCollection.GetValues("Date_Of_Audit");
            string[] OnSiteTime = formCollection.GetValues("OnSiteTime");
            string[] TravelTime = formCollection.GetValues("TravelTime");

            #region
            //List<Internal_Audit_Report> rows = new List<Internal_Audit_Report>();

            //// Iterate over the form collection
            //foreach (string key in formCollection.AllKeys)
            //{
            //    if (key.StartsWith("OnSiteTime"))
            //    {
            //        // Extract the index from the key
            //        //int index = int.Parse(key.Split('/')[1]);

            //        // Get the corresponding values
            //        string name = formCollection["TravelTime"];
            //        string age = formCollection["TravelTime"];

            //        // Create a new row and populate it with the values
            //        Internal_Audit_Report row = new Internal_Audit_Report
            //        {
            //            SDate_Of_Audit = name,
            //            OnSiteTime = age
            //            // Set other properties as needed
            //        };

            //        // Add the row to the list
            //        rows.Add(row);
            //    }
            //}
            #endregion





            //////string ProList = string.Join(",", fc["AuditeeName"]);
            //////ObjModelVisitReport.Auditee = ProList;

            //////string CostCenter = string.Join(",", fc["IdOBSTypeName"]);
            //////ObjModelVisitReport.CostCenter = CostCenter;

            int IARID = 0;
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listIARUploadedFile"] as List<FileDetails>;
            int IARID1 = 0;
            List<FileDetails> lstFileDtls1 = new List<FileDetails>();
            lstFileDtls1 = Session["listIARUploadedFile1"] as List<FileDetails>;

            string Result = string.Empty;
            string IPath = string.Empty;
            var list = Session["list"] as List<string>;
            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }
            string IPathSupportingDocument = string.Empty;
            var listSupportingDocument = Session["listSupportingDocument"] as List<string>;
            if (listSupportingDocument != null && listSupportingDocument.Count != 0)
            {
                IPathSupportingDocument = string.Join(",", listSupportingDocument.ToList());
                IPathSupportingDocument = IPathSupportingDocument.TrimEnd(',');
            }

            //Result = DAlAccess.InsertUpdateforAuditReport(ObjModelVisitReport);
            try
            {

                #region Document NC
                List<string> lstAttachment = new List<string>();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                {
                    foreach (HttpPostedFileBase single in img_Banner) // Added by Sagar Panigrahi
                    {
                        //HttpPostedFileBase Imagesection;
                        //Imagesection = Request.Files[single];
                        if (single != null && single.FileName != "")
                        {
                            var filename = CommonControl.FileUpload("~/AuditReportNCDocument/", single);
                            lstAttachment.Add(filename);
                        }
                    }
                    ObjModelVisitReport.NCDocument = string.Join(",", lstAttachment);
                    if (string.IsNullOrEmpty(ObjModelVisitReport.NCDocument))
                    {
                        ObjModelVisitReport.NCDocument = "NoImage.gif";
                    }
                }
                else
                {
                    ObjModelVisitReport.NCDocument = "NoImage.gif";
                }
                #endregion

                #region Other related document
                List<string> lstAttachmentOther = new List<string>();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                {
                    foreach (HttpPostedFileBase single in img_Banner) // Added by Sagar Panigrahi
                    {
                        //HttpPostedFileBase Imagesection;
                        //Imagesection = Request.Files[single];
                        if (single != null && single.FileName != "")
                        {
                            var filename = CommonControl.FileUpload("~/AuditReportRelatedDocument/", single);
                            lstAttachmentOther.Add(filename);
                        }
                    }
                    ObjModelVisitReport.SupportingDocument = string.Join(",", lstAttachmentOther);
                    if (string.IsNullOrEmpty(ObjModelVisitReport.NCDocument))
                    {
                        ObjModelVisitReport.SupportingDocument = "NoImage.gif";
                    }
                }
                else
                {
                    ObjModelVisitReport.SupportingDocument = "NoImage.gif";
                }
                #endregion




                #region get Auditor Signature
                if (ObjModelVisitReport != null)
                // ViewBag.AuditorSign = DAlAccess.GetSignature(ObjModelVisitReport.AuditorId);
                {
                    DataTable dtSign = new DataTable();
                    dtSign = DAlAccess.GetSignature(ObjModelVisitReport.AuditorId);
                    if (dtSign.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtSign.Rows)
                        {
                            ObjModelVisitReport.AuditorSignature = Convert.ToString(dr["Signature"]);
                        }
                    }
                    //ObjModelVisitReport.AuditorSignature = Convert.ToString(ViewBag.AuditorSign);
                    #endregion End Auditor Signature

                    #region Save to Pdf Code 

                //    string DOA = Convert.ToString(ObjModelVisitReport.Date_Of_Audit);

                //    System.Text.StringBuilder strs = new System.Text.StringBuilder();
                //    string body = string.Empty;
                //    string Plan_ref_No = Convert.ToString(ObjModelVisitReport.Plan_ref_No);

                //    using (StreamReader reader = new StreamReader(Server.MapPath("~/internal-audit-report.html")))
                //    {
                //        body = reader.ReadToEnd();
                //    }

                //    body = body.Replace("[Branch]", ObjModelVisitReport.Branch);

                //    body = body.Replace("[Plan_ref_No]", Plan_ref_No);
                //    body = body.Replace("[Auditor]", ObjModelVisitReport.Auditor);
                //    body = body.Replace("[ExAuditor]", ObjModelVisitReport.ExAuditor);
                //    body = body.Replace("[Date_Of_Audit]", DOA);
                //    body = body.Replace("[Auditee]", ObjModelVisitReport.Auditee);
                //    body = body.Replace("[Department]", ObjModelVisitReport.Department);
                //    body = body.Replace("[Nominated_By_Management_Remark]", ObjModelVisitReport.Nominated_By_Management_Remark);
                //    body = body.Replace("[Nominated_By_Management_NCR]", ObjModelVisitReport.Nominated_By_Management_NCR);
                //    body = body.Replace("[Findings_From_PreviousAudit_Remark]", ObjModelVisitReport.Findings_From_PreviousAudit_Remark);
                //    body = body.Replace("[Findings_From_PreviousAudit_NCR]", ObjModelVisitReport.Findings_From_PreviousAudit_NCR);
                //    body = body.Replace("[Customer_Complaints_Remarks]", ObjModelVisitReport.Customer_Complaints_Remarks);
                //    body = body.Replace("[Customer_Complaints_NCR]", ObjModelVisitReport.Customer_Complaints_NCR);
                //    body = body.Replace("[CustomerFeedBacknAnalysis_Remark]", ObjModelVisitReport.CustomerFeedBacknAnalysis_Remark);
                //    body = body.Replace("[CustomerFeedBacknAnalysis_NCR]", ObjModelVisitReport.CustomerFeedBacknAnalysis_NCR);
                //    body = body.Replace("[Process_Measures_Remarks]", ObjModelVisitReport.Process_Measures_Remarks);
                //    body = body.Replace("[Process_Measures_NCR]", ObjModelVisitReport.Process_Measures_NCR);
                //    body = body.Replace("[Training_AppraisalnKRAs_Remarks]", ObjModelVisitReport.Training_AppraisalnKRAs_Remarks);
                //    body = body.Replace("[Training_AppraisalnKRAs_NCR]", ObjModelVisitReport.Training_AppraisalnKRAs_NCR);
                //    body = body.Replace("[Enquiry_Management_Process_Remarks]", ObjModelVisitReport.Enquiry_Management_Process_Remarks);
                //    body = body.Replace("[Enquiry_Management_Process_NCR]", ObjModelVisitReport.Enquiry_Management_Process_NCR);
                //    body = body.Replace("[Quotation_Process_Remarks]", ObjModelVisitReport.Quotation_Process_Remarks);
                //    body = body.Replace("[Quotation_Process_NCR]", ObjModelVisitReport.Quotation_Process_NCR);
                //    body = body.Replace("[OrderReviewnAccepatanceContractreview_Remarks]", ObjModelVisitReport.OrderReviewnAccepatanceContractreview_Remarks);
                //    body = body.Replace("[OrderReviewnAccepatanceContractreview_NCR]", ObjModelVisitReport.OrderReviewnAccepatanceContractreview_NCR);
                //    body = body.Replace("[OrganisationStructure_Remarks]", ObjModelVisitReport.OrganisationStructure_Remarks);
                //    body = body.Replace("[OrganisationStructure_NCR]", ObjModelVisitReport.OrganisationStructure_NCR);
                //    body = body.Replace("[Control_Of_Documents_Remarks]", ObjModelVisitReport.Control_Of_Documents_Remarks);
                //    body = body.Replace("[Control_Of_Documents_NCR]", ObjModelVisitReport.Control_Of_Documents_NCR);
                //    body = body.Replace("[Control_Of_Records_Remarks]", ObjModelVisitReport.Control_Of_Records_Remarks);
                //    body = body.Replace("[Control_Of_Records_NCR]", ObjModelVisitReport.Control_Of_Records_NCR);
                //    body = body.Replace("[Competancy_Mapping_Remarks]", ObjModelVisitReport.Competancy_Mapping_Remarks);
                //    body = body.Replace("[Competancy_Mapping_NCR]", ObjModelVisitReport.Competancy_Mapping_NCR);
                //    body = body.Replace("[Academic_records_Remarks]", ObjModelVisitReport.Academic_records_Remarks);
                //    body = body.Replace("[Academic_records_NCR]", ObjModelVisitReport.Academic_records_NCR);
                //    body = body.Replace("[NDE_certifications_Remark]", ObjModelVisitReport.NDE_certifications_Remark);
                //    body = body.Replace("[NDE_certifications_NCR]", ObjModelVisitReport.NDE_certifications_NCR);
                //    body = body.Replace("[Visual_Activity_Remarks]", ObjModelVisitReport.Visual_Activity_Remarks);
                //    body = body.Replace("[Visual_activity_NCR]", ObjModelVisitReport.Visual_activity_NCR);
                //    body = body.Replace("[TrainingRecords_Effectiveness_Remarks]", ObjModelVisitReport.TrainingRecords_Effectiveness_Remarks);
                //    body = body.Replace("[TrainingRecords_Effectiveness_NCR]", ObjModelVisitReport.TrainingRecords_Effectiveness_NCR);
                //    body = body.Replace("[Impartiality_Confidentiality_Remarks]", ObjModelVisitReport.Impartiality_Confidentiality_Remarks);
                //    body = body.Replace("[Impartiality_Confidentiality_NCR]", ObjModelVisitReport.Impartiality_Confidentiality_NCR);
                //    body = body.Replace("[InspectionCordinationnInspectionProcess_Remarks]", ObjModelVisitReport.InspectionCordinationnInspectionProcess_Remarks);
                //    body = body.Replace("[InspectionCordinationnInspectionProcess_NCR]", ObjModelVisitReport.InspectionCordinationnInspectionProcess_NCR);
                //    body = body.Replace("[Onsite_Monitoring_Remarks]", ObjModelVisitReport.Onsite_Monitoring_Remarks);
                //    body = body.Replace("[Onsite_Monitoring_NCR]", ObjModelVisitReport.Onsite_Monitoring_NCR);
                //    body = body.Replace("[Document_Work_Review_Remarks]", ObjModelVisitReport.Document_Work_Review_Remarks);
                //    body = body.Replace("[Document_Work_Review_NCR]", ObjModelVisitReport.Document_Work_Review_NCR);
                //    body = body.Replace("[Planning_opportunities_Remarks]", ObjModelVisitReport.Planning_opportunities_Remarks);
                //    body = body.Replace("[Planning_opportunities_NCR]", ObjModelVisitReport.Planning_opportunities_NCR);
                //    body = body.Replace("[Safety_Of_Personnel_Remarks]", ObjModelVisitReport.Safety_Of_Personnel_Remarks);
                //    body = body.Replace("[Safety_Of_Personnel_NCR]", ObjModelVisitReport.Safety_Of_Personnel_NCR);

                   
                //    List<Internal_Audit_Report> lstActivityMaster = new List<Internal_Audit_Report>();

                //    DataSet ds = new DataSet();

                //   foreach (var item in ObjModelVisitReport.Activity)
                //    {
                //        lstActivityMaster.Add(
                //            new Internal_Audit_Report
                //            {

                //                PKActivityTypeId = item.PKActivityTypeId,
                //                FkAuditReportId = ObjModelVisitReport.FkAuditReportId,
                //                Remarks = item.Remarks,
                //                CriticalNC = item.CriticalNC,
                //                MajorNC = item.MajorNC,
                //                MinorNC = item.MinorNC,
                //                Observation = item.Observation,
                //                Concern = item.Concern,
                //                OFI = item.OFI,
                //                ActivityNameMaster = item.ActivityNameMaster

                //            });
                //}
                //    int i = 0;
                //    string ItemDescriptioncontent = "";
                //    foreach (Internal_Audit_Report v in lstActivityMaster)
                //    {
                //        i = i + 1;

                //           ItemDescriptioncontent += "<tr><td style = ' border:1px solid #000000;vertical-align:top; text-align:center;border-bottom-width: 0px;font-size:14px;' width = '5%' align = 'center' ><span> " + Convert.ToString(v.ActivityNameMaster) /*+ ''*/ + " </span></td><td style = 'border:1px solid #000000;vertical-align:top; text-align:left;border-left-width: 0px;border-bottom-width: 0px;font-size:14px;' width = '10%' ><span> " + v.Remarks + " </span></td><td style = 'border:1px solid #000000;vertical-align:top; text-align:left;border-left-width: 0px;border-bottom-width: 0px;font-size:14px;white-space: pre-line;' width = '35%' ><span> " + v.Remarks + " </span></td></tr>";
                       
                //    }
                //    body = body.Replace("[ItemDescriptionContent]", ItemDescriptioncontent);
                    

                //    string ImageContent = null;
                //    if (ObjModelVisitReport.AuditorSignature != null)
                //    {
                //        foreach (string v in ObjModelVisitReport.AuditorSignature.Split(','))
                //        {
                //            ImageContent += "<tr><td><img src='http://10.10.30.35:8080/TuvVision" + v + "' style='width: 200px; height:40px' alt=''></td></tr>";
                //        }
                //        body = body.Replace("[Auditor_Signature]", ImageContent);
                //    }
                //    body = body.Replace("[CriticalNCCount]", ObjModelVisitReport.CriticalNCCount);
                //    body = body.Replace("[MajorNCCount]", ObjModelVisitReport.MajorNCCount);
                //    body = body.Replace("[MinorNCCount]", ObjModelVisitReport.MinorNCCount);
                //    body = body.Replace("[OBSERVATIONCount]", ObjModelVisitReport.OBSERVATIONCount);
                //    body = body.Replace("[ConcernCount]", ObjModelVisitReport.ConcernCount);
                //    body = body.Replace("[OFICount]", ObjModelVisitReport.OFICount);

                //    string charncr = Convert.ToString(ObjModelVisitReport.Total_NCR_raised);
                //    body = body.Replace("[Total_NCR_raised]", charncr);
                //    DateTime dtnw = DateTime.Now;
                //    string ccns = Convert.ToString(dtnw);
                //    body = body.Replace("[TodaysDate]", ccns);
                //    body = body.Replace("[Logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.png");

                //    strs.Append(body);
                //    PdfPageSize pageSize = PdfPageSize.A4;
                //    PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
                //    HtmlToPdf converter = new HtmlToPdf();
                //    converter.Options.MaxPageLoadTime = 180;  //=========================5-Aug-2019
                //    converter.Options.PdfPageSize = pageSize;
                //    converter.Options.PdfPageOrientation = pdfOrientation;
                //    PdfDocument doc = converter.ConvertHtmlString(body);
                //    string ReportName = ObjModelVisitReport.Plan_ref_No + ".pdf";
                //    string path = Server.MapPath("~/IAR");
                //    doc.Save(path + '\\' + ReportName);
                //    doc.Close();
                    #endregion
                    string ReportName = ObjModelVisitReport.Plan_ref_No + ".pdf";
                    ObjModelVisitReport.PDF = ReportName;


                    Result = DAlAccess.InsertUpdateforAuditReport(ObjModelVisitReport, IPath, IPathSupportingDocument);
                    ObjModelVisitReport.FkAuditReportId = Result;

                    #region Insert Date
                    if (DArray != null)
                    {
                        foreach (var d in DArray)
                        {

                            ObjModelVisitReport.SDate_Of_Audit = d.SDate_Of_Audit;
                            ObjModelVisitReport.OnSiteTime = d.OnSiteTime;
                            ObjModelVisitReport.TravelTime = d.TravelTime;
                            ObjModelVisitReport.OffSiteTime = d.OffSiteTime;
                            // ObjModelVisitReport.FkAuditReportId = Convert.ToString(ObjModelVisitReport.Internal_Audit_Id);
                            ObjModelVisitReport.FkAuditReportId = ObjModelVisitReport.FkAuditReportId;
                            int Result1 = DAlAccess.InsertAuditDate(ObjModelVisitReport);
                        }
                    }
                    #endregion

                    if (Result!=null)
                    {
                        //foreach (var item in ObjModelVisitReport.Activity)
                        //{
                        //    ObjModelVisitReport.PKActivityTypeId = item.PKActivityTypeId;
                        //    ObjModelVisitReport.FkAuditReportId = ObjModelVisitReport.FkAuditReportId;
                        //    ObjModelVisitReport.Remarks = item.Remarks;
                        //    ObjModelVisitReport.CriticalNC = item.CriticalNC;
                        //    ObjModelVisitReport.MajorNC = item.MajorNC;
                        //    ObjModelVisitReport.MinorNC = item.MinorNC;
                        //    ObjModelVisitReport.Observation = item.Observation;
                        //    ObjModelVisitReport.Concern = item.Concern;
                        //    ObjModelVisitReport.OFI = item.OFI;


                        //    ObjModelVisitReport.EvidenceChecked = item.EvidenceChecked;
                        //    ObjModelVisitReport.RCA = item.RCA;
                        //    ObjModelVisitReport.CA = item.CA;
                        //    ObjModelVisitReport.Correction = item.Correction;
                        //    ObjModelVisitReport.Auditorremarks = item.Auditorremarks;

                        //    Result = DAlAccess.InsertActivityMasterWithData(ObjModelVisitReport);
                        //}

                        foreach (var item in DActivity)
                        {
                            ObjModelVisitReport.FkAuditReportId = ObjModelVisitReport.FkAuditReportId;
                            ObjModelVisitReport.PKActivityTypeId = item.PKActivityTypeId;
                          ObjModelVisitReport.EvidenceChecked = item.EvidenceChecked ;
                          ObjModelVisitReport.Remarks = item.Remarks       ;
                          ObjModelVisitReport.CriticalNC = item.CriticalNC     ;
                          ObjModelVisitReport.MajorNC = item.MajorNC;
                          ObjModelVisitReport.MinorNC = item.MinorNC;
                          ObjModelVisitReport.Observation = item.Observation;
                          ObjModelVisitReport.Concern = item.Concern;
                          ObjModelVisitReport.OFI = item.OFI;
                          ObjModelVisitReport.RCA = item.RCA;
                          ObjModelVisitReport.CA = item.CA;
                          ObjModelVisitReport.Correction = item.Correction      ;
                          ObjModelVisitReport.Auditorremarks = item.Auditorremarks;
                          Result = DAlAccess.InsertActivityMasterWithData(ObjModelVisitReport);
                        }

                           

                            
                        


                    }
                    PrintPDF(ObjModelVisitReport.FkAuditReportId);
                    Result = DAlAccess.UpdatePDF(ObjModelVisitReport);

                    if (ObjModelVisitReport.AuditId != null && ObjModelVisitReport.AuditId != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = DAlAccess.InsertFileAttachment(lstFileDtls, Convert.ToInt32(ObjModelVisitReport.AuditId));
                            Session["listIARUploadedFile"] = null;
                        }
                    }
                    else
                    {
                        ObjModelVisitReport.AuditId = Convert.ToInt32(Session["IARIDs"]);
                        if (ObjModelVisitReport.AuditId != null && ObjModelVisitReport.AuditId != 0)
                        {
                            if (lstFileDtls != null && lstFileDtls.Count > 0)
                            {
                                Result = DAlAccess.InsertFileAttachment(lstFileDtls, ObjModelVisitReport.AuditId);
                                Session["listIARUploadedFile"] = null;
                            }
                        }
                    }
                    if (ObjModelVisitReport.AuditId != null && ObjModelVisitReport.AuditId != 0)
                    {
                        if (lstFileDtls1 != null && lstFileDtls1.Count > 0)
                        {
                            Result = DAlAccess.InsertFileAttachment1(lstFileDtls1, Convert.ToInt32(ObjModelVisitReport.AuditId));
                            Session["listIARUploadedFile1"] = null;
                        }
                    }
                    else
                    {
                        ObjModelVisitReport.AuditId = Convert.ToInt32(Session["IARIDs"]);
                        if (ObjModelVisitReport.AuditId != null && ObjModelVisitReport.AuditId != 0)
                        {
                            if (lstFileDtls1 != null && lstFileDtls1.Count > 0)
                            {
                                Result = DAlAccess.InsertFileAttachment1(lstFileDtls1, ObjModelVisitReport.AuditId);
                                Session["listIARUploadedFile1"] = null;
                            }
                        }
                    }

                    TempData["insertupdate"] = Result;
                    //return RedirectToAction("AuditreportDashBoard", "Internal_Audit_Report");
                    return Json(new { result = "Redirect", url = Url.Action("CreateInternal_Audit_Report", "Internal_Audit_Report", new { Edit = "Edit" , AuditId = ObjModelVisitReport.FkAuditReportId })   });
                    //return RedirectToAction("CreateInternal_Audit_Report", "Internal_Audit_Report", new { AuditId = ObjModelVisitReport.FkAuditReportId, Edit = "Edit" });
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
            }

            //return RedirectToAction("CreateInternal_Audit_Report", "Internal_Audit_Report", new { AuditId = ObjModelVisitReport.FkAuditReportId, Edit = "Edit" });
            //return RedirectToAction("AuditreportDashBoard", "Internal_Audit_Report");
            return Json(new { result = "Redirect", url = Url.Action("CreateInternal_Audit_Report", "Internal_Audit_Report", new { Edit = "Edit", AuditId = ObjModelVisitReport.FkAuditReportId }) });
        }

        
        public void PrintPDF(String AuditReportId)
        {
            #region Save to Pdf Code 
            DataTable dt = new DataTable();
            dt = DAlAccess.GetReportForPDF(AuditReportId);
            List<Internal_Audit_Report> lstReport = new List<Internal_Audit_Report>();
            //try
            //{
            if (dt.Rows.Count > 0)
            {

                InterAud.SDateOfAudit_TODate = Convert.ToString(dt.Rows[0]["Date_Of_Audit"]);
                InterAud.Plan_ref_No = Convert.ToString(dt.Rows[0]["plan_ref_No"]);
                InterAud.Branch = Convert.ToString(dt.Rows[0]["Branch"]);

                InterAud.Auditor = Convert.ToString(dt.Rows[0]["Auditor"]);
                InterAud.ExAuditor = Convert.ToString(dt.Rows[0]["ExAuditor"]);
                InterAud.Auditee = Convert.ToString(dt.Rows[0]["AuditeeName"]);
                InterAud.Department = Convert.ToString(dt.Rows[0]["Department"]);
                InterAud.CriticalNCCount = Convert.ToString(dt.Rows[0]["CriticalNCCount"]);
                InterAud.MajorNCCount = Convert.ToString(dt.Rows[0]["MajorNCCount"]);
                InterAud.MinorNCCount = Convert.ToString(dt.Rows[0]["MinorNCCount"]);
                InterAud.OBSERVATIONCount = Convert.ToString(dt.Rows[0]["OBSERVATIONCount"]);
                InterAud.ConcernCount = Convert.ToString(dt.Rows[0]["ConcernCount"]);
                InterAud.OFICount = Convert.ToString(dt.Rows[0]["OFICount"]);
                InterAud.AuditorSignature = Convert.ToString(dt.Rows[0]["AuditorSignature"]);
                InterAud.TotalFindings = Convert.ToString(dt.Rows[0]["TotalFindings"]);

               

  
  
  
  
  
            }


                //string DOA = Convert.ToString(ObjModelVisitReport.Date_Of_Audit);

            System.Text.StringBuilder strs = new System.Text.StringBuilder();
            string body = string.Empty;


            using (StreamReader reader = new StreamReader(Server.MapPath("~/internal-audit-report.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("[Branch]", InterAud.Branch);
            string I = "<img src = '" + ConfigurationManager.AppSettings["Web"].ToString() + "/Content/Sign/" + InterAud.AuditorSignature + "' style='width:225px;height:125px; ' align='center'>";
            body = body.Replace("[Signature]", I);
            body = body.Replace("[Plan_ref_No]", Convert.ToString(InterAud.Plan_ref_No));
            body = body.Replace("[Auditor]", InterAud.Auditor);
            body = body.Replace("[ExAuditor]", InterAud.ExAuditor);
            //  body = body.Replace("[Date_Of_Audit]", ObjModelVisitReport.Date_Of_Audit);
            body = body.Replace("[Date_Of_Audit]", InterAud.SDateOfAudit_TODate);
            body = body.Replace("[Auditee]", InterAud.Auditee);
            body = body.Replace("[Department]", InterAud.Department);
            body = body.Replace("[TotalFindings]", InterAud.TotalFindings);


            #region print activity master to PDF
            List<Internal_Audit_Report> lstActivityMaster = new List<Internal_Audit_Report>();

            DataSet ds = new DataSet();


            ds = objDALAudit.GetActivityTypeByIdForPDF(Convert.ToInt32(AuditReportId));

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstActivityMaster.Add(
                    new Internal_Audit_Report
                    {

                        ActivityNameMaster = Convert.ToString(dr["Activity"]),
                        Clause1 = Convert.ToString(dr["ClauseNo1"]),
                        Clause2 = Convert.ToString(dr["ClauseNo2"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        CriticalNCCount = Convert.ToString(dr["NC"]),

                        EvidenceChecked = Convert.ToString(dr["EvidenceChecked"]),
                        RCA = Convert.ToString(dr["RCA"]),
                        CA = Convert.ToString(dr["CA"]),
                        Correction = Convert.ToString(dr["Correction"]),
                        Auditorremarks = Convert.ToString(dr["Auditorremarks"])
                        
            });
            }
                
            int i = 0;
            string ItemDescriptioncontent = "";
            foreach (Internal_Audit_Report v in lstActivityMaster)
            {
                i = i + 1;

                //ItemDescriptioncontent += "<tr><td style = ' border:1px solid #000000;vertical-align:top; text-align:center;border-bottom-width: 0px;font-size:14px;' width = '5%' align = 'center' ><span> " + Convert.ToString(v.ActivityNameMaster) /*+ ''*/ + " </span></td><td style = 'border:1px solid #000000;vertical-align:top; text-align:left;border-left-width: 0px;border-bottom-width: 0px;font-size:14px;' width = '10%' ><span> " + v.Clause1 + " </span></td><td style = 'border:1px solid #000000;vertical-align:top; text-align:left;border-left-width: 0px;border-bottom-width: 0px;font-size:14px;white-space: pre-line;' width = '35%' ><span> " + v.Clause2 + " </span></td></tr>";
                ItemDescriptioncontent += "<tr><td>"+ i + "</td><td><span> " + Convert.ToString(v.ActivityNameMaster) /*+ ''*/ + " </span></td><td><span> " + v.Clause1 + " </span></td><td><span> " + v.Clause2 + " </span></td><td><span> " + v.EvidenceChecked + " </span></td><td><span> " + v.Remarks + " </span></td><td><span> " + v.CriticalNCCount + " </span></td><td><span> " + v.RCA + " </span></td><td><span> " + v.CA + " </span></td><td><span> " + v.Correction + " </span></td><td><span> " + v.Auditorremarks + " </span></td></tr>";
            }
            body = body.Replace("[ItemDescriptionContent]", ItemDescriptioncontent);
            #endregion


            
                 //string ImageContent = null;
                 //if (ObjModelVisitReport.AuditorSignature != null)
                 //{
                 //    foreach (string v in ObjModelVisitReport.AuditorSignature.Split(','))
                 //    {
                 //        ImageContent += "<tr><td><img src='http://10.10.30.35:8080/TuvVision" + v + "' style='width: 200px; height:40px' alt=''></td></tr>";
                 //    }
                 //    body = body.Replace("[Auditor_Signature]", ImageContent);
                 //}
                 body = body.Replace("[CriticalNCCount]", InterAud.CriticalNCCount);
            body = body.Replace("[MajorNCCount]", InterAud.MajorNCCount);
            body = body.Replace("[MinorNCCount]", InterAud.MinorNCCount);
            body = body.Replace("[OBSERVATIONCount]", InterAud.OBSERVATIONCount);
            body = body.Replace("[ConcernCount]", InterAud.ConcernCount);
            body = body.Replace("[OFICount]", InterAud.OFICount);
            string charncr = Convert.ToString(InterAud.Total_NCR_raised);
            body = body.Replace("[Total_NCR_raised]", charncr);
            DateTime dtnw = DateTime.Now;
            string ccns = Convert.ToString(dtnw);
            body = body.Replace("[TodaysDate]", ccns);
            body = body.Replace("[Logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.png");

           


            strs.Append(body);
            PdfPageSize pageSize = PdfPageSize.A4;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
            HtmlToPdf converter = new HtmlToPdf();
            // set the page timeout (in seconds)
            converter.Options.MaxPageLoadTime = 180;  //=========================5-Aug-2019
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;

            #region Header & footer
            string _Header = string.Empty;
            string _footer = string.Empty;


            StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/internal-audit-reportHeader.html"));
            _Header = _readHeader_File.ReadToEnd();
            //_Header = _Header.Replace("[logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png");
           
            _Header = _Header.Replace("[Logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.png");


            StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/internal-audit-reportFooter.html"));
            _footer = _readFooter_File.ReadToEnd();



            // header settings
            converter.Options.DisplayHeader = true ||
                true || true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            //converter.Header.Height = 72;
            converter.Header.Height = 70;
            //converter.Header.Height = 140;

            PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;

            converter.Header.Add(headerHtml);

            // footer settings
            converter.Options.DisplayFooter = true || true || true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            //converter.Footer.Height = 170;
            //converter.Footer.Height = 60;
            //converter.Footer.Height = 40;
            //converter.Footer.Height = 60;
            //converter.Footer.Height = 80;
            converter.Footer.Height = 40;

            PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);

            //PdfTextSection text1 = new PdfTextSection(510, 30, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));

            //18.10.22
            //PdfTextSection text1 = new PdfTextSection(450, 20, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
            //converter.Footer.Add(text1);



            #endregion


            #region start  landscape
            PdfDocument doc = new PdfDocument();
            //PdfPage page2 = doc.AddPage(PdfCustomPageSize.Letter,new PdfMargins(20f), PdfPageOrientation.Landscape);
            //int rotation = (int)page2.Rotation;



            PdfCustomPageSize pageSize2 = PdfCustomPageSize.A3;
            pageSize2 = PdfCustomPageSize.A3;
            PdfResizeManager resizer = new PdfResizeManager();
            resizer.PageSize = pageSize2;

            doc = converter.ConvertHtmlString(body);
           
            #endregion



            // PdfDocument doc = converter.ConvertHtmlString(body);


            //PdfDocument doc = converter.ConvertHtmlString(body);
            string ReportName = InterAud.Plan_ref_No + ".pdf";
            string path = Server.MapPath("~/IAR");
            doc.Save(path + '\\' + ReportName);
            doc.Close();
            #endregion
        }

        public ActionResult Delete(int id)
        {
            int result = 0;
            try
            {
                result = DAlAccess.DeleteReport(id);
                if (result != 0)
                {
                    TempData["deletereport"] = result;
                    return RedirectToAction("AuditreportDashBoard");
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
            }
            return View();
        }




        #region NC Upload
        //public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        //{
        //    var IPath = string.Empty;
        //    string[] splitedGrp;
        //    List<string> Selected = new List<string>();
        //    try
        //    {

        //        FormCollection fc = new FormCollection();
        //        string filePath = string.Empty;
        //        for (int i = 0; i < Request.Files.Count; i++)
        //        {
        //            HttpPostedFileBase files = Request.Files[i]; //Uploaded file
        //            int fileSize = files.ContentLength;
        //            if (files != null && files.ContentLength > 0)
        //            {
        //                // if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc"))
        //                if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))

        //                {
        //                    string fileName = files.FileName;
        //                    filePath = Path.Combine(Server.MapPath("~/AuditReportNCDocument/"), filePath);
        //                    var K = "/AuditReportNCDocument/" + fileName;
        //                    IPath = K;//.TrimStart('~');
        //                    files.SaveAs(Server.MapPath(IPath));
        //                    // string[] readText = System.IO.File.ReadAllLines(IPath, Encoding.UTF8);

        //                    var ExistingUploadFile = IPath;
        //                    splitedGrp = ExistingUploadFile.Split(',');
        //                    foreach (var single in splitedGrp)
        //                    {
        //                        Selected.Add(single);
        //                    }
        //                    Session["list"] = Selected;

        //                }
        //                else
        //                {
        //                    ViewBag.Error = "Please Select XLSX or PDF File";
        //                }
        //            }

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    return Json(IPath, JsonRequestBehavior.AllowGet);
        //}


        //added by ankush
        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            //Adding New Code 7 March 2020
            List<FileDetails> fileDetails = new List<FileDetails>();
            //---Adding end Code
            try
            {

                FormCollection fc = new FormCollection();
                string filePath = string.Empty;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file
                    int fileSize = files.ContentLength;
                    if (files != null && files.ContentLength > 0)
                    {
                        // if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc"))
                        if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))

                        {
                            string fileName = files.FileName;
                            //Adding New Code as per new requirement 7 March 2020, Manoj Sharma
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();
                            fileDetails.Add(fileDetail);
                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/AuditReportNCDocument/"), fileDetail.Id + fileDetail.Extension);
                            //filePath = Path.Combine(Server.MapPath("~/Files/Documents/"), filePath);
                            var K = "~/AuditReportNCDocument/" + fileName;
                            IPath = K;
                            files.SaveAs(filePath);

                            //filePath = Path.Combine(Server.MapPath("~/NCRDocument/"), filePath);
                            //var K = "~/NCRDocument/" + fileName;
                            ////IPath = K.TrimStart('~');
                            //IPath = K;

                            //files.SaveAs(Server.MapPath(IPath));
                            // string[] readText = System.IO.File.ReadAllLines(IPath, Encoding.UTF8);

                            var ExistingUploadFile = IPath;
                            splitedGrp = ExistingUploadFile.Split(',');
                            foreach (var single in splitedGrp)
                            {
                                Selected.Add(single);
                            }
                            Session["list"] = Selected;

                        }
                        else
                        {
                            ViewBag.Error = "Please Select XLSX or PDF File";
                        }
                    }

                }
                Session["listIARUploadedFile"] = fileDetails;

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                ViewBag.Error = "Please Select XLSX or PDF File";
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Suporting document
        //public JsonResult SupportingDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        //{
        //    var IPathSupportingDocument = string.Empty;
        //    string[] splitedGrp;
        //    List<string> Selected = new List<string>();
        //    try
        //    {

        //        FormCollection fc = new FormCollection();
        //        string filePath = string.Empty;
        //        for (int i = 0; i < Request.Files.Count; i++)
        //        {
        //            HttpPostedFileBase files = Request.Files[i]; //Uploaded file
        //            int fileSize = files.ContentLength;
        //            if (files != null && files.ContentLength > 0)
        //            {
        //                // if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc"))
        //                if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))

        //                {
        //                    string fileName = files.FileName;
        //                    filePath = Path.Combine(Server.MapPath("~/AuditReportRelatedDocument/"), filePath);
        //                    var K = "/AuditReportRelatedDocument/" + fileName;
        //                    IPathSupportingDocument = K;//.TrimStart('~');
        //                    files.SaveAs(Server.MapPath(IPathSupportingDocument));
        //                    // string[] readText = System.IO.File.ReadAllLines(IPath, Encoding.UTF8);

        //                    var ExistingUploadFile = IPathSupportingDocument;
        //                    splitedGrp = ExistingUploadFile.Split(',');
        //                    foreach (var single in splitedGrp)
        //                    {
        //                        Selected.Add(single);
        //                    }
        //                    Session["listSupportingDocument"] = Selected;

        //                }
        //                else
        //                {
        //                    ViewBag.Error = "Please Select XLSX or PDF File";
        //                }
        //            }

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    return Json(IPathSupportingDocument, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult SupportingDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            //Adding New Code 7 March 2020
            List<FileDetails> fileDetails1 = new List<FileDetails>();
            //---Adding end Code
            try
            {

                FormCollection fc = new FormCollection();
                string filePath = string.Empty;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file
                    int fileSize = files.ContentLength;
                    if (files != null && files.ContentLength > 0)
                    {
                        // if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc"))
                        if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))

                        {
                            string fileName = files.FileName;
                            //Adding New Code as per new requirement 7 March 2020, Manoj Sharma
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();
                            fileDetails1.Add(fileDetail);
                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/AuditReportRelatedDocument/"), fileDetail.Id + fileDetail.Extension);
                            //filePath = Path.Combine(Server.MapPath("~/Files/Documents/"), filePath);
                            var K = "~/AuditReportRelatedDocument/" + fileName;
                            IPath = K;
                            files.SaveAs(filePath);

                            //filePath = Path.Combine(Server.MapPath("~/NCRDocument/"), filePath);
                            //var K = "~/NCRDocument/" + fileName;
                            ////IPath = K.TrimStart('~');
                            //IPath = K;

                            //files.SaveAs(Server.MapPath(IPath));
                            // string[] readText = System.IO.File.ReadAllLines(IPath, Encoding.UTF8);

                            var ExistingUploadFile = IPath;
                            splitedGrp = ExistingUploadFile.Split(',');
                            foreach (var single in splitedGrp)
                            {
                                Selected.Add(single);
                            }
                            Session["list"] = Selected;

                        }
                        else
                        {
                            ViewBag.Error = "Please Select XLSX or PDF File";
                        }
                    }

                }
                Session["listIARUploadedFile1"] = fileDetails1;

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                ViewBag.Error = "Please Select XLSX or PDF File";
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public ActionResult DownloadPDF(string PDF)
        {
            //byte[] fileBytes = System.IO.File.ReadAllBytes("~/Content/");
            //string fileName = PDF; //+ ".ext";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            


        string filename = PDF;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "IAR/" + filename;



            byte[] filedata = System.IO.File.ReadAllBytes(filepath);

            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);



            // return File("~/Content/"+PDF, "application/pdf", PDF);
        }

        public ActionResult NCDocument(string NCDocument)
        {
            string filename = NCDocument;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "AuditReportNCDocument/" + filename;


            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);



        }

        public ActionResult SupportingDocument(string SupportingDocument)
        {
            string filename = SupportingDocument;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "AuditReportRelatedDocument/" + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }
        #region Upload & Delete File
        //Delete For Image
        [HttpPost]
        public JsonResult DeleteFile(string id)
        {
            string Results = string.Empty;
            FileDetails fileDetails = new FileDetails();
            DataTable DTGetDeleteFile = new DataTable();
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                Guid guid = new Guid(id);
                DTGetDeleteFile = DAlAccess.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = DAlAccess.DeleteUploadedFile(id);
                    var path = Path.Combine(Server.MapPath("~/AuditReportNCDocument/"), id + fileDetails.Extension);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            return Json(new { Result = "ERROR" });
        }
        public FileResult Download(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/AuditReportNCDocument/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }

        [HttpPost]
        public JsonResult DeleteFile1(string id)
        {
            string Results1 = string.Empty;
            FileDetails fileDetails1 = new FileDetails();
            DataTable DTGetDeleteFile1 = new DataTable();
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                Guid guid = new Guid(id);
                DTGetDeleteFile1 = DAlAccess.GetFileExt1(id);
                if (DTGetDeleteFile1.Rows.Count > 0)
                {
                    fileDetails1.Extension = Convert.ToString(DTGetDeleteFile1.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results1 = DAlAccess.DeleteUploadedFile1(id);
                    var path = Path.Combine(Server.MapPath("~/AuditReportRelatedDocument/"), id + fileDetails1.Extension);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            return Json(new { Result = "ERROR" });
        }
        public FileResult Download1(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/AuditReportRelatedDocument/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }
        #endregion
        #region Export to excel

        [HttpGet]
        public ActionResult ExportIndex()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<Internal_Audit_Report> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<Internal_Audit_Report> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                //added by nikita on 13092023
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "AuditReports-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<Internal_Audit_Report> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<Internal_Audit_Report> grid = new Grid<Internal_Audit_Report>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            //grid.Columns.Add(model => model.Branch).Titled("Branch");
            //grid.Columns.Add(model => model.Plan_ref_No).Titled("Plan Ref Serial No.");
            //grid.Columns.Add(model => model.Auditor).Titled("Auditor");
            //grid.Columns.Add(model => model.ExAuditor).Titled("ExAuditor");
            //grid.Columns.Add(model => model.Date_Of_Audit).Titled("Date of Audit");
            //grid.Columns.Add(model => model.Auditee).Titled("Auditee");
            //grid.Columns.Add(model => model.Department).Titled("Department");
            //grid.Columns.Add(model => model.Total_NCR_raised).Titled("Total NCR's Raised");
            //grid.Columns.Add(model => model.PDF).Titled("PDF");
            //grid.Columns.Add(model => model.NCDocument).Titled("NC Document");
            //grid.Columns.Add(model => model.SupportingDocument).Titled("Supporting Document");
            //grid.Columns.Add(model => model.IsAuditCompleted).Titled("Is Audit Completed");
            //grid.Columns.Add(model => model.AreFindingsClose).Titled("Are Findings Close");

            //added by nikita on 13092023
            grid.Columns.Add(model => model.Plan_ref_No).Titled("Plan Ref Serial No.");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.Auditor).Titled("Auditor Name");
            grid.Columns.Add(model => model.ExAuditor).Titled("External Auditor Name");
            grid.Columns.Add(model => model.Auditee).Titled("Auditees");
            grid.Columns.Add(model => model.ProposeDateFrom).Titled("Proposed Date of Audit (From Date)");
            grid.Columns.Add(model => model.ProposeDateTo).Titled("Proposed Date of Audit (To Date)");
            grid.Columns.Add(model => model.ActualAuditDateTo).Titled("Audit Date(s)");
            grid.Columns.Add(model => model.Department).Titled("Process audited");
            grid.Columns.Add(model => model.Total_NCR_raised).Titled("Total Findings");
            grid.Columns.Add(model => model.IsAuditCompleted).Titled("Is Audit Completed");
            grid.Columns.Add(model => model.AreFindingsClose).Titled("Are Findings Close");
            grid.Columns.Add(model => model.NCDocument).Titled("NC Document");


            grid.Pager = new GridPager<Internal_Audit_Report>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = InterAud.lstReport1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<Internal_Audit_Report> GetData()
        {

            DataTable DTDash = new DataTable();
            DataSet dsGetAudit = new DataSet();
            DTDash = DAlAccess.GetAuditReportDashBoard();
            List<Internal_Audit_Report> lstReport = new List<Internal_Audit_Report>();
            try
            {
                if (DTDash.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTDash.Rows)
                    {
                        lstReport.Add(
                            new Internal_Audit_Report
                            {
                                //Count = DTDash.Rows.Count,
                                //AuditId = Convert.ToInt32(dr["FkAuditId"]),
                                //Internal_Audit_Id = Convert.ToInt32(dr["Internal_Audit_Id"]),
                                //Branch = Convert.ToString(dr["Branch"]),
                                //Auditor = Convert.ToString(dr["Auditor"]),
                                //ExAuditor = Convert.ToString(dr["ExAuditor"]),
                                ////Plan_Ref_Serial_No = Convert.ToInt32(dr["Plan_Ref_Serial_No"]),
                                //Auditee = Convert.ToString(dr["Auditee"]),
                                //Department = Convert.ToString(dr["Department"]),
                                //Date_Of_Audit = Convert.ToDateTime(dr["Date_Of_Audit"]),
                                //Nominated_By_Management_Remark = Convert.ToString(dr["Nominated_By_Management_Remark"]),
                                //Nominated_By_Management_NCR = Convert.ToString(dr["Nominated_By_Management_NCR"]),
                                //Findings_From_PreviousAudit_Remark = Convert.ToString(dr["Findings_From_PreviousAudit_Remark"]),
                                //Findings_From_PreviousAudit_NCR = Convert.ToString(dr["Findings_From_PreviousAudit_NCR"]),
                                //Customer_Complaints_Remarks = Convert.ToString(dr["Customer_Complaints_Remarks"]),
                                //Customer_Complaints_NCR = Convert.ToString(dr["Customer_Complaints_NCR"]),
                                //CustomerFeedBacknAnalysis_Remark = Convert.ToString(dr["CustomerFeedBacknAnalysis_Remark"]),
                                //CustomerFeedBacknAnalysis_NCR = Convert.ToString(dr["CustomerFeedBacknAnalysis_NCR"]),
                                //Process_Measures_Remarks = Convert.ToString(dr["Process_Measures_Remarks"]),
                                //Process_Measures_NCR = Convert.ToString(dr["Process_Measures_NCR"]),
                                //Training_AppraisalnKRAs_Remarks = Convert.ToString(dr["Training_AppraisalnKRAs_Remarks"]),
                                //Training_AppraisalnKRAs_NCR = Convert.ToString(dr["Training_AppraisalnKRAs_NCR"]),
                                //Enquiry_Management_Process_Remarks = Convert.ToString(dr["Enquiry_Management_Process_Remarks"]),
                                //Enquiry_Management_Process_NCR = Convert.ToString(dr["Enquiry_Management_Process_NCR"]),
                                //Quotation_Process_Remarks = Convert.ToString(dr["Quotation_Process_Remarks"]),
                                //Quotation_Process_NCR = Convert.ToString(dr["Quotation_Process_NCR"]),
                                //OrderReviewnAccepatanceContractreview_Remarks = Convert.ToString(dr["OrderReviewnAccepatanceContractreview_Remarks"]),
                                //OrderReviewnAccepatanceContractreview_NCR = Convert.ToString(dr["OrderReviewnAccepatanceContractreview_NCR"]),
                                //OrganisationStructure_Remarks = Convert.ToString(dr["OrganisationStructure_Remarks"]),
                                //OrganisationStructure_NCR = Convert.ToString(dr["OrganisationStructure_NCR"]),
                                //Control_Of_Documents_Remarks = Convert.ToString(dr["Control_Of_Documents_Remarks"]),
                                //Control_Of_Documents_NCR = Convert.ToString(dr["Control_Of_Documents_NCR"]),
                                //Control_Of_Records_Remarks = Convert.ToString(dr["Control_Of_Records_Remarks"]),
                                //Control_Of_Records_NCR = Convert.ToString(dr["Control_Of_Records_NCR"]),
                                //Competancy_Mapping_Remarks = Convert.ToString(dr["Competancy_Mapping_Remarks"]),
                                //Competancy_Mapping_NCR = Convert.ToString(dr["Competancy_Mapping_NCR"]),
                                //TrainingRecords_Effectiveness_Remarks = Convert.ToString(dr["TrainingRecords_Effectiveness_Remarks"]),
                                //TrainingRecords_Effectiveness_NCR = Convert.ToString(dr["TrainingRecords_Effectiveness_NCR"]),
                                //Impartiality_Confidentiality_Remarks = Convert.ToString(dr["Impartiality_Confidentiality_Remarks"]),
                                //Impartiality_Confidentiality_NCR = Convert.ToString(dr["Impartiality_Confidentiality_NCR"]),
                                //InspectionCordinationnInspectionProcess_Remarks = Convert.ToString(dr["InspectionCordinationnInspectionProcess_Remarks"]),
                                //InspectionCordinationnInspectionProcess_NCR = Convert.ToString(dr["InspectionCordinationnInspectionProcess_NCR"]),
                                //Onsite_Monitoring_Remarks = Convert.ToString(dr["Onsite_Monitoring_Remarks"]),
                                //Onsite_Monitoring_NCR = Convert.ToString(dr["Onsite_Monitoring_NCR"]),
                                //Document_Work_Review_Remarks = Convert.ToString(dr["Document_Work_Review_Remarks"]),
                                //Document_Work_Review_NCR = Convert.ToString(dr["Document_Work_Review_NCR"]),
                                //Safety_Of_Personnel_Remarks = Convert.ToString(dr["Safety_Of_Personnel_Remarks"]),
                                //Safety_Of_Personnel_NCR = Convert.ToString(dr["Safety_Of_Personnel_NCR"]),
                                //Planning_opportunities_Remarks = Convert.ToString(dr["Planning_opportunities_Remarks"]),
                                //Planning_opportunities_NCR = Convert.ToString(dr["Planning_opportunities_NCR"]),
                                //Plan_ref_No = Convert.ToString(dr["Plan_ref_No"]),
                                //Total_NCR_raised = Convert.ToInt32(dr["Total_NCR_raised"]),
                                //PDF = Convert.ToString(dr["PDF"]),
                                //NCDocument = Convert.ToString(dr["NCDocument"]),
                                //SupportingDocument = Convert.ToString(dr["SupportingDocument"]),
                                //DateOfAudit_TODate = Convert.ToDateTime(dr["DateOfAudit_TODate"]),

                                //IsAuditCompleted = Convert.ToString(dr["IsAuditCompleted"]),
                                //AreFindingsClose = Convert.ToString(dr["AreFindingsClose"]),

                                //CriticalNCCount = Convert.ToString(dr["CriticalNCCount"]),
                                //MajorNCCount = Convert.ToString(dr["MajorNCCount"]),
                                //MinorNCCount = Convert.ToString(dr["MinorNCCount"]),
                                //OBSERVATIONCount = Convert.ToString(dr["OBSERVATIONCount"]),
                                //ConcernCount = Convert.ToString(dr["ConcernCount"]),
                                //OFICount = Convert.ToString(dr["OFICount"]),

                                //added by nikita on 13092023
                                Count = DTDash.Rows.Count,
                                AuditId = Convert.ToInt32(dr["AuditId"]),
                                Internal_Audit_Id = Convert.ToInt32(dr["Internal_Audit_Id"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                Auditor = Convert.ToString(dr["Auditor"]),
                                ExAuditor = Convert.ToString(dr["ExAuditor"]),
                                Auditee = Convert.ToString(dr["Auditee"]),
                                Department = Convert.ToString(dr["Department"]),
                                ActualAuditDateTo = Convert.ToString(dr["ActualAuditDateTo"]),
                                ProposeDateFrom = Convert.ToString(dr["ProposeDateFrom"]),
                                ProposeDateTo = Convert.ToString(dr["ProposeDateTo"]),
                                Plan_ref_No = Convert.ToString(dr["Plan_ref_No"]),
                                Total_NCR_raised = Convert.ToInt32(dr["TotalFindings"]),
                                PDF = Convert.ToString(dr["PDF"]),
                                NCDocument = Convert.ToString(dr["NCDocument"]),
                                SupportingDocument = Convert.ToString(dr["SupportingDocument"]),
                                IsAuditCompleted = Convert.ToString(dr["IsAuditCompleted"]),
                                AreFindingsClose = Convert.ToString(dr["AreFindingsClose"]),
                                CriticalNCCount = Convert.ToString(dr["CriticalNCCount"]),
                                MajorNCCount = Convert.ToString(dr["MajorNCCount"]),
                                MinorNCCount = Convert.ToString(dr["MinorNCCount"]),
                                OBSERVATIONCount = Convert.ToString(dr["OBSERVATIONCount"]),
                                ConcernCount = Convert.ToString(dr["ConcernCount"]),
                                OFICount = Convert.ToString(dr["OFICount"]),

                            });
                    }
                }




            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            ViewData["dashboardreport"] = lstReport;
            InterAud.lstReport1 = lstReport;

            return InterAud.lstReport1;
        }
        #endregion

        //added by rajvi 3feb2025
        [HttpGet]
        public ActionResult NDTQualifications()


        {
            DataTable DTNDT = new DataTable();
            DataSet dsNDT = new DataSet();
            DTNDT = DAlAccess.GetFinalNDTQualifications();  // Assuming this fetches the data from the stored procedure
            List<NDTQualification> lstReport = new List<NDTQualification>();
            DateTime parseddate;
            try
            {
                if (DTNDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTNDT.Rows)
                    {
                        lstReport.Add(
                            new NDTQualification
                            {
                                FirstName = Convert.ToString(dr["FirstName"]),
                                LastName = Convert.ToString(dr["LastName"]),
                                RollNo = Convert.ToString(dr["RollNo"]),
                                EmpCode = Convert.ToString(dr["EmpCode"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                MT_Perc = Convert.ToString(dr["MT_Perc"]),
                                MT_Cert_Date = DateTime.TryParse(dr["MT_Cert_Date"]?.ToString(), out parseddate) ? parseddate.ToString("dd/MM/yyyy") : string.Empty, // or use "N/A" or any default value
                                PT_Perc = Convert.ToString(dr["PT_Perc"]),
                                PT_Cert_Date = DateTime.TryParse(dr["PT_Cert_Date"]?.ToString(), out parseddate) ? parseddate.ToString("dd/MM/yyyy") : string.Empty, // or use "N/A" or any default value
                                VT_Perc = Convert.ToString(dr["VT_Perc"]),
                                VT_Cert_Date = DateTime.TryParse(dr["VT_Cert_Date"]?.ToString(), out parseddate) ? parseddate.ToString("dd/MM/yyyy") : string.Empty, // or use "N/A" or any default value
                                UT_Perc = Convert.ToString(dr["UT_Perc"]),
                                UT_Cert_Date = DateTime.TryParse(dr["UT_Cert_Date"]?.ToString(), out parseddate) ? parseddate.ToString("dd/MM/yyyy") : string.Empty, // or use "N/A" or any default value
                                RT_Perc = Convert.ToString(dr["RT_Perc"]),
                                RT_Cert_Date = DateTime.TryParse(dr["RT_Cert_Date"]?.ToString(), out parseddate) ? parseddate.ToString("dd/MM/yyyy") : string.Empty, // or use "N/A" or any default value
                                Image = Convert.ToString(dr["Image"])
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }

            // Pass the list directly to the view
            return View(lstReport);  // Pass the List<NDTQualification> directly to the view
        }

        [HttpGet]
        public ActionResult GenerateCertificate()
        {
            DataTable DTCERT = new DataTable();
            DataSet dsCERT = new DataSet();
            DTCERT = DAlAccess.GetCertificateData();  // Assuming this fetches the data from the stored procedure
            List<GenerateCertificate> lstReport = new List<GenerateCertificate>();
            try
            {
                if (DTCERT.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCERT.Rows)
                    {
                        lstReport.Add(
                            new GenerateCertificate
                            {
                                Name = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                EmpCode = Convert.ToString(dr["EmpCOde"]),
                                DateofJoining = Convert.ToDateTime(Convert.ToString(dr["DateOfJoining"])).ToString("dd/MM/yyyy"),
                                MT = Convert.ToString(dr["MT_Perc"]),
                                PT = Convert.ToString(dr["PT_Perc"]),
                                VT = Convert.ToString(dr["VT_Perc"]),
                                UT = Convert.ToString(dr["UT_Perc"]),
                                RT = Convert.ToString(dr["RT_Perc"]),
                                EyeTestCertCalid = Convert.ToString(dr["ValidDate"]),
                                //CertValid = Convert.ToString(dr["CertValidDate"]),
                                //HAsCert = Convert.ToString(dr["HasCertification"]),
                                //YesCert = Convert.ToString(dr["CertValidated"]),
                                YesEye = Convert.ToString(dr["EyeValidated"]),
                                EyeTest = Convert.ToString(dr["EyeCertification"]),
                                MTCERT = Convert.ToString(dr["MT_Certification"]),
                                PTCERT = Convert.ToString(dr["PT_Certification"]),
                                VTCERT = Convert.ToString(dr["VT_Certification"]),
                                UTCERT = Convert.ToString(dr["UT_Certification"]),
                                RTCERT = Convert.ToString(dr["RT_Certification"]),
                                // ID = Convert.ToString(dr["PK_id"]),
                                // Method = Convert.ToString(dr["FileName"]),
                                RollNo = Convert.ToString(dr["RollNo"])
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }

            // Pass the list directly to the view
            return View(lstReport);  // Pass the List<NDTQualification> directly to the view
        }

        public ActionResult UploadBulkNDT()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ExportNDTIndex()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<NDTQualification> grid = CreateNDTExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<NDTQualification> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                //added by nikita on 13092023
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "NDTQualificationReport-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<NDTQualification> CreateNDTExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<NDTQualification> grid = new Grid<NDTQualification>(GetNDTData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };




            // grid.Columns.Add(model => model.FirstName).Titled("FirstName");
            //grid.Columns.Add(model => model.LastName).Titled("LastName");
            //grid.Columns.Add(model => model.EmpCode).Titled("EmpCode");
            grid.Columns.Add(model => model.RollNo).Titled("RollNo");
            //grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.MT_Perc).Titled("MT_Perc");
            grid.Columns.Add(model => model.MT_Cert_Date).Titled("MT_Cert_Date");
            grid.Columns.Add(model => model.MT_Pass).Titled("MT_Pass");
            grid.Columns.Add(model => model.PT_Perc).Titled("PT_Perc");
            grid.Columns.Add(model => model.PT_Cert_Date).Titled("PT_Cert_Date");
            grid.Columns.Add(model => model.PT_Pass).Titled("PT_Pass");
            grid.Columns.Add(model => model.VT_Perc).Titled("VT_Perc");
            grid.Columns.Add(model => model.VT_Cert_Date).Titled("VT_Cert_Date");
            grid.Columns.Add(model => model.VT_Pass).Titled("VT_Pass");
            grid.Columns.Add(model => model.UT_Perc).Titled("UT_Perc");
            grid.Columns.Add(model => model.UT_Cert_Date).Titled("UT_Cert_Date");
            grid.Columns.Add(model => model.UT_Pass).Titled("UT_Pass");
            grid.Columns.Add(model => model.RT_Perc).Titled("RT_Perc");
            grid.Columns.Add(model => model.RT_Cert_Date).Titled("RT_Cert_Date");
            grid.Columns.Add(model => model.RT_Pass).Titled("RT_Pass");
            grid.Columns.Add(model => model.Image).Titled("Image");




            grid.Pager = new GridPager<NDTQualification>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ndtq.ndtReport1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<NDTQualification> GetNDTData()
        {
            string basePath = "/content/Sign/";
            DataTable DTDash = new DataTable();
            DataSet dsGetAudit = new DataSet();
            DTDash = DAlAccess.GetFinalNDTQualifications();
            List<NDTQualification> lstReport = new List<NDTQualification>();
            try
            {
                if (DTDash.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTDash.Rows)
                    {
                        lstReport.Add(
                            new NDTQualification
                            {

                                Count = DTDash.Rows.Count,
                                FirstName = Convert.ToString(dr["FirstName"]),
                                LastName = Convert.ToString(dr["LastName"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                RollNo = Convert.ToString(dr["RollNo"]),
                                EmpCode = Convert.ToString(dr["EmpCode"]),
                                MT_Perc = Convert.ToString(dr["MT_Perc"]),
                                MT_Cert_Date = Convert.ToString(dr["MT_Cert_Date"]),
                                PT_Perc = Convert.ToString(dr["PT_Perc"]),
                                PT_Cert_Date = Convert.ToString(dr["PT_Cert_Date"]),
                                VT_Perc = Convert.ToString(dr["VT_Perc"]),
                                VT_Cert_Date = Convert.ToString(dr["VT_Cert_Date"]),
                                UT_Perc = Convert.ToString(dr["UT_Perc"]),
                                UT_Cert_Date = Convert.ToString(dr["UT_Cert_Date"]),
                                RT_Perc = Convert.ToString(dr["RT_Perc"]),
                                RT_Cert_Date = Convert.ToString(dr["RT_Cert_Date"]),
                                Image = Convert.ToString(dr["Image"]),

                            });
                    }
                }




            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            ViewData["NDTData"] = lstReport;
            ndtq.ndtReport1 = lstReport;

            return ndtq.ndtReport1;
        }


        public ActionResult BindNdtExcelDataToGrid(HttpPostedFileBase fileData)
        {
            string basePath = "/content/Sign/";

            if (fileData == null || fileData.ContentLength == 0)
            {
                return Json(new { success = false, message = "No file uploaded" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                string fileExtension = Path.GetExtension(fileData.FileName).ToLower();
                string fileLocation = Path.Combine(Server.MapPath("~/Content/Uploads"), fileData.FileName);

                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }

                fileData.SaveAs(fileLocation);

                string connectionString = fileExtension == ".xls"
                    ? $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={fileLocation};Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\""
                    : $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileLocation};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";

                DataSet ds = new DataSet();
                using (OleDbConnection excelConnection = new OleDbConnection(connectionString))
                {
                    excelConnection.Open();

                    DataTable dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return Json(new { success = false, message = "No sheets found in the Excel file." }, JsonRequestBehavior.AllowGet);
                    }

                    string sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
                    string query = $"SELECT * FROM [{sheetName}]";

                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
                    {
                        dataAdapter.Fill(ds);
                    }
                }

                var excelData = ds.Tables[0];
                var rollnos = excelData.AsEnumerable()
                                       .Select(row => row["RollNo"]?.ToString()) // Assuming "RollNo" is the column name
                                       .Where(rollno => !string.IsNullOrEmpty(rollno))
                                       .Distinct()
                                       .ToList();

                if (rollnos.Any())
                {
                    // Create a list to hold the final result data
                    List<dynamic> finalResult = new List<dynamic>();

                    // Fetch data from the database based on the extracted roll numbers
                    foreach (var rollno in rollnos)
                    {
                        DataTable dtNDT = GetNDTQualifications(rollno);
                        if (dtNDT.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtNDT.Rows)
                            {
                                var excelRow = excelData.AsEnumerable().FirstOrDefault(r => r["RollNo"]?.ToString() == rollno);

                                if (excelRow != null)
                                {
                                    var result = new
                                    {
                                        FirstName = Convert.ToString(dr["FirstName"]),
                                        LastName = Convert.ToString(dr["LastName"]),
                                        EmpCode = Convert.ToString(dr["EmployeeCode"]),
                                        Branch = Convert.ToString(dr["Branch_Name"]),
                                        RollNo = Convert.ToString(excelRow["RollNo"]),
                                        MT_Perc = Convert.ToString(excelRow["MT Perc"]),
                                        MT_Cert_Date = Convert.ToString(excelRow["MT Cert Date"]),
                                        MT_Pass = Convert.ToString(excelRow["MT Pass"]),
                                        PT_Perc = Convert.ToString(excelRow["PT Perc"]),
                                        PT_Cert_Date = Convert.ToString(excelRow["PT Cert Date"]),
                                        PT_Pass = Convert.ToString(excelRow["PT Pass"]),
                                        VT_Perc = Convert.ToString(excelRow["VT Perc"]),
                                        VT_Cert_Date = Convert.ToString(excelRow["VT Cert Date"]),
                                        VT_Pass = Convert.ToString(excelRow["VT Pass"]),
                                        UT_Perc = Convert.ToString(excelRow["UT Perc"]),
                                        UT_Cert_Date = Convert.ToString(excelRow["UT Cert Date"]),
                                        UT_Pass = Convert.ToString(excelRow["UT Pass"]),
                                        RT_Perc = Convert.ToString(excelRow["RT Perc"]),
                                        RT_Cert_Date = Convert.ToString(excelRow["RT Cert Date"]),
                                        RT_Pass = Convert.ToString(excelRow["RT Pass"]),
                                        Image = basePath + Convert.ToString(dr["Image"]),
                                    };
                                    finalResult.Add(result);
                                }
                            }
                        }
                    }

                    // Convert List<dynamic> to DataTable
                    DataTable finalResultDataTable = ConvertToDataTable(finalResult);

                    // Pass the DataTable to the method
                    DataTable result1 = objDALNdt.InsertNDTDataToUsercreation(finalResultDataTable);

                    return Json(new { success = true, data = finalResult }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, message = "No valid roll numbers found in the uploaded file." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public DataTable ConvertToDataTable(List<dynamic> list)
        {
            var dataTable = new DataTable();

            if (list != null && list.Any())
            {
                // Dynamically create columns based on the properties of the first object in the list
                var firstItem = list.First();
                foreach (var property in firstItem.GetType().GetProperties())
                {
                    dataTable.Columns.Add(property.Name);
                }

                // Add rows to the DataTable
                foreach (var item in list)
                {
                    var row = dataTable.NewRow();
                    foreach (var property in item.GetType().GetProperties())
                    {
                        row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        public ActionResult InsertNDTDataIntoDataBase(HttpPostedFileBase file)
        {
            DataTable dt = new DataTable();
            if (file == null || file.ContentLength == 0)
            {
                // Handle the case where no file is uploaded
                ViewBag.Message = "No file selected or file is empty.";
                return View();
            }

            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            string fileLocation = Server.MapPath("~/Content/Uploads") + file.FileName;

            try
            {
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    ProcessExcelFile(file, fileLocation, fileExtension);

                    dt = objDALNdt.InsertNDTDataToUsercreation(null);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var status = dt.Rows[0]["Status"].ToString();

                        if (status == "Successful")
                        {
                            TempData["UploadedData"] = dt;

                        }


                    }

                }
                else if (fileExtension == ".xml")
                {
                    ProcessXmlFile(file, fileLocation);
                }
                else
                {
                    ViewBag.Message = "Invalid file format. Please upload an Excel or XML file.";
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                ViewBag.Message = "File uploaded and data inserted successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and display a generic error message
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }

            //  return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            string jsonResult = JsonConvert.SerializeObject(dt);
            return Json(new { success = true, data = jsonResult }, JsonRequestBehavior.AllowGet);

        }

        public DataTable GetNDTQualifications(string rollno)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("Sp_NDT_QUALIFICATIONS_REPORT", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@EmployeeRollNo", rollno); // Using the emailId passed dynamically
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            return dt;
        }

        public DataTable GetFinalNDTQualifications()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("GetNDTQualRecords", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                // CMDCallDash.Parameters.AddWithValue("@EmployeeRollNo", rollno); // Using the emailId passed dynamically
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            return dt;
        }

        private string GetExcelConnectionString(string fileExtension, string fileLocation)
        {
            if (fileExtension == ".xls")
            {
                return $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={fileLocation};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (fileExtension == ".xlsx")
            {
                return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileLocation};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            else
            {
                throw new Exception("Invalid Excel file format.");
            }
        }

        private void ProcessExcelFile(HttpPostedFileBase file, string fileLocation, string fileExtension)
        {

            // Ensure file doesn't already exist before saving
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
            }

            file.SaveAs(fileLocation);
            string connectionString = GetExcelConnectionString(fileExtension, fileLocation);

            using (OleDbConnection excelConnection = new OleDbConnection(connectionString))
            {
                excelConnection.Open();
                DataTable schemaTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schemaTable == null || schemaTable.Rows.Count == 0)
                {
                    throw new Exception("No sheets found in the Excel file.");
                }

                // Get the first sheet from the Excel file
                string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();
                string query = $"SELECT * FROM [{sheetName}]";

                DataSet ds = new DataSet();
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
                {
                    dataAdapter.Fill(ds);
                }

                // Get the column headers dynamically from the first row
                var headers = ds.Tables[0].Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList();

                // Map the Excel headers to database columns
                var headerMapping = GetHeaderColumnMapping();

                // Insert the data into the database
                // InsertDataIntoDatabase(ds, headers, headerMapping);
            }
        }

        private void ProcessXmlFile(HttpPostedFileBase file, string fileLocation)
        {
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
            }

            file.SaveAs(fileLocation);
            using (XmlTextReader xmlReader = new XmlTextReader(fileLocation))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(xmlReader);

                // Assuming the XML file also has column headers (can be customized for your XML format)
                var headers = ds.Tables[0].Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList();
                var headerMapping = GetHeaderColumnMapping();

                // InsertDataIntoDatabase(ds, headers, headerMapping);
            }
        }

        private Dictionary<string, string> GetHeaderColumnMapping()
        {
            // Define the mapping from Excel column headers to your database column names
            return new Dictionary<string, string>
    {
                { "RollNo", "RollNo" },
          { "MT Perc", "MT Perc" },
              { "MT Cert Date", "MT Cert Date" },
                  { "PT Perc", "PT Perc" },
                        { "PT Cert Date", "PT Cert Date" },
                          { "VT Perc", "VT Perc" },
{ "VT Cert date", "VT Cert Date" },
 { "UT Perc", "UT Perc" },
              { "UT Cert Date", "UT Cert Date" },
                  { "RT Perc", "RT Perc" },
                        { "RT Cert Date", "RT Cert Date" },
                        


        // Add other mappings here...
    };
        }


        [HttpPost]
        public ActionResult DownloadCertificates(List<int> ids)
        {
            List<string> certificateUrls = new List<string>();

            foreach (var id in ids)
            {
                // Logic to generate the certificate, save as file, and get the URL
                string filePath = GenerateCertificate(id); // A method to generate the certificate PDF
                certificateUrls.Add(Url.Content(filePath)); // Or use the full URL path
            }

            return Json(certificateUrls); // Return the list of file paths/URLs
        }

        private string GenerateCertificate(int id)
        {
            // Logic to generate the certificate PDF, e.g., using SelectPdf


            DataTable dtCertificateDetails = DAlAccess.GetCertificateDataFinal(id);

            DataRow row = dtCertificateDetails.Rows[0];
            string employeeName = row["DisplayName"].ToString();
            string employeeCode = row["EmpCode"].ToString();
            string location = row["Branch"].ToString();
            string MT_Pass = row["MT_Pass"].ToString();
            string PT_Pass = row["PT_Pass"].ToString();
            string UT_Pass = row["UT_Pass"].ToString();
            string VT_Pass = row["VT_Pass"].ToString();
            string RT_Pass = row["RT_Pass"].ToString();
            string ndtMethods = "";// Assume this is a comma-separated string
            string MTcertificationDate = "";//Convert.ToDateTime(row["CertIssueDate"]).ToString("dd/MM/yyyy");
            string PTcertificationDate = "";
            string VTcertificationDate = "";
            string UTcertificationDate = "";
            string RTcertificationDate = "";
            string MTCertvalidTill = "";//Convert.ToDateTime(row["CertValidDate"]).ToString("dd/MM/yyyy");
            string PTCertvalidTill = "";
            string VTCertvalidTill = "";
            string UTCertvalidTill = "";
            string RTCertvalidTill = "";
            string EyevalidTill = row["ValidDate"].ToString();
            string TrainingHrs = "";
            decimal mtperc, ptperc, vtperc, utperc, rtperc;
            string MTGrd = decimal.TryParse(row["MT_Perc"]?.ToString(), out mtperc) ? mtperc.ToString("F2") : "-";
            string PTGrd = decimal.TryParse(row["PT_Perc"]?.ToString(), out ptperc) ? ptperc.ToString("F2") : "-";
            string VTGrd = decimal.TryParse(row["VT_Perc"]?.ToString(), out vtperc) ? vtperc.ToString("F2") : "-";
            string RTGrd = decimal.TryParse(row["RT_Perc"]?.ToString(), out utperc) ? utperc.ToString("F2") : "-";
            string UTGrd = decimal.TryParse(row["UT_Perc"]?.ToString(), out rtperc) ? rtperc.ToString("F2") : "-";
            string image = row["Image"].ToString();
            //string pk_id = row["PK_id"].ToString();
            string RollNo = row["RollNo"].ToString();
            string AttachmentType = row["AttachmentType"].ToString();
            string photoPath = Url.Content("https://tiimes.tuv-india.com/content/Sign/" + image);
            DateTime parsed;
            string date = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
            DateTime? MT_CertDate = DateTime.TryParse(row["MT_Cert_Date"]?.ToString(), out parsed) ? parsed : (DateTime?)null;
            DateTime? PT_CertDate = DateTime.TryParse(row["PT_Cert_Date"]?.ToString(), out parsed) ? parsed : (DateTime?)null;
            DateTime? RT_CertDate = DateTime.TryParse(row["RT_Cert_Date"]?.ToString(), out parsed) ? parsed : (DateTime?)null;
            DateTime? UT_CertDate = DateTime.TryParse(row["UT_Cert_Date"]?.ToString(), out parsed) ? parsed : (DateTime?)null;
            DateTime? VT_CertDate = DateTime.TryParse(row["VT_Cert_Date"]?.ToString(), out parsed) ? parsed : (DateTime?)null;

            string certificateFileName = "Certificate_" + "" + "_" + employeeCode + ".pdf";
            string filePath ="~/TempImages/" + certificateFileName;

            if (MT_Pass == "Pass")
            {
                ndtMethods = "Magnetic Particle Testing (MT)";
                MTcertificationDate = Convert.ToDateTime(row["MT_Cert_Date"]).ToString("dd/MM/yyyy");
                DateTime? certValidTillDate = MT_CertDate.HasValue ? MT_CertDate.Value.AddYears(5).AddDays(-1) : (DateTime?)null;
                MTCertvalidTill = certValidTillDate.HasValue ? certValidTillDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                TrainingHrs = "40";
            }
            if (PT_Pass == "Pass")
            {
                ndtMethods = "Liquid Penetrant Testing (PT)";
                PTcertificationDate = Convert.ToDateTime(row["PT_Cert_Date"]).ToString("dd/MM/yyyy");
                DateTime? certValidTillDate = PT_CertDate.HasValue ? PT_CertDate.Value.AddYears(5).AddDays(-1) : (DateTime?)null;
                PTCertvalidTill = certValidTillDate.HasValue ? certValidTillDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                TrainingHrs = "40";
            }
            if (VT_Pass == "Pass")
            {
                ndtMethods = "Visual Testing (VT)";
                VTcertificationDate = Convert.ToDateTime(row["VT_Cert_Date"]).ToString("dd/MM/yyyy");
                DateTime? certValidTillDate = VT_CertDate.HasValue ? VT_CertDate.Value.AddYears(5).AddDays(-1) : (DateTime?)null;
                VTCertvalidTill = certValidTillDate.HasValue ? certValidTillDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                TrainingHrs = "40";
            }
            if (UT_Pass == "Pass")
            {
                ndtMethods = "Ultrasonic Testing (UT)";
                UTcertificationDate = Convert.ToDateTime(row["UT_Cert_Date"]).ToString("dd/MM/yyyy");
                DateTime? certValidTillDate = UT_CertDate.HasValue ? UT_CertDate.Value.AddYears(5).AddDays(-1) : (DateTime?)null;
                UTCertvalidTill = certValidTillDate.HasValue ? certValidTillDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                TrainingHrs = "40";
            }
            if (RT_Pass == "Pass")
            {
                ndtMethods = "Radiographic Testing (RT)";
                RTcertificationDate = Convert.ToDateTime(row["RT_Cert_Date"]).ToString("dd/MM/yyyy");
                DateTime? certValidTillDate = RT_CertDate.HasValue ? RT_CertDate.Value.AddYears(5).AddDays(-1) : (DateTime?)null;
                RTCertvalidTill = certValidTillDate.HasValue ? certValidTillDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                TrainingHrs = "40";
            }

            // Generate the PDF (you already have this logic in place in your example)
            SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
            var converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;

            //converter.Options.PdfPageSize = PdfPageSize.Custom; // Custom page size
            //converter.Options.PdfPageCustomSize = new System.Drawing.SizeF(900, 900);

            int totalTableHeight = 500; // Total height allocated for the table body
            int rowCount = 0;

            string html = "";

            html += "<style>";
            html += "body {font-family: TNG Pro;font-color:#00003c;background-color:white;}";

            // html += "tbody:before, tfoot:before {line -height: 0.07em;content: '.';color: white; /* bacground color */  display: block;  }";
            html += "td {border: 1px solid #000000;padding: 10px;text-align: center;}";
            html += "th {border: 1px solid #000000;padding: 6px;text-align: center;}";

            //html += "td {padding -top: 10px;padding-bottom: 10px;border: 1px solid #000000;text-align:center;}";

            //html += ".body-content-table td{padding-top: 6px;padding-bottom: 6px;}";
            //html += ".custom-table { border - collapse: collapse;width: 60 %;}";
            html += "#certificate {width: 1550px;margin: 0 auto;padding: 30px;font-family: TNG Pro;margin-top:-20px;margin-left:10px;margin-right:10px;margin-top:10px;margin-bottom:10px;}";
            html += ".info-container {display: flex;justify-content: space-between;align-items: flex-start;margin-top: 30px;}";

            html += " .info-table {width: 72%;border-collapse: collapse;margin-top:-10px;}";
            html += ".photo-box {width: 138px; height: 177px;text - align: center;line - height: 120px; font - size: 12px;font - weight: bold;margin-right: 125px;margin-top:-30px;}";
            html += "table {width: 100 %;border - collapse: collapse;margin - top: 20px;font-color:#00003c;}";
            html += "h2 {text - align: center;font - weight: bold;} p {margin: 10px 0;line - height: 1.5;}";
            html += "#footer-container {position: relative;width: 100%;margin-top: 20px;}";
            html += "#footer-text {font-size: 11px;text-align: left;}";
            html += "#footer-logo {position: absolute;right: 0;bottom: 0;width: 120px;height: auto;}";
            //html += ".tbodycustomheight tr{ height: 200px !important; }";

            html += "</style>";

            html += "</head>";

            html += "<body>";
            html += "<div id='certificate'>";
            html += "<div style='text-align:left;'><img src='https://tiimes.tuv-india.com/AllJsAndCss/images/logo.SVG' alt='TUV India Logo' style='width: 200px; height: auto;'></div>";
            // html += "<div style ='margin-top: 20px;'>";

            //html += "<div class='photo-box'>Photo</div>";

            //html += "<div id ='certificate' style='width: 1500px; height: 1200; padding: 30px; ; font-family: Arial, sans-serif; position: relative;'>";
            html += "<h2 style='text-align:center;font-size:22px;color:#001ed2;margin-top:0px;'>TUV India Private Limited</h2>";
            html += "<p style ='text-align: center;font-size:20px;margin-top:-15px;'><b>NDT Certificate</b></p>";
            html += "<p style ='text-align: center;font-size:17px;'><b>Number :</b> NDTC-" + RollNo + " Dated " + date + "</p>";
            html += "<p style='font-size:17px;'>This is to certify that the individual named below has successfully completed the Experience, training and examination requirement in accordance with";
            html += " the provisions of TUV India Private Limited NDT Certification procedure for the Qualification and Certification of NDT personnel, written practice document,";
            html += " TUVIS/WP/01, Revision 07, dated 16.09.2024.</p>";

            html += "<div class='info-container'>";
            html += "<table class='info-table'>";

            html += "<tr><td style='background-color:#efefef;width:32.5%;text-align:left;font-size:17px;'><b>Name</b></td><td style='width:30%;text-align:left;font-size:17px;'>" + employeeName + "</td></tr>";
            html += "<tr><td style='background-color:#efefef;width:32.5%;text-align:left;font-size:17px;'><b>Employee Code</b></td><td style='width:30%;text-align:left;font-size:17px;'>" + employeeCode + "</td></tr>";
            html += "<tr><td style='background-color:#efefef;width:32.5%;text-align:left;font-size:17px;'><b>Employee Roll No</b></td><td style='width:30%;text-align:left;font-size:17px;'>" + RollNo + "</td></tr>";
            html += "<tr><td style='background-color:#efefef;width:32.5%;text-align:left;font-size:17px;'><b>Location</b></td><td style='width:30%;text-align:left;font-size:17px;'>" + location + "</td></ tr>";
            html += "</table>";
            if (AttachmentType == "1")
            {
                html += "<div style='width: 138px; height: 177px;margin-right:125px;margin-top:-15px;border: 1px solid black;'><img style='width: 138px; height: 177px;' src='" + photoPath + "' alt='Photo' /></div>";
            }
            //else
            //{
            //    html += "<div class='photo-box'>Photo</div>";
            //}

            html += "</div>";

            html += " <p style='font-size:15px;margin-top:10px;margin-bottom:25px;'>Is Certified to perform the following NDT Method(s).</p>";

            if (MT_Pass == "Pass") rowCount++;
            if (UT_Pass == "Pass") rowCount++;
            if (VT_Pass == "Pass") rowCount++;
            if (RT_Pass == "Pass") rowCount++;
            if (PT_Pass == "Pass") rowCount++;
            int rowHeight = rowCount > 0 ? totalTableHeight / rowCount : 50;
            var passCount = 0;

            if (MT_Pass == "Pass") passCount++;
            if (UT_Pass == "Pass") passCount++;
            if (VT_Pass == "Pass") passCount++;
            if (RT_Pass == "Pass") passCount++;
            if (PT_Pass == "Pass") passCount++;
            var tableWidth = (passCount == 1) ? "105%" : "100%";
            html += " <table  style ='height:500px;width:" + tableWidth + ";border-collapse: collapse; margin-top: 20px;border: 1px solid #000000;'>";
            html += " <thead>";
            html += " <tr style = 'height:50px;'><th style='background-color:#efefef;font-size:17px;'>NDT Method</th><th  style='background-color:#efefef;font-size:17px;'>Level</th><th  style='background-color:#efefef;font-size:17px;'>Training Hours</th><th style='background-color:#efefef;font-size:17px;'>Composite Grade</th><th style='background-color:#efefef;font-size:17px;'>Date of</br>Certification</th><th style='background-color:#efefef;font-size:17px;'>Valid Till</th></tr>";

            html += " </thead>";
            html += " <tbody>";
            //  html += "<tr style='height:200px;'><td style='width:35%;font-size:15px;'>" + ndtMethods + "</td>";
            //html += "<td style='width:10%;font-size:15px;'>I I</td>";
            //html += "<td style='width:12%;font-size:15px;'>" + TrainingHrs + "</td>";
            if (MT_Pass == "Pass")
            {
                html += "<tr style='height:{rowHeight}px;'><td style='width:35%;font-size:17px;'>Magnetic Particle Testing (MT)</td>";
                html += "<td style='width:10%;font-size:17px;'>I I</td>";
                html += "<td style='width:10%;font-size:17px;'>40</td>";
                html += "<td style='width:13%;font-size:17px;'>" + MTGrd + "</td>";
                html += "<td style='width:14%;font-size:17px;'>" + MTcertificationDate + "</td><td style='width:12%;font-size:17px;'>" + MTCertvalidTill + "</td></tr>";
            }
            if (UT_Pass == "Pass")
            {
                html += "<tr style='height:{rowHeight}px;'><td style='width:35%;font-size:15px;'>Ultrasonic Testing (UT)</td>";
                html += "<td style='width:10%;font-size:17px;'>I I</td>";
                html += "<td style='width:10%;font-size:17px;'>60</td>";
                html += "<td style='width:13%;font-size:17px;'>" + UTGrd + "</td>";
                html += "<td style='width:14%;font-size:17px;'>" + UTcertificationDate + "</td><td style='width:12%;font-size:17px;'>" + UTCertvalidTill + "</td></tr>";
            }
            if (VT_Pass == "Pass")
            {
                html += "<tr style='height:{rowHeight}px;'><td style='width:35%;font-size:15px;'>Visual Testing (VT)</td>";
                html += "<td style='width:10%;font-size:17px;'>I I</td>";
                html += "<td style='width:10%;font-size:17px;'>40</td>";
                html += "<td style='width:13%;font-size:17px;'>" + VTGrd + "</td>";
                html += "<td style='width:14%;font-size:17px;'>" + VTcertificationDate + "</td><td style='width:12%;font-size:17px;'>" + VTCertvalidTill + "</td></tr>";
            }
            if (RT_Pass == "Pass")
            {
                html += "<tr style='height:{rowHeight}px;'><td style='width:35%;font-size:15px;'>Radiopgraphy Testing (RT)</td>";
                html += "<td style='width:10%;font-size:17px;'>I I</td>";
                html += "<td style='width:10%;font-size:17px;'>20</td>";
                html += "<td style='width:13%;font-size:17px;'>" + RTGrd + "</td>";
                html += "<td style='width:14%;font-size:17px;'>" + RTcertificationDate + "</td><td style='width:12%;font-size:17px;'>" + RTCertvalidTill + "</td></tr>";
            }
            if (PT_Pass == "Pass")
            {
                html += "<tr style='height:{rowHeight}px;'><td style='width:35%;font-size:15px;'>Liquid Penetrant Testing (PT)</td>";
                html += "<td style='width:10%;font-size:17px;'>I I</td>";
                html += "<td style='width:10%;font-size:17px;'>80</td>";
                html += "<td style='width:13%;font-size:17px;'>" + PTGrd + "</td>";
                html += "<td style='width:14%;font-size:17px;'>" + PTcertificationDate + "</td><td style='width:12%;font-size:17px;'>" + PTCertvalidTill + "</td></tr>";
            }




            html += " </tbody>";
            html += "</table>";



            html += "<div>";
            html += "<p style='font-size:17px;margin-top:20px;margin-bottom:20px;'>This certificate remains valid contingent upon valid annual vision examination, consistent satisfactory performance, and periodic technical performance evaluation.</p>";
            html += "<p style='font-size:17px;'> TUV India Private Limited's written practice document, TUVIS/WP/01, Revision 07, dated 16.09.2024, titled 'Written Practice for Training, ";

            html += " Examination, Qualification, and certification of NDT Personnel,' is prepared in accordance with the recommended practice ASNT SNT-TC-1A (2024)";
            html += " and is intended to meet or exceed its requirements.</p>";

            html += "<p style='font-size:17px;margin-top:20px;'><b>Designated level III :</b> Pralhad Gawade, ASNT NDT III - VT, PT, MT, RT and UT(203975)</p>";
            html += "<p style='font-size:17px;margin-top: -7px;'><b>Certified By :</b> Sandeep Deshpande, Technical Director</p>";

            html += "<p style='font-size:13px;margin-top:20px;'><b><i>Note :</b> This document is system-generated and does not require signature.</p></i>";
            html += "<p style='margin-top: -5px;font-size:13px;'><b><i>Disclaimer :</b> This certificate is valid exclusively for assignments authorized by TUV India Private Limited.</p></i>";
            html += "<p style='margin-top: -10px;font-size:13px;'><b><i>Copyright :</b> This document is the property of TUV India Pvt Ltd.and should not br reproduced, except in full without the consentof TUV India Private Limited.</p></i>";
            html += "<div id='footer-container'>";
            html += "<p style='margin-top: 15px;font-size:11px;'><b>TUV India Pvt. Ltd. (TUV NORD GROUP) : (REGD. & HEAD OFFICE)</b>, 801, Raheja Plaza I, LBS Marg, Ghatkopar (West), Mumbai-400086, Maharashtra, India.</p>";
            html += "<p style='margin-top: -10px;font-size:11px;'>Tel : + 91 22 66477000, Email : <a href='inspection@tuvnord.com'>inspection@tuvnord.com</a>; Website : <a href='www.tuvnord.com'>www.tuvnord.com/in</a>.</p>";
            html += "<p style='margin-top: -10px;font-size:11px;'><b>Form Number</b> : F-INSP-NDTC Rev.00</p>";
            html += "<img id='footer-logo' src='https://tiimes.tuv-india.com/AllJsAndCss/images/FTUEV-NORD-GROUP_Logo_Electric-Blue.svg' alt='TUV India Logo'>";
            html += "</div>";
            html += "</div>";
            html += "</div>";

            html += "</body>";
            html += "</html>";

            try
            {
                //System.IO.File.WriteAllText("~/TempImages/debugHtml.html", html);
                string debugPath = Server.MapPath("~/TempImages/debugHtml.html");
System.IO.File.WriteAllText(debugPath, html);
                //System.IO.File.WriteAllText(Server.MapPath("~/QuotationHtml/debugHtml.html", html));
                var doc = converter.ConvertHtmlString(html);
                if (System.IO.File.Exists(Server.MapPath(filePath)))
                {
                    System.IO.File.Delete(Server.MapPath(filePath));
                }
                doc.Save(Server.MapPath(filePath));
            }
            catch (Exception ex)
            {
                var exxx = ex.Message;
                //DALCalls.LogFile(ex.Message + ";" + ex.StackTrace, "GenerateCert");
            }

            return filePath;
        }

    }
}