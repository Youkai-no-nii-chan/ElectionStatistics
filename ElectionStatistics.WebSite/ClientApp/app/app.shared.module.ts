import 'rxjs';

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { ChartsPage } from './Charts/Page/ChartsPage';

import { ElectionsService } from './Elections/ElectionsService';
import { ElectionSelector } from './Elections/Selector/ElectionSelector';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
	    ChartsPage,
		HomeComponent,
	    ElectionSelector
    ],
    imports: [
        CommonModule,
	    HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'charts', component: ChartsPage },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
	    ElectionsService
    ]
})
export class AppModuleShared {
}
