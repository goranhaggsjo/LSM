using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LSM.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]                       // 20180323
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime StopDate { get; set; }

        public virtual Module Module { get; set; }
        public int ModuleId { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}