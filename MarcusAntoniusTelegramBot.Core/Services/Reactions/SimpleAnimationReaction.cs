using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Attributes;
using MarcusAntoniusTelegramBot.Core.Configs;
using MarcusAntoniusTelegramBot.Core.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;

namespace MarcusAntoniusTelegramBot.Core.Services.Reactions
{
    [ModuleConfiguration(typeof(SimpleAnimationReactionConfig), "SimpleAnimationReaction")]
    public class SimpleAnimationReaction : BaseReaction
    {
        private readonly SimpleAnimationReactionConfig _reactionConfig;
        private readonly string _animationPath = "";

        public SimpleAnimationReaction(
            ILogger<BaseReaction> logger,
            TelegramBotClient client,
            IOptions<SimpleAnimationReactionConfig> reactionConfig,
            ReactionFactoryService reactionFactoryService,
            IHostEnvironment env
            ) 
            : base(logger, client, reactionConfig?.Value?.ReactionPattern, reactionFactoryService)
        {
            _reactionConfig = reactionConfig?.Value ?? throw new ArgumentNullException($"Animation reactions config can't be null!");
            _animationPath = Path.Combine(env.ContentRootPath, "Content" , _reactionConfig.AnimationPath);

            if (!Directory.Exists(_animationPath))
                throw new DirectoryNotFoundException($"Animation path '{_animationPath}' not found!");
        }

        protected override bool ValidateMessage(Message message)
        {
            return message.Animation != null
                || message.ReplyToMessage?.From.Id == Client.BotId;
        }

        protected override async Task<bool> ProcessMessage(Message message)
        {
            string fileName = Directory.GetFiles(_animationPath).RandomItem();

            if (string.IsNullOrWhiteSpace(fileName))
                throw new FileNotFoundException($"Animation file not found!");

            InputOnlineFile file = new InputMedia(File.OpenRead(fileName),Path.GetFileName(fileName));
            await Client.SendAnimationAsync(message.Chat.Id, file, replyToMessageId: message.MessageId);

            return true;
        }
    }
}
