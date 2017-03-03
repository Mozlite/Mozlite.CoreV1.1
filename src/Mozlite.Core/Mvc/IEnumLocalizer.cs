using System;
using Microsoft.AspNetCore.Mvc.Localization;
using Mozlite.Core;

namespace Mozlite.Mvc
{
    /// <summary>
    /// 枚举本地化接口。
    /// </summary>
    public interface IEnumLocalizer : ISingletonService
    {
        /// <summary>
        /// 返回本地化字符串。
        /// </summary>
        /// <param name="name">当前枚举值。</param>
        /// <returns>返回本地化字符串。</returns>
        string L(Enum name);

        /// <summary>
        /// 返回本地化字符串。
        /// </summary>
        /// <param name="name">当前枚举值。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回本地化字符串。</returns>
        string L(Enum name, params object[] args);

        /// <summary>
        /// 获取枚举的本地化字符串。
        /// </summary>
        /// <param name="name">枚举值。</param>
        /// <returns>返回本地化字符串。</returns>
        LocalizedHtmlString this[Enum name] { get; }

        /// <summary>
        /// 获取枚举的本地化字符串。
        /// </summary>
        /// <param name="name">枚举值。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回本地化字符串。</returns>
        LocalizedHtmlString this[Enum name, params object[] args] { get; }
    }
}