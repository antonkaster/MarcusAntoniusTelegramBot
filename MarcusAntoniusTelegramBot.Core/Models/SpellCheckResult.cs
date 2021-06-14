using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Models
{
    public class SpellCheckResult
    {
        public Dictionary<string, string> Corrections = new Dictionary<string, string>();
    }
}
