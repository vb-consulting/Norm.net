using System.Collections.Generic;
using System.Linq;


namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public IEnumerable<(string name, object value)> Single(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read()
                        ? reader.GetTuplesFromReader().ToList()
                        : new List<(string name, object value)>();
                }
            }
        }

        public IEnumerable<(string name, object value)> Single(string command, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() 
                        ? reader.GetTuplesFromReader().ToList()
                        : new List<(string name, object value)>(); ;
                }
            }
        }

        public IEnumerable<(string name, object value)> Single(string command, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                cmd.AddParameters(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read()
                        ? reader.GetTuplesFromReader().ToList()
                        : new List<(string name, object value)>(); ;
                }
            }
        }
    }
}
