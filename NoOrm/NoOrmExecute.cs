using NoOrm.Extensions;

namespace NoOrm
{
    public partial class NoOrm
    {
        public INoOrm Execute(string command)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            cmd.ExecuteNonQuery();
            return this;
        }

        public INoOrm Execute(string command, params object[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            cmd.AddParameters(parameters).ExecuteNonQuery();
            return this;
        }

        public INoOrm Execute(string command, params (string name, object value)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            cmd.AddParameters(parameters).ExecuteNonQuery();
            return this;
        }
    }
}
