declare @HigherDistrictId int = 156778;
declare @ElectionId int = 2;

select * 
from ElectionResults er
join ElectoralDistricts d on d.Id = er.ElectoralDistrictId
where d.HigherDistrictId = @HigherDistrictId
and er.ElectionId = @ElectionId

select * 
from ElectionCandidateVotes v
join ElectionResults er on er.Id = v.ElectionResultId
join ElectoralDistricts d on d.Id = er.ElectoralDistrictId
where d.HigherDistrictId = @HigherDistrictId
and er.ElectionId = @ElectionId