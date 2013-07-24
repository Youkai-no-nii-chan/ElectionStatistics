using System;
using System.IO;
using ElectionInfo.Model;

namespace ElectionInfo.ManagementConsole
{
    internal class ParsersFactory : IParsersFactory
    {
        #region ����� ���������� IParsersFactory

        public IParser CreateParser(
            string uri, StreamReader reader, ModelContext context, IParser parentParser = null)
        {
            if (parentParser == null)
            {
                return new ElectionParser(uri, reader, context);
            }
            else if (parentParser is ElectionParser)
            {
                return new ElectoralDistrictParser(uri, reader, context, parentParser);
            }
            else if (parentParser is ElectoralDistrictParser)
            {
                reader.MoveTo("<!-- history -->");
                string headerFirstLine;
                if (!reader.MoveTo("</a>", "</td>", out headerFirstLine))
                {
                    throw new Exception("� ��������� ����������� ������.");
                }

                if (!headerFirstLine.Contains("��� ������"))
                {
                    //���� � ��������� ����������� ������� "��� ������", ������ ��� �������� �� ������� ���� ������ �� ������� � ������
                    return new LowestElectoralDistrictParser(uri, reader, context, parentParser);
                }
                else
                {
                    return new ElectoralDistrictParser(uri, reader, context, parentParser);
                }
            }
            else if (parentParser is LowestElectoralDistrictParser)
            {
                return new ElectionResultsParser(uri, reader, context, parentParser);
            }
            else
            {
                throw new Exception(
                    string.Format("{0} �� ����� ���� ��������� � ������� ��������.", parentParser.GetType()));
            }
        }

        #endregion
    }
}