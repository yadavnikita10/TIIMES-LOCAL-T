using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class StampRegisterController : Controller
    {
        DALStampRegister objSR = new DALStampRegister();
        StampRegister objs = new StampRegister();
        string[] r;
        string[] ids;//used in insert

        // GET: StampRegister
        public ActionResult CreateStampRegister(int? id)
        {
            var model = new StampRegister();
            DataSet dss = new DataSet();
            DataTable dsGetStamp = new DataTable();

            #region Get stamp 

            //Get Stamp (Image)
            dsGetStamp = objSR.GetImage();
            List<StampImageList> lstStampImageList = new List<StampImageList>();

            if (dsGetStamp.Rows.Count > 0)
            {

                foreach (DataRow dr in dsGetStamp.Rows)
                {

                    lstStampImageList.Add(new StampImageList
                    {
                        ImageId = Convert.ToInt32(dr["Id"]),
                        attachment = Convert.ToString(dr["attachment"]),
                        ImageName = Convert.ToString(dr["ImageName"]),
                        Quantity = Convert.ToString(dr["Quantity"]),
                        Type = Convert.ToString(dr["Type"]),
                    }
                    );
                }
            }
            ViewData["Image"] = lstStampImageList;
            #endregion

            if (id != null)
            {
                dss = objSR.GetDataById(Convert.ToInt32(id));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    model.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                    model.SurveyorName = dss.Tables[0].Rows[0]["SurveyorName"].ToString();
                    model.Location = dss.Tables[0].Rows[0]["Location"].ToString();
                    model.Remarks = dss.Tables[0].Rows[0]["Remarks"].ToString();
                    model.AdditionalStamp = Convert.ToString(dss.Tables[0].Rows[0]["AdditionalStamp"]);
                    model.HiddenAdditionalStamp = Convert.ToString(dss.Tables[0].Rows[0]["AdditionalStamp"]);
                    model.StampNumber = dss.Tables[0].Rows[0]["StampNumber"].ToString();

                    string j = dss.Tables[0].Rows[0]["JoiningDate"].ToString();




                    //model.JoiningDate = Convert.ToDateTime(j, CultureInfo.CurrentCulture).ToString();

                    //model.JoiningDate = Convert.ToDateTime(2019, CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    model.JoiningDate = Convert.ToDateTime(dss.Tables[0].Rows[0]["JoiningDate"].ToString());

                    #region session for if Data insert Without Selecting Stamp
                    if (string.IsNullOrEmpty(model.AdditionalStamp))
                    {
                        Session["ISStampPresent"] = "No";
                    }
                    else
                    {
                        Session["ISStampPresent"] = "Yes";
                    }
                    #endregion

                }

                #region Get Check box id for update
                string Result1 = string.Empty;
                string To = model.AdditionalStamp.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                List<StampImageList> lstStampImageListUpdate = new List<StampImageList>();
                //foreach (string MultiEmailTemp in EmailIDs)
                //{
                //    lstStampImageListUpdate.Add(new StampImageListUpdate
                //    {
                //        ImageUpdate = MultiEmailTemp,

                //    }
                //   );

                //}
                ViewData["ImageUpdate"] = EmailIDs;




                #endregion


                return View(model);
            }
            else
            {
                return View();
            }



        }



        [HttpPost]
        public ActionResult CreateStampRegister(StampRegister S, FormCollection fc, int[] ID_)
        {

            string Result = string.Empty;

            try
            {

                //string test = Convert.ToString(ID_);

                #region getImage Id 
                if (ID_ != null)
                {
                    ids = fc["ID_"].Split(new char[] { ',' }); //fc["ID"].Split(new char[] { ',' });

                    //Remove null entries
                    ids = ids.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    string sAdditionalStamp = String.Join(",", ids.Where(s => !string.IsNullOrEmpty(s)));
                    string a = Convert.ToString(sAdditionalStamp);
                    S.AdditionalStamp = a;
                }
                #endregion

                #region getImage Id  change
                //string[] ids = fc["ID"].Split(new char[] { ',' });

                ////Remove null entries
                //ids = ids.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                //string sAdditionalStamp = String.Join(",", ids.Where(s => !string.IsNullOrEmpty(s)));
                //string a = Convert.ToString(sAdditionalStamp);
                //S.AdditionalStamp = a;
                #endregion


                #region get count total no of Image
                int RubberStampcount = 0;
                int HardStampcount = 0;
                if (ids != null)
                {
                    foreach (var v in ids)
                    {

                        int id = Convert.ToInt16(v);
                        DataTable dtGetCount = new DataTable();
                        dtGetCount = objSR.GetCount(id);
                        if (dtGetCount.Rows.Count > 0)
                        {
                            string Stamp = dtGetCount.Rows[0]["Type"].ToString();
                            if (Stamp == "RubberStamps")
                            {
                                RubberStampcount = RubberStampcount + 1;
                                S.TotalRubberStamps = Convert.ToString(RubberStampcount);
                            }
                            else
                            {
                                HardStampcount = HardStampcount + 1;
                                S.TotalHardStamps = Convert.ToString(HardStampcount);
                            }
                        }
                    }
                }
                #endregion



                if (S.Id > 0)
                {
                    Result = objSR.Update(S);

                    if (Convert.ToInt32(Result) >= 0)
                    {

                        if(Session["ISStampPresent"] == "Yes")
                        {
                            #region Add Quantity
                            string Result2 = string.Empty;
                            string To2 = S.HiddenAdditionalStamp.ToString();
                            char[] delimiters1 = new[] { ',', ';', ' ' };
                            string[] EmailIDs1 = To2.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string MultiEmailTemp in EmailIDs1)
                            {
                                int id = Convert.ToInt32(MultiEmailTemp);

                                Result2 = objSR.AddQuantity(id);
                            }
                        }
                        


                        #endregion


                        #region Minus Quantity


                        string Result1 = string.Empty;
                        string To = S.AdditionalStamp.ToString();
                        char[] delimiters = new[] { ',', ';', ' ' };
                        string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string MultiEmailTemp in EmailIDs)
                        {
                            int id = Convert.ToInt32(MultiEmailTemp);

                            Result1 = objSR.GetQuantity(id);
                        }
                        #endregion
                    }
                }
                else
                {




                    #region chk stamp is already assign
                    DataSet ds = new DataSet();
                    ds = objSR.ChkSurveyorRecordIfExist(S);
                    if(ds.Tables[0].Rows.Count>0)
                  //  if(ds.Tables.Count>0)
                    {
                        ModelState.Clear();
                        TempData["Chkmessage"] = "Stamp already given to Surveyor";
                        return RedirectToAction("CreateStampRegister");

                    }
                    else
                    {
                        Result = objSR.Insert(S);
                        if (Convert.ToInt16(Result) > 0)
                        {
                            #region Minus Quantity

                            string Result1 = string.Empty;
                            string To = S.AdditionalStamp.ToString();
                            char[] delimiters = new[] { ',', ';', ' ' };
                            string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string MultiEmailTemp in EmailIDs)
                            {
                                int id = Convert.ToInt32(MultiEmailTemp);
                                // msg.To.Add(new MailAddress(MultiEmailTemp));
                                Result1 = objSR.GetQuantity(id);
                            }



                            #endregion

                            ModelState.Clear();
                            TempData["message"] = "Record Added Successfully...";
                        }
                        else
                        {
                            TempData["message"] = "Something went Wrong! Please try Again";
                        }
                    }

                    #endregion



                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
              return RedirectToAction("ListStampRegister", "StampRegister");
            
        }

        int id;
        [HttpGet]
        public ActionResult ListStampRegister()
        {
            #region Get stamp 
            DataTable dsGetStamp = new DataTable();

            //Get Stamp (Image)
            dsGetStamp = objSR.GetImage();
            List<StampImageList> lstStampImageList = new List<StampImageList>();

            if (dsGetStamp.Rows.Count > 0)
            {

                foreach (DataRow dr in dsGetStamp.Rows)
                {

                    lstStampImageList.Add(new StampImageList
                    {
                        ImageId = Convert.ToInt32(dr["Id"]),
                        attachment = Convert.ToString(dr["attachment"]),
                        ImageName = Convert.ToString(dr["ImageName"]),
                        Quantity = Convert.ToString(dr["Quantity"]),
                        Type = Convert.ToString(dr["Type"]),
                    }
                    );
                }
            }
            ViewData["Image"] = lstStampImageList;
            #endregion

            List<StampRegister> lmd = new List<StampRegister>();  // creating list of model.  

            DataSet ds = new DataSet();

            ds = objSR.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new StampRegister
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    SurveyorName = Convert.ToString(dr["SurveyorName"]),
                    Location = Convert.ToString(dr["Location"]),
                    Remarks=Convert.ToString(dr["Remarks"]),
                    StampNumber = Convert.ToString(dr["StampNumber"]),
                    JoiningDate = Convert.ToDateTime(dr["JoiningDate"]),
                    AdditionalStamp = Convert.ToString(dr["AdditionalStamp"]),
                    HiddenAdditionalStamp = Convert.ToString(dr["AdditionalStamp"]),
                    TotalHardStamps = Convert.ToString(dr["TotalHardStamps"]),
                    TotalRubberStamps = Convert.ToString(dr["TotalRubberStamps"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                    ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]),
                    ModifiedBy=Convert.ToString(dr["FirstName"])
                });
            }
            List<LAdditionalStamp> lst = new List<LAdditionalStamp>();
            #region Additional stamp
            string Result1 = string.Empty;

            if (ds.Tables[0].Rows.Count > 0)
            {
                string To = Convert.ToString(ds.Tables[0].Rows[0]["AdditionalStamp"]);

                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    id = Convert.ToInt32(MultiEmailTemp);
                    lst.Add(new LAdditionalStamp
                    {
                        lstAdditionalStamp = Convert.ToInt32(MultiEmailTemp),

                    });

                }
                ViewData["AdditionalStamp"] = lst;
            }
            #endregion




            return View(lmd.ToList());

        }



        public JsonResult GetSurveyorName(string prefix)

        {
            DataSet dsTopic = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            dsTopic = objSR.GetSurveyorName(prefix);
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


        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objSR.Delete(Convert.ToInt32(Id));
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
            return RedirectToAction("ListStampRegister");


        }

        string sqlFormattedDate;
        string Location;
        string JoiningDate1;
        public JsonResult GetLocation(string Category)

        {
            DataSet dsTopic = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            dsTopic = objSR.GetDateOfJoining(Category);
            List<StampRegister> searchlist = new List<StampRegister>();

            if (dsTopic.Tables[0].Rows.Count > 0)
            {

                DateTime? myDate = Convert.ToDateTime(dsTopic.Tables[0].Rows[0]["DateOfJoining"]);
                //DateTime? myDate = Convert.ToDateTime(dr["DateOfJoining"]);
                string sqlFormattedDate = myDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

                //searchlist.Add(new StampRegister
                //{
                //JoiningDate1 = sqlFormattedDate;
                // JoiningDate1 = sqlFormattedDate;

                Location = dsTopic.Tables[0].Rows[0]["BranchName"].ToString();
                //});


            }

            return Json(Location, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetDateOfJoining(string Category)

        {
            DataSet dsTopic = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            dsTopic = objSR.GetDateOfJoining(Category);
            List<StampRegister> searchlist = new List<StampRegister>();

            if (dsTopic.Tables[0].Rows.Count > 0)
            {

                DateTime? myDate = Convert.ToDateTime(dsTopic.Tables[0].Rows[0]["DateOfJoining"]);
                //DateTime? myDate = Convert.ToDateTime(dr["DateOfJoining"]);
                string sqlFormattedDate = myDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

                //searchlist.Add(new StampRegister
                //{
                //JoiningDate1 = sqlFormattedDate;
                JoiningDate1 = sqlFormattedDate;

                //  Location = dsTopic.Tables[0].Rows[0]["BranchName"].ToString();
                //});


            }

            return Json(JoiningDate1, JsonRequestBehavior.AllowGet);


        }


        public JsonResult GetDataByName(string IRNReport)

        {
            var objModel = new StampRegister();
            DataSet dsTopic = new DataSet();

            dsTopic = objSR.GetDateOfJoining(IRNReport);
            List<StampRegister> searchlist = new List<StampRegister>();

          
            if (dsTopic.Tables[0].Rows.Count > 0)
            {

               

                objModel.Location = dsTopic.Tables[0].Rows[0]["BranchName"].ToString();

                DateTime? myDate = Convert.ToDateTime(dsTopic.Tables[0].Rows[0]["DateOfJoining"]);
                string sqlFormattedDate = myDate.Value.ToString("yyyy-MM-dd");
                

                objModel.JoiningDate1 = sqlFormattedDate;
               

            }
            else
            {
                
                TempData["message"] = "Stamp already Assigned";
                //return RedirectToAction("CreateStampRegister");
                ModelState.Clear();
               // return Json(new { Url = Url.Action("CreateStampRegister") });


            }

            return Json(objModel, JsonRequestBehavior.AllowGet);


        }

    }
}