import { Component } from '@angular/core';
import { ElectoralDistrictsService, ElectoralDistrictDto } from '../ElectoralDistrictsService';
import { ElectionDto } from '../../Elections/ElectionsService';

@Component({
	 selector: 'ElectoralDistrictSelector',
	 templateUrl: './ElectoralDistrictSelector.html'
})
export class ElectoralDistrictSelector {
	constructor(private electoralDistrictsService: ElectoralDistrictsService) {
	}

	public Election: ElectionDto;
	public Districts: ElectoralDistrictDto[];

	ngOnInit() {		
		this.electoralDistrictsService
			.getByElection(this.Election)
			.subscribe(districts => { this.Districts = districts });
	}
}