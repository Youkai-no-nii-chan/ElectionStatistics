using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionStatistics.Model
{
    public class ElectionCandidate
	{
		[Required]
		public int ElectionId { get; set; }
		public virtual Election Election { get; set; }

		[Required]
		public int CandidateId { get; set; }
		public virtual Candidate Candidate { get; set; }
    }
}