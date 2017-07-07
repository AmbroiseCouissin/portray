import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
//import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
//import { DxDataGridModule, DxButtonModule } from 'devextreme-angular';
//import { BrowserModule } from '@angular/platform-browser';

import { SubmittedComponent } from './shared/submitted.component';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { KycComponent } from './components/kyc/kyc.component'
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
//import { AssetsComponent } from './components/assets/assets.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,
        KycComponent,
        SubmittedComponent,
        //AssetsComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'kyc', component: KycComponent },
           // { path: 'assets', component: AssetsComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    exports: [
        SubmittedComponent
    ],
})
export class AppModuleShared {
}
