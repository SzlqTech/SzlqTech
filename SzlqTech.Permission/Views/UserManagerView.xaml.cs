using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SzlqTech.Common.Views;
using SzlqTech.Core.Vos;
using SzlqTech.Entity;
using SzlqTech.IService;

namespace SzlqTech.Permission.Views
{
    /// <summary>
    /// UserManagerView.xaml 的交互逻辑
    /// </summary>
    [View("用户管理", "系统管理", "BookOpenBlankVariant", Ordinal = 0)]
    public partial class UserManagerView : UserControl
    {
        public UserManagerView(IMapper mapper,ISysRoleService sysRoleService)
        {
            InitializeComponent();
            List<SysRole> roles = sysRoleService.List();
            if (roles != null)
            {
                List<SysRoleVo> sysRoles = mapper.Map<List<SysRoleVo>>(roles);
                cbox.ItemsSource = sysRoles;
            }
        }
    }
}
