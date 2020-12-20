using System;
using System.Collections.Generic;
using System.Data;

namespace Norm
{
    public partial class Norm
    {
        public IEnumerable<(string name, object value)[]> Read(string command) =>
            ReadToArrayInternal(command);

        public IEnumerable<(string name, object value)[]> Read(string command, params object[] parameters) =>
            ReadToArrayInternal(command, parameters);

        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value)[] parameters) =>
            ReadToArrayInternal(command, parameters);

        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadToArrayInternal(command, parameters);

        public IEnumerable<(string name, object value)[]> Read(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadToArrayInternalUnknowParamsType(command, parameters);

        public IEnumerable<T> Read<T>(string command)
        {
            var (type, simple) = TypeCache<T>.IsSimpleType();
            if (simple)
            {
                return ReadInternal(command, r => GetFieldValue<T>(r, 0));
            }
            return Read(command).Map<T>(type);
        }

        public IEnumerable<T> Read<T>(string command, params object[] parameters)
        {
            var (type, simple) = TypeCache<T>.IsSimpleType();
            if (simple)
            {
                return ReadInternal(command, r => GetFieldValue<T>(r, 0), parameters);
            }
            return Read(command, parameters).Map<T>(type);
        }

        public IEnumerable<T> Read<T>(string command, params (string name, object value)[] parameters)
        {
            var (type, simple) = TypeCache<T>.IsSimpleType();
            if (simple)
            {
                return ReadInternal(command, r => GetFieldValue<T>(r, 0), parameters);
            }
            return Read(command, parameters).Map<T>(type);
        }

        public IEnumerable<T> Read<T>(string command, params (string name, object value, DbType type)[] parameters)
        {
            var (type, simple) = TypeCache<T>.IsSimpleType();
            if (simple)
            {
                return ReadInternal(command, r => GetFieldValue<T>(r, 0), parameters);
            }
            return Read(command, parameters).Map<T>(type);
        }

        public IEnumerable<T> Read<T>(string command, params (string name, object value, object type)[] parameters)
        {
            var (type, simple) = TypeCache<T>.IsSimpleType();
            if (simple)
            {
                return ReadInternalUnknowParamsType(command, r => GetFieldValue<T>(r, 0), parameters);
            }
            return Read(command, parameters).Map<T>(type);
        }

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1)));

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params object[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1)),
                parameters);

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1)),
                parameters);

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1)),
                parameters);

        public IEnumerable<(T1, T2)> Read<T1, T2>(string command, params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1)),
                parameters);

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2)));

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command, params object[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2)),
                parameters);

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2)),
                parameters);

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2)),
                parameters);

        public IEnumerable<(T1, T2, T3)> Read<T1, T2, T3>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2)),
                parameters);

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2),
                    GetFieldValue<T4>(r, 3)));

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command, params object[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2),
                    GetFieldValue<T4>(r, 3)),
                parameters);

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2),
                    GetFieldValue<T4>(r, 3)),
                parameters);

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2),
                    GetFieldValue<T4>(r, 3)),
                parameters);

        public IEnumerable<(T1, T2, T3, T4)> Read<T1, T2, T3, T4>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2),
                    GetFieldValue<T4>(r, 3)),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4))
            );

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command, params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5)> Read<T1, T2, T3, T4, T5>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> Read<T1, T2, T3, T4, T5, T6>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
            params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Read<T1, T2, T3, T4, T5, T6, T7>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command,
            params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command,
            params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command,
            params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Read<T1, T2, T3, T4, T5, T6, T7, T8>(string command,
            params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            string command, params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command, params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command, params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            string command, params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            string command, params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10), GetFieldValue<T12>(r, 11)
                ));

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command, params object[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10), GetFieldValue<T12>(r, 11)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command, params (string name, object value)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10), GetFieldValue<T12>(r, 11)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command, params (string name, object value, DbType type)[] parameters) =>
            ReadInternal(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10), GetFieldValue<T12>(r, 11)
                ),
                parameters);

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Read<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            string command, params (string name, object value, object type)[] parameters) =>
            ReadInternalUnknowParamsType(command,
                r => (
                    GetFieldValue<T1>(r, 0), GetFieldValue<T2>(r, 1), GetFieldValue<T3>(r, 2), GetFieldValue<T4>(r, 3),
                    GetFieldValue<T5>(r, 4),
                    GetFieldValue<T6>(r, 5), GetFieldValue<T7>(r, 6), GetFieldValue<T8>(r, 7), GetFieldValue<T9>(r, 8),
                    GetFieldValue<T10>(r, 9), GetFieldValue<T11>(r, 10), GetFieldValue<T12>(r, 11)
                ),
                parameters);
    }
}