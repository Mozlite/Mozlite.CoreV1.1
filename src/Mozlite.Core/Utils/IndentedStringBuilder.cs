﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mozlite.Utils
{
    /// <summary>
    /// 缩进字符串组合类。
    /// </summary>
    public class IndentedStringBuilder
    {
        private const byte IndentSize = 4;

        private byte _indent;
        private bool _indentPending = true;

        private readonly StringBuilder _stringBuilder = new StringBuilder();
        /// <summary>
        /// 初始化类<see cref="IndentedStringBuilder"/>。
        /// </summary>
        public IndentedStringBuilder()
        {
        }

        /// <summary>
        /// 初始化类<see cref="IndentedStringBuilder"/>。
        /// </summary>
        /// <param name="from">从当前实例再进行缩进。</param>
        public IndentedStringBuilder(IndentedStringBuilder from)
        {
            _indent = from._indent;
        }
        
        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="o">当前实例。</param>
        /// <param name="seperator">分隔符。</param>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder JoinAppend<T>(IEnumerable<T> o, string seperator = ",")
        {
            DoIndent();

            _stringBuilder.Append(string.Join(seperator, o));

            return this;
        }

        /// <summary>
        /// 添加实例，如果<paramref name="o"/>为空则不进行任何操作。
        /// </summary>
        /// <param name="o">当前实例。</param>
        /// <param name="format">格式化字符串。</param>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder AppendEx([NotNull]object o, string format)
        {
            if (string.IsNullOrWhiteSpace(o?.ToString()))
                return this;

            DoIndent();

            _stringBuilder.AppendFormat(format, o);

            return this;
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="o">当前实例。</param>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder Append(object o)
        {
            DoIndent();

            _stringBuilder.Append(o);

            return this;
        }

        /// <summary>
        /// 添加空行。
        /// </summary>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder AppendLine()
        {
            AppendLine(string.Empty);

            return this;
        }

        /// <summary>
        /// 缩进添加行实例。
        /// </summary>
        /// <param name="o">当前实例。</param>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder AppendLine(object o)
        {
            var value = o.ToString();

            if (value != string.Empty)
            {
                DoIndent();
            }

            _stringBuilder.AppendLine(value);

            _indentPending = true;

            return this;
        }

        /// <summary>
        /// 增加缩进。
        /// </summary>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder IncrementIndent()
        {
            _indent++;
            return this;
        }

        /// <summary>
        /// 减少缩进。
        /// </summary>
        /// <returns>返回缩进实例对象。</returns>
        public virtual IndentedStringBuilder DecrementIndent()
        {
            if (_indent > 0)
            {
                _indent--;
            }
            return this;
        }

        /// <summary>
        /// 增加缩进块。
        /// </summary>
        /// <returns>返回当前块实例，使用using将自动释放。</returns>
        public virtual IDisposable Indent() => new Indenter(this);

        /// <summary>
        /// 获取当前字符串实例。
        /// </summary>
        /// <returns>返回当前字符串内容实例。</returns>
        public override string ToString() => _stringBuilder.ToString();

        private void DoIndent()
        {
            if (_indentPending && _indent > 0)
            {
                _stringBuilder.Append(new string(' ', _indent * IndentSize));
            }

            _indentPending = false;
        }

        private sealed class Indenter : IDisposable
        {
            private readonly IndentedStringBuilder _stringBuilder;

            public Indenter(IndentedStringBuilder stringBuilder)
            {
                _stringBuilder = stringBuilder;

                _stringBuilder.IncrementIndent();
            }

            public void Dispose() => _stringBuilder.DecrementIndent();
        }

        /// <summary>
        /// 清空内容。
        /// </summary>
        protected void Clear() => _stringBuilder.Clear();

        /// <summary>
        /// 判断是否为空。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        protected bool IsEmpty() => _stringBuilder.Length == 0;
    }
}