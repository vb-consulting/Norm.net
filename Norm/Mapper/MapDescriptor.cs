using System.Collections.Generic;

namespace Norm.Mapper
{
    internal class MapDescriptor
    {
        public Dictionary<string, ushort[]> Names;
        public HashSet<ushort> Used;
        public int Length;

        public void Reset()
        {
            Used = new HashSet<ushort>(Length);
        }
    }
}