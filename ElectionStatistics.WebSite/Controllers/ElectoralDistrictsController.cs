using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectionStatistics.Model;
using Microsoft.AspNetCore.Mvc;

namespace ElectionStatistics.WebSite
{
    [Route("api/electoral-districts")]
    public class ElectoralDistrictsController : Controller
    {
	    private readonly ModelContext modelContext;

	    public ElectoralDistrictsController(ModelContext modelContext)
	    {
		    this.modelContext = modelContext;
	    }

	    [HttpGet("by-election")]
		public IEnumerable<ElectoralDistrictDto> GetByElection(int electionId)
        {
            return modelContext.ElectoralDistricts
				.ByElection(modelContext, electionId)
				.Select(election => new ElectoralDistrictDto
	            {
		            Id = election.Id,
					Name = election.Name
	            })
				.ToArray();
        }

        public class ElectoralDistrictDto
		{
			public int Id { get; set; }
			public string Name { get; set; }
        }
    }
}
