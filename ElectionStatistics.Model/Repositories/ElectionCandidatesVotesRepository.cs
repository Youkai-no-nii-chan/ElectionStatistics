namespace ElectionStatistics.Model
{
    public class ElectionCandidatesVotesRepository : Repository<ElectionCandidateVotes>
    {
        public ElectionCandidatesVotesRepository(ModelContext context) : base(context)
        {
        }
    }
}