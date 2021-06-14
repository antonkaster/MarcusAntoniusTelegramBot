using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Configs
{
    public class SimpleAnimationReactionConfig : BaseReactionConfig
    {
        public string AnimationPath { get; set; } = "Animations";
    }
}
