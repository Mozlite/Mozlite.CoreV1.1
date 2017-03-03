using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mozlite.Data.Metadata;
using Mozlite.Data.Migrations.Builders;
using Mozlite.Data.Migrations.Operations;

namespace Mozlite.Data.Migrations
{
    /// <summary>
    /// 迁移构建实例。
    /// </summary>
    public class MigrationBuilder
    {
        /// <summary>
        /// 初始化类<see cref="MigrationBuilder"/>。
        /// </summary>
        /// <param name="activeProvider">数据库提供者。</param>
        /// <param name="model">模型接口。</param>
        public MigrationBuilder([CanBeNull] string activeProvider, IModel model)
        {
            ActiveProvider = activeProvider;
            Model = model;
        }

        /// <summary>
        /// 激活的提供者。
        /// </summary>
        public virtual string ActiveProvider { get; }

        /// <summary>
        /// 模型接口。
        /// </summary>
        public IModel Model { get; }

        /// <summary>
        /// 操作列表。
        /// </summary>
        public virtual List<MigrationOperation> Operations { get; } = new List<MigrationOperation>();

        /// <summary>
        /// 添加列。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="column">列表达式。</param>
        /// <param name="type">字段类型。</param>
        /// <param name="unicode">是否为Unicode编码。</param>
        /// <param name="nullable">是否为空。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <param name="defaultValueSql">默认值SQL字符串。</param>
        /// <param name="computedColumnSql">计算列的SQL字符串。</param>
        /// <returns>返回操作实例。</returns>
        public virtual OperationBuilder<AddColumnOperation> AddColumn<TEntity>(
            [NotNull] Expression<Func<TEntity, object>> column,
            [CanBeNull] string type = null,
            bool? nullable = null,
            bool? unicode = null,
            [CanBeNull] object defaultValue = null,
            [CanBeNull] string defaultValueSql = null,
            [CanBeNull] string computedColumnSql = null)
        {
            Check.NotNull(column, nameof(column));

            var property = Model.GetEntity(typeof(TEntity)).FindProperty(column.GetPropertyAccess().Name);
            var operation = new AddColumnOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
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
            Operations.Add(operation);

            return new OperationBuilder<AddColumnOperation>(operation);
        }

        /// <summary>
        /// 添加外键。
        /// </summary>
        /// <typeparam name="TEntity">实体类。</typeparam>
        /// <typeparam name="TPrincipal">主键类。</typeparam>
        /// <param name="columns">字段。</param>
        /// <param name="principalColumns">主键列。</param>
        /// <param name="onUpdate">更新时候对应的操作。</param>
        /// <param name="onDelete">删除时候对应的操作。</param>
        /// <returns>返回迁移构建实例。</returns>
        public virtual OperationBuilder<AddForeignKeyOperation> AddForeignKey<TEntity, TPrincipal>(
            [NotNull] Expression<Func<TEntity, object>> columns,
            [NotNull] Expression<Func<TPrincipal, object>> principalColumns = null,
            ReferentialAction onUpdate = ReferentialAction.NoAction,
            ReferentialAction onDelete = ReferentialAction.NoAction)
        {
            Check.NotNull(columns, nameof(columns));

            var operation = new AddForeignKeyOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
                Columns = columns.GetPropertyNames(),
                PrincipalTable = Model.GetTable(typeof(TPrincipal)),
                OnUpdate = onUpdate,
                OnDelete = onDelete
            };
            if (principalColumns == null)
                operation.PrincipalColumns = operation.Columns;
            else
                operation.PrincipalColumns = principalColumns.GetPropertyNames();
            operation.Name = OperationHelper.GetName(NameType.ForeignKey, operation.Table, operation.Columns, operation.PrincipalTable);
            Operations.Add(operation);

            return new OperationBuilder<AddForeignKeyOperation>(operation);
        }

        /// <summary>
        /// 添加主键。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="columns">列表达式。</param>
        /// <returns>返回构建实例对象。</returns>
        public virtual OperationBuilder<AddPrimaryKeyOperation> AddPrimaryKey<TEntity>([NotNull] Expression<Func<TEntity, object>> columns)
        {
            Check.NotNull(columns, nameof(columns));

            var operation = new AddPrimaryKeyOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
                Columns = columns.GetPropertyNames()
            };
            operation.Name = OperationHelper.GetName(NameType.PrimaryKey, operation.Table);
            Operations.Add(operation);

            return new OperationBuilder<AddPrimaryKeyOperation>(operation);
        }

        /// <summary>
        /// 添加唯一键。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="columns">列表达式。</param>
        /// <returns>返回构建实例对象。</returns>
        public virtual OperationBuilder<AddUniqueConstraintOperation> AddUniqueConstraint<TEntity>([NotNull] Expression<Func<TEntity, object>> columns)
        {
            Check.NotNull(columns, nameof(columns));

            var operation = new AddUniqueConstraintOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
                Columns = columns.GetPropertyNames()
            };
            operation.Name = OperationHelper.GetName(NameType.UniqueKey, operation.Table, operation.Columns);
            Operations.Add(operation);

            return new OperationBuilder<AddUniqueConstraintOperation>(operation);
        }

        /// <summary>
        /// 修改列。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="type">字段类型。</param>
        /// <param name="unicode">是否为Unicode编码。</param>
        /// <param name="column">列表达式。</param>
        /// <param name="nullable">是否为空。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <param name="defaultValueSql">默认值SQL字符串。</param>
        /// <param name="computedColumnSql">计算列的SQL字符串。</param>
        /// <param name="oldClrType">原来数据类型。</param>
        /// <param name="oldType">原来字段类型。</param>
        /// <param name="oldUnicode">原来是否为Unicode编码。</param>
        /// <param name="oldMaxLength">原来长度。</param>
        /// <param name="oldIdentity">是否为自增长。</param>
        /// <param name="oldRowVersion">原来是否为RowVersion。</param>
        /// <param name="oldNullable">原来是否为空。</param>
        /// <param name="oldDefaultValue">原来默认值。</param>
        /// <param name="oldDefaultValueSql">原来默认值SQL字符串。</param>
        /// <param name="oldComputedColumnSql">原来计算列的SQL字符串。</param>
        /// <returns>返回操作实例。</returns>
        public virtual AlterOperationBuilder<AlterColumnOperation> AlterColumn<TEntity>(
            [NotNull] Expression<Func<TEntity, object>> column,
            [CanBeNull] string type = null,
            [CanBeNull] bool? unicode = null,
            [CanBeNull] bool? nullable = null,
            [CanBeNull] object defaultValue = null,
            [CanBeNull] string defaultValueSql = null,
            [CanBeNull] string computedColumnSql = null,
            [CanBeNull] Type oldClrType = null,
            [CanBeNull] string oldType = null,
            [CanBeNull] bool? oldUnicode = null,
            [CanBeNull] int? oldMaxLength = null,
            bool oldIdentity = false,
            bool oldRowVersion = false,
            bool oldNullable = false,
            [CanBeNull] object oldDefaultValue = null,
            [CanBeNull] string oldDefaultValueSql = null,
            [CanBeNull] string oldComputedColumnSql = null)
        {
            Check.NotNull(column, nameof(column));
            var property = Model.GetEntity(typeof(TEntity)).FindProperty(column.GetPropertyAccess().Name);
            var operation = new AlterColumnOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
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
                ComputedColumnSql = computedColumnSql,
                OldColumn = new ColumnOperation
                {
                    ClrType = oldClrType ?? property.ClrType,
                    ColumnType = oldType,
                    IsUnicode = oldUnicode,
                    MaxLength = oldMaxLength,
                    IsIdentity = oldIdentity,
                    IsRowVersion = oldRowVersion,
                    IsNullable = oldNullable,
                    DefaultValue = oldDefaultValue,
                    DefaultValueSql = oldDefaultValueSql,
                    ComputedColumnSql = oldComputedColumnSql
                }
            };

            Operations.Add(operation);

            return new AlterOperationBuilder<AlterColumnOperation>(operation);
        }

        /// <summary>
        /// 修改数据库。
        /// </summary>
        /// <returns>返回构建实例。</returns>
        public virtual AlterOperationBuilder<AlterDatabaseOperation> AlterDatabase()
        {
            var operation = new AlterDatabaseOperation();
            Operations.Add(operation);

            return new AlterOperationBuilder<AlterDatabaseOperation>(operation);
        }

        /// <summary>
        /// 修改序列号。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <param name="incrementBy">增量。</param>
        /// <param name="minValue">最小值。</param>
        /// <param name="maxValue">最大值。</param>
        /// <param name="cyclic">是否循环。</param>
        /// <param name="oldIncrementBy">原增量。</param>
        /// <param name="oldMinValue">原最小值。</param>
        /// <param name="oldMaxValue">原最大值。</param>
        /// <param name="oldCyclic">是否循环。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual AlterOperationBuilder<AlterSequenceOperation> AlterSequence(
            [NotNull] string name,
            [CanBeNull] string schema = null,
            int incrementBy = 1,
            [CanBeNull] long? minValue = null,
            [CanBeNull] long? maxValue = null,
            bool cyclic = false,
            int oldIncrementBy = 1,
            [CanBeNull] long? oldMinValue = null,
            [CanBeNull] long? oldMaxValue = null,
            bool oldCyclic = false)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new AlterSequenceOperation
            {
                Schema = schema,
                Name = name,
                IncrementBy = incrementBy,
                MinValue = minValue,
                MaxValue = maxValue,
                IsCyclic = cyclic,
                OldSequence = new SequenceOperation
                {
                    IncrementBy = oldIncrementBy,
                    MinValue = oldMinValue,
                    MaxValue = oldMaxValue,
                    IsCyclic = oldCyclic
                }
            };
            Operations.Add(operation);

            return new AlterOperationBuilder<AlterSequenceOperation>(operation);
        }

        /// <summary>
        /// 修改表格。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <returns>返回迁移实例。</returns>
        public virtual AlterOperationBuilder<AlterTableOperation> AlterTable<TEntity>()
        {
            var operation = new AlterTableOperation
            {
                Table = Model.GetTable(typeof(TEntity))
            };
            Operations.Add(operation);
            return new AlterOperationBuilder<AlterTableOperation>(operation);
        }

        /// <summary>
        /// 新建索引。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="columns">列表达式。</param>
        /// <param name="unique">是否唯一。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<CreateIndexOperation> CreateIndex<TEntity>(
            [NotNull] Expression<Func<TEntity, object>> columns,
            bool unique = false)
        {
            Check.NotNull(columns, nameof(columns));

            var operation = new CreateIndexOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
                Columns = columns.GetPropertyNames(),
                IsUnique = unique
            };
            operation.Name = OperationHelper.GetName(NameType.Index, operation.Table, operation.Columns);
            Operations.Add(operation);

            return new OperationBuilder<CreateIndexOperation>(operation);
        }

        /// <summary>
        /// 确认架构。
        /// </summary>
        /// <param name="name">架构名称。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<EnsureSchemaOperation> EnsureSchema(
            [NotNull] string name)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new EnsureSchemaOperation
            {
                Name = name
            };
            Operations.Add(operation);

            return new OperationBuilder<EnsureSchemaOperation>(operation);
        }

        /// <summary>
        /// 新建序列号。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <param name="startValue">起始值。</param>
        /// <param name="incrementBy">增量。</param>
        /// <param name="minValue">最小值。</param>
        /// <param name="maxValue">最大值。</param>
        /// <param name="cyclic">是否循环。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<CreateSequenceOperation> CreateSequence(
                [NotNull] string name,
                [CanBeNull] string schema = null,
                long startValue = 1L,
                int incrementBy = 1,
                [CanBeNull] long? minValue = null,
                [CanBeNull] long? maxValue = null,
                bool cyclic = false)
            => CreateSequence<long>(name, schema, startValue, incrementBy, minValue, maxValue, cyclic);

        /// <summary>
        /// 新建序列号。
        /// </summary>
        /// <typeparam name="T">列类型。</typeparam>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <param name="startValue">起始值。</param>
        /// <param name="incrementBy">增量。</param>
        /// <param name="minValue">最小值。</param>
        /// <param name="maxValue">最大值。</param>
        /// <param name="cyclic">是否循环。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<CreateSequenceOperation> CreateSequence<T>(
            [NotNull] string name,
            [CanBeNull] string schema = null,
            long startValue = 1L,
            int incrementBy = 1,
            [CanBeNull] long? minValue = null,
            [CanBeNull] long? maxValue = null,
            bool cyclic = false)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new CreateSequenceOperation
            {
                Schema = schema,
                Name = name,
                ClrType = typeof(T),
                StartValue = startValue,
                IncrementBy = incrementBy,
                MinValue = minValue,
                MaxValue = maxValue,
                IsCyclic = cyclic
            };
            Operations.Add(operation);

            return new OperationBuilder<CreateSequenceOperation>(operation);
        }

        /// <summary>
        /// 新建表格。
        /// </summary>
        /// <typeparam name="TEntity">表格实体类型。</typeparam>
        /// <param name="action">表格列定义表达式。</param>
        /// <returns>返回当前构建实例。</returns>
        public virtual CreateTableBuilder<TEntity> CreateTable<TEntity>([NotNull] Action<CreateTableBuilder<TEntity>> action)
        {
            Check.NotNull(action, nameof(action));

            var createTableOperation = new CreateTableOperation
            {
                Table = Model.GetTable(typeof(TEntity))
            };

            var builder = new CreateTableBuilder<TEntity>(createTableOperation, Model);
            action(builder);
            if (createTableOperation.PrimaryKey == null)
                builder.PrimaryKey();

            Operations.Add(createTableOperation);

            return builder;
        }

        /// <summary>
        /// 删除列。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">列名称。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropColumnOperation> DropColumn<TEntity>([NotNull] Expression<Func<TEntity, object>> name)
        {
            return DropColumn<TEntity>(name.GetPropertyAccess().Name);
        }

        /// <summary>
        /// 删除列。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">列名称。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropColumnOperation> DropColumn<TEntity>([NotNull] string name)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new DropColumnOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
                Name = name
            };
            Operations.Add(operation);
            return new OperationBuilder<DropColumnOperation>(operation);
        }

        /// <summary>
        /// 删除外键。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="columns">列表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropForeignKeyOperation> DropForeignKey<TEntity>(
            [NotNull] Expression<Func<TEntity, object>> columns)
        {
            Check.NotNull(columns, nameof(columns));

            var operation = new DropForeignKeyOperation
            {
                Table = Model.GetTable(typeof(TEntity))
            };
            operation.Name = OperationHelper.GetName(NameType.ForeignKey, operation.Table, columns.GetPropertyNames());
            Operations.Add(operation);

            return new OperationBuilder<DropForeignKeyOperation>(operation);
        }

        /// <summary>
        /// 删除外键。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">外键名称(全名，包含FK_)。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropForeignKeyOperation> DropForeignKey<TEntity>([NotNull]string name)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new DropForeignKeyOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
                Name = name
            };
            Operations.Add(operation);

            return new OperationBuilder<DropForeignKeyOperation>(operation);
        }

        /// <summary>
        /// 删除索引。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="columns">列表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropIndexOperation> DropIndex<TEntity>(
            [NotNull] Expression<Func<TEntity, object>> columns)
        {
            Check.NotNull(columns, nameof(columns));

            var operation = new DropIndexOperation
            {
                Table = Model.GetTable(typeof(TEntity))
            };
            operation.Name = OperationHelper.GetName(NameType.Index, operation.Table, columns.GetPropertyNames());
            Operations.Add(operation);

            return new OperationBuilder<DropIndexOperation>(operation);
        }

        /// <summary>
        /// 删除索引。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">索引名称（全名，包含IX_）。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropIndexOperation> DropIndex<TEntity>([NotNull] string name)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new DropIndexOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
                Name = name
            };
            Operations.Add(operation);

            return new OperationBuilder<DropIndexOperation>(operation);
        }

        /// <summary>
        /// 删除主键。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropPrimaryKeyOperation> DropPrimaryKey<TEntity>()
        {
            var operation = new DropPrimaryKeyOperation
            {
                Table = Model.GetTable(typeof(TEntity))
            };
            operation.Name = OperationHelper.GetName(NameType.PrimaryKey, operation.Table);
            Operations.Add(operation);

            return new OperationBuilder<DropPrimaryKeyOperation>(operation);
        }

        /// <summary>
        /// 删除架构。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropSchemaOperation> DropSchema(
            [NotNull] string name)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new DropSchemaOperation
            {
                Name = name
            };
            Operations.Add(operation);

            return new OperationBuilder<DropSchemaOperation>(operation);
        }

        /// <summary>
        /// 删除序列号。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropSequenceOperation> DropSequence(
            [NotNull] string name,
            [CanBeNull] string schema = null)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new DropSequenceOperation
            {
                Schema = schema,
                Name = name
            };
            Operations.Add(operation);

            return new OperationBuilder<DropSequenceOperation>(operation);
        }

        /// <summary>
        /// 删除表格。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropTableOperation> DropTable<TEntity>()
        {
            var operation = new DropTableOperation
            {
                Table = Model.GetTable(typeof(TEntity))
            };
            Operations.Add(operation);

            return new OperationBuilder<DropTableOperation>(operation);
        }

        /// <summary>
        /// 删除表格。
        /// </summary>
        /// <param name="name">表格名称。</param>
        /// <param name="schema">架构。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropTableOperation> DropTable(
            [NotNull] string name,
            [CanBeNull] string schema = null)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new DropTableOperation
            {
                Table = Model.GetTable(name, schema)
            };
            Operations.Add(operation);

            return new OperationBuilder<DropTableOperation>(operation);
        }

        /// <summary>
        /// 删除唯一键。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="columns">列表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropUniqueConstraintOperation> DropUniqueConstraint<TEntity>([NotNull] Expression<Func<TEntity, object>> columns)
        {
            Check.NotNull(columns, nameof(columns));

            var operation = new DropUniqueConstraintOperation
            {
                Table = Model.GetTable(typeof(TEntity))
            };
            operation.Name = OperationHelper.GetName(NameType.UniqueKey, operation.Table, columns.GetPropertyNames());
            Operations.Add(operation);

            return new OperationBuilder<DropUniqueConstraintOperation>(operation);
        }

        /// <summary>
        /// 删除唯一键。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">名称（全名包含UK_）。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropUniqueConstraintOperation> DropUniqueConstraint<TEntity>([NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            var operation = new DropUniqueConstraintOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
                Name = name
            };
            Operations.Add(operation);

            return new OperationBuilder<DropUniqueConstraintOperation>(operation);
        }

        /// <summary>
        /// 修改列名。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">原名称。</param>
        /// <param name="column">新列名表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RenameColumnOperation> RenameColumn<TEntity>(
            [NotNull] string name,
            [NotNull] Expression<Func<TEntity, object>> column)
        {
            Check.NotEmpty(name, nameof(name));
            Check.NotNull(column, nameof(column));

            var operation = new RenameColumnOperation
            {
                Name = name,
                Table = Model.GetTable(typeof(TEntity)),
                NewName = column.GetPropertyAccess().Name
            };
            Operations.Add(operation);

            return new OperationBuilder<RenameColumnOperation>(operation);
        }

        /// <summary>
        /// 修改列名。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">原名称。</param>
        /// <param name="newName">新列名。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RenameColumnOperation> RenameColumn<TEntity>(
            [NotNull] string name,
            [NotNull] string newName)
        {
            Check.NotEmpty(name, nameof(name));
            Check.NotEmpty(newName, nameof(newName));

            var operation = new RenameColumnOperation
            {
                Name = name,
                Table = Model.GetTable(typeof(TEntity)),
                NewName = newName
            };
            Operations.Add(operation);

            return new OperationBuilder<RenameColumnOperation>(operation);
        }

        /// <summary>
        /// 修改列名。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">原名称（全名包含IX_）。</param>
        /// <param name="columns">新列名表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RenameIndexOperation> RenameIndex<TEntity>(
            [NotNull] string name,
            [NotNull] Expression<Func<TEntity, object>> columns)
        {
            Check.NotEmpty(name, nameof(name));
            Check.NotNull(columns, nameof(columns));

            var operation = new RenameIndexOperation
            {
                Table = Model.GetTable(typeof(TEntity)),
                Name = name,
            };
            operation.NewName = OperationHelper.GetName(NameType.Index, operation.Table, columns.GetPropertyNames());
            Operations.Add(operation);

            return new OperationBuilder<RenameIndexOperation>(operation);
        }

        /// <summary>
        /// 修改序列号名称。
        /// </summary>
        /// <param name="name">原名称。</param>
        /// <param name="schema">原架构。</param>
        /// <param name="newName">新名称。</param>
        /// <param name="newSchema">新架构。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RenameSequenceOperation> RenameSequence(
            [NotNull] string name,
            [CanBeNull] string schema = null,
            [CanBeNull] string newName = null,
            [CanBeNull] string newSchema = null)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new RenameSequenceOperation
            {
                Name = name,
                Schema = schema,
                NewName = newName,
                NewSchema = newSchema
            };
            Operations.Add(operation);

            return new OperationBuilder<RenameSequenceOperation>(operation);
        }

        /// <summary>
        /// 修改表名称。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">原名称。</param>
        /// <param name="schema">原架构。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RenameTableOperation> RenameTable<TEntity>(
            [NotNull] string name,
            [CanBeNull] string schema = null)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new RenameTableOperation
            {
                Table = Model.GetTable(name, schema),
                NewTable = Model.GetTable(typeof(TEntity)),
            };
            Operations.Add(operation);

            return new OperationBuilder<RenameTableOperation>(operation);
        }

        /// <summary>
        /// 重新计算序列号。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="startValue">开始值。</param>
        /// <param name="schema">架构。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RestartSequenceOperation> RestartSequence(
            [NotNull] string name,
            long startValue = 1L,
            [CanBeNull] string schema = null)
        {
            Check.NotEmpty(name, nameof(name));

            var operation = new RestartSequenceOperation
            {
                Name = name,
                Schema = schema,
                StartValue = startValue
            };
            Operations.Add(operation);

            return new OperationBuilder<RestartSequenceOperation>(operation);
        }

        /// <summary>
        /// SQL语句。
        /// </summary>
        /// <param name="sql">SQL语句。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> Sql(
            [NotNull] string sql)
        {
            Check.NotEmpty(sql, nameof(sql));

            var operation = new SqlOperation
            {
                Sql = sql
            };
            Operations.Add(operation);

            return new OperationBuilder<SqlOperation>(operation);
        }

        /// <summary>
        /// 新建语句。
        /// </summary>
        /// <typeparam name="TEntity">模型类型。</typeparam>
        /// <param name="instance">对象实例。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> SqlCreate<TEntity>([NotNull] TEntity instance)
        {
            Check.NotNull(instance, nameof(instance));
            var operation = new SqlOperation
            {
                Instance = instance,
                EntityType = typeof(TEntity)
            };
            Operations.Add(operation);

            return new OperationBuilder<SqlOperation>(operation);
        }

        /// <summary>
        /// 更新语句。
        /// </summary>
        /// <typeparam name="TEntity">模型类型。</typeparam>
        /// <param name="expression">条件表达式。</param>
        /// <param name="instance">匿名对象。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> SqlUpdate<TEntity>([NotNull] Expression<Predicate<TEntity>> expression, [NotNull] object instance)
        {
            Check.NotNull(expression, nameof(expression));
            Check.NotNull(instance, nameof(instance));
            var operation = new SqlOperation
            {
                Instance = instance,
                EntityType = typeof(TEntity),
                Expression = expression
            };
            Operations.Add(operation);

            return new OperationBuilder<SqlOperation>(operation);
        }

        /// <summary>
        /// 删除语句。
        /// </summary>
        /// <typeparam name="TEntity">模型类型。</typeparam>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> SqlDelete<TEntity>([NotNull] Expression<Predicate<TEntity>> expression)
        {
            Check.NotNull(expression, nameof(expression));
            var operation = new SqlOperation
            {
                EntityType = typeof(TEntity),
                Expression = expression
            };
            Operations.Add(operation);

            return new OperationBuilder<SqlOperation>(operation);
        }
    }

    /// <summary>
    /// 模型迁移实例类型。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public class MigrationBuilder<TEntity>
    {
        private readonly MigrationBuilder _builder;
        /// <summary>
        /// 初始化类<see cref="MigrationBuilder{TEntity}"/>。
        /// </summary>
        /// <param name="builder">迁移实例。</param>
        public MigrationBuilder(MigrationBuilder builder)
        {
            _builder = builder;
        }

        /// <summary>
        /// 添加列。
        /// </summary>
        /// <param name="column">列表达式。</param>
        /// <param name="type">字段类型。</param>
        /// <param name="unicode">是否为Unicode编码。</param>
        /// <param name="nullable">是否为空。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <param name="defaultValueSql">默认值SQL字符串。</param>
        /// <param name="computedColumnSql">计算列的SQL字符串。</param>
        /// <returns>返回操作实例。</returns>
        public virtual OperationBuilder<AddColumnOperation> AddColumn(
                [NotNull] Expression<Func<TEntity, object>> column,
                [CanBeNull] string type = null,
                bool? nullable = null,
                bool? unicode = null,
                [CanBeNull] object defaultValue = null,
                [CanBeNull] string defaultValueSql = null,
                [CanBeNull] string computedColumnSql = null)
            => _builder.AddColumn(column, type, nullable, unicode, defaultValue, defaultValueSql, computedColumnSql);

        /// <summary>
        /// 添加外键。
        /// </summary>
        /// <typeparam name="TPrincipal">主键类。</typeparam>
        /// <param name="columns">字段。</param>
        /// <param name="principalColumns">主键列。</param>
        /// <param name="onUpdate">更新时候对应的操作。</param>
        /// <param name="onDelete">删除时候对应的操作。</param>
        /// <returns>返回迁移构建实例。</returns>
        public virtual OperationBuilder<AddForeignKeyOperation> AddForeignKey<TPrincipal>(
                [NotNull] Expression<Func<TEntity, object>> columns,
                [NotNull] Expression<Func<TPrincipal, object>> principalColumns = null,
                ReferentialAction onUpdate = ReferentialAction.NoAction,
                ReferentialAction onDelete = ReferentialAction.NoAction)
            => _builder.AddForeignKey(columns, principalColumns, onUpdate, onDelete);

        /// <summary>
        /// 添加主键。
        /// </summary>
        /// <param name="columns">列表达式。</param>
        /// <returns>返回构建实例对象。</returns>
        public virtual OperationBuilder<AddPrimaryKeyOperation> AddPrimaryKey(
                [NotNull] Expression<Func<TEntity, object>> columns)
            => _builder.AddPrimaryKey(columns);

        /// <summary>
        /// 添加唯一键。
        /// </summary>
        /// <param name="columns">列表达式。</param>
        /// <returns>返回构建实例对象。</returns>
        public virtual OperationBuilder<AddUniqueConstraintOperation> AddUniqueConstraint(
                [NotNull] Expression<Func<TEntity, object>> columns)
            => _builder.AddUniqueConstraint(columns);

        /// <summary>
        /// 修改列。
        /// </summary>
        /// <param name="type">字段类型。</param>
        /// <param name="unicode">是否为Unicode编码。</param>
        /// <param name="column">列表达式。</param>
        /// <param name="nullable">是否为空。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <param name="defaultValueSql">默认值SQL字符串。</param>
        /// <param name="computedColumnSql">计算列的SQL字符串。</param>
        /// <param name="oldClrType">原来数据类型。</param>
        /// <param name="oldType">原来字段类型。</param>
        /// <param name="oldUnicode">原来是否为Unicode编码。</param>
        /// <param name="oldMaxLength">原来长度。</param>
        /// <param name="oldIdentity">是否为自增长。</param>
        /// <param name="oldRowVersion">原来是否为RowVersion。</param>
        /// <param name="oldNullable">原来是否为空。</param>
        /// <param name="oldDefaultValue">原来默认值。</param>
        /// <param name="oldDefaultValueSql">原来默认值SQL字符串。</param>
        /// <param name="oldComputedColumnSql">原来计算列的SQL字符串。</param>
        /// <returns>返回操作实例。</returns>
        public virtual AlterOperationBuilder<AlterColumnOperation> AlterColumn(
                [NotNull] Expression<Func<TEntity, object>> column,
                [CanBeNull] string type = null,
                [CanBeNull] bool? unicode = null,
                [CanBeNull] bool? nullable = null,
                [CanBeNull] object defaultValue = null,
                [CanBeNull] string defaultValueSql = null,
                [CanBeNull] string computedColumnSql = null,
                [CanBeNull] Type oldClrType = null,
                [CanBeNull] string oldType = null,
                [CanBeNull] bool? oldUnicode = null,
                [CanBeNull] int? oldMaxLength = null,
                bool oldIdentity = false,
                bool oldRowVersion = false,
                bool oldNullable = false,
                [CanBeNull] object oldDefaultValue = null,
                [CanBeNull] string oldDefaultValueSql = null,
                [CanBeNull] string oldComputedColumnSql = null)
            =>
            _builder.AlterColumn(column, type, unicode, nullable, defaultValue, defaultValueSql, computedColumnSql,
                oldClrType, oldType, oldUnicode, oldMaxLength, oldIdentity, oldRowVersion, oldNullable, oldDefaultValue,
                oldDefaultValueSql, oldComputedColumnSql);

        /// <summary>
        /// 修改数据库。
        /// </summary>
        /// <returns>返回构建实例。</returns>
        public virtual AlterOperationBuilder<AlterDatabaseOperation> AlterDatabase()
            => _builder.AlterDatabase();

        /// <summary>
        /// 修改序列号。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <param name="incrementBy">增量。</param>
        /// <param name="minValue">最小值。</param>
        /// <param name="maxValue">最大值。</param>
        /// <param name="cyclic">是否循环。</param>
        /// <param name="oldIncrementBy">原增量。</param>
        /// <param name="oldMinValue">原最小值。</param>
        /// <param name="oldMaxValue">原最大值。</param>
        /// <param name="oldCyclic">是否循环。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual AlterOperationBuilder<AlterSequenceOperation> AlterSequence(
                [NotNull] string name,
                [CanBeNull] string schema = null,
                int incrementBy = 1,
                [CanBeNull] long? minValue = null,
                [CanBeNull] long? maxValue = null,
                bool cyclic = false,
                int oldIncrementBy = 1,
                [CanBeNull] long? oldMinValue = null,
                [CanBeNull] long? oldMaxValue = null,
                bool oldCyclic = false)
            =>
            _builder.AlterSequence(name, schema, incrementBy, minValue, maxValue, cyclic, oldIncrementBy, oldMinValue,
                oldMaxValue, oldCyclic);

        /// <summary>
        /// 修改表格。
        /// </summary>
        /// <returns>返回迁移实例。</returns>
        public virtual AlterOperationBuilder<AlterTableOperation> AlterTable()
            => _builder.AlterTable<TEntity>();

        /// <summary>
        /// 新建索引。
        /// </summary>
        /// <param name="columns">列表达式。</param>
        /// <param name="unique">是否唯一。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<CreateIndexOperation> CreateIndex(
                [NotNull] Expression<Func<TEntity, object>> columns,
                bool unique = false)
            => _builder.CreateIndex(columns, unique);

        /// <summary>
        /// 新建索引。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="columns">列表达式。</param>
        /// <param name="unique">是否唯一。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<CreateIndexOperation> CreateIndex<TModel>(
                [NotNull] Expression<Func<TModel, object>> columns,
                bool unique = false)
            => _builder.CreateIndex(columns, unique);

        /// <summary>
        /// 确认架构。
        /// </summary>
        /// <param name="name">架构名称。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<EnsureSchemaOperation> EnsureSchema(
                [NotNull] string name)
            => _builder.EnsureSchema(name);

        /// <summary>
        /// 新建序列号。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <param name="startValue">起始值。</param>
        /// <param name="incrementBy">增量。</param>
        /// <param name="minValue">最小值。</param>
        /// <param name="maxValue">最大值。</param>
        /// <param name="cyclic">是否循环。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<CreateSequenceOperation> CreateSequence(
                [NotNull] string name,
                [CanBeNull] string schema = null,
                long startValue = 1L,
                int incrementBy = 1,
                [CanBeNull] long? minValue = null,
                [CanBeNull] long? maxValue = null,
                bool cyclic = false)
            => CreateSequence<long>(name, schema, startValue, incrementBy, minValue, maxValue, cyclic);

        /// <summary>
        /// 新建序列号。
        /// </summary>
        /// <typeparam name="T">列类型。</typeparam>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <param name="startValue">起始值。</param>
        /// <param name="incrementBy">增量。</param>
        /// <param name="minValue">最小值。</param>
        /// <param name="maxValue">最大值。</param>
        /// <param name="cyclic">是否循环。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<CreateSequenceOperation> CreateSequence<T>(
                [NotNull] string name,
                [CanBeNull] string schema = null,
                long startValue = 1L,
                int incrementBy = 1,
                [CanBeNull] long? minValue = null,
                [CanBeNull] long? maxValue = null,
                bool cyclic = false)
            => _builder.CreateSequence<T>(name, schema, startValue, incrementBy, minValue, maxValue, cyclic);

        /// <summary>
        /// 新建表格。
        /// </summary>
        /// <param name="action">表格列定义表达式。</param>
        /// <returns>返回当前构建实例。</returns>
        public CreateTableBuilder<TEntity> CreateTable([NotNull] Action<CreateTableBuilder<TEntity>> action)
            => CreateTable<TEntity>(action);

        /// <summary>
        /// 新建表格。
        /// </summary>
        /// <param name="action">表格列定义表达式。</param>
        /// <returns>返回当前构建实例。</returns>
        public virtual CreateTableBuilder<TModel> CreateTable<TModel>([NotNull] Action<CreateTableBuilder<TModel>> action)
            => _builder.CreateTable(action);

        /// <summary>
        /// 删除列。
        /// </summary>
        /// <param name="name">列名称。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropColumnOperation> DropColumn([NotNull] Expression<Func<TEntity, object>> name)
            => _builder.DropColumn(name);

        /// <summary>
        /// 删除列。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">列名称。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropColumnOperation> DropColumn([NotNull] string name)
            => _builder.DropColumn<TEntity>(name);

        /// <summary>
        /// 删除外键。
        /// </summary>
        /// <param name="columns">列表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropForeignKeyOperation> DropForeignKey(
                [NotNull] Expression<Func<TEntity, object>> columns)
            => _builder.DropForeignKey(columns);

        /// <summary>
        /// 删除外键。
        /// </summary>
        /// <param name="name">外键名称(全名，包含FK_)。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropForeignKeyOperation> DropForeignKey([NotNull] string name)
            => _builder.DropForeignKey<TEntity>(name);

        /// <summary>
        /// 删除索引。
        /// </summary>
        /// <param name="columns">列表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropIndexOperation> DropIndex(
            [NotNull] Expression<Func<TEntity, object>> columns) => _builder.DropIndex(columns);

        /// <summary>
        /// 删除索引。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="name">索引名称（全名，包含IX_）。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropIndexOperation> DropIndex([NotNull] string name) => _builder.DropIndex<TEntity>(name);

        /// <summary>
        /// 删除主键。
        /// </summary>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropPrimaryKeyOperation> DropPrimaryKey() => _builder.DropPrimaryKey<TEntity>();

        /// <summary>
        /// 删除架构。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropSchemaOperation> DropSchema(
            [NotNull] string name) => _builder.DropSchema(name);

        /// <summary>
        /// 删除序列号。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="schema">架构。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropSequenceOperation> DropSequence(
            [NotNull] string name,
            [CanBeNull] string schema = null) => _builder.DropSequence(name, schema);

        /// <summary>
        /// 删除表格。
        /// </summary>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropTableOperation> DropTable() => _builder.DropTable<TEntity>();

        /// <summary>
        /// 删除表格。
        /// </summary>
        /// <param name="name">表格名称。</param>
        /// <param name="schema">架构。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropTableOperation> DropTable(
            [NotNull] string name,
            [CanBeNull] string schema = null) => _builder.DropTable(name, schema);

        /// <summary>
        /// 删除唯一键。
        /// </summary>
        /// <param name="columns">列表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropUniqueConstraintOperation> DropUniqueConstraint(
                [NotNull] Expression<Func<TEntity, object>> columns)
            => _builder.DropUniqueConstraint(columns);

        /// <summary>
        /// 删除唯一键。
        /// </summary>
        /// <param name="name">名称（全名包含UK_）。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<DropUniqueConstraintOperation> DropUniqueConstraint([NotNull] string name)
            => _builder.DropUniqueConstraint<TEntity>(name);

        /// <summary>
        /// 修改列名。
        /// </summary>
        /// <param name="name">原名称。</param>
        /// <param name="column">新列名表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RenameColumnOperation> RenameColumn(
            [NotNull] string name,
            [NotNull] Expression<Func<TEntity, object>> column) => _builder.RenameColumn(name, column);

        /// <summary>
        /// 修改列名。
        /// </summary>
        /// <param name="name">原名称。</param>
        /// <param name="newName">新列名。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RenameColumnOperation> RenameColumn(
            [NotNull] string name,
            [NotNull] string newName) => _builder.RenameColumn<TEntity>(name, newName);

        /// <summary>
        /// 修改列名。
        /// </summary>
        /// <param name="name">原名称（全名包含IX_）。</param>
        /// <param name="columns">新列名表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RenameIndexOperation> RenameIndex(
            [NotNull] string name,
            [NotNull] Expression<Func<TEntity, object>> columns) => _builder.RenameIndex(name, columns);

        /// <summary>
        /// 修改序列号名称。
        /// </summary>
        /// <param name="name">原名称。</param>
        /// <param name="schema">原架构。</param>
        /// <param name="newName">新名称。</param>
        /// <param name="newSchema">新架构。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RenameSequenceOperation> RenameSequence(
            [NotNull] string name,
            [CanBeNull] string schema = null,
            [CanBeNull] string newName = null,
            [CanBeNull] string newSchema = null) => _builder.RenameSequence(name, schema, newName, newSchema);

        /// <summary>
        /// 修改表名称。
        /// </summary>
        /// <param name="name">原名称。</param>
        /// <param name="schema">原架构。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RenameTableOperation> RenameTable(
            [NotNull] string name,
            [CanBeNull] string schema = null) => _builder.RenameTable<TEntity>(name, schema);

        /// <summary>
        /// 重新计算序列号。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="startValue">开始值。</param>
        /// <param name="schema">架构。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<RestartSequenceOperation> RestartSequence(
            [NotNull] string name,
            long startValue = 1L,
            [CanBeNull] string schema = null) => _builder.RestartSequence(name, startValue, schema);

        /// <summary>
        /// SQL语句。
        /// </summary>
        /// <param name="sql">SQL语句。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> Sql(
            [NotNull] string sql) => _builder.Sql(sql);

        /// <summary>
        /// 新建语句。
        /// </summary>
        /// <param name="instance">对象实例。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> SqlCreate([NotNull] TEntity instance)
            => _builder.SqlCreate(instance);

        /// <summary>
        /// 新建语句。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="instance">对象实例。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> SqlCreate<TModel>([NotNull] TModel instance)
            => _builder.SqlCreate(instance);

        /// <summary>
        /// 更新语句。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="instance">匿名对象。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> SqlUpdate(
                [NotNull] Expression<Predicate<TEntity>> expression, [NotNull] object instance)
            => _builder.SqlUpdate(expression, instance);

        /// <summary>
        /// 更新语句。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="expression">条件表达式。</param>
        /// <param name="instance">匿名对象。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> SqlUpdate<TModel>(
                [NotNull] Expression<Predicate<TModel>> expression, [NotNull] object instance)
            => _builder.SqlUpdate(expression, instance);

        /// <summary>
        /// 删除语句。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> SqlDelete(
                [NotNull] Expression<Predicate<TEntity>> expression)
            => _builder.SqlDelete(expression);

        /// <summary>
        /// 删除语句。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回迁移实例。</returns>
        public virtual OperationBuilder<SqlOperation> SqlDelete<TModel>(
                [NotNull] Expression<Predicate<TModel>> expression)
            => _builder.SqlDelete(expression);
    }
}