using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Extensions
{
    public static class LambdaEx
    {
        public static TInput IfNoNull<TInput>(this TInput inputObject, Action action)
            where  TInput : class
        {
            if (inputObject != null)
                action.Invoke();

            return inputObject;
        }
    }
}
