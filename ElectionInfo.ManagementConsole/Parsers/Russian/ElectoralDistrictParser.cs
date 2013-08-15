using System;
using System.IO;
using System.Linq;
using ElectionInfo.Model;

namespace ElectionInfo.ManagementConsole
{
    internal class ElectoralDistrictParser : Parser
    {
        public ElectoralDistrictParser(string url, StreamReader reader, ModelContext context, IParser parent)
            : base(url, reader, context, parent)
        {
            if (parent != null)
            {
                Election = Parent.Election;
                HigherDistrict = Parent.District;
            }
        }

        public ElectoralDistrict HigherDistrict { get; protected set; }
        public ElectoralDistrict District { get; protected set; }
        public ElectoralDistrictElection DistrictElection { get; protected set; }
        public Election Election { get; protected set; }

        public ElectoralDistrictParser Parent
        {
            get { return (ElectoralDistrictParser)((IParser)this).Parent; }
        }

        #region Члены интерфейса IParser

        public override void Parse()
        {
            string latestLine;
            byte searchResult = Reader.MoveTo(
                "<select name=\"gs\">",
                "Для просмотра данных по участковым избирательным комиссиям",
                "<b>Наименование комиссии</b>",
                out latestLine);

            string districtName;
            if (searchResult == 3)
            {
                Reader.MoveTo("<td", out latestLine);
                districtName = latestLine.GetTagValue("td");

                Console.WriteLine("Введите url для \"{0}\":", districtName);
                string uri = Console.ReadLine();
                
                SubUrls.Add(uri);
            }
            else
            {
                if (searchResult == 1)
                {
                    string[] comboBoxLines = latestLine.Split(new[] {"<option"}, StringSplitOptions.None);
                    foreach (string line in comboBoxLines)
                    {
                        string value = line.GetValueAttribute();
                        if (value != null)
                        {
                            SubUrls.Add(value);
                        }
                    }
                }
                else
                {
                    SubUrls.Add(latestLine.GetHrefAttribute());
                }

                Reader.MoveTo("<b>Наименование комиссии</b>");
                Reader.MoveTo("<td", out latestLine);
                districtName = latestLine.GetTagValue("td");
            }

            District = Context.ElectoralDistricts.GetOrAdd(districtName, HigherDistrict);
            DistrictElection = Context.ElectoralDistrictElection.GetOrAdd(Election, District, Url);

            Context.SaveChanges();
        }

        #endregion
    }
}