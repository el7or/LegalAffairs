import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { WorkItemTypeRoutingModule } from './work-item-type-routing.module';
import { WorkItemTypeFormComponent } from './work-item-type-form/work-item-type-form.component';
import { WorkItemTypeListComponent } from './work-item-type-list/work-item-type-list.component';
import { SharedModule } from 'app/shared/shared.module';

@NgModule({
  declarations: [WorkItemTypeFormComponent, WorkItemTypeListComponent],
  imports: [
    SharedModule,
    WorkItemTypeRoutingModule,
    SweetAlert2Module.forChild()
  ]
})
export class WorkItemTypeModule { }
