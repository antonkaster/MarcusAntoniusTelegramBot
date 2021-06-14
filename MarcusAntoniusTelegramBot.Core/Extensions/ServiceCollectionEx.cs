using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MarcusAntoniusTelegramBot.Core.Abstractions.Reactions;
using MarcusAntoniusTelegramBot.Core.Abstractions.Services;
using MarcusAntoniusTelegramBot.Core.Attributes;
using MarcusAntoniusTelegramBot.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace MarcusAntoniusTelegramBot.Core.Extensions
{
    public static class ServiceCollectionEx
    {
        /// <summary>
        /// Регистрирует реакцию
        /// </summary>
        /// <typeparam name="TReaction">Модуль реакции</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddReaction<TReaction>(this IServiceCollection services)
            where TReaction : class, IReaction
        {
            services.AddScoped<IReaction,TReaction>();
            services.AddConfiguration<TReaction>();

            return services;
        }

        /// <summary>
        /// Регистрирует поставщика контента
        /// </summary>
        /// <typeparam name="TContentService">Модуль поставщика контента</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddContentService<TContentService>(this IServiceCollection services)
            where TContentService : class, IContentService
        {
            services.AddScoped<IContentService, TContentService>();
            services.AddConfiguration<TContentService>();

            return services;
        }

        /// <summary>
        /// Регистрирует конфигурацию для модуля с атрибутами ModuleConfiguration
        /// </summary>
        /// <typeparam name="TService">Модуль с атрибутом ModuleConfiguration</typeparam>
        /// <param name="services"></param>
        public static void AddConfiguration<TService>(this IServiceCollection services)
            where TService : class
        {
            foreach (ModuleConfigurationAttribute attr in typeof(TService).GetCustomAttributes(typeof(ModuleConfigurationAttribute), false))
            {
                typeof(OptionsConfigurationServiceCollectionExtensions)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(m => m.Name == "Configure" && m.IsGenericMethod && m.GetParameters().Length == 2)
                    .MakeGenericMethod(attr.ConfigType)
                    .Invoke(null, new object[]
                    {
                        services,
                        AppServicesHelper.Configuration.GetSection(attr.ConfigPathName)
                    });
            }
        }
    }
}
