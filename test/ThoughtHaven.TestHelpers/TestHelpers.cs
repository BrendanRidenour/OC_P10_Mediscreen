using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ThoughtHaven
{
    public static class TestHelpers
    {
        public static TAttribute GetClassAttribute<TType, TAttribute>()
            where TAttribute : Attribute =>
            typeof(TType).GetTypeInfo().GetCustomAttribute<TAttribute>()!;

        public static IEnumerable<TAttribute> GetClassAttributes<TType, TAttribute>()
            where TAttribute : Attribute =>
            typeof(TType).GetTypeInfo().GetCustomAttributes<TAttribute>();

        public static TAttribute GetMethodAttribute<TType, TAttribute>(string methodName,
            int methodIndex = 0)
            where TAttribute : Attribute =>
            typeof(TType).GetMethods().Where(m => m.Name == methodName)
                .ToArray()[methodIndex].GetCustomAttribute<TAttribute>()!;

        public static TAttribute GetParameterAttribute<TType, TAttribute>(string methodName,
            string parameterName, int methodIndex = 0)
            where TAttribute : Attribute =>
            typeof(TType).GetMethods().Where(m => m.Name == methodName)
                .ToArray()[methodIndex].GetParameters().Where(p => p.Name == parameterName)
                .Single().GetCustomAttribute<TAttribute>()!;

        public static TAttribute GetPropertyAttribute<TType, TAttribute>(string propertyName)
            where TAttribute : Attribute =>
            typeof(TType).GetProperty(propertyName)!.GetCustomAttribute<TAttribute>()!;

        public static string CreateString(int length)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
                builder.Append('0');

            return builder.ToString();
        }
    }
}