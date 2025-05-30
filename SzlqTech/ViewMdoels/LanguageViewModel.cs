using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SzlqTech.Localization;

namespace SzlqTech.ViewMdoels
{
    public class LanguageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CultureInfo> SupportedLanguages { get; } = new()
        {
            new CultureInfo("en-US"),
            new CultureInfo("zh-CN"),
            new CultureInfo("fr-FR")
        };

        public CultureInfo SelectedLanguage
        {
            get => LocalizationService.CurrentCulture;
            set
            {
                LocalizationService.CurrentCulture = value;
                OnPropertyChanged();

                // 保存设置（可选）
                //Properties.Settings.Default.Language = value.Name;
                //Properties.Settings.Default.Save();
            }
        }

        public LanguageViewModel()
        {
            // 初始化加载保存的语言
            //var savedLang = Properties.Settings.Default.Language;
            //if (!string.IsNullOrEmpty(savedLang))
            //    SelectedLanguage = SupportedLanguages.FirstOrDefault(l => l.Name == savedLang)
            //                     ?? CultureInfo.CurrentCulture;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
