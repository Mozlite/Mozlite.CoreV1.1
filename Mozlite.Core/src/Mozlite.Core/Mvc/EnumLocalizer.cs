using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Resources;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Mozlite.Mvc
{
    /// <summary>
    /// 枚举本地化字符串实现类。
    /// </summary>
    public class EnumLocalizer : IEnumLocalizer
    {
        private readonly ConcurrentDictionary<Type, ResourceManager> _localizers = new ConcurrentDictionary<Type, ResourceManager>();
        /// <summary>
        /// 获取本地化实例对象。
        /// </summary>
        /// <param name="name">枚举类型实例。</param>
        /// <returns>返回本地化实例对象。</returns>
        protected ResourceManager GetResourceManager(Enum name)
        {
            return _localizers.GetOrAdd(name.GetType(), type =>
            {
                var assembly = type.GetTypeInfo().Assembly;
                return new ResourceManager(assembly.GetName().Name + ".Resources", assembly);
            });
        }

        /// <summary>
        /// 获取枚举值对应的本地化字符串键。
        /// </summary>
        /// <param name="name">枚举值。</param>
        /// <returns>返回本地化键值字符串。</returns>
        protected virtual string GetResouceName(Enum name)
        {
            return $"{name.GetType().Name}_{name}";
        }

        /// <summary>
        /// 返回本地化字符串。
        /// </summary>
        /// <param name="name">当前枚举值。</param>
        /// <returns>返回本地化字符串。</returns>
        public string L(Enum name)
        {
            return GetResourceManager(name).GetString(GetResouceName(name));
        }

        /// <summary>
        /// 返回本地化字符串。
        /// </summary>
        /// <param name="name">当前枚举值。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回本地化字符串。</returns>
        public string L(Enum name, params object[] args)
        {
            return string.Format(L(name), args);
        }

        /// <summary>
        /// 获取枚举的本地化字符串。
        /// </summary>
        /// <param name="name">枚举值。</param>
        /// <returns>返回本地化字符串。</returns>
        LocalizedHtmlString IEnumLocalizer.this[Enum name]
        {
            get
            {
                var resouce = L(name);
                return new LocalizedHtmlString(name.ToString(), resouce, resouce != null);
            }
        }

        /// <summary>
        /// 获取枚举的本地化字符串。
        /// </summary>
        /// <param name="name">枚举值。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回本地化字符串。</returns>
        LocalizedHtmlString IEnumLocalizer.this[Enum name, params object[] args]
        {
            get
            {
                var resouce = L(name);
                return new LocalizedHtmlString(name.ToString(), resouce, resouce != null, args);
            }
        }
    }
}