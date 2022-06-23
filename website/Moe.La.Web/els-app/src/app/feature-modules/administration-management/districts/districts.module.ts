import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { DistrictsRoutingModule } from './districts-routing.module';
import { DistrictListComponent } from './district-list/district-list.component';
import { DistrictFormComponent } from './district-form/district-form.component';

@NgModule({
  declarations: [DistrictFormComponent, DistrictListComponent],
  imports: [
    SharedModule,
    DistrictsRoutingModule,
    SweetAlert2Module.forChild()
  ]
})
export class DistrictsModule { }
