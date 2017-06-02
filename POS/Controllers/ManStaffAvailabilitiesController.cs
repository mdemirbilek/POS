using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POS;

namespace POS.Controllers
{
    public class ManStaffAvailabilitiesController : Controller
    {
        private UPOSEntities db = new UPOSEntities();

        // GET: ManStaffAvailabilities
        public ActionResult Index()
        {
            var staffAvailabilities = db.StaffAvailabilities.Include(s => s.AvailabilityPeriod).Include(s => s.Staff);
            return View(staffAvailabilities.ToList());
        }

        // GET: ManStaffAvailabilities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffAvailability staffAvailability = db.StaffAvailabilities.Find(id);
            if (staffAvailability == null)
            {
                return HttpNotFound();
            }
            return View(staffAvailability);
        }

        // GET: ManStaffAvailabilities/Create
        public ActionResult Create()
        {
            ViewBag.PeriodId = new SelectList(db.AvailabilityPeriods, "Id", "PeriodName");
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name");
            return View();
        }

        // POST: ManStaffAvailabilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StaffId,PeriodId,Mon08,Mon09,Mon10,Mon11,Mon12,Mon13,Mon14,Mon15,Mon16,Mon17,Mon18,Mon19,Mon20,Tue08,Tue09,Tue10,Tue11,Tue12,Tue13,Tue14,Tue15,Tue16,Tue17,Tue18,Tue19,Tue20,Wed08,Wed09,Wed10,Wed11,Wed12,Wed13,Wed14,Wed15,Wed16,Wed17,Wed18,Wed19,Wed20,Thu08,Thu09,Thu10,Thu11,Thu12,Thu13,Thu14,Thu15,Thu16,Thu17,Thu18,Thu19,Thu20,Fri08,Fri09,Fri10,Fri11,Fri12,Fri13,Fri14,Fri15,Fri16,Fri17,Fri18,Fri19,Fri20,Sat08,Sat09,Sat10,Sat11,Sat12,Sat13,Sat14,Sat15,Sat16,Sat17,Sat18,Sat19,Sat20,Sun08,Sun09,Sun10,Sun11,Sun12,Sun13,Sun14,Sun15,Sun16,Sun17,Sun18,Sun19,Sun20,Modifies,ModifiedBy,Modified")] StaffAvailability staffAvailability)
        {
            if (ModelState.IsValid)
            {
                db.StaffAvailabilities.Add(staffAvailability);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PeriodId = new SelectList(db.AvailabilityPeriods, "Id", "PeriodName", staffAvailability.PeriodId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name", staffAvailability.StaffId);
            return View(staffAvailability);
        }

        // GET: ManStaffAvailabilities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffAvailability staffAvailability = db.StaffAvailabilities.Find(id);
            if (staffAvailability == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeriodId = new SelectList(db.AvailabilityPeriods, "Id", "PeriodName", staffAvailability.PeriodId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name", staffAvailability.StaffId);
            return View(staffAvailability);
        }

        // POST: ManStaffAvailabilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StaffId,PeriodId,Mon08,Mon09,Mon10,Mon11,Mon12,Mon13,Mon14,Mon15,Mon16,Mon17,Mon18,Mon19,Mon20,Tue08,Tue09,Tue10,Tue11,Tue12,Tue13,Tue14,Tue15,Tue16,Tue17,Tue18,Tue19,Tue20,Wed08,Wed09,Wed10,Wed11,Wed12,Wed13,Wed14,Wed15,Wed16,Wed17,Wed18,Wed19,Wed20,Thu08,Thu09,Thu10,Thu11,Thu12,Thu13,Thu14,Thu15,Thu16,Thu17,Thu18,Thu19,Thu20,Fri08,Fri09,Fri10,Fri11,Fri12,Fri13,Fri14,Fri15,Fri16,Fri17,Fri18,Fri19,Fri20,Sat08,Sat09,Sat10,Sat11,Sat12,Sat13,Sat14,Sat15,Sat16,Sat17,Sat18,Sat19,Sat20,Sun08,Sun09,Sun10,Sun11,Sun12,Sun13,Sun14,Sun15,Sun16,Sun17,Sun18,Sun19,Sun20,Modifies,ModifiedBy,Modified")] StaffAvailability staffAvailability)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffAvailability).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PeriodId = new SelectList(db.AvailabilityPeriods, "Id", "PeriodName", staffAvailability.PeriodId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name", staffAvailability.StaffId);
            return View(staffAvailability);
        }

        // GET: ManStaffAvailabilities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffAvailability staffAvailability = db.StaffAvailabilities.Find(id);
            if (staffAvailability == null)
            {
                return HttpNotFound();
            }
            return View(staffAvailability);
        }

        // POST: ManStaffAvailabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StaffAvailability staffAvailability = db.StaffAvailabilities.Find(id);
            db.StaffAvailabilities.Remove(staffAvailability);
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
