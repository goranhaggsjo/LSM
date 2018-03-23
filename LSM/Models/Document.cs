using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSM.Models
{
    public class Document
    {
        public int Id { get; set; }
        public int Place { get; set; }              // Place in the list, 0 means no place yet
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public String Filepath { get; set; }
        
        public virtual ApplicationUser User { get; set; }
        public int? ApplicationUserId { get; set; }

        public virtual Course Course { get; set; }
        public int? CourseId { get; set; }

        public virtual Module Module { get; set; }
        public int? ModuleId { get; set; }

        public virtual Activity Activity { get; set; }
        public int? ActivityId { get; set; }

    }
}