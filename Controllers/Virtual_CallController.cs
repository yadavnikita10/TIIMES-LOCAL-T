using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{

    public class Virtual_CallController : Controller
    {
        DALCalls objDalCalls = new DALCalls();
        DALCallMaster objDAM = new DALCallMaster();
        Calls UserCalls = new Calls();
        // GET: Virtual_Call

        public ActionResult Index()
        {
            return View();
        }
       public ActionResult CallsDashBoard()
       {
            Session["GetExcelData"] = "Yes";
           DataTable DTCallDashBoard = new DataTable();
           List<Calls> lstCallDashBoard = new List<Calls>();
           DTCallDashBoard = objDAM.GetCallDashBoard();
      
       
           try 
           {
               if (DTCallDashBoard.Rows.Count > 0)
               {
                   foreach (DataRow dr in DTCallDashBoard.Rows)
                   {
                        lstCallDashBoard.Add(
                            new Calls
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Quantity = Convert.ToInt32(dr["Quantity"]),
                                Description = Convert.ToString(dr["Description"]),
                                ContactName = Convert.ToString(dr["Contact_Name"]),
                                PO_Number = Convert.ToString(dr["VendorPoNo"]),
                                AssignStatus = Convert.ToString(dr["SubVendorPoNo"]),
                                Job = Convert.ToString(dr["Job"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                UserRole = Convert.ToString(dr["SubVendorName"]),
                               Sub_Job = Convert.ToString(dr["Sub_job"]),
                               Job_Location = Convert.ToString(dr["job_Location"]),
                               Inspector = Convert.ToString(dr["Inspector"]),
                               Status=Convert.ToString(dr["status"]),
                               Urgency=Convert.ToString(dr["Urgency"]),
                               PO_Date=Convert.ToString(dr["Po_Date"]),
                               Last_Updated=Convert.ToString(dr["Last_Updated"]),
                               Assighned_To=Convert.ToString(dr["Assighned_To"]),
                               ProductList=Convert.ToString(dr["Product_item"])    ,
                               VirtualCallStatus = Convert.ToString(dr["VirtualCallStatus"]),
                               Call_No= Convert.ToString(dr["Call_No"]),
                               Company_Name = Convert.ToString(dr["Company_Name"])
                           }
                           );
                   }

               }
           }
           catch (Exception ex)
           {
               string Error = ex.Message.ToString();
           }
           ViewData["CallList"] = lstCallDashBoard;

            UserCalls.lstCallDashBoard1 = lstCallDashBoard;
           return View(UserCalls);
       }

        [HttpPost]
        public ActionResult CallsDashBoard(Calls U)
        {
            Session["GetExcelData"] = null;
            DataTable DTCallDashBoard = new DataTable();
            List<Calls> lstCallDashBoard = new List<Calls>();

            Session["FromDate"] = U.FromDate;
            Session["ToDate"] = U.ToDate;
            DTCallDashBoard = objDAM.GetCallDashBoardByDate(U);


            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new Calls
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Quantity = Convert.ToInt32(dr["Quantity"]),
                                Description = Convert.ToString(dr["Description"]),
                                ContactName = Convert.ToString(dr["Contact_Name"]),
                                PO_Number = Convert.ToString(dr["VendorPoNo"]),
                                AssignStatus = Convert.ToString(dr["SubVendorPoNo"]),
                                Job = Convert.ToString(dr["Job"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                UserRole = Convert.ToString(dr["SubVendorName"]),
                                Sub_Job = Convert.ToString(dr["Sub_job"]),
                                Job_Location = Convert.ToString(dr["job_Location"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Status = Convert.ToString(dr["status"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                PO_Date = Convert.ToString(dr["Po_Date"]),
                                Last_Updated = Convert.ToString(dr["Last_Updated"]),
                                Assighned_To = Convert.ToString(dr["Assighned_To"]),
                                ProductList = Convert.ToString(dr["Product_item"]),
                                VirtualCallStatus = Convert.ToString(dr["VirtualCallStatus"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"])
                            }
                            );
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallList"] = lstCallDashBoard;
            UserCalls.lstCallDashBoard1 = lstCallDashBoard;

            return View(UserCalls);
        }


        //[HttpGet]
        //public ActionResult Insert_Call_List()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Insert_Call_List(Calls ca)
        //{

        //    objDAM.InsertUpdateCallData(ca);

        //    return View();
        //}
        //public ActionResult UpdateProfile()
        //{
        //    string CallID = Convert.ToString(Session["CallIDs"]);

        //    if (CallID != "" && CallID != null)
        //    {
        //        DataTable DTEditCalls = new DataTable();
        //        DTEditCalls = objDAM.EditUsers(CallID);
        //        if (DTEditCalls.Rows.Count > 0)
        //        {
        //            UserCalls.PK_Call_ID = Convert.ToInt32(DTEditCalls.Rows[0]["PK_Call_ID"]);
        //            UserCalls.ContactName = Convert.ToString(DTEditCalls.Rows[0]["Contact_Name"]);
        //            UserCalls.Job = Convert.ToString(DTEditCalls.Rows[0]["Job"]);
        //            UserCalls.Vendor_Name = Convert.ToString(DTEditCalls.Rows[0]["Vendor_Name"]);
        //            UserCalls.Sub_Job = Convert.ToString(DTEditCalls.Rows[0]["Sub_job"]);
        //            UserCalls.Job_Location = Convert.ToString(DTEditCalls.Rows[0]["job_Location"]);
        //            UserCalls.Inspector = Convert.ToString(DTEditCalls.Rows[0]["Inspector"]);
        //        }
        //    }
        //        return View(UserCalls);
        //}
        [HttpGet]
     public ActionResult CreateCall(int? PK_Call_ID)
     {
            #region chk role
            DataSet dschkRole = new DataSet();
            dschkRole = objDAM.ChkRole();
            if(dschkRole.Tables[0].Rows.Count>0)
            { 
            string Role = Convert.ToString(dschkRole.Tables[0].Rows[0]["RoleName"]);

            if (Role != null || Role != "" )
            {
                UserCalls.UserRole = "Visible";
            }
            else
            {
                UserCalls.UserRole = "InVisible";
            }
            }
            else
            {
                UserCalls.UserRole = "InVisible";
            }

            #endregion

            var Data = objDalCalls.GetBranchList();
            ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");
            DataTable dtExecutingBranch = new DataTable();


            // Bind Executing Branch
            //if (PK_Call_ID != 0 && PK_Call_ID != null)
            //{
                //var varExcutingBranch = objDalCalls.GetExecutingList();
                //ViewBag.ExecutingBranch = new SelectList(varExcutingBranch, "Br_Id", "Branch_Name");
                dtExecutingBranch = objDalCalls.dtGetExecutingList();
            if(dtExecutingBranch.Rows.Count>0)
            {
                UserCalls.Executing_Branch = Convert.ToString(dtExecutingBranch.Rows[0]["Br_Id"]);
            }
                
            //}
            

            DataTable DTCallDashBoard = new DataTable();
          
            List<Calls> lstCompanyDashBoard = new List<Calls>();
           ViewData["Drpproduct"]= objDAM.GetDrpList();

          
            var Data1 = objDAM.GetDrpList();
            ViewBag.SubCatlistss = new SelectList(Data1, "Product_ID", "Product_Name");

            //ViewBag.DrpList = lstCompanyDashBoard;
            
            if (PK_Call_ID != 0 && PK_Call_ID != null)
            {

                //Added by Ankush for Delete file and update file
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDAM.EditUploadedFile(PK_Call_ID);
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
                    UserCalls.FileDetails = lstEditFileDetails;
                }
                //Added by Ankush for Delete file and update file

                string[] splitedProduct_Name;
                ViewBag.check = "productcheck";
                DataTable DTEditCall = new DataTable();
                DTEditCall = objDAM.EditCalls(PK_Call_ID);
                if (DTEditCall.Rows.Count > 0)
                {
                    UserCalls.PK_Call_ID = Convert.ToInt32(DTEditCall.Rows[0]["PK_Call_ID"]);
                    UserCalls.Quantity = Convert.ToInt32(DTEditCall.Rows[0]["Quantity"]);
                    UserCalls.PO_Number = Convert.ToString(DTEditCall.Rows[0]["Po_Number"]);
                    UserCalls.ContactName = Convert.ToString(DTEditCall.Rows[0]["Contact_Name"]);
                    UserCalls.Last_Updated = Convert.ToString(DTEditCall.Rows[0]["Last_Updated"]);
                    UserCalls.Assighned_To = Convert.ToString(DTEditCall.Rows[0]["Assighned_To"]);
                    UserCalls.PO_Date = Convert.ToString(DTEditCall.Rows[0]["Po_Date"]);
                    UserCalls.Description = Convert.ToString(DTEditCall.Rows[0]["Description"]);
                    UserCalls.Planned_Date = Convert.ToString(DTEditCall.Rows[0]["Planned_Date"]);
                    UserCalls.Company_Name = Convert.ToString(DTEditCall.Rows[0]["Company_Name"]);
                    UserCalls.Project_Name = Convert.ToString(DTEditCall.Rows[0]["Project_Name"]);
                    UserCalls.Job_Location = Convert.ToString(DTEditCall.Rows[0]["Job_Location"]);
                    UserCalls.Status = Convert.ToString(DTEditCall.Rows[0]["Status"]);
                    UserCalls.Call_received_date = Convert.ToString(DTEditCall.Rows[0]["Call_Recived_date"]);
                    UserCalls.Call_Request_Date = Convert.ToString(DTEditCall.Rows[0]["Call_Request_Date"]);
                    UserCalls.Category = Convert.ToString(DTEditCall.Rows[0]["category"]);
                    UserCalls.Source = Convert.ToString(DTEditCall.Rows[0]["Source"]);
                    UserCalls.Urgency = Convert.ToString(DTEditCall.Rows[0]["Urgency"]);
                    UserCalls.Sub_Category = Convert.ToString(DTEditCall.Rows[0]["Sub_Category"]);
                    UserCalls.Type = Convert.ToString(DTEditCall.Rows[0]["Type"]);
                    UserCalls.Vendor_Name = Convert.ToString(DTEditCall.Rows[0]["Vendor_Name"]);
                    UserCalls.Client_Email = Convert.ToString(DTEditCall.Rows[0]["Client_Email"]);
                    UserCalls.Vendor_Email = Convert.ToString(DTEditCall.Rows[0]["Vendor_Email"]);
                    UserCalls.Tuv_Branch = Convert.ToString(DTEditCall.Rows[0]["Tuv_Branch"]);
                    UserCalls.ProductList = Convert.ToString(DTEditCall.Rows[0]["Product_item"]);
                    UserCalls.Attachment = Convert.ToString(DTEditCall.Rows[0]["Attachment"]);
                    UserCalls.Sub_Job = Convert.ToString(DTEditCall.Rows[0]["Sub_Job"]);
                    UserCalls.VirtualCallStatus = Convert.ToString(DTEditCall.Rows[0]["VirtualCallStatus"]);
                    UserCalls.Executing_Branch = Convert.ToString(DTEditCall.Rows[0]["Executing_Branch"]);
                    UserCalls.Originating_Branch = Convert.ToString(DTEditCall.Rows[0]["Originating_Branch"]);
                    UserCalls.Job = Convert.ToString(DTEditCall.Rows[0]["Job"]);
                }
                List<string> Selected = new List<string>();
                var Existingins = Convert.ToString(DTEditCall.Rows[0]["Product_item"]);
                splitedProduct_Name = Existingins.Split(',');
                foreach (var single in splitedProduct_Name)
                {
                    Selected.Add(single);
                }
                ViewBag.EditproductName = Selected;

            }
            DataTable DTGetProductLst = new DataTable();
            List<NameCodeProduct> lstEditInspector = new List<NameCodeProduct>();
            DTGetProductLst = objDAM.getlistforEdit();

            if (DTGetProductLst.Rows.Count > 0)
            {
                lstEditInspector = (from n in DTGetProductLst.AsEnumerable()
                                    select new NameCodeProduct()
                                    {
                                        Name = n.Field<string>(DTGetProductLst.Columns["Name"].ToString())


                                    }).ToList();
            }

            IEnumerable<SelectListItem> ProductcheckItems;
            ProductcheckItems = new SelectList(lstEditInspector, "Name", "Name");
            ViewBag.ProjectTypeItems = ProductcheckItems;
            ViewData["ProjectTypeItems"] = ProductcheckItems;
            return View(UserCalls);
        }
        [HttpPost]
    
        public ActionResult CreateCall(Calls UserCall,FormCollection fc, List<HttpPostedFileBase> img_Banner)
        {
            string Result = string.Empty;

            int VCID = 0;
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listVCUploadedFile"] as List<FileDetails>;

            string ProList = string.Join(",", fc["ProductList"]);
            UserCall.ProductList = ProList;
            //UserCall.ProductList = string.Join(",", fc["ProductList"]);
            /*  UserCall.ProductList = string.Join(",", UserCall.ProductList);*///1,2,3 saved
            string IPath = string.Empty;
            var list = Session["list"] as List<string>;
            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }
            List<string> lstAttachment = new List<string>();
            if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
            {
                foreach (HttpPostedFileBase single in img_Banner) // Added by Sagar Panigrahi
                {
                    //HttpPostedFileBase Imagesection;
                    //Imagesection = Request.Files[single];
                    if (single != null && single.FileName != "")
                    {
                        var filename = CommonControl.FileUpload("~/VirtualCallDocument/", single);
                        lstAttachment.Add(filename);
                    }
                }
                UserCall.Attachment = string.Join(",", lstAttachment);
                if (string.IsNullOrEmpty(UserCall.Attachment))
                {
                    UserCall.Attachment = "NoImage.gif";
                }
            }
            else
            {
                UserCall.Attachment = "NoImage.gif";
            }


            try
            {
                if ( UserCall.PK_Call_ID != 0)
                {
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

                    UserCall.Call_No = Convert.ToString(ConfirmCode) + ConfirmSecondCode;
                    Result = objDAM.InsertUpdateCallData(UserCall,IPath);

                    VCID = UserCall.PK_Call_ID;
                    if (VCID != null && VCID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            //Result = objDAM.InsertFileAttachment(lstFileDtls, VCID);
                            Session["listVCUploadedFile"] = null;
                        }
                    }

                    if (Result != "" && Result != null)
                    {
                        TempData["UpdateCalls"] = Result;
                        return RedirectToAction("CallsDashBoard");
                    }
                }
                else
                {
                    UserCall.AssignStatus = "0";
                    UserCall.Status = "Open";
                    //UserCall.Status = "Assigned";
                    Result = objDAM.InsertUpdateCallData(UserCall, IPath);

                    VCID = Convert.ToInt32(Session["VCIDs"]);
                    if (VCID != null && VCID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            //Result = objDAM.InsertFileAttachment(lstFileDtls, VCID);
                            Session["listVCUploadedFile"] = null;
                        }
                    }

                    TempData["insertCalls"] = Result;

                    return RedirectToAction("CallsDashBoard");
                }
               

            }
            catch(Exception ex)
            {

                string Error = ex.Message.ToString();
            }
            return View();
        }

        public JsonResult GetAutoCompleteData(string VendorName)
        {
            DataTable DTCallDashBoard = new DataTable();
            List<Calls> lstCallDashBoard = new List<Calls>();
            DTCallDashBoard = objDAM.GetCallDashBoard();

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new Calls
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                ContactName = Convert.ToString(dr["Contact_Name"]),
                                PO_Number = Convert.ToString(dr["Po_Number"]),
                                Job = Convert.ToString(dr["Job"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                Sub_Job = Convert.ToString(dr["Sub_job"]),
                                Job_Location = Convert.ToString(dr["job_Location"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            var getList = from n in lstCallDashBoard
                    where n.Vendor_Name.StartsWith(VendorName)
                    select new { n.Vendor_Name };
            return Json(getList);
        }
        //public JsonResult GetAutoCompleteDataPItem(string Productname)
        //{
        //    DataTable DTCallDashBoard = new DataTable();
        //    List<Calls> lstCallDashBoard = new List<Calls>();
        //    DTCallDashBoard = objDAM.GetCallDashBoard();

        //    try
        //    {
        //        if (DTCallDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in DTCallDashBoard.Rows)
        //            {
        //                lstCallDashBoard.Add(
        //                    new Calls
        //                    {
        //                        PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
        //                        ContactName = Convert.ToString(dr["Contact_Name"]),
        //                        PO_Number = Convert.ToInt32(dr["Po_Number"]),
        //                        Job = Convert.ToString(dr["Job"]),
        //                        Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
        //                        Sub_Job = Convert.ToString(dr["Sub_job"]),
        //                        Job_Location = Convert.ToString(dr["job_Location"]),
        //                        Inspector = Convert.ToString(dr["Inspector"]),
        //                         Product_item =(dr["Product_item"])
        //                    }
        //                    );
        //            }
        //        }
        ////    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    var getList = from n in lstCallDashBoard
        //                  where n.Product_item.StartsWith(Productname)
        //                  select new { n.Product_item };
        //    return Json(getList);
        //}
        public JsonResult GetAutoCompleteProduct(string Productname)
        {
            DataTable DTCallDashBoard = new DataTable();
            List<Product> lstCallDashBoard = new List<Product>();
            DTCallDashBoard = objDAM.GetCallDashBoard();

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new Product
                            {
                                Product_ID = Convert.ToInt32(dr["Product_ID"]),

                                Product_Name = Convert.ToString(dr["Name"])
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            var getList = from n in lstCallDashBoard
                          where n.Product_Name.StartsWith(Productname)
                          select new { n.Product_Name };
            return Json(getList);
        }
        public JsonResult GetAutoCompleteDataCompanyName(string companyName)
        {
            DataTable DTCallDashBoard = new DataTable();
            List<Calls> lstCallDashBoard = new List<Calls>();
            DTCallDashBoard = objDAM.GetCallDashBoard();

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new Calls
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                ContactName = Convert.ToString(dr["Contact_Name"]),
                                Company_Name=Convert.ToString(dr["Company_Name"]),
                                PO_Number = Convert.ToString(dr["Po_Number"]),
                                Job = Convert.ToString(dr["Job"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                Sub_Job = Convert.ToString(dr["Sub_job"]),
                                Job_Location = Convert.ToString(dr["job_Location"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                              
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            var getList = from n in lstCallDashBoard
                          where n.Company_Name.StartsWith(companyName)
                          select new { n.Company_Name };
            return Json(getList);
        }


        public JsonResult GetRecord(string prefix)

        {
            DataTable DTCallDashBoard = new DataTable();
            List<Calls> lstCallDashBoard = new List<Calls>();
            DTCallDashBoard = objDAM.GetCompanyName(prefix);

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new Calls
                            {
                                
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                //Vendor_Name = Convert.ToString(dr["Vendor_Name"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return Json(lstCallDashBoard, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetVendor(string prefix)

        {
            DataTable DTCallDashBoard = new DataTable();
            List<Calls> lstCallDashBoard = new List<Calls>();
            DTCallDashBoard = objDAM.GetCompanyName(prefix);

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new Calls
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                ContactName = Convert.ToString(dr["Contact_Name"]),
                                PO_Number = Convert.ToString(dr["Po_Number"]),
                                Job = Convert.ToString(dr["Job"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                Sub_Job = Convert.ToString(dr["Sub_job"]),
                                Job_Location = Convert.ToString(dr["job_Location"]),
                                Inspector = Convert.ToString(dr["Inspector"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return Json(lstCallDashBoard, JsonRequestBehavior.AllowGet);

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
                            filePath = Path.Combine(Server.MapPath("~/VirtualCallDocument/"), fileDetail.Id + fileDetail.Extension);
                            //filePath = Path.Combine(Server.MapPath("~/Files/Documents/"), filePath);
                            var K = "~/VirtualCallDocument/" + fileName;
                            IPath = K;
                            files.SaveAs(filePath);

                            //filePath = Path.Combine(Server.MapPath("~/VirtualCallDocument/"), filePath);
                            //var K = "~/VirtualCallDocument/" + fileName;
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
                Session["listVCUploadedFile"] = fileDetails;

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                ViewBag.Error = "Please Select XLSX or PDF File";
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
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
                DTGetDeleteFile = objDAM.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = objDAM.DeleteUploadedFile(id);
                    var path = Path.Combine(Server.MapPath("~/VirtualCallDocument/"), id + fileDetails.Extension);
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
            return File(Path.Combine(Server.MapPath("~/VirtualCallDocument/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }
        #endregion 


        #region export to excel

        [HttpGet]
        public ActionResult ExportIndex(Calls U)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<Calls> grid = CreateExportableGrid(U);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<Calls> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                //added by nikita on 06-09-2023
                var filename = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                return File(package.GetAsByteArray(), "application/unknown", "CallsDashBoard-" + filename + ".xlsx");
            }
        }
        private IGrid<Calls> CreateExportableGrid(Calls U)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<Calls> grid = new Grid<Calls>(GetData(U));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };





            //grid.Columns.Add(model => model.PK_Call_ID).Titled("Add-On Call No");
            //grid.Columns.Add(model => model.Call_No).Titled("Call No");
            //grid.Columns.Add(model => model.ContactName).Titled("Contact Name");
            //grid.Columns.Add(model => model.Job).Titled("Job ");
            //grid.Columns.Add(model => model.Vendor_Name).Titled("Vendors Name");
            //grid.Columns.Add(model => model.UserRole).Titled("Sub Vendors Name");
            //grid.Columns.Add(model => model.Sub_Job).Titled("Sub-Job");
            //grid.Columns.Add(model => model.Job_Location).Titled("Job Location");
            //grid.Columns.Add(model => model.Inspector).Titled("Inspector");
            //grid.Columns.Add(model => model.PO_Number).Titled("PO Number");
            //grid.Columns.Add(model => model.AssignStatus).Titled("Sub PO Number");
            //grid.Columns.Add(model => model.ProductList).Titled("Product Item");
            //grid.Columns.Add(model => model.PO_Date).Titled("PO Date");
            //grid.Columns.Add(model => model.Last_Updated).Titled("Last Updated");
            //grid.Columns.Add(model => model.Assighned_To).Titled("Assigned To");
            //grid.Columns.Add(model => model.VirtualCallStatus).Titled("Add-On Call Status");

            //added by nikita on 06-09-2023
            grid.Columns.Add(model => model.PK_Call_ID).Titled("Add-On Call No");
            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.Company_Name).Titled("Customer Name");
            grid.Columns.Add(model => model.ContactName).Titled("Vendor Contact Person Name");
            grid.Columns.Add(model => model.Job).Titled("Job No");
            grid.Columns.Add(model => model.Vendor_Name).Titled("Vendors Name");
            grid.Columns.Add(model => model.UserRole).Titled("Sub Vendors Name");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub-Job");
            grid.Columns.Add(model => model.Job_Location).Titled("Inspection Location");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector Name");
            grid.Columns.Add(model => model.PO_Number).Titled("Customer PO No on Vendor");
            grid.Columns.Add(model => model.AssignStatus).Titled("Vendor PO No on Sub Vendor");
            grid.Columns.Add(model => model.ProductList).Titled("Stage Description");
            //grid.Columns.Add(model => model.PO_Date).Titled("PO Date");
            grid.Columns.Add(model => model.Last_Updated).Titled("Last Updated");
            grid.Columns.Add(model => model.Assighned_To).Titled("Assigned To Inspector Name");
            grid.Columns.Add(model => model.VirtualCallStatus).Titled("Add-On Call Status");

            grid.Pager = new GridPager<Calls>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = UserCalls.lstCallDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<Calls> GetData(Calls U)
        {

            DataTable DTCallDashBoard = new DataTable();
            List<Calls> lstCallDashBoard = new List<Calls>();

            if(Session["GetExcelData"] == "Yes")
            {
                DTCallDashBoard = objDAM.GetCallDashBoard();
            }
            else
            {

                U.FromDate = Session["FromDate"].ToString();
                U.ToDate = Session["ToDate"].ToString();
                DTCallDashBoard = objDAM.GetCallDashBoardByDate(U);
            }
            

           

            

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new Calls
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Quantity = Convert.ToInt32(dr["Quantity"]),
                                Description = Convert.ToString(dr["Description"]),
                                ContactName = Convert.ToString(dr["Contact_Name"]),
                                PO_Number = Convert.ToString(dr["VendorPoNo"]),
                                AssignStatus = Convert.ToString(dr["SubVendorPoNo"]),
                                Job = Convert.ToString(dr["Job"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                UserRole = Convert.ToString(dr["SubVendorName"]),
                                Sub_Job = Convert.ToString(dr["Sub_job"]),
                                Job_Location = Convert.ToString(dr["job_Location"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Status = Convert.ToString(dr["status"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                PO_Date = Convert.ToString(dr["Po_Date"]),
                                Last_Updated = Convert.ToString(dr["Last_Updated"]),
                                Assighned_To = Convert.ToString(dr["Assighned_To"]),
                                ProductList = Convert.ToString(dr["Product_item"]),
                                VirtualCallStatus = Convert.ToString(dr["VirtualCallStatus"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"])
                            }
                            );
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallList"] = lstCallDashBoard;

            UserCalls.lstCallDashBoard1 = lstCallDashBoard;

            
            return UserCalls.lstCallDashBoard1;
        }

        #endregion

    }
}