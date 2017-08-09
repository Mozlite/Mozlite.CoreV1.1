using System;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 外键特性。
    /// </summary>
    public class ForeignKeyAttribute
    {
        public ForeignKeyAttribute(string[] columns, Type pricipalType, string[] piColumns = null,
            DeleteBehavior behavior = DeleteBehavior.Restrict)
        {

        }
    }
}