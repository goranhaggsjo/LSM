namespace LSM.Migrations
{
    using LSM.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LSM.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "LSM.Models.ApplicationDbContext";
        }

        protected override void Seed(LSM.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // En massa komplicerad kod f�r att skapa nya anv�ndare och roller !! M�ste g�ras i r�tt ordning.


            var courses = new List<Course>

            {

                new Course { Name = "Java", Description = "Coding Java",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01")},

                new Course { Name = "Python", Description = "Coding Pyhton",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01")},

                new Course { Name = "Skiing", Description = "Hahnenkamm Rennen",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01")},

                new Course { Name = "Packrafting", Description = "Nitt�lven",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01")},

                new Course { Name = "Biking",   Description = "Over the alps",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01")}

            };



            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();





            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { "Teacher", "Student" };       // A list of different roles...
            foreach (var roleName in roleNames)
            {
                if (context.Roles.Any(r => r.Name == roleName)) continue;

                // Create new role
                var role = new IdentityRole { Name = roleName };
                var result = roleManager.Create(role);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var emails = new[] { "gandalf@aa.se", "galadriel@aa.se", "frodo@aa.se", "gimli@aa.se",
                 "knatte@aa.se",  "fnatte@aa.se",  "tjatte@aa.se" };

            //var emails = new[] { "admin@Gymbokning.se" };

            foreach (var email in emails)
            {
                if (context.Users.Any(u => u.UserName == email)) continue;

                // Create user, with username and pwd
                // Update with FirstName, LastName
                DateTime t1 = DateTime.Now;
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "Gandalf",
                    LastName = "Grey",
                    CourseId = courses[1].Id
                };

                var result = userManager.Create(user, "Gandalf1!");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }



            var u1 = userManager.FindByName("gandalf@aa.se");
            userManager.AddToRole(u1.Id, "Teacher");
            u1.CourseId = null;

            var u2 = userManager.FindByName("galadriel@aa.se");
            userManager.AddToRole(u2.Id, "Teacher");
            u2.CourseId = null;

            var u3 = userManager.FindByName("frodo@aa.se");
            userManager.AddToRoles(u3.Id, "Teacher");
            u3.CourseId = null;

            var u4 = userManager.FindByName("gimli@aa.se");
            userManager.AddToRoles(u4.Id, "Student");
            u4.CourseId = 2;

            var u5 = userManager.FindByName("knatte@aa.se");
            userManager.AddToRoles(u5.Id, "Student");
            u5.CourseId = 4;

            var u6 = userManager.FindByName("fnatte@aa.se");
            userManager.AddToRoles(u6.Id, "Student");
            u6.CourseId = 4;

            var u7 = userManager.FindByName("tjatte@aa.se");
            userManager.AddToRoles(u7.Id, "Student");
            u7.CourseId = 5;



            //u4.CourseId = courses[0].Id;
            //u5.CourseId = courses[1].Id;
            //u6.CourseId = courses[2].Id;
            //u7.CourseId = courses[3].Id;


            var modules = new List<Module>
            {
                new Module { Name = "Part1", Description = "First introduction",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = 1 },

                new Module { Name = "Part2", Description = "Going uphill",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = 2 },

                new Module { Name = "Part3", Description = "Go downhill",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = 3 },

                new Module { Name = "Part4", Description = "Go forward",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = 4 },

                new Module { Name = "Part5", Description = "Run back",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = 5 },

                new Module { Name = "Part6", Description = "Start flying",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = 5 },

                new Module { Name = "Part7", Description = "Take a walk",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = 4 }

            };



            modules.ForEach(s => context.Modules.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var act = new List<Activity>
            {
                new Activity { Name = "Act20", Description = "Try whiskey",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), ModuleId = 4 },

                new Activity { Name = "Act21", Description = "Go home",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), ModuleId = 2 },

                new Activity { Name = "Act22", Description = "Drink beer",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), ModuleId = 3 },

                new Activity { Name = "Act23", Description = "Reading",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), ModuleId = 3 },

                new Activity { Name = "Act24", Description = "Sleep",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), ModuleId = 3 }

            };


            act.ForEach(s => context.Activitys.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();


            // *****

        }
    }
}
