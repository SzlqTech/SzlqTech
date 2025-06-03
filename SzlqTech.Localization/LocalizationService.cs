using System.Configuration;
using System.Globalization;
using System.Resources;

namespace SzlqTech.Localization
{
    public static class LocalizationService
    {
        private static readonly ResourceManager _resourceManager =
            new ResourceManager("SzlqTech.Localization.Resources.Strings",
                typeof(LocalizationService).Assembly);

        public static event EventHandler LanguageChanged;

       

        private static CultureInfo _currentCulture = CultureInfo.CurrentCulture;

        //_currentCulture
        public static CultureInfo CurrentCulture
        {
            get
            {
                var CurrLangName = ConfigurationManager.AppSettings["lang"] ?? "zh-CN";
                if(_currentCulture.IetfLanguageTag!=CurrLangName)
                 _currentCulture =new CultureInfo(CurrLangName);
                return _currentCulture;
            }
            set
            {
                if (_currentCulture.Equals(value)) return;
                _currentCulture = value;
                LanguageChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static string GetString(string key, params object[] args)
        {
         
            var value = _resourceManager.GetString(key, CurrentCulture);
            if (args != null && args.Length > 0)
            {
                return args.Length > 0 ? string.Format(value ?? $"[{key}]", args) : value ?? $"[{key}]";
            }
            return value??string.Empty;
        }
    }
}
