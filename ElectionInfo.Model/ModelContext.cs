using System.Data.Entity;
using System.Data.SqlClient;
using ElectionInfo.Model.Entities;

namespace ElectionInfo.Model
{
    public class ModelContext : DbContext
    {
        public ModelContext(string connectionString)
            : base(new SqlConnection(connectionString), true)
        {
        }

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