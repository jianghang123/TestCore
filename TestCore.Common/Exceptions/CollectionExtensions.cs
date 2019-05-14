using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestCore.Common.Extensions
{
    /// <summary>
    /// 集合辅助类
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 判断集合是否为null或空
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this IEnumerable collection)
        {
            return collection == null || !collection.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// 按给定长度拆分集合，默认拆分长度为100
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="length">拆分集合长度，默认100</param>
        /// <returns></returns>
        public static List<List<T>> Split<T>(this ICollection<T> collection, int length = 100)
        {
            if (length <= 0) throw new ArgumentException("Length must greater than 0");

            var splitedList = new List<List<T>>();
          
            if (!collection.IsNullOrEmpty())
            {
                var pageNo = 0;
                while (true)
                {
                    var temp = collection.Skip(pageNo * length).Take(length).ToList();

                    if (!temp.IsNullOrEmpty())
                        splitedList.Add(temp);
                    else
                        break;

                    pageNo++;
                }
            }

            return splitedList;
        }

        /// <summary>
        /// 按照给定长度（默认100）拆分为多个批次，遍历执行action,action参数为该批次的元素集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">需要分批次执行的集合</param>
        /// <param name="action">执行的action</param>
        /// <param name="length">每批次集合的长度</param>
        public static void Batch<T>(this ICollection<T> collection, Action<List<T>> action, int length = 100)
        {
            if (collection.IsNullOrEmpty() || action == null) return;

            var batches = collection.Split(length);

            if (!batches.IsNullOrEmpty())
                batches.ForEach(action);
        }

        /// <summary>
        /// 遍历Array,可取到Array本身和元素位置
        /// </summary>
        /// <param name="array">Array本身</param>
        /// <param name="action">[Array本身，元素在数组中的位置]</param>
        public static void ForEach(this Array array, Action<Array, int[]> action)
        {
            if (array.Length == 0) return;
            var walker = new ArrayTraverse(array);

            do action(array, walker.Position);
            while (walker.Step());
        }

        /// <summary>
        /// 遍历Array重载，可取到当前遍历到的元素
        /// </summary>
        /// <param name="array">Array本身</param>
        /// <param name="action">[当前元素]</param>
        public static void ForEach<T>(this Array array, Action<T> action)
        {
            if (array.Length == 0) return;
            var walker = new ArrayTraverse(array);

            do action((T)array.GetValue(walker.Position));
            while (walker.Step());
        }

        /// <summary>
        /// 遍历Array重载，可取到当前遍历到的元素
        /// </summary>
        /// <param name="array">Array本身</param>
        /// <param name="action">[当前元素]</param>
        public static void ForEach(this Array array, Action<object> action)
        {
            if (array.Length == 0) return;
            var walker = new ArrayTraverse(array);

            do action(array.GetValue(walker.Position));
            while (walker.Step());
        }

        /// <summary>
        /// 判断是否包含指定字符串，忽略大小写
        /// </summary>
        /// <param name="contents">字符串列表</param>
        /// <param name="input">待匹配字符串</param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this IEnumerable<string> contents, string input)
        {
            if (contents.IsNullOrEmpty() || string.IsNullOrEmpty(input))
                return false;

            return contents.Contains(input, new StringIgnoreCaseComparer());
        }
    }

    /// <summary>
    /// 数组遍历器
    /// </summary>
    internal class ArrayTraverse
    {
        public int[] Position;
        private readonly long[] _maxLengths;

        public ArrayTraverse(Array array)
        {
            _maxLengths = new long[array.Rank];
            for (var i = 0; i < array.Rank; ++i)
            {
                _maxLengths[i] = array.GetLength(i) - 1;
            }

            Position = new int[array.Rank];
        }

        public bool Step()
        {
            for (var i = 0; i < Position.Length; ++i)
            {
                if (Position[i] >= _maxLengths[i]) continue;

                Position[i]++;
                for (var j = 0; j < i; j++)
                {
                    Position[j] = 0;
                }

                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 忽略大小写字符串比较器
    /// </summary>
    public class StringIgnoreCaseComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
