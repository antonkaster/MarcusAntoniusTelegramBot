using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Configs
{
    public class QuotesContentServiceConfig
    {
        public QuotesList[] Quotes { get; set; } = new QuotesList[]
        {
            new QuotesList()
            {
                Name = "Квинтилиан Марк Фабий",
                List = new []
                {
                    "Лжец должен обладать хорошей памятью"
                }
            }
        };
    }

    public class QuotesList
    {
        public string Name { get; set; } = "";
        public string[] List { get; set; } = new string[] { };
    }
}
