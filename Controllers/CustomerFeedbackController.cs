using System.Drawing.Drawing2D;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;
using System.IO;
using System.IO.Packaging;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Utilities;
using NonFactors.Mvc.Grid;

namespace TuvVision.Controllers
{
    public class CustomerFeedbackController : Controller
    {


        DALCustomerFeedback OBJCust = new DALCustomerFeedback();
        CustomerFeedback UserCust = new CustomerFeedback();
        // GET: CustomerFeedback
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FeedBackDashBoard()
        {
            Session["GetExcelData"] = "Yes";
            DataTable DTFeedback = new DataTable();
            DTFeedback = OBJCust.GetFeedbackDashBoard();
            List<CustomerFeedback> lstFeedback = new List<CustomerFeedback>();
            try
            {
                if (DTFeedback.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTFeedback.Rows)
                    {
                        lstFeedback.Add(
                            new CustomerFeedback
                            {
                                Count = DTFeedback.Rows.Count,
                                FeedBack_ID = Convert.ToInt32(dr["FeedBack_ID"]),
                                Created_by = Convert.ToString(dr["Created_by"]),
                                Created_Date = Convert.ToString(dr["Created_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Name_Of_organisation = Convert.ToString(dr["Name_Of_organisation"]),
                                Name_Of_Respondent = Convert.ToString(dr["Name_Of_Respondent"]),
                                Desighnation_Of_Respondent = Convert.ToString(dr["Desighnation_Of_Respondent"]),
                                Order_No = Convert.ToString(dr["Order_No"]),
                                Enquiry_response = Convert.ToString(dr["Enquiry_response"]),
                                Quotation_Time_Frame_feedback = Convert.ToString(dr["Quotation_Time_Frame_feedback"]),
                                Requirement_Understanding = Convert.ToString(dr["Requirement_Understanding"]),
                                Quotation_information = Convert.ToString(dr["Quotation_information"]),
                                Quotation_Submit_Response = Convert.ToString(dr["Quotation_Submit_Response"]),
                                Email_call_ResponseTime = Convert.ToString(dr["Email_call_ResponseTime"]),
                                Requested_Call_schedule = Convert.ToString(dr["Requested_Call_schedule"]),
                                Confirmation_Reception = Convert.ToString(dr["Confirmation_Reception"]),
                                Change_in_schedule_Response = Convert.ToString(dr["Change_in_schedule_Response"]),
                                Behaiviour_of_Inspector = Convert.ToString(dr["Behaiviour_of_Inspector"]),
                                implementation_of_safety_requirements_Of_Inspector = Convert.ToString(dr["implementation_of_safety_requirements_Of_Inspector"]),
                                quality_of_inspection = Convert.ToString(dr["quality_of_inspection"]),
                                efficiency_with_time = Convert.ToString(dr["efficiency_with_time"]),
                                Maintanance_Of_confidentiality_and_Integrity = Convert.ToString(dr["Maintanance_Of_confidentiality_and_Integrity"]),
                                inspection_report_or_Releasenote_Time = Convert.ToString(dr["inspection_report_or_Releasenote_Time"]),
                                Expectation_Meet = Convert.ToString(dr["Expectation_Meet"]),
                                report_for_number_of_errors = Convert.ToString(dr["report_for_number_of_errors"]),
                                association_with_TUV_India = Convert.ToString(dr["association_with_TUV_India"]),
                                Suggestions = Convert.ToString(dr["Suggestions"]),
                                Score_Achieved = Convert.ToString(dr["Score_Achieved"]),
                                Score_percentage=Convert.ToString(dr["Score_percentage"]),
                                Client_Location=Convert.ToString(dr["Client_Location"]),
                                Communication_satisfaction = Convert.ToString(dr["Communication_satisfaction"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["FeedBackList"] = lstFeedback;

            UserCust.lstFeedback1 = lstFeedback;
            return View(UserCust);
        }

        [HttpPost]
        public ActionResult FeedBackDashBoard(CustomerFeedback  c)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = c.FromDate;
            Session["ToDate"] = c.ToDate;
            DataTable DTFeedback = new DataTable();
            DTFeedback = OBJCust.GetFeedbackDashBoardByDate(c);
            List<CustomerFeedback> lstFeedback = new List<CustomerFeedback>();
            try
            {
                if (DTFeedback.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTFeedback.Rows)
                    {
                        lstFeedback.Add(
                            new CustomerFeedback
                            {
                                Count = DTFeedback.Rows.Count,
                                FeedBack_ID = Convert.ToInt32(dr["FeedBack_ID"]),
                                Created_by = Convert.ToString(dr["Created_by"]),
                                Created_Date = Convert.ToString(dr["Created_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Name_Of_organisation = Convert.ToString(dr["Name_Of_organisation"]),
                                Name_Of_Respondent = Convert.ToString(dr["Name_Of_Respondent"]),
                                Desighnation_Of_Respondent = Convert.ToString(dr["Desighnation_Of_Respondent"]),
                                Order_No = Convert.ToString(dr["Order_No"]),
                                Enquiry_response = Convert.ToString(dr["Enquiry_response"]),
                                Quotation_Time_Frame_feedback = Convert.ToString(dr["Quotation_Time_Frame_feedback"]),
                                Requirement_Understanding = Convert.ToString(dr["Requirement_Understanding"]),
                                Quotation_information = Convert.ToString(dr["Quotation_information"]),
                                Quotation_Submit_Response = Convert.ToString(dr["Quotation_Submit_Response"]),
                                Email_call_ResponseTime = Convert.ToString(dr["Email_call_ResponseTime"]),
                                Requested_Call_schedule = Convert.ToString(dr["Requested_Call_schedule"]),
                                Confirmation_Reception = Convert.ToString(dr["Confirmation_Reception"]),
                                Change_in_schedule_Response = Convert.ToString(dr["Change_in_schedule_Response"]),
                                Behaiviour_of_Inspector = Convert.ToString(dr["Behaiviour_of_Inspector"]),
                                implementation_of_safety_requirements_Of_Inspector = Convert.ToString(dr["implementation_of_safety_requirements_Of_Inspector"]),
                                quality_of_inspection = Convert.ToString(dr["quality_of_inspection"]),
                                efficiency_with_time = Convert.ToString(dr["efficiency_with_time"]),
                                Maintanance_Of_confidentiality_and_Integrity = Convert.ToString(dr["Maintanance_Of_confidentiality_and_Integrity"]),
                                inspection_report_or_Releasenote_Time = Convert.ToString(dr["inspection_report_or_Releasenote_Time"]),
                                Expectation_Meet = Convert.ToString(dr["Expectation_Meet"]),
                                report_for_number_of_errors = Convert.ToString(dr["report_for_number_of_errors"]),
                                association_with_TUV_India = Convert.ToString(dr["association_with_TUV_India"]),
                                Suggestions = Convert.ToString(dr["Suggestions"]),
                                Score_Achieved = Convert.ToString(dr["Score_Achieved"]),
                                Score_percentage = Convert.ToString(dr["Score_percentage"]),
                                Client_Location = Convert.ToString(dr["Client_Location"]),
                                Communication_satisfaction = Convert.ToString(dr["Communication_satisfaction"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["FeedBackList"] = lstFeedback;
            UserCust.lstFeedback1 = lstFeedback;
            return View(UserCust);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        [HttpGet]
        public ActionResult CreateFeedback()
        {
            return View();
        }

      [HttpPost]
        public ActionResult CreateFeedback(CustomerFeedback Cust, HttpPostedFileBase FileUpload)
        {
            string Result = string.Empty;
            var data = AuthenticationManager.User.Identity;
            var UID = data.GetUserId();


            #region Rahul Code By 03-May-2019
            HttpPostedFileBase files = FileUpload;

            if (files.ContentLength > 0 && !string.IsNullOrEmpty(files.FileName) && files.FileName.Contains(".xlsx"))
            {

                //if(files.FileName == "ExcellData.xlsx")
                //{


                try
                {

                    string fileName = files.FileName;
                    string fileContentType = files.ContentType;
                    byte[] fileBytes = new byte[files.ContentLength];
                    var data1 = files.InputStream.Read(fileBytes, 0, Convert.ToInt32(files.ContentLength));
                    var package = new ExcelPackage(files.InputStream);

                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    int noOfCol = workSheet.Dimension.End.Column;
                    int noOfRow = workSheet.Dimension.End.Row;
                    int rowIterator = 1;
                    //if (/*"Timestamp" == Convert.ToString(workSheet.Cells[rowIterator, 1].Value)*/
                    //    "Timestamp" == Convert.ToString(workSheet.Cells[rowIterator, 2].Value)
                    //    //&& "Status" == Convert.ToString(workSheet.Cells[rowIterator, 3].Value)
                    //       && "Name of your Organisation? *" == Convert.ToString(workSheet.Cells[rowIterator, 3].Value)
                    //          && "Name of your Respondent ? *" == Convert.ToString(workSheet.Cells[rowIterator, 4].Value)
                    //             && "Designation of the Respondent? *" == Convert.ToString(workSheet.Cells[rowIterator, 5].Value)
                    //            && "Order No" == Convert.ToString(workSheet.Cells[rowIterator, 6].Value)
                    //             && "Enquiry response" == Convert.ToString(workSheet.Cells[rowIterator, 7].Value)
                    //            && "Quotation Time Frame feedback" == Convert.ToString(workSheet.Cells[rowIterator, 8].Value)
                    //            && "Requirement Understanding" == Convert.ToString(workSheet.Cells[rowIterator, 9].Value)
                    //           && "Quotation information" == Convert.ToString(workSheet.Cells[rowIterator, 10].Value)
                    //           && "Quotation Submit Response" == Convert.ToString(workSheet.Cells[rowIterator, 11].Value)
                    //             && "Email call ResponseTime" == Convert.ToString(workSheet.Cells[rowIterator, 12].Value)
                    //           && "Requested Call schedule" == Convert.ToString(workSheet.Cells[rowIterator, 13].Value)
                    //           && "Confirmation Reception" == Convert.ToString(workSheet.Cells[rowIterator, 14].Value)
                    //             && "Change in schedule Response" == Convert.ToString(workSheet.Cells[rowIterator, 15].Value)
                    //              && "Communication satisfaction" == Convert.ToString(workSheet.Cells[rowIterator, 16].Value)
                    //              && "Behaiviour of Inspector" == Convert.ToString(workSheet.Cells[rowIterator, 17].Value)
                    //             && "implementation of safety requirements Of Inspector" == Convert.ToString(workSheet.Cells[rowIterator, 18].Value)
                    //            && "quality of inspection" == Convert.ToString(workSheet.Cells[rowIterator, 19].Value)
                    //            && "efficiency with time" == Convert.ToString(workSheet.Cells[rowIterator, 20].Value)
                    //            && "Maintanance Of confidentiality and Integrity" == Convert.ToString(workSheet.Cells[rowIterator, 21].Value)
                    //         && "inspection report or Releasenote Time" == Convert.ToString(workSheet.Cells[rowIterator, 22].Value)
                    //         && "Expectation Meet" == Convert.ToString(workSheet.Cells[rowIterator, 23].Value)
                    //            && "report for number of errors" == Convert.ToString(workSheet.Cells[rowIterator, 24].Value)
                    //           && "association with TUV India" == Convert.ToString(workSheet.Cells[rowIterator, 25].Value)
                    //          && "Suggestions" == Convert.ToString(workSheet.Cells[rowIterator, 26].Value)
                    //    )
                    //{

                    //}
                    //else
                    //{

                    //}
                    for (rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {

                        // string Date = Convert.ToString(workSheet.Cells[rowIterator, 1].Value);
                        //double d = double.Parse(Date);
                        //DateTime conv = DateTime.FromOADate(d);
                        string  Date1 = Convert.ToString(workSheet.Cells[rowIterator, 1].Value);
                       
                        // string Created_Date =Convert.ToString(workSheet.Cells[rowIterator, 1].Value);
                        //string Status = Convert.ToString(workSheet.Cells[rowIterator, 3].Value);
                        string Name_Of_organisation = Convert.ToString(workSheet.Cells[rowIterator, 2].Value);
                        string Name_Of_Respondent = Convert.ToString(workSheet.Cells[rowIterator, 3].Value);
                        string Desighnation_Of_Respondent = Convert.ToString(workSheet.Cells[rowIterator, 4].Value);
                        string Order_No = Convert.ToString(workSheet.Cells[rowIterator, 5].Value);
                        string ClientLocation = Convert.ToString(workSheet.Cells[rowIterator, 6].Value);
                        string Enquiry_response = Convert.ToString(workSheet.Cells[rowIterator, 7].Value);
                        string Quotation_Time_Frame_feedback = Convert.ToString(workSheet.Cells[rowIterator, 8].Value);
                        string Requirement_Understanding = Convert.ToString(workSheet.Cells[rowIterator, 9].Value);
                        string Quotation_information = Convert.ToString(workSheet.Cells[rowIterator, 10].Value);
                        string Quotation_Submit_Response = Convert.ToString(workSheet.Cells[rowIterator, 11].Value);
                        string Email_call_ResponseTime = Convert.ToString(workSheet.Cells[rowIterator, 12].Value);
                        string Requested_Call_schedule = Convert.ToString(workSheet.Cells[rowIterator, 13].Value);
                        string Confirmation_Reception = Convert.ToString(workSheet.Cells[rowIterator, 14].Value);
                        string Change_in_schedule_Response = Convert.ToString(workSheet.Cells[rowIterator, 15].Value);
                        string Communication_satisfaction = Convert.ToString(workSheet.Cells[rowIterator, 16].Value);
                        string Behaiviour_of_Inspector = Convert.ToString(workSheet.Cells[rowIterator, 17].Value);
                        string implementation_of_safety_requirements_Of_Inspector = Convert.ToString(workSheet.Cells[rowIterator, 18].Value);
                        string quality_of_inspection = Convert.ToString(workSheet.Cells[rowIterator, 19].Value);
                        string efficiency_with_time = Convert.ToString(workSheet.Cells[rowIterator, 20].Value);
                        string Maintanance_Of_confidentiality_and_Integrity = Convert.ToString(workSheet.Cells[rowIterator, 21].Value);
                        string inspection_report_or_Releasenote_Time = Convert.ToString(workSheet.Cells[rowIterator, 22].Value);
                        string Expectation_Meet = Convert.ToString(workSheet.Cells[rowIterator, 23].Value);
                        string report_for_number_of_errors = Convert.ToString(workSheet.Cells[rowIterator, 24].Value);
                        string association_with_TUV_India = Convert.ToString(workSheet.Cells[rowIterator, 25].Value);
                        string Suggestions = Convert.ToString(workSheet.Cells[rowIterator, 26].Value);
                        string Score_Achieved = Convert.ToString(workSheet.Cells[rowIterator, 27].Value);
                        string Score_percentage = Convert.ToString(workSheet.Cells[rowIterator, 28].Value);
                        try
                        {
                            //throw new System.ArgumentException();
                            DateTime Currentdate=Convert.ToDateTime(Date1);
                            string CrDate = Currentdate.ToString("dd/MM/yyyy");          
                            Cust.Created_Date =CrDate;
                            //Cust.Status = Status;
                            Cust.Name_Of_organisation = Name_Of_organisation;
                                Cust.Name_Of_Respondent = Name_Of_Respondent;
                                Cust.Desighnation_Of_Respondent = Desighnation_Of_Respondent;
                                Cust.Order_No = Order_No;
                            Cust.Client_Location = ClientLocation;
                                Cust.Enquiry_response = Enquiry_response;
                                Cust.Quotation_Time_Frame_feedback = Quotation_Time_Frame_feedback;
                                Cust.Requirement_Understanding = Requirement_Understanding;
                                Cust.Quotation_information = Quotation_information;
                                Cust.Quotation_Submit_Response = Quotation_Submit_Response;
                                Cust.Email_call_ResponseTime = Email_call_ResponseTime;
                                Cust.Requested_Call_schedule = Requested_Call_schedule;
                                Cust.Confirmation_Reception = Confirmation_Reception;
                                Cust.Change_in_schedule_Response = Change_in_schedule_Response;
                                Cust.Communication_satisfaction = Communication_satisfaction;
                                Cust.Behaiviour_of_Inspector = Behaiviour_of_Inspector;
                                Cust.implementation_of_safety_requirements_Of_Inspector = implementation_of_safety_requirements_Of_Inspector;
                                Cust.quality_of_inspection = quality_of_inspection;
                                Cust.efficiency_with_time = efficiency_with_time;
                                Cust.Maintanance_Of_confidentiality_and_Integrity = Maintanance_Of_confidentiality_and_Integrity;
                                Cust.inspection_report_or_Releasenote_Time = inspection_report_or_Releasenote_Time;
                                Cust.Expectation_Meet = Expectation_Meet;
                                Cust.report_for_number_of_errors = report_for_number_of_errors;
                                Cust.association_with_TUV_India = association_with_TUV_India;
                                Cust.Suggestions = Suggestions;
                                 Cust.Score_Achieved = Score_Achieved;
                                Cust.Score_percentage = Score_percentage;
                            if (Name_Of_Respondent != null || Name_Of_Respondent != "")
                            {
                                Result = OBJCust.InsertFeedBack(Cust);
                                if (Result != "" && Result != null)
                                {
                                    TempData["UpdateFeedBack"] = Result;

                                }
                            }
                               
                            }
                        catch (Exception ex)
                        {
                            
                            string error = ex.Message.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                    return RedirectToAction("FeedBackDashBoard");
                }


            }

            return RedirectToAction("FeedBackDashBoard");
        }
        #endregion


        #region
        [HttpGet]
        public ActionResult ExportIndex(CustomerFeedback c)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CustomerFeedback> grid = CreateExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<CustomerFeedback> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<CustomerFeedback> CreateExportableGrid(CustomerFeedback c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CustomerFeedback> grid = new Grid<CustomerFeedback>(GetData(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.Created_by).Titled("Created by");
            grid.Columns.Add(model => model.Created_Date).Titled("Timestamp");
            grid.Columns.Add(model => model.Score_Achieved).Titled("Email");
            grid.Columns.Add(model => model.Name_Of_organisation).Titled("Name of your Organization? *");
            grid.Columns.Add(model => model.Name_Of_Respondent).Titled("Name of your Respondent ? *");
            grid.Columns.Add(model => model.Desighnation_Of_Respondent).Titled("Designation of the Respondent? *");
            grid.Columns.Add(model => model.Order_No).Titled("Feedback against which Order / Project / Item / Vendor / Equipment / P.O./ TUV control No.?");
            grid.Columns.Add(model => model.Client_Location).Titled("Client Location");
            grid.Columns.Add(model => model.Enquiry_response).Titled("How well did we respond to your enquiry? *");
            grid.Columns.Add(model => model.Quotation_Time_Frame_feedback).Titled("Did You receive the Quotation within Expected Time Frame? *ENQUIRY HANDLING");
            grid.Columns.Add(model => model.Requirement_Understanding).Titled("How well did we understand your requirements? *");
            grid.Columns.Add(model => model.Quotation_information).Titled("Did the quotation contain all the required information?");
            grid.Columns.Add(model => model.Quotation_Submit_Response).Titled("How quickly were your queries resolved & revised quotation submitted? ");
            grid.Columns.Add(model => model.Email_call_ResponseTime).Titled("Was your email / call responded on time?");
            grid.Columns.Add(model => model.Requested_Call_schedule).Titled("Was the call scheduled as per your request? *");
            grid.Columns.Add(model => model.Confirmation_Reception).Titled("Did you receive information regarding confirmation of calls, inspector details in advance? *");
            grid.Columns.Add(model => model.Change_in_schedule_Response).Titled("In case of change in schedule were you informed within expected time frame? *");
            grid.Columns.Add(model => model.Communication_satisfaction).Titled("How satisfied are you with the communication process with the inspection coordination team? *");
            grid.Columns.Add(model => model.Behaiviour_of_Inspector).Titled("How would you rate the behavior of TUV inspector with your staff (w.r.t. language, attitude)? *");
            grid.Columns.Add(model => model.implementation_of_safety_requirements_Of_Inspector).Titled("How would you rate the inspector’s safety awareness & leadership in implementation of safety requirements? *");
            grid.Columns.Add(model => model.quality_of_inspection).Titled("How would you rate the quality of inspection w.r.t. observations raised? *");
            grid.Columns.Add(model => model.efficiency_with_time).Titled("How would you rate the efficiency of our inspector w.r.t time taken for inspection? *");
            grid.Columns.Add(model => model.Maintanance_Of_confidentiality_and_Integrity).Titled("How would you rate our inspectors for maintaining confidentiality & Integrity? *");
            grid.Columns.Add(model => model.inspection_report_or_Releasenote_Time).Titled("Did you receive the inspection report/Release note in time? *");
            grid.Columns.Add(model => model.Expectation_Meet).Titled("Did the report meet your expectations w.r.t. completeness, contents and conclusion *");
            grid.Columns.Add(model => model.report_for_number_of_errors).Titled("How would you rate the report for number of errors, revisions? *");
            grid.Columns.Add(model => model.association_with_TUV_India).Titled("How likely would you continue your association with TUV India? *");
            grid.Columns.Add(model => model.Suggestions).Titled("Would you like to give us any suggestion to improve our service ");
            


            grid.Pager = new GridPager<CustomerFeedback>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = UserCust.lstFeedback1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<CustomerFeedback> GetData(CustomerFeedback c)
        {

            DataTable DTFeedback = new DataTable();
            

            if (Session["GetExcelData"] == "Yes")
            {
                DTFeedback = OBJCust.GetFeedbackDashBoard();
            }
            else
            {

                c.FromDate = Session["FromDate"].ToString();
                c.ToDate = Session["ToDate"].ToString();
                DTFeedback = OBJCust.GetFeedbackDashBoardByDate(c);
            }

            List<CustomerFeedback> lstFeedback = new List<CustomerFeedback>();
            try
            {
                if (DTFeedback.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTFeedback.Rows)
                    {
                        lstFeedback.Add(
                            new CustomerFeedback
                            {
                                FeedBack_ID = Convert.ToInt32(dr["FeedBack_ID"]),
                                Created_by = Convert.ToString(dr["Created_by"]),
                                Created_Date = Convert.ToString(dr["Created_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Name_Of_organisation = Convert.ToString(dr["Name_Of_organisation"]),
                                Name_Of_Respondent = Convert.ToString(dr["Name_Of_Respondent"]),
                                Desighnation_Of_Respondent = Convert.ToString(dr["Desighnation_Of_Respondent"]),
                                Order_No = Convert.ToString(dr["Order_No"]),
                                Enquiry_response = Convert.ToString(dr["Enquiry_response"]),
                                Quotation_Time_Frame_feedback = Convert.ToString(dr["Quotation_Time_Frame_feedback"]),
                                Requirement_Understanding = Convert.ToString(dr["Requirement_Understanding"]),
                                Quotation_information = Convert.ToString(dr["Quotation_information"]),
                                Quotation_Submit_Response = Convert.ToString(dr["Quotation_Submit_Response"]),
                                Email_call_ResponseTime = Convert.ToString(dr["Email_call_ResponseTime"]),
                                Requested_Call_schedule = Convert.ToString(dr["Requested_Call_schedule"]),
                                Confirmation_Reception = Convert.ToString(dr["Confirmation_Reception"]),
                                Change_in_schedule_Response = Convert.ToString(dr["Change_in_schedule_Response"]),
                                Behaiviour_of_Inspector = Convert.ToString(dr["Behaiviour_of_Inspector"]),
                                implementation_of_safety_requirements_Of_Inspector = Convert.ToString(dr["implementation_of_safety_requirements_Of_Inspector"]),
                                quality_of_inspection = Convert.ToString(dr["quality_of_inspection"]),
                                efficiency_with_time = Convert.ToString(dr["efficiency_with_time"]),
                                Maintanance_Of_confidentiality_and_Integrity = Convert.ToString(dr["Maintanance_Of_confidentiality_and_Integrity"]),
                                inspection_report_or_Releasenote_Time = Convert.ToString(dr["inspection_report_or_Releasenote_Time"]),
                                Expectation_Meet = Convert.ToString(dr["Expectation_Meet"]),
                                report_for_number_of_errors = Convert.ToString(dr["report_for_number_of_errors"]),
                                association_with_TUV_India = Convert.ToString(dr["association_with_TUV_India"]),
                                Suggestions = Convert.ToString(dr["Suggestions"]),
                                Score_Achieved = Convert.ToString(dr["Score_Achieved"]),
                                Score_percentage = Convert.ToString(dr["Score_percentage"]),
                                Client_Location = Convert.ToString(dr["Client_Location"]),
                                Communication_satisfaction = Convert.ToString(dr["Communication_satisfaction"]),

                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["FeedBackList"] = lstFeedback;

            UserCust.lstFeedback1 = lstFeedback;
            return UserCust.lstFeedback1;
        }

        #endregion
        //*************************************** Delete Feedback - Code by Ankush ******************************************
        public ActionResult DeleteFeedback(int? bid)
        {
            try
            {
                if (OBJCust.DeleteFeedback(Convert.ToInt32(bid)))
                {
                    TempData["Deleted"] = "Feedback Details Deleted Successfully ...";

                }
                return RedirectToAction("FeedBackDashBoard");
            }
            catch (Exception)
            {
                return View();
            }
        }

    }
}



