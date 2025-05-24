using System.Runtime.Serialization;

namespace SzlqTech.Common.Exceptions
{
    public class SqlException : BaseException
    {
        public SqlException()
        {
        }

        public SqlException(string message)
            : base(message)
        {
        }

        public SqlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SqlException(string message, params object[] args)
            : base(message, args)
        {
        }

        public SqlException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        protected SqlException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
