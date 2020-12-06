using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Norm.Extensions;

namespace Norm
{
    public partial class Norm
    {
        private T SingleInternal<T>(string command, Func<DbDataReader, T> readerAction)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            Prepare(cmd);
            using var reader = cmd.ExecuteReader();
            return readerAction(reader);
        }

        private T SingleInternal<T>(string command, Func<DbDataReader, T> readerAction, params object[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            return readerAction(reader);
        }

        private T SingleInternal<T>(string command, Func<DbDataReader, T> readerAction, params (string name, object value)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            return readerAction(reader);
        }

        private T SingleInternal<T>(string command, Func<DbDataReader, T> readerAction, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            return readerAction(reader);
        }

        private T SingleInternalUnknowParamsType<T>(string command, Func<DbDataReader, T> readerAction, params (string name, object value, object type)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParametersUnknownType(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            return readerAction(reader);
        }

        private (string name, object value)[] SingleToArrayInternal(string command)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            Prepare(cmd);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.ToArray();
            }
            return Array.Empty<(string name, object value)>();
        }

        private (string name, object value)[] SingleToArrayInternal(string command, params object[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.ToArray();
            }
            return Array.Empty<(string name, object value)>();
        }

        private (string name, object value)[] SingleToArrayInternal(string command, params (string name, object value)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.ToArray();
            }
            return Array.Empty<(string name, object value)>();
        }

        private (string name, object value)[] SingleToArrayInternal(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.ToArray();
            }
            return Array.Empty<(string name, object value)>();
        }

        private (string name, object value)[] SingleToArrayInternalUnknowParamsType(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParametersUnknownType(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.ToArray();
            }
            return Array.Empty<(string name, object value)>();
        }
    }
}
