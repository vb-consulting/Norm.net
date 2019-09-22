using System;
using System.Data.Common;


namespace NoOrm
{
    public partial class NoOrmAccess
    {
        public INoOrm Execute(string command)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                cmd.ExecuteNonQuery();
                return this;
            }
        }

        public INoOrm Execute(string command, params object[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                cmd.AddParameters(parameters).ExecuteNonQuery();
                return this;
            }
        }

        public INoOrm Execute(string command, params (string name, object value)[] parameters)
        {
            using (var cmd = Connection.CreateCommand())
            {
                SetCommand(cmd, command);
                EnsureConnectionIsOpen();
                cmd.AddParameters(parameters).ExecuteNonQuery();
                return this;
            }
        }
    }
}
