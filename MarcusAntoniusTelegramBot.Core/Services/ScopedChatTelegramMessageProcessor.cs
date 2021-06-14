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
using Telegram.Bot;
using Telegram.Bot.Types;
using MarcusAntoniusTelegramBot.Core.Extensions;
using MarcusAntoniusTelegramBot.Core.Models;

namespace MarcusAntoniusTelegramBot.Core.Services
{
    public class ScopedChatTelegramMessageProcessor 
    {
        private readonly TelegramBotClient _client;
        private readonly ILogger<ScopedChatTelegramMessageProcessor> _logger;
        private readonly IEnumerable<IReaction> _reactions;

        private readonly Random _rand;


        public ScopedChatTelegramMessageProcessor(
            TelegramBotClient telegramBotClient,
            ILogger<ScopedChatTelegramMessageProcessor> logger,
            IEnumerable<IReaction> reactions
            )
        {
            _client = telegramBotClient;
            _logger = logger;
            _reactions = reactions;
            _rand = new Random(DateTime.Now.Millisecond);

        }

        public async Task ProcessMessage(Message message)
        {
            foreach (var reaction in _reactions)
            {
                try
                {
                    bool result = await reaction.Reaction(message);
                    if(result)
                        break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error on reaction");
                }
            }
        }

    }
}
