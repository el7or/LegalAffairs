import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { ProvinceFormComponent } from './province-form/province-form.component';
import { ProvinceListComponent } from './province-list/province-list.component';
import { ProvinciesRoutingModule } from './provincies-routing.module';

@NgModule({
  declarations: [
    ProvinceFormComponent,
    ProvinceListComponent
  ],
  imports: [
    SharedModule,
    ProvinciesRoutingModule,
    SweetAlert2Module.forChild()
  ]
})
export class ProvinciesModule { }
