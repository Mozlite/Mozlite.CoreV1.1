using System.Collections.Generic;
using Mozlite.Data.Metadata;
using Mozlite.Utils;

namespace Mozlite.Data.Query
{
    /// <summary>
    /// SQL脚本。
    /// </summary>
    public class SqlScript
    {
        private readonly IndentedStringBuilder _sql;
        private readonly IEnumerable<string> _parameterNames;
        private readonly IEntityType _entityType;

        /// <summary>
        /// 初始化类<see cref="SqlScript"/>。
        /// </summary>
        /// <param name="sql">脚本字符串。</param>
        /// <param name="parameterNames">参数名称。</param>
        /// <param name="entityType">当前实体。</param>
        public SqlScript(IndentedStringBuilder sql, IEnumerable<string> parameterNames, IEntityType entityType)
        {
            _sql = sql;
            _parameterNames = parameterNames;
            _entityType = entityType;
        }

        /// <summary>
        /// 初始化类<see cref="SqlScript"/>。
        /// </summary>
        /// <param name="sql">脚本字符串。</param>
        /// <param name="parameters">参数列表。</param>
        public SqlScript(IndentedStringBuilder sql, IDictionary<string, object> parameters = null)
        {
            _sql = sql;
            Parameters = parameters;
        }

        /// <summary>
        /// 参数。
        /// </summary>
        public IDictionary<string, object> Parameters { get; }

        /// <summary>
        /// 生成参数对象。
        /// </summary>
        public IDictionary<string, object> CreateParameters(object instance)
        {
            var parameters = new Dictionary<string, object>();
            foreach (var parameterName in _parameterNames)
            {
                parameters.Add(parameterName, _entityType.FindProperty(parameterName).Get(instance));
            }
            return parameters;
        }

        /// <summary>返回表示当前对象的字符串。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            return _sql.ToString();
        }
    }
}