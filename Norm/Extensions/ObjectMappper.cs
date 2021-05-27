using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Norm
{
    public static partial class NormExtensions
    {
        internal static IEnumerable<T> Map<T>(this IEnumerable<(string name, object value)[]> tuples, 
            Type type1)
        {
            var ctorInfo1 = TypeCache<T>.GetCtorInfo(type1);

            foreach (var t in tuples)
            {
                yield return t.MapInstance(type1, TypeCache<T>.CreateInstance(ctorInfo1));
            }
        }

        internal static IEnumerable<(T1, T2)> Map<T1, T2>(this IEnumerable<(string name, object value)[]> tuples, 
            Type type1, 
            Type type2)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            
            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                yield return (t1, t2);
            }
        }

        internal static IEnumerable<(T1, T2, T3)> Map<T1, T2, T3>(this IEnumerable<(string name, object value)[]> tuples,
            Type type1, 
            Type type2, 
            Type type3)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);

            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                yield return (t1, t2, t3);
            }
        }

        internal static IEnumerable<(T1, T2, T3, T4)> Map<T1, T2, T3, T4>(this IEnumerable<(string name, object value)[]> tuples,
            Type type1, 
            Type type2, 
            Type type3, 
            Type type4)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);

            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                yield return (t1, t2, t3, t4);
            }
        }

        internal static IEnumerable<(T1, T2, T3, T4, T5)> Map<T1, T2, T3, T4, T5>(this IEnumerable<(string name, object value)[]> tuples,
            Type type1, 
            Type type2, 
            Type type3, 
            Type type4, 
            Type type5)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);

            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                yield return (t1, t2, t3, t4, t5);
            }
        }

        internal static IEnumerable<(T1, T2, T3, T4, T5, T6)> Map<T1, T2, T3, T4, T5, T6>(this IEnumerable<(string name, object value)[]> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);

            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                yield return (t1, t2, t3, t4, t5, t6);
            }
        }

        internal static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Map<T1, T2, T3, T4, T5, T6, T7>(this IEnumerable<(string name, object value)[]> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6,
            Type type7)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);

            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7);
            }
        }

        internal static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Map<T1, T2, T3, T4, T5, T6, T7, T8>(this IEnumerable<(string name, object value)[]> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6,
            Type type7, 
            Type type8)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(type8);

            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                var t8 = t.MapInstance(type8, TypeCache<T8>.CreateInstance(ctorInfo8), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8);
            }
        }

        internal static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IEnumerable<(string name, object value)[]> tuples,
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
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(type9);

            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                var t8 = t.MapInstance(type8, TypeCache<T8>.CreateInstance(ctorInfo8), ref used);
                var t9 = t.MapInstance(type9, TypeCache<T9>.CreateInstance(ctorInfo9), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9);
            }
        }

        internal static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IEnumerable<(string name, object value)[]> tuples,
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
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(type9);
            var ctorInfo10 = TypeCache<T10>.GetCtorInfo(type10);

            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                var t8 = t.MapInstance(type8, TypeCache<T8>.CreateInstance(ctorInfo8), ref used);
                var t9 = t.MapInstance(type9, TypeCache<T9>.CreateInstance(ctorInfo9), ref used);
                var t10 = t.MapInstance(type10, TypeCache<T10>.CreateInstance(ctorInfo10), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            }
        }

        internal static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IEnumerable<(string name, object value)[]> tuples,
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
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(type9);
            var ctorInfo10 = TypeCache<T10>.GetCtorInfo(type10);
            var ctorInfo11 = TypeCache<T11>.GetCtorInfo(type11);

            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                var t8 = t.MapInstance(type8, TypeCache<T8>.CreateInstance(ctorInfo8), ref used);
                var t9 = t.MapInstance(type9, TypeCache<T9>.CreateInstance(ctorInfo9), ref used);
                var t10 = t.MapInstance(type10, TypeCache<T10>.CreateInstance(ctorInfo10), ref used);
                var t11 = t.MapInstance(type11, TypeCache<T11>.CreateInstance(ctorInfo11), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            }
        }

        internal static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IEnumerable<(string name, object value)[]> tuples,
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
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(type9);
            var ctorInfo10 = TypeCache<T10>.GetCtorInfo(type10);
            var ctorInfo11 = TypeCache<T11>.GetCtorInfo(type11);
            var ctorInfo12 = TypeCache<T12>.GetCtorInfo(type12);

            foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                var t8 = t.MapInstance(type8, TypeCache<T8>.CreateInstance(ctorInfo8), ref used);
                var t9 = t.MapInstance(type9, TypeCache<T9>.CreateInstance(ctorInfo9), ref used);
                var t10 = t.MapInstance(type10, TypeCache<T10>.CreateInstance(ctorInfo10), ref used);
                var t11 = t.MapInstance(type11, TypeCache<T11>.CreateInstance(ctorInfo11), ref used);
                var t12 = t.MapInstance(type12, TypeCache<T12>.CreateInstance(ctorInfo12), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            }
        }


        internal static async IAsyncEnumerable<T> Map<T>(this IAsyncEnumerable<(string name, object value)[]> tuples,
            Type type1)
        {
            var ctorInfo1 = TypeCache<T>.GetCtorInfo(type1);

            await foreach (var t in tuples)
            {
                yield return t.MapInstance(type1, TypeCache<T>.CreateInstance(ctorInfo1));
            }
        }

        internal static async IAsyncEnumerable<(T1, T2)> Map<T1, T2>(this IAsyncEnumerable<(string name, object value)[]> tuples,
            Type type1,
            Type type2)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                yield return (t1, t2);
            }
        }

        internal static async IAsyncEnumerable<(T1, T2, T3)> Map<T1, T2, T3>(this IAsyncEnumerable<(string name, object value)[]> tuples,
            Type type1,
            Type type2,
            Type type3)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                yield return (t1, t2, t3);
            }
        }

        internal static async IAsyncEnumerable<(T1, T2, T3, T4)> Map<T1, T2, T3, T4>(this IAsyncEnumerable<(string name, object value)[]> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                yield return (t1, t2, t3, t4);
            }
        }

        internal static async IAsyncEnumerable<(T1, T2, T3, T4, T5)> Map<T1, T2, T3, T4, T5>(this IAsyncEnumerable<(string name, object value)[]> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                yield return (t1, t2, t3, t4, t5);
            }
        }

        internal static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6)> Map<T1, T2, T3, T4, T5, T6>(this IAsyncEnumerable<(string name, object value)[]> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                yield return (t1, t2, t3, t4, t5, t6);
            }
        }

        internal static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Map<T1, T2, T3, T4, T5, T6, T7>(this IAsyncEnumerable<(string name, object value)[]> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6,
            Type type7)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7);
            }
        }

        internal static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Map<T1, T2, T3, T4, T5, T6, T7, T8>(this IAsyncEnumerable<(string name, object value)[]> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4,
            Type type5,
            Type type6,
            Type type7,
            Type type8)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(type8);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                var t8 = t.MapInstance(type8, TypeCache<T8>.CreateInstance(ctorInfo8), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8);
            }
        }

        internal static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IAsyncEnumerable<(string name, object value)[]> tuples,
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
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(type9);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                var t8 = t.MapInstance(type8, TypeCache<T8>.CreateInstance(ctorInfo8), ref used);
                var t9 = t.MapInstance(type9, TypeCache<T9>.CreateInstance(ctorInfo9), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9);
            }
        }

        internal static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IAsyncEnumerable<(string name, object value)[]> tuples,
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
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(type9);
            var ctorInfo10 = TypeCache<T10>.GetCtorInfo(type10);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                var t8 = t.MapInstance(type8, TypeCache<T8>.CreateInstance(ctorInfo8), ref used);
                var t9 = t.MapInstance(type9, TypeCache<T9>.CreateInstance(ctorInfo9), ref used);
                var t10 = t.MapInstance(type10, TypeCache<T10>.CreateInstance(ctorInfo10), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            }
        }

        internal static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IAsyncEnumerable<(string name, object value)[]> tuples,
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
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(type9);
            var ctorInfo10 = TypeCache<T10>.GetCtorInfo(type10);
            var ctorInfo11 = TypeCache<T11>.GetCtorInfo(type11);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                var t8 = t.MapInstance(type8, TypeCache<T8>.CreateInstance(ctorInfo8), ref used);
                var t9 = t.MapInstance(type9, TypeCache<T9>.CreateInstance(ctorInfo9), ref used);
                var t10 = t.MapInstance(type10, TypeCache<T10>.CreateInstance(ctorInfo10), ref used);
                var t11 = t.MapInstance(type11, TypeCache<T11>.CreateInstance(ctorInfo11), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            }
        }

        internal static async IAsyncEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IAsyncEnumerable<(string name, object value)[]> tuples,
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
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);
            var ctorInfo5 = TypeCache<T5>.GetCtorInfo(type5);
            var ctorInfo6 = TypeCache<T6>.GetCtorInfo(type6);
            var ctorInfo7 = TypeCache<T7>.GetCtorInfo(type7);
            var ctorInfo8 = TypeCache<T8>.GetCtorInfo(type8);
            var ctorInfo9 = TypeCache<T9>.GetCtorInfo(type9);
            var ctorInfo10 = TypeCache<T10>.GetCtorInfo(type10);
            var ctorInfo11 = TypeCache<T11>.GetCtorInfo(type11);
            var ctorInfo12 = TypeCache<T12>.GetCtorInfo(type12);

            await foreach (var t in tuples)
            {
                var used = new HashSet<ushort>(t.Length);
                var t1 = t.MapInstance(type1, TypeCache<T1>.CreateInstance(ctorInfo1), ref used);
                var t2 = t.MapInstance(type2, TypeCache<T2>.CreateInstance(ctorInfo2), ref used);
                var t3 = t.MapInstance(type3, TypeCache<T3>.CreateInstance(ctorInfo3), ref used);
                var t4 = t.MapInstance(type4, TypeCache<T4>.CreateInstance(ctorInfo4), ref used);
                var t5 = t.MapInstance(type5, TypeCache<T5>.CreateInstance(ctorInfo5), ref used);
                var t6 = t.MapInstance(type6, TypeCache<T6>.CreateInstance(ctorInfo6), ref used);
                var t7 = t.MapInstance(type7, TypeCache<T7>.CreateInstance(ctorInfo7), ref used);
                var t8 = t.MapInstance(type8, TypeCache<T8>.CreateInstance(ctorInfo8), ref used);
                var t9 = t.MapInstance(type9, TypeCache<T9>.CreateInstance(ctorInfo9), ref used);
                var t10 = t.MapInstance(type10, TypeCache<T10>.CreateInstance(ctorInfo10), ref used);
                var t11 = t.MapInstance(type11, TypeCache<T11>.CreateInstance(ctorInfo11), ref used);
                var t12 = t.MapInstance(type12, TypeCache<T12>.CreateInstance(ctorInfo12), ref used);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            }
        }

        private static T MapInstance<T>(this (string name, object value)[] tuple,
            Type type,
            T instance)
        {
            ushort i = 0;
            var properties = TypeCache<T>.GetProperties(type);
            var delegates = TypeCache<T>.GetDelegates(properties.Length);
            Dictionary<string, ushort> names = null;
            foreach (var property in properties)
            {
                var (method, nullable, code, isArray, index, isTimespan) = delegates[i];
                if (method == null)
                {
                    var propType = property.PropertyType;
                    nullable = Nullable.GetUnderlyingType(propType) != null;
                    (method, code, isArray, isTimespan) = CreateDelegate<T>(property, nullable);

                    var name = property.Name.ToLower();
                    if (names == null)
                    {
                        names = TypeCache<T>.GetNames(tuple);
                    }
                    if (!names.TryGetValue(name, out index))
                    {
                        index = ushort.MaxValue;
                    }
                    delegates[i] = (method, nullable, code, isArray, index, isTimespan);
                }
                i++;
                if (index == ushort.MaxValue)
                {
                    continue;
                }
                InvokeSet(method, nullable, code, instance, tuple[index].value, isArray, isTimespan);
            }
            return instance;
        }

        private static Type TimeSpanType = typeof(TimeSpan);

        private static T MapInstance<T>(this (string name, object value)[] tuple, 
            Type type, 
            T instance,
            ref HashSet<ushort> used)
        {
            ushort i = 0;
            var properties = TypeCache<T>.GetProperties(type);
            var delegates = TypeCache<T>.GetDelegates(properties.Length);
            Dictionary<string, ushort> names = null;
            foreach (var property in properties)
            {
                var (method, nullable, code, isArray, index, isTimespan) = delegates[i];
                if (method == null)
                {
                    var propType = property.PropertyType;
                    nullable = Nullable.GetUnderlyingType(propType) != null;
                    (method, code, isArray, isTimespan) = CreateDelegate<T>(property, nullable);

                    var name = property.Name.ToLower();
                    if (names == null)
                    {
                        names = TypeCache<T>.GetNames(tuple);
                    }
                    if (!names.TryGetValue(name, out index))
                    {
                        index = ushort.MaxValue;
                    }
                    if (used.Contains(index))
                    {
                        continue;
                    }
                    delegates[i] = (method, nullable, code, isArray, index, isTimespan);
                }
                i++;
                if (index == ushort.MaxValue)
                {
                    continue;
                }
                InvokeSet(method, nullable, code, instance, tuple[index].value, isArray, isTimespan);
                used.Add(index);
            }
            return instance;
        }

        private static (Delegate method, TypeCode code, bool isArray, bool isTimespan) CreateDelegate<T>(PropertyInfo property, bool nullable)
        {
            TypeCode code;
            bool isArray;
            var type = property.PropertyType;
            if (type.IsArray)
            {
                isArray = true;
                var elementType = type.GetElementType();
                code = Type.GetTypeCode(elementType);
                if (code == TypeCode.Object && elementType == TimeSpanType)
                {
                    return (CreateDelegateValue<T, TimeSpan[]>(property), code, isArray, true);
                }
            }
            else
            {
                isArray = false;
                code = nullable ? Type.GetTypeCode(type.GenericTypeArguments[0]) : Type.GetTypeCode(type);
                if (code == TypeCode.Object && (type == TimeSpanType || type.GenericTypeArguments[0] == TimeSpanType))
                {
                    return (CreateDelegateStruct<T, TimeSpan>(property, nullable), code, isArray, true);
                }
            }

            return code switch
            {
                TypeCode.Int32 => (isArray ? CreateDelegateValue<T, int[]>(property) : CreateDelegateStruct<T, int>(property, nullable), code, isArray, false),
                TypeCode.DateTime => (isArray ? CreateDelegateValue<T, DateTime[]>(property) : CreateDelegateStruct<T, DateTime>(property, nullable), code, isArray, false),
                TypeCode.String => (isArray ? CreateDelegateValue<T, string[]>(property) : CreateDelegateValue<T, string>(property), code, isArray, false),
                TypeCode.Boolean => (isArray ? CreateDelegateValue<T, bool[]>(property) : CreateDelegateStruct<T, bool>(property, nullable), code, isArray, false),
                TypeCode.Byte => (isArray ? CreateDelegateValue<T, byte[]>(property) : CreateDelegateStruct<T, byte>(property, nullable), code, isArray, false),
                TypeCode.Char => (isArray ? CreateDelegateValue<T, char[]>(property) : CreateDelegateStruct<T, char>(property, nullable), code, isArray, false),
                TypeCode.Decimal => (isArray ? CreateDelegateValue<T, decimal[]>(property) : CreateDelegateStruct<T, decimal>(property, nullable), code, isArray, false),
                TypeCode.Double => (isArray ? CreateDelegateValue<T, double[]>(property) : CreateDelegateStruct<T, double>(property, nullable), code, isArray, false),
                TypeCode.Int16 => (isArray ? CreateDelegateValue<T, short[]>(property) : CreateDelegateStruct<T, short>(property, nullable), code, isArray, false),
                TypeCode.Int64 => (isArray ? CreateDelegateValue<T, long[]>(property) : CreateDelegateStruct<T, long>(property, nullable), code, isArray, false),
                TypeCode.SByte => (isArray ? CreateDelegateValue<T, sbyte[]>(property) : CreateDelegateStruct<T, sbyte>(property, nullable), code, isArray, false),
                TypeCode.Single => (isArray ? CreateDelegateValue<T, float[]>(property) : CreateDelegateStruct<T, float>(property, nullable), code, isArray, false),
                TypeCode.UInt16 => (isArray ? CreateDelegateValue<T, ushort[]>(property) : CreateDelegateStruct<T, ushort>(property, nullable), code, isArray, false),
                TypeCode.UInt32 => (isArray ? CreateDelegateValue<T, uint[]>(property) : CreateDelegateStruct<T, uint>(property, nullable), code, isArray, false),
                TypeCode.UInt64 => (isArray ? CreateDelegateValue<T, ulong[]>(property) : CreateDelegateStruct<T, ulong>(property, nullable), code, isArray, false),
                _ => throw new NotImplementedException($"TypeCode {code} not implemented"),
            };
        }

        private static Delegate CreateDelegateValue<T, TProp>(PropertyInfo property)
        {
            return Delegate.CreateDelegate(typeof(Action<T, TProp>), property.GetSetMethod(true));
        }

        private static Delegate CreateDelegateStruct<T, TProp>(PropertyInfo property, bool nullable) where TProp : struct
        {
            return nullable ?
                Delegate.CreateDelegate(typeof(Action<T, TProp?>), property.GetSetMethod(true)) :
                Delegate.CreateDelegate(typeof(Action<T, TProp>), property.GetSetMethod(true));
        }

        private static void InvokeSet<T>(Delegate method, bool nullable, TypeCode code, T instance, object value, bool isArray, bool isTimespan)
        {
            if (isTimespan)
            {
                if (isArray)
                {
                    InvokeSetValue<T, TimeSpan[]>(method, instance, value);
                }
                else InvokeSetStruct<T, TimeSpan>(method, nullable, instance, value);
            }
            switch (code)
            {
                case TypeCode.Int32:
                    if (isArray) InvokeSetValue<T, int[]>(method, instance, value); else InvokeSetStruct<T, int>(method, nullable, instance, value);
                    break;
                case TypeCode.DateTime:
                    if (isArray) InvokeSetValue<T, DateTime[]> (method, instance, value); else InvokeSetStruct<T, DateTime>(method, nullable, instance, value);
                    break;
                case TypeCode.String:
                    if (isArray) InvokeSetValue<T, string[]>(method, instance, value); else InvokeSetValue<T, string>(method, instance, value);
                    break;
                case TypeCode.Boolean:
                    if (isArray) InvokeSetValue<T, bool[]>(method, instance, value); else InvokeSetStruct<T, bool>(method, nullable, instance, value);
                    break;
                case TypeCode.Byte:
                    if (isArray) InvokeSetValue<T, byte[]>(method, instance, value); else InvokeSetStruct<T, byte>(method, nullable, instance, value);
                    break;
                case TypeCode.Char:
                    if (isArray) InvokeSetValue<T, char[]>(method, instance, value); else InvokeSetStruct<T, char>(method, nullable, instance, value);
                    break;
                case TypeCode.Decimal:
                    if (isArray) InvokeSetValue<T, decimal[]>(method, instance, value); else InvokeSetStruct<T, decimal>(method, nullable, instance, value);
                    break;
                case TypeCode.Double:
                    if (isArray) InvokeSetValue<T, double[]>(method, instance, value); else InvokeSetStruct<T, double>(method, nullable, instance, value);
                    break;
                case TypeCode.Int16:
                    if (isArray) InvokeSetValue<T, short[]>(method, instance, value); else InvokeSetStruct<T, short>(method, nullable, instance, value);
                    break;
                case TypeCode.Int64:
                    if (isArray) InvokeSetValue<T, long[]>(method, instance, value); else InvokeSetStruct<T, long>(method, nullable, instance, value);
                    break;
                case TypeCode.SByte:
                    if (isArray) InvokeSetValue<T, sbyte[]>(method, instance, value); else InvokeSetStruct<T, sbyte>(method, nullable, instance, value);
                    break;
                case TypeCode.Single:
                    if (isArray) InvokeSetValue<T, float[]>(method, instance, value); else InvokeSetStruct<T, float>(method, nullable, instance, value);
                    break;
                case TypeCode.UInt16:
                    if (isArray) InvokeSetValue<T, ushort[]>(method, instance, value); else InvokeSetStruct<T, ushort>(method, nullable, instance, value);
                    break;
                case TypeCode.UInt32:
                    if (isArray) InvokeSetValue<T, uint[]>(method, instance, value); else InvokeSetStruct<T, uint>(method, nullable, instance, value);
                    break;
                case TypeCode.UInt64:
                    if (isArray) InvokeSetValue<T, ulong[]>(method, instance, value); else InvokeSetStruct<T, ulong>(method, nullable, instance, value);
                    break;
            }
        }

        private static void InvokeSetValue<T, TProp>(Delegate method, T instance, object value)
        {
            ((Action<T, TProp>)method).Invoke(instance, (TProp)value);
        }

        private static void InvokeSetStruct<T, TProp>(Delegate method, bool nullable, T instance, object value) where TProp : struct
        {
            if (nullable)
            {
                ((Action<T, TProp?>)method).Invoke(instance, (TProp?)value);
            }
            else
            {
                ((Action<T, TProp>)method).Invoke(instance, (TProp)value);
            }
        }
    }
}