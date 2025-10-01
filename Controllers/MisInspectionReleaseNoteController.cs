using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class MisInspectionReleaseNoteController : Controller
    {

        //page made by nikita on 03102023
        DALIRN OBJDALIRN = new DALIRN();
        ItemDescriptionModel modelItem = new ItemDescriptionModel();
        ReferenceDocumentsModel modelRef = new ReferenceDocumentsModel();
        InspectionActivitiesModel modelStg = new InspectionActivitiesModel();
        DocumentRevieweModel modelDr = new DocumentRevieweModel();
        InspectionvisitReportModel ObjModelVisitReport = new InspectionvisitReportModel();
        ReportModel objModelReport = new ReportModel();
        CommonControl objCommonControl = new CommonControl();
        // GET: InspectionReleaseNote
        // GET: MisInspectionReleaseNote
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        //[ValidateInput(false)]
        public ActionResult IRNForm(int? PK_IVR_ID, int? PK_RM_ID)
        {

            ObjModelVisitReport.Date_Of_Inspection = DateTime.Now.ToString("dd/MM/yyyy");
            if (PK_RM_ID != null)
            {
                ObjModelVisitReport.abcid = Convert.ToInt32(PK_RM_ID);
            }
            if (PK_IVR_ID != null)
            {
                ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(PK_IVR_ID);
            }

            Session["IRNReport"] = null;
            Session["PK_IVR_ID"] = 0;
            Session["PK_IVR_ID"] = Convert.ToInt32(PK_IVR_ID);
            Session["ReportNo"] = null;
            string DisplayIRN = string.Empty;
            DataTable LatestIRN = new DataTable();
            LatestIRN = OBJDALIRN.GetLatestIRN(PK_RM_ID);
            if (LatestIRN.Rows.Count > 0)
            {
                DisplayIRN = Convert.ToString(LatestIRN.Rows[0]["ReportName"]);
                ViewBag.LatestRep = Convert.ToString(DisplayIRN);
            }
            Session["IRNReport"] = Convert.ToString(DisplayIRN);

            DataSet GetUserbranchid = new DataSet();
            DataTable Reportdashboard = new DataTable();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();

            var Data1 = OBJDALIRN.GetAllReportList();
            ViewBag.ReportList = new SelectList(Data1, "ReportName", "ReportName");

            GetUserbranchid = OBJDALIRN.GetBranchId();
            var Data = OBJDALIRN.GetBranch();
            //var Data = OBJDALIRN.GetBranchList();







            ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");
            if (Convert.ToInt32(PK_IVR_ID) != 0)
            {
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = OBJDALIRN.EditUploadedFile(Convert.ToInt32(PK_IVR_ID));
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
                    ObjModelVisitReport.FileDetails = lstEditFileDetails;
                }

                DataSet DSJobMasterByQtId = new DataSet();
                DataSet DSEditQutationTabledata = new DataSet();
                DataSet dsDownloadPrint = new DataSet();

                dsDownloadPrint = OBJDALIRN.DownloadPrint(Convert.ToInt32(PK_IVR_ID));
                if (dsDownloadPrint.Tables[0].Rows.Count > 0)
                {
                    ObjModelVisitReport.DownloadPrint = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["Report"]);
                    ObjModelVisitReport.Report_No = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["ReportName"]);
                    ObjModelVisitReport.IsReviseReport = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["IsReviseReport"]);
                    Session["ReportNo"] = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["ReportName"]);
                }
                DSJobMasterByQtId = OBJDALIRN.Edit(Convert.ToInt32(PK_IVR_ID));
                if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
                {
                    ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubJob_No"]);
                    ObjModelVisitReport.Br_Id = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Branch"]);
                    ObjModelVisitReport.UserBranch = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Branch"]);
                    ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sap_And_Controle_No"]);
                    ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_IVR_ID"]);
                    ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name_Location"]);
                    ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Address_Of_Inspection"]);
                    ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["End_user_Name"]);
                    ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Name_Location"]);
                    // ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_Call_ID"]);
                    ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);
                    ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date_Of_Inspection"]);
                    ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Client_Name"]);
                    ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
                    ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
                    ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No"]);
                    ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Vendor_Name"]);
                    ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No_SubVendor"]);
                    ObjModelVisitReport.ReviseReason = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["ReviseReason"]);
                    ObjModelVisitReport.IVRIRNAttachment = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["IVRIRNAttachment"]);
                    ObjModelVisitReport.Emails_Distribution = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Emails_Distribution"]);
                    ObjModelVisitReport.client_Email = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["client_Email"]);
                    ObjModelVisitReport.Vendor_Email = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Email"]);
                    ObjModelVisitReport.Tuv_Branch = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Tuv_Branch"]);
                    ObjModelVisitReport.Conclusion = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Conclusion"]);
                    ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Pending_Activites"]);
                    ObjModelVisitReport.Identification_Of_Inspected = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Identification_Of_Inspected"]);
                    ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Areas_Of_Concerns"]);
                    ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Non_Conformities_raised"]);
                    ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Signature"]);
                    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Type"]);
                    ObjModelVisitReport.Report_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Report_No"]);

                    int Inspection_records = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Inspection_records"]);
                    ObjModelVisitReport.Inspection_records = Convert.ToBoolean(Inspection_records);
                    int Inspection_Photo = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Inspection_Photo"]);
                    ObjModelVisitReport.Inspection_Photo = Convert.ToBoolean(Inspection_Photo);
                    int Other_Specify = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Other_Specify"]);
                    ObjModelVisitReport.Other_Specify = Convert.ToBoolean(Other_Specify);
                    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Type"]);
                    ObjModelVisitReport.OrderStatus = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["OrderStatus"]);
                    ObjModelVisitReport.Sub_Order_Status = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Order_Status"]);
                    ObjModelVisitReport.arrey = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["IVRId"]);
                    ObjModelVisitReport.SubVendorPODate = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubVendorPODate"]);
                    ObjModelVisitReport.Date_of_PO = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date_of_Po"]);
                    ObjModelVisitReport.SubType = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubType"]);
                    ObjModelVisitReport.SubSubVendorDate_of_PO = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubvendorPODate"]);
                    ObjModelVisitReport.ddlReviseReason = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["ddlReviseReason"]);

                    string To = ObjModelVisitReport.arrey.ToString();
                    char[] delimiters = new[] { ',', ';', ' ' };
                    string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string MultiEmailTemp in EmailIDs)
                    {
                        //Reportdashboard = OBJDALIRN.GetIRNReportById(Convert.ToInt32( MultiEmailTemp));
                        Reportdashboard = OBJDALIRN.GetIRNReportByIdForBindTable(Convert.ToInt32(MultiEmailTemp));
                        if (Reportdashboard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in Reportdashboard.Rows)
                            {
                                lstCompanyDashBoard.Add(
                                    new ReportModel
                                    {
                                        ReportName = Convert.ToString(dr["ReportNo"]),
                                        Report = Convert.ToString(dr["Report"]),
                                        CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                        PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                        PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                        ProjectName = Convert.ToString(dr["ProjectName"]),
                                        inspectionDate = Convert.ToString(dr["inspectionDate"])
                                    }
                                  );
                            }
                        }
                    }
                }
                else
                {
                    //DSEditQutationTabledata = objDalVisitReport.GetCallDetails(PK_Call_ID);
                    //ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Job"]);
                    //ObjModelVisitReport.Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Executing_Branch"]);
                    //ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Actual_Visit_Date"]);
                    //ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Project_Name"]);
                    //ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Job_Location"]);
                    //ObjModelVisitReport.End_user_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["End_Customer"]);
                    //ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Name"]);
                    //ObjModelVisitReport.SubJob_No = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Job"]);
                    //ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_Call_ID"]);
                }
                //ObjModelVisitReport.arrey = null;
                ViewData["IvrList"] = lstCompanyDashBoard;
                return View(ObjModelVisitReport);
            }
            else
            {
                if (lstCompanyDashBoard.Count == 0)
                {
                    ViewData["IvrList"] = null;
                    //comment by shrutika salve 17082023
                    if (Data != null && Data.Count > 0)
                    {
                        BranchMasters firstElement = Data[0]; // Get the first element

                        // Assign values to properties
                        ObjModelVisitReport.Br_Id = firstElement.Br_Id;
                        ObjModelVisitReport.UserBranch = firstElement.Br_Id; // or another property you want to assign
                    }



                }
                else
                {
                    ViewData["IvrList"] = lstCompanyDashBoard;
                    if (GetUserbranchid.Tables[0].Rows.Count > 0)
                    {
                        ObjModelVisitReport.Br_Id = Convert.ToInt32(GetUserbranchid.Tables[0].Rows[0]["Br_Id"]);
                        ObjModelVisitReport.UserBranch = Convert.ToInt32(GetUserbranchid.Tables[0].Rows[0]["Br_Id"]);
                    }
                }
                return View(ObjModelVisitReport);
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult IRNForm(InspectionvisitReportModel IVR, HttpPostedFileBase File, FormCollection fc, HttpPostedFileBase[] Image, HttpPostedFileBase FileUpload1, ItemDescriptionModel IDM, ReferenceDocumentsModel RD, InspectionActivitiesModel StW, DocumentRevieweModel DR)
        {
            #region Added by Ankush
            int IRNID = 0;
            var IPath = string.Empty;
            string[] splitedGrp;
            List<FileDetails> fileDetails = new List<FileDetails>();
            List<string> Selected = new List<string>();
            #endregion

            DataTable ItemDescriptionDashBoard = new DataTable();
            DataTable RefranceDocumentsDashBoard = new DataTable();
            DataTable InspectionActivitesDashBoard = new DataTable();
            DataTable DocumentsReviewBoard = new DataTable();
            DataTable EquipmentDetailsBoard = new DataTable();

            //var Data = OBJDALIRN.GetBranchList();
            //ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");

            DataTable Reportdashboard = new DataTable();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();
            string Result = string.Empty;
            string Ivrid = Convert.ToString(fc["PK_IVR_ID"]);
            #region Maltipal image

            if (Image.Count() > 0)
            {
                foreach (HttpPostedFileBase item in Image)
                {
                    HttpPostedFileBase image = item;
                    if (image != null && image.ContentLength > 0)
                    {
                        string filePath = AppDomain.CurrentDomain.BaseDirectory + "Content/JobDocument\\" + image.FileName;
                        const string ImageDirectoryFP = "Content/JobDocument\\";
                        const string ImageDirectory = "~/Content/JobDocument/";
                        string ImagePath = "~/Content/JobDocument/" + image.FileName;
                        string fileNameWithExtension = System.IO.Path.GetExtension(image.FileName);
                        string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
                        string ImageName = image.FileName;

                        int iteration = 1;

                        while (System.IO.File.Exists(Server.MapPath(ImagePath)))
                        {
                            ImagePath = string.Concat(ImageDirectory, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                            filePath = string.Concat(ImageDirectoryFP, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                            ImageName = string.Concat(fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                            iteration += 1;
                        }

                        if (iteration == 1)
                        {
                            image.SaveAs(filePath);
                        }
                        else
                        {
                            image.SaveAs(AppDomain.CurrentDomain.BaseDirectory + filePath);
                        }
                        IVR.IVRIRNAttachment += ImageName + ",";
                    }
                }
            }
            #endregion

            #region Added by Ankush
            if (FileUpload1 == null)
            {

                FormCollection fca = new FormCollection();
                string filePathCMP = string.Empty;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file
                    int fileSize = files.ContentLength;
                    if (files != null && files.ContentLength > 0)
                    {
                        if (files.FileName.EndsWith(".msg") || files.FileName.EndsWith(".MSG") || files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))

                        {
                            string fileName = files.FileName;
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();
                            fileDetails.Add(fileDetail);
                            //-----------------------------------------------------
                            filePathCMP = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetail.Id + fileDetail.Extension);
                            var K = "~/Content/JobDocument/" + fileName;
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
                Session["listIRNUploadedFile"] = fileDetails;
            }
            #endregion

            try
            {
                if (Session["PK_IVR_ID"] != null)
                {
                    int PK_IVRID = Convert.ToInt32(Session["PK_IVR_ID"]);
                    #region  New Code 

                    if (PK_IVRID != 0)
                    {
                        #region File Upload Code 

                        IRNID = PK_IVRID;
                        if (IRNID != null && IRNID != 0)
                        {
                            if (fileDetails != null && fileDetails.Count > 0)
                            {
                                Result = OBJDALIRN.InsertFileAttachment(fileDetails, IRNID);
                                Session["listIRNUploadedFile"] = null;
                            }
                        }
                        //HttpPostedFileBase Imagesection;
                        //if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                        //{
                        //    Imagesection = Request.Files["img_Banner"];
                        //    if (Imagesection != null && Imagesection.FileName != "")
                        //    {
                        //        IVR.Signatures = CommonControl.FileUpload("Content/JobDocument/", Imagesection);
                        //    }
                        //    else
                        //    {
                        //        if (Imagesection.FileName != "")
                        //        {
                        //            IVR.Signatures = "NoImage.gif";
                        //        }
                        //    }
                        //}


                        #endregion

                        #region  Excel Upload code

                        Random rnd = new Random();
                        int myRandomNo = rnd.Next(10000000, 99999999);
                        string strmyRandomNo = Convert.ToString(myRandomNo);

                        HttpPostedFileBase files = FileUpload1;

                        if (FileUpload1 != null)
                        {

                            if (/*files.ContentLength > 0*/  files != null && !string.IsNullOrEmpty(files.FileName) || files.FileName.Contains(".xlsx") && files.FileName.Contains(".xlsm"))
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                //string Result = string.Empty;
                                string filePath = string.Empty;
                                // HttpPostedFileBase files = FileUpload;
                                string fileName = files.FileName;
                                string fileContentType = files.ContentType;
                                byte[] fileBytes = new byte[files.ContentLength];
                                var data1 = files.InputStream.Read(fileBytes, 0, Convert.ToInt32(files.ContentLength));
                                var package = new ExcelPackage(files.InputStream);  //===========Go to Manage Nuget in Install ExcellPackge 

                                #region save file to dir
                                string path = Server.MapPath("~/Excel/UploadedFiles/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }


                                filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);



                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);


                                }

                                string extension = Path.GetExtension(strmyRandomNo + FileUpload1.FileName);
                                FileUpload1.SaveAs(filePath);


                                filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                                #endregion





                                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                                Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath); ;//xlApp.Workbooks.Open(filePath);
                                String[] excelSheets = new String[excelBook.Worksheets.Count];
                                // var Reader = new StreamReader(File.Pa)

                                int i = 0;
                                foreach (Microsoft.Office.Interop.Excel.Worksheet wSheet in excelBook.Worksheets)
                                {
                                    excelSheets[i] = wSheet.Name;
                                    int RowsCount = wSheet.UsedRange.Rows.Count;// - 1;
                                    if (excelSheets[i] == "General ")
                                    {

                                    }
                                    else if (excelSheets[i] == "Item Description")
                                    {
                                        for (int j = 2; j <= RowsCount; j++)
                                        {
                                            //Convert.ToString(workSheet.Cells[rowIterator, 1].Value);
                                            IDM.Po_Item_No = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
                                            if (IDM.Po_Item_No != null)
                                            {
                                                IDM.Item_Code = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 2]).Value);
                                                IDM.ItemCode_Description = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 3]).Value);
                                                String Unit = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 4]).Value);

                                                if (Unit == "number") { IDM.Unit = "1"; }
                                                else if (Unit == "meter") { IDM.Unit = "2"; }
                                                else if (Unit == "km") { IDM.Unit = "3"; }
                                                else if (Unit == "meter (number)") { IDM.Unit = "4"; }
                                                else if (Unit == "km (number)") { IDM.Unit = "5"; }
                                                else if (Unit == "each") { IDM.Unit = "6"; }
                                                else if (Unit == "Piece") { IDM.Unit = "7"; }
                                                else if (Unit == "Test sample") { IDM.Unit = "8"; }
                                                else if (Unit == "AU - All Unit") { IDM.Unit = "9"; }
                                                else if (Unit == "lot") { IDM.Unit = "10"; }
                                                else if (Unit == "Set") { IDM.Unit = "11"; }
                                                else if (Unit == "Running Meter") { IDM.Unit = "12"; }
                                                else if (Unit == "kg") { IDM.Unit = "13"; }
                                                else if (Unit == "metric ton (tonne)") { IDM.Unit = "14"; }
                                                else if (Unit == "ton") { IDM.Unit = "15"; }
                                                else if (Unit == "cubic millimetre") { IDM.Unit = "16"; }
                                                else if (Unit == "cubic centimeter") { IDM.Unit = "17"; }
                                                else if (Unit == "cubic meter") { IDM.Unit = "18"; }
                                                else if (Unit == "cubic inch") { IDM.Unit = "19"; }
                                                else if (Unit == "cubic foot") { IDM.Unit = "20"; }
                                                else if (Unit == "mm") { IDM.Unit = "21"; }
                                                else if (Unit == "cm") { IDM.Unit = "22"; }
                                                else if (Unit == "in") { IDM.Unit = "23"; }
                                                else if (Unit == "foot") { IDM.Unit = "24"; }
                                                else if (Unit == "mile") { IDM.Unit = "25"; }
                                                else if (Unit == "yard") { IDM.Unit = "26"; }
                                                else if (Unit == "liter") { IDM.Unit = "27"; }
                                                else if (Unit == "kl") { IDM.Unit = "28"; }
                                                else if (Unit == "cl") { IDM.Unit = "29"; }
                                                else if (Unit == "ml") { IDM.Unit = "30"; }
                                                else if (Unit == "g") { IDM.Unit = "31"; }
                                                else if (Unit == "lb") { IDM.Unit = "32"; }
                                                else if (Unit == "oz") { IDM.Unit = "33"; }
                                                else if (Unit == "Sq. mm") { IDM.Unit = "34"; }
                                                else if (Unit == "Sq. cm") { IDM.Unit = "35"; }
                                                else if (Unit == "Sq. meter") { IDM.Unit = "36"; }
                                                else if (Unit == "Sq. in") { IDM.Unit = "37"; }
                                                else if (Unit == "Sq. foot") { IDM.Unit = "38"; }
                                                else
                                                {
                                                    IDM.Unit = "";

                                                }
                                                IDM.Po_Quantity = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 5]).Value);
                                                IDM.Offered_Quantity = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 6]).Value);
                                                IDM.Accepted_Quantity = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 7]).Value);
                                                IDM.Cumulative_Accepted_Qty = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 8]).Value);

                                                IDM.Status = "1";
                                                IDM.Type = "IRN";
                                                IDM.PK_CALL_ID = IVR.PK_Call_ID;
                                                //Result = objDalVisitReport.InsertUpdateItemDescription(IDM);
                                                Result = OBJDALIRN.InsertUpdateItemDescription(IDM);
                                            }

                                        }
                                    }
                                    else if (excelSheets[i] == "Reference Documents")
                                    {
                                        for (int j = 2; j <= RowsCount; j++)
                                        {
                                            string SrNo = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
                                            if (SrNo != null)
                                            {
                                                string DocName = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 2]).Value);
                                                if (DocName == "Others (Specify)")
                                                {
                                                    RD.Document_Name = "Others(Specify)";
                                                }
                                                else
                                                {
                                                    RD.Document_Name = DocName;
                                                }
                                                RD.Document_No = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 3]).Value);
                                                RD.VendorDocumentNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 4]).Value);
                                                RD.Approval_Status = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 5]).Value);
                                                RD.PK_CALL_ID = IVR.PK_Call_ID;
                                                Result = Result = OBJDALIRN.InsertUpdateReferenceDocuments(RD);
                                            }


                                        }
                                    }
                                    else if (excelSheets[i] == "Stages Witnessed")
                                    {
                                        for (int j = 2; j <= RowsCount; j++)
                                        {
                                            string StagesWitness = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
                                            if (StagesWitness != null)
                                            {
                                                StW.Type = "IVR";
                                                StW.Status = "1";
                                                StW.Stages_Witnessed = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
                                                StW.PK_CALL_ID = IVR.PK_Call_ID;
                                                Result = OBJDALIRN.InsertUpdateInspectionActivities(StW);
                                            }


                                        }
                                    }
                                    else if (excelSheets[i] == "Document reviewed")
                                    {
                                        for (int j = 2; j <= RowsCount; j++)
                                        {
                                            string DocumentRe = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
                                            if (DocumentRe != null)
                                            {
                                                DR.Type = "IVR";
                                                DR.Status = "1";
                                                DR.PK_CALL_ID = IVR.PK_Call_ID;
                                                DR.Description = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
                                                Result = OBJDALIRN.InsertUpdateDocumentReviewe(DR);
                                            }
                                        }
                                    }

                                    else if (excelSheets[i] == "Conclusion")
                                    {

                                    }


                                    i++;
                                }
                                excelBook.Close();


                            }
                            else
                            {

                            }
                        }

                        #endregion

                        IVR.Report_No = Convert.ToString(Session["ReportNo"]);
                        IVR.ReportName = Convert.ToString(Session["ReportNo"]);

                        string[] RepNo = IVR.ReportName.Split('v');
                        int prevcnt = 0;
                        string XYZNo = string.Empty;
                        string RevNo = string.Empty;
                        int No = Convert.ToInt32(RepNo[1]);
                        if (No != 0)
                        {
                            prevcnt = No - 1;
                            RevNo = Convert.ToString(No);
                            XYZNo = Convert.ToString(RepNo[0]) + "v" + " " + Convert.ToString(prevcnt);
                        }
                        else
                        {
                            RevNo = Convert.ToString("-");
                            XYZNo = Convert.ToString("-");
                        }
                        //------------------------------Report No Genrate code Start
                        IVR.Status = "1";
                        IVR.Type = "IRN";
                        //string id = OBJDALIRN.InsertUpdateInspectionvisit(IVR);
                        string id = OBJDALIRN.InsertUpdateInspectionvisit(IVR);



                        #region  item Description

                        ItemDescriptionDashBoard = OBJDALIRN.GetitemDescription(PK_IVRID);
                        if (ItemDescriptionDashBoard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in ItemDescriptionDashBoard.Rows)
                            {
                                var ItemDescription = new ItemDescriptionModel
                                {
                                    PK_ItemD_Id = Convert.ToInt32(dr["PK_ItemD_Id"]),
                                    Po_Item_No = Convert.ToString(dr["Po_Item_No"]),
                                    ItemCode_Description = Convert.ToString(dr["ItemCode_Description"]),
                                    Po_Quantity = Convert.ToString(dr["Po_Quantity"]),
                                    Offered_Quantity = Convert.ToString(dr["Offered_Quantity"]),
                                    PK_IVR_ID = IVR.PK_IVR_ID,
                                    Item_Code = Convert.ToString(dr["Item_Code"]),
                                    Accepted_Quantity = Convert.ToString(dr["Accepted_Quantity"]),
                                    Cumulative_Accepted_Qty = Convert.ToString(dr["Cumulative_Accepted_Qty"]),
                                    Unit = Convert.ToString(dr["Unit"]),
                                    Status = "1",
                                    Type = "IRN",

                                };
                                //Result = OBJDALIRN.InsertItemDescription(ItemDescription);
                            }
                        }
                        #endregion

                        #region Reference Documents

                        RefranceDocumentsDashBoard = OBJDALIRN.GetReferenceDocuments(PK_IVRID);
                        if (RefranceDocumentsDashBoard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in RefranceDocumentsDashBoard.Rows)
                            {
                                var ItemDescription = new ReferenceDocumentsModel
                                {
                                    Document_No = Convert.ToString(dr["Document_No"]),
                                    Document_Name = Convert.ToString(dr["Document_Name"]),
                                    Approval_Status = Convert.ToString(dr["Approval_Status"]),
                                    PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                                    VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"]),
                                    PK_IVR_ID = IVR.PK_IVR_ID,
                                    Type = "IRN",
                                    Status = "1",

                                };
                                //Result = OBJDALIRN.InsertReferenceDocuments(ItemDescription);
                            }
                        }
                        #endregion

                        #region Inspection Activities
                        InspectionActivitesDashBoard = OBJDALIRN.GetInspectionActivities(PK_IVRID);
                        if (InspectionActivitesDashBoard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in InspectionActivitesDashBoard.Rows)
                            {
                                var InspectionActivities = new InspectionActivitiesModel
                                {
                                    Stages_Witnessed = Convert.ToString(dr["Stages_Witnessed"]),
                                    PK_IA_ID = Convert.ToInt32(dr["PK_IA_ID"]),
                                    PK_IVR_ID = IVR.PK_IVR_ID,
                                    Type = "IRN",
                                    Status = "1",

                                };
                                //Result = OBJDALIRN.InsertInspectionActivities(InspectionActivities);
                            }
                        }
                        #endregion

                        #region Documents Review
                        DocumentsReviewBoard = OBJDALIRN.GetDocumentRevieweModelByCall_Id(Convert.ToInt32(PK_IVRID));
                        if (DocumentsReviewBoard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in DocumentsReviewBoard.Rows)
                            {
                                var DocumentsReview = new DocumentRevieweModel
                                {
                                    Description = Convert.ToString(dr["Description"]),
                                    PK_DR_ID = Convert.ToInt32(dr["PK_DR_ID"]),
                                    PK_IVR_ID = IVR.PK_IVR_ID,
                                    Type = "IRN",
                                    Status = "1",
                                };
                                /*Result = OBJDALIRN.InsertDocumentReviewe(DocumentsReview)*/
                                ;

                            }
                        }
                        #endregion
                        if (id != "" && id != null)
                        {
                            //this.IRNReportGanrate(Convert.ToInt32(id));
                        }
                        #region Generate PDF of IRN on Insert
                        if (IVR.PK_IVR_ID != 0)
                        {
                            DataTable ItemDescriptionDashBoard1 = new DataTable();
                            DataTable RefranceDocumentsDashBoard1 = new DataTable();
                            DataTable InspectionActivitesDashBoard1 = new DataTable();
                            DataTable DocumentsReviewBoard1 = new DataTable();
                            DataTable EquipmentDetailsBoard1 = new DataTable();
                            DataSet DSJobMasterByQtId1 = new DataSet();
                            DataTable ReportDashBoard1 = new DataTable();
                            DataTable CostSheetDashBoard1 = new DataTable();
                            int count = 0;
                            DataTable ImageReportDashBoard1 = new DataTable();
                            List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();
                            List<ItemDescriptionModel> lstCompanyDashBoard1 = new List<ItemDescriptionModel>();
                            List<ReferenceDocumentsModel> RefranceDocuments1 = new List<ReferenceDocumentsModel>();
                            List<InspectionActivitiesModel> InspectionDocuments1 = new List<InspectionActivitiesModel>();
                            List<DocumentRevieweModel> DocumentReview1 = new List<DocumentRevieweModel>();
                            List<EquipmentDetailsModel> EquipmentDetails1 = new List<EquipmentDetailsModel>();
                            List<ReportModel> ReportDashboard1 = new List<ReportModel>();

                            DataTable SubDashBoard = new DataTable();
                            int count1 = 0;
                            int count2 = 0;
                            string SrNo = null;
                            SubDashBoard = OBJDALIRN.GetReportBySubjob_Id(IVR.SubJob_No);
                            if (SubDashBoard.Rows.Count >= 0)
                            {
                                count1 = SubDashBoard.Rows.Count;
                                count2 = count1 - 1;
                            }
                            SrNo = Convert.ToString(count1);

                            ReportModel RM = new ReportModel();
                            string Result1 = "";
                            if (IVR.PK_IVR_ID != 0)
                            {
                                int i = 0;
                                int J = 0;
                                int K = 0;
                                int L = 0;
                                int M = 0;
                                int N = 0;
                                #region 
                                DSJobMasterByQtId1 = OBJDALIRN.EditInspectionVisitReportByPKCallID(IVR.PK_IVR_ID);

                                if (DSJobMasterByQtId1.Tables[0].Rows.Count > 0)
                                {
                                    ObjModelVisitReport.Branch = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Branch"]);
                                    ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sap_And_Controle_No"]);
                                    ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["PK_IVR_ID"]);
                                    ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Project_Name_Location"]);
                                    ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Address_Of_Inspection"]);
                                    ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["End_user_Name"]);
                                    ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Vendor_Name_Location"]);
                                    // ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["PK_Call_ID"]);
                                    ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Notification_Name_No_Date"]);
                                    ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Date_Of_Inspection"]);
                                    ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Client_Name"]);
                                    ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
                                    ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
                                    ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Po_No"]);
                                    ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sub_Vendor_Name"]);
                                    ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Po_No_SubVendor"]);

                                    //int kick = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Kick_Off_Pre_Inspection"]);
                                    //ObjModelVisitReport.Kick_Off_Pre_Inspection = Convert.ToBoolean(kick);

                                    //int Mi = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Material_identification"]);
                                    //ObjModelVisitReport.Material_identification = Convert.ToBoolean(Mi);

                                    //int Interim_Stages = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Interim_Stages"]);
                                    //ObjModelVisitReport.Interim_Stages = Convert.ToBoolean(Interim_Stages);

                                    //int Document_review = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Document_review"]);
                                    //ObjModelVisitReport.Document_review = Convert.ToBoolean(Document_review);

                                    //int Final_Inspection = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Final_Inspection"]);
                                    //ObjModelVisitReport.Final_Inspection = Convert.ToBoolean(Final_Inspection);

                                    //int Re_inspection = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Re_inspection"]);
                                    //ObjModelVisitReport.Re_inspection = Convert.ToBoolean(Re_inspection);

                                    ObjModelVisitReport.Conclusion = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Conclusion"]);
                                    ObjModelVisitReport.Waivers = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Waivers"]);

                                    ObjModelVisitReport.ReviseReason = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["ReviseReason"]);

                                    int dvmdr1 = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["DVendor"]);
                                    ObjModelVisitReport.vendot = Convert.ToBoolean(dvmdr1);
                                    int dclnt1 = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["DClient"]);
                                    ObjModelVisitReport.client = Convert.ToBoolean(dclnt1);
                                    int dtuv1 = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["DTUV"]);
                                    ObjModelVisitReport.TUV = Convert.ToBoolean(dtuv1);

                                    ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Pending_Activites"]);
                                    ObjModelVisitReport.Identification_Of_Inspected = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Identification_Of_Inspected"]);
                                    ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Areas_Of_Concerns"]);
                                    ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Non_Conformities_raised"]);
                                    ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Signature"]);
                                    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Type"]);
                                    ObjModelVisitReport.Report_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Report_No"]);
                                    // ObjModelVisitReport.Call_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Call_No"]);
                                    //ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Signature"]);
                                    ObjModelVisitReport.Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["LastName"]);
                                    ObjModelVisitReport.ReportCreatedDate = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["CreatedDate"]);

                                    int Inspection_records = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Inspection_records"]);
                                    ObjModelVisitReport.Inspection_records = Convert.ToBoolean(Inspection_records);

                                    int Inspection_Photo = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Inspection_Photo"]);
                                    ObjModelVisitReport.Inspection_Photo = Convert.ToBoolean(Inspection_Photo);

                                    int Other_Specify = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Other_Specify"]);
                                    ObjModelVisitReport.Other_Specify = Convert.ToBoolean(Other_Specify);

                                    ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["SubJob_No"]);
                                    ObjModelVisitReport.Sub_Order_Status = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sub_Order_Status"]);
                                    ObjModelVisitReport.OrderStatus = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["OrderStatus"]);

                                    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Type"]);

                                    int POTotalCheckBox = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["POTotalCheckBox"]);
                                    ObjModelVisitReport.POTotalCheckBox = Convert.ToBoolean(POTotalCheckBox);
                                    ObjModelVisitReport.PO_QuantityTotal = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["PO_QuantityTotal"]);
                                    ObjModelVisitReport.Offered_QuantityTotal = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Offered_QuantityTotal"]);
                                    ObjModelVisitReport.Accepted_QuantityTotal = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Accepted_QuantityTotal"]);
                                    ObjModelVisitReport.Cumulative_Accepted_QtyTotal = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Cumulative_Accepted_QtyTotal"]);
                                }
                                #endregion

                                #region  item Description

                                ItemDescriptionDashBoard1 = OBJDALIRN.GetitemDescription(IVR.PK_IVR_ID);
                                if (ItemDescriptionDashBoard1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ItemDescriptionDashBoard1.Rows)
                                    {
                                        lstCompanyDashBoard1.Add(
                                            new ItemDescriptionModel
                                            {
                                                PK_ItemD_Id = Convert.ToInt32(dr["PK_ItemD_Id"]),
                                                Po_Item_No = Convert.ToString(dr["Po_Item_No"]),
                                                ItemCode_Description = Convert.ToString(dr["ItemCode_Description"]),
                                                Po_Quantity = Convert.ToString(dr["Po_Quantity"]),
                                                Offered_Quantity = Convert.ToString(dr["Offered_Quantity"]),
                                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                                Item_Code = Convert.ToString(dr["Item_Code"]),
                                                Accepted_Quantity = Convert.ToString(dr["Accepted_Quantity"]),
                                                Cumulative_Accepted_Qty = Convert.ToString(dr["Cumulative_Accepted_Qty"]),
                                                Unit = Convert.ToString(dr["Unit"]),
                                            }
                                            );
                                    }
                                }
                                #endregion

                                #region Reference Documents

                                RefranceDocumentsDashBoard1 = OBJDALIRN.GetReferenceDocuments(IVR.PK_IVR_ID);
                                if (RefranceDocumentsDashBoard1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in RefranceDocumentsDashBoard1.Rows)
                                    {
                                        RefranceDocuments1.Add(
                                            new ReferenceDocumentsModel
                                            {
                                                Document_No = Convert.ToString(dr["Document_No"]),
                                                Document_Name = Convert.ToString(dr["Document_Name"]),
                                                Approval_Status = Convert.ToString(dr["Approval_Status"]),
                                                PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                                                VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"]),
                                                // PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])

                                            }
                                            );
                                    }
                                }
                                #endregion

                                #region Inspection Activities
                                InspectionActivitesDashBoard1 = OBJDALIRN.GetInspectionActivities(IVR.PK_IVR_ID);
                                if (InspectionActivitesDashBoard1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in InspectionActivitesDashBoard1.Rows)
                                    {
                                        InspectionDocuments1.Add(
                                            new InspectionActivitiesModel
                                            {
                                                Stages_Witnessed = Convert.ToString(dr["Stages_Witnessed"]),
                                                PK_IA_ID = Convert.ToInt32(dr["PK_IA_ID"]),
                                                //  PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])

                                            }
                                            );
                                    }
                                }
                                #endregion

                                #region Documents Review
                                DocumentsReviewBoard1 = OBJDALIRN.GetDocumentRevieweModelByCall_Id(Convert.ToInt32(IVR.PK_IVR_ID));
                                if (DocumentsReviewBoard1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in DocumentsReviewBoard1.Rows)
                                    {
                                        DocumentReview1.Add(
                                            new DocumentRevieweModel
                                            {
                                                Description = Convert.ToString(dr["Description"]),
                                                PK_DR_ID = Convert.ToInt32(dr["PK_DR_ID"]),
                                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])

                                            }
                                            );
                                    }
                                }
                                #endregion

                                #region report Count
                                ReportDashBoard1 = OBJDALIRN.GetReportByCall_Id(IVR.PK_IVR_ID);
                                if (ReportDashBoard1.Rows.Count > 0)
                                {
                                    count = ReportDashBoard1.Rows.Count;
                                }
                                string countNo = Convert.ToString(count);
                                #endregion

                                #region Save to Pdf Code 

                                #region Get Date Of Inspection & Inspector Name

                                DataSet dsIN = new DataSet();
                                int z = 0;
                                string InspectorName = "";
                                string Adate = string.Empty;
                                string Date = "";
                                string InrName = string.Empty;

                                List<string> lstInspector = new List<string>();
                                List<string> lstDates = new List<string>();

                                string InsList = string.Empty;
                                dsIN = OBJDALIRN.GetInspectionName(Convert.ToInt32(IVR.PK_IVR_ID));
                                if (dsIN.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dsIN.Tables[0].Rows)
                                    {
                                        InspectorName = Convert.ToString(dr["InspectorName"]);
                                        lstInspector.Add(InspectorName);
                                    }
                                    InsList = "On behalf of " + string.Join(",", lstInspector);
                                }
                                else
                                {
                                    InsList = "";
                                }
                                if (dsIN.Tables[1].Rows.Count > 0)
                                {
                                    InrName = dsIN.Tables[1].Rows[0]["InspectorName"].ToString();
                                }
                                else
                                {
                                    InrName = "";
                                }
                                if (dsIN.Tables[2].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dsIN.Tables[2].Rows)
                                    {
                                        Adate = Convert.ToString(dr["Actual_Visit_Date"]);
                                        lstDates.Add(Adate);
                                    }
                                    Date = string.Join(",", lstDates);


                                }
                                else
                                {
                                    Date = "";
                                }
                                #endregion
                                SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
                                System.Text.StringBuilder strs = new System.Text.StringBuilder();
                                string body = string.Empty;

                                string ItemDescriptioncontent = "";
                                string ReferenceDocumentscontent = "";
                                string InspectionDocumentsContent = "";
                                string DocumentreviewContent = "";
                                //string EquipmentDetailscontent = "";

                                string check1 = "";
                                string check2 = "";
                                string dvenr11 = string.Empty;
                                string dclnt11 = string.Empty;
                                string dtuv11 = string.Empty;

                                using (StreamReader reader = new StreamReader(Server.MapPath("~/inspection-certificate.html")))
                                {
                                    body = reader.ReadToEnd();
                                }

                                body = body.Replace("[SapAndControle_No]", ObjModelVisitReport.SubJob_No);
                                body = body.Replace("[Branch]", ObjModelVisitReport.Branch);
                                body = body.Replace("[NotificationNameNo_Date]", ObjModelVisitReport.Notification_Name_No_Date);
                                // body = body.Replace("[DateOfInspection]", ObjModelVisitReport.Date_Of_Inspection);
                                body = body.Replace("[DateOfInspection]", Date);
                                body = body.Replace("[InspectorNames]", InsList);
                                if (ObjModelVisitReport.ReviseReason != "-" && ObjModelVisitReport.ReviseReason != "")
                                {
                                    body = body.Replace("[ReviseReason]", "Reason for Revise- " + Convert.ToString(ObjModelVisitReport.ReviseReason));
                                }
                                else
                                {
                                    body = body.Replace("[ReviseReason]", "-");
                                }
                                body = body.Replace("[ProjectNameLocation]", ObjModelVisitReport.Project_Name_Location);
                                body = body.Replace("[AddressOfInspection]", ObjModelVisitReport.Address_Of_Inspection);
                                body = body.Replace("[ClientName]", ObjModelVisitReport.Client_Name);
                                body = body.Replace("[Enduser_Name]", ObjModelVisitReport.End_user_Name);
                                body = body.Replace("[DECPMCEPC_Name]", ObjModelVisitReport.DEC_PMC_EPC_Name);
                                body = body.Replace("[DECPMCEPCAssignment_No]", ObjModelVisitReport.DEC_PMC_EPC_Assignment_No);
                                body = body.Replace("[VendorNameLocation]", ObjModelVisitReport.Vendor_Name_Location);
                                body = body.Replace("[PoNo]", ObjModelVisitReport.Po_No);
                                body = body.Replace("[SubVendorName]", ObjModelVisitReport.Sub_Vendor_Name);
                                body = body.Replace("[PoNoSubVendor]", ObjModelVisitReport.Po_No_SubVendor);
                                body = body.Replace("[Conclusion]", ObjModelVisitReport.Conclusion);
                                body = body.Replace("[PendingActivites]", ObjModelVisitReport.Pending_Activites);
                                body = body.Replace("[IdentificationOfInspected]", ObjModelVisitReport.Identification_Of_Inspected);
                                body = body.Replace("[AreasOfConcerns]", ObjModelVisitReport.Areas_Of_Concerns);
                                body = body.Replace("[NonConformitiesraised]", ObjModelVisitReport.Non_Conformities_raised);
                                // body = body.Replace("[Name]", ObjModelVisitReport.Name);
                                body = body.Replace("[Name]", InrName);

                                body = body.Replace("[date]", ObjModelVisitReport.ReportCreatedDate);

                                body = body.Replace("[Waivers]", ObjModelVisitReport.Waivers);

                                body = body.Replace("[RevisionNo]", IVR.Report_No);
                                if (ObjModelVisitReport.vendot == true)
                                {
                                    dvenr11 = "<span><input type='checkbox' checked> Vendor / Sub Vendor</span>";
                                    //"<span><input type='checkbox' checked> Vendor / Sub Vendor</span>"
                                }
                                else
                                {
                                    dvenr11 = "<span><input type='checkbox'> Vendor / Sub Vendor</span>";
                                    //"<span><input type='checkbox'> Vendor / Sub Vendor</span>";
                                }

                                if (ObjModelVisitReport.client == true)
                                {
                                    dclnt11 = "<span><input type='checkbox' checked> TUVI Client / End User</span>";
                                    //"<span><input type='checkbox' checked> TUVI Client / End User</span>";
                                }
                                else
                                {
                                    dclnt11 = "<span><input type='checkbox'> TUVI Client / End User</span>";
                                    //dvenr11 = "<span><input type='checkbox'> TUVI Client / End User</span>";
                                }

                                if (ObjModelVisitReport.TUV == true)
                                {
                                    dtuv11 = "<span><input type='checkbox' checked> TUV Executing Branch / TUV Originating Branch</span>";
                                }
                                else
                                {
                                    dtuv11 = "<span><input type='checkbox'> TUV Executing Branch / TUV Originating Branch</span>";
                                }


                                if (ObjModelVisitReport.OrderStatus == "Complete")
                                {
                                    //check1 = "<td width='5%' align='center' style='border-right-width: 0px;'><span><input type='checkbox'  value=" + ObjModelVisitReport.Kick_Off_Pre_Inspection + " checked></span></td>";
                                    check1 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox'>Incomplete</label></span></td>";
                                }
                                else
                                {
                                    //check1 = "<td width='5%' align='center' style='border-right-width: 0px;'><span><input type='checkbox'  value=" + ObjModelVisitReport.Kick_Off_Pre_Inspection + "></span></td>";
                                    check1 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Incomplete</label></span></td>";
                                }
                                if (ObjModelVisitReport.Sub_Order_Status == "Complete")
                                {
                                    check2 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox'>Incomplete</label></span></td>";
                                }
                                else
                                {
                                    check2 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Incomplete</label></span></td>";
                                }
                                //foreach (ItemDescriptionModel v in lstCompanyDashBoard1)
                                //{
                                //    i = i + 1;
                                //    ItemDescriptioncontent += "<tr><td style='border: 1px solid #000000;' width='5%' align='left'><span>" + Convert.ToString(v.Po_Item_No) + ')' + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Item_Code + " </span></td><td style='border: 1px solid #000000;' width='35%'><span>" + v.ItemCode_Description + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Unit + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Po_Quantity + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Offered_Quantity + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Accepted_Quantity + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Cumulative_Accepted_Qty + "</span></td></tr>";
                                //}
                                foreach (ItemDescriptionModel v in lstCompanyDashBoard1)
                                {
                                    i = i + 1;

                                    if (i == lstCompanyDashBoard1.Count)
                                        ItemDescriptioncontent += "<tr><td style = 'border:1px solid #000000;' width = '5%' align = 'center' ><span> " + Convert.ToString(v.Po_Item_No) + ')' + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Item_Code + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;white-space: pre-line;' width = '35%' ><span> " + v.ItemCode_Description + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Unit + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Po_Quantity + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Offered_Quantity + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Accepted_Quantity + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Cumulative_Accepted_Qty + " </span></td></tr>";
                                    else
                                        ItemDescriptioncontent += "<tr><td style = 'border:1px solid #000000;border-bottom-width: 0px;' width = '5%' align = 'center' ><span> " + Convert.ToString(v.Po_Item_No) + ')' + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Item_Code + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;white-space: pre-line;' width = '35%' ><span> " + v.ItemCode_Description + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Unit + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Po_Quantity + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Offered_Quantity + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Accepted_Quantity + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Cumulative_Accepted_Qty + " </span></td></tr>";
                                }


                                //foreach (ReferenceDocumentsModel v in RefranceDocuments1)
                                //{
                                //    J = J + 1;
                                //    ReferenceDocumentscontent += "<tr><td style='border: 1px solid #000000;' width='5%' align='left'> " + J + ')' + " </td><td style='border: 1px solid #000000;' width='25%'><span style='white-space: pre-line;'>" + Convert.ToString(v.Document_Name) + "</td><td style='border: 1px solid #000000;' width='25%'><span style='white-space: pre-line;'>" + v.Document_No + " </td><td style='border: 1px solid #000000;' width='25%'><span style='white-space: pre-line;'>" + v.VendorDocumentNumber + "</td><td style='border: 1px solid #000000;' width='25%'>" + v.Approval_Status + "</td></tr>";
                                //}

                                foreach (ReferenceDocumentsModel v in RefranceDocuments1)
                                {
                                    J = J + 1;
                                    if (J == RefranceDocuments1.Count)
                                        ReferenceDocumentscontent += "<tr><td style = 'border:1px solid #000000;vertical-align:top; text-align:center;'  " + J + ')' + " </td><td width = '25%' style = 'border:1px solid #000000;border-left-width: 0px; white-space: pre-line;vertical-align:top;' > " + Convert.ToString(v.Document_Name) + " </td><td width = '25%' style = 'border:1px solid #000000;border-left-width: 0px;white-space:pre-line;vertical-align:top;' > " + v.Document_No + " </td><td width = '25%' style = 'border:1px solid #000000;border-left-width: 0px;white-space:pre-line;vertical-align:top;' > " + v.VendorDocumentNumber + " </td><td width = '25%' style = 'border:1px solid #000000;border-left-width: 0px;white-space: pre-line;vertical-align:top;' > " + v.Approval_Status + " </td></tr> ";
                                    else
                                        ReferenceDocumentscontent += "<tr><td style = 'border:1px solid #000000;border-bottom-width: 0px;vertical-align:top; text-align:center;'  " + J + ')' + " </td><td width = '25%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px; white-space: pre-line;vertical-align:top;' > " + Convert.ToString(v.Document_Name) + " </td><td width = '25%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;white-space:pre-line;vertical-align:top;' > " + v.Document_No + " </td><td width = '25%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;white-space:pre-line;vertical-align:top;' > " + v.VendorDocumentNumber + " </td><td width = '25%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;white-space: pre-line;vertical-align:top;' > " + v.Approval_Status + " </td></tr> ";
                                }


                                foreach (InspectionActivitiesModel v in InspectionDocuments1)
                                {
                                    K = K + 1;
                                    InspectionDocumentsContent += "<tr><td width='5%' align='left'><span> " + K + ')' + " </span></td><td width='95%'><span style='white-space: pre-line;'>" + Convert.ToString(v.Stages_Witnessed) + "</span></td></tr>";
                                }
                                foreach (DocumentRevieweModel v in DocumentReview1)
                                {
                                    L = L + 1;
                                    DocumentreviewContent += "<tr><td width='5%' align='left'><span> " + L + ')' + " </span></td><td width='95%' ><span style='white-space: pre-line;'>" + Convert.ToString(v.Description) + "</span></td></tr>";
                                }
                                body = body.Replace("[ItemDescriptionContent]", ItemDescriptioncontent);
                                body = body.Replace("[ReferenceDocumentsContent]", ReferenceDocumentscontent);
                                body = body.Replace("[InspectionDocumentsContent]", InspectionDocumentsContent);
                                body = body.Replace("[DocumentreviewContent]", DocumentreviewContent);
                                body = body.Replace("[Stamp]", ConfigurationManager.AppSettings["Web"].ToString() + "/Stamp.png");
                                if (ObjModelVisitReport.Signatures != null)
                                {
                                    body = body.Replace("[Signature]", "https://tiimes.tuv-india.com/Content/Sign/" + ObjModelVisitReport.Signatures + "");
                                }
                                else
                                {
                                    body = body.Replace("[Signature]", "-");
                                }
                                body = body.Replace("[Checkbox1]", check1);
                                body = body.Replace("[Checkbox2]", check2);
                                body = body.Replace("[DVendor]", dvenr11);
                                body = body.Replace("[DClient]", dclnt11);
                                body = body.Replace("[DTUV]", dtuv11);
                                body = body.Replace("[PreviousNo]", XYZNo);
                                body = body.Replace("[RevNo]", RevNo);


                                string po = Convert.ToString(ObjModelVisitReport.POTotalCheckBox);
                                body = body.Replace("[POTotalCheckBox]", Convert.ToString(ObjModelVisitReport.POTotalCheckBox));
                                body = body.Replace("[PO_QuantityTotal]", Convert.ToString(ObjModelVisitReport.PO_QuantityTotal));
                                body = body.Replace("[Offered_QuantityTotal]", Convert.ToString(ObjModelVisitReport.Offered_QuantityTotal));
                                body = body.Replace("[Accepted_QuantityTotal]", Convert.ToString(ObjModelVisitReport.Accepted_QuantityTotal));
                                body = body.Replace("[Cumulative_Accepted_QtyTotal]", Convert.ToString(ObjModelVisitReport.Cumulative_Accepted_QtyTotal));

                                if (po == "True")
                                {
                                    body = body.Replace("[POTotal]", "<tr><td style='border: 1px solid #b5b5b5;' width='5%' align='center'><span>" + "" + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + "" + " </span></td><td style='border: 1px solid #b5b5b5;white-space: pre-line;' width='35%'><span>" + "" + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + "Total" + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.PO_QuantityTotal + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.Offered_QuantityTotal + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.Accepted_QuantityTotal + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.Cumulative_Accepted_QtyTotal + "</span></td></tr>");
                                    //ItemDescriptioncontent += "<tr><td style='border: 1px solid #b5b5b5;' width='5%' align='center'><span>" + "" + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + "" + " </span></td><td style='border: 1px solid #b5b5b5;white-space: pre-line;' width='35%'><span>" + "" + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + "Total" + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.PO_QuantityTotal + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.Offered_QuantityTotal + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.Accepted_QuantityTotal + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.Cumulative_Accepted_QtyTotal + "</span></td></tr>";

                                }
                                else
                                {
                                    body = body.Replace("[Signature]", "");
                                }


                                strs.Append(body);
                                PdfPageSize pageSize = PdfPageSize.A4;
                                PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
                                HtmlToPdf converter = new HtmlToPdf();
                                converter.Options.MaxPageLoadTime = 120;  //=========================5-Aug-2019
                                converter.Options.PdfPageSize = pageSize;
                                converter.Options.PdfPageOrientation = pdfOrientation;

                                string _Header = string.Empty;
                                string _footer = string.Empty;

                                // for Report header by abel
                                StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/Inspection_Certificate_Header.html"));
                                _Header = _readHeader_File.ReadToEnd();

                                _Header = _Header.Replace("[RevisionNo]", IVR.Report_No);
                                _Header = _Header.Replace("[Logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png"); // change123 once pulished on server

                                StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/inspection-certificate-footer.html")); // Footer is used from IVR as it same and commented in pdf template.
                                _footer = _readFooter_File.ReadToEnd();

                                // header settings
                                converter.Options.DisplayHeader = true ||
                                    true || true;
                                converter.Header.DisplayOnFirstPage = true;
                                converter.Header.DisplayOnOddPages = true;
                                converter.Header.DisplayOnEvenPages = true;
                                converter.Header.Height = 80;

                                PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
                                headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                                converter.Header.Add(headerHtml);

                                // footer settings
                                converter.Options.DisplayFooter = true || true || true;
                                converter.Footer.DisplayOnFirstPage = true;
                                converter.Footer.DisplayOnOddPages = true;
                                converter.Footer.DisplayOnEvenPages = true;
                                //converter.Footer.Height = 170;
                                converter.Footer.Height = 130;

                                PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
                                footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                                converter.Footer.Add(footerHtml);

                                //end abel code

                                #region Footer Code
                                // page numbers can be added using a PdfTextSection object
                                PdfTextSection text = new PdfTextSection(0, 90, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
                                text.HorizontalAlign = PdfTextHorizontalAlign.Right;
                                converter.Footer.Add(text);
                                #endregion



                                RM.PK_RM_ID = Convert.ToInt32(IVR.abcid);

                                string ReportName = string.Empty;
                                string Report = string.Empty;
                                DataTable GetFileName = new DataTable();
                                GetFileName = OBJDALIRN.GetIRNReportById(Convert.ToInt32(RM.PK_RM_ID));
                                if (GetFileName.Rows.Count > 0)
                                {
                                    ReportName = Convert.ToString(GetFileName.Rows[0]["ReportName"]);
                                    Report = Convert.ToString(GetFileName.Rows[0]["Report"]);
                                }
                                if (Report != null)
                                {
                                    var pathdelete = Path.Combine(Server.MapPath("~/IRNReports/"), Report);
                                    if (System.IO.File.Exists(pathdelete))
                                    {
                                        System.IO.File.Delete(pathdelete);
                                    }
                                }
                                string test = Report;

                                PdfDocument doc = converter.ConvertHtmlString(body);

                                //string imgFile = Server.MapPath("~/Content/watermark.jpg");


                                ////PDFs can be edited or amended by stamping new HTML content into the foreground or background.
                                //IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
                                //var pdf = Renderer.RenderUrlAsPdf("https://www.nuget.org/packages/IronPdf");

                                //var BackgroundStamp = new IronPdf.HtmlStamp() { Html = "<img src='"+imgFile + "' />", Width = 50, Height = 25, Opacity = 50, Bottom = 5, ZIndex = IronPdf.HtmlStamp.StampLayer.BehindExistingPDFContent, HtmlBaseUrl = imgFile };
                                //pdf.StampHTML(BackgroundStamp);
                                //var ForegroundStamp = new IronPdf.HtmlStamp() { Html = "<h2 style='color:red'>copyright 2018 ironpdf.com", Width = 50, Height = 50, Opacity = 50, Rotation = -45, ZIndex = IronPdf.HtmlStamp.StampLayer.OnTopOfExistingPDFContent };
                                //pdf.StampHTML(ForegroundStamp);
                                //pdf.SaveAs(@"C:\Path\To\Stamped.pdf");


                                converter.Options.SecurityOptions.CanCopyContent = false;
                                converter.Options.SecurityOptions.CanEditAnnotations = false;
                                converter.Options.SecurityOptions.CanEditContent = false;

                                string path = Server.MapPath("~/IRNReports");
                                doc.Save(path + '\\' + Report);
                                doc.Close();
                                #endregion

                                if (RM.PK_RM_ID != 0)
                                {
                                    RM.PK_RM_ID = Convert.ToInt32(IVR.abcid);
                                    RM.Type = "IRN";
                                    RM.Status = "1";
                                    RM.Report = Report;
                                    RM.ReportName = ReportName;
                                    RM.PK_CALL_ID = IVR.PK_IVR_ID;
                                    RM.VendorName = ObjModelVisitReport.Vendor_Name_Location;
                                    RM.ClientName = ObjModelVisitReport.Client_Name;
                                    RM.ProjectName = ObjModelVisitReport.Project_Name_Location;
                                    RM.SubJob_No = ObjModelVisitReport.SubJob_No;
                                    Result1 = OBJDALIRN.InsertUpdateReport(RM);
                                    if (Result1 != "" && Result1 != null)
                                    {
                                        TempData["InsertCompany"] = Result1;
                                    }
                                }

                                #region
                                CostSheetDashBoard1 = OBJDALIRN.GetReportByCall_Id(IVR.PK_IVR_ID);
                                if (CostSheetDashBoard1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in CostSheetDashBoard1.Rows)
                                    {
                                        ReportDashboard1.Add(
                                            new ReportModel
                                            {
                                                ReportName = Convert.ToString(dr["ReportName"]),
                                                Report = Convert.ToString(dr["Report"]),
                                                CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                                PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                                // PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
                                            }
                                            );
                                    }
                                }
                                ViewData["CostSheet"] = ReportDashboard1;
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else
                    {//Insert
                        DataTable SubDashBoard = new DataTable();
                        int count1 = 0;
                        int count2 = 0;
                        string SrNo = null;
                        SubDashBoard = OBJDALIRN.GetReportBySubjob_Id(IVR.SubJob_No);
                        if (SubDashBoard.Rows.Count >= 0)
                        {
                            count1 = SubDashBoard.Rows.Count;
                            count2 = count1 + 1;
                        }
                        SrNo = Convert.ToString(count2);

                        //int _min = 10000;
                        //int _max = 99999;
                        //Random _rdm = new Random();
                        //int Rjno = _rdm.Next(_min, _max);
                        //string ConfirmCode = Convert.ToString(Rjno);

                        //int _mins = 100000;
                        //int _maxs = 999999;
                        //Random _rdms = new Random();
                        //int Rjnos = _rdm.Next(_mins, _maxs);
                        //string ConfirmSecondCode = Convert.ToString(Rjnos);

                        IVR.Report_No = Convert.ToString("IRN-" + IVR.SubJob_No + "-" + SrNo);
                        IVR.Status = "1";
                        IVR.Type = "IRN";
                        string id = OBJDALIRN.InsertUpdateInspectionvisit(IVR);
                        IVR.PK_IVR_ID = Convert.ToInt32(id);
                        if (id != "" && id != null)
                        {
                            TempData["InsertCompany"] = Result;
                            // return View(IVR);
                        }
                        IRNID = Convert.ToInt32(IVR.PK_IVR_ID);
                        if (IRNID != null && IRNID != 0)
                        {
                            if (fileDetails != null && fileDetails.Count > 0)
                            {
                                Result = OBJDALIRN.InsertFileAttachment(fileDetails, IRNID);
                                Session["listSJUploadedFile"] = null;
                            }
                        }

                        #region  item Description

                        ItemDescriptionDashBoard = OBJDALIRN.GetitemDescription(IVR.PK_IVR_ID);
                        if (ItemDescriptionDashBoard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in ItemDescriptionDashBoard.Rows)
                            {
                                var ItemDescription = new ItemDescriptionModel
                                {
                                    PK_ItemD_Id = Convert.ToInt32(dr["PK_ItemD_Id"]),
                                    Po_Item_No = Convert.ToString(dr["Po_Item_No"]),
                                    ItemCode_Description = Convert.ToString(dr["ItemCode_Description"]),
                                    Po_Quantity = Convert.ToString(dr["Po_Quantity"]),
                                    Offered_Quantity = Convert.ToString(dr["Offered_Quantity"]),
                                    PK_IVR_ID = IVR.PK_IVR_ID,
                                    Item_Code = Convert.ToString(dr["Item_Code"]),
                                    Accepted_Quantity = Convert.ToString(dr["Accepted_Quantity"]),
                                    Cumulative_Accepted_Qty = Convert.ToString(dr["Cumulative_Accepted_Qty"]),
                                    Unit = Convert.ToString(dr["Unit"]),
                                    Status = "1",
                                    Type = "IRN",

                                };
                                Result = OBJDALIRN.InsertItemDescription(ItemDescription);
                            }
                        }
                        #endregion

                        #region Reference Documents

                        RefranceDocumentsDashBoard = OBJDALIRN.GetReferenceDocuments(IVR.PK_IVR_ID);
                        if (RefranceDocumentsDashBoard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in RefranceDocumentsDashBoard.Rows)
                            {
                                var ItemDescription = new ReferenceDocumentsModel
                                {
                                    Document_No = Convert.ToString(dr["Document_No"]),
                                    Document_Name = Convert.ToString(dr["Document_Name"]),
                                    Approval_Status = Convert.ToString(dr["Approval_Status"]),
                                    PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                                    VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"]),
                                    PK_IVR_ID = IVR.PK_IVR_ID,
                                    Type = "IRN",
                                    Status = "1",

                                };
                                Result = OBJDALIRN.InsertReferenceDocuments(ItemDescription);
                            }
                        }
                        #endregion

                        #region Inspection Activities
                        InspectionActivitesDashBoard = OBJDALIRN.GetInspectionActivities(IVR.PK_IVR_ID);
                        if (InspectionActivitesDashBoard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in InspectionActivitesDashBoard.Rows)
                            {
                                var InspectionActivities = new InspectionActivitiesModel
                                {
                                    Stages_Witnessed = Convert.ToString(dr["Stages_Witnessed"]),
                                    PK_IA_ID = Convert.ToInt32(dr["PK_IA_ID"]),
                                    PK_IVR_ID = IVR.PK_IVR_ID,
                                    Type = "IRN",
                                    Status = "1",

                                };
                                Result = OBJDALIRN.InsertInspectionActivities(InspectionActivities);
                            }
                        }
                        #endregion

                        #region Documents Review
                        DocumentsReviewBoard = OBJDALIRN.GetDocumentRevieweModelByCall_Id(Convert.ToInt32(IVR.PK_IVR_ID));
                        if (DocumentsReviewBoard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in DocumentsReviewBoard.Rows)
                            {
                                var DocumentsReview = new DocumentRevieweModel
                                {
                                    Description = Convert.ToString(dr["Description"]),
                                    PK_DR_ID = Convert.ToInt32(dr["PK_DR_ID"]),
                                    PK_IVR_ID = IVR.PK_IVR_ID,
                                    Type = "IRN",
                                    Status = "1",
                                };
                                Result = OBJDALIRN.InsertDocumentReviewe(DocumentsReview);

                            }
                        }
                        #endregion

                        #region Generate PDF of IRN on Insert
                        if (IVR.PK_IVR_ID != 0)
                        {
                            DataTable ItemDescriptionDashBoard1 = new DataTable();
                            DataTable RefranceDocumentsDashBoard1 = new DataTable();
                            DataTable InspectionActivitesDashBoard1 = new DataTable();
                            DataTable DocumentsReviewBoard1 = new DataTable();
                            DataTable EquipmentDetailsBoard1 = new DataTable();
                            DataSet DSJobMasterByQtId1 = new DataSet();
                            DataTable ReportDashBoard1 = new DataTable();
                            DataTable CostSheetDashBoard1 = new DataTable();
                            int count = 0;
                            DataTable ImageReportDashBoard1 = new DataTable();
                            List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();
                            List<ItemDescriptionModel> lstCompanyDashBoard1 = new List<ItemDescriptionModel>();
                            List<ReferenceDocumentsModel> RefranceDocuments1 = new List<ReferenceDocumentsModel>();
                            List<InspectionActivitiesModel> InspectionDocuments1 = new List<InspectionActivitiesModel>();
                            List<DocumentRevieweModel> DocumentReview1 = new List<DocumentRevieweModel>();
                            List<EquipmentDetailsModel> EquipmentDetails1 = new List<EquipmentDetailsModel>();
                            List<ReportModel> ReportDashboard1 = new List<ReportModel>();


                            ReportModel RM = new ReportModel();
                            string Result1 = "";
                            if (IVR.PK_IVR_ID != 0)
                            {
                                int i = 0;
                                int J = 0;
                                int K = 0;
                                int L = 0;
                                int M = 0;
                                int N = 0;
                                #region 
                                DSJobMasterByQtId1 = OBJDALIRN.EditInspectionVisitReportByPKCallID(IVR.PK_IVR_ID);

                                if (DSJobMasterByQtId1.Tables[0].Rows.Count > 0)
                                {
                                    ObjModelVisitReport.Branch = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Branch"]);
                                    ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sap_And_Controle_No"]);
                                    ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["PK_IVR_ID"]);
                                    ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Project_Name_Location"]);
                                    ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Address_Of_Inspection"]);
                                    ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["End_user_Name"]);
                                    ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Vendor_Name_Location"]);
                                    // ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["PK_Call_ID"]);
                                    ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Notification_Name_No_Date"]);
                                    ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Date_Of_Inspection"]);
                                    ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Client_Name"]);
                                    ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
                                    ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
                                    ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Po_No"]);
                                    ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sub_Vendor_Name"]);
                                    ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Po_No_SubVendor"]);

                                    //int kick = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Kick_Off_Pre_Inspection"]);
                                    //ObjModelVisitReport.Kick_Off_Pre_Inspection = Convert.ToBoolean(kick);

                                    //int Mi = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Material_identification"]);
                                    //ObjModelVisitReport.Material_identification = Convert.ToBoolean(Mi);

                                    //int Interim_Stages = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Interim_Stages"]);
                                    //ObjModelVisitReport.Interim_Stages = Convert.ToBoolean(Interim_Stages);

                                    //int Document_review = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Document_review"]);
                                    //ObjModelVisitReport.Document_review = Convert.ToBoolean(Document_review);

                                    //int Final_Inspection = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Final_Inspection"]);
                                    //ObjModelVisitReport.Final_Inspection = Convert.ToBoolean(Final_Inspection);

                                    //int Re_inspection = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Re_inspection"]);
                                    //ObjModelVisitReport.Re_inspection = Convert.ToBoolean(Re_inspection);

                                    ObjModelVisitReport.Conclusion = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Conclusion"]);
                                    ObjModelVisitReport.Waivers = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Waivers"]);

                                    int dvmdr = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["DVendor"]);
                                    ObjModelVisitReport.vendot = Convert.ToBoolean(dvmdr);
                                    int dclnt = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["DClient"]);
                                    ObjModelVisitReport.client = Convert.ToBoolean(dclnt);
                                    int dtuv = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["DTUV"]);
                                    ObjModelVisitReport.TUV = Convert.ToBoolean(dtuv);

                                    ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Pending_Activites"]);
                                    ObjModelVisitReport.Identification_Of_Inspected = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Identification_Of_Inspected"]);
                                    ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Areas_Of_Concerns"]);
                                    ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Non_Conformities_raised"]);
                                    //ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Signatures"]);
                                    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Type"]);
                                    ObjModelVisitReport.Report_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Report_No"]);
                                    // ObjModelVisitReport.Call_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Call_No"]);
                                    ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Signature"]);
                                    ObjModelVisitReport.Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["LastName"]);
                                    ObjModelVisitReport.ReportCreatedDate = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["CreatedDate"]);

                                    int Inspection_records = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Inspection_records"]);
                                    ObjModelVisitReport.Inspection_records = Convert.ToBoolean(Inspection_records);

                                    int Inspection_Photo = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Inspection_Photo"]);
                                    ObjModelVisitReport.Inspection_Photo = Convert.ToBoolean(Inspection_Photo);

                                    int Other_Specify = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Other_Specify"]);
                                    ObjModelVisitReport.Other_Specify = Convert.ToBoolean(Other_Specify);
                                    ObjModelVisitReport.ReviseReason = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["ReviseReason"]);

                                    ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["SubJob_No"]);
                                    ObjModelVisitReport.Sub_Order_Status = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sub_Order_Status"]);
                                    ObjModelVisitReport.OrderStatus = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["OrderStatus"]);

                                    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Type"]);

                                }
                                #endregion

                                #region  item Description

                                ItemDescriptionDashBoard1 = OBJDALIRN.GetitemDescription(IVR.PK_IVR_ID);
                                if (ItemDescriptionDashBoard1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ItemDescriptionDashBoard1.Rows)
                                    {
                                        lstCompanyDashBoard1.Add(
                                            new ItemDescriptionModel
                                            {
                                                PK_ItemD_Id = Convert.ToInt32(dr["PK_ItemD_Id"]),
                                                Po_Item_No = Convert.ToString(dr["Po_Item_No"]),
                                                ItemCode_Description = Convert.ToString(dr["ItemCode_Description"]),
                                                Po_Quantity = Convert.ToString(dr["Po_Quantity"]),
                                                Offered_Quantity = Convert.ToString(dr["Offered_Quantity"]),
                                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                                Item_Code = Convert.ToString(dr["Item_Code"]),
                                                Accepted_Quantity = Convert.ToString(dr["Accepted_Quantity"]),
                                                Cumulative_Accepted_Qty = Convert.ToString(dr["Cumulative_Accepted_Qty"]),
                                                Unit = Convert.ToString(dr["Unit"]),
                                            }
                                            );
                                    }
                                }
                                #endregion

                                #region Reference Documents

                                RefranceDocumentsDashBoard1 = OBJDALIRN.GetReferenceDocuments(IVR.PK_IVR_ID);
                                if (RefranceDocumentsDashBoard1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in RefranceDocumentsDashBoard1.Rows)
                                    {
                                        RefranceDocuments1.Add(
                                            new ReferenceDocumentsModel
                                            {
                                                Document_No = Convert.ToString(dr["Document_No"]),
                                                Document_Name = Convert.ToString(dr["Document_Name"]),
                                                Approval_Status = Convert.ToString(dr["Approval_Status"]),
                                                PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                                                VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"]),
                                                // PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])

                                            }
                                            );
                                    }
                                }
                                #endregion

                                #region Inspection Activities
                                InspectionActivitesDashBoard1 = OBJDALIRN.GetInspectionActivities(IVR.PK_IVR_ID);
                                if (InspectionActivitesDashBoard1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in InspectionActivitesDashBoard1.Rows)
                                    {
                                        InspectionDocuments1.Add(
                                            new InspectionActivitiesModel
                                            {
                                                Stages_Witnessed = Convert.ToString(dr["Stages_Witnessed"]),
                                                PK_IA_ID = Convert.ToInt32(dr["PK_IA_ID"]),
                                                //  PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])

                                            }
                                        );
                                    }
                                }
                                #endregion

                                #region Documents Review
                                DocumentsReviewBoard1 = OBJDALIRN.GetDocumentRevieweModelByCall_Id(Convert.ToInt32(IVR.PK_IVR_ID));
                                if (DocumentsReviewBoard1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in DocumentsReviewBoard1.Rows)
                                    {
                                        DocumentReview1.Add(
                                            new DocumentRevieweModel
                                            {
                                                Description = Convert.ToString(dr["Description"]),
                                                PK_DR_ID = Convert.ToInt32(dr["PK_DR_ID"]),
                                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
                                            }
                                        );
                                    }
                                }
                                #endregion

                                #region report Count
                                ReportDashBoard1 = OBJDALIRN.GetReportByCall_Id(IVR.PK_IVR_ID);
                                if (ReportDashBoard1.Rows.Count > 0)
                                {
                                    count = ReportDashBoard1.Rows.Count;
                                }
                                string countNo = Convert.ToString(count);
                                #endregion

                                #region Save to Pdf Code 

                                #region Get Date Of Inspection & Inspector Name

                                DataSet dsIN = new DataSet();
                                int z = 0;
                                string InspectorName = "";
                                string Adate = string.Empty;
                                string Date = "";
                                string InrName = string.Empty;

                                List<string> lstInspector = new List<string>();
                                List<string> lstDates = new List<string>();

                                string InsList = string.Empty;
                                dsIN = OBJDALIRN.GetInspectionName(Convert.ToInt32(IVR.PK_IVR_ID));
                                if (dsIN.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dsIN.Tables[0].Rows)
                                    {
                                        InspectorName = Convert.ToString(dr["InspectorName"]);
                                        lstInspector.Add(InspectorName);
                                    }
                                    InsList = "On behalf of " + string.Join(",", lstInspector);
                                }
                                else
                                {
                                    InsList = "";
                                }
                                if (dsIN.Tables[1].Rows.Count > 0)
                                {
                                    InrName = dsIN.Tables[1].Rows[0]["InspectorName"].ToString();
                                }
                                else
                                {
                                    InrName = "";
                                }
                                if (dsIN.Tables[2].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dsIN.Tables[2].Rows)
                                    {
                                        Adate = Convert.ToString(dr["Actual_Visit_Date"]);
                                        lstDates.Add(Adate);
                                    }
                                    Date = string.Join(",", lstDates);
                                }
                                else
                                {
                                    Date = "";
                                }
                                #endregion
                                SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
                                System.Text.StringBuilder strs = new System.Text.StringBuilder();
                                string body = string.Empty;

                                string ItemDescriptioncontent = "";
                                string ReferenceDocumentscontent = "";
                                string InspectionDocumentsContent = "";
                                string DocumentreviewContent = "";
                                //string EquipmentDetailscontent = "";

                                string check1 = "";
                                string check2 = "";
                                string dvenr12 = string.Empty;
                                string dclnt12 = string.Empty;
                                string dtuv12 = string.Empty;

                                using (StreamReader reader = new StreamReader(Server.MapPath("~/inspection-certificate.html")))
                                {
                                    body = reader.ReadToEnd();
                                }

                                body = body.Replace("[SapAndControle_No]", ObjModelVisitReport.SubJob_No);
                                body = body.Replace("[Branch]", ObjModelVisitReport.Branch);
                                body = body.Replace("[NotificationNameNo_Date]", ObjModelVisitReport.Notification_Name_No_Date);
                                // body = body.Replace("[DateOfInspection]", ObjModelVisitReport.Date_Of_Inspection);
                                body = body.Replace("[DateOfInspection]", Date);
                                body = body.Replace("[InspectorNames]", InsList);
                                //body = body.Replace("[ReviseReason]", ObjModelVisitReport.ReviseReason);
                                if (ObjModelVisitReport.ReviseReason != "-" && ObjModelVisitReport.ReviseReason != "")
                                {
                                    body = body.Replace("[ReviseReason]", "Reason for Revise- " + Convert.ToString(ObjModelVisitReport.ReviseReason));
                                }
                                else
                                {
                                    body = body.Replace("[ReviseReason]", "-");
                                }
                                body = body.Replace("[ProjectNameLocation]", ObjModelVisitReport.Project_Name_Location);
                                body = body.Replace("[AddressOfInspection]", ObjModelVisitReport.Address_Of_Inspection);
                                body = body.Replace("[ClientName]", ObjModelVisitReport.Client_Name);
                                body = body.Replace("[Enduser_Name]", ObjModelVisitReport.End_user_Name);
                                body = body.Replace("[DECPMCEPC_Name]", ObjModelVisitReport.DEC_PMC_EPC_Name);
                                body = body.Replace("[DECPMCEPCAssignment_No]", ObjModelVisitReport.DEC_PMC_EPC_Assignment_No);
                                body = body.Replace("[VendorNameLocation]", ObjModelVisitReport.Vendor_Name_Location);
                                body = body.Replace("[PoNo]", ObjModelVisitReport.Po_No);
                                body = body.Replace("[SubVendorName]", ObjModelVisitReport.Sub_Vendor_Name);
                                body = body.Replace("[PoNoSubVendor]", ObjModelVisitReport.Po_No_SubVendor);
                                body = body.Replace("[Conclusion]", ObjModelVisitReport.Conclusion);
                                body = body.Replace("[PendingActivites]", ObjModelVisitReport.Pending_Activites);
                                body = body.Replace("[IdentificationOfInspected]", ObjModelVisitReport.Identification_Of_Inspected);
                                body = body.Replace("[AreasOfConcerns]", ObjModelVisitReport.Areas_Of_Concerns);
                                body = body.Replace("[NonConformitiesraised]", ObjModelVisitReport.Non_Conformities_raised);
                                // body = body.Replace("[Name]", ObjModelVisitReport.Name);
                                body = body.Replace("[Name]", InrName);

                                body = body.Replace("[date]", ObjModelVisitReport.ReportCreatedDate);

                                body = body.Replace("[Waivers]", ObjModelVisitReport.Waivers);

                                body = body.Replace("[RevisionNo]", IVR.Report_No + "- Rev 0");
                                body = body.Replace("[PreviousNo]", "-");
                                body = body.Replace("[RevNo]", "-");

                                if (ObjModelVisitReport.vendot == true)
                                {
                                    dvenr12 = "<span><input type='checkbox' checked> TUVI Client / End User</span>";
                                }
                                else
                                {
                                    dvenr12 = "<span><input type='checkbox'> TUVI Client / End User</span>";
                                }
                                if (ObjModelVisitReport.client == true)
                                {
                                    dclnt12 = "<span><input type='checkbox' checked> TUV Executing Branch / TUV Originating Branch</span>";
                                }
                                else
                                {
                                    dclnt12 = "<span><input type='checkbox'> TUV Executing Branch / TUV Originating Branch</span>";
                                }
                                if (ObjModelVisitReport.TUV == true)
                                {
                                    dtuv12 = "<span><input type='checkbox' checked> Vendor / Sub Vendor</span>";
                                }
                                else
                                {
                                    dtuv12 = "<span><input type='checkbox'> Vendor / Sub Vendor</span>";
                                }

                                if (ObjModelVisitReport.OrderStatus == "Complete")
                                {
                                    //check1 = "<td width='5%' align='center' style='border-right-width: 0px;'><span><input type='checkbox'  value=" + ObjModelVisitReport.Kick_Off_Pre_Inspection + " checked></span></td>";
                                    check1 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox'>Incomplete</label></span></td>";
                                }
                                else
                                {
                                    //check1 = "<td width='5%' align='center' style='border-right-width: 0px;'><span><input type='checkbox'  value=" + ObjModelVisitReport.Kick_Off_Pre_Inspection + "></span></td>";
                                    check1 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Incomplete</label></span></td>";
                                }
                                if (ObjModelVisitReport.Sub_Order_Status == "Complete")
                                {
                                    check2 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox'>Incomplete</label></span></td>";
                                }
                                else
                                {
                                    check2 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Incomplete</label></span></td>";
                                }
                                //foreach (ItemDescriptionModel v in lstCompanyDashBoard1)
                                //{
                                //    i = i + 1;
                                //    ItemDescriptioncontent += "<tr><td style='border: 1px solid #000000;' width='8%' align='center'><span>" + Convert.ToString(v.Po_Item_No) + ')' + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Item_Code + " </span></td><td style='border: 1px solid #b5b5b5;' width='35%'><span>" + v.ItemCode_Description + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Unit + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Po_Quantity + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Offered_Quantity + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Accepted_Quantity + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Cumulative_Accepted_Qty + "</span></td></tr>";
                                //}

                                //foreach (ReferenceDocumentsModel v in RefranceDocuments1)
                                //{
                                //    J = J + 1;
                                //    ReferenceDocumentscontent += "<tr><td style='border: 1px solid #000000;' width='8%' align='center'> " + J +')'+ " </td><td style='border: 1px solid #b5b5b5;' width='25%'>" + Convert.ToString(v.Document_Name) + "</td><td style='border: 1px solid #000000;' width='25%'>" + v.Document_No + " </td><td style='border: 1px solid #b5b5b5;' width='25%'>" + v.VendorDocumentNumber + "</td><td style='border: 1px solid #000000;' width='25%'>" + v.Approval_Status + "</td></tr>";
                                //}
                                foreach (ItemDescriptionModel v in lstCompanyDashBoard1)
                                {
                                    i = i + 1;

                                    if (i == lstCompanyDashBoard1.Count)
                                        ItemDescriptioncontent += "<tr><td style = 'border:1px solid #000000;' width = '5%' align = 'center' ><span> " + Convert.ToString(v.Po_Item_No) + ')' + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Item_Code + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '35%' ><span> " + v.ItemCode_Description + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Unit + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Po_Quantity + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Offered_Quantity + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Accepted_Quantity + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;' width = '10%' ><span> " + v.Cumulative_Accepted_Qty + " </span></td></tr>";
                                    else
                                        ItemDescriptioncontent += "<tr><td style = 'border:1px solid #000000;border-bottom-width: 0px;' width = '5%' align = 'center' ><span> " + Convert.ToString(v.Po_Item_No) + ')' + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Item_Code + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '35%' ><span> " + v.ItemCode_Description + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Unit + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Po_Quantity + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Offered_Quantity + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Accepted_Quantity + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;' width = '10%' ><span> " + v.Cumulative_Accepted_Qty + " </span></td></tr>";
                                }

                                foreach (ReferenceDocumentsModel v in RefranceDocuments1)
                                {
                                    J = J + 1;
                                    if (J == RefranceDocuments1.Count)
                                        ReferenceDocumentscontent += "<tr><td style = 'border:1px solid #000000;vertical-align:top; text-align:center;'  " + J + ')' + " </td><td width = '25%' style = 'border:1px solid #000000;border-left-width: 0px; white-space: pre-line;vertical-align:top;' > " + Convert.ToString(v.Document_Name) + " </td><td width = '25%' style = 'border:1px solid #000000;border-left-width: 0px;white-space:pre-line;vertical-align:top;' > " + v.Document_No + " </td><td width = '25%' style = 'border:1px solid #000000;border-left-width: 0px;white-space:pre-line;vertical-align:top;' > " + v.VendorDocumentNumber + " </td><td width = '25%' style = 'border:1px solid #000000;border-left-width: 0px;white-space: pre-line;vertical-align:top;' > " + v.Approval_Status + " </td></tr> ";
                                    else
                                        ReferenceDocumentscontent += "<tr><td style = 'border:1px solid #000000;border-bottom-width: 0px;vertical-align:top; text-align:center;'  " + J + ')' + " </td><td width = '25%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px; white-space: pre-line;vertical-align:top;' > " + Convert.ToString(v.Document_Name) + " </td><td width = '25%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;white-space:pre-line;vertical-align:top;' > " + v.Document_No + " </td><td width = '25%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;white-space:pre-line;vertical-align:top;' > " + v.VendorDocumentNumber + " </td><td width = '25%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;white-space: pre-line;vertical-align:top;' > " + v.Approval_Status + " </td></tr> ";
                                }


                                foreach (InspectionActivitiesModel v in InspectionDocuments1)
                                {
                                    K = K + 1;
                                    InspectionDocumentsContent += "<tr><td width='5%' align='center'><span> " + K + ')' + " </span></td><td width='95%'><span>" + Convert.ToString(v.Stages_Witnessed) + "</span></td></tr>";
                                }
                                foreach (DocumentRevieweModel v in DocumentReview1)
                                {
                                    L = L + 1;
                                    DocumentreviewContent += "<tr><td width='5%' align='center'><span> " + L + ')' + " </span></td><td width='95%' ><span>" + Convert.ToString(v.Description) + "</span></td></tr>";
                                }
                                body = body.Replace("[ItemDescriptionContent]", ItemDescriptioncontent);
                                body = body.Replace("[ReferenceDocumentsContent]", ReferenceDocumentscontent);
                                body = body.Replace("[InspectionDocumentsContent]", InspectionDocumentsContent);
                                body = body.Replace("[DocumentreviewContent]", DocumentreviewContent);
                                body = body.Replace("[Stamp]", "https://tiimes.tuv-india.com/Stamp.png");

                                if (ObjModelVisitReport.Signatures != null)
                                {
                                    body = body.Replace("[Signature]", "https://tiimes.tuv-india.com/Content/Sign/" + ObjModelVisitReport.Signatures + "");
                                }
                                else
                                {
                                    body = body.Replace("[Signature]", "-");
                                }
                                body = body.Replace("[Checkbox1]", check1);
                                body = body.Replace("[Checkbox2]", check2);
                                body = body.Replace("[DVendor]", dvenr12);
                                body = body.Replace("[DClient]", dclnt12);
                                body = body.Replace("[DTUV]", dtuv12);
                                strs.Append(body);
                                PdfPageSize pageSize = PdfPageSize.A4;
                                PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
                                HtmlToPdf converter = new HtmlToPdf();
                                converter.Options.MaxPageLoadTime = 120;  //=========================5-Aug-2019
                                converter.Options.PdfPageSize = pageSize;
                                converter.Options.PdfPageOrientation = pdfOrientation;

                                string _Header = string.Empty;
                                string _footer = string.Empty;

                                // for Report header by abel
                                StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/Inspection_Certificate_Header.html"));
                                _Header = _readHeader_File.ReadToEnd();

                                _Header = _Header.Replace("[RevisionNo]", IVR.Report_No + "- Rev 0");
                                _Header = _Header.Replace("[Logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png"); // change123 once pulished on server

                                StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/inspection-certificate-footer.html")); // Footer is used from IVR as it same and commented in pdf template.
                                _footer = _readFooter_File.ReadToEnd();

                                // header settings
                                converter.Options.DisplayHeader = true ||
                                    true || true;
                                converter.Header.DisplayOnFirstPage = true;
                                converter.Header.DisplayOnOddPages = true;
                                converter.Header.DisplayOnEvenPages = true;
                                converter.Header.Height = 75;

                                PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
                                headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                                converter.Header.Add(headerHtml);

                                // footer settings
                                converter.Options.DisplayFooter = true ||
                                    true || true;
                                converter.Footer.DisplayOnFirstPage = true;
                                converter.Footer.DisplayOnOddPages = true;
                                converter.Footer.DisplayOnEvenPages = true;
                                converter.Footer.Height = 130;

                                PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
                                footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                                converter.Footer.Add(footerHtml);

                                //end abel code

                                #region Footer Code
                                // page numbers can be added using a PdfTextSection object
                                PdfTextSection text = new PdfTextSection(0, 90, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
                                text.HorizontalAlign = PdfTextHorizontalAlign.Right;
                                converter.Footer.Add(text);
                                #endregion


                                PdfDocument doc = converter.ConvertHtmlString(body);
                                string ReportName = IVR.Report_No + "- Rev0"; // + "/" + count
                                string Report = IVR.Report_No + "- Rev0" + ".pdf"; // "/" + count +
                                string path = Server.MapPath("~/IRNReports");
                                doc.Save(path + '\\' + Report);
                                doc.Close();

                                #endregion
                                if (RM.PK_RM_ID == 0)
                                {
                                    RM.Type = "IRN";
                                    RM.Status = "1";
                                    //RM.ImageReport = ReportNames;
                                    RM.Report = Report;
                                    RM.ReportName = ReportName;
                                    RM.PK_CALL_ID = IVR.PK_IVR_ID;
                                    RM.VendorName = ObjModelVisitReport.Vendor_Name_Location;
                                    RM.ClientName = ObjModelVisitReport.Client_Name;
                                    RM.ProjectName = ObjModelVisitReport.Project_Name_Location;
                                    RM.SubJob_No = ObjModelVisitReport.SubJob_No;
                                    Result1 = OBJDALIRN.InsertUpdateReport(RM);
                                    if (Result1 != "" && Result1 != null)
                                    {
                                        TempData["InsertCompany"] = Result1;
                                    }
                                }
                                IVR.abcid = Convert.ToInt32(Session["GetReportId"]);
                                #region
                                CostSheetDashBoard1 = OBJDALIRN.GetReportByCall_Id(IVR.PK_IVR_ID);
                                if (CostSheetDashBoard1.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in CostSheetDashBoard1.Rows)
                                    {
                                        ReportDashboard1.Add(
                                            new ReportModel
                                            {
                                                ReportName = Convert.ToString(dr["ReportName"]),
                                                Report = Convert.ToString(dr["Report"]),
                                                CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                                PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                                // PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
                                            }
                                            );
                                    }
                                }
                                ViewData["CostSheet"] = ReportDashboard1;
                                #endregion
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region  Old Code 

                    if (IVR.PK_IVR_ID == 0)
                    {
                        #region File Upload Code 


                        //HttpPostedFileBase Imagesection;
                        //if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                        //{
                        //    Imagesection = Request.Files["img_Banner"];
                        //    if (Imagesection != null && Imagesection.FileName != "")
                        //    {
                        //        IVR.Signatures = CommonControl.FileUpload("Content/JobDocument/", Imagesection);
                        //    }
                        //    else
                        //    {
                        //        if (Imagesection.FileName != "")
                        //        {
                        //            IVR.Signatures = "NoImage.gif";
                        //        }
                        //    }
                        //}


                        #endregion

                        int _min = 10000;
                        int _max = 99999;
                        Random _rdm = new Random();
                        int Rjno = _rdm.Next(_min, _max);
                        string ConfirmCode = Convert.ToString(Rjno);

                        int _mins = 100000;
                        int _maxs = 999999;
                        Random _rdms = new Random();
                        int Rjnos = _rdm.Next(_mins, _maxs);
                        string ConfirmSecondCode = Convert.ToString(Rjnos);

                        IVR.Report_No = Convert.ToString(ConfirmCode) + ConfirmSecondCode + "-IRN";
                        IVR.Status = "1";
                        IVR.Type = "IRN";
                        string id = OBJDALIRN.InsertUpdateInspectionvisit(IVR);
                        IVR.PK_IVR_ID = Convert.ToInt32(id);
                        if (id != "" && id != null)
                        {
                            TempData["InsertCompany"] = Result;
                            // return View(IVR);
                        }

                    }
                    else
                    {
                        #region File Upload Code 


                        HttpPostedFileBase Imagesection;
                        if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                        {
                            Imagesection = Request.Files["img_Banner"];
                            if (Imagesection != null && Imagesection.FileName != "")
                            {
                                IVR.Signatures = CommonControl.FileUpload("Content/JobDocument/", Imagesection);
                            }
                            else
                            {
                                if (Imagesection.FileName != "")
                                {
                                    IVR.Signatures = "NoImage.gif";
                                }
                            }
                        }


                        #endregion

                        Result = OBJDALIRN.InsertUpdateInspectionvisit(IVR);
                        if (Result != null && Result != "")
                        {
                            //IVR.arrey = null;
                            TempData["UpdateCompany"] = Result;
                            //  return View(IVR);
                        }

                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = Error });
            }
            return RedirectToAction("IRNForm", "InspectionReleaseNote", new { @PK_IVR_ID = IVR.PK_IVR_ID, @PK_RM_ID = IVR.abcid });
        }


        [HttpGet]
        public ActionResult Searchdata(string SubJobNo, string PoNo)
        {
            string SubJob = string.Empty;
            if (SubJobNo != null)
            {
                int VarLength = Regex.Matches(SubJobNo, "/").Count;
                if (VarLength > 1)
                {
                    SubJob = SubJobNo.Substring(0, SubJobNo.LastIndexOf("/") + 0);
                }
                else
                {
                    SubJob = SubJobNo;
                }
            }
            DataTable Reportdashboard = new DataTable();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();
            try
            {
                Reportdashboard = OBJDALIRN.GetReportListBySubjobno(SubJobNo, PoNo);

                if (Reportdashboard.Rows.Count > 0)
                {
                    foreach (DataRow dr in Reportdashboard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReportModel
                            {
                                ReportName = Convert.ToString(dr["ReportNo"]),
                                Report = Convert.ToString(dr["Report"]),
                                CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                ProjectName = Convert.ToString(dr["ProjectName"]),
                                inspectionDate = Convert.ToString(dr["inspectionDate"])

                            }
                          );
                    }
                }
                return Json(lstCompanyDashBoard, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region
        [HttpPost]
        public JsonResult GetdataCopy(string SubJobNo, string PoNo)
        {

            DataSet DSJobMasterByQtId = new DataSet();

            if (SubJobNo != "")
            {
                DSJobMasterByQtId = OBJDALIRN.GetSubjobDetails(SubJobNo);
            }
            else
            {
                DSJobMasterByQtId = OBJDALIRN.GetSubjobDetailsByPo(PoNo);
            }

            if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
            {

                ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Control_Number"]);

                ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name"]);
                ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Client_Contact"]);
                ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["EndUser"]);
                ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["V1"]);
                // ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_Call_ID"]);
                // ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);

                ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Company_Name"]);

                ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["P1"]);
                ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["V2"]);
                ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubJob_No"]);
                ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["P2"]);


                ObjModelVisitReport.client_Email = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["client_Email"]);
                ObjModelVisitReport.Vendor_Email = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Email"]);
                ObjModelVisitReport.Tuv_Branch = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Tuv_Email"]);
                ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
                ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
                ObjModelVisitReport.Date_of_PO = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date_of_PO"]);
                ObjModelVisitReport.SubSubVendorDate_of_PO = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubSubVendorDate_of_PO"]);

            }

            return Json(ObjModelVisitReport, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Conclusion

        // GET: VisitReport
        //[HttpGet]
        //[ValidateInput(false)]
        //public ActionResult Conclusion(int? PK_IVR_ID, int? PK_RM_ID)
        //{
        //    if (Convert.ToInt32(PK_RM_ID) != 0)
        //    {
        //        ObjModelVisitReport.defid = Convert.ToInt32(PK_RM_ID);
        //        ObjModelVisitReport.abcid = Convert.ToInt32(PK_RM_ID);
        //    }
        //    if (Convert.ToInt32(PK_IVR_ID) != 0)
        //    {
        //        ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(PK_IVR_ID);
        //    }
        //    if (ObjModelVisitReport.PK_IVR_ID != 0)
        //    {
        //        DataTable DTGetUploadedFile = new DataTable();
        //        List<FileDetails> lstEditFileDetails = new List<FileDetails>();
        //        DTGetUploadedFile = OBJDALIRN.EditConUploadedFile(Convert.ToInt32(ObjModelVisitReport.PK_IVR_ID));
        //        if (DTGetUploadedFile.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in DTGetUploadedFile.Rows)
        //            {
        //                lstEditFileDetails.Add(
        //                   new FileDetails
        //                   {
        //                       PK_ID = Convert.ToInt32(dr["PK_ID"]),
        //                       FileName = Convert.ToString(dr["FileName"]),
        //                       Extension = Convert.ToString(dr["Extenstion"]),
        //                       IDS = Convert.ToString(dr["FileID"]),
        //                   }
        //                 );
        //            }
        //            ViewData["lstEditFileDetails"] = lstEditFileDetails;
        //            ObjModelVisitReport.FileDetails = lstEditFileDetails;
        //        }
        //        DataSet DSJobMasterByQtId = new DataSet();
        //        DataSet dsDownloadPrint = new DataSet();

        //        dsDownloadPrint = OBJDALIRN.DownloadPrint(Convert.ToInt32(ObjModelVisitReport.PK_IVR_ID));
        //        if (dsDownloadPrint.Tables[0].Rows.Count > 0)
        //        {
        //            ObjModelVisitReport.DownloadPrint = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["Report"]);
        //            ObjModelVisitReport.ReportName = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["ReportName"]);
        //        }

        //        DSJobMasterByQtId = OBJDALIRN.EditInspectionVisitReportByPKCallID(ObjModelVisitReport.PK_IVR_ID);

        //        if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
        //        {
        //            ViewBag.Data = string.Empty;
        //            ObjModelVisitReport.Branch = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Branch"]);
        //            ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubJob_No"]);
        //            ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sap_And_Controle_No"]);
        //            ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_IVR_ID"]);
        //            ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name_Location"]);
        //            ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Address_Of_Inspection"]);
        //            ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["End_user_Name"]);
        //            ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Name_Location"]);
        //            //ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_Call_ID"]);
        //            ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);
        //            ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date_Of_Inspection"]);
        //            ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Client_Name"]);
        //            ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
        //            ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
        //            ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No"]);
        //            ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Vendor_Name"]);
        //            ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No_SubVendor"]);

        //            ObjModelVisitReport.OrderStatus = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["OrderStatus"]);
        //            ObjModelVisitReport.Sub_Order_Status = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Order_Status"]);

        //            int dvmdr = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["DVendor"]);
        //            ObjModelVisitReport.vendot = Convert.ToBoolean(dvmdr);
        //            int dclnt = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["DClient"]);
        //            ObjModelVisitReport.client = Convert.ToBoolean(dclnt);
        //            int dtuv = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["DTUV"]);
        //            ObjModelVisitReport.TUV = Convert.ToBoolean(dtuv);

        //            ObjModelVisitReport.Conclusion = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Conclusion"]);                   
        //            ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Pending_Activites"]);
        //            ObjModelVisitReport.Identification_Of_Inspected = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Identification_Of_Inspected"]);
        //            ObjModelVisitReport.GetDataAfterSave= Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Identification_Of_Inspected"]);
        //            ViewBag.Data = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Identification_Of_Inspected"]);
        //            ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Areas_Of_Concerns"]);
        //            ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Non_Conformities_raised"]);
        //            ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Signature"]);
        //            ObjModelVisitReport.Waivers = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Waivers"]);
        //            int POTotalChk = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["POTotalCheckBox"]);
        //            ObjModelVisitReport.POTotalCheckBox = Convert.ToBoolean(POTotalChk);
        //        }
        //        return View(ObjModelVisitReport);
        //    }
        //    else
        //    {
        //        return View(ObjModelVisitReport);
        //    }
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Conclusion(InspectionvisitReportModel IVR, HttpPostedFileBase File, FormCollection fc, int? cid)
        //{
        //    #region Added by Ankush
        //    int IRNID = 0;
        //    List<FileDetails> lstFileDtls = new List<FileDetails>();
        //    lstFileDtls = Session["listNCRUploadedFile"] as List<FileDetails>;
        //    #endregion

        //    //IVR.Identification_Of_Inspected = Convert.ToString(fc["textcontent"]);
        //    //IVR.Identification_Of_Inspected = Convert.ToString(fc["Identification_Of_Inspected"]);

        //    string Result = string.Empty;
        //    try
        //    {
        //        if (IVR.PK_IVR_ID == 0)
        //        {
        //            #region File Upload Code 

        //            HttpPostedFileBase Imagesection;
        //            if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
        //            {
        //                Imagesection = Request.Files["img_Banner"];
        //                if (Imagesection != null && Imagesection.FileName != "")
        //                {
        //                    IVR.Signatures = CommonControl.FileUpload("Content/JobDocument/", Imagesection);
        //                }
        //                else
        //                {
        //                    if (Imagesection.FileName != "")
        //                    {
        //                        IVR.Signatures = "NoImage.gif";
        //                    }
        //                }
        //            }


        //            #endregion
        //            IVR.Status = "1";
        //            IVR.Type = "IRN";
        //            IVR.Identification_Of_Inspected = Convert.ToString(fc["hdnContent"]);
        //            Result = OBJDALIRN.InsertUpdateConclousion(IVR);
        //            if(IVR.OrderStatus != null)
        //            {
        //                if(IVR.OrderStatus.ToString().ToUpper() == "ACTIVE")
        //                {
        //                    CustomerFeedbackMail(IVR.client_Email.ToString());

        //                }
        //            }

        //            if (Result != "" && Result != null)
        //            {
        //                TempData["InsertCompany"] = Result;
        //                return View(IVR);
        //            }
        //        }
        //        else
        //        {
        //            #region File Upload Code 

        //            IRNID = IVR.PK_IVR_ID;
        //            IVR.Identification_Of_Inspected = Convert.ToString(fc["hdnContent"]);
        //            if (IRNID != null && IRNID != 0)
        //            {
        //                if (lstFileDtls != null && lstFileDtls.Count > 0)
        //                {
        //                    Result = OBJDALIRN.InsertConFileAttachment(lstFileDtls, IRNID);
        //                    Session["listNCRUploadedFile"] = null;
        //                }
        //            }
        //            HttpPostedFileBase Imagesection;
        //            if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
        //            {
        //                Imagesection = Request.Files["img_Banner"];
        //                if (Imagesection != null && Imagesection.FileName != "")
        //                {
        //                    IVR.Signatures = CommonControl.FileUpload("Content/JobDocument/", Imagesection);
        //                }
        //                else
        //                {
        //                    if (Imagesection.FileName != "")
        //                    {
        //                        IVR.Signatures = "NoImage.gif";
        //                    }
        //                }
        //            }
        //            #endregion

        //            Result = OBJDALIRN.InsertUpdateConclousion(IVR);
        //            if (IVR.OrderStatus != null)
        //            {
        //                if (IVR.OrderStatus.ToString().ToUpper() == "ACTIVE")
        //                {
        //                    CustomerFeedbackMail(IVR.client_Email.ToString());

        //                }
        //            }
        //            if (Result != null && Result != "")
        //            {
        //                TempData["UpdateCompany"] = Result;                        
        //                return RedirectToAction("Conclusion", new { @PK_IVR_ID = IVR.PK_IVR_ID, @PK_RM_ID = IVR.defid });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    return RedirectToAction("Conclusion", new { @PK_IVR_ID = IVR.PK_IVR_ID, @PK_RM_ID = IVR.defid });
        //}
        #endregion


        #region conclusion new
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Conclusion(int? PK_IVR_ID, int? PK_RM_ID)
        {
            if (Convert.ToInt32(PK_RM_ID) != 0)
            {
                ObjModelVisitReport.defid = Convert.ToInt32(PK_RM_ID);
                ObjModelVisitReport.abcid = Convert.ToInt32(PK_RM_ID);
            }
            if (Convert.ToInt32(PK_IVR_ID) != 0)
            {
                ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(PK_IVR_ID);
            }
            if (ObjModelVisitReport.PK_IVR_ID != 0)
            {
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = OBJDALIRN.EditConUploadedFile(Convert.ToInt32(ObjModelVisitReport.PK_IVR_ID));
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
                    ObjModelVisitReport.FileDetails = lstEditFileDetails;
                }
                DataSet DSJobMasterByQtId = new DataSet();
                DataSet dsDownloadPrint = new DataSet();

                dsDownloadPrint = OBJDALIRN.DownloadPrint(Convert.ToInt32(ObjModelVisitReport.PK_IVR_ID));
                if (dsDownloadPrint.Tables[0].Rows.Count > 0)
                {
                    ObjModelVisitReport.DownloadPrint = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["Report"]);
                    ObjModelVisitReport.ReportName = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["ReportName"]);
                }

                DSJobMasterByQtId = OBJDALIRN.EditInspectionVisitReportByPKCallID(ObjModelVisitReport.PK_IVR_ID);

                if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
                {
                    ViewBag.Data = string.Empty;
                    ObjModelVisitReport.Branch = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Branch"]);
                    ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubJob_No"]);
                    ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sap_And_Controle_No"]);
                    ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_IVR_ID"]);
                    ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name_Location"]);
                    ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Address_Of_Inspection"]);
                    ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["End_user_Name"]);
                    ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Name_Location"]);
                    //ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_Call_ID"]);
                    ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);
                    ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date_Of_Inspection"]);
                    ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Client_Name"]);
                    ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
                    ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
                    ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No"]);
                    ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Vendor_Name"]);
                    ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No_SubVendor"]);

                    ObjModelVisitReport.OrderStatus = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["OrderStatus"]);
                    ObjModelVisitReport.Sub_Order_Status = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Order_Status"]);

                    int dvmdr = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["DVendor"]);
                    ObjModelVisitReport.vendot = Convert.ToBoolean(dvmdr);
                    int dclnt = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["DClient"]);
                    ObjModelVisitReport.client = Convert.ToBoolean(dclnt);
                    int dtuv = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["DTUV"]);
                    ObjModelVisitReport.TUV = Convert.ToBoolean(dtuv);
                    ObjModelVisitReport.SubType = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubType"]);
                    ObjModelVisitReport.Conclusion = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Conclusion"]);
                    ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Pending_Activites"]);
                    ObjModelVisitReport.Identification_Of_Inspected = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Identification_Of_Inspected"]);
                    ObjModelVisitReport.GetDataAfterSave = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Identification_Of_Inspected"]);
                    ViewBag.Data = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Identification_Of_Inspected"]);
                    ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Areas_Of_Concerns"]);
                    ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Non_Conformities_raised"]);
                    ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Signature"]);
                    ObjModelVisitReport.Waivers = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Waivers"]);
                    int POTotalChk = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["POTotalCheckBox"]);
                    ObjModelVisitReport.POTotalCheckBox = Convert.ToBoolean(POTotalChk);
                }
                return View(ObjModelVisitReport);
            }
            else
            {
                return View(ObjModelVisitReport);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Conclusion(InspectionvisitReportModel IVR, HttpPostedFileBase File, FormCollection fc, int? cid)
        {
            #region Added by Ankush
            int IRNID = 0;
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listNCRUploadedFile"] as List<FileDetails>;
            #endregion

            //IVR.Identification_Of_Inspected = Convert.ToString(fc["textcontent"]);
            //IVR.Identification_Of_Inspected = Convert.ToString(fc["Identification_Of_Inspected"]);

            string Result = string.Empty;
            try
            {
                if (IVR.PK_IVR_ID == 0)
                {
                    #region File Upload Code 

                    HttpPostedFileBase Imagesection;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        Imagesection = Request.Files["img_Banner"];
                        if (Imagesection != null && Imagesection.FileName != "")
                        {
                            IVR.Signatures = CommonControl.FileUpload("Content/JobDocument/", Imagesection);
                        }
                        else
                        {
                            if (Imagesection.FileName != "")
                            {
                                IVR.Signatures = "NoImage.gif";
                            }
                        }
                    }


                    #endregion
                    IVR.Status = "1";
                    IVR.Type = "IRN";
                    IVR.Identification_Of_Inspected = Convert.ToString(fc["hdnContent"]);
                    Result = OBJDALIRN.InsertUpdateConclousion(IVR);
                    if (IVR.OrderStatus != null)
                    {
                        if (IVR.OrderStatus.ToString().ToUpper() == "ACTIVE")
                        {
                            CustomerFeedbackMail(IVR.client_Email.ToString());

                        }
                    }

                    if (Result != "" && Result != null)
                    {
                        TempData["InsertCompany"] = Result;
                        return View(IVR);
                    }
                }
                else
                {
                    #region File Upload Code 

                    IRNID = IVR.PK_IVR_ID;
                    IVR.Identification_Of_Inspected = Convert.ToString(fc["hdnContent"]);
                    if (IRNID != null && IRNID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = OBJDALIRN.InsertConFileAttachment(lstFileDtls, IRNID, IVR.defid);
                            Session["listNCRUploadedFile"] = null;
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, IRNID);
                        }
                    }
                    HttpPostedFileBase Imagesection;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        Imagesection = Request.Files["img_Banner"];
                        if (Imagesection != null && Imagesection.FileName != "")
                        {
                            IVR.Signatures = CommonControl.FileUpload("Content/JobDocument/", Imagesection);
                        }
                        else
                        {
                            if (Imagesection.FileName != "")
                            {
                                IVR.Signatures = "NoImage.gif";
                            }
                        }
                    }
                    #endregion

                    Result = OBJDALIRN.InsertUpdateConclousion(IVR);
                    if (IVR.OrderStatus != null)
                    {
                        if (IVR.OrderStatus.ToString().ToUpper() == "ACTIVE")
                        {
                            CustomerFeedbackMail(IVR.client_Email.ToString());

                        }
                    }
                    if (Result != null && Result != "")
                    {
                        TempData["UpdateCompany"] = Result;
                        return RedirectToAction("Conclusion", new { @PK_IVR_ID = IVR.PK_IVR_ID, @PK_RM_ID = IVR.defid });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = Error });
            }
            return RedirectToAction("Conclusion", new { @PK_IVR_ID = IVR.PK_IVR_ID, @PK_RM_ID = IVR.defid });
        }
        #endregion

        #region  Item Description

        [HttpGet]
        public ActionResult ItemDescription(int? PK_ItemD_Id, int? PK_IVR_ID, int? PK_RM_ID)
        {
            if (Convert.ToInt32(PK_RM_ID) != 0)
            {
                modelItem.abcid = Convert.ToInt32(PK_RM_ID);
            }
            if (Convert.ToInt32(PK_IVR_ID) != 0)
            {
                modelItem.PK_IVR_ID = Convert.ToInt32(PK_IVR_ID);
            }
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<ItemDescriptionModel> lstCompanyDashBoard = new List<ItemDescriptionModel>();
            #region Bind UOM
            List<NameCode> lstProjectType = new List<NameCode>();
            DataSet dsUnit = new DataSet();
            DALInspectionVisitReport objDalVisitReport = new DALInspectionVisitReport();
            dsUnit = objDalVisitReport.Measurement();

            if (dsUnit.Tables[0].Rows.Count > 0)
            {
                lstProjectType = (from n in dsUnit.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(dsUnit.Tables[0].Columns["Name"].ToString()),
                                      Code = n.Field<Int32>(dsUnit.Tables[0].Columns["Id"].ToString())

                                  }).ToList();
            }
            ViewBag.UOM = lstProjectType;

            #endregion
            if (Convert.ToInt32(PK_ItemD_Id) != 0)
            {
                ItemDescriptionData = OBJDALIRN.GetitemDescriptionById(Convert.ToInt32(PK_ItemD_Id));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    modelItem.PK_ItemD_Id = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_ItemD_Id"]);
                    modelItem.Po_Item_No = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Po_Item_No"]);
                    modelItem.ItemCode_Description = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["ItemCode_Description"]);
                    modelItem.Po_Quantity = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Po_Quantity"]);
                    modelItem.Offered_Quantity = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Offered_Quantity"]);
                    modelItem.PK_IVR_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IVR_ID"]);
                    // IVR.PK_CALL_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_CALL_ID"]);

                    modelItem.Item_Code = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Item_Code"]);
                    modelItem.Accepted_Quantity = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Accepted_Quantity"]);
                    modelItem.Cumulative_Accepted_Qty = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Cumulative_Accepted_Qty"]);
                    modelItem.Unit = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Unit"]);


                    CostSheetDashBoard = OBJDALIRN.GetitemDescription(Convert.ToInt32(PK_IVR_ID));
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new ItemDescriptionModel
                                {
                                    PK_ItemD_Id = Convert.ToInt32(dr["PK_ItemD_Id"]),
                                    Po_Item_No = Convert.ToString(dr["Po_Item_No"]),
                                    ItemCode_Description = Convert.ToString(dr["ItemCode_Description"]),
                                    Po_Quantity = Convert.ToString(dr["Po_Quantity"]),
                                    Offered_Quantity = Convert.ToString(dr["Offered_Quantity"]),
                                    PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                    Item_Code = Convert.ToString(dr["Item_Code"]),
                                    Accepted_Quantity = Convert.ToString(dr["Accepted_Quantity"]),
                                    Cumulative_Accepted_Qty = Convert.ToString(dr["Cumulative_Accepted_Qty"]),
                                    Unit = Convert.ToString(dr["Unit"]),
                                }
                                );
                        }
                    }

                }

            }
            else
            {
                try
                {
                    CostSheetDashBoard = OBJDALIRN.GetitemDescription(Convert.ToInt32(PK_IVR_ID));
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new ItemDescriptionModel
                                {
                                    PK_ItemD_Id = Convert.ToInt32(dr["PK_ItemD_Id"]),
                                    Po_Item_No = Convert.ToString(dr["Po_Item_No"]),
                                    ItemCode_Description = Convert.ToString(dr["ItemCode_Description"]),
                                    Po_Quantity = Convert.ToString(dr["Po_Quantity"]),
                                    Offered_Quantity = Convert.ToString(dr["Offered_Quantity"]),
                                    // PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                    Item_Code = Convert.ToString(dr["Item_Code"]),
                                    Accepted_Quantity = Convert.ToString(dr["Accepted_Quantity"]),
                                    Cumulative_Accepted_Qty = Convert.ToString(dr["Cumulative_Accepted_Qty"]),
                                    Unit = Convert.ToString(dr["Unit"]),
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(modelItem);
        }
        [HttpPost]
        public ActionResult ItemDescription(ItemDescriptionModel IVR, FormCollection fc, HttpPostedFileBase FileUpload)
        {
            string Result = string.Empty;

            if (IVR.PK_ItemD_Id == 0)
            {
                #region Rahul Code By 03-May-2019
                HttpPostedFileBase files = FileUpload;
                if (files != null)
                {


                    if (files.ContentLength > 0 && !string.IsNullOrEmpty(files.FileName) && files.FileName.Contains(".xlsx"))
                    {
                        //if(files.FileName == "ExcellData.xlsx")
                        //{
                        try
                        {

                            string fileName = files.FileName;
                            string fileContentType = files.ContentType;
                            byte[] fileBytes = new byte[files.ContentLength];
                            var data1 = files.InputStream.Read(fileBytes, 0, Convert.ToInt32(files.ContentLength));
                            var package = new ExcelPackage(files.InputStream);  //===========Go to Manage Nuget in Install ExcellPackge 

                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            int noOfCol = workSheet.Dimension.End.Column;
                            int noOfRow = workSheet.Dimension.End.Row;
                            int rowIterator = 1;
                            if ("Sr.No" == Convert.ToString(workSheet.Cells[rowIterator, 1].Value) && "Image Name" == Convert.ToString(workSheet.Cells[rowIterator, 2].Value) && "Product Image" == Convert.ToString(workSheet.Cells[rowIterator, 3].Value))
                            {

                            }
                            for (rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {

                                string PoItemNo = Convert.ToString(workSheet.Cells[rowIterator, 1].Value);
                                string itemCode = Convert.ToString(workSheet.Cells[rowIterator, 2].Value);
                                string ItemDescription = Convert.ToString(workSheet.Cells[rowIterator, 3].Value);
                                string Unit = Convert.ToString(workSheet.Cells[rowIterator, 4].Value);
                                string PoQuantity = Convert.ToString(workSheet.Cells[rowIterator, 5].Value);
                                string OfferedQuantity = Convert.ToString(workSheet.Cells[rowIterator, 6].Value);
                                string AcceptedQuantity = Convert.ToString(workSheet.Cells[rowIterator, 7].Value);
                                string Cumulative_Accepted_Qty = Convert.ToString(workSheet.Cells[rowIterator, 8].Value);
                                //string TypeOfFood = Convert.ToString(workSheet.Cells[rowIterator, 8].Value);
                                //string ProductFeatured = Convert.ToString(workSheet.Cells[rowIterator, 9].Value);
                                //string IsNew = Convert.ToString(workSheet.Cells[rowIterator, 10].Value);
                                //string Description = Convert.ToString(workSheet.Cells[rowIterator, 11].Value);
                                // string Image = Convert.ToString(workSheet.Cells[rowIterator, 3].Value);

                                //string BrandID = Convert.ToString(workSheet.Cells[rowIterator, 13].Value);
                                //string BreedID = Convert.ToString(workSheet.Cells[rowIterator, 14].Value);

                                // string ImageCode = getImagecode(Image);
                                // string Imagenames = ImageName;
                                try
                                {

                                    if (PoItemNo != "")
                                    {
                                        //itvm.MwmImage = ImageCode;
                                        //itvm.ImgSName = Imagenames;

                                        IVR.Po_Item_No = PoItemNo;
                                        IVR.Item_Code = itemCode;
                                        IVR.ItemCode_Description = ItemDescription;
                                        IVR.Unit = Unit;
                                        IVR.Po_Quantity = PoQuantity;
                                        IVR.Offered_Quantity = OfferedQuantity;
                                        IVR.Accepted_Quantity = AcceptedQuantity;
                                        IVR.Cumulative_Accepted_Qty = Cumulative_Accepted_Qty;
                                        IVR.Status = "1";
                                        IVR.Type = "IRN";
                                        Result = OBJDALIRN.InsertUpdateItemDescription(IVR);
                                    }
                                    else
                                    {
                                        throw new System.ArgumentException();
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            TempData["Message"] = ex.Message;
                        }
                    }
                    else
                    {



                        #endregion

                        IVR.Status = "1";
                        IVR.Type = "IRN";
                        Result = OBJDALIRN.InsertUpdateItemDescription(IVR);
                        if (Result != "" && Result != null)
                        {
                            TempData["InsertCompany"] = Result;

                        }
                    }
                }
                else
                {
                    IVR.Status = "1";
                    IVR.Type = "IRN";
                    Result = OBJDALIRN.InsertUpdateItemDescription(IVR);
                    if (Result != "" && Result != null)
                    {
                        TempData["InsertCompany"] = Result;

                    }
                }
            }
            else
            {


                Result = OBJDALIRN.InsertUpdateItemDescription(IVR);
                if (Result != null && Result != "")
                {

                    TempData["UpdateCompany"] = Result;

                }

            }

            ItemDescriptionModel IVRNew = new ItemDescriptionModel();
            IVRNew.PK_IVR_ID = IVR.PK_IVR_ID;

            return RedirectToAction("ItemDescription", new { @PK_IVR_ID = IVR.PK_IVR_ID, @PK_RM_ID = IVR.abcid });
        }


        public ActionResult DeleteItemDescriptionData(int? PK_ItemD_Id, int? PK_RM_ID)
        {
            DataSet ItemDescriptionData = new DataSet();
            int Result = 0;
            int PK_IVR_ID = 0;
            try
            {
                Result = OBJDALIRN.DeleteItemDescription(Convert.ToInt32(PK_ItemD_Id));

                ItemDescriptionData = OBJDALIRN.GetitemDescriptionById(Convert.ToInt32(PK_ItemD_Id));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    modelItem.PK_ItemD_Id = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_ItemD_Id"]);
                    modelItem.Po_Item_No = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Po_Item_No"]);
                    modelItem.ItemCode_Description = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["ItemCode_Description"]);
                    modelItem.Po_Quantity = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Po_Quantity"]);
                    modelItem.Offered_Quantity = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Offered_Quantity"]);
                    modelItem.Item_Code = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Item_Code"]);
                    PK_IVR_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IVR_ID"]);
                    modelItem.Accepted_Quantity = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Accepted_Quantity"]);
                    modelItem.Cumulative_Accepted_Qty = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Cumulative_Accepted_Qty"]);
                    modelItem.Unit = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Unit"]);
                }
                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    //ItemDescriptionModel IVRNews = new ItemDescriptionModel();
                    //IVRNews.PK_IVR_ID = PK_IVR_ID;
                    return RedirectToAction("ItemDescription", new { @PK_IVR_ID = Convert.ToInt32(PK_IVR_ID), @PK_RM_ID = Convert.ToInt32(PK_RM_ID) });
                }
            }
            catch (Exception)
            {
                throw;
            }
            ItemDescriptionModel IVRNew = new ItemDescriptionModel();
            IVRNew.PK_IVR_ID = PK_IVR_ID;
            return RedirectToAction("ItemDescription", new { @PK_IVR_ID = Convert.ToInt32(PK_IVR_ID), @PK_RM_ID = Convert.ToInt32(PK_RM_ID) });
        }


        #region 3-may-2019 Code By Rahul
        string getImagecode(string str)
        {
            var extrctdStrng = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str.ElementAt(i) == '=')
                {
                    var num = i + 1;
                    extrctdStrng = str.Substring(num);
                }
            }
            extrctdStrng = "https://drive.google.com/uc?export=view&id=" + extrctdStrng;
            return extrctdStrng;
        }

        #endregion
        #endregion

        #region  Reference Documents

        [HttpGet]
        public ActionResult ReferenceDocuments(int? PK_RD_ID, int? PK_IVR_ID, int? PK_RM_ID)
        {
            if (Convert.ToInt32(PK_RM_ID) != 0)
            {
                modelRef.abcid = Convert.ToInt32(PK_RM_ID);
            }
            if (Convert.ToInt32(PK_IVR_ID) != 0)
            {
                modelRef.PK_IVR_ID = Convert.ToInt32(PK_IVR_ID);
            }
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<ReferenceDocumentsModel> lstCompanyDashBoard = new List<ReferenceDocumentsModel>();
            List<ReferenceDocumentsModel> lstDocName = new List<ReferenceDocumentsModel>();

            if (Convert.ToInt32(PK_RD_ID) != 0)
            {
                ItemDescriptionData = OBJDALIRN.GetRefranceDocuments(Convert.ToInt32(PK_RD_ID));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    modelRef.Document_Name = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Document_Name"]);
                    modelRef.Document_No = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Document_No"]);
                    modelRef.Approval_Status = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Approval_Status"]);
                    modelRef.PK_RD_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_RD_ID"]);
                    modelRef.PK_IVR_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IVR_ID"]);
                    modelRef.VendorDocumentNumber = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["VendorDocumentNumber"]);

                    //CostSheetDashBoard = OBJDALIRN.GetReferenceDocuments(modelRef.PK_IVR_ID);
                    if (ItemDescriptionData.Tables.Count > 0)
                    {
                        modelRef.Visible = "True";
                        foreach (DataRow dr in ItemDescriptionData.Tables[0].Rows)
                        {
                            lstDocName.Add(
                                new ReferenceDocumentsModel
                                {
                                    Document_No = Convert.ToString(dr["Document_No"]),
                                    Document_Name = Convert.ToString(dr["Document_Name"]),
                                    Approval_Status = Convert.ToString(dr["Approval_Status"]),
                                    PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                                    VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"]),
                                }
                                );
                        }

                        ViewBag.DocName = lstDocName;
                    }

                    //CostSheetDashBoard = objDalVisitReport.GetReferenceDocuments(IVR.PK_CALL_ID);
                    //if (CostSheetDashBoard.Rows.Count > 0)
                    //{
                    //    IVR.Visible = "True";
                    //    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    //    {
                    //        lstRefranceDocsDashBoard.Add(
                    //            new ReferenceDocumentsModel
                    //            {
                    //                Document_No = Convert.ToString(dr["Document_No"]),
                    //                Document_Name = Convert.ToString(dr["Document_Name"]),
                    //                Approval_Status = Convert.ToString(dr["Approval_Status"]),
                    //                PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                    //                PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                    //                VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"])
                    //            }
                    //            );
                    //    }
                    //}




                }
            }
            else
            {
                try
                {
                    CostSheetDashBoard = OBJDALIRN.GetReferenceDocuments(modelRef.PK_IVR_ID);
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        modelRef.Visible = "False";
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new ReferenceDocumentsModel
                                {
                                    Document_No = Convert.ToString(dr["Document_No"]),
                                    Document_Name = Convert.ToString(dr["Document_Name"]),
                                    Approval_Status = Convert.ToString(dr["Approval_Status"]),
                                    PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                                    VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"]),
                                }
                                );
                        }
                    }
                    else
                    {
                        #region Bind Hardcoded Document Name
                        var arrDocName = new string[] { "PR/MR/ARM", "QAP/ITP/QCP", "Drawing(s)", "Procedure(s)", "Datasheet(s)", "Others(Specify)" };

                        foreach (var dr in arrDocName) // loop for adding add from dataset to list<modeldata>  
                        {
                            lstDocName.Add(new ReferenceDocumentsModel
                            {
                                Document_Name = dr,

                            });
                        }

                        modelRef.Visible = "True";
                        modelRef.Document_Name = null;
                        ViewData["DocName"] = lstDocName;
                        ViewBag.DocName = lstDocName;
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(modelRef);
        }
        [HttpPost]
        public ActionResult ReferenceDocuments(ReferenceDocumentsModel IVR, FormCollection fc)
        {
            string Result = string.Empty;
            try
            {
                if (IVR.PK_RD_ID == 0)
                {
                    foreach (var item in IVR.RD)
                    {
                        IVR.Type = "IRN";
                        IVR.Status = "1";
                        IVR.Document_Name = item.Document_Name;
                        IVR.Document_No = item.Document_No;
                        IVR.VendorDocumentNumber = item.VendorDocumentNumber;
                        IVR.Approval_Status = item.Approval_Status;
                        Result = OBJDALIRN.InsertUpdateReferenceDocuments(IVR);
                        if (Result != "" && Result != null)
                        {
                            TempData["InsertCompany"] = Result;
                        }
                    }

                }
                else
                {
                    foreach (var item in IVR.RD)
                    {
                        IVR.Type = "IRN";
                        IVR.Status = "1";
                        IVR.Document_Name = item.Document_Name;
                        IVR.Document_No = item.Document_No;
                        IVR.VendorDocumentNumber = item.VendorDocumentNumber;
                        IVR.Approval_Status = item.Approval_Status;

                        Result = OBJDALIRN.InsertUpdateReferenceDocuments(IVR);
                        if (Result != null && Result != "")
                        {
                            TempData["UpdateCompany"] = Result;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("ReferenceDocuments", new { @PK_IVR_ID = IVR.PK_IVR_ID, @PK_RM_ID = IVR.abcid });
        }

        [HttpPost]
        public ActionResult UpdateReferenceD(ReferenceDocumentsModel IVR, FormCollection fc)
        {

            int? PK_CALL_ID = IVR.PK_IVR_ID;

            string Result = string.Empty;
            try
            {
                // IVR.PK_RD_ID = 1;// Set Hard code value for Update
                //if (IVR.PK_RD_ID == 0)
                //{
                //    foreach (var item in IVR.RD)
                //    {
                //        IVR.Type = "IVR";
                //        IVR.Status = "1";
                //        IVR.Document_Name = item.Document_Name;
                //        IVR.Document_No = item.Document_No;
                //        IVR.VendorDocumentNumber = item.Document_No;
                //        IVR.Approval_Status = item.Approval_Status;
                //        Result = objDalVisitReport.InsertUpdateReferenceDocuments(IVR);
                //        if (Result != "" && Result != null)
                //        {
                //            TempData["InsertCompany"] = Result;
                //        }
                //    }



                //}
                //else
                //{
                foreach (var item in IVR.RD)
                {

                    IVR.PK_CALL_ID = item.PK_CALL_ID;
                    IVR.Type = "IVR";
                    IVR.Status = "1";
                    IVR.Document_Name = item.Document_Name;
                    IVR.Document_No = item.Document_No;
                    IVR.VendorDocumentNumber = item.VendorDocumentNumber;
                    IVR.Approval_Status = item.Approval_Status;
                    IVR.PK_RD_ID = item.PK_RD_ID;
                    Result = OBJDALIRN.InsertUpdateReferenceDocuments(IVR);
                    if (Result != "" && Result != null)
                    {
                        TempData["InsertCompany"] = Result;
                    }
                }


                if (Result != null && Result != "")
                {
                    TempData["UpdateCompany"] = Result;
                }

                //}
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            #region
            DataTable CostSheetDashBoard = new DataTable();
            List<ReferenceDocumentsModel> lstCompanyDashBoard = new List<ReferenceDocumentsModel>();
            CostSheetDashBoard = OBJDALIRN.GetReferenceDocuments(IVR.PK_CALL_ID);
            try
            {
                if (CostSheetDashBoard.Rows.Count > 0)
                {

                    IVR.PK_CALL_ID = Convert.ToInt32(CostSheetDashBoard.Rows[0]["PK_CALL_ID"]);
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReferenceDocumentsModel
                            {

                                Document_No = Convert.ToString(dr["Document_No"]),
                                Document_Name = Convert.ToString(dr["Document_Name"]),
                                Approval_Status = Convert.ToString(dr["Approval_Status"]),
                                PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                                PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CostSheet"] = lstCompanyDashBoard;
            ViewBag.CostSheet = lstCompanyDashBoard;
            #endregion

            IVR.Document_No = "";
            IVR.Document_Name = "";
            IVR.Approval_Status = "";
            IVR.PK_RD_ID = 0;
            IVR.VendorDocumentNumber = "";


            // IVR = null;

            ReferenceDocumentsModel IVRNew = new ReferenceDocumentsModel();

            return RedirectToAction("ReferenceDocuments", new { PK_IVR_ID = IVR.PK_IVR_ID, @PK_RM_ID = IVR.abcid });

        }

        public ActionResult DeleteIReferenceDocumentsData(int? PK_RD_ID, int? PK_RM_ID)
        {
            DataSet ItemDescriptionData = new DataSet();
            int Result = 0;

            try
            {
                Result = OBJDALIRN.DeleteRefranceDocuments(Convert.ToInt32(PK_RD_ID));
                ItemDescriptionData = OBJDALIRN.GetRefranceDocuments(Convert.ToInt32(PK_RD_ID));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    modelRef.Document_Name = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Document_Name"]);
                    modelRef.Document_No = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Document_No"]);
                    modelRef.Approval_Status = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Approval_Status"]);
                    modelRef.PK_RD_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_RD_ID"]);
                    modelRef.PK_IVR_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IVR_ID"]);
                }
                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    return RedirectToAction("ReferenceDocuments", new { @PK_IVR_ID = modelRef.PK_IVR_ID, @PK_RM_ID = Convert.ToInt32(PK_RM_ID) });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("ReferenceDocuments", new { @PK_IVR_ID = modelRef.PK_IVR_ID, @PK_RM_ID = Convert.ToInt32(PK_RM_ID) });
        }
        #endregion

        #region  Inspection Activities

        [HttpGet]
        public ActionResult InspectionActivites(int? IPK_IA_ID, int? PK_IVR_ID, int? PK_RM_ID)
        {
            if (Convert.ToInt32(PK_RM_ID) != 0)
            {
                modelStg.abcid = Convert.ToInt32(PK_RM_ID);
            }
            if (Convert.ToInt32(PK_IVR_ID) != 0)
            {
                modelStg.PK_IVR_ID = Convert.ToInt32(PK_IVR_ID);
            }
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<InspectionActivitiesModel> lstCompanyDashBoard = new List<InspectionActivitiesModel>();
            if (Convert.ToInt32(IPK_IA_ID) != 0)
            {
                ItemDescriptionData = OBJDALIRN.GetInspectionActivitiesByPKIAID(Convert.ToInt32(IPK_IA_ID));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    modelStg.Stages_Witnessed = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Stages_Witnessed"]);
                    modelStg.PK_IA_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IA_ID"]);
                    modelStg.PK_IVR_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IVR_ID"]);

                    CostSheetDashBoard = OBJDALIRN.GetInspectionActivities(modelStg.PK_IVR_ID);
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new InspectionActivitiesModel
                                {
                                    Stages_Witnessed = Convert.ToString(dr["Stages_Witnessed"]),
                                    PK_IA_ID = Convert.ToInt32(dr["PK_IA_ID"]),
                                    //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
                                }
                                );
                        }
                    }

                }

            }
            else
            {
                try
                {
                    CostSheetDashBoard = OBJDALIRN.GetInspectionActivities(modelStg.PK_IVR_ID);
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new InspectionActivitiesModel
                                {
                                    Stages_Witnessed = Convert.ToString(dr["Stages_Witnessed"]),
                                    PK_IA_ID = Convert.ToInt32(dr["PK_IA_ID"]),
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
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            //IVR.Stages_Witnessed = null;
            return View(modelStg);
        }
        [HttpPost]
        public ActionResult InspectionActivites(InspectionActivitiesModel IVR, FormCollection fc)
        {
            string Result = string.Empty;
            try
            {
                if (IVR.PK_IA_ID == 0)
                {
                    IVR.Type = "IRN";
                    IVR.Status = "1";
                    Result = OBJDALIRN.InsertUpdateInspectionActivities(IVR);
                    if (Result != "" && Result != null)
                    {
                        TempData["InsertCompany"] = Result;
                    }

                }
                else
                {
                    Result = OBJDALIRN.InsertUpdateInspectionActivities(IVR);
                    if (Result != null && Result != "")
                    {
                        TempData["UpdateCompany"] = Result;
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("InspectionActivites", new { @PK_IVR_ID = IVR.PK_IVR_ID, @PK_RM_ID = IVR.abcid });
        }

        public ActionResult DeleteIInspectionActivitesData(int? PK_IA_ID, int? PK_RM_ID)
        {
            DataSet ItemDescriptionData = new DataSet();
            int Result = 0;
            try
            {
                Result = OBJDALIRN.DeleteInspectionActivities(Convert.ToInt32(PK_IA_ID));
                ItemDescriptionData = OBJDALIRN.GetInspectionActivitiesByPKIAID(Convert.ToInt32(PK_IA_ID));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    modelStg.Stages_Witnessed = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Stages_Witnessed"]);
                    modelStg.PK_IA_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IA_ID"]);
                    modelStg.PK_IVR_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IVR_ID"]);
                }

                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    return RedirectToAction("InspectionActivites", new { @PK_IVR_ID = modelStg.PK_IVR_ID, @PK_RM_ID = Convert.ToInt32(PK_RM_ID) });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("InspectionActivites", new { @PK_IVR_ID = modelStg.PK_IVR_ID, @PK_RM_ID = Convert.ToInt32(PK_RM_ID) });
        }
        #endregion

        #region  Document Reviewd

        [HttpGet]
        public ActionResult DocumentReviewed(int? PK_DR_ID, int? PK_IVR_ID, int? PK_RM_ID)
        {
            if (Convert.ToInt32(PK_RM_ID) != 0)
            {
                modelDr.abcid = Convert.ToInt32(PK_RM_ID);
            }
            if (Convert.ToInt32(PK_IVR_ID) != 0)
            {
                modelDr.PK_IVR_ID = Convert.ToInt32(PK_IVR_ID);
            }
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<DocumentRevieweModel> lstCompanyDashBoard = new List<DocumentRevieweModel>();

            if (Convert.ToInt32(PK_DR_ID) != 0)
            {
                ItemDescriptionData = OBJDALIRN.GetDocumentRevieweModelById(Convert.ToInt32(PK_DR_ID));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    modelDr.Description = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Description"]);
                    modelDr.PK_DR_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_DR_ID"]);
                    modelDr.PK_IVR_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IVR_ID"]);

                    CostSheetDashBoard = OBJDALIRN.GetDocumentRevieweModelByCall_Id(Convert.ToInt32(modelDr.PK_IVR_ID));
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new DocumentRevieweModel
                                {
                                    Description = Convert.ToString(dr["Description"]),
                                    PK_DR_ID = Convert.ToInt32(dr["PK_DR_ID"]),
                                }
                                );
                        }
                    }

                }

            }
            else
            {
                try
                {
                    CostSheetDashBoard = OBJDALIRN.GetDocumentRevieweModelByCall_Id(Convert.ToInt32(modelDr.PK_IVR_ID));
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new DocumentRevieweModel
                                {
                                    Description = Convert.ToString(dr["Description"]),
                                    PK_DR_ID = Convert.ToInt32(dr["PK_DR_ID"]),
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(modelDr);
        }
        [HttpPost]
        public ActionResult DocumentReviewed(DocumentRevieweModel IVR, FormCollection fc)
        {
            string Result = string.Empty;
            try
            {
                if (IVR.PK_DR_ID == 0)
                {
                    IVR.Type = "IRN";
                    IVR.Status = "1";
                    Result = OBJDALIRN.InsertUpdateDocumentReviewe(IVR);
                    if (Result != "" && Result != null)
                    {
                        TempData["InsertCompany"] = Result;
                    }
                }
                else
                {
                    Result = OBJDALIRN.InsertUpdateDocumentReviewe(IVR);
                    if (Result != null && Result != "")
                    {
                        TempData["UpdateCompany"] = Result;
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("DocumentReviewed", new { @PK_IVR_ID = IVR.PK_IVR_ID, @PK_RM_ID = IVR.abcid });
        }
        public ActionResult DeleteDocumentReviewedData(int? PK_DR_ID, int? PK_RM_ID)
        {
            DataSet ItemDescriptionData = new DataSet();
            int Result = 0;

            try
            {
                Result = OBJDALIRN.DeleteDocumentReviewe(Convert.ToInt32(Convert.ToInt32(PK_DR_ID)));
                ItemDescriptionData = OBJDALIRN.GetDocumentRevieweModelById(Convert.ToInt32(PK_DR_ID));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    modelDr.Description = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Description"]);
                    modelDr.PK_DR_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_DR_ID"]);
                    modelDr.PK_IVR_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IVR_ID"]);
                }
                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    return RedirectToAction("DocumentReviewed", new { @PK_IVR_ID = modelDr.PK_IVR_ID, @PK_RM_ID = Convert.ToInt32(PK_RM_ID) });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("DocumentReviewed", new { @PK_IVR_ID = modelDr.PK_IVR_ID, @PK_RM_ID = Convert.ToInt32(PK_RM_ID) });
        }
        #endregion

        #region  Report 

        [HttpGet]
        public ActionResult IRNReports()
        {
            Session["GetExcelData"] = "Yes";
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();
            try
            {
                if (Session["FromDate1"] != null && Session["FromDate1"] != "" && Session["Todate1"] != null && Session["Todate1"] != "")
                {
                    objModelReport.FromDate = Session["FromDate1"].ToString();
                    objModelReport.ToDate = Session["Todate1"].ToString();
                    //CostSheetDashBoard = OBJDALIRN.GetIRNReportdate(objModelReport);
                    CostSheetDashBoard = OBJDALIRN.GetIRNReportdateMIS(objModelReport);
                }
                else
                {
                    CostSheetDashBoard = OBJDALIRN.GetReportByUserMIS();
                }
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
                                inspectionDate = Convert.ToString(dr["inspectionDate"]),
                                ProjectName = Convert.ToString(dr["ProjectName"]),
                                SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                VendorName = Convert.ToString(dr["Vendor_Name"]),
                                Vendor_Name_Location = Convert.ToString(dr["SubVendorName"]),
                                ClientName = Convert.ToString(dr["Po_Number"]),
                                Po_No = Convert.ToString(dr["SubVendorPoNo"]),
                                Client_Name = Convert.ToString(dr["client"]),
                                Edit = Convert.ToString(dr["Edit"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Excuting_Branch = Convert.ToString(dr["Excuting_Branch"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                ddlReviseReason = Convert.ToString(dr["ddlReviseReason"]),
                                ReviseReason = Convert.ToString(dr["ReviseReason"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {

                string Error = ex.Message.ToString();
                return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = Error });
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            objModelReport.LstDashboard = lstCompanyDashBoard;

            return View(objModelReport);
        }
        #region
        [HttpPost]
        public ActionResult IRNReports(string FromDate, string ToDate)
        {

            Session["FromDate1"] = FromDate;
            Session["Todate1"] = ToDate;

            //return View();
            return RedirectToAction("IRNReports", "MisInspectionReleaseNote");
        }

        //[HttpPost]
        //public ActionResult IRNReports(ReportModel IVR, FormCollection fc, HttpPostedFileBase File)
        //{
        //    string Result = string.Empty;
        //    try
        //    {
        //        if (IVR.PK_RM_ID == 0)
        //        {

        //            IVR.Type = "IVR";
        //            IVR.Status = "1";
        //            Result = OBJDALIRN.InsertUpdateReport(IVR);
        //            if (Result != "" && Result != null)
        //            {
        //                TempData["InsertCompany"] = Result;
        //            }

        //        }
        //        else
        //        {

        //            Result = OBJDALIRN.InsertUpdateReport(IVR);
        //            if (Result != null && Result != "")
        //            {
        //                TempData["UpdateCompany"] = Result;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    #region
        //    DataTable CostSheetDashBoard = new DataTable();
        //    List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();
        //    CostSheetDashBoard = OBJDALIRN.GetReportByCall_Id(IVR.PK_CALL_ID);
        //    try
        //    {
        //        if (CostSheetDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in CostSheetDashBoard.Rows)
        //            {
        //                lstCompanyDashBoard.Add(
        //                    new ReportModel
        //                    {
        //                        ReportName = Convert.ToString(dr["ReportName"]),
        //                        Report = Convert.ToString(dr["Report"]),
        //                        CraetedDate = Convert.ToDateTime(dr["CraetedDate"]),
        //                        PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
        //                        PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
        //                    }
        //                    );
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    ViewData["CostSheet"] = lstCompanyDashBoard;
        //    IVR.PK_RM_ID = 0;
        //    #endregion


        //    return View(IVR);
        //}

        #endregion
        //[HttpPost]
        public JsonResult IRNReportGanrate(int? PK_IVR_ID, string DName, string DNo, string PNo, string SPNo, string RReason)  //==========Revise Report Code
        {
            #region Added by Ankush
            string Result08 = string.Empty;
            int IRNID = 0;
            var IPath = string.Empty;
            string[] splitedGrp;
            List<FileDetails> fileDetails = new List<FileDetails>();
            List<string> Selected = new List<string>();
            //List<FileDetails> lstFileDtls = new List<FileDetails>();
            //fileDetails = Session["listCMPUploadedFile"] as List<FileDetails>;
            #endregion

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
                        filePathCMP = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetail.Id + fileDetail.Extension);
                        var K = "~/Content/JobDocument/" + fileName;
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
            Session["listIRNUploadedFile"] = fileDetails;
            #endregion

            IRNID = Convert.ToInt32(PK_IVR_ID);
            if (IRNID != null && IRNID != 0)
            {
                if (fileDetails != null && fileDetails.Count > 0)
                {
                    Result08 = OBJDALIRN.InsertFileAttachment(fileDetails, IRNID);
                    Session["listIRNUploadedFile"] = null;
                }
            }
            if (Convert.ToInt32(PK_IVR_ID) != 0)
            {
                string id = OBJDALIRN.UpdateReviseReport(Convert.ToInt32(PK_IVR_ID), Convert.ToString(DName), Convert.ToString(DNo), Convert.ToString(PNo), Convert.ToString(SPNo), Convert.ToString(RReason));
            }

            DataTable ItemDescriptionDashBoard = new DataTable();
            DataTable RefranceDocumentsDashBoard = new DataTable();
            DataTable InspectionActivitesDashBoard = new DataTable();
            DataTable DocumentsReviewBoard = new DataTable();
            DataTable EquipmentDetailsBoard = new DataTable();
            DataSet DSJobMasterByQtId = new DataSet();
            DataTable ReportDashBoard = new DataTable();
            DataTable CostSheetDashBoard = new DataTable();
            int count = 0;
            DataTable ImageReportDashBoard = new DataTable();
            List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();
            List<ItemDescriptionModel> lstCompanyDashBoard = new List<ItemDescriptionModel>();
            List<ReferenceDocumentsModel> RefranceDocuments = new List<ReferenceDocumentsModel>();
            List<InspectionActivitiesModel> InspectionDocuments = new List<InspectionActivitiesModel>();
            List<DocumentRevieweModel> DocumentReview = new List<DocumentRevieweModel>();
            List<EquipmentDetailsModel> EquipmentDetails = new List<EquipmentDetailsModel>();
            List<ReportModel> ReportDashboard = new List<ReportModel>();

            var Data = OBJDALIRN.GetBranchList();
            ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");

            ReportModel RM = new ReportModel();
            string Result = "";
            if (PK_IVR_ID != 0 || PK_IVR_ID != null)
            {
                int i = 0;
                int J = 0;
                int K = 0;
                int L = 0;
                int M = 0;
                int N = 0;
                #region 
                DSJobMasterByQtId = OBJDALIRN.EditInspectionVisitReportByPKCallID(Convert.ToInt32(PK_IVR_ID));

                if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
                {
                    ObjModelVisitReport.Branch = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Branch"]);
                    ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sap_And_Controle_No"]);
                    ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_IVR_ID"]);
                    ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name_Location"]);
                    ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Address_Of_Inspection"]);
                    ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["End_user_Name"]);
                    ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Name_Location"]);
                    // ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_Call_ID"]);
                    ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);
                    ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date_Of_Inspection"]);
                    ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Client_Name"]);
                    ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
                    ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
                    ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No"]);
                    ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Vendor_Name"]);
                    ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No_SubVendor"]);

                    //int kick = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Kick_Off_Pre_Inspection"]);
                    //ObjModelVisitReport.Kick_Off_Pre_Inspection = Convert.ToBoolean(kick);

                    //int Mi = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Material_identification"]);
                    //ObjModelVisitReport.Material_identification = Convert.ToBoolean(Mi);

                    //int Interim_Stages = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Interim_Stages"]);
                    //ObjModelVisitReport.Interim_Stages = Convert.ToBoolean(Interim_Stages);

                    //int Document_review = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Document_review"]);
                    //ObjModelVisitReport.Document_review = Convert.ToBoolean(Document_review);

                    //int Final_Inspection = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Final_Inspection"]);
                    //ObjModelVisitReport.Final_Inspection = Convert.ToBoolean(Final_Inspection);

                    //int Re_inspection = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Re_inspection"]);
                    //ObjModelVisitReport.Re_inspection = Convert.ToBoolean(Re_inspection);

                    ObjModelVisitReport.Conclusion = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Conclusion"]);
                    ObjModelVisitReport.Waivers = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Waivers"]);

                    ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Pending_Activites"]);
                    ObjModelVisitReport.Identification_Of_Inspected = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Identification_Of_Inspected"]);
                    ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Areas_Of_Concerns"]);
                    ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Non_Conformities_raised"]);
                    //ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Signatures"]);
                    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Type"]);
                    ObjModelVisitReport.Report_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Report_No"]);
                    // ObjModelVisitReport.Call_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Call_No"]);
                    ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Signature"]);
                    ObjModelVisitReport.Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["LastName"]);
                    ObjModelVisitReport.ReportCreatedDate = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["CreatedDate"]);

                    int Inspection_records = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Inspection_records"]);
                    ObjModelVisitReport.Inspection_records = Convert.ToBoolean(Inspection_records);

                    int Inspection_Photo = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Inspection_Photo"]);
                    ObjModelVisitReport.Inspection_Photo = Convert.ToBoolean(Inspection_Photo);

                    int Other_Specify = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Other_Specify"]);
                    ObjModelVisitReport.Other_Specify = Convert.ToBoolean(Other_Specify);

                    ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubJob_No"]);
                    ObjModelVisitReport.Sub_Order_Status = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Order_Status"]);
                    ObjModelVisitReport.OrderStatus = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["OrderStatus"]);

                    int dvmdr = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["DVendor"]);
                    ObjModelVisitReport.vendot = Convert.ToBoolean(dvmdr);
                    int dclnt = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["DClient"]);
                    ObjModelVisitReport.client = Convert.ToBoolean(dclnt);
                    int dtuv = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["DTUV"]);
                    ObjModelVisitReport.TUV = Convert.ToBoolean(dtuv);
                    ObjModelVisitReport.ReviseReason = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["ReviseReason"]);

                    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Type"]);

                }
                #endregion

                #region  item Description

                ItemDescriptionDashBoard = OBJDALIRN.GetitemDescription(PK_IVR_ID);
                if (ItemDescriptionDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in ItemDescriptionDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ItemDescriptionModel
                            {
                                PK_ItemD_Id = Convert.ToInt32(dr["PK_ItemD_Id"]),
                                Po_Item_No = Convert.ToString(dr["Po_Item_No"]),
                                ItemCode_Description = Convert.ToString(dr["ItemCode_Description"]),
                                Po_Quantity = Convert.ToString(dr["Po_Quantity"]),
                                Offered_Quantity = Convert.ToString(dr["Offered_Quantity"]),
                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                Item_Code = Convert.ToString(dr["Item_Code"]),
                                Accepted_Quantity = Convert.ToString(dr["Accepted_Quantity"]),
                                Cumulative_Accepted_Qty = Convert.ToString(dr["Cumulative_Accepted_Qty"]),
                                Unit = Convert.ToString(dr["Unit"]),
                            }
                            );
                    }
                }
                #endregion

                #region Reference Documents

                RefranceDocumentsDashBoard = OBJDALIRN.GetReferenceDocuments(PK_IVR_ID);
                if (RefranceDocumentsDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in RefranceDocumentsDashBoard.Rows)
                    {
                        RefranceDocuments.Add(
                            new ReferenceDocumentsModel
                            {
                                Document_No = Convert.ToString(dr["Document_No"]),
                                Document_Name = Convert.ToString(dr["Document_Name"]),
                                Approval_Status = Convert.ToString(dr["Approval_Status"]),
                                PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                                VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"]),
                                // PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])

                            }
                            );
                    }
                }
                #endregion

                #region Inspection Activities
                InspectionActivitesDashBoard = OBJDALIRN.GetInspectionActivities(PK_IVR_ID);
                if (InspectionActivitesDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in InspectionActivitesDashBoard.Rows)
                    {
                        InspectionDocuments.Add(
                            new InspectionActivitiesModel
                            {
                                Stages_Witnessed = Convert.ToString(dr["Stages_Witnessed"]),
                                PK_IA_ID = Convert.ToInt32(dr["PK_IA_ID"]),
                                //  PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
                            }
                            );
                    }
                }
                #endregion

                #region Documents Review
                DocumentsReviewBoard = OBJDALIRN.GetDocumentRevieweModelByCall_Id(Convert.ToInt32(PK_IVR_ID));
                if (DocumentsReviewBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DocumentsReviewBoard.Rows)
                    {
                        DocumentReview.Add(
                            new DocumentRevieweModel
                            {
                                Description = Convert.ToString(dr["Description"]),
                                PK_DR_ID = Convert.ToInt32(dr["PK_DR_ID"]),
                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
                            }
                            );
                    }
                }
                #endregion               

                #region report Count
                int count1 = 0;
                int count2 = 0;
                int count4 = 0;
                ReportDashBoard = OBJDALIRN.GetCountByCall_Id(PK_IVR_ID);
                if (ReportDashBoard.Rows.Count >= 0)
                {
                    count2 = ReportDashBoard.Rows.Count;
                    count1 = count2 + 1;
                    count4 = count2 - 1;
                }
                string countNo = Convert.ToString(count2);
                string count3 = Convert.ToString(count4);
                string ReportName1 = ObjModelVisitReport.Report_No.Trim() + "-Rev" + countNo;
                string PreviousNo = ObjModelVisitReport.Report_No.Trim() + "-Rev" + count3;

                string[] RepNo = ReportName1.Split('v');
                int prevcnt = 0;
                string XYZNo = string.Empty;
                string RevNo = string.Empty;
                int No = Convert.ToInt32(RepNo[1]);
                if (No != 0)
                {
                    prevcnt = No - 1;
                    RevNo = Convert.ToString(No);
                    XYZNo = Convert.ToString(RepNo[0]) + "v" + " " + Convert.ToString(prevcnt);
                }
                else
                {
                    RevNo = Convert.ToString("-");
                    XYZNo = Convert.ToString("-");
                }
                #endregion

                #region Save to Pdf Code 

                #region Get Date Of Inspection & Inspector Name

                DataSet dsIN = new DataSet();
                int z = 0;
                string InspectorName = "";
                string Adate = string.Empty;
                string Date = "";
                string InrName = string.Empty;

                List<string> lstInspector = new List<string>();
                List<string> lstDates = new List<string>();

                string InsList = string.Empty;
                dsIN = OBJDALIRN.GetInspectionName(Convert.ToInt32(PK_IVR_ID));
                if (dsIN.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsIN.Tables[0].Rows)
                    {
                        InspectorName = Convert.ToString(dr["InspectorName"]);
                        lstInspector.Add(InspectorName);
                    }
                    InsList = "On behalf of " + string.Join(",", lstInspector);
                }
                else
                {
                    InsList = "";
                }
                if (dsIN.Tables[1].Rows.Count > 0)
                {
                    InrName = dsIN.Tables[1].Rows[0]["InspectorName"].ToString();
                }
                else
                {
                    InrName = "";
                }
                if (dsIN.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsIN.Tables[2].Rows)
                    {
                        Adate = Convert.ToString(dr["Actual_Visit_Date"]);
                        lstDates.Add(Adate);
                    }
                    Date = string.Join(",", lstDates);
                }
                else
                {
                    Date = "";
                }
                #endregion
                SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
                System.Text.StringBuilder strs = new System.Text.StringBuilder();
                string body = string.Empty;

                string ItemDescriptioncontent = "";
                string ReferenceDocumentscontent = "";
                string InspectionDocumentsContent = "";
                string DocumentreviewContent = "";
                //string EquipmentDetailscontent = "";

                string check1 = "";
                string check2 = "";
                string dvenr12 = string.Empty;
                string dclnt12 = string.Empty;
                string dtuv12 = string.Empty;

                using (StreamReader reader = new StreamReader(Server.MapPath("~/inspection-certificate.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("[SapAndControle_No]", ObjModelVisitReport.SubJob_No);
                body = body.Replace("[Branch]", ObjModelVisitReport.Branch);
                body = body.Replace("[NotificationNameNo_Date]", ObjModelVisitReport.Notification_Name_No_Date);
                // body = body.Replace("[DateOfInspection]", ObjModelVisitReport.Date_Of_Inspection);
                body = body.Replace("[DateOfInspection]", Date);
                body = body.Replace("[InspectorNames]", InsList);
                //body = body.Replace("[ReviseReason]", ObjModelVisitReport.ReviseReason);
                if (ObjModelVisitReport.ReviseReason != "-" && ObjModelVisitReport.ReviseReason != "")
                {
                    body = body.Replace("[ReviseReason]", "Reason for Revise- " + Convert.ToString(ObjModelVisitReport.ReviseReason));
                }
                else
                {
                    body = body.Replace("[ReviseReason]", "-");
                }
                body = body.Replace("[ProjectNameLocation]", ObjModelVisitReport.Project_Name_Location);
                body = body.Replace("[AddressOfInspection]", ObjModelVisitReport.Address_Of_Inspection);
                body = body.Replace("[ClientName]", ObjModelVisitReport.Client_Name);
                body = body.Replace("[Enduser_Name]", ObjModelVisitReport.End_user_Name);
                body = body.Replace("[DECPMCEPC_Name]", ObjModelVisitReport.DEC_PMC_EPC_Name);
                body = body.Replace("[DECPMCEPCAssignment_No]", ObjModelVisitReport.DEC_PMC_EPC_Assignment_No);
                body = body.Replace("[VendorNameLocation]", ObjModelVisitReport.Vendor_Name_Location);
                body = body.Replace("[PoNo]", ObjModelVisitReport.Po_No);
                body = body.Replace("[SubVendorName]", ObjModelVisitReport.Sub_Vendor_Name);
                body = body.Replace("[PoNoSubVendor]", ObjModelVisitReport.Po_No_SubVendor);
                body = body.Replace("[Conclusion]", ObjModelVisitReport.Conclusion);
                body = body.Replace("[PendingActivites]", ObjModelVisitReport.Pending_Activites);
                body = body.Replace("[IdentificationOfInspected]", ObjModelVisitReport.Identification_Of_Inspected);
                body = body.Replace("[AreasOfConcerns]", ObjModelVisitReport.Areas_Of_Concerns);
                body = body.Replace("[NonConformitiesraised]", ObjModelVisitReport.Non_Conformities_raised);
                // body = body.Replace("[Name]", ObjModelVisitReport.Name);
                body = body.Replace("[Name]", InrName);

                body = body.Replace("[date]", ObjModelVisitReport.ReportCreatedDate);

                body = body.Replace("[Waivers]", ObjModelVisitReport.Waivers);

                body = body.Replace("[RevisionNo]", ReportName1);

                //body = body.Replace("[PreviousNo]", PreviousNo);

                if (ObjModelVisitReport.vendot == true)
                {
                    dvenr12 = "<span><input type='checkbox' checked> TUVI Client / End User</span>";
                }
                else
                {
                    dvenr12 = "<span><input type='checkbox'> TUVI Client / End User</span>";
                }
                if (ObjModelVisitReport.client == true)
                {
                    dclnt12 = "<span><input type='checkbox' checked> TUV Executing Branch / TUV Originating Branch</span>";
                }
                else
                {
                    dclnt12 = "<span><input type='checkbox'> TUV Executing Branch / TUV Originating Branch</span>";
                }
                if (ObjModelVisitReport.TUV == true)
                {
                    dtuv12 = "<span><input type='checkbox' checked> Vendor / Sub Vendor</span>";
                }
                else
                {
                    dtuv12 = "<span><input type='checkbox'> Vendor / Sub Vendor</span>";
                }

                if (ObjModelVisitReport.OrderStatus == "Complete")
                {
                    //check1 = "<td width='5%' align='center' style='border-right-width: 0px;'><span><input type='checkbox'  value=" + ObjModelVisitReport.Kick_Off_Pre_Inspection + " checked></span></td>";
                    check1 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox'>Incomplete</label></span></td>";
                }
                else
                {
                    //check1 = "<td width='5%' align='center' style='border-right-width: 0px;'><span><input type='checkbox'  value=" + ObjModelVisitReport.Kick_Off_Pre_Inspection + "></span></td>";
                    check1 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Incomplete</label></span></td>";
                }
                if (ObjModelVisitReport.Sub_Order_Status == "Complete")
                {
                    check2 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox'>Incomplete</label></span></td>";
                }
                else
                {
                    check2 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Incomplete</label></span></td>";
                }

                foreach (ItemDescriptionModel v in lstCompanyDashBoard)
                {
                    i = i + 1;
                    ItemDescriptioncontent += "<tr><td style='border: 1px solid #b5b5b5;' width='5%' align='center'><span>" + Convert.ToString(v.Po_Item_No) + ')' + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Item_Code + " </span></td><td style='border: 1px solid #b5b5b5;;white-space: pre-line;' width='35%'><span>" + v.ItemCode_Description + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Unit + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Po_Quantity + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Offered_Quantity + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Accepted_Quantity + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Cumulative_Accepted_Qty + "</span></td></tr>";
                }

                foreach (ReferenceDocumentsModel v in RefranceDocuments)
                {
                    J = J + 1;
                    //ReferenceDocumentscontent += "<tr><td style='border: 1px solid #b5b5b5;' width='5%' align='center'> " + J + ')' + " </td><td style='border: 1px solid #b5b5b5;' width='25%'>" + Convert.ToString(v.Document_Name) + "</td><td style='border: 1px solid #b5b5b5;' width='25%'>" + v.Document_No + " </td><td style='border: 1px solid #b5b5b5;' width='25%'>" + v.VendorDocumentNumber + "</td><td style='border: 1px solid #b5b5b5;' width='25%'>" + v.Approval_Status + "</td></tr>";
                    ReferenceDocumentscontent += "<tr><td  style='border: 1px solid #b5b5b5;vertical-align:top; text-align:center;'  <span>" + J + ')' + "</span> </td><td width='25%' style='border: 1px solid #b5b5b5; white-space: pre-line;vertical-align:top;' >" + Convert.ToString(v.Document_Name) + "</td><td width='25%' style='border: 1px solid #b5b5b5;white-space:pre-line;vertical-align:top;' >" + v.Document_No + " </td><td width='25%' style='border: 1px solid #b5b5b5;white-space:pre-line;vertical-align:top;' >" + v.VendorDocumentNumber + "</td><td width='25%' style='border: 1px solid #b5b5b5;white-space: pre-line;vertical-align:top;' >" + v.Approval_Status + "</td></tr>";
                }

                foreach (InspectionActivitiesModel v in InspectionDocuments)
                {
                    K = K + 1;
                    InspectionDocumentsContent += "<tr><td width='5%' align='center'><span> " + K + ')' + " </span></td><td width='95%'><span align='left'>" + Convert.ToString(v.Stages_Witnessed) + "</span></td></tr>";
                }
                foreach (DocumentRevieweModel v in DocumentReview)
                {
                    L = L + 1;
                    DocumentreviewContent += "<tr><td width='5%' align='center'><span> " + L + ')' + " </span></td><td width='95%' ><span align='left'>" + Convert.ToString(v.Description) + "</span></td></tr>";
                }
                //foreach (EquipmentDetailsModel v in EquipmentDetails)
                //{
                //    M = M + 1;
                //    EquipmentDetailscontent += "<tr><td> " + M + " </td><td>" + Convert.ToString(v.Name_Of_Equipments) + "</td><td>" + v.Range + " </td><td>" + v.Id + "</td><td>" + v.CalibrationValid_Till_date + "</td><td>" + v.Certification_No_Date + "</td></tr>";
                //}

                body = body.Replace("[ItemDescriptionContent]", ItemDescriptioncontent);
                body = body.Replace("[ReferenceDocumentsContent]", ReferenceDocumentscontent);
                body = body.Replace("[InspectionDocumentsContent]", InspectionDocumentsContent);
                body = body.Replace("[DocumentreviewContent]", DocumentreviewContent);

                //body = body.Replace("[EquipmentDetailscontent]", EquipmentDetailscontent);
                body = body.Replace("[Stamp]", "https://tiimes.tuv-india.com/Stamp.png");

                if (ObjModelVisitReport.Signatures != null)
                {
                    body = body.Replace("[Signature]", "https://tiimes.tuv-india.com/Content/Sign/" + ObjModelVisitReport.Signatures + "");
                }
                else
                {
                    body = body.Replace("[Signature]", "-");
                }
                body = body.Replace("[Checkbox1]", check1);
                body = body.Replace("[Checkbox2]", check2);
                body = body.Replace("[DVendor]", dvenr12);
                body = body.Replace("[DClient]", dclnt12);
                body = body.Replace("[DTUV]", dtuv12);
                body = body.Replace("[PreviousNo]", XYZNo);
                body = body.Replace("[RevNo]", RevNo);

                //body = body.Replace("[Checkbox3]", check3);
                //body = body.Replace("[Checkbox4]", check4);
                //body = body.Replace("[Checkbox5]", check5);
                //body = body.Replace("[Checkbox6]", check6);
                //body = body.Replace("[Checkbox7]", check7);
                //body = body.Replace("[Checkbox8]", check8);
                //body = body.Replace("[Checkbox9]", check9);

                strs.Append(body);
                PdfPageSize pageSize = PdfPageSize.A4;
                PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.MaxPageLoadTime = 120;  //=========================5-Aug-2019
                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;

                string _Header = string.Empty;
                string _footer = string.Empty;

                // for Report header by abel
                StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/Inspection_Certificate_Header.html"));
                _Header = _readHeader_File.ReadToEnd();

                _Header = _Header.Replace("[RevisionNo]", ReportName1);
                _Header = _Header.Replace("[Logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png"); // change123 once pulished on server

                StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/inspection-certificate-footer.html")); // Footer is used from IVR as it same and commented in pdf template.
                _footer = _readFooter_File.ReadToEnd();

                // header settings
                converter.Options.DisplayHeader = true ||
                    true || true;
                converter.Header.DisplayOnFirstPage = true;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = 75;

                PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
                headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Header.Add(headerHtml);

                // footer settings
                converter.Options.DisplayFooter = true || true || true;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                converter.Footer.Height = 130;

                PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
                footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Footer.Add(footerHtml);

                //end abel code

                #region Footer Code
                // page numbers can be added using a PdfTextSection object
                PdfTextSection text = new PdfTextSection(0, 70, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
                text.HorizontalAlign = PdfTextHorizontalAlign.Right;
                converter.Footer.Add(text);
                #endregion


                PdfDocument doc = converter.ConvertHtmlString(body);
                string ReportName = ObjModelVisitReport.Report_No.Trim() + "-Rev" + countNo;
                string Report = ObjModelVisitReport.Report_No.Trim() + "-Rev " + countNo + ".pdf";
                string path = Server.MapPath("~/IRNReports");
                doc.Save(path + '\\' + Report);
                doc.Close();
                #endregion
                if (RM.PK_RM_ID == 0)
                {
                    RM.Type = "IRN";
                    RM.Status = "1";
                    //RM.ImageReport = ReportNames;
                    RM.Report = Report;
                    RM.ReportName = ReportName1;
                    RM.PK_CALL_ID = PK_IVR_ID;
                    RM.VendorName = ObjModelVisitReport.Vendor_Name_Location;
                    RM.ClientName = ObjModelVisitReport.Client_Name;
                    RM.ProjectName = ObjModelVisitReport.Project_Name_Location;
                    RM.SubJob_No = ObjModelVisitReport.SubJob_No;
                    Result = OBJDALIRN.InsertUpdateReport(RM);
                    if (Result != "" && Result != null)
                    {

                        TempData["InsertCompany"] = Result;
                    }
                }

                #region
                CostSheetDashBoard = OBJDALIRN.GetReportByCall_Id(PK_IVR_ID);
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        ReportDashboard.Add(
                            new ReportModel
                            {
                                ReportName = Convert.ToString(dr["ReportName"]),
                                Report = Convert.ToString(dr["Report"]),
                                CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                // PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
                            }
                            );
                    }
                }
                ViewData["CostSheet"] = ReportDashboard;
                #endregion
                //return View();
                //return RedirectToAction("IRNReports", "InspectionReleaseNote");
                RM.PK_RM_ID = Convert.ToInt32(Session["GetReportId"]);
                //return View();
                //return RedirectToAction("IRNReports", "InspectionReleaseNote");
                return Json(new { url = Url.Action("IRNForm", "InspectionReleaseNote", new { PK_IVR_ID = RM.PK_CALL_ID, PK_RM_ID = RM.PK_RM_ID/*Session["GetReportId"].ToString()*/ }) });
                //return Json("savingStatus", JsonRequestBehavior.AllowGet);


            }
            else
            {
                return Json("savingStatus", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintReport(int? PK_IVR_ID, int? PK_RM_ID)
        {
            string ReportName = string.Empty;
            string Report = string.Empty;
            DataTable GetFileName = new DataTable();
            GetFileName = OBJDALIRN.GetIRNReportById(Convert.ToInt32(PK_RM_ID));
            if (GetFileName.Rows.Count > 0)
            {
                ReportName = Convert.ToString(GetFileName.Rows[0]["ReportName"]);
                Report = Convert.ToString(GetFileName.Rows[0]["Report"]);
            }
            if (Report != null)
            {
                var pathdelete = Path.Combine(Server.MapPath("~/IRNReports/"), Report);
                if (System.IO.File.Exists(pathdelete))
                {
                    System.IO.File.Delete(pathdelete);
                }
            }
            string Report_No = Convert.ToString(ReportName);
            string ReportName1 = Convert.ToString(ReportName);

            string[] RepNo = ReportName1.Split('v');
            int prevcnt = 0;
            string XYZNo = string.Empty;
            string RevNo = string.Empty;
            int No = Convert.ToInt32(RepNo[1]);
            if (No != 0)
            {
                prevcnt = No - 1;
                RevNo = Convert.ToString(No);
                XYZNo = Convert.ToString(RepNo[0]) + "v" + " " + Convert.ToString(prevcnt);
            }
            else
            {
                RevNo = Convert.ToString("-");
                XYZNo = Convert.ToString("-");
            }




            try
            {
                if (Convert.ToInt32(PK_IVR_ID) != 0)
                {
                    DataTable ItemDescriptionDashBoard1 = new DataTable();
                    DataTable RefranceDocumentsDashBoard1 = new DataTable();
                    DataTable InspectionActivitesDashBoard1 = new DataTable();
                    DataTable DocumentsReviewBoard1 = new DataTable();
                    DataTable EquipmentDetailsBoard1 = new DataTable();
                    DataSet DSJobMasterByQtId1 = new DataSet();
                    DataTable ReportDashBoard1 = new DataTable();
                    DataTable CostSheetDashBoard1 = new DataTable();
                    int count = 0;
                    DataTable ImageReportDashBoard1 = new DataTable();
                    List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();
                    List<ItemDescriptionModel> lstCompanyDashBoard1 = new List<ItemDescriptionModel>();
                    List<ReferenceDocumentsModel> RefranceDocuments1 = new List<ReferenceDocumentsModel>();
                    List<InspectionActivitiesModel> InspectionDocuments1 = new List<InspectionActivitiesModel>();
                    List<DocumentRevieweModel> DocumentReview1 = new List<DocumentRevieweModel>();
                    List<EquipmentDetailsModel> EquipmentDetails1 = new List<EquipmentDetailsModel>();
                    List<ReportModel> ReportDashboard1 = new List<ReportModel>();

                    //DataTable SubDashBoard = new DataTable();
                    //int count1 = 0;
                    //int count2 = 0;
                    //string SrNo = null;
                    //SubDashBoard = OBJDALIRN.GetReportBySubjob_Id(SubJob_No);
                    //if (SubDashBoard.Rows.Count >= 0)
                    //{
                    //    count1 = SubDashBoard.Rows.Count;
                    //    count2 = count1 - 1;
                    //}
                    //SrNo = Convert.ToString(count1);

                    //added by shrutika salve on 16/06/2023
                    string Result = "";
                    if (PK_IVR_ID != 0 || PK_IVR_ID != null)
                    {

                        //Result = OBJDALIRN.UpdateDownloadDate(PK_IVR_ID);
                    }


                    ReportModel RM = new ReportModel();
                    string Result1 = "";
                    if (Convert.ToInt32(PK_IVR_ID) != 0)
                    {
                        int i = 0;
                        int J = 0;
                        int K = 0;
                        int L = 0;
                        int M = 0;
                        int N = 0;
                        #region 
                        DSJobMasterByQtId1 = OBJDALIRN.EditInspectionVisitReportByPKCallID(Convert.ToInt32(PK_IVR_ID));

                        if (DSJobMasterByQtId1.Tables[0].Rows.Count > 0)
                        {
                            ObjModelVisitReport.Branch = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Branch"]);
                            ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sap_And_Controle_No"]);
                            ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["PK_IVR_ID"]);
                            ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Project_Name_Location"]);
                            ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Address_Of_Inspection"]);
                            ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["End_user_Name"]);
                            ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Vendor_Name_Location"]);
                            ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Notification_Name_No_Date"]);
                            ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Date_Of_Inspection"]);
                            ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Client_Name"]);
                            ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
                            ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
                            ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Po_No"]);
                            ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sub_Vendor_Name"]);
                            ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Po_No_SubVendor"]);
                            ObjModelVisitReport.Conclusion = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Conclusion"]);
                            ObjModelVisitReport.Waivers = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Waivers"]);
                            ObjModelVisitReport.ReviseReason = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["ReviseReason"]);
                            int dvmdr1 = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["DVendor"]);
                            ObjModelVisitReport.vendot = Convert.ToBoolean(dvmdr1);
                            int dclnt1 = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["DClient"]);
                            ObjModelVisitReport.client = Convert.ToBoolean(dclnt1);
                            int dtuv1 = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["DTUV"]);
                            ObjModelVisitReport.TUV = Convert.ToBoolean(dtuv1);
                            ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Pending_Activites"]);
                            ObjModelVisitReport.Identification_Of_Inspected = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Identification_Of_Inspected"]);
                            ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Areas_Of_Concerns"]);
                            ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Non_Conformities_raised"]);
                            //ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Signatures"]);
                            ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Type"]);
                            ObjModelVisitReport.Report_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Report_No"]);
                            ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Signature"]);
                            ObjModelVisitReport.Name = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["LastName"]);
                            ObjModelVisitReport.ReportCreatedDate = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["CreatedDate"]);
                            int Inspection_records = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Inspection_records"]);
                            ObjModelVisitReport.Inspection_records = Convert.ToBoolean(Inspection_records);
                            int Inspection_Photo = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Inspection_Photo"]);
                            ObjModelVisitReport.Inspection_Photo = Convert.ToBoolean(Inspection_Photo);
                            int Other_Specify = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Other_Specify"]);
                            ObjModelVisitReport.Other_Specify = Convert.ToBoolean(Other_Specify);
                            ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["SubJob_No"]);
                            ObjModelVisitReport.Sub_Order_Status = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sub_Order_Status"]);
                            ObjModelVisitReport.OrderStatus = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["OrderStatus"]);
                            ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Type"]);
                            int POTotalCheckBox = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["POTotalCheckBox"]);
                            ObjModelVisitReport.POTotalCheckBox = Convert.ToBoolean(POTotalCheckBox);
                            //ObjModelVisitReport.PO_QuantityTotal = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["PO_QuantityTotal"]);
                            //ObjModelVisitReport.Offered_QuantityTotal = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Offered_QuantityTotal"]);
                            //ObjModelVisitReport.Accepted_QuantityTotal = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Accepted_QuantityTotal"]);
                            //ObjModelVisitReport.Cumulative_Accepted_QtyTotal = Convert.ToInt32(DSJobMasterByQtId1.Tables[0].Rows[0]["Cumulative_Accepted_QtyTotal"]);

                            ObjModelVisitReport.PO_QuantityTotal = Convert.ToDouble(DSJobMasterByQtId1.Tables[0].Rows[0]["PO_QuantityTotal"]);
                            ObjModelVisitReport.Offered_QuantityTotal = Convert.ToDouble(DSJobMasterByQtId1.Tables[0].Rows[0]["Offered_QuantityTotal"]);
                            ObjModelVisitReport.Accepted_QuantityTotal = Convert.ToDouble(DSJobMasterByQtId1.Tables[0].Rows[0]["Accepted_QuantityTotal"]);
                            ObjModelVisitReport.Cumulative_Accepted_QtyTotal = Convert.ToDouble(DSJobMasterByQtId1.Tables[0].Rows[0]["Cumulative_Accepted_QtyTotal"]);

                            ObjModelVisitReport.UnitNameOnPDF = Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Unit"]);
                        }
                        #endregion
                        #region  item Description
                        ItemDescriptionDashBoard1 = OBJDALIRN.GetitemDescription(Convert.ToInt32(PK_IVR_ID));
                        if (ItemDescriptionDashBoard1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in ItemDescriptionDashBoard1.Rows)
                            {
                                lstCompanyDashBoard1.Add(
                                    new ItemDescriptionModel
                                    {
                                        PK_ItemD_Id = Convert.ToInt32(dr["PK_ItemD_Id"]),
                                        Po_Item_No = Convert.ToString(dr["Po_Item_No"]),
                                        ItemCode_Description = Convert.ToString(dr["ItemCode_Description"]),
                                        Po_Quantity = Convert.ToString(dr["Po_Quantity"]),
                                        Offered_Quantity = Convert.ToString(dr["Offered_Quantity"]),
                                        Item_Code = Convert.ToString(dr["Item_Code"]),
                                        Accepted_Quantity = Convert.ToString(dr["Accepted_Quantity"]),
                                        Cumulative_Accepted_Qty = Convert.ToString(dr["Cumulative_Accepted_Qty"]),
                                        Unit = Convert.ToString(dr["Unit"]),
                                    });
                            }
                        }
                        #endregion
                        #region Reference Documents
                        RefranceDocumentsDashBoard1 = OBJDALIRN.GetReferenceDocuments(Convert.ToInt32(PK_IVR_ID));
                        if (RefranceDocumentsDashBoard1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in RefranceDocumentsDashBoard1.Rows)
                            {
                                RefranceDocuments1.Add(
                                    new ReferenceDocumentsModel
                                    {
                                        Document_No = Convert.ToString(dr["Document_No"]),
                                        Document_Name = Convert.ToString(dr["Document_Name"]),
                                        Approval_Status = Convert.ToString(dr["Approval_Status"]),
                                        PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                                        VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"]),
                                    });
                            }
                        }
                        #endregion
                        #region Inspection Activities
                        InspectionActivitesDashBoard1 = OBJDALIRN.GetInspectionActivities(Convert.ToInt32(PK_IVR_ID));
                        if (InspectionActivitesDashBoard1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in InspectionActivitesDashBoard1.Rows)
                            {
                                InspectionDocuments1.Add(
                                    new InspectionActivitiesModel
                                    {
                                        Stages_Witnessed = Convert.ToString(dr["Stages_Witnessed"]),
                                        PK_IA_ID = Convert.ToInt32(dr["PK_IA_ID"]),
                                    });
                            }
                        }
                        #endregion
                        #region Documents Review
                        DocumentsReviewBoard1 = OBJDALIRN.GetDocumentRevieweModelByCall_Id(Convert.ToInt32(PK_IVR_ID));
                        if (DocumentsReviewBoard1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in DocumentsReviewBoard1.Rows)
                            {
                                DocumentReview1.Add(
                                    new DocumentRevieweModel
                                    {
                                        Description = Convert.ToString(dr["Description"]),
                                        PK_DR_ID = Convert.ToInt32(dr["PK_DR_ID"]),
                                    });
                            }
                        }
                        #endregion
                        #region Report Count
                        ReportDashBoard1 = OBJDALIRN.GetReportByCall_Id(Convert.ToInt32(PK_IVR_ID));
                        if (ReportDashBoard1.Rows.Count > 0)
                        {
                            count = ReportDashBoard1.Rows.Count;
                        }
                        string countNo = Convert.ToString(count);
                        #endregion
                        #region Save to Pdf Code 
                        #region Get Date Of Inspection & Inspector Name
                        DataSet dsIN = new DataSet();
                        int z = 0;
                        string InspectorName = "";
                        string Adate = string.Empty;
                        string Date = "";
                        string InrName = string.Empty;

                        List<string> lstInspector = new List<string>();
                        List<string> lstDates = new List<string>();

                        string InsList = string.Empty;
                        dsIN = OBJDALIRN.GetInspectionName(Convert.ToInt32(PK_IVR_ID));
                        if (dsIN.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsIN.Tables[0].Rows)
                            {
                                InspectorName = Convert.ToString(dr["InspectorName"]);
                                lstInspector.Add(InspectorName);
                            }
                            InsList = "On behalf of " + string.Join(",", lstInspector);
                        }
                        else
                        {
                            InsList = "";
                        }
                        if (dsIN.Tables[1].Rows.Count > 0)
                        {
                            InrName = dsIN.Tables[1].Rows[0]["InspectorName"].ToString();
                        }
                        else
                        {
                            InrName = "";
                        }
                        if (dsIN.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsIN.Tables[2].Rows)
                            {
                                Adate = Convert.ToString(dr["Actual_Visit_Date"]);
                                lstDates.Add(Adate);
                            }
                            Date = string.Join(",", lstDates);

                            if (Date.Length > 150)
                            {
                                Date = CommonControl.Wrap(Date, 100);
                            }

                        }
                        else
                        {
                            Date = "";
                        }
                        #endregion
                        SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
                        System.Text.StringBuilder strs = new System.Text.StringBuilder();
                        string body = string.Empty;

                        string ItemDescriptioncontent = "";
                        string ReferenceDocumentscontent = "";
                        string InspectionDocumentsContent = "";
                        string DocumentreviewContent = "";
                        //string EquipmentDetailscontent = "";

                        string check1 = "";
                        string check2 = "";
                        string dvenr11 = string.Empty;
                        string dclnt11 = string.Empty;
                        string dtuv11 = string.Empty;

                        using (StreamReader reader = new StreamReader(Server.MapPath("~/inspection-certificate.html")))
                        {
                            body = reader.ReadToEnd();
                        }

                        body = body.Replace("[SapAndControle_No]", ObjModelVisitReport.SubJob_No + "<br>" + "(" + Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sap_no"]) + ")");
                        body = body.Replace("[Branch]", ObjModelVisitReport.Branch);
                        body = body.Replace("[NotificationNameNo_Date]", ObjModelVisitReport.Notification_Name_No_Date);
                        // body = body.Replace("[DateOfInspection]", ObjModelVisitReport.Date_Of_Inspection);
                        body = body.Replace("[DateOfInspection]", Date);
                        body = body.Replace("[InspectorNames]", InsList);
                        //body = body.Replace("[ReviseReason]", ObjModelVisitReport.ReviseReason);
                        if (ObjModelVisitReport.ReviseReason != "-" && ObjModelVisitReport.ReviseReason != "")
                        {
                            body = body.Replace("[ReviseReason]", "Reason for Revise - " + Convert.ToString(ObjModelVisitReport.ReviseReason));
                        }
                        else
                        {
                            body = body.Replace("[ReviseReason]", "-");
                        }
                        body = body.Replace("[ProjectNameLocation]", ObjModelVisitReport.Project_Name_Location);
                        body = body.Replace("[AddressOfInspection]", ObjModelVisitReport.Address_Of_Inspection);
                        body = body.Replace("[ClientName]", /*ObjModelVisitReport.Client_Name*/Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Client_NameWithoutId"]));
                        body = body.Replace("[Enduser_Name]", ObjModelVisitReport.End_user_Name);
                        body = body.Replace("[DECPMCEPC_Name]", ObjModelVisitReport.DEC_PMC_EPC_Name);
                        body = body.Replace("[DECPMCEPCAssignment_No]", ObjModelVisitReport.DEC_PMC_EPC_Assignment_No);
                        body = body.Replace("[VendorNameLocation]", /*ObjModelVisitReport.Vendor_Name_Location*/Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["VendorNameWithoutId"]));
                        body = body.Replace("[PoNo]", ObjModelVisitReport.Po_No);
                        body = body.Replace("[SubVendorName]", Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["Sub_Vendor_NameWithoutId"]) /*ObjModelVisitReport.Sub_Vendor_Name*/);
                        body = body.Replace("[PoNoSubVendor]", ObjModelVisitReport.Po_No_SubVendor);
                        body = body.Replace("[Conclusion]", ObjModelVisitReport.Conclusion);
                        body = body.Replace("[PendingActivites]", ObjModelVisitReport.Pending_Activites);
                        body = body.Replace("[IdentificationOfInspected]", ObjModelVisitReport.Identification_Of_Inspected);
                        body = body.Replace("[AreasOfConcerns]", ObjModelVisitReport.Areas_Of_Concerns);
                        body = body.Replace("[NonConformitiesraised]", ObjModelVisitReport.Non_Conformities_raised);
                        // body = body.Replace("[Name]", ObjModelVisitReport.Name);
                        body = body.Replace("[Name]", InrName);

                        body = body.Replace("[date]", ObjModelVisitReport.ReportCreatedDate);

                        body = body.Replace("[Waivers]", ObjModelVisitReport.Waivers);

                        body = body.Replace("[RevisionNo]", ReportName1);
                        if (ObjModelVisitReport.vendot == true)
                        {
                            dvenr11 = "<span><input type='checkbox' checked> Vendor / Sub Vendor</span>";
                            //"<span><input type='checkbox' checked> Vendor / Sub Vendor</span>"
                        }
                        else
                        {
                            dvenr11 = "<span><input type='checkbox'> Vendor / Sub Vendor</span>";
                            //"<span><input type='checkbox'> Vendor / Sub Vendor</span>";
                        }

                        if (ObjModelVisitReport.client == true)
                        {
                            dclnt11 = "<span><input type='checkbox' checked> TUVI Client / End User</span>";
                            //"<span><input type='checkbox' checked> TUVI Client / End User</span>";
                        }
                        else
                        {
                            dclnt11 = "<span><input type='checkbox'> TUVI Client / End User</span>";
                            //dvenr11 = "<span><input type='checkbox'> TUVI Client / End User</span>";
                        }

                        if (ObjModelVisitReport.TUV == true)
                        {
                            dtuv11 = "<span><input type='checkbox' checked> TUV Executing Branch / TUV Originating Branch</span>";
                        }
                        else
                        {
                            dtuv11 = "<span><input type='checkbox'> TUV Executing Branch / TUV Originating Branch</span>";
                        }
                        if (ObjModelVisitReport.OrderStatus == "Complete")
                        {
                            //check1 = "<td width='5%' align='center' style='border-right-width: 0px;'><span><input type='checkbox'  value=" + ObjModelVisitReport.Kick_Off_Pre_Inspection + " checked></span></td>";
                            check1 = "<td width='20%'><span><strong><label style='cursor:pointer;font-size:13px;'>Order status:</label></strong></span></td><td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox'>Incomplete</label></span></td>";
                        }
                        else if (ObjModelVisitReport.OrderStatus == "Incomplete")
                        {
                            //check1 = "<td width='5%' align='center' style='border-right-width: 0px;'><span><input type='checkbox'  value=" + ObjModelVisitReport.Kick_Off_Pre_Inspection + "></span></td>";
                            check1 = "<td width='20%'><span><strong><label style='cursor:pointer;font-size:13px;'>Order status:</label></strong></span></td><td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Incomplete</label></span></td>";
                        }
                        else
                        {
                            check1 = "<td width='20%'><span><strong><label style='cursor:pointer;font-size:13px;'>Order status:</label></strong></span></td><td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Incomplete</label></span></td>";
                        }
                        if (ObjModelVisitReport.Sub_Order_Status == "Complete")
                        {
                            check2 = "<td width='20%'><span><strong><label style='cursor:pointer;font-size:13px;'>Sub order status:</label></strong></span></td><td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox'>Incomplete</label></span></td>";
                        }
                        else if (ObjModelVisitReport.Sub_Order_Status == "InComplete")
                        {
                            check2 = "<td width='20%'><span><strong><label style='cursor:pointer;font-size:13px;'>Sub order status:</label></strong></span></td><td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Incomplete</label></span></td>";
                        }
                        else
                        {
                            check2 = "<td width='20%'><span><strong><label style='cursor:pointer;font-size:13px;'>Sub order status:</label></strong></span></td><td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Incomplete</label></span></td>";
                        }
                        //foreach (ItemDescriptionModel v in lstCompanyDashBoard1)
                        //{
                        //    i = i + 1;
                        //    ItemDescriptioncontent += "<tr><td style='border: 1px solid #b5b5b5;' width='5%' align='center'><span>" + Convert.ToString(v.Po_Item_No) +')'+ "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Item_Code + " </span></td><td style='border: 1px solid #b5b5b5;white-space: pre-line;' width='35%'><span>" + v.ItemCode_Description + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Unit + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Po_Quantity + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Offered_Quantity + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Accepted_Quantity + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + v.Cumulative_Accepted_Qty + "</span></td></tr>";
                        //}

                        //foreach (ReferenceDocumentsModel v in RefranceDocuments1)
                        //{
                        //    J = J + 1;
                        //    //ReferenceDocumentscontent += "<tr><td style='border: 1px solid #b5b5b5;' width='5%' align='center'> " + J + ')' + " </td><td style='border: 1px solid #b5b5b5;' width='25%' white-space: pre-line;>" + Convert.ToString(v.Document_Name) + "</td><td style='border: 1px solid #b5b5b5;' width='25%' white-space: pre-line;>" + v.Document_No + " </td><td style='border: 1px solid #b5b5b5;' width='25%' white-space: pre-line;>" + v.VendorDocumentNumber + "</td><td style='border: 1px solid #b5b5b5;' width='25%'>" + v.Approval_Status + "</td></tr>";
                        //    //ReferenceDocumentscontent += "<tr><td style='border: 1px solid #b5b5b5;vertical-align:top; text-align:center;' > " + J + ')' + " </td><td style='border: 1px solid #b5b5b5; white-space: pre-line;vertical-align:top;' >" + Convert.ToString(v.Document_Name) + "</td><td style='border: 1px solid #b5b5b5;white-space:pre-line;vertical-align:top;' >" + v.Document_No + " </td><td style='border: 1px solid #b5b5b5;white-space:pre-line;vertical-align:top;' >" + v.VendorDocumentNumber + "</td><td style='border: 1px solid #b5b5b5;white-space: pre-line;vertical-align:top;' >" + v.Approval_Status + "</td></tr>";
                        //    ReferenceDocumentscontent += "<tr><td  style='border: 1px solid #b5b5b5;vertical-align:top; text-align:center;'  <span>" + J + ')' + "</span> </td><td width='25%' style='border: 1px solid #b5b5b5; white-space: pre-line;vertical-align:top;' >" + Convert.ToString(v.Document_Name) + "</td><td width='25%' style='border: 1px solid #b5b5b5;white-space:pre-line;vertical-align:top;' >" + v.Document_No + " </td><td width='25%' style='border: 1px solid #b5b5b5;white-space:pre-line;vertical-align:top;' >" + v.VendorDocumentNumber + "</td><td width='25%' style='border: 1px solid #b5b5b5;white-space: pre-line;vertical-align:top;' >" + v.Approval_Status + "</td></tr>";
                        //}

                        foreach (ItemDescriptionModel v in lstCompanyDashBoard1)
                        {
                            i = i + 1;

                            if (i == lstCompanyDashBoard1.Count)
                                ItemDescriptioncontent += "<tr><td style = 'border:1px solid #000000;vertical-align:top; text-align:center;font-size:14px;' width = '5%' align = 'center' ><span> " + Convert.ToString(v.Po_Item_No) /*+ ''*/ + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;vertical-align:top; text-align:left;font-size:14px;' width = '10%' ><span> " + v.Item_Code + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;vertical-align:top; text-align:left;font-size:14px;white-space: pre-line;' width = '35%' ><span> " + v.ItemCode_Description + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;vertical-align:top; text-align:left;font-size:14px;' width = '10%' ><span> " + v.Unit + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;vertical-align:top; text-align:center;font-size:14px;' width = '10%' ><span> " + v.Po_Quantity + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;vertical-align:top; text-align:center;font-size:14px;' width = '10%' ><span> " + v.Offered_Quantity + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;vertical-align:top; text-align:center;font-size:14px;' width = '10%' ><span> " + v.Accepted_Quantity + " </span></td><td style = 'border:1px solid #000000;border-left-width: 0px;vertical-align:top; text-align:center;font-size:14px;' width = '10%' ><span> " + v.Cumulative_Accepted_Qty + " </span></td></tr>";
                            else
                                ItemDescriptioncontent += "<tr><td style = 'border:1px solid #000000;border-bottom-width: 0px;vertical-align:top; text-align:center;font-size:14px;' width = '5%' align = 'center' ><span> " + Convert.ToString(v.Po_Item_No) /*+ ''*/ + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;vertical-align:top; text-align:left;font-size:14px;' width = '10%' ><span> " + v.Item_Code + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;vertical-align:top; text-align:left;font-size:14px;white-space: pre-line;' width = '35%' ><span> " + v.ItemCode_Description + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;vertical-align:top; text-align:left;font-size:14px;' width = '10%' ><span> " + v.Unit + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;vertical-align:top; text-align:center;font-size:14px;' width = '10%' ><span> " + v.Po_Quantity + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;vertical-align:top; text-align:center;font-size:14px;' width = '10%' ><span> " + v.Offered_Quantity + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;vertical-align:top; text-align:center;font-size:14px;' width = '10%' ><span> " + v.Accepted_Quantity + " </span></td><td style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;vertical-align:top; text-align:center;font-size:14px;' width = '10%' ><span> " + v.Cumulative_Accepted_Qty + " </span></td></tr>";
                        }
                        if (lstCompanyDashBoard1.Count == 0)
                        {
                            ItemDescriptioncontent += "<tr><td style = 'border:0px solid #000000;border-top-width: 1px;vertical-align:top; text-align:center;font-size:14px;' width = '5%' align = 'center' > </td><td style = 'border:0px solid #000000;border-top-width: 1px;border-left-width: 0px;vertical-align:top; text-align:left;font-size:14px;' width = '10%' ></td><td style = 'border:0px solid #000000;border-left-width: 0px;vertical-align:top; text-align:left;border-top-width: 1px;font-size:14px;' width = '35%' ></td><td style = 'border:0px solid #000000;border-top-width: 1px;border-left-width: 0px;vertical-align:top; text-align:left;font-size:14px;' width = '10%' ></td><td style = 'border:0px solid #000000;border-left-width: 0px;vertical-align:top;border-bottom-width: 0px; text-align:center;border-top-width: 1px;font-size:14px;' width = '10%' ></td><td style = 'border:0px solid #000000;border-left-width: 0px;vertical-align:top;border-top-width: 1px; text-align:center;font-size:14px;' width = '10%' ></td><td style = 'border:0px solid #000000;border-left-width: 0px;vertical-align:top; text-align:center;border-top-width: 1px;font-size:14px;' width = '10%' ></td><td style = 'border:0px solid #000000;border-top-width: 1px;border-left-width: 0px;vertical-align:top; text-align:center;font-size:14px;' width = '10%' ></td></tr>";
                        }

                        foreach (ReferenceDocumentsModel v in RefranceDocuments1)
                        {
                            J = J + 1;
                            if (J == RefranceDocuments1.Count)
                                ReferenceDocumentscontent += "<tr><td width = '4%' style = 'border:1px solid #000000;vertical-align:top; text-align:center;font-size:14px;' <span> " + J /*+ ''*/ + " </span> </td><td width = '15%' style = 'border:1px solid #000000;border-left-width: 0px; white-space: pre-line;vertical-align:top; text-align:left;font-size:14px;' > " + Convert.ToString(v.Document_Name) + " </td><td width = '33%' style = 'border:1px solid #000000;border-left-width: 0px;white-space:pre-line;vertical-align:top; text-align:left;font-size:14px;' > " + v.Document_No + " </td><td width = '33%' style = 'border:1px solid #000000;border-left-width: 0px;white-space:pre-line;vertical-align:top; text-align:left;font-size:14px;' > " + v.VendorDocumentNumber + " </td><td width = '15%' style = 'border:1px solid #000000;border-left-width: 0px;white-space: pre-line;vertical-align:top; text-align:left;font-size:14px;' > " + v.Approval_Status + " </td></tr> ";
                            else
                                ReferenceDocumentscontent += "<tr><td width = '4%' style = 'border:1px solid #000000;border-bottom-width: 0px;vertical-align:top; text-align:center;font-size:14px;' <span> " + J /*+ ''*/ + " </span> </td><td width = '15%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px; white-space: pre-line;vertical-align:top; text-align:left;font-size:14px;' > " + Convert.ToString(v.Document_Name) + " </td><td width = '33%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;white-space:pre-line;vertical-align:top; text-align:left;font-size:14px;font-size:14px;' > " + v.Document_No + " </td><td width = '33%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;white-space:pre-line;vertical-align:top; text-align:left;font-size:14px;' > " + v.VendorDocumentNumber + " </td><td width = '15%' style = 'border:1px solid #000000;border-bottom-width: 0px;border-left-width: 0px;white-space: pre-line;vertical-align:top; text-align:left;font-size:14px;' > " + v.Approval_Status + " </td></tr> ";
                        }


                        foreach (InspectionActivitiesModel v in InspectionDocuments1)
                        {
                            K = K + 1;
                            InspectionDocumentsContent += "<tr><td width='5%' style = 'vertical-align:top; text-align:center;font-size:14px;' align='center'><span> " + K + ')' + " </span></td><td width='95%' style='white-space: pre-line;vertical-align:top; text-align:left;font-size:14px;'><span>" + Convert.ToString(v.Stages_Witnessed) + "</span></td></tr>";
                        }
                        foreach (DocumentRevieweModel v in DocumentReview1)
                        {
                            L = L + 1;
                            DocumentreviewContent += "<tr><td width='5%' style = 'vertical-align:top; text-align:center;font-size:14px;' align='center'><span> " + L + ')' + " </span></td><td width='95%' style='white-space: pre-line;vertical-align:top; text-align:left;font-size:14px;'><span>" + Convert.ToString(v.Description) + "</span></td></tr>";
                        }
                        body = body.Replace("[ItemDescriptionContent]", ItemDescriptioncontent);
                        body = body.Replace("[ReferenceDocumentsContent]", ReferenceDocumentscontent);
                        body = body.Replace("[InspectionDocumentsContent]", InspectionDocumentsContent);
                        body = body.Replace("[DocumentreviewContent]", DocumentreviewContent);
                        body = body.Replace("[Stamp]", "https://tiimes.tuv-india.com/Stamp.png");

                        if (ObjModelVisitReport.Signatures != null)
                        {
                            body = body.Replace("[Signature]", "https://tiimes.tuv-india.com/Content/Sign/" + ObjModelVisitReport.Signatures + "");
                        }
                        else
                        {
                            body = body.Replace("[Signature]", "-");
                        }

                        if (Convert.ToString(DSJobMasterByQtId1.Tables[0].Rows[0]["JobType"]) == "SubSub Job")
                        {
                            body = body.Replace("[Checkbox2]", check2);
                            body = body.Replace("[Checkbox1]", "");
                        }
                        else
                        {
                            body = body.Replace("[Checkbox1]", check1);
                            body = body.Replace("[Checkbox2]", "");
                        }
                        //body = body.Replace("[Checkbox1]", check1);
                        //body = body.Replace("[Checkbox2]", check2);
                        body = body.Replace("[DVendor]", dvenr11);
                        body = body.Replace("[DClient]", dclnt11);
                        body = body.Replace("[DTUV]", dtuv11);
                        body = body.Replace("[PreviousNo]", XYZNo);
                        if (RevNo == "-")
                        {
                            body = body.Replace("[RevNo]", "0");
                        }
                        else
                        {
                            body = body.Replace("[RevNo]", RevNo);
                        }

                        string po = Convert.ToString(ObjModelVisitReport.POTotalCheckBox);
                        body = body.Replace("[POTotalCheckBox]", Convert.ToString(ObjModelVisitReport.POTotalCheckBox));
                        body = body.Replace("[PO_QuantityTotal]", Convert.ToString(ObjModelVisitReport.PO_QuantityTotal));
                        body = body.Replace("[Offered_QuantityTotal]", Convert.ToString(ObjModelVisitReport.Offered_QuantityTotal));
                        body = body.Replace("[Accepted_QuantityTotal]", Convert.ToString(ObjModelVisitReport.Accepted_QuantityTotal));
                        body = body.Replace("[Cumulative_Accepted_QtyTotal]", Convert.ToString(ObjModelVisitReport.Cumulative_Accepted_QtyTotal));

                        if (po == "True")
                        {
                            body = body.Replace("[POTotal]", "<tr><td style='border: 1px solid ;font-size:14px;border-top-width: 0px;vertical-align: text-top;text-align:center;' width='5%' align='center'><span>" + "" + "</span></td><td style='border: 1px solid;font-size:14px;border-top-width: 0px;vertical-align: text-top;text-align:center;border-left-width: 0px;' width='10%'><span>" + "" + " </span></td><td style='border: 1px solid ;white-space: pre-line;font-size:14px;border-top-width: 0px;vertical-align: text-top;text-align:right;border-left-width: 0px;' width='35%'><span><b>" + "Total" + "</b></span></td><td style='border: 1px solid;font-size:14px;border-left-width: 0px;border-top-width: 0px;' width='10%'><span>" + ObjModelVisitReport.UnitNameOnPDF + "</span></td><td style='border: 1px solid;font-size:14px;border-top-width: 0px;vertical-align: text-top;text-align:center;border-left-width: 0px;' width='10%'><span>" + ObjModelVisitReport.PO_QuantityTotal + "</span></td><td style='border: 1px solid;font-size:14px;border-top-width: 0px;vertical-align: text-top;text-align:center;border-left-width: 0px;' width='10%'><span>" + ObjModelVisitReport.Offered_QuantityTotal + "</span></td><td style='border: 1px solid;font-size:14px;border-top-width: 0px;vertical-align: text-top;text-align:center;border-left-width: 0px;' width='10%'><span>" + ObjModelVisitReport.Accepted_QuantityTotal + "</span></td><td style='border: 1px solid;font-size:14px;border-top-width: 0px;vertical-align: text-top;text-align:center;border-left-width: 0px;' width='10%'><span>" + ObjModelVisitReport.Cumulative_Accepted_QtyTotal + "</span></td></tr>");
                            //ItemDescriptioncontent += "<tr><td style='border: 1px solid #b5b5b5;' width='5%' align='center'><span>" + "" + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + "" + " </span></td><td style='border: 1px solid #b5b5b5;white-space: pre-line;' width='35%'><span>" + "" + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + "Total" + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.PO_QuantityTotal + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.Offered_QuantityTotal + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.Accepted_QuantityTotal + "</span></td><td style='border: 1px solid #b5b5b5;' width='10%'><span>" + ObjModelVisitReport.Cumulative_Accepted_QtyTotal + "</span></td></tr>";

                        }
                        else
                        {
                            body = body.Replace("[POTotal]", "");

                        }
                        strs.Append(body);
                        PdfPageSize pageSize = PdfPageSize.A4;
                        PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
                        HtmlToPdf converter = new HtmlToPdf();
                        converter.Options.MaxPageLoadTime = 120;  //=========================5-Aug-2019
                        converter.Options.PdfPageSize = pageSize;
                        converter.Options.PdfPageOrientation = pdfOrientation;


                        string _Header = string.Empty;
                        string _footer = string.Empty;

                        // for Report header by abel
                        StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/Inspection_Certificate_Header.html"));
                        _Header = _readHeader_File.ReadToEnd();

                        _Header = _Header.Replace("[RevisionNo]", ReportName1);
                        _Header = _Header.Replace("[Logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png"); // change123 once pulished on server

                        StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/inspection-certificate-footer.html")); // Footer is used from IVR as it same and commented in pdf template.
                        _footer = _readFooter_File.ReadToEnd();

                        // header settings
                        converter.Options.DisplayHeader = true || true || true;
                        converter.Header.DisplayOnFirstPage = true;
                        converter.Header.DisplayOnOddPages = true;
                        converter.Header.DisplayOnEvenPages = true;
                        converter.Header.Height = 75;

                        PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
                        headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                        converter.Header.Add(headerHtml);

                        // footer settings
                        converter.Options.DisplayFooter = true || true || true;
                        converter.Footer.DisplayOnFirstPage = true;
                        converter.Footer.DisplayOnOddPages = true;
                        converter.Footer.DisplayOnEvenPages = true;
                        //converter.Footer.Height = 130; //8/Oct/2021
                        //converter.Footer.Height = 120;
                        converter.Footer.Height = 105;
                        PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
                        footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                        converter.Footer.Add(footerHtml);

                        //end abel code

                        #region Footer Code
                        //PdfTextSection text = new PdfTextSection(-40, 80, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 9));
                        PdfTextSection text = new PdfTextSection(510, 77, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 9));
                        // text.HorizontalAlign = PdfTextHorizontalAlign.Right;
                        converter.Footer.Add(text);



                        #endregion

                        RM.PK_RM_ID = Convert.ToInt32(PK_RM_ID);

                        //string ReportName = string.Empty;
                        //string Report = string.Empty;
                        //DataTable GetFileName = new DataTable();
                        //GetFileName = OBJDALIRN.GetIRNReportById(Convert.ToInt32(RM.PK_RM_ID));
                        //if (GetFileName.Rows.Count > 0)
                        //{
                        //    ReportName = Convert.ToString(GetFileName.Rows[0]["ReportName"]);
                        //    Report = Convert.ToString(GetFileName.Rows[0]["Report"]);
                        //}
                        //if (Report != null)
                        //{
                        //    var pathdelete = Path.Combine(Server.MapPath("~/IRNReports/"), Report);
                        //    if (System.IO.File.Exists(pathdelete))
                        //    {
                        //        System.IO.File.Delete(pathdelete);
                        //    }
                        //}
                        //string test = Report;





                        PdfDocument doc = converter.ConvertHtmlString(body);


                        #region watermark test


                        string imgFile = Server.MapPath("/t2.jpg");
                        // watermark all pages - add a template containing an image 
                        // to the bottom right of the page
                        // the image should repeat on all pdf pages automatically
                        // the template should be rendered behind the rest of the page elements
                        PdfTemplate template = doc.AddTemplate(doc.Pages[0].ClientRectangle); //// 635 * 554 
                                                                                              //PdfTemplate template = doc.AddTemplate(600,600);
                        PdfImageElement img = new PdfImageElement(125, 85, imgFile);
                        //PdfImageElement img = new PdfImageElement(600,1000, imgFile);
                        img.Transparency = 15;

                        //template.Background = true;
                        template.Add(img);

                        #endregion

                        #region signature on each page




                        System.Drawing.Image imgTest = System.Drawing.Image.FromFile(Server.MapPath("~/Content/Sign/" + ObjModelVisitReport.Signatures));
                        Bitmap b = new Bitmap(imgTest);

                        string FileWithoutExt = Path.GetFileNameWithoutExtension(Server.MapPath("~/Content/Sign/" + ObjModelVisitReport.Signatures));

                        System.Drawing.Image NewImage = ResizeImageNew(b, new Size(70, 70));

                        if (System.IO.File.Exists(Server.MapPath("~/Content/Sign/") + FileWithoutExt.ToString() + "_tmp.jpg"))
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/Sign/") + ObjModelVisitReport.Signatures.ToString() + "_tmp.jpg");

                        }
                        // NewImage.Save(Server.MapPath("~/Content/Uploads/Images/") + FileWithoutExt.ToString() + "_tmp.jpg", ImageFormat.Jpeg);
                        NewImage.Save(Server.MapPath("~/Content/Uploads/Images/") + FileWithoutExt.ToString() + "_tmp.jpg", ImageFormat.Png);

                        string FinalImage = Server.MapPath("~/Content/Uploads/Images/") + FileWithoutExt.ToString() + "_tmp.jpg";

                        for (int pageno = 0; pageno < doc.Pages.Count - 1; pageno++)
                        {

                            PdfImageElement Signimg1 = new PdfImageElement(490, 610, FinalImage);
                            PdfTemplate Signtemplate = doc.AddTemplate(50, 50); //// 635 * 554  
                            Signimg1.Transparency = 40;
                            doc.Pages[pageno].Add(Signimg1); //// 635 * 554         

                        }


                        #endregion

                        string path = Server.MapPath("~/IRNReports");



                        doc.Save(path + '\\' + Report);
                        doc.Close();
                        #endregion

                        if (RM.PK_RM_ID != 0)
                        {
                            RM.PK_RM_ID = Convert.ToInt32(PK_RM_ID);
                            RM.Type = "IRN";
                            RM.Status = "1";
                            RM.Report = Report;
                            RM.ReportName = ReportName;
                            RM.PK_CALL_ID = Convert.ToInt32(PK_IVR_ID);
                            RM.VendorName = ObjModelVisitReport.Vendor_Name_Location;
                            RM.ClientName = ObjModelVisitReport.Client_Name;
                            RM.ProjectName = ObjModelVisitReport.Project_Name_Location;
                            RM.SubJob_No = ObjModelVisitReport.SubJob_No;
                            Result1 = OBJDALIRN.InsertUpdateReport(RM);
                            if (Result1 != "" && Result1 != null)
                            {
                                TempData["InsertCompany"] = Result1;
                            }
                        }
                        #region
                        CostSheetDashBoard1 = OBJDALIRN.GetReportByCall_Id(Convert.ToInt32(PK_IVR_ID));
                        if (CostSheetDashBoard1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in CostSheetDashBoard1.Rows)
                            {
                                ReportDashboard1.Add(
                                    new ReportModel
                                    {
                                        ReportName = Convert.ToString(dr["ReportName"]),
                                        Report = Convert.ToString(dr["Report"]),
                                        CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                        PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                    });
                            }
                        }
                        ViewData["CostSheet"] = ReportDashboard1;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {

                string Error = ex.Message.ToString();
                return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = Error });

            }

            //return File("~/IRNReports/"@Model.DownloadPrint, "application/pdf", Server.UrlEncode(""+filename));

            // return RedirectToAction("IRNReports", "InspectionReleaseNote");


            string newpath = Server.MapPath("~/IRNReports/");

            DataSet dsDownloadPrint = new DataSet();

            dsDownloadPrint = OBJDALIRN.DownloadPrint(Convert.ToInt32(PK_IVR_ID));
            if (dsDownloadPrint.Tables[0].Rows.Count > 0)
            {
                ObjModelVisitReport.DownloadPrint = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["Report"]);
                ObjModelVisitReport.Report_No = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["ReportName"]);
                Session["ReportNo"] = Convert.ToString(dsDownloadPrint.Tables[0].Rows[0]["ReportName"]);
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(newpath + @"\" + ObjModelVisitReport.DownloadPrint);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, ObjModelVisitReport.DownloadPrint);


            return RedirectToAction("IRNForm", "InspectionReleaseNote", new { @PK_IVR_ID = PK_IVR_ID, @PK_RM_ID = PK_RM_ID });
        }


        public ActionResult ErrorPage(String Error)
        {
            ReportModel objErr = new ReportModel();
            objErr.Error = Error;
            return View(objErr);
        }

        public ActionResult UpdateIRNReport(int? PK_IVR_ID)  //===========Print Report Code
        {
            DataTable ItemDescriptionDashBoard = new DataTable();
            DataTable RefranceDocumentsDashBoard = new DataTable();
            DataTable InspectionActivitesDashBoard = new DataTable();
            DataTable DocumentsReviewBoard = new DataTable();
            DataTable EquipmentDetailsBoard = new DataTable();
            DataSet DSJobMasterByQtId = new DataSet();
            DataTable ReportDashBoard = new DataTable();
            DataTable CostSheetDashBoard = new DataTable();
            DataSet UpdateReport = new DataSet();
            int count = 0;
            DataTable ImageReportDashBoard = new DataTable();
            List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();
            List<ItemDescriptionModel> lstCompanyDashBoard = new List<ItemDescriptionModel>();
            List<ReferenceDocumentsModel> RefranceDocuments = new List<ReferenceDocumentsModel>();
            List<InspectionActivitiesModel> InspectionDocuments = new List<InspectionActivitiesModel>();
            List<DocumentRevieweModel> DocumentReview = new List<DocumentRevieweModel>();
            List<EquipmentDetailsModel> EquipmentDetails = new List<EquipmentDetailsModel>();
            List<ReportModel> ReportDashboard = new List<ReportModel>();


            ReportModel RM = new ReportModel();
            string Result = "";




            if (PK_IVR_ID != 0 || PK_IVR_ID != null)
            {
                int i = 0;
                int J = 0;
                int K = 0;
                int L = 0;
                int M = 0;
                int N = 0;
                #region 
                DSJobMasterByQtId = OBJDALIRN.EditInspectionVisitReportByPKCallID(PK_IVR_ID);

                if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
                {
                    ObjModelVisitReport.Branch = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Branch"]);
                    ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sap_And_Controle_No"]);
                    ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_IVR_ID"]);
                    ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name_Location"]);
                    ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Address_Of_Inspection"]);
                    ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["End_user_Name"]);
                    ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Name_Location"]);
                    // ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_Call_ID"]);
                    ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);

                    ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date_Of_Inspection"]);
                    ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Client_Name"]);
                    ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
                    ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
                    ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No"]);
                    ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Vendor_Name"]);
                    ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No_SubVendor"]);

                    ObjModelVisitReport.Conclusion = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Conclusion"]);
                    ObjModelVisitReport.Waivers = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Waivers"]);
                    ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Pending_Activites"]);
                    ObjModelVisitReport.Identification_Of_Inspected = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Identification_Of_Inspected"]);
                    ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Areas_Of_Concerns"]);
                    ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Non_Conformities_raised"]);
                    //ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Signatures"]);
                    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Type"]);
                    ObjModelVisitReport.Report_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Report_No"]);

                    ObjModelVisitReport.Signatures = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Signature"]);
                    ObjModelVisitReport.Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["LastName"]);
                    ObjModelVisitReport.ReportCreatedDate = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["CreatedDate"]);

                    int Inspection_records = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Inspection_records"]);
                    ObjModelVisitReport.Inspection_records = Convert.ToBoolean(Inspection_records);

                    int Inspection_Photo = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Inspection_Photo"]);
                    ObjModelVisitReport.Inspection_Photo = Convert.ToBoolean(Inspection_Photo);

                    int Other_Specify = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["Other_Specify"]);
                    ObjModelVisitReport.Other_Specify = Convert.ToBoolean(Other_Specify);

                    ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubJob_No"]);
                    ObjModelVisitReport.Sub_Order_Status = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Order_Status"]);
                    ObjModelVisitReport.OrderStatus = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["OrderStatus"]);

                    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Type"]);

                }
                #endregion


                #region  item Description

                ItemDescriptionDashBoard = OBJDALIRN.GetitemDescription(PK_IVR_ID);
                if (ItemDescriptionDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in ItemDescriptionDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ItemDescriptionModel
                            {
                                PK_ItemD_Id = Convert.ToInt32(dr["PK_ItemD_Id"]),
                                Po_Item_No = Convert.ToString(dr["Po_Item_No"]),
                                ItemCode_Description = Convert.ToString(dr["ItemCode_Description"]),
                                Po_Quantity = Convert.ToString(dr["Po_Quantity"]),
                                Offered_Quantity = Convert.ToString(dr["Offered_Quantity"]),
                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                Item_Code = Convert.ToString(dr["Item_Code"]),
                                Accepted_Quantity = Convert.ToString(dr["Accepted_Quantity"]),
                                Cumulative_Accepted_Qty = Convert.ToString(dr["Cumulative_Accepted_Qty"]),
                                Unit = Convert.ToString(dr["Unit"]),
                            }
                            );
                    }
                }
                #endregion


                #region Reference Documents

                RefranceDocumentsDashBoard = OBJDALIRN.GetReferenceDocuments(PK_IVR_ID);
                if (RefranceDocumentsDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in RefranceDocumentsDashBoard.Rows)
                    {
                        RefranceDocuments.Add(
                            new ReferenceDocumentsModel
                            {
                                Document_No = Convert.ToString(dr["Document_No"]),
                                Document_Name = Convert.ToString(dr["Document_Name"]),
                                Approval_Status = Convert.ToString(dr["Approval_Status"]),
                                PK_RD_ID = Convert.ToInt32(dr["PK_RD_ID"]),
                                VendorDocumentNumber = Convert.ToString(dr["VendorDocumentNumber"]),
                                // PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])

                            }
                            );
                    }
                }
                #endregion


                #region Inspection Activities
                InspectionActivitesDashBoard = OBJDALIRN.GetInspectionActivities(PK_IVR_ID);
                if (InspectionActivitesDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in InspectionActivitesDashBoard.Rows)
                    {
                        InspectionDocuments.Add(
                            new InspectionActivitiesModel
                            {
                                Stages_Witnessed = Convert.ToString(dr["Stages_Witnessed"]),
                                PK_IA_ID = Convert.ToInt32(dr["PK_IA_ID"]),
                                //  PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])

                            }
                            );
                    }
                }
                #endregion

                #region Documents Review
                DocumentsReviewBoard = OBJDALIRN.GetDocumentRevieweModelByCall_Id(Convert.ToInt32(PK_IVR_ID));
                if (DocumentsReviewBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DocumentsReviewBoard.Rows)
                    {
                        DocumentReview.Add(
                            new DocumentRevieweModel
                            {
                                Description = Convert.ToString(dr["Description"]),
                                PK_DR_ID = Convert.ToInt32(dr["PK_DR_ID"]),
                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])

                            }
                            );
                    }
                }
                #endregion

                #region report Count
                ReportDashBoard = OBJDALIRN.GetReportByCall_Id(PK_IVR_ID);
                if (ReportDashBoard.Rows.Count > 0)
                {
                    int counts = ReportDashBoard.Rows.Count;
                    count = counts - 1;
                }
                string countNo = Convert.ToString(count);

                UpdateReport = OBJDALIRN.GetReportByLastId(PK_IVR_ID);
                if (UpdateReport.Tables[0].Rows.Count > 0)
                {
                    RM.PK_RM_ID = Convert.ToInt32(UpdateReport.Tables[0].Rows[0]["PK_RM_ID"]);
                    RM.Report = Convert.ToString(UpdateReport.Tables[0].Rows[0]["Report"]);
                    RM.ReportName = Convert.ToString(UpdateReport.Tables[0].Rows[0]["ReportName"]);
                    //RM.ImageReport = Convert.ToString(UpdateReport.Tables[0].Rows[0]["ImageReport"]);
                }
                #endregion

                #region Save to Pdf Code 

                #region get inspection visit Date
                DataSet dsIN = new DataSet();
                int z = 0;
                string InspectorName = "";
                string Adate = string.Empty;
                string Date = "";
                string InrName = string.Empty;

                List<string> lstInspector = new List<string>();
                List<string> lstDates = new List<string>();

                string InsList = string.Empty;
                dsIN = OBJDALIRN.GetInspectionName(Convert.ToInt32(PK_IVR_ID));
                if (dsIN.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsIN.Tables[0].Rows)
                    {
                        InspectorName = Convert.ToString(dr["InspectorName"]);
                        lstInspector.Add(InspectorName);
                    }
                    InsList = "On behalf of " + string.Join(",", lstInspector);
                }
                else
                {
                    InsList = "";
                }
                if (dsIN.Tables[1].Rows.Count > 0)
                {
                    InrName = dsIN.Tables[1].Rows[0]["InspectorName"].ToString();
                }
                else
                {
                    InrName = "";
                }
                if (dsIN.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsIN.Tables[2].Rows)
                    {
                        Adate = Convert.ToString(dr["Actual_Visit_Date"]);
                        lstDates.Add(Adate);
                    }
                    Date = string.Join(",", lstDates);
                }
                else
                {
                    Date = "";
                }
                #endregion

                SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
                System.Text.StringBuilder strs = new System.Text.StringBuilder();
                string body = string.Empty;

                string ItemDescriptioncontent = "";
                string ReferenceDocumentscontent = "";
                string InspectionDocumentsContent = "";
                string DocumentreviewContent = "";
                //string EquipmentDetailscontent = "";

                string check1 = "";
                string check2 = "";

                using (StreamReader reader = new StreamReader(Server.MapPath("~/inspection-certificate.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("[SapAndControle_No]", ObjModelVisitReport.SubJob_No);
                body = body.Replace("[Branch]", ObjModelVisitReport.Branch);
                body = body.Replace("[NotificationNameNo_Date]", ObjModelVisitReport.Notification_Name_No_Date);
                // body = body.Replace("[DateOfInspection]", ObjModelVisitReport.Date_Of_Inspection);
                body = body.Replace("[DateOfInspection]", Date);
                body = body.Replace("[InspectorNames]", InsList);
                body = body.Replace("[ProjectNameLocation]", ObjModelVisitReport.Project_Name_Location);
                body = body.Replace("[AddressOfInspection]", ObjModelVisitReport.Address_Of_Inspection);
                body = body.Replace("[ClientName]", ObjModelVisitReport.Client_Name);
                body = body.Replace("[Enduser_Name]", ObjModelVisitReport.End_user_Name);
                body = body.Replace("[DECPMCEPC_Name]", ObjModelVisitReport.DEC_PMC_EPC_Name);
                body = body.Replace("[DECPMCEPCAssignment_No]", ObjModelVisitReport.DEC_PMC_EPC_Assignment_No);
                body = body.Replace("[VendorNameLocation]", ObjModelVisitReport.Vendor_Name_Location);
                body = body.Replace("[PoNo]", ObjModelVisitReport.Po_No);
                body = body.Replace("[SubVendorName]", ObjModelVisitReport.Sub_Vendor_Name);
                body = body.Replace("[PoNoSubVendor]", ObjModelVisitReport.Po_No_SubVendor);
                body = body.Replace("[Conclusion]", ObjModelVisitReport.Conclusion);
                body = body.Replace("[PendingActivites]", ObjModelVisitReport.Pending_Activites);
                body = body.Replace("[IdentificationOfInspected]", ObjModelVisitReport.Identification_Of_Inspected);
                body = body.Replace("[AreasOfConcerns]", ObjModelVisitReport.Areas_Of_Concerns);
                body = body.Replace("[NonConformitiesraised]", ObjModelVisitReport.Non_Conformities_raised);
                //body = body.Replace("[Name]", ObjModelVisitReport.Name);
                body = body.Replace("[Name]", InrName);
                body = body.Replace("[date]", ObjModelVisitReport.ReportCreatedDate);
                body = body.Replace("[Waivers]", ObjModelVisitReport.Waivers);
                body = body.Replace("[RevisionNo]", countNo);
                if (ObjModelVisitReport.OrderStatus == "Complete")
                {
                    //check1 = "<td width='5%' align='center' style='border-right-width: 0px;'><span><input type='checkbox'  value=" + ObjModelVisitReport.Kick_Off_Pre_Inspection + " checked></span></td>";
                    check1 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox'>Incomplete</label></span></td>";
                }
                else
                {
                    //check1 = "<td width='5%' align='center' style='border-right-width: 0px;'><span><input type='checkbox'  value=" + ObjModelVisitReport.Kick_Off_Pre_Inspection + "></span></td>";
                    check1 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Incomplete</label></span></td>";
                }
                if (ObjModelVisitReport.Sub_Order_Status == "Complete")
                {
                    check2 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox'>Incomplete</label></span></td>";
                }
                else
                {
                    check2 = "<td width='80%'><span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' >Complete</label></span> &nbsp;<span><label style='cursor:pointer;font-size:13px;'><input type='checkbox' checked>Incomplete</label></span></td>";
                }

                foreach (ItemDescriptionModel v in lstCompanyDashBoard)
                {
                    i = i + 1;
                    ItemDescriptioncontent += "<tr><td style='border: 1px solid #000000;' width='5%' align='center'><span>" + Convert.ToString(v.Po_Item_No) + ')' + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Item_Code + " </span></td><td style='border: 1px solid #000000;white-space: pre-line;' width='35%'><span>" + v.ItemCode_Description + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Unit + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Po_Quantity + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Offered_Quantity + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Accepted_Quantity + "</span></td><td style='border: 1px solid #000000;' width='10%'><span>" + v.Cumulative_Accepted_Qty + "</span></td></tr>";
                }

                foreach (ReferenceDocumentsModel v in RefranceDocuments)
                {
                    J = J + 1;
                    // ReferenceDocumentscontent += "<tr><td style='border: 1px solid #b5b5b5;' width='5%'> " + J + " </td><td style='border: 1px solid #b5b5b5;' width='25%'>" + Convert.ToString(v.Document_Name) + "</td><td style='border: 1px solid #b5b5b5;' width='35%'>" + v.Document_No + " </td><td style='border: 1px solid #b5b5b5;' width='35%'>" + v.Approval_Status + "</td></tr>";
                    ReferenceDocumentscontent += "<tr><td style='border: 1px solid #000000;' width='5%' align='center'> " + J + " </td><td style='border: 1px solid #000000;' width='25%';white-space: pre-line;>" + Convert.ToString(v.Document_Name) + "</td><td style='border: 1px solid #000000;' width='25%'>" + v.Document_No + " </td><td style='border: 1px solid #000000;' width='25%'>" + v.VendorDocumentNumber + "</td><td style='border: 1px solid #000000;' width='25%'>" + v.Approval_Status + "</td></tr>";
                }

                foreach (InspectionActivitiesModel v in InspectionDocuments)
                {
                    K = K + 1;
                    //InspectionDocumentsContent += "<tr><td width='5%' align='center'><span> " + K + ')' + " </span></td><td width='95%'><span>" + Convert.ToString(v.Stages_Witnessed) + "</span></td></tr>";
                    InspectionDocumentsContent += "<tr><td width='5%' align='center'><span> " + K + " </span></td><td style='white-space: pre-line;' width='95%'><span>" + Convert.ToString(v.Stages_Witnessed) + "</span></td></tr>";
                }
                foreach (DocumentRevieweModel v in DocumentReview)
                {
                    L = L + 1;
                    //DocumentreviewContent += "<tr><td width='5%' align='center'><span> " + L + ')' + " </span></td><td width='95%' ><span>" + Convert.ToString(v.Description) + "</span></td></tr>";
                    DocumentreviewContent += "<tr><td width='5%' align='center'><span> " + L + " </span></td><td width='95%' style='white-space: pre-line;'><span>" + Convert.ToString(v.Description) + "</span></td></tr>";
                }
                //foreach (EquipmentDetailsModel v in EquipmentDetails)
                //{
                //    M = M + 1;
                //    EquipmentDetailscontent += "<tr><td> " + M + " </td><td>" + Convert.ToString(v.Name_Of_Equipments) + "</td><td>" + v.Range + " </td><td>" + v.Id + "</td><td>" + v.CalibrationValid_Till_date + "</td><td>" + v.Certification_No_Date + "</td></tr>";
                //}

                body = body.Replace("[ItemDescriptionContent]", ItemDescriptioncontent);
                body = body.Replace("[ReferenceDocumentsContent]", ReferenceDocumentscontent);
                body = body.Replace("[InspectionDocumentsContent]", InspectionDocumentsContent);
                body = body.Replace("[DocumentreviewContent]", DocumentreviewContent);
                //body = body.Replace("[EquipmentDetailscontent]", EquipmentDetailscontent);
                body = body.Replace("[Stamp]", "https://tiimes.tuv-india.com/Stamp.png");

                if (ObjModelVisitReport.Signatures != null)
                {
                    body = body.Replace("[Signature]", "https://tiimes.tuv-india.com/Content/Sign/" + ObjModelVisitReport.Signatures + "");
                }
                else
                {
                    body = body.Replace("[Signature]", "-");
                }
                body = body.Replace("[Checkbox1]", check1);
                body = body.Replace("[Checkbox2]", check2);
                //body = body.Replace("[Checkbox3]", check3);
                //body = body.Replace("[Checkbox4]", check4);
                //body = body.Replace("[Checkbox5]", check5);
                //body = body.Replace("[Checkbox6]", check6);
                //body = body.Replace("[Checkbox7]", check7);
                //body = body.Replace("[Checkbox8]", check8);
                //body = body.Replace("[Checkbox9]", check9);

                strs.Append(body);
                PdfPageSize pageSize = PdfPageSize.A4;
                PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.MaxPageLoadTime = 120;  //=========================5-Aug-2019
                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;

                string _Header = string.Empty;
                string _footer = string.Empty;

                // for Report header by abel
                StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/Inspection_Certificate_Header.html"));
                _Header = _readHeader_File.ReadToEnd();

                _Header = _Header.Replace("[RevisionNo]", countNo);
                _Header = _Header.Replace("[Logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png"); // change123 once pulished on server

                StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/inspection-certificate-footer.html")); // Footer is used from IVR as it same and commented in pdf template.
                _footer = _readFooter_File.ReadToEnd();

                // header settings
                converter.Options.DisplayHeader = true ||
                    true || true;
                converter.Header.DisplayOnFirstPage = true;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = 75;

                PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
                headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Header.Add(headerHtml);

                // footer settings
                converter.Options.DisplayFooter = true ||
                    true || true;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                converter.Footer.Height = 130;

                PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
                footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Footer.Add(footerHtml);

                //end abel code

                #region Footer Code
                // page numbers can be added using a PdfTextSection object
                PdfTextSection text = new PdfTextSection(0, 90, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
                text.HorizontalAlign = PdfTextHorizontalAlign.Right;
                converter.Footer.Add(text);
                #endregion


                PdfDocument doc = converter.ConvertHtmlString(body);
                string Report = RM.Report;
                string ReportName = null;
                //string Report = ObjModelVisitReport.Report_No + "/" + count + ".pdf";
                string path = Server.MapPath("~/IRNReports");
                if (Report == null)
                {
                    Report = ObjModelVisitReport.Report_No + "/" + ".pdf";
                    ReportName = ObjModelVisitReport.Report_No;
                }
                else
                {
                    doc.Save(path + '\\' + Report);
                    ReportName = ObjModelVisitReport.Report_No;
                }

                doc.Close();
                #endregion
                if (RM.PK_RM_ID != 0)
                {
                    RM.Type = "IRN";
                    RM.Status = "1";
                    //RM.ImageReport = ReportNames;
                    RM.Report = Report;
                    if (RM.ReportName == "")
                    {
                        RM.ReportName = ReportName;
                    }
                    else
                    {
                        RM.ReportName = RM.ReportName;
                    }

                    RM.PK_CALL_ID = PK_IVR_ID;
                    Result = OBJDALIRN.InsertUpdateReport(RM);
                    if (Result != "" && Result != null)
                    {
                        TempData["InsertCompany"] = Result;
                    }
                }
                else
                {
                    RM.Type = "IRN";
                    RM.Status = "1";
                    //RM.ImageReport = ReportNames;
                    RM.Report = Report;
                    RM.ReportName = ReportName;
                    RM.PK_CALL_ID = PK_IVR_ID;
                    RM.VendorName = ObjModelVisitReport.Vendor_Name_Location;
                    RM.ClientName = ObjModelVisitReport.Client_Name;
                    RM.ProjectName = ObjModelVisitReport.Project_Name_Location;
                    RM.SubJob_No = ObjModelVisitReport.SubJob_No;
                    Result = OBJDALIRN.InsertUpdateReport(RM);
                    if (Result != "" && Result != null)
                    {
                        TempData["InsertCompany"] = Result;
                    }
                }

                #region
                CostSheetDashBoard = OBJDALIRN.GetReportByCall_Id(PK_IVR_ID);
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        ReportDashboard.Add(
                            new ReportModel
                            {
                                ReportName = Convert.ToString(dr["ReportName"]),
                                Report = Convert.ToString(dr["Report"]),
                                CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                // PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
                            }
                            );
                    }
                }
                ViewData["CostSheet"] = ReportDashboard;
                #endregion

                return RedirectToAction("IRNReports");

            }
            else
            {


                return View();
            }
        }
        #endregion

        [HttpPost]
        public JsonResult GetSubSubSectionType(string IRNReport, string ProjectName, string Vendor, string Client)
        {
            var Data = OBJDALIRN.GetReportList(IRNReport);
            DataSet DSJobMasterByQtId = new DataSet();
            DataTable DSJobMasterByReportNo = new DataTable();

            if (IRNReport != "")
            {
                Data = OBJDALIRN.GetReportList(IRNReport);
            }
            else if (ProjectName != "")
            {
                Data = OBJDALIRN.GetReportbyProjectNameList(ProjectName);
            }
            else if (Vendor != "")
            {
                Data = OBJDALIRN.GetReportbyVendorNameList(Vendor);
            }
            else if (Client != "")
            {
                Data = OBJDALIRN.GetReportbyClientNameList(Client);
            }


            //DSJobMasterByQtId = OBJDALIRN.GetCallId(IRNReport, ProjectName, Vendor, Client);
            //if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
            //{

            //    ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sap_And_Controle_No"]);
            //    ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_IVR_ID"]);
            //    ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name_Location"]);
            //    ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Address_Of_Inspection"]);
            //    ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["End_user_Name"]);
            //    ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Name_Location"]);
            //    // ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_Call_ID"]);
            //    ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);
            //    ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date_Of_Inspection"]);
            //    ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Client_Name"]);
            //    ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
            //    ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
            //    ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No"]);
            //    ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Vendor_Name"]);
            //    ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No_SubVendor"]);
            //    ObjModelVisitReport.Emails_Distribution = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Emails_Distribution"]);
            //    ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Pending_Activites"]);
            //    ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Areas_Of_Concerns"]);
            //    ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Non_Conformities_raised"]);
            //    ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Type"]);
            //    ObjModelVisitReport.Report_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Report_No"]);
            //    //ObjModelVisitReport.Call_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Call_No"]);
            //    Session["PK_IVR_ID"] = ObjModelVisitReport.PK_IVR_ID;
            //}

            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetdataCopyByreportId(string IRNReport)
        {

            DataSet DSJobMasterByQtId = new DataSet();
            DataTable DSJobMasterByReportNo = new DataTable();

            DSJobMasterByQtId = OBJDALIRN.GetReportByCallid(IRNReport);
            if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
            {
                ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_CALL_ID"]);


            }

            DSJobMasterByQtId = OBJDALIRN.GetCallId(Convert.ToInt32(ObjModelVisitReport.PK_Call_ID));
            if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
            {

                ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sap_And_Controle_No"]);
                ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_IVR_ID"]);
                ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name_Location"]);
                ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Address_Of_Inspection"]);
                ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["End_user_Name"]);
                ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Name_Location"]);
                // ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_Call_ID"]);
                ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);
                ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date_Of_Inspection"]);
                ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Client_Name"]);
                ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
                ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
                ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No"]);
                ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Vendor_Name"]);
                ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No_SubVendor"]);
                ObjModelVisitReport.Emails_Distribution = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Emails_Distribution"]);
                ObjModelVisitReport.client_Email = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["client_Email"]);
                ObjModelVisitReport.Vendor_Email = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Email"]);
                ObjModelVisitReport.Tuv_Branch = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Tuv_Branch"]);

                ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Pending_Activites"]);
                ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Areas_Of_Concerns"]);
                ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Non_Conformities_raised"]);
                ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Type"]);
                ObjModelVisitReport.Report_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Report_No"]);
                ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubJob_No"]);
                Session["PK_IVR_ID"] = ObjModelVisitReport.PK_IVR_ID;

            }

            return Json(ObjModelVisitReport, JsonRequestBehavior.AllowGet);
        }

        #region
        [HttpGet]
        public ActionResult ExportIndex(ReportModel c)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<ReportModel> grid = CreateExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<ReportModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                //added by nikita yadav 05092023
                var filename = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                return File(package.GetAsByteArray(), "application/unknown", "IRNReports-" + filename + ".xlsx");
            }
        }
        private IGrid<ReportModel> CreateExportableGrid(ReportModel c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<ReportModel> grid = new Grid<ReportModel>(GetData(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.ReportName).Titled("Report Name");
            grid.Columns.Add(model => model.inspectionDate).Titled("Inspection Date");
            grid.Columns.Add(model => model.CraetedDate).Titled("Report Date");
            grid.Columns.Add(model => model.ProjectName).Titled("Project Name");
            grid.Columns.Add(model => model.Report).Titled("Report File");
            grid.Columns.Add(model => model.SubJob_No).Titled("Sub/Sub-Sub Job No");
            grid.Columns.Add(model => model.VendorName).Titled("Vendor Name");
            grid.Columns.Add(model => model.Vendor_Name_Location).Titled("Sub Vendor Name");
            grid.Columns.Add(model => model.ClientName).Titled("Sub Job PO No");
            grid.Columns.Add(model => model.Po_No).Titled("Sub-Sub Job PO No");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector");
            grid.Columns.Add(model => model.Excuting_Branch).Titled("Excuting Branch");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.ReviseReason).Titled("Revise Reason");
            grid.Columns.Add(model => model.ddlReviseReason).Titled("Revise Reason Detail");
            grid.Pager = new GridPager<ReportModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objModelReport.LstDashboard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<ReportModel> GetData(ReportModel c)
        {
            Session["GetExcelData"] = "Yes";
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();

            try
            {
                CostSheetDashBoard = OBJDALIRN.GetReportByUserMIS();
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReportModel
                            {
                                //ReportName = Convert.ToString(dr["ReportName"]),
                                //Report = Convert.ToString(dr["Report"]),
                                //CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                //PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                //inspectionDate = Convert.ToString(dr["inspectionDate"]),
                                //ProjectName = Convert.ToString(dr["ProjectName"]),
                                //SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                //VendorName = Convert.ToString(dr["Vendor_Name"]),
                                //Vendor_Name_Location = Convert.ToString(dr["SubVendorName"]),
                                //ClientName = Convert.ToString(dr["Po_Number"]),
                                //Po_No = Convert.ToString(dr["SubVendorPoNo"]),
                                //Excuting_Branch = Convert.ToString(dr["Excuting_Branch"]),
                                //Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                //Inspector = Convert.ToString(dr["Inspector"]),
                                //ddlReviseReason = Convert.ToString(dr["ddlReviseReason"]),
                                //ReviseReason = Convert.ToString(dr["ReviseReason"]),
                                ReportName = Convert.ToString(dr["ReportName"]),
                                Report = Convert.ToString(dr["Report"]),
                                CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                inspectionDate = Convert.ToString(dr["inspectionDate"]),
                                ProjectName = Convert.ToString(dr["ProjectName"]),
                                SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                VendorName = Convert.ToString(dr["Vendor_Name"]),
                                Vendor_Name_Location = Convert.ToString(dr["SubVendorName"]),
                                ClientName = Convert.ToString(dr["Po_Number"]),
                                Po_No = Convert.ToString(dr["SubVendorPoNo"]),
                                Client_Name = Convert.ToString(dr["client"]),
                                Edit = Convert.ToString(dr["Edit"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Excuting_Branch = Convert.ToString(dr["Excuting_Branch"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                ddlReviseReason = Convert.ToString(dr["ddlReviseReason"]),
                                ReviseReason = Convert.ToString(dr["ReviseReason"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            objModelReport.LstDashboard = lstCompanyDashBoard;

            return objModelReport.LstDashboard;
        }

        #endregion
        public ActionResult Report(string Report)
        {
            string filename = Report;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "IRNReports/" + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);





            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            //{
            //    file.WriteLine(Report.ToString());
            //}




            //byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            //string fileName = Report;//+ ".PDF";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            // var path = Server.MapPath(@"~/IVRReport/" + Report);
            //return File("~/IVRReport/" + Report, "application/pdf");
        }

        #region Added by Ankush
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
                DTGetDeleteFile = OBJDALIRN.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = OBJDALIRN.DeleteUploadedFile(id);
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

        [HttpPost]
        public JsonResult DeleteConFile(string id)
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
                DTGetDeleteFile = OBJDALIRN.GetConFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = OBJDALIRN.DeleteConUploadedFile(id);
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
        public FileResult Download(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/Content/JobDocument/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }

        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            List<FileDetails> fileDetails = new List<FileDetails>();

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
                        if (files.FileName.EndsWith(".msg") || files.FileName.EndsWith(".MSG") || files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))
                        {
                            string fileName = files.FileName;
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();


                            BinaryReader br = new BinaryReader(files.InputStream);
                            byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                            fileDetail.FileContent = bytes;


                            fileDetails.Add(fileDetail);


                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetail.Id + fileDetail.Extension);
                            var K = "~/Content/JobDocument/" + fileName;
                            IPath = K;
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
                Session["listNCRUploadedFile"] = fileDetails;
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                ViewBag.Error = "Please Select XLSX or PDF File";
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }


        public void CustomerFeedbackMail(string ClientEmail)
        {
            try
            {

                DataTable Details = new DataTable();

                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string bodyTxt = "";

                bodyTxt += " Dear Sir/Madam, " + "<br><br>";
                bodyTxt += "Customer Email <br><br>";

                bodyTxt += "Best Regards,<br>";
                bodyTxt += "TUV India Pvt Ltd.<br>";

                msg.From = new MailAddress(MailFrom);

                string To = ClientEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }

                msg.Subject = "Confirmation of inspection visit";
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

        #endregion


        #region
        [HttpGet]
        public ActionResult SearchdataForEditInspectionDate(string SubJobNo, string PoNo)
        {
            string SubJob = string.Empty;
            if (SubJobNo != null)
            {
                int VarLength = Regex.Matches(SubJobNo, "/").Count;
                if (VarLength > 1)
                {
                    SubJob = SubJobNo.Substring(0, SubJobNo.LastIndexOf("/") + 0);
                }
                else
                {
                    SubJob = SubJobNo;
                }
            }
            DataTable Reportdashboard = new DataTable();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();
            try
            {
                Reportdashboard = OBJDALIRN.GetReportListBySubjobno(SubJobNo, PoNo);

                if (Reportdashboard.Rows.Count > 0)
                {
                    foreach (DataRow dr in Reportdashboard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReportModel
                            {
                                ReportName = Convert.ToString(dr["ReportNo"]),
                                Report = Convert.ToString(dr["Report"]),
                                CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                ProjectName = Convert.ToString(dr["ProjectName"]),
                                inspectionDate = Convert.ToString(dr["inspectionDate"])

                            }
                          );
                    }
                }
                return Json(lstCompanyDashBoard, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion


        #region All Concern

        [HttpGet]
        public ActionResult AllConcern(int? PK_IVR_ID, InspectionvisitReportModel objModel, int? PK_RM_ID)
        {
            if (Convert.ToInt32(PK_RM_ID) != 0)
            {
                objModel.abcid = Convert.ToInt32(PK_RM_ID);
            }
            if (Convert.ToInt32(PK_IVR_ID) != 0)
            {
                objModel.PK_IVR_ID = Convert.ToInt32(PK_IVR_ID);
            }

            #region  All Previous Concerns
            objModel.PK_IVR_ID = Convert.ToInt32(PK_IVR_ID);
            DataSet AllPreviousConcern = new DataSet();
            List<InspectionvisitReportModel> lstAllConvern = new List<InspectionvisitReportModel>();
            AllPreviousConcern = OBJDALIRN.AllPreviousOpenConcern(PK_IVR_ID);

            if (AllPreviousConcern.Tables[1].Rows.Count > 0)
            {
                objModel.strIsIssuePending = Convert.ToString(AllPreviousConcern.Tables[1].Rows[0]["IsIssuePending"]);
                if (objModel.strIsIssuePending == "1")
                { objModel.AreasOfConcern = true; }
                else
                { objModel.AreasOfConcern = false; }

            }

            if (AllPreviousConcern.Tables.Count > 0)
            {
                if (AllPreviousConcern.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in AllPreviousConcern.Tables[0].Rows)
                    {
                        lstAllConvern.Add(
                            new InspectionvisitReportModel
                            {
                                #region
                                Areas_Of_Concerns = Convert.ToString(dr["AreasOfConcern"]),
                                //Pending_Activites = Convert.ToString(dr["Pending_Activites"]),
                                //Non_Conformities_raised = Convert.ToString(dr["Non_Conformities_raised"]),
                                //Date_Of_Inspection = Convert.ToString(dr["Date_Of_Inspection"]),
                                Report_No = Convert.ToString(dr["ReportNo"]),
                                Name = Convert.ToString(dr["Name"]),
                                PkId = Convert.ToString(dr["PkId"]),
                                Type = Convert.ToString(dr["Type"]),
                                ReopenBy = Convert.ToString(dr["ReopenBy"]),
                                #endregion


                            }
                            );
                    }

                }
            }

            ViewData["AllConcerns"] = lstAllConvern;
            ViewBag.AllConcerns = lstAllConvern;

            //DataTable DTGetUploadedFile = new DataTable();
            //List<InspectionvisitReportModel> lstEditFileDetails = new List<InspectionvisitReportModel>();
            //DTGetUploadedFile = OBJDALIRN.areaofdetails(PK_IVR_ID);
            //if (DTGetUploadedFile.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in DTGetUploadedFile.Rows)
            //    {
            //        lstEditFileDetails.Add(
            //           new InspectionvisitReportModel
            //           {
            //               AreasOfConcern = Convert.ToBoolean(dr["IsIssuePending"]),


            //           }
            //         );
            //    }
            //}
            //objModel.lst1 = lstEditFileDetails;


            #endregion


            #region NCR

            DataSet NCR = new DataSet();
            List<InspectionvisitReportModel> lstNCR = new List<InspectionvisitReportModel>();
            NCR = OBJDALIRN.NCR(PK_IVR_ID);
            if (NCR.Tables.Count > 0)
            {
                if (NCR.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in NCR.Tables[0].Rows)
                    {
                        lstNCR.Add(
                            new InspectionvisitReportModel
                            {
                                #region
                                Non_Conformities_raised = Convert.ToString(dr["NCRNo"]),
                                NCRNo = Convert.ToString(dr["Pdf"]),
                                Status = Convert.ToString(dr["Status"]),
                                NCRID = Convert.ToString(dr["Id"]),
                                CreatedBy = Convert.ToString(dr["NcrRaisedBy"]),
                                #endregion


                            }
                            );
                    }

                }
            }

            ViewData["NCR"] = lstNCR;

            #endregion




            return View(objModel);
        }

        [HttpPost]
        public ActionResult AllConcern(int? PK_Call_ID, InspectionvisitReportModel objModel, FormCollection F)
        {

            String Result = "";

            //DataTable obj = new DataTable();
            string obj = OBJDALIRN.Updateareaconcern(objModel);



            if (objModel.Concern != null)
            {

                foreach (var item in objModel.Concern)
                {
                    objModel.PkId = item.PkId;

                    objModel.Reason = item.Reason;
                    if (objModel.Reason != null)
                    {

                        Result = OBJDALIRN.CloseConcern(objModel);
                    }

                }
            }



            // int? PK_Call_ID = ObjModel.PK_Call_ID;
            return RedirectToAction("AllConcern", new { PK_IVR_ID = objModel.PK_IVR_ID, PK_RM_ID = objModel.abcid });

        }

        #endregion


        #region Earlier Issued Quantity

        [HttpGet]
        public ActionResult EarlierIssuedQuantity(int? PK_IVR_ID, ItemDescriptionModel objModel, int? PK_RM_ID)
        {
            #region  Earlier Issued Quantity
            objModel.PK_IVR_ID = Convert.ToInt32(PK_IVR_ID);
            objModel.abcid = Convert.ToInt32(PK_RM_ID);
            DataSet AllPreviousConcern = new DataSet();
            List<ItemDescriptionModel> lstAllConvern = new List<ItemDescriptionModel>();
            AllPreviousConcern = OBJDALIRN.EarlierIssuedQuantity(PK_IVR_ID);



            if (AllPreviousConcern.Tables.Count > 0)
            {
                if (AllPreviousConcern.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in AllPreviousConcern.Tables[0].Rows)
                    {
                        lstAllConvern.Add(
                            new ItemDescriptionModel
                            {
                                #region
                                PK_ItemD_Id = Convert.ToInt32(dr["PK_ItemD_Id"]),
                                Po_Item_No = Convert.ToString(dr["Po_Item_No"]),
                                ItemCode_Description = Convert.ToString(dr["ItemCode_Description"]),
                                Po_Quantity = Convert.ToString(dr["Po_Quantity"]),
                                Offered_Quantity = Convert.ToString(dr["Offered_Quantity"]),
                                PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                Item_Code = Convert.ToString(dr["Item_Code"]),
                                Accepted_Quantity = Convert.ToString(dr["Accepted_Quantity"]),
                                Cumulative_Accepted_Qty = Convert.ToString(dr["Cumulative_Accepted_Qty"]),
                                Unit = Convert.ToString(dr["Unit"]),
                                Created_By = Convert.ToString(dr["CreatedBy"]),
                                Report_No = Convert.ToString(dr["ReportNo"]),
                                #endregion


                            }
                            );
                    }

                }
            }

            ViewData["ErlierItem"] = lstAllConvern;


            #endregion

            return View(objModel);
        }





        [HttpPost]
        public JsonResult GetdownloadDatacheckdate(int pk_call_id)
        {

            DataTable data = OBJDALIRN.Datecheck(pk_call_id);

            var Test = Convert.ToString(data.Rows[0][0]);
            return Json(Test, JsonRequestBehavior.AllowGet);
        }

        //end 




        #endregion


        [HttpPost]
        public JsonResult GetdownloadData(int pk_call_id)
        {

            DataTable data = OBJDALIRN.areaofconcerncheck(pk_call_id);

            var Test = Convert.ToString(data.Rows[0][0]);
            return Json(Test, JsonRequestBehavior.AllowGet);
        }

        private static System.Drawing.Image ResizeImageNew(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);

            g.Dispose();





            return (System.Drawing.Image)b;
        }

        public void DownloadNew(String p, String d)
        {
            /// return File(Path.Combine(Server.MapPath("~/Files/Documents/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);


            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = OBJDALIRN.GetFileContent(Convert.ToInt32(d));

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

        public FileResult Download2(string d)
        {

            string FileName = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = OBJDALIRN.GetFileContent(Convert.ToInt32(d));

            if (DTDownloadFile.Rows.Count > 0)
            {
                //fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
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
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", FileName);
        }


        [HttpGet]
        public ActionResult ExportIndex1(ReportModel c)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<ReportModel> grid = CreateExportableGrid1(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<ReportModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                //added by nikita yadav 05092023
                var filename = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                return File(package.GetAsByteArray(), "application/unknown", "IRNReports-" + filename + ".xlsx");
            }
        }
        private IGrid<ReportModel> CreateExportableGrid1(ReportModel c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<ReportModel> grid = new Grid<ReportModel>(GetData1(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.ReportName).Titled("Report Name");
            grid.Columns.Add(model => model.inspectionDate).Titled("Inspection Date");
            grid.Columns.Add(model => model.CraetedDate).Titled("Report Date");
            grid.Columns.Add(model => model.ProjectName).Titled("Project Name");
            grid.Columns.Add(model => model.Report).Titled("Report");
            grid.Columns.Add(model => model.SubJob_No).Titled("Sub/Sub-Sub Job No");
            grid.Columns.Add(model => model.CustomerName).Titled("Customer Name");
            grid.Columns.Add(model => model.VendorName).Titled("Vendor Name");
            grid.Columns.Add(model => model.Vendor_Name_Location).Titled("Sub Vendor Name");
            grid.Columns.Add(model => model.ClientName).Titled("Sub Job PO No");
            grid.Columns.Add(model => model.Po_No).Titled("Sub-Sub Job PO No");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector");
            grid.Columns.Add(model => model.Excuting_Branch).Titled("Excuting Branch");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.lastdateofinspection).Titled("Last Date Of inspection");

            grid.Pager = new GridPager<ReportModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objModelReport.LstDashboard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<ReportModel> GetData1(ReportModel c)
        {
            Session["GetExcelData"] = "Yes";
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();


            try
            {
                if (Session["FromDate1"] != null && Session["FromDate1"] != "" && Session["Todate1"] != null && Session["Todate1"] != "")
                {
                    c.FromDate = Session["FromDate1"].ToString();
                    c.ToDate = Session["Todate1"].ToString();
                    CostSheetDashBoard = OBJDALIRN.GetIRNReportdate(c);
                }
                else
                {
                    CostSheetDashBoard = OBJDALIRN.GetAllIRNReport();
                }
                //CostSheetDashBoard = OBJDALIRN.GetReportByUser();
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
                                inspectionDate = Convert.ToString(dr["inspectionDate"]),
                                ProjectName = Convert.ToString(dr["ProjectName"]),
                                SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                VendorName = Convert.ToString(dr["Vendor_Name"]),
                                Vendor_Name_Location = Convert.ToString(dr["SubVendorName"]),
                                ClientName = Convert.ToString(dr["Po_Number"]),
                                Po_No = Convert.ToString(dr["SubVendorPoNo"]),
                                Excuting_Branch = Convert.ToString(dr["Excuting_Branch"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                //added by shrutika salve 06102023
                                lastdateofinspection = Convert.ToString(dr["lastdateofinspection"]),


                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            objModelReport.LstDashboard = lstCompanyDashBoard;

            return objModelReport.LstDashboard;
        }



    }
}