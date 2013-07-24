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

        public override string Name
        {
            get { return "Download-ElectionResults"; }
        }

        public override void Execute(string[] arguments)
        {
            if (arguments.Length < 1)
                throw new ArgumentException("arguments.Length < 1");

            int skipSubUrls = 0;
            int skipSubSubUrls = 0;
            if (arguments.Length >= 3 && arguments[1] == "skip")
            {
                skipSubUrls = int.Parse(arguments[2]);
                if (arguments.Length > 3)
                {
                    skipSubSubUrls = int.Parse(arguments[3]);
                }
            }

            var uri = arguments[0];

            Console.WriteLine("Начата обработка");

            webClient = new WebClient();
            parsersFactory = new ParsersFactory();

            using (logFile = new StreamWriter("log.txt", true, Encoding.GetEncoding(1251)))
            {
                using (context = new ModelContext())
                {
                    try
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        DownloadAndParse(uri, "SubUrlNumber", null, skipSubUrls, skipSubSubUrls);
                    }
                    finally
                    {
                        context.Configuration.AutoDetectChangesEnabled = true;
                    }
                }
            }

            Console.WriteLine("Завершена обработка всех дочерних uri");
        }

        private void DownloadAndParse(string url, string subUrlLogName, IParser parentParser = null, int skipSubUrls = 0, int skipSubSubUrls = 0)
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
            int subUrlNumber = 1 + skipSubUrls;
            foreach (string subUrl in parser.SubUrls.Skip(skipSubUrls))
            {
                if (parser.SubUrls.Count > 1)
                {
                    WriteLineToLog("{0} = {1}", subUrlLogName, subUrlNumber);
                }

                DownloadAndParse(subUrl, "Sub" + subUrlLogName, parser, skipSubSubUrls);

                skipSubSubUrls = 0;
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