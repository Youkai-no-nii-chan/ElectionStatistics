import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { AboutComponent } from './components/about/about.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';

@NgModule({
	 declarations: [
		  AppComponent,
		  NavMenuComponent,
		  CounterComponent,
		  FetchDataComponent,
		 AboutComponent
	 ],
	 imports: [
		  CommonModule,
		  HttpModule,
		  FormsModule,
		  RouterModule.forRoot([
				{ path: '', redirectTo: 'about', pathMatch: 'full' },
				{ path: 'about', component: AboutComponent },
				{ path: 'counter', component: CounterComponent },
				{ path: 'fetch-data', component: FetchDataComponent },
				{ path: '**', redirectTo: 'about' }
		  ])
	 ]
})
export class AppModuleShared {
}
