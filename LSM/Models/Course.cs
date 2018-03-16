using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSM.Models
{
    public class Course
    {
        public int Id { get; set; }
        public int Place { get; set; }              // This is the place, 0 means no place.
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}