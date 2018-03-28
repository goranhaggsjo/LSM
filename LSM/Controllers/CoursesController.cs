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
using Microsoft.AspNet.Identity.Owin;

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

        // AddedHAQ
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

            // Make a list of included modules
            List<string> mod2 = new List<string>();
            foreach (var mod in course.Modules)
            {
                mod2.Add(mod.Name);
            }
            ViewBag.modlist = mod2;

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);

            
            // Del modules and activities connected to this course... Done automatically if
            // foreign keys have constraints, dvs required.......
            //  But CourseId in student is declared with ?, no constraint, and must be set to null

            /*  Onödigt
            List<Module> modulelist = new List<Module>();
            foreach (var mod in course.Modules)
            {
                modulelist.Add(mod);
            }
            foreach (var m1 in modulelist)
            {
                // List all activities in module m1, and then remove them
                List<Activity> actlist = new List<Activity>();
                foreach (var act in m1.Activitys)
                {
                    actlist.Add(act);
                }
                foreach (var a1 in actlist)
                {
                    db.Activitys.Remove(a1);
                }

                db.Modules.Remove(m1);
            }
            */

            foreach (var stud in course.Users)
            {
                stud.CourseId = null;
            }


            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // AddedHAQ
        // 
        public ActionResult ShowUsers()
        {
            List<userview> users = new List<userview>();


            // var ur = db.Set<IdentityUserRole>();   // ?  Kommer ej åt ur.UserId resp .RoleId

            // nav-props Roles, motsvarar korskopplingstabellen med hash för userId och roleId
            //  AspNetUserRoles. hash db.Users.Id -> hash db.user.Roles.UserId -> db.user.Roles.RoleId ->
            //  db.Roles.Name !!

            //  Problem med "already open data reader" nedan, när man loopar och läser ur flera db-tabeller
            //  samtidigt... Gör darför .ToList på första db-accessen, där man läser ut till user1, då hämtas
            //  all data där på en gång och operationen avslutas ---  annars blir det så att db.Users förblir
            //  "öppen" och man hämtar data efterhand ? Då kan det bli krock internt sen när man öppnar
            //  Courses tabellen och läser en kurs --- db.Users kan då fortfarande vara öppen ?
            //

            var user1 = db.Users.Include(x => x.Roles).ToList();

            foreach (var user in user1)
            {
                var u = new userview();
                u.FirstName = user.FirstName;
                u.LastName = user.LastName;
                u.email =  user.Email;
               
                u.CourseId = (user.CourseId == null) ? 0 : (int)user.CourseId;   // new 20180321
                if (user.CourseId == null)
                {
                    u.CourseName = "N/A";
                }
                else
                {
                    Course course2 = db.Courses.Find(user.CourseId);
                    u.CourseName = course2.Name;
                }
                   
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var roles = userManager.GetRoles(user.Id);
                var joinedRoles = string.Join(", ", roles);
                u.Role = joinedRoles;
                users.Add(u);

                /*
                
                // Nedan funkar ej, blir en massa skumma fel, beror på att man låser db tabeller
                // när man lopar igenom, se comments ovan !!!
                 
                string r2 = "";
                string s1 = "";

                foreach (var r in user.Roles)
                {
                    if (user.Id == r.UserId) r2 = r.RoleId;
                }

                foreach (var r1 in db.Roles)
                {
                    if (r1.Id == r2) s1 = r1.Name;
                }
                */


                /*
                var r2 = from t in user.Roles
                         where t.UserId == user.Id
                         select t;

                string s1="";
                foreach ( var r3 in r2)   // r2 blir en db-tabell med en rad
                {
                    s1 = r3.RoleId;
                }

                var s = from t in db.Roles   // s blir en db-tabell med en rad
                        where t.Id == s1
                        select t;

                string s4 = "";
                foreach (var r5 in s)
                {
                    s4 = r5.Name;
                }
                */

                //u.Role = "N/A";


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

        public void qtest()
        {
            var a1 = from t in db.Activitys
                     where t.Description == "A"
                     select t;
            var m1 = from t in db.Modules
                     where t.CourseId == 1 && t.Id > 2
                     select t.Description;

            // Nonsense, just check the join syntax....
            var c1 = from c in db.Courses
                     join m in db.Modules
                     on c.Id equals m.Id
                     select new { N1 = c.Name, N2 = m.Name };


            //  c2.Modules är en nav-prop, en lista på moduler. Så en record
            //  innehåller student name, kurs namn, och en lista på moduler !
            var s1 = from s in db.Users
                     join c2 in db.Courses
                     on s.CourseId equals c2.Id
                     select new { b1 = s.FullName, b2 = c2.Name,
                     b3 = c2.Modules };

            foreach(var x in s1)
            {
                foreach(var x1 in x.b3)
                {
                    // x1 är Module-ref ! x1.Name Module-name
                }
            }


            

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
