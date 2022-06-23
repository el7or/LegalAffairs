import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { SubWorkItemTypeRoutingModule } from './sub-work-item-type-routing.module';
import { SubWorkItemTypeFormComponent } from './sub-work-item-type-form/sub-work-item-type-form.component';
import { SubWorkItemTypeListComponent } from './sub-work-item-type-list/sub-work-item-type-list.component';
import { SharedModule } from 'app/shared/shared.module';


@NgModule({
  declarations: [SubWorkItemTypeFormComponent, SubWorkItemTypeListComponent],
  imports: [
    SharedModule,
    SubWorkItemTypeRoutingModule,
    SweetAlert2Module.forChild()
  ]
})
export class SubWorkItemTypeModule { }
