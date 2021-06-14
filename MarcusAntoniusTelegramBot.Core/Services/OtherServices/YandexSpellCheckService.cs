using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using MarcusAntoniusTelegramBot.Core.Models;
using Microsoft.Extensions.Logging;

namespace MarcusAntoniusTelegramBot.Core.Services.OtherServices
{
    /// <summary>
    /// https://yandex.ru/dev/speller/doc/dg/reference/checkText.html
    /// </summary>
    public class YandexSpellCheckService : ISpellCheckService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<YandexSpellCheckService> _logger;
        private readonly string yandexSpellCheckApiUrl = "https://speller.yandex.net/services/spellservice.json/checkText?text={text}";

        public YandexSpellCheckService(
            IHttpClientFactory clientFactory,
            ILogger<YandexSpellCheckService> logger
            )
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<SpellCheckResult> CheckSpell(string text)
        {
            string requestString = yandexSpellCheckApiUrl.Replace("{text}", HttpUtility.UrlEncode(text));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestString);

            HttpResponseMessage response = await _clientFactory.CreateClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                IEnumerable<YandexSpellCheckResult> result =
                    await JsonSerializer.DeserializeAsync<IEnumerable<YandexSpellCheckResult>>(responseStream);

                if (result == null)
                {
                    _logger.LogError($"Returned null result!");
                    return new SpellCheckResult();
                }

                SpellCheckResult spellCheckResult = new SpellCheckResult();
                foreach (var item in result)
                    spellCheckResult.Corrections[item.word] = item.s.First();

                return spellCheckResult;
            }
            else
            {
                _logger.LogError($"Response status {response.StatusCode} {response.ReasonPhrase}");
                return new SpellCheckResult();
            }
        }
    }

    public class YandexSpellCheckResult
    {
        public int code { get; set; }
        public int pos { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        public int len { get; set; }
        public string word { get; set; } = "";
        public string[] s { get; set; } = new string[0];

    }
}
