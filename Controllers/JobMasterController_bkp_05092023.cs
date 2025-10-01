using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using TuvVision.DataAccessLayer;
using System.IO;
using System.Text;
using OfficeOpenXml;
using NonFactors.Mvc.Grid;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace TuvVision.Controllers
{
    public class JobMasterController : Controller
    {
        CommonControl objCommonControl = new CommonControl();
        DALJob objDalCompany = new DALJob();
        JobMasters ObjModelJob = new JobMasters();
        DALTrainingCreation objDTC = new DALTrainingCreation();
        DALSubJob objDalSubjob = new DALSubJob();
        DALQuotationMaster objQuot = new DALQuotationMaster();
		DALQuotationMaster objDALQuotationMast = new DALQuotationMaster();
        DataTable DSGetCheckList = new DataTable();
        List<JobMasters> LCheckList = new List<JobMasters>();
        int ManDayCount = 0;
		double ManHrCount = 0;
        // GET: JobMaster
        public ActionResult JobList()
        {
            DataTable JobDashBoard = new DataTable();
            List<JobMasters> lstCompanyDashBoard = new List<JobMasters>();
            JobDashBoard = objDalCompany.GetJOBList();
            try
            {
                if (JobDashBoard.Rows.Count > 0)
                {
                    //int abc = JobDashBoard.Rows.Count;
                    // int data = 1 + abc;
                    foreach (DataRow dr in JobDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new JobMasters
                            {
                                PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                Job_Number = Convert.ToString(dr["Job_Number"]),
                                Quotation_Of_Order = Convert.ToString(dr["Quotation_Of_Order"]),
                                Enquiry_Of_Order = Convert.ToString(dr["Enquiry_Of_Order"]),
                                Client_Name = Convert.ToString(dr["Client_Name"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                End_User = Convert.ToString(dr["End_User"]),
                                Service_type = Convert.ToString(dr["Service_type"]),
                                Job_type = Convert.ToString(dr["Job_type"]),
                                Customer_PoNo_PoDate = Convert.ToString(dr["Customer_PoNo_PoDate"]),
                                Customer_PO_Amount = Convert.ToDecimal(dr["Customer_PO_Amount"]),
                                Po_Validity = Convert.ToString(dr["Po_Validity"]),
                                Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                                Contract_reviewList = Convert.ToString(dr["Contract_reviewList"]),
                                Special_Notes = Convert.ToString(dr["Special_Notes"]),
                                formats_Of_Report = Convert.ToString(dr["formats_Of_Report"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Description = Convert.ToString(dr["Description"]),
                                PK_QT_ID = Convert.ToInt32(dr["PK_QT_ID"]),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                Customer_PoDate = Convert.ToString(dr["Customer_PoDate"]),
                                chkARCType = Convert.ToString(dr["chkARC"]),

                            }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["JobList"] = lstCompanyDashBoard;
            ObjModelJob.lstCompanyDashBoard1 = lstCompanyDashBoard;
            return View(ObjModelJob);
        }


        [HttpGet]
        public ActionResult JobCreation(int? PK_JOB_ID, int? PK_QT_ID)
        {
            #region Bind Branch
            DataSet dsBindBranch = new DataSet();
            List<BranchName> lstBranch = new List<BranchName>();
            dsBindBranch = objDTC.BindBranch();

            if (dsBindBranch.Tables[0].Rows.Count > 0)
            {
                lstBranch = (from n in dsBindBranch.Tables[0].AsEnumerable()
                             select new BranchName()
                             {
                                 Name = n.Field<string>(dsBindBranch.Tables[0].Columns["Branch_Name"].ToString()),
                                 Code = n.Field<Int32>(dsBindBranch.Tables[0].Columns["BR_Id"].ToString())

                             }).ToList();
            }

            IEnumerable<SelectListItem> BranchItems;
            BranchItems = new SelectList(lstBranch, "Code", "Name");
            ViewBag.ProjectTypeItems = BranchItems;
            ViewData["BranchName"] = BranchItems;
            
            //////////////////Currency
            DataSet dsCurrency = new DataSet();
            List<CurrencyName> lstCurrency = new List<CurrencyName>();
            dsCurrency = objDTC.BindCurrency();

            if (dsCurrency.Tables[0].Rows.Count > 0)
            {
                lstCurrency = (from n in dsCurrency.Tables[0].AsEnumerable()
                             select new CurrencyName()
                             {
                                 Name = n.Field<string>(dsCurrency.Tables[0].Columns["CurrencyCode"].ToString()),
                                 Code = n.Field<Int32>(dsCurrency.Tables[0].Columns["CurrencyID"].ToString())

                             }).ToList();
            }

            IEnumerable<SelectListItem> CurrencyItems;
            CurrencyItems = new SelectList(lstCurrency, "Code", "Name");
            ViewBag.CurrencyItems = CurrencyItems;
            ViewData["CurrencyItems"] = CurrencyItems;
            ObjModelJob.Currency = "1";
			ViewBag.DTestCurrency = lstCurrency;
            ViewBag.ITestCurrency = lstCurrency;


            #endregion

            #region Bind CheckList
            try
            {

                DSGetCheckList = objDalCompany.GetJobCheckList();
                if (DSGetCheckList.Rows.Count > 0)
                {
                    foreach (DataRow dr in DSGetCheckList.Rows)
                    {
                        LCheckList.Add(
                            new JobMasters
                            {
                                CheckListId = Convert.ToString(dr["PkId"]),
                                CheckListName = Convert.ToString(dr["Name"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewBag.CostSheet = LCheckList;
            #endregion

            DataTable dtUserRole = new DataTable();
            dtUserRole = objDALQuotationMast.GetUserRole();

            if (dtUserRole.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUserRole.Rows)
                {
                    ViewBag.Role = Convert.ToString(dr["FK_RoleID"]);
                }
            }

            if (PK_QT_ID > 0)
            {
                string JobCreationType = string.Empty;

                DataTable dtJobCreationType = new DataTable();

                //string JobCreationType = string.Empty;

                //DataTable dtJobCreationType = new DataTable();


                dtJobCreationType = objDALQuotationMast.JobCreationType(PK_QT_ID);

                if (dtJobCreationType.Rows.Count > 0)
                {
                    if (dtJobCreationType.Rows.Count > 1)
                    { JobCreationType = "3"; }
                    else
                    {
                        JobCreationType = dtJobCreationType.Rows[0]["CostsheetType"].ToString();
                    }
                }
                else
                {
                    JobCreationType = "1";
                }

                #region Bind Domestic International

                if (JobCreationType=="1")
                {
                    #region International Order Type 
                    DataTable dtIOrderType = new DataTable();
                    List<JobMasters> lstIOrderType = new List<JobMasters>();

                    dtIOrderType = objDALQuotationMast.IOrderTypeJob(PK_QT_ID);
                    if (dtIOrderType.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtIOrderType.Rows)
                        {
                            lstIOrderType.Add(
                               new JobMasters
                               {

                                   IOrderType = Convert.ToString(dr["IOrderType"]),
                                   IOrderRate = Convert.ToString(dr["IOrderRate"]),
                                   IEstimate_ManDays_ManMonth = Convert.ToString(dr["IEstimate_ManDays_ManMonth"]),
                                   IDistance = Convert.ToString(dr["IDistance"]),
                                   IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                                   Icurrency = Convert.ToString(dr["Icurrency"]),
                                   IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                                   ITotalAmount = Convert.ToString(dr["ITotalAmount"]),
                                   IReadOnly = Convert.ToString(dr["IReadOnly"]),
                                   IOrderTypeId = Convert.ToString(dr["Id"]),
                               }
                             );
                        }
                        //ViewBag.lstIOrderType = lstIOrderType;
                    }
                    ViewBag.lstIOrderType = lstIOrderType;
                    #endregion
                    
                }
                else if(JobCreationType == "2")
                {
                    #region Domestic Order Type 
                    DataTable dtDOrderType = new DataTable();
                    List<JobMasters> lstDOrderType = new List<JobMasters>();

                    dtDOrderType = objDALQuotationMast.DOrderTypeJob(PK_QT_ID);
                    if (dtDOrderType.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDOrderType.Rows)
                        {
                            lstDOrderType.Add(
                               new JobMasters
                               {

                                   OrderType = Convert.ToString(dr["OrderType"]),
                                   OrderRate = Convert.ToString(dr["OrderRate"]),
                                   Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                                   Distance = Convert.ToString(dr["Distance"]),
                                   EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                                   Dcurrency = Convert.ToString(dr["Dcurrency"]),
                                   DExchangeRate = Convert.ToString(dr["DExchangeRate"]),
                                   DTotalAmount = Convert.ToString(dr["DTotalAmount"]),
                                   DReadOnly = Convert.ToString(dr["DReadOnly"]),
                                   OrderTypeId = Convert.ToString(dr["Id"]),
                               }
                             );
                        }
                        ViewBag.lstDOrderType = lstDOrderType;

                    }
                    #endregion
                }
                else if (JobCreationType == "3")
                {
                    #region Domestic Order Type 
                    DataTable dtDOrderType = new DataTable();
                    List<JobMasters> lstDOrderType = new List<JobMasters>();

                    dtDOrderType = objDALQuotationMast.DOrderTypeJob(PK_QT_ID);
                    if (dtDOrderType.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDOrderType.Rows)
                        {
                            lstDOrderType.Add(
                               new JobMasters
                               {

                                   OrderType = Convert.ToString(dr["OrderType"]),
                                   OrderRate = Convert.ToString(dr["OrderRate"]),
                                   Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                                   Distance = Convert.ToString(dr["Distance"]),
                                   EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                                   Dcurrency = Convert.ToString(dr["Dcurrency"]),
                                   DExchangeRate = Convert.ToString(dr["DExchangeRate"]),
                                   DTotalAmount = Convert.ToString(dr["DTotalAmount"]),
                                   DReadOnly = Convert.ToString(dr["DReadOnly"]),
                                   OrderTypeId = Convert.ToString(dr["Id"]),
                               }
                             );
                        }
                        ViewBag.lstDOrderType = lstDOrderType;

                    }
                    #endregion

                    #region International Order Type 
                    DataTable dtIOrderType = new DataTable();
                    List<JobMasters> lstIOrderType = new List<JobMasters>();

                    dtIOrderType = objDALQuotationMast.IOrderTypeJob(PK_QT_ID);
                    if (dtIOrderType.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtIOrderType.Rows)
                        {
                            lstIOrderType.Add(
                               new JobMasters
                               {

                                   IOrderType = Convert.ToString(dr["IOrderType"]),
                                   IOrderRate = Convert.ToString(dr["IOrderRate"]),
                                   IEstimate_ManDays_ManMonth = Convert.ToString(dr["IEstimate_ManDays_ManMonth"]),
                                   IDistance = Convert.ToString(dr["IDistance"]),
                                   IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                                   Icurrency = Convert.ToString(dr["Icurrency"]),
                                   IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                                   ITotalAmount = Convert.ToString(dr["ITotalAmount"]),
                                   IReadOnly = Convert.ToString(dr["IReadOnly"]),
                                   IOrderTypeId = Convert.ToString(dr["Id"]),
                               }
                             );
                        }
                        //ViewBag.lstIOrderType = lstIOrderType;
                    }
                    ViewBag.lstIOrderType = lstIOrderType;
                    #endregion
                }






                #endregion

                #region
                
                DataSet DSJobMasterByQtId = new DataSet();
                DataSet DSEditQutationTabledata = new DataSet();

                DSEditQutationTabledata = objDalCompany.GetQutationDetails(PK_QT_ID);
                

              
                if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
                {
                    ObjModelJob.FK_CMP_ID = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FK_CMP_ID"]);
                    ObjModelJob.PK_QT_ID = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_QTID"]);
                    ObjModelJob.Description = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Quotation_Description"]);
                    ObjModelJob.Quotation_Of_Order = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["QuotationNumber"]);
                    ObjModelJob.Enquiry_Of_Order = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["EnquiryNumber"]);
                    ObjModelJob.Client_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["CompanyName"]);
                    ObjModelJob.Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["BranchName"]);
                    ObjModelJob.End_User = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["EndCustomer"]);
                    ObjModelJob.Job_type = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["ProjectType"]);
                    ObjModelJob.subserviceType = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["ServiceType"]);
                    ObjModelJob.PortfolioType = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["PortfolioName"]);

                    ObjModelJob.OBSID = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["OBSID"]);
                    ObjModelJob.ServiceID = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SErviceID"]);
                    ObjModelJob.PortID = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["PortID"]);

                    ObjModelJob.PortCode = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["PortCode"]);
                    ObjModelJob.ServiceCode = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["ServiceCode"]);

					ObjModelJob.OrderRate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["OrderRate"]);
                    ObjModelJob.Estimate_ManDays_ManMonth = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Estimate_ManDays_ManMonth"]);
                    ObjModelJob.OrderType = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["OrderType"]);
                    ObjModelJob.Exclusion = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Exclusion"]);
                    ObjModelJob.Distance = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Distance"]);
                    ObjModelJob.chkARC = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["chkARC"]);
                    ObjModelJob.CostSheetApproved = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["CostSheetApproved"]);																								 
                }
                #endregion


                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDalCompany.EditUploadedFile(PK_JOB_ID);

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
                    ObjModelJob.FileDetails = lstEditFileDetails;
                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file

                //**********************************************Code Added by Manoj Sharma for Delete file and update file
                DataTable DTGetUploadedFileFormat = new DataTable();
                List<FileDetails> lstEditFileDetailsFormat = new List<FileDetails>();
                DTGetUploadedFileFormat = objDalCompany.EditUploadedFileFormat(PK_JOB_ID);
                if (DTGetUploadedFile.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTGetUploadedFileFormat.Rows)
                    {
                        lstEditFileDetailsFormat.Add(
                           new FileDetails
                           {

                               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                               FileName = Convert.ToString(dr["FileName"]),
                               Extension = Convert.ToString(dr["Extenstion"]),
                               IDS = Convert.ToString(dr["FileID"]),
                           }
                         );
                    }
                    ViewData["lstEditFileDetailsFormat"] = lstEditFileDetailsFormat;
                    ObjModelJob.FileDetailsFormat = lstEditFileDetailsFormat;
                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file
                //return View(ObjModelJob);
            }
            else if (PK_JOB_ID > 0)
            {
                #region 

                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalCompany.EditJob(PK_JOB_ID);

                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ObjModelJob.PK_JOB_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_JOB_ID"]);
                    ObjModelJob.Description = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Description"]);
                    ObjModelJob.Quotation_Of_Order = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Quotation_Of_Order"]);
                    ObjModelJob.Enquiry_Of_Order = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Enquiry_Of_Order"]);
                    ObjModelJob.Client_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Name"]);
                    ObjModelJob.Branch = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Branch"]);
                    ObjModelJob.End_User = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["End_User"]);
                    ObjModelJob.Service_type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Service_type"]);
                    ObjModelJob.Job_type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Job_type"]);
                    ObjModelJob.Customer_PoNo_PoDate = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Customer_PoNo_PoDate"]);
                    ObjModelJob.Customer_PO_Amount = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["Customer_PO_Amount"]);
                    ObjModelJob.Po_Validity = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Po_Validity"]);
                    ObjModelJob.Estimate_ManDays_ManMonth = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Estimate_ManDays_ManMonth"]);
                    ObjModelJob.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Status"]);
                    ObjModelJob.Contract_reviewList = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Contract_reviewList"]);
                    ObjModelJob.Special_Notes = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Special_Notes"]);
                    ObjModelJob.formats_Of_Report = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["formats_Of_Report"]);
                    ObjModelJob.Attachment = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Attachment"]);
                    ObjModelJob.Job_Number = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Job_Number"]);
                    ObjModelJob.SAP_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SAP_No"]);
                    ObjModelJob.GstDetails_BillingAddress = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["GstDetails_BillingAddress"]);


                    
                    ObjModelJob.Customer_PoDate = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Customer_PoDate"]);
                    ObjModelJob.EQ_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["EQ_ID"]);
                    ObjModelJob.CMP_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["CMP_ID"]);

                    ObjModelJob.FirstYear = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["FirstYear"]);
                    ObjModelJob.SecondYear = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["SecondYear"]);
                    ObjModelJob.ThirdYear = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["ThirdYear"]);
                    ObjModelJob.FourthYear = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["FourthYear"]);
                    ObjModelJob.Balance = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["Balance"]);
                    ObjModelJob.OrderType = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["OrderType"]);
                    ObjModelJob.OrderRate = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["OrderRate"]);
                    ObjModelJob.InvoiceAmount = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["InvoiceAmount"]);

                    ObjModelJob.subserviceType = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["subServiceType"]);
                    ObjModelJob.PortfolioType = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["PortfolioType"]);
                    ObjModelJob.OBSID = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["OBSID"]);
                    ObjModelJob.ServiceID = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SErviceID"]);
                    ObjModelJob.PortID = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["PortID"]);
                    //ObjModelJob.PortCode = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["PortCode"]);
                    //ObjModelJob.ServiceCode = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ServiceCode"]);
                    ObjModelJob.OrderStatus = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["OrderStatus"]);
                    ObjModelJob.DECName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECName"]);
                    ObjModelJob.DECNumber = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECNumber"]);
                    ObjModelJob.Currency = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Currency"]);
                    ObjModelJob.SubProjectName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubProjectName"]);
					ObjModelJob.OrderRate = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["OrderRate"]);
                    ObjModelJob.OrderType = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["OrderType"]);
                    ObjModelJob.Distance = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Distance"]);
                    ObjModelJob.Estimate_ManMonth = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Estimate_ManMonth"]);
                    ObjModelJob.Estimate_ManDays_ManMonth = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Estimate_ManDays_ManMonth"]);
                    ObjModelJob.Estimate_ManHours = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Estimate_ManHR"]);

                    ObjModelJob.chkARC = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["chkARC"]);
                    ObjModelJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["checkIFCustomerSpecific"]);
                    ObjModelJob.POAwaited = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["POAwaited"]);
                    ObjModelJob.CostsheetApproval = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["CostsheetApproval"]);
                    ObjModelJob.Consume = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ConsumeCall"]);
                    ObjModelJob.Remaining = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["RemainingCall"]);
                    ObjModelJob.ProposedCall = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ProposedCall"]);
                    ObjModelJob.POMandays = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["POMandays"]);
                    
                    ObjModelJob.InspectionLocation = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["InspectionLocation"]);
                    ObjModelJob.EstimatedAmount = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Amount"]);
                    ObjModelJob.CheckListId = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["JobReviewCheckList"]);
                    ObjModelJob.CheckListDescription = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["JobReviewCheckListDescription"]);
                 //   ObjModelJob.Address = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Address"]);
                }
                
                //**********************************************Code Added by Manoj Sharma for Delete file and update file 13 March 2020
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDalCompany.EditUploadedFile(PK_JOB_ID);
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
                    ObjModelJob.FileDetails = lstEditFileDetails;
                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file Format Report
                DataTable DTGetUploadedFileFormat = new DataTable();
                List<FileDetails> lstEditFileDetailsFormat = new List<FileDetails>();
                DTGetUploadedFileFormat = objDalCompany.EditUploadedFileFormat(PK_JOB_ID);
                if (DTGetUploadedFileFormat.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTGetUploadedFileFormat.Rows)
                    {
                        lstEditFileDetailsFormat.Add(
                           new FileDetails
                           {

                               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                               FileName = Convert.ToString(dr["FileName"]),
                               Extension = Convert.ToString(dr["Extenstion"]),
                               IDS = Convert.ToString(dr["FileID"]),
                           }
                         );
                    }
                    ViewData["lstEditFileDetailsFormat"] = lstEditFileDetailsFormat;
                    ObjModelJob.FileDetailsFormat = lstEditFileDetailsFormat;
                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file

                DataTable DTCallSummary = new DataTable();
                DTCallSummary = objDalCompany.GetCallSummary(PK_JOB_ID);
                List<CallsModel> lstCallSummary = new List<CallsModel>();

                if (DTCallSummary.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallSummary.Rows)
                    {
                        lstCallSummary.Add(
                           new CallsModel
                           {

                               OpenCalls = Convert.ToString(dr["OpenCalls"]),
                               ClosedCalls = Convert.ToString(dr["ClosedCalls"]),
                               AssignedCalls = Convert.ToString(dr["AssignedCalls"]),
                               NotDoneCalls = Convert.ToString(dr["NotDoneCalls"]),
                               CancelledCalls = Convert.ToString(dr["CancelledCalls"]),
                               TotalCalls = Convert.ToString(dr["TotalCalls"]),
                           }
                         );
                    }
                    ViewData["lstCallSummary"] = lstCallSummary;
                    ObjModelJob.lstCallSummary = lstCallSummary;
                }

                DataTable DTAddMandays = new DataTable();
                DTAddMandays = objDalCompany.GetAddMandays(PK_JOB_ID);
                List<JobMasters> lstAddMandays = new List<JobMasters>();

                if (DTAddMandays.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAddMandays.Rows)
                    {
                        lstAddMandays.Add(
                           new JobMasters
                           {
                               PK_ADDPOID = Convert.ToInt32(dr["PK_ADDPOID"]),
                               Add_PoReason = Convert.ToString(dr["Reason"]),
                               Add_MandayDesc = Convert.ToString(dr["Description"]),
                               Add_Mandays = Convert.ToString(dr["ManDays"]),
                               Add_PoNumber = Convert.ToString(dr["PoNo"]),
                               Add_PoDate = Convert.ToString(dr["PoDate"]),
                               Add_PoValidity = Convert.ToString(dr["Po_Validity"]),
                               Add_POAmt = Convert.ToDecimal(dr["PO_Amount"]),
                               CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           }
                         );
                    }
                    ViewData["lstAddMandays"] = lstAddMandays;
                    ObjModelJob.lstAddMandays = lstAddMandays;
                }

                #endregion


            }
            else
            {
                return RedirectToAction("JobList");
            }

            #region
            DataTable subJobDashBoard = new DataTable();
            List<SubJobs> lstSubJobDashBoard = new List<SubJobs>();
            subJobDashBoard = objDalSubjob.GetJobDetailsCountBydata(PK_JOB_ID);
            try
            {
                if (subJobDashBoard.Rows.Count > 0)
                {

                    foreach (DataRow dr in subJobDashBoard.Rows)
                    {

                        lstSubJobDashBoard.Add(
                            new SubJobs
                            {
                                #region
                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Control_Number = Convert.ToString(dr["Control_Number"]),
                                Service_type = Convert.ToString(dr["Service_type"]),
                                vendor_name = Convert.ToString(dr["vendor_name"]),
                                Vendor_Po_No = Convert.ToString(dr["Vendor_Po_No"]),
                                Date_of_Po = dr["Date_of_Po"].ToString() == string.Empty ? string.Empty : dr["Date_of_Po"].ToString(),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Status = Convert.ToString(dr["Status"]),
                                Client_Email = Convert.ToString(dr["Client_Email"]),
                                Vendor_Email = Convert.ToString(dr["Vendor_Email"]),
                                Tuv_Email = Convert.ToString(dr["Tuv_Email"]),
                                Client_Contact = Convert.ToString(dr["Client_Contact"]),
                                Vendor_Contact = Convert.ToString(dr["Vendor_Contact"]),
                                Sub_Vendor_Contact = Convert.ToString(dr["Sub_Vendor_Contact"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Type = Convert.ToString(dr["Type"]),
                                #endregion


                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["subJobList"] = lstSubJobDashBoard;
            #endregion


            #region Bind new MVC-Grid SubJob
            //DataTable subJobDashBoard = new DataTable();
            List<JobMasters> lstSubJob = new List<JobMasters>();
            subJobDashBoard = objDalSubjob.GetJobDetailsCountBydata(PK_JOB_ID);
            try
            {
                if (subJobDashBoard.Rows.Count > 0)
                {

                    foreach (DataRow dr in subJobDashBoard.Rows)
                    {

                        lstSubJob.Add(
                            new JobMasters
                            {
                                #region
                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Control_Number = Convert.ToString(dr["Control_Number"]),
                                Service_type = Convert.ToString(dr["Service_type"]),
                                vendor_name = Convert.ToString(dr["vendor_name"]),
                                Vendor_Po_No = Convert.ToString(dr["Vendor_Po_No"]),
                                Date_of_Po = dr["Date_of_Po"].ToString() == string.Empty ? string.Empty : dr["Date_of_Po"].ToString(),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Status = Convert.ToString(dr["Status"]),
                                Client_Email = Convert.ToString(dr["Client_Email"]),
                                Vendor_Email = Convert.ToString(dr["Vendor_Email"]),
                                Tuv_Email = Convert.ToString(dr["Tuv_Email"]),
                                Client_Contact = Convert.ToString(dr["Client_Contact"]),
                                Vendor_Contact = Convert.ToString(dr["Vendor_Contact"]),
                                Sub_Vendor_Contact = Convert.ToString(dr["Sub_Vendor_Contact"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Type = Convert.ToString(dr["Type"]),
                                V1 = Convert.ToString(dr["V1"]),
                                V2 = Convert.ToString(dr["V2"]),
                                P1 = Convert.ToString(dr["P1"]),
                                P2 = Convert.ToString(dr["P2"]),
                                
                                #endregion

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            //ViewData["subJobList"] = lstSubJobDashBoard;
            ObjModelJob.lstSubJob = lstSubJob;
            #endregion

            return View(ObjModelJob);
        }


        [HttpPost]
        public ActionResult JobCreation(JobMasters JM, HttpPostedFileBase[] Image, HttpPostedFileBase[] Image1, FormCollection fc, List<HttpPostedFileBase> img_Banner)
        {
            string Result = string.Empty;
            int JobIDs = 0;
            List<JobMasters> lstSubJob = new List<JobMasters>();
            //For Attachment 
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listJobMasterUploadedFile"] as List<FileDetails>;
            //For Format Report
            List<FileDetails> lstFileDtlsFormat = new List<FileDetails>();
            lstFileDtlsFormat = Session["listFormatFile"] as List<FileDetails>;

            DataTable dtUserRole = new DataTable();
            dtUserRole = objDALQuotationMast.GetUserRole();

            if (dtUserRole.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUserRole.Rows)
                {
                    ViewBag.Role = Convert.ToString(dr["FK_RoleID"]);
                }

            }

            string IPath = string.Empty;
            var list = Session["list"] as List<string>;
            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }

            List<string> lstAttachment = new List<string>();
           

            
            string IPath1 = string.Empty;
            var list1 = Session["list1"] as List<string>;
            if (list1 != null && list1.Count != 0)
            {
                IPath1 = string.Join(",", list1.ToList());
                IPath1 = IPath1.TrimEnd(',');
            }

            List<string> lstAttachment1 = new List<string>();
           
            //////////////////Currency
            DataSet dsCurrency = new DataSet();
            List<CurrencyName> lstCurrency = new List<CurrencyName>();
            dsCurrency = objDTC.BindCurrency();

            if (dsCurrency.Tables[0].Rows.Count > 0)
            {
                lstCurrency = (from n in dsCurrency.Tables[0].AsEnumerable()
                               select new CurrencyName()
                               {
                                   Name = n.Field<string>(dsCurrency.Tables[0].Columns["CurrencyCode"].ToString()),
                                   Code = n.Field<Int32>(dsCurrency.Tables[0].Columns["CurrencyID"].ToString())

                               }).ToList();
            }

            IEnumerable<SelectListItem> CurrencyItems;
            CurrencyItems = new SelectList(lstCurrency, "Code", "Name");
            ViewBag.CurrencyItems = CurrencyItems;
            ViewData["CurrencyItems"] = CurrencyItems;


            if (JM.PK_JOB_ID == 0)
            {
				#region ForDomestic
                if(JM.lstQuotationMasterOrderType != null)
                {
                    foreach (var item in JM.lstQuotationMasterOrderType)
                    {
                        if (item.RadioButton == true)
                        {

                            if (item.OrderType == "ManMonth")
                            {
                                ManDayCount = Convert.ToInt32(ConfigurationManager.AppSettings["MonthRate"].ToString()) * Convert.ToInt32(item.Estimate_ManDays_ManMonth.ToString());
                                JM.Estimate_ManDays_ManMonth = ManDayCount.ToString();
                                JM.Estimate_ManMonth = item.Estimate_ManDays_ManMonth;
                            }
                            else if (item.OrderType == "ManHR")
                            {
                                ManDayCount = Convert.ToInt32(item.Estimate_ManDays_ManMonth.ToString()) / 8;                                
                                ManHrCount = Convert.ToDouble(item.Estimate_ManDays_ManMonth.ToString()) / 8;

                                if (ManHrCount > ManDayCount)
                                {
                                    ManHrCount = ManDayCount + 0.5;
                                    JM.Estimate_ManDays_ManMonth = ManHrCount.ToString();
                                }
                                else
                                {                                    
                                    JM.Estimate_ManDays_ManMonth = ManDayCount.ToString();
                                }
                                JM.Estimate_ManHours = item.Estimate_ManDays_ManMonth.ToString();


                            }
                            else
                            {
                                JM.Estimate_ManDays_ManMonth = item.Estimate_ManDays_ManMonth;
                            }


                            JM.OrderType = item.OrderType;
                            JM.OrderRate = item.OrderRate;
                            JM.Distance = item.Distance;
                            JM.Currency = item.Dcurrency;
                            JM.DExchangeRate = item.DExchangeRate;
                            JM.DTotalAmount = item.DTotalAmount;
                            JM.OrderTypeId = item.OrderTypeId;
                            JM.Customer_PO_Amount = Convert.ToDecimal(item.DTotalAmount);

                        }
                        else
                        {

                        }



                    }
                }
              
                #endregion

                #region ForInternational
                if (JM.lstQuotationMasterOrderTypeI != null)
                {
                    foreach (var item1 in JM.lstQuotationMasterOrderTypeI)
                    {
                        if (item1.IRadioButton == true)
                        {
                            if (item1.IOrderType == "ManMonth")
                            {
                                ManDayCount = Convert.ToInt32(ConfigurationManager.AppSettings["MonthRate"].ToString()) * Convert.ToInt32(item1.IEstimate_ManDays_ManMonth.ToString());
                                JM.Estimate_ManDays_ManMonth = ManDayCount.ToString();
                                JM.Estimate_ManMonth = item1.IEstimate_ManDays_ManMonth;
                            }
                            else
                            {
                                JM.Estimate_ManDays_ManMonth = item1.IEstimate_ManDays_ManMonth;
                            }


                            JM.OrderType = item1.IOrderType;
                            JM.OrderRate = item1.IOrderRate;
                            JM.Distance = item1.IDistance;
                            JM.Currency = item1.Icurrency;
                            JM.DExchangeRate = item1.IExchangeRate;
                            JM.DTotalAmount = item1.ITotalAmount;
                            JM.OrderTypeId = item1.IOrderTypeId;

                            JM.Customer_PO_Amount = Convert.ToDecimal(item1.ITotalAmount);
                        }
                        else
                        {

                        }
                    }
                }
                
                #endregion			 

				if (JM.chkARC)
                {
                    JM.chkARC = true;
																		  
                }
                else
                {
                    JM.chkARC = false;
                }							 
                DataSet Branchiddata = new DataSet();
                //Branchiddata = objDalCompany.GetBranchid(JM);
                Branchiddata = objDalCompany.GetBranchCode(JM);

                if (Branchiddata.Tables[0].Rows.Count > 0)
                {
                    //ObjModelJob.Br_Id = Convert.ToInt32(Branchiddata.Tables[0].Rows[0]["Br_Id"]);
                    ObjModelJob.Branch_Code = Convert.ToString(Branchiddata.Tables[0].Rows[0]["BranchCode"]);
                }
                ObjModelJob.Service_type = JM.subserviceType;
                //DataSet Servicesid = new DataSet();
                //Servicesid = objDalCompany.GetServicesTypeID(JM);
                //if (Servicesid.Tables[0].Rows.Count > 0)
                //{
                //    ObjModelJob.Service_type = Convert.ToString(Servicesid.Tables[0].Rows[0]["PK_ID"]);
                //}
                string strPreFixJob = string.Empty;
                strPreFixJob = JM.PortCode.ToString().Trim() + JM.ServiceCode.ToString().Trim(); 
                DataTable Countdata = new DataTable();
                Countdata = objDalCompany.JobNoSet(JM);
                int s = Countdata.Rows.Count;

                int YY = DateTime.Now.Year % 100;

                DataTable dtJobNoUniqueId = new DataTable();
                dtJobNoUniqueId = objDalCompany.JobNoUniqueId();
                if (dtJobNoUniqueId.Rows.Count > 0)
                {
                    ObjModelJob.JobNoUniqueId = Convert.ToString(dtJobNoUniqueId.Rows[0]["Job_number"]);
                }

                //string Jobno = ObjModelJob.Service_type+ObjModelJob.Br_Id+"00"+s+YY;
                string JobNo = string.Empty;
                string Energy = string.Empty;
                string Others = string.Empty;
                DataSet dtType = new DataSet();
                
               
                JobNo = strPreFixJob + ObjModelJob.Branch_Code + ObjModelJob.JobNoUniqueId + YY;
                JM.Job_Number = JobNo;

                if(JM.OrderType == "ManMonth")   
                {
                    ManDayCount = Convert.ToInt32(ConfigurationManager.AppSettings["MonthRate"].ToString()) * Convert.ToInt32(JM.Estimate_ManMonth.ToString());
                    JM.Estimate_ManDays_ManMonth = ManDayCount.ToString();
                }
                



                Result = objDalCompany.InsertUpdateJOb(JM, IPath, IPath1);
                JobIDs = Convert.ToInt32( Result);
               // JobIDs = Convert.ToInt32(Session["JOB_ID"]);

                if (JobIDs != null && JobIDs != 0)
                {
                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, JobIDs);
                        Result = objDalCompany.InsertFileAttachment(lstFileDtls, JobIDs);
                        Session["listJobMasterUploadedFile"] = null;
                    }
                    if (lstFileDtlsFormat != null && lstFileDtlsFormat.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtlsFormat, JobIDs);
                        Result = objDalCompany.InsertFileFormat(lstFileDtlsFormat, JobIDs);
                        Session["listFormatFile"] = null;
                    }
                }
                if (Result != "" && Result != null)
                {
                    TempData["UpdateUsers"] = Result;
                    //return RedirectToAction("JobList");
                    return RedirectToAction("JobCreation", new {@pk_Job_ID = JobIDs });
                }
                //return RedirectToAction("JobList");
            }
            else
            {               
                decimal invamount = Convert.ToDecimal(JM.Customer_PO_Amount) - Convert.ToDecimal(JM.InvoiceAmount);

                if (JM.OrderType == "ManMonth")
                {
                    ManDayCount = Convert.ToInt32(ConfigurationManager.AppSettings["MonthRate"].ToString()) * Convert.ToInt32(JM.Estimate_ManMonth.ToString());
                    JM.Estimate_ManDays_ManMonth = ManDayCount.ToString();
                }

                JM.Balance = invamount;
                

                Result = objDalCompany.InsertUpdateJOb(JM, IPath, IPath1);

                DataTable dtConsumeData = new DataTable();
                dtConsumeData = objDalCompany.GetConsumeCount(JM.PK_JOB_ID);
                JM.Consume = dtConsumeData.Rows[0]["ConsumeCall"].ToString();
                JM.Remaining = dtConsumeData.Rows[0]["RemainingCall"].ToString();
                JM.ProposedCall = dtConsumeData.Rows[0]["ProposedCall"].ToString();
                JM.POMandays = dtConsumeData.Rows[0]["POMandays"].ToString();

                JobIDs = JM.PK_JOB_ID;

                if (JobIDs != null && JobIDs != 0)
                {
                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, JobIDs);
                        Result = objDalCompany.InsertFileAttachment(lstFileDtls, JobIDs);
                        Session["listJobMasterUploadedFile"] = null;
                    }
                    if (lstFileDtlsFormat != null && lstFileDtlsFormat.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtlsFormat, JobIDs);
                        Result = objDalCompany.InsertFileFormat(lstFileDtlsFormat, JobIDs);
                        Session["listFormatFile"] = null;
                    }
                }


                #region
                
                DataTable subJobDashBoard = new DataTable();
              //  List<JobMasters> lstSubJob1 = new List<JobMasters>();
                subJobDashBoard = objDalSubjob.GetJobDetailsCountBydata(JM.PK_JOB_ID);
                try
                {
                    if (subJobDashBoard.Rows.Count > 0)
                    {

                        foreach (DataRow dr in subJobDashBoard.Rows)
                        {

                            lstSubJob.Add(
                                new JobMasters
                                {
                                    #region
                                    PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                    PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                    PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                    Project_Name = Convert.ToString(dr["Project_Name"]),
                                    Company_Name = Convert.ToString(dr["Company_Name"]),
                                    Control_Number = Convert.ToString(dr["Control_Number"]),
                                    Service_type = Convert.ToString(dr["Service_type"]),
                                    vendor_name = Convert.ToString(dr["vendor_name"]),
                                    Vendor_Po_No = Convert.ToString(dr["Vendor_Po_No"]),
                                    Date_of_Po = dr["Date_of_Po"].ToString() == string.Empty ? string.Empty : dr["Date_of_Po"].ToString(),
                                    SAP_No = Convert.ToString(dr["SAP_No"]),
                                    SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                    SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                    Status = Convert.ToString(dr["Status"]),
                                    Client_Email = Convert.ToString(dr["Client_Email"]),
                                    Vendor_Email = Convert.ToString(dr["Vendor_Email"]),
                                    Tuv_Email = Convert.ToString(dr["Tuv_Email"]),
                                    Client_Contact = Convert.ToString(dr["Client_Contact"]),
                                    Vendor_Contact = Convert.ToString(dr["Vendor_Contact"]),
                                    Sub_Vendor_Contact = Convert.ToString(dr["Sub_Vendor_Contact"]),
                                    Attachment = Convert.ToString(dr["Attachment"]),
                                    Type = Convert.ToString(dr["Type"]),
                                    V1 = Convert.ToString(dr["V1"]),
                                    V2 = Convert.ToString(dr["V2"]),
                                    P1 = Convert.ToString(dr["P1"]),
                                    P2 = Convert.ToString(dr["P2"]),
                                    #endregion

                                }
                                );
                        }
                    }
                }


                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
                ViewData["subJobList"] = lstSubJob;
                JM.lstSubJob = lstSubJob;




                //**********************************************Code Added by Manoj Sharma for Delete file and update file 13 March 2020

                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDalCompany.EditUploadedFile(JM.PK_JOB_ID);
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
                    JM.FileDetails = lstEditFileDetails;
                }




                //**********************************************Code Added by Manoj Sharma for Delete file and update file Format Report
                DataTable DTGetUploadedFileFormat = new DataTable();
                List<FileDetails> lstEditFileDetailsFormat = new List<FileDetails>();
                DTGetUploadedFileFormat = objDalCompany.EditUploadedFileFormat(JM.PK_JOB_ID);
                if (DTGetUploadedFileFormat.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTGetUploadedFileFormat.Rows)
                    {
                        lstEditFileDetailsFormat.Add(
                           new FileDetails
                           {

                               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                               FileName = Convert.ToString(dr["FileName"]),
                               Extension = Convert.ToString(dr["Extenstion"]),
                               IDS = Convert.ToString(dr["FileID"]),
                           }
                         );
                    }
                    ViewData["lstEditFileDetailsFormat"] = lstEditFileDetailsFormat;
                    JM.FileDetailsFormat = lstEditFileDetailsFormat;
                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file

                DataTable DTCallSummary = new DataTable();
                DTCallSummary = objDalCompany.GetCallSummary(JM.PK_JOB_ID);
                List<CallsModel> lstCallSummary = new List<CallsModel>();

                if (DTCallSummary.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallSummary.Rows)
                    {
                        lstCallSummary.Add(
                           new CallsModel
                           {

                               OpenCalls = Convert.ToString(dr["OpenCalls"]),
                               ClosedCalls = Convert.ToString(dr["ClosedCalls"]),
                               AssignedCalls = Convert.ToString(dr["AssignedCalls"]),
                               NotDoneCalls = Convert.ToString(dr["NotDoneCalls"]),
                               CancelledCalls = Convert.ToString(dr["CancelledCalls"]),
                               TotalCalls = Convert.ToString(dr["TotalCalls"]),
                           }
                         );
                    }
                    ViewData["lstCallSummary"] = lstCallSummary;
                    JM.lstCallSummary = lstCallSummary;
                }

                DataTable DTAddMandays = new DataTable();
                DTAddMandays = objDalCompany.GetAddMandays(JM.PK_JOB_ID);
                List<JobMasters> lstAddMandays = new List<JobMasters>();

                if (DTAddMandays.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAddMandays.Rows)
                    {
                        lstAddMandays.Add(
                           new JobMasters
                           {
                               PK_ADDPOID = Convert.ToInt32(dr["PK_ADDPOID"]),
                               Add_PoReason = Convert.ToString(dr["Reason"]),
                               Add_MandayDesc = Convert.ToString(dr["Description"]),
                               Add_Mandays = Convert.ToString(dr["ManDays"]),
                               Add_PoNumber = Convert.ToString(dr["PoNo"]),
                               Add_PoDate = Convert.ToString(dr["PoDate"]),
                               Add_PoValidity = Convert.ToString(dr["Po_Validity"]),
                               Add_POAmt = Convert.ToDecimal(dr["PO_Amount"]),
                               CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           }
                         );
                    }
                    ViewData["lstAddMandays"] = lstAddMandays;
                    JM.lstAddMandays = lstAddMandays;
                }


                // return View(ObjModelJob);

                #endregion

                if (Result != "" && Result != null)
                {
                    DataSet dsBindBranch = new DataSet();
                    List<BranchName> lstBranch = new List<BranchName>();
                    dsBindBranch = objDTC.BindBranch();

                    if (dsBindBranch.Tables[0].Rows.Count > 0)
                    {
                        lstBranch = (from n in dsBindBranch.Tables[0].AsEnumerable()
                                     select new BranchName()
                                     {
                                         Name = n.Field<string>(dsBindBranch.Tables[0].Columns["Branch_Name"].ToString()),
                                         Code = n.Field<Int32>(dsBindBranch.Tables[0].Columns["Br_Id"].ToString())

                                     }).ToList();
                    }

                    IEnumerable<SelectListItem> BranchItems;
                    BranchItems = new SelectList(lstBranch, "Code", "Name");
                    ViewBag.ProjectTypeItems = BranchItems;
                    ViewData["BranchName"] = BranchItems;
                    TempData["UpdateUsers"] = Result;
                    // return View(JM);
                    return RedirectToAction("JobCreation", new { @pk_Job_ID = JobIDs });
                }

                // return RedirectToAction("JobList");
            }
            // return RedirectToAction("JobList");
            return RedirectToAction("JobCreation", new { @pk_Job_ID = JobIDs });
        }

        [HttpGet]
        public ActionResult JobDashBoard(int? PK_JOB_ID)
        {

            if (PK_JOB_ID != 0 && PK_JOB_ID != null)
            {
                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalCompany.EditJob(PK_JOB_ID);
                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ObjModelJob.PK_JOB_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_JOB_ID"]);
                    ObjModelJob.Description = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Description"]);
                    ObjModelJob.Quotation_Of_Order = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Quotation_Of_Order"]);
                    ObjModelJob.Enquiry_Of_Order = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Enquiry_Of_Order"]);
                    ObjModelJob.Client_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Name"]);
                    ObjModelJob.Branch = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Branch"]);
                    ObjModelJob.End_User = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["End_User"]);
                    ObjModelJob.Service_type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Service_type"]);
                    ObjModelJob.Job_type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Job_type"]);
                    ObjModelJob.Customer_PoNo_PoDate = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Customer_PoNo_PoDate"]);
                    ObjModelJob.Customer_PO_Amount = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["Customer_PO_Amount"]);
                    ObjModelJob.Po_Validity = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Po_Validity"]);
                    ObjModelJob.Estimate_ManDays_ManMonth = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Estimate_ManDays_ManMonth"]);
                    ObjModelJob.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Status"]);
                    ObjModelJob.Contract_reviewList = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Contract_reviewList"]);
                    ObjModelJob.Special_Notes = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Special_Notes"]);
                    ObjModelJob.formats_Of_Report = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["formats_Of_Report"]);
                    ObjModelJob.Attachment = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Attachment"]);

                    ObjModelJob.SAP_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SAP_No"]);
                    ObjModelJob.SAPItem_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SAPItem_No"]);
                    ObjModelJob.GstDetails_BillingAddress = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["GstDetails_BillingAddress"]);
                }

                return View(ObjModelJob);
            }
            else
            {
                return View();
            }

        }

        
        [HttpPost]
        public ActionResult JobDashBoard(JobMasters JM, HttpPostedFileBase File, FormCollection fc)
        {

            if (JM.PK_JOB_ID != 0)
            {

                #region File Upload Code 


                HttpPostedFileBase Imagesection;
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                {
                    Imagesection = Request.Files["img_Banner"];
                    if (Imagesection != null && Imagesection.FileName != "")
                    {
                        JM.Attachment = CommonControl.FileUpload("~/Content/JobDocument/", Imagesection);
                    }
                    else
                    {
                        if (Imagesection.FileName != "")
                        {
                            JM.Attachment = "NoImage.gif";
                        }
                    }
                }

                HttpPostedFileBase Imagesection1;
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner1"])))
                {
                    Imagesection1 = Request.Files["img_Banner1"];
                    if (Imagesection1 != null && Imagesection1.FileName != "")
                    {
                        JM.formats_Of_Report = CommonControl.FileUpload("~/Content/JobDocument/", Imagesection1);
                    }
                    else
                    {
                        if (Imagesection1.FileName != "")
                        {
                            JM.formats_Of_Report = "NoImage.gif";
                        }
                    }
                }
                #endregion

                return RedirectToAction("JobList");
            }
            else
            {

                #region File Insert Code 



                HttpPostedFileBase Imagesection;
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                {
                    Imagesection = Request.Files["img_Banner"];
                    if (Imagesection != null && Imagesection.FileName != "")
                    {
                        JM.Attachment = CommonControl.FileUpload("~/Content/JobDocument/", Imagesection);
                    }
                    else
                    {
                        if (Imagesection.FileName != "")
                        {
                            JM.Attachment = "NoImage.gif";
                        }
                    }
                }

                HttpPostedFileBase Imagesection1;
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner1"])))
                {
                    Imagesection1 = Request.Files["img_Banner1"];
                    if (Imagesection1 != null && Imagesection1.FileName != "")
                    {
                        JM.formats_Of_Report = CommonControl.FileUpload("~/Content/JobDocument/", Imagesection1);
                    }
                    else
                    {
                        if (Imagesection1.FileName != "")
                        {
                            JM.formats_Of_Report = "NoImage.gif";
                        }
                    }
                }
                #endregion

                return RedirectToAction("JobList");
            }

        }
       

        public ActionResult DeleteJobData(int? PK_JOB_ID)
        {
            int Result = 0;
            try
            {
                Result = objDalCompany.DeleteJob(PK_JOB_ID);
                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    return RedirectToAction("JobList");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        [HttpPost]
        public ActionResult SaveFile()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    file.SaveAs(Server.MapPath($"/Content/{file.FileName}"));
                }
            }
            return Json(true);
        }

        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();

            //Adding New Code 13 March 2020
            List<FileDetails> fileDetails = new List<FileDetails>();
            List<FileDetails> fileJobDetails = new List<FileDetails>();
            List<FileDetails> fileFormatDetails = new List<FileDetails>();
            List<FileDetails> fileAddPODetails = new List<FileDetails>();

            if (Session["listJobMasterUploadedFile"] != null)
            {
                    fileJobDetails = Session["listJobMasterUploadedFile"] as List<FileDetails>;
            }

            if (Session["listFormatFile"] != null)
            {
                fileFormatDetails = Session["listFormatFile"] as List<FileDetails>;
            }

            if (Session["lstAddPOFileDtls"] != null)
            {
                fileAddPODetails = Session["lstAddPOFileDtls"] as List<FileDetails>;
            }
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
                        if (files.FileName.ToUpper().EndsWith(".MSG") || files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".PDF") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpeg") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".PNG") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))

                        {
                            string fileName = files.FileName;
                            //Adding New Code as per new requirement 12 March 2020, Manoj Sharma
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();

                            BinaryReader br = new BinaryReader(files.InputStream);
                            byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                            fileDetail.FileContent = bytes;

                            if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD")
							{
								fileJobDetails.Add(fileDetail);
							}
							else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
							{
								fileFormatDetails.Add(fileDetail);
							}
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD2")
                            {
                                fileAddPODetails.Add(fileDetail);
                            }

                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetail.Id + fileDetail.Extension);
                            var K = "~/Content/JobDocument/" + fileName;
                            IPath = K;
                           // files.SaveAs(filePath);

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
				
                if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD")				
                {
                    Session["listJobMasterUploadedFile"] = fileJobDetails;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")															 
                {
                    Session["listFormatFile"] = fileFormatDetails;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD2")
                {
                    Session["lstAddPOFileDtls"] = fileAddPODetails;
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }

        /*
        public JsonResult TemporaryFilePathDocumentAttachment1()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath1 = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            //Adding New Code 13 March 2020
            List<FileDetails> fileDetails = new List<FileDetails>();
            //---Adding end Code
            try
            {

                FormCollection fc = new FormCollection();
                string filePath = string.Empty;

                if (Session["listJobMastUploadedFile"] != null)
                {
                    fileDetails = Session["listJobMastUploadedFile"] as List<FileDetails>;
                }

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file
                    int fileSize = files.ContentLength;
                    if (files != null && files.ContentLength > 0)
                    {
                        if (files.FileName.ToUpper().EndsWith(".MSG") || files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".PDF") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpeg") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".PNG") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))
                        {
                            string fileName = files.FileName;
                            //Adding New Code as per new requirement 12 March 2020, Manoj Sharma
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();
                            fileDetails.Add(fileDetail);
                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetail.Id + fileDetail.Extension);
                            var K = "~/Content/JobDocument/" + fileName;
                            IPath1 = K;
                            files.SaveAs(filePath);

                            var ExistingUploadFile = IPath1;
                            splitedGrp = ExistingUploadFile.Split(',');
                            foreach (var single in splitedGrp)
                            {
                                Selected.Add(single);
                            }
                            Session["list1"] = Selected;
                        }
                        else
                        {
                            ViewBag.Error = "Please Select XLSX or PDF File";
                        }
                    }
                }
                Session["listJobMastUploadedFile"] = fileDetails;
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TemporaryFilePathAddPO()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath1 = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            //Adding New Code 13 March 2020
            List<FileDetails> fileDetails = new List<FileDetails>();
            //---Adding end Code
            try
            {

                FormCollection fc = new FormCollection();
                string filePath = string.Empty;

                if (Session["lstAddPOFileDtls"] != null)
                {
                    fileDetails = Session["lstAddPOFileDtls"] as List<FileDetails>;
                }

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file
                    int fileSize = files.ContentLength;
                    if (files != null && files.ContentLength > 0)
                    {
                        if (files.FileName.ToUpper().EndsWith(".MSG") || files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".PDF") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpeg") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".PNG") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))
                        {
                            string fileName = files.FileName;
                            //Adding New Code as per new requirement 12 March 2020, Manoj Sharma
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();
                            fileDetails.Add(fileDetail);
                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetail.Id + fileDetail.Extension);
                            var K = "~/Content/JobDocument/" + fileName;
                            IPath1 = K;
                            //files.SaveAs(filePath);

                            var ExistingUploadFile = IPath1;
                            splitedGrp = ExistingUploadFile.Split(',');
                            foreach (var single in splitedGrp)
                            {
                                Selected.Add(single);
                            }
                            Session["list1"] = Selected;
                        }
                        else
                        {
                            ViewBag.Error = "Please Select XLSX or PDF File";
                        }
                    }
                }
                Session["lstAddPOFileDtls"] = fileDetails;
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath1, JsonRequestBehavior.AllowGet);
        }
        */

        #region export to excel
        [HttpGet]
        public ActionResult ExportIndex()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<JobMasters> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<JobMasters> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<JobMasters> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<JobMasters> grid = new Grid<JobMasters>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.Job_Number).Titled("Job Number");
            grid.Columns.Add(model => model.Description).Titled("Description");
            grid.Columns.Add(model => model.Client_Name).Titled("Client Name");
            grid.Columns.Add(model => model.Quotation_Of_Order).Titled("Quotation");
            grid.Columns.Add(model => model.Enquiry_Of_Order).Titled("Enquiry");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.End_User).Titled("End User");
            grid.Columns.Add(model => model.Service_type).Titled("Service type");
            grid.Columns.Add(model => model.Job_type).Titled("Job Type");




            grid.Pager = new GridPager<JobMasters>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelJob.lstCompanyDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<JobMasters> GetData()
        {

            DataTable JobDashBoard = new DataTable();
            List<JobMasters> lstCompanyDashBoard = new List<JobMasters>();
            JobDashBoard = objDalCompany.GetJOBList();
            try
            {
                if (JobDashBoard.Rows.Count > 0)
                {
                    //int abc = JobDashBoard.Rows.Count;
                    // int data = 1 + abc;
                    foreach (DataRow dr in JobDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new JobMasters
                            {
                                Count = JobDashBoard.Rows.Count,
                                PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                Job_Number = Convert.ToString(dr["Job_Number"]),
                                Quotation_Of_Order = Convert.ToString(dr["Quotation_Of_Order"]),
                                Enquiry_Of_Order = Convert.ToString(dr["Enquiry_Of_Order"]),
                                Client_Name = Convert.ToString(dr["Client_Name"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                End_User = Convert.ToString(dr["End_User"]),
                                Service_type = Convert.ToString(dr["Service_type"]),
                                Job_type = Convert.ToString(dr["Job_type"]),
                                Customer_PoNo_PoDate = Convert.ToString(dr["Customer_PoNo_PoDate"]),
                                Customer_PO_Amount = Convert.ToDecimal(dr["Customer_PO_Amount"]),
                                Po_Validity = Convert.ToString(dr["Po_Validity"]),
                                Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                                Contract_reviewList = Convert.ToString(dr["Contract_reviewList"]),
                                Special_Notes = Convert.ToString(dr["Special_Notes"]),
                                formats_Of_Report = Convert.ToString(dr["formats_Of_Report"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Description = Convert.ToString(dr["Description"]),
                                //  Br_Id = Convert.ToInt32(dr["Br_Id"]),
                                //  EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                                PK_QT_ID = Convert.ToInt32(dr["PK_QT_ID"]),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                //GstDetails_BillingAddress = Convert.ToString(dr["GstDetails_BillingAddress"])

                            }
						);
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["JobList"] = lstCompanyDashBoard;
            ObjModelJob.lstCompanyDashBoard1 = lstCompanyDashBoard;


            return ObjModelJob.lstCompanyDashBoard1;
        }
        #endregion
		
				
        //Delete Uploaded Multiple file in Quotation Code added by manoj Sharma on 12 March 2020
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
              //  Guid guid = new Guid(id);
                DTGetDeleteFile = objDalCompany.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = objDalCompany.DeleteUploadedFile(id);
                    var path = Path.Combine(Server.MapPath("~/Content/JobDocument/"), id + fileDetails.Extension);
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

        public void Download(String p, String d)
        {
            /// return File(Path.Combine(Server.MapPath("~/Files/Documents/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);


            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalCompany.GetFileContent(Convert.ToInt32(d));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }

        public FileResult Download1(string d)
        {

            string FileName = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalCompany.GetFileContent(Convert.ToInt32(d));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                FileName = DTDownloadFile.Rows[0]["FileName"].ToString();
                Date = DTDownloadFile.Rows[0]["CreatedDate"].ToString();
            }

            //string myDate = "05/11/2010";
            DateTime date = Convert.ToDateTime(Date);
            int year = date.Year;
            int Month = date.Month;

            int intC = Convert.ToInt32(Month);
            string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);


            //Build the File Path.
            //string path = Server.MapPath("~/Content/JobDocument/") + d;
            //var savePath = Path.Combine(Server.MapPath("~/IVRIRNSupportDocs/"), a + item.FileName);

            string path = Server.MapPath("~/Content/" + year + "/" + CurrentMonth + "/") + FileName;
            // string path = Server.MapPath("~/Content/") + d;

            //Read the File data into Byte Array.
           bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", FileName);
        }

        public FileResult Download2(string d)
        {

            string FileName = "";
            string Date = "";


            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalCompany.GetFileContent1(Convert.ToInt32(d));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                FileName = DTDownloadFile.Rows[0]["FileName"].ToString();
                Date = DTDownloadFile.Rows[0]["CreatedDate"].ToString();
            }

            //string myDate = "05/11/2010";
            DateTime date = Convert.ToDateTime(Date);
            int year = date.Year;
            int Month = date.Month;

            int intC = Convert.ToInt32(Month);
            string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);


            //Build the File Path.
            //string path = Server.MapPath("~/Content/JobDocument/") + d;
            //var savePath = Path.Combine(Server.MapPath("~/IVRIRNSupportDocs/"), a + item.FileName);

            string path = Server.MapPath("~/Content/" + year + "/" + CurrentMonth + "/") + FileName;
            // string path = Server.MapPath("~/Content/") + d;

            //Read the File data into Byte Array.
            bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", FileName);
        }
        //Delete Uploaded Multiple file in Quotation Code added by manoj Sharma on 13 March 2020 Format Report
        [HttpPost]
        public JsonResult DeleteFileFormat(string id)
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
             //   Guid guid = new Guid(id);
                DTGetDeleteFile = objDalCompany.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = objDalCompany.DeleteUploadedFileFormat(id);
                    var path = Path.Combine(Server.MapPath("~/Content/JobDocument/"), id + fileDetails.Extension);
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
        public void DownloadFormat(String p, String d)
        {
            /// return File(Path.Combine(Server.MapPath("~/Files/Documents/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);


            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalCompany.GetFileContent1(Convert.ToInt32(d));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
                fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }

        public void DownloadSubJobAttachement(String p, String d)
        {
            /// return File(Path.Combine(Server.MapPath("~/Files/Documents/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);


            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalCompany.DownloadSubJobAttachment(Convert.ToInt32(d));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
                fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }

        [HttpGet]
        public ActionResult IVRReportsByjob(ReportModel IVR)
        {
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();
            if (IVR.SubJob_No != null)
            {
                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalSubjob.GetSubJobNoByID(IVR.SubJob_No);
                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ID = null;
                    ViewBag.ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_SubJob_Id"]);
                }
                if (IVR.PK_SubJob_Id != 0 && IVR.PK_SubJob_Id != null)
                {
                    CostSheetDashBoard = objDalSubjob.GetReportByCall_Id(Convert.ToString(IVR.PK_SubJob_Id));
                }
                else
                { CostSheetDashBoard = objDalSubjob.GetReportByCall_Id(Convert.ToString(ViewBag.ID)); }
                //CostSheetDashBoard = objDalSubjob.GetReportByCall_Id(IVR.SubJob_No);
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReportModel
                            {
                                ReportName = Convert.ToString(dr["ReportName"]),
                                Report = Convert.ToString(dr["Report"]),
                                CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                Conclusion = Convert.ToString(dr["Conclusion"]),
                                Areas_Of_Concerns = Convert.ToString(dr["Areas_Of_Concerns"]),
                                PendingActivities = Convert.ToString(dr["Pending_Activites"]),
                                NCR = Convert.ToString(dr["Pdf"]),


                            }
                            );
                    }
                }
            }
            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(IVR);
        }

        
        public ActionResult InsertAddMandays(JobMasters JM)//Checking Existing User Name
        {   
            string Result = string.Empty;
            string Result1 = string.Empty;
            string InspectorName = string.Empty;
            string joined = string.Empty;

            DataTable DTChkLeave = new DataTable();
            
            try
            {
                


                Result = objDalCompany.InsertUpdateAddMandays(JM);

                List<FileDetails> lstAddPOFileDtls = new List<FileDetails>();
                lstAddPOFileDtls = Session["lstAddPOFileDtls"] as List<FileDetails>;
                if (Result != null && Result != "0")
                {
                    if (lstAddPOFileDtls != null && lstAddPOFileDtls.Count > 0)
                    {
                        Result = objDalCompany.InsertAddPOFileAttachment(lstAddPOFileDtls, JM.PK_JOB_ID ,  Convert.ToInt32( Result));
                        Session["lstAddPOFileDtls"] = null;
                    }
                    JobRevisionMail(JM);

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(new { url = Url.Action("JobCreation", "JobMaster", new { @PK_JOB_ID = JM.PK_JOB_ID }) });
           // return RedirectToAction("JobCreation", new {@PK_JOB_ID = JM.PK_JOB_ID });
        }

        [HttpPost]
        public ActionResult UpdateCustomer(JobMasters AddPO)
        {
            string Result = string.Empty;
            string InspectorName = string.Empty;
            string joined = string.Empty;

            
            try
            {
                Result = objDalCompany.InsertUpdateAddMandays(AddPO);
            }
                   

            catch(Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return Json(new { Result = "ERROR" });
            }

            return Json(new { Result = "SUCCESS" });
        }

        public void JobRevisionMail(JobMasters CM)
        {

            string displayName = string.Empty;
            string ClientEmail = string.Empty;
            string bodyTxt = string.Empty;


            try
            {

                DataTable Details = new DataTable();
                Details = objDalCompany.GetRevisionDetails(CM.PK_JOB_ID);

                MailMessage msg = new MailMessage();


                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();


                ClientEmail = "rohini@tuv-nord.com";


                bodyTxt = @"<html>
                    <head>
                        <title></title>
                    </head>
                    <body>
                        <div>
                            <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir/Madam,</span></span></div>
                        <div>
                            &nbsp;</div>
                        <div>
                            <span style='font-size:10px;'><span style='font-family:verdana,geneva,sans-serif;'>Please be informed that <b>" + Details.Rows[0]["CreatedBy"].ToString() + "</b> has revised Job Number <b> " + CM.Job_Number.ToString() + " </b> . Below are the details for the same,</br></br></span></span></div>";

                bodyTxt = bodyTxt + "<div><span style='font-size:10px;'><span style='font-family:verdana,geneva,sans-serif;'></br></br>";
                bodyTxt = bodyTxt + "<table border='1' bordercolor='black'><tr>";

                bodyTxt = bodyTxt + "<td style='width:10%;'>Job No</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>PO NO</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>PO Amount</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%;'>PO Validity</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>PO Date</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Man days</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Reason</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Description</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Created Date</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Created by</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Modified Date</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Modified By</td> ";
                bodyTxt = bodyTxt + " </tr>";

                bodyTxt = bodyTxt + "<tr>";
                bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["Job_Number"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["PoNo"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["PO_Amount"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Po_Validity"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["PO_Date"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Mandays"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Reason"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Description"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["createdDate"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["CreatedBy"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["ModifiedDate"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["ModifiedBy"].ToString() + "</td> ";
                bodyTxt = bodyTxt + " </tr>";

                bodyTxt = bodyTxt + "</Table></span></span></div></br>";

                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Best regards,</br>";
                bodyTxt = bodyTxt + " TUV India Pvt Ltd. </br></br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Note :This is auto generated mail. Please do not reply.</span></span></div></br>";
                bodyTxt = bodyTxt + "</body></html> ";

                msg.From = new MailAddress(MailFrom, "Job - Revision for Job No : " + CM.Job_Number.ToString());


                string To = ClientEmail.ToString();

                char[] delimiters = new[] { ',', ';', ' ' };

                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }

                msg.Subject = "Revision Details for Job : " + CM.Job_Number.ToString();
                msg.Body = bodyTxt;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
                client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                client.EnableSsl = true;
                client.Send(msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
        }

        //    //Added By Satish Pawar on 15 May 2023
        //    [HttpGet]
        //    //public JsonResult Save_EmailIdOfFeedback(string JobNo,string Email_IDs,string Company_Name)
        //    public JsonResult Save_EmailIdOfFeedback(string myArray)
        //    {
        //        DataTable dt = JsonConvert.DeserializeObject<DataTable>(myArray);

        //        string Result = string.Empty;
        //        string EnceryptedKey = dt.Rows[0]["JobNo"].ToString().Trim();// Encrypt(dt.Rows[0]["JobNo"].ToString().Trim());
        //        string Feedback_Link = "https://clientportal.tuvindia.co.in/CustomerFeedback/Feedback/CustomerFeedback?tkn_key=" + EnceryptedKey;
        //        Result = objDalCompany.SaveFeedbackEmailHistory(dt.Rows[0]["JobNo"].ToString(), dt.Rows[0]["Email_IDs"].ToString(), Feedback_Link, dt.Rows[0]["Company_Name"].ToString());

        //        if (Result == "1")
        //        {
        //            Result = "Success";
        //            //SendMail(EnceryptedKey, "satyam.dube@tuvindia.co.in; satish.pawar2912@gmail.com; pshrikant@tuv-nord.com; " + dt.Rows[0]["Email_IDs"].ToString());
        //            SendMail(EnceryptedKey, "satyam.dube@tuvindia.co.in; satish.pawar2912@gmail.com");
        //        }
        //        else
        //        {
        //            Result = "Failed";
        //        }
        //        return Json(Result, JsonRequestBehavior.AllowGet);
        //    }

        //    public bool SendMail(string EnceryptedKey,string EmailID)//Live Sending Mail Utiliy
        //    {
        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //        StringBuilder MailBody = new StringBuilder();
        //        try
        //        {
        //            string Feedback_Link = "http://10.10.30.51/Customer/Feedback/BasicDetail_Stage?tkn_key=" + EnceryptedKey;
        //            //MailBody.Append("<Center>");
        //            //MailBody.Append("<table>");
        //            //MailBody.Append("<tr><td colspan='3' valign='top' bgcolor=' #FFFFFF' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 12px; color: #192F41; padding: 3px; font-weight: bold; text-transform: uppercase;'><strong>Feeback Link</strong></td></tr>");

        //            //MailBody.Append("<tr><td colspan='3' valign='top' bgcolor=' #FFFFFF' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 12px; color: #192F41; padding: 3px; font-weight: bold;'>"
        //            //    + "<a href='http://10.10.30.51/Customer/Feedback/BasicDetail_Stage?tkn_key=" + EnceryptedKey + "'><strong> Click Here </strong></a></td></tr>");


        //            //MailBody.Append("</table>");
        //            //MailBody.Append("</Center>");

        //            string _MailBody = "<table> <tr><td><p>Dear valued customer,<br/><br/>"
        //               + "We greatly value your opinion and strive to provide the best possible experience.Your feedback plays a crucial role in helping us improve our services.<br/><br/>"
        //               + "To gather your valuable feedback, we kindly request you to take a moment and fill out our customer feedback form. Your responses will help us better understand your needs and expectations, allowing us to enhance our offerings to serve you better.<br/><br/>"
        //               + "Please click on the following link to access the customer feedback form: "
        //               + "<a href='https://clientportal.tuvindia.co.in/CustomerFeedback/Feedback/BasicDetail_Stage?tkn_key=" + EnceryptedKey + "' ><strong> Click Here </strong></a><br/><br/>"
        //               + "We appreciate your time and effort in providing us with your feedback.Thank you for choosing our services, and we look forward to hearing from you.<br/><br/>"
        //               + "Best regards,<br/>"
        //               + "Team QHSE<br/>"
        //               + "TUV India Private Limited<br/></p></td></tr></table>";
        //            MailBody.Append(_MailBody);


        //            MailMessage mail = new MailMessage();
        //            SmtpClient client = new SmtpClient();

        //            //mail.To.Add(EmailID);

        //            string[] Emails = EmailID.Split(';');

        //            foreach (string _EmailID in Emails)
        //            {
        //                mail.To.Add(_EmailID);
        //            }
        //            mail.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString());
        //            mail.Subject = "Feedback";
        //            mail.IsBodyHtml = true;
        //            mail.Body = MailBody.ToString();


        //            client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
        //            client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
        //            client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
        //            client.EnableSsl = true;
        //            client.Send(mail);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //            throw ex;
        //        }
        //    }


        //    public static string Encrypt(string plainText)
        //    {
        //        byte[] key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        //        byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        //        Aes aes = Aes.Create();
        //        aes.Key = key;
        //        aes.IV = iv;

        //        byte[] encrypted;

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
        //                cs.Write(plaintextBytes, 0, plaintextBytes.Length);
        //                cs.Close();
        //            }
        //            encrypted = ms.ToArray();
        //        }

        //        return Convert.ToBase64String(encrypted);
        //    }

        //    public static string Decrypt(string cipherText)
        //    {
        //        byte[] key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        //        byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        //        Aes aes = Aes.Create();
        //        aes.Key = key;
        //        aes.IV = iv;

        //        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        //        string decrypted;

        //        using (MemoryStream ms = new MemoryStream(cipherBytes))
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
        //            {
        //                using (StreamReader sr = new StreamReader(cs))
        //                {
        //                    decrypted = sr.ReadToEnd();
        //                }
        //            }
        //        }

        //        return decrypted;
        //    }
    }
}