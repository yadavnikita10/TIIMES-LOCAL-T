//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.tool.xml;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using TuvVision.DataAccessLayer;
using TuvVision.Models;


namespace TuvVision.Controllers
{
    public class MISNCRController : Controller
    {
        // GET: MISNCR
        DALNCR objDNCR = new DALNCR();
        NCR objNCR = new NCR();
        int Id;

        public ActionResult CreateNCR(int? Id, string SubJobId, int? IVRId, string Type)
        {
            var model = new NCR();
            DataSet dss = new DataSet();
            DataSet IVRID = new DataSet();
            DataSet dsgetDataFromVisitReport = new DataSet();
            DataTable dsGetStamp = new DataTable();
            DataTable DTGetUploadedFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            model.Type = Type;
            TempData["SubJobId"] = SubJobId;
            TempData.Keep();
            if (Id != null)
            {
                #region

                dss = objDNCR.ChkNCRDataById(Convert.ToInt32(Id));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    //  dss = objDNCR.GetNCRDataById(Convert.ToInt32(Id));
                    if (dss.Tables[0].Rows.Count > 0)
                    {

                        model.PK_Call_Id = dss.Tables[0].Rows[0]["Pk_Call_Id"].ToString();
                        model.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"].ToString());
                        model.NCRNo = dss.Tables[0].Rows[0]["NCRNo"].ToString();
                        model.TUVControlNo = dss.Tables[0].Rows[0]["TUVControlNo"].ToString();
                        model.ProjectName = dss.Tables[0].Rows[0]["ProjectName"].ToString();
                        model.Client = dss.Tables[0].Rows[0]["Client"].ToString();
                        model.VenderSubVendor = dss.Tables[0].Rows[0]["V1"].ToString();
                        model.ItemEquipment = dss.Tables[0].Rows[0]["ItemEquipment"].ToString();
                        model.ReferenceDocument = dss.Tables[0].Rows[0]["ReferenceDocument"].ToString();
                        model.DescriptionOfTheNonconformity = dss.Tables[0].Rows[0]["DescriptionOfTheNonconformity"].ToString();
                        model.NCRRaisedBy = dss.Tables[0].Rows[0]["NCRRaisedBy"].ToString();
                        model.Date = Convert.ToString(dss.Tables[0].Rows[0]["Date"].ToString());

                        model.Attachment = dss.Tables[0].Rows[0]["Attachment"].ToString();
                        model.Pdf = dss.Tables[0].Rows[0]["Pdf"].ToString();
                        model.IVRId = dss.Tables[0].Rows[0]["IVRId"].ToString();
                        model.Status = dss.Tables[0].Rows[0]["Status"].ToString();
                        model.SubVendorName = dss.Tables[0].Rows[0]["V2"].ToString();

                        model.VendorPoNo = dss.Tables[0].Rows[0]["P1"].ToString();
                        model.SubVendorPoNo = dss.Tables[0].Rows[0]["P2"].ToString();
                        model.date_of_PO = dss.Tables[0].Rows[0]["D1"].ToString();
                        model.date_of_POSubVendor = dss.Tables[0].Rows[0]["D2"].ToString();
                        model.SAP = dss.Tables[0].Rows[0]["SAP_No"].ToString();

                    }
                    //Added by Ankush for Delete file and update file


                    DTGetUploadedFile = objDNCR.EditUploadedFile(Id);
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
                        model.FileDetails = lstEditFileDetails;
                    }
                    //Added by Ankush for Delete file and update file
                }
                else
                {
                    dsgetDataFromVisitReport = objDNCR.dsgetDataFromVisitReport(Convert.ToInt32(IVRId));
                    if (dsgetDataFromVisitReport.Tables[0].Rows.Count > 0)
                    {
                        model.PK_Call_Id = dsgetDataFromVisitReport.Tables[0].Rows[0]["PK_Call_ID"].ToString();
                        model.ProjectName = dsgetDataFromVisitReport.Tables[0].Rows[0]["Project_Name_Location"].ToString();
                        model.Client = dsgetDataFromVisitReport.Tables[0].Rows[0]["Client_Name"].ToString();
                        model.VenderSubVendor = dsgetDataFromVisitReport.Tables[0].Rows[0]["Vendor_Name_Location"].ToString();
                        model.DescriptionOfTheNonconformity = dsgetDataFromVisitReport.Tables[0].Rows[0]["Non_Conformities_raised"].ToString();
                        model.NCRRaisedBy = dsgetDataFromVisitReport.Tables[0].Rows[0]["CreatedBy"].ToString();
                        model.Date = Convert.ToString(dsgetDataFromVisitReport.Tables[0].Rows[0]["CreatedDate"].ToString());
                        model.IVRId = Convert.ToString(IVRId);
                        model.TUVControlNo = dsgetDataFromVisitReport.Tables[0].Rows[0]["TUVControlNo"].ToString();
                    }

                }
                #endregion

            }
            else
            {
                IVRID = objDNCR.ChkNCRDataByIVRID(Convert.ToInt32(IVRId));
                if ((IVRID.Tables[0].Rows.Count > 0))
                {
                    model.Id = Convert.ToInt32(IVRID.Tables[0].Rows[0]["Id"].ToString());
                    dss = objDNCR.ChkNCRDataById(Convert.ToInt32(model.Id));
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        //  dss = objDNCR.GetNCRDataById(Convert.ToInt32(Id));
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            model.PK_Call_Id = dss.Tables[0].Rows[0]["Pk_Call_Id"].ToString();
                            model.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"].ToString());
                            model.NCRNo = dss.Tables[0].Rows[0]["NCRNo"].ToString();
                            model.TUVControlNo = dss.Tables[0].Rows[0]["TUVControlNo"].ToString();
                            model.ProjectName = dss.Tables[0].Rows[0]["ProjectName"].ToString();
                            model.Client = dss.Tables[0].Rows[0]["Client"].ToString();
                            model.VenderSubVendor = dss.Tables[0].Rows[0]["V1"].ToString();
                            model.ItemEquipment = dss.Tables[0].Rows[0]["ItemEquipment"].ToString();
                            model.ReferenceDocument = dss.Tables[0].Rows[0]["ReferenceDocument"].ToString();
                            model.DescriptionOfTheNonconformity = dss.Tables[0].Rows[0]["DescriptionOfTheNonconformity"].ToString();
                            model.NCRRaisedBy = dss.Tables[0].Rows[0]["NCRRaisedBy"].ToString();
                            model.Date = Convert.ToString(dss.Tables[0].Rows[0]["Date"].ToString());

                            model.Attachment = dss.Tables[0].Rows[0]["Attachment"].ToString();
                            model.Pdf = dss.Tables[0].Rows[0]["Pdf"].ToString();
                            model.IVRId = dss.Tables[0].Rows[0]["IVRId"].ToString();
                            model.Status = dss.Tables[0].Rows[0]["Status"].ToString();
                            model.SubVendorName = dss.Tables[0].Rows[0]["V2"].ToString();

                            model.VendorPoNo = dss.Tables[0].Rows[0]["P1"].ToString();
                            model.SubVendorPoNo = dss.Tables[0].Rows[0]["P2"].ToString();
                            model.date_of_PO = dss.Tables[0].Rows[0]["D1"].ToString();
                            model.date_of_POSubVendor = dss.Tables[0].Rows[0]["D2"].ToString();
                            model.SAP = dss.Tables[0].Rows[0]["SAP_No"].ToString();






                        }

                        DTGetUploadedFile = objDNCR.EditUploadedFile(model.Id);
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
                            model.FileDetails = lstEditFileDetails;
                        }



                    }
                }
                else
                {
                    dsgetDataFromVisitReport = objDNCR.dsgetDataFromVisitReport(Convert.ToInt32(IVRId));
                    if (dsgetDataFromVisitReport.Tables[0].Rows.Count > 0)
                    {
                        model.PK_Call_Id = dsgetDataFromVisitReport.Tables[0].Rows[0]["PK_Call_ID"].ToString();
                        model.ProjectName = dsgetDataFromVisitReport.Tables[0].Rows[0]["Project_Name_Location"].ToString();
                        model.Client = dsgetDataFromVisitReport.Tables[0].Rows[0]["Client_Name"].ToString();
                        model.VenderSubVendor = dsgetDataFromVisitReport.Tables[0].Rows[0]["V1"].ToString();
                        model.DescriptionOfTheNonconformity = dsgetDataFromVisitReport.Tables[0].Rows[0]["Non_Conformities_raised"].ToString();
                        model.NCRRaisedBy = dsgetDataFromVisitReport.Tables[0].Rows[0]["CreatedBy"].ToString();
                        model.Date = Convert.ToString(dsgetDataFromVisitReport.Tables[0].Rows[0]["CreatedDate"].ToString());
                        model.IVRId = Convert.ToString(IVRId);
                        model.TUVControlNo = dsgetDataFromVisitReport.Tables[0].Rows[0]["TUVControlNo"].ToString();
                        model.SubVendorName = dsgetDataFromVisitReport.Tables[0].Rows[0]["V2"].ToString();


                        model.VendorPoNo = dsgetDataFromVisitReport.Tables[0].Rows[0]["P1"].ToString();
                        model.SubVendorPoNo = dsgetDataFromVisitReport.Tables[0].Rows[0]["P2"].ToString();
                        model.date_of_PO = dsgetDataFromVisitReport.Tables[0].Rows[0]["D1"].ToString();
                        model.date_of_POSubVendor = dsgetDataFromVisitReport.Tables[0].Rows[0]["D2"].ToString();
                        model.SAP = dsgetDataFromVisitReport.Tables[0].Rows[0]["SAP_No"].ToString();
                    }
                }


            }


            return View(model);

        }
        int AId;
        [HttpPost]
        public ActionResult CreateNCR(NCR N, List<HttpPostedFileBase> img_Banner)
        {
            string Result = string.Empty;
            int NCRID = 0;
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listNCRUploadedFile"] as List<FileDetails>;

            string IPath = string.Empty;
            var list = Session["list"] as List<string>;
            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }
            try
            {

                List<string> lstAttachment = new List<string>();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                {
                    foreach (HttpPostedFileBase single in img_Banner) // Added by Sagar Panigrahi
                    {
                        //HttpPostedFileBase Imagesection;
                        //Imagesection = Request.Files[single];
                        if (single != null && single.FileName != "")
                        {
                            var filename = CommonControl.FileUpload("~/NCRDocument/", single);
                            lstAttachment.Add(filename);
                        }
                    }
                    N.Attachment = string.Join(",", lstAttachment);
                    if (string.IsNullOrEmpty(N.Attachment))
                    {
                        N.Attachment = "NoImage.gif";
                    }
                }
                else
                {
                    N.Attachment = "NoImage.gif";
                }


                if (N.Id > 0)
                {
                    //Update
                    AId = objDNCR.Insert(N, IPath);
                    NCRID = N.Id;
                    if (NCRID != null && NCRID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = objDNCR.InsertFileAttachment(lstFileDtls, NCRID);
                            Session["listNCRUploadedFile"] = null;
                        }
                    }
                }
                else
                {
                    //int _min = 10000;
                    //int _max = 99999;
                    //Random _rdm = new Random();
                    //int Rjno = _rdm.Next(_min, _max);
                    //string ConfirmCode = Convert.ToString(Rjno);
                    //N.NCRNo = ConfirmCode;
                    AId = objDNCR.Insert(N, IPath);
                    NCRID = Convert.ToInt32(Session["NCRIDs"]);
                    if (NCRID != null && NCRID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = objDNCR.InsertFileAttachment(lstFileDtls, NCRID);
                            Session["listNCRUploadedFile"] = null;
                        }
                    }
                    if (Convert.ToInt16(AId) > 0)
                    {
                        ModelState.Clear();
                        TempData["message"] = "Record Added Successfully...";
                    }
                    else
                    {
                        TempData["message"] = "Something went Wrong! Please try Again";
                    }
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            // return RedirectToAction("CreateNCR", Id = AId);

            //        return RedirectToAction("CreateNCR", new RouteValueDictionary(
            //new { controller = "NCR", action = "CreateNCR", Id = AId }));

            //Note:- NCRNo Generate in NCRPrint Function

            // return RedirectToAction("NCRPrint", new RouteValueDictionary(
            //new { controller = "NCR", action = "NCRPrint", Id = AId }));

            return RedirectToAction("NCRPrintWithHeaderFooter", new RouteValueDictionary(
           new { controller = "NCR", action = "NCRPrintWithHeaderFooter", Id = AId }));


        }

        [HttpGet]
        public ActionResult ListNCR()
        {

            List<NCR> lmd = new List<NCR>();  // creating list of model.  
            DataSet ds = new DataSet();
            if (Session["FromDate"] != null && Session["FromDate"] != "" && Session["Todate"] != null && Session["Todate"] != "")
            {
                objNCR.FromDate = Session["FromDate"].ToString();
                objNCR.ToDate = Session["Todate"].ToString();
                ds = objDNCR.GetMISDataByBranchID(objNCR);
            }
            else
            {
                ds = objDNCR.GetData_(); // fill dataset  
            }

                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                    {
                        lmd.Add(new NCR
                        {


                            Id = Convert.ToInt32(dr["Id"]),
                            NCRNo = Convert.ToString(dr["NCRNo"]),
                            TUVControlNo = Convert.ToString(dr["TUVControlNo"]),
                            ProjectName = Convert.ToString(dr["ProjectName"]),
                            Client = Convert.ToString(dr["Client"]),
                            //VenderSubVendor = Convert.ToString(dr["VenderSubVendor"]),
                            SubJobNo = Convert.ToString(dr["SubJobNo"]),
                            V1 = Convert.ToString(dr["V1"]),
                            V2 = Convert.ToString(dr["V2"]),
                            P1 = Convert.ToString(dr["P1"]),
                            P2 = Convert.ToString(dr["P2"]),
                            ItemEquipment = Convert.ToString(dr["ItemEquipment"]),
                            ReferenceDocument = Convert.ToString(dr["ReferenceDocument"]),
                            DescriptionOfTheNonconformity = Convert.ToString(dr["DescriptionOfTheNonconformity"]),
                            NCRRaisedBy = Convert.ToString(dr["NCRRaisedBy"]),
                            Date = Convert.ToString(dr["Date"]),
                            Attachment = Convert.ToString(dr["Attachment"]),
                            Pdf = Convert.ToString(dr["Pdf"]),
                            IVRId = Convert.ToString(dr["IVRId"]),
                            Branch = Convert.ToString(dr["Branch"]),
                            Status = Convert.ToString(dr["Status"]),
                        });
                    }
                    ViewData["Test"] = lmd;
                    objNCR.lmd1 = lmd;
            return View(objNCR);


        }

        [HttpPost]
        public ActionResult ListNCR(string FromDate, string ToDate)
        {

            Session["FromDate"] = FromDate;
            Session["Todate"] = ToDate;

            //return View();
            return RedirectToAction("ListNCR", "MISNCR");
        }
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
                            filePath = Path.Combine(Server.MapPath("~/NCRDocument/"), fileDetail.Id + fileDetail.Extension);
                            //filePath = Path.Combine(Server.MapPath("~/Files/Documents/"), filePath);
                            var K = "~/NCRDocument/" + fileName;
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
                Session["listNCRUploadedFile"] = fileDetails;

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                ViewBag.Error = "Please Select XLSX or PDF File";
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }

        //public static string RenderPartialToString(string controlName, object viewData)
        //{
        //    ViewPage viewPage = new ViewPage() { ViewContext = new ViewContext() };

        //    viewPage.ViewData = new ViewDataDictionary(viewData);
        //    viewPage.Controls.Add(viewPage.LoadControl(controlName));

        //    StringBuilder sb = new StringBuilder();
        //    using (StringWriter sw = new StringWriter(sb))
        //    {
        //        using (HtmlTextWriter tw = new HtmlTextWriter(sw))
        //        {
        //            viewPage.RenderControl(tw);
        //        }
        //    }

        //    return sb.ToString();
        //}


        #region Itextsharp pdf
        //[HttpPost]
        //[ValidateInput(false)]
        //public FileResult Export(string GridHtml)
        //{
        //    ViewData["Test"] = "V";


        //    List<NCR> lmd = new List<NCR>();  // creating list of model.  
        //    DataSet ds = new DataSet();

        //    ds = objDNCR.GetData(); // fill dataset  

        //    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
        //    {
        //        lmd.Add(new NCR
        //        {


        //            Id = Convert.ToInt32(dr["Id"]),
        //            NCRNo = Convert.ToString(dr["NCRNo"]),
        //            TUVControlNo = Convert.ToString(dr["TUVControlNo"]),
        //            ProjectName = Convert.ToString(dr["ProjectName"]),
        //            Client = Convert.ToString(dr["Client"]),
        //            VenderSubVendor = Convert.ToString(dr["VenderSubVendor"]),
        //            ItemEquipment = Convert.ToString(dr["ItemEquipment"]),
        //            ReferenceDocument = Convert.ToString(dr["ReferenceDocument"]),
        //            DescriptionOfTheNonconformity = Convert.ToString(dr["DescriptionOfTheNonconformity"]),
        //            NCRRaisedBy = Convert.ToString(dr["NCRRaisedBy"]),
        //            Date = Convert.ToDateTime(dr["Date"]),
        //            Attachment = Convert.ToString(dr["Attachment"]),
        //            Pdf = Convert.ToString(dr["Pdf"]),
        //            IVRId = Convert.ToString(dr["IVRId"]),

        //        });
        //    }
        //    ViewData["Test"] = lmd;


        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {

        //        StringReader sr = new StringReader(GridHtml);

        //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //        pdfDoc.Open();
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        //        pdfDoc.Close();
        //        return File(stream.ToArray(), "application/pdf", "Grid.pdf");
        //    }



        //}


        #endregion

        //public ActionResult PrintPDF()
        //{
        //    empEntities context = new empEntities();
        //    List<emp_table> Data = context.emp_table.ToList();

        //    return new PartialViewAsPdf("_NCR", Data)
        //    {
        //        FileName = "TestPartialViewAsPdf.pdf"
        //    };
        //}

        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objDNCR.Delete(Convert.ToInt32(Id));
                if (Convert.ToInt16(Result) > 0)
                {


                    ModelState.Clear();
                }
                else
                {

                    TempData["message"] = "Something went Wrong! Please try Again";
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ModelState.Clear();
            return RedirectToAction("ListNCR");


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
                DTGetDeleteFile = objDNCR.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = objDNCR.DeleteUploadedFile(id);
                    var path = Path.Combine(Server.MapPath("~/NCRDocument/"), id + fileDetails.Extension);
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
            return File(Path.Combine(Server.MapPath("~/NCRDocument/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }
        #endregion 

        #region print pdf
        public ActionResult NCRPrint(int Id)
        {
            string ReportName = "";
            var modelR = new NCR();
            DataSet dsGetNCR = new DataSet();

            int count = 0;




            List<NCR> lstNCR = new List<NCR>();
            dsGetNCR = objDNCR.GetNCRDataById(Id);

            if (dsGetNCR.Tables[0].Rows.Count > 0)
            {
                modelR.Id = Convert.ToInt32(dsGetNCR.Tables[0].Rows[0]["Id"]);
                modelR.NCRNo = dsGetNCR.Tables[0].Rows[0]["NCRNo"].ToString();
                modelR.TUVControlNo = dsGetNCR.Tables[0].Rows[0]["TUVControlNo"].ToString();
                modelR.ProjectName = dsGetNCR.Tables[0].Rows[0]["ProjectName"].ToString();
                modelR.Client = dsGetNCR.Tables[0].Rows[0]["Client"].ToString();
                modelR.VenderSubVendor = dsGetNCR.Tables[0].Rows[0]["VenderSubVendor"].ToString();
                modelR.ItemEquipment = dsGetNCR.Tables[0].Rows[0]["ItemEquipment"].ToString();
                modelR.ReferenceDocument = dsGetNCR.Tables[0].Rows[0]["ReferenceDocument"].ToString();
                modelR.DescriptionOfTheNonconformity = dsGetNCR.Tables[0].Rows[0]["DescriptionOfTheNonconformity"].ToString();
                modelR.NCRRaisedBy = dsGetNCR.Tables[0].Rows[0]["NCRRaisedBy"].ToString();
                modelR.Date = Convert.ToString(dsGetNCR.Tables[0].Rows[0]["Date"]);
                modelR.Attachment = dsGetNCR.Tables[0].Rows[0]["Attachment"].ToString();
                modelR.Pdf = dsGetNCR.Tables[0].Rows[0]["Pdf"].ToString();
                modelR.SubVendorName = dsGetNCR.Tables[0].Rows[0]["SubVendorName"].ToString();
            }

            #region 
            String NCRDate;
            string GNCRNO;
            if (modelR.NCRNo != "")
            {
                NCRDate = DateTime.Now.ToString("dd-MM-yyyy");
                GNCRNO = modelR.NCRNo;
            }
            else
            {
                NCRDate = DateTime.Now.ToString("dd-MM-yyyy");
                GNCRNO = "NCR" + '/' + TempData["SubJobId"] + '/' + Id + '/' + NCRDate;

            }





            string Result1 = "";

            Result1 = objDNCR.InserPDF(ReportName, Id, GNCRNO);
            modelR.NCRNo = GNCRNO;
            #endregion

            //string d = Convert.ToString(modelR.Date);
            String Var = modelR.Date.ToString(); //only date part
            string d = Var;
            #region Save to Pdf Code 


            SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
            System.Text.StringBuilder strs = new System.Text.StringBuilder();
            string body = string.Empty;
            int J = 0;
            int K = 0;
            int L = 0;
            int M = 0;
            string Datarow = "";
            string DatarowFOI = "";
            string data1 = "";
            string data2 = "";
            string data3 = "";
            string data4 = "";
            string data5 = "";
            string data6;
            string data7 = "";
            string data8 = "";
            string data9 = "";
            string data10 = "";

            string data11 = "";


            using (StreamReader reader = new StreamReader(Server.MapPath("~/nonconformancereport.html")))
            {
                body = reader.ReadToEnd();
            }


            //foreach (NCR v in lstNCR)
            //{

            //    data1 = v.ProjectName;
            //    data2 = v.Client;
            //    data3 = v.VenderSubVendor;
            //    data4 = v.DescriptionOfTheNonconformity;
            //    data5 = v.CreatedBy;
            //    data6 = Convert.ToString(v.CreatedDate);

            //}
            data1 = modelR.ProjectName;
            data2 = modelR.Client;
            data3 = modelR.VenderSubVendor;
            data4 = modelR.DescriptionOfTheNonconformity;
            data5 = modelR.NCRRaisedBy;
            data6 = d;
            data7 = modelR.ItemEquipment;
            data8 = modelR.ReferenceDocument;
            data9 = modelR.NCRNo;
            data10 = modelR.TUVControlNo;
            data11 = modelR.SubVendorName;
            //string d = data6.ToString("MM/dd/yyyy HH:mm:ss");
            //Rahul Print
            body = body.Replace("[data1]", data1);
            body = body.Replace("[data2]", data2);
            body = body.Replace("[data3]", data3);
            body = body.Replace("[data4]", data4);
            body = body.Replace("[data5]", data5);
            body = body.Replace("[data6]", data6);
            body = body.Replace("[data7]", data7);
            body = body.Replace("[data8]", data8);
            body = body.Replace("[data9]", data9);
            body = body.Replace("[data10]", data10);

            body = body.Replace("[data11]", data11);

            //body = body.Replace("[Logo]", "http://localhost:54895/AllJsAndCss/images/logo.png");
            //body = body.Replace("[Stamp]", "http://localhost:54895/Stamp.png");
            //body = body.Replace("[Signature]", "http://localhost:54895/signature.jpg");
            //body = body.Replace("[Logo]", ConfigurationManager.AppSettings["Web"].ToString()+ "/AllJsAndCss/images/logo.png");
            body = body.Replace("[Logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png");

            body = body.Replace("[Stamp]", "~/Stamp.png");
            body = body.Replace("[Signature]", "~/signature.jpg");


            strs.Append(body);
            PdfPageSize pageSize = PdfPageSize.A4;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
            HtmlToPdf converter = new HtmlToPdf();
            string Vaibhav = null;

            //When javascript needs to be enabled
            converter.Options.JavaScriptEnabled = true;//(HtmlToPdfStartupMode)Enum.Parse(typeof(HtmlToPdfStartupMode),, true);

            converter.Options.MaxPageLoadTime = 180;
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            PdfDocument doc = converter.ConvertHtmlString(body);
            //  string ReportName = Vaibhav + "/" + count + ".pdf";
            ReportName = +Id + ".pdf";

            string path = Server.MapPath("~/NCRPDF");
            doc.Save(path + '\\' + ReportName);
            doc.Close();

            #region NCRNo Generate Code
            //String NCRDate;
            //string GNCRNO;
            if (modelR.NCRNo != "")
            {
                NCRDate = DateTime.Now.ToString("dd-MM-yyyy");
                GNCRNO = modelR.NCRNo;
            }
            else
            {
                NCRDate = DateTime.Now.ToString("dd-MM-yyyy");
                GNCRNO = "NCR" + '/' + TempData["SubJobId"] + '/' + Id + '/' + NCRDate;

            }
            #endregion



            #region Insert pdf to database
            string Result = "";

            Result = objDNCR.InserPDF(ReportName, Id, GNCRNO);
            #endregion
            #endregion

            // return View(objMTCE);
            return RedirectToAction("CreateNCR", "NCR", new { Id = modelR.Id });
        }
        #endregion

        #region Print pdf with header footer
        public ActionResult NCRPrintWithHeaderFooter(int Id)
        {
            string ReportName = "";
            var modelR = new NCR();
            DataSet dsGetNCR = new DataSet();

            int count = 0;




            List<NCR> lstNCR = new List<NCR>();
            dsGetNCR = objDNCR.GetNCRDataById(Id);

            if (dsGetNCR.Tables[0].Rows.Count > 0)
            {
                modelR.Id = Convert.ToInt32(dsGetNCR.Tables[0].Rows[0]["Id"]);
                modelR.NCRNo = dsGetNCR.Tables[0].Rows[0]["NCRNo"].ToString();
                modelR.TUVControlNo = dsGetNCR.Tables[0].Rows[0]["TUVControlNo"].ToString();
                modelR.ProjectName = dsGetNCR.Tables[0].Rows[0]["ProjectName"].ToString();
                modelR.Client = dsGetNCR.Tables[0].Rows[0]["Client"].ToString();

                modelR.VenderSubVendor = dsGetNCR.Tables[0].Rows[0]["VenderSubVendor"].ToString();

                modelR.ItemEquipment = dsGetNCR.Tables[0].Rows[0]["ItemEquipment"].ToString();
                modelR.ReferenceDocument = dsGetNCR.Tables[0].Rows[0]["ReferenceDocument"].ToString();
                modelR.DescriptionOfTheNonconformity = dsGetNCR.Tables[0].Rows[0]["DescriptionOfTheNonconformity"].ToString();
                modelR.NCRRaisedBy = dsGetNCR.Tables[0].Rows[0]["NCRRaisedBy"].ToString();
                modelR.Date = Convert.ToString(dsGetNCR.Tables[0].Rows[0]["Date"]);
                modelR.Attachment = dsGetNCR.Tables[0].Rows[0]["Attachment"].ToString();
                modelR.Pdf = dsGetNCR.Tables[0].Rows[0]["Pdf"].ToString();

                modelR.SubVendorName = dsGetNCR.Tables[0].Rows[0]["SubVendorName"].ToString();

                modelR.Signature = dsGetNCR.Tables[0].Rows[0]["Signature"].ToString();
                modelR.VendorPoNo = dsGetNCR.Tables[0].Rows[0]["PO_no"].ToString();
                modelR.SubVendorPoNo = dsGetNCR.Tables[0].Rows[0]["Po_No_SubVendor"].ToString();
                modelR.date_of_PO = dsGetNCR.Tables[0].Rows[0]["d1"].ToString();
                modelR.date_of_POSubVendor = dsGetNCR.Tables[0].Rows[0]["d2"].ToString();
                modelR.SAP = dsGetNCR.Tables[0].Rows[0]["SAP_No"].ToString();
            }

            #region 
            String NCRDate;
            string GNCRNO;
            if (modelR.NCRNo != "")
            {
                NCRDate = DateTime.Now.ToString("dd-MM-yyyy");
                GNCRNO = modelR.NCRNo;
            }
            else
            {
                NCRDate = DateTime.Now.ToString("dd-MM-yyyy");
                GNCRNO = "NCR" + '/' + TempData["SubJobId"] + '/' + Id + " Date" + NCRDate;

            }





            string Result1 = "";

            Result1 = objDNCR.InserPDF(ReportName, Id, GNCRNO);
            modelR.NCRNo = GNCRNO;
            #endregion

            //string d = Convert.ToString(modelR.Date);
            String Var = modelR.Date.ToString(); //only date part
            string d = Var;
            #region Save to Pdf Code 



            System.Text.StringBuilder strs = new System.Text.StringBuilder();
            string body = string.Empty;
            int J = 0;
            int K = 0;
            int L = 0;
            int M = 0;
            string Datarow = "";
            string DatarowFOI = "";
            string data1 = "";
            string data2 = "";
            string data3 = "";
            string data4 = "";
            string data5 = "";
            string data6;
            string data7 = "";
            string data8 = "";
            string data9 = "";
            string data10 = "";

            string data11 = "";

            string VendorPoNo = "";
            string SubVendorPoNo = "";
            string date_of_PO = "";
            string date_of_POSubVendor = "";




            using (StreamReader reader = new StreamReader(Server.MapPath("~/NCRHTMLPAGE/nonconformancereport.html")))
            {
                body = reader.ReadToEnd();
            }


            //foreach (NCR v in lstNCR)
            //{

            //    data1 = v.ProjectName;
            //    data2 = v.Client;
            //    data3 = v.VenderSubVendor;
            //    data4 = v.DescriptionOfTheNonconformity;
            //    data5 = v.CreatedBy;
            //    data6 = Convert.ToString(v.CreatedDate);
            //}
            data1 = modelR.ProjectName;
            data2 = modelR.Client;
            data3 = modelR.VenderSubVendor;
            data4 = modelR.DescriptionOfTheNonconformity;
            data5 = modelR.NCRRaisedBy;
            data6 = d;
            data7 = modelR.ItemEquipment;
            data8 = modelR.ReferenceDocument;
            data9 = modelR.NCRNo;
            data10 = modelR.TUVControlNo;
            data11 = modelR.SubVendorName;

            VendorPoNo = modelR.VendorPoNo;
            SubVendorPoNo = modelR.SubVendorPoNo;
            date_of_PO = modelR.date_of_PO;
            date_of_POSubVendor = modelR.date_of_POSubVendor;



            //string d = data6.ToString("MM/dd/yyyy HH:mm:ss");
            //Rahul Print
            body = body.Replace("[data1]", data1);
            body = body.Replace("[data2]", data2);
            body = body.Replace("[data3]", data3);
            body = body.Replace("[data4]", data4);
            body = body.Replace("[data5]", data5);
            body = body.Replace("[data6]", data6);
            body = body.Replace("[data7]", data7);
            body = body.Replace("[data8]", data8);
            body = body.Replace("[data9]", data9);
            body = body.Replace("[data10]", data10 + " (" + modelR.SAP + ")");

            body = body.Replace("[data11]", data11);
            if (date_of_PO != "" || date_of_PO != null)
            {
                body = body.Replace("[data12]", VendorPoNo + " Date " + date_of_PO);
            }
            else
            {
                body = body.Replace("[data12]", VendorPoNo);
            }
            if (date_of_POSubVendor != "" || date_of_POSubVendor != null)
            {
                body = body.Replace("[data13]", SubVendorPoNo + " Date " + date_of_POSubVendor);
            }
            else
            {
                body = body.Replace("[data13]", SubVendorPoNo);
            }


            //body = body.Replace("[date_of_PO]", date_of_PO);
            //body = body.Replace("[date_of_POSubVendor]", date_of_POSubVendor);

            //body = body.Replace("[Logo]", "http://localhost:54895/AllJsAndCss/images/logo.png");
            //body = body.Replace("[Stamp]", "http://localhost:54895/Stamp.png");
            //body = body.Replace("[Signature]", "http://localhost:54895/signature.jpg");
            // string logo = "<img src = '" + ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/" + "logo.jpg" + "' style='width:84px; ' >";

            string S = "<img src = '" + ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.jpg" + "' style='width:225px;height:125px; ' align='center'>";
            //body = body.Replace("[Logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png");
            body = body.Replace("[Logo]", S);

            //body = body.Replace("[Logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.png");
            //body = body.Replace("[Logo]", logo);
            body = body.Replace("[Stamp]", "~/Stamp.png");
            string I = "<img src = '" + ConfigurationManager.AppSettings["Web"].ToString() + "/Content/Sign/" + modelR.Signature + "' style='width:100px;height:50px; ' align='center'>";
            body = body.Replace("[Signature]", I);


            strs.Append(body);
            PdfPageSize pageSize = PdfPageSize.A4;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
            HtmlToPdf converter = new HtmlToPdf();
            string Vaibhav = null;

            //When javascript needs to be enabled
            converter.Options.JavaScriptEnabled = true;//(HtmlToPdfStartupMode)Enum.Parse(typeof(HtmlToPdfStartupMode),, true);

            converter.Options.MaxPageLoadTime = 180;
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;


            #region Header & footer
            string _Header = string.Empty;
            string _footer = string.Empty;


            StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/NCRHTMLPAGE/NCRHeader.html"));
            _Header = _readHeader_File.ReadToEnd();
            //_Header = _Header.Replace("[logo]", "https://tiimes.tuv-india.com/AllJsAndCss/images/logo.png");
            _Header = _Header.Replace("[data9]", data9);
            _Header = _Header.Replace("[data10]", data10 + " (" + modelR.SAP + ")");
            _Header = _Header.Replace("[Logo]", "https://tiimes.tuv-india.com:10443/AllJsAndCss/images/logo.png");


            StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/NCRHTMLPAGE/NCRFooter.html"));
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
            converter.Footer.Height = 80;

            PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);

            PdfTextSection text1 = new PdfTextSection(525, 50, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
            //text1.HorizontalAlign = PdfTextHorizontalAlign.Right;
            //text1.VerticalAlign = PdfTextVerticalAlign.Bottom;
            converter.Footer.Add(text1);



            #endregion

            PdfDocument doc = converter.ConvertHtmlString(body);
            //  string ReportName = Vaibhav + "/" + count + ".pdf";
            ReportName = +Id + ".pdf";

            string path = Server.MapPath("~/NCRPDF");
            doc.Save(path + '\\' + ReportName);
            doc.Close();

            #region NCRNo Generate Code
            //String NCRDate;
            //string GNCRNO;
            if (modelR.NCRNo != "")
            {
                NCRDate = DateTime.Now.ToString("dd-MM-yyyy");
                GNCRNO = modelR.NCRNo;
            }
            else
            {
                NCRDate = DateTime.Now.ToString("dd-MM-yyyy");
                GNCRNO = "NCR" + '/' + TempData["SubJobId"] + '/' + Id + '/' + NCRDate;

            }
            #endregion



            #region Insert pdf to database
            string Result = "";

            Result = objDNCR.InserPDF(ReportName, Id, GNCRNO);
            #endregion
            #endregion

            // return View(objMTCE);
            return RedirectToAction("CreateNCR", "NCR", new { Id = modelR.Id });
        }
        #endregion

        #region MIS Report
        public ActionResult NCRReport(int? Id)
        {
            return View();
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
                IGrid<NCR> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<NCR> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }
                //added by nikita on 06-09-2023
                var filename = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                return File(package.GetAsByteArray(), "application/unknown", "ListNCR-" + filename + ".xlsx");
            }
        }
        private IGrid<NCR> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<NCR> grid = new Grid<NCR>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.Id).Titled("Id");
            grid.Columns.Add(model => model.NCRNo).Titled("NCR No ");
            grid.Columns.Add(model => model.TUVControlNo).Titled("TUVI Control No ");
            grid.Columns.Add(model => model.ProjectName).Titled("Project Name");
            grid.Columns.Add(model => model.Client).Titled("Client");
            grid.Columns.Add(model => model.SubJobNo).Titled("Sub/Sub-Sub Job No");
            grid.Columns.Add(model => model.V1).Titled("Sub Job Vendor Name");
            grid.Columns.Add(model => model.V2).Titled("Sub-Sub Job Vendor Name");
            grid.Columns.Add(model => model.P1).Titled("Sub Job PO No");
            grid.Columns.Add(model => model.P2).Titled("Sub-Sub Job PO No");
            grid.Columns.Add(model => model.ItemEquipment).Titled("Item Equipment");
            grid.Columns.Add(model => model.ReferenceDocument).Titled("Reference Document");
            grid.Columns.Add(model => model.DescriptionOfTheNonconformity).Titled("Description Of The Nonconformity");
            grid.Columns.Add(model => model.NCRRaisedBy).Titled("NCR Raised By");
            grid.Columns.Add(model => model.Date).Titled("Date");
            grid.Columns.Add(model => model.IVRId).Titled("IVR Id");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.Status).Titled("Status");



            grid.Pager = new GridPager<NCR>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objNCR.lmd1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<NCR> GetData()
        {

            List<NCR> lmd = new List<NCR>();  // creating list of model.  
            DataSet ds = new DataSet();
            if (Session["FromDate"] != null && Session["FromDate"] != "" && Session["Todate"] != null && Session["Todate"] != "")
            {
                objNCR.FromDate = Session["FromDate"].ToString();
                objNCR.ToDate = Session["Todate"].ToString();
                ds = objDNCR.GetMISDataByBranchID(objNCR);
            }else
            {
                ds = objDNCR.GetData_(); // fill dataset  
            }
           
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                    {
                        lmd.Add(new NCR
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            NCRNo = Convert.ToString(dr["NCRNo"]),
                            TUVControlNo = Convert.ToString(dr["TUVControlNo"]),
                            ProjectName = Convert.ToString(dr["ProjectName"]),
                            Client = Convert.ToString(dr["Client"]),
                            //VenderSubVendor = Convert.ToString(dr["VenderSubVendor"]),
                            SubJobNo = Convert.ToString(dr["SubJobNo"]),
                            V1 = Convert.ToString(dr["V1"]),
                            V2 = Convert.ToString(dr["V2"]),
                            P1 = Convert.ToString(dr["P1"]),
                            P2 = Convert.ToString(dr["P2"]),
                            ItemEquipment = Convert.ToString(dr["ItemEquipment"]),
                            ReferenceDocument = Convert.ToString(dr["ReferenceDocument"]),
                            DescriptionOfTheNonconformity = Convert.ToString(dr["DescriptionOfTheNonconformity"]),
                            NCRRaisedBy = Convert.ToString(dr["NCRRaisedBy"]),
                            Date = Convert.ToString(dr["Date"]),
                            Attachment = Convert.ToString(dr["Attachment"]),
                            Pdf = Convert.ToString(dr["Pdf"]),
                            IVRId = Convert.ToString(dr["IVRId"]),
                            Branch = Convert.ToString(dr["Branch"]),
                            Status = Convert.ToString(dr["Status"]),
                        });
                    }
                    ViewData["Test"] = lmd;
                    objNCR.lmd1 = lmd;
                    return objNCR.lmd1;
        }
        #endregion


        public void DownloadNCRDoc(String p, String d)
        {
            /// return File(Path.Combine(Server.MapPath("~/Files/Documents/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);


            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDNCR.GetFileContent(Convert.ToInt32(d));

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

        public FileResult DownloadNCRAttach(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/NCRDocument/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }

    }
}