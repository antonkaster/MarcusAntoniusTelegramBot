using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcusAntoniusTelegramBot.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleConfigurationAttribute : Attribute 
    {
        public Type ConfigType { get; }
        public string ConfigPathName { get; }

        public ModuleConfigurationAttribute(Type configType, string configPathName)
        {
            ConfigType = configType ?? throw new ArgumentNullException(nameof(configType));

            if (string.IsNullOrWhiteSpace(configPathName))
                throw new ArgumentException(nameof(configPathName) + " can't be null!");

            ConfigPathName = configPathName;
        }
    }
}
