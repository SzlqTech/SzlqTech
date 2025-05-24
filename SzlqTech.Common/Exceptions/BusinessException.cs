using System.Runtime.Serialization;

namespace SzlqTech.Common.Exceptions
{
    public class BusinessException : BaseException
    {
        public BusinessException()
        {
        }

        public BusinessException(string message)
            : base(message)
        {
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BusinessException(string message, params object[] args)
            : base(message, args)
        {
        }

        public BusinessException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        protected BusinessException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
