using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Attributes;
using MarcusAntoniusTelegramBot.Core.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MarcusAntoniusTelegramBot.Core.Services.Reactions
{
    [ModuleConfiguration(typeof(DiceReactionConfig), "DiceReaction")]
    public class DiceReaction : BaseReaction
    {
        public DiceReaction(
            ILogger<DiceReaction> logger, 
            TelegramBotClient client, 
            IOptions<DiceReactionConfig> reactionOptions, 
            ReactionFactoryService reactionFactoryService)
            : base(logger, client, reactionOptions?.Value?.ReactionPattern, reactionFactoryService)
        {
        }

        protected override bool ValidateMessage(Message message)
        {
            return message.Dice != null;
        }

        protected override async Task<bool> ProcessMessage(Message message)
        {
            Logger.LogInformation("Dice!");
            await Client.SendDiceAsync(message.Chat.Id);

            return true;
        }

    }
}
