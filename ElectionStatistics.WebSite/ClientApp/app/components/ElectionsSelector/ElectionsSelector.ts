import { Component } from '@angular/core';
import { ElectionsService } from '../../services/ElectionsService';

@Component({
	 selector: 'ElectionsSelector',
	 templateUrl: './ElectionsSelector.html'
})
export class ElectionsSelector {
	constructor(private electionsService: ElectionsService) {
	}

	public Elections: ElectionDto[];

	ngOnInit() {		
		this.electionsService
			.getAll()
			.subscribe(	elections => { this.Elections = elections });
	}
}

interface ElectionDto {
	id: number;
	name: string;
}
