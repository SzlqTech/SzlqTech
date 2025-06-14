
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;
using SzlqTech.Localization;

namespace SzlqTech.Core.WorkFlow.ViewModels
{
    public class InnoLightChartViewModel:NavigationViewModel
    {
        public InnoLightChartViewModel()
        {
            Title = LocalizationService.GetString(AppLocalizations.ChartView);
        }
    }
}
