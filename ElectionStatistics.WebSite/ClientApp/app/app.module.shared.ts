import 'rxjs';

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { MaterialModule } from './material.module';

import { AppComponent } from './Application/app.component';
import { NavigationMenuComponent } from './NavigationMenu/NavigationMenuComponent';
import { AboutPage } from './About/AboutPage';

import { ElectionsService } from './Elections/ElectionsService';
import { ElectionSelector } from './Elections/Selector/ElectionSelector';

import { ElectoralDistrictsService } from './ElectoralDistricts/ElectoralDistrictsService';
import { ElectoralDistrictSelector } from './ElectoralDistricts/selector/ElectoralDistrictSelector';

@NgModule({
    declarations: [
        AppComponent,
	    NavigationMenuComponent,
	    AboutPage,
		ElectionSelector,
	    ElectoralDistrictSelector
    ],
    imports: [
        CommonModule,
        HttpClientModule,
		FormsModule,
		MaterialModule,
	    RouterModule.forRoot([
		    { path: '', redirectTo: 'about', pathMatch: 'full' },
		    { path: 'about', component: AboutPage },
		    { path: 'votes-percentage-distribution', component: ElectionSelector },
		    { path: '**', redirectTo: 'about' }
	    ])
    ],
    providers: [
		ElectionsService
	],
})
export class AppModuleShared {
}
