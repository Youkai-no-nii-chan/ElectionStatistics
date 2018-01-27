using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionStatistics.Model
{
    public class ElectionCandidateVotes
	{
		[Required]
		public int ElectionResultId { get; set; }
        public virtual ElectionResult ElectionResult { get; set; }

		[Required]
		public int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }

        public int Count { get; set; }
    }
}