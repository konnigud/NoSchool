using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSchool.Models.DTOModels
{
    public class CourseDetailDTO
    {
        /// <summary>
        /// The unique id of the course
        /// Examble: 99
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The course template for the course
        /// Examble: T-514-VEFT
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// The Name of the course
        /// Examble: Vefþjónustur
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The start date of the course
        /// Examble: 2015-1-1
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date of the course
        /// Examble: 2015-1-1
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The semester the course is taught
        /// Examble: 20153
        /// </summary>
        public string Semester { get; set; }

        /// <summary>
        /// The maximum allowed student to attend the class at once
        /// Examble: 50
        /// </summary>
        public int MaxStudent { get; set; }

        /// <summary>
        /// List of students attending the course
        /// </summary>
        public List<StudentDTO> StudentsInCourse { get; set; }

    }
}
