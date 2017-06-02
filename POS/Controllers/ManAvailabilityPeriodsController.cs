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
    public class ManAvailabilityPeriodsController : Controller
    {
        private UPOSEntities db = new UPOSEntities();

        // GET: ManAvailabilityPeriods
        public ActionResult Index()
        {
            var currentUser = db.Staffs.FirstOrDefault(x => x.EmailAddress == User.Identity.Name);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "781" });
            }

            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "782" });
            }

            return View(db.AvailabilityPeriods.ToList());
        }

        // GET: ManAvailabilityPeriods/Details/5
        public ActionResult Details(int? id)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "783" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailabilityPeriod availabilityPeriod = db.AvailabilityPeriods.Find(id);
            if (availabilityPeriod == null)
            {
                return HttpNotFound();
            }
            return View(availabilityPeriod);
        }

        // GET: ManAvailabilityPeriods/Create
        public ActionResult Create()
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "784" });
            }

            return View();
        }

        // POST: ManAvailabilityPeriods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PeriodName,IsActivePeriod")] AvailabilityPeriod availabilityPeriod)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "785" });
            }

            if (ModelState.IsValid)
            {
                db.AvailabilityPeriods.Add(availabilityPeriod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(availabilityPeriod);
        }

        // GET: ManAvailabilityPeriods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "786" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailabilityPeriod availabilityPeriod = db.AvailabilityPeriods.Find(id);
            if (availabilityPeriod == null)
            {
                return HttpNotFound();
            }
            return View(availabilityPeriod);
        }

        // POST: ManAvailabilityPeriods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PeriodName,IsActivePeriod")] AvailabilityPeriod availabilityPeriod)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "787" });
            }

            if (ModelState.IsValid)
            {
                db.Entry(availabilityPeriod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(availabilityPeriod);
        }

        // GET: ManAvailabilityPeriods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "788" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailabilityPeriod availabilityPeriod = db.AvailabilityPeriods.Find(id);
            if (availabilityPeriod == null)
            {
                return HttpNotFound();
            }
            return View(availabilityPeriod);
        }

        // POST: ManAvailabilityPeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "789" });
            }

            AvailabilityPeriod availabilityPeriod = db.AvailabilityPeriods.Find(id);
            db.AvailabilityPeriods.Remove(availabilityPeriod);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
