using System;
using System.Linq;

namespace UI.Logic.Extension
{
    public static class StringExt
    {
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("String is null or empty");

            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}