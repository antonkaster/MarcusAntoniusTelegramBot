using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using MarcusAntoniusTelegramBot.Core.Attributes;
using MarcusAntoniusTelegramBot.Core.Configs;
using MarcusAntoniusTelegramBot.Core.Extensions;
using Microsoft.Extensions.Options;

namespace MarcusAntoniusTelegramBot.Core.Services.ContentServices
{
    [ModuleConfiguration(typeof(QuotesContentServiceConfig), "QuotesContentService")]
    public class QuotesContentService : IContentService
    {
        //https://ra-ja.ru/glavnaya/aforizmy/vyskazyvaniya-rimskih-filosofov/
        //http://www.aphorisme.ru/personalii/filos-rim/?q=3775&dp=184

        public string Name
        {
            get => "что однажды сказал";
        }

        private readonly QuotesContentServiceConfig _serviceOptions;

        public QuotesContentService(
            IOptions<QuotesContentServiceConfig> serviceOptions
            )
        {
            _serviceOptions = serviceOptions?.Value ?? throw  new ArgumentNullException($"Content options can't be null!");
        }

        public async Task<string> GetTextContent()
        {
            QuotesList quotesList = _serviceOptions.Quotes
                            .RandomItem();
            return $"{quotesList.Name}\r\n\r\n<i>« {quotesList.List.RandomItem()} »</i>";
        }

        public Task<byte[]> GetImageContent() => Task.FromResult<byte[]>(null);
    }
}
