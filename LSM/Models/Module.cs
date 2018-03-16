﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSM.Models
{
    public class Module
    {
        public int Id { get; set; }
        public int Place { get; set; }              // Place int lists, 
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }

        public virtual Course Course { get; set; }
        public int CourseId { get; set; }

        public virtual ICollection<Activity> Activitys { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}

// This is how you tell EF that you want a foreign key.

//public class MemberDataSet
//{
//    [Key]
//    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
//    public int Id { get; set; }

//    public int? DeferredDataId { get; set; }
//    [ForeignKey("DeferredDataId")]
//    public virtual DeferredData DeferredData { get; set; }
//}