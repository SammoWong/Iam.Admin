using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Iam.Core.Extensions
{
    public static class QueryableExtension
    {
        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> source, int skipCount, int takeCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Skip(skipCount).Take(takeCount);
        }

        /// <summary>
        /// 排序并分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="skipCount"></param>
        /// <param name="takeCount"></param>
        /// <param name="ordering">排序规则：Id DESC</param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> source, int skipCount, int takeCount, string ordering)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (!string.IsNullOrWhiteSpace(ordering))
            {
                var orderedSource = source as IOrderedQueryable<T>;
                return orderedSource.OrderBy(ordering).Skip(skipCount).Take(takeCount);
            }
            return source.Skip(skipCount).Take(takeCount);
        }
    }
}
