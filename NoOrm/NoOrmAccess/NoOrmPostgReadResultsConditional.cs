using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public INoOrm Read(string command, Func<IDictionary<string, object>, bool> results)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = command;
                EnsureConnectionIsOpen();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var result = results(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                        if (!result)
                        {
                            break;
                        }
                    }

                    return this;
                }
            }
        }

        public INoOrm Read(string command, Func<IDictionary<string, object>, bool> results, params object[] parameters)
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
                        var result = results(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                        if (!result)
                        {
                            break;
                        }
                    }

                    return this;
                }
            }
        }

        public INoOrm Read(string command, Func<IDictionary<string, object>, bool> results, params (string name, object value)[] parameters)
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
                        var result = results(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                        if (!result)
                        {
                            break;
                        }
                    }

                    return this;
                }
            }
        }
    }
}
