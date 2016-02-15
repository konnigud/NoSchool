using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSchool.Service.Entities
{
    [Table("Courses")]
    class Course
    {
        [Key]
        public int ID { get; set; }

        public string TemplateID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Semester { get; set; }

        public int MaxStudent { get; set; }
    }
}
