using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;
using NonFactors.Mvc.Grid;
using SelectPdf;
using OfficeOpenXml;

namespace TuvVision.Controllers
{
    public class ApprovalController : Controller
    {

        DALApproval objA = new DALApproval();
        Approval objAModel = new Approval();
        string Result = string.Empty;

        // GET: Approval
        public ActionResult Approval(String LFKExpenseId,int LVoucherId)
        {

            #region Bind Name
            List<EmpName> lstName = new List<EmpName>();
            DataSet dsGetName = new DataSet();
            
                dsGetName = objA.GetName();
            if (dsGetName.Tables.Count > 0)//Dynamic Binding Title DropDwonlist
            {
                lstName = (from n in dsGetName.Tables[0].AsEnumerable()
                           select
                           new EmpName()
                           {
                               Name = n.Field<string>(dsGetName.Tables[0].Columns["Name"].ToString()),
                               Code = n.Field<string>(dsGetName.Tables[0].Columns["Id"].ToString())

                           }).ToList();
                ViewBag.Name = lstName;
            }
            
            else
            {

            }

            #endregion

            TempData["VoucherId"] = LVoucherId;
            TempData.Keep();


            List<ApprovalList> lmd = new List<ApprovalList>();  // creating list of model.  
            DataSet ds = new DataSet();

            TempData["InspectorId"] = objAModel.Inspector;
            TempData.Keep();

            ds = objA.GetDataByID(LFKExpenseId);

            if (ds.Tables.Count > 0)
            {
                objAModel.Inspector = ds.Tables[0].Rows[0]["CreatedBy"].ToString();

                TempData["InspectorId"] = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                TempData.Keep();
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    
                    lmd.Add(new ApprovalList
                    {

                      
                        LDate = Convert.ToDateTime(dr["Date"]),
                        LExpenseType = Convert.ToString(dr["ExpenseType"]),
                        LCountry = Convert.ToString(dr["Country"]),
                        LCurrency = Convert.ToString(dr["Currency"]),
                        LTotalAmount = Convert.ToInt32(dr["TotalAmount"]),
                        LExchRate = Convert.ToInt32(dr["ExchRate"]),
                        LDescription = Convert.ToString(dr["Description"]),
                        LAprovelAmount = Convert.ToInt32(dr["ApprovedAmount"]),
                        LPKExpenseId = Convert.ToInt32(dr["PKExpenseId"]),
                        
                    });
                }
                ViewData["InspectorVoucherDetail"] = lmd;
            }
            else
            {

            }


            return View();
        }

        [HttpPost]
        public ActionResult Approval(Approval objAModel,FormCollection fc,string AppAmount)
        {

            #region Old

            #region Bind Name
            //DataSet dsGetName = new DataSet();
            //dsGetName = objA.GetName();
            //List<EmpName> lstName = new List<EmpName>();
            //if (dsGetName.Tables.Count > 0)//Dynamic Binding Title DropDwonlist
            //{
            //    lstName = (from n in dsGetName.Tables[0].AsEnumerable()
            //               select
            //               new EmpName()
            //               {
            //                   Name = n.Field<string>(dsGetName.Tables[0].Columns["Name"].ToString()),
            //                   Code = n.Field<string>(dsGetName.Tables[0].Columns["Id"].ToString())

            //               }).ToList();
            //    ViewBag.Name = lstName;
            //}
            #endregion
            //List<ApprovelList> lmd = new List<ApprovelList>();  // creating list of model.  
            //DataSet ds = new DataSet();

            //ds = objA.GetDataByDate(objAModel);

            //if (ds.Tables.Count > 0)
            //{
            //    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            //    {
            //        lmd.Add(new ApprovelList
            //        {
            //            LPKExpenseId = Convert.ToInt32(dr["PKExpenseId"]),

            //            LActivityType = Convert.ToString(dr["ActivityType"]),
            //            LApprovelStatus1 = Convert.ToString(dr["ApprovalStatus1"]),
            //            LApprovelStatus2 = Convert.ToString(dr["ApprovalStatus2"]),
            //            LTotalAmount = Convert.ToDouble(dr["TotalAmount"]),
            //            LAprovelAmount = Convert.ToInt16(dr["ApprovedAmount"]),
            //            LTransferToFI = Convert.ToString(dr["TransferToFI"]),
            //        });
            //    }
            //    ViewData["InspectorVoucherDetail"] = lmd;
            //}
            //else
            //{

            //}
            //return View();
            #endregion


            string First = null;
            string Two = null;
            string Person = null;
            #region Get ApprovalNo - ReportingPersonNo
            string InspectorId = TempData["InspectorId"].ToString();
            DataSet ds = new DataSet();
            ds = objA.GetApprovalDetail(InspectorId);
            if (ds.Tables.Count > 0)
            {
                First = ds.Tables[0].Rows[0]["Reporting_Person_One"].ToString();
                Two = ds.Tables[0].Rows[0]["Reporting_Person_Two"].ToString();
            }
            string LoginId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

            if (First == LoginId)
            {
                Person = "ApprovalOne";

            }
            else
            {
                Person = "ApprovalTwo";
            }

            #endregion

            Double Sum=0;
            Double Sum1 = 0;
          //  Sum = Convert.ToDouble( fc["Sum"].ToString());
            Sum = Convert.ToDouble(AppAmount);

           /* foreach (var Item in objAModel.ApprovelList)
            {
                //if (Item.Checkbox1 == true)
                //{
                    objAModel.PKExpenseId = Item.LPKExpenseId;
                    objAModel.AprovelAmount = Item.LAprovelAmount;
                    Result = objA.ProvideApproval(objAModel, Person);
                    Result = objA.UpdateTransferToFI(objAModel, Person);
                Sum  += Item.LAprovelAmount;
                //}
            }*/
            
            #region Update into Voucher Table
            int VoucherId = Convert.ToInt16(TempData["VoucherId"]);
            Result =  objA.ProvideVoucherApproval(objAModel, Person,VoucherId,Sum);

            Result = objA.UpdateTransferToFIVoucher(objAModel, Person,VoucherId);

            #endregion
            return RedirectToAction("VoucherForApproval");
        }


        [HttpPost]
        public ActionResult ProvideApproval(Approval objEIModel)
        {
            string First=null;
            string Two = null;
            string Person = null;
            #region Get ApprovalNo - ReportingPersonNo
            string InspectorId = objEIModel.Inspector;//TempData["InspectorId"].ToString();
            DataSet ds = new DataSet();
            ds = objA.GetApprovalDetail(InspectorId);
            if(ds.Tables.Count>0)
            {
                 First = ds.Tables[0].Rows[0]["Reporting_Person_One"].ToString();
                 Two    = ds.Tables[0].Rows[0]["Reporting_Person_Two"].ToString();
            }
            string LoginId= Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

            if(First == LoginId)
            {
                Person = "ApprovalOne";
               
            }
            else
            {
                Person = "ApprovalTwo";
            }

                #endregion

                foreach (var Item in objEIModel.ApprovalList)
            {
                if (Item.Checkbox1 == true)
                {
                    objEIModel.PKExpenseId = Item.LPKExpenseId;
                    objEIModel.AprovelAmount = Item.LAprovelAmount;
                    Result = objA.ProvideApproval(objEIModel,Person);
                    Result = objA.UpdateTransferToFI(objEIModel, Person);
                    
                }
            }
            return RedirectToAction("VoucherForApproval");
        }



        public ActionResult VoucherForApproval()
        {

            #region Bind Name
            List<EmpName> lstName = new List<EmpName>();
            DataSet dsGetName = new DataSet();

            dsGetName = objA.GetName();
            if (dsGetName.Tables.Count > 0)//Dynamic Binding Title DropDwonlist
            {
                lstName = (from n in dsGetName.Tables[0].AsEnumerable()
                           select
                           new EmpName()
                           {
                               Name = n.Field<string>(dsGetName.Tables[0].Columns["Name"].ToString()),
                               Code = n.Field<string>(dsGetName.Tables[0].Columns["Id"].ToString())

                           }).ToList();
                ViewBag.Name = lstName;
            }

            else
            {

            }

            #endregion

            List<Approval> lmd = new List<Approval>();  // creating list of model.  
            DataSet ds = new DataSet();

            Session["GetExcelData"] = "Yes";

            TempData["InspectorId"] = objAModel.Inspector;
            TempData.Keep();

            ds = objA.GetVoucherByDate(objAModel);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new Approval
                    {
                        LVoucherId = Convert.ToInt32(dr["VoucherId"]),
                        LFKExpenseId = Convert.ToString(dr["FKExpenseId"]),
                        Date = Convert.ToString(dr["CreatedDate"]),
                        //LFKExpenseId = Convert.ToString(dr["FKExpenseId"]),
                        //LVoucherId = Convert.ToInt32(dr["VoucherId"]),
                        //Date = Convert.ToDateTime(dr["CreatedDate"]),
                        //TotalAmount = Convert.ToDouble(dr["TotalAmount"])
                        //ActivityType = Convert.ToString(dr["ActivityType"]),
                        ApprovalStatus1 = Convert.ToString(dr["ApprovelStatus1"]),
                        ApprovalStatus2 = Convert.ToString(dr["ApprovelStatus2"]),
                        TotalAmount = Convert.ToDouble(dr["VTotalAmount"]),
                        AprovelAmount = Convert.ToDouble(dr["VApprovedAmount"]),

                    });
                }
                ViewData["InspectorVoucherDetail"] = lmd;
                objAModel.LstDashboard = lmd;
            }
            else
            {
                
                objAModel.LstDashboard = lmd;
                return View(objAModel);
            }           

            return View(objAModel);
        }

        [HttpPost]
        public ActionResult VoucherForApproval(Approval objAModel)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = objAModel.FromDate;
            Session["ToDate"] = objAModel.ToDate;
            Session["Inspector"] = objAModel.Inspector;

            #region Bind Name
            List<EmpName> lstName = new List<EmpName>();
            DataSet dsGetName = new DataSet();

            dsGetName = objA.GetName();
            if (dsGetName.Tables.Count > 0)//Dynamic Binding Title DropDwonlist
            {
                lstName = (from n in dsGetName.Tables[0].AsEnumerable()
                           select
                           new EmpName()
                           {
                               Name = n.Field<string>(dsGetName.Tables[0].Columns["Name"].ToString()),
                               Code = n.Field<string>(dsGetName.Tables[0].Columns["Id"].ToString())

                           }).ToList();
                ViewBag.Name = lstName;
            }

            else
            {

            }

            #endregion

            List<Approval> lmd = new List<Approval>();  // creating list of model.  
            DataSet ds = new DataSet();

            TempData["InspectorId"] = objAModel.Inspector;
            TempData.Keep();

            ds = objA.GetVoucherByDate(objAModel);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new Approval
                    {
                        
                     

                        LVoucherId = Convert.ToInt32(dr["VoucherId"]),
                        LFKExpenseId = Convert.ToString(dr["FKExpenseId"]),
                        Inspector = Convert.ToString(dr["Inspector"]),
                        Date = Convert.ToString(dr["CreatedDate"]),

                        ApprovalStatus1 = Convert.ToString(dr["ApprovelStatus1"]),
                        ApprovalStatus2 = Convert.ToString(dr["ApprovelStatus2"]),
                        TotalAmount = Convert.ToDouble(dr["VTotalAmount"]),
                        AprovelAmount = Convert.ToDouble(dr["VApprovedAmount"]),
                        TransferToFI = Convert.ToString(dr["TransferToFI"]),

                    });
                }
                ViewData["InspectorVoucherDetail"] = lmd;
                objAModel.LstDashboard = lmd;
            }
            else
            {
                
            }
            objAModel.LstDashboard = lmd;

            return View(objAModel);
        }
        #region
        [HttpGet]
        public ActionResult ExportIndex(Approval c)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<Approval> grid = CreateExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<Approval> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<Approval> CreateExportableGrid(Approval c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<Approval> grid = new Grid<Approval>(GetData(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.LVoucherId).Titled("Voucher Generated");
            grid.Columns.Add(model => model.Date).Titled("Date");
            grid.Columns.Add(model => model.LFKExpenseId).Titled("ID");

            grid.Columns.Add(model => model.ApprovalStatus1).Titled("Approvel Status 1");
            grid.Columns.Add(model => model.ApprovalStatus2).Titled("Approvel Status 2");
            grid.Columns.Add(model => model.TotalAmount).Titled("Total Amount");
            grid.Columns.Add(model => model.AprovelAmount).Titled("Approved Amount");
            grid.Columns.Add(model => model.TransferToFI).Titled("Transfer To FI");
            

            grid.Pager = new GridPager<Approval>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objAModel.LstDashboard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<Approval> GetData(Approval c)
        {         
            DataTable CompanyDashBoard = new DataTable();
            List<Approval> lstCompanyDashBoard = new List<Approval>();

            #region Bind Name
            List<EmpName> lstName = new List<EmpName>();
            DataSet dsGetName = new DataSet();

            dsGetName = objA.GetName();
            if (dsGetName.Tables.Count > 0)//Dynamic Binding Title DropDwonlist
            {
                lstName = (from n in dsGetName.Tables[0].AsEnumerable()
                           select
                           new EmpName()
                           {
                               Name = n.Field<string>(dsGetName.Tables[0].Columns["Name"].ToString()),
                               Code = n.Field<string>(dsGetName.Tables[0].Columns["Id"].ToString())

                           }).ToList();
                ViewBag.Name = lstName;
            }

            else
            {

            }

            #endregion

            List<Approval> lmd = new List<Approval>();  // creating list of model.  
            DataSet ds = new DataSet();

            TempData["InspectorId"] = objAModel.Inspector;
            TempData.Keep();

            //Session["FromDate"] = objAModel.FromDate;
            //Session["ToDate"] = objAModel.ToDate;
            //Session["Inspector"] = objAModel.Inspector;
            string s = Session["FromDate"].ToString();
            objAModel.FromDate = Convert.ToString(Session["FromDate"]);
            objAModel.ToDate = Convert.ToString(Session["ToDate"]);
            objAModel.Inspector = Session["Inspector"].ToString();

            ds = objA.GetVoucherByDate(objAModel);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new Approval
                    {

                        //ActivityType = Convert.ToString(dr["ActivityType"]),
                        //ApprovelStatus1 = Convert.ToString(dr["ApprovelStatus1"]),
                        //ApprovelStatus2 = Convert.ToString(dr["ApprovelStatus2"]),
                        //TotalAmount = Convert.ToDouble(dr["TotalAmount"]),
                        //AprovelAmount = Convert.ToDouble(dr["ApprovedAmount"]),

                        LVoucherId = Convert.ToInt32(dr["VoucherId"]),
                        LFKExpenseId = Convert.ToString(dr["FKExpenseId"]),
                        Date = Convert.ToString(dr["CreatedDate"]),


                        ApprovalStatus1 = Convert.ToString(dr["ApprovelStatus1"]),
                        ApprovalStatus2 = Convert.ToString(dr["ApprovelStatus2"]),
                        TotalAmount = Convert.ToDouble(dr["VTotalAmount"]),
                        AprovelAmount = Convert.ToDouble(dr["VApprovedAmount"]),
                        TransferToFI = Convert.ToString(dr["TransferToFI"]),

                    });
                }
                ViewData["InspectorVoucherDetail"] = lmd;
                objAModel.LstDashboard = lmd;
            }
            else
            {

            }

            return objAModel.LstDashboard;
        }

        #endregion
    }
}