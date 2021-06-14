using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Configs
{
    public class SpellCheckReactionConfig : BaseReactionConfig
    {

        public string[] SingleErrorPhrases { get; set; } = new string[]
        {
            "Возможно, имелось ввиду '{right}' вместо '{wrong}'"
        };

        public int WrongCountBound { get; set; } = 5;

        public string[] LessThanWrongPhrases { get; set; } = new string[]
        {
            "Целых {wrongCount_withWord_error}!"
        };

        public string[] MoreThanWrongPhrases { get; set; } = new string[]
        {
            "Во имя Цезаря! Всего {wordsTotal_withWord_word} и целых {wrongCount_withWord_error}!"
        };

        public string[] WrongRightPhrases { get; set; } = new string[]
        {
            "'{right}' вместо '{wrong}'"
        };

    }
}
