using System;
using System.Collections.Generic;

namespace Otus.Delegates
{
    public static class EnumerableExtensions
    {
        public static T GetMax<T>(this IEnumerable<T> collection, Func<T, float> convertToNumber) where T : class
        {
            T maxItem = null;
            float maxValue = float.MinValue;

            foreach (var item in collection)
            {
                float current = convertToNumber(item);
                if (current > maxValue)
                {
                    maxValue = current;
                    maxItem = item;
                }
            }

            return maxItem;
        }
    }
}