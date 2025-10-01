using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class SafetyInsidentReportController : Controller
    {

        DataSet dss = new DataSet();
        DataTable dsGetStamp = new DataTable();
        DALSafetyInsidentReport objISM = new DALSafetyInsidentReport();
        DALCalls objDalCalls = new DALCalls();
        SafetyInsidentReport objM = new Models.SafetyInsidentReport();

        // GET: SafetyInsidentReport
        public ActionResult SafetyInsidentReport(int? PKId)
        {
            List<SafetyInsidentReport> lmd = new List<SafetyInsidentReport>();
            var Data = objDalCalls.GetBranchList();
            ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");
            var UserData = objISM.GetInspectorList1();
            ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");

            if (PKId > 0)
            {

                #region IncidentPhotographs att
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objISM.EditUploadedFile(PKId);

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
                    objM.IncidentPhotographs = lstEditFileDetails;
                }
                #endregion

                #region LessonsLearntPhotographs att
                DataTable DTLessonsLearntPhotographs = new DataTable();
                List<FileDetails> lstLessonsLearntPhotographs = new List<FileDetails>();
                DTLessonsLearntPhotographs = objISM.EditLearntPhotographs(PKId);

                if (DTLessonsLearntPhotographs.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTLessonsLearntPhotographs.Rows)
                    {
                        lstLessonsLearntPhotographs.Add(
                           new FileDetails
                           {

                               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                               FileName = Convert.ToString(dr["FileName"]),
                               Extension = Convert.ToString(dr["Extenstion"]),
                               IDS = Convert.ToString(dr["FileID"]),
                           }
                         );
                    }
                    ViewData["lstLessonsLearntPhotographs"] = lstLessonsLearntPhotographs;
                    objM.LessonsLearntPhotographs = lstLessonsLearntPhotographs;
                }
                #endregion

                #region LessonsLearntPhotographs att
                DataTable DTOtherAtt = new DataTable();
                List<FileDetails> lstDTOtherAtt = new List<FileDetails>();
                DTOtherAtt = objISM.OtherAtt(PKId);

                if (DTOtherAtt.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTOtherAtt.Rows)
                    {
                        lstDTOtherAtt.Add(
                           new FileDetails
                           {

                               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                               FileName = Convert.ToString(dr["FileName"]),
                               Extension = Convert.ToString(dr["Extenstion"]),
                               IDS = Convert.ToString(dr["FileID"]),
                           }
                         );
                    }
                    ViewData["lstEditFileDetails"] = lstDTOtherAtt;
                    objM.OtherAttachment = lstDTOtherAtt;
                }
                #endregion


                DataSet dss = new DataSet();

                dss = objISM.dsGetDataById(PKId);

                if (dss.Tables[0].Rows.Count > 0)
                {
                    objM.PKId = Convert.ToInt32(dss.Tables[0].Rows[0]["PKId"]);
                    objM.Branch = Convert.ToString(dss.Tables[0].Rows[0]["Branch"]);
                    objM.DateofReport = Convert.ToString(dss.Tables[0].Rows[0]["DateofReport"]);
                    objM.DateOfIncident = Convert.ToString(dss.Tables[0].Rows[0]["DateOfIncident"]);
                    objM.TypeOfIncident = Convert.ToString(dss.Tables[0].Rows[0]["TypeOfIncident"]);
                    objM.NameOfInjuredPerson = Convert.ToString(dss.Tables[0].Rows[0]["NameOfInjuredPerson"]);
                    objM.IPHomeAddress = Convert.ToString(dss.Tables[0].Rows[0]["IPHomeAddress"]);
                    objM.LocationofIncident = Convert.ToString(dss.Tables[0].Rows[0]["LocationofIncident"]);
                    objM.TypeOfInjury = Convert.ToString(dss.Tables[0].Rows[0]["TypeOfInjury"]);
                    objM.MedicalTreatmentDetails = Convert.ToString(dss.Tables[0].Rows[0]["MedicalTreatmentDetails"]);
                    objM.DescriptionOfIncident = Convert.ToString(dss.Tables[0].Rows[0]["DescriptionOfIncident"]);
                    objM.RootCauseAnalysis = Convert.ToString(dss.Tables[0].Rows[0]["RootCauseAnalysis"]);
                    objM.Correction = Convert.ToString(dss.Tables[0].Rows[0]["Correction"]);
                    objM.CorrectiveAction = Convert.ToString(dss.Tables[0].Rows[0]["CorrectiveAction"]);
                    objM.MandaysLost = Convert.ToString(dss.Tables[0].Rows[0]["MandaysLost"]);
                    objM.RiskAndOpportunities = Convert.ToBoolean(dss.Tables[0].Rows[0]["RiskAndOpportunities"]);
                    objM.AIHIRAReviewed = Convert.ToBoolean(dss.Tables[0].Rows[0]["AIHIRAReviewed"]);
                    objM.ShareLessonsLearnt = Convert.ToBoolean(dss.Tables[0].Rows[0]["ShareLessonsLearnt"]);
                    objM.Status = Convert.ToString(dss.Tables[0].Rows[0]["Status"]);
                    objM.FormFilledBy = Convert.ToString(dss.Tables[0].Rows[0]["FormFilledBy"]);
                    objM.CreationDate = Convert.ToString(dss.Tables[0].Rows[0]["CreationDate"]);
                    objM.Modifiedby = Convert.ToString(dss.Tables[0].Rows[0]["Modifiedby"]);
                    objM.ModificationDate = Convert.ToString(dss.Tables[0].Rows[0]["ModificationDate"]);
                    objM.TiimesUIN = Convert.ToString(dss.Tables[0].Rows[0]["UniqueNumber"]);
                    objM.LessonsLearned = Convert.ToString(dss.Tables[0].Rows[0]["LessonsLearned"]);
                }


                
               

            }
            else
            {

                #region Generate Unique no
                int _min = 1000;
                int _max = 9999;
                Random _rdm = new Random();
                int a = _rdm.Next(_min, _max);
                string T = "SIR" + Convert.ToString(a);
                objM.TiimesUIN = T;
                #endregion


                DataSet GetUserData = new DataSet();
                GetUserData = objISM.GetOnPageLoad();

                if (GetUserData.Tables[0].Rows.Count > 0)
                {
                    objM.NameOfInjuredPerson = Convert.ToString(GetUserData.Tables[0].Rows[0]["PK_Userid"]);
                    objM.Branch = Convert.ToString(GetUserData.Tables[0].Rows[0]["FK_BranchID"]);
                    //objM.TiimesUIN = Convert.ToString(GetUserData.Tables[0].Rows[0]["PK_Userid"]);
                }
                
            }
            return View(objM);
        }


        [HttpPost]
        public ActionResult SafetyInsidentReport(SafetyInsidentReport SIR)
        {
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listIncidentPhotographs"] as List<FileDetails>;

            List<FileDetails> lstLessonsLearntPhotographs = new List<FileDetails>();
            lstLessonsLearntPhotographs = Session["LessonsLearntPhotographs"] as List<FileDetails>;

            List<FileDetails> lstOtherAttachment = new List<FileDetails>();
            lstOtherAttachment = Session["OtherAttachment"] as List<FileDetails>;


            string Result = string.Empty;
            try
            {

                if (SIR.PKId > 0)
                {
                    //Update
                    Result = objISM.Insert(SIR);
                    if (Result != null)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            //SIR.PKId = Convert.ToInt32(Result);
                            Result = objISM.InsertFileAttachment(lstFileDtls, SIR.PKId);
                            Session["listJobMasterUploadedFile"] = null;
                        }
                        if (lstLessonsLearntPhotographs != null && lstLessonsLearntPhotographs.Count > 0)
                        {
                            //SIR.PKId = Convert.ToInt32(Result);
                            Result = objISM.InsertLessonsLearntPhotographs(lstLessonsLearntPhotographs, SIR.PKId);
                            Session["LessonsLearntPhotographs"] = null;
                        }
                        if (lstOtherAttachment != null && lstOtherAttachment.Count > 0)
                        {
                            //SIR.PKId = Convert.ToInt32(Result);
                            Result = objISM.InsertOtherAttachment(lstOtherAttachment, SIR.PKId);
                            Session["OtherAttachment"] = null;
                        }
                    }
                }
                else
                {

                   


                    Result = objISM.Insert(SIR);
                    if(Result!=null)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            SIR.PKId = Convert.ToInt32(Result);
                            Result = objISM.InsertFileAttachment(lstFileDtls, SIR.PKId);
                            Session["listJobMasterUploadedFile"] = null;
                        }
                    }
                   
                    if (Convert.ToInt16(Result) > 0)
                    {
                        ModelState.Clear();
                        TempData["message"] = "Record Added Successfully";
                    }
                    else
                    {
                        TempData["message"] = "Error";
                    }
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("ListSafetyInsidentReport", "SafetyInsidentReport");
        }

        [HttpGet]
        public ActionResult ListSafetyInsidentReport()
        {
            List<SafetyInsidentReport> lmd = new List<SafetyInsidentReport>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objISM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new SafetyInsidentReport
                {
                    

                    PKId = Convert.ToInt32(dr["PKId"]),
                    TiimesUIN = Convert.ToString(dr["UniqueNumber"]),
                    Branch                            = Convert.ToString(dr["Branch"]),
                    DateofReport                      = Convert.ToString(dr["DateofReport"]),
                    DateOfIncident                    = Convert.ToString(dr["DateOfIncident"]),
                    TypeOfIncident                    = Convert.ToString(dr["TypeOfIncident"]),
                    NameOfInjuredPerson               = Convert.ToString(dr["NameOfInjuredPerson"]),
                    IPHomeAddress                     = Convert.ToString(dr["IPHomeAddress"]),
                    LocationofIncident                = Convert.ToString(dr["LocationofIncident"]),
                    TypeOfInjury                      = Convert.ToString(dr["TypeOfInjury"]),
                    MedicalTreatmentDetails           = Convert.ToString(dr["MedicalTreatmentDetails"]),
                    DescriptionOfIncident             = Convert.ToString(dr["DescriptionOfIncident"]),
                    RootCauseAnalysis                 = Convert.ToString(dr["RootCauseAnalysis"]),
                    Correction                        = Convert.ToString(dr["Correction"]),
                    CorrectiveAction                  = Convert.ToString(dr["CorrectiveAction"]),
                    MandaysLost                       = Convert.ToString(dr["MandaysLost"]),
                    SRiskAndOpportunities              = Convert.ToString(dr["RiskAndOpportunities"]),
                    SAIHIRAReviewed                    = Convert.ToString(dr["AIHIRAReviewed"]),
                    SShareLessonsLearnt                = Convert.ToString(dr["ShareLessonsLearnt"]),
                    Status                            = Convert.ToString(dr["Status"]),
                    FormFilledBy                      = Convert.ToString(dr["FormFilledBy"]),
                    CreationDate                      = Convert.ToString(dr["CreationDate"]),
                    Modifiedby                        = Convert.ToString(dr["Modifiedby"]),
                    ModificationDate = Convert.ToString(dr["ModificationDate"]),


                });
            }
            return View(lmd.ToList());

        }

        public ActionResult Delete(int? PKId)
        {
            string Result = string.Empty;
            try
            {
                Result = objISM.Delete(Convert.ToInt32(PKId));
                if (Convert.ToInt16(Result) > 0)
                {


                    ModelState.Clear();
                }
                else
                {

                    TempData["message"] = "Error";
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ModelState.Clear();
            return RedirectToAction("ListSafetyInsidentReport");


        }


        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();

            //Adding New Code 13 March 2020
            List<FileDetails> IncidentPhotographs = new List<FileDetails>();
            List<FileDetails> LessonsLearntPhotographs = new List<FileDetails>();
            List<FileDetails> OtherAttachment = new List<FileDetails>();
            List<FileDetails> fileAddPODetails = new List<FileDetails>();

            if (Session["listIncidentPhotographs"] != null)
            {
                IncidentPhotographs = Session["listIncidentPhotographs"] as List<FileDetails>;
            }

            if (Session["LessonsLearntPhotographs"] != null)
            {
                LessonsLearntPhotographs = Session["LessonsLearntPhotographs"] as List<FileDetails>;
            }

            if (Session["OtherAttachment"] != null)
            {
                OtherAttachment = Session["OtherAttachment"] as List<FileDetails>;
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
                                IncidentPhotographs.Add(fileDetail);
                            }
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                            {
                                LessonsLearntPhotographs.Add(fileDetail);
                            }
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD2")
                            {
                                OtherAttachment.Add(fileDetail);
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
                    Session["listIncidentPhotographs"] = IncidentPhotographs;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                {
                    Session["LessonsLearntPhotographs"] = LessonsLearntPhotographs;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD2")
                {
                    Session["OtherAttachment"] = OtherAttachment;
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }

        public void Download(String p, String d)
        {
            /// return File(Path.Combine(Server.MapPath("~/Files/Documents/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);


            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objISM.GetFileContent(Convert.ToInt32(d));

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


    }
}