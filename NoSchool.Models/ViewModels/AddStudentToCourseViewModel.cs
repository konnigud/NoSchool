using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSchool.Models.ViewModels
{
    /// <summary>
    /// Contains the information needed to add a student to a course
    /// </summary>
    public class AddStudentToCourseViewModel
    {
        /// <summary>
        /// The social security number for the student
        /// </summary>
        [Required]
        [RegularExpression(@"^[0-9]{10}$")]
        public string SSN { get; set; }
    }
}
