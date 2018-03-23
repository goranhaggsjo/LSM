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
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modules
        public ActionResult Index(string sortOrder)
        {
            ViewBag.ModuleSortPar = sortOrder == "Mod" ? "Mod_desc" : "Mod";
            ViewBag.StartDateSortPar = sortOrder == "Date" ? "Date_desc" : "Date";

            // Include, Course is a nav-prop, a ref to a Course....
            // In the view, this can be referenced,    model.Course.Name

            var modules = db.Modules.Include(m => m.Course);

            switch (sortOrder)
            {
                case "Mod_desc":
                    modules = modules.OrderByDescending(v => v.Name);
                    break;
                case "Mod":
                    modules = modules.OrderBy(v => v.Name);
                    break;
                case "Date_desc":
                    modules = modules.OrderByDescending(v => v.StartDate);
                    break;
                case "Date":
                    modules = modules.OrderBy(v => v.StartDate);
                    break;

            }

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
        public ActionResult Create()
        {
            // Skapa en drop-down list i vy för att välja kurs
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,StopDate,CourseId")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }

        // GET: Modules/Edit/5
        public ActionResult Edit(int? id)
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
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }

        // AddedHAQ
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

            // Make a list of included activities
            List<string> act2 = new List<string>();
            foreach (var act in module.Activitys)
            {
                act2.Add(act.Name);
            }
            ViewBag.actlist = act2;

            return View(module);
        }

        //AddedHAQ, also remove activities
        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);

            // Del activities connected to this module,
            // first make a separate list of them, then del. Entity FW will
            // del the corresponding nav-props. Cant loop over nav-props in module.Activitys
            // and del the activities at the same time.

            // Men onödigt ! EF kommer att ta bort activities själv om dom inte längre
            // pekar på någon, om foreign key har constraints, dvs den måste finnas, och får ej
            // vara nullable deklarerad med ?
            // Dvs garbage collection i dB.

            /*  Onödigt.....
            List<Activity> a = new List<Activity>();
            foreach (var act in module.Activitys)
            {
                a.Add(act);
            }
            foreach (var a1 in a)
            {
                db.Activitys.Remove(a1);
            }
            */

            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // AddedHAQ     GET: Modules/Create
        // Called by Ajax from ShowCourseMod
        public ActionResult Create2(int? courseId)
        {
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            return PartialView();
        }

        // AddedHAQ
        // POST: Modules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "Id,Name,Description,StartDate,StopDate,CourseId")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                db.SaveChanges();
                // course-id funkar !
                // OK  return RedirectToAction("Index", "Courses");
                return RedirectToAction("ShowCourseMod", "Courses", new { id = module.CourseId });
            }

            //ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return PartialView();
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
