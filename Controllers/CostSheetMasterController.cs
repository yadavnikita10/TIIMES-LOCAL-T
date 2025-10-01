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
using System.Net.Mail;
using System.Net;

namespace TuvVision.Controllers
{
    public class CostSheetMasterController : Controller
    {
        DALCostSheetMaster objDalCostSheet = new DALCostSheetMaster();
        DALQuotationMaster objDALQuotationMast = new DALQuotationMaster();
        CostSheet ObjCost = new CostSheet();
        DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
        // GET: CostSheetMaster
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tast()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CostSheet(int? PK_EQID, string Quatation, int? PK_QTID)
        {
            ObjCost.EQ_ID = Convert.ToInt32(PK_EQID);
            ObjCost.PK_QTID = Convert.ToInt32(PK_QTID);
            ObjCost.QtnNo = Convert.ToString(Quatation);

            try
            {
                Session["EQ_ID"] = PK_EQID;
                Session["PK_QTID"] = PK_QTID;
                Session["QT_ID"] = Convert.ToString(Quatation);

                DataTable dt = new DataTable();
                dt= objDalCostSheet.GetLegalApprovalStatus(PK_QTID);

                if (dt.Rows.Count > 0)
                {
                    ObjCost.Legalstatus = dt.Rows[0]["Status"].ToString();
                }

                DataTable InspectionLocation = new DataTable();
                List<NameCode> lstInspectionLocation = new List<NameCode>();
                InspectionLocation = objDalCostSheet.GetInspectionLocation();

                if (InspectionLocation.Rows.Count > 0)
                {
                    lstInspectionLocation = (from n in InspectionLocation.AsEnumerable()
                                             select new NameCode()
                                             {
                                                 Name = n.Field<string>(InspectionLocation.Columns["Name"].ToString()),
                                                 Code = n.Field<Int32>(InspectionLocation.Columns["PK_ID"].ToString())

                                             }).ToList();
                }
                IEnumerable<SelectListItem> InspectionLocationItems;
                InspectionLocationItems = new SelectList(lstInspectionLocation, "Code", "Name");
                ViewBag.InspectionLocation = InspectionLocationItems;
                ViewBag.InspectionLocationItems = InspectionLocationItems;



                DataTable dtCurrency = new DataTable();
                List<NameCode> lstDCurrency = new List<NameCode>();
                dtCurrency = objDalCostSheet.GetCurrency();

                if (dtCurrency.Rows.Count > 0)//All Currency 
                {
                    lstDCurrency = (from n in dtCurrency.AsEnumerable()
                                    select new NameCode()
                                    {
                                        Name = n.Field<string>(dtCurrency.Columns["CurrencyCode"].ToString()),
                                        Code = n.Field<Int32>(dtCurrency.Columns["CurrencyID"].ToString())

                                    }).ToList();
                }
                ViewBag.DTestCurrency = lstDCurrency;
                ViewBag.ITestCurrency = lstDCurrency;
                ViewBag.Currency = lstDCurrency;
                IEnumerable<SelectListItem> CurrencyItems;
                CurrencyItems = new SelectList(lstDCurrency, "Code", "Name");
                ViewBag.CurrencyItems = CurrencyItems;


                if (PK_EQID > 0)
                {
                    #region Domestic Order Type 
                    DataTable dtDOrderType = new DataTable();
                    List<QuotationMaster> lstDOrderType = new List<QuotationMaster>();

                    dtDOrderType = objDALQuotationMast.DOrderType(PK_QTID);
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

                    dtIOrderType = objDALQuotationMast.IOrderType(PK_QTID);
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
                    //ViewBag.lstIOrderType = lstIOrderType;
                    #endregion


                    DataTable GetStatus = new DataTable();
                    GetStatus= objDalCostSheet.GetCostListByStatus(PK_QTID);
                    if (GetStatus.Rows.Count > 0)
                    {
                        foreach (DataRow dr in GetStatus.Rows)
                        {
                            ObjCost.Approval = Convert.ToString(dr["Status"]);
                            ObjCost.CLApproval = Convert.ToString(dr["CLStatus"]);
                        }
                    }
                    ViewBag.CST = ObjCost.Approval;
                    ViewBag.CLCST = ObjCost.CLApproval;

                    DataTable CostSheetDashBoard = new DataTable();
                    List<CostSheet> lstCompanyDashBoard = new List<CostSheet>();
                    CostSheetDashBoard = objDalCostSheet.GetCostList(PK_QTID);

                    #region International Cost sheet
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {

                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {

                            string CHComment = Convert.ToString(dr["CHCommentN"]).Replace(',', '\n');
                            string PCHComment = Convert.ToString(dr["PCHCommentN"]).Replace(',', '\n');
                            string SenderComment = Convert.ToString(dr["SenderCommentN"]).Replace(',', '\n');
                            string FinanceComment = Convert.ToString(dr["FinComment"]).Replace(',', '\n');

                             
                            ObjCost.IFileChoosen = Convert.ToString(dr["FileChoosen"]);
                            ObjCost.ICLFileChoosen = Convert.ToString(dr["CLFileChoosen"]);

                            lstCompanyDashBoard.Add(
                                new CostSheet
                                {
                                    PK_Cs_Id = Convert.ToInt32(dr["PK_Cs_Id"]),
                                    Costsheet = Convert.ToString(dr["Costsheet"]),
                                    RefNo = Convert.ToString(dr["RefNo"]),
                                    Createdate = Convert.ToDateTime(dr["Createdate"]),
                                    CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                    Status = Convert.ToString(dr["Status"]),
                                    EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                                    PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                    CLStatus = Convert.ToString(dr["CLStatus"]),
                                    FinStatus = Convert.ToString(dr["FinStatus"]),
                                    FinComment = FinanceComment,
                                    SendForApprovel = Convert.ToString(dr["SendForApprovel"]),
                                    PCHComment = PCHComment,
                                    CHComment = CHComment,                                    
                                    SenderComment = SenderComment,
                                    EComment = Convert.ToString(dr["EComment"]),
                                    InspectionLocation = Convert.ToString(dr["CostSheettype"]),
                                    CreatedByID = Convert.ToString(dr["CreatedByID"]),
                                    EstimatedAmount = Convert.ToInt32(dr["EstimatedAmount"]),

                                }
                            );
                        }
                        ViewData["InterNationalCostSheet"] = lstCompanyDashBoard;
                    }
                    #endregion



                    #region Domestic CostSheet
                    DataTable CostSheetDashBoardDomestic = new DataTable();
                    List<CostSheet> lstDomesticCostSheet = new List<CostSheet>();
                    CostSheetDashBoardDomestic = objDalCostSheet.GetCostListDomestic(PK_QTID);

                    if (CostSheetDashBoardDomestic.Rows.Count > 0)
                    {

                        foreach (DataRow dr in CostSheetDashBoardDomestic.Rows)
                        {

                            string CHComment = Convert.ToString(dr["CHCommentN"]).Replace(',', '\n');
                            string PCHComment = Convert.ToString(dr["PCHCommentN"]).Replace(',', '\n');
                            string SenderComment = Convert.ToString(dr["SenderCommentN"]).Replace(',', '\n');
                            string FinanceComment = Convert.ToString(dr["FinComment"]).Replace(',', '\n');

                            ObjCost.DFileChoosen = Convert.ToString(dr["FileChoosen"]);
                            ObjCost.CLDFileChoosen = Convert.ToString(dr["CLFileChoosen"]);

                            lstDomesticCostSheet.Add(
                                new CostSheet
                                {
                                    PK_Cs_Id = Convert.ToInt32(dr["PK_Cs_Id"]),
                                    Costsheet = Convert.ToString(dr["Costsheet"]),
                                    RefNo = Convert.ToString(dr["RefNo"]),
                                    Createdate = Convert.ToDateTime(dr["Createdate"]),
                                    CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                    Status = Convert.ToString(dr["Status"]),
                                    EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                                    PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                    CLStatus = Convert.ToString(dr["CLStatus"]),
                                    SendForApprovel = Convert.ToString(dr["SendForApprovel"]),
                                    PCHComment = PCHComment,
                                    CHComment = CHComment,
                                    SenderComment = SenderComment,
                                    EComment = Convert.ToString(dr["EComment"]),
                                    InspectionLocation = Convert.ToString(dr["CostSheettype"]),
                                    CreatedByID = Convert.ToString(dr["CreatedByID"]),
                                    EstimatedAmount = Convert.ToInt32(dr["EstimatedAmount"]),
                                    FinStatus = Convert.ToString(dr["FinStatus"]),
                                    FinComment = FinanceComment,
                                }
                            );
                        }
                        //ViewData["DomesticCostSheet"] = lstCompanyDashBoard;
                        ViewData["CostSheet"] = lstDomesticCostSheet;
                    }
                    #endregion
                    DataSet EstAmount = new DataSet();
                    EstAmount = objDalCostSheet.GetEstimatedAmount(PK_EQID);
                    if (EstAmount.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in EstAmount.Tables[0].Rows)
                        {
                            ObjCost.EstimatedAmount = Convert.ToInt32(dr["EstimatedAmount"]);
                        }
                        ViewBag.EstAmount = ObjCost.EstimatedAmount;
                    }
                    if (EstAmount.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in EstAmount.Tables[1].Rows)
                        {
                            ObjCost.Role = Convert.ToString(dr["FK_RoleID"]);
                        }
                        ViewBag.Role = ObjCost.Role;
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return View(ObjCost);
        }

        [HttpPost]
        public ActionResult CostSheet(CostSheet CM, HttpPostedFileBase File, FormCollection FC)
        {
            string Result = string.Empty;
            //int PK_EQID = Convert.ToInt32(FC["EQ_ID"]);
            int PK_QTID = Convert.ToInt32(FC["PK_QTID"]);
            try
            {
                if (CM.PK_Cs_Id == 0)
                {
                    HttpPostedFileBase Imagesection;

                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["DFileUpload"])))
                    {
                        Imagesection = Request.Files["DFileUpload"];
                        if (Imagesection != null && Imagesection.FileName != "")
                        {
                            CM.Costsheet = CommonControl.FileUpload("Content/Uploads/Images/", Imagesection);
                            CM.Status = "Not Approve";

                            CM.PK_QTID = PK_QTID;
                            CM.CostSheetType = 2;


                            Result = objDalCostSheet.InsertUpdateCost(CM);
                            if (Result != "" && Result != null)
                            {
                                TempData["InsertCostSheet"] = Result;
                            }
                        }
                        /*else
                        {
                            if (Imagesection.FileName != "")
                            {
                                CM.Costsheet = "NoImage.gif";
                            }
                        }*/
                        
                    }

                   

                    HttpPostedFileBase IImagesection;

                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["IFileUpload"])))
                    {
                        IImagesection = Request.Files["IFileUpload"];

                        if (IImagesection != null && IImagesection.FileName != "")
                        {
                            CM.Costsheet = CommonControl.FileUpload("Content/Uploads/Images/", IImagesection);
                            CM.Status = "Not Approve";

                            CM.PK_QTID = PK_QTID;
                            CM.CostSheetType = 1;


                            Result = objDalCostSheet.InsertUpdateCost(CM);
                            if (Result != "" && Result != null)
                            {
                                TempData["InsertCostSheet"] = Result;
                            }
                        }
                        /* else
                        {
                            if (IImagesection.FileName != "")
                            {
                                CM.Costsheet = "NoImage.gif";
                            }
                        }*/

                        
                    }

                    
                }
                else
                {
                    CM.CostSheetType = Convert.ToInt32(FC["ProductList"]);
                    Result = objDalCostSheet.InsertUpdateCost(CM);
                    if (Result != null && Result != "")
                    {
                        TempData["UpdateCostSheet"] = Result;
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("CostSheet", new { @PK_EQID = CM.EQ_ID, @Quatation = CM.QtnNo, @PK_QTID = CM.PK_QTID });
        }


        public ActionResult ChangeStatus(int? csid, int? PK_QTID,CostSheet C,string Comment)
        {
            try
            {
                string Result = string.Empty;
                Result = objDalCostSheet.ChangeSheetStatus(Convert.ToInt32(csid),C);

                StatusChangeMail(Convert.ToInt32( csid));
                
            }
            catch (Exception)
            {
                //return View();
            }
            return RedirectToAction("CostSheet", new { @PK_EQID = Session["EQ_ID"], @Quatation = Session["QT_ID"], @PK_QTID = PK_QTID });
        }
        public ActionResult ChangeApproval(int? csid, int? PK_QTID)
        {
            try
            {
                string Result = string.Empty;
                Result = objDalCostSheet.ChangeApproval(Convert.ToInt32(csid));
                ApprovalChangeMail(Convert.ToInt32(csid));
                
            }
            catch (Exception)
            {
                //return View();
            }

            return RedirectToAction("CostSheet", new { PK_EQID = Session["EQ_ID"], Quatation = Session["QT_ID"], @PK_QTID = PK_QTID });
        }

        public ActionResult ChangeCLStatus(int? csid, int? PK_QTID)
        {
            try
            {
                string Result = string.Empty;
                Result = objDalCostSheet.ChangeCLSheetStatus(Convert.ToInt32(csid));
                CLHApprovalChangeMail(Convert.ToInt32(csid));
                return RedirectToAction("CostSheet", new { @PK_EQID = Session["EQ_ID"], @Quatation = Session["QT_ID"], @PK_QTID = PK_QTID });
            }
            catch (Exception )
            {
                return View();
            }
        }
        public ActionResult ChangeCLApproval(int? csid, int? PK_QTID)
        {
            try
            {
                string Result = string.Empty;
                Result = objDalCostSheet.ChangeCLApproval(Convert.ToInt32(csid));
                CLHStatusChangeMail(Convert.ToInt32(csid));
                return RedirectToAction("CostSheet", new { PK_EQID = Session["EQ_ID"], Quatation = Session["QT_ID"], @PK_QTID = PK_QTID });
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult Delete(int id, int? PK_EQID,  int? PK_QTID)
        {
            int result = 0;


            try
            {
                result = objDalCostSheet.DeleteCostSheet(id);
                if (result != 0)
                {
                    TempData["result"] = result;
                    //return RedirectToAction("CostSheet");@Quatation = CM.QtnNo, 
                    return RedirectToAction("CostSheet", new { @PK_EQID = PK_EQID, @PK_QTID = PK_QTID });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        public ActionResult SendForApprovel(int id, int? PK_EQID, int? PK_QTID, CostSheet C)
        {
            string result = string.Empty;
            string PCHstrBody = string.Empty;
            string CLHstrBody = string.Empty;
            string bodyText = string.Empty;
            string PCHEmail = string.Empty;
            string PCHName = string.Empty;
            string CLHEmail = string.Empty;
            string FINEmail = string.Empty;
            string SenderEmail = string.Empty;
            string CLHName = string.Empty;
            string FINName = string.Empty;
            string EstimatedAmt = string.Empty;


            DataTable Details = new DataTable();
            Details = objDalCostSheet.GetquoationDetails(id.ToString());

            PCHEmail = Details.Rows[0]["PCHEmail"].ToString();
            CLHEmail = Details.Rows[0]["CLHEmail"].ToString();
            FINEmail = Details.Rows[0]["FINEmail"].ToString();
            SenderEmail = Details.Rows[0]["SenderEmail"].ToString();
            PCHName = Details.Rows[0]["PCHName"].ToString();
            CLHName = Details.Rows[0]["CLHName"].ToString();
            FINName = Details.Rows[0]["FINName"].ToString();
            EstimatedAmt = Details.Rows[0]["EstimatedAmount"].ToString();


            try
            {
                result = objDalCostSheet.SendForApprovel(id, C);

                if (result != string.Empty)
                {
                    TempData["result"] = result;
                    string link = ConfigurationManager.AppSettings["Web"].ToString();

                    if (result == "1") //// Auto Approval
                    {
                        
                        // PCHstrBody = " System has auto-approved the quotation number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "'><span style:'color: blue'>" + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a>Created By " + Details.Rows[0]["CreatedBy"].ToString() ;
                        bodyText =
                            @"<html>
                            <head>
                                <title></title>
                            </head>
                            <body>
                                <div>
                                    <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                                <div>&nbsp;</div>
                                <div>
                                    <span style='font-family:verdana,geneva,sans-serif;font-size:12px;'>TIIMES has <b>Auto Approved</b> Cost sheet of Quotation Number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "'><span style:'color: blue'>" + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a> Created By " + Details.Rows[0]["CreatedBy"].ToString() + "</br>";

                        bodyText = bodyText + "for Customer " + Details.Rows[0]["Client"].ToString() + " , Branch " + Details.Rows[0]["Branch"].ToString() + ".";
                        bodyText = bodyText + "</span></div></br>";
                        
                        bodyText = bodyText + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                        bodyText = bodyText + " TUV India Private Limited. " + "</br>";
                        bodyText = bodyText + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                        bodyText = bodyText + "</body></html> ";



                       // CLHstrBody = " System has auto-approved the quotation number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "'><span style:'color: blue'>" + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a>Created By " + Details.Rows[0]["CreatedBy"].ToString();


                        sendPCHApprovalMail(id.ToString(), bodyText, PCHEmail, SenderEmail, Details.Rows[0]["QuotationNumber"].ToString(), Details.Rows[0]["CreatedBy"].ToString(), PCHName.ToString(), Details.Rows[0]["Branch"].ToString(),"AUTO");
                        
                       // sendCLHApprovalMail(id.ToString(), bodyText, CLHEmail, SenderEmail, Details.Rows[0]["QuotationNumber"].ToString(), Details.Rows[0]["CreatedBy"].ToString(), CLHName.ToString(), Details.Rows[0]["Branch"].ToString(),"AUTO");

                    }
                    else if (result == "2") /// Manual Approval
                    {
                       // PCHstrBody = Details.Rows[0]["CreatedBy"].ToString() + " has sent you request to approve cost sheet of quotation number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "' ><span style:'color: blue'> " + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a>";
                      //  CLHstrBody = Details.Rows[0]["CreatedBy"].ToString() + " has sent you request to approve cost sheet of quotation number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "' ><span style:'color: blue'> " + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a>";

                        bodyText =
                            @"<html>
                            <head>
                                <title></title>
                            </head>
                            <body>
                                <div>
                                    <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                                <div>&nbsp;</div>
                                <div>
                                    <span style='font-family:verdana,geneva,sans-serif;font-size:12px;'>" + Details.Rows[0]["CreatedBy"].ToString() + " has sent you <b>Request to Approve</b> cost sheet (" + Details.Rows[0]["InspLocation"].ToString() + ") of Quotation Number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "'><span style:'color: blue'>" + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a></br>";

                        bodyText = bodyText + " for Customer " + Details.Rows[0]["Client"].ToString() + " , Branch " + Details.Rows[0]["Branch"].ToString() + ".";
                        bodyText = bodyText + "</span></div></br>";

                        bodyText = bodyText + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                        bodyText = bodyText + " TUV India Private Limited. " + "</br>";
                        bodyText = bodyText + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                        bodyText = bodyText + "</body></html> ";

                        sendPCHApprovalMail(id.ToString(), bodyText, PCHEmail, SenderEmail, Details.Rows[0]["QuotationNumber"].ToString(), Details.Rows[0]["CreatedBy"].ToString(), PCHName.ToString(), Details.Rows[0]["Branch"].ToString(), Details.Rows[0]["InspLocation"].ToString());

                        if (Convert.ToInt32(EstimatedAmt) >= 1000000)
                        {
                            sendCLHApprovalMail(id.ToString(), bodyText, CLHEmail, SenderEmail, Details.Rows[0]["QuotationNumber"].ToString(), Details.Rows[0]["CreatedBy"].ToString(), CLHName.ToString(), Details.Rows[0]["Branch"].ToString(), Details.Rows[0]["InspLocation"].ToString());
                        }

                        if (Convert.ToInt32(EstimatedAmt) >= 17000000)
                        {
                            sendFINApprovalMail(id.ToString(), bodyText, FINEmail, SenderEmail, Details.Rows[0]["QuotationNumber"].ToString(), Details.Rows[0]["CreatedBy"].ToString(), FINName.ToString(), Details.Rows[0]["Branch"].ToString(), Details.Rows[0]["InspLocation"].ToString());
                        }
                    }

                    return RedirectToAction("CostSheet", new { @PK_EQID = PK_EQID, @PK_QTID = PK_QTID });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("CostSheet", new { @PK_EQID = PK_EQID, @PK_QTID = PK_QTID });
        }

        public ActionResult AddComment(int? csid, int? PK_QTID, CostSheet C, string Comment)
        {
            try
            {
                string Result = string.Empty;
                Result = objDalCostSheet.AddComment(Convert.ToInt32(csid), C);
                return RedirectToAction("CostSheet", new { @PK_EQID = Session["EQ_ID"], @Quatation = Session["QT_ID"], @PK_QTID = C.PK_QTID/*PK_QTID*/ });
            }
            catch (Exception)
            {
                return View();
            }
        }

        public void sendPCHApprovalMail(string csID,string PCHstrBody ,  string PCHEmail, string SenderEmail,string qtnNo,string createdBy,string PCHName,string Branch,string Auto)
        {
            try
            {
                
                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                


               // PCHEmail = "pshrikant@tuv-nord.com";
               
                msg.From = new MailAddress(MailFrom,"Quotation Approval");
                

                string To = PCHEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }

                msg.CC.Add(SenderEmail);
                msg.CC.Add("pshrikant@tuv-nord.com");

                if (Auto == "AUTO")
                    msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost Sheet Auto Approved";
                else
                    msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost Sheet (" + Auto + ") Approval request.";

                msg.Body = PCHstrBody;
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

        public void sendCLHApprovalMail(string csID, string CLHstrBody, string CLHEmail, string SenderEmail, string qtnNo,string CreatedBy , string CLHName,string Branch, string Auto)
        {
            try
            {

                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                

              
             //  CLHEmail = "pshrikant@tuv-nord.com";

                msg.From = new MailAddress(MailFrom, "Quotation Approval");


                string To = CLHEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }
                msg.CC.Add("pshrikant@tuv-nord.com");

                /// msg.Subject = "Quotation Approval : " + qtnNo;

                if (Auto == "AUTO")
                    msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost Sheet Auto Approved";
                else
                    msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost Sheet (" + Auto + ") Approval request.";

                msg.Body = CLHstrBody;
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

        /// <summary>
        /// //Approval Mail
        /// </summary>
        /// <param name="id"></param>
        public void StatusChangeMail(int id)
        {
            string qtnNo = string.Empty;
            string PCHstrBody = string.Empty;
            string CLHstrBody = string.Empty;
            string PCHEmail = string.Empty;
            string CLHEmail = string.Empty;
            string SenderEmail = string.Empty;
            string CreatedBy = string.Empty;
            string PCHName = string.Empty;
            string CLHName = string.Empty;
            string Branch = string.Empty;
            string InspLocation = string.Empty;

            DataTable Details = new DataTable();
            Details = objDalCostSheet.GetQuotUserDetails(id.ToString());

            PCHEmail = Details.Rows[0]["PCHEmail"].ToString();
            CLHEmail = Details.Rows[0]["CLHname"].ToString();
            SenderEmail = Details.Rows[0]["SenderEmail"].ToString();
            CreatedBy = Details.Rows[0]["CreatedBy"].ToString();
            qtnNo = Details.Rows[0]["QuotationNumber"].ToString();
            Branch = Details.Rows[0]["Branch"].ToString();
            InspLocation = Details.Rows[0]["InspLocation"].ToString();
            PCHName = Details.Rows[0]["PCHName"].ToString();
            CLHName = Details.Rows[0]["CLHName"].ToString();


            try
            {
                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                // string PCHstrBody = "";
                string link = ConfigurationManager.AppSettings["Web"].ToString();


               // PCHEmail = "pshrikant@tuv-nord.com";

                PCHstrBody =
                    @"<html>
                    <head>
                        <title></title>
                    </head>
                    <body>
                        <div>
                            <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                        <div>&nbsp;</div>
                        <div>
                            <span style='font-family:verdana,geneva,sans-serif;font-size:12px;'>" + PCHName + " has <b>Approved</b> the cost sheet (" + InspLocation + ") of Quotation Number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "' ><span style:'color: blue'> " + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a></br>";

                PCHstrBody = PCHstrBody + " for Customer " + Details.Rows[0]["Client"].ToString() + " , Branch " + Details.Rows[0]["Branch"].ToString() + ".";
                PCHstrBody = PCHstrBody + "</span></div></br>";
                PCHstrBody = PCHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                PCHstrBody = PCHstrBody + " TUV India Private Limited. " + "</br>";
                PCHstrBody = PCHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                PCHstrBody = PCHstrBody + "</body></html> ";
                               

                msg.From = new MailAddress(MailFrom, "Quotation Approval");


                string To = SenderEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }
                msg.CC.Add("pshrikant@tuv-nord.com");
                if(SenderEmail != string.Empty && SenderEmail != null)
                    msg.CC.Add(SenderEmail);
                //msg.Subject = "Quotation Approval : " + qtnNo;

                msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost sheet (" + InspLocation + ") Approval status.";

                msg.Body = PCHstrBody;
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
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// ///Not Approved Mail
        /// </summary>
        /// <param name="id"></param>
        public void ApprovalChangeMail(int id)
        {
            string qtnNo = string.Empty;
            string PCHstrBody = string.Empty;
            string CLHstrBody = string.Empty;
            string PCHEmail = string.Empty;
            string CLHEmail = string.Empty;
            string SenderEmail = string.Empty;
            string CreatedBy = string.Empty;
            string PCHName = string.Empty;
            string CLHName = string.Empty;
            string Branch = string.Empty;
            string InspLocation = string.Empty;

            DataTable Details = new DataTable();
            Details = objDalCostSheet.GetQuotUserDetails(id.ToString());

            PCHEmail = Details.Rows[0]["PCHEmail"].ToString();
            CLHEmail = Details.Rows[0]["CLHname"].ToString();
            SenderEmail = Details.Rows[0]["SenderEmail"].ToString();
            CreatedBy = Details.Rows[0]["CreatedBy"].ToString();
            qtnNo = Details.Rows[0]["QuotationNumber"].ToString();
            Branch = Details.Rows[0]["Branch"].ToString();
            InspLocation = Details.Rows[0]["InspLocation"].ToString();
            PCHName = Details.Rows[0]["PCHName"].ToString();
            CLHName = Details.Rows[0]["CLHName"].ToString();

            try
            {
                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                //string PCHstrBody = "";
                string link = ConfigurationManager.AppSettings["Web"].ToString();
                //PCHEmail = "pshrikant@tuv-nord.com";               

                PCHstrBody =
                    @"<html>
                    <head>
                        <title></title>
                    </head>
                    <body>
                        <div>
                            <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                        <div>&nbsp;</div>
                        <div>
                            <span style='font-family:verdana,geneva,sans-serif;font-size:12px;'>" + PCHName + " has <b>Not Approved</b> the cost sheet (" + InspLocation + ") of Quotation Number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "' ><span style:'color: blue'> " + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a></br>";

                PCHstrBody = PCHstrBody + "for Customer " + Details.Rows[0]["Client"].ToString() + " , Branch " + Details.Rows[0]["Branch"].ToString() + ".";
                PCHstrBody = PCHstrBody + "</span></div></br>";
                PCHstrBody = PCHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                PCHstrBody = PCHstrBody + " TUV India Private Limited. " + "</br>";
                PCHstrBody = PCHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                PCHstrBody = PCHstrBody + "</body></html> ";


                msg.From = new MailAddress(MailFrom, "Quotation Approval");


                string To = PCHEmail.ToString();

                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }
                msg.CC.Add("pshrikant@tuv-nord.com");

                if (SenderEmail != string.Empty && SenderEmail != null)
                    msg.CC.Add(SenderEmail);

                msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost sheet (" + InspLocation + ") Approval status.";

                msg.Body = PCHstrBody;
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
            catch (Exception)
            {
                throw;
            }
            
        }


        /// <summary>
        /// //Approval Mail
        /// </summary>
        /// <param name="id"></param>
        public void CLHStatusChangeMail(int id)
        {
            string qtnNo = string.Empty;
            
            string CLHstrBody = string.Empty;
            
            string CLHEmail = string.Empty;
            string SenderEmail = string.Empty;
            string CreatedBy = string.Empty;
            
            string CLHName = string.Empty;
            string Branch = string.Empty;
            string InspLocation = string.Empty;

            DataTable Details = new DataTable();
            Details = objDalCostSheet.GetQuotUserDetails(id.ToString());

            
            CLHEmail = Details.Rows[0]["CLHEmail"].ToString();
            SenderEmail = Details.Rows[0]["SenderEmail"].ToString();
            CreatedBy = Details.Rows[0]["CreatedBy"].ToString();
            qtnNo = Details.Rows[0]["QuotationNumber"].ToString();
            Branch = Details.Rows[0]["Branch"].ToString();
            InspLocation = Details.Rows[0]["InspLocation"].ToString();            
            CLHName = Details.Rows[0]["CLHName"].ToString();


            try
            {
                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                // string PCHstrBody = "";
                string link = ConfigurationManager.AppSettings["Web"].ToString();

               // CLHEmail = "pshrikant@tuv-nord.com";

                CLHstrBody =
                    @"<html>
                    <head>
                        <title></title>
                    </head>
                    <body>
                        <div>
                            <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                        <div>&nbsp;</div>
                        <div>
                            <span style='font-family:verdana,geneva,sans-serif;font-size:12px;'>" + CLHName + " has <b>Not Approved</b> the cost sheet (" + InspLocation + ") of Quotation Number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "' ><span style:'color: blue'> " + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a></br>";

                CLHstrBody = CLHstrBody + " for Customer " + Details.Rows[0]["Client"].ToString() + " , Branch " + Details.Rows[0]["Branch"].ToString() + ".";
                CLHstrBody = CLHstrBody + "</span></div></br>";
                CLHstrBody = CLHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                CLHstrBody = CLHstrBody + " TUV India Private Limited. " + "</br>";
                CLHstrBody = CLHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                CLHstrBody = CLHstrBody + "</body></html> ";


                msg.From = new MailAddress(MailFrom, "Quotation Approval");


             //   string To = CLHEmail.ToString();
                string To = SenderEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }
                msg.CC.Add("pshrikant@tuv-nord.com");
                //msg.Subject = "Quotation Approval : " + qtnNo;

                msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost sheet (" + InspLocation + ") Approval status.";

                msg.Body = CLHstrBody;
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
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// ///Not Approved Mail
        /// </summary>
        /// <param name="id"></param>
        public void CLHApprovalChangeMail(int id)
        {
            string qtnNo = string.Empty;
            
            string CLHstrBody = string.Empty;
            
            string CLHEmail = string.Empty;
            string SenderEmail = string.Empty;
            string CreatedBy = string.Empty;
            
            string CLHName = string.Empty;
            string Branch = string.Empty;
            string InspLocation = string.Empty;

            DataTable Details = new DataTable();
            Details = objDalCostSheet.GetQuotUserDetails(id.ToString());

           
            CLHEmail = Details.Rows[0]["CLHEmail"].ToString();
            SenderEmail = Details.Rows[0]["SenderEmail"].ToString();
            CreatedBy = Details.Rows[0]["CreatedBy"].ToString();
            qtnNo = Details.Rows[0]["QuotationNumber"].ToString();
            Branch = Details.Rows[0]["Branch"].ToString();
            InspLocation = Details.Rows[0]["InspLocation"].ToString();
            
            CLHName = Details.Rows[0]["CLHName"].ToString();

            try
            {
                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string link = ConfigurationManager.AppSettings["Web"].ToString();

              //  CLHEmail = "pshrikant@tuv-nord.com";

                CLHstrBody =
                    @"<html>
                    <head>
                        <title></title>
                    </head>
                    <body>
                        <div>
                            <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                        <div>&nbsp;</div>
                        <div>
                            <span style='font-family:verdana,geneva,sans-serif;font-size:12px;'>" + CLHName + " has <b>Approved</b> the cost sheet (" + InspLocation + ") of Quotation Number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "' ><span style:'color: blue'> " + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a></br>";

                CLHstrBody = CLHstrBody + "for Customer " + Details.Rows[0]["Client"].ToString() + " , Branch " + Details.Rows[0]["Branch"].ToString() + ".";
                CLHstrBody = CLHstrBody + "</span></div></br>";
                CLHstrBody = CLHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                CLHstrBody = CLHstrBody + " TUV India Private Limited. " + "</br>";
                CLHstrBody = CLHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                CLHstrBody = CLHstrBody + "</body></html> ";


                msg.From = new MailAddress(MailFrom, "Quotation Approval");


                // string To = CLHEmail.ToString();
                string To = SenderEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }
                msg.CC.Add("pshrikant@tuv-nord.com");
                msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost sheet (" + InspLocation + ") Approval status.";

                msg.Body = CLHstrBody;
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
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult ChangeFINStatus(int? csid, int? PK_QTID)
        {
            try
            {
                string Result = string.Empty;
                Result = objDalCostSheet.ChangeFINSheetStatus(Convert.ToInt32(csid));
                FINApprovalChangeMail(Convert.ToInt32(csid));
                return RedirectToAction("CostSheet", new { @PK_EQID = Session["EQ_ID"], @Quatation = Session["QT_ID"], @PK_QTID = PK_QTID });
            }
            catch (Exception)
            {
                return View();
            }
        }
        public ActionResult ChangeFINApproval(int? csid, int? PK_QTID)
        {
            try
            {
                string Result = string.Empty;
                Result = objDalCostSheet.ChangeFINApproval(Convert.ToInt32(csid));
                FINStatusChangeMail(Convert.ToInt32(csid));
                return RedirectToAction("CostSheet", new { PK_EQID = Session["EQ_ID"], Quatation = Session["QT_ID"], @PK_QTID = PK_QTID });
            }
            catch (Exception)
            {
                return View();
            }
        }

        public void sendFINApprovalMail(string csID, string CLHstrBody, string CLHEmail, string SenderEmail, string qtnNo, string CreatedBy, string CLHName, string Branch, string Auto)
        {
            try
            {

                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();



                CLHEmail = "pshrikant@tuv-nord.com";

                msg.From = new MailAddress(MailFrom, "Quotation Approval");


                string To = CLHEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }
                msg.CC.Add("pshrikant@tuv-nord.com");

                

               
                msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost Sheet (" + Auto + ") Approval request.";

                msg.Body = CLHstrBody;
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

        public void FINStatusChangeMail(int id)
        {
            string qtnNo = string.Empty;

            string CLHstrBody = string.Empty;

            string CLHEmail = string.Empty;
            string SenderEmail = string.Empty;
            string CreatedBy = string.Empty;

            string CLHName = string.Empty;
            string Branch = string.Empty;
            string InspLocation = string.Empty;

            DataTable Details = new DataTable();
            Details = objDalCostSheet.GetQuotUserDetails(id.ToString());

            CLHEmail = Details.Rows[0]["FINEmail"].ToString();
            CLHName = Details.Rows[0]["FINName"].ToString();
            
            SenderEmail = Details.Rows[0]["SenderEmail"].ToString();
            CreatedBy = Details.Rows[0]["CreatedBy"].ToString();
            qtnNo = Details.Rows[0]["QuotationNumber"].ToString();
            Branch = Details.Rows[0]["Branch"].ToString();
            InspLocation = Details.Rows[0]["InspLocation"].ToString();
            


            try
            {
                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                // string PCHstrBody = "";
                string link = ConfigurationManager.AppSettings["Web"].ToString();

                // CLHEmail = "pshrikant@tuv-nord.com";

                CLHstrBody =
                    @"<html>
                    <head>
                        <title></title>
                    </head>
                    <body>
                        <div>
                            <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                        <div>&nbsp;</div>
                        <div>
                            <span style='font-family:verdana,geneva,sans-serif;font-size:12px;'>" + CLHName + " has <b>Not Approved</b> the cost sheet (" + InspLocation + ") of Quotation Number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "' ><span style:'color: blue'> " + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a></br>";

                CLHstrBody = CLHstrBody + " for Customer " + Details.Rows[0]["Client"].ToString() + " , Branch " + Details.Rows[0]["Branch"].ToString() + ".";
                CLHstrBody = CLHstrBody + "</span></div></br>";
                CLHstrBody = CLHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                CLHstrBody = CLHstrBody + " TUV India Private Limited. " + "</br>";
                CLHstrBody = CLHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                CLHstrBody = CLHstrBody + "</body></html> ";


                msg.From = new MailAddress(MailFrom, "Quotation Approval");


                //   string To = CLHEmail.ToString();
                string To = SenderEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }
                msg.CC.Add("pshrikant@tuv-nord.com");
                //msg.Subject = "Quotation Approval : " + qtnNo;

                msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost sheet (" + InspLocation + ") Approval status.";

                msg.Body = CLHstrBody;
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
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// ///Not Approved Mail
        /// </summary>
        /// <param name="id"></param>
        public void FINApprovalChangeMail(int id)
        {
            string qtnNo = string.Empty;

            string CLHstrBody = string.Empty;

            string CLHEmail = string.Empty;
            string SenderEmail = string.Empty;
            string CreatedBy = string.Empty;

            string CLHName = string.Empty;
            string Branch = string.Empty;
            string InspLocation = string.Empty;

            DataTable Details = new DataTable();
            Details = objDalCostSheet.GetQuotUserDetails(id.ToString());


            CLHEmail = Details.Rows[0]["FINEmail"].ToString();
            CLHName = Details.Rows[0]["FINName"].ToString();
            SenderEmail = Details.Rows[0]["SenderEmail"].ToString();
            CreatedBy = Details.Rows[0]["CreatedBy"].ToString();
            qtnNo = Details.Rows[0]["QuotationNumber"].ToString();
            Branch = Details.Rows[0]["Branch"].ToString();
            InspLocation = Details.Rows[0]["InspLocation"].ToString();

            

            try
            {
                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string link = ConfigurationManager.AppSettings["Web"].ToString();

                //  CLHEmail = "pshrikant@tuv-nord.com";

                CLHstrBody =
                    @"<html>
                    <head>
                        <title></title>
                    </head>
                    <body>
                        <div>
                            <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                        <div>&nbsp;</div>
                        <div>
                            <span style='font-family:verdana,geneva,sans-serif;font-size:12px;'>" + CLHName + " has <b>Approved</b> the cost sheet (" + InspLocation + ") of Quotation Number <a href='" + link + @Url.Action("Quotation", "QuotationMaster", new { PK_QM_ID = Details.Rows[0]["QuotationNumber"].ToString() }) + "' ><span style:'color: blue'> " + Details.Rows[0]["QuotationNumber"].ToString() + " </span></a></br>";

                CLHstrBody = CLHstrBody + "for Customer " + Details.Rows[0]["Client"].ToString() + " , Branch " + Details.Rows[0]["Branch"].ToString() + ".";
                CLHstrBody = CLHstrBody + "</span></div></br>";
                CLHstrBody = CLHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                CLHstrBody = CLHstrBody + " TUV India Private Limited. " + "</br>";
                CLHstrBody = CLHstrBody + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                CLHstrBody = CLHstrBody + "</body></html> ";


                msg.From = new MailAddress(MailFrom, "Quotation Approval");


                // string To = CLHEmail.ToString();
                string To = SenderEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }
                msg.CC.Add("pshrikant@tuv-nord.com");
                msg.Subject = "TIIMES-BDSM-" + qtnNo + "-" + Branch + "- Cost sheet (" + InspLocation + ") Approval status.";

                msg.Body = CLHstrBody;
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
            catch (Exception)
            {
                throw;
            }

        }




    }
}