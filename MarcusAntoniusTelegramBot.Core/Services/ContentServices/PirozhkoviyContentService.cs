using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace MarcusAntoniusTelegramBot.Core.Services.ContentServices
{
    public class PirozhkoviyContentService : IContentService
    {
        public string Name => "пирожок";

        private readonly string url = "http://perashki.ru/Piro/Random/";

        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<PirozhkoviyContentService> _logger;

        public PirozhkoviyContentService(
            IHttpClientFactory clientFactory,
            ILogger<PirozhkoviyContentService> logger
        )
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<string> GetTextContent()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response = await _clientFactory.CreateClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string pageContent = await response.Content.ReadAsStringAsync();

                if (pageContent == null)
                    throw new WebException($"Web page returned null result");

                HtmlDocument pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContent);

                string text = pageDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'Text')]//div").InnerText.Trim();

                if (pageContent == null)
                    throw new WebException($"Pirozhok not found!");

                return text;
            }
            else
            {
                throw new WebException($"Response status {response.StatusCode} {response.ReasonPhrase}");
            }

        }

        public Task<byte[]> GetImageContent() => Task.FromResult<byte[]>(null);
    }
}
