import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
	 selector: 'ElectionsSelector',
	 templateUrl: './ElectionsSelector.html'
})
export class ElectionsSelector {
	public elections: ElectionDto[];

	constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
		http
			.get(baseUrl + 'api/elections')
			.subscribe(
				result => { this.elections = result.json() as ElectionDto[]; },
				error => console.error(error));
	 }
}

interface ElectionDto {
	id: number;
	name: string;
}
