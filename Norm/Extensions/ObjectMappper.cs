using System;
using System.Collections.Generic;

namespace Norm
{
    public static partial class NormExtensions
    {
        internal static IEnumerable<T> Map<T>(this IEnumerable<(string name, object value)[]> tuples, 
            Type type1)
        {
            var ctorInfo1 = TypeCache<T>.GetCtorInfo(type1);
            var props = TypeCache<T>.GetProperties(type1);
            Dictionary<string, ushort> names = null; 
            var delegates = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var i1 = TypeCache<T>.CreateInstance(ctorInfo1);
                yield return t.MapInstance(ref props, ref i1, ref names, ref delegates);
            }
        }

        internal static IEnumerable<(T1, T2)> Map<T1, T2>(this IEnumerable<(string name, object value)[]> tuples, 
            Type type1, 
            Type type2)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
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

            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            var props3 = TypeCache<T3>.GetProperties(type3);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            var delegates3 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props3.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var i3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
                var t3 = t.MapInstance(ref props3, ref i3, ref names, ref used, ref delegates3);
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

            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            var props3 = TypeCache<T3>.GetProperties(type3);
            var props4 = TypeCache<T4>.GetProperties(type4);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            var delegates3 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props3.Length];
            var delegates4 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props4.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var i3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var i4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
                var t3 = t.MapInstance(ref props3, ref i3, ref names, ref used, ref delegates3);
                var t4 = t.MapInstance(ref props4, ref i4, ref names, ref used, ref delegates4);
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

            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            var props3 = TypeCache<T3>.GetProperties(type3);
            var props4 = TypeCache<T4>.GetProperties(type4);
            var props5 = TypeCache<T5>.GetProperties(type5);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            var delegates3 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props3.Length];
            var delegates4 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props4.Length];
            var delegates5 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props5.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var i3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var i4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var i5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
                var t3 = t.MapInstance(ref props3, ref i3, ref names, ref used, ref delegates3);
                var t4 = t.MapInstance(ref props4, ref i4, ref names, ref used, ref delegates4);
                var t5 = t.MapInstance(ref props5, ref i5, ref names, ref used, ref delegates5);
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
            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            var props3 = TypeCache<T3>.GetProperties(type3);
            var props4 = TypeCache<T4>.GetProperties(type4);
            var props5 = TypeCache<T5>.GetProperties(type5);
            var props6 = TypeCache<T6>.GetProperties(type6);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            var delegates3 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props3.Length];
            var delegates4 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props4.Length];
            var delegates5 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props5.Length];
            var delegates6 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props6.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var i3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var i4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var i5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var i6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
                var t3 = t.MapInstance(ref props3, ref i3, ref names, ref used, ref delegates3);
                var t4 = t.MapInstance(ref props4, ref i4, ref names, ref used, ref delegates4);
                var t5 = t.MapInstance(ref props5, ref i5, ref names, ref used, ref delegates5);
                var t6 = t.MapInstance(ref props6, ref i6, ref names, ref used, ref delegates6);
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
            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            var props3 = TypeCache<T3>.GetProperties(type3);
            var props4 = TypeCache<T4>.GetProperties(type4);
            var props5 = TypeCache<T5>.GetProperties(type5);
            var props6 = TypeCache<T6>.GetProperties(type6);
            var props7 = TypeCache<T7>.GetProperties(type7);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            var delegates3 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props3.Length];
            var delegates4 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props4.Length];
            var delegates5 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props5.Length];
            var delegates6 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props6.Length];
            var delegates7 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props7.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var i3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var i4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var i5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var i6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var i7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
                var t3 = t.MapInstance(ref props3, ref i3, ref names, ref used, ref delegates3);
                var t4 = t.MapInstance(ref props4, ref i4, ref names, ref used, ref delegates4);
                var t5 = t.MapInstance(ref props5, ref i5, ref names, ref used, ref delegates5);
                var t6 = t.MapInstance(ref props6, ref i6, ref names, ref used, ref delegates6);
                var t7 = t.MapInstance(ref props7, ref i7, ref names, ref used, ref delegates7);
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
            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            var props3 = TypeCache<T3>.GetProperties(type3);
            var props4 = TypeCache<T4>.GetProperties(type4);
            var props5 = TypeCache<T5>.GetProperties(type5);
            var props6 = TypeCache<T6>.GetProperties(type6);
            var props7 = TypeCache<T7>.GetProperties(type7);
            var props8 = TypeCache<T8>.GetProperties(type8);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            var delegates3 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props3.Length];
            var delegates4 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props4.Length];
            var delegates5 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props5.Length];
            var delegates6 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props6.Length];
            var delegates7 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props7.Length];
            var delegates8 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props8.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var i3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var i4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var i5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var i6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var i7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var i8 = TypeCache<T8>.CreateInstance(ctorInfo8);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
                var t3 = t.MapInstance(ref props3, ref i3, ref names, ref used, ref delegates3);
                var t4 = t.MapInstance(ref props4, ref i4, ref names, ref used, ref delegates4);
                var t5 = t.MapInstance(ref props5, ref i5, ref names, ref used, ref delegates5);
                var t6 = t.MapInstance(ref props6, ref i6, ref names, ref used, ref delegates6);
                var t7 = t.MapInstance(ref props7, ref i7, ref names, ref used, ref delegates7);
                var t8 = t.MapInstance(ref props8, ref i8, ref names, ref used, ref delegates8);
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
            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            var props3 = TypeCache<T3>.GetProperties(type3);
            var props4 = TypeCache<T4>.GetProperties(type4);
            var props5 = TypeCache<T5>.GetProperties(type5);
            var props6 = TypeCache<T6>.GetProperties(type6);
            var props7 = TypeCache<T7>.GetProperties(type7);
            var props8 = TypeCache<T8>.GetProperties(type8);
            var props9 = TypeCache<T9>.GetProperties(type9);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            var delegates3 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props3.Length];
            var delegates4 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props4.Length];
            var delegates5 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props5.Length];
            var delegates6 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props6.Length];
            var delegates7 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props7.Length];
            var delegates8 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props8.Length];
            var delegates9 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props9.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var i3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var i4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var i5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var i6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var i7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var i8 = TypeCache<T8>.CreateInstance(ctorInfo8);
                var i9 = TypeCache<T9>.CreateInstance(ctorInfo9);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
                var t3 = t.MapInstance(ref props3, ref i3, ref names, ref used, ref delegates3);
                var t4 = t.MapInstance(ref props4, ref i4, ref names, ref used, ref delegates4);
                var t5 = t.MapInstance(ref props5, ref i5, ref names, ref used, ref delegates5);
                var t6 = t.MapInstance(ref props6, ref i6, ref names, ref used, ref delegates6);
                var t7 = t.MapInstance(ref props7, ref i7, ref names, ref used, ref delegates7);
                var t8 = t.MapInstance(ref props8, ref i8, ref names, ref used, ref delegates8);
                var t9 = t.MapInstance(ref props9, ref i9, ref names, ref used, ref delegates9);
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
            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            var props3 = TypeCache<T3>.GetProperties(type3);
            var props4 = TypeCache<T4>.GetProperties(type4);
            var props5 = TypeCache<T5>.GetProperties(type5);
            var props6 = TypeCache<T6>.GetProperties(type6);
            var props7 = TypeCache<T7>.GetProperties(type7);
            var props8 = TypeCache<T8>.GetProperties(type8);
            var props9 = TypeCache<T9>.GetProperties(type9);
            var props10 = TypeCache<T10>.GetProperties(type10);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            var delegates3 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props3.Length];
            var delegates4 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props4.Length];
            var delegates5 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props5.Length];
            var delegates6 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props6.Length];
            var delegates7 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props7.Length];
            var delegates8 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props8.Length];
            var delegates9 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props9.Length];
            var delegates10 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props10.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var i3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var i4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var i5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var i6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var i7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var i8 = TypeCache<T8>.CreateInstance(ctorInfo8);
                var i9 = TypeCache<T9>.CreateInstance(ctorInfo9);
                var i10 = TypeCache<T10>.CreateInstance(ctorInfo10);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
                var t3 = t.MapInstance(ref props3, ref i3, ref names, ref used, ref delegates3);
                var t4 = t.MapInstance(ref props4, ref i4, ref names, ref used, ref delegates4);
                var t5 = t.MapInstance(ref props5, ref i5, ref names, ref used, ref delegates5);
                var t6 = t.MapInstance(ref props6, ref i6, ref names, ref used, ref delegates6);
                var t7 = t.MapInstance(ref props7, ref i7, ref names, ref used, ref delegates7);
                var t8 = t.MapInstance(ref props8, ref i8, ref names, ref used, ref delegates8);
                var t9 = t.MapInstance(ref props9, ref i9, ref names, ref used, ref delegates9);
                var t10 = t.MapInstance(ref props10, ref i10, ref names, ref used, ref delegates10);
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
            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            var props3 = TypeCache<T3>.GetProperties(type3);
            var props4 = TypeCache<T4>.GetProperties(type4);
            var props5 = TypeCache<T5>.GetProperties(type5);
            var props6 = TypeCache<T6>.GetProperties(type6);
            var props7 = TypeCache<T7>.GetProperties(type7);
            var props8 = TypeCache<T8>.GetProperties(type8);
            var props9 = TypeCache<T9>.GetProperties(type9);
            var props10 = TypeCache<T10>.GetProperties(type10);
            var props11 = TypeCache<T11>.GetProperties(type11);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            var delegates3 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props3.Length];
            var delegates4 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props4.Length];
            var delegates5 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props5.Length];
            var delegates6 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props6.Length];
            var delegates7 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props7.Length];
            var delegates8 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props8.Length];
            var delegates9 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props9.Length];
            var delegates10 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props10.Length];
            var delegates11 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props11.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var i3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var i4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var i5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var i6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var i7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var i8 = TypeCache<T8>.CreateInstance(ctorInfo8);
                var i9 = TypeCache<T9>.CreateInstance(ctorInfo9);
                var i10 = TypeCache<T10>.CreateInstance(ctorInfo10);
                var i11 = TypeCache<T11>.CreateInstance(ctorInfo11);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
                var t3 = t.MapInstance(ref props3, ref i3, ref names, ref used, ref delegates3);
                var t4 = t.MapInstance(ref props4, ref i4, ref names, ref used, ref delegates4);
                var t5 = t.MapInstance(ref props5, ref i5, ref names, ref used, ref delegates5);
                var t6 = t.MapInstance(ref props6, ref i6, ref names, ref used, ref delegates6);
                var t7 = t.MapInstance(ref props7, ref i7, ref names, ref used, ref delegates7);
                var t8 = t.MapInstance(ref props8, ref i8, ref names, ref used, ref delegates8);
                var t9 = t.MapInstance(ref props9, ref i9, ref names, ref used, ref delegates9);
                var t10 = t.MapInstance(ref props10, ref i10, ref names, ref used, ref delegates10);
                var t11 = t.MapInstance(ref props11, ref i11, ref names, ref used, ref delegates11);
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
            var props1 = TypeCache<T1>.GetProperties(type1);
            var props2 = TypeCache<T2>.GetProperties(type2);
            var props3 = TypeCache<T3>.GetProperties(type3);
            var props4 = TypeCache<T4>.GetProperties(type4);
            var props5 = TypeCache<T5>.GetProperties(type5);
            var props6 = TypeCache<T6>.GetProperties(type6);
            var props7 = TypeCache<T7>.GetProperties(type7);
            var props8 = TypeCache<T8>.GetProperties(type8);
            var props9 = TypeCache<T9>.GetProperties(type9);
            var props10 = TypeCache<T10>.GetProperties(type10);
            var props11 = TypeCache<T11>.GetProperties(type11);
            var props12 = TypeCache<T12>.GetProperties(type12);
            Dictionary<string, ushort> names = null;
            var delegates1 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props1.Length];
            var delegates2 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props2.Length];
            var delegates3 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props3.Length];
            var delegates4 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props4.Length];
            var delegates5 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props5.Length];
            var delegates6 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props6.Length];
            var delegates7 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props7.Length];
            var delegates8 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props8.Length];
            var delegates9 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props9.Length];
            var delegates10 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props10.Length];
            var delegates11 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props11.Length];
            var delegates12 = new (Delegate method, bool nullable, TypeCode code, bool isArray, ushort index, StructType structType)[props12.Length];
            foreach (var t in tuples)
            {
                if (names == null)
                {
                    names = GetNamesDictFromTuple(t);
                }
                var used = new HashSet<ushort>(t.Length);
                var i1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var i2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var i3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var i4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var i5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var i6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var i7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var i8 = TypeCache<T8>.CreateInstance(ctorInfo8);
                var i9 = TypeCache<T9>.CreateInstance(ctorInfo9);
                var i10 = TypeCache<T10>.CreateInstance(ctorInfo10);
                var i11 = TypeCache<T11>.CreateInstance(ctorInfo11);
                var i12 = TypeCache<T12>.CreateInstance(ctorInfo12);
                var t1 = t.MapInstance(ref props1, ref i1, ref names, ref used, ref delegates1);
                var t2 = t.MapInstance(ref props2, ref i2, ref names, ref used, ref delegates2);
                var t3 = t.MapInstance(ref props3, ref i3, ref names, ref used, ref delegates3);
                var t4 = t.MapInstance(ref props4, ref i4, ref names, ref used, ref delegates4);
                var t5 = t.MapInstance(ref props5, ref i5, ref names, ref used, ref delegates5);
                var t6 = t.MapInstance(ref props6, ref i6, ref names, ref used, ref delegates6);
                var t7 = t.MapInstance(ref props7, ref i7, ref names, ref used, ref delegates7);
                var t8 = t.MapInstance(ref props8, ref i8, ref names, ref used, ref delegates8);
                var t9 = t.MapInstance(ref props9, ref i9, ref names, ref used, ref delegates9);
                var t10 = t.MapInstance(ref props10, ref i10, ref names, ref used, ref delegates10);
                var t11 = t.MapInstance(ref props11, ref i11, ref names, ref used, ref delegates11);
                var t12 = t.MapInstance(ref props12, ref i12, ref names, ref used, ref delegates12);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            }
        }
    }
}