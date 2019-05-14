using System.Linq;

namespace System
{
    public static class ArrayExtensions
    {
        public static string [] ToStringArray(this int[] array) 
        {
            if (array == null || !array.Any()) return new string[0];

            return array.Select(c => c.ToString()).ToArray();
        }


        public static int[] ToIntArray(this string[] array)
        {
            if (array == null || !array.Any()) return new int[0];

            return array.Select(c => int.Parse(c)).ToArray();
        }

    }
}
