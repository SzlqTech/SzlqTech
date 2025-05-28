using System.ComponentModel;

namespace SzlqTech.Common.Exceptions
{
    public static class EnumExtension
    {
        // 根据名称获取枚举值，不存在则返回默认值
        public static T GetValueByName<T>(this T enumType, string name) where T : struct, Enum
        {
            if (Enum.TryParse(name, out T result))
                return result;

            // 也可以抛出异常
            throw new ArgumentException($"未找到名称为 {name} 的枚举值", nameof(name));
        }

        // 重载方法：支持忽略大小写
        public static T GetValueByName<T>(this T enumType, string name, bool ignoreCase) where T : struct, Enum
        {
            if (Enum.TryParse(name, ignoreCase, out T result))
                return result;

            throw new ArgumentException($"未找到名称为 {name} 的枚举值", nameof(name));
        }

        // 根据描述特性获取枚举值
        public static T GetValueFromDescription<T>(string description) where T : struct, Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                    is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else if (field.Name == description)
                {
                    return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException($"未找到描述为 {description} 的枚举值", nameof(description));
        }
    }
}
