using System;
using System.Linq;

namespace ElectionInfo.Model
{
    public static class ElectionCandidatesVotesQueryableExtensions
    {
        public static IQueryable<ElectionCandidateVotes> ByCandidate(
            this IQueryable<ElectionCandidateVotes> items,
            Candidate candidate)
        {
            if (candidate == null)
                throw new ArgumentNullException("candidate");

            return items.ByCandidate(candidate.Id);
        }

        public static IQueryable<ElectionCandidateVotes> ByCandidate(
            this IQueryable<ElectionCandidateVotes> items,
            int candidateId)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Where(result => result.CandidateId == candidateId);
        } 
    }
}