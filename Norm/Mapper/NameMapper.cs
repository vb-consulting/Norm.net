using System;

namespace Norm.Mapper
{
    public class NameParser
    {
        public static void Parse(ref string input)
        {
            if (NormOptions.Value.KeepOriginalNames)
            {
                input = input.ToLowerInvariant();
                return;
            }
            var result = new Span<char>(new char[input.Length]);
            int index = 0;
            for (int i = 0; i < input.Length; i++)
            {
                var ch = input[i];
                if (ch != '@' && ch != '_')
                {
                    result[index] = char.ToLowerInvariant(input[i]);
                    index++;
                }
            }
            input = result[..index].ToString();
        }
    }
}