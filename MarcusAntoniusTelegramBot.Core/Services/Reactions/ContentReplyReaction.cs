using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using MarcusAntoniusTelegramBot.Core.Attributes;
using MarcusAntoniusTelegramBot.Core.Configs;
using MarcusAntoniusTelegramBot.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace MarcusAntoniusTelegramBot.Core.Services.Reactions
{
    [ModuleConfiguration(typeof(ContentReplyReactionConfig), "ContentReplyReaction")]
    public class ContentReplyReaction : BaseReaction
    {
        private readonly IEnumerable<IContentService> _contentServices;
        private readonly ContentReplyReactionConfig _reactionConfig;

        public ContentReplyReaction(
            ILogger<ContentReplyReaction> logger, 
            TelegramBotClient client,
            IOptions<ContentReplyReactionConfig> reactionConfig,
            ReactionFactoryService reactionFactoryService,
            IEnumerable<IContentService> contentServices
            ) 
            : base(logger, client, reactionConfig?.Value?.ReactionPattern, reactionFactoryService)
        {
            _contentServices = contentServices;
            _reactionConfig = reactionConfig?.Value ?? throw new ArgumentException($"Reaction config can't be null or empty!");
        }

        protected override bool ValidateMessage(Message message)
        {
            return message.ReplyToMessage != null
                   && message.ReplyToMessage.From.Id == Client.BotId;
        }

        protected override async Task<bool> ProcessMessage(Message message)
        {
            IContentService contentService = _contentServices.RandomItem();

            string text = _reactionConfig.ReactionPhrases
                .RandomItem()
                .Replace("{name}", contentService.Name);

            string textContent = await contentService.GetTextContent();
            if (textContent != null)
                text += $"\r\n\r\n{textContent}";

            byte[] img = await contentService.GetImageContent();

            if (img == null)
            {
                await Client.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId, parseMode: ParseMode.Html);
                return true;
            }
            else
            {
                InputOnlineFile file = new InputMedia(new MemoryStream(img), "img");
                await Client.SendPhotoAsync(message.Chat.Id, file, replyToMessageId: message.MessageId, caption: text);
                return true;
            }
        }
    }
}
