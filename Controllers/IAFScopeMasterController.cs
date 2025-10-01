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
    public class IAFScopeMasterController : Controller
    {
        // GET: IAFScopeMaster
        DALIAFScopeMaster objISM = new DALIAFScopeMaster();
        IAFScopeMaster objI = new IAFScopeMaster();
        public ActionResult CreateIAFScope(int? PK_IAFScopeId)
        {
            var model = new IAFScopeMaster();
            DataSet dss = new DataSet();
            DataTable dsGetStamp = new DataTable();



            if (PK_IAFScopeId != null)
            {
                dss = objISM.GetDataById(Convert.ToInt32(PK_IAFScopeId));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    model.PK_IAFScopeId = Convert.ToInt32(dss.Tables[0].Rows[0]["PK_IAFScopeId"]);
                    model.IAFScopeName = dss.Tables[0].Rows[0]["IAFScopeName"].ToString();
                    model.IAFScopeNumber = dss.Tables[0].Rows[0]["IAFScopeNumber"].ToString();



                }
                return View(model);
            }
            else
            {
                return View();
            }

           
        }
         

        [HttpPost]
        public ActionResult CreateIAFScope(IAFScopeMaster S)
        {
            string Result = string.Empty;
            try
            {
               
                if (S.PK_IAFScopeId > 0)
                {
                    //Update
                    Result = objISM.Insert(S);
                }
                else
                {

                    Result = objISM.Insert(S);
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
            return RedirectToAction("ListIAFScopeMaster", "IAFScopeMaster");
        }


        [HttpGet]
        public ActionResult ListIAFScopeMaster()
        {
            List<IAFScopeMaster> lmd = new List<IAFScopeMaster>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objISM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new IAFScopeMaster
                {
                    PK_IAFScopeId = Convert.ToInt32(dr["PK_IAFScopeId"]),
                    IAFScopeName = Convert.ToString(dr["IAFScopeName"]),
                    IAFScopeNumber = Convert.ToString(dr["IAFScopeNumber"]),
                    

                });
            }
            objI.lstlmd1 = lmd;
            return View(objI);

        }

        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objISM.Delete(Convert.ToInt32(Id));
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
            return RedirectToAction("ListIAFScopeMaster");


        }

        #region
        [HttpGet]
        public ActionResult ExportIndex()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<IAFScopeMaster> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<IAFScopeMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                //added by nikita on 13092023
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "AddIAFScopeTCE-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<IAFScopeMaster> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<IAFScopeMaster> grid = new Grid<IAFScopeMaster>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            

            grid.Columns.Add(model => model.IAFScopeName).Titled("IAF Scope Name");
            grid.Columns.Add(model => model.IAFScopeNumber).Titled("IAF Scope Number");


            grid.Pager = new GridPager<IAFScopeMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objI.lstlmd1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<IAFScopeMaster> GetData()
        {

            List<IAFScopeMaster> lmd = new List<IAFScopeMaster>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objISM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new IAFScopeMaster
                {
                    Count = ds.Tables[0].Rows.Count,
                    PK_IAFScopeId = Convert.ToInt32(dr["PK_IAFScopeId"]),
                    IAFScopeName = Convert.ToString(dr["IAFScopeName"]),
                    IAFScopeNumber = Convert.ToString(dr["IAFScopeNumber"]),


                });
            }
            objI.lstlmd1 = lmd;
            return objI.lstlmd1;
        }
        #endregion







    }


}