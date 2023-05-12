using System;
using System.Linq;

namespace USVStudDocs.BLL.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c) && !Char.IsPunctuation(c) && !Char.IsSymbol(c))
                .ToArray());
        }
    }
}