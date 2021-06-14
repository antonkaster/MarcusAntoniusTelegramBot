using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Configs;

namespace MarcusAntoniusTelegramBot.Core.Configs
{
    public class BaseReactionConfig
    {
        public ReactionPatternConfig ReactionPattern { get; set; } = new ReactionPatternConfig()
            { MaxChanceTimeoutMinutes = 60, MinReactionChance = 0, MaxReactionChance = 100 };

    }
}
