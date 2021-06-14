using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Extensions
{
    public static class ArrayEx
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        public static TResult RandomItem<TResult>(this IEnumerable<TResult> arr)
        {
            List<TResult> list = arr.ToList();
            return list.Count == 0 ? default(TResult) : list[_random.Next(0, list.Count)];
        }
    }
}
