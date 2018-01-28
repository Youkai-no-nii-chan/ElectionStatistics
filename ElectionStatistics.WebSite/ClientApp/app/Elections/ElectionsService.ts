import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ElectionDto {
	id: number;
	name: string;
}

@Injectable()
export class ElectionsService {
	constructor(
		private httpClient: HttpClient, 
		@Inject('BASE_URL') private baseUrl: string) {
	}
	
	public getAll(): Observable<ElectionDto[]> {
		return this.httpClient
			.get(this.baseUrl + 'api/elections')
			.map(result => result as ElectionDto[])
	}
}