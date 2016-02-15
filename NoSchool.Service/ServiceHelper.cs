using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSchool.Service
{
    internal static class ServiceHelper
    {
        internal static string CurrentSemester()
        {
            string semester;

            DateTime currentDate = DateTime.Now;
            semester = "20153";
            semester = currentDate.Year.ToString();
            if (currentDate.Month < 6)
            {
                semester += "1";
            }
            else if (currentDate.Month > 7)
            {
                semester += "3";
            }
            else
            {
                semester += "2";
            }

            return semester;
        }

        internal static bool ValidateSemester(string semester)
        {
        //    //check length
        //    if (semester.Length != 5)
        //        return false;

        //    for(int i = 0; i < 5; i++)
        //    {
        //        if (!Char.IsDigit(semester[i]))
        //            return false;

        //        if(i == 4)
        //        {
        //            int c = int.Parse(semester.Substring(4));
        //            if (c > 3)
        //                return false;
        //        }

            if (!System.Text.RegularExpressions.Regex.IsMatch(semester, @"^[0-9]{4}(1|2|3)$"))
            {
                return false;
            }

            return true;
        }
    }
}
