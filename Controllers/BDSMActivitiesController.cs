using Newtonsoft.Json;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class BDSMActivitiesController : Controller
    {
        BDSMActivity BDSMActivityData = new BDSMActivity();
        DalBDSMActivityData objBDSM = new DalBDSMActivityData();
        BDSPiechart pie = new BDSPiechart();

        public ActionResult Index()
        {
            return View();
        }

      

        [HttpGet]
        public ActionResult BDSMActivity(BDSMActivity BDSM)
        {
            try
            {
                Session["GetExcelData"] = "Yes";

                DataTable DTDashBoard = new DataTable();
                List<BDSMActivity> lstBDSMActivityrecord = new List<BDSMActivity>();
                lstBDSMActivityrecord = objBDSM.GetData();
                ViewData["BDSMActivityData"] = lstBDSMActivityrecord;
                BDSMActivityData.listingBDSMActivityrecord = lstBDSMActivityrecord;

            }

            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View(BDSMActivityData);
        }
        [HttpPost]
        public ActionResult BDSMActivity(BDSMActivity BDSM, string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            DataTable DTDashBoard = new DataTable();
            List<BDSMActivity> lstBDSMActivityrecord = new List<BDSMActivity>();
            DTDashBoard = objBDSM.BdsmActivityDataSearch(FromDate, ToDate);
            if (DTDashBoard.Rows.Count > 0)
            {
                foreach (DataRow dr in DTDashBoard.Rows)
                {
                    lstBDSMActivityrecord.Add(
                        new BDSMActivity
                        {
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            HREmployeeCode = Convert.ToString(dr["HREmployeeCode"]),
                            SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                            EmployeeCategory = Convert.ToString(dr["EmployeeCategory"]),
                            Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            CostCenter = Convert.ToString(dr["Cost_Center"]),
                            Id = Convert.ToString(dr["Id"]),
                            TravelTime = Convert.ToString(dr["TravelTime"]),
                            ActivityType = Convert.ToString(dr["ActivityType"]),
                            ActivityDate = Convert.ToString(dr["ActivityDate"]),
                            StartTime = Convert.ToString(dr["StartTime"]),
                            EndTime = Convert.ToString(dr["EndTime"]),
                            Location = Convert.ToString(dr["Location"]),
                            New_ExistingCustomer = Convert.ToString(dr["New_ExistingCustomer"]),
                            Dom_Inter_Visit = Convert.ToString(dr["Dom_Inter_Visit"]),
                            Description = Convert.ToString(dr["Description"]),
                        }
               );
                }
            }

            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                BDSMActivityData.listingBDSMActivityrecord = lstBDSMActivityrecord;
                return View(BDSMActivityData);
            }
            TempData["Result"] = "No Record Found";
            TempData.Keep();
            BDSMActivityData.listingBDSMActivityrecord = lstBDSMActivityrecord;
            return View(BDSMActivityData);
        }
        public ActionResult ExportAllIndex()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<BDSMActivity> grid = CreateAllExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<BDSMActivity> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "BDSMActivity-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<BDSMActivity> CreateAllExportableGrid()
        {
            IGrid<BDSMActivity> grid = new Grid<BDSMActivity>(GetAllData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Columns.Add(model => model.Id).Titled("UIN");
            grid.Columns.Add(model => model.EmployeeName).Titled("Employee Name");
            grid.Columns.Add(model => model.HREmployeeCode).Titled("HR Employee Code");
            grid.Columns.Add(model => model.Tuv_Email_Id).Titled("TUV Email Id");
            grid.Columns.Add(model => model.MobileNo).Titled("Mobile No");
            grid.Columns.Add(model => model.Designation).Titled("Designation");
            grid.Columns.Add(model => model.Id).Titled("Id");
            grid.Columns.Add(model => model.CostCenter).Titled("Cost Center");
            grid.Columns.Add(model => model.ActivityType).Titled("Activities");
            grid.Columns.Add(model => model.ActivityDate).Titled("Activity Date");
            grid.Columns.Add(model => model.StartTime).Titled("Outdoor / On-Site Time (Hrs.):");
            grid.Columns.Add(model => model.EndTime).Titled("Office / Off-Site Time (Hrs.):");
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time (Hrs.):");
            grid.Columns.Add(model => model.Location).Titled("City/Cities:");
            grid.Columns.Add(model => model.New_ExistingCustomer).Titled("Customer Type :New/Existing");
            grid.Columns.Add(model => model.Dom_Inter_Visit).Titled("Customer Type :Domestic/International");
            grid.Columns.Add(model => model.Description).Titled("Description of Activity:");


            grid.Pager = new GridPager<BDSMActivity>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = BDSMActivityData.listingBDSMActivityrecord.Count;


            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }
            Session["FromDate"] = null;
            Session["FromDate"] = null;
            return grid;
        }
        public List<BDSMActivity> GetAllData()
        {

            DataTable DTDashBoard = new DataTable();
            List<BDSMActivity> lstBDSMActivityrecord = new List<BDSMActivity>();
            if (Session["FromDate"] != null && Session["ToDate"] != null)//added by nikita

            {
                DTDashBoard = objBDSM.BdsmActivityDataSearch(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
                if (DTDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTDashBoard.Rows)
                    {
                        lstBDSMActivityrecord.Add(new BDSMActivity
                        {
                            Count = DTDashBoard.Rows.Count,
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            HREmployeeCode = Convert.ToString(dr["HREmployeeCode"]),
                            SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                            EmployeeCategory = Convert.ToString(dr["EmployeeCategory"]),
                            Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            CostCenter = Convert.ToString(dr["Cost_Center"]),
                            Id = Convert.ToString(dr["Id"]),
                            TravelTime = Convert.ToString(dr["TravelTime"]),
                            ActivityType = Convert.ToString(dr["ActivityType"]),
                            ActivityDate = Convert.ToString(dr["ActivityDate"]),
                            StartTime = Convert.ToString(dr["StartTime"]),
                            EndTime = Convert.ToString(dr["EndTime"]),
                            Location = Convert.ToString(dr["Location"]),
                            New_ExistingCustomer = Convert.ToString(dr["New_ExistingCustomer"]),
                            Dom_Inter_Visit = Convert.ToString(dr["Dom_Inter_Visit"]),
                            Description = Convert.ToString(dr["Description"]),

                        }
                       );

                    }
                }
               
            }
            else
            {
                lstBDSMActivityrecord = objBDSM.GetData();
            }


            BDSMActivityData.listingBDSMActivityrecord = lstBDSMActivityrecord;
            return BDSMActivityData.listingBDSMActivityrecord;


        }



        public ActionResult BdsmPieChart()
        {
           
            return View();

        }
        public ActionResult GetAllBranch()
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Piechart();

                result = JsonConvert.SerializeObject(dt);
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        lstBDSMActivityrecord.Add(new BDSPiechart


                //        {
                //            employee = Convert.ToString(dr["EmployeeName"]),
                //            Branch = Convert.ToString(dr["BranchName"]),



                //        }
                //        );
                //    }
                //}
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult GetAllEmployee(string Fromdate, string ToDate)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Data(Fromdate,ToDate);
                //List<BDSPiechart> lstBDSMActivityrecord = new List<BDSPiechart>();

                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult BindDataToChart2(string Fromdate, string ToDate)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.BindData(Fromdate, ToDate);

                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult BindDataToChartNewExisting(string Fromdate, string ToDate)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.NewExistingPiechart(Fromdate, ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult BindDataToChartDomInter(string Fromdate, string ToDate)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.DomesticInternationalPiechart(Fromdate,ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult BindTotext()
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.BindDataTotext();
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult BindEmpWiseFilterData(string PK_UserID, string Fromdate, string ToDate)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.BindEmpWiseFiltereddata(PK_UserID,Fromdate,ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult GetEmployee_BranchWise(int BranchId, string Fromdate, string ToDate)
        {
            string result;
            try
            {

                DataTable dt = new DataTable();
                dt = objBDSM.BranchData(BranchId,Fromdate,ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult GetBranch_EmployeeWise(string PK_UserId,string Fromdate,string ToDate)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.GetBranch_EmployeeWise(PK_UserId, Fromdate, ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Bind_Employee_Pie(string PK_UserId, string Fromdate, string ToDate)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Bind_Employee_Pie(PK_UserId,Fromdate,ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Bind_NewExistingFilter_Pie(string PK_UserId, string Fromdate, string ToDate)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Bind_NEwExitFiltereData_Pie(PK_UserId,Fromdate,ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Bind_DomInterFilter_Pie(string PK_UserId,string Fromdate,string ToDate)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Bind_Dom_Inter_FiltereData_Pie(PK_UserId,Fromdate,ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult SearchDataFromdateTodate(string Fromdate,string ToDate)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.From_ToDate_Data(Fromdate, ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult BindSearchDataFromdateTodate(string Fromdate, string ToDate)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Bind_data_From_ToDate_Data(Fromdate, ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }




        // this method is called on clicking branch and the count of branch will bind
        public ActionResult BranchdataFilter(int BranchId, string Fromdate, string ToDate)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.BranchFilteringCountdata(BranchId,Fromdate,ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

         public ActionResult PieBindSearchDataFromdateTodate(string Fromdate, string ToDate)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Pie_Bind_data_From_ToDate_Data(Fromdate, ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult PieNew_ExistingFromdateTodate(string Fromdate, string ToDate)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Pie_New_Existing_data_From_ToDate_Data(Fromdate, ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult PieDom_Inter_FromdateTodate(string Fromdate, string ToDate)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Pie_Dom_Inter_From_ToDate_Data(Fromdate, ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult Branch_FilteringData_FromdateTodate(string Fromdate, string ToDate)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Bind_Filter_Branch_dataFrom_ToDate_Data(Fromdate, ToDate);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult Bind_Branch_OnClick_Pie(string Fromdate, string ToDate,int BranchId)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Bind_Branch_OnClick_Piedata(Fromdate, ToDate, BranchId);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult Bind_Branch_OnClick_New_Exist(string Fromdate, string ToDate, int BranchId)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Bind_Branch_OnClick_Exist_New_data(Fromdate, ToDate, BranchId);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult Bind_Branch_OnClick_Dom_Inter(string Fromdate, string ToDate, int BranchId)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objBDSM.Bind_Branch_OnClick_Dom_Inter_New_data(Fromdate, ToDate, BranchId);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

    }
}

