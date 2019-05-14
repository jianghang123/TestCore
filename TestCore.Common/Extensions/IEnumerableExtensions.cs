
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TestCore.Common.Extensions
{
    /// <summary>
    /// IEnumerable�ӿڷ�����չ
    /// </summary>
    [DebuggerStepThrough]
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> fun)
        {
            foreach (T item in source)
            {
                fun(item);
            }
            return source;
        }

        public static List<TResult> ToList<T, TResult>(this IEnumerable<T> source, Func<T, TResult> fun)
        {
            List<TResult> result = new List<TResult>();
            source.Each(m => result.Add(fun(m)));
            return result;
        }

        /// <summary>
        /// ������չ�����ֱ�ת�����ַ���������ָ���ķָ����νӣ�ƴ��һ���ַ�������
        /// </summary>
        /// <param name="collection"> Ҫ����ļ��� </param>
        /// <param name="separator"> �ָ��� </param>
        /// <returns> ƴ�Ӻ���ַ��� </returns>
        public static string ExpandAndToString<T>(this IEnumerable<T> collection, string separator)
        {
            List<T> source = collection as List<T> ?? collection.ToList();
            if (source.IsEmpty())
            {
                return null;
            }
            string result = source.Cast<object>().Aggregate<object, string>(null, (current, o) => current + string.Format("{0}{1}", o, separator));
            return result.Substring(0, result.Length - separator.Length);
        }

        /// <summary>
        /// �����Ƿ�Ϊ��
        /// </summary>
        /// <param name="collection"> Ҫ����ļ��� </param>
        /// <typeparam name="T"> ��̬���� </typeparam>
        /// <returns> Ϊ�շ���True����Ϊ�շ���False </returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return !collection.Any();
        }

        /// <summary>
        /// ���ݵ����������Ƿ�Ϊ���������Ƿ�ִ��ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="source"> Ҫ��ѯ��Դ </param>
        /// <param name="predicate"> ��ѯ���� </param>
        /// <param name="condition"> ���������� </param>
        /// <typeparam name="T"> ��̬���� </typeparam>
        /// <returns> ��ѯ�Ľ�� </returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// ����ָ���������ؼ����в��ظ���Ԫ��
        /// </summary>
        /// <typeparam name="T">��̬����</typeparam>
        /// <typeparam name="TKey">��̬ɸѡ��������</typeparam>
        /// <param name="source">Ҫ������Դ</param>
        /// <param name="keySelector">�ظ�����ɸѡ����</param>
        /// <returns>���ظ�Ԫ�صļ���</returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(group => group.First());
        }

        /// <summary>
        /// IEnumerable extension method ForEach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Colection</param>
        /// <param name="action">Action method</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action(item);
            }
        }
    }
}
