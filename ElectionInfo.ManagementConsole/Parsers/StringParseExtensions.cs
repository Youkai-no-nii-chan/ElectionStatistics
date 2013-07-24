namespace ElectionInfo.ManagementConsole
{
    internal static class StringParseExtensions
    {
        internal static string GetAttribute(this string value, string attributeName)
        {
            int start = value.IndexOf(attributeName + "=\"");
            if (start == -1)
            {
                return null;
            }
            start += attributeName.Length + 2;
            int end = value.IndexOf('\"', start);
            if (end == -1)
            {
                return null;
            }
            return value.Substring(start, end - start).Replace("&amp;", "&");
        }

        internal static string GetValueAttribute(this string source)
        {
            return GetAttribute(source, "value");
        }

        internal static string GetHrefAttribute(this string value)
        {
            return GetAttribute(value, "href");
        }

        internal static string GetTagValue(this string source, string tagName)
        {
            int start = source.IndexOf("<" + tagName);
            if (start == -1)
            {
                return null;
            }
            start = source.IndexOf(">", start);
            if (start == -1)
            {
                return null;
            }
            start++;

            int end = source.IndexOf("</" + tagName + ">", start);
            if (end == -1)
            {
                return null;
            }
            return source.Substring(start, end - start);
        }

        internal static int CountRepeats(this string source, string repeatString)
        {
            int repeats = -1;
            int start = 0;
            do
            {
                start = source.IndexOf(repeatString, start + repeatString.Length);
                repeats++;
            }
            while (start != -1);
            return repeats;
        }
    }
}