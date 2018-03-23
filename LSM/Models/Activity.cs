using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LSM.Models
{
    public enum Epass { FM = 1, EM, FMEM };

    public class Activity
    {
        public int Id { get; set; }
        public int Place { get; set; }              // In the list, this is the place. 0 is no place.
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Day { get; set; }
        public Epass Pass { get; set; }

        public virtual Module Module { get; set; }  // Sign this is the foreign key
        public int ModuleId { get; set; }           // And this is the key

        public virtual ICollection<Document> Documents { get; set; }
    }

}