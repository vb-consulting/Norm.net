using System;
using System.Collections.Generic;

namespace Norm.Mapper
{
    public static partial class NormExtensions
    {
        public static async IAsyncEnumerable<T> Map<T>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1)
        {
            var ctorInfo1 = TypeCache<T>.GetCtorInfo(ref type1);
            MapDescriptor descriptor = null;
            var delegates = CreateDelegateArray(TypeCache<T>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                var t1 = TypeCache<T>.CreateInstance(ref ctorInfo1);
                MapInstance(t, ref t1, ref descriptor, ref delegates);
                yield return t1;
            }
        }

        public static async IAsyncEnumerable<(T1, T2)> Map<T1, T2>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                descriptor.Reset();
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                yield return (t1, t2);
            }
        }

        public static async IAsyncEnumerable<(T1, T2, T3)> Map<T1, T2, T3>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2,
            Type type3)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(ref type3);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                descriptor.Reset();
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ref ctorInfo3);
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                MapInstance(t, ref t3, ref descriptor, ref delegates3);
                yield return (t1, t2, t3);
            }
        }

        public static async IAsyncEnumerable<(T1, T2, T3, T4)> Map<T1, T2, T3, T4>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(ref type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(ref type4);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                descriptor.Reset();
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ref ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ref ctorInfo4);
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                MapInstance(t, ref t3, ref descriptor, ref delegates3);
                MapInstance(t, ref t4, ref descriptor, ref delegates4);
                yield return (t1, t2, t3, t4);
            }
        }

        public static async IAsyncEnumerable<(T1, T2, T3, T4, T5)> Map<T1, T2, T3, T4, T5>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(ref type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(ref type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(ref type5);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                descriptor.Reset();
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ref ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ref ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ref ctorInfo5);
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                MapInstance(t, ref t3, ref descriptor, ref delegates3);
                MapInstance(t, ref t4, ref descriptor, ref delegates4);
                MapInstance(t, ref t5, ref descriptor, ref delegates5);
                yield return (t1, t2, t3, t4, t5);
            }
        }

        public static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> Map<T1, T2, T3, T4, T5, T6>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(ref type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(ref type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(ref type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(ref type6);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                descriptor.Reset();
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ref ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ref ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ref ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ref ctorInfo6);
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                MapInstance(t, ref t3, ref descriptor, ref delegates3);
                MapInstance(t, ref t4, ref descriptor, ref delegates4);
                MapInstance(t, ref t5, ref descriptor, ref delegates5);
                MapInstance(t, ref t6, ref descriptor, ref delegates6);
                yield return (t1, t2, t3, t4, t5, t6);
            }
        }

        public static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Map<T1, T2, T3, T4, T5, T6, T7>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6,
            Type type7)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(ref type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(ref type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(ref type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(ref type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(ref type7);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            var delegates7 = CreateDelegateArray(TypeCache<T7>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                descriptor.Reset();
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ref ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ref ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ref ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ref ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ref ctorInfo7);
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                MapInstance(t, ref t3, ref descriptor, ref delegates3);
                MapInstance(t, ref t4, ref descriptor, ref delegates4);
                MapInstance(t, ref t5, ref descriptor, ref delegates5);
                MapInstance(t, ref t6, ref descriptor, ref delegates6);
                MapInstance(t, ref t7, ref descriptor, ref delegates7);
                yield return (t1, t2, t3, t4, t5, t6, t7);
            }
        }

        public static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Map<T1, T2, T3, T4, T5, T6, T7, T8>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6,
            Type type7,
            Type type8)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(ref type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(ref type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(ref type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(ref type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(ref type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(ref type8);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            var delegates7 = CreateDelegateArray(TypeCache<T7>.GetPropertiesLength());
            var delegates8 = CreateDelegateArray(TypeCache<T8>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                descriptor.Reset();
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ref ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ref ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ref ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ref ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ref ctorInfo7);
                var t8 = TypeCache<T8>.CreateInstance(ref ctorInfo8);
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                MapInstance(t, ref t3, ref descriptor, ref delegates3);
                MapInstance(t, ref t4, ref descriptor, ref delegates4);
                MapInstance(t, ref t5, ref descriptor, ref delegates5);
                MapInstance(t, ref t6, ref descriptor, ref delegates6);
                MapInstance(t, ref t7, ref descriptor, ref delegates7);
                MapInstance(t, ref t8, ref descriptor, ref delegates8);

                yield return (t1, t2, t3, t4, t5, t6, t7, t8);
            }
        }

        public static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6,
            Type type7,
            Type type8,
            Type type9)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(ref type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(ref type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(ref type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(ref type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(ref type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(ref type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(ref type9);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            var delegates7 = CreateDelegateArray(TypeCache<T7>.GetPropertiesLength());
            var delegates8 = CreateDelegateArray(TypeCache<T8>.GetPropertiesLength());
            var delegates9 = CreateDelegateArray(TypeCache<T9>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                descriptor.Reset();
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ref ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ref ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ref ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ref ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ref ctorInfo7);
                var t8 = TypeCache<T8>.CreateInstance(ref ctorInfo8);
                var t9 = TypeCache<T9>.CreateInstance(ref ctorInfo9);
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                MapInstance(t, ref t3, ref descriptor, ref delegates3);
                MapInstance(t, ref t4, ref descriptor, ref delegates4);
                MapInstance(t, ref t5, ref descriptor, ref delegates5);
                MapInstance(t, ref t6, ref descriptor, ref delegates6);
                MapInstance(t, ref t7, ref descriptor, ref delegates7);
                MapInstance(t, ref t8, ref descriptor, ref delegates8);
                MapInstance(t, ref t9, ref descriptor, ref delegates9);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9);
            }
        }

        public static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6,
            Type type7,
            Type type8,
            Type type9,
            Type type10)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(ref type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(ref type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(ref type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(ref type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(ref type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(ref type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(ref type9);
            var ctorInfo10 = TypeCache<T10>.GetCtorInfo(ref type10);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            var delegates7 = CreateDelegateArray(TypeCache<T7>.GetPropertiesLength());
            var delegates8 = CreateDelegateArray(TypeCache<T8>.GetPropertiesLength());
            var delegates9 = CreateDelegateArray(TypeCache<T9>.GetPropertiesLength());
            var delegates10 = CreateDelegateArray(TypeCache<T10>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                descriptor.Reset();
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ref ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ref ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ref ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ref ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ref ctorInfo7);
                var t8 = TypeCache<T8>.CreateInstance(ref ctorInfo8);
                var t9 = TypeCache<T9>.CreateInstance(ref ctorInfo9);
                var t10 = TypeCache<T10>.CreateInstance(ref ctorInfo10);
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                MapInstance(t, ref t3, ref descriptor, ref delegates3);
                MapInstance(t, ref t4, ref descriptor, ref delegates4);
                MapInstance(t, ref t5, ref descriptor, ref delegates5);
                MapInstance(t, ref t6, ref descriptor, ref delegates6);
                MapInstance(t, ref t7, ref descriptor, ref delegates7);
                MapInstance(t, ref t8, ref descriptor, ref delegates8);
                MapInstance(t, ref t9, ref descriptor, ref delegates9);
                MapInstance(t, ref t10, ref descriptor, ref delegates10);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            }
        }

        public static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6,
            Type type7,
            Type type8,
            Type type9,
            Type type10,
            Type type11)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(ref type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(ref type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(ref type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(ref type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(ref type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(ref type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(ref type9);
            var ctorInfo10 = TypeCache<T10>.GetCtorInfo(ref type10);
            var ctorInfo11 = TypeCache<T11>.GetCtorInfo(ref type11);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            var delegates7 = CreateDelegateArray(TypeCache<T7>.GetPropertiesLength());
            var delegates8 = CreateDelegateArray(TypeCache<T8>.GetPropertiesLength());
            var delegates9 = CreateDelegateArray(TypeCache<T9>.GetPropertiesLength());
            var delegates10 = CreateDelegateArray(TypeCache<T10>.GetPropertiesLength());
            var delegates11 = CreateDelegateArray(TypeCache<T11>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                descriptor.Reset();
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ref ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ref ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ref ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ref ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ref ctorInfo7);
                var t8 = TypeCache<T8>.CreateInstance(ref ctorInfo8);
                var t9 = TypeCache<T9>.CreateInstance(ref ctorInfo9);
                var t10 = TypeCache<T10>.CreateInstance(ref ctorInfo10);
                var t11 = TypeCache<T11>.CreateInstance(ref ctorInfo11);
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                MapInstance(t, ref t3, ref descriptor, ref delegates3);
                MapInstance(t, ref t4, ref descriptor, ref delegates4);
                MapInstance(t, ref t5, ref descriptor, ref delegates5);
                MapInstance(t, ref t6, ref descriptor, ref delegates6);
                MapInstance(t, ref t7, ref descriptor, ref delegates7);
                MapInstance(t, ref t8, ref descriptor, ref delegates8);
                MapInstance(t, ref t9, ref descriptor, ref delegates9);
                MapInstance(t, ref t10, ref descriptor, ref delegates10);
                MapInstance(t, ref t11, ref descriptor, ref delegates11);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            }
        }

        public static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IAsyncEnumerable<ReadOnlyMemory<(string name, object value)>> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6,
            Type type7,
            Type type8,
            Type type9,
            Type type10,
            Type type11,
            Type type12)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(ref type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(ref type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(ref type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(ref type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(ref type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(ref type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(ref type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(ref type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(ref type9);
            var ctorInfo10 = TypeCache<T10>.GetCtorInfo(ref type10);
            var ctorInfo11 = TypeCache<T11>.GetCtorInfo(ref type11);
            var ctorInfo12 = TypeCache<T12>.GetCtorInfo(ref type12);

            MapDescriptor descriptor = null;
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            var delegates7 = CreateDelegateArray(TypeCache<T7>.GetPropertiesLength());
            var delegates8 = CreateDelegateArray(TypeCache<T8>.GetPropertiesLength());
            var delegates9 = CreateDelegateArray(TypeCache<T9>.GetPropertiesLength());
            var delegates10 = CreateDelegateArray(TypeCache<T10>.GetPropertiesLength());
            var delegates11 = CreateDelegateArray(TypeCache<T11>.GetPropertiesLength());
            var delegates12 = CreateDelegateArray(TypeCache<T12>.GetPropertiesLength());
            await foreach (var t in tuples)
            {
                descriptor ??= BuildDescriptor(t);
                var t1 = TypeCache<T1>.CreateInstance(ref ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ref ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ref ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ref ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ref ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ref ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ref ctorInfo7);
                var t8 = TypeCache<T8>.CreateInstance(ref ctorInfo8);
                var t9 = TypeCache<T9>.CreateInstance(ref ctorInfo9);
                var t10 = TypeCache<T10>.CreateInstance(ref ctorInfo10);
                var t11 = TypeCache<T11>.CreateInstance(ref ctorInfo11);
                var t12 = TypeCache<T12>.CreateInstance(ref ctorInfo12);
                descriptor.Reset();
                MapInstance(t, ref t1, ref descriptor, ref delegates1);
                MapInstance(t, ref t2, ref descriptor, ref delegates2);
                MapInstance(t, ref t3, ref descriptor, ref delegates3);
                MapInstance(t, ref t4, ref descriptor, ref delegates4);
                MapInstance(t, ref t5, ref descriptor, ref delegates5);
                MapInstance(t, ref t6, ref descriptor, ref delegates6);
                MapInstance(t, ref t7, ref descriptor, ref delegates7);
                MapInstance(t, ref t8, ref descriptor, ref delegates8);
                MapInstance(t, ref t9, ref descriptor, ref delegates9);
                MapInstance(t, ref t10, ref descriptor, ref delegates10);
                MapInstance(t, ref t11, ref descriptor, ref delegates11);
                MapInstance(t, ref t12, ref descriptor, ref delegates12);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            }
        }
    }
}