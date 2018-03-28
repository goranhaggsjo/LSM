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
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string usernamne = User.Identity.GetUserId();
            var user = db.Users.Find(usernamne);
            
            Course course = user.Course;
           var ModuelList = user.Course.Modules;
            //var ModuleListNow = new List<Module>();
            var ActivityListNow = new List<Activity>();
            foreach (var m in ModuelList)
            {
                //if ((m.StopDate > DateTime.Now) && (m.StartDate < DateTime.Now.AddDays(6)))
                //{
                //   ModuleListNow.Add(m);

                    
                //}

                if ((m.StopDate > DateTime.Now) && (m.StartDate < DateTime.Now.AddDays(6)))
                {

                    foreach (var a in m.Activitys)                       
                     if((a.Day > DateTime.Now) && (a.Day < DateTime.Now.AddDays(6)))
                    {
                        ActivityListNow.Add(a);


                    }


                }



            }
            //ViewBag.ModueListNow = ModuleListNow;
            ViewBag.ActivityListNow = ActivityListNow.OrderByDescending(x=>x.Day).ToList();
            return View(course);
        }

        public ActionResult Module(int? id)
        {
            string usernamne = User.Identity.GetUserId();
            var user = db.Users.Find(usernamne);
            var Modules = user.Course.Modules;
           

            foreach(var t in Modules)
            {
                if(t.Id == id)
                {
                    Module module = t;
                    return View(module);
                }
                
            }

            return RedirectToAction("Index");
                            
                     


        }

        public ActionResult Activity(int? id)

            

        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Activity activity = db.Activitys.Find(id);
            if (activity == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);




            //GRÖN KOD SKA VARA KVAR TAS EJ BORT
            //string usernamne = User.Identity.GetUserId();
            //var user = db.Users.Find(usernamne);
            //var Actitivities = user.Course.Modules;


            //foreach (var u in Actitivities)
            //{
            //    if (u.CourseId == couresId)
            //    {
            //        Module activity = u;
            //        return View(activity);
            //    }
            //    //Förs lopa igenom och hitta kurside sedna när den har hittats loppa igenom alla actitvitye som har den för att hitta ID
            //}

            return View(activity);
        }






    }
}