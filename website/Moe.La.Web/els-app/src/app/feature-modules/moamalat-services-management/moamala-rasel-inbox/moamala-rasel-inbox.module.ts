import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';
import { SharedModule } from 'app/shared/shared.module';
import { MoamalaRaselInboxRoutingModule } from './moamala-rasel-inbox-routing.module';
import { MoamalatRaselInboxListComponent } from './moamalat-rasel-inbox-list/moamalat-rasel-inbox-list.component';


@NgModule({
  declarations: [MoamalatRaselInboxListComponent],
  imports: [
    SharedModule,
    MoamalaRaselInboxRoutingModule,
    SweetAlert2Module.forChild(),
    DatePickerDualModule
  ]
})
export class MoamalaRaselInboxModule { }
