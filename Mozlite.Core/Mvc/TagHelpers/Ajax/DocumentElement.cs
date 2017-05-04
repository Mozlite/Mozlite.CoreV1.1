using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Mozlite.Mvc.TagHelpers.Ajax
{
    /// <summary>
    /// 根结点。
    /// </summary>
    public class DocumentElement : Element
    {
        private static readonly Regex _regex = new Regex("<script.*?>(.*?)</script>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private readonly List<string> _scripts = new List<string>();
        private readonly string _source;
        private int _index;
        private readonly int _maxIndex;
        /// <summary>
        /// 初始化类<see cref="DocumentElement"/>。
        /// </summary>
        /// <param name="source">源代码。</param>
        public DocumentElement(string source)
            : base(source, ElementType.Doc)
        {
            //提取脚本
            source = _regex.Replace(source, match =>
            {
                _scripts.Add(match.Groups[1].Value.Trim().Trim(';'));
                return null;
            });
            _source = source;
            _maxIndex = _source.Length - 1;
            Read(this);
        }

        private char Current => _source[_index];

        private char? Next
        {
            get
            {
                if (_source.Length > _index + 1)
                    return _source[_index + 1];
                return null;
            }
        }

        /// <summary>
        /// 加载代码。
        /// </summary>
        private void Read(Element element)
        {
            while (_maxIndex > _index)
            {
                if (Current == '{' && Next == '{')
                {
                    if (ReadCodeBlock(element))
                        break;
                }
                else
                    ReadTextBlock(element);
            }
        }

        /// <summary>
        /// 读取HTML代码。
        /// </summary>
        /// <param name="parent">父级节点。</param>
        private void ReadTextBlock(Element parent)
        {
            var sb = new StringBuilder();
            while (_maxIndex > _index)
            {
                if (Current == '{' && Next == '{')
                    break;
                sb.Append(Current);
                _index++;
            }
            parent.Add(new TextElement(sb.ToString()));
        }

        /// <summary>
        /// 读取引用字符串。
        /// </summary>
        private StringBuilder ReadQuote(char quote)
        {
            /*
                xx == 'sdf{{sdfsdf';
                xx == 'sdf\'sd{{}}fsdf';
                xx == 'sdfsd{{}}fsdf\\';
             */
            var sb = new StringBuilder();
            sb.Append(quote);
            _index++;
            while (_maxIndex > _index)
            {
                if (quote == Current)
                    break;
                if (Current == '\\')//转义符号
                {
                    sb.Append(Current);
                    _index++;
                    continue;
                }
                sb.Append(Current);
                _index++;
            }
            sb.Append(quote);
            _index++;
            return sb;
        }

        /// <summary>
        /// 读取代码片段。
        /// </summary>
        private string ReadCode()
        {
            var sb = new StringBuilder();
            _index += 2;//去掉{{
            while (_maxIndex > _index)
            {
                if (Current == '\'' || Current == '"')
                {
                    sb.Append(ReadQuote(Current));
                    continue;
                }
                if (Current == '}' && Next == '}')
                    break;
                sb.Append(Current);
                _index++;
            }
            _index += 2;//去掉}}
            return sb.ToString();
        }

        /// <summary>
        /// 读取代码块。
        /// </summary>
        /// <param name="parent">父级实例。</param>
        /// <returns>如果结束读取返回<c>true</c>。</returns>
        private bool ReadCodeBlock(Element parent)
        {
            var code = ReadCode().Trim();
            if (code.Length == 0)
                return true;
            if (code[0] == '/')
            {//结束代码块
                code = code.Substring(1).Trim();
                if (parent is CodeBlockElement block && block.Key != code)
                    throw new Exception($"{code}语法错误，结束符必须成对嵌套使用！");
                return true;
            }
            var index = code.IndexOf(' ');
            if (index == -1)
            {
                parent.Add(new CodeElement(code));
                return false;
            }
            var key = code.Substring(0, index);
            code = code.Substring(index).Trim(' ', '(', ')');
            var current = new CodeBlockElement(key, code);
            parent.Add(current);
            Read(current);
            return false;
        }

        /// <summary>
        /// 获取脚本。
        /// </summary>
        public List<string> Scripts => _scripts;
    }
}