using System;
using System.IO;
using System.Net;
using System.Text;
using ElectionInfo.Model;
using System.Linq;

namespace ElectionInfo.ManagementConsole
{
    public class DownloadElectionResultsCommand : Command
    {
        private StreamWriter logFile;
        private WebClient webClient;
        private ParsersFactory parsersFactory;
        private ModelContext context;
        private int[] skipSubUrlsCounts;

        public override string Name
        {
            get { return "Download-ElectionResults"; }
        }

        public override void Execute(string[] arguments)
        {
            if (arguments.Length < 1)
                throw new ArgumentException("arguments.Length < 1");
            
            var uri = arguments[0];

            if (arguments.Length >= 3 && arguments[1] == "skip")
            {
                skipSubUrlsCounts = arguments.Skip(2).Select(int.Parse).ToArray();
            }

            Console.WriteLine("Начата обработка");

            webClient = new WebClient();
            parsersFactory = new ParsersFactory();

            using (logFile = new StreamWriter("log.txt", true, Encoding.GetEncoding(1251)))
            {
                context = new ModelContext();
                try
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    DownloadAndParse(uri, 0);
                }
                finally
                {
                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.Dispose();
                }
            }

            Console.WriteLine("Завершена обработка всех дочерних uri");
        }

        private void DownloadAndParse(string url, int level, IParser parentParser = null)
        {
            WriteLineToLog("Начата обработка: {0}", url);
            IParser parser;
            using (var stream = webClient.OpenRead(url))
            {
                if (stream == null)
                {
                    throw new Exception(string.Format("По адресу \"{0}\" отсутствуют данные.", url));
                }
                var reader = new StreamReader(stream, Encoding.GetEncoding(1251));
                parser = parsersFactory.CreateParser(url, reader, context, parentParser);
                parser.Parse();
            }

            int skipSubUrlsCount = 0;
            if (skipSubUrlsCounts.Length > level)
            {
                skipSubUrlsCount = skipSubUrlsCounts[level];
                skipSubUrlsCounts[level] = 0;
            }

            int subUrlNumber = 1 + skipSubUrlsCount;
            foreach (string subUrl in parser.SubUrls.Skip(skipSubUrlsCount))
            {
                if (parser.SubUrls.Count > 1)
                {
                    WriteLineToLog("Level {0}, Index {1}", level, subUrlNumber);
                }

                DownloadAndParse(subUrl, level + 1, parser);

                subUrlNumber++;
            }
            WriteLineToLog("Завершена обработка: {0}", url);
        }

        private void WriteLineToLog(string format, params object[] arguments)
        {
            logFile.WriteLine(format, arguments);
            logFile.Flush();
        }
    }
}