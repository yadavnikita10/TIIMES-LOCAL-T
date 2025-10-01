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
    public class DelayController : Controller
    {

        IVRDelay objModel = new IVRDelay();
        IRNDelay objModel1 = new IRNDelay();
        DALDelay objDAL = new DALDelay();
        #region Delay IVR
        public ActionResult DelayIVR(IVRDelay n)
        {
            try
            {
                TempData["FromD"] = objModel.FromD;
                TempData["ToD"] = objModel.ToD;
                TempData.Keep();

                List<IVRDelay> lmd = new List<IVRDelay>();  // creating list of model.  
                DataSet ds = new DataSet();


                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                    ds = objDAL.GetIVRDelayByDate(n);

                    n.FromD = Convert.ToString(TempData["FromD"]);
                    n.ToD = Convert.ToString(TempData["ToD"]);
                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                }
                else
                {
                    ds = objDAL.GetIVRDelay();
                }

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                    {
                        lmd.Add(new IVRDelay
                        {
                            Call_No = Convert.ToString(dr["Call_No"]),
                            Call_CreatedDate = Convert.ToString(dr["Call_CreatedDate"]),
                            Call_CreatedBy = Convert.ToString(dr["Call_CreatedBy"]),
                            Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                            CallType = Convert.ToString(dr["DynamicColumn"]),
                            Sub_JobNo = Convert.ToString(dr["Sub_Job"]),
                            Customer_Name = Convert.ToString(dr["Customer_Name"]),
                            ProjectName = Convert.ToString(dr["Project_Name"]),
                            VendorName = Convert.ToString(dr["Vendor_Name"]),
                            SubVendorName = Convert.ToString(dr["SubVendorName"]),
                            P1 = Convert.ToString(dr["Po_Number"]),
                            P2 = Convert.ToString(dr["Po_No_SSJob"]),
                            
                            Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                            Excuting_Branch = Convert.ToString(dr["Excuting_Branch"]),
                            InspectorName = Convert.ToString(dr["InspectorName"]),
                            ContinuousDate = Convert.ToString(dr["ContinuousDate"]),
                        });
                    }
                }

                objModel.lstDelay= lmd;



            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View(objModel);
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
                IGrid<IVRDelay> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<IVRDelay> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<IVRDelay> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<IVRDelay> grid = new Grid<IVRDelay>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.Call_No).Titled("Call_No");
            grid.Columns.Add(model => model.Call_CreatedDate).Titled("Call_CreatedDate");
            grid.Columns.Add(model => model.Call_CreatedBy).Titled("Call_CreatedBy");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual_Visit_Date");
            grid.Columns.Add(model => model.CallType).Titled("CallType");
            grid.Columns.Add(model => model.ContinuousDate).Titled("ContinuousDate");
            grid.Columns.Add(model => model.Sub_JobNo).Titled("Sub_JobNo");
            grid.Columns.Add(model => model.Customer_Name).Titled("Customer_Name");
            //grid.Columns.Add(model => model.ProjectName).Titled("ProjectName");
            //grid.Columns.Add(model => model.VendorName).Titled("VendorName");
            //grid.Columns.Add(model => model.SubVendorName).Titled("SubVendorName");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating_Branch");
            grid.Columns.Add(model => model.Excuting_Branch).Titled("Excuting_Branch");
            grid.Columns.Add(model => model.InspectorName).Titled("InspectorName");




            grid.Pager = new GridPager<IVRDelay>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objModel.lstDelay.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<IVRDelay> GetData()
        {

            DataTable JobDashBoard = new DataTable();
            List<IVRDelay> lstCompanyDashBoard = new List<IVRDelay>();
            JobDashBoard = objDAL.dtGetIVRDelay();
            try
            {
                if (JobDashBoard.Rows.Count > 0)
                {
                    //int abc = JobDashBoard.Rows.Count;
                    // int data = 1 + abc;
                    foreach (DataRow dr in JobDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new IVRDelay
                            {
                                Count = JobDashBoard.Rows.Count,
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Call_CreatedDate = Convert.ToString(dr["Call_CreatedDate"]),
                                Call_CreatedBy = Convert.ToString(dr["Call_CreatedBy"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                CallType = Convert.ToString(dr["DynamicColumn"]),
                                ContinuousDate = Convert.ToString(dr["ContinuousDate"]),
                                Sub_JobNo = Convert.ToString(dr["Sub_Job"]),
                                Customer_Name = Convert.ToString(dr["Customer_Name"]),
                                //ProjectName         = Convert.ToString(dr["ProjectName"]),
                                //VendorName          = Convert.ToString(dr["VendorName"]),
                                //SubVendorName       = Convert.ToString(dr["SubVendorName"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Excuting_Branch = Convert.ToString(dr["Excuting_Branch"]),
                                InspectorName = Convert.ToString(dr["InspectorName"]),

                            }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["JobList"] = lstCompanyDashBoard;
            objModel.lstDelay = lstCompanyDashBoard;


            return objModel.lstDelay;
        }
        #endregion


        #endregion



        #region IRN Delay
        public ActionResult DelayIrn(IRNDelay n)
        {
            try
            {
                TempData["FromD"] = objModel.FromD;
                TempData["ToD"] = objModel.ToD;
                TempData.Keep();

                List<IRNDelay> lmd1 = new List<IRNDelay>();  // creating list of model.  
                DataSet ds = new DataSet();


                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                    ds = objDAL.GetIRNDelayWithD(n);

                    n.FromD = Convert.ToString(TempData["FromD"]);
                    n.ToD = Convert.ToString(TempData["ToD"]);
                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                }
                else
                {
                    ds = objDAL.GetIRNDelay();
                }

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                    {
                        lmd1.Add(new IRNDelay
                        {
                            InspectorName = Convert.ToString(dr["InspectorName"]),
                            InspectorEmail = Convert.ToString(dr["InspectorEmail"]),
                            ReportNo = Convert.ToString(dr["ReportNo"]),
                            Call_No = Convert.ToString(dr["Call_No"]),
                            Date_Of_Inspection = Convert.ToString(dr["Date_Of_Inspection"]),
                            Vendor_name = Convert.ToString(dr["Vendor_name"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                        });
                    }
                }

                objModel1.lstIRNDelay = lmd1;



            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View(objModel1);
        }
        #endregion

    }
}