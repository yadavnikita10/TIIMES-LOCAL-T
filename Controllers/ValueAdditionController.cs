using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class ValueAdditionController : Controller
    {
        ValueAddition objModel = new ValueAddition();
        DALValueAddition objDAL = new DALValueAddition();
        DALCustomerAppreciation objDAL1 = new DALCustomerAppreciation();

        DALTrainingCreation objDTC = new DALTrainingCreation();
        DALCalls objDalCalls = new DALCalls();
        CommonControl objCommonControl = new CommonControl();
        string[] splitedProduct_Name;
        // GET: ValueAddition
        public ActionResult ValueAddition(int? Id)
        {
            DataSet dss = new DataSet();
            DataSet dssGetBranch = new DataSet();

            var UserData = objDalCalls.GetInspectorListForLeaveManagement();
            ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");


            #region Generate Random no
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
            objModel.UIIN = ConfirmSecondCode;

            #endregion

            #region Bind branch
            DataSet dsBindBranch = new DataSet();
            List<BranchName> lstBranch = new List<BranchName>();
            dsBindBranch = objDTC.BindBranch();

            if (dsBindBranch.Tables[0].Rows.Count > 0)
            {
                lstBranch = (from n in dsBindBranch.Tables[0].AsEnumerable()
                             select new BranchName()
                             {
                                 Name = n.Field<string>(dsBindBranch.Tables[0].Columns["Branch_Name"].ToString()),
                                 Code = n.Field<Int32>(dsBindBranch.Tables[0].Columns["BR_Id"].ToString())

                             }).ToList();
            }

            IEnumerable<SelectListItem> BranchItems;
            BranchItems = new SelectList(lstBranch, "Code", "Name");
            ViewBag.ProjectTypeItems = BranchItems;
            ViewData["BranchName"] = BranchItems;
            #endregion

            #region Bind User List
            //var UserData = objDalCalls.GetInspectorListForLeaveManagement();
            //ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");

            DataSet dsAuditorName = new DataSet();
            List<Audit> lstAuditorNamee = new List<Audit>();
            dsAuditorName = objDAL1.GetInspectorName();


            if (dsAuditorName.Tables[0].Rows.Count > 0)
            {
                lstAuditorNamee = (from n in dsAuditorName.Tables[0].AsEnumerable()
                                   select new Audit()
                                   {
                                       DAuditorName = n.Field<string>(dsAuditorName.Tables[0].Columns["Name"].ToString()),
                                       DAuditorCode = n.Field<string>(dsAuditorName.Tables[0].Columns["Code"].ToString())

                                   }).ToList();
            }

            IEnumerable<SelectListItem> ProductcheckItems;
            ProductcheckItems = new SelectList(lstAuditorNamee, "DAuditorCode", "DAuditorName");
            ViewBag.ProjectTypeItems = ProductcheckItems;
            ViewData["ProjectTypeItems"] = lstAuditorNamee;

            //ViewData["Drpproduct"] = objDAM.GetDrpList();
            #endregion


            if (Id > 0)
            {
                ViewBag.check = "productcheck";
                #region Bind File
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDAL.GetFile(Convert.ToInt32(Id));
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
                }
                    objModel.FileDetails = lstEditFileDetails;

                    #endregion
                    dss = objDAL.GetDataById(Convert.ToInt32(Id));
                if (dss.Tables[0].Rows.Count > 0)
                {

                    objModel.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                    objModel.Branch = Convert.ToString(dss.Tables[0].Rows[0]["Branch"]);
                    objModel.Date = Convert.ToString(dss.Tables[0].Rows[0]["Date"]);
                    objModel.JobNumberWithSubJob = Convert.ToString(dss.Tables[0].Rows[0]["JobNumberWithSubJob"]);
                    objModel.ProjectName = Convert.ToString(dss.Tables[0].Rows[0]["ProjectName"]);
                    objModel.CustomerName = Convert.ToString(dss.Tables[0].Rows[0]["CustomerName"]);
                    objModel.VendorName = Convert.ToString(dss.Tables[0].Rows[0]["VendorName"]);
                    objModel.SubVendorName = Convert.ToString(dss.Tables[0].Rows[0]["SubVendorName"]);
                    objModel.EmployeeName = Convert.ToString(dss.Tables[0].Rows[0]["EmployeeName"]);
                    objModel.DescriptionOfValueAddition = Convert.ToString(dss.Tables[0].Rows[0]["DescriptionOfValueAddition"]);
                    objModel.Impact = Convert.ToString(dss.Tables[0].Rows[0]["Impact"]);
                    objModel.CreatedDate = Convert.ToString(dss.Tables[0].Rows[0]["CreatedDate"]);
                    objModel.CreatedBy = Convert.ToString(dss.Tables[0].Rows[0]["CreatedBy"]);
                    objModel.ModifiedDate = Convert.ToString(dss.Tables[0].Rows[0]["ModifiedDate"]);
                    objModel.ModifiedBy = Convert.ToString(dss.Tables[0].Rows[0]["ModifiedBy"]);
                    objModel.UIIN = Convert.ToString(dss.Tables[0].Rows[0]["UIIN"]);
                    objModel.Remarks = Convert.ToString(dss.Tables[0].Rows[0]["Remarks"]);

                    List<string> Selected = new List<string>();
                    var Existingins = Convert.ToString(dss.Tables[0].Rows[0]["EmployeeName"]);
                    splitedProduct_Name = Existingins.Split(',');
                    foreach (var single in splitedProduct_Name)
                    {
                        Selected.Add(single);
                    }
                    ViewBag.EditproductName = Selected;

                }
                return View(objModel);
            }
            else
            {
                dssGetBranch = objDAL.GetBranch();
                if (dssGetBranch.Tables[0].Rows.Count > 0)
                {
                    objModel.Branch = dssGetBranch.Tables[0].Rows[0]["Fk_branchid"].ToString();
                }

                return View(objModel);
            }


            
        }


        [HttpPost]
        public ActionResult ValueAddition(ValueAddition S, FormCollection fc)
        {
            string ProList = string.Join(",", fc["ProductListName"]);
            S.EmployeeName = ProList;

            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listJobMasterUploadedFile"] as List<FileDetails>;
            

            string Result = string.Empty;
            int Result1 = 0;
            try
            {

                if (S.Id > 0)
                {
                    ViewBag.check = "productcheck";
                    //Update
                    Result = objDAL.Insert(S);
                    //if (Convert.ToInt16(Result) > 0)
                    if (Convert.ToInt16(S.Id) > 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = objDAL.InsertFileAttachment(lstFileDtls, Convert.ToInt32(S.Id), S);
                            Session["listJobMasterUploadedFile"] = null;
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(S.Id));
                        }
                        return RedirectToAction("ValueAddition", new { Id = S.Id });
                        ModelState.Clear();
                        TempData["message"] = "Record Added Successfully";
                    }


                }
                else
                {

                    Result = objDAL.Insert(S);
                    if (Convert.ToInt16(Result) > 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = objDAL.InsertFileAttachment(lstFileDtls, Convert.ToInt32(S.Id), S);
                            Session["listJobMasterUploadedFile"] = null;
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(Result));
                        }

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
            return RedirectToAction("ValueAddition", new { Id = Result });
            //return RedirectToAction("ListIAFScopeMaster", "IAFScopeMaster");
        }


        [HttpGet]
        public JsonResult GetDataByControllNo(string Control_No)
        {
            var address = objDAL.GetDataByControllNo(Control_No);
            return Json(address, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();

            List<FileDetails> fileDetails = new List<FileDetails>();
            List<FileDetails> fileJobDetails = new List<FileDetails>();
            List<FileDetails> fileSubjobDetails = new List<FileDetails>();

            if (Session["listJobMasterUploadedFile"] != null)
            {
                fileJobDetails = Session["listJobMasterUploadedFile"] as List<FileDetails>;
            }

            if (Session["listSubUploadedFile"] != null)
            {
                fileSubjobDetails = Session["listSubUploadedFile"] as List<FileDetails>;
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
                          

                            string fileName = files.FileName;//ConfirmCode + files.FileName;
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
                                fileJobDetails.Add(fileDetail);
                            }
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                            {
                                fileSubjobDetails.Add(fileDetail);
                            }

                            //-----------------------------------------------------


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
                    Session["listJobMasterUploadedFile"] = fileJobDetails;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                {
                    Session["listSubUploadedFile"] = fileSubjobDetails;
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();

            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ListValueAddition()
        {
            List<ValueAddition> lmd = new List<ValueAddition>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objDAL.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new ValueAddition
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Branch = Convert.ToString(dr["Branch"]),
                    Date = Convert.ToString(dr["Date"]),
                    JobNumberWithSubJob = Convert.ToString(dr["JobNumberWithSubJob"]),
                    CustomerName = Convert.ToString(dr["CustomerName"]),
                    EmployeeName = Convert.ToString(dr["EmployeeName"]),
                    DescriptionOfValueAddition = Convert.ToString(dr["DescriptionOfValueAddition"]),
                    Impact = Convert.ToString(dr["Impact"]),


                });
            }
            objModel.lst1 = lmd;
            //return View(lmd.ToList());
            return View(objModel);

        }

        public FileResult Download(string d)
        {

            string FileName = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDAL.GetFileContent(Convert.ToInt32(d));

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

        [HttpPost]
        public JsonResult DeleteUploadedFile(string id)
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
                //  Guid guid = new Guid(id);
                //DTGetDeleteFile = objDalVisitReport.GetConFileExt1(id);
                //if (DTGetDeleteFile.Rows.Count > 0)
                //{
                //    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                //    fileDetails.FileName = Convert.ToString(DTGetDeleteFile.Rows[0]["FileName"]);
                //}
                if (id != null && id != "")
                {
                    Results = objDAL.DeleteFile(id);
                    //var path = Path.Combine(Server.MapPath("~/Content/"), fileDetails.FileName);
                    //if (System.IO.File.Exists(path))
                    //{
                    //    System.IO.File.Delete(path);
                    //}
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
        public ActionResult SearchByDateListValueAddition(ValueAddition objModel)
        {
            List<ValueAddition> lmd = new List<ValueAddition>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objDAL.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new ValueAddition
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Branch = Convert.ToString(dr["Branch"]),
                    Date = Convert.ToString(dr["Date"]),
                    JobNumberWithSubJob = Convert.ToString(dr["JobNumberWithSubJob"]),
                    CustomerName = Convert.ToString(dr["CustomerName"]),
                    EmployeeName = Convert.ToString(dr["EmployeeName"]),
                    DescriptionOfValueAddition = Convert.ToString(dr["DescriptionOfValueAddition"]),
                    Impact = Convert.ToString(dr["Impact"]),


                });
            }
            objModel.lst1 = lmd;
            //return View(lmd.ToList());
            return View(objModel);

        }

        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objDAL.Delete(Convert.ToInt32(Id));
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
            return RedirectToAction("ListValueAddition");


        }



        #region Export to Exel 
        public ActionResult ExportIndex1()
        {

            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<ValueAddition> grid = CreateExportableGrid1();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<ValueAddition> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<ValueAddition> CreateExportableGrid1()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<ValueAddition> grid = new Grid<ValueAddition>(GetData1());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.UIIN).Titled("UIIN");
            grid.Columns.Add(model => model.Date).Titled("Date");
            grid.Columns.Add(model => model.JobNumberWithSubJob).Titled("JobNumberWithSubJob");
            grid.Columns.Add(model => model.ProjectName).Titled("ProjectName");
            grid.Columns.Add(model => model.CustomerName).Titled("CustomerName");
            grid.Columns.Add(model => model.VendorName).Titled("VendorName");
            grid.Columns.Add(model => model.SubVendorName).Titled("SubVendorName");
            grid.Columns.Add(model => model.EmployeeName).Titled("EmployeeName");
            grid.Columns.Add(model => model.DescriptionOfValueAddition).Titled("DescriptionOfValueAddition");
            grid.Columns.Add(model => model.Impact).Titled("Impact");
            
          
            grid.Columns.Add(model => model.Remarks).Titled("Remarks ");






            grid.Pager = new GridPager<ValueAddition>(grid);
            grid.Processors.Add(grid.Pager);

            grid.Pager.RowsPerPage = objModel.lstCompanyMaster1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<ValueAddition> GetData1()
        {

            DataTable DTCompanyDashBoard = new DataTable();
            List<ValueAddition> lstCompanyDashBoard = new List<ValueAddition>();
            DTCompanyDashBoard = objDAL.GetValueADashBoard();
            try
            {
                if (DTCompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ValueAddition
                            {

                                Branch = Convert.ToString(dr["Branch "]),
                                UIIN = Convert.ToString(dr["UIIN"]),
                                Date = Convert.ToString(dr["Date"]),
                                JobNumberWithSubJob = Convert.ToString(dr["JobNumberWithSubJob "]),
                                ProjectName = Convert.ToString(dr["ProjectName"]),
                                CustomerName = Convert.ToString(dr["CustomerName"]),
                                VendorName = Convert.ToString(dr["VendorName"]),
                                SubVendorName = Convert.ToString(dr["SubVendorName"]),
                                EmployeeName = Convert.ToString(dr["EmployeeName"]),
                                DescriptionOfValueAddition = Convert.ToString(dr["DescriptionOfValueAddition"]),
                                Impact = Convert.ToString(dr["Impact"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                FromD = Convert.ToString(dr["FromD"]),
                                ToD = Convert.ToString(dr["ToD"]),
                                Remarks = Convert.ToString(dr["Remarks"]),

                            }
                            );
                    }


                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CompanyList"] = lstCompanyDashBoard;
            objModel.lstCompanyMaster1 = lstCompanyDashBoard;
            return objModel.lstCompanyMaster1;

        }
        #endregion


    }
}