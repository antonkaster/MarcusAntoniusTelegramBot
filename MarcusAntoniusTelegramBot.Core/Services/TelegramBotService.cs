using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using MarcusAntoniusTelegramBot.Core.Configs;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace MarcusAntoniusTelegramBot.Core.Services
{
    public class TelegramBotService : IHostedService
    {
        private readonly ILogger<TelegramBotService> _logger;
        private readonly ITelegramAuthorization _telegramAuthorization;
        private readonly ChatScopeResolver _chatScopeResolver;
        private readonly TelegramBotClient _telegramBot;

        private readonly TimeSpan messageIgnoreTimeout = new TimeSpan(0, 0, 0, 10);

        public TelegramBotService(
            TelegramBotClient telegramBotClient,
            ILogger<TelegramBotService> logger, 
            ITelegramAuthorization telegramAuthorization,
            ChatScopeResolver chatScopeResolver
            )
        {
            _logger = logger;
            _telegramAuthorization = telegramAuthorization;
            _chatScopeResolver = chatScopeResolver;

            _telegramBot = telegramBotClient;
            _telegramBot.OnMessage += TelegramBot_OnMessage;
            _telegramBot.OnMessageEdited += TelegramBot_OnMessageEdited;
        }

        private async void TelegramBot_OnMessageEdited(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            //_log.LogInformation($"{e.Message.From.Username} [edited]: {e.Message.Text}");
        }

        private async void TelegramBot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (!await _telegramAuthorization.CheckAccessForMessage(e.Message))
                return;

            if (DateTime.Now - e.Message.Date.ToLocalTime() > messageIgnoreTimeout)
                return;
            
            _logger.LogInformation($"[{DateTime.Now}] {e.Message.Chat.Title}-{e.Message.From.Username} : [{e.Message.Type}] {e.Message.Text}");

            var serviceScope = _chatScopeResolver.GetScope(e.Message.Chat.Id);

            ScopedChatTelegramMessageProcessor telegramMessageProcessor =
                serviceScope.ServiceProvider.GetRequiredService<ScopedChatTelegramMessageProcessor>();

            Task.Run(async () => await telegramMessageProcessor.ProcessMessage(e.Message));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _telegramBot.StartReceiving(cancellationToken: cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _telegramBot.StopReceiving();
            return  Task.CompletedTask;
        }
    }
}
