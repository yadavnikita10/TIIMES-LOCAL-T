using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class ExpenseItemController : Controller
    {

        DALExpenseItem objEI = new DALExpenseItem();
        ExpenseItem objModel = new ExpenseItem();
        DALAudit objDALAudit = new DALAudit(); 

        // GET: ExpenseItem
        public ActionResult CreateExpenseItem(int? FKId, string Type, int? PKExpenseId, string SubJobNo, int? PK_Call_Id, int? AuditId, string Pk_CC_Id, int? ActivityCode)
        {
            ViewBag.PreviousUrl = Request.UrlReferrer?.ToString(); // Store the previous URL in ViewBag
            objModel.SubJobNo = SubJobNo;
            objModel.FKId = Convert.ToInt32(FKId);
            objModel.Type = Type;
            objModel.PK_Call_Id = Convert.ToInt32(PK_Call_Id);
            objModel.ActivityType = Convert.ToString(ActivityCode);
            string Costcenter = string.Empty;

            //Added By Satish Pawar On 05 July 2023
            objModel.AuditId = Convert.ToInt32(AuditId);
            DataSet dsGetAudit_ = new DataSet();
            if (AuditId != null)
            {
                //added by nikita on 12012024

                dsGetAudit_ = objDALAudit.GetAuditCostcenter("1", AuditId);
                objModel.FKId = Convert.ToInt32(AuditId);
                Costcenter = Convert.ToString(dsGetAudit_.Tables[0].Rows[0][0]);
                //objModel.FKId = Convert.ToString(dsGetAudit_.Tables[0].Rows[0][0]);

            }
            //added by nikita on 05/02/2024
            else if (Type == "Onsite")
            {
                dsGetAudit_ = objDALAudit.GetMonitoring("4", FKId);
                Costcenter = Convert.ToString(dsGetAudit_.Tables[0].Rows[0][0]);
            }

            else if (Type == "MOM")
            {
                dsGetAudit_ = objDALAudit.GetMonitoring("3", FKId);
                Costcenter = Convert.ToString(dsGetAudit_.Tables[0].Rows[0][0]);
            }
            // if (Type == "Onsite" || Type == "MOM")
            //{

            //        dsGetAudit_ = objDALAudit.GetMonitoringCostcenter(Type, FKId);
            //        Costcenter = Convert.ToString(dsGetAudit_.Tables[0].Rows[0][0]);
            //}
            //Added By Satish Pawar On 05 July 2023
            TempData["AuditId"] = AuditId;

            TempData["FKID"] = Convert.ToInt32(FKId);
            TempData["Type"] = Type;
            TempData["SubJobNo"] = SubJobNo;
            TempData["PK_Call_Id"] = PK_Call_Id;
            TempData["Coscenter_obswise"] = Costcenter;

            TempData.Keep();
            objModel.ExchRate = Convert.ToInt32(1);
            string costcentre = "";
            if (Session["costcentre"] != null)
            {
                costcentre = Session["costcentre"].ToString();
            }

            //added by nikita on 30042024
            string cost = "";

            if (Type == "NIA" && !string.IsNullOrEmpty(Pk_CC_Id))
            {
                DataSet GetCostcenter_NIA = new DataSet();
                GetCostcenter_NIA = objEI.GetCost_center(Type, Pk_CC_Id);
                cost = Convert.ToString(GetCostcenter_NIA.Tables[0].Rows[0][0]);
                objModel.CostCenter = cost;
                Session["cost_center_NIA"] = Pk_CC_Id;

            }
            else
            {
                objModel.CostCenter = costcentre;
            }

            //end
            //objModel.CostCenter = costcentre;

            var Data = objEI.GetCountryList();
            // ViewBag.SubCatlist = new SelectList(Data, "Id", "CountryName");

            DataSet dsMasters = objEI.GetMasters();
            string jsonCountry = JsonConvert.SerializeObject(dsMasters.Tables[0], Formatting.Indented);
            string jsonExpenses = JsonConvert.SerializeObject(dsMasters.Tables[1], Formatting.Indented);
            string jsonCurrency = JsonConvert.SerializeObject(dsMasters.Tables[2], Formatting.Indented);
            string jsonOwnCarRate = JsonConvert.SerializeObject(dsMasters.Tables[3], Formatting.Indented);

            List<Country> CountryList = JsonConvert.DeserializeObject<List<Country>>(jsonCountry);
          //  ViewBag.SubCatlist = new SelectList(CountryList, "CountryName", "CountryName");
            ViewBag.SubCatlist = new SelectList(CountryList, "ID", "CountryName");

            List<ExpenseMode> ExpensesList = JsonConvert.DeserializeObject<List<ExpenseMode>>(jsonExpenses);
            ViewBag.ExpensesList = new SelectList(ExpensesList, "Mode", "Mode");

            List<ExpensesCurrency> CurrencyList = JsonConvert.DeserializeObject<List<ExpensesCurrency>>(jsonCurrency);
            ViewBag.CurrencyList = new SelectList(CurrencyList, "CurrencyCode", "CurrencyCode");

            //CarRatePerKM carRatePerKM = new CarRatePerKM();// JsonConvert.DeserializeObject<CarRatePerKM>(jsonOwnCarRate);
            List<CarRatePerKM> carRatePerKM = new List<CarRatePerKM>();
            carRatePerKM.Add(new CarRatePerKM
            {
                ID = dsMasters.Tables[3].Rows[0]["ID"].ToString(),
                CarRate = dsMasters.Tables[3].Rows[0]["CarRate"].ToString(),
                MotorBikeRate = dsMasters.Tables[3].Rows[0]["MotorBikeRate"].ToString()
            });
            ViewBag.CarRatePerKM = carRatePerKM;

            //Added BY Satish Pawar On 24 July 2023
            ViewBag.MaxExpenseLimit = dsMasters.Tables[4].Rows[0][0].ToString();

            //added  on 12042024
            var ActivityType = ActivityCode.ToString();
            Session["Activitycode"] = ActivityType;
            //end


            var myData = TempData["VisitedDates_"] as string;
            if(myData == null)
            {
                DataSet dsType = new DataSet();
                if (Type == "NIA")
                {
                    dsType = objEI.GetType(Type, FKId);
                    if (dsType != null || dsType.Tables.Count > 1)
                    {
                        string VisitedDate = Convert.ToDateTime(dsType.Tables[0].Rows[0]["DateSE"].ToString()).ToString("dd/MM/yyyy");
                        //string b = Convert.ToDateTime(a).ToString("yyyy-MM-dd");
                        
                        ViewBag.VisitedDates = VisitedDate;
                    }
                    else
                        ViewBag.VisitedDates = "";
                }
                if (Type == "IVR")
                {
                    dsType = objEI.GetType(Type, FKId);
                    if (dsType != null || dsType.Tables.Count > 1)
                    {
                        string VisitedDate = dsType.Tables[0].Rows[0]["Dates"].ToString().TrimStart(',');
                        //string b = Convert.ToDateTime(a).ToString("yyyy-MM-dd");
                        string[] a= VisitedDate.Split(',');
                        string _VisitedDate = "";
                        foreach (string date in a)
                        {
                          string  New_VisitedDate = Convert.ToDateTime(date.ToString()).ToString("dd/MM/yyyy");
                            _VisitedDate = New_VisitedDate + "," + _VisitedDate;
                        }

                        ViewBag.VisitedDates = _VisitedDate.TrimEnd(',');
                    }
                    else
                        ViewBag.VisitedDates = "";
                    
                }
                //added by nikita on 23102023
                if (Type == "Training")
                {
                    dsType = objEI.GetType(Type, FKId);
                    if (dsType != null || dsType.Tables.Count > 1)
                    {
                        //string VisitedDate = Convert.ToDateTime(dsType.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd/MM/yyyy");
                        //string b = Convert.ToDateTime(a).ToString("yyyy-MM-dd");
                        string VisitedDate = dsType.Tables[0].Rows[0]["CreatedDate"].ToString();
                        ViewBag.VisitedDates = VisitedDate;
                    }
                    else
                        ViewBag.VisitedDates = "";
                }
                if (Type == "Onsite")
                {
                    dsType = objEI.GetType(Type, FKId);
                    if (dsType != null || dsType.Tables.Count > 1)
                    {
                        string VisitedDate = dsType.Tables[0].Rows[0]["DateSE"].ToString().TrimStart(',');
                        //string b = Convert.ToDateTime(a).ToString("yyyy-MM-dd");
                        string[] a = VisitedDate.Split(',');
                        string _VisitedDate = "";
                        foreach (string date in a)
                        {
                            string New_VisitedDate = Convert.ToDateTime(date.ToString()).ToString("dd/MM/yyyy");
                            _VisitedDate = New_VisitedDate + "," + _VisitedDate;
                        }

                        ViewBag.VisitedDates = _VisitedDate.TrimEnd(',');
                    }
                    else
                        ViewBag.VisitedDates = "";

                }
                if (Type == "MOM")
                {
                    dsType = objEI.GetType(Type, FKId);
                    if (dsType != null || dsType.Tables.Count > 1)
                    {
                        string VisitedDate = dsType.Tables[0].Rows[0]["DateSE"].ToString().TrimStart(',');
                        //string b = Convert.ToDateTime(a).ToString("yyyy-MM-dd");
                        string[] a = VisitedDate.Split(',');
                        string _VisitedDate = "";
                        foreach (string date in a)
                        {
                            string New_VisitedDate = Convert.ToDateTime(date.ToString()).ToString("dd/MM/yyyy");
                            _VisitedDate = New_VisitedDate + "," + _VisitedDate;
                        }

                        ViewBag.VisitedDates = _VisitedDate.TrimEnd(',');
                    }
                    else
                        ViewBag.VisitedDates = "";

                }
                if (Type == "Internal Audit")
                {
                    dsType = objEI.GetType(Type, AuditId);
                    if (dsType != null || dsType.Tables.Count > 1)
                    {
                        string VisitedDate = dsType.Tables[0].Rows[0]["DateSE"].ToString().TrimStart(',');
                        //string b = Convert.ToDateTime(a).ToString("yyyy-MM-dd");
                        string[] a = VisitedDate.Split(',');
                        string _VisitedDate = "";
                        foreach (string date in a)
                        {
                            string New_VisitedDate = Convert.ToDateTime(date.ToString()).ToString("dd/MM/yyyy");
                            _VisitedDate = New_VisitedDate + "," + _VisitedDate;
                        }

                        ViewBag.VisitedDates = _VisitedDate.TrimEnd(',');
                    }
                    else
                        ViewBag.VisitedDates = "";

                }

                
                //added by nikita on 05042024
                if (Type == "Expediting")
                {
                    dsType = objEI.GetType(Type, FKId);
                    if (dsType != null || dsType.Tables.Count > 1)
                    {
                        string VisitedDate = dsType.Tables[0].Rows[0]["DateSE"].ToString().TrimStart(',');
                        //string b = Convert.ToDateTime(a).ToString("yyyy-MM-dd");
                        string[] a = VisitedDate.Split(',');
                        string _VisitedDate = "";
                        foreach (string date in a)
                        {
                            string New_VisitedDate = Convert.ToDateTime(date.ToString()).ToString("dd/MM/yyyy");
                            _VisitedDate = New_VisitedDate + "," + _VisitedDate;
                        }

                        ViewBag.VisitedDates = _VisitedDate.TrimEnd(',');
                    }
                    else
                        ViewBag.VisitedDates = "";

                }

                //end

                //else
                //    ViewBag.VisitedDates = "";
                //if (Type=="NIA")
                //{
                //    DataSet dsType = objEI.GetType(Type, FKId);
                //}
                //ViewBag.VisitedDates = "2023-05-24,2023-05-23,2023-05-22";
            }
            else
            {
                ViewBag.VisitedDates = myData.TrimEnd(',');
            }


            //added by nikita on  11032024

            DataTable dt = new DataTable();
            string date_ = ViewBag.VisitedDates;
            dt = objEI.Checked(date_, FKId);

            if (dt != null && dt.Rows.Count > 0)
            {
                string VisitedDate = dt.Rows[0]["IsSendForApproval"].ToString().TrimStart(',');

                //ViewBag.status = VisitedDate;
            }
            else
            {
                ViewBag.status = "";
            }

            #region get expenseItem
            List<ExpenseItem> lmd = new List<ExpenseItem>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objEI.GetExpenseItem(objModel); // fill dataset 
            
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    objModel.UIN = Convert.ToString(ds.Tables[1].Rows[0]["UIN"]);
                    objModel.SAPNo = Convert.ToString(ds.Tables[1].Rows[0]["SAP_No"]);
                }

                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new ExpenseItem
                    {
                        Type = Convert.ToString(dr["Type"]),
                        FKId = Convert.ToInt32(dr["FKId"]),
                        ExpenseType = Convert.ToString(dr["ExpenseType"]),
                        Date = Convert.ToString(dr["Date"]),
                        //TotalAmount = Convert.ToInt32(dr["TotalAmount"]),
                        TotalAmount = Convert.ToDouble(dr["TotalAmount"]),
                        PKExpenseId = Convert.ToInt32(dr["PKExpenseId"]),
                        SubJobNo = Convert.ToString(dr["SubJobNo"]),
                        UIN = Convert.ToString(dr["UIN"]),
                        VoucherNo = Convert.ToString(dr["VoucherNo"]),
                        IsSendForApproval = Convert.ToString(dr["IsSendForApproval"]),
                        SAPNo = Convert.ToString(dr["SAP_No"]),
                        City = Convert.ToString(dr["City"]),
                        Attachment = Convert.ToString(dr["Attachment"]),
                        CostCenter = Convert.ToString(dr["CostCenter"]),
                        Country = Convert.ToString(dr["Country"]),
                        KM = Convert.ToString(dr["KM"]),
                        Amount = Convert.ToDouble(dr["Amount"]),
                        ExchRate = Convert.ToDouble(dr["ExchRate"]),
                        Description = Convert.ToString(dr["Description"]),
                        Currency = Convert.ToString(dr["Currency"]),


                        //Type = Convert.ToString(dr["Type"]),
                        //FKId = Convert.ToInt32(dr["FKId"]),
                        //ExpenseType = Convert.ToString(dr["ExpenseType"]),
                        //Date = Convert.ToString(dr["Date"]),
                        //TotalAmount = Convert.ToDouble(dr["TotalAmount"]),
                        //PKExpenseId = Convert.ToInt32(dr["PKExpenseId"]),
                        //SubJobNo = Convert.ToString(dr["SubJobNo"]),
                        //UIN = Convert.ToString(dr["UIN"]),
                        //VoucherNo = Convert.ToString(dr["VoucherNo"]),
                        //IsSendForApproval = Convert.ToString(dr["IsSendForApproval"]),
                        ////SAPNo = Convert.ToString(dr["SAP_No"]),
                        //City = Convert.ToString(dr["City"]),
                        //Attachment = Convert.ToString(dr["Attachment"]),
                        //CostCenter = costcentre

                    });
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    objModel.VoucherNo = Convert.ToString(ds.Tables[0].Rows[0]["VoucherNo"]);
                    //objModel.SAPNo = Convert.ToString(ds.Tables[0].Rows[0]["SAP_No"]);
                    ViewBag.ExpenseItem = lmd;
                }
                else
                {
                    ViewBag.ExpenseItem = "";
                }
            }
            else
            {
                ViewBag.ExpenseItem = "";
            }
            #endregion

            //if (PKExpenseId > 0)
            //{
            //    DataSet dsGetDataById = new DataSet();

            //    dsGetDataById = objEI.GetDataById(Convert.ToInt32(PKExpenseId));
            //    if (dsGetDataById.Tables.Count > 0)
            //    {
            //        objModel.Country = dsGetDataById.Tables[0].Rows[0]["Country"].ToString();
            //        objModel.City = dsGetDataById.Tables[0].Rows[0]["City"].ToString();
            //        objModel.ExpenseType = dsGetDataById.Tables[0].Rows[0]["ExpenseType"].ToString();
            //        objModel.Date = Convert.ToString(dsGetDataById.Tables[0].Rows[0]["Date"]);
            //        objModel.Currency = dsGetDataById.Tables[0].Rows[0]["Currency"].ToString();
            //        objModel.Amount = Convert.ToDouble(dsGetDataById.Tables[0].Rows[0]["Amount"]);
            //        objModel.ExchRate = Convert.ToDouble(dsGetDataById.Tables[0].Rows[0]["ExchRate"]);
            //        objModel.TotalAmount = Convert.ToDouble(dsGetDataById.Tables[0].Rows[0]["TotalAmount"]);
            //        objModel.Description = dsGetDataById.Tables[0].Rows[0]["Description"].ToString();
            //        objModel.Attachment = dsGetDataById.Tables[0].Rows[0]["Attachment"].ToString();
            //        objModel.SubJobNo = dsGetDataById.Tables[0].Rows[0]["SubJobNo"].ToString();


            //    }

            //    DataTable DTGetUploadedFile = new DataTable();
            //    List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            //    DTGetUploadedFile = objEI.EditUploadedFile(PKExpenseId);
            //    if (DTGetUploadedFile.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in DTGetUploadedFile.Rows)
            //        {
            //            lstEditFileDetails.Add(
            //               new FileDetails
            //               {

            //                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
            //                   FileName = Convert.ToString(dr["FileName"]),
            //                   Extension = Convert.ToString(dr["Extenstion"]),
            //                   IDS = Convert.ToString(dr["FileID"]),
            //               }
            //             );
            //        }
            //        ViewData["lstEditFileDetails"] = lstEditFileDetails;
            //        objModel.FileDetails = lstEditFileDetails;
            //    }
            //    return View(objModel);
            //}
            //else
            //{

            //    return View(objModel);
            //}
            return View(objModel);

        }

        
        //Added By Satish Pawar on 19 May 2023
        public JsonResult Get_ExpenseById(string PKExpenseId)
        {
            DataSet dsGetDataById = new DataSet();

            dsGetDataById = objEI.GetDataById(Convert.ToInt32(PKExpenseId));
            if (dsGetDataById.Tables.Count > 0)
            {
                objModel.Country = dsGetDataById.Tables[0].Rows[0]["Country"].ToString();
                objModel.City = dsGetDataById.Tables[0].Rows[0]["City"].ToString();
                objModel.ExpenseType = dsGetDataById.Tables[0].Rows[0]["ExpenseType"].ToString();
                objModel.Date = Convert.ToString(dsGetDataById.Tables[0].Rows[0]["Date"]);
                objModel.Currency = dsGetDataById.Tables[0].Rows[0]["Currency"].ToString();
                objModel.Amount = Convert.ToDouble(dsGetDataById.Tables[0].Rows[0]["Amount"]);
                objModel.ExchRate = Convert.ToDouble(dsGetDataById.Tables[0].Rows[0]["ExchRate"]);
                objModel.TotalAmount = Convert.ToDouble(dsGetDataById.Tables[0].Rows[0]["TotalAmount"]);
                objModel.Description = dsGetDataById.Tables[0].Rows[0]["Description"].ToString();
                objModel.Attachment = dsGetDataById.Tables[0].Rows[0]["Attachment"].ToString();
                objModel.SubJobNo = dsGetDataById.Tables[0].Rows[0]["SubJobNo"].ToString();
                objModel.VoucherNo = dsGetDataById.Tables[0].Rows[0]["VoucherNo"].ToString();
                objModel.CostCenter = dsGetDataById.Tables[0].Rows[0]["CostCenter"].ToString(); //added by nikita on 03052024

            }

            DataTable DTGetUploadedFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTGetUploadedFile = objEI.EditUploadedFile(Convert.ToInt32(PKExpenseId));
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
                objModel.FileDetails = lstEditFileDetails;
            }
            string json = JsonConvert.SerializeObject(dsGetDataById.Tables[0]);
            //return View(objModel);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public void GetFile()
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            List<FileDetails> fileDetails = new List<FileDetails>();

            //---Adding end Code

            if (Session["listExpenseUploadedFile"] != null)
            {
                fileDetails = Session["listExpenseUploadedFile"] as List<FileDetails>;
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
                        if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".PDF") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpeg") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".PNG") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))
                        {
                            string fileName = files.FileName;
                            //Adding New Code as per new requirement 12 March 2020, Manoj Sharma
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();
                            fileDetails.Add(fileDetail);
                            //-----------------------------------------------------

                            filePath = Path.Combine(Server.MapPath("~/Files/ExpensesAttachment/"), fileDetail.Id + fileDetail.Extension);

                            var K = "~/Files/ExpensesAttachment/" + fileName;
                            IPath = K;
                            files.SaveAs(filePath);
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
                            ViewBag.Error = "Please Select XLSX or PDF Image File";
                        }
                    }
                }
                Session["listExpenseUploadedFile"] = fileDetails;
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
        }

        [HttpPost]
        public ActionResult CreateExpenseItem(ExpenseItem E, List<HttpPostedFileBase> img_Banner, string Save)
        {
            
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listExpenseUploadedFile"] as List<FileDetails>;

            //string pkCcId = Convert.ToString(TempData["Coscenter_obswise"]);
            var cost = "";
            DataSet GetCostcenter_NIA = new DataSet();

            GetCostcenter_NIA = objEI.GetCost_center_id(E.CostCenter);
            cost = Convert.ToString(GetCostcenter_NIA.Tables[0].Rows[0][0]);
            string pk_cc_id_ = "";
            string pkCcId = "";

            //string pkCcId = Convert.ToString(TempData["Pk_CC_Id"]);
            var activity = Session["Activitycode"].ToString();     ///added by nikita on 29042024
            //var NIA_costcenter = Session["cost_center_NIA"].ToString();
            string NIA_costcenter = "";
            if (Session["cost_center_NIA"] != null)
            {
                NIA_costcenter = Session["cost_center_NIA"].ToString();
            }
            else
            {

            }

            if (activity == "1" || activity == "02" || activity == "03" || activity == "30" || activity == "31" || activity == "34" || activity == "32" || activity == "27")
            {
                pk_cc_id_ = NIA_costcenter;
            }
            if (TempData["Coscenter_obswise"].ToString() != "")
            {
                pkCcId = Convert.ToString(TempData["Coscenter_obswise"]);
            }
            else
            {
                //pkCcId = E.CostCenter;
            }
            string buttonName = "Save";
            string buttonText = Request.Form[buttonName];

            string Result = string.Empty;
            string IPath = string.Empty;

            var list = Session["list"] as List<string>;
            List<string> lstAttachment = new List<string>();

            //List<FileDetails> lstFileDtls = new List<FileDetails>();
            //lstFileDtls = Session["listExpenseUploadedFile"] as List<FileDetails>;

            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }
            try
            {
                #region getExpenseItem

                List<ExpenseItem> lmd = new List<ExpenseItem>();  // creating list of model.  
                DataSet ds = new DataSet();

                ds = objEI.GetExpenseItem(objModel); // fill dataset  
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                    {
                        lmd.Add(new ExpenseItem
                        {
                            Type = Convert.ToString(dr["Type"]),
                            FKId = Convert.ToInt32(dr["FKId"]),
                            ExpenseType = Convert.ToString(dr["ExpenseType"]),
                            Date = Convert.ToString(dr["Date"]),
                            //TotalAmount = Convert.ToInt32(dr["TotalAmount"]),
                            TotalAmount = Convert.ToDouble(dr["TotalAmount"]),
                            PKExpenseId = Convert.ToInt32(dr["PKExpenseId"]),

                        });
                    }
                    ViewBag.ExpenseItem = lmd;
                    #endregion
                }
                int ExpID = 0;

                //double CalTotalAmount;
                //CalTotalAmount = E.Amount * E.ExchRate;
                //E.TotalAmount = CalTotalAmount;

                // if (E.PKExpenseId > 0)
                if (Save == "Update")
                {
                    string hiddenFieldValue = Request.Form["PKExpenseId"];
                    E.PKExpenseId = Convert.ToInt32(hiddenFieldValue);
                    //Update
                    Result = objEI.Insert(E, IPath, pkCcId, pk_cc_id_);

                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        Result = objEI.InsertFileAttachment(lstFileDtls, E.PKExpenseId);
                        Session["listExpenseUploadedFile"] = null;
                    }



                }
                else
                {
                    Result = objEI.Insert(E, IPath, pkCcId, pk_cc_id_);
                    ExpID = Convert.ToInt32(Session["EXID"]);

                    int pkexpenseid = E.PKExpenseId;

                    if (ExpID > 0)
                    {

                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = objEI.InsertFileAttachment(lstFileDtls, ExpID);
                            Session["listExpenseUploadedFile"] = null;
                        }
                        else if (E.PKExpenseId > 0)
                        {
                            if (lstFileDtls != null && lstFileDtls.Count > 0)
                            {
                                Result = objEI.UpdateFileAttachment(lstFileDtls, E.PKExpenseId);
                                Session["listExpenseUploadedFile"] = null;
                            }
                            else if (lstFileDtls == null || lstFileDtls.Count == 0)
                            {
                                Result = objEI.DeleteUploadedFile(E.PKExpenseId);
                                Session["listExpenseUploadedFile"] = null;
                            }
                        }
                    }
                   
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


            return RedirectToAction("CreateExpenseItem", new RouteValueDictionary(new { controller = "ExpenseItem", action = "CreateExpenseItem", FKId = TempData["FKID"], Type = TempData["Type"], SubJobNo = TempData["SubJobNo"], PK_Call_Id = TempData["PK_Call_Id"], Pk_CC_Id= cost }));
        }




        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            List<FileDetails> fileDetails = new List<FileDetails>();

            //---Adding end Code

            if (Session["listExpenseUploadedFile"] != null)
            {

                fileDetails = Session["listExpenseUploadedFile"] as List<FileDetails>;

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

                        if (files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".PDF") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpeg") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".PNG") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))

                        {

                            string fileName = files.FileName;

                            //Adding New Code as per new requirement 12 March 2020, Manoj Sharma

                            FileDetails fileDetail = new FileDetails();

                            fileDetail.FileName = fileName;

                            fileDetail.Extension = Path.GetExtension(fileName);

                            fileDetail.Id = Guid.NewGuid();

                            fileDetails.Add(fileDetail);

                            //-----------------------------------------------------

                            filePath = Path.Combine(Server.MapPath("~/Files/ExpensesAttachment/"), fileDetail.Id + fileDetail.Extension);

                            //var K = "~/Files/ExpensesAttachment/" + fileName;  //coomenetd by nikita on 31012024 

                            var K = "~/Files/ExpensesAttachment/" + fileDetail.Id + fileDetail.Extension;                            //added by nikita on 31012024

                            IPath = K;

                            files.SaveAs(filePath);



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

                            ViewBag.Error = "Please Select XLSX or PDF Image File";

                        }

                    }



                }
                Session["listExpenseUploadedFile"] = fileDetails;


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Delete(int? PKExpenseId)
        {
            string Result = string.Empty;

            int FKId = Convert.ToInt32(TempData["FKID"]);
            string Type = Convert.ToString(TempData["Type"]);
            string SubJobNo = Convert.ToString(TempData["SubJobNo"]);
            int PK_Call_Id = Convert.ToInt32(TempData["PK_Call_Id"]);
            string pk_CC_ID = string.Empty;
            //DataSet ds = new DataSet();
            //ds = objEI.GetCostCentre_ExpId(Convert.ToInt32(TempData["FKID"]));
            //if (ds.Tables[0].Rows.Count > 0)
            //    pk_CC_ID = ds.Tables[0].Rows[0][0].ToString();

            pk_CC_ID = Convert.ToString( Session["cost_center_NIA"]);



            try
            {
                Result = objEI.Delete(Convert.ToInt32(PKExpenseId));
                if (Convert.ToInt16(Result) > 0)
                {
             

                    return RedirectToAction("CreateExpenseItem", new RouteValueDictionary(new
                    {
                        controller = "ExpenseItem",
                        action = "CreateExpenseItem",
                        FKId = FKId,
                        Type = Type,
                        SubJobNo = SubJobNo,
                        PK_Call_Id = PK_Call_Id,
                        pk_CC_ID = pk_CC_ID
                    }));

                    //TempData["Type"] = Type;
                    //TempData["SubJobNo"] = SubJobNo;
                    //TempData["PK_Call_Id"] = PK_Call_Id;
                    //TempData.Keep();

                    //ModelState.Clear();
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
            //ModelState.Clear();

            //return RedirectToAction("CreateExpenseItem", new RouteValueDictionary(new { controller = "ExpenseItem", action = "CreateExpenseItem", FKId = TempData["FKID"], Type = TempData["Type"] }));

            return RedirectToAction("CreateExpenseItem", new RouteValueDictionary(new
            {
                controller = "ExpenseItem",
                action = "CreateExpenseItem",
                FKId = FKId,
                Type = Type,
                SubJobNo = SubJobNo,
                PK_Call_Id = PK_Call_Id,
                pk_CC_ID = pk_CC_ID
            }));


        }


        public FileResult Download(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/Files/ExpensesAttachment/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }




        #region Expenses
        public ActionResult Expenses()
        {
            return View();
        }

        #endregion
    }
}