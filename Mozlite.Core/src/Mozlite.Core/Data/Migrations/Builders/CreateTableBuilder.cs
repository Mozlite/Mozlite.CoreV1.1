using System;
using System.Linq;
using System.Linq.Expressions;
using Mozlite.Data.Metadata;
using Mozlite.Data.Migrations.Operations;

namespace Mozlite.Data.Migrations.Builders
{
    /// <summary>
    /// 添加表格构建实例。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public class CreateTableBuilder<TEntity> : OperationBuilder<CreateTableOperation>
    {
        private readonly IModel _model;
        private readonly IEntityType _entity;

        /// <summary>
        /// 初始化类<see cref="CreateTableBuilder{TColumns}"/>
        /// </summary>
        /// <param name="operation">新建表格的操作实例。</param>
        /// <param name="model">模型接口。</param>
        public CreateTableBuilder(
            [NotNull] CreateTableOperation operation,
            [NotNull] IModel model)
            : base(operation)
        {
            Check.NotNull(model, nameof(model));
            _model = model;
            _entity = model.GetEntity(typeof(TEntity));
        }

        /// <summary>
        /// 添加外键。
        /// </summary>
        /// <typeparam name="TPrincipal">主键类。</typeparam>
        /// <param name="columns">字段。</param>
        /// <param name="principalColumns">主键列。</param>
        /// <param name="onUpdate">更新时候对应的操作。</param>
        /// <param name="onDelete">删除时候对应的操作。</param>
        /// <param name="action">添加扩展操作。</param>
        /// <returns>返回迁移构建实例。</returns>
        public virtual CreateTableBuilder<TEntity> ForeignKey<TPrincipal>(
            [NotNull] Expression<Func<TEntity, object>> columns,
            [NotNull] Expression<Func<TPrincipal, object>> principalColumns = null,
            ReferentialAction onUpdate = ReferentialAction.NoAction,
            ReferentialAction onDelete = ReferentialAction.NoAction,
            Action<OperationBuilder<AddForeignKeyOperation>> action = null)
        {
            Check.NotNull(columns, nameof(columns));

            var operation = new AddForeignKeyOperation
            {
                Table = Operation.Table,
                Columns = columns.GetPropertyNames(),
                PrincipalTable = _model.GetTable(typeof(TPrincipal)),
                OnUpdate = onUpdate,
                OnDelete = onDelete
            };
            if (principalColumns == null)
                operation.PrincipalColumns = operation.Columns;
            else
                operation.PrincipalColumns = principalColumns.GetPropertyNames();
            operation.Name = OperationHelper.GetName(NameType.ForeignKey, operation.Table, operation.Columns, operation.PrincipalTable);
            Operation.ForeignKeys.Add(operation);

            action?.Invoke(new OperationBuilder<AddForeignKeyOperation>(operation));
            return this;
        }

        /// <summary>
        /// 添加主键。
        /// </summary>
        /// <param name="action">添加扩展信息。</param>
        /// <returns>返回迁移构建实例。</returns>
        public virtual CreateTableBuilder<TEntity> PrimaryKey(Action<OperationBuilder<AddPrimaryKeyOperation>> action = null)
        {
            var key = _model.GetEntity(typeof(TEntity)).PrimaryKey;
            if (key == null)
                return this;

            var operation = new AddPrimaryKeyOperation
            {
                Table = Operation.Table,
                Columns = key.Properties.Select(p => p.Name).ToArray()
            };
            operation.Name = OperationHelper.GetName(NameType.PrimaryKey, operation.Table);
            Operation.PrimaryKey = operation;

            action?.Invoke(new OperationBuilder<AddPrimaryKeyOperation>(operation));
            return this;
        }

        /// <summary>
        /// 添加唯一键。
        /// </summary>
        /// <param name="columns">列。</param>
        /// <param name="action">添加扩展信息。</param>
        /// <returns>返回迁移构建实例。</returns>
        public virtual CreateTableBuilder<TEntity> UniqueConstraint(
            [NotNull] Expression<Func<TEntity, object>> columns,
            Action<OperationBuilder<AddUniqueConstraintOperation>> action = null)
        {
            Check.NotNull(columns, nameof(columns));

            var operation = new AddUniqueConstraintOperation
            {
                Table = Operation.Table,
                Columns = columns.GetPropertyNames()
            };
            operation.Name = OperationHelper.GetName(NameType.UniqueKey, operation.Table, operation.Columns);
            Operation.UniqueConstraints.Add(operation);

            action?.Invoke(new OperationBuilder<AddUniqueConstraintOperation>(operation));
            return this;
        }

        /// <summary>
        /// 添加列。
        /// </summary>
        /// <typeparam name="T">属性类型。</typeparam>
        /// <param name="column">列表达式。</param>
        /// <param name="type">字段类型。</param>
        /// <param name="unicode">是否为Unicode编码。</param>
        /// <param name="nullable">是否为空。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <param name="defaultValueSql">默认值SQL字符串。</param>
        /// <param name="computedColumnSql">计算列的SQL字符串。</param>
        /// <param name="action">添加扩展。</param>
        /// <returns>返回操作实例。</returns>
        public virtual CreateTableBuilder<TEntity> Column<T>(
            [NotNull] Expression<Func<TEntity, T>> column,
            [CanBeNull] string type = null,
            bool? nullable = null,
            bool? unicode = null,
            [CanBeNull] object defaultValue = null,
            [CanBeNull] string defaultValueSql = null,
            [CanBeNull] string computedColumnSql = null,
            Action<OperationBuilder<AddColumnOperation>> action = null)
        {
            Check.NotNull(column, nameof(column));

            var property = _entity.FindProperty(column.GetPropertyAccess().Name);
            var operation = new AddColumnOperation
            {
                Table = Operation.Table,
                Name = property.Name,
                ClrType = property.ClrType,
                ColumnType = type,
                IsUnicode = unicode,
                IsIdentity = property.IsIdentity,
                MaxLength = property.GetSize(),
                IsRowVersion = property.IsRowVersion() ?? false,
                IsNullable = nullable ?? property.IsNullable,
                DefaultValue = defaultValue,
                DefaultValueSql = defaultValueSql,
                ComputedColumnSql = computedColumnSql
            };
            Operation.Columns.Add(operation);

            action?.Invoke(new OperationBuilder<AddColumnOperation>(operation));
            return this;
        }

        /// <summary>
        /// 添加扩展属性。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="value">值。</param>
        /// <returns>返回新建表格构建实例对象。</returns>
        public new virtual CreateTableBuilder<TEntity> Annotation([NotNull] string name, [NotNull] object value)
            => (CreateTableBuilder<TEntity>)base.Annotation(name, value);
    }
}
