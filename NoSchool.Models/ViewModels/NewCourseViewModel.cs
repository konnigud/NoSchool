using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NoSchool.Models.ViewModels
{
    /// <summary>
    /// Information needed to create a new course
    /// </summary>
    public class NewCourseViewModel
    {
        /// <summary>
        /// The type of course to create
        /// Examble: T-514-VEFT
        /// </summary>
        [Required]
        public string TemplateID { get; set; }

        /// <summary>
        /// Start date of the course
        /// Examble: 2015-1-1
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the course
        /// Examble: 2015-1-1
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The semester the course is taught
        /// Examble: 20153
        /// </summary>
        [Required]
        [RegularExpression(@"^[0-9]{4}(1|2|3)$", ErrorMessage = "Semester not of the right format.")]
        public string Semester { get; set; }

        /// <summary>
        /// The maximum number of stundents that can attend the course at a time.
        /// </summary>
        public int MaxStudents { get; set; }

    }
}
