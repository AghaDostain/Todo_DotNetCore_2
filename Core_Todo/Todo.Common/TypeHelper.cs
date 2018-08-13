using System;
using System.Linq.Expressions;
using System.Reflection;
using Todo.Common.Enumerations;

namespace Todo.Common
{
    public static class TypeHelper
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
        /// <summary>
        /// Return correct property name by doing a case insensitive match
        /// return null if property is not part of the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string MapToValidPropertyName<T>(string propertyName)
        {
            var propertyInfo = MapToValidProperty<T>(propertyName);
            return propertyInfo != null ? propertyInfo.Name : null;
        }
        /// <summary>
        /// Return matching property info
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo MapToValidProperty<T>(string propertyName)
        {
            var propertInfos = typeof(T).GetProperties();
            var propertyInfo = Array.Find(propertInfos, pf => pf.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            return propertyInfo;
        }
        public static Expression MakePredicate(ParameterExpression expression, string fieldName, FilterOperators operation, object value)
        {
            MemberExpression member = Expression.Property(expression, fieldName);
            ConstantExpression constant = Expression.Constant(value);
            switch (operation)
            {
                case FilterOperators.Equals:
                    return Expression.Equal(member, constant);
                case FilterOperators.GreaterThan:
                    return Expression.GreaterThan(member, constant);
                case FilterOperators.LessThan:
                    return Expression.LessThan(member, constant);
                case FilterOperators.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);
                case FilterOperators.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);
                case FilterOperators.Contains:
                    return Expression.Call(member, containsMethod, constant);
                case FilterOperators.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);
                case FilterOperators.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
            }
            return null;
        }
    }
}
