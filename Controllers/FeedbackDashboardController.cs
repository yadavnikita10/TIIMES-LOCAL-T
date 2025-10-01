
using Newtonsoft.Json;
using System;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;
using OfficeOpenXml.Style;
using NonFactors.Mvc.Grid;

namespace TuvVision.Controllers
{
    public class FeedbackDashboardController : Controller
    {
        FeedbackDashboardDAL feedbackDashboardDAL = new FeedbackDashboardDAL();
        customerFeedbackregister Obj1 = new customerFeedbackregister();
        // GET: FeedbackDashboard
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDashboard(string fromDate, string toDate)
        {
            DataSet ds = new DataSet();
            if (fromDate != null && fromDate != "" && toDate != null && toDate != "")
            {
                

                ds = feedbackDashboardDAL.GetDashboardDate(fromDate, toDate);
            }
            else
            {
                ds = feedbackDashboardDAL.GetDashboard();
            }



            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string JobNo = TuvVision.Encryption.Feedback.Encryption.Encrypt(row["JobNo"].ToString());
                if (row["JobNo"].ToString() != "Total")
                    row["JobID"] = JobNo;
            }

            string json = JsonConvert.SerializeObject(ds.Tables[0]);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClientFeedbackHistory()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Get_ClientFeedbackHistory(string tkn_key)
        {
            string JobID = TuvVision.Encryption.Feedback.Encryption.Decrypt(tkn_key);
            //string job = tkn_key;
            DataSet ds = feedbackDashboardDAL.ClientFeedbackHistory(JobID);
            string json = JsonConvert.SerializeObject(ds.Tables[0]);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult customerRagister()
        {
            DataTable DTCustomerFeedback = new DataTable();
            List<customerFeedbackregister> lstFeedbackregister = new List<customerFeedbackregister>();
            DTCustomerFeedback = feedbackDashboardDAL.GetCustomerFeedback();


            string showData = string.Empty;
            try
            {
                if (DTCustomerFeedback.Rows.Count > 0)
                {

                    foreach (DataRow dr in DTCustomerFeedback.Rows)
                    {


                        lstFeedbackregister.Add(
                            new customerFeedbackregister
                            {

                                RequestCreatedBy = Convert.ToString(dr["RequestCreatedBy"]),
                                Branch_Name = Convert.ToString(dr["Branch_Name"]),
                                RequestDate = Convert.ToString(dr["RequestDate"]),
                                CompanyName = Convert.ToString(dr["CompanyName"]),
                                JobNo = Convert.ToString(dr["JobNo"]),
                                AddressOfOrg = Convert.ToString(dr["AddressOfOrg"]),
                                ResponseRequiredDate = Convert.ToString(dr["ResponseRequiredDate"]),
                                ResponseFilledDate = Convert.ToString(dr["ResponseFilledDate"]),
                                NameOfOrg = Convert.ToString(dr["NameOfOrg"]),
                                NameOfRespondant = Convert.ToString(dr["NameOfRespondant"]),
                                DesignationOfRespondant = Convert.ToString(dr["DesignationOfRespondant"]),
                                FeedbackRemark = Convert.ToString(dr["FeedbackRemarks"]),
                                Emailid = Convert.ToString(dr["emailid"]),
                                QNo1 = Convert.ToString(dr["How well did we respond to your enquiry? "]),
                                QNo2 = Convert.ToString(dr["Did You receive the Quotation within Expected Time Frame?"]),
                                QNo3 = Convert.ToString(dr["How well did we understand your requirements?"]),
                                QNo4 = Convert.ToString(dr["Did the quotation contain all the required information? "]),
                                QNo5 = Convert.ToString(dr["How quickly were your queries resolved & revised quotation submitted?"]),
                                QNo6 = Convert.ToString(dr["Was your email / call responded on time?"]),
                                QNo7 = Convert.ToString(dr["Was the call scheduled as per your request?"]),
                                QNo8 = Convert.ToString(dr["Did you receive information regarding conformation of calls, inspector details in advance?"]),
                                QNo9 = Convert.ToString(dr["In case of change in schedule were you informed within expected time frame?"]),
                                QNo10 = Convert.ToString(dr["How satisfied are you with the communication process with the inspection coordination team?"]),
                                QNo11 = Convert.ToString(dr["How would you rate the behaviour of TUV inspector with your staff (w.r.t. language, attitude)? "]),
                                QNo12 = Convert.ToString(dr["How would you rate the inspector’s safety awareness & leadership in implementation of safety requirements?"]),
                                QNo13 = Convert.ToString(dr["How would you rate the quality of inspection w.r.t. observations raised? "]),
                                QNo14 = Convert.ToString(dr["How would you rate the efficiency of our inspector w.r.t time taken for inspection?"]),
                                QNo15 = Convert.ToString(dr["How would you rate our inspectors for maintaining confidentiality & Integrity?"]),
                                QNo16 = Convert.ToString(dr["Did you receive the inspection report/Release note in time?"]),
                                QNo17 = Convert.ToString(dr["Did the report meet your expectations w.r.t. completeness, contents and conclusion"]),
                                QNo18 = Convert.ToString(dr["How would you rate the report for number of errors, revisions?"]),
                                QNo19 = Convert.ToString(dr["for Improvement"]),
                                //QNo20 = Convert.ToString(dr["Further Exception"]),
                            }
                            );
                    }



                    Obj1.lstCustomerFeedback = lstFeedbackregister;
                    Session["CustomerList"] = lstFeedbackregister;

                    return View(Obj1);
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            //ViewData["ComplaintList"] = lstComplaintDashBoard;
            //CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
            return View();
        }

        [HttpGet]
        public ActionResult GetDashboardExportExcel(string fromDate, string toDate)
        {
            DataSet ds = new DataSet();
            if (fromDate != null && fromDate != "" && toDate != null && toDate != "")
            {
                ds = feedbackDashboardDAL.GetDashboardDate(fromDate, toDate);
            }
            else
            {
                ds = feedbackDashboardDAL.GetDashboard();
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string JobNo = TuvVision.Encryption.Feedback.Encryption.Encrypt(row["JobNo"].ToString());
                if (row["JobNo"].ToString() != "Total") ;

            }

            // Create the Excel package
            using (var package = new ExcelPackage())
            {
                // Add a worksheet to the Excel package
                var worksheet = package.Workbook.Worksheets.Add("Dashboard");

                // Load data into the worksheet
                worksheet.Cells["A1"].LoadFromDataTable(ds.Tables[0], true);

                // Set the content type and file name
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=DashboardExport.xlsx");

                // Write the Excel package to the response stream
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();
            }

            return new EmptyResult();
        }


        

        public ActionResult ExportIndex2(customerFeedbackregister U)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<customerFeedbackregister> grid = CreateExportableGrid1(U);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                using (ExcelRange range = sheet.Cells["A1:AF1"])
                {
                    range.Style.Font.Bold = true;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<customerFeedbackregister> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "customerFeedbackDashboard" + DateTime.Now.ToShortDateString() + ".xlsx");
            }
        }
        private IGrid<customerFeedbackregister> CreateExportableGrid1(customerFeedbackregister U)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<customerFeedbackregister> grid = new Grid<customerFeedbackregister>(GetData1(U));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };





            grid.Columns.Add(model => model.RequestCreatedBy).Titled("Request Created By");
            grid.Columns.Add(model => model.Branch_Name).Titled("Branch Name");
            grid.Columns.Add(model => model.RequestDate).Titled("Request Date");
            grid.Columns.Add(model => model.CompanyName).Titled("Company Name");
            grid.Columns.Add(model => model.JobNo).Titled("Job Number");
            grid.Columns.Add(model => model.AddressOfOrg).Titled("Address of Organization");
            grid.Columns.Add(model => model.ResponseRequiredDate).Titled("Response Required Date");
            grid.Columns.Add(model => model.ResponseFilledDate).Titled("Respondent Filled Date");
            grid.Columns.Add(model => model.NameOfOrg).Titled("Name of Organization");
            grid.Columns.Add(model => model.NameOfRespondant).Titled("Name of Respondent");
            grid.Columns.Add(model => model.DesignationOfRespondant).Titled("Designation of Respondent");
            grid.Columns.Add(model => model.FeedbackRemark).Titled("Feedback Remarks");
            grid.Columns.Add(model => model.Emailid).Titled("Email ID");
            grid.Columns.Add(model => model.QNo1).Titled("How well did we respond to your enquiry?");
            grid.Columns.Add(model => model.QNo2).Titled("Did You receive the Quotation within Expected Time Frame?");
            grid.Columns.Add(model => model.QNo3).Titled("How well did we understand your requirements?");
            grid.Columns.Add(model => model.QNo4).Titled("Did the quotation contain all the required information? ");
            grid.Columns.Add(model => model.QNo5).Titled("How quickly were your queries resolved & revised quotation submitted?");
            grid.Columns.Add(model => model.QNo6).Titled("Was your email / call responded on time?");
            grid.Columns.Add(model => model.QNo7).Titled("Was the call scheduled as per your request?");
            grid.Columns.Add(model => model.QNo8).Titled("Did you receive information regarding conformation of calls, inspector details in advance?");
            grid.Columns.Add(model => model.QNo9).Titled("In case of change in schedule were you informed within expected time frame?");
            grid.Columns.Add(model => model.QNo10).Titled("How satisfied are you with the communication process with the inspection coordination team?");
            grid.Columns.Add(model => model.QNo11).Titled("How would you rate the behaviour of TUV inspector with your staff (w.r.t. language, attitude)? ");
            grid.Columns.Add(model => model.QNo12).Titled("How would you rate the inspector’s safety awareness & leadership in implementation of safety requirements?");
            grid.Columns.Add(model => model.QNo13).Titled("How would you rate the quality of inspection w.r.t. observations raised? ");
            grid.Columns.Add(model => model.QNo14).Titled("How would you rate the efficiency of our inspector w.r.t time taken for inspection?");
            grid.Columns.Add(model => model.QNo15).Titled("How would you rate our inspectors for maintaining confidentiality & Integrity?");
            grid.Columns.Add(model => model.QNo16).Titled("Did you receive the inspection report/Release note in time?");
            grid.Columns.Add(model => model.QNo17).Titled("Did the report meet your expectations w.r.t. completeness, contents and conclusion");
            grid.Columns.Add(model => model.QNo18).Titled("How would you rate the report for number of errors, revisions?");
            grid.Columns.Add(model => model.QNo19).Titled("for Improvement");
            //grid.Columns.Add(model => model.QNo20).Titled("Further Exception");

            grid.Pager = new GridPager<customerFeedbackregister>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = Obj1.lstCustomerFeedback.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }



        //get excel Data//

        public List<customerFeedbackregister> GetData1(customerFeedbackregister U)
        {
            DataTable dataTable = new DataTable();
            List<customerFeedbackregister> lstFeedbackregister = new List<customerFeedbackregister>();
            dataTable = feedbackDashboardDAL.GetCustomerFeedback();
            List<customerFeedbackregister> DataList = new List<customerFeedbackregister>();

            DataTable List = new DataTable();
            // Session["DTComplaint"] = List;
            if (dataTable != null)
            {
                foreach (DataRow dr in dataTable.Rows)
                {

                    customerFeedbackregister Data = new customerFeedbackregister();

                    Data.RequestCreatedBy = Convert.ToString(dr["RequestCreatedBy"]);
                    Data.Branch_Name = Convert.ToString(dr["Branch_Name"]);
                    Data.RequestDate = Convert.ToString(dr["RequestDate"]);
                    Data.CompanyName = Convert.ToString(dr["CompanyName"]);
                    Data.JobNo = Convert.ToString(dr["JobNo"]);
                    Data.AddressOfOrg = Convert.ToString(dr["AddressOfOrg"]);
                    Data.ResponseRequiredDate = Convert.ToString(dr["ResponseRequiredDate"]);
                    Data.ResponseFilledDate = Convert.ToString(dr["ResponseFilledDate"]);
                    Data.NameOfOrg = Convert.ToString(dr["NameOfOrg"]);
                    Data.NameOfRespondant = Convert.ToString(dr["NameOfRespondant"]);
                    Data.DesignationOfRespondant = Convert.ToString(dr["DesignationOfRespondant"]);
                    Data.FeedbackRemark = Convert.ToString(dr["FeedbackRemarks"]);
                    Data.Emailid = Convert.ToString(dr["emailid"]);
                    Data.QNo1 = Convert.ToString(dr["How well did we respond to your enquiry? "]);
                    Data.QNo2 = Convert.ToString(dr["Did You receive the Quotation within Expected Time Frame?"]);
                    Data.QNo3 = Convert.ToString(dr["How well did we understand your requirements?"]);
                    Data.QNo4 = Convert.ToString(dr["Did the quotation contain all the required information? "]);
                    Data.QNo5 = Convert.ToString(dr["How quickly were your queries resolved & revised quotation submitted?"]);
                    Data.QNo6 = Convert.ToString(dr["Was your email / call responded on time?"]);
                    Data.QNo7 = Convert.ToString(dr["Was the call scheduled as per your request?"]);
                    Data.QNo8 = Convert.ToString(dr["Did you receive information regarding conformation of calls, inspector details in advance?"]);
                    Data.QNo9 = Convert.ToString(dr["In case of change in schedule were you informed within expected time frame?"]);
                    Data.QNo10 = Convert.ToString(dr["How satisfied are you with the communication process with the inspection coordination team?"]);
                    Data.QNo11 = Convert.ToString(dr["How would you rate the behaviour of TUV inspector with your staff (w.r.t. language, attitude)? "]);
                    Data.QNo12 = Convert.ToString(dr["How would you rate the inspector’s safety awareness & leadership in implementation of safety requirements?"]);
                    Data.QNo13 = Convert.ToString(dr["How would you rate the quality of inspection w.r.t. observations raised? "]);
                    Data.QNo14 = Convert.ToString(dr["How would you rate the efficiency of our inspector w.r.t time taken for inspection?"]);
                    Data.QNo15 = Convert.ToString(dr["How would you rate our inspectors for maintaining confidentiality & Integrity?"]);
                    Data.QNo16 = Convert.ToString(dr["Did you receive the inspection report/Release note in time?"]);
                    Data.QNo17 = Convert.ToString(dr["Did the report meet your expectations w.r.t. completeness, contents and conclusion"]);
                    Data.QNo18 = Convert.ToString(dr["How would you rate the report for number of errors, revisions?"]);
                    Data.QNo19 = Convert.ToString(dr["for Improvement"]);
                    //Data.QNo20 = Convert.ToString(dr["Further Exception"]);



                    DataList.Add(Data);
                }
            }

            Obj1.lstCustomerFeedback = DataList;
            return Obj1.lstCustomerFeedback;

        }


        [HttpGet]
        public JsonResult Getscore(string fromDate, string toDate)
        {
            DataSet ds = new DataSet();
            if (fromDate != null && fromDate != "" && toDate != null && toDate != "")
            {


                ds = feedbackDashboardDAL.GetData(fromDate, toDate);
            }
            else
            {
                ds = feedbackDashboardDAL.GetDashboard();
            }



            string json = JsonConvert.SerializeObject(ds.Tables[0]);
            return Json(json, JsonRequestBehavior.AllowGet);
        }



    }
}