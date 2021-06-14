using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Extensions
{
    public static class DigitsEx
    {
        private static Dictionary<int, string> Ra = new Dictionary<int, string>
        { { 1000, "M" },  { 900, "CM" },  { 500, "D" },  { 400, "CD" },  { 100, "C" },
            { 90 , "XC" },  { 50 , "L" },  { 40 , "XL" },  { 10 , "X" },
            { 9  , "IX" },  { 5  , "V" },  { 4  , "IV" },  { 1  , "I" } };

        public static string ToRoman(this int number) => Ra
            .Where(d => number >= d.Key)
            .Select(d => d.Value + ToRoman(number - d.Key))
            .FirstOrDefault();
    }
}
