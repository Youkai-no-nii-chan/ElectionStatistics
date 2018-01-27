import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavigationMenuComponent } from './components/NavigationMenuComponent/NavigationMenuComponent';
import { AboutPage } from './components/AboutPage/AboutPage';
import { ElectionsSelector } from './components/ElectionsSelector/ElectionsSelector';

@NgModule({
	 declarations: [
		AppComponent,
		NavigationMenuComponent,
		AboutPage,
		ElectionsSelector,
	 ],
	 imports: [
		  CommonModule,
		  HttpModule,
		  FormsModule,
		  RouterModule.forRoot([
				{ path: '', redirectTo: 'about', pathMatch: 'full' },
				{ path: 'about', component: AboutPage },
				{ path: 'votes-percentage-distribution', component: ElectionsSelector },
				{ path: '**', redirectTo: 'about' }
		  ])
	 ]
})
export class AppModuleShared {
}
