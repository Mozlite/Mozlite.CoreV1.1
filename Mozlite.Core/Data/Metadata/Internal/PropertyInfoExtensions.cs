using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Mozlite.Core;

namespace Mozlite.Data.Metadata.Internal
{
    [DebuggerStepThrough]
    internal static class PropertyInfoExtensions
    {
        public static PropertyInfo FindGetterProperty([NotNull] this PropertyInfo propertyInfo)
            => propertyInfo.DeclaringType
                .GetPropertiesInHierarchy(propertyInfo.Name)
                .FirstOrDefault(p => p.GetMethod != null);

        public static PropertyInfo FindSetterProperty([NotNull] this PropertyInfo propertyInfo)
            => propertyInfo.DeclaringType
                .GetPropertiesInHierarchy(propertyInfo.Name)
                .FirstOrDefault(p => p.SetMethod != null);
    }
}