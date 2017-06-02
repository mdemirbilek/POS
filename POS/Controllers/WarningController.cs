using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POS.Controllers
{
    public class WarningController : Controller
    {
        // GET: Warning
        public ActionResult Index(string id)
        {
            ViewBag.ErrMessage = "Warning!! Hmm, some bad things happened! Errör-";

            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.ErrMessage += id;
            }
            return View();
        }
    }
}