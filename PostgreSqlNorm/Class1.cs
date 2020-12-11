using System;
using Npgsql;
using Norm;

namespace PostgreSqlNorm
{
    public class Class1
    {
        public Class1()
        {
            using var connection = new NpgsqlConnection("");
            connection.Execute("");
        }
    }
}
