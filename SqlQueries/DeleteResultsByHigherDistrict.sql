declare @HigherDistrictId int = 140769;

delete v  
from ElectionCandidateVotes v
join ElectionResults er on er.Id = v.ElectionResultId
join ElectoralDistricts d on d.Id = er.ElectoralDistrictId
where d.HigherDistrictId = @HigherDistrictId

delete er 
from ElectionResults er
join ElectoralDistricts d on d.Id = er.ElectoralDistrictId
where d.HigherDistrictId = @HigherDistrictId