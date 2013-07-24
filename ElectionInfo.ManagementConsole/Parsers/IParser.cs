using System.Collections.Generic;
using System.IO;
using ElectionInfo.Model;

namespace ElectionInfo.ManagementConsole
{
    internal interface IParser
    {
        IParser Parent { get; set; }

        List<string> SubUrls { get; }

        void Parse();
    }

    internal abstract class Parser : IParser
    {
        protected string Url { get; private set; }

        protected StreamReader Reader { get; private set; }

        protected ModelContext Context { get; private set; }

        protected Parser(string url, StreamReader reader, ModelContext context, IParser parent)
        {
            Url = url;
            Reader = reader;
            Context = context;
            ((IParser) this).Parent = parent;
            SubUrls = new List<string>();
        }

        #region „лены интерфейса IParser

        IParser IParser.Parent { get; set; }

        public List<string> SubUrls { get; protected set; }

        public abstract void Parse();

        #endregion
    }
}