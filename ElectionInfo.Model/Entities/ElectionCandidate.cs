using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionInfo.Model.Entities
{
	public class ElectionCandidate
	{
		[Key, Column("ElectionId", Order = 0)]
		public int ElectionId { get; set; }
		public virtual Election Election { get; set; }

		[Key, Column("CandidateId", Order = 1)]
		public int CandidateId { get; set; }
		public virtual Candidate Candidate { get; set; }
	}
}
