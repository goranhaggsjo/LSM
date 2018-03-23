using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LSM.Models;

namespace LSM.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modules
        public ActionResult Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(modules.ToList());
        }

        // GET: Modules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        public ActionResult Create(int CourseId)
        {
            ViewBag.CourseForModule = CourseId;
            ViewBag.Course = db.Courses.Where(c => c.Id == CourseId).First();
            ViewBag.Stopdate = "";
            ViewBag.startdate = "";

            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,StopDate,CourseId")] Module module)
        {
            Course course = db.Courses.Where(predicate: c => c.Id == module.CourseId).First();

            ViewBag.Stopdate = "";
            ViewBag.startdate = "";
            int result1 = DateTime.Compare(module.StartDate, course.StartDate);
            int result2 = DateTime.Compare(module.StopDate, course.StopDate);

            if (result1 <= 0)
            {
                if (result2 >= 0)
                {
                    if (ModelState.IsValid)
                    {
                        db.Modules.Add(module);
                        db.SaveChanges();

                        return RedirectToAction("Edit", "Courses", new { id = module.CourseId });
                    }

                }
                ViewBag.Stopdate = "Stop date is after the course's stop date.";
            }
            ViewBag.startdate = "Start date is before the course's start date.";
            return View(module);
        }

        // GET: Modules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Stopdate = "";
            ViewBag.startdate = "";

            Module module = db.Modules.Find(id);
            Course course = db.Courses.Where(predicate: c => c.Id == module.CourseId).First();

            ViewBag.CourseName = "Course: " + course.Name;

            if (module == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);

            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,StopDate,CourseId")] Module module)
        {
            Course course = db.Courses.Where(predicate: c => c.Id == module.CourseId).First();
            ViewBag.CourseName = "Course: " + course.Name;

            ViewBag.Stopdate = "";
            ViewBag.Startdate = "";

            int result1 = DateTime.Compare(module.StartDate, course.StartDate);
            int result2 = DateTime.Compare(module.StopDate, course.StopDate);

            if (result1 <= 0)
            {
                if (result2 >= 0)
                {

                    if (ModelState.IsValid)
                    {
                        db.Entry(module).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Edit", "Courses", new { id = module.CourseId });
                    }
                }
                ViewBag.Stopdate = "Stop date is after the course's stop date.";
            }
            ViewBag.Startdate = "Start date is before the course's start date.";
            
            return View(module);
        }

        // GET: Modules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Edit", "Courses", new { id = module.CourseId });

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
