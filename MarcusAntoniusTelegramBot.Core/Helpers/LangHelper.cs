using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Helpers
{
    public static class LangHelper
    {
        public static readonly string[] Separators = new string[]
        {
            " ", ",", ".", "!", "?", ":", ";",
            "(",")","{","}","[","]","\"","'","`"
        };

        public static string[] SplitInToWords(this string text)
        {
            return text.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
