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
    public class QuotationServicesLinkController : Controller
    {
        QuotationServices objModel = new QuotationServices();
        DALQuotationServices objDAL = new DALQuotationServices();

        #region Services
        // GET: QuotationServicesLink
        public ActionResult QuotationServicesLink(QuotationServices objModel, int? PKServiceId)
        {
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ImageDataById = new DataSet();
            List<QuotationServices> lstCompanyDashBoard = new List<QuotationServices>();



            if (PKServiceId != null)
            {
                ImageDataById = objDAL.GetImageById(objModel.PKServiceId);
                if (ImageDataById.Tables[0].Rows.Count > 0)
                {
                    objModel.ServiceName = Convert.ToString(ImageDataById.Tables[0].Rows[0]["ServiceName"]);
                    objModel.ServiceImage = Convert.ToString(ImageDataById.Tables[0].Rows[0]["ServiceImage"]);
                    objModel.PKServiceId = Convert.ToInt32(ImageDataById.Tables[0].Rows[0]["PKServiceId"]);


                    // CostSheetDashBoard = objDAL.GetReportImageByCall_Id(objModel.PK_CALL_ID);
                    //if (CostSheetDashBoard.Rows.Count > 0)
                    //{
                    foreach (DataRow dr in ImageDataById.Tables[0].Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new QuotationServices
                            {
                                PKServiceId = Convert.ToInt16(dr["PKServiceId"]),
                                ServiceImage = Convert.ToString(dr["ServiceImage"]),
                                ServiceName = Convert.ToString(dr["ServiceName"])

                            }
                            );
                    }
                    //}

                }

            }
            else
            {
                try
                {
                    ImageDataById = objDAL.GetAllServicess();
                    if (ImageDataById.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ImageDataById.Tables[0].Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new QuotationServices
                                {
                                    PKServiceId = Convert.ToInt16(dr["PKServiceId"]),
                                    ServiceImage = Convert.ToString(dr["ServiceImage"]),
                                    ServiceName = Convert.ToString(dr["ServiceName"])
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(objModel);


        }

        [HttpPost]
        public ActionResult QuotationServicesLink(QuotationServices IVR, FormCollection fc, HttpPostedFileBase File)
        {
            string Result = string.Empty;
            try
            {
                if (IVR.PKServiceId == 0)
                {
                    HttpPostedFileBase Imagesection;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        Imagesection = Request.Files["img_Banner"];
                        if (Imagesection != null && Imagesection.FileName != "")
                        {
                            IVR.ServiceImage = CommonControl.FileUpload("QuotationHtml/QuotationServices/", Imagesection);
                            //IVR.Image = CommonControl.FileUploadCompress("Content/Uploads/Images/", Imagesection, IVR.PK_IP_Id, IVR.PK_CALL_ID.ToString());
                        }
                        else
                        {
                            if (Imagesection.FileName != "")
                            {
                                IVR.ServiceImage = "NoImage.gif";
                            }
                        }
                    }
                    //IVR.Type = "IVR";
                    IVR.Status = "1";
                    Result = objDAL.InsertUpdateServiceImage(IVR);
                    if (Result != "" && Result != null)
                    {
                        ModelState.Clear();
                        TempData["InsertCompany"] = Result;
                    }
                    // return RedirectToAction("QuotationServicesLink", IVR);
                    return RedirectToAction("QuotationServicesLink");
                }
                else
                {
                    HttpPostedFileBase Imagesection;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        Imagesection = Request.Files["img_Banner"];
                        if (Imagesection != null && Imagesection.FileName != "")
                        {
                            IVR.ServiceImage = CommonControl.FileUpload("QuotationHtml/QuotationServices/", Imagesection);
                            // IVR.Image = CommonControl.FileUploadCompress("Content/Uploads/Images/", Imagesection, IVR.PK_IP_Id, IVR.PK_CALL_ID.ToString());
                        }
                        else
                        {
                            if (Imagesection.FileName != "")
                            {
                                IVR.ServiceImage = "NoImage.gif";
                            }
                        }
                    }
                    Result = objDAL.InsertUpdateServiceImage(IVR);
                    if (Result != null && Result != "")
                    {
                        ModelState.Clear();
                        TempData["UpdateCompany"] = Result;
                    }
                    // return RedirectToAction("QuotationServicesLink", IVR);
                    return RedirectToAction("QuotationServicesLink");
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            #region
            //DataTable CostSheetDashBoard = new DataTable();
            //List<ReportImageModel> lstCompanyDashBoard = new List<ReportImageModel>();
            //CostSheetDashBoard = objDalVisitReport.GetReportImageByCall_Id(IVR.PK_CALL_ID);
            //try
            //{
            //    if (CostSheetDashBoard.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in CostSheetDashBoard.Rows)
            //        {
            //            lstCompanyDashBoard.Add(
            //                new ReportImageModel
            //                {
            //                    Image = Convert.ToString(dr["Image"]),
            //                    Heading = Convert.ToString(dr["Heading"]),
            //                    PK_IP_Id = Convert.ToInt32(dr["PK_IP_Id"]),
            //                    PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"])
            //                }
            //                );
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string Error = ex.Message.ToString();
            //}
            //ViewData["CostSheet"] = lstCompanyDashBoard;
            //IVR.PK_IP_Id = 0;
            #endregion
            //IVR.PK_IP_Id = 0;
            //IVR.Image = null;

            return View(IVR);
        }


        public ActionResult Delete(int? PKServiceId)
        {
            string Result = string.Empty;
            try
            {
                Result = objDAL.Delete(Convert.ToInt32(PKServiceId));
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
            return RedirectToAction("QuotationServicesLink");


        }
        #endregion



        #region Industries
        public ActionResult Industries(QuotationServices objModel, int? PKIndustriesId)
        {
            #region Bind dropdown
            DataSet DSGetServiceName = new DataSet();
            DSGetServiceName = objDAL.GetServiceName();
            List<NameCode> lstServiceName = new List<NameCode>();


            if (DSGetServiceName != null && DSGetServiceName.Tables.Count > 0)
            {
                lstServiceName = (from n in DSGetServiceName.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DSGetServiceName.Tables[0].Columns["ServiceName"].ToString()),
                                      Code = n.Field<Int32>(DSGetServiceName.Tables[0].Columns["PKServiceId"].ToString())
                                  }).ToList();
            }


            IEnumerable<SelectListItem> ServiceName;
            ServiceName = new SelectList(lstServiceName, "Code", "Name");
            ViewBag.ServiceName = ServiceName;
            //ViewData["ExeService"] = ServiceName;
            #endregion


            DataTable CostSheetDashBoard = new DataTable();
            DataSet ImageDataById = new DataSet();
            List<QuotationServices> lstCompanyDashBoard = new List<QuotationServices>();



            if (PKIndustriesId != null)
            {
                ImageDataById = objDAL.GetIndustryById(PKIndustriesId);
                if (ImageDataById.Tables[0].Rows.Count > 0)
                {
                    objModel.FkServiceId = Convert.ToString(ImageDataById.Tables[0].Rows[0]["FkServiceId"]);
                    objModel.IndustryImage = Convert.ToString(ImageDataById.Tables[0].Rows[0]["IndustryImage"]);
                    objModel.IndustryName = Convert.ToString(ImageDataById.Tables[0].Rows[0]["IndustryName"]);
                    objModel.PKIndustriesId = Convert.ToInt32(ImageDataById.Tables[0].Rows[0]["PKIndustriesId"]);


                    // CostSheetDashBoard = objDAL.GetReportImageByCall_Id(objModel.PK_CALL_ID);
                    //if (CostSheetDashBoard.Rows.Count > 0)
                    //{
                    foreach (DataRow dr in ImageDataById.Tables[0].Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new QuotationServices
                            {
                                FkServiceId = Convert.ToString(dr["FkServiceId"]),
                                IndustryImage = Convert.ToString(dr["IndustryImage"]),
                                IndustryName = Convert.ToString(dr["IndustryName"]),
                                PKIndustriesId = Convert.ToInt16(dr["PKIndustriesId"]),

                            }
                            );
                    }
                    //}

                }

            }
            else
            {
                try
                {
                    ImageDataById = objDAL.GetAllIndustries();
                    if (ImageDataById.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ImageDataById.Tables[0].Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new QuotationServices
                                {
                                    FkServiceId = Convert.ToString(dr["FkServiceId"]),
                                    IndustryImage = Convert.ToString(dr["IndustryImage"]),
                                    IndustryName = Convert.ToString(dr["IndustryName"]),
                                    ServiceName = Convert.ToString(dr["ServiceName"]),
                                    PKIndustriesId = Convert.ToInt16(dr["PKIndustriesId"]),
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(objModel);


        }


        [HttpPost]
        public ActionResult Industries(QuotationServices objModel)
        {
            string Result = string.Empty;
            HttpPostedFileBase Imagesection;
            if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
            {
                Imagesection = Request.Files["img_Banner"];
                if (Imagesection != null && Imagesection.FileName != "")
                {
                    objModel.IndustryImage = CommonControl.FileUpload("QuotationHtml/QuotationServices/", Imagesection);
                    //IVR.Image = CommonControl.FileUploadCompress("Content/Uploads/Images/", Imagesection, IVR.PK_IP_Id, IVR.PK_CALL_ID.ToString());
                }
                else
                {
                    if (Imagesection.FileName != "")
                    {
                        objModel.IndustryImage = "NoImage.gif";
                    }
                }
            }

            Result = objDAL.InsertUpdateIndustry(objModel);
            if (Result != "" && Result != null)
            {
                ModelState.Clear();
                TempData["InsertCompany"] = Result;
            }

            return RedirectToAction("Industries");
        }

        public ActionResult DeleteIndustries(int? PKIndustriesId)
        {
            string Result = string.Empty;
            try
            {
                Result = objDAL.DeleteIndustry(Convert.ToInt32(PKIndustriesId));
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
            return RedirectToAction("Industries");


        }

        #endregion




        #region Projects
        public ActionResult QuotationProjects(QuotationServices objModel, int? PKProjectId)
        {
            #region Bind dropdown
            DataSet DSGetIndustry = new DataSet();
            DSGetIndustry = objDAL.GetIndustryName();
            List<NameCode> lstIndustryName = new List<NameCode>();

            if (DSGetIndustry.Tables[0].Rows.Count > 0)
            {
                lstIndustryName = (from n in DSGetIndustry.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DSGetIndustry.Tables[0].Columns["IndustryName"].ToString()),
                                      Code = n.Field<Int32>(DSGetIndustry.Tables[0].Columns["PKIndustriesId"].ToString())
                                  }).ToList();
            }

            IEnumerable<SelectListItem> IndustryName;
            IndustryName = new SelectList(lstIndustryName, "Code", "Name");
            ViewBag.IndustryName = IndustryName;
           
            #endregion

            DataTable CostSheetDashBoard = new DataTable();
            DataSet ImageDataById = new DataSet();
            List<QuotationServices> lstCompanyDashBoard = new List<QuotationServices>();



            if (PKProjectId != null)
            {
                ImageDataById = objDAL.GetProjectsById(objModel.PKProjectId);
                if (ImageDataById.Tables[0].Rows.Count > 0)
                {
                    objModel.PKProjectId = Convert.ToInt32(ImageDataById.Tables[0].Rows[0]["PKProjectId"]);
                    objModel.ProjectImage = Convert.ToString(ImageDataById.Tables[0].Rows[0]["ProjectImage"]);
                    objModel.Title = Convert.ToString(ImageDataById.Tables[0].Rows[0]["Title"]);
                    objModel.Description = Convert.ToString(ImageDataById.Tables[0].Rows[0]["Description"]);
                    objModel.FkIndustryId = Convert.ToString(ImageDataById.Tables[0].Rows[0]["FkIndustryId"]);




                    foreach (DataRow dr in ImageDataById.Tables[0].Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new QuotationServices
                            {
                               PKProjectId  = Convert.ToInt16(dr["PKProjectId"]),
                               ProjectImage  = Convert.ToString(dr["ProjectImage"]),
                               Title  = Convert.ToString(dr["Title"]),
                               Description = Convert.ToString(dr["Description"])

                            }
                            );
                    }
                    //}

                }

            }
            else
            {
                try
                {
                    ImageDataById = objDAL.GetAllProjects();
                    if (ImageDataById.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ImageDataById.Tables[0].Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new QuotationServices
                                {
                                    FkIndustryId = Convert.ToString(dr["FkIndustryId"]),
                                    PKProjectId = Convert.ToInt16(dr["PKProjectId"]),
                                    ProjectImage = Convert.ToString(dr["ProjectImage"]),
                                    Title = Convert.ToString(dr["Title"]),
                                    Description = Convert.ToString(dr["Description"])
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
            }

            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(objModel);


        }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult QuotationProjects(QuotationServices IVR, FormCollection fc, HttpPostedFileBase File)
        {
            string Result = string.Empty;
            try
            {
                if (IVR.PKProjectId == 0)
                {
                    HttpPostedFileBase Imagesection;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        Imagesection = Request.Files["img_Banner"];
                        if (Imagesection != null && Imagesection.FileName != "")
                        {
                            IVR.ProjectImage = CommonControl.FileUpload("QuotationHtml/QuotationServices/", Imagesection);
                            //IVR.Image = CommonControl.FileUploadCompress("Content/Uploads/Images/", Imagesection, IVR.PK_IP_Id, IVR.PK_CALL_ID.ToString());
                        }
                        else
                        {
                            if (Imagesection.FileName != "")
                            {
                                IVR.ProjectImage = "NoImage.gif";
                            }
                        }
                    }
                    //IVR.Type = "IVR";
                    IVR.Status = "1";
                    Result = objDAL.InsertUpdateProject(IVR);
                    if (Result != "" && Result != null)
                    {
                        ModelState.Clear();
                        TempData["InsertCompany"] = Result;
                    }
                    // return RedirectToAction("QuotationServicesLink", IVR);
                    return RedirectToAction("QuotationProjects");
                }
                else
                {
                    HttpPostedFileBase Imagesection;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        Imagesection = Request.Files["img_Banner"];
                        if (Imagesection != null && Imagesection.FileName != "")
                        {
                            IVR.ProjectImage = CommonControl.FileUpload("QuotationHtml/QuotationServices/", Imagesection);
                            // IVR.Image = CommonControl.FileUploadCompress("Content/Uploads/Images/", Imagesection, IVR.PK_IP_Id, IVR.PK_CALL_ID.ToString());
                        }
                        else
                        {
                            if (Imagesection.FileName != "")
                            {
                                IVR.ProjectImage = "NoImage.gif";
                            }
                        }
                    }
                    Result = objDAL.InsertUpdateProject(IVR);
                    if (Result != null && Result != "")
                    {
                        ModelState.Clear();
                        TempData["UpdateCompany"] = Result;
                    }
                    // return RedirectToAction("QuotationServicesLink", IVR);
                    return RedirectToAction("QuotationProjects");
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            #region
           
            #endregion
           

            return View(IVR);
        }

        public ActionResult DeleteProjects(int? PKProjectId)
        {
            string Result = string.Empty;
            try
            {
                Result = objDAL.DeleteProject(Convert.ToInt32(PKProjectId));
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
            return RedirectToAction("QuotationProjects");


        }

        #endregion




        #region URL Link
        public ActionResult Servicess(QuotationServices objModel)
        {
            DataSet ImageDataById = new DataSet();
            List<QuotationServices> lstCompanyDashBoard = new List<QuotationServices>();
            
            try
            {
                ImageDataById = objDAL.GetAllServicess();
                if (ImageDataById.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ImageDataById.Tables[0].Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new QuotationServices
                            {
                                PKServiceId = Convert.ToInt16(dr["PKServiceId"]),
                                ServiceImage = Convert.ToString(dr["ServiceImage"]),
                                ServiceName = Convert.ToString(dr["ServiceName"])
                                
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
        

        ViewData["Services"] = lstCompanyDashBoard;

            return View();
        }

        public ActionResult Industry(string PkServiceId)
        {
            DataSet ImageDataById = new DataSet();
            List<QuotationServices> lstCompanyDashBoard = new List<QuotationServices>();

            try
            {
                ImageDataById = objDAL.GetAllIndustryByServicId(PkServiceId);
                if (ImageDataById.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ImageDataById.Tables[0].Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new QuotationServices
                            {
                                PKIndustriesId = Convert.ToInt16(dr["PKIndustriesId"]),
                                IndustryName = Convert.ToString(dr["IndustryName"]),
                                IndustryImage = Convert.ToString(dr["IndustryImage"])
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }


            ViewData["Industry"] = lstCompanyDashBoard;

            return View();
        }

        public ActionResult Projects(string PkIndustry)
        {
            DataSet ImageDataById = new DataSet();
            List<QuotationServices> lstCompanyDashBoard = new List<QuotationServices>();

            try
            {
                ImageDataById = objDAL.GetAllProjectByIndustryId(PkIndustry);
                if (ImageDataById.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ImageDataById.Tables[0].Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new QuotationServices
                            {
                                PKProjectId = Convert.ToInt16(dr["PKProjectId"]),
                                Title = Convert.ToString(dr["Title"]),
                                ProjectImage = Convert.ToString(dr["ProjectImage"]),
                                Description = Convert.ToString(dr["Description"])
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }


            ViewData["Projects"] = lstCompanyDashBoard;

            return View();
        }
        #endregion



    }
}