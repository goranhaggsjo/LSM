using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSM.Models
{
    public class userview
    {
        public int Id { get; set; }
        public int CourseId { get; set; }       // New 180321
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string Role { get; set; }
        public string CourseName { get; set; }     // New 180323

    }
}