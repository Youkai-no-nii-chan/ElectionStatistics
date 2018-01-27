import { NgModule } from '@angular/core';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import {
	MatCheckboxModule
} from '@angular/material';

@NgModule({
	exports: [
		NoopAnimationsModule,
		MatCheckboxModule
	]
})
export class MaterialModule {
}
