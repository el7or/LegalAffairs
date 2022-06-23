import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { CitiesRoutingModule } from './cities-routing.module';
import { CityListComponent } from './city-list/city-list.component';
import { CityFormComponent } from './city-form/city-form.component';


@NgModule({
  declarations: [CityListComponent,CityFormComponent],
  imports: [
    SharedModule,
    CitiesRoutingModule,
    SweetAlert2Module.forChild(),
  ]
})
export class CitiesModule { }
