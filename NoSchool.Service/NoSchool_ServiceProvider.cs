using NoSchool.Models.DTOModels;
using NoSchool.Models.Exceptions;
using NoSchool.Models.ViewModels;
using NoSchool.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSchool.Service
{
    /// <summary>
    /// The business logic for all course related requests
    /// </summary>
    public class CourseServiceProvider
    {

        #region Privte members and constructors
        private readonly AppDataContext _db;

        /// <summary>
        /// A default constructor for the service
        /// </summary>
        public CourseServiceProvider()
        {
            _db = new AppDataContext();
        }
        #endregion


        /// <summary>
        /// A service
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
        public List<CourseDTO> GetCourses(string semester = null )
        {
            try
            {
                if (String.IsNullOrWhiteSpace(semester))
                    semester = ServiceHelper.CurrentSemester();
                else
                {
                    if (!ServiceHelper.ValidateSemester(semester))
                        throw new NoSchoolPreconditionFailedException();
                }

                var result = (from courses in _db.Courses
                              join templates in _db.CourseTemplates on courses.TemplateID equals templates.TemplateID
                              join sic in _db.StudentsInCourses on courses.ID equals sic.CourseID into c_sic
                              where courses.Semester == semester
                              select new CourseDTO
                              {
                                  ID = courses.ID,
                                  Template = courses.TemplateID,
                                  Name = templates.TemplateID,
                                  EndDate = courses.EndDate,
                                  StartDate = courses.StartDate,
                                  MaxStudent = courses.MaxStudent,
                                  StudentInCourse = c_sic.Select(x => x.IsActive == true).Count()
                              }).ToList();

                return result;
            }
            catch(NoSchoolException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new NoSchoolInternalException();
            }
        } 

        /// <summary>
        /// Adds the new course to the database with unique id created by the database
        /// </summary>
        /// <param name="newCourse"></param>
        /// <returns>the new course</returns>
        public CourseDTO CreateCourse(NewCourseViewModel newCourse)
        {
            try
            {
                var template = _db.CourseTemplates.SingleOrDefault(x => newCourse.TemplateID == x.TemplateID);
                if (template == null)
                {
                    throw new NoSchoolPreconditionFailedException();
                }

                Entities.Course course = new Entities.Course();
                course.TemplateID = template.TemplateID;
                course.Semester = newCourse.Semester;
                course.StartDate = newCourse.StartDate;
                course.EndDate = newCourse.EndDate;
                course.MaxStudent = newCourse.MaxStudents;

                _db.Courses.Add(course);
                _db.SaveChanges();

                return new CourseDTO
                {
                    ID = course.ID,
                    Template = template.TemplateID,
                    Name = template.Name,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate,
                    Semester = course.Semester,
                    MaxStudent = course.MaxStudent,
                    StudentInCourse = 0
                };
            }
            catch(NoSchoolException)
            {
                throw;
            }
            catch
            {
                throw new NoSchoolInternalException();
            }
        }

        /// <summary>
        /// Retrives detailed information about a course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseDetailDTO GetCourseByID(int id)
        {
            try
            {
                var result = (from c in _db.Courses
                              join ct in _db.CourseTemplates on c.TemplateID equals ct.TemplateID
                              where c.ID == id
                              select new CourseDetailDTO
                              {
                                  ID = c.ID,
                                  Template = c.TemplateID,
                                  Name = ct.Name,
                                  StartDate = c.StartDate,
                                  EndDate = c.EndDate,
                                  Semester = c.Semester,
                                  MaxStudent = c.MaxStudent,
                              }).SingleOrDefault();

                if (result == null)
                    return result;

                result.StudentsInCourse = (from s in _db.Students
                                           join sc in _db.StudentsInCourses on s.ID equals sc.StudentID
                                           where sc.CourseID == id
                                           where sc.IsActive == true
                                           select new StudentDTO
                                           {
                                               Name = s.Name,
                                               SSN = s.SSN
                                           }).ToList();

                return result;
            }
            catch (NoSchoolException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new NoSchoolInternalException();
            }
        }

        /// <summary>
        /// Adds a student to a course if both exist
        /// Throws error if student already in course or no room for more students
        /// If student is on the waiting list he will be removed from there if registerd
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studentInfo"></param>
        /// <returns></returns>
        public StudentDTO AddStudentToCourse(int id, AddStudentToCourseViewModel studentInfo)
        {
           

            //checkif student exists
            var student = _db.Students.SingleOrDefault(x => x.SSN == studentInfo.SSN);
            if (student == null)
                throw new NoSchoolNotFoundException("Person not found");

            //checkif course exists
            var course = _db.Courses.SingleOrDefault(x => x.ID == id);
            if (course == null)
                throw new NoSchoolNotFoundException("Course not found");

            //checkif course is full
            var studentsInCourse = _db.StudentsInCourses.Where(x => x.CourseID == course.ID).Count(x => x.IsActive == true);
            if (course.MaxStudent <= studentsInCourse)
                throw new NoSchoolPreconditionFailedException("Max students reached");

            //check registration status of student
            var registration = _db.StudentsInCourses.Where(x => x.CourseID == course.ID).SingleOrDefault(x => x.StudentID == student.ID);
            if (registration != null && registration.IsActive)
                throw new NoSchoolPreconditionFailedException("Student already registered");

            //Update registration status of student
            if (registration == null)
            {
                registration = new Entities.StudentInCourse();
                registration.CourseID = course.ID;
                registration.StudentID = student.ID;
                registration.IsActive = true;
                _db.StudentsInCourses.Add(registration);
            }
            else
            {
                registration.IsActive = true;
            }

            //Check if student is on waiting list and remove if needed
            var waitingStatus = _db.WaitingLists.Where(x => x.CourseID == course.ID).SingleOrDefault(x => x.StudentID == student.ID);
            if(waitingStatus != null)
            {
                _db.WaitingLists.Remove(waitingStatus);
            }

            _db.SaveChanges();

            return new StudentDTO { Name = student.Name, SSN = student.SSN};
        }

        /// <summary>
        /// removes student from class if he is active.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ssn"></param>
        public void RemoveStudent(int id, string ssn)
        {
            //get and check course
            var course = _db.Courses.Where(x => x.ID == id);
            if (course == null)
                throw new NoSchoolNotFoundException();

            //get student
            var student = _db.Students.SingleOrDefault(x => x.SSN == ssn);
            if (student == null)
                throw new NoSchoolNotFoundException();

            var inCourse = _db.StudentsInCourses.Where(x => x.CourseID == id).Where(x => x.StudentID == student.ID).SingleOrDefault(x => x.IsActive);

            if(inCourse != null)
            {
                inCourse.IsActive = false;
                _db.SaveChanges();
            }

            return;

        }

        /// <summary>
        /// Gets the list of all students in a given course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<StudentDTO> GetStudents(int id)
        {
            var course = _db.Courses.SingleOrDefault(x => x.ID == id);
            if (course == null)
                throw new NoSchoolNotFoundException();

            return (from s in _db.Students
                    join sc in _db.StudentsInCourses on s.ID equals sc.StudentID
                    where sc.CourseID == id
                    where sc.IsActive == true
                    select new StudentDTO
                    {
                        Name = s.Name,
                        SSN = s.SSN
                    }).ToList();
        }

        /// <summary>
        /// Retrives all students on a waiting list for course.id = id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<StudentDTO> GetWaitingList(int id)
        {
            var course = _db.Courses.SingleOrDefault(x => x.ID == id);
            if (course == null)
                throw new NoSchoolNotFoundException();

            return (from s in _db.Students
                    join sc in _db.WaitingLists on s.ID equals sc.StudentID
                    where sc.CourseID == id
                    select new StudentDTO
                    {
                        Name = s.Name,
                        SSN = s.SSN
                    }).ToList();
        }

        //If student is not enrolled in course nor on the waiting list he will be added
        public void AddStudentToWaitingList(int id,AddStudentToCourseViewModel studentToAdd)
        {
            // get course
            var course = _db.Courses.SingleOrDefault(x => x.ID == id);
            if (course == null)
                throw new NoSchoolNotFoundException();

            //get student
            var student = _db.Students.SingleOrDefault(x => x.SSN == studentToAdd.SSN);
            if (student == null)
                throw new NoSchoolNotFoundException();

            //check if student is in course
            var inCourse = _db.StudentsInCourses.Where(x => x.StudentID == student.ID).SingleOrDefault(x => x.IsActive);
            if (inCourse != null)
                throw new NoSchoolPreconditionFailedException();

            //Check if student is on waiting list and uptade list accordingly
            var waitingListStatus = _db.WaitingLists.Where(x => x.CourseID == course.ID).SingleOrDefault(x => x.StudentID == student.ID);
            if(waitingListStatus != null)
            {
                throw new NoSchoolPreconditionFailedException();
            }

            waitingListStatus = new Entities.WaitingList();
            waitingListStatus.CourseID = course.ID;
            waitingListStatus.StudentID = student.ID;

            _db.WaitingLists.Add(waitingListStatus);
            _db.SaveChanges();


        }
    }
}
