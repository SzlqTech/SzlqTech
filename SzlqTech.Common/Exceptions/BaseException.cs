using System.Runtime.Serialization;

namespace SzlqTech.Common.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string message)
            : base(message)
        {
        }

        public BaseException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BaseException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public BaseException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public static Exception GetException(Exception exception)
        {
            GetInnerException(exception, out Exception outException);
            return outException;
        }

        private static void GetInnerException(Exception exception, out Exception outException)
        {
            if (exception.InnerException == null)
            {
                outException = exception;
            }
            else
            {
                GetInnerException(exception.InnerException, out outException);
            }
        }
    }
}
