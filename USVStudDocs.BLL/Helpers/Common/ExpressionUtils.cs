﻿using System.Linq.Expressions;

namespace USVStudDocs.BLL.Helpers.Common
{
    public static class ExpressionUtils
    {
        public static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, string comparison, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var left = propertyName.Split('.').Aggregate((Expression) parameter, Expression.Property);
            var body = MakeComparison(left, comparison, value);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression MakeComparison(Expression left, string comparison, string value)
        {
            switch (comparison)
            {
                case "eq":
                    return MakeBinary(ExpressionType.Equal, left, value);
                case "neq":
                    return MakeBinary(ExpressionType.NotEqual, left, value);
                case "gt":
                    return MakeBinary(ExpressionType.GreaterThan, left, value);
                case "gte":
                    return MakeBinary(ExpressionType.GreaterThanOrEqual, left, value);
                case "lt":
                    return MakeBinary(ExpressionType.LessThan, left, value);
                case "lte":
                    return MakeBinary(ExpressionType.LessThanOrEqual, left, value);
                case "contains":
                    return Expression.Call(MakeString(left), "Contains", Type.EmptyTypes,
                        Expression.Constant(value, typeof(string)), 
                        Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
                case "startswith":
                    return Expression.Call(MakeString(left), "StartsWith", Type.EmptyTypes,
                        Expression.Constant(value, typeof(string)));
                case "endswith":
                    return Expression.Call(MakeString(left), "EndsWith", Type.EmptyTypes,
                        Expression.Constant(value, typeof(string)));
                default:
                    throw new NotSupportedException($"Invalid comparison operator '{comparison}'.");
            }
        }

        private static Expression MakeString(Expression source)
        {
            return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
        }

        private static Expression MakeBinary(ExpressionType type, Expression left, string value)
        {
            object typedValue = value;
            
            if (left.Type.BaseType == typeof(Enum))
            {
                typedValue = !string.IsNullOrEmpty(value) ? Enum.Parse(left.Type, value) : null;
            }
            else if (left.Type != typeof(string))
            {
                if (string.IsNullOrEmpty(value))
                {
                    typedValue = null;
                    if (Nullable.GetUnderlyingType(left.Type) == null)
                        left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
                }
                else
                {
                    var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                    typedValue = valueType == typeof(Guid) ? Guid.Parse(value) : Convert.ChangeType(value, valueType);
                }
            }

            var right = Expression.Constant(typedValue, left.Type);
            return Expression.MakeBinary(type, left, right);
        }
    }
}