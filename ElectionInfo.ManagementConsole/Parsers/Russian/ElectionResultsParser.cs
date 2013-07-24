using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ElectionInfo.Model;

namespace ElectionInfo.ManagementConsole
{
    internal class ElectionResultsParser : Parser
    {
        public ElectionResultsParser(string url, StreamReader reader, ModelContext context, IParser parent)
            : base(url, reader, context, parent)
        {
            Election = Parent.Election;
            HigherDistrict = Parent.District;
        }
        
        public ElectoralDistrict HigherDistrict { get; protected set; }
        public Election Election { get; protected set; }
        public ElectoralDistrictParser Parent
        {
            get { return (ElectoralDistrictParser)((IParser)this).Parent.Parent; }
        }

        #region Члены интерфейса IParser

        public override void Parse()
        {
            var candidates = new List<Candidate>();
            var results = new List<ElectionResult>();

            string line;
            Reader.MoveTo("Число избирательных бюллетеней, не учтенных при получении");

            int lineNumber = 19;
            while (Reader.MoveTo(lineNumber + "</nobr>", "</table>", out line))
            {
                Reader.MoveTo("<nobr>", out line);
                candidates.Add(Context.CandidatesRepository
                    .GetOrCreate(line.GetTagValue("nobr").Trim(), Election));
                lineNumber++;
            }

            if (candidates.Count == 0)
                throw new InvalidOperationException("candidates.Count == 0");

            Reader.MoveTo("УИК", out line);
            results.Add(
                new ElectionResult
                    {
                        ElectoralDistrict = Context.ElectoralDistrictsRepository.GetOrCreate(line.GetTagValue("nobr"), HigherDistrict),
                        Election = Election,
                        DataSourceUrl = Url
                    });
            while (Reader.MoveTo("УИК", "</tr>", out line))
            {
                results.Add(
                    new ElectionResult
                        {
                            ElectoralDistrict = Context.ElectoralDistrictsRepository.GetOrCreate(line.GetTagValue("nobr"), HigherDistrict),
                            Election = Election,
                            DataSourceUrl = Url
                        });
            }
            Context.SaveChanges();

            SetValues(results, (u, i) => u.VotersCount = i);
            SetValues(results, (u, i) => u.ReceivedBallotsCount = i);
            SetValues(results, (u, i) => u.EarlyIssuedBallotsCount = i);
            SetValues(results, (u, i) => u.IssuedInsideBallotsCount = i);
            SetValues(results, (u, i) => u.IssuedOutsideBallotsCount = i);
            SetValues(results, (u, i) => u.CanceledBallotsCount = i);
            SetValues(results, (u, i) => u.OutsideBallotsCount = i);
            SetValues(results, (u, i) => u.InsideBallotsCount = i);
            SetValues(results, (u, i) => u.InvalidBallotsCount = i);
            SetValues(results, (u, i) => u.ValidBallotsCount = i);
            SetValues(results, (u, i) => u.ReceivedAbsenteeCertificatesCount = i);
            SetValues(results, (u, i) => u.IssuedAbsenteeCertificatesCount = i);
            SetValues(results, (u, i) => u.AbsenteeCertificateVotersCount = i);
            SetValues(results, (u, i) => u.CanceledAbsenteeCertificatesCount = i);
            SetValues(results, (u, i) => u.IssuedByHigherDistrictAbsenteeCertificatesCount = i);
            SetValues(results, (u, i) => u.LostAbsenteeCertificatesCount = i);
            SetValues(results, (u, i) => u.LostBallotsCount = i);
			SetValues(results, (u, i) => u.UnaccountedBallotsCount = i);
			
            foreach (var result in results)
            {
                Context.ElectionResults.Add(result);
            }
            Context.SaveChanges();

            foreach (var candidate in candidates)
            {
                AddVotes(results, candidate);
            }
            Context.SaveChanges();
        }

        #endregion

		private void SetValues(IEnumerable<ElectionResult> results, Action<ElectionResult, int> setValue)
        {
        	foreach (var result in results)
            {
            	string line;
            	Reader.MoveTo("<nobr", out line);
				setValue(result, int.Parse(line.GetTagValue("b")));
            }
        }

		private void AddVotes(IEnumerable<ElectionResult> results, Candidate candidate)
        {
        	foreach (var result in results)
            {
                string line;
				Reader.MoveTo("<nobr", out line);
                Context.ElectionCandidatesVotes.Add(new ElectionCandidateVotes
                {
                    Candidate = candidate,
                    ElectionResult = result,
                    Count = int.Parse(line.GetTagValue("b"))
                });
            }
        }
    }
}