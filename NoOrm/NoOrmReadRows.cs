using System.Linq;
using NoOrm.Extensions;

namespace NoOrm
{
    public partial class NoOrm
    {
        public INoOrm Read(string command, RowCallback results)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                Connection.EnsureIsOpen();
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
                Connection.EnsureIsOpen();
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
                Connection.EnsureIsOpen();
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
