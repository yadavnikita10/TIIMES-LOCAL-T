using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class CustComplaintController : Controller
    {

        ComplaitRegister CMREegister = new ComplaitRegister();
        DalCustSpecific custSpecific = new DalCustSpecific();
        DALCalls objDalCalls = new DALCalls();
        DALAudit objDALAudit = new DALAudit();
        DALComplaintRegisterMaster objDAM = new DALComplaintRegisterMaster();
        DALCallMaster objDAM1 = new DALCallMaster();



        public ActionResult ComplaintsDashBoard()
        {
            Session["GetExcelData"] = "Yes";
            DataTable DTComplaintDashBoard = new DataTable();
            List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();
            DTComplaintDashBoard = objDAM.GetComplaintDashBoard();
            string showData = string.Empty;
            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    showData = DTComplaintDashBoard.Rows[0]["showData1"].ToString();

                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new ComplaitRegister
                            {
                                PK_Complaint_ID = Convert.ToInt32(dr["PK_Complaint_ID"]),
                                Complaint_No = Convert.ToString(dr["Complaint_No"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Complaint_Date = Convert.ToString(dr["Complaint_Date"]),
                                Control_No = Convert.ToString(dr["Control_No"]),
                                Complaint_Mode = Convert.ToString(dr["Complaint_Mode"]),
                                //End = Convert.ToString(dr["Complaint_Mode"]),
                                TUV_Client = Convert.ToString(dr["TUV_Client"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector_Name = Convert.ToString(dr["Inspector"]),
                                Complaint_Details = Convert.ToString(dr["Complaint_Details"]),
                                Correction = Convert.ToString(dr["Correction"]),
                                Root_Cause = Convert.ToString(dr["Root_Cause"]),
                                CA_To_Prevent_Recurrance = Convert.ToString(dr["CA_To_Prevent_Recurrance"]),
                                Effectiveness_Of_Implementation_Of_CA = Convert.ToString(dr["Effectiveness_Of_Implementation_Of_CA"]),
                                Date_Of_Aknowledgement = Convert.ToString(dr["Date_Of_Aknowledgement"]),
                                Date_Of_PreliminaryReply = Convert.ToString(dr["Date_Of_PreliminaryReply"]),
                                Date_Of_FinalReply = Convert.ToString(dr["Date_Of_FinalReply"]),
                                Date_Of_Action = Convert.ToString(dr["Date_Of_Action"]),
                                Category = Convert.ToString(dr["Category1"]),
                                Remarks = Convert.ToString(dr["Remarks"]),
                                EndUser = Convert.ToString(dr["EndUser"]),
                                States_Of_Complaints = Convert.ToString(dr["States_Of_Complaints"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                AttributeToFaultiInspection = Convert.ToString(dr["AttributeToFaultiInspection"]),
                                showData1 = Convert.ToString(dr["showData1"]),

                            }
                            );
                    }
                    ViewData["ComplaintList"] = lstComplaintDashBoard;
                    CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
                    CMREegister.showData1 = showData;
                    return View(CMREegister);
                }
                else
                {
                    ViewData["ComplaintList"] = lstComplaintDashBoard;
                    CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
                    return View(CMREegister);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            //ViewData["ComplaintList"] = lstComplaintDashBoard;
            //CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
            return View();
        }

        [HttpPost]
        public ActionResult ComplaintsDashBoard(ComplaitRegister c)
        {
            if (c.FromDate == null || c.FromDate == "")
            {
                return RedirectToAction("ComplaintsDashBoard");
            }
            Session["GetExcelData"] = null;
            Session["FromDate"] = c.FromDate;
            Session["ToDate"] = c.ToDate;
            string showData = string.Empty;
            DataTable DTComplaintDashBoard = new DataTable();
            List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();
            DTComplaintDashBoard = objDAM.GetComplaintDashBoardByDate(c);
            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    showData = DTComplaintDashBoard.Rows[0]["showData1"].ToString();
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new ComplaitRegister
                            {
                                PK_Complaint_ID = Convert.ToInt32(dr["PK_Complaint_ID"]),
                                Complaint_No = Convert.ToString(dr["Complaint_No"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Complaint_Date = Convert.ToString(dr["Complaint_Date"]),
                                Control_No = Convert.ToString(dr["Control_No"]),
                                Complaint_Mode = Convert.ToString(dr["Complaint_Mode"]),
                                TUV_Client = Convert.ToString(dr["TUV_Client"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector_Name = Convert.ToString(dr["Inspector"]),
                                Complaint_Details = Convert.ToString(dr["Complaint_Details"]),
                                Correction = Convert.ToString(dr["Correction"]),
                                Root_Cause = Convert.ToString(dr["Root_Cause"]),
                                CA_To_Prevent_Recurrance = Convert.ToString(dr["CA_To_Prevent_Recurrance"]),
                                Effectiveness_Of_Implementation_Of_CA = Convert.ToString(dr["Effectiveness_Of_Implementation_Of_CA"]),
                                Date_Of_Aknowledgement = Convert.ToString(dr["Date_Of_Aknowledgement"]),
                                Date_Of_PreliminaryReply = Convert.ToString(dr["Date_Of_PreliminaryReply"]),
                                Date_Of_FinalReply = Convert.ToString(dr["Date_Of_FinalReply"]),
                                Date_Of_Action = Convert.ToString(dr["Date_Of_Action"]),
                                Category = Convert.ToString(dr["Category1"]),
                                Remarks = Convert.ToString(dr["Remarks"]),
                                EndUser = Convert.ToString(dr["EndUser"]),
                                States_Of_Complaints = Convert.ToString(dr["States_Of_Complaints"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                AttributeToFaultiInspection = Convert.ToString(dr["AttributeToFaultiInspection"]),
                                showData1 = Convert.ToString(dr["showData1"])
                            }
                            );

                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["ComplaintList"] = lstComplaintDashBoard;
            CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
            CMREegister.showData1 = showData;
            return View(CMREegister);
        }
        [HttpGet]
        public ActionResult CreateComplaint(int? PK_Complaint_ID)
        {
            // ComplaintMail(128);
            string[] splitedAuditorName;
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

            #region Bind Product category
            ViewData["Drpproduct"] = objDAM1.GetDrpList();
            #endregion

            #region Bind Complaint category
            ViewData["Category1"] = objDAM.GetComplaintCategory();
            #endregion

            if (PK_Complaint_ID != 0 && PK_Complaint_ID != null)
            {
                string[] splitedInspector_Name;
                //ViewBag.check = "inspector";
                ViewBag.check = "AuditorName";
                DataTable DTComplaint = new DataTable();
                ViewBag.check = "productcheck";
                ViewBag.check1 = "productcheck1";
                string[] splitedProduct_Name;
                string[] splitedCategory;

                DTComplaint = objDAM.EditComplaints(PK_Complaint_ID);

                //string insbranch = CMREegister.allInspector_Name;
                //List<string> lstbr = new List<string>();
                //lstbr = insbranch.Split(',').ToList();
                //List<string> lstt = new List<string>();
                //ViewData["branchofinspector"] = lstbr;
                string varins = CMREegister.Executing_Branch;





                if (DTComplaint.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTComplaint.Rows)
                    {
                        CMREegister.Complaint_No = Convert.ToString(DTComplaint.Rows[0]["Complaint_No"]);
                        CMREegister.Complaint_Date = Convert.ToString(DTComplaint.Rows[0]["Complaint_Date"]);
                        CMREegister.Attachment = Convert.ToString(DTComplaint.Rows[0]["Attachment"]);
                        CMREegister.Control_No = Convert.ToString(DTComplaint.Rows[0]["Control_No"]);
                        CMREegister.Complaint_Mode = Convert.ToString(DTComplaint.Rows[0]["Complaint_Mode"]);
                        CMREegister.TUV_Client = Convert.ToString(DTComplaint.Rows[0]["TUV_Client"]);
                        CMREegister.Originating_Branch = Convert.ToString(DTComplaint.Rows[0]["Originating_Branch"]);
                        CMREegister.Executing_Branch = Convert.ToString(DTComplaint.Rows[0]["Executing_Branch"]);

                        CMREegister.Inspector_Name = Convert.ToString(DTComplaint.Rows[0]["Inspector_Name"]);

                        CMREegister.Complaint_Details = Convert.ToString(DTComplaint.Rows[0]["Complaint_Details"]);
                        CMREegister.Correction = Convert.ToString(DTComplaint.Rows[0]["Correction"]);
                        CMREegister.Root_Cause = Convert.ToString(DTComplaint.Rows[0]["Root_Cause"]);
                        CMREegister.CA_To_Prevent_Recurrance = Convert.ToString(DTComplaint.Rows[0]["CA_To_Prevent_Recurrance"]);
                        CMREegister.Effectiveness_Of_Implementation_Of_CA = Convert.ToString(DTComplaint.Rows[0]["Effectiveness_Of_Implementation_Of_CA"]);
                        CMREegister.Date_Of_Aknowledgement = Convert.ToString(DTComplaint.Rows[0]["Date_Of_Aknowledgement"]);
                        CMREegister.Date_Of_PreliminaryReply = Convert.ToString(DTComplaint.Rows[0]["Date_Of_PreliminaryReply"]);
                        CMREegister.Date_Of_FinalReply = Convert.ToString(DTComplaint.Rows[0]["Date_Of_FinalReply"]);
                        CMREegister.Date_Of_Action = Convert.ToString(DTComplaint.Rows[0]["Date_Of_Action"]);
                        CMREegister.Category = Convert.ToString(DTComplaint.Rows[0]["Category"]);
                        CMREegister.Remarks = Convert.ToString(DTComplaint.Rows[0]["Remarks"]);
                        CMREegister.EndUser = Convert.ToString(DTComplaint.Rows[0]["EndUser"]);
                        CMREegister.States_Of_Complaints = Convert.ToString(DTComplaint.Rows[0]["States_Of_Complaints"]);
                        CMREegister.LastDateOfInspection = Convert.ToString(DTComplaint.Rows[0]["LastDateOfInspection"]);
                        CMREegister.AttributeToFaultiInspection = Convert.ToString(DTComplaint.Rows[0]["AttributeToFaultiInspection"]);
                        CMREegister.Vendor = Convert.ToString(DTComplaint.Rows[0]["Vendor"]);
                        CMREegister.SubVendor = Convert.ToString(DTComplaint.Rows[0]["SubVendor"]);
                        CMREegister.LessonLearned = Convert.ToString(DTComplaint.Rows[0]["LessonLearned"]);
                        CMREegister.ActionTaken = Convert.ToString(DTComplaint.Rows[0]["ActionTaken"]);
                        CMREegister.ProjectName = Convert.ToString(DTComplaint.Rows[0]["ProjectName"]);
                        CMREegister.ShareLessonsLearnt = Convert.ToBoolean(DTComplaint.Rows[0]["ShareLessonsLearnt"]);

                        CMREegister.Call_no = Convert.ToString(DTComplaint.Rows[0]["CallNo"]);
                        CMREegister.ReferenceDocument = Convert.ToString(DTComplaint.Rows[0]["ReferenceDocument"]);
                        // CMREegister.Hide = Convert.ToBoolean(DTComplaint.Rows[0]["Hide2"]);

                        //Added by Ankush for Delete file and update file
                        List<string> Selected = new List<string>();
                        var Existingins = Convert.ToString(DTComplaint.Rows[0]["Inspector_Name"]);
                        splitedInspector_Name = Existingins.Split(',');
                        foreach (var single in splitedInspector_Name)
                        {
                            Selected.Add(single);
                        }
                        ViewBag.EditinspectorName = Selected;

                        List<string> Selected1 = new List<string>();
                        var Existingins1 = Convert.ToString(DTComplaint.Rows[0]["Product_item"]);
                        splitedProduct_Name = Existingins1.Split(',');
                        foreach (var single1 in splitedProduct_Name)
                        {
                            Selected1.Add(single1);
                        }
                        ViewBag.EditproductName = Selected1;

                        #region BindCategory
                        List<string> Selected5 = new List<string>();
                        var Existingins5 = Convert.ToString(DTComplaint.Rows[0]["Category"]);
                        splitedCategory = Existingins5.Split(',');
                        foreach (var single6 in splitedCategory)
                        {
                            Selected5.Add(single6);
                        }
                        ViewBag.EditCompCategory = Selected5;
                        #endregion


                        #region Get All Category
                        DataTable DTGetAllCategory = new DataTable();
                        List<NameCodeins> lstEditCategory = new List<NameCodeins>();
                        DTGetAllCategory = objDAM.GetComplaint();

                        if (DTGetAllCategory.Rows.Count > 0)
                        {
                            lstEditCategory = (from n in DTGetAllCategory.AsEnumerable()
                                               select new NameCodeins()
                                               {
                                                   Name = n.Field<string>(DTGetAllCategory.Columns["Name"].ToString()),
                                                   PkUserId = n.Field<string>(DTGetAllCategory.Columns["Id"].ToString())

                                               }).ToList();
                        }

                        IEnumerable<SelectListItem> CategoryItems;
                        CategoryItems = new SelectList(lstEditCategory, "PkUserId", "Name");
                        ViewBag.Category = CategoryItems;
                        ViewData["Category"] = CategoryItems;
                        #endregion


                        #region Bind Inspector
                        DataTable DTGetInspectorLst = new DataTable();
                        List<NameCodeins> lstEditInspector = new List<NameCodeins>();
                        DTGetInspectorLst = objDAM.GetInspector(CMREegister.Executing_Branch);

                        if (DTGetInspectorLst.Rows.Count > 0)
                        {
                            lstEditInspector = (from n in DTGetInspectorLst.AsEnumerable()
                                                select new NameCodeins()
                                                {
                                                    Name = n.Field<string>(DTGetInspectorLst.Columns["InspectorName"].ToString()),
                                                    PkUserId = n.Field<string>(DTGetInspectorLst.Columns["PK_UserID"].ToString())

                                                }).ToList();
                        }

                        IEnumerable<SelectListItem> inspectorItems;
                        inspectorItems = new SelectList(lstEditInspector, "PkUserId", "Name");
                        ViewBag.ProjectTypeItems = inspectorItems;
                        ViewData["ProjectTypeItems"] = inspectorItems;
                        #endregion


                        #region Get inspector Name
                        List<string> SelectedAuditorName = new List<string>();
                        var EAuditorName = Convert.ToString(DTComplaint.Rows[0]["Inspector_Name"]);
                        splitedAuditorName = EAuditorName.Split(',');
                        foreach (var single1 in splitedAuditorName)
                        {
                            SelectedAuditorName.Add(single1);
                        }
                        ViewBag.AuditorNameName = SelectedAuditorName;
                        #endregion
                    }

                    DataTable DTGetProductLst = new DataTable();
                    List<NameCodeProduct> lstEditInspector1 = new List<NameCodeProduct>();
                    DTGetProductLst = objDAM1.getlistforEdit();

                    if (DTGetProductLst.Rows.Count > 0)
                    {
                        lstEditInspector1 = (from n in DTGetProductLst.AsEnumerable()
                                             select new NameCodeProduct()
                                             {
                                                 Name = n.Field<string>(DTGetProductLst.Columns["Name"].ToString())


                                             }).ToList();
                    }

                    IEnumerable<SelectListItem> ProductcheckItems;
                    ProductcheckItems = new SelectList(lstEditInspector1, "Name", "Name");
                    ViewBag.ProjectTypeItems = ProductcheckItems;


                    //Added by Ankush for Delete file and update file
                    DataTable DTGetUploadedFile = new DataTable();
                    List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                    DTGetUploadedFile = objDAM.EditUploadedFile(PK_Complaint_ID);
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
                        CMREegister.FileDetails = lstEditFileDetails;
                    }

                    /********************** 4 Attachments ******************************************/

                    DataTable DTAttach1 = new DataTable();
                    List<FileDetails> lstAttach1 = new List<FileDetails>();
                    DTAttach1 = objDAM.EditAttUploadedFile(PK_Complaint_ID, "1");

                    if (DTAttach1.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTAttach1.Rows)
                        {
                            lstAttach1.Add(
                               new FileDetails
                               {

                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstAttach1"] = lstAttach1;
                        CMREegister.ATT1 = lstAttach1;
                    }

                    DataTable DTAttach2 = new DataTable();
                    List<FileDetails> lstAttach2 = new List<FileDetails>();
                    DTAttach2 = objDAM.EditAttUploadedFile(PK_Complaint_ID, "2");

                    if (DTAttach2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTAttach2.Rows)
                        {
                            lstAttach2.Add(
                               new FileDetails
                               {

                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstAttach2"] = lstAttach2;
                        CMREegister.ATT2 = lstAttach2;
                    }

                    DataTable DTAttach3 = new DataTable();
                    List<FileDetails> lstAttach3 = new List<FileDetails>();
                    DTAttach3 = objDAM.EditAttUploadedFile(PK_Complaint_ID, "3");

                    if (DTAttach3.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTAttach3.Rows)
                        {
                            lstAttach3.Add(
                               new FileDetails
                               {

                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstAttach3"] = lstAttach3;
                        CMREegister.ATT3 = lstAttach3;
                    }

                    DataTable DTAttach4 = new DataTable();
                    List<FileDetails> lstAttach4 = new List<FileDetails>();
                    DTAttach4 = objDAM.EditAttUploadedFile(PK_Complaint_ID, "4");

                    if (DTAttach4.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTAttach4.Rows)
                        {
                            lstAttach4.Add(
                               new FileDetails
                               {

                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstAttach4"] = lstAttach4;
                        CMREegister.ATT4 = lstAttach4;
                    }


                    /*********************************************************************************************/


                    return View(CMREegister);
                }
                else
                {
                    return View();
                }

            }

            var Data = objDalCalls.GetInspectorList();
            ViewBag.SubCatlist = new SelectList(Data, "PK_UserID", "FirstName");

            return View(CMREegister);
        }

        [HttpPost]
        public ActionResult CreateComplaint(ComplaitRegister UserComplaint, HttpPostedFileBase File, string bname, FormCollection fc, HttpPostedFileBase[] Image)
        {
            #region Added by Ankush
            int CMPID = 0;
            var IPath = string.Empty;
            string[] splitedGrp;

            List<string> Selected = new List<string>();
            //List<FileDetails> lstFileDtls = new List<FileDetails>();
            //fileDetails = Session["listCMPUploadedFile"] as List<FileDetails>;
            #endregion
            string ProList = string.Join(",", fc["ProductList"]);
            UserComplaint.ProductList = ProList;

            string ProList1 = string.Join(",", fc["Category"]);
            UserComplaint.Category = ProList1;

            List<FileDetails> fileDetails = new List<FileDetails>();

            List<FileDetails> fileAtt1 = new List<FileDetails>();
            List<FileDetails> fileAtt2 = new List<FileDetails>();
            List<FileDetails> fileAtt3 = new List<FileDetails>();
            List<FileDetails> fileAtt4 = new List<FileDetails>();



            #region Added by Ankush
            FormCollection fca = new FormCollection();
            string filePathCMP = string.Empty;
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
                        FileDetails fileDetail = new FileDetails();
                        fileDetail.FileName = fileName;
                        fileDetail.Extension = Path.GetExtension(fileName);
                        fileDetail.Id = Guid.NewGuid();
                        fileDetails.Add(fileDetail);
                        //-----------------------------------------------------
                        filePathCMP = Path.Combine(Server.MapPath("~/ComplaintFolder/"), fileDetail.Id + fileDetail.Extension);
                        var K = "~/ComplaintFolder/" + fileName;
                        IPath = K;
                        files.SaveAs(filePathCMP);
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
            Session["listCMPUploadedFile"] = fileDetails;
            #endregion

            string Result = string.Empty;
            //string insname = string.Join(",", fc["InspectorName"]); 
            string insname = string.Join(",", fc["AuditorName"]);
            UserComplaint.Inspector_Name = insname;


            try
            {
                if (UserComplaint.PK_Complaint_ID != 0)
                {

                    Result = objDAM.InsertUpdateComplaintData(UserComplaint);
                    CMPID = UserComplaint.PK_Complaint_ID;
                    if (CMPID != null && CMPID != 0)
                    {

                        fileDetails = Session["ListComplaintFile"] as List<FileDetails>;

                        fileAtt1 = Session["ListFileAtt1"] as List<FileDetails>;
                        fileAtt2 = Session["ListFileAtt2"] as List<FileDetails>;
                        fileAtt3 = Session["ListFileAtt3"] as List<FileDetails>;
                        fileAtt4 = Session["ListFileAtt4"] as List<FileDetails>;


                        if (fileDetails != null && fileDetails.Count > 0)
                        {
                            Result = objDAM.InsertFileAttachment(fileDetails, CMPID);
                            Session["ListComplaintFile"] = null;
                        }

                        if (fileAtt1 != null && fileAtt1.Count > 0)
                        {
                            Result = objDAM.InsertAttFileAttachment(fileAtt1, CMPID, "1");
                            Session["ListFileAtt1"] = null;
                        }

                        if (fileAtt2 != null && fileAtt2.Count > 0)
                        {
                            Result = objDAM.InsertAttFileAttachment(fileAtt2, CMPID, "2");
                            Session["ListFileAtt2"] = null;
                        }

                        if (fileAtt3 != null && fileAtt3.Count > 0)
                        {
                            Result = objDAM.InsertAttFileAttachment(fileAtt3, CMPID, "3");
                            Session["ListFileAtt3"] = null;
                        }

                        if (fileAtt4 != null && fileAtt4.Count > 0)
                        {
                            Result = objDAM.InsertAttFileAttachment(fileAtt4, CMPID, "4");
                            Session["ListFileAtt4"] = null;
                        }




                    }
                    if (CMPID != 0 && CMPID != null)
                    {
                        TempData["UpdateComplaints"] = Result;
                        //return RedirectToAction("ComplaintsDashBoard");
                        return RedirectToAction("CreateComplaint", new { PK_Complaint_ID = CMPID });
                    }
                }
                else
                {
                    //if (UserComplaint.Attachment != null)
                    //{
                    //    UserComplaint.Attachment = CommonControl.FileUpload("Content/Uploads/Attachments/", UserComplaint.Attachment);
                    //}
                    int _min = 100000000;
                    int _max = 999999999;
                    Random _rdm = new Random();
                    int Rjno = _rdm.Next(_min, _max);
                    string ConfirmCode = Convert.ToString(Rjno);
                    UserComplaint.Complaint_No = "CN" + Convert.ToString(ConfirmCode);

                    Result = objDAM.InsertUpdateComplaintData(UserComplaint);

                    CMPID = Convert.ToInt32(Session["CMPIDs"]);
                    if (CMPID != null && CMPID != 0)
                    {

                        fileDetails = Session["ListComplaintFile"] as List<FileDetails>;

                        fileAtt1 = Session["ListFileAtt1"] as List<FileDetails>;
                        fileAtt2 = Session["ListFileAtt2"] as List<FileDetails>;
                        fileAtt3 = Session["ListFileAtt3"] as List<FileDetails>;
                        fileAtt4 = Session["ListFileAtt4"] as List<FileDetails>;


                        if (fileDetails != null && fileDetails.Count > 0)
                        {
                            Result = objDAM.InsertFileAttachment(fileDetails, CMPID);
                            Session["ListComplaintFile"] = null;
                        }

                        if (fileAtt1 != null && fileAtt1.Count > 0)
                        {
                            Result = objDAM.InsertAttFileAttachment(fileAtt1, CMPID, "1");
                            Session["ListFileAtt1"] = null;
                        }

                        if (fileAtt2 != null && fileAtt2.Count > 0)
                        {
                            Result = objDAM.InsertAttFileAttachment(fileAtt2, CMPID, "2");
                            Session["ListFileAtt2"] = null;
                        }

                        if (fileAtt3 != null && fileAtt3.Count > 0)
                        {
                            Result = objDAM.InsertAttFileAttachment(fileAtt3, CMPID, "3");
                            Session["ListFileAtt3"] = null;
                        }

                        if (fileAtt4 != null && fileAtt4.Count > 0)
                        {
                            Result = objDAM.InsertAttFileAttachment(fileAtt4, CMPID, "4");
                            Session["ListFileAtt4"] = null;
                        }

                        ComplaintMail(CMPID);
                    }
                    TempData["insertComplaints"] = Result;

                    //return RedirectToAction("ComplaintsDashBoard");
                    return RedirectToAction("CreateComplaint", new { PK_Complaint_ID = CMPID });
                }
            }
            catch (Exception ex)
            {

                string Error = ex.Message.ToString();
            }
            return View();
        }

        public JsonResult GetBranch(string bname)
        {
            DataTable dt = new DataTable();
            List<ComplaitRegister> lst = new List<ComplaitRegister>();
            dt = objDAM.GetInspector(bname);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(
                        new ComplaitRegister
                        {
                            Inspector_Name = Convert.ToString(dr["InspectorName"]),
                            PKUserId = Convert.ToString(dr["PK_UserID"]) //19Nov

                        }
                        );
                }
            }
            ViewData["inspector"] = lst;
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int ComplaintID)
        {
            int Result = 0;
            try
            {
                Result = objDAM.DeleteComplaint(ComplaintID);
                if (Result != 0)
                {
                    TempData["Result"] = Result;
                    return RedirectToAction("ComplaintsDashBoard");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        public JsonResult GetAutoCompleteDataOriginatingName(string BranchName)
        {

            DataTable DTComplaintDashBoard = new DataTable();
            List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();
            DTComplaintDashBoard = objDAM.AutoCompletDAta();

            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new ComplaitRegister
                            {
                                PK_Complaint_ID = Convert.ToInt32(dr["Br_Id"]),
                                Originating_Branch = Convert.ToString(dr["Branch_Name"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            var getList = from n in lstComplaintDashBoard
                          where n.Originating_Branch.ToUpper().StartsWith(BranchName.ToUpper())
                          select new { n.Originating_Branch };
            return Json(getList);

        }
        public JsonResult getAutoControlno(string input)
        {
            DataTable DTcontrol = new DataTable();
            List<ComplaitRegister> clst = new List<ComplaitRegister>();
            DTcontrol = objDAM.getcontrolNodata();
            try
            {
                if (DTcontrol.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTcontrol.Rows)
                    {
                        clst.Add(
                            new ComplaitRegister
                            {
                                Control_No = Convert.ToString(dr["Job_Number"])
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            var getcontrolnolst = from n in clst
                                  where n.Control_No.StartsWith(input)
                                  select new
                                  {
                                      n.Control_No
                                  };
            return Json(getcontrolnolst);
        }
        public JsonResult GetAutoCompleteDataExecutingName(string BranchName)
        {
            DataTable DTComplaintDashBoard = new DataTable();
            List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();
            DTComplaintDashBoard = objDAM.AutoCompletDAta();

            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new ComplaitRegister
                            {
                                PK_Complaint_ID = Convert.ToInt32(dr["Br_Id"]),
                                Executing_Branch = Convert.ToString(dr["Branch_Name"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            var getList = from n in lstComplaintDashBoard
                          where n.Executing_Branch.ToUpper().StartsWith(BranchName.ToUpper())
                          select new { n.Executing_Branch };

            return Json(getList);
        }
        //public JsonResult GetInspectorNameforExecutingBranch(string BranchName)
        //{
        //    DataTable dt = new DataTable();
        //    List<ComplaitRegister> lst = new List<ComplaitRegister>();
        //    dt = objDAM.GetComplaintDashBoard();
        //    try
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                lst.Add(
        //                    new ComplaitRegister
        //                    {
        //                        Inspector_Name = Convert.ToString(dr["Inspector_Name"]),
        //                        Executing_Branch = Convert.ToString(dr["Executing_Branch"])
        //                    });
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        string error = ex.Message.ToString();
        //    }
        //    BranchName = BranchName.ToLower();

        //    var getinspector = from s in lst
        //                       where s.Executing_Branch.ToLower() == BranchName
        //                       select new { s.Inspector_Name};
        //    return Json(getinspector);
        //}

        //#region Export to excel

        //[HttpGet]
        //public ActionResult ExportIndex(ComplaitRegister c)
        //{
        //    // Using EPPlus from nuget
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        Int32 row = 2;
        //        Int32 col = 1;

        //        package.Workbook.Worksheets.Add("Data");
        //        IGrid<ComplaitRegister> grid = CreateExportableGrid(c);



        //        ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

        //        foreach (IGridColumn column in grid.Columns)
        //        {
        //            sheet.Cells[1, col].Value = column.Title;
        //            sheet.Column(col++).Width = 18;

        //            column.IsEncoded = false;
        //        }

        //        foreach (IGridRow<ComplaitRegister> gridRow in grid.Rows)
        //        {
        //            col = 1;
        //            foreach (IGridColumn column in grid.Columns)
        //                sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
        //            row++;
        //        }

        //        return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
        //    }
        //}

        //private IGrid<ComplaitRegister> CreateExportableGrid(ComplaitRegister c)
        //{
        //    //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
        //    IGrid<ComplaitRegister> grid = new Grid<ComplaitRegister>(GetData(c));

        //    grid.ViewContext = new ViewContext { HttpContext = HttpContext };


        //    /// grid.Columns.Add(model => model.PK_Complaint_ID).Titled("Complaint ID");
        //    grid.Columns.Add(model => model.Complaint_No).Titled("Complaint Number");
        //    grid.Columns.Add(model => model.Complaint_Date).Titled("Complaint Date");
        //    grid.Columns.Add(model => model.Control_No).Titled("Control No");
        //    grid.Columns.Add(model => model.Complaint_Mode).Titled("Complaint Mode");
        //    grid.Columns.Add(model => model.TUV_Client).Titled("Client Name And End User Name");
        //    grid.Columns.Add(model => model.EndUser).Titled("Vendor / Sub - Vendor Name");
        //    grid.Columns.Add(model => model.ProjectName).Titled("Project Name");
        //    grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
        //    grid.Columns.Add(model => model.Executing_Branch).Titled("Executing Branch");
        //    grid.Columns.Add(model => model.Inspector_Name).Titled("Inspector Name");
        //    grid.Columns.Add(model => model.Category).Titled("Category");
        //    grid.Columns.Add(model => model.Complaint_Details).Titled("Complaint Details");
        //    grid.Columns.Add(model => model.Root_Cause).Titled("Root Cause Analysis");
        //    grid.Columns.Add(model => model.Correction).Titled("Correction");
        //    grid.Columns.Add(model => model.CA_To_Prevent_Recurrance).Titled("Corrective Action");

        //    /// grid.Columns.Add(model => model.Attachment).Titled("Attachment");           
        //    //grid.Columns.Add(model => model.CA_To_Prevent_Recurrance).Titled("CA To Prevent Recurrence");

        //    grid.Columns.Add(model => model.LastDateOfInspection).Titled("Last Date of Inspection");
        //    grid.Columns.Add(model => model.Date_Of_Aknowledgement).Titled("Date Of Acknowledgment");
        //    grid.Columns.Add(model => model.Date_Of_PreliminaryReply).Titled("Date Of Preliminary Reply");
        //    grid.Columns.Add(model => model.Date_Of_FinalReply).Titled("Date Of Final Reply");
        //    grid.Columns.Add(model => model.Date_Of_Action).Titled("Date Of Corrective Action");
        //    grid.Columns.Add(model => model.AttributeToFaultiInspection).Titled("Attribute To Faulti Inspection");
        //    grid.Columns.Add(model => model.Effectiveness_Of_Implementation_Of_CA).Titled("Effectiveness Of Implementation Of CA");
        //    grid.Columns.Add(model => model.Remarks).Titled("Remarks");
        //    grid.Columns.Add(model => model.States_Of_Complaints).Titled("Status Of Complaints");
        //    grid.Columns.Add(model => model.ModifiedBy).Titled("Modified By");
        //    grid.Columns.Add(model => model.ModifiedDate).Titled("Modified Date");


        //    grid.Pager = new GridPager<ComplaitRegister>(grid);
        //    grid.Processors.Add(grid.Pager);
        //    grid.Pager.RowsPerPage = CMREegister.lstComplaintDashBoard1.Count;

        //    foreach (IGridColumn column in grid.Columns)
        //    {
        //        column.Filter.IsEnabled = true;
        //        column.Sort.IsEnabled = true;
        //    }

        //    return grid;
        //}

        //public List<ComplaitRegister> GetData(ComplaitRegister c)
        //{

        //    DataTable DTComplaintDashBoard = new DataTable();
        //    List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();

        //    if (Session["GetExcelData"] == "Yes")
        //    {
        //        DTComplaintDashBoard = objDAM.GetComplaintDashBoard();
        //    }
        //    else
        //    {

        //        c.FromDate = Session["FromDate"].ToString();
        //        c.ToDate = Session["ToDate"].ToString();
        //        DTComplaintDashBoard = objDAM.GetComplaintDashBoardByDate(c);
        //    }


        //    try
        //    {
        //        if (DTComplaintDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in DTComplaintDashBoard.Rows)
        //            {
        //                lstComplaintDashBoard.Add(
        //                    new ComplaitRegister
        //                    {
        //                        Count = DTComplaintDashBoard.Rows.Count,
        //                        PK_Complaint_ID = Convert.ToInt32(dr["PK_Complaint_ID"]),
        //                        Complaint_No = Convert.ToString(dr["Complaint_No"]),
        //                        Attachment = Convert.ToString(dr["Attachment"]),
        //                        Complaint_Date = Convert.ToString(dr["Complaint_Date"]),
        //                        Control_No = Convert.ToString(dr["Control_No"]),
        //                        Complaint_Mode = Convert.ToString(dr["Complaint_Mode"]),
        //                        TUV_Client = Convert.ToString(dr["TUV_Client"]),
        //                        Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
        //                        Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
        //                        Inspector_Name = Convert.ToString(dr["Inspector_Name"]),
        //                        Complaint_Details = Convert.ToString(dr["Complaint_Details"]),
        //                        Correction = Convert.ToString(dr["Correction"]),
        //                        Root_Cause = Convert.ToString(dr["Root_Cause"]),
        //                        CA_To_Prevent_Recurrance = Convert.ToString(dr["CA_To_Prevent_Recurrance"]),
        //                        Effectiveness_Of_Implementation_Of_CA = Convert.ToString(dr["Effectiveness_Of_Implementation_Of_CA"]),
        //                        Date_Of_Aknowledgement = Convert.ToString(dr["Date_Of_Aknowledgement"]),
        //                        Date_Of_PreliminaryReply = Convert.ToString(dr["Date_Of_PreliminaryReply"]),
        //                        Date_Of_FinalReply = Convert.ToString(dr["Date_Of_FinalReply"]),
        //                        Date_Of_Action = Convert.ToString(dr["Date_Of_Action"]),
        //                        Category = Convert.ToString(dr["Category"]),
        //                        Remarks = Convert.ToString(dr["Remarks"]),
        //                        EndUser = Convert.ToString(dr["EndUser"]),
        //                        States_Of_Complaints = Convert.ToString(dr["States_Of_Complaints"]),
        //                    }
        //                    );

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    ViewData["ComplaintList"] = lstComplaintDashBoard;
        //    CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;

        //    return CMREegister.lstComplaintDashBoard1;
        //}




        //#endregion

        #region Export to excel

        [HttpGet]
        public ActionResult ExportIndex(ComplaitRegister c)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                DataTable grid = CreateExportableGrid(c);

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                int colcount = 1;
                int rowCount = 1;
                string strPeriod = string.Empty;
                if ((Session["FromDate"] != null && Session["FromDate"] != string.Empty) && (Session["ToDate"] != null && Session["ToDate"] != string.Empty))
                {
                    strPeriod = Session["FromDate"].ToString() + '-' + Session["ToDate"].ToString();
                }
                else
                {
                    strPeriod = "All Complaints";
                }

                sheet.Cells[rowCount, colcount].Value = "F/MR/06/03 CUSTOMER COMPLAINT REGISTER";
                rowCount++;
                sheet.Cells[rowCount, colcount].Value = "Period " + strPeriod;
                rowCount++;
                sheet.Cells[rowCount, colcount].Value = "Downloaded By " + Session["LoginName"].ToString() + " " + DateTime.Now.ToString();

                sheet.DefaultColWidth = 50;
                sheet.Protection.AllowAutoFilter = true;
                rowCount = rowCount + 2;

                for (int column = 0; column < grid.Columns.Count; column++)
                {
                    sheet.Cells[rowCount, colcount].Value = grid.Columns[column].ColumnName.ToString();
                    sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                    colcount++;

                }
                rowCount++;

                for (int rowcnt = 0; rowcnt < grid.Rows.Count; rowcnt++)
                {
                    colcount = 1;

                    for (int colcnt = 0; colcnt < grid.Columns.Count; colcnt++)
                    {
                        sheet.Cells[rowCount, colcount].Value = grid.Rows[rowcnt][colcnt].ToString();
                        colcount++;
                    }
                    rowCount++;
                }

                //added by nikita on 14092023
                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "ComplaintRegister-" + formattedDateTime + ".xlsx";

                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }

        private DataTable CreateExportableGrid(ComplaitRegister c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            DataTable grid = GetData(c);

            //grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            ///// grid.Columns.Add(model => model.PK_Complaint_ID).Titled("Complaint ID");
            //grid.Columns.Add(model => model.Complaint_No).Titled("Complaint Number");
            //grid.Columns.Add(model => model.Complaint_Date).Titled("Complaint Date");
            //grid.Columns.Add(model => model.Control_No).Titled("Control No");
            //grid.Columns.Add(model => model.Complaint_Mode).Titled("Complaint Mode");
            //grid.Columns.Add(model => model.TUV_Client).Titled("Client Name And End User Name");
            //grid.Columns.Add(model => model.EndUser).Titled("Vendor / Sub - Vendor Name");
            //grid.Columns.Add(model => model.ProjectName).Titled("Project Name");
            //grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            //grid.Columns.Add(model => model.Executing_Branch).Titled("Executing Branch");
            //grid.Columns.Add(model => model.Inspector_Name).Titled("Inspector Name");
            //grid.Columns.Add(model => model.Category).Titled("Category");
            //grid.Columns.Add(model => model.Complaint_Details).Titled("Complaint Details");
            //grid.Columns.Add(model => model.Root_Cause).Titled("Root Cause Analysis");
            //grid.Columns.Add(model => model.Correction).Titled("Correction");
            //grid.Columns.Add(model => model.CA_To_Prevent_Recurrance).Titled("Corrective Action");

            ///// grid.Columns.Add(model => model.Attachment).Titled("Attachment");           
            ////grid.Columns.Add(model => model.CA_To_Prevent_Recurrance).Titled("CA To Prevent Recurrence");

            //grid.Columns.Add(model => model.LastDateOfInspection).Titled("Last Date of Inspection");
            //grid.Columns.Add(model => model.Date_Of_Aknowledgement).Titled("Date Of Acknowledgment");
            //grid.Columns.Add(model => model.Date_Of_PreliminaryReply).Titled("Date Of Preliminary Reply");
            //grid.Columns.Add(model => model.Date_Of_FinalReply).Titled("Date Of Final Reply");
            //grid.Columns.Add(model => model.Date_Of_Action).Titled("Date Of Corrective Action");
            //grid.Columns.Add(model => model.AttributeToFaultiInspection).Titled("Attribute To Faulti Inspection");
            //grid.Columns.Add(model => model.Effectiveness_Of_Implementation_Of_CA).Titled("Effectiveness Of Implementation Of CA");
            //grid.Columns.Add(model => model.Remarks).Titled("Remarks");
            //grid.Columns.Add(model => model.States_Of_Complaints).Titled("Status Of Complaints");
            //grid.Columns.Add(model => model.ModifiedBy).Titled("Modified By");
            //grid.Columns.Add(model => model.ModifiedDate).Titled("Modified Date");


            //grid.Pager = new GridPager<ComplaitRegister>(grid);
            //grid.Processors.Add(grid.Pager);
            //grid.Pager.RowsPerPage = CMREegister.lstComplaintDashBoard1.Count;

            //foreach (IGridColumn column in grid.Columns)
            //{
            //    column.Filter.IsEnabled = true;
            //    column.Sort.IsEnabled = true;
            //}

            return grid;
        }

        public DataTable GetData(ComplaitRegister c)
        {

            DataTable DTComplaintDashBoard = new DataTable();
            List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();

            if (Session["GetExcelData"] == "Yes")
            {
                DTComplaintDashBoard = objDAM.GetExComplaintDashBoard();
            }
            else
            {

                c.FromDate = Session["FromDate"].ToString();
                c.ToDate = Session["ToDate"].ToString();
                DTComplaintDashBoard = objDAM.GetExComplaintDashBoardByDate(c);
            }


            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["ComplaintList"] = lstComplaintDashBoard;
            CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;

            return DTComplaintDashBoard;
        }




        #endregion



        [HttpGet]
        public JsonResult GetDataByControllNo(string Control_No)
        {
            var address = objDAM.GetDataByControllNo(Control_No);
            return Json(address, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetVendorBySubJobNo(string Control_No)
        {
            var address = objDAM.GetDataByControllNo(Control_No);
            return Json(address, JsonRequestBehavior.AllowGet);
        }



        #region Complaint By call_No

        public ActionResult ComplaintsDashBoardByCallNo(string VendorName, string PK_Call_ID)
        {
            Session["GetExcelData"] = "Yes";
            DataTable DTComplaintDashBoard = new DataTable();
            List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();
            DTComplaintDashBoard = objDAM.GetComplaintByVendor(VendorName);

            #region Get Complaint by Item
            DataTable DTComplaintByItem = new DataTable();
            List<ComplaitRegister> lstComplaintDashBoardByItem = new List<ComplaitRegister>();
            DTComplaintByItem = objDAM.GetComplaintByItem(PK_Call_ID);

            if (DTComplaintByItem.Rows.Count > 0)
            {
                foreach (DataRow dr in DTComplaintByItem.Rows)
                {
                    lstComplaintDashBoardByItem.Add(
                        new ComplaitRegister
                        {
                            PK_Complaint_ID = Convert.ToInt32(dr["PK_Complaint_ID"]),
                            Complaint_No = Convert.ToString(dr["Complaint_No"]),
                            Attachment = Convert.ToString(dr["Attachment"]),
                            Complaint_Date = Convert.ToString(dr["Complaint_Date"]),
                            Control_No = Convert.ToString(dr["Control_No"]),
                            Complaint_Mode = Convert.ToString(dr["Complaint_Mode"]),
                            //End = Convert.ToString(dr["Complaint_Mode"]),
                            TUV_Client = Convert.ToString(dr["TUV_Client"]),
                            Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                            Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                            Inspector_Name = Convert.ToString(dr["Inspector_Name"]),
                            Complaint_Details = Convert.ToString(dr["Complaint_Details"]),
                            Correction = Convert.ToString(dr["Correction"]),
                            Root_Cause = Convert.ToString(dr["Root_Cause"]),
                            CA_To_Prevent_Recurrance = Convert.ToString(dr["CA_To_Prevent_Recurrance"]),
                            Effectiveness_Of_Implementation_Of_CA = Convert.ToString(dr["Effectiveness_Of_Implementation_Of_CA"]),
                            Date_Of_Aknowledgement = Convert.ToString(dr["Date_Of_Aknowledgement"]),
                            Date_Of_PreliminaryReply = Convert.ToString(dr["Date_Of_PreliminaryReply"]),
                            Date_Of_FinalReply = Convert.ToString(dr["Date_Of_FinalReply"]),
                            Date_Of_Action = Convert.ToString(dr["Date_Of_Action"]),
                            Category = Convert.ToString(dr["Category"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            EndUser = Convert.ToString(dr["EndUser"]),
                            States_Of_Complaints = Convert.ToString(dr["States_Of_Complaints"]),
                            ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            Vendor = Convert.ToString(dr["Vendor"]),
                            LessonLearned = Convert.ToString(dr["LessonLearned"]),
                            AttributeToFaultiInspection = Convert.ToString(dr["AttributeToFaultiInspection"]),
                            ProductList = Convert.ToString(dr["ProductCategory"]),
                        }
                        );
                }
                ViewData["Item"] = lstComplaintDashBoardByItem;
                CMREegister.lstByItem = lstComplaintDashBoardByItem;








            }
            else
            {

            }
            #endregion
            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new ComplaitRegister
                            {
                                PK_Complaint_ID = Convert.ToInt32(dr["PK_Complaint_ID"]),
                                Complaint_No = Convert.ToString(dr["Complaint_No"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Complaint_Date = Convert.ToString(dr["Complaint_Date"]),
                                Control_No = Convert.ToString(dr["Control_No"]),
                                Complaint_Mode = Convert.ToString(dr["Complaint_Mode"]),
                                //End = Convert.ToString(dr["Complaint_Mode"]),
                                TUV_Client = Convert.ToString(dr["TUV_Client"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector_Name = Convert.ToString(dr["Inspector_Name"]),
                                Complaint_Details = Convert.ToString(dr["Complaint_Details"]),
                                Correction = Convert.ToString(dr["Correction"]),
                                Root_Cause = Convert.ToString(dr["Root_Cause"]),
                                CA_To_Prevent_Recurrance = Convert.ToString(dr["CA_To_Prevent_Recurrance"]),
                                Effectiveness_Of_Implementation_Of_CA = Convert.ToString(dr["Effectiveness_Of_Implementation_Of_CA"]),
                                Date_Of_Aknowledgement = Convert.ToString(dr["Date_Of_Aknowledgement"]),
                                Date_Of_PreliminaryReply = Convert.ToString(dr["Date_Of_PreliminaryReply"]),
                                Date_Of_FinalReply = Convert.ToString(dr["Date_Of_FinalReply"]),
                                Date_Of_Action = Convert.ToString(dr["Date_Of_Action"]),
                                Category = Convert.ToString(dr["Category"]),
                                Remarks = Convert.ToString(dr["Remarks"]),
                                EndUser = Convert.ToString(dr["EndUser"]),
                                States_Of_Complaints = Convert.ToString(dr["States_Of_Complaints"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                Vendor = Convert.ToString(dr["Vendor"]),
                                LessonLearned = Convert.ToString(dr["LessonLearned"]),
                                AttributeToFaultiInspection = Convert.ToString(dr["AttributeToFaultiInspection"]),
                                ProductList = Convert.ToString(dr["ProductCategory"]),
                            }
                            );
                    }
                    ViewData["ComplaintList"] = lstComplaintDashBoard;
                    CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
                    CMREegister.lstByVendor = lstComplaintDashBoard;






                    return View(CMREegister);
                }
                else
                {
                    ViewData["ComplaintList"] = lstComplaintDashBoard;
                    CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
                    CMREegister.lstByVendor = lstComplaintDashBoard;
                    CMREegister.lstByItem = lstComplaintDashBoardByItem;
                    return View(CMREegister);
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            //ViewData["ComplaintList"] = lstComplaintDashBoard;
            //CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
            return View();
        }

        #endregion

        public JsonResult TemporaryAttFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();

            //Adding New Code 13 March 2020
            List<FileDetails> fileDetails = new List<FileDetails>();

            List<FileDetails> fileAtt1 = new List<FileDetails>();
            List<FileDetails> fileAtt2 = new List<FileDetails>();
            List<FileDetails> fileAtt3 = new List<FileDetails>();
            List<FileDetails> fileAtt4 = new List<FileDetails>();

            if (Session["ListComplaintFile"] != null)
            {
                fileDetails = Session["ListComplaintFile"] as List<FileDetails>;
            }

            if (Session["ListFileAtt1"] != null)
            {
                fileAtt1 = Session["ListFileAtt1"] as List<FileDetails>;
            }

            if (Session["ListFileAtt2"] != null)
            {
                fileAtt2 = Session["ListFileAtt2"] as List<FileDetails>;
            }

            if (Session["ListFileAtt3"] != null)
            {
                fileAtt3 = Session["ListFileAtt3"] as List<FileDetails>;
            }

            if (Session["ListFileAtt4"] != null)
            {
                fileAtt4 = Session["ListFileAtt4"] as List<FileDetails>;
            }

            if (Session["ListFileAtt1"] != null)
            {
                fileAtt1 = Session["ListFileAtt1"] as List<FileDetails>;
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
                        if (files.FileName.ToUpper().EndsWith(".MSG") || files.FileName.ToUpper().EndsWith(".XLSX") || files.FileName.ToUpper().EndsWith(".XLS") || files.FileName.ToUpper().EndsWith(".PDF") || files.FileName.ToUpper().EndsWith(".JPEG") || files.FileName.ToUpper().EndsWith(".JPG") || files.FileName.ToUpper().EndsWith(".PNG") || files.FileName.ToUpper().EndsWith(".GIF") || files.FileName.EndsWith(".DOC") || files.FileName.ToUpper().EndsWith(".DOCX"))
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
                                fileDetails.Add(fileDetail);
                            }
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                            {
                                fileAtt1.Add(fileDetail);
                            }
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD2")
                            {
                                fileAtt2.Add(fileDetail);
                            }
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD3")
                            {
                                fileAtt3.Add(fileDetail);
                            }
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD4")
                            {
                                fileAtt4.Add(fileDetail);
                            }

                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetail.Id + fileDetail.Extension);
                            var K = "~/Content/JobDocument/" + fileName;
                            IPath = K;

                            files.SaveAs(filePath);
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
                    Session["ListComplaintFile"] = fileDetails;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                {
                    Session["ListFileAtt1"] = fileAtt1;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD2")
                {
                    Session["ListFileAtt2"] = fileAtt2;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD3")
                {
                    Session["ListFileAtt3"] = fileAtt3;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD4")
                {
                    Session["ListFileAtt4"] = fileAtt4;
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ShowComplaint(string showData)
        {
            Session["GetExcelData"] = "Yes";
            DataTable DTComplaintDashBoard = new DataTable();
            List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();
            DTComplaintDashBoard = objDAM.ShowSelectedDashBoard(showData);
            string showData1 = string.Empty;

            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    showData1 = DTComplaintDashBoard.Rows[0]["showData1"].ToString();
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new ComplaitRegister
                            {
                                PK_Complaint_ID = Convert.ToInt32(dr["PK_Complaint_ID"]),
                                Complaint_No = Convert.ToString(dr["Complaint_No"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Complaint_Date = Convert.ToString(dr["Complaint_Date"]),
                                Control_No = Convert.ToString(dr["Control_No"]),
                                Complaint_Mode = Convert.ToString(dr["Complaint_Mode"]),
                                //End = Convert.ToString(dr["Complaint_Mode"]),
                                TUV_Client = Convert.ToString(dr["TUV_Client"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector_Name = Convert.ToString(dr["Inspector"]),
                                Complaint_Details = Convert.ToString(dr["Complaint_Details"]),
                                Correction = Convert.ToString(dr["Correction"]),
                                Root_Cause = Convert.ToString(dr["Root_Cause"]),
                                CA_To_Prevent_Recurrance = Convert.ToString(dr["CA_To_Prevent_Recurrance"]),
                                Effectiveness_Of_Implementation_Of_CA = Convert.ToString(dr["Effectiveness_Of_Implementation_Of_CA"]),
                                Date_Of_Aknowledgement = Convert.ToString(dr["Date_Of_Aknowledgement"]),
                                Date_Of_PreliminaryReply = Convert.ToString(dr["Date_Of_PreliminaryReply"]),
                                Date_Of_FinalReply = Convert.ToString(dr["Date_Of_FinalReply"]),
                                Date_Of_Action = Convert.ToString(dr["Date_Of_Action"]),
                                Category = Convert.ToString(dr["Category1"]),
                                Remarks = Convert.ToString(dr["Remarks"]),
                                EndUser = Convert.ToString(dr["EndUser"]),
                                States_Of_Complaints = Convert.ToString(dr["States_Of_Complaints"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                AttributeToFaultiInspection = Convert.ToString(dr["AttributeToFaultiInspection"]),
                                showData1 = Convert.ToString(dr["showData1"]),
                            }
                            );
                    }
                    ViewData["ComplaintList"] = lstComplaintDashBoard;
                    CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
                    CMREegister.showData1 = showData1;
                    return View("ComplaintsDashBoard", CMREegister);
                }
                else
                {
                    ViewData["ComplaintList"] = lstComplaintDashBoard;
                    CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
                    CMREegister.showData1 = showData;
                    return View("ComplaintsDashBoard", CMREegister);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View("ComplaintsDashBoard", CMREegister);
        }


        public void ComplaintMail(int ComplaintID)
        {
            MailMessage msg = new MailMessage();

            string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
            string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
            string MailCC = ConfigurationManager.AppSettings["mailcc"].ToString();

            string bodyTxt = "";
            string displayName = string.Empty;


            try
            {

                DataSet Details = new DataSet();
                Details = objDAM.GetComplaintDetails(ComplaintID);

                bodyTxt = @"<html>
                        <head>
                            <title></title>
                        </head>
                        <body>
                            <div>
                                <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                            <div>
                                &nbsp;</div>
                            <div>
                                <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Complaint No  <b>" + Details.Tables[0].Rows[0]["Complaint_No"].ToString() + "</b> has been registered.Details are as below , </br></br>";

                bodyTxt = bodyTxt + "Customer Name : " + Details.Tables[0].Rows[0]["TUV_Client"].ToString() + "</br>";
                bodyTxt = bodyTxt + "Job Number : " + Details.Tables[0].Rows[0]["Control_No"].ToString() + "</br>";
                bodyTxt = bodyTxt + "Vendor : " + Details.Tables[0].Rows[0]["Vendor"].ToString() + "</br></span></span></div>";

                bodyTxt = bodyTxt + "<div></br>";
                bodyTxt = bodyTxt + "<span style = 'font-size:12px;' ><span style = 'font-family:verdana,geneva,sans-serif;'> Executinng Branch <b> " + Details.Tables[0].Rows[0]["ExeBranch"].ToString() + " </ b > shall investigate and revert to Originating Branch, <b> " + Details.Tables[0].Rows[0]["OrgBranch"].ToString() + " </b> ASAP.</ br ></ br > ";

                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br></br>";

                bodyTxt = bodyTxt + " TUV India Private Limited. " + "</br></br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                bodyTxt = bodyTxt + "</body></html> ";

                ///// displayName = Details.Rows[0]["Branch"].ToString() + "-Inspection";

                msg.From = new MailAddress(MailFrom);

                /*

                if (Details.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < Details.Tables[1].Rows.Count; i++)
                    {
                        msg.To.Add(new MailAddress(Details.Tables[1].Rows[i]["EXEPCHEmail"].ToString()));

                        if(Details.Tables[1].Rows[i]["EXEQHSEEmail"].ToString() != string.Empty)
                        {
                            msg.To.Add(new MailAddress(Details.Tables[1].Rows[i]["EXEQHSEEmail"].ToString()));
                        }
                    }
                }

                if (Details.Tables[2].Rows.Count > 0)
                {
                    for (int j = 0; j < Details.Tables[2].Rows.Count; j++)
                    {
                        msg.To.Add(new MailAddress(Details.Tables[2].Rows[j]["ORGPCHEmail"].ToString()));

                        if (Details.Tables[2].Rows[j]["ORGQHSEEmail"].ToString() != string.Empty)
                        {
                            msg.To.Add(new MailAddress(Details.Tables[2].Rows[j]["EXEQHSEEmail"].ToString()));
                        }
                    }
                }

                if (Details.Tables[0].Rows[0]["CreatedEmail"].ToString() != string.Empty)
                    msg.To.Add(new MailAddress(Details.Tables[0].Rows[0]["CreatedEmail"].ToString()));
                
                 */
                msg.To.Add("pshrikant@tuv-nord.com");

                string CCMails = MailCC.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };

                string[] EmailIDs = CCMails.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.CC.Add(new MailAddress(MultiEmailTemp));
                }



                msg.Subject = "Complaint Registration : " + Details.Tables[0].Rows[0]["Complaint_No"].ToString();
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
                string errmsg = ex.Message;

            }
        }
        // // GET: CustComplaint
        // public ActionResult Index()
        // {
        //     return View();
        // }


        // public ActionResult ComplaintsDashBoard()
        // {
        //     Session["GetExcelData"] = "Yes";
        //     DataTable DTComplaintDashBoard = new DataTable();
        //     List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();
        //     DTComplaintDashBoard = custSpecific.GetComplaintDashBoard();
        //     string showData = string.Empty;
        //     try
        //     {
        //         if (DTComplaintDashBoard.Rows.Count > 0)
        //         {
        //             showData = DTComplaintDashBoard.Rows[0]["showData1"].ToString();

        //             foreach (DataRow dr in DTComplaintDashBoard.Rows)
        //             {
        //                 lstComplaintDashBoard.Add(
        //                     new ComplaitRegister
        //                     {
        //                         PK_Complaint_ID = Convert.ToInt32(dr["PK_Complaint_ID"]),
        //                         Complaint_No = Convert.ToString(dr["Complaint_No"]),
        //                         Attachment = Convert.ToString(dr["Attachment"]),
        //                         Complaint_Date = Convert.ToString(dr["Complaint_Date"]),
        //                         Control_No = Convert.ToString(dr["Control_No"]),
        //                         Complaint_Mode = Convert.ToString(dr["Complaint_Mode"]),
        //                         //End = Convert.ToString(dr["Complaint_Mode"]),
        //                         TUV_Client = Convert.ToString(dr["TUV_Client"]),
        //                         Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
        //                         Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
        //                         Inspector_Name = Convert.ToString(dr["Inspector"]),
        //                         Complaint_Details = Convert.ToString(dr["Complaint_Details"]),
        //                         Correction = Convert.ToString(dr["Correction"]),
        //                         Root_Cause = Convert.ToString(dr["Root_Cause"]),
        //                         CA_To_Prevent_Recurrance = Convert.ToString(dr["CA_To_Prevent_Recurrance"]),
        //                         Effectiveness_Of_Implementation_Of_CA = Convert.ToString(dr["Effectiveness_Of_Implementation_Of_CA"]),
        //                         Date_Of_Aknowledgement = Convert.ToString(dr["Date_Of_Aknowledgement"]),
        //                         Date_Of_PreliminaryReply = Convert.ToString(dr["Date_Of_PreliminaryReply"]),
        //                         Date_Of_FinalReply = Convert.ToString(dr["Date_Of_FinalReply"]),
        //                         Date_Of_Action = Convert.ToString(dr["Date_Of_Action"]),
        //                         Category = Convert.ToString(dr["Category1"]),
        //                         Remarks = Convert.ToString(dr["Remarks"]),
        //                         EndUser = Convert.ToString(dr["EndUser"]),
        //                         States_Of_Complaints = Convert.ToString(dr["States_Of_Complaints"]),
        //                         ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
        //                         ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
        //                         AttributeToFaultiInspection = Convert.ToString(dr["AttributeToFaultiInspection"]),
        //                         showData1 = Convert.ToString(dr["showData1"]),

        //                     }
        //                     );
        //             }
        //             ViewData["ComplaintList"] = lstComplaintDashBoard;
        //             CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
        //             CMREegister.showData1 = showData;
        //             return View(CMREegister);
        //         }
        //         else
        //         {
        //             ViewData["ComplaintList"] = lstComplaintDashBoard;
        //             CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
        //             return View(CMREegister);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         string Error = ex.Message.ToString();
        //     }
        //     //ViewData["ComplaintList"] = lstComplaintDashBoard;
        //     //CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
        //     return View();
        // }

        //[HttpPost]
        // public ActionResult ComplaintsDashBoard(ComplaitRegister c)
        // {
        //     if (c.FromDate == null || c.FromDate == "")
        //     {
        //         return RedirectToAction("ComplaintsDashBoard");
        //     }
        //     Session["GetExcelData"] = null;
        //     Session["FromDate"] = c.FromDate;
        //     Session["ToDate"] = c.ToDate;
        //     string showData = string.Empty;
        //     DataTable DTComplaintDashBoard = new DataTable();
        //     List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();
        //     DTComplaintDashBoard = custSpecific.GetComplaintDashBoardByDate(c);
        //     try
        //     {
        //         if (DTComplaintDashBoard.Rows.Count > 0)
        //         {
        //             showData = DTComplaintDashBoard.Rows[0]["showData1"].ToString();
        //             foreach (DataRow dr in DTComplaintDashBoard.Rows)
        //             {
        //                 lstComplaintDashBoard.Add(
        //                     new ComplaitRegister
        //                     {
        //                         PK_Complaint_ID = Convert.ToInt32(dr["PK_Complaint_ID"]),
        //                         Complaint_No = Convert.ToString(dr["Complaint_No"]),
        //                         Attachment = Convert.ToString(dr["Attachment"]),
        //                         Complaint_Date = Convert.ToString(dr["Complaint_Date"]),
        //                         Control_No = Convert.ToString(dr["Control_No"]),
        //                         Complaint_Mode = Convert.ToString(dr["Complaint_Mode"]),
        //                         TUV_Client = Convert.ToString(dr["TUV_Client"]),
        //                         Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
        //                         Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
        //                         Inspector_Name = Convert.ToString(dr["Inspector"]),
        //                         Complaint_Details = Convert.ToString(dr["Complaint_Details"]),
        //                         Correction = Convert.ToString(dr["Correction"]),
        //                         Root_Cause = Convert.ToString(dr["Root_Cause"]),
        //                         CA_To_Prevent_Recurrance = Convert.ToString(dr["CA_To_Prevent_Recurrance"]),
        //                         Effectiveness_Of_Implementation_Of_CA = Convert.ToString(dr["Effectiveness_Of_Implementation_Of_CA"]),
        //                         Date_Of_Aknowledgement = Convert.ToString(dr["Date_Of_Aknowledgement"]),
        //                         Date_Of_PreliminaryReply = Convert.ToString(dr["Date_Of_PreliminaryReply"]),
        //                         Date_Of_FinalReply = Convert.ToString(dr["Date_Of_FinalReply"]),
        //                         Date_Of_Action = Convert.ToString(dr["Date_Of_Action"]),
        //                         Category = Convert.ToString(dr["Category1"]),
        //                         Remarks = Convert.ToString(dr["Remarks"]),
        //                         EndUser = Convert.ToString(dr["EndUser"]),
        //                         States_Of_Complaints = Convert.ToString(dr["States_Of_Complaints"]),
        //                         ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
        //                         ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
        //                         AttributeToFaultiInspection = Convert.ToString(dr["AttributeToFaultiInspection"]),
        //                         showData1 = Convert.ToString(dr["showData1"])
        //                     }
        //                     );

        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         string Error = ex.Message.ToString();
        //     }
        //     ViewData["ComplaintList"] = lstComplaintDashBoard;
        //     CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
        //     CMREegister.showData1 = showData;
        //     return View(CMREegister);
        // }


        // #region Export to excel

        // [HttpGet]
        // public ActionResult ExportIndex(ComplaitRegister c)
        // {
        //     // Using EPPlus from nuget
        //     using (ExcelPackage package = new ExcelPackage())
        //     {
        //         Int32 row = 2;
        //         Int32 col = 1;

        //         package.Workbook.Worksheets.Add("Data");
        //         DataTable grid = CreateExportableGrid(c);

        //         ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

        //         int colcount = 1;
        //         int rowCount = 1;
        //         string strPeriod = string.Empty;
        //         if ((Session["FromDate"] != null && Session["FromDate"] != string.Empty) && (Session["ToDate"] != null && Session["ToDate"] != string.Empty))
        //         {
        //             strPeriod = Session["FromDate"].ToString() + '-' + Session["ToDate"].ToString();
        //         }
        //         else
        //         {
        //             strPeriod = "All Complaints";
        //         }

        //         sheet.Cells[rowCount, colcount].Value = "F/MR/06/03 CUSTOMER COMPLAINT REGISTER";
        //         rowCount++;
        //         sheet.Cells[rowCount, colcount].Value = "Period " + strPeriod;
        //         rowCount++;
        //         sheet.Cells[rowCount, colcount].Value = "Downloaded By " + Session["LoginName"].ToString() + " " + DateTime.Now.ToString();

        //         sheet.DefaultColWidth = 50;
        //         sheet.Protection.AllowAutoFilter = true;
        //         rowCount = rowCount + 2;

        //         for (int column = 0; column < grid.Columns.Count; column++)
        //         {
        //             sheet.Cells[rowCount, colcount].Value = grid.Columns[column].ColumnName.ToString();
        //             sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //             sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
        //             colcount++;

        //         }
        //         rowCount++;

        //         for (int rowcnt = 0; rowcnt < grid.Rows.Count; rowcnt++)
        //         {
        //             colcount = 1;

        //             for (int colcnt = 0; colcnt < grid.Columns.Count; colcnt++)
        //             {
        //                 sheet.Cells[rowCount, colcount].Value = grid.Rows[rowcnt][colcnt].ToString();
        //                 colcount++;
        //             }
        //             rowCount++;
        //         }
        //         Session["FromDate"] = null;
        //         Session["ToDate"] = null;
        //         //added by nikita on 14092023
        //         DateTime currentDateTime = DateTime.Now;

        //         string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

        //         string filename = "ComplaintRegister-" + formattedDateTime + ".xlsx";

        //         return File(package.GetAsByteArray(), "application/unknown", filename);


        //     }
        // }

        // private DataTable CreateExportableGrid(ComplaitRegister c)
        // {
        //     //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
        //     DataTable grid = GetData(c);

        //     //grid.ViewContext = new ViewContext { HttpContext = HttpContext };


        //     ///// grid.Columns.Add(model => model.PK_Complaint_ID).Titled("Complaint ID");
        //     //grid.Columns.Add(model => model.Complaint_No).Titled("Complaint Number");
        //     //grid.Columns.Add(model => model.Complaint_Date).Titled("Complaint Date");
        //     //grid.Columns.Add(model => model.Control_No).Titled("Control No");
        //     //grid.Columns.Add(model => model.Complaint_Mode).Titled("Complaint Mode");
        //     //grid.Columns.Add(model => model.TUV_Client).Titled("Client Name And End User Name");
        //     //grid.Columns.Add(model => model.EndUser).Titled("Vendor / Sub - Vendor Name");
        //     //grid.Columns.Add(model => model.ProjectName).Titled("Project Name");
        //     //grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
        //     //grid.Columns.Add(model => model.Executing_Branch).Titled("Executing Branch");
        //     //grid.Columns.Add(model => model.Inspector_Name).Titled("Inspector Name");
        //     //grid.Columns.Add(model => model.Category).Titled("Category");
        //     //grid.Columns.Add(model => model.Complaint_Details).Titled("Complaint Details");
        //     //grid.Columns.Add(model => model.Root_Cause).Titled("Root Cause Analysis");
        //     //grid.Columns.Add(model => model.Correction).Titled("Correction");
        //     //grid.Columns.Add(model => model.CA_To_Prevent_Recurrance).Titled("Corrective Action");

        //     ///// grid.Columns.Add(model => model.Attachment).Titled("Attachment");           
        //     ////grid.Columns.Add(model => model.CA_To_Prevent_Recurrance).Titled("CA To Prevent Recurrence");

        //     //grid.Columns.Add(model => model.LastDateOfInspection).Titled("Last Date of Inspection");
        //     //grid.Columns.Add(model => model.Date_Of_Aknowledgement).Titled("Date Of Acknowledgment");
        //     //grid.Columns.Add(model => model.Date_Of_PreliminaryReply).Titled("Date Of Preliminary Reply");
        //     //grid.Columns.Add(model => model.Date_Of_FinalReply).Titled("Date Of Final Reply");
        //     //grid.Columns.Add(model => model.Date_Of_Action).Titled("Date Of Corrective Action");
        //     //grid.Columns.Add(model => model.AttributeToFaultiInspection).Titled("Attribute To Faulti Inspection");
        //     //grid.Columns.Add(model => model.Effectiveness_Of_Implementation_Of_CA).Titled("Effectiveness Of Implementation Of CA");
        //     //grid.Columns.Add(model => model.Remarks).Titled("Remarks");
        //     //grid.Columns.Add(model => model.States_Of_Complaints).Titled("Status Of Complaints");
        //     //grid.Columns.Add(model => model.ModifiedBy).Titled("Modified By");
        //     //grid.Columns.Add(model => model.ModifiedDate).Titled("Modified Date");


        //     //grid.Pager = new GridPager<ComplaitRegister>(grid);
        //     //grid.Processors.Add(grid.Pager);
        //     //grid.Pager.RowsPerPage = CMREegister.lstComplaintDashBoard1.Count;

        //     //foreach (IGridColumn column in grid.Columns)
        //     //{
        //     //    column.Filter.IsEnabled = true;
        //     //    column.Sort.IsEnabled = true;
        //     //}

        //     return grid;
        // }

        // public DataTable GetData(ComplaitRegister c)
        // {

        //     DataTable DTComplaintDashBoard = new DataTable();
        //     List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();

        //     if (Session["GetExcelData"] == "Yes")
        //     {
        //         DTComplaintDashBoard = custSpecific.GetComplaintDashBoard();
        //     }
        //     else
        //     {

        //         c.FromDate = Session["FromDate"].ToString();
        //         c.ToDate = Session["ToDate"].ToString();
        //         DTComplaintDashBoard = custSpecific.GetComplaintDashBoardByDate(c);
        //     }


        //     try
        //     {
        //         if (DTComplaintDashBoard.Rows.Count > 0)
        //         {

        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         string Error = ex.Message.ToString();
        //     }
        //     ViewData["ComplaintList"] = lstComplaintDashBoard;
        //     CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;

        //     return DTComplaintDashBoard;
        // }

        // #endregion


        // [HttpGet]
        // public ActionResult ShowComplaint(string showData)
        // {
        //     Session["GetExcelData"] = "Yes";
        //     DataTable DTComplaintDashBoard = new DataTable();
        //     List<ComplaitRegister> lstComplaintDashBoard = new List<ComplaitRegister>();
        //     DTComplaintDashBoard = custSpecific.ShowSelectedDashBoard(showData);
        //     string showData1 = string.Empty;

        //     try
        //     {
        //         if (DTComplaintDashBoard.Rows.Count > 0)
        //         {
        //             showData1 = DTComplaintDashBoard.Rows[0]["showData1"].ToString();
        //             foreach (DataRow dr in DTComplaintDashBoard.Rows)
        //             {
        //                 lstComplaintDashBoard.Add(
        //                     new ComplaitRegister
        //                     {
        //                         PK_Complaint_ID = Convert.ToInt32(dr["PK_Complaint_ID"]),
        //                         Complaint_No = Convert.ToString(dr["Complaint_No"]),
        //                         Attachment = Convert.ToString(dr["Attachment"]),
        //                         Complaint_Date = Convert.ToString(dr["Complaint_Date"]),
        //                         Control_No = Convert.ToString(dr["Control_No"]),
        //                         Complaint_Mode = Convert.ToString(dr["Complaint_Mode"]),
        //                         //End = Convert.ToString(dr["Complaint_Mode"]),
        //                         TUV_Client = Convert.ToString(dr["TUV_Client"]),
        //                         Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
        //                         Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
        //                         Inspector_Name = Convert.ToString(dr["Inspector"]),
        //                         Complaint_Details = Convert.ToString(dr["Complaint_Details"]),
        //                         Correction = Convert.ToString(dr["Correction"]),
        //                         Root_Cause = Convert.ToString(dr["Root_Cause"]),
        //                         CA_To_Prevent_Recurrance = Convert.ToString(dr["CA_To_Prevent_Recurrance"]),
        //                         Effectiveness_Of_Implementation_Of_CA = Convert.ToString(dr["Effectiveness_Of_Implementation_Of_CA"]),
        //                         Date_Of_Aknowledgement = Convert.ToString(dr["Date_Of_Aknowledgement"]),
        //                         Date_Of_PreliminaryReply = Convert.ToString(dr["Date_Of_PreliminaryReply"]),
        //                         Date_Of_FinalReply = Convert.ToString(dr["Date_Of_FinalReply"]),
        //                         Date_Of_Action = Convert.ToString(dr["Date_Of_Action"]),
        //                         Category = Convert.ToString(dr["Category1"]),
        //                         Remarks = Convert.ToString(dr["Remarks"]),
        //                         EndUser = Convert.ToString(dr["EndUser"]),
        //                         States_Of_Complaints = Convert.ToString(dr["States_Of_Complaints"]),
        //                         ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
        //                         ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
        //                         AttributeToFaultiInspection = Convert.ToString(dr["AttributeToFaultiInspection"]),
        //                         showData1 = Convert.ToString(dr["showData1"]),
        //                     }
        //                     );
        //             }
        //             ViewData["ComplaintList"] = lstComplaintDashBoard;
        //             CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
        //             CMREegister.showData1 = showData1;
        //             return View("ComplaintsDashBoard", CMREegister);
        //         }
        //         else
        //         {
        //             ViewData["ComplaintList"] = lstComplaintDashBoard;
        //             CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
        //             CMREegister.showData1 = showData;
        //             return View("ComplaintsDashBoard", CMREegister);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         string Error = ex.Message.ToString();
        //     }

        //     return View("ComplaintsDashBoard", CMREegister);
        // }

        // [HttpGet]
        // public ActionResult CreateComplaint(int? PK_Complaint_ID)
        // {

        //     string[] splitedAuditorName;
        //     #region Bind Auditor Name
        //     DataSet dsAuditorName = new DataSet();
        //     //List<AuditorName> lstAuditorNamee = new List<AuditorName>();
        //     List<Audit> lstAuditorNamee = new List<Audit>();
        //     dsAuditorName = custSpecific.BindAuditorName();

        //     if (dsAuditorName.Tables[0].Rows.Count > 0)
        //     {
        //         lstAuditorNamee = (from n in dsAuditorName.Tables[0].AsEnumerable()
        //                            select new Audit()
        //                            {
        //                                DAuditorName = n.Field<string>(dsAuditorName.Tables[0].Columns["Name"].ToString()),
        //                                DAuditorCode = n.Field<string>(dsAuditorName.Tables[0].Columns["Code"].ToString())

        //                            }).ToList();
        //     }

        //     IEnumerable<SelectListItem> AuditorName;
        //     AuditorName = new SelectList(lstAuditorNamee, "DAuditorCode", "DAuditorName");
        //     ViewBag.AuditorName = AuditorName;
        //     ViewData["AuditorName"] = AuditorName;
        //     ViewData["AuditorName"] = lstAuditorNamee;
        //     #endregion

        //     #region Bind Product category
        //     ViewData["Drpproduct"] = custSpecific.GetDrpList();
        //     #endregion

        //     #region Bind Complaint category
        //     ViewData["Category1"] = custSpecific.GetComplaintCategory();
        //     #endregion

        //     if (PK_Complaint_ID != 0 && PK_Complaint_ID != null)
        //     {
        //         string[] splitedInspector_Name;
        //         //ViewBag.check = "inspector";
        //         ViewBag.check = "AuditorName";
        //         DataTable DTComplaint = new DataTable();
        //         ViewBag.check = "productcheck";
        //         ViewBag.check1 = "productcheck1";
        //         string[] splitedProduct_Name;
        //         string[] splitedCategory;

        //         DTComplaint = custSpecific.EditComplaints(PK_Complaint_ID);

        //         //string insbranch = CMREegister.allInspector_Name;
        //         //List<string> lstbr = new List<string>();
        //         //lstbr = insbranch.Split(',').ToList();
        //         //List<string> lstt = new List<string>();
        //         //ViewData["branchofinspector"] = lstbr;
        //         string varins = CMREegister.Executing_Branch;





        //         if (DTComplaint.Rows.Count > 0)
        //         {
        //             foreach (DataRow dr in DTComplaint.Rows)
        //             {
        //                 CMREegister.Complaint_No = Convert.ToString(DTComplaint.Rows[0]["Complaint_No"]);
        //                 CMREegister.Complaint_Date = Convert.ToString(DTComplaint.Rows[0]["Complaint_Date"]);
        //                 CMREegister.Attachment = Convert.ToString(DTComplaint.Rows[0]["Attachment"]);
        //                 CMREegister.Control_No = Convert.ToString(DTComplaint.Rows[0]["Control_No"]);
        //                 CMREegister.Complaint_Mode = Convert.ToString(DTComplaint.Rows[0]["Complaint_Mode"]);
        //                 CMREegister.TUV_Client = Convert.ToString(DTComplaint.Rows[0]["TUV_Client"]);
        //                 CMREegister.Originating_Branch = Convert.ToString(DTComplaint.Rows[0]["Originating_Branch"]);
        //                 CMREegister.Executing_Branch = Convert.ToString(DTComplaint.Rows[0]["Executing_Branch"]);

        //                 CMREegister.Inspector_Name = Convert.ToString(DTComplaint.Rows[0]["Inspector_Name"]);

        //                 CMREegister.Complaint_Details = Convert.ToString(DTComplaint.Rows[0]["Complaint_Details"]);
        //                 CMREegister.Correction = Convert.ToString(DTComplaint.Rows[0]["Correction"]);
        //                 CMREegister.Root_Cause = Convert.ToString(DTComplaint.Rows[0]["Root_Cause"]);
        //                 CMREegister.CA_To_Prevent_Recurrance = Convert.ToString(DTComplaint.Rows[0]["CA_To_Prevent_Recurrance"]);
        //                 CMREegister.Effectiveness_Of_Implementation_Of_CA = Convert.ToString(DTComplaint.Rows[0]["Effectiveness_Of_Implementation_Of_CA"]);
        //                 CMREegister.Date_Of_Aknowledgement = Convert.ToString(DTComplaint.Rows[0]["Date_Of_Aknowledgement"]);
        //                 CMREegister.Date_Of_PreliminaryReply = Convert.ToString(DTComplaint.Rows[0]["Date_Of_PreliminaryReply"]);
        //                 CMREegister.Date_Of_FinalReply = Convert.ToString(DTComplaint.Rows[0]["Date_Of_FinalReply"]);
        //                 CMREegister.Date_Of_Action = Convert.ToString(DTComplaint.Rows[0]["Date_Of_Action"]);
        //                 CMREegister.Category = Convert.ToString(DTComplaint.Rows[0]["Category"]);
        //                 CMREegister.Remarks = Convert.ToString(DTComplaint.Rows[0]["Remarks"]);
        //                 CMREegister.EndUser = Convert.ToString(DTComplaint.Rows[0]["EndUser"]);
        //                 CMREegister.States_Of_Complaints = Convert.ToString(DTComplaint.Rows[0]["States_Of_Complaints"]);
        //                 CMREegister.LastDateOfInspection = Convert.ToString(DTComplaint.Rows[0]["LastDateOfInspection"]);
        //                 CMREegister.AttributeToFaultiInspection = Convert.ToString(DTComplaint.Rows[0]["AttributeToFaultiInspection"]);
        //                 CMREegister.Vendor = Convert.ToString(DTComplaint.Rows[0]["Vendor"]);
        //                 CMREegister.SubVendor = Convert.ToString(DTComplaint.Rows[0]["SubVendor"]);
        //                 CMREegister.LessonLearned = Convert.ToString(DTComplaint.Rows[0]["LessonLearned"]);
        //                 CMREegister.ActionTaken = Convert.ToString(DTComplaint.Rows[0]["ActionTaken"]);
        //                 CMREegister.ProjectName = Convert.ToString(DTComplaint.Rows[0]["ProjectName"]);
        //                 CMREegister.ShareLessonsLearnt = Convert.ToBoolean(DTComplaint.Rows[0]["ShareLessonsLearnt"]);

        //                 CMREegister.Call_no = Convert.ToString(DTComplaint.Rows[0]["CallNo"]);
        //                 CMREegister.ReferenceDocument = Convert.ToString(DTComplaint.Rows[0]["ReferenceDocument"]);
        //                 // CMREegister.Hide = Convert.ToBoolean(DTComplaint.Rows[0]["Hide2"]);

        //                 //Added by Ankush for Delete file and update file
        //                 List<string> Selected = new List<string>();
        //                 var Existingins = Convert.ToString(DTComplaint.Rows[0]["Inspector_Name"]);
        //                 splitedInspector_Name = Existingins.Split(',');
        //                 foreach (var single in splitedInspector_Name)
        //                 {
        //                     Selected.Add(single);
        //                 }
        //                 ViewBag.EditinspectorName = Selected;

        //                 List<string> Selected1 = new List<string>();
        //                 var Existingins1 = Convert.ToString(DTComplaint.Rows[0]["Product_item"]);
        //                 splitedProduct_Name = Existingins1.Split(',');
        //                 foreach (var single1 in splitedProduct_Name)
        //                 {
        //                     Selected1.Add(single1);
        //                 }
        //                 ViewBag.EditproductName = Selected1;

        //                 #region BindCategory
        //                 List<string> Selected5 = new List<string>();
        //                 var Existingins5 = Convert.ToString(DTComplaint.Rows[0]["Category"]);
        //                 splitedCategory = Existingins5.Split(',');
        //                 foreach (var single6 in splitedCategory)
        //                 {
        //                     Selected5.Add(single6);
        //                 }
        //                 ViewBag.EditCompCategory = Selected5;
        //                 #endregion


        //                 #region Get All Category
        //                 DataTable DTGetAllCategory = new DataTable();
        //                 List<NameCodeins> lstEditCategory = new List<NameCodeins>();
        //                 DTGetAllCategory = custSpecific.GetComplaint();

        //                 if (DTGetAllCategory.Rows.Count > 0)
        //                 {
        //                     lstEditCategory = (from n in DTGetAllCategory.AsEnumerable()
        //                                        select new NameCodeins()
        //                                        {
        //                                            Name = n.Field<string>(DTGetAllCategory.Columns["Name"].ToString()),
        //                                            PkUserId = n.Field<string>(DTGetAllCategory.Columns["Id"].ToString())

        //                                        }).ToList();
        //                 }

        //                 IEnumerable<SelectListItem> CategoryItems;
        //                 CategoryItems = new SelectList(lstEditCategory, "PkUserId", "Name");
        //                 ViewBag.Category = CategoryItems;
        //                 ViewData["Category"] = CategoryItems;
        //                 #endregion


        //                 #region Bind Inspector
        //                 DataTable DTGetInspectorLst = new DataTable();
        //                 List<NameCodeins> lstEditInspector = new List<NameCodeins>();
        //                 DTGetInspectorLst = custSpecific.GetInspector(CMREegister.Executing_Branch);

        //                 if (DTGetInspectorLst.Rows.Count > 0)
        //                 {
        //                     lstEditInspector = (from n in DTGetInspectorLst.AsEnumerable()
        //                                         select new NameCodeins()
        //                                         {
        //                                             Name = n.Field<string>(DTGetInspectorLst.Columns["InspectorName"].ToString()),
        //                                             PkUserId = n.Field<string>(DTGetInspectorLst.Columns["PK_UserID"].ToString())

        //                                         }).ToList();
        //                 }

        //                 IEnumerable<SelectListItem> inspectorItems;
        //                 inspectorItems = new SelectList(lstEditInspector, "PkUserId", "Name");
        //                 ViewBag.ProjectTypeItems = inspectorItems;
        //                 ViewData["ProjectTypeItems"] = inspectorItems;
        //                 #endregion


        //                 #region Get inspector Name
        //                 List<string> SelectedAuditorName = new List<string>();
        //                 var EAuditorName = Convert.ToString(DTComplaint.Rows[0]["Inspector_Name"]);
        //                 splitedAuditorName = EAuditorName.Split(',');
        //                 foreach (var single1 in splitedAuditorName)
        //                 {
        //                     SelectedAuditorName.Add(single1);
        //                 }
        //                 ViewBag.AuditorNameName = SelectedAuditorName;
        //                 #endregion
        //             }

        //             DataTable DTGetProductLst = new DataTable();
        //             List<NameCodeProduct> lstEditInspector1 = new List<NameCodeProduct>();
        //             DTGetProductLst = custSpecific.getlistforEdit();

        //             if (DTGetProductLst.Rows.Count > 0)
        //             {
        //                 lstEditInspector1 = (from n in DTGetProductLst.AsEnumerable()
        //                                      select new NameCodeProduct()
        //                                      {
        //                                          Name = n.Field<string>(DTGetProductLst.Columns["Name"].ToString())


        //                                      }).ToList();
        //             }

        //             IEnumerable<SelectListItem> ProductcheckItems;
        //             ProductcheckItems = new SelectList(lstEditInspector1, "Name", "Name");
        //             ViewBag.ProjectTypeItems = ProductcheckItems;


        //             //Added by Ankush for Delete file and update file
        //             DataTable DTGetUploadedFile = new DataTable();
        //             List<FileDetails> lstEditFileDetails = new List<FileDetails>();
        //             DTGetUploadedFile = custSpecific.EditUploadedFile(PK_Complaint_ID);
        //             if (DTGetUploadedFile.Rows.Count > 0)
        //             {
        //                 foreach (DataRow dr in DTGetUploadedFile.Rows)
        //                 {
        //                     lstEditFileDetails.Add(
        //                        new FileDetails
        //                        {

        //                            PK_ID = Convert.ToInt32(dr["PK_ID"]),
        //                            FileName = Convert.ToString(dr["FileName"]),
        //                            Extension = Convert.ToString(dr["Extenstion"]),
        //                            IDS = Convert.ToString(dr["FileID"]),
        //                        }
        //                      );
        //                 }
        //                 ViewData["lstEditFileDetails"] = lstEditFileDetails;
        //                 CMREegister.FileDetails = lstEditFileDetails;
        //             }

        //             /********************** 4 Attachments ******************************************/

        //             DataTable DTAttach1 = new DataTable();
        //             List<FileDetails> lstAttach1 = new List<FileDetails>();
        //             DTAttach1 = custSpecific.EditAttUploadedFile(PK_Complaint_ID, "1");

        //             if (DTAttach1.Rows.Count > 0)
        //             {
        //                 foreach (DataRow dr in DTAttach1.Rows)
        //                 {
        //                     lstAttach1.Add(
        //                        new FileDetails
        //                        {

        //                            PK_ID = Convert.ToInt32(dr["PK_ID"]),
        //                            FileName = Convert.ToString(dr["FileName"]),
        //                            Extension = Convert.ToString(dr["Extenstion"]),
        //                            IDS = Convert.ToString(dr["FileID"]),
        //                        }
        //                      );
        //                 }
        //                 ViewData["lstAttach1"] = lstAttach1;
        //                 CMREegister.ATT1 = lstAttach1;
        //             }

        //             DataTable DTAttach2 = new DataTable();
        //             List<FileDetails> lstAttach2 = new List<FileDetails>();
        //             DTAttach2 = custSpecific.EditAttUploadedFile(PK_Complaint_ID, "2");

        //             if (DTAttach2.Rows.Count > 0)
        //             {
        //                 foreach (DataRow dr in DTAttach2.Rows)
        //                 {
        //                     lstAttach2.Add(
        //                        new FileDetails
        //                        {

        //                            PK_ID = Convert.ToInt32(dr["PK_ID"]),
        //                            FileName = Convert.ToString(dr["FileName"]),
        //                            Extension = Convert.ToString(dr["Extenstion"]),
        //                            IDS = Convert.ToString(dr["FileID"]),
        //                        }
        //                      );
        //                 }
        //                 ViewData["lstAttach2"] = lstAttach2;
        //                 CMREegister.ATT2 = lstAttach2;
        //             }

        //             DataTable DTAttach3 = new DataTable();
        //             List<FileDetails> lstAttach3 = new List<FileDetails>();
        //             DTAttach3 = custSpecific.EditAttUploadedFile(PK_Complaint_ID, "3");

        //             if (DTAttach3.Rows.Count > 0)
        //             {
        //                 foreach (DataRow dr in DTAttach3.Rows)
        //                 {
        //                     lstAttach3.Add(
        //                        new FileDetails
        //                        {

        //                            PK_ID = Convert.ToInt32(dr["PK_ID"]),
        //                            FileName = Convert.ToString(dr["FileName"]),
        //                            Extension = Convert.ToString(dr["Extenstion"]),
        //                            IDS = Convert.ToString(dr["FileID"]),
        //                        }
        //                      );
        //                 }
        //                 ViewData["lstAttach3"] = lstAttach3;
        //                 CMREegister.ATT3 = lstAttach3;
        //             }

        //             DataTable DTAttach4 = new DataTable();
        //             List<FileDetails> lstAttach4 = new List<FileDetails>();
        //             DTAttach4 = custSpecific.EditAttUploadedFile(PK_Complaint_ID, "4");

        //             if (DTAttach4.Rows.Count > 0)
        //             {
        //                 foreach (DataRow dr in DTAttach4.Rows)
        //                 {
        //                     lstAttach4.Add(
        //                        new FileDetails
        //                        {

        //                            PK_ID = Convert.ToInt32(dr["PK_ID"]),
        //                            FileName = Convert.ToString(dr["FileName"]),
        //                            Extension = Convert.ToString(dr["Extenstion"]),
        //                            IDS = Convert.ToString(dr["FileID"]),
        //                        }
        //                      );
        //                 }
        //                 ViewData["lstAttach4"] = lstAttach4;
        //                 CMREegister.ATT4 = lstAttach4;
        //             }


        //             /*********************************************************************************************/


        //             return View(CMREegister);
        //         }
        //         else
        //         {
        //             return View();
        //         }

        //     }

        //     var Data = custSpecific.GetInspectorList();
        //     ViewBag.SubCatlist = new SelectList(Data, "PK_UserID", "FirstName");

        //     return View(CMREegister);
        // }

        // [HttpGet]
        // public ActionResult CreateComplaint(ComplaitRegister UserComplaint, HttpPostedFileBase File, string bname, FormCollection fc, HttpPostedFileBase[] Image)
        // {
        //     #region Added by Ankush
        //     int CMPID = 0;
        //     var IPath = string.Empty;
        //     string[] splitedGrp;

        //     List<string> Selected = new List<string>();
        //     //List<FileDetails> lstFileDtls = new List<FileDetails>();
        //     //fileDetails = Session["listCMPUploadedFile"] as List<FileDetails>;
        //     #endregion
        //     string ProList = string.Join(",", fc["ProductList"]);
        //     UserComplaint.ProductList = ProList;

        //     string ProList1 = string.Join(",", fc["Category"]);
        //     UserComplaint.Category = ProList1;

        //     List<FileDetails> fileDetails = new List<FileDetails>();

        //     List<FileDetails> fileAtt1 = new List<FileDetails>();
        //     List<FileDetails> fileAtt2 = new List<FileDetails>();
        //     List<FileDetails> fileAtt3 = new List<FileDetails>();
        //     List<FileDetails> fileAtt4 = new List<FileDetails>();



        //     #region Added by Ankush
        //     FormCollection fca = new FormCollection();
        //     string filePathCMP = string.Empty;
        //     for (int i = 0; i < Request.Files.Count; i++)
        //     {
        //         HttpPostedFileBase files = Request.Files[i]; //Uploaded file
        //         int fileSize = files.ContentLength;
        //         if (files != null && files.ContentLength > 0)
        //         {
        //             // if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc"))
        //             if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))

        //             {
        //                 string fileName = files.FileName;
        //                 FileDetails fileDetail = new FileDetails();
        //                 fileDetail.FileName = fileName;
        //                 fileDetail.Extension = Path.GetExtension(fileName);
        //                 fileDetail.Id = Guid.NewGuid();
        //                 fileDetails.Add(fileDetail);
        //                 //-----------------------------------------------------
        //                 filePathCMP = Path.Combine(Server.MapPath("~/ComplaintFolder/"), fileDetail.Id + fileDetail.Extension);
        //                 var K = "~/ComplaintFolder/" + fileName;
        //                 IPath = K;
        //                 files.SaveAs(filePathCMP);
        //                 var ExistingUploadFile = IPath;
        //                 splitedGrp = ExistingUploadFile.Split(',');
        //                 foreach (var single in splitedGrp)
        //                 {
        //                     Selected.Add(single);
        //                 }
        //                 Session["list"] = Selected;
        //             }
        //             else
        //             {
        //                 ViewBag.Error = "Please Select XLSX or PDF File";
        //             }
        //         }
        //     }
        //     Session["listCMPUploadedFile"] = fileDetails;
        //     #endregion

        //     string Result = string.Empty;
        //     //string insname = string.Join(",", fc["InspectorName"]); 
        //     string insname = string.Join(",", fc["AuditorName"]);
        //     UserComplaint.Inspector_Name = insname;


        //     try
        //     {
        //         if (UserComplaint.PK_Complaint_ID != 0)
        //         {

        //             Result = custSpecific.InsertUpdateComplaintData(UserComplaint);
        //             CMPID = UserComplaint.PK_Complaint_ID;
        //             if (CMPID != null && CMPID != 0)
        //             {

        //                 fileDetails = Session["ListComplaintFile"] as List<FileDetails>;

        //                 fileAtt1 = Session["ListFileAtt1"] as List<FileDetails>;
        //                 fileAtt2 = Session["ListFileAtt2"] as List<FileDetails>;
        //                 fileAtt3 = Session["ListFileAtt3"] as List<FileDetails>;
        //                 fileAtt4 = Session["ListFileAtt4"] as List<FileDetails>;


        //                 if (fileDetails != null && fileDetails.Count > 0)
        //                 {
        //                     Result = custSpecific.InsertFileAttachment(fileDetails, CMPID);
        //                     Session["ListComplaintFile"] = null;
        //                 }

        //                 if (fileAtt1 != null && fileAtt1.Count > 0)
        //                 {
        //                     Result = custSpecific.InsertAttFileAttachment(fileAtt1, CMPID, "1");
        //                     Session["ListFileAtt1"] = null;
        //                 }

        //                 if (fileAtt2 != null && fileAtt2.Count > 0)
        //                 {
        //                     Result = custSpecific.InsertAttFileAttachment(fileAtt2, CMPID, "2");
        //                     Session["ListFileAtt2"] = null;
        //                 }

        //                 if (fileAtt3 != null && fileAtt3.Count > 0)
        //                 {
        //                     Result = custSpecific.InsertAttFileAttachment(fileAtt3, CMPID, "3");
        //                     Session["ListFileAtt3"] = null;
        //                 }

        //                 if (fileAtt4 != null && fileAtt4.Count > 0)
        //                 {
        //                     Result = custSpecific.InsertAttFileAttachment(fileAtt4, CMPID, "4");
        //                     Session["ListFileAtt4"] = null;
        //                 }




        //             }
        //             if (CMPID != 0 && CMPID != null)
        //             {
        //                 TempData["UpdateComplaints"] = Result;
        //                 //return RedirectToAction("ComplaintsDashBoard");
        //                 return RedirectToAction("CreateComplaint", new { PK_Complaint_ID = CMPID });
        //             }
        //         }
        //         else
        //         {
        //             //if (UserComplaint.Attachment != null)
        //             //{
        //             //    UserComplaint.Attachment = CommonControl.FileUpload("Content/Uploads/Attachments/", UserComplaint.Attachment);
        //             //}
        //             int _min = 100000000;
        //             int _max = 999999999;
        //             Random _rdm = new Random();
        //             int Rjno = _rdm.Next(_min, _max);
        //             string ConfirmCode = Convert.ToString(Rjno);
        //             UserComplaint.Complaint_No = "CN" + Convert.ToString(ConfirmCode);

        //             Result = custSpecific.InsertUpdateComplaintData(UserComplaint);

        //             CMPID = Convert.ToInt32(Session["CMPIDs"]);
        //             if (CMPID != null && CMPID != 0)
        //             {

        //                 fileDetails = Session["ListComplaintFile"] as List<FileDetails>;

        //                 fileAtt1 = Session["ListFileAtt1"] as List<FileDetails>;
        //                 fileAtt2 = Session["ListFileAtt2"] as List<FileDetails>;
        //                 fileAtt3 = Session["ListFileAtt3"] as List<FileDetails>;
        //                 fileAtt4 = Session["ListFileAtt4"] as List<FileDetails>;


        //                 if (fileDetails != null && fileDetails.Count > 0)
        //                 {
        //                     Result = custSpecific.InsertFileAttachment(fileDetails, CMPID);
        //                     Session["ListComplaintFile"] = null;
        //                 }

        //                 if (fileAtt1 != null && fileAtt1.Count > 0)
        //                 {
        //                     Result = custSpecific.InsertAttFileAttachment(fileAtt1, CMPID, "1");
        //                     Session["ListFileAtt1"] = null;
        //                 }

        //                 if (fileAtt2 != null && fileAtt2.Count > 0)
        //                 {
        //                     Result = custSpecific.InsertAttFileAttachment(fileAtt2, CMPID, "2");
        //                     Session["ListFileAtt2"] = null;
        //                 }

        //                 if (fileAtt3 != null && fileAtt3.Count > 0)
        //                 {
        //                     Result = custSpecific.InsertAttFileAttachment(fileAtt3, CMPID, "3");
        //                     Session["ListFileAtt3"] = null;
        //                 }

        //                 if (fileAtt4 != null && fileAtt4.Count > 0)
        //                 {
        //                     Result = custSpecific.InsertAttFileAttachment(fileAtt4, CMPID, "4");
        //                     Session["ListFileAtt4"] = null;
        //                 }

        //                 ComplaintMail(CMPID);
        //             }
        //             TempData["insertComplaints"] = Result;

        //             //return RedirectToAction("ComplaintsDashBoard");
        //             return RedirectToAction("CreateComplaint", new { PK_Complaint_ID = CMPID });
        //         }
        //     }
        //     catch (Exception ex)
        //     {

        //         string Error = ex.Message.ToString();
        //     }
        //     return View();
        // }
        // public void ComplaintMail(int ComplaintID)
        // {
        //     MailMessage msg = new MailMessage();

        //     string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
        //     string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
        //     string MailCC = ConfigurationManager.AppSettings["mailcc"].ToString();

        //     string bodyTxt = "";
        //     string displayName = string.Empty;


        //     try
        //     {

        //         DataSet Details = new DataSet();
        //         Details = custSpecific.GetComplaintDetails(ComplaintID);

        //         bodyTxt = @"<html>
        //                 <head>
        //                     <title></title>
        //                 </head>
        //                 <body>
        //                     <div>
        //                         <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
        //                     <div>
        //                         &nbsp;</div>
        //                     <div>
        //                         <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Complaint No  <b>" + Details.Tables[0].Rows[0]["Complaint_No"].ToString() + "</b> has been registered.Details are as below , </br></br>";

        //         bodyTxt = bodyTxt + "Customer Name : " + Details.Tables[0].Rows[0]["TUV_Client"].ToString() + "</br>";
        //         bodyTxt = bodyTxt + "Job Number : " + Details.Tables[0].Rows[0]["Control_No"].ToString() + "</br>";
        //         bodyTxt = bodyTxt + "Vendor : " + Details.Tables[0].Rows[0]["Vendor"].ToString() + "</br></span></span></div>";

        //         bodyTxt = bodyTxt + "<div></br>";
        //         bodyTxt = bodyTxt + "<span style = 'font-size:12px;' ><span style = 'font-family:verdana,geneva,sans-serif;'> Executinng Branch <b> " + Details.Tables[0].Rows[0]["ExeBranch"].ToString() + " </ b > shall investigate and revert to Originating Branch, <b> " + Details.Tables[0].Rows[0]["OrgBranch"].ToString() + " </b> ASAP.</ br ></ br > ";

        //         bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br></br>";

        //         bodyTxt = bodyTxt + " TUV India Private Limited. " + "</br></br>";
        //         bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
        //         bodyTxt = bodyTxt + "</body></html> ";

        //         ///// displayName = Details.Rows[0]["Branch"].ToString() + "-Inspection";

        //         msg.From = new MailAddress(MailFrom);

        //         /*

        //         if (Details.Tables[1].Rows.Count > 0)
        //         {
        //             for (int i = 0; i < Details.Tables[1].Rows.Count; i++)
        //             {
        //                 msg.To.Add(new MailAddress(Details.Tables[1].Rows[i]["EXEPCHEmail"].ToString()));

        //                 if(Details.Tables[1].Rows[i]["EXEQHSEEmail"].ToString() != string.Empty)
        //                 {
        //                     msg.To.Add(new MailAddress(Details.Tables[1].Rows[i]["EXEQHSEEmail"].ToString()));
        //                 }
        //             }
        //         }

        //         if (Details.Tables[2].Rows.Count > 0)
        //         {
        //             for (int j = 0; j < Details.Tables[2].Rows.Count; j++)
        //             {
        //                 msg.To.Add(new MailAddress(Details.Tables[2].Rows[j]["ORGPCHEmail"].ToString()));

        //                 if (Details.Tables[2].Rows[j]["ORGQHSEEmail"].ToString() != string.Empty)
        //                 {
        //                     msg.To.Add(new MailAddress(Details.Tables[2].Rows[j]["EXEQHSEEmail"].ToString()));
        //                 }
        //             }
        //         }

        //         if (Details.Tables[0].Rows[0]["CreatedEmail"].ToString() != string.Empty)
        //             msg.To.Add(new MailAddress(Details.Tables[0].Rows[0]["CreatedEmail"].ToString()));

        //          */
        //         msg.To.Add("pshrikant@tuv-nord.com");

        //         string CCMails = MailCC.ToString();
        //         char[] delimiters = new[] { ',', ';', ' ' };

        //         string[] EmailIDs = CCMails.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        //         foreach (string MultiEmailTemp in EmailIDs)
        //         {
        //             msg.CC.Add(new MailAddress(MultiEmailTemp));
        //         }



        //         msg.Subject = "Complaint Registration : " + Details.Tables[0].Rows[0]["Complaint_No"].ToString();
        //         msg.Body = bodyTxt;
        //         msg.IsBodyHtml = true;
        //         msg.Priority = MailPriority.Normal;
        //         SmtpClient client = new SmtpClient();

        //         System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //         client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
        //         client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
        //         client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
        //         client.EnableSsl = true;
        //         client.Send(msg);

        //     }
        //     catch (Exception ex)
        //     {
        //         string errmsg = ex.Message;

        //     }
        // }
        //        public ActionResult Delete(int ComplaintID)
        //     {
        //     int Result = 0;
        //     try
        //     {
        //         Result = custSpecific.DeleteComplaint(ComplaintID);
        //         if (Result != 0)
        //         {
        //             TempData["Result"] = Result;
        //             return RedirectToAction("ComplaintsDashBoard");
        //         }
        //     }
        //     catch (Exception)
        //     {
        //         throw;
        //     }
        //     return View();
        // }

    }
}