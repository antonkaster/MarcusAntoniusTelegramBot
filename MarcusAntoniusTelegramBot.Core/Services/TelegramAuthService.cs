using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using MarcusAntoniusTelegramBot.Core.Configs;
using Telegram.Bot.Types;

namespace MarcusAntoniusTelegramBot.Core.Services
{
    public class TelegramAuthService : ITelegramAuthorization
    {
        private readonly GeneralBotConfig _telegramOptions;

        public TelegramAuthService(IOptions<GeneralBotConfig> telegramOptions)
        {
            _telegramOptions = telegramOptions.Value;
        }


        public async Task<bool> CheckAccessForMessage(Message telegramMessage)
        {
            return _telegramOptions.WhiteUsers == null 
                    || _telegramOptions.WhiteUsers.Length == 0
                    || _telegramOptions.WhiteUsers.Contains(telegramMessage.From.Username);
        }
    }
}
