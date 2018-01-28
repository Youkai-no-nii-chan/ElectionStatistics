import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ElectionDto } from '../Elections/ElectionsService';

export interface ElectoralDistrictDto {
	id: number;
	name: string;
}

@Injectable()
export class ElectoralDistrictsService {
	constructor(
		private httpClient: HttpClient, 
		@Inject('BASE_URL') private baseUrl: string) {
	}
	
	public getByElection(election: ElectionDto): Observable<ElectoralDistrictDto[]> {
		return this.httpClient
			.get(
				this.baseUrl + 'api/elections/by-election',
				{
					params: new HttpParams().set('electionId', election.toString())
				})
			.map(result => result as ElectoralDistrictDto[])
	}
}