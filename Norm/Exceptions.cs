using System;
using System.Collections.Generic;
using System.Text;

namespace Norm
{
    public abstract class NormException : ArgumentException
    {
        public NormException(string message) : base(message) { }
    }

    public class NormParametersException : NormException
    {
        public NormParametersException(string name) :
            base($"Parameter name \"{name}\" appears more than once. Parameter names must be unique.")
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

    public class NormCannotUsePostgresFormatParamsModeOnPreparedStatementException : NormException
    {
        public NormCannotUsePostgresFormatParamsModeOnPreparedStatementException() :
            base("Cannot set UsingPostgresFormatParamsMode on prepared statements.")
        { }
    }

    public class NormCannotUsePostgresFormatParamsModeWhenNotPostgreSqlException : NormException
    {
        public NormCannotUsePostgresFormatParamsModeWhenNotPostgreSqlException() :
            base("Cannot set UsingPostgresFormatParamsMode on connection other than PostgreSQL.")
        { }
    }

    public class NormWrongTypeParameterException : NormException
    {
        public NormWrongTypeParameterException(string name) :
            base($"Command parameter with following name: \"{name}\" has invalid database type. Database types must be enum like DbType or custom custom database provider enum like SqlDbType, or its int representation.")
        { }
    }

}
