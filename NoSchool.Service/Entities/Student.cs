using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSchool.Service.Entities
{
    [Table("Students")]
    class Student
    {
        public int ID { get; set; }

        public string SSN { get; set; }

        public string Name { get; set; }
    }
}
