using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Norm.Extensions;

namespace Norm
{
    public partial class Norm
    {
        private IEnumerable<T> ReadInternal<T>(string command, Func<DbDataReader, T> readerAction)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            Prepare(cmd);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }

        private IEnumerable<(string name, object value)[]> ReadToArrayInternal(string command)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            Prepare(cmd);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return reader.ToArray();
            }
        }

        private IEnumerable<T> ReadInternal<T>(string command, Func<DbDataReader, T> readerAction, params object[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }

        private IEnumerable<(string name, object value)[]> ReadToArrayInternal(string command, params object[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return reader.ToArray();
            }
        }

        private IEnumerable<T> ReadInternal<T>(string command, Func<DbDataReader, T> readerAction, params (string name, object value)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }

        private IEnumerable<(string name, object value)[]> ReadToArrayInternal(string command, params (string name, object value)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return reader.ToArray();
            }
        }

        private IEnumerable<T> ReadInternal<T>(string command, Func<DbDataReader, T> readerAction, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }

        private IEnumerable<(string name, object value)[]> ReadToArrayInternal(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParameters(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return reader.ToArray();
            }
        }

        private IEnumerable<T> ReadInternalUnknowParamsType<T>(string command, Func<DbDataReader, T> readerAction, params (string name, object value, object type)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParametersUnknownType(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return readerAction(reader);
            }
        }

        private IEnumerable<(string name, object value)[]> ReadToArrayInternalUnknowParamsType(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = Connection.CreateCommand();
            SetCommand(cmd, command);
            Connection.EnsureIsOpen();
            AddParametersUnknownType(cmd, parameters);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return reader.ToArray();
            }
        }
    }
}