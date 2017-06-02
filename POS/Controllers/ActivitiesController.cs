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
    [Authorize]
    public class ActivitiesController : Controller
    {
        private UPOSEntities db = new UPOSEntities();

        // GET: Activities
        public ActionResult Index(int? id)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "101" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Module activityModule = db.Modules.Where(x => x.Id == id).FirstOrDefault();
            if (activityModule == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ModuleId = activityModule.Id.ToString();
            ViewBag.ModuleName = activityModule.Name;
            ViewBag.ModuleNameEN = activityModule.NameEN;

            var activities = db.Activities.Where(x => x.ModuleId == id).Include(a => a.ActivityType).Include(a => a.EvalType).Include(a => a.Module).Include(a => a.RoomType).Include(a => a.Staff);
            return View(activities.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "102" });
            }

            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "103" });
            }

            Module activityModule = db.Modules.Where(x => x.Id == id).FirstOrDefault();
            if (activityModule == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ModuleId = activityModule.Id.ToString();
            ViewBag.ModuleName = activityModule.Name;
            ViewBag.ModuleNameEN = activityModule.NameEN;



            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name");
            ViewBag.EvalTypeId = new SelectList(db.EvalTypes, "Id", "Name");
            //ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name");
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name");
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ActivityTypeId,TotalHours,EvalTypeId,RoomTypeId,StaffId,Note,GroupNumber")] Activity activity, int? id)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "104" });
            }

            Module activityModule = db.Modules.Where(x => x.Id == id).FirstOrDefault();
            if (activityModule == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ModuleId = activityModule.Id.ToString();
            ViewBag.ModuleName = activityModule.Name;
            ViewBag.ModuleNameEN = activityModule.NameEN;

            if (ModelState.IsValid)
            {
                //Module module = db.Modules.Find(activity.ModuleId);

                activity.ModuleId = activityModule.Id;
                activity.Modified = DateTime.Now;
                activity.ModifiedBy = User.Identity.Name;

                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = activityModule.Id });
            }

            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
            ViewBag.EvalTypeId = new SelectList(db.EvalTypes, "Id", "Name", activity.EvalTypeId);
            //ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name", activity.RoomTypeId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name", activity.StaffId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "105" });
            }

            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }

            ViewBag.MyModuleId = activity.Module.Id;
            ViewBag.ModuleName = activity.Module.Name;
            ViewBag.ModuleNameEN = activity.Module.NameEN;

            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
            ViewBag.EvalTypeId = new SelectList(db.EvalTypes, "Id", "Name", activity.EvalTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name", activity.RoomTypeId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name", activity.StaffId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ModuleId,ActivityTypeId,TotalHours,EvalTypeId,RoomTypeId,StaffId,Note,Modified,ModifiedBy,GroupNumber")] Activity activity)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "106" });
            }

            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = activity.ModuleId });
            }

            ViewBag.MyModuleId = activity.Module.Id;
            ViewBag.ModuleName = activity.Module.Name;
            ViewBag.ModuleNameEN = activity.Module.NameEN;

            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
            ViewBag.EvalTypeId = new SelectList(db.EvalTypes, "Id", "Name", activity.EvalTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "Id", "Name", activity.RoomTypeId);
            ViewBag.StaffId = new SelectList(db.Staffs, "Id", "Name", activity.StaffId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "107" });
            }

            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }

            ViewBag.MyModuleId = activity.Module.Id;
            ViewBag.ModuleName = activity.Module.Name;
            ViewBag.ModuleNameEN = activity.Module.NameEN;

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "108" });
            }

            Activity activity = db.Activities.Find(id);

            int moduleId = activity.ModuleId;

            db.Activities.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index", new { Id = moduleId });
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
