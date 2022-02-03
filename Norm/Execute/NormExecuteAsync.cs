using System;
using System.Data;
using System.Threading.Tasks;

namespace Norm
{
    public partial class Norm
    {
        ///<summary>
        ///     Execute SQL command asynchronously.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<returns>A value task representing the asynchronous operation returning the same Norm instance.</returns>
        public async ValueTask<Norm> ExecuteAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            await cmd.ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        ///<summary>
        ///     Parse interpolated (formattable) command as database parameters and execute resulting SQL asynchronously.
        ///</summary>
        ///<param name="command">SQL command text as interpolated (formattable) string.</param>
        ///<returns>A value task representing the asynchronous operation returning the same Norm instance.</returns>
        public async ValueTask<Norm> ExecuteFormatAsync(FormattableString command)
        {
            using var cmd = await CreateCommandAsync(command);
            await cmd.ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        ///<summary>
        ///      Execute SQL command asynchronously with positional parameter values.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters objects array.</param>
        ///<returns>A value task representing the asynchronous operation returning the same Norm instance.</returns>
        public async ValueTask<Norm> ExecuteAsync(string command, params object[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            await cmd.ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">Parameters name and value tuples array - (string name, object value).</param>
        ///<returns>A value task representing the asynchronous operation returning the same Norm instance.</returns>
        public async ValueTask<Norm> ExecuteAsync(string command, params (string name, object value)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            await cmd.ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }

        ///<summary>
        ///     Execute SQL command asynchronously with named parameter values and custom type for each parameter.
        ///</summary>
        ///<param name="command">SQL command text.</param>
        ///<param name="parameters">
        ///     Parameters name, value and type tuples array - (string name, object value, DbType type).
        ///     Parameter type can be any type from custom db provider -  NpgsqlDbType or MySqlDbType for example.
        ///</param>
        ///<returns>A value task representing the asynchronous operation returning the same Norm instance.</returns>
        public async ValueTask<Norm> ExecuteAsync(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            await cmd.ExecuteNonQueryWithOptionalTokenAsync(cancellationToken);
            return this;
        }
    }
}
