using System.IO;
using ElectionInfo.Model;

namespace ElectionInfo.ManagementConsole
{
    internal interface IParsersFactory
    {
        IParser CreateParser(
            string uri, StreamReader reader, ModelContext context, IParser parentParser = null);
    }
}