using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OwinCas;

namespace POS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            string s = User.Identity.Name;

            int thisYear = DateTime.Now.Year;
            int nextYear = thisYear + 1;
            ViewBag.PosMessage = thisYear.ToString() + "-" + nextYear.ToString() + " Program of Studies";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Welcome..";
            ViewBag.HakanAbiEmail = "h.aras@vistula.edu.pl.";

            return View();
        }
    }
}