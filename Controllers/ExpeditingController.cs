using HtmlAgilityPack;
using Newtonsoft.Json;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class ExpeditingController : Controller
    {
        ExpeditingModel objModel = new ExpeditingModel();
        Progress objM = new Progress();
        DALExpeditingReport ObjDAL = new DALExpeditingReport();
        DALInspectionVisitReport objDalVisitReport = new DALInspectionVisitReport();
        DalExpeditingActivity ObjActivity = new DalExpeditingActivity();
        List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();
        CommonControl objCommonControl = new CommonControl();
        DALSubJob objDalSubjob = new DALSubJob();


        #region General
        // GET: Expediting
        public ActionResult ExpeditingReport(int PK_Call_ID)
        {


            Session["VisitReportNo"] = null;
            Session["ExpeditingType"] = null;
            Session["IsComfirmationToHideSave"] = null;//Nikita

            if (PK_Call_ID != 0 && PK_Call_ID != null)
            {
                DataSet DSGetRecordFromExpediting = new DataSet();
                DataSet DSGetRecordFromCall = new DataSet();
                DataSet ds = new DataSet();
                DataTable dsGetStamp = new DataTable();
                List<ExpeditingModel> lstCompanyDashBoard = new List<ExpeditingModel>();

                dsGetStamp = ObjDAL.EditExpeditingReportAttendeesName(PK_Call_ID);

                if (dsGetStamp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGetStamp.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ExpeditingModel
                            {
                                Represting = Convert.ToString(dr["Represting"]),
                                Name = Convert.ToString(dr["Name"]),
                                Email = Convert.ToString(dr["EmailID"]),
                                Designation = Convert.ToString(dr["Designation"])


                            }
                            );
                    }
                }


                ViewBag.lstObservation = lstCompanyDashBoard;

                DataTable DSdata = new DataTable();

                DSdata = ObjDAL.GetdistributionId(PK_Call_ID);

                if (DSdata.Rows.Count > 0)
                {
                    objModel.checkCustomer = Convert.ToBoolean(DSdata.Rows[0]["DCustomer"]);
                    objModel.Vendor = Convert.ToBoolean(DSdata.Rows[0]["DVendor"]);
                    objModel.TUVI = Convert.ToBoolean(DSdata.Rows[0]["DTuvi"]);

                }


                DSGetRecordFromExpediting = ObjDAL.EditExpeditingReport(PK_Call_ID);

                if (DSGetRecordFromExpediting.Tables[0].Rows.Count > 0)
                {
                    #region Get Activity Data  
                    ds = ObjActivity.GetData(Convert.ToInt32(PK_Call_ID)); // Get data from callid (tblnoninspectionActivity)

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new NonInspectionActivity
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                ActivityType = Convert.ToString(dr["ActivityType"]),
                                Location = Convert.ToString(dr["Location"]),
                                DateSE = Convert.ToString(dr["DateSE"]),
                                //EndDate = Convert.ToString(dr["enddate"]),
                                //StartDate = Convert.ToString(dr["StartDate"]),
                                //EndDate = Convert.ToString(dr["EndDate"]),
                                ServiceCode = Convert.ToString(dr["ServiceCode"]),
                                Description = Convert.ToString(dr["Description"]),
                                StartTime = Convert.ToDouble(dr["StartTime"]),
                                EndTime = Convert.ToDouble(dr["EndTime"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                TravelTime = Convert.ToDouble(dr["TravelTime"])

                            });
                        }

                        ViewBag.Dates = lmd;
                    }
                    #endregion


                    Session["IsComfirmationToHideSave"] = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["IsComfirmation"]);//Nikita
                    objModel.CMP_ID = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["FK_CMP_ID"]);
                    objModel.IsComfirmation = Convert.ToBoolean(DSGetRecordFromExpediting.Tables[0].Rows[0]["IsComfirmation"]);//Nikita

                    objModel.Date_Of_Expediting = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Date_Of_Expediting"]);
                    objModel.PK_call_id = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["PK_call_id"]);
                    objModel.Controle_No = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Controle_No"]);
                    objModel.Executing_branch = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Executing_branch"]);
                    objModel.Executing_branchId = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Executing_branchId"]);
                    objModel.Customer_Name = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Customer_Name"]);
                    objModel.End_Customer_Name = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["End_Customer_Name"]);
                    objModel.Project_Name = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Project_Name"]);
                    objModel.DEC_PMC_EPC_Name = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
                    objModel.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
                    objModel.NotificationNumber = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["NotificationNumber"]);
                    objModel.ExpeditingLocation = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["ExpeditingLocation"]);
                    objModel.VendorName = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["VendorName"]);
                    objModel.Po_No = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Po_No"]);
                    objModel.Po_Date = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Po_Date"]);
                    objModel.Sub_VendorName = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Sub_VendorName"]);
                    objModel.SubPo_No = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["SubPo_No"]);
                    objModel.SubPo_Date = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["SubPo_Date"]);
                    objModel.Contractual_DeliveryDate = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Contractual_DeliveryDate"]);
                    objModel.Expected_DeliveryDate = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Expected_DeliveryDate"]);
                    objModel.CurrentOverallPOStatus = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["CurrentOverallPOStatus"]);
                    objModel.CallIDs = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["CallIDs"]);
                    objModel.Date_Of_Inspection = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Date_Of_Expediting"]);
                    objModel.ReportName = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["ReportNo"]);
                    Session["VisitReportNo"] = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["ReportNo"]);
                    objModel.PK_Expediting_Id = Convert.ToInt32(DSGetRecordFromExpediting.Tables[0].Rows[0]["PK_Expediting_Id"]);
                    //added by shrutika salve 25012024
                    objModel.OverAll = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["OverAll"]);
                    objModel.LastExpeditingtypeandDate = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["LastExpeditingtypeandDate"]);
                    objModel.ExpeditingType = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["ExpeditingType"]);
                    objModel.AssignmentNumber = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["assignmentNumber"]);
                    Session["ExpeditingType"] = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["ExpeditingType"]);
                    //added by shrutika salve 14/04/2023
                    objModel.Sub_Job_No = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["Sub_Job"]);
                    objModel.closureRemarks = Convert.ToString(DSGetRecordFromExpediting.Tables[0].Rows[0]["ClosureRemarks"]);

                }
                else
                {
                    try
                    {

                        DSGetRecordFromCall = ObjDAL.EditExpeditingReportByCall(PK_Call_ID);
                        if (DSGetRecordFromCall.Tables[0].Rows.Count > 0)
                        {
                            //objModel.PK_Expediting_Id = Convert.ToInt32(DSGetRecordFromCall.Tables[0].Rows[0]["PK_Expediting_Id"]);
                            Session["IsComfirmationToHideSave"] = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["isComfirmation"]);//Nikita
                            objModel.IsComfirmation = Convert.ToBoolean(DSGetRecordFromCall.Tables[0].Rows[0]["isComfirmation"]);//Nikita
                            objModel.Date_Of_Expediting = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Actual_Visit_Date"]);
                            objModel.PK_call_id = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["PK_call_id"]);
                            objModel.Controle_No = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Job"]);
                            objModel.Executing_branch = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Executing_branch"]);
                            objModel.Executing_branchId = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Br_id"]);
                            objModel.Customer_Name = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Company_Name"]);
                            objModel.End_Customer_Name = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["End_Customer"]);
                            objModel.Project_Name = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Project_Name"]);
                            objModel.DEC_PMC_EPC_Name = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["DECName"]);
                            objModel.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["DECNumber"]);
                            // objModel.NotificationNumber = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["NotificationNumber"]);
                            objModel.ExpeditingLocation = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Job_Location"]);
                            objModel.VendorName = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Vendor_Name"]);
                            objModel.Po_No = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Po_Number"]);
                            objModel.Po_Date = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Date_of_PO"]);
                            objModel.Sub_VendorName = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["SubVendorName"]);
                            objModel.SubPo_No = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["SubVendorPoNo"]);
                            objModel.SubPo_Date = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["SubVendorPoDate"]);
                            objModel.CallIDs = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["CallIDs"]);
                            objModel.PK_SubJob_Id = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["PK_SubJob_Id"]);
                            objModel.Sub_Job_No = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Sub_Job"]);
                            //objModel.Contractual_DeliveryDate = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Contractual_DeliveryDate"]);
                            //objModel.Expected_DeliveryDate = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["Expected_DeliveryDate"]);
                            //objModel.CurrentOverallPOStatus = Convert.ToString(DSGetRecordFromCall.Tables[0].Rows[0]["CurrentOverallPOStatus"]);


                        }

                        #region Bind Activity 23 july 

                        DataSet dtGateDate = new DataSet();
                        dtGateDate = ObjActivity.GetDate(PK_Call_ID);  //if data is null Get dates from visit report

                        string Date = "";
                        string Dates;
                        Date = objModel.Date_Of_Expediting;
                        Dates = Date.Trim().TrimStart(','); // to remove first comma

                        string strCallID = "";
                        string strCallIDs;
                        Date = objModel.Date_Of_Expediting;
                        Dates = Date.Trim().TrimStart(','); // to remove first comma

                        strCallID = objModel.CallIDs;
                        strCallIDs = strCallID.Trim().TrimStart(','); // to remove first comma

                        string[] ArrDates = Dates.Split(',');
                        string[] ArrCallIDs = strCallIDs.Split(',');



                        for (int cnt = 0; cnt < ArrDates.Count(); cnt++)
                        {
                            lmd.Add(new NonInspectionActivity
                            {
                                DateSE = ArrDates[cnt].ToString(),
                                CallId = ArrCallIDs[cnt].ToString(),
                                Description = dtGateDate.Tables[0].Rows[0]["Vendor_Name_Location"].ToString(),

                            });
                        }

                        ViewData["Dates"] = ArrDates;
                        // ViewBag.Dates = ArrDates;
                        ViewBag.Dates = lmd;
                        ViewBag.DataEntryView = "Yes";


                        #endregion


                    }
                    catch (Exception ex)
                    {
                        string Error = ex.Message.ToString();
                        return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = Error });
                    }
                }

            }
            return View(objModel);

        }


        [HttpPost]
        public ActionResult ExpeditingReport(ExpeditingModel Exp, NonInspectionActivity R)
        {
            string Result = string.Empty;
            try
            {
                if (Exp.ReportName != null)
                {
                    if (Exp.Emp != null)
                    {
                        if (Exp.PK_call_id != "")
                        {
                            Result = ObjDAL.DeleteAttendees(Exp.PK_call_id);
                        }
                        foreach (var item in Exp.Emp)
                        {

                            Exp.Represting = item.Represting;
                            Exp.Name = item.Name;
                            Exp.Designation = item.Designation;
                            Exp.Email = item.Email;
                            Exp.PK_call_id = Exp.PK_call_id;
                            Exp.PK_Expediting_Id = Exp.PK_Expediting_Id;

                            Result = ObjDAL.InsertUpdateAttendees(Exp);

                        }
                    }



                    Result = ObjDAL.UpdateDistributionlist(Exp);

                    #region Check valid Activity
                    if (R.Activity != null)
                    {
                        foreach (var item in R.Activity)
                        {
                            // int CurrentTotal = 
                            DateTime StDt = Convert.ToDateTime(item.StartDate);
                            R.DateSE = StDt.ToString("dd/MM/yyyy");
                            R.StartTime = item.StartTime;
                            R.EndTime = item.EndTime;
                            R.TravelTime = item.TravelTime;
                            R.PK_Call_ID = R.PK_Call_ID;
                            R.CallId = Convert.ToString(item.CallId);
                            //10 Aug
                            Double CurrentTotal = Convert.ToDouble(R.StartTime) + Convert.ToDouble(R.EndTime) + Convert.ToDouble(R.TravelTime);
                            #region Chk Previous Entry
                            DataTable DTValidateTT = new DataTable();
                            {
                                DTValidateTT = objDalVisitReport.CheckPreviousActivityWithCallId(R.DateSE, R.PK_Call_ID);
                            }
                            if (R.PK_Call_ID >= 0)
                            {
                                DTValidateTT = objDalVisitReport.CheckPreviousActivityWithCallId(R.DateSE, R.PK_Call_ID);
                            }
                            else
                            {
                                DTValidateTT = objDalVisitReport.CheckPreviousActivity(R.DateSE);
                            }
                            DTValidateTT = objDalVisitReport.CheckPreviousActivity(R.DateSE);

                            DataTable DTChkLeave = new DataTable();
                            DTChkLeave = objDalVisitReport.CheckIfLeavePresent(R.DateSE);

                            if (DTChkLeave.Rows.Count > 0)
                            {
                                TempData["ErrLeave"] = "Leave has been added for " + StDt.ToString("dd/MM/yyyy");
                                return RedirectToAction("ExpeditingReport", "Expediting", Exp.PK_call_id);
                            }
                            else
                            {
                                if (DTValidateTT.Rows.Count > 0)
                                {
                                    //10 Aug
                                    //int PriviousTotal = Convert.ToInt32(DTValidateTT.Rows[0]["StartTime"]) + Convert.ToInt32(DTValidateTT.Rows[0]["EndTime"]) + Convert.ToInt32(DTValidateTT.Rows[0]["TravelTime"]);
                                    //int GrandTotal = PriviousTotal + CurrentTotal;
                                    Double PriviousTotal = Convert.ToDouble(DTValidateTT.Rows[0]["StartTime"]) + Convert.ToDouble(DTValidateTT.Rows[0]["EndTime"]) + Convert.ToDouble(DTValidateTT.Rows[0]["TravelTime"]);

                                    Double GrandTotal = PriviousTotal + CurrentTotal;


                                    if (GrandTotal > 24)
                                    {
                                        TempData["ErrAll24"] = "Exceeded limit of 24 hours for the day " + StDt.ToString("dd/MM/yyyy");
                                        return RedirectToAction("ExpeditingReport", "VisitReport", Exp.PK_call_id);
                                    }
                                }
                                else if (CurrentTotal > 24)
                                {
                                    TempData["ErrCurrent24"] = "Exceeded limit of 24 hours for the day " + StDt.ToString("dd/MM/yyyy");
                                    return RedirectToAction("ExpeditingReport", "VisitReport", Exp.PK_call_id);
                                }
                                else
                                {

                                }
                            }




                            #endregion
                        }
                    }


                    #endregion

                    #region Save Activity 
                    if (R.Activity != null)
                    {
                        foreach (var item in R.Activity)
                        {
                            int total = Convert.ToInt32(item.StartTime);
                            R.TotalTime = total;
                            DateTime StDt = Convert.ToDateTime(item.StartDate);
                            R.DateSE = StDt.ToString("dd/MM/yyyy");
                            R.StartTime = item.StartTime;
                            R.EndTime = item.EndTime;
                            R.TravelTime = item.TravelTime;
                            R.Description = item.Description;
                            R.PK_Call_ID = R.PK_Call_ID;
                            R.CallId = Convert.ToString(item.CallId);
                            //added by shrutika salve 24052024
                            R.PK_SubJob_Id = Convert.ToString(Exp.PK_SubJob_Id);
                            R.Sub_Job = Exp.Sub_Job_No;

                            Result = ObjActivity.InsertUpdateActivityExpediting(R);

                            if (Convert.ToInt16(Result) > 0)
                            {

                                TempData["message"] = "Record Added Successfully...";
                            }
                            else
                            {
                                TempData["message"] = "Something went Wrong! Please try Again";
                            }
                        }
                    }





                    #endregion
                    Result = ObjDAL.InsertUpdateExpediting(Exp);
                    if (Result != "" && Result != null)
                    {
                        return RedirectToAction("ExpeditingReport", "Expediting", new { @PK_Call_ID = Exp.PK_call_id });
                    }
                }
                else
                {
                    if (Exp.PK_Expediting_Id == 0)
                    {
                        foreach (var item in Exp.Emp)
                        {

                            Exp.Represting = item.Represting;
                            Exp.Name = item.Name;
                            Exp.Designation = item.Designation;
                            Exp.Email = item.Email;
                            Exp.PK_call_id = Exp.PK_call_id;
                            Exp.PK_Expediting_Id = Exp.PK_Expediting_Id;

                            Result = ObjDAL.InsertUpdateAttendees(Exp);

                        }

                        Result = ObjDAL.UpdateDistributionlist(Exp);

                        #region Check valid Activity
                        if (R.Activity != null)
                        {
                            foreach (var item in R.Activity)
                            {
                                // int CurrentTotal = 
                                DateTime StDt = Convert.ToDateTime(item.StartDate);
                                R.DateSE = StDt.ToString("dd/MM/yyyy");
                                R.StartTime = item.StartTime;
                                R.EndTime = item.EndTime;
                                R.TravelTime = item.TravelTime;
                                R.PK_Call_ID = R.PK_Call_ID;
                                R.CallId = Convert.ToString(item.CallId);
                                //10 Aug
                                Double CurrentTotal = Convert.ToDouble(R.StartTime) + Convert.ToDouble(R.EndTime) + Convert.ToDouble(R.TravelTime);
                                #region Chk Previous Entry
                                DataTable DTValidateTT = new DataTable();
                                {
                                    DTValidateTT = objDalVisitReport.CheckPreviousActivityWithCallId(R.DateSE, R.PK_Call_ID);
                                }
                                if (R.PK_Call_ID >= 0)
                                {
                                    DTValidateTT = objDalVisitReport.CheckPreviousActivityWithCallId(R.DateSE, R.PK_Call_ID);
                                }
                                else
                                {
                                    DTValidateTT = objDalVisitReport.CheckPreviousActivity(R.DateSE);
                                }
                                DTValidateTT = objDalVisitReport.CheckPreviousActivity(R.DateSE);

                                DataTable DTChkLeave = new DataTable();
                                DTChkLeave = objDalVisitReport.CheckIfLeavePresent(R.DateSE);

                                if (DTChkLeave.Rows.Count > 0)
                                {
                                    TempData["ErrLeave"] = "Leave has been added for " + StDt.ToString("dd/MM/yyyy");
                                    return RedirectToAction("ExpeditingReport", "Expediting", Exp.PK_call_id);
                                }
                                else
                                {
                                    if (DTValidateTT.Rows.Count > 0)
                                    {
                                        //10 Aug
                                        //int PriviousTotal = Convert.ToInt32(DTValidateTT.Rows[0]["StartTime"]) + Convert.ToInt32(DTValidateTT.Rows[0]["EndTime"]) + Convert.ToInt32(DTValidateTT.Rows[0]["TravelTime"]);
                                        //int GrandTotal = PriviousTotal + CurrentTotal;
                                        Double PriviousTotal = Convert.ToDouble(DTValidateTT.Rows[0]["StartTime"]) + Convert.ToDouble(DTValidateTT.Rows[0]["EndTime"]) + Convert.ToDouble(DTValidateTT.Rows[0]["TravelTime"]);

                                        Double GrandTotal = PriviousTotal + CurrentTotal;


                                        if (GrandTotal > 24)
                                        {
                                            TempData["ErrAll24"] = "Exceeded limit of 24 hours for the day " + StDt.ToString("dd/MM/yyyy");
                                            return RedirectToAction("ExpeditingReport", "VisitReport", Exp.PK_call_id);
                                        }
                                    }
                                    else if (CurrentTotal > 24)
                                    {
                                        TempData["ErrCurrent24"] = "Exceeded limit of 24 hours for the day " + StDt.ToString("dd/MM/yyyy");
                                        return RedirectToAction("ExpeditingReport", "VisitReport", Exp.PK_call_id);
                                    }
                                    else
                                    {

                                    }
                                }




                                #endregion
                            }
                        }


                        #endregion

                        #region Save Activity 
                        if (R.Activity != null)
                        {
                            foreach (var item in R.Activity)
                            {
                                int total = Convert.ToInt32(item.StartTime);
                                R.TotalTime = total;
                                DateTime StDt = Convert.ToDateTime(item.StartDate);
                                R.DateSE = StDt.ToString("dd/MM/yyyy");
                                R.StartTime = item.StartTime;
                                R.EndTime = item.EndTime;
                                R.TravelTime = item.TravelTime;
                                R.Description = item.Description;
                                R.PK_Call_ID = R.PK_Call_ID;
                                R.CallId = Convert.ToString(item.CallId);
                                R.PK_SubJob_Id = Convert.ToString(Exp.PK_SubJob_Id);
                                R.Sub_Job = Exp.Sub_Job_No;

                                Result = ObjActivity.InsertUpdateActivityExpediting(R);

                                if (Convert.ToInt16(Result) > 0)
                                {

                                    TempData["message"] = "Record Added Successfully...";
                                }
                                else
                                {
                                    TempData["message"] = "Something went Wrong! Please try Again";
                                }
                            }
                        }



                        #endregion




                        Result = ObjDAL.InsertUpdateExpediting(Exp);
                        Session["PK_Call_ID"] = null;
                        if (Result != "" && Result != null)
                        {
                            return RedirectToAction("ExpeditingReport", "Expediting", new { @PK_Call_ID = Exp.PK_call_id });
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.ToString();
                string Error = ex.Message.ToString();
                return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = Error });
            }
            return View();
        }


        [HttpGet]
        public ActionResult ExportIndex(ExpeditingModel IVR)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<ExpeditingModel> grid = CreateExportableGrid(IVR);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<ExpeditingModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                //aaded by nikita on 06092023

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "Expediting Reports" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<ExpeditingModel> CreateExportableGrid(ExpeditingModel IVR)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<ExpeditingModel> grid = new Grid<ExpeditingModel>(GetData(IVR));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Columns.Add(model => model.ReportName).Titled("Report No");
            grid.Columns.Add(model => model.ReportNo).Titled("Download File");
            grid.Columns.Add(model => model.Call_no).Titled("Call No");
            grid.Columns.Add(model => model.Sub_Job_No).Titled("Sub Job No");
            grid.Columns.Add(model => model.Sap_no).Titled("Job No");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            grid.Columns.Add(model => model.Customer_Name).Titled("Customer Name");
            grid.Columns.Add(model => model.VendorName).Titled("Vendor Name");
            grid.Columns.Add(model => model.Po_No).Titled("Po No");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector Name");
            grid.Columns.Add(model => model.InspectionDate).Titled("Inspection Date");
            grid.Columns.Add(model => model.ReportDate).Titled("Report Date");
            grid.Columns.Add(model => model.Product_item).Titled("Item to be inspected");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.Executing_branch).Titled("Executing Branch");

            grid.Columns.Add(model => model.Pk_RM_id).Titled("PK_RM_ID");



            grid.Pager = new GridPager<ExpeditingModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objModel.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<ExpeditingModel> GetData(ExpeditingModel IVR)
        {
            DataTable CostSheetDashBoard = new DataTable();
            List<ExpeditingModel> lstDashBoard = new List<ExpeditingModel>();

            try
            {
                CostSheetDashBoard = ObjDAL.GetReportDashboard();
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstDashBoard.Add(
                            new ExpeditingModel
                            {

                                ReportName = Convert.ToString(dr["ReportNo"]),
                                Edit = Convert.ToString(dr["Edit"]),
                                PK_call_id = Convert.ToString(dr["PK_call_id"]),
                                Sub_Job_No = Convert.ToString(dr["SubJob_No"]),
                                Sap_no = Convert.ToString(dr["Sap_And_Controle_No"]),
                                Customer_Name = Convert.ToString(dr["clientName"]),
                                Po_No = Convert.ToString(dr["po_No"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                InspectionDate = Convert.ToString(dr["InspectionDate"]),
                                ReportDate = Convert.ToString(dr["ReportDate"]),
                                Product_item = Convert.ToString(dr["Product_item"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Executing_branch = Convert.ToString(dr["Excuting_Branch"]),
                                Call_no = Convert.ToString(dr["Call_No"]),
                                Project_Name = Convert.ToString(dr["Project_Name_Location"]),
                                End_Customer_Name = Convert.ToString(dr["Client_Name"]),
                                VendorName = Convert.ToString(dr["Vendor_Name_Location"]),
                                Pk_RM_id = Convert.ToInt32(dr["pk_rm_id"]),
                                PK_Expediting_Id = Convert.ToInt32(dr["PK_Expediting_Id"]),
                                ReportNo = Convert.ToString(dr["ReportName"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();

            }

            ViewData["CostSheet"] = lstDashBoard;
            ViewBag.ExportToExcel = lstDashBoard;
            // IVR.lst1 = lstCompanyDashBoard;

            objModel.lst1 = lstDashBoard;
            return objModel.lst1;
        }


        #endregion

        #region List
        [HttpGet]
        public ActionResult ExpeditingReportList(ExpeditingModel Exp)
        {
            DataTable CostSheetDashBoard = new DataTable();
            List<ExpeditingModel> lstDashBoard = new List<ExpeditingModel>();

            try
            {
                CostSheetDashBoard = ObjDAL.GetReportDashboard();
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstDashBoard.Add(
                            new ExpeditingModel
                            {

                                ReportName = Convert.ToString(dr["ReportNo"]),
                                Edit = Convert.ToString(dr["Edit"]),
                                PK_call_id = Convert.ToString(dr["PK_call_id"]),
                                Sub_Job_No = Convert.ToString(dr["SubJob_No"]),
                                Sap_no = Convert.ToString(dr["Sap_And_Controle_No"]),
                                Customer_Name = Convert.ToString(dr["clientName"]),
                                Po_No = Convert.ToString(dr["po_No"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                InspectionDate = Convert.ToString(dr["InspectionDate"]),
                                ReportDate = Convert.ToString(dr["ReportDate"]),
                                Product_item = Convert.ToString(dr["Product_item"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Executing_branch = Convert.ToString(dr["Excuting_Branch"]),
                                Call_no = Convert.ToString(dr["Call_No"]),
                                Project_Name = Convert.ToString(dr["Project_Name_Location"]),
                                End_Customer_Name = Convert.ToString(dr["Client_Name"]),
                                VendorName = Convert.ToString(dr["Vendor_Name_Location"]),
                                Pk_RM_id = Convert.ToInt32(dr["pk_rm_id"]),
                                PK_Expediting_Id = Convert.ToInt32(dr["PK_Expediting_Id"]),
                                ReportNo = Convert.ToString(dr["ReportName"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = Error });
            }

            Exp.lst1 = lstDashBoard;
            return View(Exp);

        }
        #endregion

        #region PO Item Detail
        [HttpGet]
        public ActionResult ExpItemDescription(ExpItemDescription IVR, int? PK_Call_Id, int? PK_Item_Detail)
        {
            IVR.PK_Item_Detail = Convert.ToInt32(PK_Item_Detail);

            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<ExpItemDescription> lstCompanyDashBoard = new List<ExpItemDescription>();

            #region Bind UOM
            List<NameCode> lstProjectType = new List<NameCode>();
            DataSet dsUnit = new DataSet();
            dsUnit = objDalVisitReport.Measurement();

            if (dsUnit.Tables[0].Rows.Count > 0)
            {
                lstProjectType = (from n in dsUnit.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(dsUnit.Tables[0].Columns["Name"].ToString()),
                                      Code = n.Field<Int32>(dsUnit.Tables[0].Columns["Id"].ToString())

                                  }).ToList();
            }
            ViewBag.UOM = lstProjectType;

            var Data = objDalVisitReport.GetitemName(PK_Call_Id);
            ViewBag.Binditemdata = new SelectList(Data, "Value", "Text");


            #endregion  
            if (IVR.PK_Item_Detail != 0)
            {
                ItemDescriptionData = ObjDAL.EditItemDescription(IVR);
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    IVR.PK_Item_Detail = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_Item_Detail"]);
                    IVR.PK_Call_ID = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_Call_ID"]);
                    IVR.PO_Item_Number = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["PO_Item_Number"]);
                    IVR.ItemCode = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["ItemCode"]);
                    IVR.ItemDescription = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["ItemDescription"]);
                    IVR.Equipment_TagNo = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Equipment_TagNo"]);
                    IVR.Quantity = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Quantity"]);
                    IVR.Unit = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Unit"]);
                    IVR.Contractual_DeliveryDateAsPerPO = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Contractual_DeliveryDateAsPerPO"]);
                    IVR.EstimatedDeliveryDate = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["EstimatedDeliveryDate"]);
                    IVR.itemName = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["itemName"]);



                    CostSheetDashBoard = ObjDAL.GetitemDescription(IVR.PK_Call_ID);
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new ExpItemDescription
                                {

                                    Unit = Convert.ToString(dr["Unit"]),
                                    PK_Item_Detail = Convert.ToInt32(dr["PK_Item_Detail"]),
                                    PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                    PO_Item_Number = Convert.ToString(dr["PO_Item_Number"]),
                                    ItemCode = Convert.ToString(dr["ItemCode"]),
                                    ItemDescription = Convert.ToString(dr["ItemDescription"]),
                                    Equipment_TagNo = Convert.ToString(dr["Equipment_TagNo"]),
                                    Quantity = Convert.ToString(dr["Quantity"]),
                                    Contractual_DeliveryDateAsPerPO = Convert.ToString(dr["Contractual_DeliveryDateAsPerPO"]),
                                    EstimatedDeliveryDate = Convert.ToString(dr["EstimatedDeliveryDate"]),
                                    TotalConcerns = Convert.ToString(dr["TotalConcerns"]),
                                    actualprogress = Convert.ToString(dr["actualprogress"]),
                                    progressStatus = Convert.ToString(dr["Progress_Status"]),
                                    itemName = Convert.ToString(dr["ItemName"]),
                                }
                                );
                        }
                    }

                }

            }
            else
            {
                try
                {
                    CostSheetDashBoard = ObjDAL.GetitemDescription(IVR.PK_Call_ID);
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new ExpItemDescription
                                {

                                    Unit = Convert.ToString(dr["Unit"]),
                                    PK_Item_Detail = Convert.ToInt32(dr["PK_Item_Detail"]),
                                    PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                    PO_Item_Number = Convert.ToString(dr["PO_Item_Number"]),
                                    ItemCode = Convert.ToString(dr["ItemCode"]),
                                    ItemDescription = Convert.ToString(dr["ItemDescription"]),
                                    Equipment_TagNo = Convert.ToString(dr["Equipment_TagNo"]),
                                    Quantity = Convert.ToString(dr["Quantity"]),
                                    Contractual_DeliveryDateAsPerPO = Convert.ToString(dr["Contractual_DeliveryDateAsPerPO"]),
                                    EstimatedDeliveryDate = Convert.ToString(dr["EstimatedDeliveryDate"]),
                                    TotalConcerns = Convert.ToString(dr["TotalConcerns"]),
                                    actualprogress = Convert.ToString(dr["actualprogress"]),
                                    progressStatus = Convert.ToString(dr["Progress_Status"]),
                                    itemName = Convert.ToString(dr["ItemName"]),

                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                    return RedirectToAction("ErrorPage", "InspectionReleaseNote", new { @Error = Error });
                }
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(IVR);
        }

        //[HttpPost]
        //public ActionResult ExpItemDescription(ExpItemDescription Exp , HttpPostedFileBase FileUpload1)
        //{
        //    string Result = string.Empty;
        //    int PK_Call_Id;
        //    if (Exp.PK_Item_Detail == 0)
        //    {
        //        try
        //        {

        //            if (Exp.PK_Item_Detail == 0)
        //            {
        //                Result = ObjDAL.InsertItemDescription(Exp);
        //            }
        //            else
        //            {
        //               
        //            }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    else
        //    {
        //        Result = ObjDAL.UpdateItemDescription(Exp);
        //    }
        //    Exp.PK_Call_ID = Exp.PK_Call_ID;

        //    return RedirectToAction("ExpItemDescription", "Expediting", new { PK_Call_Id = Exp.PK_Call_ID }   );
        //}


        [HttpPost]
        public ActionResult ExpItemDescription(ExpItemDescription Exp, HttpPostedFileBase FileUpload1)
        {
            string Result = string.Empty;
            int PK_Call_Id;
            //if (Exp.PK_Item_Detail == 0)
            //{
            try
            {
                if (Exp.PK_Item_Detail == 0)
                {


                    #region Excel Upload code
                    Random rnd = new Random();
                    int myRandomNo = rnd.Next(10000000, 99999999);
                    string strmyRandomNo = Convert.ToString(myRandomNo);

                    HttpPostedFileBase files = FileUpload1;

                    if (FileUpload1 != null)
                    {
                        if (files != null && !string.IsNullOrEmpty(files.FileName) || files.FileName.Contains(".xlsx") && files.FileName.Contains(".xlsm"))
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            string filePath = string.Empty;
                            string fileName = files.FileName;
                            string fileContentType = files.ContentType;
                            byte[] fileBytes = new byte[files.ContentLength];
                            var data1 = files.InputStream.Read(fileBytes, 0, Convert.ToInt32(files.ContentLength));
                            var package = new ExcelPackage(files.InputStream);



                            #region save file to dir
                            //string path = Server.MapPath("~/Content/JobDocument/");
                            string path1 = Server.MapPath("~/ExpeditingExcel/");
                            if (!Directory.Exists(path1))
                            {
                                Directory.CreateDirectory(path1);
                            }


                            filePath = path1 + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);



                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);


                            }

                            string extension1 = Path.GetExtension(strmyRandomNo + FileUpload1.FileName);
                            FileUpload1.SaveAs(filePath);


                            filePath = path1 + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                            #endregion

                            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                            Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath);

                            String[] excelSheets = new String[excelBook.Worksheets.Count];
                            int i = 0;
                            foreach (Microsoft.Office.Interop.Excel.Worksheet wSheet in excelBook.Worksheets)
                            {
                                excelSheets[i] = wSheet.Name;
                                int RowsCount = wSheet.UsedRange.Rows.Count;

                                if (excelSheets[i] == "Item Description")
                                {
                                    for (int j = 2; j <= RowsCount; j++)
                                    {
                                        //Convert.ToString(workSheet.Cells[rowIterator, 1].Value);
                                        Exp.PO_Item_Number = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
                                        if (Exp.PO_Item_Number != null)
                                        {
                                            Exp.ItemCode = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 2]).Value);
                                            Exp.ItemDescription = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 3]).Value);
                                            String Unit = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 4]).Value);
                                            if (Unit == "number") { Exp.Unit = "1"; }
                                            else if (Unit == "meter") { Exp.Unit = "2"; }
                                            else if (Unit == "km") { Exp.Unit = "3"; }
                                            else if (Unit == "meter (number)") { Exp.Unit = "4"; }
                                            else if (Unit == "km (number)") { Exp.Unit = "5"; }
                                            else if (Unit == "each") { Exp.Unit = "6"; }
                                            else if (Unit == "Piece") { Exp.Unit = "7"; }
                                            else if (Unit == "Test sample") { Exp.Unit = "8"; }
                                            else if (Unit == "AU - All Unit") { Exp.Unit = "9"; }
                                            else if (Unit == "lot") { Exp.Unit = "10"; }
                                            else if (Unit == "Set") { Exp.Unit = "11"; }
                                            else if (Unit == "Running Meter") { Exp.Unit = "12"; }
                                            else if (Unit == "kg") { Exp.Unit = "13"; }
                                            else if (Unit == "metric ton (tonne)") { Exp.Unit = "14"; }
                                            else if (Unit == "ton") { Exp.Unit = "15"; }
                                            else if (Unit == "cubic millimetre") { Exp.Unit = "16"; }
                                            else if (Unit == "cubic centimeter") { Exp.Unit = "17"; }
                                            else if (Unit == "cubic meter") { Exp.Unit = "18"; }
                                            else if (Unit == "cubic inch") { Exp.Unit = "19"; }
                                            else if (Unit == "cubic foot") { Exp.Unit = "20"; }
                                            else if (Unit == "mm") { Exp.Unit = "21"; }
                                            else if (Unit == "cm") { Exp.Unit = "22"; }
                                            else if (Unit == "in") { Exp.Unit = "23"; }
                                            else if (Unit == "foot") { Exp.Unit = "24"; }
                                            else if (Unit == "mile") { Exp.Unit = "25"; }
                                            else if (Unit == "yard") { Exp.Unit = "26"; }
                                            else if (Unit == "liter") { Exp.Unit = "27"; }
                                            else if (Unit == "kl") { Exp.Unit = "28"; }
                                            else if (Unit == "cl") { Exp.Unit = "29"; }
                                            else if (Unit == "ml") { Exp.Unit = "30"; }
                                            else if (Unit == "g") { Exp.Unit = "31"; }
                                            else if (Unit == "lb") { Exp.Unit = "32"; }
                                            else if (Unit == "oz") { Exp.Unit = "33"; }
                                            else if (Unit == "Sq. mm") { Exp.Unit = "34"; }
                                            else if (Unit == "Sq. cm") { Exp.Unit = "35"; }
                                            else if (Unit == "Sq. meter") { Exp.Unit = "36"; }
                                            else if (Unit == "Sq. in") { Exp.Unit = "37"; }
                                            else if (Unit == "Sq. foot") { Exp.Unit = "38"; }
                                            else
                                            {
                                                Exp.Unit = "";

                                            }

                                            Exp.Quantity = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 5]).Value);
                                            //Exp.Equipment_TagNo = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 6]).Value);
                                            Microsoft.Office.Interop.Excel.Range deliveryDateCell = (Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 6];
                                            DateTime deliveryDateValue = Convert.ToDateTime(deliveryDateCell.Value);
                                            string deliveryDateString = deliveryDateValue.ToString("dd/MM/yyyy");
                                            Exp.Contractual_DeliveryDateAsPerPO = deliveryDateString;

                                            Microsoft.Office.Interop.Excel.Range EstimatedDateCell = (Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 7];
                                            DateTime EstimatedDate = Convert.ToDateTime(EstimatedDateCell.Value);
                                            string EstimatedDateValue = EstimatedDate.ToString("dd/MM/yyyy");
                                            Exp.EstimatedDeliveryDate = EstimatedDateValue;
                                            //Exp.Accepted_Quantity = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 7]).Value);
                                            //Exp.Status = "1";
                                            //Exp.Type = "IVR";
                                            //Exp.PK_CALL_ID = PK_Call_Id;
                                            Result = ObjDAL.InsertUpdateItemDescription(Exp);
                                        }

                                    }

                                    excelBook.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        Result = ObjDAL.InsertItemDescription(Exp);
                    }
                    #endregion


                }
                else
                {
                    #region Excel Upload code
                    Random rnd = new Random();
                    int myRandomNo = rnd.Next(10000000, 99999999);
                    string strmyRandomNo = Convert.ToString(myRandomNo);

                    HttpPostedFileBase files = FileUpload1;

                    if (FileUpload1 != null)
                    {
                        if (files != null && !string.IsNullOrEmpty(files.FileName) || files.FileName.Contains(".xlsx") && files.FileName.Contains(".xlsm"))
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            string filePath = string.Empty;
                            string fileName = files.FileName;
                            string fileContentType = files.ContentType;
                            byte[] fileBytes = new byte[files.ContentLength];
                            var data1 = files.InputStream.Read(fileBytes, 0, Convert.ToInt32(files.ContentLength));
                            var package = new ExcelPackage(files.InputStream);

                            #region save file to dir
                            //string path = Server.MapPath("~/Content/JobDocument/");
                            string path = Server.MapPath("~/ExpeditingExcel/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }


                            filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);



                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);


                            }

                            string extension = Path.GetExtension(strmyRandomNo + FileUpload1.FileName);
                            FileUpload1.SaveAs(filePath);


                            filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                            #endregion

                            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                            Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath);

                            String[] excelSheets = new String[excelBook.Worksheets.Count];
                            int i = 0;
                            foreach (Microsoft.Office.Interop.Excel.Worksheet wSheet in excelBook.Worksheets)
                            {
                                excelSheets[i] = wSheet.Name;
                                int RowsCount = wSheet.UsedRange.Rows.Count;

                                if (excelSheets[i] == "Item Description")
                                {
                                    for (int j = 2; j <= RowsCount; j++)
                                    {
                                        //Convert.ToString(workSheet.Cells[rowIterator, 1].Value);
                                        Exp.PO_Item_Number = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
                                        if (Exp.PO_Item_Number != null)
                                        {
                                            Exp.ItemCode = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 2]).Value);
                                            Exp.ItemDescription = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 3]).Value);
                                            String Unit = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 4]).Value);
                                            if (Unit == "number") { Exp.Unit = "1"; }
                                            else if (Unit == "meter") { Exp.Unit = "2"; }
                                            else if (Unit == "km") { Exp.Unit = "3"; }
                                            else if (Unit == "meter (number)") { Exp.Unit = "4"; }
                                            else if (Unit == "km (number)") { Exp.Unit = "5"; }
                                            else if (Unit == "each") { Exp.Unit = "6"; }
                                            else if (Unit == "Piece") { Exp.Unit = "7"; }
                                            else if (Unit == "Test sample") { Exp.Unit = "8"; }
                                            else if (Unit == "AU - All Unit") { Exp.Unit = "9"; }
                                            else if (Unit == "lot") { Exp.Unit = "10"; }
                                            else if (Unit == "Set") { Exp.Unit = "11"; }
                                            else if (Unit == "Running Meter") { Exp.Unit = "12"; }
                                            else if (Unit == "kg") { Exp.Unit = "13"; }
                                            else if (Unit == "metric ton (tonne)") { Exp.Unit = "14"; }
                                            else if (Unit == "ton") { Exp.Unit = "15"; }
                                            else if (Unit == "cubic millimetre") { Exp.Unit = "16"; }
                                            else if (Unit == "cubic centimeter") { Exp.Unit = "17"; }
                                            else if (Unit == "cubic meter") { Exp.Unit = "18"; }
                                            else if (Unit == "cubic inch") { Exp.Unit = "19"; }
                                            else if (Unit == "cubic foot") { Exp.Unit = "20"; }
                                            else if (Unit == "mm") { Exp.Unit = "21"; }
                                            else if (Unit == "cm") { Exp.Unit = "22"; }
                                            else if (Unit == "in") { Exp.Unit = "23"; }
                                            else if (Unit == "foot") { Exp.Unit = "24"; }
                                            else if (Unit == "mile") { Exp.Unit = "25"; }
                                            else if (Unit == "yard") { Exp.Unit = "26"; }
                                            else if (Unit == "liter") { Exp.Unit = "27"; }
                                            else if (Unit == "kl") { Exp.Unit = "28"; }
                                            else if (Unit == "cl") { Exp.Unit = "29"; }
                                            else if (Unit == "ml") { Exp.Unit = "30"; }
                                            else if (Unit == "g") { Exp.Unit = "31"; }
                                            else if (Unit == "lb") { Exp.Unit = "32"; }
                                            else if (Unit == "oz") { Exp.Unit = "33"; }
                                            else if (Unit == "Sq. mm") { Exp.Unit = "34"; }
                                            else if (Unit == "Sq. cm") { Exp.Unit = "35"; }
                                            else if (Unit == "Sq. meter") { Exp.Unit = "36"; }
                                            else if (Unit == "Sq. in") { Exp.Unit = "37"; }
                                            else if (Unit == "Sq. foot") { Exp.Unit = "38"; }
                                            else
                                            {
                                                Exp.Unit = "";

                                            }

                                            Exp.Quantity = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 5]).Value);
                                            //Exp.Equipment_TagNo = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 6]).Value);
                                            Microsoft.Office.Interop.Excel.Range deliveryDateCell = (Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 6];
                                            DateTime deliveryDateValue = Convert.ToDateTime(deliveryDateCell.Value);
                                            string deliveryDateString = deliveryDateValue.ToString("dd/MM/yyyy");
                                            Exp.Contractual_DeliveryDateAsPerPO = deliveryDateString;

                                            Microsoft.Office.Interop.Excel.Range EstimatedDateCell = (Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 7];
                                            DateTime EstimatedDate = Convert.ToDateTime(EstimatedDateCell.Value);
                                            string EstimatedDateValue = EstimatedDate.ToString("dd/MM/yyyy");
                                            Exp.EstimatedDeliveryDate = EstimatedDateValue;
                                            //Exp.Accepted_Quantity = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 7]).Value);
                                            //Exp.Status = "1";
                                            //Exp.Type = "IVR";
                                            //Exp.PK_CALL_ID = PK_Call_Id;
                                            Result = ObjDAL.InsertUpdateItemDescription(Exp);
                                        }

                                    }

                                    excelBook.Close();
                                }
                            }
                        }
                    }
                    #endregion
                    else {

                        Result = ObjDAL.UpdateItemDescription(Exp);
                    }

                }
            }
            catch (Exception ex)
            {

            }
            //}
            //else
            //{
            //    Result = ObjDAL.UpdateItemDescription(Exp);
            //}
            Exp.PK_Call_ID = Exp.PK_Call_ID;

            return RedirectToAction("ExpItemDescription", "Expediting", new { PK_Call_Id = Exp.PK_Call_ID });
        }


        public ActionResult DeleteExpItemDescription(int? PK_ItemD_Id, int? PK_Call_Id)
        {
            string Result = string.Empty;
            try
            {
                Result = ObjDAL.DeleteExpItemDescription(Convert.ToInt32(PK_ItemD_Id));
                if (Convert.ToInt16(Result) > 0)
                {


                    ModelState.Clear();
                }
                else
                {

                    TempData["message"] = "Error";
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ModelState.Clear();
            return RedirectToAction("ExpItemDescription", "Expediting", new { PK_Call_Id = PK_Call_Id });


        }


        #endregion

        #region Progress


        public ActionResult ExpProgress(string PK_Call_Id)
        {

            string[] splitedProduct_Name;
            var item = TempData["itemValue"];
            var model = new Progress();
            DataSet dss = new DataSet();
            DataTable dtProgress = new DataTable();
            List<Progress> lstProgress = new List<Progress>();

            var pk_call_id1 = TempData["AllitemValue"];

            #region Bind Item
            var Data1 = ObjDAL.GetItem(PK_Call_Id);
            ViewBag.BindItem = new SelectList(Data1, "Value", "Text");

            #endregion

            #region Bind po Item
            var Data2 = ObjDAL.GetItemPOitem(PK_Call_Id);
            ViewBag.BindItempowise = new SelectList(Data2, "Valuepo", "Text");
            #endregion
            var item1 = Convert.ToString(item);

            DataTable dtAction = new DataTable();
            List<Progress> lstAction = new List<Progress>();

            dtAction = ObjDAL.GetActionDataByPK_Call_Id(PK_Call_Id);

            if (dtAction.Rows.Count > 0)
            {
                foreach (DataRow dr in dtAction.Rows)
                {
                    lstAction.Add(
                        new Progress
                        {

                            Actionstage = Convert.ToString(dr["Name"]),
                            Customer = Convert.ToString(dr["Customer"]),
                            vendor = Convert.ToString(dr["SubVendor"]),
                            TUVI = Convert.ToString(dr["TUVI"]),
                            Total = Convert.ToString(dr["Total"]),
                        }
                        );
                }
            }


            ViewBag.lstAction = lstAction;


            if (PK_Call_Id != null && item != null)
            {
                model.PK_Call_Id = PK_Call_Id;
                ViewBag.check = "productcheck";
                dtProgress = ObjDAL.fetdataitemwise(PK_Call_Id, item1);
                if (dtProgress.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProgress.Rows)
                    {
                        lstProgress.Add(new Progress
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Stages = Convert.ToString(dr["Stages"]),
                            ExpectedStartDate = Convert.ToString(dr["ExpectedStartDate"]),
                            ExpectedEndDate = Convert.ToString(dr["ExpectedEndDate"]),
                            Actual_Start_Date = Convert.ToString(dr["Actual_Start_Date"]),
                            Actual_End_Date = Convert.ToString(dr["Actual_End_Date"]),
                            //Expected_Progressper = Convert.ToString(dr["PercentageWorkDone"]),
                            //Actual_Progressper = Convert.ToString(dr["Actual_Progress"]),
                            //delaydays = Convert.ToString(dr["delaydays"]),
                            item = Convert.ToString(dr["item1"]),

                        });
                    }
                }




                List<string> Selected = lstProgress.SelectMany(p => p.item.Split(',')).ToList();

                ViewBag.EditproductName1 = Selected;


                ViewBag.CostSheet = lstProgress;




            }
            else if (pk_call_id1 != null)
            {
                dtProgress = ObjDAL.fetdataAllItemwise(PK_Call_Id);
                if (dtProgress.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProgress.Rows)
                    {
                        lstProgress.Add(new Progress
                        {
                            Stages = Convert.ToString(dr["stage"]),
                            ExpectedStartDate = Convert.ToString(dr["ExpectedStartDate"]),
                            ExpectedEndDate = Convert.ToString(dr["ExpectedEndDate"]),
                            Actual_Start_Date = Convert.ToString(dr["Actual_Start_Date"]),
                            Actual_End_Date = Convert.ToString(dr["Actual_End_Date"]),
                            Expected_Progressper = Convert.ToString(dr["PercentageWorkDone"]),
                            Actual_Progressper = Convert.ToString(dr["Actual_Progress"]),
                            delaydays = Convert.ToString(dr["delaydays"]),
                            ConcernClosureper = Convert.ToString(dr["ConcernsStageper"]),
                            Concern = Convert.ToString(dr["TotalConcerns"]),
                            ProgressStatus = Convert.ToString(dr["Progress_Status"]),
                            ActionValue = Convert.ToString(dr["actionValue"]),
                            ExpeditingDate = Convert.ToString(dr["MaxDate"])



                        });
                    }
                }


                ViewBag.CostSheetOverAll = lstProgress;
                ViewBag.CostSheetJson = Newtonsoft.Json.JsonConvert.SerializeObject(lstProgress);
                model.PK_Call_Id = pk_call_id1.ToString();
            }


            else
            {
                model.PK_Call_Id = PK_Call_Id;

                dtProgress = ObjDAL.GetProgress();
                if (dtProgress.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProgress.Rows)
                    {
                        lstProgress.Add(new Progress
                        {
                            Id = Convert.ToInt32(dr["Id"]),

                            Stages = Convert.ToString(dr["Stages"]),

                        });
                    }
                }
                ViewBag.CostSheet = lstProgress;


                //action stage Value



                return View(model);
            }



            return View(model);
        }



        [HttpPost]
        public ActionResult ExpProgress(Progress IVR, FormCollection fc, string itemValue)
        {
            string ProList = string.Join(",", fc["BrScope"]);
            IVR.item = ProList;



            string Result = string.Empty;


            foreach (var item in IVR.Activity)
            {
                if (item.PK_Process_Id != null)
                {

                    objM.PK_Process_Id = item.PK_Process_Id;
                    objM.Stages = item.Stages;
                    objM.Id = item.Id;
                    objM.ExpectedStartDate = item.ExpectedStartDate;
                    objM.ExpectedEndDate = item.ExpectedEndDate;
                    objM.item = IVR.item;
                    objM.Actual_Start_Date = item.Actual_Start_Date;
                    objM.PK_Call_Id = IVR.PK_Call_Id;

                    Result = ObjDAL.InsertUpdateProcess(objM);

                }


                else
                {
                    objM.PK_Process_Id = item.PK_Process_Id;
                    objM.Stages = item.Stages;
                    objM.Id = item.Id;
                    objM.ExpectedStartDate = item.ExpectedStartDate;
                    objM.ExpectedEndDate = item.ExpectedEndDate;
                    //objM.item = item1.item;
                    objM.item = IVR.item;
                    objM.PK_Call_Id = IVR.PK_Call_Id;
                    objM.Actual_Start_Date = item.Actual_Start_Date;
                    Result = ObjDAL.InsertUpdateProcess(objM);
                }


            }



            string sid = IVR.PK_Call_Id;
            return RedirectToAction("ExpProgress", new { PK_Call_Id = IVR.PK_Call_Id });
        }

        public ActionResult ItemValue(string pk_call_id, string item)
        {
            TempData["itemValue"] = item;
            return RedirectToAction("ExpProgress", new { pk_call_id });
        }

        public ActionResult AllDataValue(string pk_call_id)
        {
            TempData["AllitemValue"] = pk_call_id;
            return RedirectToAction("ExpProgress", new { pk_call_id });
        }

        #endregion

        #region Engineering
        public ActionResult ExpEngineering(int? PK_Engi_Id, string PK_Call_Id)
        {
            var model = new Engineering();
            DataSet dss = new DataSet();
            DataTable dsGetStamp = new DataTable();
            List<Engineering> lstCompanyDashBoard = new List<Engineering>();

            model.PK_Call_Id = PK_Call_Id;

            #region Bind ddlstatus 
            var Data = ObjDAL.GetDDLStatus(PK_Call_Id);
            ViewBag.SubCatlist = new SelectList(Data, "Value", "Text");
            #endregion

            #region Bind ddlDocument
            var Data2 = ObjDAL.GetDDLDocument(PK_Call_Id);
            ViewBag.SubDocument = new SelectList(Data2, "Value", "Text");
            #endregion

            #region Bind Item
            var Data1 = ObjDAL.GetItem(model.PK_Call_Id);
            ViewBag.BindItem = new SelectList(Data1, "Value", "Text");
            #endregion




            if (PK_Call_Id != null)
            {
                dsGetStamp = ObjDAL.GetEngineeringDataByPK_Call_Id(model.PK_Call_Id);

                if (dsGetStamp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGetStamp.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new Engineering
                            {
                                Item = Convert.ToString(dr["Item"]),
                                ReferenceDocument = Convert.ToString(dr["ReferenceDocument"]),
                                Number = Convert.ToString(dr["Number"]),
                                Status = Convert.ToString(dr["Status"]),
                                StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]),
                                PK_Engi_Id = Convert.ToInt32(dr["PK_Engi_Id"]),
                                Actual_Progressper = Convert.ToString(dr["EngineeringDateper"]),
                                EstimatedEndDate = Convert.ToString(dr["EstimatedEndDate"]),
                                QuantityNumber = Convert.ToString(dr["QuantityNumber"]),


                            }
                            );
                    }
                }

                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    model.PK_IAFScopeId = Convert.ToInt32(dss.Tables[0].Rows[0]["PK_IAFScopeId"]);
                //    model.IAFScopeName = dss.Tables[0].Rows[0]["IAFScopeName"].ToString();
                //    model.IAFScopeNumber = dss.Tables[0].Rows[0]["IAFScopeNumber"].ToString();



                //}
                ViewBag.lstDOrderType = lstCompanyDashBoard;
                return View(model);

            }
            else
            {
                return View(model);
            }


        }

        [HttpPost]
        public ActionResult ExpEngineering(Engineering IVR, List<Engineering> DArray)
        {

            IVR.PK_Call_Id = IVR.PK_Call_Id;
            string Result = string.Empty;
            try
            {

                //if (IVR.PK_Call_Id != "")
                //{
                //        Result = ObjDAL.DeleteEngineeringIfPresent(IVR.PK_Call_Id);
                //}

                foreach (var item in DArray)
                {
                    IVR.PK_Engi_Id = item.PK_Engi_Id;

                    IVR.Item = item.Item;
                    IVR.ReferenceDocument = item.ReferenceDocument;
                    IVR.Number = item.Number;
                    IVR.Status = item.Status;
                    IVR.StatusUpdatedOn = item.StatusUpdatedOn;
                    IVR.QuantityNumber = item.QuantityNumber;
                    IVR.EstimatedEndDate = item.EstimatedEndDate;
                    Result = ObjDAL.InsertEngineering(IVR);
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            //return RedirectToAction("ExpEngineering", IVR.PK_Call_Id);
            //return RedirectToAction("ExpEngineering", new { PK_Call_Id = IVR.PK_Call_Id });
            return Json(new { result = "Redirect", url = Url.Action("ExpEngineering", "Expediting", new { @PK_Call_Id = IVR.PK_Call_Id }) });
        }


        //Bind Dropdown
        public JsonResult GetStatusData(string PK_Call_Id)
        {

            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.ddlGetEngineeringStatus(PK_Call_Id);
            //if (DTGetProductLst.Rows.Count > 0)
            //{
            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())


                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");
            //}


            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }


        //Bind Dropdown
        public JsonResult GetDocumentData(string PK_Call_Id)
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.ddlGetDocumentStatus(PK_Call_Id);
            //if (DTGetProductLst.Rows.Count > 0)
            //{
            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())


                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");
            //}


            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }



        #region Comment
        //public JsonResult GetStatusData1()
        //{
        //    // Replace this with your logic to get data from the database
        //    List<SelectListItem> statusList = GetStatusFromDatabase();

        //    return Json(statusList, JsonRequestBehavior.AllowGet);
        //}

        //private List<SelectListItem> GetStatusFromDatabase()
        //{
        //    List<SelectListItem> statusList = new List<SelectListItem>
        //{
        //    new SelectListItem { Value = "1", Text = "Status 1" },
        //    new SelectListItem { Value = "2", Text = "Status 2" },
        //    new SelectListItem { Value = "3", Text = "Status 3" },
        //};

        //    return statusList;
        //}s
        #endregion









        #endregion


        #region Material

        public ActionResult ExpMaterial(int? PK_Material_Id, string PK_Call_Id)
        {

            //added by shrutika salve 27122023

            List<NameCode> lstProjectType = new List<NameCode>();
            DataSet dsUnit = new DataSet();
            dsUnit = objDalVisitReport.Measurement();

            if (dsUnit.Tables[0].Rows.Count > 0)
            {
                lstProjectType = (from n in dsUnit.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(dsUnit.Tables[0].Columns["Name"].ToString()),
                                      Code = n.Field<Int32>(dsUnit.Tables[0].Columns["Id"].ToString())

                                  }).ToList();
            }
            ViewBag.UOM = lstProjectType;

            var model = new Material();
            DataSet dss = new DataSet();
            DataTable dsGetStamp = new DataTable();
            List<Material> lstCompanyDashBoard = new List<Material>();

            model.PK_Call_Id = PK_Call_Id;

            #region Bind ddlstatus 
            //var Data = ObjDAL.GetDDLStatus();
            //ViewBag.SubCatlist = new SelectList(Data, "Value", "Text");
            var Data = ObjDAL.GetMaterialStatus(PK_Call_Id);
            ViewBag.MaterialStatus = new SelectList(Data, "Value", "Text");
            #endregion

            #region Bind Item
            var Data1 = ObjDAL.GetItem(model.PK_Call_Id);
            ViewBag.BindItem = new SelectList(Data1, "Value", "Text");
            #endregion




            if (PK_Call_Id != null)
            {
                dsGetStamp = ObjDAL.GetMaterialDataByPK_Call_Id(model.PK_Call_Id);

                if (dsGetStamp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGetStamp.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new Material
                            {
                                PK_Material_Id = Convert.ToInt32(dr["PK_Material_Id"]),
                                Item = Convert.ToString(dr["Item"]),
                                Material_Category = Convert.ToString(dr["Material_Category"]),
                                Description = Convert.ToString(dr["Description"]),
                                UOM = Convert.ToString(dr["UOM"]),
                                Quantity = Convert.ToString(dr["Quantity"]),
                                Source = Convert.ToString(dr["Source"]),
                                Status = Convert.ToString(dr["Status"]),
                                StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]),
                                OrderPlacedOn = Convert.ToString(dr["OrderPlacedOn"]),
                                PONumber = Convert.ToString(dr["PONumber"]),
                                PODate = Convert.ToString(dr["PODate"]),
                                Contractual_DeliveryDateAsPerPO = Convert.ToString(dr["Contractual_DeliveryDate"]),
                                EstimatedDeliveryDate = Convert.ToString(dr["EstimatedDeliveryDate"]),
                                MaterialActual_Progressper = Convert.ToString(dr["MaterialStageDate"]),
                                delaydays = Convert.ToString(dr["delaydays"]),
                                EstimatedEndDate = Convert.ToString(dr["EstimatedEndDate"]),
                            }
                            );
                    }
                }


                ViewBag.lstMaterial = lstCompanyDashBoard;
                return View(model);

            }
            else
            {
                return View(model);
            }


        }

        [HttpPost]
        public ActionResult ExpMaterial(Material IVR, List<Material> DArray)
        {
            IVR.PK_Call_Id = IVR.PK_Call_Id;
            string Result = string.Empty;
            try
            {

                //if (IVR.PK_Call_Id != "")
                //{
                //    Result = ObjDAL.DeleteMaterialIfPresent(IVR.PK_Call_Id);
                //}


                foreach (var item in DArray)
                {

                    IVR.PK_Material_Id = item.PK_Material_Id;

                    IVR.Item = item.Item;
                    IVR.Material_Category = item.Material_Category;
                    IVR.Description = item.Description;
                    IVR.UOM = item.UOM;
                    IVR.Quantity = item.Quantity;
                    IVR.Source = item.Source;
                    IVR.Status = item.Status;
                    IVR.StatusUpdatedOn = item.StatusUpdatedOn;
                    IVR.OrderPlacedOn = item.OrderPlacedOn;
                    IVR.PONumber = item.PONumber;
                    IVR.PODate = item.PODate;
                    IVR.Contractual_DeliveryDateAsPerPO = item.Contractual_DeliveryDateAsPerPO;
                    IVR.EstimatedEndDate = item.EstimatedEndDate;
                    IVR.EstimatedDeliveryDate = item.EstimatedDeliveryDate;

                    Result = ObjDAL.InsertMaterial(IVR);

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            // return RedirectToAction("ExpMaterial", new { PK_Call_Id = IVR.PK_Call_Id });
            return Json(new { result = "Redirect", url = Url.Action("ExpMaterial", "Expediting", new { @PK_Call_Id = IVR.PK_Call_Id }) });

        }

        public JsonResult GetMaterialStatus(string pk_call_id)
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetMaterialStatusDynamic(pk_call_id);
            //if (DTGetProductLst.Rows.Count > 0)
            //{
            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");
            //}


            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }

        //added by shrutika 27122023
        public JsonResult GetMaterialUOM()
        {
            List<NameCode> lstProjectType = new List<NameCode>();
            DataSet dsUnit = new DataSet();
            dsUnit = objDalVisitReport.Measurement();

            if (dsUnit.Tables[0].Rows.Count > 0)
            {
                lstProjectType = (from n in dsUnit.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(dsUnit.Tables[0].Columns["Name"].ToString()),
                                      Code = n.Field<Int32>(dsUnit.Tables[0].Columns["Id"].ToString())

                                  }).ToList();
            }
            ViewBag.UOM = lstProjectType;

            //}


            return Json(lstProjectType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemByPK_Call_Id(string PK_Call_Id)
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetItemDynamic(PK_Call_Id);
            //if (DTGetProductLst.Rows.Count > 0)
            //{
            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");
            //}


            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Manufacturing 

        public ActionResult ExpManufacturing(int? PK_Material_Id, string PK_Call_Id)
        {

            //added by shrutika salve 27122023

            List<NameCode> lstProjectType = new List<NameCode>();
            DataSet dsUnit = new DataSet();
            dsUnit = objDalVisitReport.Measurement();

            if (dsUnit.Tables[0].Rows.Count > 0)
            {
                lstProjectType = (from n in dsUnit.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(dsUnit.Tables[0].Columns["Name"].ToString()),
                                      Code = n.Field<Int32>(dsUnit.Tables[0].Columns["Id"].ToString())

                                  }).ToList();
            }
            ViewBag.UOM = lstProjectType;

            var model = new Manufacturing();
            DataSet dss = new DataSet();
            DataTable dsGetStamp = new DataTable();
            List<Manufacturing> lstCompanyDashBoard = new List<Manufacturing>();

            model.PK_Call_Id = PK_Call_Id;

            #region Bind ddlstatus 
            //var Data = ObjDAL.GetDDLStatus();
            //ViewBag.SubCatlist = new SelectList(Data, "Value", "Text");
            var Data = ObjDAL.GetManufacturingStatus(PK_Call_Id);
            ViewBag.MaterialStatus = new SelectList(Data, "Value", "Text");
            #endregion

            #region Bind Item
            var Data1 = ObjDAL.GetItem(model.PK_Call_Id);
            ViewBag.BindItem = new SelectList(Data1, "Value", "Text");
            #endregion




            if (PK_Call_Id != null)
            {
                dsGetStamp = ObjDAL.GetManufacturingDataByPK_Call_Id(model.PK_Call_Id);

                if (dsGetStamp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGetStamp.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new Manufacturing
                            {

                                PK_Manufacturing_Id = Convert.ToInt32(dr["PK_Manufacturing_Id"]),
                                Item = Convert.ToString(dr["Item"]),
                                PartName = Convert.ToString(dr["PartName"]),
                                UOM = Convert.ToString(dr["UOM"]),
                                Quantity = Convert.ToString(dr["Quantity"]),
                                Status = Convert.ToString(dr["Status"]),
                                StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]),
                                ManufacturingProgressper = Convert.ToString(dr["ExpManufacturingDate"]),
                                delaydays = Convert.ToString(dr["delaydays"]),
                                EstimatedEndDate = Convert.ToString(dr["EstimatedEndDate"]),
                            }
                            );
                    }
                }


                ViewBag.lstManufacturing = lstCompanyDashBoard;
                return View(model);

            }
            else
            {
                return View(model);
            }


        }

        public JsonResult GetManufacturingStatusJson(string PK_Call_Id)
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetManufacturingStatusDynamic(PK_Call_Id);
            //if (DTGetProductLst.Rows.Count > 0)
            //{
            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");
            //}


            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExpManufacturing(Manufacturing IVR, List<Manufacturing> DArray)
        {
            IVR.PK_Call_Id = IVR.PK_Call_Id;
            string Result = string.Empty;
            try
            {

                //if (IVR.PK_Call_Id != "")
                //{
                //    Result = ObjDAL.DeleteManufacturingIfPresent(IVR.PK_Call_Id);
                //}

                foreach (var item in DArray)
                {

                    IVR.PK_Manufacturing_Id = item.PK_Manufacturing_Id;
                    //if (IVR.PK_Call_Id != "")
                    //{
                    //    //update

                    //    //PK_Manufacturing_Id
                    //    //PK_Call_Id
                    //    //Item
                    //    //PartName
                    //    //UOM
                    //    //Quantity
                    //    //Status
                    //    //StatusUpdatedOn

                    //    IVR.Item = item.Item;
                    //    IVR.PartName = item.PartName;
                    //    IVR.UOM = item.UOM;
                    //    IVR.Quantity = item.Quantity;
                    //    IVR.Status = item.Status;
                    //    IVR.StatusUpdatedOn = item.StatusUpdatedOn;


                    //    Result = ObjDAL.UpdateManufacturing(IVR);
                    //}
                    //else
                    //{

                    IVR.Item = item.Item;
                    IVR.PartName = item.PartName;
                    IVR.UOM = item.UOM;
                    IVR.Quantity = item.Quantity;
                    IVR.Status = item.Status;
                    IVR.StatusUpdatedOn = item.StatusUpdatedOn;
                    IVR.EstimatedEndDate = item.EstimatedEndDate;
                    Result = ObjDAL.InsertManufacturing(IVR);
                    // }



                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            //return RedirectToAction("ExpManufacturing", new { PK_Call_Id = IVR.PK_Call_Id });
            return Json(new { result = "Redirect", url = Url.Action("ExpManufacturing", "Expediting", new { @PK_Call_Id = IVR.PK_Call_Id }) });


        }



        #endregion

        #region Final Stages

        public ActionResult ExpFinalStage(int? PK_FinalStage_Id, string PK_Call_Id)
        {
            var model = new FinalStages();
            DataSet dss = new DataSet();
            DataTable dsGetStamp = new DataTable();
            List<FinalStages> lstCompanyDashBoard = new List<FinalStages>();


            model.PK_Call_Id = PK_Call_Id;

            #region Bind ddlStage 
            var Data = ObjDAL.GetddlFinalStageMaster(PK_Call_Id);
            ViewBag.DDLFinalStageMaster = new SelectList(Data, "Value", "Text");
            #endregion

            #region Bind Item
            var Data1 = ObjDAL.GetItem(model.PK_Call_Id);
            ViewBag.BindItem = new SelectList(Data1, "Value", "Text");
            #endregion

            #region Bind ddlFinalStageStatus 
            var Data2 = ObjDAL.GetddlFinalStageStatusMaster(PK_Call_Id);
            ViewBag.DDLFinalStatusMaster = new SelectList(Data2, "Value", "Text");
            #endregion


            if (PK_Call_Id != null)
            {
                dsGetStamp = ObjDAL.GetFinalStageByPK_Call_Id(model.PK_Call_Id);

                if (dsGetStamp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGetStamp.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new FinalStages
                            {

                                PK_FinalStage_Id = Convert.ToInt32(dr["PK_FinalStage_Id"]),
                                Item = Convert.ToString(dr["Item"]),
                                Stage = Convert.ToString(dr["Stage"]),
                                Status = Convert.ToString(dr["Status"]),
                                StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]),
                                FinalstageProgressper = Convert.ToString(dr["ExpFinalStageDate"]),
                                delaydays = Convert.ToString(dr["delaydays"]),
                                EstimatedEndDate = Convert.ToString(dr["EstimatedEndDate"]),
                            }
                            );
                    }
                }


                ViewBag.lstManufacturing = lstCompanyDashBoard;
                return View(model);

            }
            else
            {
                return View(model);
            }


        }

        public JsonResult GetFinalStageMasterJson(string PK_Call_Id)
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetFinalStageMaster(PK_Call_Id);
            //if (DTGetProductLst.Rows.Count > 0)
            //{
            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");
            //}


            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetStageMasterJson(string PK_Call_Id)
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetStageMaster(PK_Call_Id);
            //if (DTGetProductLst.Rows.Count > 0)
            //{
            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");
            //}


            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetFinalStatusMasterJson(string pk_call_id)
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetFinalStatusMaster(pk_call_id);

            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");



            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExpFinalStage(FinalStages IVR, List<FinalStages> DArray)
        {
            IVR.PK_Call_Id = IVR.PK_Call_Id;
            string Result = string.Empty;
            try
            {

                //if (IVR.PK_Call_Id !="")
                //{
                //    Result = ObjDAL.DeleteFinalStage(IVR.PK_Call_Id);
                //}
                foreach (var item in DArray)
                {
                    IVR.PK_FinalStage_Id = item.PK_FinalStage_Id;
                    IVR.PK_Call_Id = IVR.PK_Call_Id;
                    IVR.Item = item.Item;
                    IVR.Stage = item.Stage;
                    IVR.Status = item.Status;
                    IVR.StatusUpdatedOn = item.StatusUpdatedOn;
                    IVR.EstimatedEndDate = item.EstimatedEndDate;

                    Result = ObjDAL.InsertFinalStage(IVR);

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            //return RedirectToAction("ExpFinalStage", new { PK_Call_Id = IVR.PK_Call_Id });
            return Json(new { result = "Redirect", url = Url.Action("ExpFinalStage", "Expediting", new { @PK_Call_Id = IVR.PK_Call_Id }) });

        }

        #endregion


        #region Concerns
        public ActionResult ExpConcerns(int? PK_Concern_Id, string PK_Call_Id)
        {
            var model = new Concerns();
            DataSet dss = new DataSet();
            DataTable dsGetStamp = new DataTable();
            List<Concerns> lstCompanyDashBoard = new List<Concerns>();
            List<Concerns> lstCompanyDashBoard1 = new List<Concerns>();


            model.PK_Call_ID = PK_Call_Id;

            #region Bind ddlStage 
            var Data = ObjDAL.GetddlConcernsStage();
            ViewBag.DDLFinalStageMaster = new SelectList(Data, "Value", "Text");
            #endregion

            #region Bind Item
            var Data1 = ObjDAL.GetItem(model.PK_Call_ID);
            ViewBag.BindItem = new SelectList(Data1, "Value", "Text");
            #endregion

            #region Bind ddlConcernStatus 
            var Data2 = ObjDAL.GetddlConcernStatus();
            ViewBag.DDLConcernStatus = new SelectList(Data2, "Value", "Text");
            #endregion

            #region Concern Category Master
            var Data3 = ObjDAL.GetddlConcernCategory();
            ViewBag.DDLConcernCategory = new SelectList(Data3, "Value", "Text");
            #endregion


            if (PK_Call_Id != null)
            {



                List<Concerns> previousComments = new List<Concerns>();
                DataSet ds = ObjDAL.privousCommentsPK_Call_Id(model.PK_Call_ID);

                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    Concerns comment = new Concerns
                    {
                        PreviousComments = Convert.ToString(row["PreviousComments"]),
                    };

                    previousComments.Add(comment);
                }


                ViewBag.previousComments = previousComments;



                dsGetStamp = ObjDAL.GetConcernDataByPK_Call_Id(model.PK_Call_ID);

                if (dsGetStamp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGetStamp.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new Concerns
                            {



                                PK_Concern_Id = Convert.ToInt32(dr["pk_concerns_id"]),
                                PK_Call_ID = Convert.ToString(dr["PK_Call_ID"]),
                                Date = Convert.ToString(dr["Date"]),
                                Category = Convert.ToString(dr["Category"]),
                                Item = Convert.ToString(dr["Item"]),
                                Stage = Convert.ToString(dr["Stage"]),
                                Details = Convert.ToString(dr["Details"]),
                                MitigationBy = Convert.ToString(dr["MitigationBy"]),
                                ResponsiblePerson = Convert.ToString(dr["ResponsiblePerson"]),
                                ExpectedClosureDate = Convert.ToString(dr["ExpectedClosureDate"]),
                                Status = Convert.ToString(dr["Status"]),
                                StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]),
                                Comment = Convert.ToString(dr["Comment"]),

                                ConcernsstageProgressper = Convert.ToString(dr["ConcernsStageDate"]),
                                delaydays = Convert.ToString(dr["delaydays"]),
                                ActionReq = Convert.ToString(dr["ActionReq"]),

                                //PreviousComments = Convert.ToString(dr["PreviousComments"]),

                            }
                            );
                    }
                }


                ViewBag.lstManufacturing = lstCompanyDashBoard;
                return View(model);

            }
            else
            {
                return View(model);
            }


        }

        public JsonResult GetConcernCategoryJson()
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetConcernCategoryMaster();

            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");



            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemJson(string PK_Call_Id)
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetItemMaster(PK_Call_Id);

            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");



            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConcernStageJson()
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetConcernStageJson();

            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");



            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConcernStatusJson(string PK_Call_Id)
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetConcernStatusJsonD(PK_Call_Id);

            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");



            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExpConcerns(Concerns IVR, List<Concerns> DArray)
        {
            IVR.PK_Call_ID = IVR.PK_Call_ID;
            string Result = string.Empty;
            try
            {

                //if (IVR.PK_Call_ID != "")
                //{
                //    Result = ObjDAL.DeleteConcern(IVR.PK_Call_ID);
                //}
                foreach (var item in DArray)
                {
                    IVR.PK_Concern_Id = item.PK_Concern_Id;
                    IVR.PK_Call_ID = IVR.PK_Call_ID;
                    IVR.Date = item.Date;
                    IVR.Category = item.Category;
                    IVR.Item = item.Item;
                    IVR.Stage = item.Stage;
                    IVR.Details = item.Details;
                    IVR.pk_userid = item.pk_userid;
                    IVR.MitigationBy = item.MitigationBy;
                    IVR.MitigationByUserid = item.MitigationByUserid;
                    IVR.ResponsiblePerson = item.ResponsiblePerson;
                    IVR.ExpectedClosureDate = item.ExpectedClosureDate;
                    IVR.Status = item.Status;
                    IVR.StatusUpdatedOn = item.StatusUpdatedOn;
                    IVR.Comment = item.Comment;
                    IVR.ActionReq = item.ActionReq;



                    Result = ObjDAL.InsertConcern(IVR);

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            //return RedirectToAction("ExpFinalStage", new { PK_Call_Id = IVR.PK_Call_Id });
            return Json(new { result = "Redirect", url = Url.Action("ExpConcerns", "Expediting", new { @PK_Call_Id = IVR.PK_Call_ID }) });

        }


        public JsonResult GetLeadByName(string Prefix)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (Prefix != null && Prefix != "")
            {
                DTResult = ObjDAL.GetUserName(Prefix);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {
                               CompanyName = Convert.ToString(dr["CompanyName"]),
                               CompanyNames = Convert.ToString(dr["CompanyNames"]),
                               PkUserID = Convert.ToString(dr["pk_UserID"]),

                           }
                         );
                    }
                    Session["CompanyNames"] = Convert.ToString(DTResult.Rows[0]["CompanyNames"]);
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetLeadByMitigationBy(string Prefix)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (Prefix != null && Prefix != "")
            {
                DTResult = ObjDAL.GetUserName(Prefix);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {
                               CompanyName = Convert.ToString(dr["CompanyName"]),
                               CompanyNames = Convert.ToString(dr["CompanyNames"]),
                               PkUserID = Convert.ToString(dr["pk_UserID"]),

                           }
                         );
                    }
                    Session["CompanyNames"] = Convert.ToString(DTResult.Rows[0]["CompanyNames"]);
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }



        #endregion




        #region Photo
        //added photo attachement
        public ActionResult PhotoAttachement(string pk_call_id)
        {
            var model = new Photo();
            model.PK_CALL_ID = pk_call_id;
            {
                model.PK_CALL_ID = Convert.ToString(pk_call_id);
            }

            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<Photo> lstCompanyDashBoard = new List<Photo>();



            if (model.PK_IP_Id != 0)
            {
                ItemDescriptionData = ObjDAL.GetReportImageById(model.PK_IP_Id);
                if (ItemDescriptionData.Tables[0].Rows.Count > 0)
                {
                    model.Heading = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Heading"]);
                    model.Image = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["Image"]);
                    model.PK_IP_Id = Convert.ToInt32(ItemDescriptionData.Tables[0].Rows[0]["PK_IP_Id"]);
                    model.PK_CALL_ID = Convert.ToString(ItemDescriptionData.Tables[0].Rows[0]["PK_CALL_ID"]);


                    CostSheetDashBoard = ObjDAL.GetReportImageByCall_Id(model.PK_CALL_ID);
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new Photo
                                {
                                    Image = Convert.ToString(dr["Image"]),
                                    Heading = Convert.ToString(dr["Heading"]),
                                    PK_IP_Id = Convert.ToInt32(dr["PK_IP_Id"]),
                                    PK_CALL_ID = Convert.ToString(dr["PK_CALL_ID"])

                                }
                                );
                        }
                    }

                }

            }
            else
            {
                try
                {
                    CostSheetDashBoard = ObjDAL.GetReportImageByCall_Id(model.PK_CALL_ID);
                    if (CostSheetDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CostSheetDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new Photo
                                {
                                    Image = Convert.ToString(dr["Image"]),
                                    Heading = Convert.ToString(dr["Heading"]),
                                    PK_IP_Id = Convert.ToInt32(dr["PK_IP_Id"]),
                                    PK_CALL_ID = Convert.ToString(dr["PK_CALL_ID"])
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                    return RedirectToAction("ErrorPage", "PhotoAttachement", new { @Error = Error });
                }
            }

            ViewData["PhotoSheet"] = lstCompanyDashBoard;
            ViewBag.PhotoSheet = lstCompanyDashBoard;

            return View(model);
        }


        [HttpPost]
        public ActionResult PhotoAttachement(Photo model, FormCollection fc, HttpPostedFileBase File, List<HttpPostedFileBase> img_Banner)
        {
            string Result = string.Empty;
            try
            {
                if (model.PK_IP_Id == 0)
                {
                    HttpPostedFileBase Imagesection;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        foreach (HttpPostedFileBase single in img_Banner)


                        {
                            Imagesection = single;//Request.Files["img_Banner"];
                            if (Imagesection != null && Imagesection.FileName != "")
                            {

                                //IVR.Image = CommonControl.FileUpload("Content/Uploads/Images/", Imagesection);
                                //model.Image = CommonControl.FileUploadCompress("Content/ExpesitingPhoto/Images/", Imagesection, model.PK_IP_Id, model.PK_CALL_ID.ToString());
                                model.Image = CommonControl.FileUploadResize("Content/ExpesitingPhoto/Images/", Imagesection, model.PK_IP_Id, model.PK_CALL_ID.ToString());
                            }
                            else
                            {
                                if (Imagesection.FileName != "")
                                {
                                    model.Image = "NoImage.gif";
                                }
                            }

                            model.Type = "Expediting";
                            model.Status = "1";
                            Result = ObjDAL.InsertUpdateReportImage(model);
                            if (Result != "" && Result != null)
                            {
                                ModelState.Clear();
                                TempData["InsertCompany"] = Result;
                            }
                        }


                    }

                    return RedirectToAction("PhotoAttachement", model);
                }
                else
                {
                    HttpPostedFileBase Imagesection;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        Imagesection = Request.Files["img_Banner"];
                        if (Imagesection != null && Imagesection.FileName != "")
                        {

                            model.Image = CommonControl.FileUploadCompress("Content/ExpesitingPhoto/Images/", Imagesection, model.PK_IP_Id, model.PK_CALL_ID.ToString());
                        }
                        else
                        {
                            if (Imagesection.FileName != "")
                            {
                                model.Image = "NoImage.gif";
                            }
                        }
                    }
                    Result = ObjDAL.InsertUpdateReportImage(model);
                    if (Result != null && Result != "")
                    {
                        ModelState.Clear();
                        TempData["UpdateCompany"] = Result;
                    }
                    return RedirectToAction("PhotoAttachement", model);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            #region
            DataTable CostSheetDashBoard = new DataTable();
            List<Photo> lstCompanyDashBoard = new List<Photo>();
            CostSheetDashBoard = ObjDAL.GetReportImageByCall_Id(model.PK_CALL_ID);
            try
            {
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new Photo
                            {
                                Image = Convert.ToString(dr["Image"]),
                                Heading = Convert.ToString(dr["Heading"]),
                                PK_IP_Id = Convert.ToInt32(dr["PK_IP_Id"]),
                                PK_CALL_ID = Convert.ToString(dr["PK_CALL_ID"])
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                return RedirectToAction("ErrorPage", "PhotoAttachement", new { @Error = Error });
            }
            ViewData["CostSheet"] = lstCompanyDashBoard;
            model.PK_IP_Id = 0;
            #endregion
            model.PK_IP_Id = 0;
            model.Image = null;
            // IVR.Heading = null;
            return View(model);
        }



        public ActionResult DeleteImage(int? aid, int? kid)
        {
            var model = new Photo();
            model.PK_CALL_ID = Convert.ToString(kid);
            try
            {
                if (ObjDAL.DeleteImage(Convert.ToInt32(aid)))
                {
                    TempData["Deleted"] = "Activity Details Deleted Successfully ..!";
                }
                return RedirectToAction("PhotoAttachement", new { @sid = Convert.ToInt32(model.PK_CALL_ID) });
            }
            catch (Exception)
            {
                return View();
            }
        }



        [HttpPost]
        public ActionResult SaveHeading(Photo IVR, FormCollection fc)
        {
            string Result = string.Empty;

            foreach (var item in IVR.Activity)
            {
                if (item.chkbox == true)
                {
                    int aid = item.PK_IP_Id;
                    bool s = ObjDAL.DeleteImage(Convert.ToInt32(aid));
                }
                else
                {
                    IVR.PK_CALL_ID = item.PK_CALL_ID;
                    IVR.Heading = item.Heading;
                    IVR.PK_IP_Id = item.PK_IP_Id;
                    Result = ObjDAL.UpdateHeading(IVR);
                }

            }

            string PK_Call_ID = IVR.PK_CALL_ID;
            return RedirectToAction("PhotoAttachement", new { PK_Call_ID = IVR.PK_CALL_ID });
        }

        #endregion

        #region Observation

        [HttpGet]
        public ActionResult Observation(string PK_Call_ID)
        {
            var model = new Observation();
            model.PK_Call_ID = PK_Call_ID;

            DataTable dsGetStamp = new DataTable();
            List<Observation> lstCompanyDashBoard = new List<Observation>();

            DataTable dsdata = new DataTable();
            List<Observation> lst = new List<Observation>();
            #region Bind Item
            var Data1 = ObjDAL.GetSatges(PK_Call_ID);
            ViewBag.BindSatges = new SelectList(Data1, "Value", "Text");
            #endregion

            if (PK_Call_ID != null)
            {
                dsGetStamp = ObjDAL.GetObservationDataByPK_Call_Id(model.PK_Call_ID);

                if (dsGetStamp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGetStamp.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new Observation
                            {

                                PK_Final = Convert.ToInt32(dr["pk_observationid"]),
                                //CreatedDate= Convert.ToString(dr["CreatedDate"]),
                                Stage = Convert.ToString(dr["Stages"]),
                                Findings = Convert.ToString(dr["Findings"]),
                            }
                            );
                    }
                }


                ViewBag.lstObservation = lstCompanyDashBoard;

                dsdata = ObjDAL.GetObservation(model.PK_Call_ID);

                if (dsdata.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsdata.Rows)
                    {
                        lst.Add(
                            new Observation
                            {

                                PK_Final = Convert.ToInt32(dr["pk_observationid"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Stage = Convert.ToString(dr["Stages"]),
                                Findings = Convert.ToString(dr["Findings"]),
                            }
                            );
                    }
                }
                ViewBag.lst = lst;
                return View(model);

            }
            else
            {
                return View(model);
            }



        }

        [HttpPost]
        public ActionResult Observation(Observation O, List<Observation> DArray)
        {

            O.PK_Call_ID = O.PK_Call_ID;
            string Result = string.Empty;
            try
            {

                if (O.PK_Call_ID != "")
                {
                    Result = ObjDAL.DeleteObservation(O.PK_Call_ID);
                }
                foreach (var item in DArray)
                {

                    O.PK_Call_ID = O.PK_Call_ID;

                    O.Stage = item.Stage;
                    O.Findings = item.Findings;


                    Result = ObjDAL.InsertObservation(O);

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            //return RedirectToAction("Observation", new { @PK_Call_Id = O.PK_Call_ID });
            //return Json(new {url = Url.Action("Observation", "Expediting", new { @PK_Call_Id = O.PK_Call_ID }) });
            return Json(new { result = "Redirect", url = Url.Action("Observation", "Expediting", new { @PK_Call_Id = O.PK_Call_ID }) });


        }

        //added by shrutika observation
        public JsonResult GetObservationStageJson()
        {
            IEnumerable<SelectListItem> ProductcheckItems;
            DataTable DTGetProductLst = new DataTable();
            DTGetProductLst = ObjDAL.GetObservationMaster();
            //if (DTGetProductLst.Rows.Count > 0)
            //{
            List<NameStatusEngineering> statusList = new List<NameStatusEngineering>();
            {
                statusList = (from n in DTGetProductLst.AsEnumerable()
                              select new NameStatusEngineering()
                              {
                                  Text = n.Field<string>(DTGetProductLst.Columns["Text"].ToString()),
                                  Value = n.Field<int>(DTGetProductLst.Columns["Value"].ToString())

                              }).ToList();
            }

            ProductcheckItems = new SelectList(statusList, "Value", "Text");
            //}


            return Json(ProductcheckItems, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Records
        //attachement Records 
        public ActionResult Records(string Pk_call_id)
        {
            var model = new Records();
            model.PK_Call_ID = Pk_call_id;
            string Result1 = string.Empty;
            List<FileDetails> lstFileDtls1 = new List<FileDetails>();

            DataTable dtAttach = new DataTable();
            List<Records> lstDTPANEye = new List<Records>();




            dtAttach = ObjDAL.GetFileContent(Pk_call_id);

            if (dtAttach.Rows.Count > 0)
            {
                foreach (DataRow dr in dtAttach.Rows)
                {
                    lstDTPANEye.Add(
                       new Records
                       {

                           FileName = Convert.ToString(dr["FileName"]),


                           Name = Convert.ToString(dr["Name"]),

                       }
                     );
                }

                model.ReportDetails = lstDTPANEye;
                ViewData["DocAttachmentsTest"] = lstDTPANEye;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Records(Records R)
        {


            return RedirectToAction("Records", new { PK_Call_Id = R.PK_Call_ID });
        }


        [HttpPost]
        public JsonResult InsertDocumentRecords(string Pk_call_id, string Name)
        {
            string Results = string.Empty;
            int intUserID = 0;



            FileDetails fileDetails = new FileDetails();
            DataTable DTGetDeleteFile = new DataTable();

            List<FileDetails> DOCUploaded = new List<FileDetails>();

            string Result = string.Empty;
            string RetValue = string.Empty;
            bool validFile = false;


            try
            {
                DOCUploaded = Session["DocsEyeTestUploaded"] as List<FileDetails>;

                if (DOCUploaded != null && DOCUploaded.Count > 0)
                {
                    foreach (var fileDetail in DOCUploaded)
                    {

                        fileDetail.FileName = Regex.Replace(fileDetail.FileName, "[^a-zA-Z0-9_.]+", "_");
                    }

                    RetValue = ObjDAL.InsertFileAttachment(DOCUploaded, Pk_call_id, Name);
                    CommonControl objCommonControl = new CommonControl();
                    objCommonControl.SaveExpediting(DOCUploaded, Convert.ToInt32(Pk_call_id));
                    Session["DocsEyeTestUploaded"] = null;

                    //    RetValue = ObjDAL.InsertFileAttachment(DOCUploaded, Pk_call_id,Name );
                    //    CommonControl objCommonControl = new CommonControl();
                    //    objCommonControl.SaveExpediting(DOCUploaded, Convert.ToInt32(Pk_call_id));
                    //Session["DocsEyeTestUploaded"] = null;


                }

                if (String.IsNullOrEmpty(RetValue))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    //return Json("ERROR", JsonRequestBehavior.AllowGet);
                    return Json(new { result = "TYPEERROR", JsonRequestBehavior.AllowGet });
                }


                return Json(new { result = "SUCCESS", JsonRequestBehavior.AllowGet });


            }
            catch (Exception ex)
            {

                return Json("Error", JsonRequestBehavior.AllowGet);
            }


        }

        public FileResult DownloadFormat1(int d)
        {

            string FileName = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = ObjDAL.GetConFileExt(Convert.ToInt32(d));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                ////  bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
                fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
            }

            string a = DateTime.Now.Month.ToString();
            int intC = Convert.ToInt32(a);
            string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);

            string CurrentYear = DateTime.Now.Year.ToString();
            string pathYear = "~/Content/ExpeditingAttachement/" + CurrentYear;
            string pathMonth = "~/Content/ExpeditingAttachement/" + CurrentMonth;
            string FinalPath = "~/Content/ExpeditingAttachement/" + CurrentYear + '/' + CurrentMonth;
            string FinalPath1 = "Content\\ExpeditingAttachement\\" + CurrentYear + '\\' + CurrentMonth + '\\';





            //string path = Server.MapPath("~/Content/ExpeditingAttachement/" + fileName);
            // string path = Server.MapPath(FinalPath1 + fileName);

            string path = Server.MapPath("~/Content/ExpeditingAttachement/" + CurrentYear + "/" + CurrentMonth + "/") + fileName;


            bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }



        [HttpPost]
        public JsonResult DeleteFileFormat(string id, string Pk_call_id)
        {
            string Results = string.Empty;
            FileDetails fileDetails = new FileDetails();
            DataTable DTGetDeleteFile = new DataTable();
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { result = "Error" });

            }
            try
            {

                if (id != null && id != "")
                {
                    Results = ObjDAL.DeleteConUploadedFile(id);

                    return Json(new { result = "OK", JsonRequestBehavior.AllowGet });

                }
            }
            catch (Exception ex)
            {

                return Json(new { result = "ERROR", url = Url.Action("Records", "Expediting", new { @pk_call_id = Pk_call_id }) });
            }
            return Json(new { result = "SUCCESS", url = Url.Action("Records", "Expediting", new { @pk_call_id = Pk_call_id }) });

        }
        #endregion


        public ActionResult ExpDelays(string PK_Call_ID)
        {
            var model = new ExpDelays();
            model.PK_Call_ID = PK_Call_ID;
            DataSet dss = new DataSet();
            DataTable dtProgress = new DataTable();
            List<ExpDelays> lstProgress = new List<ExpDelays>();


            if (PK_Call_ID != null)
            {

                dtProgress = ObjDAL.EXpDalayWise(PK_Call_ID);
                if (dtProgress.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProgress.Rows)
                    {
                        lstProgress.Add(new ExpDelays
                        {
                            item = Convert.ToString(dr["PO_Item_Number"]),
                            PoitemCode = Convert.ToString(dr["ItemCode"]),
                            POQuantity = Convert.ToString(dr["Quantity"]),
                            UOM = Convert.ToString(dr["Unit"]),
                            CDD = Convert.ToString(dr["Contractual_DeliveryDateAsPerPO"]),
                            EDD = Convert.ToString(dr["EstimatedDeliveryDate"]),
                            ExpstartDate = Convert.ToString(dr["exprctedstartDate"]),
                            ExpEndDate = Convert.ToString(dr["ExpectedEndDate"]),
                            ActstartDate = Convert.ToString(dr["Actual_Start_Date"]),
                            ActEndDate = Convert.ToString(dr["Actual_End_Date"]),
                            Concerns = Convert.ToString(dr["TotalConcerns"]),
                            engDelay = Convert.ToString(dr["DelayEng"]),
                            ProDelay = Convert.ToString(dr["DelayProcurement"]),
                            manuDelay = Convert.ToString(dr["DelayManufacturing"]),
                            FinalDelay = Convert.ToString(dr["DelayFinal"]),
                            engExp = Convert.ToString(dr["ExceptedEng"]),
                            ProExp = Convert.ToString(dr["ExceptedProcurement"]),
                            manuExp = Convert.ToString(dr["ExceptedManufacturing"]),
                            FinalExp = Convert.ToString(dr["ExceptedFinal"]),
                            engAct = Convert.ToString(dr["ActEng"]),
                            ProAct = Convert.ToString(dr["ActProcurement"]),
                            manuAct = Convert.ToString(dr["ActManufacturing"]),
                            FinalAct = Convert.ToString(dr["ActFinal"]),
                            actualprogress = Convert.ToString(dr["actualprogress"]),
                            progressStatus = Convert.ToString(dr["Progress_Status"]),

                        });
                    }
                }

                ViewBag.CostSheet = lstProgress;

            }

            else
            {


            }

            return View(model);
        }



        public ActionResult EXp_commercial(string PK_Call_ID)
        {
            var model = new ExpCommercial();
            model.PK_Call_Id = PK_Call_ID;
            var Data = ObjDAL.GetStatus();
            ViewBag.Status = new SelectList(Data, "Value", "Text");

            var Data1 = ObjDAL.GetSecondStatus();
            ViewBag.secondStatus = new SelectList(Data1, "Value", "Text");


            var Data2 = ObjDAL.GetLastStatus();
            ViewBag.GetLastStatus = new SelectList(Data2, "Value", "Text");
            DataSet dss = new DataSet();

            DataTable dtPayment = new DataTable();
            List<ExpCommercial> lstProgress = new List<ExpCommercial>();

            model.PK_Call_Id = PK_Call_ID;

            dtPayment = ObjDAL.EXpPayment(PK_Call_ID);
            if (dtPayment.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPayment.Rows)
                {
                    lstProgress.Add(new ExpCommercial
                    {
                        Id = Convert.ToInt32(dr["pk_Documentsid"]),

                        Documents = Convert.ToString(dr["Documents"]),
                        Status = Convert.ToString(dr["StatusOfSubmission"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Date = Convert.ToString(dr["PaymentDate"]),

                    });
                }
            }
            ViewBag.payment = lstProgress;



            return View(model);
        }

        [HttpPost]
        public ActionResult EXp_commercial(ExpCommercial EC)
        {
            string Result = string.Empty;
            var obj = new ExpCommercial();


            foreach (var item in EC.Activity)
            {


                obj.Documents = Convert.ToString(item.Id);
                obj.Status = item.Status;
                obj.Remarks = item.Remarks;
                obj.Date = item.Date;

                obj.PK_Call_Id = EC.PK_Call_Id;


                Result = ObjDAL.InsertUpdatePayments(obj);


            }





            return RedirectToAction("EXp_commercial", new { PK_Call_Id = EC.PK_Call_Id });

        }

        [HttpPost]
        public ActionResult StatusSaveEng(string PK_Call_Id, string ActionValue, string stage)
        {



            var result = ObjDAL.InsertdataEng(stage, PK_Call_Id, ActionValue);


            return Json(new { success = true });
        }


        [HttpGet]
        public ActionResult GetActionData(string PK_Call_Id, string stage)
        {

            DataSet ds = ObjDAL.GetRecord(PK_Call_Id, stage);
            string materialData = JsonConvert.SerializeObject(ds.Tables[0]);



            return Json(materialData, JsonRequestBehavior.AllowGet);
        }

        #region AddedActionvalue
        [HttpGet]
        public ActionResult JobPageActionValue(int PK_JOB_ID)
        {
            var model = new StatusData();
            model.PK_JOB_ID = PK_JOB_ID;

            #region Bind ddlstatus 
            var Data = objDalSubjob.GetDDLStatus(Convert.ToString(PK_JOB_ID));
            ViewBag.SubCatlist = new SelectList(Data, "Value", "Text");
            #endregion



            #region Bind ddlstatus 
            var Data1 = objDalSubjob.GetMaterialStatus(Convert.ToString(PK_JOB_ID));
            ViewBag.MaterialStatus = new SelectList(Data1, "Value", "Text");
            #endregion

            #region Bind Item
            var Data2 = objDalSubjob.GetManufacturingStatus(Convert.ToString(PK_JOB_ID));
            ViewBag.ManufacturingStatus = new SelectList(Data2, "Value", "Text");
            #endregion
            #region Bind ddlFinalStageStatus 
            var Data3 = objDalSubjob.GetddlFinalStageStatusMaster(Convert.ToString(PK_JOB_ID));
            ViewBag.DDLFinalStatusMaster = new SelectList(Data3, "Value", "Text");
            #endregion

            #region Bind Finalstage 
            var Data4 = objDalSubjob.GetFinalstage(Convert.ToString(PK_JOB_ID));
            ViewBag.SubFinalStage = new SelectList(Data4, "Value", "Text");
            #endregion

            #region Bind Finalstage 
            var Data5 = objDalSubjob.GetDocument(Convert.ToString(PK_JOB_ID));
            ViewBag.RefDocument = new SelectList(Data5, "Value", "Text");
            #endregion

            return View();
        }


        //added by shrutika salve 
        [HttpPost]
        public ActionResult StatusSave(string status)
        {
            var result = objDalSubjob.Insertdata(status);

            return Json(new { success = true });
        }


        [HttpPost]
        public ActionResult StatusSaveMaterial(string status)
        {
            var result = objDalSubjob.InsertdataMaterial(status);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult StatusSaveManufacturing(string status)
        {
            var result = objDalSubjob.InsertdataManufacturing(status);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult StatusSaveFinal(string status)
        {
            var result = objDalSubjob.InsertdataFinal(status);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult StatusSaveFinalstage(string myArray, string PK_JOB_ID)
        {
            List<FinalStages> statusDatavalue = JsonConvert.DeserializeObject<List<FinalStages>>(myArray);

            foreach (FinalStages statusData1 in statusDatavalue)
            {
                var result = objDalSubjob.InsertdataFinalstage(statusData1.Status, PK_JOB_ID);
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult StatusSaveDocument(string myArray, string PK_JOB_ID)
        {
            List<FinalStages> statusDataList = JsonConvert.DeserializeObject<List<FinalStages>>(myArray);
            foreach (FinalStages statusData2 in statusDataList)
            {
                var result = objDalSubjob.InsertdataDocument(statusData2.Document, PK_JOB_ID);
            }

            return Json(new { success = true });
        }


        [HttpPost]
        public ActionResult StatusSaveInsertData(string myArray, string PK_JOB_ID)
        {
            string Result = string.Empty;

            // Deserialize JSON array into a List of a custom class
            List<StatusData> statusDataList = JsonConvert.DeserializeObject<List<StatusData>>(myArray);

            foreach (StatusData statusData in statusDataList)
            {
                if (statusData.statusValue != null && statusData.statusValue != "")
                {
                    Result = objDalSubjob.SaveStageWiseData(statusData.status, statusData.statusValue, statusData.Stageid.ToString(), statusData.statusid.ToString(), PK_JOB_ID, statusData.statusActionRequirdby);
                }

            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult deleteData(string Stageid, string Statusid, string statusName, string PK_JOB_ID)
        {
            var result = objDalSubjob.deleteRecord(Stageid, Statusid, statusName, PK_JOB_ID);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult deleteDataFinalStage(string statusName, string PK_JOB_ID)
        {
            var result = objDalSubjob.deleteRecordFinalStage(statusName, PK_JOB_ID);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult deleteDataDocument(string statusName, string pk_job_id)
        {
            var result = objDalSubjob.deleteRecordDocument(statusName, pk_job_id);

            return Json(new { success = true });
        }


        [HttpGet]
        public ActionResult GetMaterialData(string PK_JOB_ID)
        {

            DataSet ds = objDalSubjob.GetRecord(PK_JOB_ID);
            string materialData = JsonConvert.SerializeObject(ds.Tables[0]);



            return Json(materialData, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetFinalData(string PK_JOB_ID)
        {

            DataSet ds = objDalSubjob.GetFinalRecord(PK_JOB_ID);
            string FinalData = JsonConvert.SerializeObject(ds.Tables[0]);



            return Json(FinalData, JsonRequestBehavior.AllowGet);
        }




        #endregion

        #region Generating Expediting Report   
        [HttpPost]
        public ActionResult GenerateExpeditingReport(ExpeditingData ExpeditingData) //int PK_call_id, string chartData)
        {
            try
            {
                SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
                string body = string.Empty;
                System.Text.StringBuilder strs = new System.Text.StringBuilder();
                //var expeditingDataObject = JsonConvert.DeserializeObject<ExpeditingData>(expeditingData);

                // Now you can access PK_call_id and chartData properties
                var PK_call_id = ExpeditingData.PK_call_id;
                var chartData = HttpUtility.UrlDecode(ExpeditingData.chartData);

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(chartData);

                // Get the first div element
                HtmlNode firstDiv = htmlDoc.DocumentNode.SelectSingleNode("//div");

                if (firstDiv != null)
                {
                    // Get the style attribute
                    HtmlAttribute styleAttribute = firstDiv.Attributes["style"];

                    // If style attribute exists, append width and height attributes
                    if (styleAttribute != null)
                    {
                        styleAttribute.Value += "; width:650px; height:210px;";
                    }
                    else
                    {
                        // If no style attribute exists, create a new one with width and height
                        firstDiv.Attributes.Add("style", "width:650px; height:210px;");
                    }

                    // Output the modified HTML
                    chartData = htmlDoc.DocumentNode.OuterHtml;
                }
                else
                {
                    Console.WriteLine("No div element found.");
                }

                using (StreamReader reader = new StreamReader(Server.MapPath("~/QuotationHtml/ExpeditingHtml.html")))
                {
                    body = reader.ReadToEnd();
                }

                DataTable DSGetRecordFromExpediting = new DataTable();

                DSGetRecordFromExpediting = ObjDAL.EditExpeditingReport(PK_call_id).Tables[0];
                objModel.IsComfirmation = Convert.ToBoolean(DSGetRecordFromExpediting.Rows[0]["isComfirmation"]);
                body = body.Replace("[ExpeditingDate]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Date_Of_Expediting"]));
                body = body.Replace("[ProjectName]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Project_Name"]));
                body = body.Replace("[VendorName]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["VendorName"]));
                body = body.Replace("[ExpeditingLocation]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ExpeditingLocation"]));
                body = body.Replace("[CustomerName]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["End_Customer_Name"]));
                body = body.Replace("[PONumber]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["PONumber"]));
                body = body.Replace("[ExecutingBranch]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Executing_Branch"]));
                body = body.Replace("[TUVIPLCustomer]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Customer_Name"]));
                body = body.Replace("[SubVendor]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Sub_VendorName"]));
                body = body.Replace("[AssignmentNo]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["assignmentNumber"]));
                body = body.Replace("[DEC/EPC/PMC]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["DEC_PMC_EPC_Name"]));
                body = body.Replace("[SubPONumber]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["SubPo_No"]));
                body = body.Replace("[LastExp]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["LastExpeditingtypeandDate"]));
                body = body.Replace("[DEC/EPC/PMCAssignNo]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["DEC_PMC_EPC_Assignment_No"]));
                body = body.Replace("[ContractualDate]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Contractual_DeliveryDate"]));
                body = body.Replace("[ExpeditingClosureRemark]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ClosureRemarks"]));
                body = body.Replace("[IspectorName]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["InspectorName"]));
                body = body.Replace("[ReportDate]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ReportDate"]));

                string I = "<img src = '" + ConfigurationManager.AppSettings["Web"].ToString() + "/Content/sign/" + (DSGetRecordFromExpediting.Rows[0]["Signature"]) + "' style='width:225px;height:125px; ' align='center'>";

                if (Convert.ToInt32(objModel.IsComfirmation) != 0)
                {

                    body = body.Replace("[Signature]", I);
                }
                else
                {
                    body = body.Replace("[Signature]", "");
                }


                body = body.Replace("[GanttChart]", chartData);

                //DataTable DSGetRecordFromCall = new DataTable();
                //DSGetRecordFromCall = ObjDAL.EditExpeditingReportByCall(PK_call_id).Tables[0];

                DataTable dtAllOver = new DataTable();
                dtAllOver = ObjDAL.fetdataAllItemwise(Convert.ToString(PK_call_id));
                body = body.Replace("[EngExpStart]", Convert.ToString(dtAllOver.Rows[0]["ExpectedStartDate"]).Replace(" 00:00:00", ""));
                body = body.Replace("[EngExpEnd]", Convert.ToString(dtAllOver.Rows[0]["ExpectedEndDate"]).Replace(" 00:00:00", ""));
                body = body.Replace("[EngActualStart]", Convert.ToString(dtAllOver.Rows[0]["Actual_Start_Date"]).Replace(" 00:00:00", ""));
                body = body.Replace("[EngActualEnd]", Convert.ToString(dtAllOver.Rows[0]["Actual_End_Date"]).Replace(" 00:00:00", ""));
                body = body.Replace("[EngExpProg]", Convert.ToString(dtAllOver.Rows[0]["PercentageWorkDone"]));
                body = body.Replace("[EngActualProg]", Convert.ToString(dtAllOver.Rows[0]["Actual_Progress"]));
                body = body.Replace("[EngConcernClosure]", Convert.ToString(dtAllOver.Rows[0]["ConcernsStageper"]));
                body = body.Replace("[EngConcern]", Convert.ToString(dtAllOver.Rows[0]["TotalConcerns"]));
                body = body.Replace("[EngStatus]", Convert.ToString(dtAllOver.Rows[0]["Progress_Status"]));
                body = body.Replace("[ProcExpStart]", Convert.ToString(dtAllOver.Rows[1]["ExpectedStartDate"]).Replace(" 00:00:00", ""));
                body = body.Replace("[ProcExpEnd]", Convert.ToString(dtAllOver.Rows[1]["ExpectedEndDate"]).Replace(" 00:00:00", ""));
                body = body.Replace("[ProcActualStart]", Convert.ToString(dtAllOver.Rows[1]["Actual_Start_Date"]).Replace(" 00:00:00", ""));
                body = body.Replace("[ProcActualEnd]", Convert.ToString(dtAllOver.Rows[1]["Actual_End_Date"]).Replace(" 00:00:00", ""));
                body = body.Replace("[ProcExpProg]", Convert.ToString(dtAllOver.Rows[1]["PercentageWorkDone"]));
                body = body.Replace("[ProcActualProg]", Convert.ToString(dtAllOver.Rows[1]["Actual_Progress"]));
                body = body.Replace("[ProcConcernClosure]", Convert.ToString(dtAllOver.Rows[1]["ConcernsStageper"]));
                body = body.Replace("[ProcConcern]", Convert.ToString(dtAllOver.Rows[1]["TotalConcerns"]));
                body = body.Replace("[ProcStatus]", Convert.ToString(dtAllOver.Rows[1]["Progress_Status"]));
                body = body.Replace("[ManuExpStart]", Convert.ToString(dtAllOver.Rows[2]["ExpectedStartDate"]).Replace(" 00:00:00", ""));
                body = body.Replace("[ManuExpEnd]", Convert.ToString(dtAllOver.Rows[2]["ExpectedEndDate"]).Replace(" 00:00:00", ""));
                body = body.Replace("[ManuActualStart]", Convert.ToString(dtAllOver.Rows[2]["Actual_Start_Date"]).Replace(" 00:00:00", ""));
                body = body.Replace("[ManuActualEnd]", Convert.ToString(dtAllOver.Rows[2]["Actual_End_Date"]).Replace(" 00:00:00", ""));
                body = body.Replace("[ManuExpProg]", Convert.ToString(dtAllOver.Rows[2]["PercentageWorkDone"]));
                body = body.Replace("[ManuActualProg]", Convert.ToString(dtAllOver.Rows[2]["Actual_Progress"]));
                body = body.Replace("[ManuConcernClosure]", Convert.ToString(dtAllOver.Rows[2]["ConcernsStageper"]));
                body = body.Replace("[ManuConcern]", Convert.ToString(dtAllOver.Rows[2]["TotalConcerns"]));
                body = body.Replace("[ManuStatus]", Convert.ToString(dtAllOver.Rows[2]["Progress_Status"]));
                body = body.Replace("[FinalExpStart]", Convert.ToString(dtAllOver.Rows[3]["ExpectedStartDate"]).Replace(" 00:00:00", ""));
                body = body.Replace("[FinalExpEnd]", Convert.ToString(dtAllOver.Rows[3]["ExpectedEndDate"]).Replace(" 00:00:00", ""));
                body = body.Replace("[FinalActualStart]", Convert.ToString(dtAllOver.Rows[3]["Actual_Start_Date"]).Replace(" 00:00:00", ""));
                body = body.Replace("[FinalActualEnd]", Convert.ToString(dtAllOver.Rows[3]["Actual_End_Date"]).Replace(" 00:00:00", ""));
                body = body.Replace("[FinalExpProg]", Convert.ToString(dtAllOver.Rows[3]["PercentageWorkDone"]));
                body = body.Replace("[FinalActualProg]", Convert.ToString(dtAllOver.Rows[3]["Actual_Progress"]));
                body = body.Replace("[FinalConcernClosure]", Convert.ToString(dtAllOver.Rows[3]["ConcernsStageper"]));
                body = body.Replace("[FinalConcern]", Convert.ToString(dtAllOver.Rows[3]["TotalConcerns"]));
                body = body.Replace("[FinalStatus]", Convert.ToString(dtAllOver.Rows[3]["Progress_Status"]));
                body = body.Replace("[OverallPlannedStart]", Convert.ToString(dtAllOver.Rows[4]["ExpectedStartDate"]).Replace(" 00:00:00", ""));
                body = body.Replace("[OverallPlannedEnd]", Convert.ToString(dtAllOver.Rows[4]["ExpectedEndDate"]).Replace(" 00:00:00", ""));
                body = body.Replace("[OverallActualStart]", Convert.ToString(dtAllOver.Rows[4]["Actual_Start_Date"]).Replace(" 00:00:00", ""));
                body = body.Replace("[OverallActualEnd]", Convert.ToString(dtAllOver.Rows[4]["Actual_End_Date"]).Replace(" 00:00:00", ""));
                body = body.Replace("[OverallPlannedProg]", Convert.ToString(dtAllOver.Rows[4]["PercentageWorkDone"]));
                body = body.Replace("[OverallActual]", Convert.ToString(dtAllOver.Rows[4]["Actual_Progress"]));
                body = body.Replace("[OverallStatus]", Convert.ToString(dtAllOver.Rows[4]["Progress_Status"]));
                body = body.Replace("[OverallProgress]", Convert.ToString(dtAllOver.Rows[4]["Progress_Status"]));
                body = body.Replace("[OverallConcernClosure]", Convert.ToString(dtAllOver.Rows[2]["ConcernsStageper"]));
                body = body.Replace("[OverallConcern]", Convert.ToString(dtAllOver.Rows[2]["TotalConcerns"]));
                string OverallStatus = Convert.ToString(dtAllOver.Rows[4]["Progress_Status"]);

                string StatusColor = string.Empty;
                if (OverallStatus == "Early") { StatusColor = "#334BDB"; }
                else if (OverallStatus == "On Time") { StatusColor = "#A3F333"; }
                else if (OverallStatus == "Delay") { StatusColor = "#FFEF33"; }

                HtmlDocument htmlDoc1 = new HtmlDocument();
                htmlDoc1.LoadHtml(body);
                HtmlNode tdStyle = htmlDoc1.DocumentNode.SelectSingleNode("//td[@id='OverallStatus']");
                if (tdStyle != null)
                {
                    // Get the style attribute
                    HtmlAttribute styleAttribute = tdStyle.Attributes["style"];

                    // If style attribute exists, append width and height attributes
                    if (styleAttribute != null)
                    {
                        styleAttribute.Value += "; background-color:" + StatusColor;
                    }
                }
                body = htmlDoc1.DocumentNode.OuterHtml;

                DataTable dtAction = new DataTable();

                dtAction = ObjDAL.GetActionDataByPK_Call_Id(Convert.ToString(PK_call_id));

                body = body.Replace("[ConcernVendor]", Convert.ToString(dtAction.Rows[0]["SubVendor"]));
                body = body.Replace("[ConcernCust]", Convert.ToString(dtAction.Rows[0]["Customer"]));
                body = body.Replace("[ConcernTUVI]", Convert.ToString(dtAction.Rows[0]["TUVI"]));
                body = body.Replace("[EngVendor]", Convert.ToString(dtAction.Rows[1]["SubVendor"]));
                body = body.Replace("[EngCust]", Convert.ToString(dtAction.Rows[1]["Customer"]));
                body = body.Replace("[EngTUVI]", Convert.ToString(dtAction.Rows[1]["TUVI"]));
                body = body.Replace("[ConcernTotal]", Convert.ToString(dtAction.Rows[0]["Total"]));
                body = body.Replace("[EngTotal]", Convert.ToString(dtAction.Rows[1]["Total"]));
 DataTable dsGetStamp = new DataTable();
               
                dsGetStamp = ObjDAL.GetOpenConcernDataByPK_Call_Id(Convert.ToString(PK_call_id));
                string StrOpenConcern = string.Empty;

                if (dsGetStamp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGetStamp.Rows)
                    {
                        string POItem = AddBreak(Convert.ToString(dr["po_item_number"]));
                        string Stage = Convert.ToString(dr["Stages"]);
                        string RaisedOn = Convert.ToString(dr["RaisedOn"]);
                        string Details = Convert.ToString(dr["Details"]);
                        string ActionReq = Convert.ToString(dr["ActionReq"]);
                        string MitigationBy = Convert.ToString(dr["MitigationBy"]);
                        string ExpectedClosureDate = Convert.ToString(dr["ExpectedClosureDate"]);
                        string Comment = Convert.ToString(dr["Comment"]);

                        StrOpenConcern = StrOpenConcern + "<tr><td style=\"width:38.75pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + POItem + "</span></p></td><td style=\"width:60.3pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + Stage + "</span></p></td><td style=\"width:50.35pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + RaisedOn + "</span></p></td><td style=\"width:150.2pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + Details + "</span></p></td><td style=\"width:70.75pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + ActionReq + "</span></p></td><td colspan=\"3\" style=\"width:190.05pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + MitigationBy + "</span></p></td><td style=\"width:70.55pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + ExpectedClosureDate + "</span></p></td><td colspan=\"3\" style=\"width:200.05pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + Comment + "</span></p></td></tr>";
                        //"<tr><td style=\"width:41.3pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ POItem + "</span></p></td><td colspan=\"2\" style=\"width:58.95pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ Stage + "</span></p></td><td style=\"width:29.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ RaisedOn + "</span></p></td><td style=\"width:30.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ Details + "</span></p></td><td style=\"width:38.8pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ ActionReq + "</span></p></td><td colspan=\"2\" style=\"width:46.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span> "
                        //+ MitigationBy + "</span></p></td><td style=\"width:40.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ ExpectedClosureDate + "</span></p></td><td colspan=\"3\" style=\"width:124.4pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ Comment + "</span></p></td></tr>";
                    }
                    body = body.Replace("[OpenConcern]", StrOpenConcern);
                }
                else
                {
                    HtmlDocument htmldoc = new HtmlDocument();
                    htmldoc.LoadHtml(body);

                    HtmlNode divToRemove = htmldoc.DocumentNode.SelectSingleNode("//div[@id='OpenConcernDiv']");

                    if (divToRemove != null)
                    {
                        divToRemove.Remove();
                    }

                    body = htmldoc.DocumentNode.OuterHtml;
                }


                DataTable CostSheetDashBoard = new DataTable();
                string StrPOItem = string.Empty;

                CostSheetDashBoard = ObjDAL.GetitemDescription(PK_call_id);

                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        string PO_Item_Number = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
                        string ItemCode = Convert.ToString(dr["ItemCode"]);
                        string ItemDescription = Convert.ToString(dr["ItemDescription"]);
                        string Unit = Convert.ToString(dr["Unit"]);
                        string Quantity = Convert.ToString(dr["Quantity"]);
                        string Contractual_DeliveryDateAsPerPO = Convert.ToString(dr["Contractual_DeliveryDateAsPerPO"]);
                        string EstimatedDeliveryDate = Convert.ToString(dr["EstimatedDeliveryDate"]);
                        string progressStatus = Convert.ToString(dr["Progress_Status"]);

                        StrPOItem = StrPOItem + "<tr style=\"height:20.9pt\"><td style=\"width:42.2pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + PO_Item_Number + "</span></p></td><td style=\"width:58.05pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + ItemCode + "</span></p></td><td colspan=\"5\" style=\"width:120pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + ItemDescription + "</span></p></td><td style=\"width:40.65pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + Unit + "</span></p></td><td style=\"width:10.25pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + Quantity + "</span></p></td><td style=\"width:28.8pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + Contractual_DeliveryDateAsPerPO + "</span></p></td><td style=\"width:28.45pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + EstimatedDeliveryDate + "</span></p></td><td style=\"width:60.55pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + progressStatus + "</span></p></td></tr>";
                        //"<tr style=\"height:20.9pt\"><td colspan=\"2\" style=\"width:42.2pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ PO_Item_Number + "</span></p></td><td style=\"width:58.05pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ ItemCode + "</span></p></td><td colspan=\"4\" style=\"width:131pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ ItemDescription + "</span></p></td><td style=\"width:35.65pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ Unit + "</span></p></td><td style=\"width:40.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ Quantity + "</span></p></td><td style=\"width:18.8pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ Contractual_DeliveryDateAsPerPO + "</span></p></td><td style=\"width:18.45pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ EstimatedDeliveryDate + "</span></p></td><td style=\"width:65.55pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //+ progressStatus + "</span></p></td></tr>";
                    }
                }

                body = body.Replace("[PODetails]", StrPOItem);

                DataTable dtEngg = new DataTable();
                dtEngg = ObjDAL.GetEngineeringDataByPK_Call_Id(Convert.ToString(PK_call_id));
                string StrEngineeringData = string.Empty;
                if (dtEngg.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEngg.Rows)
                    {
                        string Item = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
                        string ReferenceDocument = Convert.ToString(dr["DocumentRefName"]);
                        string DocNumber = Convert.ToString(dr["Number"]);
                        string QuantityNumber = Convert.ToString(dr["QuantityNumber"]);
                        string Status = Convert.ToString(dr["name"]);
                        string StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]);
                        string EDD = Convert.ToString(dr["EstimatedEndDate"]);

                        StrEngineeringData = StrEngineeringData + "<tr><td style=\"width:42.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Item + "</span></p></td><td colspan=\"2\" style=\"width:100.3pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + ReferenceDocument + "</span></p></td><td colspan=\"7\" style=\"width:270.05pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + DocNumber + "</span></p></td><td colspan=\"3\" style=\"width:70.9pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + QuantityNumber + "</span></p></td><td colspan=\"2\" style=\"width:90.35pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Status + "</span></p></td><td style=\"width:80.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + StatusUpdatedOn + "</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + EDD + "</span></p></td></tr>";
                    }
                    body = body.Replace("[EngData]", StrEngineeringData);
                }

                DataTable dtProc = new DataTable();
                dtProc = ObjDAL.GetMaterialDataByPK_Call_Id(Convert.ToString(PK_call_id));
                string StrProcurement = string.Empty;

                if (dtProc.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProc.Rows)
                    {
                        string Item = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
                        string Description = Convert.ToString(dr["Description"]);
                        string UOM = Convert.ToString(dr["UnitOM"]);
                        string Quantity = Convert.ToString(dr["Quantity"]);
                        string SupplierName = Convert.ToString(dr["OrderPlacedOn"]);
                        string CDD = Convert.ToString(dr["Contractual_DeliveryDate"]);
                        string EDD = Convert.ToString(dr["EstimatedDeliveryDate"]);
                        string Status = Convert.ToString(dr["Stages"]);
                        string StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]);
                        string EstimatedEndDate = Convert.ToString(dr["EstimatedEndDate"]);

                        StrProcurement = StrProcurement + "<tr><td style=\"width:42.75pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + Item + "</span></p></td><td colspan=\"4\" style=\"width:220.3pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + Description + "</span></p></td><td style=\"width:40.05pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + UOM + "</span></p></td><td colspan=\"1\" style=\"width:40.9pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + Quantity + "</span></p></td><td colspan=\"2\" style=\"width:120.35pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + SupplierName + "</span></p></td><td style=\"width:60.25pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + CDD + "</span></p></td><td style=\"width:60.25pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + EDD + "</span></p></td><td style=\"width:55.25pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + Status + "</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + StatusUpdatedOn + "</span></p></td><td style=\"width:50.25pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
                            + EstimatedEndDate + "</span></p></td></tr>";
                        //"<tr><td style=\"width:42.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //            + Item + "</span></p></td><td colspan=\"3\" style=\"width:200.3pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //            + Description + "</span></p></td><td colspan=\"1\" style=\"width:50.05pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //            + UOM + "</span></p></td><td colspan=\"2\" style=\"width:50.9pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //            + Quantity + "</span></p></td><td colspan=\"2\" style=\"width:70.35pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //            + SupplierName + "</span></p><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>Name</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //            + CDD + "</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //            + EDD + "</span></p></td><td style=\"width:55.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //            + Status + " </span></p></td><td style =\"width:80.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //            + StatusUpdatedOn + "</span></p></td><td style=\"width:60.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        //            + EstimatedEndDate + "</span></p></td></tr>";
                    }
                    body = body.Replace("[ProcData]", StrProcurement);
                }

                DataTable dtManu = new DataTable();
                dtManu = ObjDAL.GetManufacturingDataByPK_Call_Id(Convert.ToString(PK_call_id));
                string StrManufacture = string.Empty;

                if (dtManu.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtManu.Rows)
                    {
                        string Item = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
                        string PartName = Convert.ToString(dr["PartName"]);
                        string Status = Convert.ToString(dr["Stages"]);
                        string StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]);
                        string EstimatedEndDate = Convert.ToString(dr["EstimatedEndDate"]);

                        StrManufacture = StrManufacture + "<tr><td style=\"width:42.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Item + "</span></p></td><td colspan=\"13\" style=\"width:530.2pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + PartName + "</span></p></td><td style=\"width:100pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Status + "</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + StatusUpdatedOn + "</span></p></td><td style=\"width:60.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + EstimatedEndDate + "</span></p></td></tr>";
                    }
                    body = body.Replace("[ManuData]", StrManufacture);
                }

                DataTable dtFinal = new DataTable();
                dtFinal = ObjDAL.GetFinalStageByPK_Call_Id(Convert.ToString(PK_call_id));
                string StrFinal = string.Empty;

                if (dtFinal.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinal.Rows)
                    {
                        string Item = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
                        string Stage = Convert.ToString(dr["Stage"]);
                        string Status = Convert.ToString(dr["Stages"]);
                        string StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]);
                        string EstimatedEndDate = Convert.ToString(dr["EstimatedEndDate"]);

                        StrFinal = StrFinal + "<tr><td style=\"width:42.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Item + "</span></p></td><td colspan=\"13\" style=\"width:530.2pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Stage + "</span></p></td><td style=\"width:100pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Status + "</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + StatusUpdatedOn + "</span></p></td><td style=\"width:60.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + EstimatedEndDate + "</span></p></td></tr>";
                    }
                    body = body.Replace("[FinalData]", StrFinal);
                }

                DataTable dtPayment = new DataTable();
                dtPayment = ObjDAL.EXpPayment(Convert.ToString(PK_call_id));
                string StrPayment = string.Empty;

                if (dtPayment.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPayment.Rows)
                    {
                        string Documents = Convert.ToString(dr["Documents"]);
                        string Date = Convert.ToString(dr["PaymentDate"]);
                        string Status = Convert.ToString(dr["Stages"]);
                        string Remarks = Convert.ToString(dr["Remarks"]);

                        StrPayment = StrPayment + "<tr><td colspan=\"7\" style=\"width:260pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Documents + "</span></p></td><td colspan=\"1\" style=\"width:75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Date + "</span></p></td><td colspan=\"2\" style=\"width:160pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Status + "</span></p></td><td colspan=\"6\" style=\"width:320pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Remarks + "</span></p></td></tr>";
                    }

                    body = body.Replace("[CommercialData]", StrPayment);
                }

                DataTable dtAttendee = new DataTable();
                dtAttendee = ObjDAL.EditExpeditingReportAttendeesName(PK_call_id);
                string StrAttendee = string.Empty;

                if (dtAttendee.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAttendee.Rows)
                    {
                        string Representing = Convert.ToString(dr["Represting"]);
                        string Name = Convert.ToString(dr["Name"]);
                        string Email = Convert.ToString(dr["EmailID"]);
                        string Designation = Convert.ToString(dr["Designation"]);

                        StrAttendee = StrAttendee + "<tr><td colspan=\"8\" style=\"width:160pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Representing + "</span></p></td><td colspan=\"2\" style=\"width:190pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Name + "</span></p></td><td colspan=\"2\" style=\"width:165pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Designation + "</span></p></td><td colspan=\"4\" style=\"width:300pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Email + "</span></p></td></tr>";
                    }
                    body = body.Replace("[AttendeeData]", StrAttendee);
                }

                DataTable DSdata = new DataTable();
                DSdata = ObjDAL.GetdistributionId(PK_call_id);
                string strDistribution = string.Empty;

                if (DSdata.Rows.Count > 0)
                {
                    Boolean Customer = Convert.ToBoolean(DSdata.Rows[0]["DCustomer"]);
                    string Cust = (Customer == true) ? "checked=\"checked\"" : "";
                    Boolean Vendor = Convert.ToBoolean(DSdata.Rows[0]["DVendor"]);
                    string Vend = (Vendor == true) ? "checked=\"checked\"" : "";
                    Boolean TUVI = Convert.ToBoolean(DSdata.Rows[0]["DTuvi"]);
                    string TUV = (TUVI == true) ? "checked=\"checked\"" : "";

                    strDistribution = strDistribution + "<td colspan=\"6\" style=\"padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><input type=\"checkbox\""
                        + Cust + "\"><label style=\"font-size:9pt\">Customer / End User</label></td><td colspan=\"6\" style=\"padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><input type=\"checkbox\""
                        + Vend + "\"><label style=\"font-size:9pt\">Vendor/ Sub Vendor</label></td><td colspan=\"6\" style=\"border-right: 0.75pt solid black;padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><input type=\"checkbox\""
                        + TUV + "\"><label style=\"font-size:9pt\">TUVI Executing / Originationg Branch</label></td>";

                }
                body = body.Replace("[DistributionData]", strDistribution);

                DataTable dtObs = new DataTable();
                dtObs = ObjDAL.GetObservationDataByPK_Call_Id(Convert.ToString(PK_call_id));
                string StrObservationData = string.Empty;
                if (dtObs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtObs.Rows)
                    {
                        string Stages = Convert.ToString(dr["ObsStages"]);

                        if (Stages == "Engineering") { body = body.Replace("[EngObservation]", Convert.ToString(dr["Findings"])); }
                        else if (Stages == "Procurement") { body = body.Replace("[ProcObservation]", Convert.ToString(dr["Findings"])); }
                        else if (Stages == "Manufacturing") { body = body.Replace("[ManuObservation]", Convert.ToString(dr["Findings"])); }
                        else if (Stages == "Final") { body = body.Replace("[FinalObservation]", Convert.ToString(dr["Findings"])); }

                        //body = body.Replace("[ExpeditingClosureRemark]", Convert.ToString(dtAction.Rows[1]["Customer"]));
                    }
                    body = body.Replace("[EngObservation]", "");
                    body = body.Replace("[ProcObservation]", "");
                    body = body.Replace("[ManuObservation]", "");
                    body = body.Replace("[FinalObservation]", "");
                }

                int count = 0;
                #region report Count
                DSdata = objDalVisitReport.GetReportByCall_Id(PK_call_id);
                if (DSdata.Rows.Count > 0)
                {
                    int counts = DSdata.Rows.Count;
                    count = counts - 1;
                }
                string countNo = Convert.ToString(count);
                #endregion

                #region Report Image data
                DataTable ImageReportDashBoard = new DataTable();
                List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();
                //GetReportImageByCall_Id
                ImageReportDashBoard = ObjDAL.GetReportImageByCall_Id(Convert.ToString(PK_call_id));
                if (ImageReportDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in ImageReportDashBoard.Rows)
                    {
                        ImageDashBoard.Add(
                            new ReportImageModel
                            {
                                Image = Convert.ToString(dr["Image"]),
                                Heading = Convert.ToString(dr["Heading"]),
                            }
                            );
                    }
                }
                #endregion

                #region Image Save to pdf
                SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
                System.Text.StringBuilder strss = new System.Text.StringBuilder();

                string bodys = string.Empty;
                string ImageContent = string.Empty;
                string ReportNames = string.Empty;
                string paths = string.Empty;
                int img = 0;
                int imagecount = ImageReportDashBoard.Rows.Count;
                int rows = imagecount / 3;
                int imageposted = 0;
                int reminder = (imagecount % 3);
                int iteration = 1;
                if (imagecount > 0 && imagecount < 3)
                {
                    rows = 1;
                }

                ///First File start
                PdfPageSize pageSizes = PdfPageSize.A4;
                PdfPageOrientation pdfOrientations = PdfPageOrientation.Landscape;

                HtmlToPdf converters = new HtmlToPdf();

                #endregion

                #region Vaibhav If Images count less than 6

                if (imagecount <= 6)
                {
                    for (int ic = 0; ic < rows; ic++)
                    {
                        if (imageposted > 0)
                        {
                            if ((imageposted % 6) == 0)
                            {

                                body = body.Replace("[ExpeditingImage]", ImageContent);

                                ImageContent = string.Empty;
                                iteration = iteration + 1;
                                ViewBag.Reminder = "1";
                            }
                        }

                        ImageContent += "<tr><td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img]["Heading"].ToString() + "</td>";
                        ImageContent += "<td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img + 1]["Heading"].ToString() + "</td>";
                        ImageContent += "<td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img + 2]["Heading"].ToString() + "</td></tr>";

                        ImageContent += "<tr><td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td>";
                        ImageContent += "<td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img + 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td>";
                        ImageContent += "<td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img + 2]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";

                        img = img + 3;
                        imageposted = imageposted + 3;
                    }



                    #region Reminder = 1

                    if (reminder == 1)
                    {
                        if (ImageContent != string.Empty)
                        {
                            ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding: 10px; width: 33 %;\" align=\"center\">" + ImageReportDashBoard.Rows[imagecount - 1]["Heading"].ToString() + "</td></tr>";
                            ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding:10px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[imagecount - 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";
                        }
                        else
                        {
                            ImageContent = "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding: 10px; width: 33 %;\" align=\"center\">" + ImageReportDashBoard.Rows[imagecount - 1]["Heading"].ToString() + "</td></tr>";
                            ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding:10px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[imagecount - 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";
                        }

                        body = body.Replace("[ExpeditingImage]", ImageContent);


                    }

                    #region 4 Image If not reminder =0
                    if (imageposted <= imagecount)
                    {
                        if (reminder == 0)
                        {
                            ViewBag.Reminder = "2";
                            body = body.Replace("[ExpeditingImage]", ImageContent);

                            ImageContent = string.Empty;
                            iteration = iteration + 1;
                        }
                    }

                    #endregion

                }
                #endregion
                #endregion

                #region Vaibhav If Images count more than 6
                else
                {
                    for (int ic = 0; ic < rows; ic++)
                    {

                        ImageContent += "<tr><td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img]["Heading"].ToString() + "</td>";
                        ImageContent += "<td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img + 1]["Heading"].ToString() + "</td>";
                        ImageContent += "<td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img + 2]["Heading"].ToString() + "</td></tr>";

                        ImageContent += "<tr><td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td>";
                        ImageContent += "<td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img + 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td>";
                        ImageContent += "<td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img + 2]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";

                        img = img + 3;
                        imageposted = imageposted + 3;
                    }

                    ViewBag.Reminder = "2";

                    #region  reminder
                    if (reminder == 1)
                    {

                        if (ImageContent != string.Empty)
                        {

                            ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding: 10px; width: 33 %;\" align=\"center\">" + ImageReportDashBoard.Rows[imagecount - 1]["Heading"].ToString() + "</td></tr>";
                            ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding:10px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[imagecount - 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";

                        }
                        else
                        {
                            ImageContent = "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding: 10px; width: 33 %;\" align=\"center\">" + ImageReportDashBoard.Rows[imagecount - 1]["Heading"].ToString() + "</td></tr>";
                            ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding:10px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[imagecount - 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";
                        }

                        body = body.Replace("[ExpeditingImage]", ImageContent);

                        #region Initial setting vaibhav
                        //strs.Append(body);
                        PdfPageSize vpageSize = PdfPageSize.A4;
                        PdfPageOrientation vpdfOrientation = PdfPageOrientation.Landscape;
                        HtmlToPdf Vconverter = new HtmlToPdf();

                        // set the page timeout (in seconds)
                        Vconverter.Options.MaxPageLoadTime = 240;  //=========================5-Aug-2019
                        Vconverter.Options.PdfPageSize = vpageSize;
                        Vconverter.Options.PdfPageOrientation = vpdfOrientation;
                        #endregion

                        #region Header and Footer Vaibhav
                        #region Heder code
                        string _VHeader = string.Empty;
                        string _Vfooter = string.Empty;

                        // for Report header by abel
                        StreamReader _VreadHeader_File = new StreamReader(Server.MapPath("~/QuotationHtml/Expediting-header.html"));
                        _VHeader = _VreadHeader_File.ReadToEnd();
                        _VHeader = _VHeader.Replace("[logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.svg");
                        _VHeader = _VHeader.Replace("[ExpeditingType]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ExpeditingType"]));
                        _VHeader = _VHeader.Replace("[ReportNumber]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ReportNo"]));

                        #endregion

                        #region Footer Code

                        StreamReader _VreadFooter_File = new StreamReader(Server.MapPath("~/QuotationHtml/Expediting-footer.html"));
                        _Vfooter = _VreadFooter_File.ReadToEnd();
                        _Vfooter = _Vfooter.Replace("[LogoFooter]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/FTUEV-NORD-GROUP_Logo_Electric-Blue.svg");

                        // header settings
                        Vconverter.Options.DisplayHeader = true || true || true;
                        Vconverter.Header.DisplayOnFirstPage = true;
                        Vconverter.Header.DisplayOnOddPages = true;
                        Vconverter.Header.DisplayOnEvenPages = true;
                        Vconverter.Header.Height = 75;

                        PdfHtmlSection VheaderHtml = new PdfHtmlSection(_VHeader, string.Empty);
                        VheaderHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                        Vconverter.Header.Add(VheaderHtml);

                        // footer settings
                        Vconverter.Options.DisplayFooter = true || true || true;
                        Vconverter.Footer.DisplayOnFirstPage = true;
                        Vconverter.Footer.DisplayOnOddPages = true;
                        Vconverter.Footer.DisplayOnEvenPages = true;

                        //Vconverter.Footer.Height = 150;
                        Vconverter.Footer.Height = 105;
                        //converter.Footer.Height = 120;

                        PdfHtmlSection VfooterHtml = new PdfHtmlSection(_Vfooter, string.Empty);
                        VfooterHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                        Vconverter.Footer.Add(VfooterHtml);
                        #endregion
                        #endregion

                        SelectPdf.PdfDocument docs = Vconverter.ConvertHtmlString(bodys);
                        ReportNames = Convert.ToString(PK_call_id) + "_Img_" + iteration + ".pdf";

                        paths = Server.MapPath("~/Content/" + Convert.ToString(PK_call_id));
                        docs.Save(paths + '\\' + ReportNames);
                        docs.Close();
                        bodys = string.Empty;
                        ImageContent = string.Empty;
                        ViewBag.Reminder = "1";
                    }
                    #endregion


                    #region If ImageContent not null

                    if (ViewBag.Reminder != "1")
                    {
                        ViewBag.Reminder = "2";
                        body = body.Replace("[ExpeditingImage]", ImageContent);

                    }
                    #endregion

                }

                #endregion

                //PdfPageSize pageSize = PdfPageSize.A4;
                //PdfPageOrientation pdfOrientation = PdfPageOrientation.Landscape;
                HtmlToPdf converter = new HtmlToPdf();

                SelectPdf.PdfDocument idoc = converter.ConvertHtmlString(body);
                //if (Convert.ToInt32(objModel.IsComfirmation) != 1)
                //{
                //    string imgFile1 = Server.MapPath("~/WaterMark.png");
                //    SelectPdf.PdfTemplate template1 = idoc.AddTemplate(idoc.Pages[0].ClientRectangle);
                //    PdfImageElement img1 = new PdfImageElement(150, 150, imgFile1);
                //    img1.Transparency = 15;
                //    template1.Add(img1);
                //}
                int ImagePageCount = idoc.Pages.Count;

                DataTable dtProgress = new DataTable();

                dtProgress = ObjDAL.EXpDalayWise(Convert.ToString(PK_call_id));
                string StrProgress = string.Empty;
                if (dtProgress.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProgress.Rows)
                    {
                        string item = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
                        string PoitemCode = Convert.ToString(dr["ItemCode"]);
                        string ExpstartDate = Convert.ToString(dr["exprctedstartDate"]);
                        string ExpEndDate = Convert.ToString(dr["ExpectedEndDate"]);
                        string ActstartDate = Convert.ToString(dr["Actual_Start_Date"]);
                        string ActEndDate = Convert.ToString(dr["Actual_End_Date"]);
                        string Concerns = Convert.ToString(dr["TotalConcerns"]);
                        string engDelay = Convert.ToString(dr["DelayEng"]);
                        string ProDelay = Convert.ToString(dr["DelayProcurement"]);
                        string manuDelay = Convert.ToString(dr["DelayManufacturing"]);
                        string FinalDelay = Convert.ToString(dr["DelayFinal"]);
                        string engExp = Convert.ToString(dr["ExceptedEng"]);
                        string ProExp = Convert.ToString(dr["ExceptedProcurement"]);
                        string manuExp = Convert.ToString(dr["ExceptedManufacturing"]);
                        string FinalExp = Convert.ToString(dr["ExceptedFinal"]);
                        string engAct = Convert.ToString(dr["ActEng"]);
                        string ProAct = Convert.ToString(dr["ActProcurement"]);
                        string manuAct = Convert.ToString(dr["ActManufacturing"]);
                        string FinalAct = Convert.ToString(dr["ActFinal"]);
                        string actualprogress = Convert.ToString(dr["actualprogress"]);
                        string progressStatus = Convert.ToString(dr["Progress_Status"]);

                        StrProgress = StrProgress + "<tr><td  style=\"width:30pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + item + "</span></p></td><td  style=\"width:40pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + PoitemCode + "</span></p></td><td  style=\"width:40pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + ExpstartDate + "</span></p></td><td  style=\"width:40pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + ExpEndDate + "</span></p></td><td  style=\"width:40pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + ActstartDate + "</span></p></td><td  style=\"width:40pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + ActEndDate + "</span></p></td><td  style=\"width:40.65pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + Concerns + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + engDelay + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + ProDelay + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + manuDelay + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + FinalDelay + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + engExp + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + ProExp + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + manuExp + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + FinalExp + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + engAct + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + ProAct + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + manuAct + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + FinalAct + "</span></p></td><td  style=\"width:20.65pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + actualprogress + "</span></p></td><td  style=\"width:20.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                        + progressStatus + "</span></p></td></tr>";
                    }
                    body = body.Replace("[ItemWiseProgressData]", StrProgress);
                }

                DataTable dtClosedConcern = new DataTable();
                dtClosedConcern = ObjDAL.GetClosedConcernDataByPK_Call_Id(Convert.ToString(PK_call_id));
                string StrClosedConcern = string.Empty;

                if (dtClosedConcern.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtClosedConcern.Rows)
                    {
                        string POItem = Convert.ToString(dr["po_item_number"]);
                        string Stage = Convert.ToString(dr["Stages"]);
                        string RaisedOn = Convert.ToString(dr["RaisedOn"]);
                        string Details = Convert.ToString(dr["Details"]);
                        string ActionReq = Convert.ToString(dr["ActionReq"]);
                        string MitigationBy = Convert.ToString(dr["MitigationBy"]);
                        string ExpectedClosureDate = Convert.ToString(dr["ExpectedClosureDate"]);
                        string Comment = Convert.ToString(dr["Comment"]);

                        StrClosedConcern = StrClosedConcern + "<tr><td style=\"width:41.3pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + POItem + "</span></p></td><td colspan=\"2\" style=\"width:58.95pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Stage + "</span></p></td><td style=\"width:29.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + RaisedOn + "</span></p></td><td style=\"width:30.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Details + "</span></p></td><td style=\"width:38.8pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + ActionReq + "</span></p></td><td colspan=\"2\" style=\"width:46.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span> "
                            + MitigationBy + "</span></p></td><td style=\"width:40.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + ExpectedClosureDate + "</span></p></td><td colspan=\"3\" style=\"width:124.4pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
                            + Comment + "</span></p></td></tr>";
                    }
                    body = body.Replace("[ClosedConcern]", StrClosedConcern);
                }
                else
                {
                    HtmlDocument htmldoc = new HtmlDocument();
                    htmldoc.LoadHtml(body);

                    HtmlNode divToRemove = htmldoc.DocumentNode.SelectSingleNode("//div[@id='ClosedConcernDiv']");

                    if (divToRemove != null)
                    {
                        divToRemove.Remove();
                    }

                    body = htmldoc.DocumentNode.OuterHtml;
                }


                PdfPageSize pageSize = PdfPageSize.A4;
                PdfPageOrientation pdfOrientation = PdfPageOrientation.Landscape;
                //HtmlToPdf converter = new HtmlToPdf();

                SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(body);
                int PageCount = doc1.Pages.Count;
                body = body.Replace("[PageCount]", "(Refer Page " + Convert.ToString(PageCount) + " Of " + Convert.ToString(PageCount) + " )");
                strs.Append(body);

                converter.Options.MaxPageLoadTime = 240;
                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;

                string _Header = string.Empty;
                string _footer = string.Empty;

                StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/QuotationHtml/Expediting-header.html"));
                _Header = _readHeader_File.ReadToEnd();


                if (Convert.ToInt32(objModel.IsComfirmation) != 0)
                {
                    _Header = _Header.Replace("[logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.svg");

                }

                else
                {
                    _Header = _Header.Replace("[logo]", "");
                }
                _Header = _Header.Replace("[ExpeditingType]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ExpeditingType"]));
                _Header = _Header.Replace("[ReportNumber]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ReportNo"]));

                StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/QuotationHtml/Expediting-footer.html"));
                _footer = _readFooter_File.ReadToEnd();

                if (Convert.ToInt32(objModel.IsComfirmation) != 0)
                {
                    _footer = _footer.Replace("[LogoFooter]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/FTUEV-NORD-GROUP_Logo_Electric-Blue.svg");
                }

                else
                {
                    _footer = _footer.Replace("[LogoFooter]", "");

                }


                // header settings
                converter.Options.DisplayHeader = true ||
                    true || true;
                converter.Header.DisplayOnFirstPage = true;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = 85;

                PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
                headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;

                converter.Header.Add(headerHtml);

                // footer settings
                converter.Options.DisplayFooter = true || true || true;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                converter.Footer.Height = 100;

                PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
                footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Footer.Add(footerHtml);

                PdfTextSection text1 = new PdfTextSection(65, 73, "Page: {page_number} of {total_pages}", new System.Drawing.Font("TNG Pro", 8, FontStyle.Bold));

                converter.Footer.Add(text1);

                converter.Options.SecurityOptions.CanAssembleDocument = true;
                converter.Options.SecurityOptions.CanCopyContent = true;
                converter.Options.SecurityOptions.CanEditAnnotations = true;
                converter.Options.SecurityOptions.CanEditContent = true;
                converter.Options.SecurityOptions.CanFillFormFields = true;
                converter.Options.SecurityOptions.CanPrint = true;

                //PdfDocument doc = converter.ConvertHtmlString(body);
                PdfDocument doc = new PdfDocument();

                converter.Options.PdfCompressionLevel = PdfCompressionLevel.Best;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;

                doc1 = converter.ConvertHtmlString(body);




                if (Convert.ToInt32(objModel.IsComfirmation) != 1)
                {
                    string imgFile1 = Server.MapPath("~/WaterMark.png");
                    //SelectPdf.PdfTemplate template1 = doc1.AddTemplate(doc1.Pages[0].ClientRectangle);
                    //PdfImageElement img1 = new PdfImageElement(0, 0,0,0, imgFile1);
                    //img1.Transparency = 15;
                    //template1.Add(img1);
                    var pageRectangle = doc1.Pages[0].ClientRectangle;
                    float pageWidth = pageRectangle.Width;
                    float pageHeight = pageRectangle.Height;

                    // Load the image to determine its size
                    PdfImageElement img1 = new PdfImageElement(0, 0, imgFile1);

                    // Calculate center position for the image
                    float imgX = (pageWidth - img1.Width) / 2;
                    float imgY = (pageHeight - img1.Height) / 2;

                    // Set the image's position
                    img1.X = imgX;
                    img1.Y = imgY;
                    img1.Transparency = 15;

                    // Add the template to the page
                    SelectPdf.PdfTemplate template1 = doc.AddTemplate(pageRectangle);
                    template1.Add(img1);
                }
                int PageNo = doc1.Pages.Count;

                int pages = imagecount / 6;
                int remainder = imagecount % 6;
                if (remainder > 0) { remainder = 1; } else { remainder = 0; }
                string TotalImagePages = string.Empty;
                int PhotoPageNo = (PageNo - (pages + remainder)) + 1;

                for (int c = PhotoPageNo; c <= PageNo; c++)
                {
                    // Check the condition involving a, b, and c
                    if (c >= PhotoPageNo && c <= PageNo)
                    {
                        // Append the valid value of c to the string
                        TotalImagePages += c + ",";
                    }
                }

                if (TotalImagePages == "")
                {
                    TotalImagePages = "";
                }
                else
                {
                    TotalImagePages = TotalImagePages.Remove(TotalImagePages.Length - 1);
                }
                // TotalImagePages = TotalImagePages.Remove(TotalImagePages.Length - 1);

                body = body.Replace("[PhotoPageNo]", TotalImagePages);


                // Now you have the total number of pages
                // Convert HTML string to PDF document
                doc = converter.ConvertHtmlString(body);
                if (Convert.ToInt32(objModel.IsComfirmation) != 1)
                {
                    string imgFile1 = Server.MapPath("~/WaterMark.png");
                    //SelectPdf.PdfTemplate template1 = doc.AddTemplate(doc.Pages[0].ClientRectangle);
                    //PdfImageElement img1 = new PdfImageElement(0, 0,0,0, imgFile1);
                    //img1.Transparency = 15;
                    //template1.Add(img1);
                    var pageRectangle = doc.Pages[0].ClientRectangle;
                    float pageWidth = pageRectangle.Width;
                    float pageHeight = pageRectangle.Height;

                    // Load the image to determine its size
                    PdfImageElement img1 = new PdfImageElement(0, 0, imgFile1);

                    // Calculate center position for the image
                    float imgX = (pageWidth - img1.Width) / 2;
                    float imgY = (pageHeight - img1.Height) / 2;

                    // Set the image's position
                    img1.X = imgX;
                    img1.Y = imgY;
                    img1.Transparency = 15;

                    // Add the template to the page
                    SelectPdf.PdfTemplate template1 = doc.AddTemplate(pageRectangle);
                    template1.Add(img1);
                }
                string ReportName = Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ReportNo"]) + ".pdf";
                string Savepath = Server.MapPath("~/ExpeditingDocument");
                doc.Save(Savepath + '\\' + ReportName);
                doc.Close();
                byte[] fileBytes = System.IO.File.ReadAllBytes(Savepath + @"\" + ReportName);

                if (Convert.ToInt32(objModel.IsComfirmation) != 0)
                {

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, ReportName);
                }
                else
                {
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "draft.pdf");


                }

                //return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        //public ActionResult GenerateExpeditingReport(ExpeditingData ExpeditingData) //int PK_call_id, string chartData)
        //{
        //    try
        //    {
        //        SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
        //        string body = string.Empty;
        //        System.Text.StringBuilder strs = new System.Text.StringBuilder();
        //        //var expeditingDataObject = JsonConvert.DeserializeObject<ExpeditingData>(expeditingData);

        //        // Now you can access PK_call_id and chartData properties
        //        var PK_call_id = ExpeditingData.PK_call_id;
        //        var chartData = HttpUtility.UrlDecode(ExpeditingData.chartData);

        //        HtmlDocument htmlDoc = new HtmlDocument();
        //        htmlDoc.LoadHtml(chartData);

        //        // Get the first div element
        //        HtmlNode firstDiv = htmlDoc.DocumentNode.SelectSingleNode("//div");

        //        if (firstDiv != null)
        //        {
        //            // Get the style attribute
        //            HtmlAttribute styleAttribute = firstDiv.Attributes["style"];

        //            // If style attribute exists, append width and height attributes
        //            if (styleAttribute != null)
        //            {
        //                styleAttribute.Value += "; width:650px; height:210px;";
        //            }
        //            else
        //            {
        //                // If no style attribute exists, create a new one with width and height
        //                firstDiv.Attributes.Add("style", "width:650px; height:210px;");
        //            }

        //            // Output the modified HTML
        //            chartData = htmlDoc.DocumentNode.OuterHtml;
        //        }
        //        else
        //        {
        //            Console.WriteLine("No div element found.");
        //        }

        //        using (StreamReader reader = new StreamReader(Server.MapPath("~/QuotationHtml/ExpeditingHtml.html")))
        //        {
        //            body = reader.ReadToEnd();
        //        }

        //        DataTable DSGetRecordFromExpediting = new DataTable();

        //        DSGetRecordFromExpediting = ObjDAL.EditExpeditingReport(PK_call_id).Tables[0];

        //        body = body.Replace("[ExpeditingDate]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Date_Of_Expediting"]));
        //        body = body.Replace("[ProjectName]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Project_Name"]));
        //        body = body.Replace("[VendorName]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["VendorName"]));
        //        body = body.Replace("[ExpeditingLocation]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ExpeditingLocation"]));
        //        body = body.Replace("[CustomerName]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["End_Customer_Name"]));
        //        body = body.Replace("[PONumber]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["PONumber"]));
        //        body = body.Replace("[ExecutingBranch]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Executing_Branch"]));
        //        body = body.Replace("[TUVIPLCustomer]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Customer_Name"]));
        //        body = body.Replace("[SubVendor]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Sub_VendorName"]));
        //        body = body.Replace("[AssignmentNo]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["assignmentNumber"]));
        //        body = body.Replace("[DEC/EPC/PMC]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["DEC_PMC_EPC_Name"]));
        //        body = body.Replace("[SubPONumber]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["SubPo_No"]));
        //        body = body.Replace("[LastExp]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["LastExpeditingtypeandDate"]));
        //        body = body.Replace("[DEC/EPC/PMCAssignNo]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["DEC_PMC_EPC_Assignment_No"]));
        //        body = body.Replace("[ContractualDate]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["Contractual_DeliveryDate"]));
        //        body = body.Replace("[ExpeditingClosureRemark]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ClosureRemarks"]));
        //        body = body.Replace("[IspectorName]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["InspectorName"]));
        //        body = body.Replace("[ReportDate]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ReportDate"]));
        //        string I = "<img src = '" + ConfigurationManager.AppSettings["Web"].ToString() + "/Content/sign/" + (DSGetRecordFromExpediting.Rows[0]["Signature"]) + "' style='width:225px;height:125px; ' align='center'>";
        //        body = body.Replace("[Signature]", I);
        //        body = body.Replace("[GanttChart]", chartData);

        //        //DataTable DSGetRecordFromCall = new DataTable();
        //        //DSGetRecordFromCall = ObjDAL.EditExpeditingReportByCall(PK_call_id).Tables[0];

        //        DataTable dtAllOver = new DataTable();
        //        dtAllOver = ObjDAL.fetdataAllItemwise(Convert.ToString(PK_call_id));
        //        body = body.Replace("[EngExpStart]", Convert.ToString(dtAllOver.Rows[0]["ExpectedStartDate"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[EngExpEnd]", Convert.ToString(dtAllOver.Rows[0]["ExpectedEndDate"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[EngActualStart]", Convert.ToString(dtAllOver.Rows[0]["Actual_Start_Date"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[EngActualEnd]", Convert.ToString(dtAllOver.Rows[0]["Actual_End_Date"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[EngExpProg]", Convert.ToString(dtAllOver.Rows[0]["PercentageWorkDone"]));
        //        body = body.Replace("[EngActualProg]", Convert.ToString(dtAllOver.Rows[0]["Actual_Progress"]));
        //        body = body.Replace("[EngConcernClosure]", Convert.ToString(dtAllOver.Rows[0]["ConcernsStageper"]));
        //        body = body.Replace("[EngConcern]", Convert.ToString(dtAllOver.Rows[0]["TotalConcerns"]));
        //        body = body.Replace("[EngStatus]", Convert.ToString(dtAllOver.Rows[0]["Progress_Status"]));
        //        body = body.Replace("[ProcExpStart]", Convert.ToString(dtAllOver.Rows[1]["ExpectedStartDate"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[ProcExpEnd]", Convert.ToString(dtAllOver.Rows[1]["ExpectedEndDate"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[ProcActualStart]", Convert.ToString(dtAllOver.Rows[1]["Actual_Start_Date"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[ProcActualEnd]", Convert.ToString(dtAllOver.Rows[1]["Actual_End_Date"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[ProcExpProg]", Convert.ToString(dtAllOver.Rows[1]["PercentageWorkDone"]));
        //        body = body.Replace("[ProcActualProg]", Convert.ToString(dtAllOver.Rows[1]["Actual_Progress"]));
        //        body = body.Replace("[ProcConcernClosure]", Convert.ToString(dtAllOver.Rows[1]["ConcernsStageper"]));
        //        body = body.Replace("[ProcConcern]", Convert.ToString(dtAllOver.Rows[1]["TotalConcerns"]));
        //        body = body.Replace("[ProcStatus]", Convert.ToString(dtAllOver.Rows[1]["Progress_Status"]));
        //        body = body.Replace("[ManuExpStart]", Convert.ToString(dtAllOver.Rows[2]["ExpectedStartDate"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[ManuExpEnd]", Convert.ToString(dtAllOver.Rows[2]["ExpectedEndDate"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[ManuActualStart]", Convert.ToString(dtAllOver.Rows[2]["Actual_Start_Date"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[ManuActualEnd]", Convert.ToString(dtAllOver.Rows[2]["Actual_End_Date"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[ManuExpProg]", Convert.ToString(dtAllOver.Rows[2]["PercentageWorkDone"]));
        //        body = body.Replace("[ManuActualProg]", Convert.ToString(dtAllOver.Rows[2]["Actual_Progress"]));
        //        body = body.Replace("[ManuConcernClosure]", Convert.ToString(dtAllOver.Rows[2]["ConcernsStageper"]));
        //        body = body.Replace("[ManuConcern]", Convert.ToString(dtAllOver.Rows[2]["TotalConcerns"]));
        //        body = body.Replace("[ManuStatus]", Convert.ToString(dtAllOver.Rows[2]["Progress_Status"]));
        //        body = body.Replace("[FinalExpStart]", Convert.ToString(dtAllOver.Rows[3]["ExpectedStartDate"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[FinalExpEnd]", Convert.ToString(dtAllOver.Rows[3]["ExpectedEndDate"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[FinalActualStart]", Convert.ToString(dtAllOver.Rows[3]["Actual_Start_Date"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[FinalActualEnd]", Convert.ToString(dtAllOver.Rows[3]["Actual_End_Date"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[FinalExpProg]", Convert.ToString(dtAllOver.Rows[3]["PercentageWorkDone"]));
        //        body = body.Replace("[FinalActualProg]", Convert.ToString(dtAllOver.Rows[3]["Actual_Progress"]));
        //        body = body.Replace("[FinalConcernClosure]", Convert.ToString(dtAllOver.Rows[3]["ConcernsStageper"]));
        //        body = body.Replace("[FinalConcern]", Convert.ToString(dtAllOver.Rows[3]["TotalConcerns"]));
        //        body = body.Replace("[FinalStatus]", Convert.ToString(dtAllOver.Rows[3]["Progress_Status"]));
        //        body = body.Replace("[OverallPlannedStart]", Convert.ToString(dtAllOver.Rows[4]["ExpectedStartDate"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[OverallPlannedEnd]", Convert.ToString(dtAllOver.Rows[4]["ExpectedEndDate"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[OverallActualStart]", Convert.ToString(dtAllOver.Rows[4]["Actual_Start_Date"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[OverallActualEnd]", Convert.ToString(dtAllOver.Rows[4]["Actual_End_Date"]).Replace(" 00:00:00", ""));
        //        body = body.Replace("[OverallPlannedProg]", Convert.ToString(dtAllOver.Rows[4]["PercentageWorkDone"]));
        //        body = body.Replace("[OverallActual]", Convert.ToString(dtAllOver.Rows[4]["Actual_Progress"]));
        //        body = body.Replace("[OverallStatus]", Convert.ToString(dtAllOver.Rows[4]["Progress_Status"]));
        //        body = body.Replace("[OverallProgress]", Convert.ToString(dtAllOver.Rows[4]["Progress_Status"]));
        //        body = body.Replace("[OverallConcernClosure]", Convert.ToString(dtAllOver.Rows[2]["ConcernsStageper"]));
        //        body = body.Replace("[OverallConcern]", Convert.ToString(dtAllOver.Rows[2]["TotalConcerns"]));
        //        string OverallStatus = Convert.ToString(dtAllOver.Rows[4]["Progress_Status"]);

        //        string StatusColor = string.Empty;
        //        if (OverallStatus == "Early") { StatusColor = "#334BDB"; }
        //        else if (OverallStatus == "On Time") { StatusColor = "#A3F333"; }
        //        else if (OverallStatus == "Delay") { StatusColor = "#FFEF33"; }

        //        HtmlDocument htmlDoc1 = new HtmlDocument();
        //        htmlDoc1.LoadHtml(body);
        //        HtmlNode tdStyle = htmlDoc1.DocumentNode.SelectSingleNode("//td[@id='OverallStatus']");
        //        if (tdStyle != null)
        //        {
        //            // Get the style attribute
        //            HtmlAttribute styleAttribute = tdStyle.Attributes["style"];

        //            // If style attribute exists, append width and height attributes
        //            if (styleAttribute != null)
        //            {
        //                styleAttribute.Value += "; background-color:" + StatusColor;
        //            }
        //        }
        //        body = htmlDoc1.DocumentNode.OuterHtml;

        //        DataTable dtAction = new DataTable();

        //        dtAction = ObjDAL.GetActionDataByPK_Call_Id(Convert.ToString(PK_call_id));

        //        body = body.Replace("[ConcernVendor]", Convert.ToString(dtAction.Rows[0]["SubVendor"]));
        //        body = body.Replace("[ConcernCust]", Convert.ToString(dtAction.Rows[0]["Customer"]));
        //        body = body.Replace("[ConcernTUVI]", Convert.ToString(dtAction.Rows[0]["TUVI"]));
        //        body = body.Replace("[EngVendor]", Convert.ToString(dtAction.Rows[1]["SubVendor"]));
        //        body = body.Replace("[EngCust]", Convert.ToString(dtAction.Rows[1]["Customer"]));
        //        body = body.Replace("[EngTUVI]", Convert.ToString(dtAction.Rows[1]["TUVI"]));
        //        body = body.Replace("[ConcernTotal]", Convert.ToString(dtAction.Rows[0]["Total"]));
        //        body = body.Replace("[EngTotal]", Convert.ToString(dtAction.Rows[1]["Total"]));

        //        DataTable dsGetStamp = new DataTable();
        //        dsGetStamp = ObjDAL.GetOpenConcernDataByPK_Call_Id(Convert.ToString(PK_call_id));
        //        string StrOpenConcern = string.Empty;

        //        if (dsGetStamp.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dsGetStamp.Rows)
        //            {
        //                string POItem = AddBreak(Convert.ToString(dr["po_item_number"]));
        //                string Stage = Convert.ToString(dr["Stages"]);
        //                string RaisedOn = Convert.ToString(dr["RaisedOn"]);
        //                string Details = Convert.ToString(dr["Details"]);
        //                string ActionReq = Convert.ToString(dr["ActionReq"]);
        //                string MitigationBy = Convert.ToString(dr["MitigationBy"]);
        //                string ExpectedClosureDate = Convert.ToString(dr["ExpectedClosureDate"]);
        //                string Comment = Convert.ToString(dr["Comment"]);

        //                StrOpenConcern = StrOpenConcern + "<tr><td style=\"width:38.75pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + POItem + "</span></p></td><td style=\"width:60.3pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + Stage + "</span></p></td><td style=\"width:50.35pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + RaisedOn + "</span></p></td><td style=\"width:150.2pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + Details + "</span></p></td><td style=\"width:70.75pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + ActionReq + "</span></p></td><td colspan=\"3\" style=\"width:190.05pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + MitigationBy + "</span></p></td><td style=\"width:70.55pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + ExpectedClosureDate + "</span></p></td><td colspan=\"3\" style=\"width:200.05pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + Comment + "</span></p></td></tr>";
        //                //"<tr><td style=\"width:41.3pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ POItem + "</span></p></td><td colspan=\"2\" style=\"width:58.95pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ Stage + "</span></p></td><td style=\"width:29.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ RaisedOn + "</span></p></td><td style=\"width:30.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ Details + "</span></p></td><td style=\"width:38.8pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ ActionReq + "</span></p></td><td colspan=\"2\" style=\"width:46.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span> "
        //                //+ MitigationBy + "</span></p></td><td style=\"width:40.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ ExpectedClosureDate + "</span></p></td><td colspan=\"3\" style=\"width:124.4pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ Comment + "</span></p></td></tr>";
        //            }
        //            body = body.Replace("[OpenConcern]", StrOpenConcern);
        //        }
        //        else
        //        {
        //            HtmlDocument htmldoc = new HtmlDocument();
        //            htmldoc.LoadHtml(body);

        //            HtmlNode divToRemove = htmldoc.DocumentNode.SelectSingleNode("//div[@id='OpenConcernDiv']");

        //            if (divToRemove != null)
        //            {
        //                divToRemove.Remove();
        //            }

        //            body = htmldoc.DocumentNode.OuterHtml;
        //        }


        //        DataTable CostSheetDashBoard = new DataTable();
        //        string StrPOItem = string.Empty;

        //        CostSheetDashBoard = ObjDAL.GetitemDescription(PK_call_id);

        //        if (CostSheetDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in CostSheetDashBoard.Rows)
        //            {
        //                string PO_Item_Number = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
        //                string ItemCode = Convert.ToString(dr["ItemCode"]);
        //                string ItemDescription = Convert.ToString(dr["ItemDescription"]);
        //                string Unit = Convert.ToString(dr["Unit"]);
        //                string Quantity = Convert.ToString(dr["Quantity"]);
        //                string Contractual_DeliveryDateAsPerPO = Convert.ToString(dr["Contractual_DeliveryDateAsPerPO"]);
        //                string EstimatedDeliveryDate = Convert.ToString(dr["EstimatedDeliveryDate"]);
        //                string progressStatus = Convert.ToString(dr["Progress_Status"]);

        //                StrPOItem = StrPOItem + "<tr style=\"height:20.9pt\"><td style=\"width:42.2pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + PO_Item_Number + "</span></p></td><td style=\"width:58.05pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + ItemCode + "</span></p></td><td colspan=\"5\" style=\"width:120pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + ItemDescription + "</span></p></td><td style=\"width:40.65pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + Unit + "</span></p></td><td style=\"width:10.25pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + Quantity + "</span></p></td><td style=\"width:28.8pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + Contractual_DeliveryDateAsPerPO + "</span></p></td><td style=\"width:28.45pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + EstimatedDeliveryDate + "</span></p></td><td style=\"width:60.55pt; border-style:solid; border-width:0.75pt;  padding-left:2.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + progressStatus + "</span></p></td></tr>";
        //                //"<tr style=\"height:20.9pt\"><td colspan=\"2\" style=\"width:42.2pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ PO_Item_Number + "</span></p></td><td style=\"width:58.05pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ ItemCode + "</span></p></td><td colspan=\"4\" style=\"width:131pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ ItemDescription + "</span></p></td><td style=\"width:35.65pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ Unit + "</span></p></td><td style=\"width:40.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ Quantity + "</span></p></td><td style=\"width:18.8pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ Contractual_DeliveryDateAsPerPO + "</span></p></td><td style=\"width:18.45pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ EstimatedDeliveryDate + "</span></p></td><td style=\"width:65.55pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //+ progressStatus + "</span></p></td></tr>";
        //            }
        //        }

        //        body = body.Replace("[PODetails]", StrPOItem);

        //        DataTable dtEngg = new DataTable();
        //        dtEngg = ObjDAL.GetEngineeringDataByPK_Call_Id(Convert.ToString(PK_call_id));
        //        string StrEngineeringData = string.Empty;
        //        if (dtEngg.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtEngg.Rows)
        //            {
        //                string Item = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
        //                string ReferenceDocument = Convert.ToString(dr["DocumentRefName"]);
        //                string DocNumber = Convert.ToString(dr["Number"]);
        //                string QuantityNumber = Convert.ToString(dr["QuantityNumber"]);
        //                string Status = Convert.ToString(dr["name"]);
        //                string StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]);
        //                string EDD = Convert.ToString(dr["EstimatedEndDate"]);

        //                StrEngineeringData = StrEngineeringData + "<tr><td style=\"width:42.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Item + "</span></p></td><td colspan=\"2\" style=\"width:100.3pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + ReferenceDocument + "</span></p></td><td colspan=\"7\" style=\"width:270.05pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + DocNumber + "</span></p></td><td colspan=\"3\" style=\"width:70.9pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + QuantityNumber + "</span></p></td><td colspan=\"2\" style=\"width:90.35pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Status + "</span></p></td><td style=\"width:80.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + StatusUpdatedOn + "</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + EDD + "</span></p></td></tr>";
        //            }
        //            body = body.Replace("[EngData]", StrEngineeringData);
        //        }

        //        DataTable dtProc = new DataTable();
        //        dtProc = ObjDAL.GetMaterialDataByPK_Call_Id(Convert.ToString(PK_call_id));
        //        string StrProcurement = string.Empty;

        //        if (dtProc.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtProc.Rows)
        //            {
        //                string Item = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
        //                string Description = Convert.ToString(dr["Description"]);
        //                string UOM = Convert.ToString(dr["UnitOM"]);
        //                string Quantity = Convert.ToString(dr["Quantity"]);
        //                string SupplierName = Convert.ToString(dr["OrderPlacedOn"]);
        //                string CDD = Convert.ToString(dr["Contractual_DeliveryDate"]);
        //                string EDD = Convert.ToString(dr["EstimatedDeliveryDate"]);
        //                string Status = Convert.ToString(dr["Stages"]);
        //                string StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]);
        //                string EstimatedEndDate = Convert.ToString(dr["EstimatedEndDate"]);

        //                StrProcurement = StrProcurement + "<tr><td style=\"width:42.75pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + Item + "</span></p></td><td colspan=\"4\" style=\"width:220.3pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + Description + "</span></p></td><td style=\"width:40.05pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + UOM + "</span></p></td><td colspan=\"1\" style=\"width:40.9pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + Quantity + "</span></p></td><td colspan=\"2\" style=\"width:120.35pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + SupplierName + "</span></p></td><td style=\"width:60.25pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + CDD + "</span></p></td><td style=\"width:60.25pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + EDD + "</span></p></td><td style=\"width:55.25pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + Status + "</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + StatusUpdatedOn + "</span></p></td><td style=\"width:50.25pt; border-style:solid; border-width:0.75pt; padding-right:2pt; padding-left:2pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:8pt\"><span>"
        //                    + EstimatedEndDate + "</span></p></td></tr>";
        //                //"<tr><td style=\"width:42.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //            + Item + "</span></p></td><td colspan=\"3\" style=\"width:200.3pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //            + Description + "</span></p></td><td colspan=\"1\" style=\"width:50.05pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //            + UOM + "</span></p></td><td colspan=\"2\" style=\"width:50.9pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //            + Quantity + "</span></p></td><td colspan=\"2\" style=\"width:70.35pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //            + SupplierName + "</span></p><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>Name</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //            + CDD + "</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //            + EDD + "</span></p></td><td style=\"width:55.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //            + Status + " </span></p></td><td style =\"width:80.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //            + StatusUpdatedOn + "</span></p></td><td style=\"width:60.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                //            + EstimatedEndDate + "</span></p></td></tr>";
        //            }
        //            body = body.Replace("[ProcData]", StrProcurement);
        //        }

        //        DataTable dtManu = new DataTable();
        //        dtManu = ObjDAL.GetManufacturingDataByPK_Call_Id(Convert.ToString(PK_call_id));
        //        string StrManufacture = string.Empty;

        //        if (dtManu.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtManu.Rows)
        //            {
        //                string Item = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
        //                string PartName = Convert.ToString(dr["PartName"]);
        //                string Status = Convert.ToString(dr["Stages"]);
        //                string StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]);
        //                string EstimatedEndDate = Convert.ToString(dr["EstimatedEndDate"]);

        //                StrManufacture = StrManufacture + "<tr><td style=\"width:42.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Item + "</span></p></td><td colspan=\"13\" style=\"width:530.2pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + PartName + "</span></p></td><td style=\"width:100pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Status + "</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + StatusUpdatedOn + "</span></p></td><td style=\"width:60.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + EstimatedEndDate + "</span></p></td></tr>";
        //            }
        //            body = body.Replace("[ManuData]", StrManufacture);
        //        }

        //        DataTable dtFinal = new DataTable();
        //        dtFinal = ObjDAL.GetFinalStageByPK_Call_Id(Convert.ToString(PK_call_id));
        //        string StrFinal = string.Empty;

        //        if (dtFinal.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtFinal.Rows)
        //            {
        //                string Item = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
        //                string Stage = Convert.ToString(dr["Stage"]);
        //                string Status = Convert.ToString(dr["Stages"]);
        //                string StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]);
        //                string EstimatedEndDate = Convert.ToString(dr["EstimatedEndDate"]);

        //                StrFinal = StrFinal + "<tr><td style=\"width:42.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Item + "</span></p></td><td colspan=\"13\" style=\"width:530.2pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Stage + "</span></p></td><td style=\"width:100pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Status + "</span></p></td><td style=\"width:70.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + StatusUpdatedOn + "</span></p></td><td style=\"width:60.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + EstimatedEndDate + "</span></p></td></tr>";
        //            }
        //            body = body.Replace("[FinalData]", StrFinal);
        //        }

        //        DataTable dtPayment = new DataTable();
        //        dtPayment = ObjDAL.EXpPayment(Convert.ToString(PK_call_id));
        //        string StrPayment = string.Empty;

        //        if (dtPayment.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtPayment.Rows)
        //            {
        //                string Documents = Convert.ToString(dr["Documents"]);
        //                string Date = Convert.ToString(dr["PaymentDate"]);
        //                string Status = Convert.ToString(dr["Stages"]);
        //                string Remarks = Convert.ToString(dr["Remarks"]);

        //                StrPayment = StrPayment + "<tr><td colspan=\"7\" style=\"width:260pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Documents + "</span></p></td><td colspan=\"1\" style=\"width:75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Date + "</span></p></td><td colspan=\"2\" style=\"width:160pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Status + "</span></p></td><td colspan=\"6\" style=\"width:320pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Remarks + "</span></p></td></tr>";
        //            }

        //            body = body.Replace("[CommercialData]", StrPayment);
        //        }

        //        DataTable dtAttendee = new DataTable();
        //        dtAttendee = ObjDAL.EditExpeditingReportAttendeesName(PK_call_id);
        //        string StrAttendee = string.Empty;

        //        if (dtAttendee.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtAttendee.Rows)
        //            {
        //                string Representing = Convert.ToString(dr["Represting"]);
        //                string Name = Convert.ToString(dr["Name"]);
        //                string Email = Convert.ToString(dr["EmailID"]);
        //                string Designation = Convert.ToString(dr["Designation"]);

        //                StrAttendee = StrAttendee + "<tr><td colspan=\"8\" style=\"width:160pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Representing + "</span></p></td><td colspan=\"2\" style=\"width:190pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Name + "</span></p></td><td colspan=\"2\" style=\"width:165pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Designation + "</span></p></td><td colspan=\"4\" style=\"width:300pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Email + "</span></p></td></tr>";
        //            }
        //            body = body.Replace("[AttendeeData]", StrAttendee);
        //        }

        //        DataTable DSdata = new DataTable();
        //        DSdata = ObjDAL.GetdistributionId(PK_call_id);
        //        string strDistribution = string.Empty;

        //        if (DSdata.Rows.Count > 0)
        //        {
        //            Boolean Customer = Convert.ToBoolean(DSdata.Rows[0]["DCustomer"]);
        //            string Cust = (Customer == true) ? "checked=\"checked\"" : "";
        //            Boolean Vendor = Convert.ToBoolean(DSdata.Rows[0]["DVendor"]);
        //            string Vend = (Vendor == true) ? "checked=\"checked\"" : "";
        //            Boolean TUVI = Convert.ToBoolean(DSdata.Rows[0]["DTuvi"]);
        //            string TUV = (TUVI == true) ? "checked=\"checked\"" : "";

        //            strDistribution = strDistribution + "<td colspan=\"6\" style=\"padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><input type=\"checkbox\""
        //                + Cust + "\"><label style=\"font-size:9pt\">Customer / End User</label></td><td colspan=\"6\" style=\"padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><input type=\"checkbox\""
        //                + Vend + "\"><label style=\"font-size:9pt\">Vendor/ Sub Vendor</label></td><td colspan=\"6\" style=\"border-right: 0.75pt solid black;padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><input type=\"checkbox\""
        //                + TUV + "\"><label style=\"font-size:9pt\">TUVI Executing / Originationg Branch</label></td>";

        //        }
        //        body = body.Replace("[DistributionData]", strDistribution);

        //        DataTable dtObs = new DataTable();
        //        dtObs = ObjDAL.GetObservationDataByPK_Call_Id(Convert.ToString(PK_call_id));
        //        string StrObservationData = string.Empty;
        //        if (dtObs.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtObs.Rows)
        //            {
        //                string Stages = Convert.ToString(dr["ObsStages"]);

        //                if (Stages == "Engineering") { body = body.Replace("[EngObservation]", Convert.ToString(dr["Findings"])); }
        //                else if (Stages == "Procurement") { body = body.Replace("[ProcObservation]", Convert.ToString(dr["Findings"])); }
        //                else if (Stages == "Manufacturing") { body = body.Replace("[ManuObservation]", Convert.ToString(dr["Findings"])); }
        //                else if (Stages == "Final") { body = body.Replace("[FinalObservation]", Convert.ToString(dr["Findings"])); }

        //                //body = body.Replace("[ExpeditingClosureRemark]", Convert.ToString(dtAction.Rows[1]["Customer"]));
        //            }
        //            body = body.Replace("[EngObservation]", "");
        //            body = body.Replace("[ProcObservation]", "");
        //            body = body.Replace("[ManuObservation]", "");
        //            body = body.Replace("[FinalObservation]", "");
        //        }

        //        int count = 0;
        //        #region report Count
        //        DSdata = objDalVisitReport.GetReportByCall_Id(PK_call_id);
        //        if (DSdata.Rows.Count > 0)
        //        {
        //            int counts = DSdata.Rows.Count;
        //            count = counts - 1;
        //        }
        //        string countNo = Convert.ToString(count);
        //        #endregion

        //        #region Report Image data
        //        DataTable ImageReportDashBoard = new DataTable();
        //        List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();
        //        //GetReportImageByCall_Id
        //        ImageReportDashBoard = ObjDAL.GetReportImageByCall_Id(Convert.ToString(PK_call_id));
        //        if (ImageReportDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in ImageReportDashBoard.Rows)
        //            {
        //                ImageDashBoard.Add(
        //                    new ReportImageModel
        //                    {
        //                        Image = Convert.ToString(dr["Image"]),
        //                        Heading = Convert.ToString(dr["Heading"]),
        //                    }
        //                    );
        //            }
        //        }
        //        #endregion

        //        #region Image Save to pdf
        //        SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
        //        System.Text.StringBuilder strss = new System.Text.StringBuilder();

        //        string bodys = string.Empty;
        //        string ImageContent = string.Empty;
        //        string ReportNames = string.Empty;
        //        string paths = string.Empty;
        //        int img = 0;
        //        int imagecount = ImageReportDashBoard.Rows.Count;
        //        int rows = imagecount / 3;
        //        int imageposted = 0;
        //        int reminder = (imagecount % 3);
        //        int iteration = 1;
        //        if (imagecount > 0 && imagecount < 3)
        //        {
        //            rows = 1;
        //        }

        //        ///First File start
        //        PdfPageSize pageSizes = PdfPageSize.A4;
        //        PdfPageOrientation pdfOrientations = PdfPageOrientation.Landscape;

        //        HtmlToPdf converters = new HtmlToPdf();

        //        #endregion

        //        #region Vaibhav If Images count less than 6

        //        if (imagecount <= 6)
        //        {
        //            for (int ic = 0; ic < rows; ic++)
        //            {
        //                if (imageposted > 0)
        //                {
        //                    if ((imageposted % 6) == 0)
        //                    {

        //                        body = body.Replace("[ExpeditingImage]", ImageContent);

        //                        ImageContent = string.Empty;
        //                        iteration = iteration + 1;
        //                        ViewBag.Reminder = "1";
        //                    }
        //                }

        //                ImageContent += "<tr><td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img]["Heading"].ToString() + "</td>";
        //                ImageContent += "<td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img + 1]["Heading"].ToString() + "</td>";
        //                ImageContent += "<td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img + 2]["Heading"].ToString() + "</td></tr>";

        //                ImageContent += "<tr><td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td>";
        //                ImageContent += "<td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img + 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td>";
        //                ImageContent += "<td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img + 2]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";

        //                img = img + 3;
        //                imageposted = imageposted + 3;
        //            }



        //            #region Reminder = 1

        //            if (reminder == 1)
        //            {
        //                if (ImageContent != string.Empty)
        //                {
        //                    ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding: 10px; width: 33 %;\" align=\"center\">" + ImageReportDashBoard.Rows[imagecount - 1]["Heading"].ToString() + "</td></tr>";
        //                    ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding:10px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[imagecount - 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";
        //                }
        //                else
        //                {
        //                    ImageContent = "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding: 10px; width: 33 %;\" align=\"center\">" + ImageReportDashBoard.Rows[imagecount - 1]["Heading"].ToString() + "</td></tr>";
        //                    ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding:10px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[imagecount - 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";
        //                }

        //                body = body.Replace("[ExpeditingImage]", ImageContent);


        //            }

        //            #region 4 Image If not reminder =0
        //            if (imageposted <= imagecount)
        //            {
        //                if (reminder == 0)
        //                {
        //                    ViewBag.Reminder = "2";
        //                    body = body.Replace("[ExpeditingImage]", ImageContent);

        //                    ImageContent = string.Empty;
        //                    iteration = iteration + 1;
        //                }
        //            }

        //            #endregion

        //        }
        //        #endregion
        //        #endregion

        //        #region Vaibhav If Images count more than 6
        //        else
        //        {
        //            for (int ic = 0; ic < rows; ic++)
        //            {

        //                ImageContent += "<tr><td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img]["Heading"].ToString() + "</td>";
        //                ImageContent += "<td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img + 1]["Heading"].ToString() + "</td>";
        //                ImageContent += "<td style=\"padding: 10px; width: 33 %;border:1px solid #000000;border-top-width: 0px;\" align=\"center\">" + ImageReportDashBoard.Rows[img + 2]["Heading"].ToString() + "</td></tr>";

        //                ImageContent += "<tr><td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td>";
        //                ImageContent += "<td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img + 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td>";
        //                ImageContent += "<td style=\"padding:10px;border:1px solid #000000;border-top-width: 0px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[img + 2]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";

        //                img = img + 3;
        //                imageposted = imageposted + 3;
        //            }

        //            ViewBag.Reminder = "2";

        //            #region  reminder
        //            if (reminder == 1)
        //            {

        //                if (ImageContent != string.Empty)
        //                {

        //                    ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding: 10px; width: 33 %;\" align=\"center\">" + ImageReportDashBoard.Rows[imagecount - 1]["Heading"].ToString() + "</td></tr>";
        //                    ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding:10px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[imagecount - 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";

        //                }
        //                else
        //                {
        //                    ImageContent = "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding: 10px; width: 33 %;\" align=\"center\">" + ImageReportDashBoard.Rows[imagecount - 1]["Heading"].ToString() + "</td></tr>";
        //                    ImageContent += "<tr><td style=\"border:1px solid #000000;border-top-width: 0px;padding:10px;\" align=\"center\"><img src=\"" + ConfigurationManager.AppSettings["Web"].ToString() + "/CompressFiles/" + ImageReportDashBoard.Rows[imagecount - 1]["Image"].ToString() + "\" style=\"width:280px;height:205px; \" align=\"center\" alt=\"\"></td></tr>";
        //                }

        //                body = body.Replace("[ExpeditingImage]", ImageContent);

        //                #region Initial setting vaibhav
        //                //strs.Append(body);
        //                PdfPageSize vpageSize = PdfPageSize.A4;
        //                PdfPageOrientation vpdfOrientation = PdfPageOrientation.Landscape;
        //                HtmlToPdf Vconverter = new HtmlToPdf();

        //                // set the page timeout (in seconds)
        //                Vconverter.Options.MaxPageLoadTime = 240;  //=========================5-Aug-2019
        //                Vconverter.Options.PdfPageSize = vpageSize;
        //                Vconverter.Options.PdfPageOrientation = vpdfOrientation;
        //                #endregion

        //                #region Header and Footer Vaibhav
        //                #region Heder code
        //                string _VHeader = string.Empty;
        //                string _Vfooter = string.Empty;

        //                // for Report header by abel
        //                StreamReader _VreadHeader_File = new StreamReader(Server.MapPath("~/QuotationHtml/Expediting-header.html"));
        //                _VHeader = _VreadHeader_File.ReadToEnd();
        //                _VHeader = _VHeader.Replace("[logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.svg");
        //                _VHeader = _VHeader.Replace("[ExpeditingType]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ExpeditingType"]));
        //                _VHeader = _VHeader.Replace("[ReportNumber]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ReportNo"]));

        //                #endregion

        //                #region Footer Code

        //                StreamReader _VreadFooter_File = new StreamReader(Server.MapPath("~/QuotationHtml/Expediting-footer.html"));
        //                _Vfooter = _VreadFooter_File.ReadToEnd();
        //                _Vfooter = _Vfooter.Replace("[LogoFooter]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/FTUEV-NORD-GROUP_Logo_Electric-Blue.svg");

        //                // header settings
        //                Vconverter.Options.DisplayHeader = true || true || true;
        //                Vconverter.Header.DisplayOnFirstPage = true;
        //                Vconverter.Header.DisplayOnOddPages = true;
        //                Vconverter.Header.DisplayOnEvenPages = true;
        //                Vconverter.Header.Height = 75;

        //                PdfHtmlSection VheaderHtml = new PdfHtmlSection(_VHeader, string.Empty);
        //                VheaderHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //                Vconverter.Header.Add(VheaderHtml);

        //                // footer settings
        //                Vconverter.Options.DisplayFooter = true || true || true;
        //                Vconverter.Footer.DisplayOnFirstPage = true;
        //                Vconverter.Footer.DisplayOnOddPages = true;
        //                Vconverter.Footer.DisplayOnEvenPages = true;

        //                //Vconverter.Footer.Height = 150;
        //                Vconverter.Footer.Height = 105;
        //                //converter.Footer.Height = 120;

        //                PdfHtmlSection VfooterHtml = new PdfHtmlSection(_Vfooter, string.Empty);
        //                VfooterHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //                Vconverter.Footer.Add(VfooterHtml);
        //                #endregion
        //                #endregion

        //                SelectPdf.PdfDocument docs = Vconverter.ConvertHtmlString(bodys);
        //                ReportNames = Convert.ToString(PK_call_id) + "_Img_" + iteration + ".pdf";

        //                paths = Server.MapPath("~/Content/" + Convert.ToString(PK_call_id));
        //                docs.Save(paths + '\\' + ReportNames);
        //                docs.Close();
        //                bodys = string.Empty;
        //                ImageContent = string.Empty;
        //                ViewBag.Reminder = "1";
        //            }
        //            #endregion


        //            #region If ImageContent not null

        //            if (ViewBag.Reminder != "1")
        //            {
        //                ViewBag.Reminder = "2";
        //                body = body.Replace("[ExpeditingImage]", ImageContent);

        //            }
        //            #endregion

        //        }

        //        #endregion

        //        //PdfPageSize pageSize = PdfPageSize.A4;
        //        //PdfPageOrientation pdfOrientation = PdfPageOrientation.Landscape;
        //        HtmlToPdf converter = new HtmlToPdf();

        //        SelectPdf.PdfDocument idoc = converter.ConvertHtmlString(body);
        //        int ImagePageCount = idoc.Pages.Count;

        //        DataTable dtProgress = new DataTable();

        //        dtProgress = ObjDAL.EXpDalayWise(Convert.ToString(PK_call_id));
        //        string StrProgress = string.Empty;
        //        if (dtProgress.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtProgress.Rows)
        //            {
        //                string item = AddBreak(Convert.ToString(dr["PO_Item_Number"]));
        //                string PoitemCode = Convert.ToString(dr["ItemCode"]);
        //                string ExpstartDate = Convert.ToString(dr["exprctedstartDate"]);
        //                string ExpEndDate = Convert.ToString(dr["ExpectedEndDate"]);
        //                string ActstartDate = Convert.ToString(dr["Actual_Start_Date"]);
        //                string ActEndDate = Convert.ToString(dr["Actual_End_Date"]);
        //                string Concerns = Convert.ToString(dr["TotalConcerns"]);
        //                string engDelay = Convert.ToString(dr["DelayEng"]);
        //                string ProDelay = Convert.ToString(dr["DelayProcurement"]);
        //                string manuDelay = Convert.ToString(dr["DelayManufacturing"]);
        //                string FinalDelay = Convert.ToString(dr["DelayFinal"]);
        //                string engExp = Convert.ToString(dr["ExceptedEng"]);
        //                string ProExp = Convert.ToString(dr["ExceptedProcurement"]);
        //                string manuExp = Convert.ToString(dr["ExceptedManufacturing"]);
        //                string FinalExp = Convert.ToString(dr["ExceptedFinal"]);
        //                string engAct = Convert.ToString(dr["ActEng"]);
        //                string ProAct = Convert.ToString(dr["ActProcurement"]);
        //                string manuAct = Convert.ToString(dr["ActManufacturing"]);
        //                string FinalAct = Convert.ToString(dr["ActFinal"]);
        //                string actualprogress = Convert.ToString(dr["actualprogress"]);
        //                string progressStatus = Convert.ToString(dr["Progress_Status"]);

        //                StrProgress = StrProgress + "<tr><td  style=\"width:30pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + item + "</span></p></td><td  style=\"width:40pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + PoitemCode + "</span></p></td><td  style=\"width:40pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + ExpstartDate + "</span></p></td><td  style=\"width:40pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + ExpEndDate + "</span></p></td><td  style=\"width:40pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + ActstartDate + "</span></p></td><td  style=\"width:40pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + ActEndDate + "</span></p></td><td  style=\"width:40.65pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + Concerns + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + engDelay + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + ProDelay + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + manuDelay + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + FinalDelay + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + engExp + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + ProExp + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + manuExp + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + FinalExp + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + engAct + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + ProAct + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + manuAct + "</span></p></td><td style=\"width:14pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + FinalAct + "</span></p></td><td  style=\"width:20.65pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + actualprogress + "</span></p></td><td  style=\"width:20.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                + progressStatus + "</span></p></td></tr>";
        //            }
        //            body = body.Replace("[ItemWiseProgressData]", StrProgress);
        //        }

        //        DataTable dtClosedConcern = new DataTable();
        //        dtClosedConcern = ObjDAL.GetClosedConcernDataByPK_Call_Id(Convert.ToString(PK_call_id));
        //        string StrClosedConcern = string.Empty;

        //        if (dtClosedConcern.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtClosedConcern.Rows)
        //            {
        //                string POItem = Convert.ToString(dr["po_item_number"]);
        //                string Stage = Convert.ToString(dr["Stages"]);
        //                string RaisedOn = Convert.ToString(dr["RaisedOn"]);
        //                string Details = Convert.ToString(dr["Details"]);
        //                string ActionReq = Convert.ToString(dr["ActionReq"]);
        //                string MitigationBy = Convert.ToString(dr["MitigationBy"]);
        //                string ExpectedClosureDate = Convert.ToString(dr["ExpectedClosureDate"]);
        //                string Comment = Convert.ToString(dr["Comment"]);

        //                StrClosedConcern = StrClosedConcern + "<tr><td style=\"width:41.3pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;word-wrap:break-word;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + POItem + "</span></p></td><td colspan=\"2\" style=\"width:58.95pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Stage + "</span></p></td><td style=\"width:29.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + RaisedOn + "</span></p></td><td style=\"width:30.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Details + "</span></p></td><td style=\"width:38.8pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + ActionReq + "</span></p></td><td colspan=\"2\" style=\"width:46.75pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span> "
        //                    + MitigationBy + "</span></p></td><td style=\"width:40.25pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + ExpectedClosureDate + "</span></p></td><td colspan=\"3\" style=\"width:124.4pt; border-style:solid; border-width:0.75pt; padding-right:5.03pt; padding-left:5.03pt; vertical-align:top;\"><p style=\"margin-top:0pt; margin-bottom:0pt; font-size:9pt\"><span>"
        //                    + Comment + "</span></p></td></tr>";
        //            }
        //            body = body.Replace("[ClosedConcern]", StrClosedConcern);
        //        }
        //        else
        //        {
        //            HtmlDocument htmldoc = new HtmlDocument();
        //            htmldoc.LoadHtml(body);

        //            HtmlNode divToRemove = htmldoc.DocumentNode.SelectSingleNode("//div[@id='ClosedConcernDiv']");

        //            if (divToRemove != null)
        //            {
        //                divToRemove.Remove();
        //            }

        //            body = htmldoc.DocumentNode.OuterHtml;
        //        }


        //        PdfPageSize pageSize = PdfPageSize.A4;
        //        PdfPageOrientation pdfOrientation = PdfPageOrientation.Landscape;
        //        //HtmlToPdf converter = new HtmlToPdf();

        //        SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(body);
        //        int PageCount = doc1.Pages.Count;
        //        body = body.Replace("[PageCount]", "(Refer Page " + Convert.ToString(PageCount) + " Of " + Convert.ToString(PageCount) + " )");
        //        strs.Append(body);

        //        converter.Options.MaxPageLoadTime = 240;
        //        converter.Options.PdfPageSize = pageSize;
        //        converter.Options.PdfPageOrientation = pdfOrientation;

        //        string _Header = string.Empty;
        //        string _footer = string.Empty;

        //        StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/QuotationHtml/Expediting-header.html"));
        //        _Header = _readHeader_File.ReadToEnd();
        //        _Header = _Header.Replace("[logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.svg");
        //        _Header = _Header.Replace("[ExpeditingType]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ExpeditingType"]));
        //        _Header = _Header.Replace("[ReportNumber]", Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ReportNo"]));

        //        StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/QuotationHtml/Expediting-footer.html"));
        //        _footer = _readFooter_File.ReadToEnd();
        //        _footer = _footer.Replace("[LogoFooter]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/FTUEV-NORD-GROUP_Logo_Electric-Blue.svg");

        //        // header settings
        //        converter.Options.DisplayHeader = true ||
        //            true || true;
        //        converter.Header.DisplayOnFirstPage = true;
        //        converter.Header.DisplayOnOddPages = true;
        //        converter.Header.DisplayOnEvenPages = true;
        //        converter.Header.Height = 85;

        //        PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
        //        headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;

        //        converter.Header.Add(headerHtml);

        //        // footer settings
        //        converter.Options.DisplayFooter = true || true || true;
        //        converter.Footer.DisplayOnFirstPage = true;
        //        converter.Footer.DisplayOnOddPages = true;
        //        converter.Footer.DisplayOnEvenPages = true;
        //        converter.Footer.Height = 100;

        //        PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
        //        footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //        converter.Footer.Add(footerHtml);

        //        PdfTextSection text1 = new PdfTextSection(65, 73, "Page: {page_number} of {total_pages}", new System.Drawing.Font("TNG Pro", 8, FontStyle.Bold));

        //        converter.Footer.Add(text1);

        //        converter.Options.SecurityOptions.CanAssembleDocument = true;
        //        converter.Options.SecurityOptions.CanCopyContent = true;
        //        converter.Options.SecurityOptions.CanEditAnnotations = true;
        //        converter.Options.SecurityOptions.CanEditContent = true;
        //        converter.Options.SecurityOptions.CanFillFormFields = true;
        //        converter.Options.SecurityOptions.CanPrint = true;

        //        //PdfDocument doc = converter.ConvertHtmlString(body);
        //        PdfDocument doc = new PdfDocument();
        //        converter.Options.PdfCompressionLevel = PdfCompressionLevel.Best;
        //        converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
        //        converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;

        //        doc1 = converter.ConvertHtmlString(body);
        //        int PageNo = doc1.Pages.Count;

        //        int pages = imagecount / 6;
        //        int remainder = imagecount % 6;
        //        if (remainder > 0) { remainder = 1; } else { remainder = 0; }
        //        string TotalImagePages = string.Empty;
        //        int PhotoPageNo = (PageNo - (pages + remainder)) + 1;

        //        for (int c = PhotoPageNo; c <= PageNo; c++)
        //        {
        //            // Check the condition involving a, b, and c
        //            if (c >= PhotoPageNo && c <= PageNo)
        //            {
        //                // Append the valid value of c to the string
        //                TotalImagePages += c + ",";
        //            }
        //        }

        //        if (TotalImagePages == "")
        //        {
        //            TotalImagePages = "";
        //        }
        //        else
        //        {
        //            TotalImagePages = TotalImagePages.Remove(TotalImagePages.Length - 1);
        //        }
        //        // TotalImagePages = TotalImagePages.Remove(TotalImagePages.Length - 1);

        //        body = body.Replace("[PhotoPageNo]", TotalImagePages);


        //        // Now you have the total number of pages
        //        // Convert HTML string to PDF document
        //        doc = converter.ConvertHtmlString(body);

        //        string ReportName = Convert.ToString(DSGetRecordFromExpediting.Rows[0]["ReportNo"]) + ".pdf";
        //        string Savepath = Server.MapPath("~/ExpeditingDocument");
        //        doc.Save(Savepath + '\\' + ReportName);
        //        doc.Close();
        //        byte[] fileBytes = System.IO.File.ReadAllBytes(Savepath + @"\" + ReportName);


        //        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, ReportName);
        //        //return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }

        //}


        public string AddBreak(string input)
        {
            string[] parts = input.Split(',');
            string Retinput = "";
            for (int i = 0; i < parts.Length; i++)
            {
                if (i > 0 && i % 3 == 0) // Add <br/> after every 5th comma
                {
                    Retinput = Retinput + parts[i] + ",<br/>";
                }

                //Console.Write(parts[i]);

                // Add a comma if it's not the last element
                if ((i < parts.Length - 1) && parts.Length > 1)
                {
                    Retinput = Retinput + parts[i] + ",";
                }
                else
                {
                    Retinput = Retinput + parts[i];
                }

            }
            return Retinput;
        }

        [HttpGet]
        public ActionResult CreateGanttChart(string PK_Call_Id)
        {
            try
            {
                List<Progress> lstProgress = new List<Progress>();
                if (PK_Call_Id != null)
                {
                    DataTable dtProgress = new DataTable();

                    dtProgress = ObjDAL.fetdataAllItemwise(PK_Call_Id);
                    if (dtProgress.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProgress.Rows)
                        {
                            lstProgress.Add(new Progress
                            {
                                Stages = Convert.ToString(dr["stage"]),
                                ExpectedStartDate = Convert.ToString(dr["ExpectedStartDate"]),
                                ExpectedEndDate = Convert.ToString(dr["ExpectedEndDate"]),
                                Actual_Start_Date = Convert.ToString(dr["Actual_Start_Date"]),
                                Actual_End_Date = Convert.ToString(dr["Actual_End_Date"]),
                                Expected_Progressper = Convert.ToString(dr["PercentageWorkDone"]),
                                Actual_Progressper = Convert.ToString(dr["Actual_Progress"]),
                                delaydays = Convert.ToString(dr["delaydays"]),
                                ConcernClosureper = Convert.ToString(dr["ConcernsStageper"]),
                                Concern = Convert.ToString(dr["TotalConcerns"]),
                                ProgressStatus = Convert.ToString(dr["Progress_Status"]),
                                ActionValue = Convert.ToString(dr["actionValue"])
                            });
                        }
                    }
                }

                string CostSheetJson = Newtonsoft.Json.JsonConvert.SerializeObject(lstProgress);
                return Json(CostSheetJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [HttpPost]
        public ActionResult deleteengData(string varPK_Engi_Id)
        {
            var result = ObjDAL.DeleteEngineeringIfPresent(varPK_Engi_Id);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult deleteMaterialData(string varPK_Material_Id)
        {
            var result = ObjDAL.DeleteMaterialIfPresent(varPK_Material_Id);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult deleteManuData(string varPK_Manufacturing_Id)
        {
            var result = ObjDAL.DeleteManufacturingIfPresent(varPK_Manufacturing_Id);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult deleteFinalData(string varCPK_FinalStage_Id)
        {
            var result = ObjDAL.DeleteFinalStage(varCPK_FinalStage_Id);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult deleteConcData(string varCPK_Concern_Id)
        {
            var result = ObjDAL.DeleteConcern(varCPK_Concern_Id);

            return Json(new { success = true });
        }


        #region Expediting Dashboard

        public ActionResult ExpeditingDashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FillCompany(string Field)
        {
            DataTable DtList = new DataTable();
            DtList = ObjDAL.FillCompany(Field);
            string lstDropdown = ObjDAL.ConvertDataTabletoString(DtList);
            //List<Dropdown> lstDropdown = new List<Dropdown>();           

            //if (DtList.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in DtList.Rows)
            //    {
            //        lstDropdown.Add(
            //            new Dropdown
            //            {
            //                ID = Convert.ToString(dr["ID"]),
            //                Value = Convert.ToString(dr["Value"])
            //            }
            //            );
            //    }
            //}
            return Json(lstDropdown, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FillALLDropdown(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {
            DataSet DtList = new DataSet();
            DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "1", TagNumber, itemName);

            string Projlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            // string Vendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[1]);
            string SubVendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[1]);
            string POlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[2]);
            string Itemlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[3]);
            //string Callscount = ObjDAL.ConvertDataTabletoString(DtList.Tables[4]);
            string Onsite = DtList.Tables[4].Rows[0]["Onsite"].ToString();
            string Desk = DtList.Tables[4].Rows[0]["Desk"].ToString();
            string TotalVisit = DtList.Tables[4].Rows[0]["TotalVisit"].ToString();
            string gantchart = ObjDAL.ConvertDataTabletoString(DtList.Tables[5]);
            string Actual_Progressper = DtList.Tables[6].Rows[0]["Actual_Progressper"].ToString();
            string Expected_Progressper = DtList.Tables[6].Rows[0]["Expected_Progressper"].ToString();
            string overAll = DtList.Tables[6].Rows[0]["Progress_Status"].ToString();
            string actionValuelst = ObjDAL.ConvertDataTabletoString(DtList.Tables[8]);

            string Openconcern = DtList.Tables[7].Rows[0]["Openconcern"].ToString();
            string closeconcerns = DtList.Tables[7].Rows[0]["closeconcerns"].ToString();
            string vendorCount = DtList.Tables[9].Rows[0]["TotalVendor"].ToString();

            string Concernslit = ObjDAL.ConvertDataTabletoString(DtList.Tables[10]);
            string gantchart1 = ObjDAL.ConvertDataTabletoString(DtList.Tables[11]);

            return Json(new { Projlst = Projlst, SubVendorlst = SubVendorlst, POlst = POlst, Itemlst = Itemlst, Onsite = Onsite, Desk = Desk, gantchart = gantchart, Actual_Progressper = Actual_Progressper, Expected_Progressper = Expected_Progressper, overAll = overAll, Openconcern = Openconcern, closeconcerns = closeconcerns, actionValuelst = actionValuelst, vendorCount = vendorCount, Concernslit = Concernslit, gantchart1 = gantchart1, TotalVisit = TotalVisit }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FillOnProjectChange(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {
            DataSet DtList = new DataSet();
            DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "2", TagNumber, itemName);

            //string Vendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string SubVendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string POlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[1]);

            string Onsite = DtList.Tables[2].Rows[0]["Onsite"].ToString();
            string Desk = DtList.Tables[2].Rows[0]["Desk"].ToString();
            string TotalVisit = DtList.Tables[2].Rows[0]["TotalVisit"].ToString();
            string gantchart = ObjDAL.ConvertDataTabletoString(DtList.Tables[3]);
            string Actual_Progressper = DtList.Tables[4].Rows[0]["Actual_Progressper"].ToString();
            string Expected_Progressper = DtList.Tables[4].Rows[0]["Expected_Progressper"].ToString();
            string overAll = DtList.Tables[4].Rows[0]["Progress_Status"].ToString();

            string Openconcern = DtList.Tables[5].Rows[0]["Openconcern"].ToString();
            string closeconcerns = DtList.Tables[5].Rows[0]["closeconcerns"].ToString();
            string actionValuelst = ObjDAL.ConvertDataTabletoString(DtList.Tables[6]);

            string vendorCount = DtList.Tables[7].Rows[0]["TotalVendor"].ToString();
            string Concernslit = ObjDAL.ConvertDataTabletoString(DtList.Tables[8]);

            string gantchart1 = ObjDAL.ConvertDataTabletoString(DtList.Tables[9]);

            string VendorPoNumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[10]);

            string PoNumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[11]);
            string itemtagNumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[12]);
            string ItemName = ObjDAL.ConvertDataTabletoString(DtList.Tables[13]);



            return Json(new { SubVendorlst = SubVendorlst, POlst = POlst, Onsite = Onsite, Desk = Desk, gantchart = gantchart, Actual_Progressper = Actual_Progressper, Expected_Progressper = Expected_Progressper, overAll = overAll, Openconcern = Openconcern, closeconcerns = closeconcerns, actionValuelst = actionValuelst, vendorCount = vendorCount, Concernslit = Concernslit, gantchart1 = gantchart1, TotalVisit = TotalVisit, VendorPoNumber = VendorPoNumber, PoNumber = PoNumber, itemtagNumber = itemtagNumber, ItemName = ItemName }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FillOnVendorChange(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {
            DataSet DtList = new DataSet();
            DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "3", TagNumber, itemName);

            //string Projlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            //string SubVendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[1]);
            string POlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string Onsite = DtList.Tables[1].Rows[0]["Onsite"].ToString();
            string Desk = DtList.Tables[1].Rows[0]["Desk"].ToString();
            string TotalVisit = DtList.Tables[1].Rows[0]["TotalVisit"].ToString();
            string gantchart = ObjDAL.ConvertDataTabletoString(DtList.Tables[2]);
            string Actual_Progressper = DtList.Tables[3].Rows[0]["Actual_Progressper"].ToString();
            string Expected_Progressper = DtList.Tables[3].Rows[0]["Expected_Progressper"].ToString();
            string overAll = DtList.Tables[3].Rows[0]["Progress_Status"].ToString();

            string Openconcern = DtList.Tables[4].Rows[0]["Openconcern"].ToString();
            string closeconcerns = DtList.Tables[4].Rows[0]["closeconcerns"].ToString();
            string actionValuelst = ObjDAL.ConvertDataTabletoString(DtList.Tables[5]);

            string vendorCount = DtList.Tables[6].Rows[0]["TotalVendor"].ToString();
            string Concernslit = ObjDAL.ConvertDataTabletoString(DtList.Tables[7]);
            string gantchart1 = ObjDAL.ConvertDataTabletoString(DtList.Tables[8]);

            string PoNumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[9]);
            string itemtagNumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[10]);



            return Json(new { POlst = POlst, Onsite = Onsite, Desk = Desk, gantchart = gantchart, Actual_Progressper = Actual_Progressper, Expected_Progressper = Expected_Progressper, overAll = overAll, Openconcern = Openconcern, closeconcerns = closeconcerns, actionValuelst = actionValuelst, vendorCount = vendorCount, Concernslit = Concernslit, gantchart1 = gantchart1, TotalVisit = TotalVisit, PoNumber = PoNumber, itemtagNumber = itemtagNumber }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FillOnSubVendorChange(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {
            DataSet DtList = new DataSet();
            DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "4", TagNumber, itemName);

            //string Projlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            //string Vendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[1]);
            string POlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string Onsite = DtList.Tables[1].Rows[0]["Onsite"].ToString();
            string Desk = DtList.Tables[1].Rows[0]["Desk"].ToString();
            string TotalVisit = DtList.Tables[1].Rows[0]["TotalVisit"].ToString();

            string PoNumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[2]);
            string itemtagNumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[3]);

            string selectedponumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[4]);

            string selectedpoitem = ObjDAL.ConvertDataTabletoString(DtList.Tables[5]);

            string selecteditemcode = ObjDAL.ConvertDataTabletoString(DtList.Tables[6]);

            string ItemName = ObjDAL.ConvertDataTabletoString(DtList.Tables[7]);

            string SelectdItemName = ObjDAL.ConvertDataTabletoString(DtList.Tables[8]);

            return Json(new { POlst = POlst, Onsite = Onsite, Desk = Desk, TotalVisit = TotalVisit, PoNumber = PoNumber, itemtagNumber = itemtagNumber, selectedponumber = selectedponumber, selectedpoitem = selectedpoitem, selecteditemcode = selecteditemcode, ItemName = ItemName, SelectdItemName = SelectdItemName }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FillOnPONumberChange(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {
            DataSet DtList = new DataSet();
            DataSet Dtlist2 = new DataSet();
            DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "5", TagNumber, itemName);

            //string Projlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            //string Vendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[1]);
            //string SubVendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[2]);
            string Itemlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string Taglst = ObjDAL.ConvertDataTabletoString(DtList.Tables[1]);

            string Onsite = DtList.Tables[2].Rows[0]["Onsite"].ToString();
            string Desk = DtList.Tables[2].Rows[0]["Desk"].ToString();
            string TotalVisit = DtList.Tables[2].Rows[0]["TotalVisit"].ToString();

            string selectedVendor = ObjDAL.ConvertDataTabletoString(DtList.Tables[3]);

            string selectedpoitem = ObjDAL.ConvertDataTabletoString(DtList.Tables[4]);
            string selecteditemcode = ObjDAL.ConvertDataTabletoString(DtList.Tables[5]);
            string TotalSubVendor = ObjDAL.ConvertDataTabletoString(DtList.Tables[6]);

            string ItemName = ObjDAL.ConvertDataTabletoString(DtList.Tables[7]);

            string SelectdItemName = ObjDAL.ConvertDataTabletoString(DtList.Tables[8]);




            if (Company != null && Project == "0" && SubVendorname == "" && PONumber == "" && ItemNumber == "")
            {
                Dtlist2 = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "8", TagNumber, itemName);
            }
            else
            {
                Dtlist2 = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "7", TagNumber, itemName);
            }

            string OnsiteValuelst = ObjDAL.ConvertDataTabletoString(Dtlist2.Tables[0]);

            return Json(new { Itemlst = Itemlst, Onsite = Onsite, Desk = Desk, TotalVisit = TotalVisit, Taglst = Taglst, OnsiteValuelst = OnsiteValuelst, selectedponumber = selectedVendor, selectedpoitem = selectedpoitem, selecteditemcode = selecteditemcode, TotalSubVendor = TotalSubVendor, ItemName = ItemName, SelectdItemName = SelectdItemName }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FillOnItemNumberChange(string Company, string Project, string VendorName, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {
            DataSet DtList = new DataSet();
            DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "6", TagNumber, itemName);

            //string Projlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            // string Vendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[1]);
            //string SubVendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string Taglst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string Onsite = DtList.Tables[1].Rows[0]["Onsite"].ToString();
            string Desk = DtList.Tables[1].Rows[0]["Desk"].ToString();
            string TotalVisit = DtList.Tables[1].Rows[0]["TotalVisit"].ToString();

            string selectedPONumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[2]);

            string selectedvendorName = ObjDAL.ConvertDataTabletoString(DtList.Tables[3]);

            string selecteditemcode = ObjDAL.ConvertDataTabletoString(DtList.Tables[4]);

            string TotalSubVendor = ObjDAL.ConvertDataTabletoString(DtList.Tables[5]);

            string TotalPovendor = ObjDAL.ConvertDataTabletoString(DtList.Tables[6]);

            string ItemName = ObjDAL.ConvertDataTabletoString(DtList.Tables[7]);

            string SelectdItemName = ObjDAL.ConvertDataTabletoString(DtList.Tables[8]);




            //string vendorCount = DtList.Tables[6].Rows[0]["TotalVendor"].ToString();

            return Json(new { Taglst = Taglst, Onsite = Onsite, Desk = Desk, TotalVisit = TotalVisit, selectedvendorName = selectedvendorName, selectedPONumber = selectedPONumber, selecteditemcode = selecteditemcode, TotalSubVendor = TotalSubVendor, TotalPovendor = TotalPovendor, ItemName = ItemName, SelectdItemName = SelectdItemName }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FillOnsiteReportData(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {
            DataSet DtList = new DataSet();

            if (Company != null && Project == "0" && SubVendorname == "" && PONumber == "" && ItemNumber == "")
            {
                DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "8", TagNumber, itemName);
            }
            else
            {
                DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "7", TagNumber, itemName);
            }



            string OnsiteValuelst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);

            return Json(new { OnsiteValuelst = OnsiteValuelst }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult FillConcernsData(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string Stage)
        {
            DataSet DtList = new DataSet();

            if (Company != null && Project == "0" && SubVendorname == "" && PONumber == "" && ItemNumber == "")
            {
                DtList = ObjDAL.FillALLDropdownConcerns(Company, Project, SubVendorname, PONumber, ItemNumber, "9", Stage);
            }
            else
            {
                DtList = ObjDAL.FillALLDropdownConcerns(Company, Project, SubVendorname, PONumber, ItemNumber, "10", Stage);
            }



            string ConcernsValuedata = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);

            return Json(new { ConcernsValuedata = ConcernsValuedata }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult FillEngData(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string Stage)
        {
            DataSet DtList = new DataSet();

            if (Company != null && Project == "0" && SubVendorname == "" && PONumber == "" && ItemNumber == "" && PONumber == null && ItemNumber == null)
            {
                DtList = ObjDAL.FillALLDropdownConcerns(Company, Project, SubVendorname, PONumber, ItemNumber, "11", Stage);
            }
            else
            {
                DtList = ObjDAL.FillALLDropdownConcerns(Company, Project, SubVendorname, PONumber, ItemNumber, "12", Stage);
            }



            string EngValuedata = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);

            return Json(new { EngValuedata = EngValuedata }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult FillDeskReportData(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {
            DataSet DtList = new DataSet();

            if (Company != null && Project == "0" && SubVendorname == "" && PONumber == "" && ItemNumber == "")
            {
                DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "14", TagNumber, itemName);
            }
            else
            {
                DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "13", TagNumber, itemName);
            }



            string DeskValuelst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);

            return Json(new { DeskValuelst = DeskValuelst }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult FillTotalReportData(string Company, string Project, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {
            DataSet DtList = new DataSet();

            if (Company != null && Project == "0" && SubVendorname == "" && PONumber == "" && ItemNumber == "")
            {
                DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "16", TagNumber, itemName);
            }
            else
            {
                DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "15", TagNumber, itemName);
            }



            string TotalValuelst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);

            return Json(new { TotalValuelst = TotalValuelst }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult FillOnItemCodeChange(string Company, string Project, string VendorName, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {
            DataSet DtList = new DataSet();
            DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "17", TagNumber, itemName);

            //string Projlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            // string Vendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[1]);
            //string SubVendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string POlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string Onsite = DtList.Tables[1].Rows[0]["Onsite"].ToString();
            string Desk = DtList.Tables[1].Rows[0]["Desk"].ToString();
            string TotalVisit = DtList.Tables[1].Rows[0]["TotalVisit"].ToString();


            string selectedPONumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[2]);

            string selectedvendorName = ObjDAL.ConvertDataTabletoString(DtList.Tables[3]);

            string selecteditemcode = ObjDAL.ConvertDataTabletoString(DtList.Tables[4]);

            string TotalSubVendor = ObjDAL.ConvertDataTabletoString(DtList.Tables[5]);

            string TotalPovendor = ObjDAL.ConvertDataTabletoString(DtList.Tables[6]);

            string ItemName = ObjDAL.ConvertDataTabletoString(DtList.Tables[7]);

            string SelectdItemName = ObjDAL.ConvertDataTabletoString(DtList.Tables[8]);



            //string vendorCount = DtList.Tables[6].Rows[0]["TotalVendor"].ToString();

            return Json(new { POlst = POlst, Onsite = Onsite, Desk = Desk, TotalVisit = TotalVisit, selectedvendorName = selectedvendorName, selectedPONumber = selectedPONumber, selecteditemcode = selecteditemcode, TotalSubVendor = TotalSubVendor, TotalPovendor = TotalPovendor, ItemName = ItemName, SelectdItemName = SelectdItemName }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult FillGantChartData(string Company, string Project, string VendorName, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {

            DataSet DtList = new DataSet();
            DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "18", TagNumber, itemName);

            string gantchart = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string Actual_Progressper = DtList.Tables[1].Rows[0]["Actual_Progressper"].ToString();
            string Expected_Progressper = DtList.Tables[1].Rows[0]["Expected_Progressper"].ToString();
            string overAll = DtList.Tables[1].Rows[0]["Progress_Status"].ToString();

            string gantchart1 = ObjDAL.ConvertDataTabletoString(DtList.Tables[2]);

            string Openconcern = DtList.Tables[3].Rows[0]["Openconcern"].ToString();
            string closeconcerns = DtList.Tables[3].Rows[0]["closeconcerns"].ToString();
            string actionValuelst = ObjDAL.ConvertDataTabletoString(DtList.Tables[4]);
            string vendorCount = DtList.Tables[5].Rows[0]["TotalVendor"].ToString();
            string Concernslit = ObjDAL.ConvertDataTabletoString(DtList.Tables[6]);


            return Json(new { gantchart = gantchart, Actual_Progressper = Actual_Progressper, Expected_Progressper = Expected_Progressper, overAll = overAll, Openconcern = Openconcern, closeconcerns = closeconcerns, actionValuelst = actionValuelst, vendorCount = vendorCount, Concernslit = Concernslit, gantchart1 = gantchart1 }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult Concerns()
        {
            var model = new Concerns();
            DataSet DtList = new DataSet();

            string stage = Session["stage"].ToString();
            if (Session["CompanyName"].ToString() != null || Session["SubVendorname"].ToString() != null || Session["PONumber"].ToString() != null || Session["ItemNumber"].ToString() != null)
            {
                string Project = Session["ProjectName"].ToString();
                string Company = Session["CompanyName"].ToString();
                string SubVendorname = Session["SubVendorname"].ToString();
                string PONumber = Session["PONumber"].ToString();
                string ItemNumber = Session["ItemNumber"].ToString();
                DataTable dsGetStamp = new DataTable();
                List<Concerns> lstCompanyDashBoard = new List<Concerns>();


                #region Bind ddlConcernStatus 
                var Data2 = ObjDAL.GetddlConcernStatus();
                ViewBag.DDLConcernStatus = new SelectList(Data2, "Value", "Text");
                #endregion

                if (Company != null && Project == "0" && SubVendorname == "" && PONumber == "" && ItemNumber == "")
                {
                    dsGetStamp = ObjDAL.GetConcernData(Company, Project, SubVendorname, PONumber, ItemNumber, "19", stage);
                }
                else
                {
                    dsGetStamp = ObjDAL.GetConcernData(Company, Project, SubVendorname, PONumber, ItemNumber, "20", stage);
                }







                if (dsGetStamp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGetStamp.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new Concerns
                            {
                                PK_Concern_Id = Convert.ToInt32(dr["pk_concerns_id"]),
                                PK_Call_ID = Convert.ToString(dr["PK_Call_ID"]),
                                Date = Convert.ToString(dr["Date"]),
                                Category = Convert.ToString(dr["Category"]),
                                Item = Convert.ToString(dr["Item"]),
                                Stage = Convert.ToString(dr["StageName"]),
                                Details = Convert.ToString(dr["Details"]),
                                MitigationBy = Convert.ToString(dr["MitigationBy"]),
                                ResponsiblePerson = Convert.ToString(dr["ResponsiblePerson"]),
                                ExpectedClosureDate = Convert.ToString(dr["ExpectedClosureDate"]),
                                Status = Convert.ToString(dr["Status"]),
                                StatusUpdatedOn = Convert.ToString(dr["StatusUpdatedOn"]),
                                Comment = Convert.ToString(dr["Comment"]),

                                //ConcernsstageProgressper = Convert.ToString(dr["ConcernsStageDate"]),
                                //delaydays = Convert.ToString(dr["delaydays"]),
                                ActionReq = Convert.ToString(dr["ActionReq"]),

                                VendorPONumber = Convert.ToString(dr["Vendor_Po_No"]),

                            }
                            );
                    }
                }

                ViewData["lstConcernData"] = lstCompanyDashBoard;
                //ViewBag.lstConcern = lstCompanyDashBoard;

                lstCompanyDashBoard = model.lstConcerns;




                //ViewBag.ConcernData = model;
                //Session["CompanyName"] = null;
                return View(model);

            }
            return View();
        }






        public ActionResult ConcernsOpenClose(string Company, string Project, string VendorName, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string stage)
        {

            DataSet DtList = new DataSet();
            Session["CompanyName"] = Company;
            Session["ProjectName"] = Project;
            Session["VendorName"] = VendorName;
            Session["SubVendorname"] = SubVendorname;
            Session["PONumber"] = PONumber;
            Session["ItemNumber"] = ItemNumber;
            Session["stage"] = stage;
            DtList = ObjDAL.FillALLDropdownConcerns(Company, Project, SubVendorname, PONumber, ItemNumber, "19", stage);
            string Concernslit = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            return Json(new { Concernslit = Concernslit }, JsonRequestBehavior.AllowGet);

        }





        [HttpPost]
        public ActionResult UpdateSelectedRows(List<Concerns> selectedRows)
        {
            var IVR = new Concerns();
            string Result = string.Empty;

            foreach (var row in selectedRows)
            {
                IVR.Status = row.Status;
                IVR.Details = row.Details;
                IVR.StatusUpdatedOn = row.StatusUpdatedOn;
                IVR.Comment = row.Comment;
                IVR.PK_Concern_Id = row.PK_Concern_Id;
                IVR.MitigationBy = row.MitigationBy;


                Result = ObjDAL.UpdateConcern(IVR);
            }




            return Json(new { success = true });
        }


        //public ActionResult Vendordetails(string SubVendorname)
        //{
        //    var model = new Progress();


        //    DataTable dsGetStamp = new DataTable();
        //    List<Progress> lstCompanyDashBoard = new List<Progress>();


        //        dsGetStamp = ObjDAL.GetConcernData("", "", SubVendorname, "", "", "21", "");          

        //    if (dsGetStamp.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dsGetStamp.Rows)
        //        {
        //            lstCompanyDashBoard.Add(
        //                new Progress
        //                {
        //                    PK_Call_Id = Convert.ToString(dr["PK_Call_ID"]),
        //                    pk_subjobid = Convert.ToString(dr["PK_SubJob_Id"]),
        //                    vendor = Convert.ToString(dr["vendor_name"]),
        //                    VendorPO = Convert.ToString(dr["Vendor_Po_No"]),
        //                    ExpeditingDate = Convert.ToString(dr["date_of_expediting"]),
        //                    ExpectedStartDate = Convert.ToString(dr["ExpectedStartDate"]),
        //                    ExpectedEndDate = Convert.ToString(dr["ExpectedEndDate"]),
        //                    Actual_Start_Date = Convert.ToString(dr["Actual_Start_Date"]),
        //                    Actual_End_Date = Convert.ToString(dr["Actual_End_Date"]),
        //                    Expected_Progressper = Convert.ToString(dr["Expected_Progressper"]),
        //                    Actual_Progressper = Convert.ToString(dr["Actual_Progressper"]),



        //                     ProgressStatus= Convert.ToString(dr["Progress_Status"]),



        //                }
        //                );
        //        }
        //    }

        //    ViewData["VendorDetails"] = lstCompanyDashBoard;

        //    //ViewBag.lstConcern = lstCompanyDashBoard;

        //    lstCompanyDashBoard = model.Activity;



        //    // return View(model);
        //    return Json(lstCompanyDashBoard, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult Vendordetails(string Project)
        {
            var model = new Progress();
            var lstCompanyDashBoard = new List<Progress>();

            DataTable dsGetStamp = ObjDAL.GetConcernData("", Project, "", "", "", "21", "");

            if (dsGetStamp.Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetStamp.Rows)
                {
                    lstCompanyDashBoard.Add(
                        new Progress
                        {
                            PK_Call_Id = Convert.ToString(dr["PK_Call_ID"]),
                            pk_subjobid = Convert.ToString(dr["PK_SubJob_Id"]),
                            vendor = Convert.ToString(dr["vendor_name"]),
                            VendorPO = Convert.ToString(dr["Vendor_Po_No"]),
                            ExpeditingDate = Convert.ToString(dr["date_of_expediting"]),
                            ExpectedStartDate = FormatDate(dr["ExpectedStartDate"]),
                            ExpectedEndDate = FormatDate(dr["ExpectedEndDate"]),
                            Actual_Start_Date = FormatDate(dr["Actual_Start_Date"]),
                            Actual_End_Date = FormatDate(dr["Actual_End_Date"]),
                            Expected_Progressper = Convert.ToString(dr["Expected_Progressper"]),
                            Actual_Progressper = Convert.ToString(dr["Actual_Progressper"]),
                            ProgressStatus = Convert.ToString(dr["Progress_Status"]),
                        }
                    );
                }
            }

            ViewData["VendorDetails"] = lstCompanyDashBoard;
            model.Activity = lstCompanyDashBoard;

            DataTable dsGetStampvalue = ObjDAL.GetConcernData("", Project, "", "", "", "25", "");
            if (dsGetStampvalue != null && dsGetStampvalue.Rows.Count > 0)
            {
                model.Total = Convert.ToString(dsGetStampvalue.Rows[0]["TotalNo"]);
                model.OntimeCount = Convert.ToString(dsGetStampvalue.Rows[0]["OnTimeCount"]);
                model.DelayCount = Convert.ToString(dsGetStampvalue.Rows[0]["DelayCount"]);
                model.earlycount = Convert.ToString(dsGetStampvalue.Rows[0]["EarlyCount"]);
            }
            return View(model);
        }






        private string FormatDate(object dateObj)
        {
            if (dateObj == DBNull.Value)
            {
                return string.Empty;
            }

            DateTime date;
            if (DateTime.TryParse(dateObj.ToString(), out date))
            {
                return date.ToString("dd/MM/yyyy");
            }
            return string.Empty;
        }

        [HttpGet]
        public ActionResult POitemdetails(string Project)
        {
            var model = new Progress();
            var lstCompanyDashBoard = new List<Progress>();

            DataTable dsGetStamp = ObjDAL.GetConcernData("", Project, "", "", "", "22", "");

            if (dsGetStamp.Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetStamp.Rows)
                {
                    lstCompanyDashBoard.Add(
                        new Progress
                        {
                            PK_Call_Id = Convert.ToString(dr["PK_Call_ID"]),
                            pk_subjobid = Convert.ToString(dr["PK_SubJob_Id"]),
                            itemnumbar = Convert.ToString(dr["PO_Item_Number"]),
                            vendor = Convert.ToString(dr["vendor_name"]),
                            VendorPO = Convert.ToString(dr["Vendor_Po_No"]),
                            ExpeditingDate = Convert.ToString(dr["date_of_expediting"]),
                            ExpectedStartDate = FormatDate(dr["ExpectedStartDate"]),
                            ExpectedEndDate = FormatDate(dr["ExpectedEndDate"]),
                            Actual_Start_Date = FormatDate(dr["Actual_Start_Date"]),
                            Actual_End_Date = FormatDate(dr["Actual_End_Date"]),
                            Expected_Progressper = Convert.ToString(dr["Expected_Progressper"]),
                            Actual_Progressper = Convert.ToString(dr["Actual_Progressper"]),
                            ProgressStatus = Convert.ToString(dr["Progress_Status"]),

                            //added by shrutika salve 05082024
                            itemCode = Convert.ToString(dr["ItemCode"]),
                            ItemDescription = Convert.ToString(dr["ItemDescription"]),
                            itemtag = Convert.ToString(dr["Equipment_TagNo"]),
                            Quantity = Convert.ToString(dr["Quantity"]),
                            Unit = Convert.ToString(dr["UnitName"]),
                        }
                    );
                }
            }

            ViewData["POItemDetails"] = lstCompanyDashBoard;
            model.Activity = lstCompanyDashBoard;

            DataTable dsGetStampvalue = ObjDAL.GetConcernData("", Project, "", "", "", "26", "");
            if (dsGetStampvalue != null && dsGetStampvalue.Rows.Count > 0)
            {
                model.Total = Convert.ToString(dsGetStampvalue.Rows[0]["TotalNo"]);
                model.OntimeCount = Convert.ToString(dsGetStampvalue.Rows[0]["OnTimeCount"]);
                model.DelayCount = Convert.ToString(dsGetStampvalue.Rows[0]["DelayCount"]);
                model.earlycount = Convert.ToString(dsGetStampvalue.Rows[0]["EarlyCount"]);
            }
            return View(model);
        }




        [HttpGet]
        public ActionResult FillItemNameChange(string Company, string Project, string VendorName, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string ItemName)
        {
            DataSet DtList = new DataSet();
            DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "23", TagNumber, ItemName);

            //string Projlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            // string Vendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[1]);
            //string SubVendorlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string POlst = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string Onsite = DtList.Tables[1].Rows[0]["Onsite"].ToString();
            string Desk = DtList.Tables[1].Rows[0]["Desk"].ToString();
            string TotalVisit = DtList.Tables[1].Rows[0]["TotalVisit"].ToString();


            string selectedPONumber = ObjDAL.ConvertDataTabletoString(DtList.Tables[2]);

            string selectedvendorName = ObjDAL.ConvertDataTabletoString(DtList.Tables[3]);

            //string selecteditemcode = ObjDAL.ConvertDataTabletoString(DtList.Tables[4]);
            string selectedpoitem = ObjDAL.ConvertDataTabletoString(DtList.Tables[4]);

            string TotalSubVendor = ObjDAL.ConvertDataTabletoString(DtList.Tables[5]);

            string TotalPovendor = ObjDAL.ConvertDataTabletoString(DtList.Tables[6]);

            string Taglst = ObjDAL.ConvertDataTabletoString(DtList.Tables[7]);

            string POItemCode = ObjDAL.ConvertDataTabletoString(DtList.Tables[8]);


            //string vendorCount = DtList.Tables[6].Rows[0]["TotalVendor"].ToString();

            return Json(new { POlst = POlst, Onsite = Onsite, Desk = Desk, TotalVisit = TotalVisit, selectedvendorName = selectedvendorName, selectedPONumber = selectedPONumber, selectedpoitem = selectedpoitem, TotalSubVendor = TotalSubVendor, TotalPovendor = TotalPovendor, Taglst = Taglst, POItemCode = POItemCode }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult FillGantChartDataItemName(string Company, string Project, string VendorName, string SubVendorname, string PONumber, string ItemNumber, string TagNumber, string itemName)
        {

            DataSet DtList = new DataSet();
            DtList = ObjDAL.FillALLDropdown(Company, Project, SubVendorname, PONumber, ItemNumber, "24", TagNumber, itemName);

            string gantchart = ObjDAL.ConvertDataTabletoString(DtList.Tables[0]);
            string Actual_Progressper = DtList.Tables[1].Rows[0]["Actual_Progressper"].ToString();
            string Expected_Progressper = DtList.Tables[1].Rows[0]["Expected_Progressper"].ToString();
            string overAll = DtList.Tables[1].Rows[0]["Progress_Status"].ToString();

            string gantchart1 = ObjDAL.ConvertDataTabletoString(DtList.Tables[2]);

            string Openconcern = DtList.Tables[3].Rows[0]["Openconcern"].ToString();
            string closeconcerns = DtList.Tables[3].Rows[0]["closeconcerns"].ToString();
            string actionValuelst = ObjDAL.ConvertDataTabletoString(DtList.Tables[4]);
            string vendorCount = DtList.Tables[5].Rows[0]["TotalVendor"].ToString();
            string Concernslit = ObjDAL.ConvertDataTabletoString(DtList.Tables[6]);


            return Json(new { gantchart = gantchart, Actual_Progressper = Actual_Progressper, Expected_Progressper = Expected_Progressper, overAll = overAll, Openconcern = Openconcern, closeconcerns = closeconcerns, actionValuelst = actionValuelst, vendorCount = vendorCount, Concernslit = Concernslit, gantchart1 = gantchart1 }, JsonRequestBehavior.AllowGet);

        }


        #endregion
    


    //added by nikita on 11/11/2024
    public ActionResult SendExpeditingReport(string PK_Call_Id, string CMP_ID, string CustomerName, string VendorName, string Sub_VendorName, string Date_Of_Expediting)
    {
        objModel.Date_Of_Expediting = Date_Of_Expediting;
        objModel.PK_call_id = PK_Call_Id;
        objModel.Customer_Name = CustomerName;
        objModel.VendorName = VendorName;
        objModel.Sub_VendorName = Sub_VendorName;
        objModel.CMP_ID = CMP_ID;

        var contacts = objModel.VendorName?.Split(',').Select(c => new SelectListItem
        {
            Value = c,
            Text = c
        }).ToList() ?? new List<SelectListItem>();

        ViewBag.Contacts = contacts;

        var contacts_ = objModel.Sub_VendorName?.Split(',').Select(c => new SelectListItem
        {
            Value = c,
            Text = c
        }).ToList() ?? new List<SelectListItem>();

        ViewBag.Contacts_ = contacts_;




        var contactsClient = objModel.CMP_ID?.Split(',').Select(c => new SelectListItem
        {
            Value = c,
            Text = c
        }).ToList() ?? new List<SelectListItem>();

        ViewBag.contactsClient = contactsClient;


        return View(objModel);

    }

    [HttpPost]
    public ActionResult GetData(int Pk_Call_id)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = ObjDAL.GetEmaildata(Pk_Call_id);
        }
        catch (Exception ex)
        {
            string mesage = ex.Message;
            return Json("Error", JsonRequestBehavior.AllowGet);
        }
        var Response = JsonConvert.SerializeObject(dt);
        return Json(Response, JsonRequestBehavior.AllowGet);
    }

    public ActionResult GetAttachmentData(int Pk_Call_id)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = ObjDAL.GetAttachmentsData(Pk_Call_id);
        }
        catch (Exception ex)
        {
            string mesage = ex.Message;
            return Json("Error", JsonRequestBehavior.AllowGet);
        }
        var result = JsonConvert.SerializeObject(dt);
        return Json(result, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult UpdateMailFlag(int? pk_call_id)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ObjDAL.UpdateMailFlag(pk_call_id);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public JsonResult UpdateTUVMail(int? PK_Call_ID, string Email)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ObjDAL.UpdateTuvMail(PK_Call_ID, Email);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public JsonResult Updatetable(int? pk_call_id, string Email, string GetType)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ObjDAL.Updatetable_vendor_Email(pk_call_id, Email, GetType);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public JsonResult UpdateClientTable(int? pk_call_id, string Email, string ClientName)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ObjDAL.UpdateClientTable(pk_call_id, Email, ClientName);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public JsonResult UpdateTUVEmial(int? pk_call_id, string Email, string ClientName)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ObjDAL.UpdateTUVEmial(pk_call_id, Email, ClientName);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult updateClientName(int pk_call_id, string[] Email, string clientname)
    {
        DataTable dt = new DataTable();
        try
        {
            foreach (var email in Email)
            {
                // Assuming objDalVisitReport.updateClient is designed to handle individual emails
                dt = ObjDAL.updateClient(pk_call_id, email, clientname);
            }
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult updateVendorName(int pk_call_id, string[] Email, string Vendor_Name_Location)
    {
        DataTable dt = new DataTable();
        try
        {
            foreach (var email in Email)
            {
                // Assuming objDalVisitReport.updateClient is designed to handle individual emails
                dt = ObjDAL.updateVendor(pk_call_id, email, Vendor_Name_Location);
            }
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult updateSubVendorName(int pk_call_id, string[] Email, string Vendor_Name_Location)
    {
        DataTable dt = new DataTable();
        try
        {
            foreach (var email in Email)
            {
                // Assuming objDalVisitReport.updateClient is designed to handle individual emails
                dt = ObjDAL.updateSubVendor(pk_call_id, email, Vendor_Name_Location);
            }
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult updateSubSubVendorName(int pk_call_id, string[] Email, string Vendor_Name_Location)
    {
        DataTable dt = new DataTable();
        try
        {
            foreach (var email in Email)
            {
                // Assuming objDalVisitReport.updateClient is designed to handle individual emails
                dt = ObjDAL.updateSubSubVendor(pk_call_id, email, Vendor_Name_Location);
            }
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult DeleteEmail(int pk_call_id, string Type, string Name, string Email)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ObjDAL.DeleteEmail(pk_call_id, Type, Name, Email);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            return Json(new { success = false });
        }

    }

    [HttpPost]
    public ActionResult SendExpeditingMail(string emails, string attachments, string pk_call_id, string mailsubject)
    {
        string displayName = string.Empty;
        string ClientEmail = string.Empty;
        string bodyTxt = string.Empty;
        string Mailsubject = mailsubject;
        try
        {
            DataTable dt = new DataTable();
            dt = ObjDAL.SaveData_(pk_call_id, emails, attachments, mailsubject);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
}
}
