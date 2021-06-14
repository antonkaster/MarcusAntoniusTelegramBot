using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Abstractions.Services
{
    public interface IContentService
    {
        string Name { get; }
        Task<string> GetTextContent();
        Task<byte[]> GetImageContent();
    }
}
