using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public IEnumerable<IDictionary<string, object>> Read(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = command;
                EnsureConnectionIsOpen();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return Enumerable.Range(0, reader.FieldCount)
                            .ToDictionary(reader.GetName, reader.GetValue);
                    }
                }
            }
        }

        public IEnumerable<IDictionary<string, object>> Read(string command, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = command;
                EnsureConnectionIsOpen();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return Enumerable.Range(0, reader.FieldCount)
                            .ToDictionary(reader.GetName, reader.GetValue);
                    }
                }
            }
        }

        public IEnumerable<IDictionary<string, object>> Read(string command, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = command;
                EnsureConnectionIsOpen();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return Enumerable.Range(0, reader.FieldCount)
                            .ToDictionary(reader.GetName, reader.GetValue);
                    }
                }
            }
        }
    }
}
