using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Masuit.Tools;
using Masuit.Tools.Systems;
using NLog;
using Prism.Regions;

using SqlqTech.Core.Vo;

using System.Collections.ObjectModel;
using System.Windows.Threading;
using SzlqTech.Common.Nlogs;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;
using SzlqTech.Entity;
using SzlqTech.IService;
using SzlqTech.Localization;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class ProductViewModel: NavigationViewModel
    {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper mapper;
        private readonly IProductService productService;

        public ProductViewModel(IMapper mapper,IProductService productService)
        {
           Title= LocalizationService.GetString(AppLocalizations.ProductManagement);
            this.mapper = mapper;
            this.productService = productService;
        }

        [ObservableProperty]
        public ObservableCollection<ProductVo> productVos;

        [ObservableProperty]
        public ProductVo selectedProductVo;

        [RelayCommand]
        public void Add()
        {
            
            ProductVos.Add(new ProductVo() { });
        }

        [RelayCommand]
        public async Task Save()
        {
            await SetBusyAsync(async () =>
            {
                try
                {
                    if (Valid())
                    {
                        foreach (var item in ProductVos)
                        {
                            if (item.Id == 0)
                            {
                                item.Id = SnowFlake.LongId;
                            }
                        }
                        List<Product> products = mapper.Map<List<Product>>(ProductVos);
                        await productService.SaveOrUpdateBatchAsync(products);
                        SendSuccessMsg();
                    }
                    else
                    {
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            SendMessage("产品信息填写不完整或错误");
                        });

                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorHandler($"保存产品失败，失败原因: [{ex.Message}]");
                    SendErrorMsg();
                }
            });


        }

        public bool Valid()
        {
            //空吗和重码判断
            if(ProductVos.Any(o=>string.IsNullOrEmpty(o.ProductName))) return false;
            if (ProductVos.Any(o => string.IsNullOrEmpty(o.ProductCode))) return false;
            if(ProductVos.GroupBy(o => o.ProductCode).Any(g=>g.Count()>1) ) return false;
            
            return true;
        }

        [RelayCommand]
        public void Delete(ProductVo vo)
        {
            if(vo==null)
            {
                SendMessage("请选择要删除的产品");
                return;
            }
            if (vo.Id == 0)
            {
                ProductVos.Remove(vo);
            }
            else
            {
                if(productService.Exist(o=>o.Id==vo.Id))
                {
                    productService.RemoveById(vo.Id);
                }
                ProductVos.Remove(vo);
            }
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            ProductVos = new ObservableCollection<ProductVo>();
            List<Product> products =await productService.ListAsync();
            if(products != null&&products.Count>0)
            {
                List<ProductVo> vos=mapper.Map<List<ProductVo>>(products);
                ProductVos.AddRange(vos);
            }
        }
    }
}
