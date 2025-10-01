using Newtonsoft.Json;
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
    public class OtherServicesController : Controller
    {
        DALNonInspectionActivity objNIA = new DALNonInspectionActivity();
        DALUsers objDalCreateUser = new DALUsers(); 
        NonInspectionActivity objModel = new NonInspectionActivity();
       
        [HttpGet]
        public ActionResult OtherServices(NonInspectionActivity n, int? id, String AddExpense1)
        {
            string id_ = id.ToString();

            var Data2 = objDalCreateUser.GetActivityList();
            ViewBag.Activity = new SelectList(Data2, "Code", "Name");

            DataTable CostCenterName = objNIA.GetServiceName();
            if (CostCenterName.Rows.Count > 0)
            {
                objModel.ServiceName = CostCenterName.Rows[0]["ServiceName"].ToString();
                objModel.ServiceCode = CostCenterName.Rows[0]["Pk_CC_Id"].ToString();
            }
            else
            {
                objModel.ServiceName = string.Empty;
            }

            var Data1 = objDalCreateUser.GetCostCenterList();
            if (id_ == "")
            {
                ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center", objModel.ServiceCode);
            }
            else
            {
                ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center", objModel.CostCenter);
            }

            DataSet CostCenter = objNIA.GetServiceCode();
            objModel.ServiceCode = CostCenter.Tables[0].Rows[0]["CostCenter"].ToString();

            TempData["FromD"] = n.FromD;
            TempData["ToD"] = n.ToD;
            TempData.Keep();
            if (AddExpense1 == "AddExpense")
            {
                objModel.AddExpense = "Yes";
            }
            else
            {
                objModel.AddExpense = null;
            }

            #region Bind List
            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
            DataSet ds = new DataSet();


            if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
            {
                //  ds = objNIA.GetDataByDate(n); // fill dataset
                ds = objNIA.GetNonInspectionDataByDate(n);

                n.FromD = Convert.ToString(TempData["FromD"]);
                n.ToD = Convert.ToString(TempData["ToD"]);
                objModel.FromD = Convert.ToString(TempData["FromD"]);
                objModel.ToD = Convert.ToString(TempData["ToD"]);
            }
            else
            {
                // ds = objNIA.GetData(); // fill dataset 
                ds = objNIA.GetNoninspectionData();
            }



            //added by nikita on 20052024
            var allowedActivityTypes = new List<string> { "42", "44", "45", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "37" };



            if (ds.Tables.Count > 0)
            {

                #region Bind data By Id
                DataSet dss = new DataSet();
                dss = objNIA.GetDataById(Convert.ToInt32(id));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    objModel.ActivityType = dss.Tables[0].Rows[0]["ActivityType"].ToString();
                    if (allowedActivityTypes.Contains(objModel.ActivityType))
                    {
                        objModel.ServiceCode = null;
                        objModel.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);

                        objModel.ActivityTypeName = dss.Tables[0].Rows[0]["ActivityTypeName"].ToString();
                        objModel.Description = dss.Tables[0].Rows[0]["Description"].ToString();
                        objModel.StartDate = Convert.ToString(dss.Tables[0].Rows[0]["DateSE"]);
                        objModel.Location = dss.Tables[0].Rows[0]["Location"].ToString();
                        objModel.ServiceCode = dss.Tables[0].Rows[0]["ServiceCode"].ToString();
                        objModel.StartTime = Convert.ToDouble(dss.Tables[0].Rows[0]["StartTime"].ToString());
                        objModel.EndTime = Convert.ToDouble(dss.Tables[0].Rows[0]["EndTime"].ToString());
                        objModel.ODTime = Convert.ToDouble(dss.Tables[0].Rows[0]["ODTime"].ToString());
                        objModel.TravelTime = Convert.ToDouble(dss.Tables[0].Rows[0]["TravelTime"].ToString());
                        objModel.JobNumber = dss.Tables[0].Rows[0]["JobNumber"].ToString();
                        objModel.Attachment = dss.Tables[0].Rows[0]["Attachment"].ToString();
                        objModel.WFHCheckbox = Convert.ToBoolean(dss.Tables[0].Rows[0]["WFHCheckbox"]);
                        objModel.CallNumber = dss.Tables[0].Rows[0]["Call_No"].ToString();
                        objModel.Rating = dss.Tables[0].Rows[0]["Ratings"].ToString();
                        objModel.NewExistingCustomer = dss.Tables[0].Rows[0]["New_ExistingCustomer"].ToString();
                        objModel.DomesticInternationVisit = dss.Tables[0].Rows[0]["Dom_Inter_Visit"].ToString();
                        objModel.CostCenter = dss.Tables[0].Rows[0]["CostCenter_id"].ToString();  //added by nikita on 30042024
                    }
                }

                #endregion
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    var activityType = Convert.ToString(dr["ActivityType"]);
                    if (allowedActivityTypes.Contains(activityType))
                    {
                        lmd.Add(new NonInspectionActivity
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            ActivityType = Convert.ToString(dr["ActivityType"]),
                            ActivityTypeName = Convert.ToString(dr["ActivityTypeName"]),
                            Location = Convert.ToString(dr["Location"]),
                            DateSE = Convert.ToString(dr["DateSE"]),
                            ServiceCode = Convert.ToString(dr["ServiceCode"]),
                            Description = Convert.ToString(dr["Description"]),
                            StartTime = Convert.ToDouble(dr["StartTime"]),
                            EndTime = Convert.ToDouble(dr["EndTime"]),
                            TravelTime = Convert.ToDouble(dr["TravelTime"]),
                            JobNumber = Convert.ToString(dr["JobNumber"]),
                            Attachment = Convert.ToString(dr["Attachment"]),
                            SAP_No = Convert.ToString(dr["SAP_No"]),
                            ServiceName = Convert.ToString(dr["ServiceName"]),
                            NewExistingCustomer = Convert.ToString(dr["CustType"]),
                            DomesticInternationVisit = Convert.ToString(dr["VisitType"]),
                            CostCenter = Convert.ToString(dr["CostCenter_id"]), //added by nikita on 30042024
                            issendforapprovalExpense = Convert.ToString(dr["IsSendForApproval_expense"]), //added by nikita on 30042024
                            issendforapprovalope = Convert.ToString(dr["IsSendForApproval_ope"]), //added by nikita on 30042024

                        });
                    }
                }
                ViewBag.CostSheet = lmd;
                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;

                return View(objModel);
            }
            else
            {

                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;
                return View(objModel);
            }
            #endregion




            return View(objModel);
        }



        
        [HttpPost]
        public ActionResult Create(NonInspectionActivity R, HttpPostedFileBase File, HttpPostedFileBase[] Image,int? Id)
        {
            string Result = string.Empty;
            try
            {
                if (Image.Count() > 0)
                {
                    foreach (HttpPostedFileBase item in Image)
                    {
                        HttpPostedFileBase image = item;
                        if (image != null && image.ContentLength > 0)
                        {
                            string filePath = AppDomain.CurrentDomain.BaseDirectory + "NonInspectionActivityDocument\\" + image.FileName;
                            const string ImageDirectoryFP = "NonInspectionActivityDocument\\";
                            const string ImageDirectory = "~/NonInspectionActivityDocument/";
                            string ImagePath = "~/NonInspectionActivityDocument/" + image.FileName;
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
                            R.Attachment += ImageName + ",";
                        }
                    }
                }
                #region Insert Update Single Page

                if(Id>0)
                {
                    #region Update
                    double total = Convert.ToDouble(R.StartTime) + Convert.ToDouble(R.ODTime);
                    int _currtotal = Convert.ToInt32(R.StartTime) + Convert.ToInt32(R.EndTime) + Convert.ToInt32(R.ODTime);
                    R.TotalTime = total;
                    R.DateSE = R.StartDate;
                    DataSet DTValidateTT = new DataSet();
                    DTValidateTT = objNIA.ValidateTT(R.DateSE);
                    if (DTValidateTT.Tables[0].Rows.Count > 0)
                    {
                        int _prevtotal = Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["StartTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["EndTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["ODTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["TravelTime"]);

                        int _addition = _currtotal + _prevtotal;
                        if (_addition <= 24)
                        {
                            //Result = objNIA.Update(R, Convert.ToInt32(Id));
                            Result = objNIA.Insert(R, Convert.ToInt32(Id));
                            if (Convert.ToInt16(Result) > 0)
                            {
                                ModelState.Clear();
                                TempData["Success"] = "Record Added Successfully.";
                            }
                            else
                            {
                                TempData["Failure"] = "Something went Wrong! Please try Again";
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Exceeded limit of 24 hours for the day  " + R.DateSE.ToString();
                            return RedirectToAction("NonInspectionActivity", "NonInspectionActivity");
                        }
                    }
                    #endregion

                }
                else
                {
                    #region Insert
                    #region Added by Ankush (Iterate between Dates)
                    double total = Convert.ToDouble(R.EndTime) + Convert.ToDouble(R.ODTime) + Convert.ToDouble(R.TravelTime);
                    int _currtotal = Convert.ToInt32(R.StartTime) + Convert.ToInt32(R.EndTime) + Convert.ToInt32(R.ODTime) + Convert.ToInt32(R.TravelTime);
                    bool blnLeaveApplied = false;
                    bool blnHoursExceeded = false;
                    bool FinalblnLeaveApplied = false;
                    bool FinalblnHoursExceeded = false;
                    string strDateLeave = string.Empty;
                    string strDateHoursExceed = string.Empty;
                    R.TotalTime = total;
                    DataSet DTValidateTT = new DataSet();

                    DateTime Start = DateTime.ParseExact(R.StartDate, "dd/MM/yyyy", null);
                    DateTime End = DateTime.ParseExact(R.EndDate, "dd/MM/yyyy", null);

                    for (DateTime date = Start; date.Date <= End; date = date.AddDays(1))
                    {
                        blnLeaveApplied = false;

                        DateTime StDt = date;
                        R.DateSE = StDt.ToString("dd/MM/yyyy");


                        DTValidateTT = objNIA.ValidateTT(StDt.ToString("dd/MM/yyyy"));

                        if (DTValidateTT.Tables[1].Rows.Count > 0) //// Leave Checking
                        {
                            if (Convert.ToInt32(DTValidateTT.Tables[1].Rows[0][0].ToString()) > 0)
                            {
                                if (strDateLeave == string.Empty)
                                    strDateLeave = StDt.ToString("dd/MM/yyyy");
                                else
                                    strDateLeave = strDateLeave + " , " + StDt.ToString("dd/MM/yyyy");
                                blnLeaveApplied = true;
                                FinalblnLeaveApplied = true;

                            }

                        }

                        if (blnLeaveApplied != true)
                        {
                            if (DTValidateTT.Tables[0].Rows.Count > 0)
                            {

                                int _prevtotal = Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["StartTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["EndTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["ODTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["TravelTime"]);

                                int _addition = _currtotal + _prevtotal;



                                if (_addition < 24)
                                {
                                    if (Convert.ToInt32(Id) > 0)
                                    {
                                        //Result = objNIA.Update(R, Convert.ToInt32(Id));
                                        Result = objNIA.Insert(R, Convert.ToInt32(Id));
                                    }
                                    else
                                    {
                                        Result = objNIA.Insert(R, Convert.ToInt32(Id));
                                    }


                                    if (Convert.ToInt16(Result) > 0)
                                    {
                                        ModelState.Clear();
                                        TempData["Success"] = "Record Added Successfully.";
                                    }
                                    else
                                    {
                                        TempData["Failure"] = "Something went Wrong! Please try Again.";
                                    }
                                }
                                else
                                {

                                    if (strDateHoursExceed == string.Empty)
                                        strDateHoursExceed = StDt.ToString("dd/MM/yyyy");
                                    else
                                        strDateHoursExceed = strDateHoursExceed + " , " + StDt.ToString("dd/MM/yyyy");

                                    FinalblnHoursExceeded = true;

                                }
                            }
                        }
                    }

                    if (FinalblnLeaveApplied)
                    {
                        TempData["Error"] = "Leave has been added for " + strDateLeave;
                    }

                    if (FinalblnHoursExceeded)
                    {
                        if (TempData["Error"] != null)
                            //TempData["Error"] = TempData["Error"]  + " <br/>" + "Exceeded limit of 24 hours for the day  " + strDateHoursExceed;
                            TempData["Error"] = TempData["Error"] + " " + "Exceeded limit of 24 hours for the day  " + strDateHoursExceed;
                        else
                            TempData["Error"] = "Exceeded limit of 24 hours for the day " + strDateHoursExceed;
                    }

                    #endregion
                    #endregion
                }





                #endregion



            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("OtherServices", "OtherServices");
        }

        [HttpGet]
        public ActionResult ListNonInspectionActivity()
        {
            Session["GetExcelData"] = "Yes";

            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
            DataSet ds = new DataSet();
            FormCollection fc = new FormCollection();

            ///if ((fc["FromD"] != null && fc["FromD"] != string.Empty) && (fc["ToD"] != null && fc["ToD"] != string.Empty))
            if (TempData["FromD"] != null && TempData["ToD"] != null)
            {
                NonInspectionActivity n = new NonInspectionActivity();
             
                n.FromD = Convert.ToString(TempData["FromD"]);
                n.ToD = Convert.ToString(TempData["ToD"]);
                objModel.FromD = Convert.ToString(TempData["FromD"]);
                objModel.ToD = Convert.ToString(TempData["ToD"]);
                TempData.Keep();
                ds = objNIA.GetDataByDate(n);
            }
            else
            {
                ds = objNIA.GetData();
            }


            var allowedActivityTypes = new List<string> { "42", "44", "45", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "37" };


            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    var activityType = Convert.ToString(dr["ActivityType"]);
                    if (allowedActivityTypes.Contains(activityType))
                    {
                        lmd.Add(new NonInspectionActivity
                        {
                            Id = Convert.ToInt32(dr["Id"]),

                            Location = Convert.ToString(dr["Location"]),

                            DateSE = Convert.ToString(dr["DateSE"]),

                            ServiceCode = Convert.ToString(dr["ServiceCode"]),
                            Description = Convert.ToString(dr["Description"]),

                            StartTime = Convert.ToDouble(dr["StartTime"]),
                            EndTime = Convert.ToDouble(dr["EndTime"]),
                            TravelTime = Convert.ToDouble(dr["TravelTime"]),

                            JobNumber = Convert.ToString(dr["JobNumber"]),
                            Attachment = Convert.ToString(dr["Attachment"]),
                            SAP_No = Convert.ToString(dr["SAP_No"]),
                            ServiceName = Convert.ToString(dr["ServiceName"]),
                            CostCenter = Convert.ToString(dr["CostCenter_id"]), //added by nikita on 30042024

                        });
                    }
                }
                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;
                
                return View(objModel);
            }
            else
            {
                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;
                return View(objModel);
            }
            
        }

        [HttpPost]
        public ActionResult ListNonInspectionActivity(NonInspectionActivity n ,FormCollection fc)
        {
            Session["GetExcelData"] = null;


            TempData["FromD"] = n.FromD;
            TempData["ToD"] = n.ToD;
            TempData.Keep();

            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
            DataSet ds = new DataSet();
            var allowedActivityTypes = new List<string> { "42", "44", "45", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "37" };

            /// if((fc["FromD"] != null && fc["FromD"] != string.Empty) && (fc["ToD"] != null && fc["ToD"] != string.Empty) )

            if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
            {
               // ds = objNIA.GetDataByDate(n); // fill dataset
                ds = objNIA.GetCalenderDataByDate(n);

                n.FromD = Convert.ToString(TempData["FromD"]);
                n.ToD = Convert.ToString(TempData["ToD"]);
                objModel.FromD = Convert.ToString(TempData["FromD"]);
                objModel.ToD = Convert.ToString(TempData["ToD"]);
            }
            else
            {
                //ds = objNIA.GetData(); // fill dataset 

                ds = objNIA.GetCalenderData();

            }

            
            lmd.Clear();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    var activityType = Convert.ToString(dr["ActivityType"]);
                    if (allowedActivityTypes.Contains(activityType))
                    {
                        lmd.Add(new NonInspectionActivity
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            ActivityType = Convert.ToString(dr["ActivityType"]),
                            Location = Convert.ToString(dr["Location"]),
                            DateSE = Convert.ToString(dr["DateSE"]),
                            ServiceCode = Convert.ToString(dr["ServiceCode"]),
                            Description = Convert.ToString(dr["Description"]),
                            StartTime = Convert.ToDouble(dr["StartTime"]),
                            EndTime = Convert.ToDouble(dr["EndTime"]),
                            TravelTime = Convert.ToDouble(dr["TravelTime"]),
                            Attachment = Convert.ToString(dr["Attachment"]),

                        });
                    }
                }
            }
            ViewData["NonInspectionActivityList"] = lmd;
            objModel.NIADashBoard = lmd;
            //return View(lmd.ToList());
            return RedirectToAction("OtherServices", n);
            return View(objModel);

        }

        [HttpGet]
        public ActionResult UpdateNonInspectionActivity(int? id)
        {
            var model = new NonInspectionActivity();
            DataSet dss = new DataSet();

            var Data1 = objDalCreateUser.GetCostCenterList();
            ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");

            dss = objNIA.GetDataById(Convert.ToInt32(id));
            if (dss.Tables[0].Rows.Count > 0)
            {
                model.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                model.ActivityType = dss.Tables[0].Rows[0]["ActivityType"].ToString();
                model.Description = dss.Tables[0].Rows[0]["Description"].ToString();
                
                model.DateSE = Convert.ToString(dss.Tables[0].Rows[0]["DateSE"]);
                model.Location = dss.Tables[0].Rows[0]["Location"].ToString();
                model.ServiceCode = dss.Tables[0].Rows[0]["ServiceCode"].ToString();
                model.StartTime = Convert.ToDouble( dss.Tables[0].Rows[0]["StartTime"].ToString());
                model.EndTime = Convert.ToDouble( dss.Tables[0].Rows[0]["EndTime"].ToString());
                model.ODTime = Convert.ToDouble( dss.Tables[0].Rows[0]["ODTime"].ToString());
                model.TravelTime = Convert.ToDouble(dss.Tables[0].Rows[0]["TravelTime"].ToString());
                model.JobNumber = dss.Tables[0].Rows[0]["JobNumber"].ToString();
                model.Attachment = dss.Tables[0].Rows[0]["Attachment"].ToString();
                model.WFHCheckbox = Convert.ToBoolean(dss.Tables[0].Rows[0]["WFHCheckbox"]);
                model.CallNumber = dss.Tables[0].Rows[0]["Call_No"].ToString();
                model.Rating = dss.Tables[0].Rows[0]["Ratings"].ToString();

            }
            return View(model);
        }

       

        //After Update Return List
        [HttpPost]
        public ActionResult UpdateNonInspectionActivity(NonInspectionActivity N, int? Id, HttpPostedFileBase File, HttpPostedFileBase[] Image)
        {
            string Result = string.Empty;
            try
            {
                if (Image.Count() > 0)
                {
                    foreach (HttpPostedFileBase item in Image)
                    {
                        HttpPostedFileBase image = item;
                        if (image != null && image.ContentLength > 0)
                        {
                            string filePath = AppDomain.CurrentDomain.BaseDirectory + "NonInspectionActivityDocument\\" + image.FileName;
                            const string ImageDirectoryFP = "NonInspectionActivityDocument\\";
                            const string ImageDirectory = "~/NonInspectionActivityDocument/";
                            string ImagePath = "~/NonInspectionActivityDocument/" + image.FileName;
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


                // DateTime Start = DateTime.ParseExact(N.StartDate, "dd/MM/yyyy", null);
                //DateTime End = DateTime.ParseExact(N.EndDate, "dd/MM/yyyy", null);

                double total = Convert.ToDouble(N.StartTime)  + Convert.ToDouble(N.ODTime);
                int _currtotal = Convert.ToInt32(N.StartTime) + Convert.ToInt32(N.EndTime) + Convert.ToInt32(N.ODTime);
                N.TotalTime = total;

                DataSet DTValidateTT = new DataSet();
                DTValidateTT = objNIA.ValidateTT(N.DateSE);
                if (DTValidateTT.Tables[0].Rows.Count > 0)
                {
                    int _prevtotal = Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["StartTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["EndTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["ODTime"]) + Convert.ToInt32(DTValidateTT.Tables[0].Rows[0]["TravelTime"]);

                    int _addition = _currtotal + _prevtotal;
                    if (_addition <= 24)
                    {
                        Result = objNIA.Update(N, Convert.ToInt32(Id));
                        if (Convert.ToInt16(Result) > 0)
                        {
                            ModelState.Clear();
                            TempData["Success"] = "Record Added Successfully.";
                        }
                        else
                        {
                            TempData["Failure"] = "Something went Wrong! Please try Again";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Exceeded limit of 24 hours for the day  " + N.DateSE.ToString();
                        return RedirectToAction("OtherServices", "OtherServices");
                    }
                }

            /*    for (DateTime date = Start; date.Date <= End; date = date.AddDays(1))
                {
                    DateTime StDt = date;
                    N.DateSE = StDt.ToString("dd/MM/yyyy");

                    DataTable DTValidateTT = new DataTable();
                    DTValidateTT = objNIA.ValidateTT(StDt);
                    if (DTValidateTT.Rows.Count > 0)
                    {
                        int _prevtotal = Convert.ToInt32(DTValidateTT.Rows[0]["StartTime"]) + Convert.ToInt32(DTValidateTT.Rows[0]["EndTime"]) + Convert.ToInt32(DTValidateTT.Rows[0]["ODTime"]);
                        int _addition = _currtotal + _prevtotal;
                        if (_addition < 24)
                        {
                            Result = objNIA.Update(N, Convert.ToInt32(Id));
                            if (Convert.ToInt16(Result) > 0)
                            {
                                ModelState.Clear();
                                TempData["Success"] = "Record Added Successfully...";
                            }
                            else
                            {
                                TempData["Failure"] = "Something went Wrong! Please try Again";
                            }
                        }
                        else
                        {
                            TempData["Error"] = "You have excided 24 hrs for the day of " + StDt.ToString("dd/MM/yyyy");
                            return RedirectToAction("ListNonInspectionActivity", "NonInspectionActivity");
                        }
                    }                    
                }                */

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ModelState.Clear();
            return RedirectToAction("OtherServices", "OtherServices");            
        }
        
        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objNIA.Delete( Convert.ToInt32(Id));
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
            //   return RedirectToAction("ListNonInspectionActivity");
            return RedirectToAction("Otherservice", "Otherservice");


        }
        #region MISFeedBack Export to excel

        [HttpGet]
        public ActionResult ExportIndex(NonInspectionActivity c)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<NonInspectionActivity> grid = CreateExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<NonInspectionActivity> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<NonInspectionActivity> CreateExportableGrid(NonInspectionActivity c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<NonInspectionActivity> grid = new Grid<NonInspectionActivity>(GetData(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.DateSE).Titled("Date");
            grid.Columns.Add(model => model.ActivityType).Titled("Activity");
            grid.Columns.Add(model => model.JobNumber).Titled("TUVI Control No.");
            grid.Columns.Add(model => model.SAP_No).Titled("SAP Number");
            grid.Columns.Add(model => model.StartTime).Titled("Outdoor / On-Site Time (Hrs.)");
            grid.Columns.Add(model => model.EndTime).Titled("Office / Off-Site Time (Hrs.) ");
            
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time (Hrs.)");

            grid.Columns.Add(model => model.Location).Titled("City/Cities");
            grid.Columns.Add(model => model.Description).Titled("Description of Activity");
            grid.Columns.Add(model => model.ServiceCode).Titled("Service Code");
            grid.Columns.Add(model => model.NewExistingCustomer).Titled("Customer Type1");
            grid.Columns.Add(model => model.DomesticInternationVisit).Titled("Customer Type2");
           
           
            grid.Columns.Add(model => model.Id).Titled("ID");




            grid.Pager = new GridPager<NonInspectionActivity>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objModel.NIADashBoard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        /*
         * public JsonResult ValidateJob(string JobNo)//Checking Existing User Name
        {
            string Result = string.Empty;
            DataTable dtJobExist = new DataTable();
            try
            {
                dtJobExist = objNIA.ValidateJob(JobNo);
                if (dtJobExist.Rows.Count > 0)
                {
                    return Json(1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(0);
        }

    */
        public JsonResult ValidateJob(string JobNo, string ActType)//Checking Existing User Name
        {
            try
            {
                DataTable dtJobExist = new DataTable();
                bool blnValid = false;

                if (!string.IsNullOrEmpty(JobNo))
                {
                    // Check for specific prefixes in JobNo
                    if (ActType == "44" || ActType == "45")
                    {
                        if (JobNo.StartsWith("42") || JobNo.StartsWith("43") || JobNo.StartsWith("44") || JobNo.StartsWith("45"))
                        {
                            dtJobExist = objNIA.ValidateControlNo(JobNo.Trim());
                            blnValid = dtJobExist.Rows.Count > 0;
                        }
                    }
                    else if (ActType == "48" || ActType == "49")
                    {
                        if (JobNo.StartsWith("04") || JobNo.StartsWith("10"))
                        {
                            dtJobExist = objNIA.ValidateControlNo(JobNo.Trim());
                            blnValid = dtJobExist.Rows.Count > 0;
                        }
                    }
                    else if (ActType == "37")
                    {

                        dtJobExist = objNIA.ValidateJob(JobNo.Trim(), ActType);
                        blnValid = dtJobExist.Rows.Count > 0;
                        //if (JobNo.StartsWith("11"))
                        //{
                        //    dtJobExist = objNIA.ValidateJob(JobNo.Trim());
                        //    blnValid = dtJobExist.Rows.Count > 0;
                        //}
                    }
                    else if (ActType == "50" || ActType == "51")
                    {
                        if (JobNo.StartsWith("06"))
                        {
                            dtJobExist = objNIA.ValidateControlNo(JobNo.Trim());
                            blnValid = dtJobExist.Rows.Count > 0;
                        }
                    }
                    else if (ActType == "52" || ActType == "53")
                    {
                        if (JobNo.StartsWith("07"))
                        {
                            dtJobExist = objNIA.ValidateControlNo(JobNo.Trim());
                            blnValid = dtJobExist.Rows.Count > 0;
                        }
                    }
                    else if (ActType == "54" || ActType == "55")
                    {
                        if (JobNo.StartsWith("08"))
                        {
                            dtJobExist = objNIA.ValidateControlNo(JobNo.Trim());
                            blnValid = dtJobExist.Rows.Count > 0;
                        }
                    }
                    else if (ActType == "56" || ActType == "57")
                    {
                        if (JobNo.StartsWith("03"))
                        {
                            dtJobExist = objNIA.ValidateControlNo(JobNo.Trim());
                            blnValid = dtJobExist.Rows.Count > 0;
                        }
                    }
                }
                else
                {
                    dtJobExist = objNIA.ValidateJob(JobNo.Trim(), ActType);
                    blnValid = dtJobExist.Rows.Count > 0;
                }

                if (blnValid)
                    return Json(1);
                else
                    return Json(0);
            }
            catch (Exception ex)
            {
                // Log the error
                string error = ex.Message;
                return Json(0); // Return error status
            }
        }
        public List<NonInspectionActivity> GetData(NonInspectionActivity c)
        {
            DataSet DTFeedback = new DataSet();

            FormCollection fc = new FormCollection();


            if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
            {
                objModel.FromD = Convert.ToString(TempData["FromD"]);
                objModel.ToD = Convert.ToString(TempData["ToD"]);
                c.FromD = Convert.ToString(TempData["FromD"]);
                c.ToD = Convert.ToString(TempData["ToD"]);
                //// DTFeedback = objNIA.GetDataByDate(c);
                DTFeedback = objNIA.GetNonInspectionDataByDate(c);
            }
            else
            {
                ////DTFeedback = objNIA.GetData();
                DTFeedback = objNIA.GetNoninspectionData();
            }




            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
            //DataSet ds = new DataSet();

            //ds = objNIA.GetData(); // fill dataset  

            if (DTFeedback.Tables.Count > 0)
            {
                foreach (DataRow dr in DTFeedback.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new NonInspectionActivity
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        Location = Convert.ToString(dr["Location"]),
                       // StartDate = Convert.ToString(dr["startdate"]),
                        //EndDate = Convert.ToString(dr["enddate"]),
                        DateSE = Convert.ToString(dr["DateSE"]),
                        JobNumber = Convert.ToString(dr["JobNumber"]),

                        ServiceCode = Convert.ToString(dr["ServiceCode"]),
                        Description = Convert.ToString(dr["Description"]),

                        StartTime = Convert.ToDouble(dr["StartTime"]),
                        EndTime = Convert.ToDouble(dr["EndTime"]),
                        TravelTime = Convert.ToDouble(dr["TravelTime"]),

                        Attachment = Convert.ToString(dr["Attachment"]),
                        SAP_No = Convert.ToString(dr["SAP_No"]),
                        NewExistingCustomer = Convert.ToString(dr["CustType"]),
                        DomesticInternationVisit   = Convert.ToString(dr["VisitType"])

                    });
                }
                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;
                //return View(lmd.ToList());
                
            }
            return objModel.NIADashBoard;
        }





        #endregion


        public JsonResult CheckValidCall(string companyname)//Checking Existing User Name
        {
            string Result = string.Empty;
            DataTable DTExistRoleName = new DataTable();
            try
            {
                DTExistRoleName = objNIA.DuplicateCall(companyname);
                if (DTExistRoleName.Rows.Count > 0)
                {
                    return Json(1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(0);
        }

        //added by nikita on 06052024
        public ActionResult CheckSendForApproval(string pkcallid)
        {
            DataTable ds = new DataTable();
            string result;
            try
            {
                ds = objNIA.CheckSendForApproval(pkcallid);
                result = JsonConvert.SerializeObject(ds);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }
    }
}


