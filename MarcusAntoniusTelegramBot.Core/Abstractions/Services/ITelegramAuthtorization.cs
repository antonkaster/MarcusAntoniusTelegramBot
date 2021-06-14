using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MarcusAntoniusTelegramBot.Core.Abstractions.Services
{
    public interface ITelegramAuthorization
    {
        Task<bool> CheckAccessForMessage(Message telegramMessage);
    }
}
