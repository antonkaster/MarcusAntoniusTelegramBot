using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Models;

namespace MarcusAntoniusTelegramBot.Core.Abstractions.Services
{
    public interface ISpellCheckService
    {
        Task<SpellCheckResult> CheckSpell(string text);
    }
}
