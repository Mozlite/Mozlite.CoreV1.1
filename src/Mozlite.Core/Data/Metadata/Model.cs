using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Mozlite.Data.Metadata.Internal;

namespace Mozlite.Data.Metadata
{
    /// <summary>
    /// 模型类型。
    /// </summary>
    public class Model : IModel
    {
        private readonly ISqlHelper _sqlHelper;
        /// <summary>
        /// 初始化类<see cref="Model"/>。
        /// </summary>
        /// <param name="sqlHelper">SQL辅助接口。</param>
        public Model(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        private readonly ConcurrentDictionary<Type, ITable> _tables = new ConcurrentDictionary<Type, ITable>();
        private readonly ConcurrentDictionary<Type, Lazy<IEntityType>> _entityTypes = new ConcurrentDictionary<Type, Lazy<IEntityType>>();

        /// <summary>
        /// 通过类型获取实体类型。
        /// </summary>
        /// <param name="type">当前类型实例。</param>
        /// <returns>返回实体类型。</returns>
        public IEntityType GetEntity(Type type)
        {
            return _entityTypes
                .GetOrAdd(type, _ => new Lazy<IEntityType>(() => new EntityType(type)))
                .Value;
        }

        /// <summary>
        /// 获取表格。
        /// </summary>
        /// <param name="type">当前类型。</param>
        /// <returns>返回表格。</returns>
        public ITable GetTable(Type type)
        {
            return _tables.GetOrAdd(type, _ =>
            {
                var tableAttribute = type.GetTypeInfo().GetCustomAttribute<TableAttribute>();
                if (tableAttribute == null)
                {
                    var name = type.FullName
                        .Replace("Mozlite.Extensions.", string.Empty)
                        .Replace(".Models.", ".")
                        .Replace("..", ".")
                        .Replace(".", "_")
                        .Trim('_');
                    name = $"$pre:{name}";
                    return new Table(name, null, _sqlHelper.DelimitIdentifier(name));
                }
                if (tableAttribute.EntityType != null)
                    return GetTable(tableAttribute.EntityType);
                var tableName = $"$pre:{tableAttribute.Name}";
                return new Table(tableName, tableAttribute.Schema, _sqlHelper.DelimitIdentifier(tableName, tableAttribute.Schema), type);
            });
        }

        /// <summary>
        /// 实例化一个表格，不进行缓存。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <returns>返回表格实例。</returns>
        public ITable GetTable(string name, string schema)
        {
            return new Table(name, schema, _sqlHelper.DelimitIdentifier(name, schema));
        }

        /// <summary>
        /// 通过表格名称和架构获取当前实体。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <returns>返回实体对象。</returns>
        public IEntityType FindEntity(string name, string schema)
        {
            var type = _tables.Values.FirstOrDefault(table =>
                table.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(schema, table.Schema, StringComparison.OrdinalIgnoreCase) &&
                table.DeclaringType != null)
                ?.DeclaringType;
            if (type == null)
                return null;
            return GetEntity(type);
        }

        private class Table : ITable
        {
            private readonly string _tableName;
            public Table(string name, string schema, string tableName, Type declaringType = null)
            {
                _tableName = tableName;
                DeclaringType = declaringType;
                Name = name;
                Schema = schema;
            }

            /// <summary>
            /// 名称。
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// 架构。
            /// </summary>
            public string Schema { get; }

            /// <summary>
            /// 声明类型。
            /// </summary>
            public Type DeclaringType { get; }

            /// <summary>返回表示当前对象的字符串。</summary>
            /// <returns>表示当前对象的字符串。</returns>
            public override string ToString()
            {
                return _tableName;
            }
        }
    }
}