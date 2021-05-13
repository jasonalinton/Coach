using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coach.Service._Inventory;
using Coach.Service.Logging;
using CoachModel._Inventory._Log;

namespace Coach.Site.Areas.OG.Controllers
{
    public class InventoryController : Controller
    {
        // GET: Inventory
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Physical()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InventoryLog()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLogItems()
        {
            try
            {
                var InventoryLogService = new InventoryLogService();
                var LogItems = InventoryLogService.GetLogItems();

                return Json(new { LogItems, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error getting Log Items");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LogLogItemValue(LogItem LogItem)
        {
            try
            {
                var InventoryLogService = new InventoryLogService();
                InventoryLogService.LogLogItem(LogItem);

                var LogItems = InventoryLogService.GetLogItems();
                LogItems.Single(x => x.idLogItem == LogItem.idLogItem).WasSaved = true;

                return Json(new { LogItems, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error logging Log Item value ");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateLogItemValue(LogItem LogItem)
        {
            try
            {
                var InventoryLogService = new InventoryLogService();
                InventoryLogService.UpdateLogItem(LogItem);

                var LogItems = InventoryLogService.GetLogItems();
                LogItems.Single(x => x.idLogItem == LogItem.idLogItem).WasSaved = true;

                return Json(new { LogItems, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating Log Item value");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CatchControllerError(Exception ex)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ErrorLog.txt";
            var errorString = System.IO.File.ReadAllText(path);

            errorString += "\r\n";
            errorString += ex.Message;
            if (ex.InnerException != null)
                errorString += ex.InnerException.Message;

            System.IO.File.WriteAllText(path, errorString);

            return Json(new { success = false, error = ex.Message }, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}