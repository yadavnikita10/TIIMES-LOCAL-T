using Newtonsoft.Json;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{



    public class TrainingScheduleController : Controller
    {
        // GET: TrainingSchedule
        CommonControl objCommonControl = new CommonControl();
        DALTrainingSchedule objDTS = new DALTrainingSchedule();
        DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
        DataTable DTTraineeId = new DataTable();
        DALQuiz ObjDal = new DALQuiz();

        TrainingRecordById model = new TrainingRecordById();

        TrainingScheduleModel objTSM = new TrainingScheduleModel();

        public ActionResult TrainingSchedule(string UserID)
        {
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



            //// user Roles
            if (DSEditGetList.Tables[1].Rows.Count > 0)
            {
                lstEditUserList = (from n in DSEditGetList.Tables[1].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(DSEditGetList.Tables[1].Columns["RoleName"].ToString()),
                                       Code = n.Field<Int32>(DSEditGetList.Tables[1].Columns["UserRoleID"].ToString())

                                   }).ToList();
            }
            IEnumerable<SelectListItem> UserNameItems;
            UserNameItems = new SelectList(lstEditUserList, "Code", "Name");
            ViewData["RoleNameItems"] = UserNameItems;
            ViewBag.UserRole = lstEditUserList;


            //// Scope
            if (DSEditGetList.Tables[2].Rows.Count > 0)
            {
                lstScope = (from n in DSEditGetList.Tables[2].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DSEditGetList.Tables[2].Columns["ScopeName"].ToString()),
                                      Code = n.Field<Int32>(DSEditGetList.Tables[2].Columns["PK_ID"].ToString())

                                  }).ToList();
            }
            IEnumerable<SelectListItem> ScopeItems;
            ScopeItems = new SelectList(lstScope, "Code", "Name");
            ViewData["Scopes"] = ScopeItems;
            ViewBag.Scope = lstScope;

            if (DSEditGetList.Tables[3].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
            {
                lstEditBranchList = (from n in DSEditGetList.Tables[3].AsEnumerable()
                                     select new NameCode()
                                     {
                                         Name = n.Field<string>(DSEditGetList.Tables[3].Columns["Branch_Name"].ToString()),
                                         Code = n.Field<Int32>(DSEditGetList.Tables[3].Columns["Br_Id"].ToString())
                                     }).ToList();
            }
            IEnumerable<SelectListItem> BranchNameItems;
            BranchNameItems = new SelectList(lstEditBranchList, "Code", "Name");
            ViewData["BranchNameItems"] = BranchNameItems;
            objTSM.Status = Convert.ToString("No");

            if (DSEditGetList.Tables[4].Rows.Count > 0)
            {
                lstEmploymentCategory = (from n in DSEditGetList.Tables[4].AsEnumerable()
                            select new NameCode()
                            {
                                Name = n.Field<string>(DSEditGetList.Tables[4].Columns["Name"].ToString()),
                                Code = n.Field<Int32>(DSEditGetList.Tables[4].Columns["Id"].ToString())

                            }).ToList();
            }
            IEnumerable<SelectListItem> lstEmploymentCategory1;
            lstEmploymentCategory1 = new SelectList(lstEmploymentCategory, "Code", "Name");
            ViewData["EmpCategory"] = lstEmploymentCategory1;
            ViewBag.EmpCategory = lstEmploymentCategory1;


            if (DSEditGetList.Tables[5].Rows.Count > 0)
            {
                listTrainCate = (from n in DSEditGetList.Tables[5].AsEnumerable()
                                 select new NameCode()
                                 {
                                     Name = n.Field<string>(DSEditGetList.Tables[5].Columns["Name"].ToString()),
                                     Code = n.Field<Int32>(DSEditGetList.Tables[5].Columns["Id"].ToString())

                                 }).ToList();
            }
            IEnumerable<SelectListItem> lstTrainCate;
            lstTrainCate = new SelectList(listTrainCate, "Code", "Name");
            ViewData["TrainCategory"] = lstTrainCate;
            ViewBag.TrainCategory = lstTrainCate;

            DataTable DTEditUser = new DataTable();


            #region Domestic Order Type 
            DataTable dtDOrderType = new DataTable();
            List<TrainingTime> lstDOrderType = new List<TrainingTime>();

            dtDOrderType = objDTS.GetTrainingStartTimeEndTime(UserID);
            if (dtDOrderType.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDOrderType.Rows)
                {
                    lstDOrderType.Add(
                       new TrainingTime
                       {

                           TrainingStartDate = Convert.ToString(dr["TrainingStartDate"]),
                           TrainingStartTime = Convert.ToString(dr["TrainingStartTime"]),
                           TrainingEndTime = Convert.ToString(dr["TrainingEndTime"]),
                           TotalHours = Convert.ToString(dr["TotalHours"]),
                          

                           



                       }
                     );
                }
                ViewBag.lstDOrderType = lstDOrderType;

            }
            #endregion



            if (UserID != "" && UserID != null)
            {

                #region Get File

                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDTS.GetTrainingScheduleFile(UserID);

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
                    ViewData["lstEditFileDetails"] = lstEditFileDetails;
                    objTSM.FileDetails = lstEditFileDetails;
                }
                #endregion


                DTEditUser = objDTS.GetTrainingDeatils(UserID);

                if (DTEditUser.Rows.Count > 0)
                {
                    objTSM.PK_TrainingScheduleId = Convert.ToInt32(DTEditUser.Rows[0]["PK_TrainingScheduleId"]);  
                    objTSM.TrainerId = Convert.ToString(DTEditUser.Rows[0]["TrainerId"]);
                    objTSM.TraineeId = Convert.ToString(DTEditUser.Rows[0]["TraineeId"]);
                    objTSM.TrainingStartDate = Convert.ToString(DTEditUser.Rows[0]["TrainingStartDate"]);
                    objTSM.TrainingEndDate = Convert.ToString(DTEditUser.Rows[0]["TrainingEndDate"]);
                    objTSM.TrainingStartTime = Convert.ToString(DTEditUser.Rows[0]["TrainingStartTime"]);
                    objTSM.TrainingEndTime = Convert.ToString(DTEditUser.Rows[0]["TrainingEndTime"]);
                    objTSM.TrainerName = Convert.ToString(DTEditUser.Rows[0]["TrainerName"]);
                    objTSM.TraineeName = Convert.ToString(DTEditUser.Rows[0]["TraineeName"]);
                    objTSM.BranchName = Convert.ToString(DTEditUser.Rows[0]["BranchName"]);
                    objTSM.ExternalTrainer = Convert.ToString(DTEditUser.Rows[0]["ExternalTrainer"]);
                    objTSM.TrainingRecordId = Convert.ToString(DTEditUser.Rows[0]["TrainingRecordId"]);
                    objTSM.Branch = Convert.ToString(DTEditUser.Rows[0]["Branch"]);
                    objTSM.TotalHours = Convert.ToString(DTEditUser.Rows[0]["TotalHours"]);
                    objTSM.Venue = Convert.ToString(DTEditUser.Rows[0]["Venue"]);
                    objTSM.Scope = Convert.ToString(DTEditUser.Rows[0]["Scope"]);
                    objTSM.Status = Convert.ToString(DTEditUser.Rows[0]["Status"]);
                    //objTSM.chkAllEmployee = Convert.ToString(DTEditUser.Rows[0]["chkAllEmployee"]);
                    objTSM.QuizeTimeInHours = Convert.ToString(DTEditUser.Rows[0]["QuizeTimeInHours"]);
                    objTSM.TrainingName = Convert.ToString(DTEditUser.Rows[0]["TrainingTopic"]);
                    objTSM.EvaluationMethod = Convert.ToString(DTEditUser.Rows[0]["EvaluationMethod"]);
                    objTSM.Category = Convert.ToString(DTEditUser.Rows[0]["Category"]);
                    objTSM.TrainType = Convert.ToString(DTEditUser.Rows[0]["TrainType"]);
                    objTSM.TypeOfEmployee = Convert.ToString(DTEditUser.Rows[0]["TypeOfEmployee"]);
                    objTSM.EmployementCategory = Convert.ToString(DTEditUser.Rows[0]["EmployementCategory"]);
                    objTSM.ProjectType = Convert.ToString(DTEditUser.Rows[0]["ProjectType"]);
                    objTSM.FileName = Convert.ToString(DTEditUser.Rows[0]["FileName"]);
                    objTSM.QuizeEndDate = Convert.ToString(DTEditUser.Rows[0]["QuizeEndDate"]);
                    objTSM.QuizeEndTime1 = Convert.ToString(DTEditUser.Rows[0]["QuizeEndTime"]);
                    objTSM.Link = Convert.ToString(DTEditUser.Rows[0]["Link"]);


                    ViewData["TrainCategorychecked"] = Convert.ToString(DTEditUser.Rows[0]["Category"]);
                    ViewData["OBSTypeChecked"] = Convert.ToString(DTEditUser.Rows[0]["ProjectType"]);
                    ViewData["EmpCategorychecked"] = Convert.ToString(DTEditUser.Rows[0]["EmployementCategory"]);
                    ViewData["ListBranchchecked"] = Convert.ToString(DTEditUser.Rows[0]["Branch"]);
                    ViewData["ListScopeschecked"] = Convert.ToString(DTEditUser.Rows[0]["Scope"]);
                    ViewData["UserRolechchecked"] = Convert.ToString(DTEditUser.Rows[0]["UserRole"]);

                }
            }




            return View(objTSM);
        }


        public JsonResult GetRecord(string prefix)

        {
            DataSet dsTopic = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            dsTopic = objDTS.GetTopicList(prefix);
            List<TrainingScheduleModel> searchlist = new List<TrainingScheduleModel>();

            foreach (DataRow dr in dsTopic.Tables[0].Rows)

            {

                searchlist.Add(new TrainingScheduleModel
                {
                    TrainingName = dr["TrainingTopic"].ToString(),
                    TrainingId = Convert.ToInt32(dr["Id"])
                });

            }
            //var getdata = (from n in searchlist
            //               where n.TrainingName.StartsWith(prefix)
            //               select new { label = n.TrainingName, value = n.TrainingId });
            return Json(searchlist, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetTrainerName(string prefix)
        {
            /*
            DataSet dsTopic = new DataSet();
            
            dsTopic = objDTS.GetTrainerName(prefix);
            List<TrainingScheduleModel> searchlist = new List<TrainingScheduleModel>();

            foreach (DataRow dr in dsTopic.Tables[0].Rows)
            {
                searchlist.Add(
                
                new TrainingScheduleModel
                {
                    //TrainerName = dr["TrainerName"].ToString(),
                    //TrainerId = dr["TrainerId"].ToString(),
                    TrainerName = Convert.ToString(dr["TrainerName"]),                    
                    TrainerId = Convert.ToString(dr["TrainerId"]),


                });
                return Json(searchlist, JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);

            */

            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();
            if (prefix != null && prefix != "")
            {
                DTResult = objDalEnquiryMaster.GetTrainerNames(prefix);

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

                           }
                        );
                    }
                    Session["CompanyNames"] = Convert.ToString(DTResult.Rows[0]["CompanyNames"]);
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }


            return Json("Failed", JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetTraineeName(string Prefix, string obsType, string EmpCat, string TypeOfEmp, string Branch, string TrainType, string UserRole)
        {
            DataTable dsTopic = new DataTable();
            // DataSet ds = dblayer.GetName(prefix);
            if (Prefix != null && Prefix != "")
            {
                dsTopic = objDalEnquiryMaster.GetTraineeName(Prefix, obsType, EmpCat, TypeOfEmp, Branch, TrainType, UserRole);
                List<EnquiryMaster> searchlist = new List<EnquiryMaster>();

                if (dsTopic.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsTopic.Rows)
                    {
                        searchlist.Add(new EnquiryMaster
                        {
                            CompanyName = Convert.ToString(dr["CompanyName"]),
                            CompanyNames = Convert.ToString(dr["CompanyNames"]),
                            PkUserID = Convert.ToString(dr["pk_UserID"]),
                        });

                    }
                    return Json(searchlist, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("Failed", JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetBranchName(string prefix)

        {
            DataSet dsTopic = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            dsTopic = objDTS.GetBranchName(prefix);
            List<TrainingScheduleModel> searchlist = new List<TrainingScheduleModel>();

            foreach (DataRow dr in dsTopic.Tables[0].Rows)

            {

                searchlist.Add(new TrainingScheduleModel
                {
                    BranchName = dr["BranchName"].ToString(),
                    BranchId = Convert.ToInt32(dr["BranchId"])
                });

            }
            return Json(searchlist, JsonRequestBehavior.AllowGet);

        }


        int iTrainingId;
        string ITrainerId; string ITraineeId; int IBranchId;


        [HttpPost]
        public ActionResult ScheduleTraining(TrainingScheduleModel TSM, FormCollection FC, List<TrainingTime> DArray)
        {
            //Fetch Training Id
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["DocsUploaded"] as List<FileDetails>;

            List<FileDetails> lstFileDtls1 = new List<FileDetails>();
            lstFileDtls1 = Session["FILEUPLOAD1"] as List<FileDetails>;
            

            string CTrainingId = TSM.TrainingName;
       
            CTrainingId = CTrainingId.TrimEnd(',');
            DataSet DSTrainingId = new DataSet();
            DSTrainingId = objDTS.GetTrainingIdByName(CTrainingId);
            if (DSTrainingId.Tables[0].Rows.Count > 0)
            {
                iTrainingId = Convert.ToInt32(DSTrainingId.Tables[0].Rows[0]["Id"]);
            }

            //Fetch Trainer name
            if (TSM.ExternalTrainer != string.Empty && TSM.ExternalTrainer !=null)
            {

            }
            else
            {
                string CTrainerName = TSM.TrainerName;

                CTrainerName = CTrainerName.TrimEnd(',');
                DataTable DSTrainerId = new DataTable();

                DSTrainerId = objDTS.GetTraineeIdByName(CTrainerName);

                if (DSTrainerId.Rows.Count > 0)
                {
                    ITrainerId = String.Join(",", DSTrainerId.AsEnumerable().Select(x => x.Field<string>("PK_UserID").ToString()).ToArray());

                }
            }



            //Fetch Trainee Id
            string CTraineeId = string.Empty;
            DataTable DSTraineeId = new DataTable();

            if (TSM.TypeOfEmployee.ToUpper() != "FORALLEMP")
            {
                CTraineeId = TSM.TraineeName;
                CTraineeId = CTraineeId.TrimEnd(',');
                

                DSTraineeId = objDTS.GetTraineeIdByName(CTraineeId);

                if (DSTraineeId.Rows.Count > 0)
                {
                    ITraineeId = String.Join(",", DSTraineeId.AsEnumerable().Select(x => x.Field<string>("PK_UserID").ToString()).ToArray());

                }
            }
            else
            {
                DSTraineeId = objDTS.GetAllTraineeIdByName(CTraineeId,TSM.ProjectType,TSM.EmployementCategory,TSM.TypeOfEmployee,TSM.Branch,TSM.TrainType,TSM.UserRole);
                if (DSTraineeId.Rows.Count > 0)
                {
                    ITraineeId = String.Join(",", DSTraineeId.AsEnumerable().Select(x => x.Field<string>("PK_UserID").ToString()).ToArray());

                }

            }

            string RemoveLastComa = ITraineeId.TrimEnd(',');
            List<string> TraineeIDForReport = RemoveLastComa.Split(',').ToList();




            //Fetch Branch Name
            string CBranchId = TSM.BranchName;
            // string IBranchId;
            
            DataTable DSBranchId = new DataTable();
            DSBranchId = objDTS.GetBranchIdByName(CBranchId);
            if (DSBranchId.Rows.Count > 0)
            {
                
                IBranchId = Convert.ToInt16(DSBranchId.Rows[0]["Br_Id"]);

              
            }


           

            string Result = string.Empty;
            int ReturnScheduleId = 0;
            try
            {


                ReturnScheduleId = objDTS.Insert(TSM, iTrainingId, ITrainerId, ITraineeId, IBranchId);

                #region Insert Training Start Date
                if (DArray != null)
                {
                    foreach (var d in DArray)
                    {





                        TSM.TrainingStartDate = d.TrainingStartDate;
                        TSM.TrainingStartTime = d.TrainingStartTime;
                        TSM.TrainingEndTime = d.TrainingEndTime;
                        TSM.TotalHours = d.TotalHours;
                        TSM.PK_TrainingScheduleId = ReturnScheduleId;
                        
                       int Result1 = objDTS.InsertTrainingStartTimeEndTime(TSM);
                    }
                }
                #endregion


                if (ReturnScheduleId != null && ReturnScheduleId != 0)
                {
                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        Result = objDTS.InsertFileAttachment(lstFileDtls, ReturnScheduleId);
                        Session["DocsUploaded"] = null;
                    }
                    if (lstFileDtls1 != null && lstFileDtls1.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtls1, ReturnScheduleId);
                        Result = objDTS.InsertTrainingScheduleFileAttachment(lstFileDtls1, ReturnScheduleId);
                        Session["FILEUPLOAD1"] = null;
                    }

                }

           

                if (Convert.ToInt16(ReturnScheduleId) > 0)
                {
                    ModelState.Clear();
                    TempData["message"] = "Record Added Successfully...";
                }
                else
                {
                    TempData["message"] = "Something went Wrong! Please try Again";
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                return Json(new { result = "error", url = Url.Action("TrainingSchedule", "TrainingSchedule", new { @UserID = ReturnScheduleId }) });
               
            }
            return Json(new { result = "Redirect", url = Url.Action("TrainingSchedule", "TrainingSchedule", new { @UserID = ReturnScheduleId }) });
           /// return RedirectToAction("ListScheduleTraining", "TrainingSchedule");
        }



        [HttpGet]
        public ActionResult ListScheduleTraining()
        {
            List<TrainingScheduleModel> lmd = new List<TrainingScheduleModel>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objDTS.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new TrainingScheduleModel
                {
                    PK_TrainingScheduleId = Convert.ToInt32(dr["PK_TrainingScheduleId"]),
                    TrainingId = Convert.ToInt32(dr["TrainingId"]),
                    TrainingName = dr["TopicName"].ToString(),
                    TrainerId = dr["TrainerId"].ToString(),
                    TraineeId = dr["TraineeId"].ToString(),
                    //  BranchId = Convert.ToInt16(dr["BranchId"]),
                    SBranchId = dr["BranchId"].ToString(),
                    TrainingStartDate = Convert.ToString(dr["TrainingStartDate"]),
                    //TrainingEndDate = Convert.ToString(dr["TrainingEndDate"]),
                    //TrainingStartTime = dr["TrainingStartTime"].ToString(),
                    //TrainingEndTime = dr["TrainingEndTime"].ToString(),
                    TrainerName = dr["TrainerName"].ToString(),
                    TraineeName = dr["TraineeName"].ToString(),
                    BranchName = dr["BranchName"].ToString(),
                    ExternalTrainer = dr["ExternalTrainer"].ToString(),
                    Venue = dr["Venue"].ToString(),
                    Scope = dr["Scope"].ToString(),
                    Remarks = dr["Remarks"].ToString(),
                    TotalHours = dr["TotalHours"].ToString(),
                    Status = dr["Status"].ToString(),
                    QuizeTimeInHours = dr["QuizeTimeInHours"].ToString(),
                    FileName = dr["FileName"].ToString(),
                    Link = dr["Link"].ToString(),
                    TrainType = dr["TrainType"].ToString(),
                    TrainingScheduleFor = dr["TrainingScheduleFor"].ToString(),
                    TrainingAttended = dr["TrainingAttended"].ToString(),
                    Pass = dr["Pass"].ToString(),
                    Failed = dr["Failed"].ToString(),
                    Feedback = dr["Feedback"].ToString(),

                });
            }
          
            return View(lmd.ToList());

        }



        [HttpGet]
        public ActionResult UpdateScheduleTraining(int? id)
        {
            int xxx = 0;
            DataSet dss = new DataSet();
            DataSet dss1 = new DataSet();
            DataTable dss2 = new DataTable();
            DataSet dss3 = new DataSet();
            var model = new TrainingScheduleModel();
            var model1 = new TrainingScheduleModel();

            dss = objDTS.GetDataById(Convert.ToInt32(id));

            if (dss.Tables[3].Rows.Count > 0)
            {
                ViewBag.AttandanceMarked = Convert.ToString("AttendanceMarked");
                model.Status = Convert.ToString("Yes");
            }
            else
            {
                ViewBag.AttandanceMarked = Convert.ToString("AttendanceNotMarked");
                model.Status = Convert.ToString("No");
            }


            List<AccessorNameCode> lstAccessorName = new List<AccessorNameCode>();

            dss1 = objDTS.GetAccessorName();

            if (dss1.Tables[0].Rows.Count > 0)
            {
                lstAccessorName = (from n in dss1.Tables[0].AsEnumerable()
                                   select new AccessorNameCode()
                                   {
                                       AccessorN = n.Field<string>(dss1.Tables[0].Columns["AccessorName"].ToString()),
                                       AccessorCode = n.Field<string>(dss1.Tables[0].Columns["AccessorId"].ToString())

                                   }).ToList();
            }
            ViewBag.AccessorName = lstAccessorName;
            //ViewData["AccessorName"] = lstAccessorName;
            if (dss.Tables[0].Rows.Count > 0)
            {
                model.PK_TrainingScheduleId = Convert.ToInt32(dss.Tables[0].Rows[0]["PK_TrainingScheduleId"]);
                model.TrainingName = dss.Tables[0].Rows[0]["TopicName"].ToString();
                model.TrainerName = dss.Tables[0].Rows[0]["TrainerName"].ToString();
                model.TraineeName = dss.Tables[0].Rows[0]["TraineeName"].ToString();
                //model.BranchName   =  dss.Tables[0].Rows[0]["BranchName"].ToString();
                model.ExternalTrainer = dss.Tables[0].Rows[0]["ExternalTrainer"].ToString();
                model.BranchName = dss.Tables[0].Rows[0]["BranchName"].ToString();
                model.TrainingStartDate = Convert.ToString(dss.Tables[0].Rows[0]["TrainingStartDate"]);
                model.TrainingEndDate = Convert.ToString(dss.Tables[0].Rows[0]["TrainingEndDate"]);
                model.TrainingStartTime = dss.Tables[0].Rows[0]["TrainingStartTime"].ToString();
                model.TrainingEndTime = dss.Tables[0].Rows[0]["TrainingEndTime"].ToString();
                model.TraineeId = dss.Tables[0].Rows[0]["TraineeId"].ToString();
                model.TotalHours = dss.Tables[0].Rows[0]["TotalHours"].ToString();
                model.Venue = dss.Tables[0].Rows[0]["Venue"].ToString();
                model.Scope = dss.Tables[0].Rows[0]["Scope"].ToString();
                model.Remarks = dss.Tables[0].Rows[0]["Remarks"].ToString();
                model.chkAllEmployee = Convert.ToBoolean(Convert.ToInt32(dss.Tables[0].Rows[0]["chkAllEmployee"].ToString()));
                model.checkBoxBranch = Convert.ToBoolean(Convert.ToInt32( dss.Tables[0].Rows[0]["chkBoxBranch"].ToString()));
                model.QuizeTimeInHours = Convert.ToString(Convert.ToInt32(dss.Tables[0].Rows[0]["QuizeTimeInHours"].ToString()));
            }

           
            dss2 = objDTS.GetTrainingRecordDataById(Convert.ToInt32(id));
            List<TrainingRecordList> lstTrainingRecordList = new List<TrainingRecordList>();

            if (dss2.Rows.Count > 0)
            {
                model.Attachment = dss2.Rows[0]["Attachment"].ToString();
                foreach (DataRow dr in dss2.Rows)
                {

                    lstTrainingRecordList.Add(new TrainingRecordList
                    {
                        RTrainingScheduleId = Convert.ToInt32(dr["TrainingScheduleId"]),

                        RTraineeId = Convert.ToString(dr["TraineeId"]),

                        RTraineeName = Convert.ToString(dr["TraineeName"]),
                        //booldata = Convert.ToInt32(dr["IsPresent"] is DBNull ? false : Convert.ToInt32(dr["IsPresent"])),

                        RIsPresent = Convert.ToBoolean(dr["IsPresent"] is DBNull ? false : Convert.ToBoolean(dr["IsPresent"])),
                        //RIsPresent = Convert.ToBoolean(booldata),
                        //RAccessorName = Convert.ToString(dr["AccessorId"]),
                        AccessorCode = Convert.ToString(dr["AccessorId"]),
                        
                    }
                );
                }
            }

            ViewData["TrainingRecord"] = lstTrainingRecordList;




            //Bind Trainee name in Training Record

            string RemoveLastComa = model.TraineeName.TrimEnd(',');
            List<string> lTraineeName = RemoveLastComa.Split(',').ToList();
            ViewBag.v = lTraineeName;



            //Bind TrainingScheduleId
            ViewBag.TrainingScheduleId = model.PK_TrainingScheduleId;







            string RemoveLastComaTraineeId = model.TraineeId.TrimEnd(',');
            List<string> lTraineeId = RemoveLastComaTraineeId.Split(',').ToList();
            ViewBag.NTraineeId = lTraineeId;
            // Session["ABC"] = lTraineeId;



            // List<TraineeNameId> lTraineeNameId = new List<TraineeNameId>();

            //lTraineeNameId = (from n in dss.Tables[0].AsEnumerable()
            //                   select new TraineeNameId()
            //                   {
            //                       TraineeNameForRecord = lTraineeName,
            //                       TraineeIdForRecord = n.Field<string>(dss.Tables[0].Columns["TraineeId"].ToString())

            //                   }).ToList();

            
            return View(model);



        }



        [HttpPost]
        public ActionResult UpdateScheduleTraining(TrainingScheduleModel N, int? Id, HttpPostedFileBase File, HttpPostedFileBase[] Image, FormCollection f)
        {

            if (Image.Count() > 0)
            {
                foreach (HttpPostedFileBase item in Image)
                {
                    HttpPostedFileBase image = item;
                    if (image != null && image.ContentLength > 0)
                    {
                        string filePath = AppDomain.CurrentDomain.BaseDirectory + "TrainingRecordDocument\\" + image.FileName;
                        const string ImageDirectoryFP = "TrainingRecordDocument\\";
                        const string ImageDirectory = "~/TrainingRecordDocument/";
                        string ImagePath = "~/TrainingRecordDocument/" + image.FileName;
                        string fileNameWithExtension = System.IO.Path.GetExtension(image.FileName);
                        string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
                        string ImageName = image.FileName;

                        int iteration = 1;

                        while (System.IO.File.Exists(Server.MapPath(ImagePath)))
                        {
                            ImagePath = string.Concat(ImageDirectory, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                            filePath = string.Concat(ImageDirectoryFP, fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                            ImageName = string.Concat(fileNameWithoutExtension, "-", iteration, fileNameWithExtension);
                            iteration += 1;
                        }

                        if (iteration == 1)
                        {
                            image.SaveAs(filePath);
                        }
                        else
                        {
                            image.SaveAs(AppDomain.CurrentDomain.BaseDirectory + filePath);
                        }
                        N.Attachment += ImageName + ",";
                    }
                }
            }
            string Result = string.Empty;
            try
            {
                //TSM, iTrainingId, ITrainerId, ITraineeId, IBranchId

                //foreach (TrainingScheduleModel t in customers)

                //{
                string r = f["TraineeId"];
                // List<string> s = (List<string>)Session["ABC"];


                objTSM.Attachment = N.Attachment;
             /*   foreach (var item in N.TrainingRecordList)
                {


                    objTSM.AccessorName = item.AccessorN;
                    objTSM.AccessorId = item.AccessorCode;
                    objTSM.RIsPresent = item.RIsPresent;
                    objTSM.RTraineeId = item.RTraineeId;
                    objTSM.RTrainingScheduleId = N.RTrainingScheduleId;
                    Result = objDTS.AddTrainingRecord(objTSM);
                }



                if (Convert.ToInt16(Result) > 0)
                {

                    ModelState.Clear();
                }
                else
                {
                    TempData["message"] = "Something went Wrong! Please try Again";
                }*/

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            
            //Fetch Training Id
            string CTrainingId = N.TrainingName;
            // string ITrainerId;
            // int iTrainingId;
            CTrainingId = CTrainingId.TrimEnd(',');
            DataSet DSTrainingId = new DataSet();
            DSTrainingId = objDTS.GetTrainingIdByName(CTrainingId);
            if (DSTrainingId.Tables[0].Rows.Count > 0)
            {
                iTrainingId = Convert.ToInt32(DSTrainingId.Tables[0].Rows[0]["Id"]);
                //int abc = Convert.ToInt32(DSTrainingId.Tables[0].Rows[0]["Id"].ToString());
                //++iTrainingId+',';
            }

            //Fetch Trainer name

            string CTrainerName = N.TrainerName;
            //string ITrainerId;
            CTrainerName = CTrainerName.TrimEnd(',');
            DataTable DSTrainerId = new DataTable();

            DSTrainerId = objDTS.GetTraineeIdByName(CTrainerName);

            if (DSTrainerId.Rows.Count > 0)
            {
                ITrainerId = String.Join(",", DSTrainerId.AsEnumerable().Select(x => x.Field<string>("PK_UserID").ToString()).ToArray());

            }

            string CTraineeId = f["TraineeID"];
            //string ITraineeId;
            CTraineeId = CTraineeId.TrimEnd(',');
            //Fetch Trainee Id
       /*     if (ViewBag.AttandanceMarked != "AttendanceMarked")
            {
               
                DataTable DSTraineeId = new DataTable();

                DSTraineeId = objDTS.GetTraineeIdByName(CTraineeId);

                if (DSTraineeId.Rows.Count > 0)
                {
                    ITraineeId = String.Join(",", DSTraineeId.AsEnumerable().Select(x => x.Field<string>("PK_UserID").ToString()).ToArray());

                }
            }*/
            //Fetch Branch Name
            string CBranchId = N.BranchName;
            // string IBranchId;
            CBranchId = CBranchId.TrimEnd(',');
            DataTable DSBranchId = new DataTable();
            DSBranchId = objDTS.GetBranchIdByName(CBranchId);
            if (DSBranchId.Rows.Count > 0)
            {
               // IBranchId = string.Join(",", DSBranchId.AsEnumerable().Select(x => x.Field<String>("Br_Id").ToString()).ToArray());
                //int CIBranchId = Convert.ToInt32(IBranchId);
                IBranchId = Convert.ToInt32(DSBranchId.Rows[0]["Br_Id"]);
            }





            // string Result = string.Empty;
            try
            {
                //TSM, iTrainingId, ITrainerId, ITraineeId, IBranchId

                int ReturnScheduleId = 0;
                //ReturnScheduleId = objDTS.Update(N, iTrainingId, ITrainerId, ITraineeId, IBranchId);
                ReturnScheduleId = objDTS.Update(N, iTrainingId, ITrainerId, CTraineeId, IBranchId);

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

            return RedirectToAction("ListScheduleTraining", "TrainingSchedule");

        }
        #region Added by Ankush 
        [HttpGet]
        public ActionResult TrainingAttendance(int? tid)
        {
            var model = new TrainingScheduleModel();
            List<TrainingRecordList> TraineeList = new List<TrainingRecordList>();
            DataSet dss = new DataSet();

            DataTable Validate = new DataTable();
            Validate = objDTS.ValidateRecord(Convert.ToInt32(tid));
            if (Validate.Rows.Count > 0)
            {
                foreach (DataRow dr in Validate.Rows)
                {
                    TraineeList.Add(
                        new TrainingRecordList
                        {
                            EmpType = Convert.ToString(dr["EmpType"]),
                            RTraineeId = Convert.ToString(dr["Trainee_ID"]),
                            RTraineeName = Convert.ToString(dr["TraineeName"]),
                            RIsPresent = Convert.ToBoolean(dr["IsPresent"]),

                            Branch = Convert.ToString(dr["Branch"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),

                            PK_ID =Convert.ToInt32(dr["PK_ID"])
                        });
                }
                ViewBag.Trainee = TraineeList;
                dss = objDTS.GetDataById(Convert.ToInt32(tid));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    model.PK_TrainingScheduleId = Convert.ToInt32(dss.Tables[0].Rows[0]["PK_TrainingScheduleId"]);
                    ViewBag.Data = model.PK_TrainingScheduleId;
                    model.TrainingName = dss.Tables[0].Rows[0]["TopicName"].ToString();
                    ViewBag.TrngNm = Convert.ToString(dss.Tables[0].Rows[0]["TopicName"].ToString());
                    model.TrainerName = dss.Tables[0].Rows[0]["TrainerName"].ToString();
                    model.TraineeName = dss.Tables[0].Rows[0]["TraineeName"].ToString();
                    model.TraineeId = dss.Tables[0].Rows[0]["TraineeId"].ToString();
                    model.TrainType= dss.Tables[0].Rows[0]["TrainType"].ToString();
                    //model.TrainerId = dss.Tables[0].Rows[0]["TrainerId"].ToString();
                }
            }
            else
            {                
                dss = objDTS.GetDataById(Convert.ToInt32(tid));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    model.PK_TrainingScheduleId = Convert.ToInt32(dss.Tables[0].Rows[0]["PK_TrainingScheduleId"]);
                    ViewBag.Data = model.PK_TrainingScheduleId;
                    model.TrainingName = dss.Tables[0].Rows[0]["TopicName"].ToString();
                    ViewBag.TrngNm = Convert.ToString(dss.Tables[0].Rows[0]["TopicName"].ToString());
                    model.TrainerName = dss.Tables[0].Rows[0]["TrainerName"].ToString();
                    model.TraineeName = dss.Tables[0].Rows[0]["TraineeName"].ToString();
                    model.TraineeId = dss.Tables[0].Rows[0]["TraineeId"].ToString();
                    model.EmployementCategory = dss.Tables[0].Rows[0]["EmployementCategory"].ToString();
                    model.TrainType = dss.Tables[0].Rows[0]["TrainType"].ToString();
                    model.TrainerId = dss.Tables[0].Rows[0]["TrainerId"].ToString();

                }
                DataTable GetUserData = new DataTable();

                GetUserData = objDTS.GetTraineeData(model.TraineeId, model.TrainerId);
                if (GetUserData.Rows.Count > 0)
                {
                    foreach (DataRow dr in GetUserData.Rows)
                    {
                        TraineeList.Add(
                            new TrainingRecordList
                            {
                                EmpType = Convert.ToString(dr["EmpType"]),
                                RTraineeId = Convert.ToString(dr["PK_UserID"]),
                                RTraineeName = Convert.ToString(dr["TraineeName"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                Email = Convert.ToString(dr["Email"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                EmployementCategory = Convert.ToString(dr["EmployementCategory"]),
                            });
                    }
                    ViewBag.Trainee = TraineeList;
                }
            }            
            return View(model);
        }
        [HttpPost]
        public ActionResult TrainingAttendance(TrainingScheduleModel N, FormCollection fc)
        {
            string Result = string.Empty;
            foreach (var item in N.TrainingRecordList)
            {
                if (item.PK_ID == 0)
                {
                    objTSM.RIsPresent = item.RIsPresent;
                    objTSM.RTraineeId = item.RTraineeId;
                    objTSM.RTrainingScheduleId = N.RTrainingScheduleId;
                    objTSM.TraineeName = item.RTraineeName;
                    objTSM.EmpType = item.EmpType;
                    Result = objDTS.AddTrainingAttendance(objTSM);
                }
                else
                {
                    objTSM.PK_ATID = item.PK_ID;
                    objTSM.RIsPresent = item.RIsPresent;
                    objTSM.RTraineeId = item.RTraineeId;
                    objTSM.RTrainingScheduleId = N.RTrainingScheduleId;
                    objTSM.TraineeName = item.RTraineeName;
                    objTSM.EmpType = item.EmpType;
                    Result = objDTS.AddTrainingAttendance(objTSM);
                }
                
            }
            return RedirectToAction("TrainingAttendance", new { @tid=Convert.ToInt32(N.RTrainingScheduleId)});
        }
        #endregion

        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objDTS.Delete(Convert.ToInt32(Id));
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
            return RedirectToAction("ListScheduleTraining");
        }

        [HttpGet]
        public ActionResult TrainingDetails()
        {
            List<TrainingScheduleModel> lmd = new List<TrainingScheduleModel>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objDTS.GetDetails(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new TrainingScheduleModel
                {
                    PK_TrainingScheduleId = Convert.ToInt32(dr["PK_TrainingScheduleId"]),
                    TrainingId = Convert.ToInt32(dr["TrainingId"]),
                    TrainingName = dr["TopicName"].ToString(),
                    TrainerId = dr["TrainerId"].ToString(),
                    TraineeId = dr["TraineeId"].ToString(),                    
                    SBranchId = dr["BranchId"].ToString(),
                    TrainingStartDate = Convert.ToString(dr["TrainingStartDate"]),
                    //TrainingEndDate = Convert.ToString(dr["TrainingEndDate"]),
                    //TrainingStartTime = dr["TrainingStartTime"].ToString(),
                    //TrainingEndTime = dr["TrainingEndTime"].ToString(),

                    TrainerName = dr["TrainerName"].ToString(),
                    TraineeName = dr["TraineeName"].ToString(),
                    BranchName = dr["BranchName"].ToString(),
                    ExternalTrainer = dr["ExternalTrainer"].ToString(),
                    TrainType = dr["TrainType"].ToString(),
                    Venue = dr["Venue"].ToString(),
                    Link = dr["Link"].ToString(),
                    FileName = dr["FileName"].ToString(),
                    MobileNo = dr["MobileNo"].ToString(),
                    TotalHours = dr["TotalHours"].ToString(),
                    CreatedBy = dr["OrganiserName"].ToString(),
                    Scope = dr["Scope"].ToString(),
                    QuizButtonVisibleOnList = dr["QuizButtonVisibleOnList"].ToString(),
                    QuizeStartDate = dr["QuizStartDate"].ToString(),
                    QuizeEndDate = dr["QuizeEndDate"].ToString(),
                    EvaluationMethod = dr["EvaluationMethod"].ToString(),



                });
            }

            ViewBag.TrainingList = lmd;
            return View(lmd.ToList());
        }

        [HttpGet]
        public ActionResult ELearningTrainingDetail()
        {
            List<TrainingScheduleModel> lmd = new List<TrainingScheduleModel>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objDTS.GetElearningDetails(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new TrainingScheduleModel
                {
                    PK_TrainingScheduleId = Convert.ToInt32(dr["PK_TrainingScheduleId"]),
                    TrainingId = Convert.ToInt32(dr["TrainingId"]),
                    TrainingName = dr["TopicName"].ToString(),
                    TrainerId = dr["TrainerId"].ToString(),
                    TraineeId = dr["TraineeId"].ToString(),
                    SBranchId = dr["BranchId"].ToString(),
                    TrainingStartDate = Convert.ToString(dr["TrainingStartDate"]),
                    //TrainingEndDate = Convert.ToString(dr["TrainingEndDate"]),
                    //TrainingStartTime = dr["TrainingStartTime"].ToString(),
                    //TrainingEndTime = dr["TrainingEndTime"].ToString(),
                    Link = dr["Link"].ToString(),
                    TrainerName = dr["TrainerName"].ToString(),
                    TraineeName = dr["TraineeName"].ToString(),
                    BranchName = dr["BranchName"].ToString(),
                    ExternalTrainer = dr["ExternalTrainer"].ToString(),
                    TrainType = dr["TrainType"].ToString(),
                    Venue = dr["Venue"].ToString(),
                    FileName = dr["FileName"].ToString(),
                    MobileNo = dr["MobileNo"].ToString(),
                    CreatedBy = dr["OrganiserName"].ToString(),
                    Scope = dr["Scope"].ToString(),
                    EvaluationMethod = dr["EvaluationMethod"].ToString(),
                    QuizeStartDate = dr["QuizStartDate"].ToString(),
                    QuizeEndDate = dr["QuizeEndDate"].ToString(),

                });
            }

            ViewBag.TrainingList = lmd;
            return View(lmd.ToList());
        }

        [HttpGet]
        public ActionResult TrainingFeedBack(int? fid, String chkAtt, String Type1)
        {

            #region Check Attendence 
            if (chkAtt == "AttChk")
            {
                DataSet dschkAtte = new DataSet();
                dschkAtte = ObjDal.ChkAtt(Convert.ToInt32(fid));
                if (dschkAtte.Tables[0].Rows.Count > 0)
                {
                    
                }
                else
                {
                    TempData["Err"] = "Your Attendence is not mark please contact Organiser.";
                    TempData.Keep();
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

            #endregion

            DataSet ds = new DataSet();

            TimeSpan time1 = TimeSpan.FromHours(24);

            ds = ObjDal.GetTrainingByID(Convert.ToInt32(fid));

            DateTime now1 = DateTime.Now;
            if(ds.Tables[0].Rows[0]["QuizeEndDate"].ToString()!="" && ds.Tables[0].Rows[0]["QuizeEndDate"].ToString() != null)
            {
                DateTime date = Convert.ToDateTime(ds.Tables[0].Rows[0]["QuizeEndDate"]);
            }
            else
            {

            }
          
            
            
            //TimeSpan time = TimeSpan.Parse("20:30");
            //DateTime result = date + time;
            string s = "";

            //14/04/23
            //int timereached1 = DateTime.Compare(now1.Date, date.Date);
            //if (timereached1 >= 1)
            //{
            //    TempData["Err1"] = "Quize link expired please contact Organiser.";
            //    TempData.Keep();
            //    return RedirectToAction("TrainingDetails", "TrainingSchedule");
            //}

            var model = new TrainingScheduleModel();            
            DataSet dss = new DataSet();
            DataTable GetData = new DataTable();

            model.tfid= Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

            dss = objDTS.GetDataById(Convert.ToInt32(fid));
            if (dss.Tables[0].Rows.Count > 0)
            {
                model.PK_TrainingScheduleId = Convert.ToInt32(dss.Tables[0].Rows[0]["PK_TrainingScheduleId"]);
                ViewBag.Data = model.PK_TrainingScheduleId;
                model.TrainingName = dss.Tables[0].Rows[0]["TopicName"].ToString();
                ViewBag.TrngNm = Convert.ToString(dss.Tables[0].Rows[0]["TopicName"].ToString());
                model.TrainerName = dss.Tables[0].Rows[0]["TrainerName"].ToString();
                model.TraineeName = dss.Tables[0].Rows[0]["TraineeName"].ToString();
                model.TraineeId = dss.Tables[0].Rows[0]["TraineeId"].ToString();
            }
            if (dss.Tables[1].Rows.Count > 0)
            {
                ViewBag.Attandance = Convert.ToString(dss.Tables[1].Rows[0]["IsPresent"]);
            }
            else
            {
                ViewBag.Attandance = Convert.ToString("0");
            }
            if (dss.Tables[2].Rows.Count > 0)
            {
                ViewBag.FeedbackVerify = "Submitted";
            }
            else
            {
                ViewBag.FeedbackVerify = "NotSubmitted";
            }
            GetData = objDTS.GetFeedbackData(Convert.ToInt32(fid), Convert.ToString(model.tfid));
            if (GetData.Rows.Count > 0)
            {
                foreach (DataRow dr in GetData.Rows)
                {

                    model.FFID = Convert.ToInt32(dr["PK_ID"]);
                    model.C1 = Convert.ToString(dr["Aim"]);
                    model.C2 = Convert.ToString(dr["Duration"]);
                    model.C3 = Convert.ToString(dr["Expectation"]);
                    model.C4 = Convert.ToString(dr["PContent"]);
                    model.C5 = Convert.ToString(dr["Result"]);
                    model.D1 = Convert.ToString(dr["Command"]);
                    model.D2 = Convert.ToString(dr["Presentation"]);
                    model.D3 = Convert.ToString(dr["Question"]);
                    model.D4 = Convert.ToString(dr["Gesture"]);
                    model.E1 = Convert.ToString(dr["Arrangement"]);
                    model.F1 = Convert.ToString(dr["Summary"]);
                    model.G1 = Convert.ToString(dr["Rating"]);
                    model.FeedbackFile = Convert.ToString(dr["FeedbackFile"]);
                    model.SubmitCount = Convert.ToString(dr["SubmitCount"]);

                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult TrainingFeedBack(TrainingScheduleModel TF)
        {
            int i = 0;
            if (TF.FFID == 0)
            {
                #region Create Training Feedback 
                i = objDTS.AddTrainingFeedback(TF);

                DataSet GetUserData = new DataSet();
                GetUserData = objDTS.GetPDFData(TF);
                if (GetUserData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in GetUserData.Tables[0].Rows)
                    {
                        objTSM.FullName = Convert.ToString(GetUserData.Tables[0].Rows[0]["FullName"]);
                        objTSM.Designation = Convert.ToString(GetUserData.Tables[0].Rows[0]["Designation"]);
                        objTSM.Branch = Convert.ToString(GetUserData.Tables[0].Rows[0]["Branch_Name"]);
                    }
                }
                if (GetUserData.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in GetUserData.Tables[1].Rows)
                    {
                        objTSM.AccessorName = Convert.ToString(GetUserData.Tables[1].Rows[0]["TopicName"]);
                        objTSM.BranchName = Convert.ToString(GetUserData.Tables[1].Rows[0]["BranchName"]);
                        objTSM.TrainingStartDate = Convert.ToString(GetUserData.Tables[1].Rows[0]["TrainingStartDate"]);
                        objTSM.TrainingEndDate = Convert.ToString(GetUserData.Tables[1].Rows[0]["TrainingEndDate"]);
                    }
                }
                if (GetUserData.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in GetUserData.Tables[2].Rows)
                    {
                        objTSM.C1 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Aim"]);
                        objTSM.C2 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Duration"]);
                        objTSM.C3 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Expectation"]);
                        objTSM.C4 = Convert.ToString(GetUserData.Tables[2].Rows[0]["PContent"]);
                        objTSM.C5 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Result"]);
                        objTSM.D1 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Command"]);
                        objTSM.D2 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Presentation"]);
                        objTSM.D3 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Question"]);
                        objTSM.D4 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Gesture"]);
                        objTSM.E1 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Arrangement"]);
                        objTSM.F1 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Summary"]);
                        objTSM.G1 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Rating"]);
                    }
                }

                #region Generate PDF
                System.Text.StringBuilder strs = new System.Text.StringBuilder();
                string body = string.Empty;
                string CQ1 = string.Empty;
                string CQ2 = string.Empty;
                string CQ3 = string.Empty;
                string CQ4 = string.Empty;
                string CQ5 = string.Empty;
                string DQ1 = string.Empty;
                string DQ2 = string.Empty;
                string DQ3 = string.Empty;
                string DQ4 = string.Empty;
                string EQ1 = string.Empty;
                string GQ1 = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/training-feedback.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("[UserName]", objTSM.FullName);
                body = body.Replace("[Designation]", objTSM.Designation);
                body = body.Replace("[Branch]", objTSM.Branch);
                body = body.Replace("[Title]", objTSM.AccessorName);
                body = body.Replace("[Venue]", objTSM.BranchName);
                string adate = objTSM.TrainingStartDate;
                body = body.Replace("[StartDate]", objTSM.TrainingStartDate);
                body = body.Replace("[EndDate]", objTSM.TrainingEndDate);
                body = body.Replace("[Summary]", objTSM.F1);
                if (objTSM.C1 == "Yes")
                {
                    CQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type ='checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Yes</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>No</span>&nbsp;<span><label class='checkbox-style'><input type ='checkbox'><span class='checkmark'></span></label></span></td>";
                }
                else
                {
                    CQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type ='checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Yes</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>No</span>&nbsp;<span><label class='checkbox-style'><input type ='checkbox' checked><span class='checkmark'></span></label></span></td>";
                }
                if (objTSM.C2 == "Too Short")
                {
                    CQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Too short</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Appropriate</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Too long</span></td>";
                }
                else if (objTSM.C2 == "Appropriate")
                {
                    CQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Too short</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Appropriate</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Too long</span></td>";
                }
                else
                {
                    CQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Too short</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Appropriate</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Too long</span></td>";
                }
                if (objTSM.C3 == "Fully")
                {
                    CQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Fully</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Partially</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                else if (objTSM.C3 == "Partially")
                {
                    CQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Fully</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Partially</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                else
                {
                    CQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Fully</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Partially</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                if (objTSM.C4 == "Relevant")
                {
                    CQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not relevant</span></td>";
                }
                else if (objTSM.C4 == "Somewhat Relevant")
                {
                    CQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not relevant</span></td>";
                }
                else
                {
                    CQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Not relevant</span></td>";
                }
                if (objTSM.C5 == "Significantly")
                {
                    CQ5 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Significantly</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Noticeably</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                else if (objTSM.C5 == "Noticeably")
                {
                    CQ5 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Significantly</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Noticeably</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                else if (objTSM.C5 == "Somewhat")
                {
                    CQ5 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Significantly</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Noticeably</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                else
                {
                    CQ5 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Significantly</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Noticeably</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                if (objTSM.D1 == "Excellent")
                {
                    DQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D1 == "Good")
                {
                    DQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D1 == "Satisfactory")
                {
                    DQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    DQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                if (objTSM.D2 == "Excellent")
                {
                    DQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D2 == "Good")
                {
                    DQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D2 == "Satisfactory")
                {
                    DQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    DQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                if (objTSM.D3 == "Excellent")
                {
                    DQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D3 == "Good")
                {
                    DQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D3 == "Satisfactory")
                {
                    DQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    DQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                if (objTSM.D4 == "Excellent")
                {
                    DQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D4 == "Good")
                {
                    DQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D4 == "Satisfactory")
                {
                    DQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    DQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                if (objTSM.E1 == "Excellent")
                {
                    EQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.E1 == "Good")
                {
                    EQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.E1 == "Satisfactory")
                {
                    EQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    EQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                if (objTSM.G1 == "Excellent")
                {
                    GQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.G1 == "Good")
                {
                    GQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.G1 == "Satisfactory")
                {
                    GQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    GQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                body = body.Replace("[C1]", CQ1);
                body = body.Replace("[C2]", CQ2);
                body = body.Replace("[C3]", CQ3);
                body = body.Replace("[C4]", CQ4);
                body = body.Replace("[C5]", CQ5);
                body = body.Replace("[D1]", DQ1);
                body = body.Replace("[D2]", DQ2);
                body = body.Replace("[D3]", DQ3);
                body = body.Replace("[D4]", DQ4);
                body = body.Replace("[E1]", EQ1);
                body = body.Replace("[G1]", GQ1);
                strs.Append(body);
                PdfPageSize pageSize = PdfPageSize.A4;
                PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.MaxPageLoadTime = 120;
                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;
                PdfDocument doc = converter.ConvertHtmlString(body);
                string sTime = Convert.ToString(DateTime.Now.Hour) + "" + Convert.ToString(DateTime.Now.Minute) + "" + Convert.ToString(DateTime.Now.Second);
                string ReportName = Convert.ToString(DateTime.Now.Year) + "" + Convert.ToString(DateTime.Now.Month) + "" + Convert.ToString(DateTime.Now.Day) + "" + sTime;
                string FileName = Convert.ToString(objTSM.tfid) + "-" + ReportName + ".pdf";
                string path = Server.MapPath("~/TrainingFeedback");
                doc.Save(path + '\\' + FileName);
                doc.Close();
                int a = objDTS.AddFeedbackFile(Convert.ToInt32(TF.PK_TrainingScheduleId), Convert.ToString(TF.tfid), Convert.ToString("~/TrainingFeedback" + FileName));
                #endregion
                #endregion
            }
            else
            {
                #region Update Training Feedback
                DataSet GetUserData = new DataSet();
                GetUserData = objDTS.GetPDFData(TF);
                if (GetUserData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in GetUserData.Tables[0].Rows)
                    {
                        objTSM.FullName = Convert.ToString(GetUserData.Tables[0].Rows[0]["FullName"]);
                        objTSM.Designation = Convert.ToString(GetUserData.Tables[0].Rows[0]["Designation"]);
                        objTSM.Branch = Convert.ToString(GetUserData.Tables[0].Rows[0]["Branch_Name"]);
                    }
                }
                if (GetUserData.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in GetUserData.Tables[1].Rows)
                    {
                        objTSM.AccessorName = Convert.ToString(GetUserData.Tables[1].Rows[0]["TopicName"]);
                        objTSM.BranchName = Convert.ToString(GetUserData.Tables[1].Rows[0]["BranchName"]);
                        objTSM.TrainingStartDate = Convert.ToString(GetUserData.Tables[1].Rows[0]["TrainingStartDate"]);
                        objTSM.TrainingEndDate = Convert.ToString(GetUserData.Tables[1].Rows[0]["TrainingEndDate"]);
                    }
                }
                if (GetUserData.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in GetUserData.Tables[2].Rows)
                    {
                        objTSM.C1 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Aim"]);
                        objTSM.C2 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Duration"]);
                        objTSM.C3 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Expectation"]);
                        objTSM.C4 = Convert.ToString(GetUserData.Tables[2].Rows[0]["PContent"]);
                        objTSM.C5 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Result"]);
                        objTSM.D1 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Command"]);
                        objTSM.D2 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Presentation"]);
                        objTSM.D3 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Question"]);
                        objTSM.D4 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Gesture"]);
                        objTSM.E1 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Arrangement"]);
                        objTSM.F1 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Summary"]);
                        objTSM.G1 = Convert.ToString(GetUserData.Tables[2].Rows[0]["Rating"]);
                    }
                }

                #region Generate PDF
                System.Text.StringBuilder strs = new System.Text.StringBuilder();
                string body = string.Empty;
                string CQ1 = string.Empty;
                string CQ2 = string.Empty;
                string CQ3 = string.Empty;
                string CQ4 = string.Empty;
                string CQ5 = string.Empty;
                string DQ1 = string.Empty;
                string DQ2 = string.Empty;
                string DQ3 = string.Empty;
                string DQ4 = string.Empty;
                string EQ1 = string.Empty;
                string GQ1 = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/training-feedback.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("[UserName]", objTSM.FullName);
                body = body.Replace("[Designation]", objTSM.Designation);
                body = body.Replace("[Branch]", objTSM.Branch);
                body = body.Replace("[Title]", objTSM.AccessorName);
                body = body.Replace("[Venue]", objTSM.BranchName);
                string adate = objTSM.TrainingStartDate;
                body = body.Replace("[StartDate]", objTSM.TrainingStartDate);
                body = body.Replace("[EndDate]", objTSM.TrainingEndDate);
                body = body.Replace("[Summary]", objTSM.F1);
                if (objTSM.C1 == "Yes")
                {
                    CQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type ='checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Yes</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>No</span>&nbsp;<span><label class='checkbox-style'><input type ='checkbox'><span class='checkmark'></span></label></span></td>";
                }
                else
                {
                    CQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type ='checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Yes</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>No</span>&nbsp;<span><label class='checkbox-style'><input type ='checkbox' checked><span class='checkmark'></span></label></span></td>";
                }
                if (objTSM.C2 == "Too Short")
                {
                    CQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Too short</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Appropriate</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Too long</span></td>";
                }
                else if (objTSM.C2 == "Appropriate")
                {
                    CQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Too short</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Appropriate</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Too long</span></td>";
                }
                else
                {
                    CQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Too short</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Appropriate</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Too long</span></td>";
                }
                if (objTSM.C3 == "Fully")
                {
                    CQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Fully</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Partially</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                else if (objTSM.C3 == "Partially")
                {
                    CQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Fully</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Partially</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                else
                {
                    CQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Fully</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Partially</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                if (objTSM.C4 == "Relevant")
                {
                    CQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not relevant</span></td>";
                }
                else if (objTSM.C4 == "Somewhat Relevant")
                {
                    CQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not relevant</span></td>";
                }
                else
                {
                    CQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat relevant</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Not relevant</span></td>";
                }
                if (objTSM.C5 == "Significantly")
                {
                    CQ5 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Significantly</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Noticeably</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                else if (objTSM.C5 == "Noticeably")
                {
                    CQ5 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Significantly</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Noticeably</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                else if (objTSM.C5 == "Somewhat")
                {
                    CQ5 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Significantly</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Noticeably</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                else
                {
                    CQ5 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Significantly</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Noticeably</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Somewhat</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Not at all</span></td>";
                }
                if (objTSM.D1 == "Excellent")
                {
                    DQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D1 == "Good")
                {
                    DQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D1 == "Satisfactory")
                {
                    DQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    DQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                if (objTSM.D2 == "Excellent")
                {
                    DQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D2 == "Good")
                {
                    DQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D2 == "Satisfactory")
                {
                    DQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    DQ2 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                if (objTSM.D3 == "Excellent")
                {
                    DQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D3 == "Good")
                {
                    DQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D3 == "Satisfactory")
                {
                    DQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    DQ3 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                if (objTSM.D4 == "Excellent")
                {
                    DQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D4 == "Good")
                {
                    DQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.D4 == "Satisfactory")
                {
                    DQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    DQ4 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                if (objTSM.E1 == "Excellent")
                {
                    EQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.E1 == "Good")
                {
                    EQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.E1 == "Satisfactory")
                {
                    EQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    EQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                if (objTSM.G1 == "Excellent")
                {
                    GQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.G1 == "Good")
                {
                    GQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else if (objTSM.G1 == "Satisfactory")
                {
                    GQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                else
                {
                    GQ1 = "<td width='100%' style='border-top-width:0; border-right-width:0; border-left-width:0; border-bottom-width:0; padding-bottom:15px;'><span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Excellent</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Good</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox'><span class='checkmark'></span></label></span>&nbsp;<span>Satisfactory</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><label class='checkbox-style'><input type = 'checkbox' checked><span class='checkmark'></span></label></span>&nbsp;<span>Poor</span></td>";
                }
                body = body.Replace("[C1]", CQ1);
                body = body.Replace("[C2]", CQ2);
                body = body.Replace("[C3]", CQ3);
                body = body.Replace("[C4]", CQ4);
                body = body.Replace("[C5]", CQ5);
                body = body.Replace("[D1]", DQ1);
                body = body.Replace("[D2]", DQ2);
                body = body.Replace("[D3]", DQ3);
                body = body.Replace("[D4]", DQ4);
                body = body.Replace("[E1]", EQ1);
                body = body.Replace("[G1]", GQ1);
                strs.Append(body);
                PdfPageSize pageSize = PdfPageSize.A4;
                PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.MaxPageLoadTime = 120;
                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;
                PdfDocument doc = converter.ConvertHtmlString(body);
                string sTime = Convert.ToString(DateTime.Now.Hour) + "" + Convert.ToString(DateTime.Now.Minute) + "" + Convert.ToString(DateTime.Now.Second);
                string ReportName = Convert.ToString(DateTime.Now.Year) + "" + Convert.ToString(DateTime.Now.Month) + "" + Convert.ToString(DateTime.Now.Day) + "" + sTime;
                string FileName = ReportName + ".pdf";
                string path = Server.MapPath("~/TrainingFeedback");
                string SavedPath = path + '\\' + FileName;
                doc.Save(SavedPath);
                doc.Close();
                TF.FeedbackFile = SavedPath;
                i = objDTS.AddTrainingFeedback(TF);
                #endregion
                #endregion
            }
            return RedirectToAction("TrainingDetails", "TrainingSchedule");
        }
        [HttpPost]
        public JsonResult GetProposedDate(string tname)
        {
            DataTable DTGetDate = new DataTable();
            DTGetDate = objDTS.GetProposedDate(Convert.ToString(tname));
            if (DTGetDate.Rows.Count > 0)
            {
                objTSM.ProposedDate = Convert.ToString(DTGetDate.Rows[0]["ProposedDate"]);
            }
            else
            {
                objTSM.ProposedDate = DateTime.Now.ToString("dd/mm/yyyy");
            }
            return Json(objTSM, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllEmpList()
        {
            DataTable DTGetEmpList = new DataTable();
            List<string> lstEmployees = new List<string>();
            DTGetEmpList = objDTS.GetEmpList();
            if (DTGetEmpList.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetEmpList.Rows)
                {
                    lstEmployees.Add(Convert.ToString(dr["TraineeName"]));
                }
                objTSM.TraineeName = string.Join(",", lstEmployees);
            }

            return Json(objTSM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllBranchEmpList()
        {
            DataTable DTGetEmpList = new DataTable();
            List<string> lstEmployees = new List<string>();
            DTGetEmpList = objDTS.GetBranchEmpList();
            if (DTGetEmpList.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetEmpList.Rows)
                {
                    lstEmployees.Add(Convert.ToString(dr["TraineeName"]));
                }
                objTSM.TraineeName = string.Join(",", lstEmployees);
            }

            return Json(objTSM, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            FormCollection fc = new FormCollection();

            //Adding New Code 13 March 2020
            string strExtension = string.Empty;


            List<FileDetails> DOCUploaded = new List<FileDetails>();
            List<FileDetails> FILEUPLOAD1 = new List<FileDetails>();

            if (Session["DocsUploaded"] != null)
            {
                DOCUploaded = Session["DocsUploaded"] as List<FileDetails>;
            }
            if (Session["FILEUPLOAD1"] != null)
            {
                FILEUPLOAD1 = Session["FILEUPLOAD1"] as List<FileDetails>;
            }

            try
            {
                


                string filePath = string.Empty;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file
                    int fileSize = files.ContentLength;
                    strExtension = Path.GetExtension(files.FileName);



                    if (files != null && files.ContentLength > 0)
                    {

                        string fileName = files.FileName;
                        FileDetails fileDetail = new FileDetails();
                        fileDetail.FileName = fileName;
                        fileDetail.Extension = Path.GetExtension(fileName);
                        fileDetail.Id = Guid.NewGuid();


                        if (Request.Files.Keys[0].ToString().ToUpper() == "DOCUPLOAD1")
                        {
                            FILEUPLOAD1.Add(fileDetail);
                        }
                        BinaryReader br = new BinaryReader(files.InputStream);
                        byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                        fileDetail.FileContent = bytes;


                        if (files.FileName.ToUpper().EndsWith(".MP4") || files.FileName.ToUpper().EndsWith(".WMV") || files.FileName.ToUpper().EndsWith(".PDF"))
                        {
                           
                            
                            //FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = Path.GetFileNameWithoutExtension(fileName);
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();

                            //BinaryReader br = new BinaryReader(files.InputStream);
                            //byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                            //fileDetail.FileContent = bytes;


                            if (Request.Files.Keys[0].ToString().ToUpper() == "DOCUPLOAD")
                            {
                                DOCUploaded.Add(fileDetail);
                            }
                           

                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/Content/Uploads/Videos/"), fileName);
                            var K = "~/Content/Uploads/Videos/" + fileName;
                            IPath = K;

                            if (files.FileName.ToUpper().EndsWith(".MP4") || files.FileName.ToUpper().EndsWith(".WMV") || files.FileName.ToUpper().EndsWith(".PDF"))
                            {
                                files.SaveAs(filePath);
                            }

                            var ExistingUploadFile = IPath;
                            splitedGrp = ExistingUploadFile.Split(',');
                            foreach (var single in splitedGrp)
                            {
                                Selected.Add(single);
                            }
                            Session["list"] = Selected;
                            //Session["DocsUploaded"] = Selected;
                        }
                        else
                        {
                            ViewBag.Error = "Please Select MP4 , PDF or WMV File";
                        }
                    }
                }

                
                if (Request.Files.Keys[0].ToString().ToUpper() == "DOCUPLOAD")
                {
                    Session["DocsUploaded"] = DOCUploaded;
                }
                if (Request.Files.Keys[0].ToString().ToUpper() == "DOCUPLOAD1")
                {
                    Session["FILEUPLOAD1"] = FILEUPLOAD1;
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }


        
        public ActionResult Export(string PK_TrainingScheduleId)
        {
            var products = new System.Data.DataTable("teste");
            
            //DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
            DataSet dtCount = new DataSet("Grid");
            dtCount = objDTS.Excel(PK_TrainingScheduleId);
            //dtCount = objDAL.ExportToExcelWinner();


            var grid = new GridView();
            grid.DataSource = dtCount;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Export.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            //TrainingAttendance(int PK_TrainingScheduleId);
            return RedirectToAction("TrainingAttendance", "TrainingSchedule", new { PK_TrainingScheduleId = PK_TrainingScheduleId });
            //return View();
        }

        
        [HttpGet]
        public ActionResult FeedBackList(int? tid)
        {

            CustomerFeedback ObjModel = new CustomerFeedback();
            ObjModel.FeedBack_ID = Convert.ToInt32(tid);
            var model = new TrainingScheduleModel();
            List<CustomerFeedback> TraineeList = new List<CustomerFeedback>();
            
            DataSet dss = new DataSet();

            DataTable Validate = new DataTable();
            Validate = objDTS.GetFeedBackList(Convert.ToInt32(tid));
            if (Validate.Rows.Count > 0)
            {
                foreach (DataRow dr in Validate.Rows)
                {
                    TraineeList.Add(
                        new CustomerFeedback
                        {
                            
                            RTraineeName = Convert.ToString(dr["Name"]),
                            Branch = Convert.ToString(dr["Branch"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            FeedBack = Convert.ToString(dr["Rating"]),
                            
                        });
                }

                //foreach (DataRow dr in JobDashBoard.Rows)
                //{
                //    lstCompanyDashBoard.Add(
                //        new JobMasters
                //        {
                //            PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                //        }
                // }

                            ViewBag.Trainee = TraineeList;
                
            }
            ObjModel.lstFeedback1 = TraineeList;
            return View(ObjModel);

        }


        public ActionResult ExportFeedBackList(string PK_TrainingScheduleId)
        {
            var products = new System.Data.DataTable("teste");

            //DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
            DataSet dtCount = new DataSet("Grid");
            dtCount = objDTS.ExportFeedBackList(PK_TrainingScheduleId);
            //dtCount = objDAL.ExportToExcelWinner();


            var grid = new GridView();
            grid.DataSource = dtCount;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename=ExportFeedBackList.xls");
            Response.AddHeader("content-disposition", "attachment; filename=ExportFeedBackList.xls");
            
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

        //[HttpPost]
        //public JsonResult ChkAttendence()
        //{
        //    objTSM.CreatedBy = Convert.ToString(System.Web.HttpContext.Current.Session["LoginID"]);
        //    int result;
        //    try
        //    {
        //        DataSet dsCheck = objDalLogout.CheckValidUser();

        //        string pwd1 = dsCheck.Tables[0].Rows[0]["UserPassword"].ToString();

        //        string pwd = Decrypt(pwd1);

        //        if (pwd != OldPassword)
        //        {
        //            return Json(new { result = "Redirect", url = Url.Action("ChangedPassword", "Logout") });
        //        }
        //        if (login.NewPassword != login.ConfirmPassword)
        //        {
        //            return Json(new { result = "RedirectPage", url = Url.Action("ChangedPassword", "Logout") });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    return Json("Failed", JsonRequestBehavior.AllowGet);
        //}


        public FileResult Download1(string d)
        {

            string FileName = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDTS.GetFileContent(Convert.ToInt32(d));

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


        #region Export To Excel New
        public ActionResult ExportIndex(string PK_TrainingScheduleId)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 15;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<TrainingScheduleModel> grid = CreateExportableGrid1(PK_TrainingScheduleId);



                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                
               


                int colcount = 0;
                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[row, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;
                    column.IsEncoded = false;
                    colcount++;
                }

               
                sheet.Cells[15, 1, 15, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;

                sheet.Cells[15, 1, 15, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);


                double finalAmount = 0;
                row++;
                foreach (IGridRow<TrainingScheduleModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                    {
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
                    }
                    finalAmount = finalAmount + Convert.ToDouble(sheet.Cells[row, colcount].Value.ToString());   // Convert.ToDouble(grid.Rows[row]["INRamount"]);

                    row++;
                }
                row++;
                sheet.Cells[row, 1, row, colcount].Style.Border.Top.Style = ExcelBorderStyle.Thick;

                sheet.Cells[row++, colcount].Value = finalAmount;


                #region 2nd Table

                int rowI = row + 1;
                string strRow = "D" + rowI;

                sheet.Cells[rowI, 2].Value = "COST ASSIGNMENT.";

                row = row + 2;
                col = 1;
                IGrid<TrainingScheduleModel> grid1 = CreateExportableGrid1(PK_TrainingScheduleId);
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
                foreach (IGridRow<TrainingScheduleModel> gridRow1 in grid1.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid1.Columns)
                    {
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow1);
                    }
                    finalAmount1 = finalAmount1 + Convert.ToDouble(sheet.Cells[row, colcount1].Value.ToString());   // Convert.ToDouble(grid.Rows[row]["INRamount"]);

                    row++;
                }
                row++;
                sheet.Cells[row, 1, row, colcount1].Style.Border.Top.Style = ExcelBorderStyle.Thick;

                sheet.Cells[row++, colcount1].Value = finalAmount1;
                #endregion






                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<TrainingScheduleModel> CreateExportableGrid1(string PK_TrainingScheduleId)
        {
            
            IGrid<TrainingScheduleModel> grid1 = new Grid<TrainingScheduleModel>(GetData1(PK_TrainingScheduleId));
            grid1.ViewContext = new ViewContext { HttpContext = HttpContext };

            //grid.Columns.Add(model => model.VoucherId).Titled("Voucher Generated");
            //grid1.Columns.Add(model => model.CostCenter).Titled("CostCenter");


            grid1.Pager = new GridPager<TrainingScheduleModel>(grid1);
            grid1.Processors.Add(grid1.Pager);
            grid1.Pager.RowsPerPage = objTSM.lstExportQuizeResult.Count;

            foreach (IGridColumn column in grid1.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }


            return grid1;
        }

        public List<TrainingScheduleModel> GetData1(string PK_TrainingScheduleId)
        {

            List<TrainingScheduleModel> lmd1 = new List<TrainingScheduleModel>(); // creating list of model.  
            DataSet ds = new DataSet();

            Session["GetExcelData"] = "Yes";
            //SP_Quiz

            DataSet dtCount = new DataSet("Grid");

         //   dtCount = objDTS.ExportQuizeResultExcel(PK_TrainingScheduleId);

            
            if (dtCount.Tables[0].Rows.Count>0)
            {
                foreach (DataRow dr in dtCount.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd1.Add(new TrainingScheduleModel
                    {

                        //CostCenter = Convert.ToString(dr["Cost_Center"]),



                    });
                }
            }

            
            objTSM.lstExportQuizeResult = lmd1;



            return objTSM.lstExportQuizeResult;
            //return  (lmd, lmd1);
            //return objEIModel;
        }


        #endregion

        public ActionResult ExportIndex1(string PK_TrainingScheduleId)
        {

            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 1;
                Int32 col = 1;
                Int32 col2 = 1;
                //int col2 = 4;
                int Qrow = 2;
                int Qrow1 = 3;
                package.Workbook.Worksheets.Add("Data");
                //DataSet dtCount = new DataSet("Grid");
                //dtCount = objDTS.ExportQuizeResultExcel(PK_TrainingScheduleId);

                DataTable dt = objDTS.ExportQuizeResultExcel(PK_TrainingScheduleId);
                DataTable dt1 = objDTS.ExportQuizeQ(PK_TrainingScheduleId);
                DataRow[] dr = null;
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];
                //sheet.Cells[row, col].Value = dt.Rows[0]["LessonId"].ToString();
                int NewQrow;
              //  row = row + 2;

                for (int col1 = 0; col1 < dt.Columns.Count; col1++)
                {

                    sheet.Cells[row, col].Value = dt.Columns[col1].ColumnName.ToString();

                    if(col1 >=6)
                    {
                        dr = dt1.Select("Id=" + dt.Columns[col1].ColumnName.ToString().Replace("Q", ""));
                        sheet.Cells[Qrow, col].Value = dr[0]["Question"].ToString();                        
                        sheet.Cells[Qrow1, col].Value = dr[0]["Answer"].ToString();

                    }                   
                    col++;
                    
                }
                row++;

                #region Print question

                //dr = null;

                //for (int col1 = 0; col1 < dt.Columns.Count; col1++)
                //{

                //    //sheet.Cells[row, col].Value = dt.Columns[col1].ColumnName.ToString();

                //    if (col2 >= 4)
                //    {

                //        //dr = dt1.Select("Id=" + dt.Columns[col1].ColumnName.ToString().Replace("Q", ""));
                //        //dr = dt1.Select("Id=" + dt.Columns[col2].ColumnName.ToString());
                //        sheet.Cells[Qrow1, col2].Value = dt.Columns["Answer"].ToString();
                //    }
                //    col2++;

                //}
                //row++;
                #endregion


                row = row+2;

                for (int row1 = 0; row1 < dt.Rows.Count; row1++)
                {
                    col = 1;

                    for (int col3 = 0; col3 < dt.Columns.Count; col3++)
                    {
                        sheet.Cells[row, col].Value = dt.Rows[row1][col3].ToString();
                        col++;
                    }

                    row++;
                }
                

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }



        }


        public ActionResult ExportListOfTrainingSchedule()
        {
            var products = new System.Data.DataTable("teste");

            //DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
            DataSet dtCount = new DataSet("Grid");
            dtCount = objDTS.ExportFeedBackList1();
            //dtCount = objDAL.ExportToExcelWinner();


            var grid = new GridView();
            grid.DataSource = dtCount;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename=ExportFeedBackList.xls");
            Response.AddHeader("content-disposition", "attachment; filename=ExportFeedBackList.xls");

            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            //TrainingAttendance(int PK_TrainingScheduleId);
            return RedirectToAction("ListScheduleTraining", "TrainingSchedule");
            //return View();
        }



        [HttpGet]
        public ActionResult TrainingRecord()
        {

            TrainingScheduleModel ObjModel = new TrainingScheduleModel();
            var model = new TrainingScheduleModel();
            List<TrainingScheduleModel> TraineeList = new List<TrainingScheduleModel>();

            DataTable dss = new DataTable();

            DataTable Validate = new DataTable();
            //Validate = objDTS.dtTrainingRecord();
            //if (Validate.Rows.Count > 0)
            //{

            //    foreach (DataRow dr in Validate.Rows)
            //    {

            //        TraineeList.Add(new TrainingScheduleModel
            //        {
            //            //Name = Convert.ToString(dr["Name"]),
            //            //MobileNo = Convert.ToString(dr["MobileNo"]),
            //            TrainingName = Convert.ToString(dr["TrainingName"]),

            //        }
            //        );
            //    }
            //}
            //ViewData["Header"] = TraineeList;
            //ObjModel.lstTrainingRecord = TraineeList;


            DataTable Validate1 = new DataTable();
            Validate1 = objDTS.dtTrainingRecordData();
            ViewData["Footer"] = Validate1;
            model.dtActivityMaster = Validate1;
            return View(model);

        }

        
        [HttpPost]
        public ActionResult TrainingRecord(TrainingScheduleModel obj)
        {

            DataTable Validate1 = new DataTable();
            Validate1 = objDTS.dtTrainingRecordDataByDate(obj);
            ViewData["Footer"] = Validate1;
            obj.dtActivityMaster = Validate1;
            return View(obj);

        }


        public ActionResult TrainingRecordExport()
        {
            var products = new System.Data.DataTable("teste");

            //DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
            DataSet dtCount = new DataSet("Grid");
            dtCount = objDTS.TrainingRecordExport();
            //dtCount = objDAL.ExportToExcelWinner();


            var grid = new GridView();
            grid.DataSource = dtCount;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename=ExportFeedBackList.xls");
            Response.AddHeader("content-disposition", "attachment; filename=ExportFeedBackList.xls");

            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            //TrainingAttendance(int PK_TrainingScheduleId);
            return RedirectToAction("ListScheduleTraining", "TrainingSchedule");
            //return View();
        }

        #region Training Record By Training Id
        [HttpGet]
        public ActionResult TrainingRecordById(int? tid)
        {
            var model = new TrainingRecordById();
            List<TrainingRecordById> TraineeList = new List<TrainingRecordById>();
            DataSet dss = new DataSet();
            model.TrainingScheduleId = Convert.ToString(tid);
            DataTable Validate = new DataTable();
            Validate = objDTS.GetTrainingRecordById(Convert.ToInt32(tid));
            if (Validate.Rows.Count > 0)
            {
                foreach (DataRow dr in Validate.Rows)
                {
                    TraineeList.Add(
                        new TrainingRecordById
                        {
                            TraineeName = Convert.ToString(dr["TraineeName"]),
                            Branch = Convert.ToString(dr["Branch"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            IsPresent = Convert.ToString(dr["IsPresent"]),
                            QuizAttended = Convert.ToString(dr["QuizAttended"]),
                            Score = Convert.ToString(dr["Score"]),
                            Result = Convert.ToString(dr["Result"]),
                            FeedBack = Convert.ToString(dr["FeedBack"]),
                            Attempt = Convert.ToString(dr["Attempt"]),
                            AttendedDate = Convert.ToString(dr["AttendedDate"]),
                        });
                }
                ViewBag.Trainee = TraineeList;

            }
            return View(model);
        }



        [HttpGet]
        public ActionResult ExportTrainingRecord(string PK_TrainingScheduleId)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<TrainingRecordById> grid = CreateExportableGrid(PK_TrainingScheduleId);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<TrainingRecordById> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<TrainingRecordById> CreateExportableGrid(string PK_TrainingScheduleId)
        {
            IGrid<TrainingRecordById> grid = new Grid<TrainingRecordById>(GetData(PK_TrainingScheduleId));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.TraineeName).Titled("TraineeName");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.Email).Titled("Email");
            grid.Columns.Add(model => model.MobileNo).Titled("MobileNo");
            grid.Columns.Add(model => model.IsPresent).Titled("IsPresent");
            grid.Columns.Add(model => model.QuizAttended).Titled("QuizAttended");
            grid.Columns.Add(model => model.Score).Titled("Score");
            grid.Columns.Add(model => model.Result).Titled("Result");
            grid.Columns.Add(model => model.FeedBack).Titled("FeedBack");
            grid.Columns.Add(model => model.Attempt).Titled("Attempt");



            grid.Pager = new GridPager<TrainingRecordById>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = model.lstEnquiryMast.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<TrainingRecordById> GetData(string PK_TrainingScheduleId)
        {

            Session["UserLoginID"] = User.Identity.IsAuthenticated;
            string UserRole = Convert.ToString(Session["role"]);

            List<TrainingRecordById> lstEnquiryMast = new List<TrainingRecordById>();
            lstEnquiryMast = objDTS.TrainingRecordExportToExcel(PK_TrainingScheduleId);
            model.lstEnquiryMast = lstEnquiryMast;
            return model.lstEnquiryMast;
        }



        #endregion


    }
}