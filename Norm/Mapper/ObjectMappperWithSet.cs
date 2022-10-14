using System;
using System.Collections.Generic;

namespace Norm.Mapper
{
    public static partial class NormExtensions
    {
        public static IEnumerable<T> Map<T>(this IEnumerable<(string name, object value, bool set)[]> tuples,
            Type type1)
        {
            var ctorInfo1 = TypeCache<T>.GetCtorInfo(type1);
            Dictionary<string, ushort> names = null;
            var namecounts = new Dictionary<string, ushort>();
            var delegates = CreateDelegateArray(TypeCache<T>.GetPropertiesLength());
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                HashSet<ushort> used = null;
                var t1 = TypeCache<T>.CreateInstance(ctorInfo1);
                t.MapInstance(ref t1, ref names, ref used, ref delegates);
                yield return t1;
            }
        }

        public static IEnumerable<(T1, T2)> Map<T1, T2>(this IEnumerable<(string name, object value, bool set)[]> tuples,
            Type type1,
            Type type2)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);

            Dictionary<string, ushort> names = null;
            var namecounts = new Dictionary<string, ushort>();
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                yield return (t1, t2);
            }
        }

        public static IEnumerable<(T1, T2, T3)> Map<T1, T2, T3>(this IEnumerable<(string name, object value, bool set)[]> tuples,
            Type type1,
            Type type2,
            Type type3)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);

            Dictionary<string, ushort> names = null;
            var namecounts = new Dictionary<string, ushort>();
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                t.MapInstance(ref t3, ref names, ref used, ref delegates3);
                yield return (t1, t2, t3);
            }
        }

        public static IEnumerable<(T1, T2, T3, T4)> Map<T1, T2, T3, T4>(this IEnumerable<(string name, object value, bool set)[]> tuples,
            Type type1,
            Type type2,
            Type type3,
            Type type4)
        {
            var ctorInfo1 = TypeCache<T1>.GetCtorInfo(type1);
            var ctorInfo2 = TypeCache<T2>.GetCtorInfo(type2);
            var ctorInfo3 = TypeCache<T3>.GetCtorInfo(type3);
            var ctorInfo4 = TypeCache<T4>.GetCtorInfo(type4);

            Dictionary<string, ushort> names = null;
            var namecounts = new Dictionary<string, ushort>();
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                t.MapInstance(ref t3, ref names, ref used, ref delegates3);
                t.MapInstance(ref t4, ref names, ref used, ref delegates4);
                yield return (t1, t2, t3, t4);
            }
        }

        public static IEnumerable<(T1, T2, T3, T4, T5)> Map<T1, T2, T3, T4, T5>(this IEnumerable<(string name, object value, bool set)[]> tuples,
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

            Dictionary<string, ushort> names = null;
            var namecounts = new Dictionary<string, ushort>();
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                t.MapInstance(ref t3, ref names, ref used, ref delegates3);
                t.MapInstance(ref t4, ref names, ref used, ref delegates4);
                t.MapInstance(ref t5, ref names, ref used, ref delegates5);
                yield return (t1, t2, t3, t4, t5);
            }
        }

        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> Map<T1, T2, T3, T4, T5, T6>(this IEnumerable<(string name, object value, bool set)[]> tuples,
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

            Dictionary<string, ushort> names = null;
            var namecounts = new Dictionary<string, ushort>();
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                t.MapInstance(ref t3, ref names, ref used, ref delegates3);
                t.MapInstance(ref t4, ref names, ref used, ref delegates4);
                t.MapInstance(ref t5, ref names, ref used, ref delegates5);
                t.MapInstance(ref t6, ref names, ref used, ref delegates6);
                yield return (t1, t2, t3, t4, t5, t6);
            }
        }

        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Map<T1, T2, T3, T4, T5, T6, T7>(this IEnumerable<(string name, object value, bool set)[]> tuples,
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

            Dictionary<string, ushort> names = null;
                var namecounts = new Dictionary<string, ushort>();
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            var delegates7 = CreateDelegateArray(TypeCache<T7>.GetPropertiesLength());
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                t.MapInstance(ref t3, ref names, ref used, ref delegates3);
                t.MapInstance(ref t4, ref names, ref used, ref delegates4);
                t.MapInstance(ref t5, ref names, ref used, ref delegates5);
                t.MapInstance(ref t6, ref names, ref used, ref delegates6);
                t.MapInstance(ref t7, ref names, ref used, ref delegates7);
                yield return (t1, t2, t3, t4, t5, t6, t7);
            }
        }

        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Map<T1, T2, T3, T4, T5, T6, T7, T8>(this IEnumerable<(string name, object value, bool set)[]> tuples,
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

            Dictionary<string, ushort> names = null;
            var namecounts = new Dictionary<string, ushort>();
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            var delegates7 = CreateDelegateArray(TypeCache<T7>.GetPropertiesLength());
            var delegates8 = CreateDelegateArray(TypeCache<T8>.GetPropertiesLength());
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var t8 = TypeCache<T8>.CreateInstance(ctorInfo8);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                t.MapInstance(ref t3, ref names, ref used, ref delegates3);
                t.MapInstance(ref t4, ref names, ref used, ref delegates4);
                t.MapInstance(ref t5, ref names, ref used, ref delegates5);
                t.MapInstance(ref t6, ref names, ref used, ref delegates6);
                t.MapInstance(ref t7, ref names, ref used, ref delegates7);
                t.MapInstance(ref t8, ref names, ref used, ref delegates8);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8);
            }
        }

        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IEnumerable<(string name, object value, bool set)[]> tuples,
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

            Dictionary<string, ushort> names = null;
            var namecounts = new Dictionary<string, ushort>();
            var delegates1 = CreateDelegateArray(TypeCache<T1>.GetPropertiesLength());
            var delegates2 = CreateDelegateArray(TypeCache<T2>.GetPropertiesLength());
            var delegates3 = CreateDelegateArray(TypeCache<T3>.GetPropertiesLength());
            var delegates4 = CreateDelegateArray(TypeCache<T4>.GetPropertiesLength());
            var delegates5 = CreateDelegateArray(TypeCache<T5>.GetPropertiesLength());
            var delegates6 = CreateDelegateArray(TypeCache<T6>.GetPropertiesLength());
            var delegates7 = CreateDelegateArray(TypeCache<T7>.GetPropertiesLength());
            var delegates8 = CreateDelegateArray(TypeCache<T8>.GetPropertiesLength());
            var delegates9 = CreateDelegateArray(TypeCache<T9>.GetPropertiesLength());
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var t8 = TypeCache<T8>.CreateInstance(ctorInfo8);
                var t9 = TypeCache<T9>.CreateInstance(ctorInfo9);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                t.MapInstance(ref t3, ref names, ref used, ref delegates3);
                t.MapInstance(ref t4, ref names, ref used, ref delegates4);
                t.MapInstance(ref t5, ref names, ref used, ref delegates5);
                t.MapInstance(ref t6, ref names, ref used, ref delegates6);
                t.MapInstance(ref t7, ref names, ref used, ref delegates7);
                t.MapInstance(ref t8, ref names, ref used, ref delegates8);
                t.MapInstance(ref t9, ref names, ref used, ref delegates9);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9);
            }
        }

        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IEnumerable<(string name, object value, bool set)[]> tuples,
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

            Dictionary<string, ushort> names = null;
            var namecounts = new Dictionary<string, ushort>();
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
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var t8 = TypeCache<T8>.CreateInstance(ctorInfo8);
                var t9 = TypeCache<T9>.CreateInstance(ctorInfo9);
                var t10 = TypeCache<T10>.CreateInstance(ctorInfo10);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                t.MapInstance(ref t3, ref names, ref used, ref delegates3);
                t.MapInstance(ref t4, ref names, ref used, ref delegates4);
                t.MapInstance(ref t5, ref names, ref used, ref delegates5);
                t.MapInstance(ref t6, ref names, ref used, ref delegates6);
                t.MapInstance(ref t7, ref names, ref used, ref delegates7);
                t.MapInstance(ref t8, ref names, ref used, ref delegates8);
                t.MapInstance(ref t9, ref names, ref used, ref delegates9);
                t.MapInstance(ref t10, ref names, ref used, ref delegates10);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            }
        }

        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this IEnumerable<(string name, object value, bool set)[]> tuples,
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

            Dictionary<string, ushort> names = null;
            var namecounts = new Dictionary<string, ushort>();
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
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var t8 = TypeCache<T8>.CreateInstance(ctorInfo8);
                var t9 = TypeCache<T9>.CreateInstance(ctorInfo9);
                var t10 = TypeCache<T10>.CreateInstance(ctorInfo10);
                var t11 = TypeCache<T11>.CreateInstance(ctorInfo11);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                t.MapInstance(ref t3, ref names, ref used, ref delegates3);
                t.MapInstance(ref t4, ref names, ref used, ref delegates4);
                t.MapInstance(ref t5, ref names, ref used, ref delegates5);
                t.MapInstance(ref t6, ref names, ref used, ref delegates6);
                t.MapInstance(ref t7, ref names, ref used, ref delegates7);
                t.MapInstance(ref t8, ref names, ref used, ref delegates8);
                t.MapInstance(ref t9, ref names, ref used, ref delegates9);
                t.MapInstance(ref t10, ref names, ref used, ref delegates10);
                t.MapInstance(ref t11, ref names, ref used, ref delegates11);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            }
        }

        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this IEnumerable<(string name, object value, bool set)[]> tuples,
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

            Dictionary<string, ushort> names = null;

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
            foreach (var t in tuples)
            {
                names ??= GetNamesDictFromTuple(t);
                var used = new HashSet<ushort>(t.Length);
                var t1 = TypeCache<T1>.CreateInstance(ctorInfo1);
                var t2 = TypeCache<T2>.CreateInstance(ctorInfo2);
                var t3 = TypeCache<T3>.CreateInstance(ctorInfo3);
                var t4 = TypeCache<T4>.CreateInstance(ctorInfo4);
                var t5 = TypeCache<T5>.CreateInstance(ctorInfo5);
                var t6 = TypeCache<T6>.CreateInstance(ctorInfo6);
                var t7 = TypeCache<T7>.CreateInstance(ctorInfo7);
                var t8 = TypeCache<T8>.CreateInstance(ctorInfo8);
                var t9 = TypeCache<T9>.CreateInstance(ctorInfo9);
                var t10 = TypeCache<T10>.CreateInstance(ctorInfo10);
                var t11 = TypeCache<T11>.CreateInstance(ctorInfo11);
                var t12 = TypeCache<T12>.CreateInstance(ctorInfo12);
                t.MapInstance(ref t1, ref names, ref used, ref delegates1);
                t.MapInstance(ref t2, ref names, ref used, ref delegates2);
                t.MapInstance(ref t3, ref names, ref used, ref delegates3);
                t.MapInstance(ref t4, ref names, ref used, ref delegates4);
                t.MapInstance(ref t5, ref names, ref used, ref delegates5);
                t.MapInstance(ref t6, ref names, ref used, ref delegates6);
                t.MapInstance(ref t7, ref names, ref used, ref delegates7);
                t.MapInstance(ref t8, ref names, ref used, ref delegates8);
                t.MapInstance(ref t9, ref names, ref used, ref delegates9);
                t.MapInstance(ref t10, ref names, ref used, ref delegates10);
                t.MapInstance(ref t11, ref names, ref used, ref delegates11);
                t.MapInstance(ref t12, ref names, ref used, ref delegates12);
                yield return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            }
        }
    }
}