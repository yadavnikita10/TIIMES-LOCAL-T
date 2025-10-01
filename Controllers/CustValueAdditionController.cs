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
    public class CustValueAdditionController : Controller
    {

        ValueAddition objModel = new ValueAddition();
        DALTrainingCreation objDTC = new DALTrainingCreation();

        DataSet dssGetBranch = new DataSet();
        DalCustSpecific custSpecific = new DalCustSpecific();

        CommonControl objCommonControl = new CommonControl();

        DataSet dss = new DataSet();
        string[] splitedProduct_Name;
        // GET: CustValueAddition
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult ListValueAddition()
        {
            List<ValueAddition> lmd = new List<ValueAddition>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = custSpecific.GetDataValueAddition(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new ValueAddition
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Branch = Convert.ToString(dr["Branch"]),
                    Date = Convert.ToString(dr["Date"]),
                    JobNumberWithSubJob = Convert.ToString(dr["JobNumberWithSubJob"]),
                    CustomerName = Convert.ToString(dr["CustomerName"]),
                    EmployeeName = Convert.ToString(dr["EmployeeName"]),
                    DescriptionOfValueAddition = Convert.ToString(dr["DescriptionOfValueAddition"]),
                    Impact = Convert.ToString(dr["Impact"]),


                });
            }
            objModel.lst1 = lmd;
            //return View(lmd.ToList());
            return View(objModel);

        }


        public ActionResult ValueAddition(int? Id)
        {
            DataSet dss = new DataSet();
            DataSet dssGetBranch = new DataSet();

            var UserData = custSpecific.GetInspectorListForLeaveManagement();
            ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");


            #region Generate Random no
            int _min = 10000;
            int _max = 99999;
            Random _rdm = new Random();
            int Rjno = _rdm.Next(_min, _max);
            string ConfirmCode = Convert.ToString(Rjno);

            int _mins = 100000;
            int _maxs = 999999;
            Random _rdms = new Random();
            int Rjnos = _rdm.Next(_mins, _maxs);
            string ConfirmSecondCode = Convert.ToString(Rjnos);
            objModel.UIIN = ConfirmSecondCode;

            #endregion

            #region Bind branch
            DataSet dsBindBranch = new DataSet();
            List<BranchName> lstBranch = new List<BranchName>();
            dsBindBranch = objDTC.BindBranch();

            if (dsBindBranch.Tables[0].Rows.Count > 0)
            {
                lstBranch = (from n in dsBindBranch.Tables[0].AsEnumerable()
                             select new BranchName()
                             {
                                 Name = n.Field<string>(dsBindBranch.Tables[0].Columns["Branch_Name"].ToString()),
                                 Code = n.Field<Int32>(dsBindBranch.Tables[0].Columns["BR_Id"].ToString())

                             }).ToList();
            }

            IEnumerable<SelectListItem> BranchItems;
            BranchItems = new SelectList(lstBranch, "Code", "Name");
            ViewBag.ProjectTypeItems = BranchItems;
            ViewData["BranchName"] = BranchItems;
            #endregion

            #region Bind User List
            //var UserData = objDalCalls.GetInspectorListForLeaveManagement();
            //ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");

            DataSet dsAuditorName = new DataSet();
            List<Audit> lstAuditorNamee = new List<Audit>();
            dsAuditorName = custSpecific.GetInspectorName_();


            if (dsAuditorName.Tables[0].Rows.Count > 0)
            {
                lstAuditorNamee = (from n in dsAuditorName.Tables[0].AsEnumerable()
                                   select new Audit()
                                   {
                                       DAuditorName = n.Field<string>(dsAuditorName.Tables[0].Columns["Name"].ToString()),
                                       DAuditorCode = n.Field<string>(dsAuditorName.Tables[0].Columns["Code"].ToString())

                                   }).ToList();
            }

            IEnumerable<SelectListItem> ProductcheckItems;
            ProductcheckItems = new SelectList(lstAuditorNamee, "DAuditorCode", "DAuditorName");
            ViewBag.ProjectTypeItems = ProductcheckItems;
            ViewData["ProjectTypeItems"] = lstAuditorNamee;

            //ViewData["Drpproduct"] = objDAM.GetDrpList();
            #endregion


            if (Id > 0)
            {
                ViewBag.check = "productcheck";
                #region Bind File
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = custSpecific.GetFile_(Convert.ToInt32(Id));
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
                objModel.FileDetails = lstEditFileDetails;

                #endregion
                dss = custSpecific.GetDataById_(Convert.ToInt32(Id));
                if (dss.Tables[0].Rows.Count > 0)
                {

                    objModel.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                    objModel.Branch = Convert.ToString(dss.Tables[0].Rows[0]["Branch"]);
                    objModel.Date = Convert.ToString(dss.Tables[0].Rows[0]["Date"]);
                    objModel.JobNumberWithSubJob = Convert.ToString(dss.Tables[0].Rows[0]["JobNumberWithSubJob"]);
                    objModel.ProjectName = Convert.ToString(dss.Tables[0].Rows[0]["ProjectName"]);
                    objModel.CustomerName = Convert.ToString(dss.Tables[0].Rows[0]["CustomerName"]);
                    objModel.VendorName = Convert.ToString(dss.Tables[0].Rows[0]["VendorName"]);
                    objModel.SubVendorName = Convert.ToString(dss.Tables[0].Rows[0]["SubVendorName"]);
                    objModel.EmployeeName = Convert.ToString(dss.Tables[0].Rows[0]["EmployeeName"]);
                    objModel.DescriptionOfValueAddition = Convert.ToString(dss.Tables[0].Rows[0]["DescriptionOfValueAddition"]);
                    objModel.Impact = Convert.ToString(dss.Tables[0].Rows[0]["Impact"]);
                    objModel.CreatedDate = Convert.ToString(dss.Tables[0].Rows[0]["CreatedDate"]);
                    objModel.CreatedBy = Convert.ToString(dss.Tables[0].Rows[0]["CreatedBy"]);
                    objModel.ModifiedDate = Convert.ToString(dss.Tables[0].Rows[0]["ModifiedDate"]);
                    objModel.ModifiedBy = Convert.ToString(dss.Tables[0].Rows[0]["ModifiedBy"]);
                    objModel.UIIN = Convert.ToString(dss.Tables[0].Rows[0]["UIIN"]);
                    objModel.Remarks = Convert.ToString(dss.Tables[0].Rows[0]["Remarks"]);

                    List<string> Selected = new List<string>();
                    var Existingins = Convert.ToString(dss.Tables[0].Rows[0]["EmployeeName"]);
                    splitedProduct_Name = Existingins.Split(',');
                    foreach (var single in splitedProduct_Name)
                    {
                        Selected.Add(single);
                    }
                    ViewBag.EditproductName = Selected;

                }
                return View(objModel);
            }
            else
            {
                dssGetBranch = custSpecific.GetBranch();
                if (dssGetBranch.Tables[0].Rows.Count > 0)
                {
                    objModel.Branch = dssGetBranch.Tables[0].Rows[0]["Fk_branchid"].ToString();
                }

                return View(objModel);
            }



        }


        [HttpPost]
        public ActionResult ValueAddition(ValueAddition S, FormCollection fc)
        {
            string ProList = string.Join(",", fc["ProductListName"]);
            S.EmployeeName = ProList;

            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listJobMasterUploadedFile"] as List<FileDetails>;


            string Result = string.Empty;
            int Result1 = 0;
            try
            {

                if (S.Id > 0)
                {
                    ViewBag.check = "productcheck";
                    //Update
                    Result = custSpecific._Insert_(S);
                    //if (Convert.ToInt16(Result) > 0)
                    if (Convert.ToInt16(S.Id) > 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = custSpecific.InsertFileAttachment_V(lstFileDtls, Convert.ToInt32(S.Id), S);
                            Session["listJobMasterUploadedFile"] = null;
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(S.Id));
                        }
                        return RedirectToAction("ValueAddition", new { Id = S.Id });
                        ModelState.Clear();
                        TempData["message"] = "Record Added Successfully";
                    }


                }
                else
                {

                    Result = custSpecific._Insert_(S);
                    if (Convert.ToInt16(Result) > 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = custSpecific.InsertFileAttachment_V(lstFileDtls, Convert.ToInt32(S.Id), S);
                            Session["listJobMasterUploadedFile"] = null;
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(Result));
                        }

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
            return RedirectToAction("ValueAddition", new { Id = Result });
            //return RedirectToAction("ListIAFScopeMaster", "IAFScopeMaster");
        }
        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = custSpecific.Delete_valueAddition(Convert.ToInt32(Id));
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
            return RedirectToAction("ListValueAddition");


        }

    }
}