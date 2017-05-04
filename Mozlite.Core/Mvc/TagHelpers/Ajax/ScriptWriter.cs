using System.Text;

namespace Mozlite.Mvc.TagHelpers.Ajax
{
    /// <summary>
    /// HTML辅助类。
    /// </summary>
    public class ScriptWriter
    {
        private readonly StringBuilder _sb;
        /// <summary>
        /// 初始化类<see cref="ScriptWriter"/>。
        /// </summary>
        public ScriptWriter()
        {
            _sb = new StringBuilder();
        }

        /// <summary>
        /// 写入代码。
        /// </summary>
        /// <param name="code">代码。</param>
        /// <returns>返回当前辅助类实例。</returns>
        public ScriptWriter Write(string code)
        {
            _sb.Append(code);
            return this;
        }

        /// <summary>
        /// 写入HTML代码。
        /// </summary>
        /// <param name="code">HTML代码。</param>
        /// <returns>返回当前辅助类实例。</returns>
        public ScriptWriter Html(string code)
        {
            code = code?.Trim().Replace("`", "\\`");
            return Write("_s.push(`")
                .Write(code)
                .Write("`);");
        }

        /// <summary>
        /// 写入代码变量。
        /// </summary>
        /// <param name="code">字符串。</param>
        /// <returns>返回当前辅助类实例。</returns>
        public ScriptWriter Code(string code)
        {
            return Write("_s.push(").Write(code).Write(");");
        }

        /// <summary>
        /// 写入字符串。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <returns>返回当前辅助类实例。</returns>
        public ScriptWriter Quote(string text)
        {
            return Write("'").Write(text?.Replace("'", "\\'")).Write("'");
        }

        /// <summary>返回表示当前对象的字符串。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}