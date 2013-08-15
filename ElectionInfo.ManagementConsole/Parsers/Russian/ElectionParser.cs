using System;
using System.IO;
using ElectionInfo.Model;

namespace ElectionInfo.ManagementConsole
{
    internal class ElectionParser : ElectoralDistrictParser
    {
        public ElectionParser(string url, StreamReader reader, ModelContext context)
            : base(url, reader, context, null)
        {
        }

        public override void Parse()
        {
            string comboBoxLine;
            Reader.MoveTo("<select name=\"gs\">", out comboBoxLine);
            string[] comboBoxLines = comboBoxLine.Split(new[] { "<option" }, StringSplitOptions.None);
            foreach (string line in comboBoxLines)
            {
                string value = line.GetValueAttribute();
                if (value != null)
                {
                    SubUrls.Add(value);
                }
            }

            Reader.MoveTo("<b>Сведения о выборах</b>");
            string electionNameLine;
            Reader.MoveTo("<b", out electionNameLine);
            string electionName = electionNameLine.GetTagValue("b");

            string districtNameLine;
            Reader.MoveTo("<b>Наименование комиссии</b>");
            Reader.MoveTo("<td", out districtNameLine);
            var districtName = districtNameLine.GetTagValue("td");

            District = Context.ElectoralDistricts.GetOrAddByUniqueName(districtName);

            Reader.MoveTo("<b>Дата голосования</b>");
            string electionDateLine;
            Reader.MoveTo("<td", out electionDateLine);
            var electionDate = DateTime.Parse(electionDateLine.GetTagValue("td"));

            Election = Context.Elections.GetOrAdd(electionName, District, electionDate, Url);

            Context.SaveChanges();
        }
    }
}