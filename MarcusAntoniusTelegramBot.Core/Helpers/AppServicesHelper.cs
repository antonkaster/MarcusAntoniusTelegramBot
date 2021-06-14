using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Configuration;

namespace MarcusAntoniusTelegramBot.Core.Helpers
{
    public static class AppServicesHelper
    {
        private static IConfiguration _configuration = null;

        public static IConfiguration Configuration
        {
            get => _configuration;
            set
            {
                if (_configuration != null)
                    throw new Exception($"Configuration already applied!");
                _configuration = value;
            }
        }

    }
}
