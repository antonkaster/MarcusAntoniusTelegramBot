using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using MarcusAntoniusTelegramBot.Core.Configs;
using MarcusAntoniusTelegramBot.Core.Services;
using MarcusAntoniusTelegramBot.Core.Services.Reactions;
using MarcusAntoniusTelegramBot.Core.Abstractions.Reactions;
using MarcusAntoniusTelegramBot.Core.Extensions;
using MarcusAntoniusTelegramBot.Core.Helpers;
using MarcusAntoniusTelegramBot.Core.Services.ContentServices;
using MarcusAntoniusTelegramBot.Core.Services.OtherServices;
using Telegram.Bot;

namespace MarcusAntoniusTelegramBot.WebHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppServicesHelper.Configuration = Configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Marcus Antonius Telegram Bot", Version = "v1" });
            });

            services.Configure<GeneralBotConfig>(Configuration);

            // TODO: алгоритм планов
            // TODO: сохранение цитат того кто пишет
            // TODO: анализ текста с выделением главного
            // TODO: смена состояния и реакция от состояния

            services.AddReaction<DiceReaction>();
            services.AddReaction<SpellCheckReaction>();
            services.AddReaction<VerbSimpleReaction>();
            services.AddReaction<ContentReplyReaction>();
            services.AddReaction<SimpleReplyReaction>();
            services.AddReaction<SimpleAnimationReaction>();

            services.AddContentService<PirozhkoviyContentService>();
            services.AddContentService<WeatherContentService>();
            services.AddContentService<QuotesContentService>();

            services.AddScoped<ScopedChatTelegramMessageProcessor>();
            services.AddScoped<ISpellCheckService, YandexSpellCheckService>();

            services.AddSingleton(new TelegramBotClient(Configuration["TelegramBotToken"]));
            services.AddSingleton<ITelegramAuthorization, TelegramAuthService>();
            services.AddSingleton<ChatScopeResolver>();
            services.AddSingleton<ReactionFactoryService>();

            services.AddHostedService<TelegramBotService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Marcus Antonius Telegram Bot v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
