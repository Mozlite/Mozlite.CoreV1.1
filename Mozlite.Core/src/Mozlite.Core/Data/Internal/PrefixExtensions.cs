namespace Mozlite.Data.Internal
{
    internal static class PrefixExtensions
    {
        internal static string ReplacePrefix(this string commandText, string prefix)
        {
            return commandText.Replace("$pre:$", string.Empty).Replace("$pre:", prefix);
        }

        internal static string EscapePrefix(this string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix) && !prefix.EndsWith("_"))
                prefix += "_";
            return prefix;
        }
    }
}