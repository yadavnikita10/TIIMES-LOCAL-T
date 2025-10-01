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
    public class GradeMasterController : Controller
    {

        DALGradeMaster objGM = new DALGradeMaster();
        GradeMaster obj = new GradeMaster(); 
        // GET: GradeMaster
        public ActionResult GradeMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(GradeMaster R)
        {
            string Result = string.Empty;
            
            DataSet chkGradeExistOrNot = new DataSet();
            try
            {
                chkGradeExistOrNot = objGM.chkGradeExistOrNot(R);


                if(chkGradeExistOrNot.Tables[0].Rows.Count >0)
                {
                    TempData["gradeMaster"] = chkGradeExistOrNot;

                }
                else
                {
                    Result = objGM.Insert(R);
                    if (Convert.ToInt16(Result) > 0)
                    {
                        ModelState.Clear();
                        TempData["message"] = "Record Added Successfully...";
                        return RedirectToAction("ListGradeMaster", "GradeMaster");
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
            return View("GradeMaster");
        }

        [HttpGet]
        public ActionResult ListGradeMaster()
        {
            List<GradeMaster> lgm = new List<GradeMaster>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objGM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lgm.Add(new GradeMaster
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    EmployeeGrade = Convert.ToString(dr["EmployeeGrade"]),
                    CarRate = Convert.ToString(dr["CarRate"]),
                    MotorBikeRate = Convert.ToString(dr["MotorBikeRate"]),
                    OPERate = Convert.ToString(dr["OPERate"]),
                    
                    
                });
            }

            obj.lmd1 = lgm;
            return View(obj);

        }

        [HttpGet]
        public ActionResult UpdateGradeMaster(int? id)
        {
            var model = new GradeMaster();
            DataSet dss = new DataSet();


            dss = objGM.GetDataById(Convert.ToInt32(id));
            if (dss.Tables[0].Rows.Count > 0)
            {
                model.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                model.EmployeeGrade = dss.Tables[0].Rows[0]["EmployeeGrade"].ToString();
                model.CarRate = dss.Tables[0].Rows[0]["CarRate"].ToString();
                
                model.MotorBikeRate = dss.Tables[0].Rows[0]["MotorBikeRate"].ToString();
                model.OPERate = dss.Tables[0].Rows[0]["OPERate"].ToString();
               
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateGradeMaster(GradeMaster N, int? Id)
        {
            string Result = string.Empty;
            try
            {
                DataSet chkGradeExistOrNot = new DataSet();
               
                Result = objGM.Update(N, Convert.ToInt32(Id));
                if (Convert.ToInt16(Result) > 0)
                {

                    ModelState.Clear();
                  return RedirectToAction("ListGradeMaster", "GradeMaster");
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
           
            return View(N);


        }


        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objGM.Delete(Convert.ToInt32(Id));
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
            return RedirectToAction("ListGradeMaster");


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
                IGrid<GradeMaster> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<GradeMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<GradeMaster> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<GradeMaster> grid = new Grid<GradeMaster>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            
            

            grid.Columns.Add(model => model.EmployeeGrade).Titled("Employee Grade");
            grid.Columns.Add(model => model.CarRate).Titled("Car Rate");
            grid.Columns.Add(model => model.MotorBikeRate).Titled("Motor Bike Rate");
            grid.Columns.Add(model => model.OPERate).Titled("OPE Rate");

            grid.Pager = new GridPager<GradeMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = obj.lmd1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<GradeMaster> GetData()
        {

            List<GradeMaster> lgm = new List<GradeMaster>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objGM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lgm.Add(new GradeMaster
                {
                    Count = ds.Tables[0].Rows.Count,
                    Id = Convert.ToInt32(dr["Id"]),
                    EmployeeGrade = Convert.ToString(dr["EmployeeGrade"]),
                    CarRate = Convert.ToString(dr["CarRate"]),
                    MotorBikeRate = Convert.ToString(dr["MotorBikeRate"]),
                    OPERate = Convert.ToString(dr["OPERate"]),


                });
            }

            obj.lmd1 = lgm;
            return obj.lmd1;
        }

        #endregion


    }
}