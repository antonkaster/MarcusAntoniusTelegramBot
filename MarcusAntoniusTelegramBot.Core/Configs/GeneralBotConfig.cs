using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Configs
{
    public class GeneralBotConfig
    {
        public string[] WhiteUsers { get; set; } = new string[0];

        public string SelfUserName { get; set; } = "";
    }
}
