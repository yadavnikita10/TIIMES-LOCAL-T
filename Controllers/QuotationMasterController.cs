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
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Mail;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
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

            #region Get Auto text


            ObjModelQuotationMast.Subject = "Quotation for Third party inspection services for MEP (Mechanical, Electrical, Plumbing & Firefighting) projects/ Equipment under an Annual Rate Contract (ARC) across pan India.";
            ObjModelQuotationMast.EnquiryAdditionRef = @"E-Mail Dated 07 Aug 24 and (Include all documents and correspondence)

Dear Sir,
Thank you very much for your enquiry. We are very keen to associate with your esteemed organization for this prestigious project.
We are pleased to submit our offer as follows:";


            ObjModelQuotationMast.ScopeOfWork = @"
Scope of Inspection shall be as per the latest Approved QAP/ITP/FATP/QCP, Drawings, Datasheets, Relevant standards, and documents as applicable for the ordered Inspection.

Mechanical inspections include Air-conditioning Products (e.g., Chillers, AHU, Ducting materials, Pumps, motors, pipes, etc.), Pipeline Products (e.g., used in Oil & Gas sector), Fire-Fighting Equipment (e.g., Pumps, Pipes, motors, etc.), and Plumbing Products (e.g., sanitary products, pipes, fittings, etc.).

Electrical inspections include Electrical Products (e.g., Cables, HT/LT Panels, switchboards, all types of Transformers, Instruments, Process Control Equipment, UPS, etc.).

<br/>•   Review / Witness / Verification of incoming materials/items/test certificates, visual and dimensional inspection, traceability of markings, and additional tests if any.
<br/>•   Review of applicability of approved procedures.
<br/>•   Review and comment, including recommendations for any additional tests, Quality Assurance, Control Programs (Plans), and Inspection Plans submitted by the OEMs or Listenlights.
<br/>•   Identify Qualified Inspectors of Class A or Class B category and seek approval of Listenlights personnel responsible for Inspection. To be finalized after classification of inspectors by M/S Listenlights.
<br/>•   Witness the tests as per the ITP at the manufacturer’s premise and test for compliance with agreed product performance parameters/acceptance norms.
<br/>•   Carry out monitoring/surveillance of the processes.
<br/>•   Review, as per ITP, of manufacturer/supplier inspection and test reports and their own certification of the equipment/items.
<br/>•   Report any non-conformances or areas of concern witnessed during inspection and testing. Communicate corrective actions and risks foreseen.
<br/>•   Stage-wise and final inspection of critical components at various vendor places.
<br/>•   Packing, Loading Inspection at vendor location (as per Listenlights' request).

Competency matrix is attached as per Annexure A.";

            ObjModelQuotationMast.Exclusion = @"Expediting, Design Appraisal/Verification, Statutory & Regulatory Services, vendor Evaluation, WPS/PQR, WPQ, type certification or any other service not specifically mentioned is excluded from the above said scope of work.

•  Expediting of critical and noncritical components at various vendor works (as per request of Listenlights). Expediting will be done by expeditors not inspectors, separate offer will be submitted for the same.

• Process Audit at vendor works for evaluating the product, process and systems prevailing in the vendor organization - Separate offer will be submitted for vendor audits.

• Evaluation of Vendor based on the parameters defined by Listenlights - Separate offer will be submitted for vendor evaluations.

Client Scope:
Client shall provide all the necessary approved documents required for performing the inspections along with inspection call.
Client shall ensure that the supplier has completed his own internal inspection satisfactorily and completed all his records and kept it ready for our third party inspection at the applicable stages. Client shall also ensure that the supplier shall provide to our inspector free access to all areas of the plant, equipment, documents and records.";

            ObjModelQuotationMast.Deliverable = @"IVR and IRN (Inspection Visit Report and Inspection Release Note / Certificate). 
IRN is the only report which confirms acceptance by TUV India inspector and client must only use IRN to authorize to dispatch. 
IVR is not the report for final acceptance / Dispatch.
<br />
TUV India shall issue Inspection Visit Report & Inspection Release Certificate after satisfactory completion of inspection visit.";


            ObjModelQuotationMast.CommunicationProtocol = @"TUV India shall handle all activities related to Co-Ordination under this contract. 
Inspection Calls/Any communications related to shall be sent to the following mail ids:

<br/>To:  
<br/>CC: ";

            ObjModelQuotationMast.FeesStructure = @"A) Our fees for the Inspection services offered on per man day/per engineer basis at PAN India shall be INR --/- 
(In words) for locations within city limits from our branch offices.
Our offices are located at Mumbai, Noida, Bangalore, Chennai, Hyderabad, Nashik, Coimbatore, Rajkot, Gandhidham, Ahmedabad, Surat, Vadodara, Pune, 
Nagpur, Kolkata, Aurangabad, Kolhapur, Ludhiana, Yamunanagar, Hubli, Vizag, and Trichy.
<br/>
B) For locations beyond 50 kms from branch offices shall be chargeable at INR /- (In words).

Notes:
<br/>1. The said Man Day fees shall be applicable for working up to 08 Hours per Day per Engineer.
<br/>2. The said fees shall be all inclusive of Inspection fee, Travel cost & other incidental expense (if any).
<br/>3. TUV India shall not claim additional fees other than the stated inspection fees, if within the above stated clause.
<br/>4. The above fees are applicable for the inspections that shall be carried out in India.
<br/>5. Additional Work hours beyond working hours as stated above shall be charged at 1.5 times on pro-rata basis.
<br/>6. Working on holidays / national holidays / Sundays shall be charged at 1.5 times on pro-rata basis.
<br/>7. Our working days are 6 days a week and we follow the Public and National Holidays as per local states.
<br/>8. Inspection call shall be provided two working days prior to the actual date of inspection.
<br/>9. In case if the call gets cancelled after our engineer reaches the vendor premises for inspection, such visit shall be considered as abortive visits 
   and shall be charged as per man day rate.";


            ObjModelQuotationMast.PaymentTerms = @"a. TUV India shall raise the Invoice immediately upon completion of the inspection visit / on a monthly basis.
b. Payment shall be done within 30 days from the date of invoice.
c. GST is applicable as per prevailing Govt. rates.";


            //ObjModelQuotationMast.Validity = "Our offer shall be valid for 30 days from the date of offer. /*Upon acceptance our prices will be held firm till March 31,*/" + stryear.ToString().Substring(stryear.Length - 4, 4) + ".";
            ObjModelQuotationMast.ValidityDate = DateTime.Today.AddDays(90).ToString("dd/MM/yyyy");

            ObjModelQuotationMast.Validity = "This offer shall be valid for 90 days(" + DateTime.Today.AddDays(90).ToString("dd/MM/yyyy") + ") from the date of offer.";
            ObjModelQuotationMast.validityNumber = "90";

            ObjModelQuotationMast.GeneralTerms = @"The GENERAL TERMS AND CONDITIONS TUVI/GTC/01 Rev. 1 Date: 07/11/2023 (Latest?) of TUV INDIA 
shall form an integral part of this quotation and shall apply to all services offered by TUV India.
In addition, TUV NORD general terms and conditions, including limitation of liability, shall be applicable. 
Upon acceptance of an order, a contract in the specified format of TUV NORD would be agreed with you.";





            #endregion

            //ObjModelQuotationMast.Validity = "Our offer                dated:           shall be valid for 30 days from the date of offer. Upon acceptance our prices will be held firm till March 31," + stryear.ToString().Substring(stryear.Length-4,4)+"." ;


            ObjModelQuotationMast.ThankYouLetter = "Dear Sir / Madam ,";
            // ObjModelQuotationMast.CommunicationProtocol = "TUV India shall handle all activities related to Co-Ordination under this contract. Inspection Calls/Any communications related to shall be sent to the following mail ids:";


            // ObjModelQuotationMast.AddEnclosures = "F/MR/27 Rev. 06 Revision date: 07/06/2022";
            ObjModelQuotationMast.AddEnclosures = "TUVI/GTC/01 Rev.1 Date:07/11/2023";

            //ObjModelQuotationMast.GeneralTerms = "In addition, TUV NORD general terms and conditions including limitation of liability shall be applicable. Upon acceptance of an order a contract in the specified format of TUV NORD would be agreed with you.";
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

            if (PK_QM_ID > 0 && PK_QM_ID != null && Revise != string.Empty && Revise != null)
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
                               ManDaysRate = Convert.ToString(dr["ManDaysRate"]),
                               ActualManDays = Convert.ToString(dr["ActualManDays"])
                           }
                         );
                    }
                    ViewBag.lstDOrderType = lstDOrderType;

                }
                #endregion

                //get FollowUp data 
                #region get FollowUp data 
                DataTable GetFollowUpData = new DataTable();
                List<QuotationMaster> lstQuotationMasterFollowUp = new List<QuotationMaster>();

                GetFollowUpData = objDALQuotationMast.FollowUpData(PK_QM_ID);
                if (GetFollowUpData.Rows.Count > 0)
                {
                    foreach (DataRow dr in GetFollowUpData.Rows)
                    {
                        lstQuotationMasterFollowUp.Add(
                           new QuotationMaster
                           {

                               Details = Convert.ToString(dr["Description"]),
                               Followupddate = Convert.ToString(dr["FollowUpDate"]),
                               NextFollowUpDate = Convert.ToString(dr["NextFollowUpDate"]),
                           }
                         );
                    }
                    ViewBag.FollowUpDates = lstQuotationMasterFollowUp;

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
                               IManDaysRate = Convert.ToString(dr["ManDaysRate"]),
                               IActualManDays = Convert.ToString(dr["ActualManDays"])
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
                    ObjModelQuotationMast.ContactName = Convert.ToString(DTEditQuotationMast.Rows[0]["ContactName"]);
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
                    ObjModelQuotationMast.Budgetary = Convert.ToString(DTEditQuotationMast.Rows[0]["Budgetary"]);  //added by nikita on 17062024
                    ObjModelQuotationMast.Source = Convert.ToString(DTEditQuotationMast.Rows[0]["Source"]); //added by nikita on 25062024
                    ObjModelQuotationMast.EnquirySource = Convert.ToString(DTEditQuotationMast.Rows[0]["Enquirysource"]); //added by nikita on 24062024

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
                    if (Convert.ToInt32(ObjModelQuotationMast.EstimatedAmount) >= 17000000)
                    {
                        if (ObjModelQuotationMast.InspectionLocation == "2")
                        {
                            DataTable DTCLEditQuotationMast = new DataTable();
                            DTCLEditQuotationMast = objDALQuotationMast.GetFINApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.CostSheetFINStatus = DTCLEditQuotationMast.Rows[0]["FINStatus"].ToString();
                        }
                        else if (ObjModelQuotationMast.InspectionLocation == "1")
                        {
                            DataTable DTCLEditQuotationMast = new DataTable();
                            DTCLEditQuotationMast = objDALQuotationMast.GetIFINApprovalStatus(PK_QM_ID);

                            ObjModelQuotationMast.ICostSheetFINStatus = DTCLEditQuotationMast.Rows[0]["FINStatus"].ToString();
                        }
                        else if (ObjModelQuotationMast.InspectionLocation == "1,2")
                        {
                            /// Domestic
                            DataTable DTCLEditQuotationMast = new DataTable();
                            DTCLEditQuotationMast = objDALQuotationMast.GetFINApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.CostSheetFINStatus = DTCLEditQuotationMast.Rows[0]["FINStatus"].ToString();
                            /// International
                            DataTable DTICLEditQuotationMast = new DataTable();
                            DTICLEditQuotationMast = objDALQuotationMast.GetIFINApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.ICostSheetCLStatus = DTICLEditQuotationMast.Rows[0]["FINStatus"].ToString();

                            //ObjModelQuotationMast.GeneralTerms = Convert.ToString(DTEditQuotationMast.Rows[0]["GeneralTerms"]);

                            //if (Convert.ToString(DTEditQuotationMast.Rows[0]["GeneralTermsCheckbox"]) == "1")
                            //{
                            //    ObjModelQuotationMast.GeneralTermsCheckbox = true;
                            //}
                            //else
                            //{
                            //    ObjModelQuotationMast.GeneralTermsCheckbox = false;
                            //}
                        }
                    }
                    if (Convert.ToInt32(ObjModelQuotationMast.EstimatedAmount) >= 1000000 || ObjModelQuotationMast.ProjectType == 9)
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
                //if (DTGetUploadedFile.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in DTGetUploadedFile.Rows)
                //    {
                //        lstEditFileDetails.Add(
                //           new FileDetails
                //           {

                //               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                //               FileName = Convert.ToString(dr["FileName"]),
                //               Extension = Convert.ToString(dr["Extenstion"]),
                //               IDS = Convert.ToString(dr["FileID"]),
                //           }
                //         );
                //    }
                //    ViewData["lstEditFileDetails"] = lstEditFileDetails;
                //    ObjModelQuotationMast.FileDetails = lstEditFileDetails;
                //}
                //added by nikita on 25062024
                if (DTGetUploadedFile.Rows.Count > 0)
                {
                    List<FileDetails> lstEditLegalFileDetails = new List<FileDetails>();
                    List<FileDetails> lstEditOtherFileDetails = new List<FileDetails>();
                    foreach (DataRow dr in DTGetUploadedFile.Rows)
                    {
                        FileDetails fileDetails = new FileDetails
                        {

                            PK_ID = Convert.ToInt32(dr["PK_ID"]),
                            FileName = Convert.ToString(dr["FileName"]),
                            Extension = Convert.ToString(dr["Extenstion"]),
                            IDS = Convert.ToString(dr["FileID"]),
                            AttachmentType = Convert.ToString(dr["Type"]),

                        };

                        if (fileDetails.AttachmentType == "Tender")
                        {
                            lstEditLegalFileDetails.Add(fileDetails);
                        }
                        else
                        {
                            lstEditOtherFileDetails.Add(fileDetails);
                        }
                        if (lstEditLegalFileDetails.Count > 0)
                        {
                            ViewData["lstEditFileDetails"] = lstEditLegalFileDetails;
                            ObjModelQuotationMast.FileDetails_ = lstEditLegalFileDetails;
                        }

                        // Set ViewData and ObjModelEnquiry for other files
                        if (lstEditOtherFileDetails.Count > 0)
                        {
                            ViewData["lstEditOtherFileDetails"] = lstEditOtherFileDetails;
                            ObjModelQuotationMast.FileDetails = lstEditOtherFileDetails;
                        }
                    }


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


                //#region Bind Company Addr
                //DataSet DsCompanyAddr = new DataSet();
                //List<NameCode> lstCompanyAddr = new List<NameCode>();

                //DsCompanyAddr = objDALQuotationMast.GetCompanyAddr(Convert.ToInt32(ObjModelQuotationMast.EQ_ID));
                ////DsCompanyAddr = objDALQuotationMast.GetCompanyAddr(Convert.ToInt32(ObjModelQuotationMast.CompanyAddress));
                //if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                //{
                //    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                //                      select new NameCode()
                //                      {
                //                          Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                //                          Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                //                      }).ToList();
                //}

                //IEnumerable<SelectListItem> ComAddrItems;
                //ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

                ////ViewBag.ComAddr = ComAddrItems;
                //ViewData["ComAddr1"] = ComAddrItems;
                //#endregion


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
                               ManDaysRate = Convert.ToString(dr["ManDaysRate"]),
                               ActualManDays = Convert.ToString(dr["ActualManDays"])
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
                               IManDaysRate = Convert.ToString(dr["ManDaysRate"]),
                               IActualManDays = Convert.ToString(dr["ActualManDays"])
                           }
                         );
                    }
                    ViewBag.lstIOrderType = lstIOrderType;
                }

                #endregion


                #region get FollowUp data 
                DataTable GetFollowUpData = new DataTable();
                List<QuotationMaster> lstQuotationMasterFollowUp = new List<QuotationMaster>();

                GetFollowUpData = objDALQuotationMast.FollowUpData(PK_QM_ID);
                if (GetFollowUpData.Rows.Count > 0)
                {
                    foreach (DataRow dr in GetFollowUpData.Rows)
                    {
                        lstQuotationMasterFollowUp.Add(
                           new QuotationMaster
                           {

                               Details = Convert.ToString(dr["Description"]),
                               Followupddate = Convert.ToString(dr["FollowUpDate"]),
                               NextFollowUpDate = Convert.ToString(dr["NextFollowUpDate"]),
                           }
                         );
                    }
                    ViewBag.FollowUpDates = lstQuotationMasterFollowUp;

                }
                #endregion

                DataTable DTEditQuotationMast = new DataTable();
                DTEditQuotationMast = objDALQuotationMast.EditQuotationMast(PK_QM_ID);//Edit Quotation
                if (DTEditQuotationMast.Rows.Count > 0)
                {
                    ObjModelQuotationMast.Visible = Convert.ToString(DTEditQuotationMast.Rows[0]["Visible"]);
                    ObjModelQuotationMast.VisibleReport = Convert.ToString(DTEditQuotationMast.Rows[0]["VisibleReport"]);

                    ObjModelQuotationMast.Editable = Convert.ToString(DTEditQuotationMast.Rows[0]["Editable"]);
                    //ObjModelQuotationMast.PCHEditable = Convert.ToString(DTEditQuotationMast.Rows[0]["PCHEditable"]);
                    ObjModelQuotationMast.DCostSheetApproveStatus = Convert.ToString(DTEditQuotationMast.Rows[0]["DCostSheetApproveStatus"]);

                    ObjModelQuotationMast.SendForApprovel = Convert.ToString(DTEditQuotationMast.Rows[0]["SendForApprovel"]);
                    ObjModelQuotationMast.PK_QTID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["PK_QTID"]);
                    ObjModelQuotationMast.QuotationNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["QuotationNumber"]);
                    ObjModelQuotationMast.EQ_ID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["EQ_ID"]);
                    ObjModelQuotationMast.QuotationCompanyName = Convert.ToString(DTEditQuotationMast.Rows[0]["CompanyName"]);
                    ObjModelQuotationMast.ContactName = Convert.ToString(DTEditQuotationMast.Rows[0]["ContactName"]);
                    ObjModelQuotationMast.QuotationBranch = Convert.ToInt32(DTEditQuotationMast.Rows[0]["BranchName"]);
                    ObjModelQuotationMast.ServiceType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ServiceType"]);
                    ObjModelQuotationMast.ProjectType = Convert.ToInt32(DTEditQuotationMast.Rows[0]["ProjectType"]);
                    ObjModelQuotationMast.FK_CMP_ID = Convert.ToInt32(DTEditQuotationMast.Rows[0]["FK_CMP_ID"]);
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
                    ObjModelQuotationMast.Budgetary = Convert.ToString(DTEditQuotationMast.Rows[0]["Budgetary"]);  //added by nikita on 17062024

                    ObjModelQuotationMast.Source = Convert.ToString(DTEditQuotationMast.Rows[0]["Source"]); //added by nikita on 24062024
                    ObjModelQuotationMast.EnquirySource = Convert.ToString(DTEditQuotationMast.Rows[0]["Enquirysource"]); //added by nikita on 24062024


                    ObjModelQuotationMast.EstimatedAmount = Convert.ToString(DTEditQuotationMast.Rows[0]["EstimatedAmount"]);
                    ObjModelQuotationMast.LostReason = Convert.ToString(DTEditQuotationMast.Rows[0]["LostReason"]);
                    ObjModelQuotationMast.IData = (byte[])(DTEditQuotationMast.Rows[0]["IData"]);

                    ObjModelQuotationMast.DLostReason = Convert.ToString(DTEditQuotationMast.Rows[0]["DLostReason"]);
                    ObjModelQuotationMast.ILostReason = Convert.ToString(DTEditQuotationMast.Rows[0]["ILostReason"]);

                    ObjModelQuotationMast.EnquiryAdditionRef = Convert.ToString(DTEditQuotationMast.Rows[0]["EnquiryAdditionRef"]);
                    ObjModelQuotationMast.JobBlock = Convert.ToString(DTEditQuotationMast.Rows[0]["JobBlock"]);
                    ObjModelQuotationMast.Followupddate = Convert.ToString(DTEditQuotationMast.Rows[0]["FollowUpdate"]);
                    ObjModelQuotationMast.DoNotFollowUP = Convert.ToBoolean(DTEditQuotationMast.Rows[0]["DoNotFollowUP"]);

                    ObjModelQuotationMast.DeleteReason = Convert.ToString(DTEditQuotationMast.Rows[0]["DeleteReason"]);
                    ObjModelQuotationMast.BudgetaryQuotation = Convert.ToBoolean(DTEditQuotationMast.Rows[0]["CarryForwardBudget"]);
                    ObjModelQuotationMast.FurtherAction = Convert.ToString(DTEditQuotationMast.Rows[0]["ActionCollectFromCustomer"]);
                    ObjModelQuotationMast.TiimesEnquiryNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["TiimesEnquiryNumber"]);
                    ObjModelQuotationMast.ValidityExpireStatus = Convert.ToString(DTEditQuotationMast.Rows[0]["expire"]);
                    ObjModelQuotationMast.wonlost_date = Convert.ToString(DTEditQuotationMast.Rows[0]["wonlost_date"]);

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


                    ObjModelQuotationMast.chkArc = Convert.ToBoolean(DTEditQuotationMast.Rows[0]["chkArc"]);
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
                    ObjModelQuotationMast.IsConfirmation = Convert.ToBoolean(DTEditQuotationMast.Rows[0]["IsConfirmation"]);
                    ObjModelQuotationMast.DomesticLostDescription = Convert.ToString(DTEditQuotationMast.Rows[0]["LostDescription_Dom"]);
                    ObjModelQuotationMast.InterNationalLostDescription = Convert.ToString(DTEditQuotationMast.Rows[0]["LostDescription_inter"]);
                    ObjModelQuotationMast.ValidityDate = Convert.ToString(DTEditQuotationMast.Rows[0]["ValidityDate"]);
                    if (string.IsNullOrEmpty(ObjModelQuotationMast.validityNumber))
                    {
                        ObjModelQuotationMast.validityNumber = "90";
                    }
                    else
                    {
                        ObjModelQuotationMast.validityNumber = Convert.ToString(DTEditQuotationMast.Rows[0]["ValidityNumber"]);
                    }


                    if (ObjModelQuotationMast.Status == 5)
                    {
                        ObjModelQuotationMast.Revise = "Revise";
                    }
                    else
                    {
                        ObjModelQuotationMast.Revise = "";
                    }

                    if (Convert.ToString(DTEditQuotationMast.Rows[0]["ExclusionCheckBox"]) == "1")
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


                    ///Finance Approval 
                    if (Convert.ToInt32(ObjModelQuotationMast.EstimatedAmount) >= 17000000)
                    {
                        if (ViewBag.EditproductName.Count > 1)
                        {
                            /// Domestic
                            DataTable DTFINEditQuotationMast = new DataTable();
                            DTFINEditQuotationMast = objDALQuotationMast.GetFINApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.CostSheetFINStatus = DTFINEditQuotationMast.Rows[0]["FinanceStatus"].ToString();

                            ///// International
                            //DataTable DTIFINEditQuotationMast = new DataTable();
                            //DTIFINEditQuotationMast = objDALQuotationMast.GetIFINApprovalStatus(PK_QM_ID);
                            //ObjModelQuotationMast.ICostSheetFINStatus = DTIFINEditQuotationMast.Rows[0]["FinanceStatus"].ToString();

                            ObjModelQuotationMast.ICostSheetFINStatus = DTFINEditQuotationMast.Rows[0]["FinanceStatus"].ToString();

                        }
                        else if (ViewBag.EditproductName.Contains("2"))
                        {
                            DataTable DTFINEditQuotationMast = new DataTable();
                            DTFINEditQuotationMast = objDALQuotationMast.GetFINApprovalStatus(PK_QM_ID);
                            ObjModelQuotationMast.CostSheetFINStatus = DTFINEditQuotationMast.Rows[0]["FinanceStatus"].ToString();
                        }
                    }

                    if (Convert.ToInt32(ObjModelQuotationMast.EstimatedAmount) >= 1000000 || ObjModelQuotationMast.ProjectType == 9)
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
                //if (DTGetUploadedFile.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in DTGetUploadedFile.Rows)
                //    {
                //        lstEditFileDetails.Add(
                //           new FileDetails
                //           {

                //               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                //               FileName = Convert.ToString(dr["FileName"]),
                //               Extension = Convert.ToString(dr["Extenstion"]),
                //               IDS = Convert.ToString(dr["FileID"]),
                //           }
                //         );
                //    }
                //    ViewData["lstEditFileDetails"] = lstEditFileDetails;
                //    ObjModelQuotationMast.FileDetails = lstEditFileDetails;
                //}
                //added by nikita on 25062024
                if (DTGetUploadedFile.Rows.Count > 0)
                {
                    List<FileDetails> lstEditLegalFileDetails = new List<FileDetails>();
                    List<FileDetails> lstEditOtherFileDetails = new List<FileDetails>();
                    foreach (DataRow dr in DTGetUploadedFile.Rows)
                    {
                        FileDetails fileDetails = new FileDetails
                        {

                            PK_ID = Convert.ToInt32(dr["PK_ID"]),
                            FileName = Convert.ToString(dr["FileName"]),
                            Extension = Convert.ToString(dr["Extenstion"]),
                            IDS = Convert.ToString(dr["FileID"]),
                            AttachmentType = Convert.ToString(dr["Type"]),

                        };

                        if (fileDetails.AttachmentType == "Tender")
                        {
                            lstEditLegalFileDetails.Add(fileDetails);
                        }
                        else
                        {
                            lstEditOtherFileDetails.Add(fileDetails);
                        }
                        if (lstEditLegalFileDetails.Count > 0)
                        {
                            ViewData["lstEditFileDetails"] = lstEditLegalFileDetails;
                            ObjModelQuotationMast.FileDetails_ = lstEditLegalFileDetails;
                        }

                        // Set ViewData and ObjModelEnquiry for other files
                        if (lstEditOtherFileDetails.Count > 0)
                        {
                            ViewData["lstEditOtherFileDetails"] = lstEditOtherFileDetails;
                            ObjModelQuotationMast.FileDetails = lstEditOtherFileDetails;
                        }
                    }


                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file
                //#region Bind Company Addr
                //DataSet DsCompanyAddr = new DataSet();
                //List<NameCode> lstCompanyAddr = new List<NameCode>();

                //DsCompanyAddr = objDALQuotationMast.GetCompanyAddr(Convert.ToInt32(ObjModelQuotationMast.EQ_ID));
                ////DsCompanyAddr = objDALQuotationMast.GetCompanyAddr(Convert.ToInt32(ObjModelQuotationMast.CompanyAddress));
                //if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                //{
                //    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                //                      select new NameCode()
                //                      {
                //                          Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                //                          Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                //                      }).ToList();
                //}

                //IEnumerable<SelectListItem> ComAddrItems;
                //ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

                ////ViewBag.ComAddr = ComAddrItems;
                //ViewData["ComAddr1"] = ComAddrItems;
                //#endregion

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
                    ObjModelQuotationMast.Enquiry = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryDescription"]);
                    ObjModelQuotationMast.Status = Convert.ToInt32(DTGetEnquiryDtls.Rows[0]["Status"]);
                    ObjModelQuotationMast.Statusname = Convert.ToString(DTGetEnquiryDtls.Rows[0]["StatusName"]);
                    ObjModelQuotationMast.EnquiryNumber = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryNumber"]);
                    ObjModelQuotationMast.FK_CMP_ID = Convert.ToInt32(DTGetEnquiryDtls.Rows[0]["FK_CMP_ID"]);
                    ObjModelQuotationMast.CompanyAddress = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Address"]);
                    ObjModelQuotationMast.chkArc = Convert.ToBoolean(DTGetEnquiryDtls.Rows[0]["chkArc"]);
                    ObjModelQuotationMast.InspectionLocation = Convert.ToString(DTGetEnquiryDtls.Rows[0]["InspectionLocation"]);

                    ObjModelQuotationMast.Budgetary = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Budgetary"]);  //added by nikita on 17062024
                    ObjModelQuotationMast.Source = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Source"]); //added by nikita on 24062024
                    ObjModelQuotationMast.EnquirySource = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Enquirysource"]); //added by nikita on 24062024

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

                //#region Domestic Order Type 
                //DataTable dtDOrderType = new DataTable();
                //List<QuotationMaster> lstDOrderType = new List<QuotationMaster>();

                //dtDOrderType = objDalEnquiryMaster.DOrderType(PK_EQID);
                //if (dtDOrderType.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dtDOrderType.Rows)
                //    {
                //        lstDOrderType.Add(
                //           new QuotationMaster
                //           {

                //               OrderType = Convert.ToString(dr["OrderType"]),
                //               OrderRate = Convert.ToString(dr["OrderRate"]),
                //               Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                //               Distance = Convert.ToString(dr["Distance"]),
                //               EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                //               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                //               DExchangeRate = Convert.ToString(dr["DExchangeRate"]),
                //               DTotalAmount = Convert.ToString(dr["DTotalAmount"]),
                //           }
                //         );
                //    }
                //    ViewBag.lstDOrderType = lstDOrderType;

                //}
                //#endregion


                //#region International Order Type 
                //DataTable dtIOrderType = new DataTable();
                //List<QuotationMaster> lstIOrderType = new List<QuotationMaster>();

                //dtIOrderType = objDalEnquiryMaster.IOrderType(PK_EQID);
                //if (dtIOrderType.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dtIOrderType.Rows)
                //    {
                //        lstIOrderType.Add(
                //           new QuotationMaster
                //           {

                //               IOrderType = Convert.ToString(dr["IOrderType"]),
                //               IOrderRate = Convert.ToString(dr["IOrderRate"]),
                //               IEstimate_ManDays_ManMonth = Convert.ToString(dr["IEstimate_ManDays_ManMonth"]),
                //               IDistance = Convert.ToString(dr["IDistance"]),
                //               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                //               Icurrency = Convert.ToString(dr["Icurrency"]),
                //               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                //               ITotalAmount = Convert.ToString(dr["ITotalAmount"]),
                //           }
                //         );
                //    }
                //    ViewBag.lstIOrderType = lstIOrderType;
                //}

                //#endregion



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

                //#region Bind Company Addr
                ////DataSet DsCompanyAddr = new DataSet();
                ////List<NameCode> lstCompanyAddr = new List<NameCode>();

                //DsCompanyAddr = objDALQuotationMast.GetCompanyAddr(PK_EQID);
                //if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                //{
                //    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                //                          select new NameCode()
                //                          {
                //                              Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                //                              Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                //                          }).ToList();
                //}

                ////IEnumerable<SelectListItem> ComAddrItems;
                //ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

                ////ViewBag.ComAddr = ComAddrItems;
                //ViewData["ComAddr1"] = ComAddrItems;
                //#endregion


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
        public ActionResult Quotation(QuotationMaster QM, FormCollection fc, string C, string ExclusionCheckBox, List<DCurrency> DArray, List<ICurrency> IArray, string GeneralTermsCheckbox)
        {
            string Result = string.Empty;
            string Status = string.Empty;
            string IPath = string.Empty;
            int QuotationID = 0;
            if (C == "1")
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

            //added by nikita on 24062024

            List<FileDetails> lstFileDtls_ = new List<FileDetails>();
            lstFileDtls_ = Session["listUploadedFile_"] as List<FileDetails>;

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
                    //else if (QM.DomStatus == 10)
                    //{

                    //    QM.Status = 10;
                    //}
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
                    //else if (QM.IntStatus == 10)
                    //{

                    //    QM.Status = 10;
                    //}
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
                    //else if (QM.DomStatus == 10 && QM.IntStatus == 10)
                    //{
                    //    QM.Status = 10; 
                    //}
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
                                QM.ManDaysRate = d.ManDaysRate;
                                QM.ActualManDays = d.ActualManDays;
                                QM.DRemarks = d.DRemark;
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
                                QM.IManDaysRate = d.IManDaysRate;
                                QM.IActualManDays = d.IActualManDays;
                                QM.Type = "I";
                                Result = objDALQuotationMast.InsertUpdateIOrderType(QM);
                            }
                        }


                        #endregion
                    }
                    //  GeneratePDF(QM); //Digital Signature
                    #endregion

                    //string PPPP = Server.MapPath("~/QuotationHtml/" + ObjModelQuotationMast.QuotationPDF);// newpath + finalReportName;
                    //string Path = PPPP; string SignLoc = "Vaibhav:"; /*string SignLoc = "TUV India representative:";*/
                    //string signannotation = ObjModelQuotationMast.FaitFully; string PReportName = ObjModelQuotationMast.QuotationPDF; string QuotationNumber = QM.QuotationNumber; int PK_QM_ID = QM.PK_QTID;

                    #region Digital Signature
                    //int iiv = Convert.ToInt32(QM.IsConfirmation); 
                    //if (Convert.ToInt32(QM.IsConfirmation) == 1)

                    //{



                    //        var redirectUrl = Url.Action("QuotationReportPrintWithDigitalSign", new
                    //        {
                    //            Path = Path,
                    //            SignLoc = SignLoc,
                    //            signannotation = signannotation,
                    //            QuotationNumber = QuotationNumber,
                    //            PK_QM_ID = PK_QM_ID
                    //        });

                    //        return Json(new { result = "Redirect", url = redirectUrl });
                    //}
                    //else
                    //{
                    //    return Json(new { result = "Redirect", url = Url.Action("Quotation", "QuotationMaster", new { @QuotationNumber = QM.QuotationNumber, @PK_QM_ID = QuotationID }) });
                    //}



                    //return RedirectToAction(nameof(QuotationReportPrintWithDigitalSign), new { Path = Path, SignLoc = SignLoc, signannotation = signannotation, QuotationNumber = QuotationNumber, PK_QM_ID = PK_QM_ID });
                    #endregion


                    if (QuotationID != null && QuotationID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            string type = null;
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, QuotationID);
                            Result = objDALQuotationMast.InsertFileAttachment(lstFileDtls, QuotationID, type);
                            Session["listQuotationUploadedFile"] = null;
                        }
                        //added by nikita on 25062024
                        if (lstFileDtls_ != null && lstFileDtls_.Count > 0)//added by nikita on 24062024
                        {
                            string type = "Tender";
                            Result = objDALQuotationMast.InsertFileAttachment(lstFileDtls_, QuotationID, type);
                            Session["listUploadedFile_"] = null;
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
                            QM.ManDaysRate = d.ManDaysRate;
                            QM.ActualManDays = d.ActualManDays;
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
                            QM.IManDaysRate = d.IManDaysRate;
                            QM.IActualManDays = d.IActualManDays;
                            QM.Type = "I";
                            Result = objDALQuotationMast.InsertUpdateIOrderType(QM);
                        }
                    }


                    #endregion


                    if (QuotationID != null && QuotationID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            string type = null;
                            Result = objDALQuotationMast.InsertFileAttachment(lstFileDtls, QuotationID, type);
                            Session["listQuotationUploadedFile"] = null;
                        }
                        //added by nikita on 25062024
                        if (lstFileDtls_ != null && lstFileDtls_.Count > 0)//added by nikita on 24062024
                        {
                            string type = "Tender";
                            Result = objDALQuotationMast.InsertFileAttachment(lstFileDtls_, QuotationID, type);
                            Session["listUploadedFile_"] = null;
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
                            QM.ManDaysRate = d.ManDaysRate;
                            QM.ActualManDays = d.ActualManDays;
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
                            QM.IManDaysRate = d.IManDaysRate;
                            QM.IActualManDays = d.IActualManDays;
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
                            string type = null;
                            Result = objDALQuotationMast.InsertFileAttachment(lstFileDtls, QuotationID, type);
                            Session["listQuotationUploadedFile"] = null;

                        }
                        if (lstFileDtls_ != null && lstFileDtls_.Count > 0) //added by nikita on 24062024
                        {
                            string type = "Tender";
                            Result = objDALQuotationMast.InsertFileAttachment(lstFileDtls_, QuotationID, type);
                            Session["listUploadedFile_"] = null;
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

        #region Digital Signatue

        public async Task<ActionResult> QuotationReportPrintWithDigitalSign(string Path, string SignLoc, string signannotation, string PReportName, string QuotationNumber, int PK_QM_ID)
        {
            // Your existing code for generating the PDF...
            string ReportName = PReportName;
            string path = Path;//Server.MapPath(Path);
            //string pdfFilePath = Path.Combine(path, ReportName);
            //string base64Pdf = ConvertPdfToBase64(pdfFilePath);
            //string XYAxis = "[10:-50]";
            string XYAxis = "[5:-50]";
            CommonControl commonControl = new CommonControl();
            string signedFilePath = await commonControl.SignPdfWithDigitalSignature(path, SignLoc, signannotation, "samco_tuv", "samco", "MWMBIGHE", XYAxis, "Quot" + PK_QM_ID);


            // Step 3: Return the signed PDF or handle the result
            if (!signedFilePath.StartsWith("Error"))
            {
                // Successfully signed, return the signed PDF
                // return Json(new { redirect = Url.Action("Quotation", new { QuotationNumber = QuotationNumber, PK_QM_ID = PK_QM_ID }) });

                //return View("Quotation", new { QuotationNumber = QuotationNumber, PK_QM_ID = PK_QM_ID });
                //return RedirectToAction("Quotation", new { QuotationNumber = QuotationNumber, PK_QM_ID = PK_QM_ID });
                return File(System.IO.File.ReadAllBytes(signedFilePath), "application/pdf", ReportName);
            }
            else
            {
                // Handle error
                return Content("Failed to sign the PDF: " + signedFilePath);
            }
        }
        #endregion



        public JsonResult TemporaryFilePathQuotationAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment

        {

            var IPath = string.Empty;

            string[] splitedGrp;

            List<string> Selected = new List<string>();

            //Adding New Code 12 March 2020

            List<FileDetails> fileDetails = new List<FileDetails>();
            //added by nikita on 24062024
            List<FileDetails> fileDetailsFormat = new List<FileDetails>();
            //---Adding end Code

            if (Session["listQuotationUploadedFile"] != null)

            {

                fileDetails = Session["listQuotationUploadedFile"] as List<FileDetails>;

            }
            if (Session["listUploadedFile_"] != null)

            {

                fileDetails = Session["listUploadedFile_"] as List<FileDetails>;

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



                            FileDetails fileDetail = new FileDetails();

                            fileDetail.FileName = fileName;

                            fileDetail.Extension = Path.GetExtension(fileName);

                            fileDetail.Id = Guid.NewGuid();

                            BinaryReader br = new BinaryReader(files.InputStream);
                            byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                            fileDetail.FileContent = bytes;
                            if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                            {
                                fileDetailsFormat.Add(fileDetail);

                            }
                            if (Request.Files.Keys[0].ToString().ToUpper() == "FILEPOND")
                            {
                                fileDetails.Add(fileDetail);

                            }

                            //fileDetails.Add(fileDetail);                            

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

                //if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                //{
                //    Session["listUploadedFile_"] = fileDetailsFormat;

                //}
                //else
                //{

                //}

                if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                {
                    Session["listUploadedFile_"] = fileDetailsFormat;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEPOND")
                {
                    Session["listQuotationUploadedFile"] = fileDetails;
                }

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
                        //body = body.Replace("[ProjectName]", "Project Name :" + ObjModelQuotationMast.Quotation_Description);
                        body = body.Replace("[ProjectName]", ObjModelQuotationMast.Quotation_Description);
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
                    //string I = "<img src = 'https://tiimes.tuv-india.com/Content/Sign/" + ObjModelQuotationMast.Signature + "' style='width:100px;height:50px; ' align='center'>";
                    //string I = "<img src = 'https://tiimes.tuv-india.com/Content/Sign/" + ObjModelQuotationMast.Signature + "' style='width:100px;height:50px; ' align='center'>";
                    string I = "<img src = 'https://tiimes.tuv-india.com/Content/Sign/" + "" + "' style='width:100px;height:50px; ' align='center'>";


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
                        //body = body.Replace("[EndCustomer]", "End Customer :" + ObjModelQuotationMast.EndCustomer);
                        body = body.Replace("[EndCustomer]", ObjModelQuotationMast.EndCustomer);
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



        //public void GeneratePDF(QuotationMaster QM)
        public ActionResult GeneratePDF(QuotationMaster QM)
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
                    ObjModelQuotationMast.Budgetary = Convert.ToString(DTPrintQuotationDtls.Rows[0]["Budgetary"]); //added by nikita on 20062024
                    ObjModelQuotationMast.IsConfirmation = Convert.ToBoolean(DTPrintQuotationDtls.Rows[0]["IsConfirmation"]);//Digital signature
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
                    string URl = "http://localhost:54895/QuotationServicesLink/Servicess?PKServiceId=" + ObjModelQuotationMast.SubServiceType + "";

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
                        body = body.Replace("[ProjectName]", ObjModelQuotationMast.Quotation_Description);
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
                    //string I = "<img src = 'https://tiimes.tuv-india.com/Content/Sign/" + ObjModelQuotationMast.Signature + "' style='width:100px;height:50px; ' align='center'>";
                    string I = "<img src = 'https://tiimes.tuv-india.com/Content/Sign/" + "" + "' style='width:100px;height:50px; ' align='center'>";


                    body = body.Replace("[Signature]", I);//Digital Signature

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
                        body = body.Replace("[EndCustomer]", ObjModelQuotationMast.EndCustomer);
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
                    if (ObjModelQuotationMast.IsConfirmation == true)
                    {
                        _Header = _Header.Replace("[logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.svg");
                    }
                    else
                    {
                        _Header = _Header.Replace("[logo]", "");
                    }

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



                    if (ObjModelQuotationMast.Budgetary == "1")
                    {

                        string imgFile1 = Server.MapPath("/Budetary.png");
                        PdfTemplate template1 = doc.AddTemplate(doc.Pages[0].ClientRectangle);
                        PdfImageElement img1 = new PdfImageElement(50, 50, imgFile1);
                        img1.Transparency = 15;
                        template1.Add(img1);
                    }
                    else
                    {



                        if (ObjModelQuotationMast.IsConfirmation == true)
                        {

                        }
                        else
                        {
                            string imgFile1 = Server.MapPath("/WaterMark.png");
                            PdfTemplate template1 = doc.AddTemplate(doc.Pages[0].ClientRectangle);
                            PdfImageElement img1 = new PdfImageElement(125, 85, imgFile1);
                            img1.Transparency = 15;
                            template1.Add(img1);
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


            #region Digital Signature

            string PPPP = Server.MapPath("~/QuotationHtml/" + ObjModelQuotationMast.QuotationPDF);// newpath + finalReportName;
            string Path = PPPP; string SignLoc = "For TUV India"; /*string SignLoc = "TUV India representative:";*/
            string signannotation = ObjModelQuotationMast.FaitFully; string PReportName = ObjModelQuotationMast.QuotationPDF; string QuotationNumber = QM.QuotationNumber; int PK_QM_ID = ObjModelQuotationMast.PK_QTID;


            //int iiv = Convert.ToInt32(QM.IsConfirmation);
            //if (Convert.ToInt32(QM.IsConfirmation) == 1)
            if (ObjModelQuotationMast.IsConfirmation == true)
            {



                //var redirectUrl = Url.Action("QuotationReportPrintWithDigitalSign", new
                //{
                //    Path = Path,
                //    SignLoc = SignLoc,
                //    signannotation = signannotation,
                //    QuotationNumber = QuotationNumber,
                //    PK_QM_ID = PK_QM_ID
                //});
                return RedirectToAction(nameof(QuotationReportPrintWithDigitalSign), new { Path = Path, SignLoc = SignLoc, signannotation = signannotation, PReportName = PReportName, PK_QM_ID = PK_QM_ID });
                //return File(System.IO.File.ReadAllBytes(PPPP), "application/pdf", ObjModelQuotationMast.QuotationPDF);
                //                return Json(new { result = "Redirect", url = redirectUrl });
            }
            else
            {
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "draft.pdf");
                //return Json(new { result = "Redirect", url = Url.Action("Quotation", "QuotationMaster", new { @QuotationNumber = QM.QuotationNumber, @PK_QM_ID = QuotationID }) });
            }



            //return RedirectToAction(nameof(QuotationReportPrintWithDigitalSign), new { Path = Path, SignLoc = SignLoc, signannotation = signannotation, QuotationNumber = QuotationNumber, PK_QM_ID = PK_QM_ID });
            #endregion

        }

        #region Delete Code By Rahul 
        public ActionResult DeleteQuotation(int? PK_QTID, string reason)
        {
            int Result = 0;
            try
            {
                Result = objDALQuotationMast.DeleteQuotation(PK_QTID, reason);
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
                        string a = CommonControl.FileUpload("CoverImages/", single);
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
        public ActionResult SendQuotation(int PK_QTID, string QuotationCompanyName, string ContactName, int FK_CMP_ID)
        {
            ObjModelQuotationMast.PK_QTID = PK_QTID;
            ObjModelQuotationMast.QuotationCompanyName = QuotationCompanyName;
            ObjModelQuotationMast.ContactName = ContactName;
            ObjModelQuotationMast.CMP_ID_ = Convert.ToString(FK_CMP_ID);
            var Contacts_client = ObjModelQuotationMast.CMP_ID_?.Split(',').Select(c => new SelectListItem
            {
                Value = c,
                Text = c
            }).ToList() ?? new List<SelectListItem>();

            ViewBag.Contacts_client = Contacts_client;
            return View(ObjModelQuotationMast);

        }

        public ActionResult GetAttachmentData_(int pk_ivr_id)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = objDALQuotationMast.GetAttachmentsData_(pk_ivr_id);
                foreach (DataRow dr in dt.Rows)
                {
                    var fileName = dr["FileName"] != DBNull.Value ? dr["FileName"].ToString() : null;
                    //var type = dr["Type"] != DBNull.Value ? dr["Type"].ToString() : null;
                    //DateTime? createdDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null;

                    if (fileName != null)
                    {
                        //CopyFileToserver(fileName, type, pk_ivr_id, createdDate.Value);
                    }
                    else
                    {
                        // Handle the case where fileName, type, or createdDate is null if needed
                        // You can log or take some action here if null values are not expected
                    }
                }

            }
            catch (Exception ex)
            {
                string mesage = ex.Message;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            var result = JsonConvert.SerializeObject(dt);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDataQuotation(int pk_ivr_id)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = objDALQuotationMast.GetEmaildata(pk_ivr_id);


            }
            catch (Exception ex)
            {
                string mesage = ex.Message;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            var Response = JsonConvert.SerializeObject(dt);
            return Json(Response, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public JsonResult UpdateClientTable(int? PK_QTID, string ClientName, string getCompanyName)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = objDALQuotationMast.UpdateClientTable(PK_QTID, ClientName, getCompanyName);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateTUVMail(string Email, int PK_QTID)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = objDALQuotationMast.UpdateTuvMail(Email, PK_QTID);


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            var Response = JsonConvert.SerializeObject(dt);
            return Json(Response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SendQuotation(string emails, string attachments, string pk_call_id, string mailsubject, string MailBody, string selectedPaths, string fileType, string Signature)
        {
            string displayName = string.Empty;
            string ClientEmail = string.Empty;
            string bodyTxt = string.Empty;
            string Mailsubject = mailsubject;
            string CcEmail = "";
            string ToEmail = "";
            var status = "";
            string MailBody_ = "";
            try
            {
                var signature = HttpUtility.UrlDecode(Signature);
                MailBody_ = MailBody.Replace("\r\n", "<br/>").Replace("\n", "<br/>");
                DataTable dt = new DataTable();
                dt = objDALQuotationMast.GetDetails(pk_call_id);
                var useremailid = dt.Rows[0]["useremailid"].ToString();
                var ApprovalOneEmail = dt.Rows[0]["ApprovalOneEmail"].ToString();
                var ApprovalTwoEmail = dt.Rows[0]["ApprovalTwoEmail"].ToString();
                var PChEmail = dt.Rows[0]["PChEmail"].ToString();
                string CcExtra = "rohini@tuv-nord.com";
                string CCnikita = "nikita.yadav@tuvindia.co.in";
                string CCmails = "pshrikant@tuv-nord.com";
                MailMessage msg = new MailMessage();
                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                //ToEmail = emails;
                //CcEmail = $"{CcExtra};{CCnikita};{CCmails};{useremailid};{ApprovalOneEmail};{ApprovalTwoEmail};{PChEmail}";
                ToEmail = "";
                CcEmail = $"{CcExtra};{CCnikita};{CCmails};{useremailid};{ApprovalOneEmail};{ApprovalTwoEmail};{PChEmail}";
                var emailList = emails.Split(';');
                List<string> tuvEmails = new List<string>();
                List<string> nonTuvEmails = new List<string>();

                foreach (var email in emailList)
                {
                    if (email.IndexOf("tuv", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        tuvEmails.Add(email);
                    }
                    else
                    {
                        nonTuvEmails.Add(email);
                    }
                }
                ToEmail = string.Join(";", nonTuvEmails);
                CcEmail += ";" + string.Join(";", tuvEmails);
                var attachmentLinks = string.Empty;
                if (!string.IsNullOrEmpty(attachments))
                {
                    var attachmentUrls = attachments.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    //var selectedPaths = attachments.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    //var attachmentUrls = attachments.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var url in attachmentUrls)

                    {
                        var decodedPath = HttpUtility.UrlDecode(url); // Decode the path if needed

                        //if(fileType=="Quotation")
                        //var filePath = Server.MapPath($"~/QuotationHtml/{decodedPath}");
                        var filePath = Server.MapPath($"~/{decodedPath}");

                        // First check in the main QuotationHtml folder
                        if (System.IO.File.Exists(filePath))
                        {
                            var attachment = new Attachment(filePath);
                            msg.Attachments.Add(attachment);
                        }
                        else
                        {
                            // If not found, search recursively in the Content folder
                            var contentFolder = Server.MapPath("~/Content/");
                            var searchPattern = Path.GetFileName(decodedPath); // Get the file name only

                            // Search for the file in all subdirectories
                            var foundFiles = Directory.GetFiles(contentFolder, searchPattern, SearchOption.AllDirectories);

                            if (foundFiles.Length > 0)
                            {
                                foreach (var foundFile in foundFiles)
                                {
                                    var attachment = new Attachment(foundFile);
                                    msg.Attachments.Add(attachment);

                                    // Optionally, you can break after the first found file if you only want to attach one
                                    // break;
                                }
                            }
                        }
                    }

                }
                if (string.IsNullOrEmpty(MailBody))
                {
                    bodyTxt = $@"
            <html>
                <head>
                    <title></title>
                </head>
                <body>
                    <div>
                        <span style='font-size:14px; font-family:Arial;color: #00003C;'>Dear Sir / Madam,</span>
                        <br/><br/>
                        <span style='font-size:14px; font-family:Arial;color: #00003C;'>Quotation Report are available on below link. link. </span>
                        <br/>
                        <br/>
                        {attachmentLinks}
                        <br/>
                         <br/><br/>
                        <span style='font-size:14px; font-family:Arial;font-style: italic;color: #FA3756;'>(Please note: The above links will expire 7 days after the date of this email).</span>

                        <br/><br/>
                       
                        <div>
                        <span style='font-size:14px; font-family:Arial;font-style: italic;color:#FA3756'>This is an automated email. Please do not reply.</span>
                        <br/>
                       </div>
                        <br/><br/>
                      
                    <div>
                        <span style='font-size:15px; font-family:Arial; font-weight:bold;color: #00003C;'>Best Regards,</span>
                      <br/>
                        <span style='font-size:14px; font-family:Arial;font-weight:bold;color: #00003C;'>TUVI</span>

                    </div>
                    <br/>
                </body>
            </html>";
                }
                else
                {
                    //bodyTxt = MailBody_ + "<br/><br/>" + attachmentLinks; // Use the provided MailBody if not empty
                    MailBody_ = $"<div style='font-size: 15px; color: #000C54;font-family: Arial;'>{MailBody.Replace("\r\n", "<br/>").Replace("\n", "<br/>")}</div>";
                    //bodyTxt = MailBody_ + "<br/><br/>" +signature + attachmentLinks; // Use the provided MailBody if not empty
                    //}

                    bodyTxt = MailBody_ + "<br/><br/>" + signature + "<br/><br/>" + msg.Attachments;
                }
                foreach (var email in ToEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.To.Add(new MailAddress(email));
                }
                foreach (var email in CcEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.CC.Add(new MailAddress(email));
                }
                msg.From = new MailAddress(MailFrom, ClientEmail);
                if (!string.IsNullOrEmpty(mailsubject))
                {
                    msg.Subject = mailsubject;
                }
                else
                {
                    msg.Subject = $"Quotationtest";
                }
                msg.Body = bodyTxt;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                client.Host = ConfigurationManager.AppSettings["smtpserver"];
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"], ConfigurationManager.AppSettings["Password"]);
                client.EnableSsl = true;
                client.Send(msg);
                updateMailFlag(pk_call_id);
                status = "Email Send Successfully";
                objDALQuotationMast.InserDataIntoHistory(pk_call_id, ToEmail, CcEmail, attachments, Mailsubject, MailBody_, status);

                //UpdateMailFlag(Convert.ToInt32(pk_call_id));
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
                string mssg = ex.Message;

                objDALQuotationMast.InserDataIntoHistory(pk_call_id, ToEmail, CcEmail, attachments, Mailsubject, MailBody_, mssg);

            }
        }

        [HttpPost]
        public JsonResult GetLeadByName(string Prefix)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();
            if (Prefix != null && Prefix != "")
            {
                DTResult = objDALQuotationMast.GetUserName(Prefix);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {
                               CompanyName = Convert.ToString(dr["CompanyName"]),
                               CompanyNames = Convert.ToString(dr["CompanyNames"]),
                               PkUserID = Convert.ToString(dr["pk_UserID"]),
                           }
                         );
                    }
                    Session["CompanyNames"] = Convert.ToString(DTResult.Rows[0]["CompanyNames"]);
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateClientTableExisting(int? PK_QTID, string[] ClientName, string getCompanyName)
        {
            try
            {
                DataTable dt = new DataTable();
                foreach (var email in ClientName)
                {
                    dt = objDALQuotationMast.UpdateClientTable_Existing(PK_QTID, email, getCompanyName);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public void updateMailFlag(string pk_call_id)
        {
            DataTable dt = new DataTable();
            dt = objDALQuotationMast.UpdateMail_Flag(pk_call_id);
        }

        public ActionResult UpdateFollowUp(string followUpDate, string details, string nextFollowUpDate, int Pk_QTID)
        {

            DataTable dt = new DataTable();
            dt = objDALQuotationMast.UpdateQuotationFollowUPTable(followUpDate, details, nextFollowUpDate, Pk_QTID);
            //return View();
            return Json(new { result = "Redirect", url = Url.Action("Quotation", "QuotationMaster", new { @PK_QM_ID = Pk_QTID }) });
        }

        [HttpGet]
        public ActionResult ViewPDF(string pdfFilePath, QuotationMaster QM)
        {


            GeneratePDF(QM);

            ViewBag.PdfFilePath = pdfFilePath;

            return View();
        }


        [HttpPost]
        public ActionResult AddCityDetails(string CityName)
        {
            string Result = string.Empty;

            try
            {

                if (CityName != "" && CityName != null)
                {
                    DataTable dtContactDetailExist = new DataTable();

                    dtContactDetailExist = objDalEnquiryMaster.ChkCityExistFromEnquiry(CityName);

                    if (dtContactDetailExist.Rows.Count > 0)//chk duplication
                    {
                        return Json(new { success = 2, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                    }
                    else//Insert
                    {
                        Result = objDALQuotationMast.InsertCity(CityName);
                        if (Result != null && Result != "")
                        {
                            return Json(new { success = 1, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                else
                {
                    return Json(new { success = 2, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(new { success = 2, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCityListValue()
        {
            DataSet DSGetEditQuotationAllddllst = new DataSet();
            DSGetEditQuotationAllddllst = objDALQuotationMast.GetEditAllddlLst();
            var cities = DSGetEditQuotationAllddllst.Tables[5].AsEnumerable()
        .Select(row => new
        {
            CityName = row.Field<string>("CityName"),
            PK_ID = row.Field<int>("PK_ID")
        }).ToList();

            return Json(cities, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ValidateEnquiryNumber(string EnquiryNumber)
        {
            DataTable dt = new DataTable();
            var data = "";
            try
            {
                dt= objDALQuotationMast.ValidateEnquiryNumber(EnquiryNumber);
                if (dt.Rows.Count > 0)
                {
                    data = JsonConvert.SerializeObject(dt);
                }
            }
            catch(Exception ex)
            {

            }
            return Json(data, JsonRequestBehavior.AllowGet);


        }
    }
}


