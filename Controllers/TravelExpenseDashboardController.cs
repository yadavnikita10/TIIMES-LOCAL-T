using Newtonsoft.Json;
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
    public class TravelExpenseDashboardController : Controller
    {
        // GET: TravelExpenseDashboard
        DalTravelExpensedashboard obj = new DalTravelExpensedashboard();

        TravelExpense TravelExpense1 = new TravelExpense();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TravelExpenseDashboard()
        {
            string result;
            try
            {
                DataSet ds = new DataSet();
                ds = obj.MultiEmployeeDropdown();
                List<TravelExpense> lasttravel = new List<TravelExpense>();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lasttravel = (from n in ds.Tables[0].AsEnumerable()
                                  select new TravelExpense()
                                  {
                                      Name = n.Field<string>(ds.Tables[0].Columns["Name"].ToString()),
                                      code = n.Field<int>(ds.Tables[0].Columns["Id"].ToString())

                                  }).ToList();
                }
                IEnumerable<SelectListItem> travelexpense;
                travelexpense = new SelectList(lasttravel, "code", "Name");

                ViewBag.SubCatlist = lasttravel;
                ViewData["Employeementcategoray"] = lasttravel;

                TravelExpense1.lstTravelExpense = lasttravel;





                ds = obj.UserRole_();
                List<TravelExpense> lasttravel1 = new List<TravelExpense>();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lasttravel1 = (from n in ds.Tables[0].AsEnumerable()
                                  select new TravelExpense()
                                  {
                                      Name = n.Field<string>(ds.Tables[0].Columns["Name"].ToString()),
                                      code = n.Field<int>(ds.Tables[0].Columns["Id"].ToString())

                                  }).ToList();
                }
                IEnumerable<SelectListItem> travelexpense1;
                travelexpense1 = new SelectList(lasttravel1, "code", "Name");

                ViewBag.UserRole = lasttravel1;
                ViewData["UserRole"] = lasttravel1;

                TravelExpense1.lstUserRole = lasttravel1;

                return View(TravelExpense1);
            }

            catch (Exception Ex)
            {
                //return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
            return View(TravelExpense1);
        }


        public ActionResult TravelExpense()
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.Piechart();

                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);



            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult TravelExpensePieChart()
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.ExpensePiechart();

                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);



            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult TravelExpenseFilterPieChart(string costcenter, string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.FilterExpensePiechart(costcenter,Fromdate,Todate,EmployeementCategoray,UserRole);

                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult TravelExpenseBranchwise()
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = obj.BranchPiechart();
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult TravelExpenseEmployeeWise()
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = obj.Employeedata();
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult TravelExpenseEmployeecategary()
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = obj.Employeementpiedata();
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult ExpenseItemTypePieChart()
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = obj.Typepiedata();
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult BindingTotaldata()
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.ToatlData();
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BindingFirstTable(string costcenter, string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.FilterFirsttable(costcenter, Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BindingSecondTable(string costcenter, string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.FilterSecondtable(costcenter, Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        //15102023
        public ActionResult filterBranchTableOnClick(string Pk_userid)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filterBranchTableOnClick(Pk_userid);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult FilterEmployemnetPie(string costcenter,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.FilterEmplyoment(costcenter,Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult FilterTypePie(string costcenter, string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.FilterTypePie(costcenter, Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult filteringcostcentertableonEmployeeclick(string costcenter, string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filtercostcenteronEmployeeClick(costcenter, Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult filteringExpenseTypeonEmployeeclick(string Pk_UserrId,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filterEmployeeExpenseTypeonEmployeeClick(Pk_UserrId,Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult filteringTypeonEmployeeclick(string Pk_UserrId,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filterEmployeeTypeonEmployeeClick(Pk_UserrId, Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult filteringEmployeementTypeonEmployeeclick(string Pk_UserrId,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filterEmployeementTypeonEmployeeClick(Pk_UserrId, Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult filterCostcenteronBranchClick(string BranchCode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filterCostcenteronBranchClick(BranchCode,Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult filterEmployeeonBranchClick(string BranchCode,string Fromdate,string Todate,string EmployeementCategoray,string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filterEmployeeonBranchClick(BranchCode,Fromdate,Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult filterTotalAmountonBranchClick(string BranchCode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filterTotalAmountonBranchClick(BranchCode,Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult filterExpenseBranchClick(string BranchCode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filterExpenseBranchClick_(BranchCode,Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult filterEmployementBranchClick(string BranchCode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filterEmployementBranchClick_(BranchCode,Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult filterTypeBranchClick(string BranchCode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            try
            {
                DataTable dt = new DataTable();
                dt = obj.filterTypeBranchClick_(BranchCode,Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MultiEmployeeDropdown()
        {
            string result;
            try
            {
                DataSet ds = new DataSet();
                ds = obj.MultiEmployeeDropdown();
                List<TravelExpense> lasttravel = new List<TravelExpense>();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lasttravel = (from n in ds.Tables[0].AsEnumerable()
                                      select new TravelExpense()
                                      {
                                          Name = n.Field<string>(ds.Tables[0].Columns["Name"].ToString()),
                                          code = n.Field<int>(ds.Tables[0].Columns["Id"].ToString())

                                      }).ToList();
                }
                IEnumerable<SelectListItem> travelexpense;
                travelexpense = new SelectList(lasttravel, "code", "Name");

                ViewBag.SubCatlist = lasttravel;
                ViewData["Employeementcategoray"] = lasttravel;

                TravelExpense1.lstTravelExpense = lasttravel;
            
                return View(TravelExpense1);
            }

            catch (Exception Ex)
            {
                //return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
            return View(TravelExpense1);

        }

        public ActionResult DatewiseCosecenterfilter(string Fromdate,string Todate, string EmployeementCategoray,string UserRole)
        {
            string result;
            //string[] d = UserRole.Split(',');
            //string d1 = string.Empty;
            //for(int i=0;i< d.Length;i++)
            //{


            //    d1 += "\'" + d[i] + "\'";
            //    if (i < d.Length - 1)
            //    {
            //        d1 += ",";
            //    }
            //    //d1 += d[i];
            //    //if (i < d.Length - 1)
            //    //{
            //    //    d1 += ",";
            //    //}
            //}
            ////d1 = d1.TrimEnd(',');
        
            //string[] d2 = EmployeementCategoray.Split(',');
            //string d3 = string.Empty;
            //for (int i = 0; i < d2.Length; i++)
            //{
            //    //d3 += d2[i] + ",";

            //    d3 += "\'" + d2[i] + "\'";
            //    if (i < d2.Length - 1)
            //    {
            //        d3 += ",";
            //    }
            //    //d3 += d2[i];
            //    //if (i < d.Length - 1)
            //    //{
            //    //    d3 += ",";
            //    //}
            //}
            ////d3 = d3.TrimEnd(',');
            try
            {
                DataTable dt = new DataTable();
                dt = obj.Datewisecostercenter(Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DatewiseBranchfilter(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {


            
            string result;
           
            try
            {
                DataTable dt = new DataTable();
                dt = obj.DatewiseBranch(Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DatewiseEmployeefilter(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
           
            try
            {
                DataTable dt = new DataTable();
                dt = obj.DatewiseEmployee(Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DatewiseExpensefilter(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            
            try
            {
                DataTable dt = new DataTable();
                dt = obj.DatewiseExpense(Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DatewiseTypefilter(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
           
            try
            {
                DataTable dt = new DataTable();
                dt = obj.DatewiseType(Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DatewiseEmployeementfilter(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            
            try
            {
                DataTable dt = new DataTable();
                dt = obj.DatewiseEmployeement(Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DatewiseTotal(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            string result;
            
            try
            {
                DataTable dt = new DataTable();
                dt = obj.DatewiseTotal(Fromdate, Todate, EmployeementCategoray, UserRole);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }
    }
}