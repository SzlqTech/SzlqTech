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
        public static CultureInfo CurrentCulture
        {
            get => _currentCulture;
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
