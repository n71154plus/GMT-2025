using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GMT_2025.Extension
    {
    public static class PropertyAccessorCache<T>
        {
        // 快取屬性委派，避免重複產生
        private static readonly ConcurrentDictionary<string, Func<T, string>> _cache = new();

        public static Func<T, string> GetStringPropertyGetter(string propertyName)
            {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            return _cache.GetOrAdd(propertyName, CreateGetter);
            }

        private static Func<T, string> CreateGetter(string propertyName)
            {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
                throw new ArgumentException($"Property '{propertyName}' not found on type {typeof(T).Name}");

            if (prop.PropertyType != typeof(string))
                {
                // 自動轉成 string (呼叫 ToString)
                var propAccess = Expression.Property(param, prop);
                var toStringMethod = typeof(object).GetMethod("ToString")!;
                var toStringCall = Expression.Call(propAccess, toStringMethod);
                var lambda = Expression.Lambda<Func<T, string>>(toStringCall, param);
                return lambda.Compile();
                }
            else
                {
                // 屬性本身就是 string
                var propAccess = Expression.Property(param, prop);
                var lambda = Expression.Lambda<Func<T, string>>(propAccess, param);
                return lambda.Compile();
                }
            }
        }
    }
