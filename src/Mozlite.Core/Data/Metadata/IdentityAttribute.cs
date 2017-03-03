using System;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 自增长特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityAttribute : Attribute
    {
    }
}