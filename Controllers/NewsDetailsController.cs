using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.BusinessEntities;
using TuvVision.BusinessServices.Implementation;
using TuvVision.BusinessServices.Interface;
using System.IO;
using System.Data;
using TuvVision.Models;
using System.Net;

namespace TuvVision.Controllers
{
    public class NewsDetailsController : Controller
    {
        private INewsDetails newsrepo = new NewsDetailss();
        private UploadFuncation gfhg = new UploadFuncation();
        public NewsDetailss DALNews = new NewsDetailss();
        // GET: NewsDetails\
        [HttpGet]
        public ActionResult GetListOfNews() //Get List Of News Details
        {
            List<News_VM> lstnews = new List<News_VM>();
            lstnews = newsrepo.GetAllNewsDetails().ToList();
            ViewBag.Data = lstnews;
            return View(lstnews);
        }

        [HttpGet]
        public ActionResult AddNewsDetails(int? id) //Add & Update NewsDetails
        {
            Session["NewsImage"] = string.Empty;
            if (id > 0)
            {
                News_VM _vmnews = newsrepo.GetNewsDataById(Convert.ToInt32(id));
                Session["NewsImage"] = Convert.ToString(_vmnews.NewsImage);

                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = DALNews.EditUploadedFile(id);
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
                    _vmnews.FileDetails = lstEditFileDetails;
                }

                return View(_vmnews);
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddNewsDetails(News_VM newsdtls, List<HttpPostedFileBase> NewsEvent)
        {
            #region Added by Ankush
            string Result = string.Empty;
            int CMPID = 0;
            var IPath = string.Empty;
            string[] splitedGrp;
            List<FileDetails> fileDetails = new List<FileDetails>();
            List<string> Selected = new List<string>();
            //List<FileDetails> lstFileDtls = new List<FileDetails>();
            //fileDetails = Session["listCMPUploadedFile"] as List<FileDetails>;
            #endregion
             
            try
            {
                if (newsdtls.NewsImage == null || newsdtls.NewsImage == "")
                {
                    newsdtls.NewsImage = Convert.ToString(Session["NewsImage"]);
                }
                else
                {
                    var files = Request.Files[0];
                    var attachcollection = new List<string>();
                    string uploadpath = "/Content/NewsImage/";

                    newsdtls.NewsImage = gfhg.FileUploadDynamicName(uploadpath, files);
                }

                List<string> lstAttachment = new List<string>();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["NewsEvent"])))
                {
                    foreach (HttpPostedFileBase single in NewsEvent) // Added by Sagar Panigrahi
                    {
                        //HttpPostedFileBase Imagesection;
                        //Imagesection = Request.Files[single];
                        if (single != null && single.FileName != "")
                        {
                            var filepath = "/Content/TinyMCE_Upload/";
                            var filename = gfhg.FileUploadDynamicName(filepath, single);

                            lstAttachment.Add(filename);
                        }
                    }
                    newsdtls.NewsEvent = string.Join(",", lstAttachment);
                }


                #region Added by Ankush

                    string filePathCMP = string.Empty;

                    foreach (HttpPostedFileBase files in NewsEvent) // Added by Sagar Panigrahi
                    {
                        
                        
                        if (files != null && files.ContentLength > 0)
                        {
                        int fileSize = files.ContentLength;
                        // if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc"))
                        if (files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpeg") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".gif"))

                            {
                                string fileName = files.FileName;
                                FileDetails fileDetail = new FileDetails();
                                fileDetail.FileName = fileName;
                                fileDetail.Extension = Path.GetExtension(fileName);
                                fileDetail.Id = Guid.NewGuid();
                                fileDetails.Add(fileDetail);
                                //-----------------------------------------------------
                                filePathCMP = Path.Combine(Server.MapPath("~/Content/TinyMCE_Upload/"), fileDetail.Id + fileDetail.Extension);
                                var K = "~/Content/TinyMCE_Upload/" + fileName;
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
                                ViewBag.Error = "Please Select Image File";
                            }
                        }
                    }                   


                Session["listCMPUploadedFile"] = fileDetails;
                #endregion
                int i = 0;
                if (newsdtls.PK_NewsId == 0)
                {
                    i = newsrepo.AddNewsDetails_New(newsdtls);
                    CMPID = Convert.ToInt32(Session["NWIDs"]);
                    if (CMPID != null && CMPID != 0)
                    {
                        if (fileDetails != null && fileDetails.Count > 0)
                        {
                            Result = DALNews.InsertFileAttachment(fileDetails, CMPID);
                            Session["listCMPUploadedFile"] = null;
                        }
                    }
                }
                else
                {
                    i = newsrepo.AddNewsDetails_New(newsdtls);
                    CMPID = newsdtls.PK_NewsId;
                    if (CMPID != null && CMPID != 0)
                    {
                        if (fileDetails != null && fileDetails.Count > 0)
                        {
                            Result = DALNews.InsertFileAttachment(fileDetails, CMPID);
                            Session["listCMPUploadedFile"] = null;
                        }
                    }                    
                }

                //int i = newsrepo.AddNewsDetails_New(newsdtls);

                if (i != 0)
                {
                    TempData["Success"] = "News Details Added Successfully...";
                    TempData.Keep();
                }
                else
                {
                    TempData["Error"] = "Something went Wrong! Please try Again";
                    TempData.Keep();
                }
                return RedirectToAction("GetListOfNews");
            }
            catch (Exception e)
            {
                throw e;
            }
            return View(newsdtls);
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            string uploadpath = "/Content/TinyMCE_Upload/";
            string Uploadedpath = "";
            if (file != null)
            {
                Uploadedpath = gfhg.FileUploadDynamicName(uploadpath, file);
            }
            return Json(new { location = Uploadedpath }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Delete(int id)
        {
            try
            {
                var delid = newsrepo.DeleteNewsById(id);
                if (delid != null)
                {
                    TempData["Deleted"] = "News Details removed Successfully";
                    TempData.Keep();
                    return RedirectToAction("GetListOfNews");
                }
                else
                {
                    TempData["Error"] = "Something went wrong. Try Again...";
                    TempData.Keep();
                    return RedirectToAction("GetListOfNews");
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("GetListOfNews");
        }

        #region Added by Ankush
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
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();
                            fileDetails.Add(fileDetail);
                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/ComplaintFolder/"), fileDetail.Id + fileDetail.Extension);
                            var K = "~/ComplaintFolder/" + fileName;
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
                Session["listCMPUploadedFile"] = fileDetails;
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                ViewBag.Error = "Please Select XLSX or PDF File";
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }
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
                DTGetDeleteFile = DALNews.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = DALNews.DeleteUploadedFile(id);
                    var path = Path.Combine(Server.MapPath("~/Content/TinyMCE_Upload/"), id + fileDetails.Extension);
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
            return File(Path.Combine(Server.MapPath("~/Content/TinyMCE_Upload/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }
        #endregion
    }
}