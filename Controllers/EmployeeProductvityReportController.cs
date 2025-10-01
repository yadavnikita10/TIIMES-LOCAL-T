using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TuvVision.DataAccessLayer;
using TuvVision.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using OfficeOpenXml;
using NonFactors.Mvc.Grid;

namespace TuvVision.Controllers
{
    public class EmployeeProductvityReportController : Controller
    {
        // GET: EmployeeProductvityReport
        DALEmployeeProductvityReport ObjDalEPReport = new DALEmployeeProductvityReport();
        EmployeeProductvityReport objEPR = new EmployeeProductvityReport();
        public ActionResult ExportToExcelEProductvityReport(int? ID, DateTime? FromDate, DateTime? ToDate, string BranchName, string InspectorID)
        {
            #region Bind dropdownlist
            Session["GetExcelData"] = "Yes";
            if (Convert.ToString(Session["Result"]) != null && Convert.ToString(Session["Result"]) != "")
            {
                TempData["Result"] = Convert.ToString(Session["Result"]);
                TempData.Keep();
            }
            DataSet DSddlBranchName = new DataSet();
            List<NameCode> lstBranch = new List<NameCode>();
            List<N> lstInspector = new List<N>();
            DSddlBranchName = ObjDalEPReport.GetDdlBranchLst();
            if (DSddlBranchName.Tables[0].Rows.Count > 0)
            {
                lstBranch = (from n in DSddlBranchName.Tables[0].AsEnumerable()
                             select new NameCode()
                             {
                                 Name = n.Field<string>(DSddlBranchName.Tables[0].Columns["BranchName"].ToString()),
                                 Code = n.Field<Int32>(DSddlBranchName.Tables[0].Columns["PK_ID"].ToString())

                             }).ToList();
            }
            ViewBag.Branch = lstBranch;
            if (DSddlBranchName.Tables[1].Rows.Count > 0)
            {
                lstInspector = (from n in DSddlBranchName.Tables[1].AsEnumerable()
                                select new N()
                                {
                                    Name = n.Field<string>(DSddlBranchName.Tables[1].Columns["Name"].ToString()),
                                    Code = n.Field<string>(DSddlBranchName.Tables[1].Columns["PK_UserID"].ToString())

                                }).ToList();
            }
            ViewBag.Inspector = lstInspector;

            #endregion




            #region list
            var todate = DateTime.Now.ToShortDateString();
            EmployeeProductvityReport ObjEPReport = new EmployeeProductvityReport();
            var grid = new GridView();
            List<EmployeeProductvityReport> lmd = new List<EmployeeProductvityReport>();

            DataSet DTEPReport = ObjDalEPReport.GetExcelEmployeeProductvityReport(FromDate, ToDate, BranchName, InspectorID);
            if (DTEPReport.Tables[0].Rows.Count < 0)
            {
                //grid.DataSource = DTEPReport;
                //grid.DataBind();
                //if (grid.Rows.Count > 0)
                foreach (DataRow dr in DTEPReport.Tables[0].Rows)
                {

                    lmd.Add(new EmployeeProductvityReport
                    {
                        BranchName = Convert.ToString(dr["BranchName"]),
                        Location_Name = Convert.ToString(dr["LocationName"]),
                        InspectorID = Convert.ToString(dr["EmployeName"]),
                        TPI = Convert.ToString(dr["TPI"]),
                        PED = Convert.ToString(dr["PED"]),
                        ASME = Convert.ToString(dr["ASME"]),
                        Energy = Convert.ToString(dr["Energy"]),
                        OTHERS = Convert.ToString(dr["OTHERS"]),
                        RAILWAY = Convert.ToString(dr["RAILWAY"]),
                        IBR = Convert.ToString(dr["IBR"]),
                        EXPEDITING_SERVICES = Convert.ToString(dr["EXPEDITING_SERVICES"]),
                        TRCU = Convert.ToString(dr["TRCU"]),
                        PESO = Convert.ToString(dr["PESO"]),
                        PNGRB = Convert.ToString(dr["PNGRB"]),
                        VENDOR_EVALUATION = Convert.ToString(dr["VENDOR_EVALUATION"]),
                        DESIGN_APPRAISAL = Convert.ToString(dr["DESIGN_APPRAISAL"]),
                        ASSET_INTEGRITY = Convert.ToString(dr["ASSET_INTEGRITY"]),
                        QUANTITY_VERIFICATION = Convert.ToString(dr["QUANTITY_VERIFICATION"]),
                        RENEWABLE = Convert.ToString(dr["RENEWABLE"]),
                        NUCLEAR = Convert.ToString(dr["NUCLEAR"]),
                        CONVENTIONAL_POWER_PLANT = Convert.ToString(dr["CONVENTIONAL_POWER_PLANT"]),
                        TotalCount = Convert.ToString(dr["TotalCount"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        TotalHoursCount = Convert.ToString(dr["TotalHoursCount"]),
                        LeveCount = Convert.ToString(dr["LeveCount"]),


                    });


                    ViewData["EmployeeProductvityReport"] = lmd;


                   


                }


                }
                objEPR.lst1 = lmd;

                #endregion


                return View(objEPR);
                //return View();
            }
        //Download Excel Report
        [HttpPost]
        public ActionResult ExportToExcelEProductvityReport(DateTime? FromDate, DateTime? ToDate, string BranchName, string InspectorID)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            Session["BranchName"] = BranchName;
            Session["InspectorID"] = InspectorID;

            var todate = DateTime.Now.ToShortDateString();
            EmployeeProductvityReport ObjEPReport = new EmployeeProductvityReport();
            var grid = new GridView();
            List<EmployeeProductvityReport> lmd = new List<EmployeeProductvityReport>();
            DataSet DTEPReport = ObjDalEPReport.GetExcelEmployeeProductvityReport(FromDate, ToDate, BranchName, InspectorID);
            if (DTEPReport.Tables[0].Rows.Count > 0)
            {
                //grid.DataSource = DTEPReport;
                //grid.DataBind();
                //if (grid.Rows.Count > 0)
                foreach (DataRow dr in DTEPReport.Tables[0].Rows)
                {

                    lmd.Add(new EmployeeProductvityReport
                    {
                        BranchName = Convert.ToString(dr["BranchName"]),
                        Location_Name = Convert.ToString(dr["LocationName"]),
                        InspectorID = Convert.ToString(dr["EmployeName"]),
                        TPI = Convert.ToString(dr["TPI"]),
                        PED = Convert.ToString(dr["PED"]),
                        ASME = Convert.ToString(dr["ASME"]),
                        Energy = Convert.ToString(dr["Energy"]),
                        OTHERS = Convert.ToString(dr["OTHERS"]),
                        RAILWAY = Convert.ToString(dr["RAILWAY"]),
                        IBR = Convert.ToString(dr["IBR"]),
                        EXPEDITING_SERVICES = Convert.ToString(dr["EXPEDITING_SERVICES"]),
                        TRCU = Convert.ToString(dr["TRCU"]),
                        PESO = Convert.ToString(dr["PESO"]),
                        PNGRB = Convert.ToString(dr["PNGRB"]),
                        VENDOR_EVALUATION = Convert.ToString(dr["VENDOR_EVALUATION"]),
                        DESIGN_APPRAISAL = Convert.ToString(dr["DESIGN_APPRAISAL"]),
                        ASSET_INTEGRITY = Convert.ToString(dr["ASSET_INTEGRITY"]),
                        QUANTITY_VERIFICATION = Convert.ToString(dr["QUANTITY_VERIFICATION"]),
                        RENEWABLE = Convert.ToString(dr["RENEWABLE"]),
                        NUCLEAR = Convert.ToString(dr["NUCLEAR"]),
                        CONVENTIONAL_POWER_PLANT = Convert.ToString(dr["CONVENTIONAL_POWER_PLANT"]),
                        TotalCount = Convert.ToString(dr["TotalCount"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        TotalHoursCount = Convert.ToString(dr["TotalHoursCount"]),
                        LeveCount = Convert.ToString(dr["LeveCount"]),


                    });


                    ViewData["EmployeeProductvityReport"] = lmd;


                    #region Bind branch name and inspector list 
                    DataSet DSddlBranchName = new DataSet();
                    List<NameCode> lstBranch = new List<NameCode>();
                    List<N> lstInspector = new List<N>();
                    DSddlBranchName = ObjDalEPReport.GetDdlBranchLst();
                    if (DSddlBranchName.Tables[0].Rows.Count > 0)
                    {
                        lstBranch = (from n in DSddlBranchName.Tables[0].AsEnumerable()
                                     select new NameCode()
                                     {
                                         Name = n.Field<string>(DSddlBranchName.Tables[0].Columns["BranchName"].ToString()),
                                         Code = n.Field<Int32>(DSddlBranchName.Tables[0].Columns["PK_ID"].ToString())

                                     }).ToList();
                    }
                    ViewBag.Branch = lstBranch;
                    if (DSddlBranchName.Tables[1].Rows.Count > 0)
                    {
                        lstInspector = (from n in DSddlBranchName.Tables[1].AsEnumerable()
                                        select new N()
                                        {
                                            Name = n.Field<string>(DSddlBranchName.Tables[1].Columns["Name"].ToString()),
                                            Code = n.Field<string>(DSddlBranchName.Tables[1].Columns["PK_UserID"].ToString())

                                        }).ToList();
                    }
                    ViewBag.Inspector = lstInspector;

                    #endregion





                    #region Export To Excel


                    //    grid.HeaderRow.Style.Add("background-color", "#FFFF00");
                    //    grid.HeaderRow.Cells[0].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[1].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[2].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[3].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[4].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[5].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[6].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[7].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[8].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[9].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[10].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[11].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[12].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[13].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[14].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[15].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[16].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[17].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[18].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[19].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[20].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[21].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[22].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[23].Style.Add("background-color", "yellow");
                    //    grid.HeaderRow.Cells[24].Style.Add("background-color", "yellow");

                    //    for (int i = 0; i < grid.Rows.Count; i++)
                    //    {
                    //        GridViewRow row = grid.Rows[i];

                    //        row.BackColor = System.Drawing.Color.White;

                    //        row.Attributes.Add("class", "textmode");

                    //        if (i % 2 != 0)
                    //        {
                    //            row.Cells[0].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[1].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[2].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[3].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[4].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[5].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[6].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[7].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[8].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[9].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[10].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[11].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[12].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[13].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[14].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[15].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[16].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[17].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[18].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[19].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[20].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[21].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[22].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[23].Style.Add("background-color", "#C2D69B");
                    //            row.Cells[24].Style.Add("background-color", "#C2D69B");
                    //        }
                    //    }
                    //}
                    //Response.ClearContent();
                    //Response.AddHeader("content-disposition", "attachment; filename=Employee_Product_Report" + "-" + todate + ".xls");
                    //Response.ContentType = "application/excel";
                    //StringWriter sw = new StringWriter();
                    //HtmlTextWriter htw = new HtmlTextWriter(sw);
                    //grid.RenderControl(htw);
                    //Response.Write(sw.ToString());
                    //Response.Flush();
                    //Response.End();

                    #endregion
                }
                objEPR.lst1 = lmd;
                return View(objEPR);

            }
            else
            {
                Session["Result"] = "No Data Found";
                return RedirectToAction("ExportToExcelEProductvityReport", "EmployeeProductvityReport", new { @ID = 0 });
            }
        }


        #region export to excel
        [HttpGet]
        public ActionResult ExportIndex()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<EmployeeProductvityReport> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<EmployeeProductvityReport> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<EmployeeProductvityReport> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<EmployeeProductvityReport> grid = new Grid<EmployeeProductvityReport>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.BranchName).Titled("Branch Name");
            grid.Columns.Add(model => model.Location_Name).Titled("Location Name");
            grid.Columns.Add(model => model.InspectorID).Titled("Employee Name");
            grid.Columns.Add(model => model.TPI).Titled("TPI");
            grid.Columns.Add(model => model.PED).Titled("PED");
            grid.Columns.Add(model => model.ASME).Titled("ASME");
            grid.Columns.Add(model => model.Energy).Titled("Energy");
            grid.Columns.Add(model => model.OTHERS).Titled("Others");
            grid.Columns.Add(model => model.RAILWAY).Titled("Railway");
            grid.Columns.Add(model => model.IBR).Titled("IBR");
            grid.Columns.Add(model => model.EXPEDITING_SERVICES).Titled("Expediting Services");
            grid.Columns.Add(model => model.TRCU).Titled("TRCU");
            grid.Columns.Add(model => model.PESO).Titled("PESO");
            grid.Columns.Add(model => model.PNGRB).Titled("PNGRB");
            grid.Columns.Add(model => model.VENDOR_EVALUATION).Titled("Vendor Evaluation");
            grid.Columns.Add(model => model.DESIGN_APPRAISAL).Titled("Design Appraisal");
            grid.Columns.Add(model => model.ASSET_INTEGRITY).Titled("Asset Integrity");
            grid.Columns.Add(model => model.QUANTITY_VERIFICATION).Titled("Quantity Verification");
            grid.Columns.Add(model => model.RENEWABLE).Titled("Renewable");
            grid.Columns.Add(model => model.NUCLEAR).Titled("Nuclear");
            grid.Columns.Add(model => model.CONVENTIONAL_POWER_PLANT).Titled("Conventional Power Plant");
            grid.Columns.Add(model => model.TotalCount).Titled("Total Count");
            grid.Columns.Add(model => model.ActivityType).Titled("Activity Type");
            grid.Columns.Add(model => model.TotalHoursCount).Titled("Total Hours Count");
            grid.Columns.Add(model => model.LeveCount).Titled("Leve Count");


            grid.Pager = new GridPager<EmployeeProductvityReport>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objEPR.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<EmployeeProductvityReport> GetData()
        {
            DataSet DTEPReport = new DataSet();
            var todate = DateTime.Now.ToShortDateString();
            EmployeeProductvityReport ObjEPReport = new EmployeeProductvityReport();
            var grid = new GridView();
            List<EmployeeProductvityReport> lmd = new List<EmployeeProductvityReport>();

            if (Session["GetExcelData"] == "Yes")
            {
                DTEPReport = ObjDalEPReport.GetExcelEmployeeProductvityReport(Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]), Session["BranchName"].ToString(), Session["InspectorID"].ToString());
            }
            else
            {


                DTEPReport = ObjDalEPReport.GetExcelEmployeeProductvityReport(Convert.ToDateTime(Session["FromDate"]), Convert.ToDateTime(Session["ToDate"]), Session["BranchName"].ToString(), Session["InspectorID"].ToString());
            }


                if (DTEPReport.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in DTEPReport.Tables[0].Rows)
                    {
                        #region

                        lmd.Add(new EmployeeProductvityReport
                        {
                            BranchName = Convert.ToString(dr["BranchName"]),
                            Location_Name = Convert.ToString(dr["LocationName"]),
                            InspectorID = Convert.ToString(dr["EmployeName"]),
                            TPI = Convert.ToString(dr["TPI"]),
                            PED = Convert.ToString(dr["PED"]),
                            ASME = Convert.ToString(dr["ASME"]),
                            Energy = Convert.ToString(dr["Energy"]),
                            OTHERS = Convert.ToString(dr["OTHERS"]),
                            RAILWAY = Convert.ToString(dr["RAILWAY"]),
                            IBR = Convert.ToString(dr["IBR"]),
                            EXPEDITING_SERVICES = Convert.ToString(dr["EXPEDITING_SERVICES"]),
                            TRCU = Convert.ToString(dr["TRCU"]),
                            PESO = Convert.ToString(dr["PESO"]),
                            PNGRB = Convert.ToString(dr["PNGRB"]),
                            VENDOR_EVALUATION = Convert.ToString(dr["VENDOR_EVALUATION"]),
                            DESIGN_APPRAISAL = Convert.ToString(dr["DESIGN_APPRAISAL"]),
                            ASSET_INTEGRITY = Convert.ToString(dr["ASSET_INTEGRITY"]),
                            QUANTITY_VERIFICATION = Convert.ToString(dr["QUANTITY_VERIFICATION"]),
                            RENEWABLE = Convert.ToString(dr["RENEWABLE"]),
                            NUCLEAR = Convert.ToString(dr["NUCLEAR"]),
                            CONVENTIONAL_POWER_PLANT = Convert.ToString(dr["CONVENTIONAL_POWER_PLANT"]),
                            TotalCount = Convert.ToString(dr["TotalCount"]),
                            ActivityType = Convert.ToString(dr["ActivityType"]),
                            TotalHoursCount = Convert.ToString(dr["TotalHoursCount"]),
                            LeveCount = Convert.ToString(dr["LeveCount"]),


                        });
                        #endregion
                    }
                    ViewData["EmployeeProductvityReport"] = lmd;

                    objEPR.lst1 = lmd;
                    return objEPR.lst1;
                }
            objEPR.lst1 = lmd;
            return objEPR.lst1;
        } 
        #endregion




    }
}