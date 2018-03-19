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

            // En massa komplicerad kod för att skapa nya användare och roller !!


            var courses = new List<Course>   
            {
                new Course { Name = "Java", Description = "Coding Java",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01")},
                new Course { Name = "Python", Description = "Coding Pyhton",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01")},
                new Course { Name = "C#", Description = "Coding C#",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01")},
                new Course { Name = "Kurs med lång beskrivning", Description = "Proin tincidunt ullamcorper lectus, sit amet tempus neque maximus quis. Phasellus at lorem id nunc hendrerit congue quis eu odio. Praesent convallis vitae odio in posuere. Interdum et malesuada fames ac ante ipsum primis in faucibus. Proin blandit diam eu dignissim pharetra. In volutpat orci nec quam placerat, nec vehicula lacus vulputate. Nullam sagittis sollicitudin nisl, non blandit orci vestibulum at. Nullam leo est, finibus ut iaculis in, consectetur eu ligula. Integer nisi neque, tempus id orci ac, porttitor sodales nunc. Praesent pretium libero scelerisque, rhoncus purus in, consectetur risus. Praesent sollicitudin est nec lectus pretium, at bibendum purus volutpat. Quisque viverra imperdiet lacus, non volutpat metus porttitor eu. Praesent convallis neque odio, cursus pretium ipsum convallis eu. Vivamus ut metus id ante tristique fringilla id quis augue.Proin tincidunt ullamcorper lectus, sit amet tempus neque maximus quis. Phasellus at lorem id nunc hendrerit congue quis eu odio. Praesent convallis vitae odio in posuere. Interdum et malesuada fames ac ante ipsum primis in faucibus. Proin blandit diam eu dignissim pharetra. In volutpat orci nec quam placerat, nec vehicula lacus vulputate. Nullam sagittis sollicitudin nisl, non blandit orci vestibulum at. Nullam leo est, finibus ut iaculis in, consectetur eu ligula. Integer nisi neque, tempus id orci ac, porttitor sodales nunc. Praesent pretium libero scelerisque, rhoncus purus in, consectetur risus. Praesent sollicitudin est nec lectus pretium, at bibendum purus volutpat. Quisque viverra imperdiet lacus, non volutpat metus porttitor eu. Praesent convallis neque odio, cursus pretium ipsum convallis eu. Vivamus ut metus id ante tristique fringilla id quis augue.",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01")},
                new Course { Name = "Kurs med många moduler, studenter och aktiviter",   Description = "Kurs med många moduler, studenter och aktiviter", 
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01")},

        };
            var date = DateTime.Now;
            for (int index = 0; index < 15; index++)
            {
                date.AddMonths(1);

                courses.Add(new Course
                {
                    Name = "Kurs" + index,
                    Description = "Qwerty 123",
                    StartDate = date,
                    StopDate = date
                });
            }
        



            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var CourseMany = context.Courses.Where(m => m.Name == "Kurs med många moduler, studenter och aktiviter").First();
            var CourseJava = context.Courses.Where(m => m.Name == "Java").First();
            var CourseC = context.Courses.Where(m => m.Name == "C#").First();
            var CoursePython = context.Courses.Where(m => m.Name == "Python").First();




            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { "Teacher", "Student" };       // A list of different roles...
            foreach (var roleName in roleNames)
            {
                if (context.Roles.Any(r => r.Name == roleName)) continue;

                // Create new role
                var role = new IdentityRole { Name = roleName };
                var result2 = roleManager.Create(role);
                if (!result2.Succeeded)
                {
                    throw new Exception(string.Join("\n", result2.Errors));
                }
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var emails = new[] { "gandalf@aa.se", "galadriel@aa.se", "frodo@aa.se", "gimli@aa.se",
                 "knatte@aa.se",  "fnatte@aa.se",  "tjatte@aa.se" };
            var name = new[] { "Gandalf", "Galadriel", "Frodo", "Gimli",
                 "Knatte",  "Fnatte",  "Tjatte" };
            var lastname = new[] { "Gandalf", "Of the Forest", "Baggings", "Craftman",
                 "Anka",  "Anka",  "Anka" };

            string emailname;

            for (int i = 0; i < emails.Length; i++)
            {
                emailname = emails[i]; //Must be set here not in the lamba expression
                if (context.Users.Any(u => u.UserName == emailname)) continue;

                // Create user, with username and pwd
                // Update with FirstName, LastName
                DateTime t1 = DateTime.Now;
                var user = new ApplicationUser
                {
                    UserName = emails[i],
                    Email = emails[i],
                    FirstName = name[i],
                    LastName = lastname[i],
                    CourseId = CourseMany.Id
                };
                var result = userManager.Create(user, "Lsm123!");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }


            //int kingnumber = 1;
            //foreach (var email in emails)
            //{


            //    kingnumber++;
            //    if (context.Users.Any(u => u.UserName == emails[i])) continue;

            //    // Create user, with username and pwd
            //    // Update with FirstName, LastName
            //    DateTime t1 = DateTime.Now;
            //    var user = new ApplicationUser
            //    {

            //        UserName = email,
            //        Email = email,
            //        FirstName = "Karl",
            //        LastName = kingnumber.ToString(),
            //        CourseId = CourseMany.Id
            //    };
            //    var result = userManager.Create(user, "Lsm123!");
            //    if (!result.Succeeded)
            //    {
            //        throw new Exception(string.Join("\n", result.Errors));
            //    }
            //}




            var u1 = userManager.FindByName("gandalf@aa.se");
            userManager.AddToRole(u1.Id, "Student");


            var u2 = userManager.FindByName("galadriel@aa.se");
            userManager.AddToRole(u2.Id, "Student");


            var u3 = userManager.FindByName("frodo@aa.se");
            userManager.AddToRoles(u3.Id, "Student");


            var u4 = userManager.FindByName("gimli@aa.se");
            userManager.AddToRoles(u4.Id, "Student");


            var u5 = userManager.FindByName("knatte@aa.se");
            userManager.AddToRoles(u5.Id, "Student");


            var u6 = userManager.FindByName("fnatte@aa.se");
            userManager.AddToRoles(u6.Id, "Student");


            var u7 = userManager.FindByName("tjatte@aa.se");
            userManager.AddToRoles(u7.Id, "Student");
            //Skippat från alla rader u7.CourseId = 5;


            //var JavaDude = new ApplicationUser
            //{
            //    UserName = "java@lsm.se",
            //    Email = "java@lsm.se",
            //    FirstName = "Java",
            //    LastName = "Dude",
            //    CourseId = CourseC.Id
            //};
            //userManager.Create(JavaDude, "Lsm123!");
            //var u8 = userManager.FindByName("java@lsm.se");

            //userManager.AddToRoles(u8.Id, "Student");

            //var Teacher = new ApplicationUser
            //{
            //    UserName = "larare@lsm.se",
            //    Email = "larare@lsm.se",
            //    FirstName = "Lärare",
            //    LastName = "Lärare",
            //    CourseId = null
            //};
            //userManager.Create(Teacher, "Lsm123!");
            //var u9 = userManager.FindByName("larare@lsm.se");
            //userManager.AddToRoles(u9.Id, "Teacher");

            //var PythonDude = new ApplicationUser
            //{
            //    UserName = "python@lsm.se",
            //    Email = "python@lsm.se",
            //    FirstName = "Python",
            //    LastName = "Dude",
            //    CourseId = CoursePython.Id
            //};
            //userManager.Create(JavaDude, "Lsm123!");
            //var u10 = userManager.FindByName("python@lsm.se");

            //var CDude = new ApplicationUser
            //{
            //    UserName = "cdude@lsm.se",
            //    Email = "cdude@lsm.se",
            //    FirstName = "C#",
            //    LastName = "Dude",
            //    CourseId = CourseC.Id
            //};
            //userManager.Create(CDude, "Lsm123!");
            //var u11 = userManager.FindByName("cdude@lsm.se");

            var modules = new List<Module>

            {
                new Module { Name = "Manga Aktivititer", Description = "First introduction",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = CourseMany.Id },
                new Module { Name = "Part1", Description = "First introduction",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = CourseJava.Id },
                new Module { Name = "Part2", Description = "Going uphill",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = CourseJava.Id },
                new Module { Name = "Part3", Description = "Go downhill",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = CourseC.Id },
                new Module { Name = "Part4", Description = "Go forward",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = CourseC.Id },
                new Module { Name = "Part5", Description = "Run back",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = CoursePython.Id },
                new Module { Name = "Part6", Description = "Start flying",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = CoursePython.Id },
                new Module { Name = "Part7", Description = "Take a walk",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), CourseId = CoursePython.Id }

            };


            var dateModule = DateTime.Now;
            for (int index = 0; index < 15; index++)
            {
                dateModule.AddMonths(1);

                modules.Add(new Module
                {
                    Name = "Module" + index,
                    Description = "Qwerty 123",
                    StartDate = date,
                    StopDate = date,
                    CourseId = CourseMany.Id

                });
            }

           

            modules.ForEach(s => context.Modules.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
            var ActivityMany = context.Modules.Where(m => m.Name == "Manga Aktivititer").First();
           
            



            var act = new List<Activity>
            {
                new Activity { Name = "Act20", Description = "Try whiskey",
                    StartDate = DateTime.Now, StopDate = DateTime.Now, ModuleId = ActivityMany.Id },
                new Activity { Name = "Act21", Description = "Go home",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), ModuleId = ActivityMany.Id },
                new Activity { Name = "Act22", Description = "Drink beer",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), ModuleId = ActivityMany.Id },
                new Activity { Name = "Act23", Description = "Reading",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), ModuleId = ActivityMany.Id },
                new Activity { Name = "Act24", Description = "Sleep",
                    StartDate = DateTime.Parse("2010-09-01"), StopDate = DateTime.Parse("2010-09-01"), ModuleId = ActivityMany.Id }


            };

            for (int index = 0; index < 15; index++)
            {


                act.Add(new Activity
                {
                    Name = "Module" + index,
                    Description = "Qwerty 123",
                    StartDate = date,
                    StopDate = date,
                    ModuleId = ActivityMany.Id

                });
            }

            act.ForEach(s => context.Activitys.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();


            //context.Courses.Find(p => p.Name == "Tjoho");

        }
    }
}
