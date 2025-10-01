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
    public class MisAppealController : Controller
    {

        DalCustSpecific custSpecific = new DalCustSpecific();
        AppealMaster AplMas = new AppealMaster();
        // GET: MisAppeal
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult AppealDashBoard()
        {
            Session["GetExcelData"] = "Yes";
            DataTable DTAppeal = new DataTable();
            List<AppealMaster> lstAppeal = new List<AppealMaster>();
            DTAppeal = custSpecific.GetAppealDashBoard();
            try
            {
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new AppealMaster
                            {

                                Appeal_ID = Convert.ToInt32(dr["Appeal_ID"]),
                                Created_by = Convert.ToString(dr["FirstName"]),
                                //Created_Date = Convert.ToString(dr["Created_Date"]),
                                Created_Date = Convert.ToDateTime(dr["Created_Date"]).ToString("yyyy-MM-dd"),
                                Modified_By = Convert.ToString(dr["Modified_By"]),
                                //Modified_Date = Convert.ToString(dr["Modified_Date"]),
                                Modified_Date = Convert.ToString(dr["Modified_Date"]) == "1900-01-01" ? "" : Convert.ToString(dr["Modified_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                //Date_of_Appeal = Convert.ToString(dr["Date_of_Appeal"]),
                                Appeal_Referance_No = Convert.ToString(dr["Appeal_Referance_No"]),
                                Appellant = Convert.ToString(dr["Appeliant"]),
                                Details_Of_Appeal = Convert.ToString(dr["Details_Of_Appeal"]),
                                TUV_Control_No = Convert.ToString(dr["TUV_Control_No"]),
                                Review_And_Analysis = Convert.ToString(dr["Review_And_Analysis"]),
                                Disposal_Action = Convert.ToString(dr["Disposal_Action"]),
                                Disposal_By = Convert.ToString(dr["Disposal_By"]),
                                Date_Of_Disposal = Convert.ToString(dr["Date_Of_Disposal"]),
                                Remarks = Convert.ToString(dr["Remarks"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                Mode_Of_Appeal = Convert.ToString(dr["Mode_Of_Appeal"]),
                                Date_of_Appeal = Convert.ToString(dr["Date_of_Appeal"])
                                //Date_of_Appeal = Convert.ToDateTime(dr["Appeal_Date"]).ToString("dd-MMM-yyyy")



                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["AppealList"] = lstAppeal;

            AplMas.lstAppeal1 = lstAppeal;


            return View(AplMas);
        }

        [HttpPost]
        public ActionResult AppealDashBoard(AppealMaster a)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = a.FromDate;
            Session["ToDate"] = a.ToDate;
            DataTable DTAppeal = new DataTable();
            List<AppealMaster> lstAppeal = new List<AppealMaster>();
            DTAppeal = custSpecific.GetAppealDashBoardWithDate(a);
            try
            {
                if (DTAppeal.Rows.Count > 0)
                {

                    foreach (DataRow dr in DTAppeal.Rows)
                    {


                        lstAppeal.Add(
                            new AppealMaster
                            {

                                Appeal_ID = Convert.ToInt32(dr["Appeal_ID"]),
                                Created_by = Convert.ToString(dr["Created_by"]),
                                Created_Date = Convert.ToString(dr["Created_Date"]),
                                Modified_By = Convert.ToString(dr["Modified_By"]),
                                Modified_Date = Convert.ToString(dr["Modified_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                //Date_of_Appeal = Convert.ToString(dr["Date_of_Appeal"]),
                                Appeal_Referance_No = Convert.ToString(dr["Appeal_Referance_No"]),
                                Appellant = Convert.ToString(dr["Appeliant"]),
                                Details_Of_Appeal = Convert.ToString(dr["Details_Of_Appeal"]),
                                TUV_Control_No = Convert.ToString(dr["TUV_Control_No"]),
                                Review_And_Analysis = Convert.ToString(dr["Review_And_Analysis"]),
                                Disposal_Action = Convert.ToString(dr["Disposal_Action"]),
                                Disposal_By = Convert.ToString(dr["Disposal_By"]),
                                Date_Of_Disposal = Convert.ToString(dr["Date_Of_Disposal"]),
                                Remarks = Convert.ToString(dr["Remarks"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                Mode_Of_Appeal = Convert.ToString(dr["Mode_Of_Appeal"]),
                                Date_of_Appeal = Convert.ToString(dr["Date_of_Appeal"])

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["AppealList"] = lstAppeal;
            AplMas.lstAppeal1 = lstAppeal;
            return View(AplMas);
        }


        [HttpGet]
        public ActionResult CreateAppeal(int? appeal_ID)
        {
            List<inscode> lstapl = new List<inscode>();
            DataTable DTAppeal = new DataTable();
            DataSet dt = new DataSet();


           
            #region Bind branch name
            DataSet dsBindBranch = new DataSet();
            List<BranchName> lstBranch = new List<BranchName>();
            dsBindBranch = custSpecific.BindBranchAppeal();

            if (dsBindBranch.Tables[0].Rows.Count > 0)
            {
                lstBranch = (from n in dsBindBranch.Tables[0].AsEnumerable()
                             select new BranchName()
                             {
                                 Name = n.Field<string>(dsBindBranch.Tables[0].Columns["BranchName"].ToString()),
                                 Code = n.Field<Int32>(dsBindBranch.Tables[0].Columns["PK_ID"].ToString())

                             }).ToList();
            }

            IEnumerable<SelectListItem> BranchItems;
            BranchItems = new SelectList(lstBranch, "Name", "Name");
            ViewBag.ProjectTypeItems = BranchItems;
            ViewData["BranchName"] = BranchItems;
            #endregion

            DTAppeal = custSpecific.EditAppeal(appeal_ID);
            if (appeal_ID != 0 && appeal_ID != null)
            {
                List<AppealMaster> app = new List<AppealMaster>();
                AplMas.Appeal_ID = Convert.ToInt32(DTAppeal.Rows[0]["Appeal_ID"]);
                AplMas.Created_by = Convert.ToString(DTAppeal.Rows[0]["Created_by"]);
                AplMas.Created_Date = Convert.ToString(DTAppeal.Rows[0]["Created_Date"]);
                AplMas.Modified_By = Convert.ToString(DTAppeal.Rows[0]["Modified_By"]);
                AplMas.Modified_Date = Convert.ToString(DTAppeal.Rows[0]["Modified_Date"]);
                AplMas.Status = Convert.ToString(DTAppeal.Rows[0]["Status"]);
                AplMas.Date_of_Appeal = Convert.ToString(DTAppeal.Rows[0]["Date_of_Appeal"]);
                AplMas.Appeal_Referance_No = Convert.ToString(DTAppeal.Rows[0]["Appeal_Referance_No"]);
                AplMas.Appellant = Convert.ToString(DTAppeal.Rows[0]["Appeliant"]);
                AplMas.Details_Of_Appeal = Convert.ToString(DTAppeal.Rows[0]["Details_Of_Appeal"]);
                AplMas.TUV_Control_No = Convert.ToString(DTAppeal.Rows[0]["TUV_Control_No"]);
                AplMas.Review_And_Analysis = Convert.ToString(DTAppeal.Rows[0]["Review_And_Analysis"]);
                AplMas.Disposal_Action = Convert.ToString(DTAppeal.Rows[0]["Disposal_Action"]);
                AplMas.Disposal_By = Convert.ToString(DTAppeal.Rows[0]["Disposal_By"]);
                AplMas.Date_Of_Disposal = Convert.ToString(DTAppeal.Rows[0]["Date_Of_Disposal"]);
                AplMas.Remarks = Convert.ToString(DTAppeal.Rows[0]["Remarks"]);
                AplMas.Attachment = Convert.ToString(DTAppeal.Rows[0]["Attachment"]);
                AplMas.Mode_Of_Appeal = Convert.ToString(DTAppeal.Rows[0]["Mode_Of_Appeal"]);

                //var inspectors = DTAppeal.AsEnumerable().Select(r => r.Field<AppealMaster>("Inspector_Name"));
                //foreach (AppealMaster ins in inspectors)
                //{
                //    lstapl.Add(
                //        AplMas.Attachment = ins;
                //    );

                //}
                //if (dt.Tables[0].Rows.Count > 0)
                //{
                //    lstapl = (from n in dt.Tables[0].AsEnumerable()
                //                          select new inscode()
                //                          {
                //                              Name = n.Field<string>(dt.Tables[0].Columns["Attachment"].ToString()),
                //                              Code = n.Field<Int32>(dt.Tables[0].Columns["Appeal_ID"].ToString())

                //                          }).ToList();
                //}
                //IEnumerable<SelectListItem> InspectorItems;
                //InspectorItems = new SelectList(lstapl, "Code", "Name");
                //ViewBag.CityNameItems = InspectorItems;
                //ViewData["CityNameItems"] = InspectorItems;

                AplMas.Branch = Convert.ToString(DTAppeal.Rows[0]["Branch"]);
            }
            return View(AplMas);
        }




        //Export to Excel


        [HttpGet]
        public ActionResult ExportIndex(AppealMaster a)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<AppealMaster> grid = CreateExportableGrid(a);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<AppealMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }
                //added by nikita on 14092023
                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "AppealRegister-" + formattedDateTime + ".xlsx";

                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<AppealMaster> CreateExportableGrid(AppealMaster a)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<AppealMaster> grid = new Grid<AppealMaster>(GetData(a));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.Appeal_ID).Titled("Appeal ID");
            grid.Columns.Add(model => model.Date_of_Appeal).Titled("Appeal Date");
            grid.Columns.Add(model => model.Appeal_Referance_No).Titled("Appeal Reference Number");
            grid.Columns.Add(model => model.Appellant).Titled("Appellant(Client/Vendor)");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.Mode_Of_Appeal).Titled("Mode Of Appeal");
            grid.Columns.Add(model => model.TUV_Control_No).Titled("TUVI Control Number");
            grid.Columns.Add(model => model.Details_Of_Appeal).Titled("Details of Appeal");
            grid.Columns.Add(model => model.Review_And_Analysis).Titled("Review And Analysis");
            grid.Columns.Add(model => model.Disposal_Action).Titled("Disposal Action");
            grid.Columns.Add(model => model.Disposal_By).Titled("Disposal By");
            grid.Columns.Add(model => model.Date_Of_Disposal).Titled("Date of Disposal");
            grid.Columns.Add(model => model.Remarks).Titled("Remarks");
            grid.Columns.Add(model => model.Attachment).Titled("Attachment");
            grid.Columns.Add(model => model.Status).Titled("Status");
            grid.Columns.Add(model => model.Created_by).Titled("Created By");
            grid.Columns.Add(model => model.Created_Date).Titled("Created Date");
            grid.Columns.Add(model => model.Modified_By).Titled("Modified_By");
            grid.Columns.Add(model => model.Modified_Date).Titled("Modified Date");


            grid.Pager = new GridPager<AppealMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = AplMas.lstAppeal1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<AppealMaster> GetData(AppealMaster a)
        {

            DataTable DTAppeal = new DataTable();
            List<AppealMaster> lstAppeal = new List<AppealMaster>();

            if (Session["GetExcelData"] == "Yes")
            {
                DTAppeal = custSpecific.GetAppealDashBoard();
            }
            else
            {
                a.FromDate = Session["FromDate"].ToString();
                a.ToDate = Session["ToDate"].ToString();
                DTAppeal = custSpecific.GetAppealDashBoardWithDate(a);
            }
            try
            {
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new AppealMaster
                            {
                                Count = DTAppeal.Rows.Count,
                                Appeal_ID = Convert.ToInt32(dr["Appeal_ID"]),
                                Created_by = Convert.ToString(dr["Created_by"]),
                                //Created_Date = Convert.ToString(dr["Created_Date"]),
                                // Created_Date = Convert.ToDateTime(dr["Created_Date"]).ToString("dd-MMM-yyyy"),
                                Created_Date = Convert.ToString(dr["Created_Date"]),
                                Modified_By = Convert.ToString(dr["Modified_By"]),
                                //Modified_Date = Convert.ToString(dr["Modified_Date"]),
                                Modified_Date = Convert.ToString(dr["Modified_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                //Date_of_Appeal = Convert.ToString(dr["Date_of_Appeal"]),
                                Appeal_Referance_No = Convert.ToString(dr["Appeal_Referance_No"]),
                                Appellant = Convert.ToString(dr["Appeliant"]),
                                Details_Of_Appeal = Convert.ToString(dr["Details_Of_Appeal"]),
                                TUV_Control_No = Convert.ToString(dr["TUV_Control_No"]),
                                Review_And_Analysis = Convert.ToString(dr["Review_And_Analysis"]),
                                Disposal_Action = Convert.ToString(dr["Disposal_Action"]),
                                Disposal_By = Convert.ToString(dr["Disposal_By"]),
                                Date_Of_Disposal = Convert.ToString(dr["Date_Of_Disposal"]),
                                Remarks = Convert.ToString(dr["Remarks"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                Mode_Of_Appeal = Convert.ToString(dr["Mode_Of_Appeal"]),
                                Date_of_Appeal = Convert.ToString(dr["Appeal_Date"])
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["AppealList"] = lstAppeal;

            AplMas.lstAppeal1 = lstAppeal;
            return AplMas.lstAppeal1;
        }


        public ActionResult Delete(int id)
        {
            int result = 0;
            try
            {
                result = custSpecific.DeleteAppeal(id);
                if (result != 0)
                {
                    TempData["result"] = result;
                    return RedirectToAction("AppealDashBoard");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
    }
}