using ImTools;
using NPOI.SS.Formula.Functions;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SzlqTech.Core.Consts;
using SzlqTech.Localization;

namespace SzlqTech.Core.Themes.Controls
{
    public class DataPager : Control, INotifyPropertyChanged
    {
        public DataPager()
        {
            ButtonCollections = new ObservableCollection<ButtonCommandModel>();
        }

        private ListBox _listBoxButtonList;
        private ComboBox comboBoxPageSize;
        private int SelectedIndex;
        private Border Border;
        private static int TotalPageCount;

        /// <summary>
        /// 显示按钮数量
        /// </summary>
        public int NumericButtonCount
        {
            get { return (int)GetValue(NumericButtonCountProperty); }
            set { SetValue(NumericButtonCountProperty, value); }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        internal ObservableCollection<ButtonCommandModel> ButtonCollections
        {
            get; set;
        }

        public static readonly DependencyProperty NumericButtonCountProperty =
            DependencyProperty.Register("NumericButtonCount", typeof(int), typeof(DataPager), new PropertyMetadata(NumericButtonCountChangedCallback));

        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(DataPager), new PropertyMetadata(PageCountChangedCallback));

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(DataPager), new PropertyMetadata(PageSizeChangedCallback));

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(DataPager), new PropertyMetadata(PageIndexChangedCallback));

        public event PropertyChangedEventHandler? PropertyChanged;

        public override void OnApplyTemplate()
        {
            var listBox = GetTemplateChild("ItemsControl") as ListBox;

            if (listBox != null)
            {
                _listBoxButtonList = listBox;
                _listBoxButtonList.ItemsSource = ButtonCollections;
            }

           
            if (GetTemplateChild("COMBOX_PAGESIZE") is ComboBox comboBoxPageSize)
            {
                this.comboBoxPageSize = comboBoxPageSize;
                this.comboBoxPageSize.SelectedIndex = SelectedIndex;
                this.comboBoxPageSize.SelectionChanged += ComboBoxPageSize_SelectionChanged;
                string pageUnit = LocalizationService.GetString(AppLocalizations.PageUnit);
                this.comboBoxPageSize.ItemsSource = new List<string> { "10"+pageUnit, "20" + pageUnit, "50" + pageUnit, "100" + pageUnit };
            }

            if(GetTemplateChild("TotalPageName") is TextBlock textBlock)
            {
                textBlock.Text=LocalizationService.GetString(AppLocalizations.TotalPageName)+":";
            }


            GetTemplateButtonByName("HomePage").Click += HomePage_Click;
            GetTemplateButtonByName("PreviousPage").Click += PreviousPage_Click;
            GetTemplateButtonByName("NextPage").Click += NextPage_Click;
            GetTemplateButtonByName("EndPage").Click += EndPage;

            base.OnApplyTemplate();
        }

        void ButtonIndexClick(ButtonCommandModel arg)
        {
            PageIndex = arg.Index - 1; //设置当前页索引    
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HomePage_Click(object sender, RoutedEventArgs e)
        {
            PageIndex = 0;
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex > 0) PageIndex -= 1;
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex + 1 == PageCount) return;

            if (PageIndex < PageCount) PageIndex += 1;
        }

        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EndPage(object sender, RoutedEventArgs e)
        {
            if (PageIndex != PageCount) PageIndex = PageCount - 1;  //设置尾页 
        }

        private Button GetTemplateButtonByName(string Name)
        {
            var button = GetTemplateChild(Name) as Button;
            button.Content=LocalizationService.GetString(Name);
            return button;
        }

        private static void NumericButtonCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataPager = (DataPager)d;

            dataPager.ButtonCollections.Clear();

            if (dataPager.PageCount == 0)
            {
                dataPager.ButtonCollections.Add(new ButtonCommandModel
                {
                    Index = 1,
                    ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                });
                return;
            }



            if (int.TryParse(e.NewValue.ToString(), out int buttonCount))
            {
                for (int i = 1; i < buttonCount + 1; i++)
                {
                    dataPager.ButtonCollections.Add(new ButtonCommandModel
                    {
                        Index = i,
                        ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                    });
                }
            }
        }

        private static void PageSizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataPager = (DataPager)d;

            if (int.TryParse(e.NewValue.ToString(), out int result))
            {
                switch (result)
                {
                    case 10: dataPager.SelectedIndex = 0; break;
                    case 20: dataPager.SelectedIndex = 1; break;
                    case 50: dataPager.SelectedIndex = 2; break;
                    case 100: dataPager.SelectedIndex = 3; break;
                    default: dataPager.SelectedIndex = 1; break;
                }
            }
        }

        private static void PageCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataPager = (DataPager)d;

            dataPager.ButtonCollections.Clear();

            if (dataPager.PageCount == 0)
            {
                dataPager.ButtonCollections.Add(new ButtonCommandModel
                {
                    Index = 1,
                    ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                });
                return;
            }

            if (int.TryParse(e.NewValue.ToString(), out int pageCount))
            {
                int Count = 0;
                //if (dataPager.NumericButtonCount > pageCount)
                //    Count = pageCount;
                //else
                //    Count = dataPager.NumericButtonCount;
                Count = pageCount;
                TotalPageCount = Count;
                //起动省略号
                if (TotalPageCount >= 11)
                {
                    for (int i = 1; i <= 9; i++)
                    {
                        dataPager.ButtonCollections.Add(new ButtonCommandModel
                        {
                            Index = i,
                            Content=i.ToString(),
                            ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                        });
                    }
                    dataPager.ButtonCollections.Add(new ButtonCommandModel
                    {
                        Index = TotalPageCount-1,
                        Content="......",                  
                    });
                    dataPager.ButtonCollections.Add(new ButtonCommandModel
                    {
                        Index = TotalPageCount,
                        Content= TotalPageCount.ToString(),
                        ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                    });
                }
                else
                {
                    for (int i = 1; i <= Count; i++)
                    {
                        dataPager.ButtonCollections.Add(new ButtonCommandModel
                        {
                            Index = i,
                            Content= i.ToString(),
                            ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                        });
                    }
                }
              
            }
        }

        private static void PageIndexChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataPager = (DataPager)d;
            var newValue = (int)e.NewValue + 1;
            var oldValue = (int)e.OldValue + 1;

            if (newValue == 1)
            {
                dataPager.RefreshButtonList(newValue);
            }
            else
            {
                var item = dataPager.ButtonCollections.FirstOrDefault(t => t.Index.Equals(newValue));
                if (item == null)
                    dataPager.RefreshButtonList(newValue - dataPager.NumericButtonCount + 1);
            }     

            if(TotalPageCount >= 11)
            {
                var buttons = new ObservableCollection<ButtonCommandModel>();
                if (newValue >= 9&&newValue<TotalPageCount-6)
                {
                    buttons.Add(new ButtonCommandModel
                    {
                        Index = 1,
                        Content = "1",
                        ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                    });
                    buttons.Add(new ButtonCommandModel
                    {
                        Index = 2,
                        Content = "......",
                        ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                    });
                    for (int i = newValue-3; i <= newValue+3; i++)
                    {
                        buttons.Add(new ButtonCommandModel
                        {
                            Index = i,
                            Content =i.ToString(),
                            ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                        });
                    }
                    buttons.Add(new ButtonCommandModel
                    {
                        Index = buttons.LastOrDefault().Index+1,
                        Content = "......",
                        ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                    });
                    buttons.Add(new ButtonCommandModel
                    {
                        Index = TotalPageCount,
                        Content = TotalPageCount.ToString(),
                        ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                    });

                    dataPager.ButtonCollections.Clear();
                    dataPager.ButtonCollections.AddRange(buttons);
                }
                else if (newValue == TotalPageCount - 6)
                {
                    buttons.Add(new ButtonCommandModel
                    {
                        Index = 1,
                        Content = "1",
                        ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                    });
                    buttons.Add(new ButtonCommandModel
                    {
                        Index = 2,
                        Content = "......",
                        ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                    });
                    for (int i = TotalPageCount - 8; i <= TotalPageCount; i++)
                    {
                        buttons.Add(new ButtonCommandModel
                        {
                            Index = i,
                            Content = i.ToString(),
                            ClickCommand = new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                        });
                    }

                    dataPager.ButtonCollections.Clear();
                    dataPager.ButtonCollections.AddRange(buttons);
                }
               
            }
            var selectedItem = dataPager.ButtonCollections.FirstOrDefault(t => t.Content.Equals(newValue.ToString()));
            if (selectedItem != null)
                dataPager._listBoxButtonList.SelectedItem = selectedItem;
        }
      

        private void ComboBoxPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (comboBoxPageSize.SelectedIndex)
            {
                case 0: PageSize = 10; break;
                case 1: PageSize = 20; break;
                case 2: PageSize = 50; break;
                case 3: PageSize = 100; break;
                default: PageSize = 10; break;
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RefreshButtonList(int Index)
        {
            for (int i = 0; i < ButtonCollections.Count; i++)
            {
                ButtonCollections[i].Index = Index;
                Index++;
            }
        }

        internal class ButtonCommandModel : BindableBase
        {
            private int index;
            public int Index
            {
                get { return index; }
                set { index = value; RaisePropertyChanged(); }
            }

            private string? content;
            public string? Content
            {
                get { return content; }
                set { content = value; RaisePropertyChanged(); }
            }

            public DelegateCommand<ButtonCommandModel> ClickCommand { get; set; }
        }
    }
}
