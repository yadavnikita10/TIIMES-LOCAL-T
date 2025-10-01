using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

using System.Configuration;
using System.Diagnostics;

namespace TuvVision.Controllers
{
    public class LibraryController : Controller
    {

        DALLibraryMaster OBJAppl = new DALLibraryMaster();
        CommonControl objCommonControl = new CommonControl();
        Library AplMas = new Library();
        // GET: Library
        public ActionResult LibraryList()
        {
            return View();
        }

        public ActionResult LibraryDetails()
        {
           
            return View();
        }

        [HttpGet]
        public ActionResult LibraryFolder(int? id, int? Lib_Id)
        {
            DataTable DTAppeal = new DataTable();
            List<Library> lstAppeal = new List<Library>();
            
            if (id == 39)
            { Session["Doc"] = "It/It Manual"; }
            else if (id == 40)
            { Session["Doc"] = "It/It Formats"; }
            else if (id == 118)
            { Session["Doc"] = "Inspection / eLibrary"; }
            else if (id == 119)
            { Session["Doc"] = "Inspection / Technical Circulars"; }
            else if (id == 120)
            { Session["Doc"] = "Inspection / Inspectors CV"; }
            else if (id == 56)
            { Session["Doc"] = "Inspection / Work Instruction"; }
            else if (id == 57)
            { Session["Doc"] = "Inspection / Inspection Formats"; }
            else if (id == 58)
            { Session["Doc"] = "Inspection / Sales and business development"; }
            else if (id == 59)
            { Session["Doc"] = "Inspection / Special Services"; }
            else if (id == 60)
            { Session["Doc"] = "Inspection / Expediting Services"; }
            else if (id == 61)
            { Session["Doc"] = "Inspection / Engineering Services"; }
            else if (id == 62)
            { Session["Doc"] = "Inspection / QA Records"; }
            else if (id == 63)
            { Session["Doc"] = "Inspection / Training"; }
            else if (id == 64)
            { Session["Doc"] = "Inspection / Others"; }
            else//27/05/2024
            { Session["Doc"] = "Inspection / Others"; }

            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = OBJAppl.GetUserRoll();
            if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
            {
                AplMas.Rollid = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FK_RoleID"]);
            }
            Library IVRNew = new Library();
            IVRNew.PK_SubSubID = id;
            Session["SubID"] = IVRNew.PK_SubSubID;
            IVRNew.Rollid = AplMas.Rollid;
            DateTime s = new DateTime();
            if (Lib_Id > 0)
            {
                DTAppeal = OBJAppl.GetLibraryDataByID(Convert.ToInt32(Lib_Id));
                try
                {
                    if (DTAppeal.Rows.Count > 0)
                    {
                        AplMas.FolderName = Convert.ToString(DTAppeal.Rows[0]["FolderName"]);
                        AplMas.Lib_Id = Convert.ToInt32(DTAppeal.Rows[0]["Lib_Id"]);
                        foreach (DataRow dr in DTAppeal.Rows)
                        {
                            lstAppeal.Add(
                                new Library
                                {
                                    PK_SubSubID = Convert.ToInt32(dr["PK_SubSubID"]),
                                    Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                    //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy"),
                                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                    CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                    ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                    //  ModifyDate = dr["ModifyDate"] !=null ? Convert.ToDateTime(dr["ModifyDate"]) : s ,
                                    ModifyBy = Convert.ToString(dr["ModifyBy"]),
                                    FolderName = Convert.ToString(dr["FolderName"]),
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
                ViewData["LibraryList"] = lstAppeal;
                return View(AplMas);
            }
            else
            {
                DTAppeal = OBJAppl.GetLibraryDashBoard(Convert.ToInt32(id));
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new Library
                            {
                                PK_SubSubID = Convert.ToInt32(dr["PK_SubSubID"]),
                                Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                    //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy"),
                                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                    //  ModifyDate = dr["ModifyDate"] !=null ? Convert.ToDateTime(dr["ModifyDate"]) : s ,
                                    ModifyBy = Convert.ToString(dr["ModifyBy"]),
                                FolderName = Convert.ToString(dr["FolderName"]),
                            }
                            );
                    }
                }
                ViewData["LibraryList"] = lstAppeal;
            }
            return View(IVRNew);
        }
        [HttpPost]
        public ActionResult LibraryFolder(Library LB)
        {
            string Result = string.Empty;
            Result = OBJAppl.InsertUpdateLibraryData(LB);
            if (Result != null && Result != "")
            {
                TempData["Success"] = "Folder Added Successfully...!!!";
                TempData.Keep();
                //TempData["UpdateCompany"] = Result;
                return RedirectToAction("LibraryFolder", new { id = Session["SubID"] });
            }
            else
            {
                TempData["Error"] = "Something went wrong, try again...!!!";
                TempData.Keep();
                return RedirectToAction("LibraryFolder", new { id = Session["SubID"] });
            }

            return RedirectToAction("LibraryFolder", new { id = Session["SubID"] });
            //return RedirectToAction ("LibraryFolder",LB.PK_SubSubID);
        }


        [HttpGet]
        public ActionResult InspectorCV(Library LB)
        {
            return RedirectToAction("CVFolder", new { id = 120 });
        }


        public ActionResult DeleteLibraryFolder(int? Lib_Id,int? PK_SubSubID)
        {
            int Result = 0;
            try
            {
                Result = OBJAppl.DeleteLibraryData(Lib_Id);
                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    return RedirectToAction("LibraryFolder",new { id = PK_SubSubID });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }




        [HttpGet]
        public ActionResult LibraryDocuments(LibraryDocumentModel LDM, string FolderName)
        {
            DataTable DTAppeal = new DataTable();
            List<LibraryDocumentModel> lstAppeal = new List<LibraryDocumentModel>();
            LibraryDocumentModel IVRNew = new LibraryDocumentModel();
            #region Library
            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = OBJAppl.GetUserRoll();
            if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
            {
                AplMas.Rollid = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FK_RoleID"]);
            }
            #endregion
            IVRNew.FolderName = FolderName;
            DTAppeal = OBJAppl.GetLibraryVideoDashBoard(Convert.ToInt32(LDM.Lib_Id));
            try
            {
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new LibraryDocumentModel
                            {
                                //PK_SubSubID = Convert.ToInt32(dr["PK_SubSubID"]),
                                Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                LP_Id = Convert.ToInt32(dr["LP_Id"]),
                                //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy"),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                //Modified_Date = Convert.ToString(dr["Modified_Date"]),
                               
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                ModifyBy = Convert.ToString(dr["ModifyBy"]),
                                PDF = Convert.ToString(dr["PDF"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["LibraryList"] = lstAppeal;
            
            IVRNew.Lib_Id = LDM.Lib_Id;
            IVRNew.Rollid = AplMas.Rollid;
            return View(IVRNew);
         
        }

        [HttpPost]
        public ActionResult LibraryDocuments(LibraryDocumentModel LB,FormCollection fc, HttpPostedFileBase[] multiImage)
        {
            string Result = string.Empty;
            #region Image Upload Code

            foreach (HttpPostedFileBase file in multiImage)
            {
                var images = file;
                var InputFileName = Path.GetFileName(images.FileName);
                var ServerSavePath = Path.Combine(Server.MapPath("~/LibraryFiles/") + InputFileName);

                file.SaveAs(ServerSavePath);
                LB.PDF = InputFileName;

                var InputFileName1 = Path.GetFileName(file.FileName);

                



               
                Result = OBJAppl.InsertUpdateLibraryDocsData(LB);
                if (Result != null && Result != "")
                {
                    TempData["UpdateCompany"] = Result;
                }
            }
            #endregion


            //Result = OBJAppl.InsertUpdateLibraryDocsData(LB);
            //if (Result != null && Result != "")
            //{
            //    TempData["UpdateCompany"] = Result;
            //}
            DataTable DTAppeal = new DataTable();
            List<LibraryDocumentModel> lstAppeal = new List<LibraryDocumentModel>();

            #region Library
            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = OBJAppl.GetUserRoll();
            if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
            {
                AplMas.Rollid = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FK_RoleID"]);
            }
            #endregion

            DTAppeal = OBJAppl.GetLibraryVideoDashBoard(Convert.ToInt32(LB.Lib_Id));
            try
            {
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new LibraryDocumentModel
                            {
                                //PK_SubSubID = Convert.ToInt32(dr["PK_SubSubID"]),
                                Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                LP_Id = Convert.ToInt32(dr["LP_Id"]),
                                //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy"),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                //Modified_Date = Convert.ToString(dr["Modified_Date"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                ModifyBy = Convert.ToString(dr["ModifyBy"]),
                                PDF = Convert.ToString(dr["PDF"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["LibraryList"] = lstAppeal;
            LibraryDocumentModel IVRNew = new LibraryDocumentModel();
            IVRNew.Lib_Id = LB.Lib_Id;
            IVRNew.Rollid = AplMas.Rollid;
            return View(IVRNew);
        }


        public ActionResult DeleteLibraryDocuments(int? LP_Id, int? Lib_Id)
        {
            int Result = 0;
            try
            {
                Result = OBJAppl.DeleteLibraryDocsData(LP_Id);
                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    return RedirectToAction("LibraryDocuments", new { Lib_Id = Lib_Id });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        public FileResult GetReport(string FileName)
        {         
            string ReportURL = Path.Combine(Server.MapPath("~/LibraryFiles/") + FileName);
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = FileName,
                Inline = true
                // false = prompt the user for downloading;  
                // true = browser to try to show the file inline
            };
            
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
           

           
            return File(FileBytes, "application/pdf", "");
        }

       

        public ActionResult GetPdf(string FileName)
           {
               string ReportURL = Path.Combine(Server.MapPath("~/LibraryFiles/") + FileName);
               byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
              // Response.AppendHeader("Content-Disposition", "inline;test.pdf");
               Response.Headers.Add("Content-Disposition", "inline");
               /// return File(FileBytes, "application/pdf");
               return View();
           }



         [HttpPost]
         public JsonResult GetPDF(string FileName)
         {
            // string ReportURL = Path.Combine(Server.MapPath("~/LibraryFiles/") + FileName);

            string ReportURL = "/LibraryFiles/" + FileName + "#toolbar=0";
            string httpFile = ConfigurationManager.AppSettings["Web"].ToString().Trim() + "/LibraryFiles/" + FileName.Trim();
            //   FileStream fs = new FileStream(ReportURL, FileMode.Open, FileAccess.Read);
          return Json(ReportURL);
            

            
         }

        /*
         public ActionResult GetPDF(LibraryDocumentModel Lib)
         {


             return View(Lib);
         }

         public ActionResult GetPDFFile(string FileName)
         {
             LibraryDocumentModel Lib = new LibraryDocumentModel();
             Lib.FileName = FileName;

             return View("GetPDF", Lib);
         }
         */

        [HttpGet]
        public ActionResult CVFolder(int? id, int? Lib_Id)
        {
            DataTable DTAppeal = new DataTable();
            List<Library> lstAppeal = new List<Library>();

            if (id == 39)
            { Session["Doc"] = "It/It Manual"; }
            else if (id == 40)
            { Session["Doc"] = "It/It Formats"; }
            else if (id == 118)
            { Session["Doc"] = "Inspection / eLibrary"; }
            else if (id == 119)
            { Session["Doc"] = "Inspection / Technical Circulars"; }
            else if (id == 120)
            { Session["Doc"] = "Inspection / Inspectors CV"; }
            else if (id == 56)
            { Session["Doc"] = "Inspection / Work Instruction"; }
            else if (id == 57)
            { Session["Doc"] = "Inspection / Inspection Formats"; }
            else if (id == 58)
            { Session["Doc"] = "Inspection / Sales and business development"; }
            else if (id == 59)
            { Session["Doc"] = "Inspection / Special Services"; }
            else if (id == 60)
            { Session["Doc"] = "Inspection / Expediting Services"; }
            else if (id == 61)
            { Session["Doc"] = "Inspection / Engineering Services"; }
            else if (id == 62)
            { Session["Doc"] = "Inspection / QA Records"; }
            else if (id == 63)
            { Session["Doc"] = "Inspection / Training"; }
            else if (id == 64)
            { Session["Doc"] = "Inspection / Others"; }

            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = OBJAppl.GetUserRoll();
            if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
            {
                AplMas.Rollid = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FK_RoleID"]);
            }
            Library IVRNew = new Library();
            IVRNew.PK_SubSubID = id;
            Session["SubID"] = IVRNew.PK_SubSubID;
            IVRNew.Rollid = AplMas.Rollid;
            DateTime s = new DateTime();
            if (Lib_Id > 0)
            {
                DTAppeal = OBJAppl.GetLibraryDataByID(Convert.ToInt32(Lib_Id));
                try
                {
                    if (DTAppeal.Rows.Count > 0)
                    {
                        AplMas.FolderName = Convert.ToString(DTAppeal.Rows[0]["FolderName"]);
                        AplMas.Lib_Id = Convert.ToInt32(DTAppeal.Rows[0]["Lib_Id"]);
                        foreach (DataRow dr in DTAppeal.Rows)
                        {
                            lstAppeal.Add(
                                new Library
                                {
                                    PK_SubSubID = Convert.ToInt32(dr["PK_SubSubID"]),
                                    Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                    //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy"),
                                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                    CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                    ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                    //  ModifyDate = dr["ModifyDate"] !=null ? Convert.ToDateTime(dr["ModifyDate"]) : s ,
                                    ModifyBy = Convert.ToString(dr["ModifyBy"]),
                                    FolderName = Convert.ToString(dr["FolderName"]),
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
                ViewData["LibraryList"] = lstAppeal;
                return View(AplMas);
            }
            else
            {
                DTAppeal = OBJAppl.GetLibraryDashBoard(Convert.ToInt32(id));
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new Library
                            {
                                PK_SubSubID = Convert.ToInt32(dr["PK_SubSubID"]),
                                Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy"),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                //  ModifyDate = dr["ModifyDate"] !=null ? Convert.ToDateTime(dr["ModifyDate"]) : s ,
                                ModifyBy = Convert.ToString(dr["ModifyBy"]),
                                FolderName = Convert.ToString(dr["FolderName"]),
                            }
                            );
                    }
                }
                ViewData["LibraryList"] = lstAppeal;
            }
            return View(IVRNew);
        }

        [HttpGet]
        public ActionResult CVDocuments(LibraryDocumentModel LDM, string FolderName)
        {
            DataTable DTAppeal = new DataTable();
            List<LibraryDocumentModel> lstAppeal = new List<LibraryDocumentModel>();
            LibraryDocumentModel IVRNew = new LibraryDocumentModel();
            #region Library
            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = OBJAppl.GetUserRoll();
            if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
            {
                AplMas.Rollid = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FK_RoleID"]);
            }
            #endregion
            IVRNew.FolderName = FolderName;
            DTAppeal = OBJAppl.GetCV(Convert.ToInt32(LDM.Lib_Id), IVRNew.FolderName);
            try
            {
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new LibraryDocumentModel
                            {
                                //PK_SubSubID = Convert.ToInt32(dr["PK_SubSubID"]),
                                //Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                //LP_Id = Convert.ToInt32(dr["LP_Id"]),
                                ////CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy"),
                                //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                //CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                ////Modified_Date = Convert.ToString(dr["Modified_Date"]),

                                //ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                //ModifyBy = Convert.ToString(dr["ModifyBy"]),

                                LP_Id = Convert.ToInt32(dr["LP_Id"]),
                                PDF = Convert.ToString(dr["PDF"]),
                                CreatedBy = Convert.ToString(dr["Name"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["LibraryList"] = lstAppeal;

            IVRNew.Lib_Id = LDM.Lib_Id;
            IVRNew.Rollid = AplMas.Rollid;
            return View(IVRNew);

        }

        [HttpGet]
        public ActionResult TutorialFolder(int? id, int? Lib_Id)
        {
            DataTable DTAppeal = new DataTable();
            List<Library> lstAppeal = new List<Library>();

            if (id == 39)
            { Session["Doc"] = "It/It Manual"; }
            else if (id == 40)
            { Session["Doc"] = "It/It Formats"; }
            else if (id == 118)
            { Session["Doc"] = "Inspection / eLibrary"; }
            else if (id == 119)
            { Session["Doc"] = "Inspection / Technical Circulars"; }
            else if (id == 120)
            { Session["Doc"] = "Inspection / Inspectors CV"; }
            else if (id == 56)
            { Session["Doc"] = "Inspection / Work Instruction"; }
            else if (id == 57)
            { Session["Doc"] = "Inspection / Inspection Formats"; }
            else if (id == 58)
            { Session["Doc"] = "Inspection / Sales and business development"; }
            else if (id == 59)
            { Session["Doc"] = "Inspection / Special Services"; }
            else if (id == 60)
            { Session["Doc"] = "Inspection / Expediting Services"; }
            else if (id == 61)
            { Session["Doc"] = "Inspection / Engineering Services"; }
            else if (id == 62)
            { Session["Doc"] = "Inspection / QA Records"; }
            else if (id == 63)
            { Session["Doc"] = "Inspection / Training"; }
            else if (id == 64)
            { Session["Doc"] = "Inspection / Others"; }
            else if (id == 281)
            { Session["Doc"] = "Tutorial / Videos"; }

            else//27/05/2024
            { Session["Doc"] = "Inspection / Others"; }

            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = OBJAppl.GetUserRoll();
            if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
            {
                AplMas.Rollid = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FK_RoleID"]);
            }


            Library IVRNew = new Library();
            IVRNew.PK_SubSubID = id;
            Session["SubID"] = IVRNew.PK_SubSubID;
            IVRNew.Rollid = AplMas.Rollid;



            DateTime s = new DateTime();
            if (Lib_Id > 0)
            {
                DTAppeal = OBJAppl.GetLibraryDataByID(Convert.ToInt32(Lib_Id));
                try
                {
                    if (DTAppeal.Rows.Count > 0)
                    {
                        AplMas.FolderName = Convert.ToString(DTAppeal.Rows[0]["FolderName"]);
                        AplMas.Lib_Id = Convert.ToInt32(DTAppeal.Rows[0]["Lib_Id"]);
                        foreach (DataRow dr in DTAppeal.Rows)
                        {
                            lstAppeal.Add(
                                new Library
                                {
                                    PK_SubSubID = Convert.ToInt32(dr["PK_SubSubID"]),
                                    Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                    //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy"),
                                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                    CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                    ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                    //  ModifyDate = dr["ModifyDate"] !=null ? Convert.ToDateTime(dr["ModifyDate"]) : s ,
                                    ModifyBy = Convert.ToString(dr["ModifyBy"]),
                                    FolderName = Convert.ToString(dr["FolderName"]),
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
                ViewData["LibraryList"] = lstAppeal;
                return View(AplMas);
            }
            else
            {
                DTAppeal = OBJAppl.GetLibraryDashBoard(Convert.ToInt32(id));
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new Library
                            {
                                PK_SubSubID = Convert.ToInt32(dr["PK_SubSubID"]),
                                Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy"),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                //  ModifyDate = dr["ModifyDate"] !=null ? Convert.ToDateTime(dr["ModifyDate"]) : s ,
                                ModifyBy = Convert.ToString(dr["ModifyBy"]),
                                FolderName = Convert.ToString(dr["FolderName"]),
                            }
                            );
                    }
                }
                ViewData["LibraryList"] = lstAppeal;
            }
            return View(IVRNew);
        }

        [HttpPost]
        public ActionResult TutorialFolder(Library LB, FormCollection fc)
        {
            string Result = string.Empty;


            string[] EmpCat;
            string ListEmpCat = string.Empty;
            ListEmpCat = string.Join(",", fc["ProductList"]);
            EmpCat = ListEmpCat.Split(',');
            ViewData["ListEmpCategory"] = ListEmpCat;


            string[] InspLoc;
            string ListInspLoc = string.Empty;
            ListInspLoc = string.Join(",", fc["ProductList1"]);
            InspLoc = ListInspLoc.Split(',');
            ViewData["ListInspLoc"] = ListInspLoc;


            string[] UserRole;
            string ListUserRole = string.Empty;
            ListUserRole = string.Join(",", fc["ProductList2"]);
            UserRole = ListUserRole.Split(',');
            ViewData["ListUserRole"] = ListUserRole;


            Result = OBJAppl.InsertUpdateTutorial(LB);


            if (Result != null && Result != "")
            {
                TempData["Success"] = "Folder Added Successfully...!!!";
                TempData.Keep();
                //TempData["UpdateCompany"] = Result;
                return RedirectToAction("TutorialFolder", new { id = Session["SubID"] });
            }
            else
            {
                TempData["Error"] = "Something went wrong, try again...!!!";
                TempData.Keep();
                return RedirectToAction("TutorialFolder", new { id = Session["SubID"] });
            }

            return RedirectToAction("TutorialFolder", new { id = Session["SubID"] });

        }


        [HttpGet]
        public ActionResult TDocuments(LibraryDocumentModel LDM, string FolderName)
        {
            DataTable DTAppeal = new DataTable();
            List<LibraryDocumentModel> lstAppeal = new List<LibraryDocumentModel>();
            LibraryDocumentModel IVRNew = new LibraryDocumentModel();
            #region Library
            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = OBJAppl.GetUserRoll();
            if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
            {
                AplMas.Rollid = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FK_RoleID"]);
            }
            #endregion

            DataSet DSGetEditQuotationAllddllst = new DataSet();

            List<NameCode> lstInspectionLocation = new List<NameCode>();
            List<NameCode> lstEmpCategory = new List<NameCode>();
            List<NameCode> lstUserRole = new List<NameCode>();

            if (AplMas.Rollid == 36)
            {

                DSGetEditQuotationAllddllst = OBJAppl.GetEditAllddlLst();

                if (DSGetEditQuotationAllddllst.Tables[1].Rows.Count > 0)
                {
                    lstInspectionLocation = (from n in DSGetEditQuotationAllddllst.Tables[1].AsEnumerable()
                                             select new NameCode()
                                             {
                                                 Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[1].Columns["Name"].ToString()),
                                                 Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[1].Columns["PK_ID"].ToString())

                                             }).ToList();
                }
                IEnumerable<SelectListItem> InspectionLocationItems;
                InspectionLocationItems = new SelectList(lstInspectionLocation, "Code", "Name");
                ViewBag.InspectionLocation = InspectionLocationItems;
                ViewBag.InspectionLocationItems = InspectionLocationItems;


                if (DSGetEditQuotationAllddllst.Tables[0].Rows.Count > 0)
                {
                    lstEmpCategory = (from n in DSGetEditQuotationAllddllst.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[0].Columns["Name"].ToString()),
                                          Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[0].Columns["PK_ID"].ToString())

                                      }).ToList();
                }
                IEnumerable<SelectListItem> EmpCategory;
                EmpCategory = new SelectList(lstEmpCategory, "Code", "Name");
                ViewBag.lstEmpCategory = EmpCategory;
                ViewBag.lstEmpCategory = EmpCategory;


                if (DSGetEditQuotationAllddllst.Tables[2].Rows.Count > 0)
                {
                    lstUserRole = (from n in DSGetEditQuotationAllddllst.Tables[2].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(DSGetEditQuotationAllddllst.Tables[2].Columns["Name"].ToString()),
                                       Code = n.Field<Int32>(DSGetEditQuotationAllddllst.Tables[2].Columns["PK_ID"].ToString())

                                   }).ToList();
                }

                IEnumerable<SelectListItem> UserRoles;
                UserRoles = new SelectList(lstUserRole, "Code", "Name");
                ViewBag.lstUserRole = UserRoles;
                ViewBag.lstUserRole = UserRoles;
            }


            IVRNew.FolderName = FolderName;
            DTAppeal = OBJAppl.GetLibraryDocsDashBoard(Convert.ToInt32(LDM.Lib_Id));
            try
            {
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new LibraryDocumentModel
                            {
                                //PK_SubSubID = Convert.ToInt32(dr["PK_SubSubID"]),
                                Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                LP_Id = Convert.ToInt32(dr["LP_Id"]),
                                //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy"),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                //Modified_Date = Convert.ToString(dr["Modified_Date"]),

                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                ModifyBy = Convert.ToString(dr["ModifyBy"]),
                                PDF = Convert.ToString(dr["PDF"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["LibraryList"] = lstAppeal;

            IVRNew.Lib_Id = LDM.Lib_Id;
            IVRNew.Rollid = AplMas.Rollid;
            return View(IVRNew);

        }
        [HttpPost]
        public ActionResult TDocuments(LibraryDocumentModel LB, FormCollection fc, HttpPostedFileBase[] multiImage)
        {
            string Result = string.Empty;

            #region Image Upload Code

            var list = Session["list"] as List<string>;

            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["TFile"] as List<FileDetails>;
            string fileName = string.Empty;
            foreach (var item in lstFileDtls)
            {
                fileName = item.FileName.ToString();

            }
            LB.FileName = fileName;
            LB.PDF = fileName;






            Result = OBJAppl.InsertUpdateTDocsData(LB);


            if (Result != null && Result != "")
            {
                if (lstFileDtls != null && lstFileDtls.Count > 0)
                {
                    string type = null;
                    objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(Result));
                    //// Result = OBJAppl.InsertFileAttachment(lstFileDtls, Result, type);
                    Session["list"] = null;
                }

                TempData["UpdateCompany"] = Result;
            }









            //}
            #endregion



            DataTable DTAppeal = new DataTable();
            List<LibraryDocumentModel> lstAppeal = new List<LibraryDocumentModel>();

            #region Library
            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = OBJAppl.GetUserRoll();
            if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
            {
                AplMas.Rollid = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FK_RoleID"]);
            }
            #endregion

            DTAppeal = OBJAppl.GetLibraryDocsDashBoard(Convert.ToInt32(LB.Lib_Id));
            try
            {
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new LibraryDocumentModel
                            {

                                Lib_Id = Convert.ToInt32(dr["Lib_Id"]),
                                LP_Id = Convert.ToInt32(dr["LP_Id"]),

                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),

                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                ModifyBy = Convert.ToString(dr["ModifyBy"]),
                                PDF = Convert.ToString(dr["PDF"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["LibraryList"] = lstAppeal;
            LibraryDocumentModel IVRNew = new LibraryDocumentModel();
            IVRNew.Lib_Id = LB.Lib_Id;
            IVRNew.Rollid = AplMas.Rollid;
            return View(IVRNew);
        }

        public ActionResult DeleteTDocuments(int? LP_Id, int? Lib_Id)
        {
            int Result = 0;
            try
            {
                Result = OBJAppl.DeleteLibraryDocsData(LP_Id);
                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    return RedirectToAction("LibraryDocuments", new { Lib_Id = Lib_Id });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        public JsonResult TemporaryFilePathQuotationAttachment()
        {

            var IPath = string.Empty;

            string[] splitedGrp;

            List<string> Selected = new List<string>();

            //Adding New Code 12 March 2020

            List<FileDetails> fileDetails = new List<FileDetails>();
            //added by nikita on 24062024
            List<FileDetails> fileDetailsFormat = new List<FileDetails>();
            //---Adding end Code

            if (Session["TFile"] != null)
            {
                fileDetails = Session["TFile"] as List<FileDetails>;
            }



            try

            {

                FormCollection fc = new FormCollection();

                string filePath = string.Empty;
                string strEXEPath = string.Empty;
                string OutfilePath = string.Empty;

                OutfilePath = ConfigurationManager.AppSettings["OutVideoFilePath"].ToString();
                strEXEPath = ConfigurationManager.AppSettings["ExeFile"].ToString();

                for (int i = 0; i < Request.Files.Count; i++)
                {

                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file

                    int fileSize = files.ContentLength;

                    if (files != null && files.ContentLength > 0)
                    {
                        if (files.FileName.ToUpper().EndsWith(".MP4") || files.FileName.ToUpper().EndsWith(".MPE4"))
                        {

                            string fileName = files.FileName;
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

                            filePath = Path.Combine(Server.MapPath("~/LibraryFiles/"), fileDetail.Id + fileDetail.Extension);
                            files.SaveAs(filePath);
                            OutfilePath = OutfilePath + fileDetail.Id + fileDetail.Extension;
                            string subtitleFile = @"D:\transcript.srt";


                            try
                            {

                                VideoCompression objvideocompress = new VideoCompression();
                                objvideocompress.CompressVideos(strEXEPath, filePath, OutfilePath);

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }



                            var ExistingUploadFile = "~/LibraryFiles/" + fileName;

                            splitedGrp = ExistingUploadFile.Split(',');

                            foreach (var single in splitedGrp)
                            {
                                Selected.Add(single);
                            }
                            Session["TFile"] = Selected;
                        }
                        else
                        {
                            ViewBag.Error = "Please Select MP4 or MPE4 Video File";
                        }
                    }
                }



                if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD")
                {
                    Session["TFile"] = fileDetails;
                }

            }

            catch (Exception ex)

            {

                string Error = ex.Message.ToString();

            }

            return Json(IPath, JsonRequestBehavior.AllowGet);

        }


        public class VideoCompression
        {

            public void CompressVideos(string exePath, string InputFile, string OutputFile)
            {
                try
                {
                    //string inputFile = @"D:\UP.Mp4";
                    //string outputFile = @"D:\VIDEOFILES\output_" + DateTime.Now.Ticks + ".mp4";
                    //string ffmpegPath = @"D:\ffmpeg.exe";  // Replace with the path to your FFmpeg executable

                    // FFmpeg command to compress the video (lower bitrate for compression)
                    string arguments = $"-i \"{InputFile}\" -vcodec libx264 -crf 28 \"{OutputFile}\"";

                    // Start the FFmpeg process to compress the video
                    Process process = new Process();
                    process.StartInfo.FileName = exePath;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                    process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

                    // Start the process
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                    Console.WriteLine("Video compression completed!");

                }
                catch (Exception ex)
                {
                    string strErr = ex.Message.ToString();
                }

            }

            public void CompressVideoAndAddTranscript(string exePath, string InputFile, string OutputFile, string subtitleFile)
            {
                // Path to the FFmpeg executable
                ////  string ffmpegPath = @"C:\path\to\ffmpeg.exe";

                // Command to compress the video and add subtitles
                ///  string arguments = $"-i \"{InputFile}\" -vf scale=1280:720 -c:v libx264 -crf 23 -preset fast -c:a aac -strict experimental -c:s mov_text -i \"{subtitleFile}\" \"{OutputFile}\"";
                string arguments = $"-i \"{InputFile}\" -vcodec libx264 -crf 28 \"{OutputFile}\"";
                // Set up the process to run FFmpeg
                // Start the FFmpeg process to compress the video
                Process process = new Process();
                process.StartInfo.FileName = exePath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

                // Start the process
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                Console.WriteLine("Video compression completed!");
            }


            public static void ExtractAudioFromVideo(string videoFile, string audioFile)
            {
                // Path to FFmpeg executable
                string ffmpegPath = @"C:\path\to\ffmpeg.exe"; // Update with your FFmpeg path

                // FFmpeg command to extract audio from video
                string arguments = $"-i \"{videoFile}\" -vn -acodec pcm_s16le -ar 44100 -ac 2 \"{audioFile}\"";

                // Set up the process to run FFmpeg
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        process.WaitForExit();

                        Console.WriteLine("FFmpeg Output: " + output);
                        Console.WriteLine("FFmpeg Error: " + error);
                    }
                }
            }

            //public static async Task<string> TranscribeAudioAsync(string audioFile)
            //{
            //    string apiKey = "YOUR_AZURE_API_KEY"; // Replace with your Azure Speech API key
            //    string region = "YOUR_AZURE_REGION"; // Replace with your Azure region (e.g., "eastus")

            //    // Create speech configuration
            //    var speechConfig = SpeechConfig.FromSubscription(apiKey, region);
            //    var audioConfig = AudioConfig.FromWavFileInput(audioFile);

            //    // Create a speech recognizer
            //    var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

            //    // Start transcription and get the result
            //    var result = await recognizer.RecognizeOnceAsync();

            //    if (result.Reason == ResultReason.RecognizedSpeech)
            //    {
            //        return result.Text; // Return the recognized text
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Speech recognition failed: {result.Reason}");
            //        return null;
            //    }
            //}
        }
    }
}
 