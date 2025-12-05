
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using System.Drawing;
namespace TuvVision.Controllers
{
    public class OPEController : Controller
    {
        // GET: OPE

        DalOPEAutomated objDMOR = new DalOPEAutomated();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OPE_Report_Generation()
        {
            return View();
        }

        public JsonResult Get_OPE_SearchData(string Month, string Year)
        {
            string JsonString = "";
            DataSet ds = new DataSet();

            Session["Month"] = Month;
            Session["Year"] = Year;
            ds = objDMOR.GetData(Month, Year);
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OPE_Approval_Generated(string month, string year)
        {
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View();
        }
        public JsonResult GetOPApprovalList(string month, string year)

        {
            DataSet ds = new DataSet();
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId_ = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

           
                ds = objDMOR.GetOPApprovalList("",month,year);
           
            //ds = objDMOR.GetOPApprovalList("");
            string JsonString = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOPApprovalListButtonwise(string Branch,string month,string year)
        {
            DataSet ds = new DataSet();
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId_ = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            ds = objDMOR.GetOPApprovalListButtonwise(Branch,month,year);
            string JsonString = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
                //JsonString = JsonConvert.SerializeObject(ds.Tables[1]);


            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOPBulk_ApprovalList(string Data1, string _OP_Number)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(Data1);
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dt);
            DataSet ds = new DataSet();
            ds = objDMOR.GetOPBulk_ApprovalList(ds1, "", _OP_Number);
            string JsonString = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFolders()
       {
            DataTable dt = objDMOR.GetStatusOpApproval();

            // Serialize DataTable to JSON string
            string json = JsonConvert.SerializeObject(dt);

            // Return as JsonResult
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOpApproval()
        {
            return View();
        }



        [HttpGet]
        public ActionResult Export_IndexOpeReport_(string month, string year)
        {
            try
            {

                DataSet dsHtml = objDMOR.GetAccountantExportData(month, year);

                using (var excelPackage = new ExcelPackage())
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    sheet.Cells["A1"].Value = "";
                    var cell = sheet.Cells["A2"];
                    cell.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["B1"].Value = "";
                    var cell12 = sheet.Cells["B1"];
                    cell12.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell12.Style.Font.Bold = true;
                    cell12.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell12.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["C1"].Value = "";
                    var cell13 = sheet.Cells["C1"];
                    cell13.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell13.Style.Font.Bold = true;
                    cell13.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell13.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["D1"].Value = "";
                    var cell14 = sheet.Cells["D1"];
                    cell14.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell14.Style.Font.Bold = true;
                    cell14.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell14.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["E1"].Value = "";
                    var cell15 = sheet.Cells["E1"];
                    cell15.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell15.Style.Font.Bold = true;
                    cell15.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell15.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["F1"].Value = "Out of Pocket Expenses Report";
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

                    sheet.Cells["A2"].Value = "Number";
                    var cell19 = sheet.Cells["A2"];
                    cell19.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell19.Style.Font.Bold = true;
                    cell19.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell19.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["B2"].Value = "Month";
                    var cell19_ = sheet.Cells["B2"];
                    cell19_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell19_.Style.Font.Bold = true;
                    cell19_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell19_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["C2"].Value = "Inspector Name";
                    var cell20 = sheet.Cells["C2"];
                    cell20.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell20.Style.Font.Bold = true;
                    cell20.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell20.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    sheet.Cells["D2"].Value = "Branch";
                    var cell2 = sheet.Cells["D2"];
                    cell2.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell2.Style.Font.Bold = true;
                    cell2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    sheet.Cells["E2"].Value = "Employee Code";
                    var cell_ = sheet.Cells["E2"];
                    cell_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell_.Style.Font.Bold = true;
                    cell_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //sap employee code
                    sheet.Cells["F2"].Value = "Sap Employee Code";
                    var cell2_ = sheet.Cells["F2"];
                    cell2_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell2_.Style.Font.Bold = true;
                    cell2_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell2_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    sheet.Cells["G2"].Value = "Cost Center";
                    var cell1 = sheet.Cells["G2"];
                    cell1.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell1.Style.Font.Bold = true;
                    cell1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["H2"].Value = "Total Amount(INR)";
                    var cell10 = sheet.Cells["H2"];
                    cell10.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell10.Style.Font.Bold = true;
                    cell10.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell10.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["I2"].Value = "Approved Amount (INR)";
                    var cell11 = sheet.Cells["I2"];
                    cell11.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell11.Style.Font.Bold = true;
                    cell11.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell11.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["J2"].Value = "PCH Remark";
                    var cell5 = sheet.Cells["J2"];
                    cell5.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell5.Style.Font.Bold = true;
                    cell5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["K2"].Value = "Admin QA Remark";
                    var cell6 = sheet.Cells["K2"];
                    cell6.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell6.Style.Font.Bold = true;
                    cell6.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell6.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["L2"].Value = "CH Remark";
                    var cell7 = sheet.Cells["L2"];
                    cell7.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell7.Style.Font.Bold = true;
                    cell7.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell7.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    sheet.Cells["M2"].Value = "Complaint";
                    var cell8 = sheet.Cells["M2"];
                    cell8.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell8.Style.Font.Bold = true;
                    cell8.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell8.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["N2"].Value = "Days";
                    var cell8_ = sheet.Cells["N2"];
                    cell8_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell8_.Style.Font.Bold = true;
                    cell8_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell8_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["O2"].Value = "Available Days";
                    var cell9_ = sheet.Cells["O2"];
                    cell9_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell9_.Style.Font.Bold = true;
                    cell9_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell9_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["P2"].Value = "Utilize";
                    var cell9 = sheet.Cells["P2"];
                    cell9.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell9.Style.Font.Bold = true;
                    cell9.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell9.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["Q2"].Value = "PCH Deduction Manday";
                    var cell11_ = sheet.Cells["Q2"];
                    cell11_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell11_.Style.Font.Bold = true;
                    cell11_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell11_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["R2"].Value = "AdminQA Deduction Manday";
                    var cell12_ = sheet.Cells["R2"];
                    cell12_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell12_.Style.Font.Bold = true;
                    cell12_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell12_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    sheet.Cells["S2"].Value = "CH Deduction Manday";
                    var cell13_ = sheet.Cells["S2"];
                    cell13_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell13_.Style.Font.Bold = true;
                    cell13_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell13_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    int j = 0;
                    decimal RemainingTotal = 0;
                    decimal OwnVehicalTotal = 0;
                    decimal RemainingTotalamount = 0;
                    decimal Working_day_month = 0;
                    decimal utilizeTotal = 0;
                    int utilizeCount = 0;
                    for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
                    {
                        sheet.Cells[3 + i, 1].Value = dsHtml.Tables[0].Rows[i]["op_number"].ToString();
                        sheet.Cells[3 + i, 2].Value = dsHtml.Tables[0].Rows[i]["MonthName"].ToString();

                        sheet.Cells[3 + i, 3].Value = dsHtml.Tables[0].Rows[i]["InspectorName"].ToString();
                        sheet.Cells[3 + i, 4].Value = dsHtml.Tables[0].Rows[i]["Branch_Name"].ToString();
                        sheet.Cells[3 + i, 5].Value = dsHtml.Tables[0].Rows[i]["EmployeeCode"].ToString();
                        sheet.Cells[3 + i, 6].Value = dsHtml.Tables[0].Rows[i]["SapEmpCode"].ToString();

                        sheet.Cells[3 + i, 7].Value = dsHtml.Tables[0].Rows[i]["CostCentre"].ToString();

                        sheet.Cells[3 + i, 8].Value = dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString();
                        sheet.Cells[3 + i, 9].Value = dsHtml.Tables[0].Rows[i]["Current_Approved_Amount"].ToString();
                        if (sheet.Cells[3 + i, 8].Value.ToString() != sheet.Cells[3 + i, 9].Value.ToString())   //added by nikita on 15012024
                        {
                            using (var range = sheet.Cells[3 + i, 1, 3 + i, 14])
                            {
                                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSalmon);
                            }
                        }
                        //sheet.Cells[3 + i, 6].Value = dsHtml.Tables[0].Rows[i]["RoleName"].ToString();
                       
                        sheet.Cells[3 + i, 10].Value = dsHtml.Tables[0].Rows[i]["Pch_Description"].ToString();
                        sheet.Cells[3 + i, 11].Value = dsHtml.Tables[0].Rows[i]["AdminOA_Description"].ToString();
                        sheet.Cells[3 + i, 12].Value = dsHtml.Tables[0].Rows[i]["Ch_Approval_description"].ToString();
                        sheet.Cells[3 + i, 13].Value = dsHtml.Tables[0].Rows[i]["Working_ManDays"].ToString();
                        sheet.Cells[3 + i, 14].Value = dsHtml.Tables[0].Rows[i]["No_of_complaints"].ToString();
                        sheet.Cells[3 + i, 15].Value = dsHtml.Tables[0].Rows[i]["Working_Days"].ToString();
                        sheet.Cells[3 + i, 16].Value = dsHtml.Tables[0].Rows[i]["Utilize"].ToString();
                        sheet.Cells[3 + i, 17].Value = dsHtml.Tables[0].Rows[i]["DeductMandays_Pch"].ToString();
                        sheet.Cells[3 + i, 18].Value = dsHtml.Tables[0].Rows[i]["DeductMandays_Admin"].ToString();
                        sheet.Cells[3 + i, 19].Value = dsHtml.Tables[0].Rows[i]["DeductMandays_Ch"].ToString();

                        RemainingTotalamount = RemainingTotalamount + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString());


                        RemainingTotal = RemainingTotal + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["Current_Approved_Amount"].ToString());
                        Working_day_month = Working_day_month + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["Working_Days"].ToString());

                        if (dsHtml.Tables[0].Rows[i]["Utilize"] != DBNull.Value &&
                              !string.IsNullOrWhiteSpace(dsHtml.Tables[0].Rows[i]["Utilize"].ToString()))
                        {
                            decimal u = Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["Utilize"]);
                            utilizeTotal += u;
                            utilizeCount++;
                        }



                        var range_ = sheet.Cells["A" + (3 + i) + ":P" + (3 + i)];
                        range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Top.Color.SetColor(Color.Black);
                        range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Left.Color.SetColor(Color.Black);
                        range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Right.Color.SetColor(Color.Black);
                        range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Bottom.Color.SetColor(Color.Black);
                        j = 3 + i;
                    }
                    decimal _GrandTotal = RemainingTotal;
                    decimal _GrandTotalAmount = RemainingTotalamount;

                    decimal Working_daysMonth = Working_day_month;
                    decimal avgUtilize = utilizeCount > 0
                           ? Math.Round(utilizeTotal / utilizeCount, 2)
                           : 0;



                    sheet.Cells["G" + (j + 1)].Value = "Grand Total:";
                    //sheet.Cells["F" + (j + 2)].Value = "Remaining Expenses total:";

                    sheet.Cells["H" + (j + 1)].Value = _GrandTotal;
                    sheet.Cells["I" + (j + 1)].Value = _GrandTotalAmount;
                    sheet.Cells["O" + (j + 1)].Value = Working_daysMonth;
                    sheet.Cells["P" + (j + 1)].Value = avgUtilize;

                    //sheet.Cells["G" + (j + 2)].Value = RemainingTotal;


                    var range1 = sheet.Cells["E" + (j + 1) + ":P" + (j + 1)];
                    range1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Top.Color.SetColor(Color.Black);
                    range1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Left.Color.SetColor(Color.Black);
                    range1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Right.Color.SetColor(Color.Black);
                    range1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Bottom.Color.SetColor(Color.Black);

                    var range_1 = sheet.Cells["A2:S2"];
                    range_1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range_1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    range_1.Style.Font.Size = 12;
                    range_1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Top.Color.SetColor(Color.Black);
                    range_1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Left.Color.SetColor(Color.Black);
                    range_1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Right.Color.SetColor(Color.Black);
                    range_1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Bottom.Color.SetColor(Color.Black);

                    sheet.Cells.AutoFitColumns();
                    byte[] excelBytes = excelPackage.GetAsByteArray();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment; filename=OPReport" + DateTime.Now + "_.xlsx");
                    Response.BinaryWrite(excelBytes);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View();

        }

    }
}