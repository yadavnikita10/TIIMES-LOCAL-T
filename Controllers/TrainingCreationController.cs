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
    public class TrainingCreationController : Controller
    {
        // GET: TrainingCreation
        DALTrainingCreation objDTC = new DALTrainingCreation();
        public ActionResult CreateTraining()
        {
            DataSet dsBindBranch = new DataSet();
            TrainingCreationModel tc = new TrainingCreationModel();
            List<BranchName> lstBranch = new List<BranchName>();
            dsBindBranch = objDTC.BindBranch();

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
            tc.BranchId = Convert.ToInt32(Session["UserBranchId"]);
            //return View();
            return View(tc);
        }

        [HttpPost]
        public ActionResult CreateTraining(TrainingCreationModel TCM)
        {
            string Result = string.Empty;
            try
            {

                Result = objDTC.Insert(TCM);
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
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("ListTrainingCreation", "TrainingCreation");
        }

        [HttpGet]
        public ActionResult ListTrainingCreation()
        {
            List<TrainingCreationModel> lmd = new List<TrainingCreationModel>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objDTC.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new TrainingCreationModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    TrainingTopic = Convert.ToString(dr["TrainingTopic"]),
                    EvaluationMethod = Convert.ToString(dr["EvaluationMethod"]),
                    BranchId = Convert.ToInt16(dr["BranchId"]),
                    BranchName = Convert.ToString(dr["BranchName"]),
                    Category = Convert.ToString(dr["Category"]),
                    Other = Convert.ToString(dr["Other"]),
                    TrainType = Convert.ToString(dr["TrainType"]),
                    Remarks = Convert.ToString(dr["Remarks"]),
                    ProposedDate = Convert.ToString(dr["ProposedDate"]),


                });
            }
            return View(lmd.ToList());

        }

        //Get Record By id for Update
        [HttpGet]
        public ActionResult UpdateTraining(int? id)
        {
            var model = new TrainingCreationModel();
            DataSet dsBindBranch = new DataSet();
            List<BranchName> lstBranch = new List<BranchName>();
            dsBindBranch = objDTC.BindBranch();

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



            DataSet dss = new DataSet();

            dss = objDTC.GetDataById(Convert.ToInt32(id));
            if (dss.Tables[0].Rows.Count > 0)
            {
               
                model.Id                 = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                model.TrainingTopic = dss.Tables[0].Rows[0]["TrainingTopic"].ToString();
                model.EvaluationMethod = dss.Tables[0].Rows[0]["EvaluationMethod"].ToString();
                model.BranchId       = Convert.ToInt32(dss.Tables[0].Rows[0]["BranchId"]);
                //model.BranchName   =  dss.Tables[0].Rows[0]["BranchName"].ToString();
                model.Category = dss.Tables[0].Rows[0]["Category"].ToString();
                model.Other   =  dss.Tables[0].Rows[0]["Other"].ToString();
                model.TrainType = dss.Tables[0].Rows[0]["TrainType"].ToString();
                model.Remarks  =  dss.Tables[0].Rows[0]["Remarks"].ToString();
                string date = Convert.ToString(dss.Tables[0].Rows[0]["ProposedDate"]);
                if (date == "")
                {
                    model.ProposedDate = Convert.ToString(DateTime.Now);
                }
                else
                {
                    model.ProposedDate = Convert.ToString(dss.Tables[0].Rows[0]["ProposedDate"]);
                }
            }
            return View(model);
        }



        //Update Record & return to list
        [HttpPost]
        public ActionResult UpdateTraining(TrainingCreationModel N, int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objDTC.Update(N, Convert.ToInt32(Id));
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
            return RedirectToAction("ListTrainingCreation", "TrainingCreation");


        }

        
        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objDTC.Delete(Convert.ToInt32(Id));
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
            return RedirectToAction("ListTrainingCreation");


        }


        //Auto Complete Training Topic
        //[HttpPost]
        //public JsonResult Index(string Prefix)
        //{
        //    DataSet dsTopic = new DataSet();
        //    //Note: you can bind same list from database
        //    // List<TrainingTopic> objTopicList = new List<TrainingTopic>();
        //    List<TrainingCreationModel> objTopicList;// = new List<TrainingCreationModel>();

        //    dsTopic = objDTC.GetTopicList();
        //    //Searching records from list using LINQ query  

        //        if (dsTopic.Tables[0].Rows.Count > 0)

        //            objTopicList = (from N in objTopicList
        //                            where N.lTopicName.StartsWith(Prefix)
        //                            select new { N.lTopicName });

        //        return Json(objTopicList, JsonRequestBehavior.AllowGet);

        //    }

        //[HttpPost]
        public JsonResult GetRecord(string prefix)

        {
            DataSet dsTopic = new DataSet();
           // DataSet ds = dblayer.GetName(prefix);
         dsTopic = objDTC.GetTopicList(prefix);
            List<TrainingCreationModel> searchlist = new List<TrainingCreationModel>();

            foreach (DataRow dr in dsTopic.Tables[0].Rows)

            {

                searchlist.Add(new TrainingCreationModel
                {
                    TrainingTopic = dr["TrainingTopic"].ToString(),
                });

            }
            return Json(searchlist, JsonRequestBehavior.AllowGet);

        }





    }
}