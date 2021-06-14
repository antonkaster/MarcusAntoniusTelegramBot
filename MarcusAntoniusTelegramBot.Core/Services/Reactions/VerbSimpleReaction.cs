using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Attributes;
using MarcusAntoniusTelegramBot.Core.Configs;
using MarcusAntoniusTelegramBot.Core.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MarcusAntoniusTelegramBot.Core.Extensions;
using MarcusAntoniusTelegramBot.Core.Services;
using MarcusAntoniusTelegramBot.Core.Services.Reactions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MarcusAntoniusTelegramBot.Core.Services.Reactions
{
    [ModuleConfiguration(typeof(VerbSimpleReactionConfig), "VerbSimpleReaction")]
    public class VerbSimpleReaction : BaseReaction
    {
        private readonly VerbSimpleReactionConfig _reactionOptions;

        public VerbSimpleReaction(
            ILogger<VerbSimpleReaction> logger, 
            TelegramBotClient client,
            IOptions<VerbSimpleReactionConfig> reactionOptions, 
            ReactionFactoryService reactionFactoryService
            ) : base(logger, client, reactionOptions?.Value?.ReactionPattern, reactionFactoryService)
        {
            _reactionOptions = reactionOptions?.Value ?? throw new ArgumentException($"Reaction options cant' be null or empty!");
        }

        protected override bool ValidateMessage(Message message)
        {
            return message.Text != null 
                   && message.Text.Length >= 10;
        }

        protected override async Task<bool> ProcessMessage(Message message)
        {
            foreach (var verbDict in _reactionOptions.VerbDict)
            {
                string selectedVerb = message.Text
                    .SplitInToWords()
                    .Select(w => w.ToLower())
                    .Where(w => w.Length > 4 && verbDict.VerbEndings.Any(w.EndsWith))
                    .RandomItem();

                if (string.IsNullOrWhiteSpace(selectedVerb))
                    continue;

                string text = verbDict.VerbPhrases
                    .RandomItem()
                    .Replace("{verb}", selectedVerb);

                await Client.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
                return true;
            }

            return false;
        }
    }
}
