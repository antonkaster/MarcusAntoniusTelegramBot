using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using MarcusAntoniusTelegramBot.Core.Attributes;
using MarcusAntoniusTelegramBot.Core.Configs;
using MarcusAntoniusTelegramBot.Core.Extensions;
using MarcusAntoniusTelegramBot.Core.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MarcusAntoniusTelegramBot.Core.Models;
using MarcusAntoniusTelegramBot.Core.Services;
using MarcusAntoniusTelegramBot.Core.Services.Reactions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MarcusAntoniusTelegramBot.Core.Services.Reactions
{
    [ModuleConfiguration(typeof(SpellCheckReactionConfig), "SpellCheckReaction")]
    public class SpellCheckReaction : BaseReaction
    {
        private static readonly Dictionary<string, string[]> Words = new Dictionary<string, string[]>()
        {
            { "ошибка", new string[3]{ "ошибка", "ошибки", "ошибок" }},
            { "слово", new string[3]{ "слово", "слова", "слов" }}
        };

        private readonly SpellCheckReactionConfig _reactionOptions;
        private readonly ISpellCheckService _spellCheckService;

        public SpellCheckReaction(
            ILogger<SpellCheckReaction> logger, 
            TelegramBotClient client,
            IOptions<SpellCheckReactionConfig> reactionOptions, 
            ReactionFactoryService reactionFactoryService,
            ISpellCheckService spellCheckService
        )
            : base(logger, client, reactionOptions?.Value?.ReactionPattern, reactionFactoryService)
        {
            _reactionOptions = reactionOptions?.Value ?? throw new ArgumentNullException($"Reaction config can't be null!");
            _spellCheckService = spellCheckService;

            if (_reactionOptions.SingleErrorPhrases == null || _reactionOptions.SingleErrorPhrases.Length == 0)
                throw new ArgumentException($"SpellCheckPhrases can't be null or empty!");
        }

        protected override bool ValidateMessage(Message message)
        {
            return message.Text != null
                   && message.Text.Length >= 10;
        }

        protected override async Task<bool> ProcessMessage(Message message)
        {
            SpellCheckResult result = await _spellCheckService.CheckSpell(message.Text);

            int wrongCount = result.Corrections.Keys.Count;

            if (wrongCount == 0)
                return false;

            string text = "";

            if (wrongCount == 1)
            {
                string wrong = result.Corrections.Keys.RandomItem();
                text = _reactionOptions.SingleErrorPhrases.RandomItem()
                    .Replace("{wrong_right}", _reactionOptions.WrongRightPhrases.RandomItem())
                    .Replace("{wrong}", wrong)
                    .Replace("{right}", result.Corrections[wrong]);
            }
            else
            {
                int wordsTotal = message.Text.SplitInToWords().Length;

                text = (wrongCount > _reactionOptions.WrongCountBound ||  wrongCount / (float)wordsTotal >= 0.5
                        ? _reactionOptions.MoreThanWrongPhrases : _reactionOptions.LessThanWrongPhrases)
                    .RandomItem()
                    .Replace("{wordsTotal}", wordsTotal.ToString())
                    .Replace("{wrongCount}", wrongCount.ToString())
                    .Replace("{wordsTotal_withWord_word}", GetCountWithCorrectWordForm("слово", wordsTotal))
                    .Replace("{wrongCount_withWord_error}", GetCountWithCorrectWordForm("ошибка", wrongCount));

                //text += "\r\n";
                int i = 0;
                foreach (var wrong in result.Corrections.Keys)
                {
                    i++;
                    text += $"\r\n {i.ToRoman()}. " + 
                            _reactionOptions.WrongRightPhrases.RandomItem()
                                .Replace("{wrong}", wrong)
                                .Replace("{right}", result.Corrections[wrong]);
                }
            }

            await Client.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);

            return true;
        }

        private static string GetCountWithCorrectWordForm(string baseWord, int count)
        {
            baseWord = baseWord.ToLower();
            if (!Words.ContainsKey(baseWord))
                throw new ArgumentException($"Unknown word '{baseWord}'");

            string word = Words[baseWord][(int)GetWordForm(count)];
            return $"{count} {word}";
        }

        private static WordForm GetWordForm(int count)
        {
            if(count == 11 || count == 12 || count == 13 || count == 14)
                return WordForm.Three;

            switch (count % 10)
            {
                case 1:
                    return WordForm.One;
                case 2:
                case 3:
                case 4:
                    return WordForm.Two;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 0:
                    return WordForm.Three;
            }

            throw new ArgumentOutOfRangeException();
        }

        private enum WordForm
        {
            One = 0,
            Two = 1,
            Three = 2
        }
    }
}
