import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SharedModule } from 'app/shared/shared.module';
import { NotificationListComponent } from './notification-list/notification-list.component';
import { NotificationServiceManagementRoutingModule } from './notification-service-management-routing.module';
import { DatePickerDualModule } from 'app/shared/components/date-picker-dual/date-picker-dual.module';

@NgModule({
  declarations: [
    NotificationListComponent,
  ],
  imports: [
    SharedModule,
    NotificationServiceManagementRoutingModule,
    SweetAlert2Module.forChild(),
    DatePickerDualModule,
  ],
  exports: [NotificationListComponent]
})
export class NotificationServiceManagementModule { }
