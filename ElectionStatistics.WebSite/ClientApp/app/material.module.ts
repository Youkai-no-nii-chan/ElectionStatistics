import { NgModule } from '@angular/core';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import {
	MatButtonModule
} from '@angular/material';

@NgModule({
	exports: [
		NoopAnimationsModule,
		MatButtonModule
	]
})
export class MaterialModule {
}
