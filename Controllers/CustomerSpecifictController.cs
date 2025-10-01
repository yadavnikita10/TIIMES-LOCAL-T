using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class CustomerSpecifictController : Controller
    {
        // GET: CustomerSpecifict

        ModelCustomerAppreciation objModel = new ModelCustomerAppreciation();
        DalCustSpecific custSpecific = new DalCustSpecific();
        ValueAddition valueaddition = new ValueAddition();
        DataSet dssGetBranch = new DataSet();
        CommonControl objCommonControl = new CommonControl();

        DataSet dss = new DataSet();
        string[] splitedProduct_Name;



        public ActionResult Index()
        {
            return View();
        }


        //code for customer Aprreciation
        [HttpGet]
        public ActionResult ListCustomerAppreciation()
        {
            List<ModelCustomerAppreciation> lmd = new List<ModelCustomerAppreciation>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = custSpecific.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new ModelCustomerAppreciation
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    UIIN = Convert.ToString(dr["UIIN"]),
                    Branch = Convert.ToString(dr["Branch"]),
                    Date = Convert.ToString(dr["Date"]),
                    Mode = Convert.ToString(dr["Mode"]),
                    JobNumberWithSubJob = Convert.ToString(dr["JobNumberWithSubJob"]),
                    CustomerName = Convert.ToString(dr["CustomerName"]),
                    EmployeeName = Convert.ToString(dr["EmployeeName"]),
                    PraisingQuote = Convert.ToString(dr["PraisingQuote"]),
                    PraisedBy = Convert.ToString(dr["PraisedBy"]),



                });
            }
            objModel.lst1 = lmd;
            //return View(lmd.ToList());
            return View(objModel);

        }
        // GET: CustomerAppreciationForm
        public ActionResult CustomerAppreciationForm(int? Id)
        {

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
            dsBindBranch = custSpecific.BindBranch();

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
            dsAuditorName = custSpecific.GetInspectorName();


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
                DTGetUploadedFile = custSpecific.GetFile(Convert.ToInt32(Id));
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
                objModel.Attachment = lstEditFileDetails;

                #endregion

                #region Bind Image File
                DataTable DTGetEUploadedFile = new DataTable();
                List<FileDetails> lstEventImagesFileDetails = new List<FileDetails>();
                DTGetEUploadedFile = custSpecific.GetEventFile(Convert.ToInt32(Id));
                if (DTGetEUploadedFile.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTGetEUploadedFile.Rows)
                    {
                        lstEventImagesFileDetails.Add(
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
                objModel.Attachment1 = lstEventImagesFileDetails;

                #endregion


                dss = custSpecific.GetDataById(Convert.ToInt32(Id));
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
                    //objModel.EmployeeName = Convert.ToString(dss.Tables[0].Rows[0]["EmployeeName"]);
                    objModel.CreatedDate = Convert.ToString(dss.Tables[0].Rows[0]["CreatedDate"]);
                    objModel.CreatedBy = Convert.ToString(dss.Tables[0].Rows[0]["CreatedBy"]);
                    objModel.ModifiedDate = Convert.ToString(dss.Tables[0].Rows[0]["ModifiedDate"]);
                    objModel.ModifiedBy = Convert.ToString(dss.Tables[0].Rows[0]["ModifiedBy"]);
                    objModel.UIIN = Convert.ToString(dss.Tables[0].Rows[0]["UIIN"]);
                    objModel.PraisingQuote = Convert.ToString(dss.Tables[0].Rows[0]["PraisingQuote"]);
                    objModel.Remarks = Convert.ToString(dss.Tables[0].Rows[0]["Remarks"]);
                    objModel.PraisedBy = Convert.ToString(dss.Tables[0].Rows[0]["PraisedBy"]);
                    objModel.ShareasNews = Convert.ToBoolean(dss.Tables[0].Rows[0]["ShareasNews"]);
                    objModel.CoverImage = Convert.ToString(dss.Tables[0].Rows[0]["CoverImage"]);

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
                dssGetBranch = custSpecific.GetBranch();
                if (dssGetBranch.Tables[0].Rows.Count > 0)
                {
                    objModel.Branch = dssGetBranch.Tables[0].Rows[0]["Fk_branchid"].ToString();
                }

                return View(objModel);
            }


        }


        [HttpPost]
        public ActionResult CustomerAppreciationForm(ModelCustomerAppreciation S, FormCollection fc)
        {
            string ProList = string.Join(",", fc["ProductListName"]);
            S.EmployeeName = ProList;

            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listJobMasterUploadedFile"] as List<FileDetails>;

            List<FileDetails> lstEventImages = new List<FileDetails>();
            lstEventImages = Session["lstEventImages"] as List<FileDetails>;


            string Result = string.Empty;
            int Result1 = 0;
            try
            {

                if (S.Id > 0)
                {
                    //Update
                    Result = custSpecific.Insert(S);
                    //if (Convert.ToInt16(Result) > 0)
                    if (Convert.ToInt16(S.Id) > 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = custSpecific.InsertFileAttachment(lstFileDtls, Convert.ToInt32(S.Id), S, "1");
                            Session["listJobMasterUploadedFile"] = null;
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(S.Id));
                        }

                        if (lstEventImages != null && lstEventImages.Count > 0)
                        {
                            Result = custSpecific.InsertFileAttachment(lstEventImages, Convert.ToInt32(S.Id), S, "2");
                            Session["lstEventImages"] = null;
                            objCommonControl.SaveFileToPhysicalLocation(lstEventImages, Convert.ToInt32(S.Id));
                        }

                        return RedirectToAction("CustomerAppreciationForm", new { Id = S.Id });

                        ModelState.Clear();

                        TempData["message"] = "Record Added Successfully";
                    }


                }
                else
                {

                    Result = custSpecific.Insert(S);
                    if (Convert.ToInt16(Result) > 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = custSpecific.InsertFileAttachment(lstFileDtls, Convert.ToInt32(Result), S, "1");
                            Session["listJobMasterUploadedFile"] = null;
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(Result));
                        }

                        if (lstEventImages != null && lstEventImages.Count > 0)
                        {
                            Result = custSpecific.InsertFileAttachment(lstEventImages, Convert.ToInt32(Result), S, "2");
                            Session["lstEventImages"] = null;
                            objCommonControl.SaveFileToPhysicalLocation(lstEventImages, Convert.ToInt32(Result));
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
            return RedirectToAction("CustomerAppreciationForm", new { Id = Result });
            //return RedirectToAction("ListIAFScopeMaster", "IAFScopeMaster");
        }



        public ActionResult Delete(int? id)
        {
            string Result = string.Empty;
            try
            {
                Result = custSpecific.Delete(Convert.ToInt32(id));
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
            return RedirectToAction("ListCustomerAppreciation");


        }


        //added on 10102023 for Export excel

        [HttpGet]
        public ActionResult ExportIndex()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<ModelCustomerAppreciation> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<ModelCustomerAppreciation> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "CustomerAppreciation-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<ModelCustomerAppreciation> CreateExportableGrid()
        {
            IGrid<ModelCustomerAppreciation> grid = new Grid<ModelCustomerAppreciation>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(c => c.UIIN).Titled("Id");
            grid.Columns.Add(c => c.Branch).Titled("Branch");
            grid.Columns.Add(c => c.Date).Titled("Date");
            grid.Columns.Add(c => c.Mode).Titled("Mode of receipt");
            grid.Columns.Add(c => c.JobNumberWithSubJob).Titled("Job Number With SubJob");
            grid.Columns.Add(c => c.CustomerName).Titled("Customer Name");
            grid.Columns.Add(c => c.PraisingQuote).Titled("Employee Name");
            grid.Columns.Add(c => c.PraisedBy).Titled("Praised By");





            grid.Pager = new GridPager<ModelCustomerAppreciation>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objModel.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            return grid;
        }

        public List<ModelCustomerAppreciation> GetData()
        {



            List<ModelCustomerAppreciation> lst = new List<ModelCustomerAppreciation>();
            DataSet ds = new DataSet();

            ds = custSpecific.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lst.Add(new ModelCustomerAppreciation
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    UIIN = Convert.ToString(dr["UIIN"]),
                    Branch = Convert.ToString(dr["Branch"]),
                    Date = Convert.ToString(dr["Date"]),
                    Mode = Convert.ToString(dr["Mode"]),
                    JobNumberWithSubJob = Convert.ToString(dr["JobNumberWithSubJob"]),
                    CustomerName = Convert.ToString(dr["CustomerName"]),
                    EmployeeName = Convert.ToString(dr["EmployeeName"]),
                    PraisingQuote = Convert.ToString(dr["PraisingQuote"]),
                    PraisedBy = Convert.ToString(dr["PraisedBy"]),



                });
            }
            objModel.lst1 = lst;
            //return View(lmd.ToList());
            return objModel.lst1;
        }





    }
}