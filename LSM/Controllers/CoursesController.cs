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
using Microsoft.AspNet.Identity.EntityFramework;

namespace LSM.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // GET: Courses/Details/5
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

        // GET: Courses/Create
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
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
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
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
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
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // AddedHAQ
        // 
        public ActionResult ShowUsers()
        {
            List<userview> users = new List<userview>();
            //var userStore = new UserStore<ApplicationUser>(db);
            //var userManager = new UserManager<ApplicationUser>(userStore);
            // var u1 = userManager.FindByName(model.Email);

            foreach (var user in db.Users)
            {
                var u = new userview();
                u.FirstName = user.FirstName;
                u.LastName = user.LastName;
                u.email = user.Email;
                //string rolename = userManager.GetRoles(user.Id).FirstOrDefault();
                u.Role = "N/A";
                users.Add(u);
            }

            return View(users.ToList());
  
        }

        
        // AddedHAQ
        //  Input id is course id, send it back to next view in viewbag
        public ActionResult ShowCourseMod(int? id)
        {
            List<Module> m1 = new List<Module>();
            Course course = db.Courses.Find(id);
            ViewBag.Name = course.Name;
            ViewBag.Description = course.Description;
            ViewBag.Start = course.StartDate.ToString();
            ViewBag.Id = id;

            //var m1 = new Module();
            foreach (var m in course.Modules)
                m1.Add(m);

            return View(m1);

        }


        // AddedHAQ
        //  Testing partial view
        public ActionResult ShowCourseStud(int? id)
        {
            List<ApplicationUser> s1 = new List<ApplicationUser>();
            Course course = db.Courses.Find(id);
           
            foreach (var s in course.Users)
                s1.Add(s);

            return PartialView(s1);
        }

        // AddedHAQ
        //  Testing Ajax view
        //public PartialViewResult Act(int? moduleId)
        public ActionResult Act(int? moduleId)
        {
            List<Activity> a1 = new List<Activity>();
            Module mod = db.Modules.Find(moduleId);
         
            foreach (var a in mod.Activitys)
                a1.Add(a);

            return PartialView(a1);
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
