using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace MarcusAntoniusTelegramBot.Core.Abstractions.Reactions
{
    public interface IReaction
    {
        Task<bool> Reaction(Message message);
    }
}
