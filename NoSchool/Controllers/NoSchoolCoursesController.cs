using System.Collections.Generic;
using System.Web.Http;
using System.Net;
using NoSchool.Models.DTOModels;
using NoSchool.Service;
using NoSchool.Models.Exceptions;
using NoSchool.Models.ViewModels;

namespace NoSchool.Controllers
{

    /// <summary>
    /// A controller for actions for the coureses part of NoSchool
    /// api/coureses
    /// </summary>
    [RoutePrefix("api/courses")]
    public class NoSchoolCoursesController : ApiController
    {
        private CourseServiceProvider _service;

        public NoSchoolCoursesController()
        {
            _service = new CourseServiceProvider();
        }

        /// <summary>
        /// Get all courses on a semester
        /// If semester is not included, the current semester is checked
        /// </summary>
        /// <param name="semester">Examble: "20153"</param>
        /// <returns>List of courses</returns>
        [HttpGet]
        [Route("")]
        public List<CourseDTO> GetCourses(string semester = null)
        {
            try
            {
                return _service.GetCourses(semester);
            }
            catch(NoSchoolException nex)
            {                   
                if (nex is NoSchoolNotFoundException)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                if (nex is NoSchoolPreconditionFailedException)
                    throw new HttpResponseException(HttpStatusCode.PreconditionFailed);

                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
        
        /// <summary>
        /// Creates a new course with a predifined template
        /// </summary>
        /// <param name="newCourse"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateCourse(NewCourseViewModel newCourse)
        {
            if(!ModelState.IsValid)
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }


            try
            {
                var createdCourse = _service.CreateCourse(newCourse);
                var location = Url.Link("GetCourse", new { id = createdCourse.ID });
                return Created(location, createdCourse);
            }
            catch (NoSchoolException nex)
            {
                
                if (nex is NoSchoolPreconditionFailedException)
                    return StatusCode(HttpStatusCode.PreconditionFailed);

                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets a course with the id value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}", Name = "GetCourse")]
        public IHttpActionResult GetCourse(int id)
        {
            try
            {
                var result = _service.GetCourseByID(id);
                if(result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (NoSchoolException nex)
            {

                if (nex is NoSchoolPreconditionFailedException)
                    return StatusCode(HttpStatusCode.PreconditionFailed);

                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("{id:int}/students")]
        public IHttpActionResult GetAllStudentInCourse(int id)
        {
            try
            {
                var result = _service.GetStudents(id);
                return Ok(result);
            }
            catch (NoSchoolException nex)
            {
                if (nex is NoSchoolNotFoundException)
                    return NotFound();

                if (nex is NoSchoolPreconditionFailedException)
                    return StatusCode(HttpStatusCode.PreconditionFailed);

                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Adds a student to course if the student is not already in the course or the course is full
        /// Student must exist
        /// </summary>
        /// <param name="id">Id of the course</param>
        /// <param name="studentToAdd">Student info that is needed</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id:int}/students")]
        public IHttpActionResult AddStudentToCourse(int id, AddStudentToCourseViewModel studentToAdd)
        {
            try
            {
                var student = _service.AddStudentToCourse(id, studentToAdd);
                return Created("",student);
            }
            catch (NoSchoolException nex)
            {
                if (nex is NoSchoolNotFoundException)
                    return NotFound();

                if (nex is NoSchoolPreconditionFailedException)
                    return StatusCode(HttpStatusCode.PreconditionFailed);

                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Removes a student form a course if he is registerd
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ssn"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}/students/{SSN}")]
        public IHttpActionResult RemoveStudentFromCourse(int id, string ssn)
        {
            try
            {
                _service.RemoveStudent(id, ssn);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (NoSchoolException nex)
            {
                if (nex is NoSchoolNotFoundException)
                    return NotFound();

                if (nex is NoSchoolPreconditionFailedException)
                    return StatusCode(HttpStatusCode.PreconditionFailed);

                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Returns all student in waiting list for course with ID = id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}/waitinglist")]
        public IHttpActionResult GetWaitingList(int id)
        {
            try
            {
                var result = _service.GetWaitingList(id);
                return Ok(result);
            }
            catch (NoSchoolException nex)
            {
                if (nex is NoSchoolNotFoundException)
                    return NotFound();

                if (nex is NoSchoolPreconditionFailedException)
                    return StatusCode(HttpStatusCode.PreconditionFailed);

                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Adds a student to the waiting list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id:int}/waitinglist")]
        public IHttpActionResult AddToWaitingList(int id,AddStudentToCourseViewModel student)
        {
            try
            {
                _service.AddStudentToWaitingList(id,student);
                return Ok();
            }
            catch (NoSchoolException nex)
            {
                if (nex is NoSchoolNotFoundException)
                    return NotFound();

                if (nex is NoSchoolPreconditionFailedException)
                    return StatusCode(HttpStatusCode.PreconditionFailed);

                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}
