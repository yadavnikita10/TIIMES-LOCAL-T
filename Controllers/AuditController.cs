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
    public class AuditController : Controller
    {
        DALAudit objDALAudit = new DALAudit();
        Audit model = new Audit();
        DALTrainingSchedule objDTS = new DALTrainingSchedule();

        // GET: Audit
        public ActionResult Audit(int? AuditId)
        {

            model.AuditStandard = "ISO 9001:2015 & ISO 17020:2012";

            #region Generate Unique no
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            int a = _rdm.Next(_min, _max);
            model.AuditNumber = "IAR" + Convert.ToString(a);
            //objM.TiimesUIN = T;
            #endregion

            //model.AuditNumber = "ISO 9001:2015 & ISO 17020:2012";


            DataSet dss = new DataSet();

            //added by nikita on 11012023
            #region Bind OBS Type    
            List<NameCode> lstProjectType = new List<NameCode>();
            List<NameCode> lstScope = new List<NameCode>();
            DataSet DSGetAllddllst = new DataSet();
            DataSet dtIAFScope = new DataSet();
            DataSet DSEditGetList = new DataSet();

            List<NameCode> lstEditBranchList = new List<NameCode>();
            List<NameCode> lstEditUserList = new List<NameCode>();
            List<NameCode> lstEmploymentCategory = new List<NameCode>();
            List<NameCode> listTrainCate = new List<NameCode>();

            DSEditGetList = objDTS.GetDdlLst();

            //// OBS Types
            if (DSEditGetList.Tables[0].Rows.Count > 0)
            {
                lstProjectType = (from n in DSEditGetList.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DSEditGetList.Tables[0].Columns["ProjectName"].ToString()),
                                      Code = n.Field<Int32>(DSEditGetList.Tables[0].Columns["PK_ID"].ToString())

                                  }).ToList();
            }
            IEnumerable<SelectListItem> OBSTypeItems;
            OBSTypeItems = new SelectList(lstProjectType, "Code", "Name");

            ViewBag.OBSType = lstProjectType;
            ViewData["OBSType"] = OBSTypeItems;

            #endregion

            #region Bind branch name
            DataSet dsBindBranch = new DataSet();
            List<BranchName> lstBranch = new List<BranchName>();
            dsBindBranch = objDALAudit.BindBranch();

            if (dsBindBranch.Tables[0].Rows.Count > 0)
            {
                lstBranch = (from n in dsBindBranch.Tables[0].AsEnumerable()
                             select new BranchName()
                             {
                                 Name = n.Field<string>(dsBindBranch.Tables[0].Columns["Branch_Name"].ToString()),
                                 Code = n.Field<Int32>(dsBindBranch.Tables[0].Columns["Br_Id"].ToString())

                             }).ToList();
            }

            IEnumerable<SelectListItem> BranchItems;
            BranchItems = new SelectList(lstBranch, "Code", "Name");
            ViewBag.ProjectTypeItems = BranchItems;
            ViewData["BranchName"] = BranchItems;
            #endregion

            #region Bind Auditor Name
            DataSet dsAuditorName = new DataSet();
            //List<AuditorName> lstAuditorNamee = new List<AuditorName>();
            List<Audit> lstAuditorNamee = new List<Audit>();
            dsAuditorName = objDALAudit.BindAuditorName();

            if (dsAuditorName.Tables[0].Rows.Count > 0)
            {
                lstAuditorNamee = (from n in dsAuditorName.Tables[0].AsEnumerable()
                             select new Audit()
                             {
                                 DAuditorName = n.Field<string>(dsAuditorName.Tables[0].Columns["Name"].ToString()),
                                 DAuditorCode = n.Field<string>(dsAuditorName.Tables[0].Columns["Code"].ToString())

                             }).ToList();
            }

            IEnumerable<SelectListItem> AuditorName;
            AuditorName = new SelectList(lstAuditorNamee, "DAuditorCode", "DAuditorName");
            ViewBag.AuditorName = AuditorName;
            ViewData["AuditorName"] = AuditorName;
            ViewData["AuditorName"] = lstAuditorNamee;
            #endregion


            if (AuditId != null)
            {
                ViewBag.checkAuditee = "Auditee";
                ViewBag.check = "AuditorName";
                string[] splitedProduct_Name;
                string[] splitedAuditorName;

                dss = objDALAudit.GetDataById(Convert.ToInt32(AuditId));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    
                    model.AuditId = Convert.ToInt32(dss.Tables[0].Rows[0]["AuditId"]);
                    model.Branch = dss.Tables[0].Rows[0]["Branch"].ToString();
                    model.TypeOfAudit = dss.Tables[0].Rows[0]["TypeOfAudit"].ToString();
                    model.AuditStandard = dss.Tables[0].Rows[0]["AuditStandard"].ToString();
                    model.AuditorName = dss.Tables[0].Rows[0]["AuditorName"].ToString();
                    model.ExAuditor = dss.Tables[0].Rows[0]["ExAuditor"].ToString();
                    model.Auditee = dss.Tables[0].Rows[0]["Auditee"].ToString();
                    //model.ProposeDate = dss.Tables[0].Rows[0].IsNull("ProposeDate") ? (DateTime?)null : (DateTime?)dss.Tables[0].Rows[0]["ProposeDate"];
                    //model.ScheduleDate = dss.Tables[0].Rows[0].IsNull("ScheduleDate") ? (DateTime?)null : (DateTime?)dss.Tables[0].Rows[0]["ScheduleDate"];

                    model.ProposeDate = dss.Tables[0].Rows[0]["ProposeDate"].ToString();
                    model.ScheduleDate = dss.Tables[0].Rows[0]["ScheduleDate"].ToString();

                    model.Remark = dss.Tables[0].Rows[0]["Remark"].ToString();
                    ViewData["OBSTypechecked"] = dss.Tables[0].Rows[0]["CostCenter"].ToString();  // added by nikita on 10012024


                    List<string> Selected = new List<string>();
                    var Existingins = Convert.ToString(dss.Tables[0].Rows[0]["Auditee"]);
                    splitedProduct_Name = Existingins.Split(',');
                    foreach (var single in splitedProduct_Name)
                    {
                        Selected.Add(single);
                    }
                    ViewBag.EditproductName = Selected;

                    List<string> SelectedAuditorName = new List<string>();
                    var EAuditorName = Convert.ToString(dss.Tables[0].Rows[0]["AuditorName"]);
                    splitedAuditorName = EAuditorName.Split(',');
                    foreach (var single1 in splitedAuditorName)
                    {
                        SelectedAuditorName.Add(single1);
                    }
                    ViewBag.AuditorNameName = SelectedAuditorName;



                }
                return View(model);
            }
            else
            {
                return View(model);
            }



           
        }



        [HttpPost]
        public ActionResult Audit(Audit S, FormCollection fc)
        {
            string ProList = string.Join(",", fc["AuditeeName"]);
            S.Auditee = ProList;
            string StrAuditor = string.Join(",", fc["AuditorName"]);
            S.AuditorName = StrAuditor;
            string Costcenter = string.Join(",", fc["costcenter"]);
            S.CostCenter = Costcenter;
            string Result = string.Empty;
            try
            {

                if (S.AuditId > 0)
                {
                    //Update
                    Result = objDALAudit.Insert(S);
                }
                else
                {

                    Result = objDALAudit.Insert(S);
                    if (Convert.ToInt16(Result) > 0)
                    {
                        ModelState.Clear();
                        TempData["message"] = "Record Added Successfully";
                    }
                    else
                    {
                        TempData["message"] = "Error";
                    }
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("ListAudit", "Audit");
        }


        [HttpGet]
        public ActionResult ListAudit()
        {
            List<Audit> lmd = new List<Audit>();  // creating list of model.  
            DataSet ds = new DataSet();


           


            ds = objDALAudit.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                //DateTime sqlFormattedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ScheduleDate"]);

                //DateTime a = sqlFormattedDate.HasValue ? sqlFormattedDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "<not available>";

               // DateTime? myDate = Convert.ToDateTime(dr["ScheduleDate"]);
                DateTime? myDate = dr.IsNull("ScheduleDate") ? (DateTime?)null : (DateTime?)dr["ScheduleDate"];

                //string sqlFormattedDate = myDate.Value.ToString("yyyy-MM-dd");

                string sqlFormattedDate = myDate.HasValue ? myDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "0001-01-01";
                string SStatus = "";
                if (sqlFormattedDate== "0001-01-01")
                {
                     SStatus = "Not Schedule";
                }
                else
                {
                     SStatus = "Scheduled";
                }
                lmd.Add(new Audit
                {

                    AuditId = Convert.ToInt32(dr["AuditId"]),
                    Branch = Convert.ToString(dr["Branch"]),
                    TypeOfAudit = Convert.ToString(dr["TypeOfAudit"]),
                    AuditStandard = Convert.ToString(dr["AuditStandard"]),
                    AuditorName = Convert.ToString(dr["AuditorName"]),
                    ExAuditor=Convert.ToString(dr["ExAuditor"]),
                    Auditee = Convert.ToString(dr["Auditee"]),
                    //ProposeDate = dr.IsNull("ProposeDate") ? (DateTime?)null : (DateTime?)dr["ProposeDate"],
                    ProposeDate = Convert.ToString(dr["ProposeDate"]),
                    // ScheduleDate = Convert.ToDateTime(sqlFormattedDate),//dr.IsNull("ScheduleDate") ? (DateTime?)null : (DateTime?)dr["ScheduleDate"],
                    //ScheduleDate = Convert.ToDateTime(sqlFormattedDate),
                    ScheduleDate = Convert.ToString(dr["ScheduleDate"]),
                    ActualAuditDateFrom = Convert.ToString(dr["ActualAuditDateFrom"]),
                    ActualAuditDateTo = Convert.ToString(dr["ActualAuditDateTo"]),
                    TotalFindings = Convert.ToString(dr["TotalFindings"]),
                    AreFindingsClose = Convert.ToString(dr["AreFindingsClose"]),
                    PDF = Convert.ToString(dr["PDF"]),
                    //Remark          =Convert.ToString(dr["Remark"]),
                    Status = SStatus,
                    SActualDateFrom = Convert.ToString(dr["ActualAuditDateFrom1"]),
                    SActualDateTo = Convert.ToString(dr["ActualAuditDateTo1"]),
                    SProposeDateFrom = Convert.ToString(dr["ProposeDate1"]),
                    SProposeDateTo = Convert.ToString(dr["ScheduleDate1"]),
                    AuditNumber = Convert.ToString(dr["AuditNumber"]),

                });
            }

            model.lmd1 = lmd;
              return View(model);
            //return View(lmd.ToList());

        }


        public ActionResult Delete(int? AuditId)
        {
            string Result = string.Empty;
            try
            {
                Result = objDALAudit.Delete(Convert.ToInt32(AuditId));
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
            return RedirectToAction("ListAudit");


        }

        #region export to excel
        [HttpGet]
        public ActionResult ExportIndex()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<Audit> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<Audit> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<Audit> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<Audit> grid = new Grid<Audit>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(model => model.AuditId).Titled("AuditId");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.TypeOfAudit).Titled("Type Of Audit");
            grid.Columns.Add(model => model.AuditStandard).Titled("Audit Standard");
            grid.Columns.Add(model => model.AuditorName).Titled("Auditor Name");
            grid.Columns.Add(model => model.ExAuditor).Titled("External Auditor");
            grid.Columns.Add(model => model.Auditee).Titled("Auditee");
            grid.Columns.Add(model => model.ProposeDate).Titled("Propose Date (From)");
            grid.Columns.Add(model => model.ScheduleDate).Titled("Propose Date (To)");
            grid.Columns.Add(model => model.Remark).Titled("Remark");
            grid.Columns.Add(model => model.Status).Titled("Status");
            

            grid.Pager = new GridPager<Audit>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = model.lmd1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<Audit> GetData()
        {

            List<Audit> lmd = new List<Audit>();  // creating list of model.  
            DataSet ds = new DataSet();





            ds = objDALAudit.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                //DateTime sqlFormattedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ScheduleDate"]);

                //DateTime a = sqlFormattedDate.HasValue ? sqlFormattedDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "<not available>";

                // DateTime? myDate = Convert.ToDateTime(dr["ScheduleDate"]);
                DateTime? myDate = dr.IsNull("ScheduleDate") ? (DateTime?)null : (DateTime?)dr["ScheduleDate"];

                //string sqlFormattedDate = myDate.Value.ToString("yyyy-MM-dd");

                string sqlFormattedDate = myDate.HasValue ? myDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "0001-01-01";
                string SStatus = "";
                if (sqlFormattedDate == "0001-01-01")
                {
                    SStatus = "Not Schedule";
                }
                else
                {
                    SStatus = "Schedule";
                }
                lmd.Add(new Audit
                {

                    AuditId = Convert.ToInt32(dr["AuditId"]),
                    Branch = Convert.ToString(dr["Branch"]),
                    TypeOfAudit = Convert.ToString(dr["TypeOfAudit"]),
                    AuditStandard = Convert.ToString(dr["AuditStandard"]),
                    AuditorName = Convert.ToString(dr["AuditorName"]),
                    ExAuditor = Convert.ToString(dr["ExAuditor"]),
                    Auditee = Convert.ToString(dr["Auditee"]),
                    //ProposeDate = dr.IsNull("ProposeDate") ? (DateTime?)null : (DateTime?)dr["ProposeDate"],
                    ProposeDate = Convert.ToString(dr["ProposeDate"]),
                    // ScheduleDate = Convert.ToDateTime(sqlFormattedDate),//dr.IsNull("ScheduleDate") ? (DateTime?)null : (DateTime?)dr["ScheduleDate"],
                    //ScheduleDate = Convert.ToDateTime(sqlFormattedDate),
                    ScheduleDate = Convert.ToString(dr["ScheduleDate"]),
                    

                    Remark = Convert.ToString(dr["Remark"]),
                    Status = SStatus

                });
            }

            model.lmd1 = lmd;


            return model.lmd1;
        }
        #endregion


    }
}