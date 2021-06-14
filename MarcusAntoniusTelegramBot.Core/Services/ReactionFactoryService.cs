using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MarcusAntoniusTelegramBot.Core.Configs;
using Telegram.Bot.Types;

namespace MarcusAntoniusTelegramBot.Core.Services
{
    public class ReactionFactoryService
    {
        private readonly ILoggerFactory _log;

        public ReactionFactoryService(
            ILoggerFactory logger
            )
        {
            _log = logger;
        }

        public Func<Message,Task<bool>> RegisterReactionFunction(ReactionPatternConfig config, Func<Message,Task<bool>> reactionFunc)
        {
            return new ReactionExecuter(config, reactionFunc, _log.CreateLogger<ReactionExecuter>()).CheckAndDo;
        }

        private class ReactionExecuter
        {
            private readonly ReactionPatternConfig _config;
            private readonly Func<Message,Task<bool>> _reactionFunc;
            private readonly ILogger<ReactionExecuter> _logger;

            private readonly Random _rand;
            private DateTime _lastReaction = DateTime.MinValue;
            private int _reactionChance = 90;

            public ReactionExecuter(ReactionPatternConfig config, Func<Message,Task<bool>> reactionFunc, ILogger<ReactionExecuter> logger)
            {
                _config = config ?? throw new ArgumentNullException(nameof(config));
                _reactionFunc = reactionFunc ?? throw new ArgumentNullException(nameof(reactionFunc));
                _logger = logger;
                _rand = new Random(DateTime.Now.Millisecond);
            }

            public async Task<bool> CheckAndDo(Message message)
            {
                if (!CheckPossibleReaction()) 
                    return false;

                bool result = await _reactionFunc(message);

                if (result)
                {
                    _lastReaction = DateTime.Now;
                    _logger.LogInformation($"Reaction succeed!");
                }


                return result;

            }
    
            private bool CheckPossibleReaction()
            {
                _reactionChance = (int)Math.Min(
                    _config.MaxReactionChance,
                    _config.MinReactionChance +
                    (DateTime.Now - _lastReaction) / _config.MaxChanceTimeout * (_config.MaxReactionChance - _config.MinReactionChance)
                );

                int rand = _rand.Next(0, 100);
                bool result = rand <= _reactionChance;

                _logger.LogInformation($"Reaction chance {_reactionChance}% [dice {rand}/100 -> {result}]");

                return result;
            }

        }
    }
}
