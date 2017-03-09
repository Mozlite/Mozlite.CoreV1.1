using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mozlite.Data;
using Mozlite.Data.Metadata;
using Mozlite.Data.Query;

namespace Mozlite.Extensions.Searching
{
    /// <summary>
    /// 检索管理器。
    /// </summary>
    public class SearchManager : ISearchManager
    {
        private readonly IModel _model;
        private readonly IRepository<SearchIndex> _indexes;
        private readonly IRepository<SearchDescriptor> _searches;
        private readonly ISqlGenerator _generator;
        private readonly ISqlHelper _sqlHelper;
        private readonly ConcurrentDictionary<Type, string> _scripts = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// 初始化类<see cref="SearchManager"/>。
        /// </summary>
        /// <param name="model">模型管理接口。</param>
        /// <param name="indexes">索引数据库操作接口。</param>
        /// <param name="searches">搜索实体数据库操作接口。</param>
        /// <param name="generator">SQL生成器。</param>
        /// <param name="sqlHelper">SQL辅助接口。</param>
        public SearchManager(IModel model, IRepository<SearchIndex> indexes, IRepository<SearchDescriptor> searches, ISqlGenerator generator, ISqlHelper sqlHelper)
        {
            _model = model;
            _indexes = indexes;
            _searches = searches;
            _generator = generator;
            _sqlHelper = sqlHelper;
        }

        /// <inheritdoc />
        public IEntityType GetSearchEntity(Type type)
        {
            return _model.GetEntity(type);
        }

        /// <inheritdoc />
        public async Task<SearchEntry> GetIndexAsync(IEntityType entityType)
        {
            var sql = _generator.SearchIndex(entityType);
            using (var reader = await _indexes.ExecuteReaderAsync(sql))
            {
                if (await reader.ReadAsync())
                    return new SearchEntry(reader);
            }
            return null;
        }

        /// <inheritdoc />
        public async Task<bool> SaveAsync(IEntityType entityType, SearchDescriptor search, IEnumerable<string> indexes)
        {
            //防止重复，空值等等
            indexes = indexes.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            return await _searches.BeginTransactionAsync(async db =>
            {
                var dbSearch = await db.FindAsync(s => s.ProviderName == search.ProviderName && s.TargetId == search.TargetId);
                if (dbSearch != null)
                {
                    //重新生成的索引描述信息
                    if (!await db.UpdateAsync(s => s.Id == dbSearch.Id, new { search.IndexedDate, search.Summary }))
                        return false;
                    search.Id = dbSearch.Id;
                }
                else if (!await db.CreateAsync(search))
                    return false;

                //删除关联表
                var indb = db.As<SearchInIndex>();
                if (dbSearch != null)
                    await indb.DeleteAsync(si => si.SearchId == dbSearch.Id);

                //添加关键词和关联表
                var indexdb = db.As<SearchIndex>();
                foreach (var index in indexes)
                {
                    if (string.IsNullOrWhiteSpace(index))
                        continue;
                    //添加关键词
                    var entry = await indexdb.FindAsync(x => x.Name == index);
                    if (entry == null)
                    {
                        entry = new SearchIndex { Name = index };
                        if (!await indexdb.CreateAsync(entry))
                            return false;
                    }
                    //添加关联表
                    if (!await indb.CreateAsync(new SearchInIndex { IndexId = entry.Id, SearchId = search.Id }))
                        return false;
                }

                //更新到实体表
                var script = _scripts.GetOrAdd(entityType.ClrType, _ => _generator.SetSearchIndexed(entityType));
                if (await db.ExecuteNonQueryAsync(script, new { Id = search.TargetId }))
                    return true;
                return false;
            }, 300);
        }

        /// <inheritdoc />
        public virtual async Task<SearchQuery> LoadAsync(SearchQuery query)
        {
            if (query.Page <= 1)
                await _indexes.IncreaseByAsync(si => si.Name == query.Q, s => s.Priority, 1);
            return await _searches.LoadAsync(query);
        }

        /// <summary>
        /// 提示搜索列。
        /// </summary>
        /// <param name="name">搜索字符串。</param>
        /// <returns>返回检索的关键词列表。</returns>
        public Task<IEnumerable<SearchIndex>> LoadSuggestions(string name)
        {
            return _indexes.AsQueryable()
                .Where(x => x.Name.StartsWith(name))
                .AsEnumerableAsync(10);
        }
    }
}