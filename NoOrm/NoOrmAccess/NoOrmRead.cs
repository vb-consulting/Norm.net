using System.Collections.Generic;


namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public IEnumerable<IEnumerable<(string name, object value)>> Read(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
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
                EnsureConnectionIsOpen();
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
                EnsureConnectionIsOpen();
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
