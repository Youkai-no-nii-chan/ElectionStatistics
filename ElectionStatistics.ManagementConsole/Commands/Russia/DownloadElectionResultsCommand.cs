using HtmlAgilityPack;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ElectionStatistics.ManagementConsole
{
	internal class DownloadElectionResultsCommand : Command
	{
		private RestClient client = new RestClient();

		public override string Name => "Download-ElectionResults";

		public override void Execute(string[] arguments)
		{
			if (arguments.Length != 1)
				throw new ArgumentException("arguments.Length != 1");

			var vrn = arguments[0];

			var districts =
				GetDistrictInfo(null, vrn, 3);

			var results = districts
				.SelectMany(d => d.InnerDistricts)
				.SelectMany(d => d.InnerDistricts)
				.Select(d => new { Name = d.Name, Results = GetElectionResults(d.Vibid) })
				.ToList();
		}

		private string Get(string uri)
		{
			client.BaseUrl = new Uri(uri);

			var request = new RestRequest(Method.GET);

			var response = client.Execute(request);

			return response.Content;
		}

		private List<ElectoralDistrictDto> GetDistrictInfo(string vibid, string vrn, int maxNestingLevel)
		{
			if (maxNestingLevel <= 0)
				return null;

			string url;

			if (vibid == null)
				url = $"http://www.vybory.izbirkom.ru/region/izbirkom?action=show&global=1&vrn={vrn}&region=0&prver=0&pronetvd=0";
			else
				url = $"http://www.vybory.izbirkom.ru/region/izbirkom?action=show&global=1&vrn={vrn}&region=0&prver=0&pronetvd=0&vibid={vibid}";

			var selector = new Regex(@"<option\s+value=""[^""]+vibid=([0-9]+)[^""]*"">\s*([^<]+)<\/option>");

			var result = Get(url);

			return selector
				.Matches(result)
				.OfType<Match>()
				.Select(m => new ElectoralDistrictDto
				{
					Name = m.Groups[2].Value,
					Vibid = m.Groups[1].Value,
					InnerDistricts = GetDistrictInfo(m.Groups[1].Value, vrn, maxNestingLevel - 1)
				})
				.ToList();
		}

		public List<ElectionResultsDto> GetElectionResults(string vibid)
		{
			var response = Get($"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&global=true&region=1&sub_region=1&vibid={vibid}&type=233");
			var doc = new HtmlDocument();

			doc.LoadHtml(response);

			var candidatesTable = doc
				.DocumentNode
				.SelectNodes("//*[contains(., 'Число избирателей')]")
				.Last()
				.ParentNode
				.ParentNode
				.ParentNode;

			var resultsTable = candidatesTable
				.ParentNode
				.ParentNode
				.Elements("td")
				.Last()
				.Element("div")
				.Element("table");

			var metrics = candidatesTable
				.Elements("tr")
				.Select(e => e.Elements("td").ToList())
				.Select(tds => new {
					Metric = tds[1].Element("nobr").InnerText.Trim(' '),
					Value = tds.Last().Element("nobr").Element("b").InnerText.Trim(' ')
				})
				.ToList();

			var results = resultsTable
				.Elements("tr")
				.Select(e => e
					.Elements("td")
					.Select(td => td.Element("nobr"))
					.Select(td => td?.Element("b")?.InnerText ?? td?.InnerText)
					.ToList())
				.ToList();

			var result = new List<ElectionResultsDto>();

			for (int i = 0; i < metrics.Count; i++)
			{
				if (i == 0)
				{
					result = results[i].Select(r => new ElectionResultsDto { ElectoralDistrictName = r }).ToList();
					continue;
				}

				if (metrics[i].Metric == "&nbsp;")
					continue;

				if (metrics[i].Metric.StartsWith("Число"))
				{
					for (int j = 0; j < results[i].Count(); j++)
						electionResultsFieldParsers[metrics[i].Metric](result[j], int.Parse(results[i][j]));

					continue;
				}

				for (int j = 0; j < results[i].Count(); j++)
				{
					result[j].Votes[metrics[i].Metric] = int.Parse(results[i][j]);

					continue;
				}
			}

			return result;
		}

		private Dictionary<string, Action<ElectionResultsDto, int>> electionResultsFieldParsers =
			new Dictionary<string, Action<ElectionResultsDto, int>>
			{
				{"Число избирателей, внесенных в список избирателей на момент окончания голосования",
					(e, v) => e.VotersCount = v},
				{"Число избирательных бюллетеней, полученных участковой избирательной комиссией",
					(e, v) => e.ReceivedBallotsCount = v},
				{"Число избирательных бюллетеней, выданных избирателям, проголосовавшим досрочно",
					(e, v) => e.EarlyIssuedBallotsCount = v},
				{"Число избирательных бюллетеней, выданных в помещении для голосования в день голосования",
					(e, v) => e.IssuedInsideBallotsCount = v},
				{"Число избирательных бюллетеней, выданных вне помещения для голосования в день голосования",
					(e, v) => e.IssuedOutsideBallotsCount = v},
				{"Число погашенных избирательных бюллетеней",
					(e, v) => e.CanceledBallotsCount = v},
				{"Число избирательных бюллетеней, содержащихся в переносных ящиках для голосования",
					(e, v) => e.OutsideBallotsCount = v},
				{"Число избирательных бюллетеней, содержащихся в стационарных ящиках для голосования",
					(e, v) => e.InsideBallotsCount = v},
				{"Число недействительных избирательных бюллетеней",
					(e, v) => e.InvalidBallotsCount = v},
				{"Число действительных избирательных бюллетеней",
					(e, v) => e.ValidBallotsCount = v},
				{"Число открепительных удостоверений, полученных участковой избирательной комиссией",
					(e, v) => e.ReceivedAbsenteeCertificatesCount = v},
				{"Число открепительных удостоверений, выданных на избирательном участке до дня голосования",
					(e, v) => e.IssuedAbsenteeCertificatesCount = v},
				{"Число избирателей, проголосовавших по открепительным удостоверениям на избирательном участке",
					(e, v) => e.AbsenteeCertificateVotersCount = v},
				{"Число погашенных неиспользованных открепительных удостоверений",
					(e, v) => e.CanceledAbsenteeCertificatesCount = v},
				{"Число открепительных удостоверений, выданных избирателям территориальной избирательной комиссией",
					(e, v) => e.IssuedByHigherDistrictAbsenteeCertificatesCount = v},
				{"Число утраченных открепительных удостоверений",
					(e, v) => e.LostAbsenteeCertificatesCount = v},
				{"Число утраченных избирательных бюллетеней",
					(e, v) => e.LostBallotsCount = v},
				{"Число избирательных бюллетеней, не учтенных при получении",
					(e, v) => e.UnaccountedBallotsCount = v},
			};
	}
}
