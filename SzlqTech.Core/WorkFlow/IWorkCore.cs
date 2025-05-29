
namespace SzlqTech.Core.WorkFlow
{
    public interface IWorkCore
    {
        /// <summary>
        /// 启动流程
        /// </summary>
        void StartExecute();

        /// <summary>
        /// 停止流程
        /// </summary>
        void StopExecute();
    }
}
