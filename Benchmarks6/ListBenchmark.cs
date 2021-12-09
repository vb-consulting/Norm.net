using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Benchmarks6
{
    public class ListBenchmark
    {
        private int[] _array;

        [GlobalSetup]
        public void Setup()
        {
            _array = new int[10000];
            for (var i = 0; i < 10000; i++)
            {
                _array[i] = i;
            }
        }

        [Benchmark(Baseline = true)]
        public void Foreach()
        {
            foreach (var item in _array)
            {
            }
        }

        [Benchmark]
        public void For()
        {
            for (var i = 0; i < _array.Length; i++)
            {
                _ = _array[i];
            }
        }

        [Benchmark]
        public void Foreach_Span()
        {
            var s1 = new Span<(Type type, string name, PropertyInfo info)>();
            var span = new Span<int>(_array, 0, _array.Length);

            foreach (var item in span)
            {
            }
        }

        private Span<(Type type, string name, PropertyInfo info)> GetSpan()
        {
            return new Span<(Type type, string name, PropertyInfo info)>();
        }

        /*
        [Benchmark]
        public void Foreach_Span_NoMarshal()
        {
            var array = _list.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(_list) as int[];
            var span = new Span<int>(array, 0, array.Length);

            foreach (var item in span)
            {
            }
        }
        */
    }
}
