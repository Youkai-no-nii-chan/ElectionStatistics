using System.Data.Entity;

namespace ElectionInfo.Model
{
    public class ModelContext : DbContext
    {
        public ModelContext() : base("ModelContext")
        {
            CandidatesRepository = new CandidatesRepository(this);
            ElectoralDistrictsRepository = new ElectoralDistrictsRepository(this);
        }

        public CandidatesRepository CandidatesRepository { get; private set; }
        public ElectoralDistrictsRepository ElectoralDistrictsRepository { get; private set; }

        public DbSet<Candidate> Candidates
        {
            get { return Set<Candidate>(); }
        }

        public DbSet<Election> Elections
        {
            get { return Set<Election>(); }
        }

        public DbSet<ElectoralDistrict> ElectoralDistricts
        {
            get { return Set<ElectoralDistrict>(); }
        }

        public DbSet<ElectoralDistrictElection> ElectoralDistrictElection
        {
            get { return Set<ElectoralDistrictElection>(); }
        }

        public DbSet<ElectionResult> ElectionResults
        {
            get { return Set<ElectionResult>(); }
        }

        public DbSet<ElectionCandidateVotes> ElectionCandidatesVotes
        {
            get { return Set<ElectionCandidateVotes>(); }
        }
    }
}