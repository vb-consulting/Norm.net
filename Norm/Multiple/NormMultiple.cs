using System;
using System.Data;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Execute SQL command and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public NormMultipleReader Multiple(string command)
        {
            using var cmd = CreateCommand(command);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters, execute SQL command and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public NormMultipleReader MultipleFormat(FormattableString command)
        {
            using var cmd = CreateCommand(command);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public async ValueTask<NormMultipleReader> MultipleAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, this);
            }
            return new NormMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, this);
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters, execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public async ValueTask<NormMultipleReader> MultipleFormatAsync(FormattableString command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, this);
            }
            return new NormMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command with positional parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public NormMultipleReader Multiple(string command, params object[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command asynchronously and return disposable reader object for multiple result sets.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        ///<param name="parameters">Parameters objects array.</param>
        public async ValueTask<NormMultipleReader> MultipleAsync(string command, params object[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, this);
            }
            return new NormMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command with named parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public NormMultipleReader Multiple(string command, params (string name, object value)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public async ValueTask<NormMultipleReader> MultipleAsync(string command, params (string name, object value)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, this);
            }
            return new NormMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command with named parameter values and DbType type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public NormMultipleReader Multiple(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values and DbType type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name, value and type tuples array - (string name, object value, DbType type).</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public async ValueTask<NormMultipleReader> MultipleAsync(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, this);
            }
            return new NormMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command with named parameter values and custom type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>Disposable NormMultipleReader instance.</returns>
        public NormMultipleReader Multiple(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleReader(cmd.ExecuteReader(), cancellationToken, this);
        }

        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values and custom type for each parameter and return disposable reader object for multiple result sets..
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>A value task representing the asynchronous operation returning disposable NormMultipleReader instance.</returns>
        public async ValueTask<NormMultipleReader> MultipleAsync(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, this);
            }
            return new NormMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, this);
        }
    }
}
