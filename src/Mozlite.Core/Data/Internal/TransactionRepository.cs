using Microsoft.Extensions.Logging;
using Mozlite.Data.Metadata;
using Mozlite.Data.Query;

namespace Mozlite.Data.Internal
{
    /// <summary>
    /// 数据库事务操作实现类。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public class TransactionRepository<TModel> : RepositoryBase<TModel>, ITransactionRepository<TModel>
    {
        private readonly IExecutor _executor;
        private readonly IModel _model;

        /// <summary>
        /// 获取其他模型表格操作实例。
        /// </summary>
        /// <typeparam name="TOther">其他模型类型。</typeparam>
        /// <returns>返回当前事务的模型数据库操作实例。</returns>
        public ITransactionRepository<TOther> As<TOther>()
        {
            return new TransactionRepository<TOther>(_executor, Logger, _model, _model.GetEntity(typeof(TOther)), SqlHelper, SqlGenerator);
        }

        /// <summary>
        /// 初始化类<see cref="RepositoryBase{TModel}"/>。
        /// </summary>
        /// <param name="executor">数据库执行接口。</param>
        /// <param name="logger">日志接口。</param>
        /// <param name="model">模型接口。</param>
        /// <param name="entityType">模型实例接口。</param>
        /// <param name="sqlHelper">SQL辅助接口。</param>
        /// <param name="sqlGenerator">脚本生成器。</param>
        public TransactionRepository(IExecutor executor, ILogger logger, IModel model, IEntityType entityType, ISqlHelper sqlHelper, ISqlGenerator sqlGenerator)
            : base(executor, logger, entityType, sqlHelper, sqlGenerator)
        {
            _executor = executor;
            _model = model;
        }
    }
}