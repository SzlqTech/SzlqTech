
using System.Diagnostics;
using System.Reflection;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Helper;

namespace SzlqTech.Common.Views
{
    [Serializable]
    public class FuncStrip : BaseModel
    {
        public FuncStrip()
        {
        }

        #region 字段属性


        private string? _id;

        //        private string? _roleCode;

        private string? _viewId;

        private string? _viewText;

        private EntryType _entryType;

        private string? _parentUrl;

        private string? _command;

        private int _ordinal;

        private string? _text;

        private string? _textEN;

        private string? _textZH;

        private byte[]? _image;
        #endregion

        #region 映射属性
        /// <summary>
        /// Id
        /// </summary>
        public string? Id
        {
            get => _id;
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// 模块Id
        /// </summary>
        public string? ViewId
        {
            get => _viewId;
            set
            {
                if (_viewId != value)
                {
                    _viewId = value;
                    OnPropertyChanged(nameof(ViewId));
                }
            }
        }

        public string? ViewText
        {
            get => _viewText;
            set
            {
                if (value == _viewText) return;
                _viewText = value;
                OnPropertyChanged(nameof(ViewText));
            }
        }

        public EntryType EntryType
        {
            get => _entryType;
            set
            {
                if (!_entryType.Equals(value)) return;
                _entryType = value;
                OnPropertyChanged(nameof(EntryType));
            }
        }

        /// <summary>
        /// 默认模块
        /// </summary>
        public string? ParentUrl
        {
            get => _parentUrl;
            set
            {
                if (_parentUrl != value)
                {
                    _parentUrl = value;
                    OnPropertyChanged(nameof(ParentUrl));
                }
            }
        }

        ///// <summary>
        ///// 编码
        ///// </summary>
        ////public string? Code
        //{
        //    get
        //    {
        //        return code;
        //    }
        //    set
        //    {
        //        if (code != value)
        //        {
        //            code = value;
        //            OnPropertyChanged(nameof(Code));
        //        }
        //    }
        //}

        ///// <summary>
        ///// 服务
        ///// </summary>
        //public string? ServiceType
        //{
        //    get
        //    {
        //        return serviceType;
        //    }
        //    set
        //    {
        //        if (serviceType != value)
        //        {
        //            serviceType = value;
        //            OnPropertyChanged(nameof(ServiceType));
        //        }
        //    }
        //}

        /// <summary>
        /// 命令
        /// </summary>
        public string? Command
        {
            get => _command;
            set
            {
                if (_command != value)
                {
                    _command = value;
                    OnPropertyChanged(nameof(Command));
                }
            }
        }

        /// <summary>
        /// 位置
        /// </summary>
        public int Ordinal
        {
            get => _ordinal;
            set
            {
                if (_ordinal != value)
                {
                    _ordinal = value;
                    OnPropertyChanged(nameof(Ordinal));
                }
            }
        }


        /// <summary>
        /// 文本
        /// </summary>
        public string? Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string? TextEN
        {
            get => _textEN;
            set
            {
                if (_textEN != value)
                {
                    _textEN = value;
                    OnPropertyChanged(nameof(TextEN));
                }
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string? TextZH
        {
            get => _textZH;
            set
            {
                if (_textZH != value)
                {
                    _textZH = value;
                    OnPropertyChanged(nameof(TextZH));
                }
            }
        }

        /// <summary>
        /// 图标
        /// </summary>
        public byte[]? Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }

        #endregion


        string? group;

        public string? Group
        {
            get => group;
            set
            {
                if (group != value)
                {
                    group = value;
                    OnPropertyChanged(nameof(Group));
                }
            }
        }

        private bool isChecked;
        /// <summary>
        /// 选择状态
        /// </summary>
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }


        public override string ToString()
        {
            return string.Format("{0}【{1}】", _text, _command);
        }

        private FastInvokeHandler? _fastInvoke;
        public void Fire(IBaseView view)
        {
            if (_fastInvoke == null)
            {
                Debug.Assert(_command != null, nameof(_command) + " != null");
                var mi = view.GetType().GetMethod(_command, BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic);
                if (mi == null)
                    throw new ArgumentNullException(string.Format("控制器【{0}】不存在【{1}】服务", _parentUrl, _command));
                _fastInvoke = FastInvoke.GetMethodInvoker(mi);
            }
            _fastInvoke(view, view);

        }

    }
}
