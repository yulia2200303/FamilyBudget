using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.Storage;

namespace UI.Logic
{
    public static class LocalizationHelper
    {
        public static string DefautLocale = "en";

        public static void SetLocale(string locale)
        {
            ApplicationLanguages.PrimaryLanguageOverride = locale;
            ApplicationData.Current.LocalSettings.Values["locale"] = locale;
        }

        public static string GetCurrentLocale()
        {
            var locale = ApplicationData.Current.LocalSettings.Values["locale"];
            if (string.IsNullOrEmpty(locale?.ToString())) return DefautLocale;
            return (string)locale;
        }

        public static string GetString(string key)
        {
            return ResourceLoader.GetForCurrentView().GetString(key);
        }

        public static Dictionary<string, string> GetSupportedLocales()
        {
            return new Dictionary<string, string>()
            {
                {
                    "en", "English"
                },
                {
                    "ru", "Русский"
                },
                {
                    "be", "Беларускi"
                },

                {
                    "de", "German"
                }
            };
        }

    }
}
