using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using OfficeOpenXml;
using NonFactors.Mvc.Grid;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using OfficeOpenXml.Style;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace TuvVision.Controllers
{
    public class LeaveController : Controller
    {
        // GET: Leave
        DALLeave DALobj = new DALLeave();
        DALCalls objDalCalls = new DALCalls();
        CommonControl objCommonControl = new CommonControl();
        Leave objL = new Leave();
        public ActionResult LeaveDetail()
        {

            Session["GetExcelData"] = "Yes";

            List<Leave> lmd = new List<Leave>();  // creating list of model.  
            DataSet ds = new DataSet();

            #region added for search inside gridview
            //if (Session["FromDate"] != null && Session["ToDate"] != null)
            if (TempData["FromDate"] != null && TempData["ToDate"] != null)
            {
                //Leave n = new Leave();
                objL.FromD = Convert.ToString(TempData["FromDate"]);
                objL.ToD = Convert.ToString(TempData["ToDate"]);
                TempData.Keep();
                ds = DALobj.GetDataByDate(objL);
            }
            else
            {
                ds = DALobj.GetLeaveDashboard();
            }
            #endregion

            // ds = DALobj.GetLeaveDashboard(); // fill dataset  

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new Leave
                    {
                        Id = Convert.ToInt32(dr["ID"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        DateSE = Convert.ToString(dr["DateSE"]),
                        FirstName = Convert.ToString(dr["Name"]),
                        LeaveAddedBy = Convert.ToString(dr["LeaveAddedBy"]),
                        LTILeave = Convert.ToString(dr["ChkLTILeave"]),
                        Attachment = Convert.ToString(dr["Attachment"]),


                    });
                }

            }
            objL.LeaveDashBoard = lmd;
            ViewData["BranchList"] = lmd;
            return View(objL);




        }

        [HttpPost]
        public ActionResult LeaveDetail(Leave LD)
        {
            List<Leave> lmd = new List<Leave>();  // creating list of model.  
            DataSet ds = new DataSet();

            //ds = DALobj.GetDataByDate(LD); // fill dataset  

            Session["GetExcelData"] = null;
            //Session["FromDate"] = LD.FromD;
            //Session["ToDate"] = LD.ToD;

            TempData["FromDate"] = LD.FromD;
            TempData["ToDate"] = LD.ToD;
            TempData.Keep();

            //if ((TempData["FromDate"] != null && TempData["FromDate"] != string.Empty) && (TempData["ToDate"] != null && TempData["ToDate"] != string.Empty))
            //{
            //    TempData.Keep();
            //    return RedirectToAction("LeaveDetail");
            //    ds = DALobj.GetDataByDate(LD);  // fill dataset 
            //}
            //else
            //{
            //    return RedirectToAction("LeaveDetail");
            //    ds = DALobj.GetLeaveDashboard();
            //}





            //ds = DALobj.GetDataByDate(LD);

            //if (ds.Tables.Count >0 )
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            //        {
            //            lmd.Add(new Leave
            //            {
            //                Id = Convert.ToInt32(dr["ID"]),
            //                ActivityType = Convert.ToString(dr["ActivityType"]),
            //                DateSE = Convert.ToString(dr["DateSE"]),
            //                FirstName = Convert.ToString(dr["Name"]),
            //                LeaveAddedBy = Convert.ToString(dr["LeaveAddedBy"]),
            //                Attachment = Convert.ToString(dr["Attachment"]),


            //            });
            //        }

            //    }
            //}

            // objL.LeaveDashBoard = lmd;

            return RedirectToAction("AddLeave");
            return View(objL);




        }


        public ActionResult AddLeave(int? Id)
        {
            Session["GetExcelData"] = "Yes";

            List<Leave> lmd = new List<Leave>();  // creating list of model.  
            DataSet ds = new DataSet();

            #region added for search inside gridview            
            if (TempData["FromDate"] != null && TempData["ToDate"] != null)
            {
                objL.FromD = Convert.ToString(TempData["FromDate"]);
                objL.ToD = Convert.ToString(TempData["ToDate"]);
                TempData.Keep();
                ds = DALobj.GetDataByDate(objL);
            }
            else
            {
                ds = DALobj.GetLeaveDashboard();
            }
            #endregion

              if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new Leave
                    {
                        Id = Convert.ToInt32(dr["ID"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        DateSE = Convert.ToString(dr["DateSE"]),
                        FirstName = Convert.ToString(dr["Name"]),
                        LeaveAddedBy = Convert.ToString(dr["LeaveAddedBy"]),
                        LTILeave = Convert.ToString(dr["ChkLTILeave"]),
                        Attachment = Convert.ToString(dr["Attachment"]),

                    });
                }

            }
            objL.LeaveDashBoard = lmd;
            ViewData["BranchList"] = lmd;

            var UserData = objDalCalls.GetInspectorListForLeaveManagement();
            ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");
            DataSet dss = new DataSet();
            if (Id != null)
            {
                dss = DALobj.GetDataById(Convert.ToInt32(Id));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    objL.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                    objL.FirstName = dss.Tables[0].Rows[0]["CreatedBy"].ToString();
                    objL.StartDate = dss.Tables[0].Rows[0]["DateSE"].ToString();
                    objL.EndDate = dss.Tables[0].Rows[0]["DateSE"].ToString();
                    objL.ActivityType = dss.Tables[0].Rows[0]["ActivityType"].ToString();
                    objL.Attachment = dss.Tables[0].Rows[0]["Attachment"].ToString();
                    objL.Reason = dss.Tables[0].Rows[0]["Reason"].ToString();
                }
                return View(objL);
            }
            //else
            //{
            //    return View();
            //}
            return View(objL);


            //return View();
        }

        //DateTime Start;
        //DateTime End;
        string Start;
        string End;


        [HttpPost]
        public ActionResult AddLeave(Leave objL, HttpPostedFileBase File/*, HttpPostedFileBase img_Banner*/)
        {
            String Result = "";
            DataTable DTValidateTT = new DataTable();
            DataTable DtchkLeaveExist = new DataTable();
            string str = "";
            string strLeaveExist = "";
            if (objL.Id > 0)
            {
                //Start = DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", null);
                //End = DateTime.ParseExact(objL.EndDate, "dd/MM/yyyy", null);
                Start = Convert.ToString(objL.StartDate);
                End = Convert.ToString(objL.EndDate);
            }
            else
            {

                Start = Convert.ToString(objL.StartDate);
                End = Convert.ToString(objL.EndDate);
                //Start = DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", null);
                //End = DateTime.ParseExact(objL.EndDate, "dd/MM/yyyy", null);
            }



            DateTime Start1;
            DateTime End1;
            if (objL.Id > 0)
            {
                Start1 = DateTime.ParseExact(Start, "dd/MM/yyyy", null);//Start;
                End1 = DateTime.ParseExact(End, "dd/MM/yyyy", null);//Start;//End;
                for (DateTime date = Start1; date.Date <= End1; date = date.AddDays(1))
                {
                    #region Upload Image/Document
                    //if (img_Banner.Count() > 0)
                    //{
                    //    foreach (HttpPostedFileBase item in img_Banner)
                    //    {
                    //        HttpPostedFileBase image = item;
                    //        if (image != null && image.ContentLength > 0)
                    //        {
                    //            string filePath = AppDomain.CurrentDomain.BaseDirectory + "NonInspectionActivityDocument\\" + image.FileName;
                    //            const string ImageDirectoryFP = "NonInspectionActivityDocument\\";
                    //            const string ImageDirectory = "~/NonInspectionActivityDocument/";
                    //            string ImagePath = "~/NonInspectionActivityDocument/" + image.FileName;
                    //            string fileNameWithExtension = System.IO.Path.GetExtension(image.FileName);
                    //            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
                    //            string ImageName = image.FileName;




                    //            image.SaveAs(filePath);



                    //            objL.Attachment = ImageName;
                    //        }
                    //    }
                    //}

                    HttpPostedFileBase img_Banner;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        img_Banner = Request.Files["img_Banner"];
                        if (img_Banner != null && img_Banner.FileName != "")
                        {
                            objL.Attachment = CommonControl.FileUpload("NonInspectionActivityDocument/", img_Banner);
                        }
                        else
                        {
                            if (img_Banner.FileName != "")
                            {
                                objL.Attachment = "NoImage.gif";
                            }
                        }
                    }
                    #endregion

                    DateTime StDt = date;

                    DTValidateTT = DALobj.CheckValidLeave(StDt.ToString("dd/MM/yyyy"), objL);

                    objL.DateSE = StDt.ToString("dd/MM/yyyy");

                    if (DTValidateTT.Rows.Count > 0)
                    {

                        str = str + ',' + StDt.ToString("dd/MM/yyyy");
                        TempData["MsgCallAssigned"] = "Call is assigned for the day of " + str;
                    }
                    else
                    {
                        Result = DALobj.Update(objL);

                    }
                }

            }
            else
            {
                Start1 = DateTime.ParseExact(Start, "dd/MM/yyyy", null);//Start;
                End1 = DateTime.ParseExact(End, "dd/MM/yyyy", null);//Start;//End;
                List<Leave> lmd = new List<Leave>();
                //for (DateTime date = Start; date.Date <= End; date = date.AddDays(1))
                for (DateTime date = Start1; date.Date <= End1; date = date.AddDays(1))
                {
                    DateTime StDt = date;
                    objL.DateSE = StDt.ToString("dd/MM/yyyy");
                    if (objL.Id > 0)
                    {
                        //Update

                    }
                    else
                    {



                        DTValidateTT = DALobj.CheckValidLeave(StDt.ToString("dd/MM/yyyy"), objL);

                        DtchkLeaveExist = DALobj.CheckIfLeavePresent(StDt.ToString("dd/MM/yyyy"), objL);

                        if (DTValidateTT.Rows.Count > 0)
                        {

                            str = str + ',' + StDt.ToString("dd/MM/yyyy");
                            TempData["MsgCallAssigned"] = "Call is assigned for the day of " + str;
                        }
                        else if (DtchkLeaveExist.Rows.Count > 0)
                        {
                            strLeaveExist = strLeaveExist + ',' + StDt.ToString("dd/MM/yyyy");
                            TempData["MsgIfLeaveExist"] = "Leave is already added for the day " + strLeaveExist/*StDt.ToString("dd/MM/yyyy")*/;
                        }
                        else
                        {
                            #region Upload Image/Document
                            //if (img_Banner.Count() > 0)
                            //{
                            //    foreach (HttpPostedFileBase item in img_Banner)
                            //    {
                            //        HttpPostedFileBase image = item;
                            //        if (image != null && image.ContentLength > 0)
                            //        {
                            //            string filePath = AppDomain.CurrentDomain.BaseDirectory + "NonInspectionActivityDocument\\" + image.FileName;
                            //            const string ImageDirectoryFP = "NonInspectionActivityDocument\\";
                            //            const string ImageDirectory = "~/NonInspectionActivityDocument/";
                            //            string ImagePath = "~/NonInspectionActivityDocument/" + image.FileName;
                            //            string fileNameWithExtension = System.IO.Path.GetExtension(image.FileName);
                            //            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
                            //            string ImageName = image.FileName;


                            //                image.SaveAs(filePath);

                            //            objL.Attachment = ImageName;
                            //        }
                            //    }
                            //}

                            HttpPostedFileBase img_Banner;
                            if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                            {
                                img_Banner = Request.Files["img_Banner"];
                                if (img_Banner != null && img_Banner.FileName != "")
                                {
                                    objL.Attachment = CommonControl.FileUpload("NonInspectionActivityDocument/", img_Banner);
                                }
                                else
                                {
                                    if (img_Banner.FileName != "")
                                    {
                                        objL.Attachment = "NoImage.gif";
                                    }
                                }
                            }
                            #endregion


                            Result = DALobj.Insert(objL);
                            if (Convert.ToInt16(Result) > 0)
                            {
                                TempData["message"] = "Record Added Successfully";
                            }
                            else
                            {
                                TempData["message"] = "Error";
                            }
                        }

                        DtchkLeaveExist.Clear();
                        DTValidateTT.Clear();


                    }
                }
            }
            return RedirectToAction("AddLeave");
            return RedirectToAction("LeaveDetail", "Leave");

        }



        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = DALobj.Delete(Convert.ToInt32(Id));
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
            return RedirectToAction("AddLeave");


        }

        [HttpGet]
        public ActionResult HolidayList()
        {

            Leave ObjLeave = new Leave();

            try
            {


                DataSet ds = new DataSet();

                List<Leave> lstLeave = new List<Leave>();
                Session["GetExcelData"] = "Yes";




                TempData.Keep();

                ds = DALobj.GetHolidayDetail();


                if (ds.Tables.Count > 0)
                {
                    ObjLeave.dtLeave = ds.Tables[0];
                    ViewData["lstLeave"] = lstLeave;
                }

                //   objProfile.LstUserData = lstUserDashBoard;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(ObjLeave);
        }



        [HttpGet]
        public ActionResult DownloadHolidayTemplate(Leave objLeave)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 1;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                DataTable dt = DALobj.GetBranchMaster();

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];
                sheet.Cells[row, col].Value = "Sr.No";
                col++;
                sheet.Cells[row, col].Value = "Occasion";
                col++;
                sheet.Cells[row, col].Value = "Date";
                col++;
                foreach (DataRow column in dt.Rows)
                {
                    sheet.Cells[row, col].Value = column[0];
                    sheet.Column(col++).Width = 18;


                }

                //foreach (IGridRow<CallsModel> gridRow in dt.Rows)
                //{
                //    col = 1;
                //    foreach (IGridColumn column in dt.Columns)
                //        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                //    row++;
                //}

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");

            }

        }

        //[HttpPost]

        //public ActionResult HolidayList(Leave objLeave, HttpPostedFileBase FileUpload1)
        //{

        //    Random rnd = new Random();
        //    int myRandomNo = rnd.Next(10000000, 99999999);
        //    string strmyRandomNo = Convert.ToString(myRandomNo);
        //    HttpPostedFileBase files = FileUpload1;

        //    if (FileUpload1 != null)
        //    {


        //        if (/*files.ContentLength > 0*/  files != null && !string.IsNullOrEmpty(files.FileName) || files.FileName.Contains(".xlsx") && files.FileName.Contains(".xlsm"))
        //        {
        //            GC.Collect();
        //            GC.WaitForPendingFinalizers();
        //            //string Result = string.Empty;
        //            string filePath = string.Empty;
        //            // HttpPostedFileBase files = FileUpload;
        //            string fileName = files.FileName;
        //            string fileContentType = files.ContentType;
        //            byte[] fileBytes = new byte[files.ContentLength];
        //            var data1 = files.InputStream.Read(fileBytes, 0, Convert.ToInt32(files.ContentLength));
        //            var package = new ExcelPackage(files.InputStream);  //===========Go to Manage Nuget in Install ExcellPackge 

        //            #region save file to dir
        //            //string path = Server.MapPath("~/Content/JobDocument/");
        //            string path = Server.MapPath("~/IVRIRNExcel/");
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }


        //            filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);



        //            if (System.IO.File.Exists(filePath))
        //            {
        //                System.IO.File.Delete(filePath);


        //            }

        //            string extension = Path.GetExtension(strmyRandomNo + FileUpload1.FileName);
        //            FileUpload1.SaveAs(filePath);


        //            filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
        //            #endregion





        //            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
        //            Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath); ;//xlApp.Workbooks.Open(filePath);
        //            String[] excelSheets = new String[excelBook.Worksheets.Count];
        //            // var Reader = new StreamReader(File.Pa)
        //            string Result = string.Empty;
        //            int i = 0;
        //            foreach (Microsoft.Office.Interop.Excel.Worksheet wSheet in excelBook.Worksheets)
        //            {
        //                excelSheets[i] = wSheet.Name;
        //                int RowsCount = wSheet.UsedRange.Rows.Count;// - 1;


        //                for (int j = 2; j <= RowsCount; j++)
        //                {
        //                    string SrNo = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
        //                    if (SrNo != null)
        //                    {
        //                        objLeave.Occasion = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 2]).Value);
        //                        objLeave.Date = Convert.ToDateTime(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 3]).Value);
        //                        objLeave.AHMEDABAD = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 4]).Value);
        //                        objLeave.AURANGABAD = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 5]).Value);
        //                        objLeave.BANGALORE = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 6]).Value);
        //                        objLeave.BELGAUM = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 7]).Value);
        //                        objLeave.CHENNAI = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 8]).Value);
        //                        objLeave.COIMBATORE = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 9]).Value);
        //                        objLeave.DELHI = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 10]).Value);
        //                        objLeave.HUBLI = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 11]).Value);
        //                        objLeave.HYDERABAD = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 12]).Value);
        //                        objLeave.KOLHAPUR = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 13]).Value);
        //                        objLeave.KOLKATA = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 14]).Value);
        //                        objLeave.LUDHIANA = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 15]).Value);
        //                        objLeave.MUMBAI = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 16]).Value);
        //                        objLeave.NAGPUR = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 17]).Value);
        //                        objLeave.NASHIK = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 18]).Value);
        //                        objLeave.PCGJamnagar = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 19]).Value);
        //                        objLeave.PUNE = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 20]).Value);
        //                        objLeave.Trichy = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 21]).Value);
        //                        objLeave.Vadodara = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 22]).Value);
        //                        objLeave.VIZAG = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 23]).Value);
        //                    }



        //                    //Result = DALobj.InserHolodayListBranch(objLeave ,FileUpload1.FileName , strmyRandomNo);





        //                }

        //            }
        //        }
        //    }
        //    return View();
        //}

        [HttpGet]
        public ActionResult HolidayDetailDashboard(string Year)
        {
           

                Leave ObjLeave = new Leave();

                try
                {


                    DataSet ds = new DataSet();

                    List<Leave> lstLeave = new List<Leave>();
                    Session["GetExcelData"] = "Yes";

                ObjLeave.Year = Convert.ToInt32(Year);


                    TempData.Keep();

                    ds = DALobj.GetHolidayListDetail(ObjLeave);
                   ViewBag.SubCatlist = DALobj.GetYears(DateTime.Now.Year);

                if (ds.Tables.Count > 0)
                    {
                        ObjLeave.dtLeave = ds.Tables[0];
                        ViewData["lstLeave"] = lstLeave;
                   
                }

                    //   objProfile.LstUserData = lstUserDashBoard;

                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                return View(ObjLeave);
           
        }

        [HttpPost]

        public ActionResult HolidayDetailDashboard(Leave objLeave, HttpPostedFileBase FileUpload1, string Upload, string Clear)
        {

            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);
            string strmyRandomNo = Convert.ToString(myRandomNo);
            HttpPostedFileBase files = FileUpload1;
            string Result = string.Empty;
            if (!string.IsNullOrEmpty(Upload))
            {



                if (FileUpload1 != null)
                {


                    if (/*files.ContentLength > 0*/  files != null && !string.IsNullOrEmpty(files.FileName) || files.FileName.Contains(".xlsx") && files.FileName.Contains(".xlsm"))
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        //string Result = string.Empty;
                        string filePath = string.Empty;
                        // HttpPostedFileBase files = FileUpload;
                        string fileName = files.FileName;
                        string fileContentType = files.ContentType;
                        byte[] fileBytes = new byte[files.ContentLength];
                        var data1 = files.InputStream.Read(fileBytes, 0, Convert.ToInt32(files.ContentLength));
                        var package = new ExcelPackage(files.InputStream);  //===========Go to Manage Nuget in Install ExcellPackge 

                        #region save file to dir
                        //string path = Server.MapPath("~/Content/JobDocument/");
                        string path = Server.MapPath("~/IVRIRNExcel/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }


                        filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);



                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);


                        }

                        string extension = Path.GetExtension(strmyRandomNo + FileUpload1.FileName);
                        FileUpload1.SaveAs(filePath);


                        filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                        #endregion

                        Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                        Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath); ;//xlApp.Workbooks.Open(filePath);
                        String[] excelSheets = new String[excelBook.Worksheets.Count];
                        // var Reader = new StreamReader(File.Pa)

                        int i = 0;
                        foreach (Microsoft.Office.Interop.Excel.Worksheet wSheet in excelBook.Worksheets)
                        {
                            excelSheets[i] = wSheet.Name;
                            int RowsCount = wSheet.UsedRange.Rows.Count;// - 1;
                            DataTable dt = DALobj.GetBranchMaster();
                            DataTable dt1 = new DataTable();
                            DataRow dR = dt1.NewRow();
                            dt1.Columns.Add("SrNo");
                            dt1.Columns.Add("Date");
                            dt1.Columns.Add("Occassion");

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i][0].ToString().Contains("PCG"))
                                    dt1.Columns.Add(dt.Rows[i][0].ToString().Replace(" ", ""));
                                else
                                    dt1.Columns.Add(dt.Rows[i][0].ToString());


                            }

                            for (int j = 2; j <= RowsCount; j++)
                            {
                               
                                DataRow row = dt1.NewRow();

                                for (i = 0; i < dt1.Columns.Count; i++)
                                {


                                    row[i] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, i + 1]).Value);


                                }
                                dt1.Rows.Add(row);
                            }
                            Result = DALobj.InserHolodayListBranch(dt1, objLeave);

                        }

                        


                    }
                }

                return RedirectToAction("HolidayList", "Leave");


            }
            if (!string.IsNullOrEmpty(Clear))
            {
                Result = DALobj.DeleteHolidayList(objLeave);
                return RedirectToAction("HolidayList", "Leave");
            }
            return View();
        }


        [HttpGet]
        public ActionResult LeaveApplicationUser(string ID1)
        {
            Leave objLeave = new Leave();
            List<Leave> lmd = new List<Leave>();  // creating list of model.  
            DataSet ds = new DataSet();
            int _min = 10000;
            int _max = 99999;
            Random _rdm = new Random();
            int Rjno = _rdm.Next(_min, _max);
            string ConfirmCode = Convert.ToString(Rjno);
            objL.uniqueNumber = ConfirmCode;
            if (ID1 != null) {
                objL.UniversalID = ID1;
            }
           
         
            ds = DALobj.GetUserLeaveApplication(ID1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new Leave
                    {
                        Id = Convert.ToInt32(dr["ID"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                       // StartDate = Convert.ToString(dr["StartDate"]),
                        StartDate = Convert.ToString(dr["dt"]),
                        Status = Convert.ToString(dr["status"]),
                        LeaveAddedBy = Convert.ToString(dr["CreatedBy"]),
                        LTILeave = Convert.ToString(dr["ChkLTILeave"]),
                        Reason = Convert.ToString(dr["Reason"]),
                        UniversalID = Convert.ToString(dr["UniversalID"]),
                        AutoPL = Convert.ToString(dr["autopl"]),
                        LeaveCount  = Convert.ToString(dr["autocnt"]),



                    });
                }

            }
            if (ds.Tables[1].Rows.Count > 0)
            {
          
                objL.ApproveName = ds.Tables[1].Rows[0]["Approve"].ToString();
                
            }
            DataSet dss = new DataSet();
            if (ID1 != null)
            {
                dss = DALobj.EditUserLeaveApplication(ID1);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    objL.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                    objL.FirstName = dss.Tables[0].Rows[0]["CreatedBy"].ToString();
                    objL.StartDate = dss.Tables[0].Rows[0]["FromDates"].ToString();
                    objL.ToD = dss.Tables[0].Rows[0]["ToDate"].ToString();
                    objL.ActivityType = dss.Tables[0].Rows[0]["ActivityType"].ToString();
                    objL.Reason = dss.Tables[0].Rows[0]["Reason"].ToString();
                    objL.ChkLTILeave = Convert.ToBoolean(dss.Tables[0].Rows[0]["ChkLTILeave"]);
                    objL.UniversalID = Convert.ToString(dss.Tables[0].Rows[0]["Universal_ID"]);
                }
              
            }
            DataTable DTGetUploadedFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTGetUploadedFile = DALobj.EditUploadedFile(ID1);
            if (DTGetUploadedFile.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetUploadedFile.Rows)
                {
                    lstEditFileDetails.Add(
                       new FileDetails
                       {

                           PK_ID = Convert.ToInt32(dr["PK_ID"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Extension = Convert.ToString(dr["Extenstion"]),
                           IDS = Convert.ToString(dr["FileID"]),
                       }
                     );
                }
                
                
            }
            ViewData["lstEditFileDetails"] = lstEditFileDetails;
            objL.FileDetails = lstEditFileDetails;

            DataTable DTGetUploadedFile1 = new DataTable();
            List<FileDetails> lstEditFileDetails1 = new List<FileDetails>();
            DTGetUploadedFile1 = DALobj.EditUploadedFile1(ID1);
            if (DTGetUploadedFile1.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetUploadedFile1.Rows)
                {
                    lstEditFileDetails1.Add(
                       new FileDetails
                       {

                           PK_ID = Convert.ToInt32(dr["PK_ID"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Extension = Convert.ToString(dr["Extenstion"]),
                           IDS = Convert.ToString(dr["FileID"]),
                       }
                     );
                }
                
            }
            ViewData["lstEditFileDetails1"] = lstEditFileDetails1;
            objL.FileDetails1 = lstEditFileDetails1;


            objL.LeaveDashBoard = lmd;
            ViewData["Leave"] = lmd;
            return View(objL);
        }

        [HttpPost]
        public ActionResult LeaveApplicationUser(Leave objLeave)
        {
       

            string Result = "";
        DataSet DTValidateTT = new DataSet();
            DataSet DTValidateTT1 = new DataSet();
            DataTable DtchkLeaveExist = new DataTable();
            List<FileDetails> lstFileDtls = new List<FileDetails>();
           lstFileDtls = Session["listUploadedFile"] as List<FileDetails>;
            List<FileDetails> lstFileDtlsFormat = new List<FileDetails>();
            lstFileDtlsFormat = Session["listFormatFile"] as List<FileDetails>;
            if (objLeave.StartDate != null && objLeave.ToD != null)
            {
                DTValidateTT = DALobj.CheckCallMasterDetail(objLeave);
                if (DTValidateTT.Tables.Count > 0)
                {

                    
                    TempData["CallAssigned"] = "CallAssigned";
                    TempData.Keep();
                    return RedirectToAction("LeaveApplicationUser");
                    return View();
                }
            }
            //if (objLeave.ActivityType == "Weekly off" && objLeave.UniversalID==null)
            //{
            //    DTValidateTT = DALobj.WeeklyoffCount(objLeave);
            //    if (DTValidateTT.Tables.Count > 0)
            //    {

            //        ViewBag.Message = "Weekly off";
            //        return RedirectToAction("LeaveApplicationUser");
            //        return View();
            //    }
            //}

            if (objLeave.ActivityType == "Holiday" && objLeave.UniversalID == null)
            {
                DTValidateTT1 = DALobj.HolidayCount(objLeave);
                //DataTable yourTable = DTValidateTT.Tables["yourTable"];
                if (DTValidateTT1.Tables.Count > 0)
                {



                }
                else
                {
                    TempData["Message1"] = "Holiday Not Valid";
                    TempData.Keep();


                    return RedirectToAction("LeaveApplicationUser");

                    return View();
                }
            }

            if (objLeave.StartDate != null && objLeave.ToD != null && objLeave.UniversalID == null)
            {
                if (objLeave.ChkHalfDayLeave == true)
                {
                    DTValidateTT = DALobj.OnlyLeaveValidate(objLeave);
                    if (DTValidateTT.Tables[0].Rows.Count > 0)
                    {
                        TempData["A"] = "Leave Already Added";
                        TempData.Keep();
                        return RedirectToAction("LeaveApplicationUser", "Leave");
                    }
                    else
                    {
                       // Result = DALobj.InsertUserLeaveApplicationHalfday(objLeave);
                    }

                }
                else
                {
                    DTValidateTT = DALobj.LeaveValidate(objLeave);
                    if (DTValidateTT.Tables[0].Rows.Count > 0)
                    {

                        //ViewBag.Message = "Leave Added SuccessFully";
                        TempData["A"] = "Leave Already Added";
                        TempData.Keep();
                        return RedirectToAction("LeaveApplicationUser", "Leave");
                    }
                }
            }
            if (objLeave.UniversalID==null)
            {
                objLeave.UniversalID = objCommonControl.GetUniqueID();
            }
           
           
            if (objLeave.StartDate == objLeave.ToD && objLeave.ActivityType != "Holiday")
            {
                //chk if holiday
            //    DTValidateTT = DALobj.ValidateLeaveApplicationDate(objLeave);
            //    if (DTValidateTT.Tables.Count > 0)
            //    {

            //        ViewBag.Message = "Please Select Proper Date";
            //        return View();
            //    }



            //    else
            //{

                    if (objLeave.ChkHalfDayLeave == true)
                    {
                        Result = DALobj.InsertUserLeaveApplicationHalfday(objLeave);
                    }
                    else
                    {
                        Result = DALobj.InsertUserLeaveApplication(objLeave);
                    }

                    if (objLeave.Id > 0)
                {
                    string type = null;
                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, objLeave.Id);
                        Result = DALobj.InsertFileAttachment(lstFileDtls, objLeave.UniversalID, type = "Seak", objLeave.Id);
                        Session["listUploadedFile"] = null;

                    }
                    if (lstFileDtlsFormat != null && lstFileDtlsFormat.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtlsFormat, objLeave.Id);
                        Result = DALobj.InsertFileAttachment(lstFileDtlsFormat, objLeave.UniversalID, type = "Other", objLeave.Id);
                        Session["listFormatFile"] = null;
                    }
                }
                if (objLeave.Id > 0)
                {
                    TempData["S"] = "Leave Added SuccessFully";
                    TempData.Keep();
                }
                else
                {
                    TempData["E"] = "Something Went Wrong Please Check";
                }


                DtchkLeaveExist.Clear();
                DTValidateTT.Clear();


                //return View(objLeave);

            ////}

            }

            else
            {
            
                Result = DALobj.InsertUserLeaveApplication(objLeave);
                if (objLeave.Id > 0)
                {

                    string type = null;
                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, objLeave.Id);
                        Result = DALobj.InsertFileAttachment(lstFileDtls, objLeave.UniversalID, type = "Seak" ,objLeave.Id);
                        Session["listUploadedFile"] = null;
                    }
                    if (lstFileDtlsFormat != null && lstFileDtlsFormat.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtlsFormat, objLeave.Id);
                        Result = DALobj.InsertFileAttachment(lstFileDtlsFormat, objLeave.UniversalID, type = "Other" ,objLeave.Id);
                        Session["listFormatFile"] = null;
                    }
                    TempData["S"] = "Leave Added SuccessFully";
                    TempData.Keep();
                }
                else
                {
                    TempData["E"] = "Something Went Wrong";
                    TempData.Keep();
                }


                DtchkLeaveExist.Clear();
                DTValidateTT.Clear();


                return RedirectToAction("LeaveApplicationUser", "Leave");

            }
            return RedirectToAction("LeaveApplicationUser", "Leave");
        }


        public ActionResult DeleteUserLeave(string Id)
        {
            string Result = string.Empty;
            try
            {
                Result = DALobj.DeleteUserLeaveApplication(Id);
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
            return RedirectToAction("LeaveApplicationUser");


        }


        public ActionResult LeaveManagementApprover()
        {
            Leave OBJLeave = new Leave();
       
            try
            {


                DataSet ds = new DataSet();

                List<Leave> lstLeave = new List<Leave>();
                Session["GetExcelData"] = "Yes";




                TempData.Keep();

                ds = DALobj.GetEmployeeDataSheet();


                if (ds.Tables.Count > 0)
                {
                    OBJLeave.dtLeave = ds.Tables[0];
                    ViewData["dtLeave"] = lstLeave;
                }
              
                DataTable dtUserLeave = new DataTable();

                dtUserLeave = DALobj.GetApprovalRejectEmployeeDataSheet();

                if (dtUserLeave.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUserLeave.Rows)
                    {


                        lstLeave.Add(
                           new Leave
                           {
                               UniversalID = Convert.ToString(dr["Universal_ID"]),
                                 FirstName = Convert.ToString(dr["Name"]),
                               FromD = Convert.ToString(dr["StartDate"]),
                               Reason = Convert.ToString(dr["Reason"]),
                               ChkLTILeavestring = Convert.ToString(dr["ChkLTILeave"]),
                               uniqueNumber  = Convert.ToString(dr["uniqueNumber"]),
                               Status = Convert.ToString(dr["Status"]),


                           }
                        );
                    }
                }


                OBJLeave.lst1 = lstLeave;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(OBJLeave);
        }

        [HttpGet]
        public ActionResult ChangeApproval(string ID)
        {
       
            string comment1 = null;
            try
            {
                string Result = string.Empty;
                Result = DALobj.ChangeApproval(ID ,comment1);
               

            }
            catch (Exception)
            {
                //return View();
            }

            return RedirectToAction("LeaveManagementApprover");
        }

        public ActionResult RejectApprover(string ID , string comment)
        {
            try
            {
                string Result = string.Empty;
                Result = DALobj.RejectApprover(ID ,comment);


            }
            catch (Exception)
            {
                //return View();
            }

            return RedirectToAction("LeaveManagementApprover");
        }

        [HttpGet]
        public ActionResult MoveToApprovalList(string ID)
        {

            string comment1 = null;
            try
            {
                string Result = string.Empty;
                Result = DALobj.MoveToApprovalList(ID, comment1);


            }
            catch (Exception)
            {
                //return View();
            }

            return RedirectToAction("LeaveManagementApprover");
        }

        [HttpGet]

        public ActionResult LeaveApprovalList(string message)
        {
            //if (message == "Data Inserted Successfully")
            //{
            //    TempData["Success"] = "Data Inserted Successfully";
            //    TempData.Keep();
              

            //}
            //if (message == "Something went wrong")
            //{
            //    TempData["Error"] = "Something went wrong";
            //    TempData.Keep();


            //}
            Leave objLeave = new Leave();
            DataSet DSGetAllddllst = new DataSet();
            DataSet ds = new DataSet();
            List<NameCode> lstProjectType = new List<NameCode>();
            var Data = DALobj.GetBranchList();
            ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");
            DSGetAllddllst = DALobj.GetAllddlLst();
            if (DSGetAllddllst.Tables[0].Rows.Count > 0)
            {
                lstProjectType = (from n in DSGetAllddllst.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DSGetAllddllst.Tables[0].Columns["ProjectName"].ToString()),
                                      Code = n.Field<Int32>(DSGetAllddllst.Tables[0].Columns["PK_ID"].ToString())

                                  }).ToList();
            }
            ViewBag.OBSType = lstProjectType;
            ds = DALobj.GetLeaveApprovalDashboard();


            if (ds.Tables.Count > 0)
            {
                objLeave.dtLeave = ds.Tables[0];
            }
            return View(objLeave);

        }

        public JsonResult GetClusterHead(string Prefix)
        {
           DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (Convert.ToString(Session["FirstName"]) != null && Convert.ToString(Session["LastName"]) != "")
            {
                Session["FirstName"] = null;
            }
            if (Prefix != null && Prefix != "")
            {
                DTResult = DALobj.GetClusterHead(Prefix);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {
                               CompanyName = Convert.ToString(dr["CompanyName"]),
                               CompanyNames = Convert.ToString(dr["CompanyNames"]),
                               PkUserID = Convert.ToString(dr["pk_UserID"]),

                             

                               //PreviousClosing = Convert.ToDecimal(dr["BseCurrprice"]),
                           }

                         );
                    }
                    Session["CompanyName"] = Convert.ToString(DTResult.Rows[0]["CompanyName"]);
              return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public ActionResult LeaveApprovalList(Leave objLeave)
        {
            string Result = string.Empty;
            string message = null;
            try
            {
                
           Result = DALobj.InsertApprovalList(objLeave);

                if(Convert.ToInt32(Result) > 0)
                {
                    TempData["S"] = "S";
                    TempData.Keep();
                    return RedirectToAction("LeaveApprovalList", "Leave");
                    //string str = TempData.Peek("Message").ToString();
                    //return Redirect("LeaveApprovalList");
                    // return RedirectToAction("LeaveApprovalList", "Leave" , new {message=str });
                }
                 else
                {

                    TempData["E"] = "E";
                    TempData.Keep();
                    //string str = TempData.Peek("Message").ToString();
                    
                    return Redirect("LeaveApprovalList");
                }



            }

            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(objLeave);



        }

        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            //Adding New Code 7 March 2020
            List<FileDetails> fileDetails = new List<FileDetails>();
   
            List<FileDetails> FileLeave = new List<FileDetails>();
            //---Adding end Code
            if (Session["listUploadedFile"] != null)
            {
                fileDetails = Session["listUploadedFile"] as List<FileDetails>;
            }
              if (Session["listFormatFile"] != null)
            {
                FileLeave = Session["listFormatFile"] as List<FileDetails>;
            }

            try
            {

                FormCollection fc = new FormCollection();
                string filePath = string.Empty;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file
                    int fileSize = files.ContentLength;
                    if (files != null && files.ContentLength > 0)
                    {
                        if (files.FileName.ToUpper().EndsWith(".MSG") || files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".PDF") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpeg") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".PNG") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))
                        {

                            //string fileName = files.FileName;
                            string fileName = Regex.Replace(files.FileName, "[^a-zA-Z0-9_.]+", "_");
                            //Adding New Code as per new requirement 7 March 2020, Manoj Sharma
                            //FileDetails fileDetail = new FileDetails();
                            //fileDetail.FileName = fileName;
                            //fileDetail.Extension = Path.GetExtension(fileName);
                            //fileDetail.Id = Guid.NewGuid();
                            //fileDetails.Add(fileDetail);

                            FileDetails fileDetail = new FileDetails();
                            //fileDetail.FileName = fileName;
                            fileDetail.FileName = Regex.Replace(fileName, "[^a-zA-Z0-9_.]+", "_");
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();

                            BinaryReader br = new BinaryReader(files.InputStream);
                            byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                            fileDetail.FileContent = bytes;
                            if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD")
                            {
                                fileDetails.Add(fileDetail);
                            }
                            if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                            {
                                FileLeave.Add(fileDetail);
                            }
                            //-----------------------------------------------------
                          //  filePath = Path.Combine(Server.MapPath("~/Files/Documents/"), fileDetail.Id + fileDetail.Extension);
                            filePath = Path.Combine(Server.MapPath("~/Files/Documents/"), fileDetail.FileName + fileDetail.Extension);

                            var K = "~/Files/Documents/" + fileName;

                            IPath = K;//K.TrimStart('~');

                           /// files.SaveAs(filePath);

                            var ExistingUploadFile = IPath;
                            splitedGrp = ExistingUploadFile.Split(',');

                            foreach (var single in splitedGrp)
                            {
                                Selected.Add(single);
                            }
                            Session["list"] = Selected;

                        }
                        else
                        {
                            ViewBag.Error = "Please Select XLSX or PDF File";
                        }
                    }
                    

                }
                if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD")
                {
                    Session["listUploadedFile"] = fileDetails;
                }
             if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                {
                    Session["listFormatFile"] = FileLeave;
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }


     

        public FileResult Download1(string d, string TableType)
        {

            string FileName = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = DALobj.GetFileContent(d ,TableType);

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                FileName = DTDownloadFile.Rows[0]["FileName"].ToString();
                Date = DTDownloadFile.Rows[0]["CreatedDate"].ToString();
            }

            //string myDate = "05/11/2010";
            DateTime date = Convert.ToDateTime(Date);
            int year = date.Year;
            int Month = date.Month;

            int intC = Convert.ToInt32(Month);
            string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);


            //Build the File Path.
            //string path = Server.MapPath("~/Content/JobDocument/") + d;
            //var savePath = Path.Combine(Server.MapPath("~/IVRIRNSupportDocs/"), a + item.FileName);

            string path = Server.MapPath("~/Content/" + year + "/" + CurrentMonth + "/") + FileName;
            // string path = Server.MapPath("~/Content/") + d;

            //Read the File data into Byte Array.
            bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", FileName);
        }

        public ActionResult AllLeave()
        {
            Leave OBJLeave = new Leave();

            try
            {


                DataSet ds = new DataSet();

                List<Leave> lstLeave = new List<Leave>();
                Session["GetExcelData"] = "Yes";




                TempData.Keep();

                ds = DALobj.GetEmployeeDataSheetAll();


                if (ds.Tables.Count > 0)
                {
                    OBJLeave.dtLeave = ds.Tables[0];
                    ViewData["dtLeave"] = lstLeave;
                }

                DataTable dtUserLeave = new DataTable();

                dtUserLeave = DALobj.GetApprovalRejectEmployeeDataSheet();

                if (dtUserLeave.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUserLeave.Rows)
                    {


                        lstLeave.Add(
                           new Leave
                           {
                               UniversalID = Convert.ToString(dr["Universal_ID"]),
                               FirstName = Convert.ToString(dr["Name"]),
                               FromD = Convert.ToString(dr["StartDate"]),
                               Reason = Convert.ToString(dr["Reason"]),
                               ChkLTILeavestring = Convert.ToString(dr["ChkLTILeave"]),
                               uniqueNumber = Convert.ToString(dr["uniqueNumber"]),
                               Status = Convert.ToString(dr["Status"]),


                           }
                        );
                    }
                }


                OBJLeave.lst1 = lstLeave;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(OBJLeave);
        }


        public void LeaveApplicationMail(int LeaveID)
        {
            try
            {

                DataSet Details = new DataSet();
                MailMessage msg = new MailMessage();

                string strApp1 = string.Empty;
                string strApp2 = string.Empty;
                string strPCH = string.Empty;

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string bodyTxt = "";
                string ClientEmail = string.Empty;
                string displayName = string.Empty;

                ClientEmail = "pshrikant@tuv-nord.com";

                Details = DALobj.GetLeaveDetails(LeaveID);

                if (Details.Tables.Count > 0)
                {
                    bodyTxt = @"<html><head><title></title></head>
                    <body><div>
                        <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                        <div>&nbsp;</div>
                        <div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Leave application added by " + Details.Tables[1].Rows[0]["Creater"].ToString() + " of type " + Details.Tables[0].Rows[0]["ActivityType"].ToString() + " for  " + Details.Tables[0].Rows[0]["StartDate"].ToString()
                        + " to " + Details.Tables[0].Rows[0]["EndDate"].ToString() + ". Kindly Approve/Reject.</br></br>";
                    bodyTxt = bodyTxt + "</span></span></div>";
                    bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                    bodyTxt = bodyTxt + " TUV India Private Limited. " + "</br>";
                    bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                    bodyTxt = bodyTxt + "</body></html> ";

                    msg.From = new MailAddress(MailFrom, MailFrom);

                    msg.Bcc.Add("rohini@tuv-nord.com");
                    msg.Bcc.Add("pshrikant@tuv-nord.com");

                    // string To = ClientEmail.ToString();

                    char[] delimiters = new[] { ',', ';', ' ' };
                    //  string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (Details.Tables[1].Rows.Count > 0)
                    {
                        strApp1 = Details.Tables[1].Rows[0]["App1"].ToString();
                        strApp2 = Details.Tables[1].Rows[0]["App2"].ToString();
                        strPCH = Details.Tables[1].Rows[0]["PCHEmail"].ToString();



                        if (strApp1 == strApp2 && strApp2 == strPCH)
                        {
                            msg.To.Add(strApp1);
                        }
                        else
                        {
                            msg.To.Add(strApp1);
                            msg.To.Add(strApp2);
                            msg.To.Add(strPCH);
                        }
                        msg.CC.Add(Details.Tables[1].Rows[0]["CreatorEmail"].ToString());
                    }




                    msg.Subject = "TIIMES-Leave Application-" + Details.Tables[1].Rows[0]["Creater"].ToString() + "- (" + Details.Tables[0].Rows[0]["StartDate"].ToString() + "-" + Details.Tables[0].Rows[0]["EndDate"].ToString() + ")";
                    msg.Body = bodyTxt;
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.Normal;
                    SmtpClient client = new SmtpClient();
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
                    client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
                    client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                    client.EnableSsl = true;
                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
        }

        public void ApprovalMail(string LeaveID, string status)
        {
            try
            {

                DataSet Details = new DataSet();
                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string bodyTxt = "";
                string ClientEmail = string.Empty;
                string displayName = string.Empty;
                char[] delimiters = new[] { ',', ';', ' ' };

                ClientEmail = "pshrikant@tuv-nord.com";

                Details = DALobj.GetApproverDetails(LeaveID.ToString());


                if (Details.Tables.Count > 0)
                {
                    if (status == "APP")
                    {
                        bodyTxt = @"<html><head><title></title></head>
                        <body><div>
                        <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear " + Details.Tables[1].Rows[0]["Creater"].ToString() + " ,</span></span></div>";

                        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Your leave application of type " + Details.Tables[0].Rows[0]["ActivityType"].ToString() + " for  " + Details.Tables[1].Rows[0]["StartDate"].ToString() + " to " + Details.Tables[1].Rows[0]["EndDate"].ToString() + " has been Approved.</br></br>";
                        bodyTxt = bodyTxt + "</span></span></div>";
                        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                        bodyTxt = bodyTxt + " TUV India Private Limited. " + "</br></br>";
                        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                        bodyTxt = bodyTxt + "</body></html> ";
                    }
                    else if (status == "REJ")
                    {
                        bodyTxt = @"<html><head><title></title></head>
                        <body><div>
                        <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear " + Details.Tables[1].Rows[0]["Creater"].ToString() + " ,</span></span></div>";

                        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Your leave application of type " + Details.Tables[0].Rows[0]["ActivityType"].ToString() + " for  " + Details.Tables[1].Rows[0]["StartDate"].ToString() + " to " + Details.Tables[1].Rows[0]["EndDate"].ToString() + " has been Rejected.</br></br>";
                        bodyTxt = bodyTxt + "</span></span></div>";
                        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br>";
                        bodyTxt = bodyTxt + " TUV India Private Limited. " + "</br></br>";
                        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                        bodyTxt = bodyTxt + "</body></html> ";
                    }

                    msg.From = new MailAddress(MailFrom, MailFrom);


                    string To = ClientEmail.ToString();

                    string CCMails = ConfigurationManager.AppSettings["TIIMESTeam"].ToString();
                    string PayrollMails = ConfigurationManager.AppSettings["Payroll"].ToString();

                    if (Details.Tables[1].Rows[0]["CreatorEmail"].ToString() != null)
                        msg.To.Add(Details.Tables[1].Rows[0]["CreatorEmail"].ToString());

                    if (Details.Tables[1].Rows[0]["App1"].ToString() != null)
                        msg.CC.Add(Details.Tables[1].Rows[0]["App1"].ToString());

                    if (Details.Tables[1].Rows[0]["App2"].ToString() != null)
                        msg.To.Add(Details.Tables[1].Rows[0]["App2"].ToString());

                    string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string MultiEmailTemp in EmailIDs)
                    {
                        msg.To.Add(new MailAddress(MultiEmailTemp));
                    }

                    string[] strCCMails = CCMails.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string CCMailID in strCCMails)
                    {
                        msg.CC.Add(new MailAddress(CCMailID));
                    }

                    string[] strPayrollMails = PayrollMails.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string CCMailID1 in strPayrollMails)
                    {
                        msg.CC.Add(new MailAddress(CCMailID1));
                    }


                    msg.Subject = "Leave Application from " + Details.Tables[1].Rows[0]["Creater"].ToString();
                    msg.Body = bodyTxt;
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.Normal;

                    SmtpClient client = new SmtpClient();
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
                    client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
                    client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                    client.EnableSsl = true;
                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
        }

        //added by nikita on 23052024

        public ActionResult Abscond(int? Id)
        {
            Session["GetExcelData"] = "Yes";

            List<Leave> lmd = new List<Leave>();  // creating list of model.  
            DataSet ds = new DataSet();

            if (TempData["FromDate"] != null && TempData["ToDate"] != null)
            {
                objL.FromD = Convert.ToString(TempData["FromDate"]);
                objL.ToD = Convert.ToString(TempData["ToDate"]);
                TempData.Keep();
                ds = DALobj.GetDataByDate_(objL);
            }
            else
            {
                ds = DALobj.GetAbscondDashboard();
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new Leave
                    {

                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        DateSE = Convert.ToString(dr["DateSE"]),
                        FirstName = Convert.ToString(dr["Name"]),
                        LeaveAddedBy = Convert.ToString(dr["LeaveAddedBy"]),


                    });
                }

            }
            objL.LeaveDashBoard = lmd;
            ViewData["BranchList"] = lmd;


            var UserData = objDalCalls.GetInspectorListForLeaveManagement();
            ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");
            DataSet dss = new DataSet();
            if (Id != null)
            {
                dss = DALobj.GetDataById(Convert.ToInt32(Id));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    objL.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                    objL.FirstName = dss.Tables[0].Rows[0]["CreatedBy"].ToString();
                    objL.StartDate = dss.Tables[0].Rows[0]["DateSE"].ToString();
                    objL.EndDate = dss.Tables[0].Rows[0]["DateSE"].ToString();
                    objL.ActivityType = dss.Tables[0].Rows[0]["ActivityType"].ToString();
                    objL.Attachment = dss.Tables[0].Rows[0]["Attachment"].ToString();
                    objL.Reason = dss.Tables[0].Rows[0]["Reason"].ToString();
                }
                return View(objL);
            }
            //else
            //{
            //    return View();
            //}
            return View(objL);


            //return View();
        }

        [HttpPost]
        public ActionResult Abscond(Leave objL)
        {
            string Result;
            DataTable DTValidateTT = new DataTable();
            List<Leave> lmd = new List<Leave>();  // creating list of model.  
            DataTable DtchkLeaveExist = new DataTable();
            DataSet ds = new DataSet();

            string fromD = objL.FromD;
            string ToD = objL.ToD;

            TempData["FromDate"] = fromD;
            TempData["ToDate"] = ToD;

            if (TempData["FromDate"] != null && TempData["ToDate"] != null)
            {
                objL.FromD = Convert.ToString(TempData["FromDate"]);
                objL.ToD = Convert.ToString(TempData["ToDate"]);
                TempData.Keep();
                ds = DALobj.GetDataByDate_(objL);
            }
            else
            {
                ds = DALobj.GetAbscondDashboard();
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new Leave
                    {

                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        DateSE = Convert.ToString(dr["DateSE"]),
                        FirstName = Convert.ToString(dr["Name"]),
                        LeaveAddedBy = Convert.ToString(dr["LeaveAddedBy"]),


                    });
                }

            }
            objL.LeaveDashBoard = lmd;
            ViewData["BranchList"] = lmd;


            string username = objL.FirstName;
            string date = objL.StartDate;
            if (objL.Id > 0)
            {
                Start = Convert.ToString(objL.StartDate);
            }
            else
            {
                Result = DALobj.InsertUserLeaveForAbscond(objL);
                SendMAilOnAbscond(username, date);
            }
            return RedirectToAction("Abscond");

        }


        public void SendMAilOnAbscond(string username, string date)
        {
            string displayName = string.Empty;
            string ClientEmail = string.Empty;
            string bodyTxt = string.Empty;
            string currentsession = Session["fullName"].ToString();
            try
            {
                DataTable Details = DALobj.GetSatusForMail(username);
                if (Details.Rows.Count > 0)
                {
                    string inspectorName = Details.Rows[0]["Name"].ToString();
                    string AdminQA = Details.Rows[0]["AdminQA"].ToString();
                    string CH_Name = Details.Rows[0]["CH_Name"].ToString();
                    string ApprovalName_1 = Details.Rows[0]["ApprovalName_1"].ToString();
                    string BranchQA = Details.Rows[0]["BranchQA"].ToString();
                    string ApprovalName_2 = Details.Rows[0]["ApprovalName_2"].ToString();
                    string PCH_Name = Details.Rows[0]["PCH_Name"].ToString();
                    string CcExtra = " fredrick@tuv-nord.com";
                    string CCmails = "sshilpa@tuv-nord.com";
                    string CCmails_ = "psneha@tuv-nord.com";
                    string CCmails_hr = "mnikhil@tuv-nord.com";
                    string sunny = "sunnydc@tuv-nord.com";
                    
                    string rohini = "rohini@tuv-nord.com";
                    string shirish = "shirish@tuv-nord.com";
                    string shrikant = "pshrikant@tuv-nord.com";
                    string sunil = "sunilj@tuv-nord.com";
                    string support = "support.india@tuv-nord.com";

                    MailMessage msg = new MailMessage();
                    string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                    string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                    //string ToEmail = inspectoremail;
                    //string ToEmail = CCmails;

                    string ToEmail = $"{AdminQA};{BranchQA};{ApprovalName_1};{ApprovalName_2};{PCH_Name};{CH_Name}";
                    string CcEmail =$"{CcExtra};{CCmails};{CCmails_};{CCmails_hr};{sunny};{rohini};{shirish};{shrikant};{sunil};{support}";
                    bodyTxt = $@"
            <html>
                <head>
                    <title></title>
                </head>
                <body>
                    <div>

                        <br/><br/>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Please note that {inspectorName} employment status has been marked as absconding since {date} by {currentsession} today..</span>
                        <br/><br/>
                        <br/><br/>
                        
                    </div>
                    <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Best regards,<br/>TUV India Pvt Ltd.</span>
                    </div>
                    <br/>
                    <div>
                        <span style='font-size:12px; font-family:verdana,geneva,sans-serif;'>Note: This is an auto-generated mail. Please do not reply.</span>
                    </div>
                </body>
            </html>";

                    foreach (var email in ToEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        msg.To.Add(new MailAddress(email));
                    }

                    foreach (var email in CcEmail.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        msg.CC.Add(new MailAddress(email));
                    }

                    msg.From = new MailAddress(MailFrom, ClientEmail);
                    msg.Subject = "TIIMES – User - Absconding employee notification";
                    msg.Body = bodyTxt;
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.Normal;

                    SmtpClient client = new SmtpClient();
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                    client.Host = ConfigurationManager.AppSettings["smtpserver"];
                    client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"], ConfigurationManager.AppSettings["Password"]);
                    client.EnableSsl = true;
                    client.Send(msg);
                }
                else
                {
                    Console.WriteLine("No details found for the given call ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

