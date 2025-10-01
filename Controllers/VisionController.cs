using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class VisionController : Controller
    {
        // GET: Vision
        public ActionResult Index()
        {
            return View(DapperORM.ReturnList<ItemMasterModel>("ViewAllItem"));
        }

        [HttpGet]
        public ActionResult IteamInsert(int id = 0)
        {
            return View();
        }

        [HttpPost]
        public ActionResult IteamInsert(ItemMasterModel ITM)
        {

            DynamicParameters para = new DynamicParameters();
            para.Add("@inventoryCode", ITM.inventoryCode);
            para.Add("@inventoryName", ITM.inventoryName);
            para.Add("@inventoryPrice", ITM.inventoryPrice);
            para.Add("@inventoryAvailability", ITM.inventoryAvailability);
            para.Add("@inventoryTamilName", ITM.inventoryTamilName);
            para.Add("@inventoryImageURL", ITM.inventoryImageURL);

            DapperORM.ExecutewithoutReturn("InsertData", para);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UserMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserMaster(int id = 0)
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserList()
        {
            return View(DapperORM.ReturnList<ItemMasterModel>("ViewAllItem"));
        }

    }
}