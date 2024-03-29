using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace Byte.Core.Common.Helpers
{
    public static class MapperHelper<TSource, TTarget> where TSource : class where TTarget : class
    {
        private static Func<TSource, TTarget> MapFunc
        {
            get;
            set;
        }

        private static Action<TSource, TTarget> MapAction
        {
            get;
            set;
        }

        //
        // 摘要:
        //     将对象TSource转换为TTarget
        //
        // 参数:
        //   source:
        public static TTarget Map(TSource source)
        {
            if (MapFunc == null)
            {
                MapFunc = GetMapFunc();
            }

            return MapFunc(source);
        }

        public static List<TTarget> MapList(IEnumerable<TSource> sources)
        {
            if (MapFunc == null)
            {
                MapFunc = GetMapFunc();
            }

            List<TTarget> list = new List<TTarget>();
            foreach (TSource source in sources)
            {
                list.Add(MapFunc(source));
            }

            return list;
        }

        //
        // 摘要:
        //     将对象TSource的值赋给给TTarget
        //
        // 参数:
        //   source:
        //
        //   target:
        public static void Map(TSource source, TTarget target)
        {
            if (MapAction == null)
            {
                MapAction = GetMapAction();
            }

            MapAction(source, target);
        }

        private static Func<TSource, TTarget> GetMapFunc()
        {
            Type typeFromHandle = typeof(TSource);
            Type typeFromHandle2 = typeof(TTarget);
            ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "p");
            List<MemberBinding> list = new List<MemberBinding>();
            foreach (PropertyInfo item in from x in typeFromHandle2.GetProperties()
                                          where x.PropertyType.IsPublic && x.CanWrite
                                          select x)
            {
                PropertyInfo property = typeFromHandle.GetProperty(item.Name);
                if (!(property == null) && property.CanRead && !property.PropertyType.IsNotPublic && property.GetCustomAttribute<NotMappedAttribute>() == null)
                {
                    MemberExpression memberExpression = Expression.Property(parameterExpression, property);
                    if (!property.PropertyType.IsValueType && property.PropertyType != item.PropertyType)
                    {
                        if (property.PropertyType.IsClass && item.PropertyType.IsClass && !property.PropertyType.IsGenericType && !item.PropertyType.IsGenericType)
                        {
                            Expression classExpression = GetClassExpression(memberExpression, property.PropertyType, item.PropertyType);
                            list.Add(Expression.Bind(item, classExpression));
                        }

                        if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && typeof(IEnumerable).IsAssignableFrom(item.PropertyType))
                        {
                            Expression listExpression = GetListExpression(memberExpression, property.PropertyType, item.PropertyType);
                            list.Add(Expression.Bind(item, listExpression));
                        }
                    }
                    else if (!(item.PropertyType != property.PropertyType))
                    {
                        list.Add(Expression.Bind(item, memberExpression));
                    }
                }
            }

            BinaryExpression test = Expression.NotEqual(parameterExpression, Expression.Constant(null, typeFromHandle));
            MemberInitExpression ifTrue = Expression.MemberInit(Expression.New(typeFromHandle2), list);
            return Expression.Lambda<Func<TSource, TTarget>>(Expression.Condition(test, ifTrue, Expression.Constant(null, typeFromHandle2)), new ParameterExpression[1]
            {
                parameterExpression
            }).Compile();
        }

        //
        // 摘要:
        //     类型是class时赋值
        //
        // 参数:
        //   sourceProperty:
        //
        //   sourceType:
        //
        //   targetType:
        private static Expression GetClassExpression(Expression sourceProperty, Type sourceType, Type targetType)
        {
            BinaryExpression test = Expression.NotEqual(sourceProperty, Expression.Constant(null, sourceType));
            MethodCallExpression ifTrue = Expression.Call(typeof(MapperHelper<,>).MakeGenericType(sourceType, targetType).GetMethod("Map", new Type[1]
            {
                sourceType
            }), sourceProperty);
            return Expression.Condition(test, ifTrue, Expression.Constant(null, targetType));
        }

        //
        // 摘要:
        //     类型为集合时赋值
        //
        // 参数:
        //   sourceProperty:
        //
        //   sourceType:
        //
        //   targetType:
        private static Expression GetListExpression(Expression sourceProperty, Type sourceType, Type targetType)
        {
            BinaryExpression test = Expression.NotEqual(sourceProperty, Expression.Constant(null, sourceType));
            Type type = sourceType.IsArray ? sourceType.GetElementType() : sourceType.GetGenericArguments()[0];
            Type type2 = targetType.IsArray ? targetType.GetElementType() : targetType.GetGenericArguments()[0];
            MethodCallExpression methodCallExpression = Expression.Call(typeof(MapperHelper<,>).MakeGenericType(type, type2).GetMethod("MapList", new Type[1]
            {
                sourceType
            }), sourceProperty);
            Expression ifTrue = (targetType == methodCallExpression.Type) ? methodCallExpression : (targetType.IsArray ? Expression.Call(methodCallExpression, methodCallExpression.Type.GetMethod("ToArray")) : ((!typeof(IDictionary).IsAssignableFrom(targetType)) ? ((Expression)Expression.Convert(methodCallExpression, targetType)) : ((Expression)Expression.Constant(null, targetType))));
            return Expression.Condition(test, ifTrue, Expression.Constant(null, targetType));
        }

        private static Action<TSource, TTarget> GetMapAction()
        {
            Type typeFromHandle = typeof(TSource);
            Type typeFromHandle2 = typeof(TTarget);
            ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "p");
            ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2, "t");
            List<Expression> list = new List<Expression>();
            foreach (PropertyInfo item in from x in typeFromHandle2.GetProperties()
                                          where x.PropertyType.IsPublic && x.CanWrite
                                          select x)
            {
                PropertyInfo property = typeFromHandle.GetProperty(item.Name);
                if (!(property == null) && property.CanRead && !property.PropertyType.IsNotPublic && property.GetCustomAttribute<NotMappedAttribute>() == null)
                {
                    MemberExpression memberExpression = Expression.Property(parameterExpression, property);
                    MemberExpression left = Expression.Property(parameterExpression2, item);
                    if (!property.PropertyType.IsValueType && property.PropertyType != item.PropertyType)
                    {
                        if (property.PropertyType.IsClass && item.PropertyType.IsClass && !property.PropertyType.IsGenericType && !item.PropertyType.IsGenericType)
                        {
                            Expression classExpression = GetClassExpression(memberExpression, property.PropertyType, item.PropertyType);
                            list.Add(Expression.Assign(left, classExpression));
                        }

                        if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && typeof(IEnumerable).IsAssignableFrom(item.PropertyType))
                        {
                            Expression listExpression = GetListExpression(memberExpression, property.PropertyType, item.PropertyType);
                            list.Add(Expression.Assign(left, listExpression));
                        }
                    }
                    else if (!(item.PropertyType != property.PropertyType))
                    {
                        list.Add(Expression.Assign(left, memberExpression));
                    }
                }
            }

            BinaryExpression test = Expression.NotEqual(parameterExpression, Expression.Constant(null, typeFromHandle));
            BlockExpression ifTrue = Expression.Block(list);
            ConditionalExpression ifTrue2 = Expression.IfThen(test, ifTrue);
            return Expression.Lambda<Action<TSource, TTarget>>(Expression.IfThen(Expression.NotEqual(parameterExpression2, Expression.Constant(null, typeFromHandle2)), ifTrue2), new ParameterExpression[2]
            {
                parameterExpression,
                parameterExpression2
            }).Compile();
        }
    }
}
