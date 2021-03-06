﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Norm
{
    public abstract class NormException : ArgumentException
    {
        public NormException(string message) : base(message) { }
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

    public class NormMultipleMappingsException : NormException
    {
        public NormMultipleMappingsException() :
            base(@"Multiple mappings requires types of same category: 
Classes with classes, records with records, value tuples with value tuple and simple types with simple types. 
For example pappings `Read<Class, int>` or `Read<int, Class>` are not permitted.")
        { }
    }
}
