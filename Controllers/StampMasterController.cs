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
    public class StampMasterController : Controller
    {
        // GET: StampMaster
        DALStampMaster objSM = new DALStampMaster();

        [HttpGet]
        public ActionResult CreateStampMaster(int? id)
        {
            var model = new StampMaster();
            DataSet dss = new DataSet();
            if (id != null)
            {
                dss = objSM.GetDataById(Convert.ToInt32(id));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    model.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                    model.Attachment = dss.Tables[0].Rows[0]["Attachment"].ToString();
                    model.ImageName = dss.Tables[0].Rows[0]["ImageName"].ToString();
                    model.Type = dss.Tables[0].Rows[0]["Type"].ToString();
                    model.Quantity = dss.Tables[0].Rows[0]["Quantity"].ToString();
                    //    model.JoiningDate = Convert.ToDateTime(dss.Tables[0].Rows[0]["JoiningDate"].ToString());

                }
                return View(model);
            }
            else
            {
                return View();
            }



        }



        [HttpPost]
        public ActionResult CreateStampMaster(StampMaster S, HttpPostedFileBase File)
        {
            string Result = string.Empty;
            try
            {
                HttpPostedFileBase Imagesection;
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                {
                    Imagesection = Request.Files["img_Banner"];
                    if (Imagesection != null && Imagesection.FileName != "")
                    {
                        S.Attachment = CommonControl.FileUpload("StampMasterDocument/", Imagesection);
                    }
                    else
                    {
                        if (Imagesection.FileName != "")
                        {
                            S.Attachment = "NoImage.gif";
                        }
                    }
                }

                if (S.Id > 0)
                {
                     Result = objSM.Insert(S);
                }
                else
                {
                    //#region File Upload Code 
                    Result = objSM.Insert(S);
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
            return RedirectToAction("ListStampMaster", "StampMaster");
        }
    

        [HttpGet]
        public ActionResult ListStampMaster()
        {
            List<StampMaster> lmd = new List<StampMaster>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objSM.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new StampMaster
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Attachment = Convert.ToString(dr["Attachment"]),
                    ImageName = Convert.ToString(dr["ImageName"]),
                    Quantity = Convert.ToString(dr["Quantity"]),
                    Type = Convert.ToString(dr["Type"]),
                    ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                    ModifiedDate = Convert.ToString(dr["ModifiedDate"]),

                });
            }
            return View(lmd.ToList());

        }






        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objSM.Delete(Convert.ToInt32(Id));
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
            return RedirectToAction("ListStampMaster");


        }


    }
}