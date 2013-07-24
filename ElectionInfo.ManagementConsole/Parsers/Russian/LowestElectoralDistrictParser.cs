using System.IO;
using ElectionInfo.Model;

namespace ElectionInfo.ManagementConsole
{
    internal class LowestElectoralDistrictParser : Parser
    {
        public LowestElectoralDistrictParser(string url, StreamReader reader, ModelContext context, IParser parent)
            : base(url, reader, context, parent)
        {
        }

        public override void Parse()
        {
            string line;
            Reader.MoveTo("Сводная таблица ", out line);
            SubUrls.Add(line.GetHrefAttribute());
        }
    }
}