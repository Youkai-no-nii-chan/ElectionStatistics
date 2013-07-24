using System;
using System.IO;
using System.Linq;
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

            District = Context.ElectoralDistrictsRepository.GetOrCreateByUniqueName(districtName);

            Reader.MoveTo("<b>Дата голосования</b>");
            string electionDateLine;
            Reader.MoveTo("<td", out electionDateLine);
            DateTime electionDate = DateTime.Parse(electionDateLine.GetTagValue("td"));

            Election = Context.Elections.FirstOrDefault(
                election => election.Name == electionName && election.Date == electionDate);
            if (Election == null)
            {
                Election = new Election
                                     {
                                         Name = electionName,
                                         ElectoralDistrict = District,
                                         DataSourceUrl = Url,
                                         Date = electionDate
                                     };
                Context.Elections.Add(Election);
            }
            Context.SaveChanges();
        }
    }
}