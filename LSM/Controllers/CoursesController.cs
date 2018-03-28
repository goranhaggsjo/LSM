using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LSM.Models;
using Microsoft.AspNet.Identity;

namespace LSM.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public ActionResult Index(int? id, string message="None")
        {
            ViewBag.Message = message;
            string usernamne = User.Identity.GetUserId();
            var user = db.Users.Find(usernamne);
            if (user.Course == null)
            {
                return View(db.Courses.OrderByDescending(x => x.StartDate).ToList());

            }
            return RedirectToAction("Index", "Student");
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,StopDate")] Course course)
        {
            if (ModelState.IsValid)
            {
              
                db.Courses.Add(course);
                db.SaveChanges();
                string messagetowrite = "Course " + course.Name + " added!";
                return RedirectToAction("Index", new {message = messagetowrite });
            }

            return View(course);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id, string Message = "None")
        {
            ViewBag.Message = Message;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,StopDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                string messagetowrite = "Course " + course.Name + " edited!";
                return RedirectToAction("Index", new {message = messagetowrite });
            }
            return View(course);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            foreach(var user in course.Users)
            {
                user.CourseId = null;

            }
            
            db.Courses.Remove(course);
            db.SaveChanges();
            string messagetowrite = "Course " + course.Name + " deleted!";
            return RedirectToAction("Index", new {message = messagetowrite});
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
