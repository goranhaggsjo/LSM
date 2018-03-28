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
                string messagetowrite = "Module " + module.Name + " added!";


                return RedirectToAction("Edit", "Courses", new {id = module.CourseId, message =  messagetowrite});
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }

        // GET: Modules/Edit/5
        public ActionResult Edit(int? id, string message = "None")
        {   ViewBag.Message = message;
            if (id == null)
            {return RedirectToAction("Index", "Courses");
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {return RedirectToAction("Index", "Courses");
            }            
            return View(module);
        }

        // POST: Modules/Edit/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,StopDate,CourseId")] Module module)
        {  if (ModelState.IsValid)
            {  db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                string messagetowrite = "Module " + module.Name + " edited!";
                return RedirectToAction("Edit", "Courses", new { id = module.CourseId, message = messagetowrite });
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }

        // GET: Modules/Delete/5
        public ActionResult Delete(int? id)
        {  if (id == null)
            {  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            { return HttpNotFound();
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
            string messagetowrite = "Module " + module.Name + " deleted!";
            return RedirectToAction("Edit", "Courses", new { id = module.CourseId, message = messagetowrite });

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
