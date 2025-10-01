using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;
using OfficeOpenXml;
using SelectPdf;
using NonFactors.Mvc.Grid;
using OfficeOpenXml.Style;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;

namespace TuvVision.Controllers
{
    public class ExpenseVoucherController : Controller
    {
        // GET: ExpenseVoucher
        DALExpenseItem objEI = new DALExpenseItem();
        DALExpenseVoucher objEV = new DALExpenseVoucher();
        ExpenseItem objEIModel = new ExpenseItem();
        string Result = string.Empty;
        public ActionResult ExpenseVoucher()
        {
            List<ExpenseItem> lmd = new List<ExpenseItem>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objEV.GetDataById(objEIModel);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new ExpenseItem
                    {
                        PKExpenseId = Convert.ToInt32(dr["PKExpenseId"]),
                        ExpenseType = Convert.ToString(dr["ExpenseType"]),
                        Date = Convert.ToString(dr["Date"]),
                        TotalAmount = Convert.ToDouble(dr["TotalAmount"])


                    });
                }
                //ViewBag.ExpenseVoucher = lmd;
                ViewData["ExpenseVoucher"] = lmd;
            }
            else
            {

            }

            return View();
        }


        [HttpPost]
        public ActionResult ExpenseVoucher(ExpenseItem objEIModel)
        {

            List<ExpenseItemList> lmd = new List<ExpenseItemList>();  // creating list of model.  
            DataSet ds = new DataSet();
            if (objEIModel.FromDate != null && objEIModel.ToDate != null)
            {
                ds = objEV.GetDataByDate(objEIModel);
            }
            else
            {
                ds = objEV.GetDataById(objEIModel);
            }

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new ExpenseItemList
                    {
                        LPKExpenseId = Convert.ToInt32(dr["PKExpenseId"]),
                        LExpenseType = Convert.ToString(dr["ExpenseType"]),
                        LDate = Convert.ToString(dr["Date"]),
                        LTotalAmount = Convert.ToDouble(dr["TotalAmount"])


                    });
                }
                ViewData["ExpenseVoucher"] = lmd;

            }
            else
            {

            }

            return View();


        }


        [HttpPost]
        public ActionResult GenerateExpenseVoucher(ExpenseItem objEIModel, FormCollection fr, ExpenseItemList EIL)
        {
            string CommaSepPKExpenseId;
            string[] city;
            string[] NamesArray = null;
            Double VoucherTotalAmount;
            List<ExpenseItemList> lmd = new List<ExpenseItemList>();


            foreach (var Item in objEIModel.ExpenseItemList)
            {
                if (Item.Checkbox1 == true)
                {
                    #region Store list of primary key
                    lmd.Add(new ExpenseItemList
                    {
                        LPKExpenseId = Item.LPKExpenseId
                    });
                    NamesArray = lmd.Select(c => c.LPKExpenseId.ToString()).ToArray();
                    #endregion

                    objEIModel.TotalAmount += Item.LTotalAmount;


                    objEIModel.PKExpenseId = Item.LPKExpenseId;
                    Result = objEV.GenerateExpenseVoucher(objEIModel);
                }
            }

            #region Generate voucher insert to Voucher
            if (NamesArray != null)
            {
                VoucherTotalAmount = Convert.ToDouble(objEIModel.TotalAmount);
                CommaSepPKExpenseId = String.Join(",", NamesArray);
                Result = objEV.InsertToVoucher(CommaSepPKExpenseId, VoucherTotalAmount);
            }
            #endregion

            //CommaSepPKExpenseId = String.Join(",", ViewBag.l);
            return RedirectToAction("VoucherStatus");
        }

        public ActionResult VoucherStatus()
        {
            List<ExpenseItem> lmd = new List<ExpenseItem>();  // creating list of model.  
            DataSet ds = new DataSet();

            Session["GetExcelData"] = "Yes";

            ds = objEV.GetVoucherStatus(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new ExpenseItem
                {
                    //Date = Convert.ToDateTime(dr["Date"]),
                    //VoucherGenerated = Convert.ToString(dr["VoucherGenerated"]),
                    //ActivityType = Convert.ToString(dr["ActivityType"]),
                    //ApprovalStatus1 = Convert.ToString(dr["ApprovalStatus1"]),
                    //ApprovalStatus2 = Convert.ToString(dr["ApprovalStatus2"]),
                    //TotalAmount = Convert.ToDouble(dr["TotalAmount"]),
                    //ApprovedAmount = Convert.ToDouble(dr["ApprovedAmount"]),
                    //ExpenseType = Convert.ToString(dr["ExpenseType"]),
                    FKExpenseId = Convert.ToString(dr["FKExpenseId"]),
                    Date = Convert.ToString(dr["CreatedDate"]),
                    VoucherId = Convert.ToInt32(dr["VoucherId"]),
                    /// EndCity = Convert.ToString(dr["EndCity"]),

                    ApprovalStatus1 = Convert.ToString(dr["ApprovalStatus1"]),
                    ApprovalStatus2 = Convert.ToString(dr["ApprovalStatus2"]),
                    TotalAmount = Convert.ToDouble(dr["VTotalAmount"]),
                    ApprovedAmount = Convert.ToDouble(dr["VApprovedAmount"]),
                    TransferToFI = Convert.ToString(dr["TransferToFI"]),

                });
            }
            objEIModel.LstDashboard = lmd;
            return View(objEIModel);
        }

        public ActionResult Voucher(string FKVoucherId)
        {


            List<ExpenseItem> lmd = new List<ExpenseItem>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objEV.Voucher(FKVoucherId); // fill dataset  
            Session["FKVoucherId"] = FKVoucherId;
            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new ExpenseItem
                {
                    Date = Convert.ToString(dr["Date"]),
                    ExpenseType = Convert.ToString(dr["ExpenseType"]),
                    //Country = Convert.ToString(dr["Country"]),
                    Currency = Convert.ToString(dr["Currency"]),
                    //TotalAmount = Convert.ToInt32(dr["TotalAmount"]),
                    ExchRate = Convert.ToInt32(dr["ExchRate"]),
                    Description = Convert.ToString(dr["Description"]),
                    //EndCity = Convert.ToString(dr["EndCity"]),
                    Amount = Convert.ToInt32(dr["Amount"]),


                });
            }
            objEIModel.LstDashboard = lmd;

            return View(objEIModel);

            //  return View(lmd.ToList());
        }

        [HttpGet]
        public JsonResult ValidateSendForapproval(string VoucherNo)
        {
            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            System.Data.DataTable dt = objEI.validate_data(VoucherNo);
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

        #region
        //[HttpGet]
        //        public ActionResult ExportIndex(ExpenseItem c)
        //        {
        //            // Using EPPlus from nuget
        //            using (ExcelPackage package = new ExcelPackage())
        //            {
        //                Int32 row = 15;
        //                Int32 col = 1;

        //                package.Workbook.Worksheets.Add("Data");
        //                IGrid<ExpenseItem> grid = CreateExportableGrid(c);



        //                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

        //                sheet.Cells["D2"].Value = "TUV India Private Ltd.";
        //                sheet.Cells["A4"].Value = "Name";
        //                sheet.Cells["A5"].Value = "Employee Code";
        //                sheet.Cells["A6"].Value = "Cost Centre";

        //                sheet.Cells["B4"].Value = Session["fullName"].ToString();
        //                sheet.Cells["B5"].Value = Session["EmpCode"].ToString();
        //                sheet.Cells["B6"].Value = Session["costcentre"].ToString();

        //                sheet.Cells["D8"].Value = "Trip Detail";
        //                sheet.Cells["A10"].Value = "Trip No";
        //                sheet.Cells["B10"].Value = "";
        //                sheet.Cells["A11"].Value = "Start Date";
        //                sheet.Cells["B11"].Value = Session["FromDate"].ToString();
        //                sheet.Cells["A12"].Value = "Sales Order No";
        //                sheet.Cells["B12"].Value = "";
        //                sheet.Cells["A12"].Value = "Cost Center";
        //                sheet.Cells["B12"].Value = "";

        //                sheet.Cells["C11"].Value = "End Date";
        //                sheet.Cells["D11"].Value = Session["ToDate"].ToString();
        //                sheet.Cells["C12"].Value = "Item No";
        //                sheet.Cells["D12"].Value = "";
        //                sheet.Cells["C13"].Value = "Reason";
        //                sheet.Cells["D13"].Value = "";



        //                int colcount = 0;
        //                foreach (IGridColumn column in grid.Columns)
        //                {
        //                    sheet.Cells[row, col].Value = column.Title;
        //                    sheet.Column(col++).Width = 18;
        //                    column.IsEncoded = false;
        //                    colcount++;
        //                }

        //                //sheet.Cells[8,1,8, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;

        //                //sheet.Cells[8, 1, 8, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

        //                sheet.Cells[15, 1, 15, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;

        //                sheet.Cells[15, 1, 15, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);


        //                //Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

        //                double finalAmount = 0;
        //                row++;
        //                foreach (IGridRow<ExpenseItem> gridRow in grid.Rows)
        //                {
        //                    col = 1;
        //                    foreach (IGridColumn column in grid.Columns)
        //                    {
        //                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
        //                    }
        //                    finalAmount = finalAmount + Convert.ToDouble(sheet.Cells[row, colcount].Value.ToString());   // Convert.ToDouble(grid.Rows[row]["INRamount"]);

        //                    row++;
        //                }
        //                row++;
        //                sheet.Cells[row, 1, row, colcount].Style.Border.Top.Style = ExcelBorderStyle.Thick;

        //                sheet.Cells[row++, colcount].Value = finalAmount;


        //                #region 2nd Table


        //                //Int32 row1 = row + 1;
        //                //Int32 col1 = 1;
        //                //IGrid<ExpenseItem> grid1 = CreateExportableGrid1(c);
        //                //int colcount1 = 0;
        //                //foreach (IGridColumn column1 in grid1.Columns)
        //                //{
        //                //    sheet.Cells[row1, col1].Value = column1.Title;
        //                //    sheet.Column(col1++).Width = 18;
        //                //    column1.IsEncoded = false;
        //                //    colcount1++;
        //                //}



        //                //double finalAmount1 = 0;
        //                //row1++;
        //                //foreach (IGridRow<ExpenseItem> gridRow1 in grid1.Rows)
        //                //{
        //                //    col1 = 1;
        //                //    foreach (IGridColumn column in grid1.Columns)
        //                //    {
        //                //        sheet.Cells[row1, col1++].Value = column.ValueFor(gridRow1);
        //                //    }
        //                //   // finalAmount1 = finalAmount1 + Convert.ToDouble(sheet.Cells[row1, colcount1].Value.ToString());   // Convert.ToDouble(grid.Rows[row]["INRamount"]);

        //                //    row1++;
        //                //}
        //                //row1++;
        //                //sheet.Cells[row1, 1, row1, colcount1].Style.Border.Top.Style = ExcelBorderStyle.Thick;

        //                //sheet.Cells[row1++, colcount1].Value = finalAmount1;

        //                int rowI = row + 1;
        //                string strRow = "D" + rowI;

        //                sheet.Cells[rowI, 2].Value = "COST ASSIGNMENT.";

        //                row = row + 2;
        //                col = 1;
        //                IGrid<ExpenseItem> grid1 = CreateExportableGrid1(c);
        //                int colcount1 = 0;
        //                foreach (IGridColumn column1 in grid1.Columns)
        //                {
        //                    sheet.Cells[row, col].Value = column1.Title;
        //                    sheet.Column(col++).Width = 18;
        //                    column1.IsEncoded = false;
        //                    colcount1++;
        //                }



        //                double finalAmount1 = 0;
        //                row++;
        //                foreach (IGridRow<ExpenseItem> gridRow1 in grid1.Rows)
        //                {
        //                    col = 1;
        //                    foreach (IGridColumn column in grid1.Columns)
        //                    {
        //                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow1);
        //                    }
        //                    finalAmount1 = finalAmount1 + Convert.ToDouble(sheet.Cells[row, colcount1].Value.ToString());   // Convert.ToDouble(grid.Rows[row]["INRamount"]);

        //                    row++;
        //                }
        //                row++;
        //                sheet.Cells[row, 1, row, colcount1].Style.Border.Top.Style = ExcelBorderStyle.Thick;

        //                sheet.Cells[row++, colcount1].Value = finalAmount1;
        //                #endregion






        //                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
        //            }
        //        }
        //        private IGrid<ExpenseItem> CreateExportableGrid(ExpenseItem c)
        //        {
        //            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
        //            IGrid<ExpenseItem> grid = new Grid<ExpenseItem>(GetData(c));
        //            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

        //            //grid.Columns.Add(model => model.VoucherId).Titled("Voucher Generated");

        //            grid.Columns.Add(model => model.Date).Titled("Date");
        //            grid.Columns.Add(model => model.SAPNo).Titled("SAP No");
        //            grid.Columns.Add(model => model.SubJobNo).Titled("Job No");
        //            grid.Columns.Add(model => model.ExpenseType).Titled("Expense Name");
        //            grid.Columns.Add(model => model.Country).Titled("Country");
        //            grid.Columns.Add(model => model.Currency).Titled("Currency");
        //            grid.Columns.Add(model => model.ExchRate).Titled("ExchRate");

        //            grid.Columns.Add(model => model.Description).Titled("Description");
        //            grid.Columns.Add(model => model.INRAmount).Titled("INRAmount");

        //            grid.Pager = new GridPager<ExpenseItem>(grid);
        //            grid.Processors.Add(grid.Pager);
        //            grid.Pager.RowsPerPage = objEIModel.LstDashboard.Count;

        //            foreach (IGridColumn column in grid.Columns)
        //            {
        //                column.Filter.IsEnabled = true;
        //                column.Sort.IsEnabled = true;
        //            }


        //            return grid;
        //        }

        //        private IGrid<ExpenseItem> CreateExportableGrid1(ExpenseItem c)
        //        {
        //            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
        //            IGrid<ExpenseItem> grid1 = new Grid<ExpenseItem>(GetData1(c));
        //            grid1.ViewContext = new ViewContext { HttpContext = HttpContext };

        //            //grid.Columns.Add(model => model.VoucherId).Titled("Voucher Generated");

        //            grid1.Columns.Add(model => model.CostCenter).Titled("CostCenter");
        //            grid1.Columns.Add(model => model.SAPNo).Titled("SAPNo");
        //            grid1.Columns.Add(model => model.CompanyName).Titled("CompanyName");
        //            grid1.Columns.Add(model => model.Amount).Titled("INRAmount");

        //            grid1.Pager = new GridPager<ExpenseItem>(grid1);
        //            grid1.Processors.Add(grid1.Pager);
        //            grid1.Pager.RowsPerPage = objEIModel.LstCostDistribution.Count;

        //            foreach (IGridColumn column in grid1.Columns)
        //            {
        //                column.Filter.IsEnabled = true;
        //                column.Sort.IsEnabled = true;
        //            }


        //            return grid1;
        //        }

        //        public List<ExpenseItem> GetData(ExpenseItem c)
        //        {
        //            List<ExpenseItem> lmd = new List<ExpenseItem>();
        //            List<ExpenseItem> lmd1 = new List<ExpenseItem>(); // creating list of model.  
        //            DataSet ds = new DataSet();

        //            Session["GetExcelData"] = "Yes";


        //            if (Session["FKVoucherId"] != null)
        //            {
        //                ds = objEV.Voucher(Session["FKVoucherId"].ToString());
        //            }
        //            else
        //            {
        //                ds = objEV.GetVoucherStatus();
        //            }

        //            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
        //            {
        //                lmd.Add(new ExpenseItem
        //                {
        //                    Date = Convert.ToString(dr["Date"]),
        //                    ExpenseType = Convert.ToString(dr["ExpenseType"]),
        //                    Country = Convert.ToString(dr["City"]),
        //                    Currency = Convert.ToString(dr["Currency"]),
        //                    Amount = Convert.ToDouble(dr["Amount"]),
        //                    ExchRate = Convert.ToDouble(dr["ExchRate"]),
        //                    Description = Convert.ToString(dr["Description"]),
        //                    INRAmount = Convert.ToDouble(dr["INRAmount"]),

        //                    SAPNo = Convert.ToString(dr["SAP_No"]),
        //                    SubJobNo = Convert.ToString(dr["SubJob_No"]),
        //                });
        //            }
        //            objEIModel.LstDashboard = lmd;



        //            Session["FromDate"] = Convert.ToString(ds.Tables[2].Rows[0]["firstDate"]);
        //            Session["ToDate"] = Convert.ToString(ds.Tables[2].Rows[0]["LastDate"]);

        //            //foreach (DataRow dr in ds.Tables[1].Rows) // loop for adding add from dataset to list<modeldata>  
        //            //{
        //            //    lmd.Add(new ExpenseItem
        //            //    {

        //            //        CostCenter = Convert.ToString(dr["Cost_Center"]),
        //            //        SAPNo = Convert.ToString(dr["SAP_No"]),
        //            //        SubJobNo = Convert.ToString(dr["SubJob_No"]),
        //            //        CompanyName = Convert.ToString(dr["Company_Name"]),
        //            //        Amount = Convert.ToDouble(dr["Amount"]),


        //            //    });
        //            //}
        //            //objEIModel.LstCostDistribution = lmd1;



        //            return objEIModel.LstDashboard;
        //            //return  (lmd, lmd1);
        ////return objEIModel;
        //        }





        //        public List<ExpenseItem> GetData1(ExpenseItem c)
        //        {
        //            List<ExpenseItem> lmd = new List<ExpenseItem>();
        //            List<ExpenseItem> lmd1 = new List<ExpenseItem>(); // creating list of model.  
        //            DataSet ds = new DataSet();

        //            Session["GetExcelData"] = "Yes";


        //            if (Session["FKVoucherId"] != null)
        //            {
        //                ds = objEV.Voucher(Session["FKVoucherId"].ToString());
        //            }
        //            else
        //            {
        //                ds = objEV.GetVoucherStatus();
        //            }

        //            //foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
        //            //{
        //            //    lmd.Add(new ExpenseItem
        //            //    {



        //            //        Date = Convert.ToString(dr["Date"]),
        //            //        ExpenseType = Convert.ToString(dr["ExpenseType"]),
        //            //        Country = Convert.ToString(dr["Country"]),
        //            //        Currency = Convert.ToString(dr["Currency"]),
        //            //        Amount = Convert.ToDouble(dr["Amount"]),
        //            //        ExchRate = Convert.ToDouble(dr["ExchRate"]),
        //            //        Description = Convert.ToString(dr["Description"]),
        //            //        INRAmount = Convert.ToDouble(dr["INRAmount"]),



        //            //    });
        //            //}
        //            //objEIModel.LstDashboard = lmd;


        //            foreach (DataRow dr in ds.Tables[1].Rows) // loop for adding add from dataset to list<modeldata>  
        //            {
        //                lmd1.Add(new ExpenseItem
        //                {

        //                    CostCenter = Convert.ToString(dr["Cost_Center"]),
        //                    SAPNo = Convert.ToString(dr["SAP_No"]),
        //                    //SubJobNo = Convert.ToString(dr["SubJob_No"]),
        //                    CompanyName = Convert.ToString(dr["Company_Name"]),
        //                    Amount = Convert.ToDouble(dr["Amount"]),


        //                });
        //            }
        //            objEIModel.LstCostDistribution = lmd1;



        //            return objEIModel.LstCostDistribution;
        //            //return  (lmd, lmd1);
        //            //return objEIModel;
        //        }
        //        #endregion

        //    //    public Microsoft.Office.Interop.Excel.Application PrepareForExport(System.Data.DataSet ds, string[] sheet)
        //    //    {
        //    //        object missing = System.Reflection.Missing.Value;
        //    //        Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
        //    //        Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(missing);

        //    //        Microsoft.Office.Interop.Excel.DataTable dt1 = new Microsoft.Office.Interop.Excel.DataTable();
        //    //        dt1 = ds.Tables[0];
        //    //        DataTable dt2 = new DataTable();
        //    //        dt2 = ds.Tables[1];


        //    //        Excel.Worksheet newWorksheet;
        //    //        newWorksheet = (Excel.Worksheet)excel.Worksheets.Add(missing, missing, missing, missing);
        //    //        newWorksheet.Name = "Name of data sheet";

        //    //        int iCol1 = 0;
        //    //        foreach (DataColumn c in dt1.Columns)
        //    //        {
        //    //            iCol1++;
        //    //            excel.Cells[1, iCol1] = c.ColumnName;
        //    //        }

        //    //        int iRow1 = 0;
        //    //        foreach (DataRow r in dt1.Rows)
        //    //        {
        //    //            iRow1++;

        //    //            for (int i = 1; i < dt1.Columns.Count + 1; i++)
        //    //            {

        //    //                if (iRow1 == 1)
        //    //                {
        //    //                    // Add the header the first time through 
        //    //                    excel.Cells[iRow1, i] = dt1.Columns[i - 1].ColumnName;
        //    //                }

        //    //                excel.Cells[iRow1 + 1, i] = r[i - 1].ToString();
        //    //            }

        //    //        }

        //    //        int iCol2 = 0;
        //    //        foreach (DataColumn c in dt2.Columns)
        //    //        {
        //    //            iCol2++;
        //    //            excel.Cells[1, iCol] = c.ColumnName;
        //    //        }


        //    //        int iRow2 = 0;
        //    //        foreach (DataRow r in dt2.Rows)
        //    //        {
        //    //            iRow2++;

        //    //            for (int i = 1; i < dt2.Columns.Count + 1; i++)
        //    //            {

        //    //                if (iRow2 == 1)
        //    //                {
        //    //                    // Add the header the first time through 
        //    //                    excel.Cells[iRow2, i] = dt2.Columns[i - 1].ColumnName;
        //    //                }

        //    //                excel.Cells[iRow2 + 1, i] = r[i - 1].ToString();
        //    //            }

        //    //        }




        //    //        return excel;

        //    //}
        #endregion






    }
}
