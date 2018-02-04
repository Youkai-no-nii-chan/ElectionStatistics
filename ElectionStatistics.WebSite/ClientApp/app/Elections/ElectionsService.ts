import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';

export interface ElectionDto {
	id: number;
	name: string;
}

@Injectable()
export class ElectionsService {
	constructor(
		private http: Http, 
		@Inject('BASE_URL') private baseUrl: string) {
	}
	
	public getAll() {
		return this.http
			.get(this.baseUrl + 'api/elections');
	}
}