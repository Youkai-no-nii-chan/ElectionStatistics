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
            Reader.MoveTo("Число бюллетеней, не учтенных при получении");

            int lineNumber = 10;
            while (Reader.MoveTo(lineNumber + "</nobr>", "</table>", out line))
            {
                Reader.MoveTo("<nobr>", out line);
                candidates.Add(Context.Candidates
                    .GetOrAdd(line.GetTagValue("nobr").Trim(), Election));
                lineNumber++;
            }

            if (candidates.Count == 0)
                throw new InvalidOperationException("candidates.Count == 0");

            Reader.MoveTo("УИК", out line);
            var district = new ElectoralDistrict(line.GetTagValue("nobr"), HigherDistrict);
            Context.ElectoralDistricts.Add(district);
            results.Add(
                new ElectionResult
                    {
                        ElectoralDistrict = district,
                        Election = Election,
                        DataSourceUrl = Url
                    });
            while (Reader.MoveTo("УИК", "</tr>", out line))
            {
                district = new ElectoralDistrict(line.GetTagValue("nobr"), HigherDistrict);
                Context.ElectoralDistricts.Add(district);
                results.Add(
                    new ElectionResult
                        {
                            ElectoralDistrict = district,
                            Election = Election,
                            DataSourceUrl = Url
                        });
            }
            Context.SaveChanges();

            SetValues(results, (u, i) => u.VotersCount = i); // 1
            SetValues(results, (u, i) => u.ReceivedBallotsCount = i); // 2
            // SetValues(results, (u, i) => u.EarlyIssuedBallotsCount = i);
            SetValues(results, (u, i) => u.IssuedInsideBallotsCount = i); // 3
            SetValues(results, (u, i) => u.IssuedOutsideBallotsCount = i); // 4
            SetValues(results, (u, i) => u.CanceledBallotsCount = i); // 5
            SetValues(results, (u, i) => u.OutsideBallotsCount = i); // 6
            SetValues(results, (u, i) => u.InsideBallotsCount = i); // 7
            SetValues(results, (u, i) => u.InvalidBallotsCount = i); // 8
            SetValues(results, (u, i) => u.ValidBallotsCount = i); // 9
            SetValues(results, (u, i) => u.ReceivedAbsenteeCertificatesCount = i); // 9а
            SetValues(results, (u, i) => u.IssuedAbsenteeCertificatesCount = i); // 9б
            SetValues(results, (u, i) => u.AbsenteeCertificateVotersCount = i);  // 9в
            SetValues(results, (u, i) => u.CanceledAbsenteeCertificatesCount = i); // 9г
            SetValues(results, (u, i) => u.IssuedByHigherDistrictAbsenteeCertificatesCount = i); // 9д
            SetValues(results, (u, i) => u.LostAbsenteeCertificatesCount = i); // 9е
            SetValues(results, (u, i) => u.LostBallotsCount = i); // 9ж
            SetValues(results, (u, i) => u.UnaccountedBallotsCount = i); // 9з
			
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