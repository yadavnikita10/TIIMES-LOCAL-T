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
    public class BranchTargetController : Controller
    {
        DateTime startdate; //value coming from db
        DateTime enddate; //value coming from db
        BranchTarget objBranchTarget = new BranchTarget();
        DALBranchTarget objDAL = new DALBranchTarget();

        // GET: BranchTarget
        public ActionResult BranchTarget(string Year, string Month, string ServiceCode)
        {
            DALUsers objDalCreateUser = new DALUsers();
            var Data1 = objDalCreateUser.GetCostCenterList();
            //ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");


            var Data2 = objDAL.GetOBSType();
            ViewBag.SubCatlist = new SelectList(Data2, "PK_ID", "PortfolioName");
            //ViewBag.SubCatlist = new SelectList(Data2, "groupID", "groupName");



            #region Bind Financial Year

            DateTime curDate = DateTime.Now;




            int CurrentYear = curDate.Year;
            int PreviousYear = (curDate.Year - 1);
            int NextYear = (curDate.Year + 1);
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = string.Empty;
            if (curDate.Month > 3)
            {
                FinYear = CurYear + "-" + NexYear;
            }
            else
            {
                FinYear = PreYear + "-" + CurYear;
            }

            ViewBag.VYear = FinYear;
            ViewBag.VYear = new SelectList(FinYear, "PK_UserID", "FirstName");
            #endregion



            #region chk if data present by Cost Center
            List<BranchTarget> lmd = new List<BranchTarget>();  // creating list of model.  
            DataSet ds = new DataSet();



            ds = objDAL.GetDataOnCostCenterSelection(Year, ServiceCode); // fill dataset  
            objBranchTarget.Year = Year;//ds.Tables[0].Rows[0]["Year"].ToString();
            objBranchTarget.Month = Month;//ds.Tables[0].Rows[0]["Month"].ToString();
            objBranchTarget.ServiceCode = ServiceCode;

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {


                lmd.Add(new BranchTarget
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Branch = Convert.ToString(dr["Branch"]),
                    BranchId = Convert.ToInt32(dr["BranchId"]),
                    OrderBookingTarget = Convert.ToString(dr["OrderBookingTarget"]),
                    InvoicingTarget = Convert.ToString(dr["InvoicingTarget"]),
                    CostTarget = Convert.ToString(dr["CostTarget"]),
                    CollectionTarget = Convert.ToString(dr["CollectionTarget"]),

                    Outstanding = Convert.ToString(dr["Outstanding"]),
                    DSO = Convert.ToString(dr["DSO"]),
                    ActualSales = Convert.ToString(dr["ActualSales"]),
                    Actual_EBIT = Convert.ToString(dr["Actual_EBIT"]),
                    Actual_Cost_SalesEBIT = Convert.ToString(dr["Actual_Cost_SalesEBIT"]),
                    ActualCost = Convert.ToString(dr["ActualCost"]),
                    ActualInvoicing = Convert.ToString(dr["ActualInvoicing"]),
                    ActualCollection = Convert.ToString(dr["ActualCollection"]),

                });
            }
            // return View(lmd.ToList());
            //ViewBag.BranchTarget = null;
            //  lmd = null;


            ViewBag.BranchTarget = lmd;
            #endregion

            #region chk if data present by month
            //List<BranchTarget> lmd = new List<BranchTarget>();  // creating list of model.  
            //DataSet ds = new DataSet();

            //ds = objDAL.GetDataOnMonthSelection(Year, Month); // fill dataset  
            //objBranchTarget.Year = Year;//ds.Tables[0].Rows[0]["Year"].ToString();
            //objBranchTarget.Month = Month;//ds.Tables[0].Rows[0]["Month"].ToString();

            //foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            //{


            //    lmd.Add(new BranchTarget
            //    {
            //        Branch = Convert.ToString(dr["Branch"]),
            //        BranchId = Convert.ToInt32(dr["BranchId"]),
            //        OrderBookingTarget = Convert.ToString(dr["OrderBookingTarget"]),
            //        InvoicingTarget = Convert.ToString(dr["InvoicingTarget"]),
            //        CostTarget = Convert.ToString(dr["CostTarget"]),
            //        CollectionTarget = Convert.ToString(dr["CollectionTarget"]),



            //    });
            //}

            //ViewBag.BranchTarget = lmd;
            #endregion



            //RedirectToAction("lmd")
            //return RedirectToAction("BranchTarget", "BranchTarget", new { Year = Year, ServiceCode = ServiceCode });
            return View();
        }

        [HttpPost]
        public ActionResult BranchTarget(BranchTarget objBranchTarget, string Year, string Month)
        {
            string Result = string.Empty;

            #region
            //try
            //{

            //    if (objBranchTarget.Id > 0)
            //    {
            //        //Update
            //        Result = objDAL.Insert(objBranchTarget);
            //    }
            //    else
            //    {

            //        Result = objDAL.Insert(objBranchTarget);
            //        if (Convert.ToInt16(Result) > 0)
            //        {
            //            ModelState.Clear();
            //            TempData["message"] = "Record Added Successfully";
            //        }
            //        else
            //        {
            //            TempData["message"] = "Error";
            //        }
            //    }


            //}
            //catch (Exception ex)
            //{
            //    string Error = ex.Message.ToString();
            //}
            #endregion

            foreach (var item in objBranchTarget.BTarget)
            {
                objBranchTarget.Branch = item.Branch;
                objBranchTarget.BranchId = item.BranchId;
                objBranchTarget.ServiceCode = objBranchTarget.ServiceCode;
                objBranchTarget.OrderBookingTarget = item.OrderBookingTarget;
                objBranchTarget.InvoicingTarget = item.InvoicingTarget;
                objBranchTarget.CostTarget = item.CostTarget;
                objBranchTarget.CollectionTarget = item.CollectionTarget;

                objBranchTarget.Outstanding = item.Outstanding;
                objBranchTarget.DSO = item.DSO;
                objBranchTarget.ActualSales = item.ActualSales;
                objBranchTarget.Actual_EBIT = item.Actual_EBIT;
                objBranchTarget.Actual_Cost_SalesEBIT = item.Actual_Cost_SalesEBIT;
                objBranchTarget.ActualCost = item.ActualCost;
                objBranchTarget.ActualInvoicing = item.ActualInvoicing;
                objBranchTarget.ActualCollection = item.ActualCollection;

                if (item.Id > 0)
                {
                    objBranchTarget.Id = item.Id;
                    Result = objDAL.Insert(objBranchTarget);
                }
                else
                {
                    Result = objDAL.Insert(objBranchTarget);
                }
            }


            return RedirectToAction("BranchTarget", "BranchTarget", new { Year = Year, ServiceCode = objBranchTarget.ServiceCode });
        }


        [HttpGet]
        public ActionResult ListBranchMaster()
        {
            //List<BranchTarget> lmd = new List<BranchTarget>();  // creating list of model.  
            //DataSet ds = new DataSet();

            //ds = objDAL.GetData(); // fill dataset  

            //foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            //{
            //    lmd.Add(new BranchTarget
            //    {
            //        PK_IAFScopeId = Convert.ToInt32(dr["PK_IAFScopeId"]),
            //        IAFScopeName = Convert.ToString(dr["IAFScopeName"]),
            //        IAFScopeNumber = Convert.ToString(dr["IAFScopeNumber"]),


            //    });
            //}
            //return View(lmd.ToList());
            return View();
        }


        private static List<SelectListItem> PopulateMonths()
        {
            List<SelectListItem> months = new List<SelectListItem>();
            months.Add(new SelectListItem { Value = "1", Text = "January" });
            months.Add(new SelectListItem { Value = "2", Text = "February" });
            months.Add(new SelectListItem { Value = "3", Text = "March" });
            months.Add(new SelectListItem { Value = "4", Text = "April" });
            months.Add(new SelectListItem { Value = "5", Text = "May" });
            months.Add(new SelectListItem { Value = "6", Text = "June" });
            months.Add(new SelectListItem { Value = "7", Text = "July" });
            months.Add(new SelectListItem { Value = "8", Text = "August" });
            months.Add(new SelectListItem { Value = "9", Text = "September" });
            months.Add(new SelectListItem { Value = "10", Text = "October" });
            months.Add(new SelectListItem { Value = "11", Text = "November" });
            months.Add(new SelectListItem { Value = "12", Text = "December" });

            return months;
        }


        [HttpGet]
        public ActionResult GetDataMonthSelection(string Year, string Month)
        {
            string Result = string.Empty;

            return View();
        }



        [HttpPost]
        public ActionResult GetDataMonthSelection(BranchTarget objBranchTarget, string Year, string Month, string CostCenter)
        {
            string Result = string.Empty;
            #region chk if data present by month
            List<BranchTarget> lmd = new List<BranchTarget>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objDAL.GetDataOnMonthSelection(Year, Month); // fill dataset  
            objBranchTarget.Year = Year;//ds.Tables[0].Rows[0]["Year"].ToString();
            objBranchTarget.Month = Month;//ds.Tables[0].Rows[0]["Month"].ToString();

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {


                lmd.Add(new BranchTarget
                {
                    OrderBookingTarget = Convert.ToString(dr["OrderBookingTarget"]),
                    InvoicingTarget = Convert.ToString(dr["InvoicingTarget"]),
                    CostTarget = Convert.ToString(dr["CostTarget"]),
                    CollectionTarget = Convert.ToString(dr["CollectionTarget"]),



                });
            }
            // return View(lmd.ToList());

            ViewBag.BranchTarget = lmd;
            #endregion
            return RedirectToAction("BranchTarget", new { Year = Year, Month = Month });
        }

        [HttpPost]
        public ActionResult GetDataCostCenterSelection(BranchTarget objBranchTarget, string Year, string Month, string CostCenter)
        {
            string Result = string.Empty;
            #region chk if data present by Cost Center
            List<BranchTarget> lmd = new List<BranchTarget>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objDAL.GetDataOnCostCenterSelection(Year, CostCenter); // fill dataset  
            objBranchTarget.Year = Year;//ds.Tables[0].Rows[0]["Year"].ToString();
            objBranchTarget.Month = Month;//ds.Tables[0].Rows[0]["Month"].ToString();

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {


                lmd.Add(new BranchTarget
                {
                    OrderBookingTarget = Convert.ToString(dr["OrderBookingTarget"]),
                    InvoicingTarget = Convert.ToString(dr["InvoicingTarget"]),
                    CostTarget = Convert.ToString(dr["CostTarget"]),
                    CollectionTarget = Convert.ToString(dr["CollectionTarget"]),



                });
            }
            // return View(lmd.ToList());

            ViewBag.BranchTarget = lmd;
            #endregion

            //string a = "< a href = "@Url.Action("Index","About")" > About </ a >";// @Url.Action("BranchTarget", "BranchTarget",new {Year:Year,ServiceCode = CostCenter  });
            // return Json(new { success = 3, responseText = "Code mathched" }, JsonRequestBehavior.AllowGet);

            return Json(new { result = "Redirect", url = Url.Action("BranchTarget", "BranchTarget", new { @Year = Year, @ServiceCode = CostCenter }) });
            //  return RedirectToAction("BranchTarget", new { Year = Year, ServiceCode = CostCenter });
        }


        public ActionResult IndividualTarget(string Year)

        {
            #region Bind Branch Wise Employee
            List<IndividualTarget> lmd = new List<IndividualTarget>();  // creating list of model.  
            DataSet ds = new DataSet();
            string BranchId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            ds = objDAL.GetIndividualBranchWiseData(BranchId,Year);

            if(ds.Tables[0].Rows.Count > 0)
            {
                   foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                   {

                        lmd.Add(new IndividualTarget
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            BranchId = Convert.ToString(dr["BranchId"]),
                            Branch = Convert.ToString(dr["BranchName"]),
                            EmployeeName= Convert.ToString(dr["Name"]),
                            EmployeeId = Convert.ToString(dr["EmployeeId"]),
                            OrderBookingTarget =  Convert.ToString(dr["OrderBookingTarget"]),
                            MarketingVisits = Convert.ToString(dr["MarketingVisits"]),
                            RoleName = Convert.ToString(dr["RoleName"]),
                            ReportingPersonTwo = Convert.ToString(dr["UReporting2"]),


                        });
                    }
            }
            ViewBag.IndividualTarget = lmd;
            #endregion

            return View();
        }

        [HttpPost]
        public ActionResult IndividualTarget(string Year, string Month,IndividualTarget objIndividualTarget)
        {
            string Result = string.Empty;
            #region Insert Update
            foreach (var item in objIndividualTarget.ITarget)
            {
                objIndividualTarget.Year = objIndividualTarget.Year;
                objIndividualTarget.EmployeeName = item.EmployeeName;
                objIndividualTarget.EmployeeId = item.EmployeeId;
                objIndividualTarget.BranchId = item.BranchId;
                objIndividualTarget.OrderBookingTarget = item.OrderBookingTarget;
                objIndividualTarget.MarketingVisits = item.MarketingVisits;


                if (item.Id > 0)
                {
                    objIndividualTarget.Id = item.Id;
                    Result = objDAL.InsertIndividualTarget(objIndividualTarget);
                }
                else
                {
                    Result = objDAL.InsertIndividualTarget(objIndividualTarget);
                }
            }

            #endregion  
            return RedirectToAction("IndividualTarget", "BranchTarget", new { Year = Year });
        }


        [HttpPost]
        public ActionResult GetIndividualDataByBranch(BranchTarget objBranchTarget, string Year, string Month, string CostCenter)
        {
            string Result = string.Empty;
           
            return Json(new { result = "Redirect", url = Url.Action("IndividualTarget", "BranchTarget", new { @Year = Year }) });
            
        }

    }
}