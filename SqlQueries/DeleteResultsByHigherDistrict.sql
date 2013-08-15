declare @HigherDistrictId int = 146697;
declare @ElectionId int = 2;

delete v  
from ElectionCandidateVotes v
join ElectionResults er on er.Id = v.ElectionResultId
join ElectoralDistricts d on d.Id = er.ElectoralDistrictId
where d.HigherDistrictId = @HigherDistrictId
and er.ElectionId = @ElectionId

delete er 
from ElectionResults er
join ElectoralDistricts d on d.Id = er.ElectoralDistrictId
where d.HigherDistrictId = @HigherDistrictId
and er.ElectionId = @ElectionId