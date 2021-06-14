using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;

namespace MarcusAntoniusTelegramBot.Core.Services
{
    public class ChatScopeResolver : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<long, IServiceScope> _scopes;

        public ChatScopeResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _scopes = new Dictionary<long, IServiceScope>();
        }

        public IServiceScope GetScope(long chatId)
        {
            if (!_scopes.ContainsKey(chatId)) 
                _scopes[chatId] = _serviceProvider
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();

            return _scopes[chatId];
        }

        private bool disposed = false;
        public void Dispose()
        {
            if (disposed) 
                return;

            disposed = true;
            foreach (var serviceScope in _scopes.Values)
                serviceScope.Dispose();
        }
    }
}
