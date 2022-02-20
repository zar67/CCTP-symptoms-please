using System;
using System.Collections.Generic;

namespace SymptomsPlease.Utilities.ExtensionMethods
{
    public static class ListExtensions
    {
        private static Random m_random = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = m_random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}