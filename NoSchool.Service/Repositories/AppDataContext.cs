using NoSchool.Service.Entities;
using System.Data.Entity;

namespace NoSchool.Service.Repositories
{
    class AppDataContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseTemplate> CourseTemplates { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentInCourse> StudentsInCourses { get; set; }

        public DbSet<WaitingList> WaitingLists { get; set; }
    }
}
