﻿using System;
using System.Linq;

namespace ElectionInfo.Model
{
    public static class ElectionResultsQueryableExtensions
    {
        public static IQueryable<ElectionResult> ByHigherDistrict(
            this IQueryable<ElectionResult> items,
            ElectoralDistrict district)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            var hierarchyPath = district.HierarchyPath + "\\" + district.Id;
            return items
                .Where(electionResult => electionResult.ElectoralDistrict.HierarchyPath.StartsWith(hierarchyPath));
        }

        public static IQueryable<ElectionResult> ByElection(
            this IQueryable<ElectionResult> items, 
            int electionId)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Where(result => result.ElectionId == electionId);
        }
    }
}