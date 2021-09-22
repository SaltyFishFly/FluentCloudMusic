using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentNetease.Classes
{
    public static class Extension
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            Random random = new Random();
            List<T> result = new List<T>(list);

            int n = result.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = result[k];
                result[k] = result[n];
                result[n] = value;
            }
            return result;
        }
    }
}
