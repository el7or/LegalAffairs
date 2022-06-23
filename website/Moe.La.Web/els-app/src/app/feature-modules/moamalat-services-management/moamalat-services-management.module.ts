import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { MoamalatServicesManagementRoutingModule } from './moamalat-services-management-routing.module';
import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';
import { SharedModule } from 'app/shared/shared.module';
import { MoamalatListComponent } from './moamalat-list/moamalat-list.component';
import { MoamalatViewComponent } from './moamalat-view/moamalat-view.component';
import { ChangeMoamalaStatusFormComponent } from './change-moamala-status-form/change-moamala-status-form.component';
import { MoamalatListTableComponent } from './moamalat-list/moamalat-list-table/moamalat-list-table.component';
import { MoamalatFormComponent } from './moamalat-form/moamalat-form.component';

@NgModule({
  declarations: [
    MoamalatListComponent,
    MoamalatViewComponent,
    ChangeMoamalaStatusFormComponent,
    MoamalatListTableComponent,
    MoamalatFormComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    MoamalatServicesManagementRoutingModule,
    SweetAlert2Module.forChild(),
    DatePickerDualModule,
  ],
  exports: [MoamalatListTableComponent]
})
export class MoamalatServicesManagementModule { }
