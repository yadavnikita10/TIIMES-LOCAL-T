using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.BusinessEntities;
using TuvVision.BusinessServices.Implementation;
using TuvVision.BusinessServices.Interface;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class DisplayNewsController : Controller
    {
        INewsDetails newsrepo = new NewsDetailss();
        News_VM _vmnews = new News_VM();
        public NewsDetailss DALNews = new NewsDetailss();
        DataTable DTGetUploadedFile = new DataTable();
        // GET: DisplayNews
        public ActionResult DisplayListNewsContent()
        {
            List<News_VM> lstnews = new List<News_VM>();
            lstnews = newsrepo.GetAllNewsDetails().ToList();
            ViewBag.Data = lstnews;
            DTGetUploadedFile = DALNews.UpdateUserFlag();
            return View(lstnews);
        }


        [HttpGet]
        public ActionResult ReadNewsContent(int? id)
        {
            if (id > 0)
            {
                News_VM _vmnews = newsrepo.GetNewsDataById(Convert.ToInt32(id));

                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();

                List<string> GetImages = new List<string>();

                DTGetUploadedFile = DALNews.EditUploadedFile(id);

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
                        GetImages.Add("~/Content/TinyMCE_Upload/" + Convert.ToString(dr["FileID"]) + Convert.ToString(dr["Extenstion"]));
                    }
                    ViewBag.GetImages = string.Join(",", GetImages);

                    ViewData["lstEditFileDetails"] = lstEditFileDetails;
                    _vmnews.FileDetails = lstEditFileDetails;
                }

                DTGetUploadedFile = DALNews.GetEventImagesCAPP(id);
                string EImages = string.Empty;

                if (DTGetUploadedFile.Rows.Count > 0)
                {
                    for (int row = 0; row < DTGetUploadedFile.Rows.Count; row++)
                    {
                      //  EImages = EImages + Convert.ToString(DTGetUploadedFile.Rows[row]["NewsEvent"]);
                        if (EImages == string.Empty)
                            EImages = Convert.ToString(DTGetUploadedFile.Rows[row]["NewsEvent"]);
                        else
                            EImages = EImages + "," + Convert.ToString(DTGetUploadedFile.Rows[row]["NewsEvent"]);
                    }
                    ViewBag.GetEImages = EImages;
                }          

                  
                return View(_vmnews);
            }
            return View();
        }
    }
}