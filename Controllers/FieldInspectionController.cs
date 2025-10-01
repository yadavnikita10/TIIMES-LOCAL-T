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
    public class FieldInspectionController : Controller
    {
        // GET: FieldInspection

        DALFieldInspectionMaster objFIM = new DALFieldInspectionMaster();
        FieldInspectionMaster obj = new FieldInspectionMaster();
        public ActionResult CreateFieldInspection(int? PK_FieldInspection)
        {
            var model = new FieldInspectionMaster();
            #region Bind IAFScope
            List<NameCode> lstIAFScope = new List<NameCode>();
            DataSet dsGetIAFScope = new DataSet();
            dsGetIAFScope = objFIM.GetIAFScope();
            if (dsGetIAFScope.Tables[0].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
            {
                lstIAFScope = (from n in dsGetIAFScope.Tables[0].AsEnumerable()
                                select new NameCode()
                                {
                                    Name = n.Field<string>(dsGetIAFScope.Tables[0].Columns["IAFScopeName"].ToString()),
                                    Code = n.Field<Int32>(dsGetIAFScope.Tables[0].Columns["PK_IAFScopeId"].ToString())

                                }).ToList();
            }
            ViewBag.TitleName = lstIAFScope;
            #endregion
            DataSet dsGetFieldInspection = new DataSet();
            if (PK_FieldInspection != null)
            {
                dsGetFieldInspection = objFIM.GetDataById(Convert.ToInt32(PK_FieldInspection));
                if (dsGetFieldInspection.Tables[0].Rows.Count > 0)
                {
                    model.PK_FieldInspection = Convert.ToInt32(dsGetFieldInspection.Tables[0].Rows[0]["PK_FieldInspection"]);
                    model.InspectionName = dsGetFieldInspection.Tables[0].Rows[0]["InspectionName"].ToString();
                    model.FK_IAFScopeId = dsGetFieldInspection.Tables[0].Rows[0]["FK_IAFScopeId"].ToString();



                }


                return View(model);
            }
            else
            {
                return View();
            }

            
        }



        [HttpPost]
        public ActionResult CreateFieldInspection(FieldInspectionMaster S)
        {
            string Result = string.Empty;
            try
            {

                if (S.PK_FieldInspection > 0)
                {
                    //Update
                    Result = objFIM.Insert(S);
                }
                else
                {

                    Result = objFIM.Insert(S);
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
            return RedirectToAction("ListFieldInspection", "FieldInspection");
        }

        [HttpGet]
        public ActionResult ListFieldInspection()
        {
            List<FieldInspectionMaster> lmd = new List<FieldInspectionMaster>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objFIM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new FieldInspectionMaster
                {
                    PK_FieldInspection = Convert.ToInt32(dr["PK_FieldInspection"]),
                    InspectionName = Convert.ToString(dr["InspectionName"]),
                    IAFScopeName = Convert.ToString(dr["IAFScopeName"]),


                });
            }

            obj.lmd1 = lmd;
            return View(obj);

        }

        public ActionResult Delete(int? PK_FieldInspection)
        {
            string Result = string.Empty;
            try
            {
                Result = objFIM.Delete(Convert.ToInt32(PK_FieldInspection));
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
            return RedirectToAction("ListFieldInspection");


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
                IGrid<FieldInspectionMaster> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<FieldInspectionMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<FieldInspectionMaster> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<FieldInspectionMaster> grid = new Grid<FieldInspectionMaster>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.InspectionName).Titled("Inspection Name");
            grid.Columns.Add(model => model.IAFScopeName).Titled("IAF Scope Number");
            

            grid.Pager = new GridPager<FieldInspectionMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = obj.lmd1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<FieldInspectionMaster> GetData()
        {

            List<FieldInspectionMaster> lmd = new List<FieldInspectionMaster>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objFIM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new FieldInspectionMaster
                {
                    Count = ds.Tables[0].Rows.Count,
                    PK_FieldInspection = Convert.ToInt32(dr["PK_FieldInspection"]),
                    InspectionName = Convert.ToString(dr["InspectionName"]),
                    IAFScopeName = Convert.ToString(dr["IAFScopeName"]),


                });
            }

            obj.lmd1 = lmd;
            return obj.lmd1;
        }
        #endregion




    }
}