using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Mozlite.Data.Metadata;
using Newtonsoft.Json;

namespace Mozlite.Extensions
{
    /// <summary>
    /// 扩展基类。
    /// </summary>
    public abstract class ExtendBase : IEnumerable<string>
    {
        private IDictionary<string, string> _extendProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 扩展方法。
        /// </summary>
        [Ignore(Ignore.List)]
        public string ExtendProperties
        {
            get { return JsonConvert.SerializeObject(_extendProperties); }
            set { _extendProperties = JsonConvert.DeserializeObject<Dictionary<string, string>>(value); }
        }

        /// <summary>
        /// 索引访问和设置扩展属性。
        /// </summary>
        /// <param name="name">索引值。</param>
        /// <returns>返回当前扩展属性值。</returns>
        [Ignore(Ignore.All)]
        public string this[string name]
        {
            get
            {
                if (!name.StartsWith("ex:"))
                    name = "ex:" + name;
                string value;
                _extendProperties.TryGetValue(name, out value);
                return value;
            }
            set
            {
                if (!name.StartsWith("ex:"))
                    name = "ex:" + name;
                _extendProperties[name] = value;
            }
        }

        /// <summary>返回一个循环访问集合的枚举器。</summary>
        /// <returns>用于循环访问集合的枚举数。</returns>
        public IEnumerator<string> GetEnumerator()
        {
            return _extendProperties.Keys.GetEnumerator();
        }

        /// <summary>返回循环访问集合的枚举数。</summary>
        /// <returns>可用于循环访问集合的 <see cref="T:System.Collections.IEnumerator" /> 对象。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 自增长唯一Id，主键。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 从表单中读取扩展属性对象。
        /// </summary>
        /// <param name="request">HTTP请求。</param>
        public void ReadFrom(HttpRequest request)
        {
            foreach (var key in request.Form.Keys)
            {
                if (key.StartsWith("ex:", StringComparison.OrdinalIgnoreCase))
                    _extendProperties[key] = request.Form[key];
            }
        }
    }
}