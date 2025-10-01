using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class CompentencyMatrixController : Controller
    {
        // GET: CompentencyMatrix
        DALCompentencyMatrix objCM = new DALCompentencyMatrix();
        //public ActionResult CompentencyMatrixMaster(int? Id)
        //{
        //    var model = new CompentencyMatrixMaster();
        //    DataSet dss = new DataSet();
        //    DataTable dsGetStamp = new DataTable();



        //    if (Id != null)
        //    {
        //        dss = objCM.GetDataById(Convert.ToInt32(Id));
        //        if (dss.Tables[0].Rows.Count > 0)
        //        {
        //            model.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
        //            model.Name = dss.Tables[0].Rows[0]["Name"].ToString();




        //        }
        //        return View(model);
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}

        public ActionResult CompentencyMatrixMaster(int? Id)
        {
            var model = new CompentencyMatrixMaster();
            DataSet dss = new DataSet();
            DataTable dsGetStamp = new DataTable();
            DALCalls objDalCalls = new DALCalls();
            DALCallMaster objDAM = new DALCallMaster();

            //  var Data = objDalCalls.GetBranchList();
            //ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");
            //ViewData["Drpproduct"] = objDAM.GetDrpList();



            var Data = objDAM.GetDrpList();
            ViewBag.SubCatlist = new SelectList(Data, "Product_ID", "Product_Name");


            var UserData = objCM.GetInspectorList(); 
            ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");


            if (Id != null)
            {
                ViewBag.check = "productcheck";
                dss = objCM.GetDataById(Convert.ToInt32(Id));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    string[] splitedProduct_Name;
                    model.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                    model.Name = dss.Tables[0].Rows[0]["Name"].ToString();
                    // model.Item = dss.Tables[0].Rows[0]["Item"].ToString();
                    model.ProjectName = dss.Tables[0].Rows[0]["ProjectName"].ToString();
                    model.BranchName = dss.Tables[0].Rows[0]["MainBranch"].ToString();
                    model.Branchid = dss.Tables[0].Rows[0]["branchid"].ToString();

                    List<string> Selected = new List<string>();
                    var Existingins = Convert.ToString(dss.Tables[0].Rows[0]["Item"]);
                    splitedProduct_Name = Existingins.Split(',');
                    foreach (var single in splitedProduct_Name)
                    {
                        Selected.Add(single);
                    }
                    ViewBag.EditproductName = Selected;


                    DataSet ds = new DataSet();
                    ds = objCM.GetDatainspector(Convert.ToInt32(Id));
                    List<CompentencyMatrixMaster> lmd = new List<CompentencyMatrixMaster>();

                    if (ds.Tables.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                        {

                            lmd.Add(new CompentencyMatrixMaster
                            {
                                inspectorid = Convert.ToInt32(dr["Id"]),
                                inspectorName = Convert.ToString(dr["inspectorName"]),
                                Id = Convert.ToInt32(dr["Customerid"]),
                                BranchName = Convert.ToString(dr["mainBranch"]),
                                //added by shrutika salve 16072024
                                Branchid = Convert.ToString(dr["Branchid"])
                            });

                            ViewData["Details"] = lmd;

                        }

                    }




                }
                return View(model);
            }


            else
            {
                var Data2 = objDAM.MainBranch();

                dsGetStamp = Data2.Tables[0];

                model.BranchName = Data2.Tables[0].Rows[0][0].ToString();
                model.Branchid = Data2.Tables[0].Rows[0][1].ToString();
                Session["MainBranch"] = model.BranchName;
                Session["Branchid"] = model.Branchid;
                return View();
            }

        }


        //[HttpPost]
        //public ActionResult CompentencyMatrixMaster(CompentencyMatrixMaster S)
        //{
        //    string Result = string.Empty;
        //    try
        //    {

        //        if (S.Id > 0)
        //        {
        //            //Update
        //            Result = objCM.Insert(S);
        //        }
        //        else
        //        {

        //            Result = objCM.Insert(S);
        //            if (Convert.ToInt16(Result) > 0)
        //            {
        //                ModelState.Clear();
        //                TempData["message"] = "Record Added Successfully";
        //            }
        //            else
        //            {
        //                TempData["message"] = "Error";
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    return RedirectToAction("ListCompentencyMatrixMaster", "CompentencyMatrix");
        //}


        [HttpPost]
        public ActionResult CompentencyMatrixMaster(CompentencyMatrixMaster S, FormCollection fc)
        {

            string Result = string.Empty;
            string Result1 = string.Empty;
            try
            {

                if (S.Id > 0)
                {
                    string ProList = string.Join(",", fc["ProductList"]);
                    S.Item = ProList;
                    //Update
                    Result = objCM.Insert(S);



                    string list = string.Join(",", fc["inspectorName"]);
                    if (list != "")
                    {

                        string[] separator = { "," };
                        string[] strList = list.Split(separator, StringSplitOptions.None);



                        foreach (string s in strList)
                        {
                            S.inspectorName = s;
                            //Session["Branchid"] = S.Branchid;
                            S.Branchid = Convert.ToString(Session["Branchid"]);
                            Result1 = objCM.InsertinspectorList(S);

                        }



                    }




                }
                else
                {
                    string ProList = string.Join(",", fc["ProductList"]);
                    S.Item = ProList;
                    //Session["Branchid"] = S.Branchid;
                    S.Branchid = Convert.ToString(Session["Branchid"]);
                    Result = objCM.Insert(S);
                    S.Id = Convert.ToInt32(Session["PkID"]);
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
            return RedirectToAction("CompentencyMatrixMaster", "CompentencyMatrix", new { id = S.Id });
        }

        [HttpGet]
        public ActionResult ListCompentencyMatrixMaster()
        {
            List<CompentencyMatrixMaster> lmd = new List<CompentencyMatrixMaster>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objCM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new CompentencyMatrixMaster
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Name = Convert.ToString(dr["Name"]),
                    //added by shrutika salve 16072024
                    Branchid = Convert.ToString(dr["MainBranch"])



                });
            }
            return View(lmd.ToList());

        }

        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objCM.Delete(Convert.ToInt32(Id));
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
            //return RedirectToAction("ListIAFScopeMaster");
            return RedirectToAction("ListCompentencyMatrixMaster");


        }


        public ActionResult DeleteinspectorName(int? id, int? inspectorid)
        {
            string Result = string.Empty;
            try
            {
                Result = objCM.Deleteinspector(Convert.ToInt32(id), Convert.ToInt32(inspectorid));
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

            return RedirectToAction("CompentencyMatrixMaster", "CompentencyMatrix", new { id = id });


        }


        [HttpPost]
        public ActionResult GetSurveyorName(string prefix)
        {
            try
            {
                DataSet dsTopic = objCM.GetSurveyorName(prefix);

                if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    List<StampRegister> searchlist = new List<StampRegister>();

                    foreach (DataRow dr in dsTopic.Tables[0].Rows)
                    {
                        searchlist.Add(new StampRegister
                        {
                            SurveyorName = dr["SurveyorName"].ToString(),
                            SurveyorId = dr["SurveyorId"].ToString()
                        });
                    }

                    return Json(searchlist, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new List<StampRegister>(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }

        [HttpPost]
        public JsonResult ExcelUpload(HttpPostedFileBase FileUpload1, monitors CM)
        {
            if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            {
                try
                {
                    Random rnd = new Random();
                    int myRandomNo = rnd.Next(10000000, 99999999);
                    string strmyRandomNo = Convert.ToString(myRandomNo);
                    string path = Server.MapPath("~/Excel/");

                    // Ensure the directory exists
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Save the uploaded file
                    string filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    FileUpload1.SaveAs(filePath);

                    // Process the Excel file
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath);
                    Microsoft.Office.Interop.Excel.Worksheet wSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];

                    // Dictionary to store extracted values
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    List<object> tableData = new List<object>();

                    // Extract specific values from cells
                    values["itemDescription"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[2, 3]).Value); // Item Description
                    values["Reference_Document"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[3, 3]).Value); // Reference Document
                    values["Details_of_inspection_activity"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[4, 3]).Value);



                    // Extract questions and answers
                    Dictionary<string, string> answers = new Dictionary<string, string>();
                    for (int i = 5; i <= 21; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["ifyes"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[22, 3]).Value);


                    for (int i = 23; i <= 40; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["ifno"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[41, 3]).Value);

                    for (int i = 42; i <= 56; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }


                    values["trainingtopic"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[57, 3]).Value);

                    for (int i = 58; i <= 58; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["observation"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[59, 3]).Value);
                    values["obscatno"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[60, 1]).Value);
                    values["obscat"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[60, 3]).Value);


                    for (int i = 61; i <= 69; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;

                        }
                    }

                    int startrow = 60;
                    int startcol = 5;
                    int endrow = 64;
                    int endcol = 6;


                    for (int row = startrow; row <= endrow; row++)
                    {
                        var rowData = new List<string>(); // A list to hold cell data for a single row
                        for (int col = startcol; col <= endcol; col++)
                        {
                            // Add the text of each cell to the row data
                            rowData.Add(wSheet.Cells[row, col].Text ?? string.Empty); // Handle nulls
                        }
                        tableData.Add(rowData); // Add the row to the table
                    }

                    // Serialize the plain table data to JSON
                    var tablehtml = Newtonsoft.Json.JsonConvert.SerializeObject(tableData);
                    // Clean up Excel resources
                    excelBook.Close(false);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                    // Return values and answers as JSON
                    return Json(new { success = true, values = values, answers = answers, tablehtml = tablehtml }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "No file uploaded or file is empty." });
            }
        }

        [HttpPost]
        public JsonResult OffsiteExcelUpload(HttpPostedFileBase FileUpload1, ModelOffSiteMonitoring OffM)
        {
            if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            {
                try
                {
                    Random rnd = new Random();
                    int myRandomNo = rnd.Next(10000000, 99999999);
                    string strmyRandomNo = Convert.ToString(myRandomNo);
                    string path = Server.MapPath("~/IVRIRNExcel/");

                    // Ensure the directory exists
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Save the uploaded file
                    string filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    FileUpload1.SaveAs(filePath);

                    // Process the Excel file
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath);
                    Microsoft.Office.Interop.Excel.Worksheet wSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];

                    // Dictionary to store extracted values
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    List<object> tableData = new List<object>();

                    // Extract specific values from cells
                    values["itemDescription"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[2, 3]).Value); // Item Description
                    values["Reference_Document"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[3, 3]).Value); // Reference Document
                    //values["Details_of_inspection_activity"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[3, 3]).Value);



                    // Extract questions and answers
                    Dictionary<string, string> answers = new Dictionary<string, string>();
                    for (int i = 4; i <= 7; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["ifyes"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[8, 3]).Value);


                    for (int i = 9; i <= 11; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["ifIRN"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[12, 3]).Value);

                    for (int i = 13; i <= 38; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }


                    values["rptprcs"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[39, 3]).Value);

                    for (int i = 40; i <= 52; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["trngtpc"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[53, 3]).Value);

                    for (int i = 54; i <= 54; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["observation"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[55, 3]).Value);
                    values["obscat"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[56, 3]).Value);
                    values["obscatno"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[56, 3]).Value);


                    for (int i = 57; i <= 65; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;

                        }
                    }

                    int startrow = 56;
                    int startcol = 5;
                    int endrow = 60;
                    int endcol = 6;


                    for (int row = startrow; row <= endrow; row++)
                    {
                        var rowData = new List<string>(); // A list to hold cell data for a single row
                        for (int col = startcol; col <= endcol; col++)
                        {
                            // Add the text of each cell to the row data
                            rowData.Add(wSheet.Cells[row, col].Text ?? string.Empty); // Handle nulls
                        }
                        tableData.Add(rowData); // Add the row to the table
                    }

                    // Serialize the plain table data to JSON
                    var tablehtml = Newtonsoft.Json.JsonConvert.SerializeObject(tableData);


                    // Clean up Excel resources
                    excelBook.Close(false);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

                    // Return values and answers as JSON
                    return Json(new { success = true, values = values, answers = answers, tablehtml = tablehtml }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "No file uploaded or file is empty." });
            }
        }

        [HttpPost]
        public JsonResult MentoringExcelUpload(HttpPostedFileBase FileUpload1, Mentoring MM)
        {
            if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            {
                try
                {
                    Random rnd = new Random();
                    int myRandomNo = rnd.Next(10000000, 99999999);
                    string strmyRandomNo = Convert.ToString(myRandomNo);
                    string path = Server.MapPath("~/IVRIRNExcel/");

                    // Ensure the directory exists
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Save the uploaded file
                    string filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    FileUpload1.SaveAs(filePath);

                    // Process the Excel file
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath);
                    Microsoft.Office.Interop.Excel.Worksheet wSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];

                    // Dictionary to store extracted values
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    List<object> tableData = new List<object>();

                    // Extract specific values from cells
                    values["itemDescription"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[2, 3]).Value); // Item Description
                    values["DesInsAct"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[3, 3]).Value); // Reference Document
                    //values["Details_of_inspection_activity"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[4, 3]).Value);



                    // Extract questions and answers
                    Dictionary<string, string> answers = new Dictionary<string, string>();
                    for (int i = 4; i <= 12; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["trainingtopic"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[13, 3]).Value);


                    for (int i = 14; i <= 14; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["observation"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[15, 3]).Value);
                    values["obscatno"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[16, 1]).Value);
                    values["obscat"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[16, 3]).Value);


                    for (int i = 17; i <= 25; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }



                    int startrow = 16;
                    int startcol = 5;
                    int endrow = 20;
                    int endcol = 6;


                    for (int row = startrow; row <= endrow; row++)
                    {
                        var rowData = new List<string>(); // A list to hold cell data for a single row
                        for (int col = startcol; col <= endcol; col++)
                        {
                            // Add the text of each cell to the row data
                            rowData.Add(wSheet.Cells[row, col].Text ?? string.Empty); // Handle nulls
                        }
                        tableData.Add(rowData); // Add the row to the table
                    }

                    // Serialize the plain table data to JSON
                    var tablehtml = Newtonsoft.Json.JsonConvert.SerializeObject(tableData);


                    // Clean up Excel resources
                    excelBook.Close(false);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

                    // Return values and answers as JSON
                    return Json(new { success = true, values = values, answers = answers, tablehtml = tablehtml }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "No file uploaded or file is empty." });
            }
        }

        [HttpPost]
        public JsonResult MOMsiteExcelUpload(HttpPostedFileBase FileUpload1, MonitoringOfMonitors MOM)
        {
            if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            {
                try
                {
                    Random rnd = new Random();
                    int myRandomNo = rnd.Next(10000000, 99999999);
                    string strmyRandomNo = Convert.ToString(myRandomNo);
                    string path = Server.MapPath("~/IVRIRNExcel/");

                    // Ensure the directory exists
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Save the uploaded file
                    string filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    FileUpload1.SaveAs(filePath);

                    // Process the Excel file
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath);
                    Microsoft.Office.Interop.Excel.Worksheet wSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];

                    // Dictionary to store extracted values
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    List<object> tableData = new List<object>();

                    // Extract specific values from cells
                    values["itemDescription"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[2, 3]).Value); // Item Description
                    //values["DesInsAct"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[3, 3]).Value); // Reference Document
                    //values["Details_of_inspection_activity"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[4, 3]).Value);



                    // Extract questions and answers
                    Dictionary<string, string> answers = new Dictionary<string, string>();
                    for (int i = 3; i <= 24; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["trainingtopic"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[25, 3]).Value);




                    values["observation"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[26, 3]).Value);
                    values["obscatno"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[27, 1]).Value);
                    values["obscat"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[27, 3]).Value);


                    for (int i = 28; i <= 36; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }



                    int startrow = 28;
                    int startcol = 5;
                    int endrow = 32;
                    int endcol = 6;


                    for (int row = startrow; row <= endrow; row++)
                    {
                        var rowData = new List<string>(); // A list to hold cell data for a single row
                        for (int col = startcol; col <= endcol; col++)
                        {
                            // Add the text of each cell to the row data
                            rowData.Add(wSheet.Cells[row, col].Text ?? string.Empty); // Handle nulls
                        }
                        tableData.Add(rowData); // Add the row to the table
                    }

                    // Serialize the plain table data to JSON
                    var tablehtml = Newtonsoft.Json.JsonConvert.SerializeObject(tableData);


                    // Clean up Excel resources
                    excelBook.Close(false);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

                    // Return values and answers as JSON
                    return Json(new { success = true, values = values, answers = answers, tablehtml = tablehtml }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "No file uploaded or file is empty." });
            }
        }

    }
}