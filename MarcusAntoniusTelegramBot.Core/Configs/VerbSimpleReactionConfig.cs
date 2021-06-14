using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Configs
{
    public class VerbSimpleReactionConfig : BaseReactionConfig
    {
        public VerbDictConfig[] VerbDict { get; set; } = new VerbDictConfig[0];
    }

    public class VerbDictConfig
    {
        public string[] VerbEndings { get; set; } = new string[] { };

        public string[] VerbPhrases { get; set; } = new string[] { };
    }
}
