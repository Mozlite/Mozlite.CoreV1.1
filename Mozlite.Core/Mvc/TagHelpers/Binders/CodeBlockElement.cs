using System.Text;

namespace Mozlite.Mvc.TagHelpers.Binders
{
    /// <summary>
    /// 代码节点。
    /// </summary>
    public class CodeBlockElement : Element
    {
        /// <summary>
        /// 代码关键字。
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 条件表达式。
        /// </summary>
        public string Condition { get; }

        /// <summary>
        /// 初始化类<see cref="CodeBlockElement"/>。
        /// </summary>
        /// <param name="key">代码关键字。</param>
        /// <param name="condition">条件表达式。</param>
        public CodeBlockElement(string key, string condition) : base(null, ElementType.Block)
        {
            Key = key;
            Condition = condition;
        }

        /// <summary>返回表示当前对象的字符串。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            var sb=new StringBuilder();
            sb.Append("{{").Append(Key).Append(" ").Append(Condition).Append("}}");
            sb.Append(base.ToString());
            sb.Append("{{/").Append(Key).Append("}}");
            return sb.ToString();
        }
    }
}