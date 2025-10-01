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
    public class RangeInspectionMasterController : Controller
    {
        // GET: RangeInspectionMaster
        DALRangeInspectionMaster objRIM = new DALRangeInspectionMaster();

        RangeInspectionMaster obj = new RangeInspectionMaster();
        public ActionResult CreateRangeInspectionMaster(int? PK_RangeInspectionId)
        {
            var model = new RangeInspectionMaster();
            #region Bind IAFScope
            List<NameCode> lstIAFScope = new List<NameCode>();
            DataSet dsGetIAFScope = new DataSet();
            dsGetIAFScope = objRIM.GetFieldOfInspection();
            if (dsGetIAFScope.Tables[0].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
            {
                lstIAFScope = (from n in dsGetIAFScope.Tables[0].AsEnumerable()
                               select new NameCode()
                               {
                                   Name = n.Field<string>(dsGetIAFScope.Tables[0].Columns["InspectionName"].ToString()),
                                   Code = n.Field<Int32>(dsGetIAFScope.Tables[0].Columns["PK_FieldInspection"].ToString())

                               }).ToList();
            }
            ViewBag.TitleName = lstIAFScope;
            #endregion
            DataSet dsGetFieldInspection = new DataSet();
            if (PK_RangeInspectionId != null)
            {
                dsGetFieldInspection = objRIM.GetDataById(Convert.ToInt32(PK_RangeInspectionId));
                if (dsGetFieldInspection.Tables[0].Rows.Count > 0)
                {
                    model.PK_RangeInspectionId = Convert.ToInt32(dsGetFieldInspection.Tables[0].Rows[0]["PK_RangeInspectionId"]);
                    model.RangeInspection = dsGetFieldInspection.Tables[0].Rows[0]["RangeInspection"].ToString();
                    model.FK_FieldInspection = dsGetFieldInspection.Tables[0].Rows[0]["FK_FieldInspection"].ToString();
                    model.MinimumEducationQua = dsGetFieldInspection.Tables[0].Rows[0]["MinimumEducationQua"].ToString();
                    model.MinimumRequirmentForLevel3 = dsGetFieldInspection.Tables[0].Rows[0]["MinimumRequirmentForLevel3"].ToString();



                }
                return View(model);
            }
            else
            {
                return View();
            }

            
        }


        [HttpPost]
        public ActionResult CreateRangeInspectionMaster(RangeInspectionMaster S)
        {
            string Result = string.Empty;
            try
            {

                if (S.PK_RangeInspectionId > 0)
                {
                    //Update
                    Result = objRIM.Insert(S);
                }
                else
                {

                    Result = objRIM.Insert(S);
                    if (Convert.ToInt16(Result) > 0)
                    {
                        ModelState.Clear();
                        TempData["message"] = "Record Added Successfully...";
                    }
                    else
                    {
                        TempData["message"] = "Something went Wrong! Please try Again";
                    }
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("ListRangeInspectionMaster", "RangeInspectionMaster");
        }

        [HttpGet]
        public ActionResult ListRangeInspectionMaster()
        {
            List<RangeInspectionMaster> lmd = new List<RangeInspectionMaster>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objRIM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new RangeInspectionMaster
                {
                    PK_RangeInspectionId = Convert.ToInt32(dr["PK_RangeInspectionId"]),
                    RangeInspection = Convert.ToString(dr["RangeInspection"]),
                    FK_FieldInspectionName = Convert.ToString(dr["FK_FieldInspectionName"]),
                    MinimumEducationQua = Convert.ToString(dr["MinimumEducationQua"]),
                    MinimumRequirmentForLevel3 = Convert.ToString(dr["MinimumRequirmentForLevel3"]),

                });
            }

            obj.lmd1 = lmd;
            return View(obj);

        }


        public ActionResult Delete(int? PK_RangeInspectionId)
        {
            string Result = string.Empty;
            try
            {
                Result = objRIM.Delete(Convert.ToInt32(PK_RangeInspectionId));
                if (Convert.ToInt16(Result) > 0)
                {


                    ModelState.Clear();
                }
                else
                {

                    TempData["message"] = "Something went Wrong! Please try Again";
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ModelState.Clear();
            return RedirectToAction("ListRangeInspectionMaster");


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
                IGrid<RangeInspectionMaster> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<RangeInspectionMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                //added by nikita on 13092023
                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "RegEnquiryReportOrderTypeWise-" + formattedDateTime + ".xlsx";

                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<RangeInspectionMaster> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<RangeInspectionMaster> grid = new Grid<RangeInspectionMaster>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            //added by nikita on 13092023
            grid.Columns.Add(model => model.RangeInspection).Titled("PK_Range Inspection Id");
            grid.Columns.Add(model => model.RangeInspection).Titled("Range Inspection");
            grid.Columns.Add(model => model.FK_FieldInspectionName).Titled("Field Inspection Name");
            grid.Columns.Add(model => model.MinimumEducationQua).Titled("Minimum Educational Qualification");
            grid.Columns.Add(model => model.MinimumRequirmentForLevel3).Titled("Minimum Requirement for Level 3");



            grid.Pager = new GridPager<RangeInspectionMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = obj.lmd1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<RangeInspectionMaster> GetData()
        {

            List<RangeInspectionMaster> lmd = new List<RangeInspectionMaster>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objRIM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new RangeInspectionMaster
                {
                    //added by nikita on 13092023
                    Count = ds.Tables[0].Rows.Count,
                    PK_RangeInspectionId = Convert.ToInt32(dr["PK_RangeInspectionId"]),
                    RangeInspection = Convert.ToString(dr["RangeInspection"]),
                    FK_FieldInspectionName = Convert.ToString(dr["FK_FieldInspectionName"]),
                    MinimumEducationQua = Convert.ToString(dr["MinimumEducationQua"]),
                    MinimumRequirmentForLevel3 = Convert.ToString(dr["MinimumRequirmentForLevel3"]),


                });
            }

            obj.lmd1 = lmd;
           
            return obj.lmd1;
        }
        #endregion





    }
}