using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Configs
{
    public class ReactionPatternConfig
    {
        public int MaxChanceTimeoutMinutes { get; set; } = 10;

        private TimeSpan _maxChanceTimeout = TimeSpan.Zero;
        public TimeSpan MaxChanceTimeout
        {
            get
            {
                if(_maxChanceTimeout == TimeSpan.Zero)
                    _maxChanceTimeout = new TimeSpan(0, 0, MaxChanceTimeoutMinutes, 0);
                return _maxChanceTimeout;
            }
        }

        public int MaxReactionChance { get; set; } = 90;
        public int MinReactionChance { get; set; } = 30;

    }
}
