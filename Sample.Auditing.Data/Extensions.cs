using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Auditing.Data
{
    public static class Extensions
    {
        public static IDictionary<TKey, TValue> NullIfEmpty<TKey, TValue>(this IDictionary<TKey, TValue> enumeration)
        {
            if (enumeration == null || !enumeration.Any())
            {
                return null;
            }
            return enumeration;
        }
    }
}
