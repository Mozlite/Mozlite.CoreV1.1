using System;
using System.Collections.Generic;

namespace Mozlite.Extensions.Searching
{
    /// <summary>
    /// 索引实例提供者基类。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public abstract class SearchIndexProvider<TModel> : ISearchIndexProvider
        where TModel : class, ISearchable, new()
    {
        /// <inheritdoc />
        public Type Model => typeof(TModel);

        /// <inheritdoc />
        public virtual string ProviderName => Model.FullName;

        /// <inheritdoc />
        public abstract string Summarized(SearchEntry entry);

        /// <inheritdoc />
        public abstract IEnumerable<string> Indexed(SearchEntry entry);
    }
}