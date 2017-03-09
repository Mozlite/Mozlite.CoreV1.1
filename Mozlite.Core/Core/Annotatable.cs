using System;
using System.Collections.Generic;
using System.Linq;
namespace Mozlite.Core
{
    /// <summary>
    /// 所有可扩展对象的基类。
    /// </summary>
    public class Annotatable : IMutableAnnotatable
    {
        private readonly Lazy<SortedDictionary<string, Annotation>> _annotations =
            new Lazy<SortedDictionary<string, Annotation>>(() => new SortedDictionary<string, Annotation>());

        /// <summary>
        /// 获取所有扩展实例列表。
        /// </summary>
        /// <returns>所有扩展实例列表。</returns>
        public virtual IEnumerable<Annotation> GetAnnotations() =>
            _annotations.IsValueCreated
                ? _annotations.Value.Values
                : Enumerable.Empty<Annotation>();

        /// <summary>
        /// 获取扩展对象实例值。
        /// </summary>
        /// <typeparam name="TValue">值类型。</typeparam>
        /// <param name="name">扩展名称。</param>
        /// <returns>返回值实例对象。</returns>
        public virtual TValue GetAnnotation<TValue>(string name)
        {
            var value =this[name];
            if (value == null)
                return default(TValue);
            return (TValue) value;
        }

        /// <summary>
        /// 添加一个扩展到对象中，如果已经存在扩展名称将抛出错误。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <param name="value">值。</param>
        /// <returns>返回当前添加的扩展实例。</returns>
        public virtual Annotation AddAnnotation(string name, object value)
        {
            Check.NotEmpty(name, nameof(name));
            var annotation = CreateAnnotation(name, value);
            return AddAnnotation(name, annotation);
        }

        /// <summary>
        /// 添加一个扩展到对象中，如果已经存在扩展名称将抛出错误。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <param name="annotation">扩展实例。</param>
        /// <returns>返回当前添加的扩展实例。</returns>
        protected virtual Annotation AddAnnotation([NotNull] string name, [NotNull] Annotation annotation)
        {
            var previousLength = _annotations.Value.Count;
            SetAnnotation(name, annotation);

            if (previousLength == _annotations.Value.Count)
            {
                throw new InvalidOperationException(string.Format(Resources.DuplicateAnnotation, name));
            }

            return annotation;
        }

        /// <summary>
        /// 设置名称为<paramref name="name"/>的扩展实例。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <param name="annotation">扩展实例。</param>
        /// <returns>返回扩展实例。</returns>
        protected virtual Annotation SetAnnotation([NotNull] string name, [NotNull] Annotation annotation)
        {
            var oldAnnotation = FindAnnotation(name);

            _annotations.Value[name] = annotation;

            return OnAnnotationSet(name, annotation, oldAnnotation);
        }

        /// <summary>
        /// 设置扩展实例后执行的方法。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <param name="annotation">扩展实例。</param>
        /// <param name="oldAnnotation">原来存储在对象中的实例。</param>
        /// <returns>返回扩展实例。</returns>
        protected virtual Annotation OnAnnotationSet(
                [NotNull] string name, [CanBeNull] Annotation annotation, [CanBeNull] Annotation oldAnnotation)
            => annotation;

        /// <summary>
        /// 获取或存储扩展实例，如果已经存在将获取已经存在的实例，否则将保存<paramref name="value"/>对象到实例中并返回。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <param name="value">扩展值代理方法。</param>
        /// <returns>返回扩展实例对象。</returns>
        public virtual Annotation GetOrAddAnnotation([NotNull] string name, [NotNull] Func<object> value)
            => FindAnnotation(name) ?? AddAnnotation(name, value());
        
        /// <summary>
        /// 从对象中查找一个名称为<paramref name="name"/>的扩展实例。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <returns>返回得到的扩展实例，如果不存在返回null。</returns>
        public virtual Annotation FindAnnotation(string name)
        {
            Check.NotEmpty(name, nameof(name));

            if (!_annotations.IsValueCreated)
            {
                return null;
            }

            Annotation annotation;
            return _annotations.Value.TryGetValue(name, out annotation)
                ? annotation
                : null;
        }

        /// <summary>
        /// 删除对象中的扩展实例。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <returns>返回被删除的扩展实例。</returns>
        public virtual Annotation RemoveAnnotation(string name)
        {
            Check.NotNull(name, nameof(name));

            var annotation = FindAnnotation(name);
            if (annotation == null)
            {
                return null;
            }

            _annotations.Value.Remove(name);

            OnAnnotationSet(name, null, annotation);

            return annotation;
        }

        /// <summary>
        /// 获取或设置扩展实例。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <returns>返回扩展实例值。</returns>
        public virtual object this[[NotNull] string name]
        {
            get { return FindAnnotation(name)?.Value; }
            [param: CanBeNull]
            set
            {
                Check.NotEmpty(name, nameof(name));

                if (value == null)
                {
                    RemoveAnnotation(name);
                }
                else
                {
                    SetAnnotation(name, CreateAnnotation(name, value));
                }
            }
        }

        /// <summary>
        /// 新建一个扩展实例。
        /// </summary>
        /// <param name="name">扩展名称。</param>
        /// <param name="value">值对象。</param>
        /// <returns>返回一个扩展实例。</returns>
        protected virtual Annotation CreateAnnotation([NotNull] string name, [NotNull] object value)
            => new Annotation(name, value);

        IEnumerable<IAnnotation> IAnnotatable.GetAnnotations() => GetAnnotations();

        IAnnotation IAnnotatable.FindAnnotation(string name) => FindAnnotation(name);
    }
}