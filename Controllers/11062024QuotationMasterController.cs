using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using TuvVision.DataAccessLayer;
using System.Text;
using System.IO;
using System.Web.Mvc;
using OfficeOpenXml;
using NonFactors.Mvc.Grid;
using System.Net;
using SelectPdf;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Globalization;

namespace TuvVision.Controllers
{
    public class QuotationMasterController : Controller
    {
        CommonControl objCommonControl = new CommonControl();
        DALQuotationMaster objDALQuotationMast = new DALQuotationMaster();
        QuotationMaster ObjModelQuotationMast = new QuotationMaster();
        DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
        DataTable DSGetImage = new DataTable();
        List<QuotationMaster> lstImage = new List<QuotationMaster>();

        DataSet DsCompanyAddr = new DataSet();
        List<NameCode> lstCompanyAddr = new List<NameCode>();
        IEnumerable<SelectListItem> ComAddrItems;
        // GET: QuotationMaster
        public ActionResult QuotationMasterDashBoard(string Type)
        {
            Session["UserLoginID"] = User.Identity.IsAuthenticated;
            string UserRole = Convert.ToString(Session["role"]);
            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
			ObjModelQuotationMast.DashboardType = Type;									   
            lstQuotationMast = objDALQuotationMast.QuotaionMastertDashBoard(Type);
            ViewData["QuotationMaster"] = lstQuotationMast;
            ObjModelQuotationMast.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return View(ObjModelQuotationMast);
        }
        [HttpGet]
        public ActionResult Quotation(int? PK_QM_ID, int? PK_EQID, string QuotationNumber, string Revise)
        {
            //Bind quotation Lost reason
            var Data1 = objDALQuotationMast.BindQuotationLostReason();
            ViewBag.QLostReason = new SelectList(Data1, "LostPK_Id", "DLostReason");

           

            /*
            #region Bind Quotation pdf Image
            try
            {

                DSGetImage = objDALQuotationMast.GetImage();
                if (DSGetImage.Rows.Count > 0)
                {
                    foreach (DataRow dr in DSGetImage.Rows)
                    {
                        lstImage.Add(
                            new QuotationMaster
                            {
                                IId = Convert.ToInt32(dr["Id"]),
                                IName = Convert.ToString(dr["Name"]),
                                
                               // IData = (byte[])dr["Data"],

                                
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

           ViewBag.CostSheet = lstImage;
            #endregion
            */

            string QtnNo = string.Empty;
            DataTable dtQt = new DataTable();
            dtQt = objDALQuotationMast.GetQuotationNo(PK_QM_ID);
            if (dtQt.Rows.Count > 0)
            {
                QuotationNumber = dtQt.Rows[0]["QuotationNumber"].ToString();
            }

            if (QuotationNumber != null)
            {                
                //int VarLength = Regex.Matches(QuotationNumber, "/").Count;
                //if (VarLength > 4)
                //{
                //    QtnNo = QuotationNumber.Substring(0, QuotationNumber.LastIndexOf("/") + 0);
                //}
                //else
                //{
                    QtnNo = QuotationNumber;
                //}

            }
            //if (QuotationNumber != null)
            //{
            //    ObjModelQuotationMast.Validity = "Our offer " + QuotationNumber.ToString() + " dated: " + DateTime.Now.ToString("dd/MM/yyyy") + " shall be valid for 30 days from the date of offer.Upon acceptance our prices will be held firm till March 31, 2021";
            //}
            //else
            //{
            //    ObjModelQuotationMast.Validity = "Our offer                dated:           shall be valid for 30 days from the date of offer.Upon acceptance our prices will be held firm till March 31, 2021";
            //}
            string stryear = string.Empty;
            stryear = GetCurrentFinancialYear(); 



            ObjModelQuotationMast.Validity = "Our offer                dated:           shall be valid for 30 days from the date of offer. Upon acceptance our prices will be held firm till March 31," + stryear.ToString().Substring(stryear.Length-4,4)+"." ;
            ObjModelQuotationMast.ThankYouLetter = "Dear Sir / Madam ,";
            ObjModelQuotationMast.CommunicationProtocol = "TUV India shall handle all activities related to Co-Ordination under this contract. Inspection Calls/Any communications related to shall be sent to the following mail ids:";
            // ObjModelQuotationMast.AddEnclosures = "F/MR/27 Rev. 06 Revision date: 07/06/2022";
            ObjModelQuotationMast.AddEnclosures = "TUVI/GTC/01 Rev.0 Date:01/05/2023";
            
            ObjModelQuotationMast.GeneralTerms = "In addition, TUV NORD general terms and conditions including limitation of liability shall be applicable. Upon acceptance of an order a contract in the specified format of TUV NORD would be agreed with you.";
            ObjModelQuotationMast.ComplimentaryClose = "We trust you will find the above offer competitive and realistic to provide required level of service and look forward to be associated with you in this prestigious project. In case you need any further clarifications / comments, please contact us, we will be pleased to furnish the same promptly.";


            //ObjModelQuotationMast.GeneralTerms = "Other terms and conditions as per attached sheet & General Terms & conditions of TUV NORD including limitation of liability ";

            DataTable WonQuotationCount = new DataTable();
            WonQuotationCount = objDALQuotationMast.QuotationCount(Convert.ToString(QtnNo));
            if (WonQuotationCount.Rows.Count > 0)
            {
                ViewBag.WonCount = Convert.ToString("pass");
            }
            else
            {
                ViewBag.WonCount = Convert.ToString("failed");
            }

            if (PK_QM_ID > 0 && PK_QM_ID != null && Revise !=string.Empty && Revise != null)
            {
				string[] splitedCity;
                string[] splitedCountry;

                string[] InspectionLocation;
                #region Domestic Order Type 
                DataTable dtDOrderType = new DataTable();
                List<QuotationMaster> lstDOrderType = new List<QuotationMaster>();

                dtDOrderType = objDALQuotationMast.DOrderType(PK_QM_ID);
                if (dtDOrderType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDOrderType.Rows)
                    {
                        lstDOrderType.Add(
                           new QuotationMaster
                           {

                               OrderType = Convert.ToString(dr["OrderType"]),
                               OrderRate = Convert.ToString(dr["OrderRate"]),
                               Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                               Distance = Convert.ToString(dr["Distance"]),
                               EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               DExchangeRate = Convert.ToString(dr["DExchangeRate"]),
                               DTotalAmount = Convert.ToString(dr["DTotalAmount"]),
                               DRemarks = Convert.ToString(dr["Remark"]),
                           }
                         );
                    }
                    ViewBag.lstDOrderType = lstDOrderType;

                }
                #endregion


                #region International Order Type 
                DataTable dtIOrderType = new DataTable();
                List<QuotationMaster> lstIOrderType = new List<QuotationMaster>();

                dtIOrderType = objDALQuotationMast.IOrderType(PK_QM_ID);
                if (dtIOrderType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIOrderType.Rows)
                    {
                        lstIOrderType.Add(
                           new QuotationMaster
                           {

                               IOrderType = Convert.ToString(dr["IOrderType"]),
                               IOrderRate = Convert.ToString(dr["IOrderRate"]),
                               IEstimate_ManDays_ManMonth = Convert.ToString(dr["IEstimate_ManDays_ManMonth"]),
                               IDistance = Convert.ToString(dr["IDistance"]),
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               Icurrency = Convert.ToString(dr["Icurrency"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               ITotalAmount = Convert.ToString(dr["ITotalAmount"]),
                               IRemarks = Convert.ToString(dr["Remark"]),
                           }
                         );
                    }
                    ViewBag.lstIOrderType = lstIOrderType;
                }

                #endregion

                List<string> Selected = new List<string>();
                List<string> SelectedInspectionLocation = new List<string>();
                List<string> SelectedCity = new List<string>();
                List<string> SelectedCountry = new List<string>();

                ViewBag.check = "productcheck";	
				
                DataTable DTEditQuotationMast = new DataTable();
                DTEditQuotationMast = objDALQuotationMast.RevisedQuotationMast(PK_QM_ID);
                if (DTEditQuotationMast.Rows.Count > 0)
                {
                    ObjModelQuotationMast.QuotationImage = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationImage"]);
                    ObjModelQuotationMast.PK_QTID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["PK_QTID"]);
                    ObjModelQuotationMast.QuotationNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationNumber"]);
                    ObjModelQuotationMast.FK_CMP_ID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["FK_CMP_ID"]);
                    ObjModelQuotationMast.CompanyAddress = Convert.ToString(DTEditQuotationMast.Rows[0]["Address"]);
                    //ObjModelQuotationMast.Revise = Revise;
                    ObjModelQuotationMast.Revise = Revise;
                    //TempData["QuotNumber"] = QuotationNumber;
                    //TempData.Keep();
                    ObjModelQuotationMast.EQ_ID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["EQ_ID"]);
                    ObjModelQuotationMast.QuotationCompanyName = Convert.ToString(DTEditQuotationMast.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.QuotationBranch = Convert.ToInt32(DTEditQuotationMast.Rows[0]["BranchName"]);
                    ObjModelQuotationMast.ServiceType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ServiceType"]);
                    ObjModelQuotationMast.ProjectType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ProjectType"]);
                    ObjModelQuotationMast.PortfolioType = Convert.ToString(DTEditQuotationMast.Rows[0]["PortfolioType"]);
                    ObjModelQuotationMast.SubServiceType = Convert.ToString(DTEditQuotationMast.Rows[0]["SubServiceType"]);

                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTEditQuotationMast.Rows[0]["Enquiry"]);
                    ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTEditQuotationMast.Rows[0]["Quotation_Description"]);
                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTEditQuotationMast.Rows[0]["EndCustomer"]);
                    ObjModelQuotationMast.Reference = Convert.ToString(DTEditQuotationMast.Rows[0]["Reference"]);
                    ObjModelQuotationMast.ExpiryDate = Convert.ToString(DTEditQuotationMast.Rows[0]["ExpiryDate"]);
                    ObjModelQuotationMast.Status = Convert.ToInt32(DTEditQuotationMast.Rows[0]["Status"]);
                    ObjModelQuotationMast.GST = Convert.ToString(DTEditQuotationMast.Rows[0]["GST"]);
                    ObjModelQuotationMast.Attachment = Convert.ToString(DTEditQuotationMast.Rows[0]["Attachment"]);
                    ObjModelQuotationMast.Remark = Convert.ToString(DTEditQuotationMast.Rows[0]["Remark"]);
                    ObjModelQuotationMast.HeaderDetails = Convert.ToString(DTEditQuotationMast.Rows[0]["HeaderDetails"]);
                    ObjModelQuotationMast.Subject = Convert.ToString(DTEditQuotationMast.Rows[0]["Subject"]);
                    ObjModelQuotationMast.Deliverable = Convert.ToString(DTEditQuotationMast.Rows[0]["Deliverable"]);
                    ObjModelQuotationMast.Commercials = Convert.ToString(DTEditQuotationMast.Rows[0]["Commercials"]);
                    ObjModelQuotationMast.FeesStructure = Convert.ToString(DTEditQuotationMast.Rows[0]["FeesStructure"]);
                    ObjModelQuotationMast.PaymentTerms = Convert.ToString(DTEditQuotationMast.Rows[0]["PaymentTerms"]);
                    ObjModelQuotationMast.KeyNotes = Convert.ToString(DTEditQuotationMast.Rows[0]["KeyNotes"]);
                    ObjModelQuotationMast.AddEnclosures = Convert.ToString(DTEditQuotationMast.Rows[0]["AddEnclosures"]);
                    ObjModelQuotationMast.EnquiryNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["EnquiryNumber"]);
                    ObjModelQuotationMast.To = Convert.ToString(DTEditQuotationMast.Rows[0]["T_O"]);
                    ObjModelQuotationMast.CC = Convert.ToString(DTEditQuotationMast.Rows[0]["CC"]);
                    ObjModelQuotationMast.CommunicationProtocol = Convert.ToString(DTEditQuotationMast.Rows[0]["CommunicationProtocol"]);
                    ObjModelQuotationMast.Coordinators = Convert.ToString(DTEditQuotationMast.Rows[0]["Coordinators"]);
                    ObjModelQuotationMast.EscalationMatrix = Convert.ToString(DTEditQuotationMast.Rows[0]["EscalationMatrix"]);
                    ObjModelQuotationMast.ThankYouLetter = Convert.ToString(DTEditQuotationMast.Rows[0]["ThankYouLetter"]);
                    ObjModelQuotationMast.ScopeOfWork = Convert.ToString(DTEditQuotationMast.Rows[0]["ScopeOfWork"]);
                    ObjModelQuotationMast.QuotationPDF = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationPDF"]);
                    ObjModelQuotationMast.CostSheetApproveStatus = Convert.ToString(DTEditQuotationMast.Rows[0]["DCostSheetApproveStatus"]);
                    ObjModelQuotationMast.ICostSheetApproveStatus = Convert.ToString(DTEditQuotationMast.Rows[0]["ICostSheetApproveStatus"]);

                    ObjModelQuotationMast.ThirdPartyInspectionService = Convert.ToString(DTEditQuotationMast.Rows[0]["ThirdPartyInspectionService"]);
                    ObjModelQuotationMast.EstimatedAmount = Convert.ToString(DTEditQuotationMast.Rows[0]["EstimatedAmount"]);

                   
                    ObjModelQuotationMast.IGST = Convert.ToString(DTEditQuotationMast.Rows[0]["IGST"]);
                    ObjModelQuotationMast.IData = (byte[])(DTEditQuotationMast.Rows[0]["IData"]);
                    var EInspectionLocation = Convert.ToString(DTEditQuotationMast.Rows[0]["InspectionLocation"]);
                    InspectionLocation = EInspectionLocation.Split(',');
                    foreach (var single in InspectionLocation)
                    {
                        SelectedInspectionLocation.Add(single);
                    }

                    ViewBag.EditproductName = SelectedInspectionLocation;

                    //ObjModelQuotationMast.City = Convert.ToString(DTEditQuotationMast.Rows[0]["City"]);
                    var ECityName = Convert.ToString(DTEditQuotationMast.Rows[0]["City"]);
                    splitedCity = ECityName.Split(',');
                    foreach (var single in splitedCity)
                    {
                        SelectedCity.Add(single);
                    }

                    ViewBag.EditCityName = SelectedCity;



                    // ObjModelQuotationMast.Country = Convert.ToString(DTEditQuotationMast.Rows[0]["Country"]);
                    var ECountryName = Convert.ToString(DTEditQuotationMast.Rows[0]["Country"]);
                    if (ECountryName != string.Empty)
                    {
                        splitedCountry = ECountryName.Split(',');
                        foreach (var single in splitedCountry)
                        {
                            SelectedCountry.Add(single);
                        }
                    }
                    ViewBag.EditCountryName = SelectedCountry;

                    ObjModelQuotationMast.DEstimatedAmount = Convert.ToString(DTEditQuotationMast.Rows[0]["DEstimatedAmount"]);
                    ObjModelQuotationMast.Dcurrency = Convert.ToString(DTEditQuotationMast.Rows[0]["Dcurrency"]);
                    ObjModelQuotationMast.DExchangeRate = Convert.ToString(DTEditQuotationMast.Rows[0]["DExchangeRate"]);
                    ObjModelQuotationMast.DTotalAmount = Convert.ToString(DTEditQuotationMast.Rows[0]["DTotalAmount"]);
                    ObjModelQuotationMast.IEstimatedAmount = Convert.ToString(DTEditQuotationMast.Rows[0]["IEstimatedAmount"]);
                    ObjModelQuotationMast.Icurrency = Convert.ToString(DTEditQuotationMast.Rows[0]["Icurrency"]);
                    ObjModelQuotationMast.IExchangeRate = Convert.ToString(DTEditQuotationMast.Rows[0]["IExchangeRate"]);
                    ObjModelQuotationMast.ITotalAmount = Convert.ToString(DTEditQuotationMast.Rows[0]["ITotalAmount"]);
                   
                    ObjModelQuotationMast.chkArc = Convert.ToBoolean(DTEditQuotationMast.Rows[0]["chkArc"]);
                    ObjModelQuotationMast.OrderRate = Convert.ToString(DTEditQuotationMast.Rows[0]["OrderRate"]);
                    ObjModelQuotationMast.Estimate_ManMonth = Convert.ToString(DTEditQuotationMast.Rows[0]["Estimate_ManMonth"]);
                    ObjModelQuotationMast.Estimate_ManDays_ManMonth = Convert.ToString(DTEditQuotationMast.Rows[0]["Estimate_ManDays_ManMonth"]);
                    ObjModelQuotationMast.OrderType = Convert.ToString(DTEditQuotationMast.Rows[0]["OrderType"]);
                    ObjModelQuotationMast.Exclusion = Convert.ToString(DTEditQuotationMast.Rows[0]["Exclusion"]);

                    ObjModelQuotationMast.DomStatus = Convert.ToInt32(DTEditQuotationMast.Rows[0]["DOM_Status"]);
                    ObjModelQuotationMast.IntStatus = Convert.ToInt32(DTEditQuotationMast.Rows[0]["Int_Status"]);
                    ObjModelQuotationMast.Statusname = Convert.ToString(DTEditQuotationMast.Rows[0]["StatusName"]);
                    ObjModelQuotationMast.EnquiryAdditionRef = Convert.ToString(DTEditQuotationMast.Rows[0]["EnquiryAdditionRef"]);
                    ObjModelQuotationMast.QuotationPDF = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationPdf"]);

                    if (Convert.ToString(DTEditQuotationMast.Rows[0]["ExclusionCheckBox"]) == "1")
                    {
                        ObjModelQuotationMast.ExclusionCheckBox = true;
                    }
                    else
                    {
                        ObjModelQuotationMast.ExclusionCheckBox = false;
                    }

                    ObjModelQuotationMast.GeneralTerms = Convert.ToString(DTEditQuotationMast.Rows[0]["GeneralTerms"]);

                    if (Convert.ToString(DTEditQuotationMast.Rows[0]["GeneralTermsCheckbox"]) == "1")
                    {
                        ObjModelQuotationMast.GeneralTermsCheckbox = true;
                    }
                    else
                    {
                        ObjModelQuotationMast.GeneralTermsCheckbox = false;
                    }

                    ObjModelQuotationMast.Distance = Convert.ToString(DTEditQuotationMast.Rows[0]["Distance"]);
                    if (Convert.ToInt32( ObjModelQuotationMast.EstimatedAmount) >= 1000000)
                    {
                        if (ObjModelQuotationMast.InspectionLocation == "2")
                        {
                            DataTable DTCLEditQuotationMast = new DataTable();
                            DTCLEditQuotationMast = objDALQuotationMast.GetCLApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.CostSheetCLStatus = DTCLEditQuotationMast.Rows[0]["CLStatus"].ToString();
                        }
                        else if (ObjModelQuotationMast.InspectionLocation == "1")
                        {
                            DataTable DTCLEditQuotationMast = new DataTable();
                            DTCLEditQuotationMast = objDALQuotationMast.GetICLApprovalStatus(PK_QM_ID);

                            ObjModelQuotationMast.ICostSheetCLStatus = DTCLEditQuotationMast.Rows[0]["CLStatus"].ToString();
                        }
                        else if (ObjModelQuotationMast.InspectionLocation == "1,2")
                        {
                            /// Domestic
                            DataTable DTCLEditQuotationMast = new DataTable();
                            DTCLEditQuotationMast = objDALQuotationMast.GetCLApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.CostSheetCLStatus = DTCLEditQuotationMast.Rows[0]["CLStatus"].ToString();

                            /// International
                            DataTable DTICLEditQuotationMast = new DataTable();
                            DTICLEditQuotationMast = objDALQuotationMast.GetICLApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.ICostSheetCLStatus = DTICLEditQuotationMast.Rows[0]["CLStatus"].ToString();

                            ObjModelQuotationMast.GeneralTerms = Convert.ToString(DTEditQuotationMast.Rows[0]["GeneralTerms"]);

                            if (Convert.ToString(DTEditQuotationMast.Rows[0]["GeneralTermsCheckbox"]) == "1")
                            {
                                ObjModelQuotationMast.GeneralTermsCheckbox = true;
                            }
                            else
                            {
                                ObjModelQuotationMast.GeneralTermsCheckbox = false;
                            }

                        }	 
                    }
                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDALQuotationMast.EditUploadedFile(PK_QM_ID);
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
                    ObjModelQuotationMast.FileDetails = lstEditFileDetails;
                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file
                DataSet DSGetEditQuotationAllddllst = new DataSet();
                List<NameCode> lstEditBranch = new List<NameCode>();
                List<NameCode> lstEditServiceType = new List<NameCode>();
                List<NameCode> lstEditProjectType = new List<NameCode>();
                List<NameCode> lstEditStatusType = new List<NameCode>();
                
                List<NameCode> lstEditProfileType = new List<NameCode>();
				List<NameCode> lstInspectionLocation = new List<NameCode>();
                List<NameCode> lstCityName = new List<NameCode>();
                List<NameCode> lstCountryName = new List<NameCode>();
                List<NameCode> lstDCurrency = new List<NameCode>();
                List<NameCode> lstICurrency = new List<NameCode>();
                
                DSGetEditQuotationAllddllst = objDALQuotationMast.GetEditAllddlLst();
                if (DSGetEditQuotationAllddllst.Tables[0].Rows.Count > 0)
                {
                    lstEditProjectType = (from n in DSGetEditQuotationAllddllst.Tables[0].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[0].Columns["ProjectName"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[0].Columns["PK_ID"].ToString())

                                          }).ToList();
                }

                IEnumerable<SelectListItem> ProjectTypeItems;
                ProjectTypeItems = new SelectList(lstEditProjectType, "Code", "Name");
                ViewBag.ProjectTypeItems = ProjectTypeItems;
                ViewData["ProjectTypeItems"] = ProjectTypeItems;

                if (DSGetEditQuotationAllddllst.Tables[1].Rows.Count > 0)
                {
                    lstEditBranch = (from n in DSGetEditQuotationAllddllst.Tables[1].AsEnumerable()
                                     select new NameCode()
                                     {
                                         Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[1].Columns["BranchName"].ToString()),
                                         Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[1].Columns["PK_ID"].ToString())

                                     }).ToList();
                }
                IEnumerable<SelectListItem> BranchNameTypeItems;
                BranchNameTypeItems = new SelectList(lstEditBranch, "Code", "Name");
                ViewBag.BranchNameTypeItems = BranchNameTypeItems;
                ViewData["BranchNameTypeItems"] = BranchNameTypeItems;

                if (DSGetEditQuotationAllddllst.Tables[2].Rows.Count > 0)
                {
                    lstEditServiceType = (from n in DSGetEditQuotationAllddllst.Tables[2].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[2].Columns["Name"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[2].Columns["PK_ID"].ToString())

                                          }).ToList();
                }
                IEnumerable<SelectListItem> ServiceTypeItems;
                ServiceTypeItems = new SelectList(lstEditServiceType, "Code", "Name");
                ViewBag.ServiceTypeItems = ServiceTypeItems;
                ViewData["ServiceTypeItems"] = ServiceTypeItems;

                if (DSGetEditQuotationAllddllst.Tables[3].Rows.Count > 0)
                {
                    lstEditStatusType = (from n in DSGetEditQuotationAllddllst.Tables[3].AsEnumerable()
                                         select new NameCode()
                                         {
                                             Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[3].Columns["QTStatus"].ToString()),
                                             Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[3].Columns["PK_ID"].ToString())

                                         }).ToList();
                }
                IEnumerable<SelectListItem> StatusTypeItems;
                StatusTypeItems = new SelectList(lstEditStatusType, "Code", "Name");
                ViewBag.StatusTypeItems = StatusTypeItems;
                ViewData["StatusTypeItems"] = StatusTypeItems;

                if (DSGetEditQuotationAllddllst.Tables[4].Rows.Count > 0)
                {
                    lstEditProfileType = (from n in DSGetEditQuotationAllddllst.Tables[4].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[4].Columns["Name"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[4].Columns["PK_ID"].ToString())

                                          }).ToList();
                }

                IEnumerable<SelectListItem> PortfolioTypeItems;
                PortfolioTypeItems = new SelectList(lstEditProfileType, "Code", "Name");
                ViewBag.ProfileTypeItems = PortfolioTypeItems;
                ViewData["PortfolioTypeItems"] = PortfolioTypeItems;

				if (DSGetEditQuotationAllddllst.Tables[7].Rows.Count > 0)
                {
                    lstInspectionLocation = (from n in DSGetEditQuotationAllddllst.Tables[7].AsEnumerable()
                                             select new NameCode()
                                             {
                                                 Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[7].Columns["Name"].ToString()),
                                                 Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[7].Columns["PK_ID"].ToString())

                                             }).ToList();
                }
                IEnumerable<SelectListItem> InspectionLocationItems;
                InspectionLocationItems = new SelectList(lstInspectionLocation, "Code", "Name");
                ViewBag.InspectionLocation = InspectionLocationItems;
                ViewBag.InspectionLocationItems = InspectionLocationItems;


                if (DSGetEditQuotationAllddllst.Tables[5].Rows.Count > 0)//City
                {
                    lstCityName = (from n in DSGetEditQuotationAllddllst.Tables[5].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[5].Columns["CityName"].ToString()),
                                       Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[5].Columns["PK_ID"].ToString())

                                   }).ToList();
                }
                //ViewBag.CityName = lstCityName;
                IEnumerable<SelectListItem> CityNameItems;//14
                CityNameItems = new SelectList(lstCityName, "Code", "Name");
                ViewBag.CityNameItems = CityNameItems;
                ViewData["CityNameItems"] = CityNameItems;



                if (DSGetEditQuotationAllddllst.Tables[6].Rows.Count > 0)//Country
                {
                    lstCountryName = (from n in DSGetEditQuotationAllddllst.Tables[6].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[6].Columns["CountryName"].ToString()),
                                          Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[6].Columns["PK_ID"].ToString())

                                      }).ToList();
                }
                //ViewBag.CountryName = lstCountryName;
                IEnumerable<SelectListItem> CountryNameItems;
                CountryNameItems = new SelectList(lstCountryName, "Code", "Name");
                ViewBag.CountryNameItems = CountryNameItems;
                ViewData["CountryNameItems"] = CountryNameItems;


                if (DSGetEditQuotationAllddllst.Tables[8].Rows.Count > 0)//All Currency 
                {
                    lstDCurrency = (from n in DSGetEditQuotationAllddllst.Tables[8].AsEnumerable()
                                    select new NameCode()
                                    {
                                        Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[8].Columns["CurrencyCode"].ToString()),
                                        Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[8].Columns["CurrencyID"].ToString())

                                    }).ToList();
                }
                ViewBag.Currency = lstDCurrency;
                IEnumerable<SelectListItem> CurrencyItems;
                CurrencyItems = new SelectList(lstDCurrency, "Code", "Name");
                ViewBag.CurrencyItems = CurrencyItems;
                               
                ViewBag.DTestCurrency = lstDCurrency;
                ViewBag.ITestCurrency = lstDCurrency;


                #region Bind Company Addr
                DataSet DsCompanyAddr = new DataSet();
                List<NameCode> lstCompanyAddr = new List<NameCode>();

                DsCompanyAddr = objDALQuotationMast.GetCompanyAddr(Convert.ToInt32(ObjModelQuotationMast.EQ_ID));
                //DsCompanyAddr = objDALQuotationMast.GetCompanyAddr(Convert.ToInt32(ObjModelQuotationMast.CompanyAddress));
                if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                {
                    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                          Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                      }).ToList();
                }

                IEnumerable<SelectListItem> ComAddrItems;
                ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

                //ViewBag.ComAddr = ComAddrItems;
                ViewData["ComAddr1"] = ComAddrItems;
                #endregion


                return View(ObjModelQuotationMast);



            }
            if (QuotationNumber != "" && QuotationNumber != null)
            {
				string[] splitedCity;
                string[] splitedCountry;
                
                string[] InspectionLocation;
                List<string> Selected = new List<string>();
                List<string> SelectedInspectionLocation = new List<string>();
                List<string> SelectedCity = new List<string>();
                List<string> SelectedCountry = new List<string>();

                ViewBag.check = "productcheck";			   
                TempData["QuotNumber"] = QuotationNumber;
                TempData.Keep();
                Session["QuotationNumber"] = QuotationNumber;


				// PK_QM_ID = ObjModelQuotationMast.PK_QTID;
                #region Domestic Order Type 
                DataTable dtDOrderType = new DataTable();
                List<QuotationMaster> lstDOrderType = new List<QuotationMaster>();
                
                dtDOrderType = objDALQuotationMast.DOrderType(PK_QM_ID);
                if (dtDOrderType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDOrderType.Rows)
                    {
                        lstDOrderType.Add(
                           new QuotationMaster
                           {

                               OrderType = Convert.ToString(dr["OrderType"]),
                               OrderRate = Convert.ToString(dr["OrderRate"]),
                               Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                               Distance = Convert.ToString(dr["Distance"]),
                               EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               DExchangeRate = Convert.ToString(dr["DExchangeRate"]),
                               DTotalAmount = Convert.ToString(dr["DTotalAmount"]),
                               DRemarks = Convert.ToString(dr["Remark"]),
                           }
                         );
                    }
                    ViewBag.lstDOrderType = lstDOrderType;
                    
                }
                #endregion


                #region International Order Type 
                DataTable dtIOrderType = new DataTable();
                List<QuotationMaster> lstIOrderType = new List<QuotationMaster>();

                dtIOrderType = objDALQuotationMast.IOrderType(PK_QM_ID);
                if (dtIOrderType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIOrderType.Rows)
                    {
                        lstIOrderType.Add(
                           new QuotationMaster
                           {

                               IOrderType = Convert.ToString(dr["IOrderType"]),
                               IOrderRate = Convert.ToString(dr["IOrderRate"]),
                               IEstimate_ManDays_ManMonth = Convert.ToString(dr["IEstimate_ManDays_ManMonth"]),
                               IDistance = Convert.ToString(dr["IDistance"]),
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               Icurrency = Convert.ToString(dr["Icurrency"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               ITotalAmount = Convert.ToString(dr["ITotalAmount"]),
                               IRemarks = Convert.ToString(dr["Remark"]),
                           }
                         );
                    }
                    ViewBag.lstIOrderType = lstIOrderType;
                }
                
                #endregion	  
                DataTable DTEditQuotationMast = new DataTable();
                DTEditQuotationMast = objDALQuotationMast.EditQuotationMast(PK_QM_ID);//Edit Quotation
                if (DTEditQuotationMast.Rows.Count > 0)
                {
					ObjModelQuotationMast.SendForApprovel = Convert.ToString(DTEditQuotationMast.Rows[0]["SendForApprovel"]);																										 
                    ObjModelQuotationMast.PK_QTID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["PK_QTID"]);
                    ObjModelQuotationMast.QuotationNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationNumber"]);
                    ObjModelQuotationMast.EQ_ID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["EQ_ID"]);
                    ObjModelQuotationMast.QuotationCompanyName = Convert.ToString(DTEditQuotationMast.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.QuotationBranch = Convert.ToInt32(DTEditQuotationMast.Rows[0]["BranchName"]);
                    ObjModelQuotationMast.ServiceType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ServiceType"]);
                    ObjModelQuotationMast.ProjectType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ProjectType"]);
                    
                    ObjModelQuotationMast.PortfolioType = Convert.ToString(DTEditQuotationMast.Rows[0]["PortfolioType"]);
                    ObjModelQuotationMast.SubServiceType = Convert.ToString(DTEditQuotationMast.Rows[0]["SubServiceType"]);

                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTEditQuotationMast.Rows[0]["EnquiryDescriptionAndNotes"]);
                    ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTEditQuotationMast.Rows[0]["Quotation_Description"]);
                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTEditQuotationMast.Rows[0]["EndCustomer"]);
                    ObjModelQuotationMast.Reference = Convert.ToString(DTEditQuotationMast.Rows[0]["Reference"]);
                    ObjModelQuotationMast.ExpiryDate = Convert.ToString(DTEditQuotationMast.Rows[0]["ExpiryDate"]);
                    ObjModelQuotationMast.Status = Convert.ToInt32(DTEditQuotationMast.Rows[0]["Status"]);
                    ObjModelQuotationMast.Statusname = Convert.ToString(DTEditQuotationMast.Rows[0]["StatusName"]);
                    ObjModelQuotationMast.GST = Convert.ToString(DTEditQuotationMast.Rows[0]["GST"]);
                    ObjModelQuotationMast.Attachment = Convert.ToString(DTEditQuotationMast.Rows[0]["Attachment"]);
                    ObjModelQuotationMast.Remark = Convert.ToString(DTEditQuotationMast.Rows[0]["Remark"]);
                    ObjModelQuotationMast.HeaderDetails = Convert.ToString(DTEditQuotationMast.Rows[0]["HeaderDetails"]);
                    ObjModelQuotationMast.Subject = Convert.ToString(DTEditQuotationMast.Rows[0]["Subject"]);
                    ObjModelQuotationMast.Deliverable = Convert.ToString(DTEditQuotationMast.Rows[0]["Deliverable"]);
                    ObjModelQuotationMast.Commercials = Convert.ToString(DTEditQuotationMast.Rows[0]["Commercials"]);
                    ObjModelQuotationMast.FeesStructure = Convert.ToString(DTEditQuotationMast.Rows[0]["FeesStructure"]);
                    ObjModelQuotationMast.PaymentTerms = Convert.ToString(DTEditQuotationMast.Rows[0]["PaymentTerms"]);
                    ObjModelQuotationMast.KeyNotes = Convert.ToString(DTEditQuotationMast.Rows[0]["KeyNotes"]);
                    ObjModelQuotationMast.AddEnclosures = Convert.ToString(DTEditQuotationMast.Rows[0]["AddEnclosures"]);
                    ObjModelQuotationMast.EnquiryNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["EnquiryNumber"]);
                    ObjModelQuotationMast.To = Convert.ToString(DTEditQuotationMast.Rows[0]["T_O"]);
                    ObjModelQuotationMast.CC = Convert.ToString(DTEditQuotationMast.Rows[0]["CC"]);
                    ObjModelQuotationMast.CommunicationProtocol = Convert.ToString(DTEditQuotationMast.Rows[0]["CommunicationProtocol"]);
                    ObjModelQuotationMast.Coordinators = Convert.ToString(DTEditQuotationMast.Rows[0]["Coordinators"]);
                    ObjModelQuotationMast.EscalationMatrix = Convert.ToString(DTEditQuotationMast.Rows[0]["EscalationMatrix"]);
                    ObjModelQuotationMast.ScopeOfWork = Convert.ToString(DTEditQuotationMast.Rows[0]["ScopeOfWork"]);
                    ObjModelQuotationMast.Validity = Convert.ToString(DTEditQuotationMast.Rows[0]["Validity"]);
                    ObjModelQuotationMast.CompanyAddress = Convert.ToString(DTEditQuotationMast.Rows[0]["CompanyAddress"]);
                    ObjModelQuotationMast.ThirdPartyInspectionService = Convert.ToString(DTEditQuotationMast.Rows[0]["ThirdPartyInspectionService"]);
                    ObjModelQuotationMast.ThankYouLetter = Convert.ToString(DTEditQuotationMast.Rows[0]["ThankYouLetter"]);
                    ObjModelQuotationMast.QuotationPDF = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationPdf"]);
                    ObjModelQuotationMast.CostSheetApproveStatus = Convert.ToString(DTEditQuotationMast.Rows[0]["DCostSheetApproveStatus"]);
                    ObjModelQuotationMast.ICostSheetApproveStatus = Convert.ToString(DTEditQuotationMast.Rows[0]["ICostSheetApproveStatus"]);


                    ObjModelQuotationMast.EstimatedAmount = Convert.ToString(DTEditQuotationMast.Rows[0]["EstimatedAmount"]);
                    ObjModelQuotationMast.LostReason = Convert.ToString(DTEditQuotationMast.Rows[0]["LostReason"]);
                    ObjModelQuotationMast.IData = (byte[])(DTEditQuotationMast.Rows[0]["IData"]);

                    ObjModelQuotationMast.DLostReason = Convert.ToString(DTEditQuotationMast.Rows[0]["DLostReason"]);
                    ObjModelQuotationMast.ILostReason = Convert.ToString(DTEditQuotationMast.Rows[0]["ILostReason"]);

                    ObjModelQuotationMast.EnquiryAdditionRef = Convert.ToString(DTEditQuotationMast.Rows[0]["EnquiryAdditionRef"]);

                    //14 jan
                    // ObjModelQuotationMast.InspectionLocation = Convert.ToString(DTEditQuotationMast.Rows[0]["InspectionLocation"]);
                    var EInspectionLocation = Convert.ToString(DTEditQuotationMast.Rows[0]["InspectionLocation"]);
                    InspectionLocation = EInspectionLocation.Split(',');
                    foreach (var single in InspectionLocation)
                    {
                        SelectedInspectionLocation.Add(single);
                    }
                   
                    ViewBag.EditproductName = SelectedInspectionLocation;
                    
                    //ObjModelQuotationMast.City = Convert.ToString(DTEditQuotationMast.Rows[0]["City"]);
                    var ECityName = Convert.ToString(DTEditQuotationMast.Rows[0]["City"]);
                    if (ECityName != string.Empty)
                    {
                        splitedCity = ECityName.Split(',');
                        foreach (var single in splitedCity)
                        {
                            SelectedCity.Add(single);
                        }
                    }
                    ViewBag.EditCityName = SelectedCity;



                    // ObjModelQuotationMast.Country = Convert.ToString(DTEditQuotationMast.Rows[0]["Country"]);
                    var ECountryName = Convert.ToString(DTEditQuotationMast.Rows[0]["Country"]);
                    if (ECountryName != string.Empty)
                    {
                        splitedCountry = ECountryName.Split(',');
                        foreach (var single in splitedCountry)
                        {
                            SelectedCountry.Add(single);
                        }
                    }
                    ViewBag.EditCountryName = SelectedCountry;

                   
                    ObjModelQuotationMast.chkArc		 = Convert.ToBoolean(DTEditQuotationMast.Rows[0]["chkArc"]);
                    ObjModelQuotationMast.OrderRate = Convert.ToString(DTEditQuotationMast.Rows[0]["OrderRate"]);
                    ObjModelQuotationMast.Estimate_ManMonth = Convert.ToString(DTEditQuotationMast.Rows[0]["Estimate_ManMonth"]);
                    ObjModelQuotationMast.Estimate_ManDays_ManMonth = Convert.ToString(DTEditQuotationMast.Rows[0]["Estimate_ManDays_ManMonth"]);
                    ObjModelQuotationMast.OrderType = Convert.ToString(DTEditQuotationMast.Rows[0]["OrderType"]);
                    ObjModelQuotationMast.Exclusion = Convert.ToString(DTEditQuotationMast.Rows[0]["Exclusion"]);
                    ObjModelQuotationMast.Distance = Convert.ToString(DTEditQuotationMast.Rows[0]["Distance"]);

                    ObjModelQuotationMast.DomStatus = Convert.ToInt32(DTEditQuotationMast.Rows[0]["DOM_Status"]);
                    ObjModelQuotationMast.IntStatus = Convert.ToInt32(DTEditQuotationMast.Rows[0]["Int_Status"]);
                    ObjModelQuotationMast.IGST = Convert.ToString(DTEditQuotationMast.Rows[0]["IGST"]);
                    ObjModelQuotationMast.CSSentforApproval = Convert.ToString(DTEditQuotationMast.Rows[0]["CSSentforApproval"]);
                    ObjModelQuotationMast.ICSSentforApproval = Convert.ToString(DTEditQuotationMast.Rows[0]["ICSSentforApproval"]);

                    ObjModelQuotationMast.CreatedBy = Convert.ToString(DTEditQuotationMast.Rows[0]["CreatedBy"]);
                    ObjModelQuotationMast.GeneralTerms = Convert.ToString(DTEditQuotationMast.Rows[0]["GeneralTerms"]);
                    ObjModelQuotationMast.ComplimentaryClose = Convert.ToString(DTEditQuotationMast.Rows[0]["ComplimentaryClose"]);
                    ObjModelQuotationMast.QuotationImage = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationImage"]);
                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTEditQuotationMast.Rows[0]["EnquiryDescriptionAndNotes"]);
                    ObjModelQuotationMast.ReviseReason = Convert.ToString(DTEditQuotationMast.Rows[0]["ReviseReason"]);
                    if(ObjModelQuotationMast.Status==5)
                    {
                        ObjModelQuotationMast.Revise = "Revise";
                    }
                    else
                    {
                        ObjModelQuotationMast.Revise = "";
                    }

                    if (Convert.ToString(DTEditQuotationMast.Rows[0]["ExclusionCheckBox"]) =="1")
                    {
                        ObjModelQuotationMast.ExclusionCheckBox = true;
                    }
                    else
                    {
                        ObjModelQuotationMast.ExclusionCheckBox = false;
                    }

                    if (Convert.ToString(DTEditQuotationMast.Rows[0]["GeneralTermsCheckbox"]) == "1")
                    {
                        ObjModelQuotationMast.GeneralTermsCheckbox = true;
                    }
                    else
                    {
                        ObjModelQuotationMast.GeneralTermsCheckbox = false;
                    }


                    if (Convert.ToInt32(ObjModelQuotationMast.EstimatedAmount) >= 1000000)
                    {
                        if (ViewBag.EditproductName.Count > 1)
                        {
                            /// Domestic
                            DataTable DTCLEditQuotationMast = new DataTable();
                            DTCLEditQuotationMast = objDALQuotationMast.GetCLApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.CostSheetCLStatus = DTCLEditQuotationMast.Rows[0]["CLStatus"].ToString();

                            /// International
                            DataTable DTICLEditQuotationMast = new DataTable();
                            DTICLEditQuotationMast = objDALQuotationMast.GetICLApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.ICostSheetCLStatus = DTICLEditQuotationMast.Rows[0]["CLStatus"].ToString();

                        }
                        else if (ViewBag.EditproductName.Contains("2"))
                        {
                            DataTable DTCLEditQuotationMast = new DataTable();
                            DTCLEditQuotationMast = objDALQuotationMast.GetCLApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.CostSheetCLStatus = DTCLEditQuotationMast.Rows[0]["CLStatus"].ToString();
                        }
                        else if (ViewBag.EditproductName.Contains("1"))
                        {
                            DataTable DTCLEditQuotationMast = new DataTable();
                            DTCLEditQuotationMast = objDALQuotationMast.GetICLApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.ICostSheetCLStatus = DTCLEditQuotationMast.Rows[0]["CLStatus"].ToString();
                        }
                        else if (ViewBag.EditproductName.Count > 1)
                        {
                            /// Domestic
                            DataTable DTCLEditQuotationMast = new DataTable();
                            DTCLEditQuotationMast = objDALQuotationMast.GetCLApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.CostSheetCLStatus = DTCLEditQuotationMast.Rows[0]["CLStatus"].ToString();

                            /// International
                            DataTable DTICLEditQuotationMast = new DataTable();
                            DTICLEditQuotationMast = objDALQuotationMast.GetICLApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.ICostSheetCLStatus = DTICLEditQuotationMast.Rows[0]["CLStatus"].ToString();

                        }
					}                   
                }
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                PK_QM_ID = ObjModelQuotationMast.PK_QTID;
                DTGetUploadedFile = objDALQuotationMast.EditUploadedFile(PK_QM_ID);
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
                    ObjModelQuotationMast.FileDetails = lstEditFileDetails;
                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file
                #region Bind Company Addr
                DataSet DsCompanyAddr = new DataSet();
                List<NameCode> lstCompanyAddr = new List<NameCode>();

                DsCompanyAddr = objDALQuotationMast.GetCompanyAddr(Convert.ToInt32(ObjModelQuotationMast.EQ_ID));
                //DsCompanyAddr = objDALQuotationMast.GetCompanyAddr(Convert.ToInt32(ObjModelQuotationMast.CompanyAddress));
                if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                {
                    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                          Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                      }).ToList();
                }

                IEnumerable<SelectListItem> ComAddrItems;
                ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

                //ViewBag.ComAddr = ComAddrItems;
                ViewData["ComAddr1"] = ComAddrItems;
                #endregion

                DataSet DSGetEditQuotationAllddllst = new DataSet();
                List<NameCode> lstEditBranch = new List<NameCode>();
                List<NameCode> lstEditServiceType = new List<NameCode>();
                List<NameCode> lstEditProjectType = new List<NameCode>();
                List<NameCode> lstEditStatusType = new List<NameCode>();
                List<NameCode> lstEditProfileType = new List<NameCode>();
                

				List<NameCode> lstInspectionLocation = new List<NameCode>();
                List<NameCode> lstCityName = new List<NameCode>();
                List<NameCode> lstCountryName = new List<NameCode>();
                List<NameCode> lstDCurrency = new List<NameCode>();
                List<NameCode> lstICurrency = new List<NameCode>();
                DSGetEditQuotationAllddllst = objDALQuotationMast.GetEditAllddlLst();
                if (DSGetEditQuotationAllddllst.Tables[0].Rows.Count > 0)
                {
                    lstEditProjectType = (from n in DSGetEditQuotationAllddllst.Tables[0].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[0].Columns["ProjectName"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[0].Columns["PK_ID"].ToString())

                                          }).ToList();
                }

                IEnumerable<SelectListItem> ProjectTypeItems;
                ProjectTypeItems = new SelectList(lstEditProjectType, "Code", "Name");
                ViewBag.ProjectTypeItems = ProjectTypeItems;
                ViewData["ProjectTypeItems"] = ProjectTypeItems;

                if (DSGetEditQuotationAllddllst.Tables[1].Rows.Count > 0)
                {
                    lstEditBranch = (from n in DSGetEditQuotationAllddllst.Tables[1].AsEnumerable()
                                     select new NameCode()
                                     {
                                         Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[1].Columns["BranchName"].ToString()),
                                         Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[1].Columns["PK_ID"].ToString())

                                     }).ToList();
                }
                IEnumerable<SelectListItem> BranchNameTypeItems;
                BranchNameTypeItems = new SelectList(lstEditBranch, "Code", "Name");
                ViewBag.BranchNameTypeItems = BranchNameTypeItems;
                ViewData["BranchNameTypeItems"] = BranchNameTypeItems;

                if (DSGetEditQuotationAllddllst.Tables[2].Rows.Count > 0)
                {
                    lstEditServiceType = (from n in DSGetEditQuotationAllddllst.Tables[2].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[2].Columns["Name"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[2].Columns["PK_ID"].ToString())

                                          }).ToList();
                }
                IEnumerable<SelectListItem> ServiceTypeItems;
                ServiceTypeItems = new SelectList(lstEditServiceType, "Code", "Name");
                ViewBag.ServiceTypeItems = ServiceTypeItems;
                ViewData["ServiceTypeItems"] = ServiceTypeItems;

                if (DSGetEditQuotationAllddllst.Tables[3].Rows.Count > 0)
                {
                    lstEditStatusType = (from n in DSGetEditQuotationAllddllst.Tables[3].AsEnumerable()
                                         select new NameCode()
                                         {
                                             Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[3].Columns["QTStatus"].ToString()),
                                             Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[3].Columns["PK_ID"].ToString())

                                         }).ToList();
                }
                IEnumerable<SelectListItem> StatusTypeItems;
                StatusTypeItems = new SelectList(lstEditStatusType, "Code", "Name");
                ViewBag.StatusTypeItems = StatusTypeItems;
                ViewData["StatusTypeItems"] = StatusTypeItems;

                if (DSGetEditQuotationAllddllst.Tables[4].Rows.Count > 0)
                {
                    lstEditProfileType = (from n in DSGetEditQuotationAllddllst.Tables[4].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[4].Columns["Name"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[4].Columns["PK_ID"].ToString())

                                          }).ToList();
                }

                IEnumerable<SelectListItem> PortfolioTypeItems;
                PortfolioTypeItems = new SelectList(lstEditProfileType, "Code", "Name");
                ViewBag.ProfileTypeItems = PortfolioTypeItems;
                ViewData["PortfolioTypeItems"] = PortfolioTypeItems;

				if (DSGetEditQuotationAllddllst.Tables[7].Rows.Count > 0)
                {
                    lstInspectionLocation = (from n in DSGetEditQuotationAllddllst.Tables[7].AsEnumerable()
                                             select new NameCode()
                                             {
                                                 Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[7].Columns["Name"].ToString()),
                                                 Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[7].Columns["PK_ID"].ToString())

                                             }).ToList();
                }
                IEnumerable<SelectListItem> InspectionLocationItems;
                InspectionLocationItems = new SelectList(lstInspectionLocation, "Code", "Name");
                ViewBag.InspectionLocation = InspectionLocationItems;
                ViewBag.InspectionLocationItems = InspectionLocationItems;


                if (DSGetEditQuotationAllddllst.Tables[5].Rows.Count > 0)//City
                {
                    lstCityName = (from n in DSGetEditQuotationAllddllst.Tables[5].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[5].Columns["CityName"].ToString()),
                                       Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[5].Columns["PK_ID"].ToString())

                                   }).ToList();
                }
                //ViewBag.CityName = lstCityName;
                IEnumerable<SelectListItem> CityNameItems;//14
                CityNameItems = new SelectList(lstCityName, "Code", "Name");
                ViewBag.CityNameItems = CityNameItems;
                ViewData["CityNameItems"] = CityNameItems;



                if (DSGetEditQuotationAllddllst.Tables[6].Rows.Count > 0)//Country
                {
                    lstCountryName = (from n in DSGetEditQuotationAllddllst.Tables[6].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[6].Columns["CountryName"].ToString()),
                                          Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[6].Columns["PK_ID"].ToString())

                                      }).ToList();
                }
                //ViewBag.CountryName = lstCountryName;
                IEnumerable<SelectListItem> CountryNameItems;
                CountryNameItems = new SelectList(lstCountryName, "Code", "Name");
                ViewBag.CountryNameItems = CountryNameItems;
                ViewData["CountryNameItems"] = CountryNameItems;


                if (DSGetEditQuotationAllddllst.Tables[8].Rows.Count > 0)//All Currency 
                {
                    lstDCurrency = (from n in DSGetEditQuotationAllddllst.Tables[8].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[8].Columns["CurrencyCode"].ToString()),
                                       Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[8].Columns["CurrencyID"].ToString())

                                   }).ToList();
                }
                ViewBag.DTestCurrency = lstDCurrency;
                ViewBag.ITestCurrency = lstDCurrency;
                ViewBag.Currency = lstDCurrency;
                IEnumerable<SelectListItem> CurrencyItems;
                CurrencyItems = new SelectList(lstDCurrency, "Code", "Name");
                ViewBag.CurrencyItems = CurrencyItems;
                //Binding Job List against Quotation
                List<QuotationMaster> lstJobMast = new List<QuotationMaster>();
                lstJobMast = objDALQuotationMast.GetJobNoForQuotation(QuotationNumber);
                ObjModelQuotationMast.JobDashBoard = lstJobMast;
                ViewData["JobMaster"] = lstJobMast;

                return View(ObjModelQuotationMast);

            }
            if (PK_EQID > 0 && PK_EQID != null)
            {
				 string[] splitedCity;
                string[] splitedCountry;

                string[] InspectionLocation;
                List<string> Selected = new List<string>();
                List<string> SelectedInspectionLocation = new List<string>();
                List<string> SelectedCity = new List<string>();
                List<string> SelectedCountry = new List<string>();										   
                DataTable DTGetEnquiryDtls = new DataTable();
                ViewBag.check = "productcheck";
                DTGetEnquiryDtls = objDALQuotationMast.GetEnquiryDetals(PK_EQID);
                if (DTGetEnquiryDtls.Rows.Count > 0)
                {
                    ObjModelQuotationMast.EQ_ID = Convert.ToInt32(DTGetEnquiryDtls.Rows[0]["EQ_ID"]);
                    ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryDescription"]);
                    ObjModelQuotationMast.QuotationCompanyName = Convert.ToString(DTGetEnquiryDtls.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EndCustomer"]);
                    ObjModelQuotationMast.ExpiryDate = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EstClose"]);
                    //ObjModelQuotationMast.Reference = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryReferenceNo"]);
                    ObjModelQuotationMast.EnquiryAdditionRef = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryReferenceNo"]);
                    //ObjModelQuotationMast.EstimatedAmount = Convert.ToDecimal(DTGetEnquiryDtls.Rows[0]["EstimatedAmount"]);
                    ObjModelQuotationMast.ProjectType = Convert.ToInt32(DTGetEnquiryDtls.Rows[0]["ProjectType"]);
                    ObjModelQuotationMast.PortfolioType = Convert.ToString(DTGetEnquiryDtls.Rows[0]["PortfolioType"]);
                    ObjModelQuotationMast.SubServiceType = Convert.ToString(DTGetEnquiryDtls.Rows[0]["SubServiceType"]);

                    //ObjModelQuotationMast.Notes = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Notes"]);
                    ObjModelQuotationMast.QuotationBranch = Convert.ToInt32(DTGetEnquiryDtls.Rows[0]["Branch"]);
                    ObjModelQuotationMast.ServiceType = Convert.ToInt32(DTGetEnquiryDtls.Rows[0]["Type"]);
                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTGetEnquiryDtls.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.Status = Convert.ToInt32(DTGetEnquiryDtls.Rows[0]["Status"]);
					ObjModelQuotationMast.Statusname = Convert.ToString(DTGetEnquiryDtls.Rows[0]["StatusName"]);																					
                    ObjModelQuotationMast.EnquiryNumber = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryNumber"]);
                    ObjModelQuotationMast.FK_CMP_ID = Convert.ToInt32(DTGetEnquiryDtls.Rows[0]["FK_CMP_ID"]);
                    ObjModelQuotationMast.CompanyAddress = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Address"]);
					ObjModelQuotationMast.chkArc = Convert.ToBoolean(DTGetEnquiryDtls.Rows[0]["chkArc"]);
                    ObjModelQuotationMast.InspectionLocation = Convert.ToString(DTGetEnquiryDtls.Rows[0]["InspectionLocation"]);

                    var EInspectionLocation = Convert.ToString(DTGetEnquiryDtls.Rows[0]["InspectionLocation"]);
                    InspectionLocation = EInspectionLocation.Split(',');
                    foreach (var single in InspectionLocation)
                    {
                        SelectedInspectionLocation.Add(single);
                    }

                    ViewBag.EditproductName = SelectedInspectionLocation;

                    var ECityName = Convert.ToString(DTGetEnquiryDtls.Rows[0]["City"]);
                    if (ECityName != string.Empty)
                    {
                        splitedCity = ECityName.Split(',');
                        foreach (var single in splitedCity)
                        {
                            SelectedCity.Add(single);
                        }
                    }
                    ViewBag.EditCityName = SelectedCity;



                    // ObjModelQuotationMast.Country = Convert.ToString(DTEditQuotationMast.Rows[0]["Country"]);
                    var ECountryName = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Country"]);
                    if (ECountryName != string.Empty)
                    {
                        splitedCountry = ECountryName.Split(',');
                        foreach (var single in splitedCountry)
                        {
                            SelectedCountry.Add(single);
                        }
                    }
                    ViewBag.EditCountryName = SelectedCountry;


                }

                #region Domestic Order Type 
                DataTable dtDOrderType = new DataTable();
                List<QuotationMaster> lstDOrderType = new List<QuotationMaster>();

                dtDOrderType = objDalEnquiryMaster.DOrderType(PK_EQID);
                if (dtDOrderType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDOrderType.Rows)
                    {
                        lstDOrderType.Add(
                           new QuotationMaster
                           {

                               OrderType = Convert.ToString(dr["OrderType"]),
                               OrderRate = Convert.ToString(dr["OrderRate"]),
                               Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                               Distance = Convert.ToString(dr["Distance"]),
                               EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               DExchangeRate = Convert.ToString(dr["DExchangeRate"]),
                               DTotalAmount = Convert.ToString(dr["DTotalAmount"]),
                           }
                         );
                    }
                    ViewBag.lstDOrderType = lstDOrderType;

                }
                #endregion


                #region International Order Type 
                DataTable dtIOrderType = new DataTable();
                List<QuotationMaster> lstIOrderType = new List<QuotationMaster>();

                dtIOrderType = objDalEnquiryMaster.IOrderType(PK_EQID);
                if (dtIOrderType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIOrderType.Rows)
                    {
                        lstIOrderType.Add(
                           new QuotationMaster
                           {

                               IOrderType = Convert.ToString(dr["IOrderType"]),
                               IOrderRate = Convert.ToString(dr["IOrderRate"]),
                               IEstimate_ManDays_ManMonth = Convert.ToString(dr["IEstimate_ManDays_ManMonth"]),
                               IDistance = Convert.ToString(dr["IDistance"]),
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               Icurrency = Convert.ToString(dr["Icurrency"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               ITotalAmount = Convert.ToString(dr["ITotalAmount"]),
                           }
                         );
                    }
                    ViewBag.lstIOrderType = lstIOrderType;
                }

                #endregion



                DataSet DSGetEditAllddllst = new DataSet();
               
                List<NameCode> lstEditBranch = new List<NameCode>();
                List<NameCode> lstEditServiceType = new List<NameCode>();
                List<NameCode> lstEditProjectType = new List<NameCode>();
                List<NameCode> lstEditStatusType = new List<NameCode>();
                List<NameCode> lstEditPortfolioType = new List<NameCode>();
                
				List<NameCode> lstInspectionLocation = new List<NameCode>();
                List<NameCode> lstCityName = new List<NameCode>();
                List<NameCode> lstCountryName = new List<NameCode>();
                List<NameCode> lstCurrency = new List<NameCode>();

                List<NameCode> lstDCurrency = new List<NameCode>();
                List<NameCode> lstICurrency = new List<NameCode>();

                #region Bind Company Addr
                //DataSet DsCompanyAddr = new DataSet();
                //List<NameCode> lstCompanyAddr = new List<NameCode>();

                DsCompanyAddr = objDALQuotationMast.GetCompanyAddr(PK_EQID);
                if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                {
                    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                              Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                          }).ToList();
                }

                //IEnumerable<SelectListItem> ComAddrItems;
                ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

                //ViewBag.ComAddr = ComAddrItems;
                ViewData["ComAddr1"] = ComAddrItems;
                #endregion


                DSGetEditAllddllst = objDALQuotationMast.GetEditAllddlLst();
                if (DSGetEditAllddllst.Tables[0].Rows.Count > 0)
                {
                    lstEditProjectType = (from n in DSGetEditAllddllst.Tables[0].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditAllddllst.Tables[0].Columns["ProjectName"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditAllddllst.Tables[0].Columns["PK_ID"].ToString())

                                          }).ToList();
                }

                IEnumerable<SelectListItem> ProjectTypeItems;
                ProjectTypeItems = new SelectList(lstEditProjectType, "Code", "Name");
                ViewBag.ProjectTypeItems = ProjectTypeItems;
                ViewData["ProjectTypeItems1"] = ProjectTypeItems;

                if (DSGetEditAllddllst.Tables[1].Rows.Count > 0)
                {
                    lstEditBranch = (from n in DSGetEditAllddllst.Tables[1].AsEnumerable()
                                     select new NameCode()
                                     {
                                         Name = n.Field<string>(DSGetEditAllddllst.Tables[1].Columns["BranchName"].ToString()),
                                         Code = n.Field<Int32>(DSGetEditAllddllst.Tables[1].Columns["PK_ID"].ToString())

                                     }).ToList();
                }
                IEnumerable<SelectListItem> BranchNameTypeItems;
                BranchNameTypeItems = new SelectList(lstEditBranch, "Code", "Name");
                ViewBag.BranchNameTypeItems = BranchNameTypeItems;
                ViewData["BranchNameTypeItems"] = BranchNameTypeItems;

                if (DSGetEditAllddllst.Tables[2].Rows.Count > 0)
                {
                    lstEditServiceType = (from n in DSGetEditAllddllst.Tables[2].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditAllddllst.Tables[2].Columns["Name"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditAllddllst.Tables[2].Columns["PK_ID"].ToString())

                                          }).ToList();
                }
                IEnumerable<SelectListItem> ServiceTypeItems;
                ServiceTypeItems = new SelectList(lstEditServiceType, "Code", "Name");
                ViewBag.ServiceTypeItems = ServiceTypeItems;
                ViewData["ServiceTypeItems"] = ServiceTypeItems;

                if (DSGetEditAllddllst.Tables[3].Rows.Count > 0)
                {
                    lstEditStatusType = (from n in DSGetEditAllddllst.Tables[3].AsEnumerable()
                                         select new NameCode()
                                         {
                                             Name = n.Field<string>(DSGetEditAllddllst.Tables[3].Columns["QTStatus"].ToString()),
                                             Code = n.Field<Int32>(DSGetEditAllddllst.Tables[3].Columns["PK_ID"].ToString())

                                         }).ToList();
                }
                IEnumerable<SelectListItem> StatusTypeItems;
                StatusTypeItems = new SelectList(lstEditStatusType, "Code", "Name");
                ViewBag.StatusTypeItems = StatusTypeItems;
                ViewData["StatusTypeItems"] = StatusTypeItems;


                if (DSGetEditAllddllst.Tables[4].Rows.Count > 0)
                {
                    lstEditPortfolioType = (from n in DSGetEditAllddllst.Tables[4].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditAllddllst.Tables[4].Columns["Name"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditAllddllst.Tables[4].Columns["PK_ID"].ToString())

                                          }).ToList();
                }

                IEnumerable<SelectListItem> PortfolioTypeItems;
                PortfolioTypeItems = new SelectList(lstEditPortfolioType, "Code", "Name");
                ViewBag.ProfileTypeItems = PortfolioTypeItems;
                ViewData["PortfolioTypeItems"] = PortfolioTypeItems;

				if (DSGetEditAllddllst.Tables[7].Rows.Count > 0)
                {
                    lstInspectionLocation = (from n in DSGetEditAllddllst.Tables[7].AsEnumerable()
                                             select new NameCode()
                                             {
                                                 Name = n.Field<string>(DSGetEditAllddllst.Tables[7].Columns["Name"].ToString()),
                                                 Code = n.Field<Int32>(DSGetEditAllddllst.Tables[7].Columns["PK_ID"].ToString())

                                             }).ToList();
                }
                IEnumerable<SelectListItem> InspectionLocationItems;
                InspectionLocationItems = new SelectList(lstInspectionLocation, "Code", "Name");
                ViewBag.InspectionLocation = InspectionLocationItems;
                ViewBag.InspectionLocationItems = InspectionLocationItems;


                if (DSGetEditAllddllst.Tables[5].Rows.Count > 0)//City
                {
                    lstCityName = (from n in DSGetEditAllddllst.Tables[5].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(DSGetEditAllddllst.Tables[5].Columns["CityName"].ToString()),
                                       Code = n.Field<Int32>(DSGetEditAllddllst.Tables[5].Columns["PK_ID"].ToString())

                                   }).ToList();
                }
                //ViewBag.CityName = lstCityName;
                //24 June
                IEnumerable<SelectListItem> CityNameItems;//14
                CityNameItems = new SelectList(lstCityName, "Code", "Name");
                ViewBag.CityNameItems = CityNameItems;
                ViewData["CityNameItems"] = CityNameItems;



                if (DSGetEditAllddllst.Tables[6].Rows.Count > 0)//Country
                {
                    lstCountryName = (from n in DSGetEditAllddllst.Tables[6].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetEditAllddllst.Tables[6].Columns["CountryName"].ToString()),
                                          Code = n.Field<Int32>(DSGetEditAllddllst.Tables[6].Columns["PK_ID"].ToString())

                                      }).ToList();
                }
                //ViewBag.CountryName = lstCountryName;
                ViewBag.CountryNameItems = lstCountryName;
                //24 June
                IEnumerable<SelectListItem> CountryNameItems;
                CountryNameItems = new SelectList(lstCountryName, "Code", "Name");
                ViewBag.CountryNameItems = CountryNameItems;
                ViewData["CountryNameItems"] = CountryNameItems;


                if (DSGetEditAllddllst.Tables[8].Rows.Count > 0)//All Currency 
                {
                    lstDCurrency = (from n in DSGetEditAllddllst.Tables[8].AsEnumerable()
                                    select new NameCode()
                                    {
                                        Name = n.Field<string>(DSGetEditAllddllst.Tables[8].Columns["CurrencyCode"].ToString()),
                                        Code = n.Field<Int32>(DSGetEditAllddllst.Tables[8].Columns["CurrencyID"].ToString())

                                    }).ToList();
                }
                ViewBag.Currency = lstDCurrency;
                ViewBag.DTestCurrency = lstDCurrency;
                ViewBag.ITestCurrency = lstDCurrency;
                IEnumerable<SelectListItem> CurrencyItems;
                CurrencyItems = new SelectList(lstDCurrency, "Code", "Name");
                //ViewBag.DCurrencyItems = CurrencyItems;
                ViewBag.CurrencyItems = CurrencyItems;
                return View(ObjModelQuotationMast);

            }
            else
            {
                DataSet DSGetAllddllst = new DataSet();
                List<NameCode> lstBranch = new List<NameCode>();
                List<NameCode> lstServiceType = new List<NameCode>();
                List<NameCode> lstProjectType = new List<NameCode>();
                List<NameCode> lstStatusType = new List<NameCode>();
                List<NameCode> lstPortfolioType = new List<NameCode>();
                
                DSGetAllddllst = objDALQuotationMast.GetAllddlLst();
                if (DSGetAllddllst.Tables[0].Rows.Count > 0)
                {
                    lstProjectType = (from n in DSGetAllddllst.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetAllddllst.Tables[0].Columns["ProjectName"].ToString()),
                                          Code = n.Field<Int32>(DSGetAllddllst.Tables[0].Columns["PK_ID"].ToString())

                                      }).ToList();
                }
                ViewBag.ProjectType = lstProjectType;

                if (DSGetAllddllst.Tables[1].Rows.Count > 0)
                {
                    lstBranch = (from n in DSGetAllddllst.Tables[1].AsEnumerable()
                                 select new NameCode()
                                 {
                                     Name = n.Field<string>(DSGetAllddllst.Tables[1].Columns["BranchName"].ToString()),
                                     Code = n.Field<Int32>(DSGetAllddllst.Tables[1].Columns["PK_ID"].ToString())

                                 }).ToList();
                }
                ViewBag.Branch = lstBranch;

                if (DSGetAllddllst.Tables[2].Rows.Count > 0)
                {
                    lstServiceType = (from n in DSGetAllddllst.Tables[2].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetAllddllst.Tables[2].Columns["Name"].ToString()),
                                          Code = n.Field<Int32>(DSGetAllddllst.Tables[2].Columns["PK_ID"].ToString())

                                      }).ToList();
                }
                ViewBag.ServiceType = lstServiceType;

                if (DSGetAllddllst.Tables[3].Rows.Count > 0)
                {
                    lstStatusType = (from n in DSGetAllddllst.Tables[3].AsEnumerable()
                                     select new NameCode()
                                     {
                                         Name = n.Field<string>(DSGetAllddllst.Tables[3].Columns["QTStatus"].ToString()),
                                         Code = n.Field<Int32>(DSGetAllddllst.Tables[3].Columns["PK_ID"].ToString())
                                     }).ToList();
                }
                ViewBag.StatusType = lstStatusType;

                if (DSGetAllddllst.Tables[4].Rows.Count > 0)
                {
                    lstPortfolioType = (from n in DSGetAllddllst.Tables[4].AsEnumerable()
                                            select new NameCode()
                                            {
                                                Name = n.Field<string>(DSGetAllddllst.Tables[4].Columns["Name"].ToString()),
                                                Code = n.Field<Int32>(DSGetAllddllst.Tables[4].Columns["PK_ID"].ToString())

                                            }).ToList();
                }

                IEnumerable<SelectListItem> PortfolioTypeItems;
                PortfolioTypeItems = new SelectList(lstPortfolioType, "Code", "Name");
                ViewBag.ProfileTypeItems = PortfolioTypeItems;
                ViewData["PortfolioTypeItems"] = PortfolioTypeItems;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Quotation(QuotationMaster QM, FormCollection fc, string C,string ExclusionCheckBox, List<DCurrency> DArray, List<ICurrency> IArray, string GeneralTermsCheckbox)
        {
            string Result = string.Empty;
            string Status = string.Empty;
            string IPath = string.Empty;
            int QuotationID = 0;
			if(C=="1")
            {
                QM.chkArc = true;
            }
            else
            {
                QM.chkArc = false;
            }
            if (ExclusionCheckBox == "1")
            {
                QM.ExclusionCheckBox = true;
            }
            else
            {
                QM.ExclusionCheckBox = false;
            }
            if (GeneralTermsCheckbox == "1")
            {
                QM.GeneralTermsCheckbox = true;
            }
            else
            {
                QM.GeneralTermsCheckbox = false;
            }
            

            string QtnNo = string.Empty;
            if (QM.QuotationNumber != null)
            {
                int VarLength = Regex.Matches(QM.QuotationNumber, "/").Count;
                if (VarLength > 4)
                {
                    QtnNo = QM.QuotationNumber.Substring(0, QM.QuotationNumber.LastIndexOf("/") + 0);
                }
                else
                {
                    QtnNo = QM.QuotationNumber;
                }
            }            

            var list = Session["list"] as List<string>;

            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listQuotationUploadedFile"] as List<FileDetails>;

            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }
            try
            {
				 if (QM.InspectionLocation == "2") /// Only Domestic
                {
                    //// 2 - WON  1---- LOST
                    if (QM.DomStatus == 2)
                    {
                        QM.Status = 3; /// Won
                    }
                    else if (QM.DomStatus == 1)
                    {
                        QM.Status = 4; /// LOSt
                    }
                    else
                    {
                        QM.Status = 1; /// Open
                    }

                }
                else if (QM.InspectionLocation == "1") /// Only International
                {

                    if (QM.IntStatus == 2)
                    {
                        QM.Status = 3; /// WON
                    }
                    else if (QM.IntStatus == 1)
                    {
                        QM.Status = 4; /// LOSt
                    }
                    else
                    {
                        QM.Status = 1; /// Open
                    }
                }
                else if (QM.InspectionLocation == "1,2") /// Both Domestic & Int
                {
                    //// 2 - WON  1---- LOST
                    if (QM.DomStatus == 2 && QM.IntStatus == 2)
                    {
                        QM.Status = 3; /// Won
                    }
                    else if (QM.DomStatus == 2 || QM.IntStatus == 2)
                    {
                        QM.Status = 8; /// Partially Won
                    }
                    else if (QM.DomStatus == 1 && QM.IntStatus == 1)
                    {
                        QM.Status = 4; /// LOSt
                    }
                    else
                    {
                        QM.Status = 1; /// Open
                    }
                }
				
                if (QM.Revise == null && QM.QuotationNumber != "" && QM.QuotationNumber != null) //====Edit
                {
                    Result = objDALQuotationMast.InsertAndUpdateQuotation(QM, IPath);
                    if (Result != null)
                    {
                        
                        if (QM.Status == Convert.ToInt32(3))
                        {
                            //Status = objDALQuotationMast.UpdateStatus(Convert.ToString(QM.QuotationNumber), Convert.ToString(QtnNo));
                        }                        
                    }
                    QuotationID = QM.PK_QTID;

                    #region vai 14 Sept
                    if (QM.DomStatus == 2)
                    {

                    }
                    else
                    {
                        #region Insert Update Order Type Domestic
                        if (DArray != null)
                        {


                            foreach (var d in DArray)
                            {
                                QM.OrderType = d.OrderType;
                                QM.OrderRate = d.OrderRate;
                                QM.Estimate_ManDays_ManMonth = d.Estimate_ManDays;
                                QM.Estimate_ManMonth = d.Estimate_ManMonth;
                                QM.Distance = d.Distance;
                                QM.DEstimatedAmount = d.EstimatedAmount;
                                QM.Dcurrency = d.Currency;
                                QM.DExchangeRate = d.ExchangeRate;
                                QM.DTotalAmount = d.DTotalAmount;
                                QM.PK_QTID = QuotationID;
                                QM.Type = "D";
                                Result = objDALQuotationMast.InsertUpdateOrderType(QM);
                            }
                        }
                        #endregion
                    }
                    if (QM.IntStatus == 2)
                    {

                    }
                    else
                    {
                        #region Insert Update Order Type International
                        if (IArray != null)
                        {
                            foreach (var d in IArray)
                            {
                                QM.IOrderType = d.IOrderType;
                                QM.IOrderRate = d.IOrderRate;
                                QM.IEstimate_ManDays_ManMonth = d.IEstimate_ManDays;
                                QM.IEstimate_ManMonth = d.IEstimate_ManMonth;
                                QM.IDistance = d.IDistance;
                                QM.IEstimatedAmount = d.IEstimatedAmount;
                                QM.Icurrency = d.InternationalCurrency;
                                QM.IExchangeRate = d.IExchangeRate;
                                QM.ITotalAmount = d.ITotalAmount;
                                QM.PK_QTID = QuotationID;
                                QM.IRemarks = d.IRemark;
                                QM.Type = "I";
                                Result = objDALQuotationMast.InsertUpdateIOrderType(QM);
                            }
                        }


                        #endregion
                    }
                    GeneratePDF(QM);
                    #endregion

                    if (QuotationID != null && QuotationID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, QuotationID);
                            Result = objDALQuotationMast.InsertFileAttachment(lstFileDtls, QuotationID);
                            Session["listQuotationUploadedFile"] = null;
                        }
                    }
                    return Json(new { result = "Redirect", url = Url.Action("Quotation", "QuotationMaster", new { @QuotationNumber = QM.QuotationNumber, @PK_QM_ID = QuotationID }) });
                }
                if (QM.PK_QTID != 0 && QM.Revise != null) //======Revision
                {
                    Result = objDALQuotationMast.InsertAndUpdateQuotation(QM, IPath);
                    QuotationID = Convert.ToInt32(Session["QTID"]);

                    #region Insert Update Order Type Domestic
                    if (DArray != null)
                    {
                        foreach (var d in DArray)
                        {
                            QM.OrderType = d.OrderType;
                            QM.OrderRate = d.OrderRate;
                            QM.Estimate_ManDays_ManMonth = d.Estimate_ManDays;
                            QM.Estimate_ManMonth = d.Estimate_ManMonth;
                            QM.Distance = d.Distance;
                            QM.DEstimatedAmount = d.EstimatedAmount;
                            QM.Dcurrency = d.Currency;
                            QM.DExchangeRate = d.ExchangeRate;
                            QM.DTotalAmount = d.DTotalAmount;
                            QM.PK_QTID = QuotationID;
                            QM.Type = "D";
                            Result = objDALQuotationMast.InsertUpdateOrderType(QM);
                        }
                    }
                    #endregion

                    #region Insert Update Order Type International
                    if (IArray != null)
                    {
                        foreach (var d in IArray)
                        {
                            QM.IOrderType = d.IOrderType;
                            QM.IOrderRate = d.IOrderRate;
                            QM.IEstimate_ManDays_ManMonth = d.IEstimate_ManDays;
                            QM.IEstimate_ManMonth = d.IEstimate_ManMonth;
                            QM.IDistance = d.IDistance;
                            QM.IEstimatedAmount = d.IEstimatedAmount;
                            QM.Icurrency = d.InternationalCurrency;
                            QM.IExchangeRate = d.IExchangeRate;
                            QM.ITotalAmount = d.ITotalAmount;
                            QM.PK_QTID = QuotationID;
                            QM.Type = "I";
                            Result = objDALQuotationMast.InsertUpdateIOrderType(QM);
                        }
                    }


                    #endregion


                    if (QuotationID != null && QuotationID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = objDALQuotationMast.InsertFileAttachment(lstFileDtls, QuotationID);
                            Session["listQuotationUploadedFile"] = null;
                        }
                    }
                    //return Json(new { success = 2, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                    return Json(new { result = "RedirectRevised", url = Url.Action("Quotation", "QuotationMaster", new { @PK_QM_ID = QuotationID }) });
                }
                else //=====Insert
                {
                    Result = objDALQuotationMast.InsertAndUpdateQuotation(QM, IPath);
                    QuotationID = Convert.ToInt32(Session["QTID"]);

					#region Insert Update Order Type Domestic
                    if(DArray != null)
                    {

                    
                    foreach (var d in DArray)
                    {
                        QM.OrderType = d.OrderType;
                        QM.OrderRate = d.OrderRate;
                        QM.Estimate_ManDays_ManMonth = d.Estimate_ManDays;
                        QM.Estimate_ManMonth = d.Estimate_ManMonth;
                        QM.Distance = d.Distance;
                        QM.DEstimatedAmount = d.EstimatedAmount;
                        QM.Dcurrency = d.Currency;
                        QM.DExchangeRate = d.ExchangeRate;
                        QM.DTotalAmount = d.DTotalAmount;
                        QM.PK_QTID = QuotationID;
                        QM.Type = "D";
                        Result = objDALQuotationMast.InsertUpdateOrderType(QM);
                    }
                    }
                    #endregion

                    #region Insert Update Order Type International
                    if (IArray != null)
                    {
                        foreach (var d in IArray)
                        {
                            QM.IOrderType = d.IOrderType;
                            QM.IOrderRate = d.IOrderRate;
                            QM.IEstimate_ManDays_ManMonth = d.IEstimate_ManDays;
                            QM.IEstimate_ManMonth = d.IEstimate_ManMonth;
                            QM.IDistance = d.IDistance;
                            QM.IEstimatedAmount = d.IEstimatedAmount;
                            QM.Icurrency = d.InternationalCurrency;
                            QM.IExchangeRate = d.IExchangeRate;
                            QM.ITotalAmount = d.ITotalAmount;
                            QM.PK_QTID = QuotationID;
                            QM.Type = "I";
                            Result = objDALQuotationMast.InsertUpdateIOrderType(QM);
                        }
                    }
               
                    #endregion
                    QM.QuotationNumber = Convert.ToString(Session["QuotationNumber"]);
                    if (QuotationID != 0 && QuotationID != null)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = objDALQuotationMast.InsertFileAttachment(lstFileDtls, QuotationID);
                            Session["listQuotationUploadedFile"] = null;

                        }
                        
                        return Json(new { result = "Redirect", url = Url.Action("Quotation", "QuotationMaster", new { @QuotationNumber = QM.QuotationNumber, @PK_QM_ID = QuotationID }) });
                    }
                    return Json(new { success = 4, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);

                    // return Json(new { success = 1, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(new { success = 4, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TemporaryFilePathQuotationAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment

        {

            var IPath = string.Empty;

            string[] splitedGrp;

            List<string> Selected = new List<string>();

            //Adding New Code 12 March 2020

            List<FileDetails> fileDetails = new List<FileDetails>();

            //---Adding end Code

            if (Session["listQuotationUploadedFile"] != null)

            {

                fileDetails = Session["listQuotationUploadedFile"] as List<FileDetails>;

            }



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

                            //FileDetails fileDetail = new FileDetails();

                            //fileDetail.FileName = fileName;

                            //fileDetail.Extension = Path.GetExtension(fileName);

                            //fileDetail.Id = Guid.NewGuid();

                            //fileDetails.Add(fileDetail);

                            FileDetails fileDetail = new FileDetails();

                            fileDetail.FileName = fileName;

                            fileDetail.Extension = Path.GetExtension(fileName);

                            fileDetail.Id = Guid.NewGuid();

                            BinaryReader br = new BinaryReader(files.InputStream);
                            byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                            fileDetail.FileContent = bytes;

                            fileDetails.Add(fileDetail);                            

                            filePath = Path.Combine(Server.MapPath("~/Files/QuotationAttachment/"), fileDetail.Id + fileDetail.Extension);

                            var K = "~/Files/QuotationAttachment/" + fileName;

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

                            ViewBag.Error = "Please Select XLSX or PDF Image File";

                        }

                    }



                }

                Session["listQuotationUploadedFile"] = fileDetails;



            }

            catch (Exception ex)

            {

                string Error = ex.Message.ToString();

            }

            return Json(IPath, JsonRequestBehavior.AllowGet);

        }
        public ActionResult QuotationDetails(QuotationMaster QM)
        {
            QM.QuotationNumber = Convert.ToString(Session["QuotationNumber"]);
            int PKQTID = Convert.ToInt32(Session["QTID"]);
            if (PKQTID == 0)
            {
                QM.PK_QTID = QM.PK_QTID;
            }
            else
            {
                QM.PK_QTID = PKQTID;
            }
            if (QM.QuotationNumber != "" && QM.QuotationNumber != null || QM.PK_QTID > 0 && QM.PK_QTID != null)
            {
                DataTable DTPrintQuotationDtls = new DataTable();
                DTPrintQuotationDtls = objDALQuotationMast.GetPrintQuotationDtls(QM);
                if (DTPrintQuotationDtls.Rows.Count > 0)
                {
                    ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EnquiryDescription"]);
                    ObjModelQuotationMast.PK_QTID = Convert.ToInt32(DTPrintQuotationDtls.Rows[0]["PK_QTID"]);
                    ObjModelQuotationMast.QuotationNumber = Convert.ToString(DTPrintQuotationDtls.Rows[0]["QuotationNumber"]);
                    ObjModelQuotationMast.EQ_ID = Convert.ToInt32(DTPrintQuotationDtls.Rows[0]["EQ_ID"]);
                    ObjModelQuotationMast.Associates = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Associates"]);

                    ObjModelQuotationMast.QuotationCompanyName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.ContactName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ContactName"]);
                    ObjModelQuotationMast.Email = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Email"]);
                    ObjModelQuotationMast.Mobile = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Mobile"]);
                    ObjModelQuotationMast.Landline = Convert.ToString(DTPrintQuotationDtls.Rows[0]["HomePhone"]);
                    ObjModelQuotationMast.BranchName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["BranchName"]);

                    ObjModelQuotationMast.ServType = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Name"]);
                    ObjModelQuotationMast.ProjectName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ProjectName"]);
                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Enquiry"]);
                    //ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Quotation_Description"]);
                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EndCustomer"]);
                    ObjModelQuotationMast.Reference = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Reference"]);
                    ViewBag.CreatedDate = Convert.ToDateTime(DTPrintQuotationDtls.Rows[0]["ExpiryDate"]).ToString("dd/MM/yyyy");
                    ObjModelQuotationMast.CreatedDate = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ExpiryDate"]); //Today Change 3 March 2020 Manoj Sharma
                    ObjModelQuotationMast.HeaderDetails = Convert.ToString(DTPrintQuotationDtls.Rows[0]["HeaderDetails"]);
                    ObjModelQuotationMast.Subject = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Subject"]);
                    ObjModelQuotationMast.Deliverable = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Deliverable"]);
                    ObjModelQuotationMast.Commercials = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Commercials"]);
                    ObjModelQuotationMast.FeesStructure = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FeesStructure"]);
                    ObjModelQuotationMast.PaymentTerms = Convert.ToString(DTPrintQuotationDtls.Rows[0]["PaymentTerms"]);

                    ObjModelQuotationMast.KeyNotes = Convert.ToString(DTPrintQuotationDtls.Rows[0]["KeyNotes"]);
                    ObjModelQuotationMast.AddEnclosures = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AddEnclosures"]);
                    ObjModelQuotationMast.To = Convert.ToString(DTPrintQuotationDtls.Rows[0]["T_O"]);
                    ObjModelQuotationMast.CC = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CC"]);
                    ObjModelQuotationMast.CommunicationProtocol = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CommunicationProtocol"]);
                    ObjModelQuotationMast.Coordinators = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Coordinators"]);
                    ObjModelQuotationMast.EscalationMatrix = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EscalationMatrix"]);
                    ObjModelQuotationMast.FaitFully = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FaithFully"]);

                    ObjModelQuotationMast.Validity = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Validity"]);
                    ObjModelQuotationMast.CompanyAddress = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CompanyAddress"]);
                    ObjModelQuotationMast.FromAddress = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FromAddress"]);
                    ObjModelQuotationMast.ScopeOfWork = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ScopeOfWork"]);
                    ObjModelQuotationMast.Ref = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EnquiryReferenceNo"]);
                    ObjModelQuotationMast.ThirdPartyInspectionService = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ThirdPartyInspectionService"]);
                    ObjModelQuotationMast.ThankYouLetter = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ThankYouLetter"]);
                }
                return View(ObjModelQuotationMast);
            }
            return View();
        }


        public ActionResult QuotationDetailsGeneratePDF(QuotationMaster QM)
        {
            //Array a = new Array;//ObjModelQuotationMast.Quotation_Description;
            QM.QuotationNumber = Convert.ToString(Session["QuotationNumber"]);
            int PKQTID = Convert.ToInt32(Session["QTID"]);
            if (PKQTID == 0)
            {
                QM.PK_QTID = QM.PK_QTID;
            }
            else
            {
                QM.PK_QTID = PKQTID;
            }
            if (QM.QuotationNumber != "" && QM.QuotationNumber != null || QM.PK_QTID > 0 && QM.PK_QTID != null)
            {
                DataTable DTPrintQuotationDtls = new DataTable();
                DTPrintQuotationDtls = objDALQuotationMast.GetPrintQuotationDtls(QM);
                if (DTPrintQuotationDtls.Rows.Count > 0)
                {
                    ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EnquiryDescription"]);
                    ObjModelQuotationMast.PK_QTID = Convert.ToInt32(DTPrintQuotationDtls.Rows[0]["PK_QTID"]);
                    ObjModelQuotationMast.QuotationNumber = Convert.ToString(DTPrintQuotationDtls.Rows[0]["QuotationNumber"]);
                    ObjModelQuotationMast.EQ_ID = Convert.ToInt32(DTPrintQuotationDtls.Rows[0]["EQ_ID"]);
                    ObjModelQuotationMast.Associates = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Associates"]);

                    ObjModelQuotationMast.QuotationCompanyName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.ContactName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ContactName"]);
                    ObjModelQuotationMast.Email = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Email"]);
                    ObjModelQuotationMast.Mobile = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Mobile"]);
                    ObjModelQuotationMast.Landline = Convert.ToString(DTPrintQuotationDtls.Rows[0]["HomePhone"]);
                    ObjModelQuotationMast.BranchName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["BranchName"]);

                    ObjModelQuotationMast.ServType = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Name"]);
                    ObjModelQuotationMast.ProjectName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ProjectName"]);
                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Enquiry"]);
                    //ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Quotation_Description"]);
                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EndCustomer"]);
                    ObjModelQuotationMast.Reference = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Reference"]);
                    ViewBag.CreatedDate = Convert.ToDateTime(DTPrintQuotationDtls.Rows[0]["ExpiryDate"]).ToString("dd/MM/yyyy");
                    ObjModelQuotationMast.CreatedDate = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ExpiryDate"]); //Today Change 3 March 2020 Manoj Sharma
                    ObjModelQuotationMast.HeaderDetails = Convert.ToString(DTPrintQuotationDtls.Rows[0]["HeaderDetails"]);
                    ObjModelQuotationMast.Subject = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Subject"]);
                    ObjModelQuotationMast.Deliverable = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Deliverable"]);
                    ObjModelQuotationMast.Commercials = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Commercials"]);
                    ObjModelQuotationMast.FeesStructure = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FeesStructure"]);
                    ObjModelQuotationMast.PaymentTerms = Convert.ToString(DTPrintQuotationDtls.Rows[0]["PaymentTerms"]);

                    ObjModelQuotationMast.KeyNotes = Convert.ToString(DTPrintQuotationDtls.Rows[0]["KeyNotes"]);
                    ObjModelQuotationMast.AddEnclosures = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AddEnclosures"]);
                    ObjModelQuotationMast.To = Convert.ToString(DTPrintQuotationDtls.Rows[0]["T_O"]);
                    ObjModelQuotationMast.CC = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CC"]);
                    ObjModelQuotationMast.CommunicationProtocol = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CommunicationProtocol"]);
                    ObjModelQuotationMast.Coordinators = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Coordinators"]);
                    ObjModelQuotationMast.EscalationMatrix = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EscalationMatrix"]);
                    ObjModelQuotationMast.FaitFully = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FaithFully"]);

                    ObjModelQuotationMast.Validity = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Validity"]);
                    ObjModelQuotationMast.CompanyAddress = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CompanyAddress"]);
                    ObjModelQuotationMast.FromAddress = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FromAddr"]);
                    ObjModelQuotationMast.ScopeOfWork = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ScopeOfWork"]);
                    ObjModelQuotationMast.Ref = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EnquiryReferenceNo"]);
                    ObjModelQuotationMast.ThirdPartyInspectionService = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ThirdPartyInspectionService"]);
                    ObjModelQuotationMast.ThankYouLetter = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ThankYouLetter"]);
                    ObjModelQuotationMast.FromAddress = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FromAddr"]);
                    ObjModelQuotationMast.Mobile = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Mobile"]);
                    ObjModelQuotationMast.Designation = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Designation"]);
                    ObjModelQuotationMast.AddEnclosures = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AddEnclosures"]);
                    ObjModelQuotationMast.Signature = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Signature"]);
                    ObjModelQuotationMast.CreatedDate = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CreatedDate"]);
                    ObjModelQuotationMast.CostSheetApproveStatus = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CostSheetApproveStatus"]);
                    ObjModelQuotationMast.Exclusion = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Exclusion"]);
                    ObjModelQuotationMast.IData = (byte[])(DTPrintQuotationDtls.Rows[0]["PDFImage"]);

                    ObjModelQuotationMast.QuotationCreatedName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Name"]);
                    ObjModelQuotationMast.QuotationCreatedDesignation = Convert.ToString(DTPrintQuotationDtls.Rows[0]["QuotationCreatedDesignation"]);
                    ObjModelQuotationMast.QuotationCreatedMobile = Convert.ToString(DTPrintQuotationDtls.Rows[0]["MobileNo"]);
                    ObjModelQuotationMast.QuotationCreatedEmail = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EmailID"]);
                    ObjModelQuotationMast.QuotationCreatedLandline = Convert.ToString(DTPrintQuotationDtls.Rows[0]["LandLine"]);
                    //ObjModelQuotationMast.AssociatesAddr = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AssociatesAddr"]);
                    ObjModelQuotationMast.AssociatesAddr = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CompanyAddressPDF"]);
                    ObjModelQuotationMast.AssociatesEmail = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AssociatesEmail"]);
                    ObjModelQuotationMast.AssociatesMobile = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AssociatesMobile"]);
                    ObjModelQuotationMast.SubServiceType = Convert.ToString(DTPrintQuotationDtls.Rows[0]["SubServiceType"]);
                    ObjModelQuotationMast.EnquiryAdditionRef = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EnquiryAdditionRef"]);
                    ObjModelQuotationMast.ComplimentaryClose = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ComplimentaryClose"]);
                    ObjModelQuotationMast.ReviseNoForPDF = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ReviseNoForPDF"]);
                    ObjModelQuotationMast.SupersadesOfQForPDF = Convert.ToString(DTPrintQuotationDtls.Rows[0]["SupersadesOfQForPDF"]);
                    ObjModelQuotationMast.ReviseReason = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ReviseReason"]);
                    ObjModelQuotationMast.PreviousQuotationReviseDate = Convert.ToString(DTPrintQuotationDtls.Rows[0]["PreviousQuotationReviseDate"]);
                    ObjModelQuotationMast.AutoA = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AutoA"]);

                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EndCustomer"]);

                    if (Convert.ToString(DTPrintQuotationDtls.Rows[0]["GeneralTermsCheckbox"]) == "1")
                    {
                        ObjModelQuotationMast.GeneralTermsCheckbox = true;
                    }
                    else
                    {
                        ObjModelQuotationMast.GeneralTermsCheckbox = false;
                    }
                    ObjModelQuotationMast.GeneralTerms = Convert.ToString(DTPrintQuotationDtls.Rows[0]["GeneralTerms"]);

                    if (Convert.ToInt32(ObjModelQuotationMast.EstimatedAmount) >= 1000000)
                    {
                        DataTable DTCLEditQuotationMast = new DataTable();
                        DTCLEditQuotationMast = objDALQuotationMast.GetCLApprovalStatus(QM.PK_QTID);
                        ObjModelQuotationMast.CostSheetCLStatus = DTCLEditQuotationMast.Rows[0]["CLStatus"].ToString();

                    }

                    #region Generate Link for pdf
                    // string URl = "a "+ a + a" ;//"http://localhost:54895/QuotationServicesLink/QuotationServicesLink?PKServiceId= " +aa+";//+  "ObjModelQuotationMast.SubServiceType" + ";
                    //string URl = "http://localhost:54895/QuotationServicesLink/Servicess?PKServiceId=" + ObjModelQuotationMast.SubServiceType + "";
                    string URl = "https://tiimes.tuv-india.com/QuotationServicesLink/Servicess?PKServiceId=" + ObjModelQuotationMast.SubServiceType + "";

                    #endregion

                    #region Generate PDF
                    SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
                    System.Text.StringBuilder strs = new System.Text.StringBuilder();
                    string body = string.Empty;
                    string ProjectName = "";
                    string ReferenceDocumentscontent = "";


                    using (StreamReader reader = new StreamReader(Server.MapPath("~/QuotationHtml/quotation-contentarea.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    if (ObjModelQuotationMast.Quotation_Description == null || ObjModelQuotationMast.Quotation_Description == "")
                    {
                        body = body.Replace("[ProjectName]", "");
                    }
                    else
                    {
                        body = body.Replace("[ProjectName]", "Project Name :" + ObjModelQuotationMast.Quotation_Description);
                    }
                    body = body.Replace("[URL]", URl);
                    body = body.Replace("[QuotationCompanyName]", ObjModelQuotationMast.QuotationCompanyName);
                    body = body.Replace("[Associates]", ObjModelQuotationMast.Associates);
                    body = body.Replace("[QuotationNumber]", ObjModelQuotationMast.QuotationNumber);
                    body = body.Replace("[CreatedDate]", Convert.ToString(ObjModelQuotationMast.CreatedDate));
                    body = body.Replace("[ThirdPartyInspectionService]", Convert.ToString(ObjModelQuotationMast.ThirdPartyInspectionService));
                    body = body.Replace("[ContactName]", ObjModelQuotationMast.ContactName);
                    body = body.Replace("[Email]", ObjModelQuotationMast.Email);
                    body = body.Replace("[Mobile]", ObjModelQuotationMast.Mobile);
                    body = body.Replace("[Subject]", Convert.ToString(ObjModelQuotationMast.Subject));
                    body = body.Replace("[Ref]", ObjModelQuotationMast.Ref);
                    body = body.Replace("[ThankYouLetter]", ObjModelQuotationMast.ThankYouLetter);
                    string s = ObjModelQuotationMast.ScopeOfWork;
                    body = body.Replace("[ScopeOfWork]", ObjModelQuotationMast.ScopeOfWork.Replace("<p>", "").Replace("</p>", ""));
                    body = body.Replace("[Deliverable]", ObjModelQuotationMast.Deliverable.Replace("<p>", "").Replace("</p>", ""));
                    body = body.Replace("[CommunicationProtocol]", ObjModelQuotationMast.CommunicationProtocol.Replace("<p>", "").Replace("</p>", ""));
                    body = body.Replace("[FeesStructure]", ObjModelQuotationMast.FeesStructure.Replace("<p>", "").Replace("</p>", ""));
                    body = body.Replace("[Validity]", ObjModelQuotationMast.Validity);
                    body = body.Replace("[FaitFully]", ObjModelQuotationMast.FaitFully);
                    body = body.Replace("[PaymentTerms]", ObjModelQuotationMast.PaymentTerms.Replace("<p>", "").Replace("</p>", ""));
                    body = body.Replace("[FromAddress]", ObjModelQuotationMast.FromAddress);
                    body = body.Replace("[stamp]", "https://tiimes.tuv-india.com/QuotationHtml/stump-sign.png");
                    body = body.Replace("[QuCode]", "https://tiimes.tuv-india.com/QuotationHtml/qrcode.png");
                    body = body.Replace("[Mobile]", ObjModelQuotationMast.Mobile);
                    body = body.Replace("[Designation]", ObjModelQuotationMast.Designation);
                    body = body.Replace("[AddEnclosures]", ObjModelQuotationMast.AddEnclosures);

                    body = body.Replace("[QuotationCreatedName]", ObjModelQuotationMast.QuotationCreatedName);
                    body = body.Replace("[QuotationCreatedDesignation]", ObjModelQuotationMast.QuotationCreatedDesignation);
                    body = body.Replace("[QuotationCreatedMobile]", ObjModelQuotationMast.QuotationCreatedMobile);
                    body = body.Replace("[QuotationCreatedEmail]", ObjModelQuotationMast.QuotationCreatedEmail);
                    body = body.Replace("[QuotationCreatedLandline]", ObjModelQuotationMast.QuotationCreatedLandline);
                    body = body.Replace("[AssociatesAddr]", ObjModelQuotationMast.AssociatesAddr);
                    body = body.Replace("[AssociatesMobile]", ObjModelQuotationMast.AssociatesMobile);
                    body = body.Replace("[AssociatesEmail]", ObjModelQuotationMast.AssociatesEmail);
                    body = body.Replace("[ComplimentaryClose]", ObjModelQuotationMast.ComplimentaryClose);


                    //<p><span><strong> Revision Number </strong> (If Applicable)  [[ReviseNoForPDF]] : [ReviseReason]</span></p>
                    //<span><strong>This Document Supersedes Quotation No. :</strong>[SupersadesOfQForPDF]</span>

                    string strReviseNoForPDF = "<p style='margin-bottom:3px;'><span><strong> Revision Number </strong> [" + ObjModelQuotationMast.ReviseNoForPDF + "] Reason for revision - " + ObjModelQuotationMast.ReviseReason + "</strong></span></p>" + " ";
                    string strSupersadesOfQForPDF = "<span><strong>This Document Supersedes Quotation No. :</strong>" + ObjModelQuotationMast.SupersadesOfQForPDF + " Dated : " + ObjModelQuotationMast.PreviousQuotationReviseDate + "</span>";

                    if (ObjModelQuotationMast.ReviseNoForPDF == null || ObjModelQuotationMast.ReviseNoForPDF == "")
                    {
                        body = body.Replace("[ReviseNoForPDF]", "");
                        body = body.Replace("[SupersadesOfQForPDF]", "");
                    }
                    else
                    {
                        body = body.Replace("[ReviseNoForPDF]", strReviseNoForPDF);
                        body = body.Replace("[SupersadesOfQForPDF]", strSupersadesOfQForPDF);
                    }



                    body = body.Replace("[ReviseReason]", ObjModelQuotationMast.ReviseReason);


                    //string I = "<img src = 'https://tiimes.tuv-india.com/Content/Sign/" + ObjModelQuotationMast.Signature + "' style='width:100px;height:50px; ' align='center'>";
                    string I = "<img src = 'https://tiimes.tuv-india.com/Content/Sign/" + ObjModelQuotationMast.Signature + "' style='width:100px;height:50px; ' align='center'>";


                    body = body.Replace("[Signature]", I);

                    body = body.Replace("[CreatedDate]", Convert.ToString(ObjModelQuotationMast.CreatedDate));
                    body = body.Replace("[EnquiryAdditionRef]", Convert.ToString(ObjModelQuotationMast.EnquiryAdditionRef));



                    int ExclusionCheckBox = Convert.ToInt32(DTPrintQuotationDtls.Rows[0]["ExclusionCheckBox"]);
                    ObjModelQuotationMast.ExclusionCheckBox = Convert.ToBoolean(ExclusionCheckBox);

                    if (ObjModelQuotationMast.ExclusionCheckBox == true)
                    {
                        // string strE = "<tr><td align='left'> Exclusion :" + ObjModelQuotationMast.Exclusion + " </td></tr>";
                        //string strE = "<tr><td align='left'><p style='font - size: 18px; text - align: justify; font - family:Arial; margin-bottom: 3px;'><strong><u> Exclusions:</u></strong></p>" + "</br>" + ObjModelQuotationMast.Exclusion + " </td></tr>";
                        //string strE = "<tr><td align='left'><p style='font - size: 14px; text - align: justify; font - family:Arial; margin-bottom: 3px;'><strong><u> Exclusions:</u></strong></p><span style='white-space: pre-line;'>" + ObjModelQuotationMast.Exclusion + "</span>" + " </td></tr>";
                        string strE = "<tr><td align='left'><p style='font - size: 18px; text - align: justify; font - family:Arial; margin-bottom: 3px;'><strong><label style='font-size: 18px;'><u style='font - size: 18px;'> Exclusions:</u></label></strong></p><span style='white-space: pre-line;'>" + ObjModelQuotationMast.Exclusion + "</span>" + " </td></tr>";
                        body = body.Replace("[Exclusion]", strE);
                    }
                    else
                    {
                        body = body.Replace("[Exclusion]", "");
                    }

                    if (ObjModelQuotationMast.GeneralTermsCheckbox == true)
                    {
                        body = body.Replace("[GeneralTerms]", ObjModelQuotationMast.GeneralTerms);
                    }
                    else
                    {
                        body = body.Replace("[GeneralTerms]", "");
                    }
                    if (ObjModelQuotationMast.EndCustomer == null || ObjModelQuotationMast.EndCustomer == "")
                    {
                        body = body.Replace("[EndCustomer]", "");
                    }
                    else
                    {
                        body = body.Replace("[EndCustomer]", "End Customer :" + ObjModelQuotationMast.EndCustomer);
                    }



                    string byteData2 = Convert.ToBase64String(ObjModelQuotationMast.IData);
                    string a3 = "<img src = 'data:image/jpg;base64," + byteData2 + " ' style='width:100%;height:400px;' />";
                    body = body.Replace("[QuotationFirstPageImage1]", a3);

                    strs.Append(body);
                    PdfPageSize pageSize = PdfPageSize.A4;
                    PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
                    HtmlToPdf converter = new HtmlToPdf();


                    #region Count Page No
                    SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(body);
                    int PageCount = doc1.Pages.Count;
                    body = body.Replace("[PageCount]", ObjModelQuotationMast.AddEnclosures + ' ' + "(Refer Page " + Convert.ToString(PageCount) + " Of " + Convert.ToString(PageCount) + " )");
                    strs.Append(body);
                    #endregion



                    // set the page timeout (in seconds)
                    converter.Options.MaxPageLoadTime = 240;  //=========================5-Aug-2019
                    converter.Options.PdfPageSize = pageSize;
                    converter.Options.PdfPageOrientation = pdfOrientation;

                    string _Header = string.Empty;
                    string _footer = string.Empty;


                    StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/QuotationHtml/quotation-header.html"));
                    _Header = _readHeader_File.ReadToEnd();
                    //_Header = _Header.Replace("[logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png");
                    _Header = _Header.Replace("[logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.svg");
                    _Header = _Header.Replace("[QuotationNumber]", ObjModelQuotationMast.QuotationNumber);
                    _Header = _Header.Replace("[Date]", Convert.ToString(ObjModelQuotationMast.CreatedDate));

                    StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/QuotationHtml/quotation-footer.html"));
                    _footer = _readFooter_File.ReadToEnd();
                    _footer = _footer.Replace("[LogoFooter]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/FTUEV-NORD-GROUP_Logo_Electric-Blue.svg");
                    // header settings
                    converter.Options.DisplayHeader = true ||
                        true || true;
                    converter.Header.DisplayOnFirstPage = true;
                    converter.Header.DisplayOnOddPages = true;
                    converter.Header.DisplayOnEvenPages = true;
                    //converter.Header.Height = 75;
                    //converter.Header.Height = 140;
                    converter.Header.Height = 100;

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
                    converter.Footer.Height = 70;

                    PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
                    footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    converter.Footer.Add(footerHtml);


                    #region Footer Code
                    ///  PdfTextSection text1 = new PdfTextSection(5, 30, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
                    PdfTextSection text1 = new PdfTextSection(30, 45, "Page: {page_number} of {total_pages}                    ", new System.Drawing.Font("TNG Pro", 8));
                    //text1.HorizontalAlign = PdfTextHorizontalAlign.Right;
                    //text1.VerticalAlign = PdfTextVerticalAlign.Bottom;
                    converter.Footer.Add(text1);




                    // page numbers can be added using a PdfTextSection object
                    //PdfTextSection text = new PdfTextSection(1, 145, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));

                    //text.HorizontalAlign = PdfTextHorizontalAlign.Center;
                    //converter.Footer.Add(text);

                    #endregion

                    if (ObjModelQuotationMast.CostSheetApproveStatus == "1")
                    {
                        converter.Options.SecurityOptions.CanAssembleDocument = false;
                        converter.Options.SecurityOptions.CanCopyContent = false;
                        converter.Options.SecurityOptions.CanEditAnnotations = false;
                        converter.Options.SecurityOptions.CanEditContent = false;
                        converter.Options.SecurityOptions.CanFillFormFields = false;
                        converter.Options.SecurityOptions.CanPrint = false;
                    }
                    else
                    {
                        converter.Options.SecurityOptions.CanAssembleDocument = true;
                        converter.Options.SecurityOptions.CanCopyContent = true;
                        converter.Options.SecurityOptions.CanEditAnnotations = true;
                        converter.Options.SecurityOptions.CanEditContent = true;
                        converter.Options.SecurityOptions.CanFillFormFields = true;
                        converter.Options.SecurityOptions.CanPrint = true;
                    }
                    PdfDocument doc = converter.ConvertHtmlString(body);

                    if (ObjModelQuotationMast.CostSheetApproveStatus == "1")
                    {
                        doc.Security.CanAssembleDocument = false;
                        doc.Security.CanCopyContent = false;
                        doc.Security.CanEditAnnotations = false;
                        doc.Security.CanEditContent = false;
                        doc.Security.CanFillFormFields = false;
                        doc.Security.CanPrint = false;
                    }
                    else
                    {
                        doc.Security.CanAssembleDocument = true;
                        doc.Security.CanCopyContent = true;
                        doc.Security.CanEditAnnotations = true;
                        doc.Security.CanEditContent = true;
                        doc.Security.CanFillFormFields = true;
                        doc.Security.CanPrint = true;
                    }


                    string ReportName = ObjModelQuotationMast.QuotationNumber + ".pdf";
                    string path = Server.MapPath("~/QuotationHtml");
                    //set document permissions
                    if (ObjModelQuotationMast.AutoA == "3")
                    {

                    }
                    else
                    {
                        if (ObjModelQuotationMast.CostSheetApproveStatus == "0")
                        {
                            string imgFile1 = Server.MapPath("/t6.png");
                            PdfTemplate template1 = doc.AddTemplate(doc.Pages[0].ClientRectangle);
                            //PdfImageElement img1 = new PdfImageElement(150, 50, imgFile1);
                            PdfImageElement img1 = new PdfImageElement(125, 85, imgFile1);
                            img1.Transparency = 15;
                            template1.Add(img1);
                        }
                        else
                        {

                        }
                    }

                    // get image path
                    ///  string imgFile = Server.MapPath("/t1.jpg");
                    //string imgFile = Server.MapPath("/t2.jpg");
                    // watermark all pages - add a template containing an image 
                    // to the bottom right of the page
                    // the image should repeat on all pdf pages automatically
                    // the template should be rendered behind the rest of the page elements
                    //for (int cnt = 0; cnt < doc.Pages.Count; cnt++)
                    //{
                    //    if (cnt > 0)
                    //    {
                    //        PdfTemplate template = doc.AddTemplate(doc.Pages[0].ClientRectangle); //// 635 * 554                         
                    //        PdfImageElement img = new PdfImageElement(125, 150, imgFile);
                    //        img.Transparency = 15;
                    //        //template.Add(img);
                    //        doc.Pages[cnt].Add(img); //// 635 * 554  
                    //    }


                    //}
                    doc.Save(path + '\\' + ReportName);
                    doc.Close();
                    string Result = "";
                    ObjModelQuotationMast.QuotationPDF = ReportName;
                    Result = objDALQuotationMast.InsertUpdateReport(ObjModelQuotationMast);
                    #endregion





                }
                //return View(ObjModelQuotationMast);


                string newpath = Server.MapPath("~/QuotationHtml/");

                byte[] fileBytes = System.IO.File.ReadAllBytes(newpath + @"\" + ObjModelQuotationMast.QuotationPDF);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, ObjModelQuotationMast.QuotationPDF);






                return RedirectToAction("Quotation", new { QuotationNumber = ObjModelQuotationMast.QuotationNumber, PK_QM_ID = ObjModelQuotationMast.PK_QTID });
            }
            return RedirectToAction("Quotation", new { QuotationNumber = ObjModelQuotationMast.QuotationNumber, PK_QM_ID = ObjModelQuotationMast.PK_QTID });
            // return View();
        }



        public void GeneratePDF(QuotationMaster QM)
        {
            QM.QuotationNumber = Convert.ToString(Session["QuotationNumber"]);
            int PKQTID = Convert.ToInt32(Session["QTID"]);
            if (PKQTID == 0)
            {
                QM.PK_QTID = QM.PK_QTID;
            }
            else
            {
                QM.PK_QTID = PKQTID;
            }
            if (QM.QuotationNumber != "" && QM.QuotationNumber != null || QM.PK_QTID > 0 && QM.PK_QTID != null)
            {
                DataTable DTPrintQuotationDtls = new DataTable();
                DTPrintQuotationDtls = objDALQuotationMast.GetPrintQuotationDtls(QM);
                if (DTPrintQuotationDtls.Rows.Count > 0)
                {
                    ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EnquiryDescription"]);
                    ObjModelQuotationMast.PK_QTID = Convert.ToInt32(DTPrintQuotationDtls.Rows[0]["PK_QTID"]);
                    ObjModelQuotationMast.QuotationNumber = Convert.ToString(DTPrintQuotationDtls.Rows[0]["QuotationNumber"]);
                    ObjModelQuotationMast.EQ_ID = Convert.ToInt32(DTPrintQuotationDtls.Rows[0]["EQ_ID"]);
                    ObjModelQuotationMast.Associates = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Associates"]);

                    ObjModelQuotationMast.QuotationCompanyName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.ContactName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ContactName"]);
                    ObjModelQuotationMast.Email = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Email"]);
                    ObjModelQuotationMast.Mobile = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Mobile"]);
                    ObjModelQuotationMast.Landline = Convert.ToString(DTPrintQuotationDtls.Rows[0]["HomePhone"]);
                    ObjModelQuotationMast.BranchName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["BranchName"]);

                    ObjModelQuotationMast.ServType = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Name"]);
                    ObjModelQuotationMast.ProjectName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ProjectName"]);
                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Enquiry"]);
                    //ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Quotation_Description"]);
                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EndCustomer"]);
                    ObjModelQuotationMast.Reference = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Reference"]);
                    ViewBag.CreatedDate = Convert.ToDateTime(DTPrintQuotationDtls.Rows[0]["ExpiryDate"]).ToString("dd/MM/yyyy");
                    ObjModelQuotationMast.CreatedDate = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ExpiryDate"]); //Today Change 3 March 2020 Manoj Sharma
                    ObjModelQuotationMast.HeaderDetails = Convert.ToString(DTPrintQuotationDtls.Rows[0]["HeaderDetails"]);
                    ObjModelQuotationMast.Subject = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Subject"]);
                    ObjModelQuotationMast.Deliverable = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Deliverable"]);
                    ObjModelQuotationMast.Commercials = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Commercials"]);
                    ObjModelQuotationMast.FeesStructure = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FeesStructure"]);
                    ObjModelQuotationMast.PaymentTerms = Convert.ToString(DTPrintQuotationDtls.Rows[0]["PaymentTerms"]);

                    ObjModelQuotationMast.KeyNotes = Convert.ToString(DTPrintQuotationDtls.Rows[0]["KeyNotes"]);
                    ObjModelQuotationMast.AddEnclosures = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AddEnclosures"]);
                    ObjModelQuotationMast.To = Convert.ToString(DTPrintQuotationDtls.Rows[0]["T_O"]);
                    ObjModelQuotationMast.CC = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CC"]);
                    ObjModelQuotationMast.CommunicationProtocol = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CommunicationProtocol"]);
                    ObjModelQuotationMast.Coordinators = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Coordinators"]);
                    ObjModelQuotationMast.EscalationMatrix = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EscalationMatrix"]);
                    ObjModelQuotationMast.FaitFully = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FaithFully"]);

                    ObjModelQuotationMast.Validity = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Validity"]);
                    ObjModelQuotationMast.CompanyAddress = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CompanyAddress"]);
                    ObjModelQuotationMast.FromAddress = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FromAddr"]);
                    ObjModelQuotationMast.ScopeOfWork = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ScopeOfWork"]);
                    ObjModelQuotationMast.Ref = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EnquiryReferenceNo"]);
                    ObjModelQuotationMast.ThirdPartyInspectionService = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ThirdPartyInspectionService"]);
                    ObjModelQuotationMast.ThankYouLetter = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ThankYouLetter"]);
                    ObjModelQuotationMast.FromAddress = Convert.ToString(DTPrintQuotationDtls.Rows[0]["FromAddr"]);
                    ObjModelQuotationMast.Mobile = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Mobile"]);
                    ObjModelQuotationMast.Designation = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Designation"]);
                    ObjModelQuotationMast.AddEnclosures = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AddEnclosures"]);
                    ObjModelQuotationMast.Signature = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Signature"]);
                    ObjModelQuotationMast.CreatedDate = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CreatedDate"]);
                    ObjModelQuotationMast.CostSheetApproveStatus = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CostSheetApproveStatus"]);
                    ObjModelQuotationMast.Exclusion = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Exclusion"]);
                    ObjModelQuotationMast.IData = (byte[])(DTPrintQuotationDtls.Rows[0]["PDFImage"]);

                    ObjModelQuotationMast.QuotationCreatedName = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Name"]);
                    ObjModelQuotationMast.QuotationCreatedDesignation = Convert.ToString(DTPrintQuotationDtls.Rows[0]["QuotationCreatedDesignation"]);
                    ObjModelQuotationMast.QuotationCreatedMobile = Convert.ToString(DTPrintQuotationDtls.Rows[0]["MobileNo"]);
                    ObjModelQuotationMast.QuotationCreatedEmail = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EmailID"]);
                    ObjModelQuotationMast.QuotationCreatedLandline = Convert.ToString(DTPrintQuotationDtls.Rows[0]["LandLine"]);
                    //ObjModelQuotationMast.AssociatesAddr = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AssociatesAddr"]);
                    ObjModelQuotationMast.AssociatesAddr = Convert.ToString(DTPrintQuotationDtls.Rows[0]["CompanyAddressPDF"]);
                    ObjModelQuotationMast.AssociatesEmail = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AssociatesEmail"]);
                    ObjModelQuotationMast.AssociatesMobile = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AssociatesMobile"]);
                    ObjModelQuotationMast.SubServiceType = Convert.ToString(DTPrintQuotationDtls.Rows[0]["SubServiceType"]);
                    ObjModelQuotationMast.EnquiryAdditionRef = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EnquiryAdditionRef"]);
                    ObjModelQuotationMast.ComplimentaryClose = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ComplimentaryClose"]);
                    ObjModelQuotationMast.ReviseNoForPDF = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ReviseNoForPDF"]);
                    ObjModelQuotationMast.SupersadesOfQForPDF = Convert.ToString(DTPrintQuotationDtls.Rows[0]["SupersadesOfQForPDF"]);
                    ObjModelQuotationMast.ReviseReason = Convert.ToString(DTPrintQuotationDtls.Rows[0]["ReviseReason"]);
                    ObjModelQuotationMast.PreviousQuotationReviseDate = Convert.ToString(DTPrintQuotationDtls.Rows[0]["PreviousQuotationReviseDate"]);
                    ObjModelQuotationMast.AutoA = Convert.ToString(DTPrintQuotationDtls.Rows[0]["AutoA"]);

                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTPrintQuotationDtls.Rows[0]["EndCustomer"]);

                    if (Convert.ToString(DTPrintQuotationDtls.Rows[0]["GeneralTermsCheckbox"]) == "1")
                    {
                        ObjModelQuotationMast.GeneralTermsCheckbox = true;
                    }
                    else
                    {
                        ObjModelQuotationMast.GeneralTermsCheckbox = false;
                    }
                    ObjModelQuotationMast.GeneralTerms = Convert.ToString(DTPrintQuotationDtls.Rows[0]["GeneralTerms"]);

                    if (Convert.ToInt32(ObjModelQuotationMast.EstimatedAmount) >= 1000000)
                    {
                        DataTable DTCLEditQuotationMast = new DataTable();
                        DTCLEditQuotationMast = objDALQuotationMast.GetCLApprovalStatus(QM.PK_QTID);
                        ObjModelQuotationMast.CostSheetCLStatus = DTCLEditQuotationMast.Rows[0]["CLStatus"].ToString();

                    }

                    #region Generate Link for pdf
                    // string URl = "a "+ a + a" ;//"http://localhost:54895/QuotationServicesLink/QuotationServicesLink?PKServiceId= " +aa+";//+  "ObjModelQuotationMast.SubServiceType" + ";
                    //string URl = "http://localhost:54895/QuotationServicesLink/Servicess?PKServiceId=" + ObjModelQuotationMast.SubServiceType + "";
                    string URl = "https://tiimes.tuv-india.com/QuotationServicesLink/Servicess?PKServiceId=" + ObjModelQuotationMast.SubServiceType + "";

                    #endregion

                    #region Generate PDF
                    SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
                    System.Text.StringBuilder strs = new System.Text.StringBuilder();
                    string body = string.Empty;
                    string ProjectName = "";
                    string ReferenceDocumentscontent = "";


                    using (StreamReader reader = new StreamReader(Server.MapPath("~/QuotationHtml/quotation-contentarea.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    if (ObjModelQuotationMast.Quotation_Description == null || ObjModelQuotationMast.Quotation_Description == "")
                    {
                        body = body.Replace("[ProjectName]", "");
                    }
                    else
                    {
                        body = body.Replace("[ProjectName]", "Project Name :" + ObjModelQuotationMast.Quotation_Description);
                    }
                    body = body.Replace("[URL]", URl);
                    body = body.Replace("[QuotationCompanyName]", ObjModelQuotationMast.QuotationCompanyName);
                    body = body.Replace("[Associates]", ObjModelQuotationMast.Associates);
                    body = body.Replace("[QuotationNumber]", ObjModelQuotationMast.QuotationNumber);
                    body = body.Replace("[CreatedDate]", Convert.ToString(ObjModelQuotationMast.CreatedDate));
                    body = body.Replace("[ThirdPartyInspectionService]", Convert.ToString(ObjModelQuotationMast.ThirdPartyInspectionService));
                    body = body.Replace("[ContactName]", ObjModelQuotationMast.ContactName);
                    body = body.Replace("[Email]", ObjModelQuotationMast.Email);
                    body = body.Replace("[Mobile]", ObjModelQuotationMast.Mobile);
                    body = body.Replace("[Subject]", Convert.ToString(ObjModelQuotationMast.Subject));
                    body = body.Replace("[Ref]", ObjModelQuotationMast.Ref);
                    body = body.Replace("[ThankYouLetter]", ObjModelQuotationMast.ThankYouLetter);
                    string s = ObjModelQuotationMast.ScopeOfWork;
                    body = body.Replace("[ScopeOfWork]", ObjModelQuotationMast.ScopeOfWork.Replace("<p>", "").Replace("</p>", ""));
                    body = body.Replace("[Deliverable]", ObjModelQuotationMast.Deliverable.Replace("<p>", "").Replace("</p>", ""));
                    body = body.Replace("[CommunicationProtocol]", ObjModelQuotationMast.CommunicationProtocol.Replace("<p>", "").Replace("</p>", ""));
                    //string pattern = @"<br\s*/>\s*(<br\s*/>\s*)+";
                    //body = Regex.Replace(body, pattern, "<br />");
                    body = body.Replace("[FeesStructure]", ObjModelQuotationMast.FeesStructure.Replace("<p>", "").Replace("</p>", ""));
                    body = body.Replace("[Validity]", ObjModelQuotationMast.Validity);
                    body = body.Replace("[FaitFully]", ObjModelQuotationMast.FaitFully);
                    body = body.Replace("[PaymentTerms]", ObjModelQuotationMast.PaymentTerms.Replace("<p>", "").Replace("</p>", ""));
                    body = body.Replace("[FromAddress]", ObjModelQuotationMast.FromAddress);
                    body = body.Replace("[stamp]", "https://tiimes.tuv-india.com/QuotationHtml/stump-sign.png");
                    body = body.Replace("[QuCode]", "https://tiimes.tuv-india.com/QuotationHtml/qrcode.png");
                    body = body.Replace("[Mobile]", ObjModelQuotationMast.Mobile);
                    body = body.Replace("[Designation]", ObjModelQuotationMast.Designation);
                    body = body.Replace("[AddEnclosures]", ObjModelQuotationMast.AddEnclosures);

                    body = body.Replace("[QuotationCreatedName]", ObjModelQuotationMast.QuotationCreatedName);
                    body = body.Replace("[QuotationCreatedDesignation]", ObjModelQuotationMast.QuotationCreatedDesignation);
                    body = body.Replace("[QuotationCreatedMobile]", ObjModelQuotationMast.QuotationCreatedMobile);
                    body = body.Replace("[QuotationCreatedEmail]", ObjModelQuotationMast.QuotationCreatedEmail);
                    body = body.Replace("[QuotationCreatedLandline]", ObjModelQuotationMast.QuotationCreatedLandline);
                    body = body.Replace("[AssociatesAddr]", ObjModelQuotationMast.AssociatesAddr);
                    body = body.Replace("[AssociatesMobile]", ObjModelQuotationMast.AssociatesMobile);
                    body = body.Replace("[AssociatesEmail]", ObjModelQuotationMast.AssociatesEmail);
                    body = body.Replace("[ComplimentaryClose]", ObjModelQuotationMast.ComplimentaryClose);


                    //<p><span><strong> Revision Number </strong> (If Applicable)  [[ReviseNoForPDF]] : [ReviseReason]</span></p>
                    //<span><strong>This Document Supersedes Quotation No. :</strong>[SupersadesOfQForPDF]</span>

                    string strReviseNoForPDF = "<p style='margin-bottom:3px;'><span><strong> Revision Number </strong> [" + ObjModelQuotationMast.ReviseNoForPDF + "] Reason for revision - " + ObjModelQuotationMast.ReviseReason + "</strong></span></p>" + " ";
                    string strSupersadesOfQForPDF = "<span><strong>This Document Supersedes Quotation No. :</strong>" + ObjModelQuotationMast.SupersadesOfQForPDF + " Dated : " + ObjModelQuotationMast.PreviousQuotationReviseDate + "</span>";

                    if (ObjModelQuotationMast.ReviseNoForPDF == null || ObjModelQuotationMast.ReviseNoForPDF == "")
                    {
                        body = body.Replace("[ReviseNoForPDF]", "");
                        body = body.Replace("[SupersadesOfQForPDF]", "");
                    }
                    else
                    {
                        body = body.Replace("[ReviseNoForPDF]", strReviseNoForPDF);
                        body = body.Replace("[SupersadesOfQForPDF]", strSupersadesOfQForPDF);
                    }



                    body = body.Replace("[ReviseReason]", ObjModelQuotationMast.ReviseReason);


                    //string I = "<img src = 'https://tiimes.tuv-india.com/Content/Sign/" + ObjModelQuotationMast.Signature + "' style='width:100px;height:50px; ' align='center'>";
                    string I = "<img src = 'https://tiimes.tuv-india.com/Content/Sign/" + ObjModelQuotationMast.Signature + "' style='width:100px;height:50px; ' align='center'>";


                    body = body.Replace("[Signature]", I);

                    body = body.Replace("[CreatedDate]", Convert.ToString(ObjModelQuotationMast.CreatedDate));
                    body = body.Replace("[EnquiryAdditionRef]", Convert.ToString(ObjModelQuotationMast.EnquiryAdditionRef));



                    int ExclusionCheckBox = Convert.ToInt32(DTPrintQuotationDtls.Rows[0]["ExclusionCheckBox"]);
                    ObjModelQuotationMast.ExclusionCheckBox = Convert.ToBoolean(ExclusionCheckBox);

                    if (ObjModelQuotationMast.ExclusionCheckBox == true)
                    {
                        // string strE = "<tr><td align='left'> Exclusion :" + ObjModelQuotationMast.Exclusion + " </td></tr>";
                        //string strE = "<tr><td align='left'><p style='font - size: 18px; text - align: justify; font - family:Arial; margin-bottom: 3px;'><strong><u> Exclusions:</u></strong></p>" + "</br>" + ObjModelQuotationMast.Exclusion + " </td></tr>";
                        //string strE = "<tr><td align='left'><p style='font - size: 14px; text - align: justify; font - family:Arial; margin-bottom: 3px;'><strong><u> Exclusions:</u></strong></p><span style='white-space: pre-line;'>" + ObjModelQuotationMast.Exclusion + "</span>" + " </td></tr>";
                        string strE = "<tr><td align='left'><p style='font - size: 18px; text - align: justify; font - family:Arial; margin-bottom: 3px;'><strong><label style='font-size: 18px;'><u style='font - size: 18px;'> Exclusions:</u></label></strong></p><span style='white-space: pre-line;'>" + ObjModelQuotationMast.Exclusion + "</span>" + " </td></tr>";
                        body = body.Replace("[Exclusion]", strE);
                    }
                    else
                    {
                        body = body.Replace("[Exclusion]", "");
                    }

                    if (ObjModelQuotationMast.GeneralTermsCheckbox == true)
                    {
                        body = body.Replace("[GeneralTerms]", ObjModelQuotationMast.GeneralTerms);
                    }
                    else
                    {
                        body = body.Replace("[GeneralTerms]", "");
                    }
                    if (ObjModelQuotationMast.EndCustomer == null || ObjModelQuotationMast.EndCustomer == "")
                    {
                        body = body.Replace("[EndCustomer]", "");
                    }
                    else
                    {
                        body = body.Replace("[EndCustomer]", "End Customer :" + ObjModelQuotationMast.EndCustomer);
                    }



                    string byteData2 = Convert.ToBase64String(ObjModelQuotationMast.IData);
                    string a3 = "<img src = 'data:image/jpg;base64," + byteData2 + " ' style='width:100%;height:400px;' />";
                    body = body.Replace("[QuotationFirstPageImage1]", a3);

                    strs.Append(body);
                    PdfPageSize pageSize = PdfPageSize.A4;
                    PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
                    HtmlToPdf converter = new HtmlToPdf();


                    #region Count Page No
                    SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(body);
                    int PageCount = doc1.Pages.Count;
                    body = body.Replace("[PageCount]", ObjModelQuotationMast.AddEnclosures + ' ' + "(Refer Page " + Convert.ToString(PageCount) + " Of " + Convert.ToString(PageCount) + " )");
                    strs.Append(body);
                    #endregion



                    // set the page timeout (in seconds)
                    converter.Options.MaxPageLoadTime = 240;  //=========================5-Aug-2019
                    converter.Options.PdfPageSize = pageSize;
                    converter.Options.PdfPageOrientation = pdfOrientation;

                    string _Header = string.Empty;
                    string _footer = string.Empty;


                    StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/QuotationHtml/quotation-header.html"));
                    _Header = _readHeader_File.ReadToEnd();
                    //_Header = _Header.Replace("[logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png");
                    _Header = _Header.Replace("[logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.svg");
                    _Header = _Header.Replace("[QuotationNumber]", ObjModelQuotationMast.QuotationNumber);
                    _Header = _Header.Replace("[Date]", Convert.ToString(ObjModelQuotationMast.CreatedDate));

                    StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/QuotationHtml/quotation-footer.html"));
                    _footer = _readFooter_File.ReadToEnd();
                    _footer = _footer.Replace("[LogoFooter]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/FTUEV-NORD-GROUP_Logo_Electric-Blue.svg");
                    // header settings
                    converter.Options.DisplayHeader = true ||
                        true || true;
                    converter.Header.DisplayOnFirstPage = true;
                    converter.Header.DisplayOnOddPages = true;
                    converter.Header.DisplayOnEvenPages = true;
                    //converter.Header.Height = 75;
                    //converter.Header.Height = 140;
                    converter.Header.Height = 100;

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
                    converter.Footer.Height = 70;

                    PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
                    footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    converter.Footer.Add(footerHtml);


                    #region Footer Code
                    ///  PdfTextSection text1 = new PdfTextSection(5, 30, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
                    PdfTextSection text1 = new PdfTextSection(30, 45, "Page: {page_number} of {total_pages}                    ", new System.Drawing.Font("TNG Pro", 8));
                    //text1.HorizontalAlign = PdfTextHorizontalAlign.Right;
                    //text1.VerticalAlign = PdfTextVerticalAlign.Bottom;
                    converter.Footer.Add(text1);




                    // page numbers can be added using a PdfTextSection object
                    //PdfTextSection text = new PdfTextSection(1, 145, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));

                    //text.HorizontalAlign = PdfTextHorizontalAlign.Center;
                    //converter.Footer.Add(text);

                    #endregion

                    if (ObjModelQuotationMast.CostSheetApproveStatus == "1")
                    {
                        converter.Options.SecurityOptions.CanAssembleDocument = false;
                        converter.Options.SecurityOptions.CanCopyContent = false;
                        converter.Options.SecurityOptions.CanEditAnnotations = false;
                        converter.Options.SecurityOptions.CanEditContent = false;
                        converter.Options.SecurityOptions.CanFillFormFields = false;
                        converter.Options.SecurityOptions.CanPrint = false;
                    }
                    else
                    {
                        converter.Options.SecurityOptions.CanAssembleDocument = true;
                        converter.Options.SecurityOptions.CanCopyContent = true;
                        converter.Options.SecurityOptions.CanEditAnnotations = true;
                        converter.Options.SecurityOptions.CanEditContent = true;
                        converter.Options.SecurityOptions.CanFillFormFields = true;
                        converter.Options.SecurityOptions.CanPrint = true;
                    }
                    body = Regex.Replace(body, @"\n", "");
                    PdfDocument doc = converter.ConvertHtmlString(body);

                    if (ObjModelQuotationMast.CostSheetApproveStatus == "1")
                    {
                        doc.Security.CanAssembleDocument = false;
                        doc.Security.CanCopyContent = false;
                        doc.Security.CanEditAnnotations = false;
                        doc.Security.CanEditContent = false;
                        doc.Security.CanFillFormFields = false;
                        doc.Security.CanPrint = false;
                    }
                    else
                    {
                        doc.Security.CanAssembleDocument = true;
                        doc.Security.CanCopyContent = true;
                        doc.Security.CanEditAnnotations = true;
                        doc.Security.CanEditContent = true;
                        doc.Security.CanFillFormFields = true;
                        doc.Security.CanPrint = true;
                    }


                    string ReportName = ObjModelQuotationMast.QuotationNumber + ".pdf";
                    string path = Server.MapPath("~/QuotationHtml");
                    //set document permissions
                    if (ObjModelQuotationMast.AutoA == "3")
                    {

                    }
                    else
                    {
                        if (ObjModelQuotationMast.CostSheetApproveStatus == "0")
                        {
                            string imgFile1 = Server.MapPath("/t6.png");
                            PdfTemplate template1 = doc.AddTemplate(doc.Pages[0].ClientRectangle);
                            //PdfImageElement img1 = new PdfImageElement(150, 50, imgFile1);
                            PdfImageElement img1 = new PdfImageElement(125, 85, imgFile1);
                            img1.Transparency = 15;
                            template1.Add(img1);
                        }
                        else
                        {

                        }
                    }

                    // get image path
                    ///  string imgFile = Server.MapPath("/t1.jpg");
                    //string imgFile = Server.MapPath("/t2.jpg");
                    // watermark all pages - add a template containing an image 
                    // to the bottom right of the page
                    // the image should repeat on all pdf pages automatically
                    // the template should be rendered behind the rest of the page elements
                    //for (int cnt = 0; cnt < doc.Pages.Count; cnt++)
                    //{
                    //    if (cnt > 0)
                    //    {
                    //        PdfTemplate template = doc.AddTemplate(doc.Pages[0].ClientRectangle); //// 635 * 554                         
                    //        PdfImageElement img = new PdfImageElement(125, 150, imgFile);
                    //        img.Transparency = 15;
                    //        //template.Add(img);
                    //        doc.Pages[cnt].Add(img); //// 635 * 554  
                    //    }


                    //}
                    doc.Save(path + '\\' + ReportName);
                    doc.Close();
                    string Result = "";
                    ObjModelQuotationMast.QuotationPDF = ReportName;
                    Result = objDALQuotationMast.InsertUpdateReport(ObjModelQuotationMast);
                    #endregion





                }
                //return View(ObjModelQuotationMast);

            }
                string newpath = Server.MapPath("~/QuotationHtml/");

                byte[] fileBytes = System.IO.File.ReadAllBytes(newpath + @"\" + ObjModelQuotationMast.QuotationPDF);
            }

        #region Delete Code By Rahul 
        public ActionResult DeleteQuotation(int? PK_QTID)
        {
            int Result = 0;
            try
            {
                Result = objDALQuotationMast.DeleteQuotation(PK_QTID);
                if (Result != 0)
                {
                    TempData["DeletedQuotation"] = Result;
                    return RedirectToAction("QuotationMasterDashBoard", "QuotationMaster");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        #endregion

        #region Code By Rahul 

        [HttpGet]
        public ActionResult Searchdata(string Category, string data)
        {
            string QuotationNumber = data;
            string Quotation_Description = "";
            string Enquiry = "";
            string CompanyName = "";
            string EndCustomer = "";
            DataTable DTEditQuotationMast = new DataTable();
            if (Category == "Quotation Number")
            {
                QuotationNumber = data;
                Quotation_Description = "";
                Enquiry = "";
                CompanyName = "";
                EndCustomer = "";
            }
            else if (Category == "Description")
            {
                QuotationNumber = "";
                Quotation_Description = data;
                Enquiry = "";
                CompanyName = "";
                EndCustomer = "";
            }
            else if (Category == "Enquiry Description")
            {
                QuotationNumber = "";
                Quotation_Description = "";
                Enquiry = data;
                CompanyName = "";
                EndCustomer = "";
            }
            else if (Category == "Client Name")
            {
                QuotationNumber = "";
                Quotation_Description = "";
                Enquiry = "";
                CompanyName = data;
                EndCustomer = "";
            }
            else if (Category == "Project Name")
            {
                QuotationNumber = "";
                Quotation_Description = "";
                Enquiry = "";
                CompanyName = "";
                EndCustomer = data;
            }

            try
            {
                DTEditQuotationMast = objDALQuotationMast.CopyQuotation(QuotationNumber, Quotation_Description, Enquiry, CompanyName, EndCustomer);

                if (DTEditQuotationMast.Rows.Count > 0)
                {
                    ObjModelQuotationMast.PK_QTID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["PK_QTID"]);
                    ObjModelQuotationMast.QuotationNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationNumber"]);

                    ObjModelQuotationMast.Revise = "";
                    //TempData["QuotNumber"] = QuotationNumber;
                    //TempData.Keep();
                    ObjModelQuotationMast.EQ_ID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["EQ_ID"]);
                    ObjModelQuotationMast.QuotationCompanyName = Convert.ToString(DTEditQuotationMast.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.QuotationBranch = Convert.ToInt32(DTEditQuotationMast.Rows[0]["BranchName"]);
                    ObjModelQuotationMast.ServiceType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ServiceType"]);
                    ObjModelQuotationMast.ProjectType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ProjectType"]);
                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTEditQuotationMast.Rows[0]["Enquiry"]);
                    ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTEditQuotationMast.Rows[0]["Quotation_Description"]);
                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTEditQuotationMast.Rows[0]["EndCustomer"]);
                    ObjModelQuotationMast.Reference = Convert.ToString(DTEditQuotationMast.Rows[0]["Reference"]);
                    ObjModelQuotationMast.ExpiryDate = Convert.ToString(DTEditQuotationMast.Rows[0]["ExpiryDate"]);
                    ObjModelQuotationMast.Status = Convert.ToInt32(DTEditQuotationMast.Rows[0]["Status"]);
                    ObjModelQuotationMast.GST = Convert.ToString(DTEditQuotationMast.Rows[0]["GST"]);
                    ObjModelQuotationMast.Attachment = Convert.ToString(DTEditQuotationMast.Rows[0]["Attachment"]);
                    ObjModelQuotationMast.Remark = Convert.ToString(DTEditQuotationMast.Rows[0]["Remark"]);
                    ObjModelQuotationMast.HeaderDetails = Convert.ToString(DTEditQuotationMast.Rows[0]["HeaderDetails"]);
                    ObjModelQuotationMast.Subject = Convert.ToString(DTEditQuotationMast.Rows[0]["Subject"]);
                    ObjModelQuotationMast.Deliverable = Convert.ToString(DTEditQuotationMast.Rows[0]["Deliverable"]);
                    ObjModelQuotationMast.Commercials = Convert.ToString(DTEditQuotationMast.Rows[0]["Commercials"]);
                    ObjModelQuotationMast.FeesStructure = Convert.ToString(DTEditQuotationMast.Rows[0]["FeesStructure"]);
                    ObjModelQuotationMast.PaymentTerms = Convert.ToString(DTEditQuotationMast.Rows[0]["PaymentTerms"]);
                    ObjModelQuotationMast.KeyNotes = Convert.ToString(DTEditQuotationMast.Rows[0]["KeyNotes"]);
                    ObjModelQuotationMast.AddEnclosures = Convert.ToString(DTEditQuotationMast.Rows[0]["AddEnclosures"]);
                    ObjModelQuotationMast.EnquiryNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["EnquiryNumber"]);
                    ObjModelQuotationMast.To = Convert.ToString(DTEditQuotationMast.Rows[0]["T_O"]);
                    ObjModelQuotationMast.CC = Convert.ToString(DTEditQuotationMast.Rows[0]["CC"]);
                    ObjModelQuotationMast.CommunicationProtocol = Convert.ToString(DTEditQuotationMast.Rows[0]["CommunicationProtocol"]);
                    ObjModelQuotationMast.Coordinators = Convert.ToString(DTEditQuotationMast.Rows[0]["Coordinators"]);
                    ObjModelQuotationMast.EscalationMatrix = Convert.ToString(DTEditQuotationMast.Rows[0]["EscalationMatrix"]);

                }
                return Json(ObjModelQuotationMast, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion


        #region Copy Quotation
        [HttpGet]
        public ActionResult Copydata(string Category)
        {
            string QuotationNumber = Category;
            //string Quotation_Description = "";
            //string Enquiry = "";
            //string CompanyName = "";
            //string EndCustomer = "";
            DataTable DTEditQuotationMast = new DataTable();
            //if (Category == "Quotation Number")
            //{
            //    QuotationNumber = data;
            //    Quotation_Description = "";
            //    Enquiry = "";
            //    CompanyName = "";
            //    EndCustomer = "";
            //}
            //else if (Category == "Description")
            //{
            //    QuotationNumber = "";
            //    Quotation_Description = data;
            //    Enquiry = "";
            //    CompanyName = "";
            //    EndCustomer = "";
            //}
            //else if (Category == "Enquiry Description")
            //{
            //    QuotationNumber = "";
            //    Quotation_Description = "";
            //    Enquiry = data;
            //    CompanyName = "";
            //    EndCustomer = "";
            //}
            //else if (Category == "Client Name")
            //{
            //    QuotationNumber = "";
            //    Quotation_Description = "";
            //    Enquiry = "";
            //    CompanyName = data;
            //    EndCustomer = "";
            //}
            //else if (Category == "Project Name")
            //{
            //    QuotationNumber = "";
            //    Quotation_Description = "";
            //    Enquiry = "";
            //    CompanyName = "";
            //    EndCustomer = data;
            //}

            try
            {
                DTEditQuotationMast = objDALQuotationMast.NewCopyQuotation(QuotationNumber);

                if (DTEditQuotationMast.Rows.Count > 0)
                {
                    ObjModelQuotationMast.PK_QTID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["PK_QTID"]);
                    ObjModelQuotationMast.QuotationNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationNumber"]);

                    ObjModelQuotationMast.Revise = "";
                    //TempData["QuotNumber"] = QuotationNumber;
                    //TempData.Keep();
                    ObjModelQuotationMast.EQ_ID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["EQ_ID"]);
                    ObjModelQuotationMast.QuotationCompanyName = Convert.ToString(DTEditQuotationMast.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.QuotationBranch = Convert.ToInt32(DTEditQuotationMast.Rows[0]["BranchName"]);
                    ObjModelQuotationMast.ServiceType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ServiceType"]);
                    ObjModelQuotationMast.ProjectType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ProjectType"]);
                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTEditQuotationMast.Rows[0]["Enquiry"]);
                    ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTEditQuotationMast.Rows[0]["Quotation_Description"]);
                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTEditQuotationMast.Rows[0]["EndCustomer"]);
                    ObjModelQuotationMast.Reference = Convert.ToString(DTEditQuotationMast.Rows[0]["Reference"]);
                    ObjModelQuotationMast.ExpiryDate = Convert.ToString(DTEditQuotationMast.Rows[0]["ExpiryDate"]);
                    ObjModelQuotationMast.Status = Convert.ToInt32(DTEditQuotationMast.Rows[0]["Status"]);
                    ObjModelQuotationMast.GST = Convert.ToString(DTEditQuotationMast.Rows[0]["GST"]);
                    ObjModelQuotationMast.Attachment = Convert.ToString(DTEditQuotationMast.Rows[0]["Attachment"]);
                    ObjModelQuotationMast.Remark = Convert.ToString(DTEditQuotationMast.Rows[0]["Remark"]);
                    ObjModelQuotationMast.HeaderDetails = Convert.ToString(DTEditQuotationMast.Rows[0]["HeaderDetails"]);
                    ObjModelQuotationMast.Subject = Convert.ToString(DTEditQuotationMast.Rows[0]["Subject"]);
                    ObjModelQuotationMast.Deliverable = Convert.ToString(DTEditQuotationMast.Rows[0]["Deliverable"]);
                    ObjModelQuotationMast.Commercials = Convert.ToString(DTEditQuotationMast.Rows[0]["Commercials"]);
                    ObjModelQuotationMast.FeesStructure = Convert.ToString(DTEditQuotationMast.Rows[0]["FeesStructure"]);
                    ObjModelQuotationMast.PaymentTerms = Convert.ToString(DTEditQuotationMast.Rows[0]["PaymentTerms"]);
                    ObjModelQuotationMast.KeyNotes = Convert.ToString(DTEditQuotationMast.Rows[0]["KeyNotes"]);
                    ObjModelQuotationMast.AddEnclosures = Convert.ToString(DTEditQuotationMast.Rows[0]["AddEnclosures"]);
                    ObjModelQuotationMast.EnquiryNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["EnquiryNumber"]);
                    ObjModelQuotationMast.To = Convert.ToString(DTEditQuotationMast.Rows[0]["T_O"]);
                    ObjModelQuotationMast.CC = Convert.ToString(DTEditQuotationMast.Rows[0]["CC"]);
                    ObjModelQuotationMast.CommunicationProtocol = Convert.ToString(DTEditQuotationMast.Rows[0]["CommunicationProtocol"]);
                    ObjModelQuotationMast.Coordinators = Convert.ToString(DTEditQuotationMast.Rows[0]["Coordinators"]);
                    ObjModelQuotationMast.EscalationMatrix = Convert.ToString(DTEditQuotationMast.Rows[0]["EscalationMatrix"]);
                    ObjModelQuotationMast.ScopeOfWork = Convert.ToString(DTEditQuotationMast.Rows[0]["ScopeOfWork"]);
                    ObjModelQuotationMast.ThankYouLetter = Convert.ToString(DTEditQuotationMast.Rows[0]["ThankYouLetter"]);
                    ObjModelQuotationMast.ThirdPartyInspectionService = Convert.ToString(DTEditQuotationMast.Rows[0]["ThirdPartyInspectionService"]);
                    ObjModelQuotationMast.Exclusion = Convert.ToString(DTEditQuotationMast.Rows[0]["Exclusion"]);
                    ObjModelQuotationMast.ComplimentaryClose = Convert.ToString(DTEditQuotationMast.Rows[0]["ComplimentaryClose"]);

                }
                return Json(ObjModelQuotationMast, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Export to excel
        [HttpGet]
        public ActionResult ExportIndex(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<QuotationMaster> grid = CreateExportableGrid(Type);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<QuotationMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd/MM/yyyy" + '-' + "HH:mm:ss");

                string filename = "QuotationMaster-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<QuotationMaster> CreateExportableGrid(String Type)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<QuotationMaster> grid = new Grid<QuotationMaster>(GetData(Type));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            //grid.Columns.Add(model => model.QuotationNumber).Titled("Quotation Number");
            //grid.Columns.Add(model => model.Quotation_Description).Titled("Description");
            //grid.Columns.Add(model => model.Reference).Titled("Reference");
            //grid.Columns.Add(model => model.Enquiry).Titled("Enquiry");
            //grid.Columns.Add(model => model.ExpiryDate).Titled("Expiry Date");
            //grid.Columns.Add(model => model.StatusType).Titled("Status");
            //grid.Columns.Add(model => model.ApprovalStatus).Titled("Approval Status");

            grid.Columns.Add(model => model.EnquiryNumber).Titled("Enquiry Reference No");
            grid.Columns.Add(model => model.QuotationNumber).Titled("Quotation Number");
                /// .Encoded(false).RenderedAs(o => Html.ActionLink(o.QuotationNumber, "Quotation", new { PK_QM_ID = o.PK_QTID }, 
                /// new { title = "Quotation Number New" })).Filterable(true);

            grid.Columns.Add(model => model.Enquiry).Titled("Customer Name");
            grid.Columns.Add(model => model.Quotation_Description).Titled("Project Name");
            grid.Columns.Add(model => model.EstimatedAmount).Titled("Quotation Amount (INR)");
            grid.Columns.Add(model => model.InspectionLocation).Titled("Inspection Location");
            grid.Columns.Add(model => model.StatusType).Titled("Status");
            
            grid.Columns.Add(model => model.DApprovalStatus).Titled("Domestic Approval Status");
            grid.Columns.Add(model => model.IApprovalStatus).Titled("International Approval Status");
            grid.Columns.Add(model => model.CreatedBy).Titled("Created By");
            grid.Columns.Add(model => model.CreatedDate).Titled("Created Date");
            grid.Columns.Add(model => model.ExpiryDate).Titled("Expiry Date");
            grid.Columns.Add(model => model.JobNo).Titled("Job No");
            grid.Columns.Add(model => model.ProjectName).Titled("OBS Type");
            grid.Columns.Add(model => model.PortfolioType).Titled("Service Portfolio Type");
            grid.Columns.Add(model => model.SubServiceType).Titled("Service Type");





            grid.Pager = new GridPager<QuotationMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelQuotationMast.lstQuotationMasterDashBoard1.Count;


            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<QuotationMaster> GetData(String Type)
        {

            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            lstQuotationMast = objDALQuotationMast.QuotaionMastertDashBoard(Type);

            ObjModelQuotationMast.lstQuotationMasterDashBoard1 = lstQuotationMast;


            return ObjModelQuotationMast.lstQuotationMasterDashBoard1;
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
                Guid guid = new Guid(id);
                DTGetDeleteFile = objDALQuotationMast.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = objDALQuotationMast.DeleteUploadedFile(id);
                    var path = Path.Combine(Server.MapPath("~/Files/QuotationAttachment/"), id + fileDetails.Extension);
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
            // return File(Path.Combine(Server.MapPath("~/Files/QuotationAttachment/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDALQuotationMast.GetFileContent(Convert.ToInt32(d));

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
            DTDownloadFile = objDALQuotationMast.GetFileContent(Convert.ToInt32(d));

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

        public JsonResult GetClientRecord(string prefix)

        {
            DataSet dsTopic = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            // dsTopic = objDTS.GetTopicList(prefix);
            dsTopic = objDALQuotationMast.GetClientRecord(prefix);
            List<QuotationMaster> searchlist = new List<QuotationMaster>();

            foreach (DataRow dr in dsTopic.Tables[0].Rows)

            {

                searchlist.Add(new QuotationMaster
                {
                    ClientName = dr["CompanyName"].ToString(),
                    
                });

            }
            //var getdata = (from n in searchlist
            //               where n.TrainingName.StartsWith(prefix)
            //               select new { label = n.TrainingName, value = n.TrainingId });
            return Json(searchlist, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetQuotationDescription(string prefix)

        {
            DataSet dsTopic = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            // dsTopic = objDTS.GetTopicList(prefix);
            dsTopic = objDALQuotationMast.GetQuotationDescription(prefix);
            List<QuotationMaster> searchlist = new List<QuotationMaster>();

            foreach (DataRow dr in dsTopic.Tables[0].Rows)

            {

                searchlist.Add(new QuotationMaster
                {
                    Quotation_Description = dr["Quotation_Description"].ToString(),

                });

            }
            //var getdata = (from n in searchlist
            //               where n.TrainingName.StartsWith(prefix)
            //               select new { label = n.TrainingName, value = n.TrainingId });
            return Json(searchlist, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetEnquiryDescription(string prefix)

        {
            DataSet dsTopic = new DataSet();
           
            dsTopic = objDALQuotationMast.GetEnquiryDescription(prefix);
            List<QuotationMaster> searchlist = new List<QuotationMaster>();

            foreach (DataRow dr in dsTopic.Tables[0].Rows)

            {

                searchlist.Add(new QuotationMaster
                {
                    Enquiry_Description = dr["Enquiry_Description"].ToString(),

                });

            }
           
            return Json(searchlist, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult NewSearchdataold(string Category, string data)
        {
            string QuotationNumber = data;
            string Quotation_Description = "";
            string Enquiry = "";
            string CompanyName = "";
            string EndCustomer = "";
            DataTable DTEditQuotationMast = new DataTable();
            if (Category == "Quotation Number")
            {
                QuotationNumber = data;
                Quotation_Description = "";
                Enquiry = "";
                CompanyName = "";
                EndCustomer = "";
            }
            else if (Category == "Description")
            {
                QuotationNumber = "";
                Quotation_Description = data;
                Enquiry = "";
                CompanyName = "";
                EndCustomer = "";
            }
            else if (Category == "Enquiry Description")
            {
                QuotationNumber = "";
                Quotation_Description = "";
                Enquiry = data;
                CompanyName = "";
                EndCustomer = "";
            }
            else if (Category == "Client Name")
            {
                QuotationNumber = "";
                Quotation_Description = "";
                Enquiry = "";
                CompanyName = data;
                EndCustomer = "";
            }
            else if (Category == "Project Name")
            {
                QuotationNumber = "";
                Quotation_Description = "";
                Enquiry = "";
                CompanyName = "";
                EndCustomer = data;
            }

            try
            {
                DTEditQuotationMast = objDALQuotationMast.CopyQuotation(QuotationNumber, Quotation_Description, Enquiry, CompanyName, EndCustomer);

                if (DTEditQuotationMast.Rows.Count > 0)
                {
                    ObjModelQuotationMast.PK_QTID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["PK_QTID"]);
                    ObjModelQuotationMast.QuotationNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationNumber"]);

                    ObjModelQuotationMast.Revise = "";
                    //TempData["QuotNumber"] = QuotationNumber;
                    //TempData.Keep();
                    ObjModelQuotationMast.EQ_ID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["EQ_ID"]);
                    ObjModelQuotationMast.QuotationCompanyName = Convert.ToString(DTEditQuotationMast.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.QuotationBranch = Convert.ToInt32(DTEditQuotationMast.Rows[0]["BranchName"]);
                    ObjModelQuotationMast.ServiceType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ServiceType"]);
                    ObjModelQuotationMast.ProjectType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ProjectType"]);
                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTEditQuotationMast.Rows[0]["Enquiry"]);
                    ObjModelQuotationMast.Quotation_Description = Convert.ToString(DTEditQuotationMast.Rows[0]["Quotation_Description"]);
                    ObjModelQuotationMast.EndCustomer = Convert.ToString(DTEditQuotationMast.Rows[0]["EndCustomer"]);
                    ObjModelQuotationMast.Reference = Convert.ToString(DTEditQuotationMast.Rows[0]["Reference"]);
                    ObjModelQuotationMast.ExpiryDate = Convert.ToString(DTEditQuotationMast.Rows[0]["ExpiryDate"]);
                    ObjModelQuotationMast.Status = Convert.ToInt32(DTEditQuotationMast.Rows[0]["Status"]);
                    ObjModelQuotationMast.GST = Convert.ToString(DTEditQuotationMast.Rows[0]["GST"]);
                    ObjModelQuotationMast.Attachment = Convert.ToString(DTEditQuotationMast.Rows[0]["Attachment"]);
                    ObjModelQuotationMast.Remark = Convert.ToString(DTEditQuotationMast.Rows[0]["Remark"]);
                    ObjModelQuotationMast.HeaderDetails = Convert.ToString(DTEditQuotationMast.Rows[0]["HeaderDetails"]);
                    ObjModelQuotationMast.Subject = Convert.ToString(DTEditQuotationMast.Rows[0]["Subject"]);
                    ObjModelQuotationMast.Deliverable = Convert.ToString(DTEditQuotationMast.Rows[0]["Deliverable"]);
                    ObjModelQuotationMast.Commercials = Convert.ToString(DTEditQuotationMast.Rows[0]["Commercials"]);
                    ObjModelQuotationMast.FeesStructure = Convert.ToString(DTEditQuotationMast.Rows[0]["FeesStructure"]);
                    ObjModelQuotationMast.PaymentTerms = Convert.ToString(DTEditQuotationMast.Rows[0]["PaymentTerms"]);
                    ObjModelQuotationMast.KeyNotes = Convert.ToString(DTEditQuotationMast.Rows[0]["KeyNotes"]);
                    ObjModelQuotationMast.AddEnclosures = Convert.ToString(DTEditQuotationMast.Rows[0]["AddEnclosures"]);
                    ObjModelQuotationMast.EnquiryNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["EnquiryNumber"]);
                    ObjModelQuotationMast.To = Convert.ToString(DTEditQuotationMast.Rows[0]["T_O"]);
                    ObjModelQuotationMast.CC = Convert.ToString(DTEditQuotationMast.Rows[0]["CC"]);
                    ObjModelQuotationMast.CommunicationProtocol = Convert.ToString(DTEditQuotationMast.Rows[0]["CommunicationProtocol"]);
                    ObjModelQuotationMast.Coordinators = Convert.ToString(DTEditQuotationMast.Rows[0]["Coordinators"]);
                    ObjModelQuotationMast.EscalationMatrix = Convert.ToString(DTEditQuotationMast.Rows[0]["EscalationMatrix"]);

                }
                return Json(ObjModelQuotationMast, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
       
        public JsonResult NewSearchdata(string ClientName, string QuotationDescription, string QuotationNumberSearch, string EnquiryDescription)
        {
            int id = 0;
            string i = "";
          


            
            //DataTable DSJobMasterByQtId = new DataTable();

           

            //DSJobMasterByQtId = objDALQuotationMast.GetQuotationDescription(ClientName, QuotationDescription);

            //if (DSJobMasterByQtId.Rows.Count > 0)
            //{
               
            //    i = String.Join(",", DSJobMasterByQtId.AsEnumerable().Select(x => x.Field<string>("PK_Call_ID").ToString()).ToArray());

               
            //}


            var Data = objDALQuotationMast.GetQuotationDescription(ClientName, QuotationDescription, QuotationNumberSearch);

           
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        public static string GetCurrentFinancialYear()
        {
            int CurrentYear = DateTime.Today.Year;
            int PreviousYear = DateTime.Today.Year - 1;
            int NextYear = DateTime.Today.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (DateTime.Today.Month > 3)
                FinYear = CurYear + "-" + NexYear;
            else
                FinYear = PreYear + "-" + CurYear;
            return FinYear.Trim();
        }
		
		public JsonResult CheckForJobCreation(string PK_QTID)//Checking Existing User Name
        {
            string Result = string.Empty;
            DataTable DTExistRoleName = new DataTable();
            try
            {
                DTExistRoleName = objDALQuotationMast.ChkForJob(PK_QTID);
													
								  

                if (DTExistRoleName.Rows.Count > 0)
                {
                    int QuotationAmount = Convert.ToInt32(DTExistRoleName.Rows[0]["Amount"]);
                    foreach (DataRow row in DTExistRoleName.Rows)
                    {
                        if (QuotationAmount >= 1000000) //CH Approvel
                        {
                            //object BMS = row["Status"];
                            //object CAS = row["CLStatus"];

                            object IBMS = row["IBMS"];
                            object ICLS = row["ICLS"];
                            object DBMS = row["DBMS"];
                            object DCLS = row["DCLS"];
                            string InterNational = "";
                            string Domestic = "";

                            if (ICLS == DBNull.Value || IBMS == DBNull.Value)
                            {
                                //Not Approve make "0"
                                InterNational = "NotA";

                            }
                            else
                            {
                                InterNational = "A";
                            }
                            if (DBMS == DBNull.Value || DCLS == DBNull.Value)
                            {
                                Domestic = "NotA";
                                //return Json(0);
                            }
                            else
                            {
                                Domestic = "A";
                            }

                            #region send status
                            if (InterNational == "NotA" && Domestic == "NotA")
                            {
                                Session["JobCreationType"] = "0";
                                return Json(0);// Both Quotation Not Approved

                            }
                            else if (InterNational == "NotA" && Domestic == "A")
                            {
                                Session["JobCreationType"] = "1";
                                return Json(1);// Domestic Approved

                            }
                            else if (InterNational == "A" && Domestic == "NotA")
                            {
                                Session["JobCreationType"] = "2";
                                return Json(2);// InterNational Approved

                            }
                            else if (InterNational == "A" && Domestic == "A")
                            {
                                Session["JobCreationType"] = "3";
                                return Json(3);// Both Approved

                            }
                            #endregion


                        }
                        else //CL Approvel not required
                        {
                            object IBMS = row["IBMS"];
                            object ICLS = row["ICLS"];
                            object DBMS = row["DBMS"];
                            object DCLS = row["DCLS"];
                            object Auto = row["Auto"];
                            string InterNational = "";
                            string Domestic = "";

                            if (IBMS == DBNull.Value)
                            {
                                //Not Approve make "0"
                                InterNational = "NotA";

                            }
                            else
                            {
                                InterNational = "A";
                            }
                            if (DBMS == DBNull.Value)
                            {
                                Domestic = "NotA";
                                //return Json(0);
                            }
                            else
                            {
                                Domestic = "A";
                            }
                            if (Auto == "Auto")//Auto Appro
                            {
                                Domestic = "A";
                                //return Json(0);
                            }
                            else
                            {
                                //Domestic = "NotA";
                            }

                            #region send status
                            if (InterNational == "NotA" && Domestic == "NotA")
                            {
                                Session["JobCreationType"] = "0";
                                return Json(0);// Both Quotation Not Approved

                            }
                            else if (InterNational == "NotA" && Domestic == "A")
                            {
                                Session["JobCreationType"] = "1";
                                return Json(1);// Domestic Approved

                            }
                            else if (InterNational == "A" && Domestic == "NotA")
                            {
                                Session["JobCreationType"] = "2";
                                return Json(2);// InterNational Approved

                            }
                            else if (InterNational == "A" && Domestic == "A")
                            {
                                Session["JobCreationType"] = "3";
                                return Json(3);// Both Approved

                            }
                            #endregion
                        }

                    }
                }
                else
                {
                    return Json(0);
                }




            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(1);
        }



        [HttpGet]
        public ActionResult AddQuotationImage(string Category, string data)
        {
            DataTable DSGetImage = new DataTable();
            List<QuotationMaster> lstImage = new List<QuotationMaster>();
            try
            {

                DSGetImage = objDALQuotationMast.GetImageFromDatabase();
                if (DSGetImage.Rows.Count > 0)
                {
                    foreach (DataRow dr in DSGetImage.Rows)
                    {
                        lstImage.Add(
                            new QuotationMaster
                            {
                                IId = Convert.ToInt32(dr["Id"]),
                                IName = Convert.ToString(dr["Name"]),
                                //IDatabyte = Convert.ToByte(dr["Data"]),
                                IData = (byte[])dr["Data"],

                                //PK_IP_Id = Convert.ToInt32(dr["PK_IP_Id"]),
                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewBag.CostSheet = lstImage;
            return View();
        }
        [HttpPost]
        public ActionResult AddQuotationImage(HttpPostedFileBase postedFile, QuotationMaster QM, List<HttpPostedFileBase> img_Banner)
        {
            string Result = string.Empty;
            byte[] bytes;
            //using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            //{
            //    bytes = br.ReadBytes(postedFile.ContentLength);
            //}
            //QM.IName = Path.GetFileName(postedFile.FileName);
            //QM.IContentType = postedFile.ContentType;
            //QM.IData = bytes;
            //Result = objDALQuotationMast.InsertUpdateImage(QM);
            //return RedirectToAction("AddQuotationImage");

            #region  upload multiple Image
            HttpPostedFileBase Imagesection;
            if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
            {
                foreach (HttpPostedFileBase single in img_Banner) // Added by Sagar Panigrahi

                {

                   

                    Imagesection = single;//Request.Files["img_Banner"];
                    if (Imagesection != null && Imagesection.FileName != "")
                    {                       

                        #region Save Image in Folder
                        
                        //Set the Image File Path.
                        //string filePath = "~/CoverImages/" + single.FileName;
                        string a= CommonControl.FileUpload("CoverImages/", single);
                        //Save the Image File in Folder.
                        #endregion


                        using (BinaryReader br = new BinaryReader(single.InputStream))
                        {
                            bytes = br.ReadBytes(single.ContentLength);
                        }

                        QM.IName = Path.GetFileName(single.FileName);
                        QM.IContentType = single.ContentType;
                        QM.IData = bytes;

                        Result = objDALQuotationMast.InsertUpdateImage(QM);
                    }
                    else
                    {

                    }
                }
            }
            return RedirectToAction("AddQuotationImage");
            #endregion

        }


        [HttpPost]
        public ActionResult DeleteImage(QuotationMaster IVR, FormCollection fc)
        {
            string Result = string.Empty;

            foreach (var item in IVR.LImage)
            {
                if (item.chkbox == true)
                {
                    int aid = item.IId;
                    bool s = objDALQuotationMast.DeleteQuotationImage(Convert.ToInt32(aid));
                }
                else
                {

                }

            }


            return RedirectToAction("AddQuotationImage");
        }

        //public ActionResult AddLandingImageForQuotationPDF(int? csid, int? PK_QTID, CostSheet C, string Comment)
        //{
        //    try
        //    {
        //        string Result = string.Empty;
        //        Result = objDalCostSheet.AddComment(Convert.ToInt32(csid), C);
        //        return RedirectToAction("CostSheet", new { @PK_EQID = Session["EQ_ID"], @Quatation = Session["QT_ID"], @PK_QTID = C.PK_QTID/*PK_QTID*/ });
        //    }
        //    catch (Exception)
        //    {
        //        return View();
        //    }
        //}

        [HttpPost]
        public ActionResult AddLandingImageForQuotationPDF(QuotationMaster IVR, FormCollection fc, string IId, int? PK_QM_ID)
        {
            string a = fc["chkbox"];
            string Result = string.Empty;

            //foreach (var item in IVR.LImage)
            //{
            //    if (item.chkbox == true)
            //    {

            //    }
            //    else
            //    {

            //    }

            //}

            //int? PK_QM_ID = IVR.PK_QTID;
            return RedirectToAction("Quotation", new { PK_QM_ID = PK_QM_ID });
        }



        [HttpPost]

        public JsonResult AddLandingImageForQuotationPDF1(string IId, string QID, QuotationMaster Q)
        {

            //var Data = objDALQuotationMast.GetQuotationDescription(ClientName, QuotationDescription, QuotationNumberSearch);
            var Data = objDALQuotationMast.UpdateQuotationImage(IId, QID);

            return Json(Data, JsonRequestBehavior.AllowGet);
        }


        
        public JsonResult LoadImage()
        {
            try
            { 

                DSGetImage = objDALQuotationMast.GetImage();
                if (DSGetImage.Rows.Count > 0)
                {
                    foreach (DataRow dr in DSGetImage.Rows)
                    {
                        lstImage.Add(
                            new QuotationMaster
                            {
                                IId = Convert.ToInt32(dr["Id"]),
                                IName = Convert.ToString(dr["Name"]),                                
                                IData = (byte[])dr["Data"],                                
                            }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                return Json("Failure", JsonRequestBehavior.AllowGet);
            }

            ViewBag.CostSheet = lstImage;

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Details(string customerId)
        {

            try
            {

                DSGetImage = objDALQuotationMast.GetImage();
                if (DSGetImage.Rows.Count > 0)
                {
                    foreach (DataRow dr in DSGetImage.Rows)
                    {
                        lstImage.Add(
                            new QuotationMaster
                            {
                                IId = Convert.ToInt32(dr["Id"]),
                                IName = Convert.ToString(dr["Name"]),
                                //IData = (byte[])dr["Data"],
                            }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                return Json("Failure", JsonRequestBehavior.AllowGet);
            }

            ObjModelQuotationMast.JobDashBoard = lstImage;
            ViewBag.CostSheet = lstImage;

            return PartialView("Details", ObjModelQuotationMast);
        }

    }
}