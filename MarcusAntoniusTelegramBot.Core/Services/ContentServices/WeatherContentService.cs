using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace MarcusAntoniusTelegramBot.Core.Services.ContentServices
{
    public class WeatherContentService : IContentService
    {
        public string Name => "погоду";

        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<WeatherContentService> _logger;

        private readonly string _weatherImageUrl = "https://ru-meteo.ru/informer/moscow-2-8.png";

        public WeatherContentService(
            IHttpClientFactory clientFactory,
            ILogger<WeatherContentService> logger
            )
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<byte[]> GetImageContent()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _weatherImageUrl);

            HttpResponseMessage response = await _clientFactory.CreateClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                byte[] content = await response.Content.ReadAsByteArrayAsync();

                if (content == null)
                    throw new WebException($"Web page returned null result");

                return content;
            }
            else
            {
                throw new WebException($"Response status {response.StatusCode} {response.ReasonPhrase}");
            }

        }

        public Task<string> GetTextContent() => Task.FromResult<string>(null);
    }
}
