import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';

export interface ElectionDto {
	id: number;
	name: string;
}

@Injectable()
export class ElectionsService {
	private baseUrl: string;
	private http: Http;

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
		this.baseUrl = baseUrl;
		this.http = http;
	}
	
	public getAll(): Observable<ElectionDto[]> {
		return this.http
			.get(this.baseUrl + 'api/elections')
			.map(result => result.json() as ElectionDto[])
	}
}