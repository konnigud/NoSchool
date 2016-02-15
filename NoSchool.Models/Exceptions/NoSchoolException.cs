using System;

namespace NoSchool.Models.Exceptions
{
    /// <summary>
    /// The basic Exception type for all No school exceptions
    /// </summary>
    public abstract class NoSchoolException : ApplicationException
    {
        public NoSchoolException() { }

        public NoSchoolException(string message) : base(message) { }
        
    }

    /// <summary>
    /// InternalService error 500
    /// </summary>
    public class NoSchoolInternalException: NoSchoolException
    {
        public NoSchoolInternalException() { }

        public NoSchoolInternalException(string message) : base(message) { }
    }

    /// <summary>
    /// NotFound error 404
    /// </summary>
    public class NoSchoolNotFoundException : NoSchoolException
    {
        public NoSchoolNotFoundException() { }

        public NoSchoolNotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// Precondition failed error 412
    /// </summary>
    public class NoSchoolPreconditionFailedException : NoSchoolException
    {
        public NoSchoolPreconditionFailedException() { }

        public NoSchoolPreconditionFailedException(string message) : base(message) { }

    }
}
