using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MarcusAntoniusTelegramBot.Core.Abstractions.Reactions;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using MarcusAntoniusTelegramBot.Core.Configs;
using MarcusAntoniusTelegramBot.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MarcusAntoniusTelegramBot.Core.Services.Reactions
{
    public abstract class BaseReaction : IReaction
    {
        protected readonly ILogger<BaseReaction> Logger;
        protected readonly TelegramBotClient Client;
        private readonly Func<Message, Task<bool>> reaction;

        protected BaseReaction(
            ILogger<BaseReaction> logger,
            TelegramBotClient client,
            ReactionPatternConfig reactionConfig,
            ReactionFactoryService reactionFactoryService
        )
        {
            Logger = logger;
            Client = client;

            reaction = reactionFactoryService.RegisterReactionFunction(
                reactionConfig,
                ProcessMessage);
        }

        public async Task<bool> Reaction(Message message)
        {
            if(ValidateMessage(message))
                return await reaction(message);

            return false;
        }

        protected abstract bool ValidateMessage(Message message);

        protected abstract Task<bool> ProcessMessage(Message message);
    }
}
