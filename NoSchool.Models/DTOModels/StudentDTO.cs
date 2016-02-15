using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSchool.Models.DTOModels
{
    /// <summary>
    /// Student basic information
    /// </summary>
    public class StudentDTO
    {
        /// <summary>
        /// The social security of the student
        /// Examble: "1234567890"
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// The students full name
        /// </summary>
        public string Name { get; set; }
    }
}
