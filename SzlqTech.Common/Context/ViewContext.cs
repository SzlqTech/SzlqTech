using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Views;

namespace SzlqTech.Common.Context
{
    public class ViewContext
    {
        /// <summary>
        /// 根节点
        /// </summary>
        static readonly List<ViewStrip> ViewCache = new List<ViewStrip>();

        /// <summary>
        /// 节点与入口
        /// </summary>
        static readonly List<ViewStrip> StripCache = new List<ViewStrip>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewStrips"></param>
        /// <param name="funcStrips"></param>
        public static void SetContextStrip(List<ViewStrip> viewStrips, List<FuncStrip> funcStrips)
        {
            viewStrips.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
            funcStrips.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
            List<ViewStrip> roots = viewStrips.FindAll(obj => obj.Id == obj.RootId);

            foreach (var view in roots)
            {
                if (view.EntryType == EntryType.Catalog)
                {
                    ReserveStrip(view, viewStrips, funcStrips);
                }
                else
                {
                    List<FuncStrip> subFunc = funcStrips.FindAll(obj => obj.ViewId == view.Id);
                    subFunc.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
                    view.FuncStripList.AddRange(subFunc);
                }
            }
            ViewCache.Clear();
            ViewCache.AddRange(roots);
            StripCache.Clear();
            StripCache.AddRange(viewStrips);
        }

        // /// <summary>
        // /// 
        // /// </summary>
        // /// <param name="viewStrips"></param>
        // /// <param name="funcStrips"></param>
        // /// <param name="operateStrips"></param>
        // public static void SetContextStrip(List<ViewStrip> viewStrips, List<FuncStrip> funcStrips, List<OperateStrip> operateStrips)
        // {
        //     viewStrips.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
        //     funcStrips.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
        //     operateStrips.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
        //     List<ViewStrip> roots = viewStrips.FindAll(obj => obj.Id == obj.RootId);
        //
        //     foreach (var view in roots)
        //     {
        //         if (view.EntryType == EntryType.Catalog)
        //         {
        //             ReserveStrip(view, viewStrips, funcStrips);
        //         }
        //         else
        //         {
        //             List<FuncStrip> subFunc = funcStrips.FindAll(obj => obj.ViewId == view.Id);
        //             subFunc.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
        //             view.FuncStripList.AddRange(subFunc);
        //
        //             List<OperateStrip> subOperateStrips = operateStrips.FindAll(obj => obj.ViewId == view.Id);
        //             subOperateStrips.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
        //             view.OperateStripList.AddRange(subOperateStrips);
        //         }
        //     }
        //     ViewCache.Clear();
        //     ViewCache.AddRange(roots);
        //     StripCache.Clear();
        //     StripCache.AddRange(viewStrips);
        // }

        /// <summary>
        /// 根据parent遍历所有菜单和按钮
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewStrips"></param>
        /// <param name="funcStrips"></param>
        static void ReserveStrip(ViewStrip parent, List<ViewStrip> viewStrips, List<FuncStrip> funcStrips)
        {
            List<ViewStrip> subViews = viewStrips.FindAll(obj => obj.ParentId == parent.Id && obj.Id != obj.ParentId);
            foreach (var sv in subViews)
            {
                if (sv.EntryType == EntryType.Catalog)
                {
                    ReserveStrip(sv, viewStrips, funcStrips);
                    //parent.ViewStripList.Add(sv);
                }
                else
                {
                    List<FuncStrip> subFunc = funcStrips.FindAll(obj => obj.ViewId == sv.Id);
                    sv.FuncStripList.AddRange(subFunc);
                }
                parent.ViewStripList.Add(sv);

            }
        }

        public static List<ViewStrip> GetViewStripList()
        {
            return ViewCache;
        }

        public static List<ViewStrip> GetAllViewStripList()
        {
            List<ViewStrip> viewStrips = new List<ViewStrip>();
            viewStrips.AddRange(ViewCache);
            viewStrips.AddRange(StripCache);
            return viewStrips;
        }

        public static ViewStrip GetViewStrip(string viewId)
        {
            return StripCache.Find(strip => strip.Id == viewId);
        }

        public static ViewStrip? GetViewStrip(Type viewType)
        {
            return StripCache.Find(strip => strip.TypeName == viewType.AssemblyQualifiedName);
        }
    }
}
