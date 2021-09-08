using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Marlin.Core
{
    public class Settings
    {
        static Settings()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            Current = config.GetSection("Marlin").Get<Settings>();
        }

        public string StorageImplementationType { get; set; }
        public string AuthorizationHandlerImplementationType { get; set; }
        public string MessageHandlerImplementationType { get; set; }
        public string StorageSource { get; set; }
        public List<SettingValue> SettingValues { get; set; }
        public static Settings Current { get; set; }
        public static T Get<T>(string key, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Current.SettingValues == null)
            {
                if (!EqualityComparer<T>.Default.Equals(defaultValue, default))
                {
                    return defaultValue;
                }

                return default;
            }

            SettingValue settingValue = Current.SettingValues.Where(x => x.Key == key).FirstOrDefault();

            if(settingValue == null)
            {
                if(EqualityComparer<T>.Default.Equals(defaultValue, default))
                {
                    throw new ApplicationException($"Key {key} not found.");
                }

                return defaultValue;
            }

            T value = (T)settingValue.Value;

            return value;
        }

        public static List<T> GetList<T>(string key, bool required = true)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            List<SettingValue> settingValues = Current.SettingValues.Where(x => x.Key == key).ToList();

            if (settingValues == null || settingValues.Count == 0)
            {
                if (required)
                {
                    throw new ApplicationException($"Settings {key} not found.");
                }

                return null;
            }

            return settingValues.Select(x => (T)x.Value).Distinct().ToList();
        }
    }

    public class SettingValue
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
