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
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using SelectPdf;
using System.IO;

namespace TuvVision.Controllers
{
    public class CalenderController : Controller
    {
        DALNonInspectionActivity objNIA = new DALNonInspectionActivity();
        DALUsers objDalCreateUser = new DALUsers();
        NonInspectionActivity objModel = new NonInspectionActivity();
        // GET: Calender
        public ActionResult Calender()
        {
            try
            {


                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();

                Session["GetExcelData"] = "Yes";

                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                   
                    NonInspectionActivity n = new NonInspectionActivity();
                   
                    n.FromD = Convert.ToString(TempData["FromD"]);
                    n.ToD = Convert.ToString(TempData["ToD"]);
                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                    TempData.Keep();

                    ds = objNIA.GetCalenderDataByDate(n);
                }
                else
                {
                    ds = objNIA.GetCalenderData();
                }


            
                

                //ds = objNIA.GetCalenderData(); // fill dataset  

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                    {
                        lmd.Add(new NonInspectionActivity
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            ActivityType = Convert.ToString(dr["ActivityType"]),
                            Location = Convert.ToString(dr["Location"]),
                            
                            DateSE = Convert.ToString(dr["DateSE"]),
                            Name = Convert.ToString(dr["UserName"]),
                            Branch = Convert.ToString(dr["Branch_Name"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            Code = Convert.ToString(dr["SOCode"]),

                            ServiceCode = Convert.ToString(dr["ServiceCode"]),
                            Description = Convert.ToString(dr["Description"]),

                            StartTime = Convert.ToDouble(dr["StartTime"]),
                            EndTime = Convert.ToDouble(dr["EndTime"]),                            
                            TravelTime = Convert.ToDouble(dr["TravelTime"]),

                            Attachment = Convert.ToString(dr["Attachment"]),
                            JobNumber = Convert.ToString(dr["JobNumber"]),
                            SAP_No = Convert.ToString(dr["SAP_No"])

                        });
                    }
                    ViewData["NonInspectionActivityList"] = lmd;
                    objModel.NIADashBoard = lmd;
                   
                }
                else
                {
                    ViewData["NonInspectionActivityList"] = lmd;
                    objModel.NIADashBoard = lmd;
                  
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(objModel);
        }

        [HttpPost]
        public ActionResult Calender(NonInspectionActivity n,FormCollection fc)
        {
            try
            {
                Session["GetExcelData"] = null;

                TempData["FromD"] = n.FromD;
                TempData["ToD"] = n.ToD;
                TempData.Keep();


                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();

                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                    n.FromD = Convert.ToString(TempData["FromD"]);
                    n.ToD = Convert.ToString(TempData["ToD"]);
                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                    TempData.Keep();
                    ds = objNIA.GetCalenderDataByDate(n); // fill dataset  
                }
                else
                {
                    ds = objNIA.GetCalenderData(); // fill dataset  
                }

               
                lmd.Clear();

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                    {
                        lmd.Add(new NonInspectionActivity
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            ActivityType = Convert.ToString(dr["ActivityType"]),
                            Location = Convert.ToString(dr["Location"]),
                           
                            DateSE = Convert.ToString(dr["DateSE"]),
                            Name = Convert.ToString(dr["UserName"]),
                            Branch = Convert.ToString(dr["Branch_Name"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            Code = Convert.ToString(dr["SOCode"]),

                            ServiceCode = Convert.ToString(dr["ServiceCode"]),
                            Description = Convert.ToString(dr["Description"]),
                            StartTime = Convert.ToDouble(dr["StartTime"]),
                            EndTime = Convert.ToDouble(dr["EndTime"]),
                            
                            TravelTime = Convert.ToDouble(dr["TravelTime"]),
                            Attachment = Convert.ToString(dr["Attachment"]),
                            JobNumber = Convert.ToString(dr["JobNumber"]),
                            SAP_No = Convert.ToString(dr["SAP_No"])

                        });
                    }
                }
                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;

            }
            //return View(lmd.ToList());
            return View(objModel);
        }


        #region export to excel 

        [HttpGet]
        public ActionResult ExportIndex(NonInspectionActivity c)
        {
            // Using EPPlus from nuget
            Session["GetExcelData"] = "Yes";
            //Session["FromDate"] = c.FromD;
           // Session["ToDate"] = c.ToD;
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<NonInspectionActivity> grid = CreateExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<NonInspectionActivity> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "Calender-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
                //return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<NonInspectionActivity> CreateExportableGrid(NonInspectionActivity c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<NonInspectionActivity> grid = new Grid<NonInspectionActivity>(GetData(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(model => model.DateSE).Titled("Date");
            grid.Columns.Add(model => model.Name).Titled("Name");
            grid.Columns.Add(model => model.Branch).Titled("Branch Name");
            grid.Columns.Add(model => model.EmployeeCode).Titled("Hr-Emp-Code");
            grid.Columns.Add(model => model.ActivityType).Titled("Activity Type");
            grid.Columns.Add(model => model.JobNumber).Titled("Job Number");
            grid.Columns.Add(model => model.SAP_No).Titled("SAP Number");
            //grid.Columns.Add(model => model.Code).Titled("Sap No.Costcenter");

            grid.Columns.Add(model => model.StartTime).Titled("Outdoor Time/OnSite Time");
            grid.Columns.Add(model => model.EndTime).Titled("Office Time/Offsite Time");
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time");
            grid.Columns.Add(model => model.Location).Titled("Location");
            grid.Columns.Add(model => model.Description).Titled("Description");
            grid.Columns.Add(model => model.ServiceCode).Titled("Service Code");
            grid.Columns.Add(model => model.Id).Titled("Id");

            grid.Pager = new GridPager<NonInspectionActivity>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objModel.NIADashBoard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<NonInspectionActivity> GetData(NonInspectionActivity c)
        {
            DataSet DTFeedback = new DataSet();
            FormCollection fc = new FormCollection();

            if (TempData["FromD"] != null && TempData["ToD"] != null)
            {
                c.FromD = Convert.ToString(TempData["FromD"]);
                c.ToD = Convert.ToString(TempData["ToD"]);
                objModel.FromD = Convert.ToString(TempData["FromD"]);
                objModel.ToD = Convert.ToString(TempData["ToD"]);
                TempData.Keep();
                DTFeedback = objNIA.GetCalenderDataByDate(c);
            }
            else
            {
                DTFeedback = objNIA.GetCalenderData();
            }

            //if (Session["FromDate"] != null && Session["ToDate"] != null)
            //{
            //    c.FromD = Session["FromDate"].ToString();
            //    c.ToD = Session["ToDate"].ToString();
            //    DTFeedback = objNIA.GetCalenderDataByDate(c);
            //}
            //else
            //{
            //    DTFeedback = objNIA.GetCalenderData();
            //}
            
            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
            //DataSet ds = new DataSet();

            //ds = objNIA.GetData(); // fill dataset  

            if (DTFeedback.Tables.Count > 0)
            {
                foreach (DataRow dr in DTFeedback.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new NonInspectionActivity
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        Location = Convert.ToString(dr["Location"]),
                        StartDate = Convert.ToString(dr["startdate"]),
                        EndDate = Convert.ToString(dr["enddate"]),
                        DateSE = Convert.ToString(dr["DateSE"]),
                        Name = Convert.ToString(dr["UserName"]),
                        Branch = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        Code = Convert.ToString(dr["SOCode"]),

                        ServiceCode = Convert.ToString(dr["ServiceCode"]),
                        Description = Convert.ToString(dr["Description"]),

                        StartTime = Convert.ToDouble(dr["StartTime"]),
                        EndTime = Convert.ToDouble(dr["EndTime"]),
                        TravelTime = Convert.ToDouble(dr["TravelTime"]),

                        Attachment = Convert.ToString(dr["Attachment"]),
                        JobNumber = Convert.ToString(dr["JobNumber"]),
                        SAP_No = Convert.ToString(dr["SAP_No"])

                    });
                }
                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;
                //return View(lmd.ToList());

            }
            return objModel.NIADashBoard;
        }


        #endregion


        [HttpGet]
        public ActionResult UpdateNonInspectionActivity(int? id)
        {
            var model = new NonInspectionActivity();
            DataSet dss = new DataSet();

            var Data1 = objDalCreateUser.GetCostCenterList();
            ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");

            dss = objNIA.GetDataById(Convert.ToInt32(id));
            if (dss.Tables[0].Rows.Count > 0)
            {
                model.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                model.ActivityType = dss.Tables[0].Rows[0]["ActivityType"].ToString();
                model.Description = dss.Tables[0].Rows[0]["Description"].ToString();
                
             //   model.StartDate = Convert.ToString(dss.Tables[0].Rows[0]["StartDate"]);
               // model.EndDate = Convert.ToString(dss.Tables[0].Rows[0]["EndDate"]);
                model.Location = dss.Tables[0].Rows[0]["Location"].ToString();
                model.DateSE = dss.Tables[0].Rows[0]["DateSE"].ToString();
                model.ServiceCode = dss.Tables[0].Rows[0]["ServiceCode"].ToString();
                model.StartTime = Convert.ToDouble(dss.Tables[0].Rows[0]["StartTime"].ToString());
                model.EndTime = Convert.ToDouble( dss.Tables[0].Rows[0]["EndTime"].ToString());
                model.ODTime = Convert.ToDouble(dss.Tables[0].Rows[0]["ODTime"].ToString());
                model.TravelTime = Convert.ToDouble(dss.Tables[0].Rows[0]["TravelTime"].ToString());
                model.JobNumber = dss.Tables[0].Rows[0]["JobNumber"].ToString();
                model.Attachment = dss.Tables[0].Rows[0]["Attachment"].ToString();

            }
            return View(model);
        }
     

        //After Update Return List
        [HttpPost]
        public ActionResult UpdateNonInspectionActivity(NonInspectionActivity R, int? Id, HttpPostedFileBase File, HttpPostedFileBase[] Image)
        {
            string Result = string.Empty;
            try
            {
                if (Image.Count() > 0)
                {
                    foreach (HttpPostedFileBase item in Image)
                    {
                        HttpPostedFileBase image = item;
                        if (image != null && image.ContentLength > 0)
                        {
                            string filePath = AppDomain.CurrentDomain.BaseDirectory + "NonInspectionActivityDocument\\" + image.FileName;
                            const string ImageDirectoryFP = "NonInspectionActivityDocument\\";
                            const string ImageDirectory = "~/NonInspectionActivityDocument/";
                            string ImagePath = "~/NonInspectionActivityDocument/" + image.FileName;
                            string fileNameWithExtension = System.IO.Path.GetExtension(image.FileName);
                            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
                            string ImageName = image.FileName;

                            int iteration = 1;

                            while (System.IO.File.Exists(Server.MapPath(ImagePath)))
                            {
                                ImagePath = string.Concat(ImageDirectory, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                                filePath = string.Concat(ImageDirectoryFP, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                                ImageName = string.Concat(fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                                iteration += 1;
                            }
                            if (iteration == 1)
                            {
                                image.SaveAs(filePath);
                            }
                            else
                            {
                                image.SaveAs(AppDomain.CurrentDomain.BaseDirectory + filePath);
                            }
                            R.Attachment += ImageName + ",";
                        }
                    }
                }


                double total = Convert.ToDouble(R.EndTime) + Convert.ToDouble(R.ODTime) + Convert.ToDouble(R.TravelTime);

                int _currtotal = Convert.ToInt32(R.StartTime) + Convert.ToInt32(R.EndTime) + Convert.ToInt32(R.ODTime) + Convert.ToInt32(R.TravelTime);

                bool blnLeaveApplied = false;
                
                R.TotalTime = total;

                DateTime Start = DateTime.ParseExact(R.StartDate, "dd/MM/yyyy", null);
                DateTime End = DateTime.ParseExact(R.EndDate, "dd/MM/yyyy", null);


                for (DateTime date = Start; date.Date <= End; date = date.AddDays(1))
                {
                    DateTime StDt = date;
                    R.DateSE = StDt.ToString("dd/MM/yyyy");
                    DataSet DTValidateTT = new DataSet();
                    DTValidateTT = objNIA.ValidateTT(StDt.ToString("dd/MM/yyyy"));

                    if (DTValidateTT.Tables[1].Rows.Count > 0) //// Leave Checking
                    {
                        if (Convert.ToInt32(DTValidateTT.Tables[1].Rows[0][0].ToString()) > 0)
                        {
                            if (TempData["Error"] != null)
                            {
                                if (TempData["Error"].ToString() == string.Empty)
                                {
                                    TempData["Error"] = "Leave has been applied for " + DTValidateTT.Tables[0].Rows[0]["DateSE"].ToString();
                                }
                                else
                                {
                                    TempData["Error"] = TempData["Error"] + DTValidateTT.Tables[0].Rows[0]["DateSE"].ToString();
                                }
                            }
                            else
                            {
                                TempData["Error"] = "Leave has been applied for " + DTValidateTT.Tables[0].Rows[0]["DateSE"].ToString();
                            }

                        }
                        blnLeaveApplied = true;
                    }

                    if (!blnLeaveApplied)
                    {
                        if (DTValidateTT.Tables[0].Rows.Count > 0)
                        {

                            int _prevtotal = Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["StartTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["EndTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["ODTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["TravelTime"]);

                            int _addition = _currtotal + _prevtotal;



                            if (_addition < 24)
                            {
                                //Result = objNIA.Insert(R);

                                if (Convert.ToInt16(Result) > 0)
                                {
                                    ModelState.Clear();
                                    TempData["Success"] = "Record Added Successfully...";
                                }
                                else
                                {
                                    TempData["Failure"] = "Something went Wrong! Please try Again";
                                }
                            }
                            else
                            {
                                TempData["Error"] = "You have excided 24 hrs for the day of " + StDt.ToString("dd/MM/yyyy");
                                // return RedirectToAction("ListNonInspectionActivity", "NonInspectionActivity");
                            }
                        }
                    }
                }              

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ModelState.Clear();
            return RedirectToAction("Calender", "Calender");
        }

        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objNIA.Delete(Convert.ToInt32(Id));
                if (Convert.ToInt16(Result) > 0)
                {


                    ModelState.Clear();
                }
                else
                {

                    TempData["message"] = "Something went Wrong! Please try Again";
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ModelState.Clear();
            return RedirectToAction("calender");


        }

        public ActionResult AttendanceSheet()
        {
            try
            {

                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();

                Session["GetExcelData"] = "Yes";

                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                    NonInspectionActivity n = new NonInspectionActivity();

                    n.StartDate = Convert.ToString(TempData["FromD"]);
                    n.EndDate = Convert.ToString(TempData["ToD"]);

                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                    TempData.Keep();

                    ds = objNIA.GetAttendanceSheet(n);
                }
                DataTable DtActivityMaster = new DataTable();

                DtActivityMaster = objNIA.GetActivityMaster();

                objModel.dtActivityMaster = DtActivityMaster;



                if (ds.Tables.Count > 0)
                {
                    objModel.dtUserAttendance = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(objModel);
        }

        [HttpPost]
        public ActionResult AttendanceSheet(NonInspectionActivity n, FormCollection fc)
        {
            try
            {
                Session["GetExcelData"] = null;

                TempData["FromD"] = n.FromD;
                TempData["ToD"] = n.ToD;
                TempData.Keep();


                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();

                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                    n.StartDate = Convert.ToString(TempData["FromD"]);
                    n.EndDate = Convert.ToString(TempData["ToD"]);

                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                    TempData.Keep();

                    ds = objNIA.GetAttendanceSheet(n); // fill dataset  
                }

                DataTable DtActivityMaster = new DataTable();

                DtActivityMaster = objNIA.GetActivityMaster();

                objModel.dtActivityMaster = DtActivityMaster;



                if (ds.Tables.Count > 0)
                {
                    objModel.dtUserAttendance = ds.Tables[0];
                }


            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
            //return View(lmd.ToList());
            return View(objModel);
        }


        #region export Attendance to excel 

        
        [HttpGet]
        public ActionResult ExportAttIndex(NonInspectionActivity c)
        {

            Session["GetExcelData"] = "Yes";

            using (ExcelPackage package = new ExcelPackage())
            {

                Int32 col = 1;
                Int32 row = 1;
                int LeaveCount = 0;
                int nullcount = 0;
                int SelectDays = 0;
                double filledPercentage = 0;
                string DayName = string.Empty;


                int OTHLeaveCount = 0;

                int CLeaveCount = 0;
                int SLeaveCount = 0;
                int PLeaveCount = 0;
                int WKDLeaveCount = 0;
                int WOLeaveCount = 0;
                int PHLeaveCount = 0;

                int WorkingDays = 0;


                package.Workbook.Worksheets.Add("Data");
                /// IGrid<NonInspectionActivity> grid = CreateAttExportableGrid(c);

                DataTable grid = CreateAttExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (DataColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.ColumnName.ToString();
                    sheet.Column(col++).Width = 18;
                }
                row++;
                for (int Nrow = 0; Nrow < grid.Rows.Count; Nrow++)
                {

                    for (int Ncol = 0; Ncol < grid.Columns.Count; Ncol++)
                    {
                        if (grid.Rows[Nrow][Ncol] != DBNull.Value &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMAIL" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TS FILLED DAYS" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WORKING DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OTHER LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SICK LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "CASUAL LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PRIVILEGE LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKLY OFF COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PUBLIC HOLIDAY COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKEND LEAVE COUNT"

                        )
                        {

                            DayName = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(grid.Columns[Ncol].ColumnName.ToString()).DayOfWeek);

                            if (grid.Rows[Nrow][Ncol].ToString() == "CL")
                            {
                                CLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "SL")
                            {
                                SLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "PL" || grid.Rows[Nrow][Ncol].ToString() == "A33")
                            {
                                PLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "33") // Weekly Off Count
                            {
                                WOLeaveCount++;
                                OTHLeaveCount++;
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "35") // Public Holidays
                            {
                                PHLeaveCount++;
                                OTHLeaveCount++;
                            }

                            nullcount++;
                        }
                    }
                    col = 1;

                    for (int Ncol1 = 0; Ncol1 < grid.Columns.Count; Ncol1++)
                    {

                        if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMAIL"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"

                        )
                        {

                            if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = LeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "OTHER LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = OTHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "CASUAL LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = CLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SICK LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = SLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PRIVILEGE LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WORKING DAYS")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WorkingDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKLY OFF COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WOLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PUBLIC HOLIDAY COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SELECTED PERIOD DAYS")
                            {
                                SelectDays = Convert.ToInt32(grid.Rows[Nrow][Ncol1].ToString());
                                sheet.Cells[row, col].Value = SelectDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "TS FILLED DAYS")
                            {
                                filledPercentage = (Convert.ToDouble(nullcount) / Convert.ToDouble(SelectDays)) * 100;
                                if (filledPercentage > 72)
                                {
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                                else
                                {
                                    sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCyan);
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "PL" || grid.Rows[Nrow][Ncol1].ToString() == "CL" || grid.Rows[Nrow][Ncol1].ToString() == "SL" || grid.Rows[Nrow][Ncol1].ToString() == "A33")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                if (grid.Rows[Nrow][Ncol1].ToString() == "A33")
                                {
                                    sheet.Cells[row, col].Value = "AUTO PL";
                                }
                                else
                                {
                                    sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "33")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                sheet.Cells[row, col].Value = "WO";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "35")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PeachPuff);
                                sheet.Cells[row, col].Value = "PH";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "INSP")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                sheet.Cells[row, col].Value = "P";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString().Contains(","))
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Honeydew);
                                sheet.Cells[row, col].Value = "P";
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKEND LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WKDLeaveCount.ToString();
                            }
                            else
                            {
                                if (grid.Rows[Nrow][Ncol1] == DBNull.Value)
                                    sheet.Cells[row, col].Value = string.Empty;
                                else
                                    sheet.Cells[row, col].Value = "P"; // grid.Rows[Nrow][Ncol1].ToString();
                            }

                        }
                        else
                        {
                            sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                        }
                        col++;

                    }



                    row++;







                }
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "AttendanceSheet-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }

        }

        private DataTable CreateAttExportableGrid(NonInspectionActivity c)
        {

            ///// IGrid<NonInspectionActivity> grid = new Grid<NonInspectionActivity>(GetAttData(c));


            DataTable grid = GetAttData(c);


            return grid;
        }

        public DataTable GetAttData(NonInspectionActivity c)
        {
            DataSet ds = new DataSet();
            FormCollection fc = new FormCollection();

            if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
            {
                c.StartDate = Convert.ToString(TempData["FromD"]);
                c.EndDate = Convert.ToString(TempData["ToD"]);

                objModel.FromD = Convert.ToString(TempData["FromD"]);
                objModel.ToD = Convert.ToString(TempData["ToD"]);
                TempData.Keep();

                ds = objNIA.GetAttendanceSheet(c); // fill dataset  
            }

            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  


            if (ds.Tables.Count > 0)
            {
                /*foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new NonInspectionActivity
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        Location = Convert.ToString(dr["Location"]),
                        StartDate = Convert.ToString(dr["startdate"]),
                        EndDate = Convert.ToString(dr["enddate"]),
                        DateSE = Convert.ToString(dr["DateSE"]),

                        ServiceCode = Convert.ToString(dr["ServiceCode"]),
                        Description = Convert.ToString(dr["Description"]),

                        StartTime = Convert.ToDouble(dr["StartTime"]),
                        EndTime = Convert.ToDouble(dr["EndTime"]),
                        TravelTime = Convert.ToDouble(dr["TravelTime"]),

                        Attachment = Convert.ToString(dr["Attachment"]),
                        JobNumber = Convert.ToString(dr["JobNumber"]),
                        SAP_No = Convert.ToString(dr["SAP_No"])

                    });
                }
                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;
                */

            }
            return ds.Tables[0];
        }


        #endregion

        #region export Attendance to excel 

        [HttpGet]
        public ActionResult GenerateAttExport(NonInspectionActivity c)
        {

            Session["GetExcelData"] = "Yes";

            using (ExcelPackage package = new ExcelPackage())
            {

                Int32 col = 1;
                Int32 row = 1;
                int LeaveCount = 0;
                int nullcount = 0;

                int OTHLeaveCount = 0;

                int CLeaveCount = 0;
                int SLeaveCount = 0;
                int PLeaveCount = 0;

                int WOLeaveCount = 0;
                int PHLeaveCount = 0;
                int SelectDays = 0;
                int WKDLeaveCount = 0;
                int NACount = 0;
                int WorkingDays = 0;
                double filledPercentage = 0;
                string DayName = string.Empty;

                package.Workbook.Worksheets.Add("Data");
                /// IGrid<NonInspectionActivity> grid = CreateAttExportableGrid(c);

                DataTable grid = CreateAttExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (DataColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.ColumnName.ToString();
                    sheet.Column(col++).Width = 18;
                }
                row++;
                for (int Nrow = 0; Nrow < grid.Rows.Count; Nrow++)
                {

                    for (int Ncol = 0; Ncol < grid.Columns.Count; Ncol++)
                    {
                        if (grid.Rows[Nrow][Ncol] != DBNull.Value &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMAIL" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TS FILLED DAYS" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WORKING DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OTHER LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SICK LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "CASUAL LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PRIVILEGE LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKLY OFF COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PUBLIC HOLIDAY COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKEND LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "NOA COUNT"

                        )
                        {

                            DayName = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(grid.Columns[Ncol].ColumnName.ToString()).DayOfWeek);

                            if (grid.Rows[Nrow][Ncol].ToString().Contains("CL"))
                            {
                                CLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString().Contains("SL"))
                            {
                                SLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString().Contains("PL") || grid.Rows[Nrow][Ncol].ToString().Contains("A33"))
                            {
                                PLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "33") // Weekly Off Count
                            {
                                WOLeaveCount++;
                                OTHLeaveCount++;
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "35") // Public Holidays
                            {
                                PHLeaveCount++;
                                OTHLeaveCount++;
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString().Contains("48"))
                            {
                                NACount++;
                                
                                
                            }

                            nullcount++;
                        }
                    }

                    col = 1;
                    for (int Ncol1 = 0; Ncol1 < grid.Columns.Count; Ncol1++)
                    {
                        if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMAIL"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"

                        )
                        {

                            if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = LeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "OTHER LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = OTHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "CASUAL LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = CLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SICK LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = SLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PRIVILEGE LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WORKING DAYS")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WorkingDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKLY OFF COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WOLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PUBLIC HOLIDAY COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SELECTED PERIOD DAYS")
                            {
                                SelectDays = Convert.ToInt32(grid.Rows[Nrow][Ncol1].ToString());
                                sheet.Cells[row, col].Value = SelectDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "TS FILLED DAYS")
                            {
                                filledPercentage = (Convert.ToDouble(nullcount) / Convert.ToDouble(SelectDays)) * 100;
                                if (filledPercentage > 72)
                                {
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                                else
                                {
                                    sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCyan);
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString().Contains("PL") || grid.Rows[Nrow][Ncol1].ToString().Contains("CL") || grid.Rows[Nrow][Ncol1].ToString().Contains("SL") || grid.Rows[Nrow][Ncol1].ToString().Contains("A33"))
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                if (grid.Rows[Nrow][Ncol1].ToString().Contains("A33"))
                                {
                                    sheet.Cells[row, col].Value = "AUTO PL";
                                }
                                else
                                {
                                    sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "33")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                sheet.Cells[row, col].Value = "WO";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "35")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PeachPuff);
                                sheet.Cells[row, col].Value = "PH";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "INSP")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                sheet.Cells[row, col].Value = "P";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString().Contains("48"))
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                sheet.Cells[row, col].Value = "NoA";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString().Contains(","))
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Honeydew);
                                sheet.Cells[row, col].Value = "P";
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKEND LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WKDLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "NOA COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                sheet.Cells[row, col].Value = NACount.ToString();
                            }
                           
                            else
                            {
                                if (grid.Rows[Nrow][Ncol1] == DBNull.Value)
                                    sheet.Cells[row, col].Value = string.Empty;
                                else
                                    sheet.Cells[row, col].Value = "P"; // grid.Rows[Nrow][Ncol1].ToString();
                            }

                        }
                        else
                        {
                            sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                        }
                        col++;

                    }
                    row++;
                }
                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }

        }




        #endregion


        //added by ruchita  on 08052024
        [HttpGet]
        public ActionResult ExportPDF(NonInspectionActivity c)
        {
            // Read the HTML template
            string htmlTemplatePath = Server.MapPath("~/Exportsummery/ObsHtml.html");
            string body = string.Empty;
            SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
            using (StreamReader reader = new StreamReader(htmlTemplatePath))
            {
                body = reader.ReadToEnd();
            }
            string employeeGrade = "";
            string categoryName = "";
            string SAPEmpCode = "";
            string SAPVenderCode = "";
            string branchName = string.Empty;
            DataTable dt = new DataTable();
            string UserID = (string)Session["UserID"];
            dt = GetDetails(UserID);
            if (dt.Rows.Count > 0)
            {
                categoryName = dt.Rows[0]["Name"].ToString();
                employeeGrade = dt.Rows[0]["EmployeeGrade"].ToString();
                SAPEmpCode = dt.Rows[0]["SAPEmpCode"].ToString();
                SAPVenderCode = dt.Rows[0]["SAP_VendorCode"].ToString();
                branchName = Convert.ToString(dt.Rows[0]["Branch_Name"]);

            }
            System.Text.StringBuilder tableBody = new System.Text.StringBuilder();

            DataTable grid = CreateExportableGrid_(c);
            //string fullName = (string)Session["fullName"];
            //body = body.Replace("[Name]", fullName.ToString());
            //string Branch = (string)Session["UserBranchId"]; ;
            //body = body.Replace("[Branch]", Branch.ToString());
            //string Empcode = (string)Session["EmpCode"]; 
            //body = body.Replace("[Empcode]", Empcode.ToString());
            //string Category = categoryName;
            //body = body.Replace("[Category]", Category.ToString());
            //string Cost = (string)Session["costcentre"];
            //body = body.Replace("[Cost]", Cost.ToString());
            //string Grade = employeeGrade;
            //body = body.Replace("[Grade]", Grade.ToString());
            //string SAPemp = SAPEmpCode;
            //body = body.Replace("[SAPemp]", SAPemp.ToString());

            //string SAPVendor = SAPVenderCode;
            //body = body.Replace("[SAPVendor]", SAPVenderCode.ToString());

            //string period = c.FromD + " To " + c.ToD;
            //body = body.Replace("[period]", period.ToString());
            //DateTime PrintedOn = DateTime.Now;
            //body = body.Replace("[PrintedOn]", PrintedOn.ToString());

            // Iterate through each row in the DataTable
            //commented on 28082024
            //foreach (DataRow row in grid.Rows)
            //{
            //    // Append a new row tag for each row
            //    tableBody.Append("<tr>");

            //    // Iterate through each column in the row
            //    foreach (DataColumn column in grid.Columns)
            //    {
            //        // Get the column data, handling DBNull.Value if necessary
            //        string columnData = row.IsNull(column) ? "" : row[column].ToString();

            //        // Append the column data to the table row
            //        tableBody.Append("<td>").Append(columnData).Append("</td>");
            //    }

            //    // Close the row tag
            //    tableBody.Append("</tr>");
            //}
            foreach (DataRow row in grid.Rows)
            {
                // Check the ActivityType and apply the corresponding background color
                bool isWeeklyOff = row["ActivityType"].ToString() == "Weekly off";
                bool isNoAssignment = row["ActivityType"].ToString() == "No Assignment";
                bool isHoliday = row["ActivityType"].ToString() == "Holiday";
                bool isSickLeave = row["ActivityType"].ToString() == "Sick Leave";
                bool iscasual = row["ActivityType"].ToString() == "Casual Leave";
                bool isprivilege = row["ActivityType"].ToString() == "Privilege Leave";


                if (isWeeklyOff)
                {
                    tableBody.Append("<tr style='background-color: #FFF366;'>"); // Yellow
                }
                else if (isNoAssignment)
                {
                    tableBody.Append("<tr style='background-color: #FDAFB5;'>"); // Red
                }
                else if (isHoliday)
                {
                    tableBody.Append("<tr style='background-color: #FFF366;'>"); // Yellow
                }
                else if (isSickLeave)
                {
                    tableBody.Append("<tr style='background-color: #FFF366;'>"); // Yellow
                }
                else if (iscasual)
                {
                    tableBody.Append("<tr style='background-color: #FFF366;'>"); // Yellow
                }
                else if (isprivilege)
                {
                    tableBody.Append("<tr style='background-color: #FFF366;'>"); // Yellow
                }
                else
                {
                    tableBody.Append("<tr>");
                }

                foreach (DataColumn column in grid.Columns)
                {
                    string columnData = row.IsNull(column) ? "" : row[column].ToString();
                    tableBody.Append("<td>").Append(columnData).Append("</td>");
                }

                tableBody.Append("</tr>");
            }
            body = body.Replace("[TableBody]", tableBody.ToString());

            PdfPageSize pageSize = PdfPageSize.A4;
            //PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Landscape; // Set landscape orientation

            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.MaxPageLoadTime = 360;



            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.SecurityOptions.CanAssembleDocument = true;
            converter.Options.SecurityOptions.CanCopyContent = true;
            converter.Options.SecurityOptions.CanEditAnnotations = true;
            converter.Options.SecurityOptions.CanEditContent = true;
            converter.Options.SecurityOptions.CanFillFormFields = true;
            converter.Options.SecurityOptions.CanPrint = true;




            #region Header & footer
            string _Header = string.Empty;
            string _footer = string.Empty;

            StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/Exportsummery/header.html"));
            _Header = _readHeader_File.ReadToEnd();

            string fullName = (string)Session["fullName"];
            _Header = _Header.Replace("[Name]", fullName.ToString());
            string Branch = branchName;
            _Header = _Header.Replace("[Branch]", Branch.ToString());
            string Empcode = (string)Session["EmpCode"];
            _Header = _Header.Replace("[Empcode]", Empcode.ToString());
            string Category = categoryName;
            _Header = _Header.Replace("[Category]", Category.ToString());
            string Cost = (string)Session["costcentre"];
            _Header = _Header.Replace("[Cost]", Cost.ToString());
            //string Grade = employeeGrade;
            //_Header = _Header.Replace("[Grade]", Grade.ToString());
            string SAPemp = SAPEmpCode;
            _Header = _Header.Replace("[SAPemp]", SAPemp.ToString());

            string SAPVendor = SAPVenderCode;
            _Header = _Header.Replace("[SAPVendor]", SAPVenderCode.ToString());

            string period = c.FromD + " To " + c.ToD;
            _Header = _Header.Replace("[period]", period.ToString());
            DateTime PrintedOn = DateTime.Now;
            _Header = _Header.Replace("[PrintedOn]", PrintedOn.ToString());



            StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/Exportsummery/footer.html"));
            _footer = _readFooter_File.ReadToEnd();


            // header settings
            converter.Options.DisplayHeader = true ||
                true || true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = 110;

            PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;

            converter.Header.Add(headerHtml);

            // footer settings
            converter.Options.DisplayFooter = true || true || true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Footer.Height = 61;

            PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);

            PdfTextSection text1 = new PdfTextSection(30, 52, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("TNG Pro", 8));

            converter.Footer.Add(text1);



            #endregion

            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(body);
            int PageCount = doc.Pages.Count;


            // Generate file name and path
            string filename = "Calender-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".pdf";
            string filePath = Server.MapPath("~/PDFs/" + filename);

            // Save PDF to file
            doc.Save(filePath);
            doc.Close();

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/pdf", filename);
        }
        public DataTable GetDetails(string userID)
        {
            DataTable dt = new DataTable();
            dt = objNIA.GetUserDetails(userID);
            return dt;
        }

        private DataTable CreateExportableGrid_(NonInspectionActivity c)
        {
            DataTable dataTable = new DataTable();

            // Add columns to the DataTable
            dataTable.Columns.Add("Date", typeof(string));
            //dataTable.Columns.Add("Name", typeof(string));
            //dataTable.Columns.Add("BranchName", typeof(string));
            //dataTable.Columns.Add("Hr-Employee-Code", typeof(string));
            dataTable.Columns.Add("ActivityType", typeof(string));
            dataTable.Columns.Add("JobNumber", typeof(string));
            dataTable.Columns.Add("SAPNumber", typeof(string));
            dataTable.Columns.Add("Cost Center", typeof(string));
            dataTable.Columns.Add("OutdoorTime/On-SiteTime", typeof(string));
            dataTable.Columns.Add("OfficeTime/Off-siteTime", typeof(string));
            dataTable.Columns.Add("TravelTime", typeof(double));
            dataTable.Columns.Add("Location", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            //dataTable.Columns.Add("ServiceCode", typeof(string));
            dataTable.Columns.Add("Id", typeof(string));

            var data = GetData_(c);

            foreach (var item in data)
            {
                DataRow row = dataTable.NewRow();

                row["Date"] = item.DateSE;
                //row["Name"] = item.Name;
                //row["BranchName"] = item.Branch;
                //row["Hr-Employee-Code"] = item.EmployeeCode;
                row["ActivityType"] = item.ActivityType;
                row["JobNumber"] = item.JobNumber;
                row["SAPNumber"] = item.SAP_No;
                row["Cost Center"] = item.Code;
                row["OutdoorTime/On-SiteTime"] = item.StartTime;
                row["OfficeTime/Off-siteTime"] = item.EndTime;
                row["TravelTime"] = item.TravelTime;
                row["Location"] = item.Location;
                row["Description"] = item.Description;
                //row["ServiceCode"] = item.ServiceCode;
                row["Id"] = item.Id;

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public List<NonInspectionActivity> GetData_(NonInspectionActivity c)
        {
            DataSet DTFeedback = new DataSet();
            FormCollection fc = new FormCollection();

            if (TempData["FromD"] != null && TempData["ToD"] != null)
            {
                c.FromD = Convert.ToString(TempData["FromD"]);
                c.ToD = Convert.ToString(TempData["ToD"]);
                objModel.FromD = Convert.ToString(TempData["FromD"]);
                objModel.ToD = Convert.ToString(TempData["ToD"]);
                TempData.Keep();
                DTFeedback = objNIA.GetCalenderDataByDate(c);
            }
            else
            {
                DTFeedback = objNIA.GetCalenderData();
            }

            //if (Session["FromDate"] != null && Session["ToDate"] != null)
            //{
            //    c.FromD = Session["FromDate"].ToString();
            //    c.ToD = Session["ToDate"].ToString();
            //    DTFeedback = objNIA.GetCalenderDataByDate(c);
            //}
            //else
            //{
            //    DTFeedback = objNIA.GetCalenderData();
            //}

            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
            //DataSet ds = new DataSet();

            //ds = objNIA.GetData(); // fill dataset  

            if (DTFeedback.Tables.Count > 0)
            {
                foreach (DataRow dr in DTFeedback.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new NonInspectionActivity
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        Location = Convert.ToString(dr["Location"]),
                        StartDate = Convert.ToString(dr["startdate"]),
                        EndDate = Convert.ToString(dr["enddate"]),
                        DateSE = Convert.ToString(dr["DateSE"]),
                        Name = Convert.ToString(dr["UserName"]),
                        Branch = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        Code = Convert.ToString(dr["SOCode"]),

                        ServiceCode = Convert.ToString(dr["ServiceCode"]),
                        Description = Convert.ToString(dr["Description"]),

                        StartTime = Convert.ToDouble(dr["StartTime"]),
                        EndTime = Convert.ToDouble(dr["EndTime"]),
                        TravelTime = Convert.ToDouble(dr["TravelTime"]),

                        Attachment = Convert.ToString(dr["Attachment"]),
                        JobNumber = Convert.ToString(dr["JobNumber"]),
                        SAP_No = Convert.ToString(dr["SAP_No"])

                    });
                }
                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;
                //return View(lmd.ToList());

            }
            return objModel.NIADashBoard;
        }
        //end
    }
}