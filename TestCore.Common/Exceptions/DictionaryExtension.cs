using System.Collections.Generic;

namespace TestCore.Common.Extensions
{
    public static class DictionaryExtension
    {
        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            var inverted = new SortedDictionary<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> key in source)
                inverted.Add(key.Key, key.Value);

            return inverted;
        }
    }
}
