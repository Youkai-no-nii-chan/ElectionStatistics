import { Component } from '@angular/core';
import { ElectionsService, ElectionDto } from '../ElectionsService';

@Component({
	 selector: 'ElectionSelector',
	 templateUrl: './ElectionSelector.html'
})
export class ElectionSelector {
	constructor(private electionsService: ElectionsService) {
	}

	public Elections: ElectionDto[];

	ngOnInit() {		
		this.electionsService
			.getAll()
			.subscribe(elections => { this.Elections = elections });
	}
}
