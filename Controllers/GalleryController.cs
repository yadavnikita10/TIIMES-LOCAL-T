using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.BusinessEntities;
using TuvVision.BusinessServices.Implementation;
using TuvVision.BusinessServices.Interface;
using System.IO;

namespace TuvVision.Controllers
{
    public class GalleryController : Controller
    {
        private IGalleryServices igalserv = new GalleryServices();
        List<SelectListItem> ddlYears = new List<SelectListItem>();
        // GET: Gallery
        [HttpGet]
        public ActionResult AddEventGallery(int? id) //Add & Update Gallery Details
        {
            ViewBag.YearList = GetYears(0);
            if (id > 0)
            {
                Gallery_VM _vmgal = igalserv.GetGalleryById(Convert.ToInt32(id));
                
                return View(_vmgal);
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddEventGallery(Gallery_VM gal, List<HttpPostedFileBase> Gallery)
        {
            try
            {

                if (Gallery != null)
                {
                    string filepath = "/Content/FoodGallery/";

                    foreach (var item in Gallery)
                    {
                        if (item.FileName.EndsWith(".JPEG") || item.FileName.EndsWith(".jpg") || item.FileName.EndsWith(".jpeg") || item.FileName.EndsWith(".JPG") || item.FileName.EndsWith(".png") || item.FileName.EndsWith(".gif"))
                        {
                            var savedpath = this.FileUploadDynamicName(filepath, item);

                            gal.Gallery = savedpath;

                            int i = igalserv.AddGalleryDetails(gal);
                            if (i != 0)
                            {
                                TempData["Success"] = "Gallery Details Added Successfully...";
                                TempData.Keep();
                                //return RedirectToAction("GelListOfGallery");
                            }
                            else
                            {
                                TempData["Error"] = "Something went Wrong! Please try Again";
                                TempData.Keep();
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Something went Wrong! Please try Again";
                            TempData.Keep();
                            return RedirectToAction("AddEventGallery");
                        }
                    }                  
                }
                return RedirectToAction("GelListOfGallery");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string FileUploadDynamicName(string path, HttpPostedFileBase fu) //Method for creating unique Name for file in Directory
        {
            try
            {
                if (fu != null)
                {
                    string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fu.FileName);
                    string sTime = Convert.ToString(DateTime.Now.Hour) + "" + Convert.ToString(DateTime.Now.Minute) + "" + Convert.ToString(DateTime.Now.Second);
                    string ReportName = fileNameWithoutExtension + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + sTime + System.IO.Path.GetExtension(fu.FileName);
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + ReportName;
                    string ImageDirectoryFP = path.Replace("/", "\\");
                    string ImageDirectory = "~/" + path;
                    string fileNameWithExtension = System.IO.Path.GetExtension(ReportName);
                    string ImagePath = "~" + path + ReportName;
                    string ImageName = ReportName;
                    fu.SaveAs(filePath);

                    return ImagePath;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        [HttpGet]
        public ActionResult GelListOfGallery() //Get List Of Gallery Details
        {
            List<Gallery_VM> lstgal = new List<Gallery_VM>();
            lstgal = igalserv.GetAllGalleryList().ToList();
            ViewBag.Data = lstgal;
            return View(lstgal);
        }
        //public ActionResult YearList(int? Year)
        //{
        //    if (Year == null)
        //    {
        //        Year = DateTime.Now.Year;
        //    }

        //    ViewBag.linktoYearId = GetYears(Year);
        //    return View();
        //}
        public SelectList GetYears(int? iSelectedYear)
        {
            int CurrentYear = DateTime.Now.Year;

            for (int i = 1970; i <= DateTime.Now.Year; i++)
            {
                ddlYears.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            //Default It will Select Current Year  
            return new SelectList(ddlYears, "Value", "Text", iSelectedYear);

        }
        [HttpGet]
        public ActionResult GetAllYearList() //Get List Of Year
        {
            List<Gallery_VM> lstyrs = new List<Gallery_VM>();
            lstyrs = igalserv.GetAllYearList().ToList();
            ViewBag.GetYearList = lstyrs;

            ViewBag.GallaryData = igalserv.GetAllList();

            return View(lstyrs);
        }

        public ActionResult GelAllTitleList(string yr) //Get List Of Title's
        {
            List<Gallery_VM> lstttl = new List<Gallery_VM>();
            if (yr == "All")
            {
               
                var lstttla = igalserv.GetAllList().ToList();

                return PartialView("_GetTitlePartial", lstttla);
            }
            lstttl = igalserv.GetAllTitleList(Convert.ToInt32(yr)).ToList();

            return PartialView("_GetTitlePartial", lstttl);
        }
        public ActionResult GelAllGalyList(string titl) //Get List Of Images in Gallery
        {

            var lstttl = igalserv.GetAllList(); //GetAllGallery(titl).ToList();

            return PartialView("_DisplayGallery", lstttl);
        }
        public ActionResult GelAllTitles() //Get List Of Title's
        {
            List<Gallery_VM> lstttl = new List<Gallery_VM>();
            lstttl = igalserv.GetAllList().ToList();

            return PartialView("_GetTitlePartial", lstttl);
        }
        public ActionResult DeleteGallery(int? id)
        {
            try
            {
                //var currentDir = "~/Content/FoodGallery/";
                //var files = Directory.GetFiles(currentDir);

                if (igalserv.DeleteImage(id))
                {
                    TempData["Deleted"] = "Gallery Details removed Successfully";
                    TempData.Keep();
                    return RedirectToAction("GelListOfGallery");
                }
                else
                {
                    TempData["Error"] = "Something went wrong. Try Again...";
                    TempData.Keep();
                    return RedirectToAction("GelListOfGallery");
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("GelListOfGallery");
        }
        public ActionResult DeleteMultiple(int[] ImgIds)
        {
                foreach (int item in ImgIds)
                {
                    var idlist = igalserv.DeleteImage(Convert.ToInt32(item));
                }
            TempData["Deleted"] = "Gallery Details removed Successfully";
            TempData.Keep();

            return Json("Success", JsonRequestBehavior.AllowGet);
        }
    }
}
