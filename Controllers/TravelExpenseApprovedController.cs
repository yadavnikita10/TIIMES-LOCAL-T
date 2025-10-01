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
    public class TravelExpenseApprovedController : Controller
    {

        TravelExpense Expense = new TravelExpense();  //added by nikita on 09102023
        DalCallHistory Dalobj = new DalCallHistory();

        // GET: TravelExpenseApproved
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult TravelExpenseApproved()
        {
            TravelExpense Expense = new TravelExpense();
            List<TravelExpense> lstexpense = new List<TravelExpense>();

            try
            {
                DataTable DataTable = new DataTable();
                DataTable = Dalobj.GetExpenseDetails_Approved();
                if (DataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in DataTable.Rows)
                    {
                        lstexpense.Add(
                            new TravelExpense
                            {
                                EmployeeName = Convert.ToString(dr["EmployeeName"]),
                                RoleName = Convert.ToString(dr["RoleName"]),
                                ExpenseType = Convert.ToString(dr["ExpenseType"]),
                                Currency = Convert.ToString(dr["Currency"]),
                                Description = Convert.ToString(dr["Description"]),
                                VoucherGenerated = Convert.ToString(dr["VoucherGenerated"]),
                                CountryName = Convert.ToString(dr["CountryName"]),
                                City = Convert.ToString(dr["City"]),
                                Amount = Convert.ToString(dr["Amount"]),
                                ExchRate = Convert.ToString(dr["ExchRate"]),
                                Type = Convert.ToString(dr["Type"]),
                                TotalAmount = Convert.ToString(dr["TotalAmount"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                                EndCity = Convert.ToString(dr["EndCity"]),
                                Kilometer = Convert.ToString(dr["Kilometer"]),
                                SubJobNo = Convert.ToString(dr["SubJobNo"]),
                                IsActive = Convert.ToString(dr["IsActive"]),
                                IsSendForApproval = Convert.ToString(dr["IsSendForApproval"]),
                                UIN = Convert.ToString(dr["UIN"]),
                                VoucherNo = Convert.ToString(dr["VoucherNo"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                IsSendForApprovalAmount = Convert.ToString(dr["IsSendForApproval_Date"]),


                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Date = Convert.ToString(dr["Date"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                EmpCategoray = Convert.ToString(dr["EmpCategoray"]),
                                Cost_Center = Convert.ToString(dr["Cost_Center"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                                Designation = Convert.ToString(dr["Designation"]),


                                BranchName = Convert.ToString(dr["Branch_Name"]),
                                //BranchName = Convert.ToString(dr["subcategory_IVR"]),
                                subCategory = Convert.ToString(dr["subcategory_IVR"]),
                                ExpenseagainstGenertaed = Convert.ToString(dr["Expenses_against_SAP_No_costcenter"]),
                                ExpenseagainstGenertaed_No = Convert.ToString(dr["sap_no_costcenter"]),




                            }
                            );
                    }
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            ViewData["UserList"] = lstexpense;
            Expense.lstTravelExpense = lstexpense;

            return View(Expense);
        }


        [HttpPost]
        public ActionResult TravelExpenseApproved(string FromDate, string ToDate, string SAP_No)
        {
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            Session["SAP_No"] = SAP_No;
            TravelExpense Expense = new TravelExpense();
            List<TravelExpense> lstexpense = new List<TravelExpense>();

            try
            {
                DataTable DataTable = new DataTable();
                DataTable = Dalobj.GetExpenseDetailsDatewise_Approved(FromDate, ToDate, SAP_No);
                if (DataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in DataTable.Rows)
                    {
                        lstexpense.Add(
                            new TravelExpense
                            {
                                EmployeeName = Convert.ToString(dr["EmployeeName"]),
                                RoleName = Convert.ToString(dr["RoleName"]),
                                ExpenseType = Convert.ToString(dr["ExpenseType"]),
                                Currency = Convert.ToString(dr["Currency"]),
                                Description = Convert.ToString(dr["Description"]),
                                VoucherGenerated = Convert.ToString(dr["VoucherGenerated"]),
                                CountryName = Convert.ToString(dr["CountryName"]),
                                City = Convert.ToString(dr["City"]),
                                Amount = Convert.ToString(dr["Amount"]),
                                ExchRate = Convert.ToString(dr["ExchRate"]),
                                Type = Convert.ToString(dr["Type"]),
                                TotalAmount = Convert.ToString(dr["TotalAmount"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                                EndCity = Convert.ToString(dr["EndCity"]),
                                Kilometer = Convert.ToString(dr["Kilometer"]),
                                SubJobNo = Convert.ToString(dr["SubJobNo"]),
                                IsActive = Convert.ToString(dr["IsActive"]),
                                IsSendForApproval = Convert.ToString(dr["IsSendForApproval"]),
                                UIN = Convert.ToString(dr["UIN"]),
                                VoucherNo = Convert.ToString(dr["VoucherNo"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                IsSendForApprovalAmount = Convert.ToString(dr["IsSendForApproval_Date"]),


                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Date = Convert.ToString(dr["Date"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                EmpCategoray = Convert.ToString(dr["EmpCategoray"]),
                                Cost_Center = Convert.ToString(dr["Cost_Center"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                                Designation = Convert.ToString(dr["Designation"]),


                                BranchName = Convert.ToString(dr["Branch_Name"]),
                                //BranchName = Convert.ToString(dr["subcategory_IVR"]),
                                subCategory = Convert.ToString(dr["subcategory_IVR"]),
                                ExpenseagainstGenertaed = Convert.ToString(dr["Expenses_against_SAP_No_costcenter"]),
                                ExpenseagainstGenertaed_No = Convert.ToString(dr["sap_no_costcenter"]),


                            }
                            );
                    }
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            ViewData["UserList"] = lstexpense;
            Expense.lstTravelExpense = lstexpense;
            return View(Expense);
        }

        //Export To Excel for Travel Expense 



        [HttpGet]
        public ActionResult ExportIndexTravelExpense(TravelExpense c)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<TravelExpense> grid = CreateExportableGridTravelExpense(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<TravelExpense> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                Session["FromDate"] = null;

                Session["ToDate"] = null;

                Session["SAP_No"] = null;
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "TravelExpense" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<TravelExpense> CreateExportableGridTravelExpense(TravelExpense c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<TravelExpense> grid = new Grid<TravelExpense>(GetDataTravelExpense(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.EmployeeName).Titled("Employee Name");
            grid.Columns.Add(model => model.RoleName).Titled("RoleName");
            grid.Columns.Add(model => model.CountryName).Titled("Country Name");
            grid.Columns.Add(model => model.Tuv_Email_Id).Titled("Tuv Email Id");
            grid.Columns.Add(model => model.MobileNo).Titled("MobileNo");
            grid.Columns.Add(model => model.Designation).Titled("Designation");
            grid.Columns.Add(model => model.BranchName).Titled("Branch Name");
            grid.Columns.Add(model => model.EmpCategoray).Titled("Emp Categoray");
            grid.Columns.Add(model => model.EmployeeCode).Titled("Employee Code");
            grid.Columns.Add(model => model.SAPEmpCode).Titled("SAP Emp Code");
            grid.Columns.Add(model => model.SubJobNo).Titled("Sub JobNo");
            grid.Columns.Add(model => model.SAP_No).Titled("SAP No");
            grid.Columns.Add(model => model.CountryName).Titled("Country Name");
            grid.Columns.Add(model => model.Cost_Center).Titled("User Cost Center");
            grid.Columns.Add(model => model.Date).Titled("Date");
            grid.Columns.Add(model => model.City).Titled("City");
            grid.Columns.Add(model => model.ExpenseType).Titled("Expense Type");
            grid.Columns.Add(model => model.Amount).Titled("Amount");
            grid.Columns.Add(model => model.Kilometer).Titled("Kilometer");
            grid.Columns.Add(model => model.Currency).Titled("Currency");
            grid.Columns.Add(model => model.ExchRate).Titled("Exch Rate");
            grid.Columns.Add(model => model.TotalAmount).Titled("TotalAmount");
            grid.Columns.Add(model => model.Description).Titled("Description");
            grid.Columns.Add(model => model.VoucherGenerated).Titled("Voucher Generated");
            grid.Columns.Add(model => model.VoucherNo).Titled("Voucher No");
            grid.Columns.Add(model => model.IsSendForApproval).Titled("Is Send For Approval");
            grid.Columns.Add(model => model.IsSendForApprovalAmount).Titled("Is Send For Approval Amount");
            grid.Columns.Add(model => model.Type).Titled("Main Category");
            grid.Columns.Add(model => model.subCategory).Titled("sub Category");
            grid.Columns.Add(model => model.ExpenseagainstGenertaed).Titled("Expense Against Generated SAP NO/User Costcenter");
            grid.Columns.Add(model => model.ExpenseagainstGenertaed_No).Titled("Expense Against Generated SAP NO/User Costcenter No");
            grid.Columns.Add(model => model.CreatedDate).Titled("Created Date");
            grid.Columns.Add(model => model.ModifiedDate).Titled("Modified Date");
            grid.Columns.Add(model => model.ApprovedAmount).Titled("Approved Amount");
            grid.Columns.Add(model => model.UIN).Titled("UIN");
            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.IsActive).Titled("IsActive");



            grid.Pager = new GridPager<TravelExpense>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = Expense.lstTravelExpense.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<TravelExpense> GetDataTravelExpense(TravelExpense c)
        {
            DataTable DataTable = new DataTable();
            List<TravelExpense> lstexpense = new List<TravelExpense>();

            string FromDate = string.Empty;
            string ToDate = string.Empty;
            string SAP_No = string.Empty;

            if ((Session["FromDate"] != null && Session["ToDate"] != null) || Session["SAP_No"] != null)
            {
                FromDate = Convert.ToString(Session["FromDate"]);
                ToDate = Convert.ToString(Session["ToDate"]);
                SAP_No = Convert.ToString(Session["SAP_No"]);

                DataTable = Dalobj.GetExpenseDetailsDatewise_Approved(FromDate, ToDate, SAP_No);

            }
            else
            {
                DataTable = Dalobj.GetExpenseDetails_Approved();

            }

            try
            {
                if (DataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in DataTable.Rows)
                    {
                        lstexpense.Add(
                            new TravelExpense
                            {
                                //Count=DataTable.Rows.Count,


                                EmployeeName = Convert.ToString(dr["EmployeeName"]),
                                RoleName = Convert.ToString(dr["RoleName"]),
                                ExpenseType = Convert.ToString(dr["ExpenseType"]),
                                Currency = Convert.ToString(dr["Currency"]),
                                Description = Convert.ToString(dr["Description"]),
                                VoucherGenerated = Convert.ToString(dr["VoucherGenerated"]),
                                CountryName = Convert.ToString(dr["CountryName"]),
                                City = Convert.ToString(dr["City"]),
                                Amount = Convert.ToString(dr["Amount"]),
                                ExchRate = Convert.ToString(dr["ExchRate"]),
                                Type = Convert.ToString(dr["Type"]),
                                TotalAmount = Convert.ToString(dr["TotalAmount"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                                EndCity = Convert.ToString(dr["EndCity"]),
                                Kilometer = Convert.ToString(dr["Kilometer"]),
                                SubJobNo = Convert.ToString(dr["SubJobNo"]),
                                IsActive = Convert.ToString(dr["IsActive"]),
                                IsSendForApproval = Convert.ToString(dr["IsSendForApproval"]),
                                UIN = Convert.ToString(dr["UIN"]),
                                VoucherNo = Convert.ToString(dr["VoucherNo"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                IsSendForApprovalAmount = Convert.ToString(dr["IsSendForApproval_Date"]),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Date = Convert.ToString(dr["Date"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                EmpCategoray = Convert.ToString(dr["EmpCategoray"]),
                                Cost_Center = Convert.ToString(dr["Cost_Center"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                BranchName = Convert.ToString(dr["Branch_Name"]),
                                //BranchName = Convert.ToString(dr["subcategory_IVR"]),
                                subCategory = Convert.ToString(dr["subcategory_IVR"]),
                                ExpenseagainstGenertaed = Convert.ToString(dr["Expenses_against_SAP_No_costcenter"]),
                                ExpenseagainstGenertaed_No = Convert.ToString(dr["sap_no_costcenter"]),




                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["TravelExpense"] = lstexpense;
            Expense.lstTravelExpense = lstexpense;

            return Expense.lstTravelExpense;
        }
    }
}