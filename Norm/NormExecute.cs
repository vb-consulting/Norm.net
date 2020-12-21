using System.Data;
using Norm.Interfaces;

namespace Norm
{
    public partial class Norm
    {
        public INorm Execute(string command)
        {
            using var cmd = CreateCommand(command);
            cmd.ExecuteNonQuery();
            return this;
        }

        public INorm Execute(string command, params object[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            cmd.ExecuteNonQuery();
            return this;
        }

        public INorm Execute(string command, params (string name, object value)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            cmd.ExecuteNonQuery();
            return this;
        }

        public INorm Execute(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            cmd.ExecuteNonQuery();
            return this;
        }

        public INorm Execute(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            cmd.ExecuteNonQuery();
            return this;
        }
    }
}
