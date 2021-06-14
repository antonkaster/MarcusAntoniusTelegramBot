using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Configs
{
    public class ContentReplyReactionConfig : BaseReactionConfig
    {
        public string[] ReactionPhrases { get; set; } = new[]
        {
            "Давай я лучше расскажу тебе {name}"
        };

    }
}
