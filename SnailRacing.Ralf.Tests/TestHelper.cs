using DSharpPlus.Entities;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SnailRacing.Ralf.Tests
{
    internal class TestHelper
    {
        public static T? CreateInstance<T>(params object[] args)
            where T: class?
        {
            var type = typeof(T);
            if (!string.IsNullOrEmpty(type.FullName))
            {
                var instance = type.Assembly.CreateInstance(
                  type.FullName, false,
                  BindingFlags.Instance | BindingFlags.NonPublic,
                  null, args, null, null);
                return instance as T;
            }
            return null;
        }

        internal static void SetProperty<T, TValue>(T obj, Expression<Func<T, TValue>> propertyLambda, TValue value)
        {
            var member = propertyLambda.Body as MemberExpression;

            if (member == null) throw new Exception("Invalid expression");

            MethodInfo setMethod = ((PropertyInfo)member.Member).GetSetMethod(true);
            setMethod?.Invoke(obj, new object[] { value });
        }
    }
}


