using Newtonsoft.Json;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class GenerateVoucherController : Controller
    {
        DALExpenseItem objEI = new DALExpenseItem();
        DALExpenseVoucher objEV = new DALExpenseVoucher();
        ExpenseItem objEIModel = new ExpenseItem();

        // GET: GenerateVoucher
        public ActionResult GenerateVoucher()
        {

            return View();
        }
        public ActionResult GenerateVoucher_UserHistory(string VoucherNo, string IsApproval,string Datedidd)
        {

            ViewBag.VoucherNo = VoucherNo;
            ViewBag.IsApproval = IsApproval;
            ViewBag.PreviousUrl = Request.UrlReferrer?.ToString(); // Store the previous URL in ViewBag
            ViewBag.datediff = Datedidd;

            return View();
        }
        //[HttpGet]
        //public JsonResult GetVoucherList()
        //{
        //    string Role = Session["RoleName"].ToString();

        //    string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        //    DataSet ds = objEI.GetVoucherList(UserId, Role);
        //    //string json = JsonConvert.SerializeObject(ds.Tables[0]);
        //    string json = "";
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        json = JsonConvert.SerializeObject(ds.Tables[0]);
        //    }
        //    else
        //    {
        //        json = JsonConvert.SerializeObject("Error");
        //    }
        //    return Json(json, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public JsonResult GetVoucherList()
        {
            string Role = Session["RoleName"].ToString();

            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

            DataSet ds = new DataSet();

            //ds = objEI.CheckUser(UserId, Role);

            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    ds = ds = objEI.GetVoucher(UserId, Role);
            //}
            //else if (Role == "Accounts")
            //{
            //    ds = ds = objEI.GetVoucherAccounts(UserId, Role);

            //}
            //else
            //{
            //    ds = objEI.GetVoucherList(UserId, Role);
            //}
            ds = objEI.Getuservoucherlist(UserId, Role);

            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVoucherListApproval()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetVoucherList_Approval()
        {
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.GetVoucherList_Approved(UserId, Role);
            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VoucherHistory(string VoucherNo, string IsApproval)
        {
            ViewBag.VoucherNo = VoucherNo;
            ViewBag.IsApproval = IsApproval;
            ViewBag.PreviousUrl = Request.UrlReferrer?.ToString(); // Store the previous URL in ViewBag

            return View();
        }



        [HttpGet]
        public JsonResult Voucher_History(string VoucherNo, string IsApproval)
        {
            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.Get_VoucherDetails_History_ID(VoucherNo, IsApproval);
            Session["VoucherNo"] = VoucherNo;
            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }



        //added by nikita on 21112023

        [HttpGet]
        public JsonResult Voucher_History_Description(string VoucherNo)
        {
            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.Get_VoucherDetails_History_ID_Description(VoucherNo);
            Session["VoucherNo"] = VoucherNo;
            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SendForApproval(string VoucherNo)
        {
            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.SendForApproval(VoucherNo);
            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveVoucher(List<ExpenseItem> input, string ApprovedAmounttotal)
        {
            //  List<ExpenseItem> expenseItems = JsonConvert.DeserializeObject<List<ExpenseItem>>(input);
            ExpenseItem QM = new ExpenseItem();
            DataSet ds = new DataSet();
            if (input != null)
            {
                foreach (var d in input)
                {
                    QM.VoucherNo = d.VoucherNo;
                    QM.ApprovedAmount = d.ApprovedAmount;
                    QM.TotalAmount  = d.TotalAmount;
                    QM.Remarks = d.Remarks;
                    QM.PKExpenseId = d.PKExpenseId;
                    //QM.ApprovedAmount_V = d.ApprovedAmount;

                    ds = objEI.Approval(QM, ApprovedAmounttotal);
                }
            }
            
            //DataSet ds = objEI.Approval(expenseItems[0]);

            //string json = JsonConvert.SerializeObject(ds.Tables[0]);

            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        [HttpPost]
        public JsonResult BulkApproveVoucher(string Data1)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(Data1);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            string Result = string.Empty;

            /// DataSet ds1 = objEI.BulkApproval(ds);
            Result = objEI.BulkApproval(ds);

            string json = "";
           // if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)

            if (Result != "-1" || Result != string.Empty)
            {
             //    json = JsonConvert.SerializeObject(ds1.Tables[0]);
                json = "Success";
            }
            else
            {
              //  json = JsonConvert.SerializeObject("Error");
                json = "Error";
            }
            //List<ExpenseItem> expenseItems = JsonConvert.DeserializeObject<List<ExpenseItem>>(data);

            //DataSet ds = objEI.Approval(expenseItems[0]);

            //string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

            ////string json = JsonConvert.SerializeObject(ds.Tables[0]);
            //string json = "";
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    json = JsonConvert.SerializeObject(ds.Tables[0]);
            //}
            //else
            //{
            //    json = JsonConvert.SerializeObject("Error");
            //}
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GenerateVoucherApproval()
        //{

        //    string Role = Session["RoleName"].ToString();

        //    string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        //    DataSet ds = objEI.GetVoucherList(UserId, Role); //commented on 23112023
        //   // DataSet ds = objEI.GetVoucherList_Approval(UserId, Role);

        //    List<ExpenseItem> lstComplaintDashBoard1 = new List<ExpenseItem>();

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {


        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {


        //            lstComplaintDashBoard1.Add(new ExpenseItem
        //            {
        //                Name = Convert.ToString(dr["Name"]),
        //                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
        //                EmployementCategory = Convert.ToString(dr["EmployementCategory"]),
        //                PK_UserID = Convert.ToString(dr["PK_UserID"]),
        //                VoucherNo = Convert.ToString(dr["VoucherNo"]),
        //                TotalAmount = Convert.ToDouble(dr["TotalAmount"]),
        //                ApprovedAmount = Convert.ToDouble(dr["ApprovedAmount"]),
        //                DaysPresent = Convert.ToString(dr["DaysPresent"]),
        //                Status = Convert.ToString(dr["Status"]),
        //                IsVisibleCheckBox = Convert.ToString(dr["IsVisibleCheckBox"]),
        //                IsSendForApproval = Convert.ToString(dr["IsSendForApproval"]),
        //                Month_Name = Convert.ToString(dr["Month_Name"]), //added by nikita on 21012024
        //                // PKExpenseId = Convert.ToInt32(dr["PKExpenseId"]),
        //            });
        //        }
        //    }

        //    ViewData["ListData"] = lstComplaintDashBoard1;
        //    objEIModel.lst1 = lstComplaintDashBoard1;


        //    return View(objEIModel);
        //}
        //

        //added by nikita on 27032024

        public ActionResult GenerateVoucherApproval()
        {

            return View();
        }

        [HttpGet]
        public ActionResult GenerateVoucherApprovalList()
        {

            string Role = Session["RoleName"].ToString();

            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.GetVoucherList(UserId, Role); //commented on 23112023
                                                             // DataSet ds = objEI.GetVoucherList_Approval(UserId, Role);


            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);

        }
         //added by nikita on 29072024
        [HttpPost]
        public ActionResult SaveAttachment(string VoucherNo, string file)
        {
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listExpenseUploadedFile"] as List<FileDetails>;
            string result;
            if (lstFileDtls != null && lstFileDtls.Count > 0)
            {
                result = objEI.InsertFileAttachment_(lstFileDtls, VoucherNo);
                Session["listExpenseUploadedFile"] = null;
            }
            else if (lstFileDtls == null || lstFileDtls.Count == 0)
            {
                result = objEI.DeleteUploadedFile_(VoucherNo);
                Session["listExpenseUploadedFile"] = null;
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAttachment(string VoucherNo)
        {
            try
            {
                DataTable DTGetUploadedFile = objEI.EditUploadedFile_(VoucherNo);
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();

                if (DTGetUploadedFile.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTGetUploadedFile.Rows)
                    {
                        lstEditFileDetails.Add(new FileDetails
                        {
                            PK_ID = Convert.ToInt32(dr["PK_ID"]),
                            FileName = Convert.ToString(dr["FileName"]),
                            Extension = Convert.ToString(dr["Extenstion"]),
                            IDS = Convert.ToString(dr["FileID"]),
                        });
                    }
                }

                return Json(lstEditFileDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteAttachment(int fileid)
        {
            string Result = string.Empty;
            try
            {
                Result = objEI.Delete_(Convert.ToInt32(fileid));
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ApprovedList()
        {

            return View();
        }

        [HttpGet]
        public JsonResult GetVoucherApprovalList(string data)
        {
            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.GetVoucherApprovalList(UserId);
            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        //
        [HttpGet]
        public JsonResult ExportToExcel1(string VoucherNo)
        {
            try
            {
                DataSet ds1 = new DataSet();

                ds1 = objEV.VoucherData(VoucherNo);
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Sheet1");

                    sheet.Cells["A1"].Value = "TUV India Private Ltd.";
                    sheet.Cells["A3"].Value = "Expenses Report:";
                    sheet.Cells["C3"].Value = "Month:";
                    //sheet.Cells["D3"].Value = Session["Month"].ToString();
                    sheet.Cells["E3"].Value = "Year:";
                    //sheet.Cells["f3"].Value = Session["Year"].ToString();
                    sheet.Cells["G3"].Value = "Submission Date:";
                    //sheet.Cells["H3"].Value = Session["ModifiedDate"].ToString();
                    sheet.Cells["A5"].Value = "Expenses Voucher Number:";
                    sheet.Cells["A7"].Value = "Branch:";
                    sheet.Cells["A8"].Value = "Name of Employee:";
                    sheet.Cells["A9"].Value = "HR Employee Code:";
                    sheet.Cells["A10"].Value = "SAP EMp Code:";
                    sheet.Cells["A11"].Value = "Cost Centre:";
                    //sheet.Cells["B11"].Value = Session["CostCenter"].ToString();
                    sheet.Cells["E5"].Value = "printed on:";
                    sheet.Cells["A12"].Value = "Current Residing Address:";
                    sheet.Cells["A14"].Value = "Details:-";

                    // Generate the file stream
                    MemoryStream stream = new MemoryStream();
                    package.SaveAs(stream);

                    // Set the response headers for the file download
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=ExportedData.xlsx");
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();

                    //File(package.GetAsByteArray(), "application/unknown", "Approved Voucher" + DateTime.Now.ToShortDateString() + ".xlsx");

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }



        public FileResult ExportToExcel(string VoucherNo)
        {
            DataSet ds = objEV.VoucherData(VoucherNo);
            DataTable dataTable = ds.Tables[0]; // Replace this with your own DataTable retrieval logic

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Set static cell values
                worksheet.Cells["A1"].Value = "Report Title";
                worksheet.Cells["A2"].Value = "Date: " + DateTime.Now.ToString("yyyy-MM-dd");

                // Set the header row
                int rowIndex = 4;
                foreach (DataColumn column in dataTable.Columns)
                {
                    worksheet.Cells[rowIndex, column.Ordinal + 1].Value = column.ColumnName;
                }

                // Set the data rows
                rowIndex++;
                foreach (DataRow row in dataTable.Rows)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cells[rowIndex, i + 1].Value = row[i];
                    }
                    rowIndex++;
                }

                // Auto-fit the columns for better readability
                worksheet.Cells.AutoFitColumns();

                // Generate the file stream
                MemoryStream stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Return the file for download
                string fileName = "ExportedData.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        //added by shrutika salve 12/06/2023

        public ActionResult ExportIndex(ExpenseItem c,string VoucherNo)
        {
            //string VoucherNo = Session["VoucherNo"].ToString();

            if (VoucherNo != "" || VoucherNo != null)
            {
                // Using EPPlus from nuget
                using (ExcelPackage package = new ExcelPackage())
                {
                    Int32 row = 15;
                    Int32 col = 1;

                    package.Workbook.Worksheets.Add("Data");
                    IGrid<ExpenseItem> grid = CreateExportableGrid(c, VoucherNo);
                    List<ExpenseItem> Datavalue = (List<ExpenseItem>)Session["DataList"];

                    double a = 0;

                    foreach (var item in Datavalue)
                    {
                        if (item.ExpenseType == "Own Bike" || item.ExpenseType == "Own Car")
                        {
                            a += Convert.ToDouble(item.INRAmount1);
                        }
                    }
                    ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];
                    sheet.Cells["A1"].Value = "TUV India Private Ltd.";
                    var cell = sheet.Cells["A1"];
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    cell.Style.Font.Size = 14; // Replace 12 with your desired font size
                    cell.Style.Font.Bold = true;
                    sheet.Cells["A3"].Value = "Expenses Report:";
                    var cell3 = sheet.Cells["A3"];
                    // Replace 12 with your desired font size
                    cell3.Style.Font.Bold = true;
                    //sheet.Cells["B3"].Value = 
                    sheet.Cells["C3"].Value = "Month:";
                    var cell4 = sheet.Cells["C3"];
                    cell4.Style.Font.Bold = true;
                    sheet.Cells["D3"].Value = Session["Month"].ToString();
                    sheet.Cells["E3"].Value = "Year:";
                    var cell5 = sheet.Cells["E3"];
                    cell5.Style.Font.Bold = true;//Session["fullName"].ToString();
                    sheet.Cells["f3"].Value = Session["Year"].ToString();
                    sheet.Cells["G3"].Value = "Submission Date:";
                    var cell6 = sheet.Cells["G3"];
                    cell6.Style.Font.Bold = true;
                    sheet.Cells["H3"].Value = Session["ModifiedDate"].ToString();

                    //sheet.Cells["H3"].Value= ;

                    sheet.Cells["A5"].Value = "Expenses Voucher Number:";
                    var cell2 = sheet.Cells["A5"];
                    cell2.Style.Font.Size = 13; // Replace 12 with your desired font size
                    cell2.Style.Font.Bold = true;
                    //sheet.Cells["B5"].Value = Session["VoucherNo"].ToString();
                    sheet.Cells["B5"].Value = VoucherNo;
                    sheet.Cells["A7"].Value = "Branch:";
                    var cell7 = sheet.Cells["A7"];
                    cell7.Style.Font.Bold = true;
                    sheet.Cells["B7"].Value = Session["Branch"].ToString();
                    sheet.Cells["A8"].Value = "Name of Employee:";
                    var cell8 = sheet.Cells["A8"];
                    cell8.Style.Font.Bold = true;
                    sheet.Cells["B8"].Value = Session["fullName"].ToString();

                    sheet.Cells["A9"].Value = "HR Employee Code:";
                    var cell9 = sheet.Cells["A9"];
                    cell9.Style.Font.Bold = true;
                    sheet.Cells["B9"].Value = Session["EmpCode"].ToString();
                    sheet.Cells["A10"].Value = "SAP EMp Code:";
                    var cell10 = sheet.Cells["A10"];
                    cell10.Style.Font.Bold = true;
                    sheet.Cells["B10"].Value = Session["SAPNo"].ToString();
                    sheet.Cells["A11"].Value = "Cost Centre:";
                    var cell11 = sheet.Cells["A11"];
                    cell11.Style.Font.Bold = true;
                    sheet.Cells["B11"].Value = Session["CostCenter"].ToString();
                    sheet.Cells["E5"].Value = "printed on:";
                    var cell12 = sheet.Cells["E5"];
                    cell12.Style.Font.Bold = true;
                    sheet.Cells["F5"].Value = DateTime.Now.ToShortDateString();
                    sheet.Cells["A12"].Value = "Current Residing Address:";
                    var cell13 = sheet.Cells["A12"];
                    cell13.Style.Font.Bold = true;
                    sheet.Cells["B12"].Value = Session["Address"].ToString();
                    //sheet.Cells["B10"].Value = 

                    sheet.Cells["A14"].Value = "Details:-";
                    var cell14 = sheet.Cells["A14"];
                    cell14.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell14.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    cell14.Style.Font.Size = 14; // Replace 12 with your desired font size
                    cell14.Style.Font.Bold = true;






                    int colcount = 0;
                    foreach (IGridColumn column in grid.Columns)
                    {
                        sheet.Cells[row, col].Value = column.Title;
                        sheet.Column(col++).Width = 18;
                        column.IsEncoded = false;
                        colcount++;
                    }

                    //sheet.Cells[8,1,8, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    //sheet.Cells[8, 1, 8, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                    sheet.Cells[15, 1, 15, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    sheet.Cells[15, 1, 15, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);


                    //Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                    double finalAmount = 0;
                    row++;
                    foreach (IGridRow<ExpenseItem> gridRow in grid.Rows)
                    {
                        col = 1;
                        foreach (IGridColumn column in grid.Columns)
                        {
                            sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
                        }

                        finalAmount = finalAmount + Convert.ToDouble(sheet.Cells[row, colcount].Value.ToString());   // Convert.ToDouble(grid.Rows[row]["INRamount"]);

                        row++;
                    }
                    row++;
                    sheet.Cells[row, 1, row, colcount].Style.Border.Top.Style = ExcelBorderStyle.Thick;

                    int rowvalue = row + 1;
                    sheet.Cells[rowvalue, colcount].Value = finalAmount;
                    int inta = colcount - 1;
                    int introw = rowvalue + 1;
                    int introwadd = introw + 1;


                    int introw1 = row;
                    sheet.Cells[rowvalue, inta].Value = "Grand Total";
                    sheet.Cells[introw, inta].Value = "Own Vehicle Expenses total:";

                    sheet.Cells[introw, colcount].Value = a;


                    sheet.Cells[introwadd, inta].Value = "Remaining Expenses total:";
                    sheet.Cells[introwadd, colcount].Value = finalAmount - a;




                    #region 2nd Table


                    //Int32 row1 = row + 1;
                    //Int32 col1 = 1;
                    //IGrid<ExpenseItem> grid1 = CreateExportableGrid1(c);
                    //int colcount1 = 0;
                    //foreach (IGridColumn column1 in grid1.Columns)
                    //{
                    //    sheet.Cells[row1, col1].Value = column1.Title;
                    //    sheet.Column(col1++).Width = 18;
                    //    column1.IsEncoded = false;
                    //    colcount1++;
                    //}



                    //double finalAmount1 = 0;
                    //row1++;
                    //foreach (IGridRow<ExpenseItem> gridRow1 in grid1.Rows)
                    //{
                    //    col1 = 1;
                    //    foreach (IGridColumn column in grid1.Columns)
                    //    {
                    //        sheet.Cells[row1, col1++].Value = column.ValueFor(gridRow1);
                    //    }
                    //   // finalAmount1 = finalAmount1 + Convert.ToDouble(sheet.Cells[row1, colcount1].Value.ToString());   // Convert.ToDouble(grid.Rows[row]["INRamount"]);

                    //    row1++;
                    //}
                    //row1++;
                    //sheet.Cells[row1, 1, row1, colcount1].Style.Border.Top.Style = ExcelBorderStyle.Thick;

                    //sheet.Cells[row1++, colcount1].Value = finalAmount1;

                    int rowI = row + 1;
                    string strRow = "E" + rowI;

                    sheet.Cells[rowI, 1].Value = "Amont Distribution:";
                    var cell15 = sheet.Cells[rowI, 1];
                    cell15.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell15.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                    row = row + 2;
                    col = 1;
                    IGrid<ExpenseItem> grid1 = CreateExportableGrid1(c);
                    int colcount1 = 0;
                    foreach (IGridColumn column1 in grid1.Columns)
                    {
                        sheet.Cells[row, col].Value = column1.Title;
                        sheet.Column(col++).Width = 18;
                        column1.IsEncoded = false;
                        colcount1++;
                    }

                    sheet.Cells[row, 1, row, colcount1].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    sheet.Cells[row, 1, row, colcount1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);


                    double finalAmount1 = 0;
                    row++;
                    foreach (IGridRow<ExpenseItem> gridRow1 in grid1.Rows)
                    {
                        col = 1;
                        foreach (IGridColumn column in grid1.Columns)
                        {
                            sheet.Cells[row, col++].Value = column.ValueFor(gridRow1);
                        }
                        finalAmount1 = finalAmount1 + Convert.ToDouble(sheet.Cells[row, colcount1].Value.ToString());   // Convert.ToDouble(grid.Rows[row]["INRamount"]);

                        row++;
                    }
                    row++;
                    sheet.Cells[row, 1, row, colcount1].Style.Border.Top.Style = ExcelBorderStyle.Thick;

                    sheet.Cells[row++, colcount1].Value = finalAmount1;
                    #endregion





                    return File(package.GetAsByteArray(), "application/unknown", "Approved Voucher" + DateTime.Now.ToShortDateString() + ".xlsx");
                }
            }
            else
            {
                return View();
            }
        }

        private IGrid<ExpenseItem> CreateExportableGrid(ExpenseItem c, string VoucherNo)
        {

            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<ExpenseItem> grid = new Grid<ExpenseItem>(GetData(c, VoucherNo));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            //grid.Columns.Add(model => model.VoucherId).Titled("Voucher Generated");

            grid.Columns.Add(model => model.Date).Titled("Date");
            grid.Columns.Add(model => model.SAPNo).Titled("SAP No");
            grid.Columns.Add(model => model.SubJobNo).Titled("Job No");
            grid.Columns.Add(model => model.ExpenseType).Titled("Expense Name");
            grid.Columns.Add(model => model.VendorName).Titled("Vendor Name");
            grid.Columns.Add(model => model.City).Titled("Location");
            grid.Columns.Add(model => model.Country).Titled("Country");
            grid.Columns.Add(model => model.Currency).Titled("Currency");
            grid.Columns.Add(model => model.ExchRate).Titled("ExchRate");

            grid.Columns.Add(model => model.Description).Titled("Description");
            grid.Columns.Add(model => model.INRAmount).Titled("Amont in INR");


            grid.Pager = new GridPager<ExpenseItem>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objEIModel.LstDashboard.Count;
            //grid.Pager.RowsPerPage = objEIModel.LstDashboard;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }


            return grid;
        }

        private IGrid<ExpenseItem> CreateExportableGrid1(ExpenseItem c)
        {

            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<ExpenseItem> grid1 = new Grid<ExpenseItem>(GetData1(c));
            grid1.ViewContext = new ViewContext { HttpContext = HttpContext };

            //grid.Columns.Add(model => model.VoucherId).Titled("Voucher Generated");

            grid1.Columns.Add(model => model.CostCenter).Titled("Cost Center");
            grid1.Columns.Add(model => model.SubJobNo).Titled("Job Number");
            grid1.Columns.Add(model => model.SAPNo).Titled("SAPNo");
            grid1.Columns.Add(model => model.CompanyName).Titled("CompanyName");
            grid1.Columns.Add(model => model.Amount).Titled("INRAmount");

            grid1.Pager = new GridPager<ExpenseItem>(grid1);
            grid1.Processors.Add(grid1.Pager);
            grid1.Pager.RowsPerPage = objEIModel.LstCostDistribution.Count;

            foreach (IGridColumn column in grid1.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }


            return grid1;
        }

        public List<ExpenseItem> GetData(ExpenseItem c, string VoucherNo)
        {

            List<ExpenseItem> Data1 = new List<ExpenseItem>();

            DataSet Data = new DataSet();
            string User = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            Data = objEV.Data(User);


            if (Data.Tables.Count > 0)
            {
                objEIModel.month = Data.Tables[0].Rows[0]["MonthName"].ToString();
                objEIModel.SAPNo = Data.Tables[0].Rows[0]["SAPEmpCode"].ToString();
                objEIModel.Address = Data.Tables[0].Rows[0]["Address1"].ToString();
                objEIModel.Year = Convert.ToString(Data.Tables[0].Rows[0]["Year"]);
                objEIModel.ModifiedDate = Data.Tables[0].Rows[0]["SubmissionDate"].ToString();
                objEIModel.CostCenter = Convert.ToString(Data.Tables[0].Rows[0]["Cost_center"]);
                objEIModel.VoucherNo = Convert.ToString(Data.Tables[0].Rows[0]["VoucherNo"]);
                objEIModel.Branch = Convert.ToString(Data.Tables[0].Rows[0]["Branch"]);
            }
            string month = objEIModel.month;
            Session["Month"] = month;
            string Year = objEIModel.Year;
            Session["Year"] = Year;
            string Address = objEIModel.Address;
            Session["Address"] = Address;
            string ModifiedDate = objEIModel.ModifiedDate;
            Session["ModifiedDate"] = ModifiedDate;
            string CostCenter = objEIModel.CostCenter;
            Session["CostCenter"] = CostCenter;

            //string VoucherNo = objEIModel.VoucherNo;
            //Session["VoucherNo"] = VoucherNo;

            string Branch = objEIModel.Branch;
            Session["Branch"] = Branch;
            string SAPNo = objEIModel.SAPNo;
            Session["SAPNo"] = SAPNo;


            List<ExpenseItem> lmd = new List<ExpenseItem>();
            List<ExpenseItem> lmd1 = new List<ExpenseItem>(); // creating list of model.  
            DataSet ds = new DataSet();

            //Session["GetExcelData"] = "Yes";
            //string Role = Session["RoleName"].ToString();


            ds = objEV.VoucherData(VoucherNo);
            ////ds = objEV.GetVoucherStatus(Role, UserId);
            //if (Session["VoucherNo"] != null)
            //{
            //    ds = objEV.VoucherData(Session["VoucherNo"].ToString());

            //}


            //ds = objEV.GetVoucherStatus();


            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new ExpenseItem
                {
                    Date = Convert.ToString(dr["Date"]),
                    ExpenseType = Convert.ToString(dr["ExpenseType"]),
                    Country = Convert.ToString(dr["Country"]),
                    Currency = Convert.ToString(dr["Currency"]),
                    Amount = Convert.ToInt32(dr["Amount"]),
                    VoucherNo = Convert.ToString(dr["VoucherNo"]),
                    SAPNo = Convert.ToString(dr["SAPEmpCode"]),
                    VendorName = Convert.ToString(dr["vendor_name"]),
                    Description = Convert.ToString(dr["Description"]),
                    INRAmount = Convert.ToDouble(dr["INRAmount"]),
                    INRAmount1 = Convert.ToDouble(dr["INRAmount1"]),

                    ExchRate = Convert.ToInt32(dr["ExchRate"]),
                    ModifiedDate = Convert.ToString(dr["SubmissionDate"]),
                    SubJobNo = Convert.ToString(dr["SubJob_No"]),
                    City = Convert.ToString(dr["City"]),

                });
            }


            objEIModel.LstDashboard = lmd;





            //Session["FromDate"] = Convert.ToString(ds.Tables[2].Rows[0]["firstDate"]);
            //Session["ToDate"] = Convert.ToString(ds.Tables[2].Rows[0]["LastDate"]);

            //foreach (DataRow dr in ds.Tables[1].Rows) // loop for adding add from dataset to list<modeldata>  
            //{
            //    lmd.Add(new ExpenseItem
            //    {ex

            //        CostCenter = Convert.ToString(dr["Cost_Center"]),
            //        SAPNo = Convert.ToString(dr["SAP_No"]),
            //        SubJobNo = Convert.ToString(dr["SubJob_No"]),
            //        CompanyName = Convert.ToString(dr["Company_Name"]),
            //        Amount = Convert.ToDouble(dr["Amount"]),


            //    });
            //}
            //objEIModel.LstCostDistribution = lmd1;


            Session["DataList"] = objEIModel.LstDashboard;

            return objEIModel.LstDashboard;

            //return  (lmd, lmd1);
            //return objEIModel;
        }

        public List<ExpenseItem> GetData1(ExpenseItem c)
        {


            List<ExpenseItem> lmd = new List<ExpenseItem>();
            List<ExpenseItem> lmd1 = new List<ExpenseItem>(); // creating list of model.



            DataSet ds = new DataSet();

            Session["GetExcelData"] = "Yes";





            if (Session["VoucherNo"] != null)
            {
                ds = objEV.VoucherData(Session["VoucherNo"].ToString());

            }


            //foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            //{
            //    lmd.Add(new ExpenseItem
            //    {



            //      Date = Convert.ToString(dr["Date"]),
            //     ExpenseType = Convert.ToString(dr["ExpenseType"]),
            //        Country = Convert.ToString(dr["Country"]),
            //        Currency = Convert.ToString(dr["Currency"]),
            //        Amount = Convert.ToDouble(dr["Amount"]),
            //        ExchRate = Convert.ToDouble(dr["ExchRate"]),
            //        Description = Convert.ToString(dr["Description"]),
            //        INRAmount = Convert.ToDouble(dr["INRAmount"]),



            //    });
            //}
            //objEIModel.LstDashboard = lmd;


            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd1.Add(new ExpenseItem
                {

                    CostCenter = Convert.ToString(dr["Cost_Center1"]),
                    SAPNo = Convert.ToString(dr["SAPEmpCode"]),
                    SubJobNo = Convert.ToString(dr["SubJob_No"]),
                    CompanyName = Convert.ToString(dr["Company_Name"]),
                    Amount = Convert.ToDouble(dr["Amount"]),


                });
            }
            objEIModel.LstCostDistribution = lmd1;



            return objEIModel.LstCostDistribution;
            //return  (lmd, lmd1);
            //return objEIModel;
        }

        public ActionResult ConvertToPdf(string VoucherNo)
        {
            DataSet dsHtml = new DataSet();
            dsHtml = objEV.GetDataForPdf(VoucherNo);
            string htmlContent = "";
            string input = VoucherNo;
            string[] parts = input.Split('/');
            string year = parts[2];

            //string month = parts[1];
            //string year = parts[2];


            //using (var excelPackage = new ExcelPackage())
            //{
            //    // Create the worksheet
            //    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
            //    sheet.Cells["A1"].Value = "TUV India Private Ltd.";
            //    var cell = sheet.Cells["A1"];
            //    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
            //    cell.Style.Font.Size = 14; // Replace 12 with your desired font size
            //    cell.Style.Font.Bold = true;

            //    sheet.Cells["A3"].Value = "Expenses Report:";
            //    var cell3 = sheet.Cells["A3"];
            //    // Replace 12 with your desired font size
            //    cell3.Style.Font.Bold = true;
            //    //sheet.Cells["B3"].Value = 
            //    sheet.Cells["C3"].Value = "Month:";
            //    var cell4 = sheet.Cells["C3"];
            //    cell4.Style.Font.Bold = true;

            //    sheet.Cells["D3"].Value = month;
            //    sheet.Cells["E3"].Value = "Year:";
            //    var cell5 = sheet.Cells["E3"];
            //    cell5.Style.Font.Bold = true;//Session["fullName"].ToString();
            //    sheet.Cells["f3"].Value = "20" + year;
            string[] year1 = year.Split('-');
            string year2 = year1[0];
            string year_ = year1[1];




            using (var excelPackage = new ExcelPackage())
            {
                // Create the worksheet
                var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                sheet.Cells["A1"].Value = "TUV India Private Ltd.";
                var cell = sheet.Cells["A1"];
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                cell.Style.Font.Size = 14; // Replace 12 with your desired font size
                cell.Style.Font.Bold = true;

                sheet.Cells["A3"].Value = "Expenses Report:";
                var cell3 = sheet.Cells["A3"];
                cell3.Style.Font.Bold = true;
                sheet.Cells["B3"].Value = "Year:";
                var cell5 = sheet.Cells["B3"];
                cell5.Style.Font.Bold = true;//Session["fullName"].ToString();
                sheet.Cells["C3"].Value = "20" + year2 + "-20" + year_;


                sheet.Cells["G3"].Value = "Submission Date:";
                var cell6 = sheet.Cells["G3"];
                cell6.Style.Font.Bold = true;
                sheet.Cells["H3"].Value = dsHtml.Tables[0].Rows[0]["IsSendForApproval_Date"].ToString();


                sheet.Cells["A5"].Value = "Expenses Voucher Number:";
                var cell2 = sheet.Cells["A5"];
                cell2.Style.Font.Size = 13; // Replace 12 with your desired font size
                cell2.Style.Font.Bold = true;

                //sheet.Cells["B5"].Value = Session["VoucherNo"].ToString();
                sheet.Cells["B5"].Value = VoucherNo;
                sheet.Cells["A7"].Value = "Branch:";
                var cell7 = sheet.Cells["A7"];
                cell7.Style.Font.Bold = true;
                sheet.Cells["B7"].Value = dsHtml.Tables[2].Rows[0]["Branch"].ToString();

                sheet.Cells["A8"].Value = "Name of Employee:";
                var cell8 = sheet.Cells["A8"];
                cell8.Style.Font.Bold = true;
                sheet.Cells["B8"].Value = dsHtml.Tables[2].Rows[0]["EmployeeName"].ToString();

                sheet.Cells["A9"].Value = "HR Employee Code:";
                var cell9 = sheet.Cells["A9"];
                cell9.Style.Font.Bold = true;
                sheet.Cells["B9"].Value = dsHtml.Tables[2].Rows[0]["EmployeeCode"].ToString();
                sheet.Cells["A10"].Value = "SAP EMp Code:";
                var cell10 = sheet.Cells["A10"];
                cell10.Style.Font.Bold = true;
                sheet.Cells["B10"].Value = dsHtml.Tables[2].Rows[0]["SAPEmpCode"].ToString();
                sheet.Cells["A11"].Value = "Cost Centre:";
                var cell11 = sheet.Cells["A11"];
                cell11.Style.Font.Bold = true;
                sheet.Cells["B11"].Value = dsHtml.Tables[2].Rows[0]["Cost_center"].ToString();
                sheet.Cells["E5"].Value = "printed on:";
                var cell12 = sheet.Cells["E5"];
                cell12.Style.Font.Bold = true;
                sheet.Cells["F5"].Value = DateTime.Now.ToShortDateString();
                sheet.Cells["A12"].Value = "Current Residing Address:";
                var cell13 = sheet.Cells["A12"];
                cell13.Style.Font.Bold = true;
                sheet.Cells["B12"].Value = dsHtml.Tables[2].Rows[0]["Address1"].ToString();
                //sheet.Cells["B10"].Value = 

                sheet.Cells["A14"].Value = "Details:-";
                var cell14 = sheet.Cells["A14"];
                cell14.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell14.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                cell14.Style.Font.Size = 14; // Replace 12 with your desired font size
                cell14.Style.Font.Bold = true;

                sheet.Cells["A15"].Value = "Date";
                sheet.Cells["B15"].Value = "Type";
                sheet.Cells["C15"].Value = "SAP No";
                sheet.Cells["D15"].Value = "Sub Job No";
                sheet.Cells["E15"].Value = "Expense Name";
                sheet.Cells["F15"].Value = "KM";
                sheet.Cells["G15"].Value = "Customer Name";
                sheet.Cells["H15"].Value = "Vendor Name";
                sheet.Cells["I15"].Value = "Location";
                sheet.Cells["J15"].Value = "Country";
                sheet.Cells["K15"].Value = "Currency";
                sheet.Cells["L15"].Value = "ExchRate";
                sheet.Cells["M15"].Value = "Description";
                sheet.Cells["N15"].Value = "Amount in INR";

                var range = sheet.Cells["A15:N15"];

                // Apply formatting to the range
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                range.Style.Font.Size = 12;
                range.Style.Font.Bold = true;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Color.SetColor(Color.Black);
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Color.SetColor(Color.Black);
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Color.SetColor(Color.Black);
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Color.SetColor(Color.Black);

                decimal RemainingTotal = 0;
                decimal OwnVehicalTotal = 0;
                int j = 0;
                for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
                {
                    sheet.Cells[16 + i, 1].Value = dsHtml.Tables[0].Rows[i]["Date"].ToString();
                    sheet.Cells[16 + i, 2].Value = dsHtml.Tables[0].Rows[i]["Type"].ToString();
                    sheet.Cells[16 + i, 3].Value = dsHtml.Tables[0].Rows[i]["SAP_No"].ToString();
                    sheet.Cells[16 + i, 4].Value = dsHtml.Tables[0].Rows[i]["SubJob_No"].ToString();
                    sheet.Cells[16 + i, 5].Value = dsHtml.Tables[0].Rows[i]["ExpenseName"].ToString();
                    sheet.Cells[16 + i, 6].Value = dsHtml.Tables[0].Rows[i]["KM"].ToString();
                    sheet.Cells[16 + i, 7].Value = dsHtml.Tables[0].Rows[i]["Company_Name"].ToString();
                    sheet.Cells[16 + i, 8].Value = dsHtml.Tables[0].Rows[i]["vendor_name"].ToString();
                    sheet.Cells[16 + i, 9].Value = dsHtml.Tables[0].Rows[i]["City/Cities"].ToString();
                    sheet.Cells[16 + i, 10].Value = dsHtml.Tables[0].Rows[i]["Country"].ToString();
                    sheet.Cells[16 + i, 11].Value = dsHtml.Tables[0].Rows[i]["Currency"].ToString();
                    sheet.Cells[16 + i, 12].Value = dsHtml.Tables[0].Rows[i]["ExchRate"].ToString();
                    sheet.Cells[16 + i, 13].Value = dsHtml.Tables[0].Rows[i]["Description"].ToString();
                    sheet.Cells[16 + i, 14].Value = dsHtml.Tables[0].Rows[i]["Amount"].ToString();

                    var range_ = sheet.Cells["A" + (16 + i) + ":N" + (16 + i)];
                    range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range_.Style.Border.Top.Color.SetColor(Color.Black);
                    range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range_.Style.Border.Left.Color.SetColor(Color.Black);
                    range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range_.Style.Border.Right.Color.SetColor(Color.Black);
                    range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range_.Style.Border.Bottom.Color.SetColor(Color.Black);


                    if (dsHtml.Tables[0].Rows[i]["ExpenseName"].ToString() == "Own Bike" || dsHtml.Tables[0].Rows[i]["ExpenseName"].ToString() == "Own Car")
                    {
                        OwnVehicalTotal = OwnVehicalTotal + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["Amount"].ToString());
                    }
                    else
                    {
                        RemainingTotal = RemainingTotal + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["Amount"].ToString());
                    }
                    j = 16 + i;
                }
                decimal _GrandTotal = OwnVehicalTotal + RemainingTotal;

                sheet.Cells["M" + (j + 1)].Value = "Grand Total:";
                sheet.Cells["M" + (j + 2)].Value = "Own Vehicle Expenses total:";
                sheet.Cells["M" + (j + 3)].Value = "Remaining Expenses total:";

                sheet.Cells["N" + (j + 1)].Value = _GrandTotal;
                sheet.Cells["N" + (j + 2)].Value = OwnVehicalTotal;
                sheet.Cells["N" + (j + 3)].Value = RemainingTotal;


                var range1 = sheet.Cells["M" + (j + 1) + ":N" + (j + 1)];
                range1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range1.Style.Border.Top.Color.SetColor(Color.Black);
                range1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range1.Style.Border.Left.Color.SetColor(Color.Black);
                range1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range1.Style.Border.Right.Color.SetColor(Color.Black);
                range1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range1.Style.Border.Bottom.Color.SetColor(Color.Black);

                var range2 = sheet.Cells["M" + (j + 2) + ":N" + (j + 2)];
                range2.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range2.Style.Border.Top.Color.SetColor(Color.Black);
                range2.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range2.Style.Border.Left.Color.SetColor(Color.Black);
                range2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range2.Style.Border.Right.Color.SetColor(Color.Black);
                range2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range2.Style.Border.Bottom.Color.SetColor(Color.Black);

                var range3 = sheet.Cells["M" + (j + 3) + ":N" + (j + 3)];
                range3.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range3.Style.Border.Top.Color.SetColor(Color.Black);
                range3.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range3.Style.Border.Left.Color.SetColor(Color.Black);
                range3.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range3.Style.Border.Right.Color.SetColor(Color.Black);
                range3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range3.Style.Border.Bottom.Color.SetColor(Color.Black);

                sheet.Cells["A" + (j + 4)].Value = "Type";
                sheet.Cells["B" + (j + 4)].Value = "Cost Center";
                sheet.Cells["C" + (j + 4)].Value = "Job Number";
                sheet.Cells["D" + (j + 4)].Value = "SAP No";
                sheet.Cells["E" + (j + 4)].Value = "Customer Name";
                sheet.Cells["F" + (j + 4)].Value = "Amount In INR";

                var _range = sheet.Cells["A" + (j + 4) + ":F" + (j + 4)];

                // Apply formatting to the range
                _range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                _range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                _range.Style.Font.Size = 12;
                _range.Style.Font.Bold = true;
                _range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                _range.Style.Border.Top.Color.SetColor(Color.Black);
                _range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                _range.Style.Border.Left.Color.SetColor(Color.Black);
                _range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                _range.Style.Border.Right.Color.SetColor(Color.Black);
                _range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                _range.Style.Border.Bottom.Color.SetColor(Color.Black);

                decimal GrandTotal = 0;
                int k = 0;
                int l = j + 5;
                for (int i = 0; i < dsHtml.Tables[1].Rows.Count; i++)
                {
                    sheet.Cells[l + i, 1].Value = dsHtml.Tables[1].Rows[i]["Type"].ToString();
                    sheet.Cells[l + i, 2].Value = dsHtml.Tables[1].Rows[i]["Cost_center"].ToString();
                    sheet.Cells[l + i, 3].Value = dsHtml.Tables[1].Rows[i]["SubJob_No"].ToString();
                    sheet.Cells[l + i, 4].Value = dsHtml.Tables[1].Rows[i]["SAP_No"].ToString();
                    sheet.Cells[l + i, 5].Value = dsHtml.Tables[1].Rows[i]["Company_Name"].ToString();
                    sheet.Cells[l + i, 6].Value = dsHtml.Tables[1].Rows[i]["TotalAmount"].ToString();

                    var range_ = sheet.Cells["A" + (l + i) + ":F" + (l + i)];
                    range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range_.Style.Border.Top.Color.SetColor(Color.Black);
                    range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range_.Style.Border.Left.Color.SetColor(Color.Black);
                    range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range_.Style.Border.Right.Color.SetColor(Color.Black);
                    range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range_.Style.Border.Bottom.Color.SetColor(Color.Black);
                    k = l + i;
                    GrandTotal = GrandTotal + Convert.ToDecimal(dsHtml.Tables[1].Rows[i]["TotalAmount"].ToString());
                }
                sheet.Cells["E" + (k + 1)].Value = "Amount In INR";
                sheet.Cells["F" + (k + 1)].Value = GrandTotal;

                var rangeTotal = sheet.Cells["E" + (k + 1) + ":F" + (k + 1)];
                rangeTotal.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rangeTotal.Style.Border.Top.Color.SetColor(Color.Black);
                rangeTotal.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rangeTotal.Style.Border.Left.Color.SetColor(Color.Black);
                rangeTotal.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rangeTotal.Style.Border.Right.Color.SetColor(Color.Black);
                rangeTotal.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rangeTotal.Style.Border.Bottom.Color.SetColor(Color.Black);

                sheet.Cells.AutoFitColumns();

                // Convert the Excel package to a byte array
                byte[] excelBytes = excelPackage.GetAsByteArray();

                // Set the response content type and headers
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + VoucherNo.Replace(" - ", "_") + ".xlsx");

                // Write the Excel data to the response
                Response.BinaryWrite(excelBytes);
                Response.Flush();
                Response.End();
            }

            return View();


            
        }
      
        public ActionResult SapDownload(string Data1)
        {
            try
            {

                //string data = Request.QueryString["data"];
                //DataTable dt = JsonConvert.DeserializeObject<DataTable>(data);

                //DataSet sapData = new DataSet();
                //sapData.Tables.Add(dt);
                //string Result = string.Empty;
                //DataSet dsHtml = objEV.GetSapData(sapData);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(Data1);

                DataSet sapData = new DataSet();
                sapData.Tables.Add(dt);
                string Result = string.Empty;
                DataSet dsHtml = objEV.GetSapData(sapData);
                using (var excelPackage = new ExcelPackage())
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    sheet.Cells["A1"].Value = "1";
                    var cell = sheet.Cells["A1"];
                    cell.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["B1"].Value = "KR";
                    var cell12 = sheet.Cells["B1"];
                    cell12.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell12.Style.Font.Bold = true;
                    cell12.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell12.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["C1"].Value = DateTime.Now.ToString("dd.MM.yyy");
                    var cell13 = sheet.Cells["C1"];
                    cell13.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell13.Style.Font.Bold = true;
                    cell13.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell13.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["D1"].Value = "INR";
                    var cell14 = sheet.Cells["D1"];
                    cell14.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell14.Style.Font.Bold = true;
                    cell14.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell14.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["E1"].Value = "93";
                    var cell15 = sheet.Cells["E1"];
                    cell15.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell15.Style.Font.Bold = true;
                    cell15.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell15.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["F1"].Value = "Example : TRAVELLING";
                    var cell16 = sheet.Cells["F1"];
                    cell16.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell16.Style.Font.Bold = true;
                    cell16.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell16.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["G1"].Value = "";
                    var cell17 = sheet.Cells["G1"];
                    cell17.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell17.Style.Font.Bold = true;
                    cell17.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell17.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["H1"].Value = "";
                    var cell18 = sheet.Cells["H1"];
                    cell18.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell18.Style.Font.Bold = true;
                    cell18.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell18.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["I1"].Value = "Costcentre";
                    var cell19 = sheet.Cells["I1"];
                    cell19.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell19.Style.Font.Bold = true;
                    cell19.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell19.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["J1"].Value = "Item No";
                    var cell20 = sheet.Cells["J1"];
                    cell20.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell20.Style.Font.Bold = true;
                    cell20.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell20.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    sheet.Cells["K1"].Value = "Sap No";
                    var cell2341 = sheet.Cells["K1"];
                    cell2341.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell2341.Style.Font.Bold = true;
                    cell2341.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell2341.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //sheet.Cells["A2"].Value = "Sap Uploading";
                    //var cell2 = sheet.Cells["A2"];
                    //cell2.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell2.Style.Font.Bold = true;
                    //cell2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    //sheet.Cells["B2"].Value = "Document Type";
                    //var cell_ = sheet.Cells["B2"];
                    //cell_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell_.Style.Font.Bold = true;
                    //cell_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //sheet.Cells["C2"].Value = "Vendor Code or GL Code";
                    //var cell1 = sheet.Cells["C2"];
                    //cell1.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell1.Style.Font.Bold = true;
                    //cell1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //sheet.Cells["D2"].Value = "INR";
                    //var cell2_ = sheet.Cells["D2"];
                    //cell2_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell2_.Style.Font.Bold = true;
                    //cell2_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell2_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //sheet.Cells["E2"].Value = "Sales Order";
                    //var cell3 = sheet.Cells["E2"];
                    //cell3.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell3.Style.Font.Bold = true;
                    //cell3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    //sheet.Cells["F2"].Value = "Trip Details";
                    //var cell4 = sheet.Cells["F2"];
                    //cell4.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell4.Style.Font.Bold = true;
                    //cell4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //cell4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //sheet.Cells["G2"].Value = "Assignment ";
                    //var cell5 = sheet.Cells["G2"];
                    //cell5.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell5.Style.Font.Bold = true;
                    //cell5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //sheet.Cells["H2"].Value = "Tax code";
                    //var cell6 = sheet.Cells["H2"];
                    //cell6.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell6.Style.Font.Bold = true;
                    //cell6.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell6.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //sheet.Cells["I2"].Value = "Costcentre";
                    //var cell7 = sheet.Cells["I2"];
                    //cell7.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell7.Style.Font.Bold = true;
                    //cell7.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell7.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //sheet.Cells["J2"].Value = "Item No";
                    //var cell8 = sheet.Cells["J2"];
                    //cell8.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell8.Style.Font.Bold = true;
                    //cell8.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell8.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ////added by nikita on 22012024
                    //sheet.Cells["K2"].Value = "Sap No";
                    //var cell9 = sheet.Cells["K2"];
                    //cell9.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell9.Style.Font.Bold = true;
                    //cell9.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //cell9.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    int j = 0;
                    for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
                    {
                        sheet.Cells[2 + i, 1].Value = dsHtml.Tables[0].Rows[i]["SAP uploading"].ToString();
                        sheet.Cells[2 + i, 2].Value = dsHtml.Tables[0].Rows[i]["Document Type"].ToString();
                        sheet.Cells[2 + i, 3].Value = dsHtml.Tables[0].Rows[i]["Vendor Code or GL Code"].ToString();
                        sheet.Cells[2 + i, 4].Value = dsHtml.Tables[0].Rows[i]["INR"].ToString();
                        //sheet.Cells[3 + i, 4].Value = dsHtml.Tables[0].Rows[i]["INR"].ToString();
                        //sheet.Cells[3 + i, 4].Value = Convert.ToSingle(dsHtml.Tables[0].Rows[i]["INR"]);
                        sheet.Cells[2 + i, 5].Value = dsHtml.Tables[0].Rows[i]["Sales Order"].ToString();
                        sheet.Cells[2 + i, 6].Value = dsHtml.Tables[0].Rows[i]["trip Details"].ToString();
                        sheet.Cells[2 + i, 7].Value = dsHtml.Tables[0].Rows[i]["Assignment"].ToString();
                        sheet.Cells[2 + i, 8].Value = dsHtml.Tables[0].Rows[i]["Tax code"].ToString();
                        sheet.Cells[2 + i, 9].Value = dsHtml.Tables[0].Rows[i]["Costcentre"].ToString();
                        sheet.Cells[2 + i, 10].Value = dsHtml.Tables[0].Rows[i]["Item No"].ToString();
                        sheet.Cells[2 + i, 11].Value = dsHtml.Tables[0].Rows[i]["Subjobno"].ToString();


                        //var range_ = sheet.Cells["A" + (3 + i) + ":k" + (3 + i)];
                        //range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //range_.Style.Border.Top.Color.SetColor(Color.Black);
                        //range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //range_.Style.Border.Left.Color.SetColor(Color.Black);
                        //range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        //range_.Style.Border.Right.Color.SetColor(Color.Black);
                        //range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //range_.Style.Border.Bottom.Color.SetColor(Color.Black);



                        j = 3 + i;
                    }

                    sheet.Cells.AutoFitColumns();
                    byte[] excelBytes = excelPackage.GetAsByteArray();
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SapDownloadReport.xlsx");

                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            }
            //return View();

        }


        //public ActionResult SapDownload(string Data1)
        //{
        //    try
        //    {

        //        //string data = Request.QueryString["data"];
        //        //DataTable dt = JsonConvert.DeserializeObject<DataTable>(data);

        //        //DataSet sapData = new DataSet();
        //        //sapData.Tables.Add(dt);
        //        //string Result = string.Empty;
        //        //DataSet dsHtml = objEV.GetSapData(sapData);
        //        DataTable dt = JsonConvert.DeserializeObject<DataTable>(Data1);

        //        DataSet sapData = new DataSet();
        //        sapData.Tables.Add(dt);
        //        string Result = string.Empty;
        //        DataSet dsHtml = objEV.GetSapData(sapData);
        //        using (var excelPackage = new ExcelPackage())
        //        {
        //            var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

        //            sheet.Cells["A1"].Value = "1";
        //            var cell = sheet.Cells["A2"];
        //            cell.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["B1"].Value = "KR";
        //            var cell12 = sheet.Cells["B1"];
        //            cell12.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell12.Style.Font.Bold = true;
        //            cell12.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell12.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["C1"].Value = "";
        //            var cell13 = sheet.Cells["C1"];
        //            cell13.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell13.Style.Font.Bold = true;
        //            cell13.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell13.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["D1"].Value = "INR";
        //            var cell14 = sheet.Cells["D1"];
        //            cell14.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell14.Style.Font.Bold = true;
        //            cell14.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell14.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["E1"].Value = "93";
        //            var cell15 = sheet.Cells["E1"];
        //            cell15.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell15.Style.Font.Bold = true;
        //            cell15.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell15.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["F1"].Value = "Example : TRAVELLING";
        //            var cell16 = sheet.Cells["F1"];
        //            cell16.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell16.Style.Font.Bold = true;
        //            cell16.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell16.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["G1"].Value = "";
        //            var cell17 = sheet.Cells["G1"];
        //            cell17.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell17.Style.Font.Bold = true;
        //            cell17.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell17.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["H1"].Value = "";
        //            var cell18 = sheet.Cells["H1"];
        //            cell18.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell18.Style.Font.Bold = true;
        //            cell18.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell18.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["I1"].Value = "Costcentre";
        //            var cell19 = sheet.Cells["I1"];
        //            cell19.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell19.Style.Font.Bold = true;
        //            cell19.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell19.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["J1"].Value = "Item No";
        //            var cell20 = sheet.Cells["J1"];
        //            cell20.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell20.Style.Font.Bold = true;
        //            cell20.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell20.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



        //            sheet.Cells["A2"].Value = "Sap Uploading";
        //            var cell2 = sheet.Cells["A2"];
        //            cell2.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell2.Style.Font.Bold = true;
        //            cell2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


        //            sheet.Cells["B2"].Value = "Document Type";
        //            var cell_ = sheet.Cells["B2"];
        //            cell_.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell_.Style.Font.Bold = true;
        //            cell_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["C2"].Value = "Vendor Code or GL Code";
        //            var cell1 = sheet.Cells["C2"];
        //            cell1.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell1.Style.Font.Bold = true;
        //            cell1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["D2"].Value = "INR";
        //            var cell2_ = sheet.Cells["D2"];
        //            cell2_.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell2_.Style.Font.Bold = true;
        //            cell2_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell2_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["E2"].Value = "Sales Order";
        //            var cell3 = sheet.Cells["E2"];
        //            cell3.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell3.Style.Font.Bold = true;
        //            cell3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


        //            sheet.Cells["F2"].Value = "Trip Details";
        //            var cell4 = sheet.Cells["F2"];
        //            cell4.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell4.Style.Font.Bold = true;
        //            cell4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["G2"].Value = "Assignment ";
        //            var cell5 = sheet.Cells["G2"];
        //            cell5.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell5.Style.Font.Bold = true;
        //            cell5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["H2"].Value = "Tax code";
        //            var cell6 = sheet.Cells["H2"];
        //            cell6.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell6.Style.Font.Bold = true;
        //            cell6.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell6.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["I2"].Value = "Costcentre";
        //            var cell7 = sheet.Cells["I2"];
        //            cell7.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell7.Style.Font.Bold = true;
        //            cell7.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell7.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["J2"].Value = "Item No";
        //            var cell8 = sheet.Cells["J2"];
        //            cell8.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell8.Style.Font.Bold = true;
        //            cell8.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell8.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            //added by nikita on 22012024
        //            sheet.Cells["K2"].Value = "Sap No";
        //            var cell9 = sheet.Cells["K2"];
        //            cell9.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell9.Style.Font.Bold = true;
        //            cell9.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell9.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


        //            int j = 0;
        //            for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
        //            {
        //                sheet.Cells[3 + i, 1].Value = dsHtml.Tables[0].Rows[i]["SAP uploading"].ToString();
        //                sheet.Cells[3 + i, 2].Value = dsHtml.Tables[0].Rows[i]["Document Type"].ToString();
        //                sheet.Cells[3 + i, 3].Value = dsHtml.Tables[0].Rows[i]["Vendor Code or GL Code"].ToString();
        //                sheet.Cells[3 + i, 4].Value = dsHtml.Tables[0].Rows[i]["INR"].ToString();
        //                //sheet.Cells[3 + i, 4].Value = dsHtml.Tables[0].Rows[i]["INR"].ToString();
        //                //sheet.Cells[3 + i, 4].Value = Convert.ToSingle(dsHtml.Tables[0].Rows[i]["INR"]);
        //                sheet.Cells[3 + i, 5].Value = dsHtml.Tables[0].Rows[i]["Sales Order"].ToString();
        //                sheet.Cells[3 + i, 6].Value = dsHtml.Tables[0].Rows[i]["trip Details"].ToString();
        //                sheet.Cells[3 + i, 7].Value = dsHtml.Tables[0].Rows[i]["Assignment"].ToString();
        //                sheet.Cells[3 + i, 8].Value = dsHtml.Tables[0].Rows[i]["Tax code"].ToString();
        //                sheet.Cells[3 + i, 9].Value = dsHtml.Tables[0].Rows[i]["Costcentre"].ToString();
        //                sheet.Cells[3 + i, 10].Value = dsHtml.Tables[0].Rows[i]["Item No"].ToString();
        //                sheet.Cells[3 + i, 11].Value = dsHtml.Tables[0].Rows[i]["Subjobno"].ToString();


        //                var range_ = sheet.Cells["A" + (3 + i) + ":k" + (3 + i)];
        //                range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Top.Color.SetColor(Color.Black);
        //                range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Left.Color.SetColor(Color.Black);
        //                range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Right.Color.SetColor(Color.Black);
        //                range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Bottom.Color.SetColor(Color.Black);



        //                j = 3 + i;
        //            }

        //            sheet.Cells.AutoFitColumns();
        //            byte[] excelBytes = excelPackage.GetAsByteArray();
        //            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            //Response.AddHeader("Content-Disposition", "attachment; filename=SapDownloadReport" + DateTime.Now + "_.xlsx");
        //            //Response.BinaryWrite(excelBytes);
        //            //Response.Flush();
        //            //Response.End();
        //            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SapDownloadReport.xlsx");

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

        //    }
        //    //return View();

        //}


        //added by nikita on 05122023 for Button show

        public ActionResult VoucherHistorApprovedList(string VoucherNo, string IsApproval)
        {
            ViewBag.VoucherNo = VoucherNo;
            ViewBag.IsApproval = IsApproval;
            ViewBag.PreviousUrl = Request.UrlReferrer?.ToString(); // Store the previous URL in ViewBag

            return View();
        }

        public JsonResult CheckDataForButton(string VoucherNo)
        {
            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.GetData_(VoucherNo);
            Session["VoucherNo"] = VoucherNo;
            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //added by nikita on 30112023

        public ActionResult GeneratePDF(string VoucherNo)
        {
            //string VoucherNo = @"MUM/EXP/23-24/001688";
            //string VoucherNo = VoucherNo;
            DataSet dsHtml = new DataSet();
            dsHtml = objEV.GetDataForPdf(VoucherNo);
            string input = VoucherNo;
            string[] parts = input.Split('/');
            string year = parts[2];


            string[] year1 = year.Split('-');
            string year2 = year1[0];
            string year_ = year1[1];

            decimal RemainingTotal = 0;
            decimal OwnVehicalTotal = 0;


            #region Generate Link for pdf
            // string URl = "a "+ a + a" ;//"http://localhost:54895/QuotationServicesLink/QuotationServicesLink?PKServiceId= " +aa+";//+  "expensepdf.SubServiceType" + ";
            //string URl = "http://localhost:54895/QuotationServicesLink/Servicess?PKServiceId=" + expensepdf.SubServiceType + "";
            //string URl = "https://tiimes.tuv-india.com/QuotationServicesLink/Servicess?PKServiceId=" + expensepdf.VoucherNo + "";

            #endregion

            //SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
            System.Text.StringBuilder strs = new System.Text.StringBuilder();
            string body = string.Empty;


            DataTable dsGetStamp = new DataTable();
            dsGetStamp = dsHtml.Tables[0];
            string minDateString = null;
            string maxDateString = null;
            string StrOpenConcern = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/QuotationHtml/ExpensePdf.html")))
            {
                body = reader.ReadToEnd();
            }
            if (dsGetStamp.Rows.Count > 0)
            {
                DateTime minDate = DateTime.MaxValue.Date;

                DateTime maxDate = DateTime.MinValue.Date;
                foreach (DataRow dr in dsGetStamp.Rows)
                {


                    string Date = Convert.ToString(dr["Date"]);
                    string ExpenseName = Convert.ToString(dr["ExpenseName"]);
                    string Cities = Convert.ToString(dr["City/Cities"]);
                    string Description = Convert.ToString(dr["Description"]);
                    string KM = Convert.ToString(dr["KM"]);
                    string ExchRate = Convert.ToString(dr["ExchRate"]);
                    string Amount = Convert.ToString(dr["Amount"]);
                    string Rate = Convert.ToString(dr["Rate"]);

                    DateTime date_ = Convert.ToDateTime(dr["Date"]);
                    if (date_ < minDate)
                    {
                        minDate = date_;
                    }
                    if (date_ > maxDate)
                    {
                        maxDate = date_;
                    }
                    StrOpenConcern = StrOpenConcern + "<tr><td style='text-align:center;font-size:15px;color:black;font-family:\"TNG Pro\";line-height:2'>" + Date + "</td><td style='text-align:left; padding-left:1%;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + ExpenseName + "</td><td style='text-align:left; padding-left:1%;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + Cities + "</td><td style='text-align:left; padding-left:1%;font-size:15px;color:black;font-family:\"TNG Pro\";word-break:break-word;overflow-wrap:break-word;white-space:normal;'>" + Description + "</td><td style='text-align:center;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + KM + "</td><td style='text-align:center;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + ExchRate + "</td><td style='text-align:center;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + Rate + "</td><td style='text-align:right; padding-right:1%;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + Amount + "</td></tr>";

                    if (ExpenseName == "Own Bike" || ExpenseName == "Own Car")
                    {
                        OwnVehicalTotal += Convert.ToDecimal(Amount);
                    }
                    else
                    {   
                        RemainingTotal += Convert.ToDecimal(Amount);
                    }

                }
                minDateString = minDate.ToString("dd/MM/yyyy");
                maxDateString = maxDate.ToString("dd/MM/yyyy");
                decimal _GrandTotal = OwnVehicalTotal + RemainingTotal;
                StrOpenConcern += "<tr><td></td><td></td><td></td><td></td><td></td><td></td><td style='color:black;line-height:2;font-family:\"TNG Pro\";font-size:15px;font-weight:bold;'>Grand Total</td><td style='text-align:right; padding-right:1%;font-size:15px;color:black;font-family:\"TNG Pro\";font-weight:bold;'>" + _GrandTotal + "</td></tr>";
                StrOpenConcern += "<tr><td></td><td></td><td></td><td></td><td></td><td></td><td style='color:black;line-height:2;font-family:\"TNG Pro\";font-size:15px;font-weight:bold;'>OwnVehicalTotal</td><td style='text-align:right; padding-right:1%;font-size:15px;color:black;font-family:\"TNG Pro\";font-weight:bold;'>" + OwnVehicalTotal + "</td></tr>";
                StrOpenConcern += "<tr><td></td><td></td><td></td><td></td><td></td><td></td><td style='color:black;line-height:2;font-family:\"TNG Pro\";font-size:15px;font-weight:bold;'>RemainingTotal</td><td style='text-align:right; padding-right:1%;font-size:15px;color:black;font-family:\"TNG Pro\";font-weight:bold;'>" + RemainingTotal + "</td></tr>";
            }
            body = body.Replace("[ExpData]", StrOpenConcern);

            decimal RemainingTotal_ = 0;
            decimal GrandTotal_ = 0;
            string StrOpenConcern_ = string.Empty;

            DataTable dsGetStamp1 = new DataTable();
            dsGetStamp1 = dsHtml.Tables[1];
            if (dsGetStamp1.Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetStamp1.Rows)
                {

                    string Type = Convert.ToString(dr["Type"]);
                    string Cost_center = Convert.ToString(dr["Cost_center"]);
                    string SubJob_No = Convert.ToString(dr["SubJob_No"]);
                    string SAP_No = Convert.ToString(dr["SAP_No"]);
                    string Company_Name = Convert.ToString(dr["Company_Name"]);
                    string TotalAmount = Convert.ToString(dr["TotalAmount"]);

                    StrOpenConcern_ = StrOpenConcern_ + "<tr><td style='text-align:left; padding-left:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + Type + "</td><td style='text-align:left; padding-left:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + Cost_center + "</td><td style='text-align:left; padding-left:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + SubJob_No + "</td><td style='text-align:left; padding-left:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + SAP_No + "</td><td style='text-align:left; padding-left:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + Company_Name + "</td><td style='text-align:right; padding-right:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + TotalAmount + "</td></tr>";
                    RemainingTotal_ += Convert.ToDecimal(TotalAmount);
                }
                GrandTotal_ = RemainingTotal_;

                // Append the total rows outside of the loop
                StrOpenConcern_ += "<tr><td></td><td></td><td></td><td></td><td style='text-align:right;padding-right:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;font-weight:bold;'>Grand Total</td><td style='text-align:right; padding-right:1%;text:size:15px;color:black;font-family:\"TNG Pro\";font-weight:bold;'>" + GrandTotal_ + "</td></tr>";


            }

            body = body.Replace("[CostExpData]", StrOpenConcern_);

            string StrOpenConcern_1 = string.Empty;


            DataTable dt = objEV.Get_VoucherDetails_History_ID_Description_pdf(VoucherNo);


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    string Date = Convert.ToString(dr["Date"]);
                    string Stage = Convert.ToString(dr["Status"]);
                    string Approver = Convert.ToString(dr["ApproverName"]);
                    string Description = Convert.ToString(dr["Description"]);
                    string TotalAmount = Convert.ToString(dr["Deduct"]);
                    string ApprovedAmount = Convert.ToString(dr["Amount"]);



                    //< td style = 'text-align:right; padding-right:1%;text:size:15px;color:black;font-family:'TNG Pro';' > " + ApprovedAmount + " </ td >
                       StrOpenConcern_1 = StrOpenConcern_1 + "<tr style='text-align:center;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'><td style='text-align:center;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + Date + "</td><td style='text-align:left; padding-left:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + Stage + "</td><td style='text-align:left; padding-left:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + Approver + "</td><td style='text-align:left; padding-left:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + Description + "</td><td style='text-align:right; padding-right:1%;text:size:15px;color:black;font-family:\"TNG Pro\";line-height:2;'>" + TotalAmount + "</td></tr>";
                }


            }

            body = body.Replace("[Description]", StrOpenConcern_1);


            DataTable dsGetStamp_ = new DataTable();
            dsGetStamp_ = dsHtml.Tables[2];

            string _Header = string.Empty;
            string _footer = string.Empty;
           


            StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/QuotationHtml/Expense-header.html"));
            _Header = _readHeader_File.ReadToEnd();


            string Empcode = dsHtml.Tables[2].Rows[0]["EmployeeCode"].ToString();
            _Header = _Header.Replace("[EMPcode]", Empcode);


            //2103024
            string Employeement = dsHtml.Tables[0].Rows[0]["Name"].ToString();
            _Header = _Header.Replace("[Employeement]", Employeement);

            string EmployeeGrade = dsHtml.Tables[0].Rows[0]["EmployeeGrade"].ToString();
            _Header = _Header.Replace("[EmployeeGrade]", EmployeeGrade);

            string sendforapproval = dsHtml.Tables[0].Rows[0]["IsSendForApproval_Date"].ToString();
            body = body.Replace("[sendApproval]", sendforapproval);

            string SapEMPcode = dsHtml.Tables[2].Rows[0]["SAPEmpCode"].ToString();
            _Header = _Header.Replace("[SapEMPcode]", SapEMPcode);

            string Year = "20" + year2 + "-20" + year_.ToString();
            _Header = _Header.Replace("[Year]", Year);

            _Header = _Header.Replace("[Date]", minDateString + " - " + maxDateString);

            string salesorderno = dsHtml.Tables[0].Rows[0]["SAP_No"].ToString();
            _Header = _Header.Replace("[Sales Order No]", salesorderno);

            string SAP_Vendor_Code = dsHtml.Tables[2].Rows[0]["SAP_VendorCode"].ToString();
            _Header = _Header.Replace("[SAP Vendor Code]", SAP_Vendor_Code);

            _Header = _Header.Replace("[date]", DateTime.Now.ToShortDateString());
            string VoucherNo_ = VoucherNo.ToString();
            _Header = _Header.Replace("[Voucher No]", VoucherNo_);


            string replacedvoucherNo = VoucherNo_.Replace("/", "_");

            string costcenter = dsHtml.Tables[2].Rows[0]["Cost_center"].ToString();
            _Header = _Header.Replace("[costcenter]", costcenter);

            string EmployeeName = dsHtml.Tables[2].Rows[0]["EmployeeName"].ToString();
            _Header = _Header.Replace("[Name]", EmployeeName);


            string Branch = dsHtml.Tables[2].Rows[0]["Branch"].ToString();
            _Header = _Header.Replace("[Branch]", Branch);



            StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/QuotationHtml/Expense-footer.html"));
            _footer = _readFooter_File.ReadToEnd();

            
            PdfPageSize pageSize = PdfPageSize.A4;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
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



            // header settings
            converter.Options.DisplayHeader = true ||
                true || true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = 165;

            PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;

            converter.Header.Add(headerHtml);

            // footer settings
            converter.Options.DisplayFooter = true || true || true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Footer.Height = 60;

            PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);
            PdfTextSection text1 = new PdfTextSection(40, 15, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("TNG Pro", 8));
            converter.Footer.Add(text1);
            SelectPdf.PdfDocument doc_ = converter.ConvertHtmlString(body);
            int PageCount = doc_.Pages.Count;

            string path = Server.MapPath("~/QuotationHtml");
            PdfDocument doc = converter.ConvertHtmlString(body);
            doc.Save(path + '\\' + replacedvoucherNo + ".pdf");
            doc.Close();

            byte[] fileBytes = System.IO.File.ReadAllBytes(path + '\\' + replacedvoucherNo + ".pdf");


            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, replacedvoucherNo + ".pdf");

        }

        public ActionResult sapUpload()
        {
            return View();
        }


        public ActionResult sapUploaded()
        {
            return View();
        }
        //added by nikita on 27032024
        [HttpGet]
        public JsonResult GetVoucherforsap()
        {
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.GetVoucherList_Approvedforsap(UserId, Role);
            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetVoucherList_forsap()
        {
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.GetVoucherList_Uploadedsap(UserId, Role);
            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateSapData(string Data1)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(Data1);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            string Result = string.Empty;  
            Result = objEI.UpdateSapDataFlag(ds);
            string json = "";
            if (Result != "-1" || Result != string.Empty)
            {
                json = "Success";
            }
            else
            {
                json = "Error";
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ValidateSendForapproval(string VoucherNo)
        {
            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataTable dt = objEI.validate_data(VoucherNo);
            //ViewData["issapupload"] = ds.Tables[0].Rows[0]["issapdownload"];

            Session["VoucherNo"] = VoucherNo;
            string result = JsonConvert.SerializeObject(dt);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ValidateVoucherAttachments(string Voucherno)
        {
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.ValidateVoucher(Voucherno);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GeneratePDF(string VoucherNo)
        //{
        //    //string VoucherNo = @"MUM/EXP/23-24/001688";
        //    //string VoucherNo = VoucherNo;
        //    DataSet dsHtml = new DataSet();
        //    dsHtml = objEV.GetDataForPdf(VoucherNo);
        //    string htmlContent = "";
        //    string input = VoucherNo;
        //    string[] parts = input.Split('/');
        //    string year = parts[2];


        //    string[] year1 = year.Split('-');
        //    string year2 = year1[0];
        //    string year_ = year1[1];

        //    decimal RemainingTotal = 0;
        //    decimal OwnVehicalTotal = 0;


        //    #region Generate Link for pdf
        //    // string URl = "a "+ a + a" ;//"http://localhost:54895/QuotationServicesLink/QuotationServicesLink?PKServiceId= " +aa+";//+  "expensepdf.SubServiceType" + ";
        //    //string URl = "http://localhost:54895/QuotationServicesLink/Servicess?PKServiceId=" + expensepdf.SubServiceType + "";
        //    //string URl = "https://tiimes.tuv-india.com/QuotationServicesLink/Servicess?PKServiceId=" + expensepdf.VoucherNo + "";

        //    #endregion

        //    SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
        //    System.Text.StringBuilder strs = new System.Text.StringBuilder();
        //    string body = string.Empty;
        //    string ProjectName = "";
        //    string ReferenceDocumentscontent = "";


        //    DataTable dsGetStamp = new DataTable();
        //    dsGetStamp = dsHtml.Tables[0];
        //    string minDateString = null;
        //    string maxDateString = null;
        //    string StrOpenConcern = string.Empty;
        //    using (StreamReader reader = new StreamReader(Server.MapPath("~/QuotationHtml/ExpensePdf.html")))
        //    {
        //        body = reader.ReadToEnd();
        //    }
        //    if (dsGetStamp.Rows.Count > 0)
        //    {
        //        DateTime minDate = DateTime.MaxValue.Date;

        //        DateTime maxDate = DateTime.MinValue.Date;
        //        foreach (DataRow dr in dsGetStamp.Rows)
        //        {


        //            string Date = Convert.ToString(dr["Date"]);
        //            string ExpenseName = Convert.ToString(dr["ExpenseName"]);
        //            string Cities = Convert.ToString(dr["City/Cities"]);
        //            string Description = Convert.ToString(dr["Description"]);
        //            string KM = Convert.ToString(dr["KM"]);
        //            string ExchRate = Convert.ToString(dr["ExchRate"]);
        //            string Amount = Convert.ToString(dr["Amount"]);
        //            string Rate = Convert.ToString(dr["Rate"]);

        //            DateTime date_ = Convert.ToDateTime(dr["Date"]);
        //            if (date_ < minDate)
        //            {
        //                minDate = date_;
        //            }
        //            if (date_ > maxDate)
        //            {
        //                maxDate = date_;
        //            }
        //            StrOpenConcern = StrOpenConcern + "<tr style='text-align:center'><td>" + Date + "</td><td>" + ExpenseName + "</td><td>" + Cities + "</td><td>" + Description + "</td><td>" + KM + "</td><td>" + ExchRate + "</td><td>" + Rate + "</td><td>" + Amount + "</td></tr>";

        //            if (ExpenseName == "Own Bike" || ExpenseName == "Own Car")
        //            {
        //                OwnVehicalTotal += Convert.ToDecimal(Amount);
        //            }
        //            else
        //            {
        //                RemainingTotal += Convert.ToDecimal(Amount);
        //            }
        //            //decimal _GrandTotal = OwnVehicalTotal + RemainingTotal;

        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>Grand Total</td><td>" + _GrandTotal + "</td></tr>";
        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>OwnVehicalTotal</td><td>" + OwnVehicalTotal + "</td></tr>";
        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>RemainingTotal</td><td>" + RemainingTotal + "</td></tr>";

        //        }
        //        minDateString = minDate.ToString("dd/MM/yyyy");
        //        maxDateString = maxDate.ToString("dd/MM/yyyy");
        //        decimal _GrandTotal = OwnVehicalTotal + RemainingTotal;

        //        // Append the total rows outside of the loop
        //        //StrOpenConcern += "<tr><td></td><td></td><td></td><td></td><td></td><td>Grand Total</td><td>" + _GrandTotal + "</td></tr>";
        //        //StrOpenConcern += "<tr><td></td><td></td><td></td><td></td><td></td><td>OwnVehicalTotal</td><td>" + OwnVehicalTotal + "</td></tr>";
        //        //StrOpenConcern += "<tr><td></td><td></td><td></td><td></td><td></td><td>RemainingTotal</td><td>" + RemainingTotal + "</td></tr>";
        //        StrOpenConcern += "<tr style='text-align:center'><td></td><td></td><td></td><td></td><td></td><td></td><td>Grand Total</td><td>" + _GrandTotal + "</td></tr>";
        //        StrOpenConcern += "<tr style='text-align:center'><td></td><td></td><td></td><td></td><td></td><td></td><td>OwnVehicalTotal</td><td>" + OwnVehicalTotal + "</td></tr>";
        //        StrOpenConcern += "<tr style='text-align:center'><td></td><td></td><td></td><td></td><td></td><td></td><td>RemainingTotal</td><td>" + RemainingTotal + "</td></tr>";

        //    }
        //    body = body.Replace("[ExpData]", StrOpenConcern);

        //    decimal RemainingTotal_ = 0;
        //    decimal GrandTotal_ = 0;
        //    string StrOpenConcern_ = string.Empty;

        //    DataTable dsGetStamp1 = new DataTable();
        //    dsGetStamp1 = dsHtml.Tables[1];
        //    if (dsGetStamp1.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dsGetStamp1.Rows)
        //        {

        //            string Type = Convert.ToString(dr["Type"]);
        //            string Cost_center = Convert.ToString(dr["Cost_center"]);
        //            string SubJob_No = Convert.ToString(dr["SubJob_No"]);
        //            string SAP_No = Convert.ToString(dr["SAP_No"]);
        //            string Company_Name = Convert.ToString(dr["Company_Name"]);
        //            string TotalAmount = Convert.ToString(dr["TotalAmount"]);

        //            StrOpenConcern_ = StrOpenConcern_ + "<tr style='text-align:center'><td>" + Type + "</td><td>" + Cost_center + "</td><td>" + SubJob_No + "</td><td>" + SAP_No + "</td><td>" + Company_Name + "</td><td>" + TotalAmount + "</td></tr>";
        //            RemainingTotal_ += Convert.ToDecimal(TotalAmount);

        //            //decimal _GrandTotal = OwnVehicalTotal + RemainingTotal;

        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>Grand Total</td><td>" + _GrandTotal + "</td></tr>";
        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>OwnVehicalTotal</td><td>" + OwnVehicalTotal + "</td></tr>";
        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>RemainingTotal</td><td>" + RemainingTotal + "</td></tr>";

        //        }
        //        GrandTotal_ = RemainingTotal_;

        //        // Append the total rows outside of the loop
        //        StrOpenConcern_ += "<tr style='text-align:center'><td></td><td></td><td></td><td></td><td>Grand Total</td><td>" + GrandTotal_ + "</td></tr>";


        //    }

        //    body = body.Replace("[CostExpData]", StrOpenConcern_);

        //    string StrOpenConcern_1 = string.Empty;


        //    DataTable dt = objEV.Get_VoucherDetails_History_ID_Description_pdf(VoucherNo);


        //    if (dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {

        //            string Date = Convert.ToString(dr["Date"]);
        //            string Stage = Convert.ToString(dr["Status"]);
        //            string Approver = Convert.ToString(dr["ApproverName"]);
        //            string Description = Convert.ToString(dr["Description"]);
        //            string TotalAmount = Convert.ToString(dr["Deduct"]);
        //            string ApprovedAmount = Convert.ToString(dr["Amount"]);


        //            StrOpenConcern_1 = StrOpenConcern_1 + "<tr style='text-align:center'><td>" + Date + "</td><td>" + Stage + "</td><td>" + Approver + "</td><td>" + Description + "</td><td>" + TotalAmount + "</td><td>" + ApprovedAmount + "</td></tr>";

        //            //decimal _GrandTotal = OwnVehicalTotal + RemainingTotal;

        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>Grand Total</td><td>" + _GrandTotal + "</td></tr>";
        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>OwnVehicalTotal</td><td>" + OwnVehicalTotal + "</td></tr>";
        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>RemainingTotal</td><td>" + RemainingTotal + "</td></tr>";

        //        }


        //    }

        //    body = body.Replace("[Description]", StrOpenConcern_1);


        //    DataTable dsGetStamp_ = new DataTable();
        //    dsGetStamp_ = dsHtml.Tables[2];


        //    string Empcode = dsHtml.Tables[2].Rows[0]["EmployeeCode"].ToString();
        //    body = body.Replace("[EMPcode]", Empcode);


        //    string Employeement = dsHtml.Tables[0].Rows[0]["Name"].ToString();
        //    body = body.Replace("[Employeement]", Employeement);

        //    string sendforapproval = dsHtml.Tables[0].Rows[0]["IsSendForApproval_Date"].ToString();
        //    body = body.Replace("[sendApproval]", sendforapproval);

        //    string EmployeeGrade = dsHtml.Tables[0].Rows[0]["EmployeeGrade"].ToString();
        //    body = body.Replace("[EmployeeGrade]", EmployeeGrade);

        //    string SapEMPcode = dsHtml.Tables[2].Rows[0]["SAPEmpCode"].ToString();
        //    body = body.Replace("[SapEMPcode]", SapEMPcode);

        //    string Year = "20" + year2 + "-20" + year_.ToString();
        //    body = body.Replace("[Year]", Year);

        //    //string date = dsHtml.Tables[0].Rows[0]["Date"].ToString();
        //    //body = body.Replace("[Date]", date);
        //    body = body.Replace("[Date]", minDateString + " - " + maxDateString);


        //    string salesorderno = dsHtml.Tables[0].Rows[0]["SAP_No"].ToString();
        //    body = body.Replace("[Sales Order No]", salesorderno);

        //    string SAP_Vendor_Code = dsHtml.Tables[2].Rows[0]["SAP_VendorCode"].ToString();
        //    body = body.Replace("[SAP Vendor Code]", SAP_Vendor_Code);



        //    body = body.Replace("[date]", DateTime.Now.ToShortDateString());
        //    //// string VoucherNo_ = dsHtml.Tables[2].Rows[2]["VoucherNo"].ToString(); Commented by Rohini 01/03/2024
        //    string VoucherNo_ = VoucherNo.ToString();
        //    body = body.Replace("[Voucher No]", VoucherNo_);

        //    string replacedvoucherNo = VoucherNo_.Replace("/", "_");

        //    string costcenter = dsHtml.Tables[2].Rows[0]["Cost_center"].ToString();
        //    body = body.Replace("[costcenter]", costcenter);

        //    string EmployeeName = dsHtml.Tables[2].Rows[0]["EmployeeName"].ToString();
        //    body = body.Replace("[Name]", EmployeeName);


        //    string Branch = dsHtml.Tables[2].Rows[0]["Branch"].ToString();
        //    body = body.Replace("[Branch]", Branch);
        //    //body = body.Replace("[Voucher No]", StrOpenConcern);


        //    //PdfPageSize pageSize = PdfPageSize.A4;
        //    //PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
        //    //HtmlToPdf converter = new HtmlToPdf();

        //    //SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(body);
        //    //int PageCount = doc1.Pages.Count;
        //    //body = body.Replace("[PageCount]", ObjModelQuotationMast.AddEnclosures + ' ' + "(Refer Page " + Convert.ToString(PageCount) + " Of " + Convert.ToString(PageCount) + " )");
        //    //strs.Append(body);
        //    PdfPageSize pageSize = PdfPageSize.A4;
        //    PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
        //    HtmlToPdf converter = new HtmlToPdf();
        //    SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(body);
        //    int PageCount = doc1.Pages.Count;
        //    body = body.Replace("[PageCount]", "(Refer Page " + Convert.ToString(PageCount) + " Of " + Convert.ToString(PageCount) + " )");
        //    strs.Append(body);


        //    converter.Options.MaxPageLoadTime = 240;
        //    converter.Options.PdfPageSize = pageSize;
        //    converter.Options.PdfPageOrientation = pdfOrientation;
        //    converter.Options.SecurityOptions.CanAssembleDocument = true;
        //    converter.Options.SecurityOptions.CanCopyContent = true;
        //    converter.Options.SecurityOptions.CanEditAnnotations = true;
        //    converter.Options.SecurityOptions.CanEditContent = true;
        //    converter.Options.SecurityOptions.CanFillFormFields = true;
        //    converter.Options.SecurityOptions.CanPrint = true;

        //    string path = Server.MapPath("~/QuotationHtml");
        //    PdfDocument doc = converter.ConvertHtmlString(body);
        //    doc.Save(path + '\\' + replacedvoucherNo + ".pdf");
        //    doc.Close();

        //    byte[] fileBytes = System.IO.File.ReadAllBytes(path + '\\' + replacedvoucherNo + ".pdf");


        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, replacedvoucherNo + ".pdf");

        //}
        [HttpGet]
        public JsonResult GetDataInExcel(string FromDate, string ToDate)
        {
            try
            {
                string role = Session["RoleName"].ToString(); // "Approval";
                string userId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

                DataSet dsHtml = objEI.GetDatewiseApprovedlist(FromDate, ToDate, role, userId);

                using (var excelPackage = new ExcelPackage())
                {
                    // Create the worksheet
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    // Add the headers
                    int headerRow = 1;
                    for (int col = 0; col < dsHtml.Tables[0].Columns.Count; col++)
                    {
                        sheet.Cells[headerRow, col + 1].Value = dsHtml.Tables[0].Columns[col].ColumnName;
                        var headerCell = sheet.Cells[headerRow, col + 1];
                        headerCell.Style.Font.Bold = true;
                        headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCell.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        headerCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }

                    // Add the data rows
                    int dataRowStart = headerRow + 1;
                    for (int row = 0; row < dsHtml.Tables[0].Rows.Count; row++)
                    {
                        for (int col = 0; col < dsHtml.Tables[0].Columns.Count; col++)
                        {
                            sheet.Cells[dataRowStart + row, col + 1].Value = dsHtml.Tables[0].Rows[row][col].ToString();
                        }
                    }

                    sheet.Cells.AutoFitColumns();

                    // Convert the Excel package to a byte array
                    byte[] excelBytes = excelPackage.GetAsByteArray();

                    // Set the response content type and headers
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment; filename=ApprovedList.xlsx");

                    // Write the Excel data to the response
                    Response.BinaryWrite(excelBytes);
                    Response.Flush();
                    Response.End();
                }

                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public ActionResult validateDate_MAxDate(string Voucherno)
        {
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objEI.GetVoucherMaxDate(Voucherno);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelayRemark(string Voucherno, string Remarks)
        {
            string JsonString = "";
            DataSet ds = new DataSet();
            ds = objEI.Voucher_DelayRemark(Voucherno, Remarks); // fill dataset  
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                JsonString = JsonConvert.SerializeObject("Error");
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delayflag(string Voucherno)
        {
            string JsonString = "";
            DataSet ds = new DataSet();
            ds = objEI.Voucher_delayFlag(Voucherno); // fill dataset  
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                JsonString = JsonConvert.SerializeObject("Error");
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveSendForApproval()
        {
            return View();
        }


        public ActionResult RemoveApproval(string Voucherno)
        {
            string jsonstring;
            DataTable dt = new DataTable();
            dt = objEI.RemoveExpenseApproval(Voucherno);
            if (dt!=null&&dt.Rows.Count > 0)
            {
                jsonstring = JsonConvert.SerializeObject(dt);
                SendApprovalRemovalMail(Voucherno);
            }
            else
            {
                jsonstring = JsonConvert.SerializeObject("Error");
            }
            return Json(jsonstring, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetVoucherList_ForRemoveApproval(string Voucherno)
        {
            string Role = Session["RoleName"].ToString();

            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

            DataSet ds = new DataSet();            
            ds = objEI.Getuservoucherlist_Remove(Voucherno, UserId);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public void SendApprovalRemovalMail(string voucherno)
        {

            DataTable dt = new DataTable();
            dt= objEI.GetApprovalId(voucherno);


            StringBuilder MailBody = new StringBuilder();
            try
            {

                if (dt.Rows.Count > 0)
                {
                    string Approval1 = dt.Rows[0]["ApprovalOneEmail"].ToString();
                    string Approval2 = dt.Rows[0]["ApprovalName_2"].ToString();
                    string PCH = dt.Rows[0]["PCH_Name"].ToString();
                    string Createdby = dt.Rows[0]["CreatedByEmail"].ToString();
                    string Username = dt.Rows[0]["Name"].ToString();
                    string RemovedApprovalBy = dt.Rows[0]["RemovedApprovalBy"].ToString();
                    
                    string ccList = $"rohini@tuv-nord.com;nikita.yadav@tuvindia.co.in;pshrikant@tuv-nord.com;skashyap@tuv-nord.com;{Approval1};{Approval2};{PCH};";
                    string toList = $"{Createdby}";
                    MailMessage msg = new MailMessage();
                    string fromEmail = ConfigurationManager.AppSettings["MailFrom"];
                    string smtpHost = ConfigurationManager.AppSettings["SmtpServer"];
                    if (Approval1 != "")
                    {
                        MailBody.Append("<div style='font-family: Arial, sans-serif; line-height: 1.6;'>");
                        MailBody.Append("<div style='width: 80%; margin: auto; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>");
                        MailBody.Append($"<p>Dear {Username},</p>");
                        MailBody.Append($"<p>This is to inform you that the approval for the voucher number <strong>{voucherno}</strong> has been <strong>removed</strong> by <strong>{RemovedApprovalBy}</strong>.</p>");
                        MailBody.Append("<p>Please review the changes and take any necessary actions accordingly.</p>");
                        MailBody.Append("<br />");
                        MailBody.Append("<br />");
                        MailBody.Append("<p>Best regards,</p>");
                        MailBody.Append("<p><strong>TUVI</strong></p>");

                        MailBody.Append("</div>");
                        MailBody.Append("</div>");

                    }


                    msg.From= new MailAddress(fromEmail, "TIIMES Notification");
                    msg.Subject = $"Voucher {voucherno} Removed from Approval Workflow";
                    msg.Body = MailBody.ToString();
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.Normal;
               

                    foreach (var email in toList.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                        msg.To.Add(email.Trim());

                    foreach (var email in ccList.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                        msg.CC.Add(email.Trim());

                    SmtpClient client = new SmtpClient
                    {
                        Host = smtpHost,
                        Port = int.Parse(ConfigurationManager.AppSettings["Port"] ?? "587"),
                        EnableSsl = true,
                        Credentials = new System.Net.NetworkCredential(
                            ConfigurationManager.AppSettings["User"],
                            ConfigurationManager.AppSettings["Password"]
                        )
                    };

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public ActionResult TravelForm()
        {
            return View();
        }
    }
}