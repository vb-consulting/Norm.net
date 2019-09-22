using System.Linq;

namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public INoOrm Read(string command, RowCallback results)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results(reader.GetValuesFromReader().ToArray());
                    }
                    return this;
                }
            }
        }

        public INoOrm Read(string command, RowCallback results, params object[] parameters)
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
                        results(reader.GetValuesFromReader().ToArray());
                    }

                    return this;
                }
            }
        }

        public INoOrm Read(string command, RowCallback results, params (string name, object value)[] parameters)
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
                        results(reader.GetValuesFromReader().ToArray());
                    }

                    return this;
                }
            }
        }
    }
}
