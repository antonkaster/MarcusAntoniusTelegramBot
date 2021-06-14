using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Attributes;
using MarcusAntoniusTelegramBot.Core.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MarcusAntoniusTelegramBot.Core.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MarcusAntoniusTelegramBot.Core.Services.Reactions
{
    [ModuleConfiguration(typeof(SimpleReplyReactionConfig), "SimpleReplyReaction")]
    public class SimpleReplyReaction: BaseReaction
    {
        private readonly SimpleReplyReactionConfig _reactionConfig;

        public SimpleReplyReaction(
            ILogger<SimpleReplyReaction> logger, 
            TelegramBotClient client, 
            IOptions<SimpleReplyReactionConfig> reactionConfig, 
            ReactionFactoryService reactionFactoryService
            ) 
            : base(logger, client, reactionConfig?.Value?.ReactionPattern, reactionFactoryService)
        {
            _reactionConfig = reactionConfig?.Value ?? throw new ArgumentException($"Reaction config can't be null or empty!"); ;
        }

        protected override bool ValidateMessage(Message message)
        {
            return message.ReplyToMessage != null
                   && message.ReplyToMessage.From.Id == Client.BotId;
        }

        protected override async Task<bool> ProcessMessage(Message message)
        {
            string text = _reactionConfig.ReactionPhrases.RandomItem();
            await Client.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
            return true;
        }
    }
}
