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
using System.Net;
using OfficeOpenXml;
using NonFactors.Mvc.Grid;
using System.Globalization;
using System.Net.Mail;
using Newtonsoft.Json;

namespace TuvVision.Controllers
{
    public class EnquiryMasterController : Controller
    {
        CommonControl objCommonControl = new CommonControl();
        DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
        EnquiryMaster ObjModelEnquiry = new EnquiryMaster();
        List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
        DataTable dtContactList = new DataTable();
        List<NameCode> lstEditContactNameEQ_ID = new List<NameCode>();
        DALCompanyMaster objDalCompany = new DALCompanyMaster();
        DataTable DSGetCheckList = new DataTable();
        List<EnquiryMaster> LCheckList = new List<EnquiryMaster>();
        List<EnquiryMaster> lstConflictJobs = new List<EnquiryMaster>();
        List<EnquiryMaster> lstConflictEnquiry = new List<EnquiryMaster>();

        public ActionResult EnquiryMasterDashBoard(string Type)
        {
            //Session["UserLoginID"] = User.Identity.IsAuthenticated;
            //string UserRole = Convert.ToString(Session["role"]);
            //List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            //lstEnquiryMast = objDalEnquiryMaster.GetEnquiryListDashBoard();
            //ViewData["EnquiryMaster"] = lstEnquiryMast;
            //return View();
            Session["UserLoginID"] = User.Identity.IsAuthenticated;
            string UserRole = Convert.ToString(Session["role"]);
            EnquiryMaster objmodel = new EnquiryMaster();
            List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            lstEnquiryMast = objDalEnquiryMaster.GetEnquiryListDashBoard1(Type);
            objmodel.lstEnquiryMast = lstEnquiryMast;
            return View(objmodel);
        }
        public ActionResult Enquiry(int? EQ_ID)
        {

            string[] RegretReason;
            string[] ActionTaken;

            ViewBag.BindOrderTypes = new List<SelectListItem>
       {
        new SelectListItem { Text = "Man Day", Value = "ManDays" },
        new SelectListItem { Text = "Man Month", Value = "ManMonth" },
        new SelectListItem { Text = "Lump Sum", Value = "LumSum" },
        new SelectListItem { Text = "Percentage", Value = "Perc" },
        new SelectListItem { Text = "Man Hour", Value = "ManHR" },
        new SelectListItem{ Text="1/2 ManDays", Value = "1/2ManDays"},
        new SelectListItem{ Text="Certification", Value = "Certification"},
        };

            #region Bind CheckList
            try
            {

                DSGetCheckList = objDalEnquiryMaster.GetEnquiryCheckList();
                if (DSGetCheckList.Rows.Count > 0)
                {
                    foreach (DataRow dr in DSGetCheckList.Rows)
                    {
                        LCheckList.Add(
                            new EnquiryMaster
                            {
                                CheckListId = Convert.ToString(dr["Id"]),
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

            var Data4 = objDalEnquiryMaster.GetAddress();
            ViewBag.Address = new SelectList(Data4, "CompanyAddress", "Address");


            #region Get ConflictDetail
            //DataTable DTGetConflictDetail = new DataTable();
            //DTGetConflictDetail = objDalEnquiryMaster.GetConflictDetail(EQ_ID);

            //try
            //{
            //    if (DTGetConflictDetail.Rows.Count > 0)
            //    {
            //        ObjModelEnquiry.ConflictType = Convert.ToString(DTGetConflictDetail.Rows[0]["ConflictType"]);
            //        if (ObjModelEnquiry.ConflictType == "Conflict")
            //        {
            //            ObjModelEnquiry.ConflictSituation = "Yes";
            //        }
            //        else
            //        {
            //            ObjModelEnquiry.ConflictSituation = "No";
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    string Error = ex.Message.ToString();
            //}

            //ObjModelEnquiry.lstConflictJobs = lstConflictJobs;

            #endregion

            var Data1 = objDalEnquiryMaster.GetRegretReason();
            ViewBag.RegretReason1 = new SelectList(Data1, "RegretId", "RegretReason");

            var Data2 = objDalEnquiryMaster.GetRegretActionTaken();
            ViewBag.ActionTaken = new SelectList(Data2, "RegretActionTakenId", "RegretActionTaken");


            ObjModelEnquiry.RefDate = DateTime.Now.ToString("dd/MM/yyyy");



            DataSet DSGetEditAllddllst = new DataSet();

            List<NameCode> lstDCurrency = new List<NameCode>();

            DSGetEditAllddllst = objDalEnquiryMaster.GetEditAllddlLst();

            if (DSGetEditAllddllst.Tables[9].Rows.Count > 0)//All Currency 
            {
                lstDCurrency = (from n in DSGetEditAllddllst.Tables[9].AsEnumerable()
                                select new NameCode()
                                {
                                    Name = n.Field<string>(DSGetEditAllddllst.Tables[9].Columns["Name"].ToString()),
                                    Code = n.Field<Int32>(DSGetEditAllddllst.Tables[9].Columns["PK_ID"].ToString())

                                }).ToList();
            }
            IEnumerable<SelectListItem> Dcurrency;
            Dcurrency = new SelectList(lstDCurrency, "Code", "Name");
            ViewBag.Dcurrency = Dcurrency;
            ViewBag.Icurrency = Dcurrency;
            ViewData["Dcurrency"] = Dcurrency;
            if (EQ_ID > 0)
            {
                ViewBag.check = "productcheck";
                TempData["QuotationEnableButton"] = EQ_ID;
                TempData.Keep();

                string[] splitedCity;
                string[] splitedCountry;
                string[] splitedUploadDoc;
                string[] InspectionLocation;
                List<string> Selected = new List<string>();
                List<string> SelectedCountry = new List<string>();
                List<string> SelectedInspectionLocation = new List<string>();

                DataTable DTGetEnquiryDtls = new DataTable();
                DTGetEnquiryDtls = objDalEnquiryMaster.GetEnquiryDetals(EQ_ID);
                if (DTGetEnquiryDtls.Rows.Count > 0)
                {

                    #region Bind Contact list
                    if (DTGetEnquiryDtls.Rows[0]["FK_CMP_ID"].ToString() != "" || DTGetEnquiryDtls.Rows[0]["FK_CMP_ID"].ToString() != null)
                    {

                        //dtContactList = objDalEnquiryMaster.GetInspectorList(DTGetEnquiryDtls.Rows[0]["FK_CMP_ID"].ToString());
                        dtContactList = objDalEnquiryMaster.GetInspector(DTGetEnquiryDtls.Rows[0]["FK_CMP_ID"].ToString());
                        if (dtContactList.Rows.Count > 0)
                        {
                            lstEditContactNameEQ_ID = (from n in dtContactList.AsEnumerable()
                                                       select new NameCode()
                                                       {
                                                           Name = n.Field<string>(dtContactList.Columns["ContactName"].ToString()),
                                                           Code = n.Field<Int32>(dtContactList.Columns["PK_ContID"].ToString())

                                                       }).ToList();
                        }
                        IEnumerable<SelectListItem> ContactTypeItems;
                        ContactTypeItems = new SelectList(lstEditContactNameEQ_ID, "Code", "Name");
                        ViewBag.ContactTypeItems = ContactTypeItems;
                        ViewData["ContactTypeItems"] = ContactTypeItems;

                        DataTable GetDuplicate = new DataTable();
                        GetDuplicate = objDalEnquiryMaster.GetDuplicate(DTGetEnquiryDtls.Rows[0]["FK_CMP_ID"].ToString(),EQ_ID);
                        if (GetDuplicate.Rows.Count > 0)
                        {
                            ObjModelEnquiry.GetDuplicateEnquiry = GetDuplicate.Rows[0]["Duplicatecnt"].ToString();
                           
                        }
                        
                    }


                    else
                    {

                    }

                    #endregion




                    ObjModelEnquiry.ConflictSituation = Convert.ToString(DTGetEnquiryDtls.Rows[0]["IsConflict"]);
                    if (ObjModelEnquiry.ConflictSituation == "Conflict")
                    {
                        ObjModelEnquiry.ConflictType = "Conflict";
                        ObjModelEnquiry.ConflictSituation = "Yes";
                    }
                    else
                    {
                        ObjModelEnquiry.ConflictSituation = "No";
                    }


                    ObjModelEnquiry.AllA = Convert.ToString(DTGetEnquiryDtls.Rows[0]["AllA"]);
                    ObjModelEnquiry.JobCount = Convert.ToString(DTGetEnquiryDtls.Rows[0]["JobCreated"]);
                    ObjModelEnquiry.QuoGenerated = Convert.ToString(DTGetEnquiryDtls.Rows[0]["QuoGenerated"]);
                    ObjModelEnquiry.EQ_ID = Convert.ToInt32(DTGetEnquiryDtls.Rows[0]["EQ_ID"]);
                    ObjModelEnquiry.EnquiryDescription = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryDescription"]);
                    ObjModelEnquiry.CompanyName = Convert.ToString(DTGetEnquiryDtls.Rows[0]["CompanyName"]);
                    ObjModelEnquiry.oldcustomername = Convert.ToString(DTGetEnquiryDtls.Rows[0]["OldCompanyName"]);
                    ObjModelEnquiry.EndCustomer = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EndCustomer"]);
                    //DateTime ECDate = Convert.ToDateTime(DTGetEnquiryDtls.Rows[0]["EstClose"]);
                    //ObjModelEnquiry.EstClose = ECDate.AddDays(2);
                    ObjModelEnquiry.EstClose = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EstClose"]);
                    ObjModelEnquiry.EnquiryReferenceNo = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryReferenceNo"]);
                    ObjModelEnquiry.EstimatedAmount = Convert.ToDecimal(DTGetEnquiryDtls.Rows[0]["EstimatedAmount"]);
                    //change by shrutika salve 23122024
                    ObjModelEnquiry.CMP_ID = Convert.ToInt32(DTGetEnquiryDtls.Rows[0]["FK_CMP_ID"]);

                    ObjModelEnquiry.ProjectType = Convert.ToString(DTGetEnquiryDtls.Rows[0]["ProjectType"]);
                    ObjModelEnquiry.PortfolioType = Convert.ToString(DTGetEnquiryDtls.Rows[0]["PortfolioType"]);
                    ObjModelEnquiry.SubServiceType = Convert.ToString(DTGetEnquiryDtls.Rows[0]["SubServiceType"]);

                    ObjModelEnquiry.Country = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Country"]);
                    ObjModelEnquiry.Notes = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Notes"]);
                    ObjModelEnquiry.InspectionLocation = Convert.ToString(DTGetEnquiryDtls.Rows[0]["InspectionLocation"]);
                    var EInspectionLocation = Convert.ToString(DTGetEnquiryDtls.Rows[0]["InspectionLocation"]);
                    InspectionLocation = EInspectionLocation.Split(',');
                    foreach (var single in InspectionLocation)
                    {
                        SelectedInspectionLocation.Add(single);
                    }
                    //ViewData["UploadDocument"] = Selected;
                    ViewBag.EditproductName = SelectedInspectionLocation;
                    ObjModelEnquiry.Branch = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Branch"]);
                    //ObjModelEnquiry.QuotationHide = Convert.ToString(DTGetEnquiryDtls.Rows[0]["QuotationHide"]);
                    ObjModelEnquiry.Source = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Source"]);
                    ObjModelEnquiry.Type = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Type"]);
                    ObjModelEnquiry.EnquiryNumber = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryNumber"]);
                    ObjModelEnquiry.ContactName = Convert.ToString(DTGetEnquiryDtls.Rows[0]["ContactName"]);
                    ObjModelEnquiry.DocumentAttached = Convert.ToString(DTGetEnquiryDtls.Rows[0]["DocumentAttached"]);
                    ObjModelEnquiry.RegretStatus = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EStatus"]);
                    ObjModelEnquiry.RegretReasonDescription = Convert.ToString(DTGetEnquiryDtls.Rows[0]["RegretReasonDescription"]);
                    ObjModelEnquiry.DeleteReason = Convert.ToString(DTGetEnquiryDtls.Rows[0]["DeleteReason"]);
                    ObjModelEnquiry.CostSheetA = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Costsheet"]);
                    ViewBag.Regret = Convert.ToString(ObjModelEnquiry.RegretStatus);

                    //   ObjModelEnquiry.RegretReason = Convert.ToString(DTGetEnquiryDtls.Rows[0]["RegretReason"]);

                    List<string> SelectedRegretReason = new List<string>();
                    var EDRegretReason = Convert.ToString(DTGetEnquiryDtls.Rows[0]["RegretReason"]);
                    RegretReason = EDRegretReason.Split(',');
                    foreach (var single in RegretReason)
                    {
                        SelectedRegretReason.Add(single);
                    }

                    ViewBag.EditRegretReason = SelectedRegretReason;

                    var selectedOrderTypeList = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnqOrderType"])
                                      .Split(',')
                                      .Select(s => s.Trim())
                                      .ToList();
                    ViewBag.OrderTypenew = selectedOrderTypeList;


                    ///ObjModelEnquiry.RegretActionTaken = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryRegretActionTaken"]);


                    List<string> SelectedActionTaken = new List<string>();
                    var EDActionTaken = Convert.ToString(DTGetEnquiryDtls.Rows[0]["EnquiryRegretActionTaken"]);
                    ActionTaken = EDActionTaken.Split(',');
                    foreach (var single1 in ActionTaken)
                    {
                        SelectedActionTaken.Add(single1);
                    }
                    ViewBag.EditActionTaken = SelectedActionTaken;


                    ObjModelEnquiry.DEstimatedAmount = Convert.ToString(DTGetEnquiryDtls.Rows[0]["DEstimatedAmount"]);
                    ObjModelEnquiry.Dcurrency = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Dcurrency"]);
                    ObjModelEnquiry.DExchangeRate = Convert.ToString(DTGetEnquiryDtls.Rows[0]["DExchangeRate"]);
                    ObjModelEnquiry.DTotalAmount = Convert.ToString(DTGetEnquiryDtls.Rows[0]["DTotalAmount"]);
                    ObjModelEnquiry.IEstimatedAmount = Convert.ToString(DTGetEnquiryDtls.Rows[0]["IEstimatedAmount"]);
                    ObjModelEnquiry.Icurrency = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Icurrency"]);
                    ObjModelEnquiry.IExchangeRate = Convert.ToString(DTGetEnquiryDtls.Rows[0]["IExchangeRate"]);
                    ObjModelEnquiry.ITotalAmount = Convert.ToString(DTGetEnquiryDtls.Rows[0]["ITotalAmount"]);
                    ObjModelEnquiry.chkArc = Convert.ToBoolean(DTGetEnquiryDtls.Rows[0]["chkArc"]);
                    ObjModelEnquiry.ENQDuplicate = Convert.ToString(DTGetEnquiryDtls.Rows[0]["ENQDuplicate"]);

                    //added by nikita on 25062024

                    ObjModelEnquiry.LegalReview = Convert.ToBoolean(DTGetEnquiryDtls.Rows[0]["LegalReview"]);
                    ObjModelEnquiry.Quotationviewed = Convert.ToBoolean(DTGetEnquiryDtls.Rows[0]["QuotationReview"]);
                    ObjModelEnquiry.Legalcomment = Convert.ToString(DTGetEnquiryDtls.Rows[0]["LegalComment"]);
                    ObjModelEnquiry.Budgetary = Convert.ToBoolean(DTGetEnquiryDtls.Rows[0]["Budgetary"]);


                    var ExistingUploadDocName = Convert.ToString(DTGetEnquiryDtls.Rows[0]["DocumentAttached"]);
                    splitedUploadDoc = ExistingUploadDocName.Split(',');
                    foreach (var single in splitedUploadDoc)
                    {
                        Selected.Add(single);
                    }
                    ViewData["UploadDocument"] = Selected;
                    ViewBag.UploadDocument = Selected;
                    //ObjModelEnquiry.City = Convert.ToString(DTGetEnquiryDtls.Rows[0]["City"]);
                    var ExistingCityName = Convert.ToString(DTGetEnquiryDtls.Rows[0]["City"]);
                    splitedCity = ExistingCityName.Split(',');
                    foreach (var single in splitedCity)
                    {
                        Selected.Add(single);
                    }
                    ViewBag.EditCityName = Selected;

                    //ObjModelEnquiry.Country = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Country"]);
                    var ExistingCountryName = Convert.ToString(DTGetEnquiryDtls.Rows[0]["Country"]);
                    splitedCountry = ExistingCountryName.Split(',');
                    foreach (var single in splitedCountry)
                    {
                        //Selected.Add(single);
                        if (single == "")
                        {
                            SelectedCountry.Add("0");
                        }
                        else
                        {
                            SelectedCountry.Add(single);
                        }
                    }
                    ViewBag.EditCountryName = SelectedCountry;


                    ObjModelEnquiry.lstPedType = objDalEnquiryMaster.BindEditPedType(ObjModelEnquiry.SubType);
                    ObjModelEnquiry.RefDate = Convert.ToString(DTGetEnquiryDtls.Rows[0]["RefDate"]);
                    ObjModelEnquiry.LeadGivenBy = Convert.ToString(DTGetEnquiryDtls.Rows[0]["LeadGivenBy"]);
                    ObjModelEnquiry.NotesbyLeads = Convert.ToString(DTGetEnquiryDtls.Rows[0]["NotesbyLeads"]);
                    ObjModelEnquiry.CheckListId = Convert.ToString(DTGetEnquiryDtls.Rows[0]["CheckListId"]);
                    ObjModelEnquiry.CheckListDescription = Convert.ToString(DTGetEnquiryDtls.Rows[0]["CheckListDescription"]);
                    //ObjModelEnquiry.lstCity = objDalEnquiryMaster.BindCity(ObjModelEnquiry.City);
                    //ObjModelEnquiry.lstCountry = objDalEnquiryMaster.BindCountry(ObjModelEnquiry.Country);
                    //ObjModelEnquiry.lstOtherType = objDalEnquiryMaster.BindEditOtherType(ObjModelEnquiry.OtherType);
                    //ObjModelEnquiry.lstEnergyType = objDalEnquiryMaster.BindEditEnergyType(ObjModelEnquiry.EnergyType);
                    //#region Domestic Order Type 
                    //DataTable dtDOrderType = new DataTable();
                    //List<QuotationMaster> lstDOrderType = new List<QuotationMaster>();

                    //dtDOrderType = objDalEnquiryMaster.DOrderType(EQ_ID);
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
                    //               Remark = Convert.ToString(dr["Remark"]),
                    //           }
                    //         );
                    //    }
                    //    ViewBag.lstDOrderType = lstDOrderType;

                    //}
                    //#endregion


                    //#region International Order Type 
                    //DataTable dtIOrderType = new DataTable();
                    //List<QuotationMaster> lstIOrderType = new List<QuotationMaster>();

                    //dtIOrderType = objDalEnquiryMaster.IOrderType(EQ_ID);
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
                    //               Remark = Convert.ToString(dr["Remark"]),
                    //           }
                    //         );
                    //    }
                    //    ViewBag.lstIOrderType = lstIOrderType;
                    //}

                    //#endregion		  
                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDalEnquiryMaster.EditUploadedFile(EQ_ID);
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
                //    ObjModelEnquiry.FileDetails = lstEditFileDetails;
                //}
                //added by nikita on 25062024
                if (DTGetUploadedFile.Rows.Count > 0)
                {
                    List<FileDetails> lstEditLegalFileDetails = new List<FileDetails>();
                    List<FileDetails> lstEditOtherFileDetails = new List<FileDetails>();
                    List<FileDetails> lstEditNDAFileDetails = new List<FileDetails>();

                    foreach (DataRow dr in DTGetUploadedFile.Rows)
                    {
                        FileDetails fileDetails = new FileDetails
                        {
                            PK_ID = Convert.ToInt32(dr["PK_ID"]),
                            FileName = Convert.ToString(dr["FileName"]),
                            Extension = Convert.ToString(dr["Extenstion"]),
                            IDS = Convert.ToString(dr["FileID"]),
                            // Assuming FileDetails class has AttachmentType property
                            AttachmentType = Convert.ToString(dr["AttachmentType"])
                        };

                        if (fileDetails.AttachmentType == "Legal")
                        {
                            lstEditLegalFileDetails.Add(fileDetails);
                        }
                        else if (fileDetails.AttachmentType == "NDA")
                        {
                            lstEditNDAFileDetails.Add(fileDetails);
                        }
                        else
                        {
                            lstEditOtherFileDetails.Add(fileDetails);
                        }
                    }
                    if (lstEditLegalFileDetails.Count > 0)
                    {
                        ViewData["lstEditLegalFileDetails"] = lstEditLegalFileDetails;
                        ObjModelEnquiry.FileDetails_ = lstEditLegalFileDetails;
                    }

                    // Set ViewData and ObjModelEnquiry for other files
                    if (lstEditOtherFileDetails.Count > 0)
                    {
                        ViewData["lstEditOtherFileDetails"] = lstEditOtherFileDetails;
                        ObjModelEnquiry.FileDetails = lstEditOtherFileDetails;
                    }
                    if (lstEditNDAFileDetails.Count > 0)
                    {
                        ViewData["lstEditNDAFileDetails"] = lstEditNDAFileDetails;
                        ObjModelEnquiry.FileDetailsformats = lstEditNDAFileDetails;
                    }
                }

                //**********************************************Code Added by Manoj Sharma for Delete file and update file

                List<NameCode> lstEditBranch = new List<NameCode>();
                List<NameCode> lstEditServiceType = new List<NameCode>();
                List<NameCode> lstEditProjectType = new List<NameCode>();
                List<NameCode> lstEditInspectionLocation = new List<NameCode>();
                List<NameCode> lstEditSource = new List<NameCode>();
                List<NameCode> lstEditContactName = new List<NameCode>();
                List<NameCode> lstEditCityName = new List<NameCode>();
                List<NameCode> lstEditCountryName = new List<NameCode>();
                List<NameCode> lstEditPortfolioType = new List<NameCode>();

                DSGetEditAllddllst = objDalEnquiryMaster.GetEditAllddlLst();
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
                ViewData["ProjectTypeItems"] = ProjectTypeItems;

                if (DSGetEditAllddllst.Tables[1].Rows.Count > 0)
                {
                    lstEditBranch = (from n in DSGetEditAllddllst.Tables[1].AsEnumerable()
                                     select new NameCode()
                                     {
                                         Name = n.Field<string>(DSGetEditAllddllst.Tables[1].Columns["BranchName"].ToString()),
                                         Code = n.Field<Int32>(DSGetEditAllddllst.Tables[1].Columns["PK_ID"].ToString())

                                     }).ToList();
                }
                IEnumerable<SelectListItem> BranchNameItems;
                BranchNameItems = new SelectList(lstEditBranch, "Code", "Name");
                ViewBag.BranchNameItems = BranchNameItems;
                ViewData["BranchNameItems"] = BranchNameItems;

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
                ViewBag.ServiceNameItems = ServiceTypeItems;
                ViewData["ServiceTypeItems"] = ServiceTypeItems;

                if (DSGetEditAllddllst.Tables[3].Rows.Count > 0)
                {
                    lstEditServiceType = (from n in DSGetEditAllddllst.Tables[3].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditAllddllst.Tables[3].Columns["Name"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditAllddllst.Tables[3].Columns["PK_ID"].ToString())

                                          }).ToList();
                }
                IEnumerable<SelectListItem> InspectionLocationItems;
                InspectionLocationItems = new SelectList(lstEditServiceType, "Code", "Name");
                ViewBag.InspectionLocationItems = InspectionLocationItems;
                ViewData["InspectionLocationItems"] = InspectionLocationItems;

                if (DSGetEditAllddllst.Tables[4].Rows.Count > 0)
                {
                    lstEditSource = (from n in DSGetEditAllddllst.Tables[4].AsEnumerable()
                                     select new NameCode()
                                     {
                                         Name = n.Field<string>(DSGetEditAllddllst.Tables[4].Columns["SourceName"].ToString()),
                                         Code = n.Field<Int32>(DSGetEditAllddllst.Tables[4].Columns["PK_SourceID"].ToString())

                                     }).ToList();
                }
                IEnumerable<SelectListItem> SourceTypeItems;
                SourceTypeItems = new SelectList(lstEditSource, "Code", "Name");
                ViewBag.SourceTypeItems = SourceTypeItems;
                ViewData["SourceTypeItems"] = SourceTypeItems;

                //if (DSGetEditAllddllst.Tables[5].Rows.Count > 0)
                //{
                //    lstEditContactName = (from n in DSGetEditAllddllst.Tables[5].AsEnumerable()
                //                          select new NameCode()
                //                          {
                //                              Name = n.Field<string>(DSGetEditAllddllst.Tables[5].Columns["ContactName"].ToString()),
                //                              Code = n.Field<Int32>(DSGetEditAllddllst.Tables[5].Columns["PK_ContID"].ToString())

                //                          }).ToList();
                //}
                //IEnumerable<SelectListItem> ContactTypeItems;
                //ContactTypeItems = new SelectList(lstEditContactName, "Code", "Name");
                //ViewBag.ContactTypeItems = ContactTypeItems;
                //ViewData["ContactTypeItems"] = ContactTypeItems;

                if (DSGetEditAllddllst.Tables[5].Rows.Count > 0)
                {
                    lstEditContactName = (from n in DSGetEditAllddllst.Tables[5].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditAllddllst.Tables[5].Columns["ContactName"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditAllddllst.Tables[5].Columns["PK_ContID"].ToString())

                                          }).ToList();
                }
                ViewBag.ContactType = lstEditContactName;

                if (DSGetEditAllddllst.Tables[6].Rows.Count > 0)
                {
                    lstEditCityName = (from n in DSGetEditAllddllst.Tables[6].AsEnumerable()
                                       select new NameCode()
                                       {
                                           Name = n.Field<string>(DSGetEditAllddllst.Tables[6].Columns["CityName"].ToString()),
                                           Code = n.Field<Int32>(DSGetEditAllddllst.Tables[6].Columns["PK_ID"].ToString())

                                       }).ToList();
                }
                IEnumerable<SelectListItem> CityNameItems;
                CityNameItems = new SelectList(lstEditCityName, "Code", "Name");
                ViewBag.CityNameItems = CityNameItems;
                ViewData["CityNameItems"] = CityNameItems;

                if (DSGetEditAllddllst.Tables[7].Rows.Count > 0)
                {
                    lstEditCountryName = (from n in DSGetEditAllddllst.Tables[7].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetEditAllddllst.Tables[7].Columns["CountryName"].ToString()),
                                              Code = n.Field<Int32>(DSGetEditAllddllst.Tables[7].Columns["PK_ID"].ToString())

                                          }).ToList();
                }
                IEnumerable<SelectListItem> CountryNameItems;
                CountryNameItems = new SelectList(lstEditCountryName, "Code", "Name");
                ViewBag.CountryNameItems = CountryNameItems;
                ViewData["CountryNameItems"] = CountryNameItems;

                if (DSGetEditAllddllst.Tables[8].Rows.Count > 0)//PortFolio
                {
                    lstEditPortfolioType = (from n in DSGetEditAllddllst.Tables[8].AsEnumerable()
                                            select new NameCode()
                                            {
                                                Name = n.Field<string>(DSGetEditAllddllst.Tables[8].Columns["Name"].ToString()),
                                                Code = n.Field<Int32>(DSGetEditAllddllst.Tables[8].Columns["PK_ID"].ToString())

                                            }).ToList();
                }
                IEnumerable<SelectListItem> PortFolioName;
                PortFolioName = new SelectList(lstEditPortfolioType, "Code", "Name");
                ViewBag.PortfolioTypeItems = lstEditPortfolioType;
                ViewData["PortfolioTypeItems"] = PortFolioName;

                if (DSGetEditAllddllst.Tables[9].Rows.Count > 0)//All Currency 
                {
                    lstDCurrency = (from n in DSGetEditAllddllst.Tables[9].AsEnumerable()
                                    select new NameCode()
                                    {
                                        Name = n.Field<string>(DSGetEditAllddllst.Tables[9].Columns["Name"].ToString()),
                                        Code = n.Field<Int32>(DSGetEditAllddllst.Tables[9].Columns["PK_ID"].ToString())

                                    }).ToList();
                }
                ViewBag.DTestCurrency = lstDCurrency;
                ViewBag.ITestCurrency = lstDCurrency;
                ViewBag.Currency = lstDCurrency;
                IEnumerable<SelectListItem> CurrencyItems;
                CurrencyItems = new SelectList(lstDCurrency, "Code", "Name");
                ViewBag.CurrencyItems = CurrencyItems;


                #region 5 feb
                //if (DSGetEditAllddllst.Tables[9].Rows.Count > 0)//All Currency 
                //{
                //    lstDCurrency = (from n in DSGetEditAllddllst.Tables[9].AsEnumerable()
                //                    select new NameCode()
                //                    {
                //                        Name = n.Field<string>(DSGetEditAllddllst.Tables[9].Columns["Name"].ToString()),
                //                        Code = n.Field<Int32>(DSGetEditAllddllst.Tables[9].Columns["PK_ID"].ToString())

                //                    }).ToList();
                //}
                //IEnumerable<SelectListItem> acurrency;//15
                //acurrency = new SelectList(lstDCurrency, "Code", "Name");
                //ViewBag.Dcurrency = acurrency;
                //ViewData["Dcurrency"] = acurrency;

                #endregion 5 feb											   
                //Adding Contact Model Poup Code Here
                DataSet DSEditGetAllDdlList = new DataSet();
                List<NameCode> lstEditCompanyList = new List<NameCode>();
                List<NameCode> lstEditTitleList = new List<NameCode>();
                DSEditGetAllDdlList = objDalEnquiryMaster.GetContactDdlList();
                if (DSEditGetAllDdlList.Tables[0].Rows.Count > 0)
                {
                    //comment by shrutika salve 21052024
                    //if (DSEditGetAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    //{
                    //    lstEditCompanyList = (from n in DSEditGetAllDdlList.Tables[0].AsEnumerable()
                    //                          select new NameCode()
                    //                          {
                    //                              Name = n.Field<string>(DSEditGetAllDdlList.Tables[0].Columns["Company_Name"].ToString()),
                    //                              Code = n.Field<Int32>(DSEditGetAllDdlList.Tables[0].Columns["CMP_ID"].ToString())

                    //                          }).ToList();
                    //}
                    //ViewBag.ContactCompanyName = lstEditCompanyList;
                    if (DSEditGetAllDdlList.Tables[1].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
                    {
                        lstEditTitleList = (from n in DSEditGetAllDdlList.Tables[1].AsEnumerable()
                                            select new NameCode()
                                            {
                                                Name = n.Field<string>(DSEditGetAllDdlList.Tables[1].Columns["TitleName"].ToString()),
                                                Code = n.Field<Int32>(DSEditGetAllDdlList.Tables[1].Columns["PK_TitleID"].ToString())

                                            }).ToList();
                    }
                    ViewBag.ContactTitleName = lstEditTitleList;

                    //Binding  Quotation List
                    List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
                    lstQuotationMast = objDalEnquiryMaster.QuotaionMastertDashBoard(EQ_ID);
                    ViewData["QuotationMaster"] = lstQuotationMast;

                    return View(ObjModelEnquiry);
                }
            }
            else
            {
                DataSet DSGetAllddllst = new DataSet();
                List<NameCode> lstBranch = new List<NameCode>();
                List<NameCode> lstServiceType = new List<NameCode>();
                List<NameCode> lstProjectType = new List<NameCode>();
                List<NameCode> lstInspectionLocation = new List<NameCode>();
                List<NameCode> lstSource = new List<NameCode>();
                List<NameCode> lstContactName = new List<NameCode>();
                List<NameCode> lstCityName = new List<NameCode>();
                List<NameCode> lstCountryName = new List<NameCode>();
                List<NameCode> lstPortfolioType = new List<NameCode>();
                List<NameCode> lstCurrency = new List<NameCode>();

                //  DSGetAllddllst = objDalEnquiryMaster.GetAllddlLst();

                DSGetAllddllst = objDalEnquiryMaster.GetDropDownList();

                if (DSGetAllddllst.Tables[0].Rows.Count > 0)
                {
                    lstProjectType = (from n in DSGetAllddllst.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetAllddllst.Tables[0].Columns["ProjectName"].ToString()),
                                          Code = n.Field<Int32>(DSGetAllddllst.Tables[0].Columns["PK_ID"].ToString())

                                      }).ToList();
                }
                ViewBag.OBSType = lstProjectType;

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

                if (DSGetAllddllst.Tables[1].Rows.Count > 0)
                {
                    ObjModelEnquiry.Branch = Convert.ToString(DSGetAllddllst.Tables[1].Rows[0]["BranchName"]);
                }

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
                    lstInspectionLocation = (from n in DSGetAllddllst.Tables[3].AsEnumerable()
                                             select new NameCode()
                                             {
                                                 Name = n.Field<string>(DSGetAllddllst.Tables[3].Columns["Name"].ToString()),
                                                 Code = n.Field<Int32>(DSGetAllddllst.Tables[3].Columns["PK_ID"].ToString())

                                             }).ToList();
                }
                ViewBag.InspectionLocation = lstInspectionLocation;

                if (DSGetAllddllst.Tables[4].Rows.Count > 0)
                {
                    lstSource = (from n in DSGetAllddllst.Tables[4].AsEnumerable()
                                 select new NameCode()
                                 {
                                     Name = n.Field<string>(DSGetAllddllst.Tables[4].Columns["SourceName"].ToString()),
                                     Code = n.Field<Int32>(DSGetAllddllst.Tables[4].Columns["PK_SourceID"].ToString())

                                 }).ToList();
                }
                ViewBag.SourceType = lstSource;


                if (DSGetAllddllst.Tables[5].Rows.Count > 0)
                {
                    lstContactName = (from n in DSGetAllddllst.Tables[5].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetAllddllst.Tables[5].Columns["ContactName"].ToString()),
                                          Code = n.Field<Int32>(DSGetAllddllst.Tables[5].Columns["PK_ContID"].ToString())

                                      }).ToList();
                }
                ViewBag.ContactType = lstContactName;

                if (DSGetAllddllst.Tables[6].Rows.Count > 0)//City
                {
                    lstCityName = (from n in DSGetAllddllst.Tables[6].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(DSGetAllddllst.Tables[6].Columns["CityName"].ToString()),
                                       Code = n.Field<Int32>(DSGetAllddllst.Tables[6].Columns["PK_ID"].ToString())

                                   }).ToList();
                }
                ViewBag.CityName = lstCityName;
                if (DSGetAllddllst.Tables[7].Rows.Count > 0)//Country
                {
                    lstCountryName = (from n in DSGetAllddllst.Tables[7].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetAllddllst.Tables[7].Columns["CountryName"].ToString()),
                                          Code = n.Field<Int32>(DSGetAllddllst.Tables[7].Columns["PK_ID"].ToString())

                                      }).ToList();
                }
                ViewBag.CountryName = lstCountryName;
                ViewBag.CountryNameItems = lstCountryName;

                if (DSGetAllddllst.Tables[8].Rows.Count > 0)//PortFolio
                {
                    lstPortfolioType = (from n in DSGetAllddllst.Tables[8].AsEnumerable()
                                        select new NameCode()
                                        {
                                            Name = n.Field<string>(DSGetAllddllst.Tables[8].Columns["Name"].ToString()),
                                            Code = n.Field<Int32>(DSGetAllddllst.Tables[8].Columns["PK_ID"].ToString())

                                        }).ToList();
                }
                ViewBag.PortfolioType = lstPortfolioType;

                if (DSGetAllddllst.Tables[9].Rows.Count > 0)//All Currency 
                {
                    lstCurrency = (from n in DSGetAllddllst.Tables[9].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(DSGetAllddllst.Tables[9].Columns["Name"].ToString()),
                                       Code = n.Field<Int32>(DSGetAllddllst.Tables[9].Columns["PK_ID"].ToString())

                                   }).ToList();
                }
                ViewBag.Currency = lstCurrency;
                IEnumerable<SelectListItem> CurrencyItems;
                CurrencyItems = new SelectList(lstCurrency, "Code", "Name");
                ViewBag.CurrencyItems = CurrencyItems;
                ViewBag.Dcurrency = CurrencyItems;
                ViewData["Dcurrency"] = CurrencyItems;




                //Adding Contact Model Poup Code Here
                DataSet DSGetAllDdlList = new DataSet();
                List<NameCode> lstCompanyList = new List<NameCode>();
                List<NameCode> lstTitleList = new List<NameCode>();
                DSGetAllDdlList = objDalEnquiryMaster.GetContactDdlList();
                if (DSGetAllDdlList.Tables[0].Rows.Count > 0)
                {
                    if (DSGetAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    {
                        lstCompanyList = (from n in DSGetAllDdlList.Tables[0].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetAllDdlList.Tables[0].Columns["Company_Name"].ToString()),
                                              Code = n.Field<Int32>(DSGetAllDdlList.Tables[0].Columns["CMP_ID"].ToString())

                                          }).ToList();
                    }
                    ViewBag.ContactCompanyName = lstCompanyList;
                    if (DSGetAllDdlList.Tables[1].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
                    {
                        lstTitleList = (from n in DSGetAllDdlList.Tables[1].AsEnumerable()
                                        select new NameCode()
                                        {
                                            Name = n.Field<string>(DSGetAllDdlList.Tables[1].Columns["TitleName"].ToString()),
                                            Code = n.Field<Int32>(DSGetAllDdlList.Tables[1].Columns["PK_TitleID"].ToString())

                                        }).ToList();
                    }
                    ViewBag.ContactTitleName = lstTitleList;
                }
                else
                {
                    ViewBag.ContactCompanyName = null;
                    ViewBag.ContactTitleName = null;
                }

            }
            return View();
        }
        [HttpPost]
        public ActionResult Enquiry(EnquiryMaster EM, string[] City, string[] Country, HttpPostedFileBase[] files, string C, List<DCurrency> DArray, List<ICurrency> IArray)
        {
            if (C == "1")
            {
                EM.chkArc = true;
            }
            else
            {
                EM.chkArc = false;
            }
           // EM.chkArc =Convert.ToBoolean(C);
            int EnquiryID = 0;
            string Result = string.Empty;
            string fileName = string.Empty;
            string IPath = string.Empty;
            string ECity = string.Empty;
            string ECountry = string.Empty;
            string EUploadDoc = string.Empty;
            double AutoOrderRate;
            var list = Session["list"] as List<string>;
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listUploadedFile"] as List<FileDetails>;

            //added by nikita on 25062024

            List<FileDetails> lstFileDtls_ = new List<FileDetails>();
            lstFileDtls_ = Session["listUploadedFile_"] as List<FileDetails>;

            List<FileDetails> NdaAttachment = new List<FileDetails>();
            NdaAttachment = Session["FileDetailsformats_"] as List<FileDetails>;

            if (City != null)
            {
                ECity = string.Join(",", City.ToArray().Where(s => !string.IsNullOrEmpty(s)));
                ECity = ECity.TrimEnd(',');
            }
            if (Country != null)
            {
                ECountry = string.Join(",", Country.ToArray().Where(s => !string.IsNullOrEmpty(s)));
                ECountry = ECountry.TrimEnd(',');
            }
            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }
            else
            {
                IPath = EM.DocumentAttached;
            }

            try
            {
                string strOBSType = string.Empty;
                string strPortfolioType = string.Empty;
                string strSubServiceType = string.Empty;
                strOBSType = EM.ProjectType.Length < 2 ? EM.ProjectType.ToString().Trim().PadLeft(2, '0') : EM.ProjectType;
                // strPortfolioType = EM.PortfolioType.Length < 2 ?EM.PortfolioType.ToString().Trim().PadLeft(2, '0') : EM.PortfolioType;
                //strSubServiceType = EM.SubServiceType.Length <2 ? EM.SubServiceType.ToString().Trim().PadLeft(2, '0') : EM.SubServiceType;
                strSubServiceType = objDalEnquiryMaster.GetServiceCode(Convert.ToInt32(EM.SubServiceType));
                strPortfolioType = objDalEnquiryMaster.GetProfileCode(Convert.ToInt32(EM.PortfolioType));

                EM.Type = strPortfolioType + strSubServiceType;
                // EM.PortfolioType = strPortfolioType;
                //    EM.SubServiceType = strSubServiceType;
                if (EM.EQ_ID != 0)
                {
                    Result = objDalEnquiryMaster.InsertAndUpdateEnquiry(EM, ECity, ECountry, IPath);
                    EnquiryID = EM.EQ_ID;
                    //#region Insert Update Order Type Domestic
                    //               if (DArray != null)
                    //               {
                    //                   foreach (var d in DArray)
                    //                   {
                    //                       EM.OrderType = d.OrderType;
                    //                       EM.OrderRate = d.OrderRate;
                    //                       EM.Estimate_ManDays_ManMonth = d.Estimate_ManDays;
                    //                       EM.Estimate_ManMonth = d.Estimate_ManMonth;
                    //                       EM.Distance = d.Distance;
                    //                       EM.DEstimatedAmount = d.EstimatedAmount;
                    //                       EM.Dcurrency = d.Currency;
                    //                       EM.DExchangeRate = d.ExchangeRate;
                    //                       EM.DTotalAmount = d.DTotalAmount;
                    //                       EM.EQ_ID = EnquiryID ;
                    //                       EM.Type = "D";
                    //                       EM.Remark = d.DRemark;
                    //                       if (d.OrderType.ToString().ToUpper() == "LUMSUM" || d.OrderType.ToString().ToUpper() == "PERC")
                    //                       {
                    //                           AutoOrderRate = (Convert.ToDouble(d.DTotalAmount) / Convert.ToDouble(d.Estimate_ManDays));
                    //                           EM.AutoOrderRate = AutoOrderRate.ToString();
                    //                       }
                    //                       Result = objDalEnquiryMaster.InsertUpdateOrderType(EM);
                    //                   }
                    //               }
                    //               #endregion

                    //               #region Insert Update Order Type International

                    //               if (IArray != null)
                    //               {
                    //                   foreach (var d in IArray)
                    //                   {
                    //                       EM.IOrderType = d.IOrderType;
                    //                       EM.IOrderRate = d.IOrderRate;
                    //                       EM.IEstimate_ManDays_ManMonth = d.IEstimate_ManDays;
                    //                       EM.IEstimate_ManMonth = d.IEstimate_ManMonth;
                    //                       EM.IDistance = d.IDistance;
                    //                       EM.IEstimatedAmount = d.IEstimatedAmount;
                    //                       EM.Icurrency = d.InternationalCurrency;
                    //                       EM.IExchangeRate = d.IExchangeRate;
                    //                       EM.ITotalAmount = d.ITotalAmount;
                    //                       EM.EQ_ID = EnquiryID;
                    //                       EM.Type = "I";
                    //                       EM.Remark = d.IRemark;
                    //                       if (d.IOrderType.ToString().ToUpper() == "LUMSUM" || d.IOrderType.ToString().ToUpper() == "PERC")
                    //                       {
                    //                           AutoOrderRate = (Convert.ToDouble(d.ITotalAmount) / Convert.ToDouble(d.IEstimate_ManDays));
                    //                           EM.AutoOrderRate = AutoOrderRate.ToString();
                    //                       }
                    //                       Result = objDalEnquiryMaster.InsertUpdateIOrderType(EM);
                    //                   }
                    //               }


                    //               #endregion
                    if (EnquiryID != null && EnquiryID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, EnquiryID);
                            Result = objDalEnquiryMaster.InsertFileAttachment(lstFileDtls, EnquiryID, null);
                            Session["listUploadedFile"] = null;
                        }
                        //added by nikita on 25062024
                        if (lstFileDtls_ != null && lstFileDtls_.Count > 0)
                        {
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls_, EnquiryID);
                            Result = objDalEnquiryMaster.InsertFileLegalAttachment(lstFileDtls_, EnquiryID, "Legal");
                            Session["listUploadedFile_"] = null;
                        }
                        if (NdaAttachment != null && NdaAttachment.Count > 0)
                        {
                            objCommonControl.SaveFileToPhysicalLocation(NdaAttachment, EnquiryID);
                            Result = objDalEnquiryMaster.InsertFileLegalAttachment(NdaAttachment, EnquiryID, "NDA");
                            Session["FileDetailsformats_"] = null;
                        }



                    }

                    //added by nikita on 25062024
                    var Roleid = Convert.ToString(Session["RoleID"]);
                    if (Roleid == "60")
                    {
                        if (EM.Source == "11")

                        {
                            DataTable dt_ = new DataTable();
                            dt_ = objDalEnquiryMaster.GetEnquiryNumber(EM.EQ_ID);
                            var Enquirynumber = dt_.Rows[0][0].ToString();
                            var legalreview = dt_.Rows[0][1].ToString();
                            var QuotationViewd = dt_.Rows[0][2].ToString();
                            var Comments = dt_.Rows[0][3].ToString();
                            var LegalApprovedBy = dt_.Rows[0][4].ToString();
                            var Createdby = dt_.Rows[0][5].ToString();
                            var Createdbymail_id = dt_.Rows[0][6].ToString();
                            var LegallyAprovedMailid = dt_.Rows[0][7].ToString();
                            var date = dt_.Rows[0][8].ToString();
                            var Branch = dt_.Rows[0][9].ToString();
                            var CompamyName = dt_.Rows[0][10].ToString();
                            if (legalreview == "1" && QuotationViewd == "1")
                            {
                                SendMAillegal_user2(Enquirynumber, LegalApprovedBy, Createdby, LegallyAprovedMailid, Createdbymail_id, Comments, date, Branch, CompamyName);
                            }
                            else if (legalreview == "1" && QuotationViewd == "0")
                            {
                                SendMAillegal_user(Enquirynumber, LegalApprovedBy, Createdby, LegallyAprovedMailid, Createdbymail_id, Comments, date, Branch, CompamyName);

                            }
                            else
                            {

                            }
                        }
                    }

                    //return Json(new { success = 2, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                    return Json(new { result = "Redirect", url = Url.Action("Enquiry", "EnquiryMaster", new { @EQ_ID = EnquiryID }) });
                }
                else
                {
                    Result = objDalEnquiryMaster.InsertAndUpdateEnquiry(EM, ECity, ECountry, IPath);
                    EnquiryID = Convert.ToInt32(Session["EnquiryIDs"]);

                    //added by nikita on 25062024
                    if (EM.Source == "11")
                    {
                        DataTable dt = new DataTable();
                        dt = objDalEnquiryMaster.GetEnquiryNumber(EnquiryID);
                        var Enquirynumber = dt.Rows[0][0].ToString();
                        var Createdby = dt.Rows[0][5].ToString();
                        var TUvEmailId = dt.Rows[0][7].ToString();
                        var Branch = dt.Rows[0][9].ToString();
                        var CompamyName = dt.Rows[0][10].ToString();


                        SendMAillegal(Enquirynumber, Createdby, TUvEmailId, Branch, CompamyName);
                    }
                    //#region Insert Update Order Type Domestic
                    //if (DArray != null)
                    //{
                    //    foreach (var d in DArray)
                    //    {
                    //        EM.OrderType = d.OrderType;
                    //        EM.OrderRate = d.OrderRate;
                    //        EM.Estimate_ManDays_ManMonth = d.Estimate_ManDays;
                    //        EM.Estimate_ManMonth = d.Estimate_ManMonth;
                    //        EM.Distance = d.Distance;
                    //        EM.DEstimatedAmount = d.EstimatedAmount;
                    //        EM.Dcurrency = d.Currency;
                    //        EM.DExchangeRate = d.ExchangeRate;
                    //        EM.DTotalAmount = d.DTotalAmount;
                    //        EM.EQ_ID = EnquiryID;
                    //        EM.Type = "D";
                    //        EM.Remark = d.DRemark;
                    //        Result = objDalEnquiryMaster.InsertUpdateOrderType(EM);
                    //    }
                    //}
                    //#endregion

                    //#region Insert Update Order Type International

                    //if (IArray != null)
                    //{
                    //    foreach (var d in IArray)
                    //    {
                    //        EM.IOrderType = d.IOrderType;
                    //        EM.IOrderRate = d.IOrderRate;
                    //        EM.IEstimate_ManDays_ManMonth = d.IEstimate_ManDays;
                    //        EM.IEstimate_ManMonth = d.IEstimate_ManMonth;
                    //        EM.IDistance = d.IDistance;
                    //        EM.IEstimatedAmount = d.IEstimatedAmount;
                    //        EM.Icurrency = d.InternationalCurrency;
                    //        EM.IExchangeRate = d.IExchangeRate;
                    //        EM.ITotalAmount = d.ITotalAmount;
                    //        EM.EQ_ID = EnquiryID;
                    //        EM.Type = "I";
                    //        EM.Remark = d.IRemark;
                    //        Result = objDalEnquiryMaster.InsertUpdateIOrderType(EM);
                    //    }
                    //}


                    //#endregion

                    #region Mail send For Conflict Enquiry

                    DataTable DTGetConflictDetail = new DataTable();
                    DTGetConflictDetail = objDalEnquiryMaster.GetConflictDetail(EnquiryID);

                    try
                    {
                        if (DTGetConflictDetail.Rows.Count > 0)
                        {
                            ObjModelEnquiry.ConflictType = Convert.ToString(DTGetConflictDetail.Rows[0]["ConflictType"]);
                            if (ObjModelEnquiry.ConflictType == "Conflict")
                            {
                                TempData["Conflict"] = "Conflict";
                                TempData.Keep();
                                SendConflictMail(EnquiryID);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string Error = ex.Message.ToString();
                    }

                    #endregion



                    if (EnquiryID != null && EnquiryID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, EnquiryID);
                            Result = objDalEnquiryMaster.InsertFileAttachment(lstFileDtls, EnquiryID, null);
                            Session["listUploadedFile"] = null;
                        }
                        //added by nikita on 25062024
                        if (lstFileDtls_ != null && lstFileDtls_.Count > 0)
                        {
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls_, EnquiryID);
                            Result = objDalEnquiryMaster.InsertFileLegalAttachment(lstFileDtls_, EnquiryID, "Legal");
                            Session["listUploadedFile_"] = null;
                        }
                        if (NdaAttachment != null && NdaAttachment.Count > 0)
                        {
                            objCommonControl.SaveFileToPhysicalLocation(NdaAttachment, EnquiryID);
                            Result = objDalEnquiryMaster.InsertFileLegalAttachment(NdaAttachment, EnquiryID, "NDA");
                            Session["FileDetailsformats_"] = null;
                        }
                    }
                    return Json(new { result = "Redirect", url = Url.Action("Enquiry", "EnquiryMaster", new { @EQ_ID = EnquiryID }) });
                }
                //// }
                // else
                // {

                // }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(new { success = 3, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
        }

        #region Code by  Rahul 
        [HttpPost]
        public JsonResult GetSubSubSectionType(string CompanyName)
        {
            DataTable DTGetContactlst = new DataTable();
            List<NameCode> lstContactName = new List<NameCode>();
            DTGetContactlst = objDalEnquiryMaster.GetInspector(CompanyName);
            //var Data = objDalEnquiryMaster.GetInspectorList(Convert.ToString(CompanyName));
            if (DTGetContactlst.Rows.Count > 0)
            {
                lstContactName = (from n in DTGetContactlst.AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DTGetContactlst.Columns["ContactName"].ToString()),
                                      Code = n.Field<Int32>(DTGetContactlst.Columns["PK_ContID"].ToString())

                                  }).ToList();
            }
            //ViewBag.ContactType = lstContactName;
            Session["CompanyNames"] = CompanyName;
            Session["CompanyNameInsertInContact"] = CompanyName;
            ObjModelEnquiry.DisplayCompanyName = CompanyName;
            TempData["DisplayCompanyName"] = CompanyName;
            return Json(lstContactName, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public JsonResult GetCompanyName(string Prefix)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (Convert.ToString(Session["CompanyNames"]) != null && Convert.ToString(Session["CompanyNames"]) != "")
            {
                Session["CompanyNames"] = null;
            }
            if (Prefix != null && Prefix != "")
            {
                DTResult = objDalEnquiryMaster.GetCompanyName(Prefix);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {
                               CompanyName = Convert.ToString(dr["CompanyName"]),
                               CompanyNames = Convert.ToString(dr["CompanyNames"]),
                               //PreviousClosing = Convert.ToDecimal(dr["BseCurrprice"]),
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
        public JsonResult GetCompanyNameSearch(string companyCode, string CompanyName)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (!string.IsNullOrEmpty(CompanyName))
            {
                DTResult = objDalEnquiryMaster.GetselectedCompanyName(CompanyName, companyCode);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {

                               Company_Name = Convert.ToString(dr["Company_Name"]),
                               CMP_ID = Convert.ToInt32(dr["CMP_ID"])

                           }
                        );
                    }
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCompanyNameTest(string data)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (Convert.ToString(Session["CompanyNames"]) != null && Convert.ToString(Session["CompanyNames"]) != "")
            {
                Session["CompanyNames"] = null;
            }
            if (data != null && data != "")
            {
                DTResult = objDalEnquiryMaster.GetCompanyNameTest(data);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {
                               CompanyName = Convert.ToString(dr["CompanyName"]),
                               CompanyNames = Convert.ToString(dr["CompanyNames"]),
                               CMP_ID = Convert.ToInt16(dr["CMP_ID"]),
                               //PreviousClosing = Convert.ToDecimal(dr["BseCurrprice"]),
                           }
                         );
                    }
                    Session["CompanyNames"] = Convert.ToString(DTResult.Rows[0]["CompanyNames"]);
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetComapnyDetails(string CompanyNames)
        {
            DataSet DSGetCompanyDetls = new DataSet();
            DSGetCompanyDetls = objDalEnquiryMaster.GetCompanyNameDetls(CompanyNames);
            if (DSGetCompanyDetls.Tables[0].Rows.Count > 0)
            {
                EnquiryMaster lstCompanyDtls = new EnquiryMaster
                {
                    EnquiryDescription = Convert.ToString(DSGetCompanyDetls.Tables[0].Rows[0]["CompanyName"]),
                    CompanyName = Convert.ToString(DSGetCompanyDetls.Tables[0].Rows[0]["CompanyName"]),
                    EndCustomer = Convert.ToString(DSGetCompanyDetls.Tables[0].Rows[0]["CompanyName"]),
                    Branch = Convert.ToString(DSGetCompanyDetls.Tables[0].Rows[0]["BranchID"])
                };
                return Json(lstCompanyDtls, JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindPedType(int SubType)
        {

            List<NameCode> ped = new List<NameCode>();
            ped = objDalEnquiryMaster.BindPedType(SubType);
            return Json(new SelectList(ped, "Code", "Name"));
        }
        public JsonResult BindOtherType(int OtherType)
        {

            List<NameCode> other = new List<NameCode>();
            other = objDalEnquiryMaster.BindPedType(OtherType);
            return Json(new SelectList(other, "Code", "Name"));
        }
        public JsonResult BindEnergyType(int EnergyType)
        {

            List<NameCode> energy = new List<NameCode>();
            energy = objDalEnquiryMaster.BindPedType(EnergyType);
            return Json(new SelectList(energy, "Code", "Name"));
        }
        public JsonResult BindCity(string City)
        {

            List<NameCode> city = new List<NameCode>();
            city = objDalEnquiryMaster.BindCity(City);
            ViewBag.CityName = city;
            return Json(new SelectList(city, "Code", "Name"));
        }
        public JsonResult BindCountry(string Country)
        {
            List<NameCode> country = new List<NameCode>();
            country = objDalEnquiryMaster.BindCountry(Country);
            ViewBag.CountryName = country;
            return Json(new SelectList(country, "Code", "Name"));
        }
        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            //Adding New Code 7 March 2020
            List<FileDetails> fileDetails = new List<FileDetails>();
            List<FileDetails> fileDetailsFormat = new List<FileDetails>();
            List<FileDetails> fileDetailsFormat_ = new List<FileDetails>();



            //---Adding end Code
            //if (Session["listUploadedFile"] != null)
            //{
            //    fileDetails = Session["listUploadedFile"] as List<FileDetails>;
            //}
            if (Session["listUploadedFile"] != null)
            {
                fileDetails = Session["listUploadedFile"] as List<FileDetails>;
            }
            if (Session["listUploadedFile_"] != null)
            {
                fileDetails = Session["listUploadedFile_"] as List<FileDetails>;
            }
            if (Session["FileDetailsformats_"] != null)
            {
                fileDetails = Session["FileDetailsformats_"] as List<FileDetails>;
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
                            //Adding New Code as per new requirement 7 March 2020, Manoj Sharma
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

                            //added by nikita on 25062024
                            if (Request.Files.Keys[0].ToString().ToUpper() == "FILEPOND")
                            {
                                fileDetails.Add(fileDetail);

                            }
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD2")
                            {
                                fileDetailsFormat.Add(fileDetail);

                            }
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD3")
                            {
                                fileDetailsFormat_.Add(fileDetail);

                            }

                            //fileDetails.Add(fileDetail);


                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/Files/Documents/"), fileDetail.Id + fileDetail.Extension);

                            var K = "~/Files/Documents/" + fileName;

                            IPath = K;//K.TrimStart('~');

                            //files.SaveAs(filePath);

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
                //Session["listUploadedFile"] = fileDetails;
                if (Request.Files.Keys[0].ToString().ToUpper() == "FILEPOND")
                {
                    Session["listUploadedFile"] = fileDetails;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD2")
                {
                    Session["listUploadedFile_"] = fileDetailsFormat;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD3")
                {
                    Session["FileDetailsformats_"] = fileDetailsFormat_;
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }
        //Adding Code Contact Details in Enquiry Modules for Popupu 17 June 2019 as Per Client Requirement
        [HttpGet]
        public ActionResult ContactDetails(int? CNT_ID)
        {


            DataSet DSGetAllDdlList = new DataSet();
            List<NameCode> lstCompanyList = new List<NameCode>();
            List<NameCode> lstTitleList = new List<NameCode>();
            string CompanyName1 = Convert.ToString(Session["CompanyNames"]);

            string CompanyName = Convert.ToString(Session["CompanyNameInsertInContact"]);



            DSGetAllDdlList = objDalEnquiryMaster.GetContactDdlList();
            if (DSGetAllDdlList.Tables[0].Rows.Count > 0)
            {
                if (DSGetAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                {
                    lstCompanyList = (from n in DSGetAllDdlList.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetAllDdlList.Tables[0].Columns["Company_Name"].ToString()),
                                          Code = n.Field<Int32>(DSGetAllDdlList.Tables[0].Columns["CMP_ID"].ToString())

                                      }).ToList();
                }
                ViewBag.ContactCompanyName = lstCompanyList;
                if (DSGetAllDdlList.Tables[0].Rows.Count > 0)
                {
                    ObjModelEnquiry.ContactCompanyName = Convert.ToString(DSGetAllDdlList.Tables[0].Rows[0]["Company_Name"]);
                }
                if (DSGetAllDdlList.Tables[1].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
                {
                    lstTitleList = (from n in DSGetAllDdlList.Tables[1].AsEnumerable()
                                    select new NameCode()
                                    {
                                        Name = n.Field<string>(DSGetAllDdlList.Tables[1].Columns["TitleName"].ToString()),
                                        Code = n.Field<Int32>(DSGetAllDdlList.Tables[1].Columns["PK_TitleID"].ToString())

                                    }).ToList();
                }
                ViewBag.ContactTitleName = lstTitleList;
            }
            //}
            return View();
        }


        [HttpPost]
        public ActionResult ContactDetails(EnquiryMaster ECM, FormCollection fc)
        {
            string Result = string.Empty;

            #region GetCompanyName


            string CompanyName = Convert.ToString(Session["CompanyNames"]);
            DataSet DSGetAllDdlList = new DataSet();
            List<NameCode> lstCompanyList = new List<NameCode>();
            //string cmpId = string.Empty;
            //cmpId = fc["CompId"].ToString();

            //ECM.ContactCompanyName = cmpId;

            DSGetAllDdlList = objDalEnquiryMaster.GetContactDdlList();
            if (DSGetAllDdlList.Tables[0].Rows.Count > 0)
            {
                if (DSGetAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                {
                    lstCompanyList = (from n in DSGetAllDdlList.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetAllDdlList.Tables[0].Columns["Company_Name"].ToString()),
                                          Code = n.Field<Int32>(DSGetAllDdlList.Tables[0].Columns["CMP_ID"].ToString())

                                      }).ToList();
                }
            }

            //ECM.CompanyName = DSGetAllDdlList.Tables[0].Rows[0]["Company_Name"].ToString();
            // ECM.ContactCompanyName = DSGetAllDdlList.Tables[0].Rows[0]["CMP_ID"].ToString();
            //ECM.Address = DSGetAllDdlList.Tables[0].Rows[0]["Address"].ToString();

            #endregion

            try
            {
                if (ECM.CompanyName != "" && ECM.CompanyName != null)
                {
                    DataTable dtContactDetailExist = new DataTable();

                    dtContactDetailExist = objDalCompany.ChkContactDetailExistFromEnquiry(ECM, CompanyName);

                    if (dtContactDetailExist.Rows.Count > 0)//chk duplication
                    {
                        return Json(new { success = 2, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                    }
                    else//Insert
                    {
                        Result = objDalEnquiryMaster.InsertUpdateContact(ECM);
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



        #region test
        [HttpGet]
        public ActionResult ContactDetailsPartial(int? CNT_ID)
        {
            DataSet DSGetAllDdlList = new DataSet();
            List<NameCode> lstCompanyList = new List<NameCode>();
            List<NameCode> lstTitleList = new List<NameCode>();
            string CompanyName = Convert.ToString(Session["CompanyNames"]);

            DSGetAllDdlList = objDalEnquiryMaster.GetContactDdlList();
            if (DSGetAllDdlList.Tables[0].Rows.Count > 0)
            {
                if (DSGetAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                {
                    lstCompanyList = (from n in DSGetAllDdlList.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetAllDdlList.Tables[0].Columns["Company_Name"].ToString()),
                                          Code = n.Field<Int32>(DSGetAllDdlList.Tables[0].Columns["CMP_ID"].ToString())

                                      }).ToList();
                }
                ViewBag.ContactCompanyName = lstCompanyList;
                if (DSGetAllDdlList.Tables[0].Rows.Count > 0)
                {
                    ObjModelEnquiry.ContactCompanyName = Convert.ToString(DSGetAllDdlList.Tables[0].Rows[0]["Company_Name"]);
                }
                if (DSGetAllDdlList.Tables[1].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
                {
                    lstTitleList = (from n in DSGetAllDdlList.Tables[1].AsEnumerable()
                                    select new NameCode()
                                    {
                                        Name = n.Field<string>(DSGetAllDdlList.Tables[1].Columns["TitleName"].ToString()),
                                        Code = n.Field<Int32>(DSGetAllDdlList.Tables[1].Columns["PK_TitleID"].ToString())

                                    }).ToList();
                }
                ViewBag.ContactTitleName = lstTitleList;
            }
            //}
            return View();
        }
        #endregion


        #region Delete Code By Rahul 
        public ActionResult DeleteEnquiry(int? EQ_ID, string reason)
        {
            int Result = 0;
            try
            {
                Result = objDalEnquiryMaster.DeleteEnquiry(EQ_ID, reason);
                if (Result != 0)
                {
                    TempData["DeleteCompany"] = Result;
                    return RedirectToAction("EnquiryMasterDashBoard", "EnquiryMaster");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        #endregion
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
                DTGetDeleteFile = objDalEnquiryMaster.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = objDalEnquiryMaster.DeleteUploadedFile(id);
                    var path = Path.Combine(Server.MapPath("~/Files/Documents/"), id + fileDetails.Extension);
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
            // return File(Path.Combine(Server.MapPath("~/Files/Documents/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalEnquiryMaster.GetFileContent(Convert.ToInt32(d));

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
            DTDownloadFile = objDalEnquiryMaster.GetFileContent(Convert.ToInt32(d));

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

        #region Export to excel
        [HttpGet]
        public ActionResult ExportIndex(String Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<EnquiryMaster> grid = CreateExportableGrid(Type);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<EnquiryMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "EnquiryMaster-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<EnquiryMaster> CreateExportableGrid(String Type)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<EnquiryMaster> grid = new Grid<EnquiryMaster>(GetData(Type));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.EnquiryNumber).Titled("Enquiry Number");
            grid.Columns.Add(model => model.EnquiryDescription).Titled("Project Name");
            grid.Columns.Add(model => model.Company_Name).Titled("Customer Name");
            grid.Columns.Add(model => model.ProjectType).Titled("OBS Type");
            grid.Columns.Add(model => model.PortfolioType).Titled("Service Portfolio Type");
            grid.Columns.Add(model => model.Type).Titled("Service Type");
            grid.Columns.Add(model => model.Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.OpendateS).Titled("Enquiry Creation Date");
            grid.Columns.Add(model => model.Owner).Titled("Created By");//added by nikita change title name
            grid.Columns.Add(model => model.RegretStatus).Titled("Status");


            grid.Pager = new GridPager<EnquiryMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelEnquiry.lstEnquiryMast.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<EnquiryMaster> GetData(String Type)
        {

            Session["UserLoginID"] = User.Identity.IsAuthenticated;
            string UserRole = Convert.ToString(Session["role"]);


            lstEnquiryMast = objDalEnquiryMaster.GetEnquiryListDashBoard(Type);
            ObjModelEnquiry.lstEnquiryMast = lstEnquiryMast;
            return ObjModelEnquiry.lstEnquiryMast;
        }
        #endregion

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

        [HttpPost]
        public JsonResult GetProfile(int Project)
        {
            DataTable DTGetProfileList = new DataTable();
            List<NameCode> lstProfileList = new List<NameCode>();
            DTGetProfileList = objDalEnquiryMaster.GetProfileList(Project);

            if (DTGetProfileList.Rows.Count > 0)
            {
                lstProfileList = (from n in DTGetProfileList.AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DTGetProfileList.Columns["Name"].ToString()),
                                      Code = n.Field<Int32>(DTGetProfileList.Columns["PK_ID"].ToString())

                                  }).ToList();
            }



            return Json(lstProfileList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSubServiceList(int Portfolio)
        {
            DataTable DTGetProfileList = new DataTable();
            List<NameCode> lstProfileList = new List<NameCode>();
            DTGetProfileList = objDalEnquiryMaster.GetSubServiceList(Portfolio);

            if (DTGetProfileList.Rows.Count > 0)
            {
                lstProfileList = (from n in DTGetProfileList.AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DTGetProfileList.Columns["Name"].ToString()),
                                      Code = n.Field<Int32>(DTGetProfileList.Columns["PK_ID"].ToString())

                                  }).ToList();
                return Json(lstProfileList, JsonRequestBehavior.AllowGet);

            }
            return Json("Failed", JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult GetCompanyAddress(int? CNT_ID)
        {
            if (Convert.ToInt32(CNT_ID) != 0)
            {
                DataTable DTGetCompanyAddress = new DataTable();
                DTGetCompanyAddress = objDalEnquiryMaster.GetEnquiryDetals(Convert.ToInt32(CNT_ID));
                if (DTGetCompanyAddress.Rows.Count > 0)
                {
                    ObjModelEnquiry.Address = Convert.ToString(DTGetCompanyAddress.Rows[0]["CompanyAddress"]);
                }
            }
            return Json(ObjModelEnquiry, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckValidCompany(string companyname)//Checking Existing User Name
        {
            string Result = string.Empty;
            DataTable DTExistRoleName = new DataTable();
            try
            {
                DTExistRoleName = objDalEnquiryMaster.GetCompany(companyname);
                if (DTExistRoleName.Rows.Count > 0)
                {
                    return Json(1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(0);
        }

        public JsonResult GetLeadByName(string Prefix)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (Prefix != null && Prefix != "")
            {
                DTResult = objDalEnquiryMaster.GetUserName(Prefix);
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


        #region Conflict Jobs

        [HttpGet]
        public ActionResult GetConflictJob(int PK_EQID)
        {

            ObjModelEnquiry.EQ_ID = PK_EQID;
            #region Get ConflictDetail 
            DataTable DTGetConflictDetail = new DataTable();
            DTGetConflictDetail = objDalEnquiryMaster.GetConflictDetail1(PK_EQID);

            try
            {
                if (DTGetConflictDetail.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTGetConflictDetail.Rows)
                    {
                        lstConflictJobs.Add(
                            new EnquiryMaster
                            {
                                PK_Job_Id = Convert.ToInt32(dr["PK_Job_Id"]),
                                JobNumber = Convert.ToString(dr["Job_Number"]),
                                EnquiryNumber = Convert.ToString(dr["Enquiry_Of_Order"]),
                                CompanyName = Convert.ToString(dr["Client_Name"]),
                                SubServiceType = Convert.ToString(dr["ServiceName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ObjModelEnquiry.lstConflictJobs = lstConflictJobs;
            ViewBag.lstConflictJobs = lstConflictJobs;
            #endregion


            #region Get Approval Detail multiple Comment

            //DataTable GetConflictApproval = new DataTable();
            //List<EnquiryMaster> lstCompanyDashBoard = new List<EnquiryMaster>();
            //GetConflictApproval = objDalEnquiryMaster.GetConflictApprovalDetail(PK_EQID);
            //if (GetConflictApproval.Rows.Count > 0)
            //{

            //    foreach (DataRow dr in GetConflictApproval.Rows)
            //    {



            //        lstCompanyDashBoard.Add(
            //            new EnquiryMaster
            //            {
            //                Reason = Convert.ToString(dr["Reason"]),
            //            });
            //    }
            //}

            //ViewBag.lstIOrderType = lstCompanyDashBoard;

            #endregion

            //643545
            #region Get Approval Detail
            DataTable GetConflictApproval = new DataTable();
            GetConflictApproval = objDalEnquiryMaster.GetConflictApprovalDetail(PK_EQID);
            if (GetConflictApproval.Rows.Count > 0)
            {
                ObjModelEnquiry.Reason = Convert.ToString(GetConflictApproval.Rows[0]["Reason"]);
                ObjModelEnquiry.PCHApproval = Convert.ToString(GetConflictApproval.Rows[0]["ConfPCHApproval"]);
                ObjModelEnquiry.CHApproval = Convert.ToString(GetConflictApproval.Rows[0]["ConfCHApproval"]);
                ObjModelEnquiry.QAApproval = Convert.ToString(GetConflictApproval.Rows[0]["ConfAdminQAApproval"]);

                ObjModelEnquiry.PCHID = Convert.ToString(GetConflictApproval.Rows[0]["PCH_Name"]);
                ObjModelEnquiry.CHID = Convert.ToString(GetConflictApproval.Rows[0]["CH_NAME"]);
                ObjModelEnquiry.BranchQAID = Convert.ToString(GetConflictApproval.Rows[0]["BranchQA"]);
                ObjModelEnquiry.SessionId = System.Web.HttpContext.Current.Session["UserIDs"].ToString();
            }
            #endregion


            //}
            return View(ObjModelEnquiry);
        }

        [HttpPost]
        public ActionResult GetConflictJob(EnquiryMaster CM)
        {
            string Result = string.Empty;

            try
            {
                Result = objDalEnquiryMaster.InsertAndUpdateMetegatedReason(CM);
                if (Result != null && Result != "")
                {
                    TempData["UpdateCostSheet"] = Result;
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("GetConflictJob", new { PK_EQID = CM.EQ_ID });

        }


        public ActionResult ChangeApprovalStatus(int EQ_ID, string Type, string Status)
        {
            string Result = string.Empty;

            try
            {
                Result = objDalEnquiryMaster.ChangeStatus(EQ_ID, Type, Status);

                SendApprovedNotApprovedMail(EQ_ID, Type, Status);
                if (Result != null && Result != "")
                {
                    TempData["UpdateCostSheet"] = Result;
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("GetConflictJob", new { PK_EQID = EQ_ID });

        }

        #region send Approved/Not Approved Mail
        public void SendApprovedNotApprovedMail(int EQ_ID, string Type, string Status)
        {
            string displayName = string.Empty;
            string ClientEmail = string.Empty;
            string bodyTxt = string.Empty;
            //string number = voucherno;
            DataTable DTGetConflictDetail = new DataTable();
            DTGetConflictDetail = objDalEnquiryMaster.GetConflictAdminDForApprovalMail(EQ_ID);

            try
            {
                if (DTGetConflictDetail.Rows.Count > 0)
                {
                    ObjModelEnquiry.PCHApproval = Convert.ToString(DTGetConflictDetail.Rows[0]["PCHEmail"]);
                    ObjModelEnquiry.CHApproval = Convert.ToString(DTGetConflictDetail.Rows[0]["CHEmail"]);
                    //ObjModelEnquiry.QAApproval = Convert.ToString(DTGetConflictDetail.Rows[0]["BranchQA"]);
                    ObjModelEnquiry.CreatedBy = Convert.ToString(DTGetConflictDetail.Rows[0]["Tuv_Email_Id"]);
                    ObjModelEnquiry.PCHName = Convert.ToString(DTGetConflictDetail.Rows[0]["PCHName"]);
                    ObjModelEnquiry.CHName = Convert.ToString(DTGetConflictDetail.Rows[0]["CHName"]);
                    ObjModelEnquiry.ConflictEnquiryCreatedbyEmail = Convert.ToString(DTGetConflictDetail.Rows[0]["CreatedbyName"]);

                    if (Type == "PCH")
                    {
                        ObjModelEnquiry.ConflictApprovalName = Convert.ToString(DTGetConflictDetail.Rows[0]["PCHName"]);
                    }
                    else
                    {
                        ObjModelEnquiry.ConflictApprovalName = Convert.ToString(DTGetConflictDetail.Rows[0]["CHName"]);
                    }
                    if (Status == "1")
                    {
                        ObjModelEnquiry.ConflictApprovalName = "Approved";

                    }
                    else
                    {
                        ObjModelEnquiry.ConflictApprovalName = "Not Approved";
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }



            try
            {

                string CcExtra = "";
                string CC_1 = ObjModelEnquiry.PCHApproval;
                string CC_2 = ObjModelEnquiry.CHApproval;
                string CC_3 = "vaipatil@tuv-nord.com";
                string CC_4 = "pshrikant@tuv-nord.com";

                //string CC_3 = "vaipatil@tuv-nord.com";
                string Tomails_ = ObjModelEnquiry.CreatedBy;

                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string ToEmail = Tomails_;
                string CcEmail = $"{CC_1};{CC_2};{CC_3};{CC_4}";


                bodyTxt = $@"
            <html>
                <head>
                    <title></title>
                </head>
                <body>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Dear { ObjModelEnquiry.ConflictEnquiryCreatedbyEmail  }, </span>
                        <br/><br/>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>{ObjModelEnquiry.ConflictApprovalName  } has {ObjModelEnquiry.ApprovedNotApproved} Enquiry.</span>
                        <br/><br/>
                    </div>
                    <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Best regards,<br/>TUV India Pvt Ltd.</span>
                    </div>
                    <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Note: This is an auto-generated mail. Please do not reply.</span>
                    </div>
                </body>
            </html>";

                foreach (var email in ToEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.To.Add(new MailAddress(email));
                }

                foreach (var email in CcEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.CC.Add(new MailAddress(email));
                }

                msg.From = new MailAddress(MailFrom, ClientEmail);
                msg.Subject = "TIIMES – Conflict";
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion


        [HttpGet]
        public ActionResult GetConflictEnquiry(int PK_EQID)
        {

            ObjModelEnquiry.EQ_ID = PK_EQID;
            #region Get ConflictDetail 

            #region Get ConflictDetail 
            DataTable DTGetConflictDetail = new DataTable();
            DTGetConflictDetail = objDalEnquiryMaster.GetConflictEnquiryD(PK_EQID);

            try
            {
                if (DTGetConflictDetail.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTGetConflictDetail.Rows)
                    {
                        lstConflictEnquiry.Add(
                            new EnquiryMaster
                            {

                                EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                                CompanyName = Convert.ToString(dr["CompanyName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            //ObjModelEnquiry.lstConflictJobs = lstConflictEnquiry;
            ViewBag.lstConflictEnquiry = lstConflictEnquiry;
            #endregion

            #endregion



            //643545
            #region Get Approval Detail
            DataTable GetConflictConfirmation = new DataTable();
            GetConflictConfirmation = objDalEnquiryMaster.GetConflictConfirmationDetail(PK_EQID);
            if (GetConflictConfirmation.Rows.Count > 0)
            {
                ObjModelEnquiry.ConflictConfirmation = Convert.ToBoolean(GetConflictConfirmation.Rows[0]["ConflictConfirmation"]);
                ObjModelEnquiry.SessionId = System.Web.HttpContext.Current.Session["UserIDs"].ToString();
            }
            #endregion


            //}
            return View(ObjModelEnquiry);
        }


        [HttpPost]
        public ActionResult GetConflictEnquiry(EnquiryMaster EQ)
        {
            string Result = string.Empty;

            try
            {
                Result = objDalEnquiryMaster.ChangeStatusConflictEnquiry(EQ);
                if (Result != null && Result != "")
                {
                    TempData["UpdateCostSheet"] = Result;
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("GetConflictEnquiry", new { PK_EQID = EQ.EQ_ID });

        }

        #region Conflict Mail
        public void SendConflictMail(int EnquiryNumber)
        {
            string displayName = string.Empty;
            string ClientEmail = string.Empty;
            string bodyTxt = string.Empty;
            //string number = voucherno;
            DataTable DTGetConflictDetail = new DataTable();
            DTGetConflictDetail = objDalEnquiryMaster.GetConflictAdminD(EnquiryNumber);

            try
            {
                if (DTGetConflictDetail.Rows.Count > 0)
                {
                    ObjModelEnquiry.PCHApproval = Convert.ToString(DTGetConflictDetail.Rows[0]["PCHEmail"]);
                    ObjModelEnquiry.CHApproval = Convert.ToString(DTGetConflictDetail.Rows[0]["CHEmail"]);
                    //ObjModelEnquiry.QAApproval = Convert.ToString(DTGetConflictDetail.Rows[0]["BranchQA"]);
                    ObjModelEnquiry.CreatedBy = Convert.ToString(DTGetConflictDetail.Rows[0]["Tuv_Email_Id"]);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }



            try
            {

                string CcExtra = "";
                string CC_1 = ObjModelEnquiry.PCHApproval;
                string CC_2 = ObjModelEnquiry.CHApproval;
                string CC_3 = "vaipatil@tuv-nord.com";
                string CC_4 = "pshrikant@tuv-nord.com";
                string Tomails_ = ObjModelEnquiry.CreatedBy;

                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string ToEmail = Tomails_;
                string CcEmail = $"{CC_1};{CC_2};{CC_3};{CC_4}";


                bodyTxt = $@"
            <html>
                <head>
                    <title></title>
                </head>
                <body>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Dear Sir, ,</span>
                        <br/><br/>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>This Enquiry is conflicting with Existing Jobs</span>
                        <br/><br/>
                    </div>
                    <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Best regards,<br/>TUV India Pvt Ltd.</span>
                    </div>
                    <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Note: This is an auto-generated mail. Please do not reply.</span>
                    </div>
                </body>
            </html>";

                foreach (var email in ToEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.To.Add(new MailAddress(email));
                }

                foreach (var email in CcEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.CC.Add(new MailAddress(email));
                }

                msg.From = new MailAddress(MailFrom, ClientEmail);
                msg.Subject = "TIIMES – Conflict";
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion


        #endregion


        //added by nikita on 25062024


        public void SendMAillegal(string EnquiryNumber, string Createdby, string Createdbymail_id, string Branch, string CompanyName)
        {
            string displayName = string.Empty;
            string ClientEmail = string.Empty;
            string bodyTxt = string.Empty;



            //string number = voucherno;

            try
            {
                DataTable dt = objDalEnquiryMaster.GetLegalMail();
                var LegalApproval = dt.Rows[0][1].ToString();
                var LegalApprovalEmailId = dt.Rows[0][0].ToString();
                string CcExtra = "rohini@tuv-nord.com";
                string CCnikita = "nikita.yadav@tuvindia.co.in";
                string CCmails = "pshrikant@tuv-nord.com";

                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string ToEmail = LegalApprovalEmailId;
                string CcEmail = $"{CcExtra};{CCnikita};{CCmails}";

                //string CcEmail = $"{coordinatorEmail};{BranchQA};{ApprovalName_1};{ApprovalName_2};{PCH_Name};{CcExtra};{CCnikita};{CCmails},{CCmails_}";
                //{ CCmails}
                bodyTxt = $@"
            <html>
                <head>
                    <title></title>
                </head>
                <body>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span>
                        <br/><br/>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>{Createdby} has sent you Request to review the Tender’s legal clauses of Enquiry Number <a href='https://tiimes.tuv-india.com/EnquiryMaster/Enquiry?enquiryNumber={EnquiryNumber}'>{EnquiryNumber}</a> ,  for Customer {CompanyName}, Branch {Branch}.</span>
                        <br/><br/>
                    </div>
                    <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Best regards,<br/>TUV India Pvt Ltd.</span>
                    </div>
                    <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Note: This is an auto-generated mail. Please do not reply.</span>
                    </div>
                </body>
            </html>";

                foreach (var email in ToEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.To.Add(new MailAddress(email));
                }

                foreach (var email in CcEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.CC.Add(new MailAddress(email));
                }

                msg.From = new MailAddress(MailFrom, ClientEmail);
                msg.Subject = $"TIIMES-BDSM-{EnquiryNumber}-{Branch}- Tender’s legal clauses review";
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void SendMAillegal_user(string EnquiryNumber, string LegalApprovedBy, string Createdby, string LegallyAprovedMailid, string Createdbymail_id, string Comments, string Date, string Branch, string customerName)
        {
            string displayName = string.Empty;
            string ClientEmail = string.Empty;
            string bodyTxt = string.Empty;
            string Comments_ = Comments;
            try
            {
                string CcExtra = "rohini@tuv-nord.com";
                string CCnikita = "nikita.yadav@tuvindia.co.in";
                string CCmails = "pshrikant@tuv-nord.com";

                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string ToEmail = Createdbymail_id;
                string CcEmail = $"{CcExtra};{CCnikita};{CCmails}";

                //string CcEmail = $"{coordinatorEmail};{BranchQA};{ApprovalName_1};{ApprovalName_2};{PCH_Name};{CcExtra};{CCnikita};{CCmails},{CCmails_}";
                //{ CCmails}
                bodyTxt = $@"
            <html>
                <head>
                    <title></title>
                </head>
                <body>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Dear {Createdby},</span>
                        <br/><br/>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Enquiry  Number <a href='https://tiimes.tuv-india.com/EnquiryMaster/Enquiry?enquiryNumber={EnquiryNumber}'>{EnquiryNumber}</a> , for Customer {customerName} of {Branch}, Tenders legal clauses were reviewed by {LegalApprovedBy} with below Comments -</span>
                        <br/>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>{Comments}.</span>

                        <br/>
                    </div>
                    </div>
                    <br/>
                        
                       <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Best regards,<br/>TUV India Pvt Ltd.</span>
                    </div>
                    <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Note: This is an auto-generated mail. Please do not reply.</span>
                    </div>
                </body>
            </html>";

                foreach (var email in ToEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.To.Add(new MailAddress(email));
                }

                foreach (var email in CcEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.CC.Add(new MailAddress(email));
                }

                msg.From = new MailAddress(MailFrom, ClientEmail);
                msg.Subject = $"TIIMES-BDSM-{EnquiryNumber}-{Branch}- Tender’s legal clauses review Status -";
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //added by nikita 

        public void SendMAillegal_user2(string EnquiryNumber, string LegalApprovedBy, string Createdby, string LegallyAprovedMailid, string Createdbymail_id, string Comments, string Date, string Branch, string customerName)
        {
            string displayName = string.Empty;
            string ClientEmail = string.Empty;
            string bodyTxt = string.Empty;
            string Comments_ = Comments;
            try
            {
                string CcExtra = "rohini@tuv-nord.com";
                string CCnikita = "nikita.yadav@tuvindia.co.in";
                string CCmails = "pshrikant@tuv-nord.com";

                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string ToEmail = Createdbymail_id;
                string CcEmail = $"{CcExtra};{CCnikita};{CCmails}";

                //string CcEmail = $"{coordinatorEmail};{BranchQA};{ApprovalName_1};{ApprovalName_2};{PCH_Name};{CcExtra};{CCnikita};{CCmails},{CCmails_}";
                //{ CCmails}
                bodyTxt = $@"
            <html>
                <head>
                    <title></title>
                </head>
                <body>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Dear {Createdby},</span>
                        <br/><br/>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Enquiry  Number <a href='https://tiimes.tuv-india.com/EnquiryMaster/Enquiry?enquiryNumber={EnquiryNumber}'>{EnquiryNumber}</a> , for Customer {customerName} of {Branch}, Tenders legal clauses were reviewed by {LegalApprovedBy} with below Comments and allowed to take further action.</span>
                        <br/>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>{Comments}.</span>

                        <br/>
                    </div>
                    </div>
                    <br/>
                        
                       <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Best regards,<br/>TUV India Pvt Ltd.</span>
                    </div>
                    <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Note: This is an auto-generated mail. Please do not reply.</span>
                    </div>
                </body>
            </html>";

                foreach (var email in ToEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.To.Add(new MailAddress(email));
                }

                foreach (var email in CcEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.CC.Add(new MailAddress(email));
                }

                msg.From = new MailAddress(MailFrom, ClientEmail);
                msg.Subject = $"TIIMES-BDSM-{EnquiryNumber}-{Branch}- Tender’s legal clauses review Status -";
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        [HttpGet]
        public JsonResult GetAddressOfCompany(string Company_Name)
        {

            #region Bind Company Addr
            DataSet DsCompanyAddr = new DataSet();
            List<NameCode> lstCompanyAddr = new List<NameCode>();
            DsCompanyAddr = objDalEnquiryMaster.GetCompanyAddrVendor(Company_Name);
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

            var address = objDalCompany.GetAddressByName(Company_Name);
            return Json(lstCompanyAddr, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetIndustries()
        {
            DataSet DSEditGetList = objDalCompany.GetAllDdlLst();
            List<NameCode> lstEditIndustryList = new List<NameCode>();

            if (DSEditGetList.Tables[2].Rows.Count > 0)
            {
                lstEditIndustryList = (from n in DSEditGetList.Tables[2].AsEnumerable()
                                       select new NameCode()
                                       {
                                           Name = n.Field<string>("IndustryName"),
                                           Code = n.Field<Int32>("PK_IndID")
                                       }).ToList();
            }

            return Json(lstEditIndustryList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CreateCompany(CompanyMaster CM, HttpPostedFileBase Upload)
        {
            int CompanyId = 0;
            try
            {
                if (CM.CMP_ID != 0)
                {
                    CompanyId = objDalCompany.InsertUpdateCompany(CM);
                    if (CompanyId > 0)
                    {
                        TempData["UpdateCompany"] = CompanyId;
                        return Json(new { success = 2, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                        //return RedirectToAction("CreateCompany", "CompanyMasters", new { @CompanyID = CompanyId });
                    }
                }
                else
                {
                    CompanyId = objDalCompany.InsertUpdateCompany(CM);
                    if (CompanyId > 0)
                    {

                        TempData["InsertCompany"] = CompanyId;
                        // return RedirectToAction("CreateCompany", "CompanyMasters", new { @CompanyID = CompanyId });
                        return Json(new { success = 1, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(new { success = 3, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCompany(int? CompanyID)
        {
            int Result = 0;
            try
            {
                Result = objDalCompany.DeleteCompany(CompanyID);
                if (Result != 0)
                {
                    TempData["DeleteCompany"] = Result;
                    return Json(new { success = 1, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("CompanyDashBoard", "CompanyMasters");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }


        [HttpPost]
        public JsonResult getCompanyData(string CompanyID)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (!string.IsNullOrEmpty(CompanyID))
            {
                DTResult = objDalEnquiryMaster.GetselectedCompanyData(CompanyID);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {

                               Company_Name = Convert.ToString(dr["Company_Name"]),
                               CMP_ID = Convert.ToInt32(dr["CMP_ID"]),
                               InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                               Address = Convert.ToString(dr["Address"]),
                               OffAddrPin = Convert.ToString(dr["OffAddrPin"]),
                               Pan_No = Convert.ToString(dr["Pan_No"]),
                               Ind_ID = Convert.ToInt32(dr["Ind_ID"]),
                           }
                        );
                    }
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetSearchCompanyName(string CompanyName)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (!string.IsNullOrEmpty(CompanyName))
            {
                DTResult = objDalEnquiryMaster.GetSearchCompanyNametoshow(CompanyName);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {
                               CMP_ID = Convert.ToInt32(dr["Cmp_ID"]),
                               CompanyName = Convert.ToString(dr["Company_Name"]),
                           }
                        );
                    }
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult vendorContactDetails(EnquiryMaster ECM, FormCollection fc)
        {
            string Result = string.Empty;

            #region GetCompanyName


            string CompanyName = Convert.ToString(Session["CompanyNames"]);
            DataSet DSGetAllDdlList = new DataSet();
            List<NameCode> lstCompanyList = new List<NameCode>();
            //string cmpId = string.Empty;
            //cmpId = fc["CompId"].ToString();

            //ECM.ContactCompanyName = cmpId;

            //DSGetAllDdlList = objDalEnquiryMaster.GetContactDdlList();
            //if (DSGetAllDdlList.Tables[0].Rows.Count > 0)
            //{
            //    if (DSGetAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
            //    {
            //        lstCompanyList = (from n in DSGetAllDdlList.Tables[0].AsEnumerable()
            //                          select new NameCode()
            //                          {
            //                              Name = n.Field<string>(DSGetAllDdlList.Tables[0].Columns["Company_Name"].ToString()),
            //                              Code = n.Field<Int32>(DSGetAllDdlList.Tables[0].Columns["CMP_ID"].ToString())

            //                          }).ToList();
            //    }
            //}

            //ECM.CompanyName = DSGetAllDdlList.Tables[0].Rows[0]["Company_Name"].ToString();
            // ECM.ContactCompanyName = DSGetAllDdlList.Tables[0].Rows[0]["CMP_ID"].ToString();
            //ECM.Address = DSGetAllDdlList.Tables[0].Rows[0]["Address"].ToString();

            #endregion

            try
            {
                if (ECM.CompanyName != "" && ECM.CompanyName != null)
                {
                    DataTable dtContactDetailExist = new DataTable();

                    dtContactDetailExist = objDalCompany.ChkContactDetailExistFromEnquiry(ECM, CompanyName);

                    if (dtContactDetailExist.Rows.Count > 0)//chk duplication
                    {
                        return Json(new { success = 2, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);
                    }
                    else//Insert
                    {
                        Result = objDalEnquiryMaster.InsertUpdatevendorContact(ECM);
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


        [HttpPost]
        public JsonResult GetSearchCompanyData(string CompanyName)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (!string.IsNullOrEmpty(CompanyName))
            {
                DTResult = objDalEnquiryMaster.GetSearchCompanyName(CompanyName);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {
                               CompanyCode = Convert.ToString(dr["Customer"]),
                               CustomerSearch = Convert.ToString(dr["CompanyName"]),
                               Address = Convert.ToString(dr["Address"]),
                               Email = Convert.ToString(dr["EmailAddress"]),
                               Pan_No = Convert.ToString(dr["PanCard"]),
                               GST_NO = Convert.ToString(dr["GST"]),
                               New_Cmp = Convert.ToInt32(dr["new_Cmp"]),
                               State = Convert.ToString(dr["State"]),
                               VATRegistrationNo = Convert.ToString(dr["VATRegistrationNo"]),
                           }
                        );
                    }
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsapCompanyName(string Prefix)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (Convert.ToString(Session["CompanyNames"]) != null && Convert.ToString(Session["CompanyNames"]) != "")
            {
                Session["CompanyNames"] = null;
            }
            if (Prefix != null && Prefix != "")
            {
                DTResult = objDalEnquiryMaster.GetSapCompanyName(Prefix);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {
                               CompanyName = Convert.ToString(dr["CompanyName"]),
                               CompanyNames = Convert.ToString(dr["CompanyNames"]),
                               //PreviousClosing = Convert.ToDecimal(dr["BseCurrprice"]),
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
        public JsonResult GetEnquiry(string CompanyName, int EQ_Id, int CMP_id)
        {
            DataSet ds = objDalEnquiryMaster.Get_Enquirydetails(CompanyName, EQ_Id, CMP_id);

            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject(new List<object>());
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetBudgetaryEnqiry(string CompanyName, int EQ_Id, int CMP_id)
        {
            DataSet ds = objDalEnquiryMaster.Get_EnquiryBudgetarydetails(CompanyName, EQ_Id, CMP_id);

            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject(new List<object>());
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


    }
}