using NonFactors.Mvc.Grid;
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
    public class CallRegisterController : Controller
    {

        DalCallRegister DalCall = new DalCallRegister();
        CallsRegister Objcall = new CallsRegister();
        // GET: CallRegister
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CallRegister()
        {
            try
            {
                Session["GetExcelData"] = "Yes";

                DataTable DTDashBoard = new DataTable();
                List<CallsRegister> lstcalls = new List<CallsRegister>();
                lstcalls = DalCall.GetData();
                ViewData["CallsRegister"] = lstcalls;
                Objcall.listingCallRegister = lstcalls;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View(Objcall);
        }



        [HttpPost]
        public ActionResult CallRegister(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            DataTable DTDashBoard = new DataTable();
            List<CallsRegister> lstcalls = new List<CallsRegister>();
            DTDashBoard = DalCall.CallsRegisterDataSearch(FromDate, ToDate);
            if (DTDashBoard.Rows.Count > 0)
            {
                foreach (DataRow dr in DTDashBoard.Rows)
                {
                    lstcalls.Add(
                        new CallsRegister
                        {
                            Job = Convert.ToString(dr["Job"]),
                            call_no = Convert.ToString(dr["call_no"]),
                            Call_Received_Date = Convert.ToString(dr["Call Received Date"]),
                            Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                            Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                            createddate = Convert.ToString(dr["CreatedDate"]),
                            CreatedBy = Convert.ToString(dr["CreatedBy"]),
                            Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                            Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                            Continuous_Call = Convert.ToString(dr["Continuous Call"]),
                            inspector = Convert.ToString(dr["Inspector"]),
                            status = Convert.ToString(dr["Status"]),
                            iscompetant = Convert.ToString(dr["iscompetant"]),
                            CallAssignBy = Convert.ToString(dr["CallAssignBy"]),
                            client = Convert.ToString(dr["client"]),
                            End_Customer = Convert.ToString(dr["End_Customer"]),
                            Vendor_Name = Convert.ToString(dr["Vendor_Name"]),

                            //added by shrutika salve 29012023
                            ProjectName = Convert.ToString(dr["Project_Name"]),
                            DECPMCName = Convert.ToString(dr["DECName"]),
                            itemTobeInspected = Convert.ToString(dr["Itemtobeinspected"]),
                            stageOfinspection = Convert.ToString(dr["stageInspection"]),
                            primaryMaterial = Convert.ToString(dr["PrimaryMaterial"]),
                            StageDescription = Convert.ToString(dr["Description"]),
                            itemQty = Convert.ToString(dr["Quantity"]),
                            EstimatedTimeinHours = Convert.ToString(dr["EstimatedHours"]),
                            FormFilled = Convert.ToString(dr["FormFilled"]),
                            IsVerified = Convert.ToString(dr["IsVerified"]),

                            //added by shrutika salve 29032024
                            Finalinsepection = Convert.ToString(dr["FinalInspection"]),
                            ManMonthsAssi = Convert.ToString(dr["ManMonthsAssignment"]),
                            subendusername = Convert.ToString(dr["subendUsername"]),
                            subsubendusername = Convert.ToString(dr["SubSubendUsername"]),
                            //added by shrutika salve 24042024
                            vendorLoaction = Convert.ToString(dr["InspectionLocation"]),
                            SAP_NO = Convert.ToString(dr["sapnumber"]),
                            CallCancelDate = Convert.ToString(dr["CallCancelledDate"]),
                            callcancelby = Convert.ToString(dr["CallCancelledBy"]),
                            CallAssignDate = Convert.ToString(dr["callAssignDate"]),
                        }

               );
                }
            }

            else
            {


                TempData["Result"] = "No Record Found";
                TempData.Keep();
                Objcall.listingCallRegister = lstcalls;
                return View(Objcall);

            }

            TempData["Result"] = "No Record Found";
            TempData.Keep();
            Objcall.listingCallRegister = lstcalls;
            return View(Objcall);







        }
        public ActionResult ExportAllIndex()
        {
            // Using EPPlus from nuget
            using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsRegister> grid = CreateAllExportableGrid();
                OfficeOpenXml.ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<CallsRegister> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "CallsRegister-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<CallsRegister> CreateAllExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsRegister> grid = new Grid<CallsRegister>(GetAllData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Columns.Add(model => model.Job).Titled("Job");
            grid.Columns.Add(model => model.call_no).Titled("Call No");
            grid.Columns.Add(model => model.inspector).Titled("Inspector");
            grid.Columns.Add(model => model.Vendor_Name).Titled("Vendor Name");
            //added by shrutika salve 24042024
            grid.Columns.Add(model => model.vendorLoaction).Titled("Inspection Location");

            grid.Columns.Add(model => model.client).Titled("Customer Name");
            grid.Columns.Add(model => model.End_Customer).Titled("End Customer Name");

            grid.Columns.Add(model => model.ProjectName).Titled("Project Name");
            grid.Columns.Add(model => model.DECPMCName).Titled("DEC/PMC/EPC Name");
            grid.Columns.Add(model => model.itemTobeInspected).Titled("Item to be Inspected");
            grid.Columns.Add(model => model.stageOfinspection).Titled("Stages of Inspection:");
            grid.Columns.Add(model => model.primaryMaterial).Titled("Primary Material");
            grid.Columns.Add(model => model.StageDescription).Titled("Stage Description");
            grid.Columns.Add(model => model.itemQty).Titled("Item Quantity");
            grid.Columns.Add(model => model.EstimatedTimeinHours).Titled("Estimated Inspection Time in hours");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.Executing_Branch).Titled("Executing Branch");
            grid.Columns.Add(model => model.createddate).Titled("Call Created Date");
            grid.Columns.Add(model => model.Call_Received_Date).Titled("Call Received Date");
            grid.Columns.Add(model => model.Call_Request_Date).Titled("Call Request Date");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.CreatedBy).Titled("Call Created By");
            grid.Columns.Add(model => model.CallAssignBy).Titled("Call Assign By");
            grid.Columns.Add(model => model.status).Titled("Status");
            grid.Columns.Add(model => model.Continuous_Call).Titled("Continuous Call");
            grid.Columns.Add(model => model.iscompetant).Titled("Is inspector Competant");
            grid.Columns.Add(model => model.FormFilled).Titled("Form Filled");
            //added by shrutika salve 29032024
            grid.Columns.Add(model => model.Finalinsepection).Titled("Final Inspection");
            grid.Columns.Add(model => model.ManMonthsAssi).Titled("Man Month Assignment");

            grid.Columns.Add(model => model.subendusername).Titled("Sub End User Name");
            grid.Columns.Add(model => model.subsubendusername).Titled("SubSub End User Name");
            grid.Columns.Add(model => model.SAP_NO).Titled("SAP Number");
            //  grid.Columns.Add(model => model.IsVerified).Titled("Verified");




            grid.Pager = new GridPager<CallsRegister>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = Objcall.listingCallRegister.Count;


            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }
            Session["FromDate"] = null;
            Session["FromDate"] = null;
            return grid;
        }
        public List<CallsRegister> GetAllData()
        {

            DataTable DTDashBoard = new DataTable();
            List<CallsRegister> lstcalls = new List<CallsRegister>();
            if (Session["FromDate"] != null && Session["ToDate"] != null)//added by nikita

            {
                DTDashBoard = DalCall.CallsRegisterDataSearch(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
                if (DTDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTDashBoard.Rows)
                    {
                        lstcalls.Add(new CallsRegister
                        {
                            Count = DTDashBoard.Rows.Count,
                            Job = Convert.ToString(dr["Job"]),
                            call_no = Convert.ToString(dr["call_no"]),
                            Call_Received_Date = Convert.ToString(dr["Call Received Date"]),
                            Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                            Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                            createddate = Convert.ToString(dr["CreatedDate"]),
                            CreatedBy = Convert.ToString(dr["CreatedBy"]),
                            Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                            Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                            Continuous_Call = Convert.ToString(dr["Continuous Call"]),
                            inspector = Convert.ToString(dr["Inspector"]),
                            status = Convert.ToString(dr["Status"]),
                            iscompetant = Convert.ToString(dr["iscompetant"]),
                            CallAssignBy = Convert.ToString(dr["CallAssignBy"]),
                            client = Convert.ToString(dr["client"]),
                            End_Customer = Convert.ToString(dr["End_Customer"]),
                            Vendor_Name = Convert.ToString(dr["Vendor_Name"]),

                            //added by shrutika salve 29012023
                            ProjectName = Convert.ToString(dr["Project_Name"]),
                            DECPMCName = Convert.ToString(dr["DECName"]),
                            itemTobeInspected = Convert.ToString(dr["Itemtobeinspected"]),
                            stageOfinspection = Convert.ToString(dr["stageInspection"]),
                            primaryMaterial = Convert.ToString(dr["PrimaryMaterial"]),
                            StageDescription = Convert.ToString(dr["Description"]),
                            itemQty = Convert.ToString(dr["Quantity"]),
                            EstimatedTimeinHours = Convert.ToString(dr["EstimatedHours"]),
                            FormFilled = Convert.ToString(dr["FormFilled"]),
                            IsVerified = Convert.ToString(dr["IsVerified"]),

                            //added by shrutika salve 29032024
                            Finalinsepection = Convert.ToString(dr["FinalInspection"]),
                            ManMonthsAssi = Convert.ToString(dr["ManMonthsAssignment"]),
                            subendusername = Convert.ToString(dr["subendUsername"]),
                            subsubendusername = Convert.ToString(dr["SubSubendUsername"]),
                            //added by shrutika salve 24042024
                            vendorLoaction = Convert.ToString(dr["InspectionLocation"]),
                             SAP_NO= Convert.ToString(dr["sapnumber"])
                        }
                       );

                    }
                }
               
            }
            else
            {
                //return RedirectToAction("YourElseAction");
                lstcalls = DalCall.GetData();
            }


            Objcall.listingCallRegister = lstcalls;
            return Objcall.listingCallRegister; 


        }
    }
}