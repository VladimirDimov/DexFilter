import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { BasicExampleComponent } from './basic-example/basic-example.component';
import { AgGridModule } from 'ag-grid-angular';
import { DatesExampleComponent } from './dates-example/dates-example.component';
import { CustomFilterComponent } from './custom-filter-eample/custom-filter.component';
import { OverrideFilterComponent } from './override-filter-eample/override-filter.component';
import { CustomOrderByComponent } from './custom-orderby-example/custom-orderby.component';
import { FullExampleComponent } from './full-example/full-example.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    BasicExampleComponent,
    DatesExampleComponent,
    CustomFilterComponent,
    OverrideFilterComponent,
    CustomOrderByComponent,
    FullExampleComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AgGridModule,
    RouterModule.forRoot([
      { path: '', component: BasicExampleComponent, pathMatch: 'full' },
      { path: 'dates-example', component: DatesExampleComponent },
      { path: 'override-filter-example', component: OverrideFilterComponent },
      { path: 'custom-filter-example', component: CustomFilterComponent },
      { path: 'custom-orderby-example', component: CustomOrderByComponent },
      { path: 'full-example', component: FullExampleComponent },
    ]),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
