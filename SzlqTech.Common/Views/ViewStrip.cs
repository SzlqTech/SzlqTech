using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Exceptions;

namespace SzlqTech.Common.Views
{
    public class ViewStrip : BaseModel
    {


        #region 字段属性
        private string _id = null!;

        private string? _rootId;

        private string? _parentId;

        private string? _code;

        private string? _assembly;

        private string? _text;

        private string? _textEN;

        private string? _textZH;

        private string? _description;

        private int _ordinal;

        private string? _typeName;

        private string? _xml;

        private string? _url;

        private Type? _type;

        private List<ViewStrip>? _viewStripList;

        private string? _command;

        private object? _parent;

        //private Bitmap _icon;

        private List<FuncStrip>? _funcStripList;

        // private List<OperateStrip> _operateStripList;

        #endregion

        public ViewStrip() { }

        public ViewStrip(string xml)
        {
            _xml = xml;
        }

        #region 映射属性
        /// <summary>
        /// Id
        /// </summary>
        public virtual string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        /// <summary>
        /// 上级模块
        /// </summary>
        public virtual string? ParentId
        {
            get => _parentId;
            set
            {
                if (_parentId != value)
                {
                    _parentId = value;
                    OnPropertyChanged(nameof(ParentId));
                }
            }
        }

        public virtual string? RootId
        {
            get => _rootId;
            set
            {
                if (_rootId != value)
                {
                    _rootId = value;
                    OnPropertyChanged(nameof(RootId));
                }
            }
        }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string? Code
        {
            get => _code;
            set
            {
                if (_code != value)
                {
                    _code = value;
                    OnPropertyChanged(nameof(Code));
                }
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual string? Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged("Caption");
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

        public virtual string? Assembly
        {
            get => _assembly;
            set
            {
                if (_assembly != value)
                {
                    _assembly = value;
                    OnPropertyChanged(nameof(Assembly));
                }
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string? Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        /// <summary>
        /// 位置
        /// </summary>
        public virtual int Ordinal
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

        private EntryType entryType;


        /// <summary>
        /// 入口类型
        /// </summary>
        public virtual EntryType EntryType
        {
            get => entryType;
            set
            {
                if (entryType != value)
                {
                    entryType = value;
                    OnPropertyChanged(nameof(EntryType));
                }
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual string? TypeName
        {
            get => _typeName;
            set
            {
                if (_typeName != value)
                {
                    _typeName = value;
                    OnPropertyChanged(nameof(TypeName));
                }
            }
        }

        /// <summary>
        /// 配置
        /// </summary>
        public virtual string? XML
        {
            get => this._xml;
            set
            {
                if (_xml != value)
                {
                    _xml = value;
                    OnPropertyChanged(nameof(XML));
                }
            }
        }

        /// <summary>
        /// 路径
        /// </summary>
        public virtual string? Url
        {
            get => _url;
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged(nameof(Url));
                }
            }
        }

        public virtual string? Command
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

        // public Bitmap Icon
        // {
        //     get => _icon;
        //     set
        //     {
        //         if (_icon != value)
        //         {
        //             _icon = value;
        //             OnPropertyChanged(nameof(Icon));
        //         }
        //     }
        // }



        #endregion


        public Type Type
        {
            get
            {
                if (_type == null)
                {
                    _type = Type.GetType(_typeName);
                    if (_type == null)
                    {
                        throw new ArgumentException($"没有找到 [{this._text}({this._id})]");
                    }
                    if (_type.GetInterface(nameof(IBaseView), true) == null)
                    {
                        throw new ArgumentException("类型不正确");
                    }
                }
                return _type;
            }
        }


        [XmlIgnore]
        public List<ViewStrip> ViewStripList => _viewStripList ??= new List<ViewStrip>();

        [XmlIgnore]
        public List<FuncStrip> FuncStripList => _funcStripList ??= new List<FuncStrip>();

        // [XmlIgnore]
        // public List<OperateStrip> OperateStripList => _operateStripList ??= new List<OperateStrip>();


        #region ICacade
        [XmlIgnore]
        public object? Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                OnPropertyChanged(nameof(Parent));
            }
        }
        #endregion

        public override string ToString()
        {
            return $"【{_text}】({_url})";
        }

        public string EnglishString => $"[{_textEN}]({_url})";
    }
}
