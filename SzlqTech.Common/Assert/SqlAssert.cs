using Masuit.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzlqTech.Common.Assert
{
    public sealed class SqlAssert
    {
        public static void IsTrue(bool expression, string message, params object[] args)
        {
            if (!expression)
            {
                throw ExceptionUtils.SqlException(message, args);
            }
        }

        public static void IsFalse(bool expression, string message, params object[] args)
        {
            IsTrue(!expression, message, args);
        }

        public static void IsNull(object? obj, string message, params object[] args)
        {
            IsTrue(obj == null, message, args);
        }

        public static void NotNull(object? obj, string message, params object[] args)
        {
            IsTrue(obj != null, message, args);
        }

        public static void NotEmpty(string? val, string message, params object[] args)
        {
            IsTrue(!string.IsNullOrEmpty(val), message, args);
        }

        public static void NotEmpty(IEnumerable<dynamic>? list, string message, params object[] args)
        {
            IsTrue(!list.IsNullOrEmpty(), message, args);
        }

        public static void NotEmpty(dynamic[]? array, string message, params object[] args)
        {
            IsTrue(!array.IsNullOrEmpty(), message, args);
        }

        public static void NotEmpty(IDictionary<string, object>? dictionary, string message, params object[] args)
        {
            IsTrue(!dictionary.IsNullOrEmpty(), message, args);
        }

        public static void IsEmpty(IDictionary<string, object>? dictionary, string message, params object[] args)
        {
            IsTrue(dictionary.IsNullOrEmpty(), message, args);
        }

        public static void IsEqual(object? obj1, object? obj2, string message, params object[] args)
        {
            if (obj1 == null)
            {
                IsNull(obj2, message, args);
            }
            else
            {
                IsTrue(obj1.Equals(obj2), message, args);
            }
        }

        public static void NotEqual(object? obj1, object? obj2, string message, params object[] args)
        {
            if (obj1 == null)
            {
                NotNull(obj2, message, args);
            }
            else
            {
                IsTrue(!obj1.Equals(obj2), message, args);
            }
        }
    }
}
