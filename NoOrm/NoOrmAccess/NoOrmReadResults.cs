using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public INoOrm Read(string command, Action<IDictionary<string, object>> results)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                    }
                    return this;
                }
            }
        }

        public INoOrm Read(string command, Action<IDictionary<string, object>> results, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                    }

                    return this;
                }
            }
        }

        public INoOrm Read(string command, Action<IDictionary<string, object>> results, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                    }

                    return this;
                }
            }
        }
    }
}
