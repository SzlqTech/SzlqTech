
namespace SzlqTech.Common.Exceptions
{
    public sealed class ExceptionUtils
    {
        public static SqlException SqlException(string message, Exception exception, params object[] objs)
        {
            return new SqlException(string.Format(message, objs), exception);
        }

        public static SqlException SqlException(string message, params object[] objs)
        {
            return new SqlException(string.Format(message, objs));
        }

        public static SqlException SqlException(Exception exception)
        {
            return new SqlException(exception);
        }

        public static void ThrowSqlException(bool condition, string message, params object[] objs)
        {
            if (condition)
            {
                throw SqlException(message, objs);
            }
        }

        public static BusinessException BusinessException(string message, Exception exception, params object[] objs)
        {
            return new BusinessException(string.Format(message, objs), exception);
        }

        public static BusinessException BusinessException(string message, params object[] objs)
        {
            return new BusinessException(string.Format(message, objs));
        }

        public static BusinessException BusinessException(Exception exception)
        {
            return new BusinessException(exception);
        }

        public static void ThrowBusinessException(bool condition, string message, params object[] objs)
        {
            if (condition)
            {
                throw BusinessException(message, objs);
            }
        }

        public static EquipmentException EquipmentException(string message, Exception exception, params object[] objs)
        {
            return new EquipmentException(string.Format(message, objs), exception);
        }

        public static EquipmentException EquipmentException(string message, params object[] objs)
        {
            return new EquipmentException(string.Format(message, objs));
        }

        public static EquipmentException EquipmentException(Exception exception)
        {
            return new EquipmentException(exception);
        }

        public static void ThrowEquipmentException(bool condition, string message, params object[] objs)
        {
            if (condition)
            {
                throw EquipmentException(message, objs);
            }
        }

        //public static WorkflowException WorkflowException(string message, Exception exception, params object[] objs)
        //{
        //    return new WorkflowException(string.Format(message, objs), exception);
        //}

        //public static WorkflowException WorkflowException(string message, params object[] objs)
        //{
        //    return new WorkflowException(string.Format(message, objs));
        //}

        //public static WorkflowException WorkflowException(Exception exception)
        //{
        //    return new WorkflowException(exception);
        //}

        //public static void ThrowWorkflowException(bool condition, string message, params object[] objs)
        //{
        //    if (condition)
        //    {
        //        throw WorkflowException(message, objs);
        //    }
        //}

        public static ArgumentException ArgumentException(string message, Exception exception, params object[] objs)
        {
            return new ArgumentException(string.Format(message, objs), exception);
        }

        public static ArgumentException ArgumentException(string message, params object[] objs)
        {
            return new ArgumentException(string.Format(message, objs));
        }

        public static void ThrowArgumentException(bool condition, string message, params object[] objs)
        {
            if (condition)
            {
                throw ArgumentException(message, objs);
            }
        }
    }
}
