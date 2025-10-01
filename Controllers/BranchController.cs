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
using OfficeOpenXml;
using NonFactors.Mvc.Grid;

namespace TuvVision.Controllers
{
    public class BranchController : Controller
    {
        DALUsers objDalCreateUser = new DALUsers();
        DALBranchMaster objDalCompany = new DALBranchMaster();
        BranchMasters ObjModelCompany = new BranchMasters();
        List<BranchMasters> lstCompanyDashBoard = new List<BranchMasters>();
        // GET: Branch
        public ActionResult Index()
        {
            return View();
        }



        #region  Branch Master Code 

        public ActionResult Branch()
        {
            DataTable CompanyDashBoard = new DataTable();

            CompanyDashBoard = objDalCompany.GetBranchList();
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new BranchMasters
                            {
                                Br_Id = Convert.ToInt32(dr["Br_Id"]),
                                Branch_Name = Convert.ToString(dr["Branch_Name"]),
                                Branch_Code = Convert.ToString(dr["Branch_Code"]),
                                Email_Id = Convert.ToString(dr["Email_Id"]),
                                CityName = Convert.ToString(dr["CityName"]),
                                State = Convert.ToString(dr["State"]),

                                Address1 = Convert.ToString(dr["Address1"]),
                                Address2 = Convert.ToString(dr["Address2"]),
                                Address3 = Convert.ToString(dr["Address3"]),
                                Manager = Convert.ToString(dr["Manager"]),
                                Postal_Code = Convert.ToString(dr["Postal_Code"]),
                                Country = Convert.ToString(dr["Country"]),
                                Service_Code = Convert.ToString(dr["Service_Code"]),
                                Sequence_Number = Convert.ToString(dr["Sequence_Number"]),
                                BranchAdmin = Convert.ToString(dr["BranchAdmin"]),
                                Status = Convert.ToString(dr["Status"]),
                                Coordinator_Email_Id = Convert.ToString(dr["Coordinator_Email_Id"]),
                                Attachment = Convert.ToString(dr["Attachment"])
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;
            ObjModelCompany.lstCompanyDashBoard1 = lstCompanyDashBoard;
            return View(ObjModelCompany);
        }


        [HttpGet]
        public ActionResult CreateBranch(int? Br_Id)
        {
            DataTable DTUserDashBoard = new DataTable();
            List<Users> lstUserDashBoard = new List<Users>();
            DTUserDashBoard = objDalCreateUser.GetUserDashBoard();
            if (DTUserDashBoard.Rows.Count > 0)
            {
                foreach (DataRow dr in DTUserDashBoard.Rows)
                {
                    lstUserDashBoard.Add(
                        new Users
                        {
                            PK_UserID = Convert.ToString(dr["PK_UserID"]),
                            FullName = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),
                        }
                        );
                }
            }

            ViewBag.Employees = new SelectList(lstUserDashBoard, "PK_UserID", "FullName");
            if (Br_Id != 0 && Br_Id != null)
            {
                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalCompany.EditBranch(Br_Id);
                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ObjModelCompany.Br_Id = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["Br_Id"]);
                    ObjModelCompany.Branch_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Branch_Name"]);
                    ObjModelCompany.Branch_Code = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Branch_Code"]);
                    ObjModelCompany.Manager = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Manager"]);
                    ObjModelCompany.Service_Code = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Service_Code"]);
                    ObjModelCompany.Sequence_Number = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Sequence_Number"]);
                    ObjModelCompany.Address1 = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Address1"]);
                    ObjModelCompany.Address2 = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Address2"]);
                    ObjModelCompany.Address3 = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Address3"]);
                    ObjModelCompany.Country = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Country"]);
                    ObjModelCompany.State = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["State"]);
                    ObjModelCompany.Postal_Code = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Postal_Code"]);
                    ObjModelCompany.Email_Id = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Email_Id"]);
                    ObjModelCompany.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Status"]);
                    ObjModelCompany.CityName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["CityName"]);

                    ObjModelCompany.Attachment = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Attachment"]);
                    ObjModelCompany.Coordinator_Email_Id = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Coordinator_Email_Id"]);

                    ObjModelCompany.BranchAdmin = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["BranchAdmin"]);
                }
              
                return View(ObjModelCompany);
            }
            else
            {
              
                
            }
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBranch(BranchMasters CM, List<HttpPostedFileBase> img_Banner)
        {
            string Result = string.Empty;

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
                            var filename = CommonControl.FileUpload("~/Content/Uploads/Images/", single);
                            lstAttachment.Add(filename);
                        }
                    }
                    CM.Attachment = string.Join(",", lstAttachment);
                    if(string.IsNullOrEmpty(CM.Attachment))
                    {
                        CM.Attachment = "NoImage.gif";
                    }
                }
                else
                {
                    CM.Attachment = "NoImage.gif";
                }
                if (CM.Br_Id == 0)
                {
                    CM.Status = "1";
                    Result = objDalCompany.InsertUpdateBranch(CM, IPath);
                    if (Result != "" && Result != null)
                    {
                        TempData["InsertCompany"] = Result;
                      
                        return RedirectToAction("Branch");
                    }
                }
                else
                {
                    Result = objDalCompany.InsertUpdateBranch(CM, IPath);
                    if (Result != null && Result != "")
                    {

                        TempData["UpdateCompany"] = Result;
                        return RedirectToAction("Branch");
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return View();
        }




        public ActionResult DeleteBranch(int? Br_Id)
        {
            int Result = 0;
            try
            {
                Result = objDalCompany.DeleteBranch(Br_Id);
                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    return RedirectToAction("Branch", "Branch");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        public JsonResult doesBranchNameExist(string Branch_Name,int? Br_Id)
        {
            try
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
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
                            filePath = Path.Combine(Server.MapPath("~/Content/Uploads/Images/"), filePath);
                            var K = "~/Content/Uploads/Images/" + fileName;
                            IPath = K;//K.TrimStart('~');
                            files.SaveAs(Server.MapPath(IPath));
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


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }

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
                IGrid<BranchMasters> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<BranchMasters> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<BranchMasters> CreateExportableGrid()
        {
            
            //IGrid<BranchMaster> grid = new Grid<BranchMaster>(GetData1());
            IGrid<BranchMasters> grid = new Grid<BranchMasters>(GetData1());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            


            grid.Columns.Add(model => model.Branch_Name).Titled("Branch Name");
            grid.Columns.Add(model => model.BranchAdmin).Titled("Branch Admin");
            grid.Columns.Add(model => model.Branch_Code).Titled("Branch Code");
            grid.Columns.Add(model => model.Email_Id).Titled("Email Id");
            grid.Columns.Add(model => model.Country).Titled("Country");
            grid.Columns.Add(model => model.CityName).Titled("City Name");
            grid.Columns.Add(model => model.Address1).Titled("Address 1");
            grid.Columns.Add(model => model.Address2).Titled("Address 2");
            grid.Columns.Add(model => model.Status).Titled("Status");

            grid.Pager = new GridPager<BranchMasters>(grid);
            grid.Processors.Add(grid.Pager);
            //grid.Pager.RowsPerPage = 6;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

       // List<BranchMasters> lstCompanyDashBoard = new List<BranchMasters>();
        public List<BranchMasters> GetData1()
        {
            
            DataTable CompanyDashBoard = new DataTable();
          
            CompanyDashBoard = objDalCompany.GetBranchList();
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new BranchMasters
                            {
                                Br_Id = Convert.ToInt32(dr["Br_Id"]),
                                Branch_Name = Convert.ToString(dr["Branch_Name"]),
                                Branch_Code = Convert.ToString(dr["Branch_Code"]),
                                Email_Id = Convert.ToString(dr["Email_Id"]),
                                CityName = Convert.ToString(dr["CityName"]),
                                State = Convert.ToString(dr["State"]),

                                Address1 = Convert.ToString(dr["Address1"]),
                                Address2 = Convert.ToString(dr["Address2"]),
                                Address3 = Convert.ToString(dr["Address3"]),
                                Manager = Convert.ToString(dr["Manager"]),
                                Postal_Code = Convert.ToString(dr["Postal_Code"]),
                                Country = Convert.ToString(dr["Country"]),
                                Service_Code = Convert.ToString(dr["Service_Code"]),
                                Sequence_Number = Convert.ToString(dr["Sequence_Number"]),
                                BranchAdmin = Convert.ToString(dr["BranchAdmin"]),
                                Status = Convert.ToString(dr["Status"]),
                                Coordinator_Email_Id = Convert.ToString(dr["Coordinator_Email_Id"]),
                                Attachment = Convert.ToString(dr["Attachment"])
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ObjModelCompany.lstCompanyDashBoard1 = lstCompanyDashBoard;

            //  return ObjModelCompany.lstCompanyDashBoard1;
            return ObjModelCompany.lstCompanyDashBoard1;
        }
        #endregion

     


    }
}