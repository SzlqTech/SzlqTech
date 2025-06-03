using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;
using SzlqTech.Localization;

namespace SzlqTech.Core.WorkFlow.ViewModels
{
    public class InnoLightTraceViewModel:NavigationViewModel
    {
        public InnoLightTraceViewModel()
        {
            Title = LocalizationService.GetString(AppLocalizations.InnoLight);
        }
    }
}
