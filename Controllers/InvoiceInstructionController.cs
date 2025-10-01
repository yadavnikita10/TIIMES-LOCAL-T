using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Data;
using System.IO;
using TuvVision.DataAccessLayer;
using TuvVision.Models;
using OfficeOpenXml;
using NonFactors.Mvc.Grid;

namespace TuvVision.Controllers
{
    public class InvoiceInstructionController : Controller
    {
        DALInvoiceInstruction OBJInvoiceInstruction = new DALInvoiceInstruction();
        InvoiceInstructionModel IIM = new InvoiceInstructionModel();
        MISOrderStatus objMOS = new MISOrderStatus();
        DALUsers objDalCreateUser = new DALUsers();
        DALNonInspectionActivity objNIA = new DALNonInspectionActivity();

        // GET: InvoiceInstruction

        [HttpGet]
        public ActionResult InvoiceInstructionDetails(int? INV_ID, int? PK_JOB_ID)
        {
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<InvoiceInstructionModel> lstCompanyDashBoard = new List<InvoiceInstructionModel>();
            ViewData["NoReport"] = null;
            int sid = Convert.ToInt32(INV_ID);
            DataSet DSEditGetList = new DataSet();


            var Data1 = objDalCreateUser.GetCostCenterList();
            ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");
            DataSet CostCenter = objNIA.GetServiceCode();
            IIM.ServiceCode = CostCenter.Tables[0].Rows[0]["CostCenter"].ToString();

            
            DataSet dsSubJobList = new DataSet();
            
            List<CallsModel> lstSubJobByJobNo = new List<CallsModel>();
            dsSubJobList = OBJInvoiceInstruction.BindSubJobControlNo(Convert.ToInt32(PK_JOB_ID));

            if (dsSubJobList.Tables[0].Rows.Count > 0)
            {
                lstSubJobByJobNo = (from n in dsSubJobList.Tables[0].AsEnumerable()
                                    select new CallsModel()
                                    {
                                        DSubJob_No = n.Field<string>(dsSubJobList.Tables[0].Columns["SubJob_No"].ToString()),
                                        DPK_SubJob_Id = n.Field<int>(dsSubJobList.Tables[0].Columns["PK_SubJob_Id"].ToString())

                                    }).ToList();
            }
            if (ViewData["SubJob_NoByJobNo"] == null)
            {
                IEnumerable<SelectListItem> AuditorName;
                AuditorName = new SelectList(lstSubJobByJobNo, "DAuditorCode", "DAuditorName");

                ViewData["SubJob_NoByJobNo"] = lstSubJobByJobNo;
            }


            #region
            if (sid != 0)
            {
                ItemDescriptionData = OBJInvoiceInstruction.GetInvoiceInstructionById(Convert.ToInt32(PK_JOB_ID));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    IIM.PoAmount = Convert.ToDecimal(ItemDescriptionData.Tables[0].Rows[0]["Customer_PO_Amount"]);
                    IIM.Balance = Convert.ToDecimal(ItemDescriptionData.Tables[0].Rows[0]["Balance"]);
                    IIM.PK_JOB_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_JOB_ID"]);
                    IIM.Job_Number = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Job_Number"]);
                    IIM.OrderType = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["OrderType"]);
                    IIM.OrderRate = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["OrderRate"]);
                }

                ItemDescriptionData = OBJInvoiceInstruction.EditInvoiceInstructionById(Convert.ToInt32(INV_ID));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    IIM.PoAmount = Convert.ToDecimal(ItemDescriptionData.Tables[0].Rows[0]["PoAmount"]);
                    IIM.InvoiceAmount = Convert.ToDecimal(ItemDescriptionData.Tables[0].Rows[0]["InvoiceAmount"]);
                    Session["PreviousAmount"] = Convert.ToDecimal(ItemDescriptionData.Tables[0].Rows[0]["InvoiceAmount"]);
                    IIM.TotalAmount = Convert.ToDecimal(ItemDescriptionData.Tables[0].Rows[0]["TotalAmount"]);
                    IIM.Balance = Convert.ToDecimal(ItemDescriptionData.Tables[0].Rows[0]["Balance"]);
                    IIM.InvoiceDate = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["InvoiceDate"]);
                    IIM.InvoiceNumber = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["InvoiceNumber"]);
                    IIM.INV_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["INV_ID"]);
                    IIM.PK_JOB_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_JOB_ID"]);
                    IIM.OrderType = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["OrderType"]);
                    IIM.OrderRate = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["OrderRate"]);
                    IIM.GSTDetail = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["GSTDetail"]);
                    IIM.PK_SubJob_Id = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_SubJob_Id"]);
                    IIM.rptType = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["rptT_ype"]);
                    IIM.Invoicetext = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Invoicetext"]);
                    IIM.ServiceCode = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["ServiceCode"]);
                    IIM.SAPInvNo = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["SAPInvNo"]);
                }
                CostSheetDashBoard = OBJInvoiceInstruction.GetInvoiceInstruction(Convert.ToInt32(PK_JOB_ID));

                try
                {
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new InvoiceInstructionModel
                                {
                                    JobNo = Convert.ToString(dr["Job_Number"]),
                                    SubJobNo = Convert.ToString(dr["SubJob_No"]),
                                    PoAmount = Convert.ToDecimal(dr["PoAmount"]),
                                    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]),
                                    TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                                    Balance = Convert.ToDecimal(dr["Balance"]),
                                    InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                                    InvoiceNumber = Convert.ToString(dr["InvoiceNumber"]),
                                    INV_ID = Convert.ToInt32(dr["INV_ID"]),
                                    PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                    OrderType = Convert.ToString(dr["OrderType"]),
                                    OrderRate = Convert.ToString(dr["OrderRate"]),
                                    GSTDetail = Convert.ToString(dr["GSTDetail"]),
                                });
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
            }
            #endregion
            else
            {
                IIM.InvoiceDate = DateTime.Now.ToString("dd/MM/yyyy");
                ItemDescriptionData = OBJInvoiceInstruction.GetInvoiceInstructionById(Convert.ToInt32(PK_JOB_ID));
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    IIM.PoAmount = Convert.ToDecimal(ItemDescriptionData.Tables[0].Rows[0]["Customer_PO_Amount"]);
                    IIM.Balance = Convert.ToDecimal(ItemDescriptionData.Tables[0].Rows[0]["Balance"]);
                    IIM.PK_JOB_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_JOB_ID"]);
                    IIM.Job_Number = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Job_Number"]);
                    IIM.OrderType = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["OrderType"]);
                    IIM.OrderRate = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["OrderRate"]);
                    IIM.GSTDetail = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["GSTDetail"]);
                    string strInvoiceNo = string.Empty;
                    strInvoiceNo = OBJInvoiceInstruction.InvoiceIDByJob(Convert.ToInt32(PK_JOB_ID));
                    IIM.InvoiceNumber = strInvoiceNo;

                }
                CostSheetDashBoard = OBJInvoiceInstruction.GetInvoiceInstruction(Convert.ToInt32(PK_JOB_ID));
                try
                {
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new InvoiceInstructionModel
                                {
                                    JobNo = Convert.ToString(dr["Job_Number"]),
                                    SubJobNo = Convert.ToString(dr["SubJob_No"]),
                                    PoAmount = Convert.ToDecimal(dr["PoAmount"]),
                                    InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]),
                                    TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                                    Balance = Convert.ToDecimal(dr["Balance"]),
                                    InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                                    InvoiceNumber = Convert.ToString(dr["InvoiceNumber"]),
                                    INV_ID = Convert.ToInt32(dr["INV_ID"]),
                                    PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                    OrderType = Convert.ToString(dr["OrderType"]),
                                    OrderRate = Convert.ToString(dr["OrderRate"]),
                                    GSTDetail = Convert.ToString(dr["GSTDetail"]),
                                    ServiceCode = Convert.ToString(dr["ServiceCode"]),
                                    SAPInvNo = Convert.ToString(dr["SAPInvNo"]),
                                    
                        });
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
            }
            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(IIM);
        }

        [HttpPost]
        public ActionResult InvoiceInstructionDetails(InvoiceInstructionModel IIM, FormCollection fc, string Command)
        {
            string Result = string.Empty;
            DataTable CompanyDashBoard = new DataTable();

            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();

            List<InvoiceInstructionModel> lstCompanyDashBoard = new List<InvoiceInstructionModel>();
            List<InvoiceList> lstIVRList = new List<InvoiceList>();
            string[] split;
            string ListPKSubJob = string.Empty;
            string[] split1;
            string ListPKSubJob1 = string.Empty;
            DataSet dsSubJobList = new DataSet();
            ViewData["NoReport"] = null;
            List<CallsModel> lstSubJobByJobNo = new List<CallsModel>();

            var Data1 = objDalCreateUser.GetCostCenterList();
            ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");
            DataSet CostCenter = objNIA.GetServiceCode();
           // IIM.ServiceCode = CostCenter.Tables[0].Rows[0]["CostCenter"].ToString();

            dsSubJobList = OBJInvoiceInstruction.BindSubJobControlNo(Convert.ToInt32(IIM.PK_JOB_ID));

            if (dsSubJobList.Tables[0].Rows.Count > 0)
            {
                lstSubJobByJobNo = (from n in dsSubJobList.Tables[0].AsEnumerable()
                                    select new CallsModel()
                                    {
                                        DSubJob_No = n.Field<string>(dsSubJobList.Tables[0].Columns["SubJob_No"].ToString()),
                                        DPK_SubJob_Id = n.Field<int>(dsSubJobList.Tables[0].Columns["PK_SubJob_Id"].ToString())

                                    }).ToList();
            }

            if (ViewData["SubJob_NoByJobNo"] == null)
            {
                IEnumerable<SelectListItem> AuditorName;
                AuditorName = new SelectList(lstSubJobByJobNo, "DAuditorCode", "DAuditorName");

                ViewData["SubJob_NoByJobNo"] = lstSubJobByJobNo;
            }


            if (Command != string.Empty && Command != null)
            {
                if (Command.ToUpper() == "SEARCH")
                {
                    CostSheetDashBoard = OBJInvoiceInstruction.GetInvoiceInstruction(Convert.ToInt32(IIM.PK_JOB_ID));
                    try
                    {
                        if (CostSheetDashBoard.Rows.Count > 0)
                        {
                            foreach (DataRow dr in CostSheetDashBoard.Rows)
                            {
                                lstCompanyDashBoard.Add(
                                    new InvoiceInstructionModel
                                    {
                                        JobNo = Convert.ToString(dr["Job_Number"]),
                                        SubJobNo = Convert.ToString(dr["SubJob_No"]),
                                        PoAmount = Convert.ToDecimal(dr["PoAmount"]),
                                        InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]),
                                        TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                                        Balance = Convert.ToDecimal(dr["Balance"]),
                                        InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                                        InvoiceNumber = Convert.ToString(dr["InvoiceNumber"]),
                                        INV_ID = Convert.ToInt32(dr["INV_ID"]),
                                        PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                        OrderType = Convert.ToString(dr["OrderType"]),
                                        OrderRate = Convert.ToString(dr["OrderRate"]),
                                        GSTDetail = Convert.ToString(dr["GSTDetail"]),
                                    });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string Error = ex.Message.ToString();
                    }

                    ViewData["CostSheet"] = lstCompanyDashBoard;
                    if (IIM.ChkMultipleSubJobNo == true)
                    {
                        ListPKSubJob = string.Join(",", fc["AuditeeName"]);
                        split = ListPKSubJob.Split(',');
                        ViewData["ListPKSubJobChecked"] = ListPKSubJob;  
                        ViewData["SubJob_NoByJobNo"] = lstSubJobByJobNo;
                    }

                    CompanyDashBoard = OBJInvoiceInstruction.GetreportName(IIM.FromDate, IIM.ToDate, IIM.Job_Number, ListPKSubJob, IIM.OrderType,IIM.PK_JOB_ID);




                    try
                    {
                        lstIVRList.Clear();
                        if (CompanyDashBoard.Rows.Count > 0)
                        {
                            ViewData["NoReport"] = null;
                            Session["count"] = CompanyDashBoard.Rows.Count;
                            foreach (DataRow dr in CompanyDashBoard.Rows)
                            {
                                lstIVRList.Add(
                                    new InvoiceList
                                    {

                                        Id = Convert.ToString(dr["Id"]),
                                        Inspectiondate = Convert.ToString(dr["Inspectiondate"]),
                                        reportName = Convert.ToString(dr["ReportName"]),
                                        CreatedBy = Convert.ToString(dr["CreatedBy"]),

                                    }
                                    );
                            }
                        }
                        else
                        {
                            ViewData["NoReport"] = "No visit Report/IRn available for invoicing against these Sub Jobs.";

                        }
                    }
                    catch (Exception ex)
                    {
                        string Error = ex.Message.ToString();
                    }
                    ViewData["ListDashboard"] = lstIVRList;
                    IIM.ListDashboard = lstIVRList;
                    return View(IIM);
                }
            }
            else if (IIM.INV_ID == 0)
            {
                DataTable INVData = new DataTable();
                INVData = OBJInvoiceInstruction.GetBalanceAmount(IIM.PK_JOB_ID);

                decimal poAmt = Convert.ToDecimal(IIM.PoAmount);

                decimal bln = Convert.ToDecimal(IIM.Balance);
                decimal InvAmount = Convert.ToDecimal(INVData.Rows[0]["Amount"].ToString());
                
                decimal NewinvAmount;
                decimal inamt = Convert.ToDecimal(IIM.InvoiceAmount);
                NewinvAmount = poAmt - (InvAmount + inamt);
                
                IIM.Balance = NewinvAmount;
                //ListPKSubJob1 = string.Join(",", fc["t"]);
                ListPKSubJob1 = Convert.ToString(fc["hdnContent"]);
                split1 = ListPKSubJob1.Split(',');
                if (split1.Length > 0)
                {

                    IIM.rptType = ListPKSubJob1.ToString();
                }
                if (IIM.ChkMultipleSubJobNo == true)
                {
                    ListPKSubJob = string.Join(",", fc["AuditeeName"]);
                    split = ListPKSubJob.Split(',');
                    ViewData["ListPKSubJobChecked"] = ListPKSubJob;

                    int subjobCount = OBJInvoiceInstruction.GetSubJobCount(IIM.PK_JOB_ID);
                    if (split.Length < subjobCount)
                    {
                        foreach (string item in split)
                        {
                            IIM.PK_SubJob_Id = Convert.ToInt32(item);
                            Result = OBJInvoiceInstruction.InsertUpdateInvoiceInstruction(IIM);
                        }
                        OBJInvoiceInstruction.UpdateBalanceInJobMaster(IIM);
                    }
                    else
                    {
                        Result = OBJInvoiceInstruction.InsertUpdateInvoiceInstruction(IIM);
                        OBJInvoiceInstruction.UpdateBalanceInJobMaster(IIM);
                    }
                }
                else
                {
                    Result = OBJInvoiceInstruction.InsertUpdateInvoiceInstruction(IIM);
                    OBJInvoiceInstruction.UpdateBalanceInJobMaster(IIM);
                }
            }
            else
            {
                decimal prvamt = Convert.ToDecimal(Session["PreviousAmount"]);
                decimal bal = Convert.ToDecimal(IIM.Balance);
                decimal total = prvamt + bal;
                decimal invamt = Convert.ToDecimal(IIM.InvoiceAmount);
                decimal abc = total - invamt;
                IIM.Balance = abc;

                Result = OBJInvoiceInstruction.InsertUpdateInvoiceInstruction(IIM);
                OBJInvoiceInstruction.UpdateBalanceInJobMaster(IIM);
            }
           
            if (Result != null && Result != "")
            {
                TempData["UpdateCompany"] = Result;
            }
            //return View();
            return RedirectToAction("InvoiceInstructionDetails", new { PK_Job_ID=IIM.PK_JOB_ID });
        }

        public ActionResult DeleteInstructionDetails(int? INV_ID, int? sid, decimal? blnamt, decimal? invamt)
        {
            DataSet ItemDescriptionData = new DataSet();
            int Result = 0;
            try
            {
                Result = OBJInvoiceInstruction.DeleteInvoiceInstructionData(Convert.ToInt32(INV_ID));
                if (Result != 0)
                {
                    TempData["Success"] = "Invoice Deleted Successfully ...";
                    TempData.Keep();
                    decimal Balance = Convert.ToDecimal(blnamt) + Convert.ToDecimal(invamt);
                    OBJInvoiceInstruction.UpdateBalance(Convert.ToInt32(sid), Convert.ToDecimal(Balance));//===Amount Update in Job Table 
                    TempData["DeleteBranch"] = Result;
                }
                else
                {
                    TempData["Error"] = "Something went Wrong, Please try Again !";
                    TempData.Keep();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("InvoiceInstructionDetails", new { @PK_JOB_ID = Convert.ToInt32(sid) });
            //return RedirectToAction("ItemDescription", IVRNew);
        }
        
        [HttpGet]
        public ActionResult MISOrderStatus()
        {


            List<MISOrderStatus> lmd = new List<MISOrderStatus>();
            DataSet ds = new DataSet();

            ds = OBJInvoiceInstruction.dsMisOrderStatus(); // fill dataset  
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    

                    lmd.Add(new MISOrderStatus
                    {


                        //Job_Number = Convert.ToString(dr["Job_Number"]),
                        //Client_Name = Convert.ToString(dr["Client_Name"]),
                        //Customer_PO_Amount = Convert.ToString(dr["Customer_PO_Amount"]),
                        //Balance = Convert.ToString(dr["Balance"]),
                        //InvoiceAmount = Convert.ToString(dr["InvoiceAmount"])

                        IRNnumber = Convert.ToString(dr["IRN_Number"]),
                        IRNCreatedDate = Convert.ToString(dr["CreatedDate"]),
                        Control_number = Convert.ToString(dr["Sap_And_Controle_No"]),
                        Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                        executing_branch = Convert.ToString(dr["Excuting_Branch"]),
                        Inspection_dates = Convert.ToString(dr["Date_Of_Inspection"]),
                        Project_name = Convert.ToString(dr["Project_Name_Location"]),
                        Client_name = Convert.ToString(dr["Client_Name"]),
                        Vendor_name = Convert.ToString(dr["Vendor_Name_Location"]),
                        PO_Number_date = Convert.ToString(dr["Po_No"]),
                        Sub_vendor_name = Convert.ToString(dr["Sub_Vendor_Name"]),
                        Inspector_names = Convert.ToString(dr["Inspector"]),
                        PO_status = Convert.ToString(dr["Status"]),
                        //IRN_Creation_date = Convert.ToString(dr["IRN_Creation_date"]),
                        Po_amount = Convert.ToString(dr["Customer_PO_Amount"]),
                        Invoiced_amount = Convert.ToString(dr["InvoiceAmount"]),
                        Pending_amount = Convert.ToString(dr["Balance"])



                    });
                }
            }
            //ViewData["OPEReport"] = lmd;
            objMOS.lst1 = lmd;


            return View(objMOS);
        }

        [HttpPost]
        public ActionResult MISOrderStatus(MISOrderStatus MOS)
        {
            if (MOS.FromDate == "" || MOS.FromDate == null)
            {
                return RedirectToAction("MISOrderStatus");
            }


            List<MISOrderStatus> lmd = new List<MISOrderStatus>();
            DataSet ds = new DataSet();

            



            ds = OBJInvoiceInstruction.dsMisOrderStatusByDate(MOS); // fill dataset  
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {


                    lmd.Add(new MISOrderStatus
                    {


                        Job_Number = Convert.ToString(dr["Job_Number"]),
                        Client_Name = Convert.ToString(dr["Client_Name"]),
                        Customer_PO_Amount = Convert.ToString(dr["Customer_PO_Amount"]),
                        Balance = Convert.ToString(dr["Balance"]),
                        InvoiceAmount = Convert.ToString(dr["InvoiceAmount"])



                    });
                }
            }
            //ViewData["OPEReport"] = lmd;
            objMOS.lst1 = lmd;


            return View(objMOS);
        }

        #region ExportTOExcel MISOrderStatus
       // [HttpGet]
        //public ActionResult ExportIndexMISOrderStatus()
        //{
            
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        Int32 row = 2;
        //        Int32 col = 1;

        //        package.Workbook.Worksheets.Add("Data");
        //        IGrid<MISOrderStatus> grid = CreateExportableGridMISOrderStatus();
        //        ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

        //        foreach (IGridColumn column in grid.Columns)
        //        {
        //            sheet.Cells[1, col].Value = column.Title;
        //            sheet.Column(col++).Width = 18;

        //            column.IsEncoded = false;
        //        }

        //        foreach (IGridRow<MISOrderStatus> gridRow in grid.Rows)
        //        {
        //            col = 1;
        //            foreach (IGridColumn column in grid.Columns)
        //                sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

        //            row++;
        //        }

        //        return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
        //    }
        //}
        //private IGrid<MISOrderStatus> CreateExportableGridMISOrderStatus()
        //{
            
        //    IGrid<MISOrderStatus> grid = new Grid<MISOrderStatus>(GetDataMISOrderStatus());
        //    grid.ViewContext = new ViewContext { HttpContext = HttpContext };


        //    grid.Columns.Add(model => model.Service_type).Titled("Service type");
        //    grid.Columns.Add(model => model.Type).Titled("Job Type");


        //    grid.Pager = new GridPager<MISOrderStatus>(grid);
        //    grid.Processors.Add(grid.Pager);
        //    grid.Pager.RowsPerPage = ObjModelQuotationMast.lstQuotationMasterDashBoard1.Count;

        //    foreach (IGridColumn column in grid.Columns)
        //    {
        //        column.Filter.IsEnabled = true;
        //        column.Sort.IsEnabled = true;
        //    }

        //    return grid;
        //}

        //public List<MISOrderStatus> GetDataMISOrderStatus()
        //{

        //    DataTable DTExportToExcel = new DataTable();
        //    #region test
        //    DataTable subJobDashBoard1 = new DataTable();

        //    subJobDashBoard1 = objDalSubjob.GetSubJOBList();
        //    try
        //    {
        //        if (subJobDashBoard1.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in subJobDashBoard1.Rows)
        //            {
        //                lstSubJobDashBoard.Add(
        //                    new SubJobs
        //                    {
        //                        #region
        //                        PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
        //                        PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
        //                        PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),

        //                        #endregion


        //                    }

        //                    );
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    #endregion
        //    ObjModelsubJob.lstSubJobDashBoard1 = lstSubJobDashBoard;
        //    return ObjModelsubJob.lstSubJobDashBoard1;
        //}
        #endregion

    }
}