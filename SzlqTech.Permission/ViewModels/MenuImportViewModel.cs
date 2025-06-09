using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Prism.Ioc;
using System.Collections.ObjectModel;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.Vos;
using SzlqTech.Entity;
using SzlqTech.IService;
using SzlqTech.Localization;

namespace SzlqTech.Permission.ViewModels
{
    public partial class MenuImportViewModel:NavigationViewModel
    {
        private readonly IContainerProvider provider;
        private readonly ISysMenuService sysMenuService;
        private readonly IMapper mapper;

        public MenuImportViewModel(IContainerProvider provider, ISysMenuService sysMenuService, IMapper mapper) 
        {
            Title = LocalizationService.GetString(AppLocalizations.MenuImport);
            this.sysMenuService = sysMenuService;
            this.mapper = mapper;
            ImportMenus = new ObservableCollection<SysMenuVo>();
           
        }

        #region func



        /// <summary>
        ///  通过dll文件导入菜单
        /// </summary>
        [RelayCommand]
        private void Import()
        {
            OpenFileDialog dlg = new OpenFileDialog();
           // dlg.Filter = MessageTips.ComponentFilter;
            dlg.InitialDirectory = Environment.CurrentDirectory;
            if (dlg.ShowDialog() == true)
            {

                List<SysMenu> list = sysMenuService.ReadFromFile(dlg.FileName);
                ImportMenus = new ObservableCollection<SysMenuVo>();
                List<SysMenuVo> menus = mapper.Map<List<SysMenuVo>>(list);
                ImportMenus.AddRange(menus);
                //sysMenuService.SaveBatchNotExist(list);
               
                foreach (var model in ImportMenus)
                {
                    model.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == nameof(SysMenuVo.IsSelected))
                            OnPropertyChanged();
                           
                    };
                }

            }
        }

        [RelayCommand]
        private void Save()
        {
            List<SysMenuVo> list = ImportMenus.ToList().FindAll(s => s.IsSelected == true);
            list.ForEach(s => s.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            List<SysMenu> menus = mapper.Map<List<SysMenu>>(list);
            sysMenuService.SaveBatchNotExist(menus);
            SendMessage("菜单导入成功");

        }


        #endregion



        #region prop
        [ObservableProperty]
        private ObservableCollection<SysMenuVo> importMenus;

     
        private bool? isAllItems1Selected;

        public bool? IsAllItems1Selected
        {
            get
            {
                var selected = ImportMenus.Select(item => item.IsSelected).Distinct().ToList();
                return selected.Count == 1 ? selected.Single() : (bool?)null;
            }
            set
            {
                if (value.HasValue)
                {
                    SelectAll(value.Value, ImportMenus);
                    OnPropertyChanged();
                    
                }
            }
        }

        private static void SelectAll(bool select, IEnumerable<SysMenuVo> models)
        {
            foreach (var model in models)
            {
                model.IsSelected = select;
            }
        }

        #endregion
    }
}
