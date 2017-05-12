using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mozlite.Mvc.TagHelpers.Binders
{
    /// <summary>
    /// 节点实例。
    /// </summary>
    public abstract class Element : ElementBase, IEnumerable<ElementBase>
    {
        /// <summary>
        /// 添加子节点。
        /// </summary>
        /// <param name="element">节点实例对象。</param>
        public void Add(ElementBase element)
        {
            element.Parent = this;
            _children.Add(element);
        }

        private readonly List<ElementBase> _children = new List<ElementBase>();

        /// <summary>返回一个循环访问集合的枚举器。</summary>
        /// <returns>用于循环访问集合的枚举数。</returns>
        public IEnumerator<ElementBase> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        /// <summary>返回循环访问集合的枚举数。</summary>
        /// <returns>可用于循环访问集合的 <see cref="T:System.Collections.IEnumerator" /> 对象。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>返回表示当前对象的字符串。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var element in this)
            {
                sb.Append(element);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 初始化类<see cref="Element"/>。
        /// </summary>
        /// <param name="source">源代码。</param>
        /// <param name="type">类型。</param>
        protected Element(string source, ElementType type) : base(source, type)
        {
        }
    }
}