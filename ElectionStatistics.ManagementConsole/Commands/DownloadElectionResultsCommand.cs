using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

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

			var url = arguments[0];

			var content = Get(url);


		}

		private string Get(string uri)
		{
			client.BaseUrl = new Uri(uri);

			var request = new RestRequest(Method.GET);

			var response = client.Execute(request);

			return response.Content;
		}
	}
}
