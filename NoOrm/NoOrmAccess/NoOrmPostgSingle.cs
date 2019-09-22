using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public IDictionary<string, object> Single(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read()
                        ? Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue)
                        : new Dictionary<string, object>();
                }
            }
        }

        public IDictionary<string, object> Single(string command, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() 
                        ? Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue) 
                        : new Dictionary<string, object>();
                }
            }
        }

        public IDictionary<string, object> Single(string command, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read()
                        ? Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue)
                        : new Dictionary<string, object>();
                }
            }
        }
    }
}
