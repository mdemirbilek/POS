using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POS;
using POS.Models;

namespace POS.Controllers
{
    public class AvailableRoomsController : Controller
    {
        private UPOSEntities db = new UPOSEntities();

        // GET: AvailableRooms
        public ActionResult Index()
        {
            var rooms = db.vwRoomAvailabilities;
            return View(rooms.OrderBy(x => x.Name).ThenBy(x => x.IntervalTime).ToList());
        }
        //.OrderBy(x => x.Name)

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
