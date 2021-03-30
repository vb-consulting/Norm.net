using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Norm.Interfaces;

namespace Norm
{
    public partial class Norm
    {
        public INormMultipleReader Multiple(string command)
        {
            using var cmd = CreateCommand(command);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public INormMultipleReader MultipleFormat(FormattableString command)
        {
            using var cmd = CreateCommand(command);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleAsync(string command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleFormatAsync(FormattableString command)
        {
            using var cmd = await CreateCommandAsync(command);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }

        public INormMultipleReader Multiple(string command, params object[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleAsync(string command, params object[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }

        public INormMultipleReader Multiple(string command, params (string name, object value)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleAsync(string command, params (string name, object value)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }

        public INormMultipleReader Multiple(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleAsync(string command, params (string name, object value, DbType type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }

        public INormMultipleReader Multiple(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = CreateCommand(command, parameters);
            return new NormMultipleMultipleReader(cmd.ExecuteReader(), cancellationToken, convertsDbNull);
        }

        public async ValueTask<INormMultipleReader> MultipleAsync(string command, params (string name, object value, object type)[] parameters)
        {
            using var cmd = await CreateCommandAsync(command, parameters);
            if (cancellationToken.HasValue)
            {
                return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(cancellationToken.Value), cancellationToken, convertsDbNull);
            }
            return new NormMultipleMultipleReader(await cmd.ExecuteReaderAsync(), cancellationToken, convertsDbNull);
        }
    }
}
