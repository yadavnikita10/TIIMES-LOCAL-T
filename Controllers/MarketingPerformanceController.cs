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
    public class MarketingPerformanceController : Controller
    {

        MarketingPerformance obj1 = new MarketingPerformance();
        QHSEPerformance obj2 = new QHSEPerformance();
        DALMarketingPerformance objDAL = new DALMarketingPerformance();
        OperationPerformance obj3 = new OperationPerformance();
        // GET: MarketingPerformance
        public ActionResult MarketingPerformance()
        {

            DataTable dtMarketingPerformance = new DataTable();
            List<MarketingPerformance> lstComplaintDashBoard = new List<MarketingPerformance>();


            if (Session["FinalFromDate"] != null && Session["FinalTodate"] != null)
            {

                obj1.FromDate = Convert.ToString(Session["FinalFromDate"].ToString());
                obj1.ToDate = Convert.ToString(Session["FinalTodate"].ToString());
                dtMarketingPerformance = objDAL.GetData(obj1);


                try
                {
                    if (dtMarketingPerformance.Rows.Count > 0)
                    {
                        
                        foreach (DataRow dr in dtMarketingPerformance.Rows)
                        {
                            lstComplaintDashBoard.Add(
                                new MarketingPerformance
                                {
                                    PK_UserId = Convert.ToString(dr["PK_UserId"]),
                                    TotalEnquiries = Convert.ToString(dr["TotalEnquiries"]),
                                    EnquiriesOnTime = Convert.ToString(dr["EnquiriesOnTime"]),
                                    OnTimePercentage = Convert.ToString(dr["OnTimePercentage"]),
                                    QuotationsGeneratedAgainstEnquiries = Convert.ToString(dr["QuotationsGeneratedAgainstEnquiries"]),
                                    QuotationsPercentage = Convert.ToString(dr["QuotationsPercentage"]),
                                    TotalQuotationsGenerated = Convert.ToString(dr["TotalQuotationsGenerated"]),
                                    ActionTakenQuotations = Convert.ToString(dr["NonStatusOneQuotations"]),
                                    ActionTakenQuotationsPercentage = Convert.ToString(dr["NonStatusOneQuotationsPercentage"]),
                                    JobCreated = Convert.ToString(dr["TotalJobs"]),
                                    JobCreatedOnTime = Convert.ToString(dr["JobsWithin10Days"]),
                                    Name = Convert.ToString(dr["Name"]),
                                    JobPercentage = Convert.ToString(dr["JobPercentage"]),
                                    OverAllPercentage = Convert.ToString(dr["OverallPercentage"]),
                                    Branch = Convert.ToString(dr["BranchName"]),
                                });
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }

                ViewData["MarketingPer"] = lstComplaintDashBoard;

            }



            else
            {

            }



            return View();
        }


        [HttpPost]
        public ActionResult MarketingPerformance(MarketingPerformance CM)
        {
            Session["FinalFromDate"] = CM.FromDate;
            Session["FinalTodate"] = CM.ToDate;
            return RedirectToAction("MarketingPerformance");

        }



        public ActionResult MarketingKPI()
        {

            DataTable dtMarketingPerformance = new DataTable();
            List<MarketingPerformance> lstComplaintDashBoard = new List<MarketingPerformance>();


            if (Session["FinalFromDate"] != null && Session["FinalTodate"] != null)
            {

                obj1.FromDate = Convert.ToString(Session["FinalFromDate"].ToString());
                obj1.ToDate = Convert.ToString(Session["FinalTodate"].ToString());
                dtMarketingPerformance = objDAL.GetDataKPI(obj1);


                try
                {
                    if (dtMarketingPerformance.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dtMarketingPerformance.Rows)
                        {
                            lstComplaintDashBoard.Add(
                                new MarketingPerformance
                                {
                                    //PK_UserId = Convert.ToString(dr["PK_UserId"]),
                                    Name = Convert.ToString(dr["InspectorName"]),
                                    Branch = Convert.ToString(dr["Branch_Name"]),
                                    profilePerformancePercentage = Convert.ToString(dr["profilePerformancePercentage"]),
                                    TSFilledPercentage = Convert.ToString(dr["TSFilledPercentage"]),
                                    AVG = Convert.ToString(dr["AVG"]),
                                    KPI = Convert.ToString(dr["KPI"]),

                                });
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }

                ViewData["MarketingKPI"] = lstComplaintDashBoard;

            }



            else
            {

            }



            return View();
        }


        [HttpPost]
        public ActionResult MarketingKPI(MarketingPerformance CM)
        {
            Session["FinalFromDate"] = CM.FromDate;
            Session["FinalTodate"] = CM.ToDate;
            return RedirectToAction("MarketingKPI");

        }

        #region QHSE Performance
        public ActionResult QHSEPerformance()
        {

            DataTable dtQHSEPerformance = new DataTable();
            List<QHSEPerformance> lstQHSEPerformance = new List<QHSEPerformance>();


            if (Session["QHSEFinalFromDate"] != null && Session["QHSEFinalTodate"] != null)
            {

                obj1.FromDate = Convert.ToString(Session["QHSEFinalFromDate"].ToString());
                obj1.ToDate = Convert.ToString(Session["QHSEFinalTodate"].ToString());
                dtQHSEPerformance = objDAL.GetQHSEPerformance(obj1);


                try
                {
                    if (dtQHSEPerformance.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dtQHSEPerformance.Rows)
                        {
                            lstQHSEPerformance.Add(
                                new QHSEPerformance
                                {

                                    Branch_Name = Convert.ToString(dr["Branch_Name"]),
                                    No_Of_Complaints = Convert.ToString(dr["No_Of_Complaints"]),
                                    No_Of_Complaints_On_Time = Convert.ToString(dr["No_Of_Complaints_On_Time"]),
                                    Percentage_On_Time = Convert.ToString(dr["Percentage_On_Time"]),
                                    No_Of_Inspectors = Convert.ToString(dr["No_Of_Inspectors"]),
                                    NumberOfTCEFilled = Convert.ToString(dr["NumberOfTCEFilled"]),
                                    Percentage_TCE_Filled = Convert.ToString(dr["Percentage_TCE_Filled"]),
                                    No_Of_SafetyIncidentReports = Convert.ToString(dr["No_Of_SafetyIncidentReports"]),
                                    No_Of_OnTimeReports = Convert.ToString(dr["No_Of_OnTimeReports"]),
                                    Percentage_OnTimeReports = Convert.ToString(dr["Percentage_OnTimeReports"]),
                                    No_Of_Complaints_ReceivedExe = Convert.ToString(dr["No_Of_Complaints_ReceivedExe"]),
                                    No_Of_Complaints_Closed = Convert.ToString(dr["No_Of_Complaints_Closed"]),
                                    No_Of_Complaints_Open = Convert.ToString(dr["No_Of_Complaints_Open"]),



                                });
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }

                ViewData["QHSEPerformance"] = lstQHSEPerformance;

            }



            else
            {

            }



            return View();
        }


        [HttpPost]
        public ActionResult QHSEPerformance(QHSEPerformance CM)
        {
            Session["QHSEFinalFromDate"] = CM.FromDate;
            Session["QHSEFinalTodate"] = CM.ToDate;
            return RedirectToAction("QHSEPerformance");

        }
        #endregion

        #region Operation Performance
        public ActionResult OperationPerformance()
        {

            DataTable dtOperationPerformance = new DataTable();
            List<OperationPerformance> lstOperationPerformance = new List<OperationPerformance>();


            if (Session["OperationFromDate"] != null && Session["OperationTodate"] != null)
            {

                obj3.FromDate = Convert.ToString(Session["OperationFromDate"].ToString());
                obj3.ToDate = Convert.ToString(Session["OperationTodate"].ToString());
                dtOperationPerformance = objDAL.OperationPerformance(obj3);


                try
                {
                    if (dtOperationPerformance.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dtOperationPerformance.Rows)
                        {
                            lstOperationPerformance.Add(
                                new OperationPerformance
                                {

                                    InspectorName = Convert.ToString(dr["InspectorName"]),
                                    Branch = Convert.ToString(dr["Branch"]),
                                    TSFilledPercentage = Convert.ToString(dr["TSFilledPercentage"]),
                                    ProfilePercentage = Convert.ToString(dr["ProfilePercentage"]),
                                    AVG = Convert.ToString(dr["AVG"]),



                                });
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }

                ViewData["OperationPerformance"] = lstOperationPerformance;

            }



            else
            {

            }



            return View();
        }


        [HttpPost]
        public ActionResult OperationPerformance(OperationPerformance CM)
        {
            Session["OperationFromDate"] = CM.FromDate;
            Session["OperationTodate"] = CM.ToDate;
            return RedirectToAction("OperationPerformance");

        }
        #endregion


        #region Branch Performance

        public ActionResult BranchPerformance()
        {

            DataTable dtBranchPerformance = new DataTable();
            List<BranchPerformance> lstBranchPerformance = new List<BranchPerformance>();

            #region Bind Dropdownlist
            DataSet ds = objDAL.GetDates();  // Replace with your method to get the DataSet

            // Extract the DataTable from the DataSet
            DataTable dt = ds.Tables[0];

            // Create a list of SelectListItem to store the dropdown options
            List<SelectListItem> dropdownItems = new List<SelectListItem>();

            foreach (DataRow row in dt.Rows)
            {
                // Assuming you have columns "Id" and "Name" in your DataTable
                dropdownItems.Add(new SelectListItem
                {
                    Value = row["MonthYear"].ToString(),
                    Text = row["MonthYear"].ToString()
                });
            }

            // Pass the list to the ViewBag or ViewModel
            ViewBag.DropdownListItems = dropdownItems;
            #endregion

            BranchPerformance objBP = new BranchPerformance();

            if (Session["SelectedMonthYear"] != null)
            {

                objBP.SelectedValue = Convert.ToString(Session["SelectedMonthYear"].ToString());

                dtBranchPerformance = objDAL.GetBranchPerformance(objBP);


                try
                {
                    if (dtBranchPerformance.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dtBranchPerformance.Rows)
                        {
                            lstBranchPerformance.Add(
                                    new BranchPerformance
                                    {

                                        BranchName = Convert.ToString(dr["BranchName"]),
                                        AvgInspectorPer = Convert.ToString(dr["AvgInspectorPer"]),
                                        AvgCordinatorPer = Convert.ToString(dr["AvgCordinatorPer"]),
                                        AvgMarketingPer = Convert.ToString(dr["AvgMarketingPer"]),
                                        AvgQHSEPer = Convert.ToString(dr["AvgQHSEPer"]),
                                        AvgOperationPer = Convert.ToString(dr["AvgOperationPer"]),
                                        GrandFinal = Convert.ToString(dr["GrandFinal"]),




                                    });
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }

                ViewData["BranchPerformance"] = lstBranchPerformance;

            }



            else
            {

            }



            return View(objBP);
        }

        [HttpPost]
        public ActionResult BranchPerformance(BranchPerformance BP)
        {
            Session["SelectedMonthYear"] = BP.SelectedValue;
            return RedirectToAction("BranchPerformance");
        }



        #endregion


    }
}