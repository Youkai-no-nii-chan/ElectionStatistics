import { NgModule } from '@angular/core';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import {
	MatSelectModule
} from '@angular/material';

@NgModule({
	exports: [
		NoopAnimationsModule,
		MatSelectModule
	]
})
export class MaterialModule {
}
