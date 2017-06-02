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
    public class UserRolesController : Controller
    {
        private UPOSEntities db = new UPOSEntities();

        // GET: UserRoles
        public ActionResult Index()
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "501" });
            }

            return View(db.UserRoles.ToList());
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "502" });
            }

            UserRole userRole = db.UserRoles.Find(id);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            return View(userRole);
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "503" });
            }

            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmailAddress,AssignedRole")] UserRole userRole)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "504" });
            }

            if (ModelState.IsValid)
            {
                db.UserRoles.Add(userRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userRole);
        }

        // GET: UserRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "505" });
            }

            UserRole userRole = db.UserRoles.Find(id);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            return View(userRole);
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmailAddress,AssignedRole")] UserRole userRole)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "506" });
            }

            if (ModelState.IsValid)
            {
                db.Entry(userRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userRole);
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "507" });
            }

            UserRole userRole = db.UserRoles.Find(id);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            return View(userRole);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "superman"))
            {
                return RedirectToAction("Index", "Warning", new { id = "508" });
            }

            UserRole userRole = db.UserRoles.Find(id);
            db.UserRoles.Remove(userRole);
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
