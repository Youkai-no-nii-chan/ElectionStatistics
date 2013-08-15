using System.Data.Entity;

namespace ElectionInfo.Model
{
    public class ModelContext : DbContext
    {
        public ModelContext() : base("ModelContext")
        {
            Candidates = new CandidatesRepository(this);
            Elections = new ElectionsRepository(this);
            ElectoralDistricts = new ElectoralDistrictsRepository(this);
            ElectoralDistrictElection = new ElectoralDistrictElectionsRepository(this);
            ElectionResults = new ElectionResultsRepository(this);
            ElectionCandidatesVotes = new ElectionCandidatesVotesRepository(this);
        }

        public CandidatesRepository Candidates { get; private set; }
        public ElectionsRepository Elections { get; private set; }
        public ElectoralDistrictsRepository ElectoralDistricts { get; private set; }
        public ElectoralDistrictElectionsRepository ElectoralDistrictElection { get; private set; }
        public ElectionResultsRepository ElectionResults { get; private set; }
        public ElectionCandidatesVotesRepository ElectionCandidatesVotes { get; private set; }

        #region Список таблиц для автоматической миграции
        private DbSet<Candidate> CandidatesTable
        {
            get { return Set<Candidate>(); }
        }

        private DbSet<Election> ElectionsTable
        {
            get { return Set<Election>(); }
        }

        private DbSet<ElectoralDistrict> ElectoralDistrictsTable
        {
            get { return Set<ElectoralDistrict>(); }
        }

        private DbSet<ElectoralDistrictElection> ElectoralDistrictElectionTable
        {
            get { return Set<ElectoralDistrictElection>(); }
        }

        private DbSet<ElectionResult> ElectionResultsTable
        {
            get { return Set<ElectionResult>(); }
        }

        private DbSet<ElectionCandidateVotes> ElectionCandidatesVotesTable
        {
            get { return Set<ElectionCandidateVotes>(); }
        }
        #endregion
    }
}