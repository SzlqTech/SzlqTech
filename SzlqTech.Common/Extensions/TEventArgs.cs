

namespace SzlqTech.Common.Extensions
{
    public class TEventArgs<T> : EventArgs
    {
        public T Data { get; set; }

        public TEventArgs(T data)
        {
            Data = data;
        }
    }
}
