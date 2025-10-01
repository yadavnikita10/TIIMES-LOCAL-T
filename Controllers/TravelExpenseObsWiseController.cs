using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.Models;
using TuvVision.DataAccessLayer;

namespace TuvVision.Controllers
{
    public class TravelExpenseObsWiseController : Controller
    {

        DalTravelExpensedashboard obj = new DalTravelExpensedashboard();
        Travel Travel1 = new Travel();

        // GET: TravelObsWise
        //public ActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public ActionResult TravelExpensesObsWise()
        {
            Session["FromDatetohold"] = null;
            Session["Todatetohold"] = null;
            Session["UserRoletohold"] = null;
            Session["Employeetohold"] = null;
            DataSet ds = new DataSet();
            ds = obj.MultiEmployeeDropdown();
            List<Travel> lasttravel = new List<Travel>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lasttravel = (from n in ds.Tables[0].AsEnumerable()
                              select new Travel()
                              {
                                  Name = n.Field<string>(ds.Tables[0].Columns["Name"].ToString()),
                                  code = n.Field<int>(ds.Tables[0].Columns["Id"].ToString())

                              }).ToList();
            }
            IEnumerable<SelectListItem> travelexpense;
            travelexpense = new SelectList(lasttravel, "code", "Name");

            ViewBag.SubCatlist = lasttravel;
            ViewData["Employeementcategoray"] = lasttravel;

            Travel1.lstTravel = lasttravel;





            ds = obj.UserRole_();
            List<Travel> lasttravel1 = new List<Travel>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                lasttravel1 = (from n in ds.Tables[0].AsEnumerable()
                               select new Travel()
                               {
                                   Name = n.Field<string>(ds.Tables[0].Columns["Name"].ToString()),
                                   code = n.Field<int>(ds.Tables[0].Columns["Id"].ToString())

                               }).ToList();
            }
            IEnumerable<SelectListItem> travelexpense1;
            travelexpense1 = new SelectList(lasttravel1, "code", "Name");

            ViewBag.UserRole = lasttravel1;
            ViewData["UserRole"] = lasttravel1;

            Travel1.lstUserRole = lasttravel1;

            FormCollection fc = new FormCollection();
            DataTable dt = new DataTable();

            if (Session["FromDate"] != null && Session["ToDate"] != null || Session["Userrole"] !=null || Session["Employement"] !=null)
            {


                Travel1.Fromd = Convert.ToString(Session["FromDate"]);
                Travel1.Tod = Convert.ToString(Session["ToDate"]);
                Travel1.UserRole = Convert.ToString(Session["Userrole"]);
                Travel1.EmpCategoray = Convert.ToString(Session["Employement"]);
                //Session.Keep();
                dt = obj.TravelExpese_obswise_Datewise(Travel1);
            }
            else
            {
                dt = obj.TravelExpese_obswise();

            }

            //return View(Travel1);
            List<Travel> lstOBS = new List<Travel>();

            string showData = string.Empty;
            try
            {

                if (dt.Rows.Count > 0)
                {
                    List<Travel> lstComplaintDashBoard1 = new List<Travel>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        lstComplaintDashBoard1.Add(new Travel
                        {
                            BranchName = Convert.ToString(dr["Branch_Name"]),
                            ptitotal = dr["ptitotal"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ptitotal"]),
                            GrandTotal = dr["GrandTotal_"] == DBNull.Value ? 0 : Convert.ToInt32(dr["GrandTotal_"]),
                            ptiApproved = dr["ptiApproved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ptiApproved"]),
                            ISIM_SP_total = dr["ISIM_SP_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_SP_total"]),
                            ISIM_SP_Approved = dr["ISIM_SP_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_SP_Approved"]),
                            ISIM_total = dr["ISIM_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_total"]),
                            ISIM_Approved = dr["ISIM_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_Approved"]),
                            ISGB_total = dr["ISGB_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGB_total"]),
                            ISGB_Approved = dr["ISGB_Approved"] == DBNull.Value ? 0 : Convert.ToInt32((dr["ISGB_Approved"])),
                            DVS_total = dr["DVS_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DVS_total"]),
                            DVS_Approved = dr["DVS_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DVS_Approved"]),
                            ISET_total = dr["ISET_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISET_total"]),
                            ISET_Approved = dr["ISET_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISET_Approved"]),
                            PCG_total = dr["PCG_total"] == DBNull.Value ? 0 : Convert.ToInt32((dr["PCG_total"])),
                            PCG_Approved = dr["PCG_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PCG_Approved"]),
                            ISGR_total = dr["ISGR_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGR_total"]),
                            ISGR_Approved = dr["ISGR_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGR_Approved"]),
                            PC_total = dr["PC_total"] == DBNull.Value ? 0 : Convert.ToInt32((dr["PC_total"])),
                            PC_Approved = dr["PC_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PC_Approved"]),
                            Totalamount = dr["total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["total"]),
                            Approvedamount = dr["approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["approved"]),

                            //ISETREnewable_Total = dr["GrandTotal_"] == DBNull.Value ? 0 : Convert.ToInt32(dr["GrandTotal_"]),
                            ISETREnewable_Approved = dr["GrandTotal_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["GrandTotal_Approved"]),


                        });
                    }

                    ViewData["ListData"] = lstComplaintDashBoard1;
                    Session["FromDatetohold"] = Travel1.Fromd;
                    Session["Todatetohold"] = Travel1.Tod;
                    Session["UserRoletohold"] = Travel1.UserRole;
                    Session["Employeetohold"] = Travel1.EmpCategoray;

                    //Session["FromDatetohold"] = OBS.FromDate;
                    //Session["Todatetohold"] = OBS.ToDate;
                    //Session["UserRoletohold"] = OBS.UserRole;
                    //Session["Employeetohold"] = OBS.EmployementCategory;


                    Travel grandTotal = new Travel();
                    grandTotal.BranchName = "Grand Total";
                    grandTotal.GrandGrandTotal = lstComplaintDashBoard1.Sum(item => (item.GrandTotal));
                    grandTotal.GrandGrandTotal_Approved = lstComplaintDashBoard1.Sum(item => (item.ISETREnewable_Approved));

                    grandTotal.grandptitotal = lstComplaintDashBoard1.Sum(item => (item.ptitotal));
                    grandTotal.grandptiApproved = lstComplaintDashBoard1.Sum(item => (item.ptiApproved));
                    grandTotal.grandISIM_total = lstComplaintDashBoard1.Sum(item => (item.ISIM_total));
                    grandTotal.grandISIM_Approved = lstComplaintDashBoard1.Sum(item => (item.ISIM_Approved));
                    grandTotal.grandISIM_SP_total = lstComplaintDashBoard1.Sum(item => (item.ISIM_SP_total));
                    grandTotal.grandISIM_SP_Approved = lstComplaintDashBoard1.Sum(item => (item.ISIM_SP_Approved));
                    grandTotal.grandISGB_total = lstComplaintDashBoard1.Sum(item => (item.ISGB_total));
                    grandTotal.grandISGB_Approved = lstComplaintDashBoard1.Sum(item => (item.ISGB_Approved));
                    grandTotal.grandDVS_total = lstComplaintDashBoard1.Sum(item => (item.DVS_total));
                    grandTotal.grandDVS_Approved = lstComplaintDashBoard1.Sum(item => (item.DVS_Approved));
                    grandTotal.grandISET_total = lstComplaintDashBoard1.Sum(item => (item.ISET_total));
                    grandTotal.grandISET_Approved = lstComplaintDashBoard1.Sum(item => (item.ISET_Approved));
                    grandTotal.grandPCG_total = lstComplaintDashBoard1.Sum(item => (item.PCG_total));
                    grandTotal.grandPCG_Approved = lstComplaintDashBoard1.Sum(item => (item.PCG_Approved));
                    grandTotal.grandISGR_total = lstComplaintDashBoard1.Sum(item => (item.ISGR_total));
                    grandTotal.grandISGR_Approved = lstComplaintDashBoard1.Sum(item => (item.ISGR_Approved));
                    grandTotal.grandPC_total = lstComplaintDashBoard1.Sum(item => (item.PC_total));
                    grandTotal.grandPC_Approved = lstComplaintDashBoard1.Sum(item => (item.PC_Approved));
                    grandTotal.grandISETREnewable_Total = lstComplaintDashBoard1.Sum(item => (item.ISETREnewable_Total));
                    grandTotal.grandISETREnewable_Approved = lstComplaintDashBoard1.Sum(item => (item.ISETREnewable_Approved));
                    grandTotal.grandtotal1 = lstComplaintDashBoard1.Sum(item => (item.Totalamount));
                    grandTotal.grandapproved = lstComplaintDashBoard1.Sum(item => (item.Approvedamount));

                    ViewData["GrandTotal"] = grandTotal;
                    List<Travel> modelList = new List<Travel>();
                    modelList.Add(grandTotal);

                    Travel viewModel = new Travel
                    {
                        lstTravel = lstOBS,
                        GrandTotallist = modelList
                    };
                    viewModel.Fromd = Convert.ToString(Session["FromDatetohold"]);
                    viewModel.Tod = Convert.ToString(Session["Todatetohold"]);
                    viewModel.UserRole = Convert.ToString(Session["UserRoletohold"]);
                    viewModel.EmpCategoray = Convert.ToString(Session["Employeetohold"]);

                    //viewModel.Fromd = Session["FromDate"].ToString();
                    // viewModel.Fromd = Session["ToDate"].ToString();
                    return View(viewModel);
                }
            }
            

            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["Userrole"] = null;
            Session["Employement"] = null;
            
            return View();
        }

        [HttpPost]
        public ActionResult TravelExpensesObsWise(Travel TE,FormCollection fc)
        {



            Session["FromDate"] = TE.Fromd;
            Session["ToDate"] = TE.Tod;

            string ProList = string.Join(",", fc["AuditeeName"]);
            TE.UserRole = ProList;
            string ProList1 = string.Join(",", fc["AuditeeName1"]);
            TE.EmpCategoray = ProList1;
            Session["Employement"] = TE.EmpCategoray;
            Session["Userrole"] = TE.UserRole;
            return RedirectToAction("TravelExpensesObsWise");

            //Session["UserRole"] = TE.UserRole;
            //Session["EmpCategoray"] = TE.EmpCategoray;
            //ViewBag.checkAuditee = "Auditee1";
            //ViewBag.SubCatlist = TE.EmpCategoray;
            //ViewBag.checkAuditee = "Auditee";
            //ViewBag.UserRole= TE.UserRole;

            ////RedirectForBinding(TE.UserRole, TE.EmpCategoray);
            ////ViewBag.SubCatlist = ProList;
            ////ViewBag.UserRole = ProList1;
            ////return View(Travel1);
            //DataTable dt = new DataTable();
            //List<Travel> lstOBS = new List<Travel>();
            //dt = obj.TravelExpese_obswise_Datewise(TE);
            //string showData = string.Empty;
            //try
            //{

            //    if (dt.Rows.Count > 0)
            //    {
            //        List<Travel> lstComplaintDashBoard1 = new List<Travel>();

            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            lstComplaintDashBoard1.Add(new Travel
            //            {
            //                BranchName = Convert.ToString(dr["Branch_Name"]),
            //                ptitotal = dr["ptitotal"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ptitotal"]),
            //                //GrandTotal = dr["GrandTotal_"] == DBNull.Value ? 0 : Convert.ToInt32(dr["GrandTotal_"]),
            //                ptiApproved = dr["ptiApproved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ptiApproved"]),
            //                ISIM_SP_total = dr["ISIM_SP_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_SP_total"]),
            //                ISIM_SP_Approved = dr["ISIM_SP_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_SP_Approved"]),
            //                ISIM_total = dr["ISIM_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_total"]),
            //                ISIM_Approved = dr["ISIM_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_Approved"]),
            //                ISGB_total = dr["ISGB_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGB_total"]),
            //                ISGB_Approved = dr["ISGB_Approved"] == DBNull.Value ? 0 : Convert.ToInt32((dr["ISGB_Approved"])),
            //                DVS_total = dr["DVS_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DVS_total"]),
            //                DVS_Approved = dr["DVS_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DVS_Approved"]),
            //                ISET_total = dr["ISET_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISET_total"]),
            //                ISET_Approved = dr["ISET_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISET_Approved"]),
            //                PCG_total = dr["PCG_total"] == DBNull.Value ? 0 : Convert.ToInt32((dr["PCG_total"])),
            //                PCG_Approved = dr["PCG_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PCG_Approved"]),
            //                ISGR_total = dr["ISGR_total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGR_total"]),
            //                ISGR_Approved = dr["ISGR_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGR_Approved"]),
            //                PC_total = dr["PC_total"] == DBNull.Value ? 0 : Convert.ToInt32((dr["PC_total"])),
            //                PC_Approved = dr["PC_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PC_Approved"]),
            //                Totalamount = dr["total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["total"]),
            //                Approvedamount = dr["approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["approved"]),

            //                ISETREnewable_Total = dr["ISETREnewable_Total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISETREnewable_Total"]),
            //                ISETREnewable_Approved = dr["ISETREnewable_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISETREnewable_Approved"]),

            //                SumOfptiISSIm = dr["GrandTotal_"] == DBNull.Value ? 0 : Convert.ToInt32(dr["GrandTotal_"]),
            //                SumOfptiISSImApproved = dr["GrandTotal_Approved"] == DBNull.Value ? 0 : Convert.ToInt32(dr["GrandTotal_Approved"]),

            //            });
            //        }

            //        ViewData["ListData"] = lstComplaintDashBoard1;

            //        //Session["FromDatetohold"] = OBS.FromDate;
            //        //Session["Todatetohold"] = OBS.ToDate;
            //        //Session["UserRoletohold"] = OBS.UserRole;
            //        //Session["Employeetohold"] = OBS.EmployementCategory;


            //        Travel grandTotal = new Travel();
            //        grandTotal.BranchName = "Grand Total";
            //        grandTotal.GrandGrandTotal = lstComplaintDashBoard1.Sum(item => (item.SumOfptiISSIm));
            //        grandTotal.GrandGrandTotal_Approved = lstComplaintDashBoard1.Sum(item => (item.SumOfptiISSImApproved));

            //        grandTotal.grandptitotal = lstComplaintDashBoard1.Sum(item => (item.ptitotal));
            //        grandTotal.grandptiApproved = lstComplaintDashBoard1.Sum(item => (item.ptiApproved));
            //        grandTotal.grandISIM_total = lstComplaintDashBoard1.Sum(item => (item.ISIM_total));
            //        grandTotal.grandISIM_Approved = lstComplaintDashBoard1.Sum(item => (item.ISIM_Approved));
            //        grandTotal.grandISIM_SP_total = lstComplaintDashBoard1.Sum(item => (item.ISIM_SP_total));
            //        grandTotal.grandISIM_SP_Approved = lstComplaintDashBoard1.Sum(item => (item.ISIM_SP_Approved));
            //        grandTotal.grandISGB_total = lstComplaintDashBoard1.Sum(item => (item.ISGB_total));
            //        grandTotal.grandISGB_Approved = lstComplaintDashBoard1.Sum(item => (item.ISGB_Approved));
            //        grandTotal.grandDVS_total = lstComplaintDashBoard1.Sum(item => (item.DVS_total));
            //        grandTotal.grandDVS_Approved = lstComplaintDashBoard1.Sum(item => (item.DVS_Approved));
            //        grandTotal.grandISET_total = lstComplaintDashBoard1.Sum(item => (item.ISET_total));
            //        grandTotal.grandISET_Approved = lstComplaintDashBoard1.Sum(item => (item.ISET_Approved));
            //        grandTotal.grandPCG_total = lstComplaintDashBoard1.Sum(item => (item.PCG_total));
            //        grandTotal.grandPCG_Approved = lstComplaintDashBoard1.Sum(item => (item.PCG_Approved));
            //        grandTotal.grandISGR_total = lstComplaintDashBoard1.Sum(item => (item.ISGR_total));
            //        grandTotal.grandISGR_Approved = lstComplaintDashBoard1.Sum(item => (item.ISGR_Approved));
            //        grandTotal.grandPC_total = lstComplaintDashBoard1.Sum(item => (item.PC_total));
            //        grandTotal.grandPC_Approved = lstComplaintDashBoard1.Sum(item => (item.PC_Approved));
            //        grandTotal.grandISETREnewable_Total = lstComplaintDashBoard1.Sum(item => (item.ISETREnewable_Total));
            //        grandTotal.grandISETREnewable_Approved = lstComplaintDashBoard1.Sum(item => (item.ISETREnewable_Approved));
            //        grandTotal.grandtotal1 = lstComplaintDashBoard1.Sum(item => (item.Totalamount));
            //        grandTotal.grandapproved = lstComplaintDashBoard1.Sum(item => (item.Approvedamount));

            //        ViewData["GrandTotal"] = grandTotal;
            //        List<Travel> modelList = new List<Travel>();
            //        modelList.Add(grandTotal);

            //        Travel viewModel = new Travel
            //        {
            //            lstTravel = lstOBS,
            //            GrandTotallist = modelList
            //        };
            //        return View(viewModel);
            //    }
            //}


            //catch (Exception ex)
            //{
            //    string Error = ex.Message.ToString();
            //}
            //return View();
        }


        //public ActionResult RedirectForBinding(string selectedUserRoleId, string selectedEmployeeId)
        //{
        //    DataSet ds = new DataSet();
        //    ds = obj.MultiEmployeeDropdown();
        //    List<Travel> lasttravel = new List<Travel>();

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        lasttravel = (from n in ds.Tables[0].AsEnumerable()
        //                      select new Travel()
        //                      {
        //                          Name = n.Field<string>(ds.Tables[0].Columns["Name"].ToString()),
        //                          code = n.Field<int>(ds.Tables[0].Columns["Id"].ToString())
        //                      }).ToList();
        //    }

        //    ViewBag.SubCatlist = lasttravel.Select(item => item.code).ToList(); // Store just the IDs

        //    ds = obj.UserRole_();
        //    List<Travel> lasttravel1 = new List<Travel>();
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        lasttravel1 = (from n in ds.Tables[0].AsEnumerable()
        //                       select new Travel()
        //                       {
        //                           Name = n.Field<string>(ds.Tables[0].Columns["Name"].ToString()),
        //                           code = n.Field<int>(ds.Tables[0].Columns["Id"].ToString())
        //                       }).ToList();
        //    }
        //    ViewBag.UserRole = lasttravel1;
        //    // Find the selected user and employee based on their IDs          
        //    return View();
        //}
    }
}