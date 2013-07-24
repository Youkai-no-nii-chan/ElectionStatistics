using System;
using System.IO;

namespace ElectionInfo.ManagementConsole
{
    internal static class TextReadParseExtensions
    {
        public static bool MoveTo(this TextReader reader, string value)
        {
            string latestLine;
            return MoveTo(reader, value, out latestLine);
        }

        public static bool MoveTo(this TextReader reader, string value, out string latestLine)
        {
            do
            {
                latestLine = reader.ReadLine();
                if (latestLine == null)
                {
                    return false;
                }

                if (latestLine.Contains(value))
                {
                    return true;
                }
            }
            while (true);
        }

        public static bool MoveTo(this TextReader reader, string value, string end)
        {
            string latestLine;
            return MoveTo(reader, value, end, out latestLine);
        }

        public static bool MoveTo(this TextReader reader, string value, string end, out string latestLine)
        {
            do
            {
                latestLine = reader.ReadLine();
                if (latestLine == null)
                {
                    throw new Exception(string.Format("Значения \"{0}\" и \"{1}\"  в потоке не найдено.", value, end));
                }

                if (latestLine.Contains(value))
                {
                    return true;
                }

                if (latestLine.Contains(end))
                {
                    return false;
                }
            }
            while (true);
        }

        public static byte MoveTo(
            this TextReader reader, string value1, string value2, string value3, out string latestLine)
        {
            do
            {
                latestLine = reader.ReadLine();
                if (latestLine == null)
                {
                    throw new Exception(
                        string.Format(
                            "Значений \"{0}\",  \"{1}\" и \"{2}\"  в потоке не найдено.", value1, value2, value3));
                }

                if (latestLine.Contains(value1))
                {
                    return 1;
                }

                if (latestLine.Contains(value2))
                {
                    return 2;
                }

                if (latestLine.Contains(value3))
                {
                    return 3;
                }
            }
            while (true);
        }
    }
}