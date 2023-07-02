using System;

namespace Norm
{
    public abstract class NormException : ArgumentException
    {
        public NormException(string message) : base(message) { }
    }

    public class NormParametersException : NormException
    {
        public NormParametersException(string name) :
            base($"Parameter name \"{name}\" appears more than once. Parameter names must be unique when using positional parameters. Try using named parameters instead.")
        { }
    }

    public class NormPositionalParametersWithStoredProcedureException : NormException
    {
        public NormPositionalParametersWithStoredProcedureException() : 
            base("Cannot use positional parameters that are not DbParameter type with command type StoredProcedure. Use named parameters instead.") { }
    }
    
    public class NormValueTupleTooLongException : NormException
    {
        public NormValueTupleTooLongException() :
            base("Too many named tuple members. Maximum is 14.")
        { }
    }

    public class NormNullException : NormException
    {
        public NormNullException(string fieldName, string propertyName) :
            base($"Can't map null value for database field \"{fieldName}\" to non-nullable property \"{propertyName}\".")
        { }
    }

    public class NormReaderAlreadyAssignedException : NormException
    {
        public NormReaderAlreadyAssignedException() :
            base("DbReader callback has been already assigned. You can't assign it twice. It may be global handler in options or double call on WithReaderCallback method on same instance.")
        { }
    }
}
