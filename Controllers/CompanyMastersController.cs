using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using TuvVision.DataAccessLayer;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using NonFactors.Mvc.Grid;
using System.Net;
using Newtonsoft.Json;

namespace TuvVision.Controllers
{
    public class CompanyMastersController : Controller
    {
        DALCompanyMaster objDalCompany = new DALCompanyMaster();
        CompanyMaster ObjModelCompany = new CompanyMaster();
        DataSet DSGetAllDdlList = new DataSet();
        List<NameCode> lstStatusList = new List<NameCode>();
        // GET: CompanyMasters
        public ActionResult CompanyDashBoard()
        {
            DataTable DTCompanyDashBoard = new DataTable();
            List<CompanyMaster> lstCompanyDashBoard = new List<CompanyMaster>();
            DTCompanyDashBoard = objDalCompany.GetCompanyDashBoard();
            try
            {
                if (DTCompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CompanyMaster
                            {
                                //CMP_ID = Convert.ToInt32(dr["CMP_ID"]),
                                //Company_Name = Convert.ToString(dr["CompanyName"]),
                                //BranchName = Convert.ToString(dr["BranchName"]),
                                //Email = Convert.ToString(dr["Email"]),
                                //TitleName = Convert.ToString(dr["Title"]),

                                //Status = Convert.ToString(dr["StatusName"]),
                                //CorporateName = Convert.ToString(dr["CorporateName"]),
                                //Address = Convert.ToString(dr["Address"]),
                                //Pan_No = Convert.ToString(dr["Pan_No"]),
                                //companyId = Convert.ToString(dr["companyId"]),
                                CMP_ID = Convert.ToInt32(dr["CMP_ID"]),
                                Company_Name = Convert.ToString(dr["CompanyName"]),
                                BranchName = Convert.ToString(dr["BranchName"]),
                                Email = Convert.ToString(dr["Email"]),
                                TitleName = Convert.ToString(dr["Title"]),
                                // Mobile = Convert.ToString(dr["Mobile"]),
                                Status = Convert.ToString(dr["StatusName"]),
                                CorporateName = Convert.ToString(dr["CorporateName"]),
                                //Address = Convert.ToString(dr["Address"]),
                                Pan_No = Convert.ToString(dr["Pan_No"]),
                                companyId = Convert.ToString(dr["companyId"]),
                                IndustryName = Convert.ToString(dr["IndustryName"]),
                                Work_Phone = Convert.ToString(dr["Landline"]),
                                Address_Account = Convert.ToString(dr["SiteAddress"]),
                                AddressType = Convert.ToString(dr["Type"]),

                                Website = Convert.ToString(dr["Website"]),

                            }
                            );
                    }


                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CompanyList"] = lstCompanyDashBoard;
            ObjModelCompany.lstCompanyMaster1 = lstCompanyDashBoard;
            return View(ObjModelCompany);
        }
        [HttpGet]
        public ActionResult CreateCompany(int? CompanyID)
        {

            ObjModelCompany.vendorid = 1;

            if (CompanyID != 0 && CompanyID != null)
            {

                TempData["ShowContactName"] = CompanyID;
                TempData.Keep();
                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalCompany.EditCompany(CompanyID);
                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ObjModelCompany.CMP_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["CMP_ID"]);
                    ObjModelCompany.Company_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Company_Name"]);
                    ObjModelCompany.Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Email"]);
                    ObjModelCompany.Website = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Website"]);
                    ObjModelCompany.Work_Phone = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Work_Phone"]);
                    ObjModelCompany.TitleName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Title"]);
                    ObjModelCompany.Ind_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["Ind_ID"]);
                    ObjModelCompany.Address = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Address"]);
                    ObjModelCompany.Main = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Main"]);
                    ObjModelCompany.BranchName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Br_Id"]);
                    ObjModelCompany.Fax_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Fax_No"]);
                    ObjModelCompany.Branch_Description = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Branch_Description"]);
                    ObjModelCompany.Pan_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Pan_No"]);
                    ObjModelCompany.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Status"]);
                    ObjModelCompany.CG_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["CG_ID"]);
                    //ObjModelCompany.Address_Account = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Address_Account"]);
                    ObjModelCompany.Fax_Account = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Fax_Account"]);
                    ObjModelCompany.Home_Page = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Home_Page"]);
                    ObjModelCompany.Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Contact"]);
                    ObjModelCompany.Mobile = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Mobile"]);
                    ObjModelCompany.CreatedBy = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["CreatedBy"]);
                    ObjModelCompany.CreatedByName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["CreatedByName"]);
                    ObjModelCompany.createDate = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["CreatedDate"]);
                    ObjModelCompany.OffAddrPin = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["OffAddrPin"]);
                    //ObjModelCompany.SiteAddrPin = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SiteAddrPin"]);
                    ObjModelCompany.InspectionLocation = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["InspectionLocation"]);
                }

                #region Bind Addr
                DataTable dtAttach = new DataTable();
                List<CompanyMaster> lstDTPAN = new List<CompanyMaster>();

                dtAttach = objDalCompany.GetSiteAdressDet(CompanyID);

                if (dtAttach.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAttach.Rows)
                    {
                        lstDTPAN.Add(
                            new CompanyMaster
                            {
                                AddressType = Convert.ToString(dr["AddressType"]),
                                Address_Account = Convert.ToString(dr["SiteAddress"]),
                                SiteAddrPin = Convert.ToString(dr["SitePIN"]),
                                SiteID = Convert.ToString(dr["PKId"])
                            }
                            );
                    }

                    ObjModelCompany.lstSiteAddress = lstDTPAN;
                    ViewData["lstSiteAddress"] = lstDTPAN;
                }
                #endregion
                List<NameCode> lstEditBranchList = new List<NameCode>();
                List<NameCode> lstEditTitleList = new List<NameCode>();
                List<NameCode> lstEditIndustryList = new List<NameCode>();
                List<NameCode> lstEditStatusList = new List<NameCode>();
                List<NameCode> lstEditCorporateList = new List<NameCode>();
                DataSet DSEditGetList = new DataSet();
                DSEditGetList = objDalCompany.GetAllDdlLst();
                if (DSEditGetList.Tables[0].Rows.Count > 0)//Dynamic Binding Branch Code DropDwonlist
                {
                    lstEditBranchList = (from n in DSEditGetList.Tables[0].AsEnumerable()
                                         select new NameCode()
                                         {
                                             Name = n.Field<string>(DSEditGetList.Tables[0].Columns["BranchName"].ToString()),
                                             Code = n.Field<Int32>(DSEditGetList.Tables[0].Columns["PK_ID"].ToString())

                                         }).ToList();
                }
                IEnumerable<SelectListItem> BranchNameItems;
                BranchNameItems = new SelectList(lstEditBranchList, "Code", "Name");
                ViewData["BranchNameItems"] = BranchNameItems;

                if (DSEditGetList.Tables[1].Rows.Count > 0)//Dynamic Binding Industry Code DropDwonlist
                {
                    lstEditTitleList = (from n in DSEditGetList.Tables[1].AsEnumerable()
                                        select new NameCode()
                                        {
                                            Name = n.Field<string>(DSEditGetList.Tables[1].Columns["TitleName"].ToString()),
                                            Code = n.Field<Int32>(DSEditGetList.Tables[1].Columns["PK_TitleID"].ToString())

                                        }).ToList();
                }
                IEnumerable<SelectListItem> TitleNameItems;
                TitleNameItems = new SelectList(lstEditTitleList, "Code", "Name");
                ViewData["TitleNameItems"] = TitleNameItems;

                if (DSEditGetList.Tables[2].Rows.Count > 0)//Dynamic Binding Title Code DropDwonlist
                {
                    lstEditIndustryList = (from n in DSEditGetList.Tables[2].AsEnumerable()
                                           select new NameCode()
                                           {
                                               Name = n.Field<string>(DSEditGetList.Tables[2].Columns["IndustryName"].ToString()),
                                               Code = n.Field<Int32>(DSEditGetList.Tables[2].Columns["PK_IndID"].ToString())

                                           }).ToList();
                }
                IEnumerable<SelectListItem> IndustryNameItems;
                IndustryNameItems = new SelectList(lstEditIndustryList, "Code", "Name");
                ViewData["IndustryNameItems"] = IndustryNameItems;

                if (DSEditGetList.Tables[3].Rows.Count > 0)//Dynamic Binding Status Code DropDwonlist
                {
                    lstEditStatusList = (from n in DSEditGetList.Tables[3].AsEnumerable()
                                         select new NameCode()
                                         {
                                             Name = n.Field<string>(DSEditGetList.Tables[3].Columns["StatusName"].ToString()),
                                             Code = n.Field<Int32>(DSEditGetList.Tables[3].Columns["PK_StatusID"].ToString())

                                         }).ToList();
                }
                IEnumerable<SelectListItem> StatusNameItems;
                StatusNameItems = new SelectList(lstEditStatusList, "Code", "Name");
                ViewData["StatusNameItems"] = StatusNameItems;

                if (DSEditGetList.Tables[4].Rows.Count > 0)//Dynamic Binding Corporate  Sectore Code DropDwonlist
                {
                    lstEditCorporateList = (from n in DSEditGetList.Tables[4].AsEnumerable()
                                            select new NameCode()
                                            {
                                                Name = n.Field<string>(DSEditGetList.Tables[4].Columns["CorporateName"].ToString()),
                                                Code = n.Field<Int32>(DSEditGetList.Tables[4].Columns["PK_CorpID"].ToString())

                                            }).ToList();
                }
                IEnumerable<SelectListItem> CorporateNameItems;
                CorporateNameItems = new SelectList(lstEditCorporateList, "Code", "Name");
                ViewData["CorporateNameItems"] = CorporateNameItems;

                List<CompanyMaster> lstContact = new List<CompanyMaster>();

                if (DSEditCompany.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSEditCompany.Tables[1].Rows)
                    {
                        lstContact.Add(new CompanyMaster
                        {
                            PK_ContID = Convert.ToInt32(dr["PK_ContID"]),
                            ContactName = Convert.ToString(dr["ContactName"]),
                            CompanyName = Convert.ToString(dr["CompanyName"]),
                            TitleName = Convert.ToString(dr["TitleName"]),
                            HomePhone = Convert.ToString(dr["HomePhone"]),
                            Pan_No = Convert.ToString(dr["PanNumber"]),
                            Address = Convert.ToString(dr["Address"]),
                            Mobile = Convert.ToString(dr["Mobile"]),
                            Fax_No = Convert.ToString(dr["FaxNo"]),
                            IsMainContactString = Convert.ToString(dr["IsMainContact"]),
                            FK_CMP_ID = Convert.ToString(dr["FK_CMP_ID"]),
                            ContactStatus = Convert.ToString(dr["ContactStatus"])
                        });
                    }
                }
                ViewData["ContactList"] = lstContact;
                ObjModelCompany.ListDashboard = lstContact;
                return View(ObjModelCompany);
            }
            else
            {
                ViewBag.UserBranchId = 0;
                if (Session["UserBranchId"] != null)
                {
                    var FkBranchId = Convert.ToString(Session["UserBranchId"]);
                    ViewBag.UserBranchId = FkBranchId;
                }

                DataSet DSGetAllDdlList = new DataSet();
                List<NameCode> lstBranchList = new List<NameCode>();
                List<NameCode> lstTitleList = new List<NameCode>();
                List<NameCode> lstIndustryList = new List<NameCode>();
                List<NameCode> lstStatusList = new List<NameCode>();
                List<NameCode> lstCorporateList = new List<NameCode>();
                DSGetAllDdlList = objDalCompany.GetAllDdlLst();
                if (DSGetAllDdlList.Tables.Count > 0)
                {
                    if (DSGetAllDdlList.Tables[0].Rows.Count > 0)
                    {
                        if (DSGetAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                        {
                            lstBranchList = (from n in DSGetAllDdlList.Tables[0].AsEnumerable()
                                             select new NameCode()
                                             {
                                                 Name = n.Field<string>(DSGetAllDdlList.Tables[0].Columns["BranchName"].ToString()),
                                                 Code = n.Field<Int32>(DSGetAllDdlList.Tables[0].Columns["PK_ID"].ToString())

                                             }).ToList();
                        }
                        ViewBag.BranchName = lstBranchList;
                        if (DSGetAllDdlList.Tables[1].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
                        {
                            lstTitleList = (from n in DSGetAllDdlList.Tables[1].AsEnumerable()
                                            select new NameCode()
                                            {
                                                Name = n.Field<string>(DSGetAllDdlList.Tables[1].Columns["TitleName"].ToString()),
                                                Code = n.Field<Int32>(DSGetAllDdlList.Tables[1].Columns["PK_TitleID"].ToString())

                                            }).ToList();
                        }
                        ViewBag.TitleName = lstTitleList;
                        if (DSGetAllDdlList.Tables[2].Rows.Count > 0)//Dynamic Binding Industry DropDwonlist
                        {
                            lstIndustryList = (from n in DSGetAllDdlList.Tables[2].AsEnumerable()
                                               select new NameCode()
                                               {
                                                   Name = n.Field<string>(DSGetAllDdlList.Tables[2].Columns["IndustryName"].ToString()),
                                                   Code = n.Field<Int32>(DSGetAllDdlList.Tables[2].Columns["PK_IndID"].ToString())

                                               }).ToList();
                        }
                        ViewBag.IndustryName = lstIndustryList;

                        if (DSGetAllDdlList.Tables[3].Rows.Count > 0)//Dynamic Binding Status DropDwonlist
                        {
                            lstStatusList = (from n in DSGetAllDdlList.Tables[3].AsEnumerable()
                                             select new NameCode()
                                             {
                                                 Name = n.Field<string>(DSGetAllDdlList.Tables[3].Columns["StatusName"].ToString()),
                                                 Code = n.Field<Int32>(DSGetAllDdlList.Tables[3].Columns["PK_StatusID"].ToString())

                                             }).ToList();
                        }
                        ViewBag.StatusName = lstStatusList;
                        if (DSGetAllDdlList.Tables[4].Rows.Count > 0)//Dynamic Binding Corporate DropDwonlist
                        {
                            lstCorporateList = (from n in DSGetAllDdlList.Tables[4].AsEnumerable()
                                                select new NameCode()
                                                {
                                                    Name = n.Field<string>(DSGetAllDdlList.Tables[4].Columns["CorporateName"].ToString()),
                                                    Code = n.Field<Int32>(DSGetAllDdlList.Tables[4].Columns["PK_CorpID"].ToString())

                                                }).ToList();
                        }
                        ViewBag.CorporateName = lstCorporateList;
                    }

                }
                return View();
            }

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateCompany(CompanyMaster CM, HttpPostedFileBase Upload)
        {
            int CompanyId = 0;
            try
            {
                if (CM.CMP_ID != 0)
                {
                    CompanyId = objDalCompany.InsertUpdateCompany(CM);
                    if (CompanyId > 0)
                    {
                        TempData["UpdateCompany"] = CompanyId;
                        //return RedirectToAction("CompanyDashBoard", "CompanyMasters");
                        return RedirectToAction("CreateCompany", "CompanyMasters", new { @CompanyID = CompanyId });
                    }
                }
                else
                {
                    CompanyId = objDalCompany.InsertUpdateCompany(CM);
                    if (CompanyId > 0)
                    {

                        TempData["InsertCompany"] = CompanyId;
                        //return RedirectToAction("CompanyDashBoard", "CompanyMasters");
                        return RedirectToAction("CreateCompany", "CompanyMasters", new { @CompanyID = CompanyId });
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("CompanyDashBoard", "CompanyMasters");
        }
        public ActionResult DeleteCompany(int? CompanyID)
        {
            int Result = 0;
            try
            {
                Result = objDalCompany.DeleteCompany(CompanyID);
                if (Result != 0)
                {
                    TempData["DeleteCompany"] = Result;
                    return RedirectToAction("CompanyDashBoard", "CompanyMasters");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        [HttpGet]
        public ActionResult ContactDetails(int? CNT_ID, string CompanyName, int? CompId, string address)
        {
            #region bind status


            DSGetAllDdlList = objDalCompany.GetAllDdlLst();


            if (DSGetAllDdlList.Tables[3].Rows.Count > 0)//Dynamic Binding Status DropDwonlist
            {
                lstStatusList = (from n in DSGetAllDdlList.Tables[3].AsEnumerable()
                                 select new NameCode()
                                 {
                                     Name = n.Field<string>(DSGetAllDdlList.Tables[3].Columns["StatusName"].ToString()),
                                     Code = n.Field<Int32>(DSGetAllDdlList.Tables[3].Columns["PK_StatusID"].ToString())

                                 }).ToList();
            }
            ViewBag.StatusName = lstStatusList;
            #endregion
            ViewBag.ActionType = "False";
            if (!string.IsNullOrEmpty(CompanyName))
            {
                ViewBag.SelectedCompanyName = CompanyName;
                ViewBag.CompanyId = CompId;
                ViewBag.Address = address;
                ViewBag.ActionType = "True";
            }
            if (CNT_ID != 0 && CNT_ID != null)
            {

                DataTable DTEditContact = new DataTable();
                DTEditContact = objDalCompany.EditContact(CNT_ID);
                if (DTEditContact.Rows.Count > 0)
                {
                    ObjModelCompany.ContactName = Convert.ToString(DTEditContact.Rows[0]["ContactName"]);
                    ObjModelCompany.CompanyName = Convert.ToString(DTEditContact.Rows[0]["CompanyName"]);
                    ObjModelCompany.TitleName = Convert.ToString(DTEditContact.Rows[0]["Title"]);
                    ObjModelCompany.HomePhone = Convert.ToString(DTEditContact.Rows[0]["HomePhone"]);
                    ObjModelCompany.Pan_No = Convert.ToString(DTEditContact.Rows[0]["PanNumber"]);
                    ObjModelCompany.Mobile = Convert.ToString(DTEditContact.Rows[0]["Mobile"]);
                    ObjModelCompany.Fax_No = Convert.ToString(DTEditContact.Rows[0]["FaxNo"]);
                    ObjModelCompany.IsMainContact = Convert.ToBoolean(DTEditContact.Rows[0]["IsMainContact"]);
                    ObjModelCompany.Address = Convert.ToString(DTEditContact.Rows[0]["Address"]);
                    ObjModelCompany.PK_ContID = Convert.ToInt32(DTEditContact.Rows[0]["PK_ContID"]);
                    ObjModelCompany.ContactStatus = Convert.ToString(DTEditContact.Rows[0]["ContactStatus"]);
                    ObjModelCompany.Email = Convert.ToString(DTEditContact.Rows[0]["Email"]);
                    ObjModelCompany.FK_CMP_ID = Convert.ToString(DTEditContact.Rows[0]["FK_CMP_ID"]);
                }
                #region Bind Company Addr
                DataSet DsCompanyAddr = new DataSet();
                List<NameCode> lstCompanyAddr = new List<NameCode>();
                if (CompId > 0)
                {
                    ObjModelCompany.FK_CMP_ID = Convert.ToString(CompId);
                    DsCompanyAddr = objDalCompany.GetCompanyAddrForUpdate(ObjModelCompany.FK_CMP_ID);
                }
                else
                {
                    DsCompanyAddr = objDalCompany.GetCompanyAddrForUpdate(ObjModelCompany.FK_CMP_ID);
                }

                if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                {
                    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                          Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                      }).ToList();
                }

                IEnumerable<SelectListItem> ComAddrItems;
                ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

                //ViewBag.ComAddr = ComAddrItems;
                ViewData["ComAddr1"] = ComAddrItems;
                #endregion
                DataSet DSEditAllDdlList = new DataSet();
                List<NameCode> lstEditCompanyList = new List<NameCode>();
                List<NameCode> lstEditTitleList = new List<NameCode>();
                DSEditAllDdlList = objDalCompany.GetContactDdlList();
                if (DSEditAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                {
                    lstEditCompanyList = (from n in DSEditAllDdlList.Tables[0].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSEditAllDdlList.Tables[0].Columns["Company_Name"].ToString()),
                                              Code = n.Field<Int32>(DSEditAllDdlList.Tables[0].Columns["CMP_ID"].ToString())

                                          }).ToList();
                }
                IEnumerable<SelectListItem> CompanyNameItems;
                CompanyNameItems = new SelectList(lstEditCompanyList, "Code", "Name");
                ViewData["CompanyNameItems"] = CompanyNameItems;

                if (DSEditAllDdlList.Tables[1].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
                {
                    lstEditTitleList = (from n in DSEditAllDdlList.Tables[1].AsEnumerable()
                                        select new NameCode()
                                        {
                                            Name = n.Field<string>(DSEditAllDdlList.Tables[1].Columns["TitleName"].ToString()),
                                            Code = n.Field<Int32>(DSEditAllDdlList.Tables[1].Columns["PK_TitleID"].ToString())

                                        }).ToList();
                }
                IEnumerable<SelectListItem> TitleNameItems;
                TitleNameItems = new SelectList(lstEditTitleList, "Code", "Name");
                ViewData["TitleNameItems"] = TitleNameItems;

                return View(ObjModelCompany);
            }
            else
            {

                #region Bind Company Addr
                DataSet DsCompanyAddr = new DataSet();
                List<NameCode> lstCompanyAddr = new List<NameCode>();
                if (CompId > 0)
                {
                    ObjModelCompany.FK_CMP_ID = Convert.ToString(CompId);
                    DsCompanyAddr = objDalCompany.GetCompanyAddrForUpdate(ObjModelCompany.FK_CMP_ID);
                }
                else
                {
                    DsCompanyAddr = objDalCompany.GetCompanyAddrForUpdate(ObjModelCompany.FK_CMP_ID);
                }

                if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                {
                    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                          Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                      }).ToList();
                }

                IEnumerable<SelectListItem> ComAddrItems;
                ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

                //ViewBag.ComAddr = ComAddrItems;
                ViewData["ComAddr1"] = ComAddrItems;
                #endregion
                DataSet DSGetAllDdlList = new DataSet();
                List<NameCode> lstCompanyList = new List<NameCode>();
                List<NameCode> lstTitleList = new List<NameCode>();
                DSGetAllDdlList = objDalCompany.GetContactDdlList();
                if (DSGetAllDdlList.Tables[0].Rows.Count > 0)
                {
                    if (DSGetAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    {
                        lstCompanyList = (from n in DSGetAllDdlList.Tables[0].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetAllDdlList.Tables[0].Columns["Company_Name"].ToString()),
                                              Code = n.Field<Int32>(DSGetAllDdlList.Tables[0].Columns["CMP_ID"].ToString()),



                                          }).ToList();
                    }
                    ViewBag.CompanyName = lstCompanyList;
                    if (DSGetAllDdlList.Tables[1].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
                    {
                        lstTitleList = (from n in DSGetAllDdlList.Tables[1].AsEnumerable()
                                        select new NameCode()
                                        {
                                            Name = n.Field<string>(DSGetAllDdlList.Tables[1].Columns["TitleName"].ToString()),
                                            Code = n.Field<Int32>(DSGetAllDdlList.Tables[1].Columns["PK_TitleID"].ToString())

                                        }).ToList();
                    }
                    ViewBag.TitleName = lstTitleList;
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult ContactDetails(CompanyMaster CPM, FormCollection FC)
        {
            string Result = string.Empty;
            string CompanyId = string.Empty;
            int CompId = 0;
            string CompanyName = FC["hidCompany"];
            var ABC = FC["ActionType"];
            string ActionType = Convert.ToString(FC["ActionType"]);
            if (ActionType == "True")
            {
                CPM.FK_CMP_ID = Convert.ToString(FC["CompId"]);
            }
            try
            {
                if (CPM.PK_ContID != 0)
                {
                    CompanyId = objDalCompany.InsertUpdateContact(CPM, CompanyName);
                    if (CompanyId != "")
                    {
                        CompId = Convert.ToInt32(CompanyId);
                    }

                    if (Result != "" && Result != null)
                    {
                        TempData["UpdateContact"] = Result;
                        if (!string.IsNullOrEmpty(CPM.FK_CMP_ID))
                        {
                            int ConvertedCompId = Convert.ToInt32(CPM.FK_CMP_ID);
                            return RedirectToAction("CreateCompany", new { CompanyID = ConvertedCompId });
                        }
                    }
                    //return RedirectToAction("CreateCompany", new { CompanyID = CPM.FK_CMP_ID });
                    return RedirectToAction("ContactDashBoard", "CompanyMasters");
                }

                else
                {
                    DataTable dtContactDetailExist = new DataTable();

                    dtContactDetailExist = objDalCompany.ChkContactDetailExist(CPM, CompanyName);

                    if (dtContactDetailExist.Rows.Count > 0)
                    {
                        TempData["ContactExist"] = "ABC";
                        return RedirectToAction("ContactDetails", "CompanyMasters");
                    }
                    else
                    {
                        CompanyId = objDalCompany.InsertUpdateContact(CPM, CompanyName);
                        CompId = Convert.ToInt32(CompanyId);
                        if (Result != null && Result != "")
                        {

                            TempData["InsertContact"] = Result;
                            if (!string.IsNullOrEmpty(CPM.FK_CMP_ID))
                            {
                                int ConvertedCompId = Convert.ToInt32(CPM.FK_CMP_ID);
                                return RedirectToAction("CreateCompany", new { CompanyID = ConvertedCompId });
                            }

                        }
                        //return RedirectToAction("CreateCompany", new { CompanyID = CompId });
                        return RedirectToAction("ContactDashBoard", "CompanyMasters");
                    }


                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("CreateCompany", new { CompanyID = CPM.FK_CMP_ID });
        }
        public ActionResult DeleteContact(int? CNT_ID)
        {
            int Result = 0;
            try
            {
                Result = objDalCompany.DeleteContact(CNT_ID);
                if (Result != 0)
                {
                    TempData["DeleteContact"] = Result;
                    return RedirectToAction("ContactDashBoard", "CompanyMasters");
                }
            }
            catch (Exception ex)
            {
                string Errore = ex.Message.ToString();
            }
            return View();
        }
        public ActionResult ContactDashBoard()
        {
            DataTable DTGetContact = new DataTable();
            List<CompanyMaster> lstContact = new List<CompanyMaster>();
            DTGetContact = objDalCompany.GetContactDashBoard();

            if (DTGetContact.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetContact.Rows)
                {
                    lstContact.Add(new CompanyMaster
                    {
                        PK_ContID = Convert.ToInt32(dr["PK_ContID"]),
                        ContactName = Convert.ToString(dr["ContactName"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        TitleName = Convert.ToString(dr["TitleName"]),
                        HomePhone = Convert.ToString(dr["HomePhone"]),
                        Pan_No = Convert.ToString(dr["PanNumber"]),
                        Address = Convert.ToString(dr["Address"]),
                        Mobile = Convert.ToString(dr["Mobile"]),
                        Fax_No = Convert.ToString(dr["FaxNo"]),
                        PrimaryContact = Convert.ToString(dr["IsMainContact"]),
                        FK_CMP_ID = Convert.ToString(dr["FK_CMP_ID"]),
                        BranchName = Convert.ToString(dr["Branch_Name"]),
                        ContactStatus = Convert.ToString(dr["ContactStatus"]),
                        Email = Convert.ToString(dr["Email"])
                    });
                }
            }
            ViewData["ContactList"] = lstContact;
            ObjModelCompany.ContactDashBoard = lstContact;
            return View(ObjModelCompany);
        }
        [HttpGet]
        public JsonResult GetAddressOfCompany(string Company_Name)
        {

            #region Bind Company Addr
            DataSet DsCompanyAddr = new DataSet();
            List<NameCode> lstCompanyAddr = new List<NameCode>();
            DsCompanyAddr = objDalCompany.GetCompanyAddrVendor(Company_Name);
            if (DsCompanyAddr.Tables[0].Rows.Count > 0)
            {
                lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                      Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                  }).ToList();
            }

            IEnumerable<SelectListItem> ComAddrItems;
            ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

            //ViewBag.ComAddr = ComAddrItems;
            ViewData["ComAddr1"] = ComAddrItems;
            #endregion

            var address = objDalCompany.GetAddressByName(Company_Name);
            return Json(lstCompanyAddr, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetCompanyAddress(string Prefix)
        {
            DataSet DsCompanyAddr = new DataSet();
            List<NameCode> lstCompanyAddr = new List<NameCode>();
            string CompAddress = string.Empty;
            if (Prefix != null && Prefix != "")
            {
                DsCompanyAddr = objDalCompany.GetCompanyAddr(Prefix);
                if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                {
                    /// CompAddress = DTResult.Rows[0]["Address"].ToString();
                    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                          Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                      }).ToList();
                    return Json(lstCompanyAddr, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("failure", JsonRequestBehavior.AllowGet);

        }


        public JsonResult DuplicateCompanyName(string companyname)//Checking Existing User Name
        {
            string Result = string.Empty;
            DataTable DTExistRoleName = new DataTable();
            try
            {
                DTExistRoleName = objDalCompany.DuplicateCompany(companyname);
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

        #region Export to Exel contacts

        [HttpGet]
        public ActionResult ExportIndex()
        {

            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CompanyMaster> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<CompanyMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd/MM/yyyy" + '-' + "HH:mm:ss");

                string filename = "ContactDashBoard-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<CompanyMaster> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CompanyMaster> grid = new Grid<CompanyMaster>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.FK_CMP_ID).Titled("Company Id");
            grid.Columns.Add(model => model.ContactName).Titled("Contact Name");
            grid.Columns.Add(model => model.CompanyName).Titled("Company Name");
            grid.Columns.Add(model => model.Mobile).Titled("Mobile Number");
            grid.Columns.Add(model => model.Address).Titled("Office Address");
            grid.Columns.Add(model => model.Email).Titled("Email Id");
            grid.Columns.Add(model => model.PrimaryContact).Titled("Is it primary contact(Yes/No)");
            grid.Columns.Add(model => model.ContactStatus).Titled("Status");
            grid.Columns.Add(model => model.BranchName).Titled("Branch");
            grid.Columns.Add(model => model.TitleName).Titled("Designation");





            grid.Pager = new GridPager<CompanyMaster>(grid);
            grid.Processors.Add(grid.Pager);

            //grid.Pager.RowsPerPage = 6;
            grid.Pager.RowsPerPage = ObjModelCompany.ContactDashBoard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<CompanyMaster> GetData()
        {

            DataTable DTGetContact = new DataTable();
            List<CompanyMaster> lstContact = new List<CompanyMaster>();
            DTGetContact = objDalCompany.GetContactDashBoard();

            if (DTGetContact.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetContact.Rows)
                {
                    lstContact.Add(new CompanyMaster
                    {
                        PK_ContID = Convert.ToInt32(dr["PK_ContID"]),
                        ContactName = Convert.ToString(dr["ContactName"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        TitleName = Convert.ToString(dr["TitleName"]),
                        HomePhone = Convert.ToString(dr["HomePhone"]),
                        Pan_No = Convert.ToString(dr["PanNumber"]),
                        Address = Convert.ToString(dr["Address"]),
                        Mobile = Convert.ToString(dr["Mobile"]),
                        Fax_No = Convert.ToString(dr["FaxNo"]),
                        PrimaryContact = Convert.ToString(dr["IsMainContact"]),
                        FK_CMP_ID = Convert.ToString(dr["FK_CMP_ID"]),
                        BranchName = Convert.ToString(dr["Branch_Name"]),
                        Email = Convert.ToString(dr["Email"]),
                        ContactStatus = Convert.ToString(dr["ContactStatus"])
                    });
                }
            }
            ViewData["CompanyList"] = lstContact;
            ObjModelCompany.ContactDashBoard = lstContact;
            return ObjModelCompany.ContactDashBoard;

        }

        #endregion


        #region Export to Exel Companies
        public ActionResult ExportIndex1()
        {

            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CompanyMaster> grid = CreateExportableGrid1();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<CompanyMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "CompanyManagement-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<CompanyMaster> CreateExportableGrid1()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CompanyMaster> grid = new Grid<CompanyMaster>(GetData1());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.CMP_ID).Titled("Company (ID)");

            grid.Columns.Add(model => model.Company_Name).Titled("Company Name");


            grid.Columns.Add(model => model.Address_Account).Titled("Company Address");
            grid.Columns.Add(model => model.AddressType).Titled("Address Type");


            grid.Columns.Add(model => model.Work_Phone).Titled("Landline Number");

            grid.Columns.Add(model => model.Email).Titled("Email id");


            //grid.Columns.Add(model => model.Pan_No).Titled("Pan No.");
            //grid.Columns.Add(model => model.Contact).Titled("Contact");
            //grid.Columns.Add(model => model.Mobile).Titled("Mobile");
            grid.Columns.Add(model => model.CorporateName).Titled("Corporate Group");
            grid.Columns.Add(model => model.IndustryName).Titled("Industry Type");
            grid.Columns.Add(model => model.BranchName).Titled("Branch Name");
            grid.Columns.Add(model => model.Pan_No).Titled("PAN Card Number");
            grid.Columns.Add(model => model.Website).Titled("Website Address");



            grid.Pager = new GridPager<CompanyMaster>(grid);
            grid.Processors.Add(grid.Pager);

            grid.Pager.RowsPerPage = ObjModelCompany.lstCompanyMaster1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<CompanyMaster> GetData1()
        {

            DataTable DTCompanyDashBoard = new DataTable();
            List<CompanyMaster> lstCompanyDashBoard = new List<CompanyMaster>();
            DTCompanyDashBoard = objDalCompany.GetCompanyDashBoard();
            try
            {
                if (DTCompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CompanyMaster
                            {
                                CMP_ID = Convert.ToInt32(dr["CMP_ID"]),
                                Company_Name = Convert.ToString(dr["CompanyName"]),
                                BranchName = Convert.ToString(dr["BranchName"]),
                                Email = Convert.ToString(dr["Email"]),
                                TitleName = Convert.ToString(dr["Title"]),
                                // Mobile = Convert.ToString(dr["Mobile"]),
                                Status = Convert.ToString(dr["StatusName"]),
                                CorporateName = Convert.ToString(dr["CorporateName"]),
                                Address = Convert.ToString(dr["Address"]),
                                Pan_No = Convert.ToString(dr["Pan_No"]),
                                //Address_Account = Convert.ToString(dr["SiteAddress"]),
                                IndustryName = Convert.ToString(dr["IndustryName"]),
                                Website = Convert.ToString(dr["Website"]),
                                Work_Phone = Convert.ToString(dr["Landline"]),
                                Address_Account = Convert.ToString(dr["SiteAddress"]),
                                AddressType = Convert.ToString(dr["Type"]),
                            }
                            );
                    }


                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CompanyList"] = lstCompanyDashBoard;
            ObjModelCompany.lstCompanyMaster1 = lstCompanyDashBoard;
            return ObjModelCompany.lstCompanyMaster1;

        }
        #endregion

        [HttpPost]
        public JsonResult InsertSiteAddress(int CMP_ID, string SiteAdd, string SiteAddrPin, string AddressType)
        {
            string Results = string.Empty;
            int intUserID = 0;



            string Result = string.Empty;
            int RetValue = 0;



            try
            {
                RetValue = objDalCompany.InsertUpdateSiteAddress(CMP_ID, SiteAdd, SiteAddrPin, AddressType);

                if (RetValue != 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("ERROR", JsonRequestBehavior.AllowGet);
                }


                #region Bind Addr
                DataTable dtAttach = new DataTable();
                List<CompanyMaster> lstDTPAN = new List<CompanyMaster>();

                dtAttach = objDalCompany.GetSiteAdressDet(CMP_ID);

                if (dtAttach.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAttach.Rows)
                    {
                        lstDTPAN.Add(
                            new CompanyMaster
                            {
                                AddressType = Convert.ToString(dr["AddressType"]),
                                Address_Account = Convert.ToString(dr["SiteAddress"]),
                                SiteAddrPin = Convert.ToString(dr["SitePIN"]),
                                SiteID = Convert.ToString(dr["PKId"])
                            }
                            );
                    }

                    ObjModelCompany.lstSiteAddress = lstDTPAN;
                    ViewData["lstSiteAddress"] = lstDTPAN;
                }
                #endregion



                return Json("SUCCESS", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult DeleteSiteAddress(string id)
        {
            string Results = string.Empty;
            FileDetails fileDetails = new FileDetails();
            DataTable DTGetDeleteFile = new DataTable();
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {

                if (id != null && id != "")
                {
                    Results = objDalCompany.DeleteSiteAddress(id);

                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            return Json(new { Result = "ERROR" });
        }


        //Added By Satish Pawar On 11 May 2023
        [HttpGet]
        public JsonResult GetEmailIdOfCompany(string Company_Name)
        {

            #region Bind Company Addr
            DataSet DsCompanyEmail = new DataSet();
            List<EmailCode> lstCompanyEmail = new List<EmailCode>();
            DsCompanyEmail = objDalCompany.GetCompanyEmailId(Company_Name);
            string jsonString = "";
            if (DsCompanyEmail.Tables[0].Rows.Count > 0)
            {
                jsonString = JsonConvert.SerializeObject(DsCompanyEmail, Formatting.Indented);


            }

            IEnumerable<SelectListItem> ComEmailItems;
            ComEmailItems = new SelectList(lstCompanyEmail, "Code", "Name");

            //ViewBag.ComAddr = ComAddrItems;
            ViewData["ComEmail"] = ComEmailItems;
            #endregion

            //var Email = objDalCompany.GetAddressByName(Company_Name);
            return Json(jsonString, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAddressOfCompanyVendor(string Company_Name)
        {

            #region Bind Company Addr
            DataSet DsCompanyAddr = new DataSet();
            List<NameCode> lstCompanyAddr = new List<NameCode>();
            DsCompanyAddr = objDalCompany.GetCompanyAddrVendor_(Company_Name);
            if (DsCompanyAddr.Tables[0].Rows.Count > 0)
            {
                lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                      Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                  }).ToList();
            }

            IEnumerable<SelectListItem> ComAddrItems;
            ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

            //ViewBag.ComAddr = ComAddrItems;
            ViewData["ComAddr1"] = ComAddrItems;
            #endregion

            var address = objDalCompany.GetAddressByName(Company_Name);
            return Json(lstCompanyAddr, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAddressOfCompanyVendor_(string Company_Name)
        {

            #region Bind Company Addr
            DataSet DsCompanyAddr = new DataSet();
            List<NameCode> lstCompanyAddr = new List<NameCode>();
            DsCompanyAddr = objDalCompany.GetCompanyAddrVendor_(Company_Name);
            if (DsCompanyAddr.Tables[0].Rows.Count > 0)
            {
                lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                      Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                  }).ToList();
            }

            IEnumerable<SelectListItem> ComAddrItems;
            ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

            //ViewBag.ComAddr = ComAddrItems;
            ViewData["ComAddr1"] = ComAddrItems;
            #endregion

            var address = objDalCompany.GetAddressByName(Company_Name);
            return Json(lstCompanyAddr, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SapCustomerContactDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SapContactDashBoard(string CMP_id)
        {
            DataTable DTGetContact = new DataTable();
            List<CompanyMaster> lstContact = new List<CompanyMaster>();
            DTGetContact = objDalCompany.GetSapContactDashBoard(CMP_id);

            if (DTGetContact.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetContact.Rows)
                {
                    lstContact.Add(new CompanyMaster
                    {
                        PK_ContID = Convert.ToInt32(dr["PK_ContID"]),
                        ContactName = Convert.ToString(dr["ContactName"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        TitleName = Convert.ToString(dr["TitleName"]),
                        HomePhone = Convert.ToString(dr["HomePhone"]),
                        Pan_No = Convert.ToString(dr["PanNumber"]),
                        Address = Convert.ToString(dr["Address"]),
                        Mobile = Convert.ToString(dr["Mobile"]),
                        Fax_No = Convert.ToString(dr["FaxNo"]),
                        PrimaryContact = Convert.ToString(dr["IsMainContact"]),
                        FK_CMP_ID = Convert.ToString(dr["FK_CMP_ID"]),
                        BranchName = Convert.ToString(dr["Branch_Name"]),
                        ContactStatus = Convert.ToString(dr["ContactStatus"]),
                        Email = Convert.ToString(dr["Email"])
                    });
                }
            }
            ViewData["ContactList"] = lstContact;
            ObjModelCompany.ContactDashBoard = lstContact;
            return View(ObjModelCompany);
        }


        [HttpGet]
        public ActionResult SapContactDetails(int? CNT_ID, string CompanyName, int? CompId, string address, string cmpId)
        {
            #region bind status


            DSGetAllDdlList = objDalCompany.GetAllDdlLst();


            if (DSGetAllDdlList.Tables[3].Rows.Count > 0)//Dynamic Binding Status DropDwonlist
            {
                lstStatusList = (from n in DSGetAllDdlList.Tables[3].AsEnumerable()
                                 select new NameCode()
                                 {
                                     Name = n.Field<string>(DSGetAllDdlList.Tables[3].Columns["StatusName"].ToString()),
                                     Code = n.Field<Int32>(DSGetAllDdlList.Tables[3].Columns["PK_StatusID"].ToString())

                                 }).ToList();
            }
            ViewBag.StatusName = lstStatusList;
            #endregion
            ViewBag.ActionType = "False";
            if (!string.IsNullOrEmpty(CompanyName))
            {
                ViewBag.SelectedCompanyName = CompanyName;
                ViewBag.CompanyId = CompId;
                ViewBag.Address = address;
                ViewBag.ActionType = "True";
            }
            if (cmpId != null && cmpId != "")
            {
                DataTable DTEditContact = new DataTable();
                DTEditContact = objDalCompany.GetcompanyContact(cmpId);
                if (DTEditContact.Rows.Count > 0)
                {
                    ObjModelCompany.CompanyName = Convert.ToString(DTEditContact.Rows[0]["CompanyName"]);
                    ObjModelCompany.FK_CMP_ID = Convert.ToString(DTEditContact.Rows[0]["CMP_ID"]);
                    ObjModelCompany.SapNo = Convert.ToString(DTEditContact.Rows[0]["ClientCode"]);
                }
            }

            if (CNT_ID != 0 && CNT_ID != null)
            {

                DataTable DTEditContact = new DataTable();
                DTEditContact = objDalCompany.EditContact(CNT_ID);
                if (DTEditContact.Rows.Count > 0)
                {
                    ObjModelCompany.ContactName = Convert.ToString(DTEditContact.Rows[0]["ContactName"]);
                    ObjModelCompany.CompanyName = Convert.ToString(DTEditContact.Rows[0]["CompanyName"]);
                    ObjModelCompany.TitleName = Convert.ToString(DTEditContact.Rows[0]["Title"]);
                    ObjModelCompany.HomePhone = Convert.ToString(DTEditContact.Rows[0]["HomePhone"]);
                    ObjModelCompany.Pan_No = Convert.ToString(DTEditContact.Rows[0]["PanNumber"]);
                    ObjModelCompany.Mobile = Convert.ToString(DTEditContact.Rows[0]["Mobile"]);
                    ObjModelCompany.Fax_No = Convert.ToString(DTEditContact.Rows[0]["FaxNo"]);
                    ObjModelCompany.IsMainContact = Convert.ToBoolean(DTEditContact.Rows[0]["IsMainContact"]);
                    ObjModelCompany.Address = Convert.ToString(DTEditContact.Rows[0]["Address"]);
                    ObjModelCompany.PK_ContID = Convert.ToInt32(DTEditContact.Rows[0]["PK_ContID"]);
                    ObjModelCompany.ContactStatus = Convert.ToString(DTEditContact.Rows[0]["ContactStatus"]);
                    ObjModelCompany.Email = Convert.ToString(DTEditContact.Rows[0]["Email"]);
                    ObjModelCompany.FK_CMP_ID = Convert.ToString(DTEditContact.Rows[0]["FK_CMP_ID"]);
                    ObjModelCompany.SapNo = Convert.ToString(DTEditContact.Rows[0]["SalesOrderNo"]);
                }
                #region Bind Company Addr
                DataSet DsCompanyAddr = new DataSet();
                List<NameCode> lstCompanyAddr = new List<NameCode>();
                if (CompId > 0)
                {
                    ObjModelCompany.FK_CMP_ID = Convert.ToString(CompId);
                    DsCompanyAddr = objDalCompany.GetCompanyAddrForUpdate(ObjModelCompany.FK_CMP_ID);
                }
                else
                {
                    DsCompanyAddr = objDalCompany.GetCompanyAddrForUpdate(ObjModelCompany.FK_CMP_ID);
                }

                if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                {
                    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                          Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                      }).ToList();
                }

                IEnumerable<SelectListItem> ComAddrItems;
                ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

                //ViewBag.ComAddr = ComAddrItems;
                ViewData["ComAddr1"] = ComAddrItems;
                #endregion
                DataSet DSEditAllDdlList = new DataSet();
                List<NameCode> lstEditCompanyList = new List<NameCode>();
                List<NameCode> lstEditTitleList = new List<NameCode>();
                DSEditAllDdlList = objDalCompany.GetContactDdlList();
                if (DSEditAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                {
                    lstEditCompanyList = (from n in DSEditAllDdlList.Tables[0].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSEditAllDdlList.Tables[0].Columns["Company_Name"].ToString()),
                                              Code = n.Field<Int32>(DSEditAllDdlList.Tables[0].Columns["CMP_ID"].ToString())

                                          }).ToList();
                }
                IEnumerable<SelectListItem> CompanyNameItems;
                CompanyNameItems = new SelectList(lstEditCompanyList, "Code", "Name");
                ViewData["CompanyNameItems"] = CompanyNameItems;

                if (DSEditAllDdlList.Tables[1].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
                {
                    lstEditTitleList = (from n in DSEditAllDdlList.Tables[1].AsEnumerable()
                                        select new NameCode()
                                        {
                                            Name = n.Field<string>(DSEditAllDdlList.Tables[1].Columns["TitleName"].ToString()),
                                            Code = n.Field<Int32>(DSEditAllDdlList.Tables[1].Columns["PK_TitleID"].ToString())

                                        }).ToList();
                }
                IEnumerable<SelectListItem> TitleNameItems;
                TitleNameItems = new SelectList(lstEditTitleList, "Code", "Name");
                ViewData["TitleNameItems"] = TitleNameItems;

                return View(ObjModelCompany);
            }
            else
            {

                #region Bind Company Addr
                DataSet DsCompanyAddr = new DataSet();
                List<NameCode> lstCompanyAddr = new List<NameCode>();
                if (CompId > 0)
                {
                    ObjModelCompany.FK_CMP_ID = Convert.ToString(CompId);
                    DsCompanyAddr = objDalCompany.GetCompanyAddrForUpdate(ObjModelCompany.FK_CMP_ID);
                }
                else
                {
                    DsCompanyAddr = objDalCompany.GetCompanyAddrForUpdate(ObjModelCompany.FK_CMP_ID);
                }

                if (DsCompanyAddr.Tables[0].Rows.Count > 0)
                {
                    lstCompanyAddr = (from n in DsCompanyAddr.Tables[0].AsEnumerable()
                                      select new NameCode()
                                      {
                                          Name = n.Field<string>(DsCompanyAddr.Tables[0].Columns["ComAddr"].ToString()),
                                          Code = n.Field<Int32>(DsCompanyAddr.Tables[0].Columns["Id"].ToString())

                                      }).ToList();
                }

                IEnumerable<SelectListItem> ComAddrItems;
                ComAddrItems = new SelectList(lstCompanyAddr, "Code", "Name");

                //ViewBag.ComAddr = ComAddrItems;
                ViewData["ComAddr1"] = ComAddrItems;
                #endregion
                DataSet DSGetAllDdlList = new DataSet();
                List<NameCode> lstCompanyList = new List<NameCode>();
                List<NameCode> lstTitleList = new List<NameCode>();
                DSGetAllDdlList = objDalCompany.GetContactDdlList();
                if (DSGetAllDdlList.Tables[0].Rows.Count > 0)
                {
                    if (DSGetAllDdlList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    {
                        lstCompanyList = (from n in DSGetAllDdlList.Tables[0].AsEnumerable()
                                          select new NameCode()
                                          {
                                              Name = n.Field<string>(DSGetAllDdlList.Tables[0].Columns["Company_Name"].ToString()),
                                              Code = n.Field<Int32>(DSGetAllDdlList.Tables[0].Columns["CMP_ID"].ToString()),



                                          }).ToList();
                    }
                    ViewBag.CompanyName = lstCompanyList;
                    if (DSGetAllDdlList.Tables[1].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
                    {
                        lstTitleList = (from n in DSGetAllDdlList.Tables[1].AsEnumerable()
                                        select new NameCode()
                                        {
                                            Name = n.Field<string>(DSGetAllDdlList.Tables[1].Columns["TitleName"].ToString()),
                                            Code = n.Field<Int32>(DSGetAllDdlList.Tables[1].Columns["PK_TitleID"].ToString())

                                        }).ToList();
                    }
                    ViewBag.TitleName = lstTitleList;
                }
            }
            return View(ObjModelCompany);
        }
        [HttpPost]
        public ActionResult SapContactDetails(CompanyMaster CPM, FormCollection FC)
        {
            string Result = string.Empty;
            string CompanyId = string.Empty;
            int CompId = 0;
            string CompanyName = FC["hidCompany"];
            var ABC = FC["ActionType"];
            string ActionType = Convert.ToString(FC["ActionType"]);
            if (ActionType == "True")
            {
                CPM.FK_CMP_ID = Convert.ToString(FC["CompId"]);
            }
            try
            {
                if (CPM.PK_ContID != 0)
                {
                    CompanyId = objDalCompany.InsertUpdateContact(CPM, CompanyName);
                    if (CompanyId != "")
                    {
                        CompId = Convert.ToInt32(CompanyId);
                    }

                    if (Result != "" && Result != null)
                    {
                        TempData["UpdateContact"] = Result;
                        if (!string.IsNullOrEmpty(CPM.FK_CMP_ID))
                        {
                            int ConvertedCompId = Convert.ToInt32(CPM.FK_CMP_ID);
                            return RedirectToAction("CreateCompany", new { CompanyID = ConvertedCompId });

                        }
                    }
                    //return RedirectToAction("CreateCompany", new { CompanyID = CPM.FK_CMP_ID });
                    return RedirectToAction("SapContactDashBoard", "CompanyMasters", new { CMP_id = CPM.SapNo });
                }

                else
                {
                    DataTable dtContactDetailExist = new DataTable();

                    dtContactDetailExist = objDalCompany.ChkContactDetailExist(CPM, CompanyName);

                    if (dtContactDetailExist.Rows.Count > 0)
                    {
                        TempData["ContactExist"] = "ABC";
                        return RedirectToAction("SapContactDetails", "CompanyMasters");
                    }
                    else
                    {
                        CompanyId = objDalCompany.InsertUpdateContact(CPM, CompanyName);
                        CompId = Convert.ToInt32(CompanyId);
                        if (Result != null && Result != "")
                        {

                            TempData["InsertContact"] = Result;
                            if (!string.IsNullOrEmpty(CPM.FK_CMP_ID))
                            {
                                int ConvertedCompId = Convert.ToInt32(CPM.FK_CMP_ID);
                                return RedirectToAction("CreateCompany", new { CompanyID = ConvertedCompId });
                            }
                        }
                        //return RedirectToAction("CreateCompany", new { CompanyID = CompId });
                        return RedirectToAction("SapContactDashBoard", "CompanyMasters", new { CMP_id = CPM.SapNo });
                    }


                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("CreateCompany", new { CompanyID = CPM.FK_CMP_ID });
        }


        public ActionResult InactiveCustomer()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetSearchCompanyData(string CompanyName)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (!string.IsNullOrEmpty(CompanyName))
            {
                DTResult = objDalCompany.GetSearchCompanyName(CompanyName);
                if (DTResult.Rows.Count > 0)
                {
                    //foreach (DataRow dr in DTResult.Rows)
                    //{
                    //    lstAutoComplete.Add(
                    //       new EnquiryMaster
                    //       {

                    //           CustomerSearch = Convert.ToString(dr["Company_Name"]),
                    //       }
                    //    );
                    //}
                    var Response = JsonConvert.SerializeObject(DTResult);
                    return Json(Response, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetLeadByName(string Prefix)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();
            if (Prefix != null && Prefix != "")
            {
                DTResult = objDalCompany.GetSearchCompanyName(Prefix);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new EnquiryMaster
                           {
                               CompanyName = Convert.ToString(dr["CompanyName"]),

                           }
                         );
                    }
                    Session["CompanyNames"] = Convert.ToString(DTResult.Rows[0]["CompanyNames"]);
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCustomer(string sapcustomer)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (!string.IsNullOrEmpty(sapcustomer))
            {
                DTResult = objDalCompany.UpdateCustomer(sapcustomer);
            }
            var Response = JsonConvert.SerializeObject(DTResult);
            return Json(Response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetCompanyData()
        {
            DataTable DTResult = new DataTable();
                DTResult = objDalCompany.GetCompanyData();
            var Response = JsonConvert.SerializeObject(DTResult);
            return Json(Response, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetCustomerJobData(string CompanyName)
        {
            DataTable DTResult = new DataTable();
            List<EnquiryMaster> lstAutoComplete = new List<EnquiryMaster>();

            if (!string.IsNullOrEmpty(CompanyName))
            {
                DTResult = objDalCompany.GetCustomerJobData(CompanyName);
            }
            var Response = JsonConvert.SerializeObject(DTResult);
            return Json(Response, JsonRequestBehavior.AllowGet);

        }
    }
}