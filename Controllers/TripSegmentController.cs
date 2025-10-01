using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class TripSegmentController : Controller
    {
        DALTripSegment objTS = new DALTripSegment();
        TripSegment objModel = new TripSegment();
        // GET: TripSegment
        public ActionResult CreateTripSegment(int? FKId, string Type, int? PKExpenseId, string SubJobNo, int? PK_Call_Id)
        {
            objModel.FKId = Convert.ToInt32(FKId);
            objModel.Type = Type;
            objModel.SubJobNo = SubJobNo;
            objModel.PK_Call_Id = Convert.ToInt32(PK_Call_Id);

            TempData["FKID"] = Convert.ToInt32(FKId);
            TempData["Type"] = Type;
            TempData["SubJobNo"] = SubJobNo;
            TempData["PK_Call_Id"] = PK_Call_Id;
            TempData.Keep();

            #region get TripSegment
            List<TripSegment> lmd = new List<TripSegment>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objTS.GetTripSegment(objModel); // fill dataset  
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new TripSegment
                    {
                        Type = Convert.ToString(dr["Type"]),
                        FKId = Convert.ToInt32(dr["FKId"]),
                        ExpenseType = Convert.ToString(dr["ExpenseType"]),
                        Date = Convert.ToString(dr["Date"]),
                        TotalAmount = Convert.ToInt32(dr["TotalAmount"]),
                        PKExpenseId = Convert.ToInt32(dr["PKExpenseId"]),
                        StartCity  = Convert.ToString(dr["City"]),
                        EndCity = Convert.ToString(dr["EndCity"]),
                        SubJobNo = Convert.ToString(dr["SubJobNo"]),
                    });
                }
                ViewBag.TripSegment = lmd;
            }
            else
            {

            }
            #endregion

            if (PKExpenseId > 0)
            {
                DataSet dsGetDataById = new DataSet();

                dsGetDataById = objTS.GetDataById(Convert.ToInt32(PKExpenseId));
                if (dsGetDataById.Tables.Count > 0)
                {

                    objModel.StartCity = dsGetDataById.Tables[0].Rows[0]["City"].ToString();
                    objModel.EndCity = dsGetDataById.Tables[0].Rows[0]["EndCity"].ToString();
                    objModel.ExpenseType = dsGetDataById.Tables[0].Rows[0]["ExpenseType"].ToString();
                    objModel.Date = Convert.ToString(dsGetDataById.Tables[0].Rows[0]["Date"]);
                    objModel.TotalAmount = Convert.ToInt16(dsGetDataById.Tables[0].Rows[0]["TotalAmount"]);
                    objModel.Description = dsGetDataById.Tables[0].Rows[0]["Description"].ToString();
                    objModel.EndCity = dsGetDataById.Tables[0].Rows[0]["EndCity"].ToString();
                    objModel.Kilometer = Convert.ToInt32(dsGetDataById.Tables[0].Rows[0]["Kilometer"]);
                    objModel.SubJobNo = dsGetDataById.Tables[0].Rows[0]["SubJobNo"].ToString();
                }
                return View(objModel);
            }
            else
            {
                return View(objModel);
            }



            
        }


        [HttpPost]
        public ActionResult CreateTripSegment(TripSegment T)
        {
            string Result = string.Empty;
            double CarRate = 0;
            double BikeRate = 0;
            try
            {
                #region Get Employee Grade
                DataSet ds = new DataSet();

                ds = objTS.GetEmployeeGrade();
                if(ds.Tables.Count>0)
                {
                    CarRate = Convert.ToDouble(ds.Tables[0].Rows[0]["CarRate"]);
                    BikeRate = Convert.ToDouble(ds.Tables[0].Rows[0]["MotorBikeRate"]);

                }
                if(T.ExpenseType=="Car")
                {
                    T.TotalAmount = CarRate * T.Kilometer;
                    
                }
                else
                {
                    T.TotalAmount = BikeRate * T.Kilometer;
                }
                

                #endregion


                if (T.PKExpenseId > 0)
                {
                    //Update
                    Result = objTS.Insert(T);
                }
                else
                {

                    Result = objTS.Insert(T);
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
            //return RedirectToAction("ListIAFScopeMaster", "IAFScopeMaster");

            return RedirectToAction("CreateTripSegment", new RouteValueDictionary(
   new { controller = "TripSegment", action = "CreateTripSegment", FKId = TempData["FKID"], Type = TempData["Type"], SubJobNo = TempData["SubJobNo"], PK_Call_Id = TempData["PK_Call_Id"] }));

        }

        public ActionResult Delete(int? PKExpenseId)
        {
            string Result = string.Empty;
            try
            {
                Result = objTS.Delete(Convert.ToInt32(PKExpenseId));
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

            return RedirectToAction("CreateExpenseItem", new RouteValueDictionary(
      new { controller = "ExpenseItem", action = "CreateExpenseItem", FKId = TempData["FKID"], Type = TempData["Type"] }));


        }

    }
}