using System.Runtime.Serialization;

namespace SzlqTech.Common.Exceptions
{
    public class EquipmentException : BaseException
    {
        public EquipmentException()
        {
        }

        public EquipmentException(string message)
            : base(message)
        {
        }

        public EquipmentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public EquipmentException(string message, params object[] args)
            : base(message, args)
        {
        }

        public EquipmentException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        protected EquipmentException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
