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
    public class StaffAvailabilitiesController : Controller
    {
        private UPOSEntities db = new UPOSEntities();

        // GET: StaffAvailabilities
        public ActionResult Index()
        {
            var currentUser = db.Staffs.FirstOrDefault(x => x.EmailAddress == User.Identity.Name);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "701" });
            }

            AvailabilityPeriod currentPeriod = db.AvailabilityPeriods.FirstOrDefault(x => x.IsActivePeriod == true);
            if (currentPeriod == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "702" });
            }

            StaffAvailability sa = db.StaffAvailabilities.FirstOrDefault(x => x.StaffId == currentUser.Id && x.PeriodId == currentPeriod.Id);
            if (sa == null)
            {
                bool result = InsertAvailability(currentUser.Id, currentPeriod.Id);
                if (result == false)
                {
                    return RedirectToAction("Index", "Warning", new { id = "703" });
                }
            }

            StaffAvailability sAvailability = db.StaffAvailabilities.Include(s => s.AvailabilityPeriod).Include(s => s.Staff).FirstOrDefault(s => s.StaffId == currentUser.Id && s.PeriodId == currentPeriod.Id);
            if (sAvailability == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "704" });
            }

            return RedirectToAction("Edit", new { id = sAvailability.Id });
        }

        public string GetColor(bool asdf)
        {
            string color = "aquamarine";
            if (!asdf)
            {
                color = "red";
            }
            return color;
        }


        // GET: StaffAvailabilities/Edit/5
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

        // POST: StaffAvailabilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StaffId,PeriodId,Mon08,Mon09,Mon10,Mon11,Mon12,Mon13,Mon14,Mon15,Mon16,Mon17,Mon18,Mon19,Mon20,Tue08,Tue09,Tue10,Tue11,Tue12,Tue13,Tue14,Tue15,Tue16,Tue17,Tue18,Tue19,Tue20,Wed08,Wed09,Wed10,Wed11,Wed12,Wed13,Wed14,Wed15,Wed16,Wed17,Wed18,Wed19,Wed20,Thu08,Thu09,Thu10,Thu11,Thu12,Thu13,Thu14,Thu15,Thu16,Thu17,Thu18,Thu19,Thu20,Fri08,Fri09,Fri10,Fri11,Fri12,Fri13,Fri14,Fri15,Fri16,Fri17,Fri18,Fri19,Fri20,Sat08,Sat09,Sat10,Sat11,Sat12,Sat13,Sat14,Sat15,Sat16,Sat17,Sat18,Sat19,Sat20,Sun08,Sun09,Sun10,Sun11,Sun12,Sun13,Sun14,Sun15,Sun16,Sun17,Sun18,Sun19,Sun20,Modified,ModifiedBy")] StaffAvailability staffAvailability)
        {
            if (ModelState.IsValid)
            {
                staffAvailability.Modified = DateTime.Now;
                staffAvailability.ModifiedBy = User.Identity.Name;
                db.Entry(staffAvailability).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = staffAvailability.Id });
            }
            ViewBag.PeriodId = new SelectList(db.AvailabilityPeriods, "Id", "PeriodName", staffAvailability.PeriodId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name", staffAvailability.StaffId);
            return View(staffAvailability);
        }


        protected bool InsertAvailability(int staffId, int periodId)
        {
            bool result = false;

            StaffAvailability sa = new StaffAvailability();
            sa.StaffId = staffId;
            sa.PeriodId = periodId;
            sa.Mon08 = true;
            sa.Mon09 = true;
            sa.Mon10 = true;
            sa.Mon11 = true;
            sa.Mon12 = true;
            sa.Mon13 = true;
            sa.Mon14 = true;
            sa.Mon15 = true;
            sa.Mon16 = true;
            sa.Mon17 = true;
            sa.Mon18 = true;
            sa.Mon19 = true;
            sa.Mon20 = true;
            sa.Tue08 = true;
            sa.Tue09 = true;
            sa.Tue10 = true;
            sa.Tue11 = true;
            sa.Tue12 = true;
            sa.Tue13 = true;
            sa.Tue14 = true;
            sa.Tue15 = true;
            sa.Tue16 = true;
            sa.Tue17 = true;
            sa.Tue18 = true;
            sa.Tue19 = true;
            sa.Tue20 = true;
            sa.Wed08 = true;
            sa.Wed09 = true;
            sa.Wed10 = true;
            sa.Wed11 = true;
            sa.Wed12 = true;
            sa.Wed13 = true;
            sa.Wed14 = true;
            sa.Wed15 = true;
            sa.Wed16 = true;
            sa.Wed17 = true;
            sa.Wed18 = true;
            sa.Wed19 = true;
            sa.Wed20 = true;
            sa.Thu08 = true;
            sa.Thu09 = true;
            sa.Thu10 = true;
            sa.Thu11 = true;
            sa.Thu12 = true;
            sa.Thu13 = true;
            sa.Thu14 = true;
            sa.Thu15 = true;
            sa.Thu16 = true;
            sa.Thu17 = true;
            sa.Thu18 = true;
            sa.Thu19 = true;
            sa.Thu20 = true;
            sa.Fri08 = true;
            sa.Fri09 = true;
            sa.Fri10 = true;
            sa.Fri11 = true;
            sa.Fri12 = true;
            sa.Fri13 = true;
            sa.Fri14 = true;
            sa.Fri15 = true;
            sa.Fri16 = true;
            sa.Fri17 = true;
            sa.Fri18 = true;
            sa.Fri19 = true;
            sa.Fri20 = true;
            sa.Sat08 = true;
            sa.Sat09 = true;
            sa.Sat10 = true;
            sa.Sat11 = true;
            sa.Sat12 = true;
            sa.Sat13 = true;
            sa.Sat14 = true;
            sa.Sat15 = true;
            sa.Sat16 = true;
            sa.Sat17 = true;
            sa.Sat18 = true;
            sa.Sat19 = true;
            sa.Sat20 = true;
            sa.Sun08 = true;
            sa.Sun09 = true;
            sa.Sun10 = true;
            sa.Sun11 = true;
            sa.Sun12 = true;
            sa.Sun13 = true;
            sa.Sun14 = true;
            sa.Sun15 = true;
            sa.Sun16 = true;
            sa.Sun17 = true;
            sa.Sun18 = true;
            sa.Sun19 = true;
            sa.Sun20 = true;
            sa.Modified = DateTime.Now;
            sa.ModifiedBy = "System";

            try
            {
                db.StaffAvailabilities.Add(sa);
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
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
