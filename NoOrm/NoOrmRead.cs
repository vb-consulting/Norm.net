using System.Collections.Generic;
using NoOrm.Extensions;


namespace NoOrm
{
    public partial class NoOrm
    {
        public IEnumerable<IEnumerable<(string name, object value)>> Read(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                Connection.EnsureIsOpen();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader.GetTuplesFromReader();
                    }
                }
            }
        }

        public IEnumerable<IEnumerable<(string name, object value)>> Read(string command, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                Connection.EnsureIsOpen();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader.GetTuplesFromReader();
                    }
                }
            }
        }

        public IEnumerable<IEnumerable<(string name, object value)>> Read(string command, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                Connection.EnsureIsOpen();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader.GetTuplesFromReader();
                    }
                }
            }
        }
    }
}
