using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.Models;
using TuvVision.DataAccessLayer;
using System.Data;
using OfficeOpenXml;
using NonFactors.Mvc.Grid;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using OfficeOpenXml.Style;
using System.Globalization;
//using OfficeOpenXml.Style;
//using Microsoft.Office.Interop.Excel;

namespace TuvVision.Controllers
{
    public class QuizController : Controller
    {
        QuizModel ObjModel = new QuizModel();
        DALQuiz ObjDal = new DALQuiz();
        QuizModel obj = new QuizModel();
        
        // GET: Quiz
        [HttpGet]
        public ActionResult AddQuiz(int? qid, int? rid)
        {
            ViewBag.RollList = new SelectList(ObjDal.GetTopicList(), "TopicID", "Name");
            DataTable DSQuestion = new DataTable();
            if (qid > 0)
            { 
                DSQuestion = ObjDal.GetQuestionByID(Convert.ToInt32(qid));
                if (DSQuestion.Rows.Count > 0)
                {
                    ObjModel.QuizID = Convert.ToInt32(DSQuestion.Rows[0]["PK_ID"]);
                    ObjModel.Question = Convert.ToString(DSQuestion.Rows[0]["Question"]);
                    ObjModel.TrainingID = Convert.ToInt32(DSQuestion.Rows[0]["FK_TrainingID"]);
                    ObjModel.Option1 = Convert.ToString(DSQuestion.Rows[0]["Option1"]);
                    ObjModel.Option2 = Convert.ToString(DSQuestion.Rows[0]["Option2"]);
                    ObjModel.Option3 = Convert.ToString(DSQuestion.Rows[0]["Option3"]);
                    ObjModel.Option4 = Convert.ToString(DSQuestion.Rows[0]["Option4"]);
                    ObjModel.Answer = Convert.ToString(DSQuestion.Rows[0]["Answer"]);
                }                
            }
            ObjModel.TopicName = Convert.ToInt32(rid);
            return View(ObjModel);
        }
        [HttpPost]
        public ActionResult AddQuiz(QuizModel vm, HttpPostedFileBase FileUpload1)
        {
            // int i = ObjDal.AddQuiz(vm);

            int i = 0;
           int k = 0;
            #region  Excel Upload code
            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);
            string strmyRandomNo = Convert.ToString(myRandomNo);


            HttpPostedFileBase files = FileUpload1;

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


                    foreach (Microsoft.Office.Interop.Excel.Worksheet wSheet in excelBook.Worksheets)
                    {
                        excelSheets[k] = wSheet.Name;
                        int RowsCount = wSheet.UsedRange.Rows.Count;// - 1;

                        if (excelSheets[k] == "Sheet1")
                        {
                            for (int j = 2; j <= RowsCount; j++)
                            {
                               // string SrNo = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
                                if (vm.Question != null || vm.Question != "")
                                {
                                    vm.TopicName = vm.TopicName;
                                    vm.Question = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 1]).Value);
                                    vm.Option1 = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 2]).Value);
                                    vm.Option2 = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 3]).Value);
                                    vm.Option3 = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 4]).Value);
                                    vm.Option4 = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 5]).Value);
                                    vm.Answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[j, 6]).Value);



                                    i = ObjDal.AddQuiz(vm);
                                }


                            }
                        }



                        k++;
                    }
                    excelBook.Close();


                }

            }
            else
            {
                i = ObjDal.AddQuiz(vm);
            }
            #endregion


            int kid = vm.TopicName;
            if (i != 0)
            {
                TempData["Success"] = "Question Added Successfully...";
                TempData.Keep();
                return RedirectToAction("QuestionList", new { sid = Convert.ToInt32(kid) });
            }
            else
            {
                TempData["Error"] = "Something went Wrong, Please try Again !";
                TempData.Keep();
            }
            return RedirectToAction("QuestionList", new { sid = Convert.ToInt32(kid) });
        }
        public ActionResult DeleteQuestion(int? bid, int? tid)
        {
            try
            {
                if (ObjDal.DeleteRoll(Convert.ToInt32(bid)))
                {
                    TempData["Deleted"] = "Question Details Deleted Successfully ...";

                }
                return RedirectToAction("QuestionList", new { sid = Convert.ToInt32(tid) });
            }
            catch (Exception)
            {
                return View("QuestionList");
            }
        }
        public ActionResult QuestionList(int? sid)
        {

            #region Get Quize End Date
            List<QuizModel> lmd = new List<QuizModel>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = ObjDal.GetQuizeEndDate(sid); // fill dataset  

            if (ds.Tables[0].Rows.Count > 0)
            {
                obj.QuizeEndDate = Convert.ToString(ds.Tables[0].Rows[0]["QuizeEndDate"]);
                obj.QuizeEndTime1 = Convert.ToString(ds.Tables[0].Rows[0]["QuizeEndTime"]);
                obj.QuizStartDate = Convert.ToString(ds.Tables[0].Rows[0]["QuizStartDate"]);
                obj.ReattendQuizStartDate = Convert.ToString(ds.Tables[0].Rows[0]["ReattendQuizStartDate"]);
                obj.ReattendQuizEndDate = Convert.ToString(ds.Tables[0].Rows[0]["ReattendQuizEndDate"]);
                obj.QTimeInMinutes = Convert.ToString(ds.Tables[0].Rows[0]["QTimeInMinutes"]);

            }

           
                    #endregion
                    string TPName = string.Empty;
            TPName = ObjDal.GetTopicList().Where(m => m.TopicID == sid).FirstOrDefault().Name;
            ViewBag.TP = Convert.ToString(TPName);
            ViewBag.ID = Convert.ToInt32(sid);

            List<QuizModel> listqns = new List<QuizModel>();
            
            listqns = ObjDal.GetQuestionList(Convert.ToInt32(sid)).ToList();
            ViewBag.QuestionList = listqns;
            obj.lstFeedback = listqns;
            obj.QuizID = Convert.ToInt32(sid);
            //return View(listqns);
            return View(obj);
        }
        public ActionResult TopicList()
        {
            List<TopicList> listtopic = new List<TopicList>();
            listtopic = ObjDal.GetTopicList().ToList();
            ViewBag.AllTopicList = listtopic;
            return View(listtopic);
        }
        [HttpGet]
        public ActionResult GetQuiz(int? sid,String chkAtt, String Type1,string Reattempt)
        {

            string TPName = string.Empty;
            TPName = ObjDal.GetTopicList().Where(m => m.TopicID == sid).FirstOrDefault().Name;
            ViewBag.TP = Convert.ToString(TPName);
            DataSet VerifyQuiz = new DataSet();

            DataSet ds = new DataSet();
            #region Check Attendence 
            if (chkAtt=="AttChk")
            {
                DataSet dschkAtte = new DataSet();
                dschkAtte = ObjDal.ChkAtt(Convert.ToInt32(sid));
                if(dschkAtte.Tables[0].Rows.Count>0)
                {
                   
                }
                else
                {
                    TempData["Err"] = "Your Attendence is not marked please contact Organiser.";
                    TempData.Keep();
                    return RedirectToAction("TrainingDetails", "TrainingSchedule");
                }

            }

            #endregion


            if(Reattempt=="Yes")
            {
                DataSet dsReattemptDatechk = new DataSet();
                dsReattemptDatechk = ObjDal.ReattemptDatechk(Convert.ToInt32(sid));
                if (dsReattemptDatechk.Tables[0].Rows[0]["QuizDateVerification"].ToString() == "QuizNotSchedule" || dsReattemptDatechk.Tables[0].Rows[0]["QuizDateVerification"].ToString() == "QuizNotSchedule")
                {
                    TempData["Reattempt"] = "Reattempt quiz date is not mentioned. Please contact the administrator!";
                    TempData.Keep();
                    return RedirectToAction("TrainingDetails", "TrainingSchedule");
                }
                else if (dsReattemptDatechk.Tables[0].Rows[0]["QuizDateVerification"].ToString() == "QuizNotStart")
                {
                    TempData["Reattempt"] = "The reattempt quiz date has not started; please contact the admin.";
                    TempData.Keep();
                    return RedirectToAction("TrainingDetails", "TrainingSchedule");
                }
                else if (dsReattemptDatechk.Tables[0].Rows[0]["QuizDateVerification"].ToString() == "QuizDateExpired")
                {
                    TempData["Reattempt"] = "The reattempt quiz link has ended.";
                    TempData.Keep();
                    return RedirectToAction("TrainingDetails", "TrainingSchedule");
                }
                else
                {
                    VerifyQuiz = ObjDal.QuizVerification(Convert.ToInt32(sid));
                    ViewBag.QTimeInMinutes = Convert.ToString(VerifyQuiz.Tables[5].Rows[0]["QTimeInMinutes"]);   //413
                    ViewBag.Psid = Convert.ToInt32(sid);   //417

                    List<QuizQuestion> listqns1 = new List<QuizQuestion>();
                    listqns1 = ObjDal.GetQuizQuestion(Convert.ToInt32(sid)).ToList();
                    ViewBag.QuizQuestions = listqns1;
                    ViewBag.QuizVerify = "NotSubmitted";
                    return View();
                }

                    
            }
            else
            {
                
                VerifyQuiz = ObjDal.QuizVerification(Convert.ToInt32(sid));
                

                if (VerifyQuiz.Tables[0].Rows.Count > 0)
                {
                    ViewBag.QuizVerify = "Submitted";
                    ViewBag.PassFail = VerifyQuiz.Tables[3].Rows[0]["Result"].ToString();
                    ViewBag.NoOfAttempt = VerifyQuiz.Tables[4].Rows[0]["NoOfAttempt"].ToString();
                    ViewBag.sid = Convert.ToInt32(sid);



                    ds = ObjDal.GetTrainingByID(Convert.ToInt32(sid));

                    DateTime now2 = DateTime.Now;
                    DateTime date2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["QuizeEndDate"]);
                    string s2 = "";


                    int timereached2 = DateTime.Compare(now2.Date, date2.Date);
                    if (timereached2 >= 1)
                    {
                        ViewBag.ResultShow = "Yes";
                    }


                    if (VerifyQuiz.Tables[1].Rows.Count > 0)
                    {
                        ViewBag.Percent = Convert.ToString(VerifyQuiz.Tables[1].Rows[0]["TotalPercent"]);
                    }
                    else
                    {
                        ViewBag.Percent = Convert.ToString("0");
                    }
                    List<QuizQuestion> listqns1 = new List<QuizQuestion>();
                    listqns1 = ObjDal.GetQuizQuestion(Convert.ToInt32(sid)).ToList();
                    ViewBag.QuizQuestions = listqns1;
                    return View();


                }
                else
                {
                    ViewBag.QuizVerify = "NotSubmitted";
                }
            }

            


            

            TimeSpan time1 = TimeSpan.FromHours(24);
            DateTime Startdate;
            DateTime date;

            ds = ObjDal.GetTrainingByID(Convert.ToInt32(sid));
            DateTime now1 = DateTime.Now;

            if(ds.Tables[0].Rows[0]["QuizStartDate"].ToString()==null || ds.Tables[0].Rows[0]["QuizStartDate"].ToString() == "")
            {
                ViewBag.Result = "Quiz date not Start";

                TempData["QuizNotStart"] = "Quiz date not Start";
                TempData.Keep();
                if(Type1=="E")
                {
                    return RedirectToAction("ELearningTrainingDetail", "TrainingSchedule");
                }
                else
                {
                    return RedirectToAction("TrainingDetails", "TrainingSchedule");
                }
                
            }
            else if(ds.Tables[0].Rows[0]["QuizStartDate"].ToString()!=null || ds.Tables[0].Rows[0]["QuizStartDate"].ToString() == "")
            {
                Startdate = Convert.ToDateTime(ds.Tables[0].Rows[0]["QuizStartDate"]);
                int intDate = DateTime.Compare(now1.Date, Startdate.Date);
                if (intDate < 0)
                {
                    TempData["QuizNotStart"] = "Quiz date not Start";
                    TempData.Keep();
                    ViewBag.Result = "Quiz date not Start";
                    if (Type1 == "E")
                    {
                        return RedirectToAction("ELearningTrainingDetail", "TrainingSchedule");
                    }
                    else
                    {
                        return RedirectToAction("TrainingDetails", "TrainingSchedule");
                    }
                }

            }

             Startdate = Convert.ToDateTime(ds.Tables[0].Rows[0]["QuizStartDate"]);
             date = Convert.ToDateTime(ds.Tables[0].Rows[0]["QuizeEndDate"]);



            string s = "";

            int timereached1 = DateTime.Compare(now1.Date, date.Date);
            if(timereached1>=1)
            {
                ViewBag.ResultShow = "Yes";
                TempData["Err1"] = "Quize link expired please contact Organiser.";
                TempData.Keep();
                if(ViewBag.QuizVerify == "Submitted")
                {
                    return RedirectToAction("GetQuiz", "Quiz", new { sid = sid });
                }
                if (ViewBag.QuizVerify == "NotSubmitted")
                {
                    ViewBag.QuizVerify = "Submitted";
                    Reattempt = "Yes";
                    TempData["Err1"] = null;
                    return RedirectToAction("GetQuiz", "Quiz", new { sid = sid, Reattempt = "Yes" });//need to Test
                    return RedirectToAction("TrainingDetails", "TrainingSchedule");
                }
            }

            
            



            //DateTime _enddate = Convert.ToDateTime(ds.Tables[0].Rows[0]["TrainingEndDate"]);
            //DateTime _etime = Convert.ToDateTime(ds.Tables[0].Rows[0]["TrainingEndTime"]);
            //DateTime QuizeEndTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["QuizeEndTime"]);

            //TimeSpan QuizEtime = _etime.TimeOfDay;
            //DateTime QuizEDate = _enddate.Date;

            //DateTime _curdate = DateTime.Now.Date;
            //TimeSpan _curTime = DateTime.Now.TimeOfDay;

            //TimeSpan timetoCompare = QuizEtime.Add(time1);

            
            
            //int timereached = TimeSpan.Compare(QuizEtime, _curTime);

            

            #region Vaibhav start
            //DateTime ScheduleTime = _etime.AddDays(-1); 
            //var calculateHours = (ScheduleTime - _etime).TotalHours;

        
            //DateTime curent = DateTime.Now.Date;
            //DateTime fromDate =  ScheduleTime;
            //DateTime toDate = _etime;

            //DateTime start = ScheduleTime; 
            //DateTime end = _etime; 
            //DateTime now = DateTime.Now;


            //if ((now > start) && (now > QuizeEndTime))
            //{
            //    ViewBag.QzDate = "True";
            //}
            //else if (now > end)
            //{
            //    ViewBag.QzDate = "Expired";
            //}
            //else
            //{
            //    ViewBag.QzDate = "False";
            //}


            
            #endregion


            #region old logic
            //if (_curdate >= QuizEDate && timereached >= 0)
            //{
            //    ViewBag.QzDate = "True";
            //}
            //else
            //{
            //    ViewBag.QzDate = "False";
            //}
            #endregion



            string TPName1 = string.Empty;
            TPName = ObjDal.GetTopicList().Where(m => m.TopicID == sid).FirstOrDefault().Name;
            ViewBag.TP = Convert.ToString(TPName);
            ViewBag.ID = Convert.ToInt32(sid);
            
            VerifyQuiz = ObjDal.QuizVerification(Convert.ToInt32(sid));

            if (VerifyQuiz.Tables[0].Rows.Count > 0)
            {
                ViewBag.QuizVerify = "Submitted";
            }
            else
            {
                ViewBag.QuizVerify = "NotSubmitted";
            }

            if (VerifyQuiz.Tables[1].Rows.Count > 0)
            {
                ViewBag.Percent = Convert.ToString(VerifyQuiz.Tables[1].Rows[0]["TotalPercent"]);
            }
            else
            {
                ViewBag.Percent = Convert.ToString("0");
            }
            if (VerifyQuiz.Tables[2].Rows.Count > 0)
            {
                ViewBag.Attandance = Convert.ToString(VerifyQuiz.Tables[2].Rows[0]["IsPresent"]);
            }
            else
            {
                ViewBag.Attandance = Convert.ToString("0");
            }
            ViewBag.QTimeInMinutes = Convert.ToString(VerifyQuiz.Tables[5].Rows[0]["QTimeInMinutes"]);
            List<QuizQuestion> listqns = new List<QuizQuestion>();
            listqns = ObjDal.GetQuizQuestion(Convert.ToInt32(sid)).ToList();
            ViewBag.QuizQuestions = listqns;
            ViewBag.Psid = Convert.ToInt32(sid);
            return View();
        }

        /*public ActionResult GetQuiz(int? sid, DateTime edate, string etime)
        {
            TimeSpan time1 = TimeSpan.FromHours(24);
            DateTime _enddate = Convert.ToDateTime(edate);
            DateTime _etime = Convert.ToDateTime(etime);

            TimeSpan QuizEtime = _etime.TimeOfDay;
            DateTime QuizEDate = _enddate.Date;

            DateTime _curdate = DateTime.Now.Date;
            TimeSpan _curTime = DateTime.Now.TimeOfDay;
            
            TimeSpan timetoCompare = QuizEtime.Add(time1);


            /*  string[] str1a = QuizEtime.ToString().Split(':');
                string date = Convert.ToString(_enddate);
                  string time = Convert.ToString(_etime);
                string str1 = date.Substring(0, 11);

                string[] str1a = str1.Split('-');
              string time = Convert.ToString(_etime);

              string str1b = Convert.ToString(str1a[1]) + "-" + Convert.ToString(str1a[0]) + "-" + Convert.ToString(str1a[2]);

              string str2 = time.Substring(time.Length - 8);
              string str3 = Convert.ToString(str1b + str2);

              DateTime QuizSDT = Convert.ToDateTime(str3);
              DateTime QuizEDT = QuizSDT.AddHours(24);
              DateTime _curdate = DateTime.Now;

              if (_curdate >= QuizSDT && _curdate <= QuizEDT)
              {
                  ViewBag.QzDate = "True";
              }
              else
              {
                  ViewBag.QzDate = "False";
              }   
              */
        /* int timereached = TimeSpan.Compare(QuizEtime, _curTime);

         if(_curdate >=QuizEDate && timereached >= 0)
         {
             ViewBag.QzDate = "True";
         }
         else
         {
             ViewBag.QzDate = "False";
         }


         string TPName = string.Empty;
         TPName = ObjDal.GetTopicList().Where(m => m.TopicID == sid).FirstOrDefault().Name;
         ViewBag.TP = Convert.ToString(TPName);
         ViewBag.ID = Convert.ToInt32(sid);

         DataSet VerifyQuiz = new DataSet();
         VerifyQuiz = ObjDal.QuizVerification(Convert.ToInt32(sid));

         if (VerifyQuiz.Tables[0].Rows.Count > 0)
         {
             ViewBag.QuizVerify = "Submitted";
         }
         else
         {
             ViewBag.QuizVerify = "NotSubmitted";
         }

         if (VerifyQuiz.Tables[1].Rows.Count > 0)
         {
             ViewBag.Percent = Convert.ToString(VerifyQuiz.Tables[1].Rows[0]["TotalPercent"]);
         }
         else
         {
             ViewBag.Percent = Convert.ToString("0");
         }
         if (VerifyQuiz.Tables[2].Rows.Count > 0)
         {
             ViewBag.Attandance = Convert.ToString(VerifyQuiz.Tables[2].Rows[0]["IsPresent"]);
         }
         else
         {
             ViewBag.Attandance = Convert.ToString("0");
         }
         List<QuizQuestion> listqns = new List<QuizQuestion>();
         listqns = ObjDal.GetQuizQuestion(Convert.ToInt32(sid)).ToList();
         ViewBag.QuizQuestions = listqns;
         return View();
     }*/


        //added by nikita on 23102023
        public ActionResult GetExpense(int? sid, String chkAtt, String Type1, string TrainingStartDate)
        {
            try
            {
                #region Check Attendence 
                if (chkAtt == "AttChk")
                {
                    DataSet dschkAtte = new DataSet();
                    dschkAtte = ObjDal.ChkAtt(Convert.ToInt32(sid));
                    if (dschkAtte.Tables[0].Rows.Count > 0)
                    {
                      
                        return RedirectToAction("CreateExpenseItem", "ExpenseItem", new { Date = TrainingStartDate, Type = Type1, FKId = sid });
                    }
                    else
                    {
                        TempData["Err"] = "Your Attendence is not marked please contact Organiser.";
                        TempData.Keep();
                        return RedirectToAction("TrainingDetails", "TrainingSchedule");
                    }

                }

                #endregion
            }
            catch(Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult GetQuiz(QuizModel QM)
        {
            string Answer = string.Empty;
            string Result = string.Empty;
            foreach (var item in QM.QuizQuestion)
            {
                ObjModel.TrainingID = Convert.ToInt32(item.TrainingID);
                ObjModel.QuizID = Convert.ToInt32(item.TQuizID);
                ObjModel.UserID = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
                if (item.TOption1 != "" && item.TOption1 != null)
                {
                    ObjModel.Answer = Convert.ToString(item.TOption1);
                }
                //else if (item.TOption2 != "" && item.TOption2 != null)
                //{
                //    ObjModel.Answer = Convert.ToString(item.TOption2);
                //}
                //else if (item.TOption3 != "" && item.TOption3 != null)
                //{
                //    ObjModel.Answer = Convert.ToString(item.TOption3);
                //}
                //else if (item.TOption4 != "" && item.TOption4 != null)
                //{
                //    ObjModel.Answer = Convert.ToString(item.TOption4);
                //}
                else
                {
                    ObjModel.Answer = Convert.ToString("Missed");
                }
                Answer = ObjDal.AddQuizAnswers(ObjModel);              
            }
            Result = ObjDal.CalculateResult(ObjModel);
            return RedirectToAction("TrainingDetails", "TrainingSchedule");
        }

        public ActionResult ResultList(int? rid)
        {            
            List<QuizModel> listqns = new List<QuizModel>();
            listqns = ObjDal.GetResultList(Convert.ToInt32(rid)).ToList();
            ObjModel.PK_TrainingScheduleId = Convert.ToString(rid);
            ViewBag.ResultList = listqns;


            List<QuizModel> listQ = new List<QuizModel>();
            listQ = ObjDal.GetQList(Convert.ToInt32(rid)).ToList();
            ViewBag.Question = listQ;

            List<QuizModel> listA = new List<QuizModel>();
            listA = ObjDal.GetAList(Convert.ToInt32(rid)).ToList();
            ViewBag.Answer = listA;



            return View(ObjModel);
        }
        public ActionResult FeedbackList(int? fid)
        {
            ViewBag.id = Convert.ToInt32(fid);
            List<QuizModel> listfeedback = new List<QuizModel>();
            listfeedback = ObjDal.GetFeedbackList(Convert.ToInt32(fid)).ToList();
            ViewBag.FeedbackList = listfeedback;
            return View(listfeedback);
        }

        #region export to excel Feedback Report
        [HttpGet]
        public ActionResult ExportFeedbackReport(int? sid)
        {
            Session["FeedbackID"] = 0;
            Session["FeedbackID"] = Convert.ToInt32(sid);            
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<QuizModel> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;
                    column.IsEncoded = false;
                }
                foreach (IGridRow<QuizModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                    sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
                    row++;
                }
                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<QuizModel> CreateExportableGrid()
        {
            IGrid<QuizModel> grid = new Grid<QuizModel>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            //grid.Columns.Add(model => model.Date).Titled("Date");
            grid.Columns.Add(model => model.UserID).Titled("Trainee Name");
            grid.Columns.Add(model => model.C1).Titled("Were you aware of the aims and objectives of the programme.");
            grid.Columns.Add(model => model.C2).Titled("The duration of the session was");
            grid.Columns.Add(model => model.C3).Titled("The training programme met your expectations"); 
            grid.Columns.Add(model => model.C4).Titled("The content of the training was relevant to the objectives of the training");
            grid.Columns.Add(model => model.C5).Titled("As a result of the training, I will be able to do my job more effectively");
            grid.Columns.Add(model => model.D1).Titled("Knowledge and command of the subject");
            grid.Columns.Add(model => model.D2).Titled("Presentation/Communication skills");
            grid.Columns.Add(model => model.D3).Titled("Question handling skills");
            grid.Columns.Add(model => model.D4).Titled("Gestures, body language and eye contact of the trainer");
            grid.Columns.Add(model => model.E1).Titled("How was the Administrative arrangements for the programme");
            grid.Columns.Add(model => model.F1).Titled("Summary");
            grid.Columns.Add(model => model.G1).Titled("Overall Rating of the programme on 4 point scale");

            grid.Pager = new GridPager<QuizModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModel.lstFeedback.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }
            return grid;
        }
        public List<QuizModel> GetData()
        {
            int sid = Convert.ToInt32(Session["FeedbackID"]);
            List<QuizModel> lmd = new List<QuizModel>();
            DataSet ds = new DataSet();
            ds = ObjDal.GetData(sid);  
             
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new QuizModel
                    {
                        UserID = Convert.ToString(dr["TraineeName"]),
                        C1 = Convert.ToString(dr["Aim"]),
                        C2 = Convert.ToString(dr["Duration"]),
                        C3 = Convert.ToString(dr["Expectation"]),
                        C4 = Convert.ToString(dr["PContent"]),
                        C5 = Convert.ToString(dr["Result"]),
                        D1 = Convert.ToString(dr["Command"]),
                        D2 = Convert.ToString(dr["Presentation"]),
                        D3 = Convert.ToString(dr["Question"]),
                        D4 = Convert.ToString(dr["Gesture"]),
                        E1 = Convert.ToString(dr["Arrangement"]),
                        F1 = Convert.ToString(dr["Summary"]),
                        G1 = Convert.ToString(dr["Rating"]),
                    });
                }
            }
            ViewData["FeedbckReport"] = lmd;
            ObjModel.lstFeedback = lmd;
            return ObjModel.lstFeedback;
        }
        #endregion


        public ActionResult ExportSetQuizeList(string PK_TrainingScheduleId)
        {
            var products = new System.Data.DataTable("teste");

            //DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
            DataTable dtCount = new DataTable("Grid");
            dtCount = ObjDal.ExportSetQuizeList(PK_TrainingScheduleId);
            //dtCount = objDAL.ExportToExcelWinner();


            var grid = new GridView();
            grid.DataSource = dtCount;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename=ExportFeedBackList.xls");
            Response.AddHeader("content-disposition", "attachment; filename=ExportSetQuizeList.xls");

            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            //TrainingAttendance(int PK_TrainingScheduleId);
            return RedirectToAction("ExportFeedBackList", "TrainingSchedule", new { tid = Convert.ToInt32(PK_TrainingScheduleId) });
            //return View();
        }


        public ActionResult ExportResultList1(string PK_TrainingScheduleId)
        {
            var products = new System.Data.DataTable("teste");

            DataTable dtCount = new DataTable("Grid");
            dtCount = ObjDal.ExportSetQuizeList(PK_TrainingScheduleId);


            var grid = new GridView();
            grid.DataSource = dtCount;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ResultList.xls");

            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("ResultList", "Quiz", new { rid = Convert.ToInt32(PK_TrainingScheduleId) });

        }

        public ActionResult ExportResultList(int c)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                int rowI = 1;
                string strRow = "A" + rowI;
                Int32 row = 1;
                Int32 col = 1;


                package.Workbook.Worksheets.Add("Data");
                IGrid<QuizModel> grid1 = CreateExportableGrid1(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];
                
                
                int colcount1 = 0;
                foreach (IGridColumn column1 in grid1.Columns)
                {
                    sheet.Cells[row, col].Value = column1.Title;
                    sheet.Column(col++).Width = 18;
                    column1.IsEncoded = false;
                    colcount1++;
                }



                double finalAmount1 = 0;
                row++;
                foreach (IGridRow<QuizModel> gridRow1 in grid1.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid1.Columns)
                    {
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow1);
                    }
                    //finalAmount1 = finalAmount1 + Convert.ToDouble(sheet.Cells[row, colcount1].Value.ToString());   // Convert.ToDouble(grid.Rows[row]["INRamount"]);

                    row++;
                }
                row++;
                sheet.Cells[row, 1, row, colcount1].Style.Border.Top.Style = ExcelBorderStyle.Thick;

                sheet.Cells[row++, colcount1].Value = finalAmount1;

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<QuizModel> CreateExportableGrid1(int c)
        {
            IGrid<QuizModel> grid1 = new Grid<QuizModel>(GetData1(c));
            grid1.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid1.Columns.Add(model => model.Question).Titled("Question");
            

            grid1.Pager = new GridPager<QuizModel>(grid1);
            grid1.Processors.Add(grid1.Pager);
            grid1.Pager.RowsPerPage = ObjModel.lstHeaderQuetion.Count;

            foreach (IGridColumn column in grid1.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }


            return grid1;
        }

        public List<QuizModel> GetData1(int c)
        {
            List<QuizModel> lmd = new List<QuizModel>();
            List<QuizModel> lmd1 = new List<QuizModel>(); // creating list of model.  
            DataSet ds = new DataSet();

            Session["GetExcelData"] = "Yes";


            ds = ObjDal.GetHeaderQuetion(c);




            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd1.Add(new QuizModel
                {

                    Question = Convert.ToString(dr["Question"]),
                    


                });
            }
            ObjModel.lstHeaderQuetion = lmd1;



            return ObjModel.lstHeaderQuetion;

        }


        [HttpPost]
        public ActionResult SubmitQ(QuizModel TSM)
        {
            string ReturnScheduleId;
            ReturnScheduleId = ObjDal.InsertEndDate(TSM);
            return Json(new { result = "Redirect", url = Url.Action("QuestionList", "Quiz", new { @sid = TSM.PK_TrainingScheduleId }) });
        }


        #region chart example

        //public ActionResult Index()
        //{
        //    // Mock data
        //    var model = new List<DrilldownChartModel>
        //{
        //    new DrilldownChartModel
        //    {
        //        Category = "Category 1",
        //        Value = 30,
        //        DrilldownData = new List<DrilldownItem>
        //        {
        //            new DrilldownItem { SubCategory = "Subcategory 1", SubValue = 10 },
        //            new DrilldownItem { SubCategory = "Subcategory 2", SubValue = 20 }
        //        }
        //    },
        //    new DrilldownChartModel
        //    {
        //        Category = "Category 2",
        //        Value = 50,
        //        DrilldownData = new List<DrilldownItem>
        //        {
        //            new DrilldownItem { SubCategory = "Subcategory 3", SubValue = 30 },
        //            new DrilldownItem { SubCategory = "Subcategory 4", SubValue = 20 }
        //        }
        //    }
        //};

        //    return View(model);
        //}

        #endregion

        public ActionResult Q2(int sid, int qTime)
        {
            ViewBag.Time = qTime;
            List<QuizQuestion> listqns = new List<QuizQuestion>();
            listqns = ObjDal.GetQuizQuestion(Convert.ToInt32(sid)).ToList();
            ViewBag.QuizQuestions = listqns;

            return View();
        }


    }
}