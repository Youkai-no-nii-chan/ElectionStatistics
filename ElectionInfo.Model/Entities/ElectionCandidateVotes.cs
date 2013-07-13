using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionInfo.Model.Entities
{
    public class ElectionCandidateVotes
    {
        [Key, Column("ElectionResultId", Order = 0)]
        public int ElectionResultId { get; set; }
        public virtual ElectionResult ElectionResult { get; set; }

        [Key, Column("CandidateId", Order = 1)]
        public int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }

        public int Count { get; set; }
    }
}