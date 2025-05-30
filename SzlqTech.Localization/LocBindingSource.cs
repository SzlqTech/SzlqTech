
using System.ComponentModel;

namespace SzlqTech.Localization
{
    public class LocBindingSource : INotifyPropertyChanged
    {
        public string Value => LocalizationService.GetString(Key, Args);
        public string Key { get; }
        public object[] Args { get; }

        public LocBindingSource(string key, object[] args)
        {
            Key = key;
            Args = args;
            LocalizationService.LanguageChanged += (s, e) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
